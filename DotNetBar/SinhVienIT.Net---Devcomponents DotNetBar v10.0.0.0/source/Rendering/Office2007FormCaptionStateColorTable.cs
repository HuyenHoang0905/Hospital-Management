using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines color table for the Office 2007 style Form caption color state.
    /// </summary>
    public class Office2007FormStateColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the top part of the background.
        /// </summary>
        public LinearGradientColorTable CaptionTopBackground = null;

        /// <summary>
        /// Gets or sets the colors for the bottom part of the background.
        /// </summary>
        public LinearGradientColorTable CaptionBottomBackground = null;

        /// <summary>
        /// Gets or sets the array of colors used to draw the border that separates the form caption and the form content. Applies only to the Office2007Form rendering.
        /// </summary>
        public Color[] CaptionBottomBorder = null;

        /// <summary>
        /// Gets or sets the color of caption text.
        /// </summary>
        public Color CaptionText = Color.Empty;

        /// <summary>
        /// Gets or sets the color of caption extra text that is appended to the caption.
        /// </summary>
        public Color CaptionTextExtra = Color.Empty;

        /// <summary>
        /// Gets or sets the array of colors that represents the border colors. Outer border is at index 0.
        /// </summary>
        public Color[] BorderColors = null; // OuterBorder = Color.Empty;
    }
}
