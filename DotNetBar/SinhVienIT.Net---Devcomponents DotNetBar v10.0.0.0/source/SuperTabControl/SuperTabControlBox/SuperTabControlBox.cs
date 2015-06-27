#if FRAMEWORK20
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    public class SuperTabControlBox : BaseItem
    {
        #region Private variables

        private SuperTabStripItem _TabStripItem;

        private SuperTabCloseBox _CloseBox;
        private SuperTabMenuBox _MenuBox;

        private BaseItem _HotItem;
        private BaseItem _MouseDownItem;

        private bool _IsMouseOver;
        private bool _IsMouseDown;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabStripItem">Associated SuperTabStripItem</param>
        public SuperTabControlBox(SuperTabStripItem tabStripItem)
        {
            _TabStripItem = tabStripItem;

            MouseDownCapture = true;
            MouseUpNotification = true;

            Style = eDotNetBarStyle.Office2007;

            _MenuBox = new SuperTabMenuBox(this);
            _CloseBox = new SuperTabCloseBox(this);

            _MenuBox.Visible = true;
            _CloseBox.Visible = false;

            SetParent(tabStripItem);
            SetIsContainer(true);

            SubItems.Add(_MenuBox);
            SubItems.Add(_CloseBox);
        }

        #region Public override designer hiding

        [Browsable(false)]
        public override bool BeginGroup
        {
            get { return base.BeginGroup; }
            set { base.BeginGroup = value; }
        }

        [Browsable(false)]
        public override bool AutoCollapseOnClick
        {
            get { return base.AutoCollapseOnClick; }
            set { base.AutoCollapseOnClick = value; }
        }

        [Browsable(false)]
        public override bool CanCustomize
        {
            get { return base.CanCustomize; }
            set { base.CanCustomize = value; }
        }

        [Browsable(false)]
        public override string Category
        {
            get { return base.Category; }
            set { base.Category = value; }
        }

        [Browsable(false)]
        public override bool ClickAutoRepeat
        {
            get { return base.ClickAutoRepeat; }
            set { base.ClickAutoRepeat = value; }
        }

        [Browsable(false)]
        public override int ClickRepeatInterval
        {
            get { return base.ClickRepeatInterval; }
            set { base.ClickRepeatInterval = value; }
        }

        [Browsable(false)]
        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }

        [Browsable(false)]
        public override bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        [Browsable(false)]
        public override bool Stretch
        {
            get { return base.Stretch; }
            set { base.Stretch = value; }
        }

        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(false)]
        public override bool ThemeAware
        {
            get { return base.ThemeAware; }
            set { base.ThemeAware = value; }
        }

        [Browsable(false)]
        public override string Tooltip
        {
            get { return base.Tooltip; }
            set { base.Tooltip = value; }
        }

        [Browsable(false)]
        public override eItemAlignment ItemAlignment
        {
            get { return base.ItemAlignment; }
            set { base.ItemAlignment = value; }
        }

        [Browsable(false)]
        public override string KeyTips
        {
            get { return base.KeyTips; }
            set { base.KeyTips = value; }
        }

        [Browsable(false)]
        public override ShortcutsCollection Shortcuts
        {
            get { return base.Shortcuts; }
            set { base.Shortcuts = value; }
        }

        [Browsable(false)]
        public override string Description
        {
            get { return base.Description; }
            set { base.Description = value; }
        }

        [Browsable(false)]
        public override bool GlobalItem
        {
            get { return base.GlobalItem; }
            set { base.GlobalItem = value; }
        }

        [Browsable(false)]
        public override bool ShowSubItems
        {
            get { return base.ShowSubItems; }
            set { base.ShowSubItems = value; }
        }

        [Browsable(false)]
        public override string GlobalName
        {
            get { return base.GlobalName; }
            set { base.GlobalName = value; }
        }

        [Browsable(false)]
        public override Command Command
        {
            get { return base.Command; }
            set { base.Command = value; }
        }

        [Browsable(false)]
        public override object CommandParameter
        {
            get { return base.CommandParameter; }
            set { base.CommandParameter = value; }
        }

        #endregion

        #region Public properties

        #region Bounds

        /// <summary>
        /// Gets or sets the ControlBox Bounds
        /// </summary>
        [Browsable(false)]
        public override Rectangle Bounds
        {
            get { return (base.Bounds); }

            set
            {
                base.Bounds = value;

                RecalcLayout();
            }
        }

        #endregion

        #region CloseBox

        /// <summary>
        /// Gets the CloseBox
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SuperTabCloseBox CloseBox
        {
            get { return (_CloseBox); }
        }

        #endregion

        #region MenuBox

        /// <summary>
        /// Gets the MenuBox
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SuperTabMenuBox MenuBox
        {
            get { return (_MenuBox); }
        }

        #endregion

        #region Visible

        /// <summary>
        /// Gets or sets the ControlBox Visible state
        /// </summary>
        public override bool Visible
        {
            get { return base.Visible; }

            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;

                    MyRefresh();
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region IsMouseDown

        /// <summary>
        /// Gets the MouseDown state
        /// </summary>
        internal bool IsMouseDown
        {
            get { return (_IsMouseDown); }
        }

        #endregion

        #region IsMouseOver

        /// <summary>
        /// Gets the MouseOver state
        /// </summary>
        internal bool IsMouseOver
        {
            get { return (_IsMouseOver); }
        }

        #endregion

        #region TabDisplay

        /// <summary>
        /// Gets the TabStrip TabDisplay
        /// </summary>
        internal SuperTabStripBaseDisplay TabDisplay
        {
            get { return (_TabStripItem.TabDisplay); }
        }

        #endregion

        #endregion

        #region RecalcSize

        /// <summary>
        /// Performs RecalcSize processing
        /// </summary>
        public override void RecalcSize()
        {
            Size size = Size.Empty;

            foreach (BaseItem item in SubItems)
                size = LayoutItem(item, size, true);

            WidthInternal = size.Width;
            HeightInternal = size.Height;
        }

        #endregion

        #region RecalcLayout

        /// <summary>
        /// Performs RecalcLayout processing
        /// </summary>
        private void RecalcLayout()
        {
            Size size = Size.Empty;

            foreach (BaseItem item in SubItems)
                size = LayoutItem(item, size, false);

            Refresh();
        }

        #endregion

        #region LayoutItem

        /// <summary>
        /// Performs individual running layout processing for the given BaseItem
        /// </summary>
        /// <param name="item">Item to Layout</param>
        /// <param name="size">Running Layout Size</param>
        /// <param name="recalc">Whether a recalcSize is needed</param>
        /// <returns>New running Size</returns>
        private Size LayoutItem(BaseItem item, Size size, bool recalc)
        {
            if (item.Visible == true)
            {
                // Recalc if requested

                if (recalc == true)
                    item.RecalcSize();

                // If this is the first item, then give
                // us a little padding around the ControlBox

                if (size.IsEmpty == true)
                {
                    if (Orientation == eOrientation.Horizontal)
                        size.Width += 5;
                    else
                        size.Height += 5;
                }

                // Get the item Bounds

                Rectangle r = new Rectangle(Bounds.X, Bounds.Y, item.WidthInternal, item.HeightInternal);

                // Set the item location

                if (_TabStripItem.IsVertical == true)
                {
                    r.Y += size.Height;
                    r.X += (Bounds.Width - r.Width) / 2;
                }
                else
                {
                    r.X += size.Width;
                    r.Y += (Bounds.Height - r.Height) / 2;
                }

                // Set the Bounds

                item.Bounds = r;

                // Set the item Size

                if (_TabStripItem.IsVertical == true)
                {
                    size.Height += item.HeightInternal + 2;

                    if (item.WidthInternal > size.Width)
                        size.Width = item.WidthInternal;
                }
                else
                {
                    size.Width += item.WidthInternal + 2;

                    if (item.HeightInternal > size.Height)
                        size.Height = item.HeightInternal;
                }

                // Set its display status

                if (DesignMode == false)
                    item.Displayed = true;
            }

            return (size);
        }

        #endregion

        #region MyRefresh

        /// <summary>
        /// Refresh code
        /// </summary>
        private void MyRefresh()
        {
            if (TabDisplay != null)
            {
                SuperTabStripItem tsi = TabDisplay.TabStripItem;

                if (tsi != null)
                {
                    tsi.NeedRecalcSize = true;
                    tsi.Refresh();
                }
            }
        }

        #endregion

        #region Paint

        /// <summary>
        /// Paint procesing
        /// </summary>
        /// <param name="p"></param>
        public override void Paint(ItemPaintArgs p)
        {
            foreach (BaseItem item in SubItems)
            {
                if (item.Visible == true)
                    item.Paint(p);
            }
        }

        #endregion

        #region Mouse support

        #region InternalMouseEnter

        /// <summary>
        /// InternalMouseEnter
        /// </summary>
        public override void InternalMouseEnter()
        {
            base.InternalMouseEnter();

            _IsMouseOver = true;
        }

        #endregion

        #region InternalMouseLeave

        /// <summary>
        /// InternalMouseLeave
        /// </summary>
        public override void InternalMouseLeave()
        {
            base.InternalMouseLeave();

            _IsMouseOver = false;

            if (_HotItem != null)
            {
                _HotItem.InternalMouseLeave();

                _HotItem = null;
            }
        }

        #endregion        

        #region InternalMouseMove

        /// <summary>
        /// InternalMouseMove
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            base.InternalMouseMove(objArg);

            _HotItem = GetBoxItemFromPoint(objArg.Location);
        }

        #endregion

        #region InternalMouseDown

        /// <summary>
        /// InternalMouseDown
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            base.InternalMouseDown(objArg);

            _IsMouseDown = true;
            _MouseDownItem = _HotItem;
        }

        #endregion

        #region InternalMouseUp

        /// <summary>
        /// InternalMouseUp
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            base.InternalMouseUp(objArg);

            _IsMouseDown = false;

            if (_MouseDownItem != null)
            {
                if (_MouseDownItem != _HotItem)
                    _MouseDownItem.InternalMouseUp(objArg);

                _MouseDownItem = null;
            }
        }

        #endregion

        #endregion

        #region GetBoxItemFromPoint

        /// <summary>
        /// Gets the ControlBoxItem from the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private BaseItem GetBoxItemFromPoint(Point pt)
        {
            foreach (BaseItem item in SubItems)
            {
                if (item.Visible == true)
                {
                    if (item.Bounds.Contains(pt))
                        return (item);
                }
            }

            return (null);
        }

        #endregion

        #region OnItemAdded

        /// <summary>
        /// OnItemAdded
        /// </summary>
        /// <param name="item"></param>
        protected internal override void OnItemAdded(BaseItem item)
        {
            base.OnItemAdded(item);

            MyRefresh();
        }

        #endregion

        #region OnAfterItemRemoved

        /// <summary>
        /// OnAfterItemRemoved
        /// </summary>
        /// <param name="item"></param>
        protected internal override void OnAfterItemRemoved(BaseItem item)
        {
            base.OnAfterItemRemoved(item);

            MyRefresh();
        }

        #endregion

        #region RemoveUserItems

        /// <summary>
        /// Removes all user added items from the ControlBox
        /// </summary>
        public void RemoveUserItems()
        {
            for (int i = SubItems.Count - 1; i >= 0; i--)
            {
                if (SubItems[i] != _MenuBox && SubItems[i] != _CloseBox)
                    SubItems._RemoveAt(i);
            }

            MyRefresh();
        }

        #endregion

        #region Copy object support

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            SuperTabControlBox objCopy = new SuperTabControlBox(_TabStripItem);
            CopyToItem(objCopy);

            return (objCopy);
        }

        protected override void CopyToItem(BaseItem copy)
        {
            SuperTabControlBox objCopy = copy as SuperTabControlBox;
            base.CopyToItem(objCopy);
        }

        #endregion
    }
}
#endif