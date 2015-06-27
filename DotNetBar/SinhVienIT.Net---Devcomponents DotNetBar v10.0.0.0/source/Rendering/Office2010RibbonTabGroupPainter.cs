using System;
using System.Text;
using DevComponents.DotNetBar.Rendering;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar.Rendering
{
    internal class Office2010RibbonTabGroupPainter : Office2007RibbonTabGroupPainter
    {
        #region Internal Implementation
        protected override void PaintTabGroupBackground(System.Drawing.Graphics g, Office2007RibbonTabGroupColorTable colorTable, System.Drawing.Rectangle bounds, System.Drawing.Rectangle groupBounds, bool glassEnabled)
        {
            if (colorTable == null)
                return;

            // Draw title rectangle part of the group
            Rectangle r = bounds;
            r.Height -= 2;

            // GDI+ bug
            Rectangle rFill = r;
            rFill.Width--;
            rFill.Height--;

            // First draw background
            DisplayHelp.FillRectangle(g, rFill, colorTable.Background);//.Start, colorTable.Background.End, 90, new float[] { 0f, 1f, 1f }, new float[] { 0f, .4f, 1f });

            r = bounds;
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;

            Rectangle rAll = groupBounds;

            if (!colorTable.Border.IsEmpty)
            {
                r.Height += 18;
                using (SolidBrush brush = new SolidBrush(colorTable.Border.Start))
                {
                    // Draw border top
                    g.FillRectangle(brush, rAll.X+1, rAll.Y, rAll.Width-2, 4);
                }

                // Draw border ... Left first
                DisplayHelp.FillRectangle(g, new Rectangle(rAll.X+1, r.Y, 1, r.Height), colorTable.Border);
                // Then right
                DisplayHelp.FillRectangle(g, new Rectangle(rAll.Right - 1, r.Y, 1, r.Height), colorTable.Border);
                
                using (Pen pen = new Pen(Color.FromArgb(92, Color.White)))
                {
                    g.DrawLine(pen, rAll.X, r.Y, rAll.X, r.Height - 2);
                    g.DrawLine(pen, rAll.Right, r.Y, rAll.Right, r.Height - 2);
                }
            }
            g.SmoothingMode = sm;
        }

        protected override ThemeTextFormat GetThemeFormat()
        {
            ThemeTextFormat tf = ThemeTextFormat.Center | ThemeTextFormat.EndEllipsis | ThemeTextFormat.Top | ThemeTextFormat.SingleLine;
            return tf;
        }
        protected override eTextFormat GetTextFormat()
        {
            return eTextFormat.Top;
        }
        protected override Themes.RECT GetThemeTextBounds(Rectangle rect)
        {
            Themes.RECT bounds = new Themes.RECT(new Rectangle(0, 6, rect.Width, rect.Height - 6));
            return bounds;
        }
        protected override Rectangle GetTextBounds(RibbonTabGroupRendererEventArgs e)
        {
            Rectangle r = e.Bounds;
            r.Y += 6;
            r.Height -= 6;
            return r;
        }
        #endregion
    }
}
