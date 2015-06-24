using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    public class NumericIndicator : GaugeIndicator
    {
        #region Private variables

        private NumericIndicatorStyle _Style;

        private int _NumberOfDigits;
        private int _NumberOfDecimals;
        private NumericElement[] _Digits;

        private Color _DigitColor;
        private Color _DigitDimColor;
        private Color _DecimalColor;
        private Color _DecimalDimColor;

        private float _SegmentWidth;
        private float _SeparatorWidth;
        private GradientFillColor _SeparatorColor;

        private DisplayPlacement _DigitPlacement;

        private string _FormatString;
        private Font _AbsFont;

        private bool _ShowDecimalPoint;
        private bool _ShowLeadingZeros;
        private bool _ShowDimColonPoints;
        private bool _ShowDimDecimalPoint;
        private bool _ShowDimSegments;

        private SignVisibility _SignVisibility;

        private float _ShearFactor;
        private Padding _Padding;

        private NumericRangeCollection _Ranges;

        #endregion

        public NumericIndicator()
        {
            InitIndicator();
        }

        #region InitIndicator

        private void InitIndicator()
        {
            _NumberOfDigits = 6;
            _NumberOfDecimals = 2;
            _DecimalColor = Color.Firebrick;
            _DigitColor = Color.SteelBlue;
            _DigitPlacement = DisplayPlacement.Far;
            _Padding = new Padding(0);
            _SegmentWidth = .5f;
            _SeparatorWidth = .01f;
            _ShearFactor = -0.1f;
            _ShowDimColonPoints = true;
            _ShowDimDecimalPoint = true;
            _ShowDimSegments = true;
            _SignVisibility = SignVisibility.WhenNegative;
            _Style = NumericIndicatorStyle.Mechanical;

            AllocateElements();
        }

        #endregion

        #region Public properties

        #region DecimalColor

        /// <summary>
        /// Gets or sets the default Decimal Color
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "FireBrick")]
        [Description("Indicates the default Decimal Color.")]
        public Color DecimalColor
        {
            get { return (_DecimalColor); }

            set
            {
                if (_DecimalColor != value)
                {
                    _DecimalColor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region DecimalDimColor

        /// <summary>
        /// Gets or sets the default Decimal Dim Color (Dim LED color)
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the default Decimal Dim Color (Dim LED color).")]
        public Color DecimalDimColor
        {
            get { return (_DecimalDimColor); }

            set
            {
                if (_DecimalDimColor != value)
                {
                    _DecimalDimColor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeDecimalDimColor()
        {
            return (_DecimalDimColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetDecimalDimColor()
        {
            _DecimalDimColor = Color.Empty;
        }
        #endregion

        #region DigitColor

        /// <summary>
        /// Gets or sets the default Digit Color
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "SteelBlue")]
        [Description("Indicates the default Digit Color.")]
        public Color DigitColor
        {
            get { return (_DigitColor); }

            set
            {
                if (_DigitColor != value)
                {
                    _DigitColor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region DigitDimColor

        /// <summary>
        /// Gets or sets the default Digit Dim Color (Dim LED color)
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the default Digit Dim Color (Dim LED color).")]
        public Color DigitDimColor
        {
            get { return (_DigitDimColor); }

            set
            {
                if (_DigitDimColor != value)
                {
                    _DigitDimColor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeDigitDimColor()
        {
            return (_DigitDimColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetDigitDimColor()
        {
            _DigitDimColor = Color.Empty;
        }

        #endregion

        #region DigitPlacement

        /// <summary>
        /// Gets or sets the Placement of the digits within the indicator
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(DisplayPlacement.Far)]
        [Description("Indicates the Alignment of the digits.")]
        public DisplayPlacement DigitPlacement
        {
            get { return (_DigitPlacement); }

            set
            {
                if (_DigitPlacement != value)
                {
                    _DigitPlacement = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Digits

        /// <summary>
        /// Gets the array of defined digits (units and decimals).
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NumericElement[] Digits
        {
            get { return (_Digits); }
        }

        #endregion

        #region FormatString

        /// <summary>
        /// Gets or sets the .Net format string to use when displaying the indicator value.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        [Description("Indicates the .Net format string to use when displaying the indicator value.")]
        [Editor("DevComponents.Instrumentation.Design.FormatStringEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public string FormatString
        {
            get { return (_FormatString); }

            set
            {
                if (_FormatString != value)
                {
                    _FormatString = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region NumberOfDecimals

        /// <summary>
        /// Gets or sets the number of decimals to display.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(2)]
        [Description("Determines the number of decimals to display.")]
        public int NumberOfDecimals
        {
            get { return (_NumberOfDecimals); }

            set
            {
                if (_NumberOfDecimals != value)
                {
                    if (value > _NumberOfDigits)
                        throw new ArgumentException("Number of decimals can not exceed NumberOfDigits.");

                    _NumberOfDecimals = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region NumberOfDigits

        /// <summary>
        /// Gets or sets the number of total digits to display (units and decimals).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(6)]
        [Description("Determines the number of total digits to display (units and decimals).")]
        public int NumberOfDigits
        {
            get { return (_NumberOfDigits); }

            set
            {
                if (_NumberOfDigits != value)
                {
                    if (value < _NumberOfDecimals)
                        throw new ArgumentException("Number of digits can not be less than NumberOfDecimals.");

                    _NumberOfDigits = value;

                    AbsFont = null;

                    AllocateElements();

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Padding

        /// <summary>
        /// Gets or sets the indicator padding.
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Description("Indicates the indicator padding.")]
        public Padding Padding
        {
            get { return (_Padding); }

            set
            {
                if (_Padding != value)
                {
                    _Padding = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializePadding()
        {
            return (_Padding.All != 0);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetPadding()
        {
            _Padding = new Padding(0);
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
        public NumericRangeCollection Ranges
        {
            get
            {
                if (_Ranges == null)
                {
                    _Ranges = new NumericRangeCollection();
                    _Ranges.CollectionChanged += NumericRanges_CollectionChanged;
                }

                return (_Ranges);
            }

            set
            {
                if (_Ranges != null)
                    _Ranges.CollectionChanged -= NumericRanges_CollectionChanged;

                _Ranges = value;

                if (_Ranges != null)
                    _Ranges.CollectionChanged += NumericRanges_CollectionChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region SegmentWidth

        /// <summary>
        /// Gets or sets the Digital segment width, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(.5f)]
        [Editor("DevComponents.Instrumentation.Design.RangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Digital element width, specified as a percentage.")]
        public float SegmentWidth
        {
            get { return (_SegmentWidth); }

            set
            {
                if (_SegmentWidth != value)
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentException("Value must be between 0 and +1.");

                    _SegmentWidth = value;

                    if (_Style != NumericIndicatorStyle.Mechanical)
                        SetRecalcSegments();

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region SeparatorColor

        /// <summary>
        /// Gets or sets the Digit Separator Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Digit Separator Color.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor SeparatorColor
        {
            get
            {
                if (_SeparatorColor == null)
                {
                    _SeparatorColor = new GradientFillColor();
                    _SeparatorColor.ColorTableChanged += Color_ColorTableChanged;
                }

                return (_SeparatorColor);
            }

            set
            {
                if (_SeparatorColor != null)
                    _SeparatorColor.ColorTableChanged -= Color_ColorTableChanged;

                _SeparatorColor = value;

                if (_SeparatorColor != null)
                    _SeparatorColor.ColorTableChanged += Color_ColorTableChanged;

                OnGaugeItemChanged();
            }
        }

        #endregion

        #region SeparatorWidth

        /// <summary>
        /// Gets or sets the Length of the Pin, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(.01f)]
        [Editor("DevComponents.Instrumentation.Design.RangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the width of the Mechanical Style Digits Separator, specified as a percentage.")]
        public float SeparatorWidth
        {
            get { return (_SeparatorWidth); }

            set
            {
                if (_SeparatorWidth != value)
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentException("Value must be between 0 and +1.");

                    _SeparatorWidth = value;

                    AbsFont = null;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ShearFactor

        /// <summary>
        /// Gets or Sets the shear coefficient for italicizing the Digits display
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(-0.1F)]
        [Description("Indicates the shear coefficient for italicizing the Digits display. Default is -0.1")]
        public float ShearFactor
        {
            get { return (_ShearFactor); }

            set
            {
                if (_ShearFactor != value)
                {
                    _ShearFactor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ShowDecimalPoint

        /// <summary>
        /// Gets or sets whether the decimal point should be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether the decimal point should be displayed.")]
        public bool ShowDecimalPoint
        {
            get { return (_ShowDecimalPoint); }

            set
            {
                if (_ShowDecimalPoint != value)
                {
                    _ShowDecimalPoint = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ShowDimColonPoints

        /// <summary>
        /// Gets or sets whether dim colon points should be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates whether dim colon points should be displayed.")]
        public bool ShowDimColonPoints
        {
            get { return (_ShowDimColonPoints); }

            set
            {
                if (_ShowDimColonPoints != value)
                {
                    _ShowDimColonPoints = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ShowDimDecimalPoint

        /// <summary>
        /// Gets or sets whether dim decimal points should be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates whether dim decimal points should be displayed.")]
        public bool ShowDimDecimalPoint
        {
            get { return (_ShowDimDecimalPoint); }

            set
            {
                if (_ShowDimDecimalPoint != value)
                {
                    _ShowDimDecimalPoint = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ShowDimSegments

        /// <summary>
        /// Gets or sets whether dim segments should be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates whether dim segments should be displayed.")]
        public bool ShowDimSegments
        {
            get { return (_ShowDimSegments); }

            set
            {
                if (_ShowDimSegments != value)
                {
                    _ShowDimSegments = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ShowLeadingZeros

        /// <summary>
        /// Gets or sets whether leading zeros should be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether leading zeros should be displayed.")]
        public bool ShowLeadingZeros
        {
            get { return (_ShowLeadingZeros); }

            set
            {
                if (_ShowLeadingZeros != value)
                {
                    _ShowLeadingZeros = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region SignVisibility

        /// <summary>
        /// Gets or sets when the +/- sign should be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(SignVisibility.WhenNegative)]
        [Description("Indicates when the +/- sign should be displayed.")]
        public SignVisibility SignVisibility
        {
            get { return (_SignVisibility); }

            set
            {
                if (_SignVisibility != value)
                {
                    _SignVisibility = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets or sets the Indicator Style
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(NumericIndicatorStyle.Mechanical)]
        [Description("Indicates the Indicator Style.")]
        public NumericIndicatorStyle Style
        {
            get { return (_Style); }

            set
            {
                if (_Style != value)
                {
                    _Style = value;

                    OnGaugeItemChanged(true);

                    AllocateElements();
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
                if (AutoSize == false || _NumberOfDigits <= 0)
                    return (Font);

                if (_AbsFont == null)
                    AbsFont = GetNewAbsFont();

                return (_AbsFont);
            }

            set
            {
                if (_AbsFont != null)
                    _AbsFont.Dispose();

                _AbsFont = value;
            }
        }

        #region GetNewAbsFont

        private Font GetNewAbsFont()
        {
            int sepWidth = AbsSeparatorWidth;
            int width = (Bounds.Width - (_Padding.Left + _Padding.Right + BackColor.BorderWidth * 2));
            int height = (Bounds.Height - (_Padding.Top + _Padding.Bottom + BackColor.BorderWidth * 2));

            int n = (width - (_NumberOfDigits - 1) * sepWidth) / _NumberOfDigits;

            if (n <= 0)
                n = 2;

            int low = 2;
            int high = 128;

            while (low <= high)
            {
                int mid = (low + high) / 2;

                using (Font font = new Font(Font.FontFamily, mid, Font.Style))
                {
                    Size size = TextRenderer.MeasureText(
                        string.IsNullOrEmpty(Text) ? "8" : "M", font);

                    if (size.Height > height || size.Width > n)
                        high = mid - 1;

                    else if (size.Height < height && size.Width < n)
                        low = mid + 1;

                    else
                    {
                        low = mid;
                        break;
                    }
                }
            }

            return (new Font(Font.FontFamily, low, Font.Style));
        }

        #endregion

        #endregion

        #region AbsSeparatorWidth

        internal int AbsSeparatorWidth
        {
            get { return ((int)(Bounds.Width * _SeparatorWidth)); }
        }

        #endregion

        #endregion

        #region Event processing

        #region Color_ColorTableChanged processing

        void Color_ColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #region NumericRange processing

        void NumericRanges_CollectionChanged(object sender, EventArgs e)
        {
            foreach (NumericRange range in _Ranges)
            {
                range.NumericIndicator = this;

                range.IndicatorRangeChanged -= NumericRange_ItemChanged;
                range.IndicatorRangeChanged += NumericRange_ItemChanged;
            }
        }

        void NumericRange_ItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #endregion

        #region SetRecalcSegments

        private void SetRecalcSegments()
        {
            if (_Digits != null)
            {
                foreach (DigitalElement item in _Digits)
                {
                    if (item != null)
                    {
                        item.NeedRecalcLayout = true;
                        item.NeedRecalcSegments = true;
                    }
                }
            }
        }

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                NeedRecalcLayout = true;

                AbsFont = null;

                if (_NumberOfDigits > 0)
                {
                    LayoutElements();
                    UpdateElements();
                }

                NeedRecalcLayout = false;
            }
        }

        #region AllocateElements

        private void AllocateElements()
        {
            NumericElement[] digit = new NumericElement[_NumberOfDigits];

            for (int i = 0; i < _NumberOfDigits; i++)
            {
                NumericElement d;

                switch (_Style)
                {
                    case NumericIndicatorStyle.Mechanical:
                        d = new MechanicalElement(this);
                        break;

                    case NumericIndicatorStyle.Digital7Segment:
                        d = new Seg7Element(this);
                        break;

                    default:
                        d = new Seg16Element(this);
                        break;
                }

                digit[i] = d;

                if (_Digits != null)
                {
                    if (i < _Digits.Length)
                        _Digits[i].CopyToItem(d);
                }
            }

            if (_Digits != null)
            {
                for (int i = _NumberOfDigits; i < _Digits.Length; i++)
                    _Digits[i].Dispose();
            }

            _Digits = digit;
        }

        #endregion

        #region LayoutElements

        private void LayoutElements()
        {
            if (_NumberOfDigits > 0)
            {
                int sepWidth = AbsSeparatorWidth;

                int width = Bounds.Width - (_Padding.Left + _Padding.Right + BackColor.BorderWidth * 2);
                int height = Bounds.Height - (_Padding.Top + _Padding.Bottom + BackColor.BorderWidth * 2);

                int shear = 0;

                if (Style != NumericIndicatorStyle.Mechanical)
                {
                    int w = width / _NumberOfDigits;

                    if (ShearFactor > 0)
                        width -= (int)(w * _SegmentWidth);

                    if (ShearFactor != 0)
                    {
                        shear = (int)(w * ShearFactor);
                        width -= Math.Abs(shear);
                    }

                }

                int ewidth = (width - (_NumberOfDigits - 1) * sepWidth) / _NumberOfDigits;
                int espread = width - ((ewidth + sepWidth) * _NumberOfDigits - sepWidth);

                Rectangle r = new Rectangle(
                    Bounds.X + BackColor.BorderWidth + _Padding.Left,
                    Bounds.Y + BackColor.BorderWidth + _Padding.Top, ewidth, height);

                if (shear < 0)
                    r.X += -shear;

                for (int i = 0; i < _NumberOfDigits; i++)
                {
                    r.Width = (ewidth + (i < espread ? 1 : 0));

                    _Digits[i].Bounds = r;

                    r.X += (r.Width + sepWidth);
                }
            }
        }

        #endregion

        #region UpdateElements

        private void UpdateElements()
        {
            bool numeric;
            string s = GetOutputString(out numeric);

            if (s != null)
            {
                if (_Style == NumericIndicatorStyle.Mechanical)
                    UpdateMechanicalElements(s, numeric);
                else
                    UpdateSegmentedElements(s, numeric);
            }
        }

        #region GetOutputString

        private string GetOutputString(out bool numeric)
        {
            numeric = false;

            string s = GetOverrideString();

            int nDigits;

            if (s != null)
                return (AlignOutputString(s, false, out nDigits));

            numeric = true;

            return (GetValueString());
        }

        #region GetValueString

        private string GetValueString()
        {
            int n = _NumberOfDigits;

            if ((_Style != NumericIndicatorStyle.Mechanical) || _ShowDecimalPoint == false)
                n++;

            string s = "";

            if (String.IsNullOrEmpty(_FormatString) == false)
            {
                try
                {
                    switch (_FormatString[0])
                    {
                        case 'X':
                        case 'x':
                            s = String.Format("{0:" + _FormatString + "}", (int)DValue);
                            break;

                        default:
                            s = String.Format("{0:" + _FormatString + "}", DValue);
                            break;
                    }
                }
                catch
                {
                }
            }
            else
            {
                string t = ".".PadLeft(Math.Max(1, n - _NumberOfDecimals),
                                       _ShowLeadingZeros ? '0' : '#') + "".PadLeft(_NumberOfDecimals, '0');

                s = String.Format("{0:" + t + "}", Math.Abs(DValue));

                s = ProcessSign(s);
            }

            int nDigits;

            s = AlignOutputString(s, _ShowDecimalPoint == false, out nDigits);

            if (s.Length > nDigits)
                return ("".PadRight(_NumberOfDigits, '-'));

            return (s);
        }

        #endregion

        #region ProcessSign

        private string ProcessSign(string s)
        {
            char sign = '\0';

            if (Value < 0)
            {
                if (_SignVisibility == SignVisibility.Always ||
                    _SignVisibility == SignVisibility.WhenNegative)
                {
                    sign = '-';
                }
            }
            else
            {
                if (_SignVisibility == SignVisibility.Always ||
                    _SignVisibility == SignVisibility.WhenPositive)
                {
                    sign = '+';
                }
            }

            if (sign != '\0')
            {
                if (_ShowLeadingZeros == true)
                {
                    if (s[0] != '0')
                        return ("".PadRight(_NumberOfDigits, '-'));

                    return (sign + s.Substring(1));
                }

                return (sign + s);
            }

            return (s);
        }

        #endregion

        #region AlignOutputString

        private string AlignOutputString(
            string s, bool removeDp, out int nDigits)
        {
            nDigits = _NumberOfDigits;

            if (string.IsNullOrEmpty(s) == false)
            {
                if (Style != NumericIndicatorStyle.Mechanical && removeDp == false)
                {
                    removeDp = true;

                    if (s[0] == '.' || s[0] == ':')
                        s = " " + s;
                }

                if (removeDp == true)
                {
                    foreach (char c in s)
                    {
                        if (c == '.' || c == ':')
                            nDigits++;
                    }
                }

                switch (_DigitPlacement)
                {
                    case DisplayPlacement.Center:
                        int n = (nDigits - s.Length)/2;

                        if (n > 0)
                            s = s.PadLeft(s.Length + n, ' ');
                        break;

                    case DisplayPlacement.Far:
                        if (s.Length < nDigits)
                            s = s.PadLeft(nDigits, ' ');
                        break;
                }
            }

            return (s);
        }

        #endregion

        #endregion

        #region UpdateMechanicalElements

        private void UpdateMechanicalElements(string s, bool numeric)
        {
            Color color, decimalColor;
            GetIndColors(out color, out decimalColor);

            int index = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (index >= _NumberOfDigits)
                    break;

                if (numeric == true && s[i] == '.')
                {
                    color = decimalColor;

                    if (_ShowDecimalPoint == false)
                        continue;
                }

                NumericElement element = _Digits[index++];

                element.Value = s[i];
                element.DigitColor = color;
            }

            for (int i = index; i < _NumberOfDigits; i++)
            {
                _Digits[i].Value = ' ';
                _Digits[i].DigitColor = color;
            }
        }

        #endregion

        #region UpdateSegmentedElements

        private void UpdateSegmentedElements(string s, bool numeric)
        {
            Color color, dimColor;
            Color decimalColor, decimalDimColor;
            GetIndColors(out color, out dimColor, out decimalColor, out decimalDimColor);

            int index = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '.')
                {
                    if (numeric == true)
                    {
                        color = decimalColor;
                        dimColor = decimalDimColor;

                        if (_ShowDecimalPoint == false)
                            continue;
                    }

                    if (index > 0)
                        _Digits[index - 1].DecimalPointOn = true;
                }
                else if (s[i] == ':')
                {
                    if (index > 0)
                        _Digits[index - 1].ColonPointsOn = true;
                }
                else
                {
                    if (index >= _NumberOfDigits)
                        break;

                    InitElement(_Digits[index++], s[i], color, dimColor);
                }
            }

            for (int i = index; i < _NumberOfDigits; i++)
                InitElement(_Digits[i], ' ', color, dimColor);
        }

        #region InitElement

        private void InitElement(NumericElement element,
            char value, Color color, Color dimColor)
        {
            element.Value = value;

            element.DigitColor = color;
            element.DigitDimColor = dimColor;

            element.ColonPointsOn = false;
            element.DecimalPointOn = false;

            element.ShowDimColonPoints = _ShowDimColonPoints;
            element.ShowDimDecimalPoint = _ShowDimDecimalPoint;
            element.ShowDimSegments = _ShowDimSegments;
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            RecalcLayout();

            Region rgn = g.Clip;

            Rectangle r = Bounds;
            r.Width++;
            r.Height++;

            g.SetClip(r, CombineMode.Intersect);

            if (GaugeControl.OnPreRenderIndicator(e, this) == false)
            {
                FillBackground(g, Bounds, GetBackColor());

                DrawElements(e);
                DrawBorder(e);

                GaugeControl.OnPostRenderIndicator(e, this);
            }

            g.Clip = rgn;
        }

        #region FillBackground

        private void FillBackground(Graphics g,
            Rectangle bounds, GradientFillColor fillColor)
        {
            Rectangle r = bounds;

            GradientFillType fillType =
                fillColor.End.IsEmpty ? GradientFillType.None : fillColor.GradientFillType;

            switch (fillType)
            {
                case GradientFillType.Angle:
                    using (Brush br = fillColor.GetBrush(r))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillRectangle(br, r);
                    }
                    break;

                case GradientFillType.StartToEnd:
                    using (Brush br = fillColor.GetBrush(r, 90))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillRectangle(br, r);
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

                        g.FillRectangle(br, bounds);
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

                        g.FillRectangle(br, bounds);
                    }
                    break;

                case GradientFillType.Center:
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddRectangle(r);

                        using (PathGradientBrush br = new PathGradientBrush(path))
                        {
                            br.CenterColor = fillColor.Start;
                            br.SurroundColors = new Color[] { fillColor.End };

                            g.FillRectangle(br, r);
                        }
                    }
                    break;

                default:
                    using (Brush br = new SolidBrush(fillColor.Start))
                        g.FillRectangle(br, r);

                    break;
            }
        }

        #endregion

        #region DrawElements

        private void DrawElements(PaintEventArgs e)
        {
            if (_Digits != null)
            {
                for (int i = 0; i < _Digits.Length; i++)
                {
                    if (GaugeControl.OnPreRenderIndicatorDigit(e, this, _Digits[i], i) == false)
                    {
                        _Digits[i].OnPaint(e);

                        GaugeControl.OnPostRenderIndicatorDigit(e, this, _Digits[i], i);
                    }
                }
            }
        }

        #endregion

        #region DrawBorder

        private void DrawBorder(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (_Digits != null && _Style == NumericIndicatorStyle.Mechanical)
            {
                int sepWidth = AbsSeparatorWidth;

                if (sepWidth > 0 && _SeparatorColor != null)
                {
                    for (int i = 0; i < _Digits.Length - 1; i++)
                    {
                        Rectangle r = _Digits[i].Bounds;

                        r.X = r.Right + 1;
                        r.Y = Bounds.Y + BackColor.BorderWidth;

                        r.Width = sepWidth;
                        r.Height = Bounds.Height - (BackColor.BorderWidth * 2);

                        FillBackground(g, r, _SeparatorColor);

                        if (_SeparatorColor.BorderWidth > 0)
                        {
                            using (Pen pen = new Pen(_SeparatorColor.BorderColor, _SeparatorColor.BorderWidth))
                            {
                                pen.Alignment = PenAlignment.Inset;

                                g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                                g.DrawLine(pen, r.Right, r.Y, r.Right, r.Bottom);
                            }
                        }
                    }
                }
            }

            if (BackColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(BackColor.BorderColor, BackColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    g.DrawRectangle(pen, Bounds);
                }
            }
        }

        #endregion

        #endregion

        #region GetBackColor

        private GradientFillColor GetBackColor()
        {
            if (_Ranges != null)
            {
                NumericRange range = _Ranges.GetValueRange(Value);

                if (range != null && (range.BackColor.IsEmpty == false))
                    return (range.BackColor);
            }

            return (BackColor);
        }

        #endregion

        #region GetIndColors

        private void GetIndColors(out Color digitColor, out Color decimalColor)
        {
            digitColor = _DigitColor;
            decimalColor = _DecimalColor;

            if (_Ranges != null)
            {
                foreach (NumericRange range in _Ranges)
                {
                    if (Value >= range.StartValue && Value <= range.EndValue)
                    {
                        if (range.DigitColor.IsEmpty == false)
                            digitColor = range.DigitColor;

                        if (range.DecimalColor.IsEmpty == false)
                            decimalColor = range.DecimalColor;

                        break;
                    }
                }
            }
        }

        private void GetIndColors(out Color digitColor, out Color digitDimColor, 
            out Color decimalColor,out Color decimalDimColor)
        {
            digitColor = _DigitColor;
            decimalColor = _DecimalColor;
            digitDimColor = _DigitDimColor;
            decimalDimColor = _DecimalDimColor;

            if (_Ranges != null)
            {
                foreach (NumericRange range in _Ranges)
                {
                    if (Value >= range.StartValue && Value <= range.EndValue)
                    {
                        if (range.DigitColor.IsEmpty == false)
                            digitColor = range.DigitColor;

                        if (range.DecimalColor.IsEmpty == false)
                            decimalColor = range.DecimalColor;

                        if (range.DigitDimColor.IsEmpty == false)
                            digitDimColor = range.DigitDimColor;

                        if (range.DecimalDimColor.IsEmpty == false)
                            decimalDimColor = range.DecimalDimColor;

                        break;
                    }
                }
            }
        }

        #endregion

        #region Refresh

        internal void Refresh()
        {
            if (NeedRecalcLayout == false)
                OnGaugeItemChanged(false);
        }

        #endregion

        #region RefreshElements

        public void RefreshElements()
        {
            if (_Digits != null)
            {
                foreach (NumericElement item in _Digits)
                    item.RefreshElements();

                Refresh();
            }
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            NumericIndicator c = copy as NumericIndicator;

            if (c != null)
            {
                base.CopyToItem(c);

                c.DecimalColor = _DecimalColor;
                c.DecimalDimColor = _DecimalDimColor;
                c.DigitColor = _DigitColor;
                c.DigitDimColor = _DigitDimColor;
                c.DigitPlacement = _DigitPlacement;
                c.FormatString = _FormatString;
                c.NumberOfDecimals = _NumberOfDecimals;
                c.NumberOfDigits = _NumberOfDigits;
                c.Padding = _Padding;

                if (_Ranges != null)
                    c.Ranges = (NumericRangeCollection)_Ranges.Clone();

                c.SegmentWidth = _SegmentWidth;

                if (_SeparatorColor != null)
                    c.SeparatorColor = (GradientFillColor)_SeparatorColor.Clone();

                c.SeparatorWidth = _SeparatorWidth;
                c.ShearFactor = _ShearFactor;
                c.ShowDecimalPoint = _ShowDecimalPoint;
                c.ShowDimColonPoints = _ShowDimColonPoints;
                c.ShowDimDecimalPoint = _ShowDimDecimalPoint;
                c.ShowDimSegments = _ShowDimSegments;
                c.ShowLeadingZeros = _ShowLeadingZeros;
                c.SignVisibility = _SignVisibility;
                c.Style = _Style;
            }
        }

        #endregion
    }

    #region Enums

    public enum NumericIndicatorStyle
    {
        Mechanical,
        Digital7Segment,
        Digital16Segment
    }

    public enum SignVisibility
    {
        Never,
        WhenNegative,
        WhenPositive,
        Always
    }

    #endregion

}
