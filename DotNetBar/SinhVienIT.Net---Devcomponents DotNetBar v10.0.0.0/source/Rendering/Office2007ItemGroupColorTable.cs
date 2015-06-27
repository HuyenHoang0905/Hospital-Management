using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for ItemContainer object with BeginGroup set to true.
    /// </summary>
    public class Office2007ItemGroupColorTable
    {
        /// <summary>
        /// Gets or sets the outer border colors.
        /// </summary>
        public LinearGradientColorTable OuterBorder = new LinearGradientColorTable(ColorScheme.GetColor("99B6E0"), ColorScheme.GetColor("7394BD"));
        /// <summary>
        /// Gets or sets the inner border colors.
        /// </summary>
        public LinearGradientColorTable InnerBorder = new LinearGradientColorTable(ColorScheme.GetColor("D5E3F1"), ColorScheme.GetColor("E3EDFB"));

        /// <summary>
        /// Gets or sets the top background colors.
        /// </summary>
        public LinearGradientColorTable TopBackground = new LinearGradientColorTable(ColorScheme.GetColor("C8DBEE"), ColorScheme.GetColor("C9DDF6"));
        /// <summary>
        /// Gets or sets the bottom background colors.
        /// </summary>
        public LinearGradientColorTable BottomBackground = new LinearGradientColorTable(ColorScheme.GetColor("BCD0E9"), ColorScheme.GetColor("D0E1F7"));

        /// <summary>
        /// Gets or sets the dark color of item devider for items inside of the ItemContainer.
        /// </summary>
        public Color ItemGroupDividerDark = Color.FromArgb(196, ColorScheme.GetColor("B8C8DC"));

        /// <summary>
        /// Gets or sets the light color of item devider for items inside of the ItemContainer.
        /// </summary>
        public Color ItemGroupDividerLight = Color.FromArgb(128, ColorScheme.GetColor("FFFFFF"));

    }
}
