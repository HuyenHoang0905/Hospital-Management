using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewCheckBoxXCell : DataGridViewCell
    {
        #region Private variables

        private bool _MouseEntered;
        private string _Text;

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
        /// <returns></returns>
        private Rectangle GetAdjustedEditingControlBounds(
            Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
        {
            // Add a 1 pixel padding around the editing control

            editingControlBounds.X += 1;
            editingControlBounds.Y += 1;
            editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);
            editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);

            // Adjust the vertical location of the editing control

            Rectangle r = GetContentBounds(editingControlBounds);

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
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height) / 2;
                        break;

                    case DataGridViewContentAlignment.MiddleCenter:
                        editingControlBounds.X += (editingControlBounds.Width - r.Width) / 2;
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height) / 2;
                        break;

                    case DataGridViewContentAlignment.MiddleRight:
                        editingControlBounds.X = (editingControlBounds.Right - r.Width);
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height) / 2;
                        break;

                    case DataGridViewContentAlignment.BottomLeft:
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height);
                        break;

                    case DataGridViewContentAlignment.BottomCenter:
                        editingControlBounds.X += (editingControlBounds.Width - r.Width) / 2;
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

            editingControlBounds.Width = Math.Max(1, editingControlBounds.Width);
            editingControlBounds.Height = Math.Max(1, r.Height);

            return (editingControlBounds);
        }

        #endregion

        #region GetPreferredSize

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
            if (DataGridView == null)
                return (new Size(-1, -1));

            Size preferredSize = Size.Empty;

            if (constraintSize.Width == 0)
            {
                DataGridViewCheckBoxXColumn oc = OwningColumn as DataGridViewCheckBoxXColumn;

                if (oc != null)
                {
                    GetFormattedValue(GetValue(rowIndex), rowIndex,
                                      ref cellStyle, null, null, new DataGridViewDataErrorContexts());

                    if (rowIndex < DataGridView.RowCount)
                        oc.OnBeforeCellPaint(rowIndex, ColumnIndex);

                    CheckBoxX cb = oc.CheckBoxX;

                    SizeF sf = graphics.MeasureString(cb.Text, cellStyle.Font);

                    preferredSize.Width = (int) sf.Width + 4;
                    preferredSize.Height = cb.Height;

                    preferredSize.Width += cb.CheckBoxItem.CheckSignSize.Width;
                }
            }

            return (preferredSize);
        }

        #endregion

        #region GetFormattedValue

        protected override object GetFormattedValue(object value, int rowIndex,
            ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return (base.GetFormattedValue(value, rowIndex,
                ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context));
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
                        DataGridViewCheckBoxXColumn oc = (DataGridViewCheckBoxXColumn)OwningColumn;
                        Bitmap bm = oc.GetCellBitmap(cellBounds);

                        if (bm != null)
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                            {
                                PaintButtonBackground(g, cellStyle, rBk);
                                PaintButtonContent(cellBounds, rowIndex, value, cellStyle, paintParts, g);

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
        /// <param name="g"></param>
        private void PaintButtonContent(Rectangle cellBounds, int rowIndex, object value,
             DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Graphics g)
        {
            DataGridViewCheckBoxXColumn oc = (DataGridViewCheckBoxXColumn) OwningColumn;
            CheckBoxX bx = oc.CheckBoxX;

            string text = bx.Text;
            bool isChecked = bx.Checked;
            object checkValue = bx.CheckValue;
            CheckState checkState = bx.CheckState;
            bool isMouseOver = bx.CheckBoxItem.IsMouseOver;
            bool isMouseDown = bx.CheckBoxItem.IsMouseDown;

            GraphicsState gs = g.Save();

            try
            {
                oc.InCellCallBack = true;

                cellBounds.X = 0;
                cellBounds.Y = 0;
                cellBounds.Width -= (oc.DividerWidth + 1);
                cellBounds.Height -= 1;

                bx.CheckValue = value;

                bx.Font = cellStyle.Font;
                bx.ForeColor = cellStyle.ForeColor;
                bx.BackColor = Selected ? Color.Transparent : cellStyle.BackColor;

                if (rowIndex != oc.ActiveRowIndex)
                {
                    bx.CheckBoxItem.IsMouseOver = false;
                    bx.CheckBoxItem.IsMouseDown = false;
                }

                if (rowIndex < DataGridView.RowCount)
                {
                    bx.Text = text;

                    if (PartsSet(paintParts, DataGridViewPaintParts.ContentForeground))
                        oc.OnBeforeCellPaint(rowIndex, ColumnIndex);
                }
                else
                {
                    bx.Text = "";
                }

                _Text = bx.Text;

                Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);

                bx.CallBasePaintBackground = false;

                g.TranslateTransform(r.X, r.Y);

                bx.Bounds = r;
                bx.RecalcLayout();
                bx.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
            }
            finally
            {
                bx.Text = text;

                if (bx.ThreeState == true)
                    bx.CheckValue = checkValue;

                bx.Checked = isChecked;
                bx.CheckState = checkState;

                bx.CheckBoxItem.IsMouseOver = isMouseOver;
                bx.CheckBoxItem.IsMouseDown = isMouseDown;

                g.Restore(gs);

                oc.InCellCallBack = false;
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

            DataGridViewCheckBoxXColumn oc = OwningColumn as DataGridViewCheckBoxXColumn;

            if (oc != null)
            {
                _MouseEntered = false;

                oc.ActiveRowIndex = rowIndex;
                ToolTipText = _Text;

                RefreshCell(ColumnIndex, rowIndex);
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

            DataGridViewCheckBoxXColumn oc = OwningColumn as DataGridViewCheckBoxXColumn;

            if (oc != null)
            {
                oc.ActiveRowIndex = -1;

                if (_MouseEntered == true)
                {
                    _MouseEntered = false;

                    oc.CheckBoxX.CheckBoxItem.InternalMouseLeave();
                }

                RefreshCell(ColumnIndex, rowIndex);
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

            DataGridViewCheckBoxXColumn oc = OwningColumn as DataGridViewCheckBoxXColumn;

            if (oc != null)
            {
                if (this.ReadOnly == true || oc.ReadOnly == true || DataGridView.ReadOnly == true)
                    return;

                Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                cellBounds.X = 0;
                cellBounds.Y = 0;

                DataGridViewCellStyle cellStyle =
                    DataGridView.Columns[e.ColumnIndex].DefaultCellStyle;

                Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
                Point pt = new Point(e.X - r.X, e.Y - r.Y);

                if (oc.CheckBoxX.CheckBoxItem.DisplayRectangle.Contains(pt) == true)
                {
                    if (_MouseEntered == false)
                    {
                        _MouseEntered = true;
                        oc.CheckBoxX.CheckBoxItem.InternalMouseEnter();
                    }
                }
                else
                {
                    if (_MouseEntered == true)
                    {
                        _MouseEntered = false;
                        oc.CheckBoxX.CheckBoxItem.InternalMouseLeave();
                    }
                }

                oc.CheckBoxX.CheckBoxItem.InternalMouseMove(
                    new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                RefreshCell(e.ColumnIndex, e.RowIndex);
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

            if (_MouseEntered == true)
            {
                DataGridViewCheckBoxXColumn oc = OwningColumn as DataGridViewCheckBoxXColumn;

                if (oc != null && !oc.ReadOnly)
                {
                    oc.DownRowIndex = e.RowIndex;

                    Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                    cellBounds.X = 0;
                    cellBounds.Y = 0;

                    DataGridViewCellStyle cellStyle =
                        DataGridView.Columns[e.ColumnIndex].DefaultCellStyle;

                    Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
                    Point pt = new Point(e.X - r.X, e.Y - r.Y);

                    if (oc.CheckBoxX.CheckBoxItem.DisplayRectangle.Contains(pt) == true)
                    {
                        oc.CheckBoxX.CheckValue = GetValue(e.RowIndex);

                        oc.CheckBoxX.CheckBoxItem.InternalMouseDown(
                            new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                        RefreshCell(e.ColumnIndex, e.RowIndex);
                    }
                }
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

            DataGridViewCheckBoxXColumn oc = OwningColumn as DataGridViewCheckBoxXColumn;

            if (oc != null && !oc.ReadOnly)
            {
                Rectangle cellBounds = DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                cellBounds.X = 0;
                cellBounds.Y = 0;

                DataGridViewCellStyle cellStyle =
                    DataGridView.Columns[e.ColumnIndex].DefaultCellStyle;

                Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
                Point pt = new Point(e.X - r.X, e.Y - r.Y);

                oc.CheckBoxX.CheckBoxItem.InternalMouseUp(
                    new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                if (e.RowIndex == oc.DownRowIndex)
                {
                    if (r.Contains(e.Location) == true)
                        Value = oc.CheckBoxX.CheckValue;

                    RefreshCell(e.ColumnIndex, e.RowIndex);
                }

                oc.DownRowIndex = -1;
            }
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
            DataGridViewCheckBoxXColumn oc = (DataGridViewCheckBoxXColumn)OwningColumn;

            cellBounds.Height--;

            if (Selected == false)
                cellBounds.Width -= (oc.DividerWidth + 1);

            return (cellBounds);
        }

        #endregion

        #region GetContentBounds

        /// <summary>
        /// Gets the Content bounds for the given cell
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        private Rectangle GetContentBounds(Rectangle cellBounds)
        {
            DataGridViewCheckBoxXColumn oc = OwningColumn as DataGridViewCheckBoxXColumn;

            if (oc != null)
            {
                if (oc.CheckBoxX.Parent == null)
                {
                    Form form = oc.DataGridView.FindForm();

                    if (form != null)
                        oc.CheckBoxX.Parent = form;
                }

                oc.CheckBoxX.CheckBoxItem.RecalcSize();

                cellBounds.Width = Math.Min(oc.CheckBoxX.CheckBoxItem.WidthInternal, cellBounds.Width);
                cellBounds.Width -= (oc.DividerWidth + 3);
                cellBounds.Height = oc.CheckBoxX.CheckBoxItem.HeightInternal;
            }

            return (cellBounds);
        }

        #endregion

        #region RefreshCell

        /// <summary>
        /// Initiates the refresh of the cell
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        private void RefreshCell(int columnIndex, int rowIndex)
        {
            DataGridView.InvalidateCell(columnIndex, rowIndex);
        }

        #endregion
    }
}
