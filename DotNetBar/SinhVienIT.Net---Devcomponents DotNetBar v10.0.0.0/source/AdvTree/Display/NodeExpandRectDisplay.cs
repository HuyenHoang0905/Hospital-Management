using System;
using System.Drawing;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents class that paints rectangular expand button.
	/// </summary>
	public class NodeExpandRectDisplay:NodeExpandDisplay
	{
		/// <summary>
		/// Draw rectangular type expand button.
		/// </summary>
		/// <param name="e">Expand button context information.</param>
		public override void DrawExpandButton(NodeExpandPartRendererEventArgs e)
		{
			if(e.ExpandPartBounds.IsEmpty)
				return;

			Brush brush=GetBackgroundBrush(e);
			if(brush!=null)
			{
				e.Graphics.FillRectangle(brush,e.ExpandPartBounds);
				brush.Dispose();
			}

            Pen pen = GetBorderPen(e);
            if (pen != null)
            {
                e.Graphics.DrawRectangle(pen, e.ExpandPartBounds);
                pen.Dispose();
                pen = null;
            }

			if(e.Node.Expanded)
			{
                pen = GetExpandPen(e);
                if (pen != null)
                {
                    e.Graphics.DrawLine(pen, e.ExpandPartBounds.X + 2, e.ExpandPartBounds.Y + e.ExpandPartBounds.Height / 2, e.ExpandPartBounds.Right - 2, e.ExpandPartBounds.Y + e.ExpandPartBounds.Height / 2);
                    pen.Dispose();
                }
			}
			else
			{
                pen = GetExpandPen(e);
                if (pen != null)
				{
					e.Graphics.DrawLine(pen,e.ExpandPartBounds.X+2,e.ExpandPartBounds.Y+e.ExpandPartBounds.Height/2,e.ExpandPartBounds.Right-2,e.ExpandPartBounds.Y+e.ExpandPartBounds.Height/2);
					e.Graphics.DrawLine(pen,e.ExpandPartBounds.X+e.ExpandPartBounds.Width/2,e.ExpandPartBounds.Y+2,e.ExpandPartBounds.X+e.ExpandPartBounds.Width/2,e.ExpandPartBounds.Bottom-2);
                    pen.Dispose();
				}
			}
		}
	}
}
