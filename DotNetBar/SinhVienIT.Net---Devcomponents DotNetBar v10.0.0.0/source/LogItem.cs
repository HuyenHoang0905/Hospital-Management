#if (FRAMEWORK20)
using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents an item that displays log information in list container.
    /// </summary>
    public class LogItem : BaseItem
    {
        #region Private Variables
        private const int DefaultWidth = 96;
        private const int VerticalPadding = 3;
        private const int HorizontalPadding = 7;
        private const int TextLineSpacing = 4;
        private const int MinimumHeight = 32;
        private Size _CalloutSize = new Size(10, 10);
        private int _CornerSize = 5;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the LogItem class.
        /// </summary>
        /// <param name=""></param>
        public LogItem():this("","")
        {
        }
		/// <summary>
        /// Creates new instance of LogItem and assigns the name to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public LogItem(string sItemName):this(sItemName,"") {}
		/// <summary>
        /// Creates new instance of LogItem and assigns the name and text to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		/// <param name="ItemText">item text.</param>
        public LogItem(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
            SetIsContainer(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _Image);
            }
            base.Dispose(disposing);
        }

        /// <summary>
		/// Returns copy of the item.
		/// </summary>
		public override BaseItem Copy()
		{
            LogItem copy = new LogItem(m_Name);
			this.CopyToItem(copy);
			return copy;
		}
		/// <summary>
		/// Copies the ButtonItem specific properties to new instance of the item.
		/// </summary>
		/// <param name="copy">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem instanceCopy)
        {
            LogItem copy = instanceCopy as LogItem;
            base.CopyToItem(instanceCopy);

            copy.BackColor = this.BackColor;
            copy.Created = this.Created;
            copy.HeaderTextColor = this.HeaderTextColor;
            if (this.Image != null)
                copy.Image = (Image) this.Image.Clone();
            copy.ImagePosition = this.ImagePosition;
            copy.SourceName = this.SourceName;
            copy.TextColor = this.TextColor;
        }

        public override BaseItem ItemAtLocation(int x, int y)
        {
            if (this.SubItems.Count > 0)
            {
                BaseItem item = base.ItemAtLocation(x, y);
                if (item != null) return item;
            }
            return this.Bounds.Contains(x, y) ? this : null;
        }

        protected virtual int GetTextWidth(Rectangle bounds)
        {
            int textWidth = Math.Max(0, bounds.Width - (HorizontalPadding * 2 + _CalloutSize.Width + (_Image != null ? _ImageSize.Width : 0)));
            return textWidth;
        }

        public override void RecalcSize()
        {
            // We accept the width assigned and calculate the height based on that...
            Rectangle bounds = this.DisplayRectangle;
            Size imageSize = _ImageSize;

            if (bounds.Width <= 0)
            {
                if (_Image != null)
                    bounds.Width = imageSize.Width;
                bounds.Width += DefaultWidth;
            }

            if (_Image != null && bounds.Width - (imageSize.Width + 25) <= 1)
            {
                bounds.Height = imageSize.Height;
            }
            else
            {
                int totalHeight = VerticalPadding * 2;
                Control parent = this.ContainerControl as Control;
                if (parent != null)
                {
                    Font font = parent.Font;
                    using (Graphics g = parent.CreateGraphics())
                    {
                        int textWidth = GetTextWidth(bounds);
                        Size textSize = TextDrawing.MeasureString(g, (string.IsNullOrEmpty(this.SourceName) ? "A" : this.SourceName), font);
                        totalHeight += textSize.Height + TextLineSpacing;

                        textSize = ButtonItemLayout.MeasureItemText(this, g, textWidth, font, eTextFormat.WordBreak, parent.RightToLeft == RightToLeft.Yes);
                        //textSize = TextDrawing.MeasureString(g, this.Text, font, textWidth, eTextFormat.WordBreak);

                        totalHeight += textSize.Height + TextLineSpacing;
                    }
                }
                bounds.Height = Math.Max(totalHeight, (imageSize != null ? imageSize.Height : MinimumHeight));
            }

            base.RecalcSize();

            m_Rect = bounds;
        }

        public override void Paint(ItemPaintArgs p)
        {
            Graphics g = p.Graphics;
            Rectangle bounds = this.DisplayRectangle;

            if (_Image != null)
            {
                Rectangle imageRect = GetImageRect(bounds);
                g.DrawImage(_Image, imageRect);
            }

            Rectangle contentBounds = GetContentBounds(bounds);

            // Draw shadow
            Rectangle shadowBounds = contentBounds;
            shadowBounds.Inflate(1, 0);
            shadowBounds.Offset(0, 1);
            using (GraphicsPath path = CreateCalloutPath(shadowBounds))
            {
                using (Pen pen = new Pen(Color.FromArgb(32, Color.Black)))
                    g.DrawPath(pen, path);
            }

            using (GraphicsPath path = CreateCalloutPath(contentBounds))
            {
                Brush fill = GetCalloutBrush();
                if (fill != null)
                {
                    g.FillPath(fill, path);
                    fill.Dispose();
                }
                Pen pen = GetCalloutPen();
                if (pen != null)
                {
                    g.DrawPath(pen, path);
                    pen.Dispose();
                }
            }

            Color headerColor = GetHeaderColor();
            Rectangle headerTextBounds = GetTextBounds(bounds);
            Rectangle textBounds = headerTextBounds;
            string userName = this.SourceName;
            if (!string.IsNullOrEmpty(userName))
            {
                using (Font font = new Font(p.Font, FontStyle.Bold))
                {
                    TextDrawing.DrawString(g, userName, font, headerColor, headerTextBounds, eTextFormat.Top | eTextFormat.Left);
                    textBounds.Y += font.Height + TextLineSpacing;
                    textBounds.Height -= font.Height + TextLineSpacing;
                }
            }
            if (_Created != DateTime.MinValue && _Created != DateTime.MaxValue)
            {
                TimeSpan span = DateTime.Now.Subtract(_Created);
                //Console.WriteLine("Tweet from {0} created on {1}", SourceName, _Created);
                string timeString = GetHumanTimeSpan(span);
                using (Font font = new Font(p.Font, FontStyle.Italic))
                {
                    TextDrawing.DrawString(g, timeString, font, headerColor, headerTextBounds, eTextFormat.Top | eTextFormat.Right);
                    headerTextBounds.Y += font.Height + TextLineSpacing;
                    headerTextBounds.Height -= font.Height + TextLineSpacing;
                }
            }

            string text = this.Text;
            Color textColor = GetTextColor();
            if (!string.IsNullOrEmpty(text))
            {
                Font font = p.Font;
                if (this.TextMarkupBody != null)
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, textColor, p.RightToLeft);
                    d.HotKeyPrefixVisible = true;
                    this.TextMarkupBody.Bounds = textBounds;
                    this.TextMarkupBody.Render(d);
                }
                else
                    TextDrawing.DrawString(g,
                        text,
                        font,
                        textColor,
                        textBounds,
                        eTextFormat.Top | eTextFormat.Left | eTextFormat.WordBreak | eTextFormat.WordEllipsis);
            }
        }

        private Color _TextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the control text body.
        /// </summary>
        [Category("Columns"), Description("Indicates color of control text body.")]
        public Color TextColor
        {
            get { return _TextColor; }
            set { _TextColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !_TextColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            this.TextColor = Color.Empty;
        }

        private Color GetTextColor()
        {
            if (!_TextColor.IsEmpty)
                return _TextColor;
            return Color.Black;
        }

        private Color _HeaderTextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the header text.
        /// </summary>
        [Category("Columns"), Description("Indicates color of header text.")]
        public Color HeaderTextColor
        {
            get { return _HeaderTextColor; }
            set { _HeaderTextColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHeaderTextColor()
        {
            return !_HeaderTextColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHeaderTextColor()
        {
            this.HeaderTextColor = Color.Empty;
        }

        private Color GetHeaderColor()
        {
            if (!_HeaderTextColor.IsEmpty) return _HeaderTextColor;
            return ColorScheme.GetColor(0x5D8CCB);
        }

        protected virtual Rectangle GetTextBounds(Rectangle bounds)
        {
            Rectangle textBounds = GetContentBounds(bounds);
            if (_ImagePosition == eLogItemImagePosition.Left)
                textBounds.X += _CalloutSize.Width;
            textBounds.Width -= _CalloutSize.Width;
            textBounds.Inflate(-HorizontalPadding, -VerticalPadding);
            return textBounds;
        }

        private Brush GetCalloutBrush()
        {
            return new SolidBrush(_BackColor);
            return new SolidBrush(ColorScheme.GetColor(0xE6EDF7));
            return new SolidBrush(Color.FromArgb(208, Color.White));
        }

        private Pen GetCalloutPen()
        {
            return new Pen(ColorScheme.GetColor(0x5D8CC9), 1f);
        }

        private GraphicsPath CreateCalloutPath(Rectangle contentBounds)
        {
            GraphicsPath path = new GraphicsPath();
            if (_ImagePosition == eLogItemImagePosition.Left)
            {
                if (_CornerSize == 0)
                {
                    Rectangle pathBounds = contentBounds;
                    pathBounds.Width--;
                    pathBounds.Height--;
                    int i = _CalloutSize.Height / 2;
                    path.AddLine(pathBounds.X + _CalloutSize.Width, pathBounds.Y + _CalloutSize.Height + i, pathBounds.X, pathBounds.Y + i * 2);
                    path.AddLine(pathBounds.X, pathBounds.Y + i * 2, pathBounds.X + _CalloutSize.Width, pathBounds.Y + i);
                    path.AddLine(pathBounds.X + _CalloutSize.Width, pathBounds.Y, pathBounds.Right, pathBounds.Y);
                    path.AddLine(pathBounds.Right, pathBounds.Bottom, pathBounds.X + _CalloutSize.Width, pathBounds.Bottom);
                    path.CloseAllFigures();
                }
                else
                {
                    Rectangle pathBounds = contentBounds;
                    pathBounds.Width--;
                    pathBounds.Height--;
                    int topOffset = _CornerSize;
                    int i = _CalloutSize.Height / 2;
                    path.AddLine(pathBounds.X + _CalloutSize.Width, pathBounds.Y + topOffset + _CalloutSize.Height + i, pathBounds.X, pathBounds.Y + topOffset + i * 2);
                    path.AddLine(pathBounds.X, pathBounds.Y + topOffset + i * 2, pathBounds.X + _CalloutSize.Width, pathBounds.Y + topOffset + i);
                    pathBounds.X += _CalloutSize.Width;
                    pathBounds.Width -= _CalloutSize.Width;
                    ArcData cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.TopLeft);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
                    cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.TopRight);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
                    cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.BottomRight);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
                    cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.BottomLeft);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
                    path.CloseAllFigures();
                }
            }
            else
            {
                if (_CornerSize == 0)
                {
                    Rectangle pathBounds = contentBounds;
                    pathBounds.Width--;
                    pathBounds.Height--;
                    int i = _CalloutSize.Height / 2;
                    path.AddLine(pathBounds.Right - _CalloutSize.Width, pathBounds.Y + _CalloutSize.Height + i, pathBounds.Right, pathBounds.Y + i * 2);
                    path.AddLine(pathBounds.Right, pathBounds.Y + i * 2, pathBounds.Right - _CalloutSize.Width, pathBounds.Y + i);
                    path.AddLine(pathBounds.Right - _CalloutSize.Width, pathBounds.Y, pathBounds.X, pathBounds.Y);
                    path.AddLine(pathBounds.X, pathBounds.Bottom, pathBounds.Right - _CalloutSize.Width, pathBounds.Bottom);
                    path.CloseAllFigures();
                }
                else
                {
                    Rectangle pathBounds = contentBounds;
                    pathBounds.Width--;
                    pathBounds.Height--;
                    int topOffset = _CornerSize;
                    int i = _CalloutSize.Height / 2;

                    pathBounds.Width -= _CalloutSize.Width;
                    ArcData cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.TopLeft);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
                    cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.TopRight);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);

                    pathBounds.Width += _CalloutSize.Width;
                    path.AddLine(pathBounds.Right - _CalloutSize.Width, pathBounds.Y + i, pathBounds.Right, pathBounds.Y + i * 2);
                    path.AddLine(pathBounds.Right, pathBounds.Y + i * 2, pathBounds.Right - _CalloutSize.Width, pathBounds.Y + _CalloutSize.Height + i);
                    pathBounds.Width -= _CalloutSize.Width;

                    cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.BottomRight);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
                    cornerArc = ElementStyleDisplay.GetCornerArc(pathBounds, _CornerSize, eCornerArc.BottomLeft);
                    path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
                    path.CloseAllFigures();
                }
            }

            return path;
        }

        private Rectangle GetContentBounds(Rectangle bounds)
        {
            if (_Image != null)
            {
                if (_ImagePosition == eLogItemImagePosition.Left)
                    bounds.X += _ImageSize.Width;
                bounds.Width -= _ImageSize.Width;
            }
            // Adjust for shadow
            bounds.Height--;
            bounds.Width--;
            return bounds;
        }

        private Rectangle GetImageRect(Rectangle bounds)
        {
            if (_ImagePosition == eLogItemImagePosition.Left)
                return new Rectangle(bounds.X, bounds.Y, _ImageSize.Width, _ImageSize.Height);
            return new Rectangle(bounds.Right - _ImageSize.Width, bounds.Y, _ImageSize.Width, _ImageSize.Height);
        }

        private Image _Image = null;
        private Size _ImageSize = new Size(48, 48);
        /// <summary>
        /// Gets or sets the image displayed on the log item.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Specifies image displayed on the log item.")]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image == value)
                    return;
                _Image = value;
                _ImageSize = new Size(48, 48);
                if (_Image != null)
                {
                    try
                    {
                        _ImageSize = _Image.Size;
                    }
                    catch { }
                }
                NeedRecalcSize = true;
                this.Refresh();
            }
        }

        private eLogItemImagePosition _ImagePosition = eLogItemImagePosition.Left;
        /// <summary>
        /// Gets or sets the image position in relation to text.
        /// </summary>
        [DefaultValue(eLogItemImagePosition.Left), Category("Appearance"), Description("Specifies the image position in relation to text.")]
        public eLogItemImagePosition ImagePosition
        {
            get { return _ImagePosition; }
            set
            {

                if (_ImagePosition == value)
                    return;
                _ImagePosition = value;
                NeedRecalcSize = true;
                this.Refresh();
            }
        }

        private string _SourceName = "";
        [DefaultValue(""), Category("Appearance"), Description("Specifies the log item source name")]
        public string SourceName
        {
            get { return _SourceName; }
            set
            {
                _SourceName = value;
            }
        }

        private DateTime _Created;
        public DateTime Created
        {
            get { return _Created; }
            set { _Created = value; }
        }

        private string GetHumanTimeSpan(TimeSpan span)
        {
            if (span.TotalMinutes <= 1.5)
                return "now";
            else if (span.TotalMinutes < 2)
                return "about a minute ago";
            else if (span.TotalMinutes < 60)
                return span.TotalMinutes.ToString("N0") + " minutes ago";
            else if (span.TotalHours < 2)
                return span.TotalHours.ToString("N0") + " hour ago";
            else if (span.TotalHours < 24)
                return span.TotalHours.ToString("N0") + " hours ago";
            else if (span.TotalDays < 2)
                return span.TotalDays.ToString("N0") + " day ago";
            else if (span.TotalDays < 30)
                return span.TotalDays.ToString("N0") + " days ago";
            return (span.TotalDays / 30).ToString("N0") + " months ago";
        }

        private Color _BackColor = Color.White;
        /// <summary>
        /// Gets or sets the background color of the item.
        /// </summary>
        [Category("Appearance"), Description("Indicates background color of the item.")]
        public Color BackColor
        {
            get { return _BackColor; }
            set
            {
                if (value != _BackColor)
                {
                    Color oldValue = _BackColor;
                    _BackColor = value;
                    OnBackColorChanged(oldValue, value);
                }
            }
        }
        private void OnBackColorChanged(Color oldValue, Color newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("BackColor"));
            this.Refresh();
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor()
        {
            return _BackColor != Color.White;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackColor()
        {
            this.BackColor = Color.White;
        }
        protected void SetBackColor(Color color)
        {
            _BackColor = color;
        }
        #endregion

        #region Markup Support
        /// <summary>
        /// Gets whether item supports text markup. Default is false.
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return true; }
        }

        protected override void TextMarkupLinkClick(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
            if (link != null)
                OnMarkupLinkClick(new MarkupLinkClickEventArgs(link.Name, link.HRef));
            base.TextMarkupLinkClick(sender, e);
        }

        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (this.MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }
        #endregion
    }

    public enum eLogItemImagePosition
    {
        Left,
        Right
    }
}
#endif