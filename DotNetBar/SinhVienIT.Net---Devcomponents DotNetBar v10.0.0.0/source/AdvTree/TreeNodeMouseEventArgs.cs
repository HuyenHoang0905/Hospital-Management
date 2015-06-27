using System;
using System.Windows.Forms;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents event arguments for node mouse based events.
	/// </summary>
	public class TreeNodeMouseEventArgs:EventArgs
	{
		public TreeNodeMouseEventArgs(Node node, MouseButtons button, int clicks, int delta, int x, int y)
		{
			this.Node = node;
			this.Button = button;
			this.Clicks = clicks;
			this.Delta = delta;
			this.X = x;
			this.Y = y;
		}
		
		/// <summary>
		/// Gets node affected by mouse action.
		/// </summary>
		public readonly Node Node;
		
		/// <summary>
		/// Gets which mouse button was pressed.
		/// </summary>
		public readonly MouseButtons Button;
		
		/// <summary>
		/// Gets the number of times the mouse button was pressed and released.
		/// </summary>
		public readonly int Clicks;
		
		/// <summary>
		/// Gets a signed count of the number of detents the mouse wheel has rotated. A detent is one notch of the mouse wheel.
		/// </summary>
		public readonly int Delta;
		
		/// <summary>
		/// Gets the x-coordinate of the mouse.
		/// </summary>
		public readonly int X;
		
		/// <summary>
		/// Gets the y-coordinate of the mouse.
		/// </summary>
		public readonly int Y;
	}
}
