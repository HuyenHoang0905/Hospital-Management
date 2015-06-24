using System;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    internal class Office2007BarBackgroundPainter : BarBackgroundPainter, IOffice2007Painter
    {
        #region Private Variables
        private float m_TopSplit = .4f;
        #endregion

        #region IOffice2007Painter
        private Office2007ColorTable m_ColorTable = null; //new Office2007ColorTable();
        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return m_ColorTable; }
            set { m_ColorTable = value; }
        }
        #endregion

        #region Docked Background
        /// <summary>
        /// Paints background of docked bar.
        /// </summary>
        /// <param name="e">Context information</param>
        public override void PaintDockedBackground(ToolbarRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            Bar bar = e.Bar;
            ItemPaintArgs pa = e.ItemPaintArgs;
            Rectangle r = e.Bounds;
            ColorScheme cs = m_ColorTable.LegacyColors;
            if (bar.LayoutType == eLayoutType.DockContainer || bar.LayoutType == eLayoutType.TaskList)
            {
                if(bar.HasFocus)
                    DisplayHelp.FillRectangle(g, r, cs.BarBackground, cs.BarBackground2, cs.BarBackgroundGradientAngle);
                else
                    DisplayHelp.FillRectangle(g, r, cs.BarCaptionInactiveBackground, cs.BarCaptionInactiveBackground2, cs.BarCaptionInactiveBackgroundGAngle);
            }
            else if (bar.MenuBar)
            {
                DisplayHelp.FillRectangle(g, r, cs.MenuBarBackground, cs.MenuBarBackground2, cs.MenuBarBackgroundGradientAngle);
            }
            else if (bar.GrabHandleStyle != eGrabHandleStyle.ResizeHandle && bar.BarType != eBarType.StatusBar)
            {
                // Docked state
                if (bar.ItemsContainer.m_BackgroundColor.IsEmpty && bar.BackColor != Color.Transparent)
                {
                    if (bar.IsThemed)
                    {
                        Rectangle rb = new Rectangle(-bar.Location.X, -bar.Location.Y, bar.Parent.Width, bar.Parent.Height);
                        ThemeRebar theme = ((IThemeCache)bar).ThemeRebar;
                        theme.DrawBackground(g, ThemeRebarParts.Background, ThemeRebarStates.Normal, rb);
                    }
                    else
                    {
                        if (IsGradientBackground(bar))
                            DisplayHelp.FillRectangle(g, r, cs.BarBackground, cs.BarBackground2, cs.BarBackgroundGradientAngle, new float[] { 0, .12f, 1f }, new float[] { 0, .50f, 1f });
                        else
                            DisplayHelp.FillRectangle(g, r, cs.BarBackground);
                    }
                }
                else if (!bar.ItemsContainer.BackColor.IsEmpty)
                    DisplayHelp.FillRectangle(g, r, bar.ItemsContainer.BackColor);

                if (bar.Parent != null && bar.Parent.BackgroundImage != null && bar.Parent is DockSite)
                {
                    Rectangle rb = new Rectangle(-bar.Location.X, -bar.Location.Y, bar.Parent.Width, bar.Parent.Height);
                    DockSite site = bar.Parent as DockSite;
                    BarFunctions.PaintBackgroundImage(g, rb, site.BackgroundImage, site.BackgroundImagePosition, site.BackgroundImageAlpha);
                }
                else if(bar.BackgroundImage!=null)
                    BarFunctions.PaintBackgroundImage(g, r, bar.BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);

                if (!bar.IsThemed && bar.LayoutType == eLayoutType.Toolbar && bar.BackColor != Color.Transparent && pa!=null)
                {
                    using (Pen p = new Pen(pa.Colors.BarDockedBorder, 1))
                    {
                        g.DrawLine(p, 0, bar.Height - 1, bar.Width, bar.Height - 1);
                    }
                }
                else
                {
                    Rectangle border = r; // bar.ClientRectangle;
                    border.Inflate(-2, -2);
                    BarFunctions.DrawBorder(g, bar.DockedBorderStyle, border, bar.SingleLineColor);
                }
            }
            else
            {
                if (!bar.BackColor.IsEmpty && bar.ShouldSerializeBackColor())
                {
                    DisplayHelp.FillRectangle(g, r, bar.BackColor);
                }
                else
                {
                    Office2007BarColorTable colorTable = m_ColorTable.Bar;

                    Rectangle back = r;
                    back.Inflate(1, 1);
                    // Fill top background part
                    back.Height = (int)(back.Height * m_TopSplit);
                    back.Height++;
                    DisplayHelp.FillRectangle(g, back, colorTable.ToolbarTopBackground);
                    back.Height--;

                    // Fill bottom background part
                    back.Y += back.Height;
                    back.Height = r.Height - back.Height + 1;
                    DisplayHelp.FillRectangle(g, back, colorTable.ToolbarBottomBackground);

                    if (bar.BarType == eBarType.StatusBar && bar.Items.Count > 0 && bar.Items[bar.Items.Count - 1] is ItemContainer && e.ItemPaintArgs != null && e.ItemPaintArgs.CachedPaint)
                    {
                        ItemContainer ic = bar.Items[bar.Items.Count - 1] as ItemContainer;
                        if (ic.Visible && ic.BackgroundStyle.Class == ElementStyleClassKeys.Office2007StatusBarBackground2Key)
                        {
                            Rectangle bounds = new Rectangle(ic.Bounds.X, r.Y, ic.Bounds.Width, e.Bar.Height + 1);
                            if (e.ItemPaintArgs.RightToLeft)
                            {
                                bounds.Width += bounds.X;
                                bounds.X = 0;
                            }
                            else
                            {
                                bounds.X += e.ItemPaintArgs.ContainerControl.Left;
                                bounds.Width += r.Right - bounds.Right;
                            }
                            ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(ic.BackgroundStyle, g, bounds));
                        }
                    }

                    if (bar.Parent != null && bar.Parent.BackgroundImage != null && bar.Parent is DockSite)
                    {
                        Rectangle backImageRect = new Rectangle(-bar.Location.X, -bar.Location.Y, bar.Parent.Width, bar.Parent.Height);
                        DockSite site = bar.Parent as DockSite;
                        BarFunctions.PaintBackgroundImage(g, backImageRect, site.BackgroundImage, site.BackgroundImagePosition, site.BackgroundImageAlpha);
                    }
                    else if (bar.BackgroundImage != null)
                        BarFunctions.PaintBackgroundImage(g, bar.ClientRectangle, bar.BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);

                    if (!colorTable.ToolbarBottomBorder.IsEmpty && bar.BarType != eBarType.StatusBar ||
                        !colorTable.StatusBarTopBorder.IsEmpty && bar.BarType == eBarType.StatusBar)
                    {
                        if (bar.BarType == eBarType.StatusBar)
                        {
                            using (Pen pen = new Pen(colorTable.StatusBarTopBorder, 1))
                                g.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
                            if (!colorTable.StatusBarTopBorderLight.IsEmpty)
                            {
                                using (Pen pen = new Pen(colorTable.StatusBarTopBorderLight, 1))
                                    g.DrawLine(pen, r.X, r.Y + 1, r.Right, r.Y + 1);
                            }
                        }
                        else
                        {
                            using (Pen pen = new Pen(colorTable.ToolbarBottomBorder, 1))
                                g.DrawLine(pen, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
                        }
                    }
                }
            }
            if (pa != null && !pa.CachedPaint)
                bar.PaintGrabHandle(pa);
        }

        private bool IsGradientBackground(Bar bar)
        {
            if (bar.Style == eDotNetBarStyle.VS2005 && bar.LayoutType == eLayoutType.DockContainer)
                return false;
            return true;
        }
        #endregion

        #region Floating Background
        /// <summary>
        /// Paints background of floating bar.
        /// </summary>
        /// <param name="e">Context information</param>
        public override void PaintFloatingBackground(ToolbarRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            Bar bar = e.Bar;
            ItemPaintArgs pa = e.ItemPaintArgs;
            Rectangle r = e.Bounds;

            ColorScheme cs = m_ColorTable.LegacyColors;
            DisplayHelp.FillRectangle(g, r, cs.BarBackground, cs.BarBackground2, cs.BarBackgroundGradientAngle, new float[] { 0, .12f, 1f }, new float[] { 0, .50f, 1f });

            //Office2007BarColorTable colorTable = m_ColorTable.Bar;
            //Rectangle back = r;
            //// Fill top background part
            //back.Height = (int)(back.Height * m_TopSplit);
            //DisplayHelp.FillRectangle(g, back, colorTable.ToolbarTopBackground);

            //// Fill bottom background part
            //back.Y += back.Height;
            //back.Height = r.Height - back.Height;
            //DisplayHelp.FillRectangle(g, back, colorTable.ToolbarBottomBackground);

            if (bar.BackgroundImage != null)
                BarFunctions.PaintBackgroundImage(g, bar.ClientRectangle, bar.BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);
        }
        #endregion

        #region Popup Background
        /// <summary>
        /// Paints background of popup bar.
        /// </summary>
        /// <param name="e">Context information</param>
        public override void PaintPopupBackground(ToolbarRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            Bar bar = e.Bar;
            ItemPaintArgs pa = e.ItemPaintArgs;
            Office2007BarColorTable colorTable = m_ColorTable.Bar;
            int cornerSize = bar.CornerSize;
            Rectangle r = e.Bounds;

            DisplayHelp.FillRectangle(g, r, colorTable.PopupToolbarBackground.Start, colorTable.PopupToolbarBackground.End, colorTable.PopupToolbarBackground.GradientAngle);

            if (bar.BackgroundImage != null)
                BarFunctions.PaintBackgroundImage(g, bar.ClientRectangle, bar.BackgroundImage, bar.BackgroundImagePosition, bar.BackgroundImageAlpha);

            bar.PaintSideBar(g);

            Rectangle borderRectangle = r;

            if (bar.DisplayShadow && !bar.AlphaShadow)
                borderRectangle = new Rectangle(0, 0, bar.ClientSize.Width - 2, bar.ClientSize.Height - 2);

            using (Pen p = new Pen(colorTable.PopupToolbarBorder, 1))
                DisplayHelp.DrawRoundedRectangle(g, p, borderRectangle, cornerSize);

            if (bar.DisplayShadow && !bar.AlphaShadow)
            {
                // Shadow
                Point[] pt = new Point[3];
                pt[0].X = 2;
                pt[0].Y = r.Height - 1;
                pt[1].X = r.Width - 1;
                pt[1].Y = r.Height - 1;
                pt[2].X = r.Width - 1;
                pt[2].Y = 2;
                using (Pen p = new Pen(SystemColors.ControlDark, 2))
                    g.DrawLines(p, pt);
            }
        }
        #endregion
    }
}
