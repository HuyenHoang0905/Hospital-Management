#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar.Schedule
{
    public class CalendarItem : PopupItem
    {
        #region enums

        public enum eHitArea
        {
            None,
            Move,
            LeftResize,
            RightResize,
            TopResize,
            BottomResize
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when CategoryColor has changed
        /// </summary>
        [Description("Occurs when the CategoryColor property has changed.")]
        public event EventHandler<EventArgs> CategoryColorChanged;

        /// <summary>
        /// Occurs when StartTime has Changed
        /// </summary>
        public event EventHandler StartTimeChanged;

        /// <summary>
        /// Occurs when EndTime has Changed
        /// </summary>
        public event EventHandler EndTimeChanged;

        /// <summary>
        /// Occurs when IsSelected has Changed
        /// </summary>
        public event EventHandler IsSelectedChanged;

        /// <summary>
        /// Occurs when CollateId has Changed
        /// </summary>
        public event EventHandler CollateIdChanged;

        #endregion

        #region Private variables

        private DateTime _StartTime;        // Start time
        private DateTime _EndTime;          // End time
        private string _CategoryColor;      // CategoryColor

        private int _CollateId = -1;

        private object _ModelItem;          // Model item
        private object _RootItem;           // Root model item
        private bool _IsRecurring;          // Is recurring
        private bool _IsSelected;           // Is selected

        private eHitArea _HitArea = eHitArea.None;
        private DateTime _LastMouseDown = DateTime.MinValue;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CalendarItem()
        {
            // Set our default Model item

            _ModelItem = this;

            MouseUpNotification = true;

            SetIsContainer(true);
        }

        #region Public properties

        #region CollateId

        /// <summary>
        /// Gets or sets the CollateId used for TimeLine row collation.
        /// </summary>
        public virtual int CollateId
        {
            get { return (_CollateId); }

            set
            {
                if (_CollateId != value)
                {
                    _CollateId = value;

                    OnCollateIdChanged();
                }
            }
        }

        protected virtual void OnCollateIdChanged()
        {
            if (CollateIdChanged != null)
                CollateIdChanged(this, new EventArgs());

            this.Refresh();
        }

        #endregion

        #region StartTime

        /// <summary>
        /// Gets or sets the CalendarItem Start time.
        /// </summary>
        public virtual DateTime StartTime
        {
            get { return (_StartTime); }

            set
            {
                if (_StartTime != value)
                {
                    _StartTime = value;

                    OnStartTimeChanged();
                }
            }
        }

        protected virtual void OnStartTimeChanged()
        {
            if (StartTimeChanged != null)
                StartTimeChanged(this, new EventArgs());

            this.Refresh();
        }

        #endregion

        #region EndTime

        /// <summary>
        /// Gets or sets the CalendarItem End time.
        /// </summary>
        public virtual DateTime EndTime
        {
            get { return (_EndTime); }

            set
            {
                if (_EndTime != value)
                {
                    _EndTime = value;

                    OnEndTimeChanged();
                }
            }
        }

        protected virtual void OnEndTimeChanged()
        {
            if (EndTimeChanged != null)
                EndTimeChanged(this, new EventArgs());

            this.Refresh();
        }

        #endregion

        #region CategoryColor

        /// <summary>
        /// Gets or sets the category color used for TimeLine CondensedView markers.
        /// Use static members on Appointment class to assign the category color for example Appointment.CategoryRed.
        /// </summary>
        public virtual string CategoryColor
        {
            get { return (_CategoryColor); }

            set
            {
                if (_CategoryColor == null || _CategoryColor.Equals(value) == false)
                {
                    string oldValue = _CategoryColor;
                    _CategoryColor = value;

                    OnCategoryColorChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Sends ChangedEvent for the CategoryColor property
        /// </summary>
        /// <param name="oldValue">Old CategoryColor</param>
        /// <param name="newValue">New CategoryColor</param>
        protected virtual void OnCategoryColorChanged(string oldValue, string newValue)
        {
            if (CategoryColorChanged != null)
                CategoryColorChanged(this, new CategoryColorChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region ModelItem

        /// <summary>
        /// Gets and sets the Model item
        /// </summary>
        public object ModelItem
        {
            get { return (_ModelItem); }
            set { _ModelItem = value; }
        }

        #endregion

        #region RootItem

        /// <summary>
        /// Gets and sets the Root Model item
        /// </summary>
        public object RootItem
        {
            get { return (_RootItem); }
            set { _RootItem = value; }
        }

        #endregion

        #region IsRecurring

        /// <summary>
        /// Gets and sets the IsRecurring item status
        /// </summary>
        public bool IsRecurring
        {
            get { return (_IsRecurring); }
            set { _IsRecurring = value; }
        }

        #endregion

        #region IsSelected

        /// <summary>
        /// Gets and sets the selection state
        /// </summary>
        public virtual bool IsSelected
        {
            get { return (_IsSelected); }

            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;

                    Control c = this.ContainerControl as Control;

                    if (c != null)
                    {
                        Rectangle r = Bounds;
                        r.Inflate(3, 3);

                        c.Invalidate(r);
                    }

                    OnIsSelectedChanged();
                }
            }
        }

        private void OnIsSelectedChanged()
        {
            if (IsSelectedChanged != null)
                IsSelectedChanged(this, new EventArgs());
        }

        #endregion

        #region HitArea

        /// <summary>
        /// Gets and sets the last hit area
        /// </summary>
        public eHitArea HitArea
        {
            get { return (_HitArea); }
            set { _HitArea = value; }
        }

        #endregion

        #endregion

        #region Mouse support

        /// <summary>
        /// Handles control MouseDown events
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            base.InternalMouseDown(objArg);

            if (objArg.Button == MouseButtons.Left)
            {
                if (IsSimpleMouseDown())
                    this.IsSelected = true;

                InternalMouseMove(objArg);

                _LastMouseDown = DateTime.Now;
            }
        }

        /// <summary>
        /// Determines if it is a simple, single-click MouseDown
        /// </summary>
        /// <returns></returns>
        private bool IsSimpleMouseDown()
        {
            return (_LastMouseDown == DateTime.MinValue ||
                    _LastMouseDown != DateTime.MinValue &&
                    DateTime.Now.Subtract(_LastMouseDown).TotalMilliseconds > SystemInformation.DoubleClickTime);
        }

        #endregion

        #region RecalcSize

        public override void RecalcSize()
        {
        }

        #endregion

        #region Paint

        public override void Paint(ItemPaintArgs p)
        {
        }

        #endregion

        #region IsMarkupSupported

        /// <summary>
        /// IsMarkupSupported
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return (true); }
        }

        #endregion

        #region Copy object

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            CalendarItem objCopy = new CalendarItem();
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the CalendarItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New CalendarItem instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            CalendarItem objCopy = copy as CalendarItem;

            if (objCopy != null)
            {
                base.CopyToItem(objCopy);

                objCopy._StartTime = this._StartTime;
                objCopy._EndTime = this._EndTime;
            }
        }

        #endregion
    }
}
#endif

