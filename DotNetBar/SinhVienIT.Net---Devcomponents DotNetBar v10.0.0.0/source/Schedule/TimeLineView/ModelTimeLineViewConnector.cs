#if FRAMEWORK20
using System;
using System.Collections.Generic;
using DevComponents.Schedule.Model;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Schedule
{
    internal class ModelTimeLineViewConnector : ModelViewConnector
    {
        #region Const

        private const int DaysInWeek = 7;

        #endregion

        #region Static data

        static private AppointmentSubsetCollection _lineAppts;

        static private DateTime _lineStartTime;     // TimeLine start date
        static private DateTime _lineEndTime;       // TimeLine end date
        static private int _lineState;              // Refresh state

        static private List<Appointment> _periodAppts;

        static private DateTime _periodStartTime;   // Period start date
        static private DateTime _periodEndTime;     // Period end date

        #endregion

        #region Private variables

        private CalendarModel _Model;       // The associated CalendarModel
        private TimeLineView _View;         // The associated _TimeLineView

        private DateTime _ViewStartTime;    // View start time
        private DateTime _ViewEndTime;      // View end time
        private int _ViewState;             // View refresh state

        private DayInfo[] _DayInfo;         // DayInfo array (WorkStartTimes)

        private bool _IsConnected;          // Connection status

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Assoc CalendarModel</param>
        /// <param name="timeLineView">Assoc TimeLineView</param>
        public ModelTimeLineViewConnector(CalendarModel model, TimeLineView timeLineView)
        {
            _Model = model;
            _View = timeLineView;
        }

        #region Public properties

        /// <summary>
        /// Gets the connection status
        /// </summary>
        public override bool IsConnected
        {
            get { return (_IsConnected); }
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        public AppointmentSubsetCollection Appts
        {
            get { return (_lineAppts); }
        }

        /// <summary>
        /// Gets the DayInfo array
        /// </summary>
        public DayInfo[] DayInfo
        {
            get { return (_DayInfo); }
        }

        #endregion

        #region Connect processing

        /// <summary>
        /// Performs Model connection processing
        /// </summary>
        public override void Connect()
        {
            VerifyModel();

            if (_IsConnected)
                Disconnect();

            LoadData();

            // Get notification on Model property changes

            HookEvents(true);

            _IsConnected = true;
        }

        #endregion

        #region Hook events

        /// <summary>
        /// Hooks or unhooks our system events
        /// </summary>
        /// <param name="hook"></param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _Model.PropertyChanged += ModelPropertyChanged;
                _Model.SubPropertyChanged += ModelSubPropertyChanged;

                _View.CalendarView.CustomItems.CollectionChanged += CustomItemsCollectionChanged;
            }
            else
            {
                _Model.PropertyChanged -= ModelPropertyChanged;
                _Model.SubPropertyChanged -= ModelSubPropertyChanged;

                _View.CalendarView.CustomItems.CollectionChanged -= CustomItemsCollectionChanged;
            }
        }

        #endregion

        #region Event processing

        #region Model property change processing

        /// <summary>
        /// Handles Model property change notifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CalendarModel.AppointmentsPropertyName)
            {
                _ViewStartTime = new DateTime();

                RefreshData(_ViewState == _lineState);

                _View.NeedRecalcLayout = true;
                _View.NeedRecalcSize = true;
            }
            else if (e.PropertyName == CalendarModel.WorkDaysPropertyName)
            {
                UpdateWorkDayDetails();

                _View.Refresh();
            }
        }

        #endregion

        #region ModelSubPropertyChanged

        /// <summary>
        /// Handles ModelSubProperty change notifications
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SubPropertyChangedEventArgs</param>
        private void ModelSubPropertyChanged(object sender, SubPropertyChangedEventArgs e)
        {
            if (e.Source is WorkDay)
            {
                UpdateWorkDayDetails();
            }
            else if (e.Source is Owner && e.PropertyChangedArgs.PropertyName == Owner.DisplayNamePropertyName)
            {
                Owner owner = (Owner)e.Source;

                if (_View.OwnerKey != null && _View.OwnerKey.Equals(owner.Key))
                    _View.DisplayName = owner.DisplayName;
            }
            else if (e.Source is Appointment)
            {
                Appointment app = e.Source as Appointment;
                AppointmentTimeLineView appView;

                string name = e.PropertyChangedArgs.PropertyName;

                if (name.Equals("Tooltip"))
                {
                    appView = GetViewFromTimeLine(app);

                    if (appView != null)
                        appView.Tooltip = app.Tooltip;
                }
                else if (name.Equals("IsSelected"))
                {
                    appView = GetViewFromTimeLine(app);

                    if (appView != null)
                        appView.IsSelected = app.IsSelected;
                }
                else if (name.Equals("CategoryColor") || name.Equals("TimeMarkedAs"))
                {
                    appView = GetViewFromTimeLine(app);

                    if (appView != null)
                        appView.Refresh();
                }
                else if (name.Equals("OwnerKey"))
                {
                    if (_View.CalendarView.IsMultiCalendar == true)
                    {
                        if (_View.OwnerKey == app.OwnerKey)
                        {
                            _ViewStartTime = new DateTime();

                            RefreshData(false);

                            _View.NeedRecalcLayout = true;
                            _View.RecalcSize();
                        }
                        else
                        {
                            appView = GetViewFromTimeLine(app);

                            if (appView != null)
                            {
                                _ViewStartTime = new DateTime();

                                RefreshData(false);

                                _View.NeedRecalcLayout = true;
                                _View.RecalcSize();
                            }
                        }
                    }
                }
                else if (name.Equals("Visible"))
                {
                    RefreshData(true);
                }
            }
        }

        #endregion

        #region CustomItems_CollectionChanged

        /// <summary>
        /// Handles CustomItemCollection change events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CustomItemsCollectionChanged(object sender, EventArgs e)
        {
            _ViewStartTime = new DateTime();

            RefreshData(_ViewState == _lineState);

            _View.NeedRecalcLayout = true;
            _View.NeedRecalcSize = true;
        }

        #endregion

        #endregion

        #region Disconnect processing

        /// <summary>
        /// Severs the Model/TimeLineView connection
        /// </summary>
        public override void Disconnect()
        {
            VerifyModel();

            if (_IsConnected)
            {
                _IsConnected = false;
                _lineState = 0;

                // Clear our TimeLine items and
                // stop notification on Model property changes

                ClearTimeLineItems();

                HookEvents(false);
            }
        }

        /// <summary>
        /// Clears TimeLine view items
        /// </summary>
        private void ClearTimeLineItems()
        {
            if (_View.CalendarItems.Count > 0)
            {
                // Loop through each CalendarItem, resetting
                // it's associated connection

                for (int i = _View.CalendarItems.Count - 1; i >= 0; i--)
                {
                    AppointmentTimeLineView view =
                        _View.CalendarItems[i] as AppointmentTimeLineView;

                    if (view != null)
                    {
                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;

                        view.Appointment = null;
                        view.IsSelected = false;
                    }

                    _View.CalendarItems.RemoveAt(i);
                }
            }
                
            _View.SubItems.Clear();
        }

        #endregion

        #region LoadData processing

        /// <summary>
        /// Loads Model/TimeLineView connection data
        /// </summary>
        private void LoadData()
        {
            LoadViewData(true, false);

            UpdateWorkDayDetails();
        }

        #endregion

        #region LoadViewData

        /// <summary>
        /// Loads the view data
        /// </summary>
        /// <param name="reload">Forceful reload</param>
        /// <param name="validate">Validation needed</param>
        private void LoadViewData(bool reload, bool validate)
        {
            DateTime startTime, endTime;
            GetDateRange(out startTime, out endTime);

            reload = LoadPeriodData(reload, startTime, endTime);

            if (reload == true ||
                _ViewStartTime != startTime || _ViewEndTime != endTime)
            {
                _ViewStartTime = startTime;
                _ViewEndTime = endTime;

                if (validate == true)
                {
                    RemoveOutdatedViews(_periodAppts);
                    RemoveOutdatedCustomItems();
                }

                UpdateTimeLineView(_periodAppts);
                UpdateCustomItems();
            }
        }

        #endregion

        #region LoadPeriodData

        /// <summary>
        /// Loads the Period data (visible view range)
        /// </summary>
        /// <param name="reload">Forceful reload</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>reload flag</returns>
        private bool LoadPeriodData(bool reload, DateTime startTime, DateTime endTime)
        {
            reload = LoadLineData(reload);

            if (reload == true ||
                _periodStartTime != startTime || _periodEndTime != endTime)
            {
                _periodStartTime = startTime;
                _periodEndTime = endTime;

                _periodAppts = new List<Appointment>();

                for (int i=0; i<_lineAppts.Count; i++)
                {
                    if (_lineAppts[i].EndTime > _periodStartTime && _lineAppts[i].StartTime < _periodEndTime)
                        _periodAppts.Add(_lineAppts[i]);
                }

                reload = true;
            }

            return (reload);
        }

        #endregion

        #region LoadLineData

        /// <summary>
        /// Loads the TimeLine appointment data
        /// </summary>
        /// <param name="reload"></param>
        private bool LoadLineData(bool reload)
        {
            if (reload == true ||
                _lineStartTime != _View.StartDate || _lineEndTime != _View.EndDate)
            {
                _lineStartTime = _View.StartDate;
                _lineEndTime = _View.EndDate;

                _lineAppts = new AppointmentSubsetCollection(_Model, _lineStartTime, _lineEndTime);

                _lineState = _lineState ^ 1;
            }

            if (_ViewState != _lineState)
            {
                _ViewState = _lineState;

                 _View.ModelReloaded = true;

                 reload = true;
            }

            return (reload);
        }

        #endregion

        #region RefreshData processing

        /// <summary>
        /// Refreshes the data in a previously established
        /// and loaded connection
        /// </summary>
        public void RefreshData(bool reload)
        {
            if (_View.Displayed == true)
                LoadViewData(reload, true);
        }

        #endregion

        #region GetDateRange

        /// <summary>
        /// Gets the range of appointment dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void GetDateRange(out DateTime startDate, out DateTime endDate)
        {
            int scol = _View.FirstVisibleColumn;
            int ncols = _View.ClientRect.Width / _View.ColumnWidth;

            startDate = _View.StartDate;
            endDate = startDate;

            try
            {
                startDate = startDate.AddMinutes(scol * _View.CalendarView.BaseInterval);
                endDate = startDate.AddMinutes(ncols * _View.CalendarView.BaseInterval);

                startDate = startDate.AddMinutes(-_View.CalendarView.BaseInterval);
                endDate = endDate.AddMinutes(_View.CalendarView.BaseInterval);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        #endregion

        #region UpdateTimeLineView

        /// <summary>
        /// Updates the TimeLine view
        /// </summary>
        /// <param name="appointments"></param>
        private void UpdateTimeLineView(List<Appointment> appointments)
        {
            // Loop through each appointment
            // updating the assoc view accordingly

            foreach (Appointment appointment in appointments)
            {
                if (IsAppointmentVisible(appointment))
                {
                    // Get the assoc view

                    AppointmentTimeLineView view =
                        GetViewFromTimeLine(appointment) ?? GetNewView(appointment);

                    // Set the view start and end times to
                    // match the assoc appointment

                    view.StartTime = appointment.StartTime;
                    view.EndTime = appointment.EndTime;

                    // Update the item data

                    if (view.TimeLineView == null)
                    {
                        view.TimeLineView = _View;

                        _View.CalendarItems.Add(view);
                        _View.SubItems.Add(view);

                        view.IsSelectedChanged += _View.ItemIsSelectedChanged;
                    }
                }
            }
        }

        #endregion

        #region UpdateCustomItems

        /// <summary>
        /// Updates the TimeLine CustomItems
        /// </summary>
        private void UpdateCustomItems()
        {
            CustomCalendarItemCollection items = _View.CalendarView.CustomItems;

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    CustomCalendarItem item = items[i];

                    if (IsCustomItemVisible(item) == true &&
                        (item.StartTime < _ViewEndTime && item.EndTime > _ViewStartTime))
                    {
                        item.CalendarView = _View.CalendarView;

                        CustomCalendarItem ci = 
                            GetItemFromTimeLine(item) ?? GetNewCustomItem(item);

                        if (ci.StartTime != item.StartTime || ci.EndTime != item.EndTime)
                        {
                            ci.StartTime = item.StartTime;
                            ci.EndTime = item.EndTime;
                        }
                    }
                }
            }
        }

        #endregion

        #region UpdateWorkDayDetails

        /// <summary>
        /// Updates the WorkDay details array
        /// </summary>
        private void UpdateWorkDayDetails()
        {
            // Update workDay timings

            if (_DayInfo == null)
                _DayInfo = new DayInfo[DaysInWeek];

            for (int i = 0; i < DaysInWeek; i++)
            {
                if (_DayInfo[i] == null)
                    _DayInfo[i] = new DayInfo();

                Owner owner = _Model.Owners[_View.OwnerKey];

                WorkDay workDay = (owner != null && owner.WorkDays.Count > 0)
                    ? owner.WorkDays[(DayOfWeek)i] : _Model.WorkDays[(DayOfWeek)i];

                if (workDay != null)
                {
                    _DayInfo[i].WorkStartTime = workDay.WorkStartTime;
                    _DayInfo[i].WorkEndTime = workDay.WorkEndTime;
                }
                else
                {
                    _DayInfo[i].WorkStartTime = new WorkTime();
                    _DayInfo[i].WorkEndTime = new WorkTime();
                }
            }
        }

        #endregion

        #region RemoveOutdatedViews

        /// <summary>
        /// Removes Outdated Views
        /// </summary>
        /// <param name="appts"></param>
        private void RemoveOutdatedViews(List<Appointment> appts)
        {
            for (int i=_View.CalendarItems.Count - 1; i>=0; i--)
            {
                AppointmentTimeLineView view =
                    _View.CalendarItems[i] as AppointmentTimeLineView;

                if (view != null)
                {
                    if (ValidViewAppointment(appts, view) == false)
                    {
                        view.TimeLineView = null;

                        _View.NeedRecalcLayout = true;

                        _View.SubItems._Remove(view);
                        _View.CalendarItems.RemoveAt(i);

                        if (view == _View.SelectedItem)
                            _View.SelectedItem = null;

                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the provided view is valid, given
        /// the current list of Appointments
        /// </summary>
        /// <param name="appts"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool ValidViewAppointment(
            List<Appointment> appts, AppointmentTimeLineView view)
        {
            if (IsAppointmentVisible(view.Appointment) == false)
                return (false);

            if (view.IsSelected == true)
            {
                if (_Model.Appointments.Contains(view.Appointment) == true)
                    return (true);
            }

            return (appts.Contains(view.Appointment));
        }

        #endregion

        #region RemoveOutdatedCustomItems

        /// <summary>
        /// Removes out dated CustomItems
        /// </summary>
        private void RemoveOutdatedCustomItems()
        {
            for (int i = _View.CalendarItems.Count - 1; i >= 0; i--)
            {
                CustomCalendarItem item = _View.CalendarItems[i] as CustomCalendarItem;

                if (item != null)
                {
                    if (IsValidItem(item) == false ||
                       (item.IsSelected == false && (item.EndTime < _ViewStartTime || item.StartTime > _ViewEndTime)))
                    {
                        _View.NeedRecalcSize = true;
                        _View.SubItems._Remove(item);
                        _View.CalendarItems.RemoveAt(i);

                        if (item == _View.SelectedItem)
                            _View.SelectedItem = null;

                        item.IsSelectedChanged -= _View.ItemIsSelectedChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the given CustomItem is valid
        /// for the current view
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool IsValidItem(CustomCalendarItem view)
        {
            return (IsCustomItemVisible(view) == true &&
                _View.CalendarView.CustomItems.Contains(view.BaseCalendarItem) == true);
        }

        #endregion

        #region GetViewFromTimeLine

        /// <summary>
        /// Gets the AppointmentView from the timeline
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>AppointmentView or null</returns>
        private AppointmentTimeLineView GetViewFromTimeLine(Appointment appointment)
        {
            foreach (CalendarItem item in _View.CalendarItems)
            {
                AppointmentTimeLineView view = item as AppointmentTimeLineView;

                if (view != null && view.Appointment == appointment)
                    return (view);
            }

            return (null);
        }

        #endregion

        #region GetItemFromTimeLine

        /// <summary>
        /// Gets the CustomCalendarItem from the timeline.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>CustomCalendarItem or null</returns>
        private CustomCalendarItem GetItemFromTimeLine(CustomCalendarItem item)
        {
            foreach (CalendarItem citem in _View.CalendarItems)
            {
                CustomCalendarItem view = citem as CustomCalendarItem;

                if (view != null && (view == item || view.BaseCalendarItem == item))
                    return (view);
            }

            return (null);
        }

        #endregion

        #region GetNewView

        /// <summary>
        /// Gets a new appointment view
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <returns>New view</returns>
        private AppointmentTimeLineView GetNewView(Appointment appointment)
        {
            AppointmentTimeLineView view = new AppointmentTimeLineView(_View, appointment);

            view.Tooltip = appointment.Tooltip;

            return (view);
        }

        #endregion

        #region GetNewCustomItem

        /// <summary>
        /// Gets a new CustomCalendarItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns>CustomCalendarItem</returns>
        private CustomCalendarItem GetNewCustomItem(CustomCalendarItem item)
        {
            CustomCalendarItem ci = (CustomCalendarItem)item.Copy();
            ci.BaseCalendarItem = item;

            _View.CalendarItems.Add(ci);
            _View.NeedRecalcLayout = true;

            _View.SubItems.Add(ci);

            ci.IsSelectedChanged += _View.ItemIsSelectedChanged;

            return (ci);
        }

        #endregion

        #region View support routines

        /// <summary>
        /// Returns the view
        /// </summary>
        /// <returns></returns>
        public override eCalendarView GetView()
        {
            return (eCalendarView.TimeLine);
        }

        /// <summary>
        /// Verifies the Model and MonthView are valid
        /// </summary>
        private void VerifyModel()
        {
            if (_Model == null)
                throw new NullReferenceException("CalendarModel must be set on connector.");

            if (_View == null)
                throw new NullReferenceException("AppointmentTimeLineView must be set on connector.");
        }

        #endregion

    }
}
#endif

