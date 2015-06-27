using System;
using System.Collections.ObjectModel;
using System.Drawing;
using DevComponents.Schedule.Model;

namespace DevComponents.DotNetBar.Schedule
{
    /// <summary>
    /// ViewDisplayCustomizations
    /// </summary>
    public class ViewDisplayCustomizations
    {
        #region Events

        /// <summary>
        /// Occurs when the ViewDisplayCustomizations have changed
        /// </summary>
        public event EventHandler<EventArgs> CollectionChanged;

        #endregion

        #region Private variables

        private CalendarView _CalendarView;
        private DaySlotBackgrounds _DaySlotBackgrounds;

        #endregion

        /// <summary>
        /// ViewDisplayCustomizations
        /// </summary>
        /// <param name="calendarView"></param>
        public ViewDisplayCustomizations(CalendarView calendarView)
        {
            _CalendarView = calendarView;
        }

        #region Public properties

        /// <summary>
        /// DaySlotBackgrounds
        /// </summary>
        public DaySlotBackgrounds DaySlotBackgrounds
        {
            get
            {
                if (_DaySlotBackgrounds == null)
                {
                    _DaySlotBackgrounds = new DaySlotBackgrounds();
                    _DaySlotBackgrounds.CollectionChanged += _DaySlotBackgrounds_CollectionChanged;
                }

                return (_DaySlotBackgrounds);
            }

            set
            {
                if (_DaySlotBackgrounds != null)
                    _DaySlotBackgrounds.CollectionChanged -= _DaySlotBackgrounds_CollectionChanged;

                _DaySlotBackgrounds = value;

                if (_DaySlotBackgrounds != null)
                    _DaySlotBackgrounds.CollectionChanged += _DaySlotBackgrounds_CollectionChanged;
            }
        }
        
        #endregion

        #region Event handling

        /// <summary>
        /// Handles DaySlotBackgrounds CollectionChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _DaySlotBackgrounds_CollectionChanged(object sender, EventArgs e)
        {
            _CalendarView.Refresh();

            if (CollectionChanged != null)
                CollectionChanged(this, EventArgs.Empty);
        }

        #endregion

        #region GetDaySlotAppearance

        /// <summary>
        /// Retrieves the DaySlotAppearance from the given criteris
        /// </summary>
        /// <param name="ownerKey"></param>
        /// <param name="dateTime"></param>
        /// <param name="wkStart"></param>
        /// <param name="wkEnd"></param>
        /// <returns></returns>
        internal DaySlotAppearance GetDaySlotAppearance(
            string ownerKey, DateTime dateTime, WorkTime wkStart, WorkTime wkEnd)
        {
            foreach (DaySlotBackground dsb in DaySlotBackgrounds)
            {
                if (IsValidSlot(dsb, dateTime, wkStart, wkEnd) == true)
                {
                    if (IsValidOwner(dsb, ownerKey) == true)
                        return (dsb.Appearance);
                }
            }

            return (null);
        }

        #endregion

        #region IsValidSlot

        /// <summary>
        /// Determines if the given slot is a valid
        /// day and time slot
        /// </summary>
        /// <param name="dsb"></param>
        /// <param name="dateTime"></param>
        /// <param name="wkStart"></param>
        /// <param name="wkEnd"></param>
        /// <returns></returns>
        private bool IsValidSlot(DaySlotBackground dsb,
            DateTime dateTime, WorkTime wkStart, WorkTime wkEnd)
        {
            if (IsValidDaySlot(dsb, dateTime) == true)
                return (IsValidTimeSlot(dsb, wkStart, wkEnd));

            return (false);
        }

        #endregion

        #region IsValidTimeSlot

        /// <summary>
        /// Determines if the given slot is a valid time slot
        /// </summary>
        /// <param name="dsb"></param>
        /// <param name="wkStart"></param>
        /// <param name="wkEnd"></param>
        /// <returns></returns>
        private bool IsValidTimeSlot(DaySlotBackground dsb, WorkTime wkStart, WorkTime wkEnd)
        {
            return (wkStart < dsb.Appearance.EndTime && wkEnd > dsb.Appearance.StartTime);
        }

        #endregion

        #region IsValidDaySlot

        /// <summary>
        /// Determines if the given slot is a valid time slot
        /// </summary>
        /// <param name="dsb"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private bool IsValidDaySlot(DaySlotBackground dsb, DateTime dateTime)
        {
            if (dsb.DateTime == DateTime.MinValue)
                return (dsb.DayOfWeek == dateTime.DayOfWeek);

            return (dsb.DateTime.Date == dateTime.Date);
        }

