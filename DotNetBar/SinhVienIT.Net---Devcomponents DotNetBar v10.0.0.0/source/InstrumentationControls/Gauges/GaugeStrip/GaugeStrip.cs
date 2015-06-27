using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Text;

namespace DevComponents.Instrumentation
{
    public class GaugeStrip : GaugeItem, IDisposable
    {
        #region Events

        [Description("Occurs when the coverage of a Strip changes.")]
        public event EventHandler<EventArgs> StripCoverChanged;

        #endregion

        #region Private variables

        private double _StartValue;
        private double _EndValue;
        private float _ScaleOffset;

        private GradientFillColor _FillColor;

        private Color _LabelColor;
        private GradientFillColor _CapFillColor;
        private GradientFillColor _PointerFillColor;
        private GradientFillColor _MajorTickMarkFillColor;
        private GradientFillColor _MinorTickMarkFillColor;
        private GaugeScale _Scale;

        private float _StartAngle;
        private float _SweepAngle;

        private double _MinValue;
        private double _MaxValue;
        private Rectangle _Bounds;

        private GraphicsPath _StripePath;

        #endregion

        public GaugeStrip(GaugeScale scale)
            : this()
        {
            _Scale = scale;
        }

        public GaugeStrip()
        {
            FillColor = new GradientFillColor();

            _StartValue = double.NaN;
            _EndValue = double.NaN;
        }

        #region Public properties

        #region CapFillColor

