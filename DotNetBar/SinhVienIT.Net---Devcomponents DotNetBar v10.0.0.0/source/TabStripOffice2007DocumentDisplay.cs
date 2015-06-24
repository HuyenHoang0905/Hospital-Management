using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides display capabilities for TabStrip with Office 2007 Document style.
    /// </summary>
    internal class TabStripOffice2007DocumentDisplay : TabStripBaseDisplay
    {
        #region Private Variable
        private int m_TopLeftCornerSize = 3;
        #endregion
        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public TabStripOffice2007DocumentDisplay() { }

        #region Methods
        public override void Paint(Graphics g, TabStrip tabStrip)
        {
            base.Paint(g, tabStrip);

            TabColorScheme colorScheme = tabStrip.ColorScheme;
            Rectangle clientRect = tabStrip.DisplayRectangle;

            if (colorScheme.TabBackgroundImage != null)
                BarFunctions.PaintBackgroundImage(g, clientRect, colorScheme.TabBackgroundImage, eBackgroundImagePosition.Tile, 255);

            if (colorScheme.TabBackground != Color.Transparent && !colorScheme.TabBackground.IsEmpty)
            {
                if (colorScheme.TabBackground2.IsEmpty)
                {
                    if (!colorScheme.TabBackground.IsEmpty)
                    {
                        using (SolidBrush brush = new SolidBrush(colorScheme.TabBackground))
                            g.FillRectangle(brush, clientRect);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                        g.FillRectangle(brush, clientRect);
                    using (LinearGradientBrush brush = CreateTabGradientBrush(clientRect, colorScheme.TabBackground, colorScheme.TabBackground2, colorScheme.TabBackgroundGradientAngle))
                        g.FillRectangle(brush, clientRect);
                }
            }

            Rectangle selectedRect = Rectangle.Empty;
            if (tabStrip.SelectedTab != null)
                selectedRect = tabStrip.SelectedTab.DisplayRectangle;

            using(Region tabRegion = new Region(tabStrip.DisplayRectangle))
                DrawBackground(tabStrip, clientRect, g, colorScheme, tabRegion, tabStrip.TabAlignment, selectedRect);

            Rectangle systemBoxRect = tabStrip.GetSystemBoxRectangle();
            systemBoxRect.Inflate(-2, -2);
            for (int i = tabStrip.Tabs.Count - 1; i >= 0; i--)
            {
                TabItem tab = tabStrip.Tabs[i];

                if (!tab.Visible || tab == tabStrip.SelectedTab)
                    continue;
                if(!tab.DisplayRectangle.IntersectsWith(systemBoxRect) && tab.DisplayRectangle.IntersectsWith(clientRect))
                    PaintTab(g, tab, false, false);
            }

            if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
            {
                if (!GetAdjustedRect(tabStrip.SelectedTab).IntersectsWith(systemBoxRect))
                    PaintTab(g, tabStrip.SelectedTab, false, false);
            }

            g.ResetClip();
            tabStrip.PaintTabSystemBox(g);
        }

        private Rectangle GetAdjustedRect(TabItem item)
        {
            Rectangle r = item.DisplayRectangle;
            if (item.TabAlignment == eTabStripAlignment.Top || item.TabAlignment == eTabStripAlignment.Bottom)
                r.Width -= (r.Height - 6);
            else
                r.Height -= (r.Width - 6);
            return r;
        }

        protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
        {
            return GetTabPath(tab.DisplayRectangle, tab.TabAlignment, true);
        }

        private GraphicsPath GetTabPath(Rectangle tabDisplayRectangle, eTabStripAlignment align, bool closePath)
        {
            Rectangle r = tabDisplayRectangle;

            if (align == eTabStripAlignment.Right)
                r = new Rectangle(r.X, r.Y, r.Height, r.Width);
            else if (align == eTabStripAlignment.Left)
                r = new Rectangle(r.X, r.Y, r.Height, r.Width);

            if (align == eTabStripAlignment.Bottom || align == eTabStripAlignment.Top)
                r.Offset(0, 1);
            else
                r.Offset(1, 0);

            GraphicsPath path = new GraphicsPath();

            if (align == eTabStripAlignment.Bottom || align == eTabStripAlignment.Left)
            {
                path.AddLine(r.X, r.Y, r.X, r.Bottom - m_TopLeftCornerSize);
                path.AddLine(r.X, r.Bottom - m_TopLeftCornerSize, r.X+m_TopLeftCornerSize, r.Bottom);
                path.AddLine(r.X + m_TopLeftCornerSize, r.Bottom, r.Right - (r.Height/2), r.Bottom);

                Point[] p = new Point[4];
                p[0].X = r.Right - (r.Height / 2);
                p[0].Y = r.Bottom;
                p[1].X = p[0].X + 5;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + r.Height / 2;
                p[2].Y = r.Y + 3;
                p[3].X = p[2].X + 4;
                p[3].Y = r.Y;
                path.AddCurve(p, 0, 3, .1f);

                if (closePath)
                    path.AddLine(r.Right, r.Y, r.X, r.Y);
            }
            else
            {
                path.AddLine(r.X, r.Bottom, r.X, r.Y + m_TopLeftCornerSize);
                path.AddLine(r.X, r.Y + m_TopLeftCornerSize, r.X + m_TopLeftCornerSize, r.Y);
                path.AddLine(r.X + m_TopLeftCornerSize, r.Y, r.Right - (r.Height / 2), r.Y);

                Point[] p = new Point[4];
                p[0].X = r.Right - (r.Height / 2);
                p[0].Y = r.Y;
                p[1].X = p[0].X + 5;
                p[1].Y = p[0].Y + 3;
                p[2].X = p[1].X + r.Height / 2;
                p[2].Y = r.Bottom - 3;
                p[3].X = p[2].X + 4;
                p[3].Y = r.Bottom;
                path.AddCurve(p, 0, 3, .1f);

                if (closePath)
                    path.AddLine(r.Right, r.Bottom, r.X, r.Bottom);
            }

            if (align == eTabStripAlignment.Left)
            {
                // Left
                Matrix m = new Matrix();
                m.RotateAt(90, new PointF(r.Right, r.Bottom));
                m.Translate(-r.Width-2, r.Width - (r.Height), MatrixOrder.Append);
                //m.RotateAt(-90, new PointF(r.X, r.Bottom));
                //m.Translate(r.Height, r.Width - r.Height, MatrixOrder.Append);
                path.Transform(m);
            }
            else if (align == eTabStripAlignment.Right)
            {
                // Right
                Matrix m = new Matrix();
                //RectangleF rf=path.GetBounds();
                m.RotateAt(90, new PointF(r.Right, r.Bottom));
                m.Translate(-r.Width, r.Width - (r.Height), MatrixOrder.Append);
                path.Transform(m);
            }

            if (closePath)
                path.CloseAllFigures();

            return path;
        }

        protected override void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
        {
            Rectangle r = tab.DisplayRectangle;
            eTabStripAlignment align = tab.TabAlignment;
            Region oldClip = g.Clip;
            Rectangle rClip = Rectangle.Ceiling(path.GetBounds());
            if (tab.TabAlignment == eTabStripAlignment.Right || tab.TabAlignment == eTabStripAlignment.Bottom)
                rClip.Inflate(1, 1);
            else if (tab.TabAlignment == eTabStripAlignment.Left && !tab.IsSelected)
                rClip.Width--;
            
            g.SetClip(rClip);

            if (!colors.BorderColor.IsEmpty)
            {
                using (GraphicsPath borderPath = GetTabPath(r, align, false))
                {
                    using (Pen pen = new Pen(colors.BorderColor))
                        g.DrawPath(pen, borderPath);
                }
            }

            if (align == eTabStripAlignment.Top)
            {
                r.Offset(1, 1);
                r.Width--;
            }
            else if (align == eTabStripAlignment.Bottom)
            {
                r.Offset(1, -1);
                r.Width--;
            }
            else if (align == eTabStripAlignment.Left)
            {
                r.Offset(1, 1);
                r.Height--;
            }
            else if (align == eTabStripAlignment.Right)
            {
                r.Offset(-1, 1);
            }

            if (!colors.LightBorderColor.IsEmpty)
            {
                using (GraphicsPath borderPath = GetTabPath(r, align, false))
                {
                    rClip = Rectangle.Ceiling(path.GetBounds());
                    if (align == eTabStripAlignment.Left)
                        rClip.Width--;
                    g.SetClip(rClip);

                    using (Pen pen = new Pen(colors.LightBorderColor))
                        g.DrawPath(pen, borderPath);
                }
            }

            g.Clip = oldClip;
        }

        protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
        {
            base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            using (Pen pen = new Pen(colors.TabItemBorder, 1))
            {
                int tabExtension = 9;
                if (selectedTabRect.IsEmpty)
                    tabExtension = 0;
                Color tabItemSelectedColor2 = colors.TabItemSelectedBackground2;
                if (tabStrip.SelectedTab != null)
                {
                    TabColors tc = tabStrip.GetTabColors(tabStrip.SelectedTab);
                    tabItemSelectedColor2 = tc.BackColor2;
                }
                if (tabAlignment == eTabStripAlignment.Top)
                {
                    if (selectedTabRect.X > tabStripRect.Right || selectedTabRect.Bottom < tabStripRect.Bottom - 4)
                    {
                        selectedTabRect = Rectangle.Empty;
                        tabExtension = 0;
                    }
                    Rectangle r = new Rectangle(tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Width, 1);
                    DisplayHelp.FillRectangle(g, r, tabItemSelectedColor2, Color.Empty);
                    g.DrawLine(pen, r.X, r.Y, selectedTabRect.X, r.Y);
                    g.DrawLine(pen, selectedTabRect.Right + tabExtension, r.Y, r.Right, r.Y);
                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                    g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
                }
                else if (tabAlignment == eTabStripAlignment.Bottom)
                {
                    if (selectedTabRect.X > tabStripRect.Right || selectedTabRect.Y>4)
                    {
                        selectedTabRect = Rectangle.Empty;
                        tabExtension = 0;
                    }
                    Rectangle r = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width, 1);
                    DisplayHelp.FillRectangle(g, r, tabItemSelectedColor2, Color.Empty);
                    g.DrawLine(pen, r.X, r.Bottom -1, selectedTabRect.X, r.Bottom - 1);
                    g.DrawLine(pen, selectedTabRect.Right + tabExtension, r.Bottom - 1, r.Right, r.Bottom - 1);
                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom -1);
                    g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);
                }
                else if (tabAlignment == eTabStripAlignment.Left)
                {
                    if (selectedTabRect.Y > tabStripRect.Bottom || selectedTabRect.Right < tabStripRect.Right - 4)
                    {
                        selectedTabRect = Rectangle.Empty;
                        tabExtension = 0;
                    }
                    Rectangle r = new Rectangle(tabStripRect.Right -1, tabStripRect.Y, 1, tabStripRect.Height);
                    DisplayHelp.FillRectangle(g, r, tabItemSelectedColor2, Color.Empty);
                    g.DrawLine(pen, r.X, r.Y, r.X, selectedTabRect.Y);
                    g.DrawLine(pen, r.X, selectedTabRect.Bottom + tabExtension, r.X, r.Bottom);
                    g.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
                    g.DrawLine(pen, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
                }
                else if (tabAlignment == eTabStripAlignment.Right)
                {
                    if (selectedTabRect.Y > tabStripRect.Bottom || selectedTabRect.X>4)
                    {
                        selectedTabRect = Rectangle.Empty;
                        tabExtension = 0;
                    }
                    Rectangle r = new Rectangle(tabStripRect.X, tabStripRect.Y, 1, tabStripRect.Height);
                    DisplayHelp.FillRectangle(g, r, tabItemSelectedColor2, Color.Empty);
                    g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, selectedTabRect.Y);
                    g.DrawLine(pen, r.Right - 1, selectedTabRect.Bottom + tabExtension, r.Right - 1, r.Bottom);
                    g.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
                    g.DrawLine(pen, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
                }
            }
            g.SmoothingMode = sm;
        }

        protected override void AdjustTextRectangle(ref Rectangle rText, eTabStripAlignment tabAlignment)
        {
            if (tabAlignment == eTabStripAlignment.Top || tabAlignment == eTabStripAlignment.Bottom)
                rText.Width -= 3;
            base.AdjustTextRectangle(ref rText, tabAlignment);
        }

        protected override void AdjustCloseButtonRectangle(ref Rectangle close, bool closeOnLeftSide, bool vertical)
        {
            if (!closeOnLeftSide)
            {
                if (!vertical)
                    close.X -= TabCloseButtonSpacing + close.Height / 6;
                else
                    close.Y -= TabCloseButtonSpacing + close.Height / 6;

            }
        }
        #endregion


    }
}
