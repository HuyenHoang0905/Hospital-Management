#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevComponents.Schedule.Model.Primitives;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents appointment recurrence definition.
    /// </summary>
    public class AppointmentRecurrence : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Internal Implementation
        public AppointmentRecurrence()
        {
            _SkippedRecurrences = new CustomCollection<DateTime>();
            _SkippedRecurrences.CollectionChanged += new NotifyCollectionChangedEventHandler(SkippedRecurrencesCollectionChanged);
        }

        private eRecurrenceRangeLimitType _RangeType = eRecurrenceRangeLimitType.NoEndDate;
        /// <summary>
        /// Gets or sets the range type for the recurrence. Default value is no end date for recurrence.
        /// </summary>
        [DefaultValue(eRecurrenceRangeLimitType.NoEndDate)]
        public eRecurrenceRangeLimitType RangeLimitType
        {
            get { return _RangeType; }
            set
            {
                if (_RangeType != value)
                {
                    eRecurrenceRangeLimitType oldValue = _RangeType;
                    _RangeType = value;
                    OnRangeTypeChanged(oldValue, _RangeType);
                }
            }
        }

        private void OnRangeTypeChanged(eRecurrenceRangeLimitType oldValue, eRecurrenceRangeLimitType newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RangeLimitType"));
        }

        private DateTime _RangeEndDate = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the recurrence end date. To specify the end date for recurrence set this property and RangeLimitType property to RangeEndDate.
        /// </summary>
        public DateTime RangeEndDate
        {
            get { return _RangeEndDate; }
            set 
            {
                if (_RangeEndDate != value)
                {
                    DateTime oldValue = _RangeEndDate;
                    _RangeEndDate = value;
                    OnRangeEndDateChanged(oldValue, _RangeEndDate);
                }
            }
        }

        private void OnRangeEndDateChanged(DateTime oldValue, DateTime newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RangeEndDate"));
        }

        private int _RangeNumberOfOccurrences = 0;
        /// <summary>
        /// Gets or sets number of occurrences after which recurrence ends. To specify limited number of recurrences
        /// set this property and set RangeLimitType to RangeNumberOfOccurrences.
        /// </summary>
        [DefaultValue(0)]
        public int RangeNumberOfOccurrences
        {
            get { return _RangeNumberOfOccurrences; }
            set
            {
                if (_RangeNumberOfOccurrences != value)
                {
                    if (value < 0) throw new ArgumentException("Negative values not allows for range limit.");

                    int oldValue = _RangeNumberOfOccurrences;
                    _RangeNumberOfOccurrences = value;
                    OnRangeNumberOfOccurrencesChanged(oldValue, _RangeNumberOfOccurrences);
                }
            }
        }

        private void OnRangeNumberOfOccurrencesChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RangeNumberOfOccurrences"));
        }

        private object _Tag = null;
        /// <summary>
        /// Gets or sets additional data associated with the object.
        /// </summary>
        [DefaultValue(null)]
        public object Tag
        {
            get { return _Tag; }
            set
            {
                if (value != _Tag)
                {
                    object oldValue = _Tag;
                    _Tag = value;
                    OnTagChanged(oldValue, value);
                }
            }
        }

        private void OnTagChanged(object oldValue, object newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Tag"));

        }

        private eRecurrencePatternType _RecurrenceType = eRecurrencePatternType.Daily;
        /// <summary>
        /// Gets or sets the recurring frequency for appointment i.e. daily, weekly, monthly or yearly.
        /// Default value is Daily.
        /// </summary>
        [DefaultValue(eRecurrencePatternType.Daily)]
        public eRecurrencePatternType RecurrenceType
        {
            get { return _RecurrenceType; }
            set
            {
                if (value != _RecurrenceType)
                {
                    eRecurrencePatternType oldValue = _RecurrenceType;
                    _RecurrenceType = value;
                    OnRecurrenceTypeChanged(oldValue, value);
                }
            }
        }

        private void OnRecurrenceTypeChanged(eRecurrencePatternType oldValue, eRecurrencePatternType newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RecurrenceType"));
        }

        internal void GenerateSubset(AppointmentSubsetCollection subsetCollection, DateTime startDate, DateTime endDate)
        {
            IRecurrenceGenerator generator = GetRecurrenceGenerator();

            if (_RecurrenceType == eRecurrencePatternType.Daily)
            {
                generator.GenerateDailyRecurrence(subsetCollection, this, startDate, endDate);
            }
            else if (_RecurrenceType == eRecurrencePatternType.Weekly)
            {
                generator.GenerateWeeklyRecurrence(subsetCollection, this, startDate, endDate);
            }
            else if (_RecurrenceType == eRecurrencePatternType.Monthly)
            {
                generator.GenerateMonthlyRecurrence(subsetCollection, this, startDate, endDate);
            }
            else if (_RecurrenceType == eRecurrencePatternType.Yearly)
            {
                generator.GenerateYearlyRecurrence(subsetCollection, this, startDate, endDate);
            }
        }

        private IRecurrenceGenerator _Generator = null;
        private IRecurrenceGenerator GetRecurrenceGenerator()
        {
            if (_Generator == null) _Generator = new RecurrenceGenerator();
            return _Generator;
        }

        private Appointment _Appointment;
        /// <summary>
        /// Gets reference to appointment recurrence is assigned to.
        /// </summary>
        [Browsable(false)]
        public Appointment Appointment
        {
            get { return _Appointment; }
            internal set { _Appointment = value; }
        }

        private DailyRecurrenceSettings _Daily;
        /// <summary>
        /// Gets the settings for Daily recurrence type.
        /// </summary>
        [Browsable(false)]
        public DailyRecurrenceSettings Daily
        {
            get 
            {
                if (_Daily == null)
                {
                    _Daily = new DailyRecurrenceSettings(this);
                    _Daily.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
                }
                return _Daily; 
            }
        }

        private WeeklyRecurrenceSettings _Weekly;
        /// <summary>
        /// Gets the settings for Weekly recurrence type.
        /// </summary>
        public WeeklyRecurrenceSettings Weekly
        {
            get 
            {
                if (_Weekly == null)
                {
                    _Weekly = new WeeklyRecurrenceSettings(this);
                    _Weekly.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
                }
                return _Weekly; 
            }
        }

        private MonthlyRecurrenceSettings _Monthly;
        /// <summary>
        /// Gets the settings for monthly recurrence type.
        /// </summary>
        [Browsable(false)]
        public MonthlyRecurrenceSettings Monthly
        {
            get 
            {
                if (_Monthly == null)
                {
                    _Monthly = new MonthlyRecurrenceSettings(this);
                    _Monthly.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
                }
                return _Monthly; 
            }
        }

        private YearlyRecurrenceSettings _Yearly = null;
        /// <summary>
        /// Gets the settings for yearly recurrence type.
        /// </summary>
        public YearlyRecurrenceSettings Yearly
        {
            get
            {
                if (_Yearly == null)
                {
                    _Yearly = new YearlyRecurrenceSettings(this);
                    _Yearly.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
                }
                return _Yearly; 
            }
        }

        private DateTime _RecurrenceStartDate = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the recurrence start date. Default value is DateTime.MinValue which indicates that recurrence starts after
        /// the appointment ends.
        /// </summary>
        public DateTime RecurrenceStartDate
        {
            get { return _RecurrenceStartDate; }
            set
            {
                if (value != _RecurrenceStartDate)
                {
                    DateTime oldValue = _RecurrenceStartDate;
                    _RecurrenceStartDate = value;
                    OnRecurrenceStartDateChanged(oldValue, value);
                }
            }
        }

        private void OnRecurrenceStartDateChanged(DateTime oldValue, DateTime newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("RecurrenceStartDate"));
        }

        private SubPropertyChangedEventHandler _ChildPropertyChangedEventHandler = null;
        private SubPropertyChangedEventHandler ChildPropertyChangedEventHandler
        {
            get
            {
                if (_ChildPropertyChangedEventHandler == null) _ChildPropertyChangedEventHandler = new SubPropertyChangedEventHandler(ChildPropertyChanged);
                return _ChildPropertyChangedEventHandler;
            }
        }

        private void ChildPropertyChanged(object sender, SubPropertyChangedEventArgs e)
        {
            OnSubPropertyChanged(e);
        }

        private CustomCollection<DateTime> _SkippedRecurrences = null;
        /// <summary>
        /// Gets or set the list of dates on which the recurrences are skipped.
        /// </summary>
        public CustomCollection<DateTime> SkippedRecurrences
        {
            get
            {
                return _SkippedRecurrences;
            }
        }
        private void SkippedRecurrencesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("SkippedRecurrences"));
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler eh = PropertyChanged;
            if (eh != null) eh(this, e);
            OnSubPropertyChanged(new SubPropertyChangedEventArgs(this, e));
        }
        #endregion

        #region INotifySubPropertyChanged Members
        /// <summary>
        /// Occurs when property or property of child objects has changed. This event is similar to PropertyChanged event with key
        /// difference that it occurs for the property changed of child objects as well.
        /// </summary>
        public event SubPropertyChangedEventHandler SubPropertyChanged;
        /// <summary>
        /// Raises the SubPropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnSubPropertyChanged(SubPropertyChangedEventArgs e)
        {
            SubPropertyChangedEventHandler eh = SubPropertyChanged;
            if (eh != null) eh(this, e);
        }
        #endregion
    }
}
#endif