        /// <summary>
        /// Gets or sets the Cap Fill Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Cap Fill Color.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor CapFillColor
        {
            get
            {
                if (_CapFillColor == null)
                {
                    _CapFillColor = new GradientFillColor();
                    _CapFillColor.ColorTableChanged += PointerFillColor_ColorTableChanged;
                }

                return (_CapFillColor);
            }

            set
            {
                if (_CapFillColor != null)
                    _CapFillColor.ColorTableChanged -= PointerFillColor_ColorTableChanged;

                _CapFillColor = value;

                if (_CapFillColor != null)
                    _CapFillColor.ColorTableChanged += PointerFillColor_ColorTableChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region EndValue

        /// <summary>
        /// Gets or sets the Ending value for the area
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(double.NaN)]
        [Description("Indicates the Ending value for the area.")]
        public double EndValue
        {
            get { return (_EndValue); }

            set
            {
                if (_EndValue != value)
                {
                    _EndValue = value;

                    OnStripCoverChanged();
                }
            }
        }

        #endregion

        #region PointerFillColor

        /// <summary>
        /// Gets or sets the Pointer Fill Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Pointer Fill Color.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor PointerFillColor
        {
            get
            {
                if (_PointerFillColor == null)
                {
                    _PointerFillColor = new GradientFillColor();
                    _PointerFillColor.ColorTableChanged += PointerFillColor_ColorTableChanged;
                }

                return (_PointerFillColor);
            }

            set
            {
                if (_PointerFillColor != null)
                    _PointerFillColor.ColorTableChanged -= PointerFillColor_ColorTableChanged;

                _PointerFillColor = value;

                if (_PointerFillColor != null)
                    _PointerFillColor.ColorTableChanged += PointerFillColor_ColorTableChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region FillColor

        /// <summary>
        /// Gets or sets the area Fill Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the area Fill Color.")]
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

                OnGaugeItemChanged();
            }
        }

        #endregion

        #region LabelColor

        /// <summary>
        /// Gets or sets the Section Label Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Section Label Color.")]
        public Color LabelColor
        {
            get { return (_LabelColor); }

            set
            {
                if (_LabelColor != value)
                {
                    _LabelColor = value;

                    OnGaugeItemChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeLabelColor()
        {
            return (_LabelColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void ResetLabelColor()
        {
            LabelColor = Color.Empty;
        }

        #endregion

        #region MajorTickMarkFillColor

        /// <summary>
        /// Gets or sets the MajorTickMark Fill Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the MajorTickMark Fill Color.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor MajorTickMarkFillColor
        {
            get
            {
                if (_MajorTickMarkFillColor == null)
                {
                    _MajorTickMarkFillColor = new GradientFillColor();
                    _MajorTickMarkFillColor.ColorTableChanged += TickMarkFillColor_ColorTableChanged;
                }

                return (_MajorTickMarkFillColor);
            }

            set
            {
                if (_MajorTickMarkFillColor != null)
                    _MajorTickMarkFillColor.ColorTableChanged -= TickMarkFillColor_ColorTableChanged;

                _MajorTickMarkFillColor = value;

                if (_MajorTickMarkFillColor != null)
                    _MajorTickMarkFillColor.ColorTableChanged += TickMarkFillColor_ColorTableChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region MinorTickMarkFillColor

        /// <summary>
        /// Gets or sets the MinorTickMark Fill Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the MinorTickMark Fill Color.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor MinorTickMarkFillColor
        {
            get
            {
                if (_MinorTickMarkFillColor == null)
                {
                    _MinorTickMarkFillColor = new GradientFillColor();
                    _MinorTickMarkFillColor.ColorTableChanged += TickMarkFillColor_ColorTableChanged;
                }

                return (_MinorTickMarkFillColor);
            }

            set
            {
                if (_MinorTickMarkFillColor != null)
                    _MinorTickMarkFillColor.ColorTableChanged -= TickMarkFillColor_ColorTableChanged;

                _MinorTickMarkFillColor = value;

                if (_MinorTickMarkFillColor != null)
                    _MinorTickMarkFillColor.ColorTableChanged += TickMarkFillColor_ColorTableChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region Scale

        /// <summary>
        /// Gets the associated Scale
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GaugeScale Scale
        {
            get { return (_Scale); }
            internal set { _Scale = value; }
        }

        #endregion

        #region ScaleOffset

        /// <summary>
        /// Gets or sets the distance from the Scale, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the distance from the Scale, measured as a percentage.")]
        public float ScaleOffset
        {
            get { return (_ScaleOffset); }

            set
            {
                if (_ScaleOffset != value)
                {
                    if (value < -1 || value > 1)
                        throw new ArgumentException("Scale Offset must be bwtween -1 and +1");

                    _ScaleOffset = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region StartValue

        /// <summary>
        /// Gets or sets the Starting value for the area
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(double.NaN)]
        [Description("Indicates the Starting value for the area.")]
        public double StartValue
        {
            get { return (_StartValue); }

            set
            {
                if (_StartValue != value)
                {
                    _StartValue = value;

                    OnStripCoverChanged();
                }
            }
        }

        private void OnStripCoverChanged()
        {
            if (StripCoverChanged != null)
                StripCoverChanged(this, EventArgs.Empty);

            OnGaugeItemChanged(true);
        }

        #endregion

        #region Visible

        /// <summary>
        /// Gets or sets the item Visibility state.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates the item Visibility state.")]
        [ParenthesizePropertyName(true)]
        public override bool Visible
        {
            get { return (base.Visible); }

            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;

                    OnStripCoverChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region Bounds

        internal Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        #endregion

        #region HasCapFillColor

        internal bool HasCapFillColor
        {
            get { return (_CapFillColor != null && _CapFillColor.IsEmpty == false); }
        }

        #endregion

        #region HasMajorTickMarkFillColor

        internal bool HasMajorTickMarkFillColor
        {
            get { return (_MajorTickMarkFillColor != null &&
                _MajorTickMarkFillColor.IsEmpty == false); }
        }

        #endregion

        #region HasMinorTickMarkFillColor

        internal bool HasMinorTickMarkFillColor
        {
            get { return (_MinorTickMarkFillColor != null && _MinorTickMarkFillColor.IsEmpty == false); }
        }

        #endregion

        #region HasPointerFillColor

        internal bool HasPointerFillColor
        {
            get { return (_PointerFillColor != null && _PointerFillColor.IsEmpty == false); }
        }

        #endregion

        #region MaxValue

        internal double MaxValue
        {
            get { return (_MaxValue); }
            set { _MaxValue = value; }
        }

        #endregion

        #region MinValue

        internal double MinValue
        {
            get { return (_MinValue); }
            set { _MinValue = value; }
        }

        #endregion

        #region StartAngle

        internal float StartAngle
        {
            get { return (_StartAngle); }
        }

        #endregion

        #region StripePath

        internal GraphicsPath StripePath
        {
            get { return (_StripePath); }

            set
            {
                if (_StripePath != value)
                {
                    if (_StripePath != null)
                        _StripePath.Dispose();

                    _StripePath = value;
                }
            }
        }

        #endregion

        #region SweepAngle

        internal float SweepAngle
        {
            get { return (_SweepAngle); }
        }

        #endregion

        #endregion

        #region Event processing

        void FillColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        void TickMarkFillColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        void PointerFillColor_ColorTableChanged(object sender, EventArgs e)
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

                CalcStripMetrics();

                _Scale.NeedTickMarkRecalcLayout = true;
                _Scale.NeedPointerRecalcLayout = true;

                StripePath = null;
            }
        }

        #region CalcStripMetrics

        private void CalcStripMetrics()
        {
            _MinValue = (_StartValue.Equals(double.NaN) ? _Scale.MinValue : _StartValue);
            _MaxValue = (_EndValue.Equals(double.NaN) ? _Scale.MaxValue : _EndValue);

            if (_MinValue > _Scale.MaxValue)
                _MinValue = _Scale.MaxValue;

            _MinValue -= _Scale.MinValue;

            if (_MinValue < 0)
                _MinValue = 0;

            if (_MaxValue < _Scale.MinValue)
                _MaxValue = _Scale.MinValue;

            _MaxValue -= _Scale.MinValue;

            if (_MaxValue > _Scale.MaxValue - _Scale.MinValue)
                _MaxValue = _Scale.MaxValue - _Scale.MinValue;

            if (Scale is GaugeCircularScale)
                CalcCircularMetrics(Scale as GaugeCircularScale);
        }

        #region CalcCircularMetrics

        private void CalcCircularMetrics(GaugeCircularScale scale)
        {
            float spread = (float)Math.Abs(scale.MaxValue - scale.MinValue);

            if (spread == 0)
                spread = 1;

            float dv = scale.SweepAngle / spread;

            _StartAngle = (float)(dv * _MinValue) + scale.StartAngle;
            _SweepAngle = (float)(dv * (_MaxValue - _MinValue));

            if (scale.Reversed == true)
            {
                _StartAngle = (scale.StartAngle + scale.SweepAngle) - (_StartAngle - scale.StartAngle);
                _SweepAngle = -_SweepAngle;
            }
        }

        #endregion

        #endregion

        #endregion

        #region GetPoint

        protected PointF GetPoint(float radius, float angle)
        {
            PointF pt = new PointF();

            // Normalize the angle and calculate some
            // working vars

            if (angle < 0)
                angle += 360;

            angle = angle % 360;

            // Determine the angle quadrant, and then calculate
            // the intersecting coordinate accordingly

            double radians = GetRadians(angle % 90);

            if (angle < 90)
            {
                pt.X = (float)(Math.Cos(radians) * radius);
                pt.Y = (float)(Math.Sin(radians) * radius);
            }
            else if (angle < 180)
            {
                pt.X = -(float)(Math.Sin(radians) * radius);
                pt.Y = (float)(Math.Cos(radians) * radius);
            }
            else if (angle < 270)
            {
                pt.X = -(float)(Math.Cos(radians) * radius);
                pt.Y = -(float)(Math.Sin(radians) * radius);
            }
            else
            {
                pt.X = (float)(Math.Sin(radians) * radius);
                pt.Y = -(float)(Math.Cos(radians) * radius);
            }

            pt.X += _Scale.Center.X;
            pt.Y += _Scale.Center.Y;

            return (pt);
        }

        #endregion

        #region GetRadians

        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="theta">Degrees</param>
        /// <returns>Radians</returns>
        protected double GetRadians(float theta)
        {
            return (theta * Math.PI / 180);
        }

        #endregion

        #region ValueInRange

        internal bool ValueInRange(double value)
        {
            RecalcLayout();

            double startValue = (_StartValue.Equals(double.NaN) ? MinValue : _StartValue);
            double endValue = (_EndValue.Equals(double.NaN) ? MaxValue : _EndValue);

            if (startValue > endValue)
            {
                double temp = startValue;
                startValue = endValue;
                endValue = temp;
            }

            return (value >= startValue && value <= endValue);
        }

        #endregion

        #region ProcessTemplateText

        protected override void ProcessTemplateText(
            GaugeControl gauge, StringBuilder sb, string key, string data)
        {
            switch (key)
            {
                case "StartValue":
                    sb.Append(string.IsNullOrEmpty(data)
                        ? _StartValue.ToString()
                        : String.Format("{0:" + data + "}", _StartValue));
                    break;

                case "EndValue":
                    sb.Append(string.IsNullOrEmpty(data)
                        ? _EndValue.ToString()
                        : String.Format("{0:" + data + "}", _EndValue));
                    break;

                default:
                    base.ProcessTemplateText(gauge, sb, key, data);
                    break;
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            StripePath = null;
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeStrip copy = new GaugeStrip();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeStrip c = copy as GaugeStrip;

            if (c != null)
            {
                base.CopyToItem(c);

                if (_CapFillColor != null)
                    c.CapFillColor = (GradientFillColor)_CapFillColor.Clone();

                c.EndValue = _EndValue;

                if (_PointerFillColor != null)
                    c.PointerFillColor = (GradientFillColor)_PointerFillColor.Clone();

                if (_FillColor != null)
                    c.FillColor = (GradientFillColor)_FillColor.Clone();

                c.LabelColor = _LabelColor;

                if (_MajorTickMarkFillColor != null)
                    c.MajorTickMarkFillColor = (GradientFillColor)_MajorTickMarkFillColor.Clone();

                if (_MinorTickMarkFillColor != null)
                    c.MinorTickMarkFillColor = (GradientFillColor)_MinorTickMarkFillColor.Clone();

                c.ScaleOffset = _ScaleOffset;
                c.StartValue = _StartValue;
            }
        }

        #endregion
    }
}
