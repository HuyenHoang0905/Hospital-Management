#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents the calendar day.
    /// </summary>
    public class Day
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Day class.
        /// </summary>
        /// <param name="dayDate"></param>
        /// <param name="calendar"></param>
        public Day(DateTime dayDate, CalendarModel calendar)
        {
            _DayDate = dayDate;
            _Calendar = calendar;
        }

        #endregion

        #region Internal Implementation
        private AppointmentSubsetCollection _Appointments;
        /// <summary>
        /// Gets appointments that start on this day.
        /// </summary>
        public AppointmentSubsetCollection Appointments
        {
            get 
            {
                if (_Appointments == null)
                    _Appointments = new AppointmentSubsetCollection(_Calendar, _DayDate, DateTimeHelper.EndOfDay(_DayDate));
                return _Appointments; 
            }
        }

        /// <summary>
        /// Invalidate the day appointments
        /// </summary>
        internal void InvalidateAppointments()
        {
            if (_Appointments != null)
                _Appointments.InvalidateCollection();
        }

        private DateTime _DayDate = DateTime.MinValue;
        /// <summary>
        /// Gets the date this day represents.
        /// </summary>
        [Browsable(false)]
        public DateTime DayDate
        {
            get { return _DayDate; }
            internal set { _DayDate = value; }
        }

        private CalendarModel _Calendar;
        /// <summary>
        /// Gets the Calendar this day is part of.
        /// </summary>
        [Browsable(false)]
        public CalendarModel Calendar
        {
            get { return _Calendar; }
            internal set { _Calendar = value; }
        }
        #endregion
    }
}
#endif

