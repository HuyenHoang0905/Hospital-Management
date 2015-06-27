using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for ComboItemsEditor.
    /// </summary>
    public class GalleryGroupCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public GalleryGroupCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(GalleryGroup);
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is GalleryGroup)
            {
                GalleryGroup group = item as GalleryGroup;
                group.Text = group.Name;
            }
            return item;
        }
    }
}
