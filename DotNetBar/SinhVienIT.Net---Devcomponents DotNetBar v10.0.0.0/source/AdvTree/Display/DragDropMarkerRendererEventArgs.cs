using System;
using System.Text;
using System.Drawing;

namespace DevComponents.AdvTree.Display
{
    /// <summary>
    /// Provides data for the NodeRenderer.RenderDragDropMarker event.
    /// </summary>
    public class DragDropMarkerRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets reference to Graphics object, canvas node is rendered on.
        /// </summary>
        public System.Drawing.Graphics Graphics = null;
        /// <summary>
        /// Gets or sets the selection bounds.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;

        /// <summary>
        /// Initializes a new instance of the DragDropMarkerRendererEventArgs class.
        /// </summary>
        public DragDropMarkerRendererEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DragDropMarkerRendererEventArgs class.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="bounds"></param>
        public DragDropMarkerRendererEventArgs(System.Drawing.Graphics graphics, Rectangle bounds)
        {
            Graphics = graphics;
            Bounds = bounds;
        }
    }
}
