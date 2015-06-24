using System;

namespace DevComponents.Tree
{
	/// <summary>
	/// Provides data for TreeGX Cell events.
	/// </summary>
	public class TreeGXCellEventArgs:EventArgs
	{
		/// <summary>
		/// Default constructor for event data.
		/// </summary>
		/// <param name="action">Type of the action event is raised for.</param>
		/// <param name="cell">Cell that event is raised for.</param>
		public TreeGXCellEventArgs(eTreeAction action, Cell cell)
		{
			this.Action=action;
			this.Cell=cell;
		}

		/// <summary>
		/// Indicates the type of the action performed on a cell.
		/// </summary>
		public eTreeAction Action=eTreeAction.Code;
		/// <summary>
		/// Indicates the cell that action is peformed on.
		/// </summary>
		public DevComponents.Tree.Cell Cell=null;
	}
}
