#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents collection of working days.
    /// </summary>
    public class WorkDayCollection : Collection<WorkDay>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AppointmentCollection class.
        /// </summary>
        /// <param name="calendar"></param>
        public WorkDayCollection(CalendarModel calendar)
        {
            _Calendar = calendar;
        }

        /// <summary>
        /// Initializes a new instance of the WorkDayCollection class.
        /// </summary>
        /// <param name="owner"></param>
        public WorkDayCollection(Owner owner)
        {
            _Owner = owner;
        }
        #endregion
        #region Internal Implementation
        private Owner _Owner = null;
        /// <summary>
        /// Gets the Owner of work-day collection.
        /// </summary>
        public Owner Owner
        {
            get { return _Owner; }
            internal set { _Owner = value; }
        }

        private CalendarModel _Calendar = null;
        /// <summary>
        /// Gets the calendar collection is associated with.
        /// </summary>
        public CalendarModel Calendar
        {
            get { return _Calendar; }
            internal set
            {
                _Calendar = value;
                UpdateItemsCalendarModel();
            }
        }

        internal void UpdateItemsCalendarModel()
        {
            CalendarModel model = GetCalendarModel();
            foreach (WorkDay item in this.Items)
            {
                item.Calendar = model;
            }
        }

        protected override void RemoveItem(int index)
        {
            WorkDay day = this[index];
            OnBeforeRemove(index, day);
            base.RemoveItem(index);
            OnAfterRemove(index, day);
        }

        private void OnAfterRemove(int index, WorkDay day)
        {
            CalendarModel model = GetCalendarModel();
            if (model != null)
                model.WorkDayRemoved(day);
        }

        private void OnBeforeRemove(int index, WorkDay day)
        {
            day.Calendar = null;
        }

        protected override void InsertItem(int index, WorkDay item)
        {
            OnBeforeInsert(index, item);
            base.InsertItem(index, item);
            OnAfterInsert(index, item);
        }

        private void OnAfterInsert(int index, WorkDay item)
        {
            CalendarModel model = GetCalendarModel();
            if (model != null)
                model.WorkDayAdded(item);
        }

        private void OnBeforeInsert(int index, WorkDay item)
        {
            if (this[item.DayOfWeek] != null)
                throw new InvalidOperationException("Day '" + item.DayOfWeek.ToString() + "' already in collection.");
            item.Calendar = GetCalendarModel();
        }

        private CalendarModel GetCalendarModel()
        {
            if (_Calendar != null) return _Calendar;
            if (_Owner != null) return _Owner.Calendar;
            return null;
        }

        protected override void SetItem(int index, WorkDay newItem)
        {
            WorkDay oldItem = this[index];
            OnBeforeSetItem(index, oldItem, newItem);
            base.SetItem(index, newItem);
            OnAfterSetItem(index, oldItem, newItem);
        }

        private void OnAfterSetItem(int index, WorkDay oldItem, WorkDay newItem)
        {
            CalendarModel model = GetCalendarModel();
            if (model != null)
            {
                model.WorkDayRemoved(oldItem);
                model.WorkDayAdded(newItem);
            }
        }

        private void OnBeforeSetItem(int index, WorkDay oldItem, WorkDay newItem)
        {
            oldItem.Calendar = null;
            newItem.Calendar = GetCalendarModel();
        }

        protected override void ClearItems()
        {
            CalendarModel model = GetCalendarModel();
            foreach (WorkDay item in this)
            {
                item.Calendar = null;
                if (model != null)
                    model.WorkDayRemoved(item);
            }
            base.ClearItems();
        }

        /// <summary>
        /// Gets the item based on the Key assigned to the item
        /// </summary>
        /// <param name="day">Day of week to retrive data for.</param>
        /// <returns>Reference to WorkDay or null if no day in collection.</returns>
        public WorkDay this[DayOfWeek day]
        {
            get
            {
                foreach (WorkDay item in this.Items)
                {
                    if (item.DayOfWeek == day) return item;
                }
                return null;
            }
        }
        #endregion
    }
}
#endif

