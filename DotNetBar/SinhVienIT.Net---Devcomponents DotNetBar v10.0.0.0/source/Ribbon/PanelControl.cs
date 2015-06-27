using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents graphical panel control with support for different visual styles and gradients.
    /// </summary>
    [ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.PanelControlDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), DefaultEvent("Click")]
    public class PanelControl : System.Windows.Forms.Panel, IButtonControl
    {
        #region Private Variables
        private ElementStyle m_Style;
        private ElementStyle m_StyleMouseOver;
        private ElementStyle m_StyleMouseDown;

        private ColorScheme m_ColorScheme = null;
        private eDotNetBarStyle m_ColorSchemeStyle = eDotNetBarStyle.Office2003;

        private bool m_MouseOver = false, m_MouseDown = false;

        private Rectangle m_ClientTextRectangle = Rectangle.Empty;
        private bool m_TextDockConstrained = true;
        private bool m_ShowFocusRectangle;
        private string m_Text = "";

        // Theme Caching Support
        private ThemeTab m_ThemeTab = null;

        private DialogResult m_DialogResult = DialogResult.None;
        private bool m_IsDefault = false;

        private bool m_AntiAlias = true;
        private Color m_CanvasColor = Color.White;

        protected Bitmap m_ThemeCachedBitmap = null;
        private bool m_SuspendPaint = false;
        private TextMarkup.BodyElement m_TextMarkup = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        #endregion

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public PanelControl()
        {
            if (!ColorFunctions.ColorsLoaded)
            {
                NativeFunctions.RefreshSettings();
                NativeFunctions.OnDisplayChange();
                ColorFunctions.LoadColors();
            }

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ContainerControl, true);

            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);

            m_ColorScheme = new ColorScheme(m_ColorSchemeStyle);

            ResetStyle();
            ResetStyleMouseOver();
            ResetStyleMouseDown();

            StyleManager.Register(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) StyleManager.Unregister(this);
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            OnColorSchemeChanged();
        }

        internal TextMarkup.BodyElement TextMarkupElement
        {
            get { return m_TextMarkup; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        /// Gets or sets Bar Color Scheme.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), Category("Appearance"), Description("Gets or sets Bar Color Scheme."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DevComponents.DotNetBar.ColorScheme ColorScheme
        {
            get { return m_ColorScheme; }
            set
            {
                if (value == null)
                    throw new ArgumentException("NULL is not a valid value for this property.");
                m_ColorScheme = value;
                OnColorSchemeChanged();
                if (this.Visible)
                    this.Invalidate();
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeColorScheme()
        {
            return m_ColorScheme.SchemeChanged;
        }

        internal ColorScheme GetColorScheme()
        {
            if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle))
            {
                Rendering.Office2007Renderer r = Rendering.GlobalManager.Renderer as Rendering.Office2007Renderer;
                if (r != null && r.ColorTable.LegacyColors != null)
                    return r.ColorTable.LegacyColors;
            }

            return m_ColorScheme;
        }

        private void OnVisualPropertyChanged(object sender, EventArgs e)
        {
            RefreshStyleSystemColors();

            SetRegion();

            this.Invalidate();
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnRightToLeftChanged(e);
        }

        private eCornerType m_CurrentCornerType = eCornerType.Square;
        private int m_CornerDiameter = 8;
        /// <summary>
        /// Applies any region related settings from style to control.
        /// </summary>
        protected virtual void SetRegion()
        {
            SetRegion(false);
        }
        private void SetRegion(bool bResize)
        {
            if (!CanSetRegion) return;
            ElementStyle style = GetStyle();
            if (bResize || m_CurrentCornerType != style.CornerType || m_CornerDiameter != style.CornerDiameter)
            {
                m_CurrentCornerType = style.CornerType;
                m_CornerDiameter = style.CornerDiameter;
                if (style != null && !(DrawThemedPane && BarFunctions.ThemedOS) && style.CornerType != eCornerType.Square)
                {
                    this.Region = ElementStyleDisplay.GetStyleRegion(new ElementStyleDisplayInfo(style, null, new Rectangle(0,0,this.Width, this.Height)));
                }
                else
                    this.Region = null;
            }
        }
        internal bool CanSetRegion = true;

        /// <summary>
        /// Applies color scheme colors to the style objects.
        /// </summary>
        public void RefreshStyleSystemColors()
        {
            ColorScheme cs = GetColorScheme();
            if (m_Style != null)
                m_Style.SetColorScheme(cs);
            if (m_StyleMouseOver != null)
                m_StyleMouseOver.SetColorScheme(cs);
            if (m_StyleMouseDown != null)
                m_StyleMouseDown.SetColorScheme(cs);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_MOUSEWHEEL = 0x020A;
            const int WM_HSCROLL = 0x0114;
            const int WM_VSCROLL = 0x0115;

            switch (m.Msg)
            {
                case WM_HSCROLL:
                case WM_VSCROLL:
                case WM_MOUSEWHEEL:
                    if (this.Controls.Count == 0)
                    {
                        RefreshTextClientRectangle();
                        this.Invalidate();
                    }
                    else
                    {
                        base.WndProc(ref m);
                        this.Refresh();
                        return;
                    }
                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Creates the Graphics object for the control.
        /// </summary>
        /// <returns>The Graphics object for the control.</returns>
        public new Graphics CreateGraphics()
        {
            Graphics g = base.CreateGraphics();
            if (m_AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#if FRAMEWORK20
                if (!SystemInformation.IsFontSmoothingEnabled)
#endif
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
            return g;
        }

        protected virtual void BaseWndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);
        }

        /// <summary>
        /// Returns the current style of the control.
        /// </summary>
        /// <returns>Instance of ElementStyle object.</returns>
        protected virtual ElementStyle GetStyle()
        {
            return m_Style;
        }

        /// <summary>
        /// Returns the current mouse down style of the control.
        /// </summary>
        /// <returns>Instance of ElementStyle object.</returns>
        protected virtual ElementStyle GetStyleMouseDown()
        {
            return m_StyleMouseDown;
        }

        /// <summary>
        /// Returns the current mouse over style of the control.
        /// </summary>
        /// <returns>Instance of ElementStyle object.</returns>
        protected virtual ElementStyle GetStyleMouseOver()
        {
            return m_StyleMouseOver;
        }

        /// <summary>
        /// Paints panel using Windows themes.
        /// </summary>
        /// <param name="e">Paint event arguments</param>
        protected virtual void PaintThemed(PaintEventArgs e)
        {
            if (m_ThemeCachedBitmap == null || m_ThemeCachedBitmap.Size != this.ClientRectangle.Size)
            {
                DisposeThemeCachedBitmap();
                Bitmap bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, e.Graphics);
                Graphics gTmp = Graphics.FromImage(bmp);
                try
                {
                    this.ThemeTab.DrawBackground(gTmp, ThemeTabParts.Pane, ThemeTabStates.Normal, new Rectangle(0, 0, bmp.Width, bmp.Height));
                }
                finally
                {
                    gTmp.Dispose();
                }
                e.Graphics.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                ElementStyle style = GetStyle();
                if (style.BackgroundImage != null)
                    BarFunctions.PaintBackgroundImage(e.Graphics, this.ClientRectangle, style.BackgroundImage, style.BackgroundImagePosition, style.BackgroundImageAlpha);
                m_ThemeCachedBitmap = bmp;
            }
            else
            {
                e.Graphics.DrawImage(m_ThemeCachedBitmap, 0, 0, m_ThemeCachedBitmap.Width, m_ThemeCachedBitmap.Height);
            }
        }

        /// <summary>
        /// Prepares paint surface for paint operation. Called as first thing in Paint event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void PaintPrepare(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(m_CanvasColor))
                e.Graphics.FillRectangle(brush, new Rectangle(0,0,this.Width, this.Height));
            ElementStyle style = GetStyle();
            if ((style.BackColor.IsEmpty || style.BackColor.A < 255) &&
                (style.BackColor2.IsEmpty || style.BackColor2.A < 255))
            {
                base.OnPaintBackground(e);
            }

            if (m_AntiAlias)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
        }

        /// <summary>
        /// Paints panel given current style.
        /// </summary>
        /// <param name="e">Paint event arguments</param>
        protected virtual void PaintStyled(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (ElementStyle style = GetStyle().Copy() as ElementStyle)
            {
                if (m_MouseDown && this.Enabled)
                    style.ApplyStyle(GetStyleMouseDown());
                else if (m_MouseOver && this.Enabled)
                    style.ApplyStyle(GetStyleMouseOver());

                PaintInnerContent(e, style, true);

                if (this.Focused && m_ShowFocusRectangle)
                {
                    Rectangle r = this.ClientRectangle;
                    r.Inflate(-2, -2);
                    if (r.Width > 0 && r.Height > 0)
                        ControlPaint.DrawFocusRectangle(g, r);
                }
            }
        }

        /// <summary>
        /// Paints insides of the control.
        /// </summary>
        /// <param name="e">Paint event arguments.</param>
        protected virtual void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
        {
            Graphics g = e.Graphics;
            if (m_TextMarkup == null)
                RefreshTextClientRectangle();
            Rectangle r = new Rectangle(0,0,this.Width, this.Height);
            Rectangle rText = m_ClientTextRectangle;
            rText.Inflate(-1, -1);

            if (!this.Enabled)
                style.TextColor = GetColorScheme().ItemDisabledText;

            ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(style, g, r);
            info.RightToLeft = (this.RightToLeft == RightToLeft.Yes);
            ElementStyleDisplay.Paint(info);

            if (paintText)
            {
                if (m_TextMarkup == null)
                {
                    info.Bounds = rText;
                    ElementStyleDisplay.PaintText(info, this.Text, this.Font);
                }
                else
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, style.TextColor, (this.RightToLeft == RightToLeft.Yes), e.ClipRectangle, true);
                    m_TextMarkup.Render(d);
                }
            }

        }

		private bool m_Painting = false;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_SuspendPaint || m_Painting) return;
			m_Painting=true;
			try
			{
				SmoothingMode sm = e.Graphics.SmoothingMode;
				TextRenderingHint th = e.Graphics.TextRenderingHint;

				PaintPrepare(e);

				if (DrawThemedPane && BarFunctions.ThemedOS)
				{
					PaintThemed(e);
				}
				else
				{
					PaintStyled(e);
				}

				e.Graphics.SmoothingMode = sm;
				e.Graphics.TextRenderingHint = th;

				base.OnPaint(e);
			}
			finally
			{
				m_Painting=false;
			}
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RefreshTextClientRectangle();
            SetRegion(true);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetRegion(true);
        }

        protected override void NotifyInvalidate(Rectangle invalidatedArea)
        {
            base.NotifyInvalidate(invalidatedArea);
            SetRegion(true);
        }

        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    m_SuspendPaint = true;
        //    base.OnEnabledChanged(e);
        //    m_SuspendPaint = false;
        //}

        protected override void OnChangeUICues(UICuesEventArgs e)
        {
            if (m_ShowFocusRectangle)
            {
                if (e.ChangeFocus)
                    this.Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_TextMarkup != null)
                m_TextMarkup.MouseMove(this, e);
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (m_TextMarkup != null)
                m_TextMarkup.MouseDown(this, e);

            if (e.Button == MouseButtons.Left)
            {
                if (!m_MouseDown)
                {
                    m_MouseDown = true;
                    if (GetStyleMouseDown().Custom)
                        this.Invalidate(false);
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_TextMarkup != null)
                m_TextMarkup.MouseUp(this, e);

            base.OnMouseUp(e);

            if (m_MouseDown)
            {
                m_MouseDown = false;
                if (GetStyleMouseDown().Custom)
                    this.Invalidate(false);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (m_TextMarkup != null)
                m_TextMarkup.Click(this);
            base.OnClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!m_MouseOver)
            {
                m_MouseOver = true;
                if (GetStyleMouseOver().Custom)
                    this.Invalidate(false);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_TextMarkup != null)
                m_TextMarkup.MouseLeave(this);

            base.OnMouseLeave(e);

            if (m_MouseOver)
            {
                m_MouseOver = false;
                if (GetStyleMouseOver().Custom)
                    this.Invalidate(false);
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (m_TextMarkup != null)
                m_TextMarkup.InvalidateElementsSize();
            RefreshTextClientRectangle();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            string text = this.Text;

            if (m_TextMarkup != null)
            {
                m_TextMarkup.MouseLeave(this);
                m_TextMarkup.HyperLinkClick -= new EventHandler(TextMarkupLinkClicked);
                m_TextMarkup = null;
            }

            if (TextMarkup.MarkupParser.IsMarkup(ref text))
                m_TextMarkup = TextMarkup.MarkupParser.Parse(text);

            if (m_TextMarkup != null)
            {
                m_TextMarkup.HyperLinkClick += new EventHandler(TextMarkupLinkClicked);
                RefreshTextClientRectangle();
            }
            base.OnTextChanged(e);
        }

        private void TextMarkupLinkClicked(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
            if (link != null)
            {
                OnMarkupLinkClick(new MarkupLinkClickEventArgs(link.Name, link.HRef));
            }
        }

        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (this.MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }

        protected virtual void ResizeMarkup()
        {
            if (m_TextMarkup != null)
            {
                Rectangle r = this.ClientTextRectangle;
                r.Inflate(-2, 0);
                r.X += ElementStyleLayout.LeftWhiteSpace(this.Style);
                r.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(this.Style);
                r.Y += ElementStyleLayout.TopWhiteSpace(this.Style);
                r.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(this.Style);
                
                if (r.Width <= 0 || r.Height <= 0)
                    return;
                Graphics g = this.CreateGraphics();
                try
                {
                    if (m_AntiAlias)
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                    }
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, SystemColors.Control, (this.RightToLeft == RightToLeft.Yes));
                    m_TextMarkup.Measure(r.Size, d);
                    m_TextMarkup.Arrange(r, d);
                    if (this.AutoScroll)
                    {
                        Size autoScrollMinSize = Size.Empty;
                        if (m_TextMarkup.Bounds.Height > r.Height)
                            autoScrollMinSize.Height = m_TextMarkup.Bounds.Height + m_Style.MarginTop + m_Style.MarginBottom + 2;
                        if (m_TextMarkup.Bounds.Width > r.Width)
                            autoScrollMinSize.Width = m_TextMarkup.Bounds.Width + m_Style.MarginLeft + m_Style.MarginRight + 2;

                        if (this.AutoScrollMinSize != autoScrollMinSize)
                            this.AutoScrollMinSize = autoScrollMinSize;
                    }
                    else if (!this.AutoScrollMinSize.IsEmpty)
                        this.AutoScrollMinSize = Size.Empty;
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);

            Application.DoEvents();

            m_ColorScheme.Refresh(null, true);
            RefreshStyleSystemColors();

            if (m_ThemeTab != null)
                RefreshThemes();

            this.Invalidate(true);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DisposeThemes();
            base.OnHandleDestroyed(e);
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (IsMnemonic(charCode, this.Text))
            {
                OnClick(new EventArgs());
                return true;
            }
            return base.ProcessMnemonic(charCode);
        }

        /// <summary>
        ///   Gets or sets the text displayed on panel.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Appearance"), Description("Gets or sets the text displayed on panel.")]
        public override string Text
        {
            get { return m_Text; }
            set
            {
                if (value.Length < 0) return;
                base.Text = value;
                m_Text = value;
                OnTextChanged(new EventArgs());
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether focus rectangle is displayed when control has focus.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(false), Category("Appearance"), Description("Indicates whether focus rectangle is displayed when control has focus.")]
        public bool ShowFocusRectangle
        {
            get { return m_ShowFocusRectangle; }
            set
            {
                m_ShowFocusRectangle = value;
                if (this.DesignMode)
                    this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the canvas color for the panel. Canvas color will be visible on areas of the control that do not get covered
        /// by the style and it will also be used as a base color for style to be painted on.
        /// </summary>
        [Browsable(true), Category("Background"), Description("Gets or sets the canvas color.")]
        public Color CanvasColor
        {
            get
            {
                return m_CanvasColor;
            }
            set
            {
                m_CanvasColor = value;
                if (this.DesignMode)
                    this.Invalidate();
            }
        }
        /// <summary>
        /// Indicates whether CanvasColor should be serialized. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCanvasColor()
        { return (m_CanvasColor != Color.White); }
        /// <summary>
        /// Resets CanvasColor to it's default value. Used by windows forms designer design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCanvasColor()
        {
            m_CanvasColor = Color.White;
        }

        /// <summary>
        /// Gets or sets the panel style.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Indicates panel style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ElementStyle Style
        {
            get { return m_Style; }
            //set
            //{
            //    if (value == null)
            //        throw new InvalidOperationException("Null is not valid value for this property");
            //    m_Style = value; 
            //}
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        public void ResetStyle()
        {
			if(m_Style==null)
			{
				// Set default style
				m_Style = new ElementStyle();
                m_Style.SetColorScheme(GetColorScheme());
				m_Style.StyleChanged += new EventHandler(this.OnVisualPropertyChanged);
			}
			else
				m_Style.Reset();

            RefreshStyleSystemColors();
            this.Invalidate();
        }

        /// <summary>
        /// Resets the internal mouse tracking properties that track whether mouse is over the panel and whether is mouse pressed while over the panel.
        /// </summary>
        public void ResetMouseTracking()
        {
            m_MouseOver = false;
            m_MouseDown = false;
            this.Invalidate();
        }

        /// <summary>
        /// Gets or sets the panel style when mouse hovers over the panel.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Style"), Description("Gets or sets the panel style when mouse hovers over the panel."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ElementStyle StyleMouseOver
        {
            get
            {
                return m_StyleMouseOver;
            }
            //set
            //{
            //    if (value == null)
            //        throw new InvalidOperationException("Null is not valid value for this property.");
            //    m_StyleMouseOver = value;
            //}
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        public void ResetStyleMouseOver()
        {
			if(m_StyleMouseOver==null)
			{
				m_StyleMouseOver = new ElementStyle();
                m_StyleMouseOver.SetColorScheme(GetColorScheme());
				m_StyleMouseOver.StyleChanged += new EventHandler(this.OnVisualPropertyChanged);
			}
			else
				m_StyleMouseOver.Reset();
            this.Invalidate();
        }

        /// <summary>
        /// Gets or sets the panel style when mouse button is pressed on the panel.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Style"), Description("Gets or sets the panel style when mouse button is pressed on the panel."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ElementStyle StyleMouseDown
        {
            get
            {
                return m_StyleMouseDown;
            }
            //set
            //{
            //    if (value == null)
            //        throw new InvalidOperationException("Null is not valid value for this property.");
            //    m_StyleMouseDown = value;
            //}
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        public void ResetStyleMouseDown()
        {
			if(m_StyleMouseDown==null)
			{
				m_StyleMouseDown = new ElementStyle();
                m_StyleMouseDown.SetColorScheme(GetColorScheme());
				m_StyleMouseDown.StyleChanged += new EventHandler(this.OnVisualPropertyChanged);
			}
			else
				m_StyleMouseDown.Reset();
            this.Invalidate();
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set
            {
                if (m_AntiAlias != value)
                {
                    m_AntiAlias = value;
                    this.Invalidate();
                }
            }
        }

        internal eDotNetBarStyle EffectiveColorSchemeStyle
        {
            get
            {
                if (m_ColorSchemeStyle == eDotNetBarStyle.StyleManagerControlled)
                    return StyleManager.GetEffectiveStyle();
                return m_ColorSchemeStyle;
            }
        }
        /// <summary>
        ///     Gets or sets color scheme style.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets color scheme style."), DefaultValue(eDotNetBarStyle.Office2003)]
        public eDotNetBarStyle ColorSchemeStyle
        {
            get { return m_ColorSchemeStyle; }
            set
            {
                m_ColorSchemeStyle = value;
                m_ColorScheme = new ColorScheme(EffectiveColorSchemeStyle);
                OnColorSchemeChanged();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Called after either ColorScheme or ColorSchemeStyle has changed. If you override make sure that you call base implementation so default
        /// processing can occur.
        /// </summary>
        protected virtual void OnColorSchemeChanged()
        {
            RefreshStyleSystemColors();
        }

        protected virtual void RefreshTextClientRectangle()
        {
            Rectangle r = this.DisplayRectangle;
            if (!m_TextDockConstrained)
                return;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.Dock != DockStyle.None)
                {
                    if (ctrl.Dock == DockStyle.Fill)
                    {
                        r = Rectangle.Empty;
                        break;
                    }
                    switch (ctrl.Dock)
                    {
                        case DockStyle.Left:
                            {
                                r.X += ctrl.Width;
                                r.Width -= ctrl.Width;
                                break;
                            }
                        case DockStyle.Right:
                            {
                                r.Width -= ctrl.Width;
                                break;
                            }
                        case DockStyle.Top:
                            {
                                r.Y += ctrl.Height;
                                r.Height -= ctrl.Height;
                                break;
                            }
                        case DockStyle.Bottom:
                            {
                                r.Height -= ctrl.Height;
                                break;
                            }
                    }
                }
            }
            if (r.Width <= 0 || r.Height <= 0)
                r = Rectangle.Empty;
            m_ClientTextRectangle = r;

            ResizeMarkup();
        }

        /// <summary>
        /// Gets or sets whether text rectangle painted on panel is considering docked controls inside the panel. 
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
        public virtual bool TextDockConstrained
        {
            get { return m_TextDockConstrained; }
            set
            {
                m_TextDockConstrained = value;
                if (this.DesignMode)
                    this.Invalidate();
            }

        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (this.DesignMode)
            {
                this.Invalidate();
            }
        }
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (m_TextDockConstrained && this.DesignMode)
            {
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the text rectangle. This property is set by internal implementation and it should not be set by outside code.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ClientTextRectangle
        {
            get { return m_ClientTextRectangle; }
            set { m_ClientTextRectangle = value; }
        }

        /// <summary>
        ///     Applies predefined Panel color scheme to the control.
        /// </summary>
        public void ApplyPanelStyle()
        {
            ApplyPanelStyle(eDotNetBarStyle.Office2003);
        }

        /// <summary>
        ///     Applies predefined Panel color scheme to the control.
        /// </summary>
        public void ApplyPanelStyle(eDotNetBarStyle style)
        {
            this.ColorSchemeStyle = style;

            this.ResetStyle();
            this.ResetStyleMouseDown();
            this.ResetStyleMouseOver();

            m_Style.Border = eStyleBorderType.Solid;
            m_Style.BorderWidth = 1;
            m_Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
            m_Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
            m_Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
            m_Style.BackColorGradientAngle = GetColorScheme().PanelBackgroundGradientAngle;
            m_Style.TextColorSchemePart = eColorSchemePart.PanelText;
            m_Style.TextAlignment = eStyleTextAlignment.Center;
            m_Style.TextLineAlignment = eStyleTextAlignment.Center;

            this.Invalidate();
        }

        /// <summary>
        ///     Applies predefined Button color scheme to the control.
        /// </summary>
        public void ApplyButtonStyle()
        {
            this.ColorSchemeStyle = eDotNetBarStyle.Office2003;

            this.ResetStyle();
            this.ResetStyleMouseDown();
            this.ResetStyleMouseOver();

            this.Style.TextAlignment = eStyleTextAlignment.Center;
            this.Style.TextLineAlignment = eStyleTextAlignment.Center;
            this.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.Style.BackgroundImagePosition = DevComponents.DotNetBar.eStyleBackgroundImage.Tile;
            this.Style.Border = eStyleBorderType.Solid;
            this.Style.BorderWidth = 1;
            this.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.Style.BackColorGradientAngle = 90;
            this.StyleMouseDown.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.StyleMouseDown.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.StyleMouseDown.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBorder;
            this.StyleMouseDown.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedText;
            this.StyleMouseOver.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground;
            this.StyleMouseOver.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground2;
            this.StyleMouseOver.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBorder;
            this.StyleMouseOver.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotText;

            this.Invalidate();
        }

        /// <summary>
        ///     Applies predefined Label color scheme to the control.
        /// </summary>
        public void ApplyLabelStyle()
        {
            this.ResetStyleMouseDown();
            this.ResetStyleMouseOver();

            ApplyLabelStyle(this.Style);
            this.Invalidate();
        }

        /// <summary>
        /// Applies predefined lable style to the ElementStyle object.
        /// </summary>
        /// <param name="style">Reference to ElementStyle object.</param>
        protected virtual void ApplyLabelStyle(ElementStyle style)
        {
            style.Reset();
            style.TextAlignment = eStyleTextAlignment.Center;
            style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            style.BackgroundImagePosition = DevComponents.DotNetBar.eStyleBackgroundImage.Tile;
            style.BorderColorSchemePart = eColorSchemePart.BarDockedBorder;
            style.BorderWidth = 1;
            style.TextColorSchemePart = eColorSchemePart.ItemText;
            style.BackColorGradientAngle = 90;

        }
        /// <summary>
        /// Gets or sets whether painting of the control is suspended.
        /// </summary>
        protected virtual bool SuspendPaint
        {
            get { return m_SuspendPaint; }
            set { m_SuspendPaint = value; }
        }

        #region IButtonControl implementation
        /// <summary>
        /// Gets or sets the value returned to the parent form when the button is clicked.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DialogResult.None), Description("Gets or sets the value returned to the parent form when the button is clicked.")]
        public DialogResult DialogResult
        {
            get
            {
                return m_DialogResult;
            }

            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                {
                    m_DialogResult = value;
                }
            }
        }

        /// <summary>
        /// Notifies a control that it is the default button so that its appearance and behavior is adjusted accordingly.
        /// </summary>
        /// <param name="value">true if the control should behave as a default button; otherwise false.</param>
        public void NotifyDefault(bool value)
        {
            if (m_IsDefault != value)
            {
                m_IsDefault = value;
            }
        }

        /// <summary>
        /// Generates a Click event for the control.
        /// </summary>
        public void PerformClick()
        {
            if (this.CanSelect)
            {
                this.OnClick(EventArgs.Empty);
            }
        }
        #endregion

        #region Themes Support
        /// <summary>
        /// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.DefaultValue(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
        public virtual bool ThemeAware
        {
            get
            {
                return DrawThemedPane;
            }
            set
            {
                DisposeThemeCachedBitmap();
                DrawThemedPane = value;
                this.Invalidate();
            }
        }

        internal bool DrawThemedPane = false;
        private void RefreshThemes()
        {
            DisposeThemeCachedBitmap();
            if (m_ThemeTab != null)
            {
                m_ThemeTab.Dispose();
                m_ThemeTab = new ThemeTab(this);
            }
        }
        private void DisposeThemes()
        {
            if (m_ThemeTab != null)
            {
                m_ThemeTab.Dispose();
                m_ThemeTab = null;
            }
            DisposeThemeCachedBitmap();
        }
        protected void DisposeThemeCachedBitmap()
        {
            if (m_ThemeCachedBitmap != null)
            {
                m_ThemeCachedBitmap.Dispose();
                m_ThemeCachedBitmap = null;
            }
        }
        internal DevComponents.DotNetBar.ThemeTab ThemeTab
        {
            get
            {
                if (m_ThemeTab == null)
                    m_ThemeTab = new ThemeTab(this);
                return m_ThemeTab;
            }
        }
        #endregion
    }
}
