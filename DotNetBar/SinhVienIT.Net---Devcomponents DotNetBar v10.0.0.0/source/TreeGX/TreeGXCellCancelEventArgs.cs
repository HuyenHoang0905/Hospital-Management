using System;

namespace DevComponents.Tree
{
	/// <summary>
	/// rovides data for TreeGX Cell events that can be canceled.
	/// </summary>
	public class TreeGXCellCancelEventArgs:TreeGXCellEventArgs
	{
		/// <summary>
		/// Default constructor for event data.
		/// </summary>
		/// <param name="action">Type of the action event is raised for.</param>
		/// <param name="cell">Cell that event is raised for.</param>
		public TreeGXCellCancelEventArgs(eTreeAction action, Cell cell):base(action,cell)
		{
		}
		
		/// <summary>
		/// Indicates that event action should be canceled.
		/// </summary>
		public bool Cancel=false;
	}
}
