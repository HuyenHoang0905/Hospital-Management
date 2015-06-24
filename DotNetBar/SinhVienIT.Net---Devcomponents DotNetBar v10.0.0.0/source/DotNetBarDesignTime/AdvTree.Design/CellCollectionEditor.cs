using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.AdvTree.Design
{
    /// <summary>
    /// Support for Cell tabs design-time editor.
    /// </summary>
    public class CellCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        /// <summary>Creates new instance of cell collection editor.</summary>
        /// <param name="type">Type to initialize editor with.</param>
        public CellCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(Cell);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(Cell) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is Cell)
            {
                Cell cell = item as Cell;
                cell.Text = cell.Name;
            }
            return item;
        }
    }
}