        #endregion

        #region IsValidOwner

        /// <summary>
        /// Determines if the given owner key is valid for the slot
        /// </summary>
        /// <param name="dsb"></param>
        /// <param name="ownerKey"></param>
        /// <returns></returns>
        private bool IsValidOwner(DaySlotBackground dsb, string ownerKey)
        {
            if (dsb.HasOwnerKeys == true)
            {
                if (dsb.OwnerKeys.Contains("") == true)
                    return (true);

                if (dsb.OwnerKeys.Contains(ownerKey) == false)
                    return (false);
            }

            return (true);
        }

        #endregion
    }

    /// <summary>
    /// DaySlotBackgrounds
    /// </summary>
    public class DaySlotBackgrounds : Collection<DaySlotBackground>
    {
        #region Events

        /// <summary>
        /// Occurs when the DaySlotBackgrounds collection changes
        /// </summary>
        public event EventHandler<EventArgs> CollectionChanged;

        #endregion

        #region Private variables

        private bool _IsRangeSet;
        private bool _SuspendUpdate;

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets and sets the SuspendUpdate state
        /// </summary>
        internal bool SuspendUpdate
        {
            get { return (_SuspendUpdate); }
            set { _SuspendUpdate = value; }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the DaySlotBackground for the given DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        public void Remove(DateTime dateTime)
        {
            int n = Items.Count;

            try
            {
                SuspendUpdate = true;

                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    DaySlotBackground dsb = Items[i];

                    if (dsb.DateTime.Equals(dateTime) == true)
                        RemoveAt(i);
                }
            }
            finally
            {
                SuspendUpdate = false;

                if (Items.Count != n)
                    OnCollectionChanged();
            }
        }

        /// <summary>
        /// Removes the DaySlotBackground for the given DayOfWeek
        /// </summary>
        /// <param name="dayOfWeek"></param>
        public void Remove(DayOfWeek dayOfWeek)
        {
            int n = Items.Count;

            try
            {
                SuspendUpdate = true;

                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    DaySlotBackground dsb = Items[i];

                    if (dsb.DateTime.Equals(DateTime.MinValue) == true)
                    {
                        if (dsb.DayOfWeek == dayOfWeek)
                            RemoveAt(i);
                    }
                }
            }
            finally
            {
                SuspendUpdate = false;

                if (Items.Count != n)
                    OnCollectionChanged();
            }
        }

        #endregion

        #region AddRange

