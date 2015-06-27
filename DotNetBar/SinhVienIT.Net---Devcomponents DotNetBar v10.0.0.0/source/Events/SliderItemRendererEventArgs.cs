using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the Slider item rendering events.
    /// </summary>
    public class SliderItemRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the reference to the item being rendered.
        /// </summary>
        public SliderItem SliderItem = null;
        
        /// <summary>
        /// Gets or sets the reference to graphics object.
        /// </summary>
        public Graphics Graphics = null;

        internal ItemPaintArgs ItemPaintArgs = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values.
        /// </summary>
        /// <param name="overflowItem">Reference to the Slider item being rendered.</param>
        /// <param name="g">Reference to the graphics object.</param>
        public SliderItemRendererEventArgs(SliderItem item, Graphics g)
        {
            this.SliderItem = item;
            this.Graphics = g;
        }
    }
}
