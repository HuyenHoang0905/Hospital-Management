using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.WinForms.Drawing
{
    [ToolboxItem(false)]
    public abstract class Border : Component
    {
        #region Internal Implementation
        /// <summary>
        /// Creates the pen for the border.
        /// </summary>
        /// <returns>Returns pen or null if pen cannot be created.</returns>
        public abstract Pen CreatePen();

        internal int _Width = 0;
        /// <summary>
        /// Gets or sets the border width. Default value is 0.
        /// </summary>
        [DefaultValue(0), Description("Indicates border width.")]
        public int Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
            }
        }

        internal static Rectangle Deflate(Rectangle bounds, Border border)
        {
            if (border == null) return bounds;
            bounds.Inflate(-border.Width, -border.Width);
            return bounds;
        }
        #endregion
    }
}
