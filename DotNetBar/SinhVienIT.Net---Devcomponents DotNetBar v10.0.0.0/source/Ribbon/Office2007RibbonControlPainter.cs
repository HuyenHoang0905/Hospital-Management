using System;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
    internal class Office2007RibbonControlPainter : RibbonControlPainter, Rendering.IOffice2007Painter
    {
        private Rendering.Office2007ColorTable m_ColorTable = null; //new Rendering.Office2007ColorTable();

        #region Ribbon Control Background
        /// <summary>
        /// Paints controls background
        /// </summary>
        public override void PaintBackground(RibbonControlRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            RibbonControl rc = e.RibbonControl;
            Rendering.Office2007RibbonColorTable ct = m_ColorTable.RibbonControl;
            Rendering.Office2007RibbonTabItemStateColorTable selectedTabColors = null;

            PaintCaptionBackground(e, rc.DisplayRectangle);
            
            if (rc.Expanded && rc.RibbonStrip.HasVisibleTabs)
            {
                int cornerSize = ct.CornerSize;
                // Draw the border which is base for the tabs
                Rectangle selectedTabRect = Rectangle.Empty;
                if (rc.SelectedRibbonTabItem == null || !rc.Expanded)
                    selectedTabRect = Rectangle.Empty;
                else
                {
                    selectedTabRect = rc.SelectedRibbonTabItem.DisplayRectangle;
                    selectedTabColors = Office2007RibbonTabItemPainter.GetStateColorTable(Office2007RibbonTabItemPainter.GetColorTable(this.ColorTable, rc.SelectedRibbonTabItem), rc.SelectedRibbonTabItem, rc.Expanded);
                }

                Rectangle bottomBounds = new Rectangle(rc.ClientRectangle.X, rc.ClientRectangle.Bottom - cornerSize, rc.ClientRectangle.Width - 1, cornerSize);
                if (!selectedTabRect.IsEmpty)
                    bottomBounds = new Rectangle(rc.ClientRectangle.X, selectedTabRect.Bottom,
                        rc.ClientRectangle.Width - 1, rc.ClientRectangle.Bottom - selectedTabRect.Bottom);

                using (GraphicsPath path = GetBottomLinePath(bottomBounds, cornerSize))
                {
                    using (SolidBrush brush = new SolidBrush(m_ColorTable.RibbonBar.Default.TopBackground.Start))
                        g.FillPath(brush, path);
                }

                // Dark border
                using (GraphicsPath path = GetBottomLinePath(bottomBounds, cornerSize))
                {
                    Region oldRegion = null;
                    if (g.Clip != null) oldRegion = g.Clip as Region;

                    if (!selectedTabRect.IsEmpty)
                    {
                        if (selectedTabColors != null)
                        {
                            using (Pen pen = new Pen(selectedTabColors.Background.End, 1))
                                g.DrawLine(pen, selectedTabRect.X + 1, selectedTabRect.Bottom, selectedTabRect.Right - 1, selectedTabRect.Bottom);
                        }
                        g.SetClip(new Rectangle(selectedTabRect.X + 1, selectedTabRect.Bottom, selectedTabRect.Width - 1, 1), CombineMode.Exclude);
                    }

                    using (Pen pen = new Pen(ct.OuterBorder.Start, 1))
                        g.DrawPath(pen, path);

                    if (!selectedTabRect.IsEmpty)
                    {
                        if (oldRegion != null)
                            g.Clip = oldRegion;
                        else
                            g.ResetClip();
                    }
                    if (oldRegion != null) oldRegion.Dispose();
                }

                // Light border
                bottomBounds.Y++;
                using (GraphicsPath path = GetBottomLinePath(bottomBounds, cornerSize))
                {
                    using (Pen pen = new Pen(ct.InnerBorder.Start, 1))
                        g.DrawPath(pen, path);
                }
            }
            else
            {
                int topLineY = rc.ClientRectangle.Bottom - 2;
                if (!ct.TabDividerBorder.IsEmpty)
                {
                    using (Pen pen = new Pen(ct.TabDividerBorder, 1))
                        g.DrawLine(pen, 0, topLineY, rc.ClientRectangle.Right, topLineY);
                    topLineY++;
                }

                if (!ct.TabDividerBorderLight.IsEmpty)
                {
                    using (Pen pen = new Pen(ct.TabDividerBorderLight, 1))
                        g.DrawLine(pen, 0, topLineY, rc.ClientRectangle.Right, topLineY);
                }
            }
        }

        /// <summary>
        /// Paints form caption background
        /// </summary>
        public override void PaintCaptionBackground(RibbonControlRendererEventArgs e, Rectangle displayRect)
        {
            Graphics g = e.Graphics;
            RibbonControl rc = e.RibbonControl;
            Rendering.Office2007RibbonColorTable ct = m_ColorTable.RibbonControl;

            // Draw top divider line that sits between the tabs and any items above
            int topLineY = GetTabYPosition(rc.Items);
            if (topLineY == -1) topLineY = rc.RibbonStrip.GetAbsoluteCaptionHeight() + 2;

            if (topLineY > 2)
            {
                if (e.GlassEnabled && rc.EffectiveStyle == eDotNetBarStyle.Windows7)
                    topLineY -= 2;
                else
                    topLineY -= 3;

                if (!e.GlassEnabled)
                {
                    if (!ct.TabDividerBorder.IsEmpty)
                    {
                        using (Pen pen = new Pen(ct.TabDividerBorder, 1))
                            g.DrawLine(pen, 0, topLineY, displayRect.Right + 12, topLineY); // 12 extension when painting from Office2007RibbonForm
                        topLineY++;
                    }

                    if (!ct.TabDividerBorderLight.IsEmpty)
                    {
                        using (Pen pen = new Pen(ct.TabDividerBorderLight, 1))
                            g.DrawLine(pen, 0, topLineY, displayRect.Right + 12, topLineY);
                    }
                }
                else
                {
                    if (rc.EffectiveStyle != eDotNetBarStyle.Office2010)
                    {
                        using (Pen pen = new Pen(m_ColorTable.Form.Active.BorderColors[0], 1))
                            g.DrawLine(pen, 0, topLineY, displayRect.Right + 12, topLineY); // 12 extension when painting from Office2007RibbonForm
                    }
                }
            }

            // Draw Caption if necessary
            if (rc.RibbonStrip.CaptionVisible)
            {
                Rendering.Office2007FormStateColorTable formct = m_ColorTable.Form.Active;
                System.Windows.Forms.Form form = rc.FindForm();
                if (!rc.IsDesignMode && form != null && (form != System.Windows.Forms.Form.ActiveForm && form.MdiParent == null ||
                    form.MdiParent != null && form.MdiParent.ActiveMdiChild != form))
                {
                    formct = m_ColorTable.Form.Inactive;
                }
                Rectangle captionRect = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width, topLineY - 1);
                if (captionRect.Height <= 0)
                    captionRect.Height = rc.RibbonStrip.GetAbsoluteCaptionHeight();

                if (!e.GlassEnabled)
                {
                    Rectangle drawRect = captionRect;
                    if (form is Office2007RibbonForm)
                        drawRect.Width = ((Office2007RibbonForm)form).Width;
                    SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.Default;

                    if (!formct.CaptionTopBackground.End.IsEmpty && !formct.CaptionBottomBackground.End.IsEmpty)
                    {
                        using (LinearGradientBrush lb = new LinearGradientBrush(drawRect, formct.CaptionTopBackground.Start, formct.CaptionBottomBackground.End, formct.CaptionTopBackground.GradientAngle))
                        {
                            ColorBlend cb = new ColorBlend(4);
                            cb.Colors = new Color[] { formct.CaptionTopBackground.Start, formct.CaptionTopBackground.End, formct.CaptionBottomBackground.Start, formct.CaptionBottomBackground.End };
                            cb.Positions = new float[] { 0, .4f, .4f, 1f };
                            lb.InterpolationColors = cb;
                            g.FillRectangle(lb, drawRect);
                        }
                    }
                    else
                    {
                        using (LinearGradientBrush lb = new LinearGradientBrush(drawRect, formct.CaptionTopBackground.Start, formct.CaptionBottomBackground.Start, formct.CaptionTopBackground.GradientAngle))
                        {
                            g.FillRectangle(lb, drawRect);
                        }
                    }
                    //// Top Part
                    //Rectangle topCaptionPart = new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, (int)(drawRect.Height * .4));
                    //DisplayHelp.FillRectangle(g, topCaptionPart, formct.CaptionTopBackground);
                    //Rectangle bottomCaptionPart = new Rectangle(drawRect.X, topCaptionPart.Bottom, drawRect.Width, drawRect.Height - topCaptionPart.Height);
                    //DisplayHelp.FillRectangle(g, bottomCaptionPart, formct.CaptionBottomBackground);
                    g.SmoothingMode = sm;
                }
                rc.RibbonStrip.CaptionBounds = captionRect;
            }
        }

        private GraphicsPath GetBottomLinePath(Rectangle bottomBounds, int cornerSize)
        {
            GraphicsPath path = new GraphicsPath();

            if (cornerSize > 0)
            {
                if (bottomBounds.Height > cornerSize)
                    path.AddLine(bottomBounds.X, bottomBounds.Bottom, bottomBounds.X, bottomBounds.Y + cornerSize);
                ElementStyleDisplay.AddCornerArc(path, bottomBounds, cornerSize, eCornerArc.TopLeft);

                path.AddLine(bottomBounds.X + cornerSize, bottomBounds.Y, bottomBounds.Right - cornerSize, bottomBounds.Y);

                ElementStyleDisplay.AddCornerArc(path, bottomBounds, cornerSize, eCornerArc.TopRight);

                if (bottomBounds.Height > cornerSize)
                    path.AddLine(bottomBounds.Right, bottomBounds.Y + cornerSize, bottomBounds.Right, bottomBounds.Bottom);
            }
            else
            {
                path.AddRectangle(new Rectangle(bottomBounds.X, bottomBounds.Y, bottomBounds.Right, 3));
            }

            return path;
        }

        private int GetTabYPosition(SubItemsCollection c)
        {
            foreach (BaseItem item in c)
            {
                if (item is RibbonTabItem && item.Visible)
                {
                    return item.Bounds.Top;
                }
            }
            return -1;
        }
        #endregion

        #region IOffice2007Painter Members

        public DevComponents.DotNetBar.Rendering.Office2007ColorTable ColorTable
        {
            get
            {
                return m_ColorTable;
            }
            set
            {
                m_ColorTable=value;
            }
        }

        #endregion

        #region Form Caption Text Drawing
        public override void PaintCaptionText(RibbonControlRendererEventArgs e)
        {
            RibbonStrip rs = e.RibbonControl.RibbonStrip;
            if (!rs.CaptionVisible || rs.CaptionBounds.IsEmpty)
                return;

            Graphics g = e.Graphics;
            bool isMaximized = false;
            bool isFormActive = true;
            Rendering.Office2007FormStateColorTable formct = m_ColorTable.Form.Active;
            System.Windows.Forms.Form form = rs.FindForm();
            if (form != null && (form != System.Windows.Forms.Form.ActiveForm && form.MdiParent == null ||
                form.MdiParent != null && form.MdiParent.ActiveMdiChild != form))
            {
                formct = m_ColorTable.Form.Inactive;
                isFormActive = false;
            }
            string text = e.RibbonControl.TitleText;
            string plainText = text;
            bool isTitleTextMarkup = e.RibbonControl.RibbonStrip.TitleTextMarkupBody != null;
            if (isTitleTextMarkup)
                plainText = e.RibbonControl.RibbonStrip.TitleTextMarkupBody.PlainText;
            if (form != null)
            {
                if (text == "")
                {
                    text = form.Text;
                    plainText = text;
                }
                isMaximized = form.WindowState == System.Windows.Forms.FormWindowState.Maximized;
            }

            Rectangle captionRect = rs.CaptionBounds;

            // Exclude quick access toolbar if any
            if (!rs.QuickToolbarBounds.IsEmpty)
                DisplayHelp.ExcludeEdgeRect(ref captionRect, rs.QuickToolbarBounds);
            else
            {
                BaseItem sb = e.RibbonControl.GetApplicationButton();
                if(sb!=null && sb.Visible && sb.Displayed)
                    DisplayHelp.ExcludeEdgeRect(ref captionRect, sb.Bounds);
                else
                    DisplayHelp.ExcludeEdgeRect(ref captionRect, new Rectangle(0, 0, 22, 22)); // The system button in top-left corner
            }

            if(!rs.SystemCaptionItemBounds.IsEmpty)
                DisplayHelp.ExcludeEdgeRect(ref captionRect, rs.SystemCaptionItemBounds);

            
            // Array of the rectangles after they are split
            ArrayList rects = new ArrayList(5);
            ArrayList tempRemoveList = new ArrayList(5);
            // Exclude Context Tabs Captions if any
            if (rs.TabGroupsVisible)
            {
                foreach (RibbonTabItemGroup group in rs.TabGroups)
                {
                    foreach (Rectangle r in group.DisplayPositions)
                    {
                        if (rects.Count > 0)
                        {
                            tempRemoveList.Clear();
                            Rectangle[] arrCopy = new Rectangle[rects.Count];
                            rects.CopyTo(arrCopy);
                            for(int irc = 0; irc<arrCopy.Length; irc++)
                            {
                                if (arrCopy[irc].IntersectsWith(r))
                                {
                                    tempRemoveList.Add(irc);
                                    Rectangle[] splitRects = DisplayHelp.ExcludeRectangle(arrCopy[irc], r);
                                    rects.AddRange(splitRects);
                                }
                            }
                            foreach (int idx in tempRemoveList)
                                rects.RemoveAt(idx);
                        }
                        else
                        {
                            if (r.IntersectsWith(captionRect))
                            {
                                Rectangle[] splitRects = DisplayHelp.ExcludeRectangle(captionRect, r);
                                if (splitRects.Length > 0)
                                {
                                    rects.AddRange(splitRects);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            Font font = System.Windows.Forms.SystemInformation.MenuFont;
            bool disposeFont = true;
            if (rs.CaptionFont != null)
            {
                font.Dispose();
                font = rs.CaptionFont;
                disposeFont = false;
            }
            Size textSize = Size.Empty;
            if (isTitleTextMarkup)
            {
                textSize = e.RibbonControl.RibbonStrip.TitleTextMarkupBody.Bounds.Size;
            }
            else
            {
                textSize = TextDrawing.MeasureString(g, plainText, font);
            }

            if (rects.Count > 0)
            {
                rs.CaptionTextBounds = (Rectangle[])rects.ToArray(typeof(Rectangle));
                if (e.RibbonControl.RightToLeft == System.Windows.Forms.RightToLeft.No)
                    rects.Reverse();
                captionRect = Rectangle.Empty;
                foreach(Rectangle r in rects)
                {
                    if (r.Width >= textSize.Width)
                    {
                        captionRect = r;
                        break;
                    }
                    else if (r.Width > captionRect.Width)
                        captionRect = r;
                }
            }
            else
                rs.CaptionTextBounds = new Rectangle[] { captionRect };

            if (captionRect.Width > 6 && captionRect.Height > 6)
            {
                if (e.GlassEnabled && e.ItemPaintArgs != null && e.ItemPaintArgs.ThemeWindow != null && !e.RibbonControl.IsDesignMode)
                {
                    if (!e.ItemPaintArgs.CachedPaint || isMaximized)
                        PaintGlassText(g, plainText, font, captionRect, isMaximized);
                }
                else
                {
                    if (!isTitleTextMarkup)
                        TextDrawing.DrawString(g, plainText, font, formct.CaptionText, captionRect, eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter | eTextFormat.EndEllipsis | eTextFormat.NoPrefix);
                    else
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, formct.CaptionText, e.RibbonControl.RightToLeft == System.Windows.Forms.RightToLeft.Yes, captionRect, false);
                        d.AllowMultiLine = false;
                        d.IgnoreFormattingColors = !isFormActive;
                        TextMarkup.BodyElement body = e.RibbonControl.RibbonStrip.TitleTextMarkupBody;
                        if (e.RibbonControl.RibbonStrip.TitleTextMarkupLastArrangeBounds != captionRect)
                        {
                            body.Measure(captionRect.Size, d);
                            body.Arrange(captionRect, d);
                            e.RibbonControl.RibbonStrip.TitleTextMarkupLastArrangeBounds = captionRect;
                            Rectangle mr = body.Bounds;
                            if (mr.Width < captionRect.Width)
                                mr.Offset((captionRect.Width - mr.Width) / 2, 0);
                            if (mr.Height < captionRect.Height)
                                mr.Offset(0, (captionRect.Height - mr.Height) / 2);
                            body.Bounds = mr;
                        }
                        Region oldClip = g.Clip;
                        g.SetClip(captionRect, CombineMode.Intersect);
                        body.Render(d);
                        g.Clip = oldClip;
                        if (oldClip != null) oldClip.Dispose();
                    }
                }
            }

            if (disposeFont) font.Dispose();
        }

        //private void PaintGlassTextTheme(Graphics g, string text, Font font, Rectangle captionRect, ItemPaintArgs pa)
        //{
        //    //TextDrawing.DrawString(g, text, font, SystemColors.ActiveCaptionText, captionRect, eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter | eTextFormat.EndEllipsis | eTextFormat.NoPrefix);
        //    //Bitmap bmp = new Bitmap(captionRect.Width, captionRect.Height);
        //    //bmp.MakeTransparent();
        //    //using (Graphics gb = Graphics.FromImage(bmp))
        //    {
        //        //gb.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, bmp.Width, bmp.Height));
        //        //gb.FillRectangle(new SolidBrush(Color.FromArgb(186, Color.White)), new Rectangle(0, 0, bmp.Width, bmp.Height));
        //        //g.ResetTransform();
        //        //e.ItemPaintArgs.ThemeWindow.DrawText(g, text, font, captionRect,
        //        //    ThemeWindowParts.Caption, ThemeWindowStates.CaptionActive);
        //        //g.ResetTransform();
        //        Themes.DTTOPTS options = new Themes.DTTOPTS();
        //        options.iGlowSize = 12;
        //        options.crText = new Themes.COLORREF(SystemColors.ActiveCaptionText);
        //        options.dwFlags = (int)(Themes.DTT_VALIDBITS.DTT_TEXTCOLOR |
        //            Themes.DTT_VALIDBITS.DTT_APPLYOVERLAY |
        //            Themes.DTT_VALIDBITS.DTT_COMPOSITED | Themes.DTT_VALIDBITS.DTT_GLOWSIZE
        //                                                                                     );
        //        pa.ThemeWindow.DrawTextEx(g, text, font, captionRect,
        //            ThemeWindowParts.Caption, ThemeWindowStates.CaptionActive,
        //            ThemeTextFormat.Center | ThemeTextFormat.VCenter | ThemeTextFormat.EndEllipsis | ThemeTextFormat.NoPrefix |
        //            ThemeTextFormat.SingleLine,
        //            options);
        //        //g.ResetTransform();
        //    }

        //    //bmp.MakeTransparent(Color.Black);
        //    //g.DrawImage(bmp, captionRect.Location);
        //    //bmp.Save(@"TitleImage.png", System.Drawing.Imaging.ImageFormat.Png);
        //    //bmp.Dispose();
        //}
#if FRAMEWORK20
        public static void PaintTextOnGlass(Graphics g, string text, Font font, Rectangle rect, ThemeTextFormat tf)
        {
            PaintTextOnGlass(g, text, font, rect, tf, SystemColors.ControlText, false, true, 16);
        }
        public static void PaintTextOnGlass(Graphics g, string text, Font font, Rectangle rect, ThemeTextFormat tf, Color textColor, bool copySourceBackground, bool renderGlow, int glowSize)
        {
            IntPtr hdc = g.GetHdc();
            const int SRCCOPY = 0x00CC0020;
            try
            {
                IntPtr memdc = WinApi.CreateCompatibleDC(hdc);
                try
                {
                    WinApi.BITMAPINFO info = new WinApi.BITMAPINFO();
                    info.biWidth = rect.Width;
                    info.biHeight = -rect.Height;
                    info.biPlanes = 1;
                    info.biBitCount = 32;
                    info.biSize = Marshal.SizeOf(info);
                    IntPtr dib = WinApi.CreateDIBSection(hdc, info, 0, 0, IntPtr.Zero, 0);
                    WinApi.SelectObject(memdc, dib);

                    IntPtr fontHandle = font.ToHfont();
                    WinApi.SelectObject(memdc, fontHandle);

                    Themes.RECT bounds = new Themes.RECT(new Rectangle(0, 0, rect.Width, rect.Height));
                    System.Windows.Forms.VisualStyles.VisualStyleRenderer themeRenderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.Window.Caption.Active);
                    Themes.DTTOPTS dttOpts = new Themes.DTTOPTS();
                    dttOpts.iGlowSize = glowSize;
                    dttOpts.crText = new Themes.COLORREF(textColor);
                    dttOpts.dwFlags = (int)Themes.DTT_VALIDBITS.DTT_COMPOSITED | 
                        (renderGlow ? (int)Themes.DTT_VALIDBITS.DTT_GLOWSIZE : 0) | 
                        (int)Themes.DTT_VALIDBITS.DTT_TEXTCOLOR;
                    dttOpts.dwSize = Marshal.SizeOf(dttOpts);

                    if (copySourceBackground)
                        WinApi.BitBlt(memdc, 0, 0, rect.Width, rect.Height, hdc, rect.Left, rect.Top, SRCCOPY);

                    Themes.DrawThemeTextEx(themeRenderer.Handle, memdc, 0, 0, text, -1, (int)tf, ref bounds, ref dttOpts);

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
        }

        public static void PaintShadowTextOnGlass(Graphics g, string text, Font font, Rectangle rect, ThemeTextFormat tf, Color textColor, Color shadowColor, bool copySourceBackground)
        {
            Point transformPoint = new Point();
            using (Matrix trans = g.Transform)
            {
                Point[] pts = new Point[1];
                pts[0] = transformPoint;
                trans.TransformPoints(pts);
                transformPoint = pts[0];
            }
            
            IntPtr hdc = g.GetHdc();
            const int SRCCOPY = 0x00CC0020;
            try
            {
                IntPtr memdc = WinApi.CreateCompatibleDC(hdc);
                try
                {
                    WinApi.BITMAPINFO info = new WinApi.BITMAPINFO();
                    info.biWidth = rect.Width;
                    info.biHeight = -rect.Height;
                    info.biPlanes = 1;
                    info.biBitCount = 32;
                    info.biSize = Marshal.SizeOf(info);
                    IntPtr dib = WinApi.CreateDIBSection(hdc, info, 0, 0, IntPtr.Zero, 0);
                    WinApi.SelectObject(memdc, dib);

                    IntPtr fontHandle = font.ToHfont();
                    WinApi.SelectObject(memdc, fontHandle);

                    Themes.RECT bounds = new Themes.RECT(new Rectangle(0, 0, rect.Width, rect.Height));
                    System.Windows.Forms.VisualStyles.VisualStyleRenderer themeRenderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.Window.Caption.Active);
                    Themes.DTTOPTS dttOpts = new Themes.DTTOPTS();
                    dttOpts.crText = new Themes.COLORREF(shadowColor);
                    dttOpts.dwFlags = (int)Themes.DTT_VALIDBITS.DTT_COMPOSITED |
                        (int)Themes.DTT_VALIDBITS.DTT_TEXTCOLOR;
                    dttOpts.dwSize = Marshal.SizeOf(dttOpts);

                    if (copySourceBackground)
                        WinApi.BitBlt(memdc, 0, 0, rect.Width, rect.Height, hdc, rect.Left + transformPoint.X, rect.Top + transformPoint.Y, SRCCOPY);
                    // Shadow
                    bounds.Offset(1, 2);
                    Themes.DrawThemeTextEx(themeRenderer.Handle, memdc, 0, 0, text, -1, (int)tf, ref bounds, ref dttOpts);
                    bounds.Offset(-1, -1);
                    dttOpts.crText = new Themes.COLORREF(textColor);
                    Themes.DrawThemeTextEx(themeRenderer.Handle, memdc, 0, 0, text, -1, (int)tf, ref bounds, ref dttOpts);
                    //Themes.DrawThemeTextEx(themeRenderer.Handle, memdc, 0, 0, text, -1, (int)tf, ref bounds, ref dttOpts);

                    WinApi.BitBlt(hdc, rect.Left + transformPoint.X, rect.Top + transformPoint.Y, rect.Width, rect.Height, memdc, 0, 0, SRCCOPY);

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
        }

        private void PaintGlassText(Graphics g, string text, Font font, Rectangle captionRect, bool isMaximized)
        {
            if (isMaximized && !BarFunctions.IsWindows7)
            {
                captionRect.Offset(0, 3);
                TextDrawing.DrawString(g, text, font, Color.White, captionRect, eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter | eTextFormat.EndEllipsis | eTextFormat.NoPrefix);
                return;
            }
            if (isMaximized && BarFunctions.IsWindows7)
            {
                captionRect.Y += 6;
                captionRect.Height -= 6;
            }
            ThemeTextFormat tf = ThemeTextFormat.Center | ThemeTextFormat.EndEllipsis | ThemeTextFormat.VCenter | ThemeTextFormat.SingleLine;
            PaintTextOnGlass(g, text, font, captionRect, tf);
        }
#else
        private void PaintGlassText(Graphics g, string text, Font font, Rectangle captionRect, bool isMaximized)
        {
            if (isMaximized)
            {
                captionRect.Offset(0, 3);
                TextDrawing.DrawString(g, text, font, Color.White, captionRect, eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter | eTextFormat.EndEllipsis | eTextFormat.NoPrefix);
                return;
            }

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;
            using (GraphicsPath path = new GraphicsPath())
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                format.FormatFlags |= StringFormatFlags.NoWrap;
                const float emMulti = 1.3285f;
                path.AddString(text, font.FontFamily, (int)font.Style, font.SizeInPoints * emMulti, captionRect, format);

                using (Pen glowPen = new Pen(Color.FromArgb(215, Color.White), 4f))
                {
                    //glowPen.PenType = PenType.PathGradient;
                    GraphicsPath p1 = new GraphicsPath();
                    Rectangle rc = captionRect;
                    rc.Inflate(captionRect.Width/2, -3);
                    p1.AddEllipse(rc);
                    PathGradientBrush pb = new PathGradientBrush(p1);
                    pb.CenterColor = Color.FromArgb(164, Color.White);
                    pb.SurroundColors = new Color[] { Color.Transparent };
                    glowPen.Brush = pb;
                    g.DrawPath(glowPen, path);
                }
                //GraphicsPath glowPath = path.Clone() as GraphicsPath;
                //using (Region reg = new Region())
                //{
                //    reg.MakeEmpty();
                //    reg.Union(glowPath);
                //    using (Pen pen = new Pen(Color.Black, 5))
                //        glowPath.Widen(pen);
                //    reg.Union(glowPath);
                //    GraphicsPath p1 = new GraphicsPath();
                //    RectangleF rf = glowPath.GetBounds();
                //    p1.AddEllipse(rf.X - 12, rf.Y - 4, rf.Width + 24, rf.Height + 8);
                //    using (PathGradientBrush pb = new PathGradientBrush(p1))
                //    {
                //        pb.CenterColor = Color.FromArgb(164, Color.White);
                //        pb.SurroundColors = new Color[] { Color.Transparent };
                //        Blend blend = new Blend();
                //        blend.Factors = new float[] { 0f, .9f, 1f };
                //        blend.Positions = new float[] { .0f, .9f, 1f };
                //        pb.Blend = blend;
                //        g.FillRegion(pb, reg);
                //    }
                //    p1.Dispose();
                //    glowPath.Dispose();
                //}
                
                g.FillPath(SystemBrushes.ActiveCaptionText, path);
            }
            g.SmoothingMode = sm;
        }
#endif

        private class RectangleWidthComparer : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(object x, object y)
            {
                if (x is Rectangle && y is Rectangle)
                {
                    return ((Rectangle)x).Width - ((Rectangle)y).Width;
                }
                else
                    return ((new CaseInsensitiveComparer()).Compare(x, y));
            }
        }
        #endregion

        #region Quick Access Toolbar background
        /// <summary>
        /// Paints the background of quick access toolbar.
        /// </summary>
        public override void PaintQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
        {
            RibbonStrip rs = e.RibbonControl.RibbonStrip;
            RibbonControl ribbon = e.RibbonControl;
            if (!rs.CaptionVisible)
                return;
            eDotNetBarStyle effectiveStyle = ribbon.EffectiveStyle;
            Graphics g = e.Graphics;
            bool rightToLeft = (rs.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
            bool drawQatBackground = true;
            // Get appropriate color table
            Rendering.Office2007QuickAccessToolbarStateColorTable ct = m_ColorTable.QuickAccessToolbar.Active;
            System.Windows.Forms.Form form = rs.FindForm();
            bool isMaximized = false;
            if (form != null) isMaximized = form.WindowState == System.Windows.Forms.FormWindowState.Maximized;
            if (form != null && !e.RibbonControl.IsDesignMode && (form != System.Windows.Forms.Form.ActiveForm && form.MdiParent == null ||
                form.MdiParent != null && form.MdiParent.ActiveMdiChild != form))
                ct = m_ColorTable.QuickAccessToolbar.Inactive;

            // Get right most X position of the Quick Access Toolbar
            int right = 0, sysLeft = 0;
            Size qatSize = Size.Empty;
            for (int i = rs.QuickToolbarItems.Count - 1; i >= 0; i--)
            {
                BaseItem item = rs.QuickToolbarItems[i];
                if (!item.Visible || !item.Displayed)
                    continue;
                if (item is QatCustomizeItem) qatSize = item.DisplayRectangle.Size;
                if (item.ItemAlignment == eItemAlignment.Near && item.Visible && i>0)
                {
                    if(rightToLeft)
                        right = item.DisplayRectangle.X;
                    else
                        right = item.DisplayRectangle.Right;
                    break;
                }
                else if (item.ItemAlignment == eItemAlignment.Far && item.Visible)
                {
                    if(rightToLeft)
                        sysLeft = item.DisplayRectangle.Right;
                    else
                        sysLeft = item.DisplayRectangle.X;
                }
            }
            if (right == 0) drawQatBackground = false;
            if (rs.CaptionContainerItem is CaptionItemContainer && ((CaptionItemContainer)rs.CaptionContainerItem).MoreItems!=null)
            {
                if (rightToLeft)
                    right = ((CaptionItemContainer)rs.CaptionContainerItem).MoreItems.DisplayRectangle.X;
                else
                    right = ((CaptionItemContainer)rs.CaptionContainerItem).MoreItems.DisplayRectangle.Right;
                qatSize = ((CaptionItemContainer)rs.CaptionContainerItem).MoreItems.DisplayRectangle.Size;
            }

            Rectangle r = rs.CaptionBounds;

            if (sysLeft > 0)
            {
                if(rightToLeft)
                    rs.SystemCaptionItemBounds = new Rectangle(r.X, r.Y, sysLeft, r.Height);
                else
                    rs.SystemCaptionItemBounds = new Rectangle(sysLeft, r.Y, r.Right - sysLeft, r.Height);
            }

            if (right == 0 || r.Height <= 0 || r.Width <= 0)
                return;

            if (!ribbon.QatPositionedBelowRibbon)
            {
                BaseItem startButton = ribbon.GetApplicationButton();
                if (startButton != null)
                {
                    int startIndex = rs.QuickToolbarItems.IndexOf(startButton);
                    if (rs.QuickToolbarItems.Count - 1 > startIndex)
                    {
                        BaseItem firstItem = rs.QuickToolbarItems[startIndex + 1];
                        if (rightToLeft)
                        {
                            r.Width -= r.Right - firstItem.DisplayRectangle.Right;
                        }
                        else
                        {
                            r.Width -= firstItem.DisplayRectangle.X - r.X;
                            r.X = firstItem.DisplayRectangle.X;
                        }
                    }
                }
                r.Height = ((CaptionItemContainer)ribbon.RibbonStrip.CaptionContainerItem).MaxItemHeight + 6;
            }

            // Create QAT rectangle
            int reduction = (r.Right - right);

            if (rightToLeft)
            {
                r.Width = reduction;
                if (rightToLeft) r.X += right;
            }
            else
            {
                r.Width -= reduction;
            }

            if (e.GlassEnabled)
            {
                r.Inflate(0, -3);
                r.Offset(0, 2);
            }
            else
            {
                r.Inflate(0, -2);
                if (isMaximized)
                {
                    r.Y++;
                    r.Height--;
                }
            }

            // Draw it, fill first
            GraphicsPath path = GetQuickToolbarBackPath(r, rightToLeft);
            if (path == null) return;

            // Get total background bounds and save them
            Rectangle clip = Rectangle.Ceiling(path.GetBounds());
            rs.QuickToolbarBounds = clip;

            // Adjust the bounds of QAT so it is drawn before the QAT Customize Item
            r.Width -= (qatSize.Width + qatSize.Height / 2);
            if (rightToLeft)
                r.X += (qatSize.Width + qatSize.Height / 2) + 3;
            path.Dispose();
            path = GetQuickToolbarBackPath(r, rightToLeft);

            SmoothingMode sm = g.SmoothingMode;
            if (!ribbon.QatPositionedBelowRibbon && drawQatBackground && r.Width>6)
            {
                bool glassBorderPainted = false;
                if (e.GlassEnabled)
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    if (e.GlassEnabled && isMaximized && effectiveStyle == eDotNetBarStyle.Office2007)
                    {
                        DisplayHelp.FillPath(g, path, Color.FromArgb(72, Color.LightGray), Color.Empty);
                    }
                    if(!ct.GlassBorder.IsEmpty)
                        DisplayHelp.DrawGradientPathBorder(g, path, ct.GlassBorder, 1);
                    g.SmoothingMode = sm;
                    if (e.GlassEnabled) glassBorderPainted = true;
                }
                else
                {
                    if (!ct.OutterBorderColor.IsEmpty)
                    {
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        r.X++;
                        //r.Y++;
                        r.Y--;
                        r.Height += 2;
                        using (GraphicsPath pathOuter = GetQuickToolbarBackPath(r, rightToLeft))
                        {
                            using (Pen pen = new Pen(ct.OutterBorderColor))
                                g.DrawPath(pen, pathOuter);
                        }
                        r.X--;
                        //r.Y--;
                        r.Y++;
                        r.Height -= 2;
                        g.SmoothingMode = sm;
                        if (e.GlassEnabled) glassBorderPainted = true;
                    }
                }

                //if (!e.GlassEnabled) // Fill painted only if Glass is not enabled
                //{
                    Region oldClip = g.Clip;
                    g.SetClip(path, CombineMode.Intersect);

                    Rectangle top = new Rectangle(clip.X, clip.Y, clip.Width, (int)(r.Height * .2));
                    Rectangle bottom = new Rectangle(clip.X, top.Bottom, clip.Width, clip.Height - top.Height);

                    g.SmoothingMode = SmoothingMode.Default;
                    if (!e.GlassEnabled)
                    {
                        DisplayHelp.FillRectangle(g, top, ct.TopBackground);
                        DisplayHelp.FillRectangle(g, bottom, ct.BottomBackground);
                    }
                    else
                    {
                        if(!ct.GlassBorder.IsEmpty)
                            DisplayHelp.FillRectangle(g, clip, Color.FromArgb(64, Color.White));
                    }
                    g.SmoothingMode = sm;
                    clip.Height -= 2;
                    clip.Width += 2;
                    clip.Y++;
                    clip.X++;
                    g.SetClip(clip);

                    //if (!ct.InnerBorderColor.IsEmpty)
                    //{
                    //    r.X--;
                    //    path = GetQuickToolbarBackPath(r, rightToLeft, true);
                    //    using (Pen pen = new Pen(ct.InnerBorderColor))
                    //        g.DrawPath(pen, path);
                    //    path.Dispose();
                    //    r.X++;
                    //}

                    g.Clip = oldClip;
                    if (oldClip != null) oldClip.Dispose();
                //}

                if (!ct.MiddleBorderColor.IsEmpty && !(e.GlassEnabled && glassBorderPainted))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    path = GetQuickToolbarBackPath(r, rightToLeft);
                    using (Pen pen = new Pen(ct.MiddleBorderColor))
                        g.DrawPath(pen, path);
                    path.Dispose();
                    path = null;
                    g.SmoothingMode = sm;
                }
            }
            if (path != null) path.Dispose();
        }

        private GraphicsPath GetQuickToolbarBackPath(Rectangle r, bool rightToLeft)
        {
            r.Offset(-1, 0);
            r.Height--;
            r.Width--;
            if (r.Width < 2 || r.Height < 2) return null;
            GraphicsPath path = new GraphicsPath();
            
            int outdent = 11;
            if (rightToLeft)
            {
                path.AddCurve(new Point[] { new Point(r.Right + outdent, r.Y), new Point(r.Right + 2, r.Y + 10), new Point(r.Right, r.Bottom) }, .6f);
                path.AddLine(r.Right, r.Bottom, r.X, r.Bottom);
                path.AddArc(r.X - (outdent + 1), r.Y, 20, r.Height, 90, 180);
                path.AddLine(r.X, r.Y, r.Right + outdent, r.Y);
            }
            else
            {
                path.AddCurve(new Point[] { new Point(r.X - outdent, r.Y), new Point(r.X - 2, r.Y + 10), new Point(r.X, r.Bottom) }, .6f);
                path.AddLine(r.X, r.Bottom, r.Right, r.Bottom);
                path.AddArc(r.Right - (outdent + 1), r.Y, 20, r.Height, 90, -180);
                path.AddLine(r.Right, r.Y, r.X - outdent, r.Y);
            }
            path.CloseAllFigures();
            return path;
        }

        //private GraphicsPath GetQuickToolbarBackPath(Rectangle r, bool rightToLeft, bool arcOnly)
        //{
        //    GraphicsPath path = new GraphicsPath();
        //    if (rightToLeft)
        //    {
        //        int arcSize = (int)(r.Height * .8);
        //        Rectangle arcRect = new Rectangle(r.X - r.Height / 2, r.Y, r.Width + r.Height / 2, r.Height);
        //        path.AddLine(arcRect.X, r.Bottom, arcRect.X, arcRect.Y + arcSize);
        //        ElementStyleDisplay.AddCornerArc(path, arcRect, arcSize, eCornerArc.TopLeft);
        //        if (!arcOnly)
        //        {
        //            path.AddLine(r.X, r.Y, r.Right, r.Y);
        //            path.AddLine(r.Right, r.Y, r.Right, r.Bottom);
        //            path.CloseAllFigures();
        //        }
        //    }
        //    else
        //    {
        //        if (!arcOnly)
        //        {
        //            path.AddLine(r.X, r.Bottom, r.X, r.Y);
        //            path.AddLine(r.X, r.Y, r.Right, r.Y);
        //        }
        //        Rectangle arcRect = new Rectangle(r.X, r.Y, r.Width + r.Height / 2, r.Height);
        //        int arcSize = (int)(r.Height * .8);
        //        ElementStyleDisplay.AddCornerArc(path, arcRect, arcSize, eCornerArc.TopRight);
        //        path.AddLine(arcRect.Right, arcRect.Y + arcSize, arcRect.Right, r.Bottom);
        //        if (!arcOnly) path.CloseAllFigures();
        //    }

        //    return path;
        //}
        #endregion
    }
}
