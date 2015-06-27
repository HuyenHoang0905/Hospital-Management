using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    internal class SideBarPainter
    {
        public virtual void PaintSideBar(SideBarRendererEventArgs e){}

        public virtual void PaintSideBarPanelItem(SideBarPanelItemRendererEventArgs e) { }
    }
}
