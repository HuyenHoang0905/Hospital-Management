using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents data for key tips rendering.
    /// </summary>
    public class KeyTipsRendererEventArgs:EventArgs
    {
        /// <summary>
        /// Gets or sets the graphics object used for rendering.
        /// </summary>
        public Graphics Graphics=null;
        /// <summary>
        /// Gets or sets key tip bounds.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;
        /// <summary>
        /// Gets or sets the text of key tip to be rendered.
        /// </summary>
        public string KeyTip = "";
        /// <summary>
        /// Gets or sets the font key tip should be rendered with.
        /// </summary>
        public Font Font = null;
        /// <summary>
        /// Reference object for which Key Tip is rendered. For example this could be reference to an instance of ButtonItem or BaseItem as well
        /// as reference to System.Windows.Forms.Control object. Always test for type before accessing this reference.
        /// </summary>
        public object ReferenceObject = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values.
        /// </summary>
        public KeyTipsRendererEventArgs(Graphics g, Rectangle bounds, string keyTip, Font font, object referenceObject)
        {
            this.Graphics = g;
            this.Bounds = bounds;
            this.KeyTip = keyTip;
            this.Font = font;
            this.ReferenceObject = referenceObject;
        }
    }
}
