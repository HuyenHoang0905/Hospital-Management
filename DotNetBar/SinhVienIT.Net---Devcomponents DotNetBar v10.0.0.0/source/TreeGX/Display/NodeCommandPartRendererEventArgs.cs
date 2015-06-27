using System;
using System.Drawing;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Summary description for NodeCommandPartRendererEventArgs.
	/// </summary>
	public class NodeCommandPartRendererEventArgs:EventArgs
	{  
		/// <summary>
		/// Gets or sets reference to Graphics object, canvas node is rendered on.
		/// </summary>
		public System.Drawing.Graphics Graphics=null;
		/// <summary>
		/// Gets or sets the reference to Node object being rendered.
		/// </summary>
		public DevComponents.Tree.Node Node=null;
		/// <summary>
		/// Gets or sets command part absolute bounds.
		/// </summary>
		public Rectangle CommandPartBounds=Rectangle.Empty;
		/// <summary>
		/// Gets or sets command part back color.
		/// </summary>
		public Color BackColor=Color.Empty;
		/// <summary>
		/// Gets or sets command part end gradient color.
		/// </summary>
		public Color BackColor2=Color.Empty;
		/// <summary>
		/// Gets or sets command part text color.
		/// </summary>
		public Color ForeColor=Color.Empty;
		/// <summary>
		/// Gets or sets gradient angle.
		/// </summary>
		public int BackColorGradientAngle=0;
		/// <summary>
		/// Gets or sets command part back color when mouse is over the part.
		/// </summary>
		public Color MouseOverBackColor=Color.Empty;
		/// <summary>
		/// Gets or sets command part end gradient back color when mouse is over the part.
		/// </summary>
		public Color MouseOverBackColor2=Color.Empty;
		/// <summary>
		/// Gets or sets text color when mouse is over the part.
		/// </summary>
		public Color MouseOverForeColor=Color.Empty;
		/// <summary>
		/// Gets or sets gradient angle.
		/// </summary>
		public int MouseOverBackColorGradientAngle=0;
		
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		/// <param name="g"></param>
		public NodeCommandPartRendererEventArgs(Graphics g)
		{
		}
	}
}
