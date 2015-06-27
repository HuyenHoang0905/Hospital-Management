using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GradientFillColorConvertor))]
    [Editor(typeof(GradientFillColorEditor), typeof(UITypeEditor))]
    public class GradientFillColor : LinearGradientColorTable, ICloneable
    {
        #region Private properties

        private int _BorderWidth;
        private Color _BorderColor;
        private GradientFillType _GradientFillType = GradientFillType.Auto;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public GradientFillColor() { }
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="color">Beginning color.</param>
        public GradientFillColor(Color color)
        {
            Color1 = color;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="color1">Begin color.</param>
        /// <param name="color2">End color.</param>
        public GradientFillColor(Color color1, Color color2)
        {
            Color1 = color1;
            Color2 = color2;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="color1">Beginning color in hexadecimal representation like FFFFFF.</param>
        /// <param name="color2">End color in hexadecimal representation like FFFFFF.</param>
        public GradientFillColor(string color1, string color2)
        {
            Color1 = ColorFactory.GetColor(color1);
            Color2 = ColorFactory.GetColor(color2);
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="color1">Beginning color in 32-bit RGB representation.</param>
        /// <param name="color2">End color in 32-bit RGB representation.</param>
        public GradientFillColor(int color1, int color2)
        {
            Color1 = ColorFactory.GetColor(color1);
            Color2 = ColorFactory.GetColor(color2);
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="color1">Beginning color in 32-bit RGB representation.</param>
        /// <param name="color2">End color in 32-bit RGB representation.</param>
        /// <param name="gradientAngle">Gradient angle.</param>
        public GradientFillColor(int color1, int color2, int gradientAngle)
        {
            Color1 = ColorFactory.GetColor(color1);
            Color2 = ColorFactory.GetColor(color2);
            GradientAngle = gradientAngle;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="color1">Beginning color.</param>
        /// <param name="color2">End color.</param>
        /// <param name="gradientAngle">Gradient angle.</param>
        public GradientFillColor(Color color1, Color color2, int gradientAngle)
        {
            Color1 = color1;
            Color2 = color2;
            GradientAngle = gradientAngle;
        }

        #endregion

        #region Hidden properties

// ReSharper disable UnusedMember.Local
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color Start
// ReSharper restore UnusedMember.Local
        {
            get { return (base.Start); }
            set { base.Start = value; }
        }

        // ReSharper disable UnusedMember.Local
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color End
        // ReSharper restore UnusedMember.Local
        {
            get { return (base.End); }
            set { base.End = value; }
        }

        #endregion

        #region Public properties

        #region Color1

        /// <summary>
        /// Gets or sets the beginning gradient Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the beginning gradient Color.")]
        public Color Color1
        {
            get { return (base.Start); }
            set { base.Start = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeColor1()
        {
            return (Color1.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetColor1()
        {
            Color1 = Color.Empty;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal override bool ShouldSerializeStart()
        {
            return (false);
        }

        #endregion

        #region Color2

        /// <summary>
        /// Gets or sets the ending gradient Color
        /// </summary>
        /// <returns></returns>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the ending gradient Color.")]
        public Color Color2
        {
            get { return (base.End); }
            set { base.End = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeColor2()
        {
            return (Color2.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetColor2()
        {
            Color2 = Color.Empty;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal override bool ShouldSerializeEnd()
        {
            return (false);
        }

        #endregion

        #region BorderColor

        /// <summary>
        /// Gets or sets the border Color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the border Color.")]
        public Color BorderColor
        {
            get { return (_BorderColor); }

            set
            {
                if (_BorderColor != value)
                {
                    _BorderColor = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeBorderColor()
        {
            return (_BorderColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetBorderColor()
        {
            BorderColor = Color.Empty;
        }

        #endregion

        #region BorderWidth

        /// <summary>
        /// Gets or sets the border width
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(0)]
        [Description("Indicates the border width.")]
        public int BorderWidth
        {
            get { return (_BorderWidth); }

            set
            {
                if (_BorderWidth != value)
                {
                    _BorderWidth = value;

                    OnColorTableChanged();
                }
            }
        }

        #endregion

        #region GradientFillType

        /// <summary>
        /// Gets or sets the Gradient FillType
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue(GradientFillType.Auto)]
        [Description("Indicates the Gradient FillType.")]
        public GradientFillType GradientFillType
        {
            get { return (_GradientFillType); }

            set
            {
                if (_GradientFillType != value)
                {
                    _GradientFillType = value;

                    OnColorTableChanged();
                }
            }
        }

        #endregion

        #endregion

        #region IsEqualTo

        /// <summary>
        /// Determines if the fillcolor is equal to the given one
        /// </summary>
        /// <param name="fillColor">FillColor to compare</param>
        /// <returns>true if equal</returns>
        public bool IsEqualTo(GradientFillColor fillColor)
        {
            return (Color1 == fillColor.Color1 && Color2 == fillColor.Color2 &&
                GradientAngle == fillColor.GradientAngle && GradientFillType == fillColor.GradientFillType &&
                BorderColor == fillColor.BorderColor && BorderWidth == fillColor._BorderWidth);
        }

        /// <summary>
        /// Determines if the fillcolor is equal to the given one
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="angle"></param>
        /// <param name="gradientFill"></param>
        /// <param name="borderColor"></param>
        /// <param name="borderWidth"></param>
        /// <returns></returns>
        public bool IsEqualTo(Color begin, Color end, float angle,
            GradientFillType gradientFill, Color borderColor, int borderWidth)
        {
            return (Color1 == begin && End == end &&
                GradientAngle == angle && GradientFillType == gradientFill &&
                borderColor == BorderColor && borderWidth == BorderWidth);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            GradientFillColor fillColor = new GradientFillColor();

            fillColor.Color1 = Color1;
            fillColor.Color2 = Color2;
            fillColor.BorderColor = BorderColor;
            fillColor.BorderWidth = BorderWidth;
            fillColor.GradientFillType = GradientFillType;
            fillColor.GradientAngle = GradientAngle;

            return (fillColor);
        }

        #endregion
    }

    #region Enums

    public enum GradientFillType
    {
        Auto,
        Angle,
        Center,
        HorizontalCenter,
        None,
        VerticalCenter,
        StartToEnd,
    }

    #endregion

    #region GradientFillColorConvertor

    public class GradientFillColorConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GradientFillColor gc = value as GradientFillColor;

                if (gc != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    string s = gc.Start.IsEmpty ? "(Empty)" : cvt.ConvertToString(gc.Start);

                    if (gc.End != Color.Empty)
                        s += ", " + cvt.ConvertToString(gc.End);

                    return (s);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

    #region GradientFillColorEditor

    public class GradientFillColorEditor : UITypeEditor
    {
        #region GetPaintValueSupported

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return (true);
        }

        #endregion

        #region PaintValue

        public override void PaintValue(PaintValueEventArgs e)
        {
            GradientFillColor fc = e.Value as GradientFillColor;

            if (fc != null)
            {
                GradientFillType fillType = GetGradientFillType(fc);

                switch (fillType)
                {
                    case GradientFillType.None:
                        using (Brush br = new SolidBrush(fc.Start))
                            e.Graphics.FillRectangle(br, e.Bounds);
                        break;

                    case GradientFillType.Auto:
                    case GradientFillType.Angle:
                        using (Brush br = fc.GetBrush(e.Bounds))
                            e.Graphics.FillRectangle(br, e.Bounds);
                        break;
                        
                    case GradientFillType.Center:
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            path.AddRectangle(e.Bounds);

                            using (PathGradientBrush br = new PathGradientBrush(path))
                            {
                                br.CenterColor = fc.Start;
                                br.SurroundColors = new Color[] {fc.End};

                                e.Graphics.FillPath(br, path);
                            }
                        }
                        break;

                    case GradientFillType.HorizontalCenter:
                        Rectangle t1 = e.Bounds;
                        t1.Height /= 2;

                        using (LinearGradientBrush br = new LinearGradientBrush(t1, fc.End, fc.Color1, 90))
                        {
                            br.WrapMode = WrapMode.TileFlipXY;

                            e.Graphics.FillRectangle(br, e.Bounds);
                        }
                        break;

                    case GradientFillType.StartToEnd:
                        using (Brush br = fc.GetBrush(e.Bounds, 0))
                            e.Graphics.FillRectangle(br, e.Bounds);
                        break;

                    case GradientFillType.VerticalCenter:
                        Rectangle t2 = e.Bounds;
                        t2.Width /= 2;

                        using (LinearGradientBrush br = new LinearGradientBrush(t2, fc.End, fc.Color1, 0f))
                        {
                            br.WrapMode = WrapMode.TileFlipXY;

                            e.Graphics.FillRectangle(br, e.Bounds);
                        }
                        break;
                }
            }
        }

        #endregion

        #region GetGradientFillType

        private GradientFillType GetGradientFillType(GradientFillColor fc)
        {
            return (fc.End.IsEmpty ? GradientFillType.None : fc.GradientFillType);
        }

        #endregion
    }

    #endregion
}
