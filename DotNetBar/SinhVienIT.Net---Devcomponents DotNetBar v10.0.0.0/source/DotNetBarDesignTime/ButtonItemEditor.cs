using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for ComboItemsEditor.
    /// </summary>
    public class ButtonItemEditor : System.ComponentModel.Design.CollectionEditor
    {
        public ButtonItemEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(ButtonItem);
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is ButtonItem)
            {
                ButtonItem button = item as ButtonItem;
                button.Text = "New Item";
                button.ImagePosition = eImagePosition.Left;
            }
            else if (item is LabelItem)
            {
                LabelItem label = item as LabelItem;
                label.Text = "New Label";
            }
            return item;
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(ButtonItem), typeof(LabelItem) };
        }
    }
}
