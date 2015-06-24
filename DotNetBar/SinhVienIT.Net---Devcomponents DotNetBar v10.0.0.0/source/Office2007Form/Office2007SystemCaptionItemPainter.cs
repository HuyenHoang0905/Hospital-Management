using System;
using System.Text;
using DevComponents.DotNetBar.Rendering;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the painter for the Office 2007 SystemCaptionItem
    /// </summary>
    internal class Office2007SystemCaptionItemPainter : SystemCaptionItemPainter, IOffice2007Painter
    {
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

        #region Internal Implementation
        /// <summary>
        /// Paints the SystemCaptionItem as icon in left hand corner.
        /// </summary>
        /// <param name="e"></param>
        public override void PaintSystemIcon(SystemCaptionItemRendererEventArgs e)
        {
            System.Drawing.Graphics g = e.Graphics;
            SystemCaptionItem item = e.SystemCaptionItem;

            Rectangle r = item.DisplayRectangle;
            r.Offset((r.Width - 16) / 2, (r.Height - 16) / 2);
            if (item.Icon != null)
            {
                if (System.Environment.Version.Build <= 3705 && System.Environment.Version.Revision == 288 && System.Environment.Version.Major == 1 && System.Environment.Version.Minor == 0)
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        NativeFunctions.DrawIconEx(hdc, r.X, r.Y, item.Icon.Handle, r.Width, r.Height, 0, IntPtr.Zero, 3);
                    }
                    finally
                    {
                        g.ReleaseHdc(hdc);
                    }
                }
                else
                    g.DrawIcon(item.Icon, r);
            }
        }

        /// <summary>
        /// Paints the SystemCaptionItem as set of buttons minimize, restore/maximize and close.
        /// </summary>
        /// <param name="e"></param>
        public override void PaintFormButtons(SystemCaptionItemRendererEventArgs e)
        {
            if (e.GlassEnabled) // When Windows Vista Glass is enabled we let system caption button paint themselves
                return;

            System.Drawing.Graphics g = e.Graphics;
            SystemCaptionItem item = e.SystemCaptionItem;
            Office2007SystemButtonColorTable colorTable = m_ColorTable.SystemButton;
            Rectangle r = item.DisplayRectangle;
            Region oldClip = g.Clip;
            Rectangle rclip = r;
            rclip.Height++;
            g.SetClip(rclip);

            Size buttonSize = item.GetButtonSize();
            if (buttonSize.Height > r.Height)
                buttonSize = new Size(r.Height, r.Height);

            // Minimize button
            Rectangle rb = new Rectangle(r.X, r.Y + (r.Height - buttonSize.Height) / 2, buttonSize.Width, buttonSize.Height);
            Office2007SystemButtonStateColorTable ct = colorTable.Default;

            if (item.HelpVisible && (!item.IsRightToLeft || item.CloseVisible && item.IsRightToLeft))
            {
                Office2007SystemButtonColorTable originalColorTable = colorTable;
                if (item.CloseEnabled && item.IsRightToLeft && m_ColorTable.SystemButtonClose != null)
                    colorTable = m_ColorTable.SystemButtonClose;
                
                if (item.CloseEnabled && item.IsRightToLeft || !item.IsRightToLeft)
                {
                    if (item.MouseDownButton == SystemButton.Help && !item.IsRightToLeft ||
                        item.MouseDownButton == SystemButton.Close && item.IsRightToLeft)
                        ct = colorTable.Pressed;
                    else if (item.MouseOverButton == SystemButton.Help && !item.IsRightToLeft ||
                        item.MouseOverButton == SystemButton.Close && item.IsRightToLeft)
                        ct = colorTable.MouseOver;
                }
                PaintBackground(g, rb, ct);
                if (item.IsRightToLeft)
                    PaintClose(g, rb, ct, item.CloseEnabled);
                else
                    PaintHelp(g, rb, ct);

                rb.Offset(rb.Width + 1, 0);
                colorTable = originalColorTable;
            }

            if (item.MinimizeVisible && item.HelpVisible || item.MinimizeVisible && !item.HelpVisible && (!item.IsRightToLeft || item.CloseVisible && item.IsRightToLeft))
            {
                ct = colorTable.Default;
                if (item.CloseEnabled && item.IsRightToLeft || !item.IsRightToLeft)
                {
                    if (item.MouseDownButton == SystemButton.Minimize && !item.IsRightToLeft ||
                        item.MouseDownButton == SystemButton.Close && item.IsRightToLeft)
                        ct = colorTable.Pressed;
                    else if (item.MouseOverButton == SystemButton.Minimize && !item.IsRightToLeft ||
                        item.MouseOverButton == SystemButton.Close && item.IsRightToLeft)
                        ct = colorTable.MouseOver;
                }
                PaintBackground(g, rb, ct);
                if (item.IsRightToLeft)
                    PaintClose(g, rb, ct, item.CloseEnabled);
                else
                    PaintMinimize(g, rb, ct);

                rb.Offset(rb.Width + 1, 0);
            }

            if (item.RestoreMaximizeVisible)
            {
                if (item.RestoreEnabled)
                {
                    ct = colorTable.Default;
                    if (item.MouseDownButton == SystemButton.Restore)
                        ct = colorTable.Pressed;
                    else if (item.MouseOverButton == SystemButton.Restore)
                        ct = colorTable.MouseOver;
                    PaintBackground(g, rb, ct);
                    PaintRestore(g, rb, ct);
                }
                else
                {
                    ct = colorTable.Default;
                    if (item.MouseDownButton == SystemButton.Maximize)
                        ct = colorTable.Pressed;
                    else if (item.MouseOverButton == SystemButton.Maximize)
                        ct = colorTable.MouseOver;
                    PaintBackground(g, rb, ct);
                    PaintMaximize(g, rb, ct);
                }

                rb.Offset(rb.Width + 1, 0);
            }

            if (item.CloseVisible && !item.IsRightToLeft || !item.HelpVisible && item.MinimizeVisible && item.IsRightToLeft || item.HelpVisible && item.IsRightToLeft)
            {
                Office2007SystemButtonColorTable originalColorTable = colorTable;
                if (item.CloseEnabled && !item.IsRightToLeft && m_ColorTable.SystemButtonClose != null)
                    colorTable = m_ColorTable.SystemButtonClose;

                ct = colorTable.Default;
                if (item.CloseEnabled && !item.IsRightToLeft ||
                    item.IsRightToLeft)
                {
                    if (item.MouseDownButton == SystemButton.Close && !item.IsRightToLeft ||
                        item.MouseDownButton == SystemButton.Minimize && item.IsRightToLeft)
                        ct = colorTable.Pressed;
                    else if (item.MouseOverButton == SystemButton.Close && !item.IsRightToLeft ||
                        item.MouseOverButton == SystemButton.Minimize && item.IsRightToLeft)
                        ct = colorTable.MouseOver;
                }

                PaintBackground(g, rb, ct);
                if (item.IsRightToLeft)
                {
                    if (item.HelpVisible)
                        PaintHelp(g, rb, ct);
                    else
                        PaintMinimize(g, rb, ct);
                }
                else
                    PaintClose(g, rb, ct, item.CloseEnabled);

                colorTable = originalColorTable;
            }

            g.Clip = oldClip;
        }

        /// <summary>
        /// Paints the background of the button using specified color table colors.
        /// </summary>
        /// <param name="g">Graphics object.</param>
        /// <param name="r">Background bounds</param>
        /// <param name="ct">Color Table</param>
        protected virtual void PaintBackground(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            int cornerSize = 2;
            Rectangle border = r;
            if (!ct.OuterBorder.IsEmpty)
                r.Inflate(-1, -1);

            Rectangle rt = new Rectangle(r.X, r.Y, r.Width, r.Height / 2);
            if (!ct.TopBackground.IsEmpty)
            {
                rt.Height++;
                DisplayHelp.FillRectangle(g, rt, ct.TopBackground);
                rt.Height--;
            }

            Region oldClip = g.Clip;

            if (!ct.BottomBackground.IsEmpty)
            {
                rt.Y += rt.Height;
                rt.Height = (r.Height - rt.Height);
                DisplayHelp.FillRectangle(g, rt, ct.BottomBackground);
            }

            // Highlight
            if (ct.TopHighlight != null && !ct.TopHighlight.IsEmpty)
            {
                Rectangle fill = r;
                fill.Height = fill.Height / 2;
                DrawHighlight(g, ct.TopHighlight, fill, new PointF(fill.X + fill.Width / 2, fill.Bottom));
            }

            // Highlight
            if (ct.BottomHighlight != null && !ct.BottomHighlight.IsEmpty)
            {
                Rectangle fill = r;
                fill.Height = fill.Height / 2;
                fill.Y += (r.Height - fill.Height);
                DrawHighlight(g, ct.BottomHighlight, fill, new PointF(fill.X + fill.Width / 2, fill.Bottom));
            }

            if (!ct.OuterBorder.IsEmpty)
            {
                DisplayHelp.DrawRoundGradientRectangle(g, border, ct.OuterBorder, 1, cornerSize);
                border.Inflate(-1, -1);
            }

            if (!ct.InnerBorder.IsEmpty)
            {
                DisplayHelp.DrawRoundGradientRectangle(g, border, ct.InnerBorder, 1, cornerSize);
            }
        }

        private void DrawHighlight(Graphics g, LinearGradientColorTable c, Rectangle r, PointF centerPoint)
        {
            Rectangle ellipse = new Rectangle(r.X, r.Y, r.Width, r.Height * 2);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(ellipse);
            PathGradientBrush brush = new PathGradientBrush(path);
            brush.CenterColor = c.Start;
            brush.SurroundColors = new Color[] { c.End };
            brush.CenterPoint = centerPoint;
            Blend blend = new Blend();
            blend.Factors = new float[] { 0f, .5f, 1f };
            blend.Positions = new float[] { .0f, .4f, 1f };
            brush.Blend = blend;

            g.FillRectangle(brush, r);
            brush.Dispose();
            path.Dispose();
        }

        protected virtual void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;

            Size s = new Size(7, 3);
            Rectangle rm = GetSignRect(r, s);

            DisplayHelp.DrawLine(g, rm.X, rm.Y, rm.Right, rm.Y, ct.DarkShade, 1);
            rm.Offset(0, 1);
            DisplayHelp.DrawLine(g, rm.X, rm.Y, rm.Right, rm.Y, ct.Foreground.Start, 1);
            rm.Offset(0, 1);
            DisplayHelp.DrawLine(g, rm.X, rm.Y, rm.Right, rm.Y, ct.LightShade, 1);

            g.SmoothingMode = sm;
        }

        protected virtual Rectangle GetSignRect(Rectangle r, Size s)
        {
            if (r.Height < 10)
                return Rectangle.Empty;

            return new Rectangle(r.X + (r.Width - s.Width) / 2, r.Bottom - r.Height / 4 - s.Height, s.Width, s.Height);
        }

        protected virtual void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;

            Size s = new Size(9, 8);
            Rectangle rm = GetSignRect(r, s);

            Rectangle r1 = new Rectangle(rm.X, rm.Y + 1, 7, 7);
            Region oldClip = g.Clip;
            for (int i = 0; i < 2; i++)
            {
                DisplayHelp.DrawGradientRectangle(g, new Rectangle(r1.X, r1.Y + 1, r1.Width, r1.Height - 1), ct.Foreground, 1);
                DisplayHelp.DrawLine(g, r1.X, r1.Y, r1.Right - 1, r1.Y, ct.DarkShade, 1);
                DisplayHelp.DrawLine(g, r1.X + 1, r1.Y + 2, r1.Right - 2, r1.Y + 2, ct.LightShade, 1);
                DisplayHelp.DrawLine(g, r1.X + 1, r1.Y + 2, r1.Right - 2, r1.Y + 2, ct.LightShade, 1);
                g.SetClip(r1, CombineMode.Exclude);
                r1.Offset(2, -1);
            }

            if (oldClip != null)
                g.Clip = oldClip;
            else
                g.ResetClip();

            g.SmoothingMode = sm;
        }

        protected virtual void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;

            Size s = new Size(9, 8);
            Rectangle rm = GetSignRect(r, s);

            DisplayHelp.DrawLine(g, rm.X, rm.Y, rm.Right - 1, rm.Y, ct.DarkShade, 1);
            rm.Y++;
            rm.Height--;
            DisplayHelp.DrawLine(g, rm.X, rm.Y, rm.Right - 1, rm.Y, ct.Foreground.Start, 1);
            rm.Y++;
            rm.Height--;
            Rectangle r1 = rm;
            r1.Height--;
            DisplayHelp.DrawGradientRectangle(g, r1, ct.Foreground, 1);

            DisplayHelp.DrawLine(g, rm.X, rm.Bottom - 1, rm.Right - 1, rm.Bottom - 1, ct.LightShade, 1);

            g.SmoothingMode = sm;
        }

        protected virtual void PaintClose(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            Size s = new Size(11, 9);
            Rectangle rm = GetSignRect(r, s);

            Rectangle r1 = rm;
            r1.Inflate(-1, 0);
            r1.Height--;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLine(r1.X, r1.Y, r1.X + 2, r1.Y);
                path.AddLine(r1.X + 2, r1.Y, r1.X + 4, r1.Y + 2);
                path.AddLine(r1.X + 4, r1.Y + 2, r1.X + 6, r1.Y + 0);
                path.AddLine(r1.X + 6, r1.Y + 0, r1.X + 8, r1.Y + 0);
                path.AddLine(r1.X + 8, r1.Y + 0, r1.X + 5, r1.Y + 3);
                path.AddLine(r1.X + 5, r1.Y + 4, r1.X + 8, r1.Y + 7);
                path.AddLine(r1.X + 8, r1.Y + 7, r1.X + 6, r1.Y + 7);
                path.AddLine(r1.X + 6, r1.Y + 7, r1.X + 4, r1.Y + 5);
                path.AddLine(r1.X + 4, r1.Y + 5, r1.X + 2, r1.Y + 7);
                path.AddLine(r1.X + 2, r1.Y + 7, r1.X + 0, r1.Y + 7);
                path.AddLine(r1.X + 0, r1.Y + 7, r1.X + 3, r1.Y + 4);
                path.AddLine(r1.X + 3, r1.Y + 3, r1.X, r1.Y);
                if (isEnabled)
                {
                    DisplayHelp.FillPath(g, path, ct.Foreground);
                    DisplayHelp.DrawGradientPathBorder(g, path, ct.Foreground, 1);
                }
                else
                {
                    LinearGradientColorTable lg = new LinearGradientColorTable(ct.Foreground.Start.IsEmpty ? ct.Foreground.Start : Color.FromArgb(128, ct.Foreground.Start),
                        ct.Foreground.End.IsEmpty ? ct.Foreground.End : Color.FromArgb(128, ct.Foreground.End),
                        ct.Foreground.GradientAngle);
                    DisplayHelp.FillPath(g, path, lg);
                    DisplayHelp.DrawGradientPathBorder(g, path, lg, 1);
                }
            }

            if (!ct.DarkShade.IsEmpty)
            {
                using (Pen pen = new Pen(isEnabled ? ct.DarkShade : Color.FromArgb(128, ct.DarkShade), 1))
                {
                    g.DrawLine(pen, r1.X, r1.Y, r1.X + 2, r1.Y);
                    g.DrawLine(pen, r1.X + 2, r1.Y, r1.X + 4, r1.Y + 2);
                    g.DrawLine(pen, r1.X + 4, r1.Y + 2, r1.X + 6, r1.Y + 0);
                    g.DrawLine(pen, r1.X + 6, r1.Y + 0, r1.X + 8, r1.Y + 0);
                }
            }

            if (!ct.LightShade.IsEmpty)
            {
                using (Pen pen = new Pen(isEnabled ? ct.LightShade : Color.FromArgb(128, ct.LightShade), 1))
                {
                    g.DrawLine(pen, rm.X + 0, rm.Y + 8, rm.X + 3, rm.Y + 8);
                    g.DrawLine(pen, rm.X + 3, rm.Y + 8, rm.X + 5, rm.Y + 6);
                    g.DrawLine(pen, rm.X + 5, rm.Y + 6, rm.X + 7, rm.Y + 8);
                    g.DrawLine(pen, rm.X + 7, rm.Y + 8, rm.X + 10, rm.Y + 8);
                }
            }

            g.SmoothingMode = sm;

        }

        protected virtual void PaintHelp(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            SmoothingMode sm = g.SmoothingMode;
            TextRenderingHint th = g.TextRenderingHint;
            g.SmoothingMode = SmoothingMode.Default;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
#if FRAMEWORK20
            using (Font font = new Font(SystemFonts.DefaultFont, FontStyle.Bold))
#else
			using(Font font = new Font("Arial", 10, FontStyle.Bold))
#endif
            {
                Size s = TextDrawing.MeasureString(g, "?", font);
                s.Width += 4;
                s.Height -= 2;
                Rectangle rm = GetSignRect(r, s);

                rm.Offset(1, 1);
                using (SolidBrush brush = new SolidBrush(ct.LightShade))
                    g.DrawString("?", font, brush, rm);
                rm.Offset(-1, -1);
                using (SolidBrush brush = new SolidBrush(ct.Foreground.Start))
                    g.DrawString("?", font, brush, rm);

            }
            g.SmoothingMode = sm;
            g.TextRenderingHint = th;
        }
        #endregion
    }
}
