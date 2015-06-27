using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents RibbonStrip control internally used by RibbonControl.
    /// </summary>
    [ToolboxItem(false), ComVisible(false)]
    public class RibbonStrip : ItemControl
    {
        private const int Office2010GlassExcludeTopPart = 29;
        #region Private Variables & Constructor
        private RibbonStripContainerItem m_StripContainer = null;
        //private GenericItemContainer m_ItemContainer=null;
        private RibbonTabItemGroupCollection m_Groups = null;
        private int m_TabGroupHeight = 14;
        private bool m_TabGroupsVisible = false;
        private Font m_DefaultGroupFont = null;
        private Font m_DefaultGroupFontAuto = null;
        private int m_CaptionHeight = 0;
        private bool m_CaptionVisible = false;
        private bool m_AutoExpand = true;
        //private GenericItemContainer m_CaptionContainer = null;
        //private SystemCaptionItem m_SystemCaptionItem = null;
        private int m_RibbonStripIndent = 46;
        private Rectangle m_CaptionBounds = Rectangle.Empty;
        private Rectangle m_QuickToolbarBounds = Rectangle.Empty;
        private Rectangle m_SystemCaptionItemBounds = Rectangle.Empty;
        private Rectangle[] m_CaptionTextBounds = null;
        private Font m_CaptionFont = null;
        private bool m_CanCustomize = true;
        private ElementStyle m_DefaultBackgroundStyle = new ElementStyle();
        private bool m_KeyTipsEnabled = true;
        private string m_TitleText = "";
        private bool m_CanSupportGlass = false;

        public RibbonStrip()
        {
            this.SetStyle(ControlStyles.StandardDoubleClick, true);

            m_StripContainer = new RibbonStripContainerItem(this);
            m_StripContainer.GlobalItem = false;
            m_StripContainer.ContainerControl = this;
            m_StripContainer.Displayed = true;
            m_StripContainer.SetOwner(this);
            m_StripContainer.Style = eDotNetBarStyle.Office2003;
            this.SetBaseItemContainer(m_StripContainer);

            this.ColorScheme.Style = eDotNetBarStyle.Office2003;

            this.AutoSize = true;

            m_Groups = new RibbonTabItemGroupCollection();
            m_Groups.Owner = this;

            // Setup system caption item
            m_StripContainer.SystemCaptionItem.Click += new EventHandler(SystemCaptionClick);
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when text markup link from TitleText markup is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        [Description("Occurs when text markup link from TitleText markup is clicked.")]
        public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets whether RibbonStrip control employs the Windows Vista Glass support when available. This is managed automatically by Ribbon Control and
        /// no setting is necessary on your part.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanSupportGlass
        {
            get { return m_CanSupportGlass; }
            set { m_CanSupportGlass = value; }
        }

        internal override bool IsGlassEnabled
        {
            get
            {
                bool baseIsGlassEnabled = base.IsGlassEnabled;
                if (!m_CanSupportGlass || !baseIsGlassEnabled) return false;
                Form form = this.FindForm();
                if (form is Office2007Form) return false;
                if (form is Office2007RibbonForm)
                    return ((Office2007RibbonForm)form).IsGlassEnabled;
                return baseIsGlassEnabled;
            }
        }

        private TextMarkup.BodyElement m_TitleTextMarkup = null;
        /// <summary>
        /// Gets or sets the rich text displayed on Ribbon Title instead of the Form.Text property. This property supports text-markup.
        /// You can use <font color="SysCaptionTextExtra"> markup to instruct the markup renderer to use Office 2007 system caption extra text color which
        /// changes depending on the currently selected color table. Note that when using this property you should manage also the Form.Text property since
        /// that is the text that will be displayed in Windows task-bar and elsewhere where system Form.Text property is used.
        /// You can also use the hyperlinks as part of the text markup and handle the TitleTextMarkupLinkClick event to be notified when they are clicked.
        /// </summary>
        [Browsable(true), DefaultValue(""), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), EditorBrowsable(EditorBrowsableState.Always), Category("Appearance"), Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
        public string TitleText
        {
            get { return m_TitleText; }
            set
            {
                if (value == null) value = "";
                m_TitleText = value;
                m_TitleTextMarkup = null;

                if (!TextMarkup.MarkupParser.IsMarkup(ref m_TitleText))
                    return;

                m_TitleTextMarkup = TextMarkup.MarkupParser.Parse(m_TitleText);

                if (m_TitleTextMarkup != null)
                    m_TitleTextMarkup.HyperLinkClick += new EventHandler(InternalTitleTextMarkupLinkClick);
                TitleTextMarkupLastArrangeBounds = Rectangle.Empty;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Occurs when text markup link is clicked.
        /// </summary>
        private void InternalTitleTextMarkupLinkClick(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
            if (link != null)
            {
                if (TitleTextMarkupLinkClick != null)
                    TitleTextMarkupLinkClick(this, new MarkupLinkClickEventArgs(link.Name, link.HRef));
            }
        }

        /// <summary>
        /// Gets reference to parsed markup body element if text was markup otherwise returns null.
        /// </summary>
        internal TextMarkup.BodyElement TitleTextMarkupBody
        {
            get { return m_TitleTextMarkup; }
        }

        internal Rectangle TitleTextMarkupLastArrangeBounds = Rectangle.Empty;
        /// <summary>
        /// Gets or sets whether KeyTips functionality is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Behavior"), Description("Indicates whether KeyTips functionality is enabled.")]
        public bool KeyTipsEnabled
        {
            get { return m_KeyTipsEnabled; }
            set { m_KeyTipsEnabled = value; }
        }

        /// <summary>
        /// Gets or sets whether control can be customized and items added by end-user using context menu to the quick access toolbar.
        /// Caption of the control must be visible for customization to be enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Customization"), Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
        public bool CanCustomize
        {
            get { return m_CanCustomize; }
            set { m_CanCustomize = value; }
        }

        /// <summary>
        /// Gets or sets the explicit height of the caption provided by control. Caption height when set is composed of the TabGroupHeight and
        /// the value specified here. Default value is 0 which means that system default caption size is used.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Appearance"), Description("Indicates explicit height of the caption provided by control")]
        public int CaptionHeight
        {
            get { return m_CaptionHeight; }
            set
            {
                m_CaptionHeight = value;
                m_StripContainer.NeedRecalcSize = true;
            }
        }

        internal bool HasVisibleTabs
        {
            get
            {
                foreach (BaseItem item in this.Items)
                {
                    if (item.Visible) return true;
                }
                return false;
            }
        }

        internal Rectangle[] CaptionTextBounds
        {
            get { return m_CaptionTextBounds; }
            set { m_CaptionTextBounds = value; }
        }

        internal Rectangle CaptionBounds
        {
            get { return m_CaptionBounds; }
            set { m_CaptionBounds = value; }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle QuickToolbarBounds
        {
            get { return m_QuickToolbarBounds; }
            set { m_QuickToolbarBounds = value; }
        }

        internal Rectangle SystemCaptionItemBounds
        {
            get { return m_SystemCaptionItemBounds; }
            set { m_SystemCaptionItemBounds = value; }
        }

        internal SystemCaptionItem SystemCaptionItem
        {
            get { return m_StripContainer.SystemCaptionItem; }
        }

        /// <summary>
        /// Gets or sets whether custom caption line provided by the control is visible. Default value is false.
        /// This property should be set to true when control is used on Office2007RibbonForm.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance"), Description("Indicates whether custom caption line provided by the control is visible.")]
        public bool CaptionVisible
        {
            get { return m_CaptionVisible; }
            set
            {
                m_CaptionVisible = value;
                OnCaptionVisibleChanged();
            }
        }

        /// <summary>
        /// Gets or sets the font for the form caption text when CaptionVisible=true. Default value is NULL which means that system font is used.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Appearance"), Description("")]
        public Font CaptionFont
        {
            get { return m_CaptionFont; }
            set
            {
                m_CaptionFont = value;
                m_StripContainer.NeedRecalcSize = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the indent of the ribbon strip. The indent setting is useful when control is used with caption visible and the Office 2007
        /// style start button. The indent specified here will move the ribbon strip so the start button does not overlap the tabs.
        /// Value of this property is used only when CaptionVisible = true.
        /// Default value is 46.
        /// </summary>
        [Browsable(true), DefaultValue(46), Category("Layout"), Description("Indicates indent of the ribbon strip.")]
        public int RibbonStripIndent
        {
            get { return m_RibbonStripIndent; }
            set
            {
                m_RibbonStripIndent = value;
                this.RecalcLayout();
            }
        }

        private void OnCaptionVisibleChanged()
        {
            m_StripContainer.NeedRecalcSize = true;
            this.RecalcLayout();
        }

        /// <summary>
        /// Gets or sets the height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.
        /// Default value is 14 pixels. To show tab groups you need to assign the RibbonTabItem.Group property and set TabGroupsVisible=true.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(14), Category("Tab Groups"), Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
        public int TabGroupHeight
        {
            get { return m_TabGroupHeight; }
            set
            {
                m_TabGroupHeight = value;
                m_StripContainer.NeedRecalcSize = true;
                if (this.DesignMode)
                    this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether tab group line that is displayed above the RibbonTabItem objects that have group assigned is visible.
        /// Default value is false. To show tab groups you need to assign the RibbonTabItem.Group property and set TabGroupsVisible=true. Use TabGroupHeight
        /// property to control height of the group line.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(false), Category("Tab Groups"), Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
        public bool TabGroupsVisible
        {
            get { return m_TabGroupsVisible; }
            set
            {
                m_TabGroupsVisible = value;
                m_StripContainer.NeedRecalcSize = true;
                if (this.DesignMode)
                    this.RecalcLayout();
            }
        }

        /// <summary>
        /// Collection of RibbonTabItemGroup items. Groups are assigned optionally to one or more RibbonTabItem object through the RibbonTabItem.Group
        /// property to visually group tabs that belong to same functions. These tabs should be positioned next to each other.
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.RibbonTabItemGroupCollectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Tab Groups"), Browsable(true), DevCoBrowsable(true)]
        public RibbonTabItemGroupCollection TabGroups
        {
            get { return m_Groups; }
        }

        internal eDotNetBarStyle EffectiveStyle
        {
            get
            {
                return m_StripContainer.EffectiveStyle;
            }
        }
        /// <summary>
        /// Gets/Sets the visual style of the control.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Specifies the visual style of the control."), DefaultValue(eDotNetBarStyle.Office2003)]
        public eDotNetBarStyle Style
        {
            get
            {
                return m_StripContainer.Style;
            }
            set
            {
                this.ColorScheme.Style = value;
                m_StripContainer.Style = value;
                this.Invalidate();
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Returns collection of items on a bar.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public SubItemsCollection Items
        {
            get
            {
                return m_StripContainer.RibbonStripContainer.SubItems;
            }
        }

        /// <summary>
        /// Returns currently selected RibbonTabItem. RibbonTabItems are selected using the Checked property. Only a single
        /// RibbonTabItem can be Checked at any given time.
        /// </summary>
        [Browsable(false)]
        public RibbonTabItem SelectedRibbonTabItem
        {
            get
            {
                foreach (BaseItem item in this.Items)
                {
                    if (item is RibbonTabItem && ((RibbonTabItem)item).Checked)
                        return (RibbonTabItem)item;
                }
                return null;
            }
        }

        internal override ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = base.GetItemPaintArgs(g);
            RibbonControl rc = this.Parent as RibbonControl;
            if (rc != null)
                pa.ControlExpanded = rc.Expanded || rc.IsPopupMode;
            return pa;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WinApi.WindowsMessages.WM_NCHITTEST)
            {
                // Get position being tested...
                int x = WinApi.LOWORD(m.LParam);
                int y = WinApi.HIWORD(m.LParam);
                Point p = PointToClient(new Point(x, y));
                if (IsGlassEnabled && this.CaptionVisible)
                {
                    Rectangle r = new Rectangle(this.Width - SystemInformation.CaptionButtonSize.Width * 3, 0, SystemInformation.CaptionButtonSize.Width * 3, SystemInformation.CaptionButtonSize.Height + 6);
                    if (r.Contains(p))
                    {
                        m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                        return;
                    }

                    r = new Rectangle(0, 0, this.Width, 4);
                    if (r.Contains(p))
                    {
                        m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                        return;
                    }

                    if (BarFunctions.IsWindows7 && this.IsMaximized)
                    {
                        Rectangle[] captionTextBounds = m_CaptionTextBounds;
                        if (captionTextBounds != null)
                        {
                            foreach (Rectangle item in captionTextBounds)
                            {
                                if (item.Contains(p))
                                {
                                    m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                                    return;
                                }
                            }
                        }
                    }
                }
                else if (this.CaptionVisible && !this.IsMaximized)
                {
                    Rectangle r = new Rectangle(0, 0, this.Width, 4);
                    if (r.Contains(p))
                    {
                        m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                        return;
                    }
                }

                if (m_CaptionVisible && this.EffectiveStyle == eDotNetBarStyle.Office2010 && (p.X < 28 && this.RightToLeft == RightToLeft.No || p.X > this.Width - 28 && this.RightToLeft == RightToLeft.Yes) && p.Y < 28)
                {
                    m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                    return;
                }
                else if (m_CaptionVisible && this.EffectiveStyle == eDotNetBarStyle.Windows7 && (p.X < 28 && this.RightToLeft == RightToLeft.No || p.X > this.Width - 28 && this.RightToLeft == RightToLeft.Yes) && p.Y < 28)
                {
                    m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                    return;
                }
            }

            base.WndProc(ref m);
        }

        internal bool IsMaximized
        {
            get
            {
                Form f = this.FindForm();
                return (f != null && f.WindowState == FormWindowState.Maximized);
            }
        }

        protected override void ClearBackground(ItemPaintArgs pa)
        {
            if (this.IsGlassEnabled)
            {
                Office2007RibbonForm f = this.FindForm() as Office2007RibbonForm;
                if (f != null)
                {
                    pa.Graphics.Clear(Color.Transparent);
                    pa.Graphics.SetClip(new Rectangle(0, 0, this.Width, f.GlassHeight - 1), System.Drawing.Drawing2D.CombineMode.Exclude);
                }
                base.ClearBackground(pa);
                pa.Graphics.ResetClip();
            }
            else
                base.ClearBackground(pa);
        }
        protected override Rectangle GetPaintControlBackgroundRectangle()
        {
            if (this.IsGlassEnabled && this.EffectiveStyle == eDotNetBarStyle.Office2010)
            {
                Rectangle r = base.GetPaintControlBackgroundRectangle();
                r.Y += Office2010GlassExcludeTopPart;
                r.Height -= Office2010GlassExcludeTopPart;
                return r;
            }
            return base.GetPaintControlBackgroundRectangle();
        }
        protected override void PaintControlBackground(ItemPaintArgs pa)
        {
            bool resetClip = false;
            if (this.IsGlassEnabled)
            {
                Office2007RibbonForm f = this.FindForm() as Office2007RibbonForm;
                if (f != null)
                {
                    pa.Graphics.SetClip(new Rectangle(0, 0, this.Width, f.GlassHeight - (this.EffectiveStyle == eDotNetBarStyle.Office2010 && pa.GlassEnabled ? Office2010GlassExcludeTopPart : 1)),
                        System.Drawing.Drawing2D.CombineMode.Exclude);
                    resetClip = true;
                }
            }

            base.PaintControlBackground(pa);

            if (resetClip) pa.Graphics.ResetClip();

            m_QuickToolbarBounds = Rectangle.Empty;
            m_CaptionBounds = Rectangle.Empty;
            m_SystemCaptionItemBounds = Rectangle.Empty;

            Rendering.BaseRenderer renderer = GetRenderer();
            if (renderer != null && this.Parent is RibbonControl)
            {
                renderer.DrawRibbonControlBackground(new RibbonControlRendererEventArgs(pa.Graphics, this.Parent as RibbonControl, pa.GlassEnabled));

                if (m_CaptionVisible)
                    renderer.DrawQuickAccessToolbarBackground(new RibbonControlRendererEventArgs(pa.Graphics, this.Parent as RibbonControl, pa.GlassEnabled));
            }

            if (m_TabGroupsVisible)
            {
                PaintTabGroups(pa);
            }

            // Paint form caption text
            if (renderer != null && m_CaptionVisible)
            {
                RibbonControlRendererEventArgs rer = new RibbonControlRendererEventArgs(pa.Graphics, this.Parent as RibbonControl, pa.GlassEnabled);
                rer.ItemPaintArgs = pa;
                renderer.DrawRibbonFormCaptionText(rer);
            }

#if TRIAL
            if (NativeFunctions.ColorExpAlt())
				{
					pa.Graphics.Clear(Color.White);
					TextDrawing.DrawString(pa.Graphics, "Trial Version Expired", this.Font, Color.FromArgb(128, Color.Black), this.ClientRectangle, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
				}
                //else
                //{
                //    TextDrawing.DrawString(pa.Graphics, "Trial Version", this.Font, Color.FromArgb(128, Color.Black), new Rectangle(0, 0, this.Width - 12, this.Height-4), eTextFormat.Right | eTextFormat.Bottom);
                //}
#endif
        }

        private void PaintTabGroups(ItemPaintArgs pa)
        {
            if (m_Groups.Count == 0)
                return;
            RibbonTabItemGroup currentGroup = null;
            Rectangle groupRect = Rectangle.Empty;
            Rectangle groupBounds = Rectangle.Empty;
            int y = 0;
            eDotNetBarStyle effectiveStyle = this.EffectiveStyle;

            if (m_CaptionVisible)
            {
                if (effectiveStyle == eDotNetBarStyle.Office2010)
                    y += 1;
                else
                    y += (pa.GlassEnabled && effectiveStyle == eDotNetBarStyle.Windows7) ? 4 : 3;
            }

            foreach (RibbonTabItemGroup group in this.TabGroups)
                group.DisplayPositions.Clear();

            foreach (BaseItem item in this.Items)
            {
#if TRIAL
                if (NativeFunctions.ColorExpAlt())
					return;
#endif
                if (item is RibbonTabItem)
                {
                    RibbonTabItem tab = item as RibbonTabItem;
                    if (tab.Group != currentGroup)
                    {
                        if (tab.Visible)
                        {
                            if (currentGroup != null)
                                PaintGroup(pa, currentGroup, groupRect, groupBounds);
                            currentGroup = tab.Group;
                            if (currentGroup != null)
                            {
                                currentGroup.DisplayPositions.Clear();
                                groupRect = new Rectangle(tab.DisplayRectangle.X, y, tab.DisplayRectangle.Width, (effectiveStyle== eDotNetBarStyle.Office2010?tab.DisplayRectangle.Y : tab.DisplayRectangle.Y - 4));
                                groupBounds = new Rectangle(tab.DisplayRectangle.X, y, tab.DisplayRectangle.Width, tab.DisplayRectangle.Bottom - m_TabGroupHeight);
                            }
                        }
                    }
                    else if (currentGroup != null)
                    {
                        if (tab.Visible)
                        {
                            groupRect.Width += (tab.DisplayRectangle.Right - groupRect.Right);
                            groupBounds.Width += (tab.DisplayRectangle.Right - groupBounds.Right);
                        }
                    }
                    else
                    {
                        groupRect = Rectangle.Empty;
                        groupBounds = Rectangle.Empty;
                    }
                }
                else if (currentGroup != null)
                {
                    PaintGroup(pa, currentGroup, groupRect, groupBounds);
                }
            }

            if (currentGroup != null)
                PaintGroup(pa, currentGroup, groupRect, groupBounds);
        }

        private void PaintGroup(ItemPaintArgs pa, RibbonTabItemGroup group, Rectangle rect, Rectangle groupBounds)
        {
            if (m_CaptionVisible && !m_QuickToolbarBounds.IsEmpty && rect.IntersectsWith(m_QuickToolbarBounds) || m_SystemCaptionItemBounds.IntersectsWith(groupBounds))
                return;
            eDotNetBarStyle effectiveStyle = this.EffectiveStyle;

            if (effectiveStyle == eDotNetBarStyle.Office2010)
                group.DisplayPositions.Add(new Rectangle(rect.X, rect.Y, rect.Width + 1, rect.Height));
            else
                group.DisplayPositions.Add(rect);

            
            if (BarFunctions.IsOffice2007Style(effectiveStyle))
            {
                RibbonTabGroupRendererEventArgs e = new RibbonTabGroupRendererEventArgs(pa.Graphics, group, rect, groupBounds, this.GetDefaultGroupFont(), pa, effectiveStyle);
                Rendering.BaseRenderer renderer = GetRenderer();
                renderer.DrawRibbonTabGroup(e);
            }
            else
            {
                ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(group.Style, pa.Graphics, rect);
                ElementStyleDisplay.Paint(info);
                ElementStyleDisplay.PaintText(info, group.GroupTitle, this.GetDefaultGroupFont());
            }
        }

        private Font GetDefaultGroupFont()
        {
            if (m_DefaultGroupFont == null)
            {
                try
                {
                    if (m_DefaultGroupFontAuto == null)
                        m_DefaultGroupFontAuto = new Font(this.Font.Name, this.Font.SizeInPoints - 1);
                }
                catch
                {
                    return this.Font;
                }
                return m_DefaultGroupFontAuto;
            }
            return m_DefaultGroupFont;
        }

        /// <summary>
        /// Gets or sets default font for tab groups. This font will be used if font is not specified by group style element.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true),Category("Tab Groups"),DefaultValue(null)]
        public Font DefaultGroupFont
        {
            get { return m_DefaultGroupFont; }
            set
            {
                m_DefaultGroupFont = value;
                if (this.DesignMode)
                    this.Refresh();
            }
        }
        /// <summary>
        /// Resets DefaultGroupFont property to default value null.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDefaultGroupFont()
        {
            TypeDescriptor.GetProperties(this)["DefaultGroupFont"].SetValue(this, null);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            m_DefaultGroupFont = null;
        }

        protected override ElementStyle GetBackgroundStyle()
        {
            if (this.BackgroundStyle.Custom)
                return base.GetBackgroundStyle();
            return m_DefaultBackgroundStyle;
        }

        internal void InitDefaultStyles()
        {
            m_DefaultBackgroundStyle.SetColorScheme(this.GetColorScheme());
            // Initialize Default Styles
            if (this.Parent is RibbonControl)
                RibbonPredefinedColorSchemes.ApplyRibbonElementStyle(m_DefaultBackgroundStyle, (RibbonControl)this.Parent);
        }

        internal ElementStyle InternalGetBackgroundStyle()
        {
            return this.GetBackgroundStyle();
        }

        internal Rectangle GetItemContainerBounds()
        {
            Rectangle r = base.GetItemContainerRectangle();
            if (m_TabGroupsVisible)
            {
                r.Y += m_TabGroupHeight + 1;
            }
            if (m_CaptionVisible)
            {
                r.Y += GetAbsoluteCaptionHeight();
            }
            if (m_CaptionVisible && m_RibbonStripIndent > 0)
            {
                eDotNetBarStyle effectiveStyle = this.EffectiveStyle;
                if (effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Windows7)
                {
                    if (this.RightToLeft == RightToLeft.No)
                        r.X += m_RibbonStripIndent;
                    r.Width -= m_RibbonStripIndent;
                }
            }
            return r;
        }

        internal Rectangle GetCaptionContainerBounds()
        {
            Rectangle baseRect = base.GetItemContainerRectangle();
            if (this.IsGlassEnabled)
                baseRect.Y += 3;
            return new Rectangle(baseRect.X, baseRect.Y, baseRect.Width, GetAbsoluteCaptionHeight());
        }

        private int GetCaptionHeight()
        {
            if (m_CaptionHeight == 0)
            {
                //int captionHeight = (SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height * NativeFunctions.BorderMultiplierFactor) - 
                //(System.Environment.OSVersion.Version.Major < 6?14:12);
                int captionHeight = 16; // System.Environment.OSVersion.Version.Major < 6 ? 13 : 16;
                if (this.IsGlassEnabled && EffectiveStyle != eDotNetBarStyle.Office2010 && EffectiveStyle != eDotNetBarStyle.Windows7)
                    captionHeight += 0;
                else if (!this.IsGlassEnabled && EffectiveStyle == eDotNetBarStyle.Office2010)
                    captionHeight -= 2;
                else if (!this.IsGlassEnabled && EffectiveStyle == eDotNetBarStyle.Windows7)
                    captionHeight -= 2;

                return captionHeight;
            }
            else
            {
                return m_CaptionHeight;
            }
        }

        internal int GetAbsoluteCaptionHeight()
        {
            return GetCaptionHeight() + (!m_TabGroupsVisible ? m_TabGroupHeight : 0);
        }

        internal int GetTotalCaptionHeight()
        {
            return GetCaptionHeight() + m_TabGroupHeight + ((this.EffectiveStyle == eDotNetBarStyle.Office2010 || this.EffectiveStyle == eDotNetBarStyle.Windows7) ? 0 : 0);
        }

        internal int GetGlassCaptionHeight()
        {
            if(this.EffectiveStyle == eDotNetBarStyle.Office2010)
                return this.Height;
            else
                return GetTotalCaptionHeight();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            InitDefaultStyles();
            base.OnPaint(e);
        }

        protected override void RecalcSize()
        {
            InitDefaultStyles();
            base.RecalcSize();
        }

        /// <summary>
        /// Gets or sets whether control is collapsed when RibbonTabItem is double clicked and expanded when RibbonTabItem is clicked.
        /// </summary>
        internal bool AutoExpand
        {
            get { return m_AutoExpand; }
            set { m_AutoExpand = value; }
        }

        /// <summary>
        /// Returns automatically calculated height of the control given current content.
        /// </summary>
        /// <returns>Height in pixels.</returns>
        public override int GetAutoSizeHeight()
        {
            int i = base.GetAutoSizeHeight();
            //if (m_TabGroupsVisible)
            //    i += m_TabGroupHeight + 1;
            //if (m_CaptionVisible)
            //    i += m_CaptionHeight;
            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                Rendering.BaseRenderer renderer = GetRenderer();
                if (renderer is Rendering.Office2007Renderer)
                    i += ((Rendering.Office2007Renderer)renderer).ColorTable.RibbonControl.CornerSize;
            }

            return i;
        }

        protected override bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            Rectangle r = this.DisplayRectangle;
            r.Location = this.PointToScreen(r.Location);

            RibbonControl rc = this.Parent as RibbonControl;

            if (this.Parent is RibbonControl)
            {
                r = this.Parent.DisplayRectangle;
                r.Location = this.Parent.PointToScreen(r.Location);
                if (rc.SelectedRibbonTabItem != null && rc.SelectedRibbonTabItem.Panel is IKeyTipsControl && ((IKeyTipsControl)rc.SelectedRibbonTabItem.Panel).ShowKeyTips)
                    return false;
            }

            if (rc != null && !rc.MouseWheelTabScrollEnabled) return false;

            Point mousePos = Control.MousePosition;
            bool expanded = true;
            if (rc != null) expanded = rc.Expanded;

            bool parentActive = true;
            Form parentForm = this.FindForm();
            if (parentForm != null && !BarFunctions.IsFormActive(parentForm))
                parentActive = false;

            if (parentActive && r.Contains(mousePos) && !this.ShowKeyTips && expanded && this.Items.Count > 0)
            {
                IntPtr handle = NativeFunctions.WindowFromPoint(new NativeFunctions.POINT(mousePos));
                Control c = Control.FromChildHandle(handle);
                if (c == null)
                    c = Control.FromHandle(handle);
                if (c is RibbonBar || c is RibbonStrip || c is RibbonControl || c is RibbonPanel)
                {
                    RibbonTabItem selectedTab = this.SelectedRibbonTabItem;
                    int start = 0;
                    int end = this.Items.Count - 1;

                    int direction = 1;
                    if (wParam.ToInt64() > 0)
                    {
                        direction = -1;
                        end = 0;
                    }
                    if (selectedTab != null)
                    {
                        start = this.Items.IndexOf(selectedTab) + direction;
                        if (direction < 0 && start < 0) return false;

                        if (start == this.Items.Count)
                            start = 0;
                        else if (start < 0)
                            start = this.Items.Count - 1;
                    }

                    int index = start - direction;
                    do
                    {
                        index += direction;
                        if (index < 0 || index > this.Items.Count - 1) break;

                        if (this.Items[index] is RibbonTabItem && this.Items[index].Visible)
                        {
                            ((RibbonTabItem)this.Items[index]).Checked = true;
                            return true;
                        }
                    } while (index != end);
                }
            }

            return false;
        }

        private ArrayList GetAllRibbonBars()
        {
            ArrayList list = new ArrayList();
            if (!(this.Parent is RibbonControl))
                return list;

            foreach (Control c in this.Parent.Controls)
            {
                GetAllRibbonBars(c, list);
            }

            RibbonControl rc = this.Parent as RibbonControl;
            if (!rc.Expanded && this.SelectedRibbonTabItem != null && this.SelectedRibbonTabItem.Panel != null && this.SelectedRibbonTabItem.Panel.Parent != rc)
            {
                GetAllRibbonBars(this.SelectedRibbonTabItem.Panel, list);
            }

            return list;
        }

        private void GetAllRibbonBars(Control c, ArrayList list)
        {
            if (c is RibbonBar)
                list.Add(c);

            foreach (Control child in c.Controls)
                GetAllRibbonBars(child, list);
        }

        /// <summary>
        /// Returns the collection of items with the specified name.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <returns></returns>
        public override ArrayList GetItems(string ItemName)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByName(GetBaseItemContainer(), ItemName, list);

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null && rc.QatPositionedBelowRibbon)
            {
                BarFunctions.GetSubItemsByName(rc.QatToolbar.GetBaseItemContainer(), ItemName, list);
            }

            ArrayList ribbonBars = GetAllRibbonBars();
            foreach (RibbonBar b in ribbonBars)
                BarFunctions.GetSubItemsByName(b.GetBaseItemContainer(), ItemName, list);

            if (rc != null && rc.GlobalContextMenuBar != null)
                BarFunctions.GetSubItemsByName(rc.GlobalContextMenuBar.ItemsContainer, ItemName, list);

            return list;
        }

        /// <summary>
        /// Returns the collection of items with the specified name and type.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <returns></returns>
        public override ArrayList GetItems(string ItemName, Type itemType)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, list, itemType);

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null && rc.QatPositionedBelowRibbon)
            {
                BarFunctions.GetSubItemsByNameAndType(rc.QatToolbar.GetBaseItemContainer(), ItemName, list, itemType);
            }

            ArrayList ribbonBars = GetAllRibbonBars();
            foreach (RibbonBar b in ribbonBars)
                BarFunctions.GetSubItemsByNameAndType(b.GetBaseItemContainer(), ItemName, list, itemType);

            if (rc != null && rc.GlobalContextMenuBar != null)
                BarFunctions.GetSubItemsByNameAndType(rc.GlobalContextMenuBar.ItemsContainer, ItemName, list, itemType);

            return list;
        }

        /// <summary>
        /// Returns the collection of items with the specified name and type.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <param name="useGlobalName">Indicates whether GlobalName property is used for searching.</param>
        /// <returns></returns>
        public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, list, itemType, useGlobalName);

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null && rc.QatPositionedBelowRibbon)
            {
                BarFunctions.GetSubItemsByNameAndType(rc.QatToolbar.GetBaseItemContainer(), ItemName, list, itemType, useGlobalName);
            }

            ArrayList ribbonBars = GetAllRibbonBars();
            foreach (RibbonBar b in ribbonBars)
                BarFunctions.GetSubItemsByNameAndType(b.GetBaseItemContainer(), ItemName, list, itemType, useGlobalName);

            if (rc != null && rc.GlobalContextMenuBar != null)
                BarFunctions.GetSubItemsByNameAndType(rc.GlobalContextMenuBar.ItemsContainer, ItemName, list, itemType, useGlobalName);

            return list;
        }

        /// <summary>
        /// Returns the first item that matches specified name.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <returns></returns>
        public override BaseItem GetItem(string ItemName)
        {
            BaseItem item = BarFunctions.GetSubItemByName(GetBaseItemContainer(), ItemName);
            if (item != null)
                return item;

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null && rc.QatPositionedBelowRibbon)
            {
                item = BarFunctions.GetSubItemByName(rc.QatToolbar.GetBaseItemContainer(), ItemName);
                if (item != null)
                    return item;
            }

            ArrayList ribbonBars = GetAllRibbonBars();
            foreach (RibbonBar b in ribbonBars)
            {
                if (b.OverflowState && b.OverflowRibbonBar != null)
                    item = BarFunctions.GetSubItemByName(b.OverflowRibbonBar.GetBaseItemContainer(), ItemName);
                else
                    item = BarFunctions.GetSubItemByName(b.GetBaseItemContainer(), ItemName);
                if (item != null)
                    return item;
            }

            if (rc != null && rc.GlobalContextMenuBar != null)
                return BarFunctions.GetSubItemByName(rc.GlobalContextMenuBar.ItemsContainer, ItemName);

            return null;
        }
        #endregion

        #region KeyTips Support
        private bool m_AltFocus = false;
        private IKeyTipsControl m_AltFocusedControl = null;

        /// <summary>
        /// Called when ShowKeyTips on RibbonBar contained by this Ribbon is set to true
        /// </summary>
        internal void OnRibbonBarShowKeyTips(RibbonBar bar)
        {
            if (!m_AltFocus)
            {
                m_AltFocus = true;
            }

            if (m_AltFocusedControl != bar.Parent && bar.Parent is RibbonPanel)
            {
                if (m_AltFocusedControl != null)
                    m_AltFocusedControl.ShowKeyTips = false;
                m_AltFocusedControl = bar.Parent as IKeyTipsControl;
                if (m_AltFocusedControl != null)
                    m_AltFocusedControl.ShowKeyTips = true;
            }
        }

        protected override void OnShowKeyTipsChanged()
        {
            //if (this.SelectedRibbonTabItem != null && this.SelectedRibbonTabItem.Panel != null)
            //{
            //    Control panel = this.SelectedRibbonTabItem.Panel;
            //    foreach (Control c in panel.Controls)
            //    {
            //        if (c is RibbonBar && c.Visible)
            //        {
            //            ((RibbonBar)c).ShowKeyTips = this.ShowKeyTips;
            //        }
            //    }
            //}
            RibbonControl rc = this.GetRibbonControl();
            if (rc != null && rc.QatPositionedBelowRibbon)
                rc.QatToolbar.ShowKeyTips = this.ShowKeyTips;
            base.OnShowKeyTipsChanged();

            if (this.ShowKeyTips && !m_AltFocus)
                GiveKeyTipsAltFocus();
        }

        internal void BackstageTabClosed(SuperTabControl tabControl)
        {
            if (m_AltFocusedControl == tabControl || m_AltFocusedControl == tabControl.TabStrip)
                m_AltFocusedControl = null;
        }

        protected override bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {

            if (wParam.ToInt32() == 27)
            {
                RibbonControl rc = this.GetRibbonControl();
                if (rc != null)
                    rc.OnEscapeKeyDown();
            }

            if (m_AltFocus)
            {
                if (HasRegisteredPopups)
                {
                    bool ret = base.OnKeyDown(hWnd, wParam, lParam);
                    if (!HasRegisteredPopups) ReleaseAltFocus();
                }
                Keys key = (Keys)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                if (key == Keys.None)
                    key = (Keys)wParam.ToInt32();

                if (key == Keys.Escape)
                {
                    bool reShowKeyTips = false;
                    if (!this.ShowKeyTips && m_AltFocusedControl != null)
                    {
                        if (m_AltFocusedControl is RibbonPanel)
                            reShowKeyTips = true;
                    }

                    ReleaseAltFocus();

                    if (reShowKeyTips)
                    {
                        GiveKeyTipsAltFocus();
                    }

                    return base.OnKeyDown(hWnd, wParam, lParam);
                }

                char accessKey = CharFromInt((int)key);
                if (accessKey == char.MinValue)
                {
                    ReleaseAltFocus();
                    return true;
                }
                else if (key == Keys.Space || key == Keys.Down || key == Keys.Right || key == Keys.Tab)
                {
                    ReleaseAltFocus();
                    return false;
                }

                //if (m_AltFocusedControl != null && m_AltFocusedControl.ProcessMnemonicEx(accessKey))
                //{
                //    ReleaseAltFocus();
                //    return true;
                //}
                //else 
                if (this.ProcessMnemonic(accessKey))
                    return true;

                return true;
            }
            else if (wParam.ToInt32() == 112 && Control.ModifierKeys == Keys.Control)
            {
                Form frm = this.FindForm();
                if (BarFunctions.IsFormActive(frm))
                {
                    // Collapse ribbon
                    RibbonControl rc = this.GetRibbonControl();
                    if (rc.AutoExpand)
                        rc.Expanded = !rc.Expanded;
                }
            }

            return base.OnKeyDown(hWnd, wParam, lParam);
        }

        private char CharFromInt(int key)
        {
            char[] ch = new char[1];
            byte[] by = new byte[1];
            try
            {
                by[0] = System.Convert.ToByte(key);
                System.Text.Encoding.Default.GetDecoder().GetChars(by, 0, 1, ch, 0);
            }
            catch (Exception)
            {
                return char.MinValue;
            }

            return ch[0];
        }

        protected override bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (wParam.ToInt32() == 18 || wParam.ToInt32() == 121)
            {
                bool callBase = true;
                if (m_AltFocus)
                {
                    if (wParam.ToInt32() == 18 || wParam.ToInt32() == 121)
                        callBase = false;
                    ReleaseAltFocus();
                }
                else
                {
                    Form f = this.FindForm();
                    if (f == Form.ActiveForm)
                    {
                        callBase = false;
                        GiveKeyTipsAltFocus();
                    }
                }
                if (!callBase)
                    return true;
            }
            return base.OnSysKeyUp(hWnd, wParam, lParam);
        }

        private void GiveKeyTipsAltFocus()
        {
            if (!m_KeyTipsEnabled) return;

            m_AltFocus = true;
            this.ShowKeyTips = true;
            SetupActiveWindowTimer();
        }

        private void ReleaseAltFocus()
        {
            if (m_AltFocus)
            {
                ReleaseActiveWindowTimer();
                if (!this.ShowKeyTips && m_AltFocusedControl != null)
                {
                    m_AltFocusedControl.ShowKeyTips = false;
                    m_AltFocusedControl = null;
                }
                this.ShowKeyTips = false;
            }

            m_AltFocus = false;
        }

        /// <summary>
        /// Gets whether Ribbon is in key-tips mode including its child controls.
        /// </summary>
        [Browsable(false)]
        public bool IsInKeyTipsMode
        {
            get
            {
                return m_AltFocus | this.ShowKeyTips;
            }
        }

        /// <summary>
        /// Forces the control to exit Ribbon Key-Tips mode.
        /// </summary>
        public void ExitKeyTipsMode()
        {
            ReleaseAltFocus();
        }

        internal void OnChildItemClick(BaseItem item)
        {
            if (this.ShowKeyTips || m_AltFocusedControl != null && m_AltFocusedControl.ShowKeyTips)
            {
                this.ShowKeyTips = false;
                ReleaseAltFocus();
            }
        }

        protected override bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (this.ShowKeyTips || m_AltFocusedControl != null && m_AltFocusedControl.ShowKeyTips)
            {
                this.ShowKeyTips = false;
                ReleaseAltFocus();
            }

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null) rc.OnSysMouseDown(hWnd, wParam, lParam);

            return base.OnSysMouseDown(hWnd, wParam, lParam);
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (m_AltFocusedControl != null)
            {
                if (m_AltFocusedControl.ProcessMnemonicEx(charCode))
                {
                    ReleaseAltFocus();
                    return true;
                }
                return false;
            }
            if (!m_AltFocus && (Control.ModifierKeys & Keys.Alt) != Keys.Alt)
                return false;
            return base.ProcessMnemonic(charCode);
        }

        public override bool ProcessMnemonicEx(char charCode)
        {
            // If different ribbon tab item is selected we should not exit the Alt mode...
            BaseItem item = null;

            if (m_CaptionVisible)
            {
                item = GetItemForMnemonic(m_StripContainer.CaptionContainer, charCode, false, true);
                if (item != null)
                {
                    if (ProcessItemMnemonicKey(item))
                    {
                        ReleaseAltFocus();
                        return true;
                    }
                }
            }

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null && rc.QatPositionedBelowRibbon)
            {
                if (rc.QatToolbar.ProcessMnemonicEx(charCode))
                {
                    ReleaseAltFocus();
                    return true;
                }
            }

            if (item == null)
            {
                item = GetItemForMnemonic(this.GetBaseItemContainer(), charCode, true, true);
            }

            if (item is RibbonTabItem && item.Visible)
            {
                if (!m_AltFocus)
                {
                    GiveKeyTipsAltFocus();
                }
                this.ShowKeyTips = false;
                if (item != this.SelectedRibbonTabItem)
                {
                    // Switch the tab
                    item.RaiseClick();
                    //this.ShowKeyTips = true;
                }
                else if (this.SelectedRibbonTabItem != null && rc != null && !rc.Expanded && !rc.IsPopupMode)
                    rc.RibbonTabItemClick(this.SelectedRibbonTabItem);
                if (this.SelectedRibbonTabItem != null && m_KeyTipsEnabled)
                {
                    if (m_AltFocusedControl != null)
                        m_AltFocusedControl.ShowKeyTips = false;
                    m_AltFocusedControl = this.SelectedRibbonTabItem.Panel;
                    if (m_AltFocusedControl != null)
                        m_AltFocusedControl.ShowKeyTips = true;
                }

                // Keep the Alt focus
                return true;
            }
            else if (item != null && ProcessItemMnemonicKey(item))
            {
                this.ShowKeyTips = false;
                if (item is Office2007StartButton)
                {
                    Office2007StartButton appButton = (Office2007StartButton)item;
                    if (appButton.BackstageTab != null)
                    {
                        if (m_AltFocusedControl != null)
                            m_AltFocusedControl.ShowKeyTips = false;
                        m_AltFocusedControl = appButton.BackstageTab.TabStrip;
                        if (!m_AltFocusedControl.ShowKeyTips)
                            m_AltFocusedControl.ShowKeyTips = true;
                    }

                }
                return true;
            }

            //if (base.ProcessMnemonicEx(charCode))
            //{
            //    ReleaseAltFocus();
            //    return true;
            //}

            if (m_AltFocus && this.SelectedRibbonTabItem != null && this.SelectedRibbonTabItem.Panel != null)
            {
                Control panel = this.SelectedRibbonTabItem.Panel;
                foreach (Control c in panel.Controls)
                {
                    if (c is RibbonBar && c.Visible)
                    {
                        if (IsMnemonic(charCode, c.Text))
                        {
                            // Select RibbonBar for Alt+ Key access
                            this.ShowKeyTips = false;
                            ((RibbonBar)c).ShowKeyTips = true;
                            if (m_AltFocusedControl != null)
                                m_AltFocusedControl.ShowKeyTips = false;
                            m_AltFocusedControl = (IKeyTipsControl)c;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected override void OnActiveWindowChanged()
        {
            ReleaseAltFocus();
            base.OnActiveWindowChanged();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            return base.ProcessDialogKey(keyData);
        }
        #endregion

        #region Caption Container Support
        /// <summary>
        /// Called when item on popup container is right-clicked.
        /// </summary>
        /// <param name="item">Instance of the item that is right-clicked.</param>
        protected override void OnPopupItemRightClick(BaseItem item)
        {
            RibbonControl rc = this.GetRibbonControl();
            if (rc != null)
                rc.ShowCustomizeContextMenu(item, false);
        }
        //private RibbonControl GetRibbonControl()
        //{
        //    Control parent = this.Parent;
        //    while (parent != null && !(parent is RibbonControl))
        //        parent = parent.Parent;
        //    if (parent is RibbonControl)
        //        return parent as RibbonControl;
        //    return null;
        //}
        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_TitleTextMarkup != null)
                m_TitleTextMarkup.MouseLeave(this);
            base.OnMouseLeave(e);
        }

        internal bool MouseDownOnCaption
        {
            get
            {
                return m_MouseDownOnCaption;
            }
        }

        private bool m_MouseDownOnCaption = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_MouseDownOnCaption = false;
            if (m_CaptionVisible)
            {
                m_MouseDownOnCaption = HitTestCaption(new Point(e.X, e.Y));
                if (e.Button == MouseButtons.Right && m_MouseDownOnCaption)
                {
                    Form form = this.FindForm();
                    if (form != null)
                    {
                        const int TPM_RETURNCMD = 0x0100;
                        Point p = Control.MousePosition;
                        byte[] bx = BitConverter.GetBytes(p.X);
                        byte[] by = BitConverter.GetBytes(p.Y);
                        byte[] blp = new byte[] { bx[0], bx[1], by[0], by[1] };
                        int lParam = BitConverter.ToInt32(blp, 0);
                        this.Capture = false;
                        NativeFunctions.SendMessage(form.Handle, NativeFunctions.WM_SYSCOMMAND, NativeFunctions.TrackPopupMenu(
                            NativeFunctions.GetSystemMenu(form.Handle, false), TPM_RETURNCMD, Control.MousePosition.X, Control.MousePosition.Y, 0, form.Handle, IntPtr.Zero), lParam);
                        return;
                    }
                }

                if (m_TitleTextMarkup != null)
                    m_TitleTextMarkup.MouseDown(this, e);

                e = TranslateMouseEventArgs(e);
            }

            if (e.Button == MouseButtons.Right && this.QuickToolbarBounds.Contains(e.X, e.Y))
            {
                RibbonControl rc = this.GetRibbonControl();
                if (rc != null)
                    rc.OnRibbonStripRightClick(this, e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Left && m_CaptionVisible && m_TabGroupsVisible && !m_StripContainer.RibbonStripContainer.DisplayRectangle.Contains(e.X, e.Y))
            {
                foreach (RibbonTabItemGroup group in this.TabGroups)
                {
                    if (!group.Visible) continue;
                    foreach (Rectangle r in group.DisplayPositions)
                    {
                        if (r.Contains(e.X, e.Y))
                        {
                            if (!group.IsTabFromGroupSelected)
                            {
                                group.SelectFirstTab();
                                return;
                            }
                            break;
                        }
                    }
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_CaptionVisible)
            {
                if (m_TitleTextMarkup != null)
                    m_TitleTextMarkup.MouseUp(this, e);

                e = TranslateMouseEventArgs(e);
            }
            m_MouseDownOnCaption = false;
            base.OnMouseUp(e);
        }

        internal BaseItem GetApplicationButton()
        {
            if (!this.CaptionVisible)
                return null;
            BaseItem cont = this.CaptionContainerItem;

            if (this.EffectiveStyle == eDotNetBarStyle.Office2010 && this.Items.Count > 0 && this.Items[0] is Office2007StartButton)
                return this.Items[0];

            if (cont.SubItems.Count > 0 && cont.SubItems[0] is Office2007StartButton)
                return cont.SubItems[0];

            if (cont.SubItems.Count > 0 && cont.SubItems[0] is ButtonItem && ((ButtonItem)cont.SubItems[0]).HotTrackingStyle == eHotTrackingStyle.Image)
                return cont.SubItems[0];

            return null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_CaptionVisible)
            {
                if (e.Button == MouseButtons.Left && m_MouseDownOnCaption && HitTestCaption(new Point(e.X, e.Y)))
                {
                    Form form = this.FindForm();
                    if (form != null && form.WindowState == FormWindowState.Normal)
                    {
                        PopupItem popup = GetApplicationButton() as PopupItem;
                        if (popup != null && popup.Expanded) popup.Expanded = false;
                        const int HTCAPTION = 2;
                        Point p = Control.MousePosition;
                        byte[] bx = BitConverter.GetBytes(p.X);
                        byte[] by = BitConverter.GetBytes(p.Y);
                        byte[] blp = new byte[] { bx[0], bx[1], by[0], by[1] };
                        int lParam = BitConverter.ToInt32(blp, 0);
                        this.Capture = false;
                        NativeFunctions.SendMessage(form.Handle, NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_MOVE + HTCAPTION, lParam);
                        m_MouseDownOnCaption = false;
                        return;
                    }
                }
                if (m_TitleTextMarkup != null)
                    m_TitleTextMarkup.MouseMove(this, e);
                e = TranslateMouseEventArgs(e);
            }
            base.OnMouseMove(e);
        }

        protected override void InternalOnClick(MouseButtons mb, Point mousePos)
        {
            if (m_CaptionVisible)
            {
                MouseEventArgs e = new MouseEventArgs(mb, 0, mousePos.X, mousePos.Y, 0);
                e = TranslateMouseEventArgs(e);
                mousePos = new Point(e.X, e.Y);
            }

            base.InternalOnClick(mb, mousePos);
        }

        private MouseEventArgs TranslateMouseEventArgs(MouseEventArgs e)
        {
            if (e.Y <= 6)
            {
                Form form = this.FindForm();
                if (form != null && form.WindowState == FormWindowState.Maximized && form is Office2007RibbonForm)
                {
                    if (e.X <= 4)
                    {
                        BaseItem sb = GetApplicationButton();
                        if (sb != null)
                        {
                            e = new MouseEventArgs(e.Button, e.Clicks, sb.LeftInternal + 1, sb.TopInternal + 1, e.Delta);
                        }
                    }
                    else if (e.X >= this.Width - 6)
                        e = new MouseEventArgs(e.Button, e.Clicks, this.SystemCaptionItem.DisplayRectangle.Right - 4, this.SystemCaptionItem.DisplayRectangle.Top + 4, e.Delta);
                    else
                        e = new MouseEventArgs(e.Button, e.Clicks, e.X, this.CaptionContainerItem.TopInternal + 5, e.Delta);
                }
            }
            return e;
        }

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    base.OnMouseUp(e);
        //    if (m_CaptionVisible) m_CaptionContainer.InternalMouseUp(e);
        //}

        //protected override void OnClick(EventArgs e)
        //{
        //    if (m_CaptionVisible) m_CaptionContainer.InternalClick(MouseButtons, MousePosition);
        //    base.OnClick(e);
        //}

        protected override void OnClick(EventArgs e)
        {
            if (m_CaptionVisible)
            {
                if (m_TitleTextMarkup != null)
                    m_TitleTextMarkup.Click(this);
            }
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (m_CaptionVisible)
            {
                // Check whether double click is on caption so window can be maximized/restored
                Point p = this.PointToClient(Control.MousePosition);
                if (HitTestCaption(p))
                {
                    Form form = this.FindForm();
                    if (form != null && form.MaximizeBox && (form.FormBorderStyle == FormBorderStyle.Sizable || form.FormBorderStyle == FormBorderStyle.SizableToolWindow))
                    {
                        if (form.WindowState == FormWindowState.Normal)
                        {
                            NativeFunctions.PostMessage(form.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_MAXIMIZE, 0);
                        }
                        else if (form.WindowState == FormWindowState.Maximized)
                        {
                            NativeFunctions.PostMessage(form.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_RESTORE, 0);
                        }
                    }
                }
                //m_CaptionContainer.InternalDoubleClick(MouseButtons, MousePosition);
            }
            base.OnDoubleClick(e);
        }

        /// <summary>
        /// Returns true if point is inside the caption area.
        /// </summary>
        /// <param name="p">Client point coordinates.</param>
        /// <returns>True if point is inside of caption area otherwise false.</returns>
        protected bool HitTestCaption(Point p)
        {
            if (m_CaptionBounds.Contains(p) && m_CaptionTextBounds != null)
            {
                foreach (Rectangle r in m_CaptionTextBounds)
                {
                    if (r.Contains(p))
                        return true;
                }
            }
            return false;
        }

        //protected override void PaintControl(ItemPaintArgs pa)
        //{
        //    base.PaintControl(pa);

        //    if (m_CaptionContainer == null || this.IsDisposed || !m_CaptionVisible)
        //        return;

        //    m_CaptionContainer.Paint(pa);
        //}

        //protected override void RecalcSize()
        //{
        //    base.RecalcSize();

        //    if (!BarFunctions.IsHandleValid(this) || IsUpdateSuspended || !m_CaptionVisible)
        //        return;

        //    m_CaptionContainer.LeftInternal = 0;
        //    m_CaptionContainer.TopInternal = 0;
        //    m_CaptionContainer.WidthInternal = this.Width;
        //    m_CaptionContainer.HeightInternal = m_CaptionHeight;
        //    m_CaptionContainer.RecalcSize();
        //}

        private void SystemCaptionClick(object sender, EventArgs e)
        {
            SystemCaptionItem sci = sender as SystemCaptionItem;
            Form frm = this.FindForm();

            if (frm == null)
                return;

            if (sci.LastButtonClick == sci.MouseDownButton)
            {
                if (sci.LastButtonClick == SystemButton.Minimize)
                {
                    NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_MINIMIZE, 0);
                }
                else if (sci.LastButtonClick == SystemButton.Maximize)
                {
                    NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_MAXIMIZE, 0);
                }
                else if (sci.LastButtonClick == SystemButton.Restore)
                {
                    NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_RESTORE, 0);
                }
                else if (sci.LastButtonClick == SystemButton.Close)
                {
                    NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_CLOSE, 0);
                }
                else if (sci.LastButtonClick == SystemButton.Help)
                {
                    NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_CONTEXTHELP, 0);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!m_CaptionVisible)
                return;

            Form frm = this.FindForm();
            if (frm == null)
                return;

            if (frm.WindowState == FormWindowState.Maximized || frm.WindowState == FormWindowState.Minimized)
                m_StripContainer.SystemCaptionItem.RestoreEnabled = true;
            else
                m_StripContainer.SystemCaptionItem.RestoreEnabled = false;
        }

        /// <summary>
        /// Returns collection of items on a bar.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public SubItemsCollection QuickToolbarItems
        {
            get { return m_StripContainer.CaptionContainer.SubItems; }
        }

        /// <summary>
        /// Gets the reference to the internal container item for the items displayed in control caption.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GenericItemContainer CaptionContainerItem
        {
            get { return m_StripContainer.CaptionContainer; }
        }

        /// <summary>
        /// Gets the reference to the internal container for the ribbon tabs and other items.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GenericItemContainer StripContainerItem
        {
            get { return m_StripContainer.RibbonStripContainer; }
        }

        //internal override bool MenuFocus
        //{
        //    get
        //    {
        //        return base.MenuFocus;
        //    }
        //    set
        //    {
        //        if (this.MenuFocus != value)
        //        {
        //            if (m_CaptionVisible && (m_CaptionContainer.GetHotSubItem() != null || m_CaptionContainer.ExpandedItem()!=null))
        //            {
        //                if (value)
        //                {
        //                    SetMenuFocus(value);
        //                    m_CaptionContainer.SetSystemFocus();
        //                    SetupActiveWindowTimer();
        //                    this.Refresh();
        //                }
        //                else
        //                {
        //                    ReleaseActiveWindowTimer();
        //                    m_CaptionContainer.AutoExpand = false;
        //                    m_CaptionContainer.ReleaseSystemFocus();
        //                    m_CaptionContainer.ContainerLostFocus();
        //                    base.MenuFocus = value;
        //                }
        //            }
        //            else
        //                base.MenuFocus = value;
        //        }
        //    }
        //}

        protected override void PaintKeyTips(Graphics g)
        {
            if (!this.ShowKeyTips)
                return;

            KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, "", GetKeyTipFont(), null);

            Rendering.BaseRenderer renderer = GetRenderer();
            PaintContainerKeyTips(m_StripContainer.RibbonStripContainer, renderer, e);
            if (m_CaptionVisible)
                PaintContainerKeyTips(m_StripContainer.CaptionContainer, renderer, e);
        }

        protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
        {
            Rectangle r = base.GetKeyTipRectangle(g, item, font, keyTip);
            if (this.QuickToolbarItems.Contains(item))
                r.Y += 4;
            return r;
        }
        #endregion

        #region Mdi Child System Item
        internal void ClearMDIChildSystemItems(bool bRecalcLayout)
        {
            if (m_StripContainer.RibbonStripContainer == null)
                return;
            bool recalc = false;
            try
            {
                if (this.Items.Contains("dotnetbarsysiconitem"))
                {
                    this.Items.Remove("dotnetbarsysiconitem");
                    recalc = true;
                }
                if (this.Items.Contains("dotnetbarsysmenuitem"))
                {
                    this.Items.Remove("dotnetbarsysmenuitem");
                    recalc = true;
                }
                if (bRecalcLayout && recalc)
                    this.RecalcLayout();
            }
            catch (Exception)
            {
            }
        }

        internal void ShowMDIChildSystemItems(System.Windows.Forms.Form objMdiChild, bool bRecalcLayout)
        {
            ClearMDIChildSystemItems(bRecalcLayout);

            if (objMdiChild == null)
                return;

            MDISystemItem mdi = new MDISystemItem("dotnetbarsysmenuitem");
            if (!objMdiChild.ControlBox)
                mdi.CloseEnabled = false;
            if (!objMdiChild.MinimizeBox)
                mdi.MinimizeEnabled = false;
            if (!objMdiChild.MaximizeBox)
            {
                mdi.RestoreEnabled = false;
            }
            mdi.ItemAlignment = eItemAlignment.Far;
            mdi.Click += new System.EventHandler(this.MDISysItemClick);

            this.Items.Add(mdi);

            if (bRecalcLayout)
                this.RecalcLayout();
        }

        private void MDISysItemClick(object sender, System.EventArgs e)
        {
            MDISystemItem mdi = sender as MDISystemItem;
            Form frm = this.FindForm();
            if (frm != null)
                frm = frm.ActiveMdiChild;
            if (frm == null)
            {
                ClearMDIChildSystemItems(true);
                return;
            }
            if (mdi.LastButtonClick == SystemButton.Minimize)
            {
                NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_MINIMIZE, 0);
            }
            else if (mdi.LastButtonClick == SystemButton.Restore)
            {
                NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_RESTORE, 0);
            }
            else if (mdi.LastButtonClick == SystemButton.Close)
            {
                NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_CLOSE, 0);
            }
            else if (mdi.LastButtonClick == SystemButton.NextWindow)
            {
                NativeFunctions.PostMessage(frm.Handle.ToInt32(), NativeFunctions.WM_SYSCOMMAND, NativeFunctions.SC_NEXTWINDOW, 0);
            }
        }
        #endregion

        internal void InvokeMouseDown(MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        internal void InvokeMouseMove(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        internal void InvokeMouseUp(MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        internal void InvokeClick(EventArgs e)
        {
            OnClick(e);
        }
    }
}
