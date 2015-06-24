using System;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents circular progress indicator.
    /// </summary>
    [ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class CircularProgressItem : BaseItem
    {
        #region Events

        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of circular progress indicator.
        /// </summary>
        public CircularProgressItem() : this("", "") { }
        /// <summary>
        /// Creates new instance of circular progress indicator and assigns the name to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        public CircularProgressItem(string sItemName) : this(sItemName, "") { }
        /// <summary>
        /// Creates new instance of circular progress indicator and assigns the name and text to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        /// <param name="ItemText">item text.</param>
        public CircularProgressItem(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
            _SpokeAngles = GetSpokeAngles(_SpokeCount);
        }
        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            CircularProgressItem objCopy = new CircularProgressItem(m_Name);
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the ProgressBarItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ProgressBarItem instance.</param>
        internal void InternalCopyToItem(ProgressBarItem copy)
        {
            CopyToItem(copy);
        }

        /// <summary>
        /// Copies the ProgressBarItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ProgressBarItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            CircularProgressItem objCopy = copy as CircularProgressItem;
            base.CopyToItem(objCopy);

            
        }

        protected override void Dispose(bool disposing)
        {
            Stop();
            base.Dispose(disposing);
        }
        #endregion

        #region Implementation
        public override void Paint(ItemPaintArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (_ProgressBarType == eCircularProgressType.Line)
            {
                PaintLineProgressBar(e);
            }
            else if (_ProgressBarType == eCircularProgressType.Dot)
            {
                PaintDotProgressBar(e);
            }
            else if (_ProgressBarType == eCircularProgressType.Donut)
            {
                PaintDonutProgressBar(e);
            }
            else if (_ProgressBarType == eCircularProgressType.Spoke)
            {
                PaintSpokeProgressBar(e);
            }
            else if (_ProgressBarType == eCircularProgressType.Pie)
            {
                PaintPieProgressBar(e);
            }

            PaintLabel(e);
            
            if (this.Focused && this.DesignMode)
            {
                Rectangle r = this.DisplayRectangle;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(e.Graphics, r, e.Colors.ItemDesignTimeBorder);
            }

            this.DrawInsertMarker(e.Graphics);
        }

        private void PaintLabel(ItemPaintArgs e)
        {
            if (!_TextVisible || string.IsNullOrEmpty(this.Text))
                return;
            Font font = e.Font;
            Graphics g = e.Graphics;
            Color textColor = GetTextColor(e);
            Rectangle textBounds = Rectangle.Empty;
            Rectangle bounds = m_Rect;
            Rectangle progressBounds = GetProgressBarBounds();
            eTextFormat format = eTextFormat.Default | eTextFormat.NoClipping;

            if (_TextPosition == eTextPosition.Left)
            {
                textBounds = new Rectangle(bounds.X + _TextPadding.Left, bounds.Y + _TextPadding.Top,
                    m_Rect.Width - _TextPadding.Horizontal - TextContentSpacing - progressBounds.Width,
                    m_Rect.Height - _TextPadding.Vertical);
                format |= eTextFormat.VerticalCenter;
            }
            else if (_TextPosition == eTextPosition.Right)
            {
                textBounds = new Rectangle(bounds.X + _TextPadding.Left + TextContentSpacing + progressBounds.Width, bounds.Y + _TextPadding.Top, 
                    m_Rect.Width - _TextPadding.Horizontal - TextContentSpacing - progressBounds.Width,
                    m_Rect.Height - _TextPadding.Vertical);
                format |= eTextFormat.VerticalCenter;
            }
            else if (_TextPosition == eTextPosition.Top)
            {
                textBounds = new Rectangle(bounds.X + _TextPadding.Left, bounds.Y + _TextPadding.Top,
                    m_Rect.Width - _TextPadding.Horizontal,
                    m_Rect.Height - _TextPadding.Vertical - progressBounds.Height - TextContentSpacing);
                format |= eTextFormat.HorizontalCenter;
            }
            else if (_TextPosition == eTextPosition.Bottom)
            {
                textBounds = new Rectangle(bounds.X + _TextPadding.Left, bounds.Y + _TextPadding.Top + TextContentSpacing + progressBounds.Height,
                     m_Rect.Width - _TextPadding.Horizontal,
                    m_Rect.Height - _TextPadding.Vertical - progressBounds.Height - TextContentSpacing);
                format |= eTextFormat.HorizontalCenter;
            }
            if (_TextWidth > 0)
            {
                textBounds.Width = _TextWidth;
                format |= eTextFormat.WordBreak;
            }
            //g.FillRectangle(Brushes.WhiteSmoke, textBounds);
            TextDrawing.DrawString(g, this.Text, font, textColor, textBounds, format);
        }

        private Color GetTextColor(ItemPaintArgs e)
        {
            if (!_TextColor.IsEmpty) return _TextColor;
            return LabelItem.GetTextColor(e, this.EffectiveStyle, this.GetEnabled(), _TextColor);
        }

        private bool RenderesProgressText
        {
            get
            {
                return _ProgressTextVisible && (!_IsEndlessProgressBar || !string.IsNullOrEmpty(_ProgressText));
            }
        }
        private void PaintPieProgressBar(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = GetProgressBarBounds();
            PointF centerPoint = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
            float penWidth = (float)Math.Max(1.5f, bounds.Height * .2);
            bounds.Inflate(-1, -1);
            bounds.Width--;
            bounds.Height--;

            float borderWidth = 1f;
            if (bounds.Height > 31)
                borderWidth = bounds.Height * .05f;

            if (!_IsEndlessProgressBar)
            {
                int value = GetValue();
                Rectangle pieBounds = bounds;
                pieBounds.Inflate(-(int)borderWidth, -(int)borderWidth);

                int sweepAngle = (int)(360 * ((double)value / Math.Max(1, (_Maximum - _Minimum))));
                using (SolidBrush brush = new SolidBrush(_ProgressColor))
                {
                    g.FillPie(brush, pieBounds, 270, sweepAngle);
                }
            }
            else
            {
                Rectangle pieBounds = bounds;
                pieBounds.Inflate(-(int)borderWidth, -(int)borderWidth);

                int startAngle = (int)(360 * _EndlessProgressValue / (double)_SpokeCount);
                int sweepAngle = 90;
                using (SolidBrush brush = new SolidBrush(_ProgressColor))
                {
                    g.FillPie(brush, pieBounds, startAngle, sweepAngle);
                }
            }

            Rectangle borderBounds = bounds;
            borderBounds.Width--;
            borderBounds.Height--;

            borderBounds.Offset(1, 1);
            using (Pen pen = new Pen(_PieBorderDark, borderWidth))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawEllipse(pen, borderBounds);
            }
            borderBounds.Offset(-1, -1);
            //using (Pen pen = new Pen(_PieBorderLight, borderWidth))
            //    g.DrawEllipse(pen, borderBounds);

            using (Pen pen = new Pen(_PieBorderLight, borderWidth + .5f))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawEllipse(pen, borderBounds);
                g.DrawEllipse(pen, borderBounds);
            }

            if (RenderesProgressText)
            {
                bounds.Offset(1, 0);
                PaintProgressText(g, bounds, (int)(bounds.Height * .4f), e.Font);
            }
        }

        private void PaintSpokeProgressBar(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = GetProgressBarBounds();
            // Account for border and shade
            bounds.Width -= 2;
            bounds.Height -= 2;

            PointF centerPoint = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
            float penWidth = (float)Math.Max(1.5f, bounds.Height * .2);
            //bounds.Inflate(-(int)(penWidth / 2), -(int)(penWidth / 2));
            GraphicsPath clipPath = new GraphicsPath();
            Rectangle clipPathEllipse = bounds;
            clipPathEllipse.Inflate(-(int)((float)bounds.Height / 3f), -(int)((float)bounds.Height / 3f));
            clipPath.AddEllipse(clipPathEllipse);
            Region oldClip = g.Clip;
            g.SetClip(clipPath, CombineMode.Exclude);

            if (!_IsEndlessProgressBar)
            {
                int value = GetValue();

                int sweepAngle = (int)(360 * ((double)value / Math.Max(1, (_Maximum - _Minimum))));
                using (SolidBrush brush = new SolidBrush(_ProgressColor))
                {
                    g.FillPie(brush, bounds, 270, sweepAngle);
                }
            }
            else
            {
                int startAngle = (int)(360 * _EndlessProgressValue / (double)_SpokeCount);
                int sweepAngle = 90;
                using (SolidBrush brush = new SolidBrush(_ProgressColor))
                {
                    g.FillPie(brush, bounds, startAngle, sweepAngle);
                }
            }

            int radius = bounds.Width / 2;

            PointF shadeCenterPoint = centerPoint;
            float shadeSpokeWidth = 1f;
            float circleWidth = 1f;
            float shadeCircleWidth = 1f;
            //if (bounds.Height < 28)
            //{
            //    shadeSpokeWidth = 1f;
            //    circleWidth = 1f;
            //    shadeCircleWidth = 1f;
            //}
            radius--;
            using (Pen pen = new Pen(_SpokeBorderDark, shadeSpokeWidth))
            {
                pen.Alignment = PenAlignment.Right;
                PointF p1 = GetCoordinate(centerPoint, radius, 315); p1.X++;
                PointF p2 = GetCoordinate(shadeCenterPoint, radius, 135); p2.X++;
                g.DrawLine(pen, p1, p2); g.DrawLine(pen, p1, p2);
                p1 = GetCoordinate(centerPoint, radius, 270); p1.X++;
                p2 = GetCoordinate(shadeCenterPoint, radius, 90); p2.X++;
                g.DrawLine(pen, p1, p2);
                p1 = GetCoordinate(centerPoint, radius, 225); p1.Y++;
                p2 = GetCoordinate(shadeCenterPoint, radius, 45); p2.Y++;
                g.DrawLine(pen, p1, p2);
                p1 = GetCoordinate(centerPoint, radius, 180); p1.Y++;
                p2 = GetCoordinate(shadeCenterPoint, radius, 0); p2.Y++;
                g.DrawLine(pen, p1, p2);
            }

            using (Pen pen = new Pen(_SpokeBorderDark, shadeCircleWidth))
            {
                pen.Alignment = PenAlignment.Inset;
                Rectangle shadeBounds = bounds;
                shadeBounds.Offset(1, 1);
                //shadeBounds.Width--;
                //shadeBounds.Height--;
                g.DrawEllipse(pen, shadeBounds);
            }

            using (Pen pen = new Pen(_SpokeBorderLight, shadeSpokeWidth))
            {
                g.DrawLine(pen, GetCoordinate(centerPoint, radius, 315), GetCoordinate(centerPoint, radius, 135));
                g.DrawLine(pen, GetCoordinate(centerPoint, radius, 270), GetCoordinate(centerPoint, radius, 90));
                g.DrawLine(pen, GetCoordinate(centerPoint, radius, 225), GetCoordinate(centerPoint, radius, 45));
                g.DrawLine(pen, GetCoordinate(centerPoint, radius, 180), GetCoordinate(centerPoint, radius, 0));
            }

            using (Pen pen = new Pen(_SpokeBorderLight, circleWidth))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawEllipse(pen, bounds);
                g.DrawEllipse(pen, bounds);
            }
            g.Clip = oldClip;
            oldClip.Dispose();

            float innerCircleWidth = 1f;
            using (Pen pen = new Pen(Color.White, innerCircleWidth))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawEllipse(pen, clipPathEllipse);
                g.DrawEllipse(pen, clipPathEllipse);
            }

            if (RenderesProgressText)
            {
                //bounds.Y--;
                PaintProgressText(g, bounds, (int)(bounds.Height * .35f), e.Font);
            }
        }

        private int GetValue()
        {
            int value = Math.Min(_Maximum, Math.Max(_Minimum, _Value));
            if (this.DesignMode && value == _Minimum) value = (int)(_Maximum * .75d);
            return value;
        }

        private void PaintDonutProgressBar(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;
            RectangleF bounds = GetProgressBarBounds();
            PointF centerPoint = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
            float penWidth = (float)Math.Max(1.5f, bounds.Height * .2);
            bounds.Inflate(-Math.Max(1, penWidth / 2), -Math.Max(1, penWidth / 2));
            bounds.Width--;
            bounds.Height--;
            if (!_IsEndlessProgressBar)
            {
                int value = GetValue();

                int sweepAngle = (int)(360 * ((double)value / Math.Max(1, (_Maximum - _Minimum))));
                using (Pen pen = new Pen(_ProgressColor, penWidth))
                {

                    g.DrawArc(pen, bounds, 270, sweepAngle);
                }
            }
            else
            {
                int startAngle = (int)(360 * _EndlessProgressValue / (double)_SpokeCount);
                int sweepAngle = 140;
                using (Pen pen = new Pen(_ProgressColor, penWidth))
                {
                    pen.EndCap = LineCap.Round;
                    pen.StartCap = LineCap.Round;
                    g.DrawArc(pen, bounds, startAngle, sweepAngle);
                }
            }

            if (RenderesProgressText)
            {
                PaintProgressText(g, bounds, (int)(bounds.Height * .4f), e.Font);
            }
        }

        private void PaintDotProgressBar(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = GetProgressBarBounds();
            PointF centerPoint = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

            int outerRadius = 14;
            int circleSize = 2;

            outerRadius = (int)Math.Round(bounds.Width * .40d);
            circleSize = Math.Max(1, (int)Math.Round(outerRadius * .25d));

            int value = GetValue();

            if (!_IsEndlessProgressBar)
            {
                int spoke = (int)Math.Round(_SpokeCount * ((double)value / Math.Max(1, (_Maximum - _Minimum))));
                for (int i = 0; i < spoke; i++)
                {
                    PointF endPoint = GetCoordinate(centerPoint, outerRadius, _SpokeAngles[i]);
                    RectangleF circleBounds = new RectangleF(endPoint, Size.Empty);
                    circleBounds.Inflate(circleSize, circleSize);
                    using (SolidBrush brush = new SolidBrush(ColorFromSpokeIndex(i)))
                    {
                        g.FillEllipse(brush, circleBounds);
                    }
                }
            }
            else if (_IsRunning) // Endless Progress Bar
            {
                int position = _EndlessProgressValue;
                for (int i = 0; i < _SpokeCount; i++)
                {
                    position = position % _SpokeCount;

                    PointF endPoint = GetCoordinate(centerPoint, outerRadius, _SpokeAngles[position]);
                    RectangleF circleBounds = new RectangleF(endPoint, Size.Empty);
                    circleBounds.Inflate(circleSize, circleSize);
                    using (SolidBrush brush = new SolidBrush(ColorFromSpokeIndex(i)))
                    {
                        g.FillEllipse(brush, circleBounds);
                    }

                    position++;

                }
            }

            if (RenderesProgressText)
            {
                PaintProgressText(g, bounds, (int)(bounds.Height * .35f), e.Font);
            }
        }

        private int _SpokeCount = 12;
        private int _EndlessProgressValue = 0;
        private void PaintLineProgressBar(ItemPaintArgs e)
        {
            //e.Graphics.DrawRectangle(Pens.Green, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            Graphics g = e.Graphics;
            Rectangle bounds = GetProgressBarBounds();
            PointF centerPoint = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

            int innerRadius = 6;
            int outerRadius = 14;
            int spokeSize = 2;

            outerRadius = (int)Math.Round(bounds.Width * .45d);
            innerRadius = (int)Math.Round(outerRadius * .45d);
            spokeSize = Math.Max(2, (int)Math.Round(outerRadius * .15d));

            int value = GetValue();

            if (!_IsEndlessProgressBar)
            {
                int spoke = (int)Math.Round(_SpokeCount * ((double)value / Math.Max(1, (_Maximum - _Minimum))));
                for (int i = 0; i < spoke; i++)
                {
                    PointF startPoint = GetCoordinate(centerPoint, innerRadius, _SpokeAngles[i]);
                    PointF endPoint = GetCoordinate(centerPoint, outerRadius, _SpokeAngles[i]);
                    using (Pen pen = new Pen(ColorFromSpokeIndex(i), spokeSize))
                    {
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLine(pen, startPoint, endPoint);
                    }
                }
            }
            else if (_IsRunning) // Endless Progress Bar
            {
                int position = _EndlessProgressValue;
                for (int i = 0; i < _SpokeCount; i++)
                {
                    position = position % _SpokeCount;

                    PointF startPoint = GetCoordinate(centerPoint, innerRadius, _SpokeAngles[position]);
                    PointF endPoint = GetCoordinate(centerPoint, outerRadius, _SpokeAngles[position]);
                    using (Pen pen = new Pen(ColorFromSpokeIndex(i), spokeSize))
                    {
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLine(pen, startPoint, endPoint);
                    }

                    position++;

                }
            }

            if (RenderesProgressText)
            {
                PaintProgressText(g, bounds, innerRadius, e.Font);
            }

        }

        private string GetProgressValueText()
        {
            try
            {
                return string.Format(_ProgressTextFormat, _Value);
            }
            catch
            {
                return "Format Error";
            }
        }
        private void PaintProgressText(Graphics g, RectangleF bounds, int innerRadius, Font baseFont)
        {
            //StringFormat format = (StringFormat)StringFormat.GenericDefault.Clone();
            //format.Alignment = StringAlignment.Center;
            //format.LineAlignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.NoWrap;
            //format.Trimming = StringTrimming.None;
            eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.NoPadding | eTextFormat.SingleLine | eTextFormat.NoClipping;
            float fontSize = Math.Max(1f, innerRadius / 1.8f);
            Color textColor = _ProgressTextColor.IsEmpty ? _ProgressColor : _ProgressTextColor;

            using (Font font = new Font(baseFont.FontFamily, fontSize, FontStyle.Regular))
            {
                TextDrawing.DrawString(g, GetProgressValueText(), font, textColor, Rectangle.Round(bounds), format);
                //using (SolidBrush brush = new SolidBrush(textColor))
                //    g.DrawString(string.Format("{0}%", _Value), font, brush, bounds, format);
            }
        }
        private Color ColorFromSpokeIndex(int spokeIndex, int spokeCount)
        {
            return Color.FromArgb((int)(210 * (double)spokeIndex / spokeCount) + 45, _ProgressColor);
        }
        private Color ColorFromSpokeIndex(int spokeIndex)
        {
            return ColorFromSpokeIndex(spokeIndex, _SpokeCount);
        }

        private PointF GetCoordinate(PointF centerPoint, int radius, double angle)
        {
            double dblAngle = Math.PI * angle / 180;

            return new PointF(centerPoint.X + radius * (float)Math.Cos(dblAngle),
                              centerPoint.Y + radius * (float)Math.Sin(dblAngle));
        }

        internal static readonly int TextContentSpacing = 3;
        private Size _TextSize = Size.Empty;
        public override void RecalcSize()
        {
            Rectangle r = new Rectangle(m_Rect.X, m_Rect.Y, _Diameter, _Diameter);
            if (_TextVisible && !string.IsNullOrEmpty(this.Text))
            {
                Control parent = this.ContainerControl as Control;
                if (parent != null)
                {
                    Font font = parent.Font;
                    using (Graphics g = parent.CreateGraphics())
                    {
                        Size textSize = ButtonItemLayout.MeasureItemText(this, g, _TextWidth, font, (_TextWidth > 0 ? eTextFormat.WordBreak : eTextFormat.SingleLine), parent.RightToLeft == RightToLeft.Yes);
                        _TextSize = textSize;
                        textSize.Width += _TextPadding.Horizontal;
                        textSize.Height += _TextPadding.Vertical;
                        if (_TextPosition == eTextPosition.Left || _TextPosition == eTextPosition.Right)
                        {
                            textSize.Width += TextContentSpacing;
                            r.Width += textSize.Width;
                            r.Height = Math.Max(r.Height, textSize.Height);
                        }
                        else
                        {
                            textSize.Height += TextContentSpacing;
                            r.Height += textSize.Height;
                            r.Width = Math.Max(r.Width, textSize.Width);
                        }

                    }
                }
            }

            m_Rect = r;
            base.RecalcSize();
        }

        private Rectangle _ProgressBarBounds = Rectangle.Empty;
        private Rectangle GetProgressBarBounds()
        {
            if (string.IsNullOrEmpty(Text) || !_TextVisible)
            {
                return new Rectangle(m_Rect.X + (m_Rect.Width - _Diameter) / 2, m_Rect.Y + (m_Rect.Height - _Diameter) / 2, _Diameter, _Diameter);
            }

            if (_TextPosition == eTextPosition.Top)
            {
                return new Rectangle(m_Rect.X + (m_Rect.Width - _Diameter) / 2, m_Rect.Y + (m_Rect.Height - _Diameter), _Diameter, _Diameter);
            }
            else if (_TextPosition == eTextPosition.Right)
            {
                return new Rectangle(m_Rect.X, m_Rect.Y + (m_Rect.Height - _Diameter) / 2, _Diameter, _Diameter);
            }
            else if (_TextPosition == eTextPosition.Left)
            {
                return new Rectangle(m_Rect.Right - _Diameter, 
                    m_Rect.Y + (m_Rect.Height - _Diameter) / 2, 
                    _Diameter, _Diameter);
            }
            else if (_TextPosition == eTextPosition.Bottom)
            {
                return new Rectangle(m_Rect.X + (m_Rect.Width - _Diameter) / 2, m_Rect.Y, _Diameter, _Diameter);
            }
            return new Rectangle(m_Rect.X, m_Rect.Y, _Diameter, _Diameter);
        }

        private double[] _SpokeAngles = null;
        private double[] GetSpokeAngles(int numberOfSpokes)
        {
            double[] angles = new double[numberOfSpokes];
            double angleStep = 360d / numberOfSpokes;

            for (int i = 0; i < numberOfSpokes; i++)
                angles[i] = (i == 0 ? 270 + angleStep : angles[i - 1] + angleStep);

            return angles;
        }

        private eCircularProgressType _ProgressBarType = eCircularProgressType.Line;
        /// <summary>
        /// Gets or sets the circular progress bar type.
        /// </summary>
        [DefaultValue(eCircularProgressType.Line), Category("Appearance"), Description("Indicates circular progress bar type.")]
        public eCircularProgressType ProgressBarType
        {
            get { return _ProgressBarType; }
            set
            {
                if (value != _ProgressBarType)
                {
                    eCircularProgressType oldValue = _ProgressBarType;
                    _ProgressBarType = value;
                    OnProgressBarTypeChanged(oldValue, value);
                }
            }
        }

        private void OnProgressBarTypeChanged(eCircularProgressType oldValue, eCircularProgressType newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarType"));
            this.Refresh();
        }

        private int _Maximum = 100;
        /// <summary>
        /// Gets or sets the maximum value of the progress bar.
        /// </summary>
        [Description("Indicates maximum value of the progress bar."), Category("Behavior"), DefaultValue(100)]
        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                if (value != _Maximum)
                {
                    int oldValue = _Maximum;
                    _Maximum = value;
                    OnMaximumChanged(oldValue, value);
                }
            }
        }
        private void OnMaximumChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Maximum"));
            CoerceValue();
        }

        private int _Minimum = 0;
        /// <summary>
        /// Gets or sets the minimum value of the progress bar.
        /// </summary>
        [Description("Indicates minimum value of the progress bar."), Category("Behavior"), DefaultValue(0)]
        public int Minimum
        {
            get { return _Minimum; }
            set
            {
                if (value != _Minimum)
                {
                    int oldValue = _Minimum;
                    _Minimum = value;
                    OnMinimumChanged(oldValue, value);
                }
            }
        }
        private void OnMinimumChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Minimum"));
            CoerceValue();
        }

        private void CoerceValue()
        {
            int newValue = _Value;
            if (_Value < _Minimum)
                newValue = _Minimum;
            else if (_Value > _Maximum)
                newValue = _Maximum;
            Value = newValue;
        }

        private Color _ProgressTextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the progress percentage text.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of progress percentage text")]
        public Color ProgressTextColor
        {
            get { return _ProgressTextColor; }
            set { _ProgressTextColor = value; this.Refresh(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeProgressTextColor()
        {
            return !_ProgressTextColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetProgressTextColor()
        {
            this.ProgressTextColor = Color.Empty;
        }

        private bool _ProgressTextVisible = false;
        /// <summary>
        /// Gets or sets whether text that displays the progress bar completion percentage text is visible. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Appearance"), Description("Indicates whether text that displays the progress bar completion percentage text is visible")]
        public bool ProgressTextVisible
        {
            get { return _ProgressTextVisible; }
            set
            {
                if (value != _ProgressTextVisible)
                {
                    bool oldValue = _ProgressTextVisible;
                    _ProgressTextVisible = value;
                    OnProgressTextVisibleChanged(oldValue, value);
                }
            }
        }

        private void OnProgressTextVisibleChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ProgressTextVisible"));
            this.Refresh();
        }

        private string _ProgressText = "";
        /// <summary>
        /// Gets or sets the text displayed on top of the circular progress bar.
        /// </summary>
        [DefaultValue(""), Category("Appearance"), Description("Indicates text displayed on top of the circular progress bar.")]
        public string ProgressText
        {
            get { return _ProgressText; }
            set
            {
                if (value != _ProgressText)
                {
                    string oldValue = _ProgressText;
                    _ProgressText = value;
                    OnProgressTextChanged(oldValue, value);
                }
            }
        }

        private void OnProgressTextChanged(string oldValue, string newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("ProgressText"));
            Refresh();
        }

        private int _Value;
        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        [Description("Indicates current value of the progress bar."), Category("Behavior"), DefaultValue(0)]
        public int Value
        {
            get { return _Value; }
            set
            {
                value = Math.Min(_Maximum, Math.Max(value, _Minimum));

                if (value != _Value)
                {
                    int oldValue = _Value;
                    _Value = value;
                    OnValueChanged(oldValue, value);
                }
            }
        }

        private void OnValueChanged(int oldValue, int newValue)
        {
            if (!_IsEndlessProgressBar)
                this.Refresh();
            OnValueChanged(EventArgs.Empty);
            OnPropertyChanged(new PropertyChangedEventArgs("Value"));
        }

        /// <summary>
        /// Occurs when Value property has changed.
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Raises ValueChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Called when property on CircularProgressBar changes.
        /// </summary>
        /// <param name="propertyChangedEventArgs">Property Change Arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
        }

        private void MoveEndlessProgressBar()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    MoveEndlessProgressBar();
                }));
                return;
            }

            _EndlessProgressValue = ++_EndlessProgressValue % _SpokeCount;
            this.Refresh();
            Control container = this.ContainerControl as Control;
            if (container != null)
                container.Update();
        }

        private bool _IsEndlessProgressBar = false;
        private BackgroundWorker _LoopWorker = null;
        /// <summary>
        /// Starts the progress bar loop for endless type progress bar. Progress bar will continue to run until Stop() method is called.
        /// </summary>
        public void Start()
        {
            if (_IsRunning) return;

            _IsEndlessProgressBar = true;
            _IsRunning = true;
            _LoopWorker = new BackgroundWorker();
            _LoopWorker.WorkerSupportsCancellation = true;
            _LoopWorker.DoWork += LoopWorkerDoWork;
            _LoopWorker.RunWorkerAsync();
        }

        void LoopWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                MoveEndlessProgressBar();
                Thread.Sleep(_AnimationSpeed);
            }
        }

        /// <summary>
        /// Stops the progress bar loop for endless type progress bar.
        /// </summary>
        public void Stop()
        {
            if (!_IsRunning) return;

            _IsEndlessProgressBar = false;

            BackgroundWorker worker = _LoopWorker;
            _LoopWorker = null;
            _IsRunning = false;

            worker.CancelAsync();
            worker.DoWork -= LoopWorkerDoWork;
            worker.Dispose();
            this.Refresh();
        }

        private bool _IsRunning = false;
        /// <summary>
        /// Gets or sets whether endless type progress bar is running.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool IsRunning
        {
            get { return _IsRunning; }
            set
            {
                if (_IsRunning != value)
                {
                    if (value)
                        Start();
                    else
                        Stop();
                }
            }
        }

        private static readonly Color DefaultProgressColor = Color.DarkSlateGray;//Color.FromArgb(143, 223, 95);
        private Color _ProgressColor = DefaultProgressColor;
        /// <summary>
        /// Gets or sets the color of the color of progress indicator.
        /// </summary>
        [Category("Columns"), Description("Indicates color of progress indicator.")]
        public Color ProgressColor
        {
            get { return _ProgressColor; }
            set { _ProgressColor = value; this.Refresh(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeProgressColor()
        {
            return _ProgressColor != DefaultProgressColor;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetProgressColor()
        {
            this.ProgressColor = DefaultProgressColor;
        }

        private int _Diameter = 24;
        /// <summary>
        /// Gets or sets circular progress indicator diameter in pixels.
        /// </summary>
        [DefaultValue(24), Category("Appearance"), Description("Indicates circular progress indicator diameter in pixels.")]
        public int Diameter
        {
            get { return _Diameter; }
            set
            {
                if (value != _Diameter)
                {
                    int oldValue = _Diameter;
                    _Diameter = value;
                    OnDiameterChanged(oldValue, value);
                }
            }
        }
        private void OnDiameterChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Diameter"));
            NeedRecalcSize = true;
            this.Refresh();
        }

        private eTextPosition _TextPosition = eTextPosition.Left;
        /// <summary>
        /// Gets or sets the text position in relation to the circular progress indicator.
        /// </summary>
        [DefaultValue(eTextPosition.Left), Category("Appearance"), Description("Indicatesd text position in relation to the circular progress indicator.")]
        public eTextPosition TextPosition
        {
            get { return _TextPosition; }
            set
            {
                if (value != _TextPosition)
                {
                    eTextPosition oldValue = _TextPosition;
                    _TextPosition = value;
                    OnTextPositionChanged(oldValue, value);
                }
            }
        }
        private void OnTextPositionChanged(eTextPosition oldValue, eTextPosition newValue)
        {
            NeedRecalcSize = true;
            this.Refresh();
            OnPropertyChanged(new PropertyChangedEventArgs("TextPosition"));
        }

        private bool _TextVisible = true;
        /// <summary>
        /// Gets or sets whether text/label displayed next to the item is visible. 
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether caption/label set using Text property is visible.")]
        public bool TextVisible
        {
            get { return _TextVisible; }
            set
            {
                if (value != _TextVisible)
                {
                    bool oldValue = _TextVisible;
                    _TextVisible = value;
                    OnTextVisibleChanged(oldValue, value);
                }
            }
        }

        private void OnTextVisibleChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("TextVisible"));
            NeedRecalcSize = true;
            this.Refresh();
        }

        private int _TextWidth = 0;
        /// <summary>
        /// Gets or sets the suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.
        /// </summary>
        [DefaultValue(0), Category("Appearance"), Description("Indicates suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.")]
        public int TextWidth
        {
            get { return _TextWidth; }
            set
            {
                if (value != _TextWidth)
                {
                    int oldValue = _TextWidth;
                    _TextWidth = value;
                    OnTextWidthChanged(oldValue, value);
                }
            }
        }
        private void OnTextWidthChanged(int oldValue, int newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("TextWidth"));
            NeedRecalcSize = true;
            Refresh();
        }

        private Padding _TextPadding = new Padding(0, 0, 0, 0);
        /// <summary>
        /// Gets or sets text padding.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Gets or sets text padding."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Padding TextPadding
        {
            get { return _TextPadding; }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextPadding()
        {
            return _TextPadding.Bottom != 0 || _TextPadding.Top != 0 || _TextPadding.Left != 0 || _TextPadding.Right != 0;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextPadding()
        {
            _TextPadding = new Padding(0, 0, 0, 0);
        }
        private void TextPaddingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedRecalcSize = true;
            this.Refresh();
        }

        private Color _TextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the text label.
        /// </summary>
        [Category("Columns"), Description("Indicates color of text label.")]
        public Color TextColor
        {
            get { return _TextColor; }
            set { _TextColor = value; this.Refresh(); }
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

        private static readonly Color DefaultPieBorderDarkColor = Color.FromArgb(50, Color.Black);
        private Color _PieBorderDark = DefaultPieBorderDarkColor;
        /// <summary>
        /// Gets or sets the color of the pie progress bar dark border. 
        /// </summary>
        [Category("Pie"), Description("Indicates color of pie progress bar dark border.")]
        public Color PieBorderDark
        {
            get { return _PieBorderDark; }
            set { _PieBorderDark = value; this.Refresh(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePieBorderDark()
        {
            return _PieBorderDark != DefaultPieBorderDarkColor;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPieBorderDark()
        {
            this.PieBorderDark = DefaultPieBorderDarkColor;
        }

        private static readonly Color DefaultPieBorderLightColor = Color.FromArgb(255, Color.White);
        private Color _PieBorderLight = DefaultPieBorderLightColor;
        /// <summary>
        /// Gets or sets the color of the pie progress bar light border. 
        /// </summary>
        [Category("Pie"), Description("Indicates color of pie progress bar light border. ")]
        public Color PieBorderLight
        {
            get { return _PieBorderLight; }
            set { _PieBorderLight = value; this.Refresh(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePieBorderLight()
        {
            return _PieBorderLight != DefaultPieBorderLightColor;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPieBorderLight()
        {
            this.PieBorderLight = DefaultPieBorderLightColor;
        }

        private static readonly Color DefaultSpokeBorderDarkColor = Color.FromArgb(48, Color.Black);
        private Color _SpokeBorderDark = DefaultSpokeBorderDarkColor;
        /// <summary>
        /// Gets or sets the color of the spoke progress bar dark border.
        /// </summary>
        [Category("Spoke"), Description("Indicates color of spoke progress bar dark border.")]
        public Color SpokeBorderDark
        {
            get { return _SpokeBorderDark; }
            set { _SpokeBorderDark = value; this.Refresh(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSpokeBorderDark()
        {
            return _SpokeBorderDark != DefaultSpokeBorderDarkColor;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSpokeBorderDark()
        {
            this.SpokeBorderDark = DefaultSpokeBorderDarkColor;
        }

        private static readonly Color DefaultSpokeBorderLightColor = Color.FromArgb(255, Color.White);
        private Color _SpokeBorderLight = DefaultSpokeBorderLightColor;
        /// <summary>
        /// Gets or sets the color of the spoke progress bar light border.
        /// </summary>
        [Category("Spoke"), Description("Indicates color of spoke progress bar light border..")]
        public Color SpokeBorderLight
        {
            get { return _SpokeBorderLight; }
            set { _SpokeBorderLight = value; this.Refresh(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSpokeBorderLight()
        {
            return _SpokeBorderLight != DefaultSpokeBorderLightColor;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSpokeBorderLight()
        {
            this.SpokeBorderLight = DefaultSpokeBorderLightColor;
        }

        private string _ProgressTextFormat = "{0}%";
        /// <summary>
        /// Gets or sets format string for progress value.
        /// </summary>
        [DefaultValue("{0}%"), Category("Appearance"), Description("Indicates format string for progress value.")]
        public string ProgressTextFormat
        {
            get { return _ProgressTextFormat; }
            set
            {
                if (value != _ProgressTextFormat)
                {
                    string oldValue = _ProgressTextFormat;
                    _ProgressTextFormat = value;
                    OnProgressTextFormatChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when ProgressTextFormat property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnProgressTextFormatChanged(string oldValue, string newValue)
        {
            this.Refresh();
            OnPropertyChanged(new PropertyChangedEventArgs("ProgressTextFormat"));
        }

        private int _AnimationSpeed = 100;
        /// <summary>
        /// Gets or sets the animation speed for endless running progress. Lower number means faster running.
        /// </summary>
        [DefaultValue(100), Description("Indicates the animation speed for endless running progress. Lower number means faster running."), Category("Behavior")]
        public int AnimationSpeed
        {
            get { return _AnimationSpeed; }
            set 
            {
                if (value < 1)
                    value = 1;
                _AnimationSpeed = value; 
            }
        }
        #endregion
    }

    /// <summary>
    /// Defines available circular progress bar types.
    /// </summary>
    public enum eCircularProgressType
    {
        /// <summary>
        /// Line spokes progress bar.
        /// </summary>
        Line,
        /// <summary>
        /// Dot type/FireFox progress bar.
        /// </summary>
        Dot,
        /// <summary>
        /// Donut type progress bar.
        /// </summary>
        Donut,
        /// <summary>
        /// Spoke type progress bar.
        /// </summary>
        Spoke,
        /// <summary>
        /// Pie type progress bar.
        /// </summary>
        Pie
    }
}
