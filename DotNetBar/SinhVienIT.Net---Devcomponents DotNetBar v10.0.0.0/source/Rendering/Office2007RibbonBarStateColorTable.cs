using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the color table of RibbonBar for Office 2007 style. Default values represent blue Luna theme.
    /// </summary>
    public class Office2007RibbonBarStateColorTable
    {
        #region Color Definition
        /// <summary>
        /// Gets or sets the height in pixels of top background part.
        /// </summary>
        public float TopBackgroundHeight = 15;
        /// <summary>
        /// Gets or sets the outer border colors.
        /// </summary>
        public LinearGradientColorTable OuterBorder = new LinearGradientColorTable(0xC6D2DF, 0x9FC1DB);
        /// <summary>
        /// Gets or sets the inner border colors.
        /// </summary>
        public LinearGradientColorTable InnerBorder = new LinearGradientColorTable(0xEFF4FA, 0xF3F9FF);

        /// <summary>
        /// Gets or sets the top background colors.
        /// </summary>
        public LinearGradientColorTable TopBackground = new LinearGradientColorTable(0xDEE8F5, 0xD1DFF0);
        /// <summary>
        /// Gets or sets the bottom background colors.
        /// </summary>
        public LinearGradientColorTable BottomBackground = new LinearGradientColorTable(0xC7D8ED, 0xE7F2FF);

        /// <summary>
        /// Gets or sets the title background colors.
        /// </summary>
        public LinearGradientColorTable TitleBackground = new LinearGradientColorTable(0xC2D9F1, 0xC1D9F0);
        /// <summary>
        /// Gets or sets the color of title text.
        /// </summary>
        public Color TitleText = ColorScheme.GetColor(0x3E6AAA);
        #endregion
    }
}
