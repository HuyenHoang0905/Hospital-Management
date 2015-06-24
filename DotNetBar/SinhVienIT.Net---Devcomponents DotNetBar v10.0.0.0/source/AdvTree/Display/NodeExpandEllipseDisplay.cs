using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents class that paints elliptical expand button.
	/// </summary>
	public class NodeExpandEllipseDisplay:NodeExpandDisplay
	{
		/// <summary>Draws ellipse type expand button.</summary>
		/// <param name="e">Expand context drawing information.</param>
		public override void DrawExpandButton(NodeExpandPartRendererEventArgs e)
		{
			if(e.ExpandPartBounds.IsEmpty)
				return;
			
			Brush brush=GetBackgroundBrush(e);
			if(brush!=null)
			{
				e.Graphics.FillEllipse(brush,e.ExpandPartBounds);
				brush.Dispose();
			}

            Pen pen=GetBorderPen(e);
            if (pen != null)
            {
                SmoothingMode sm = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(pen, e.ExpandPartBounds);
                e.Graphics.SmoothingMode = sm;
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
