using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    public class SideBarRendererEventArgs : EventArgs
    {
        public readonly SideBar SideBar = null;

        /// <summary>
        /// Gets or sets the reference to graphics object.
        /// </summary>
        public Graphics Graphics = null;

        internal ItemPaintArgs ItemPaintArgs = null;

        /// <summary>
        /// Initializes a new instance of the SideBarRendererEventArgs class.
        /// </summary>
        /// <param name="sideBar"></param>
        /// <param name="graphics"></param>
        public SideBarRendererEventArgs(SideBar sideBar, Graphics graphics)
        {
            SideBar = sideBar;
            Graphics = graphics;
        }
    }
}
