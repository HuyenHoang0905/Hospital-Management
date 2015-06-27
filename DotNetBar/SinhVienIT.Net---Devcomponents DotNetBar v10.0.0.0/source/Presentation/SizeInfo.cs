using System;
using System.Text;

namespace DevComponents.DotNetBar.Presentation
{
    /// <summary>
    /// Describes shape size.
    /// </summary>
    internal class SizeInfo
    {
        /// <summary>
        /// Gets or sets the width of the shape. When RelativeWidth is specified then number specifed here is added to the actual shape width.
        /// </summary>
        public int Width = 0;
        /// <summary>
        /// Gets or sets the height of the shape. When RelativeHeight is specified the number specified here is added to the actual shape height.
        /// </summary>
        public int Height = 0;
        /// <summary>
        /// Gets or sets the relative shape width.
        /// </summary>
        public eRelativeSize RelativeWidth = eRelativeSize.NotSet;
        /// <summary>
        /// Gets or sets the relative shape height.
        /// </summary>
        public eRelativeSize RelativeHeight = eRelativeSize.NotSet;
    }

    internal enum eRelativeSize
    {
        NotSet,
        Width,
        Height
    }
}
