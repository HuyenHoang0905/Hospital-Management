using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.AdvTree.Display;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents the line connector display class.
	/// </summary>
	public class LineConnectorDisplay:NodeConnectorDisplay
	{
		/// <summary>
		/// Draws connector line between two nodes.
		/// </summary>
		/// <param name="info">Connector context information.</param>
		public override void DrawConnector(ConnectorRendererEventArgs info)
		{
			if(info.NodeConnector.LineColor.IsEmpty || info.NodeConnector.LineWidth<=0)
				return;

            Point pStart, pEnd;

            // FromNode is null when connector is rendered for the child node
            if (info.FromNode == null)
            {
                Rectangle cellBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, info.ToNode, info.Offset);
                Rectangle expandBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds, info.ToNode, info.Offset);
                pStart = new Point(cellBounds.X - 4, cellBounds.Y + cellBounds.Height / 2);
                pEnd = new Point(expandBounds.X + expandBounds.Width / 2, pStart.Y);
            }
            else
            {
                // FromNode is parent node, ToNode is last visible child node. Connector is vertical line from parent to last visible child
                Rectangle cellBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, info.FromNode, info.Offset);
                Rectangle expandBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds, info.ToNode, info.Offset);
                pStart = new Point(expandBounds.X + expandBounds.Width / 2, cellBounds.Bottom);
                pEnd = new Point(pStart.X, expandBounds.Y + expandBounds.Height / 2);
            }

            Graphics g = info.Graphics;
            using (Pen pen = GetLinePen(info))
            {
                SmoothingMode sm = g.SmoothingMode;
                if (pen.DashStyle != DashStyle.Solid)
                    g.SmoothingMode = SmoothingMode.Default;
                g.DrawLine(pen, pStart, pEnd);
                g.SmoothingMode = sm;
            }
		}
	}
}
