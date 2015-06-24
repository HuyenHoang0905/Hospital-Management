using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the Navigation Pane rendering events.
    /// </summary>
    public class NavPaneRenderEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Gets or sets the rendering bounds.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;

        /// <summary>
        /// Creates new instance of the objects and initializes it with default values.
        /// </summary>
        public NavPaneRenderEventArgs(Graphics g, Rectangle bounds)
        {
            this.Graphics = g;
            this.Bounds = bounds;
        }
    }
}
