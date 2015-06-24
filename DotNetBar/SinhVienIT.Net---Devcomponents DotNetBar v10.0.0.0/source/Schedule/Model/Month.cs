#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents the calendar month.
    /// </summary>
    public class Month
    {
        #region Private Variables
        private int _Year = 0, _Month = 0;
        private ReadOnlyCollection<Day> _ReadOnlyDays = null;
        private List<Day> _Days = null;
        private CalendarModel _CalendarModel = null;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the Month class.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public Month(CalendarModel calendar, int year, int month)
        {
            _CalendarModel = calendar;
            _Year = year;
            _Month = month;
        }

        /// <summary>
        /// Gets collection of days in this month.
        /// </summary>
        public ReadOnlyCollection<Day> Days
        {
            get
            {
                if (_ReadOnlyDays == null)
                    CreateDayCollection();
                return _ReadOnlyDays;
            }
        }

        private void CreateDayCollection()
        {
            Calendar cal = _CalendarModel.GetCalendar();
            int daysCount = cal.GetDaysInMonth(_Year, _Month);
            _Days = new List<Day>(daysCount);
            for (int i = 0; i < daysCount; i++)
            {
                Day day = new Day(new DateTime(_Year, _Month, i + 1), _CalendarModel);
  
                _Days.Add(day);
            }
            _ReadOnlyDays = new ReadOnlyCollection<Day>(_Days);
        }

        /// <summary>
        /// Gets the month year.
        /// </summary>
        [Browsable(false)]
        public int Year
        {
            get { return _Year; }
        }

        /// <summary>
        /// Gets the month.
        /// </summary>
        [Browsable(false)]
        public int MonthOfYear
        {
            get { return _Month; }
        }

        /// <summary>
        /// Gets the Calendar this day is part of.
        /// </summary>
        [Browsable(false)]
        public CalendarModel CalendarModel
        {
            get { return _CalendarModel; }
            internal set { _CalendarModel = value; }
        }

        internal void InvalidateAppointments()
        {
            if (_Days == null) return;

            foreach (Day day in _Days)
            {
                day.InvalidateAppointments();
            }
        } 
        
        internal void InvalidateAppointments(int day)
        {
            if (_Days == null) return;

            _Days[day - 1].InvalidateAppointments();
        }
        #endregion



    }
}
#endif

