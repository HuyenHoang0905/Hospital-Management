using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides event arguments for command button events.
	/// </summary>
	public class CommandButtonEventArgs:EventArgs
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="action">Action type.</param>
		/// <param name="node">Context node.</param>
		public CommandButtonEventArgs(eTreeAction action, Node node)
		{
			this.Action=action;
			this.Node=node;
		}
		
		/// <summary>
		/// Indicates the action type that caused the event.
		/// </summary>
		public eTreeAction Action=eTreeAction.Code;
		/// <summary>
		/// Indicates the node action is peformed on.
		/// </summary>
		public DevComponents.AdvTree.Node Node=null;
	}
}
