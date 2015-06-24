using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides data for AdvTree Cell events.
	/// </summary>
	public class AdvTreeCellEventArgs:EventArgs
	{
		/// <summary>
		/// Default constructor for event data.
		/// </summary>
		/// <param name="action">Type of the action event is raised for.</param>
		/// <param name="cell">Cell that event is raised for.</param>
		public AdvTreeCellEventArgs(eTreeAction action, Cell cell)
		{
			this.Action=action;
			this.Cell=cell;
		}

		/// <summary>
		/// Indicates the type of the action performed on a cell.
		/// </summary>
		public eTreeAction Action=eTreeAction.Code;
		/// <summary>
		/// Indicates the cell that action is performed on.
		/// </summary>
		public DevComponents.AdvTree.Cell Cell=null;
	}
}
