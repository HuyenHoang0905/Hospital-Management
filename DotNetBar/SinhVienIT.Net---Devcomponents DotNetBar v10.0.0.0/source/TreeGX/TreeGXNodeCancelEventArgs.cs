using System;

namespace DevComponents.Tree
{
	/// <summary>
	/// Provides data for TreeGX Node events that can be cancelled.
	/// </summary>
	public class TreeGXNodeCancelEventArgs:TreeGXNodeEventArgs
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="action">Default action</param>
		/// <param name="node">Default node.</param>
		public TreeGXNodeCancelEventArgs(eTreeAction action, Node node):base(action,node)
		{
		}
		
		/// <summary>
		/// Indicates that event action should be canceled.
		/// </summary>
		public bool Cancel=false;
	}
}
