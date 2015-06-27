using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents specific date based work day.
    /// </summary>
    public class CalendarWorkDay : BaseWorkDay
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the WorkDay class.
        /// </summary>
        public CalendarWorkDay()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WorkDay class.
        /// </summary>
        /// <param name="date">Date this work-day represents</param>
        public CalendarWorkDay(DateTime date)
        {
            _Date = date;
        }

        private DateTime _Date = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the date this day represents.
        /// </summary>
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                value = value.Date;
                if (value != _Date)
                {
                    DateTime oldValue = _Date;
                    _Date = value;
                    OnDateChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when Date property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value.</param>
        /// <param name="newValue">New property value.</param>
        protected virtual void OnDateChanged(DateTime oldValue, DateTime newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Date"));
        }
        #endregion
    }
}
