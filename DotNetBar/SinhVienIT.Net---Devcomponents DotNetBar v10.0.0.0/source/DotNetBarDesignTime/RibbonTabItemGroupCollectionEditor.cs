using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for RibbonTabItemGroupCollectionEditor.
    /// </summary>
    public class RibbonTabItemGroupCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public RibbonTabItemGroupCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(RibbonTabItemGroup);
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is RibbonTabItemGroup)
            {
                RibbonTabItemGroup group = item as RibbonTabItemGroup;
                DesignerSupport.SetDefaults(group);
            }
            return item;
        }
    }
}
