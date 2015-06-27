using System;
using System.Windows.Forms;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides data for AdvTree Cell events that can be canceled.
	/// </summary>
	public class TreeCellCancelEventArgs:AdvTreeCellEventArgs
	{
		/// <summary>
		/// Default constructor for event data.
		/// </summary>
		/// <param name="action">Type of the action event is raised for.</param>
		/// <param name="cell">Cell that event is raised for.</param>
		public TreeCellCancelEventArgs(eTreeAction action, Cell cell):base(action,cell)
		{
		}
		
		/// <summary>
		/// Indicates that event action should be canceled.
		/// </summary>
		public bool Cancel=false;
	}

    /// <summary>
    /// Provides data for AdvTree.BeforeCheck event.
    /// </summary>
    public class AdvTreeCellBeforeCheckEventArgs : TreeCellCancelEventArgs
    {
        public CheckState NewCheckState = CheckState.Indeterminate;

        /// <summary>
        /// Initializes a new instance of the AdvTreeCellBeforeCheckEventArgs class.
        /// </summary>
        /// <param name="newCheckState"></param>
        public AdvTreeCellBeforeCheckEventArgs(eTreeAction action, Cell cell, CheckState newCheckState)
            : base(action, cell)
        {
            NewCheckState = newCheckState;
        }
    }
}
