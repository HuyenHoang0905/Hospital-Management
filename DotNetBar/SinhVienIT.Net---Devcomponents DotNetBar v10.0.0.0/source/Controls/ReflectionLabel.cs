using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents an single line of text label control with text-markup support and built-in reflection.
    /// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.ReflectionLabelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ToolboxBitmap(typeof(LabelX), "Controls.ReflectionLabel.ico")]
    public class ReflectionLabel : ControlWithBackgroundStyle
    {
        #region Private Variables
        private TextMarkup.BodyElement _TextMarkup = null;
        private Bitmap _ReflectionBitmap = null;
        private float _ReflectionFactor = .52f;
        private Size _TextSize = Size.Empty;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override void Dispose(bool disposing)
        {
            DisposeReflectionImage();
            base.Dispose(disposing);
        }

        protected override void PaintContent(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SmoothingMode sm = g.SmoothingMode;
            if (AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            Rectangle r = GetContentRectangle();
            Point reflectionLocation = r.Location;
            ElementStyle style = GetBackgroundStyle();
            if (_TextMarkup != null)
            {
                TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, (style.TextColor.IsEmpty?this.ForeColor:style.TextColor), 
                    (this.RightToLeft == RightToLeft.Yes), Rectangle.Empty, true);
                if (!this.Enabled)
                {
                    d.IgnoreFormattingColors = true;
                    d.CurrentForeColor = SystemColors.ControlDark;
                }
                _TextMarkup.Render(d);
                reflectionLocation.Y = _TextMarkup.Bounds.Bottom;
                reflectionLocation.X = 0;
            }
            else
            {
                int totalHeight = (int)(_TextSize.Height * (1 + _ReflectionFactor));
                int y = r.Y, x = r.X;
                if (style.TextLineAlignment == eStyleTextAlignment.Center)
                {
                    y += (r.Height - totalHeight) / 2;
                }
                else if (style.TextLineAlignment == eStyleTextAlignment.Far)
                {
                    y += (r.Height - totalHeight);
                }
                TextDrawing.DrawString(g, this.Text, this.Font, (style.TextColor.IsEmpty ? this.ForeColor : style.TextColor), new Rectangle(x, y, r.Width, r.Height), GetTextFormat());
                reflectionLocation.Y = y + _TextSize.Height;
            }

            Rectangle rClip = new Rectangle(0, reflectionLocation.Y, this.Width, this.Height - reflectionLocation.Y);
            g.SetClip(rClip, CombineMode.Replace);

            int fontBaseOffset = (int)Math.Floor(Font.Size * Font.FontFamily.GetCellDescent(Font.Style) / Font.FontFamily.GetEmHeight(Font.Style));
            reflectionLocation.Y -= fontBaseOffset;

            if (_ReflectionBitmap != null && _ReflectionEnabled)
            {
                g.DrawImage(_ReflectionBitmap, reflectionLocation);
            }
            g.ResetClip();
            if (AntiAlias)
                g.SmoothingMode = sm;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            UpdateDisplay();
            base.OnTextChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            UpdateDisplay();
            base.OnFontChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            UpdateDisplay();
            base.OnForeColorChanged(e);
        }

        protected override void OnVisualPropertyChanged()
        {
            UpdateDisplay();
            base.OnVisualPropertyChanged();
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateDisplay();
            base.OnResize(e);
        }

        private void UpdateDisplay()
        {
            string text = this.Text;

            if (_TextMarkup != null)
            {
                _TextMarkup.MouseLeave(this);
                _TextMarkup.HyperLinkClick -= new EventHandler(TextMarkupLinkClicked);
                _TextMarkup = null;
            }

            if (TextMarkup.MarkupParser.IsMarkup(ref text))
                _TextMarkup = TextMarkup.MarkupParser.Parse(text);

            if (_TextMarkup != null)
            {
                _TextMarkup.HyperLinkClick += new EventHandler(TextMarkupLinkClicked);
            }

            CreateReflection();
            this.Invalidate();
        }

        private void CreateReflection()
        {
            DisposeReflectionImage();
            if (Text.Length == 0 || this.Width <= 0 || this.Height <= 0 || this.Font == null) return;

            // Create image based on the control content
            Bitmap bmp = null;
            if (_TextMarkup == null)
            {
                using(Graphics g = this.CreateGraphics())
                {
                    if (AntiAlias)
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                    }
                    Size s = TextDrawing.MeasureString(g, this.Text, this.Font);
                    bmp = new Bitmap(this.Width, s.Height, PixelFormat.Format32bppArgb);
                    _TextSize = s;
                }
                bmp.MakeTransparent();

                Color transparentColor = Color.Empty;
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    ElementStyle style = GetBackgroundStyle();
                    
                    if (AntiAlias)
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                        transparentColor = style.BackColor.IsEmpty ? this.BackColor : style.BackColor;
                        if (transparentColor.IsEmpty || transparentColor == Color.Transparent) transparentColor = Color.WhiteSmoke;
                        DisplayHelp.FillRectangle(g, new Rectangle(0, 0, bmp.Width, bmp.Height), transparentColor);
                    }

                    TextDrawing.DrawString(g, this.Text, this.Font, (style.TextColor.IsEmpty ? this.ForeColor : style.TextColor),
                        new Rectangle(0, 0, bmp.Width, bmp.Height), GetTextFormat());
                }
                if (!transparentColor.IsEmpty)
                    bmp.MakeTransparent(transparentColor);
            }
            else
            {
                ResizeMarkup();
                if (_TextMarkup.Bounds.Height > 0)
                {
                    bmp = new Bitmap(this.Width, _TextMarkup.Bounds.Height, PixelFormat.Format32bppArgb);
                    Color transparentColor = Color.Empty;
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        ElementStyle style = GetBackgroundStyle();

                        if (AntiAlias)
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                            transparentColor = style.BackColor.IsEmpty ? this.BackColor : style.BackColor;
                            if (transparentColor.IsEmpty || transparentColor == Color.Transparent) transparentColor = Color.WhiteSmoke;
                            DisplayHelp.FillRectangle(g, new Rectangle(0, 0, bmp.Width, bmp.Height), transparentColor);
                        }
                        if (_TextMarkup.Bounds.Top > 0)
                            g.TranslateTransform(0, -(_TextMarkup.Bounds.Top - GetContentRectangle().Y));
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, (style.TextColor.IsEmpty ? this.ForeColor : style.TextColor),
                        (this.RightToLeft == RightToLeft.Yes), Rectangle.Empty, true);
                        if (!this.Enabled)
                        {
                            d.IgnoreFormattingColors = true;
                            d.CurrentForeColor = SystemColors.ControlDark;
                        }
                        _TextMarkup.Render(d);
                    }
                    if (!transparentColor.IsEmpty)
                        bmp.MakeTransparent(transparentColor);
                }
            }

            if (bmp != null)
            {
                _ReflectionBitmap = ImageHelper.CreateReflectionImage(bmp);
                bmp.Dispose();
            }
        }

        private eTextFormat GetTextFormat()
        {
            ElementStyle style = GetBackgroundStyle();
            if (style.TextAlignment == eStyleTextAlignment.Center)
                return eTextFormat.HorizontalCenter;
            else if (style.TextAlignment == eStyleTextAlignment.Far)
                return eTextFormat.Right;
            return eTextFormat.Default;
        }

        private void DisposeReflectionImage()
        {
            if (_ReflectionBitmap != null)
            {
                _ReflectionBitmap.Dispose();
                _ReflectionBitmap = null;
            }
        }

        private void TextMarkupLinkClicked(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
            if (link != null)
            {
                OnMarkupLinkClick(new MarkupLinkClickEventArgs(link.Name, link.HRef));
            }
        }

        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (this.MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }

        protected virtual void ResizeMarkup()
        {
            if (_TextMarkup != null)
            {
                Rectangle r = this.GetContentRectangle();
                if (r.Width <= 0 || r.Height <= 0)
                    return;

                Graphics g = this.CreateGraphics();
                try
                {
                    if (AntiAlias)
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                    }
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, SystemColors.Control, (this.RightToLeft == RightToLeft.Yes));
                    Size measureSize = new Size(10000, r.Height);
                    _TextMarkup.Measure(measureSize, d);
                    ElementStyle style = GetBackgroundStyle();

                    int totalHeight = (int)(_TextMarkup.Bounds.Height * (1 + _ReflectionFactor));
                    if (totalHeight < r.Height)
                    {
                        if (style.TextLineAlignment == eStyleTextAlignment.Center)
                        {
                            r.Y += (r.Height - totalHeight) / 2;
                            r.Height = _TextMarkup.Bounds.Height;
                        }
                        else if (style.TextLineAlignment == eStyleTextAlignment.Far)
                        {
                            r.Y = (r.Bottom - totalHeight);
                            r.Height = _TextMarkup.Bounds.Height;
                        }
                    }

                    if (_TextMarkup.Bounds.Width < r.Width)
                    {
                        if (style.TextAlignment == eStyleTextAlignment.Center)
                        {
                            r.X += (r.Width - _TextMarkup.Bounds.Width) / 2;
                            r.Width = _TextMarkup.Bounds.Width;
                        }
                        else if (style.TextAlignment == eStyleTextAlignment.Far && this.RightToLeft == RightToLeft.No ||
                            style.TextAlignment == eStyleTextAlignment.Near && this.RightToLeft == RightToLeft.Yes)
                        {
                            r.X = (r.Right - _TextMarkup.Bounds.Width);
                            r.Width = _TextMarkup.Bounds.Width;
                        }
                    }

                    _TextMarkup.Arrange(r, d);
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            CreateReflection();
            this.Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(175, 70);
            }
        }

        private bool _ReflectionEnabled = true;
        /// <summary>
        /// Gets or sets whether reflection effect is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether reflection effect is enabled.")]
        public bool ReflectionEnabled
        {
            get { return _ReflectionEnabled; }
            set
            {
                if (_ReflectionEnabled != value)
                {
                    _ReflectionEnabled = value;
                    this.Invalidate();
                }
            }
        }
        #endregion

        #region Property hiding
        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

#if FRAMEWORK20
		[Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        [Browsable(false)]
        public new System.Windows.Forms.Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }
#endif
        [Browsable(true), DefaultValue(""), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), EditorBrowsable(EditorBrowsableState.Always), Category("Appearance"), Description("Gets or sets the text displayed on label.")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        #endregion
    }
}
