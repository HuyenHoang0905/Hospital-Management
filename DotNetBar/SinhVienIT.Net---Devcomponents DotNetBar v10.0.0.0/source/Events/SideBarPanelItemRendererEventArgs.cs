using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    public class SideBarPanelItemRendererEventArgs : EventArgs
    {
        public readonly SideBarPanelItem SideBarPanelItem = null;
        /// <summary>
        /// Gets or sets the reference to graphics object.
        /// </summary>
        public Graphics Graphics = null;

        internal ItemPaintArgs ItemPaintArgs = null;

        /// <summary>
        /// Initializes a new instance of the SideBarPanelItemRendererEventArgs class.
        /// </summary>
        /// <param name="sideBarPanelItem"></param>
        /// <param name="graphics"></param>
        public SideBarPanelItemRendererEventArgs(SideBarPanelItem sideBarPanelItem, Graphics graphics)
        {
            SideBarPanelItem = sideBarPanelItem;
            Graphics = graphics;
        }
    }
}
