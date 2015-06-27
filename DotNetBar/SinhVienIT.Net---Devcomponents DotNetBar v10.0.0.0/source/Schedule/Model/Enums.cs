#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Specifies notification type when appointment start time has been reached.
    /// </summary>
    [Flags()]
    public enum eStartTimeAction
    {
        /// <summary>
        /// No action is taken.
        /// </summary>
        None = 0,
        /// <summary>
        /// StartTimeReached event is fired.
        /// </summary>
        StartTimeReachedEvent = 1,
        /// <summary>
        /// StartTimeCommand is executed.
        /// </summary>
        Command = 2,
        /// <summary>
        /// Both event and command are performed.
        /// </summary>
        StartTimeReachedEventAndCommand = StartTimeReachedEvent | Command
    }

    /// <summary>
    /// Specifies notification type when reminder time has been reached.
    /// </summary>
    [Flags()]
    public enum eReminderAction
    {
        /// <summary>
        /// No action is taken.
        /// </summary>
        None = 0,
        /// <summary>
        /// Reminder event is fired.
        /// </summary>
        Event = 1,
        /// <summary>
        /// Reminder Command is executed.
        /// </summary>
        Command = 2,
        /// <summary>
        /// Both event and command are performed.
        /// </summary>
        EventAndCommand = Event | Command
    }

    /// <summary>
    /// Specifies the recurrence range type.
    /// </summary>
    public enum eRecurrenceRangeLimitType
    {
        /// <summary>
        /// Recurrence range has no end date specified.
        /// </summary>
        NoEndDate,
        /// <summary>
        /// Recurrence ends on date specified by RangeEndDate property.
        /// </summary>
        RangeEndDate,
        /// <summary>
        /// Recurrence ends after specified number of repeats by RangeNumberOfOccurrences property.
        /// </summary>
        RangeNumberOfOccurrences
    }

    /// <summary>
    /// Specifies the pattern type for appointment recurrence.
    /// </summary>
    public enum eRecurrencePatternType
    {
        /// <summary>
        /// Appointment recurs daily.
        /// </summary>
        Daily,
        /// <summary>
        /// Appointment recurs weekly.
        /// </summary>
        Weekly,
        /// <summary>
        /// Appointment recurs monthly.
        /// </summary>
        Monthly,
        /// <summary>
        /// Appointment recurs yearly.
        /// </summary>
        Yearly
    }

    /// <summary>
    /// Specifies the relative day in month for recurrence.
    /// </summary>
    public enum eRelativeDayInMonth
    {
        /// <summary>
        /// No value specified.
        /// </summary>
        None,
        /// <summary>
        /// The first occurrence of the specified day in its month. 
        /// </summary>
        First,
        /// <summary>
        /// The second occurrence of the specified day in its month. 
        /// </summary>
        Second,
        /// <summary>
        /// The third occurrence of the specified day in its month. 
        /// </summary>
        Third,
        /// <summary>
        /// The fourth occurrence of the specified day in its month. 
        /// </summary>
        Fourth,
        /// <summary>
        /// The last occurrence of the specified day in its month. 
        /// </summary>
        Last
    }

    /// <summary>
    /// Specifies on which day the appointment is repeated.
    /// </summary>
    [Flags()]
    public enum eDayOfWeekRecurrence
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,
        WeekendDays = Saturday | Sunday,
        WeekDays = Monday | Tuesday | Wednesday | Thursday | Friday,
        All = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
    }

    /// <summary>
    /// Specifies on which days daily recurrence is repeated.
    /// </summary>
    public enum eDailyRecurrenceRepeat
    {
        /// <summary>
        /// Appointment is repeated on all days.
        /// </summary>
        All = eDayOfWeekRecurrence.Monday | eDayOfWeekRecurrence.Tuesday | eDayOfWeekRecurrence.Wednesday | eDayOfWeekRecurrence.Thursday | eDayOfWeekRecurrence.Friday | eDayOfWeekRecurrence.Saturday | eDayOfWeekRecurrence.Sunday,
        
        /// <summary>
        /// Appointment is repeated on week-days only, Monday-Friday.
        /// </summary>
        WeekDays = eDayOfWeekRecurrence.Monday | eDayOfWeekRecurrence.Tuesday | eDayOfWeekRecurrence.Wednesday | eDayOfWeekRecurrence.Thursday | eDayOfWeekRecurrence.Friday,
        
        /// <summary>
        /// Appointment is repeated on weekend-days only, Saturday-Sunday.
        /// </summary>
        WeekendDays = eDayOfWeekRecurrence.WeekendDays
    }
}
#endif

