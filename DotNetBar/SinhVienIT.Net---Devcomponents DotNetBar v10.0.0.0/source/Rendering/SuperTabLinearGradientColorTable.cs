using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering
{
    [TypeConverter(typeof(SuperTabLinearGradientColorTableConvertor))]
    public class SuperTabLinearGradientColorTable
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabLinearGradientColorTable is changed
        /// </summary>
        [Description("Event raised when the SuperTabLinearGradientColorTable is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private Color[] _Colors;
        private float[] _Positions;
        private int _GradientAngle = 90;
        private bool? _AdaptiveGradient;

        #endregion

        #region Constructors

        public SuperTabLinearGradientColorTable()
        {
        }

        public SuperTabLinearGradientColorTable(Color color)
        {
            _Colors = new Color[1];

            _Colors[0] = color;
        }

        public SuperTabLinearGradientColorTable(Color start, Color end)
        {
            _Colors = new Color[2];

            _Colors[0] = start;
            _Colors[1] = end;
        }

        public SuperTabLinearGradientColorTable(Color start, Color end, int gradient)
        {
            _Colors = new Color[2];

            _Colors[0] = start;
            _Colors[1] = end;
            _GradientAngle = gradient;
        }

        public SuperTabLinearGradientColorTable(Color[] colors, float[] positions)
        {
            _Colors = colors;
            _Positions = positions;
        }

        #endregion

        #region Public properties

        #region Colors

        /// <summary>
        /// Gets or sets the Gradient Colors
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Gradient Colors.")]
        public Color[] Colors
        {
            get { return (_Colors); }

            set
            {
                if (_Colors != value)
                {
                    _Colors = value;

                    OnColorTableChanged();
                }
            }
        }

        #endregion

        #region Positions

        /// <summary>
        /// Gets or sets the Gradient Color Positions
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Gradient Color Positions.")]
        public float[] Positions
        {
            get { return (_Positions); }

            set
            {
                if (_Positions != value)
                {
                    _Positions = value;

                    OnColorTableChanged();
                }
            }
        }

        #endregion

        #region GradientAngle

        /// <summary>
        /// Gets or sets the Gradient angle
        /// </summary>
        [Browsable(true), DefaultValue(90)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Gradient angle.")]
        public int GradientAngle
        {
            get { return (_GradientAngle); }

            set
            {
                if (_GradientAngle != value)
                {
                    _GradientAngle = value;

                    OnColorTableChanged();
                }
            }
        }

        #endregion

        #region AdaptiveGradient

        /// <summary>
        /// Gets or sets whether the Gradient will adapt to changes in the TabAlignment
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [NotifyParentProperty(true)]
        [Description("Indicates whether the Gradient will adapt to changes in the TabAlignment.")]
        public bool? AdaptiveGradient
        {
            get { return (_AdaptiveGradient); }

            set
            {
                if (_AdaptiveGradient != value)
                {
                    _AdaptiveGradient = value;

                    OnColorTableChanged();
                }
            }
        }

        #endregion

        #region IsEmpty

        /// <summary>
        /// Gets whether color definition is empty
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return ((_Colors == null || _Colors.Length == 0) && 
                    (_Positions == null || _Positions.Length == 0) &&
                    _GradientAngle == 90 && _AdaptiveGradient.HasValue == false);
            }
        }

        #endregion

        #endregion

        #region OnColorTableChanged

        protected void OnColorTableChanged()
        {
            if (ColorTableChanged != null)
                ColorTableChanged(this, EventArgs.Empty);
        }

        #endregion

        #region GetBrush

        public Brush GetBrush(Rectangle r)
        {
            return (GetBrush(r, _GradientAngle));
        }

        public Brush GetBrush(Rectangle r, int angle)
        {
            if (_Colors != null && _Colors.Length > 0)
            {
                if (_Colors.Length == 1)
                    return (new SolidBrush(Colors[0]));

                LinearGradientBrush lbr =
                    new LinearGradientBrush(r, Color.White, Color.White, angle);

                lbr.InterpolationColors = GetColorBlend();

                return (lbr);
            }

            return (null);
        }

        #region GetColorBlend

        private ColorBlend GetColorBlend()
        {
            ColorBlend cb = new ColorBlend(_Colors.Length);
            float[] cp = _Positions;

            if (cp == null || cp.Length != _Colors.Length)
            {
                cp = new float[_Colors.Length];

                float f = 1 / _Colors.Length;

                for (int i = 0; i < cp.Length; i++)
                    cp[i] = i * f;

                cp[_Colors.Length - 1] = 1;
            }

            for (int i = 0; i < _Colors.Length; i++)
                cb.Colors[i] = _Colors[i];

            cb.Positions = cp;

            return (cb);
        }

        #endregion

        #endregion

        #region GetColorBlendCollection

        public BackgroundColorBlendCollection GetColorBlendCollection()
        {
            BackgroundColorBlendCollection cbc = new BackgroundColorBlendCollection();

            float[] cp = _Positions;

            if (cp == null || cp.Length != _Colors.Length)
            {
                cp = new float[_Colors.Length];

                float f = 1 / _Colors.Length;

                for (int i = 0; i < cp.Length; i++)
                    cp[i] = i * f;

                cp[_Colors.Length - 1] = 1;
            }

            for (int i = 0; i < _Colors.Length; i++)
                cbc.Add(new BackgroundColorBlend(_Colors[i], cp[i]));

            return (cbc);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            SuperTabLinearGradientColorTable gct = new SuperTabLinearGradientColorTable();

            gct.Colors = Colors;
            gct.Positions = Positions;
            gct.GradientAngle = GradientAngle;
            gct.AdaptiveGradient = AdaptiveGradient;

            return (gct);
        }

        #endregion
    }

    #region SuperTabLinearGradientColorTableConvertor

    public class SuperTabLinearGradientColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                SuperTabLinearGradientColorTable lct =
                    value as SuperTabLinearGradientColorTable;

                if (lct != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    if (lct.Colors != null)
                    {
                        if (lct.Colors[0] != Color.Empty)
                            return (cvt.ConvertToString(lct.Colors[0]));

                        if (lct.Colors.Length > 1 && lct.Colors[1] != Color.Empty)
                            return (cvt.ConvertToString(lct.Colors[1]));
                    }

                    if (lct.GradientAngle != 90)
                        return (lct.GradientAngle.ToString());

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion
}
