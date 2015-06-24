using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the RenderSystemCaptionItem event.
    /// </summary>
    public class SystemCaptionItemRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics control is rendered on.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Gets reference to SystemCaptionItem being rendered.
        /// </summary>
        public SystemCaptionItem SystemCaptionItem = null;

        /// <summary>
        /// Gets whether Windows Vista Glass is enabled.
        /// </summary>
        public bool GlassEnabled = false;

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        /// <param name="g">Reference to Graphics object.</param>
        /// <param name="item">Reference to item being rendered.</param>
        /// <param name="glassEnabled">Indicates whether Vista Glass effect is enabled.</param>
        public SystemCaptionItemRendererEventArgs(Graphics g, SystemCaptionItem item, bool glassEnabled)
        {
            this.Graphics = g;
            this.SystemCaptionItem = item;
            this.GlassEnabled = glassEnabled;
        }
    }

    
}
