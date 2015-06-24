#if FRAMEWORK20
using System;
using DevComponents.Schedule.Model;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Schedule
{
    internal class ModelYearViewConnector : ModelViewConnector
    {
        #region Static data

        static private AppointmentSubsetCollection _lineAppts;

        static private int _lineState;              // Refresh state
        static private DateTime _lineStartTime;     // YearView start date
        static private DateTime _lineEndTime;       // YearView end date

        #endregion

        #region Private variables

        private CalendarModel _Model;       // The associated CalendarModel
        private YearView _View;             // The associated YearView

        private bool _IsConnected;          // Connection status
        private int _ViewState;             // View refresh state

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Assoc CalendarModel</param>
        /// <param name="yearView">Assoc YearView</param>
        public ModelYearViewConnector(CalendarModel model, YearView yearView)
        {
            _Model = model;
            _View = yearView;
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

        #region Internal properties

        internal int ViewState
        {
            get { return (_ViewState); }
        }

        #endregion

        #region Connect processing

        /// <summary>
        /// Performs Model connection processing
        /// </summary>
        public override void Connect()
        {
            // Load the connection data

            if (_IsConnected)
                Disconnect();

            LoadData(_ViewState == _lineState);

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
            if (_IsConnected)
            {
                // Loop through each week, clearing each
                // associated view connection

                for (int i = 0; i < _View.YearMonths.Length; i++)
                    _View.YearMonths[i].AppBits.SetAll(false);

                // Stop notification on Model property changes

                _Model.PropertyChanged -= ModelPropertyChanged;
                _Model.SubPropertyChanged -= ModelSubPropertyChanged;
                _View.CalendarView.CustomItems.CollectionChanged -= CustomItemsCollectionChanged;

                _IsConnected = false;
            }
        }

        #endregion

        #region LoadData processing

        /// <summary>
        /// Loads Model/YearView connection data
        /// </summary>
        private void LoadData(bool reload)
        {
            if (reload == true ||
                _lineStartTime != _View.StartDate || _lineEndTime != _View.EndDate)
            {
                _lineStartTime = _View.StartDate;
                _lineEndTime = _View.EndDate;

                _lineAppts = new AppointmentSubsetCollection(_Model, _View.StartDate, _View.EndDate.AddDays(1));
            }

            if (reload == true)
                _lineState = _lineState ^ 1;

            _ViewState = _lineState;

            foreach (YearMonth yearMonth in _View.YearMonths)
                yearMonth.AppBits.SetAll(false);

            int rootMonth = _View.StartDate.Year * 12 + _View.StartDate.Month;

            for (int i = 0; i < _lineAppts.Count; i++)
            {
                Appointment app = _lineAppts[i];

                if (IsAppointmentVisible(app) == true)
                {
                    int month = (app.StartTime.Year * 12 + app.StartTime.Month) - rootMonth;

                    if ((uint)month < _View.YearMonths.Length)
                    {
                        YearMonth ym = _View.YearMonths[month];
                        DateTime date = app.StartTime;

                        int day = app.StartTime.Day - 1;

                        while (date < app.EndTime || (date == app.EndTime && app.StartTime == app.EndTime))
                        {
                            if (day >= ym.DaysInMonth)
                            {
                                month++;

                                if (month >= _View.YearMonths.Length)
                                    break;

                                ym = _View.YearMonths[month];
                                day = 0;
                            }

                            ym.AppBits.Set(day++, true);

                            date = date.AddDays(1);
                        }
                    }
                }
            }

            foreach (YearMonth yearMonth in _View.YearMonths)
                UpdateCustomItems(yearMonth);
        }

        #endregion

        #region UpdateCustomItems

        /// <summary>
        /// UpdateCustomItems
        /// </summary>
        /// <param name="yearMonth"></param>
        private void UpdateCustomItems(YearMonth yearMonth)
        {
            CustomCalendarItemCollection items = _View.CalendarView.CustomItems;

            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    CustomCalendarItem item = items[i];

                    if (IsCustomItemVisible(item) == true &&
                        (item.StartTime.Year == yearMonth.StartDate.Year && item.StartTime.Month == yearMonth.StartDate.Month))
                    {
                        yearMonth.AppBits.Set(item.StartTime.Day - 1, true);
                        break;
                    }
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
            LoadData(_ViewState == _lineState);

            _View.NeedRecalcLayout = true;
            _View.NeedRecalcSize = true;
        }

        #endregion

        #region GetFirstAppointment

        /// <summary>
        /// GetFirstAppointment
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal Appointment GetFirstAppointment(DateTime date)
        {
            Appointment app = null;
            AppointmentSubsetCollection asc = _Model.GetDay(date).Appointments;

            if (asc.Count > 0)
            {
                for (int i = 0; i < asc.Count; i++)
                {
                    if (IsAppointmentVisible(asc[i]) == true)
                    {
                        if (app == null || asc[i].StartTime < app.StartTime)
                            app = asc[i];
                    }
                }

                return (app);
            }

            return (null);
        }

        #endregion

        #region GetFirstCustomItem

        /// <summary>
        /// GetFirstCustomItem
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal CustomCalendarItem GetFirstCustomItem(DateTime date)
        {
            CustomCalendarItemCollection items = _View.CalendarView.CustomItems;

            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    CustomCalendarItem item = items[i];

                    if (IsCustomItemVisible(item) == true &&
                        (item.StartTime.Date == date.Date))
                    {
                        return (item);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetView

        /// <summary>
        /// Returns the Month view
        /// </summary>
        /// <returns></returns>
        public override eCalendarView GetView()
        {
            return (eCalendarView.Year);
        }

        #endregion

        #region ResetModelData

        /// <summary>
        /// ResetModelData
        /// </summary>
        static internal void ResetModelData()
        {
            _lineStartTime = DateTime.MinValue;
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
                LoadData(_ViewState == _lineState);

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

                string name = e.PropertyChangedArgs.PropertyName;

                if (name.Equals("OwnerKey"))
                {
                    if (_View.CalendarView.IsMultiCalendar == true)
                    {
                        if (_View.OwnerKey == app.OwnerKey)
                        {
                            LoadData(_ViewState == _lineState);

                            _View.NeedRecalcSize = true;
                            _View.Refresh();
                        }
                    }
                }
                else if (name.Equals("Visible"))
                {
                    LoadData(true);
                }
            }
        }

        #endregion
    }
}
#endif

