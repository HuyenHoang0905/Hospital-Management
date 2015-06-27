using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for CheckBoxItem rendering events.
    /// </summary>
    public class CheckBoxItemRenderEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;

        /// <summary>
        /// Gets or sets the reference to CheckBoxItem being rendered.
        /// </summary>
        public CheckBoxItem CheckBoxItem = null;

        /// <summary>
        /// ColorScheme object that is used to provide colors for rendering check box item in legacy styles like Office 2003. Office 2007 style
        /// uses color tables provided by renderers.
        /// </summary>
        public ColorScheme ColorScheme = null;

        /// <summary>
        /// Indicates whether item is in Right-To-Left environment.
        /// </summary>
        public bool RightToLeft = false;

        /// <summary>
        /// Gets or sets the text font.
        /// </summary>
        public Font Font = null;

        /// <summary>
        /// Gets or sets the ItemPaintArgs reference.
        /// </summary>
        internal ItemPaintArgs ItemPaintArgs;

        /// <summary>
        /// Creates new instance of the object and provides default values.
        /// </summary>
        /// <param name="g">Reference to Graphics object</param>
        /// <param name="item">Reference to CheckBoxItem</param>
        /// <param name="cs">Reference to legacy ColorScheme</param>
        /// <param name="f">Indicates the font for the text.</param>
        /// <param name="rtl">Indicates whether item is in Right-To-Left environment.</param>
        public CheckBoxItemRenderEventArgs(Graphics g, CheckBoxItem item, ColorScheme cs, Font f, bool rtl)
        {
            this.Graphics = g;
            this.CheckBoxItem = item;
            this.ColorScheme = cs;
            this.RightToLeft = rtl;
            this.Font = f;
        }
    }
}
