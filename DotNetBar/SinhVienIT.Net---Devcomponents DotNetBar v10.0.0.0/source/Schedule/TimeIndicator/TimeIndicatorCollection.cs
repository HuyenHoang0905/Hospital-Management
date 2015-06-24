#if FRAMEWORK20
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class TimeIndicatorCollection : Collection<TimeIndicator>
    {
        #region Events

        /// <summary>
        /// Occurs when the TimeIndicator collection has changed
        /// </summary>
        [Description("Occurs when the TimeIndicator collection has changed.")]
        public event EventHandler<EventArgs> TimeIndicatorCollectionChanged;

        /// <summary>
        /// Occurs when a TimeIndicator time has changed
        /// </summary>
        [Description("Occurs when a TimeIndicator time has changed.")]
        public event EventHandler<TimeIndicatorTimeChangedEventArgs> TimeIndicatorTimeChanged;

        /// <summary>
        /// Occurs when a TimeIndicator Color has changed
        /// </summary>
        [Description("Occurs when a TimeIndicator Color has changed.")]
        public event EventHandler<TimeIndicatorColorChangedEventArgs> TimeIndicatorColorChanged;

        #endregion

        #region Private variables

        private int _UpdateCount;
        private Timer _Timer;

        #endregion

        #region AddRange

        /// <summary>
        /// Adds a range of TimeIndicators to the collection
        /// </summary>
        /// <param name="items">Array of items to add</param>
        public void AddRange(TimeIndicator[] items)
        {
            try
            {
                BeginUpdate();

                for (int i = 0; i < items.Length; i++)
                    Add(items[i]);
            }
            finally
            {
                EndUpdate();
            }
        }

        #endregion

        #region RemoveItem

        /// <summary>
        /// Processes list RemoveItem calls
        /// </summary>
        /// <param name="index">Index to remove</param>
        protected override void RemoveItem(int index)
        {
            if (Items[index].IsProtected == false)
            {
                Items[index].TimeIndicatorChanged -= IndicatorCollectionChanged;
                Items[index].TimeIndicatorTimeChanged -= IndicatorTimeChanged;
                Items[index].TimeIndicatorColorChanged -= IndicatorColorChanged;

                base.RemoveItem(index);

                OnCollectionChanged();
            }
        }

        #endregion

        #region InsertItem

        /// <summary>
        /// Processes list InsertItem calls
        /// </summary>
        /// <param name="index">Index to add</param>
        /// <param name="item">TimeIndicator to add</param>
        protected override void InsertItem(int index, TimeIndicator item)
        {
            if (item != null)
            {
                item.TimeIndicatorChanged += IndicatorCollectionChanged;
                item.TimeIndicatorTimeChanged += IndicatorTimeChanged;
                item.TimeIndicatorColorChanged += IndicatorColorChanged;

                base.InsertItem(index, item);

                OnCollectionChanged();
            }
        }

        #endregion

        #region SetItem

        /// <summary>
        /// Processes list SetItem calls (e.g. replace)
        /// </summary>
        /// <param name="index">Index to replace</param>
        /// <param name="newItem">TimeIndicator to replace</param>
        protected override void SetItem(int index, TimeIndicator newItem)
        {
            if (Items[index].IsProtected == false)
            {
                if (newItem != null)
                {
                    Items[index].TimeIndicatorChanged -= IndicatorCollectionChanged;
                    Items[index].TimeIndicatorTimeChanged -= IndicatorTimeChanged;
                    Items[index].TimeIndicatorColorChanged -= IndicatorColorChanged;

                    newItem.TimeIndicatorChanged += IndicatorCollectionChanged;
                    newItem.TimeIndicatorTimeChanged += IndicatorTimeChanged;
                    newItem.TimeIndicatorColorChanged += IndicatorColorChanged;

                    base.SetItem(index, newItem);

                    OnCollectionChanged();
                }
            }
        }

        #endregion

        #region ClearItems

        /// <summary>
        /// Processes list Clear calls (e.g. remove all)
        /// </summary>
        protected override void ClearItems()
        {
            try
            {
                BeginUpdate();

                for (int i = Count - 1; i>=0 ; i--)
                {
                    if (Items[i].IsProtected == false)
                    {
                        Items[i].TimeIndicatorChanged -= IndicatorCollectionChanged;
                        Items[i].TimeIndicatorTimeChanged -= IndicatorTimeChanged;
                        Items[i].TimeIndicatorColorChanged -= IndicatorColorChanged;

                        RemoveAt(i);
                    }
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        #endregion

        #region Events

        #region IndicatorCollectionChanged

        /// <summary>
        /// IndicatorCollectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IndicatorCollectionChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        }

        #endregion

        #region IndicatorColorChanged

        /// <summary>
        /// IndicatorColorChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IndicatorColorChanged(object sender, TimeIndicatorColorChangedEventArgs e)
        {
            OnTimeIndicatorColorChanged(e);
        }

        #endregion

        #region IndicatorTimeChanged

        /// <summary>
        /// IndicatorTimeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IndicatorTimeChanged(object sender, TimeIndicatorTimeChangedEventArgs e)
        {
            OnTimeIndicatorTimeChanged(e);
        }

        #endregion

        #endregion

        #region OnCollectionChanged

        /// <summary>
        /// Propagates TimeIndicatorCollectionChanged events
        /// </summary>
        protected virtual void OnCollectionChanged()
        {
            if (_UpdateCount == 0)
            {
                UpdateTimerUse();

                if (TimeIndicatorCollectionChanged != null)
                    TimeIndicatorCollectionChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region OnTimeIndicatorColorChanged

        /// <summary>
        /// Propagates OnTimeIndicatorColorChanged events
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTimeIndicatorColorChanged(TimeIndicatorColorChangedEventArgs e)
        {
            if (_UpdateCount == 0)
            {
                if (TimeIndicatorColorChanged != null)
                    TimeIndicatorColorChanged(this, e);
            }
        }

        #endregion

        #region OnTimeIndicatorTimeChanged

        /// <summary>
        /// Propagates OnTimeIndicatorTimeChanged events
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTimeIndicatorTimeChanged(TimeIndicatorTimeChangedEventArgs e)
        {
            if (_UpdateCount == 0)
            {
                if (TimeIndicatorTimeChanged != null)
                    TimeIndicatorTimeChanged(this, e);
            }
        }

        #endregion

        #region System Timer support

        #region UpdateTimerUse

        /// <summary>
        /// Updates our system timer use
        /// </summary>
        private void UpdateTimerUse()
        {
            // If we need a timer, then allocate it
            // and initialize it to fire approx every minute

            if (TimerNeeded() == true)
            {
                if (_Timer == null)
                {
                    _Timer = new Timer();

                    _Timer.Tick += Timer_Tick;

                    DateTime now = DateTime.Now;
                    _Timer.Interval = (60 - now.Second) * 1000 + (1050 - now.Millisecond);

                    _Timer.Enabled = true;
                }
            }
            else
            {
                if (_Timer != null)
                {
                    _Timer.Enabled = false;
                    _Timer.Dispose();
                    _Timer = null;
                }
            }
        }

        #endregion

        #region TimerNeeded

        /// <summary>
        /// Determines if a system timer is needed
        /// </summary>
        /// <returns>true if needed</returns>
        private bool TimerNeeded()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                TimeIndicator ti = Items[i];

                if (ti.IsDesignMode == true)
                    return (false);

                if (ti.Enabled == true &&
                    ti.IndicatorSource == eTimeIndicatorSource.SystemTime)
                {
                    return (true);
                }
            }

            return (false);
        }

        #endregion

        #region Timer_Tick

        /// <summary>
        /// Handles our timer tick events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            _Timer.Interval = (60 - now.Second) * 1000 + (1050 - now.Millisecond);

            for (int i = 0; i < Items.Count; i++)
            {
                TimeIndicator ti = Items[i];

                if (ti.Enabled == true &&
                    ti.IndicatorSource == eTimeIndicatorSource.SystemTime)
                {
                    ti.IndicatorTime = now;
                }
            }
        }

        #endregion

        #endregion

        #region Begin/EndUpdate

        /// <summary>
        /// Begins Update block
        /// </summary>
        public void BeginUpdate()
        {
            _UpdateCount++;
        }

        /// <summary>
        /// Ends update block
        /// </summary>
        public void EndUpdate()
        {
            if (_UpdateCount == 0)
            {
                throw new InvalidOperationException(
                    "EndUpdate must be called After BeginUpdate");
            }

            _UpdateCount--;

            if (_UpdateCount == 0)
                OnCollectionChanged();
        }

        #endregion
    }

    #region TimeIndicatorTimeChangedEventArgs

    /// <summary>
    /// TimeIndicatorTimeChangedEventArgs
    /// </summary>
    public class TimeIndicatorTimeChangedEventArgs : EventArgs
    {
        #region Private variables

        private TimeIndicator _TimeIndicator;

        private DateTime _OldTime;
        private DateTime _NewTime;

        #endregion

        public TimeIndicatorTimeChangedEventArgs(
            TimeIndicator timeIndicator, DateTime oldTime, DateTime newTime)
        {
            _TimeIndicator = timeIndicator;

            _OldTime = oldTime;
            _NewTime = newTime;
        }

        #region Public properties

        /// <summary>
        /// Gets the TimeIndicator being affected
        /// </summary>
        public TimeIndicator TimeIndicator
        {
            get { return (_TimeIndicator); }
        }

        /// <summary>
        /// Gets the old DateTime
        /// </summary>
        public DateTime OldTime
        {
            get { return (_OldTime); }
        }

        /// <summary>
        /// Gets the new DateTime
        /// </summary>
        public DateTime NewTime
        {
            get { return (_NewTime); }
        }

        #endregion
    }

    #endregion

    #region TimeIndicatorColorChangedEventArgs

    /// <summary>
    /// TimeIndicatorColorChangedEventArgs
    /// </summary>
    public class TimeIndicatorColorChangedEventArgs : EventArgs
    {
        #region Private variables

        private TimeIndicator _TimeIndicator;

        #endregion

        public TimeIndicatorColorChangedEventArgs(TimeIndicator timeIndicator)
        {
            _TimeIndicator = timeIndicator;
        }

        #region Public properties

        /// <summary>
        /// Gets the TimeIndicator being affected
        /// </summary>
        public TimeIndicator TimeIndicator
        {
            get { return (_TimeIndicator); }
        }

        #endregion
    }

    #endregion
}
#endif

