using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for ButtonItem rendering.
    /// </summary>
    public class RibbonTabItemRendererEventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;
        /// <summary>
        /// Gets the reference to ButtonItem instance being rendered.
        /// </summary>
        public RibbonTabItem RibbonTabItem = null;

        /// <summary>
        /// Reference to internal data.
        /// </summary>
        internal ItemPaintArgs ItemPaintArgs = null;

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public RibbonTabItemRendererEventArgs() { }

        /// <summary>
        /// Creates new instance of the object and initializes it with default values
        /// </summary>
        /// <param name="g">Reference to Graphics object.</param>
        /// <param name="button">Reference to ButtonItem object.</param>
        public RibbonTabItemRendererEventArgs(Graphics g, RibbonTabItem button)
        {
            this.Graphics = g;
            this.RibbonTabItem = button;
        }

        /// <summary>
        /// Creates new instance of the object and initializes it with default values
        /// </summary>
        /// <param name="g">Reference to Graphics object.</param>
        /// <param name="button">Reference to ButtonItem object.</param>
        internal RibbonTabItemRendererEventArgs(Graphics g, RibbonTabItem button, ItemPaintArgs pa)
        {
            this.Graphics = g;
            this.RibbonTabItem = button;
            this.ItemPaintArgs = pa;
        }
    }
}
