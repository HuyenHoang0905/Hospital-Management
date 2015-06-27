using System.Drawing;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Represent class that paints selection around node.
	/// </summary>
	internal class NodeSelectionDisplay
	{
		public void PaintSelection(SelectionRendererEventArgs info)
		{
			Rectangle inside=info.Bounds;
			inside.Inflate(1,1);
			inside.Width--;
			inside.Height--;
			Rectangle outside=info.Bounds;
			outside.Inflate(info.Width,info.Width);
			outside.Width--;
			outside.Height--;

			if(!info.BorderColor.IsEmpty)
			{
				using(Pen pen=new Pen(info.BorderColor,1))
				{
					info.Graphics.DrawRectangle(pen,inside);
					info.Graphics.DrawRectangle(pen,outside);
				}
			}

			if(!info.FillColor.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(info.FillColor))
				{
					Region region=new Region(outside);
					region.Exclude(inside);
					info.Graphics.FillRegion(brush,region);
				}
			}
		}
	}

	internal class NodeSelectionDisplayInfo
	{
		public Node Node=null;
		public Graphics Graphics=null;
		public Rectangle Bounds=Rectangle.Empty;
		public Color BorderColor=Color.Empty;
		public Color FillColor=Color.Empty;
		public int Width=4;
	}
}
