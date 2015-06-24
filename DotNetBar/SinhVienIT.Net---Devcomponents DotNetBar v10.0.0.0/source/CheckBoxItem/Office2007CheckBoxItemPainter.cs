using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar.Rendering
{
    internal class Office2007CheckBoxItemPainter: CheckBoxItemPainter, IOffice2007Painter
    {
        #region Private Variables
        
        #endregion

        #region IOffice2007Painter
        private Office2007ColorTable m_ColorTable = null;

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
        public override void Paint(CheckBoxItemRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            CheckBoxItem item = e.CheckBoxItem;
            bool rtl = e.RightToLeft;
            Font font = e.Font;
            Office2007CheckBoxStateColorTable ct = this.GetCheckBoxStateColorTable(e);

            Rectangle checkBoxPosition = Rectangle.Empty;
            Rectangle textRect = Rectangle.Empty;
            eTextFormat tf = eTextFormat.Default;
            if (e.ItemPaintArgs != null && e.ItemPaintArgs.ContainerControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                tf |= eTextFormat.PrefixOnly;

            Size checkSignSize = item.GetCheckSignSize();

            float baselineOffsetPixels = g.DpiY / 72f * (font.SizeInPoints / font.FontFamily.GetEmHeight(font.Style) * font.FontFamily.GetCellAscent(font.Style)) + .5f;
            //float fontDescentPixels = g.DpiY / 72f * (font.SizeInPoints / font.FontFamily.GetEmHeight(font.Style) * font.FontFamily.GetCellDescent(font.Style));

            if (item.CheckBoxPosition == eCheckBoxPosition.Left && !rtl || item.CheckBoxPosition == eCheckBoxPosition.Right && rtl)
            {
                checkBoxPosition = new Rectangle(item.DisplayRectangle.X + item.CheckTextSpacing / 2,
                    item.DisplayRectangle.Y + (item.DisplayRectangle.Height - checkSignSize.Height) / 2,
                    checkSignSize.Width, checkSignSize.Height);
                textRect = new Rectangle(checkBoxPosition.Right + item.CheckTextSpacing / 2, item.DisplayRectangle.Y,
                    item.DisplayRectangle.Right - (checkBoxPosition.Right + item.CheckTextSpacing / 2), item.DisplayRectangle.Height);

                if (item.TextMarkupBody == null)
                    textRect.Y = checkBoxPosition.Bottom - (int)baselineOffsetPixels - 2;
            }
            else if (item.CheckBoxPosition == eCheckBoxPosition.Right && !rtl || item.CheckBoxPosition == eCheckBoxPosition.Left && rtl)
            {
                checkBoxPosition = new Rectangle(item.DisplayRectangle.Right - item.CheckTextSpacing / 2 - checkSignSize.Width,
                    item.DisplayRectangle.Y + (item.DisplayRectangle.Height - checkSignSize.Height) / 2,
                    checkSignSize.Width, checkSignSize.Height);
                textRect = new Rectangle(item.DisplayRectangle.X, item.DisplayRectangle.Y,
                    checkBoxPosition.X - (item.DisplayRectangle.X + item.CheckTextSpacing / 2), item.DisplayRectangle.Height);
                if (item.TextMarkupBody == null)
                    textRect.Y = checkBoxPosition.Bottom - (int)baselineOffsetPixels - 2;

                tf |= eTextFormat.Right;
            }
            else if (item.CheckBoxPosition == eCheckBoxPosition.Top)
            {
                checkBoxPosition = new Rectangle(item.DisplayRectangle.X + (item.DisplayRectangle.Width - checkSignSize.Width) / 2,
                    item.DisplayRectangle.Y + item.VerticalPadding, checkSignSize.Width, checkSignSize.Height);
                textRect = new Rectangle(item.DisplayRectangle.X, checkBoxPosition.Bottom,
                    item.DisplayRectangle.Width, item.DisplayRectangle.Bottom - checkBoxPosition.Bottom);
                tf |= eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter;
            }
            else if (item.CheckBoxPosition == eCheckBoxPosition.Bottom)
            {
                checkBoxPosition = new Rectangle(item.DisplayRectangle.X + (item.DisplayRectangle.Width - checkSignSize.Width) / 2,
                    item.DisplayRectangle.Bottom - item.VerticalPadding - checkSignSize.Height, checkSignSize.Width, checkSignSize.Height);
                textRect = new Rectangle(item.DisplayRectangle.X, item.DisplayRectangle.Y,
                    item.DisplayRectangle.Width, checkBoxPosition.Y - (item.DisplayRectangle.Y + item.VerticalPadding));

                tf |= eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter;
            }

            if (item.CheckState == CheckState.Unchecked && item.CheckBoxImageUnChecked != null)
                g.DrawImage(item.CheckBoxImageUnChecked, checkBoxPosition);
            else if (item.CheckState == CheckState.Checked && item.CheckBoxImageChecked != null)
                g.DrawImage(item.CheckBoxImageChecked, checkBoxPosition);
            else if (item.CheckState == CheckState.Indeterminate && item.CheckBoxImageIndeterminate != null)
                g.DrawImage(item.CheckBoxImageIndeterminate, checkBoxPosition);
            else
            {
                if (item.CheckBoxStyle == eCheckBoxStyle.CheckBox)
                {
                    PaintCheckBox(g, checkBoxPosition, ct, item.CheckState);
                }
                else
                {
                    PaintRadioButton(g, checkBoxPosition, ct, item.Checked);
                }
            }

            Color textColor = ct.Text;
            if (!item.TextColor.IsEmpty) textColor = item.TextColor;
            if (item.Text != "" && !textRect.IsEmpty && !textColor.IsEmpty && item.Orientation != eOrientation.Vertical && item.TextVisible)
            {
                if (item.TextMarkupBody != null)
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, textColor, rtl);
                    d.HotKeyPrefixVisible = !((tf & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                    if ((tf & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
                        textRect.Y = item.TopInternal + (item.Bounds.Height - textRect.Height) / 2;
                    else if ((tf & eTextFormat.Bottom) == eTextFormat.Bottom)
                        textRect.Y += (item.Bounds.Height - textRect.Height) + 1;

                    item.TextMarkupBody.Bounds = textRect;
                    item.TextMarkupBody.Render(d);
                }
                else
                {
#if FRAMEWORK20
                    if (e.ItemPaintArgs != null && e.ItemPaintArgs.GlassEnabled && item.Parent is CaptionItemContainer && !(e.ItemPaintArgs.ContainerControl is QatToolbar))
                    {
                        if (!e.ItemPaintArgs.CachedPaint)
                            Office2007RibbonControlPainter.PaintTextOnGlass(g, item.Text, font, textRect, TextDrawing.GetTextFormat(tf));
                    }
                    else
#endif
                        TextDrawing.DrawString(g, item.Text, font, textColor, textRect, tf);
                }
            }

            if (item.Focused && item.DesignMode)
            {
                Rectangle r = item.DisplayRectangle;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(g, r, e.ColorScheme.ItemDesignTimeBorder);
            }
        }

        public void PaintRadioButton(Graphics g, Rectangle checkBoxPosition, Office2007CheckBoxStateColorTable ct, bool isChecked)
        {
            Rectangle r = checkBoxPosition;
            r.Inflate(-1, -1);
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(r);
                DisplayHelp.FillPath(g, path, ct.CheckBackground);
            }
            using (Pen pen = new Pen(ct.CheckBorder, 1))
            {
                g.DrawEllipse(pen, checkBoxPosition);
            }
            // Draw inner border
            checkBoxPosition.Inflate(-3, -3);
            using (Pen pen = new Pen(ct.CheckInnerBorder, 1))
            {
                g.DrawEllipse(pen, checkBoxPosition);
            }

            if (!ct.CheckInnerBackground.IsEmpty)
            {
                if (ct.CheckInnerBackground.End.IsEmpty)
                {
                    r = checkBoxPosition;
                    r.Inflate(-1, -1);
                    using (SolidBrush brush = new SolidBrush(ct.CheckInnerBackground.Start))
                        g.FillEllipse(brush, r);
                }
                else
                {
                    // Draw gradient
                    Region old = g.Clip;
                    g.SetClip(checkBoxPosition, System.Drawing.Drawing2D.CombineMode.Replace);

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(checkBoxPosition);

                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = ct.CheckInnerBackground.Start;
                            brush.SurroundColors = new Color[] { ct.CheckInnerBackground.End };
                            brush.CenterPoint = new PointF(checkBoxPosition.X, checkBoxPosition.Y);
                            g.FillEllipse(brush, checkBoxPosition);
                        }
                    }
                    g.Clip = old;
                }
            }

            if (isChecked && !ct.CheckSign.IsEmpty)
            {
                r = checkBoxPosition;
                //r.Inflate(-1, -1);
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(r);
                    DisplayHelp.FillPath(g, path, ct.CheckSign);
                }
            }
        }

        public void PaintCheckBox(Graphics g, Rectangle checkBoxPosition, Office2007CheckBoxStateColorTable ct, CheckState checkState)
        {
            Rectangle r = checkBoxPosition;
            r.Inflate(-1, -1);
            if(checkBoxPosition.Width<5 || checkBoxPosition.Height<5) return;

            DisplayHelp.FillRectangle(g, r, ct.CheckBackground);
            DisplayHelp.DrawRectangle(g, ct.CheckBorder, checkBoxPosition);

            // Inside rectangle
            checkBoxPosition.Inflate(-2, -2);

            if (!ct.CheckInnerBackground.IsEmpty)
            {
                if (ct.CheckInnerBackground.End.IsEmpty)
                {
                    r = checkBoxPosition;
                    r.Inflate(-1, -1);
                    DisplayHelp.FillRectangle(g, r, ct.CheckInnerBackground.Start);
                }
                else
                {
                    // Draw gradient
                    Region old = g.Clip;
                    g.SetClip(checkBoxPosition, System.Drawing.Drawing2D.CombineMode.Intersect);

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddRectangle(checkBoxPosition);

                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = ct.CheckInnerBackground.Start;
                            brush.SurroundColors = new Color[] { ct.CheckInnerBackground.End };
                            brush.CenterPoint = new PointF(checkBoxPosition.X, checkBoxPosition.Y);
                            g.FillRectangle(brush, checkBoxPosition);
                        }
                    }
                    g.Clip = old;
                }
            }

            DisplayHelp.DrawRectangle(g, ct.CheckInnerBorder, checkBoxPosition);

            if (checkState == CheckState.Indeterminate)
            {
                checkBoxPosition.Inflate(-2, -2);
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.None;
                DisplayHelp.FillRectangle(g, checkBoxPosition, ct.CheckSign);
                g.SmoothingMode = sm;
            }
            else if (checkState == CheckState.Checked && !ct.CheckSign.IsEmpty)
            {
                using (GraphicsPath path = GetCheckSign(checkBoxPosition))
                    DisplayHelp.FillPath(g, path, ct.CheckSign);
            }
        }

        private GraphicsPath GetCheckSign(Rectangle outterRect)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = outterRect;
            r.Inflate(-1, -1);

            path.AddLine(r.X, r.Y + r.Height * .75f, r.X + r.Width * .3f, r.Bottom);
            path.AddLine(r.X + r.Width * .4f, r.Bottom, r.Right , r.Y + r.Height * .05f);
            path.AddLine(r.Right - r.Width * .3f, r.Y, r.X + r.Width * .25f, r.Y + r.Height * .75f);
            path.AddLine(r.X + r.Width * .1f, r.Y + r.Height * .5f, r.X, r.Y + r.Height * .55f);
            path.CloseAllFigures();
           
            return path;
        }

        private Office2007CheckBoxStateColorTable GetCheckBoxStateColorTable(CheckBoxItemRenderEventArgs e)
        {
            CheckBoxItem item = e.CheckBoxItem;
            if (m_ColorTable != null && BarFunctions.IsOffice2007Style(e.CheckBoxItem.EffectiveStyle))
            {
                Office2007CheckBoxColorTable ct = m_ColorTable.CheckBoxItem;
                if (!item.GetEnabled())
                    return ct.Disabled;
                else if (item.IsMouseDown)
                    return ct.Pressed;
                else if (item.IsMouseOver)
                    return ct.MouseOver;
                return ct.Default;
            }
            else
            {
                ColorScheme cs = e.ColorScheme;
                // Create color table based on the ColorScheme object...
                Office2007CheckBoxStateColorTable ct = new Office2007CheckBoxStateColorTable();
                if (!item.GetEnabled())
                {
                    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckBorder = cs.ItemDisabledText;
                    ct.CheckInnerBorder = cs.ItemDisabledText;
                    ct.CheckInnerBackground = new LinearGradientColorTable();
                    ct.CheckSign = new LinearGradientColorTable(cs.ItemDisabledText, Color.Empty);
                    ct.Text = cs.ItemDisabledText;
                }
                else if (item.IsMouseDown)
                {
                    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckBorder = cs.ItemPressedBorder;
                    ct.CheckInnerBorder = cs.ItemPressedBorder;
                    ct.CheckInnerBackground = new LinearGradientColorTable(cs.ItemPressedBackground, cs.ItemPressedBackground2);
                    ct.CheckSign = new LinearGradientColorTable(cs.ItemPressedText, Color.Empty);
                    ct.Text = cs.ItemPressedText;
                }
                else if (item.IsMouseOver)
                {
                    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckBorder = cs.ItemHotBorder;
                    ct.CheckInnerBorder = cs.ItemHotBorder;
                    ct.CheckInnerBackground = new LinearGradientColorTable(cs.ItemHotBackground, cs.ItemHotBackground2);
                    ct.CheckSign = new LinearGradientColorTable(cs.ItemHotText, Color.Empty);
                    ct.Text = cs.ItemHotText;
                }
                else
                {
                    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckBorder = cs.PanelBorder;
                    ct.CheckInnerBorder = ColorBlendFactory.SoftLight(cs.PanelBorder, Color.White);
                    ct.CheckInnerBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckSign = new LinearGradientColorTable(cs.ItemText, Color.Empty);
                    ct.Text = cs.ItemText;
                }
                return ct;
            }
        }
        #endregion
    }
}
