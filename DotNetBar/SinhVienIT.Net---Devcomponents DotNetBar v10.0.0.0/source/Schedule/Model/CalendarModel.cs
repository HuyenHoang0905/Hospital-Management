#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents the calendar model control.
    /// </summary>
    public class CalendarModel : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Events
        /// <summary>
        /// Occurs when an appointment has been added to the model.
        /// </summary>
        public event AppointmentEventHandler AppointmentAdded;
        /// <summary>
        /// Occurs when an appointment has been removed from the model.
        /// </summary>
        public event AppointmentEventHandler AppointmentRemoved;
        /// <summary>
        /// Occurs when AppointmentStartTime has been reached. This event can be used to trigger appointment reminders. Note that event handler will be called on the thread of System.Timer which is different
        /// than UI thread. You should use BeginInvoke calls to marshal the calls to your UI thread.
        /// </summary>
        public event AppointmentEventHandler AppointmentStartTimeReached;
        /// <summary>
        /// Occurs when Reminder's ReminderTime has been reached. Note that event handler will be called on the thread of System.Timer which is different
        /// than UI thread. You should use BeginInvoke calls to marshal the calls to your UI thread.
        /// </summary>
        [Description("Occurs when Reminder's ReminderTime has been reached.")]
        public event ReminderEventHandler ReminderNotification;
        /// <summary>
        /// Occurs when Appointments collection has been cleared.
        /// </summary>
        public event EventHandler AppointmentsCleared;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CalendarModel class.
        /// </summary>
        public CalendarModel()
        {
            _Appointments = new AppointmentCollection(this);
            _Owners = new OwnerCollection(this);
            _WorkDays = new WorkDayCollection(this);
            _CalendarWorkDays = new CalendarWorkDayCollection(this);
            // Initialize default work-days
            _WorkDays.Add(new WorkDay(DayOfWeek.Monday));
            _WorkDays.Add(new WorkDay(DayOfWeek.Tuesday));
            _WorkDays.Add(new WorkDay(DayOfWeek.Wednesday));
            _WorkDays.Add(new WorkDay(DayOfWeek.Thursday));
            _WorkDays.Add(new WorkDay(DayOfWeek.Friday));
        }
        #endregion

        #region Internal Implementation
        private AppointmentCollection _Appointments;
        /// <summary>
        /// Gets appointments associated with this calendar.
        /// </summary>
        public AppointmentCollection Appointments
        {
            get { return _Appointments; }
        }

        private OwnerCollection _Owners;
        /// <summary>
        /// Gets owners of appointments associated with this calendar.
        /// </summary>
        public OwnerCollection Owners
        {
            get { return _Owners; }
        }

        private WorkDayCollection _WorkDays;
        /// <summary>
        /// Gets working days associated with this calendar.
        /// </summary>
        public WorkDayCollection WorkDays
        {
            get { return _WorkDays; }
        }

        private CalendarWorkDayCollection _CalendarWorkDays = null;
        /// <summary>
        /// Gets the calendar/date based working days collection. This collection allows you to specify working time for specific dates. Values specified here take precedence over working hours set through WorkDays collection.
        /// </summary>
        public CalendarWorkDayCollection CalendarWorkDays
        {
            get { return _CalendarWorkDays; }
        }

        /// <summary>
        /// Gets reference to the Day object which represents day in calendar.
        /// </summary>
        /// <param name="date">Date to retrieve day for.</param>
        /// <returns>Returns reference to Day object.</returns>
        public Day GetDay(DateTime date)
        {
            Year year = null;
            if(!_Years.TryGetValue(date.Year, out year))
                year = CreateYear(date.Year);

            return year.Months[date.Month - 1].Days[date.Day - 1];
            //return new Day(date, this);
        }

        private Year CreateYear(int y)
        {
            Year year = new Year(y, this);
            _Years.Add(y, year);
            return year;
        }

        /// <summary>
        /// Returns true if appointment overlapps with one or more of the appointments in the model.
        /// </summary>
        /// <param name="app">Appointment to check overlap for.</param>
        /// <returns>true if there are appointments overlapping appointment otherwise false.</returns>
        public bool ContainsOverlappingAppointments(Appointment app)
        {
            int duration = (int)Math.Max(1, app.EndTime.Subtract(app.StartTime).TotalDays);
            for (int i = 0; i < duration; i++)
            {
                DateTime date = app.StartTime.Date.AddDays(i);
                Day day = GetDay(date);
                foreach (Appointment item in day.Appointments)
                {
                    if (item != app && DateTimeHelper.TimePeriodsOverlap(item.StartTime, item.EndTime, app.StartTime, app.EndTime))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds appointments that overlap with the parameter appointment.
        /// </summary>
        /// <param name="app">Appointment to use to find overlapps</param>
        /// <returns>Array of appointments that overlap parameter.</returns>
        public Appointment[] FindOverlappingAppointments(Appointment app)
        {
            List<Appointment> overlaps = new List<Appointment>();
            int duration = (int)Math.Max(1, app.EndTime.Subtract(app.StartTime).TotalDays);
            for (int i = 0; i < duration; i++)
            {
                DateTime date = app.StartTime.Date.AddDays(i);
                Day day = GetDay(date);
                foreach (Appointment item in day.Appointments)
                {
                    if (item != app && DateTimeHelper.TimePeriodsOverlap(item.StartTime, item.EndTime, app.StartTime, app.EndTime))
                        overlaps.Add(item);
                }
            }

            return overlaps.ToArray();
        }

        //public Month GetMonth(int year, int month)
        //{
        //    return null;
        //}

        //private HolidaysCollection _Holidays;
        ///// <summary>
        ///// Gets the collection of holidays associated with calendar.
        ///// </summary>
        //public HolidaysCollection Holidays
        //{
        //    get { return _Holidays; }
        //}

        /// <summary>
        /// Returns the calendar date time which has seconds part set to 0.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetCalendarDateTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }

        public static DateTime CurrentDateTime
        {
            get
            {
                DateTime dt = DateTime.Now;
                return dt;
            }
        }

        internal System.Globalization.Calendar GetCalendar()
        {
            return CultureInfo.CurrentCulture.Calendar;
        }

        private Dictionary<int, Year> _Years = new Dictionary<int, Year>();

        internal void InternalAppointmentRemoved(Appointment item, bool isClearing)
        {
            item.SubPropertyChanged -= this.ChildPropertyChangedEventHandler;
            if (!item.IsRecurringInstance && !isClearing)
                InvalidateAppointmentCache(item);
            if (!isClearing)
                OnAppointmentRemoved(new AppointmentEventArgs(item));
        }

        internal void InternalAppointmentsCleared()
        {
            InvalidateAppointmentCache();
            OnAppointmentsCleared(EventArgs.Empty);
        }

        protected virtual void OnAppointmentsCleared(EventArgs e)
        {
            EventHandler handler = AppointmentsCleared;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Raises the AppointmentRemoved event.
        /// </summary>
        /// <param name="appointmentEventArgs">Event arguments</param>
        protected virtual void OnAppointmentRemoved(AppointmentEventArgs appointmentEventArgs)
        {
            AppointmentEventHandler handler = AppointmentRemoved;
            if (handler != null) handler(this, appointmentEventArgs);
        }

        internal void InternalAppointmentAdded(Appointment item)
        {
            item.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
            if (!item.IsRecurringInstance)
                InvalidateAppointmentCache(item);
            OnAppointmentAdded(new AppointmentEventArgs(item));
        }

        /// <summary>
        /// Raises the AppointmentAdded event.
        /// </summary>
        /// <param name="appointmentEventArgs">Event arguments</param>
        protected virtual void OnAppointmentAdded(AppointmentEventArgs appointmentEventArgs)
        {
            AppointmentEventHandler handler = AppointmentAdded;
            if (handler != null) handler(this, appointmentEventArgs);
        }

        internal void OwnerRemoved(Owner item)
        {
            item.SubPropertyChanged -= this.ChildPropertyChangedEventHandler;
            OnPropertyChanged(new PropertyChangedEventArgs("Owners"));
        }

        internal void OwnerAdded(Owner item)
        {
            item.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
            OnPropertyChanged(new PropertyChangedEventArgs("Owners"));
        }

        internal void WorkDayRemoved(WorkDay item)
        {
            item.SubPropertyChanged -= this.ChildPropertyChangedEventHandler;
            OnPropertyChanged(new PropertyChangedEventArgs("WorkDays"));
        }

        internal void WorkDayAdded(WorkDay item)
        {
            item.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
            OnPropertyChanged(new PropertyChangedEventArgs("WorkDays"));
        }

        internal void CalendarWorkDateRemoved(CalendarWorkDay item)
        {
            item.SubPropertyChanged -= this.ChildPropertyChangedEventHandler;
            OnPropertyChanged(new PropertyChangedEventArgs("CalendarWorkDays"));
        }

        internal void CalendarWorkDateAdded(CalendarWorkDay item)
        {
            item.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
            OnPropertyChanged(new PropertyChangedEventArgs("CalendarWorkDays"));
        }

        private SubPropertyChangedEventHandler _ChildPropertyChangedEventHandler = null;
        private SubPropertyChangedEventHandler ChildPropertyChangedEventHandler
        {
            get
            {
                if (_ChildPropertyChangedEventHandler == null) _ChildPropertyChangedEventHandler = new SubPropertyChangedEventHandler(ChildPropertyChanged);
                return _ChildPropertyChangedEventHandler;
            }
        }

        private void ChildPropertyChanged(object sender, SubPropertyChangedEventArgs e)
        {
            Appointment app = sender as Appointment;
            OnSubPropertyChanged(e);

            if (app != null)
            {
                if (IsNonTimeProperty(e.PropertyChangedArgs.PropertyName)) return;
                if (app.InMoveTo && e.PropertyChangedArgs.PropertyName != "EndTime") return;
                if (IsRecurranceProperty(e.PropertyChangedArgs.PropertyName) ||
                    e.Source is DailyRecurrenceSettings || 
                    e.Source is WeeklyRecurrenceSettings || 
                    e.Source is YearlyRecurrenceSettings || 
                    e.Source is MonthlyRecurrenceSettings)
                    InvalidateAppointmentCache();
                else
                    InvalidateAppointmentCache(app);
            }
        }

        private bool IsRecurranceProperty(string propertyName)
        {
            return propertyName == Appointment.RecurrencePropertyName;
        }

        private void InvalidateAppointmentCache(Appointment app)
        {
            if (IsUpdateSuspended) return;
            if (app.Recurrence != null)
            {
                // Invalidate all
                InvalidateAppointmentCache();
            }
            else if (_Years.ContainsKey(app.LocalStartTime.Year))
            {
                DateTime d = DateTimeHelper.BeginningOfDay(app.LocalStartTime);
                DateTime end = app.LocalEndTime;
                int year = d.Year;
                while (d < end)
                {
                    _Years[d.Year].InvalidateAppointments(d.Month, d.Day);
                    d = d.AddDays(1);
                    if (d.Year != year && !_Years.ContainsKey(d.Year))
                        break;
                }
            }
            AccessToday();
            OnPropertyChanged(new PropertyChangedEventArgs(AppointmentsPropertyName));
        }

        /// <summary>
        /// Invalidates appointments cache store and causes recurrences to be regenerated when requested.
        /// </summary>
        public void InvalidateAppointmentCache()
        {
            if (IsUpdateSuspended) return;

            // Invalidate all
            foreach (Year year in _Years.Values)
            {
                year.InvalidateAppointments();
            }
            AccessToday();
            OnPropertyChanged(new PropertyChangedEventArgs(AppointmentsPropertyName));
        }

        private void AccessToday()
        {
            DateTime today = DateTime.Today;
            Day day = this.GetDay(today);
            int appCount = day.Appointments.Count;
        }
        private bool IsNonTimeProperty(string propertyName)
        {
            if (propertyName == "Description" || propertyName == "IsRecurringInstance" || propertyName == "Tag"
                || propertyName == "OwnerKey" || propertyName == "IsSelected" || propertyName == "Locked" || propertyName=="LocalStartTime" || propertyName=="LocalEndTime")
                return true;
            return false;
        }

        private int _UpdatesCount = 0;
        /// <summary>
        /// Suspends internal control updates to the cache structures etc. When making changes on multiple appointments 
        /// time related properties or when adding multiple appointments before doing so call BeginUpdate and after 
        /// updates are done call EndUpdate method to optimize performance.
        /// <remarks>Calls to BeginUpdate method can be nested and only last outer most EndUpdate call will resume internal control updates.</remarks>
        /// </summary>
        public void BeginUpdate()
        {
            _UpdatesCount++;
        }
        /// <summary>
        /// Resumes internal control updates that were suspended using BeginUpdate call and invalidates internal cache.
        /// </summary>
        public void EndUpdate()
        {
            if (_UpdatesCount == 0)
                throw new InvalidOperationException("EndUpdate must be called AFTER BeginUpdate");
            _UpdatesCount--;
            if (_UpdatesCount == 0)
                InvalidateAppointmentCache();
        }

        /// <summary>
        /// Gets whether internal control update is suspended due to the call to BeginUpdate method.
        /// </summary>
        [Browsable(false)]
        public bool IsUpdateSuspended
        {
            get
            {
                return _UpdatesCount > 0;
            }
        }

        internal static string AppointmentsPropertyName
        {
            get { return "Appointments"; }
        }
        internal static string WorkDaysPropertyName
        {
            get { return "WorkDays"; }
        }
        internal static string CalendarWorkDaysPropertyName
        {
            get { return "CalendarWorkDays"; }
        }

        private TimeZoneInfo _DisplayTimeZone = null;
        /// <summary>
        /// Gets or sets the default display time zone used for the appointments. Default value is null which indicates that system time-zone is used.
        /// Display Time zone can also be set for each Owner on Owner object. Value set here is used if specific display time-zone is not set on user.
        /// </summary>
        [DefaultValue(null)]
        public TimeZoneInfo DisplayTimeZone
        {
            get { return _DisplayTimeZone; }
            set
            {
                if (value != _DisplayTimeZone)
                {
                    TimeZoneInfo oldValue = _DisplayTimeZone;
                    _DisplayTimeZone = value;
                    InvalidateAppointmentTimes();
                    InvalidateAppointmentCache();
                    OnDisplayTimeZoneChanged(oldValue, value);
                }
            }
        }

        private void InvalidateAppointmentTimes()
        {
            foreach (Appointment item in _Appointments)
            {
                item.InvokeLocalTimePropertyChange();
            }
        }

        private void OnDisplayTimeZoneChanged(TimeZoneInfo oldValue, TimeZoneInfo newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("DisplayTimeZone"));
            
        }


        internal void InvokeAppointmentStartTimeReached(Appointment appointment)
        {
            OnAppointmentStartTimeReached(new AppointmentEventArgs(appointment));
        }

        /// <summary>
        /// Raises AppointmentStartTimeReached event.
        /// </summary>
        /// <param name="appointmentEventArgs">Event arguments</param>
        protected virtual void OnAppointmentStartTimeReached(AppointmentEventArgs appointmentEventArgs)
        {
            AppointmentEventHandler handler = AppointmentStartTimeReached;
            if (handler != null)
                handler(this, appointmentEventArgs);
        }

        internal void InvokeReminderNotification(Reminder reminder)
        {
            OnReminderNotification(new ReminderEventArgs(reminder));
        }
        /// <summary>
        /// Raises ReminderNotification event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnReminderNotification(ReminderEventArgs e)
        {
            ReminderEventHandler h = ReminderNotification;
            if (h != null)
                h(this, e);
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler eh = PropertyChanged;

            if (eh != null)
                eh(this, e);

            OnSubPropertyChanged(new SubPropertyChangedEventArgs(this, e));
        }

        #endregion

        #region INotifySubPropertyChanged Members

        /// <summary>
        /// Occurs when property or property of child objects has changed. This event is similar to PropertyChanged event with key
        /// difference that it occurs for the property changed of child objects as well.
        /// </summary>
        public event SubPropertyChangedEventHandler SubPropertyChanged;
        /// <summary>
        /// Raises the SubPropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnSubPropertyChanged(SubPropertyChangedEventArgs e)
        {
            SubPropertyChangedEventHandler eh = SubPropertyChanged;
            if (eh != null) eh(this, e);
        }
        #endregion

        #region CustomReminders
        private ReminderCollection _CustomReminders = null;
        /// <summary>
        /// Gets the collection of custom reminders that are not associated with appointments.
        /// </summary>
        public ReminderCollection CustomReminders
        {
            get
            {
                if (_CustomReminders == null) _CustomReminders = new ReminderCollection(this);
                return _CustomReminders;
            }
        }
        #endregion
    }

    #region Events Support
    public delegate void AppointmentEventHandler(object sender, AppointmentEventArgs e);
    /// <summary>
    /// Defines arguments for appointment related events.
    /// </summary>
    public class AppointmentEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the appointment referenced by this event.
        /// </summary>
        public Appointment Appointment;

        /// <summary>
        /// Initializes a new instance of the AppointmentEventArgs class.
        /// </summary>
        /// <param name="appointment"></param>
        public AppointmentEventArgs(Appointment appointment)
        {
            Appointment = appointment;
        }
    }
    #endregion
    
}
#endif

