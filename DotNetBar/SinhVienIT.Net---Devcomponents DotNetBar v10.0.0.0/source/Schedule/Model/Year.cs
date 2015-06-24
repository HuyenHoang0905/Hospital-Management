#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents the calendar year.
    /// </summary>
    public class Year
    {
        #region Internal Implementation
        private int _Year = 0;
        private CalendarModel _CalendarModel = null;
        private ReadOnlyCollection<Month> _ReadOnlyMonths = null;
        private List<Month> _Months = null;

        /// <summary>
        /// Initializes a new instance of the Year class.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="calendarModel"></param>
        public Year(int year, CalendarModel calendarModel)
        {
            _Year = year;
            _CalendarModel = calendarModel;
        }

        /// <summary>
        /// Returns read-only collection of months in year.
        /// </summary>
        public ReadOnlyCollection<Month> Months
        {
            get
            {
                if (_ReadOnlyMonths == null)
                    CreateCollection();
                return _ReadOnlyMonths;
            }
        }

        private void CreateCollection()
        {
            _Months = new List<Month>(12);
            for (int i = 0; i < 12; i++)
            {
                _Months.Add(new Month(_CalendarModel, _Year, i + 1));
            }
            _ReadOnlyMonths = new ReadOnlyCollection<Month>(_Months);
        }

        internal void InvalidateAppointments()
        {
            if (_Months == null) return;
            foreach (Month month in _Months)
            {
                month.InvalidateAppointments();
            }
        }

        internal void InvalidateAppointments(int month, int day)
        {
            if (_Months == null) return;
            _Months[month - 1].InvalidateAppointments(day);
        }
        #endregion
    }
}
#endif

