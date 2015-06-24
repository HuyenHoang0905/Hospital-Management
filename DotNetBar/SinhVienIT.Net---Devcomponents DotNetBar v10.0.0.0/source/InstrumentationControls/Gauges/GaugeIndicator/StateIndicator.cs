using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    public class StateIndicator : GaugeIndicator
    {
        #region Private variables

        private StateIndicatorStyle _Style;
        private StateRangeCollection _Ranges;

        private Image _Image;
        private float _Angle;
        private float _RoundRectangleArc;

        private Color _TextColor;
        private TextAlignment _TextAlignment;
        private float _TextVerticalOffset;
        private float _TextHorizontalOffset;

        private Font _AbsFont;

        #endregion

        public StateIndicator()
        {
            InitIndicator();
        }

        #region InitIndicator

        private void InitIndicator()
        {
            _Style = StateIndicatorStyle.Circular;

            _RoundRectangleArc = .75f;

            _TextColor = Color.Black;
            _TextAlignment = TextAlignment.MiddleCenter;

            BackColor.Color1 = Color.PaleGreen;
            BackColor.Color2 = Color.DarkGreen;
        }

        #endregion

        #region Public properties

        #region Angle

        /// <summary>
        /// Gets or sets the amount to rotate the indicator, specified in degrees.
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Determines the amount to rotate the indicator, specified in degrees.")]
        public float Angle
        {
            get { return (_Angle); }

            set
            {
                if (_Angle != value)
                {
                    _Angle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets or sets the Image to use
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Image to use.")]
        public Image Image
        {
            get { return (_Image); }

            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Ranges

        /// <summary>
        /// Gets the collection of Ranges associated with the Indicator
        /// </summary>
        [Browsable(true), Category("Behavior")]
        [Description("Indicates the collection of Ranges associated with the Indicator.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public StateRangeCollection Ranges
        {
            get
            {
                if (_Ranges == null)
                {
                    _Ranges = new StateRangeCollection();
                    _Ranges.CollectionChanged += StateRanges_CollectionChanged;
                }

                return (_Ranges);
            }

            set
            {
                if (_Ranges != null)
                    _Ranges.CollectionChanged -= StateRanges_CollectionChanged;

                _Ranges = value;

                if (_Ranges != null)
                    _Ranges.CollectionChanged += StateRanges_CollectionChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region RoundRectangleArc

        /// <summary>
        /// Gets or sets the RoundRectangle corner radius,
        /// measured as a percentage of the width/height.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(.75f)]
        [Description("Indicates the RoundRectangle corner radius, measured as a percentage of the width/height.")]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float RoundRectangleArc
        {
            get { return (_RoundRectangleArc); }

            set
            {
                if (_RoundRectangleArc != value)
                {
                    if (value < 0 || value > 1f)
                        throw new ArgumentException("Radius must be between 0 and 1");

                    _RoundRectangleArc = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets or sets the Indicator Style
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(StateIndicatorStyle.Circular)]
        [Description("Indicates the Indicator Style.")]
        public StateIndicatorStyle Style
        {
            get { return (_Style); }

            set
            {
                if (_Style != value)
                {
                    _Style = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets the alignment of the text
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(TextAlignment.MiddleCenter)]
        [Description("Indicates the alignment of the text.")]
        public TextAlignment TextAlignment
        {
            get { return (_TextAlignment); }

            set
            {
                if (_TextAlignment != value)
                {
                    _TextAlignment = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region TextColor

        /// <summary>
        /// Gets or sets the text Color
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        [Description("Indicates the text Color.")]
        public Color TextColor
        {
            get { return (_TextColor); }

            set
            {
                if (_TextColor != value)
                {
                    _TextColor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region TextHorizontalOffset

        /// <summary>
        /// Gets or sets the horizontal distance to offset the Indicator Text, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the horizontal distance to offset the Indicator Text, measured as a percentage.")]
        public float TextHorizontalOffset
        {
            get { return (_TextHorizontalOffset); }

            set
            {
                if (_TextHorizontalOffset != value)
                {
                    _TextHorizontalOffset = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region TextVerticalOffset

        /// <summary>
        /// Gets or sets the vertical distance to offset the Indicator Text, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the vertical distance to offset the Indicator Text, measured as a percentage.")]
        public float TextVerticalOffset
        {
            get { return (_TextVerticalOffset); }

            set
            {
                if (_TextVerticalOffset != value)
                {
                    _TextVerticalOffset = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsFont

        internal override Font AbsFont
        {
            get
            {
                if (AutoSize == false)
                    return (Font);

                if (_AbsFont == null)
                {
                    int n = Math.Min(Bounds.Width, Bounds.Height);

                    float emSize = Font.SizeInPoints;
                    emSize = (emSize / 40) * n;

                    AbsFont = new Font(Font.FontFamily, emSize, Font.Style);
                }

                return (_AbsFont);
            }

            set
            {
                if (_AbsFont != null)
                    _AbsFont.Dispose();

                _AbsFont = value;
            }
        }

        #endregion

        #endregion

        #region Event processing

        #region StateRange processing

        void StateRanges_CollectionChanged(object sender, EventArgs e)
        {
            foreach (StateRange range in _Ranges)
            {
                range.StateIndicator = this;

                range.IndicatorRangeChanged -= StateRange_ItemChanged;
                range.IndicatorRangeChanged += StateRange_ItemChanged;
            }
        }

        void StateRange_ItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                Size size = Bounds.Size;
                
                base.RecalcLayout();

                if (size.Equals(Bounds.Size) == false)
                    AbsFont = null;
            }
        }

        #endregion

        #region OnPaint

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            RecalcLayout();

            if (GaugeControl.OnPreRenderIndicator(e, this) == false)
            {
                InterpolationMode im = g.InterpolationMode;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.TranslateTransform(Center.X, Center.Y);
                g.RotateTransform(_Angle % 360);

                Rectangle r = new Rectangle(0, 0, Bounds.Width, Bounds.Height);

                r.X -= Bounds.Width / 2;
                r.Y -= Bounds.Height / 2;

                StateRange range = GetStateRange();

                DrawStateImage(g, r, range);
                DrawStateText(g, r, range);

                GaugeControl.OnPostRenderIndicator(e, this);

                g.ResetTransform();

                g.InterpolationMode = im;
            }
        }

        #region GetStateRange

        private StateRange GetStateRange()
        {
            return (_Ranges != null ? _Ranges.GetValueRange(DValue) : null);
        }

        #endregion

        #region DrawStateImage

        private void DrawStateImage(Graphics g, Rectangle r, StateRange range)
        {
            Image image = (range != null) ? range.Image : _Image;

            if (image != null)
            {
                if (AutoSize == true)
                {
                    g.DrawImage(image, r);
                }
                else
                {
                    r.X += (r.Width - image.Width) / 2;
                    r.Y += (r.Height - image.Height) / 2;

                    g.DrawImageUnscaled(image, r);
                }
            }
            else
            {
                GradientFillColor fillColor =
                    (range != null) ? range.BackColor : BackColor;

                using (GraphicsPath path = GetStatePath(r))
                    RenderBackPath(g, path, r, fillColor);
            }
        }

        #region GetStatePath

        private GraphicsPath GetStatePath(Rectangle r)
        {
            switch (_Style)
            {
                case StateIndicatorStyle.Circular:
                    return (GetCircularState(r));

                case StateIndicatorStyle.Rectangular:
                    return (GetRectState(r));

                default:
                    return (GetRoundRectState(r));
            }
        }

        #endregion

        #region GetCircularState

        private GraphicsPath GetCircularState(Rectangle bounds)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddEllipse(bounds);

            return (path);
        }

        #endregion

        #region GetRectState

        private GraphicsPath GetRectState(Rectangle bounds)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(bounds);

            return (path);
        }

        #endregion

        #region GetRoundRectState

        private GraphicsPath GetRoundRectState(Rectangle bounds)
        {
            GraphicsPath path = new GraphicsPath();

            int m = Math.Min(bounds.Width, bounds.Height);
            int n = (int)(m * _RoundRectangleArc) + 1;

            Rectangle t = new Rectangle(bounds.Right - n, bounds.Bottom - n, n, n);
            path.AddArc(t, 0, 90);

            t.X = bounds.X;
            path.AddArc(t, 90, 90);

            t.Y = bounds.Y;
            path.AddArc(t, 180, 90);

            t.X = bounds.Right - n;
            path.AddArc(t, 270, 90);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region RenderBackPath

        private void RenderBackPath(Graphics g,
            GraphicsPath path, Rectangle bounds, GradientFillColor fillColor)
        {
            Rectangle r = bounds;
            GradientFillType fillType = fillColor.GradientFillType;

            if (fillColor.End.IsEmpty)
            {
                fillType = GradientFillType.None;
            }
            else
            {
                if (fillColor.GradientFillType == GradientFillType.Auto)
                    fillType = GradientFillType.Center;
            }

            switch (fillType)
            {
                case GradientFillType.Angle:
                    using (Brush br = fillColor.GetBrush(r))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.StartToEnd:
                    using (Brush br = fillColor.GetBrush(r, 90))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.HorizontalCenter:
                    r.Height /= 2;

                    if (r.Height <= 0)
                        r.Height = 1;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(r, fillColor.Start, fillColor.End, 90))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.VerticalCenter:
                    r.Width /= 2;

                    if (r.Width <= 0)
                        r.Width = 1;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(r, fillColor.Start, fillColor.End, 0f))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.Center:
                    using (PathGradientBrush br = new PathGradientBrush(path))
                    {
                        br.CenterPoint = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
                        br.CenterColor = fillColor.Start;
                        br.SurroundColors = new Color[] { fillColor.End };

                        g.FillPath(br, path);
                    }
                    break;

                default:
                    using (Brush br = new SolidBrush(fillColor.Start))
                        g.FillPath(br, path);

                    break;
            }

            if (fillColor.BorderWidth > 0 && fillColor.BorderColor.IsEmpty == false)
            {
                using (Pen pen = new Pen(fillColor.BorderColor, fillColor.BorderWidth))
                    g.DrawPath(pen, path);
            }
            else
            {
                if (fillType == GradientFillType.Center)
                {
                    using (Pen pen = new Pen(fillColor.End))
                        g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #endregion

        #region DrawStateText

        private void DrawStateText(Graphics g, Rectangle r, StateRange range)
        {
            string text = (range != null)
                ? range.Text : GetOverrideString();

            if (string.IsNullOrEmpty(text) == false)
            {
                using (StringFormat sf = new StringFormat())
                {
                    SetStringAlignment(sf);

                    Color color = _TextColor;

                    if (range != null)
                    {
                        color = range.TextColor;

                        int dx = (int)(r.Width * range.TextHorizontalOffset);
                        int dy = (int)(r.Height * range.TextVerticalOffset);

                        r.Offset(dx, dy);
                    }
                    else
                    {
                        int dx = (int)(r.Width * _TextHorizontalOffset);
                        int dy = (int)(r.Height * _TextVerticalOffset);

                        r.Offset(dx, dy);
                    }

                    using (Brush br = new SolidBrush(color))
                        g.DrawString(text, AbsFont, br, r, sf);
                }
            }
        }

        #region SetStringAlignment

        private void SetStringAlignment(StringFormat sf)
        {
            switch (_TextAlignment)
            {
                case TextAlignment.TopLeft:
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Alignment = StringAlignment.Near;
                    break;

                case TextAlignment.TopCenter:
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Alignment = StringAlignment.Center;
                    break;

                case TextAlignment.TopRight:
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Alignment = StringAlignment.Far;
                    break;

                case TextAlignment.MiddleLeft:
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Near;
                    break;

                case TextAlignment.MiddleCenter:
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    break;

                case TextAlignment.MiddleRight:
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Far;
                    break;

                case TextAlignment.BottomLeft:
                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Near;
                    break;

                case TextAlignment.BottomCenter:
                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Center;
                    break;

                case TextAlignment.BottomRight:
                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Far;
                    break;
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Refresh

        internal void Refresh()
        {
            if (NeedRecalcLayout == false)
                OnGaugeItemChanged(false);
        }

        #endregion

        #region Contains

        internal override bool Contains(Point pt)
        {
            using (GraphicsPath path = GetStatePath(Bounds))
            {
                Matrix matrix = new Matrix();
                matrix.RotateAt(_Angle, Center);

                path.Transform(matrix);

                return (path.IsVisible(pt));
            }
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            StateIndicator c = copy as StateIndicator;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Angle = _Angle;
                c.Image = _Image;

                if (_Ranges != null)
                    c.Ranges = (StateRangeCollection)_Ranges.Clone();

                c.RoundRectangleArc = _RoundRectangleArc;
                c.Style = _Style;
                c.TextAlignment = _TextAlignment;
                c.TextColor = _TextColor;
                c.TextHorizontalOffset = _TextHorizontalOffset;
                c.TextVerticalOffset = _TextVerticalOffset;
            }
        }

        #endregion
    }

    #region Enums

    public enum StateIndicatorStyle
    {
        Rectangular,
        RoundedRectangular,
        Circular
    }

    #endregion

}
