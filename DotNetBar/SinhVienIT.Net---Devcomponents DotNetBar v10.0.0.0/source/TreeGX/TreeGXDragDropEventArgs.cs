using System;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents event arguments for BeforeNodeDrop and AfterNodeDrop events
	/// </summary>
	public class TreeGXDragDropEventArgs : TreeGXNodeCancelEventArgs
	{
		public TreeGXDragDropEventArgs(eTreeAction action, Node node, Node oldParentNode, Node newParentNode):base(action,node)
		{
			this.NewParentNode = newParentNode;
			this.OldParentNode = oldParentNode;
		}
		
		/// <summary>
		/// Returns reference to the old parent node.
		/// </summary>
		public readonly Node OldParentNode=null;
		
		/// <summary>
		/// Reference to the new parent node if event is not canceled.
		/// </summary>
		public readonly Node NewParentNode=null;

        /// <summary>
        /// Gets or sets whether drag node is being copied instead of moved.
        /// </summary>
        public bool IsCopy = false;
	}
}
