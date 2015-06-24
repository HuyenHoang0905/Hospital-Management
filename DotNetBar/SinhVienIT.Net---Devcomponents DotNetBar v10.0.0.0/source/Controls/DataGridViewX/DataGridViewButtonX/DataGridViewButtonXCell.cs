using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewButtonXCell : DataGridViewButtonCell
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
            get { return (typeof (string)); }
        }

        #endregion

        #endregion

        #region GetContentBounds

        /// <summary>
        /// GetContentBounds
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="cellStyle"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
        {
            Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(ColumnIndex, rowIndex, false);
            cellBounds.Location = new Point(0, 0);

            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn)OwningColumn;
            ButtonX bx = oc.ButtonX;

            Rectangle rBt = GetButtonBounds(cellBounds, true);

            if (bx.ButtonItem.SubItems.Count > 0)
                rBt.Width -= bx.ButtonItem.SubItemsExpandWidth;

            return (rBt);
        }

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
                        DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn)OwningColumn;
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
            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn)OwningColumn;
            ButtonX bx = oc.ButtonX;

            Rectangle rBt = GetButtonBounds(cellBounds, true);

            if (rBt.Width > 0 && rBt.Height > 0)
            {
                bool mouseIsOver = bx.ButtonItem.MouseIsOver;
                bool mouerIsOverExpand = bx.ButtonItem.MouseIsOverExpand;
                bool mouseIsDown = bx.ButtonItem.MouseIsDown;
                bool buttonIsExpanded = bx.ButtonItem.ButtonIsExpanded;

                try
                {
                    string s = oc.Text;

                    oc.InCellCallBack = true;

                    if (rowIndex != oc.ActiveRowIndex)
                    {
                        bx.ButtonItem.MouseIsOver = false;
                        bx.ButtonItem.MouseIsOverExpand = false;
                        bx.ButtonItem.MouseIsDown = false;
                        bx.ButtonItem.ButtonIsExpanded = false;
                    }

                    bx.Font = cellStyle.Font;
                    bx.ForeColor = cellStyle.ForeColor;
                    bx.BackColor = cellStyle.BackColor;

                    if (rowIndex < DataGridView.RowCount)
                    {
                        bx.Text = UseColumnTextForButtonValue ? s : GetValue(value);

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
                    bx.ButtonItem.MouseIsOver = mouseIsOver;
                    bx.ButtonItem.MouseIsOverExpand = mouerIsOverExpand;
                    bx.ButtonItem.MouseIsDown = mouseIsDown;
                    bx.ButtonItem.ButtonIsExpanded = buttonIsExpanded;

                    oc.InCellCallBack = false;
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

            DoMouseEnter(rowIndex);
        }

        /// <summary>
        /// Establishes the given rowIndex as the
        /// ActiveRowIndex.
        /// </summary>
        /// <param name="rowIndex"></param>
        internal void DoMouseEnter(int rowIndex)
        {
            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn)OwningColumn;
            ButtonX bx = oc.ButtonX;

            oc.CurrentRowIndex = rowIndex;

            if (bx.Expanded == false)
            {
                oc.ActiveRowIndex = rowIndex;

                bx.ButtonItem.InternalMouseEnter();

                RefreshButton(ColumnIndex, rowIndex);
            }
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

            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn)OwningColumn;
            ButtonX bx = oc.ButtonX;

            oc.CurrentRowIndex = -1;

            if (bx.Expanded == false)
            {
                oc.ActiveRowIndex = -1;

                bx.ButtonItem.InternalMouseLeave();

                RefreshButton(ColumnIndex, rowIndex);
            }
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

            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn) OwningColumn;
            ButtonX bx = oc.ButtonX;

            if (oc.ActiveRowIndex == e.RowIndex || bx.Expanded == false)
            {
                bx.ButtonItem.InternalMouseMove(
                    new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));

                RefreshButton(e.ColumnIndex, e.RowIndex);
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

            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn) OwningColumn;
            ButtonX bx = oc.ButtonX;

            if (DataGridView.Controls.Contains(bx) == false)
                DataGridView.Controls.Add(bx);

            if (oc.ExpandClosed == false || bx.ButtonItem.MouseIsOverExpand == false)
            {
                Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Rectangle rBt = GetButtonBounds(cellBounds, false);

                Point pt = e.Location;
                pt.Offset(rBt.Location);

                if (rBt.Contains(pt) == true)
                {
                    bx.Bounds = rBt;

                    bx.ButtonItem.InternalMouseDown(
                        new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));

                    DataGridView.Invalidate(rBt);
                }
            }

            oc.ExpandClosed = false;
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

            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn) OwningColumn;
            ButtonX bx = oc.ButtonX;

            Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            Rectangle rBt = GetButtonBounds(cellBounds, false);

            DataGridView.Invalidate(rBt);

            bx.Bounds = rBt;
            bx.ButtonItem.InternalMouseUp(
                new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
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
            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn)OwningColumn;

            cellBounds.Height--;

            if (Selected == false)
                cellBounds.Width -= (oc.DividerWidth + 1);

            return (cellBounds);
        }

        #endregion

        #region GetButtonBounds

        /// <summary>
        /// Gets the button bounds for the given cell
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <param name="localize"></param>
        /// <returns></returns>
        private Rectangle GetButtonBounds(Rectangle cellBounds, bool localize)
        {
            DataGridViewButtonXColumn oc = (DataGridViewButtonXColumn)OwningColumn;

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
        /// <returns></returns>
        private string GetValue(object value)
        {
            return (value != Convert.DBNull ? Convert.ToString(value) : "" );
        }

        #endregion

        #region RefreshButton

        /// <summary>
        /// Initiates the refresh of the cell button
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        private void RefreshButton(int columnIndex, int rowIndex)
        {
            Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(columnIndex, rowIndex, false);
            Rectangle rBt = GetButtonBounds(cellBounds, false);

            DataGridView.Invalidate(rBt);
        }

        #endregion
    }
}
