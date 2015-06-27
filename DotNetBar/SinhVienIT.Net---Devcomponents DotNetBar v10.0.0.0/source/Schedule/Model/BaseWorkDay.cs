using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Defines a working day.
    /// </summary>
    public abstract class BaseWorkDay : INotifyPropertyChanged, INotifySubPropertyChanged
    {
        #region Internal Implementation
        protected WorkTime _WorkStartTime = new WorkTime(8, 0);
        /// <summary>
        /// Gets or sets the work start time.
        /// </summary>
        public WorkTime WorkStartTime
        {
            get { return _WorkStartTime; }
            set
            {
                WorkTime oldValue = _WorkStartTime;
                _WorkStartTime = value;
                OnWorkStartTimeChanged(oldValue, value);
            }
        }
        /// <summary>
        /// Called when WorkStartTime has changed.
        /// </summary>
        /// <param name="oldValue">Old property value.</param>
        /// <param name="newValue">New property value.</param>
        protected virtual void OnWorkStartTimeChanged(WorkTime oldValue, WorkTime newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("WorkStartTime"));
        }

        protected WorkTime _WorkEndTime = new WorkTime(17, 0);
        /// <summary>
        /// Gets or sets the work end time.
        /// </summary>
        public WorkTime WorkEndTime
        {
            get { return _WorkEndTime; }
            set
            {
                WorkTime oldValue = _WorkEndTime;
                _WorkEndTime = value;
                OnWorkEndTimeChanged(oldValue, value);
            }
        }
        /// <summary>
        /// Called when WorkEndTime has changed.
        /// </summary>
        /// <param name="oldValue">Old property value.</param>
        /// <param name="newValue">New property value.</param>
        protected virtual void OnWorkEndTimeChanged(WorkTime oldValue, WorkTime newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("WorkEndTime"));

        }

        private CalendarModel _Calendar = null;
        /// <summary>
        /// Gets the calendar work day is associated with.
        /// </summary>
        [Browsable(false)]
        public CalendarModel Calendar
        {
            get { return _Calendar; }
            internal set
            {
                if (_Calendar != value)
                {
                    _Calendar = value;
                }
            }
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
