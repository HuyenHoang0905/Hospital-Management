using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.AdvTree.Design
{
    #region NodeCollectionEditor
    /// <summary>
    /// Support for Node tabs design-time editor.
    /// </summary>
    public class NodeCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public NodeCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(Node);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(Node) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is Node)
            {
                Node node = item as Node;
                node.Text = node.Name;
            }
            return item;
        }
    }
    #endregion
}
