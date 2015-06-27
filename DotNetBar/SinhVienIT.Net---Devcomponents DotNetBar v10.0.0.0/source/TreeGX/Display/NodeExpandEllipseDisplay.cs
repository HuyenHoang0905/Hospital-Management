using System.Drawing;

namespace DevComponents.Tree.Display
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
            Rectangle expandPartBounds = e.ExpandPartBounds;
            if (expandPartBounds.IsEmpty)
                return;

            Brush brush = GetBackgroundBrush(e);

            expandPartBounds.Width--;
            expandPartBounds.Height--;

            if (brush != null)
            {
                e.Graphics.FillEllipse(brush, expandPartBounds);
                brush.Dispose();
            }

            using (Pen pen = GetBorderPen(e))
            {
                e.Graphics.DrawEllipse(pen, expandPartBounds);
            }

            if (e.Node.Expanded)
            {
                using (Pen pen = GetExpandPen(e))
                {
                    e.Graphics.DrawLine(pen, expandPartBounds.X + 2, expandPartBounds.Y + expandPartBounds.Height / 2, expandPartBounds.Right - 2, expandPartBounds.Y + expandPartBounds.Height / 2);
                }
            }
            else
            {
                using (Pen pen = GetExpandPen(e))
                {
                    e.Graphics.DrawLine(pen, expandPartBounds.X + 2, expandPartBounds.Y + expandPartBounds.Height / 2, expandPartBounds.Right - 2, expandPartBounds.Y + expandPartBounds.Height / 2);
                    e.Graphics.DrawLine(pen, expandPartBounds.X + expandPartBounds.Width / 2, expandPartBounds.Y + 2, expandPartBounds.X + expandPartBounds.Width / 2, expandPartBounds.Bottom - 2);
                }
            }
        }
	}
}
