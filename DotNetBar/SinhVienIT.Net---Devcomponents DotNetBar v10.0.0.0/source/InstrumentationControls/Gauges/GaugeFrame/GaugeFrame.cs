using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GaugeFrameConvertor))]
    public class GaugeFrame : IDisposable
    {
        #region Events

        [Description("Occurs when the Gauge Frame is changed.")]
        public event EventHandler<EventArgs> GaugeFrameChanged;

        [Description("Occurs before the Gauge Frame is rendered.")]
        public event EventHandler<PreRenderFrameEventArgs> PreRenderFrame;

        [Description("Occurs after the Gauge Frame has been rendered.")]
        public event EventHandler<PostRenderFrameEventArgs> PostRenderFrame;

        [Description("Occurs when the Gauge Frame Region should be set.")]
        public event EventHandler<SetFrameRegionEventArgs> SetGaugeFrameRegion;

        #endregion

        #region Private variables

        private GaugeFrameStyle _Style;

        private float _InnerBevel;
        private float _OuterBevel;
        private float _RoundRectangleArc;

        private float _BackSigmaFocus;
        private float _BackSigmaScale;
        private float _FrameSigmaFocus;
        private float _FrameSigmaScale;

        private GradientFillColor _BackColor;
        private GradientFillColor _FrameColor;

        private bool _ClipOuterFrame;
        private bool _AddGlassEffect;

        private Image _Image;

        private GaugeControl _GaugeControl;

        private bool _AutoCenter;
        private bool _NeedRecalcLayout;

        private Rectangle _Bounds;
        private Rectangle _BackBounds;
        private PointF _Center;

        private GaugeFrameRenderer _Renderer;

        #endregion

        public GaugeFrame(GaugeControl gaugeControl)
        {
            _GaugeControl = gaugeControl;

            Style = GaugeFrameStyle.None;

            _InnerBevel = .035f;
            _OuterBevel = .05f;
            _RoundRectangleArc = .125f;

            _BackSigmaFocus = 1;
            _BackSigmaScale = 1;
            _FrameSigmaFocus = .15f;
            _FrameSigmaScale = 1;

            BackColor = new GradientFillColor(Color.Silver, Color.Gray, 45);
            FrameColor = new GradientFillColor(Color.Gainsboro, Color.Gray, 45);
        }

        #region Public properties

        #region AbsBevelInside

        /// <summary>
        /// Gets the calculated inside bevel dimension for the frame
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int AbsBevelInside
        {
            get
            {
                int n = MinDimension;

                return (_InnerBevel < .5f) ?
                    (int)(n * _InnerBevel) : n / 2;
            }
        }

        #endregion

        #region AbsBevelOutside

        /// <summary>
        /// Gets the calculated outside bevel dimension for the frame
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int AbsBevelOutside
        {
            get
            {
                int n = MinDimension;

                return (_OuterBevel < .5f) ?
                    (int)(n * _OuterBevel) : n / 2;
            }
        }

        #endregion

        #region AddGlassEffect

        /// <summary>
        /// Gets or sets whether to add a Glass Effect to the frame
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether to add a Glass Effect to the frame.")]
        public bool AddGlassEffect
        {
            get { return (_AddGlassEffect); }

            set
            {
                if (_AddGlassEffect != value)
                {
                    _AddGlassEffect = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region BackBounds

        /// <summary>
        /// Gets the background bounding rectangle
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BackBounds
        {
            get { return (_BackBounds); }
            internal set { _BackBounds = value; }
        }

        #endregion

        #region BackColor

        /// <summary>
        /// Gets or sets the background Color of the Gauge.
        /// </summary>
        [Browsable(true),Category("Appearance")]
        [Description("Indicates the background Color of the Gauge.")]
        public GradientFillColor BackColor
        {
            get
            {
                if (_BackColor == null)
                {
                    _BackColor = new GradientFillColor();
                    _BackColor.ColorTableChanged += BackColor_ColorTableChanged;
                }

                return (_BackColor);
            }

            set
            {
                if (_BackColor != null)
                    _BackColor.ColorTableChanged -= BackColor_ColorTableChanged;

                _BackColor = value;

                if (_BackColor != null)
                    _BackColor.ColorTableChanged += BackColor_ColorTableChanged;

                OnGaugeFrameChanged();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeBackColor()
        {
            return (BackColor.IsEqualTo(Color.Silver,
                Color.Gray, 45, GradientFillType.Auto, Color.Empty, 0) == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void ResetBackColor()
        {
            BackColor = new GradientFillColor(Color.Silver, Color.Gray, 45);
        }

        #endregion

        #region BackSigmaFocus

        /// <summary>
        /// Gets or sets the SigmaBellShape Focus used when Center filling the frame.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(1f)]
        [Description("Indicates the SigmaBellShape Focus used when Center filling the frame.")]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float BackSigmaFocus
        {
            get { return (_BackSigmaFocus); }

            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentException("Value must be between 0 and 1.");

                if (_BackSigmaFocus != value)
                {
                    _BackSigmaFocus = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region BackSigmaScale

        /// <summary>
        /// Gets or sets the SigmaBellShape Scale used when Center filling the frame.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(1f)]
        [Description("Indicates the SigmaBellShape Scale used when Center filling the frame.")]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float BackSigmaScale
        {
            get { return (_BackSigmaScale); }

            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentException("Value must be between 0 and 1.");

                if (_BackSigmaScale != value)
                {
                    _BackSigmaScale = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region Bounds

        /// <summary>
        /// Gets the bounding frame rectangle
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds
        {
            get { return (_Bounds); }
            internal set { _Bounds = value; }
        }

        #endregion

        #region Center

        /// <summary>
        /// Gets the center of the gauge frame
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF Center
        {
            get { return (_Center); }
            internal set { _Center = value; }
        }

        #endregion

        #region InnerBevel

        /// <summary>
        /// Gets or sets the inner frame bevel width, measured as a percentage of the width/height.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(.035f)]
        [Description("Indicates the inner frame bevel width, measured as a percentage of the width/height.")]
        [Editor("DevComponents.Instrumentation.Design.HalfRadiusRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float InnerBevel
        {
            get { return (_InnerBevel); }

            set
            {
                if (_InnerBevel != value)
                {
                    if (value < 0 || value > .5f)
                        throw new ArgumentException("Value must be between 0 and .5");

                    _InnerBevel = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region OuterBevel

        /// <summary>
        /// Gets or sets the outer frame bevel width, measured as a percentage of the width/height.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(.05f)]
        [Description("Indicates the outer frame bevel width, measured as a percentage of the width/height.")]
        [Editor("DevComponents.Instrumentation.Design.HalfRadiusRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float OuterBevel
        {
            get { return (_OuterBevel); }

            set
            {
                if (value < 0 || value > .5f)
                    throw new ArgumentException("Value must be between 0 and .5");

                if (_OuterBevel != value)
                {
                    _OuterBevel = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region ClipOuterFrame

        /// <summary>
        /// Gets and sets whether the frame exterior is clipped.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether the frame exterior is clipped.")]
        public bool ClipOuterFrame
        {
            get { return (_ClipOuterFrame); }

            set
            {
                if (_ClipOuterFrame != value)
                {
                    _ClipOuterFrame = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region FrameColor

        /// <summary>
        /// Gets or sets the Frame Color of the Gauge.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Frame Color of the Gauge.")]
        public GradientFillColor FrameColor
        {
            get
            {
                if (_FrameColor == null)
                {
                    _FrameColor = new GradientFillColor();
                    _FrameColor.ColorTableChanged += FrameColor_ColorTableChanged;
                }

                return (_FrameColor);
            }

            set
            {
                if (_FrameColor != null)
                    _FrameColor.ColorTableChanged -= FrameColor_ColorTableChanged;

                _FrameColor = value;

                if (_FrameColor != null)
                    _FrameColor.ColorTableChanged += FrameColor_ColorTableChanged;

                OnGaugeFrameChanged();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeFrameColor()
        {
            return (FrameColor.IsEqualTo(Color.Gainsboro,
                Color.Gray, 45, GradientFillType.Auto, Color.Empty, 0) == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetFrameColor()
        {
            FrameColor = new GradientFillColor(Color.Gainsboro, Color.Gray, 45);
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets of sets the Image to use for the Gauge Frame
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Image to use for the Gauge Frame.")]
        public Image Image
        {
            get { return (_Image); }

            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region FrameSigmaFocus

        /// <summary>
        /// Gets or sets the SigmaBellShape.Focus used when Center filling the frame.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(.15f)]
        [Description("Indicates the SigmaBellShape.Focus used when Center filling the frame.")]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float FrameSigmaFocus
        {
            get { return (_FrameSigmaFocus); }

            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentException("Value must be between 0 and 1.");

                if (_FrameSigmaFocus != value)
                {
                    _FrameSigmaFocus = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region FrameSigmaScale

        /// <summary>
        /// Gets or sets the SigmaBellShape.Scale used when Center filling the frame.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(1f)]
        [Description("Indicates the SigmaBellShape.Scale used when Center filling the frame.")]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float FrameSigmaScale
        {
            get { return (_FrameSigmaScale); }

            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentException("Value must be between 0 and 1.");

                if (_FrameSigmaScale != value)
                {
                    _FrameSigmaScale = value;

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region GaugeControl

        /// <summary>
        /// Gets the parent GaugeControl.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GaugeControl GaugeControl
        {
            get { return (_GaugeControl); }
        }

        #endregion

        #region RoundRectangleArc

        /// <summary>
        /// Gets or sets the arc radius used when drawing RoundRectangle
        /// frames, measured as a percentage of the width/height.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(.125f)]
        [Description("Indicates the arc radius used when drawing RoundRectangle frames, measured as a percentage of the width/height.")]
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

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets or sets the frame Style.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(GaugeFrameStyle.None)]
        [Description("Indicates the frame Style.")]
        public GaugeFrameStyle Style
        {
            get { return (_Style); }

            set
            {
                if (_Style != value)
                {
                    _Style = value;
                    _AutoCenter = false;

                    switch (_Style)
                    {
                        case GaugeFrameStyle.Circular:
                            _AutoCenter = true;
                            _Renderer = new GaugeFrameCircularRenderer(this);
                            break;

                        case GaugeFrameStyle.Rectangular:
                            _Renderer = new GaugeFrameRectangularRenderer(this);
                            break;

                        case GaugeFrameStyle.RoundedRectangular:
                            _Renderer = new GaugeFrameRoundRectRenderer(this);
                            break;

                        default:
                            _Renderer = null;
                            break;
                    }

                    OnGaugeFrameChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AutoCenter

        internal bool AutoCenter
        {
            get { return (_AutoCenter); }
        }

        #endregion

        #region Renderer

        internal GaugeFrameRenderer Renderer
        {
            get { return (_Renderer); }
            set { _Renderer = value; }
        }

        #endregion

        #region MinDimension

        internal int MinDimension
        {
            get { return (Math.Min(_Bounds.Width, _Bounds.Height)); }
        }

        #endregion

        #region NeedRecalcLayout

        internal bool NeedRecalcLayout
        {
            get { return (_NeedRecalcLayout); }
            set { _NeedRecalcLayout = value; }
        }

        #endregion

        #endregion

        #region Event processing

        #region BackColor_ColorTableChanged

        void BackColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeFrameChanged();
        }

        #endregion

        #region FrameColor_ColorTableChanged

        void FrameColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeFrameChanged();
        }

        #endregion

        #endregion

        #region OnGaugeFrameChanged

        private void OnGaugeFrameChanged()
        {
            _NeedRecalcLayout = true;

            if (GaugeFrameChanged != null)
                GaugeFrameChanged(this, EventArgs.Empty);
        }

        #endregion

        #region RecalcLayout

        internal void RecalcLayout()
        {
            if (_NeedRecalcLayout == true)
            {
                int n = Math.Min(_GaugeControl.Width, _GaugeControl.Height);

                if (_Style == GaugeFrameStyle.Circular)
                {
                    int x = (_GaugeControl.Width - n) / 2;
                    int y = (_GaugeControl.Height - n) / 2;

                    n = Math.Max(n, 1);

                    _Bounds = new Rectangle(x, y, n, n);
                }
                else
                {
                    _Bounds = new Rectangle(Point.Empty, _GaugeControl.Bounds.Size);
                }

                _Center = new PointF(_Bounds.X + _Bounds.Width / 2, _Bounds.Y + _Bounds.Height / 2);

                int m = AbsBevelInside + AbsBevelOutside;

                _BackBounds = _Bounds;
                _BackBounds.Inflate(-m, -m);

                if (_ClipOuterFrame == true)
                    OnSetFrameRegion();
                else
                    _GaugeControl.Region = null;

                _NeedRecalcLayout = false;
            }
        }

        #endregion

        #region OnPaint

        internal void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();

            if (OnPreRenderFrame(e) == false)
            {
                if (_Image != null)
                {
                    e.Graphics.DrawImage(_Image, _Bounds);
                }
                else
                {
                    if (_Renderer != null)
                        _Renderer.RenderFrame(e);
                }
            }

            OnPostRenderFrame(e);
        }

        #region OnPreRenderFrame

        private bool OnPreRenderFrame(PaintEventArgs e)
        {
            if (PreRenderFrame != null)
            {
                PreRenderFrameEventArgs args =
                    new PreRenderFrameEventArgs(e.Graphics, _Bounds);

                PreRenderFrame(this, args);

                return (args.Cancel);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderFrame

        private void OnPostRenderFrame(PaintEventArgs e)
        {
            if (PostRenderFrame != null)
            {
                PostRenderFrameEventArgs args =
                    new PostRenderFrameEventArgs(e.Graphics, _Bounds);

                PostRenderFrame(this, args);
            }
        }

        #endregion

        #endregion

        #region OnSetFrameRegion

        private void OnSetFrameRegion()
        {
            if (SetGaugeFrameRegion != null)
            {
                SetFrameRegionEventArgs args =
                    new SetFrameRegionEventArgs(_Bounds);

                SetGaugeFrameRegion(this, args);

                if (args.Cancel == true)
                    return;
            }

            if (_Renderer != null)
                _Renderer.SetFrameRegion();
            else
                _GaugeControl.Region = null;
        }

        #endregion

        #region PreRenderContent

        internal void PreRenderContent(PaintEventArgs e)
        {
            if (_Renderer != null)
                _Renderer.PreRenderContent(e);
        }

        #endregion

        #region PostRenderContent

        internal void PostRenderContent(PaintEventArgs e)
        {
            if (_Renderer != null)
                _Renderer.PostRenderContent(e);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_BackColor != null)
                _BackColor.ColorTableChanged -= BackColor_ColorTableChanged;

            if (_FrameColor != null)
                _FrameColor.ColorTableChanged -= FrameColor_ColorTableChanged;
        }

        #endregion
    }

    #region GaugeFrameConvertor

    public class GaugeFrameConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeFrame gf = value as GaugeFrame;

                if (gf != null)
                {
                    switch (gf.Style)
                    {
                        case GaugeFrameStyle.Circular:
                            return ("Circular");

                        case GaugeFrameStyle.Rectangular:
                            return ("Rectangular");

                        case GaugeFrameStyle.RoundedRectangular:
                            return ("Rounded Rectangular");

                        default:
                            return ("None");
                    }
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

    #region Enums

    public enum GaugeFrameStyle
    {
        None,

        Circular,
        Rectangular,
        RoundedRectangular,
    }

    #endregion

    #region EventArgs

    #region GetFrameRegionEventArgs

    /// <summary>
    /// SetFrameRegionEventArgs
    /// </summary>
    public class SetFrameRegionEventArgs : CancelEventArgs
    {
        #region Private variables

        private Region _Region;
        private Rectangle _Bounds;

        #endregion

        public SetFrameRegionEventArgs(Rectangle bounds)
        {
            _Bounds = bounds;
        }

        #region Public properties

        #region Bounds

        /// <summary>
        /// Gets the Region Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion

        #region Region

        /// <summary>
        /// Gets or sets the frame Region
        /// </summary>
        public Region Region
        {
            get { return (_Region); }
            set { _Region = value; }
        }

        #endregion

        #endregion
    }

    #endregion

    #region PostRenderFrameEventArgs

    /// <summary>
    /// PostRenderFrameEventArgs
    /// </summary>
    public class PostRenderFrameEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private Rectangle _Bounds;

        #endregion

        public PostRenderFrameEventArgs(Graphics graphics, Rectangle bounds)
        {
            _Graphics = graphics;
            _Bounds = bounds;
        }

        #region Public properties

        #region Bounds

        /// <summary>
        /// Gets the Frame Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion

        #region Graphics

        /// <summary>
        /// Gets the Graphics object to use for the render
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        #endregion

        #endregion
    }

    #endregion

    #region PreRenderFrameEventArgs

    public class PreRenderFrameEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private Rectangle _Bounds;

        #endregion

        public PreRenderFrameEventArgs(Graphics graphics, Rectangle bounds)
        {
            _Graphics = graphics;
            _Bounds = bounds;
        }

        #region Public properties

        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion
    }

    #endregion

    #endregion
}
