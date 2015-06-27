using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for the Quick Access toolbar.
    /// </summary>
    public class Office2007QuickAccessToolbarStateColorTable
    {
        /// <summary>
        /// Gets or sets the colors of the top background.
        /// </summary>
        public LinearGradientColorTable TopBackground = null;

        /// <summary>
        /// Gets or sets the colors of the bottom background.
        /// </summary>
        public LinearGradientColorTable BottomBackground = null;

        /// <summary>
        /// Gets or sets the outer border color.
        /// </summary>
        public Color OutterBorderColor = Color.Empty;

        /// <summary>
        /// Gets or sets the middle border color.
        /// </summary>
        public Color MiddleBorderColor = Color.Empty;

        /// <summary>
        /// Gets or sets the inner border color.
        /// </summary>
        public Color InnerBorderColor = Color.Empty;

        /// <summary>
        /// Gets or sets the border when Windows Vista Glass is enabled.
        /// </summary>
        public LinearGradientColorTable GlassBorder = null;
    }
}
