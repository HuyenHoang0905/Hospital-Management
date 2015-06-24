using System;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Presentation
{
    /// <summary>
    /// Defines the shape border.
    /// </summary>
    internal class ShapeBorder
    {
        #region Properties
        /// <summary>
        /// Gets or sets the border width in pixels.
        /// </summary>
        public int Width = 0;

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        public Color Color1 = Color.Empty;

        /// <summary>
        /// Gets or sets the ending gradient border color.
        /// </summary>
        public Color Color2 = Color.Empty;

        /// <summary>
        /// Gets or sets the gradient angle. Default value is 90.
        /// </summary>
        public int GradientAngle = 90;
        #endregion

        #region Internal Implementation
        public ShapeBorder() {}
        public ShapeBorder(int borderWidth) { Width = borderWidth; }
        public ShapeBorder(LinearGradientColorTable table)
        {
            this.Color1 = table.Start;
            this.Color2 = table.End;
            this.Width = 1;
        }
        public ShapeBorder(LinearGradientColorTable table, int width)
        {
            this.Color1 = table.Start;
            this.Color2 = table.End;
            this.Width = width;
        }
        public void Apply(LinearGradientColorTable table)
        {
            if (table == null)
            {
                this.Color1 = Color.Empty;
                this.Color2 = Color.Empty;
            }
            else
            {
                this.Color1 = table.Start;
                this.Color2 = table.End;
            }
        }
        public ShapeBorder(Color color1, Color color2, int width)
        {
            this.Color1 = color1;
            this.Color2 = color2;
            this.Width = width;
        }
        public ShapeBorder(Color color1, int width)
        {
            this.Color1 = color1;
            this.Color2 = Color.Empty;
            this.Width = width;
        }
        #endregion
    }
}