        /// <summary>
        /// Adds a range of DaySlotBackgrounds
        /// </summary>
        /// <param name="daySlotBackgrounds"></param>
        public void AddRange(DaySlotBackgrounds daySlotBackgrounds)
        {
            try
            {
                _IsRangeSet = true;

                foreach (DaySlotBackground dsb in daySlotBackgrounds)
                    Add(dsb);
            }
            finally
            {
                _IsRangeSet = false;

                OnCollectionChanged();
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
            base.RemoveItem(index);

            OnCollectionChanged();
        }

        #endregion

        #region InsertItem

        /// <summary>
        /// Processes list InsertItem calls
        /// </summary>
        /// <param name="index">Index to add</param>
        /// <param name="item">Text to add</param>
        protected override void InsertItem(int index, DaySlotBackground item)
        {
            if (item != null)
            {
                base.InsertItem(index, item);

                item.DaySlotBackgrounds = this;

                OnCollectionChanged();
            }
        }

        #endregion

        #region SetItem

        /// <summary>
        /// Processes list SetItem calls (e.g. replace)
        /// </summary>
        /// <param name="index">Index to replace</param>
        /// <param name="newItem">Text to replace</param>
        protected override void SetItem(int index, DaySlotBackground newItem)
        {
            base.SetItem(index, newItem);

            newItem.DaySlotBackgrounds = this;

            OnCollectionChanged();
        }

        #endregion

        #region ClearItems

        /// <summary>
        /// Processes list Clear calls (e.g. remove all)
        /// </summary>
        protected override void ClearItems()
        {
            if (Count > 0)
            {
                base.ClearItems();

                OnCollectionChanged();
            }
        }

        #endregion

        #region OnCollectionChanged

        /// <summary>
        /// Handles collection change notification
        /// </summary>
        private void OnCollectionChanged()
        {
            if (_SuspendUpdate == false && _IsRangeSet == false)
            {
                if (CollectionChanged != null)
                    CollectionChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }

    /// <summary>
    /// DaySlotBackground
    /// </summary>
    public class DaySlotBackground
    {
        #region Events

        /// <summary>
        /// Occurs when the DaySlotBackground collection changes
        /// </summary>
        public event EventHandler<EventArgs> CollectionChanged;

        #endregion

        #region Private variables

        private DayOfWeek _DayOfWeek;
        private DateTime _DateTime = DateTime.MinValue;

        private DaySlotAppearance _Appearance;
        private DaySlotBackgrounds _DaySlotBackgrounds;

        private OwnerKeyCollection _OwnerKeys;

        #endregion

        #region Constructors

        public DaySlotBackground(DateTime dateTime, DaySlotAppearance appearance)
        {
            _DateTime = dateTime;

            _Appearance = appearance;
        }

        public DaySlotBackground(DayOfWeek dayOfWeek, DaySlotAppearance appearance)
        {
            _DayOfWeek = dayOfWeek;
            _DateTime = DateTime.MinValue;

            _Appearance = appearance;
        }

        #endregion

        #region Public properties

        #region Appearance

        /// <summary>
        /// Gets or sets the Appearance
        /// </summary>
        public DaySlotAppearance Appearance
        {
            get { return (_Appearance); }

            set
            {
                _Appearance = value;

                OnCollectionChanged();
            }
        }

        #endregion

        #region DateTime

        /// <summary>
        /// Gets or sets the DateTime
        /// </summary>
        public DateTime DateTime
        {
            get { return (_DateTime); }

            set
            {
                if (_DateTime != value)
                {
                    _DateTime = value;

                    OnCollectionChanged();
                }
            }
        }

        #endregion

        #region DayOfWeek

        /// <summary>
        /// Gets or sets the DayOfWeek
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get { return (_DayOfWeek); }

            set
            {
                if (_DayOfWeek != value)
                {
                    _DayOfWeek = value;

                    OnCollectionChanged();
                }
            }
        }

        #endregion

        #region OwnerKeys

        /// <summary>
        /// Gets or sets the OwnerKeyCollection
        /// </summary>
        public OwnerKeyCollection OwnerKeys
        {
            get
            {
                if (_OwnerKeys == null)
                {
                    _OwnerKeys = new OwnerKeyCollection();

                    _OwnerKeys.CollectionChanged += OwnerKeys_CollectionChanged;
                }

                return (_OwnerKeys);
            }

            set
            {
                if (_OwnerKeys != null)
                    _OwnerKeys.CollectionChanged -= OwnerKeys_CollectionChanged;

                _OwnerKeys = value;

                if (_OwnerKeys != null)
                    _OwnerKeys.CollectionChanged += OwnerKeys_CollectionChanged;
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region DaySlotBackgrounds

        /// <summary>
        /// Gets or sets the DaySlotBackgrounds
        /// </summary>
        internal DaySlotBackgrounds DaySlotBackgrounds
        {
            get { return (_DaySlotBackgrounds); }

            set
            {
                _DaySlotBackgrounds = value;

                OnCollectionChanged();
            }
        }

        #endregion

        #region HasOwnerKeys

        /// <summary>
        /// HasOwnerKeys
        /// </summary>
        internal bool HasOwnerKeys
        {
            get { return (_OwnerKeys != null && _OwnerKeys.Count > 0); }
        }

        #endregion

        #endregion

        #region Event processing

        /// <summary>
        /// Processes OwnerKeys_CollectionChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OwnerKeys_CollectionChanged(object sender, EventArgs e)
        {
            OnCollectionChanged();
        } 

        #endregion

        #region OnCollectionChanged

        /// <summary>
        /// Handles collection change notification
        /// </summary>
        private void OnCollectionChanged()
        {
            if (CollectionChanged != null)
                CollectionChanged(this, EventArgs.Empty);
        }

        #endregion
    }

    /// <summary>
    /// DaySlotAppearance
    /// </summary>
    public class DaySlotAppearance
    {
        #region Private variables

        private WorkTime _StartTime;
        private WorkTime _EndTime;

        private Color _BackColor;
        private Color _HourBorderColor;
        private Color _HalfHourBorderColor;

        private string _Text;
        private ContentAlignment _TextAlignment;
        private Color _TextColor;
        private Color _SelectedTextColor;
        private Font _Font;

        private bool _OnTop;
        private bool _ShowTextWhenSelected = true;

        #endregion

        #region Constructors

        public DaySlotAppearance(WorkTime startTime, WorkTime endTime,
            Color backColor, Color hourBorderColor, Color halfHourBorderColor)
        {
            _StartTime = startTime;
            _EndTime = endTime;

            _BackColor = backColor;
            _HourBorderColor = hourBorderColor;
            _HalfHourBorderColor = halfHourBorderColor;
        }

        public DaySlotAppearance(int startHour, int startMinute,
            int endHour, int endMinute, Color backColor, Color hourBorderColor, Color halfHourBorderColor)
            : this(new WorkTime(startHour, startMinute), new WorkTime(endHour, endMinute), backColor, hourBorderColor, halfHourBorderColor)
        {
        }

        #endregion

        #region Public properties

        #region BackColor

        /// <summary>
        /// Gets or sets the BackColor
        /// </summary>
        public Color BackColor
        {
            get { return (_BackColor); }
            set { _BackColor = value; }
        }

        #endregion

        #region EndTime

        /// <summary>
        /// Gets or sets the Appearance end time
        /// </summary>
        public WorkTime EndTime
        {
            get { return (_EndTime); }
            set { _EndTime = value; }
        }

        #endregion

        #region Font

        /// <summary>
        /// Gets or sets the DaySlot Font
        /// </summary>
        public Font Font
        {
            get { return (_Font); }
            set { _Font = value; }
        }

        #endregion

        #region HalfHourBorderColor

        /// <summary>
        /// Gets or sets the HalfHourBorderColor
        /// </summary>
        public Color HalfHourBorderColor
        {
            get { return (_HalfHourBorderColor); }
            set { _HalfHourBorderColor = value; }
        }

        #endregion

        #region HourBorderColor

        /// <summary>
        /// Gets or sets the HourBorderColor
        /// </summary>
        public Color HourBorderColor
        {
            get { return (_HourBorderColor); }
            set { _HourBorderColor = value; }
        }

        #endregion

        #region OnTop

        /// <summary>
        /// Gets or sets whether the Text is on top of the borders
        /// </summary>
        public bool OnTop
        {
            get { return (_OnTop); }
            set { _OnTop = value; }
        }

        #endregion

        #region StartTime

        /// <summary>
        /// Gets or sets the Appearance start time
        /// </summary>
        public WorkTime StartTime
        {
            get { return (_StartTime); }
            set { _StartTime = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        public string Text
        {
            get { return (_Text); }
            set { _Text = value; }
        }

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets the Text Alignment
        /// </summary>
        public ContentAlignment TextAlignment
        {
            get { return (_TextAlignment); }
            set { _TextAlignment = value; }
        }

        #endregion

        #region TextColor

        /// <summary>
        /// Gets or sets the Text Color
        /// </summary>
        public Color TextColor
        {
            get { return (_TextColor); }
            set { _TextColor = value; }
        }

        #endregion

        #region SelectedTextColor

        /// <summary>
        /// Gets or sets the Selected Text Color
        /// </summary>
        public Color SelectedTextColor
        {
            get { return (_SelectedTextColor); }
            set { _SelectedTextColor = value; }
        }

        #endregion

        #region ShowTextWhenSelected

        /// <summary>
        /// Gets or sets wheter the Text is displayed when cells are selected
        /// </summary>
        public bool ShowTextWhenSelected
        {
            get { return (_ShowTextWhenSelected); }
            set { _ShowTextWhenSelected = value; }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// OwnerKeyCollection
    /// </summary>
    public class OwnerKeyCollection : Collection<string>
    {
        #region Events

        /// <summary>
        /// Occurs when the OwnerKeyCollection changes
        /// </summary>
        public event EventHandler<EventArgs> CollectionChanged;

        #endregion

        #region RemoveItem

        /// <summary>
        /// Processes list RemoveItem calls
        /// </summary>
        /// <param name="index">Index to remove</param>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            OnCollectionChanged();
        }

        #endregion

        #region InsertItem

        /// <summary>
        /// Processes list InsertItem calls
        /// </summary>
        /// <param name="index">Index to add</param>
        /// <param name="item">Text to add</param>
        protected override void InsertItem(int index, string item)
        {
            if (item != null)
            {
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
        /// <param name="newItem">Text to replace</param>
        protected override void SetItem(int index, string newItem)
        {
            base.SetItem(index, newItem);

            OnCollectionChanged();
        }

        #endregion

        #region ClearItems

        /// <summary>
        /// Processes list Clear calls (e.g. remove all)
        /// </summary>
        protected override void ClearItems()
        {
            if (Count > 0)
            {
                base.ClearItems();

                OnCollectionChanged();
            }
        }

        #endregion

        #region OnCollectionChanged

        /// <summary>
        /// Handles collection change notification
        /// </summary>
        private void OnCollectionChanged()
        {
            if (CollectionChanged != null)
                CollectionChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
