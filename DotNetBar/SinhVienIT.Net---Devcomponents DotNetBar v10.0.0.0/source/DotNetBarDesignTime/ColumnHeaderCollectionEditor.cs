using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar.Design
{
    #region ColumnHeaderCollectionEditor
    /// <summary>
    /// Support for ColumnHeader tabs design-time editor.
    /// </summary>
    public class ColumnHeaderCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        /// <summary>Creates new instance of the class</summary>
        /// <param name="type">Type to initialize editor with.</param>
        public ColumnHeaderCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(ColumnHeader);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(ColumnHeader) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is ColumnHeader)
            {
                ColumnHeader ch = item as ColumnHeader;
                ch.Text = ch.Name;
            }
            return item;
        }
    }
    #endregion
}
