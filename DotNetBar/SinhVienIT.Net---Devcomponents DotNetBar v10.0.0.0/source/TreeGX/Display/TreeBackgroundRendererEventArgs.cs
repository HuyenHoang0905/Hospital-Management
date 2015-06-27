using System;
using System.Drawing;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Provides data for tree background rendering events.
	/// </summary>
	public class TreeBackgroundRendererEventArgs
	{
		/// <summary>
		/// Gets or sets reference to Graphics object, canvas tree background is rendered on.
		/// </summary>
		public System.Drawing.Graphics Graphics=null;
		
		/// <summary>
		/// Gets or sets the reference to TreeGX control.
		/// </summary>
		public TreeGX TreeGX = null;
		
		/// <summary>
		/// Creates new instance of the class and initializes it with default values.
		/// </summary>
		/// <param name="g">Reference to graphics object.</param>
		public TreeBackgroundRendererEventArgs(Graphics g, TreeGX tree)
		{
			this.Graphics = g;
			this.TreeGX = tree;
		}
	}
}
