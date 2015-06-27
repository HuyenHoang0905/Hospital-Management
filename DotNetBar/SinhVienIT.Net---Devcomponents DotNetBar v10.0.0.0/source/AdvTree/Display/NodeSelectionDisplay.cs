using System.Drawing;
using DevComponents.WinForms.Drawing;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represent class that paints selection around node.
	/// </summary>
	internal class NodeSelectionDisplay
	{
		public void PaintSelection(SelectionRendererEventArgs info)
		{
            if (info.SelectionBoxStyle == eSelectionStyle.HighlightCells)
                PaintHighlightCellsSelectionStyle(info);
            else if (info.SelectionBoxStyle == eSelectionStyle.FullRowSelect)
                PaintFullRowSelectSelectionStyle(info);
            else if (info.SelectionBoxStyle == eSelectionStyle.NodeMarker)
                PaintNodeMarkerSelectionStyle(info);
			
		}

        public void PaintHotTracking(SelectionRendererEventArgs info)
        {
            // Full row is just a rectangle with the background...
            Shape[] fullRowShapes = GetHotTrackingShapes();
            Graphics g = info.Graphics;
            Rectangle bounds = info.Bounds;
            bounds.Width--;
            bounds.Height--;
            foreach (Shape shape in fullRowShapes)
            {
                shape.Paint(g, bounds);
            }
        }

        private void PaintFullRowSelectSelectionStyle(SelectionRendererEventArgs info)
        {
            // Full row is just a rectangle with the background...
            Shape[] fullRowShapes = GetFullRowShapes(info.TreeActive);
            Graphics g = info.Graphics;
            Rectangle bounds = info.Bounds;
            //bounds.Width--;
            //bounds.Height--;

            foreach (Shape shape in fullRowShapes)
            {
                shape.Paint(g, bounds);
            }
        }

        Shape[] _FullRowShapes = null;
        private Shape[] GetFullRowShapes(bool treeActive)
        {
            if (_FullRowShapes == null)
            {
                _FullRowShapes = new Shape[1];
                _FullRowShapes[0] = new RectangleShape();
            }
            RectangleShape shape = (RectangleShape)_FullRowShapes[0];
            SelectionColorTable colors = treeActive ? _SelectionColors.FullRowSelect : _SelectionColors.FullRowSelectInactive;
            shape.Fill = colors.Fill;
            shape.Border = colors.Border;

            return _FullRowShapes;
        }

        private void PaintHighlightCellsSelectionStyle(SelectionRendererEventArgs info)
        {
            // Full row is just a rectangle with the background...
            Shape[] fullRowShapes = GetHighlightCellsShapes(info.TreeActive);
            Graphics g = info.Graphics;
            Rectangle bounds = info.Bounds;
            bounds.Width--;
            bounds.Height--;
            foreach (Shape shape in fullRowShapes)
            {
                shape.Paint(g, bounds);
            }
        }

        Shape[] _HighlightCellsShapes = null;
        private Shape[] GetHighlightCellsShapes(bool treeActive)
        {
            if (_HighlightCellsShapes == null)
            {
                _HighlightCellsShapes = new Shape[1];
                RectangleShape rectShape = new RectangleShape();
                rectShape.CornerRadius = new CornerRadius(2);
                RectangleShape inner = new RectangleShape();
                //inner.CornerRadius = new CornerRadius(2);
                rectShape.Content = inner;
                _HighlightCellsShapes[0] = rectShape;
            }

            SelectionColorTable colorTable = treeActive ? _SelectionColors.HighlightCells : _SelectionColors.HighlightCellsInactive;
            RectangleShape shape = (RectangleShape)_HighlightCellsShapes[0];
            shape.Fill = colorTable.Fill;
            shape.Border = colorTable.Border;
            shape = (RectangleShape)shape.Content;
            shape.Border = colorTable.InnerBorder;

            return _HighlightCellsShapes;
        }

        Shape[] _HotTrackingShapes = null;
        private Shape[] GetHotTrackingShapes()
        {
            if (_HotTrackingShapes == null)
            {
                _HotTrackingShapes = new Shape[1];
                RectangleShape rectShape = new RectangleShape();
                rectShape.CornerRadius = new CornerRadius(2);
                RectangleShape inner = new RectangleShape();
                //inner.CornerRadius = new CornerRadius(2);
                rectShape.Content = inner;
                _HotTrackingShapes[0] = rectShape;
            }

            SelectionColorTable colorTable = _SelectionColors.NodeHotTracking;
            RectangleShape shape = (RectangleShape)_HotTrackingShapes[0];
            shape.Fill = colorTable.Fill;
            shape.Border = colorTable.Border;
            shape = (RectangleShape)shape.Content;
            shape.Border = colorTable.InnerBorder;

            return _HotTrackingShapes;
        }

        private void PaintNodeMarkerSelectionStyle(SelectionRendererEventArgs info)
        {
            Rectangle inside = info.Bounds;
            int borderWidth = 4;
            inside.Inflate(1, 1);
            inside.Width--;
            inside.Height--;
            Rectangle outside = info.Bounds;
            outside.Inflate(borderWidth, borderWidth);
            outside.Width--;
            outside.Height--;

            SelectionColorTable colorTable = info.TreeActive ? _SelectionColors.NodeMarker : _SelectionColors.NodeMarkerInactive;

            if (colorTable.Border != null)
            {
                Pen pen = colorTable.Border.CreatePen();
                if (pen != null)
                {
                    info.Graphics.DrawRectangle(pen, inside);
                    info.Graphics.DrawRectangle(pen, outside);
                    pen.Dispose();
                }
            }

            if (colorTable.Fill != null)
            {
                Brush brush = colorTable.Fill.CreateBrush(outside);
                if (brush != null)
                {
                    Region region = new Region(outside);
                    region.Exclude(inside);
                    info.Graphics.FillRegion(brush, region);
                    brush.Dispose();
                }
            }
        }

        private TreeSelectionColors _SelectionColors = null;
        public TreeSelectionColors SelectionColors
        {
            get { return _SelectionColors; }
            set { _SelectionColors = value; }
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
