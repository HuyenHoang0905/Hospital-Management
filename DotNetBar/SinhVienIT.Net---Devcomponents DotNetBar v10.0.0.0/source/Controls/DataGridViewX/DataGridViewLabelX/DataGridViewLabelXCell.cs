using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewLabelXCell : DataGridViewButtonCell
    {
        #region Private variables

        private DataGridViewCellStyle _CellStyle;

        #endregion

        #region Public properties

        #region EditType

        /// <summary>
        /// Gets the Type of the editing control associated with the cell
        /// </summary>
        public override Type EditType
        {
            get { return (null); }
        }

        #endregion

        #region FormattedValueType

        /// <summary>
        /// FormattedValueType
        /// </summary>
        public override Type FormattedValueType
        {
            get { return (typeof(string)); }
        }

        #endregion

        #endregion

        #region GetAdjustedEditingControlBounds

        /// <summary>
        /// GetAdjustedEditingControlBounds
        /// </summary>
        /// <param name="editingControlBounds"></param>
        /// <param name="cellStyle"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private Rectangle GetAdjustedEditingControlBounds(
            Rectangle editingControlBounds, DataGridViewCellStyle cellStyle, int rowIndex)
        {
            // Add a 1 pixel padding around the editing control

            editingControlBounds.X += 1;
            editingControlBounds.Y += 1;
            editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);
            editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);

            // Adjust the vertical location of the editing control

            Rectangle r = GetContentBounds(rowIndex);

            if (cellStyle != null)
            {
                switch (cellStyle.Alignment)
                {
                    case DataGridViewContentAlignment.TopCenter:
                        editingControlBounds.X += (editingControlBounds.Width - r.Width) / 2;
                        break;

                    case DataGridViewContentAlignment.TopRight:
                        editingControlBounds.X = (editingControlBounds.Right - r.Width);
                        break;

                    case DataGridViewContentAlignment.MiddleLeft:
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height)/2;
                        break;

                    case DataGridViewContentAlignment.MiddleCenter:
                        editingControlBounds.X += (editingControlBounds.Width - r.Width)/2;
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height)/2;
                        break;

                    case DataGridViewContentAlignment.MiddleRight:
                        editingControlBounds.X = (editingControlBounds.Right - r.Width);
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height)/2;
                        break;

                    case DataGridViewContentAlignment.BottomLeft:
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height);
                        break;

                    case DataGridViewContentAlignment.BottomCenter:
                        editingControlBounds.X += (editingControlBounds.Width - r.Width)/2;
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height);
                        break;

                    case DataGridViewContentAlignment.BottomRight:
                        editingControlBounds.X = (editingControlBounds.Right - r.Width);
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height);
                        break;
                }
            }

            if (editingControlBounds.Y < 1)
                editingControlBounds.Y = 1;

            editingControlBounds.Width = Math.Max(1, r.Width);
            editingControlBounds.Height = Math.Max(1, r.Height);

            return (editingControlBounds);
        }

        #endregion

        #region GetPreferredSize

        /// <summary>
        /// GetPreferredSize
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="cellStyle"></param>
        /// <param name="rowIndex"></param>
        /// <param name="constraintSize"></param>
        /// <returns></returns>
        protected override Size GetPreferredSize(Graphics graphics,
               DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            Size size = Size.Empty;
            
            if (DataGridView == null)
                return new Size(-1, -1);

            DataGridViewLabelXColumn oc = OwningColumn as DataGridViewLabelXColumn;

            if (oc != null)
            {
                LabelX lx = oc.LabelX;

                if (oc.LabelX.IsHandleCreated == false)
                {
                    if (oc.LabelX.Parent == null)
                    {
                        Form form = oc.DataGridView.FindForm();

                        if (form != null)
                            oc.LabelX.Parent = form;
                    }

                    oc.LabelX.Visible = true;
                    oc.LabelX.Visible = false;
                }

                FreeDimension fd = GetDFromConstraint(constraintSize);

                if (fd != FreeDimension.Width)
                    constraintSize.Width -= 2;

                lx.Font = cellStyle.Font;
                lx.WordWrap = GetWordWrap(oc, cellStyle);
                lx.TextAlignment = GetTextAlignment(cellStyle.Alignment);
                lx.MaximumSize = constraintSize;
                lx.Image = oc.Image;

                string s = lx.Text = @" ";

                if (rowIndex < DataGridView.RowCount)
                    s = GetValue(DataGridView.Rows[rowIndex].Cells[ColumnIndex].Value);

                if (string.IsNullOrEmpty(s) == true)
                    s = " ";

                lx.Text = s;

                size = lx.GetPreferredSize(constraintSize);

                Rectangle stdBorderWidths = BorderWidths(new DataGridViewAdvancedBorderStyle());

                int hpad = (stdBorderWidths.Left + stdBorderWidths.Width) + cellStyle.Padding.Horizontal;
                int vpad = (stdBorderWidths.Top + stdBorderWidths.Height) + cellStyle.Padding.Vertical;

                switch (fd)
                {
                    case FreeDimension.Height:
                        size.Width = 0;
                        break;

                    case FreeDimension.Width:
                        size.Height = 0;
                        break;
                }

                if (fd != FreeDimension.Height)
                    size.Width += (hpad + 4);

                if (fd != FreeDimension.Width)
                    size.Height += (vpad + 2);
            }

            return size;
        }

        #region GetDFromConstraint

        private enum FreeDimension
        {
            Both,
            Height,
            Width
        }

        private FreeDimension GetDFromConstraint(Size constraintSize)
        {
            if (constraintSize.Width == 0)
            {
                if (constraintSize.Height == 0)
                    return (FreeDimension.Both);

                return (FreeDimension.Width);
            }

            return (FreeDimension.Height);
        }

        #endregion

        #endregion

        #region Paint

        #region Paint

        /// <summary>
        /// Cell painting
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="elementState"></param>
        /// <param name="value"></param>
        /// <param name="formattedValue"></param>
        /// <param name="errorText"></param>
        /// <param name="cellStyle"></param>
        /// <param name="advancedBorderStyle"></param>
        /// <param name="paintParts"></param>
        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState,
            object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (DataGridView != null)
            {
                // First paint the borders of the cell

                if (PartsSet(paintParts, DataGridViewPaintParts.Border))
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

                // Now paint the background and content

                if (PartsSet(paintParts, DataGridViewPaintParts.Background))
                {
                    Rectangle rBk = GetBackBounds(cellBounds);

                    if (rBk.Height > 0 && rBk.Width > 0)
                    {
                        DataGridViewLabelXColumn oc = (DataGridViewLabelXColumn)OwningColumn;
                        Bitmap bm = oc.GetCellBitmap(cellBounds);

                        if (bm != null)
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                            {
                                PaintLabelBackground(g, cellStyle, rBk);
                                PaintLabelContent(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, g);

                                graphics.DrawImageUnscaledAndClipped(bm, rBk);
                            }

                            if ((DataGridView.ShowCellErrors == true) && 
                                (paintParts & DataGridViewPaintParts.ErrorIcon) == DataGridViewPaintParts.ErrorIcon)
                            {
                                base.PaintErrorIcon(graphics, clipBounds, cellBounds, errorText);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region PaintLabelBackground

        /// <summary>
        /// Paints the Label background
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellStyle"></param>
        /// <param name="rBack"></param>
        private void PaintLabelBackground(Graphics g,
            DataGridViewCellStyle cellStyle, Rectangle rBack)
        {
            Rectangle r = rBack;
            r.Location = new Point(0, 0);

            DataGridViewX dx = DataGridView as DataGridViewX;

            if (dx != null && dx.Enabled == true && Selected == true)
            {
                Office2007ButtonItemPainter.PaintBackground(g, dx.ButtonStateColorTable,
                    r, RoundRectangleShapeDescriptor.RectangleShape, false,
                    false);
            }
            else
            {
                using (Brush br = new SolidBrush(cellStyle.BackColor))
                    g.FillRectangle(br, r);
            }
        }

        #endregion

        #region PaintLabelContent

        /// <summary>
        /// Paints the Label background and content
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="paintParts"></param>
        /// <param name="g"></param>
        private void PaintLabelContent(Rectangle cellBounds, int rowIndex, object value,
            DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Graphics g)
        {
            DataGridViewLabelXColumn oc = (DataGridViewLabelXColumn) OwningColumn;
            LabelX lx = oc.LabelX;

            _CellStyle = cellStyle;

            string s = GetValue(value);

            oc.InCallBack = true;
            bool wordWrap = lx.WordWrap;
            StringAlignment textAlignment = lx.TextAlignment;

            GraphicsState gs = g.Save();

            try
            {
                cellBounds.X = 0;
                cellBounds.Y = 0;
                cellBounds.Width -= (oc.DividerWidth + 1);
                cellBounds.Height -= 1;

                lx.Font = cellStyle.Font;
                lx.ForeColor = cellStyle.ForeColor;
                lx.BackColor = Selected ? Color.Transparent : cellStyle.BackColor;

                lx.WordWrap = GetWordWrap(oc, cellStyle);
                lx.TextAlignment = GetTextAlignment(cellStyle.Alignment);

                Rectangle r = cellBounds;

                if (rowIndex < DataGridView.RowCount)
                {
                    lx.Text = s;

                    if (PartsSet(paintParts, DataGridViewPaintParts.ContentForeground))
                        oc.OnBeforeCellPaint(rowIndex, ColumnIndex);

                    r = GetAdjustedEditingControlBounds(cellBounds, cellStyle, rowIndex);
                }
                else
                {
                    lx.Text = " ";
                }

                lx.CallBasePaintBackground = false;

                g.TranslateTransform(r.X, r.Y);

                lx.Bounds = r;
                lx.RecalcLayout();
                lx.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
            }
            finally
            {
                lx.WordWrap = wordWrap;
                lx.TextAlignment = textAlignment;
                oc.InCallBack = false;

                g.Restore(gs);
            }
        }

        #region GetWordWrap

        private bool GetWordWrap(DataGridViewLabelXColumn oc, DataGridViewCellStyle cellStyle)
        {
            if (oc.WordWrap == true)
                return (true);

            switch (cellStyle.WrapMode)
            {
                case DataGridViewTriState.True:
                    return (true);

                case DataGridViewTriState.False:
                    return (false);
            }

            return (DataGridView.DefaultCellStyle.WrapMode == DataGridViewTriState.True);
        }

        #endregion

        #endregion

        #endregion

        #region Mouse processing

        #region OnMouseEnter

        /// <summary>
        /// OnMouseEnter
        /// </summary>
        /// <param name="rowIndex"></param>
        protected override void OnMouseEnter(int rowIndex)
        {
            base.OnMouseEnter(rowIndex);

            DataGridViewLabelXColumn oc = (DataGridViewLabelXColumn)OwningColumn;
            LabelX bx = oc.LabelX;

            bx.LabelItem.InternalMouseEnter();
        }

        #endregion

        #region OnMouseLeave

        /// <summary>
        /// Processes MouseLeave events
        /// </summary>
        /// <param name="rowIndex"></param>
        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);

            DataGridViewLabelXColumn oc = (DataGridViewLabelXColumn)OwningColumn;
            LabelX bx = oc.LabelX;

            bx.LabelItem.InternalMouseLeave();
        }

        #endregion

        #region OnMouseMove

        /// <summary>
        /// Processes MouseMove events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseMove(e);

            DataGridViewLabelXColumn oc = OwningColumn as DataGridViewLabelXColumn;

            if (oc != null)
            {
                if ((uint)e.RowIndex < DataGridView.Rows.Count)
                {
                    Point pt = CellAlignPoint(e);

                    oc.LabelX.LabelItem.InternalMouseMove(
                        new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));
                }
            }
        }

        #endregion

        #region OnMouseDown

        /// <summary>
        /// Processes MouseDown events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDown(e);

            DataGridViewLabelXColumn oc = OwningColumn as DataGridViewLabelXColumn;

            if (oc != null)
            {
                Point pt = CellAlignPoint(e);

                oc.LabelX.LabelItem.InternalMouseDown(
                    new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                RefreshCell(e.ColumnIndex, e.RowIndex);
            }
        }

        #endregion

        #region OnMouseUp

        /// <summary>
        /// Processes MouseUp events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);

            DataGridViewLabelXColumn oc = OwningColumn as DataGridViewLabelXColumn;

            if (oc != null)
            {
                Point pt = CellAlignPoint(e);

                oc.LabelX.LabelItem.InternalMouseUp(
                    new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                RefreshCell(e.ColumnIndex, e.RowIndex);
            }
        }

        #endregion

        #region CellAlignPoint

        /// <summary>
        /// CellAlignPoint
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Point CellAlignPoint(DataGridViewCellMouseEventArgs e)
        {
            Rectangle bounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            bounds.Location = new Point(0, 0);

            Rectangle r = GetAdjustedEditingControlBounds(bounds, _CellStyle, e.RowIndex);

            return (new Point(e.X - r.X, e.Y - r.Y));
        }

        #endregion

        #endregion

        #region PartsSet

        /// <summary>
        /// Determines if the given part is set
        /// </summary>
        /// <param name="paintParts"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        private bool PartsSet(DataGridViewPaintParts paintParts, DataGridViewPaintParts parts)
        {
            return ((paintParts & parts) == parts);
        }

        #endregion

        #region GetBackBounds

        /// <summary>
        /// Gets the background bounds for the given cell
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        private Rectangle GetBackBounds(Rectangle cellBounds)
        {
            DataGridViewLabelXColumn oc = (DataGridViewLabelXColumn)OwningColumn;

            cellBounds.Height--;

            if (Selected == false)
                cellBounds.Width -= (oc.DividerWidth + 1);

            return (cellBounds);
        }

        #endregion

        #region GetValue

        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetValue(object value)
        {
            return (value != Convert.DBNull ? Convert.ToString(value) : "");
        }

        #endregion

        #region GetTextAlignment

        /// <summary>
        /// GetHorizontalAlignment
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        private StringAlignment GetTextAlignment(DataGridViewContentAlignment alignment)
        {
            switch (alignment)
            {
                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    return (StringAlignment.Center);

                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    return (StringAlignment.Far);

                default:
                    return (StringAlignment.Near);
            }
        }

        #endregion

        #region RefreshCell

        /// <summary>
        /// Initiates the refresh of the cell label
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        internal void RefreshCell(int columnIndex, int rowIndex)
        {
            DataGridView.InvalidateCell(columnIndex, rowIndex);
        }

        #endregion

    }
}
