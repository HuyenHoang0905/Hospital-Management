using System;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display
{
    /// <summary>
    /// Provides data for RenderColumnHeader event.
    /// </summary>
    public class ColumnHeaderRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the column header that is rendered.
        /// </summary>
        public ColumnHeader ColumnHeader = null;
        /// <summary>
        /// Target Graphics canvas.
        /// </summary>
        public Graphics Graphics;
        /// <summary>
        /// Gets the bounds of the column header.
        /// </summary>
        public Rectangle Bounds;
        /// <summary>
        /// Gets the effective style for the column.
        /// </summary>
        public ElementStyle Style = null;
        /// <summary>
        /// Gets the AdvTree control header is rendered for.
        /// </summary>
        public AdvTree Tree = null;
        /// <summary>
        /// Gets or sets the color of the column sort indicator.
        /// </summary>
        public Color SortIndicatorColor = Color.Empty;

        /// <summary>
        /// Initializes a new instance of the ColumnHeaderRendererEventArgs class.
        /// </summary>
        public ColumnHeaderRendererEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ColumnHeaderRendererEventArgs class.
        /// </summary>
        /// <param name="columnHeader"></param>
        /// <param name="graphics"></param>
        /// <param name="bounds"></param>
        /// <param name="style"></param>
        public ColumnHeaderRendererEventArgs(AdvTree tree, ColumnHeader columnHeader, Graphics graphics, Rectangle bounds, ElementStyle style)
        {
            Tree = tree;
            ColumnHeader = columnHeader;
            Graphics = graphics;
            Bounds = bounds;
            Style = style;
        }
    }
}
