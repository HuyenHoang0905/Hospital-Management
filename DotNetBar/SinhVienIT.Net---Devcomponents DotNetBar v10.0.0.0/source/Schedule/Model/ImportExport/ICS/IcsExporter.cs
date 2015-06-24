using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DevComponents.Schedule.Model.Serialization
{
    /// <summary>
    /// Export DNB internal Model/Appointment data layout into
    /// ICS (Internet Calendaring and Scheduling - RFC5545) format file.
    /// </summary>
    public class IcsExporter
    {
        #region Private constants

        private const int FoldCount = 74;

        #endregion

        #region Private variables

        private DateTime _DtStamp = DateTime.Now;

        private string _ProdId = "PRODID:-//DotNetBar\\, Inc//iCal 1.0//EN";

        private CalendarModel _Model;

        private string[] _CalNames;
        private string[] _OwnerKeys;

        private string _ExportFile;
        private StreamWriter _StreamWriter;

        #endregion

        #region Constructors

        /// <summary>
        /// IcsExporter
        /// </summary>
        public IcsExporter()
        {
        }

        /// <summary>
        /// IcsExporter
        /// </summary>
        /// <param name="model"></param>
        public IcsExporter(CalendarModel model)
        {
            Model = model;
        }

        #endregion

        #region Public properties

        #region ProdId

        /// <summary>
        /// Gets or sets the Calendar Product Id.
        /// </summary>
        public string ProdId
        {
            get { return (_ProdId); }
            set { _ProdId = value; }
        }

        #endregion

        #region Model

        /// <summary>
        /// CalendarModel
        /// </summary>
        public CalendarModel Model
        {
            get { return (_Model); }
            set { _Model = value; }
        }

        #endregion

        #endregion

        #region Export

        /// <summary>
        /// Exports Appointment data in the iCalendar format
        /// </summary>
        /// <param name="ownerKeys">Array of OwnerKeys</param>
        /// <param name="calNames">Array of CalendarNames</param>
        /// <param name="streamWriter">Export StreamWriter</param>
        public void Export(string[] ownerKeys, string[] calNames, StreamWriter streamWriter)
        {
            if (_Model == null)
                throw new Exception("Model can not be null.");

            _CalNames = calNames;
            _OwnerKeys = ownerKeys;

            if (streamWriter == null)
                throw new Exception("Invalid StreamWriter.");

            _StreamWriter = streamWriter;

            if (String.IsNullOrEmpty(_ExportFile) == true)
                _ExportFile = streamWriter.BaseStream.ToString();

            ExportOwnerData();

            streamWriter.Flush();
        }

        #region Export variations

        /// <summary>
        /// Exports all appointments to the given export file.
        /// </summary>
        /// <param name="exportFile">Output file path</param>
        public void Export(string exportFile)
        {
            Export((string[])null, null, exportFile);
        }

        /// <summary>
        /// Exports all appointments to the given export stream.
        /// </summary>
        /// <param name="streamWriter">Output StreamWriter</param>
        public void Export(StreamWriter streamWriter)
        {
            Export((string[])null, null, streamWriter);
        }

        /// <summary>
        /// Exports all appointments for the specified OwnerKey to
        /// the given export file.
        /// </summary>
        /// <param name="ownerKey">Appointment OwnerKey</param>
        /// <param name="exportFile">Output file path</param>
        public void Export(string ownerKey, string exportFile)
        {
            Export(ownerKey, ownerKey, exportFile);
        }

        /// <summary>
        /// Exports all appointments for the specified OwnerKey to
        /// the given export stream.
        /// </summary>
        /// <param name="ownerKey">Appointment OwnerKey</param>
        /// <param name="streamWriter">Output StreamWriter</param>
        public void Export(string ownerKey, StreamWriter streamWriter)
        {
            Export(ownerKey, ownerKey, streamWriter);
        }

        /// <summary>
        /// Exports all appointments for the specified OwnerKey to
        /// the given export file, using the specified calendar name.
        /// </summary>
        /// <param name="ownerKey">Appointment OwnerKey</param>
        /// <param name="calName">Associated Calendar Name</param>
        /// <param name="exportFile">Output file path</param>
        public void Export(string ownerKey, string calName, string exportFile)
        {
            string[] ownerKeys = new string[] { ownerKey };
            string[] calNames = new string[] { calName };

            Export(ownerKeys, calNames, exportFile);
        }

        /// <summary>
        /// Exports all appointments for the specified OwnerKey to
        /// the given export stream, using the specified calendar name.
        /// </summary>
        /// <param name="ownerKey">Appointment OwnerKey</param>
        /// <param name="calName">Associated Calendar Name</param>
        /// <param name="streamWriter">Output StreamWriter</param>
        public void Export(string ownerKey, string calName, StreamWriter streamWriter)
        {
            string[] ownerKeys = new string[] { ownerKey };
            string[] calNames = new string[] { calName };

            Export(ownerKeys, calNames, streamWriter);
        }

        /// <summary>
        /// Exports all appointments for the specified OwnerKey array to
        /// the given export file, using the specified associated calendar name array.
        /// </summary>
        /// <param name="ownerKeys">Array of OwnerKeys</param>
        /// <param name="calNames">Array of 1:1 associated Calendar Names</param>
        /// <param name="exportFile">Output file path</param>
        public virtual void Export(string[] ownerKeys, string[] calNames, string exportFile)
        {
            using (StreamWriter streamWriter = new StreamWriter(exportFile))
                Export(ownerKeys, calNames, streamWriter);
        }

        #endregion

        #endregion

        #region ExportOwnerData

        /// <summary>
        /// Exports model data for each requested OwnerKey
        /// </summary>
        private void ExportOwnerData()
        {
            if (_OwnerKeys == null)
                _OwnerKeys = GetDefaultOwnerKeys();

            if (_CalNames == null)
                _CalNames = _OwnerKeys;

            string lastCalName = null;

            for (int i = 0; i < _OwnerKeys.Length; i++)
            {
                string ownerKey = _OwnerKeys[i] ?? "";

                string calName = ((i < _CalNames.Length)
                                      ? _CalNames[i]
                                      : _CalNames[_CalNames.Length - 1]) ?? ownerKey;

                if (calName.Equals(lastCalName) == false)
                {
                    if (lastCalName != null)
                        ExportCalEnd();

                    ExportCalBegin(calName);

                    lastCalName = calName;
                }

                foreach (Appointment app in Model.Appointments)
                {
                    if (IsAppointmentVisible(app, ownerKey) == true)
                        ExportVEvent(app);
                }
            }

            if (lastCalName != null)
                ExportCalEnd();
        }

        #endregion

        #region ExportCalBegin

        /// <summary>
        /// Exports the beginning calendar sequence
        /// </summary>
        /// <param name="calName"></param>
        private void ExportCalBegin(string calName)
        {
            ExportLine("BEGIN:VCALENDAR");
            ExportLine("CALSCALE:GREGORIAN");

            if (_ProdId != null)
                ExportLine(_ProdId);

            ExportLine("X-WR-CALNAME;VALUE=TEXT:" + calName);
            ExportLine("VERSION:2.0");
        }

        #endregion

        #region ExportCalEnd

        /// <summary>
        /// Exports the ending calendar sequence
        /// </summary>
        private void ExportCalEnd()
        {
            ExportLine("END:VCALENDAR\n");
        }

        #endregion

        #region ExportVEvent

        #region ExportVEvent

        /// <summary>
        /// Exports a calendar event (appointment)
        /// </summary>
        /// <param name="app"></param>
        private void ExportVEvent(Appointment app)
        {
            ExportLine("\nBEGIN:VEVENT");

            if (String.IsNullOrEmpty(app.Subject) == false)
                ExportLine("SUMMARY:" + AddMetaData(app.Subject));

            if (String.IsNullOrEmpty(app.Description) == false)
                ExportLine("DESCRIPTION:" + AddMetaData(app.Description));

            ExportLine("UID:" + Guid.NewGuid().ToString());
            ExportLine("DTSTAMP:" + GetUniversalTime(_DtStamp));

            if (String.IsNullOrEmpty(app.CategoryColor) == false)
                ExportLine("X-DNB-CATEGORYCOLOR:" + app.CategoryColor);

            if (String.IsNullOrEmpty(app.DisplayTemplate) == false)
                ExportLine("X-DNB-DISPLAYTEMPLATE:" + AddMetaData(app.DisplayTemplate));

            if (app.ImageAlign != eImageContentAlignment.TopLeft)
                ExportLine("X-DNB-IMAGEALIGN:" + Enum.GetName(typeof(eImageContentAlignment), app.ImageAlign));

            if (String.IsNullOrEmpty(app.ImageKey) == false)
                ExportLine("X-DNB-IMAGEKEY:" + app.ImageKey);

            if (app.Locked == true)
                ExportLine("X-DNB-LOCKED:true");

            if (app.StartTimeAction != eStartTimeAction.None)
                ExportLine("X-DNB-STARTTIMEACTION:" + Enum.GetName(typeof(eStartTimeAction), app.StartTimeAction));

            if (String.IsNullOrEmpty(app.TimeMarkedAs) == false)
                ExportLine("X-DNB-TIMEMARKEDAS:" + app.TimeMarkedAs);

            if (String.IsNullOrEmpty(app.Tooltip) == false)
                ExportLine("X-DNB-TOOLTIP:" + AddMetaData(app.Tooltip));

            if ((app.StartTime.Hour == 0 && app.StartTime.Minute == 0) &&
                (app.EndTime.Hour == 0 && app.EndTime.Minute == 0))
            {
                ExportLine("DTSTART;VALUE=DATE:" + app.StartTime.ToString("yyyyMMdd"));

                if ((app.EndTime - app.StartTime).TotalMinutes != 60 * 24)
                    ExportLine("DTEND;VALUE=DATE:" + app.EndTime.ToString("yyyyMMdd"));
            }
            else
            {
                ExportLine("DTSTART:" + GetUniversalTime(app.StartTime));
                ExportLine("DTEND:" + GetUniversalTime(app.EndTime));
            }

            ExportRRule(app);
            ExportAlarms(app);

            ExportLine("END:VEVENT");
        }

        #region ExportRRule

        #region ExportRRule

        /// <summary>
        /// Exports the appointment Recurrence Rule
        /// </summary>
        /// <param name="app"></param>
        private void ExportRRule(Appointment app)
        {
            if (app.Recurrence != null)
            {
                switch (app.Recurrence.RecurrenceType)
                {
                    case eRecurrencePatternType.Daily:
                        ExportDailyRRule(app.Recurrence);
                        break;

                    case eRecurrencePatternType.Weekly:
                        ExportWeeklyRRule(app.Recurrence);
                        break;

                    case eRecurrencePatternType.Monthly:
                        ExportMonthlyRRule(app.Recurrence);
                        break;

                    default:
                        ExportYearlyRRule(app);
                        break;
                }

                ExportExDate(app.Recurrence);
            }
        }

        #endregion

        #region ExportDailyRRule

        /// <summary>
        /// ExportDailyRRule
        /// </summary>
        /// <param name="recur"></param>
        private void ExportDailyRRule(AppointmentRecurrence recur)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("RRULE:FREQ=DAILY");

            AddRRuleInterval(sb, recur.Daily.RepeatInterval);
            AddRRuleByDay(sb, recur.Daily.RepeatOnDaysOfWeek);

            AddRRuleRangeInfo(sb, recur);

            ExportLine(sb.ToString());
        }

        #endregion

        #region ExportWeeklyRRule

        /// <summary>
        /// ExportWeeklyRRule
        /// </summary>
        /// <param name="recur"></param>
        private void ExportWeeklyRRule(AppointmentRecurrence recur)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("RRULE:FREQ=WEEKLY");

            AddRRuleInterval(sb, recur.Weekly.RepeatInterval);

            if (recur.Weekly.RepeatOnDaysOfWeek != eDayOfWeekRecurrence.None)
                AddRRuleByDay(sb, recur.Weekly.RepeatOnDaysOfWeek);

            AddRRuleRangeInfo(sb, recur);

            ExportLine(sb.ToString());
        }

        #endregion

        #region ExportMonthlyRRule

        /// <summary>
        /// ExportMonthlyRRule
        /// </summary>
        /// <param name="recur"></param>
        private void ExportMonthlyRRule(AppointmentRecurrence recur)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("RRULE:FREQ=MONTHLY");

            AddRRuleInterval(sb, recur.Monthly.RepeatInterval);

            if (recur.Monthly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None)
                sb.AppendFormat("{0}{1:D}", ";BYMONTHDAY=", recur.Monthly.RepeatOnDayOfMonth);
            else
                AddRRuleByDay(sb, recur.Monthly.RepeatOnRelativeDayInMonth, recur.Monthly.RelativeDayOfWeek);

            AddRRuleRangeInfo(sb, recur);

            ExportLine(sb.ToString());
        }

        #endregion

        #region ExportYearlyRRule

        /// <summary>
        /// ExportYearlyRRule
        /// </summary>
        /// <param name="app"></param>
        private void ExportYearlyRRule(Appointment app)
        {
            AppointmentRecurrence recur = app.Recurrence;

            StringBuilder sb = new StringBuilder();

            sb.Append("RRULE:FREQ=YEARLY");
            sb.AppendFormat("{0}{1:D}", ";BYMONTH=", recur.Yearly.RepeatOnMonth);

            if (recur.Yearly.RepeatOnRelativeDayInMonth == eRelativeDayInMonth.None)
            {
                if (recur.Yearly.RepeatOnDayOfMonth != app.StartTime.Day)
                    sb.AppendFormat("{0}{1:D}", ";BYMONTHDAY=", recur.Yearly.RepeatOnDayOfMonth);
            }
            else
            {
                AddRRuleByDay(sb, recur.Yearly.RepeatOnRelativeDayInMonth, recur.Yearly.RelativeDayOfWeek);
            }

            AddRRuleRangeInfo(sb, recur);

            ExportLine(sb.ToString());
        }

        #endregion

        #region AddRRuleByDay

        /// <summary>
        /// AddRRuleByDay
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="repeat"></param>
        private void AddRRuleByDay(StringBuilder sb, eDayOfWeekRecurrence repeat)
        {
            sb.Append(";BYDAY=");

            if ((repeat & eDayOfWeekRecurrence.Sunday) != 0)
                sb.Append("SU,");

            if ((repeat & eDayOfWeekRecurrence.Monday) != 0)
                sb.Append("MO,");

            if ((repeat & eDayOfWeekRecurrence.Tuesday) != 0)
                sb.Append("TU,");

            if ((repeat & eDayOfWeekRecurrence.Wednesday) != 0)
                sb.Append("WE,");

            if ((repeat & eDayOfWeekRecurrence.Thursday) != 0)
                sb.Append("TH,");

            if ((repeat & eDayOfWeekRecurrence.Friday) != 0)
                sb.Append("FR,");

            if ((repeat & eDayOfWeekRecurrence.Saturday) != 0)
                sb.Append("SA,");

            sb.Length--;
        }

        #endregion

        #region AddRRuleByDay

        /// <summary>
        /// AddRRuleByDay
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="repeat"></param>
        private void AddRRuleByDay(StringBuilder sb, eDailyRecurrenceRepeat repeat)
        {
            sb.Append(";BYDAY=");

            switch (repeat)
            {
                case eDailyRecurrenceRepeat.All:
                    sb.Append(";BYDAY=SU,MO,TU,WE,TH,FR,SA");
                    break;

                case eDailyRecurrenceRepeat.WeekDays:
                    sb.Append(";BYDAY=MO,TU,WE,TH,FR");
                    break;

                default:
                    sb.Append(";BYDAY=SA,SU");
                    break;
            }
        }

        #endregion

        #region AddRRuleByDay

        /// <summary>
        /// AddRRuleByDay
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="rday"></param>
        /// <param name="dow"></param>
        private void AddRRuleByDay(StringBuilder sb, eRelativeDayInMonth rday, DayOfWeek dow)
        {
            sb.Append(";BYDAY=");

            switch (rday)
            {
                case eRelativeDayInMonth.First:
                    sb.Append("1");
                    break;

                case eRelativeDayInMonth.Second:
                    sb.Append("2");
                    break;

                case eRelativeDayInMonth.Third:
                    sb.Append("3");
                    break;

                case eRelativeDayInMonth.Fourth:
                    sb.Append("4");
                    break;

                case eRelativeDayInMonth.Last:
                    sb.Append("-1");
                    break;
            }

            switch (dow)
            {
                case DayOfWeek.Sunday:
                    sb.Append("SU");
                    break;

                case DayOfWeek.Monday:
                    sb.Append("MO");
                    break;

                case DayOfWeek.Tuesday:
                    sb.Append("TU");
                    break;

                case DayOfWeek.Wednesday:
                    sb.Append("WE");
                    break;

                case DayOfWeek.Thursday:
                    sb.Append("TH");
                    break;

                case DayOfWeek.Friday:
                    sb.Append("FR");
                    break;

                default:
                    sb.Append("SA");
                    break;
            }
        }

        #endregion

        #region AddRRuleInterval

        /// <summary>
        /// AddRRuleInterval
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="interval"></param>
        private void AddRRuleInterval(StringBuilder sb, int interval)
        {
            if (interval > 1)
                sb.Append(";INTERVAL=" + interval.ToString());
        }

        #endregion

        #region AddRRuleRangeInfo

        /// <summary>
        /// AddRRuleRangeInfo
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="recur"></param>
        private void AddRRuleRangeInfo(StringBuilder sb, AppointmentRecurrence recur)
        {
            switch (recur.RangeLimitType)
            {
                case eRecurrenceRangeLimitType.RangeNumberOfOccurrences:
                    if (recur.RangeNumberOfOccurrences > 0)
                        sb.Append(";COUNT=" + (recur.RangeNumberOfOccurrences + 1).ToString());
                    break;

                case eRecurrenceRangeLimitType.RangeEndDate:
                    sb.Append(";UNTIL=" + GetUtcDate(recur.RangeEndDate));
                    break;
            }

            if (recur.RecurrenceStartDate != DateTime.MinValue && recur.RecurrenceStartDate != recur.Appointment.StartTime)
                ExportLine("X-DNB-RECSTARTDATE=" + GetUtcDate(recur.RecurrenceStartDate));
        }

        #endregion

        #endregion

        #region ExportExDate

        /// <summary>
        /// ExportExDate
        /// </summary>
        /// <param name="recur"></param>
        private void ExportExDate(AppointmentRecurrence recur)
        {
            if (recur.SkippedRecurrences != null)
            {
                foreach (DateTime date in recur.SkippedRecurrences)
                    ExportLine("EXDATE:" + GetUtcDate(date));
            }
        }

        #endregion

        #region ExportAlarms

        /// <summary>
        /// ExportAlarms
        /// </summary>
        /// <param name="app"></param>
        private void ExportAlarms(Appointment app)
        {
            if (app.Reminders != null)
            {
                foreach (Reminder rem in app.Reminders)
                {
                    ExportLine("\nBEGIN:VALARM");

                    if (string.IsNullOrEmpty(rem.Description) == false)
                        ExportLine("DESCRIPTION:" + rem.Description);

                    ExportLine("TRIGGER;VALUE=DURATION:" + GetDuration(rem.ReminderTime - app.StartTime));
                    ExportLine("ACTION:DISPLAY");
                    ExportLine("END:VALARM");
                }
            }

            if (app.StartTimeAction != eStartTimeAction.None)
            {
                ExportLine("\nBEGIN:VALARM");
                ExportLine("TRIGGER;VALUE=DURATION:PT0M");
                ExportLine("ACTION:DISPLAY");
                ExportLine("END:VALARM\n");
            }
        }

        #endregion

        #region GetDuration

        /// <summary>
        /// GetDuration
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        private string GetDuration(TimeSpan ts)
        {
            StringBuilder sb = new StringBuilder();

            if (ts.TotalMinutes < 0)
                sb.Append("-");

            sb.Append("P");

            int totalMinutes = (int)Math.Abs(ts.TotalMinutes);

            int days = totalMinutes / (24 * 60);

            if (days > 0)
            {
                sb.Append(days + "D");
                totalMinutes %= (24 * 60);
            }

            if (totalMinutes > 0)
            {
                sb.Append("T");

                int hours = totalMinutes / 60;

                if (hours > 0)
                {
                    sb.Append(hours + "H");
                    totalMinutes %= 60;
                }

                if (totalMinutes > 0)
                    sb.Append(totalMinutes + "M");
            }

            return (sb.ToString());
        }

        #endregion

        #region GetUtcDate

        /// <summary>
        /// GetUtcDate
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private string GetUtcDate(DateTime dateTime)
        {
            DateTime date = dateTime.ToUniversalTime();

            return (date.ToString("yyyyMMdd\\tHHmms\\z"));
        }

        #endregion

        #endregion

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

        #endregion

        #region ExportLine

        /// <summary>
        /// Exports a line
        /// </summary>
        /// <param name="text"></param>
        private void ExportLine(string text)
        {
            _StreamWriter.WriteLine(WrapLine(text, FoldCount));
        }

        #region WrapLine

        /// <summary>
        /// Performs line wrapping (aka folding)
        /// </summary>
        /// <param name="text">Text to wrap</param>
        /// <param name="wrapLength">Wrapping length</param>
        /// <returns></returns>
        private string WrapLine(string text, int wrapLength)
        {
            if (text.Length <= wrapLength)
                return (text);

            StringBuilder sb = new StringBuilder();

            int len;

            for (int i = 0; i < text.Length; i += len)
            {
                len = text.Length - i;

                if (len > wrapLength)
                    len = BreakLine(text, i, wrapLength);

                if (i > 0)
                    sb.Append("\n ");

                sb.Append(text, i, len);
            }

            return (sb.ToString());
        }

        #endregion

        #region BreakLine

        /// <summary>
        /// Determines where to break a line of text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="pos">Current text pos</param>
        /// <param name="max">Max line length</param>
        /// <returns></returns>
        private int BreakLine(string text, int pos, int max)
        {
            int i = max - 1;

            while (i > 0 && IsBreakChar(text[pos + i]) == false)
                i--;

            if (i <= 0)
                return (max);

            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]) == true)
                i--;

            return (i + 1);
        }

        #endregion

        #region IsBreakChar

        /// <summary>
        /// Determines if a char is a break char
        /// </summary>
        /// <param name="c"></param>
        /// <returns>true if break char</returns>
        private bool IsBreakChar(char c)
        {
            return (Char.IsWhiteSpace(c) || c == ',' || c == ';');
        }

        #endregion

        #endregion

        #region GetDefaultOwnerKeys

        /// <summary>
        /// GetDefaultOwnerKeys
        /// </summary>
        /// <returns>A list of all defined model OwnerKeys</returns>
        private string[] GetDefaultOwnerKeys()
        {
            List<string> list = new List<string>();

            foreach (Appointment app in Model.Appointments)
            {
                string key = app.OwnerKey ?? "";

                if (list.Contains(app.OwnerKey) == false)
                    list.Add(key);
            }

            list.Sort();

            string[] ownerKeys = new string[list.Count];
            list.CopyTo(ownerKeys);

            return (ownerKeys);
        }

        #endregion

        #region GetUniversalTime

        /// <summary>
        /// GetUniversalTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetUniversalTime(DateTime date)
        {
            return (date.ToUniversalTime().ToString("yyyyMMdd\\tHHmms\\z"));
        }

        #endregion

        #region IsAppointmentVisible

        /// <summary>
        /// Determines if an appointment is visible
        /// with respect to the given ownerKey
        /// </summary>
        /// <param name="app"></param>
        /// <param name="ownerKey"></param>
        /// <returns></returns>
        protected virtual bool IsAppointmentVisible(Appointment app, string ownerKey)
        {
            if (string.IsNullOrEmpty(ownerKey))
                return (true);

            return (app.OwnerKey == ownerKey);
        }

        #endregion
    }
}