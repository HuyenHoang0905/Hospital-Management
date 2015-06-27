#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents working day in calendar.
    /// </summary>
    public class WorkDay : BaseWorkDay
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the WorkDay class.
        /// </summary>
        public WorkDay()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WorkDay class.
        /// </summary>
        /// <param name="dayOfWeek"></param>
        public WorkDay(DayOfWeek dayOfWeek)
        {
            _DayOfWeek = dayOfWeek;
        }

        /// <summary>
        /// Initializes a new instance of the WorkDay class.
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <param name="workStartTime"></param>
        /// <param name="workEndTime"></param>
        public WorkDay(DayOfWeek dayOfWeek, WorkTime workStartTime, WorkTime workEndTime)
        {
            _DayOfWeek = dayOfWeek;
            _WorkStartTime = workStartTime;
            _WorkEndTime = workEndTime;
        }

        private DayOfWeek _DayOfWeek = DayOfWeek.Monday;
        /// <summary>
        /// Gets or sets the day of week this instance represents.
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get { return _DayOfWeek; }
            set
            {
                if (value != _DayOfWeek)
                {
                    DayOfWeek oldValue = _DayOfWeek;
                    _DayOfWeek = value;
                    OnDayOfWeekChanged(oldValue, value);
                }
            }
        }

        private void OnDayOfWeekChanged(DayOfWeek oldValue, DayOfWeek newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("DayOfWeek"));
        }
        #endregion
    }
}
#endif

