using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewProgressBarXCell : DataGridViewButtonCell
    {
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
                        DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;
                        Bitmap bm = oc.GetCellBitmap(cellBounds);

                        if (bm != null)
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                            {
                                PaintButtonBackground(g, cellStyle, rBk);
                                PaintButtonContent(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, bm);

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

        #region PaintButtonBackground

        /// <summary>
        /// Paints the Button background
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellStyle"></param>
        /// <param name="rBack"></param>
        private void PaintButtonBackground(Graphics g,
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

        #region PaintButtonContent

        /// <summary>
        /// Paints the button background and content
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="paintParts"></param>
        /// <param name="bm"></param>
        private void PaintButtonContent(Rectangle cellBounds, int rowIndex, object value, 
            DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Bitmap bm)
        {
            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;
            ProgressBarX bx = oc.ProgressBarX;

            Rectangle rBt = GetContentBounds(cellBounds, true);

            if (rBt.Width > 0 && rBt.Height > 0)
            {
                string s = oc.Text;

                oc.InCallBack = true;

                try
                {
                    bx.Font = cellStyle.Font;
                    bx.ForeColor = cellStyle.ForeColor;
                    bx.BackColor = cellStyle.BackColor;

                    bx.Value = GetValue(value, bx.Minimum);

                    if (rowIndex < DataGridView.RowCount)
                    {
                        bx.Text = s;

                        if (PartsSet(paintParts, DataGridViewPaintParts.ContentForeground))
                            oc.OnBeforeCellPaint(rowIndex, ColumnIndex);
                    }
                    else
                    {
                        bx.Text = "";
                    }

                    bx.Bounds = rBt;
                    bx.RecalcLayout();
                    bx.DrawToBitmap(bm, rBt);
                }
                finally
                {
                    oc.InCallBack = false;
                }
            }
        }

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

            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn) OwningColumn;
            ProgressBarX bx = oc.ProgressBarX;

            bx.ProgressBarItem.InternalMouseEnter();
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

            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;
            ProgressBarX bx = oc.ProgressBarX;

            bx.ProgressBarItem.InternalMouseLeave();
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

            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;
            ProgressBarX bx = oc.ProgressBarX;

            bx.ProgressBarItem.InternalMouseMove(
                new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
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

            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;
            ProgressBarX bx = oc.ProgressBarX;

            Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            Rectangle rBt = GetContentBounds(cellBounds, false);

            Point pt = e.Location;
            pt.Offset(rBt.Location);

            if (rBt.Contains(pt) == true)
            {
                bx.Bounds = rBt;

                bx.ProgressBarItem.InternalMouseDown(
                    new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));

                DataGridView.Invalidate(rBt);
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

            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;
            ProgressBarX bx = oc.ProgressBarX;

            Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            Rectangle rBt = GetContentBounds(cellBounds, false);

            bx.Bounds = rBt;

            bx.ProgressBarItem.InternalMouseUp(
                new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));

            DataGridView.Invalidate(rBt);
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
            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;

            cellBounds.Height--;

            if (Selected == false)
                cellBounds.Width -= (oc.DividerWidth + 1);

            return (cellBounds);
        }

        #endregion

        #region GetContentBounds

        /// <summary>
        /// Gets the content bounds for the given cell
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <param name="localize"></param>
        /// <returns></returns>
        private Rectangle GetContentBounds(Rectangle cellBounds, bool localize)
        {
            DataGridViewProgressBarXColumn oc = (DataGridViewProgressBarXColumn)OwningColumn;

            cellBounds.Width -= (oc.DividerWidth + 3);
            cellBounds.Height -= 3;

            if (localize == true)
                cellBounds.Location = new Point(0, 0);

            cellBounds.X++;
            cellBounds.Y++;

            return (cellBounds);
        }

        #endregion

        #region GetValue

        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        private int GetValue(object value, int minimum)
        {
            if (value == Convert.DBNull ||
                (value is string && String.IsNullOrEmpty((string) value) == true))
            {
                return (minimum);
            }

            return (Convert.ToInt32(value));
        }

        #endregion
    }
}
