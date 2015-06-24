using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Support for TabStrip tabs design-time editor.
    /// </summary>
    public class TabStripTabsEditor : CollectionEditor
    {
        public TabStripTabsEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(TabItem);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(TabItem) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is TabItem)
            {
                TabItem tabItem = item as TabItem;
                tabItem.Text = "My Tab";
            }
            return item;
        }
    }
}
