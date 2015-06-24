using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(TickMarkLayoutConvertor))]
    public class TickMarkLayout : IDisposable, ICloneable
    {
        #region Events

        public event EventHandler<EventArgs> TickMarkLayoutChanged;

        #endregion

        #region Private variables

        private GaugeMarkerStyle _Style;

        private DisplayPlacement _Placement;
        private float _ScaleOffset;

        private float _Width;
        private float _Length;

        private GradientFillColor _FillColor;
        private GaugeTickMarkOverlap _Overlap;

        private Image _Image;
        private Bitmap _Bitmap;

        private float _DefaultWidth;
        private float _DefaultLength;
        private GaugeMarkerStyle _DefaultStyle;

        #endregion

        public TickMarkLayout()
            : this(GaugeMarkerStyle.Rectangle, .045f, .09f)
        {
        }

        public TickMarkLayout(GaugeMarkerStyle style, float width, float length)
        {
            _Style = style;
            _Width = width;
            _Length = length;

            _DefaultStyle = style;
            _DefaultWidth = width;
            _DefaultLength = length;

            _Placement = DisplayPlacement.Center;

            FillColor = new GradientFillColor(Color.DarkGray, Color.White);
            FillColor.BorderColor = Color.DimGray;
            FillColor.BorderWidth = 1;

            _Overlap = GaugeTickMarkOverlap.ReplaceAll;
        }

        #region Public properties

        #region FillColor

        /// <summary>
        /// Gets or sets the TickMark Fill Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the TickMark Fill Color.")]
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

                OnTickMarkLayoutChanged();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeFillColor()
        {
            return (_FillColor.IsEqualTo(Color.DarkGray,
                Color.White, 90, GradientFillType.Auto, Color.DimGray, 1) == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetFillColor()
        {
            FillColor = new GradientFillColor(Color.DarkGray, Color.White);
            FillColor.BorderColor = Color.DimGray;
            FillColor.BorderWidth = 1;
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets or sets the Image to use for the TickMark
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Image to use for the TickMark.")]
        public Image Image
        {
            get { return (_Image); }

            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    OnTickMarkLayoutChanged();
                }
            }
        }

        #endregion

        #region Length

        /// <summary>
        /// Gets or sets the Length of the TickMark, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.RangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Length of the TickMark, specified as a percentage.")]
        public float Length
        {
            get { return (_Length); }

            set
            {
                if (value < 0)
                    throw new ArgumentException("Value can not be less than zero.");

                if (_Length != value)
                {
                    _Length = value;

                    OnTickMarkLayoutChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLength()
        {
            return (_Length != _DefaultLength);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLength()
        {
            Length = _DefaultLength;
        }

        #endregion

        #region Placement

        /// <summary>
        /// Gets or sets the Placement of the TickMarks with respect to the Scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(DisplayPlacement.Center)]
        [Description("Indicates the Placement of the TickMarks with respect to the Scale.")]
        public DisplayPlacement Placement
        {
            get { return (_Placement); }

            set
            {
                if (_Placement != value)
                {
                    _Placement = value;

                    OnTickMarkLayoutChanged();
                }
            }
        }

        #endregion

        #region ScaleOffset

        /// <summary>
        /// Gets or sets the distance from the TickMark to the Scale, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the distance from the TickMark to the Scale, measured as a percentage.")]
        public float ScaleOffset
        {
            get { return (_ScaleOffset); }

            set
            {
                if (_ScaleOffset != value)
                {
                    _ScaleOffset = value;

                    OnTickMarkLayoutChanged();
                }
            }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets or sets the TickMark Style
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the TickMark Style.")]
        public GaugeMarkerStyle Style
        {
            get { return (_Style); }

            set
            {
                if (_Style != value)
                {
                    _Style = value;

                    OnTickMarkLayoutChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeStyle()
        {
            return (_Style != _DefaultStyle);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetStyle()
        {
            Style = _DefaultStyle;
        }

        #endregion

        #region Overlap

        /// <summary>
        /// Gets or sets how the TickMark overlaps previous TickMarks
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(GaugeTickMarkOverlap.ReplaceAll)]
        [Description("Indicates how the TickMark overlaps previous TickMarks.")]
        public GaugeTickMarkOverlap Overlap
        {
            get { return (_Overlap); }

            set
            {
                if (_Overlap != value)
                {
                    _Overlap = value;

                    OnTickMarkLayoutChanged();
                }
            }
        }

        #endregion

        #region Width

        /// <summary>
        /// Gets or sets the Width of the TickMark, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.WidthRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Width of the TickMark, specified as a percentage.")]
        public float Width
        {
            get { return (_Width); }

            set
            {
                if (value < 0)
                    throw new ArgumentException("Value can not be less than zero.");

                if (_Width != value)
                {
                    _Width = value;

                    OnTickMarkLayoutChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWidth()
        {
            return (_Width != _DefaultWidth);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWidth()
        {
            Width = _DefaultWidth;
        }

        #endregion

        #endregion

        #region Internal properties

        #region Bitmap

        internal Bitmap Bitmap
        {
            get { return (_Bitmap); }

            set
            {
                if (_Bitmap != null)
                    _Bitmap.Dispose();

                _Bitmap = value;
            }
        }

                #endregion

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
            }
            else
            {
                if (_FillColor != null)
                    _FillColor.ColorTableChanged -= FillColor_ColorTableChanged;
            }
        }

        #endregion

        #region Event processing

        void FillColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnTickMarkLayoutChanged();
        }

        #endregion

        #region OnTickMarkLayoutChanged

        protected virtual void OnTickMarkLayoutChanged()
        {
            if (TickMarkLayoutChanged != null)
                TickMarkLayoutChanged(this, EventArgs.Empty);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            HookEvents(false);
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            TickMarkLayout copy = new
                TickMarkLayout(_Style, _Width, _Length);

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal virtual void CopyToItem(TickMarkLayout copy)
        {
            if (_FillColor != null)
                copy.FillColor = (GradientFillColor) _FillColor.Clone();

            copy.Image = _Image;
            copy.Length = _Length;
            copy.Placement = _Placement;
            copy.ScaleOffset = _ScaleOffset;
            copy.Style = _Style;
            copy.Overlap = _Overlap;
            copy.Width = _Width;
        }

        #endregion
    }

    #region TickMarkLayoutConvertor

    public class TickMarkLayoutConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                TickMarkLayout layout = value as TickMarkLayout;

                if (layout != null)
                {
                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion
}
