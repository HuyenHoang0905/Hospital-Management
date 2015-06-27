using System;
using System.Drawing;

namespace DevComponents.Tree.Display
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
            Rectangle expandPartBounds = e.ExpandPartBounds;
            if (expandPartBounds.IsEmpty)
                return;

            expandPartBounds.Width--;
            expandPartBounds.Height--;

            Brush brush = GetBackgroundBrush(e);
            if (brush != null)
            {
                e.Graphics.FillRectangle(brush, expandPartBounds);
                brush.Dispose();
            }

            using (Pen pen = GetBorderPen(e))
            {
                e.Graphics.DrawRectangle(pen, expandPartBounds);
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
