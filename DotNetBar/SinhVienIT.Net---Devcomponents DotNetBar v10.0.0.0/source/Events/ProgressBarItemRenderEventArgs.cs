using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the ProgressBarItem rendering events.
    /// </summary>
    public class ProgressBarItemRenderEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Gets or sets the reference to ProgressBarItem being rendered.
        /// </summary>
        public ProgressBarItem ProgressBarItem = null;

        /// <summary>
        /// Indicates whether item is in Right-To-Left environment.
        /// </summary>
        public bool RightToLeft = false;

        /// <summary>
        /// Gets or sets the text font.
        /// </summary>
        public Font Font = null;

        /// <summary>
        /// Creates new instance of the object and provides default values.
        /// </summary>
        /// <param name="g">Reference to Graphics object</param>
        /// <param name="item">Reference to ProgressBarItem</param>
        /// <param name="f">Indicates the font for the text.</param>
        /// <param name="rtl">Indicates whether item is in Right-To-Left environment.</param>
        public ProgressBarItemRenderEventArgs(Graphics g, ProgressBarItem item, Font f, bool rtl)
        {
            this.Graphics = g;
            this.ProgressBarItem = item;
            this.RightToLeft = rtl;
            this.Font = f;
        }
    }
}
