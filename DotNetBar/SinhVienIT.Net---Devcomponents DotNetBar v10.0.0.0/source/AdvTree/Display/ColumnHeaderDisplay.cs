using System;
using System.Text;
using DevComponents.DotNetBar;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.AdvTree.Display
{
    internal class ColumnHeaderDisplay
    {
        internal void DrawColumnHeader(ColumnHeaderRendererEventArgs e, ElementStyleDisplayInfo di)
        {
            ElementStyleDisplay.Paint(di);
            di.Bounds.Inflate(-1, -1);
            if (di.Bounds.Width > 1 && di.Bounds.Height > 1)
            {
                if (e.ColumnHeader.IsFirstVisible)
                {
                    Rectangle r = di.Bounds;
                    r.Width -= 3;
                    r.X += 3;
                    di.Bounds = r;
                }

                if (e.ColumnHeader.SortDirection != eSortDirection.None && !e.SortIndicatorColor.IsEmpty)
                {
                    using (GraphicsPath sortShapePath = UIGraphics.GetTrianglePath(
                        new Point(di.Bounds.Right - 11, di.Bounds.Y + (di.Bounds.Height - 5) / 2), 9, 
                        (e.ColumnHeader.SortDirection == eSortDirection.Ascending ? eTriangleDirection.Top : eTriangleDirection.Bottom)))
                    {
                        SmoothingMode sm = e.Graphics.SmoothingMode;
                        e.Graphics.SmoothingMode = SmoothingMode.Default;
                        using (SolidBrush brush = new SolidBrush(e.SortIndicatorColor))
                            e.Graphics.FillPath(brush, sortShapePath);
                        e.Graphics.SmoothingMode = sm;
                    }
                    di.Bounds.Width -= 12;
                }

                if (e.ColumnHeader.Image != null)
                {
                    Image image = e.ColumnHeader.Image;
                    Rectangle r = di.Bounds;
                    if (e.ColumnHeader.ImageAlignment == eColumnImageAlignment.Left)
                    {
                        e.Graphics.DrawImage(image, r.X,
                                              r.Y + (r.Height - image.Height) / 2, image.Width, image.Height);
                        r.X += image.Width + 2;
                        r.Width -= image.Width + 2;

                    }
                    else if (e.ColumnHeader.ImageAlignment == eColumnImageAlignment.Right)
                    {
                        e.Graphics.DrawImage(image, r.Right - image.Width,
                                              r.Y + (r.Height - image.Height) / 2, image.Width, image.Height);
                        r.Width -= image.Width + 2;
                    }
                    di.Bounds = r;
                }

                ElementStyleDisplay.PaintText(di, e.ColumnHeader.Text, e.Tree.Font);
            }
        }

        internal static void PaintColumnMoveMarker(Graphics g, AdvTree tree, int columnMoveMarkerIndex, ColumnHeaderCollection columns)
        {
            if (columnMoveMarkerIndex == -1) throw new ArgumentException("columnMoveMarkerIndex must be grater or equal than 0");
            if (columns == null) throw new ArgumentNullException("columns");

            Color lineColor = ColorScheme.GetColor("834DD5");
            Color fillColor = ColorScheme.GetColor("CCCFF8");
            Size markerSize = new Size(10, 14);

            ColumnHeader header = null;

            if (columnMoveMarkerIndex == columns.Count)
                header = columns.LastVisibleColumn;
            else
                header = columns[columnMoveMarkerIndex];
            Rectangle markerBounds = Rectangle.Empty;
            if (columnMoveMarkerIndex == columns.Count)
                markerBounds = new Rectangle(header.Bounds.Right - markerSize.Width, header.Bounds.Bottom - markerSize.Height, markerSize.Width, markerSize.Height);
            else if (columns[columnMoveMarkerIndex] == columns.FirstVisibleColumn)
                markerBounds = new Rectangle(header.Bounds.X, header.Bounds.Bottom - markerSize.Height, markerSize.Width, markerSize.Height);
            else
                markerBounds = new Rectangle(header.Bounds.X - markerSize.Width / 2 - tree.NodeLayout.GetCellLayout().LayoutSettings.CellHorizontalSpacing, header.Bounds.Bottom - markerSize.Height, markerSize.Width, markerSize.Height);
            if (tree.AutoScrollPosition.X != 0)
                markerBounds.Offset(tree.AutoScrollPosition.X, 0);
            using (GraphicsPath path = CreateMarker(markerBounds))
            {
                using (SolidBrush brush = new SolidBrush(fillColor))
                    g.FillPath(brush, path);
                using (Pen pen = new Pen(lineColor, 1))
                    g.DrawPath(pen, path);
            }
        }
        private static GraphicsPath CreateMarker(Rectangle markerBounds)
        {
            markerBounds.Height--;
            GraphicsPath path = new GraphicsPath();
            path.AddLine(markerBounds.X + markerBounds.Width / 2, markerBounds.Bottom, markerBounds.X, markerBounds.Bottom - markerBounds.Width / 2);
            path.AddLine(markerBounds.X, markerBounds.Bottom - markerBounds.Width / 2, markerBounds.X + markerBounds.Width / 3, markerBounds.Bottom - markerBounds.Width / 2);
            path.AddLine(markerBounds.X + markerBounds.Width / 3, markerBounds.Bottom - markerBounds.Width / 2, markerBounds.X + markerBounds.Width / 3, markerBounds.Y);
            path.AddLine(markerBounds.X + markerBounds.Width / 3, markerBounds.Y, markerBounds.Right - markerBounds.Width / 3, markerBounds.Y);
            path.AddLine(markerBounds.Right - markerBounds.Width / 3, markerBounds.Y, markerBounds.Right - markerBounds.Width / 3, markerBounds.Bottom - markerBounds.Width / 2);
            path.AddLine(markerBounds.Right - markerBounds.Width / 3, markerBounds.Bottom - markerBounds.Width / 2, markerBounds.Right, markerBounds.Bottom - markerBounds.Width / 2);
            path.CloseAllFigures();
            return path;
        }
    }
}
