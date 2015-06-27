using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DevComponents.DotNetBar;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.AdvTree
{
    internal class ColumnHeaderControl : Control
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ColumnHeaderControl class.
        /// </summary>
        public ColumnHeaderControl()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.StandardDoubleClick | ControlStyles.StandardClick, true);
        }
        #endregion

        #region Internal Implementation
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DisplayHelp.FillRectangle(g, this.ClientRectangle, this.BackColor);

            if (_Columns == null)
            {
                return;
            }

            AdvTree tree = GetTree();
            if (tree != null)
            {
                SmoothingMode sm = g.SmoothingMode;
                TextRenderingHint th = g.TextRenderingHint;
                if (tree.AntiAlias)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                }
                //Creates the drawing matrix with the right zoom;
                if (tree.Zoom != 1)
                {
                    System.Drawing.Drawing2D.Matrix mx = GetTranslationMatrix(tree.Zoom);
                    //use it for drawing
                    g.Transform = mx;
                }

                tree.NodeDisplay.PaintColumnHeaders(_Columns, g, true);

                int columnMoveMarkerIndex = _ColumnMoveMarkerIndex;
                if (columnMoveMarkerIndex >= 0)
                    DevComponents.AdvTree.Display.ColumnHeaderDisplay.PaintColumnMoveMarker(g, tree, columnMoveMarkerIndex, _Columns);

                if (tree.AntiAlias)
                {
                    g.SmoothingMode = sm;
                    g.TextRenderingHint = th;
                }
            }


            base.OnPaint(e);
        }

        private int _ColumnMoveMarkerIndex = -1;
        /// <summary>
        /// Gets or sets the column move marker that marks insertion point for column that is dragged. Marker is drawn before the column specified by this index.
        /// </summary>
        internal int ColumnMoveMarkerIndex
        {
            get { return _ColumnMoveMarkerIndex; }
            set
            {
                _ColumnMoveMarkerIndex = value;
                this.Invalidate();
            }
        }
        

        /// <summary>
        /// Returns mouse position which is translated if control Zoom is not equal 1
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        private Point GetLayoutPosition(float zoom, int x, int y)
        {
            if (zoom == 1)
                return new Point(x, y);
            Point[] p = new Point[] { new Point(x, y) };
            using (System.Drawing.Drawing2D.Matrix mx = GetTranslationMatrix(zoom))
            {
                mx.Invert();
                mx.TransformPoints(p);
            }
            return p[0];
        }

        private System.Drawing.Drawing2D.Matrix GetTranslationMatrix(float zoom)
        {
            System.Drawing.Drawing2D.Matrix mx = new System.Drawing.Drawing2D.Matrix(zoom, 0, 0, zoom, 0, 0);
            return mx;
        }

        private AdvTree GetTree()
        {
            return this.Parent as AdvTree;
        }

        private ColumnHeaderCollection _Columns = null;
        /// <summary>
        /// Gets or sets the column header collection to be rendered.
        /// </summary>
        public ColumnHeaderCollection Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }

        private Cursor _OldCursor = null;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            AdvTree tree = GetTree();

            Point p = GetLayoutPosition(tree.Zoom, e.X, e.Y);
            if (e.Button == MouseButtons.Left && _MouseDownHeader != null && tree.AllowUserToReorderColumns && (Math.Abs(_MouseDownPoint.X-e.X)>1))
            {
                tree.StartColumnReorder(p.X, p.Y);
                return;
            }

            if (tree == null || e.Button != MouseButtons.None || !tree.AllowUserToResizeColumns) return;

            
            if (tree.CanResizeColumnAt(p.X, p.Y))
            {
                if (_OldCursor == null)
                {
                    _OldCursor = this.Cursor;
                    this.Cursor = Cursors.VSplit;
                }
            }
            else 
            {
                ReleaseCursor();
            }

            base.OnMouseMove(e);
        }

        ColumnHeader _MouseDownHeader = null;
        private Point _MouseDownPoint = Point.Empty;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            AdvTree tree = GetTree();
            Point p = Point.Empty;
            
            bool canResize = false;
            _MouseDownPoint = e.Location;

            if (tree != null)
                p = GetLayoutPosition(tree.Zoom, e.X, e.Y);

            if (tree != null && tree.AllowUserToResizeColumns && e.Button == MouseButtons.Left)
                canResize = tree.CanResizeColumnAt(p.X, p.Y);

            if (tree != null)
            {
                p = GetLayoutPosition(tree.Zoom, e.X, e.Y);
                ColumnHeader ch = tree.GetColumnAt(p.X, p.Y, _Columns);
                if (ch != null)
                {
                    _MouseDownHeader = ch;
                    if (!canResize)
                        ch.OnMouseDown(e);
                }
            }

            if (tree == null || e.Button != MouseButtons.Left || !tree.AllowUserToResizeColumns) return;

            if (canResize)
            {
                tree.StartColumnResize(p.X, p.Y);
            }

            this.Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ReleaseCursor();
            _MouseDownPoint = Point.Empty;

            if (_MouseDownHeader!=null)
            {
                _MouseDownHeader.OnMouseUp(e);
                _MouseDownHeader = null;
            }
            this.Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnClick(EventArgs e)
        {
            AdvTree tree = GetTree();
            Point p = Point.Empty;

            if (tree != null)
            {
                Point tp = tree.PointToClient(Control.MousePosition);
                p = GetLayoutPosition(tree.Zoom, tp.X, tp.Y);
                ColumnHeader ch = tree.GetColumnAt(p.X, p.Y, _Columns);
                if (ch != null)
                {
                    ch.OnClick(e);
                }
            }
            this.Invalidate();
            base.OnClick(e);
        }

        internal DateTime IgnoreDoubleClickTime = DateTime.MinValue;
        
        protected override void OnDoubleClick(EventArgs e)
        {
            AdvTree tree = GetTree();
            Point p = Point.Empty;

            if (tree != null)
            {
                Point tp = tree.PointToClient(Control.MousePosition);
                p = GetLayoutPosition(tree.Zoom, tp.X, tp.Y);
                ColumnHeader ch = tree.GetColumnAt(p.X, p.Y, _Columns);
                if (ch != null)
                {
                    DateTime now = DateTime.Now;
                    if (IgnoreDoubleClickTime != DateTime.MinValue && now.Subtract(IgnoreDoubleClickTime).TotalMilliseconds <= SystemInformation.DoubleClickTime)
                    {
                        IgnoreDoubleClickTime = DateTime.MinValue;
                        return;
                    }
                    IgnoreDoubleClickTime = DateTime.MinValue;

                    ch.OnDoubleClick(e);
                }
            }
            this.Invalidate();

            base.OnDoubleClick(e);
        }

        private void ReleaseCursor()
        {
            if (_OldCursor != null)
            {
                this.Cursor = _OldCursor;
                _OldCursor = null;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (Control.MouseButtons != MouseButtons.Left)
                ReleaseCursor();
            base.OnMouseLeave(e);
        }
        #endregion
    }
}
