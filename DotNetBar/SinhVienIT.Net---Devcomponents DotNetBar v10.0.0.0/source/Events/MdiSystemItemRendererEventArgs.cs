using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the RenderMdiSystemitem event.
    /// </summary>
    public class MdiSystemItemRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Gets or sets the reference to MdiSystemItem being rendered.
        /// </summary>
        public MDISystemItem MdiSystemItem = null;

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        /// <param name="g">Reference to graphics object.</param>
        /// <param name="mdi">Reference to MdiSystemItem being rendered.</param>
        public MdiSystemItemRendererEventArgs(Graphics g, MDISystemItem mdi)
        {
            this.Graphics = g;
            this.MdiSystemItem = mdi;
        }
    }
}
