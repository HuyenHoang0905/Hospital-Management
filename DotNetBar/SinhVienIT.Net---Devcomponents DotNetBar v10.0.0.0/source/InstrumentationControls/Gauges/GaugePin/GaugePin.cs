using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GaugePinConvertor))]
    public class GaugePin : GaugeItem
    {
        #region Private variables

        private GaugeMarkerStyle _Style = GaugeMarkerStyle.Circle;
        private DisplayPlacement _Placement = DisplayPlacement.Center;

        private float _ScaleOffset;         // Relative distance from scale
        private float _EndOffset;           // Relative distance or degrees from end

        private GradientFillColor _FillColor;

        private Image _Image;

        private int _Radius;
        private float _Angle;
        private Rectangle _Bounds;

        private float _Width;
        private float _Length;
        private bool _IsMaxPin;

        private GaugeMarker _GaugeMarker;
        private GaugePinLabel _Label;

        private GaugeScale _Scale;

        #endregion

        public GaugePin(GaugeScale scale, bool isMmaxPin, string name)
        {
            _Scale = scale;
            _IsMaxPin = isMmaxPin;

            Name = name;

            _GaugeMarker = new GaugeMarker();
            _Label = new GaugePinLabel(this);

            FillColor = new GradientFillColor(Color.WhiteSmoke);
            FillColor.BorderColor = Color.DimGray;
            FillColor.BorderWidth = 1;

            Length = .06f;
            Width = .06f;

            _EndOffset = .02f;

            HookEvents(true);
        }

        #region Public properties

        #region EndOffset

        /// <summary>
        /// Gets or sets the offset from the end of the scale, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(.02f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the offset from the end of the scale, specified as a percentage.")]
        public float EndOffset
        {
            get { return (_EndOffset); }

            set
            {
                if (_EndOffset != value)
                {
                    if (value < -1 || value > 1)
                        throw new ArgumentException("Value must be between -1 and +1.");

                    _EndOffset = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region FillColor

        /// <summary>
        /// Gets or sets Indicates the Pin FillColor
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Pin FillColor.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor FillColor
        {
            get
            {
                if (_FillColor == null)
                {
                    _FillColor = new GradientFillColor();
                    _FillColor.ColorTableChanged += FillColor_ColorTableChanged;
                }

                return (_FillColor);
            }

            set
            {
                if (_FillColor != null)
                    _FillColor.ColorTableChanged -= FillColor_ColorTableChanged;

                _FillColor = value;

                if (_FillColor != null)
                    _FillColor.ColorTableChanged += FillColor_ColorTableChanged;

                OnGaugeItemChanged(true);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeFillColor()
        {
            return (FillColor.IsEqualTo(Color.WhiteSmoke,
                Color.Empty, 90, GradientFillType.Auto, Color.DimGray, 1) == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetFillColor()
        {
            FillColor = new GradientFillColor(Color.WhiteSmoke);
            FillColor.BorderColor = Color.DimGray;
            FillColor.BorderWidth = 1;
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets or sets the Image to use for the Pin
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Image to use for the Pin.")]
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

        #region IsMaxPin

        /// <summary>
        /// Gets whether the pin is the Maximum Pin
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMaxPin
        {
            get { return (_IsMaxPin); }
            internal set { _IsMaxPin = value; }
        }

        #endregion

        #region Label

        /// <summary>
        /// Gets or sets the Label associated with the Pin
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Label associated with the Pin.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugePinLabel Label
        {
            get { return (_Label); }
            internal set { _Label = value; }
        }

        #endregion

        #region Length

        /// <summary>
        /// Gets or sets the Length of the Pin, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(.06f)]
        [Editor("DevComponents.Instrumentation.Design.RangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Length of the Pin, specified as a percentage.")]
        public float Length
        {
            get { return (_Length); }

            set
            {
                if (_Length != value)
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentException("Value must be between 0 and +1.");

                    _Length = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Placement

        /// <summary>
        /// Gets or sets the Placement of the Pin with respect to the Scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(DisplayPlacement.Center)]
        [Description("Indicates the Placement of the Pin with respect to the Scale.")]
        public DisplayPlacement Placement
        {
            get { return (_Placement); }

            set
            {
                if (_Placement != value)
                {
                    _Placement = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Scale

        /// <summary>
        /// Gets the pin's associated Scale
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GaugeScale Scale
        {
            get { return (_Scale); }

            internal set
            {
                _Scale = value;
                _Label.Scale = value;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region ScaleOffset

        /// <summary>
        /// Gets or sets the distance from the Pin to the Scale, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the distance from the Pin to the Scale, measured as a percentage.")]
        public float ScaleOffset
        {
            get { return (_ScaleOffset); }

            set
            {
                if (_ScaleOffset != value)
                {
                    _ScaleOffset = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion
        
        #region Style

        /// <summary>
        /// Gets or sets the Pin display style
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(GaugeMarkerStyle.Circle)]
        [Description("Indicates the Pin display style.")]
        public GaugeMarkerStyle Style
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

        #region Width

        /// <summary>
        /// Gets or sets the Width of the Pin, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(.06f)]
        [Editor("DevComponents.Instrumentation.Design.WidthRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Width of the Pin, specified as a percentage.")]
        public float Width
        {
            get { return (_Width); }

            set
            {
                if (_Width != value)
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentException("Value must be between 0 and +1.");

                    _Width = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region Angle

        internal float Angle
        {
            get { return (_Angle); }
        }

        #endregion

        #region Bounds

        internal Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        #endregion

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
                _Label.GaugeItemChanged += GaugePinLabel_GaugeItemChanged;
            else
                _Label.GaugeItemChanged -= GaugePinLabel_GaugeItemChanged;
        }

        #endregion

        #region Event processing

        void FillColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        void GaugePinLabel_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                CalcPinMetrics();

                _GaugeMarker.Clear();
                _Label.NeedRecalcLayout = true;
            }
        }

        #region CalcPinMetrics

        private void CalcPinMetrics()
        {
            if (Scale is GaugeCircularScale)
                CalcCircularMetrics(Scale as GaugeCircularScale);

            else if (Scale is GaugeLinearScale)
                CalcLinearMetrics(Scale as GaugeLinearScale);
        }

        #region CalcCircularMetrics

        private void CalcCircularMetrics(GaugeCircularScale scale)
        {
            _Radius = scale.AbsRadius;
            _Bounds = new Rectangle(0, 0, (int)(_Radius * _Width), (int)(_Radius * _Length));

            int m = scale.AbsScaleWidth;
            int offset = (int)(scale.AbsRadius * _ScaleOffset);

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    _Radius -= ((_Bounds.Height + m) / 2 + offset);
                    break;

                case DisplayPlacement.Center:
                    _Radius += (offset);
                    break;

                case DisplayPlacement.Far:
                    _Radius += ((_Bounds.Height + m) / 2 + offset);
                    break;
            }

            float angle = _EndOffset * 360;

            if (_IsMaxPin != scale.Reversed)
                _Angle = scale.StartAngle + scale.SweepAngle + angle;
            else
                _Angle = scale.StartAngle - angle;

            Point pt = scale.GetPoint(_Radius, _Angle);

            _Bounds.Location = new Point(pt.X - _Bounds.Width / 2, pt.Y - _Bounds.Height / 2);

        }

        #endregion

        #region CalcLinearMetrics

        private void CalcLinearMetrics(GaugeLinearScale scale)
        {
            if (scale.Orientation == Orientation.Horizontal)
                CalcHorizontalMetrics(scale);
            else
                CalcVerticalMetrics(scale);
        }

        #region CalcHorizontalMetrics

        private void CalcHorizontalMetrics(GaugeLinearScale scale)
        {
            int width = scale.AbsWidth;
            int offset = (int)(width * _ScaleOffset);
            int m = scale.AbsScaleWidth;

            _Bounds = new Rectangle(0, 0, (int)(width * _Width), (int)(width * _Length));

            int x = (int)(_EndOffset * scale.AbsLength);

            if (_IsMaxPin != scale.Reversed)
                x += scale.ScaleBounds.Right;
            else
                x = scale.ScaleBounds.Left - x;

            x -= _Bounds.Width / 2;

            int y = scale.ScaleBounds.Y;

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    y -= (_Bounds.Height + offset);
                    break;

                case DisplayPlacement.Center:
                    y -= ((_Bounds.Height - m) / 2 + offset);
                    break;

                case DisplayPlacement.Far:
                    y += (m + offset);
                    break;
            }

            _Bounds.Location = new Point(x, y);
        }

        #endregion

        #region CalcVerticalMetrics

        private void CalcVerticalMetrics(GaugeLinearScale scale)
        {
            int width = scale.AbsWidth;
            int offset = (int)(width * _ScaleOffset);
            int m = scale.AbsScaleWidth;

            _Bounds = new Rectangle(0, 0, (int)(width * _Width), (int)(width * _Length));

            int y = (int)(_EndOffset * scale.AbsLength);

            if (_IsMaxPin != scale.Reversed)
                y = scale.ScaleBounds.Top - y;
            else
                y += scale.ScaleBounds.Bottom;

            y -= _Bounds.Height / 2;

            int x = scale.ScaleBounds.X;

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    x -= (_Bounds.Width + offset);
                    break;

                case DisplayPlacement.Center:
                    x -= ((_Bounds.Width - m ) / 2 + offset);
                    break;

                case DisplayPlacement.Far:
                    x += (m + offset);
                    break;
            }

            _Bounds.Location = new Point(x, y);
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();

            if (Scale.GaugeControl.OnPreRenderScaleGaugePin(e, this) == false)
            {
                Graphics g = e.Graphics;

                if (_Bounds.Width > 0 && _Bounds.Height > 0)
                {
                    Image image = _Image ?? GetPinBitmap(g);

                    if (image != null)
                    {
                        if (Scale is GaugeCircularScale)
                            PaintCircularPin(g, image);

                        else if (Scale is GaugeLinearScale)
                            PaintLinearPin(g, image);
                    }

                    if (_Label.Visible)
                        _Label.OnPaint(e);
                }

                Scale.GaugeControl.OnPostRenderScaleGaugePin(e, this);
            }
        }

        #region PaintCircularPin

        private void PaintCircularPin(Graphics g, Image image)
        {
            Rectangle r = new Rectangle(0, 0, _Bounds.Width, _Bounds.Height);

            float angle = _Angle + 90;

            if (_Placement == DisplayPlacement.Near)
                angle += 180;

            g.TranslateTransform(_Bounds.X + _Bounds.Width / 2, _Bounds.Y + _Bounds.Height / 2);
            g.RotateTransform(angle % 360);

            r.X -= _Bounds.Width / 2;
            r.Y -= _Bounds.Height / 2;

            g.DrawImage(image, r);
            g.ResetTransform();
        }

        #endregion

        #region PaintLinearPin

        #region PaintLinearPin

        private void PaintLinearPin(Graphics g, Image image)
        {
            g.DrawImage(image, _Bounds);
        }

        #endregion

        #endregion

        #region GetPinBitmap

        internal Bitmap GetPinBitmap(Graphics g)
        {
            if (_Style != GaugeMarkerStyle.None)
                return (_GaugeMarker.GetMarkerBitmap(g, _Style, _FillColor, _Bounds.Width, _Bounds.Height));

            return (null);
        }

        #endregion

        #endregion

        #region OnDispose

        protected override void OnDispose()
        {
            HookEvents(false);

            _GaugeMarker.Dispose();

            base.OnDispose();
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugePin copy = new
                GaugePin(_Scale, _IsMaxPin, Name);

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugePin c = copy as GaugePin;

            if (c != null)
            {
                base.CopyToItem(c);

                c.EndOffset = _EndOffset;

                if (_FillColor != null)
                    c.FillColor = (GradientFillColor)_FillColor.Clone();

                c.Image = _Image;
                c.IsMaxPin = _IsMaxPin;
                c.Length = _Length;
                c.Placement = _Placement;
                c.Scale = _Scale;
                c.ScaleOffset = _ScaleOffset;
                c.Style = _Style;
                c.Width = _Width;

                c.Label = new GaugePinLabel(c);
                _Label.CopyToItem(c.Label);
            }
        }

        #endregion
    }

    #region GaugePinConvertor

    public class GaugePinConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugePin pin = value as GaugePin;

                if (pin != null)
                {
                    //ColorConverter cvt = new ColorConverter();

                    //if (lct.Start != Color.Empty)
                    //    return (cvt.ConvertToString(lct.Start));

                    //if (lct.End != Color.Empty)
                    //    return (cvt.ConvertToString(lct.End));

                    //if (lct.GradientAngle != 90)
                    //    return (lct.GradientAngle.ToString());

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

}
