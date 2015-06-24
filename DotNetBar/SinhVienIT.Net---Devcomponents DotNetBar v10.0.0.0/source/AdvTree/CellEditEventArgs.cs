using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents event arguments for cell editing events.
	/// </summary>
	public class CellEditEventArgs : EventArgs
	{
		/// <summary>
		/// Indicates the action that caused the event.
		/// </summary>
		public eTreeAction Action=eTreeAction.Code;
		/// <summary>
		/// Indicates the cell that is affected.
		/// </summary>
		public DevComponents.AdvTree.Cell Cell=null;
		/// <summary>
		/// Indicates new text that will be assigned to the cell if one is appropriate for given event.
		/// </summary>
		public string NewText="";

		/// <summary>
		/// Indicates whether the current action is cancelled. For BeforeCellEdit event setting this
		/// property to true will cancel the editing. For AfterCellEdit event setting this property to
		/// true will cancel any changes made to the text and edits will not be accepted. For CellEditEnding
		/// event setting this property to true will keep the cell in edit mode.
		/// </summary>
		public bool Cancel=false;

		/// <summary>
		/// Initializes new instance of CellEditEventArgs class.
		/// </summary>
		/// <param name="cell">Reference to Cell this event is raised for.</param>
		/// <param name="action">Indicates the action that caused the event.</param>
		/// <param name="newText">Indicates new text of the cell if it applies to given event.</param>
		public CellEditEventArgs(Cell cell, eTreeAction action, string newText)
		{
			this.Action=action;
			this.Cell=cell;
			this.NewText=newText;
		}
	}
}
