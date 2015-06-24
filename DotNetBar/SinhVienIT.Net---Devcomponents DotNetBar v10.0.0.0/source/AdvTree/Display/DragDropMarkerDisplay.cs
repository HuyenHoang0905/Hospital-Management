using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.AdvTree.Display
{
    internal class DragDropMarkerDisplay
    {
        public void DrawMarker(DragDropMarkerRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;
            if (bounds.IsEmpty || _MarkerColor.IsEmpty) return;

            if (bounds.Width == AdvTree.DragInsertMarkSize) // Vertical insert mark
            {
                using (SolidBrush brush = new SolidBrush(_MarkerColor))
                {
                    using (Pen pen = new Pen(brush, 1))
                    {
                        Point p = new Point(bounds.X + 4, bounds.Y);
                        g.DrawLine(pen, p.X, p.Y, p.X, bounds.Bottom - 1);
                    }

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddLine(bounds.X, bounds.Y, bounds.X + 8, bounds.Y );
                        path.AddLine(bounds.X + 8, bounds.Y, bounds.X + 4, bounds.Y + 4);
                        path.CloseAllFigures();
                        g.FillPath(brush, path);
                    }
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddLine(bounds.X, bounds.Bottom, bounds.X + 8, bounds.Bottom);
                        path.AddLine(bounds.X + 8, bounds.Bottom, bounds.X + 4, bounds.Bottom - 4);
                        path.CloseAllFigures();
                        g.FillPath(brush, path);
                    }
                }
            }
            else
            {
                // Horizontal insert mark
                using (SolidBrush brush = new SolidBrush(_MarkerColor))
                {
                    using (Pen pen = new Pen(brush, 1))
                    {
                        Point p = new Point(bounds.X, bounds.Y + 4);
                        g.DrawLine(pen, p.X, p.Y, bounds.Right - 1, p.Y);
                    }

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddLine(bounds.X, bounds.Y, bounds.X, bounds.Y + 8);
                        path.AddLine(bounds.X, bounds.Y + 8, bounds.X + 4, bounds.Y + 4);
                        path.CloseAllFigures();
                        g.FillPath(brush, path);
                    }
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Y + 8);
                        path.AddLine(bounds.Right, bounds.Y + 8, bounds.Right - 4, bounds.Y + 4);
                        path.CloseAllFigures();
                        g.FillPath(brush, path);
                    }
                }
            }
        }

        private Color _MarkerColor;
        public Color MarkerColor
        {
            get { return _MarkerColor; }
            set { _MarkerColor = value; }
        }
    }
}
