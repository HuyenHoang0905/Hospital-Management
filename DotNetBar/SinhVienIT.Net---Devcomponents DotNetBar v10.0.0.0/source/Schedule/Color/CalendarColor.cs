#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace DevComponents.DotNetBar.Schedule
{
    #region ColorDef

    /// <summary>
    /// Color definition class
    /// </summary>
    [TypeConverter(typeof(ColorDefConvertor))]
    public class ColorDef
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabColorStates is changed
        /// </summary>
        [Description("Event raised when the ColorDef is changed")]
        public event EventHandler<EventArgs> ColorDefChanged;

        #endregion

        #region Private variables

        private Color[] _Colors;        // Color values
        private float[] _Positions;     // Gradient color positions
        private float _Angle;           // Gradient angle

        #endregion

        #region Constructors

        public ColorDef()
        {
        }

        /// <summary>
        /// Constructor - solid def
        /// </summary>
        /// <param name="rgb">RGB value</param>
        public ColorDef(int rgb)
            : this(ColorScheme.GetColor(rgb))
        {
        }

        /// <summary>
        /// Constructor - solid def
        /// </summary>
        /// <param name="color">Color</param>
        public ColorDef(Color color)
        {
            _Colors = new Color[1];

            _Colors[0] = color;
            _Positions = null;
        }

        /// <summary>
        /// Constructor - 2 color def
        /// </summary>
        /// <param name="start">Start Color</param>
        /// <param name="end">End Color</param>
        public ColorDef(Color start, Color end)
            : this(start, end, 90f)
        {
        }

        /// <summary>
        /// Constructor - 2 color def
        /// </summary>
        /// <param name="start">Start Color</param>
        /// <param name="end">End Color</param>
        /// <param name="angle">Gradient angle</param>
        public ColorDef(Color start, Color end, float angle)
        {
            _Colors = new Color[] { start, end };
            _Positions = new float[] { 0, 1 };

            _Angle = angle;
        }

        /// <summary>
        /// Constructor - Gradient def
        /// </summary>
        /// <param name="rgbs">Array of RGB values</param>
        /// <param name="cPositions">Gradient positions</param>
        public ColorDef(int[] rgbs, float[] cPositions)
            : this(rgbs, cPositions, 90f)
        {
        }

        /// <summary>
        /// Constructor - Gradient def
        /// </summary>
        /// <param name="colors">Array of Color values</param>
        /// <param name="cPositions">Gradient positions</param>
        public ColorDef(Color[] colors, float[] cPositions)
            : this(colors, cPositions, 90f)
        {
        }

        /// <summary>
        /// Constructor - Gradient def
        /// </summary>
        /// <param name="rgbs">Array of RGB values</param>
        /// <param name="cPositions">Gradient positions</param>
        /// <param name="angle">Gradient angle</param>
        public ColorDef(int[] rgbs, float[] cPositions, float angle)
        {
            _Colors = new Color[rgbs.Length];

            for (int i = 0; i < rgbs.Length; i++)
                _Colors[i] = ColorScheme.GetColor(rgbs[i]);

            _Positions = cPositions;
            _Angle = angle;
        }

        /// <summary>
        /// Constructor - Gradient def
        /// </summary>
        /// <param name="colors">Array of Color values</param>
        /// <param name="cPositions">Gradient positions</param>
        /// <param name="angle">Gradient angle</param>
        public ColorDef(Color[] colors, float[] cPositions, float angle)
        {
            _Colors = colors;
            _Positions = cPositions;
            _Angle = angle;
        }

        #endregion

        #region Public properties

        #region Colors

        /// <summary>
        /// Gets or sets the Color array
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Color array")]
        public Color[] Colors
        {
            get { return (_Colors); }
            set { _Colors = value; OnColorDefChanged(); }
        }

        #endregion

        #region Positions

        /// <summary>
        /// Gets or sets the Color Positions
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Color Positions.")]
        public float[] Positions
        {
            get { return (_Positions); }
            set { _Positions = value; OnColorDefChanged(); }
        }

        #endregion

        #region Angle

        /// <summary>
        /// Gets or sets the Gradient Angle
        /// </summary>
        [Browsable(true), DefaultValue(0f)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Gradient Angle.")]
        public float Angle
        {
            get { return (_Angle); }
            set { _Angle = value; OnColorDefChanged(); }
        }

        #endregion

        #region IsEmpty

        /// <summary>
        /// IsEmpty
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEmpty
        {
            get { return (_Colors == null || 
                _Colors.Length == 1 && _Colors[0].IsEmpty); }
        }

        #endregion

        #endregion

        #region OnColorDefChanged

        /// <summary>
        /// OnColorDefChanged
        /// </summary>
        private void OnColorDefChanged()
        {
            if (ColorDefChanged != null)
                ColorDefChanged(this, EventArgs.Empty);
        }

        #endregion
    }

    #region ColorDefConvertor

    /// <summary>
    /// ColorDefConvertor
    /// </summary>
    public class ColorDefConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                ColorDef cd = value as ColorDef;

                if (cd != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    if (cd.Colors != null)
                    {
                        if (cd.Colors[0] != Color.Empty)
                            return (cvt.ConvertToString(cd.Colors[0]));

                        if (cd.Colors.Length > 1 && cd.Colors[1] != Color.Empty)
                            return (cvt.ConvertToString(cd.Colors[1]));
                    }

                    if (cd.Angle != 0)
                        return (cd.Angle.ToString());
                }

                return (String.Empty);
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

    #endregion

    #region CalendarColor

    public class CalendarColor
    {
        #region Private variables

        private eCalendarColor _ColorSch;   // Current color scheme enum
        private ColorDef[] _ColorTable;     // Color scheme definition

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colorSch">eCalendarColor</param>
        public CalendarColor(eCalendarColor colorSch)
        {
            _ColorSch = colorSch;

            SetColorTable();
        }

        #region Public properties

        /// <summary>
        /// Gets and sets ColorTable
        /// </summary>
        public ColorDef[] ColorTable
        {
            get { return (_ColorTable); }
            set { _ColorTable = value; }
        }

        /// <summary>
        /// Gets and sets calendar color scheme
        /// </summary>
        public eCalendarColor ColorSch
        {
            get { return (_ColorSch); }

            set
            {
                if (_ColorSch != value)
                {
                    _ColorSch = value;

                    SetColorTable();
                }
            }
        }

        #region SetColorTable

        public virtual void SetColorTable()
        {
        }

        #endregion

        #endregion

        #region Get Color

        /// <summary>
        /// Gets the Color of the calendar part
        /// </summary>
        /// <param name="part">Calendar part</param>
        /// <returns>Color</returns>
        public Color GetColor(int part)
        {
            return (_ColorTable[part].Colors[0]);
        }

        #endregion

        #region GetColorDef

        /// <summary>
        /// Gets the ColorDef of the part
        /// </summary>
        /// <param name="part">Calendar part</param>
        /// <returns>Part ColorDef</returns>
        public ColorDef GetColorDef(int part)
        {
            return (_ColorTable[part]);
        }

        #endregion

        #region BrushPart routines

        /// <summary>
        /// Creates a LinearGradientBrush from the given part
        /// </summary>
        /// <param name="part">Color part</param>
        /// <param name="r">Gradient Rectangle</param>
        /// <returns>Created Brush</returns>
        public Brush BrushPart(int part, Rectangle r)
        {
            return (BrushPart(GetColorDef(part), r));
        }

        /// <summary>
        /// Creates a LinearGradientBrush from the given ColorDef
        /// </summary>
        /// <param name="cDef">ColorDef</param>
        /// <param name="r">Gradient Rectangle</param>
        /// <returns>Created Brush</returns>
        public Brush BrushPart(ColorDef cDef, Rectangle r)
        {
            return (BrushPart(cDef, r, cDef.Angle));
        }
        
        /// <summary>
        /// Creates a LinearGradientBrush from the given ColorDef
        /// </summary>
        /// <param name="cDef">ColorDef</param>
        /// <param name="r">Gradient Rectangle</param>
        /// <param name="angle">Gradient angle</param>
        /// <returns>Created Brush</returns>
        public Brush BrushPart(ColorDef cDef, Rectangle r, float angle)
        {
            if (cDef.Colors.Length == 1)
                return (new SolidBrush(cDef.Colors[0]));

            LinearGradientBrush lbr =
                new LinearGradientBrush(r, Color.White, Color.White, angle);

            lbr.InterpolationColors = GetColorBlend(cDef);

            return (lbr);
        }

        /// <summary>
        /// Creates a ColorBlend from the given ColorDef
        /// </summary>
        /// <param name="cDef">ColorDef for blend</param>
        /// <returns>ColorBlend</returns>
        private ColorBlend GetColorBlend(ColorDef cDef)
        {
            ColorBlend cb = new ColorBlend(cDef.Colors.Length);

            // Set each Color and position from the
            // provided color definition

            cb.Colors = cDef.Colors;
            cb.Positions = GetPositions(cDef);

            return (cb);
        }

        /// <summary>
        /// Gets the array of color positions
        /// </summary>
        /// <param name="cDef"></param>
        /// <returns></returns>
        private float[] GetPositions(ColorDef cDef)
        {
            float[] cp = cDef.Positions;

            if (cp == null || cp.Length != cDef.Colors.Length)
            {
                cp = new float[cDef.Colors.Length];

                float f = 1f / cDef.Colors.Length;

                for (int i = 0; i < cp.Length; i++)
                    cp[i] = i * f;

                cp[cDef.Colors.Length - 1] = 1;
            }

            return (cp);
        }

        #endregion
    }

    #endregion
}
#endif

