using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents events arguments for the Ribbon Control rendering events.
    /// </summary>
    public class RibbonControlRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics control is rendered on.
        /// </summary>
        public Graphics Graphics = null;
        /// <summary>
        /// Gets the reference to RibbonControl instance being rendered.
        /// </summary>
        public RibbonControl RibbonControl = null;

        /// <summary>
        /// Gets whether Windows Vista Glass is enabled.
        /// </summary>
        public bool GlassEnabled = false;

        internal ItemPaintArgs ItemPaintArgs = null;

        /// <summary>
        /// Creates new instance and initializes it with the default values.
        /// </summary>
        /// <param name="g">Reference to Graphics object</param>
        /// <param name="rc">Reference to RibbonControl</param>
        public RibbonControlRendererEventArgs(Graphics g, RibbonControl rc, bool glassEnabled)
        {
            this.Graphics = g;
            this.RibbonControl = rc;
            this.GlassEnabled = glassEnabled;
        }
    }
}
