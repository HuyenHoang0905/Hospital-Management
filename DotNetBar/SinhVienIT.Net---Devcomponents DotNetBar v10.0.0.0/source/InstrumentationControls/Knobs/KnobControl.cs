using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Defines Knob instrumentation control.
    /// </summary>
    [ToolboxItem(true)]
    [Designer("DevComponents.Instrumentation.Design.KnobControlDesigner, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5")]
    public partial class KnobControl : Control, IDisposable
    {
        #region Delegates

        private delegate void InternalRender(PaintEventArgs e);

        public delegate void PreRenderEventHandler(object sender, PreRenderEventArgs e);
        public delegate void PostRenderEventHandler(object sender, PostRenderEventArgs e);
        public delegate void RenderFocusRectEventHandler(object sender, RenderFocusRectEventArgs e);

        #endregion

        #region Events

        #region RenderEvent

        #region PreRender

        /// <summary>
        /// Event raised just before the ZoneIndicator is rendered
        /// </summary>
        [Description("Event raised just before the ZoneIndicator is rendered ")]
        public event PreRenderEventHandler PreRenderZoneIndicator;

        /// <summary>
        /// Event raised just before the Minor Tick Marks are rendered
        /// </summary>
        [Description("Event raised just before the Minor Tick Marks are rendered ")]
        public event PreRenderEventHandler PreRenderTickMinor;

        /// <summary>
        /// Event raised just before the Major Tick Marks are rendered
        /// </summary>
        [Description("Event raised just before the Major Tick Marks are rendered ")]
        public event PreRenderEventHandler PreRenderTickMajor;

        /// <summary>
        /// Event raised just before the Tick Labels are rendered
        /// </summary>
        [Description("Event raised just before the Tick Labels are rendered ")]
        public event PreRenderEventHandler PreRenderTickLabel;

        /// <summary>
        /// Event raised just before the KnobFace is rendered
        /// </summary>
        [Description("Event raised just before the KnobFace is rendered ")]
        public event PreRenderEventHandler PreRenderKnobFace;

        /// <summary>
        /// Event raised just before the KnobIndicator is rendered
        /// </summary>
        [Description("Event raised just before the KnobIndicator is rendered ")]
        public event PreRenderEventHandler PreRenderKnobIndicator;

        #endregion

        #region PostRender

        /// <summary>
        /// Event raised right after the ZoneIndicator is rendered
        /// </summary>
        [Description("Event raised right after the ZoneIndicator is rendered ")]
        public event PostRenderEventHandler PostRenderZoneIndicator;

        /// <summary>
        /// Event raised right after the Minor Tick Marks are rendered
        /// </summary>
        [Description("Event raised right after the Minor Tick Marks are rendered ")]
        public event PostRenderEventHandler PostRenderTickMinor;

        /// <summary>
        /// Event raised right after the Major Tick Marks are rendered
        /// </summary>
        [Description("Event raised right after the Major Tick Marks are rendered ")]
        public event PostRenderEventHandler PostRenderTickMajor;

        /// <summary>
        /// Event raised right after the Tick Labels are rendered
        /// </summary>
        [Description("Event raised right after the Tick Labels are rendered ")]
        public event PostRenderEventHandler PostRenderTickLabel;

        /// <summary>
        /// Event raised right after the KnobFace is rendered
        /// </summary>
        [Description("Event raised right after the KnobFace is rendered ")]
        public event PostRenderEventHandler PostRenderKnobFace;

        /// <summary>
        /// Event raised right after the KnobIndicator is rendered
        /// </summary>
        [Description("Event raised right after the KnobIndicator is rendered ")]
        public event PostRenderEventHandler PostRenderKnobIndicator;

        #endregion

        #region RenderFocusRect

        /// <summary>
        /// Event raised when the Focus Rectangle needs rendered
        /// </summary>
        [Description("Event raised when the Focus Rectangle needs rendered ")]
        public event RenderFocusRectEventHandler RenderFocusRect;

        #endregion

        #endregion

        #region ValueChanged

        /// <summary>
        /// Event raised when the Focus Rectangle needs rendered
        /// </summary>
        [Description("Event raised when the Knob Value has changed.")]
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion

        #region ColorTableChanged

        /// <summary>
        /// Event raised when the Knob ColorTable has changed
        /// </summary>
        [Description("Event raised when the Knob ColorTable has changed.")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #endregion

        #region Constants

        internal int MinKnobSize = 60;

        #endregion

        #region Private variables

        private BaseKnob _Knob;                             // Base knob
        private eKnobStyle _KnobStyle = eKnobStyle.Style1;  // Default style

        private decimal _Value;                             // Knob Value
        private decimal _MajorTickAmount = 10;              // Major tick amount
        private decimal _MinorTickAmount = 2;               // Minor tick amount

        private bool _AllowDecimalValueSelection = true;    // Allow decimal values or not
        private int _SelectionDecimals = 1;

        private decimal _MinValue;                          // Minimum allowable value
        private decimal _MaxValue = 100;                    // Maximum allowable value

        private int _StartAngle = 130;                      // Starting knob angle
        private int _SweepAngle = 280;                      // Sweep angle (start + sweep == end)

        private int _MinZonePercentage = 80;                // Min zone percentage
        private int _MaxZonePercentage = 20;                // Max zone percentage

        private bool _KnobRotating;                         // Is the knob being rotated by the mouse? 
        private decimal _SaveValue;                         // Rotation cancel value

        private bool _FocusCuesEnabled = true;              // Are Focus cues enabled?
        private bool _ShowTickLabels = true;                // Show tick labels?
        private bool _ReadOnly;                             // ReadOnly?

        private KnobColorTable _KnobColor;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public KnobControl()
        {
            // Initialize our control

            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, true);

            // Set our Knob Style and default KnobColorTable 

            SetKnobStyle(eKnobStyle.Style1);

            KnobColor = new KnobColorTable();

            // Hook on to our events

            HookEvents(true);
        }

        #region Public properties

        #region AllowDecimalValueSelection

        /// <summary>
        /// Gets and sets whether Values with decimals can be used
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether Values with decimals can be used")]
        public bool AllowDecimalValueSelection
        {
            get { return (_AllowDecimalValueSelection); }
            set { _AllowDecimalValueSelection = value; InvalidateKnob(); }
        }

        #endregion

        #region FocusCuesEnabled

        /// <summary>
        /// Gets or sets whether control displays focus cues when focused.
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether control displays focus cues when focused.")]
        public bool FocusCuesEnabled
        {
            get { return _FocusCuesEnabled; }

            set
            {
                _FocusCuesEnabled = value;

                if (this.Focused)
                    InvalidateKnob();
            }
        }

        #endregion

        #region Font

        /// <summary>
        /// Gets or sets the KnobControl Font
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the KnobControl Font")]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; InvalidateKnob(); }
        }

        #endregion

        #region KnobColor

        /// <summary>
        /// Gets and sets the display Colors of the KnobControl
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the display Colors of the KnobControl")]
        public KnobColorTable KnobColor
        {
            get { return (_KnobColor); }

            set
            {
                if (_KnobColor != null)
                    _KnobColor.ColorTableChanged -= KnobColor_ColorTableChanged;

                _KnobColor = value;

                if (_KnobColor != null)
                    _KnobColor.ColorTableChanged += KnobColor_ColorTableChanged;

                OnColorTableChanged();
                
                InvalidateKnob();
            }
        }

        /// <summary>
        /// Gets whether property should be serialized
        /// </summary>
        public bool ShouldSerializeKnobColor()
        {
            return (_KnobColor.IsEmpty == false);
        }

        /// <summary>
        /// Resets property to it's default value
        /// </summary>
        public void ResetKnobColor()
        {
            KnobColor = KnobColorTable.Empty;
        }

        #endregion

        #region KnobStyle

        /// <summary>
        /// Gets and sets the display style of the knob
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(eKnobStyle.Style1)]
        [Description("Indicates the display style of the knob")]
        public eKnobStyle KnobStyle
        {
            get { return _KnobStyle; }

            set
            {
                if (_KnobStyle != value)
                {
                    SetKnobStyle(value);

                    InvalidateKnob();
                }
            }
        }

        #region SetKnobStyle

        /// <summary>
        /// Sets the display style for the control
        /// </summary>
        /// <param name="style"></param>
        private void SetKnobStyle(eKnobStyle style)
        {
            switch (style)
            {
                case eKnobStyle.Style1:
                    _Knob = new KnobStyle1(this);
                    break;

                case eKnobStyle.Style2:
                    _Knob = new KnobStyle2(this);
                    break;

                case eKnobStyle.Style3:
                    _Knob = new KnobStyle3(this);
                    break;

                default:
                    _Knob = new KnobStyle4(this);
                    break;
            }

            _KnobStyle = style;
        }

        #endregion

        #endregion

        #region MinZonePercentage

        /// <summary>
        /// Get and sets the numeric value that
        /// represents the MinZoneIndicator percentage
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(80)]
        [Description("Indicates numeric value that represents the MinZoneIndicator percentage")]
        public int MinZonePercentage
        {
            get { return (_MinZonePercentage); }

            set
            {
                if (_MinZonePercentage != value)
                {
                    if (value < 0)
                    {
                        const string s = "Value must be non-negative.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    if (value > 100)
                    {
                        const string s = "Value must be less or equal to 100.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    _MinZonePercentage = value;

                    InvalidateKnob();
                }
            }
        }

        #endregion

        #region MajorTickAmount

        /// <summary>
        /// Gets and sets the amount each
        /// major tick represents on the knob
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the amount each major tick represents on the knob")]
        public decimal MajorTickAmount
        {
            get { return _MajorTickAmount; }

            set
            {
                if (_MajorTickAmount != value)
                {
                    if (value < 0)
                    {
                        const string s = "Value must be non-negative.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    _MajorTickAmount = value;

                    InvalidateKnob();
                }
            }
        }

        /// <summary>
        /// Gets whether property should be serialized
        /// </summary>
        public bool ShouldSerializeMajorTickAmount()
        {
            return (_MajorTickAmount != 10);
        }

        /// <summary>
        /// Resets property to it's default value
        /// </summary>
        public void ResetMajorTickAmount()
        {
            MajorTickAmount = 10;
        }

        #endregion

        #region MaxValue

        /// <summary>
        /// Get and sets the upper limit of the knob range
        /// </summary>
        [Browsable(true), Category("Behavior")]
        [Description("Indicates the upper limit of the knob range")]
        public decimal MaxValue
        {
            get
            {
                return ((_AllowDecimalValueSelection == false) ?
                    Math.Round(_MaxValue) : Math.Round(_MaxValue, _SelectionDecimals));
            }

            set
            {
                if (_MaxValue != value)
                {
                    if (value <= MinValue)
                        MinValue = value - 1;
                    
                    _MaxValue = value;

                    // Keep the current Value within the
                    // users MinValue and MaxValue range
                    
                    if (_Value > _MaxValue)
                        _Value = _MaxValue;

                    InvalidateKnob();
                }
            }
        }

        /// <summary>
        /// Gets whether property should be serialized
        /// </summary>
        public bool ShouldSerializeMaxValue()
        {
            return (_MaxValue != 100);
        }

        /// <summary>
        /// Resets property to it's default value
        /// </summary>
        public void ResetMaxValue()
        {
            MaxValue = 100;
        }

        #endregion

        #region MinorTickAmount

        /// <summary>
        /// Gets and sets the amount each
        /// minor tick represents on the knob
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the amount each minor tick represents on the knob")]
        public decimal MinorTickAmount
        {
            get { return _MinorTickAmount; }

            set
            {
                if (_MinorTickAmount != value)
                {
                    if (value < 0)
                    {
                        const string s = "Value must be non-negative.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    _MinorTickAmount = value;

                    InvalidateKnob();
                }
            }
        }

        /// <summary>
        /// Gets whether property should be serialized
        /// </summary>
        public bool ShouldSerializeMinorTickAmount()
        {
            return (_MinorTickAmount != 2);
        }

        /// <summary>
        /// Resets property to it's default value
        /// </summary>
        public void ResetMinorTickAmount()
        {
            MinorTickAmount = 2;
        }

        #endregion

        #region MinValue

        /// <summary>
        /// Gets and sets the lower limit of the knob range
        /// </summary>
        [Browsable(true), Category("Behavior")]
        [Description("Indicates the lower limit of the knob range")]
        public decimal MinValue
        {
            get
            {
                return ((_AllowDecimalValueSelection == false) ?
                    Math.Round(_MinValue) : Math.Round(_MinValue, _SelectionDecimals));
            }

            set
            {
                if (_MinValue != value)
                {
                    if (value >= MaxValue)
                        MaxValue = value + 1;

                    _MinValue = value;

                    // Keep the current Value within the
                    // users MinValue and MaxValue range

                    if (_Value < _MinValue)
                        Value = _MinValue;

                    InvalidateKnob();
                }
            }
        }

        /// <summary>
        /// Gets whether property should be serialized
        /// </summary>
        public bool ShouldSerializeMinValue()
        {
            return (_MinValue != 100);
        }

        /// <summary>
        /// Resets property to it's default value
        /// </summary>
        public void ResetMinValue()
        {
            MinValue = 0;
        }

        #endregion

        #region MaxZonePercentage

        /// <summary>
        /// Get and sets the numeric value that
        /// represents the MaxZoneIndicator percentage
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(20)]
        [Description("Indicates numeric value that represents the MaxZoneIndicator percentage")]
        public int MaxZonePercentage
        {
            get { return (_MaxZonePercentage); }

            set
            {
                if (_MaxZonePercentage != value)
                {
                    if (value < 0)
                    {
                        const string s = "Value must be non-negative.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    if (value > 100)
                    {
                        const string s = "Value must be less or equal to 100.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    _MaxZonePercentage = value;

                    InvalidateKnob();
                }
            }
        }

        #endregion

        #region ReadOnly

        /// <summary>
        /// Get and sets the (user access) ReadOnly state of the control
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(false)]
        [Description("Indicates the (user access) ReadOnly state of the control")]
        public bool ReadOnly
        {
            get { return (_ReadOnly); }
            set { _ReadOnly = value; }
        }

        #endregion

        #region SelectionDecimals

        /// <summary>
        /// Gets and sets the number of selection decimals. This is used in conjunction with AllowDecimalValueSelection.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(1)]
        [Description("Indicates the number of selection decimals. This is used in conjunction with AllowDecimalValueSelection.")]
        public int SelectionDecimals
        {
            get { return (_SelectionDecimals); }
            set { _SelectionDecimals = value; InvalidateKnob(); }
        }

        #endregion

        #region ShowTickLabels

        [Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether control displays the MajorTick text labels.")]
        public bool ShowTickLabels
        {
            get { return (_ShowTickLabels); }
            set { _ShowTickLabels = value; InvalidateKnob(); }
        }

        #endregion

        #region StartAngle

        /// <summary>
        /// Gets and sets the angle measured
        /// from the x-axis to the starting point of the gauge zone
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(130)]
        [Description("Angle measured from the x-axis to the starting point of the gauge zone")]
        public int StartAngle
        {
            get { return _StartAngle; }

            set
            {
                if (_StartAngle != value)
                {
                    if (value < 0 || value > 360)
                    {
                        const string s = "Value must be between 0 and 360, inclusive.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    _StartAngle = value;

                    InvalidateKnob();
                }
            }
        }

        #endregion

        #region SweepAngle

        /// <summary>
        /// Get and sets the angle measured from the StartAngle to the ending point of the gauge zone.
        /// Positive values signify clockwise rotation; negative values, counter-clockwise rotation.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(280)]
        [Description("Angle measured from the StartAngle to the ending point of the gauge zone. Positive values signify clockwise rotation; negative values, counter-clockwise rotation.")]
        public int SweepAngle
        {
            get { return _SweepAngle; }

            set
            {
                if (_SweepAngle != value)
                {
                    int n = Math.Abs(value);

                    if (n > 360)
                    {
                        const string s = "Value must be between -360 and 360, inclusive.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    _SweepAngle = value;

                    InvalidateKnob();
                }
            }
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets and sets the numeric value that
        /// represents the current position of the knob selector
        /// </summary>
        [Browsable(true), Category("Behavior")]
        [Description("Indicates numeric value that represents the current position of the knob selector")]
        public decimal Value
        {
            get
            {
                return (_Value);
            }

            set
            {
                if (_Value != value)
                {
                    value = (_AllowDecimalValueSelection == false) ?
                        Math.Round(value) : (Math.Round(value, _SelectionDecimals));

                    if (value < MinValue)
                    {
                        const string s = "Value must be greater or equal than the Minimum property value.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    if (value > MaxValue)
                    {
                        const string s = "Value must be greater or equal than the Maximum property value.";
                        throw new ArgumentOutOfRangeException(s);
                    }

                    decimal oldValue = _Value;
                    _Value = value;

                    OnValueChanged(oldValue, value);

                    InvalidateKnob();
                }
            }
        }

        /// <summary>
        /// Gets whether property should be serialized
        /// </summary>
        public bool ShouldSerializeValue()
        {
            return (Value != 0);
        }

        /// <summary>
        /// Resets property to it's default value
        /// </summary>
        public void ResetValue()
        {
            Value = 0;
        }

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks or unhooks needed events
        /// </summary>
        /// <param name="hook">true to hook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
                Resize += KnobControl_Resize;
            else
                Resize -= KnobControl_Resize;
        }

        #endregion

        #region Event processing

        #region KnobControl_Resize

        /// <summary>
        /// Control Resize processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KnobControl_Resize(object sender, EventArgs e)
        {
            if (Width < MinKnobSize)
                Width = MinKnobSize;

            if (Height < MinKnobSize)
                Height = MinKnobSize;

            _Knob.ResetKnob();
        }

        #endregion

        #region KnobColor_ColorTableChanged

        void KnobColor_ColorTableChanged(object sender, EventArgs e)
        {
            Invalidate();

            OnColorTableChanged();
        }

        #endregion

        #endregion

        #region OnGotFocus

        /// <summary>
        /// Handles control GotFocus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (_FocusCuesEnabled == true)
                InvalidateKnob();

            base.OnGotFocus(e);
        }

        #endregion

        #region OnLostFocus

        /// <summary>
        /// Handles control GotFocus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (_FocusCuesEnabled == true)
                InvalidateKnob();

            base.OnLostFocus(e);
        }

        #endregion

        #region OnValueChanged

        /// <summary>
        /// Called when the control 'Value' is changed
        /// </summary>
        protected virtual void OnValueChanged(decimal oldValue, decimal newValue)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region OnColorTableChanged

        /// <summary>
        /// Called when the Knob ColorTable has changed
        /// </summary>
        private void OnColorTableChanged()
        {
            if (ColorTableChanged != null)
                ColorTableChanged(this, EventArgs.Empty);
        }

        #endregion

        #region OnPaint

        #region OnPaint

        /// <summary>
        /// Paints the contents of the control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Paint our background

            using (Brush br = new SolidBrush(BackColor))
            {
                Rectangle r = Bounds;
                r.Location = Point.Empty;

                g.FillRectangle(br, r);
            }

            // Initialize the rendering process
            // and render each individual part

            _Knob.InitRender(e);

            RenderZoneIndicator(e);
            RenderTickMinor(e);
            RenderTickMajor(e);
            RenderTickLabel(e);
            RenderKnobFace(e);
            RenderKnobIndicator(e);
            RenderFocusRectangle(e);

            g.SmoothingMode = sm;
        }

        #endregion

        #region RenderZoneIndicator

        /// <summary>
        /// RenderZoneIndicator
        /// </summary>
        /// <param name="e"></param>
        private void RenderZoneIndicator(PaintEventArgs e)
        {
            RenderPart(e, _Knob.ZoneIndicatorBounds,
                PreRenderZoneIndicator, PostRenderZoneIndicator, _Knob.RenderZoneIndicator);
        }

        #endregion

        #region RenderTickMinor

        /// <summary>
        /// RenderTickMinor
        /// </summary>
        /// <param name="e"></param>
        private void RenderTickMinor(PaintEventArgs e)
        {
            RenderPart(e, _Knob.ZoneIndicatorBounds,
                PreRenderTickMinor, PostRenderTickMinor, _Knob.RenderTickMinor);
        }

        #endregion

        #region RenderTickMajor

        /// <summary>
        /// RenderTickMajor
        /// </summary>
        /// <param name="e"></param>
        private void RenderTickMajor(PaintEventArgs e)
        {
            RenderPart(e, _Knob.ZoneIndicatorBounds,
                PreRenderTickMajor, PostRenderTickMajor, _Knob.RenderTickMajor);
        }

        #endregion

        #region RenderTickLabel

        /// <summary>
        /// RenderTickLabel
        /// </summary>
        /// <param name="e"></param>
        private void RenderTickLabel(PaintEventArgs e)
        {
            if (_ShowTickLabels == true)
            {
                RenderPart(e, _Knob.TickLabelBounds,
                           PreRenderTickLabel, PostRenderTickLabel, _Knob.RenderTickLabel);
            }
        }

        #endregion

        #region RenderKnobFace

        /// <summary>
        /// RenderKnobFace
        /// </summary>
        /// <param name="e"></param>
        private void RenderKnobFace(PaintEventArgs e)
        {
            RenderPart(e, _Knob.KnobFaceBounds,
                PreRenderKnobFace, PostRenderKnobFace, _Knob.RenderKnobFace);
        }

        #endregion

        #region RenderKnobIndicator

        /// <summary>
        /// RenderKnobIndicator
        /// </summary>
        /// <param name="e"></param>
        private void RenderKnobIndicator(PaintEventArgs e)
        {
            RenderPart(e, _Knob.KnobIndicatorBounds,
                PreRenderKnobIndicator, PostRenderKnobIndicator, _Knob.RenderKnobIndicator);

        }

        #endregion

        #region RenderFocusRectangle

        /// <summary>
        /// RenderFocusRectangle
        /// </summary>
        /// <param name="e"></param>
        private void RenderFocusRectangle(PaintEventArgs e)
        {
            if (_FocusCuesEnabled == true && Focused == true)
            {
                if (RenderFocusRect != null)
                {
                    RenderFocusRectEventArgs ev =
                        new RenderFocusRectEventArgs(e.Graphics, e.ClipRectangle, _Knob.FocusRectBounds);

                    RenderFocusRect(this, ev);
                }
                else
                {
                    _Knob.RenderFocusRect(e);
                }
            }
        }

        #endregion

        #region RenderPart

        /// <summary>
        /// Renders an individual 'part' of the control knob
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bounds">Bounding rectangle</param>
        /// <param name="preRender">User PreRender callout</param>
        /// <param name="postRender">User PostRender callout</param>
        /// <param name="internalRender">Internal render callout</param>
        private void RenderPart(PaintEventArgs e, Rectangle bounds,
            PreRenderEventHandler preRender, PostRenderEventHandler postRender,
            InternalRender internalRender)
        {
            bool cancel = false;

            // If the user wants to PreRender the control
            // then callout to their associated handler

            if (preRender != null)
            {
                PreRenderEventArgs ev =
                    new PreRenderEventArgs(e.Graphics, e.ClipRectangle, bounds);

                preRender(this, ev);

                cancel = ev.Cancel;
            }

            // If our internal rendering was not canceled
            // by the user (via the PreRender event), then
            // perform our own rendering of the control

            if (cancel == false)
            {
                internalRender(e);

                // If the user cancelled our internal rendering
                // then their is no need to PostRender, but if not
                // then we need to give them another shot at the
                // rendering process

                if (postRender != null)
                {
                    PostRenderEventArgs ev =
                        new PostRenderEventArgs(e.Graphics, e.ClipRectangle, bounds);

                    postRender(this, ev);
                }
            }
        }

        #endregion

        #endregion

        #region Keyboard support

        #region IsInputKey

        /// <summary>
        /// Routine to signify that the directional keys
        /// (up/down/left/right) are special input keys
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the key is an input key</returns>
        protected override bool IsInputKey(Keys key)
        {
            if (_ReadOnly == true)
                return (false);

            switch (key)
            {
                // Incrementing keys

                case Keys.Up:
                case Keys.Right:
                case Keys.End:
                case Keys.PageUp:

                // Decrementing keys

                case Keys.Down:
                case Keys.Left:
                case Keys.Home:
                case Keys.PageDown:

                    return (true);
            }

            return (base.IsInputKey(key));
        }

        #endregion

        #region OnKeyDown

        /// <summary>
        /// Handles knob rotation via key input
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_ReadOnly == false)
            {
                switch (e.KeyCode)
                {
                    case Keys.Home:
                        Value = MinValue;
                        break;

                    case Keys.End:
                        Value = MaxValue;
                        break;

                    case Keys.Right:
                    case Keys.Up:
                        AdjustValue(GetDelta(ModifierKeys, true));
                        break;

                    case Keys.Left:
                    case Keys.Down:
                        AdjustValue(GetDelta(ModifierKeys, false));
                        break;

                    case Keys.PageUp:
                        AdjustValue(GetDelta(Keys.Control | Keys.Shift, true));
                        break;

                    case Keys.PageDown:
                        AdjustValue(GetDelta(Keys.Control | Keys.Shift, false));
                        break;
                }
            }
        }

        #endregion

        #region AdjustValue

        /// <summary>
        /// Adjusts the Value via the keyboard or mouse
        /// </summary>
        /// <param name="delta"></param>
        private void AdjustValue(decimal delta)
        {
            decimal value = Value + delta;

            if (value < MinValue)
                value = MinValue;

            if (value > MaxValue)
                value = MaxValue;

            Value = value;
        }

        #endregion

        #region GetDelta

        /// <summary>
        /// Calculates the delta adjustment for the
        /// pos or neg increment with respect to the
        /// supplied modifiers (Control/Shift)
        /// </summary>
        /// <param name="mods">Keys.Control and/or Keys.Shift</param>
        /// <param name="inc">Denotes whether to increment or decrement</param>
        /// <returns>Signed delta value</returns>
        private decimal GetDelta(Keys mods, bool inc)
        {
            decimal delta = 0;

            // If the Control Key is pressed then the user is
            // wanting to jump by tick amounts (either Minor or Major)

            if ((mods & Keys.Control) == Keys.Control)
            {
                // Shift Key denotes Major tick changes, no shift
                // key denotes Minor tick changes

                delta = ((mods & Keys.Shift) == Keys.Shift) ?
                    MajorTickAmount : MinorTickAmount;

                // Make our first adjustment align
                // on a tick boundary

                if (delta > 0)
                {
                    decimal dv = Value % delta;

                    if (dv != 0)
                        delta = inc ? delta - dv : dv;

                    if (AllowDecimalValueSelection == false)
                        delta = Math.Round(delta, 1);
                }
            }

            // Adjust delta to a valid unit

            if (delta == 0)
            {
                // No tick change, so just advance
                // by single units

                delta = (AllowDecimalValueSelection == true) ?
                    (decimal) (1 / Math.Pow(10, SelectionDecimals)) : 1;
            }

            return (inc == true ? delta : -delta);
        }

        #endregion

        #endregion

        #region Mouse support

        #region OnMouseDown

        /// <summary>
        /// MouseDown processing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_ReadOnly == false)
            {
                // Check to see if the user's MouseDown
                // was inside our knob control

                Point pt = new Point(e.X, e.Y);

                if (DesignMode == false && _Knob.PointInControl(pt) == true)
                {
                    // Save the current control Value incase
                    // the user cancels the operation

                    _SaveValue = Value;

                    // Set the new knob Value and set our state
                    // to signify we are rotating the knob

                    Value = _Knob.GetValueFromPoint(pt);

                    Cursor = Cursors.Hand;

                    _KnobRotating = true;

                    // Give the control the focus

                    Focus();
                }
            }
        }

        #endregion

        #region OnMouseUp

        /// <summary>
        /// MouseUp processing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // If the user is actively was rotating the knob
            // then reset our state processing accordingly

            if (_KnobRotating == true)
            {
                Cursor = Cursors.Default;

                _KnobRotating = false;
            }
        }

        #endregion

        #region OnMouseMove

        /// <summary>
        /// MouseMove processing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Process the mouse movement if the user
            // is actively rotating the knob

            if (_KnobRotating == true)
            {
                Point pt = new Point(e.X, e.Y);

                // If the mouse is within the knob control
                // then set the new Value accordingly, otherwise
                // restore the Value to our previous Value

                if (_Knob.PointInControl(pt) == true)
                {
                    Value = _Knob.GetValueFromPoint(pt);
                    Cursor = Cursors.Hand;
                }
                else
                {
                    Value = _SaveValue;
                    Cursor = Cursors.Default;
                }
            }
        }

        #endregion

        #region OnMouseWheel

        /// <summary>
        /// Handles MouseWheel events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (_ReadOnly == false)
            {
                // Adjust the Value based upon the
                // MouseWheel delta value

                AdjustValue(GetDelta(ModifierKeys, e.Delta > 0));
            }

            // Add call the base handler

            base.OnMouseWheel(e);
        }

        #endregion

        #endregion

        #region InvalidateKnob

        /// <summary>
        /// Invalidates the knob
        /// </summary>
        private void InvalidateKnob()
        {
            // Signal that the knob definition changed and
            // that a reconfigure of the control will need
            // to be done

            _Knob.ResetKnob();

            // Invalidate the control

            Invalidate();
        }

        #endregion

        #region GetValueFromPoint

        /// <summary>
        /// Gets the Knob value from the given Point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>Value</returns>
        public decimal GetValueFromPoint(Point pt)
        {
            return (_Knob.GetValueFromPoint(pt));
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// IDisposable.Dispose
        /// </summary>
        void IDisposable.Dispose()
        {
            HookEvents(false);

            Dispose();
        }

        #endregion
    }

    #region Enums

    /// <summary>
    /// Various Knob Control display styles
    /// </summary>
    public enum eKnobStyle
    {
        Style1,
        Style2,
        Style3,
        Style4
    }

    #endregion

    #region EventArgs

    #region PreRenderEventArgs

    /// <summary>
    /// PreRenderEventArgs - user cancellable
    /// </summary>
    public class PreRenderEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;         // Graphics object
        private Rectangle _ClipRectangle;   // Cliprect
        private Rectangle _Bounds;          // Bounding rectangle

        #endregion

        /// <summary>
        /// PreRenderEventArgs
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipRectangle"></param>
        /// <param name="bounds"></param>
        public PreRenderEventArgs(Graphics graphics, Rectangle clipRectangle, Rectangle bounds)
        {
            _Graphics = graphics;
            _ClipRectangle = clipRectangle;
            _Bounds = bounds;
        }

        #region Public properties

        /// <summary>
        /// Gets the event Graphics object
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the event ClipRectangle
        /// </summary>
        public Rectangle ClipRectangle
        {
            get { return (_ClipRectangle); }
        }

        /// <summary>
        /// Gets the event Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion
    }

    #endregion

    #region PostRenderEventArgs

    /// <summary>
    /// PostRenderEventArgs
    /// </summary>
    public class PostRenderEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;         // Graphics object
        private Rectangle _ClipRectangle;   // Cliprect
        private Rectangle _Bounds;          // Bounding rectangle

        #endregion

        /// <summary>
        /// PostRenderEventArgs
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipRectangle"></param>
        /// <param name="bounds"></param>
        public PostRenderEventArgs(Graphics graphics, Rectangle clipRectangle, Rectangle bounds)
        {
            _Graphics = graphics;
            _ClipRectangle = clipRectangle;
            _Bounds = bounds;
        }

        #region Public properties

        /// <summary>
        /// Gets the event Graphics object
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the event ClipRectangle
        /// </summary>
        public Rectangle ClipRectangle
        {
            get { return (_ClipRectangle); }
        }

        /// <summary>
        /// Gets the event Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion
    }

    #endregion

    #region RenderFocusRectEventArgs

    /// <summary>
    /// RenderFocusRectEventArgs
    /// </summary>
    public class RenderFocusRectEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;         // Graphics object
        private Rectangle _ClipRectangle;   // Cliprect
        private Rectangle _Bounds;          // Bounding rectangle

        #endregion

        /// <summary>
        /// RenderFocusRectEventArgs
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipRectangle"></param>
        /// <param name="bounds"></param>
        public RenderFocusRectEventArgs(Graphics graphics, Rectangle clipRectangle, Rectangle bounds)
        {
            _Graphics = graphics;
            _ClipRectangle = clipRectangle;
            _Bounds = bounds;
        }

        #region Public properties

        /// <summary>
        /// Gets the event Graphics object
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the event ClipRectangle
        /// </summary>
        public Rectangle ClipRectangle
        {
            get { return (_ClipRectangle); }
        }

        /// <summary>
        /// Gets the event Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion
    }

    #endregion

    #region ValueChangedEventArgs

    /// <summary>
    /// ValueChangedEventArgs
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        #region Private variables

        private decimal _OldValue;          // Old value
        private decimal _NewValue;          // New value

        #endregion

        /// <summary>
        /// ValueChangedEventArgs
        /// </summary>
        public ValueChangedEventArgs(decimal oldValue, decimal newValue)
        {
            _OldValue = oldValue;
            _NewValue = newValue;
        }

        #region Public properties

        /// <summary>
        /// Gets the old value
        /// </summary>
        public decimal OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// Gets the new value
        /// </summary>
        public decimal NewValue
        {
            get { return (_NewValue); }
        }

        #endregion
    }

    #endregion

    #endregion
}
