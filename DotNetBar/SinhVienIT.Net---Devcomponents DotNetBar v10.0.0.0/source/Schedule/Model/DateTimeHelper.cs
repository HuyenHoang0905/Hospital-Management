#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Schedule.Model
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns number of weekdays (Monday-Friday) between two dates.
        /// </summary>
        /// <param name="startDateTime">Start date</param>
        /// <param name="endDateTime">End date</param>
        /// <returns>Total number of weekdays between two dates</returns>
        public static int TotalWeekDays(DateTime startDateTime, DateTime endDateTime)
        {
            int totalDays = 0;
            startDateTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day);
            endDateTime = new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, 23, 59, 0);
            
            // Start date to monday
            if (startDateTime.DayOfWeek == DayOfWeek.Saturday)
                startDateTime = startDateTime.AddDays(2);
            else if (startDateTime.DayOfWeek == DayOfWeek.Sunday)
                startDateTime = startDateTime.AddDays(1);
            else
            {
                DateTime newStartDateTime = startDateTime.AddDays(8 - (int)startDateTime.DayOfWeek);
                if (newStartDateTime > endDateTime)
                {
                    if (endDateTime.DayOfWeek == DayOfWeek.Saturday)
                        endDateTime = endDateTime.AddDays(-1);
                    else if (endDateTime.DayOfWeek == DayOfWeek.Sunday)
                        endDateTime = endDateTime.AddDays(-2);
                    return (int)Math.Ceiling(Math.Max(0, endDateTime.Subtract(startDateTime).TotalDays));
                }
                totalDays = 6 - (int)startDateTime.DayOfWeek;
                startDateTime = newStartDateTime;
            }

            // End date to Sunday
            if (endDateTime.DayOfWeek == DayOfWeek.Saturday)
                endDateTime = endDateTime.AddDays(1);
            else if (endDateTime.DayOfWeek != DayOfWeek.Sunday)
            {
                int d = (int)endDateTime.DayOfWeek;
                DateTime newEndDateTime = endDateTime.AddDays(-(int)endDateTime.DayOfWeek);
                totalDays += d;
                endDateTime=newEndDateTime;
            }

            int t = (int)Math.Max(0, Math.Ceiling(endDateTime.Subtract(startDateTime).TotalDays));
            if (t > 0)
                totalDays += (t / 7) * 5;

            return totalDays;
            
        }

        /// <summary>
        /// Return total number of days specified by day parameter between two dates.
        /// </summary>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <param name="day">Day of week</param>
        /// <returns>Number of days between two dates</returns>
        public static int TotalDays(DateTime startDate, DateTime endDate, DayOfWeek day)
        {
            if (endDate < startDate) return 0;


            if (startDate.DayOfWeek > day)
                startDate = startDate.AddDays(7 - ((int)startDate.DayOfWeek - (int)day));
            else
                startDate = startDate.AddDays(day - startDate.DayOfWeek);
            if (endDate < startDate) return 0;

            if (endDate.DayOfWeek < day)
                endDate = endDate.AddDays(-(7 - ((int)day - (int)endDate.DayOfWeek)));
            else
                endDate = endDate.AddDays(day - endDate.DayOfWeek);

            if (endDate.Subtract(startDate).TotalDays <= 1) return 1;

            int t = (int)Math.Ceiling(endDate.Subtract(startDate).TotalDays);
            int totalDays = (int)Math.Floor(Math.Max(1, (double)t / 7)) + 1; // +1 since endDate is always on the day
            //if (endDate.RelativeDayOfWeek == day) totalDays++;
            return totalDays;
        }

        public static int TotalNumberOfDays(DateTime startDate, DateTime endDate, eDayOfWeekRecurrence daysOfWeek)
        {
            int totalDays=0;
            if (daysOfWeek == eDayOfWeekRecurrence.All)
                return (int)Math.Ceiling(endDate.Subtract(startDate).TotalDays);

            if ((daysOfWeek & eDayOfWeekRecurrence.Friday) != 0)
                totalDays += TotalDays(startDate, endDate, DayOfWeek.Friday);
            if ((daysOfWeek & eDayOfWeekRecurrence.Monday) != 0)
                totalDays += TotalDays(startDate, endDate, DayOfWeek.Monday);
            if ((daysOfWeek & eDayOfWeekRecurrence.Saturday) != 0)
                totalDays += TotalDays(startDate, endDate, DayOfWeek.Saturday);
            if ((daysOfWeek & eDayOfWeekRecurrence.Sunday) != 0)
                totalDays += TotalDays(startDate, endDate, DayOfWeek.Sunday);
            if ((daysOfWeek & eDayOfWeekRecurrence.Thursday) != 0)
                totalDays += TotalDays(startDate, endDate, DayOfWeek.Thursday);
            if ((daysOfWeek & eDayOfWeekRecurrence.Tuesday) != 0)
                totalDays += TotalDays(startDate, endDate, DayOfWeek.Tuesday);
            if ((daysOfWeek & eDayOfWeekRecurrence.Wednesday) != 0)
                totalDays += TotalDays(startDate, endDate, DayOfWeek.Wednesday);

            return totalDays;
        }

        /// <summary>
        /// Returns the date/time that represents end of the day value.
        /// </summary>
        /// <param name="_DayDate"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(DateTime day)
        {
            return new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);
        }

        /// <summary>
        /// Returns the date/time that represents beginning of the day value.
        /// </summary>
        /// <param name="_DayDate"></param>
        /// <returns></returns>
        public static DateTime BeginningOfDay(DateTime day)
        {
            return new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
        }

        /// <summary>
        /// Returns true if date falls at begging of the day 12:00 AM
        /// </summary>
        /// <param name="_DayDate"></param>
        /// <returns></returns>
        public static bool IsBeginningOfDay(DateTime day)
        {
            return day.Equals(BeginningOfDay(day));
        }

        internal static bool HasDay(DayOfWeek dayOfWeek, eDayOfWeekRecurrence days)
        {
            if (dayOfWeek == DayOfWeek.Monday && (days & eDayOfWeekRecurrence.Monday) != 0) return true;
            if (dayOfWeek == DayOfWeek.Tuesday && (days & eDayOfWeekRecurrence.Tuesday) != 0) return true;
            if (dayOfWeek == DayOfWeek.Wednesday && (days & eDayOfWeekRecurrence.Wednesday) != 0) return true;
            if (dayOfWeek == DayOfWeek.Thursday && (days & eDayOfWeekRecurrence.Thursday) != 0) return true;
            if (dayOfWeek == DayOfWeek.Friday && (days & eDayOfWeekRecurrence.Friday) != 0) return true;
            if (dayOfWeek == DayOfWeek.Saturday && (days & eDayOfWeekRecurrence.Saturday) != 0) return true;
            if (dayOfWeek == DayOfWeek.Sunday && (days & eDayOfWeekRecurrence.Sunday) != 0) return true;

            return false;
        }

        public static bool IsWeekendDay(DateTime currentDay)
        {
            return currentDay.DayOfWeek == DayOfWeek.Sunday || currentDay.DayOfWeek == DayOfWeek.Saturday;
        }

        /// <summary>
        /// Gets greater date between two dates.
        /// </summary>
        /// <param name="date1">Date 1</param>
        /// <param name="date2">Date 2</param>
        /// <returns>Greater date.</returns>
        public static DateTime MaxDate(DateTime date1, DateTime date2)
        {
            return date1 > date2 ? date1 : date2;
        }

        /// <summary>
        /// Returns true if both dates are on same day and year.
        /// </summary>
        /// <param name="date1">First date</param>
        /// <param name="date2">Second date</param>
        /// <returns>true if dates are on same day and year</returns>
        public static bool IsSameDay(DateTime date1, DateTime date2)
        {
            return (date1.Year == date2.Year && date1.Month == date2.Month &&
                date1.Day == date2.Day) || date1.Date.AddDays(1) == date2;

            //return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
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
            if (startTime1 <= startTime2 && endTime1 > startTime2 || startTime1 >= startTime2 && startTime1 < endTime2)
                return true;
            return false;
        }
    }
}
#endif

