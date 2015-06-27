using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides display capabilities for TabStrip with Office 2007 Dock style.
    /// </summary>
    internal class TabStripOffice2007DockDisplay : TabStripBaseDisplay
    {
        #region Private Variables
        private int m_CornerSize = 1;
        #endregion

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

            using (Region tabRegion = new Region(tabStrip.DisplayRectangle))
                DrawBackground(tabStrip, clientRect, g, colorScheme, tabRegion, tabStrip.TabAlignment, selectedRect);

            Rectangle systemBoxRect = tabStrip.GetSystemBoxRectangle();

            for (int i = tabStrip.Tabs.Count - 1; i >= 0; i--)
            {
                TabItem tab = tabStrip.Tabs[i];

                if (!tab.Visible || tab == tabStrip.SelectedTab)
                    continue;
                if (!tab.DisplayRectangle.IntersectsWith(systemBoxRect))
                {   if(tab.DisplayRectangle.IntersectsWith(clientRect))
                        PaintTab(g, tab, false, false);
                }
                else
                {
                    Region oldClip = g.Clip;
                    g.SetClip(systemBoxRect, CombineMode.Exclude);
                    PaintTab(g, tab, false, false);
                    g.Clip = oldClip;
                }
            }

            if (tabStrip.SelectedTab != null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
            {
                if (!tabStrip.SelectedTab.DisplayRectangle.IntersectsWith(systemBoxRect))
                    PaintTab(g, tabStrip.SelectedTab, false, false);
                else
                {
                    Region oldClip = g.Clip;
                    g.SetClip(systemBoxRect, CombineMode.Exclude);
                    PaintTab(g, tabStrip.SelectedTab, false, false);
                    g.Clip = oldClip;
                }
            }

            g.ResetClip();
            tabStrip.PaintTabSystemBox(g);
        }

        protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
        {
            return GetTabItemPath(tab.DisplayRectangle, tab.TabAlignment, true);
        }

        private GraphicsPath GetTabItemPath(Rectangle tabRect, eTabStripAlignment align, bool closePath)
        {
            GraphicsPath path = new GraphicsPath();
            switch (align)
            {
                case eTabStripAlignment.Top:
                    {
                        //tabRect.Width--;
                        if (closePath) tabRect.Height+=2;
                        path.AddLine(tabRect.X, tabRect.Bottom, tabRect.X, tabRect.Y + m_CornerSize);
                        path.AddLine(tabRect.X, tabRect.Y + m_CornerSize, tabRect.X+m_CornerSize, tabRect.Y);
                        path.AddLine(tabRect.X + m_CornerSize, tabRect.Y, tabRect.Right-m_CornerSize, tabRect.Y);
                        path.AddLine(tabRect.Right - m_CornerSize, tabRect.Y, tabRect.Right, tabRect.Y+m_CornerSize);
                        path.AddLine(tabRect.Right, tabRect.Y + m_CornerSize, tabRect.Right, tabRect.Bottom);
                        break;
                    }
                case eTabStripAlignment.Bottom:
                    {
                        //tabRect.Width--;
                        if (!closePath)
                            tabRect.Y++;
                        tabRect.Height--;
                        path.AddLine(tabRect.X, tabRect.Y, tabRect.X, tabRect.Bottom - m_CornerSize);
                        path.AddLine(tabRect.X, tabRect.Bottom - m_CornerSize, tabRect.X + m_CornerSize, tabRect.Bottom);
                        path.AddLine(tabRect.X + m_CornerSize, tabRect.Bottom, tabRect.Right - m_CornerSize, tabRect.Bottom);
                        path.AddLine(tabRect.Right - m_CornerSize, tabRect.Bottom, tabRect.Right, tabRect.Bottom - m_CornerSize);
                        path.AddLine(tabRect.Right, tabRect.Bottom - m_CornerSize, tabRect.Right, tabRect.Y);
                        break;
                    }
                case eTabStripAlignment.Left:
                    {
                        tabRect.X--;
                        tabRect.Height--;
                        if (closePath)
                            tabRect.Width++;
                        path.AddLine(tabRect.Right, tabRect.Y, tabRect.X + m_CornerSize, tabRect.Y);
                        path.AddLine(tabRect.X + m_CornerSize, tabRect.Y, tabRect.X, tabRect.Y+m_CornerSize);
                        path.AddLine(tabRect.X, tabRect.Y + m_CornerSize, tabRect.X, tabRect.Bottom - m_CornerSize);
                        path.AddLine(tabRect.X, tabRect.Bottom - m_CornerSize, tabRect.X+m_CornerSize, tabRect.Bottom);
                        path.AddLine(tabRect.X + m_CornerSize, tabRect.Bottom, tabRect.Right, tabRect.Bottom);

                        break;
                    }
                case eTabStripAlignment.Right:
                    {
                        if (!closePath)
                            tabRect.X++;
                        //if (closePath)
                        //    tabRect.Width--;
                        path.AddLine(tabRect.X, tabRect.Y, tabRect.Right - m_CornerSize, tabRect.Y);
                        path.AddLine(tabRect.Right - m_CornerSize, tabRect.Y, tabRect.Right, tabRect.Y + m_CornerSize);
                        path.AddLine(tabRect.Right, tabRect.Y + m_CornerSize, tabRect.Right, tabRect.Bottom - m_CornerSize);
                        path.AddLine(tabRect.Right, tabRect.Bottom - m_CornerSize, tabRect.Right - m_CornerSize, tabRect.Bottom);
                        path.AddLine(tabRect.Right - m_CornerSize, tabRect.Bottom, tabRect.X, tabRect.Bottom);

                        break;
                    }
            }

            if (closePath)
                path.CloseAllFigures();

            return path;
        }

        protected override void DrawTabBorder(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
        {
            Rectangle r = tab.DisplayRectangle;
            eTabStripAlignment align = tab.TabAlignment;
            //Region oldClip = g.Clip;
            //Rectangle rClip = Rectangle.Ceiling(path.GetBounds());
            //if (tab.TabAlignment == eTabStripAlignment.Right || tab.TabAlignment == eTabStripAlignment.Bottom)
            //    rClip.Inflate(1, 1);

            //g.SetClip(rClip);

            if (!colors.BorderColor.IsEmpty)
            {
                using (GraphicsPath borderPath = GetTabItemPath(r, align, false))
                {
                    using (Pen pen = new Pen(colors.BorderColor))
                        g.DrawPath(pen, borderPath);
                }
            }

            if (align == eTabStripAlignment.Top)
            {
                r.Offset(1, 1);
                r.Width-=2;
                r.Height--;
            }
            else if (align == eTabStripAlignment.Bottom)
            {
                r.Offset(1, 0);
                r.Height--;
                r.Width-=2;
            }
            else if (align == eTabStripAlignment.Left)
            {
                r.Offset(1, 1);
                r.Height-=2;
                r.Width-=2;
            }
            else if (align == eTabStripAlignment.Right)
            {
                r.Offset(1, 1);
                r.Height -= 2;
                r.Width -= 2;
            }

            if (!colors.LightBorderColor.IsEmpty)
            {
                using (GraphicsPath borderPath = GetTabItemPath(r, align, false))
                {
                    //rClip = Rectangle.Ceiling(path.GetBounds());
                    //g.SetClip(rClip);

                    using (Pen pen = new Pen(colors.LightBorderColor))
                        g.DrawPath(pen, borderPath);
                }
            }

            //g.Clip = oldClip;
        }

        protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
        {
            base.DrawBackground(tabStrip, tabStripRect, g, colors, tabsRegion, tabAlignment, selectedTabRect);

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            if (!colors.TabItemBorder.IsEmpty)
            {
                using (Pen pen = new Pen(colors.TabItemBorder, 1))
                {
                    if (tabAlignment == eTabStripAlignment.Top)
                    {
                        if (selectedTabRect.X > tabStripRect.Right)
                            selectedTabRect = Rectangle.Empty;
                        Rectangle r = new Rectangle(tabStripRect.X, tabStripRect.Bottom - 1, tabStripRect.Width, 2);
                        DisplayHelp.FillRectangle(g, r, colors.TabItemBackground, Color.Empty);
                        g.DrawLine(pen, r.X, r.Y, selectedTabRect.X - 1, r.Y);
                        g.DrawLine(pen, selectedTabRect.Right, r.Y, r.Right, r.Y);
                        g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                        g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
                    }
                    else if (tabAlignment == eTabStripAlignment.Bottom)
                    {
                        if (selectedTabRect.X > tabStripRect.Right)
                            selectedTabRect = Rectangle.Empty;
                        Rectangle r = new Rectangle(tabStripRect.X, tabStripRect.Y, tabStripRect.Width, 1);
                        DisplayHelp.FillRectangle(g, r, colors.TabItemBackground, Color.Empty);
                        g.DrawLine(pen, r.X, r.Bottom - 1, selectedTabRect.X - 1, r.Bottom - 1);
                        g.DrawLine(pen, selectedTabRect.Right, r.Bottom - 1, r.Right, r.Bottom - 1);
                        g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom - 1);
                        g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);
                    }
                    else if (tabAlignment == eTabStripAlignment.Left)
                    {
                        if (selectedTabRect.Y > tabStripRect.Bottom)
                            selectedTabRect = Rectangle.Empty;
                        Rectangle r = new Rectangle(tabStripRect.Right - 1, tabStripRect.Y, 1, tabStripRect.Height);
                        DisplayHelp.FillRectangle(g, r, colors.TabItemBackground, Color.Empty);
                        g.DrawLine(pen, r.X, r.Y, r.X, selectedTabRect.Y - 1);
                        g.DrawLine(pen, r.X, selectedTabRect.Bottom, r.X, r.Bottom);
                        g.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
                        g.DrawLine(pen, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
                    }
                    else if (tabAlignment == eTabStripAlignment.Right)
                    {
                        if (selectedTabRect.Y > tabStripRect.Bottom)
                            selectedTabRect = Rectangle.Empty;
                        Rectangle r = new Rectangle(tabStripRect.X, tabStripRect.Y, 1, tabStripRect.Height);
                        DisplayHelp.FillRectangle(g, r, colors.TabItemBackground, Color.Empty);
                        g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, selectedTabRect.Y - 1);
                        g.DrawLine(pen, r.Right - 1, selectedTabRect.Bottom, r.Right - 1, r.Bottom);
                        g.DrawLine(pen, r.X, r.Y, r.Right - 1, r.Y);
                        g.DrawLine(pen, r.X, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
                    }
                }
            }
            g.SmoothingMode = sm;
        }
        #endregion
    }
}
