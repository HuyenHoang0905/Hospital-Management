#if FRAMEWORK20
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    public class SuperTabStripItem : BaseItem, IDesignTimeProvider
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
        /// Occurs when the TabStrip TabColor has changed
        /// </summary>
        [Description("Occurs when the TabStrip TabColor has changed.")]
        public event EventHandler<SuperTabStripTabColorChangedEventArgs> TabStripTabColorChanged;

        /// <summary>
        /// Occurs when the TabStrip Color has changed
        /// </summary>
        [Description("Occurs when the TabStrip TabColor has changed.")]
        public event EventHandler<EventArgs> TabStripColorChanged;

        #endregion

        #region Private variables

        private eSuperTabStyle _TabStyle = eSuperTabStyle.Office2007;
        private eSuperTabLayoutType _TabLayoutType = eSuperTabLayoutType.SingleLine;

        private Image _TabCloseButtonNormal;
        private Image _TabCloseButtonHot;
        private Image _TabCloseButtonPressed;

        private Size _TabCloseButtonSize = new Size(16, 16);
        private Size _DefaultTabCloseButtonSize = new Size(16, 16);

        private int _TabHorizontalSpacing = 5;
        private int _TabVerticalSpacing = 4;

        private bool _CloseButtonOnTabsVisible;
        private bool _CloseButtonOnTabsAlwaysDisplayed = true;

        private bool _DisplaySelectedTextOnly;
        private bool _ShowFocusRectangle;
        private Size _FixedTabSize = Size.Empty;
        private bool _HorizontalText = true;

        private bool _AutoCloseTabs = true;
        private bool _AutoSelectAttachedControl;

        private bool _ReorderTabsEnabled = true;

        private Font _TabFont;
        private Font _SelectedTabFont;
        private SuperTabColorTable _TabStripColor = new SuperTabColorTable();

        private SuperTabItem _HotTab;
        private SuperTabItem _SelectedTab;
        private BaseItem _VisibleTab;
        private int _SelectedTabIndex;

        private SuperTabControlBox _TabControlBox;
        private SuperTabStripBaseDisplay _TabDisplay;
        private Rectangle _TabItemsBounds;

        private SuperTabItem _DesignTimeSelection;
        private eTabStripAlignment _TabAlignment = eTabStripAlignment.Top;
        private eTabCloseButtonPosition _CloseButtonPosition = eTabCloseButtonPosition.Right;

        private SuperTabStrip _TabStrip;

        private int _TabLines;
        private eItemAlignment _TextAlignment;

        private eSuperTabArea _TabArea;
        private SuperTabItem _MouseDownTab;
        private Point _MouseDownLocation;
        private bool _IsTabDragging;
        private SuperTabDragWindow _TabDragWindow;

        private BaseItem _InsertTab;
        private bool _InsertBefore;
        private bool _RecalcInProgress;

        private Padding _ItemPadding;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabStrip">Associated SuperTabStrip</param>
        public SuperTabStripItem(SuperTabStrip tabStrip)
        {
            _TabStrip = tabStrip;

            _TabControlBox = new SuperTabControlBox(this);
            _TabDisplay = new Office2007SuperTabStrip(this);
            _ItemPadding = new Padding(0, 0, 0, 0);
            
            SetIsContainer(true);

            HookEvents(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _TabCloseButtonNormal);
                BarUtilities.DisposeImage(ref _TabCloseButtonHot);
                BarUtilities.DisposeImage(ref _TabCloseButtonPressed);
            }
            base.Dispose(disposing);
        }

        #region Public properties

        #region AutoCloseTabs

        /// <summary>
        /// Gets or sets AutoCloseTabs
        /// </summary>
        public bool AutoCloseTabs
        {
            get { return (_AutoCloseTabs); }
            set { _AutoCloseTabs = value; }
        }

        #endregion

        #region AutoSelectAttachedControl

        /// <summary>
        /// Gets or sets whether the control attached to the TabItem.AttachedControl property
        /// is automatically selected when TabItem becomes the selected tab. Default value is true.
        /// </summary>
        public bool AutoSelectAttachedControl
        {
            get { return (_AutoSelectAttachedControl); }
            set { _AutoSelectAttachedControl = value; }
        }

        #endregion

        #region CloseButton properties

        #region TabCloseButtonNormal

        /// <summary>
        /// Gets or sets TabCloseButtonNormal
        /// </summary>
        public Image TabCloseButtonNormal
        {
            get { return (_TabCloseButtonNormal); }

            set
            {
                _TabCloseButtonNormal = value;

                _TabCloseButtonSize = (_TabCloseButtonNormal != null)
                    ? _TabCloseButtonNormal.Size : _DefaultTabCloseButtonSize;

                MyRefresh();
            }
        }

        #endregion

        #region TabCloseButtonHot

        /// <summary>
        /// Gets or sets TabCloseButtonHot
        /// </summary>
        public Image TabCloseButtonHot
        {
            get { return (_TabCloseButtonHot); }

            set
            {
                _TabCloseButtonHot = value;

                MyRefresh();
            }
        }

        #endregion

        #region TabCloseButtonPressed

        /// <summary>
        /// Gets or sets TabCloseButtonPressed
        /// </summary>
        public Image TabCloseButtonPressed
        {
            get { return (_TabCloseButtonPressed); }

            set
            {
                _TabCloseButtonPressed = value;

                MyRefresh();
            }
        }

        #endregion

        #region TabHorizontalSpacing

        /// <summary>
        /// Gets or sets TabHorizontalSpacing
        /// </summary>
        public int TabHorizontalSpacing
        {
            get { return (_TabHorizontalSpacing); }

            set
            {
                if (_TabHorizontalSpacing != value)
                {
                    _TabHorizontalSpacing = value;

                    MyRefresh();
                }
            }
        }

        #endregion

        #region TabVerticalSpacing

        /// <summary>
        /// Gets or sets TabVerticalSpacing
        /// </summary>
        public int TabVerticalSpacing
        {
            get { return (_TabVerticalSpacing); }

            set
            {
                if (_TabVerticalSpacing != value)
                {
                    _TabVerticalSpacing = value;

                    MyRefresh();
                }
            }
        }

        #endregion

        #region CloseButtonOnTabsVisible

        /// <summary>
        /// Gets or sets CloseButtonOnTabsVisible
        /// </summary>
        public bool CloseButtonOnTabsVisible
        {
            get { return (_CloseButtonOnTabsVisible); }

            set
            {
                if (_CloseButtonOnTabsVisible != value)
                {
                    _CloseButtonOnTabsVisible = value;

                    MyRefresh();
                }
            }
        }

        #endregion

        #region CloseButtonOnTabsAlwaysDisplayed

        /// <summary>
        /// Gets or sets CloseButtonOnTabsAlwaysDisplayed
        /// </summary>
        public bool CloseButtonOnTabsAlwaysDisplayed
        {
            get { return (_CloseButtonOnTabsAlwaysDisplayed); }

            set
            {
                if (_CloseButtonOnTabsAlwaysDisplayed != value)
                {
                    _CloseButtonOnTabsAlwaysDisplayed = value;

                    MyRefresh();
                }
            }
        }

        #endregion

        #region CloseButtonPosition

        /// <summary>
        /// Gets or sets CloseButtonPosition
        /// </summary>
        public eTabCloseButtonPosition CloseButtonPosition
        {
            get { return _CloseButtonPosition; }

            set
            {
                _CloseButtonPosition = value;

                MyRefresh();
            }
        }

        #endregion

        #endregion

        #region ControlBox

        /// <summary>
        /// Gets the ControlBox
        /// </summary>
        public SuperTabControlBox ControlBox
        {
            get { return (_TabControlBox); }
        }

        #endregion

        #region DesignTimeSelection

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SuperTabItem DesignTimeSelection
        {
            get { return _DesignTimeSelection; }

            set
            {
                _DesignTimeSelection = value;

                Refresh();
            }
        }

        #endregion

        #region DisplaySelectedTextOnly

        /// <summary>
        /// Gets or sets DisplaySelectedTextOnly
        /// </summary>
        public bool DisplaySelectedTextOnly
        {
            get { return _DisplaySelectedTextOnly; }

            set
            {
                _DisplaySelectedTextOnly = value;

                MyRefresh();
            }
        }

        #endregion

        #region Expanded

        /// <summary>
        /// Gets or set the Expanded state
        /// </summary>
        public override bool Expanded
        {
            get { return (base.Expanded); }

            set
            {
                base.Expanded = value;

                for (int i = 0; i < SubItems.Count; i++)
                    SubItems[i].Expanded = value;

                if (ControlBox.Visible == true)
                {
                    ControlBox.Expanded = value;

                    for (int i = 0; i < ControlBox.SubItems.Count; i++)
                        ControlBox.SubItems[i].Expanded = value;
                }
            }
        }

        #endregion

        #region Font properties

        #region TabFont

        /// <summary>
        /// Gets or sets the tab font
        /// </summary>
        public Font TabFont
        {
            get { return (_TabFont); }

            set
            {
                _TabFont = value;

                MyRefresh();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTabFont()
        {
            return (_TabFont != null);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTabFont()
        {
            TabFont = null;
        }

        #endregion

        #region SelectedTabFont

        /// <summary>
        /// Gets or sets the selected tab font
        /// </summary>
        public Font SelectedTabFont
        {
            get { return (_SelectedTabFont); }

            set
            {
                _SelectedTabFont = value;

                MyRefresh();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectedTabFont()
        {
            return (_SelectedTabFont != null);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelectedTabFont()
        {
            SelectedTabFont = null;
        }

        #endregion

        #endregion

        #region HorizontalText

        /// <summary>
        /// Gets or sets the HorizontalText
        /// </summary>
        public bool HorizontalText
        {
            get { return (_HorizontalText); }

            set
            {
                _HorizontalText = value;

                MyRefresh();
            }
        }

        #endregion

        #region ItemPadding

        /// <summary>
        /// Gets or sets the BaseItem tab padding.
        /// </summary>
        public Padding ItemPadding
        {
            get { return (_ItemPadding); }
            set { _ItemPadding = value; }
        }

        #endregion

        #region ShowFocusRectangle

        /// <summary>
        /// Gets or sets ShowFocusRectangle
        /// </summary>
        public bool ShowFocusRectangle
        {
            get { return (_ShowFocusRectangle); }
            set { _ShowFocusRectangle = value; }
        }

        #endregion

        #region Tab properties

        #region FixedTabSize

        /// <summary>
        /// Gets or sets the FixedTabSize
        /// </summary>
        public Size FixedTabSize
        {
            get { return _FixedTabSize; }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);

                if ((value.Width > 0 || value.Height > 0) && _TabDisplay != null)
                {
                    Size minSize = _TabDisplay.MinTabSize;

                    if (value.Width > 0)
                    {
                        if (minSize.Width > value.Width)
                            value.Width = minSize.Width;
                    }

                    if (value.Height > 0)
                    {
                        if (minSize.Height > value.Height)
                            value.Height = minSize.Height;
                    }
                }

                if (_FixedTabSize != value)
                {
                    _FixedTabSize = value;

                    MyRefresh();
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFixedTabSize()
        {
            return (_FixedTabSize.IsEmpty == false);
        }

        #endregion

        #region IsTabDragging

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public bool IsTabDragging
        {
            get { return (_IsTabDragging); }
            internal set { _IsTabDragging = value; }
        }

        #endregion

        #region MinTabStripHeight

        /// <summary>
        /// Gets the MinTabStripHeight
        /// </summary>
        public int MinTabStripHeight
        {
            get { return (_TabDisplay.MinTabStripHeight); }
        }

        #endregion

        #region ReorderTabsEnabled

        /// <summary>
        /// Gets or sets ReorderTabsEnabled
        /// </summary>
        public bool ReorderTabsEnabled
        {
            get { return (_ReorderTabsEnabled); }
            set { _ReorderTabsEnabled = value; }
        }

        #endregion

        #region SelectedTab

        /// <summary>
        /// Gets or sets the selected tab
        /// </summary>
        public SuperTabItem SelectedTab
        {
            get { return (_SelectedTab); }

            set
            {
                if (value != null)
                {
                    if (value.Visible == true)
                        SelectTab(value, eEventSource.Code);
                }
                else
                {
                    SelectTab(null, eEventSource.Code);
                }
            }
        }

        #endregion

        #region SelectedTabIndex

        /// <summary>
        /// Gets or sets the selected tab index
        /// </summary>
        public int SelectedTabIndex
        {
            get
            {
                if (_SelectedTab != null)
                    return (SubItems.IndexOf(_SelectedTab));

                return (-1);
            }

            set
            {
                if (value >= 0 && value < SubItems.Count)
                {
                    SuperTabItem tab = SubItems[value] as SuperTabItem;

                    if (tab != null)
                    {
                        if (tab.Visible == true)
                            SelectedTab = tab;
                    }
                    else
                    {
                        SelectedTab = null;
                    }
                }
            }
        }

        #endregion

        #region TabAlignment

        /// <summary>
        /// Gets or sets the tab alignment
        /// </summary>
        public eTabStripAlignment TabAlignment
        {
            get { return (_TabAlignment); }

            set
            {
                if (_TabAlignment != value)
                {
                    _TabAlignment = value;

                    SyncOrientation();
                    MyRefresh();
                }
            }
        }

        #region SyncOrientation

        /// <summary>
        /// Syncs the Orientation of each SubItem to
        /// the current TabAlignment Orientation
        /// </summary>
        private void SyncOrientation()
        {
            eDesignMarkerOrientation orientation = GetTabAlignmentBasedOrientation();

            foreach (BaseItem item in SubItems)
                item.DesignMarkerOrientation = orientation;
        }

        #endregion

        #region GetTabAlignmentBasedOrientation

        /// <summary>
        /// Gets the Orientation based upon the current
        /// TabAlignment
        /// </summary>
        /// <returns></returns>
        private eDesignMarkerOrientation GetTabAlignmentBasedOrientation()
        {
            return ((IsVertical == true)
                ? eDesignMarkerOrientation.Vertical
                : eDesignMarkerOrientation.Horizontal);
        }

        #endregion

        #region OnItemAdded

        /// <summary>
        /// Makes sure all newly added items
        /// are set to the design orientation and style
        /// </summary>
        /// <param name="item"></param>
        protected internal override void OnItemAdded(BaseItem item)
        {
            item.DesignMarkerOrientation = GetTabAlignmentBasedOrientation();
            item.Style = GetStyleFromTabStyle();

            base.OnItemAdded(item);
        }

        #endregion

        #endregion

        #region TabLayoutType

        /// <summary>
        /// Gets or sets the TabLayoutType
        /// </summary>
        public eSuperTabLayoutType TabLayoutType
        {
            get { return (_TabLayoutType); }

            set
            {
                _TabLayoutType = value;
                _TabControlBox.Bounds = Rectangle.Empty;

                OnTabStyleChanged();

                MyRefresh();
            }
        }

        #endregion

        #region TabStripColor

        /// <summary>
        /// Gets or sets the TabStripColor
        /// </summary>
        public SuperTabColorTable TabStripColor
        {
            get { return (_TabStripColor); }

            set
            {
                if (_TabStripColor.Equals(value) == false)
                {
                    if (_TabStripColor != null)
                        _TabStripColor.ColorTableChanged -= ColorTable_ColorTableChanged;

                    _TabStripColor = value;

                    if (value != null)
                        _TabStripColor.ColorTableChanged += ColorTable_ColorTableChanged;

                    OnTabStripColorChanged();

                    MyRefresh();
                }
            }
        }

        #endregion

        #region TabStyle

        /// <summary>
        /// Gets or sets the TabStyle
        /// </summary>
        public eSuperTabStyle TabStyle
        {
            get { return (_TabStyle); }

            set
            {
                if (_TabStyle != value)
                {
                    _TabStyle = value;

                    switch (value)
                    {
                        case eSuperTabStyle.Office2007:
                            _TabDisplay = new Office2007SuperTabStrip(this);
                            break;

                        case eSuperTabStyle.Office2010BackstageBlue:
                            _TabDisplay = new Office2010BackstageSuperTabStrip(this);
                            break;

                        case eSuperTabStyle.OneNote2007:
                            _TabDisplay = new OneNote2007SuperTabStrip(this);
                            break;

                        case eSuperTabStyle.VisualStudio2008Dock:
                            _TabDisplay = new VS2008DockSuperTabStrip(this);
                            break;

                        case eSuperTabStyle.VisualStudio2008Document:
                            _TabDisplay = new VS2008DocumentSuperTabStrip(this);
                            break;

                        case eSuperTabStyle.WinMediaPlayer12:
                            _TabDisplay = new WinMediaPlayer12SuperTabStrip(this);
                            break;
                    }

                    ApplyTabStyle();
                }
            }
        }

        #region ApplyTabStyle

        /// <summary>
        /// Applies the current TabStyle to each tab and
        /// sets the item style to the DotNetBarStyle from the TabStyle
        /// </summary>
        private void ApplyTabStyle()
        {
            eDotNetBarStyle style = GetStyleFromTabStyle();

            foreach (BaseItem item in SubItems)
            {
                SuperTabItem tab = item as SuperTabItem;

                if (tab != null)
                    tab.TabStyle = _TabStyle;

                item.Style = style;
            }

            TabStrip.RecalcLayout();
        }

        #endregion

        #region GetStyleFromTabStyle

        /// <summary>
        /// Gets the DotNetBarStyle from the SuperTabStyle
        /// </summary>
        /// <returns>eDotNetBarStyle</returns>
        private eDotNetBarStyle GetStyleFromTabStyle()
        {
            eDotNetBarStyle style = eDotNetBarStyle.StyleManagerControlled;
            eSuperTabStyle tabStyle = _TabStyle;

            if (tabStyle == eSuperTabStyle.VisualStudio2008Dock ||
                tabStyle == eSuperTabStyle.VisualStudio2008Document)
            {
                style = eDotNetBarStyle.VS2005;
            }

            return style;
        }

        #endregion

        #endregion

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets the TextAlignment
        /// </summary>
        public eItemAlignment TextAlignment
        {
            get { return (_TextAlignment); }

            set
            {
                if (_TextAlignment != value)
                {
                    _TextAlignment = value;

                    Refresh();
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region HotTab

        /// <summary>
        /// Gets the HotTab
        /// </summary>
        internal SuperTabItem HotTab
        {
            get { return (_HotTab); }
        }

        #endregion

        #region InsertTab

        /// <summary>
        /// Gets the InsertTab
        /// </summary>
        internal BaseItem InsertTab
        {
            get { return (_InsertTab); }
        }

        #endregion

        #region InsertBefore

        /// <summary>
        /// Gets whether to insert before or after
        /// </summary>
        internal bool InsertBefore
        {
            get { return (_InsertBefore); }
        }

        #endregion

        #region MouseOverTab

        /// <summary>
        /// Gets the MouseOver tab
        /// </summary>
        internal SuperTabItem MouseOverTab
        {
            get { return (_HotTab); }
        }

        #endregion

        #region IsVertical

        /// <summary>
        /// Gets TabStrip vertical orientation
        /// </summary>
        internal bool IsVertical
        {
            get
            {
                return (_TabAlignment == eTabStripAlignment.Left ||
                    _TabAlignment == eTabStripAlignment.Right);
            }
        }

        #endregion

        #region TabCloseButtonSize

        /// <summary>
        /// Gets the tab close button size
        /// </summary>
        internal Size TabCloseButtonSize
        {
            get { return (_TabCloseButtonSize); }
        }

        #endregion

        #region TabDisplay

        /// <summary>
        /// Gets the TabDisplay
        /// </summary>
        internal SuperTabStripBaseDisplay TabDisplay
        {
            get { return (_TabDisplay); }
        }

        #endregion

        #region TabItemsBounds

        /// <summary>
        /// Gets or sets the TabItemsBounds
        /// </summary>
        internal Rectangle TabItemsBounds
        {
            get { return (_TabItemsBounds); }
            set { _TabItemsBounds = value; }
        }

        #endregion

        #region TabLines

        /// <summary>
        /// Gets or sets the number of TabLines
        /// </summary>
        internal int TabLines
        {
            get { return (_TabLines); }
            set { _TabLines = value; }
        }

        #endregion

        #region TabStrip

        /// <summary>
        /// Gets the TabStrip
        /// </summary>
        internal SuperTabStrip TabStrip
        {
            get { return (_TabStrip); }
        }

        #endregion

        #region VisibleTabCount

        /// <summary>
        /// Gets the visible tab count
        /// </summary>
        internal int VisibleTabCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < SubItems.Count; i++)
                {
                    if (SubItems[i].Visible)
                        count++;
                }

                return (count);
            }
        }

        #endregion

        #region VisibleTab

        /// <summary>
        /// Gets or sets the promoted visible tab
        /// </summary>
        internal BaseItem VisibleTab
        {
            get { return (_VisibleTab ?? _SelectedTab); }

            set
            {
                if (_VisibleTab != value)
                {
                    _VisibleTab = value;

                    MyRefresh();
                }
            }
        }

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks or unhooks our events
        /// </summary>
        /// <param name="hook"></param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
                SubItemsChanged += SuperTabStripItem_SubItemsChanged;
            else
                SubItemsChanged -= SuperTabStripItem_SubItemsChanged;
        }

        #endregion

        #region Event processing

        #region SubItemsChanged

        /// <summary>
        /// Handles SubItemsChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SuperTabStripItem_SubItemsChanged(object sender, CollectionChangeEventArgs e)
        {
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    TabItemAdded(e.Element as BaseItem);
                    break;

                case CollectionChangeAction.Remove:
                    TabItemRemoved(e.Element as BaseItem);
                    break;

                case CollectionChangeAction.Refresh:
                    MyRefresh();
                    break;
            }
        }

        #endregion

        #region TabItemAdded

        /// <summary>
        /// Handles newly added items
        /// </summary>
        /// <param name="item"></param>
        internal void TabItemAdded(BaseItem item)
        {
            SuperTabItem tab = item as SuperTabItem;

            if (tab != null)
            {
                tab.TabStripItem = this;
                tab.Visible = true;

                tab.TabColorChanged += Tab_TabColorChanged;

                if (TabItemOpen != null)
                {
                    SuperTabStripTabItemOpenEventArgs args =
                        new SuperTabStripTabItemOpenEventArgs(tab);

                    TabItemOpen(this, args);
                }

                if (VisibleTabCount == 1 && SelectedTab == null)
                    SelectedTab = tab;
            }

            MyRefresh();
        }

        #endregion

        #region TabRemoved

        /// <summary>
        /// Handles newly removed items
        /// </summary>
        /// <param name="item"></param>
        internal void TabItemRemoved(BaseItem item)
        {
            if (TabRemoved != null)
            {
                SuperTabStripTabRemovedEventArgs args = new
                    SuperTabStripTabRemovedEventArgs(item);

                TabRemoved(this, args);
            }

            SuperTabItem tab = item as SuperTabItem;

            if (tab != null)
            {
                tab.TabColorChanged -= Tab_TabColorChanged;

                SuperTabControlPanel panel = tab.AttachedControl as SuperTabControlPanel;

                if (panel != null)
                    panel.PanelColorChanged -= Tab_TabColorChanged;

                if (SelectedTab == tab)
                    OnSelectedTabRemoved(_SelectedTabIndex, tab);

                if (VisibleTabCount == 0)
                    SelectedTab = null;
            }

            Refresh();
        }

        #endregion

        #region Tab_TabColorChanged

        /// <summary>
        /// Handles tab color changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Tab_TabColorChanged(object sender, EventArgs e)
        {
            if (TabStripTabColorChanged != null)
            {
                SuperTabItem tab = sender as SuperTabItem;

                if (tab != null)
                {
                    TabStripTabColorChanged(this,
                        new SuperTabStripTabColorChangedEventArgs(tab));
                }
            }

            Refresh();
        }

        #endregion

        #region ColorTable_ColorTableChanged

        /// <summary>
        /// Handles color table changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ColorTable_ColorTableChanged(object sender, EventArgs e)
        {
            OnTabStripColorChanged();
        }

        #endregion

        #endregion

        #region MyRefresh

        /// <summary>
        /// Performs NeedRecalcSize and refresh
        /// </summary>
        private void MyRefresh()
        {
            NeedRecalcSize = true;
            Refresh();
        }

        #endregion

        #region RecalcSize

        /// <summary>
        /// Recalculates the size of the tabs.
        /// </summary>
        public override void RecalcSize()
        {
            if (SuspendLayout == false)
            {
                if (_RecalcInProgress == false)
                {
                    try
                    {
                        _RecalcInProgress = true;

                        RecalcDisplaySize();
                    }
                    finally
                    {
                        _RecalcInProgress = false;
                    }
                }
            }
        }

        #region RecalcDisplaySize

        private void RecalcDisplaySize()
        {
            base.RecalcSize();

            Control container = this.ContainerControl as Control;

            if (container != null)
            {
                using (Graphics g = BarFunctions.CreateGraphics(container))
                {
                    if (_TabControlBox.Visible == true)
                    {
                        _TabControlBox.RecalcSize();

                        if (DesignMode == false)
                            _TabControlBox.Displayed = true;
                    }

                    _TabDisplay.RecalcSize(g);
                }
            }
        }

        #endregion

        #endregion

        #region EnsureVisible

        /// <summary>
        /// Ensures that the given item is visible on the TabStrip
        /// </summary>
        /// <param name="item"></param>
        public void EnsureVisible(BaseItem item)
        {
            VisibleTab = item;
        }

        #endregion

        #region SelectTab

        /// <summary>
        /// Selects the given tab
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="eventSource"></param>
        public void SelectTab(SuperTabItem tab, eEventSource eventSource)
        {
            bool refresh = (_VisibleTab != tab || _SelectedTab != tab);

            _VisibleTab = tab;

            if (_SelectedTab != tab)
            {
                SuperTabItem oldTab = _SelectedTab;

                if (OnSelectedTabChanging(tab, eventSource) == false)
                {
                    OnBeforeTabDisplay(tab);

                    Control hideControl = null;

                    if (_SelectedTab != null && _SelectedTab.AttachedControl != null)
                        hideControl = _SelectedTab.AttachedControl;

                    _SelectedTab = tab;
                    _SelectedTabIndex = SubItems.IndexOf(tab);
                    
                    if (_SelectedTab != null && _SelectedTab.AttachedControl != null)
                    {
                        _SelectedTab.AttachedControl.BringToFront();
                        _SelectedTab.AttachedControl.Visible = true;

                        SuperTabControl tc = _TabStrip.Parent as SuperTabControl;

                        if (tc != null)
                        {
                            bool select = !(tc.TabStop && tc.Focused && eventSource == eEventSource.Keyboard);

                            if (_AutoSelectAttachedControl && select)
                                _SelectedTab.AttachedControl.Select();
                        }

                        if (hideControl == _SelectedTab.AttachedControl)
                            hideControl = null;
                    }

                    if (hideControl != null)
                        hideControl.Visible = false;

                    OnSelectedTabChanged(oldTab, _SelectedTab, eventSource);
                }
            }

            if (refresh == true)
                MyRefresh();
        }

        #endregion

        #region SelectPreviousTab

        /// <summary>
        /// Selects the previous tab
        /// </summary>
        /// <returns></returns>
        public bool SelectPreviousTab()
        {
            return SelectPreviousTab(eEventSource.Code);
        }

        /// <summary>
        /// Selects the previous tab
        /// </summary>
        /// <returns></returns>
        private bool SelectPreviousTab(eEventSource eventSource)
        {
            if (SelectedTab != null && SubItems.IndexOf(SelectedTab) > 0)
            {
                for (int i = SubItems.IndexOf(SelectedTab) - 1; i >= 0; i--)
                {
                    if (SubItems[i].Visible)
                    {
                        SelectTab(SubItems[i] as SuperTabItem, eventSource);
                        return (true);
                    }
                }
            }

            return (false);
        }

        #endregion

        #region SelectNextTab

        /// <summary>
        /// Selects the next tab
        /// </summary>
        /// <returns></returns>
        public bool SelectNextTab()
        {
            return SelectNextTab(eEventSource.Code);
        }

        /// <summary>
        /// Selects the next tab
        /// </summary>
        /// <returns></returns>
        private bool SelectNextTab(eEventSource eventSource)
        {
            if (SelectedTab != null && SubItems.IndexOf(SelectedTab) < SubItems.Count)
            {
                for (int i = SubItems.IndexOf(SelectedTab) + 1; i < SubItems.Count; i++)
                {
                    if (SubItems[i].Visible)
                    {
                        SelectTab(SubItems[i] as SuperTabItem, eventSource);
                        return (true);
                    }
                }
            }

            return (false);
        }

        #endregion

        #region UpdateSelectedTab

        /// <summary>
        /// UpdateSelectedTab
        /// </summary>
        internal void UpdateSelectedTab()
        {
            if (SelectedTab != null)
            {
                if (SelectedTab.Visible == false)
                    SelectedTab = GetNextVisibleTab(SelectedTabIndex);
            }
            else
            {
                SelectedTab = GetNextVisibleTab(0);
            }

            RecalcSize();
        }

        #endregion

        #region "On" processing

        #region OnTabStyleChanged

        /// <summary>
        /// OnTabStyleChanged
        /// </summary>
        private void OnTabStyleChanged()
        {
            MyRefresh();

            EnsureVisible(this.SelectedTab);
        }

        #endregion

        #region OnMeasureTabItem

        /// <summary>
        /// OnMeasureTabItem
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="size"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        internal Size OnMeasureTabItem(SuperTabItem tab, Size size, Graphics g)
        {
            if (MeasureTabItem != null)
            {
                SuperTabMeasureTabItemEventArgs args =
                    new SuperTabMeasureTabItemEventArgs(tab, size, g);

                MeasureTabItem(this, args);

                return (args.Size);
            }

            return (size);
        }

        #endregion

        #region OnPreRenderTabItem

        /// <summary>
        /// OnPreRenderTabItem
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        internal bool OnPreRenderTabItem(SuperTabItem tab, Graphics g)
        {
            if (PreRenderTabItem != null)
            {
                SuperTabPreRenderTabItemEventArgs args =
                    new SuperTabPreRenderTabItemEventArgs(tab, g);

                PreRenderTabItem(this, args);

                return (args.Cancel);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderTabItem

        /// <summary>
        /// OnPostRenderTabItem
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="g"></param>
        internal void OnPostRenderTabItem(SuperTabItem tab, Graphics g)
        {
            if (PostRenderTabItem != null)
            {
                SuperTabPostRenderTabItemEventArgs args =
                    new SuperTabPostRenderTabItemEventArgs(tab, g);

                PostRenderTabItem(this, args);
            }
        }

        #endregion

        #region OnGetTabItemContentRectangle

        /// <summary>
        /// OnGetTabItemContentRectangle
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        internal Rectangle OnGetTabItemContentRectangle(SuperTabItem tab)
        {
            if (GetTabItemContentRectangle != null)
            {
                SuperTabGetTabItemContentRectangleEventArgs args =
                    new SuperTabGetTabItemContentRectangleEventArgs(tab);

                GetTabItemContentRectangle(this, args);

                return (args.ContentRectangle);
            }

            return (tab.TabItemDisplay.ContentRectangle());
        }

        #endregion

        #region OnGetTabItemPath

        /// <summary>
        /// OnGetTabItemPath
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        internal GraphicsPath OnGetTabItemPath(SuperTabItem tab)
        {
            if (GetTabItemPath != null)
            {
                SuperTabGetTabItemPathEventArgs args =
                    new SuperTabGetTabItemPathEventArgs(tab);

                GetTabItemPath(this, args);

                return (args.Path);
            }

            return (null);
        }

        #endregion

        #region OnSelectedTabRemoved

        /// <summary>
        /// OnSelectedTabRemoved
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        internal void OnSelectedTabRemoved(int index, SuperTabItem item)
        {
            if (SubItems.Count > 0)
            {
                if (index >= SubItems.Count)
                    index = SubItems.Count - 1;

                SelectedTab = GetNextVisibleTab(index) ?? GetNextVisibleTab(0);
            }
            else
            {
                SelectedTab = null;
            }
        }

        #endregion

        #region OnSelectedTabChanging

        /// <summary>
        /// OnSelectedTabChanging
        /// </summary>
        /// <param name="value"></param>
        /// <param name="eventSource"></param>
        /// <returns></returns>
        private bool OnSelectedTabChanging(SuperTabItem value, eEventSource eventSource)
        {
            if (SelectedTabChanging != null)
            {
                SuperTabStripSelectedTabChangingEventArgs eventData = new
                    SuperTabStripSelectedTabChangingEventArgs(_SelectedTab, value, eventSource);

                SelectedTabChanging(this, eventData);

                return (eventData.Cancel);
            }

            return (false);
        }

        #endregion

        #region OnSelectedTabChanged

        /// <summary>
        /// OnSelectedTabChanged
        /// </summary>
        /// <param name="oldTab"></param>
        /// <param name="newTab"></param>
        /// <param name="eventSource"></param>
        private void OnSelectedTabChanged(SuperTabItem oldTab,
            SuperTabItem newTab, eEventSource eventSource)
        {
            if (SelectedTabChanged != null)
            {
                SuperTabStripSelectedTabChangedEventArgs eventData = new
                    SuperTabStripSelectedTabChangedEventArgs(oldTab, newTab, eventSource);

                SelectedTabChanged(this, eventData);
            }
        }

        #endregion

        #region OnBeforeTabDisplay

        /// <summary>
        /// OnBeforeTabDisplay
        /// </summary>
        /// <param name="item"></param>
        internal void OnBeforeTabDisplay(SuperTabItem item)
        {
            if (BeforeTabDisplay != null)
            {
                SuperTabStripBeforeTabDisplayEventArgs args = new
                    SuperTabStripBeforeTabDisplayEventArgs(item);

                BeforeTabDisplay(this, args);
            }
        }

        #endregion

        #region OnTabItemClose

        /// <summary>
        /// OnTabItemClose
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        private bool OnTabItemClose(SuperTabItem tab)
        {
            if (TabItemClose != null)
            {
                SuperTabStripTabItemCloseEventArgs args =
                    new SuperTabStripTabItemCloseEventArgs(tab);

                args.Cancel = (AutoCloseTabs == false);

                TabItemClose(this, args);

                return (args.Cancel);
            }

            return (AutoCloseTabs == false);
        }

        #endregion

        #region OnTabStripColorChanged

        /// <summary>
        /// OnTabStripColorChanged
        /// </summary>
        private void OnTabStripColorChanged()
        {
            if (TabStripColorChanged != null)
                TabStripColorChanged(this, EventArgs.Empty);

            MyRefresh();
        }

        #endregion

        #region OnPaintBackground

        /// <summary>
        /// OnPaintBackground
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ct"></param>
        internal void OnPaintBackground(Graphics g, SuperTabColorTable ct)
        {
            if (TabStripPaintBackground != null)
                TabStripPaintBackground(this, new SuperTabStripPaintBackgroundEventArgs(g, ct));
        }

        #endregion

        #region OnGetTextBounds

        /// <summary>
        /// OnGetTextBounds
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        internal Rectangle OnGetTextBounds(SuperTabItem tab, Rectangle bounds)
        {
            if (GetTabTextBounds != null)
            {
                SuperTabGetTabTextBoundsEventArgs ev = new
                    SuperTabGetTabTextBoundsEventArgs(tab, bounds);

                GetTabTextBounds(this, ev);

                bounds = ev.Bounds;
            }

            return (bounds);
        }

        #endregion

        #region OnGetImageBounds

        /// <summary>
        /// OnGetImageBounds
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        internal Rectangle OnGetImageBounds(SuperTabItem tab, Rectangle bounds)
        {
            if (GetTabImageBounds != null)
            {
                SuperTabGetTabImageBoundsEventArgs ev = new
                    SuperTabGetTabImageBoundsEventArgs(tab, bounds);

                GetTabImageBounds(this, ev);

                bounds = ev.Bounds;
            }

            return (bounds);
        }

        #endregion

        #region OnGetCloseBounds

        /// <summary>
        /// OnGetCloseBounds
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        internal Rectangle OnGetCloseBounds(SuperTabItem tab, Rectangle bounds)
        {
            if (GetTabCloseBounds != null)
            {
                SuperTabGetTabCloseBoundsEventArgs ev = new
                    SuperTabGetTabCloseBoundsEventArgs(tab, bounds);

                GetTabCloseBounds(this, ev);

                bounds = ev.Bounds;
            }

            return (bounds);
        }

        #endregion

        #region OnTabMoving

        /// <summary>
        /// OnTabMoving
        /// </summary>
        /// <param name="moveTab"></param>
        /// <param name="insertTab"></param>
        /// <param name="insertBefore"></param>
        /// <returns></returns>
        protected bool OnTabMoving(
            BaseItem moveTab, BaseItem insertTab, bool insertBefore)
        {
            if (TabMoving != null)
            {
                SuperTabStripTabMovingEventArgs ev =
                    new SuperTabStripTabMovingEventArgs(moveTab, insertTab, insertBefore);

                TabMoving(this, ev);

                return (ev.CanMove);
            }

            return (true);
        }

        #endregion

        #region OnTabMoved

        /// <summary>
        /// OnTabMoved
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        protected bool OnTabMoved(SuperTabItem tab)
        {
            if (TabMoved != null)
            {
                SuperTabStripTabMovedEventArgs ev =
                    new SuperTabStripTabMovedEventArgs(tab);

                TabMoved(this, ev);

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #endregion

        #region ItemAtLocation

        /// <summary>
        /// ItemAtLocation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override BaseItem ItemAtLocation(int x, int y)
        {
            Point pt = new Point(x, y);

            return (GetTabFromPoint(pt) ?? GetItemFromPoint(pt));
        }

        #endregion

        #region Mouse support

        #region InternalClick

        /// <summary>
        /// InternalClick
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="pt"></param>
        public override void InternalClick(MouseButtons mb, Point pt)
        {
            base.InternalClick(mb, pt);

            SuperTabItem tab = GetTabFromPoint(pt);

            if (tab != null)
                tab.InternalClick(mb, pt);
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
            InternalOnMouseDown(objArg);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalOnMouseDown(MouseEventArgs objArg)
        {
            HideToolTip();

            _TabArea = eSuperTabArea.InNone;

            SuperTabItem tab = GetTabFromPoint(objArg.Location);

            _MouseDownTab = tab;
            _MouseDownLocation = objArg.Location;

            if (tab != null)
            {
                tab.InternalMouseDown(objArg);

                if (objArg.Button == MouseButtons.Left)
                {
                    _TabArea = tab.GetTabAreaFromPoint(objArg.Location);

                    if (_TabArea == eSuperTabArea.InContent || _TabArea == eSuperTabArea.InImage)
                    {
                        if (_SelectedTab != tab)
                            SelectedTab = tab;
                    }
                }
            }
            else
            {
                if (ControlBox.Visible == true)
                {
                    if (ControlBox.IsMouseOver == true)
                        ControlBox.InternalMouseDown(objArg);
                }
            }
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

            InternalOnMouseUp(objArg);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalOnMouseUp(MouseEventArgs objArg)
        {
            if (_IsTabDragging == false)
                ProcessMouseUp(objArg);
            else
                ProcessTabUp();

            _InsertTab = null;
            _MouseDownTab = null;
            _IsTabDragging = false;
        }

        #region ProcessMouseUp

        private void ProcessMouseUp(MouseEventArgs objArg)
        {
            SuperTabItem tab = GetTabFromPoint(objArg.Location);

            if (tab != null)
            {
                if (objArg.Button == MouseButtons.Left)
                {
                    eSuperTabArea upTabArea = tab.GetTabAreaFromPoint(objArg.Location);

                    if (_TabArea == eSuperTabArea.InCloseBox && upTabArea == eSuperTabArea.InCloseBox)
                    {
                        CloseTab(tab);
                    }
                }
            }
            else
            {
                if (ControlBox.Visible == true)
                {
                    if (ControlBox.IsMouseDown == true)
                        ControlBox.InternalMouseUp(objArg);
                }
            }
        }

        #endregion

        #region ProcessTabUp

        private void ProcessTabUp()
        {
            _TabDragWindow.Owner.Cursor = Cursors.Default;

            _TabDragWindow.Dispose();
            _TabDragWindow = null;

            if (_InsertTab != null)
            {
                SuperTabItem dragTab = _HotTab;

                if (OnTabMoved(dragTab) == false)
                {
                    int n = SubItems.IndexOf(_InsertTab);
                    int m = SubItems.IndexOf(dragTab);

                    if (n >= 0 && m >= 0)
                    {
                        if ((_InsertBefore == false && n + 1 != m) ||
                            (_InsertBefore == true && n - 1 != m))
                        {
                            SubItems.Remove(dragTab);

                            n = SubItems.IndexOf(_InsertTab);

                            if (_InsertBefore == false)
                                n++;

                            SubItems.Insert(n, dragTab);

                            SelectedTab = dragTab;
                        }
                    }
                }
            }

            Refresh();
        }

        #endregion

        #endregion

        #region InternalMouseMove

        /// <summary>
        /// InternalMouseMove
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            if (_IsTabDragging == false)
                base.InternalMouseMove(objArg);

            InternalOnMouseMove(objArg);
        }

        /// <summary>
        /// InternalOnMouseMove
        /// </summary>
        /// <param name="objArg"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalOnMouseMove(MouseEventArgs objArg)
        {
            if (_IsTabDragging == false)
                ProcessMouseMove(objArg);
            else
                ProcessTabMove(objArg);
        }

        #region ProcessMouseMove

        /// <summary>
        /// ProcessMouseMove
        /// </summary>
        /// <param name="objArg"></param>
        private void ProcessMouseMove(MouseEventArgs objArg)
        {
            SuperTabItem tab = GetTabFromPoint(objArg.Location);

            if (_HotTab != tab || _HotTab != null)
                HotTabMouseMove(objArg, tab);
            else
                ControlBoxMouseMove(objArg);
        }

        #region HotTabMouseMove

        /// <summary>
        /// HotTabMouseMove
        /// </summary>
        /// <param name="objArg"></param>
        /// <param name="tab"></param>
        private void HotTabMouseMove(MouseEventArgs objArg, SuperTabItem tab)
        {
            if (_HotTab != tab)
            {
                if (_HotTab != null) _HotTab.Refresh();
                if (_HotTab == null || _HotTab.CloseButtonPressed == false)
                {
                    if (_HotTab != null)
                        _HotTab.InternalMouseLeave();

                    _HotTab = tab;

                    if (_HotTab != null)
                    {
                        _HotTab.InternalMouseEnter();
                        _HotTab.InternalMouseMove(objArg);
                    }
                }

                if (_HotTab != null) _HotTab.Refresh();
                //Refresh();

                ResetHover();
                HideToolTip();
            }
            else if (_HotTab != null)
            {
                if (StartTabMove(objArg) == false)
                    _HotTab.InternalMouseMove(objArg);
            }
        }

        #endregion

        #region ControlBoxMouseMove

        /// <summary>
        /// ControlBoxMouseMove
        /// </summary>
        /// <param name="objArg"></param>
        private void ControlBoxMouseMove(MouseEventArgs objArg)
        {
            if (ControlBox.Visible == true)
            {
                if (ControlBox.Bounds.Contains(objArg.Location))
                {
                    if (ControlBox.IsMouseOver == false)
                        ControlBox.InternalMouseEnter();

                    ControlBox.InternalMouseMove(objArg);
                }
                else
                {
                    if (ControlBox.IsMouseOver == true)
                        ControlBox.InternalMouseLeave();
                }
            }
        }

        #endregion

        #endregion

        #region ProcessTabMove

        /// <summary>
        /// ProcessTabMove
        /// </summary>
        /// <param name="objArg"></param>
        private void ProcessTabMove(MouseEventArgs objArg)
        {
            _TabDragWindow.Location = new Point(Control.MousePosition.X,
                Control.MousePosition.Y + _TabDragWindow.Owner.Cursor.Size.Height / 2);

            _InsertTab = null;

            BaseItem item = GetItemFromPoint(objArg.Location);

            if (item != null && _HotTab != null)
            {
                Point pt = objArg.Location;

                switch (_TabStrip.TabAlignment)
                {
                    case eTabStripAlignment.Top:
                    case eTabStripAlignment.Bottom:
                        _InsertBefore = (pt.X < item.Bounds.X + (item.Bounds.Width / 2));
                        break;

                    default:
                        _InsertBefore = (pt.Y < item.Bounds.Y + (item.Bounds.Height / 2));
                        break;
                }

                int n = SubItems.IndexOf(item);
                int m = SubItems.IndexOf(_HotTab);

                if (m != n && ((_InsertBefore == true && n - 1 != m) || (_InsertBefore == false && n + 1 != m)))
                    _InsertTab = item;
            }

            if (OnTabMoving(_HotTab, _InsertTab, _InsertBefore) == false)
                _InsertTab = null;

            _TabDragWindow.Owner.Cursor =
                (_InsertTab != null) ? Cursors.Hand : Cursors.No;

            Refresh();
        }

        #endregion

        #region StartTabMove

        /// <summary>
        /// StartTabMove
        /// </summary>
        /// <param name="objArg"></param>
        /// <returns></returns>
        private bool StartTabMove(MouseEventArgs objArg)
        {
            if (_ReorderTabsEnabled == true && _HotTab == _MouseDownTab &&
                _MouseDownTab.CloseButtonPressed == false)
            {
                if (Math.Abs(_MouseDownLocation.X - objArg.Location.X) > 8 ||
                    Math.Abs(_MouseDownLocation.Y - objArg.Location.Y) > 8)
                {
                    _IsTabDragging = true;

                    _TabDragWindow = new SuperTabDragWindow();
                    _TabDragWindow.Owner = this.TabStrip.FindForm();

                    _TabDragWindow.Paint += TabDragWindow_Paint;

                    SetDragWindowRegion();
                    _TabDragWindow.Show();

                    _TabDragWindow.Size = _MouseDownTab.Bounds.Size;

                    return (true);
                }
            }

            return (false);
        }

        #endregion

        #region SetDragWindowRegion

        /// <summary>
        /// SetDragWindowRegion
        /// </summary>
        private void SetDragWindowRegion()
        {
            Rectangle saveRect = _MouseDownTab.Bounds;
            Rectangle dragRect = saveRect;

            dragRect.Location = new Point(0, 0);

            try
            {
                _MouseDownTab.Bounds = dragRect;

                using (GraphicsPath path = _MouseDownTab.GetTabItemPath())
                {
                    GraphicsPath cpath = (GraphicsPath) path.Clone();

                    using (Pen pen = new Pen(Color.Black, 2))
                        cpath.Widen(pen);

                    Region rgn = new Region(path);
                    rgn.Union(cpath);

                    _TabDragWindow.Region = rgn;
                }
            }
            finally
            {
                _MouseDownTab.Bounds = saveRect;
            }
        }

        #endregion

        #region TabDragWindow_Paint

        /// <summary>
        /// TabDragWindow_Paint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabDragWindow_Paint(object sender, PaintEventArgs e)
        {
            ItemPaintArgs p = new ItemPaintArgs(null, _TabStrip, e.Graphics, new ColorScheme());

            Rectangle saveRect = _MouseDownTab.Bounds;
            Rectangle dragRect = saveRect;

            dragRect.Location = new Point(0, 0);

            try
            {
                _MouseDownTab.Bounds = dragRect;
                _MouseDownTab.Paint(p);
            }
            finally
            {
                _MouseDownTab.Bounds = saveRect;
            }
        }

        #endregion

        #endregion

        #region InternalMouseLeave

        /// <summary>
        /// InternalMouseLeave
        /// </summary>
        public override void InternalMouseLeave()
        {
            base.InternalMouseLeave();

            InternalOnMouseLeave();
        }

        /// <summary>
        /// InternalOnMouseLeave
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalOnMouseLeave()
        {
            HideToolTip();

            if (_HotTab != null)
            {
                if (SubItems.Contains(_HotTab))
                {
                    _HotTab.CloseButtonMouseOver = false;
                    _HotTab.Refresh();

                    _HotTab = null;

                    Refresh();
                }
                else
                {
                    _HotTab = null;
                }
            }
            else
            {
                if (ControlBox.Visible == true)
                {
                    if (ControlBox.IsMouseOver == true)
                        ControlBox.InternalMouseLeave();
                }
            }
        }

        #endregion

        #endregion

        #region GetTabFromPoint

        /// <summary>
        /// Get the tab at the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>SuperTabItem or null</returns>
        public SuperTabItem GetTabFromPoint(Point pt)
        {
            if (SelectedTab != null)
            {
                if (SelectedTab.PointInTab(pt) == true)
                    return (SelectedTab);
            }

            for (int i = 0; i < SubItems.Count; i++)
            {
                SuperTabItem tab = SubItems[i] as SuperTabItem;

                if (tab != null)
                {
                    if (tab.Visible == true && tab.Displayed == true)
                    {
                        if (tab.PointInTab(pt) == true)
                            return (tab);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetItemFromPoint

        /// <summary>
        /// Gets the item at the given point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>BaseItem or null</returns>
        public BaseItem GetItemFromPoint(Point pt)
        {
            for (int i = 0; i < SubItems.Count; i++)
            {
                BaseItem item = SubItems[i];

                if (item.Visible == true && item.Displayed == true)
                {
                    if (item.DisplayRectangle.Contains(pt))
                        return (item);
                }
            }

            return (null);
        }

        #endregion

        #region GetNextVisibleTab

        /// <summary>
        /// Gets the next visible Tab
        /// </summary>
        /// <param name="index"></param>
        /// <returns>SuperTabItem</returns>
        internal SuperTabItem GetNextVisibleTab(int index)
        {
            if (index < 0 || index >= SubItems.Count)
                index = 0;

            for (int i = index; i < SubItems.Count; i++)
            {
                SuperTabItem tab = SubItems[i] as SuperTabItem;

                if (tab != null && tab.Visible == true)
                    return (tab);
            }

            return (null);
        }

        #endregion

        #region CloseTab

        /// <summary>
        /// Closes teh given tab
        /// </summary>
        /// <param name="tab"></param>
        public void CloseTab(SuperTabItem tab)
        {
            if (tab != null && SubItems.Contains(tab))
            {
                if (OnTabItemClose(tab) == false)
                {
                    if (_TabStrip.IsDesignMode == false)
                    {
                        SubItems.Remove(tab);

                        Control c = tab.AttachedControl;

                        if (c != null)
                        {
                            if (_TabStrip.Controls.Contains(c))
                                _TabStrip.Controls.Remove(c);

                            c.Dispose();
                        }

                        _TabStrip.RecalcLayout();
                    }
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
            if (_TabDisplay != null)
                _TabDisplay.Paint(p);
        }

        #endregion

        #region SubItemSizeChanged

        /// <summary>
        /// SubItemSizeChanged
        /// </summary>
        /// <param name="objChildItem"></param>
        public override void SubItemSizeChanged(BaseItem objChildItem)
        {
            base.SubItemSizeChanged(objChildItem);

            _TabStrip.RecalcLayout();

            UpdateSelectedTab();
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            SuperTabStripItem objCopy = new SuperTabStripItem(_TabStrip);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New SuperTabStripItem instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            SuperTabStripItem objCopy = copy as SuperTabStripItem;

            if (objCopy != null)
            {
                base.CopyToItem(objCopy);

                objCopy.AutoCloseTabs = _AutoCloseTabs;
                objCopy.TabCloseButtonNormal = _TabCloseButtonNormal;
                objCopy.TabCloseButtonHot = _TabCloseButtonHot;
                objCopy.TabHorizontalSpacing = _TabHorizontalSpacing;
                objCopy.TabVerticalSpacing = _TabVerticalSpacing;
                objCopy.CloseButtonOnTabsVisible = _CloseButtonOnTabsVisible;
                objCopy.CloseButtonOnTabsAlwaysDisplayed = _CloseButtonOnTabsAlwaysDisplayed;
                objCopy.CloseButtonPosition = _CloseButtonPosition;
                objCopy.DisplaySelectedTextOnly = _DisplaySelectedTextOnly;
                objCopy.TabFont = _TabFont;
                objCopy.SelectedTabFont = _SelectedTabFont;
                objCopy.HorizontalText = _HorizontalText;
                objCopy.ShowFocusRectangle = _ShowFocusRectangle;
                objCopy.FixedTabSize = _FixedTabSize;
                objCopy.ReorderTabsEnabled = _ReorderTabsEnabled;
                objCopy.TabAlignment = _TabAlignment;
                objCopy.TabLayoutType = _TabLayoutType;
                objCopy.TabStripColor = _TabStripColor;
                objCopy.TabStyle = _TabStyle;
                objCopy.TextAlignment = _TextAlignment;
            }
        }

        #endregion

        #region IDesignTimeProvider Members

        protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
        {
            InsertPosition ip = DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);

            if (ip != null)
            {
                if (ip.TargetProvider != this && dragItem is SuperTabItem)
                    return (null);
            }

            return (ip);
        }

        InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
        {
            return (GetContainerInsertPosition(pScreen, dragItem));
        }

        void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool before)
        {
            DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, before);
        }

        void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool before)
        {
            DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, before);
        }

        #endregion
    }

    #region enums

    public enum eSuperTabStyle
    {
        Office2007,
        Office2010BackstageBlue,
        OneNote2007,
        VisualStudio2008Dock,
        VisualStudio2008Document,
        WinMediaPlayer12
    }

    public enum eSuperTabLayoutType
    {
        SingleLine,
        SingleLineFit,
        MultiLine,
        MultiLineFit
    }

    #endregion

    #region ValueChangingEventArgs

    /// <summary>
    /// Generic ValueChangingEventArgs
    /// </summary>
    /// <typeparam name="T1">oldValue type</typeparam>
    /// <typeparam name="T2">newValue type</typeparam>
    public class ValueChangingEventArgs<T1, T2> : CancelEventArgs
    {
        #region Private variables

        private T1 _OldValue;
        private T2 _NewValue;

        #endregion

        public ValueChangingEventArgs(T1 oldValue, T2 newValue)
        {
            _OldValue = oldValue;
            _NewValue = newValue;
        }

        #region Public properties

        /// <summary>
        /// Gets the old value
        /// </summary>
        public T1 OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// Gets the new value
        /// </summary>
        public T2 NewValue
        {
            get { return (_NewValue); }
        }

        #endregion
    }

    #endregion

    #region ValueChangingSourceEventArgs

    /// <summary>
    /// Generic ValueChangingSourceEventArgs
    /// </summary>
    /// <typeparam name="T1">oldValue type</typeparam>
    /// <typeparam name="T2">newValue type</typeparam>
    /// <typeparam name="T3">EventSource</typeparam>
    public class ValueChangingSourceEventArgs<T1, T2, T3> : CancelEventArgs
    {
        #region Private variables

        private T1 _OldValue;
        private T2 _NewValue;
        private T3 _EventSource;

        #endregion

        public ValueChangingSourceEventArgs(T1 oldValue, T2 newValue, T3 eventSource)
        {
            _OldValue = oldValue;
            _NewValue = newValue;
            _EventSource = eventSource;
        }

        #region Public properties

        /// <summary>
        /// Gets the old value
        /// </summary>
        public T1 OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// Gets the new value
        /// </summary>
        public T2 NewValue
        {
            get { return (_NewValue); }
        }

        /// <summary>
        /// Gets the eventSource
        /// </summary>
        public T3 EventSource
        {
            get { return (_EventSource); }
        }

        #endregion
    }

    #endregion

    #region ValueChangedEventArgs

    /// <summary>
    /// Generic ValueChangedEventArgs
    /// </summary>
    /// <typeparam name="T1">oldValue type</typeparam>
    /// <typeparam name="T2">newValue type</typeparam>
    public class ValueChangedEventArgs<T1, T2> : EventArgs
    {
        #region Private variables

        private T1 _OldValue;
        private T2 _NewValue;

        #endregion

        public ValueChangedEventArgs(T1 oldValue, T2 newValue)
        {
            _OldValue = oldValue;
            _NewValue = newValue;
        }

        #region Public properties

        /// <summary>
        /// Gets the old value
        /// </summary>
        public T1 OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// Gets the new value
        /// </summary>
        public T2 NewValue
        {
            get { return (_NewValue); }
        }

        #endregion
    }

    #endregion

    #region ValueChangedSourceEventArgs

    /// <summary>
    /// Generic ValueChangedSourceEventArgs
    /// </summary>
    /// <typeparam name="T1">oldValue type</typeparam>
    /// <typeparam name="T2">newValue type</typeparam>
    /// <typeparam name="T3">EventSource</typeparam>
    public class ValueChangedEventArgs<T1, T2, T3> : EventArgs
    {
        #region Private variables

        private T1 _OldValue;
        private T2 _NewValue;
        private T3 _EventSource;

        #endregion

        public ValueChangedEventArgs(T1 oldValue, T2 newValue, T3 eventSource)
        {
            _OldValue = oldValue;
            _NewValue = newValue;
            _EventSource = eventSource;
        }

        #region Public properties

        /// <summary>
        /// Gets the old value
        /// </summary>
        public T1 OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// Gets the new value
        /// </summary>
        public T2 NewValue
        {
            get { return (_NewValue); }
        }

        /// <summary>
        /// Gets the eventSource
        /// </summary>
        public T3 EventSource
        {
            get { return (_EventSource); }
        }

        #endregion
    }

    #endregion

    #region TabActionEventArgs

    public class TabActionEventArgs : EventArgs
    {
        #region Private variables

        private BaseItem _Tab;

        #endregion

        public TabActionEventArgs(BaseItem tab)
        {
            _Tab = tab;
        }

        #region Public properties

        /// <summary>
        /// Gets the tab
        /// </summary>
        public BaseItem Tab
        {
            get { return (_Tab); }
        }

        #endregion
    }

    #endregion

    #region SuperTabStripTabActionCancelEventArgs

    public class SuperTabStripTabActionCancelEventArgs : CancelEventArgs
    {
        #region Private variables

        private BaseItem _Tab;

        #endregion

        public SuperTabStripTabActionCancelEventArgs(BaseItem tab)
        {
            _Tab = tab;
        }

        #region Public properties

        /// <summary>
        /// Gets the tab
        /// </summary>
        public BaseItem Tab
        {
            get { return (_Tab); }
        }

        #endregion
    }

    #endregion

    #region SuperTabStripSelectedTabChangingEventArgs

    public class SuperTabStripSelectedTabChangingEventArgs : ValueChangingSourceEventArgs<BaseItem, BaseItem, eEventSource>
    {
        public SuperTabStripSelectedTabChangingEventArgs(BaseItem oldValue, BaseItem newValue, eEventSource source)
            : base(oldValue, newValue, source)
        {
        }
    }

    #endregion

    #region SuperTabStripSelectedTabChangedEventArgs

    public class SuperTabStripSelectedTabChangedEventArgs : ValueChangingSourceEventArgs<BaseItem, BaseItem, eEventSource>
    {
        public SuperTabStripSelectedTabChangedEventArgs(BaseItem oldValue, BaseItem newValue, eEventSource source)
            : base(oldValue, newValue, source)
        {
        }
    }

    #endregion

    #region SuperTabStripBeforeTabDisplayEventArgs

    public class SuperTabStripBeforeTabDisplayEventArgs : TabActionEventArgs
    {
        public SuperTabStripBeforeTabDisplayEventArgs(BaseItem tab)
            : base(tab)
        {
        }
    }

    #endregion

    #region SuperTabStripTabRemovedEventArgs

    public class SuperTabStripTabRemovedEventArgs : TabActionEventArgs
    {
        public SuperTabStripTabRemovedEventArgs(BaseItem tab)
            : base(tab)
        {
        }
    }

    #endregion

    #region SuperTabStripTabMovingEventArgs

    public class SuperTabStripTabMovingEventArgs : EventArgs
    {
        #region Private variables

        private BaseItem _MoveTab;
        private BaseItem _InsertTab;
        private bool _InsertBefore;

        private bool _CanMove = true;

        #endregion

        public SuperTabStripTabMovingEventArgs(
            BaseItem moveTab, BaseItem insertTab, bool insertBefore)
        {
            _MoveTab = moveTab;
            _InsertTab = insertTab;
            _InsertBefore = insertBefore;
        }

        #region Public properties

        public BaseItem MoveTab
        {
            get { return (_MoveTab); }
        }

        public BaseItem InsertTab
        {
            get { return (_InsertTab); }
        }

        public bool InsertBefore
        {
            get { return (_InsertBefore); }
        }

        public bool CanMove
        {
            get { return (_CanMove); }
            set { _CanMove = value; }
        }

        #endregion
    }

    #endregion

    #region SuperTabStripTabMovedEventArgs

    public class SuperTabStripTabMovedEventArgs : SuperTabStripTabActionCancelEventArgs
    {
        public SuperTabStripTabMovedEventArgs(BaseItem tab)
            : base(tab)
        {
        }
    }

    #endregion

    #region SuperTabStripTabItemCloseEventArgs

    public class SuperTabStripTabItemCloseEventArgs : SuperTabStripTabActionCancelEventArgs
    {
        public SuperTabStripTabItemCloseEventArgs(BaseItem tab)
            : base(tab)
        {
        }
    }

    #endregion

    #region SuperTabStripTabItemOpenEventArgs

    public class SuperTabStripTabItemOpenEventArgs : TabActionEventArgs
    {
        public SuperTabStripTabItemOpenEventArgs(BaseItem tab)
            : base(tab)
        {
        }
    }

    #endregion

    #region SuperTabMeasureTabItemEventArgs

    public class SuperTabMeasureTabItemEventArgs : EventArgs
    {
        #region Private variables

        private SuperTabItem _Tab;
        private Size _Size;
        private Graphics _Graphics;

        #endregion

        public SuperTabMeasureTabItemEventArgs(SuperTabItem tab, Size size, Graphics graphics)
        {
            _Tab = tab;
            _Size = size;
            _Graphics = graphics;
        }

        #region Public properties

        public SuperTabItem Tab
        {
            get { return (_Tab); }
        }

        public Size Size
        {
            get { return (_Size); }
            set { _Size = value; }
        }

        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        #endregion
    }

    #endregion

    #region SuperTabPreRenderTabItemEventArgs

    public class SuperTabPreRenderTabItemEventArgs : CancelEventArgs
    {
        #region Private variables

        private SuperTabItem _Tab;
        private Graphics _Graphics;

        #endregion

        public SuperTabPreRenderTabItemEventArgs(SuperTabItem tab, Graphics graphics)
        {
            _Tab = tab;
            _Graphics = graphics;
        }

        #region Public properties

        public SuperTabItem Tab
        {
            get { return (_Tab); }
        }

        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        #endregion
    }

    #endregion

    #region SuperTabPostRenderTabItemEventArgs

    public class SuperTabPostRenderTabItemEventArgs : EventArgs
    {
        #region Private variables

        private SuperTabItem _Tab;
        private Graphics _Graphics;

        #endregion

        public SuperTabPostRenderTabItemEventArgs(SuperTabItem tab, Graphics graphics)
        {
            _Tab = tab;
            _Graphics = graphics;
        }

        #region Public properties

        public SuperTabItem Tab
        {
            get { return (_Tab); }
        }

        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        #endregion
    }

    #endregion

    #region SuperTabGetTabTextBoundsEventArgs

    public class SuperTabGetTabTextBoundsEventArgs : EventArgs
    {
        #region Private variables

        private SuperTabItem _Tab;
        private Rectangle _Bounds;

        #endregion

        public SuperTabGetTabTextBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
        {
            _Tab = tab;
            _Bounds = bounds;
        }

        #region Public properties

        public SuperTabItem Tab
        {
            get { return (_Tab); }
        }

        public Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        #endregion
    }

    #endregion

    #region SuperTabGetTabImageBoundsEventArgs

    public class SuperTabGetTabImageBoundsEventArgs : SuperTabGetTabTextBoundsEventArgs
    {
        public SuperTabGetTabImageBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
            : base(tab, bounds)
        {
        }
    }

    #endregion

    #region SuperTabGetTabCloseBoundsEventArgs

    public class SuperTabGetTabCloseBoundsEventArgs : SuperTabGetTabImageBoundsEventArgs
    {
        public SuperTabGetTabCloseBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
            : base(tab, bounds)
        {
        }
    }

    #endregion

    #region SuperTabStripPaintBackgroundEventArgs

    public class SuperTabStripPaintBackgroundEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private SuperTabColorTable _ColorTable;

        #endregion

        public SuperTabStripPaintBackgroundEventArgs(Graphics graphics, SuperTabColorTable colorTable)
        {
            _Graphics = graphics;
            _ColorTable = colorTable;
        }

        #region Public properties

        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        public SuperTabColorTable ColorTable
        {
            get { return (_ColorTable); }
        }

        #endregion
    }

    #endregion

    #region SuperTabStripTabColorChangedEventArgs

    /// <summary>
    /// TabStripTabColorChangedEventArgs
    /// </summary>
    public class SuperTabStripTabColorChangedEventArgs : EventArgs
    {
        #region Private variables

        private SuperTabItem _Tab;

        #endregion

        #region Public properties

        public SuperTabItem Tab
        {
            get { return (_Tab); }
        }

        #endregion

        public SuperTabStripTabColorChangedEventArgs(SuperTabItem tab)
        {
            _Tab = tab;
        }
    }

    #endregion    

    #region SuperTabGetTabItemPathEventArgs

    public class SuperTabGetTabItemPathEventArgs : EventArgs
    {
        #region Private variables

        private SuperTabItem _Tab;
        private GraphicsPath _Path;

        #endregion

        public SuperTabGetTabItemPathEventArgs(SuperTabItem tab)
        {
            _Tab = tab;
        }

        #region Public properties

        public SuperTabItem Tab
        {
            get { return (_Tab); }
        }

        public GraphicsPath Path
        {
            get { return (_Path); }
            set { _Path = value; }
        }

        #endregion
    }

    #endregion

    #region SuperTabGetTabItemGetContentRectangleEventArgs

    public class SuperTabGetTabItemContentRectangleEventArgs : EventArgs
    {
        #region Private variables

        private SuperTabItem _Tab;
        private Rectangle _ContentRectangle;

        #endregion

        public SuperTabGetTabItemContentRectangleEventArgs(SuperTabItem tab)
        {
            _Tab = tab;
            _ContentRectangle = tab.TabItemDisplay.ContentRectangle();
        }

        #region Public properties

        public SuperTabItem Tab
        {
            get { return (_Tab); }
        }

        public Rectangle ContentRectangle
        {
            get { return (_ContentRectangle); }
            set { _ContentRectangle = value; }
        }

        #endregion
    }

    #endregion
}
#endif