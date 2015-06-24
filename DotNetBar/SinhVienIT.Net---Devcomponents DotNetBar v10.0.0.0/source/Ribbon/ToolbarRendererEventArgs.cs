using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for toolbar rendering.
    /// </summary>
    public class ToolbarRendererEventArgs
    {
        /// <summary>
        /// Gets or sets the reference to Bar object being rendered
        /// </summary>
        public Bar Bar = null;

        /// <summary>
        /// Gets or sets Graphics object bar is rendered on.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Gets the bounds of the region that should be rendered.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;

        /// <summary>
        /// Reference to internal data.
        /// </summary>
        internal ItemPaintArgs ItemPaintArgs = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default data.
        /// </summary>
        /// <param name="bar">Reference to bar object.</param>
        /// <param name="g">Reference to Graphics object.</param>
        public ToolbarRendererEventArgs(Bar bar, Graphics g, Rectangle bounds)
        {
            this.Bar = bar;
            this.Graphics = g;
            this.Bounds = bounds;
        }
    }
}
