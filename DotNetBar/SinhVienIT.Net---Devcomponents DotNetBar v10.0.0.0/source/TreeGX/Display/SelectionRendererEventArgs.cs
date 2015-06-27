using System;
using System.Drawing;

namespace DevComponents.Tree
{
	/// <summary>
	/// Data form RenderSelection event.
	/// </summary>
	public class SelectionRendererEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets reference to Graphics object, canvas node is rendered on.
		/// </summary>
		public System.Drawing.Graphics Graphics=null;
		/// <summary>
		/// Gets or sets the reference to selected Node object.
		/// </summary>
		public DevComponents.Tree.Node Node=null;
		/// <summary>
		/// Gets or sets the selection bounds.
		/// </summary>
		public Rectangle Bounds=Rectangle.Empty;
		/// <summary>
		/// Gets or sets border color of the selection rectangle.
		/// </summary>
		public Color BorderColor=Color.Empty;
		/// <summary>
		/// Gets or sets fill color of the selection rectangle.
		/// </summary>
		public Color FillColor=Color.Empty;
		/// <summary>
		/// Gets or sets the width in pixels of the border of the selection rectangle.
		/// </summary>
		public int Width=4;
	}
}
