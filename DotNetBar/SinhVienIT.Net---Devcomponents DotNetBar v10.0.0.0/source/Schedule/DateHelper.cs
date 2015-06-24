#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Globalization;

namespace DevComponents.DotNetBar.Schedule
{
    internal static class DateHelper
    {
        #region Date/Time Helpers

        /// <summary>
        /// Returns the date that is 30 minutes before or after input date if input date minute is 0 or 30. Otherwise it returns next increment to 0 or 30.
        /// </summary>
        /// <param name="date">Date and time.</param>
        /// <param name="forward">Indicates whether to add or subtract minutes.</param>
        /// <returns>New date time.</returns>
        public static DateTime GetNext30Minute(DateTime date, bool forward)
        {
            if (date.Minute == 0 || date.Minute == 30)
                return (date.AddMinutes(forward ? 30 : -30));

            return (date.AddMinutes((forward ? 1 : -1) * Math.Abs(30 - date.Minute)));
        }

        /// <summary>
        /// Returns date that starts with the day. If passed date is not on the requested date function returns first date with day that is before passed date.
        /// </summary>
        /// <param name="date">Date to inspect.</param>
        /// <param name="dayOfWeek">Day of week</param>
        /// <returns>Date that starts on given day of week.</returns>
        public static DateTime GetDateForDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            if (date.DayOfWeek != dayOfWeek)
            {
                // Go back to the first day of week
                while (date.DayOfWeek != dayOfWeek)
                {
                    date = date.AddDays(-1);
                }
            }

            return (date);
        }

        public static DateTime GetEndOfWeek(DateTime date, DayOfWeek lastDayOfWeek)
        {
            while (date.DayOfWeek != lastDayOfWeek)
                date = date.AddDays(1);

            return (date);
        }

        internal static DayOfWeek GetFirstDayOfWeek()
        {
            return (ScheduleSettings.GetActiveCulture().DateTimeFormat.FirstDayOfWeek);
        }

        internal static DayOfWeek GetLastDayOfWeek()
        {
            DayOfWeek firstDay = GetFirstDayOfWeek();

            int lastDay = (int)firstDay + 6;

            if (lastDay > 6) 
                lastDay = lastDay - 7;

            return (DayOfWeek)lastDay;
        }

        /// <summary>
        /// Returns whether two days fall on same month and year.
        /// </summary>
        /// <param name="date1">First date</param>
        /// <param name="date2">Second date</param>
        /// <returns>true if dates are on same month and year</returns>
        public static bool IsSameMonth(DateTime date1, DateTime date2)
        {
            return (date1.Year == date2.Year && date1.Month == date2.Month);
        }

        public static DayOfWeek GetNextDay(DayOfWeek currentDay)
        {
            if ((int)currentDay == 6) 
                return (DayOfWeek)0;

            return (DayOfWeek)(currentDay + 1);
        }

        /// <summary>
        /// Returns true if time periods overlap.
        /// </summary>
        /// <param name="startTime1">Start of first period.</param>
        /// <param name="endTime1">End of first period.</param>
        /// <param name="startTime2">Start of second period.</param>
        /// <param name="endTime2">End of second period.</param>
        /// <returns>true if periods overlap</returns>
        public static bool TimePeriodsOverlap(DateTime startTime1, DateTime endTime1, DateTime startTime2, DateTime endTime2)
        {
            return (startTime1 <= startTime2 && endTime1 > startTime2 ||
                startTime1 >= startTime2 && startTime1 < endTime2);
        }

        public static int GetNumberOfWeeks(DateTime startDate, DateTime endDate)
        {
            if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            {
                startDate = DateTime.Today.Date;
                endDate = startDate.AddMonths(1);
            }

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue || endDate < startDate)
            {
                return (0);
            }

            endDate = GetEndOfWeek(endDate, DateHelper.GetLastDayOfWeek());
            startDate = GetDateForDayOfWeek(startDate, DateHelper.GetFirstDayOfWeek());

            return ((int)Math.Max(Math.Ceiling(endDate.Subtract(startDate).TotalDays / 7), 1));
        }

        /// <summary>
        /// Gets the abbreviated month name for
        /// the given date
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>Abbreviated name</returns>
        public static string GetThreeLetterMonth(DateTime date)
        {
            CultureInfo ci = ScheduleSettings.GetActiveCulture();

            return (ci.DateTimeFormat.GetAbbreviatedMonthName(date.Month));
        }

        /// <summary>
        /// Gets the abbreviated day name for
        /// the given date
        /// </summary>
        /// <param name="dayOfWeek">Day of week</param>
        /// <returns>Abbreviated name</returns>
        public static string GetThreeLetterDayOfWeek(DayOfWeek dayOfWeek)
        {
            CultureInfo ci = ScheduleSettings.GetActiveCulture();

            return (ci.DateTimeFormat.GetAbbreviatedDayName(dayOfWeek));
        }

        #endregion
    }
}
#endif
