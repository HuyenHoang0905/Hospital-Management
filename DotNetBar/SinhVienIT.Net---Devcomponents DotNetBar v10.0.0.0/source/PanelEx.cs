using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents graphical panel control with support for different visual styles and gradients.
	/// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.PanelExDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), DefaultEvent("Click")]
    public class PanelEx : System.Windows.Forms.Panel, IButtonControl, Controls.INonClientControl
	{
		#region Private Variables
		private ItemStyle m_Style;
		private ItemStyle m_StyleMouseOver;
		private ItemStyle m_StyleMouseDown;

		private ColorScheme m_ColorScheme=null;
		private eDotNetBarStyle m_ColorSchemeStyle=eDotNetBarStyle.Office2003;

		private bool m_MouseOver=false, m_MouseDown=false;

		private Rectangle m_ClientTextRectangle=Rectangle.Empty;
		private bool m_TextDockConstrained=true;
		private bool m_ShowFocusRectangle;
		private string m_Text="";

		// Theme Caching Support
		private ThemeTab m_ThemeTab=null;

		private DialogResult m_DialogResult=DialogResult.None;
		private bool m_IsDefault=false;

		private bool m_AntiAlias=true;
		private Color m_CanvasColor=Color.White;

		protected Bitmap m_ThemeCachedBitmap=null;
        private bool m_SuspendPaint = false;
        private TextMarkup.BodyElement m_TextMarkup = null;
        private bool m_RenderText = true;
        private bool m_RightToLeftLayout = false;
        private bool m_MarkupUsesStyleAlignment = false;
        private Controls.NonClientPaintHandler m_NCPainter = null;
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
		public PanelEx()
		{
			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}

			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor,true);
			this.SetStyle(ControlStyles.ContainerControl,true);

			this.SetStyle(ControlStyles.Selectable,true);
			//this.SetStyle(ControlStyles.StandardDoubleClick,false);

			m_ColorScheme=new ColorScheme(m_ColorSchemeStyle);

			ResetStyle();
			ResetStyleMouseOver();
			ResetStyleMouseDown();

            m_NCPainter = new Controls.NonClientPaintHandler(this, eScrollBarSkin.Optimized);
			this.BackColor=Color.Transparent;

            StyleManager.Register(this);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing) StyleManager.Unregister(this);
            if (m_NCPainter != null)
            {
                m_NCPainter.Dispose();
                //m_NCPainter = null;
            }
            if (m_Style != null) m_Style.VisualPropertyChanged -= new EventHandler(this.OnVisualPropertyChanged);
            if (m_StyleMouseDown != null) m_StyleMouseDown.VisualPropertyChanged -= new EventHandler(this.OnVisualPropertyChanged);
            if (m_StyleMouseOver != null) m_StyleMouseOver.VisualPropertyChanged -= new EventHandler(this.OnVisualPropertyChanged);
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            OnColorSchemeChanged();
        }

        internal void DisableSelection()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            this.TabStop = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether right-to-left mirror placement is turned on. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Layout"), Description("Indicates whether right-to-left mirror placement is turned on. ")]
        public bool RightToLeftLayout
        {
            get { return m_RightToLeftLayout; }
            set
            {
                if (m_RightToLeftLayout != value)
                {
                    m_RightToLeftLayout = value;
                    if (this.IsHandleCreated)
                        this.RecreateHandle();
                }
            }
        }
		
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public new System.Windows.Forms.BorderStyle BorderStyle
		{
			get {return base.BorderStyle;}
			set {base.BorderStyle=value;}
		}

		/// <summary>
		/// Gets or sets Bar Color Scheme. Note that when ColorSchemeStyle property is set to Office 2007 style the color scheme is always retrived from the GlobalManager.Renderer and any\
        /// changes made on this property will not have any effect.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Category("Appearance"),Description("Gets or sets Bar Color Scheme."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DevComponents.DotNetBar.ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				if(value==null)
					throw new ArgumentException("NULL is not a valid value for this property.");
				m_ColorScheme=value;
                OnColorSchemeChanged();
				if(this.Visible)
					this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColorScheme()
		{
            return m_ColorScheme.SchemeChanged && !BarFunctions.IsOffice2007Style(m_ColorSchemeStyle);
		}
		
		private void OnVisualPropertyChanged(object sender, EventArgs e)
		{
            if (m_MarkupUsesStyleAlignment) this.ResizeMarkup();

            if (m_SuspendPaint)
            {
                return;
            }

			RefreshStyleSystemColors();
			
			SetRegion();

            this.Invalidate();
		}

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle))
                RefreshStyleSystemColors();
            base.OnVisibleChanged(e);
        }

		private eCornerType m_CurrentCornerType=eCornerType.Square;
		private int m_CornerDiameter=8;
		private void SetRegion()
		{
			SetRegion(false);
		}
		private void SetRegion(bool bResize)
		{
			if(bResize || m_CurrentCornerType!=m_Style.CornerType || m_CornerDiameter!=m_Style.CornerDiameter)
			{
				m_CurrentCornerType=m_Style.CornerType;
				m_CornerDiameter=m_Style.CornerDiameter;
				if(m_Style!=null && !(DrawThemedPane && BarFunctions.ThemedOS) && m_Style.CornerType!=eCornerType.Square)
				{
					this.Region=m_Style.GetRegion(this.ClientRectangle);
				}
				else
					this.Region=null;
			}
		}

		/// <summary>
		/// Applies color scheme colors to the style objects.
		/// </summary>
		public void RefreshStyleSystemColors()
		{
			ColorScheme cs=GetColorScheme();
			if(m_Style!=null)
				m_Style.ApplyColorScheme(cs);
			if(m_StyleMouseOver!=null)
				m_StyleMouseOver.ApplyColorScheme(cs);
			if(m_StyleMouseDown!=null)
				m_StyleMouseDown.ApplyColorScheme(cs);
		}

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            bool callBase = true;

            switch (m.Msg)
            {
                case (int)WinApi.WindowsMessages.WM_HSCROLL:
                case (int)WinApi.WindowsMessages.WM_VSCROLL:
                case (int)WinApi.WindowsMessages.WM_MOUSEWHEEL:
                    {
                        if(this.Controls.Count==0)
                            m_SuspendPaint = true;
                        if (m_NCPainter != null)
                            m_NCPainter.SuspendPaint = true;

                        try
                        {
                            if (m_NCPainter != null)
                                callBase = m_NCPainter.WndProc(ref m);
                            if (callBase)
                                base.WndProc(ref m);
                        }
                        finally
                        {
                            if (this.Controls.Count == 0)
                                m_SuspendPaint = false;
                            if (m_NCPainter != null)
                                m_NCPainter.SuspendPaint = false;
                        }
                        if (m_NCPainter != null)
                            m_NCPainter.PaintNonClientAreaBuffered();

                        if (this.Controls.Count == 0)
                        {
                            RefreshTextClientRectangle();
                            this.Invalidate();
                        }
                        return;
                    }
            }

            if (m_NCPainter != null)
                callBase = m_NCPainter.WndProc(ref m);

            if ((m.Msg == (int)WinApi.WindowsMessages.WM_NCPAINT || m.Msg == (int)WinApi.WindowsMessages.WM_PAINT) && this.AutoScroll && this.VScroll && this.HScroll)
            {
                PaintScrollCorner();
            }

            if (callBase)
                base.WndProc(ref m);
        }

	    private void PaintScrollCorner()
	    {
	        IntPtr dc = WinApi.GetWindowDC(this.Handle);
	        try
	        {
	            using (Graphics g = Graphics.FromHdc(dc))
	            {
	                g.FillRectangle(SystemBrushes.Control, this.Width - SystemInformation.VerticalScrollBarWidth,
	                                this.Height - SystemInformation.HorizontalScrollBarHeight,
	                                SystemInformation.VerticalScrollBarWidth,
	                                SystemInformation.HorizontalScrollBarHeight);
	            }
	        }
	        finally
	        {
	            WinApi.ReleaseDC(this.Handle, dc);
	        }
	    }

	    /// <summary>
        /// Gets or sets whether paint operations for the control are suspended.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SuspendPaint
        {
            get { return m_SuspendPaint; }
            set
            {
                if (m_SuspendPaint != value)
                {
                    m_SuspendPaint = value;
                    if (!m_SuspendPaint)
                    {
                        SetRegion();
                        this.Invalidate();
                    }
                }
            }
        }

        private bool IsThemed
        {
            get { return DrawThemedPane && BarFunctions.ThemedOS; }
        }

		protected override void OnPaint(PaintEventArgs e)
		{
            if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0 || m_SuspendPaint)
                return;
            
            using (SolidBrush brush = new SolidBrush(m_CanvasColor))
                e.Graphics.FillRectangle(brush, this.ClientRectangle);

			if((m_Style.BackColor1.Color.IsEmpty || m_Style.BackColor1.Color.A<255) &&
				(m_Style.BackColor2.Color.IsEmpty || m_Style.BackColor2.Color.A<255))
			{
				base.OnPaintBackground(e);
			}

            SmoothingMode sm = e.Graphics.SmoothingMode;
            TextRenderingHint th = e.Graphics.TextRenderingHint;

			if(m_AntiAlias)
			{
				e.Graphics.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}

            ItemStyle style = m_Style.Clone() as ItemStyle;
            if (m_MouseDown && this.Enabled && m_StyleMouseDown.Custom)
                style.ApplyStyle(m_StyleMouseDown);
            else if (m_MouseOver && this.Enabled && m_StyleMouseOver.Custom)
                style.ApplyStyle(m_StyleMouseOver);

            if (m_TextMarkup == null)
                RefreshTextClientRectangle();

            Rectangle r = this.DisplayRectangle; // new Rectangle(0, 0, this.Width, this.Height);
