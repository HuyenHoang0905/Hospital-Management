#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Defines the daily recurrence settings.
    /// </summary>
    public class DailyRecurrenceSettings : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Internal Implementation
        private AppointmentRecurrence _Recurrence = null;

        /// <summary>
        /// Initializes a new instance of the DailyRecurrenceSettings class.
        /// </summary>
        /// <param name="recurrence"></param>
        public DailyRecurrenceSettings(AppointmentRecurrence recurrence)
        {
            _Recurrence = recurrence;
        }

        private eDailyRecurrenceRepeat _RepeatOnDaysOfWeek = eDailyRecurrenceRepeat.All;
        /// <summary>
        /// Gets or sets the days of week on which appointment is repeated.
        /// </summary>
        [DefaultValue(eDailyRecurrenceRepeat.All)]
        public eDailyRecurrenceRepeat RepeatOnDaysOfWeek
        {
            get { return _RepeatOnDaysOfWeek; }
            set
            {
                if (value != _RepeatOnDaysOfWeek)
                {
                    eDailyRecurrenceRepeat oldValue = _RepeatOnDaysOfWeek;
                    _RepeatOnDaysOfWeek = value;
                    OnRepeatOnDaysOfWeekChanged(oldValue, value);
                }
            }
        }

        private void OnRepeatOnDaysOfWeekChanged(eDailyRecurrenceRepeat oldValue, eDailyRecurrenceRepeat newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RepeatOnDaysOfWeek"));
        }

        private int _RepeatInterval = 1;
        /// <summary>
        /// Gets or sets the interval between recurring appointments. Default value is 1. Setting this value to for example 3 means that
        /// recurrence is repeated every 3 days.
        /// </summary>
        [DefaultValue(1)]
        public int RepeatInterval
        {
            get { return _RepeatInterval; }
            set
            {
                if (value != _RepeatInterval)
                {
                    int oldValue = _RepeatInterval;
                    _RepeatInterval = value;
                    OnRepeatIntervalChanged(oldValue, value);
                }
            }
        }

        private void OnRepeatIntervalChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RepeatInterval"));
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

