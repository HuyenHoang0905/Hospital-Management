using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.WinForms.Drawing
{
    public class SolidBorder : Border
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SolidBorder class.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        public SolidBorder(Color color, int width)
        {
            _Color = color;
            _Width = width;
        }

        /// <summary>
        /// Initializes a new instance of the SolidBorder class.
        /// </summary>
        /// <param name="color"></param>
        public SolidBorder(Color color)
        {
            _Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the SolidBorder class.
        /// </summary>
        public SolidBorder()
        {
        }
        #endregion
        #region Internal Implementation
        /// <summary>
        /// Creates the pen for the border.
        /// </summary>
        /// <returns>Returns pen or null if pen cannot be created.</returns>
        public override Pen CreatePen()
        {
            if (!CanCreatePen()) return null;

            return new Pen(_Color, _Width);
        }

        private bool CanCreatePen()
        {
            return !_Color.IsEmpty && _Width > 0;
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

        
        #endregion
    }
}
