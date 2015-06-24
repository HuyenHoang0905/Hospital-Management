

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents expand button display using predefined images.
	/// </summary>
	public class NodeExpandImageDisplay:NodeExpandDisplay
	{
		/// <summary>
		/// Draws image type expand button.
		/// </summary>
		/// <param name="e">Expand context information</param>
		public override void DrawExpandButton(NodeExpandPartRendererEventArgs e)
		{
			if(e.Node.Expanded)
			{
				if(e.ExpandImageCollapse!=null)
					e.Graphics.DrawImage(e.ExpandImageCollapse,e.ExpandPartBounds);
			}
			else if(e.ExpandImage!=null)
				e.Graphics.DrawImage(e.ExpandImage,e.ExpandPartBounds);
		}
	}
}
