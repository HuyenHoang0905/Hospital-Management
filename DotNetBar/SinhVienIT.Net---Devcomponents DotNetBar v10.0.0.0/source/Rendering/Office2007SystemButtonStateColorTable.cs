using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for single state of Office 2007 style system button displayed in form caption.
    /// </summary>
    public class Office2007SystemButtonStateColorTable
    {
        /// <summary>
        /// Gets or sets the outer border colors.
        /// </summary>
        public LinearGradientColorTable OuterBorder = new LinearGradientColorTable();
        /// <summary>
        /// Gets or sets the inner border colors.
        /// </summary>
        public LinearGradientColorTable InnerBorder = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets top part background colors.
        /// </summary>
        public LinearGradientColorTable TopBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets bottom part background colors.
        /// </summary>
        public LinearGradientColorTable BottomBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets highlight colors for the top background part.
        /// </summary>
        public LinearGradientColorTable TopHighlight = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets highlight colors for the bottom background part.
        /// </summary>
        public LinearGradientColorTable BottomHighlight = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the foreground color for the button.
        /// </summary>
        public LinearGradientColorTable Foreground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the dark shading color for the foreground.
        /// </summary>
        public Color DarkShade = Color.Empty;

        /// <summary>
        /// Gets or sets the light shading color for the foreground.
        /// </summary>
        public Color LightShade = Color.Empty;
    }
}
