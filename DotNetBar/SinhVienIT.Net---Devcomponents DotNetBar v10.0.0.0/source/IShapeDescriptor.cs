using System;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    public interface IShapeDescriptor
    {
        /// <summary>
        /// Returns the shape that fits given bounds.
        /// </summary>
        /// <param name="bounds">Bounds to fit shape in.</param>
        /// <returns>GraphicsPath representing shape or null if shape cannot be created.</returns>
        GraphicsPath GetShape(Rectangle bounds);

        /// <summary>
        /// Returns the inner shape based on the specified border size.
        /// </summary>
        /// <param name="bounds">Bounds to fit shape in.</param>
        /// <returns>GraphicsPath representing shape or null if shape cannot be created.</returns>
        GraphicsPath GetInnerShape(Rectangle bounds, int borderSize);

        /// <summary>
        /// Returns whether shape can be drawn given the bounds.
        /// </summary>
        /// <param name="bounds">Bounds to test.</param>
        /// <returns>true if shape can be drawn inside of bounds otherwise false.</returns>
        bool CanDrawShape(Rectangle bounds);
    }
}