#if FRAMEWORK20
            r.X -= this.Padding.Left;
            r.Y -= this.Padding.Top;
            r.Width += this.Padding.Horizontal;
            r.Height += this.Padding.Vertical;
#else
            r.X -= this.DockPadding.Left;
            r.Y -= this.DockPadding.Top;
            r.Width += this.DockPadding.Left + this.DockPadding.Right;
            r.Height += this.DockPadding.Top + this.DockPadding.Bottom;
#endif
            Rectangle rText = m_ClientTextRectangle;
            rText.Inflate(-1, -1);

            if (!this.Enabled)
                style.ForeColor.Color = m_ColorScheme.ItemDisabledText;

            Graphics g = e.Graphics;
            
			if(IsThemed)
			{
				if(m_ThemeCachedBitmap==null || m_ThemeCachedBitmap.Size!=this.ClientRectangle.Size)
				{
					DisposeThemeCachedBitmap();
					Bitmap bmp=new Bitmap(this.ClientRectangle.Width,this.ClientRectangle.Height,e.Graphics);
					Graphics gTmp=Graphics.FromImage(bmp);
					try
					{
						this.ThemeTab.DrawBackground(gTmp,ThemeTabParts.Pane,ThemeTabStates.Normal,new Rectangle(0,0,bmp.Width,bmp.Height));
					}
					finally
					{
						gTmp.Dispose();
					}
					e.Graphics.DrawImage(bmp,0,0,bmp.Width,bmp.Height);
					if(m_Style.BackgroundImage!=null)
						BarFunctions.PaintBackgroundImage(e.Graphics,this.ClientRectangle,m_Style.BackgroundImage,m_Style.BackgroundImagePosition,m_Style.BackgroundImageAlpha);
					m_ThemeCachedBitmap=bmp;
				}
				else
				{
					e.Graphics.DrawImage(m_ThemeCachedBitmap,0,0,m_ThemeCachedBitmap.Width,m_ThemeCachedBitmap.Height);
				}

                if (this.RenderText)
                {
                    Region oldClip = g.Clip;
                    Rectangle rClip = this.ClientRectangle;
                    rClip.Inflate(-2, -2);
                    rClip.Height -= 3;
                    g.SetClip(rClip);
                    if (m_TextMarkup == null)
                        style.PaintText(g, this.Text, rText, this.Font);
                    else
                    {
                        Rectangle clip = e.ClipRectangle;
                        if (clip.Height == this.ClientRectangle.Height && clip.Width < this.ClientRectangle.Width)
                            clip.Width = this.ClientRectangle.Width;
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, style.ForeColor.Color, (this.RightToLeft == RightToLeft.Yes), clip, true);
                        m_TextMarkup.Render(d);
                    }
                    g.Clip = oldClip;
                }
			}
			else
			{
                if (m_TextMarkup == null)
                {
                    if(m_RenderText)
                        style.Paint(g, r, this.Text, rText, this.Font);
                    else
                        style.Paint(g, r);
                }
                else
                {
                    style.Paint(g, r);
                    if (m_RenderText)
                    {
                        Rectangle clip = e.ClipRectangle;
                        if (clip.Height == this.ClientRectangle.Height && clip.Width < this.ClientRectangle.Width)
                            clip.Width = this.ClientRectangle.Width;
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, style.ForeColor.Color, (this.RightToLeft == RightToLeft.Yes), clip, true);
                        m_TextMarkup.Render(d);
                    }
                }
			}

            if (this.Focused && m_ShowFocusRectangle)
            {
                r = this.ClientRectangle;
                r.Inflate(-2, -2);
                if (r.Width > 0 && r.Height > 0)
                    ControlPaint.DrawFocusRectangle(g, r);
            }

            e.Graphics.SmoothingMode = sm;
            e.Graphics.TextRenderingHint = th;

			base.OnPaint(e);
		}

        internal bool RenderText
        {
            get { return m_RenderText; }
            set { m_RenderText = value; }
        }

		protected override void OnResize(EventArgs e)
		{
            if (m_NCPainter != null) m_NCPainter.PaintNonClientAreaBuffered();
			base.OnResize(e);
            RefreshTextClientRectangle();
			SetRegion(true);
		}

        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    m_SuspendPaint = true;
        //    base.OnEnabledChanged(e);
        //    m_SuspendPaint = false;
        //    this.Invalidate();
        //}

		protected override void OnChangeUICues(UICuesEventArgs e)
		{
			base.OnChangeUICues(e);
			if(m_ShowFocusRectangle)
			{
				this.Refresh();
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if(m_ShowFocusRectangle)
				this.Refresh();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if(m_ShowFocusRectangle)
				this.Refresh();
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
			if(e.Button==MouseButtons.Left)
			{
                SetMouseDown(true);
			}
			base.OnMouseDown(e);
		}

        /// <summary>
        /// Sets the internal mouse down flag which controls appearance of the control. You can use this method to simulate the pressed state for the panel with appropriate StyleMouseDown assigned.
        /// </summary>
        /// <param name="mouseDown">New value for the mouse down flag.</param>
        public void SetMouseDown(bool mouseDown)
        {
            if (m_MouseDown != mouseDown)
            {
                m_MouseDown = mouseDown;
                if (m_StyleMouseDown.Custom)
                    this.Invalidate(false);
            }
        }

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

            if (m_TextMarkup != null)
                m_TextMarkup.MouseUp(this, e);

            SetMouseDown(false);
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

            SetMouseOver(true);
		}

        /// <summary>
        /// Sets the mouse over internal flag that tracks whether the mouse is over the control. You can use this method to simulate the mouse over appearance when appropriate StyleMouseOver style is set.
        /// </summary>
        /// <param name="mouseOver">New value for the mouse over flag.</param>
        public void SetMouseOver(bool mouseOver)
        {
            if (m_MouseOver != mouseOver)
            {
                m_MouseOver = mouseOver;
                if (m_StyleMouseOver.Custom)
                    this.Invalidate(false);
            }
        }

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
            if (m_TextMarkup != null)
                m_TextMarkup.MouseLeave(this);
            SetMouseOver(false);
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);

			Application.DoEvents();
			
			m_ColorScheme.Refresh(null,true);
			RefreshStyleSystemColors();
			
			if(m_ThemeTab!=null)
				RefreshThemes();

			this.Invalidate(true);
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeThemes();
            base.OnHandleDestroyed(e);
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
            string text=this.Text;
            
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
        [Browsable(true), DevCoBrowsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), EditorBrowsable(EditorBrowsableState.Always), Category("Appearance"), Description("Gets or sets the text displayed on panel.")]
		public override string Text
		{
			get {return m_Text;}
			set
			{
                if (m_Text != value)
                {
                    base.Text = value;
                    m_Text = value;
                    OnTextChanged(new EventArgs());
                    this.Refresh();
                }
			}
		}

		/// <summary>
		/// Gets or sets whether focus rectangle is displayed when control has focus.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Appearance"),Description("Indicates whether focus rectangle is displayed when control has focus.")]
		public bool ShowFocusRectangle
		{
			get {return m_ShowFocusRectangle;}
			set
			{
				m_ShowFocusRectangle=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the canvas color for the panel. Canvas color will be visible on areas of the control that do not get covered
		/// by the style and it will also be used as a base color for style to be painted on.
		/// </summary>
		[Browsable(true),Category("Background"),Description("Gets or sets the canvas color.")]
		public Color CanvasColor
		{
			get
			{
				return m_CanvasColor;
			}
			set
			{
				m_CanvasColor=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}
		/// <summary>
		/// Indicates whether CanvasColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCanvasColor()
		{return (m_CanvasColor!=Color.White);}
		/// <summary>
		/// Resets CanvasColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetCanvasColor()
		{
			m_CanvasColor=Color.White;
		}

		/// <summary>
		/// Gets or sets the panel style.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets panel style."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle Style
		{
			get
			{
				return m_Style;
			}
		}
		/// <summary>
		///     Resets the style to it's default value.
		/// </summary>
		public void ResetStyle()
		{
            if (m_Style != null) m_Style.VisualPropertyChanged -= new EventHandler(this.OnVisualPropertyChanged);
			// Set default style
			m_Style=new ItemStyle();			
			m_Style.VisualPropertyChanged+=new EventHandler(this.OnVisualPropertyChanged);
			RefreshStyleSystemColors();
		}

		/// <summary>
		/// Resets the internal mouse tracking properties that track whether mouse is over the panel and whether is mouse pressed while over the panel.
		/// </summary>
		public void ResetMouseTracking()
		{
			m_MouseOver=false;
			m_MouseDown=false;
			this.Refresh();
		}

		/// <summary>
		/// Gets or sets the panel style when mouse hovers over the panel.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the panel style when mouse hovers over the panel."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle StyleMouseOver
		{
			get
			{
				return m_StyleMouseOver;
			}
		}
		/// <summary>
		///     Resets the style to it's default value.
		/// </summary>
		public void ResetStyleMouseOver()
		{
            if (m_StyleMouseOver != null) m_StyleMouseOver.VisualPropertyChanged -= new EventHandler(this.OnVisualPropertyChanged);
			m_StyleMouseOver=new ItemStyle();
			m_StyleMouseOver.VisualPropertyChanged+=new EventHandler(this.OnVisualPropertyChanged);
		}

		/// <summary>
		/// Gets or sets the panel style when mouse button is pressed on the panel.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the panel style when mouse button is pressed on the panel."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle StyleMouseDown
		{
			get
			{
				return m_StyleMouseDown;
			}
		}
		/// <summary>
		///     Resets the style to it's default value.
		/// </summary>
		public void ResetStyleMouseDown()
		{
            if (m_StyleMouseDown != null) m_StyleMouseDown.VisualPropertyChanged -= new EventHandler(this.OnVisualPropertyChanged);
			m_StyleMouseDown=new ItemStyle();
			m_StyleMouseDown.VisualPropertyChanged+=new EventHandler(this.OnVisualPropertyChanged);
		}

		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor=value;}
		}

		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get {return base.BackgroundImage;}
			set {base.BackgroundImage=value;}
		}

		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get {return base.ForeColor;}
			set {base.ForeColor=value;}
		}

		/// <summary>
		/// Gets or sets whether anti-alias smoothing is used while painting.
		/// </summary>
		[DefaultValue(true),Browsable(true),Category("Appearance"),Description("Gets or sets whether anti-aliasing is used while painting.")]
		public bool AntiAlias
		{
			get {return m_AntiAlias;}
			set
			{
				if(m_AntiAlias!=value)
				{
					m_AntiAlias=value;
                    OnAntiAliasChanged();
					this.Refresh();
				}
			}
		}

        /// <summary>
        /// Called when AntiAlias property has changed.
        /// </summary>
        protected virtual void OnAntiAliasChanged() { }

        protected ColorScheme GetColorScheme()
        {
            if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle))
            {
                Rendering.Office2007Renderer r = Rendering.GlobalManager.Renderer as Rendering.Office2007Renderer;
                if (r != null && r.ColorTable.LegacyColors != null)
                    return r.ColorTable.LegacyColors;
            }

            return m_ColorScheme;
        }


		/// <summary>
		///     Gets or sets color scheme style.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Gets or sets color scheme style."),DefaultValue(eDotNetBarStyle.Office2003)]
		public eDotNetBarStyle ColorSchemeStyle
		{
			get {return m_ColorSchemeStyle;}
			set
			{
				m_ColorSchemeStyle=value;
                if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle) && Rendering.GlobalManager.Renderer is Rendering.Office2007Renderer)
                    m_ColorScheme = ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable.LegacyColors;
                else
                    m_ColorScheme = new ColorScheme(m_ColorSchemeStyle);

                if (m_NCPainter != null)
                {
                    if (BarFunctions.IsOffice2007Style(value))
                        m_NCPainter.SkinScrollbars = eScrollBarSkin.Optimized;
                    else
                        m_NCPainter.SkinScrollbars = eScrollBarSkin.None;
                }

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

