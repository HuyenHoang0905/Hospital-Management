using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the color table for the tab item.
    /// </summary>
    public class Office2007TabItemStateColorTable
    {
        /// <summary>
        /// Gets or sets top part background colors.
        /// </summary>
        public LinearGradientColorTable TopBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the bottom part background colors.
        /// </summary>
        public LinearGradientColorTable BottomBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the outer border colors.
        /// </summary>
        public Color OuterBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the inner border colors.
        /// </summary>
        public Color InnerBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the text colors.
        /// </summary>
        public Color Text = Color.Empty;
    }
}
