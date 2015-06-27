#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace DevComponents.Schedule.Model
{
    internal class RecurrenceGenerator : IRecurrenceGenerator
    {

        #region IRecurrenceGenerator Members

        /// <summary>
        /// Generates Daily recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Daily recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        public void GenerateDailyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate)
        {
            Appointment app = recurrence.Appointment;
            int appointmentDaysDuration = (int)Math.Max(0, Math.Ceiling(app.EndTime.Date.Subtract(app.StartTime.Date).TotalDays));
            DateTime recurrenceStartDate = DateTimeHelper.MaxDate(recurrence.RecurrenceStartDate, DateTimeHelper.IsBeginningOfDay(app.EndTime) ? app.EndTime : app.EndTime.AddDays(recurrence.Daily.RepeatInterval).Date);

            if (recurrenceStartDate > endDate) return;

            int repeats = 0;

            // Check the range first
            if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate)
            {
                if (startDate > recurrence.RangeEndDate) return;
            }
            else if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences)
            {
                DateTime rangeStartDate = DateTimeHelper.MaxDate(recurrence.RecurrenceStartDate, DateTimeHelper.IsBeginningOfDay(app.LocalEndTime) ? app.LocalEndTime : app.LocalEndTime.AddDays(recurrence.Daily.RepeatInterval).Date);
                int totalDays = (int)Math.Ceiling(startDate.Subtract(rangeStartDate).TotalDays);

                switch (recurrence.Daily.RepeatOnDaysOfWeek)
                {
                    case eDailyRecurrenceRepeat.All:
                        repeats = Math.Max(0, totalDays / (recurrence.Daily.RepeatInterval + appointmentDaysDuration));

                        if (repeats >= recurrence.RangeNumberOfOccurrences)
                            return;
                        break;

                    case eDailyRecurrenceRepeat.WeekDays:
                        repeats = Math.Max(0, DateTimeHelper.TotalWeekDays(recurrenceStartDate, startDate));

                        // Assume weekdays repeat
                        if (repeats > recurrence.RangeNumberOfOccurrences)
                            return;
                        break;

                    default:
                        repeats = Math.Max(0, DateTimeHelper.TotalWeekDays(recurrenceStartDate, startDate));
                        repeats = Math.Max(0, totalDays - repeats);

                        // Assume weekend days repeat
                        if (repeats > recurrence.RangeNumberOfOccurrences)
                            return;
                        break;
                }

                //repeats = 0;
            }

            DateTime currentDay = recurrenceStartDate; // DateTimeHelper.MaxDate(startDate, recurrenceStartDate);

            while (currentDay <= endDate)
            {
                if (currentDay >= startDate || currentDay < startDate && IsRecurringOnDay(currentDay, startDate, endDate, app))
                {
                    if (!IsRecurringOnDay(currentDay, startDate, endDate, app)) break;
                    if (recurrence.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All ||
                        (recurrence.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.WeekDays && !DateTimeHelper.IsWeekendDay(currentDay)) ||
                        (recurrence.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.WeekendDays && DateTimeHelper.IsWeekendDay(currentDay)))
                    {
                        if (!IsIgnoredRecurrence(currentDay, recurrence))
                        {
                            subsetCollection.Add(CreateRecurringAppointmentInstance(currentDay, app));
                            repeats++;
                        }
                    }
                }

                switch (recurrence.Daily.RepeatOnDaysOfWeek)
                {
                    case eDailyRecurrenceRepeat.All:
                        currentDay = currentDay.AddDays(recurrence.Daily.RepeatInterval + appointmentDaysDuration);
                        break;

                    case eDailyRecurrenceRepeat.WeekDays:
                        //currentDay = currentDay.AddDays(currentDay.DayOfWeek == DayOfWeek.Saturday ? 2 : Math.Max(1, recurrence.Daily.RepeatInterval + appointmentDaysDuration));
                        currentDay = currentDay.AddDays(Math.Max(1, recurrence.Daily.RepeatInterval + appointmentDaysDuration)); // Changed for consistency
                        break;

                    case eDailyRecurrenceRepeat.WeekendDays:
                        while (true)
                        {
                            currentDay = currentDay.AddDays(1);

                            if (DateTimeHelper.IsWeekendDay(currentDay) == true)
                                break;
                        }
                        break;
                }

                if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate && currentDay > recurrence.RangeEndDate ||
                    recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && repeats >= recurrence.RangeNumberOfOccurrences)
                    break;
            }
        }

        private static bool IsIgnoredRecurrence(DateTime currentDay, AppointmentRecurrence recurrence)
        {
            if (recurrence.SkippedRecurrences.Count == 0) return false;
            foreach (DateTime date in recurrence.SkippedRecurrences)
            {
                if (date.Date == currentDay.Date)
                    return true;
            }
            return false;
        }

        private bool IsRecurringOnDay(DateTime currentDay, DateTime rangeStart, DateTime rangeEnd, Appointment rootAppointment)
        {
            TimeSpan duration = rootAppointment.EndTime.Subtract(rootAppointment.StartTime);
            DateTime startTime = rootAppointment.GetValidDateTime(new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, rootAppointment.StartTime.Hour, rootAppointment.StartTime.Minute, 0));
            DateTime endTime = rootAppointment.GetValidDateTime(startTime.Add(duration));

            DateTime localStartTime = rootAppointment.GetLocalDateTime(rootAppointment.GetUTCDateTime(startTime));
            DateTime localEndTime = rootAppointment.GetLocalDateTime(rootAppointment.GetUTCDateTime(endTime));

            // If time-zone is not used return false
            if (startTime == localStartTime) return currentDay >= rangeStart;

            return DateTimeHelper.TimePeriodsOverlap(localStartTime, localEndTime, rangeStart, rangeEnd);
        }

        private Appointment CreateRecurringAppointmentInstance(DateTime currentDay, Appointment rootAppointment)
        {
            Appointment app = rootAppointment.Copy();
            TimeSpan duration = rootAppointment.EndTime.Subtract(rootAppointment.StartTime);
            DateTime startTime = app.GetValidDateTime(new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, rootAppointment.StartTime.Hour, rootAppointment.StartTime.Minute, 0));
            DateTime endTime = app.GetValidDateTime(startTime.Add(duration));

            app.StartTime = startTime;
            app.EndTime = endTime;
            app.IsRecurringInstance = true;
            app.RootAppointment = rootAppointment;
            app.Tooltip = rootAppointment.Tooltip;
            app.Visible = rootAppointment.Visible;

            foreach (Reminder reminder in app.Reminders)
            {
                //reminder.ReminderTime = app.StartTime.Add(reminder.ReminderTime.Subtract(rootAppointment.StartTime));
                reminder.IsActive = true;
            }

            return app;
        }

        /// <summary>
        /// Generates Weekly recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Weekly recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        public void GenerateWeeklyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate)
        {
            Appointment app = recurrence.Appointment;
            DateTime baseStartDate = app.EndTime.Date;
            if (app.EndTime.Date > app.StartTime.Date && app.EndTime.TimeOfDay > app.StartTime.TimeOfDay && baseStartDate != DateTime.MinValue)
                baseStartDate = baseStartDate.AddDays(1);
            DateTime recurrenceStartDate = baseStartDate;
            if (RepeatsOnSingleDayOnly(recurrence.Weekly.RepeatOnDaysOfWeek) && !(app.StartTime.Date == startDate.Date && endDate.Date == startDate.Date))
                recurrenceStartDate = baseStartDate.AddMilliseconds(-1).AddDays((recurrence.Weekly.RepeatInterval - 1) * 7 /*+ (recurrence.Weekly.RepeatInterval > 1 ? (8 - (int)baseStartDate.DayOfWeek) : 1)*/).Date;
            else
                recurrenceStartDate = app.EndTime.Date.AddDays(1);
            recurrenceStartDate = DateTimeHelper.MaxDate(recurrence.RecurrenceStartDate, recurrenceStartDate);

            if (recurrenceStartDate > endDate) return;

            int totalRecurrences = 0;
            // Check the range first
            if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate)
            {
                if (startDate > recurrence.RangeEndDate) return;
            }
            else if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences)
            {
                int totalDays = (int)Math.Ceiling(startDate.Subtract(recurrenceStartDate).TotalDays);
                double weeks = totalDays / 7;
                int occurrencesPerWeek = GetNumberOfDays(recurrence.Weekly.RepeatOnDaysOfWeek);
                if (occurrencesPerWeek == 0)
                    throw new ArgumentException("Weekly recurrence must have at least single day selected using RepeatOnDaysOfWeek property.");
                totalRecurrences = Math.Max(0, DateTimeHelper.TotalNumberOfDays(recurrenceStartDate, startDate, recurrence.Weekly.RepeatOnDaysOfWeek));
                if (recurrence.Weekly.RepeatInterval > 1) totalRecurrences = totalRecurrences / recurrence.Weekly.RepeatInterval;
                if (totalRecurrences > recurrence.RangeNumberOfOccurrences) return;
            }

            DateTime currentDay = recurrenceStartDate;// DateTimeHelper.MaxDate(recurrenceStartDate, startDate);
            while (currentDay <= endDate)
            {
                if (currentDay >= startDate && DateTimeHelper.HasDay(currentDay.DayOfWeek, recurrence.Weekly.RepeatOnDaysOfWeek)
                    && !IsIgnoredRecurrence(currentDay, recurrence))
                {
                    subsetCollection.Add(CreateRecurringAppointmentInstance(currentDay, app));
                    totalRecurrences++;
                }

                if (currentDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    while (currentDay.DayOfWeek != DayOfWeek.Sunday)
                    {
                        currentDay = currentDay.AddDays(1);
                        if (currentDay > endDate || recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate && currentDay > recurrence.RangeEndDate)
                            break;
                        if (currentDay >= startDate && DateTimeHelper.HasDay(currentDay.DayOfWeek, recurrence.Weekly.RepeatOnDaysOfWeek)
                            && !IsIgnoredRecurrence(currentDay, recurrence))
                        {
                            subsetCollection.Add(CreateRecurringAppointmentInstance(currentDay, app));
                            totalRecurrences++;
                            if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate && currentDay > recurrence.RangeEndDate ||
                                recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && totalRecurrences >= recurrence.RangeNumberOfOccurrences)
                                break;
                        }
                    }
                }
                currentDay = currentDay.AddDays(1 + (recurrence.Weekly.RepeatInterval - 1) * 7);
                if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate && currentDay > recurrence.RangeEndDate ||
                    recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && totalRecurrences >= recurrence.RangeNumberOfOccurrences)
                    break;
            }
        }

        private bool RepeatsOnSingleDayOnly(eDayOfWeekRecurrence dow)
        {
            return dow == eDayOfWeekRecurrence.Monday || dow == eDayOfWeekRecurrence.Tuesday ||
                dow == eDayOfWeekRecurrence.Wednesday || dow == eDayOfWeekRecurrence.Thursday ||
                dow == eDayOfWeekRecurrence.Friday || dow == eDayOfWeekRecurrence.Saturday || dow == eDayOfWeekRecurrence.Sunday;
        }

        private int GetNumberOfDays(eDayOfWeekRecurrence daysOfWeek)
        {
            int days = 0;
            if ((daysOfWeek & eDayOfWeekRecurrence.Friday) != 0)
                days++;
            if ((daysOfWeek & eDayOfWeekRecurrence.Monday) != 0)
                days++;
            if ((daysOfWeek & eDayOfWeekRecurrence.Saturday) != 0)
                days++;
            if ((daysOfWeek & eDayOfWeekRecurrence.Sunday) != 0)
                days++;
            if ((daysOfWeek & eDayOfWeekRecurrence.Thursday) != 0)
                days++;
            if ((daysOfWeek & eDayOfWeekRecurrence.Tuesday) != 0)
                days++;
            if ((daysOfWeek & eDayOfWeekRecurrence.Wednesday) != 0)
                days++;

            return days;
        }

        /// <summary>
        /// Generates Monthly recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Monthly recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        public void GenerateMonthlyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate)
        {
            Appointment app = recurrence.Appointment;
            DateTime recurrenceStartDate = DateTimeHelper.MaxDate(recurrence.RecurrenceStartDate, app.EndTime/*.AddDays(1).Date*/);
            if (recurrenceStartDate > endDate) return;

            int totalRecurrences = 0;
            // Check the range first
            if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate)
            {
                if (startDate > recurrence.RangeEndDate) return;
            }
            else if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && recurrence.RangeNumberOfOccurrences > 0)
            {
                int count = recurrence.RangeNumberOfOccurrences + 1;
                DateTime testDate = DateTimeHelper.BeginningOfDay(recurrenceStartDate);
                DateTime startDateOnly = DateTimeHelper.BeginningOfDay(startDate);
                while (count > 0 && testDate < startDateOnly)
                {
                    int repeatInterval = recurrence.Monthly.RepeatInterval;
                    while (repeatInterval > 0)
                    {
                        testDate = GetNextMonthlyRecurrence(recurrence, testDate);
                        repeatInterval--;
                    }
                    if (testDate < startDate)
                        totalRecurrences++;
                    count--;
                }
                if (count == 0) return;
            }

            DateTime currentDate = recurrenceStartDate;// DateTimeHelper.MaxDate(recurrenceStartDate, startDate).AddMonths(-1);

            do
            {
                int repeatCount = recurrence.Monthly.RepeatInterval;
                do
                {
                    repeatCount--;
                    currentDate = GetNextMonthlyRecurrence(recurrence, currentDate);
                } while (repeatCount > 0);

                if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate && currentDate > recurrence.RangeEndDate ||
                    recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && totalRecurrences >= recurrence.RangeNumberOfOccurrences)
                    break;

                if (currentDate >= startDate && currentDate <= endDate && !IsIgnoredRecurrence(currentDate, recurrence))
                {
                    subsetCollection.Add(CreateRecurringAppointmentInstance(currentDate, app));
                    totalRecurrences++;
                }
            } while (currentDate <= endDate);
        }

        private DateTime GetNextMonthlyRecurrence(AppointmentRecurrence recurrence, DateTime startDate)
        {
            MonthlyRecurrenceSettings monthly = recurrence.Monthly;
            return GetNextMonthlyRecurrence(startDate, monthly.RepeatOnRelativeDayInMonth,
                monthly.RelativeDayOfWeek, monthly.RepeatOnDayOfMonth);
        }

        private static DateTime GetNextMonthlyRecurrence(DateTime startDate, eRelativeDayInMonth repeatOnRelativeDayInMonth, DayOfWeek relativeDayOfWeek, int repeatOnDayOfMonth)
        {
            Calendar cal = GetCalendar();

            if (repeatOnRelativeDayInMonth == eRelativeDayInMonth.None)
            {
                DateTime dt = startDate.AddDays(cal.GetDaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);
                return dt.AddDays(Math.Min(cal.GetDaysInMonth(dt.Year, dt.Month), repeatOnDayOfMonth) - 1);
            }
            else
            {
                if (repeatOnRelativeDayInMonth == eRelativeDayInMonth.First)
                {
                    DateTime dt = startDate.AddDays(cal.GetDaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);
                    while (dt.DayOfWeek != relativeDayOfWeek)
                        dt = dt.AddDays(1);
                    return dt;
                }
                else if (repeatOnRelativeDayInMonth == eRelativeDayInMonth.Last)
                {
                    DateTime dt = startDate.AddDays(cal.GetDaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);
                    dt = dt.AddDays(cal.GetDaysInMonth(dt.Year, dt.Month) - 1);
                    while (dt.DayOfWeek != relativeDayOfWeek)
                        dt = dt.AddDays(-1);
                    return dt;
                }
                else
                {
                    // Second, third and forth
                    int relCount = 2;
                    if (repeatOnRelativeDayInMonth == eRelativeDayInMonth.Third)
                        relCount = 3;
                    else if (repeatOnRelativeDayInMonth == eRelativeDayInMonth.Fourth)
                        relCount = 4;
                    DateTime dt = startDate.AddDays(cal.GetDaysInMonth(startDate.Year, startDate.Month) - startDate.Day);
                    while (relCount > 0)
                    {
                        dt = dt.AddDays(1);
                        if (dt.DayOfWeek == relativeDayOfWeek)
                            relCount--;
                    }
                    return dt;
                }
            }
            throw new InvalidOperationException("Could not find the next relative date for monthly recurrence starting on " + startDate.ToString() + " RelativeDayInMonth=" + repeatOnRelativeDayInMonth.ToString() + " RelativeDayOfWeek=" + relativeDayOfWeek.ToString());
        }

        private static Calendar GetCalendar()
        {
            return CultureInfo.CurrentCulture.Calendar;
        }

        /// <summary>
        /// Generates Yearly recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Monthly recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        public void GenerateYearlyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate)
        {
            Appointment app = recurrence.Appointment;
            DateTime recurrenceStartDate = DateTimeHelper.MaxDate(recurrence.RecurrenceStartDate, app.EndTime.AddDays(1).Date);
            if (recurrenceStartDate > endDate) return;

            if (!(startDate.Month <= recurrence.Yearly.RepeatOnMonth && endDate.Month >= recurrence.Yearly.RepeatOnMonth) && endDate.Subtract(startDate).TotalDays < 28)
                return;
            int totalRecurrences = 0;
            // Check the range first
            if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate)
            {
                if (startDate > recurrence.RangeEndDate) return;
            }
            else if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && recurrence.RangeNumberOfOccurrences > 0 && recurrenceStartDate < startDate)
            {
                int count = recurrence.RangeNumberOfOccurrences + 1;
                DateTime testDate = DateTimeHelper.BeginningOfDay(recurrenceStartDate.AddDays(-(recurrenceStartDate.Day - 1))).AddMonths(-1);
                if (testDate < app.EndTime)
                    testDate = app.EndTime.Date.AddDays(1);
                DateTime startDateOnly = DateTimeHelper.BeginningOfDay(startDate);
                while (count > 0 && testDate < startDateOnly)
                {
                    testDate = GetNextYearlyRecurrence(recurrence, testDate);
                    if (testDate >= startDateOnly) break;
                    count--;
                    totalRecurrences++;
                }
                if (count == 0) return;
            }

            DateTime currentDate = DateTimeHelper.MaxDate(startDate, recurrenceStartDate).AddDays(-1);
            do
            {
                currentDate = GetNextYearlyRecurrence(recurrence, currentDate);

                if (recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate && currentDate > recurrence.RangeEndDate ||
                    recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && totalRecurrences >= recurrence.RangeNumberOfOccurrences)
                    break;

                if (currentDate >= startDate && currentDate <= endDate && !IsIgnoredRecurrence(currentDate, recurrence))
                {
                    subsetCollection.Add(CreateRecurringAppointmentInstance(currentDate, app));
                    totalRecurrences++;
                }
            } while (currentDate <= endDate);
        }

        internal static DateTime GetNextYearlyRecurrence(AppointmentRecurrence recurrence, DateTime startDate)
        {
            YearlyRecurrenceSettings yearly = recurrence.Yearly;
            Calendar cal = GetCalendar();

            if (startDate.Month < yearly.RepeatOnMonth)
            {
                startDate = startDate.AddMonths((yearly.RepeatOnMonth - startDate.Month) - 1);
                return GetNextMonthlyRecurrence(startDate, yearly.RepeatOnRelativeDayInMonth, yearly.RelativeDayOfWeek, yearly.RepeatOnDayOfMonth);
            }
            else if (startDate.Month == yearly.RepeatOnMonth)
            {
                DateTime refDate = startDate.AddDays(-(startDate.Day));
                DateTime ret = GetNextMonthlyRecurrence(refDate, yearly.RepeatOnRelativeDayInMonth, yearly.RelativeDayOfWeek, yearly.RepeatOnDayOfMonth);
                if (ret > startDate) return ret;
            }

            // Forward to next year and right month
            startDate = startDate.AddDays(cal.GetDaysInYear(startDate.Year) - startDate.DayOfYear).AddMonths(yearly.RepeatOnMonth - 1);
            return GetNextMonthlyRecurrence(startDate, yearly.RepeatOnRelativeDayInMonth, yearly.RelativeDayOfWeek, yearly.RepeatOnDayOfMonth);
        }

        #endregion
    }

    internal interface IRecurrenceGenerator
    {
        /// <summary>
        /// Generates Daily recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Daily recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        void GenerateDailyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Generates Weekly recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Weekly recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        void GenerateWeeklyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Generates Monthly recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Monthly recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        void GenerateMonthlyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate);

        /// <
        /// summary>
        /// Generates Yearly recurring appointments. If appointment is assigned to calendar method must populate the Calendar.Appointments collection as well.
        /// </summary>
        /// <param name="subsetCollection">Collection to add generated recurrences to</param>
        /// <param name="recurrence">Recurrence description, must be of Monthly recurrence type.</param>
        /// <param name="startDate">Start date for generation.</param>
        /// <param name="endDate">End date for generation.</param>
        void GenerateYearlyRecurrence(AppointmentSubsetCollection subsetCollection, AppointmentRecurrence recurrence, DateTime startDate, DateTime endDate);
    }
}
#endif

