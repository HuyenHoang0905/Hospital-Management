using System;
using System.Text;
using DevComponents.DotNetBar.Rendering;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    public class RibbonClientPanelDesigner : PanelControlDesigner
    {
        protected override void SetDesignTimeDefaults()
        {
            PanelControl p = this.Control as PanelControl;
            if (p == null)
                return;
            p.CanvasColor = SystemColors.Control;
            p.Style.Class = ElementStyleClassKeys.RibbonClientPanelKey;
        }
    }
}
