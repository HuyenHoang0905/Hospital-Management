#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents an calendar appointment.
    /// </summary>
    public class Appointment : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Private Variables
        private bool _StartTimeNotificationRegistered = false;
        private bool _ReminderNotificationRegistered = false;
        private static long AutoIdCounter;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when system time reaches the appointment start time and StartTimeAction is set to fire event. Note that event handler will be called on the thread of System.Timer which is different
        /// than UI thread. You should use BeginInvoke calls to marshal the calls to your UI thread.
        /// </summary>
        [Description("Occurs when system time reaches the appointment start time and StartTimeAction is set to fire event.")]
        public event EventHandler StartTimeReached;
        
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Appointment class.
        /// </summary>
        public Appointment()
        {
            _Reminders = new ReminderCollection(this);
            _AutoId = ++AutoIdCounter;
        }
        /// <summary>
        /// Initializes a new instance of the Appointment class.
        /// </summary>
        /// <param name="subject">Appointment subject.</param>
        /// <param name="startTime">Appointment start time</param>
        /// <param name="endTime">Appointment end time</param>
        public Appointment(DateTime startTime, DateTime endTime, string subject)
            : this()
        {
            Subject = subject;
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// Initializes a new instance of the Appointment class.
        /// </summary>
        /// <param name="subject">Appointment subject.</param>
        /// <param name="startTime">Appointment start time</param>
        /// <param name="endTime">Appointment end time</param>
        /// <param name="ownerKey">Appointment owner key</param>
        public Appointment(DateTime startTime, DateTime endTime, string subject, string ownerKey)
            : this()
        {
            Subject = subject;
            StartTime = startTime;
            EndTime = endTime;
            OwnerKey = ownerKey;
        }

        /// <summary>
        /// Initializes a new instance of the Appointment class.
        /// </summary>
        /// <param name="subject">Appointment subject.</param>
        /// <param name="startTime">Appointment start time</param>
        /// <param name="durationInMinutes">Appointment duration in minutes</param>
        public Appointment(DateTime startTime, double durationInMinutes, string subject)
            : this()
        {
            Subject = subject;
            StartTime = startTime;
            EndTime = startTime.AddMinutes(durationInMinutes);
        }

        /// <summary>
        /// Initializes a new instance of the Appointment class.
        /// </summary>
        /// <param name="subject">Appointment subject.</param>
        /// <param name="startTime">Appointment start time</param>
        /// <param name="durationInMinutes">Appointment duration in minutes</param>
        /// <param name="ownerKey">Appointment owner key</param>
        public Appointment(DateTime startTime, double durationInMinutes, string subject, string ownerKey)
            : this()
        {
            Subject = subject;
            StartTime = startTime;
            EndTime = startTime.AddMinutes(durationInMinutes);
            OwnerKey = ownerKey;
        }
        #endregion

        #region Internal Implementation
        private bool _Visible = true;
        /// <summary>
        /// Gets or sets whether appointment is visible in user interface views. Default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (value != _Visible)
                {
                    bool oldValue = _Visible;
                    _Visible = value;
                    OnVisibleChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when Visible property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnVisibleChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
        }

        private string _Tooltip = "";
        /// <summary>
        /// Gets or sets the tooltip that is assigned to the appointment view.
        /// </summary>
        [DefaultValue("")]
        public string Tooltip
        {
            get { return _Tooltip; }
            set
            {
                if (value == null) value = "";
                if (value != _Tooltip)
                {
                    object oldValue = _Tooltip;
                    _Tooltip = value;
                    OnTooltipChanged(oldValue, value);
                }
            }
        }

        private void OnTooltipChanged(object oldValue, object newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Tooltip"));
            
        }
        private string _CategoryColor;
        /// <summary>
        /// Gets or sets the appointment category color string based key that is used to lookup for appointment background and border colors.
        /// Use static members on Appointment class to assign the category color for example Appointment.CategoryRed.
        /// </summary>
        public string CategoryColor
        {
            get { return _CategoryColor; }
            set
            {
                if (value != _CategoryColor)
                {
                    string oldValue = _CategoryColor;
                    _CategoryColor = value;
                    OnCategoryColorChanged(oldValue, value);
                }
            }
        }

        private void OnCategoryColorChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("CategoryColor"));
        }

        private string _TimeMarkedAs;
        /// <summary>
        /// Gets or sets how the time used by appointment is marked on calendar. For example Free, Tentative, Busy etc.
        /// Use static members on Appointment class to assign the time marker for example Appointment.TimeMarkerBusy
        /// </summary>
        public string TimeMarkedAs
        {
            get { return _TimeMarkedAs; }
            set
            {
                if (value != _TimeMarkedAs)
                {
                    string oldValue = _TimeMarkedAs;
                    _TimeMarkedAs = value;
                    OnTimeMarkedAsChanged(oldValue, value);
                }
            }
        }
        private void OnTimeMarkedAsChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("TimeMarkedAs"));
            
        }

        private ReminderCollection _Reminders = null;
        /// <summary>
        /// Gets the collection of reminders associated with this Appointment.
        /// </summary>
        [Browsable(false)]
        public ReminderCollection Reminders
        {
            get { return _Reminders; }
        }

        private string _Subject = "";
        /// <summary>
        /// Gets or sets the appointment subject.
        /// </summary>
        [DefaultValue("")]
        public string Subject
        {
            get { return _Subject; }
            set
            {
                if (_Subject != value)
                {
                    string oldValue = _Subject;
                    _Subject = value;
                    OnSubjectChanged(oldValue, _Subject);
                }
            }
        }

        private void OnSubjectChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Subject"));
        }

        private string _Description = "";
        /// <summary>
        /// Gets or sets the appointment description.
        /// </summary>
        [DefaultValue("")]
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    string oldValue = _Description;
                    _Description = value;
                    OnDescriptionChanged(oldValue, _Description);
                }
            }
        }

        private void OnDescriptionChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Description"));
        }

        private string _DisplayTemplate = "";
        /// <summary>
        /// Gets or sets the appointment display template.
        /// </summary>
        [DefaultValue("")]
        public string DisplayTemplate
        {
            get { return _DisplayTemplate; }

            set
            {
                if (_DisplayTemplate != value)
                {
                    _DisplayTemplate = value;

                    OnDisplayTemplateChanged();
                }
            }
        }

        private void OnDisplayTemplateChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("DisplayTemplate"));
        }

        private bool _IsSelected = false;
        /// <summary>
        /// Gets or sets whether appointment is selected in the user interface.
        /// </summary>
        [Browsable(false)]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    bool oldValue = _IsSelected;
                    _IsSelected = value;
                    OnIsSelectedChanged(oldValue, value);
                }
            }
        }

        private void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("IsSelected"));
            
        }

        private bool _Locked = false;
        /// <summary>
        /// Gets or sets whether appointment modification through user interface is disabled. Default value is false.
        /// </summary>
        [DefaultValue(false)]
        public bool Locked
        {
            get { return _Locked; }
            set
            {
                if (value != _Locked)
                {
                    bool oldValue = _Locked;
                    _Locked = value;
                    OnLockedChanged(oldValue, value);
                }
            }
        }

        private void OnLockedChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Locked"));
            
        }

        private DateTime _StartTime;
        /// <summary>
        /// Gets or sets the appointment start time.
        /// </summary>
        public DateTime StartTime
        {
            get { return GetTimeZoneDateTime(_StartTime); }
            set
            {
                value = GetUTCDateTime(CalendarModel.GetCalendarDateTime(value));
                if (!_StartTime.Equals(value))
                {
                    bool oldMultiDay = this.IsMultiDayOrAllDayEvent;
                    DateTime oldValue = _StartTime;
                    _StartTime = value;
                    OnStartTimeChanged(oldValue, _StartTime);
                    if(oldMultiDay!=this.IsMultiDayOrAllDayEvent)
                        OnPropertyChanged(new PropertyChangedEventArgs("IsMultiDayOrAllDayEvent"));
                }
            }
        }

        private void UpdateReminderStartTime(DateTime oldStartTime, DateTime newStartTime)
        {
            if(oldStartTime==DateTime.MinValue) return;

            foreach (Reminder item in _Reminders)
            {
                item.ReminderTime = newStartTime.Add(item.ReminderTime.Subtract(oldStartTime));
            }
        }
        private void OnStartTimeChanged(DateTime oldValue, DateTime newValue)
        {
            UpdateReminderStartTime(oldValue, newValue);
            UpdateNotification();
            OnPropertyChanged(new AppointmentTimePropertyChangedEventArgs("StartTime", oldValue, newValue));
            OnPropertyChanged(new PropertyChangedEventArgs("LocalStartTime"));
        }

        internal void InvokeLocalTimePropertyChange()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("LocalStartTime"));
            OnPropertyChanged(new PropertyChangedEventArgs("LocalEndTime"));
        }

        private DateTime _EndTime;
        /// <summary>
        /// Gets or sets the appointment end time.
        /// </summary>
        public DateTime EndTime
        {
            get 
            {
                return GetTimeZoneDateTime(_EndTime); 
            }
            set 
            {
                value = GetUTCDateTime(CalendarModel.GetCalendarDateTime(value));
                if (!_EndTime.Equals(value))
                {
                    bool oldMultiDay = this.IsMultiDayOrAllDayEvent;

                    DateTime oldValue = _EndTime;
                    _EndTime = value;
                    OnEndTimeChanged(oldValue, _EndTime);

                    if (oldMultiDay != this.IsMultiDayOrAllDayEvent)
                        OnPropertyChanged(new PropertyChangedEventArgs("IsMultiDayOrAllDayEvent"));
                }
            }
        }

        private void OnEndTimeChanged(DateTime oldValue, DateTime newValue)
        {
            OnPropertyChanged(new AppointmentTimePropertyChangedEventArgs("EndTime", oldValue, newValue));
            OnPropertyChanged(new PropertyChangedEventArgs("LocalEndTime"));
        }

        private bool _InMoveTo;
        internal bool InMoveTo
        {
            get { return _InMoveTo; }
        }

        /// <summary>
        /// Moves the appointment to the specified date and time while keeping its duration constant.
        /// </summary>
        /// <param name="newStartTime">New start date and time for appointment.</param>
        public void MoveTo(DateTime newStartTime)
        {
            TimeSpan duration = EndTime.Subtract(StartTime);
            DateTime oldStartTime = this.LocalStartTime;
            try
            {
                _InMoveTo = true;
                this.LocalStartTime = newStartTime;
                this.LocalEndTime = newStartTime.Add(duration);

                foreach (Reminder item in this.Reminders)
                {
                    TimeSpan span = item.ReminderTime.Subtract(oldStartTime);
                    item.ReminderTime = newStartTime.Add(span);
                }
            }
            finally
            {
                _InMoveTo = false;
            }
        }

        private TimeZoneInfo _TimeZone;
        /// <summary>
        /// Gets or sets the time-zone this appointment is defined in. Default value is null which indicates that appointment is
        /// in current system time zone. Note that setting the time zone will affect StartTime and EndTime of appointment if set.
        /// It will convert them to the TimeZone you set but as absolute values meaning that 10:00 AM in previous time zone
        /// will become 10:00 AM in TimeZone you just assigned.
        /// </summary>
        [DefaultValue(null)]
        public TimeZoneInfo TimeZone
        {
            get { return _TimeZone; }
            set
            {
                if (value != _TimeZone)
                {
                    TimeZoneInfo oldValue = _TimeZone;
                    _TimeZone = value;
                    OnTimeZoneChanged(oldValue, value);
                }
            }
        }

        private void OnTimeZoneChanged(TimeZoneInfo oldValue, TimeZoneInfo newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("TimeZone"));
            // Update start and end time
            if (_StartTime.Kind == DateTimeKind.Utc)
            {
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(_StartTime, oldValue ?? TimeZoneInfo.Local);
                this.StartTime = date;
            }
            if (_EndTime.Kind == DateTimeKind.Utc)
            {
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(_EndTime, oldValue ?? TimeZoneInfo.Local);
                this.EndTime = date;
            }
        }

        internal DateTime GetValidDateTime(DateTime date)
        {
            if (date == DateTime.MinValue || date == DateTime.MaxValue || date.Kind == DateTimeKind.Utc)
                return date;
            TimeZoneInfo timeZone = _TimeZone ?? TimeZoneInfo.Local;
            if (timeZone.IsInvalidTime(date))
            {
                while (timeZone.IsInvalidTime(date))
                    date = date.AddHours(1);
            }
            return date;
        }

        internal DateTime GetUTCDateTime(DateTime date)
        {
            if (date == DateTime.MinValue || date == DateTime.MaxValue || date.Kind == DateTimeKind.Utc)
                return date;
            if (_TimeZone == null)
                return TimeZoneInfo.ConvertTimeToUtc(date);
            return TimeZoneInfo.ConvertTimeToUtc(date, _TimeZone);
        }

        private DateTime GetTimeZoneDateTime(DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc) return date;
            TimeZoneInfo zone = _TimeZone;
            if (zone == null) zone = TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeFromUtc(date, zone);
        }

        /// <summary>
        /// Gets or sets the StartTime of appointment in local, display time-zone.
        /// </summary>
        public DateTime LocalStartTime
        {
            get
            {
                return GetLocalDateTime(_StartTime);
            }
            set
            {
                StartTime = LocalToCurrenTimeZone(value);
            }
        }

        /// <summary>
        /// Gets or sets the StartTime of appointment in local, display time-zone.
        /// </summary>
        public DateTime LocalEndTime
        {
            get
            {
                return GetLocalDateTime(_EndTime);
            }
            set
            {
                EndTime = LocalToCurrenTimeZone(value);
            }
        }

        /// <summary>
        /// Gets the UTC Start Time of appointment.
        /// </summary>
        public DateTime UtcStartTime
        {
            get { return _StartTime; }
        }

        /// <summary>
        /// Gets the UTC End Time of appointment.
        /// </summary>
        public DateTime UtcEndTime
        {
            get { return _EndTime; }
        }

        private DateTime LocalToCurrenTimeZone(DateTime value)
        {
            if (_TimeZone == null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(TimeZoneInfo.ConvertTimeToUtc(value, GetLocalTimeZone()), TimeZoneInfo.Local);
            }

            DateTime date = TimeZoneInfo.ConvertTimeToUtc(value, GetLocalTimeZone());
            return TimeZoneInfo.ConvertTimeFromUtc(date, _TimeZone);
        }

        internal DateTime GetLocalDateTime(DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc || value == DateTime.MinValue || value == DateTime.MaxValue) return value;
            return TimeZoneInfo.ConvertTimeFromUtc(value, GetLocalTimeZone());
        }

        private TimeZoneInfo GetLocalTimeZone()
        {
            if (!string.IsNullOrEmpty(_OwnerKey) && _Calendar != null)
            {
                Owner owner = _Calendar.Owners[_OwnerKey];
                if (owner != null && owner.DisplayTimeZone != null)
                    return owner.DisplayTimeZone;
            }
            if (_Calendar == null || _Calendar.DisplayTimeZone == null) return TimeZoneInfo.Local;
            return _Calendar.DisplayTimeZone;
        }

        private CalendarModel _Calendar = null;
        /// <summary>
        /// Gets the calendar appointment is associated with.
        /// </summary>
        [Browsable(false)]
        public CalendarModel Calendar
        {
            get { return _Calendar; }
            internal set
            {
                if (_Calendar != value)
                {
                    _Calendar = value;
                    UpdateNotification();
                }
            }
        }

        private void UpdateNotification()
        {
            if (_Calendar == null)
            {
                if (_StartTimeNotificationRegistered)
                    NotificationServer.Unregister(this.StartTimeNotificationRequest);
                _StartTimeNotificationRegistered = false;
                this.StartTimeNotificationRequest = null;

                foreach (Reminder reminder in this.Reminders)
                {
                    reminder.UpdateNotifications();
                }
                return;
            }

            // Get Now in UTC of current machine, _StartTime is already UTC
            DateTime now = TimeZoneInfo.ConvertTimeToUtc(CalendarModel.GetCalendarDateTime(CalendarModel.CurrentDateTime));

            // Check start-time notification requirement
            if (_StartTime != DateTime.MinValue && _StartTimeAction != eStartTimeAction.None && _StartTime > now)
            {
                if (_StartTimeNotificationRegistered)
                    NotificationServer.Unregister(this.StartTimeNotificationRequest);
                NotificationServer.Register(this.StartTimeNotificationRequest);
                _StartTimeNotificationRegistered = true;
            }
            else if (_StartTimeNotificationRegistered)
                NotificationServer.Unregister(this.StartTimeNotificationRequest);

            // Check reminders notification requirement
            foreach (Reminder reminder in this.Reminders)
            {
                reminder.UpdateNotifications();
            }
        }

        private DateTime GetNextNotificationTime()
        {
            DateTime nextNotify = DateTime.MinValue;

            if (_StartTime != DateTime.MinValue && _StartTimeAction != eStartTimeAction.None)
                nextNotify = _StartTime;

            return nextNotify;
        }

        private bool NeedsNotification
        {
            get
            {
                if (_StartTime > DateTime.MinValue && _StartTimeAction != eStartTimeAction.None)
                    return true;
                // Check reminders

                return false;
            }
        }

        private eStartTimeAction _StartTimeAction = eStartTimeAction.None;
        /// <summary>
        /// Gets or sets the action performed when StartTime of appointment is reached. Default is none.
        /// </summary>
        [DefaultValue(eStartTimeAction.None)]
        public eStartTimeAction StartTimeAction
        {
            get { return _StartTimeAction; }
            set 
            {
                if (_StartTimeAction != value)
                {
                    eStartTimeAction oldValue = _StartTimeAction;
                    _StartTimeAction = value;
                    OnStartTimeActionChanged(oldValue, _StartTimeAction);
                }
            }
        }

        private void OnStartTimeActionChanged(eStartTimeAction oldValue, eStartTimeAction newValue)
        {
            if (oldValue == eStartTimeAction.None || newValue == eStartTimeAction.None)
                UpdateNotification();
            OnPropertyChanged(new PropertyChangedEventArgs("StartTimeAction"));
        }

        private NotificationRequest _StartTimeNotificationRequest = null;
        private NotificationRequest StartTimeNotificationRequest
        {
            get
            {
                if (_StartTimeNotificationRequest == null)
                {
                    _StartTimeNotificationRequest = new NotificationRequest(_StartTime, new NotificationServerEventHandler(OnStartTimeNotification));
                }
                else
                    _StartTimeNotificationRequest.NotificationTime = _StartTime;
                return _StartTimeNotificationRequest;
            }
            set
            {
                _StartTimeNotificationRequest = value;
            }
        }

        private void OnStartTimeNotification(NotificationRequest request, NotificationServerEventArgs e)
        {
            if (_StartTimeAction != eStartTimeAction.None)
            {
                this.StartTimeNotificationRequest = null;
                ProcessStartTimeReachedActions();
            }
        }

        private void ProcessStartTimeReachedActions()
        {
            if (_Calendar != null)
            {
                _Calendar.InvokeAppointmentStartTimeReached(this);
            }

            if (_StartTimeAction == eStartTimeAction.None) return;

            if ((_StartTimeAction & eStartTimeAction.StartTimeReachedEvent) != 0)
            {
                OnStartTimeReached(new EventArgs());
            }

            if ((_StartTimeAction & eStartTimeAction.Command) != 0)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Raises the StartTimeReached event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnStartTimeReached(EventArgs e)
        {
            EventHandler h = StartTimeReached;
            if (h != null)
                h(this, e);
        }

        private bool _IsRecurringInstance = false;
        /// <summary>
        /// Gets whether this appointment is the recurring appointment instance.
        /// </summary>
        [Browsable(false)]
        public bool IsRecurringInstance
        {
            get { return _IsRecurringInstance; }
            set
            {
                if (_IsRecurringInstance != value)
                {
                    bool oldValue = _IsRecurringInstance;
                    _IsRecurringInstance = value;
                    OnIsRecurringInstanceChanged(oldValue, _IsRecurringInstance);
                }
            }
        }

        private void OnIsRecurringInstanceChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("IsRecurringInstance"));
        }

        private Appointment _RootAppointment = null;
        /// <summary>
        /// Gets or sets the root appointment if this instance is an recurring appointment instance (IsRecurringInstance=true).
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Appointment RootAppointment
        {
            get { return _RootAppointment; }
            internal set { _RootAppointment = value; }
        }

        private object _Tag = null;
        /// <summary>
        /// Gets or sets additional data associated with the object.
        /// </summary>
        [DefaultValue(null)]
        public object Tag
        {
            get { return _Tag; }
            set
            {
                if (value != _Tag)
                {
                    object oldValue = _Tag;
                    _Tag = value;
                    OnTagChanged(oldValue, value);
                }
            }
        }

        private void OnTagChanged(object oldValue, object newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Tag"));
            
        }

        private AppointmentRecurrence _Recurrence = null;
        /// <summary>
        /// Gets or sets the reference to the appointment recurrence definition object which defines
        /// recurring appointment properties and range.
        /// </summary>
        [DefaultValue(null)]
        public AppointmentRecurrence Recurrence
        {
            get { return _Recurrence; }
            set
            {
                if (value != _Recurrence)
                {
                    AppointmentRecurrence oldValue = _Recurrence;
                    _Recurrence = value;
                    OnRecurrenceChanged(oldValue, value);
                }
            }
        }

        private void OnRecurrenceChanged(AppointmentRecurrence oldValue, AppointmentRecurrence newValue)
        {
            if (oldValue != null)
            {
                oldValue.Appointment = null;
                oldValue.SubPropertyChanged -= this.ChildPropertyChangedEventHandler;
            }

            if (newValue != null)
            {
                if (newValue.Appointment != null)
                    throw new ArgumentException("Recurrence value already assigned to the appointment. Recurrence can be assigned to single appointment only");
                newValue.Appointment = this;
                newValue.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
            }
            OnPropertyChanged(new PropertyChangedEventArgs("Recurrence"));
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
            OnSubPropertyChanged(e);
        }

        /// <summary>
        /// Creates an copy of the appointment.
        /// </summary>
        /// <returns>Appointment copy.</returns>
        public Appointment Copy()
        {
            Appointment copy = new Appointment();

            copy.OwnerKey = this.OwnerKey;
            copy.Description = this.Description;
            copy.EndTime = this.EndTime;
            copy.CategoryColor = this.CategoryColor;
            copy.DisplayTemplate = this.DisplayTemplate;
            copy.ImageAlign = this.ImageAlign;
            copy.ImageKey = this.ImageKey;
            copy.Locked = this.Locked;
            copy.TimeMarkedAs = this.TimeMarkedAs;
            copy.TimeZone = this.TimeZone;
            copy.Tooltip = this.Tooltip;

            if (_Reminders != null)
            {
                foreach (Reminder rem in _Reminders)
                    copy.Reminders.Add(rem.Copy());
            }

            copy.StartTime = this.StartTime;
            copy.StartTimeAction = this.StartTimeAction;

            if (StartTimeReached != null)
                copy.StartTimeReached = (EventHandler)this.StartTimeReached.Clone();

            copy.Subject = this.Subject;
            copy.Tag = this.Tag;

            return copy;
        }

        private string _OwnerKey = "";
        /// <summary>
        /// Gets or sets the owner of the appointment. Default value is empty string which indicates default owner.
        /// </summary>
        public string OwnerKey
        {
            get { return _OwnerKey; }
            set
            {
                if (value == null) value = "";
                if (value != _OwnerKey)
                {
                    string oldValue = _OwnerKey;
                    _OwnerKey = value;
                    OnOwnerKeyChanged(oldValue, value);
                }
            }
        }

        private void OnOwnerKeyChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("OwnerKey"));
        }

        /// <summary>
        /// Gets whether appointment is all day event or whether it spans multiple days.
        /// </summary>
        public bool IsMultiDayOrAllDayEvent
        {
            get
            {
                if (this.StartTime < this.EndTime && (this.EndTime.Subtract(this.StartTime).TotalDays >= 1 || !DateTimeHelper.IsSameDay(this.StartTime, this.EndTime)))
                    return true;
                return false;
            }
        }

        private int _Id = 0;
        /// <summary>
        /// Gets or sets the appointment identifier. This property is provided for your usage in serialization scenarios. It is not set by the control.
        /// </summary>
        [DefaultValue(0)]
        public int Id
        {
            get { return _Id; }
            set
            {
                if (value != _Id)
                {
                    int oldValue = _Id;
                    _Id = value;
                    OnIdChanged(oldValue, value);
                }
            }
        }

        private void OnIdChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Id"));
        }

        private long _AutoId;
        /// <summary>
        /// Gets the automatically generated identifier that identifies appointment.
        /// </summary>
        public long AutoId
        {
            get
            {
                return _AutoId;
            }
        }

        public override string ToString()
        {
            return string.Format("Subject: {0}, StartTime: {1}, EndTime: {2}, Id: {3}, AutoId: {4}", this.Subject, this.StartTime, this.EndTime, this.Id, this.AutoId);
        }

        private string _ImageKey;
        /// <summary>
        /// Gets or sets the image key for the image displayed on appointment view. ImageList property on CalendarView must be set for this property to work.
        /// </summary>
        public string ImageKey
        {
            get { return _ImageKey; }
            set
            {
                if (value != _ImageKey)
                {
                    string oldValue = _ImageKey;
                    _ImageKey = value;
                    OnImageKeyChanged(oldValue, value);
                }
            }
        }

        private void OnImageKeyChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ImageKey"));
        }

        private eImageContentAlignment _ImageAlign = eImageContentAlignment.TopLeft;
        /// <summary>
        /// Gets or sets the image alignment in relation to the appointment view content. Default value is TopLeft.
        /// </summary>
        public eImageContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                if (value != _ImageAlign)
                {
                    eImageContentAlignment oldValue = _ImageAlign;
                    _ImageAlign = value;
                    OnImageAlignChanged(oldValue, value);
                }
            }
        }

        private void OnImageAlignChanged(eImageContentAlignment oldValue, eImageContentAlignment newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ImageAlign"));
            
        }
        #endregion

        #region Static Members
        public static readonly string CategoryYellow = "Yellow";
        public static readonly string CategoryDefault = null;
        public static readonly string CategoryBlue = "Blue";
        public static readonly string CategoryGreen = "Green";
        public static readonly string CategoryOrange = "Orange";
        public static readonly string CategoryPurple = "Purple";
        public static readonly string CategoryRed = "Red";

        public static readonly string TimerMarkerDefault = null;
        public static readonly string TimerMarkerBusy = "Busy";
        public static readonly string TimerMarkerFree = "Free";
        public static readonly string TimerMarkerTentative = "Tentative";
        public static readonly string TimerMarkerOutOfOffice = "OutOfOffice";

        internal static readonly string RecurrencePropertyName = "Recurrence";
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
            if (eh != null) eh(this, e);
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
    }
    /// <summary>
    /// Provides more information about the time change for appointment StartTime and EndTime property changes.
    /// </summary>
    public class AppointmentTimePropertyChangedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Gets the old value.
        /// </summary>
        public readonly DateTime OldTimeValue;
        /// <summary>
        /// Gets the new value.
        /// </summary>
        public readonly DateTime NewTimeValue;

        /// <summary>
        /// Initializes a new instance of the AppointmentTimePropertyChangedEventArgs class.
        /// </summary>
        /// <param name="oldTimeValue"></param>
        /// <param name="newTimeValue"></param>
        public AppointmentTimePropertyChangedEventArgs(string propertyName, DateTime oldTimeValue, DateTime newTimeValue)
            : base(propertyName)
        {
            OldTimeValue = oldTimeValue;
            NewTimeValue = newTimeValue;
        }
    }

    public enum eImageContentAlignment
    {
        BottomCenter = 0x200,
        BottomLeft = 0x100,
        BottomRight = 0x400,
        MiddleCenter = 0x20,
        MiddleLeft = 0x10,
        MiddleRight = 0x40,
        TopCenter = 2,
        TopLeft = 1,
        TopRight = 4
    }
}
#endif

