using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the color table for the Scroll bar in single state.
    /// </summary>
    public class Office2007ScrollBarStateColorTable
    {
        /// <summary>
        /// Gets or sets the outer border color for the scroll bar thumb.
        /// </summary>
        public LinearGradientColorTable ThumbOuterBorder = null;

        /// <summary>
        /// Gets or sets the inner border color for the scroll bar thumb.
        /// </summary>
        public LinearGradientColorTable ThumbInnerBorder = null;
        
        /// <summary>
        /// Gets or sets the thumb background color blend collection.
        /// </summary>
        public BackgroundColorBlendCollection ThumbBackground = new BackgroundColorBlendCollection();

        /// <summary>
        /// Gets or sets the directional sign background color for the scroll bar thumb.
        /// </summary>
        public LinearGradientColorTable ThumbSignBackground = null;

        /// <summary>
        /// Gets or sets the outer border color for the scroll bar track button.
        /// </summary>
        public LinearGradientColorTable TrackOuterBorder = null;

        /// <summary>
        /// Gets or sets the inner border color for the scroll bar track button.
        /// </summary>
        public LinearGradientColorTable TrackInnerBorder = null;

        /// <summary>
        /// Gets or sets the track background color blend collection.
        /// </summary>
        public BackgroundColorBlendCollection TrackBackground = new BackgroundColorBlendCollection();

        /// <summary>
        /// Gets or sets the background color for the track signs.
        /// </summary>
        public LinearGradientColorTable TrackSignBackground = null;

        /// <summary>
        /// Gets or sets the background colors for the entire control.
        /// </summary>
        public LinearGradientColorTable Background = null;

        /// <summary>
        /// Gets or sets the border colors for the entire control.
        /// </summary>
        public LinearGradientColorTable Border = null;
    }
}
