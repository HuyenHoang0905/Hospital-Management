using System;
using System.Drawing;
using System.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines the event arguments class for ColorPickerDropDown ColorPreview event.
    /// </summary>
    public class ColorPreviewEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the color that is being previewed.
        /// </summary>
        public Color Color = Color.Empty;

        /// <summary>
        /// Gets the ColorItem if available for the color being previewed. This property can be null if there is no ColorItem connected with the color.
        /// </summary>
        public ColorItem ColorItem = null;

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="c">Color being previewed.</param>
        /// <param name="ci">ColorItem connected with the color.</param>
        public ColorPreviewEventArgs(Color c, ColorItem ci)
        {
            this.Color = c;
            this.ColorItem = ci;
        }
    }

    /// <summary>
    /// Defines delegate for ColorPreview event.
    /// </summary>
    public delegate void ColorPreviewEventHandler(object sender, ColorPreviewEventArgs e);
}
