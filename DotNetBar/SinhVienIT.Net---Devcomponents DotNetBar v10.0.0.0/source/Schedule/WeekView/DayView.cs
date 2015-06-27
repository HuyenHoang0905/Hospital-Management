#if FRAMEWORK20
using System;

namespace DevComponents.DotNetBar.Schedule
{
    public class DayView : WeekDayView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView"></param>
        public DayView(CalendarView calendarView)
            : base(calendarView, eCalendarView.Day)
        {
        }

        #region RecalcSize support

        /// <summary>
        /// Normalizes the user specified start and end dates
        /// </summary>
        /// <param name="startDate">[out] Normalized start date</param>
        /// <param name="endDate">[out] Normalized end date</param>
        protected override void NormalizeDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = this.StartDate;

            // If both values are unset, then set them to
            // today's date

            if (startDate == DateTime.MinValue)
                startDate = DateTime.Today.Date;

            endDate = startDate;

            DaysOfTheWeek = new DaysOfTheWeek(startDate.DayOfWeek, 1);
        }

        #endregion

    }
}
#endif

