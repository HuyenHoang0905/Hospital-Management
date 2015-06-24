#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using DevComponents.Schedule.Model.Primitives;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents subset of appointments collection.
    /// </summary>
    public class AppointmentSubsetCollection : CustomCollection<Appointment>
    {
        #region Private Variables
        private DateTime _StartDate = DateTime.MinValue;
        private DateTime _EndDate = DateTime.MinValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AppointmentSubsetCollection class with appointments between given start and end date.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public AppointmentSubsetCollection(CalendarModel calendar, DateTime start, DateTime end)
        {
            _Calendar = calendar;
            _StartDate = start;
            _EndDate = end;
            PopulateCollection();
        }
        #endregion

        #region Internal Implementation

        private CalendarModel _Calendar = null;
        /// <summary>
        /// Gets the calendar collection is associated with this collection.
        /// </summary>
        public CalendarModel Calendar
        {
            get { return _Calendar; }
            internal set { _Calendar = value; }
        }

        protected override void InsertItem(int index, Appointment item)
        {
            OnBeforeInsert(index, item);
            base.InsertItem(index, item);
        }

        private void OnBeforeInsert(int index, Appointment item)
        {
            item.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
            item.Calendar = _Calendar;
            if (item.IsRecurringInstance && _Calendar != null)
                _Calendar.InternalAppointmentAdded(item);

        }

        protected override void SetItem(int index, Appointment item)
        {
            OnBeforeSetItem(index, item);
            base.SetItem(index, item);
        }

        private void OnBeforeSetItem(int index, Appointment item)
        {
            Appointment app = this[index];
            app.SubPropertyChanged -= this.ChildPropertyChangedEventHandler;
            app.Calendar = null;
            item.SubPropertyChanged += this.ChildPropertyChangedEventHandler;
            app.Calendar = _Calendar;
        }

        protected override void RemoveItem(int index)
        {
            OnBeforeRemove(index);
            base.RemoveItem(index);
        }

        private void OnBeforeRemove(int index)
        {
            Appointment item = this[index];
            OnAppointmentRemoved(item);
        }

        private void OnAppointmentRemoved(Appointment item)
        {
            item.SubPropertyChanged -= this.ChildPropertyChangedEventHandler;
            if (item.IsRecurringInstance && _Calendar != null)
                _Calendar.InternalAppointmentRemoved(item, false);
            if (item.IsRecurringInstance)
                item.Calendar = null;
        }
        protected override void ClearItems()
        {
            foreach (Appointment item in this.GetItemsDirect())
            {
                OnAppointmentRemoved(item);
            }
            base.ClearItems();
        }

        private void PopulateCollection()
        {
            this.Clear();

            foreach (Appointment app in _Calendar.Appointments)
            {
                if (app.LocalStartTime >= _StartDate && app.LocalStartTime <= _EndDate || app.LocalEndTime > _StartDate && (app.LocalEndTime <= _EndDate || app.LocalStartTime < _StartDate))
                {
                    this.Add(app);
                }
                if (app.Recurrence != null && IsRecurrenceInRange(app, _StartDate, _EndDate))
                {
                    int count = this.GetItemsDirect().Count;
                    app.Recurrence.GenerateSubset(this, _StartDate, _EndDate);
                    if (count == this.GetItemsDirect().Count && TotalDaysDuration(app.StartTime, app.EndTime) >= 1 &&
                        app.LocalEndTime < _StartDate)
                    {
                        // Nothing generated lets wind back and look to see is there an recurrence that needs to be captured in this view
                        int daysLookBack = TotalDaysDuration(app.StartTime, app.EndTime);
                        DateTime start = _StartDate;
                        for (int i = 0; i < daysLookBack; i++)
                        {
                            start = start.AddDays(-1);
                            AppointmentSubsetCollection dayCollection = _Calendar.GetDay(start).Appointments;
                            foreach (Appointment subAppointment in dayCollection)
                            {
                                if (subAppointment.IsRecurringInstance && subAppointment.RootAppointment == app &&
                                    (subAppointment.LocalStartTime >= _StartDate && subAppointment.LocalStartTime <= _EndDate ||
                                     subAppointment.LocalEndTime > _StartDate && (subAppointment.LocalEndTime <= _EndDate ||
                                                                             subAppointment.LocalStartTime < _StartDate)))
                                {
                                    this.Add(subAppointment);
                                    daysLookBack = -1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            _IsCollectionUpToDate = true;
        }

        private int TotalDaysDuration(DateTime startTime, DateTime endTime)
        {
            if (endTime.Hour == 0 && endTime.Minute == 0 && endTime.Second == 0)
                endTime = endTime.AddMinutes(-1);
            return (int)Math.Max(0, Math.Ceiling(endTime.Date.Subtract(startTime.Date).TotalDays));
        }

        private bool IsRecurrenceInRange(Appointment app, DateTime startDate, DateTime endDate)
        {
            if (app.Recurrence.RecurrenceType == eRecurrencePatternType.Yearly)
            {
                // Simple range check on number of occurrences
                if (app.Recurrence.RangeLimitType == eRecurrenceRangeLimitType.RangeNumberOfOccurrences && app.LocalEndTime.Subtract(endDate).TotalDays / 365 > app.Recurrence.RangeNumberOfOccurrences)
                    return false;
                // Date check based on next expected recurrence date
                DateTime nextRecurrence = RecurrenceGenerator.GetNextYearlyRecurrence(app.Recurrence, startDate.Date.AddDays(-startDate.DayOfYear));
                if (nextRecurrence > endDate || nextRecurrence.Add(endDate.Subtract(startDate)) < startDate)
                    return false;
            }

            if (app.Recurrence.RangeEndDate == DateTime.MinValue || app.Recurrence.RangeEndDate >= _EndDate || app.Recurrence.RangeEndDate < _EndDate && app.Recurrence.RangeEndDate > _StartDate)
                return true;

            return false;
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
            InvalidateCollection();
        }

        /// <summary>
        /// Invalidates collection content due to the change to appointments or some other condition. Invalidating collection
        /// content causes the collection elements to be re-generated on next collection read access.
        /// </summary>
        public virtual void InvalidateCollection()
        {
            _IsCollectionUpToDate = false;
        }

        protected override void OnCollectionReadAccess()
        {
            if (!_IsCollectionUpToDate) PopulateCollection();
            base.OnCollectionReadAccess();
        }

        private bool _IsCollectionUpToDate = false;
        internal bool IsCollectionUpToDate
        {
            get { return _IsCollectionUpToDate; }
            set
            {
                _IsCollectionUpToDate = value;
            }
        }
        
        #endregion
    }
}
#endif

