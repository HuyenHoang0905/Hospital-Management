using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for ComboItemsEditor.
    /// </summary>
    public class SideBarPanelsEditor : System.ComponentModel.Design.CollectionEditor
    {
        public SideBarPanelsEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(SideBarPanelItem);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(SideBarPanelItem) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is SideBarPanelItem)
            {
                SideBarPanelItem panel = item as SideBarPanelItem;
                panel.Text = "New Panel";
            }
            return item;
        }
    }
}
