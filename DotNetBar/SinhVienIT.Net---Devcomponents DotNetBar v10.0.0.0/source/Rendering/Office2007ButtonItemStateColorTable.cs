using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents a color table for ButtonItem in certain state like but not limited to mouse over, checked or pressed.
    /// </summary>
    public class Office2007ButtonItemStateColorTable
    {
        #region Internal Implementation
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
        /// Gets or sets highlight colors for the top background part.
        /// </summary>
        public LinearGradientColorTable TopBackgroundHighlight = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets bottom part background colors.
        /// </summary>
        public LinearGradientColorTable BottomBackground = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets highlight colors for the bottom background part.
        /// </summary>
        public LinearGradientColorTable BottomBackgroundHighlight = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the split border colors that divides button text and image from expand part of the button.
        /// </summary>
        public LinearGradientColorTable SplitBorder = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the split border light colors that divides button text and image from expand part of the button.
        /// </summary>
        public LinearGradientColorTable SplitBorderLight = new LinearGradientColorTable();

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color Text = Color.Empty;

        /// <summary>
        /// Gets or sets the background color of the expand sign.
        /// </summary>
        public Color ExpandBackground = Color.Empty;
        /// <summary>
        /// Gets or sets the outline light color of the expand sign.
        /// </summary>
        public Color ExpandLight = Color.Empty;
        /// <summary>
        /// Gets or sets the single gradient background for the button. When specified it is used instead of TopBackground and BottomBackground for rendering.
        /// </summary>
        public LinearGradientColorTable Background = null;
        #endregion
    }
}
