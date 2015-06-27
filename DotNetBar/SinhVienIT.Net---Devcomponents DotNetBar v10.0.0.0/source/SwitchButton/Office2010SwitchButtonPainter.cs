using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar.Rendering
{
    internal class Office2010SwitchButtonPainter : SwitchButtonPainter, IOffice2007Painter
    {
        #region Internal Implementation
        private static readonly int SwitchCornerSize = 2;
        public override void Paint(SwitchButtonRenderEventArgs e)
        {
            SwitchButtonItem switchButton = e.SwitchButtonItem;
            bool enabled = switchButton.Enabled;
            SwitchButtonColorTable colorTable = enabled ? this.ColorTable.SwitchButton.Default : this.ColorTable.SwitchButton.Disabled;
            if (colorTable == null) colorTable = new SwitchButtonColorTable();
            
            Rectangle bounds = switchButton.Bounds;
            bounds.X = bounds.Right - switchButton.Margin.Right - switchButton.ButtonWidth;
            bounds.Width = switchButton.ButtonWidth;
            bounds.Y += switchButton.Margin.Top + (bounds.Height - switchButton.Margin.Vertical - switchButton.ButtonHeight) / 2;
            bounds.Height = switchButton.ButtonHeight;
            switchButton.ButtonBounds = bounds;

            Graphics g = e.Graphics;
            if (switchButton.TextVisible && !string.IsNullOrEmpty(switchButton.Text))
            {
                Rectangle textRect = switchButton.Bounds;
                textRect.Width -= switchButton.ButtonWidth + switchButton.Margin.Right;
                textRect.Y += switchButton.TextPadding.Top;
                textRect.Height -= switchButton.TextPadding.Vertical;
                bool rtl = e.RightToLeft;
                Color textColor = (switchButton.TextColor.IsEmpty || !enabled) ? colorTable.TextColor : switchButton.TextColor;
                Font textFont = e.Font;
                eTextFormat tf = eTextFormat.Left | eTextFormat.VerticalCenter;
                if (switchButton.TextMarkupBody != null)
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, textFont, textColor, rtl);
                    d.HotKeyPrefixVisible = !((tf & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                    if ((tf & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
                        textRect.Y = switchButton.TopInternal + (switchButton.Bounds.Height - switchButton.TextMarkupBody.Bounds.Height) / 2;
                    else if ((tf & eTextFormat.Bottom) == eTextFormat.Bottom)
                        textRect.Y += (switchButton.TextMarkupBody.Bounds.Height - textRect.Height) + 1;
                    textRect.Height = switchButton.TextMarkupBody.Bounds.Height;
                    switchButton.TextMarkupBody.Bounds = textRect;
                    switchButton.TextMarkupBody.Render(d);
                }
                else
                {
#if FRAMEWORK20
                    if (e.ItemPaintArgs != null && e.ItemPaintArgs.GlassEnabled && switchButton.Parent is CaptionItemContainer && !(e.ItemPaintArgs.ContainerControl is QatToolbar))
                    {
                        if (!e.ItemPaintArgs.CachedPaint)
                            Office2007RibbonControlPainter.PaintTextOnGlass(g, switchButton.Text, textFont, textRect, TextDrawing.GetTextFormat(tf));
                    }
                    else
#endif
                        TextDrawing.DrawString(g, switchButton.Text, textFont, textColor, textRect, tf);
                }
            }


            bool switchState = switchButton.Value;
            string offText = switchButton.OffText;
            string onText = switchButton.OnText;
            Font font = (switchButton.SwitchFont == null) ? new Font(e.Font, FontStyle.Bold) : switchButton.SwitchFont;
            Color textOffColor = (switchButton.OffTextColor.IsEmpty || !enabled) ? colorTable.OffTextColor : switchButton.OffTextColor;
            Color textOnColor = (switchButton.OnTextColor.IsEmpty || !enabled) ? colorTable.OnTextColor : switchButton.OnTextColor;

            int switchWidth = switchButton.SwitchWidth;
            int switchX = Math.Min(bounds.X + switchButton.SwitchOffset, bounds.Right);
            if (switchState)
            {
                switchX = Math.Max(bounds.Right - switchWidth - switchButton.SwitchOffset, bounds.X);
            }

            Color borderColor = (switchButton.BorderColor.IsEmpty || !enabled) ? colorTable.BorderColor : switchButton.BorderColor;
            Color offBackgroundColor = (switchButton.OffBackColor.IsEmpty || !enabled) ? colorTable.OffBackColor : switchButton.OffBackColor;
            Color onBackgroundColor = (switchButton.OnBackColor.IsEmpty || !enabled) ? colorTable.OnBackColor : switchButton.OnBackColor;

            // Set clip
            Rectangle innerBoundsClip = bounds;
            innerBoundsClip.Inflate(-1, -1);
            GraphicsPath innerClipPath = DisplayHelp.GetRoundedRectanglePath(innerBoundsClip, SwitchCornerSize - 1);
            Region oldClip = g.Clip;
            g.SetClip(innerClipPath, System.Drawing.Drawing2D.CombineMode.Intersect);
            innerClipPath.Dispose();

            // Draw On Background, it is to the left of the switch
            Rectangle onBounds = new Rectangle(switchX - (bounds.Width - switchWidth), bounds.Y, bounds.Width - switchWidth + 2, bounds.Height);
            switchButton.OnPartBounds = onBounds;
            DisplayHelp.FillRoundedRectangle(g, onBounds, SwitchCornerSize - 1, onBackgroundColor);
            DisplayHelp.FillRoundedRectangle(g, onBounds, SwitchCornerSize - 1, Color.FromArgb(onBackgroundColor.GetBrightness() > .90f ? 8 : 24, Color.Black), Color.FromArgb(32, Color.White));
            // Draw On Text
            TextDrawing.DrawString(g, onText, font, textOnColor, onBounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);

            // Draw Off Background, it is on the right of the switch 
            Rectangle offBounds = new Rectangle(switchX + switchWidth - 2, bounds.Y, bounds.Width - switchWidth + 2, bounds.Height);
            switchButton.OffPartBounds = offBounds;
            DisplayHelp.FillRoundedRectangle(g, offBounds, SwitchCornerSize, offBackgroundColor);
            DisplayHelp.FillRoundedRectangle(g, offBounds, SwitchCornerSize, Color.FromArgb(offBackgroundColor.GetBrightness() > .90f ? 12 : 32, Color.Black), Color.FromArgb(64, Color.White));
            // Draw Off Text
            TextDrawing.DrawString(g, offText, font, textOffColor, offBounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);

            // Restore old clip
            g.Clip = oldClip;
            oldClip.Dispose();

            // Main control border
            DisplayHelp.DrawRoundedRectangle(g, borderColor, bounds, SwitchCornerSize);

            LinearGradientColorTable innerOverlay = new LinearGradientColorTable(Color.FromArgb(96, Color.DarkGray), Color.Transparent, 90);
            Rectangle overlayBounds = bounds;
            overlayBounds.Inflate(-SwitchCornerSize / 2, -1);
            DisplayHelp.DrawLine(g, overlayBounds.X, overlayBounds.Y, overlayBounds.Right, overlayBounds.Y, Color.FromArgb(24, Color.Black), 1);
            overlayBounds.Y++;
            DisplayHelp.DrawLine(g, overlayBounds.X, overlayBounds.Y, overlayBounds.Right, overlayBounds.Y, Color.FromArgb(8, Color.Black), 1);

            // Draw Switch on top
            Rectangle switchBounds = new Rectangle(switchX, bounds.Y, switchWidth, bounds.Height);
            switchButton.SwitchBounds = switchBounds;
            Color switchBorderColor = (switchButton.SwitchBorderColor.IsEmpty || !enabled) ? colorTable.SwitchBorderColor : switchButton.SwitchBorderColor;
            Color switchFillColor = (switchButton.SwitchBackColor.IsEmpty || !enabled) ? colorTable.SwitchBackColor : switchButton.SwitchBackColor;

            if (switchBounds.Right + 2 < bounds.Right)
            {
                DisplayHelp.DrawLine(g, switchBounds.Right, switchBounds.Y + 3, switchBounds.Right, switchBounds.Bottom - 2, Color.FromArgb(24, Color.Black), 1);
                DisplayHelp.DrawLine(g, switchBounds.Right + 1, switchBounds.Y + 3, switchBounds.Right + 1, switchBounds.Bottom - 2, Color.FromArgb(8, Color.Black), 1);
            }

            DisplayHelp.DrawRoundedRectangle(g, switchBorderColor, switchFillColor, switchBounds, SwitchCornerSize);
            DisplayHelp.FillRoundedRectangle(g, switchBounds, SwitchCornerSize, Color.FromArgb(switchFillColor.GetBrightness() > .90f ? 42 : 72, Color.Black), Color.FromArgb(64, Color.White));
            DisplayHelp.DrawLine(g, switchBounds.X + 2, switchBounds.Y + 1, switchBounds.Right - 3, switchBounds.Y + 1, Color.FromArgb(164, Color.White), 1);

            if (switchButton.IsReadOnly && switchButton.ShowReadOnlyMarker)
            {
                Color markerColor=switchButton.ReadOnlyMarkerColor;
                Rectangle marker = new Rectangle(switchBounds.X + (switchBounds.Width - 7) / 2, switchBounds.Y + (switchBounds.Height - 10) / 2, 7, 10);
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.None;
                using (SolidBrush brush = new SolidBrush(markerColor))
                {
                    g.FillRectangle(brush, new Rectangle(marker.X, marker.Y + 4, marker.Width, marker.Height - 4));
                    g.FillRectangle(Brushes.White, new Rectangle(marker.X + 3, marker.Y + 5, 1, 2));
                }
                using (Pen pen = new Pen(markerColor, 1))
                {
                    g.DrawLine(pen, marker.X + 2, marker.Y + 0, marker.X + 4, marker.Y + 0);
                    g.DrawLine(pen, marker.X + 1, marker.Y + 1, marker.X + 1, marker.Y + 3);
                    g.DrawLine(pen, marker.X + 5, marker.Y + 1, marker.X + 5, marker.Y + 3);
                }
                g.SmoothingMode = sm;
            }
        }
        #endregion

        #region IOffice2007Painter
        private Office2007ColorTable _ColorTable = null;

        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return _ColorTable; }
            set { _ColorTable = value; }
        }
        #endregion
    }
}
