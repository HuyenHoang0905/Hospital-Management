#if FRAMEWORK20
using System;
using System.Collections.Generic;
using DevComponents.Schedule.Model;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Schedule
{
    internal class ModelMonthViewConnector : ModelViewConnector
    {
        #region Constants

        private const int DaysInWeek = 7;

        #endregion

        #region Private variables

        private CalendarModel _Model;       // The associated CalendarModel
        private MonthView _View;            // The associated MonthView

        private bool _IsConnected;          // Connection status

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Assoc CalendarModel</param>
        /// <param name="monthView">Assoc MonthView</param>
        public ModelMonthViewConnector(CalendarModel model, MonthView monthView)
        {
            _Model = model;
            _View = monthView;
        }

        #region Public properties

        /// <summary>
        /// Gets the connection status
        /// </summary>
        public override bool IsConnected
        {
            get { return _IsConnected; }
        }

        #endregion

        #region Connect processing

        /// <summary>
        /// Performs Model connection processing
        /// </summary>
        public override void Connect()
        {
            VerifyModel();

            // Load the connection data

            if (_IsConnected)
                Disconnect();

            LoadData();

            // Get notification on Model property changes

            _Model.PropertyChanged += ModelPropertyChanged;
            _Model.SubPropertyChanged += ModelSubPropertyChanged;

            _View.CalendarView.CustomItems.CollectionChanged += CustomItemsCollectionChanged;

            _IsConnected = true;
        }

        #endregion

        #region Disconnect processing

        /// <summary>
        /// Severs the Model/MonthView connection
        /// </summary>
        public override void Disconnect()
        {
            VerifyModel();

            if (_IsConnected)
            {
                // Loop through each week, clearing each
                // associated view connection

                for (int i = 0; i < _View.MonthWeeks.Length; i++)
                    ClearMonthWeek(_View.MonthWeeks[i]);

                _View.SubItems.Clear();

                // Stop notification on Model property changes

                _Model.PropertyChanged -= ModelPropertyChanged;
                _Model.SubPropertyChanged -= ModelSubPropertyChanged;

                _View.CalendarView.CustomItems.CollectionChanged -= CustomItemsCollectionChanged;

                _IsConnected = false;
            }
        }

        /// <summary>
        /// Clears individual MonthWeek view connections
        /// </summary>
        /// <param name="monthWeek">MonthWeek</param>
        private void ClearMonthWeek(MonthWeek monthWeek)
        {
            if (monthWeek.CalendarItems.Count > 0)
            {
                // Loop through each CalendarItem, resetting
                // it's associated connection

                for (int i = monthWeek.CalendarItems.Count - 1; i >= 0; i--)
                {
                    AppointmentMonthView view =
                        monthWeek.CalendarItems[i] as AppointmentMonthView;

                    if (view != null)
                    {
                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;

                        view.Appointment = null;
                        view.IsSelected = false;
                        view.MonthWeek = null;
                    }

                    monthWeek.CalendarItems.RemoveAt(i);
                }
            }
        }

        #endregion

        #region LoadData processing

        /// <summary>
        /// Loads Model/MonthView connection data
        /// </summary>
        private void LoadData()
        {
            List<Appointment> appts = new List<Appointment>();

            for (int i = 0; i < _View.MonthWeeks.Length; i++)
            {
                // Accumulate Model appointments for the week

                appts.Clear();

                DateTime date = _View.MonthWeeks[i].FirstDayOfWeek;

                for (int j = 0; j < DaysInWeek; j++)
                {
                    AppointmentSubsetCollection asc = _Model.GetDay(date).Appointments;

                    if (asc.Count > 0)
                        appts.AddRange(asc);

                    date = date.AddDays(1);
                }

                // Remove multi-day duplicates

                appts = RemoveDuplicates(appts);

                // Update the MonthView

                UpdateWeekView(_View.MonthWeeks[i], appts, null);
                UpdateCustomItems(_View.MonthWeeks[i], null);
            }
        }

        #endregion

        #region RefreshData processing

        /// <summary>
        /// Refreshes the data in a previously established
        /// and loaded connection
        /// </summary>
        public void RefreshData()
        {
            List<List<Appointment>> weeklyAppointments = new List<List<Appointment>>(_View.MonthWeeks.Length);
            List<AppointmentMonthView> removedViews = new List<AppointmentMonthView>();
            List<CustomCalendarItem> removedItems = new List<CustomCalendarItem>();

            foreach (MonthWeek monthWeek in _View.MonthWeeks)
            {
                List<Appointment> appointments = new List<Appointment>();
                DateTime date = monthWeek.FirstDayOfWeek;

                for (int i = 0; i < DaysInWeek; i++)
                {
                    Day day = _Model.GetDay(date);
                    appointments.AddRange(day.Appointments);
                    date = date.AddDays(1);
                }

                // Remove duplicate multi-day appointments and add
                // then to our weekly appointments list

                appointments = RemoveDuplicates(appointments);
                weeklyAppointments.Add(appointments);

                // Process and record any out-dated views

                RemoveOutdatedViews(ref removedViews, monthWeek, appointments);
                RemoveOutdatedCustomItems(ref removedItems, monthWeek);
            }

            // Process each week using our accumulated info

            for (int i = 0; i < _View.MonthWeeks.Length; i++)
            {
                UpdateWeekView(_View.MonthWeeks[i], weeklyAppointments[i], removedViews);
                UpdateCustomItems(_View.MonthWeeks[i], removedItems);
            }

            // Process remaining removed data

            ProcessRemovedData(removedViews, removedItems);
        }

        #region ProcessRemovedData

        /// <summary>
        /// Process any remaining removed data
        /// </summary>
        /// <param name="removedViews"></param>
        /// <param name="removedItems"></param>
        private void ProcessRemovedData(List<AppointmentMonthView> removedViews,
            List<CustomCalendarItem> removedItems)
        {
            // Process any remaining removedViews

            if (removedViews != null && removedViews.Count > 0)
            {
                for (int i = 0; i < removedViews.Count; i++)
                    removedViews[i].Dispose();
            }

            // Process any remaining removed items

            if (removedItems != null && removedItems.Count > 0)
            {
                for (int i = 0; i < removedItems.Count; i++)
                    removedItems[i].Dispose();

                _View.NeedRecalcLayout = true;
            }
        }

        #endregion

        #endregion

        #region UpdateWeekView processing

        /// <summary>
        /// Updates individual MonthWeek views
        /// </summary>
        /// <param name="monthWeek">MonthWeek</param>
        /// <param name="appointments">List of appointments</param>
        /// <param name="cachedViews">List of cached views</param>
        private void UpdateWeekView(MonthWeek monthWeek,
            List<Appointment> appointments, List<AppointmentMonthView> cachedViews)
        {
            // Loop through each appointment
            // updating the assoc view accordingly

            foreach (Appointment appointment in  appointments)
            {
                if (IsAppointmentVisible(appointment))
                {
                    // Get the assoc view

                    AppointmentMonthView view = (GetViewFromWeek(monthWeek, appointment) ??
                                                 GetViewFromCache(appointment, cachedViews)) ??
                                                 GetNewView(appointment);

                    // Set the view start and end times to
                    // match the assoc appointment

                    view.StartTime = appointment.StartTime;
                    view.EndTime = appointment.EndTime;

                    // And update the MonthWeek data

                    if (view.MonthWeek == null)
                    {
                        view.MonthWeek = monthWeek;

                        monthWeek.CalendarItems.Add(view);
                        _View.SubItems.Add(view);

                        view.IsSelectedChanged += _View.ItemIsSelectedChanged;
                    }
                }
            }
        }

        #endregion

        #region UpdateCustomItems

        private void UpdateCustomItems(
            MonthWeek monthWeek, List<CustomCalendarItem> cachedItems)
        {
            CustomCalendarItemCollection items = _View.CalendarView.CustomItems;

            if (items != null && items.Count > 0)
            {
                DateTime startTime = monthWeek.FirstDayOfWeek;
                DateTime endTime = monthWeek.FirstDayOfWeek.AddDays(7);

                for (int i = 0; i < items.Count; i++)
                {
                    CustomCalendarItem item = items[i];

                    if (IsCustomItemVisible(item) == true &&
                        (item.StartTime < endTime && item.EndTime > startTime))
                    {
                        item.CalendarView = _View.CalendarView;

                        CustomCalendarItem ci = GetItemFromWeek(monthWeek, item);

                        if (ci == null)
                        {
                            ci = GetItemFromCache(item, cachedItems) ??
                                 GetNewCustomItem(item);

                            monthWeek.CalendarItems.Add(ci);
                            _View.SubItems.Insert(0, ci);

                            ci.IsSelectedChanged += _View.ItemIsSelectedChanged;
                        }

                        if (ci.BaseCalendarItem != item)
                        {
                            ci.StartTime = item.StartTime;
                            ci.EndTime = item.EndTime;
                        }
                    }
                }
            }
        }

        #endregion

        #region GetViewFromView

        /// <summary>
        /// Gets all appointment AppointmentMonthViews
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private List<AppointmentMonthView> GetViewsFromAll(Appointment app)
        {
            List<AppointmentMonthView> list = new List<AppointmentMonthView>();

            for (int i = 0; i < _View.MonthWeeks.Length; i++)
            {
                AppointmentMonthView view =
                    GetViewFromWeek(_View.MonthWeeks[i], app);

                if (view != null)
                    list.Add(view);
            }

            return (list);
        }

        /// <summary>
        /// Gets the view from the MonthWeek list
        /// </summary>
        /// <param name="monthWeek">MonthWeek</param>
        /// <param name="appointment">Appointment</param>
        /// <returns>Appointment view</returns>
        private AppointmentMonthView GetViewFromWeek(MonthWeek monthWeek, Appointment appointment)
        {
            foreach (CalendarItem item in monthWeek.CalendarItems)
            {
                AppointmentMonthView view = item as AppointmentMonthView;

                if (view != null && view.Appointment == appointment)
                    return (view);
            }

            return (null);
        }

        /// <summary>
        /// Gets the view from the cached list
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <param name="cachedViews">Cached views</param>
        /// <returns>Appointment view</returns>
        private AppointmentMonthView GetViewFromCache(Appointment appointment,
            List<AppointmentMonthView> cachedViews)
        {
            if (cachedViews != null && cachedViews.Count > 0)
            {
                foreach (AppointmentMonthView appView in cachedViews)
                {
                    if (appView.Appointment == appointment)
                    {
                        cachedViews.Remove(appView);

                        return (appView);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetItemFromWeek

        private CustomCalendarItem GetItemFromWeek(MonthWeek monthWeek, CustomCalendarItem item)
        {
            foreach (CalendarItem citem in monthWeek.CalendarItems)
            {
                CustomCalendarItem view = citem as CustomCalendarItem;

                if (view != null && (view == item || view.BaseCalendarItem == item))
                    return (view);
            }

            return (null);
        }

        #endregion

        #region GetItemFromCache

        private CustomCalendarItem GetItemFromCache(
            CustomCalendarItem item, List<CustomCalendarItem> cachedItems)
        {
            if (cachedItems != null && cachedItems.Count > 0)
            {
                foreach (CustomCalendarItem view in cachedItems)
                {
                    if (view == item || view.BaseCalendarItem == item)
                    {
                        cachedItems.Remove(view);

                        return (view);
                    }
                }
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
        private AppointmentMonthView GetNewView(Appointment appointment)
        {
            AppointmentMonthView view = new AppointmentMonthView(_View, appointment);

            view.Tooltip = appointment.Tooltip;

            // Set the selected state to match any current
            // views that are present for this appointment

            for (int i = 0; i < _View.MonthWeeks.Length; i++)
            {
                AppointmentMonthView viewPart =
                    GetViewFromWeek(_View.MonthWeeks[i], appointment);

                if (viewPart != null)
                {
                    view.IsSelected = viewPart.IsSelected;
                    break;
                }
            }

            return (view);
        }

        #endregion

        #region GetNewCustomItem

        /// <summary>
        /// Gets a new CustomItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private CustomCalendarItem GetNewCustomItem(CustomCalendarItem item)
        {
            CustomCalendarItem ci = (CustomCalendarItem)item.Copy();

            ci.BaseCalendarItem = item;
            ci.ModelItem = item;

            // Set the selected state to match any current
            // views that are present for this appointment

            for (int i = 0; i < _View.MonthWeeks.Length; i++)
            {
                CustomCalendarItem viewPart =
                    GetItemFromWeek(_View.MonthWeeks[i], item);

                if (viewPart != null)
                {
                    ci.IsSelected = viewPart.IsSelected;
                    break;
                }
            }

            return (ci);
        }

        #endregion

        #region RemoveDuplicates

        /// <summary>
        /// Removes duplicate multi-day appointments
        /// </summary>
        /// <param name="appointments"></param>
        /// <returns>Trimmed list</returns>
        private List<Appointment> RemoveDuplicates(List<Appointment> appointments)
        {
            List<Appointment> a = new List<Appointment>();

            foreach (Appointment app in appointments)
            {
                if (app.Visible == true)
                {
                    if (a.Contains(app) == false)
                        a.Add(app);
                }
            }

            return (a);
        }

        #endregion

        #region RemoveOutdatedViews

        /// <summary>
        /// Removes out-dated views
        /// </summary>
        /// <param name="removedViews"></param>
        /// <param name="monthWeek"></param>
        /// <param name="appointments"></param>
        /// <returns></returns>
        private void RemoveOutdatedViews(ref List<AppointmentMonthView> removedViews,
            MonthWeek monthWeek, List<Appointment> appointments)
        {
            List<AppointmentMonthView> removeList = null;

            for (int i = monthWeek.CalendarItems.Count - 1; i >= 0; i--)
            {
                AppointmentMonthView view =
                    monthWeek.CalendarItems[i] as AppointmentMonthView;

                if (view != null)
                {
                    if (!appointments.Contains(view.Appointment) || !IsAppointmentVisible(view.Appointment))
                    {
                        if (removeList == null)
                            removeList = new List<AppointmentMonthView>();

                        removeList.Add(view);
                        _View.SubItems._Remove(view);
                        view.MonthWeek = null;

                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;

                        monthWeek.CalendarItems.RemoveAt(i);
                    }
                }
            }

            if (removeList != null)
            {
                if (removedViews == null)
                    removedViews = new List<AppointmentMonthView>();

                removedViews.AddRange(removeList);
            }
        }

        #endregion

        #region RemoveOutdatedCustomItems

        private void RemoveOutdatedCustomItems(
            ref List<CustomCalendarItem> removedViews, MonthWeek monthWeek)
        {
            List<CustomCalendarItem> removeList = null;

            DateTime startTime = monthWeek.FirstDayOfWeek;
            DateTime endTime = monthWeek.FirstDayOfWeek.AddDays(7);

            for (int i = monthWeek.CalendarItems.Count - 1; i >= 0; i--)
            {
                CustomCalendarItem view =
                    monthWeek.CalendarItems[i] as CustomCalendarItem;

                if (view != null)
                {
                    if (IsValidItem(view) == false ||
                        (view.EndTime < startTime || view.StartTime >= endTime))
                    {
                        if (removeList == null)
                            removeList = new List<CustomCalendarItem>();

                        removeList.Add(view);
                        _View.SubItems._Remove(view);

                        _View.NeedRecalcSize = true;

                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;

                        monthWeek.CalendarItems.RemoveAt(i);
                    }
                }
            }

            if (removeList != null)
            {
                if (removedViews == null)
                    removedViews = new List<CustomCalendarItem>();

                removedViews.AddRange(removeList);
            }
        }

        private bool IsValidItem(CustomCalendarItem view)
        {
            return (IsCustomItemVisible(view) == true &&
                _View.CalendarView.CustomItems.Contains(view.BaseCalendarItem) == true);
        }

        #endregion

        #region GetView

        /// <summary>
        /// Returns the Month view
        /// </summary>
        /// <returns></returns>
        public override eCalendarView GetView()
        {
            return (eCalendarView.Month);
        }

        /// <summary>
        /// Verifies the Model and MonthView are valid
        /// </summary>
        private void VerifyModel()
        {
            if (_Model == null)
                throw new NullReferenceException("CalendarModel must be set on connector.");

            if (_View == null)
                throw new NullReferenceException("MonthCalendarView must be set on connector.");
        }

        #endregion

        #region ModelPropertyChanged

        /// <summary>
        /// Handles Model property change notifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CalendarModel.AppointmentsPropertyName)
            {
                RefreshData();

                _View.NeedRecalcSize = true;
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
            if (e.Source is Owner && e.PropertyChangedArgs.PropertyName == Owner.DisplayNamePropertyName)
            {
                Owner owner = (Owner)e.Source;

                if (_View.OwnerKey != null && _View.OwnerKey.Equals(owner.Key))
                    _View.DisplayName = owner.DisplayName;
            }
            else if (e.Source is Appointment)
            {
                Appointment app = e.Source as Appointment;
                List<AppointmentMonthView> list;

                string name = e.PropertyChangedArgs.PropertyName;

                if (name.Equals("Tooltip"))
                {
                    list = GetViewsFromAll(app);

                    for (int i = 0; i < list.Count; i++)
                        list[i].Tooltip = app.Tooltip;
                }
                else if (name.Equals("IsSelected"))
                {
                    list = GetViewsFromAll(app);

                    for (int i = 0; i < list.Count; i++)
                        list[i].IsSelected = app.IsSelected;
                }
                else if (name.Equals("CategoryColor") || name.Equals("TimeMarkedAs"))
                {
                    list = GetViewsFromAll(app);

                    for (int i = 0; i < list.Count; i++)
                        list[i].Refresh();
                }
                else if (name.Equals("OwnerKey"))
                {
                    if (_View.CalendarView.IsMultiCalendar == true)
                    {
                        if (_View.OwnerKey == app.OwnerKey)
                        {
                            RefreshData();

                            _View.NeedRecalcSize = true;
                            _View.Refresh();
                        }
                        else
                        {
                            list = GetViewsFromAll(app);

                            if (list.Count > 0)
                            {
                                RefreshData();

                                _View.NeedRecalcSize = true;
                                _View.Refresh();
                            }
                        }
                    }
                }
                else if (name.Equals("Visible"))
                {
                    RefreshData();
                }
            }
        }

        #endregion

        #region CustomItems_CollectionChanged

        void CustomItemsCollectionChanged(object sender, EventArgs e)
        {
            RefreshData();

            _View.NeedRecalcSize = true;
            _View.Refresh();
        }

        #endregion
    }
}
#endif

