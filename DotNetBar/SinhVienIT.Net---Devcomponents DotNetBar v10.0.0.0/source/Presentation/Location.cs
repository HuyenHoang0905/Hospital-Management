using System;
using System.Text;

namespace DevComponents.DotNetBar.Presentation
{
    /// <summary>
    /// Describes the shape location.
    /// </summary>
    internal class Location
    {
        public Location() { }
        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Location(int x, int y, eRelativeLocation relativeX, eRelativeLocation relativeY)
        {
            this.X = x;
            this.Y = y;
            this.RelativeX = relativeX;
            this.RelativeY = relativeY;
        }

        /// <summary>
        /// Gets or sets the X location of the shape relative to it's parent.
        /// </summary>
        public int X = 0;
        /// <summary>
        /// Gets or sets the Y location of the shape relative to it's parent.
        /// </summary>
        public int Y = 0;
        /// <summary>
        /// Gets or sets the relative X position.
        /// </summary>
        public eRelativeLocation RelativeX = eRelativeLocation.NotSet;
        /// <summary>
        /// Gets or sets the relative Y position.
        /// </summary>
        public eRelativeLocation RelativeY = eRelativeLocation.NotSet;
    }

    /// <summary>
    /// Describes the relative location.
    /// </summary>
    internal enum eRelativeLocation
    {
        NotSet,
        Top,
        Left,
        Right,
        Bottom
    }
}
