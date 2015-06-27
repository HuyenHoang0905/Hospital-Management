using System;
using System.Drawing;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the colors for the RibbonTabItem state like but not limited to selected, mouse over etc.
    /// </summary>
    public class Office2007RibbonTabItemStateColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the outer border.
        /// </summary>
        public LinearGradientColorTable OuterBorder = new LinearGradientColorTable();
        /// <summary>
        /// Gets or sets the colors for the inner border.
        /// </summary>
        public LinearGradientColorTable InnerBorder = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the background colors.
        /// </summary>
        public LinearGradientColorTable Background = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the background highlight colors.
        /// </summary>
        public LinearGradientColorTable BackgroundHighlight = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color Text = Color.Empty;
    }
}
