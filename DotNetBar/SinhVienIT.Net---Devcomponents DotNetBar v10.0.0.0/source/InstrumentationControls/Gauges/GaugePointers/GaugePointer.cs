using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Collection of GaugePointers
    /// </summary>
    public class GaugePointerCollection : GenericCollection<GaugePointer>
    {
        #region ICloneable Members

        public override object Clone()
        {
            GaugePointerCollection copy = new GaugePointerCollection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(GaugePointerCollection copy)
        {
            foreach (GaugePointer item in this)
            {
                GaugePointer ic = new GaugePointer();

                item.CopyToItem(ic);

                copy.Add(ic);
            }
        }

        #endregion
    }

    public class GaugePointer : GaugeItem
    {
        #region Private variables

        private PointerStyle _Style;

        private BarPointerStyle _BarStyle;
        private GaugeMarkerStyle _MarkerStyle;
        private NeedlePointerStyle _NeedleStyle;
        private NeedlePointerCapStyle _CapStyle;
        private BulbStyle _BulbStyle;

        private DisplayPlacement _Placement;
        private float _ScaleOffset;

        private int _Radius;
        private int _Offset;
        private float _Width;
        private float _Length;

        private bool _UnderTickMarks;

        private GradientFillColor _FillColor;
        private GradientFillColor _FillColorEx;
        private GradientFillColor _CapFillColor;
        private GradientFillColor _CapFillColorEx;

        private bool _AllowUserChange;
        private Cursor _ChangeCursor;

        private double _Value;
        private double _DValue;
        private double _SnapInterval;
        private double _DampeningSweepTime;

        private Image _Image;

        private GradientFillColor _ThermoBackColor;
        private PointerOrigin _Origin;
        private double _OriginInterval;

        private float _BulbSize;
        private float _BulbOffset;

        private Image _CapImage;
        private bool _CapOnTop;
        private float _CapWidth;
        private float _CapOuterBevel;
        private float _CapInnerBevel;
        private bool _RotateCap;

        private bool _HonorMinPin;
        private bool _HonorMaxPin;

        private bool _Dampening;
        private double _DeltaValue;
        private long _DStartTicks;
        private double _DStartValue;
        private double _DEndValue;
        private bool _DShowMinLabel;
        private bool _DShowMaxLabel;
        private bool _DSlideScale;

        private GaugePointerRenderer _Renderer;

        private GaugeScale _Scale;

        private List<GaugeStrip> _RangeList;
        private List<GaugeStrip> _SectionList;
        private bool _InScale;

        private double _MouseDownAngle;
        private double _MouseDownRadians;

        #endregion

        public GaugePointer(GaugeScale scale)
            : this()
        {
            _Scale = scale;

            InitGaugePointer();
        }

        public GaugePointer()
        {
            InitGaugePointer();
        }

        #region InitGaugePointer

        private void InitGaugePointer()
        {
            Style = PointerStyle.Marker;

            FillColor = new GradientFillColor();

            _BarStyle = BarPointerStyle.Square;
            _Origin = PointerOrigin.Minimum;
            _OriginInterval = double.NaN;

            _BulbSize = .14f;
            _BulbStyle = BulbStyle.Bulb;

            _CapInnerBevel = .14f;
            _CapOuterBevel = .1f;
            _CapOnTop = true;
            _CapStyle = NeedlePointerCapStyle.Style2;
            _CapWidth = .3f;

            _ChangeCursor = Cursors.Hand;
            _DampeningSweepTime = 1;

            _HonorMaxPin = true;
            _HonorMinPin = true;

            _MarkerStyle = GaugeMarkerStyle.Triangle;
            _NeedleStyle = NeedlePointerStyle.Style1;

            _Placement = DisplayPlacement.Center;
            _Value = double.NaN;
            _Length = .14f;
            _Width = .14f;

            _DValue = double.NaN;
        }

        #endregion

        #region Public properties

        #region AllowUserChange

        /// <summary>
        /// Gets or sets whether the user can change the pointer interactively by the mouse
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(false)]
        [Description("Indicates whether the user can change the pointer interactively by the mouse.")]
        public bool AllowUserChange
        {
            get { return (_AllowUserChange); }

            set
            {
                if (_AllowUserChange != value)
                {
                    _AllowUserChange = value;

                    OnGaugeItemChanged();
                }
            }
        }

        #endregion

        #region BarStyle

        /// <summary>
        /// Gets or sets the Bar Style
        /// </summary>
        [Browsable(true), Category("Style Specific"), DefaultValue(BarPointerStyle.Square)]
        [Description("Indicates the Bar Style.")]
        public BarPointerStyle BarStyle
        {
            get { return (_BarStyle); }

            set
            {
                if (_BarStyle != value)
                {
                    _BarStyle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region BulbOffset

        /// <summary>
        /// Gets or sets the distance of the thermometer bulb from the start of the scale, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Style Specific"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the distance of the thermometer bulb from the start of the scale, measured as a percentage.")]
        public float BulbOffset
        {
            get { return (_BulbOffset); }

            set
            {
                if (_BulbOffset != value)
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentException("Value must be betweel 0 and +1.");

                    _BulbOffset = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region BulbSize

        /// <summary>
        /// Gets or sets the size of the thermometer bulb, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Style Specific"), DefaultValue(.14f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetPosRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the size of the thermometer bulb, measured as a percentage.")]
        public float BulbSize
        {
            get { return (_BulbSize); }

            set
            {
                if (_BulbSize != value)
                {
                    _BulbSize = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region BulbStyle

        /// <summary>
        /// Gets or sets the Thermometer Bulb Style
        /// </summary>
        [Browsable(true), Category("Style Specific"), DefaultValue(BulbStyle.Bulb)]
        [Description("Indicates the Thermometer Bulb Style.")]
        public BulbStyle BulbStyle
        {
            get { return (_BulbStyle); }

            set
            {
                if (_BulbStyle != value)
                {
                    _BulbStyle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region CapBounds

        /// <summary>
        /// Gets the CapBounds
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle CapBounds
        {
            get
            {
                if (_Renderer != null && _Renderer is GaugeNeedleRenderer)
                    return (((GaugeNeedleRenderer)_Renderer).CapBounds);

                return (Rectangle.Empty);
            }
        }

        #endregion

        #region CapFillColor

        /// <summary>
        /// Gets or sets the Needle Cap Fill Color
        /// </summary>
        [Browsable(true), Category("Needle Cap")]
        [Description("Indicates the Needle Cap Fill Color.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor CapFillColor
        {
            get
            {
                if (_CapFillColor == null)
                {
                    _CapFillColor = new GradientFillColor(Color.WhiteSmoke, Color.DimGray, 90);
                    _CapFillColor.BorderColor = Color.DimGray;
                    _CapFillColor.BorderWidth = 1;

                    _CapFillColor.ColorTableChanged += FillColorColorTableChanged;
                }

                return (_CapFillColor);
            }

            set
            {
                if (_CapFillColor != null)
                    _CapFillColor.ColorTableChanged -= FillColorColorTableChanged;

                _CapFillColor = value;

                if (_CapFillColor != null)
                    _CapFillColor.ColorTableChanged += FillColorColorTableChanged;

                UpdateFillColors();

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region CapFillColorEx

        /// <summary>
        /// Gets the range/section adjusted Pointer CapFillColor
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GradientFillColor CapFillColorEx
        {
            get { return (_CapFillColorEx ?? _CapFillColor); }
        }

        #endregion

        #region CapImage

        /// <summary>
        /// Gets or sets the Image to use for the Needle Cap
        /// </summary>
        [Browsable(true), Category("Needle Cap"), DefaultValue(null)]
        [Description("Indicates the Image to use for the Needle Cap.")]
        public Image CapImage
        {
            get { return (_CapImage); }

            set
            {
                if (_CapImage != value)
                {
                    _CapImage = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region CapInnerBevel

        /// <summary>
        /// Gets or sets the width of the needle cap's inner bevel, measured as a percentage of the cap width
        /// </summary>
        [Browsable(true), Category("Needle Cap"), DefaultValue(.14f)]
        [Description("Indicates the width of the needle cap's inner bevel, measured as a percentage of the cap width.")]
        [Editor("DevComponents.Instrumentation.Design.HalfRadiusRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float CapInnerBevel
        {
            get { return (_CapInnerBevel); }

            set
            {
                if (_CapInnerBevel != value)
                {
                    if (value < 0 || value > .5f)
                        throw new ArgumentException("Value must be between 0 and .5");

                    _CapInnerBevel = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region CapOnTop

        /// <summary>
        /// Gets or sets whether the cap is ontop of the needle
        /// </summary>
        [Browsable(true), Category("Needle Cap"), DefaultValue(true)]
        [Description("Indicates whether the cap is ontop of the needle.")]
        public bool CapOnTop
        {
            get { return (_CapOnTop); }

            set
            {
                if (_CapOnTop != value)
                {
                    _CapOnTop = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region CapOuterBevel

        /// <summary>
        /// Gets or sets the width of the needle cap's outer bevel, measured as a percentage of the cap width
        /// </summary>
        [Browsable(true), Category("Needle Cap"), DefaultValue(.1f)]
        [Description("Indicates the width of the needle cap's outer bevel, measured as a percentage of the cap width.")]
        [Editor("DevComponents.Instrumentation.Design.HalfRadiusRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float CapOuterBevel
        {
            get { return (_CapOuterBevel); }

            set
            {
                if (_CapOuterBevel != value)
                {
                    if (value < 0 || value > .5f)
                        throw new ArgumentException("Value must be between 0 and .5");

                    _CapOuterBevel = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region CapStyle

        /// <summary>
        /// Gets or sets the style of the needle cap
        /// </summary>
        [Browsable(true), Category("Needle Cap"), DefaultValue(NeedlePointerCapStyle.Style2)]
        [Description("Indicates the style of the needle cap.")]
        [ParenthesizePropertyName(true)]
        public NeedlePointerCapStyle CapStyle
        {
            get { return (_CapStyle); }

            set
            {
                if (_CapStyle != value)
                {
                    _CapStyle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region CapWidth

        /// <summary>
        /// Gets or sets the width of the needle cap
        /// </summary>
        [Browsable(true), Category("Needle Cap"), DefaultValue(.3f)]
        [Description("Indicates the width of the needle cap.")]
        [Editor("DevComponents.Instrumentation.Design.CapWidthRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float CapWidth
        {
            get { return (_CapWidth); }

            set
            {
                if (_CapWidth != value)
                {
                    _CapWidth = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ChangeCursor

        /// <summary>
        /// Gets or sets the Cursor displayed when the user can change the Pointer
        /// </summary>
        [Browsable(true), Category("Behavior")]
        [Description("Indicates the Cursor displayed when the user can change the Pointer.")]
        public Cursor ChangeCursor
        {
            get { return (_ChangeCursor); }

            set
            {
                if (_ChangeCursor != value)
                {
                    _ChangeCursor = value;

                    OnGaugeItemChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeChangeCursor()
        {
            return (_ChangeCursor != Cursors.Hand);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetChangeCursor()
        {
            _ChangeCursor = Cursors.Hand;
        }

        #endregion

        #region DampeningSweepTime

        /// <summary>
        /// Gets or sets the time it takes for the pointer to travel the entire scale, measured in seconds
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(1d)]
        [Description("Indicates the time it takes for the pointer to travel the entire scale, measured in seconds.")]
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

        #region FillColor

        /// <summary>
        /// Gets or sets the Pointer FillColor
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Pointer FillColor.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor FillColor
        {
            get
            {
                if (_FillColor == null)
                {
                    _FillColor = new GradientFillColor();
                    _FillColor.ColorTableChanged += FillColorColorTableChanged;
                }

                return (_FillColor);
            }

            set
            {
                if (_FillColor != null)
                    _FillColor.ColorTableChanged -= FillColorColorTableChanged;

                _FillColor = value;

                if (_FillColor != null)
                    _FillColor.ColorTableChanged += FillColorColorTableChanged;

                UpdateFillColors();

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region FillColorEx

        /// <summary>
        /// Gets the range/section adjusted Pointer FillColor
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GradientFillColor FillColorEx
        {
            get { return (_FillColorEx ?? _FillColor); }
        }

        #endregion

        #region HonorMaxPin

        /// <summary>
        /// Gets or sets whether the pointer honors the Maximum Pin
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether the pointer honors the Maximum Pin.")]
        public bool HonorMaxPin
        {
            get { return (_HonorMaxPin); }

            set
            {
                if (_HonorMaxPin != value)
                {
                    _HonorMaxPin = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region HonorMinPin

        /// <summary>
        /// Gets or sets whether the pointer honors the Minimum Pin
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether the pointer honors the Minimum Pin.")]
        public bool HonorMinPin
        {
            get { return (_HonorMinPin); }

            set
            {
                if (_HonorMinPin != value)
                {
                    _HonorMinPin = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets or sets the Image to use for the Pointer
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Image to use for the Pointer.")]
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

        #region IntervalPoint

        /// <summary>
        /// Gets the Pointer Interval Point for the current Value
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point IntervalPoint
        {
            get
            {
                if (NeedRecalcLayout == true)
                    RecalcLayout();

                return (_Renderer.IntervalPoint);
            }
        }

        #endregion

        #region IntervalAngle

        /// <summary>
        /// Gets the Pointer Interval Angle for the current Value
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float IntervalAngle
        {
            get
            {
                if (NeedRecalcLayout == true)
                    RecalcLayout();

                return (_Renderer.IntervalAngle);
            }
        }

        #endregion

        #region Length

        /// <summary>
        /// Gets or sets the Pointer length, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(.14f)]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Pointer length, specified as a percentage.")]
        public float Length
        {
            get { return (_Length); }

            set
            {
                if (_Length != value)
                {
                    _Length = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region MarkerStyle

        /// <summary>
        /// Gets or sets the Marker style
        /// </summary>
        [Browsable(true), Category("Style Specific"), DefaultValue(GaugeMarkerStyle.Triangle)]
        [Description("Indicates the Marker style.")]
        public GaugeMarkerStyle MarkerStyle
        {
            get { return (_MarkerStyle); }

            set
            {
                if (_MarkerStyle != value)
                {
                    _MarkerStyle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region NeedleStyle

        /// <summary>
        /// Gets or sets the width of the needle cap
        /// </summary>
        [Browsable(true), Category("Style Specific"), DefaultValue(NeedlePointerStyle.Style1)]
        [Description("Indicates the width of the needle cap.")]
        public NeedlePointerStyle NeedleStyle
        {
            get { return (_NeedleStyle); }

            set
            {
                if (_NeedleStyle != value)
                {
                    _NeedleStyle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Origin

        /// <summary>
        /// Gets or sets the Bar/Thermometer Pointer origin
        /// </summary>
        [Browsable(true)]
        [Category("Style Specific"), DefaultValue(PointerOrigin.Minimum)]
        [Description("Indicates the Bar/Thermometer Pointer origin.")]
        public PointerOrigin Origin
        {
            get { return (_Origin); }

            set
            {
                if (_Origin != value)
                {
                    _Origin = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region OriginInterval

        /// <summary>
        /// Gets or sets the custom Bar/Thermometer origin interval
        /// </summary>
        [Browsable(true)]
        [Category("Style Specific"), DefaultValue(double.NaN)]
        [Description("Indicates the custom Bar/Thermometer origin interval.")]
        public double OriginInterval
        {
            get { return (_OriginInterval); }

            set
            {
                if (_OriginInterval != value)
                {
                    _OriginInterval = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Placement

        /// <summary>
        /// Gets or sets the Placement of the pointer with respect to the Scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(DisplayPlacement.Center)]
        [Description("Indicates the Placement of the pointer with respect to the Scale.")]
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

        #region RotateCapImage

        /// <summary>
        /// Gets or sets whether the needle cap is rotated to match the needle angle
        /// </summary>
        [Browsable(true)]
        [Category("Needle Cap"), DefaultValue(false)]
        [Description("Indicates whether the needle cap is rotated to match the needle angle.")]
        public bool RotateCap
        {
            get { return (_RotateCap); }

            set
            {
                if (_RotateCap != value)
                {
                    _RotateCap = value;

                    OnGaugeItemChanged();
                }
            }
        }

        #endregion

        #region Scale

        /// <summary>
        /// Gets the pointer's associated Scale
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GaugeScale Scale
        {
            get { return (_Scale); }

            internal set
            {
                _Scale = value;

                UpdateStripData();
                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region ScaleOffset

        /// <summary>
        /// Gets or sets the distance from the Pointer to the Scale, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the distance from the Pointer to the Scale, measured as a percentage.")]
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

        #region SnapInterval

        /// <summary>
        /// Gets or sets the interval to use to Snap user input values to
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(0d)]
        [Description("Indicates the interval to use to Snap user input values to.")]
        public double SnapInterval
        {
            get { return (_SnapInterval); }

            set
            {
                if (_SnapInterval != value)
                {
                    _SnapInterval = value;

                    OnGaugeItemChanged();
                }
            }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets or sets the Pointer style
        /// </summary>
        [Browsable(true), Category("Style Specific"), DefaultValue(PointerStyle.Marker)]
        [Description("Indicates the Pointer style.")]
        [ParenthesizePropertyName(true)]
        public PointerStyle Style
        {
            get { return (_Style); }

            set
            {
                if (_Style != value)
                {
                    _Style = value;

                    SetPointerRenderer();

                    OnGaugeItemChanged(true);
                }
            }
        }

        #region SetPointerRenderer

        private void SetPointerRenderer()
        {
            switch (_Style)
            {
                case PointerStyle.Bar:
                    _Renderer = new GaugeBarRenderer(this);
                    break;

                case PointerStyle.Marker:
                    _Renderer = new GaugeMarkerRenderer(this);
                    break;

                case PointerStyle.Needle:
                    _Renderer = new GaugeNeedleRenderer(this);
                    break;

                case PointerStyle.Thermometer:
                    _Renderer = new GaugeThermoRenderer(this);
                    break;
            }
        }

        #endregion

        #endregion

        #region ThermoBackColor

        /// <summary>
        /// Gets or sets the Thermometers's Background "tube" Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Thermometers's Background \"tube\" Color.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor ThermoBackColor
        {
            get
            {
                if (_ThermoBackColor == null)
                {
                    _ThermoBackColor = new GradientFillColor();
                    _ThermoBackColor.ColorTableChanged += FillColorColorTableChanged;
                }

                return (_ThermoBackColor);
            }

            set
            {
                if (_ThermoBackColor != null)
                    _ThermoBackColor.ColorTableChanged -= FillColorColorTableChanged;

                _ThermoBackColor = value;

                if (_ThermoBackColor != null)
                    _ThermoBackColor.ColorTableChanged += FillColorColorTableChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region UnderTickMarks

        /// <summary>
        /// Gets or sets whether the marker is under all TickMarks
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether the marker is under all TickMarks.")]
        public bool UnderTickMarks
        {
            get { return (_UnderTickMarks); }

            set
            {
                if (_UnderTickMarks != value)
                {
                    _UnderTickMarks = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets or sets the Pointer value
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(double.NaN)]
        [Description("Indicates the Pointer value.")]
        public double Value
        {
            get { return (_Value); }

            set
            {
                if (_Value != value)
                {
                    double oldValue = _Value;
                    _Value = value;

                    OnValueChanged(oldValue, true);
                }
            }
        }

        #endregion

        #region ValueEx

        /// <summary>
        /// Gets or sets the value of the pointer - but with no dampening
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
                    double oldValue = _Value;
                    _Value = value;

                    OnValueChanged(oldValue, false);
                }
            }
        }

        #endregion

        #region Width

        /// <summary>
        /// Gets or sets the Pointer width, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(.14f)]
        [Editor("DevComponents.Instrumentation.Design.WidthRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Pointer width, specified as a percentage.")]
        public float Width
        {
            get { return (_Width); }

            set
            {
                if (_Width != value)
                {
                    _Width = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsScaleOffset

        internal int AbsScaleOffset
        {
            get
            {
                if (Scale is GaugeCircularScale)
                    return (GetAbsScaleOffset(Scale as GaugeCircularScale));

                return (GetAbsScaleOffset(Scale as GaugeLinearScale));
            }
        }

        #region GetAbsScaleOffset

        private int GetAbsScaleOffset(GaugeCircularScale scale)
        {
            return ((int)(scale.AbsRadius * ScaleOffset));
        }

        #endregion

        #region GetAbsScaleOffset

        private int GetAbsScaleOffset(GaugeLinearScale scale)
        {
            return ((int)(scale.AbsWidth * ScaleOffset));
        }

        #endregion

        #endregion

        #region DValue

        internal double DValue
        {
            get { return (_DValue); }

            set
            {
                _DValue = value;

                if (_Scale != null)
                    _Scale.GaugeControl.DValueChange = true;

                OnGaugeItemChanged(true);
            }
        }

            #endregion

        #region MouseDownAngle

        internal double MouseDownAngle
        {
            get { return (_MouseDownAngle); }
            set { _MouseDownAngle = value; }
        }

        #endregion

        #region MouseDownRadians

        internal double MouseDownRadians
        {
            get { return (_MouseDownRadians); }
            set { _MouseDownRadians = value; }
        }

        #endregion

        #region Offset

        internal int Offset
        {
            get { return (_Offset); }
        }

        #endregion

        #region Radius

        internal int Radius
        {
            get { return (_Radius); }
        }

        #endregion

        #endregion

        #region Event processing

        void FillColorColorTableChanged(object sender, EventArgs e)
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

                if (Scale is GaugeCircularScale)
                    CalcCircularMetrics(Scale as GaugeCircularScale);

                else if (Scale is GaugeLinearScale)
                    CalcLinearMetrics(Scale as GaugeLinearScale);

                if (_Renderer != null)
                    _Renderer.RecalcLayout();
            }
        }

        #region CalcCircularMetrics

        private void CalcCircularMetrics(GaugeCircularScale scale)
        {
            int radius = scale.AbsRadius;
            int offset = AbsScaleOffset;
            int scaleWidth = (int)(radius * scale.Width);

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    _Radius = radius - (scaleWidth / 2) + offset;
                    break;

                case DisplayPlacement.Center:
                    _Radius = radius + offset;
                    break;

                case DisplayPlacement.Far:
                    _Radius = radius + (scaleWidth / 2) + offset;
                    break;
            }
        }

        #endregion

        #region CalcLinearMetrics

        private void CalcLinearMetrics(GaugeLinearScale scale)
        {
            int n = scale.AbsWidth;
            int offset = AbsScaleOffset;
            int scaleWidth = (int)(n * scale.Width);

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    _Offset = (-scaleWidth / 2) - offset;
                    break;

                case DisplayPlacement.Center:
                    _Offset = -offset;
                    break;

                case DisplayPlacement.Far:
                    _Offset = (scaleWidth / 2) + offset;
                    break;
            }
        }

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();

            if (Scale.GaugeControl.OnPreRenderScalePointer(e, this) == false)
            {
                if (_Renderer != null)
                {
                    switch (_Scale.Style)
                    {
                        case GaugeScaleStyle.Circular:
                            _Renderer.RenderCircular(e);
                            break;

                        case GaugeScaleStyle.Linear:
                            _Renderer.RenderLinear(e);
                            break;
                    }
                }

                Scale.GaugeControl.OnPostRenderScalePointer(e, this);
            }
        }

        #endregion

        #region OnValueChanged

        private void OnValueChanged(double oldValue, bool canDampen)
        {
            if (_Scale != null)
            {
                _Scale.GaugeControl.OnPointerValueChanged(this, oldValue, _Value);

                if (_Scale.Visible == true)
                {
                    UpdateStripData();

                    if (InitiateDampening(canDampen) == true)
                        return;
                }
            }

            SetFinalOffsets();

            DValue = _Value;
        }

        #endregion

        #region Dampening support

        #region InitiateDampening

        private bool InitiateDampening(bool canDampen)
        {
            if (_Scale.GaugeControl.IsHandleCreated &&
                _Scale.GaugeControl.InDesignMode == false)
            {
                if (_Renderer != null)
                {
                    double min, max;

                    _Renderer.GetRangeEx(out min, out max);

                    if (_DValue < _Value)
                    {
                        double start = Math.Max(min, _DValue);

                        if (start < max)
                        {
                            if (StartDampening(canDampen, start, Math.Min(max, _Value)))
                                return (true);
                        }
                    }
                    else if (_DValue > _Value)
                    {
                        double start = Math.Min(max, _DValue);

                        if (start > min)
                        {
                            if (StartDampening(canDampen, start, Math.Max(min, _Value)))
                                return (true);
                        }
                    }
                }
            }

            return (false);
        }

        #endregion

        #region StartDampening

        private bool StartDampening(bool canDampen, double start, double end)
        {
            double dampenTime = _DampeningSweepTime;

            if (ValueInView(end) == false)
            {
                canDampen = true;

                dampenTime = (_MouseDown == true || dampenTime <= 0)
                    ? 2 : dampenTime;
            }

            if (canDampen == true && dampenTime > 0)
            {
                _DStartValue = start;
                _DEndValue = end;

                _DeltaValue = Scale.Spread / (dampenTime * 1000);
                _DStartTicks = DateTime.Now.Ticks;

                if (_Dampening == false)
                {
                    _Dampening = true;
                    _DSlideScale = false;

                    _DShowMinLabel = _Scale.Labels.ShowMinLabel;
                    _DShowMaxLabel = _Scale.Labels.ShowMaxLabel;

                    _Scale.GaugeControl.DampeningUpdate += DampeningUpdate;
                    _Scale.GaugeControl.StartDampening();
                }

                return (true);
            }

            return (false);
        }

        #endregion

        #region DampeningUpdate

        void DampeningUpdate(object sender, EventArgs e)
        {
            SetScaleValue();
            SetInterimOffsets();

            if (_DValue == _DEndValue)
            {
                if (_MouseDown == true && _AllowUserChange == true)
                {
                    if (_Renderer != null)
                    {
                        Point pt = Control.MousePosition;
                        Point pt2 = Scale.GaugeControl.PointToClient(pt);

                        ValueEx = _Renderer.GetValueFromPoint(pt2);
                    }
                }
                else
                {
                    SetFinalOffsets();
                }
            }
        }

        #endregion

        #region SetScaleValue

        private void SetScaleValue()
        {
            double ms = new TimeSpan(DateTime.Now.Ticks - _DStartTicks).TotalMilliseconds;
            double delta = _DeltaValue * ms;

            double n = _DValue;

            if (n < _Value)
                n = Math.Min(_DStartValue + delta, _DEndValue);

            else if (n > _Value)
                n = Math.Max(_DStartValue - delta, _DEndValue);

            SetScaleValue(n);
        }

        private void SetScaleValue(double n)
        {
            if (_DValue != _Value)
            {
                double spread = _Scale.Spread;

                if (n > _Scale.MaxValue)
                {
                    _Scale.MaxValue = Math.Min(n, _Scale.AbsMaxLimit);
                    _Scale.MinValue = _Scale.MaxValue - spread;

                    _DSlideScale = true;
                }
                else if (n < _Scale.MinValue)
                {
                    _Scale.MinValue = Math.Max(n, _Scale.AbsMinLimit);
                    _Scale.MaxValue = _Scale.MinValue + spread;

                    _DSlideScale = true;
                }
            }

            DValue = n;
        }

        #endregion

        #region SetInterimOffsets

        private void SetInterimOffsets()
        {
            if (_DSlideScale == true)
            {
                GaugeTickMark major = _Scale.MajorTickMarks;
                GaugeTickMark minor = _Scale.MinorTickMarks;

                major.IntervalOffset = GetIntervalOffset(major.Interval);
                minor.IntervalOffset = GetIntervalOffset(minor.Interval);

                if (_DShowMinLabel == true)
                {
                    if (minor.Visible && minor.Interval > 0)
                    {
                        _Scale.Labels.ShowMinLabel =
                            (minor.IntervalOffset == 0);
                    }
                    else if (major.Visible && major.Interval > 0)
                    {
                        _Scale.Labels.ShowMinLabel =
                            (major.IntervalOffset == 0);
                    }
                }

                if (_DShowMaxLabel == true)
                {
                    if (minor.Visible && minor.Interval > 0)
                    {
                        _Scale.Labels.ShowMaxLabel =
                            (_Scale.MaxValue % minor.Interval == 0);
                    }
                    else if (major.Visible && major.Interval > 0)
                    {
                        _Scale.Labels.ShowMaxLabel =
                            (_Scale.MaxValue % major.Interval == 0);
                    }
                }
            }
        }

        #endregion

        #region SetFinalOffsets

        private void SetFinalOffsets()
        {
            if (_DSlideScale == true)
            {
                GaugeTickMark major = _Scale.MajorTickMarks;
                GaugeTickMark minor = _Scale.MinorTickMarks;

                double interval;

                if (_SnapInterval > 0)
                {
                    interval = _SnapInterval;
                }
                else
                {
                    interval = ((minor.Visible && minor.Interval > 0)
                        ? minor.Interval : (major.Visible && major.Interval > 0)
                              ? major.Interval : 1);

                    if ((int)interval == interval)
                        interval = 1;
                }

                double minLimit = _Scale.AbsMinLimit;
                double k = (int)((_Scale.MinValue - minLimit) / interval) * interval + minLimit;
                double spread = _Scale.Spread;

                if (_DStartValue < _DEndValue && (k + spread < _Scale.MaxValue))
                    k += interval;

                _Scale.MinValue = k;
                _Scale.MaxValue = k + spread;

                if (minor.Visible && minor.Interval > 0)
                    minor.IntervalOffset = GetIntervalOffset(minor.Interval);

                if (major.Visible && major.Interval > 0)
                    major.IntervalOffset = GetIntervalOffset(major.Interval);

                _Scale.Labels.ShowMinLabel = _DShowMinLabel;
                _Scale.Labels.ShowMaxLabel = _DShowMaxLabel;
            }

            if (_Dampening == true)
            {
                _Scale.GaugeControl.StopDampening();
                _Scale.GaugeControl.DampeningUpdate -= DampeningUpdate;

                _Dampening = false;
            }
        }

        #endregion

        #region GetIntervalOffset

        private double GetIntervalOffset(double interval)
        {
            double n = _Scale.MinValue - _Scale.AbsMinLimit;

            return (Math.Ceiling(n / interval) * interval - n);
        }

        #endregion

        #region ValueInView

        private bool ValueInView(double value)
        {
            return ((_Scale.MinValue <= _Scale.AbsMinLimit || value >= _Scale.MinValue) &&
                 ((_Scale.MaxValue >= _Scale.AbsMaxLimit || value <= _Scale.MaxValue)));
        }

        #endregion

        #endregion

        #region UpdateStripData

        #region UpdateStripData

        internal void UpdateStripData()
        {
            if (_Scale != null)
            {
                ProcessScaleChanges();
                ProcessSectionChanges();
                ProcessRangeChanges();
            }
                
            UpdateFillColors();
        }

        #endregion

        #region ProcessScaleChanges

        private void ProcessScaleChanges()
        {
            bool oldInScale = _InScale;

            _InScale = (_Value >= Scale.MinValue && _Value <= Scale.MaxValue);

            if (_Scale.GaugeControl != null)
            {
                if (_InScale != oldInScale)
                {
                    if (_InScale == true)
                        Scale.GaugeControl.OnScaleEnter(this, Scale);
                    else
                        Scale.GaugeControl.OnScaleExit(this, Scale);
                }
            }
        }

        #endregion

        #region ProcessSectionChanges

        private void ProcessSectionChanges()
        {
            if (Scale.HasSections == true)
            {
                List<GaugeStrip> list = new List<GaugeStrip>();

                foreach (GaugeSection section in Scale.Sections)
                {
                    if (section.Visible == true)
                    {
                        if (section.ValueInRange(_Value) == true)
                            list.Add(section);
                    }
                }

                if (_Scale.GaugeControl != null)
                {
                    if (_SectionList != null)
                    {
                        foreach (GaugeSection section in _SectionList)
                        {
                            if (list.Contains(section) == false)
                                Scale.GaugeControl.OnSectionExit(this, section);
                        }
                    }

                    foreach (GaugeSection section in list)
                    {
                        if (_SectionList == null || _SectionList.Contains(section) == false)
                            Scale.GaugeControl.OnSectionEnter(this, section);
                    }
                }

                _SectionList = list;
            }
        }

        #endregion

        #region ProcessRangeChanges

        private void ProcessRangeChanges()
        {
            if (Scale.HasRanges == true)
            {
                List<GaugeStrip> list = new List<GaugeStrip>();

                foreach (GaugeRange range in Scale.Ranges)
                {
                    if (range.Visible == true)
                    {
                        if (range.ValueInRange(_Value) == true)
                            list.Add(range);
                    }
                }

                if (_Scale.GaugeControl != null)
                {
                    if (_RangeList != null)
                    {
                        foreach (GaugeRange range in _RangeList)
                        {
                            if (list.Contains(range) == false)
                                Scale.GaugeControl.OnRangeExit(this, range);
                        }
                    }

                    foreach (GaugeRange range in list)
                    {
                        if (_RangeList == null || _RangeList.Contains(range) == false)
                            Scale.GaugeControl.OnRangeEnter(this, range);
                    }
                }

                _RangeList = list;
            }
        }

        #endregion

        #region UpdateFillColors

        #region UpdateFillColors

        private void UpdateFillColors()
        {
            _FillColorEx = GetFillColorEx(_RangeList)
                ?? (GetFillColorEx(_SectionList) ?? FillColor);

            if (_Style == PointerStyle.Needle)
            {
                _CapFillColorEx = (GetCapFillColorEx(_RangeList)
                    ?? GetCapFillColorEx(_SectionList)) ?? CapFillColor;
            }
        }

        #endregion

        #region GetFillColorEx

        private GradientFillColor GetFillColorEx(List<GaugeStrip> list)
        {
            if (list != null)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].HasPointerFillColor == true)
                        return (list[i].PointerFillColor);
                }
            }

            return (null);
        }

        #endregion

        #region GetCapFillColorEx

        private GradientFillColor GetCapFillColorEx(List<GaugeStrip> list)
        {
            if (list != null)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].HasCapFillColor == true)
                        return (list[i].CapFillColor);
                }
            }

            return (null);
        }

        #endregion

        #endregion

        #endregion

        #region EndInit

        internal void EndInit()
        {
            UpdateStripData();
        }

        #endregion

        #region FindItem

        internal override GaugeItem FindItem(Point pt)
        {
            if (_Renderer != null)
            {
                GraphicsPath path = _Renderer.GetPointerPath();

                if (path != null)
                {
                    if (path.IsVisible(pt) == true)
                        return (this);
                }
                else
                {
                    GaugeBarRenderer bar = _Renderer as GaugeBarRenderer;

                    if (bar != null)
                    {
                        Rectangle r;

                        if (Scale is GaugeCircularScale)
                        {
                            r = new Rectangle(
                                bar.IntervalPoint.X - 5, bar.IntervalPoint.Y - 5, 10, 10);
                        }
                        else
                        {
                            r = bar.Bounds;

                            if (r.Width < 10)
                            {
                                r.X -= 5;
                                r.Width = 10;
                            }

                            if (r.Height < 10)
                            {
                                r.Y -= 5;
                                r.Height = 10;
                            }
                        }

                        if (r.Contains(pt) == true)
                            return (this);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetPointerPath

        /// <summary>
        /// Gets the Pointers GraphicsPath
        /// </summary>
        /// <returns>Pointers GraphicsPath</returns>
        public GraphicsPath GetPointerPath()
        {
            if (_Renderer != null)
                return (_Renderer.GetPointerPath());

            return (null);
        }

        #endregion

        #region GetCapBounds

        /// <summary>
        /// Gets the Pointer's Needle Cap bounds
        /// </summary>
        /// <returns></returns>
        public Rectangle GetCapBounds()
        {
            if (_Renderer != null && _Renderer is GaugeNeedleRenderer)
                return (((GaugeNeedleRenderer)_Renderer).CapBounds);

            return (Rectangle.Empty);
        }

        #endregion

        #region GetValueFromPoint

        /// <summary>
        /// Gets the value of the pointer from the given Point
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Value</returns>
        public double GetValueFromPoint(Point point)
        {
            if (_Renderer != null)
                return (_Renderer.GetValueFromPoint(point));

            return (double.NaN);
        }

        #endregion

        #region AppendTemplateText

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

        #region Mouse processing

        #region OnMouseEnter

        internal override void OnMouseEnter()
        {
            base.OnMouseEnter();

            if (_AllowUserChange == true)
                Scale.GaugeControl.Cursor = _ChangeCursor;
        }

        #endregion

        #region OnMouseMove

        internal override void OnMouseMove(MouseEventArgs e, bool mouseDown)
        {
            base.OnMouseMove(e, mouseDown);

            if (mouseDown == true)
            {
                if (_AllowUserChange == true)
                {
                    if (_Renderer != null)
                    {
                        double oldValue = Value;
                        double newValue = _Renderer.GetValueFromPoint(e.Location);

                        if (Scale.GaugeControl.OnPointerChanging(this, oldValue, ref newValue) == false)
                        {
                            ValueEx = newValue;

                            Scale.GaugeControl.OnPointerChanged(this, oldValue, newValue);
                        }
                    }
                }
            }
        }

        #endregion

        #region OnMouseDown

        private bool _MouseDown;

        internal override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_AllowUserChange == true)
            {
                _MouseDown = true;

                if (_Renderer != null)
                    _Renderer.OnMouseDown(e);
            }
        }

        #endregion

        #region OnMouseUp

        internal override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _MouseDown = false;
        }

        #endregion

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugePointer copy = new GaugePointer();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugePointer c = copy as GaugePointer;

            if (c != null)
            {
                base.CopyToItem(c);

                c.AllowUserChange = _AllowUserChange;
                c.BarStyle = _BarStyle;
                c.BulbOffset = _BulbOffset;
                c.BulbSize = _BulbSize;

                if (_CapFillColor != null)
                    c.CapFillColor = (GradientFillColor) _CapFillColor.Clone();

                c.CapImage = _CapImage;
                c.CapInnerBevel = _CapInnerBevel;
                c.CapOnTop = _CapOnTop;
                c.CapOuterBevel = _CapOuterBevel;
                c.CapStyle = _CapStyle;
                c.CapWidth = _CapWidth;
                c.ChangeCursor = _ChangeCursor;
                c.DampeningSweepTime = _DampeningSweepTime;

                if (_FillColor != null)
                    c.FillColor = (GradientFillColor)_FillColor.Clone();

                c.HonorMaxPin = _HonorMaxPin;
                c.HonorMinPin = _HonorMinPin;
                c.Image = _Image;
                c.Length = _Length;
                c.MarkerStyle = _MarkerStyle;
                c.NeedleStyle = _NeedleStyle;
                c.Origin = _Origin;
                c.OriginInterval = _OriginInterval;
                c.Placement = _Placement;
                c.RotateCap = _RotateCap;
                c.ScaleOffset = _ScaleOffset;
                c.SnapInterval = _SnapInterval;
                c.Style = _Style;

                if (_ThermoBackColor != null)
                    c.ThermoBackColor = (GradientFillColor)_ThermoBackColor.Clone();

                c.UnderTickMarks = _UnderTickMarks;
                c.Value = _Value;
                c.Width = _Width;
            }
        }

        #endregion
    }

    #region Enums

    public enum PointerStyle
    {
        Bar,
        Marker,
        Needle,
        Thermometer
    }

    public enum PointerOrigin
    {
        Minimum,
        Maximum,
        Custom
    }

    #endregion
}
