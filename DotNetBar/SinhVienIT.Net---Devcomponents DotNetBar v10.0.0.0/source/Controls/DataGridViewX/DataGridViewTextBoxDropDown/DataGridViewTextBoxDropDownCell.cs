using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewTextBoxDropDownCell : DataGridViewTextBoxCell
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        #region Public properties

        #region EditType

        /// <summary>
        /// Gets the Type of the editing control associated with the cell
        /// </summary>
        public override Type EditType
        {
            get { return (typeof(DataGridViewTextBoxDropDownEditingControl)); }
        }

        #endregion

        #endregion

        #region InitializeEditingControl

        /// <summary>
        /// InitializeEditingControl
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="initialFormattedValue"></param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex,
            object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            DetachEditingControl();

            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            DataGridViewTextBoxDropDownEditingControl ctl =
                (DataGridViewTextBoxDropDownEditingControl)DataGridView.EditingControl;

            DataGridViewTextBoxDropDownColumn oc = (DataGridViewTextBoxDropDownColumn)OwningColumn;
            TextBoxDropDown tb = oc.TextBoxDropDown;

            ctl.AutoCompleteCustomSource = tb.AutoCompleteCustomSource;
            ctl.AutoCompleteMode = tb.AutoCompleteMode;
            ctl.AutoCompleteSource = tb.AutoCompleteSource;
            ctl.BackColor = tb.BackColor;
            ctl.CharacterCasing = tb.CharacterCasing;
            ctl.DropDownControl = tb.DropDownControl;
            ctl.Enabled = tb.Enabled;
            ctl.FocusHighlightColor = tb.FocusHighlightColor;
            ctl.FocusHighlightEnabled = tb.FocusHighlightEnabled;
            ctl.ForeColor = tb.ForeColor;
            ctl.HideSelection = tb.HideSelection;
            ctl.ImeMode = tb.ImeMode;
            ctl.MaxLength = tb.MaxLength;
            ctl.PasswordChar = tb.PasswordChar;
            ctl.RightToLeft = tb.RightToLeft;
            ctl.TextAlign = GetTextAlignment(dataGridViewCellStyle.Alignment);
            ctl.UseSystemPasswordChar = tb.UseSystemPasswordChar;
            ctl.WatermarkBehavior = tb.WatermarkBehavior;
            ctl.WatermarkColor = tb.WatermarkColor;
            ctl.WatermarkEnabled = tb.WatermarkEnabled;
            ctl.WatermarkFont = tb.WatermarkFont;
            ctl.WatermarkText = tb.WatermarkText;

            ctl.TextBox.Multiline = (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True);

            ctl.BackgroundStyle.ApplyStyle(tb.BackgroundStyle);
            ctl.BackgroundStyle.Class = tb.BackgroundStyle.Class;

            tb.ButtonClear.CopyToItem(ctl.ButtonClear);
            tb.ButtonCustom.CopyToItem(ctl.ButtonCustom);
            tb.ButtonCustom2.CopyToItem(ctl.ButtonCustom2);
            tb.ButtonDropDown.CopyToItem(ctl.ButtonDropDown);

            ctl.ButtonClearClick += ButtonClearClick;
            ctl.ButtonCustomClick += ButtonCustomClick;
            ctl.ButtonCustom2Click += ButtonCustom2Click;
            ctl.ButtonDropDownClick += ButtonDropDownClick;
            ctl.KeyDown += KeyDown;

            ctl.Text = (initialFormattedValue != null && initialFormattedValue != Convert.DBNull
                ? Convert.ToString(initialFormattedValue) : "");
        }

        #endregion

        #region DetachEditingControl

        /// <summary>
        /// DetachEditingControl
        /// </summary>
        public override void DetachEditingControl()
        {
            if (DataGridView != null && DataGridView.EditingControl != null)
            {
                DataGridViewTextBoxDropDownEditingControl di =
                    DataGridView.EditingControl as DataGridViewTextBoxDropDownEditingControl;

                if (di != null)
                {
                    di.ButtonClearClick -= ButtonClearClick;
                    di.ButtonCustomClick -= ButtonCustomClick;
                    di.ButtonCustom2Click -= ButtonCustom2Click;
                    di.ButtonDropDownClick -= ButtonDropDownClick;
                    di.KeyDown -= KeyDown;
                }
            }

            base.DetachEditingControl();
        }

        #endregion

        #region Event processing

        #region ButtonClearClick

        /// <summary>
        /// ButtonClearClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonClearClick(object sender, CancelEventArgs e)
        {
            ((DataGridViewTextBoxDropDownColumn)OwningColumn).DoButtonClearClick(sender, e);
        }

        #endregion

        #region ButtonCustomClick

        /// <summary>
        /// ButtonCustomClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonCustomClick(object sender, EventArgs e)
        {
            ((DataGridViewTextBoxDropDownColumn)OwningColumn).DoButtonCustomClick(sender, e);
        }

        #endregion

        #region ButtonCustom2Click

        /// <summary>
        /// ButtonCustom2Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonCustom2Click(object sender, EventArgs e)
        {
            ((DataGridViewTextBoxDropDownColumn)OwningColumn).DoButtonCustom2Click(sender, e);
        }

        #endregion

        #region ButtonDropDownClick

        /// <summary>
        /// ButtonDropDownClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonDropDownClick(object sender, CancelEventArgs e)
        {
            ((DataGridViewTextBoxDropDownColumn)OwningColumn).DoButtonDropDownClick(sender, e);
        }

        #endregion

        #region KeyDown

        /// <summary>
        /// KeyDown routine forwards all DataGridView sent keys to
        /// the underlying focusable control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyDown(object sender, KeyEventArgs e)
        {
            if (DataGridView != null && DataGridView.EditingControl != null)
            {
                DataGridViewTextBoxDropDownEditingControl di =
                    DataGridView.EditingControl as DataGridViewTextBoxDropDownEditingControl;

                if (di != null)
                    PostMessage(di.TextBox.Handle, 256, (int) e.KeyCode, 1);
            }
        }

        #endregion

        #endregion

        #region PositionEditingControl

        /// <summary>
        /// PositionEditingControl
        /// </summary>
        /// <param name="setLocation"></param>
        /// <param name="setSize"></param>
        /// <param name="cellBounds"></param>
        /// <param name="cellClip"></param>
        /// <param name="cellStyle"></param>
        /// <param name="singleVerticalBorderAdded"></param>
        /// <param name="singleHorizontalBorderAdded"></param>
        /// <param name="isFirstDisplayedColumn"></param>
        /// <param name="isFirstDisplayedRow"></param>
        public override void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds,
            Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded,
            bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
        {
            Rectangle editingControlBounds =
                PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded,
                                     singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);

            editingControlBounds = GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);

            DataGridView.EditingControl.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
            DataGridView.EditingControl.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
        }

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

            Rectangle r = GetCellBounds(editingControlBounds);

            if (cellStyle.WrapMode != DataGridViewTriState.True)
            {
                if (r.Height < editingControlBounds.Height)
                {
                    switch (cellStyle.Alignment)
                    {
                        case DataGridViewContentAlignment.MiddleLeft:
                        case DataGridViewContentAlignment.MiddleCenter:
                        case DataGridViewContentAlignment.MiddleRight:
                            editingControlBounds.Y += (editingControlBounds.Height - r.Height)/2;
                            break;

                        case DataGridViewContentAlignment.BottomLeft:
                        case DataGridViewContentAlignment.BottomCenter:
                        case DataGridViewContentAlignment.BottomRight:
                            editingControlBounds.Y += (editingControlBounds.Height - r.Height);
                            break;
                    }
                }

                editingControlBounds.Height = Math.Max(1, r.Height);
            }

            editingControlBounds.Width = Math.Max(1, editingControlBounds.Width);

            return (editingControlBounds);
        }

        #endregion

        #region GetPreferredSize

        ///// <summary>
        ///// GetPreferredSize
        ///// </summary>
        ///// <param name="graphics"></param>
        ///// <param name="cellStyle"></param>
        ///// <param name="rowIndex"></param>
        ///// <param name="constraintSize"></param>
        ///// <returns></returns>
        //protected override Size GetPreferredSize(Graphics graphics,
        //       DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        //{
        //    if (DataGridView == null)
        //        return (new Size(-1, -1));

        //    DataGridViewTextBoxDropDownColumn oc = (DataGridViewTextBoxDropDownColumn)OwningColumn;

        //    Size size = oc.TextBoxDropDown.PreferredSize;
        //    size.Height += 3;

        //    return (size);
        //}

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
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value, object formattedValue,
            string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
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
                        DataGridViewTextBoxDropDownColumn oc = (DataGridViewTextBoxDropDownColumn)OwningColumn;
                        Bitmap bm = oc.GetCellBitmap(cellBounds);

                        if (bm != null)
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                            {
                                PaintCellBackground(g, cellStyle, rBk);
                                PaintCellContent(g, cellBounds, rowIndex, formattedValue, cellStyle, paintParts, bm);

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

        #region PaintCellBackground

        /// <summary>
        /// Paints the cell background
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellStyle"></param>
        /// <param name="rBack"></param>
        private void PaintCellBackground(Graphics g,
            DataGridViewCellStyle cellStyle, Rectangle rBack)
        {
            Rectangle r = rBack;
            r.Location = new Point(0, 0);

            DataGridViewX dx = DataGridView as DataGridViewX;

            if (dx != null && dx.Enabled == true && Selected == true)
            {
                Office2007ButtonItemPainter.PaintBackground(g, dx.ButtonStateColorTable,
                    r, RoundRectangleShapeDescriptor.RectangleShape, false, false);
            }
            else
            {
                using (Brush br = new SolidBrush(cellStyle.BackColor))
                    g.FillRectangle(br, r);
            }
        }

        #endregion

        #region PaintCellContent

        /// <summary>
        /// Paints the cell content
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="paintParts"></param>
        /// <param name="bm"></param>
        private void PaintCellContent(Graphics g, Rectangle cellBounds, int rowIndex, object value,
            DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Bitmap bm)
        {
            DataGridViewTextBoxDropDownColumn oc = (DataGridViewTextBoxDropDownColumn)OwningColumn;
            TextBoxDropDown di = oc.TextBoxDropDown;

            Point ptCurrentCell = DataGridView.CurrentCellAddress;
            bool cellCurrent = ptCurrentCell.X == ColumnIndex && ptCurrentCell.Y == rowIndex;
            bool cellEdited = cellCurrent && DataGridView.EditingControl != null;

            // If the cell is in editing mode, there is nothing else to paint

            if (cellEdited == false && rowIndex < DataGridView.RowCount)
            {
                if (PartsSet(paintParts, DataGridViewPaintParts.ContentForeground))
                {
                    cellBounds.X = 0;
                    cellBounds.Y = 0;
                    cellBounds.Width -= (oc.DividerWidth + 1);
                    cellBounds.Height -= 1;

                    di.Font = cellStyle.Font;
                    di.ForeColor = cellStyle.ForeColor;
                    di.BackColor = cellStyle.BackColor;

                    di.TextAlign = GetTextAlignment(cellStyle.Alignment);
                    di.Text = GetValue(value);

                    oc.OnBeforeCellPaint(rowIndex, ColumnIndex);

                    Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);

                    if (oc.DisplayControlForCurrentCellOnly == false)
                        DrawControl(di, cellStyle, r, bm, g);
                    else
                        DrawText(di, cellStyle, r, g);
                }
            }
        }

        #region DrawControl

        /// <summary>
        /// DrawControl
        /// </summary>
        /// <param name="di"></param>
        /// <param name="cellStyle"></param>
        /// <param name="r"></param>
        /// <param name="bm"></param>
        /// <param name="g"></param>
        private void DrawControl(TextBoxDropDown di, DataGridViewCellStyle cellStyle, Rectangle r, Bitmap bm, Graphics g)
        {
            if (di.ButtonGroup.Items.Count > 0)
            {
                using (Bitmap bm2 = new Bitmap(bm))
                {
                    di.Bounds = r;
                    di.DrawToBitmap(bm2, r);

                    foreach (VisualItem item in di.ButtonGroup.Items)
                    {
                        if (item.Visible == true)
                        {
                            Rectangle t = item.RenderBounds;
                            t.X += r.X;
                            t.Y += r.Y;

                            g.DrawImage(bm2, t, t, GraphicsUnit.Pixel);

                            if (t.Left < r.Right)
                                r.Width -= (r.Right - t.Left - 1);
                        }
                    }
                }
            }

            DrawText(di, cellStyle, r, g);
        }

        #endregion

        #region DrawText

        /// <summary>
        /// DrawText
        /// </summary>
        /// <param name="di"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="cellStyle"></param>
        private void DrawText(TextBoxDropDown di, DataGridViewCellStyle cellStyle, Rectangle r, Graphics g)
        {
            r.Inflate(-2, 0);

            eTextFormat tf = eTextFormat.Default | eTextFormat.NoPrefix;

            switch (di.TextAlign)
            {
                case HorizontalAlignment.Center:
                    tf |= eTextFormat.HorizontalCenter;
                    break;

                case HorizontalAlignment.Right:
                    tf |= eTextFormat.Right;
                    break;
            }

            if (cellStyle.WrapMode == DataGridViewTriState.True)
                tf |= eTextFormat.WordBreak;

            switch (cellStyle.Alignment)
            {
                case DataGridViewContentAlignment.TopLeft:
                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.TopRight:
                    tf |= eTextFormat.Top;
                    break;

                case DataGridViewContentAlignment.BottomLeft:
                case DataGridViewContentAlignment.BottomCenter:
                case DataGridViewContentAlignment.BottomRight:
                    tf |= eTextFormat.Bottom;
                    break;

                default:
                    tf |= eTextFormat.VerticalCenter;
                    break;
            }

            TextDrawing.DrawString(g, di.Text, di.Font, di.ForeColor, r, tf);
        }

        #endregion

        #endregion

        #endregion

        #region GetBackBounds

        /// <summary>
        /// Gets the background bounds for the given cell
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        private Rectangle GetBackBounds(Rectangle cellBounds)
        {
            DataGridViewTextBoxDropDownColumn oc = (DataGridViewTextBoxDropDownColumn)OwningColumn;

            cellBounds.Height--;

            if (Selected == false)
                cellBounds.Width -= (oc.DividerWidth + 1);

            return (cellBounds);
        }

        #endregion

        #region GetCellBounds

        /// <summary>
        /// Gets the button bounds for the given cell
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        private Rectangle GetCellBounds(Rectangle cellBounds)
        {
            DataGridViewTextBoxDropDownColumn oc = (DataGridViewTextBoxDropDownColumn)OwningColumn;

            Size size = oc.TextBoxDropDown.PreferredSize;

            cellBounds.Location = new Point(1, 1);

            cellBounds.Width -= oc.DividerWidth;
            cellBounds.Height = size.Height - 1;

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
        /// GetTextAlignment
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        private HorizontalAlignment GetTextAlignment(DataGridViewContentAlignment alignment)
        {
            switch (alignment)
            {
                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    return (HorizontalAlignment.Center);

                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    return (HorizontalAlignment.Right);

                default:
                    return (HorizontalAlignment.Left);
            }
        }

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
    }
}
