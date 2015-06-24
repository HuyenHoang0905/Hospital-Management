using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DevComponents.Schedule.Model.Serialization
{
    /// <summary>
    /// ICS (Internet Calendaring and Scheduling - RFC5545) import into
    /// corresponding DNB internal Model/Appointment data layout
    /// </summary>
    public class IcsImporter
    {
        #region Static data

        #region Component token data

        private static TokenRegistry[] _components = new TokenRegistry[]
        {
            new TokenRegistry("daylight", (int)ComponentToken.Daylight),
            new TokenRegistry("standard", (int)ComponentToken.Standard),
            new TokenRegistry("valarm", (int)ComponentToken.VAlarm),
            new TokenRegistry("vcalendar", (int)ComponentToken.VCalendar),
            new TokenRegistry("vevent", (int)ComponentToken.VEvent),
            new TokenRegistry("vfreebusy", (int)ComponentToken.VFreeBusy),
            new TokenRegistry("vjournal", (int)ComponentToken.VJournal),
            new TokenRegistry("vtimezone", (int)ComponentToken.VTimezone),
            new TokenRegistry("vtodo", (int)ComponentToken.VTodo),
        };

        #endregion

        #region Property token data

        private static TokenRegistry[] _properties = new TokenRegistry[]
        {
            //new TokenRegistry("action", (int)PropertyToken.Action),
            //new TokenRegistry("attach", (int)PropertyToken.Attach),
            //new TokenRegistry("attendee", (int)PropertyToken.Attendee),
            new TokenRegistry("begin", (int)PropertyToken.Begin),
            new TokenRegistry("calscale", (int)PropertyToken.CalScale),
            //new TokenRegistry("categories", (int)PropertyToken.Categories),
            //new TokenRegistry("class", (int)PropertyToken.Class),
            new TokenRegistry("comment", (int)PropertyToken.Comment),
            //new TokenRegistry("completed", (int)PropertyToken.Completed),
            //new TokenRegistry("contact", (int)PropertyToken.Contact),
            //new TokenRegistry("created", (int)PropertyToken.Created),
            new TokenRegistry("description", (int)PropertyToken.Description),
            new TokenRegistry("dtend", (int)PropertyToken.DtEnd),
            new TokenRegistry("dtstamp", (int)PropertyToken.DtStamp),
            new TokenRegistry("dtstart", (int)PropertyToken.DtStart),
            new TokenRegistry("due", (int)PropertyToken.Due),
            new TokenRegistry("duration", (int)PropertyToken.Duration),
            new TokenRegistry("end", (int)PropertyToken.End),
            new TokenRegistry("exdate", (int)PropertyToken.ExDate),
            //new TokenRegistry("exrule", (int)PropertyToken.ExRule),
            //new TokenRegistry("freebusy", (int)PropertyToken.FreeBusy),
            //new TokenRegistry("geo", (int)PropertyToken.Geo),
            //new TokenRegistry("last-modified", (int)PropertyToken.LastModified),
            //new TokenRegistry("location", (int)PropertyToken.Location),
            //new TokenRegistry("method", (int)PropertyToken.Method),
            //new TokenRegistry("organizer", (int)PropertyToken.Organizer),
            //new TokenRegistry("percent-complete", (int)PropertyToken.PercentComplete),
            //new TokenRegistry("priority", (int)PropertyToken.Priority),
            //new TokenRegistry("progid", (int)PropertyToken.ProgId),
            new TokenRegistry("rdate", (int)PropertyToken.RDate),
            new TokenRegistry("recurrence-id", (int)PropertyToken.RecurrenceId),
            //new TokenRegistry("related-to", (int)PropertyToken.RelatedTo),
            //new TokenRegistry("repeat", (int)PropertyToken.Repeat),
            //new TokenRegistry("request-status", (int)PropertyToken.RequestStatus),
            //new TokenRegistry("resources", (int)PropertyToken.Resources),
            new TokenRegistry("rrule", (int)PropertyToken.RRule),
            //new TokenRegistry("sequence", (int)PropertyToken.Sequence),
            //new TokenRegistry("status", (int)PropertyToken.Status),
            new TokenRegistry("summary", (int)PropertyToken.Summary),
            //new TokenRegistry("transp", (int)PropertyToken.Transp),
            new TokenRegistry("trigger", (int)PropertyToken.Trigger),
            new TokenRegistry("tzid", (int)PropertyToken.TzId),
            new TokenRegistry("tzname", (int)PropertyToken.TzName),
            new TokenRegistry("tzoffsetfrom", (int)PropertyToken.TzOffsetFrom),
            new TokenRegistry("tzoffsetto", (int)PropertyToken.TzOffsetTo),
            //new TokenRegistry("tzurl", (int)PropertyToken.TzUrl),
            new TokenRegistry("uid", (int)PropertyToken.Uid),
            //new TokenRegistry("url", (int)PropertyToken.Url),
            new TokenRegistry("version", (int)PropertyToken.Version),
            new TokenRegistry("x-dnb-categorycolor", (int)PropertyToken.XDnbCategoryColor),
            new TokenRegistry("x-dnb-displaytemplate", (int)PropertyToken.XDnbDisplayTemplate),
            new TokenRegistry("x-dnb-imagealign", (int)PropertyToken.XDnbImageAlign),
            new TokenRegistry("x-dnb-imagekey", (int)PropertyToken.XDnbImageKey),
            new TokenRegistry("x-dnb-locked", (int)PropertyToken.XDnbLocked),
            new TokenRegistry("x-dnb-recstartdate", (int)PropertyToken.XDnbRecStartDate),
            new TokenRegistry("x-dnb-starttimeaction", (int)PropertyToken.XDnbStartTimeAction),
            new TokenRegistry("x-dnb-timemarkedas", (int)PropertyToken.XDnbTimeMarkedAs),
            new TokenRegistry("x-dnb-tooltip", (int)PropertyToken.XDnbTooltip),
            new TokenRegistry("x-wr-calname", (int)PropertyToken.XWrCalname),
        };

        #endregion

        #region Parameter token data

        private static TokenRegistry[] _parameters = new TokenRegistry[]
        {
            //new TokenRegistry("altrep", (int)eParameterToken.AltRep),
            //new TokenRegistry("cn", (int)eParameterToken.Cn),
            //new TokenRegistry("cutype", (int)eParameterToken.CuType),
            //new TokenRegistry("delegated-from", (int)eParameterToken.DelegatedFrom),
            //new TokenRegistry("delegated-to", (int)eParameterToken.DelegatedTo),
            //new TokenRegistry("dir", (int)eParameterToken.Dir),
            //new TokenRegistry("encoding", (int)eParameterToken.Encoding),
            //new TokenRegistry("fmttype", (int)eParameterToken.FmtType),
            //new TokenRegistry("fbtype", (int)eParameterToken.FbType),
            //new TokenRegistry("language", (int)eParameterToken.Language),
            //new TokenRegistry("member", (int)eParameterToken.Member),
            //new TokenRegistry("parstat", (int)eParameterToken.ParStat),
            //new TokenRegistry("range", (int)eParameterToken.Range),
            //new TokenRegistry("related", (int)eParameterToken.Related),
            //new TokenRegistry("reltype", (int)eParameterToken.RelType),
            //new TokenRegistry("role", (int)eParameterToken.Role),
            //new TokenRegistry("rsvp", (int)eParameterToken.Rsvp),
            //new TokenRegistry("sent-by", (int)eParameterToken.SentBy),
            new TokenRegistry("tzid", (int)ParameterToken.TzId),
            new TokenRegistry("value", (int)ParameterToken.Value),
        };

        #endregion

        #region Value token data

        private static TokenRegistry[] _values = new TokenRegistry[]
        {
            new TokenRegistry("byday", (int)ValueToken.ByDay),
            new TokenRegistry("byhour", (int)ValueToken.ByHour),
            new TokenRegistry("byminute", (int)ValueToken.ByMinute),
            new TokenRegistry("bymonth", (int)ValueToken.ByMonth),
            new TokenRegistry("bymonthday", (int)ValueToken.ByMonthDay),
            new TokenRegistry("bysecond", (int)ValueToken.BySecond),
            new TokenRegistry("bysetpos", (int)ValueToken.BySetPos),
            new TokenRegistry("byweekno", (int)ValueToken.ByWeekNo),
            new TokenRegistry("byyearday", (int)ValueToken.ByYearDay),
            new TokenRegistry("count", (int)ValueToken.Count),
            new TokenRegistry("daily", (int)ValueToken.Daily),
            new TokenRegistry("date", (int)ValueToken.Date),
            new TokenRegistry("datetime", (int)ValueToken.DateTime),
            new TokenRegistry("duration", (int)ValueToken.Duration),
            new TokenRegistry("end", (int)ValueToken.End),
            new TokenRegistry("fr", (int)ValueToken.Fr),
            new TokenRegistry("freq", (int)ValueToken.Freq),
            new TokenRegistry("gregorian", (int)ValueToken.Gregorian),
            new TokenRegistry("hourly", (int)ValueToken.Hourly),
            new TokenRegistry("interval", (int)ValueToken.Interval),
            new TokenRegistry("minutely", (int)ValueToken.Minutely),
            new TokenRegistry("mo", (int)ValueToken.Mo),
            new TokenRegistry("monthly", (int)ValueToken.Monthly),
            new TokenRegistry("period", (int)ValueToken.Period),
            new TokenRegistry("sa", (int)ValueToken.Sa),
            new TokenRegistry("secondly", (int)ValueToken.Secondly),
            new TokenRegistry("start", (int)ValueToken.Start),
            new TokenRegistry("su", (int)ValueToken.Su),
            new TokenRegistry("text", (int)ValueToken.Text),
            new TokenRegistry("th", (int)ValueToken.Th),
            new TokenRegistry("tu", (int)ValueToken.Tu),
            new TokenRegistry("until", (int)ValueToken.Until),
            new TokenRegistry("we", (int)ValueToken.We),
            new TokenRegistry("weekly", (int)ValueToken.Weekly),
            new TokenRegistry("wkst", (int)ValueToken.WkSt),
            new TokenRegistry("yearly", (int)ValueToken.Yearly),
        };

        #endregion

        #endregion

        #region Constants

        private const string Version = "2.0";

        #endregion

        #region Constructors

        public IcsImporter()
        {
        }

        public IcsImporter(CalendarModel model)
        {
            Model = model;
        }

        #endregion

        #region Private variables

        private int _EntryIndex;

        private IcsComponent _IncludeComponent = IcsComponent.All;
        private Dictionary<string, List<Appointment>> _Uid = new Dictionary<string, List<Appointment>>();
        private List<CalendarTimeZone> _CalendarTimeZone;

        private List<CalendarEntry> _CalendarEntries = new List<CalendarEntry>();

        private CalendarModel _Model;

        private string[] _CalNames;
        private string[] _OwnerKeys;

        private string _ImportFile;

        private bool _IgnoreErrors;

        #endregion

        #region Public properties

        #region IgnoreErrors

        /// <summary>
        /// Ignore import Errors
        /// </summary>
        public bool IgnoreErrors
        {
            get { return (_IgnoreErrors); }
            set { _IgnoreErrors = value; }
        }

        #endregion

        #region IncludeComponent

        /// <summary>
        /// Gets or sets the iCalendar components to include in the import.
        /// </summary>
        [DefaultValue(IcsComponent.All)]
        [Description("Indicates which iCalendar component to include in the import.")]
        public IcsComponent IncludeComponent
        {
            get { return (_IncludeComponent); }
            set { _IncludeComponent = value; }
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

        #region Import

        /// <summary>
        /// Imports all appointments from the given export file's
        /// calName array Calendar entries, and associates them with the
        /// given OwnerKey array entries.
        /// </summary>
        /// <param name="calNames">Array of Calendar names</param>
        /// <param name="ownerKeys">Array of associated OwnerKeys</param>
        /// <param name="streamReader">Import file path.</param>
        public void Import(string[] calNames, string[] ownerKeys, StreamReader streamReader)
        {
            if (_Model == null)
                throw new Exception("Model can not be null.");

            if (streamReader == null)
                throw new Exception("Invalid StreamReader.");

            _CalNames = calNames;
            _OwnerKeys = ownerKeys;

            if (String.IsNullOrEmpty(_ImportFile) == true)
                _ImportFile = streamReader.BaseStream.ToString();

            _CalendarEntries.Clear();

            Parse(streamReader);

            if (_CalendarEntries.Count > 0)
            {
                CalendarEntry entry = GetFirstEntry();

                try
                {
                    Model.BeginUpdate();

                    ProcessCalendarEntries(entry);
                }
                finally
                {
                    Model.EndUpdate();
                }
            }
        }

        #region Import variations

        /// <summary>
        /// Imports all appointments from the given input file.
        /// </summary>
        /// <param name="importFile">Input file</param>
        public void Import(string importFile)
        {
            Import((string[])null, null, importFile);
        }

        /// <summary>
        /// Imports all appointments from the given input stream.
        /// </summary>
        /// <param name="streamReader">Input file StreamReader</param>
        public void Import(StreamReader streamReader)
        {
            Import((string[])null, null, streamReader);
        }

        /// <summary>
        /// Imports all appointments from the given export file
        /// calName Calendar entry.
        /// </summary>
        /// <param name="calName">Calendar entry name</param>
        /// <param name="importFile">Import file path.</param>
        public void Import(string calName, string importFile)
        {
            Import(calName, calName, importFile);
        }

        /// <summary>
        /// Imports all appointments from the given export stream
        /// calName Calendar entry.
        /// </summary>
        /// <param name="calName">Calendar entry name</param>
        /// <param name="streamReader">Import file StreamReader.</param>
        public void Import(string calName, StreamReader streamReader)
        {
            Import(calName, calName, streamReader);
        }

        /// <summary>
        /// Imports all appointments from the given export file
        /// calName Calendar entry, and associates them with the
        /// given OwnerKey.
        /// </summary>
        /// <param name="calName">Calendar entry name</param>
        /// <param name="ownerKey">Associated OwnerKey</param>
        /// <param name="importFile">Import file path.</param>
        public void Import(string calName, string ownerKey, string importFile)
        {
            string[] calNames = new string[] { calName };
            string[] ownerKeys = new string[] { ownerKey };

            Import(calNames, ownerKeys, importFile);
        }

        /// <summary>
        /// Imports all appointments from the given export stream
        /// calName Calendar entry, and associates them with the
        /// given OwnerKey.
        /// </summary>
        /// <param name="calName">Calendar entry name</param>
        /// <param name="ownerKey">Associated OwnerKey</param>
        /// <param name="streamReader">Import file StreamReader.</param>
        public void Import(string calName, string ownerKey, StreamReader streamReader)
        {
            string[] calNames = new string[] { calName };
            string[] ownerKeys = new string[] { ownerKey };

            Import(calNames, ownerKeys, streamReader);
        }

        /// <summary>
        /// Imports all appointments from the given export file
        /// calName array Calendar entries, and associates them with the
        /// given OwnerKey array entries.
        /// </summary>
        /// <param name="calNames">Array of Calendar names</param>
        /// <param name="ownerKeys">Array of associated OwnerKeys</param>
        /// <param name="importFile">Import file path.</param>
        public void Import(string[] calNames, string[] ownerKeys, string importFile)
        {
            _ImportFile = importFile;

            using (StreamReader streamReader = new StreamReader(importFile))
                Import(calNames, ownerKeys, streamReader);
        }

        #endregion

        #endregion

        #region ProcessCalendarEntries

        /// <summary>
        /// Initiates the processing of each calendar component entry
        /// </summary>
        /// <param name="entry"></param>
        private void ProcessCalendarEntries(CalendarEntry entry)
        {
            while (entry != null)
            {
                switch ((PropertyToken)GetToken(_properties, entry.Id))
                {
                    case PropertyToken.Begin:
                        switch ((ComponentToken)GetToken(_components, entry.Value))
                        {
                            case ComponentToken.VCalendar:
                                ProcessVCalendar();
                                break;
                        }
                        break;
                }

                entry = GetNextEntry();
            }
        }

        #endregion

        #region ProcessVCalendar

        #region ProcessVCalendar

        /// <summary>
        /// Processes individual calendar component entries
        /// </summary>
        private void ProcessVCalendar()
        {
            string ownerKey = "";
            bool validCalName = true;

            CalendarEntry entry = GetNextEntry();

            while (entry != null)
            {
                switch ((PropertyToken) GetToken(_properties, entry.Id))
                {
                    case PropertyToken.Begin:
                        bool include = false;
                        ComponentToken ctkn = (ComponentToken)GetToken(_components, entry.Value);

                        if (validCalName == true)
                        {
                            switch (ctkn)
                            {
                                case ComponentToken.VEvent:
                                    include = (_IncludeComponent & IcsComponent.Event) != 0;

                                    if (include == true)
                                        ProcessVEvent(ownerKey);
                                    break;

                                case ComponentToken.VTimezone:
                                    include = (_IncludeComponent & IcsComponent.Timezone) != 0;

                                    if (include == true)
                                        ProcessVTimezone();
                                    break;

                                case ComponentToken.VTodo:
                                    include = (_IncludeComponent & IcsComponent.Todo) != 0;

                                    if (include == true)
                                        ProcessVTodo(ownerKey);
                                    break;

                                case ComponentToken.VFreeBusy:
                                case ComponentToken.VJournal:
                                    break;

                                default:
                                    ReportError(entry, "Unexpected \"BEGIN:" + entry.Value.ToUpper() + "\"");
                                    break;
                            }
                        }

                        if (include == false)
                            SkipComponent(ctkn);
                        break;

                    case PropertyToken.CalScale:
                        ProcessCalScale(entry);
                        break;

                    case PropertyToken.End:
                        ProcessCalEnd(entry);
                        return;

                    case PropertyToken.Version:
                        ProcessVersion(entry);
                        break;

                    case PropertyToken.XWrCalname:
                        string calName = ProcessWrCalName(entry);

                        validCalName = IsValidCalName(calName);

                        if (validCalName == true)
                            ownerKey = GetOwnerKey(calName);
                        break;
                }

                entry = GetNextEntry();
            }

            ReportError(entry, "Expected \"END:VCALENDAR\"");
        }

        #endregion

        #region ProcessCalEnd

        /// <summary>
        /// Processes final Calendar component termination
        /// </summary>
        /// <param name="entry"></param>
        private void ProcessCalEnd(CalendarEntry entry)
        {
            if ((ComponentToken)GetToken(_components, entry.Value) != ComponentToken.VCalendar)
                ReportError(entry, "Expected \"END:VCALENDAR\"");
        }

        #endregion

        #region ProcessCalScale

        /// <summary>
        /// Processes Calendar Scale (Gregorian only support)
        /// </summary>
        /// <param name="entry"></param>
        private void ProcessCalScale(CalendarEntry entry)
        {
            ValueToken tkn = (ValueToken)GetToken(_values, entry.Value);

            if (tkn != ValueToken.Gregorian)
                ReportError(entry, "Only GREGORIAN calendar scale is supported");
        }

        #endregion

        #region ProcessVersion

        /// <summary>
        /// Processes Calendar version entries
        /// </summary>
        /// <param name="entry"></param>
        private void ProcessVersion(CalendarEntry entry)
        {
            if (entry.Value.Equals(Version) == false)
                ReportError(entry, "iCalendar VERSION 2.0 is required");
        }

        #endregion

        #region ProcessVEvent

        #region ProcessVEvent

        /// <summary>
        /// Processes Event components for the given OwnerKey
        /// </summary>
        /// <param name="ownerKey">Associated OwnerKey</param>
        private void ProcessVEvent(string ownerKey)
        {
            Appointment app = new Appointment();
            app.OwnerKey = ownerKey;

            VEventData evData = new VEventData();

            CalendarEntry entry = GetNextEntry();

            while (entry != null)
            {
                PropertyToken tkn = (PropertyToken)GetToken(_properties, entry.Id);

                switch (tkn)
                {
                    case PropertyToken.Begin:
                        ComponentToken ctkn = (ComponentToken)GetToken(_components, entry.Value);

                        switch (ctkn)
                        {
                            case ComponentToken.VAlarm:
                                ProcessVAlarm(evData);
                                break;

                            default:
                                ReportError(entry, "Unexpected \"BEGIN:" + entry.Value.ToUpper() + "\"");
                                SkipComponent(ctkn);
                                break;
                        }
                        break;

                    case PropertyToken.Description:
                        app.Description = ProcessDescription(entry, evData);
                        break;

                    case PropertyToken.DtEnd:
                        app.EndTime = ProcessDate(entry, ref evData.IsDtEndValue);
                        break;

                    case PropertyToken.DtStart:
                        app.StartTime = ProcessDate(entry, ref evData.IsDtStartValue);
                        break;

                    case PropertyToken.Duration:
                        evData.Duration = ProcessDuration(entry, entry.Value);
                        break;

                    case PropertyToken.End:
                        if ((ComponentToken)GetToken(_components, entry.Value) != ComponentToken.VEvent)
                            ReportError(entry, "Expected \"END:VEVENT\"");

                        ProcessVEventEnd(entry, app, evData);
                        return;

                    case PropertyToken.ExDate:
                        ProcessExDate(entry, evData);
                        break;

                    case PropertyToken.RDate:
                        evData.RecurDate = ProcessRDate(entry, evData.RecurDate);
                        break;

                    case PropertyToken.RecurrenceId:
                        evData.RecIdDate = ProcessRecurrenceId(entry);
                        break;

                    case PropertyToken.RRule:
                        evData.RecurRule = ProcessRRule(entry);
                        break;

                    case PropertyToken.Summary:
                        app.Subject = ProcessSummary(entry, evData);
                        break;

                    case PropertyToken.Uid:
                        evData.UidApps = ProcessUid(entry, app);
                        break;

                    case PropertyToken.XDnbCategoryColor:
                        app.CategoryColor = ProcessXDnbCategoryColor(entry, evData);
                        break;

                    case PropertyToken.XDnbDisplayTemplate:
                        app.DisplayTemplate = ProcessXDnbDisplayTemplate(entry, evData);
                        break;

                    case PropertyToken.XDnbImageAlign:
                        app.ImageAlign = ProcessXDnbImageAlign(entry, evData);
                        break;

                    case PropertyToken.XDnbImageKey:
                        app.ImageKey = ProcessXDnbImageKey(entry, evData);
                        break;

                    case PropertyToken.XDnbLocked:
                        app.Locked = ProcessXDnbLocked(entry, evData);
                        break;

                    case PropertyToken.XDnbRecStartDate:
                        evData.RecStartDate = ProcessXDnbRecStartDate(entry);
                        break;

                    case PropertyToken.XDnbStartTimeAction:
                        app.StartTimeAction = ProcessXDnbStartTimeAction(entry, evData);
                        break;

                    case PropertyToken.XDnbTimeMarkedAs:
                        app.TimeMarkedAs = ProcessXDnbTimeMarkedAs(entry, evData);
                        break;

                    case PropertyToken.XDnbTooltip:
                        app.Tooltip = ProcessXDnbTooltip(entry, evData);
                        break;
                }

                entry = GetNextEntry();
            }

            ReportError(entry, "Expected \"END:VEVENT\"");
        }

        #endregion

        #region ProcessVEventEnd

        #region ProcessVEventEnd

        /// <summary>
        /// Processes Event termination
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessVEventEnd(CalendarEntry entry, Appointment app, VEventData evData)
        {
            if (evData.RecIdDate != DateTime.MinValue)
                ProcessRecurrenceId(entry, app, evData);
            else
                ProcessEventEndEx(Model, entry, app, evData);
        }

        #endregion

        #region ProcessEventEndEx

        /// <summary>
        /// ProcessEventEndEx
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entry"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessEventEndEx(CalendarModel model,
            CalendarEntry entry, Appointment app, VEventData evData)
        {
            if (ValidateAppTime(entry, app, evData) == true)
            {
                if (evData.UidApps == null)
                {
                    evData.UidApps = new List<Appointment>();
                    evData.UidApps.Add(app);
                }

                if (evData.RecurRule != null)
                {
                    switch (evData.RecurRule.Freq)
                    {
                        case Frequency.Daily:
                            ProcessDailyRecurrence(model, app, evData);
                            break;

                        case Frequency.Weekly:
                            ProcessWeeklyRecurrence(model, app, evData);
                            break;

                        case Frequency.Monthly:
                            ProcessMonthlyRecurrence(model, app, evData);
                            break;

                        case Frequency.Yearly:
                            ProcessYearlyRecurrence(model, app, evData);
                            break;
                    }

                    ProcessRDateRange(model, app, evData);
                }
                else
                {
                    model.Appointments.Add(app);
                }

                ProcessVEventReminder(evData);
            }
        }

        #region ValidateAppTime

        private bool ValidateAppTime(CalendarEntry entry, Appointment app, VEventData evData)
        {
            if (app.StartTime == DateTime.MinValue)
            {
                ReportError(entry, "DTSTART not set");
                return (false);
            }

            if (evData.Duration.TotalSeconds != 0)
                app.EndTime = app.StartTime.Add(evData.Duration);

            else if (app.EndTime == DateTime.MinValue)
            {
                app.EndTime = (evData.IsDtStartValue == true)
                                  ? app.StartTime.AddDays(1)
                                  : app.StartTime;
            }

            if (app.StartTime > app.EndTime)
            {
                ReportError(entry, "DTSTART is greater than DTEND");
                return (false);
            }

            evData.StartTime = app.StartTime;
            evData.EndTime = app.EndTime;

            return (true);
        }

        #endregion

        #endregion

        #region ProcessDailyRecurrence

        /// <summary>
        /// Processes Daily recurrences
        /// </summary>
        /// <param name="model"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessDailyRecurrence(CalendarModel model, Appointment app, VEventData evData)
        {
            AppointmentRecurrence recur = new AppointmentRecurrence();

            recur.RecurrenceType = eRecurrencePatternType.Daily;

            recur.Daily.RepeatInterval = evData.RecurRule.Interval;

            switch (evData.RecurRule.ByDays)
            {
                case eDayOfWeekRecurrence.WeekDays:
                    recur.Daily.RepeatOnDaysOfWeek = eDailyRecurrenceRepeat.WeekDays;
                    break;

                case eDayOfWeekRecurrence.WeekendDays:
                    recur.Daily.RepeatOnDaysOfWeek = eDailyRecurrenceRepeat.WeekendDays;
                    break;
            }

            ProcessRecurrenceRange(app, recur, evData);

            app.Recurrence = recur;

            model.Appointments.Add(app);
        }

        #endregion

        #region ProcessWeeklyRecurrence

        /// <summary>
        /// Processes Weekly recurrences
        /// </summary>
        /// <param name="model"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessWeeklyRecurrence(CalendarModel model, Appointment app, VEventData evData)
        {
            AppointmentRecurrence recur = new AppointmentRecurrence();

            recur.RecurrenceType = eRecurrencePatternType.Weekly;

            recur.Weekly.RepeatInterval = evData.RecurRule.Interval;

            recur.Weekly.RepeatOnDaysOfWeek = (evData.RecurRule.ByDays == eDayOfWeekRecurrence.None)
                                                  ? GetRecurrenceDay(app.StartTime)
                                                  : evData.RecurRule.ByDays;

            ProcessRecurrenceRange(app, recur, evData);

            app.Recurrence = recur;

            model.Appointments.Add(app);
        }

        #region GetRecurrenceDay

        private eDayOfWeekRecurrence GetRecurrenceDay(DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return (eDayOfWeekRecurrence.Monday);

                case DayOfWeek.Tuesday:
                    return (eDayOfWeekRecurrence.Tuesday);

                case DayOfWeek.Wednesday:
                    return (eDayOfWeekRecurrence.Wednesday);

                case DayOfWeek.Thursday:
                    return (eDayOfWeekRecurrence.Thursday);

                case DayOfWeek.Friday:
                    return (eDayOfWeekRecurrence.Friday);

                case DayOfWeek.Saturday:
                    return (eDayOfWeekRecurrence.Saturday);

                default:
                    return (eDayOfWeekRecurrence.Sunday);
            }
        }

        #endregion

        #endregion

        #region ProcessMonthlyRecurrence

        #region ProcessMonthlyRecurrence

        /// <summary>
        /// Processes Monthly recurrences
        /// </summary>
        /// <param name="model"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessMonthlyRecurrence(CalendarModel model, Appointment app, VEventData evData)
        {
            RRule rrule = evData.RecurRule;

            if (rrule.ByDays == eDayOfWeekRecurrence.None)
            {
                if (rrule.ByMonthDay != null)
                {
                    foreach (int day in rrule.ByMonthDay)
                        AddMonthlyByMonthDay(model, app, day, evData);
                }
                else
                {
                    AddMonthlyByMonthDay(model, app, app.StartTime.Day, evData);
                }
            }
            else
            {
                for (int i = 0; i < rrule.ByDay.Length; i++)
                {
                    eDayOfWeekRecurrence eday = ((eDayOfWeekRecurrence)(1 << i));
                    DayOfWeek dayOfWeek = GetRelativeDayOfWeek(eday);

                    if ((rrule.ByDays & eday) != 0)
                    {
                        foreach (int byDay in rrule.ByDay[i])
                            AddMonthlyByDay(model, app, dayOfWeek, byDay, evData);
                    }
                }
            }
        }

        #endregion

        #region AddMonthlyByDay

        #region AddMonthlyByDay

        private void AddMonthlyByDay(CalendarModel model,
            Appointment app, DayOfWeek dayOfWeek, int byDay, VEventData evData)
        {
            AppointmentRecurrence recur = new AppointmentRecurrence();

            recur.RecurrenceType = eRecurrencePatternType.Monthly;

            recur.Monthly.RepeatInterval = evData.RecurRule.Interval;
            recur.Monthly.RelativeDayOfWeek = dayOfWeek;
            recur.Monthly.RepeatOnRelativeDayInMonth = GetRelativeDay(byDay);

            if (app.Recurrence != null)
            {
                app = app.Copy();

                app.StartTime = evData.StartTime;
                app.EndTime = evData.EndTime;

                evData.UidApps.Add(app);
            }

            model.Appointments.Add(app);

            DateTime oldStartTime = app.StartTime;

            SetNewByDayAppStartTime(app, dayOfWeek, byDay);

            if (app.StartTime < oldStartTime)
                recur.SkippedRecurrences.Add(app.StartTime);

            ProcessRecurrenceRange(app, recur, evData);

            app.Recurrence = recur;
        }

        #endregion

        #region SetNewByDayAppStartTime

        /// <summary>
        /// SetNewByDayAppStartTime
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="byDay"></param>
        private void SetNewByDayAppStartTime(
            Appointment app, DayOfWeek dayOfWeek, int byDay)
        {
            DateTime date = app.StartTime.AddDays(-app.StartTime.Day + 1);

            if (byDay >= 0)
            {
                while (date.DayOfWeek != dayOfWeek)
                    date = date.AddDays(1);

                if (byDay > 0)
                    date = date.AddDays((byDay - 1) * 7);
            }
            else
            {
                date = date.AddMonths(1).AddDays(-1);

                while (date.DayOfWeek != dayOfWeek)
                    date = date.AddDays(-1);
            }

            if (app.StartTime != date)
                app.MoveTo(date);
        }

        #endregion

        #endregion

        #region AddMonthlyByMonthDay

        private void AddMonthlyByMonthDay(CalendarModel model,
            Appointment app, int byMonthDay, VEventData evData)
        {
            AppointmentRecurrence recur = new AppointmentRecurrence();

            recur.RecurrenceType = eRecurrencePatternType.Monthly;

            recur.Monthly.RepeatOnRelativeDayInMonth = eRelativeDayInMonth.None;
            recur.Monthly.RepeatOnDayOfMonth = byMonthDay;
            recur.Monthly.RepeatInterval = evData.RecurRule.Interval;

            if (app.Recurrence != null)
            {
                app = app.Copy();

                evData.UidApps.Add(app);
            }

            model.Appointments.Add(app);

            int day = DateTime.DaysInMonth(app.StartTime.Year, app.StartTime.Month);

            if (day < byMonthDay)
                byMonthDay = day;

            DateTime startTime = new DateTime(app.StartTime.Year,
                app.StartTime.Month, byMonthDay, app.StartTime.Hour, app.StartTime.Minute, 0);

            if (startTime < app.StartTime)
                recur.SkippedRecurrences.Add(startTime);

            if (app.StartTime != startTime)
                app.MoveTo(startTime);

            ProcessRecurrenceRange(app, recur, evData);

            app.Recurrence = recur;
        }

        #endregion

        #endregion

        #region ProcessYearlyRecurrence

        #region ProcessYearlyRecurrence

        /// <summary>
        /// Processes Yearly recurrences
        /// </summary>
        /// <param name="model"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessYearlyRecurrence(
            CalendarModel model, Appointment app, VEventData evData)
        {
            RRule rrule = evData.RecurRule;

            if (rrule.ByMonth == null)
            {
                rrule.ByMonth = new int[1];
                rrule.ByMonth[0] = app.StartTime.Month;
            }

            foreach (int byMonth in rrule.ByMonth)
            {
                if (rrule.ByDays == eDayOfWeekRecurrence.None)
                {
                    if (rrule.ByMonthDay != null)
                    {
                        foreach (int day in rrule.ByMonthDay)
                            AddYearlyByMonthDay(model, app, byMonth, day, evData);
                    }
                    else
                    {
                        AddYearlyByMonthDay(model, app, byMonth, app.StartTime.Day, evData);
                    }
                }
                else
                {
                    for (int i = 0; i < rrule.ByDay.Length; i++)
                    {
                        eDayOfWeekRecurrence eday = ((eDayOfWeekRecurrence)(1 << i));
                        DayOfWeek dayOfWeek = GetRelativeDayOfWeek(eday);

                        if ((rrule.ByDays & eday) != 0)
                        {
                            foreach (int byDay in rrule.ByDay[i])
                                AddYearlyByDay(model, app, byMonth, dayOfWeek, byDay, evData);
                        }
                    }
                }
            }
        }

        #endregion

        #region AddYearlyByDay

        private void AddYearlyByDay(CalendarModel model, Appointment app,
            int byMonth, DayOfWeek dayOfWeek, int byDay, VEventData evData)
        {
            AppointmentRecurrence recur = new AppointmentRecurrence();

            recur.RecurrenceType = eRecurrencePatternType.Yearly;

            recur.Yearly.RepeatOnMonth = byMonth;
            recur.Yearly.RelativeDayOfWeek = dayOfWeek;
            recur.Yearly.RepeatOnRelativeDayInMonth = GetRelativeDay(byDay);

            if (app.Recurrence != null)
            {
                app = app.Copy();

                evData.UidApps.Add(app);
            }

            model.Appointments.Add(app);

            DateTime oldStartTime = app.StartTime;

            SetNewByDayAppStartTime(app, dayOfWeek, byDay);

            if (app.StartTime < oldStartTime)
                recur.SkippedRecurrences.Add(app.StartTime);

            ProcessRecurrenceRange(app, recur, evData);

            app.Recurrence = recur;
        }

        #endregion

        #region AddYearlyByMonthDay

        private void AddYearlyByMonthDay(CalendarModel model,
            Appointment app, int byMonth, int byMonthDay, VEventData evData)
        {
            AppointmentRecurrence recur = new AppointmentRecurrence();

            recur.RecurrenceType = eRecurrencePatternType.Yearly;

            recur.Yearly.RepeatOnMonth = byMonth;
            recur.Yearly.RepeatOnRelativeDayInMonth = eRelativeDayInMonth.None;
            recur.Yearly.RepeatOnDayOfMonth = byMonthDay;

            if (app.Recurrence != null)
            {
                app = app.Copy();

                evData.UidApps.Add(app);
            }

            model.Appointments.Add(app);

            int day = DateTime.DaysInMonth(app.StartTime.Year, byMonth);

            if (day < byMonthDay)
                byMonthDay = day;

            DateTime startTime = new DateTime(app.StartTime.Year,
                byMonth, byMonthDay, app.StartTime.Hour, app.StartTime.Minute, 0);

            if (startTime < app.StartTime)
                recur.SkippedRecurrences.Add(startTime);

            if (app.StartTime != startTime)
                app.MoveTo(startTime);

            ProcessRecurrenceRange(app, recur, evData);

            app.Recurrence = recur;
        }

        #endregion

        #endregion

        #region ProcessVEventReminder

        /// <summary>
        /// Processes event reminders
        /// </summary>
        /// <param name="evData"></param>
        private void ProcessVEventReminder(VEventData evData)
        {
            if (evData.VAlarms != null)
            {
                foreach (VAlarm valarm in evData.VAlarms)
                {
                    foreach (Appointment app in evData.UidApps)
                    {
                        if ((valarm.Rel == Related.Start && valarm.Duration.TotalSeconds == 0) ||
                            (valarm.Rel == Related.Date && valarm.Date.Add(valarm.Duration) == app.StartTime))
                        {
                            app.StartTimeAction = eStartTimeAction.StartTimeReachedEvent;
                        }
                        else
                        {
                            string s = String.IsNullOrEmpty(valarm.Description)
                                           ? app.Description
                                           : valarm.Description;

                            DateTime date = app.StartTime;

                            switch (valarm.Rel)
                            {
                                case Related.End:
                                    date = app.EndTime;
                                    break;

                                case Related.Date:
                                    date = valarm.Date;
                                    break;
                            }

                            date = date.Add(valarm.Duration);

                            app.Reminders.Add(new Reminder(s, date));
                        }
                    }
                }
            }
        }

        #endregion

        #region ProcessRecurrenceId

        /// <summary>
        /// Processes RecurrenceId events
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessRecurrenceId(CalendarEntry entry, Appointment app, VEventData evData)
        {
            if (evData.UidApps != null)
            {
                Appointment uidApp = null;

                foreach (Appointment uApp in evData.UidApps)
                {
                    if (uApp.Recurrence != null)
                    {
                        if (uidApp == null)
                            uidApp = uApp;

                        uApp.Recurrence.SkippedRecurrences.Add(evData.RecIdDate);
                    }
                }

                if (uidApp != null)
                {
                    if (app.StartTime == DateTime.MinValue)
                        app.StartTime = evData.RecIdDate;

                    if (evData.Duration.TotalSeconds != 0)
                        app.EndTime = app.StartTime.Add(evData.Duration);

                    else if (app.EndTime == DateTime.MinValue)
                        app.EndTime = app.StartTime.Add(uidApp.EndTime - uidApp.StartTime);

                    if (app.StartTime > app.EndTime)
                    {
                        ReportError(entry, "DTSTART is greater than DTEND");
                        app.EndTime = app.StartTime.Add(uidApp.EndTime - uidApp.StartTime);
                    }

                    if ((evData.AppPropSet & AppProp.Description) == 0)
                        app.Description = uidApp.Description;

                    if ((evData.AppPropSet & AppProp.Summary) == 0)
                        app.Subject = uidApp.Subject;

                    if ((evData.AppPropSet & AppProp.XDnbCategoryColor) == 0)
                        app.CategoryColor = uidApp.CategoryColor;

                    if ((evData.AppPropSet & AppProp.XDnbDisplayTemplate) == 0)
                        app.DisplayTemplate = uidApp.DisplayTemplate;

                    if ((evData.AppPropSet & AppProp.XDnbImageAlign) == 0)
                        app.ImageAlign = uidApp.ImageAlign;

                    if ((evData.AppPropSet & AppProp.XDnbImageKey) == 0)
                        app.ImageKey = uidApp.ImageKey;

                    if ((evData.AppPropSet & AppProp.XDnbLocked) == 0)
                        app.Locked = uidApp.Locked;

                    if ((evData.AppPropSet & AppProp.XDnbStartTimeAction) == 0)
                        app.StartTimeAction = uidApp.StartTimeAction;

                    if ((evData.AppPropSet & AppProp.XDnbTimeMarkedAs) == 0)
                        app.TimeMarkedAs = uidApp.TimeMarkedAs;

                    if ((evData.AppPropSet & AppProp.XDnbTooltip) == 0)
                        app.Tooltip = uidApp.Tooltip;

                    Model.Appointments.Add(app);
                }
            }
        }

        #endregion

        #region ProcessRecurrenceRange

        /// <summary>
        /// Processes Recurrence range values
        /// </summary>
        /// <param name="app"></param>
        /// <param name="recur"></param>
        /// <param name="evData"></param>
        private void ProcessRecurrenceRange(
            Appointment app, AppointmentRecurrence recur, VEventData evData)
        {
            recur.RecurrenceStartDate =
                (evData.RecStartDate != DateTime.MinValue) ? evData.RecStartDate : app.StartTime;

            if (evData.RecurRule.Count > 0)
            {
                recur.RangeLimitType = eRecurrenceRangeLimitType.RangeNumberOfOccurrences;
                recur.RangeNumberOfOccurrences = evData.RecurRule.Count - 1;
            }

            if (evData.RecurRule.Until != DateTime.MinValue)
            {
                recur.RangeLimitType = eRecurrenceRangeLimitType.RangeEndDate;
                recur.RangeEndDate = evData.RecurRule.Until;
            }

            if (evData.ExDates != null)
            {
                foreach (DateTime date in evData.ExDates)
                {
                    if (date >= app.StartTime)
                    {
                        if (IsSkippableDate(app, date, evData) == true)
                            recur.SkippedRecurrences.Add(date);
                    }
                }
            }
        }

        #region IsSkippableDate

        #region IsSkippableDate

        /// <summary>
        /// Determines if the given date is a valid
        /// SkippedRecurrence date.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="date"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private bool IsSkippableDate(Appointment app, DateTime date, VEventData evData)
        {
            if (IsSkippableByMonth(app, date, evData) == false)
                return (false);

            if (IsSkippableByMonthDay(app, date, evData) == false)
                return (false);

            return (IsSkippableByDay(app, date, evData));
        }

        #endregion

        #region IsSkippableByMonthDay

        /// <summary>
        /// Determines if the given date is a skippable
        /// byMonthDate
        /// </summary>
        /// <param name="app"></param>
        /// <param name="date"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private bool IsSkippableByMonthDay(Appointment app, DateTime date, VEventData evData)
        {
            if (evData.RecurRule.ByMonthDay != null)
            {
                if (date.Day != app.StartTime.Day)
                    return (false);
            }

            return (true);
        }

        #endregion

        #region IsSkippableByMonth

        /// <summary>
        /// Determines if the given date is a skippable ByMonth date
        /// </summary>
        /// <param name="app"></param>
        /// <param name="date"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private bool IsSkippableByMonth(Appointment app, DateTime date, VEventData evData)
        {
            if (evData.RecurRule.ByMonth != null)
            {
                if (date.Month != app.StartTime.Month)
                    return (false);
            }

            return (true);
        }

        #endregion

        #region IsSkippableByDay

        /// <summary>
        /// Determines if the given date is a skippable ByDays date
        /// </summary>
        /// <param name="app"></param>
        /// <param name="date"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private bool IsSkippableByDay(Appointment app, DateTime date, VEventData evData)
        {
            if (evData.RecurRule.ByDays != eDayOfWeekRecurrence.None)
                return (date.DayOfWeek == app.StartTime.DayOfWeek);

            return (true);
        }

        #endregion

        #endregion

        #endregion

        #region ProcessRDateRange

        /// <summary>
        /// ProcessRDateRange
        /// </summary>
        /// <param name="model"></param>
        /// <param name="app"></param>
        /// <param name="evData"></param>
        private void ProcessRDateRange(
            CalendarModel model, Appointment app, VEventData evData)
        {
            if (evData.RecurDate != null)
            {
                if (evData.RecurDate.DtRange != null)
                {
                    foreach (DateRange range in evData.RecurDate.DtRange)
                    {
                        Appointment appRange = app.Copy();

                        appRange.StartTime = range.StartTime;
                        appRange.EndTime = range.EndTime;

                        model.Appointments.Add(appRange);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region ProcessDate

        #region ProcessDate

        /// <summary>
        /// Processes Attribute date values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="isDateValue"></param>
        /// <returns></returns>
        private DateTime ProcessDate(CalendarEntry entry, ref bool isDateValue)
        {
            isDateValue = false;

            if (entry.Attributes.Count > 0)
            {
                foreach (AttributeData attr in entry.Attributes)
                {
                    ParameterToken tkn = (ParameterToken)GetToken(_parameters, attr.Id);

                    switch (tkn)
                    {
                        case ParameterToken.TzId:
                            return (ProcessTzIdDateTime(attr.Value, entry, entry.Value));

                        case ParameterToken.Value:
                            switch ((ValueToken)GetToken(_values, attr.Value))
                            {
                                case ValueToken.Date:
                                    isDateValue = true;

                                    return (ProcessDateValue(entry, entry.Value));

                                case ValueToken.DateTime:
                                    return (ProcessDateTime(entry, entry.Value));

                                default:
                                    ReportError(entry,
                                        "Unrecognized Date attribute Value (" + attr.Id + "=" + attr.Value + ")");

                                    return (DateTime.MinValue);
                            }
                    }
                }
            }

            if (entry.Value.Length > 8)
                return (ProcessDateTime(entry, entry.Value));

            isDateValue = true;

            return (ProcessDateValue(entry, entry.Value));
        }

        #endregion

        #region ProcessDateTime

        /// <summary>
        /// Processes attribute DateTime values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sdate"></param>
        /// <returns></returns>
        private DateTime ProcessDateTime(CalendarEntry entry, string sdate)
        {
            sdate = sdate.ToLower();

            DateTime date;

            if (sdate.EndsWith("z"))
            {
                if (DateTime.TryParseExact(sdate, "yyyyMMdd\\tHHmms\\z",
                                           CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                {
                    ReportError(entry, "Invalid DateTime value specified");
                }
                else
                {
                    date = date.ToLocalTime();
                }
            }
            else
            {
                if (DateTime.TryParseExact(sdate, "yyyyMMdd\\tHHmms",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                {
                    ReportError(entry, "Invalid DateTime value specified");
                }
            }

            return (date);
        }

        #endregion

        #region ProcessDateValue

        /// <summary>
        /// Processes Value=Date entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sDate"></param>
        /// <returns></returns>
        private DateTime ProcessDateValue(CalendarEntry entry, string sDate)
        {
            DateTime date;

            if (DateTime.TryParseExact(sDate, "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
            {
                ReportError(entry, "Invalid Date Value specified");
            }

            return (date);
        }

        #endregion

        #region ProcessTzIdDateTime

        #region ProcessTzIdDateTime

        /// <summary>
        /// Processes TxId attribute DateTime values
        /// </summary>
        /// <param name="tzId"></param>
        /// <param name="entry"></param>
        /// <param name="sdate"></param>
        /// <returns></returns>
        private DateTime ProcessTzIdDateTime(string tzId, CalendarEntry entry, string sdate)
        {
            sdate = sdate.ToLower();
            
            DateTime date = ProcessDateTime(entry, sdate);

            if (sdate.EndsWith("z"))
                return (date);

            date = date.ToLocalTime();

            TimeSpan ts = GetTzIdDelta(tzId, date);

            return (date.Add(ts));
        }

        #endregion

        #region GetTzIdDelta

        /// <summary>
        /// GetTzIdDelta
        /// </summary>
        /// <param name="tzId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private TimeSpan GetTzIdDelta(string tzId, DateTime date)
        {
            CalendarTimeZone zone = GetTimeZone(tzId);

            if (zone != null && zone.Part != null)
            {
                TimeZonePart tzPart = null;
                TimeSpan tzSpan = TimeSpan.MaxValue;

                foreach (TimeZonePart part in zone.Part)
                {
                    if (part.RecurRule != null)
                    {
                        DateTime pdate = GetLastPartDate(part, date);

                        TimeSpan span = date - pdate;

                        if (span >= TimeSpan.Zero && span < tzSpan)
                        {
                            tzSpan = span;
                            tzPart = part;
                        }
                    }
                }

                if (tzPart != null)
                    return (new TimeSpan(0, -tzPart.OffsetTo, 0));
            }

            return (TimeSpan.Zero);
        }

        #endregion

        #region GetLastPartDate

        /// <summary>
        /// GetLastPartDate
        /// </summary>
        /// <param name="part"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime GetLastPartDate(TimeZonePart part, DateTime date)
        {
            DateTime pdate = part.StartDate;

            if (part.Appc == null)
            {
                CalendarModel model = new CalendarModel();
                Appointment app = new Appointment();

                app.StartTime = part.StartDate.Date;
                app.EndTime = app.StartTime;

                VEventData evData = new VEventData();

                evData.RecurRule = part.RecurRule;

                ProcessEventEndEx(model, null, app, evData);

                part.Appc = new AppointmentSubsetCollection(model, part.StartDate, date);
            }

            if (part.Appc.Count > 0)
            {
                foreach (Appointment app in part.Appc)
                {
                    if (app.StartTime > pdate && app.StartTime <= date)
                        pdate = app.StartTime;
                }
            }

            return (pdate);
        }

        #endregion

        #region GetTimeZone

        /// <summary>
        /// GetTimeZone
        /// </summary>
        /// <param name="tzId"></param>
        /// <returns></returns>
        private CalendarTimeZone GetTimeZone(string tzId)
        {
            if (_CalendarTimeZone != null)
            {
                foreach (CalendarTimeZone zone in _CalendarTimeZone)
                {
                    if (zone.TzId.Equals(tzId) == true)
                        return (zone);
                }
            }

            return (null);
        }

        #endregion

        #endregion

        #endregion

        #region ProcessDescription

        /// <summary>
        /// Processes event Description values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private string ProcessDescription(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.Description;

            return (entry.Value);
        }

        #endregion

        #region ProcessDuration

        /// <summary>
        /// Processes event Duration values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private TimeSpan ProcessDuration(CalendarEntry entry, string value)
        {
            const string s = "^(?<sign>[+-]+)*";
            const string d = "P((?<days>\\d+)D)*";
            const string t = "(T((?<hours>\\d+)H)*((?<minutes>\\d+)M)*((?<seconds>\\d+)S)*)*";
            const string w = "((?<weeks>\\d+)W)*";

            Regex p = new Regex(s + d + t + w);
            Match ma = p.Match(value);

            if (ma.Success == true)
            {
                int sign = ma.Groups["sign"].Value.Equals("-") ? -1 : 1;

                int days = GetGroupIntValue(ma, "days", sign);
                int hours = GetGroupIntValue(ma, "hours", sign);
                int minutes = GetGroupIntValue(ma, "minutes", sign);
                int seconds = GetGroupIntValue(ma, "seconds", sign);
                int weeks = GetGroupIntValue(ma, "weeks", sign);

                return (new TimeSpan(weeks * 7 + days, hours, minutes, seconds));
            }

            ReportError(entry, "Invalid DURATION specified");

            return (TimeSpan.Zero);
        }

        #region GetGroupIntValue

        /// <summary>
        /// Gets the int value from the given Regex Match
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="group"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private int GetGroupIntValue(Match ma, string group, int sign)
        {
            string s = ma.Groups[group].Value;

            return (string.IsNullOrEmpty(s) ? 0 : int.Parse(s) * sign);
        }

        #endregion

        #endregion

        #region ProcessExDate

        /// <summary>
        /// Processes event ExDate values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        private void ProcessExDate(CalendarEntry entry, VEventData evData)
        {
            List<DateTime> exDates = null;

            if (entry.Attributes.Count > 0)
            {
                foreach (AttributeData attr in entry.Attributes)
                {
                    ParameterToken tkn = (ParameterToken) GetToken(_parameters, attr.Id);

                    switch (tkn)
                    {
                        case ParameterToken.TzId:
                            if (exDates != null)
                                ReportError(entry, "Multiple EXDATE parameter types");

                            exDates = ProcessTzIdExDateTimeValues(attr.Value, entry, entry.Value);
                            break;

                        case ParameterToken.Value:
                            if (exDates != null)
                                ReportError(entry, "Multiple EXDATE parameter types");

                            switch ((ValueToken)GetToken(_values, attr.Value))
                            {
                                case ValueToken.Date:
                                    exDates = ProcessExDateValues(entry);
                                    break;

                                case ValueToken.DateTime:
                                    exDates = ProcessExDateTimeValues(entry, entry.Value);
                                    break;

                                default:
                                    ReportError(entry, "Unrecognized ExDate attribute Value (" +
                                        attr.Id + "=" + attr.Value + ")");

                                    break;
                            } 
                            break;
                    }
                }
            }

            if (exDates == null)
                exDates = ProcessExDateTimeValues(entry, entry.Value);

            UnionExDates(evData, exDates);
        }

        #region ProcessExDateValues

        /// <summary>
        /// Processes ExDate Value=Date values
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private List<DateTime> ProcessExDateValues(CalendarEntry entry)
        {
            string[] dates = entry.Value.Split(',');

            if (dates.Length > 0)
            {
                List<DateTime> exDates = new List<DateTime>(dates.Length);

                for (int i = 0; i < dates.Length; i++)
                    exDates.Add(ProcessDateValue(entry, dates[i]));

                return (exDates);
            }

            return (null);
        }

        #endregion

        #region ProcessExDateTimeValues

        /// <summary>
        /// Processes ExDate Value=DateTime values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<DateTime> ProcessExDateTimeValues(CalendarEntry entry, string value)
        {
            string[] dates = value.Split(',');

            if (dates.Length > 0)
            {
                List<DateTime> exDates = new List<DateTime>(dates.Length);

                for (int i = 0; i < dates.Length; i++)
                    exDates.Add(ProcessDateTime(entry, dates[i]));

                return (exDates);
            }

            return (null);
        }

        #endregion

        #region ProcessTzIdExDateTimeValues

        /// <summary>
        /// Processes ExDate Value=DateTime values
        /// </summary>
        /// <param name="tzId"></param>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<DateTime> ProcessTzIdExDateTimeValues(string tzId, CalendarEntry entry, string value)
        {
            string[] dates = value.Split(',');

            if (dates.Length > 0)
            {
                List<DateTime> exDates = new List<DateTime>(dates.Length);

                for (int i = 0; i < dates.Length; i++)
                    exDates.Add(ProcessTzIdDateTime(tzId, entry, dates[i]));

                return (exDates);
            }

            return (null);
        }

        #endregion

        #region UnionExDates

        /// <summary>
        /// Combines the main evData exDates list with a secondary
        /// accumulated exDates list
        /// </summary>
        /// <param name="evData"></param>
        /// <param name="exDates"></param>
        private void UnionExDates(VEventData evData, List<DateTime> exDates)
        {
            if (evData.ExDates == null)
            {
                evData.ExDates = exDates;
            }
            else if (exDates != null)
            {
                foreach (DateTime date in exDates)
                {
                    if (evData.ExDates.Contains(date) == false)
                        evData.ExDates.Add(date);
                }
            }
        }

        #endregion

        #endregion

        #region ProcessRDate

        #region ProcessRDate

        /// <summary>
        /// Processes RDate attributes and values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="rdateOld"></param>
        /// <returns></returns>
        private RDate ProcessRDate(CalendarEntry entry, RDate rdateOld)
        {
            RDate rdateNew = null;

            if (entry.Attributes.Count > 0)
            {
                foreach (AttributeData attr in entry.Attributes)
                {
                    ParameterToken tkn = (ParameterToken)GetToken(_parameters, attr.Id);

                    switch (tkn)
                    {
                        case ParameterToken.TzId:
                            if (rdateNew != null)
                                ReportError(entry, "Multiple RDATE parameter types");

                            rdateNew = ProcessTzIdRDateTimeValues(attr.Value, entry, entry.Value);
                            break;

                        case ParameterToken.Value:
                            if (rdateNew != null)
                                ReportError(entry, "Multiple RDATE parameter types");

                            switch ((ValueToken)GetToken(_values, attr.Value))
                            {
                                case ValueToken.Date:
                                    rdateNew = ProcessRDateValues(entry);
                                    break;

                                case ValueToken.DateTime:
                                    rdateNew = ProcessRDateTimeValues(entry, entry.Value);
                                    break;

                                case ValueToken.Period:
                                    rdateNew = ProcessRDatePeriod(entry);
                                    break;

                                default:
                                    ReportError(entry, "Unrecognized RDate attribute Value (" +
                                        attr.Id + "=" + attr.Value + ")");

                                    break;
                            }
                            break;
                    }
                }
            }

            if (rdateNew == null)
                rdateNew = ProcessRDateTimeValues(entry, entry.Value);

            return (UnionRDates(rdateOld, rdateNew));
        }

        #endregion

        #region ProcessRDateValues

        /// <summary>
        /// Processes RDate Value=Date values
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private RDate ProcessRDateValues(CalendarEntry entry)
        {
            string[] dates = entry.Value.Split(',');

            if (dates.Length > 0)
            {
                RDate rdate = new RDate();

                rdate.DtRange = new DateRange[dates.Length];

                for (int i = 0; i < dates.Length; i++)
                    rdate.DtRange[i].StartTime = ProcessDateValue(entry, dates[i]);

                return (rdate);
            }

            return (null);
        }

        #endregion

        #region ProcessRDateTimeValues

        /// <summary>
        /// Processes RDate Value=DateTime values
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private RDate ProcessRDateTimeValues(CalendarEntry entry, string value)
        {
            string[] dates = value.Split(',');

            if (dates.Length > 0)
            {
                RDate rdate = new RDate();

                rdate.DtRange = new DateRange[dates.Length];

                for (int i = 0; i < dates.Length; i++)
                    rdate.DtRange[i].StartTime = ProcessDateTime(entry, dates[i]);

                return (rdate);
            }

            return (null);
        }

        #endregion

        #region ProcessTzIdRDateTimeValues

        /// <summary>
        /// Processes RDate Value=DateTime values
        /// </summary>
        /// <param name="tzId"></param>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private RDate ProcessTzIdRDateTimeValues(string tzId, CalendarEntry entry, string value)
        {
            string[] dates = value.Split(',');

            if (dates.Length > 0)
            {
                RDate rdate = new RDate();

                rdate.DtRange = new DateRange[dates.Length];

                for (int i = 0; i < dates.Length; i++)
                    rdate.DtRange[i].StartTime = ProcessTzIdDateTime(tzId, entry, dates[i]);

                return (rdate);
            }

            return (null);
        }

        #endregion

        #region ProcessRDatePeriod

        /// <summary>
        /// Processes RDate Value=Period values
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private RDate ProcessRDatePeriod(CalendarEntry entry)
        {
            string[] dates = entry.Value.Split(',');

            if (dates.Length > 0)
            {
                RDate rdate = new RDate();

                rdate.DtRange = new DateRange[dates.Length];

                for (int i = 0; i < dates.Length; i++)
                {
                    string[] parts = dates[i].Split('/');

                    if (parts.Length != 2)
                    {
                        ReportError(entry, "RDATE PERIOD not formed correctly");
                        break;
                    }

                    rdate.DtRange[i] = new DateRange();
                    rdate.DtRange[i].StartTime = ProcessDateTime(entry, parts[0]);

                    parts[1] = parts[1].TrimStart(' ');

                    if (parts[1].StartsWith("P") == true)
                    {
                        TimeSpan duration = ProcessDuration(entry, parts[1]);

                        rdate.DtRange[i].EndTime =
                            rdate.DtRange[i].StartTime.Add(duration);
                    }
                    else
                    {
                        rdate.DtRange[i].EndTime = ProcessDateTime(entry, parts[1]);
                    }
                }

                return (rdate);
            }

            return (null);
        }

        #endregion

        #region UnionRDates

        /// <summary>
        /// Combines RDate values
        /// </summary>
        /// <param name="rdateOld"></param>
        /// <param name="rdateNew"></param>
        /// <returns></returns>
        private RDate UnionRDates(RDate rdateOld, RDate rdateNew)
        {
            if (rdateNew != null)
            {
                if (rdateOld != null)
                {
                    DateRange[] range = new
                        DateRange[rdateOld.DtRange.Length + rdateNew.DtRange.Length];

                    Array.Copy(rdateOld.DtRange, range, rdateOld.DtRange.Length);
                    Array.Copy(rdateNew.DtRange, 0, range, rdateOld.DtRange.Length, rdateNew.DtRange.Length);

                    rdateNew.DtRange = range;
                }

                return (rdateNew);
            }

            return (rdateOld);
        }

        #endregion

        #endregion

        #region ProcessRecurrenceId

        /// <summary>
        /// Processes RecurrenceId entries
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private DateTime ProcessRecurrenceId(CalendarEntry entry)
        {
            bool isDateValue = true;

            return (ProcessDate(entry, ref isDateValue));
        }

        #endregion

        #region ProcessRRule

        #region ProcessRRule

        /// <summary>
        /// Processes RRule entries
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private RRule ProcessRRule(CalendarEntry entry)
        {
            RRule rrule = new RRule();
            List<AttributeData> attributes = new List<AttributeData>();

            string s = ";" + entry.Value;

            ParseAttributeData(attributes, ref s);

            foreach (AttributeData attr in attributes)
            {
                ValueToken tkn = (ValueToken)GetToken(_values, attr.Id);

                switch (tkn)
                {
                    case ValueToken.ByDay:
                        ProcessByDay(entry, rrule, attr.Value);
                        break;

                    case ValueToken.ByMonth:
                        rrule.ByMonth = ProcessByIntValues(entry, attr.Value, 1, 12, tkn);
                        break;

                    case ValueToken.ByMonthDay:
                        rrule.ByMonthDay = ProcessByIntValues(entry, attr.Value, 1, 31, tkn);
                        break;

                    case ValueToken.Count:
                        ProcessCount(entry, rrule, attr);
                        break;

                    case ValueToken.Freq:
                        ProcessFreq(entry, rrule, attr);
                        break;

                    case ValueToken.Interval:
                        ProcessInterval(entry, rrule, attr);
                        break;

                    case ValueToken.Until:
                        ProcessUntil(entry, rrule, attr);
                        break;

                    case ValueToken.BySecond:
                    case ValueToken.ByMinute:
                    case ValueToken.ByHour:
                    case ValueToken.ByYearDay:
                    case ValueToken.ByWeekNo:
                    case ValueToken.BySetPos:
                    case ValueToken.WkSt:
                        break;

                    default:
                        ReportError(entry, "Unrecognized RRULE Value - " + attr.Id);
                        break;
                }
            }

            return (rrule);
        }

        #endregion

        #region ProcessByDay

        /// <summary>
        /// Processes ByDay= attribute entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="rrule"></param>
        /// <param name="value"></param>
        private void ProcessByDay(CalendarEntry entry, RRule rrule, string value)
        {
            rrule.ByDays = eDayOfWeekRecurrence.None;

            Regex p = new Regex("(?<dayNum>[+-]{0,1}\\d+)*(?<weekDay>\\w+)");
            MatchCollection mc = p.Matches(value);

            if (mc.Count > 0)
            {
                rrule.ByDay = new List<int>[7];

                foreach (Match ma in mc)
                {
                    int dayOfWeek;
                    eDayOfWeekRecurrence weekDay = ProcessWeekDay(entry, ma.Groups["weekDay"].Value, out dayOfWeek);

                    rrule.ByDays |= weekDay;

                    string s = ma.Groups["dayNum"].Value;

                    int n = 0;

                    if (String.IsNullOrEmpty(s) == false)
                    {
                        n = int.Parse(s);

                        if (Math.Abs(n) > 53)
                        {
                            ReportError(entry,
                                String.Format("BYDAY value ({0}) is not in the range +/- 1 to 53", n));

                            n = 0;
                        }
                    }

                    if (rrule.ByDay[dayOfWeek] == null)
                        rrule.ByDay[dayOfWeek] = new List<int>();

                    rrule.ByDay[dayOfWeek].Add(n);
                }
            }
        }

        #region ProcessWeekDay

        /// <summary>
        /// Processes individual ByDay.WeekDay entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="s"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private eDayOfWeekRecurrence ProcessWeekDay(
            CalendarEntry entry, string s, out int dayOfWeek)
        {
            ValueToken tkn = (ValueToken)GetToken(_values, s);

            switch (tkn)
            {
                case ValueToken.Mo:
                    dayOfWeek = 0;
                    return (eDayOfWeekRecurrence.Monday);

                case ValueToken.Tu:
                    dayOfWeek = 1;
                    return (eDayOfWeekRecurrence.Tuesday);

                case ValueToken.We:
                    dayOfWeek = 2;
                    return (eDayOfWeekRecurrence.Wednesday);

                case ValueToken.Th:
                    dayOfWeek = 3;
                    return (eDayOfWeekRecurrence.Thursday);

                case ValueToken.Fr:
                    dayOfWeek = 4;
                    return (eDayOfWeekRecurrence.Friday);

                case ValueToken.Sa:
                    dayOfWeek = 5;
                    return (eDayOfWeekRecurrence.Saturday);

                case ValueToken.Su:
                    dayOfWeek = 6;
                    return (eDayOfWeekRecurrence.Sunday);

                default:
                    dayOfWeek = 0;
                    ReportError(entry, "Unrecognized WeekDay - " + s);
                    return (eDayOfWeekRecurrence.None);
            }
        }

        #endregion

        #endregion

        #region ProcessByIntValues

        /// <summary>
        /// Processes ByMonth= and ByMonthDay= attribute entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="tkn"></param>
        /// <returns></returns>
        private int[] ProcessByIntValues(CalendarEntry entry,
            string value, int minValue, int maxValue, ValueToken tkn)
        {
            string[] byValues = value.Split(',');

            if (byValues.Length > 0)
            {
                int[] byIntValues = new int[byValues.Length];

                for (int i = 0; i < byValues.Length; i++)
                {
                    int n;

                    if (int.TryParse(byValues[i], out n) == false)
                    {
                        ReportError(entry, String.Format("\"{0}\" is not a valid Integer value", byValues[i]));

                        n = minValue;
                    }
                    else
                    {
                        int absn = Math.Abs(n);

                        if (absn < minValue || absn > maxValue)
                        {
                            ReportError(entry, String.Format(
                                "{0} value ({1}) is not in the range +/- {2} to {3}",
                                n, _values[(int) tkn].Text.ToUpper(), minValue, maxValue));

                            break;
                        }
                    }

                    byIntValues[i] = n;
                }

                return (byIntValues);
            }

            return (null);
        }

        #endregion

        #region ProcessCount

        /// <summary>
        /// Processes RRule Count attribute entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="rrule"></param>
        /// <param name="attr"></param>
        private void ProcessCount(CalendarEntry entry, RRule rrule, AttributeData attr)
        {
            int n;

            if (int.TryParse(attr.Value, out n) == false)
            {
                n = 1;
                ReportError(entry, String.Format("\"{0}\" is not a valid COUNT value", attr.Value));
            }

            rrule.Count = n;
        }

        #endregion

        #region ProcessFreq

        /// <summary>
        /// Processes Freq= attribute entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="rrule"></param>
        /// <param name="attr"></param>
        private void ProcessFreq(CalendarEntry entry, RRule rrule, AttributeData attr)
        {
            ValueToken tkn = (ValueToken)GetToken(_values, attr.Value);

            switch (tkn)
            {
                case ValueToken.Secondly:
                    rrule.Freq = Frequency.Secondly;
                    break;

                case ValueToken.Minutely:
                    rrule.Freq = Frequency.Minutely;
                    break;

                case ValueToken.Hourly:
                    rrule.Freq = Frequency.Hourly;
                    break;

                case ValueToken.Daily:
                    rrule.Freq = Frequency.Daily;
                    break;

                case ValueToken.Weekly:
                    rrule.Freq = Frequency.Weekly;
                    break;

                case ValueToken.Monthly:
                    rrule.Freq = Frequency.Monthly;
                    break;

                case ValueToken.Yearly:
                    rrule.Freq = Frequency.Yearly;
                    break;

                default:
                    ReportError(entry, "Invalid FREQ value");
                    rrule.Freq = Frequency.Daily;
                    break;
            }
        }

        #endregion

        #region ProcessInterval

        /// <summary>
        /// Processes Interval= attribute entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="rrule"></param>
        /// <param name="attr"></param>
        private void ProcessInterval(CalendarEntry entry, RRule rrule, AttributeData attr)
        {
            int n;

            if (int.TryParse(attr.Value, out n) == false)
            {
                n = 1;
                ReportError(entry, String.Format("\"{0}\" is not a valid INTERVAL value", attr.Value));
            }

            rrule.Interval = n;
        }

        #endregion

        #region ProcessUntil

        /// <summary>
        /// Processes Until= attribute entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="rrule"></param>
        /// <param name="attr"></param>
        private void ProcessUntil(CalendarEntry entry, RRule rrule, AttributeData attr)
        {
            rrule.Until = (attr.Value.Length > 8)
                              ? ProcessDateTime(entry, attr.Value)
                              : ProcessDateValue(entry, attr.Value);
        }

        #endregion

        #endregion

        #region ProcessSummary

        /// <summary>
        /// Processes Summary entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private string ProcessSummary(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.Summary;

            return (entry.Value);
        }

        #endregion

        #region ProcessUid

        /// <summary>
        /// Processes UID entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        private List<Appointment> ProcessUid(CalendarEntry entry, Appointment app)
        {
            List<Appointment> uidApp;

            if (_Uid.TryGetValue(entry.Value, out uidApp) == false)
            {
                uidApp = new List<Appointment>();
                uidApp.Add(app);

                _Uid.Add(entry.Value, uidApp);
            }

            return (uidApp);
        }

        #endregion

        #region ProcessVAlarm

        /// <summary>
        /// Processes VAlarm component entries
        /// </summary>
        /// <param name="evData"></param>
        private void ProcessVAlarm(VEventData evData)
        {
            VAlarm valarm = new VAlarm();

            CalendarEntry entry = GetNextEntry();

            while (entry != null)
            {
                PropertyToken tkn = (PropertyToken)GetToken(_properties, entry.Id);

                switch (tkn)
                {
                    case PropertyToken.Description:
                        valarm.Description = entry.Value;
                        break;

                    case PropertyToken.End:
                        if ((ComponentToken)GetToken(_components, entry.Value) != ComponentToken.VAlarm)
                            ReportError(entry, "Expected \"END:VALARM\"");

                        if (evData.VAlarms == null)
                            evData.VAlarms = new List<VAlarm>();

                        evData.VAlarms.Add(valarm);
                        return;

                    case PropertyToken.Trigger:
                        ProcessTrigger(entry, valarm);
                        break;
                }

                entry = GetNextEntry();
            }

            ReportError(entry, "Expected \"END:VALARM\"");
        }

        #region ProcessTrigger

        /// <summary>
        /// Processes VAlarm Trigger properties
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="valarm"></param>
        private void ProcessTrigger(CalendarEntry entry, VAlarm valarm)
        {
            if (entry.Attributes.Count > 0)
            {
                foreach (AttributeData attr in entry.Attributes)
                {
                    ParameterToken tkn = (ParameterToken)GetToken(_parameters, attr.Id);

                    switch (tkn)
                    {
                        case ParameterToken.Related:
                            valarm.Rel = ProcessRelated(entry, attr.Value);
                            break;

                        case ParameterToken.Value:
                            switch ((ValueToken)GetToken(_values, attr.Value))
                            {
                                case ValueToken.Date:
                                    valarm.Rel = Related.Date;
                                    valarm.Date = ProcessDateValue(entry, entry.Value);
                                    break;

                                case ValueToken.DateTime:
                                    valarm.Rel = Related.Date;
                                    valarm.Date = ProcessDateTime(entry, entry.Value);
                                    break;

                                case ValueToken.Duration:
                                    valarm.Duration = ProcessDuration(entry, entry.Value);
                                    break;
                            }
                            break;
                    }
                }
            }
            else
            {
                valarm.Duration = ProcessDuration(entry, entry.Value);
            }
        }

        #region ProcessRelated

        private Related ProcessRelated(CalendarEntry entry, string value)
        {
            ValueToken rtkn = (ValueToken)GetToken(_values, value);

            switch (rtkn)
            {
                case ValueToken.Start:
                    return (Related.Start);

                case ValueToken.End:
                    return (Related.End);

                default:
                    ReportError(entry, "Invalid RELATED value: " + value.ToUpper());
                    return (Related.Date);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Process DNB specific

        #region ProcessXDnbCategoryColor

        /// <summary>
        /// Processes DNB CategoryColor entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private string ProcessXDnbCategoryColor(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbCategoryColor;

            return(entry.Value);
        }

        #endregion

        #region ProcessXDnbDisplayTemplate

        /// <summary>
        /// Processes DNB DisplayTemplate entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private string ProcessXDnbDisplayTemplate(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbDisplayTemplate;

            return (entry.Value);
        }

        #endregion

        #region ProcessXDnbImageAlign

        /// <summary>
        /// Processes DNB specific ImageAlign entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private eImageContentAlignment ProcessXDnbImageAlign(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbImageAlign;

            return ((eImageContentAlignment)
                GetEnumValue(typeof(eImageContentAlignment), entry.Value));
        }

        #endregion

        #region ProcessXDnbImageKey

        /// <summary>
        /// Processes DNB ImageKey entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private string ProcessXDnbImageKey(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbImageKey;

            return (entry.Value);
        }

        #endregion

        #region ProcessXDnbLocked

        /// <summary>
        /// Processes DNB Locked entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private bool ProcessXDnbLocked(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbLocked;

            return (entry.Value.ToLower().Equals("true"));
        }

        #endregion

        #region ProcessXDnbRecStartDate

        /// <summary>
        /// Processes DNB RecStartDate entries
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private DateTime ProcessXDnbRecStartDate(CalendarEntry entry)
        {
            return (ProcessDateTime(entry, entry.Value));
        }

        #endregion

        #region ProcessXDnbStartTimeAction

        /// <summary>
        /// Processes DNB StartTimeAction entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private eStartTimeAction ProcessXDnbStartTimeAction(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbStartTimeAction;

            return ((eStartTimeAction)
                GetEnumValue(typeof(eStartTimeAction), entry.Value));
        }

        #endregion

        #region ProcessXDnbTimeMarkedAs

        /// <summary>
        /// Processes DNB TimeMarkedAs entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private string ProcessXDnbTimeMarkedAs(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbTimeMarkedAs;

            return (entry.Value);
        }

        #endregion

        #region ProcessXDnbTooltip

        /// <summary>
        /// Processes DNB ToolTip entries
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evData"></param>
        /// <returns></returns>
        private string ProcessXDnbTooltip(CalendarEntry entry, VEventData evData)
        {
            evData.AppPropSet |= AppProp.XDnbTooltip;

            return (entry.Value);
        }

        #endregion

        #endregion

        #endregion

        #region ProcessVTimezone

        #region ProcessVTimezone

        /// <summary>
        /// Processes VTimeZone components
        /// </summary>
        private void ProcessVTimezone()
        {
            CalendarTimeZone zone = new CalendarTimeZone();

            CalendarEntry entry = GetNextEntry();

            while (entry != null)
            {
                PropertyToken tkn = (PropertyToken)GetToken(_properties, entry.Id);

                switch (tkn)
                {
                     case PropertyToken.Begin:
                        ComponentToken ctkn = (ComponentToken) GetToken(_components, entry.Value);

                        switch (ctkn)
                        {
                            case ComponentToken.Daylight:
                            case ComponentToken.Standard:
                                ProcessZonePart(zone, ctkn);
                                break;
                        }
                        break;
                
                    case PropertyToken.End:
                        ProcessVTimezoneEnd(entry, zone);
                        return;

                    case PropertyToken.TzId:
                        zone.TzId = ProcessTzId(entry);
                        break;
                }

                entry = GetNextEntry();
            }

            ReportError(entry, "Expected \"END:VTIMEZONE\"");
        }

        #endregion

        #region ProcessTzId

        /// <summary>
        /// Processes TxId values
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private string ProcessTzId(CalendarEntry entry)
        {
            return (entry.Value);
        }

        #endregion

        #region ProcessTzOffset

        /// <summary>
        /// Processes TzOffset values
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private int ProcessTzOffset(CalendarEntry entry)
        {
            const string s = "^(?<sign>[+-]+)*";
            const string t = "((?<hours>\\d{2})(?<minutes>\\d{2})*)*";

            Regex p = new Regex(s + t);
            Match ma = p.Match(entry.Value);

            if (ma.Success == true)
            {
                int sign = ma.Groups["sign"].Value.Equals("-") ? -1 : 1;

                int hours = GetGroupIntValue(ma, "hours", sign);
                int minutes = GetGroupIntValue(ma, "minutes", sign);

                return (hours * 60 + minutes);
            }

            ReportError(entry, "Invalid TZOFFSET");

            return (0);
        }

        #endregion

        #region ProcessVTimezoneEnd

        /// <summary>
        /// ProcessVTimezoneEnd
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="zone"></param>
        private void ProcessVTimezoneEnd(CalendarEntry entry, CalendarTimeZone zone)
        {
            if ((ComponentToken)GetToken(_components, entry.Value) != ComponentToken.VTimezone)
                ReportError(entry, "Expected \"END:VTIMEZONE\"");

            if (_CalendarTimeZone == null)
                _CalendarTimeZone = new List<CalendarTimeZone>();

            _CalendarTimeZone.Add(zone);
        }

        #endregion

        #region ProcessZonePart

        /// <summary>
        /// Processes VTimeZone parts (DayLight, Standard)
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="tkn"></param>
        private void ProcessZonePart(CalendarTimeZone zone, ComponentToken tkn)
        {
            bool isDtStartValue = false;

            TimeZonePart part = new TimeZonePart();

            CalendarEntry entry = GetNextEntry();

            while (entry != null)
            {
                switch ((PropertyToken) GetToken(_properties, entry.Id))
                {
                    case PropertyToken.DtStart:
                        part.StartDate = ProcessDate(entry, ref isDtStartValue);
                        break;

                    case PropertyToken.RDate:
                        part.RecurDate = ProcessRDate(entry, part.RecurDate);
                        break;

                    case PropertyToken.RRule:
                        part.RecurRule = ProcessRRule(entry);
                        break;

                    case PropertyToken.TzOffsetTo:
                        part.OffsetTo = ProcessTzOffset(entry);
                        break;

                    case PropertyToken.End:
                        ProcessZonePartEnd(entry, zone, part, tkn);
                        return;
                }

                entry = GetNextEntry();
            }

            ReportError(entry, "Expected \"END:" + _components[(int)tkn].Text.ToUpper() + "\"");
        }

        private void ProcessZonePartEnd(CalendarEntry entry,
            CalendarTimeZone zone, TimeZonePart part, ComponentToken tkn)
        {
            if ((ComponentToken)GetToken(_components, entry.Value) != tkn)
                ReportError(entry, "Expected \"END:" + _components[(int)tkn].Text.ToUpper() + "\"");

            if (zone.Part == null)
                zone.Part = new List<TimeZonePart>();

            zone.Part.Add(part);
        }

        #endregion

        #endregion

        #region ProcessVTodo

        /// <summary>
        /// Processes VToDo entries
        /// </summary>
        /// <param name="ownerKey"></param>
        private void ProcessVTodo(string ownerKey)
        {
            Appointment app = new Appointment();
            app.OwnerKey = ownerKey;

            VEventData evData = new VEventData();

            CalendarEntry entry = GetNextEntry();

            while (entry != null)
            {
                PropertyToken tkn = (PropertyToken)GetToken(_properties, entry.Id);

                switch (tkn)
                {
                    case PropertyToken.Begin:
                        ComponentToken ctkn = (ComponentToken)GetToken(_components, entry.Value);

                        switch (ctkn)
                        {
                            case ComponentToken.VAlarm:
                                ProcessVAlarm(evData);
                                break;

                            default:
                                ReportError(entry, "Unexpected \"BEGIN:" + entry.Value.ToUpper() + "\"");
                                SkipComponent(ctkn);
                                break;
                        }
                        break;

                    case PropertyToken.Description:
                        app.Description = entry.Value;
                        break;

                    case PropertyToken.DtEnd:
                    case PropertyToken.Due:
                        app.EndTime = ProcessDate(entry, ref evData.IsDtEndValue);
                        break;

                    case PropertyToken.DtStart:
                        app.StartTime = ProcessDate(entry, ref evData.IsDtStartValue);
                        break;

                    case PropertyToken.Duration:
                        evData.Duration = ProcessDuration(entry, entry.Value);
                        break;

                    case PropertyToken.End:
                        if ((ComponentToken)GetToken(_components, entry.Value) != ComponentToken.VTodo)
                            ReportError(entry, "Expected \"END:VTODO\"");
                        
                        ProcessVEventEnd(entry, app, evData);
                        return;

                    case PropertyToken.RDate:
                        evData.RecurDate = ProcessRDate(entry, evData.RecurDate);
                        break;

                    case PropertyToken.RRule:
                        evData.RecurRule = ProcessRRule(entry);
                        break;

                    case PropertyToken.Summary:
                        app.Subject = entry.Value;
                        break;
                }

                entry = GetNextEntry();
            }

            ReportError(entry, "Expected \"END:VTODO\"");
        }

        #endregion

        #region ProcessWrCalName

        /// <summary>
        /// Processes X-WR-CALNAME entries
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private string ProcessWrCalName(CalendarEntry entry)
        {
            if (entry.Attributes.Count > 0)
            {
                foreach (AttributeData attr in entry.Attributes)
                {
                    ParameterToken tkn = (ParameterToken)GetToken(_parameters, attr.Id);

                    if (tkn == ParameterToken.Value)
                        return (entry.Value);
                }
            }

            return (null);
        }

        #endregion

        #endregion

        #region SkipComponent

        /// <summary>
        /// Skips the given component block (until it reaches
        /// the corresponding ctkn:END value)
        /// </summary>
        /// <param name="ctkn"></param>
        private void SkipComponent(ComponentToken ctkn)
        {
            int level = 1;

            CalendarEntry entry = GetNextEntry();

            while (entry != null)
            {
                PropertyToken tkn = (PropertyToken) GetToken(_properties, entry.Id);

                switch (tkn)
                {
                    case PropertyToken.Begin:
                        level++;
                        break;

                    case PropertyToken.End:
                        level--;

                        if (level == 0)
                        {
                            if ((ComponentToken) GetToken(_components, entry.Value) != ctkn)
                            {
                                ReportError(entry, "Expected \"END:" +
                                                    Enum.GetName(typeof(ComponentToken), ctkn).ToUpper() + "\"");
                            }
                            return;
                        }
                        break;
                }

                entry = GetNextEntry();
            }

            ReportError(entry, "Expected \"END:" +
                                Enum.GetName(typeof(ComponentToken), ctkn).ToUpper() + "\"");
        }

        #endregion

        #region Token processing

        #region GetToken

        /// <summary>
        /// Gets the token associated with the given text
        /// </summary>
        /// <param name="reg">Token registry</param>
        /// <param name="text">Token text</param>
        /// <returns>Token, or -1 if not found</returns>
        private int GetToken(TokenRegistry[] reg, string text)
        {
            int index = Array.BinarySearch(reg, text.ToLower());

            if (index >= 0)
                return (reg[index].Token);

            return (-1);
        }

        #endregion

        #region GetFirstEntry

        /// <summary>
        /// Gets the first CalendarEntry
        /// </summary>
        /// <returns></returns>
        private CalendarEntry GetFirstEntry()
        {
            _EntryIndex = 0;

            return (GetNextEntry());
        }

        #endregion

        #region GetNextEntry

        /// <summary>
        /// Gets the next CalendarEntry
        /// </summary>
        /// <returns></returns>
        private CalendarEntry GetNextEntry()
        {
            if (_EntryIndex >= _CalendarEntries.Count)
                return (null);

            return (_CalendarEntries[_EntryIndex++]);
        }

        #endregion

        #region GetEnumValue

        /// <summary>
        /// Gets the enum value, given the token type and text
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetEnumValue(Type type, string name)
        {
            return ((int)Enum.Parse(type, name));
        }

        #endregion

        #endregion

        #region Parse support

        #region Parse

        /// <summary>
        /// Initiates the parsing of the given import file
        /// </summary>
        /// <param name="streamReader"></param>
        private void Parse(StreamReader streamReader)
        {
            int lineStart = 1;
            int lineEnd = 1;

            string s = GetNextLine(streamReader, ref lineStart, ref lineEnd);

            while (s != null)
            {
                ParseLine(s, lineStart, lineEnd);

                s = GetNextLine(streamReader, ref lineStart, ref lineEnd);
            }
        }

        #endregion

        #region GetNextLine

        /// <summary>
        /// Gets the next logical line of text
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        private string GetNextLine(
            StreamReader sr, ref int lineStart, ref int lineEnd)
        {
            string s = null;

            if (sr.EndOfStream == false)
            {
                s = sr.ReadLine();

                lineStart = lineEnd;
                lineEnd++;

                while (sr.EndOfStream == false)
                {
                    if (Char.IsWhiteSpace((char)sr.Peek()) == false)
                        break;

                    string t = sr.ReadLine();

                    if (t.Length > 0)
                        s += t.Substring(1);

                    lineEnd++;
                }
            }

            return (s);
        }

        #endregion

        #region ParseLine

        /// <summary>
        /// Initiates the syntactic parsing of the given line
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        private void ParseLine(string s, int lineStart, int lineEnd)
        {
            if (IsBlankLine(s) == false)
            {
                CalendarEntry entry = new CalendarEntry(lineStart, lineEnd);

                if (ParseId(entry, ref s) == true)
                {
                    ParseAttributes(entry, ref s);
                    ParseValue(entry, ref s);

                    _CalendarEntries.Add(entry);
                }
            }
        }

        #region IsBlankLine

        /// <summary>
        /// Determines if the given line is blank
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsBlankLine(IEnumerable<char> s)
        {
            foreach (char c in s)
            {
                if (Char.IsWhiteSpace(c) == false)
                    return (false);
            }

            return (true);
        }

        #endregion

        #endregion

        #region ParseId

        /// <summary>
        /// Parses the Id portion of the given line
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool ParseId(CalendarEntry entry, ref string s)
        {
            Regex p = new Regex("^[^;:]+\\s*");
            Match ma = p.Match(s);

            if (ma.Success == false)
            {
                ReportError(entry, "Expected ID:");
                return (false);
            }

            entry.Id = ma.Value;

            s = s.Substring(ma.Index + ma.Length);

            return (true);
        }

        #endregion

        #region ParseAttributes

        #region ParseAttributes

        /// <summary>
        /// Initiates the parsing of the line attributes
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="s"></param>
        private void ParseAttributes(CalendarEntry entry, ref string s)
        {
            if (s.StartsWith(";") == true)
                ParseAttributeData(entry.Attributes, ref s);
        }

        #endregion

        #region ParseAttributeData

        /// <summary>
        /// Parses line attribute data
        /// </summary>
        /// <param name="list"></param>
        /// <param name="s"></param>
        private void ParseAttributeData(List<AttributeData> list, ref string s)
        {
            Regex p = new Regex("\\s*;\\s*(?<id>\\w+)\\s*=\\s*((?<value>\"[^\"]*\")|(?<value>[^;:]+))");
            MatchCollection mc = p.Matches(s);

            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    AttributeData attr = new
                        AttributeData(mc[i].Groups["id"].Value, mc[i].Groups["value"].Value);

                    list.Add(attr);
                }

                s = s.Substring(mc[mc.Count - 1].Index + mc[mc.Count - 1].Length);
            }
        }

        #endregion

        #endregion

        #region ParseValue

        #region ParseValue

        /// <summary>
        /// Parses the value portion of the given line
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="s"></param>
        private void ParseValue(CalendarEntry entry, ref string s)
        {
            Regex p = new Regex("^:\\s*((?<value>\"[^\"]\")|(?<value>.+))");
            Match ma = p.Match(s);

            if (ma.Success == true)
            {
                entry.Value = RemoveMetaData(ma.Groups["value"].Value);

                s = s.Substring(ma.Index + ma.Length);
            }
        }

        #endregion

        #region RemoveMetaData

        /// <summary>
        /// Removes Meta data from the value text
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string RemoveMetaData(string s)
        {
            Regex p = new Regex("\\\\[\\\\,;nN]");
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

                    char c = ma.Value[1];

                    if (c == 'n' || c == 'N')
                        c = '\n';

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

        #endregion

        #region GetRelativeDay

        /// <summary>
        /// Gets the eRelativeDayInMonth from the given dayOfWeek
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        protected virtual eRelativeDayInMonth GetRelativeDay(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 0:
                    return eRelativeDayInMonth.First;

                case 1:
                case 2:
                case 3:
                case 4:
                    return ((eRelativeDayInMonth)dayOfWeek);

                default:
                    return (eRelativeDayInMonth.Last);
            }
        }

        #endregion

        #region GetRelativeDayOfWeek

        /// <summary>
        /// Gets the DayOfWeek from the given eDayOfWeekRecurrence 
        /// </summary>
        /// <param name="byDays"></param>
        /// <returns></returns>
        protected virtual DayOfWeek GetRelativeDayOfWeek(eDayOfWeekRecurrence byDays)
        {
            if ((byDays & eDayOfWeekRecurrence.Monday) != 0)
                return (DayOfWeek.Monday);

            if ((byDays & eDayOfWeekRecurrence.Tuesday) != 0)
                return (DayOfWeek.Tuesday);

            if ((byDays & eDayOfWeekRecurrence.Wednesday) != 0)
                return (DayOfWeek.Wednesday);

            if ((byDays & eDayOfWeekRecurrence.Thursday) != 0)
                return (DayOfWeek.Thursday);

            if ((byDays & eDayOfWeekRecurrence.Friday) != 0)
                return (DayOfWeek.Friday);

            if ((byDays & eDayOfWeekRecurrence.Saturday) != 0)
                return (DayOfWeek.Saturday);

            return (DayOfWeek.Sunday);
        }

        #endregion

        #region IsValidCalName

        /// <summary>
        /// Determines if the given Calendar (by it's name) is
        /// a valid calendar to import
        /// </summary>
        /// <param name="calName"></param>
        /// <returns></returns>
        protected virtual bool IsValidCalName(string calName)
        {
            if (_CalNames == null)
                return (true);

            if (String.IsNullOrEmpty(calName) == true)
                return (true);

            for (int i = 0; i < _CalNames.Length; i++)
            {
                string name = _CalNames[i];

                if (string.IsNullOrEmpty(name) || name.Equals(calName))
                    return (true);
            }

            return (false);
        }

        #endregion

        #region GetOwnerKey

        /// <summary>
        /// Gets the OwnerKey based upon the
        /// given Calendar name
        /// </summary>
        /// <param name="calName"></param>
        /// <returns></returns>
        protected virtual string GetOwnerKey(string calName)
        {
            if (String.IsNullOrEmpty(calName) == true)
                return ("");

            if (_CalNames != null && _OwnerKeys != null)
            {
                for (int i = 0; i < _CalNames.Length; i++)
                {
                    string name = _CalNames[i];

                    if (string.IsNullOrEmpty(name) || name.Equals(calName))
                    {
                        if (_OwnerKeys.Length > i)
                            return (_OwnerKeys[i] ?? "");

                        break;
                    }
                }
            }

            return (calName);
        }

        #endregion

        #region ReportError

        /// <summary>
        /// Reports encountered import errors
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="text"></param>
        private void ReportError(CalendarEntry entry, string text)
        {
            if (IgnoreErrors == false)
            {
                if (entry != null)
                {
                    string s = String.Format("Error: {0}. \n{1}: Line {2}",
                                             text, _ImportFile, entry.LineStart);

                    if (entry.LineEnd - entry.LineStart > 1)
                        s += ("-" + (entry.LineEnd - 1));

                    throw new Exception(s);
                }
                else
                {
                    string s = String.Format("Error: {0}. {1}:", text, _ImportFile);

                    throw new Exception(s);
                }
            }
        }

        #endregion

        #region Class defs

        #region CalendarTimeZone

        private class CalendarTimeZone
        {
            #region Public variables

            public string TzId;
            public List<TimeZonePart> Part;

            #endregion
        }

        #endregion

        #region RDate

        #region RDate

        private class RDate
        {
            #region Public variables

            public DateRange[] DtRange;

            #endregion
        }

        #endregion

        #region DateRange

        private class DateRange
        {
            #region Public variables

            public DateTime StartTime;
            public DateTime EndTime;

            #endregion
        }

        #endregion

        #endregion

        #region RRule

        private class RRule
        {
            #region Public variables

            public Frequency Freq;
            public DateTime Until;

            public int Count;
            public int Interval = 1;

            public List<int>[] ByDay;

            public int[] ByMonth;
            public int[] ByMonthDay;

            public eDayOfWeekRecurrence ByDays;

            #endregion
        }

        #endregion

        #region TimeZonePart

        private class TimeZonePart
        {
            #region Public variables

            public DateTime StartDate;

            public int OffsetTo;

            public RRule RecurRule;
            public RDate RecurDate;

            public AppointmentSubsetCollection Appc;

            #endregion
        }

        #endregion

        #region TokenRegistry

        private class TokenRegistry : IComparable
        {
            #region Public variables

            public int Token;
            public string Text;

            #endregion

            public TokenRegistry(string text, int token)
            {
                Token = token;
                Text = text;
            }

            #region IComparable Members

            public int CompareTo(object obj)
            {
                string s = obj as string;

                return (s != null ? Text.CompareTo(s) : 0);
            }

            #endregion
        }

        #endregion

        #region VAlarm

        private class VAlarm
        {
            #region Public variables

            public DateTime Date;
            public TimeSpan Duration;
            public Related Rel = Related.Start;
            public string Description;

            #endregion
        }

        #endregion

        #region VEventData

        private class VEventData
        {
            #region Public variables

            public bool IsDtEndValue;
            public bool IsDtStartValue;

            public RRule RecurRule;
            public RDate RecurDate;

            public TimeSpan Duration;
            public List<VAlarm> VAlarms;
            public List<Appointment> UidApps;

            public DateTime RecIdDate;
            public DateTime RecStartDate;

            public AppProp AppPropSet;

            public List<DateTime> ExDates;

            public DateTime StartTime;
            public DateTime EndTime;

            #endregion
        }

        #endregion

        #endregion

        #region Private enums

        #region Frequency

        private enum Frequency
        {
            Secondly,
            Minutely,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly
        }

        #endregion

        #region Related

        private enum Related
        {
            Start,
            End,
            Date,
        }

        #endregion

        #region AppProp

        [Flags]
        private enum AppProp
        {
            Description = (1 << 0),
            Summary = (1 << 1),

            XDnbCategoryColor = (1 << 2),
            XDnbDisplayTemplate = (1 << 3),
            XDnbImageAlign = (1 << 4),
            XDnbImageKey = (1 << 5),
            XDnbLocked = (1 << 6),
            XDnbStartTimeAction = (1 << 7),
            XDnbTimeMarkedAs = (1 << 8),
            XDnbTooltip = (1 << 9),
        }

        #endregion

        #region Registry tokens

        #region ComponentToken

        private enum ComponentToken
        {
            Daylight,
            Standard,
            VAlarm,
            VCalendar,
            VEvent,
            VFreeBusy,
            VJournal,
            VTimezone,
            VTodo,
        }

        #endregion

        #region PropertyToken

        private enum PropertyToken
        {
            //Action,
            //Attach,
            //Attendee,
            Begin,
            CalScale,
            //Categories,
            //Class,
            Comment,
            //Completed,
            //Contact,
            //Created,
            Description,
            DtEnd,
            DtStamp,
            DtStart,
            Due,
            Duration,
            End,
            ExDate,
            //ExRule,
            //FreeBusy,
            //Geo,
            //LastModified,
            //Location,
            //Method,
            //Organizer,
            //PercentComplete,
            //Priority,
            //ProgId,
            RDate,
            RecurrenceId,
            //RelatedTo,
            //Repeat,
            //RequestStatus,
            //Resources,
            RRule,
            //Sequence,
            //Status,
            Summary,
            //Transp,
            Trigger,
            TzId,
            TzName,
            TzOffsetFrom,
            TzOffsetTo,
            //TzUrl,
            Uid,
            //Url,
            Version,
            XDnbCategoryColor,
            XDnbDisplayTemplate,
            XDnbImageAlign,
            XDnbImageKey,
            XDnbLocked,
            XDnbRecStartDate,
            XDnbStartTimeAction,
            XDnbTimeMarkedAs,
            XDnbTooltip,
            XWrCalname,
        }

        #endregion

        #region ParameterToken

        private enum ParameterToken
        {
            //AltRep,
            //Cn,
            //CuType,
            //DelegatedFrom,
            //DelegatedTo,
            //Dir,
            //Encoding,
            //FmtType,
            //FbType,
            //Language,
            //Member,
            //ParStat,
            //Range,
            Related,
            //RelType,
            //Role,
            //Rsvp,
            //SentBy,
            TzId,
            Value,
        }

        #endregion

        #region ValueToken

        private enum ValueToken
        {
            ByDay,
            ByHour,
            ByMinute,
            ByMonth,
            ByMonthDay,
            BySecond,
            BySetPos,
            ByWeekNo,
            ByYearDay,
            Count,
            Daily,
            Date,
            DateTime,
            Duration,
            End,
            Fr,
            Freq,
            Gregorian,
            Hourly,
            Interval,
            Minutely,
            Mo,
            Monthly,
            Period,
            Sa,
            Secondly,
            Start,
            Su,
            Text,
            Th,
            Tu,
            Until,
            We,
            Weekly,
            WkSt,
            Yearly,
        }

        #endregion

        #endregion

        #endregion

        #region Public enums

        [Flags]
        public enum IcsComponent
        {
            None = 0,

            Alarm = (1 << 0),
            Event = (1 << 1),
            Timezone = (1 << 2),
            Todo = (1 << 3),

            All = (Alarm | Event | Timezone |Todo),
        }

        #endregion
    }
}