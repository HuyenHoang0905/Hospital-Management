using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents collection of calendar work days.
    /// </summary>
    public class CalendarWorkDayCollection : Collection<CalendarWorkDay>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AppointmentCollection class.
        /// </summary>
        /// <param name="calendar"></param>
        public CalendarWorkDayCollection(CalendarModel calendar)
        {
            _Calendar = calendar;
        }

        /// <summary>
        /// Initializes a new instance of the CalendarWorkDayCollection class.
        /// </summary>
        /// <param name="owner"></param>
        public CalendarWorkDayCollection(Owner owner)
        {
            _Owner = owner;
        }
        #endregion

        #region Internal Implementation
        private Dictionary<DateTime, CalendarWorkDay> _ItemsDictionary = new Dictionary<DateTime, CalendarWorkDay>();

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
            foreach (CalendarWorkDay item in this.Items)
            {
                item.Calendar = model;
            }
        }

        protected override void RemoveItem(int index)
        {
            CalendarWorkDay day = this[index];
            OnBeforeRemove(index, day);
            base.RemoveItem(index);
            OnAfterRemove(index, day);
        }

        private void OnAfterRemove(int index, CalendarWorkDay day)
        {
            _ItemsDictionary.Remove(day.Date);

            CalendarModel model = GetCalendarModel();
            if (model != null)
                model.CalendarWorkDateRemoved(day);
        }

        private void OnBeforeRemove(int index, CalendarWorkDay day)
        {
            day.Calendar = null;
        }

        protected override void InsertItem(int index, CalendarWorkDay item)
        {
            OnBeforeInsert(index, item);
            base.InsertItem(index, item);
            OnAfterInsert(index, item);
        }

        private void OnAfterInsert(int index, CalendarWorkDay item)
        {
            CalendarModel model = GetCalendarModel();
            if (model != null)
                model.CalendarWorkDateAdded(item);
            _ItemsDictionary.Add(item.Date, item);
        }

        private void OnBeforeInsert(int index, CalendarWorkDay item)
        {
            if (this[item.Date] != null)
                throw new InvalidOperationException("Date '" + item.Date.ToString() + "' already in collection.");
            item.Calendar = GetCalendarModel();
        }

        private CalendarModel GetCalendarModel()
        {
            if (_Calendar != null) return _Calendar;
            if (_Owner != null) return _Owner.Calendar;
            return null;
        }

        protected override void SetItem(int index, CalendarWorkDay newItem)
        {
            CalendarWorkDay oldItem = this[index];
            OnBeforeSetItem(index, oldItem, newItem);
            base.SetItem(index, newItem);
            OnAfterSetItem(index, oldItem, newItem);
        }

        private void OnAfterSetItem(int index, CalendarWorkDay oldItem, CalendarWorkDay newItem)
        {
            CalendarModel model = GetCalendarModel();
            if (model != null)
            {
                model.CalendarWorkDateRemoved(oldItem);
                model.CalendarWorkDateAdded(newItem);
            }
        }

        private void OnBeforeSetItem(int index, CalendarWorkDay oldItem, CalendarWorkDay newItem)
        {
            if (this[newItem.Date] != null)
                throw new InvalidOperationException("Date '" + newItem.Date.ToString() + "' already in collection.");

            oldItem.Calendar = null;
            newItem.Calendar = GetCalendarModel();
        }

        protected override void ClearItems()
        {
            CalendarModel model = GetCalendarModel();
            foreach (CalendarWorkDay item in this)
            {
                item.Calendar = null;
                if (model != null)
                    model.CalendarWorkDateRemoved(item);
            }
            base.ClearItems();
        }

        /// <summary>
        /// Gets the item based on the Key assigned to the item
        /// </summary>
        /// <param name="date">Date to retrieve data for.</param>
        /// <returns>Reference to CalendarWorkDay or null if no day in collection.</returns>
        public CalendarWorkDay this[DateTime date]
        {
            get
            {
                CalendarWorkDay item = null;
                if (_ItemsDictionary.TryGetValue(date.Date, out item))
                    return item;

                return null;
            }
        }
        #endregion
    }
}
