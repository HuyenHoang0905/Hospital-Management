using System;
using System.Drawing;
using System.Text;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents color table for Bar objects in various states.
    /// </summary>
    public class Office2007BarColorTable
    {
        /// <summary>
        /// Gets or sets the colors for top part of toolbar background.
        /// </summary>
        public LinearGradientColorTable ToolbarTopBackground = new LinearGradientColorTable(ColorScheme.GetColor("D7E6F9"), ColorScheme.GetColor("C7DCF8"));

        /// <summary>
        /// Gets or sets the colors for bottom part of toolbar background.
        /// </summary>
        public LinearGradientColorTable ToolbarBottomBackground = new LinearGradientColorTable(ColorScheme.GetColor("B3D0F5"), ColorScheme.GetColor("D7E5F7"));

        /// <summary>
        /// Gets or sets the color of the bottom border.
        /// </summary>
        public Color ToolbarBottomBorder = ColorScheme.GetColor("BAD4F7");

        /// <summary>
        /// Gets or sets the popup toolbar background color.
        /// </summary>
        public LinearGradientColorTable PopupToolbarBackground = new LinearGradientColorTable(ColorScheme.GetColor("FAFAFA"), Color.Empty);

        /// <summary>
        /// Gets or sets the color of popup toolbar border.
        /// </summary>
        public Color PopupToolbarBorder = ColorScheme.GetColor("868686");

        /// <summary>
        /// Gets or sets the status bar top border color.
        /// </summary>
        public Color StatusBarTopBorder = Color.Empty;
        /// <summary>
        /// Gets or sets the status bar top border light color.
        /// </summary>
        public Color StatusBarTopBorderLight = Color.Empty;
        /// <summary>
        /// Gets or sets the alternative background colors for the status bar.
        /// </summary>
        public BackgroundColorBlendCollection StatusBarAltBackground = new BackgroundColorBlendCollection();
    }
}
