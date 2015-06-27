using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents panel used by RibbonTabItem as a container panel for the control.
	/// </summary>
    [ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.RibbonPanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class RibbonPanel : PanelControl, IKeyTipsControl
	{
		#region Private Variables
		private const string INFO_TEXT="Drop RibbonBar or other controls here. Drag and Drop tabs and items to re-order.";
		private RibbonTabItem m_RibbonTabItem=null;
		private bool m_UseCustomStyle=false;
        private bool m_DefaultLayout = false;
        private bool m_StretchLastRibbonBar = false;
        private ElementStyle m_DefaultStyle = new ElementStyle();
        private RightToLeft m_RightToLeft = RightToLeft.No;
        private bool m_InternalLayoutSuspend = false;
        private ButtonX m_ButtonScrollRight = null;
        private ButtonX m_ButtonScrollLeft = null;
        private Rectangle m_ViewBounds = Rectangle.Empty;
        private int m_ScrollOffset = 0;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Creates new instance of the panel.
		/// </summary>
		public RibbonPanel():base()
		{
			this.BackColor=SystemColors.Control;
		}

        protected override ElementStyle GetStyle()
        {
            if (!this.Style.Custom)
            {
                return GetDefaultStyle();
            }
            return base.GetStyle();
        }

        private bool m_PopupMode = false;
        private RibbonControl m_RibbonControl = null;
        internal RibbonControl GetRibbonControl()
        {
            if (m_PopupMode)
                return m_RibbonControl;
            return this.Parent as RibbonControl;
        }

        internal bool IsPopupMode
        {
            get { return m_PopupMode; }
        }

        internal void SetPopupMode(bool popupMode, RibbonControl rc)
        {
            m_PopupMode = popupMode;
            if (m_PopupMode)
            {
                m_RibbonControl = rc;

#if FRAMEWORK20
                if (this.Padding.Bottom > 0)
                {
                    this.Height += this.Padding.Bottom;
                    this.Padding = new System.Windows.Forms.Padding(this.Padding.Left, this.Padding.Bottom, this.Padding.Right, this.Padding.Bottom);
                }
#else
                if (this.DockPadding.Bottom > 0)
                {
                    this.DockPadding.Top = this.DockPadding.Bottom;
                    this.Height += this.DockPadding.Bottom;
                }
#endif
            }
            else
            {
#if FRAMEWORK20
                if (this.Padding.Top > 0)
                {
                    this.Height -= this.Padding.Top;
                    this.Padding = new System.Windows.Forms.Padding(this.Padding.Left, 0, this.Padding.Right, this.Padding.Bottom);
                }
#else
                if (this.DockPadding.Top > 0)
                {
                    this.Height -= this.DockPadding.Top;
                    this.DockPadding.Top = 0;
                }
#endif
                m_RibbonControl = null;
            }
        }

        private ElementStyle GetDefaultStyle()
        {
            RibbonControl rc = GetRibbonControl();

            if (rc!=null && this.ColorSchemeStyle != rc.Style)
                this.ColorSchemeStyle = rc.Style;

            m_DefaultStyle.SetColorScheme(this.ColorScheme);

            if (BarFunctions.IsOffice2007Style(this.EffectiveColorSchemeStyle) || rc != null && BarFunctions.IsOffice2007Style(rc.EffectiveStyle))
            {
                m_DefaultStyle.Reset();

                Rendering.Office2007ColorTable ct = null;
                if (rc != null)
                    ct = rc.GetOffice2007ColorTable();
                else if (Rendering.GlobalManager.Renderer is Rendering.Office2007Renderer)
                {
                    ct = ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable;
                }
                else
                    return this.Style;

                m_DefaultStyle.Border = eStyleBorderType.Double;
                if (rc == null || !rc.IsPopupMode)
                    m_DefaultStyle.BorderTop = eStyleBorderType.None;

                m_DefaultStyle.BorderWidth = 1;
                m_DefaultStyle.CornerDiameter = ct.RibbonControl.CornerSize;
                m_DefaultStyle.CornerType = eCornerType.Rounded;
                if (!m_PopupMode)
                {
                    m_DefaultStyle.CornerTypeTopLeft = eCornerType.Square;
                    m_DefaultStyle.CornerTypeTopRight = eCornerType.Square;
                }
                else if (ct.InitialColorScheme == DevComponents.DotNetBar.Rendering.eOffice2007ColorScheme.Black)
                    m_DefaultStyle.BorderTop = eStyleBorderType.None;
                if (rc != null && rc.EffectiveStyle == eDotNetBarStyle.Office2010)
                {
                    m_DefaultStyle.CornerTypeBottomRight = eCornerType.Square;
                    m_DefaultStyle.CornerTypeBottomLeft = eCornerType.Square;
                    if (rc.RibbonStrip.IsGlassEnabled)
                    {
                        m_DefaultStyle.BorderLeftWidth = 0;
                        m_DefaultStyle.BorderRightWidth = 0;
                        m_DefaultStyle.BorderLeft = eStyleBorderType.None;
                        m_DefaultStyle.BorderRight = eStyleBorderType.None;
                    }
                }
                else if (rc != null && rc.EffectiveStyle == eDotNetBarStyle.Windows7)
                {
                    m_DefaultStyle.CornerType = eCornerType.Square;
                    m_DefaultStyle.CornerTypeBottomRight = eCornerType.Square;
                    m_DefaultStyle.CornerTypeBottomLeft = eCornerType.Square;
                    m_DefaultStyle.CornerTypeTopRight = eCornerType.Square;
                    m_DefaultStyle.CornerTypeTopLeft = eCornerType.Square;
                    m_DefaultStyle.CornerDiameter = 0;
                    if (rc.RibbonStrip.IsGlassEnabled)
                    {
                        m_DefaultStyle.BorderLeft = eStyleBorderType.None;
                        m_DefaultStyle.BorderRight = eStyleBorderType.None;
                    }
                }

                // Border Colors
                m_DefaultStyle.BorderColor = ct.RibbonControl.OuterBorder.Start;
                m_DefaultStyle.BorderColor2 = ct.RibbonControl.OuterBorder.End;
                m_DefaultStyle.BorderColorLight = ct.RibbonControl.InnerBorder.Start;
                m_DefaultStyle.BorderColorLight2 = ct.RibbonControl.InnerBorder.End;

                // Background colors
                m_DefaultStyle.BackColorGradientAngle = 90;
                m_DefaultStyle.BackColor = ct.RibbonBar.Default.TopBackground.Start;
                if (ct.RibbonControl.PanelTopBackgroundHeight==  0 && ct.RibbonControl.PanelBottomBackground==null)
                {
                    m_DefaultStyle.BackColor = ct.RibbonControl.PanelTopBackground.Start;
                    m_DefaultStyle.BackColor2 = ct.RibbonControl.PanelTopBackground.End;
                }
                else
                {
                    m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(ct.RibbonControl.PanelTopBackground.Start, 0));
                    m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(ct.RibbonControl.PanelTopBackground.End, ct.RibbonControl.PanelTopBackgroundHeight));
                    m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(ct.RibbonControl.PanelBottomBackground.Start, ct.RibbonControl.PanelTopBackgroundHeight));
                    m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(ct.RibbonControl.PanelBottomBackground.End, 1));
                }

            }
            else
            {
                ApplyLabelStyle(m_DefaultStyle);
                m_DefaultStyle.BorderBottom = eStyleBorderType.Solid;
            }

            return m_DefaultStyle;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (!(e.Control is RibbonBar))
            {
                m_DefaultLayout = false;
            }
            base.OnControlAdded(e);
        }

        internal bool InternalLayoutSuspend
        {
            get { return m_InternalLayoutSuspend; }
            set { m_InternalLayoutSuspend = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (m_DefaultLayout || this.DesignMode)
            {
                base.OnLayout(levent);
                return;
            }

            if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0 || !this.IsHandleCreated)
                return;

            Form f = this.FindForm();
            if (f != null && f.WindowState == FormWindowState.Minimized)
                return;

            // Has RightToLeft changed, if so mirror content
            if (m_RightToLeft != this.RightToLeft)
            {
                m_RightToLeft = this.RightToLeft;
                Rectangle r = GetLayoutRectangle();
                foreach (Control c in this.Controls)
                {
                    if (!(c is RibbonBar))
                        continue;
                    // Mirror the X position of each RibbonBar
                    c.Left = r.Width - c.Width - c.Left;
                }
            }

            if(!m_InternalLayoutSuspend)
                LayoutRibbons();
        }

        private Rectangle GetLayoutRectangle()
        {
            Rectangle r = this.ClientRectangle;
            r.X += this.DockPadding.Left;
            r.Width -= this.DockPadding.Left + this.DockPadding.Right;
            r.Y += this.DockPadding.Top;
            r.Height -= this.DockPadding.Top + (this.EffectiveColorSchemeStyle == eDotNetBarStyle.Office2010 && this.DockPadding.Bottom == 3 ? 2 : this.DockPadding.Bottom);

            return r;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void LayoutRibbons()
        {
            int spacing = 2;
            Rectangle r = GetLayoutRectangle();
            int currentWidth = -spacing;
            
            ArrayList resizeOrder = new ArrayList();
            ArrayList layoutOrder = new ArrayList();
            ArrayList overflowList = new ArrayList();
            Control lastVisibleControl = null;
            int lastX = 0;
            bool resizeOrderSpecified = false;
            
            RibbonBar barGallery = null;
            foreach (Control c in this.Controls)
            {
                RibbonBar bar = c as RibbonBar;
                if (bar == null || !bar.Visible) continue;
                int barContentBasedWidth = bar.GetContentBasedSize().Width;

                currentWidth += barContentBasedWidth + spacing;
                
                if (bar.GalleryStretch != null && bar.GalleryStretch.Visible) barGallery = bar;

                if (bar.ResizeOrderIndex != 0)
                    resizeOrderSpecified = true;
                resizeOrder.Add(bar);
                layoutOrder.Add(bar);
                if (bar.OverflowState) overflowList.Add(bar);
                if (bar.Left > lastX)
                {
                    lastX = bar.Left;
                    lastVisibleControl = c;
                }
            }

            layoutOrder.Sort(new XPositionComparer());
            if (resizeOrderSpecified)
            {
                resizeOrder.Sort(new ResizeOrderComparer());
                overflowList.Sort(new ResizeOrderComparer());
            }
            else
            {
                resizeOrder.Sort(new XPositionComparer());
                overflowList.Sort(new XPositionComparer());
            }

            int lastControlIncrease = 0;

            if (currentWidth > r.Width)
            {
                // One or more ribbons must be reduced in size...
                int totalReduction = currentWidth - r.Width;

                // Check whether gallery can be reduced first
                if (barGallery != null && !barGallery.GalleryStretch.IsAtMinimumSize) // barGallery.GalleryStretch.WidthInternal > barGallery.GalleryStretch.MinimumSize.Width)
                {
                    int oldWidth = barGallery.GetContentBasedSize().Width;
                    if (barGallery.GalleryStretch.WidthInternal - barGallery.GalleryStretch.MinimumSize.Width >= totalReduction)
                        barGallery.Size = new Size(oldWidth - totalReduction, r.Height);
                    else
                        barGallery.Size = new Size(oldWidth - (barGallery.GalleryStretch.WidthInternal - barGallery.GalleryStretch.MinimumSize.Width), r.Height);
                    totalReduction -= (oldWidth - barGallery.GetContentBasedSize().Width);
                }

                if (totalReduction > 0)
                {
                    int c = resizeOrder.Count - 1;
                    for (int i = c; i >= 0; i--)
                    {
                        RibbonBar bar = resizeOrder[i] as RibbonBar;
                        int oldWidth = bar.GetContentBasedSize().Width;
                        if (oldWidth > totalReduction && bar.AutoSizeItems)
                            bar.Size = new Size(oldWidth - totalReduction, r.Height);
                        else
                            bar.Size = new Size(1, r.Height);
                        totalReduction -= (oldWidth - bar.GetContentBasedSize().Width);
                        if (totalReduction <= 0) break;
                    }
                }

                if (totalReduction != 0) lastControlIncrease = Math.Abs(totalReduction);
            }
            else if (currentWidth < r.Width)
            {
                // One or more ribbons must be increased in size
                int totalIncrease = r.Width - currentWidth;
                // Start with overflows first
                ArrayList overflowed = new ArrayList();
                overflowed.AddRange(overflowList.ToArray());
                foreach(RibbonBar bar in overflowed)
                {
                    int oldWidth = bar.GetContentBasedSize().Width;
                    bar.Size = new Size(oldWidth + totalIncrease, r.Height);
                    totalIncrease -= (bar.GetContentBasedSize().Width - oldWidth);
                    if (totalIncrease >= 0 && !bar.OverflowState)
                        overflowList.Remove(bar);

                    if (totalIncrease <= 0 || bar.GetContentBasedSize().Width - oldWidth == 0) break;
                }

                if (totalIncrease > 0 && overflowList.Count == 0)
                {
                    int c = resizeOrder.Count - 1;
                    for (int i = c; i >= 0; i--)
                    {
                        RibbonBar bar = resizeOrder[i] as RibbonBar;
                        if (bar.OverflowState || overflowed.Contains(bar)) continue;
                        int oldWidth = bar.GetContentBasedSize().Width;
                        if (oldWidth == bar.Width && bar.LastReducedSize.IsEmpty && bar.BeforeOverflowSize.IsEmpty && bar.GalleryStretch == null)
                            continue;
                        if (bar.Width == oldWidth + totalIncrease)
                        {
                            if(!(bar == lastVisibleControl && m_StretchLastRibbonBar)) // If not stretching last bar and current bar is last one
                                totalIncrease = 0;
                            break;
                        }
                        bar.Size = new Size(oldWidth + totalIncrease, r.Height);
                        totalIncrease -= (bar.GetContentBasedSize().Width - oldWidth);
                        if (totalIncrease <= 0) break;
                    }
                }
                if (totalIncrease > 0) lastControlIncrease = totalIncrease;
            }

            RepositionRibbons(layoutOrder, r, spacing, lastVisibleControl, lastControlIncrease);
        }
        
        private void RepositionRibbons(ArrayList layoutOrder, Rectangle layoutRectangle, int spacing, Control lastVisibleControl, int lastControlIncrease)
        {
            Point p = layoutRectangle.Location;

            // Is there an offset?
            int offset = 0;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                int currentWidth = 0;
                foreach (RibbonBar bar in layoutOrder)
                {
                    currentWidth += bar.GetContentBasedSize().Width + spacing;
                }
                if (currentWidth < layoutRectangle.Width)
                {
                    offset = spacing + layoutRectangle.Width - currentWidth;
                }
            }
            Rectangle viewBounds = Rectangle.Empty;
            p.X += m_ScrollOffset;
            foreach (RibbonBar bar in layoutOrder)
            {
                Size contentSize = bar.GetContentBasedSize();
                if (bar == lastVisibleControl && m_StretchLastRibbonBar && !bar.OverflowState)
                    contentSize.Width += lastControlIncrease;
                //Rectangle bounds = new Rectangle(p.X, p.Y, width, r.Height);
                Rectangle bounds = new Rectangle(p.X + offset, p.Y, contentSize.Width, layoutRectangle.Height);
                if (this.DesignMode)
                    TypeDescriptor.GetProperties(bar)["Bounds"].SetValue(bar, bounds);
                else
                    bar.Bounds = bounds;
                viewBounds = Rectangle.Union(bounds, viewBounds);
                p.X += (bar.Width + spacing);
            }

            m_ViewBounds = viewBounds;
            if (m_ViewBounds.Right > this.Width)
            {
                if (m_ButtonScrollRight == null)
                {
                    m_ButtonScrollRight = new ButtonX();
                    m_ButtonScrollRight.Text = "<expand direction=\"right\"/>";
                    m_ButtonScrollRight.ColorTable = eButtonColor.OrangeWithBackground;
                    m_ButtonScrollRight.Click += new EventHandler(ScrollPanelRightClick);
                }
                m_ButtonScrollRight.Bounds = new Rectangle(this.Width - 12, 0, 12, this.Height - 2);
                this.Controls.Add(m_ButtonScrollRight);
                m_ButtonScrollRight.BringToFront();
            }
            else
            {
                if (m_ButtonScrollRight != null)
                {
                    m_ButtonScrollRight.Visible = false;
                    this.Controls.Remove(m_ButtonScrollRight);
                    m_ButtonScrollRight.Dispose();
                    m_ButtonScrollRight = null;
                }
            }

            if (m_ScrollOffset < 0)
            {
                if (m_ButtonScrollLeft == null)
                {
                    m_ButtonScrollLeft = new ButtonX();
                    m_ButtonScrollLeft.Text = "<expand direction=\"left\"/>";
                    m_ButtonScrollLeft.ColorTable = eButtonColor.OrangeWithBackground;
                    m_ButtonScrollLeft.Click += new EventHandler(ScrollPanelLeftClick);
                }
                m_ButtonScrollLeft.Bounds = new Rectangle(0, 0, 12, this.Height - 2);
                this.Controls.Add(m_ButtonScrollLeft);
                m_ButtonScrollLeft.BringToFront();
            }
            else
            {
                if (m_ButtonScrollLeft != null)
                {
                    m_ButtonScrollLeft.Visible = false;
                    this.Controls.Remove(m_ButtonScrollLeft);
                    m_ButtonScrollLeft.Dispose();
                    m_ButtonScrollLeft = null;
                }
            }
        }

        private void ScrollPanelRightClick(object sender, EventArgs e)
        {
            ScrollRight();
        }

        /// <summary>
        /// Scrolls the RibbonBar controls to the right one step if there is more of the controls on the panel that can fit into the available space.
        /// </summary>
        public void ScrollRight()
        {
            if (m_ScrollOffset + m_ViewBounds.Width <= this.Width)
                return;

            SetScrollOffset(-Math.Min(this.Width, m_ViewBounds.Right - this.Width + 2));
        }

        private void ScrollPanelLeftClick(object sender, EventArgs e)
        {
            ScrollLeft();
        }

        /// <summary>
        /// Scrolls the RibbonBar controls one step to the left.
        /// </summary>
        public void ScrollLeft()
        {
            SetScrollOffset(Math.Min(Math.Abs(m_ScrollOffset), this.Width));
        }

        /// <summary>
        /// Resets the panel scroll position.
        /// </summary>
        public void ResetScrollPosition()
        {
            SetScrollOffset(-m_ScrollOffset);
        }

        private void SetScrollOffset(int offset)
        {
            m_ScrollOffset += offset;
            this.PerformLayout();
        }

        private class XPositionComparer : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(object x, object y) 
            {
                if (x is Control && y is Control)
                {
                    return ((Control)x).Left - ((Control)y).Left;
                }
                else
                    return( (new CaseInsensitiveComparer()).Compare(x, y));
            }
        }

        private class ResizeOrderComparer : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(object x, object y)
            {
                if (x is RibbonBar && y is RibbonBar)
                {
                    return ((RibbonBar)x).ResizeOrderIndex - ((RibbonBar)y).ResizeOrderIndex;
                }
                else
                    return ((new CaseInsensitiveComparer()).Compare(x, y));
            }
        }

		private Rectangle GetThemedRect(Rectangle r)
		{
			const int offset=6;
			r.Y-=offset;
			r.Height+=offset;
			return r;
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			bool baseCall=true;
			if(DrawThemedPane && BarFunctions.ThemedOS)
			{
				Rectangle r=GetThemedRect(this.ClientRectangle);
				eTabStripAlignment tabAlignment=eTabStripAlignment.Top;
				
				Rectangle rTemp=new Rectangle(0,0,r.Width,r.Height);
				if(tabAlignment==eTabStripAlignment.Right || tabAlignment==eTabStripAlignment.Left)
					rTemp=new Rectangle(0,0,rTemp.Height,rTemp.Width);
				if(m_ThemeCachedBitmap==null || m_ThemeCachedBitmap.Size!=rTemp.Size)
				{
					DisposeThemeCachedBitmap();
					Bitmap bmp=new Bitmap(rTemp.Width,rTemp.Height,e.Graphics);
					try
					{
						Graphics gTemp=Graphics.FromImage(bmp);
						try
						{
							using(SolidBrush brush=new SolidBrush(Color.Transparent))
								gTemp.FillRectangle(brush,0,0,bmp.Width,bmp.Height);
							this.ThemeTab.DrawBackground(gTemp,ThemeTabParts.Pane,ThemeTabStates.Normal,rTemp);
						}
						finally
						{
							gTemp.Dispose();
						}
					}
					finally
					{
						if(tabAlignment==eTabStripAlignment.Left)
							bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
						else if(tabAlignment==eTabStripAlignment.Right)
							bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
						else if(tabAlignment==eTabStripAlignment.Bottom)
							bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
						e.Graphics.DrawImageUnscaled(bmp,r.X,r.Y);							
						m_ThemeCachedBitmap=bmp;
					}
				}
				else
					e.Graphics.DrawImageUnscaled(m_ThemeCachedBitmap,r.X,r.Y);

				baseCall=false;
			}

			if(baseCall)
				base.OnPaint(e);

			if(this.DesignMode && this.Controls.Count==0 && this.Text=="")
			{
				Rectangle r=this.ClientRectangle;
				r.Inflate(-2,-2);
				StringFormat sf=BarFunctions.CreateStringFormat();
				sf.Alignment=StringAlignment.Center;
				sf.LineAlignment=StringAlignment.Center;
				sf.Trimming=StringTrimming.EllipsisCharacter;
				Font font=new Font(this.Font,FontStyle.Bold);
				e.Graphics.DrawString(INFO_TEXT,font,new SolidBrush(ControlPaint.Dark(this.Style.BackColor)),r,sf);
				font.Dispose();
				sf.Dispose();
			}
            if (this.Parent is RibbonControl) ((RibbonControl)this.Parent).RibbonStrip.InvalidateKeyTipsCanvas();
		}

        //protected override void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
        //{
        //    if (m_PopupMode && m_RibbonTabItem != null)
        //    {

        //    }

        //    base.PaintInnerContent(e, style, paintText);
        //}

        /// <summary>
        /// Gets or sets whether default control layout is used instead of Rendering layout for RibbonBar controls positioning. By default
        /// internal layout logic is used so proper resizing of Ribbons can be performed. You can disable internal layout by setting this property
        /// to true.
        /// Default value is false.
        /// </summary>
        [Browsable(true),DefaultValue(false),Category("Layout"),Description("Indicates whether default control layout is used instead of Rendering layout for RibbonBar controls positioning.")]
        public bool DefaultLayout
        {
            get { return m_DefaultLayout; }
            set
            {
                m_DefaultLayout = value;
                this.PerformLayout();
            }
        }

		/// <summary>
		/// Indicates whether style of the panel is managed by tab control automatically.
		/// Set this to true if you would like to control style of the panel.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Appearance"),Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
		public bool UseCustomStyle
		{
			get {return m_UseCustomStyle;}
			set {m_UseCustomStyle=value;}
		}

		/// <summary>
		/// Gets or sets TabItem that this panel is attached to.
		/// </summary>
		[Browsable(false),DefaultValue(null),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RibbonTabItem RibbonTabItem
		{
			get {return m_RibbonTabItem;}
			set	{m_RibbonTabItem=value;}
		}

        /// <summary>
        /// Gets or sets whether last RibbonBar is stretched to fill available space inside of the panel. Default value is false.
        /// </summary>
        [Browsable(true),DefaultValue(false), Category("Layout"), Description("Whether last RibbonBar is stretched to fill available space inside of the panel.")]
        public bool StretchLastRibbonBar
        {
            get { return m_StretchLastRibbonBar; }
            set
            {
                m_StretchLastRibbonBar = value;
                this.PerformLayout();
            }
        }

		protected override void OnResize(EventArgs e)
		{
            m_ScrollOffset = 0;
			DisposeThemeCachedBitmap();
			base.OnResize(e);
		}

		/// <summary>
		/// Gets or sets which edge of the parent container a control is docked to.
		/// </summary>
		[Browsable(false),DefaultValue(DockStyle.None)]
		public override DockStyle Dock
		{
			get {return base.Dock;}
			set {base.Dock=value;}
		}

		/// <summary>
		/// Gets or sets the size of the control.
		/// </summary>
		[Browsable(false)]
		public new Size Size
		{
			get {return base.Size;}
			set {base.Size=value;}
		}

		/// <summary>
		/// Gets or sets the coordinates of the upper-left corner of the control relative to the upper-left corner of its container.
		/// </summary>
		[Browsable(false)]
		public new Point Location
		{
			get {return base.Location;}
			set {base.Location=value;}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is displayed.
		/// </summary>
		[Browsable(false)]
		public new bool Visible
		{
			get {return base.Visible;}
			set {base.Visible=value;}
		}

		/// <summary>
		/// Gets or sets which edges of the control are anchored to the edges of its container.
		/// </summary>
		[Browsable(false)]
		public override AnchorStyles Anchor
		{
			get {return base.Anchor;}
			set {base.Anchor=value;}
		}

        [Browsable(false), DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        bool IKeyTipsControl.ProcessMnemonicEx(char charCode)
        {
            if (this.Controls.Count == 0) return false;

            Control[] ca = new Control[this.Controls.Count];
            this.Controls.CopyTo(ca, 0);
            ArrayList controls = new ArrayList(ca);
            controls.Sort(new XPositionComparer());
            foreach (Control c in controls)
            {
                IKeyTipsControl ktc = c as IKeyTipsControl;
                if (ktc!=null && c.Visible && c.Enabled)
                {
                    string oldStack = ktc.KeyTipsKeysStack;
                    bool ret = ktc.ProcessMnemonicEx(charCode);
                    if (ret)
                        return true;
                    if (ktc.KeyTipsKeysStack != oldStack)
                    {
                        ((IKeyTipsControl)this).KeyTipsKeysStack = ktc.KeyTipsKeysStack;
                        return false;
                    }
                }
            }
            return false;
        }

        private bool m_ShowKeyTips = false;
        bool IKeyTipsControl.ShowKeyTips
        {
            get
            {
                return m_ShowKeyTips;
            }
            set
            {
                m_ShowKeyTips = value;
                Control[] controls = new Control[this.Controls.Count];
                this.Controls.CopyTo(controls, 0);
                foreach (Control c in controls)
                {
                    if (c is IKeyTipsControl && c.Enabled && (c.Visible || !m_ShowKeyTips))
                        ((IKeyTipsControl)c).ShowKeyTips = m_ShowKeyTips;
                }
            }
        }

        private string m_KeyTipsKeysStack = "";
        string IKeyTipsControl.KeyTipsKeysStack
        {
            get { return m_KeyTipsKeysStack; }
            set
            {
                m_KeyTipsKeysStack = value;
                foreach (Control c in this.Controls)
                {
                    if (c is IKeyTipsControl && c.Visible && c.Enabled)
                        ((IKeyTipsControl)c).KeyTipsKeysStack = m_KeyTipsKeysStack;
                }
            }
        }
		#endregion
	}
}
