using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// RibbonTabItem painter for Office 2007 style
    /// </summary>
    internal class Office2007RibbonTabItemPainter : Office2007ButtonItemPainter
    {
        protected override Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, CompositeImage image)
        {
            Rectangle r = base.GetTextRectangle(button, pa, stringFormat, image);
            if(image==null)
                r.Inflate(-3, 0);
            return r;
        }

        public override eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
        {
            eTextFormat sf = base.GetStringFormat(button, pa, image);
            sf &= ~(sf & eTextFormat.EndEllipsis);
            return sf;
        }

        protected override void PaintState(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r, bool isMouseDown)
        {
            if (r.IsEmpty || !IsItemEnabled(button, pa) || r.Width==0 || r.Height==0)
                return;

            RibbonTabItem tab = button as RibbonTabItem;
            if (tab == null || IsOnMenu(button, pa))
            {
                base.PaintState(button, pa, image, r, isMouseDown);
                return;
            }

            bool isOnMenu = pa.IsOnMenu;
            Office2007RibbonTabItemColorTable tabColorTable = GetColorTable(tab);
            if (tabColorTable == null)
                return;
            bool ribbonExpanded = pa.ControlExpanded;

            Office2007RibbonTabItemStateColorTable stateColors = GetStateColorTable(tabColorTable, tab, ribbonExpanded);

            if (stateColors == null)
                return;

            Graphics g = pa.Graphics;
            int cornerSize = tabColorTable.CornerSize;
            Region oldClip = g.Clip;
            try
            {
                Rectangle rClip = r;
                rClip.Inflate(1, 0);
                g.SetClip(rClip, CombineMode.Replace);

                if (stateColors != null)
                {
                    using (GraphicsPath path = GetTabPath(r, cornerSize, true, button.EffectiveStyle))
                    {
                        DisplayHelp.FillPath(g, path, stateColors.Background);
                        DisplayHelp.DrawGradientPathBorder(g, path, stateColors.OuterBorder, 1);
                    }
                    if (tab.Checked && ribbonExpanded && tab.RenderTabState /*|| tab.IsMouseOver*/)
                    {
                        SmoothingMode sm = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.Default;

                        if (this.ColorTable.RibbonControl.TabsBackground.Start.GetBrightness() > .5)
                        {
                            using (GraphicsPath path = new GraphicsPath())
                            {
                                path.AddRectangle(new Rectangle(r.Right - 1, r.Y + cornerSize + 1, 1, r.Height - cornerSize - 3));
                                DisplayHelp.FillPath(g, path, Color.FromArgb(96, stateColors.OuterBorder.Start), Color.FromArgb(32, stateColors.OuterBorder.End), 90);
                            }
                            using (GraphicsPath path = new GraphicsPath())
                            {
                                path.AddRectangle(new Rectangle(r.X + 1, r.Y + cornerSize + 1, 1, r.Height - cornerSize - 3));
                                DisplayHelp.FillPath(g, path, Color.FromArgb(32, stateColors.OuterBorder.Start), Color.FromArgb(8, stateColors.OuterBorder.End), 90);
                            }
                        }
                        g.SmoothingMode = sm;
                    }

                    Rectangle r1 = r;
                    r1.Inflate(-1, 0);
                    r1.Height--;
                    r1.Y++;
                    using (GraphicsPath path = GetTabPath(r1, cornerSize, true, button.EffectiveStyle))
                    {
                        DisplayHelp.DrawGradientPathBorder(g, path, stateColors.InnerBorder, 1);
                    }

                    if (tab.Checked && ribbonExpanded && tab.RenderTabState)
                    {
                        using (SolidBrush brush = new SolidBrush(stateColors.InnerBorder.Start))
                        {
                            SmoothingMode sm = g.SmoothingMode;
                            g.SmoothingMode = SmoothingMode.None;
                            g.FillRectangle(brush, new Rectangle(r1.X + cornerSize, r1.Y + 1, r1.Width - cornerSize * 2, 2));
                            g.SmoothingMode = sm;
                        }
                    }

                    float topSplit = .6f;
                    float bottomSplit = .4f;

                    Rectangle fillRectangle = r;
                    Rectangle backRect = new Rectangle(fillRectangle.X, fillRectangle.Y + (int)(fillRectangle.Height * topSplit), fillRectangle.Width, (int)(fillRectangle.Height * bottomSplit));

                    if (!stateColors.BackgroundHighlight.IsEmpty)
                    {
                        Rectangle ellipse = new Rectangle(backRect.X, backRect.Y, fillRectangle.Width, fillRectangle.Height);
                        GraphicsPath path = new GraphicsPath();
                        path.AddEllipse(ellipse);
                        PathGradientBrush brush = new PathGradientBrush(path);
                        brush.CenterColor = stateColors.BackgroundHighlight.Start;
                        brush.SurroundColors = new Color[] { stateColors.BackgroundHighlight.End };
                        brush.CenterPoint = new PointF(ellipse.X + ellipse.Width / 2, fillRectangle.Bottom + 2);
                        Blend blend = new Blend();
                        blend.Factors = new float[] { 0f, .8f, 1f };
                        blend.Positions = new float[] { .0f, .55f, 1f };
                        brush.Blend = blend;
                        g.FillRectangle(brush, backRect);
                        brush.Dispose();
                        path.Dispose();
                    }
                }

                if (tab.ReducedSize && !tab.Checked && !tab.IsMouseOver && tabColorTable != null && tabColorTable.Selected != null)
                {
                    Color c = this.ColorTable.RibbonControl.OuterBorder.Start;
                    if (!c.IsEmpty)
                        DisplayHelp.DrawGradientLine(g, new Point(r.Right - 1, r.Y), new Point(r.Right - 1, r.Bottom - 1),
                            Color.Transparent, c, 90, 1, new float[] { 0, .8f, 1f }, new float[] { 0, .50f, 1f });
                }

                g.Clip = oldClip;
            }
            finally
            {
                if (oldClip != null) oldClip.Dispose();
            }
        }

        private GraphicsPath GetTabPath(Rectangle r, int cornerSize, bool getBottomPart, eDotNetBarStyle style)
        {
            GraphicsPath path = new GraphicsPath();
            int bottomCorner = (style == eDotNetBarStyle.Windows7 || style == eDotNetBarStyle.Office2010) ? 1 : 2;
            if (getBottomPart)
                path.AddLine(r.X, r.Bottom, r.X + bottomCorner, r.Bottom - bottomCorner);
            else
                path.AddLine(r.X, r.Bottom, r.X, r.Y + cornerSize);
            r.Inflate(-bottomCorner, 0);

            ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.TopLeft);
            ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.TopRight);

            if (getBottomPart)
                path.AddLine(r.Right, r.Bottom - bottomCorner, r.Right + bottomCorner, r.Bottom);
            else
                path.AddLine(r.Right, r.Y + cornerSize, r.Right, r.Bottom);

            return path;
        }

        protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
        {
            if (!IsItemEnabled(button, pa) || !(button is RibbonTabItem))
                return base.GetTextColor(button, pa);

            RibbonTabItem tab = button as RibbonTabItem;
            
            Color textColor = Color.Empty;

            Office2007RibbonTabItemStateColorTable ct = GetStateColorTable(GetColorTable(tab), tab, pa.ControlExpanded);

            if (ct != null)
            {
                textColor = ct.Text;
            }

            if (textColor.IsEmpty)
                return base.GetTextColor(button, pa);

            return textColor;
        }

        private Office2007RibbonTabItemColorTable GetColorTable(RibbonTabItem tab)
        {
            return GetColorTable(this.ColorTable, tab);
        }

        internal static Office2007RibbonTabItemColorTable GetColorTable(Office2007ColorTable table, RibbonTabItem tab)
        {
            if (table == null)
                return null;

            Office2007RibbonTabItemColorTable rt = table.RibbonTabItemColors[tab.GetColorTableName()];
            if (rt == null && table.RibbonTabItemColors.Count > 0)
                rt = table.RibbonTabItemColors[0];

            return rt;
        }

        internal static Office2007RibbonTabItemColorTable GetColorTable(Office2007ColorTable table)
        {
            if (table == null)
                return null;

            Office2007RibbonTabItemColorTable rt = table.RibbonTabItemColors["Default"];
            if (rt == null && table.RibbonTabItemColors.Count > 0)
                rt = table.RibbonTabItemColors[0];

            return rt;
        }

        internal static Office2007RibbonTabItemStateColorTable GetStateColorTable(Office2007RibbonTabItemColorTable tabColorTable, RibbonTabItem tab, bool ribbonExpanded)
        {
            if (tabColorTable == null)
                return null;

            Office2007RibbonTabItemStateColorTable stateColors = null;

            if(!tab.RenderTabState)
                stateColors = tabColorTable.Default;
            else if (tab.Checked && tab.IsMouseOver && ribbonExpanded)
                stateColors = tabColorTable.SelectedMouseOver;
            else if (tab.Checked && ribbonExpanded)
                stateColors = tabColorTable.Selected;
            else if (tab.IsMouseOver)
                stateColors = tabColorTable.MouseOver;
            else if (tab.GetEnabled())
                stateColors = tabColorTable.Default;

            return stateColors;
        }
#if FRAMEWORK20
        public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image)
        {
            eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
            if (!(effectiveStyle == eDotNetBarStyle.Office2010 && pa.GlassEnabled))
            {
                base.PaintButtonText(button, pa, textColor, image);
                return;
            }

            Rectangle r = GetTextRectangle(button, pa, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter, image);
            //r.Offset(0, 3);
            //r.Height -= 2;
            ThemeTextFormat textFormat = ThemeTextFormat.Center | ThemeTextFormat.VCenter | ThemeTextFormat.HidePrefix | ThemeTextFormat.SingleLine;
            bool renderGlow = true;
            if (effectiveStyle == eDotNetBarStyle.Office2010 && StyleManager.Style == eStyle.Office2010Black)
                renderGlow = false;
            Office2007RibbonControlPainter.PaintTextOnGlass(pa.Graphics, button.Text, pa.Font, r, textFormat, textColor, true, renderGlow, 10);
        }
#endif
    }
}