#if FRAMEWORK20
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (this.Controls.Count == 0)
            {
                if (!BarFunctions.IsHandleValid(this))
                    return proposedSize;
                Size size = GetAutoSize();
                if (size.IsEmpty) return proposedSize;
                return size;
            }
            else
                return base.GetPreferredSize(proposedSize);
        }
#endif

        /// <summary>
        /// Returns the size of the panel calculated based on the text assigned.
        /// </summary>
        /// <returns>Calculated size of the panel or Size.Empty if panel size cannot be calculated.</returns>
        public Size GetAutoSize()
        {
            return GetAutoSize(0);
        }

        /// <summary>
        /// Returns the size of the panel calculated based on the text assigned.
        /// </summary>
        /// <returns>Calculated size of the panel or Size.Empty if panel size cannot be calculated.</returns>
        public Size GetAutoSize(int preferedWidth)
        {
            if (m_TextMarkup == null && this.Text.Length == 0) return new Size(16, 16);
            Size size = Size.Empty;

            if (m_TextMarkup != null)
            {
                if (preferedWidth == 0)
                {
                    size = m_TextMarkup.Bounds.Size;
                }
                else
                {
                    size = GetMarkupSize(preferedWidth);
                }
                size.Width += 4;
                size.Height += 6;
            }
            else if (this.Text.Length > 0)
            {
                Font font = this.Font;
                if (m_Style.Font != null) font = m_Style.Font;
                using (Graphics g = BarFunctions.CreateGraphics(this))
                {
                    if(preferedWidth<=0)
                        size = TextDrawing.MeasureString(g, this.Text, font);
                    else
                        size = TextDrawing.MeasureString(g, this.Text, font, preferedWidth);
                }
                size.Width += 2;
                size.Height += 2;
            }
            if (size.IsEmpty) return size;

            size.Width += m_Style.MarginLeft + m_Style.MarginRight;
            size.Height += m_Style.MarginTop + m_Style.MarginBottom;

            return size;
        }

		protected virtual void RefreshTextClientRectangle()
		{
			Rectangle r=this.DisplayRectangle;

            if (m_TextDockConstrained)
            {
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
            }

			m_ClientTextRectangle=r;

            ResizeMarkup();

		}

        private Size GetMarkupSize(int proposedWidth)
        {
            Size size = Size.Empty;
            if (m_TextMarkup != null)
            {
                Rectangle r = new Rectangle(0, 0, proposedWidth, 500);
                r.Inflate(-2, -2);
                r.Inflate(-(m_Style.MarginLeft + m_Style.MarginRight), -(m_Style.MarginTop + m_Style.MarginBottom));
                if (this.IsThemed)
                    r.Height -= 4;
                Graphics g = this.CreateGraphics();
                TextMarkup.BodyElement markup = TextMarkup.MarkupParser.Parse(this.Text);
                try
                {
                    if (m_AntiAlias)
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                    }
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, SystemColors.Control, (this.RightToLeft == RightToLeft.Yes));
                    markup.Measure(r.Size, d);
                    size = markup.Bounds.Size;
                }
                finally
                {
                    g.Dispose();
                }
            }

            return size;
        }

        /// <summary>
        /// Updates the markup size to reflect current position of the scrollbars. You must call this method if you are scrolling control with markup using the AutoScrollPosition property.
        /// </summary>
        public void UpdateMarkupSize()
        {
            RefreshTextClientRectangle();
            this.Invalidate();
        }

        protected virtual void ResizeMarkup()
        {
            if (m_TextMarkup != null)
            {
                Rectangle r = this.ClientTextRectangle;
                r.Inflate(-2, -2);
                r.Inflate(-(m_Style.MarginLeft + m_Style.MarginRight), -(m_Style.MarginTop + m_Style.MarginBottom));
                if (this.IsThemed)
                    r.Height -= 4;
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
                    if (m_MarkupUsesStyleAlignment)
                    {
                        if (m_TextMarkup.Bounds.Height < r.Height)
                        {
                            if (this.Style.LineAlignment == StringAlignment.Center)
                            {
                                r.Y += (r.Height - m_TextMarkup.Bounds.Height) / 2;
                                r.Height = m_TextMarkup.Bounds.Height;
                            }
                            else if (this.Style.LineAlignment == StringAlignment.Far)
                            {
                                r.Y = (r.Bottom - m_TextMarkup.Bounds.Height);
                                r.Height = m_TextMarkup.Bounds.Height;
                            }
                        }

                        if (m_TextMarkup.Bounds.Width < r.Width)
                        {
                            if (this.Style.Alignment == StringAlignment.Center)
                            {
                                r.X += (r.Width - m_TextMarkup.Bounds.Width) / 2;
                                r.Width = m_TextMarkup.Bounds.Width;
                            }
                            else if (this.Style.Alignment == StringAlignment.Far && this.RightToLeft == RightToLeft.No || 
                                this.Style.Alignment == StringAlignment.Near && this.RightToLeft== RightToLeft.Yes)
                            {
                                r.X = (r.Right - m_TextMarkup.Bounds.Width);
                                r.Width = m_TextMarkup.Bounds.Width;
                            }
                        }
                    }
                    
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

        /// <summary>
        /// Gets or sets a value indicating whether the container will allow the user to scroll to any controls placed outside of its visible boundaries.
        /// </summary>
        [DefaultValue(false), Browsable(true)]
        public override bool AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                if (!value)
                    this.AutoScrollMinSize = Size.Empty;
                base.AutoScroll = value;
                this.RefreshTextClientRectangle();
            }
        }

		/// <summary>
		/// Gets or sets whether text rectangle painted on panel is considering docked controls inside the panel. 
		/// </summary>
		[DefaultValue(true),Category("Appearance"),Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
		public bool TextDockConstrained
		{
			get {return m_TextDockConstrained;}
			set
			{
				m_TextDockConstrained=value;
				if(this.DesignMode)
					this.Refresh();
			}
		
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
            RefreshTextClientRectangle();
			if(this.DesignMode)
			{
				this.Refresh();
			}
		}
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
            RefreshTextClientRectangle();
			if(m_TextDockConstrained && this.DesignMode)
			{
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the text rectangle. This property is set by internal implementation and it should not be set by outside code.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle ClientTextRectangle
		{
			get {return m_ClientTextRectangle;}
            set { m_ClientTextRectangle = value; }
		}

		/// <summary>
		///     Applies predefined Panel color scheme to the control.
		/// </summary>
		public void ApplyPanelStyle()
		{
			//this.ColorSchemeStyle=eDotNetBarStyle.Office2003;

			this.ResetStyle();
			this.ResetStyleMouseDown();
			this.ResetStyleMouseOver();

			m_Style.Border=eBorderType.SingleLine;
			m_Style.BorderColor.ColorSchemePart=eColorSchemePart.PanelBorder;
			m_Style.BackColor1.ColorSchemePart=eColorSchemePart.PanelBackground;
			m_Style.BackColor2.ColorSchemePart=eColorSchemePart.PanelBackground2;
			m_Style.GradientAngle=m_ColorScheme.PanelBackgroundGradientAngle;
			m_Style.ForeColor.ColorSchemePart=eColorSchemePart.PanelText;
			m_Style.Alignment=StringAlignment.Center;
			m_Style.LineAlignment=StringAlignment.Center;

			this.Refresh();
		}

		/// <summary>
		///     Applies predefined Button color scheme to the control.
		/// </summary>
		public void ApplyButtonStyle()
		{
			//this.ColorSchemeStyle=eDotNetBarStyle.Office2003;

			this.ResetStyle();
			this.ResetStyleMouseDown();
			this.ResetStyleMouseOver();

			this.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			//this.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
			this.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
			this.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
			this.Style.GradientAngle = 90;
			this.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
			this.StyleMouseDown.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
			this.StyleMouseDown.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
			this.StyleMouseDown.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBorder;
			this.StyleMouseDown.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedText;
			this.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;
			this.StyleMouseOver.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground;
			this.StyleMouseOver.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground2;
			this.StyleMouseOver.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBorder;
			this.StyleMouseOver.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotText;

			this.Refresh();
		}

		/// <summary>
		///     Applies predefined Label color scheme to the control.
		/// </summary>
		public void ApplyLabelStyle()
		{
			this.ResetStyle();
			this.ResetStyleMouseDown();
			this.ResetStyleMouseOver();

			TypeDescriptor.GetProperties(this.Style)["Alignment"].SetValue(this.Style,System.Drawing.StringAlignment.Center);
			TypeDescriptor.GetProperties(this.Style.BackColor1)["ColorSchemePart"].SetValue(this.Style.BackColor1,DevComponents.DotNetBar.eColorSchemePart.BarBackground);
			//TypeDescriptor.GetProperties(this.Style.BackColor2)["ColorSchemePart"].SetValue(this.Style.BackColor2,DevComponents.DotNetBar.eColorSchemePart.BarBackground2);
			//TypeDescriptor.GetProperties(this.Style)["BackgroundImagePosition"].SetValue(this.Style,eBackgroundImagePosition.Tile);
			TypeDescriptor.GetProperties(this.Style.BorderColor)["ColorSchemePart"].SetValue(this.Style.BorderColor,eColorSchemePart.BarDockedBorder);
			TypeDescriptor.GetProperties(this.Style.ForeColor)["ColorSchemePart"].SetValue(this.Style.ForeColor,eColorSchemePart.ItemText);
			TypeDescriptor.GetProperties(this.Style)["GradientAngle"].SetValue(this.Style,90);
			//this.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
			//this.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;

			this.Refresh();
		}

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (m_RightToLeftLayout)
                {
                    const int WS_EX_LAYOUTRTL = 0x400000;
                    const int WS_EX_NOINHERITLAYOUT = 0x100000;
                    cp.ExStyle |= WS_EX_LAYOUTRTL | WS_EX_NOINHERITLAYOUT;
                }
                return cp;
            }
        }

        /// <summary>
        /// Gets or sets whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control. Default value is false.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(false), Description("Indicates whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control.")]
        public virtual bool MarkupUsesStyleAlignment
        {
            get { return m_MarkupUsesStyleAlignment; }
            set
            {
                if (m_MarkupUsesStyleAlignment != value)
                {
                    m_MarkupUsesStyleAlignment = value;
                    if (BarFunctions.IsHandleValid(this) && m_TextMarkup!=null)
                    {
                        this.ResizeMarkup();
                        this.Invalidate();
                    }
                }
            }
        }

		#region IButtonControl implementation
		/// <summary>
		/// Gets or sets the value returned to the parent form when the button is clicked.
		/// </summary>
		[Browsable(true),Category("Behavior"),DefaultValue(DialogResult.None),Description("Gets or sets the value returned to the parent form when the button is clicked.")]
		public DialogResult DialogResult
		{
			get
			{
				return m_DialogResult;
			}

			set
			{
				if(Enum.IsDefined(typeof(DialogResult), value))                
				{
					m_DialogResult=value;
				}
			}    
		}

		/// <summary>
		/// Notifies a control that it is the default button so that its appearance and behavior is adjusted accordingly.
		/// </summary>
		/// <param name="value">true if the control should behave as a default button; otherwise false.</param>
		public void NotifyDefault(bool value)
		{
			if(m_IsDefault!=value)
			{
				m_IsDefault=value;
			}
		}

		/// <summary>
		/// Generates a Click event for the control.
		/// </summary>
		public void PerformClick()
		{
			if(this.CanSelect)
			{
				this.OnClick(EventArgs.Empty);
			}
		}
		#endregion

		#region Themes Support
		/// <summary>
		/// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public virtual bool ThemeAware
		{
			get
			{
				return DrawThemedPane;
			}
			set
			{
				DisposeThemeCachedBitmap();
				DrawThemedPane=value;
				this.Refresh();
			}
		}

		internal bool DrawThemedPane=false;
		private void RefreshThemes()
		{
			DisposeThemeCachedBitmap();
			if(m_ThemeTab!=null)
			{
				m_ThemeTab.Dispose();
				m_ThemeTab=new ThemeTab(this);
			}
		}
		private void DisposeThemes()
		{
			if(m_ThemeTab!=null)
			{
				m_ThemeTab.Dispose();
				m_ThemeTab=null;
			}
			DisposeThemeCachedBitmap();
		}
		protected void DisposeThemeCachedBitmap()
		{
			if(m_ThemeCachedBitmap!=null)
			{
				m_ThemeCachedBitmap.Dispose();
				m_ThemeCachedBitmap=null;
			}
		}
		internal DevComponents.DotNetBar.ThemeTab ThemeTab
		{
			get
			{
				if(m_ThemeTab==null)
					m_ThemeTab=new ThemeTab(this);
				return m_ThemeTab;
			}
		}
		#endregion

        #region INonClientControl Members
        private Rendering.BaseRenderer m_DefaultRenderer = null;
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        private Rendering.BaseRenderer GetRenderer()
        {
            if (Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            
            if (m_DefaultRenderer == null)
                m_DefaultRenderer = new Rendering.Office2007Renderer();

            return m_DefaultRenderer;
        }

        void DevComponents.DotNetBar.Controls.INonClientControl.BaseWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        ItemPaintArgs DevComponents.DotNetBar.Controls.INonClientControl.GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(this as IOwner, this, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = false; // m_DesignerSelection;
            pa.GlassEnabled = !this.DesignMode && WinApi.IsGlassEnabled;
            return pa;
        }

        ElementStyle DevComponents.DotNetBar.Controls.INonClientControl.BorderStyle
        {
            get { return null; }
        }

        void DevComponents.DotNetBar.Controls.INonClientControl.PaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        IntPtr DevComponents.DotNetBar.Controls.INonClientControl.Handle
        {
            get { return this.Handle; }
        }

        int DevComponents.DotNetBar.Controls.INonClientControl.Width
        {
            get { return this.Width; }
        }

        int DevComponents.DotNetBar.Controls.INonClientControl.Height
        {
            get { return this.Height; }
        }

        bool DevComponents.DotNetBar.Controls.INonClientControl.IsHandleCreated
        {
            get { return this.IsHandleCreated; }
        }

        Point DevComponents.DotNetBar.Controls.INonClientControl.PointToScreen(Point client)
        {
            return this.PointToScreen(client);
        }

        Color DevComponents.DotNetBar.Controls.INonClientControl.BackColor
        {
            get { return this.BackColor; }
        }
        void DevComponents.DotNetBar.Controls.INonClientControl.AdjustClientRectangle(ref Rectangle r) { }
        void DevComponents.DotNetBar.Controls.INonClientControl.AdjustBorderRectangle(ref Rectangle r) { }
        void DevComponents.DotNetBar.Controls.INonClientControl.RenderNonClient(Graphics g) { }
        #endregion
	}
}
