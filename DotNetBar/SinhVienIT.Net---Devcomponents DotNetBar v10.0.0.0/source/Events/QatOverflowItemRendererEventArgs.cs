using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the Quick Access Toolbar Overflow item rendering events.
    /// </summary>
    public class QatOverflowItemRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the reference to the item being rendered.
        /// </summary>
        public QatOverflowItem OverflowItem = null;
        /// <summary>
        /// Gets or sets the reference to graphics object.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values.
        /// </summary>
        /// <param name="overflowItem">Reference to the overflow item being rendered.</param>
        /// <param name="g">Reference to the graphics object.</param>
        public QatOverflowItemRendererEventArgs(QatOverflowItem overflowItem, Graphics g)
        {
            this.OverflowItem = overflowItem;
            this.Graphics = g;
        }
    }
}
