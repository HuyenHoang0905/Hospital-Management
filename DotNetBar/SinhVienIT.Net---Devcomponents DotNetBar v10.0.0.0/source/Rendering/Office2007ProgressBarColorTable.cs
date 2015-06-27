using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines color table for the Office 2007 style ProgressBarItem.
    /// </summary>
    public class Office2007ProgressBarColorTable
    {
        /// <summary>
        /// Gets or sets the background color collection blend for the item background.
        /// </summary>
        public GradientColorTable BackgroundColors = null;

        /// <summary>
        /// Gets or sets the outer border color.
        /// </summary>
        public Color OuterBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the inner border color.
        /// </summary>
        public Color InnerBorder = Color.Empty;

        /// <summary>
        /// Gets or sets the color collection blend for the current progress part of the item.
        /// </summary>
        public GradientColorTable Chunk = null;

        /// <summary>
        /// Gets or sets the color collection blend for overlay for the current progress of the item.
        /// </summary>
        public GradientColorTable ChunkOverlay = null;

        /// <summary>
        /// Gets or sets the color collection blend of shadow for the current progress of the item.
        /// </summary>
        public GradientColorTable ChunkShadow = null;
    }
}
