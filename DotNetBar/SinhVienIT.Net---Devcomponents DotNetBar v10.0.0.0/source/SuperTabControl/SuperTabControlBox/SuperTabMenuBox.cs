#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    public class SuperTabMenuBox : PopupItem
    {
        #region Private variables

        private SuperTabControlBox _ControlBox;

        private Size _ItemSize = new Size(16, 16);

        private bool _IsMouseOver;
        private bool _IsMouseDown;

        private bool _AutoHide;
        private bool _ShowTabsOnly = true;
        private bool _ShowImages = true;
        private bool _RaiseClickOnSelection;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controlBox">Associated SuperTabControlBox</param>
        public SuperTabMenuBox(SuperTabControlBox controlBox)
        {
            _ControlBox = controlBox;

            Style = eDotNetBarStyle.Office2007;
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

        #region AutoHide

        /// <summary>
        /// Gets or sets whether the MenuBox is automatically hidden when the tab items size does not exceed the size of the control
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether the MenuBox is automatically hidden when the tab items size does not exceed the size of the control.")]
        public bool AutoHide
        {
            get { return (_AutoHide); }

            set
            {
                if (_AutoHide != value)
                {
                    _AutoHide = value;

                    Visible = (value == true) ? AllItemsVisible() == false : true;

                    MyRefresh();
                }
            }
        }

        #endregion

        #region RaiseClickOnSelection

        /// <summary>
        /// Gets or sets whether the MenuBox raises a ClickEvent when selected
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether the MenuBox raises a ClickEvent when selected.")]
        public bool RaiseClickOnSelection
        {
            get { return (_RaiseClickOnSelection); }
            set { _RaiseClickOnSelection = value; }
        }

        #endregion

        #region ShowTabsOnly

        /// <summary>
        /// Gets or sets whether MenuBox shows only Tabs entries
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates whether MenuBox shows only Tabs entries.")]
        public bool ShowTabsOnly
        {
            get { return (_ShowTabsOnly); }
            set { _ShowTabsOnly = value; }
        }

        #endregion

        #region ShowImages

        /// <summary>
        /// Gets or sets whether the MenuBox displays each menu entry with its associated Image/Icon
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates whether the MenuBox displays each menu entry with its associated Image/Icon.")]
        public bool ShowImages
        {
            get { return (_ShowImages); }
            set { _ShowImages = value; }
        }

        #endregion

        #region Visible

        /// <summary>
        /// Gets or sets MenuBox Visible state
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates MenuBox Visible state.")]
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

        #endregion

        #region RecalcSize

        /// <summary>
        /// RecalcSize
        /// </summary>
        public override void RecalcSize()
        {
            base.RecalcSize();

            WidthInternal = _ItemSize.Width;
            HeightInternal = _ItemSize.Height;
        }

        #endregion

        #region MyRefresh

        /// <summary>
        /// Refreshes the display
        /// </summary>
        private void MyRefresh()
        {
            if (_ControlBox.TabDisplay != null)
            {
                SuperTabStripItem tsi = _ControlBox.TabDisplay.TabStripItem;

                if (tsi != null)
                {
                    _ControlBox.NeedRecalcSize = true;
                    _ControlBox.RecalcSize();

                    tsi.NeedRecalcSize = true;
                    tsi.Refresh();
                }
            }
        }

        #endregion

        #region Paint

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="p"></param>
        public override void Paint(ItemPaintArgs p)
        {
            Graphics g = p.Graphics;

            SuperTabColorTable sct = _ControlBox.TabDisplay.GetColorTable();
            Color imageColor = sct.ControlBoxDefault.Image;

            if (IsMouseOver == true)
            {
                Rectangle r = Bounds;

                r.Width--;
                r.Height--;

                imageColor = sct.ControlBoxMouseOver.Image;

                using (Brush br = new SolidBrush(sct.ControlBoxMouseOver.Background))
                    g.FillRectangle(br, r);

                using (Pen pen = new Pen(sct.ControlBoxMouseOver.Border))
                    g.DrawRectangle(pen, r);
            }

            if (AllItemsVisible() == true)
                g.DrawImageUnscaled(GetMenuButton1(g, imageColor), Bounds);
            else
                g.DrawImageUnscaled(GetMenuButton2(g, imageColor), Bounds);
        }

        #region AllItemsVisible

        /// <summary>
        /// Determines if all the items are visible
        /// </summary>
        /// <returns>true if all visible</returns>
        private bool AllItemsVisible()
        {
            SubItemsCollection tabs = _ControlBox.TabDisplay.Tabs;

            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].Visible == true && tabs[i].Displayed == false)
                    return (false);
            }

            return (true);
        }

        #endregion

        #region GetMenuButton1

        /// <summary>
        /// Gets MenuButton1
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Bitmap GetMenuButton1(Graphics g, Color color)
        {
            return (_ControlBox.TabDisplay.GetMenuButton1(g, color));
        }

        #endregion

        #region GetMenuButton2

        /// <summary>
        /// Gets MenuButton2
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Bitmap GetMenuButton2(Graphics g, Color color)
        {
            return (_ControlBox.TabDisplay.GetMenuButton2(g, color));
        }

        #endregion

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

            Refresh();
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

            Refresh();
        }

        #endregion

        #region InternalMouseDown

        /// <summary>
        /// InternalMouseDown
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            _IsMouseDown = true;

            if (DesignMode == false && objArg.Button == MouseButtons.Left)
            {
                LoadTabMenu();

                Point pt = GetPopupPosition();

                Popup(pt);
            }
        }

        #region GetPopupPosition

        /// <summary>
        /// Gets the popup menu position
        /// </summary>
        /// <returns></returns>
        private Point GetPopupPosition()
        {
            Point pt = new Point(Bounds.X, Bounds.Bottom);

            Control c = (Control)_ControlBox.GetContainerControl(true);

            if (c != null)
                pt = c.PointToScreen(pt);

            return (pt);
        }

        #endregion

        #region LoadTabMenu

        /// <summary>
        /// Loads the TabMenu
        /// </summary>
        private void LoadTabMenu()
        {
            SubItems.Clear();

            SubItemsCollection tabs = _ControlBox.TabDisplay.Tabs;

            if (_ShowTabsOnly == true)
                LoadTabsMenu(tabs);
            else
                LoadItemsMenu(tabs);
        }

        #region LoadTabsMenu

        /// <summary>
        /// Loads Tabs only into the TabMenu
        /// </summary>
        /// <param name="tabs"></param>
        private void LoadTabsMenu(SubItemsCollection tabs)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                SuperTabItem tab = tabs[i] as SuperTabItem;

                if (tab != null && tab.Visible == true)
                {
                    ButtonItem bi = new ButtonItem();

                    bi.Tag = tab;
                    bi.Text = tab.Text;
                    bi.Click += SuperTabMenuBoxClick;

                    SubItems.Add(bi);

                    if (_ShowImages == true)
                        SetMenuImage(bi, tab.GetTabImage());

                    if (tab.IsSelected == true)
                        bi.Checked = true;
                }
            }
        }

        #endregion

        #region LoadItemsMenu

        /// <summary>
        /// Loads all items into the TabMenu
        /// </summary>
        /// <param name="tabs"></param>
        private void LoadItemsMenu(SubItemsCollection tabs)
        {
            ButtonItem[] items = new ButtonItem[tabs.Count];

            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].Visible == true)
                {
                    items[i] = new ButtonItem();

                    items[i].Tag = tabs[i];
                    items[i].Text = tabs[i].Text;
                    items[i].Click += SuperTabMenuBoxClick;

                    if (_ShowImages == true)
                    {
                        if (tabs[i] is SuperTabItem)
                            SetMenuImage(items[i], ((SuperTabItem) tabs[i]).GetTabImage());

                        else if (tabs[i] is ButtonItem)
                            SetMenuImage(items[i], ((ButtonItem) tabs[i]).GetImage());
                    }
                }
            }

            int index = _ControlBox.TabDisplay.TabStripItem.SelectedTabIndex;

            if (index >= 0 && index < tabs.Count)
                items[index].Checked = true;

            SubItems.AddRange(items);
        }

        #endregion

        #region SetMenuImage

        /// <summary>
        /// Sets the TabMenu entry image
        /// </summary>
        /// <param name="bi"></param>
        /// <param name="image"></param>
        private void SetMenuImage(ButtonItem bi, CompositeImage image)
        {
            if (image != null)
            {
                if (image.Icon != null)
                    bi.Icon = image.Icon;

                else if (image.Image != null)
                {
                    if (ImageAnimator.CanAnimate(image.Image) == true)
                    {
                        FrameDimension frameDimensions =
                            new FrameDimension(image.Image.FrameDimensionsList[0]);

                        image.Image.SelectActiveFrame(frameDimensions, 0);

                        bi.Image = new Bitmap(image.Image);
                        bi.NeedRecalcSize = true;
                    }
                    else
                    {
                        bi.Image = image.Image;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region SuperTabMenuBox_Click

        /// <summary>
        /// Handles SuperTabMenuBox_Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SuperTabMenuBoxClick(object sender, EventArgs e)
        {
            ButtonItem bi = sender as ButtonItem;

            if (bi != null)
            {
                BaseItem item = bi.Tag as BaseItem;

                if (item != null)
                {
                    SuperTabItem tab = bi.Tag as SuperTabItem;

                    if (tab != null)
                        _ControlBox.TabDisplay.TabStripItem.SelectedTab = tab;
                    else
                        _ControlBox.TabDisplay.TabStripItem.VisibleTab = item;

                    if (_RaiseClickOnSelection == true)
                        item.RaiseClick(eEventSource.Code);
                }
            }
        }

        #endregion

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

            Refresh();
        }

        #endregion

        #endregion

        #region Copy object support

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            SuperTabMenuBox objCopy = new SuperTabMenuBox(_ControlBox);
            CopyToItem(objCopy);

            return (objCopy);
        }

        protected override void CopyToItem(BaseItem copy)
        {
            SuperTabMenuBox objCopy = copy as SuperTabMenuBox;
            base.CopyToItem(objCopy);
        }

        #endregion
    }

    #region enums

    #region TabMenuOpenEventArgs

    public class TabMenuOpenEventArgs : CancelEventArgs
    {
        #region Private variables

        private ButtonItem _TabMenu;

        #endregion

        public TabMenuOpenEventArgs(ButtonItem tabMenu)
        {
            _TabMenu = tabMenu;
        }

        #region Public properties

        public ButtonItem TabMenu
        {
            get { return (_TabMenu); }
        }

        #endregion
    }

    #endregion

    #region TabMenuCloseEventArgs

    public class TabMenuCloseEventArgs : EventArgs
    {
        #region Private variables

        private ButtonItem _TabMenu;

        #endregion

        public TabMenuCloseEventArgs(ButtonItem tabMenu)
        {
            _TabMenu = tabMenu;
        }

        #region Public properties

        public ButtonItem TabMenu
        {
            get { return (_TabMenu); }
        }

        #endregion
    }

    #endregion

    #endregion
}
#endif