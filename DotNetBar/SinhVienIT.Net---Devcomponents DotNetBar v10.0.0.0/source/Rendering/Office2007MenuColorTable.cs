using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for the menus.
    /// </summary>
    public class Office2007MenuColorTable
    {
        /// <summary>
        /// Gets or sets the menu background colors.
        /// </summary>
        public LinearGradientColorTable Background = null;

        /// <summary>
        /// Gets or sets the menu side background colors.
        /// </summary>
        public LinearGradientColorTable Side = null;

        /// <summary>
        /// Gets or sets the menu side background colors for the items that were not recently used.
        /// </summary>
        public LinearGradientColorTable SideUnused = null;

        /// <summary>
        /// Gets or sets the menu border background colors.
        /// </summary>
        public LinearGradientColorTable Border = null;

        /// <summary>
        /// Gets or sets the menu side border.
        /// </summary>
        public LinearGradientColorTable SideBorder = null;

        /// <summary>
        /// Gets or sets the light menu side border.
        /// </summary>
        public LinearGradientColorTable SideBorderLight = null;

        /// <summary>
        /// Gets or sets the background color blend for the special file menu background.
        /// </summary>
        public BackgroundColorBlendCollection FileBackgroundBlend = new BackgroundColorBlendCollection();

        /// <summary>
        /// Gets or sets the two column container border color.
        /// </summary>
        public Color FileContainerBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the two column container light border color.
        /// </summary>
        public Color FileContainerBorderLight = Color.Empty;

        /// <summary>
        /// Gets or sets the background color of first file column.
        /// </summary>
        public Color FileColumnOneBackground = Color.Empty;

        /// <summary>
        /// Gets or sets the border color of first file column.
        /// </summary>
        public Color FileColumnOneBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the background color of first file column.
        /// </summary>
        public Color FileColumnTwoBackground = Color.Empty;

        ///// <summary>
        ///// Gets or sets the border color of first file column.
        ///// </summary>
        //public Color FileColumnTwoBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the background color blend for the bottom container on file menu.
        /// </summary>
        public BackgroundColorBlendCollection FileBottomContainerBackgroundBlend = new BackgroundColorBlendCollection();
    }
}
