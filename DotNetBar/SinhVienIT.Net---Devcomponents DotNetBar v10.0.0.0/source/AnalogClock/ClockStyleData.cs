using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Data storage class for clock visual styles.
    /// </summary>
    [Description("Clock Style"),
    TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class ClockStyleData : IDisposable, INotifyPropertyChanged
    {

        private eClockStyles _Style;
        /// <summary>
        /// Gets or sets the PredefinedStyles value for this style.
        /// </summary>
        [Browsable(false)]
        public eClockStyles Style
        {
            get { return _Style; }
            set
            {
                if (value != _Style)
                {
                    _Style = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Style"));
                }
            }
        }


        private eClockShapes _ClockShape;
        /// <summary>
        /// Gets or sets the clock shape value for this style.
        /// </summary>
        [DefaultValue(eClockShapes.Round),
        Category("Appearance"),
        Description("The clock shape for this style.")]
        public eClockShapes ClockShape
        {
            get { return _ClockShape; }
            set
            {
                if (value != _ClockShape)
                {
                    _ClockShape = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ClockShape"));
                }
            }
        }

        private void ColorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_Parent != null) _Parent.Invalidate();
        }

        private ColorData _BezelColor;
        /// <summary>
        /// Gets or sets the bezel color data for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The bezel color data for this style.")]
        public ColorData BezelColor
        {
            get { return _BezelColor; }
            set
            {
                if (value != _BezelColor)
                {
                    if (_BezelColor != null) _BezelColor.PropertyChanged -= ColorPropertyChanged;
                    _BezelColor = value;
                    if (_BezelColor != null) _BezelColor.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("BezelColor"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBezelColor()
        {
            BezelColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(255, 255, 255), Color.FromArgb(152, 152, 152), Color.FromArgb(120, 120, 120), 1.0f, 45.0f);
        }
        

        private float _BezelWidth;
        /// <summary>
        /// Gets or sets the width of clock bezel as a percentage value ranging from 0.0 to 1.0.
        /// </summary>
        [DefaultValue(0.03f),
        Category("Appearance"),
        Description("The width of clock bezel as a percentage value ranging from 0.0 to 1.0.")]
        public float BezelWidth
        {
            get { return _BezelWidth; }
            set
            {
                if (value != _BezelWidth)
                {
                    _BezelWidth = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BezelWidth"));
                }
            }
        }

        private ColorData _FaceColor;
        /// <summary>
        /// Gets or sets the face color data for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The face color data for this style.")]
        public ColorData FaceColor
        {
            get { return _FaceColor; }
            set
            {
                if (value != _FaceColor)
                {
                    if (_FaceColor != null) _FaceColor.PropertyChanged -= ColorPropertyChanged;
                    _FaceColor = value;
                    if (_FaceColor != null) _FaceColor.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("FaceColor"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFaceColor()
        {
            FaceColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(191, 204, 213), Color.FromArgb(255, 255, 255), Color.FromArgb(135, 145, 161), 1.0f, 45.0f);
        }

        private Image _FaceBackgroundImage;
        /// <summary>
        /// Gets or sets the face background image for this style.
        /// </summary>
        [DefaultValue(null),
        Category("Appearance"),
        Description("The face background image for this style.")]
        public Image FaceBackgroundImage
        {
            get { return _FaceBackgroundImage; }
            set
            {
                if (value != _FaceBackgroundImage)
                {
                    _FaceBackgroundImage = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("FaceBackgroundImage"));
                }
            }
        }

        private ClockHandStyleData _HourHandStyle;
        /// <summary>
        /// Gets or sets the hour hand style for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The hour hand style for this style.")]
        public ClockHandStyleData HourHandStyle
        {
            get { return _HourHandStyle; }
            set
            {
                if (value != _HourHandStyle)
                {
                    if (_HourHandStyle != null) _HourHandStyle.PropertyChanged -= ColorPropertyChanged;
                    _HourHandStyle = value;
                    if (_HourHandStyle != null) _HourHandStyle.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("HourHandStyle"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHourHandStyle()
        {
            HourHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.55f, 0.01f);
        }

        private ClockHandStyleData _MinuteHandStyle;
        /// <summary>
        /// Gets or sets the minute hand style for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The minute hand style for this style.")]
        public ClockHandStyleData MinuteHandStyle
        {
            get { return _MinuteHandStyle; }
            set
            {
                if (value != _MinuteHandStyle)
                {
                    if (_MinuteHandStyle != null) _MinuteHandStyle.PropertyChanged -= ColorPropertyChanged;
                    _MinuteHandStyle = value;
                    if (_MinuteHandStyle != null) _MinuteHandStyle.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("MinuteHandStyle"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMinuteHandStyle()
        {
            MinuteHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.8f, 0.01f);
        }


        private ClockHandStyleData _SecondHandStyle;
        /// <summary>
        /// Gets or sets the second hand style for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The second hand style for this style.")]
        public ClockHandStyleData SecondHandStyle
        {
            get { return _SecondHandStyle; }
            set
            {
                if (value != _SecondHandStyle)
                {
                    if (_SecondHandStyle != null) _SecondHandStyle.PropertyChanged -= ColorPropertyChanged;
                    _SecondHandStyle = value;
                    if (_SecondHandStyle != null) _SecondHandStyle.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("SecondHandStyle"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSecondHandStyle()
        {
            SecondHandStyle = new ClockHandStyleData(eHandStyles.Style2, 0.8f, 0.005f);
        }


        private ColorData _CapColor;
        /// <summary>
        /// Gets or sets the center cap color data for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The center cap color data for this style.")]
        public ColorData CapColor
        {
            get { return _CapColor; }
            set
            {
                if (value != _CapColor)
                {
                    if (_CapColor != null) _CapColor.PropertyChanged -= ColorPropertyChanged;
                    _CapColor = value;
                    if (_CapColor != null) _CapColor.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("CapColor"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCapColor()
        {
            CapColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
        }

        private float _CapSize;
        /// <summary>
        /// Gets or sets the center cap diameter as a percentage value ranging from 0.0 to 1.0.
        /// </summary>
        [DefaultValue(0.03f),
        Category("Appearance"),
        Description("The center cap diameter as a percentage value ranging from 0.0 to 1.0.")]
        public float CapSize
        {
            get { return _CapSize; }
            set
            {
                if (value != _CapSize)
                {
                    _CapSize = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CapSize"));
                }
            }
        }

        private Color _NumberColor;
        /// <summary>
        /// Gets or sets the face number color for this style.
        /// </summary>
        [DefaultValue(typeof(Color), "139, 158, 168"), 
        Category("Appearance"),
        Description("The face number color for this style.")]
        public Color NumberColor
        {
            get { return _NumberColor; }
            set
            {
                if (value != _NumberColor)
                {
                    _NumberColor = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("NumberColor"));
                }
            }
        }
        
        private Font _NumberFont;
        /// <summary>
        /// Gets or sets the center cap color data for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The face number font for this style.")]
        public Font NumberFont
        {
            get { return _NumberFont; }
            set
            {
                if (value != _NumberFont)
                {
                    _NumberFont = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("NumberFont"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetNumberFont()
        {
            _NumberFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        private ColorData _LargeTickColor;
        /// <summary>
        /// Gets or sets the large tick color data for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The large tick color data for this style.")]
        public ColorData LargeTickColor
        {
            get { return _LargeTickColor; }
            set
            {
                if (value != _LargeTickColor)
                {
                    if (_LargeTickColor != null) _LargeTickColor.PropertyChanged -= ColorPropertyChanged;
                    _LargeTickColor = value;
                    if (_LargeTickColor != null) _LargeTickColor.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("LargeTickColor"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLargeTickColor()
        {
            LargeTickColor = new ColorData(eBrushTypes.Centered, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(255, 255, 255), 1.0f);
        }

        private float _LargeTickLength;
        /// <summary>
        /// Gets or sets the large tick length as a percentage value ranging from 0.0 to 1.0.
        /// </summary>
        [DefaultValue(0.06f),
        Category("Appearance"),
        Description("The large tick length as a percentage value ranging from 0.0 to 1.0.")]
        public float LargeTickLength
        {
            get { return _LargeTickLength; }
            set
            {
                if (value != _LargeTickLength)
                {
                    _LargeTickLength = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("LargeTickLength"));
                }
            }
        }

        private float _LargeTickWidth;
        /// <summary>
        /// Gets or sets the large tick width as a percentage value ranging from 0.0 to 1.0.
        /// </summary>
        [DefaultValue(0.02f),
        Category("Appearance"),
        Description("The large tick width as a percentage value ranging from 0.0 to 1.0.")]
        public float LargeTickWidth
        {
            get { return _LargeTickWidth; }
            set
            {
                if (value != _LargeTickWidth)
                {
                    _LargeTickWidth = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("LargeTickWidth"));
                }
            }
        }

        private ColorData _SmallTickColor;
        /// <summary>
        /// Gets or sets the small tick color data for this style.
        /// </summary>
        [Category("Appearance"),
        Description("The small tick color data for this style.")]
        public ColorData SmallTickColor
        {
            get { return _SmallTickColor; }
            set
            {
                if (value != _SmallTickColor)
                {
                    if (_SmallTickColor != null) _SmallTickColor.PropertyChanged -= ColorPropertyChanged;
                    _SmallTickColor = value;
                    if (_SmallTickColor != null) _SmallTickColor.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("SmallTickColor"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSmallTickColor()
        {
            SmallTickColor = new ColorData(eBrushTypes.Centered, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(255, 255, 255), 1.0f);
        }

        private float _SmallTickLength;
        /// <summary>
        /// Gets or sets the small tick length as a percentage value ranging from 0.0 to 1.0.
        /// </summary>
        [DefaultValue(0.02f),
        Category("Appearance"),
        Description("The small tick length as a percentage value ranging from 0.0 to 1.0.")]
        public float SmallTickLength
        {
            get { return _SmallTickLength; }
            set
            {
                if (value != _SmallTickLength)
                {
                    _SmallTickLength = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SmallTickLength"));
                }
            }
        }

        private float _SmallTickWidth;
        /// <summary>
        /// Gets or sets the small tick width as a percentage value ranging from 0.0 to 1.0.
        /// </summary>
        [DefaultValue(0.02f),
        Category("Appearance"),
        Description("The small tick width as a percentage value ranging from 0.0 to 1.0.")]
        public float SmallTickWidth
        {
            get { return _SmallTickWidth; }
            set
            {
                if (value != _SmallTickWidth)
                {
                    _SmallTickWidth = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SmallTickWidth"));
                }
            }
        }

        private int _GlassAngle;
        /// <summary>
        /// Gets or sets the overlay glass angle, in degrees for this style.
        /// </summary>
        [DefaultValue(20),
        Category("Appearance"),
        Description("The overlay angle, in degrees for this style.")]
        public int GlassAngle
        {
            get { return _GlassAngle; }
            set
            {
                if (value != _GlassAngle)
                {
                    _GlassAngle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("_GlassAngle"));
                }
            }
        }

        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the ClockStyle class.
        /// </summary>
        public ClockStyleData()
        {
            LoadStyle(eClockStyles.Style1);
        }

        private AnalogClockControl _Parent = null;
        /// <summary>
        /// Gets the parent of the style.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AnalogClockControl Parent
        {
            get { return _Parent; }
            internal set { _Parent = value; }
        }

        /// <summary>
        /// Initializes a new instance of the ClockStyle class.
        /// </summary>
        /// <param name="style">Predefined style from the PredefinedStyles enum.</param>
        public ClockStyleData(eClockStyles style)
        {
            LoadStyle(style);
        }

        /// <summary>
        /// Initializes a new instance of the ClockStyle class.
        /// </summary>
        /// <param name="style">Predefined style from the PredefinedStyles enum.</param>
        public ClockStyleData(eClockStyles style, AnalogClockControl parent)
        {
            LoadStyle(style);
            _Parent = parent;
        }

        /// <summary>
        /// Releases all resources used by the class. 
        /// </summary>
        public void Dispose()
        {
            if (_FaceBackgroundImage != null)
                _FaceBackgroundImage.Dispose();
            if (_NumberFont != null)
                _NumberFont.Dispose();
        }

        /// <summary>
        /// Loads a predefined style
        /// </summary>
        /// <param name="style">The predefined style to load.</param>
        private void LoadStyle(eClockStyles style)
        {
            _Style = style;
            switch (style)
            {
                case eClockStyles.Style1:
                case eClockStyles.Custom:
                    _ClockShape = eClockShapes.Round;
                    BezelColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(255, 255, 255), Color.FromArgb(152, 152, 152), Color.FromArgb(120, 120, 120), 0.01f);
                    _BezelWidth = 0.03f;
                    FaceColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(191, 204, 213), Color.FromArgb(255, 255, 255), Color.FromArgb(135, 145, 161), 0.01f, 45.0f);
                    _FaceBackgroundImage = null;
                    HourHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.55f, 0.015f);
                    MinuteHandStyle = new ClockHandStyleData(eHandStyles.Style1, 0.8f, 0.01f);
                    SecondHandStyle = new ClockHandStyleData(eHandStyles.Style2, 0.8f, 0.005f);
                    CapColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
                    _CapSize = 0.03f;
                    _NumberColor = Color.FromArgb(139, 158, 168);
                    _NumberFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Pixel);
                    LargeTickColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(128, 255, 255, 255), 0.01f);
                    _LargeTickLength = 0.06f;
                    _LargeTickWidth = 0.02f;
                    SmallTickColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(122, 142, 154), Color.FromArgb(122, 142, 154), Color.FromArgb(128, 255, 255, 255), 0.01f);
                    _SmallTickLength = 0.02f;
                    _SmallTickWidth = 0.02f;
                    _GlassAngle = -20;
                    break;
                case eClockStyles.Style2:
                    _ClockShape = eClockShapes.Round;
                    BezelColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(80, 80, 80), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), 0.0f, 90.0f);
                    _BezelWidth = 0.03f;
                    FaceColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(225, 225, 225), Color.FromArgb(240, 240, 240), Color.FromArgb(0, 0, 0), 0.0f, 90.0f);
                    _FaceBackgroundImage = null;
                    HourHandStyle = new ClockHandStyleData(eHandStyles.Style3, 0.45f, 0.175f);
                    HourHandStyle.HandColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(0, 0, 0), Color.FromArgb(80, 80, 80), Color.FromArgb(64, 0, 0, 0), 0.01f, 90.0f);
                    MinuteHandStyle = new ClockHandStyleData(eHandStyles.Style3, 0.75f, 0.175f);
                    MinuteHandStyle.HandColor = new ColorData(eBrushTypes.Linear, Color.FromArgb(0, 0, 0), Color.FromArgb(80, 80, 80), Color.FromArgb(64, 0, 0, 0), 0.01f, 90.0f);
                    SecondHandStyle = new ClockHandStyleData(eHandStyles.Style4, 0.9f, 0.01f);
                    SecondHandStyle.HandColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0), Color.FromArgb(128, 192, 0, 0), 0.01f);
                    _SecondHandStyle.DrawOverCap = true;
                    CapColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(223, 0, 0, 0), 0.01f);
                    _CapSize = 0.1f;
                    _NumberColor = Color.FromArgb(0, 0, 0);
                    _NumberFont = new Font("Trebuchet MS", 12, FontStyle.Regular, GraphicsUnit.Pixel);
                    LargeTickColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(64, 0, 0, 0), 0.01f);
                    _LargeTickLength = 0.06f;
                    _LargeTickWidth = 0.01f;
                    SmallTickColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(64, 0, 0, 0), 0.01f);
                    _SmallTickLength = 0.01f;
                    _SmallTickWidth = 0.01f;
                    _GlassAngle = 0;
                    break;
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
            if (_Parent != null) _Parent.Invalidate();
        }
    }

    /// <summary>
    /// Enumeration containing the predefined clock styles.
    /// </summary>
    public enum eClockStyles
    {
        /// <summary>
        /// Style 1. Default style,
        /// </summary>
        Style1,

        /// <summary>
        /// Style 2.
        /// </summary>
        Style2,

        /// <summary>
        /// No predefined style.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Enumeration containing the predefined clock shapes.
    /// </summary>
    public enum eClockShapes
    {
        /// <summary>
        /// Round clock shape.
        /// </summary>
        Round
    }
}
