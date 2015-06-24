using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Support for BubbleBarTab tabs design-time editor.
    /// </summary>
    public class BubbleBarTabCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public BubbleBarTabCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(BubbleBarTab);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(BubbleBarTab) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is BubbleBarTab)
            {
                BubbleBarTab tabItem = item as BubbleBarTab;
                tabItem.Text = "My Tab";
                tabItem.PredefinedColor = eTabItemColor.Blue;
            }
            return item;
        }
    }
}
