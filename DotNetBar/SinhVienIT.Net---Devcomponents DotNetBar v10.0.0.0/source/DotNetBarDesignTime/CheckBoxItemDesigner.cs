using System;
using System.Text;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    public class CheckBoxItemDesigner : BaseItemDesigner
    {
        public override DesignerVerbCollection Verbs
        {
            get
            {
                return new DesignerVerbCollection();
            }
        }

        protected override void SetDesignTimeDefaults()
        {
            CheckBoxItem c = this.Component as CheckBoxItem;
            if (c != null) c.AutoCollapseOnClick = false;
        }
    }
}
