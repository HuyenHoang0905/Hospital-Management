using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the Quick Access Toolbar Customize Item rendering events.
    /// </summary>
    public class QatCustomizeItemRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the reference to the item being rendered.
        /// </summary>
        public QatCustomizeItem CustomizeItem = null;
        /// <summary>
        /// Gets or sets the reference to graphics object.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values.
        /// </summary>
        /// <param name="overflowItem">Reference to the customize item being rendered.</param>
        /// <param name="g">Reference to the graphics object.</param>
        public QatCustomizeItemRendererEventArgs(QatCustomizeItem customizeItem, Graphics g)
        {
            this.CustomizeItem = customizeItem;
            this.Graphics = g;
        }
    }
}
