using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Provides information for cell rendering methods and events.
	/// </summary>
	public class NodeCellRendererEventArgs:NodeRendererEventArgs
	{
		/// <summary>
		/// Gets or sets the cell being rendered.
		/// </summary>
		public Cell Cell=null;
		
		/// <summary>
		/// Gets or sets absolute cell bounds.
		/// </summary>
		public Rectangle CellBounds=Rectangle.Empty;
		
		/// <summary>
		/// Gets or sets the internal cell offset.
		/// </summary>
		internal Point CellOffset=Point.Empty;

        /// <summary>
        /// Gets or sets the color scheme.
        /// </summary>
        internal ColorScheme ColorScheme = null;

        internal Image CheckBoxImageChecked = null;
        internal Image CheckBoxImageUnChecked = null;
        internal Image CheckBoxImageIndeterminate = null;
        internal ItemPaintArgs ItemPaintArgs = null;

		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public NodeCellRendererEventArgs():base(null,null,Rectangle.Empty,null)
		{
		}
		
		/// <summary>
		/// Creates new instance of the class and initializes it with default values.
		/// </summary>
		/// <param name="g">Reference to graphics object.</param>
		/// <param name="node">Reference to context node.</param>
		/// <param name="bounds">Reference to node bounds</param>
		/// <param name="style">Reference to cell style</param>
		/// <param name="cell">Reference to cell</param>
		/// <param name="cellBounds">Reference to cell bounds</param>
		public NodeCellRendererEventArgs(Graphics g, Node node, Rectangle bounds, ElementStyle style, Cell cell, Rectangle cellBounds):base(g,node,bounds,style)
		{
			this.Cell = cell;
			this.CellBounds = cellBounds;
		}
	}
}
