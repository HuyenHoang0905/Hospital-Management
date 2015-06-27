using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewComboBoxExCell : DataGridViewTextBoxCell
    {
        #region Public properties

        #region EditType

        /// <summary>
        /// Gets the Type of the editing control associated with the cell
        /// </summary>
        public override Type EditType
        {
            get { return (typeof(DataGridViewComboBoxExEditingControl)); }
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

            DataGridViewComboBoxExEditingControl ctl =
                (DataGridViewComboBoxExEditingControl) DataGridView.EditingControl;

            DataGridViewComboBoxExColumn oc = (DataGridViewComboBoxExColumn) OwningColumn;
            ComboBoxEx cb = oc.ComboBoxEx;

            ctl.AutoCompleteCustomSource = cb.AutoCompleteCustomSource;
            ctl.AutoCompleteMode = cb.AutoCompleteMode;
            ctl.AutoCompleteSource = cb.AutoCompleteSource;
            ctl.DrawMode = cb.DrawMode;
            ctl.DropDownHeight = cb.DropDownHeight;
            ctl.DropDownStyle = cb.DropDownStyle;
            ctl.Enabled = cb.Enabled;
            ctl.FlatStyle = cb.FlatStyle;
            ctl.FocusCuesEnabled = cb.FocusCuesEnabled;
            ctl.FormatString = cb.FormatString;
            ctl.FormattingEnabled = cb.FormattingEnabled;
            ctl.Images = cb.Images;
            ctl.ImeMode = cb.ImeMode;
            ctl.IntegralHeight = cb.IntegralHeight;
            ctl.IsStandalone = cb.IsStandalone;
            ctl.ItemHeight = cb.ItemHeight;
            ctl.MaxDropDownItems = cb.MaxDropDownItems;
            ctl.MaxLength = cb.MaxLength;
            ctl.RenderBorder = cb.RenderBorder;
            ctl.RightToLeft = cb.RightToLeft;
            ctl.Style = cb.Style;
            ctl.WatermarkBehavior = cb.WatermarkBehavior;
            ctl.WatermarkColor = cb.WatermarkColor;
            ctl.WatermarkEnabled = cb.WatermarkEnabled;
            ctl.WatermarkFont = cb.WatermarkFont;
            ctl.WatermarkText = cb.WatermarkText;

            ctl.DataSource = cb.DataSource;

            if (ctl.DataSource is List<string> == false)
                ctl.ValueMember = cb.ValueMember;      
    
            ctl.DisplayMember = cb.DisplayMember;

            if (ctl.DataSource == null)
            {
                object[] items = new object[cb.Items.Count];
                cb.Items.CopyTo(items, 0);

                ctl.Items.Clear();
                ctl.Items.AddRange(items);

                ctl.Text = Convert.ToString(initialFormattedValue);
            }
            else
            {
                ctl.Text = cb.Text;
            }

            ctl.Click += Click;
            ctl.DrawItem += DrawItem;
            ctl.DropDownChange += DropDownChange;
            ctl.MeasureItem += MeasureItem;
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
                DataGridViewComboBoxExEditingControl cb =
                    DataGridView.EditingControl as DataGridViewComboBoxExEditingControl;

                if (cb != null)
                {
                    cb.Click -= Click;
                    cb.DrawItem -= DrawItem;
                    cb.DropDownChange -= DropDownChange;
                    cb.MeasureItem -= MeasureItem;
                }
            }

            base.DetachEditingControl();
        }

        #endregion

        #region Event processing

        #region Click

        /// <summary>
        /// Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Click(object sender, EventArgs e)
        {
            ((DataGridViewComboBoxExColumn)OwningColumn).DoComboBoxEx_Click(sender, e);
        }

        #endregion

        #region DrawItem

        /// <summary>
        /// DrawItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DrawItem(object sender, DrawItemEventArgs e)
        {
            ((DataGridViewComboBoxExColumn)OwningColumn).DoComboBoxEx_DrawItem(sender, e);
        }

        #endregion

        #region DropDownChange

        /// <summary>
        /// DropDownChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="expanded"></param>
        void DropDownChange(object sender, bool expanded)
        {
            ((DataGridViewComboBoxExColumn)OwningColumn).DoComboBoxEx_DropDownChange(
                sender, new DropDownChangeEventArgs(RowIndex, ColumnIndex, expanded));
        }

        #endregion

        #region MeasureItem

        /// <summary>
        /// MeasureItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MeasureItem(object sender, MeasureItemEventArgs e)
        {
            ((DataGridViewComboBoxExColumn)OwningColumn).DoComboBoxEx_MeasureItem(sender, e);
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

            if (r.Height < editingControlBounds.Height)
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

            DataGridViewComboBoxExColumn oc = OwningColumn as DataGridViewComboBoxExColumn;

            if (oc != null)
            {
                ComboBoxEx cb = oc.ComboBoxEx;

                preferredSize.Height = cb.Height;

                if (constraintSize.Width == 0)
                {
                    Rectangle r = DataGridView.GetCellDisplayRectangle(ColumnIndex, rowIndex, true);
                    preferredSize.Width += cb.GetThumbRect(r).Width;
                }
            }

            return (preferredSize);
        }

        #endregion

        #endregion

        #region ParseFormattedValue

        /// <summary>
        /// ParseFormattedValue
        /// </summary>
        /// <param name="formattedValue"></param>
        /// <param name="cellStyle"></param>
        /// <param name="formattedValueTypeConverter"></param>
        /// <param name="valueTypeConverter"></param>
        /// <returns></returns>
        public override object ParseFormattedValue(object formattedValue,
            DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
        {
            DataGridViewComboBoxExColumn oc = (DataGridViewComboBoxExColumn)OwningColumn;
            ComboBoxEx di = oc.ComboBoxEx;

            if (di.DataSource != null)
                return (GetDataSourceValue(oc, di, formattedValue));

            return (base.ParseFormattedValue(formattedValue,
                cellStyle, formattedValueTypeConverter, valueTypeConverter));
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
                        DataGridViewComboBoxExColumn oc = (DataGridViewComboBoxExColumn)OwningColumn;
                        Bitmap bm = oc.GetCellBitmap(cellBounds);

                        if (bm != null)
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                            {
                                PaintCellBackground(g, cellStyle, rBk);
                                PaintCellContent(g, cellBounds, rowIndex, value, cellStyle, paintParts, bm);

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

        private BindingContext _BindingContext = new BindingContext();
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
            DataGridViewComboBoxExColumn oc = (DataGridViewComboBoxExColumn)OwningColumn;
            ComboBoxEx di = oc.ComboBoxEx;

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

                    if (di.DataSource == null)
                    {
                        di.DisplayMember = "Text";
                        di.ValueMember = null;
                    }
                    else
                    {
                        value = GetDataSourceDisplayValue(oc, di, value);
                        di.BindingContext = _BindingContext;
                    }

                    di.Font = cellStyle.Font;
                    di.ForeColor = cellStyle.ForeColor;
                    di.BackColor = cellStyle.BackColor;
                    
                    di.Text = (value == Convert.DBNull || string.IsNullOrEmpty(Convert.ToString(value))) ? null : Convert.ToString(value);

                    oc.OnBeforeCellPaint(rowIndex, ColumnIndex);

                    Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);

                    if (oc.DisplayControlForCurrentCellOnly == false)
                        DrawControl(di, r, bm, g, oc);
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
        /// <param name="bm"></param>
        /// <param name="g"></param>
        /// <param name="oc"></param>
        private void DrawControl(ComboBoxEx di, Rectangle r,
            Bitmap bm, Graphics g, DataGridViewComboBoxExColumn oc)
        {
            Rectangle t = di.GetThumbRect(new Rectangle(0, 0, r.Width, r.Height));

            if (t.IsEmpty == false)
            {
                // Work around Windows XP and Windows DropDownList 
                // DrawToBitmap problems

                if (MustRenderVisibleControl(di.DropDownStyle) == true)
                {
                    di.Location = oc.DataGridView.Location;

                    if (di.Parent == null)
                    {
                        Form form = oc.DataGridView.FindForm();

                        if (form != null)
                            di.Parent = form;
                    }

                    di.SendToBack();
                    di.Visible = true;
                }

                using (Bitmap bm2 = new Bitmap(bm))
                {
                    di.Bounds = r;
                    di.DrawToBitmap(bm2, r);

                    t.X += r.X;
                    t.Y += r.Y;

                    g.DrawImage(bm2, t, t, GraphicsUnit.Pixel);

                    if (t.Left < r.Right)
                        r.Width -= (r.Right - t.Left - 3);
                }

                di.Visible = false;
            }

            DrawText(di, r, g);
        }

        #endregion

        #region DrawText

        /// <summary>
        /// DrawText
        /// </summary>
        /// <param name="di"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        private void DrawText(ComboBoxEx di, Rectangle r, Graphics g)
        {
            r.Inflate(-2, 0);

            TextDrawing.DrawString(g, di.Text, di.Font, di.ForeColor, r,
                eTextFormat.VerticalCenter | eTextFormat.EndEllipsis | eTextFormat.NoPrefix);
        }

        #endregion

        #region GetDataSource

        #region GetDataSourceDisplayValue

        private object GetDataSourceDisplayValue(
            DataGridViewComboBoxExColumn oc, ComboBoxEx di, object key)
        {
            if (key != null &&
                string.IsNullOrEmpty(di.ValueMember) == false &&
                string.IsNullOrEmpty(di.DisplayMember) == false)
            {
                CurrencyManager cm = GetCurrencyManager(oc, di);

                if (cm != null)
                {
                    string t = key.ToString();

                    for (int i = 0; i < cm.List.Count; i++)
                    {
                        if (cm.List[i] is DataRowView)
                        {
                            DataRowView drView = (DataRowView) cm.List[i];

                            string s = drView[di.ValueMember].ToString();

                            if (t.Equals(s) == true)
                                return (drView[di.DisplayMember]);
                        }
                        else if (cm.List[i] is string)
                        {
                            if (t.Equals(cm.List[i]) == true)
                                return (key);
                        }
                        else
                        {
                            object o = cm.List[i];
                            object value = o.GetType().GetProperty(di.ValueMember).GetValue(o, null);

                            if (value is string)
                            {
                                if (t.Equals(value) == true)
                                {
                                    value = o.GetType().GetProperty(di.DisplayMember).GetValue(o, null);

                                    return (value);
                                }
                            }
                        }
                    }
                }
            }

            return ((key == Convert.DBNull) ? "" : key);
        }

        #endregion

        #region GetDataSourceValue

        private object GetDataSourceValue(
            DataGridViewComboBoxExColumn oc, ComboBoxEx di, object key)
        {
            if (key != null &&
                string.IsNullOrEmpty(di.ValueMember) == false &&
                string.IsNullOrEmpty(di.DisplayMember) == false)
            {
                CurrencyManager cm = GetCurrencyManager(oc, di);

                if (cm != null)
                {
                    string t = key.ToString();

                    for (int i = 0; i < cm.List.Count; i++)
                    {
                        if (cm.List[i] is DataRowView)
                        {
                            DataRowView drView = (DataRowView) cm.List[i];

                            string s = drView[di.DisplayMember].ToString();

                            if (t.Equals(s) == true)
                                return (drView[di.ValueMember]);
                        }
                        else if (cm.List[i] is string)
                        {
                            if (t.Equals(cm.List[i]) == true)
                                return (key);
                        }
                        else
                        {
                            object o = cm.List[i];
                            object value = o.GetType().GetProperty(di.DisplayMember).GetValue(o, null);

                            if (value is string)
                            {
                                if (t.Equals(value) == true)
                                {
                                    value = o.GetType().GetProperty(di.ValueMember).GetValue(o, null);

                                    return (value);
                                }
                            }
                        }
                    }
                }
            }

            return (key);
        }

        #endregion

        #region GetCurrencyManager

        private CurrencyManager GetCurrencyManager(
            DataGridViewComboBoxExColumn oc, ComboBoxEx di)
        {
            if (oc.CurrencyManager == null)
            {
                if (DataGridView != null && DataGridView.BindingContext != null &&
                    di.DataSource != null && di.DataSource != Convert.DBNull)
                {
                    oc.CurrencyManager = (CurrencyManager) DataGridView.BindingContext[di.DataSource];
                }
            }

            return (oc.CurrencyManager);
        }

        #endregion

        #endregion

        #region MustRenderVisibleControl

        /// <summary>
        /// MustRenderVisibleControl
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private bool MustRenderVisibleControl(ComboBoxStyle style)
        {
            if (style == ComboBoxStyle.DropDownList)
                return (true);

            OperatingSystem osInfo = Environment.OSVersion;

            return (osInfo.Platform == PlatformID.Win32Windows ||
                (osInfo.Platform == PlatformID.Win32NT && osInfo.Version.Major < 6));

        }

        #endregion

        #endregion

        #endregion

        #region Mouse support

        #region OnMouseEnter

        /// <summary>
        /// OnMouseEnter
        /// </summary>
        /// <param name="rowIndex"></param>
        protected override void OnMouseEnter(int rowIndex)
        {
            DataGridViewComboBoxExColumn oc = (DataGridViewComboBoxExColumn) OwningColumn;
            ComboBoxEx di = oc.ComboBoxEx;

            if ((uint)RowIndex < DataGridView.Rows.Count)
            {
                if (di.DataSource != null)
                    this.ToolTipText = Convert.ToString(GetDataSourceDisplayValue(oc, di, Value));
                else
                    this.ToolTipText = Convert.ToString(Value);
            }

            base.OnMouseEnter(rowIndex);
        }

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
            DataGridViewComboBoxExColumn oc = (DataGridViewComboBoxExColumn)OwningColumn;

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
            DataGridViewComboBoxExColumn oc = (DataGridViewComboBoxExColumn)OwningColumn;

            Size size = oc.ComboBoxEx.Size;

            cellBounds.Location = new Point(1, 1);

            cellBounds.Width -= oc.DividerWidth;
            cellBounds.Height = Math.Min(size.Height, cellBounds.Height);

            return (cellBounds);
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
