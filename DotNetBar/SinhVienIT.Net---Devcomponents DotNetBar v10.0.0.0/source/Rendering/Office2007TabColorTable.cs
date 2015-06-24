using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the color table for the tab control.
    /// </summary>
    public class Office2007TabColorTable
    {
        /// <summary>
        /// Gets or sets the default tab item colors.
        /// </summary>
        public Office2007TabItemStateColorTable Default = new Office2007TabItemStateColorTable();

        /// <summary>
        /// Gets or sets the mouse over tab item colors.
        /// </summary>
        public Office2007TabItemStateColorTable MouseOver = new Office2007TabItemStateColorTable();

        /// <summary>
        /// Gets or sets the selected tab item colors.
        /// </summary>
        public Office2007TabItemStateColorTable Selected = new Office2007TabItemStateColorTable();

        /// <summary>
        /// Gets or sets the color of the tab background colors.
        /// </summary>
        public LinearGradientColorTable TabBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the tab-strip background image.
        /// </summary>
        public Image TabBackgroundImage = null;

        /// <summary>
        /// Gets or sets the color of the tab panel background colors.
        /// </summary>
        public LinearGradientColorTable TabPanelBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the color of tab panel border.
        /// </summary>
        public Color TabPanelBorder = Color.Empty;
    }
}
