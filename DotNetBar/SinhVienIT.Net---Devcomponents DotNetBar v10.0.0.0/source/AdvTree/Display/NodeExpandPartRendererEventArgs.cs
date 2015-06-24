using System;
using System.Drawing;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents event arguments for RenderExpandPart event.
	/// </summary>
	public class NodeExpandPartRendererEventArgs:EventArgs
	{
		/// <summary>
		/// Gets or sets reference to Graphics object, canvas node is rendered on.
		/// </summary>
		public System.Drawing.Graphics Graphics=null;
		/// <summary>
		/// Gets or sets the reference to Node object being rendered.
		/// </summary>
		public DevComponents.AdvTree.Node Node=null;
		/// <summary>Expand part bounds</summary>
		public Rectangle ExpandPartBounds=Rectangle.Empty;
		/// <summary>Expand part border color</summary>
		public Color BorderColor=Color.Empty;
		/// <summary>Expand part line color</summary>
		public Color ExpandLineColor=Color.Empty;
		/// <summary>Expand part background color</summary>
		public Color BackColor=Color.Empty;
		/// <summary>Expand part target gradient background color</summary>
		public Color BackColor2=Color.Empty;
		/// <summary>Gradient angle</summary>
		public int BackColorGradientAngle=90;
		/// <summary>Expand part image when node is expanded</summary>
		public Image ExpandImage=null;
		/// <summary>Expand part image when node is collapsed</summary>
		public Image ExpandImageCollapse=null;
		/// <summary>Internal support for expand button types</summary>
		internal eExpandButtonType ExpandButtonType=eExpandButtonType.Ellipse;
        /// <summary>Gets whether mouse is over expand part</summary>
        public bool IsMouseOver = false;

		/// <summary>
		/// Creates new instance of the class and initializes it with default values.
		/// </summary>
		/// <param name="g">Reference to graphics object.</param>
		public NodeExpandPartRendererEventArgs(Graphics g)
		{
			this.Graphics = g;
		}
	}
}
