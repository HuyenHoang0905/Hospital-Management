using System;
using System.Text;

namespace DevComponents.DotNetBar.Presentation
{
    /// <summary>
    /// Describes the padding for the shape. Padding is the space inside the shape and between it's child shapes.
    /// </summary>
    internal class PaddingInfo
    {
        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public PaddingInfo() { }

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        public PaddingInfo(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        /// <summary>
        /// Gets or sets the left padding in pixels.
        /// </summary>
        public int Left = 0;
        /// <summary>
        /// Gets or sets the right padding in pixels.
        /// </summary>
        public int Right = 0;
        /// <summary>
        /// Gets or sets the top padding in pixels.
        /// </summary>
        public int Top = 0;
        /// <summary>
        /// Gets or sets the bottom padding in pixels.
        /// </summary>
        public int Bottom = 0;

        /// <summary>
        /// Gets the total horizontal padding.
        /// </summary>
        public int HorizontalPadding
        {
            get { return Left + Right; }
        }

        /// <summary>
        /// Gets the total vertical padding.
        /// </summary>
        public int VerticalPadding
        {
            get { return Top + Bottom; }
        }
    }
}
