using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Specifies the notification interface that node uses to communicate status changes to it's parent tree.
	/// </summary>
	public interface INodeNotify
	{
		/// <summary>Called when Node.Expanded property has changed.</summary>
		/// <param name="node">Node which Expanded property has changed.</param>
		void ExpandedChanged(Node node);
		/// <summary>Called before node is collapsed</summary>
		/// <param name="e">Context information.</param>
		void OnBeforeCollapse(AdvTreeNodeCancelEventArgs e);
		/// <summary>Called before node is expanded</summary>
		/// <param name="e">Context information.</param>
		void OnBeforeExpand(AdvTreeNodeCancelEventArgs e);
		/// <summary>Called after node is collapsed.</summary>
		/// <param name="e">Context information.</param>
		void OnAfterCollapse(AdvTreeNodeEventArgs e);
		/// <summary>Called after node is expanded</summary>
		/// <param name="e">Context information</param>
		void OnAfterExpand(AdvTreeNodeEventArgs e);
	}
}
