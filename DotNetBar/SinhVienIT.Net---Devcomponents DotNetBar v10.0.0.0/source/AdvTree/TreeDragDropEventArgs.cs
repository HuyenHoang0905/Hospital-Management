using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents event arguments for BeforeNodeDrop and AfterNodeDrop events
	/// </summary>
    public class TreeDragDropEventArgs : AdvTreeMultiNodeCancelEventArgs
	{
        public TreeDragDropEventArgs(eTreeAction action, Node[] nodes, Node oldParentNode, Node newParentNode, int insertPosition)
            : base(action, nodes)
		{
			this.NewParentNode = newParentNode;
			this.OldParentNode = oldParentNode;
            this.InsertPosition = insertPosition;
		}

        public TreeDragDropEventArgs(eTreeAction action, Node[] nodes, Node oldParentNode, Node newParentNode, bool isCopy, int insertPosition)
            : base(action, nodes)
        {
            this.NewParentNode = newParentNode;
            this.OldParentNode = oldParentNode;
            this.IsCopy = isCopy;
            this.InsertPosition = insertPosition;
        }
		
		/// <summary>
		/// Returns reference to the old parent node.
		/// </summary>
		public readonly Node OldParentNode=null;
		
		/// <summary>
		/// Reference to the new parent node if event is not cancelled.
		/// </summary>
		public readonly Node NewParentNode=null;

        /// <summary>
        /// Gets or sets whether drag node is being copied instead of moved.
        /// </summary>
        public bool IsCopy = false;

        /// <summary>
        /// Gets or sets the new insert position inside of NewParentNode.Nodes collection for the node being dragged. If InsertPosition is -1 
        /// the ParentNode refers to the current mouse over node and drag &amp; drop node will be added as child node to it.
        /// </summary>
        public int InsertPosition = 0;
	}
}
