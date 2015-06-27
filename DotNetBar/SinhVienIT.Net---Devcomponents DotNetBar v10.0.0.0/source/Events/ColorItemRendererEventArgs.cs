using System;
using System.Drawing;
using System.Text;

namespace DevComponents.DotNetBar
{
    public class ColorItemRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;
        /// <summary>
        /// Gets the reference to ButtonItem instance being rendered.
        /// </summary>
        public ColorItem ColorItem = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values
        /// </summary>
        /// <param name="g">Reference to Graphics object.</param>
        /// <param name="item">Reference to ColorItem object.</param>
        public ColorItemRendererEventArgs(Graphics g, ColorItem item)
        {
            this.Graphics = g;
            this.ColorItem = item;
        }
    }
}
