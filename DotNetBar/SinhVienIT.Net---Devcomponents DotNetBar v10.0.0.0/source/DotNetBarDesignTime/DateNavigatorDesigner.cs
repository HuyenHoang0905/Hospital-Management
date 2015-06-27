using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    public class DateNavigatorDesigner : DotNetBar.Design.PanelExDesigner
    {
        protected override void SetDesignTimeDefaults()
        {
            PanelEx p = this.Control as PanelEx;
            if (p == null)
                return;
            p.CanvasColor = SystemColors.Control;
        }
    }
}
