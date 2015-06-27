using System;
using System.Text;
using DevComponents.DotNetBar.Rendering;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Rendering
{
    internal class Office2010SystemCaptionItemPainter : Office2007SystemCaptionItemPainter
    {
        #region Internal Implementation
        protected override void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            //SmoothingMode sm = g.SmoothingMode;
            //g.SmoothingMode = SmoothingMode.AntiAlias;

            Size s = new Size(11, 5);
            Rectangle rm = GetSignRect(r, s);

            DisplayHelp.DrawRoundedRectangle(g, ct.DarkShade, ct.Foreground, rm, 1);

            //g.SmoothingMode = sm;
        }

        protected override void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            //SmoothingMode sm = g.SmoothingMode;
            //g.SmoothingMode = SmoothingMode.Default;
            
            Size s = new Size(12, 11);
            Rectangle rm = GetSignRect(r, s);
            Region oldClip = g.Clip;

            using (Brush fill = DisplayHelp.CreateBrush(rm, ct.Foreground))
            {
                using (Pen pen = new Pen(ct.DarkShade))
                {
                    using (GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(new Rectangle(rm.X + 5, rm.Y, 8, 8), 1))
                    {
                        Rectangle inner = new Rectangle(rm.X + 7, rm.Y + 4, 4, 2);
                        g.SetClip(inner, CombineMode.Exclude);
                        g.SetClip(new Rectangle(rm.X, rm.Y + 3, 8, 8), CombineMode.Exclude);
                        g.FillPath(fill, path);
                        g.DrawPath(pen, path);
                        g.ResetClip();
                        g.DrawRectangle(pen, inner);
                    }
                    using (GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(new Rectangle(rm.X, rm.Y + 3, 8, 8), 1))
                    {
                        Rectangle inner = new Rectangle(rm.X + 2, rm.Y + 7, 4, 2);
                        g.SetClip(inner, CombineMode.Exclude);
                        g.FillPath(fill, path);
                        g.DrawPath(pen, path);
                        g.ResetClip();
                        g.DrawRectangle(pen, inner);
                    }
                }
            }
            if (oldClip != null)
            {
                g.Clip = oldClip;
                oldClip.Dispose();
            }
            //g.SmoothingMode = sm;
        }

        protected override void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
        {
            Size s = new Size(11, 9);
            Rectangle rm = GetSignRect(r, s);
            Region oldClip = g.Clip;

            using (Brush fill = DisplayHelp.CreateBrush(rm, ct.Foreground))
            {
                using (Pen pen = new Pen(ct.DarkShade))
                {
                    Rectangle inner = new Rectangle(rm.X + 3, rm.Y + 3, 4, 2);
                    g.SetClip(inner, CombineMode.Exclude);
                    DisplayHelp.DrawRoundedRectangle(g, pen, fill, rm.X, rm.Y, rm.Width, rm.Height, 1);
                    g.ResetClip();
                    g.DrawRectangle(pen, inner);
                }
            }

            if (oldClip != null)
            {
                g.Clip = oldClip;
                oldClip.Dispose();
            }
        }

        protected override void PaintClose(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
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
                path.AddLine(r1.X + 1, r1.Y, r1.X + 3, r1.Y);
                path.AddLine(r1.X + 3, r1.Y, r1.X + 5, r1.Y + 2);
                path.AddLine(r1.X + 5, r1.Y + 2, r1.X + 7, r1.Y);
                path.AddLine(r1.X + 7, r1.Y, r1.X + 9, r1.Y);
                path.AddLine(r1.X + 10, r1.Y + 1, r1.X + 7, r1.Y + 4);
                path.AddLine(r1.X + 7, r1.Y + 4, r1.X + 10, r1.Y + 7);
                path.AddLine(r1.X + 10, r1.Y + 7, r1.X + 9, r1.Y + 8);
                path.AddLine(r1.X + 9, r1.Y + 8, r1.X + 7, r1.Y + 8);
                path.AddLine(r1.X + 7, r1.Y + 8, r1.X + 5, r1.Y + 6);
                path.AddLine(r1.X + 5, r1.Y + 6, r1.X + 3, r1.Y + 8);
                path.AddLine(r1.X + 3, r1.Y + 8, r1.X + 1, r1.Y + 8);
                path.AddLine(r1.X, r1.Y + 7, r1.X + 3, r1.Y + 4);
                path.AddLine(r1.X + 3, r1.Y + 4, r1.X, r1.Y + 1);

                if (isEnabled)
                {
                    DisplayHelp.FillPath(g, path, ct.Foreground);
                    using (Pen pen = new Pen(ct.DarkShade))
                    {
                        g.DrawPath(pen, path);
                    }
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

            g.SmoothingMode = sm;
        }

        protected override void PaintHelp(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
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
                using (SolidBrush brush = new SolidBrush(ct.DarkShade))
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
