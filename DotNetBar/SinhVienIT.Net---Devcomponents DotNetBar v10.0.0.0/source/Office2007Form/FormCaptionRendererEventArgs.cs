using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for form caption rendering events.
    /// </summary>
    public class FormCaptionRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the reference to graphics object.
        /// </summary>
        public Graphics Graphics = null;
        /// <summary>
        /// Gets or sets the caption bounds.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;
        /// <summary>
        /// Gets or sets the form caption is rendered for.
        /// </summary>
        public Form Form = null;

        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public FormCaptionRendererEventArgs(Graphics g, Rectangle bounds, Form form)
        {
            this.Graphics = g;
            this.Bounds = bounds;
            this.Form = form;
        }
    }
}
