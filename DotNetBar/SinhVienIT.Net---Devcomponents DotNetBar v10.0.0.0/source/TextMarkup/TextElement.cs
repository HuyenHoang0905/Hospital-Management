using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class TextElement : MarkupElement
    {
        private static bool PadText = false;
        static TextElement()
        {
            string lc = System.Windows.Forms.Application.CurrentCulture.TwoLetterISOLanguageName;
            if (lc == "ja")
                PadText = true;
        }
        #region Private Variables
        private string m_Text = "";
        private bool m_TrailingSpace = false;
        private bool m_EnablePrefixHandling = true;
        #endregion

        #region Internal Implementation
        public override void Measure(System.Drawing.Size availableSize, MarkupDrawContext d)
        {
#if (FRAMEWORK20)
            if (BarUtilities.UseTextRenderer)
            {
                eTextFormat format = eTextFormat.Default | eTextFormat.NoPadding;
                if (!d.HotKeyPrefixVisible || m_EnablePrefixHandling)
                    format |= eTextFormat.HidePrefix;
                Size size = Size.Empty;
                if (m_TrailingSpace)
                {
                    if (d.CurrentFont.Italic)
                    {
                        size = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, m_Text, d.CurrentFont, 0, format));
                        size.Width += (int)(d.Graphics.MeasureString("||", d.CurrentFont).Width / 4);
                    }
                    else
                        size = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, m_Text + (BarFunctions.IsVista && m_Text.Length > 0 ? "|" : "||"), d.CurrentFont, 0, format));
                }
                else
                    size = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, m_Text, d.CurrentFont, 0, format));
                if (PadText)
                {
                    size.Width += BarUtilities.TextMarkupCultureSpecificPadding;
                    size.Height += BarUtilities.TextMarkupCultureSpecificPadding;
                }
                this.Bounds = new Rectangle(Point.Empty, size);
            }
            else
#endif
            {
                using (StringFormat format = new StringFormat(StringFormat.GenericTypographic))
                {
                    format.FormatFlags = StringFormatFlags.NoWrap;
                    if (d.HotKeyPrefixVisible || !m_EnablePrefixHandling)
                        format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                    else
                        format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;

                    if (m_TrailingSpace)
                    {
                        if (d.CurrentFont.Italic)
                        {
                            Size size = Size.Ceiling(d.Graphics.MeasureString(m_Text, d.CurrentFont, 0, format));
                            size.Width += (int)(d.Graphics.MeasureString("|", d.CurrentFont).Width / 4);
                            if (PadText)
                            {
                                size.Width += BarUtilities.TextMarkupCultureSpecificPadding;
                                size.Height += BarUtilities.TextMarkupCultureSpecificPadding;
                            }
                            this.Bounds = new Rectangle(Point.Empty, size);
                        }
                        else
                        {
                            Size size = Size.Ceiling(d.Graphics.MeasureString(m_Text + "|", d.CurrentFont, 0, format));
                            if (PadText)
                            {
                                size.Width += BarUtilities.TextMarkupCultureSpecificPadding;
                                size.Height += BarUtilities.TextMarkupCultureSpecificPadding;
                            }
                            this.Bounds = new Rectangle(Point.Empty, size);
                        }
                    }
                    else
                    {
                        Size size = Size.Ceiling(d.Graphics.MeasureString(m_Text, d.CurrentFont, 0, format));
                        if (PadText)
                        {
                            size.Width += BarUtilities.TextMarkupCultureSpecificPadding;
                            size.Height += BarUtilities.TextMarkupCultureSpecificPadding;
                        }
                        this.Bounds = new Rectangle(Point.Empty, size);
                    }
                }
            }
            IsSizeValid = true;
        }

        public override void Render(MarkupDrawContext d)
        {
            Rectangle r = this.Bounds;
            r.Offset(d.Offset);

            if (!d.ClipRectangle.IsEmpty && !r.IntersectsWith(d.ClipRectangle))
                return;

            Graphics g = d.Graphics;
            #if (FRAMEWORK20)
            if (BarUtilities.UseTextRenderer)
            {
                eTextFormat format = eTextFormat.Default | eTextFormat.NoClipping | eTextFormat.NoPadding;
                if (d.RightToLeft) format |= eTextFormat.RightToLeft;
                if (!d.HotKeyPrefixVisible)
                    format |= eTextFormat.HidePrefix;

                if (!d.ClipRectangle.IsEmpty && r.Right > d.ClipRectangle.Right)
                {
                    format|= eTextFormat.EndEllipsis;
                    r.Width -= (r.Right - d.ClipRectangle.Right);
                }
                TextDrawing.DrawString(g, m_Text, d.CurrentFont, d.CurrentForeColor, r, format);
                
            }
            else
            #endif
            {
                using (StringFormat format = new StringFormat(StringFormat.GenericTypographic))
                {
                    format.FormatFlags |= StringFormatFlags.NoWrap;
                    if (d.RightToLeft) format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    if (d.HotKeyPrefixVisible)
                        format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                    else
                        format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
                    if (!d.ClipRectangle.IsEmpty && r.Right > d.ClipRectangle.Right)
                    {
                        format.Trimming = StringTrimming.EllipsisCharacter;
                        r.Width -= (r.Right - d.ClipRectangle.Right);
                    }
                    
                    using (SolidBrush brush = new SolidBrush(d.CurrentForeColor))
                        g.DrawString(m_Text, d.CurrentFont, brush, r, format);
                }
            }

            if (d.StrikeOut == true)
            {
                // StrikeOut

                float descent = d.CurrentFont.FontFamily.GetCellDescent(d.CurrentFont.Style) *
                    d.CurrentFont.Size / d.CurrentFont.FontFamily.GetEmHeight(d.CurrentFont.Style) + 1;

                using (Pen pen = new Pen(d.CurrentForeColor, 1))
                {
                    SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.Default;

                    float y = r.Top + (r.Height + descent) / 2;

                    g.DrawLine(pen, r.X, y, r.Right - 1, y);
                    g.SmoothingMode = sm;
                }
            }

            if ((d.HyperLink && (d.HyperlinkStyle == null || d.HyperlinkStyle.UnderlineStyle != eHyperlinkUnderlineStyle.None)) || d.Underline)
            {
                // Underline Hyperlink
                float descent = d.CurrentFont.FontFamily.GetCellDescent(d.CurrentFont.Style) * d.CurrentFont.Size / d.CurrentFont.FontFamily.GetEmHeight(d.CurrentFont.Style);
                using (Pen pen = new Pen(d.CurrentForeColor, 1))
                {
                    if (d.HyperLink && d.HyperlinkStyle != null && d.HyperlinkStyle.UnderlineStyle == eHyperlinkUnderlineStyle.DashedLine)
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    descent -= 1;
                    System.Drawing.Drawing2D.SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    g.DrawLine(pen, r.X, r.Bottom - descent, r.Right - 1, r.Bottom - descent);
                    g.SmoothingMode = sm;
                }
            }

            this.RenderBounds = r;
        }

        protected override void ArrangeCore(System.Drawing.Rectangle finalRect, MarkupDrawContext d) {}

        public string Text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
                this.IsSizeValid = false;
            }
        }

        public bool TrailingSpace
        {
            get { return m_TrailingSpace; }
            set
            {
                m_TrailingSpace = value;
                this.IsSizeValid = false;
            }
        }

        public bool EnablePrefixHandling
        {
            get { return m_EnablePrefixHandling; }
            set { m_EnablePrefixHandling = value; }
        }
        #endregion
    }
}
