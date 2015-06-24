using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DevComponents.WinForms.Drawing
{
    public class GradientFill : Fill
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the GradientFill class.
        /// </summary>
        public GradientFill()
        {
        }

        /// <summary>
        /// Initializes a new instance of the GradientFill class.
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        public GradientFill(Color color1, Color color2)
        {
            _Color1 = color1;
            _Color2 = color2;
        }

        /// <summary>
        /// Initializes a new instance of the GradientFill class.
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <param name="angle"></param>
        public GradientFill(Color color1, Color color2, float angle)
        {
            _Color1 = color1;
            _Color2 = color2;
            _Angle = angle;
        }

        /// <summary>
        /// Initializes a new instance of the GradientFill class.
        /// </summary>
        /// <param name="interpolationColors"></param>
        public GradientFill(ColorStop[] interpolationColors)
        {
            _InterpolationColors.AddRange(interpolationColors);
        }

        /// <summary>
        /// Initializes a new instance of the GradientFill class.
        /// </summary>
        /// <param name="interpolationColors"></param>
        public GradientFill(ColorStop[] interpolationColors, int angle)
        {
            _InterpolationColors.AddRange(interpolationColors);
            _Angle = angle;
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Creates the brush for fill.
        /// </summary>
        /// <param name="bounds">Bounds for the brush</param>
        /// <returns>Returns brush or null if brush cannot be created for given bounds or colors are not set. It is responsibility of caller to Dispose the brush.</returns>
        public override Brush CreateBrush(Rectangle bounds)
        {
            if (_Color1.IsEmpty && _Color2.IsEmpty && _InterpolationColors.Count == 0 || bounds.Width < 1 || bounds.Height < 1) return null;

            LinearGradientBrush brush=new LinearGradientBrush(bounds, _Color1, _Color2, _Angle);
            if (_InterpolationColors.Count == 0)
                return brush;
            brush.InterpolationColors = _InterpolationColors.GetColorBlend();

            return brush;
        }

        private Color _Color1 = Color.Empty;
        /// <summary>
        /// Gets or sets the starting gradient fill color.
        /// </summary>
        [Description("Indicates the fill color.")]
        public Color Color1
        {
            get { return _Color1; }
            set { _Color1 = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        /// <returns>true if property should be serialized</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeColor1()
        {
            return !_Color1.IsEmpty;
        }
        /// <summary>
        /// Sets the property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetColor1()
        {
            Color1 = Color.Empty;
        }

        private Color _Color2 = Color.Empty;
        /// <summary>
        /// Gets or sets the end gradient fill color.
        /// </summary>
        [Description("Indicates the fill color.")]
        public Color Color2
        {
            get { return _Color2; }
            set { _Color2 = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        /// <returns>true if property should be serialized</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeColor2()
        {
            return !_Color2.IsEmpty;
        }
        /// <summary>
        /// Sets the property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetColor2()
        {
            Color2 = Color.Empty;
        }

        private ColorBlendCollection _InterpolationColors = new ColorBlendCollection();
        /// <summary>
        /// Gets the collection that defines the multicolor gradient background.
        /// </summary>
        /// <remarks>
        /// Setting this property creates a multicolor gradient with one color at each position along the gradient line. Setting this property nullifies all previous color, position, and falloff settings for this gradient fill.
        /// </remarks>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection that defines the multicolor gradient background.")]
        public ColorBlendCollection InterpolationColors
        {
            get { return _InterpolationColors; }
        }

        private float _Angle = 90;
        /// <summary>
        /// Gets or sets the gradient fill angle. Default value is 90.
        /// </summary>
        [DefaultValue(90), Description("Indicates gradient fill angle.")]
        public float Angle
        {
            get { return _Angle; }
            set
            {
                _Angle = value;
            }
        }
        /// <summary>
        /// Creates a pen based on fill parameters.
        /// </summary>
        /// <param name="width">Width of the pen to create</param>
        /// <returns>new instance of pen or null if pen cannot be created.</returns>
        public override Pen CreatePen(int width)
        {
            if (!_Color1.IsEmpty)
                return new Pen(_Color1, width);
            if (!_Color2.IsEmpty)
                return new Pen(_Color2, width);
            return null;
        }
        #endregion

    }
}
