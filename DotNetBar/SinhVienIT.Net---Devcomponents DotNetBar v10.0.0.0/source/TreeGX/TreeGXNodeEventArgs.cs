using System;

namespace DevComponents.Tree
{
	/// <summary>
	/// Provides data for TreeGX Node events.
	/// </summary>
	public class TreeGXNodeEventArgs:EventArgs
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="action">Default action</param>
		/// <param name="node">Default node.</param>
		public TreeGXNodeEventArgs(eTreeAction action, Node node)
		{
			this.Action = action;
			this.Node = node;
		}
		
		/// <summary>
		/// Indicates the type of the action performed on a node.
		/// </summary>
		public eTreeAction Action=eTreeAction.Code;
		
		/// <summary>
		/// Indicates the node that action is peformed on.
		/// </summary>
		public DevComponents.Tree.Node Node=null;
	}
}
