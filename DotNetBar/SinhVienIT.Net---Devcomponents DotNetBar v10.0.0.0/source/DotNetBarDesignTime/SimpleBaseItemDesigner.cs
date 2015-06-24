using System;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design
{
    public class SimpleBaseItemDesigner : BaseItemDesigner
    {
        public override DesignerVerbCollection Verbs
        {
            get
            {
                return new DesignerVerbCollection();
            }
        }
    }

    public class ComboBoxItemDesigner : SimpleBaseItemDesigner
    {
        protected override void SetDesignTimeDefaults()
        {
            ComboBoxItem item = this.Component as ComboBoxItem;
            if (item != null) item.DisplayMember = "Text";
            base.SetDesignTimeDefaults();
        }

        public override System.Collections.ICollection AssociatedComponents
        {
            get
            {
                ArrayList items = new ArrayList(base.AssociatedComponents);
                ComboBoxItem combo = this.Component as ComboBoxItem;
                if (combo != null)
                {
                    foreach (object item in combo.Items)
                    {
                        if (item is ComboItem)
                            items.Add(item);
                    }
                }
                return items;
            }
        }
    }
}
