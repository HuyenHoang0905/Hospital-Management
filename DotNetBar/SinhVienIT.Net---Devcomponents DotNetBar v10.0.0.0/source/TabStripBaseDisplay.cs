using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents base class for tab display.
	/// </summary>
	internal class TabStripBaseDisplay
	{
		#region Private Variables
		private bool m_AntiAlias=true;
		private bool m_HorizontalText=false;
        private bool m_CloseButtonOnTabs = false;
        private Size m_TabCloseButtonSize = new Size(11, 11);
        private int m_TabCloseButtonSpacing = 3;
		#endregion

		public TabStripBaseDisplay()
		{
		}

		#region Methods
		/// <summary>
		/// Main method for painting.
		/// </summary>
		/// <param name="g">Reference to graphics object</param>
		/// <param name="tabStrip">TabStrip to paint</param>
		public virtual void Paint(Graphics g, TabStrip tabStrip)
		{
			if(this.AntiAlias)
			{
				g.SmoothingMode=SmoothingMode.AntiAlias;
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
		}

		protected virtual void PaintTab(Graphics g, TabItem tab, bool first, bool last)
		{
			if(!tab.Visible)
				return;

            if (tab.Parent!=null && tab.Parent.HasPreRenderTabItem)
            {
                RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                tab.Parent.InvokePreRenderTabItem(re);
                if (re.Cancel) return;
            }

            using (GraphicsPath path = GetTabItemPath(tab, first, last))
            {
                TabColors colors = tab.Parent.GetTabColors(tab);

                DrawTabItemBackground(tab, path, colors, g);

                DrawTabText(tab, colors, g);
            }

            if (tab.Parent != null && tab.Parent.HasPostRenderTabItem)
            {
                RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                tab.Parent.InvokePostRenderTabItem(re);
            }
		}

		protected virtual Region GetTabsRegion(TabsCollection tabs, TabItem lastTab)
		{
			return null;
		}

		protected virtual void DrawTabItemBackground(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
		{
			RectangleF rf=path.GetBounds();
			Rectangle tabRect=new Rectangle((int)rf.X, (int)rf.Y, (int)rf.Width, (int)rf.Height);

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;

            eBackgroundColorBlendType blendType = colors.BackgroundColorBlend.GetBlendType();
            int ga = colors.BackColorGradientAngle;
            if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
                ga -= 90;

            if (blendType != eBackgroundColorBlendType.Invalid)
            {
                if (blendType == eBackgroundColorBlendType.Relative)
                {
                    if (!colors.BackColor.IsEmpty || !colors.BackColor2.IsEmpty)
                    {
                        try
                        {
                            Rectangle rb = Rectangle.Ceiling(rf);
                            rb.Inflate(1, 1);
                            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, colors.BackColor, colors.BackColor2, ga))
                            {
                                brush.InterpolationColors = colors.BackgroundColorBlend.GetColorBlend();
                                g.FillPath(brush, path);
                            }
                        }
                        catch
                        {
                            blendType = eBackgroundColorBlendType.Invalid;
                        }
                    }
                }
                else
                {
                    Rectangle bounds = Rectangle.Ceiling(rf);
                    BackgroundColorBlendCollection bc = colors.BackgroundColorBlend;
                    Region oldClip = g.Clip;
                    g.SetClip(path, CombineMode.Intersect);
                    for (int i = 0; i < bc.Count; i += 2)
                    {
                        BackgroundColorBlend b1 = bc[i];
                        BackgroundColorBlend b2 = null;
                        if (i < bc.Count)
                            b2 = bc[i + 1];
                        if (b1 != null && b2 != null)
                        {
                            Rectangle rb = new Rectangle(bounds.X, bounds.Y + (int)b1.Position, bounds.Width,
                                (b2.Position == 1f ? bounds.Height : (int)b2.Position) - (int)b1.Position);
                            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, b1.Color, b2.Color, ga))
                                g.FillRectangle(brush, rb);
                        }
                    }
                    g.Clip = oldClip;
                }
            }

            if (blendType == eBackgroundColorBlendType.Invalid)
            {
                if (colors.BackColor2.IsEmpty)
                {
                    if (!colors.BackColor.IsEmpty)
                    {
                        using (SolidBrush brush = new SolidBrush(colors.BackColor))
                            g.FillPath(brush, path);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                        g.FillPath(brush, path);
                    using (LinearGradientBrush brush = CreateTabGradientBrush(tabRect, colors.BackColor, colors.BackColor2, ga))
                        g.FillPath(brush, path);
                }
            }

            g.SmoothingMode = sm;

            DrawTabBorder(tab, path, colors, g);
		}

        protected virtual void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
        {
            if (!colors.BorderColor.IsEmpty)
            {
                using (Pen pen = new Pen(colors.BorderColor, 1))
                    g.DrawPath(pen, path);
            }
        }

		protected virtual LinearGradientBrush CreateTabGradientBrush(Rectangle r,Color color1,Color color2,int gradientAngle)
		{
			if(r.Width<=0)
				r.Width=1;
			if(r.Height<=0)
				r.Height=1;
			LinearGradientBrush brush=new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle);
			return brush;
		}

        private Rectangle DrawCloseButton(Graphics g, bool vertical, bool mouseOver, ref Rectangle tabRect, TabStrip tabStrip)
        {
            return DrawCloseButton(g, vertical, mouseOver, ref tabRect, tabStrip, true);
        }
        private Rectangle DrawCloseButton(Graphics g, bool vertical, bool mouseOver, ref Rectangle tabRect, TabStrip tabStrip, bool performPaint)
        {
            Size closeSize = m_TabCloseButtonSize;
            int spacing = m_TabCloseButtonSpacing;
            bool closeOnLeftSide = (tabStrip.CloseButtonPosition == eTabCloseButtonPosition.Left);

            Rectangle close = Rectangle.Empty; //new Rectangle(tabRect.X + spacing, tabRect.Y + (tabRect.Height - closeSize.Height) / 2, closeSize.Width, closeSize.Height);

            if (closeOnLeftSide)
            {
                if (vertical)
                    close = new Rectangle(tabRect.X + (tabRect.Width - closeSize.Width) / 2, tabRect.Y + spacing, closeSize.Width, closeSize.Height);
                else
                    close = new Rectangle(tabRect.X + spacing, tabRect.Y + (tabRect.Height - closeSize.Height) / 2, closeSize.Width, closeSize.Height);
            }
            else
            {
                if (vertical)
                    close = new Rectangle(tabRect.X + (tabRect.Width - closeSize.Width) / 2, tabRect.Bottom - spacing - closeSize.Height, closeSize.Width, closeSize.Height);
                else
                    close = new Rectangle(tabRect.Right - spacing - closeSize.Width, tabRect.Y + (tabRect.Height - closeSize.Height) / 2, closeSize.Width, closeSize.Height);
            }
            AdjustCloseButtonRectangle(ref close, closeOnLeftSide, vertical);
            if (performPaint)
                TabStripBaseDisplay.PaintTabItemCloseButton(g, close, mouseOver, tabStrip);
            if (vertical)
            {
                if (closeOnLeftSide)
                    tabRect.Y += close.Height + spacing * 2;
                tabRect.Height -= close.Height + spacing * 2;
            }
            else
            {
                if (closeOnLeftSide)
                    tabRect.X += close.Width + spacing * 2;
                tabRect.Width -= close.Width + spacing * 2;
            }

            return close;
        }

        protected virtual void AdjustCloseButtonRectangle(ref Rectangle close, bool closeOnLeftSide, bool vertical) { }

        protected virtual void DrawTabText(TabItem tab, TabColors colors, Graphics g)
        {
            int MIN_TEXT_WIDTH = 4;
            eTextFormat strFormat = eTextFormat.Default | eTextFormat.SingleLine | eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
            eTabStripAlignment align = tab.Parent.TabAlignment;
            Rectangle rText = tab.DisplayRectangle;
            bool isVertical = ((align == eTabStripAlignment.Left || align == eTabStripAlignment.Right) && !m_HorizontalText);

            if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
            {
                if (isVertical)
                {
                    rText.Y++;
                    rText.Height--;
                }
                else
                {
                    rText.X += 2;
                    rText.Width -= 2;
                }
                bool renderCloseButton = tab.IsSelected || tab.IsMouseOver || tab.Parent.CloseButtonOnTabsAlwaysDisplayed;
                tab.CloseButtonBounds = DrawCloseButton(g, isVertical, tab.CloseButtonMouseOver, ref rText, tab.Parent, renderCloseButton);
            }

            // Draw image
            CompositeImage image = GetTabImage(tab);
            if (image != null && image.Width + 4 <= rText.Width)
            {
                if (align == eTabStripAlignment.Top || align == eTabStripAlignment.Bottom || m_HorizontalText)
                {
                    image.DrawImage(g, new Rectangle(rText.X + 3, rText.Y + (rText.Height - image.Height) / 2, image.Width, image.Height));
                    int offset = image.Width + 2;
                    rText.X += offset;
                    rText.Width -= offset;
                }
                else
                {
                    image.DrawImage(g, new Rectangle(rText.X + (rText.Width - image.Width) / 2, rText.Y + 3, image.Width, image.Height));
                    int offset = image.Height + 2;
                    rText.Y += offset;
                    rText.Height -= offset;
                }
            }
            if (image != null) image.Dispose();

            // Draw text
            bool isSelected = tab == tab.Parent.SelectedTab;
            if (!tab.Parent.DisplaySelectedTextOnly || isSelected)
            {
                if (!tab.Parent.AntiAlias)
                    g.TextRenderingHint = TextRenderingHint.SystemDefault;

                Font font = tab.Parent.Font;
                if (isSelected && tab.Parent.SelectedTabFont != null)
                    font = tab.Parent.SelectedTabFont;
                AdjustTextRectangle(ref rText, align);
                if (isVertical)
                {
                    g.RotateTransform(90);
                    rText = new Rectangle(rText.Top, -rText.Right, rText.Height, rText.Width);
                }

                if (rText.Width > MIN_TEXT_WIDTH)
                {
                    if ((align == eTabStripAlignment.Left || align == eTabStripAlignment.Right) && !m_HorizontalText)
                        TextDrawing.DrawStringLegacy(g, tab.Text, font, colors.TextColor, rText, strFormat);
                    else
                        TextDrawing.DrawString(g, tab.Text, font, colors.TextColor, rText, strFormat);
                }
                //using (Pen pen = new Pen(Color.Red))
                //    g.DrawRectangle(pen, rText);
                if (isVertical)
                    g.ResetTransform();
                if (!tab.Parent.AntiAlias)
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;

                if (tab.Parent.ShowFocusRectangle && tab.Parent.Focused && isSelected)
                    ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(tab.DisplayRectangle));
            }
        }

        protected virtual void AdjustTextRectangle(ref Rectangle rText, eTabStripAlignment tabAlignment) {}

		protected virtual CompositeImage GetTabImage(TabItem tab)
		{
			Image image=tab.GetImage();
			if(image!=null)
				return new CompositeImage(image,false);
			Icon icon=tab.Icon;
			if(icon!=null)
				return new CompositeImage(icon,false,tab.IconSize);

			return null;
		}

		protected virtual Rectangle GetFocusRectangle(Rectangle rText)
		{
			rText.Inflate(-1,-1);
			return rText;
		}

		protected virtual TabItem GetLastVisibleTab(TabsCollection tabs)
		{
			int c=tabs.Count-1;
			for(int i=c;i>=0;i--)
			{
				if(tabs[i].Visible)
					return tabs[i];
			}

			return null;
		}

        protected virtual void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
		{
			if(colors.TabBackground2.IsEmpty)
			{
                if (!colors.TabBackground.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(colors.TabBackground))
                        g.FillRegion(brush, tabsRegion);
                }
			}
			else
			{
				using(LinearGradientBrush brush=BarFunctions.CreateLinearGradientBrush(tabsRegion.GetBounds(g),colors.TabBackground,colors.TabBackground2,colors.TabBackgroundGradientAngle))
					g.FillRegion(brush, tabsRegion);
			}

		}
		#endregion

		#region Tab Path Functions
		protected virtual GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
		{
			return null;
		}

		protected virtual ArcData GetCornerArc(Rectangle bounds, int cornerDiameter, eCornerArc corner)
		{
			ArcData a;
			int diameter=cornerDiameter*2;
			switch(corner)
			{
				case eCornerArc.TopLeft:
					a=new ArcData(bounds.X,bounds.Y,diameter,diameter,180,90);
					break;
				case eCornerArc.TopRight:
					a=new ArcData(bounds.Right-diameter,bounds.Y,diameter,diameter,270,90);
					break;
				case eCornerArc.BottomLeft:
					a=new ArcData(bounds.X,bounds.Bottom-diameter,diameter,diameter,90,90);
					break;
				default: // eCornerArc.BottomRight:
					a=new ArcData(bounds.Right-diameter,bounds.Bottom-diameter,diameter,diameter,0,90);
					break;
			}
			return a; 
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets whether anti-alias is used for painting the tabs. Default value is true.
		/// </summary>
		public virtual bool AntiAlias
		{
			get {return m_AntiAlias;}
			set {m_AntiAlias=value;}
		}

		/// <summary>
		/// Gets or sets whether text is drawn horizontaly regardless of tab orientation.
		/// </summary>
		public bool HorizontalText
		{
			get {return m_HorizontalText;}
			set {m_HorizontalText=value;}
		}

        /// <summary>
        /// Gets or sets whether close button is painted on each tab.
        /// </summary>
        public bool CloseButtonOnTabs
        {
            get { return m_CloseButtonOnTabs; }
            set { m_CloseButtonOnTabs = value; }
        }

        public int TabCloseButtonSpacing
        {
            get { return m_TabCloseButtonSpacing; }
            set { m_TabCloseButtonSpacing=value; }
        }
		#endregion

        #region Static Methods
        public static void PaintTabItemCloseButton(Graphics g, Rectangle r, bool mouseOver, TabStrip tabStrip)
        {
            if (tabStrip.TabCloseButtonNormal != null)
            {
                Image image = tabStrip.TabCloseButtonNormal;
                if (mouseOver && tabStrip.TabCloseButtonHot != null)
                    image = tabStrip.TabCloseButtonHot;
                g.DrawImageUnscaled(image, r);
                return;
            }
            
            Color fillColor = tabStrip.ColorScheme.TabItemBorder;
            Color lineColor = tabStrip.ColorScheme.TabItemBorderLight;
            if (!fillColor.IsEmpty && Math.Abs(fillColor.GetBrightness() - lineColor.GetBrightness()) <= .38)
            {
                lineColor = BarFunctions.Ligten(lineColor, 60);
                fillColor = BarFunctions.Darken(fillColor, 40);
            }

            if (fillColor.IsEmpty)
            {
                if (!tabStrip.ColorScheme.TabItemSelectedBorder.IsEmpty)
                {
                    fillColor = tabStrip.ColorScheme.TabItemSelectedBorder;
                    if(!tabStrip.ColorScheme.TabItemSelectedBorderLight.IsEmpty)
                        lineColor = tabStrip.ColorScheme.TabItemSelectedBorderLight;
                }
                else
                {
                    fillColor = SystemColors.ControlDark;
                    lineColor = SystemColors.ControlLightLight;
                }
            }

            if(lineColor.IsEmpty)
                lineColor = SystemColors.ControlLightLight;

            //if (!fillColor.IsEmpty && Math.Abs(fillColor.GetBrightness() - lineColor.GetBrightness()) <= .38)
            //{
            //    if (lineColor.GetBrightness() < .5)
            //    {
            //        fillColor = lineColor;
            //        lineColor = SystemColors.ControlLightLight;
            //    }
            //    else
            //        fillColor = (lineColor.GetBrightness() < .5 ? SystemColors.ControlLightLight : SystemColors.ControlDarkDark);
            //}

            fillColor = Color.FromArgb(mouseOver ? 255 : 200, fillColor);

            using(Pen pen=new Pen(Color.White,1))
                g.DrawEllipse(pen, r);
            using (Pen pen = new Pen(fillColor, 1))
            {
                g.DrawEllipse(pen, r);
            }
            using (SolidBrush brush = new SolidBrush(Color.White))
                g.FillEllipse(brush, r);
            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                g.FillEllipse(brush, r);
            }

            using (Pen pen = new Pen(lineColor, 1))
            {
                Rectangle close = r;
                close.Inflate(-3, -3);
                close.Width--;
                g.DrawLine(pen, close.X, close.Y, close.Right, close.Bottom);
                g.DrawLine(pen, close.X, close.Bottom, close.Right, close.Y);
                close.Offset(1, 0);
                g.DrawLine(pen, close.X, close.Y, close.Right, close.Bottom);
                g.DrawLine(pen, close.X, close.Bottom, close.Right, close.Y);
            }
        }
        #endregion
    }
}
