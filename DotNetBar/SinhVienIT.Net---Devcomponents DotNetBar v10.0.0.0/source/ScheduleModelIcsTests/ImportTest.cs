using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DevComponents.DotNetBar.Schedule;
using DevComponents.Schedule;
using DevComponents.Schedule.Model;
using DevComponents.Schedule.Model.Serialization;
using NUnit.Framework;

namespace ScheduleModelIcsTests
{
    public delegate void ValidateHandler(string calName, DateTime dateStart, DateTime rangeEnd);

    [TestFixture]
    public class ImportTestDaily : ImportTestBase
    {
        #region DailyTest1: Daily for 10 occurrences

        //BEGIN:VEVENT
        //SUMMARY:Daily for 10 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) January 1-10
        //DTSTART;TZID=America/New_York:20100101T090000
        //RRULE:FREQ=DAILY;COUNT=10
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void DailyTest1()
        {
            DateTime dateStart = new DateTime(2010, 1, 1, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "DailyTest1";
            const string summary = "Daily for 10 occurrences";
            const string description = "(2010 9:00 AM EDT) January 1-10";

            const string duration = "PT2H";
            const string rrule = "FREQ=DAILY;COUNT=10";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, duration, rrule);
            CalendarEnd();

            PerformTest(DailyTest1Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region DailyTest1Validate

        private void DailyTest1Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 1);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 9);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddDays(10));

