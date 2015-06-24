using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides data for AdvTree Node events that can be cancelled.
	/// </summary>
	public class AdvTreeNodeCancelEventArgs:AdvTreeNodeEventArgs
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="action">Default action</param>
		/// <param name="node">Default node.</param>
		public AdvTreeNodeCancelEventArgs(eTreeAction action, Node node):base(action,node)
		{
		}
		
		/// <summary>
		/// Indicates that event action should be canceled.
		/// </summary>
		public bool Cancel=false;
	}

    /// <summary>
    /// Provides data for AdvTree Node events that can be cancelled.
    /// </summary>
    public class AdvTreeMultiNodeCancelEventArgs : AdvTreeNodeCancelEventArgs
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="action">Default action</param>
        /// <param name="node">Default node.</param>
        public AdvTreeMultiNodeCancelEventArgs(eTreeAction action, Node[] nodes)
            : base(action, nodes[0])
        {
            Nodes = nodes;
        }

        /// <summary>
        /// Indicates the array of nodes that action is performed on.
        /// </summary>
        public DevComponents.AdvTree.Node[] Nodes = null;
    }
}
