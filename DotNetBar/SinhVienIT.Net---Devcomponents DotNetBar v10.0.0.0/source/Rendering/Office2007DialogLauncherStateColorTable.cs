using System;
using System.Drawing;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for dialog launcher state.
    /// </summary>
    public class Office2007DialogLauncherStateColorTable
    {
        /// <summary>
        /// Gets or sets the color of dialog launcher symbol.
        /// </summary>
        public Color DialogLauncher = ColorScheme.GetColor("668EAF");
        /// <summary>
        /// Gets or sets the shade color of dialog launcher symbol.
        /// </summary>
        public Color DialogLauncherShade = Color.White;

        /// <summary>
        /// Gets or sets the background color for the top part of the element.
        /// </summary>
        public LinearGradientColorTable TopBackground = new LinearGradientColorTable();
        /// <summary>
        /// Gets or sets the background color for the bottom part of the element.
        /// </summary>
        public LinearGradientColorTable BottomBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the outer border colors.
        /// </summary>
        public LinearGradientColorTable OuterBorder = new LinearGradientColorTable();
        /// <summary>
        /// Gets or sets the inner border colors.
        /// </summary>
        public LinearGradientColorTable InnerBorder = new LinearGradientColorTable();
    }
}
