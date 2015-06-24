using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevComponents.AdvTree.Layout;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display
{
    internal class NodeGroupLineDisplay
    {
        private static readonly int LineMargin = 4;
        public void DrawGroupLine(NodeRendererEventArgs e)
        {
            Color lineColor = e.Color;
            Node node = e.Node;

            if (lineColor.IsEmpty || lineColor.A == 0 || node.Cells.Count == 0) return;
            Graphics g = e.Graphics;

            Rectangle r = node.Bounds;
            
            Cell lastCell = node.Cells[node.Cells.Count - 1];
            if (lastCell.CheckBoxVisible && CellLayout.GetCheckBoxHorizontalAlign(lastCell.CheckBoxAlignment, true, eView.Tile) == eHorizontalAlign.Right)
            {
                r.Width -= (lastCell.CheckBoxBounds.Right - r.X) + LineMargin;
                r.X = lastCell.CheckBoxBounds.Right + LineMargin;
            }
            else if (!lastCell.ImageBoundsRelative.IsEmpty && CellLayout.GetHorizontalAlign(lastCell.ImageAlignment, true, eView.Tile) == eHorizontalAlign.Right)
            {
                r.Width -= (lastCell.ImageBounds.Right - r.X) + LineMargin;
                r.X = lastCell.ImageBounds.Right + LineMargin;
            }
            else if (e.Style.TextAlignment == eStyleTextAlignment.Near)
            {
                Rectangle textBounds = lastCell.TextBounds;
                if (lastCell.TextMarkupBody == null)
                    textBounds.Width = TextDrawing.MeasureString(g, lastCell.Text, e.Style.Font).Width;
                r.Width -= (textBounds.Right - r.X) + LineMargin;
                r.X = textBounds.Right + LineMargin;
            }
            else 
                return;

            using (Pen pen = new Pen(lineColor, 1))
            {
                g.DrawLine(pen, r.X, r.Y + r.Height / 2, r.Right, r.Y + r.Height / 2);
            }
        }
    }
}
