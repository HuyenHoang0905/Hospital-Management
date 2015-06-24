#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Defines monthly recurrence settings.
    /// </summary>
    public class MonthlyRecurrenceSettings : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Internal Implementation
        private AppointmentRecurrence _Recurrence = null;
        /// <summary>
        /// Initializes a new instance of the MonthlyRecurrenceSettings class.
        /// </summary>
        /// <param name="recurrence"></param>
        public MonthlyRecurrenceSettings(AppointmentRecurrence recurrence)
        {
            _Recurrence = recurrence;
        }


        private int _RepeatOnDayOfMonth = 1;
        /// <summary>
        /// Gets or sets the day of month on which appointment is repeated.
        /// When RepeatOnRelativeDayInMonth property is set to value other than None value of this property is not used.
        /// </summary>
        [DefaultValue(1)]
        public int RepeatOnDayOfMonth
        {
            get { return _RepeatOnDayOfMonth; }
            set
            {
                if (value != _RepeatOnDayOfMonth)
                {
                    int oldValue = _RepeatOnDayOfMonth;
                    _RepeatOnDayOfMonth = value;
                    OnRepeatOnDayOfMonthChanged(oldValue, value);
                }
            }
        }

        private void OnRepeatOnDayOfMonthChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RepeatOnDayOfMonth"));
        }


        private eRelativeDayInMonth _RepeatOnRelativeDayInMonth = eRelativeDayInMonth.None;
        /// <summary>
        /// Gets or sets whether appointment should repeat on first, second, third, fourth or last day in month as specified
        /// by RepeatOnDayOfMonth property. Property applies only for RecurrenceType Monthly or Yearly.
        /// </summary>
        [DefaultValue(eRelativeDayInMonth.None)]
        public eRelativeDayInMonth RepeatOnRelativeDayInMonth
        {
            get { return _RepeatOnRelativeDayInMonth; }
            set
            {
                if (value != _RepeatOnRelativeDayInMonth)
                {
                    eRelativeDayInMonth oldValue = _RepeatOnRelativeDayInMonth;
                    _RepeatOnRelativeDayInMonth = value;
                    OnRepeatOnRelativeDayInMonthChanged(oldValue, value);
                }
            }
        }

        private void OnRepeatOnRelativeDayInMonthChanged(eRelativeDayInMonth oldValue, eRelativeDayInMonth newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RepeatOnRelativeDayInMonth"));
        }

        private int _RepeatInterval = 1;
        /// <summary>
        /// Gets or sets the interval between recurring appointments. Default value is 1.
        /// <remarks>
        /// For example, setting RepeatInterval to 2 means that appointment will recur every 2 months.
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

        private DayOfWeek _RelativeDayOfWeek = DayOfWeek.Monday;
        /// <summary>
        /// Gets or sets the day of week on which relative repeat as specified by RepeatOnRelativeDayInMonth is effective.
        /// For example setting RepeatOnRelativeDayInMonth to First and RelativeDayOfWeek to Monday will repeat the appointment on first
        /// Monday in a month.
        /// </summary>
        [DefaultValue(DayOfWeek.Monday)]
        public DayOfWeek RelativeDayOfWeek
        {
            get { return _RelativeDayOfWeek; }
            set
            {
                if (value != _RelativeDayOfWeek)
                {
                    DayOfWeek oldValue = _RelativeDayOfWeek;
                    _RelativeDayOfWeek = value;
                    OnRelativeDayOfWeekChanged(oldValue, value);
                }
            }
        }

        private void OnRelativeDayOfWeekChanged(DayOfWeek oldValue, DayOfWeek newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RelativeDayOfWeek"));
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