            Assert.True(appc.Count == 10);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);
                date = date.AddDays(1);
            }
        }

        #endregion

        #endregion

        #region DailyTest2: Daily until March 24, 2010

        //BEGIN:VEVENT
        //SUMMARY:Daily until March 24, 2010
        //DESCRIPTION:(2010 9:00 AM EDT) February 2 - March 24, 2010
        //DTSTART;TZID=America/New_York:20100202T090000
        //DTEND;TZID=America/New_York:20100202T110000
        //RRULE:FREQ=DAILY;UNTIL=20100324T000000
        //END:VEVENT

        [Test]
        public void DailyTest2()
        {
            DateTime dateStart = new DateTime(2010, 2, 2, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);
            DateTime rangeEnd = new DateTime(2010, 3, 24, 0, 0, 0);

            const string calName = "DailyTest2";
            const string summary = "Daily until March 24, 2010";
            const string description = "(2010 9:00 AM EDT) February 2 - March 24, 2010";

            string rrule = "FREQ=DAILY;UNTIL=" + GetDateTimeText(rangeEnd);

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(DailyTest2Validate, calName, dateStart, rangeEnd);
        }

        #region DailyTest2Validate

        private void DailyTest2Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 1);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate);
            Assert.True(recur.RangeEndDate == rangeEnd);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 51);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);

                date = date.AddDays(1);
            }
        }

        #endregion

        #endregion

        #region DailyTest3: Every WeekEndDay - for 4 occurrences

        //BEGIN:VEVENT
        //SUMMARY:Every WeekEndDay - for 4 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) May 15-16, 22-23
        //UID:C2BD07E2-54F2-11D7-93F8-000393BCF7D2-RID4
        //DTSTART;TZID=America/New_York:20100515T090000
        //RRULE:FREQ=DAILY;COUNT=4;BYDAY=SA,SU
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void DailyTest3()
        {
            DateTime dateStart = new DateTime(2010, 5, 15, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "DailyTest3";
            const string summary = "(2010 9:00 AM EDT) May 15-16, 22-23";
            const string description = "(2010 9:00 AM EDT) May 15-16, 22-23";
            const string rrule = "FREQ=DAILY;COUNT=4;BYDAY=SA,SU";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(DailyTest3Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region DailyTest3Validate

        private void DailyTest3Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 1);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.WeekendDays);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 3);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 4);

            DateTime date = dateStart;

            Assert.True(appc[0].StartTime == date);
            Assert.True(appc[1].StartTime == date.AddDays(1));
            Assert.True(appc[2].StartTime == date.AddDays(7));
            Assert.True(appc[3].StartTime == date.AddDays(8));
        }

        #endregion

        #endregion

        #region DailyTest4: Every Day - for 21 occurrences

        //BEGIN:VEVENT
        //SUMMARY:Every Day - for 21 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) June 9-29
        //DTSTART;TZID=America/New_York:20100609T090000
        //RRULE:FREQ=DAILY;COUNT=21;BYDAY=SA,SU,MO,TU,WE,TH,FR
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void DailyTest4()
        {
            DateTime dateStart = new DateTime(2010, 6, 9, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "DailyTest4";
            const string summary = "Every Day - for 21 occurrences";
            const string description = "(2010 9:00 AM EDT) June 9-29";
            const string rrule = "FREQ=DAILY;COUNT=21;BYDAY=SA,SU,MO,TU,WE,TH,FR";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(DailyTest4Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region DailyTest4Validate

        private void DailyTest4Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 1);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 20);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 21);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);

                date = date.AddDays(1);
            }
        }

        #endregion

        #endregion

        #region DailyTest5: Every Day - for 5 occurrences, Interval 10

        //BEGIN:VEVENT
        //SUMMARY:Every Day - for 5 occurrences, Interval 10
        //DESCRIPTION:(2010 9:00 AM EDT) Jul 22; Aug 1,11,21,31
        //DTSTART;TZID=America/New_York:20100722T090000
        //RRULE:FREQ=DAILY;INTERVAL=10;COUNT=5
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void DailyTest5()
        {
            DateTime dateStart = new DateTime(2010, 7, 22, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "DailyTest5";
            const string summary = "Every Day - for 5 occurrences, Interval 10";
            const string description = "(2010 9:00 AM EDT) Jul 22; Aug 1,11,21,31";
            const string rrule = "FREQ=DAILY;INTERVAL=10;COUNT=5";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(DailyTest5Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region DailyTest5Validate

        private void DailyTest5Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 10);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 4);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 5);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);

                date = date.AddDays(10);
            }
        }

        #endregion

        #endregion

        #region DailyTest6: Every other day - forever

        //BEGIN:VEVENT
        //SUMMARY:Every other day - forever
        //DESCRIPTION:(2010 9:00 AM EDT) October 8,10,12...
        //DTSTART;TZID=America/New_York:20101008T090000
        //RRULE:FREQ=DAILY;INTERVAL=2
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void DailyTest6()
        {
            DateTime dateStart = new DateTime(2010, 10, 8, 2, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "DailyTest6";
            const string summary = "Every other day - forever";
            const string description = "(2010 9:00 AM EDT) October 8,10,12...";
            const string rrule = "FREQ=DAILY;INTERVAL=2";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(DailyTest6Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region DailyTest6Validate

        private void DailyTest6Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 2);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.NoEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection
                appc = new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 183);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == GetValidDateTime(date));

                date = date.AddDays(2);
            }
        }

        #endregion

        #endregion
    }

    [TestFixture]
    public class ImportTestWeekly : ImportTestBase
    {
        #region WeeklyTest1: Weekly for 10x

        //BEGIN:VEVENT
        //SUMMARY:Weekly for 10 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Jan 1,8,15,22,29;Feb 5,12,19,26 Mar 5
        //DTSTART;TZID=America/New_York:20100101T090000
        //RRULE:FREQ=WEEKLY;COUNT=10
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void WeeklyTest1()
        {
            DateTime dateStart = new DateTime(2010, 1, 1, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "WeeklyTest1";
            const string summary = "Weekly for 10 occurrences";
            const string description = "(2010 9:00 AM EDT) Jan 1,8,15,22,29;Feb 5,12,19,26 Mar 5";
            const string rrule = "FREQ=WEEKLY;COUNT=10";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, "PT2H", rrule);
            CalendarEnd();

            PerformTest(WeeklyTest1Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region WeeklyTest1Validate

        private void WeeklyTest1Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Weekly);
            Assert.True(recur.Weekly.RepeatInterval == 1);
            Assert.True(recur.Weekly.RepeatOnDaysOfWeek == eDayOfWeekRecurrence.Friday);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 9);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 10);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);

                date = date.AddDays(7);
            }
        }

        #endregion

        #endregion

        #region WeeklyTest2: Weekly until May 24, 2010

        //BEGIN:VEVENT
        //SUMMARY:Weekly until May 24, 2010
        //DESCRIPTION:(2010 9:00 AM EDT) Mar 11,18,25;Apr 1,8,15,22,29;May 6,13,20
        //DTSTART;TZID=America/New_York:20100311T090000
        //RRULE:FREQ=WEEKLY;UNTIL=20100524T000000
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void WeeklyTest2()
        {
            DateTime dateStart = new DateTime(2010, 3, 11, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);
            DateTime rangeEnd = new DateTime(2010, 5, 24, 0, 0, 0);

            const string calName = "WeeklyTest2";
            const string summary = "Weekly until May 24, 2010";
            const string description = "(2010 9:00 AM EDT) Mar 11,18,25;Apr 1,8,15,22,29;May 6,13,20";
            string rrule = "FREQ=WEEKLY;UNTIL=" + GetDateTimeText(rangeEnd);

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, "PT2H", rrule);
            CalendarEnd();

            PerformTest(WeeklyTest2Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region WeeklyTest2Validate

        private void WeeklyTest2Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Weekly);
            Assert.True(recur.Weekly.RepeatInterval == 1);
            Assert.True(recur.Weekly.RepeatOnDaysOfWeek == eDayOfWeekRecurrence.Thursday);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 11);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);

                date = date.AddDays(7);
            }
        }

        #endregion

        #endregion

        #region WeeklyTest3: Weekly on Tuesday and Thursday until July 4, 2010

        //BEGIN:VEVENT
        //SUMMARY:Weekly on Tuesday and Thursday until July 4, 2010
        //DESCRIPTION:(2010 9:00 AM EDT) June 3,8,10,15,17,22
        //DTSTART;TZID=America/New_York:20100603T090000
        //RRULE:FREQ=WEEKLY;UNTIL=20100623T000000Z;BYDAY=TU,TH
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void WeeklyTest3()
        {
            DateTime dateStart = new DateTime(2010, 6, 3, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "WeeklyTest3";
            const string summary = "Weekly on Tuesday and Thursday until July 4, 2010";
            const string description = "(2010 9:00 AM EDT) June 3,8,10,15,17,22,24,29; Jul 1";
            const string rrule = "FREQ=WEEKLY;UNTIL=20100704T000000Z;BYDAY=TU,TH";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, "PT2H", rrule);
            CalendarEnd();

            PerformTest(WeeklyTest3Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region WeeklyTest3Validate

        private void WeeklyTest3Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Weekly);
            Assert.True(recur.Weekly.RepeatInterval == 1);
            Assert.True(recur.Weekly.RepeatOnDaysOfWeek == (eDayOfWeekRecurrence.Tuesday | eDayOfWeekRecurrence.Thursday));

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 9);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);
                date = date.AddDays(date.DayOfWeek == DayOfWeek.Tuesday ? 2 : 5);
            }
        }

        #endregion

        #endregion

        #region WeeklyTest4: Weekly on Wednesday and Saturday for five weeks

        //BEGIN:VEVENT
        //SUMMARY:Weekly on Wednesday and Saturday for five weeks
        //DESCRIPTION:(2010 9:00 AM EDT) Jul 7,10,14,17,21,24,28,31; Aug 4,7
        //DTSTART;TZID=America/New_York:20100707T090000
        //RRULE:FREQ=WEEKLY;COUNT=10;BYDAY=SA,WE
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void WeeklyTest4()
        {
            DateTime dateStart = new DateTime(2010, 7, 7, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "WeeklyTest4";
            const string summary = "Weekly on Wednesday and Saturday for five weeks";
            const string description = "(2010 9:00 AM EDT) Jul 7,10,14,17,21,24,28,31; Aug 4,7";
            const string rrule = "FREQ=WEEKLY;COUNT=10;BYDAY=SA,WE";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(WeeklyTest4Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region WeeklyTest4Validate

        private void WeeklyTest4Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Weekly);
            Assert.True(recur.Weekly.RepeatInterval == 1);
            Assert.True(recur.Weekly.RepeatOnDaysOfWeek == (eDayOfWeekRecurrence.Saturday | eDayOfWeekRecurrence.Wednesday));

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 9);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 10);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i += 2)
            {
                Assert.True(appc[i].StartTime == date);
                date = date.AddDays(3);

                Assert.True(appc[i + 1].StartTime == date);
                date = date.AddDays(4);
            }
        }

        #endregion

        #endregion

        #region WeeklyTest5: Every other week on Mon, Wed, and Fri until Sept 24, 2010

        //BEGIN:VEVENT
        //SUMMARY:Every other week on Mon, Wed, and Fri until Sept 24, 2010
        //DESCRIPTION:(2010 9:00 AM EDT) Aug 9,11,13,23,25,27;sept 6,8,10,20,22,24
        //DTSTART;TZID=America/New_York:20100809T090000
        //RRULE:FREQ=WEEKLY;INTERVAL=2;UNTIL=20100924T100000Z;BYDAY=MO,WE,FR
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void WeeklyTest5()
        {
            DateTime dateStart = new DateTime(2010, 8, 9, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "WeeklyTest5";
            const string summary = "Every other week on Mon, Wed, and Fri until Sept 24, 2010";
            const string description = "(2010 9:00 AM EDT) Aug 9,11,13,23,25,27;sept 6,8,10,20,22,24";
            const string rrule = "FREQ=WEEKLY;INTERVAL=2;UNTIL=20100924T100000Z;BYDAY=MO,WE,FR";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(WeeklyTest5Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region WeeklyTest5Validate

        private void WeeklyTest5Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Weekly);
            Assert.True(recur.Weekly.RepeatInterval == 2);

            Assert.True(recur.Weekly.RepeatOnDaysOfWeek ==
                (eDayOfWeekRecurrence.Monday | eDayOfWeekRecurrence.Wednesday | eDayOfWeekRecurrence.Friday));

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 12);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i += 3)
            {
                Assert.True(appc[i].StartTime == date);
                date = date.AddDays(2);

                Assert.True(appc[i + 1].StartTime == date);
                date = date.AddDays(2);

                Assert.True(appc[i + 2].StartTime == date);
                date = date.AddDays(10);
            }
        }

        #endregion

        #endregion

        #region WeeklyTest6: Weekly, 3 days long, on Tue

        //BEGIN:VEVENT
        //SUMMARY:Weekly, 3 days long, on Tue
        //RRULE:FREQ=WEEKLY;INTERVAL=1;BYDAY=TU
        //DTSTART;VALUE=DATE:20100907
        //DTEND;VALUE=DATE:20100909
        //END:VEVENT

        [Test]
        public void WeeklyTest6()
        {
            DateTime dateStart = new DateTime(2010, 9, 7, 0, 0, 0);
            DateTime dateEnd = new DateTime(2010, 9, 10, 0, 0, 0);

            const string calName = "WeeklyTest6";
            const string summary = "Weekly, 3 days long, on Tue";
            const string description = "(2010 00:00 AM EDT) Sept 3-5, 10-12, ...";
            const string rrule = "FREQ=WEEKLY;INTERVAL=1;BYDAY=TU";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(WeeklyTest6Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region WeeklyTest6Validate

        private void WeeklyTest6Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Weekly);
            Assert.True(recur.Weekly.RepeatInterval == 1);

            Assert.True(recur.Weekly.RepeatOnDaysOfWeek == eDayOfWeekRecurrence.Tuesday);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.NoEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 53);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);
                Assert.True(appc[i].StartTime.DayOfWeek == DayOfWeek.Tuesday);
                Assert.True((appc[i].EndTime - appc[i].StartTime).TotalDays == 3);

                date = date.AddDays(7);
            }
        }

        #endregion

        #endregion
    }

    [TestFixture]
    public class ImportTestMonthly : ImportTestBase
    {
        #region MonthlyTest1: Monthly on the first Friday for 10x

        //BEGIN:VEVENT
        //SUMMARY:Monthly on the first Friday for 10 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Jan 1;Feb 5;Mar 5;Apr 2;May 7;Jun 4;Jul 2;Aug 6;Sept 3;Oct 1
        //DTSTART;TZID=America/New_York:20100101T090000
        //RRULE:FREQ=MONTHLY;COUNT=10;BYDAY=1FR
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void MonthlyTest1()
        {
            DateTime dateStart = new DateTime(2010, 1, 1, 0, 0, 0);

            const string calName = "MonthlyTest1";
            const string summary = "Monthly on the first Friday for 10 occurrences";
            const string description = "(2010 9:00 AM EDT) Jan 1;Feb 5;Mar 5;Apr 2;May 7;Jun 4;Jul 2;Aug 6;Sept 3;Oct 1";
            const string rrule = "FREQ=MONTHLY;COUNT=10;BYDAY=1FR";

            string uid = Guid.NewGuid().ToString();
            string sDateStart = GetDateTimeText(dateStart);

            CalendarBegin(calName);

            AddOutput("BEGIN:VEVENT");

            AddOutput("SUMMARY:" + summary);
            AddOutput("DESCRIPTION:" + description);
            AddOutput("UID:" + uid);
            AddOutput("DTSTAMP:" + sDateStart);
            AddOutput("DTSTART;VALUE=DATE:" + dateStart.ToString("yyyyMMdd"));
            AddOutput("RRULE:" + rrule);

            AddOutput("END:VEVENT");

            CalendarEnd();

            PerformTest(MonthlyTest1Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region MonthlyTest1Validate

        private void MonthlyTest1Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.First);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 9);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 10);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);

                date = GetNextFirstDate(date, date.DayOfWeek);
            }
        }

        #endregion

        #endregion

        #region MonthlyTest2: Monthly on the third Tuesday until Mar 24, 2010

        //BEGIN:VEVENT
        //SUMMARY:Monthly on the third Tuesday until Mar 24, 2010
        //DESCRIPTION:(2010 9:00 AM EDT) Jan 19;Feb 16;Mar 16
        //DTSTART;TZID=America/New_York:20100119T090000
        //RRULE:FREQ=MONTHLY;UNTIL=20100324T000000Z;BYDAY=3TU
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void MonthlyTest2()
        {
            DateTime dateStart = new DateTime(2010, 1, 19, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "MonthlyTest2";
            const string summary = "Monthly on the third Tuesday until Mar 24, 2010";
            const string description = "(2010 9:00 AM EDT) Jan 19;Feb 16;Mar 16";
            const string rrule = "FREQ=MONTHLY;UNTIL=20100324T000000Z;BYDAY=3TU";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(MonthlyTest2Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region MonthlyTest2Validate

        private void MonthlyTest2Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.Third);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 3);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(appc[i].StartTime == date);

                date = GetNextRelativeDate(date, date.DayOfWeek, 3);
            }
        }

        #endregion

        #endregion

        #region MonthlyTest3: Every other month, 2nd Sunday and last Saturday, 10x

        //BEGIN:VEVENT
        //SUMMARY:Every other month on the second Sunday and last Saturday
        //  of the month for 10 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Apr 11,24;Jun 13,26;Aug 8,28;Oct 10,30
        //DTSTART;TZID=America/New_York:20100404T090000
        //RRULE:FREQ=MONTHLY;INTERVAL=2;COUNT=10;BYDAY=-1SA,2SU
        //EXDATE:20100626T090000
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void MonthlyTest3()
        {
            DateTime dateStart = new DateTime(2010, 4, 11, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "MonthlyTest3";
            const string summary = "Every other month on the second Sunday and last Saturday of the month for 10 occurrences";
            const string description = "(2010 9:00 AM EDT) Apr 11,24;Jun 13,26;Aug 8,28;Oct 10,30";
            const string rrule = "FREQ=MONTHLY;INTERVAL=2;COUNT=10;BYDAY=-1SA,2SU";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(MonthlyTest3Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region MonthlyTest3Validate

        private void MonthlyTest3Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 2);

            for (int i = 0; i < Model.Appointments.Count; i++)
            {
                Appointment app = Model.Appointments[i];

                Assert.True(app.OwnerKey.Equals(calName));

                AppointmentRecurrence recur = app.Recurrence;

                Assert.NotNull(recur);
                Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
                Assert.True(recur.Monthly.RepeatInterval == 2);
                Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth != eRelativeDayInMonth.None);

                Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
                Assert.True(recur.RangeNumberOfOccurrences == 9);
                Assert.True(recur.RecurrenceStartDate == app.StartTime);
            }

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(2));

            Assert.True(appc.Count == 20);

            DateTime date = GetNextLastDate(dateStart.AddMonths(-1), DayOfWeek.Saturday);

            for (int i = 0; i < appc.Count / 2; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                for (int j = 0; j < 2; j++)
                    date = GetNextLastDate(date, date.DayOfWeek);
            }

            date = dateStart;

            for (int i = 0; i < appc.Count / 2; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                for (int j = 0; j < 2; j++)
                    date = GetNextRelativeDate(date, date.DayOfWeek, 2);
            }
        }

        #endregion

        #endregion

        #region MonthlyTest4: Every third month, on the 1st, 2nd, and Last Thursday, 4x

        //BEGIN:VEVENT
        //SUMMARY:Every third month on the first, second, and Last Thursday of the month for 4 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Nov 4,11,25;Feb 3,10,24;May 5,12,26;Aug 4,11,25
        //DTSTART;TZID=America/New_York:20101104T090000
        //RRULE:FREQ=MONTHLY;INTERVAL=3;COUNT=4;BYDAY=1TH,2TH,-1TH
        //EXDATE:20110210T090000
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void MonthlyTest4()
        {
            DateTime dateStart = new DateTime(2010, 11, 4, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "MonthlyTest4";
            const string summary = "Every third month on the first, second, and Last Thursday of the month for 4 occurrences";
            const string description = "(2010 9:00 AM EDT) Nov 4,11,25;Feb 3,10,24;May 5,12,26;Aug 4,11,25";
            const string rrule = "FREQ=MONTHLY;INTERVAL=3;COUNT=4;BYDAY=1TH,2TH,-1TH";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(MonthlyTest4Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region MonthlyTest4Validate

        private void MonthlyTest4Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 3);

            for (int i = 0; i < Model.Appointments.Count; i++)
            {
                Appointment app = Model.Appointments[i];

                Assert.True(app.OwnerKey.Equals(calName));

                AppointmentRecurrence recur = app.Recurrence;

                Assert.NotNull(recur);
                Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
                Assert.True(recur.Monthly.RepeatInterval == 3);
                Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth != eRelativeDayInMonth.None);

                Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
                Assert.True(recur.RangeNumberOfOccurrences == 3);
                Assert.True(recur.RecurrenceStartDate == app.StartTime);
            }

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(4));

            Assert.True(appc.Count == 12);

            DateTime date = GetNextFirstDate(dateStart.AddMonths(-1), DayOfWeek.Thursday);

            for (int i = 0; i < appc.Count / 3; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                for (int j = 0; j < 3; j++)
                    date = GetNextFirstDate(date, date.DayOfWeek);
            }

            date = GetNextRelativeDate(dateStart.AddMonths(-1), DayOfWeek.Thursday, 2);

            for (int i = 0; i < appc.Count / 3; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                for (int j = 0; j < 3; j++)
                    date = GetNextRelativeDate(date, date.DayOfWeek, 2);
            }

            date = GetNextLastDate(dateStart.AddMonths(-1), DayOfWeek.Thursday);

            for (int i = 0; i < appc.Count / 3; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                for (int j = 0; j < 3; j++)
                    date = GetNextLastDate(date, date.DayOfWeek);
            }
        }

        #endregion

        #endregion

        #region MonthlyTest5: Every month on the 2nd and 15th, 5x

        //BEGIN:VEVENT
        //SUMMARY:Monthly on the 2nd and 15th of the month for 5 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Apr 2,15;May 2,15;Jun 2,15;Jul 2,15;Aug 2,15
        //DTSTART;TZID=America/New_York:20100402T090000
        //RRULE:FREQ=MONTHLY;COUNT=5;BYMONTHDAY=2,15
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void MonthlyTest5()
        {
            DateTime dateStart = new DateTime(2010, 4, 2, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "MonthlyTest5";
            const string summary = "Monthly on the 2nd and 15th of the month for 5 occurrences";
            const string description = "(2010 9:00 AM EDT) Apr 2,15;May 2,15;Jun 2,15;Jul 2,15;Aug 2,15";
            const string rrule = "FREQ=MONTHLY;COUNT=5;BYMONTHDAY=2,15";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(MonthlyTest5Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region MonthlyTest5Validate

        private void MonthlyTest5Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 2);

            for (int i = 0; i < Model.Appointments.Count; i++)
            {
                Appointment app = Model.Appointments[i];

                Assert.True(app.OwnerKey.Equals(calName));

                AppointmentRecurrence recur = app.Recurrence;

                Assert.NotNull(recur);
                Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
                Assert.True(recur.Monthly.RepeatInterval == 1);
                Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

                Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
                Assert.True(recur.RangeNumberOfOccurrences == 4);
                Assert.True(recur.RecurrenceStartDate == app.StartTime);
            }

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(4));

            Assert.True(appc.Count == 10);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count / 2; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                date = date.AddMonths(1);
            }

            date = new DateTime(2010, 4, 15, 9, 0, 0);

            for (int i = 0; i < appc.Count / 2; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                date = date.AddMonths(1);
            }
        }

        #endregion

        #endregion

        #region MonthlyTest6: Every 6 months on the 10th thru 15th, 5x

        //BEGIN:VEVENT
        //SUMMARY:Every 6 months on the 10th thru 15th of the month for 5 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Nov 10-15,May 10-15...
        //DTSTART;TZID=America/New_York:20101110T090000
        //RRULE:FREQ=MONTHLY;INTERVAL=6;COUNT=5;BYMONTHDAY=10,11,12,13,14,15
        //EXDATE:20111113T090000
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void MonthlyTest6()
        {
            DateTime dateStart = new DateTime(2010, 11, 10, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "MonthlyTest6";
            const string summary = "Every 6 months on the 10th thru 15th of the month for 5 occurrences";
            const string description = "(2010 9:00 AM EDT) Nov 10-15,May 10-15...";
            const string rrule = "FREQ=MONTHLY;INTERVAL=6;COUNT=5;BYMONTHDAY=10,11,12,13,14,15";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(MonthlyTest6Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region MonthlyTest6Validate

        private void MonthlyTest6Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 6);

            for (int i = 0; i < Model.Appointments.Count; i++)
            {
                Appointment app = Model.Appointments[i];

                Assert.True(app.OwnerKey.Equals(calName));

                AppointmentRecurrence recur = app.Recurrence;

                Assert.NotNull(recur);
                Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
                Assert.True(recur.Monthly.RepeatInterval == 6);
                Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

                Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
                Assert.True(recur.RangeNumberOfOccurrences == 4);
                Assert.True(recur.RecurrenceStartDate == app.StartTime);
            }

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(20));

            Assert.True(appc.Count == 30);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count / 6; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);
                Assert.True(AppcContainsDate(appc, date.AddDays(1)) == true);
                Assert.True(AppcContainsDate(appc, date.AddDays(2)) == true);
                Assert.True(AppcContainsDate(appc, date.AddDays(3)) == true);
                Assert.True(AppcContainsDate(appc, date.AddDays(4)) == true);
                Assert.True(AppcContainsDate(appc, date.AddDays(5)) == true);

                date = date.AddMonths(6);
            }
        }

        #endregion

        #endregion

        #region MonthlyTest7: Monthly, every 2 months, 1st, 15th, 20th

        //BEGIN:VEVENT
        //SUMMARY:Monthly, every 2 months, 1st, 15th, 20th
        //RRULE:FREQ=MONTHLY;INTERVAL=2;BYMONTHDAY=1,15,20
        //DTSTART;VALUE=DATE:20100915
        //DTEND;VALUE=DATE:20020916
        //END:VEVENT

        [Test]
        public void MonthlyTest7()
        {
            DateTime dateStart = new DateTime(2010, 9, 15, 0, 0, 0);
            DateTime dateEnd = dateStart.AddDays(1);

            const string calName = "MonthlyTest7";
            const string summary = "Monthly, every 2 months, 1st, 15th, 20th";

            const string description = "(2010 00:00 AM EDT) Sept 15,20;Nov 1,15,20;Jan 1,15,20;" +
                                       "Mar 1,15,20;May 1,15,20;Jul 1,15,20;Sept 1,15";

            const string rrule = "FREQ=MONTHLY;INTERVAL=2;BYMONTHDAY=1,15,20";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(MonthlyTest7Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region MonthlyTest7Validate

        private void MonthlyTest7Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 3);

            for (int i = 0; i < Model.Appointments.Count; i++)
            {
                Appointment app = Model.Appointments[i];

                Assert.True(app.OwnerKey.Equals(calName));

                AppointmentRecurrence recur = app.Recurrence;

                Assert.NotNull(recur);
                Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
                Assert.True(recur.Monthly.RepeatInterval == 2);
                Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

                Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.NoEndDate);
                Assert.True(recur.RangeNumberOfOccurrences == 0);
                Assert.True(recur.RecurrenceStartDate == app.StartTime);
            }

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 19);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(AppcContainsDate(appc, date) == true);

                switch (date.Day)
                {
                    case 1:
                        date = date.AddDays(14);
                        break;

                    case 15:
                        date = date.AddDays(5);
                        break;

                    default:
                        for (int j = 0; j < 2; j++)
                            date = date.AddMonths(1);

                        date = date.AddDays(-date.Day + 1);
                        break;
                }
            }
        }

        #endregion

        #endregion
    }

    [TestFixture]
    public class ImportTestYearly : ImportTestBase
    {
        #region YearlyTest1: Yearly in March for 2x

        //BEGIN:VEVENT
        //SUMMARY:Yearly in March for 2 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Mar 24,...
        //DTSTART;TZID=America/New_York:20100324T090000
        //RRULE:FREQ=YEARLY;COUNT=2;BYMONTH=3
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void YearlyTest1()
        {
            DateTime dateStart = new DateTime(2010, 3, 24, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "YearlyTest1";
            const string summary = "Yearly in March for 2 occurrences";
            const string description = "(2010 9:00 AM EDT) Mar 24,...";
            const string rrule = "FREQ=YEARLY;COUNT=2;BYMONTH=3";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(YearlyTest1Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region YearlyTest1Validate

        private void YearlyTest1Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Yearly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 1);
            Assert.True(recur.RecurrenceStartDate == Model.Appointments[0].StartTime);

            DateTime dt1 = new DateTime(2009, 7, 1);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dt1, dt1.AddYears(1));

            Assert.True(appc.Count == 1);

            dt1 = dt1.AddYears(1);

            appc = new AppointmentSubsetCollection(Model, dt1, dt1.AddYears(1));
            Assert.True(appc.Count == 1);

            appc = new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(30));
            Assert.True(appc.Count == 2);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(AppcContainsDate(appc, date));
                date = date.AddYears(1);
            }
        }

        #endregion

        #endregion

        #region YearlyTest2: Every 1st Monday, and 2nd and 4th Thursday in April, 3x

        //BEGIN:VEVENT
        //SUMMARY:Every 1st Monday, and 2nd and 4th Thursday in April, 3x
        //DESCRIPTION:(2010 9:00 AM EDT) 
        //DTSTART;TZID=America/New_York:20100405T090000
        //RRULE:FREQ=YEARLY;COUNT=3;BYMONTH=4;BYDAY=1MO,2TH,4TH
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void YearlyTest2()
        {
            DateTime dateStart = new DateTime(2010, 4, 5, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "YearlyTest2";
            const string summary = "Every 1st Monday, and 2nd and 4th Thursday in April, 3x";
            const string description = "(2010 9:00 AM EDT) Apr 5,8,22; Apr 4,14,28; Apr 2,12,26";
            const string rrule = "FREQ=YEARLY;COUNT=3;BYMONTH=4;BYDAY=1MO,2TH,4TH";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(YearlyTest2Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region YearlyTest2Validate

        private void YearlyTest2Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 3);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Yearly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 2);
            Assert.True(recur.RecurrenceStartDate == Model.Appointments[0].StartTime);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(30));

            Assert.True(appc.Count == 9);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i += 3)
            {
                DateTime date1 = GetFirstDate(date, DayOfWeek.Monday);
                Assert.True(AppcContainsDate(appc, date1));

                date1 = GetRelativeDate(date, DayOfWeek.Thursday, 2);
                Assert.True(AppcContainsDate(appc, date1));

                date1 = GetRelativeDate(date, DayOfWeek.Thursday, 4);
                Assert.True(AppcContainsDate(appc, date1));

                date = date.AddYears(1);
            }
        }

        #endregion

        #endregion

        #region YearlyTest3: Yearly on Sept 17

        //BEGIN:VEVENT
        //SUMMARY:Yearly on Sept 17
        //DESCRIPTION:(2010 9:00 AM EDT) Mar 24,...
        //DTSTART;TZID=America/New_York:20100917T000000
        //DTEND;TZID=America/New_York:20100918T000000
        //RRULE:FREQ=YEARLY
        //END:VEVENT

        [Test]
        public void YearlyTest3()
        {
            DateTime dateStart = new DateTime(2010, 3, 24, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "YearlyTest3";
            const string summary = "Yearly on Sept 17";
            const string description = "(2010 9:00 AM EDT) Sept 17,...";
            const string rrule = "FREQ=YEARLY";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(YearlyTest3Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region YearlyTest3Validate

        private void YearlyTest3Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Yearly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.NoEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == Model.Appointments[0].StartTime);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(30));

            Assert.True(appc.Count == 31);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Assert.True(AppcContainsDate(appc, date));
                date = date.AddYears(1);
            }
        }

        #endregion

        #endregion

        #region YearlyTest4: Yearly on Sept 17 with RDate

        //BEGIN:VEVENT
        //SUMMARY:Yearly on Sept 17
        //DESCRIPTION:(2010 9:00 AM EDT) Mar 24,...
        //DTSTART;TZID=America/New_York:20100917T000000
        //DTEND;TZID=America/New_York:20100918T000000
        //RRULE:FREQ=YEARLY
        //RDATE;VALUE=PERIOD:20100920T100000z/20100920T140000z,
        // 20100922T120000z/PT3H
        //END:VEVENT

        [Test]
        public void YearlyTest4()
        {
            DateTime dateStart = new DateTime(2010, 9, 17, 10, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "YearlyTest4";
            const string summary = "Yearly on Sept 17";
            const string description = "(2010 9:00 AM EDT) Sept 17,20,22;...";
            const string rrule = "FREQ=YEARLY";

            CalendarBegin(calName);

            AddOutput("BEGIN:VEVENT");
            AddVEventBody(summary, description, dateStart, dateEnd, null, rrule);
            AddOutput("RDATE;VALUE=PERIOD:20100920T100000/20100920T140000,20100922T120000/PT3H45M");
            AddOutput("END:VEVENT");

            CalendarEnd();

            PerformTest(YearlyTest4Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region YearlyTest4Validate

        private void YearlyTest4Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 3);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Yearly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.NoEndDate);
            Assert.True(recur.RangeNumberOfOccurrences == 0);
            Assert.True(recur.RecurrenceStartDate == Model.Appointments[0].StartTime);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(30));

            Assert.True(appc.Count == 33);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count - 2; i++)
            {
                Assert.True(AppcContainsDate(appc, date));
                date = date.AddYears(1);
            }

            date = new DateTime(2010, 9, 20, 10, 0, 0);

            Appointment app = AppcGetApp(appc, date);

            Assert.NotNull(app);

            if (app != null)
                Assert.True((app.EndTime - app.StartTime).TotalMinutes == 4 * 60);

            date = new DateTime(2010, 9, 22, 12, 0, 0);

            app = AppcGetApp(appc, date);

            Assert.NotNull(app);

            if (app != null)
                Assert.True((app.EndTime - app.StartTime).TotalMinutes == 3 * 60 + 45);
        }

        #endregion

        #endregion

        #region YearlyTest5: Every year, on the 4th and 12th of Jan, Feb, and May for 10x

        //BEGIN:VEVENT
        //SUMMARY:Yearly in January, February, and May for 10 occurrences
        //DESCRIPTION:(2010 9:00 AM EDT) Jan 4;Feb 4;May 4
        //DTSTART;TZID=America/New_York:20100104T090000
        //RRULE:FREQ=YEARLY;COUNT=10;BYMONTH=1,2,5;BYMONTHDAY=4,12
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void YearlyTest5()
        {
            DateTime dateStart = new DateTime(2010, 1, 4, 9, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "YearlyTest5";
            const string summary = "Yearly in January, February, and May for 10 occurrences";
            const string description = "(2010 9:00 AM EDT) Jan 4;Feb 4;May 4";
            const string rrule = "FREQ=YEARLY;COUNT=10;BYMONTH=1,2,5;BYMONTHDAY=4,12";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateEnd, null, rrule);
            CalendarEnd();

            PerformTest(YearlyTest5Validate, calName, dateStart, DateTime.MaxValue);
        }

        #region YearlyTest5Validate

        private void YearlyTest5Validate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 6);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Yearly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 9);
            Assert.True(recur.RecurrenceStartDate == Model.Appointments[0].StartTime);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(30));

            Assert.True(appc.Count == 60);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count / 3; i += 3)
            {
                DateTime date1 = date;
                DateTime date2 = date.AddMonths(1);
                DateTime date5 = date.AddMonths(4);

                Assert.True(AppcContainsDate(appc, date1));
                Assert.True(AppcContainsDate(appc, date2));
                Assert.True(AppcContainsDate(appc, date5));

                date1 = date1.AddDays(8);
                date2 = date2.AddDays(8);
                date5 = date5.AddDays(8);

                Assert.True(AppcContainsDate(appc, date1));
                Assert.True(AppcContainsDate(appc, date2));
                Assert.True(AppcContainsDate(appc, date5));

                date = date.AddYears(1);
            }
        }

        #endregion

        #endregion
    }

    [TestFixture]
    public class ImportTestMisc : ImportTestBase
    {
        #region DurationTest: Single duration events

        //BEGIN:VEVENT
        //SUMMARY:Single shot events
        //DTSTART;TZID=Canada/Eastern:20100601T00000
        //DURATION:P*
        //END:VEVENT

        [Test]
        public void DurationTest()
        {
            DateTime dateStart = new DateTime(2010, 6, 1, 0, 0, 0);

            const string calName = "DurationTest";
            const string summary = "Single shot events";
            const string description = "(2010 1:00 PM EDT) Jun 1+";

            CalendarBegin(calName);
            AddVEvent(summary, description, dateStart, dateStart, "P2D", null);
            AddVEvent(summary, description, dateStart.AddDays(1), dateStart, "P3DT4H", null);
            AddVEvent(summary, description, dateStart.AddDays(2), dateStart, "P1DT3H45M", null);
            AddVEvent(summary, description, dateStart.AddDays(3), dateStart, "P4DT15M", null);
            AddVEvent(summary, description, dateStart.AddDays(4), dateStart, "PT4H", null);
            AddVEvent(summary, description, dateStart.AddDays(5), dateStart, "PT2H15M", null);
            AddVEvent(summary, description, dateStart.AddDays(6), dateStart, "PT12M", null);
            AddVEvent(summary, description, dateStart.AddDays(7), dateStart, "PT7M30S", null);
            CalendarEnd();

            PerformTest(DurationTestValidate, calName, dateStart, DateTime.MaxValue);
        }

        #region DurationTestValidate

        private void DurationTestValidate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 8);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 8);

            for (int i = 0; i < appc.Count; i++)
            {
                Appointment app = appc[i];

                Assert.Null(app.Recurrence);
                Assert.True(app.OwnerKey.Equals(calName));

                int actMinutes = 0;
                int appMinutes = (int)(app.EndTime - app.StartTime).TotalMinutes;

                switch (app.StartTime.Day)
                {
                    case 1: actMinutes = 60 * 48; break;
                    case 2: actMinutes = 60 * 76; break;
                    case 3: actMinutes = 60 * 27 + 45; break;
                    case 4: actMinutes = 60 * 96 + 15; break;
                    case 5: actMinutes = 60 * 4; break;
                    case 6: actMinutes = 60 * 2 + 15; break;
                    case 7: actMinutes = 12; break;
                    case 8: actMinutes = 7; break;

                    default:
                        Assert.True(false, "Invalid duration date.");
                        break;
                }

                Assert.True(appMinutes == actMinutes, "Incorrect duration.");
            }
        }

        #endregion

        #endregion

        #region ExDateTest: Daily, 30x, ExDate Jul 2,16

        //BEGIN:VEVENT
        //SUMMARY:Daily, 30x, ExDate Jul 2,16
        //EXDATE;TZID=Canada/Eastern:20100702T130000
        //EXDATE;TZID=Canada/Eastern:20100716T130000
        //DTSTART;TZID=Canada/Eastern:20100618T130000
        //DTEND;TZID=Canada/Eastern:20100618T150000
        //RRULE:FREQ=DAILY;COUNT=10
        //END:VEVENT

        [Test]
        public void ExDateTest()
        {
            DateTime dateStart = new DateTime(2010, 6, 18, 13, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "ExDateTest";
            const string summary = "Daily, 10x, ExDate Jul 2,16";
            const string description = "(2010 1:00 PM EDT) Jun 18... ExDate Jul 2,16";

            const string duration = "PT2H";
            const string rrule = "FREQ=DAILY;COUNT=30";

            CalendarBegin(calName);

            AddOutput("BEGIN:VEVENT");
            AddOutput("EXDATE;TZID=Canada/Eastern:20100702T130000");
            AddVEventBody(summary, description, dateStart, dateEnd, duration, rrule);
            AddOutput("EXDATE;TZID=Canada/Eastern:20100716T130000");
            AddOutput("END:VEVENT");

            CalendarEnd();

            PerformTest(ExDateTestValidate, calName, dateStart, DateTime.MaxValue);
        }

        #region ExDateTestValidate

        private void ExDateTestValidate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 1);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 29);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 30);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                if (date.Month == 7)
                {
                    if (date.Day == 2 || date.Day == 16)
                    {
                        Assert.True(AppcContainsDate(appc, date) == false);
                        i--;
                    }
                }
                else
                {
                    Assert.True(AppcContainsDate(appc, date) == true);
                }

                date = date.AddDays(1);
            }
        }

        #endregion

        #endregion

        #region XDNBTest: Daily, 10x

        //BEGIN:VEVENT
        //SUMMARY:Daily, 10x
        //DTSTART;TZID=Canada/Eastern:20100618T130000
        //DTEND;TZID=Canada/Eastern:20100618T150000
        //RRULE:FREQ=DAILY;COUNT=10
        //X-DNB-CATEGORYCOLOR:Purple
        //X-DNB-DISPLAYTEMPLATE:<font color=\"Blue\">[StartTime] - [EndTime]</font><br />
        //X-DNB-IMAGEALIGN:MiddleRight
        //X-DNB-IMAGEKEY:OpenFolder
        //X-DNB-LOCKED:true
        //X-DNB-TIMEMARKEDAS:OutOfOffice
        //X-DNB-TOOLTIP:MyToolTip is, of course, the best tooltip!
        //END:VEVENT

        [Test]
        public void XdnbTest()
        {
            DateTime dateStart = new DateTime(2010, 6, 18, 13, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "XDNBTest";
            const string summary = "Daily, 10x";
            const string description = "(2010 1:00 PM EDT) Jun 18...";

            const string duration = "PT2H";
            const string rrule = "FREQ=DAILY;COUNT=10";

            CalendarBegin(calName);

            AddOutput("BEGIN:VEVENT");
            AddOutput("X-DNB-CATEGORYCOLOR:Purple");
            AddOutput("X-DNB-DISPLAYTEMPLATE:<font color=\"Blue\">[StartTime] - [EndTime]</font><br />");
            AddOutput("X-DNB-IMAGEALIGN:MiddleRight");
            AddOutput("X-DNB-IMAGEKEY:OpenFolder");

            AddVEventBody(summary, description, dateStart, dateEnd, duration, rrule);

            AddOutput("X-DNB-LOCKED:true");
            AddOutput("X-DNB-TIMEMARKEDAS:OutOfOffice");
            AddOutput("X-DNB-TOOLTIP:MyToolTip is, of course, the best tooltip!");
            AddOutput("END:VEVENT");

            CalendarEnd();

            PerformTest(XdnbTestValidate, calName, dateStart, DateTime.MaxValue);
        }

        #region XdnbTestValidate

        private void XdnbTestValidate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Daily);
            Assert.True(recur.Daily.RepeatInterval == 1);
            Assert.True(recur.Daily.RepeatOnDaysOfWeek == eDailyRecurrenceRepeat.All);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 9);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 10);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                Appointment app = appc[i];

                Assert.True(app.CategoryColor.Equals(Appointment.CategoryPurple), "Incorrect CategoryColor.");
                Assert.True(app.DisplayTemplate.Equals("<font color=\"Blue\">[StartTime] - [EndTime]</font><br />"), "Incorrect DisplayTemplate.");
                Assert.True(app.ImageAlign == eImageContentAlignment.MiddleRight, "Incorrect ImageAlign.");
                Assert.True(app.ImageKey.Equals("OpenFolder"), "Incorrect ImageKey.");
                Assert.True(app.Locked, "Incorrect Locked value.");
                Assert.True(app.TimeMarkedAs.Equals(Appointment.TimerMarkerOutOfOffice), "Incorrect TimeMarkedAs.");
                Assert.True(app.Tooltip.Equals("MyToolTip is, of course, the best tooltip!"), "Incorrect Tooltip.");

                Assert.True(AppcContainsDate(appc, date), "Invalid Date.");
                date = date.AddDays(1);
            }
        }

        #endregion

        #endregion

        #region RecIdTest: Daily, 10x

        //BEGIN:VEVENT
        //SUMMARY:Monthly, 15th of month, 24x
        //DTSTART;TZID=US/Eastern:20100115T130000
        //DTEND;TZID=US/Eastern:20100115T160000
        //RRULE:FREQ=MONTHLY;BYMONTHDAY=15
        //END:VEVENT

        //BEGIN:VEVENT
        //RECURRENCE-ID;TZID=US/Eastern:20100615T130000
        //DTSTART;TZID=US/Eastern:20100615T150000
        //DURATION:PT4H45M
        //END:VEVENT

        [Test]
        public void RecIdTest()
        {
            DateTime dateStart = new DateTime(2010, 1, 15, 13, 0, 0);
            DateTime dateEnd = new DateTime(2010, 1, 15, 16, 0, 0);

            const string calName = "RecIdTest";
            const string summary = "Daily, 10x";
            const string description = "(2010 1:00 PM EDT) Jan 15, Feb 15...";
            const string rrule = "FREQ=MONTHLY;COUNT=10;BYMONTHDAY=15";

            CalendarBegin(calName);

            string uid = AddVEvent(summary, description, dateStart, dateEnd, null, rrule);

            AddOutput("BEGIN:VEVENT");
            AddOutput("UID:" + uid);
            AddOutput("RECURRENCE-ID:20100615T130000");
            AddOutput("DTSTART:20100615T153000");
            AddOutput("DURATION:PT4H45M");
            AddOutput("END:VEVENT");

            CalendarEnd();

            PerformTest(RecIdTestValidate, calName, dateStart, DateTime.MaxValue);
        }

        #region RecIdTestValidate

        private void RecIdTestValidate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 2);
            Assert.True(Model.Appointments[0].OwnerKey.Equals(calName));

            AppointmentRecurrence recur = Model.Appointments[0].Recurrence;

            Assert.NotNull(recur);
            Assert.True(recur.RecurrenceType == eRecurrencePatternType.Monthly);
            Assert.True(recur.Monthly.RepeatInterval == 1);
            Assert.True(recur.Monthly.RepeatOnDayOfMonth == 15);
            Assert.True(recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None);

            Assert.True(recur.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences);
            Assert.True(recur.RangeNumberOfOccurrences == 9);
            Assert.True(recur.RecurrenceStartDate == dateStart);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 11);

            DateTime date = dateStart;

            for (int i = 0; i < appc.Count; i++)
            {
                if (date.Month == 6)
                {
                    DateTime dateRecId = new DateTime(2010, 06, 15, 15, 30, 0);

                    Appointment app = AppcGetApp(appc, dateRecId);

                    Assert.True(app != null, "Incorrect RecurrenceId date.");

                    if (app != null)
                        Assert.True((app.EndTime - app.StartTime).TotalMinutes == 285, "Incorrect RecurrenceId duration.");
                }
                else
                {
                    Appointment app = AppcGetApp(appc, date);

                    Assert.True(app != null, "Invalid Date.");

                    if (app != null)
                        Assert.True((app.EndTime - app.StartTime).TotalMinutes == 180, "Incorrect normal duration.");
                }

                date = date.AddMonths(1);
            }
        }

        #endregion

        #endregion

        #region LongValueTest: Single shot

        //BEGIN:VEVENT
        //SUMMARY:Paris (8 km) Prologue
        //DESCRIPTION:PARIS\nThe 2003 Tour de France begins and ends in 
        // Paris.\n\nThe first finish took place on the pink track of the Parc des 
        // Princes\, then on the Bois de Vincennes municipal track (the \"Cipale\"\, 
        // Jacques Anquetil Velodrome).\n\nSince 1975\, the finish takes place 
        // against the prestigious backdrop of the Champs-Elysées (\"the most 
        // beautiful avenue in the world\"). \r\n\nThe road opened in the XVIth 
        // Century by Marie de Médicis is now the most famous in Paris\, situated 
        // between Concorde and the Arc-de-Triomphe.
        //DTSTART;VALUE=DATE:20030705
        //DTEND;VALUE=DATE:20030706
        //END:VEVENT

        private string _Description;

        [Test]
        public void LongValueTest()
        {
            DateTime dateStart = new DateTime(2010, 1, 15, 0, 0, 0);
            DateTime dateEnd = new DateTime(2010, 1, 16, 0, 0, 0);

            const string calName = "LongValueTest";
            const string summary = "LongValueTest - Single shot";

            _Description = "PARIS\\nThe 2003 Tour de France begins and ends in " +
                "Paris.\\n\\nThe first finish took place on the pink track of the Parc des " +
                "Princes\\, then on the Bois de Vincennes municipal track (the \"Cipale\"\\, " +
                "Jacques Anquetil Velodrome).\\n\\nSince 1975\\, the finish takes place " +
                "against the prestigious backdrop of the Champs-Elysées (\"the most " +
                "beautiful avenue in the world\"). \\n\\nThe road opened in the XVIth " +
                "Century by Marie de Médicis is now the most famous in Paris\\, situated " +
                "between Concorde and the Arc-de-Triomphe.";

            CalendarBegin(calName);
            AddVEvent(summary, _Description, dateStart, dateEnd, null, null);
            CalendarEnd();

            PerformTest(LongValueTestValidate, calName, dateStart, DateTime.MaxValue);
        }

        #region AddMetaData

        /// <summary>
        /// Adds escape chars to text meta data
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string AddMetaData(string s)
        {
            Regex p = new Regex("[,;\\n]");
            MatchCollection mc = p.Matches(s);

            if (mc.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                int index = 0;

                foreach (Match ma in mc)
                {
                    if (index < ma.Index)
                        sb.Append(s.Substring(index, ma.Index - index));

                    index = ma.Index + ma.Length;

                    char c = ma.Value[0];

                    if (c == '\n')
                        c = 'n';

                    sb.Append('\\');
                    sb.Append(c);
                }

                if (index < s.Length)
                    sb.Append(s.Substring(index, s.Length - index));

                return (sb.ToString());
            }

            return (s);
        }

        #endregion

        #region LongValueTestValidate

        private void LongValueTestValidate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);

            Appointment app = Model.Appointments[0];

            Assert.True(app.OwnerKey.Equals(calName));
            Assert.Null(app.Recurrence);

            string metaDesc = AddMetaData(app.Description);
            Assert.True(metaDesc.Equals(_Description), "Incorrect description");

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 1);
        }

        #endregion

        #endregion

        #region VTimeZone: Single shot

        //BEGIN:VTIMEZONE
        //TZID:/softwarestudio.org/Olson_20011030_5/Europe/London

        //BEGIN:STANDARD
        //TZOFFSETFROM:+0100
        //TZOFFSETTO:+0000
        //TZNAME:GMT
        //DTSTART:19701025T020000
        //RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=10
        //END:STANDARD

        //BEGIN:DAYLIGHT
        //TZOFFSETFROM:+0000
        //TZOFFSETTO:+0100
        //TZNAME:BST
        //DTSTART:19700329T010000
        //RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=3
        //END:DAYLIGHT

        //END:VTIMEZONE

        //BEGIN:VEVENT
        //SEQUENCE:1
        //SUMMARY:VTimeZone Test
        //DTSTART;TZID=/softwarestudio.org/Olson_20011030_5/Europe/London:20100708T100000
        //DURATION:PT2H
        //END:VEVENT

        [Test]
        public void VTimeZone()
        {
            DateTime dateStart = new DateTime(2010, 7, 7, 10, 0, 0);
            DateTime dateEnd = dateStart.AddHours(2);

            const string calName = "VTimeZone";
            const string summary = "VTimeZone - Single shot";

            const string description = "Jul 7th, ";

            CalendarBegin(calName);

            string uid = Guid.NewGuid().ToString();
            string sDateStart = GetDateTimeText(dateStart);

            AddOutput("BEGIN:VEVENT");
            AddOutput("SUMMARY:" + summary);
            AddOutput("DESCRIPTION:" + description);
            AddOutput("UID:" + uid);
            AddOutput("DTSTAMP:" + sDateStart);
            AddOutput("DTSTART;TZID=/softwarestudio.org/Olson_20011030_5/Europe/London:" + sDateStart);
            AddOutput("DTEND;TZID=/softwarestudio.org/Olson_20011030_5/Europe/London:" + GetDateTimeText(dateEnd));
            AddOutput("End:VEVENT");

            AddOutput("BEGIN:VTIMEZONE");
            AddOutput("TZID:/softwarestudio.org/Olson_20011030_5/Europe/London");

            AddOutput("BEGIN:STANDARD");
            AddOutput("TZOFFSETFROM:+0100");
            AddOutput("TZOFFSETTO:+0000");
            AddOutput("TZNAME:GMT");
            AddOutput("DTSTART:19701025T020000");
            AddOutput("RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=10");
            AddOutput("END:STANDARD");

            AddOutput("BEGIN:DAYLIGHT");
            AddOutput("TZOFFSETFROM:+0000");
            AddOutput("TZOFFSETTO:+0100");
            AddOutput("TZNAME:BST");
            AddOutput("DTSTART:19700329T010000");
            AddOutput("RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=3");
            AddOutput("END:DAYLIGHT");

            AddOutput("END:VTIMEZONE");

            CalendarEnd();

            PerformTest(VTimeZoneValidate, calName, dateStart, DateTime.MaxValue);
        }

        #region VTimeZoneValidate

        private void VTimeZoneValidate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);

            Appointment app = Model.Appointments[0];

            Assert.True(app.OwnerKey.Equals(calName));
            Assert.Null(app.Recurrence);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart.AddDays(-1), dateStart.AddYears(1));

            Assert.True(appc.Count == 1);
            Assert.True(app.StartTime == dateStart.AddHours(-7));
        }

        #endregion

        #endregion

        #region VAlarmTest

        //BEGIN:VEVENT
        //SUMMARY:OLN: Road to the Tour
        //DTSTART:20100719T081500
        //DTEND:20100719T081600

        //BEGIN:VALARM
        //TRIGGER;VALUE=DURATION:-P15DT7H12M
        //ACTION:DISPLAY
        //DESCRIPTION:Event reminder 15 days, 7 hours, and 12 min before.
        //END:VALARM

        //BEGIN:VALARM
        //TRIGGER;VALUE=DURATION:PT0M
        //ACTION:DISPLAY
        //DESCRIPTION:Event start time reached.
        //END:VALARM

        //BEGIN:VALARM
        //TRIGGER;VALUE=DURATION:PT1M
        //ACTION:DISPLAY
        //DESCRIPTION:Event reminder 1 min after.
        //END:VALARM
        //END:VEVENT

        private const string DescrBefore = "Event reminder 15 days, 7 hours, and 12 min before.";
        private const string DescrStart = "Event start time reached.";
        private const string DescrAfter = "Event reminder 1 min after.";

        [Test]
        public void VAlarmTest()
        {
            DateTime dateStart = new DateTime(2010, 7, 20, 8, 15, 0);
            DateTime dateEnd = dateStart.AddMinutes(30);

            const string calName = "VAlarmTest";
            const string summary = "OLN: Road to the Tour";

            const string description = "Jul 20th, ";

            CalendarBegin(calName);

            string uid = Guid.NewGuid().ToString();
            string sDateStart = GetDateTimeText(dateStart);

            AddOutput("BEGIN:VEVENT");

            AddOutput("SUMMARY:" + summary);
            AddOutput("DESCRIPTION:" + description);
            AddOutput("UID:" + uid);
            AddOutput("DTSTAMP:" + sDateStart);
            AddOutput("DTSTART:" + sDateStart);
            AddOutput("DTEND:" + GetDateTimeText(dateEnd));

            AddOutput("BEGIN:VALARM");
            AddOutput("TRIGGER;VALUE=DURATION:-P15DT7H12M");
            AddOutput("ACTION:DISPLAY");
            AddOutput("DESCRIPTION:" + DescrBefore);
            AddOutput("END:VALARM");

            AddOutput("BEGIN:VALARM");
            AddOutput("TRIGGER;VALUE=DURATION:PT0M");
            AddOutput("ACTION:DISPLAY");
            AddOutput("DESCRIPTION:" + DescrStart);
            AddOutput("END:VALARM");

            AddOutput("BEGIN:VALARM");
            AddOutput("TRIGGER;VALUE=DURATION:PT1M");
            AddOutput("TZNAME:BST");
            AddOutput("DESCRIPTION:" + DescrAfter);
            AddOutput("END:VALARM");

            AddOutput("END:VEVENT");

            CalendarEnd();

            PerformTest(VAlarmTestValidate, calName, dateStart, DateTime.MaxValue);
        }

        #region VAlarmTestValidate

        private void VAlarmTestValidate(string calName, DateTime dateStart, DateTime rangeEnd)
        {
            Assert.True(Model.Appointments.Count == 1);

            Appointment app = Model.Appointments[0];

            Assert.True(app.OwnerKey.Equals(calName));
            Assert.Null(app.Recurrence);

            AppointmentSubsetCollection appc =
                new AppointmentSubsetCollection(Model, dateStart, dateStart.AddYears(1));

            Assert.True(appc.Count == 1);
            Assert.True(app.StartTime == dateStart);

            Assert.NotNull(app.Reminders);

            if (app.Reminders != null)
            {
                Assert.True(app.Reminders.Count == 2);

                foreach (Reminder rem in app.Reminders)
                {
                    switch (rem.Description)
                    {
                        case DescrBefore:
                            Assert.True(rem.ReminderTime ==
                                        dateStart.AddDays(-15).AddHours(-7).AddMinutes(-12));
                            break;

                        case DescrAfter:
                            Assert.True(rem.ReminderTime == dateStart.AddMinutes(1));
                            break;

                        default:
                            Assert.Fail("Invalid Reminder.");
                            break;
                    }
                }

                Assert.True(app.StartTimeAction == eStartTimeAction.StartTimeReachedEvent);
            }
        }

        #endregion

        #endregion
    }

    #region ImportTestBase

    public class ImportTestBase
    {
        #region Private variables

        private CalendarView _CalendarView;
        private CalendarModel _Model;
        private IcsImporter _Importer;
        private IcsExporter _Exporter;

        private MemoryStream _MemoryStream;
        private StreamReader _StreamReader;
        private StreamWriter _StreamWriter;

        #endregion

        #region Public properties

        public CalendarModel Model
        {
            get { return (_Model); }
            set { _Model = value; }
        }

        public MemoryStream MemoryStream
        {
            get { return (_MemoryStream); }
            set { _MemoryStream = value; }
        }

        public StreamReader StreamReader
        {
            get { return (_StreamReader); }
            set { _StreamReader = value; }
        }

        public StreamWriter StreamWriter
        {
            get { return (_StreamWriter); }
            set { _StreamWriter = value; }
        }

        public IcsImporter Importer
        {
            get { return (_Importer); }
        }

        public IcsExporter Exporter
        {
            get { return (_Exporter); }
        }
        
        #endregion

        #region SetUp

        [SetUp]
        protected void SetUp()
        {
            Model = new CalendarModel();

            _CalendarView = new CalendarView();
            _CalendarView.CalendarModel = Model;

            _Importer = new IcsImporter(Model);
            _Exporter = new IcsExporter(Model);

            _MemoryStream = new MemoryStream();
            _StreamWriter = new StreamWriter(_MemoryStream);
            _StreamReader = new StreamReader(MemoryStream);
        }

        #endregion

        #region TearDown

        [TearDown]
        protected void TearDown()
        {
            if (_StreamWriter != null)
                _StreamWriter.Dispose();

            if (_StreamReader != null)
                _StreamReader.Dispose();

            if (_MemoryStream != null)
                _MemoryStream.Dispose();
        }

        #endregion

        #region CalendarBegin

        public void CalendarBegin(string calName)
        {
            _StreamWriter.BaseStream.SetLength(0);

            _StreamWriter.WriteLine("BEGIN:VCALENDAR");
            _StreamWriter.WriteLine("CALSCALE:GREGORIAN");
            _StreamWriter.WriteLine("X-WR-TIMEZONE;VALUE=TEXT:Canada/Eastern");
            _StreamWriter.WriteLine("PRODID:-//DNB Components\\, Inc//iCal 1.0//EN");
            _StreamWriter.WriteLine("X-WR-CALNAME;VALUE=TEXT:" + calName);
            _StreamWriter.WriteLine("VERSION:2.0");
        }

        #endregion

        #region CalendarEnd

        public void CalendarEnd()
        {
            _StreamWriter.WriteLine("END:VCALENDAR");
            _StreamWriter.Flush();
        }

        #endregion

        #region AddOutput

        public void AddOutput(string text)
        {
            _StreamWriter.WriteLine(text);
        }

        #endregion

        #region AddVEvent

        public string AddVEvent(string summary, string description,
            DateTime dateStart, DateTime dateEnd, string duration, string rrule)
        {
            _StreamWriter.WriteLine("BEGIN:VEVENT");

            string uid = AddVEventBody(summary, description, dateStart, dateEnd, duration, rrule);

            _StreamWriter.WriteLine("END:VEVENT");

            return (uid);
        }

        #endregion

        #region AddVEventBody

        public string AddVEventBody(string summary, string description,
            DateTime dateStart, DateTime dateEnd, string duration, string rrule)
        {
            string uid = Guid.NewGuid().ToString();
            string sDateStart = GetDateTimeText(dateStart);

            _StreamWriter.WriteLine("SUMMARY:" + summary);
            _StreamWriter.WriteLine("DESCRIPTION:" + description);
            _StreamWriter.WriteLine("UID:" + uid);
            _StreamWriter.WriteLine("DTSTAMP:" + sDateStart);
            _StreamWriter.WriteLine("DTSTART:" + sDateStart);

            if (duration != null)
                _StreamWriter.WriteLine("DURATION:" + duration);
            else
                _StreamWriter.WriteLine("DTEND:" + GetDateTimeText(dateEnd));

            if (rrule != null)
                _StreamWriter.WriteLine("RRULE:" + rrule);

            return (uid);
        }

        #endregion

        #region GetDateTimeText

        public string GetDateTimeText(DateTime date)
        {
            return (date.ToString("yyyyMMdd\\tHHmms"));
        }

        #endregion

        #region GetFirstDate

        public DateTime GetFirstDate(DateTime date, DayOfWeek dayOfWeek)
        {
            DateTime dt = date.AddDays(-date.Day + 1);

            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(1);

            return (dt);
        }

        #endregion

        #region GetNextFirstDate

        public DateTime GetNextFirstDate(DateTime date, DayOfWeek dayOfWeek)
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            DateTime dt = date.AddDays(calendar.GetDaysInMonth(date.Year, date.Month));

            return (GetFirstDate(dt, dayOfWeek));
        }

        #endregion

        #region GetRelativeDate

        public DateTime GetRelativeDate(DateTime date, DayOfWeek dayOfWeek, int rel)
        {
            DateTime dt = GetFirstDate(date, dayOfWeek);

            while (--rel > 0)
                dt = dt.AddDays(7);

            return (dt);
        }

        #endregion

        #region GetNextRelativeDate

        public DateTime GetNextRelativeDate(DateTime date, DayOfWeek dayOfWeek, int rel)
        {
            DateTime dt = GetNextFirstDate(date, dayOfWeek);

            while (--rel > 0)
                dt = dt.AddDays(7);

            return (dt);
        }

        #endregion

        #region GetNextLastDate

        public DateTime GetNextLastDate(DateTime date, DayOfWeek dayOfWeek)
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            DateTime dt = date.AddDays(calendar.GetDaysInMonth(date.Year, date.Month) - date.Day + 1);

            dt = dt.AddDays(calendar.GetDaysInMonth(dt.Year, dt.Month) - 1);

            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(-1);

            return (dt);
        }

        #endregion

        #region AppcContainsDate

        public bool AppcContainsDate(IEnumerable<Appointment> appc, DateTime date)
        {
            return (AppcGetApp(appc, date) != null);
        }

        #endregion

        #region AppcGetApp

        public Appointment AppcGetApp(IEnumerable<Appointment> appc, DateTime date)
        {
            foreach (Appointment app in appc)
            {
                if (app.StartTime == date)
                    return (app);
            }

            return (null);
        }

        #endregion

        #region GetValidDateTime

        public DateTime GetValidDateTime(DateTime date)
        {
            if (date == DateTime.MinValue || date == DateTime.MaxValue || date.Kind == DateTimeKind.Utc)
                return (date);

            TimeZoneInfo timeZone = TimeZoneInfo.Local;

            if (timeZone.IsInvalidTime(date))
            {
                while (timeZone.IsInvalidTime(date))
                    date = date.AddHours(1);
            }
            return date;
        }

        #endregion

        #region PerformTest

        public void PerformTest(ValidateHandler validate,
            string calName, DateTime dateStart, DateTime rangeEnd)
        {
            // Import and validate the data

            Model.Appointments.Clear();
            StreamReader.BaseStream.Position = 0;
            Importer.Import(StreamReader);

            validate(calName, dateStart, rangeEnd);

            // Export the Model data

            StreamWriter.BaseStream.SetLength(0);
            Exporter.Export(StreamWriter);

            // Re-import and validate the exported data

            Model.Appointments.Clear();
            StreamReader.BaseStream.Position = 0;
            Importer.Import(StreamReader);

            validate(calName, dateStart, rangeEnd);
        }

        #endregion
    }

    #endregion
}
