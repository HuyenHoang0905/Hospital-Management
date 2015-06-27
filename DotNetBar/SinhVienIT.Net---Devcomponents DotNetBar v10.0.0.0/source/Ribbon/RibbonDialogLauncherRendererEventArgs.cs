using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for RibbonBar rendering events.
    /// </summary>
    public class RibbonBarRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the reference to Graphics object.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Gets or sets the part bounds.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;

        /// <summary>
        /// Gets or sets the reference to RibbonBar.
        /// </summary>
        public RibbonBar RibbonBar = null;

        /// <summary>
        /// Gets or sets whether mouse over state should be painted for the ribbon bar part.
        /// </summary>
        public bool MouseOver = false;

        /// <summary>
        /// Gets or sets whether mouse is pressed over the ribbon part.
        /// </summary>
        public bool Pressed = false;

        /// <summary>
        /// Gets or sets the region that defines the content bounds. When background is rendered the renderer should set this property
        /// to define the content clip.
        /// </summary>
        public Region ContentClip = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values.
        /// </summary>
        /// <param name="g">Reference to Graphics object.</param>
        /// <param name="bounds">Bounds of the part to be rendered.</param>
        /// <param name="ribbon">Reference to ribbon bar.</param>
        public RibbonBarRendererEventArgs(Graphics g, Rectangle bounds, RibbonBar ribbon)
        {
            this.Graphics = g;
            this.Bounds = bounds;
            this.RibbonBar = ribbon;
        }
    }
}
