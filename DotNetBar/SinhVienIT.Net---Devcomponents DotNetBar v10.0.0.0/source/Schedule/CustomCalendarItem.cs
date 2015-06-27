#if FRAMEWORK20
using System;
using System.Windows.Forms;
using DevComponents.Schedule.Model;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Schedule
{
    public class CustomCalendarItem : CalendarItem
    {
        #region Events

        /// <summary>
        /// Occurs when the OwnerKey has changed
        /// </summary>
        [Description("Occurs when the OwnerKey has changed.")]
        public event EventHandler<EventArgs> OwnerKeyChanged;

        /// <summary>
        /// Occurs when Locked has changed
        /// </summary>
        [Description("Occurs when the Locked property has changed.")]
        public event EventHandler<EventArgs> LockedChanged;

        #endregion

        #region Private variables

        private string _OwnerKey = "";      // OwnerKey
        private bool _Locked;               // Locked state

        private CustomCalendarItem _BaseCalendarItem;       // Base CalendarItem
        private CalendarView _CalendarView;

        #endregion

        #region Public properties

        #region CollateId

        /// <summary>
        /// Gets or sets the CollateId used for TimeLine row collation.
        /// </summary>
        public override int CollateId
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.CollateId);

                return (base.CollateId);
            }

            set
            {
                if (_BaseCalendarItem != null)
                    _BaseCalendarItem.CollateId = value;

                base.CollateId = value;
            }
        }

        #endregion

        #region OwnerKey

        /// <summary>
        /// Gets and sets the item OwnerKey
        /// </summary>
        public string OwnerKey
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.OwnerKey);
                
                return (_OwnerKey);
            }

            set
            {
                if (_BaseCalendarItem != null)
                {
                    _BaseCalendarItem.OwnerKey = value;
                }
                else
                {
                    if (value == null)
                        value = "";

                    if (_OwnerKey.Equals(value) == false)
                    {
                        string oldValue = _OwnerKey;
                        _OwnerKey = value;

                        OnOwnerKeyChanged(oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Sends ChangedEvent for the OwnerKey property
        /// </summary>
        /// <param name="oldValue">Old OwnerKey</param>
        /// <param name="newValue">New OwnerKey</param>
        protected virtual void OnOwnerKeyChanged(string oldValue, string newValue)
        {
            if (OwnerKeyChanged != null)
                OwnerKeyChanged(this, new OwnerKeyChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region Locked

        /// <summary>
        /// Gets and set whether modification is enabled
        /// through the user interface"
        /// </summary>
        public bool Locked
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.Locked);

                return (_Locked);
            }

            set
            {
                if (_BaseCalendarItem != null)
                {
                    _BaseCalendarItem.Locked = value;
                }
                else
                {
                    if (_Locked != value)
                    {
                        bool oldValue = _Locked;
                        _Locked = value;

                        OnLockedChanged(oldValue, value);
                    }
                }
            }
        }

        /// <summary>
        /// Sends ChangedEvent for the Locked property
        /// </summary>
        /// <param name="oldValue">Old OwnerKey</param>
        /// <param name="newValue">New OwnerKey</param>
        protected virtual void OnLockedChanged(bool oldValue, bool newValue)
        {
            if (LockedChanged != null)
                LockedChanged(this, new LockedChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region StartTime

        public override DateTime StartTime
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.StartTime);

                return (base.StartTime);
            }

            set
            {
                if (_BaseCalendarItem != null)
                    _BaseCalendarItem.StartTime = value;

                base.StartTime = value;
            }
        }

        #endregion

        #region EndTime

        public override DateTime EndTime
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.EndTime);

                return (base.EndTime);
            }

            set
            {
                if (_BaseCalendarItem != null)
                    _BaseCalendarItem.EndTime = value;

                base.EndTime = value;
            }
        }

        #endregion

        #region BaseCalendarItem

        /// <summary>
        /// Base CalendarItem
        /// 
        /// This property holds the base CalendarItem from which
        /// each displayed CustomItem (of this type) is based.
        /// 
        /// In order to keep all displayed items "in-sync", it is necessary
        /// to propagate data to and from the base CalendarItem.  This is
        /// accomplished via hooking those members you are interested in, at
        /// both the item (HookEvents) and BaseCalendarItem (HookBaseEvents)
        /// level.
        /// 
        /// </summary>
        public virtual CustomCalendarItem BaseCalendarItem
        {
            get { return (_BaseCalendarItem); }
            set { _BaseCalendarItem = value; }
        }

        #endregion

        #region IsMultiDayOrAllDayEvent

        public bool IsMultiDayOrAllDayEvent
        {
            get
            {
                return (StartTime < EndTime &&
                    (EndTime.Subtract(StartTime).TotalDays >= 1 ||
                    DateTimeHelper.IsSameDay(StartTime, EndTime) == false));
            }
        }

        #endregion

        #region CategoryColor

        /// <summary>
        /// Gets or sets the category color used for TimeLine CondensedView markers.
        /// Use static members on Appointment class to assign the category color for example Appointment.CategoryRed.
        /// </summary>
        public override string CategoryColor
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.CategoryColor);

                return (base.CategoryColor);
            }

            set
            {
                if (_BaseCalendarItem != null)
                    _BaseCalendarItem.CategoryColor = value;

                base.CategoryColor = value;
            }
        }

        #endregion

        #region IsSelected

        /// <summary>
        /// Gets or sets whether the item is selected.
        /// </summary>
        public override bool IsSelected
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.IsSelected);

                return (base.IsSelected);
            }

            set
            {
                if (_BaseCalendarItem != null)
                    _BaseCalendarItem.IsSelected = value;

                base.IsSelected = value;
            }
        }

        #endregion

        #region Visible

        /// <summary>
        /// Gets and sets the item Visibility
        /// </summary>
        public override bool Visible
        {
            get
            {
                if (_BaseCalendarItem != null)
                    return (_BaseCalendarItem.Visible);

                return (base.Visible);
            }

            set
            {
                if (_BaseCalendarItem != null)
                {
                    _BaseCalendarItem.Visible = value;
                }
                else
                {
                    if (_BaseCalendarItem != null)
                        _BaseCalendarItem.Visible = value;

                    base.Visible = value;
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region CalendarView

        internal CalendarView CalendarView
        {
            get { return (_CalendarView); }
            set { _CalendarView = value; }
        }

        #endregion

        #endregion

        #region Refresh

        public override void Refresh()
        {
            if (_CalendarView != null)
            {
                Control c = (Control)_CalendarView.GetContainerControl();

                if (c != null)
                {
                    foreach (BaseItem item in _CalendarView.CalendarPanel.SubItems)
                    {
                        BaseView view = item as BaseView;

                        if (view != null)
                            RefreshItems(c, view.SubItems);
                    }
                }
            }
        }

        private void RefreshItems(Control c, SubItemsCollection subItems)
        {
            foreach (BaseItem item in subItems)
            {
                CustomCalendarItem ci = item as CustomCalendarItem;

                if (ci != null)
                {
                    if (ci == this || ci.BaseCalendarItem == this)
                        ci.Invalidate(c);
                }
                else if (item is AllDayPanel)
                {
                    RefreshItems(c, item.SubItems);
                }
            }
        }
    
        #endregion

        #region Paint

        public override void Paint(ItemPaintArgs e)
        {
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            CustomCalendarItem objCopy = new CustomCalendarItem();
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the CustomCalendarItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New CustomCalendarItem instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            CustomCalendarItem objCopy = copy as CustomCalendarItem;

            if (objCopy != null)
            {
                base.CopyToItem(objCopy);

                objCopy._OwnerKey = this._OwnerKey;
                objCopy._Locked = this.Locked;
                objCopy._CalendarView = this.CalendarView;
            }
        }

        #endregion
    }

    #region OwnerKeyChangedEventArgs

    /// <summary>
    /// OwnerKeyChangedEventArgs
    /// </summary>
    /// 
    public class OwnerKeyChangedEventArgs : ValueChangedEventArgs<string, string>
    {
        public OwnerKeyChangedEventArgs(string oldValue, string newValue)
            : base(oldValue, newValue)
        {
        }
    }

    #endregion

    #region LockedChangedEventArgs

    /// <summary>
    /// LockedChangedEventArgs
    /// </summary>
    /// 
    public class LockedChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public LockedChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }

    #endregion

    #region CategoryColorChangedEventArgs

    /// <summary>
    /// CategoryColorChangedEventArgs
    /// </summary>
    /// 
    public class CategoryColorChangedEventArgs : ValueChangedEventArgs<string, string>
    {
        public CategoryColorChangedEventArgs(string oldValue, string newValue)
            : base(oldValue, newValue)
        {
        }
    }

    #endregion

}
#endif