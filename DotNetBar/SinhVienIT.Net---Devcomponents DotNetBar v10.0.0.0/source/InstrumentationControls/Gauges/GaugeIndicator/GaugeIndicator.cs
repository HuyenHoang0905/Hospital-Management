using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    public class GaugeIndicator : GaugeItem
    {
        #region Private variables

        private bool _AutoSize;
        private bool _UnderScale;
        private int _RefreshRate;

        private double _MinValue;
        private double _MaxValue;
        private double _Value;

        private GradientFillColor _BackColor;

        private SizeF _Size;
        private PointF _Location;
        private Rectangle _Bounds;
        private Point _Center;

        private Font _Font;
        private string _Text;
        private string _EmptyString;
        private string _OverRangeString;
        private string _UnderRangeString;

        private bool _Dampening;
        private double _DampeningSweepTime;

        private double _DValue;
        private double _DeltaValue;
        private double _DStartValue;
        private double _DEndValue;

        private long _DStartTicks;
        private long _DRefreshTicks;

        private GaugeControl _GaugeControl;

        #endregion

        public GaugeIndicator()
        {
            _AutoSize = true;

            BackColor.BorderColor = Color.Black;

            _Size = new SizeF(.07f, .07f);
            _Location = new PointF(.5f, .7f);

            _UnderScale = true;
            _RefreshRate = 10;

            _EmptyString = "-";
            _OverRangeString = "Error";
            _UnderRangeString = "Error";

            _MinValue = 0;
            _MaxValue = 100;
            _Value = double.NaN;
        }

        #region Public properties

        #region AutoSize

        /// <summary>
        /// Gets or sets whether the indicator contents are auto sized
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(true)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates whether the indicator contents are auto sized.")]
        public bool AutoSize
        {
            get { return (_AutoSize); }

            set
            {
                if (_AutoSize != value)
                {
                    _AutoSize = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region BackColor

        /// <summary>
        /// Gets or sets the BackColor
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the BackColor.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor BackColor
        {
            get
            {
                if (_BackColor == null)
                {
                    _BackColor = new GradientFillColor();
                    _BackColor.ColorTableChanged += BackColorColorTableChanged;
                }

                return (_BackColor);
            }

            set
            {
                if (_BackColor != null)
                    _BackColor.ColorTableChanged -= BackColorColorTableChanged;

                _BackColor = value;

                if (_BackColor != null)
                    _BackColor.ColorTableChanged += BackColorColorTableChanged;

                OnGaugeItemChanged();
            }
        }

        #endregion

        #region Bounds

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds
        {
            get { return (_Bounds); }
            internal set { _Bounds = value; }
        }

        #endregion

        #region DampeningSweepTime

        /// <summary>
        /// Gets or sets the time it takes for the indicator to
        /// change from its minimum to maximum value, measured in seconds
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(0d)]
        [Description("Indicates the time it takes for the indicator to change from its minimum to maximum value, measured in seconds.")]
        public double DampeningSweepTime
        {
            get { return (_DampeningSweepTime); }

            set
            {
                if (_DampeningSweepTime != value)
                {
                    _DampeningSweepTime = value;

                    OnGaugeItemChanged();
                }
            }
        }

        #endregion

        #region EmptyString

        /// <summary>
        /// Gets or sets the text string to display when the Indicator Value is empty (double.NaN)
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue("-")]
        [Description("Indicates the text string to display when the Indicator Value is empty (double.NaN).")]
        public string EmptyString
        {
            get { return (_EmptyString); }

            set
            {
                if (_EmptyString != value)
                {
                    _EmptyString = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Font

        /// <summary>
        /// Gets or sets the text Font
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the text Font.")]
        public Font Font
        {
            get
            {
                if (_Font == null)
                    _Font = new Font("Microsoft SanSerif", 12);

                return (_Font);
            }

            set
            {
                if (_Font != null)
                    _Font.Dispose();

                _Font = value;

                AbsFont = null;

                OnGaugeItemChanged(true);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeFont()
        {
            if (_Font == null)
                return (false);

            using (Font font = new Font("Microsoft SanSerif", 12))
                return (_Font.Equals(font) == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void ResetFont()
        {
            Font = new Font("Microsoft SanSerif", 12);
        }

        #endregion

        #region GaugeControl

        /// <summary>
        /// Gets or sets the owning GaugeControl
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GaugeControl GaugeControl
        {
            get { return (_GaugeControl); }
            internal set { _GaugeControl = value; }
        }

        #endregion

        #region Location

        /// <summary>
        /// Gets or sets the location of the image area, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.LocationEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the location of the image area, specified as a percentage.")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF Location
        {
            get { return (_Location); }

            set
            {
                if (_Location != value)
                {
                    _Location = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLocation()
        {
            return (_Location.X != .5f || _Location.Y != .7f);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLocation()
        {
            _Location = new PointF(.5f, .7f);
        }

        #endregion

        #region MaxValue

        /// <summary>
        /// Gets or sets the Maximum value for the Indicator
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(100d)]
        [Description("Indicates the Maximum value for the Indicator.")]
        public double MaxValue
        {
            get { return (_MaxValue); }

            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;

                    DValue = _Value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region MinValue

        /// <summary>
        /// Gets or sets the Minimum value for the Indicator
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(0d)]
        [Description("Indicates the Minimum value for the Indicator.")]
        public double MinValue
        {
            get { return (_MinValue); }

            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;

                    DValue = _Value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region OverRangeString

        /// <summary>
        /// Gets or sets the text string to display when the Indicator Value is over the set MaxValue range
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue("Error")]
        [Description("Indicates the text string to display when the Indicator Value is over the set MaxValue range.")]
        public string OverRangeString
        {
            get { return (_OverRangeString); }

            set
            {
                if (_OverRangeString != value)
                {
                    _OverRangeString = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region RefreshRate

        /// <summary>
        /// Gets or sets how often the indicator is refreshed per second
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(10)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates how often the indicator is refreshed per second.")]
        public int RefreshRate
        {
            get { return (_RefreshRate); }

            set
            {
                if (_RefreshRate != value)
                {
                    if (value <= 0 || value > 1000)
                        throw new ArgumentException("Value must be between 1 and 100");

                    _RefreshRate = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Size

        /// <summary>
        /// Gets or sets the size of the indicator, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.SizeEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Determines the size of the indicator, specified as a percentage.")]
        public SizeF Size
        {
            get { return (_Size); }

            set
            {
                if (_Size != value)
                {
                    _Size = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSize()
        {
            return (_Size.Width != .07f || _Size.Height != .07f);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSize()
        {
            _Size = new SizeF(.07f, .07f);
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text to be displayed
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        [Description("Indicates the text to be displayed.")]
        public string Text
        {
            get { return (_Text); }

            set
            {
                if (_Text != value)
                {
                    _Text = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region UnderRangeString

        /// <summary>
        /// Gets or sets the text string to display when the Indicator Value is under the set MinValue range
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue("Error")]
        [Description("Indicates the text string to display when the Indicator Value is under the set MinValue range.")]
        public string UnderRangeString
        {
            get { return (_UnderRangeString); }

            set
            {
                if (_UnderRangeString != value)
                {
                    _UnderRangeString = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region UnderScale

        /// <summary>
        /// Gets or sets whether the indicator is displayed under the scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(true)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates whether the indicator is displayed under the scale.")]
        public bool UnderScale
        {
            get { return (_UnderScale); }

            set
            {
                if (_UnderScale != value)
                {
                    _UnderScale = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets or sets the indicator value
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(double.NaN)]
        [Description("Indicates the indicator value.")]
        public double Value
        {
            get { return (_Value); }

            set
            {
                if (_Value != value)
                {
                    _Value = value;

                    OnValueChanged(true);
                }
            }
        }

        #endregion

        #region ValueEx

        /// <summary>
        /// Gets or sets the value of the indicator - but with no dampening
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ValueEx
        {
            get { return (_Value); }

            set
            {
                if (_Value != value)
                {
                    _Value = value;

                    OnValueChanged(false);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsFont

        // ReSharper disable ValueParameterNotUsed
        internal virtual Font AbsFont
        {
            get { return (Font); }
            set { }
        }
        // ReSharper restore ValueParameterNotUsed

        #endregion

        #region Center

        internal Point Center
        {
            get { return (_Center); }
        }

        #endregion

        #region DValue

        internal double DValue
        {
            get { return (_DValue); }

            set
            {
                _DValue = value;

                if (GaugeControl != null)
                    GaugeControl.DValueChange = true;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #endregion

        #region Event processing

        #region BackColor processing

        void BackColorColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #endregion

        #region OnValueChanged

        internal virtual void OnValueChanged(bool dampen)
        {
            if (GaugeControl != null && Visible == true)
            {
                if (GaugeControl.InDesignMode == false)
                {
                    if (_DValue < _Value)
                    {
                        double start = Math.Max(_MinValue, _DValue);

                        if (start < _MaxValue)
                        {
                            if (StartDampening(dampen, start, Math.Min(_MaxValue, _Value)))
                                return;
                        }
                    }
                    else if (_DValue > _Value)
                    {
                        double start = Math.Min(_MaxValue, _DValue);

                        if (start > _MinValue)
                        {
                            if (StartDampening(dampen, start, Math.Max(_MinValue, _Value)))
                                return;
                        }
                    }
                }
            }

            DValue = _Value;
        }

        #endregion

        #region Dampening support

        #region StartDampening

        private bool StartDampening(bool dampen, double start, double end)
        {
            double dampenTime = _DampeningSweepTime;

            if (GaugeControl.IsHandleCreated &&
                dampen == true && dampenTime > 0)
            {
                _DStartValue = start;
                _DEndValue = end;
                _DeltaValue = (_MaxValue - _MinValue) / (dampenTime * 1000);

                _DStartTicks = DateTime.Now.Ticks;

                if (_Dampening == false)
                {
                    _Dampening = true;
                    _DRefreshTicks = _DStartTicks;

                    _GaugeControl.DampeningUpdate += DampeningUpdate;
                    _GaugeControl.StartDampening();
                }

                return (true);
            }

            return (false);
        }

        #endregion

        #region DampeningUpdate

        void DampeningUpdate(object sender, EventArgs e)
        {
            SetIndicatorValue();

            if (_DValue == _DEndValue)
            {
                if (_Dampening == true)
                {
                    _GaugeControl.DampeningUpdate -= DampeningUpdate;
                    _GaugeControl.StopDampening();

                    _Dampening = false;
                }
            }
        }

        #endregion

        #region SetIndicatorValue

        private void SetIndicatorValue()
        {
            long now = DateTime.Now.Ticks;

            double ms = new TimeSpan(now - _DStartTicks).TotalMilliseconds;
            double delta = _DeltaValue * ms;

            if (delta > 0)
            {
                double n = (_DValue <= _Value)
                               ? Math.Min(_DStartValue + delta, _DEndValue)
                               : Math.Max(_DStartValue - delta, _DEndValue);

                if (n == _DEndValue ||
                    new TimeSpan(now - _DRefreshTicks).TotalMilliseconds > 1000 / _RefreshRate)
                {
                    _DRefreshTicks = now;

                    DValue = n;
                }
            }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            base.RecalcLayout();

            bool autoCenter = _GaugeControl.Frame.AutoCenter;
            _Center = _GaugeControl.GetAbsPoint(_Location, autoCenter);

            Size size = _GaugeControl.GetAbsSize(_Size, autoCenter);

            _Bounds = new Rectangle(
                _Center.X - size.Width / 2, _Center.Y - size.Height / 2,
                size.Width, size.Height);
        }

        #endregion

        #region Contains

        internal virtual bool Contains(Point pt)
        {
            return (Bounds.Contains(pt));
        }

        #endregion

        #region GetOverrideString

        protected string GetOverrideString()
        {
            string s = null;

            if (String.IsNullOrEmpty(Text) == false)
                s = Text;

            else if (Value.Equals(double.NaN) == true)
                s = _EmptyString ?? "";

            else if (Value < MinValue)
                s = _UnderRangeString;

            else if (Value > MaxValue)
                s = _OverRangeString ?? "";

            return (s);
        }

        #endregion

        #region ProcessTemplateText

        protected override void ProcessTemplateText(
            GaugeControl gauge, StringBuilder sb, string key, string data)
        {
            if (key.Equals("Value") == true)
            {
                sb.Append(string.IsNullOrEmpty(data)
                              ? _Value.ToString()
                              : String.Format("{0:" + data + "}", _Value));
            }
            else
            {
                base.ProcessTemplateText(gauge, sb, key, data);
            }
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeIndicator c = copy as GaugeIndicator;

            if (c != null)
            {
                base.CopyToItem(c);

                c.AutoSize = _AutoSize;

                if (_BackColor != null)
                    c.BackColor = (GradientFillColor)_BackColor.Clone();

                c.DampeningSweepTime = _DampeningSweepTime;
                c.EmptyString = _EmptyString;

                if (_Font != null)
                    c.Font = (Font)_Font.Clone();

                c.Location = _Location;
                c.MaxValue = _MaxValue;
                c.MinValue = _MinValue;
                c.OverRangeString = _OverRangeString;
                c.RefreshRate = _RefreshRate;
                c.Size = _Size;
                c.Text = _Text;
                c.UnderRangeString = _UnderRangeString;
                c.UnderScale = _UnderScale;
                c.Value = _Value;
            }
        }

        #endregion
    }
}
