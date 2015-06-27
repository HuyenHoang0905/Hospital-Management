using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.WinForms.Drawing
{
    public class SolidFill : Fill
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SolidFill class.
        /// </summary>
        /// <param name="color"></param>
        public SolidFill(Color color)
        {
            _Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the SolidFill class.
        /// </summary>
        public SolidFill()
        {
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
            if (_Color.IsEmpty) return null;
            return new SolidBrush(_Color);
        }

        private Color _Color = Color.Empty;
        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        [Description("Indicates the fill color.")]
        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        /// <returns>true if property should be serialized</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeColor()
        {
            return !_Color.IsEmpty;
        }
        /// <summary>
        /// Sets the property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetColor()
        {
            Color = Color.Empty;
        }

        public override Pen CreatePen(int width)
        {
            if (!_Color.IsEmpty)
                return new Pen(_Color, width);
            return null;
        }
        #endregion
    }
}
