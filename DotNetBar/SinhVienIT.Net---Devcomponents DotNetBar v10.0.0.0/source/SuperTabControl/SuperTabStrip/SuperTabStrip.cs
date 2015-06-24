#if FRAMEWORK20
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    [ToolboxItem(true), ComVisible(false)]
    [Designer("DevComponents.DotNetBar.Design.SuperTabStripDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SuperTabStrip : ItemControl, ISupportInitialize
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

        #endregion

        #region Private variables

        private BaseItem _DesignTimeSelection;
        private SuperTabStripItem _TabStripItem;

        private bool _IsInitializing;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SuperTabStrip()
        {
            _TabStripItem = new SuperTabStripItem(this);

            _TabStripItem.GlobalItem = false;
            _TabStripItem.ContainerControl = this;
            _TabStripItem.Stretch = false;
            _TabStripItem.Displayed = true;
            _TabStripItem.SetOwner(this);

            SetBaseItemContainer(_TabStripItem);

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            HookEvents(true);

            StyleManager.Register(this);
        }

        #region Public properties

        #region AutoSelectAttachedControl

        /// <summary>
        /// Gets or sets whether the control attached to the TabItem.AttachedControl property
        /// is automatically selected when TabItem becomes the selected tab. Default value is true.
        /// </summary>
        [Browsable(false)]
        public bool AutoSelectAttachedControl
        {
            get { return (TabStripItem.AutoSelectAttachedControl); }
            set { TabStripItem.AutoSelectAttachedControl = value; }
        }

        #endregion

        #region Close button properties

        #region AutoCloseTabs

        /// <summary>
        /// Gets or sets whether tabs are automatically closed when a close button is clicked
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Close Button")]
        [Description("Indicates whether tab is automatically closed when close button is clicked.")]
        public bool AutoCloseTabs
        {
            get { return (_TabStripItem.AutoCloseTabs); }
            set { _TabStripItem.AutoCloseTabs = value; }
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
            get { return (_TabStripItem.CloseButtonOnTabsAlwaysDisplayed); }
            set { _TabStripItem.CloseButtonOnTabsAlwaysDisplayed = value; }
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
            get { return (_TabStripItem.CloseButtonOnTabsVisible); }
            set { _TabStripItem.CloseButtonOnTabsVisible = value; }
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
            get { return (_TabStripItem.CloseButtonPosition); }
            set { _TabStripItem.CloseButtonPosition = value; }
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
            get { return (_TabStripItem.TabCloseButtonNormal); }
            set { _TabStripItem.TabCloseButtonNormal = value; }
        }

        #endregion

        #region TabCloseButtonHot

        /// <summary>
        /// Gets or sets the custom Close button image that is used on tabs when the mouse is over the close button
        /// </summary>
        public Image TabCloseButtonHot
        {
            get { return (_TabStripItem.TabCloseButtonHot); }
            set { _TabStripItem.TabCloseButtonHot = value; }
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
            get { return (_TabStripItem.TabCloseButtonPressed); }
            set { _TabStripItem.TabCloseButtonPressed = value; }
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
            get { return (_TabStripItem.ControlBox); }
        }

        #endregion

        #region DesignTimeSelection

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseItem DesignTimeSelection
        {
            get { return (_DesignTimeSelection); }

            set
            {
                _DesignTimeSelection = value;

                Invalidate();
            }
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
            get { return (_TabStripItem.DisplaySelectedTextOnly); }
            set { _TabStripItem.DisplaySelectedTextOnly = value; }
        }

        #endregion

        #region HorizontalText

        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text is drawn horizontally regardless of tab orientation.")]
        public bool HorizontalText
        {
            get { return (_TabStripItem.HorizontalText); }
            set { _TabStripItem.HorizontalText = value; }
        }

        #endregion

        #region ImageList

        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), DefaultValue(null)]
        public ImageList ImageList
        {
            get { return (Images); }
            set { this.Images = value; }
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
            get { return (_TabStripItem.ItemPadding); }
            set { _TabStripItem.ItemPadding = value; }
        }

        #endregion

        #region ShowFocusRectangle

        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether focus rectangle is displayed when tab has input focus.")]
        public bool ShowFocusRectangle
        {
            get { return (_TabStripItem.ShowFocusRectangle); }
            set { _TabStripItem.ShowFocusRectangle = value; }
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
            get { return (_TabStripItem.FixedTabSize); }
            set { _TabStripItem.FixedTabSize = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFixedTabSize()
        {
            return (_TabStripItem.ShouldSerializeFixedTabSize());
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
            get { return (_TabStripItem.IsTabDragging); }
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
            get { return (_TabStripItem.ReorderTabsEnabled); }
            set { _TabStripItem.ReorderTabsEnabled = value; }
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
            get { return (_TabStripItem.SelectedTab); }
            set {_TabStripItem.SelectedTab = value; }
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
            get { return (_TabStripItem.SelectedTabFont); }
            set { _TabStripItem.SelectedTabFont = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectedTabFont()
        {
            return (_TabStripItem.ShouldSerializeSelectedTabFont());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelectedTabFont()
        {
            _TabStripItem.ResetSelectedTabFont();
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
            get { return (_TabStripItem.SelectedTabIndex); }
            set {_TabStripItem.SelectedTabIndex = value; }
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
            get { return (_TabStripItem.TabAlignment); }
            set {_TabStripItem.TabAlignment = value; }
        }

        #endregion

        #region TabFont

        /// <summary>
        /// Gets or sets the tab Font
        /// </summary>
        [Browsable(true), DevCoBrowsable(true),  Localizable(true), Category("Style"), DefaultValue(null)]
        [Description("Indicates the tab Font")]
        public Font TabFont
        {
            get { return (_TabStripItem.TabFont); }
            set { _TabStripItem.TabFont = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTabFont()
        {
            return (_TabStripItem.ShouldSerializeTabFont());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTabFont()
        {
            _TabStripItem.ResetTabFont();
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
            get { return (_TabStripItem.TabHorizontalSpacing); }
            set { _TabStripItem.TabHorizontalSpacing = value; }
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
            get { return (_TabStripItem.TabLayoutType); }
            set { _TabStripItem.TabLayoutType = value; }
        }

        #endregion

        #region Tabs

        /// <summary>
        /// Gets the collection of Tabs
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.SuperTabStripTabsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Data"), Description("Returns the collection of Tabs.")]
        public SubItemsCollection Tabs
        {
            get { return (_TabStripItem.SubItems); }
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
            get { return (_TabStripItem.TabStripColor); }
            set { _TabStripItem.TabStripColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTabStripColor()
        {
            return (_TabStripItem.TabStripColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTabStripColor()
        {
            _TabStripItem.TabStripColor = new SuperTabColorTable();
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
            get { return (_TabStripItem.TabStyle); }
            set { _TabStripItem.TabStyle = value; }
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
            get { return (_TabStripItem.TabVerticalSpacing); }
            set { _TabStripItem.TabVerticalSpacing = value; }
        }

        #endregion

        #endregion

        #region TabStripItem

        /// <summary>
        /// Gets the TabStrip associated TabStripItem
        /// </summary>
        [Browsable(false)]
        public SuperTabStripItem TabStripItem
        {
            get { return (_TabStripItem); }
        }

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets tab text alignment
        /// </summary>
        [Browsable(true), DefaultValue(eItemAlignment.Near)]
        [DevCoBrowsable(true), Category("Layout"), Description("Indicates tab text alignment.")]
        public eItemAlignment TextAlignment
        {
            get { return (_TabStripItem.TextAlignment); }
            set { _TabStripItem.TextAlignment = value; }
        }

        #endregion

        #endregion

        #region Internal properties

        #region IsDesignMode

        /// <summary>
        /// Gets the Design state for the control
        /// </summary>
        internal bool IsDesignMode
        {
            get
            {
                if (Parent != null && Parent is SuperTabControl && Parent.Site != null)
                    return (Parent.Site.DesignMode);

                return (DesignMode);
            }
        }

        #endregion

        #region IsInitializing

        /// <summary>
        /// Gets the ISupportInitialize state
        /// </summary>
        internal bool IsInitializing
        {
            get { return (_IsInitializing); }
        }

        #endregion

        #region TabDisplay

        /// <summary>
        /// Gets the TabDisplay
        /// </summary>
        internal SuperTabStripBaseDisplay TabDisplay
        {
            get { return (_TabStripItem.TabDisplay); }
        }

        #endregion

        #region ApplicationButton
        private Office2007StartButton _ApplicationButton;
        /// <summary>
        /// Gets or sets the reference to ribbon application button when tab control is used as backstage ribbon control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Office2007StartButton ApplicationButton
        {
            get { return _ApplicationButton; }
            set { _ApplicationButton = value; }
        }
        #endregion
        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks or unhooks TabStripItem events
        /// </summary>
        /// <param name="hook">true to hook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _TabStripItem.TabItemOpen += TabStripItem_TabItemOpen;
                _TabStripItem.TabItemClose += TabStripItem_TabItemClose;
                _TabStripItem.TabRemoved += TabStripItem_TabRemoved;
                _TabStripItem.TabMoving += TabStripItem_TabMoving;
                _TabStripItem.TabMoved += TabStripItem_TabMoved;

                _TabStripItem.GetTabItemPath += TabStripItem_GetTabItemPath;
                _TabStripItem.GetTabItemContentRectangle += TabStripItem_GetTabItemContentRectangle;

                _TabStripItem.BeforeTabDisplay += TabStripItem_BeforeTabDisplay;
                _TabStripItem.MeasureTabItem += TabStripItem_MeasureTabItem;
                _TabStripItem.PreRenderTabItem += TabStripItem_PreRenderTabItem;
                _TabStripItem.PostRenderTabItem += TabStripItem_PostRenderTabItem;

                _TabStripItem.GetTabTextBounds += TabStripItem_GetTabTextBounds;
                _TabStripItem.GetTabImageBounds += TabStripItem_GetTabImageBounds;
                _TabStripItem.GetTabCloseBounds += TabStripItem_GetTabCloseBounds;

                _TabStripItem.TabStripPaintBackground += TabStripItem_TabStripPaintBackground;

                _TabStripItem.SelectedTabChanging += TabStripItem_SelectedTabChanging;
                _TabStripItem.SelectedTabChanged += TabStripItem_SelectedTabChanged;

                _TabStripItem.TabStripTabColorChanged += TabStripItem_TabStripTabColorChanged;
            }
            else
            {
                _TabStripItem.TabItemOpen -= TabStripItem_TabItemOpen;
                _TabStripItem.TabItemClose -= TabStripItem_TabItemClose;
                _TabStripItem.TabRemoved -= TabStripItem_TabRemoved;
                _TabStripItem.TabMoving -= TabStripItem_TabMoving;
                _TabStripItem.TabMoved -= TabStripItem_TabMoved;

                _TabStripItem.GetTabItemPath -= TabStripItem_GetTabItemPath;
                _TabStripItem.GetTabItemContentRectangle -= TabStripItem_GetTabItemContentRectangle;

                _TabStripItem.BeforeTabDisplay -= TabStripItem_BeforeTabDisplay;
                _TabStripItem.MeasureTabItem -= TabStripItem_MeasureTabItem;
                _TabStripItem.PreRenderTabItem -= TabStripItem_PreRenderTabItem;
                _TabStripItem.PostRenderTabItem -= TabStripItem_PostRenderTabItem;

                _TabStripItem.GetTabTextBounds -= TabStripItem_GetTabTextBounds;
                _TabStripItem.GetTabImageBounds -= TabStripItem_GetTabImageBounds;
                _TabStripItem.GetTabCloseBounds -= TabStripItem_GetTabCloseBounds;

                _TabStripItem.TabStripPaintBackground -= TabStripItem_TabStripPaintBackground;

                _TabStripItem.SelectedTabChanging -= TabStripItem_SelectedTabChanging;
                _TabStripItem.SelectedTabChanged -= TabStripItem_SelectedTabChanged;

                _TabStripItem.TabStripTabColorChanged -= TabStripItem_TabStripTabColorChanged;
            }
        }

        #endregion

        #region Event processing

        #region TabStripItem_TabItemOpen

        /// <summary>
        /// TabStripItem_TabItemOpen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_TabItemOpen(object sender, SuperTabStripTabItemOpenEventArgs e)
        {
            if (TabItemOpen != null)
                TabItemOpen(this, e);
        }

        #endregion

        #region TabStripItem_TabItemClose

        /// <summary>
        /// TabStripItem_TabItemClose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (TabItemClose != null)
                TabItemClose(this, e);
        }

        #endregion

        #region TabStripItem_TabRemoved

        /// <summary>
        /// TabStripItem_TabRemoved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_TabRemoved(object sender, SuperTabStripTabRemovedEventArgs e)
        {
            if (TabRemoved != null)
                TabRemoved(this, e);
        }

        #endregion

        #region TabStripItem_TabMoving

        /// <summary>
        /// TabStripItem_TabMoving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_TabMoving(object sender, SuperTabStripTabMovingEventArgs e)
        {
            if (TabMoving != null)
                TabMoving(this, e);
        }

        #endregion

        #region TabStripItem_TabMoved

        /// <summary>
        /// TabStripItem_TabMoved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_TabMoved(object sender, SuperTabStripTabMovedEventArgs e)
        {
            if (TabMoved != null)
                TabMoved(this, e);
        }

        #endregion

        #region TabStripItem_GetTabItemPath

        /// <summary>
        /// TabStripItem_GetTabItemPath
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_GetTabItemPath(object sender, SuperTabGetTabItemPathEventArgs e)
        {
            if (GetTabItemPath != null)
                GetTabItemPath(this, e);
        }

        #endregion

        #region TabStripItem_GetTabItemContentRectangle

        /// <summary>
        /// TabStripItem_GetTabItemContentRectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_GetTabItemContentRectangle(object sender, SuperTabGetTabItemContentRectangleEventArgs e)
        {
            if (GetTabItemContentRectangle != null)
                GetTabItemContentRectangle(this, e);
        }

        #endregion

        #region TabStripItem_BeforeTabDisplay

        /// <summary>
        /// TabStripItem_BeforeTabDisplay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_BeforeTabDisplay(object sender, SuperTabStripBeforeTabDisplayEventArgs e)
        {
            if (BeforeTabDisplay != null)
                BeforeTabDisplay(this, e);
        }

        #endregion

        #region TabStripItem_MeasureTabItem

        /// <summary>
        /// TabStripItem_MeasureTabItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_MeasureTabItem(object sender, SuperTabMeasureTabItemEventArgs e)
        {
            if (MeasureTabItem != null)
                MeasureTabItem(this, e);
        }

        #endregion

        #region TabStripItem_PreRenderTabItem

        /// <summary>
        /// TabStripItem_PreRenderTabItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabStripItem_PreRenderTabItem(object sender, SuperTabPreRenderTabItemEventArgs e)
        {
            if (PreRenderTabItem != null)
                PreRenderTabItem(this, e);
        }

        #endregion

        #region TabStripItem_PostRenderTabItem

        /// <summary>
        /// TabStripItem_PostRenderTabItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_PostRenderTabItem(object sender, SuperTabPostRenderTabItemEventArgs e)
        {
            if (PostRenderTabItem != null)
                PostRenderTabItem(this, e);
        }

        #endregion

        #region TabStripItem_GetTabCloseBounds

        /// <summary>
        /// TabStripItem_GetTabCloseBounds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_GetTabCloseBounds(object sender, SuperTabGetTabCloseBoundsEventArgs e)
        {
            if (GetTabCloseBounds != null)
                GetTabCloseBounds(this, e);
        }

        #endregion

        #region TabStripItem_GetTabImageBounds

        /// <summary>
        /// TabStripItem_GetTabImageBounds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_GetTabImageBounds(object sender, SuperTabGetTabImageBoundsEventArgs e)
        {
            if (GetTabImageBounds != null)
                GetTabImageBounds(this, e);
        }

        #endregion

        #region TabStripItem_GetTabTextBounds

        /// <summary>
        /// TabStripItem_GetTabTextBounds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_GetTabTextBounds(object sender, SuperTabGetTabTextBoundsEventArgs e)
        {
            if (GetTabTextBounds != null)
                GetTabTextBounds(this, e);
        }

        #endregion

        #region TabStripItem_TabStripPaintBackground

        /// <summary>
        /// TabStripItem_TabStripPaintBackground
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_TabStripPaintBackground(object sender, SuperTabStripPaintBackgroundEventArgs e)
        {
            if (TabStripPaintBackground != null)
                TabStripPaintBackground(this, e);
        }

        #endregion

        #region TabStripItem_SelectedTabChanging

        /// <summary>
        /// TabStripItem_SelectedTabChanging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_SelectedTabChanging(object sender, SuperTabStripSelectedTabChangingEventArgs e)
        {
            if (SelectedTabChanging != null)
                SelectedTabChanging(this, e);
        }

        #endregion

        #region TabStripItem_SelectedTabChanged

        /// <summary>
        /// TabStripItem_SelectedTabChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            if (SelectedTabChanged != null)
                SelectedTabChanged(this, e);
        }

        #endregion

        #region TabStripItem_TabStripTabColorChanged

        /// <summary>
        /// TabStripItem_TabStripTabColorChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabStripItem_TabStripTabColorChanged(object sender, SuperTabStripTabColorChangedEventArgs e)
        {
            if (TabStripTabColorChanged != null)
                TabStripTabColorChanged(this, e);
        }

        #endregion

        #region Theme Changing

        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            if (BarFunctions.IsHandleValid(this))
                this.Invalidate(true);

            TabDisplay.SetDefaultColorTable();
        }

        #endregion

        #endregion

        #region DefaultSize

        /// <summary>
        /// Gets the DefaultSize
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(236, 25); }
        }

        #endregion

        #region RecalcLayout

        /// <summary>
        /// Performs layout recalculation
        /// </summary>
        public override void RecalcLayout()
        {
            base.RecalcLayout();

            if (_IsInitializing == false)
            {
                if (_TabStripItem != null)
                    _TabStripItem.RecalcSize();
            }
        }

        #endregion

        #region EnsureVisible

        /// <summary>
        /// Ensures that the given tab is visible on the TabStrip
        /// </summary>
        /// <param name="tab"></param>
        public void EnsureVisible(SuperTabItem tab)
        {
            _TabStripItem.EnsureVisible(tab);
        }

        #endregion

        #region GetTabFromPoint

        /// <summary>
        /// Gets the SuperTabItem from the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>SuperTabItem or null</returns>
        public SuperTabItem GetTabFromPoint(Point pt)
        {
            return (_TabStripItem.GetTabFromPoint(pt));
        }

        #endregion

        #region GetItemFromPoint

        /// <summary>
        /// Gets the BaseItem from the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>BaseItem or null</returns>
        public BaseItem GetItemFromPoint(Point pt)
        {
            return (_TabStripItem.GetItemFromPoint(pt));
        }

        #endregion

        #region CloseTab

        /// <summary>
        /// Closes the given tab
        /// </summary>
        /// <param name="tab"></param>
        public void CloseTab(SuperTabItem tab)
        {
            _TabStripItem.CloseTab(tab);
        }

        #endregion

        #region SelectPreviousTab

        /// <summary>
        /// Selects the previous tab
        /// </summary>
        /// <returns>true if successful</returns>
        public bool SelectPreviousTab()
        {
            return (_TabStripItem.SelectPreviousTab());
        }

        #endregion

        #region SelectNextTab

        /// <summary>
        /// Selects the next tab
        /// </summary>
        /// <returns>true if successful</returns>
        public bool SelectNextTab()
        {
            return (_TabStripItem.SelectNextTab());
        }

        #endregion

        #region OnResize

        /// <summary>
        /// OnResize processing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_TabStripItem != null)
                _TabStripItem.VisibleTab = null;

            RecalcLayout();
        }

        #endregion

        #region Mnemonic Processing
        protected override bool ProcessItemMnemonicKey(BaseItem item)
        {
            if (item != null && item.Visible && item.GetEnabled())
            {
                if (item is SuperTabItem)
                {
                    SuperTabItem tab = (SuperTabItem)item;
                    SuperTabControl tabControl = this.Parent as SuperTabControl;
                    if (tabControl != null && !tabControl.ContainsFocus) return false;
                    if (!tab.IsSelected)
                    {
                        _TabStripItem.SelectTab((SuperTabItem)item, eEventSource.Keyboard);
                        return true;
                    }
                    return false;
                }
            }
            return base.ProcessItemMnemonicKey(item);
        }
        #endregion

        #region ISupportInitialize Members

        /// <summary>
        /// BeginInit
        /// </summary>
        void ISupportInitialize.BeginInit()
        {
            _IsInitializing = true;
        }

        /// <summary>
        /// EndInit
        /// </summary>
        void ISupportInitialize.EndInit()
        {
            _IsInitializing = false;
        }

        #endregion

        #region KeyTips
        public override bool ProcessMnemonicEx(char charCode)
        {
            bool processed = base.ProcessMnemonicEx(charCode);
            if (processed)
            {
                if (this.ShowKeyTips)
                    this.ShowKeyTips = false;
                if (ApplicationButton != null)
                    ApplicationButton.BackstageMnemonicProcessed(charCode);
            }

            return processed;
        }

        protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
        {
            if (ApplicationButton != null)
            {
                Size padding = KeyTipsPainter.KeyTipsPadding;
                Size size = TextDrawing.MeasureString(g, keyTip, font);
                size.Width += padding.Width;
                size.Height += padding.Height;

                Rectangle ib = item.DisplayRectangle;
                Rectangle r;
                if (item is SuperTabItem)
                    r = new Rectangle(ib.X + 2, ib.Y, size.Width, size.Height);
                else
                    r = new Rectangle(ib.X + 24, ib.Y - 2, size.Width, size.Height);

                return r;
            }

            return base.GetKeyTipRectangle(g, item, font, keyTip);
        }
        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
                HookEvents(false);

            base.Dispose(disposing);
        }

        #endregion
    }
}
#endif