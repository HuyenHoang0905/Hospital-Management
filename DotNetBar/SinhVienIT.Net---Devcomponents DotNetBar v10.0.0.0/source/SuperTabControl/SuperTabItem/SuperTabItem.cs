#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    [Designer("DevComponents.DotNetBar.Design.SuperTabItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SuperTabItem : BaseItem
    {
        #region Events

        /// <summary>
        /// Occurs when text markup link is clicked
        /// </summary>
        [Description("Occurs when text markup link is clicked.")]
        public event MarkupLinkClickEventHandler MarkupLinkClick;

        /// <summary>
        /// Occurs when the tab colors have changed
        /// </summary>
        [Description("Occurs when the tab colors have changed.")]
        public event EventHandler<EventArgs> TabColorChanged;

        #endregion

        #region Private variables

        private Icon _Icon;
        private Image _Image;
        private int _ImageIndex = -1;
        private Rectangle _ImageBounds;

        private eTabItemColor _PredefinedColor = eTabItemColor.Default;
        private SuperTabItemColorTable _TabColor = new SuperTabItemColorTable();

        private bool _EnableMarkup = true;
        private eItemAlignment? _TextAlignment;
        private Rectangle _TextBounds;

        private Font _TabFont;
        private Font _SelectedTabFont;

        private bool _CloseButtonVisible = true;
        private bool _CloseButtonMouseOver;
        private bool _CloseButtonPressed;
        private Rectangle _CloseButtonBounds;

        private SuperTabStripItem _TabStripItem;
        private Control _AttachedControl;

        private SuperTabItemBaseDisplay _TabItemDisplay;

        private Font _DefaultSelectedTabFont;
        private bool _EnableImageAnimation;
        private bool _CanAnimateImage;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SuperTabItem()
        {
            Visible = true;
            GlobalItem = false;
            Stretch = false;
            Displayed = true;

            HookEvents(true);
        }

        #region Hidden properties

        /// <summary>
        /// Enabled
        /// </summary>
        [Browsable(false)]
        public override bool Enabled
        {
            get { return base.Enabled; }

            set
            {
                throw new Exception("SuperTabItems can not be disabled.");
            }
        }

        #endregion

        #region Protected properties

        #region IsMarkupSupported

        /// <summary>
        /// IsMarkupSupported
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return _EnableMarkup; }
        }

        #endregion

        #region DefaultSelectedTabFont

        /// <summary>
        /// Gets or sets the default SelectedTabFont
        /// </summary>
        protected Font DefaultSelectedTabFont
        {
            get
            {
                if (_DefaultSelectedTabFont == null)
                    _DefaultSelectedTabFont = new Font(SystemFonts.CaptionFont, FontStyle.Bold);

                return (_DefaultSelectedTabFont);
            }

            set
            {
                if (_DefaultSelectedTabFont != null)
                    _DefaultSelectedTabFont.Dispose();

                _DefaultSelectedTabFont = value;
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region CanAnimateImage

        internal bool CanAnimateImage
        {
            get
            {
                return (_EnableImageAnimation == true &&
                    _CanAnimateImage == true);
            }
        }

        #endregion

        #region CloseButtonBounds

        /// <summary>
        /// Gets the tab CloseButton Bounds
        /// </summary>
        internal Rectangle CloseButtonBounds
        {
            get
            {
                if (_CloseButtonBounds.IsEmpty)
                    _CloseButtonBounds = GetCloseButtonBounds();

                return (_CloseButtonBounds);
            }
        }

        #endregion

        #region CloseButtonMouseOver

        /// <summary>
        /// Gets or sets the CloseButtonMouseOver state
        /// </summary>
        internal bool CloseButtonMouseOver
        {
            get { return (_CloseButtonMouseOver); }

            set
            {
                if (_CloseButtonMouseOver != value)
                {
                    _CloseButtonMouseOver = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region CloseButtonPressed

        /// <summary>
        /// Gets or sets the CloseButtonPressed state
        /// </summary>
        internal bool CloseButtonPressed
        {
            get { return (_CloseButtonPressed); }

            set
            {
                if (_CloseButtonPressed != value)
                {
                    _CloseButtonPressed = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region ImageBounds

        /// <summary>
        /// Gets the tab Image Bounds
        /// </summary>
        internal Rectangle ImageBounds
        {
            get
            {
                if (_ImageBounds.IsEmpty)
                    _ImageBounds = GetImageBounds();

                return (_ImageBounds);
            }
        }

        #endregion

        #region IsVertical

        /// <summary>
        /// Gets the tabs vertical orientation
        /// </summary>
        internal bool IsVertical
        {
            get
            {
                return (_TabStripItem.HorizontalText == false &&
                        (_TabStripItem.TabAlignment == eTabStripAlignment.Left ||
                         _TabStripItem.TabAlignment == eTabStripAlignment.Right));
            }
        }

        #endregion

        #region TabDisplay

        /// <summary>
        /// Gets the TabItemDisplay
        /// </summary>
        internal SuperTabItemBaseDisplay TabItemDisplay
        {
            get { return (_TabItemDisplay); }
        }

        #endregion

        #region TabStrip

        /// <summary>
        /// Gets the tabs TabStrip
        /// </summary>
        internal SuperTabStrip TabStrip
        {
            get { return (TabStripItem != null ? TabStripItem.TabStrip : null); }
        }

        #endregion

        #region TextBounds

        /// <summary>
        /// Gets the tabs Text Bounds
        /// </summary>
        internal Rectangle TextBounds
        {
            get
            {
                if (_TextBounds.IsEmpty)
                    _TextBounds = GetTextBounds();

                return (_TextBounds);
            }
        }

        #endregion

        #endregion

        #region Public properties

        #region Browsable properties

        #region EnableImageAnimation

        /// <summary>
        /// Gets or sets whether image animation is enabled
        /// </summary>
        [Browsable(true), DevCoBrowsable(true)]
        [DefaultValue(false), Category("Appearance")]
        [Description("Indicates whether image animation is enabled.")]
        public bool EnableImageAnimation
        {
            get { return (_EnableImageAnimation); }

            set
            {
                if (_EnableImageAnimation != value)
                {
                    _EnableImageAnimation = value;

                    RefreshOwner();
                }
            }
        }

        #endregion

        #region TabColor

        /// <summary>
        /// Gets or sets user specified tab display colors
        /// </summary>
        [Browsable(true), Category("Style")]
        [Description("Contains user specified tab display colors.")]
        public SuperTabItemColorTable TabColor
        {
            get { return (_TabColor); }

            set
            {
                if (_TabColor.Equals(value) == false)
                {
                    if (_TabColor != null)
                        _TabColor.ColorTableChanged -= ColorTableColorTableChanged;

                    _TabColor = value;

                    if (value != null)
                        _TabColor.ColorTableChanged += ColorTableColorTableChanged;

                    OnTabColorChanged();

                    Refresh();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTabColor()
        {
            return (_TabColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTabColor()
        {
            TabColor = new SuperTabItemColorTable();
        }

        #endregion

        #region CloseButtonVisible

        /// <summary>
        /// Gets or sets whether Close button on the tab is visible when SuperTabStrip.CloseButtonOnTabsVisible property is set to true
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether Close button on the tab is visible when SuperTabStrip.CloseButtonOnTabsVisible property is set to true.")]
        public bool CloseButtonVisible
        {
            get { return (_CloseButtonVisible); }

            set
            {
                if (_CloseButtonVisible != value)
                {
                    _CloseButtonVisible = value;

                    RefreshOwner();
                }
            }
        }

        #endregion

        #region EnableMarkup

        /// <summary>
        /// Gets or sets whether text-markup support is enabled for the control's Text property
        /// </summary>
        [DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text-markup support is enabled for the control's Text property.")]
        public bool EnableMarkup
        {
            get { return (_EnableMarkup); }

            set
            {
                if (_EnableMarkup != value)
                {
                    _EnableMarkup = value;
                    NeedRecalcSize = true;

                    OnTextChanged();
                }
            }
        }

        #endregion

        #region Icon

        /// <summary>
        /// Gets or sets the tab icon. Icon has same functionality as Image except that it supports Alpha blending
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(null), Category("Appearance")]
        [Description("Indicates the tab icon. Icon has same functionality as Image except that it supports Alpha blending.")]
        public Icon Icon
        {
            get { return _Icon; }

            set
            {
                if (_Icon != value)
                {
                    _Icon = value;

                    RefreshOwner();
                }
            }
        }

        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetIcon()
        {
            Icon = null;
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets or sets the tab image
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(null), Category("Appearance")]
        [Description("Indicates the tab image.")]
        public Image Image
        {
            get { return (_Image); }

            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    OnImageChanged();
                }
            }
        }

        private void OnImageChanged()
        {
            if (_EnableImageAnimation == true)
            {
                if (_TabItemDisplay != null)
                    _TabItemDisplay.StopImageAnimation();
            }

            Image image = GetImage();

            _CanAnimateImage = ImageAnimator.CanAnimate(image);

            RefreshOwner();
        }

        #endregion

        #region ImageIndex

        /// <summary>
        /// Gets or sets the tab image index
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(-1)]
        [Category("Appearance"), Description("Indicates the tab image index")]
		[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
        [TypeConverter(typeof(ImageIndexConverter))]
        public int ImageIndex
        {
            get { return (_ImageIndex); }

            set
            {
                if (_ImageIndex != value)
                {
                    _ImageIndex = value;

                    OnImageChanged();
                }
            }
        }

        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetImageIndex()
        {
            ImageIndex = -1;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ImageList ImageList
        {
            get
            {
                if (_TabStripItem != null && _TabStripItem.TabStrip != null)
                    return (_TabStripItem.TabStrip.ImageList);

                return (null);
            }
        }

        #endregion

        #region PredefinedColor

        /// <summary>
        /// Gets or sets the predefined color for the tab
        /// </summary>
        [Browsable(true), DefaultValue(eTabItemColor.Default), Category("Style")]
        [Description("Indicates the predefined color for the tab.")]
        public eTabItemColor PredefinedColor
        {
            get { return (_PredefinedColor); }

            set
            {
                _PredefinedColor = value;

                OnTabColorChanged();
            }
        }

        #endregion

        #region SelectedTabFont

        /// <summary>
        /// Gets or sets the selected tab Font
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), DefaultValue(null)]
        [Description("Indicates the selected tab Font")]
        public Font SelectedTabFont
        {
            get { return (_SelectedTabFont); } 

            set
            {
                if (_SelectedTabFont != value)
                {
                    if (value != null)
                        DefaultSelectedTabFont = null;

                    _SelectedTabFont = value;

                    RefreshOwner();
                }
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

        #region TextAlignment

        /// <summary>
        /// Gets or sets the text alignment
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [DevCoBrowsable(true), Category("Layout"), Description("Indicates text alignment.")]
        public eItemAlignment? TextAlignment
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

        #region TabFont

        /// <summary>
        /// Gets or sets the tab Font
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), DefaultValue(null)]
        [Description("Indicates the tab Font")]
        public Font TabFont
        {
            get { return (_TabFont); }

            set
            {
                if (_TabFont != value)
                {
                    _TabFont = value;

                    RefreshOwner();
                }
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

        #region Text

        /// <summary>
        /// Gets or sets the tab text
        /// </summary>
        [Browsable(true)]
        [Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
        [Category("Appearance"), Description("Indicates the tab text.")]
        public override string Text
        {
            get { return (base.Text); }
            set { base.Text = value; }
        }

        #endregion

        #endregion

        #region Non-Browsable properties

        #region AttachedControl

        /// <summary>
        /// Gets or sets the control that is attached to this tab
        /// </summary>
        [Browsable(false), DevCoBrowsable(true), DefaultValue(null), Category("Behavior")]
        public Control AttachedControl
        {
            get { return (_AttachedControl); }

            set
            {
                SuperTabControlPanel panel = _AttachedControl as SuperTabControlPanel;

                if (panel != null)
                    panel.PanelColorChanged -= ColorTableColorTableChanged;

                _AttachedControl = value;

                panel = _AttachedControl as SuperTabControlPanel;

                if (panel != null)
                {
                    panel.TabItem = this;
                    panel.Dock = DockStyle.Fill;

                    panel.PanelColorChanged += ColorTableColorTableChanged;

                    if (_TabStripItem != null && _TabStripItem.TabStrip != null)
                    {
                        if (DesignMode == true)
                            _TabStripItem.TabStrip.Controls.Add(panel);

                        panel.BringToFront();
                        panel.Visible = (_TabStripItem.SelectedTab == this);
                    }
                }
            }
        }

        #endregion

        #region Bounds

        /// <summary>
        /// Gets or sets the tab Bounds
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle Bounds
        {
            get { return (base.Bounds); }

            set
            {
                base.Bounds = value;

                _TextBounds = Rectangle.Empty;
                _CloseButtonBounds = Rectangle.Empty;
                _ImageBounds = Rectangle.Empty;
            }
        }

        #endregion

        #region IsMouseOver

        /// <summary>
        /// Gets the MouseOver state
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMouseOver
        {
            get { return (_TabStripItem != null && _TabStripItem.MouseOverTab == this); }
        }

        #endregion

        #region IsSelected

        /// <summary>
        /// Gets the tab selected state
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSelected
        {
            get { return (_TabStripItem != null && _TabStripItem.SelectedTab == this); }
        }

        #endregion

        #region TabAlignment

        /// <summary>
        /// Gets the tab alignment
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eTabStripAlignment TabAlignment
        {
            get
            {
                if (_TabStripItem != null)
                    return (_TabStripItem.TabAlignment);

                return (eTabStripAlignment.Top);
            }
        }

        #endregion

        #region ContentRectangle

        /// <summary>
        /// Gets the tab Content Rectangle
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ContentRectangle
        {
            get
            {
                if (_TabItemDisplay != null)
                    return (_TabItemDisplay.GetContentRectangle());

                return (Rectangle.Empty);
            }
        }

        #endregion

        #region TabStripItem

        /// <summary>
        /// Gets or sets the tab TabStripItem
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SuperTabStripItem TabStripItem
        {
            get { return (_TabStripItem); }

            set
            {
                _TabStripItem = value;

                SetOwner(value);

                if (_TabStripItem != null)
                {
                    TabStyle = _TabStripItem.TabStyle;
                    ContainerControl = _TabStripItem.ContainerControl;
                }
            }
        }

        #endregion

        #region TabStyle

        /// <summary>
        /// Gets or sets the tabs TabStyle
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eSuperTabStyle TabStyle
        {
            get
            {
                if (_TabStripItem != null)
                    return (_TabStripItem.TabStyle);

                return (eSuperTabStyle.Office2007);
            }

            internal set
            {
                if (_TabItemDisplay != null)
                    _TabItemDisplay.Dispose();

                switch (value)
                {
                    case eSuperTabStyle.Office2007:
                        _TabItemDisplay = new Office2007SuperTabItem(this);
                        break;

                    case eSuperTabStyle.Office2010BackstageBlue:
                        _TabItemDisplay = new Office2010BackstageSuperTabItem(this);
                        break;

                    case eSuperTabStyle.OneNote2007:
                        _TabItemDisplay = new OneNote2007SuperTabItem(this);
                        break;

                    case eSuperTabStyle.VisualStudio2008Dock:
                        _TabItemDisplay = new VS2008DockSuperTabItem(this);
                        break;

                    case eSuperTabStyle.VisualStudio2008Document:
                        _TabItemDisplay = new VS2008DocumentSuperTabItem(this);
                        break;

                    case eSuperTabStyle.WinMediaPlayer12:
                        _TabItemDisplay = new WinMediaPlayer12SuperTabItem(this);
                        break;
                }
            }
        }

        #endregion

        #region Visible

        /// <summary>
        /// Visible
        /// </summary>
        public override bool Visible
        {
            get { return (base.Visible); }

            set
            {
                base.Visible = value;

                if (TabStripItem != null)
                    TabStripItem.UpdateSelectedTab();
            }
        }

        #endregion

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks or unhooks control events
        /// </summary>
        /// <param name="hook">true to hook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                TextChanged += SuperTabItemTextChanged;
                MarkupLinkClick += TextMarkupLinkClick;

                _TabColor.ColorTableChanged += ColorTableColorTableChanged;
            }
            else
            {
                TextChanged -= SuperTabItemTextChanged;
                MarkupLinkClick -= TextMarkupLinkClick;

                if (_TabColor != null)
                    _TabColor.ColorTableChanged -= ColorTableColorTableChanged;
            }
        }

        #endregion

        #region Event processing

        #region SuperTabItem_TextChanged

        /// <summary>
        /// SuperTabItem_TextChanged processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SuperTabItemTextChanged(object sender, EventArgs e)
        {
            NeedRecalcSize = true;
            Refresh();

            if (_TabStripItem != null)
            {
                _TabStripItem.NeedRecalcSize = true;
                _TabStripItem.Refresh();
            }
        }

        #endregion

        #region Markup Support

        /// <summary>
        /// TextMarkupLinkClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TextMarkupLinkClick(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;

            if (link != null)
                OnMarkupLinkClick(new MarkupLinkClickEventArgs(link.Name, link.HRef));

            base.TextMarkupLinkClick(sender, e);
        }

        /// <summary>
        /// OnMarkupLinkClick
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }

        #endregion

        #region ColorTable_ColorTableChanged

        /// <summary>
        /// ColorTable_ColorTableChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ColorTableColorTableChanged(object sender, EventArgs e)
        {
            OnTabColorChanged();
        }

        #endregion

        #endregion

        #region OnColorTableChanged

        /// <summary>
        /// Processes ColorTable changes
        /// </summary>
        protected void OnTabColorChanged()
        {
            if (TabColorChanged != null)
                TabColorChanged(this, EventArgs.Empty);

            Refresh();
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes the tab
        /// </summary>
        public void Close()
        {
            _TabStripItem.CloseTab(this);
        }

        #endregion

        #region GetTabFont

        /// <summary>
        /// Gets the operational tab font
        /// </summary>
        /// <returns></returns>
        public Font GetTabFont()
        {
            Font font = (_TabFont ?? _TabStripItem.TabFont) ?? SystemFonts.CaptionFont;

            if (IsSelected == true)
                font = (_SelectedTabFont ?? _TabStripItem.SelectedTabFont) ?? DefaultSelectedTabFont;

            return (font);
        }

        #endregion

        #region GetTabColorTable

        /// <summary>
        /// Gets the tab ColorTable
        /// </summary>
        /// <returns>SuperTabItemStateColorTable</returns>
        internal SuperTabItemStateColorTable GetTabColorTable()
        {
            return (_TabItemDisplay.GetTabColorTable());
        }

        /// <summary>
        /// Gets the tab ColorStateTable for the given state
        /// </summary>
        /// <param name="tabState">eTabState</param>
        /// <returns>SuperTabItemStateColorTable</returns>
        internal SuperTabItemStateColorTable GetTabColorTable(eTabState tabState)
        {
            return (_TabItemDisplay.GetTabColorTable(tabState));
        }

        #endregion

        #region GetPanelColorTable

        /// <summary>
        /// Gets the tab Panel ColorTable
        /// </summary>
        /// <returns></returns>
        internal SuperTabPanelItemColorTable GetPanelColorTable()
        {
            return (_TabItemDisplay.GetPanelColorTable());
        }

        #endregion

        #region GetTabAreaFromPoint

        /// <summary>
        /// Gets the area of the tab that contains the given Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public eSuperTabArea GetTabAreaFromPoint(Point pt)
        {
            if (Bounds.Contains(pt) == true)
            {
                if (CloseButtonBounds.Contains(pt) == true)
                    return (eSuperTabArea.InCloseBox);

                if (ImageBounds.Contains(pt) == true)
                    return (eSuperTabArea.InImage);

                return (eSuperTabArea.InContent);
            }

            return (eSuperTabArea.InNone);
        }

        #endregion

        #region PointInTab

        /// <summary>
        /// Determines if the given Point is in the tab
        /// </summary>
        /// <param name="pt">Point to test</param>
        /// <returns>true if Point is in tab</returns>
        public bool PointInTab(Point pt)
        {
            using (GraphicsPath path = GetTabItemPath())
            {
                if (path.IsVisible(pt))
                    return (true);
            }

            return (false);
        }

        #endregion

        #region GetTabItemPath

        /// <summary>
        /// Gets the tab bordering GraphicsPath
        /// </summary>
        /// <returns>GraphicsPath</returns>
        public GraphicsPath GetTabItemPath()
        {
            return (_TabItemDisplay != null ?
                _TabItemDisplay.TabItemPath() : null);
        }

        #endregion

        #region GetCloseButtonBounds

        /// <summary>
        /// Gets the Close Button bounding Rectangle
        /// </summary>
        /// <returns>Bounding Rectangle</returns>
        private Rectangle GetCloseButtonBounds()
        {
            if (CloseButtonVisible == false ||
                _TabStripItem == null ||
                _TabStripItem.CloseButtonOnTabsVisible == false)
            {
                return (Rectangle.Empty);
            }

            Rectangle r = TabItemDisplay.GetContentRectangle();

            Size closeSize = _TabStripItem.TabCloseButtonSize;
            int spacing = _TabStripItem.TabHorizontalSpacing;

            if (IsVertical == true)
            {
                if ((_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Left &&
                     _TabStripItem.TabAlignment == eTabStripAlignment.Left) ||
                    (_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Right &&
                     _TabStripItem.TabAlignment == eTabStripAlignment.Right))
                {
                    r = new Rectangle(r.X + (r.Width - closeSize.Width) / 2 + 1,
                                      r.Bottom - spacing - closeSize.Height - 1,
                                      closeSize.Width, closeSize.Height);
                }
                else
                {
                    r = new Rectangle(r.X + (r.Width - closeSize.Width) / 2 + 1,
                                      r.Y + spacing, closeSize.Width, closeSize.Height);
                }
            }
            else
            {
                if (_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Left)
                {
                    r = new Rectangle(r.X + spacing,
                                      r.Y + (r.Height - closeSize.Height) / 2,
                                      closeSize.Width, closeSize.Height);

                    if (_TabStripItem.TabAlignment == eTabStripAlignment.Right)
                    {
                        if (IsSelected == true)
                            r.X += _TabStripItem.TabDisplay.SelectedPaddingWidth;
                    }
                }
                else
                {
                    r = new Rectangle(r.Right - spacing - closeSize.Width,
                                      r.Y + (r.Height - closeSize.Height) / 2 + 1,
                                      closeSize.Width, closeSize.Height);

                    if (_TabStripItem.TabAlignment == eTabStripAlignment.Left)
                    {
                        if (IsSelected == true)
                            r.X -= _TabStripItem.TabDisplay.SelectedPaddingWidth;
                    }
                }
            }

            r = _TabStripItem.OnGetCloseBounds(this, r);

            return (r);
        }

        #endregion

        #region GetImageBounds

        /// <summary>
        /// Gets the Image bounding Rectangle
        /// </summary>
        /// <returns>Bounding Rectangle</returns>
        private Rectangle GetImageBounds()
        {
            CompositeImage image = GetTabImage();

            if (image == null)
                return (Rectangle.Empty);

            Rectangle r = TabItemDisplay.GetContentRectangle();

            r = ProcessCloseBounds(r);

            int w = IsVertical ? image.Height : image.Width;
            int h = IsVertical ? image.Width : image.Height;

            if (_TabStripItem.TabAlignment == eTabStripAlignment.Top ||
                _TabStripItem.TabAlignment == eTabStripAlignment.Bottom || _TabStripItem.HorizontalText)
			{
                r.Y += (r.Height - h) / 2;
			}
			else
			{
                r.X += (r.Width - w) / 2;

                if (_TabStripItem.TabAlignment == eTabStripAlignment.Left)
                    r.Y = r.Bottom - h;
			}

            r.Width = w;
            r.Height = h;

            r = _TabStripItem.OnGetImageBounds(this, r);

            return (r);
        }

        #region GetTabImage

        internal CompositeImage GetTabImage()
        {
            Image image = GetImage();

            if (image != null)
                return (new CompositeImage(image, false));

            return (Icon != null) ?
                (new CompositeImage(Icon, false, Icon.Size)) : null;
        }

        #endregion

        #region GetImage

        private Image GetImage()
        {
            if (_Image != null)
                return (_Image);

            if (_ImageIndex >= 0 && TabStrip != null)
            {
                ImageList imageList = TabStrip.ImageList;

                if (imageList != null && _ImageIndex < imageList.Images.Count)
                    return (imageList.Images[_ImageIndex]);
            }

            return (null);
        }

        #endregion

        #endregion

        #region GetTextBounds

        /// <summary>
        /// Gets the Text bounding Rectangle
        /// </summary>
        /// <returns>Bounding Rectangle</returns>
        private Rectangle GetTextBounds()
        {
            Rectangle r = TabItemDisplay.GetContentRectangle();
                
            r = ProcessImageBounds(
                ProcessCloseBounds(r));

            if (IsSelected || TabStripItem.DisplaySelectedTextOnly == false)
                r.Width -= _TabStripItem.TabHorizontalSpacing;

            if (IsVertical)
            {
                if (_TabStripItem.TabAlignment == eTabStripAlignment.Right)
                    r.X += 4;
            }
            else
            {
                if (IsSelected == true)
                {
                    if (_TabStripItem.TabAlignment == eTabStripAlignment.Left ||
                        _TabStripItem.TabAlignment == eTabStripAlignment.Right)
                    {
                        r.Width -= _TabStripItem.TabDisplay.SelectedPaddingWidth;
                    }
                }
            }

            _TabStripItem.OnGetTextBounds(this, r);

            return (r);
        }

        #endregion

        #region ProcessCloseBounds

        /// <summary>
        /// Calculates Close button bounds
        /// </summary>
        /// <param name="r">Running rectangle</param>
        /// <returns>Running rectangle</returns>
        private Rectangle ProcessCloseBounds(Rectangle r)
        {
            int spacing = _TabStripItem.TabHorizontalSpacing;

            Size size = new Size();

            if (_TabStripItem.CloseButtonOnTabsVisible && CloseButtonVisible)
            {
                size += CloseButtonBounds.Size;

                size.Width += spacing;
                size.Height += spacing;
            }

            if (IsVertical)
            {
                if (_TabStripItem.TabAlignment == eTabStripAlignment.Left)
                {
                    if (_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Right)
                        r.Y += size.Width;
                }
                else if (_TabStripItem.TabAlignment == eTabStripAlignment.Right)
                {
                    if (_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Left)
                        r.Y += size.Width;

                    r.Y += spacing;
                }

                r.Height -= (size.Width + spacing);
            }
            else
            {
                if (_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Left)
                    r.X += size.Width;

                r.X += spacing;
                r.Width -= (size.Width + spacing);

                if (this.IsSelected == true)
                {
                    if (_TabStripItem.TabAlignment == eTabStripAlignment.Right)
                        r.X += _TabStripItem.TabDisplay.SelectedPaddingWidth;
                }
            }

            return (r);
        }

        #endregion

        #region ProcessImageBounds

        /// <summary>
        /// Calculates Image bounds
        /// </summary>
        /// <param name="r">Running rectangle</param>
        /// <returns>Running rectangle</returns>
        private Rectangle ProcessImageBounds(Rectangle r)
        {
            CompositeImage image = GetTabImage();

            if (image != null)
            {
                int spacing = _TabStripItem.TabHorizontalSpacing;

                int w = IsVertical ? image.Height : image.Width;
                int h = IsVertical ? image.Width : image.Height;

                if (_TabStripItem.TabAlignment == eTabStripAlignment.Top ||
                    _TabStripItem.TabAlignment == eTabStripAlignment.Bottom || _TabStripItem.HorizontalText)
                {
                    r.X += (w + spacing);
                    r.Width -= (w + spacing);
                }
                else
                {
                    if (_TabStripItem.TabAlignment == eTabStripAlignment.Right)
                        r.Y += (h + spacing);

                    r.Height -= (h + spacing);
                }
            }

            return (r);
        }

        #endregion

        #region RefreshOwner

        /// <summary>
        /// Refreshes the tab owner
        /// </summary>
        private void RefreshOwner()
        {
            if (_TabStripItem != null)
            {
                _TabStripItem.NeedRecalcSize = true;
                _TabStripItem.Refresh();
            }

            Refresh();
        }

        #endregion

        #region Refresh

        /// <summary>
        /// Refreshes the tab display
        /// </summary>
        public override void Refresh()
        {
            if (SuspendLayout == false)
            {
                if (Visible == true && Displayed == true && TabStripItem != null)
                {
                    Control objCtrl = this.TabStripItem.ContainerControl as Control;

                    if (objCtrl != null && IsHandleValid(objCtrl))
                    {
                        if (NeedRecalcSize == true)
                        {
                            if (Parent is ItemContainer)
                            {
                                Parent.RecalcSize();
                            }
                            else
                            {
                                RecalcSize();

                                if (Parent != null)
                                    Parent.SubItemSizeChanged(this);
                            }
                        }

                        Invalidate(objCtrl);
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
            if (_TabItemDisplay != null)
                _TabItemDisplay.Paint(p);
        }

        #endregion

        #region Mouse support

        #region InternalMouseMove

        /// <summary>
        /// InternalMouseMove
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            base.InternalMouseMove(objArg);

            CloseButtonMouseOver =
                CloseButtonBounds.Contains(objArg.Location);
        }

        #endregion

        #region InternalMouseEnter

        /// <summary>
        /// InternalMouseEnter
        /// </summary>
        public override void InternalMouseEnter()
        {
            base.InternalMouseEnter();

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

            CloseButtonMouseOver = false;
            CloseButtonPressed = false;

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
            base.InternalMouseDown(objArg);

            if (CloseButtonMouseOver == true)
            {
                CloseButtonPressed =
                    ((objArg.Button & MouseButtons.Left) == MouseButtons.Left);
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

            CloseButtonPressed = false;
        }

        #endregion

        #endregion

        #region ToString

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (Text);
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
                HookEvents(false);

            base.Dispose(disposing);
        }

        #endregion

        #region Copy

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            SuperTabItem objCopy = new SuperTabItem();
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New SuperTabItem instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            SuperTabItem objCopy = copy as SuperTabItem;

            if (objCopy != null)
            {
                base.CopyToItem(objCopy);

                objCopy.TabColor = _TabColor;
                objCopy.CloseButtonVisible = _CloseButtonVisible;
                objCopy.EnableImageAnimation = _EnableImageAnimation;
                objCopy.EnableMarkup = _EnableMarkup;
                objCopy.Icon = _Icon;
                objCopy.Image = _Image;
                objCopy.ImageIndex = _ImageIndex;
                objCopy.PredefinedColor = _PredefinedColor;
                objCopy.SelectedTabFont = _SelectedTabFont;
                objCopy.TextAlignment = _TextAlignment;
                objCopy.TabFont = _TabFont;

                objCopy.AttachedControl = _AttachedControl;
            }
        }

        #endregion
    }

    #region enums

    /// <summary>
    /// Tab area parts
    /// </summary>
    public enum eSuperTabArea
    {
        InNone,
        InContent,
        InImage,
        InCloseBox
    }

    /// <summary>
    /// Tab states
    /// </summary>
    public enum eTabState
    {
        Default,
        Selected,
        MouseOver,
        SelectedMouseOver
    }

    #endregion
}
#endif