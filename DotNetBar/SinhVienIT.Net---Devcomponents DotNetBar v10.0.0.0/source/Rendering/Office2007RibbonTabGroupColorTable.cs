using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for the Ribbon Tab Group.
    /// </summary>
    public class Office2007RibbonTabGroupColorTable
    {
        /// <summary>
        /// Gets or sets the name of the color table.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public LinearGradientColorTable Background = new LinearGradientColorTable(ColorScheme.GetColor(0xCADBF3), ColorScheme.GetColor(0xCDBFD7));

        /// <summary>
        /// Gets or sets the background highlight colors.
        /// </summary>
        public LinearGradientColorTable BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, ColorScheme.GetColor(0xD068C9)), Color.Transparent);

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color Text = ColorScheme.GetColor(0x15428B);

        /// <summary>
        /// Gets or sets the border color
        /// </summary>
        public LinearGradientColorTable Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
    }
}
