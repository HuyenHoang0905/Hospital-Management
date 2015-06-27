using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewDoubleInputCell : DataGridViewTextBoxCell
    {
        #region Public properties

        #region EditType

        /// <summary>
        /// Gets the Type of the editing control associated with the cell
        /// </summary>
        public override Type EditType
        {
            get { return (typeof(DataGridViewDoubleInputEditingControl)); }
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

            DataGridViewDoubleInputEditingControl ctl =
                (DataGridViewDoubleInputEditingControl)DataGridView.EditingControl;

            DataGridViewDoubleInputColumn oc = OwningColumn as DataGridViewDoubleInputColumn;

            if (oc != null)
            {
                DoubleInput di = oc.DoubleInput;

                ctl.Value = GetValue(initialFormattedValue);

                ctl.DisplayFormat = di.DisplayFormat;
                ctl.Enabled = di.Enabled;
                ctl.Increment = di.Increment;
                ctl.LockUpdateChecked = di.LockUpdateChecked;
                ctl.MaxValue = di.MaxValue;
                ctl.MinValue = di.MinValue;
                ctl.ShowCheckBox = di.ShowCheckBox;
                ctl.ShowUpDown = di.ShowUpDown;

                ctl.BackgroundStyle.ApplyStyle(di.BackgroundStyle);
                ctl.BackgroundStyle.Class = di.BackgroundStyle.Class;

                ctl.InputHorizontalAlignment = GetHorizontalAlignment(dataGridViewCellStyle.Alignment);

                di.ButtonClear.CopyToItem(ctl.ButtonClear);
                di.ButtonCustom.CopyToItem(ctl.ButtonCustom);
                di.ButtonCustom2.CopyToItem(ctl.ButtonCustom2);
                di.ButtonDropDown.CopyToItem(ctl.ButtonDropDown);
                di.ButtonFreeText.CopyToItem(ctl.ButtonFreeText);

                ctl.ButtonClearClick += ButtonClearClick;
                ctl.ButtonCustomClick += ButtonCustomClick;
                ctl.ButtonCustom2Click += ButtonCustom2Click;
                ctl.ButtonDropDownClick += ButtonDropDownClick;
                ctl.ButtonFreeTextClick += ButtonFreeTextClick;
                ctl.ConvertFreeTextEntry += ConvertFreeTextEntry;

                ctl.EditCancelled = false;
            }
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
                DataGridViewDoubleInputEditingControl di =
                    DataGridView.EditingControl as DataGridViewDoubleInputEditingControl;

                if (di != null)
                {
                    di.ButtonClearClick -= ButtonClearClick;
                    di.ButtonDropDownClick -= ButtonDropDownClick;
                    di.ButtonFreeTextClick -= ButtonFreeTextClick;
                    di.ButtonCustomClick -= ButtonCustomClick;
                    di.ButtonCustom2Click -= ButtonCustom2Click;
                    di.ConvertFreeTextEntry -= ConvertFreeTextEntry;

                    if (di.EditCancelled == true)
                    {
                        di.EditCancelled = false;

                        SystemSounds.Beep.Play();
                    }
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
            ((DataGridViewDoubleInputColumn)OwningColumn).DoButtonClearClick(sender, e);
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
            ((DataGridViewDoubleInputColumn)OwningColumn).DoButtonCustomClick(sender, e);
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
            ((DataGridViewDoubleInputColumn)OwningColumn).DoButtonCustom2Click(sender, e);
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
            ((DataGridViewDoubleInputColumn)OwningColumn).DoButtonDropDownClick(sender, e);
        }

        #endregion

        #region ButtonFreeTextClick

        /// <summary>
        /// ButtonFreeTextClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonFreeTextClick(object sender, CancelEventArgs e)
        {
            ((DataGridViewDoubleInputColumn)OwningColumn).DoButtonFreeTextClick(sender, e);
        }

        #endregion

        #region ConvertFreeTextEntry

        /// <summary>
        /// ConvertFreeTextEntry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConvertFreeTextEntry(object sender, FreeTextEntryConversionEventArgs e)
        {
            ((DataGridViewDoubleInputColumn)OwningColumn).DoConvertFreeTextEntry(sender, e);
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

            if (cellStyle != null && r.Height < editingControlBounds.Height)
            {
                switch (cellStyle.Alignment)
                {
                    case DataGridViewContentAlignment.MiddleLeft:
                    case DataGridViewContentAlignment.MiddleCenter:
                    case DataGridViewContentAlignment.MiddleRight:
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height) / 2;
                        break;

                    case DataGridViewContentAlignment.BottomLeft:
                    case DataGridViewContentAlignment.BottomCenter:
                    case DataGridViewContentAlignment.BottomRight:
                        editingControlBounds.Y += (editingControlBounds.Height - r.Height);
                        break;
                }
            }

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

            Size preferredSize = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);

            DataGridViewDoubleInputColumn oc = OwningColumn as DataGridViewDoubleInputColumn;

            if (oc != null)
            {
                DoubleInput di = oc.DoubleInput;

                preferredSize.Height = di.Height;

                if (constraintSize.Width == 0)
                {
                    preferredSize.Width += GetImageWidth(di.ButtonClear);
                    preferredSize.Width += GetImageWidth(di.ButtonCustom);
                    preferredSize.Width += GetImageWidth(di.ButtonCustom2);
                    preferredSize.Width += GetImageWidth(di.ButtonDropDown);
                    preferredSize.Width += GetImageWidth(di.ButtonFreeText);

                    if (di.ShowUpDown == true)
                        preferredSize.Width += 16;

                    if (di.ShowCheckBox == true)
                        preferredSize.Width += 16;
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
            DataGridViewDoubleInputColumn oc = OwningColumn as DataGridViewDoubleInputColumn;

            if (oc != null)
            {
                DoubleInput di = oc.DoubleInput;
                di.Value = GetValue(value);

                return (di.Text);
            }

            return (base.GetFormattedValue(value, rowIndex,
                ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context));
        }

        #endregion

        #region GetImageWidth

        private int GetImageWidth(InputButtonSettings ibs)
        {
            if (ibs.Visible == true)
                return (ibs.Image != null ? ibs.Image.Width : 16);

            return (0);
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
                        DataGridViewDoubleInputColumn oc = (DataGridViewDoubleInputColumn)OwningColumn;
                        Bitmap bm = oc.GetCellBitmap(cellBounds);

                        if (bm != null)
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                            {
                                PaintCellBackground(g, cellStyle, rBk);
                                PaintCellContent(g, cellBounds, rowIndex, value, cellStyle, paintParts);

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
        private void PaintCellContent(Graphics g, Rectangle cellBounds, int rowIndex, object value,
            DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts)
        {
            DataGridViewDoubleInputColumn oc = (DataGridViewDoubleInputColumn)OwningColumn;
            DoubleInput di = oc.DoubleInput;

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
                    di.BackColor = Selected ? Color.Transparent : cellStyle.BackColor;

                    di.BackgroundStyle.TextColor = cellStyle.ForeColor;
                    di.BackgroundStyle.BackColor = Selected ? Color.Transparent : cellStyle.BackColor;

                    di.InputHorizontalAlignment = GetHorizontalAlignment(cellStyle.Alignment);

                    di.Value = GetValue(value);

                    oc.OnBeforeCellPaint(rowIndex, ColumnIndex);

                    Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);

                    if (oc.DisplayControlForCurrentCellOnly == false)
                        DrawControl(di, r, g);
                    else
                        DrawText(di, r, g);
                }
            }
        }

        #region DrawControl

        /// <summary>
        /// DrawControl
        /// </summary>
        /// <param name="di"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        private void DrawControl(DoubleInput di, Rectangle r, Graphics g)
        {
            GraphicsState gs = g.Save();

            try
            {
                g.TranslateTransform(r.X, r.Y);

                di.Width = r.Width;
                di.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
            }
            finally
            {
                g.Restore(gs);
            }
        }

        #endregion

        #region DrawText

        /// <summary>
        /// DrawText
        /// </summary>
        /// <param name="di"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        private void DrawText(DoubleInput di, Rectangle r, Graphics g)
        {
            r.Inflate(-2, 0);

            eTextFormat tf = eTextFormat.VerticalCenter;

            switch (di.InputHorizontalAlignment)
            {
                case eHorizontalAlignment.Center:
                    tf |= eTextFormat.HorizontalCenter;
                    break;

                case eHorizontalAlignment.Right:
                    tf |= eTextFormat.Right;
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
            DataGridViewDoubleInputColumn oc = (DataGridViewDoubleInputColumn)OwningColumn;

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
            DataGridViewDoubleInputColumn oc = (DataGridViewDoubleInputColumn)OwningColumn;

            Size size = oc.DoubleInput.Size;

            cellBounds.Location = new Point(1, 1);

            cellBounds.Width = Math.Min(size.Width, cellBounds.Width);
            cellBounds.Width -= oc.DividerWidth;
            cellBounds.Height = size.Height;

            return (cellBounds);
        }

        #endregion

        #region GetValue

        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double GetValue(object value)
        {
            if (value == Convert.DBNull ||
                (value is string && String.IsNullOrEmpty((string)value) == true))
            {
                return (0);
            }
            if (value is string)
            {
                string svalue = value.ToString();
                return Convert.ToDouble(svalue.Replace(" ", ""));
            }
            return (Convert.ToDouble(value));
        }

        #endregion

        #region GetHorizontalAlignment

        /// <summary>
        /// GetHorizontalAlignment
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        private eHorizontalAlignment GetHorizontalAlignment(DataGridViewContentAlignment alignment)
        {
            switch (alignment)
            {
                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    return (eHorizontalAlignment.Center);

                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    return (eHorizontalAlignment.Right);

                default:
                    return (eHorizontalAlignment.Left);
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
