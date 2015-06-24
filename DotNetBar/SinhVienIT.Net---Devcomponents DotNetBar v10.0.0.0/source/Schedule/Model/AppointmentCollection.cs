#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DevComponents.Schedule.Model
{
    public class AppointmentCollection : Collection<Appointment>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AppointmentCollection class.
        /// </summary>
        /// <param name="calendar"></param>
        public AppointmentCollection(CalendarModel calendar)
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

        protected override void InsertItem(int index, Appointment item)
        {
            OnBeforeInsert(index, item);
            base.InsertItem(index, item);
            OnAfterInsert(index, item);
        }

        private void OnAfterInsert(int index, Appointment item)
        {
            _Calendar.InternalAppointmentAdded(item);
        }

        private void OnBeforeInsert(int index, Appointment item)
        {
            item.Calendar = _Calendar;
        }

        protected override void SetItem(int index, Appointment item)
        {
            Appointment app = this[index];
            OnBeforeSetItem(index, app, item);
            base.SetItem(index, item);
            OnAfterSetItem(index, app, item);
        }

        private void OnAfterSetItem(int index, Appointment oldItem, Appointment newItem)
        {
            _Calendar.InternalAppointmentRemoved(oldItem, false);
            _Calendar.InternalAppointmentAdded(newItem);
        }

        private void OnBeforeSetItem(int index, Appointment oldItem, Appointment newItem)
        {
            oldItem.Calendar = null;
            newItem.Calendar = _Calendar;
        }

        protected override void RemoveItem(int index)
        {
            Appointment app = this[index];
            OnBeforeRemove(index);
            base.RemoveItem(index);
            OnAfterRemove(index, app);
        }

        private void OnAfterRemove(int index, Appointment app)
        {
            _Calendar.InternalAppointmentRemoved(app, false);
        }

        private void OnBeforeRemove(int index)
        {
            this[index].Calendar = null;
        }

        protected override void ClearItems()
        {
            foreach (Appointment item in this)
            {
                item.Calendar = null;
                _Calendar.InternalAppointmentRemoved(item, true);
            }
            base.ClearItems();
            _Calendar.InternalAppointmentsCleared();
        }
        #endregion
    }
}
#endif

