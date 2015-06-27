#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DevComponents.Schedule.Model
{
    public class OwnerCollection : Collection<Owner>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AppointmentCollection class.
        /// </summary>
        /// <param name="calendar"></param>
        public OwnerCollection(CalendarModel calendar)
        {
            _Calendar = calendar;
        }

        #endregion
        #region Internal Implementation
        private CalendarModel _Calendar = null;
        /// <summary>
        /// Gets the calendar collection is associated with.
        /// </summary>
        public CalendarModel Calendar
        {
            get { return _Calendar; }
            internal set { _Calendar = value; }
        }

        protected override void RemoveItem(int index)
        {
            Owner owner = this[index];
            OnBeforeRemove(index, owner);
            base.RemoveItem(index);
            OnAfterRemove(index, owner);
        }

        private void OnAfterRemove(int index, Owner owner)
        {
            _Calendar.OwnerRemoved(owner);
        }

        private void OnBeforeRemove(int index, Owner owner)
        {
            owner.Calendar = null;
        }

        protected override void InsertItem(int index, Owner item)
        {
            OnBeforeInsert(index, item);
            base.InsertItem(index, item);
            OnAfterInsert(index, item);
        }

        private void OnAfterInsert(int index, Owner item)
        {
            _Calendar.OwnerAdded(item);
        }

        private void OnBeforeInsert(int index, Owner item)
        {
            item.Calendar = _Calendar;
        }

        protected override void SetItem(int index, Owner newItem)
        {
            Owner oldItem = this[index];
            OnBeforeSetItem(index, oldItem, newItem);
            base.SetItem(index, newItem);
            OnAfterSetItem(index, oldItem, newItem);
        }

        private void OnAfterSetItem(int index, Owner oldItem, Owner newItem)
        {
            _Calendar.OwnerRemoved(oldItem);
            _Calendar.OwnerAdded(newItem);
        }

        private void OnBeforeSetItem(int index, Owner oldItem, Owner newItem)
        {
            oldItem.Calendar = null;
            newItem.Calendar = _Calendar;
        }

        protected override void ClearItems()
        {
            foreach (Owner item in this)
            {
                item.Calendar = null;
                _Calendar.OwnerRemoved(item);
            }
            base.ClearItems();
        }

        /// <summary>
        /// Gets the item based on the Key assigned to the item
        /// </summary>
        /// <param name="ownerKey"></param>
        /// <returns></returns>
        public Owner this[string ownerKey]
        {
            get 
            {
                foreach (Owner item in this.Items)
                {
                    if (item.Key == ownerKey) return item;
                }
                return null;
            }
            //set 
            //{
            //    Owner owner = this[ownerKey];
            //    this[IndexOf(owner)] = value;
            //}
        }
        #endregion
    }
}
#endif

