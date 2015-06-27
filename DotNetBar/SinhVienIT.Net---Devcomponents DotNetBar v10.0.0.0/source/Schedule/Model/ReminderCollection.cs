#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Represents collection of reminders.
    /// </summary>
    public class ReminderCollection : Collection<Reminder>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ReminderCollection class.
        /// </summary>
        /// <param name="parent"></param>
        public ReminderCollection(Appointment parent)
        {
            _Appointment = parent;
        }
        CalendarModel _Model = null;
        /// <summary>
        /// Initializes a new instance of the ReminderCollection class.
        /// </summary>
        /// <param name="parent"></param>
        public ReminderCollection(CalendarModel parentModel)
        {
            _Model = parentModel;
        }
        #endregion

        #region Internal Implementation
        protected override void InsertItem(int index, Reminder item)
        {
            OnBeforeInsert(index, item);
            base.InsertItem(index, item);
        }

        private void OnBeforeInsert(int index, Reminder item)
        {
            item.Appointment = _Appointment;
            item.ParentCollection = this;
        }

        protected override void SetItem(int index, Reminder item)
        {
            OnBeforeSetItem(index, item);
            base.SetItem(index, item);
        }

        private void OnBeforeSetItem(int index, Reminder item)
        {
            this[index].Appointment = null;
            this[index].ParentCollection = null;
            item.Appointment = _Appointment;
            item.ParentCollection = this;
        }

        protected override void RemoveItem(int index)
        {
            OnBeforeRemove(index);
            base.RemoveItem(index);
        }

        private void OnBeforeRemove(int index)
        {
            this[index].Appointment = null;
            this[index].ParentCollection = null;
        }

        protected override void ClearItems()
        {
            foreach (Reminder item in this)
            {
                item.Appointment = null;
            }
            base.ClearItems();
        }

        private Appointment _Appointment;
        /// <summary>
        /// Gets parent appointment.
        /// </summary>
        public Appointment Appointment
        {
            get { return _Appointment; }
            internal set
            {
                _Appointment = value;
            }
        }

        /// <summary>
        /// Gets parent model if collection is custom reminders collection.
        /// </summary>
        public CalendarModel ParentModel
        {
            get { return _Model; }
            internal set { _Model = value; }
        }
        #endregion
    }
}
#endif

