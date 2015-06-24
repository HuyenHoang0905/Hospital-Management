#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Defines weekly recurrence settings.
    /// </summary>
    public class WeeklyRecurrenceSettings : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Internal Implementation
        private AppointmentRecurrence _Recurrence = null;
        /// <summary>
        /// Initializes a new instance of the WeeklyRecurrenceSettings class.
        /// </summary>
        /// <param name="recurrence"></param>
        public WeeklyRecurrenceSettings(AppointmentRecurrence recurrence)
        {
            _Recurrence = recurrence;
        }


        private eDayOfWeekRecurrence _RepeatOnDaysOfWeek = eDayOfWeekRecurrence.All;
        /// <summary>
        /// Gets or sets the days of week on which appointment is repeated. This property is represented by bit-flag enum
        /// which means that you can combine the values from eDayOfWeekRecurrence enum using OR operator to specify multiple values.
        /// Default value is All.
        /// <remarks>
        /// <para>This property value cannot be set to eDayOfWeekRecurrence.None.</para> 
        /// </remarks>
        /// </summary>
        [DefaultValue(eDayOfWeekRecurrence.All)]
        public eDayOfWeekRecurrence RepeatOnDaysOfWeek
        {
            get { return _RepeatOnDaysOfWeek; }
            set
            {
                if (value == eDayOfWeekRecurrence.None)
                    throw new ArgumentException("RepeatOnDaysOfWeek cannot be set to eDayOfWeekRecurrence.None");

                if (value != _RepeatOnDaysOfWeek)
                {
                    eDayOfWeekRecurrence oldValue = _RepeatOnDaysOfWeek;
                    _RepeatOnDaysOfWeek = value;
                    OnRepeatOnDaysOfWeekChanged(oldValue, value);
                }
            }
        }

        private void OnRepeatOnDaysOfWeekChanged(eDayOfWeekRecurrence oldValue, eDayOfWeekRecurrence newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RepeatOnDaysOfWeek"));
        }

        private int _RepeatInterval = 1;
        /// <summary>
        /// Gets or sets the interval between recurring appointments. Default value is 1.
        /// <remarks>
        /// For example, setting RepeatInterval to 2 means that appointment will recur every 2 weeks.
        /// </remarks>
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

