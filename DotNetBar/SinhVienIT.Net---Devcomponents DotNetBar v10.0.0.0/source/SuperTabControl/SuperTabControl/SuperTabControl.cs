#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    [ToolboxItem(true)]
    [DefaultEvent("SelectedTabChanged")]
    [Designer("DevComponents.DotNetBar.Design.SuperTabControlDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SuperTabControl : Control, ISupportInitialize
    {
        #region Events

        /// <summary>
        /// Occurs when a tab is added to the Tabs collection
        /// </summary>
        [Description("Occurs when a tab is added to the Tabs collection.")]
        public event EventHandler<SuperTabStripTabItemOpenEventArgs> TabItemOpen;

        /// <summary>
        /// Occurs when a tab is about to Close
        /// </summary>
        [Description("Occurs when a tab is about to Close.")]
        public event EventHandler<SuperTabStripTabItemCloseEventArgs> TabItemClose;

        /// <summary>
        /// Occurs when a tab is removed from the Tabs collection
        /// </summary>
        [Description("Occurs when a tab is removed from the Tabs collection.")]
        public event EventHandler<SuperTabStripTabRemovedEventArgs> TabRemoved;
        
        /// <summary>
        /// Occurs when a tab is about to be displayed
        /// </summary>
        [Description("Occurs when a tab is about to be displayed.")]
        public event EventHandler<SuperTabStripBeforeTabDisplayEventArgs> BeforeTabDisplay;

        /// <summary>
        /// Occurs when a tab is being moved or dragged by the user
        /// </summary>
        [Description("Occurs when a tab is being moved or dragged by the user.")]
        public event EventHandler<SuperTabStripTabMovingEventArgs> TabMoving;

        /// <summary>
        /// Occurs when a tab has been moved or dragged by the user
        /// </summary>
        [Description("Occurs when a tab has been moved or dragged by the user.")]
        public event EventHandler<SuperTabStripTabMovedEventArgs> TabMoved;

        /// <summary>
        /// Occurs when the Selected tab is changing
        /// </summary>
        [Description("Occurs when the Selected tab is changing.")]
        public event EventHandler<SuperTabStripSelectedTabChangingEventArgs> SelectedTabChanging;

        /// <summary>
        /// Occurs when the Selected tab has changed
        /// </summary>
        [Description("Occurs when the Selected tab has changed.")]
        public event EventHandler<SuperTabStripSelectedTabChangedEventArgs> SelectedTabChanged;

        /// <summary>
        /// Occurs when the control needs a tab's bordering path
        /// </summary>
        [Description("Occurs when the control needs a tab's bordering path.")]
        public event EventHandler<SuperTabGetTabItemPathEventArgs> GetTabItemPath;

        /// <summary>
        /// Occurs when the control needs a tab's Content Rectangle
        /// </summary>
        [Description("Occurs when the control needs a tab's Content Rectangle.")]
        public event EventHandler<SuperTabGetTabItemContentRectangleEventArgs> GetTabItemContentRectangle;

        /// <summary>
        /// Occurs when the control needs to measure a tab
        /// </summary>
        [Description("Occurs when the control needs to measure a tab.")]
        public event EventHandler<SuperTabMeasureTabItemEventArgs> MeasureTabItem;

        /// <summary>
        /// Occurs before any tab rendering is done
        /// </summary>
        [Description("Occurs before any tab rendering is done.")]
        public event EventHandler<SuperTabPreRenderTabItemEventArgs> PreRenderTabItem;

        /// <summary>
        /// Occurs After all tab rendering is complete
        /// </summary>
        [Description("Occurs After all tab rendering is complete.")]
        public event EventHandler<SuperTabPostRenderTabItemEventArgs> PostRenderTabItem;

        /// <summary>
        /// Occurs when the control needs to get the tab's Text Bounds
        /// </summary>
        [Description("Occurs when the control needs to get the tab's Text Bounds.")]
        public event EventHandler<SuperTabGetTabTextBoundsEventArgs> GetTabTextBounds;

        /// <summary>
        /// Occurs when the control needs to get the tab's Image Bounds
        /// </summary>
        [Description("Occurs when the control needs to get the tab's Image Bounds.")]
        public event EventHandler<SuperTabGetTabImageBoundsEventArgs> GetTabImageBounds;

        /// <summary>
        /// Occurs when the control needs to get the tab's Close Button Bounds
        /// </summary>
        [Description("Occurs when the control needs to get the tab's Close Button Bounds.")]
        public event EventHandler<SuperTabGetTabCloseBoundsEventArgs> GetTabCloseBounds;

        /// <summary>
        /// Occurs when the TabStrip background needs painted
        /// </summary>
        [Description("Occurs when the TabStrip background needs painted.")]
        public event EventHandler<SuperTabStripPaintBackgroundEventArgs> TabStripPaintBackground;

        /// <summary>
        /// Occurs when a TabStrip MouseUp event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseUp event is raised.")]
        public event EventHandler<MouseEventArgs> TabStripMouseUp;

        /// <summary>
        /// Occurs when a TabStrip MouseDown event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseDown event is raised.")]
        public event EventHandler<MouseEventArgs> TabStripMouseDown;

        /// <summary>
        /// Occurs when a TabStrip MouseMove event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseMove event is raised.")]
        public event EventHandler<MouseEventArgs> TabStripMouseMove;

        /// <summary>
        /// Occurs when a TabStrip MouseClick event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseClick event is raised.")]
        public event EventHandler<MouseEventArgs> TabStripMouseClick;

        /// <summary>
        /// Occurs when a TabStrip MouseDoubleClick event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseDoubleClick event is raised.")]
        public event EventHandler<MouseEventArgs> TabStripMouseDoubleClick;

        /// <summary>
        /// Occurs when a TabStrip MouseEnter event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseEnter event is raised.")]
        public event EventHandler<EventArgs> TabStripMouseEnter;

        /// <summary>
        /// Occurs when a TabStrip MouseHover event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseHover event is raised.")]
        public event EventHandler<EventArgs> TabStripMouseHover;

        /// <summary>
        /// Occurs when a TabStrip MouseLeave event is raised
        /// </summary>
        [Description("Occurs when a TabStrip MouseLeave event is raised.")]
        public event EventHandler<EventArgs> TabStripMouseLeave;

        #endregion

        #region Private variables

        private readonly SuperTabStrip _TabStrip;

        private bool _Initializing;
        private bool _TabsVisible = true;
        private int _LoadSelectedTabIndex = -1;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SuperTabControl()
        {
            _TabStrip = new SuperTabStrip();

            Controls.Add(_TabStrip);

            _TabStrip.Dock = DockStyle.Top;
            _TabStrip.SendToBack();

            HookEvents(true);

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            StyleManager.Register(this);
        }

        #region Public properties

        #region TabStripTabStop
        /// <summary>
        /// Gets or sets whether TabStrip will get focus when Tab key is used. Default value is false.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Gets or sets whether TabStrip will get focus when Tab key is used.")]
        public bool TabStripTabStop
        {
            get { return TabStrip.TabStop; }
            set
            {
                TabStrip.TabStop = value;
            }
        }
        protected override void OnGotFocus(EventArgs e)
        {
            if (this.TabStripTabStop)
                this.TabStrip.Focus();
            base.OnGotFocus(e);
        }
        #endregion

        #region AntiAlias

        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return (_TabStrip.AntiAlias); }
            set { _TabStrip.AntiAlias = value; }
        }

        #endregion

        #region CloseButton properties

        #region AutoCloseTabs

        /// <summary>
        /// Gets or sets whether tabs are automatically closed when a close button is clicked
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Close Button")]
        [Description("Indicates whether tab is automatically closed when close button is clicked.")]
        public bool AutoCloseTabs
        {
            get { return (_TabStrip.AutoCloseTabs); }
            set { _TabStrip.AutoCloseTabs = value; }
        }

        #endregion

        #region CloseButtonOnTabsAlwaysDisplayed

        /// <summary>
        /// Gets or sets whether the tab's visible close button is displayed for every tab state
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoBrowsable(true), Category("Close Button")]
        [Description("Indicates whether the tab's visible close button is displayed for every tab state.")]
        public bool CloseButtonOnTabsAlwaysDisplayed
        {
            get { return (_TabStrip.CloseButtonOnTabsAlwaysDisplayed); }
            set { _TabStrip.CloseButtonOnTabsAlwaysDisplayed = value; }
        }

        #endregion

        #region CloseButtonOnTabsVisible

        /// <summary>
        /// Gets or sets whether close button is visible on each tab
        /// </summary>
        [Browsable(true), DefaultValue(false), DevCoBrowsable(true), Category("Close Button")]
        [Description("Indicates whether close button is visible on each tab.")]
        public bool CloseButtonOnTabsVisible
        {
            get { return (_TabStrip.CloseButtonOnTabsVisible); }
            set { _TabStrip.CloseButtonOnTabsVisible = value; }
        }

        #endregion

        #region CloseButtonPosition

        /// <summary>
        /// Gets or sets the position of the tab close button
        /// </summary>
        [Browsable(true), DefaultValue(eTabCloseButtonPosition.Right), DevCoBrowsable(true)]
        [Category("Close Button"), Description("Indicates the position of the tab close button.")]
        public eTabCloseButtonPosition CloseButtonPosition
        {
            get { return (_TabStrip.CloseButtonPosition); }
            set { _TabStrip.CloseButtonPosition = value; }
        }

        #endregion

        #region TabCloseButtonNormal

        /// <summary>
        /// Gets or sets the custom tab Close button image
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Close Button")]
        [Description("Indicates the custom tab Close button image.")]
        public Image TabCloseButtonNormal
        {
            get { return (_TabStrip.TabCloseButtonNormal); }
            set { _TabStrip.TabCloseButtonNormal = value; }
        }

        #endregion

        #region TabCloseButtonHot

        /// <summary>
        /// Gets or sets the custom Close button image that is used on tabs when the mouse is over the close button
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Close Button")]
        [Description("Indicates custom Close button image that is used on tabs when the mouse is over the close button.")]
        public Image TabCloseButtonHot
        {
            get { return (_TabStrip.TabCloseButtonHot); }
            set { _TabStrip.TabCloseButtonHot = value; }
        }

        #endregion

        #region TabCloseButtonPressed

        /// <summary>
        /// Gets or sets the custom Close button image that is used on tabs when the button has been pressed
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Close Button")]
        [Description("Indicates custom Close button image that is used on tabs when the button has been pressed.")]
        public Image TabCloseButtonPressed
        {
            get { return (_TabStrip.TabCloseButtonPressed); }
            set { _TabStrip.TabCloseButtonPressed = value; }
        }

        #endregion

        #endregion

        #region ControlBox

        /// <summary>
        /// Gets the TabStrip ControlBox
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Gets the TabStrip ControlBox.")]
        public SuperTabControlBox ControlBox
        {
            get { return (_TabStrip.ControlBox); }
        }

        #endregion

        #region DisplaySelectedTextOnly

        /// <summary>
        /// Gets or sets whether the only Text displayed is for the SelectedTab
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior"), DefaultValue(false)]
        [Description("Specifies whether the only Text displayed is for the SelectedTab.")]
        public bool DisplaySelectedTextOnly
        {
            get { return (_TabStrip.DisplaySelectedTextOnly); }
            set { _TabStrip.DisplaySelectedTextOnly = value; }
        }

        #endregion

        #region HorizontalText

        /// <summary>
        /// Gets or sets whether text is drawn horizontally regardless of tab orientation
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text is drawn horizontally regardless of tab orientation.")]
        public bool HorizontalText
        {
            get { return (_TabStrip.HorizontalText); }
            set { _TabStrip.HorizontalText = value; }
        }

        #endregion

        #region ImageList

        /// <summary>
        /// Gets or sets the ImageList used by the TabStrip and its tab items
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the ImageList used by the TabStrip and its tab items.")]
        public ImageList ImageList
        {
            get { return (_TabStrip.ImageList); }
            set { _TabStrip.ImageList = value; }
        }

        #endregion

        #region ItemPadding

        /// <summary>
        /// Gets or sets BaseItem tab padding in pixels.
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Description("Indicates BaseItem tab padding in pixels.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Padding ItemPadding
        {
            get { return (_TabStrip.ItemPadding); }
            set { _TabStrip.ItemPadding = value; }
        }

        #endregion

        #region SelectedPanel

        /// <summary>
        /// Gets or sets the SelectedTab based upon the given panel
        /// </summary>
        [Browsable(false), DevCoBrowsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SuperTabControlPanel SelectedPanel
        {
            get
            {
                if (_TabStrip.SelectedTab != null)
                    return (_TabStrip.SelectedTab.AttachedControl as SuperTabControlPanel);

                return (null);
            }

            set
            {
                if (value != null && Controls.Contains(value))
                {
                    if (value.TabItem != null && Tabs.Contains(value.TabItem))
                        _TabStrip.SelectedTab = value.TabItem;
                }
            }
        }

        #endregion

        #region ShowFocusRectangle

        /// <summary>
        /// Gets or sets whether a focus rectangle is displayed when tab has input focus
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether a focus rectangle is displayed when tab has input focus.")]
        public bool ShowFocusRectangle
        {
            get { return (_TabStrip.ShowFocusRectangle); }
            set { _TabStrip.ShowFocusRectangle = value; }
        }

        #endregion

        #region Tab properties

        #region FixedTabSize

        /// <summary>
        /// Gets or sets the fixed tab size in pixels. Either Height, Width, or both can be set
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the fixed tab size in pixels. Either Height, Width, or both can be set.")]
        public Size FixedTabSize
        {
            get { return (_TabStrip.FixedTabSize); }
            set { _TabStrip.FixedTabSize = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFixedTabSize()
        {
            return (_TabStrip.ShouldSerializeFixedTabSize());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFixedTabSize()
        {
            TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
        }

        #endregion

        #region IsTabDragging

        /// <summary>
        /// Gets or sets whether a tab is currently in a drag operation
        /// </summary>
        [Browsable(false)]
        public bool IsTabDragging
        {
            get { return (_TabStrip.IsTabDragging); }
        }

        #endregion

        #region ReorderTabsEnabled

        /// <summary>
        /// Gets or sets whether tabs can be reordered through the user interface
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior")]
        [Description("Indicates whether the control tabs can be reordered through the user interface.")]
        public bool ReorderTabsEnabled
        {
            get { return (_TabStrip.ReorderTabsEnabled); }
            set { _TabStrip.ReorderTabsEnabled = value; }
        }

        #endregion

        #region SelectedTab

        /// <summary>
        /// Gets or sets the selected tab
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SuperTabItem SelectedTab
        {
            get { return (_TabStrip.SelectedTab); }
            set { _TabStrip.SelectedTab = value; }
        }

        #endregion

        #region SelectedTabFont

        /// <summary>
        /// Gets or sets the selected tab Font
        /// </summary>
        [Browsable(true), DevCoBrowsable(true),  Localizable(true), Category("Style"), DefaultValue(null)]
        [Description("Indicates the selected tab Font")]
        public Font SelectedTabFont
        {
            get { return (_TabStrip.SelectedTabFont); }
            set { _TabStrip.SelectedTabFont = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectedTabFont()
        {
            return (_TabStrip.ShouldSerializeSelectedTabFont());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelectedTabFont()
        {
            _TabStrip.ResetSelectedTabFont();
        }

        #endregion

        #region SelectedTabIndex

        /// <summary>
        /// Gets or sets the index of the selected tab
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior")]
        [Description("Indicates the index of the selected tab.")]
        public int SelectedTabIndex
        {
            get { return (_TabStrip.SelectedTabIndex); }

            set
            {
                if (_Initializing == true)
                    _LoadSelectedTabIndex = value;
                else
                    _TabStrip.SelectedTabIndex = value;
            }
        }

        #endregion

        #region TabAlignment

        /// <summary>
        /// Gets or sets the tab alignment within the Tab-Strip control
        /// </summary>
        [Browsable(true), DefaultValue(eTabStripAlignment.Top), DevCoBrowsable(true)]
        [Category("Appearance"), Description("Indicates the tab alignment within the Tab-Strip control.")]
        public eTabStripAlignment TabAlignment
        {
            get { return (_TabStrip.TabAlignment); }

            set
            {
                if (_TabStrip.TabAlignment != value)
                {
                    _TabStrip.TabAlignment = value;

                    SetTabDocking(value);
                }
            }
        }

        #region SetTabDocking

        /// <summary>
        /// Sets the tab docking based upon the given alignment
        /// </summary>
        /// <param name="value"></param>
        private void SetTabDocking(eTabStripAlignment value)
        {
            _TabStrip.SuspendLayout();

            try
            {
                switch (value)
                {
                    case eTabStripAlignment.Top:
                        _TabStrip.Dock = DockStyle.Top;
                        break;

                    case eTabStripAlignment.Bottom:
                        _TabStrip.Dock = DockStyle.Bottom;
                        break;

                    case eTabStripAlignment.Left:
                        _TabStrip.Dock = DockStyle.Left;
                        break;

                    case eTabStripAlignment.Right:
                        _TabStrip.Dock = DockStyle.Right;
                        break;
                }

                _TabStrip.SendToBack();
            }
            finally
            {
                _TabStrip.ResumeLayout(true);
            }

            RefreshPanelsStyle();

            RecalcLayout();
        }

        #endregion

        #endregion

        #region TabFont

        /// <summary>
        /// Gets or sets the tab Font
        /// </summary>
        [Browsable(true), DevCoBrowsable(true),  Localizable(true), Category("Style"), DefaultValue(null)]
        [Description("Indicates the tab Font")]
        public Font TabFont
        {
            get { return (_TabStrip.TabFont); }
            set { _TabStrip.TabFont = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTabFont()
        {
            return (_TabStrip.ShouldSerializeTabFont());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTabFont()
        {
            _TabStrip.ResetTabFont();
        }

        #endregion

        #region TabHorizontalSpacing

        /// <summary>
        /// Gets or sets the Horizontal spacing around tab elements
        /// </summary>
        [Browsable(true), DefaultValue(5), Category("Appearance")]
        [Description("Indicates the Horizontal spacing around tab elements.")]
        public int TabHorizontalSpacing
        {
            get { return (_TabStrip.TabHorizontalSpacing); }
            set { _TabStrip.TabHorizontalSpacing = value; }
        }

        #endregion

        #region TabLayoutType

        /// <summary>
        /// Gets or sets the type of the tab layout
        /// </summary>
        [Browsable(true), DefaultValue(eSuperTabLayoutType.SingleLine), Category("Appearance")]
        [Description("Indicates the type of the tab layout.")]
        public eSuperTabLayoutType TabLayoutType
        {
            get { return (_TabStrip.TabLayoutType); }
            set { _TabStrip.TabLayoutType = value; }
        }

        #endregion

        #region Tabs

        /// <summary>
        /// Gets the collection of Tabs
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.SuperTabControlTabsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Data"), Description("Indicates the collection of Tabs.")]
        public SubItemsCollection Tabs
        {
            get { return (_TabStrip.Tabs); }
        }

        #endregion

        #region TabStrip

        /// <summary>
        /// Gets the control TabStrip
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SuperTabStrip TabStrip
        {
            get { return (_TabStrip); }
        }

        #endregion

        #region TabStripColor

        /// <summary>
        /// Gets or sets the Color of the TabStrip
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Color of the TabStrip.")]
        public SuperTabColorTable TabStripColor
        {
            get { return (_TabStrip.TabStripColor); }
            set { _TabStrip.TabStripColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTabStripColor()
        {
            return (_TabStrip.TabStripColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTabStripColor()
        {
            _TabStrip.TabStripColor = new SuperTabColorTable();
        }

        #endregion

        #region TabStyle

        /// <summary>
        /// Gets or sets the tab style
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(eSuperTabStyle.Office2007)]
        [Category("Appearance"), Description("Indicates the tab style.")]
        public eSuperTabStyle TabStyle
        {
            get { return (_TabStrip.TabStyle); }

            set
            {
                if (_TabStrip.TabStyle != value)
                {
                    _TabStrip.TabStyle = value;

                    RefreshPanelsStyle();
                }
            }
        }

        #endregion

        #region TabVerticalSpacing

        /// <summary>
        /// Gets or sets the Vertical spacing around tab elements
        /// </summary>
        [Browsable(true), DefaultValue(4), Category("Appearance")]
        [Description("Indicates the Vertical spacing around tab elements.")]
        public int TabVerticalSpacing
        {
            get { return (_TabStrip.TabVerticalSpacing); }
            set { _TabStrip.TabVerticalSpacing = value; }
        }

        #endregion

        #region TabsVisible

        /// <summary>
        /// Gets or sets whether tabs are visible
        /// </summary>
        [DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether tabs are visible")]
        public bool TabsVisible
        {
            get { return (_TabsVisible); }

            set
            {
                if (_TabsVisible != value)
                {
                    _TabsVisible = value;
                    _TabStrip.Visible = value;

                    RefreshPanelsStyle();
                }
            }
        }

        #endregion

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets tab text alignment
        /// </summary>
        [Browsable(true), DefaultValue(eItemAlignment.Near)]
        [DevCoBrowsable(true), Category("Layout"), Description("Indicates tab text alignment.")]
        public eItemAlignment TextAlignment
        {
            get { return (_TabStrip.TextAlignment); }
            set { _TabStrip.TextAlignment = value; }
        }

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks (or unhooks) underlying TabStrip events
        /// </summary>
        /// <param name="hook">true to hook, false to unhook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _TabStrip.TabItemOpen += TabStripTabItemOpen;
                _TabStrip.TabItemClose += TabStripTabItemClose;
                _TabStrip.TabRemoved += TabStripTabRemoved;
                _TabStrip.TabMoving += TabStripTabMoving;
                _TabStrip.TabMoved += TabStripTabMoved;

                _TabStrip.SelectedTabChanging += TabStripSelectedTabChanging;
                _TabStrip.SelectedTabChanged += TabStripSelectedTabChanged;

                _TabStrip.GetTabItemPath += TabStripItemGetTabItemPath;
                _TabStrip.GetTabItemContentRectangle += TabStripItemGetTabItemContentRectangle;

                _TabStrip.BeforeTabDisplay += TabStripBeforeTabDisplay;
                _TabStrip.MeasureTabItem += TabStripMeasureTabItem;
                _TabStrip.PreRenderTabItem += TabStripPreRenderTabItem;
                _TabStrip.PostRenderTabItem += TabStripPostRenderTabItem;

                _TabStrip.GetTabTextBounds += TabStripGetTabTextBounds;
                _TabStrip.GetTabImageBounds += TabStripGetTabImageBounds;
                _TabStrip.GetTabCloseBounds += TabStripGetTabCloseBounds;

                _TabStrip.TabStripPaintBackground += TabStripTabStripPaintBackground;

                _TabStrip.TabStripTabColorChanged += TabStripTabColorChanged;

                _TabStrip.MouseUp += TabStrip_MouseUp;
                _TabStrip.MouseDown += TabStrip_MouseDown;
                _TabStrip.MouseMove += TabStrip_MouseMove;
                _TabStrip.MouseClick += TabStrip_MouseClick;
                _TabStrip.MouseDoubleClick += TabStrip_MouseDoubleClick;

                _TabStrip.MouseEnter += TabStrip_MouseEnter;
                _TabStrip.MouseHover += TabStrip_MouseHover;
                _TabStrip.MouseLeave += TabStrip_MouseLeave;
            }
            else
            {
                _TabStrip.TabItemOpen -= TabStripTabItemOpen;
                _TabStrip.TabItemClose -= TabStripTabItemClose;
                _TabStrip.TabRemoved -= TabStripTabRemoved;
                _TabStrip.TabMoving -= TabStripTabMoving;
                _TabStrip.TabMoved -= TabStripTabMoved;

                _TabStrip.SelectedTabChanging -= TabStripSelectedTabChanging;
                _TabStrip.SelectedTabChanged -= TabStripSelectedTabChanged;

                _TabStrip.GetTabItemPath -= TabStripItemGetTabItemPath;
                _TabStrip.GetTabItemContentRectangle -= TabStripItemGetTabItemContentRectangle;

                _TabStrip.BeforeTabDisplay -= TabStripBeforeTabDisplay;
                _TabStrip.MeasureTabItem -= TabStripMeasureTabItem;
                _TabStrip.PreRenderTabItem -= TabStripPreRenderTabItem;
                _TabStrip.PostRenderTabItem -= TabStripPostRenderTabItem;

                _TabStrip.GetTabTextBounds -= TabStripGetTabTextBounds;
                _TabStrip.GetTabImageBounds -= TabStripGetTabImageBounds;
                _TabStrip.GetTabCloseBounds -= TabStripGetTabCloseBounds;

                _TabStrip.TabStripPaintBackground -= TabStripTabStripPaintBackground;

                _TabStrip.TabStripTabColorChanged -= TabStripTabColorChanged;

                _TabStrip.MouseUp -= TabStrip_MouseUp;
                _TabStrip.MouseDown -= TabStrip_MouseDown;
                _TabStrip.MouseMove -= TabStrip_MouseMove;
                _TabStrip.MouseClick -= TabStrip_MouseClick;
                _TabStrip.MouseDoubleClick -= TabStrip_MouseDoubleClick;

                _TabStrip.MouseEnter -= TabStrip_MouseEnter;
                _TabStrip.MouseHover -= TabStrip_MouseHover;
                _TabStrip.MouseLeave -= TabStrip_MouseLeave;
            }
        }

        #endregion

        #region Event processing

        #region TabItemOpen

        /// <summary>
        /// Handles TabItemOpen events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripTabItemOpen(object sender, SuperTabStripTabItemOpenEventArgs e)
        {
            if (TabItemOpen != null)
                TabItemOpen(this, e);
        }

        #endregion

        #region TabItemClose

        /// <summary>
        /// Handles TabItemClose events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripTabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (TabItemClose != null)
                TabItemClose(this, e);
        }

        #endregion

        #region TabMoving

        /// <summary>
        /// Handles TabMoving events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripTabMoving(object sender, SuperTabStripTabMovingEventArgs e)
        {
            if (TabMoving != null)
                TabMoving(this, e);
        }

        #endregion

        #region TabMoved

        /// <summary>
        /// Handles TabMoved events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripTabMoved(object sender, SuperTabStripTabMovedEventArgs e)
        {
            if (TabMoved != null)
                TabMoved(this, e);
        }

        #endregion

        #region TabRemoved

        /// <summary>
        /// Handles TabRemoved events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripTabRemoved(object sender, SuperTabStripTabRemovedEventArgs e)
        {
            if (TabRemoved != null)
                TabRemoved(this, e);
        }

        #endregion

        #region SelectedTabChanging

        /// <summary>
        /// Handles SelectedTabChanging events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripSelectedTabChanging(object sender, SuperTabStripSelectedTabChangingEventArgs e)
        {
            if (SelectedTabChanging != null)
                SelectedTabChanging(this, e);
        }

        #endregion

        #region SelectedTabChanged

        /// <summary>
        /// Handles SelectedTabChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripSelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            if (SelectedTabChanged != null)
                SelectedTabChanged(this, e);
        }

        #endregion

        #region GetTabItemPath

        /// <summary>
        /// Handles GetTabItemPath events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItemGetTabItemPath(object sender, SuperTabGetTabItemPathEventArgs e)
        {
            if (GetTabItemPath != null)
                GetTabItemPath(this, e);
        }

        #endregion

        #region GetTabItemContentRectangle

        /// <summary>
        /// Handles GetTabItemContentRectangle events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItemGetTabItemContentRectangle(object sender, SuperTabGetTabItemContentRectangleEventArgs e)
        {
            if (GetTabItemContentRectangle != null)
                GetTabItemContentRectangle(this, e);
        }

        #endregion

        #region BeforeTabDisplay

        /// <summary>
        /// Handles BeforeTabDisplay events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripBeforeTabDisplay(object sender, SuperTabStripBeforeTabDisplayEventArgs e)
        {
            if (BeforeTabDisplay != null)
                BeforeTabDisplay(this, e);
        }

        #endregion

        #region MeasureTabItem

        /// <summary>
        /// Handles MeasureTabItem events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripMeasureTabItem(object sender, SuperTabMeasureTabItemEventArgs e)
        {
            if (MeasureTabItem != null)
                MeasureTabItem(this, e);
        }

        #endregion

        #region PreRenderTabItem

        /// <summary>
        /// Handles PreRenderTabItem events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripPreRenderTabItem(object sender, SuperTabPreRenderTabItemEventArgs e)
        {
            if (PreRenderTabItem != null)
                PreRenderTabItem(this, e);
        }

        #endregion

        #region PostRenderTabItem

        /// <summary>
        /// Handles PostRenderTabItem events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripPostRenderTabItem(object sender, SuperTabPostRenderTabItemEventArgs e)
        {
            if (PostRenderTabItem != null)
                PostRenderTabItem(this, e);
        }

        #endregion

        #region GetTabCloseBounds

        /// <summary>
        /// Handles GetTabCloseBounds events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripGetTabCloseBounds(object sender, SuperTabGetTabCloseBoundsEventArgs e)
        {
            if (GetTabCloseBounds != null)
                GetTabCloseBounds(this, e);
        }

        #endregion

        #region GetTabImageBounds

        /// <summary>
        /// Handles GetTabImageBounds events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripGetTabImageBounds(object sender, SuperTabGetTabImageBoundsEventArgs e)
        {
            if (GetTabImageBounds != null)
                GetTabImageBounds(this, e);
        }

        #endregion

        #region GetTabTextBounds

        /// <summary>
        /// Handles GetTabTextBounds events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripGetTabTextBounds(object sender, SuperTabGetTabTextBoundsEventArgs e)
        {
            if (GetTabTextBounds != null)
                GetTabTextBounds(this, e);
        }

        #endregion

        #region TabStripPaintBackground

        /// <summary>
        /// Handles TabStripPaintBackground events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripTabStripPaintBackground(object sender, SuperTabStripPaintBackgroundEventArgs e)
        {
            if (TabStripPaintBackground != null)
                TabStripPaintBackground(this, e);
        }

        #endregion

        #region StyleManagerStyleChanged

        /// <summary>
        /// Handles StyleManagerStyleChanged events
        /// </summary>
        /// <param name="newStyle"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            RefreshPanelsStyle();
        }

        #endregion

        #region TabStripTabColorChanged

        /// <summary>
        /// Handles TabStripTabColorChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripTabColorChanged(object sender, SuperTabStripTabColorChangedEventArgs e)
        {
            SuperTabControlPanel panel = e.Tab.AttachedControl as SuperTabControlPanel;

            if (panel != null)
                ApplyPanelStyle(panel);
        }

        #endregion

        #region TabStrip_MouseUp

        /// <summary>
        /// Handles TabStrip_MouseUp events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseUp(object sender, MouseEventArgs e)
        {
            if (TabStripMouseUp != null)
                TabStripMouseUp(sender, e);
        }

        #endregion

        #region MouseDown

        /// <summary>
        /// Handles TabStrip_MouseDown events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseDown(object sender, MouseEventArgs e)
        {
            if (TabStripMouseDown != null)
                TabStripMouseDown(sender, e);
        }

        #endregion

        #region MouseMove

        /// <summary>
        /// Handles TabStrip_MouseMove events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (TabStripMouseMove != null)
                TabStripMouseMove(sender, e);
        }

        #endregion

        #region MouseClick

        /// <summary>
        /// Handles TabStrip_MouseClick events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseClick(object sender, MouseEventArgs e)
        {
            if (TabStripMouseClick != null)
                TabStripMouseClick(sender, e);
        }

        #endregion

        #region MouseDoubleClick

        /// <summary>
        /// Handles TabStrip_MouseDoubleClick events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (TabStripMouseDoubleClick != null)
                TabStripMouseDoubleClick(sender, e);
        }

        #endregion

        #region MouseEnter

        /// <summary>
        /// Handles TabStrip_MouseEnter events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseEnter(object sender, EventArgs e)
        {
            if (TabStripMouseEnter != null)
                TabStripMouseEnter(sender, e);
        }

        #endregion

        #region MouseHover

        /// <summary>
        /// Handles TabStrip_MouseHover events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseHover(object sender, EventArgs e)
        {
            if (TabStripMouseHover != null)
                TabStripMouseHover(sender, e);
        }

        #endregion

        #region MouseLeave

        /// <summary>
        /// Handles TabStrip_MouseLeave events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStrip_MouseLeave(object sender, EventArgs e)
        {
            if (TabStripMouseLeave != null)
                TabStripMouseLeave(sender, e);
        }

        #endregion

        #endregion

        #region DefaultSize

        /// <summary>
        /// Gets the DefaultSize
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(200, 100); }
        }

        #endregion

        #region CreateTab

        /// <summary>
        /// Creates a TabControl tab
        /// </summary>
        /// <param name="tabText"></param>
        /// <returns></returns>
        public SuperTabItem CreateTab(string tabText)
        {
            return (CreateTab(tabText, -1));
        }

        /// <summary>
        /// Creates a TabControl tab
        /// </summary>
        /// <param name="tabText"></param>
        /// <param name="insertAt"></param>
        /// <returns></returns>
        public SuperTabItem CreateTab(string tabText, int insertAt)
        {
            SuperTabItem tab = new SuperTabItem();
            SuperTabControlPanel panel = new SuperTabControlPanel();

            tab.Text = tabText;

            return (CreateTab(tab, panel, insertAt));
        }

        /// <summary>
        /// Creates a TabControl tab
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="panel"></param>
        /// <param name="insertAt"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SuperTabItem CreateTab(SuperTabItem tab, SuperTabControlPanel panel, int insertAt)
        {
            SuspendLayout();

            tab.AttachedControl = panel;
            panel.TabItem = tab;

            Controls.Add(panel);

            if (insertAt < 0 || insertAt >= Tabs.Count)
                Tabs.Add(tab);
            else
                Tabs.Insert(insertAt, tab);

            ApplyPanelStyle(panel);

            panel.Size = Size.Empty;
            panel.Dock = DockStyle.Fill;
            panel.BringToFront();

            ResumeLayout();

            return (tab);
        }

        #endregion

        #region SelectPreviousTab

        /// <summary>
        /// Selects the previous tab
        /// </summary>
        /// <returns>true if tab selected</returns>
        public bool SelectPreviousTab()
        {
            return (_TabStrip.SelectPreviousTab());
        }

        #endregion

        #region SelectNextTab

        /// <summary>
        /// Selects the next tab
        /// </summary>
        /// <returns>true if tab selected</returns>
        public bool SelectNextTab()
        {
            return (_TabStrip.SelectNextTab());
        }

        #endregion

        #region GetTabFromPoint

        /// <summary>
        /// Gets the SuperTabItem tab containing the given Point
        /// </summary>
        /// <param name="pt">Point to test</param>
        /// <returns>Associated tab, or null</returns>
        public SuperTabItem GetTabFromPoint(Point pt)
        {
            return (_TabStrip.GetTabFromPoint(pt));
        }

        #endregion

        #region GetItemFromPoint

        /// <summary>
        /// Gets the item (SuperTabItem or BaseView) associated
        /// with the given Point
        /// </summary>
        /// <param name="pt">Point to test</param>
        /// <returns>BaseItem or null</returns>
        public BaseItem GetItemFromPoint(Point pt)
        {
            return (_TabStrip.GetItemFromPoint(pt));
        }

        #endregion

        #region CloseTab

        /// <summary>
        /// Closes the given tab
        /// </summary>
        /// <param name="tab"></param>
        public void CloseTab(SuperTabItem tab)
        {
            _TabStrip.CloseTab(tab);
        }

        #endregion

        #region RecalcLayout

        /// <summary>
        /// Causes a Layout Recalculation of the SuperTabControl
        /// </summary>
        public void RecalcLayout()
        {
            _TabStrip.RecalcLayout();
        }

        #endregion

        #region RefreshPanelStyle

        /// <summary>
        /// Refreshes all panel styles
        /// </summary>
        private void RefreshPanelsStyle()
        {
            if (_Initializing == false)
            {
                foreach (Control c in this.Controls)
                {
                    if (c is SuperTabControlPanel)
                        ApplyPanelStyle(c as SuperTabControlPanel);
                }
            }
        }

        #endregion

        #region ApplyPanelStyle

        /// <summary>
        /// Applies color and border settings to the given panel
        /// </summary>
        /// <param name="panel"></param>
        public void ApplyPanelStyle(SuperTabControlPanel panel)
        {
            if (panel != null && panel.TabItem != null && panel.TabItem.TabStripItem != null)
            {
                ElementStyle style = new ElementStyle();

                ApplyPanelColor(panel, style);
                ApplyPanelBorder(style);
                style.BackgroundImage = panel.BackgroundImage;
                style.BackgroundImagePosition = panel.BackgroundImagePosition;
                panel.PanelStyle = style;
            }
        }

        #region ApplyPanelColor

        /// <summary>
        /// Applies color settings to the given panel
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="style"></param>
        private void ApplyPanelColor(
            SuperTabControlPanel panel, ElementStyle style)
        {
            SuperTabPanelItemColorTable pct = panel.TabItem.GetPanelColorTable();
            SuperTabItemStateColorTable sct = panel.TabItem.GetTabColorTable(eTabState.Selected);

            int angle;
            bool? adaptiveGradient;

            // If we have PanelColorTable settings, use them.  If not, use
            // the current StateColorTable panel settings

            if (pct.Background.IsEmpty == false)
            {
                angle = pct.Background.GradientAngle;
                adaptiveGradient = pct.Background.AdaptiveGradient ?? sct.Background.AdaptiveGradient;

                style.BackColorBlend.Clear();

                if (pct.Background.Colors != null)
                {
                    if (pct.Background.Colors.Length > 2)
                    {
                        style.BackColorBlend.CopyFrom(pct.Background.GetColorBlendCollection());
                    }
                    else
                    {
                        style.BackColor = pct.Background.Colors[0];

                        style.BackColor2 = (pct.Background.Colors.Length == 1) ?
                            pct.Background.Colors[0] : pct.Background.Colors[1];
                    }
                }
            }
            else
            {
                angle = sct.Background.GradientAngle;
                adaptiveGradient = sct.Background.AdaptiveGradient;

                if (sct.Background.Colors != null)
                {
                    style.BackColor = sct.Background.Colors[0];

                    style.BackColor2 = (sct.Background.Colors.Length == 1) ?
                        sct.Background.Colors[0] : sct.Background.Colors[1];
                }
            }

            // If we have an adaptive gradient, alter the gradient
            // angle to correspond to the current tab alignment

            if (adaptiveGradient != false)
            {
                switch (_TabStrip.TabAlignment)
                {
                    case eTabStripAlignment.Top:
                        style.BackColorGradientAngle = angle - 180;
                        break;

                    case eTabStripAlignment.Bottom:
                        style.BackColorGradientAngle = angle;
                        break;

                    case eTabStripAlignment.Left:
                        style.BackColorGradientAngle = angle + 90;
                        break;

                    case eTabStripAlignment.Right:
                        style.BackColorGradientAngle = angle - 90;
                        break;
                }
            }

            // Set the panel's inner and outer border colors

            style.BorderColor = pct.OuterBorder;
            style.BorderColorLight = pct.InnerBorder;
        }

        #endregion

        #region ApplyPanelBorder

        /// <summary>
        /// Applies border settings for the given panel
        /// </summary>
        /// <param name="style"></param>
        private void ApplyPanelBorder(ElementStyle style)
        {
            if (TabsVisible == true)
            {
                style.BorderWidth = 0;

                if (style.BorderColor.IsEmpty == false)
                    style.BorderWidth++;

                if (style.BorderWidth == 0)
                    style.Border = eStyleBorderType.None;

                else if (style.BorderColorLight.IsEmpty)
                    style.Border = eStyleBorderType.Solid;

                else
                    style.Border = eStyleBorderType.Double;

                switch (_TabStrip.TabAlignment)
                {
                    case eTabStripAlignment.Top:
                        style.BorderTop = eStyleBorderType.None;
                        break;

                    case eTabStripAlignment.Bottom:
                        style.BorderBottom = eStyleBorderType.None;
                        break;

                    case eTabStripAlignment.Left:
                        style.BorderLeft = eStyleBorderType.None;
                        break;

                    case eTabStripAlignment.Right:
                        style.BorderRight = eStyleBorderType.None;
                        break;
                }
            }
            else
            {
                style.Border = eStyleBorderType.None;
            }
        }

        #endregion

        #endregion

        #region OnSystemColorsChanged

        /// <summary>
        /// Performs OnSystemColorsChanged processing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);

            Application.DoEvents();

            RefreshPanelsStyle();
        }

        #endregion

        #region OnResize

        /// <summary>
        /// Performs OnResize processing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _TabStrip.Size = Size;
        }

        #endregion

        #region OnControlAdded

        /// <summary>
        /// OnControlAdded
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (DesignMode == false)
            {
                SuperTabControlPanel panel = e.Control as SuperTabControlPanel;

                if (panel != null)
                {
                    if (panel.TabItem != null)
                    {
                        if (this.Tabs.Contains(panel.TabItem) && this.SelectedTab == panel.TabItem)
                        {
                            panel.Visible = true;
                            panel.BringToFront();
                        }
                        else
                            panel.Visible = false;
                    }
                    else
                    {
                        panel.Visible = false;
                    }
                }
            }
        }

        #endregion

        #region OnControlRemoved

        /// <summary>
        /// OnControlRemoved
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            SuperTabControlPanel panel = e.Control as SuperTabControlPanel;

            if (panel != null)
            {
                if (panel.TabItem != null)
                {
                    if (Tabs.Contains(panel.TabItem))
                        Tabs.Remove(panel.TabItem);
                }
            }
        }

        #endregion

        #region ISupportInitialize Members

        /// <summary>
        /// BeginInit
        /// </summary>
        void ISupportInitialize.BeginInit()
        {
            _Initializing = true;
        }

        /// <summary>
        /// EndInit
        /// </summary>
        void ISupportInitialize.EndInit()
        {
            _Initializing = false;

            // Apply default panel style

            RefreshPanelsStyle();

            // Establish our initial selected tab

            SuperTabItem tab =
                _TabStrip.TabStripItem.GetNextVisibleTab(_LoadSelectedTabIndex);

            if (tab != null)
            {
                SelectedTab = tab;

                if (_TabStrip.SelectedTab != null && _TabStrip.SelectedTab.AttachedControl != null)
                {
                    _TabStrip.SelectedTab.AttachedControl.Visible = true;
                    _TabStrip.SelectedTab.AttachedControl.BringToFront();
                }
            }

            _LoadSelectedTabIndex = -1;

            // Make sure the pending layout is performed following
            // the designer initialization

            ResumeLayout(true);
        }

        #endregion

    }
}
#endif