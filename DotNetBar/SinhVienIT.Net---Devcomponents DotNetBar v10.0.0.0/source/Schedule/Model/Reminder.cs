#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents an appointment reminder.
    /// </summary>
    public class Reminder
    {
        #region Private variables
        private bool _NotificationRequestRegistered = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Reminder class.
        /// </summary>
        public Reminder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Reminder class.
        /// </summary>
        /// <param name="reminderTime"></param>
        public Reminder(DateTime reminderTime)
        {
            ReminderTime = reminderTime;
        }

        /// <summary>
        /// Initializes a new instance of the Reminder class.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="reminderTime"></param>
        public Reminder(string description, DateTime reminderTime)
        {
            Description = description;
            ReminderTime = reminderTime;
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when ReminderTime has been reached. Note that event handler will be called on the thread of System.Timer which is different
        /// than UI thread. You should use BeginInvoke calls to marshal the calls to your UI thread.
        /// </summary>
        [Description("Occurs when ReminderTime has been reached.")]
        public event ReminderEventHandler ReminderNotification;

        #endregion

        #region Internal Implementation
        private bool _IsActiveForPastAppointments = true;
        /// <summary>
        /// Gets or sets whether reminder will be active for appointments that are in the past. Default value is true.
        /// This property is useful if you are creating recurring appointments with reminders that start in past but don't want reminders
        /// for past instances of appointment to be active.
        /// </summary>
        public bool IsActiveForPastAppointments
        {
            get { return _IsActiveForPastAppointments; }
            set
            {
                if (value != _IsActiveForPastAppointments)
                {
                    bool oldValue = _IsActiveForPastAppointments;
                    _IsActiveForPastAppointments = value;
                    OnIsActiveForPastAppointmentsChanged(oldValue, value);
                }
            }
        }

        private void OnIsActiveForPastAppointmentsChanged(bool oldValue, bool newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("IsActiveForPastAppointments"));
            ReminderCollection parentCollection = this.ParentCollection;
            
            if (parentCollection != null)
            {
                Appointment app = this.ParentCollection.Appointment;
                if (app != null)
                {
                    UpdateNotifications();
                }
            }
        }

        private string _Description = "";
        /// <summary>
        /// Gets or sets the reminder description.
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    string oldValue = _Description;
                    _Description = value;
                    OnDescriptionChanged(oldValue, _Description);
                }
            }
        }

        private void OnDescriptionChanged(string oldValue, string newValue)
        {

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

        }


        /// <summary>
        /// Gets or sets the date and time reminder will be executed at.
        /// <remarks>
        /// Unless you mark reminder as inactive by setting the IsActive=false the reminder will occur next time
        /// notifications are updated or when appointment data is loaded.
        /// </remarks>
        /// </summary>
        private DateTime _ReminderTime = DateTime.MinValue;
        public DateTime ReminderTime
        {
            get { return GetTimeZoneDateTime(_ReminderTime); }
            set
            {
                value = GetUTCDateTime(CalendarModel.GetCalendarDateTime(value));
                if (_ReminderTime != value)
                {
                    DateTime oldValue = _ReminderTime;
                    _ReminderTime = value;
                    OnReminderTimeChanged(oldValue, _ReminderTime);
                }
            }
        }

        private DateTime GetUTCDateTime(DateTime date)
        {
            if (date == DateTime.MinValue || date == DateTime.MaxValue || date.Kind == DateTimeKind.Utc)
                return date;
            return TimeZoneInfo.ConvertTimeToUtc(date);
        }

        private DateTime GetTimeZoneDateTime(DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc) return date;
            TimeZoneInfo zone = TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeFromUtc(date, zone);
        }

        private void OnReminderTimeChanged(DateTime oldValue, DateTime newValue)
        {
            if (NeedsNotification)
                UpdateNotifications();
        }

        private bool NeedsNotification
        {
            get
            {
                return _ReminderAction != eReminderAction.None && _IsActive;
            }
        }

        /// <summary>
        /// Gets or sets the action performed when reminder time is reached. Default value is Event and Command.
        /// </summary>
        private eReminderAction _ReminderAction = eReminderAction.EventAndCommand;
        [DefaultValue(eReminderAction.EventAndCommand)]
        public eReminderAction ReminderAction
        {
            get { return _ReminderAction; }
            set
            {
                if (_ReminderAction != value)
                {
                    eReminderAction oldValue = _ReminderAction;
                    _ReminderAction = value;
                    OnReminderActionChanged(oldValue, _ReminderAction);
                }
            }
        }

        private void OnReminderActionChanged(eReminderAction oldValue, eReminderAction newValue)
        {
            if (oldValue == eReminderAction.None || newValue == eReminderAction.None)
                UpdateNotifications();
        }

        internal void UpdateNotifications()
        {
            if (_Appointment != null && !_IsActiveForPastAppointments && _Appointment.StartTime.Date < DateTime.Today)
            {
                UnregisterNotification();
                return;
            }

            if ((_Appointment != null && _Appointment.Calendar != null || _Parent != null && _Parent.ParentModel != null) &&
                _IsActive && _ReminderTime > DateTime.MinValue && _ReminderAction != eReminderAction.None)
            {
                NotificationRequest request = this.NotificationRequest;
                RegisterNotification(request);
            }
            else
            {
                UnregisterNotification();
            }
        }

        private void RegisterNotification(NotificationRequest request)
        {
            if (_NotificationRequestRegistered)
                NotificationServer.UpdateNotification(request);
            else
                NotificationServer.Register(request);
            _NotificationRequestRegistered = true;
        }

        private Appointment _Appointment;
        /// <summary>
        /// Gets the Appointment reminder is attached to.
        /// </summary>
        [Browsable(false)]
        public Appointment Appointment
        {
            get { return _Appointment; }
            internal set
            {
                if (_Appointment != value)
                {
                    _Appointment = value;
                    if (NeedsNotification) UpdateNotifications();
                }
            }
        }

        private bool _IsActive = true;
        /// <summary>
        /// Gets or sets whether reminder is active. Active reminders fire events or execute commands when 
        /// reminder time has been reached. Set this value to false to dismiss the reminder.
        /// </summary>
        [DefaultValue(true)]
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (_IsActive != value)
                {
                    bool oldValue = _IsActive;
                    _IsActive = value;
                    OnIsActiveChanged(oldValue, _IsActive);
                }
            }
        }

        private void OnIsActiveChanged(bool oldValue, bool newValue)
        {
            UpdateNotifications();
        }

        private NotificationRequest _NotificationRequest = null;
        private NotificationRequest NotificationRequest
        {
            get
            {
                if (_NotificationRequest == null)
                {
                    _NotificationRequest = new NotificationRequest(((_IsSnoozeReminder && _SnoozeDateTime != DateTime.MinValue) ? _SnoozeDateTime : _ReminderTime), new NotificationServerEventHandler(OnReminderNotification));
                }
                else
                    _NotificationRequest.NotificationTime = ((_IsSnoozeReminder && _SnoozeDateTime != DateTime.MinValue) ? _SnoozeDateTime : _ReminderTime);
                return _NotificationRequest;
            }
            set
            {
                _NotificationRequest = value;
            }
        }

        private DateTime _SnoozeDateTime = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the next snooze time for the reminder. Use the Snooze method if you want to snooze the reminder correctly.
        /// </summary>
        public DateTime SnoozeDateTime
        {
            get { return _SnoozeDateTime; }
            set
            {
                value = CalendarModel.GetCalendarDateTime(value);
                value = value.Kind == DateTimeKind.Utc ? value : GetUTCDateTime(value);
                if (value != _SnoozeDateTime)
                {
                    DateTime oldValue = _SnoozeDateTime;
                    _SnoozeDateTime = value;
                    if (_SnoozeDateTime == DateTime.MinValue)
                    {
                        this.IsSnoozeReminder = false;
                        UnregisterNotification();
                    }
                    OnSnoozeDateTimeChanged(oldValue, value);
                }
            }
        }

        private void OnSnoozeDateTimeChanged(DateTime oldValue, DateTime newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("SnoozeDateTime"));

        }

        /// <summary>
        /// Snoozes reminder so it occurs at specified notification time. This method should be used instead of the SnoozeDateTime property and it will
        /// set the SnoozeDateTime property to the next notification time.
        /// </summary>
        /// <param name="nextNotificationTime">Next reminder notification time.</param>
        public void Snooze(DateTime nextNotificationTime)
        {
            nextNotificationTime = CalendarModel.GetCalendarDateTime(nextNotificationTime);
            DateTime utcTime = nextNotificationTime.Kind == DateTimeKind.Utc ? nextNotificationTime : GetUTCDateTime(nextNotificationTime);
            UnregisterNotification();
            this.SnoozeDateTime = nextNotificationTime;
            this.IsSnoozeReminder = true;
            NotificationRequest request = this.NotificationRequest;
            request.NotificationTime = utcTime;
            RegisterNotification(request);
        }

        private void UnregisterNotification()
        {
            if (_NotificationRequestRegistered)
            {
                NotificationServer.Unregister(this.NotificationRequest);
                _NotificationRequestRegistered = false;
                this.NotificationRequest = null;
            }
        }

        private void OnReminderNotification(NotificationRequest request, NotificationServerEventArgs e)
        {
            if (_ReminderAction != eReminderAction.None)
            {
                this.NotificationRequest = null;
                ProcessNotification();
            }
        }

        /// <summary>
        /// Runs the ReminderNotification as if the reminder time has been reached.
        /// <remarks>This method is automatically called by reminder once ReminderTime has been reached.</remarks>
        /// </summary>
        public void ProcessNotification()
        {
            if (_Appointment != null && _Appointment.Calendar != null)
                _Appointment.Calendar.InvokeReminderNotification(this);
            else if (_Parent != null && _Parent.ParentModel != null)
                _Parent.ParentModel.InvokeReminderNotification(this);

            if (_ReminderAction == eReminderAction.None) return;

            if ((_ReminderAction & eReminderAction.Event) != 0)
            {
                OnReminderNotification(new ReminderEventArgs(this));
            }

            if ((_ReminderAction & eReminderAction.Command) != 0)
            {
                //throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Raises the ReminderNotification event.
        /// </summary>
        /// <param name="eventArgs"></param>
        protected virtual void OnReminderNotification(ReminderEventArgs eventArgs)
        {
            ReminderEventHandler h = ReminderNotification;
            if (h != null)
                h(this, eventArgs);
        }

        /// <summary>
        /// Creates an copy of the reminder.
        /// </summary>
        /// <returns>Reminder copy.</returns>
        public Reminder Copy()
        {
            Reminder copy = new Reminder();
            copy.Description = _Description;
            copy.Tag = _Tag;
            copy.IsActive = _IsActive;
            copy.ReminderAction = _ReminderAction;
            copy.IsActiveForPastAppointments = this.IsActiveForPastAppointments;
            if (ReminderNotification != null)
                copy.ReminderNotification = (ReminderEventHandler)ReminderNotification.Clone();
            copy.ReminderTime = ReminderTime;
            copy.IsSnoozeReminder = _IsSnoozeReminder;
            return copy;
        }

        private bool _IsSnoozeReminder = false;
        /// <summary>
        /// Gets or sets whether this reminder is snooze reminder usually created by Reminder dialog when user hits the Snooze button.
        /// Default value is false.
        /// </summary>
        public bool IsSnoozeReminder
        {
            get { return _IsSnoozeReminder; }
            set
            {
                _IsSnoozeReminder = value;
            }
        }

        private ReminderCollection _Parent = null;
        internal ReminderCollection ParentCollection
        {
            get { return _Parent; }
            set
            {
                _Parent = value;
                if (NeedsNotification)
                    UpdateNotifications();
            }
        }
        #endregion
    }

    #region Events Support
    public delegate void ReminderEventHandler(object sender, ReminderEventArgs e);
    /// <summary>
    /// Defines arguments for reminder related events.
    /// </summary>
    public class ReminderEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the reminder referenced by this event.
        /// </summary>
        public Reminder Reminder = null;

        /// <summary>
        /// Initializes a new instance of the ReminderEventArgs class.
        /// </summary>
        /// <param name="appointment"></param>
        public ReminderEventArgs(Reminder reminder)
        {
            Reminder = reminder;
        }
    }
    #endregion
}
#endif

