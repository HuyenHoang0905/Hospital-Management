using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for ComboItemsEditor.
    /// </summary>
    public class ExplorerBarPanelsEditor : System.ComponentModel.Design.CollectionEditor
    {
        public ExplorerBarPanelsEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(ExplorerBarGroupItem);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(ExplorerBarGroupItem) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is ExplorerBarGroupItem)
            {
                ExplorerBarGroupItem panel = item as ExplorerBarGroupItem;
                panel.Text = "New Group";
            }
            return item;
        }
    }
}
