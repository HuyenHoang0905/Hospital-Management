using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides data for AdvTree Node events.
	/// </summary>
	public class AdvTreeNodeEventArgs:EventArgs
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="action">Default action</param>
		/// <param name="node">Default node.</param>
		public AdvTreeNodeEventArgs(eTreeAction action, Node node)
		{
			this.Action = action;
			this.Node = node;
		}
		
		/// <summary>
		/// Indicates the type of the action performed on a node.
		/// </summary>
		public eTreeAction Action=eTreeAction.Code;
		
		/// <summary>
		/// Indicates the node that action is performed on.
		/// </summary>
		public DevComponents.AdvTree.Node Node=null;
	}
}
