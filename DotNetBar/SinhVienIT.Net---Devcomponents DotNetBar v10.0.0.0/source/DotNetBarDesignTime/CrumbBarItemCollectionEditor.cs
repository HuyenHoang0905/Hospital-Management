using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Support for ColumnHeader tabs design-time editor.
    /// </summary>
    public class CrumbBarItemCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        /// <summary>Creates new instance of the class</summary>
        /// <param name="type">Type to initialize editor with.</param>
        public CrumbBarItemCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(CrumbBarItem);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(CrumbBarItem) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is CrumbBarItem)
            {
                CrumbBarItem ch = item as CrumbBarItem;
                ch.Text = ch.Name;
            }
            return item;
        }
    }
}
