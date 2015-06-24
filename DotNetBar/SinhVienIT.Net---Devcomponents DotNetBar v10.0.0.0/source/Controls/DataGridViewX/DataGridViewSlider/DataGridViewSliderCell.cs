using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    public class DataGridViewSliderCell : DataGridViewTextBoxCell
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

        #region ValueType

        /// <summary>
        /// Gets the type of the underlying data
        /// (i.e., the type of the cell's Value property)
        /// </summary>
        public override Type ValueType
        {
            get { return (base.ValueType ?? typeof(Int32)); }
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
            Size preferredSize = new Size(-1, -1);

            if (DataGridView != null)
            {
                DataGridViewSliderColumn oc = OwningColumn as DataGridViewSliderColumn;

                if (oc != null)
                {
                    Slider sc = oc.Slider;

                    preferredSize.Height = sc.Height;

                    if (constraintSize.Width == 0)
                    {
                        preferredSize.Width += 80;

                        if (sc.LabelVisible == true)
                            preferredSize.Width += sc.LabelWidth;
                    }
                }
            }

            return (preferredSize);
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
                        DataGridViewSliderColumn oc = (DataGridViewSliderColumn)OwningColumn;
                        Bitmap bm = oc.GetCellBitmap(cellBounds);

                        if (bm != null)
                        {
                            using (Graphics g = Graphics.FromImage(bm))
                            {
                                PaintSliderBackground(g, cellStyle, rBk);
                                PaintSliderContent(cellBounds, rowIndex, formattedValue, cellStyle, paintParts, g);

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

        #region PaintSliderBackground

        /// <summary>
        /// Paints the Slider background
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellStyle"></param>
        /// <param name="rBack"></param>
        private void PaintSliderBackground(Graphics g,
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

        #region PaintSliderContent

        /// <summary>
        /// Paints the Slider content
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <param name="paintParts"></param>
        /// <param name="g"></param>
        private void PaintSliderContent(Rectangle cellBounds,
            int rowIndex, object value, DataGridViewCellStyle cellStyle,
            DataGridViewPaintParts paintParts, Graphics g)
        {
            DataGridViewSliderColumn oc = (DataGridViewSliderColumn) OwningColumn;
            Slider slider = oc.Slider;

            _CellStyle = cellStyle;

            int saveValue = slider.Value;

            eSliderPart mouseOverPart = slider.SliderItem.MouseOverPart;
            eSliderPart mouseDownPart = slider.SliderItem.MouseDownPart;

            GraphicsState gs = g.Save();

            try
            {
                cellBounds.X = 0;
                cellBounds.Y = 0;
                cellBounds.Width -= (oc.DividerWidth + 1);
                cellBounds.Height -= 1;

                if (rowIndex != oc.ActiveRowIndex)
                {
                    slider.SliderItem.MouseOverPart = eSliderPart.None;
                    slider.SliderItem.MouseDownPart = eSliderPart.None;
                }

                slider.Font = cellStyle.Font;
                slider.ForeColor = cellStyle.ForeColor;
                slider.BackColor = Selected ? Color.Transparent : cellStyle.BackColor;

                if (rowIndex < DataGridView.RowCount)
                {
                    slider.Text = oc.Text;

                    if (rowIndex != oc.ActiveRowIndex)
                        slider.Value = GetSliderValue(value);

                    if (PartsSet(paintParts, DataGridViewPaintParts.ContentForeground))
                        oc.OnBeforeCellPaint(rowIndex, ColumnIndex);
                }
                else
                {
                    slider.Text = "";
                    slider.Value = GetSliderValue(value);
                }

                Rectangle r = GetAdjustedEditingControlBounds(cellBounds, cellStyle);

                g.TranslateTransform(r.X, r.Y);

                slider.CallBasePaintBackground = false;

                slider.Bounds = r;
                slider.InternalPaint(new PaintEventArgs(g, Rectangle.Empty));
            }
            finally
            {
                g.Restore(gs);

                slider.Value = saveValue;

                slider.SliderItem.MouseOverPart = mouseOverPart;
                slider.SliderItem.MouseDownPart = mouseDownPart;
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

            DoMouseEnter(rowIndex, true);
        }

        #region DoMouseEnter

        /// <summary>
        /// Process MouseEnter state
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="refresh"></param>
        private void DoMouseEnter(int rowIndex, bool refresh)
        {
            DataGridViewSliderColumn oc = (DataGridViewSliderColumn) OwningColumn;
            Slider slider = oc.Slider;

            // Work around an issue where we get notified of a mouse enter, but
            // we will fault if we try to get the Value associated with the cell

            if (Visible == true)
            {
                oc.ActiveRowIndex = rowIndex;

                slider.Value = GetSliderValue(Value);

                slider.SliderItem.InternalMouseEnter();

                if (refresh == true)
                    RefreshSlider(ColumnIndex, rowIndex);
            }
        }

        #endregion

        #endregion

        #region OnMouseLeave

        /// <summary>
        /// Processes MouseLeave events
        /// </summary>
        /// <param name="rowIndex"></param>
        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);

            DataGridViewSliderColumn oc = (DataGridViewSliderColumn)OwningColumn;
            Slider slider = oc.Slider;

            if (oc.ActiveRowIndex >= 0)
            {
                oc.ActiveRowIndex = -1;

                slider.SliderItem.InternalMouseLeave();

                Value = slider.Value;

                RefreshSlider(ColumnIndex, rowIndex);
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

            DataGridViewSliderColumn oc = OwningColumn as DataGridViewSliderColumn;

            if (oc != null)
            {
                // Work around an issue where we get notified of a mouse enter / move, but
                // we will fault if we try to get the Value associated with the cell

                if (oc.ActiveRowIndex == -1)
                    DoMouseEnter(e.RowIndex, false);

                if (oc.ActiveRowIndex >= 0)
                {
                    Point pt = CellAlignPoint(e.X, e.Y);

                    oc.Slider.SliderItem.InternalMouseMove(
                        new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                    RefreshSlider(e.ColumnIndex, e.RowIndex);
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

            DataGridViewSliderColumn oc = OwningColumn as DataGridViewSliderColumn;

            if (oc != null)
            {
                Point pt = CellAlignPoint(e.X, e.Y);

                oc.Slider.SliderItem.InternalMouseDown(
                    new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                RefreshSlider(e.ColumnIndex, e.RowIndex);
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

            DataGridViewSliderColumn oc = OwningColumn as DataGridViewSliderColumn;

            if (oc != null)
            {
                Point pt = CellAlignPoint(e.X, e.Y);

                oc.Slider.SliderItem.InternalMouseUp(
                    new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta));

                Value = oc.Slider.Value;

                RefreshSlider(e.ColumnIndex, e.RowIndex);
            }
        }

        #endregion

        #region CellAlignPoint

        /// <summary>
        /// CellAlignPoint
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point CellAlignPoint(int x, int y)
        {
            Rectangle r = GetAdjustedEditingControlBounds(ContentBounds, _CellStyle);

            return (new Point(x, y - r.Y));
        }

        #endregion

        #endregion

        #region GetSliderValue

        /// <summary>
        /// GetSliderValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal int GetSliderValue(object value)
        {
            if (value == Convert.DBNull ||
                (value is string && String.IsNullOrEmpty((string) value) == true))
            {
                return (0);
            }

            return (Convert.ToInt32(value));
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

        #region GetBackBounds

        /// <summary>
        /// Gets the background bounds for the given cell
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        private Rectangle GetBackBounds(Rectangle cellBounds)
        {
            DataGridViewSliderColumn oc = (DataGridViewSliderColumn)OwningColumn;

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
        /// <returns></returns>
        private Rectangle GetContentBounds(Rectangle cellBounds)
        {
            DataGridViewSliderColumn oc = (DataGridViewSliderColumn)OwningColumn;

            if (oc.Slider.Parent == null)
            {
                Form form = oc.DataGridView.FindForm();

                if (form != null)
                    oc.Slider.Parent = form;
            }

            oc.Slider.SliderItem.RecalcSize();

            cellBounds.Width -= (oc.DividerWidth + 3);
            cellBounds.Height = oc.Slider.SliderItem.HeightInternal;

            return (cellBounds);
        }

        #endregion

        #region RefreshSlider

        /// <summary>
        /// Initiates the refresh of the cell slider
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        internal void RefreshSlider(int columnIndex, int rowIndex)
        {
            DataGridView.InvalidateCell(columnIndex, rowIndex);
        }

        #endregion
    }
}
