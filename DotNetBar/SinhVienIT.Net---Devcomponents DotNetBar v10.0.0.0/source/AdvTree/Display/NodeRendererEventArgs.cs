using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Summary description for NodeRendererEventArgs.
	/// </summary>
	public class NodeRendererEventArgs:EventArgs
	{
		/// <summary>
		/// Gets or sets reference to Graphics object, canvas node is rendered on.
		/// </summary>
		public System.Drawing.Graphics Graphics=null;
		/// <summary>
		/// Gets or sets the reference to Node object being rendered.
		/// </summary>
		public DevComponents.AdvTree.Node Node=null;
		/// <summary>
		/// Gets or sets the absolute node bounds.
		/// </summary>
		public Rectangle NodeBounds=Rectangle.Empty;
		/// <summary>
		/// Gets or sets the reference to element style for rendered node or cell. Style provided here is the style
		/// for current node or cell state.
		/// </summary>
		public ElementStyle Style=null;
        /// <summary>
        /// Gets or sets color that is passed to renderer. May be Color.Empty.
        /// </summary>
        public Color Color = Color.Empty;
		
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public NodeRendererEventArgs()
		{
		}
		
		public NodeRendererEventArgs(Graphics g, Node node, Rectangle bounds, ElementStyle style)
            : this(g, node, bounds, style, Color.Empty)
		{
		}

        public NodeRendererEventArgs(Graphics g, Node node, Rectangle bounds, ElementStyle style, Color color)
        {
            this.Graphics = g;
            this.Node = node;
            this.NodeBounds = bounds;
            this.Style = style;
            this.Color = color;
        }
	}
}
