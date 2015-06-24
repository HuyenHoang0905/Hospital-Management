using System;
using System.Text;
using DevComponents.DotNetBar.Rendering;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the Office 2007 Ribbon Tab Group painter.
    /// </summary>
    internal class Office2007RibbonTabGroupPainter : RibbonTabGroupPainter, IOffice2007Painter
    {
        #region Private Variables
        private Office2007ColorTable m_ColorTable = null; //new Office2007ColorTable();
        #endregion

        #region Internal Implementation
        protected virtual ThemeTextFormat GetThemeFormat()
        {
            ThemeTextFormat tf = ThemeTextFormat.Center | ThemeTextFormat.EndEllipsis | ThemeTextFormat.VCenter | ThemeTextFormat.SingleLine;
            return tf;
        }
        protected virtual eTextFormat GetTextFormat()
        {
            return eTextFormat.VerticalCenter;
        }
        protected virtual Themes.RECT GetThemeTextBounds(Rectangle rect)
        {
            Themes.RECT bounds = new Themes.RECT(new Rectangle(0, 0, rect.Width, rect.Height));
            return bounds;
        }
        protected virtual Rectangle GetTextBounds(RibbonTabGroupRendererEventArgs e)
        {
            return e.Bounds;
        }
        /// <summary>
        /// Paints ribbon tab group.
        /// </summary>
        /// <param name="e">Context information</param>
        public override void PaintTabGroup(RibbonTabGroupRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            Office2007RibbonTabGroupColorTable colorTable = GetColors(e.RibbonTabItemGroup);
            if (colorTable == null)
                return;
            #if FRAMEWORK20
            if (e.ItemPaintArgs.GlassEnabled)
            {
                if (e.ItemPaintArgs.CachedPaint) return;
                PaintTabGroupBackground(g, colorTable, e.Bounds, e.GroupBounds, true);
                
                Rectangle rect = e.Bounds;
                rect.Height -= 2;
                IntPtr hdc = g.GetHdc();
                Font font = e.GroupFont;
                string text = e.RibbonTabItemGroup.GroupTitle;
                ThemeTextFormat tf = GetThemeFormat();
                try
                {
                    IntPtr memdc = WinApi.CreateCompatibleDC(hdc);
                    try
                    {
                        WinApi.BITMAPINFO bmpInfo = new WinApi.BITMAPINFO();
                        bmpInfo.biWidth = rect.Width;
                        bmpInfo.biHeight = -rect.Height;
                        bmpInfo.biPlanes = 1;
                        bmpInfo.biBitCount = 32;
                        bmpInfo.biSize = Marshal.SizeOf(bmpInfo);
                        IntPtr dib = WinApi.CreateDIBSection(hdc, bmpInfo, 0, 0, IntPtr.Zero, 0);
                        WinApi.SelectObject(memdc, dib);

                        IntPtr fontHandle = font.ToHfont();
                        WinApi.SelectObject(memdc, fontHandle);

                        Themes.RECT bounds = GetThemeTextBounds(rect);
                        System.Windows.Forms.VisualStyles.VisualStyleRenderer themeRenderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.Window.Caption.Active);
                        Themes.DTTOPTS dttOpts = new Themes.DTTOPTS();
                        dttOpts.iGlowSize = 10;
                        dttOpts.crText = new Themes.COLORREF(colorTable.Text);
                        dttOpts.dwFlags = (int)Themes.DTT_VALIDBITS.DTT_COMPOSITED | (int)Themes.DTT_VALIDBITS.DTT_TEXTCOLOR;
                        if (colorTable.Background == null || colorTable.Background.IsEmpty || colorTable.Background.Start.A < 255)
                            dttOpts.dwFlags |= (int)Themes.DTT_VALIDBITS.DTT_GLOWSIZE;
                        dttOpts.dwSize = Marshal.SizeOf(dttOpts);
                        
                        // Draw Background
                        using (Graphics gb = Graphics.FromHdc(memdc))
                        {
                            PaintTabGroupBackground(gb, colorTable, new Rectangle(0, 0, rect.Width, rect.Height + 2), new Rectangle(0, 0, rect.Width, rect.Height + 2), true);
                        }
                        
                        Themes.DrawThemeTextEx(themeRenderer.Handle, memdc, 0, 0, text, -1, (int)tf, ref bounds, ref dttOpts);
                        

                        const int SRCCOPY = 0x00CC0020;
                        WinApi.BitBlt(hdc, rect.Left, rect.Top, rect.Width, rect.Height, memdc, 0, 0, SRCCOPY);

                        WinApi.DeleteObject(fontHandle);
                        WinApi.DeleteObject(dib);
                    }
                    finally
                    {
                        WinApi.DeleteDC(memdc);
                    }
                }
                finally
                {
                    g.ReleaseHdc(hdc);
                }
                return;
            }
            #endif

            PaintTabGroupBackground(g, colorTable, e.Bounds, e.GroupBounds, false);
            ElementStyle style = e.RibbonTabItemGroup.Style; //.Copy();
            Color styleTextColor = style.TextColor;
            Color styleTextShadowColor = style.TextShadowColor;
            Point styleTextShadowOffset = style.TextShadowOffset;
            style.FreezeEvents = true;
            style.TextColor = colorTable.Text;
            style.TextShadowColor = Color.Empty;
            style.TextShadowOffset = Point.Empty;

            ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(style, e.Graphics, GetTextBounds(e));
            ElementStyleDisplay.PaintText(info, e.RibbonTabItemGroup.GroupTitle, e.GroupFont, false, e.RibbonTabItemGroup.Style.TextFormat | GetTextFormat());

            style.TextColor = styleTextColor;
            style.TextShadowColor = styleTextShadowColor;
            style.TextShadowOffset = styleTextShadowOffset;
            style.FreezeEvents = false;
        }

        protected virtual void PaintTabGroupBackground(Graphics g, Office2007RibbonTabGroupColorTable colorTable, Rectangle bounds, Rectangle groupBounds, bool glassEnabled)
        {
            if (colorTable == null)
                return;

            // Draw title rectangle part of the group
            Rectangle r = bounds;
            r.Height -= 2;

            // GDI+ bug
            Rectangle rFill = r;
            rFill.Width--;
            //rFill.Height--;

            // First draw background
            DisplayHelp.FillRectangle(g, rFill, colorTable.Background.Start, colorTable.Background.End, 90, new float[] { 0f, .10f, .9f }, new float[] { 0f, (glassEnabled?.4f:.70f), 1f });

            // Draw highlight
            if (!colorTable.BackgroundHighlight.IsEmpty && r.Width > 0 && r.Height > 0)
            {
                //Rectangle hr = new Rectangle(r.X, r.Bottom - 3, r.Width - 1, 3);
                //DisplayHelp.FillRectangle(g, hr, colorTable.BackgroundHighlight.Start, colorTable.BackgroundHighlight.End);
                Rectangle ellipse = new Rectangle(r.X - r.Width * 3, r.Y, r.Width * 7, (int)(r.Height * 4.5f));
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(ellipse);
                PathGradientBrush brush = new PathGradientBrush(path);
                brush.CenterColor = colorTable.BackgroundHighlight.Start;
                brush.SurroundColors = new Color[] { colorTable.BackgroundHighlight.End };
                brush.CenterPoint = new PointF(ellipse.X + ellipse.Width / 2, r.Bottom);
                Blend blend = new Blend();
                blend.Factors = new float[] { 0f, .05f, 1f };
                blend.Positions = new float[] { .0f, .8f, 1f };
                brush.Blend = blend;
                path.Dispose();
                //rFill.Height++;
                g.FillRectangle(brush, rFill);
                brush.Dispose();
                path.Dispose();
            }
            // Underline highlight
            using (Pen pen = new Pen(Color.FromArgb(64, System.Windows.Forms.ControlPaint.Dark(colorTable.BackgroundHighlight.Start))))
                g.DrawLine(pen, r.X, r.Bottom, r.Right - 1, r.Bottom);

            r = bounds;
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;

            Rectangle rAll = groupBounds;

            if (!colorTable.Border.IsEmpty)
            {
                //using(SolidBrush brush=new SolidBrush(colorTable.Border.Start))
                //{
                //    // Draw border top
                //    g.FillRectangle(brush, bounds.X, bounds.Y, bounds.Width, 4);
                //}

                // Draw border ... Left first
                DisplayHelp.FillRectangle(g, new Rectangle(rAll.X, r.Y, 1, r.Height), colorTable.Border);

                // Then right
                DisplayHelp.FillRectangle(g, new Rectangle(rAll.Right - 1, r.Y, 1, r.Height), colorTable.Border);

                // Draw borders on the bottom...
                // Left first
                DisplayHelp.FillRectangle(g, new Rectangle(rAll.X, r.Bottom - 1, 1, rAll.Height - r.Height), colorTable.Border.End, Color.Transparent, 90);

                // Then right
                DisplayHelp.FillRectangle(g, new Rectangle(rAll.Right - 1, r.Bottom - 1, 1, rAll.Height - r.Height), colorTable.Border.End, Color.Transparent, 90);
            }
            g.SmoothingMode = sm;
        }

        private Office2007RibbonTabGroupColorTable GetColors(RibbonTabItemGroup group)
        {
            Office2007RibbonTabGroupColorTable c = null;
            if(group.CustomColorName!="")
                c = m_ColorTable.RibbonTabGroupColors[group.CustomColorName];
            if(c==null)
                c = m_ColorTable.RibbonTabGroupColors[Enum.GetName(typeof(eRibbonTabGroupColor), group.Color)];
            if (c == null && m_ColorTable.RibbonTabGroupColors.Count > 0)
                c = m_ColorTable.RibbonTabGroupColors[0];
            return c;
        }
        #endregion

        #region IOffice2007Painter Members

        public Office2007ColorTable ColorTable
        {
            get { return m_ColorTable; }
            set { m_ColorTable = value; }
        }

        #endregion
    }
}
