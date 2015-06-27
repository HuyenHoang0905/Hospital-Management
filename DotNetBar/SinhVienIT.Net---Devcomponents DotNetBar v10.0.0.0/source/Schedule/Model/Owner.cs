#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevComponents.DotNetBar.Schedule;

namespace DevComponents.Schedule.Model
{
    public class Owner : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Owner class.
        /// </summary>
        public Owner()
        {
            _WorkDays = new WorkDayCollection(this);
            _CalendarWorkDays = new CalendarWorkDayCollection(this);
        }

        /// <summary>
        /// Initializes a new instance of the Owner class.
        /// </summary>
        /// <param name="key"></param>
        public Owner(string key)
            : this()
        {
            _Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the Owner class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="displayName"></param>
        public Owner(string key, string displayName)
            : this()
        {
            _Key = key;
            _DisplayName = displayName;
        }

        /// <summary>
        /// Initializes a new instance of the Owner class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="displayName"></param>
        /// <param name="colorScheme"></param>
        public Owner(string key, string displayName, eCalendarColor colorScheme)
            : this()
        {
            _Key = key;
            _ColorScheme = colorScheme;
            _DisplayName = displayName;
        }

        /// <summary>
        /// Initializes a new instance of the Owner class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="colorScheme"></param>
        public Owner(string key, eCalendarColor colorScheme)
            : this()
        {
            _Key = key;
            _ColorScheme = colorScheme;
        }
        #endregion

        #region Internal Implementation
        private WorkDayCollection _WorkDays;
        /// <summary>
        /// Gets working days associated with this owner. If empty WorkDays from CalendarModel are used instead.
        /// </summary>
        public WorkDayCollection WorkDays
        {
            get { return _WorkDays; }
        }

        private CalendarWorkDayCollection _CalendarWorkDays;
        /// <summary>
        /// Gets date based working days associated with this owner. Date specific working days take precedence over days specified in WorkDays collection. If empty WorkDays on owner or from CalendarModel are used instead.
        /// </summary>
        public CalendarWorkDayCollection CalendarWorkDays
        {
            get { return _CalendarWorkDays; }
        }

        private string _Key = "";
        /// <summary>
        /// Gets or sets the unique key that identifies the owner.
        /// </summary>
        [DefaultValue("")]
        public string Key
        {
            get { return _Key; }
            set
            {
                if (value != _Key)
                {
                    string oldValue = _Key;
                    _Key = value;
                    OnKeyChanged(oldValue, value);
                }
            }
        }

        private void OnKeyChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Key"));
        }

        private string _Description = "";
        /// <summary>
        /// Gets or sets the owner description. For example if owner represents person, it would be person name or if owner represents resource
        /// like room it would be the room name.
        /// </summary>
        [DefaultValue("")]
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    string oldValue = _Description;
                    _Description = value;
                    OnDescriptionChanged(oldValue, value);
                }
            }
        }

        private void OnDescriptionChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            
        }

        private object _Tag = null;
        /// <summary>
        /// Gets or sets custom data associated with the object.
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

        private CalendarModel _Calendar = null;
        /// <summary>
        /// Gets the calendar owner is associated with.
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
                }
            }
        }

        private TimeZoneInfo _DisplayTimeZone = null;
        /// <summary>
        /// Gets or sets the display time-zone for this owner. Default value is null.
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
                    OnDisplayTimeZoneChanged(oldValue, value);
                }
            }
        }

        private void OnDisplayTimeZoneChanged(TimeZoneInfo oldValue, TimeZoneInfo newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("DisplayTimeZone"));
            
        }


        private eCalendarColor _ColorScheme = eCalendarColor.Automatic;
        /// <summary>
        /// Gets or sets the owner color scheme used to represent owner data in user interface.
        /// </summary>
        [DefaultValue(eCalendarColor.Automatic)]
        public eCalendarColor ColorScheme
        {
            get { return _ColorScheme; }
            set
            {
                if (value != _ColorScheme)
                {
                    eCalendarColor oldValue = _ColorScheme;
                    _ColorScheme = value;
                    OnColorSchemeChanged(oldValue, value);
                }
            }
        }

        private void OnColorSchemeChanged(eCalendarColor oldValue, eCalendarColor newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ColorScheme"));
            
        }
        private string _DisplayName = null;
        /// <summary>
        /// Gets or sets the display name for the owner. Display name is used in User Interface to identify the owner.
        /// If not specified the Key is used instead in UI.
        /// </summary>
        [DefaultValue(null)]
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                if (value != _DisplayName)
                {
                    string oldValue = _DisplayName;
                    _DisplayName = value;
                    OnDisplayNameChanged(oldValue, value);
                }
            }
        }

        private void OnDisplayNameChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("DisplayName"));
            
        }

        internal static string DisplayNamePropertyName = "DisplayName";
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
}
#endif

