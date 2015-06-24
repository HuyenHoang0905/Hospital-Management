#if FRAMEWORK20
using System;
using System.Windows.Forms;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the DataGridView control with enhanced Office 2007 style.
    /// </summary>
    [ToolboxBitmap(typeof(DataGridViewX), "Controls.DataGridViewX.bmp")]
    public class DataGridViewX : DataGridView
    {
        #region Private Variables
        private int m_MouseOverColumnHeader = -2;
        private int m_MouseDownColumnHeader = -2;
        private int m_MouseDownRowIndex = -2;
        private int m_MouseOverRowIndex = -2;
        private bool m_Office2007StyleEnabled = true;
        private Office2007DataGridViewColorTable m_ColorTable = null;
        private Office2007ButtonItemStateColorTable m_ButtonStateColorTable = null;
        private SelectionInfo[] m_ColumnSelectionState = new SelectionInfo[1];
        //private int m_CurrentCellRowIndex = -1;
        private int m_SelectedRowIndex = -2;
        private bool m_HighlightSelectedColumnHeaders = true;
        private bool m_SelectAllSignVisible = true;
        private bool m_PaintEnhancedSelection = true;
        #endregion

        #region Constructor
        public DataGridViewX()
            : base()
        {
            SetupScrollBars();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            StyleManager.Register(this);
        }
        #endregion

        #region internal properties

        internal Office2007ButtonItemStateColorTable ButtonStateColorTable
        {
            get { return (m_ButtonStateColorTable); }
        }

        #endregion

        #region Internal Implementation
        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            DGVScrollBar vsb = GetVScrollBar();
            if (vsb != null && vsb.Visible) vsb.Invalidate();

            DGHScrollBar hsb = GetHScrollBar();
            if (hsb != null && hsb.Visible) hsb.Invalidate();
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            bool enabled = this.Enabled;
            Office2007DataGridViewColorTable ct = GetColorTable();
            if (!m_Office2007StyleEnabled || ct == null ||
                e.Handled || e.ColumnIndex >= 0 && e.RowIndex >= 0 &&
                (!m_PaintEnhancedSelection || (e.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.Selected))
            {
                base.OnCellPainting(e);
                return;
            }

            Rectangle r = e.CellBounds;
            Graphics g = e.Graphics;
            LinearGradientColorTable bt = null;
            Color borderColor = Color.Empty;
            DataGridViewColumn column = null;
            int displayIndex = -1;
            if (e.ColumnIndex >= 0)
            {
                column = this.Columns[e.ColumnIndex];
                displayIndex = column.DisplayIndex;
            }
            
            if (e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                // Paint top-left corner
                if (m_MouseOverColumnHeader == -1)
                {
                    bt = ct.SelectorMouseOverBackground;
                    borderColor = ct.SelectorMouseOverBorder;
                }
                else
                {
                    bt = ct.SelectorBackground;
                    borderColor = ct.SelectorBorder;
                }
                DisplayHelp.FillRectangle(g, r, bt);
                DisplayHelp.DrawRectangle(g, borderColor, r);
                if (m_MouseOverColumnHeader == -1)
                    borderColor = ct.SelectorMouseOverBorderLight;
                else
                    borderColor = ct.SelectorBorderLight;
                Rectangle inner = r;
                inner.Inflate(-1, -1);
                using (Pen p = new Pen(borderColor))
                {
                    g.DrawLine(p, inner.X, inner.Bottom - 1, inner.X, inner.Y);
                    g.DrawLine(p, inner.X, inner.Y, inner.Right - 1, inner.Y);
                }
                if (m_MouseOverColumnHeader == -1)
                    borderColor = ct.SelectorMouseOverBorderDark;
                else
                    borderColor = ct.SelectorBorderDark;
                using (Pen p = new Pen(borderColor))
                {
                    g.DrawLine(p, inner.Right - 1, inner.Y, inner.Right - 1, inner.Bottom - 1);
                    g.DrawLine(p, inner.X, inner.Bottom - 1, inner.Right - 1, inner.Bottom - 1);
                }

                if (m_SelectAllSignVisible)
                {
                    GraphicsPath path = GetSelectorPath(inner);
                    if (path != null)
                    {
                        DisplayHelp.FillPath(g, path, (m_MouseOverColumnHeader == -1 ? ct.SelectorMouseOverSign : ct.SelectorSign));
                        path.Dispose();
                    }
                }
            }
            else if (e.ColumnIndex == -1)
            {
                // Paint Rows
                bt = ct.RowNormalBackground;
                borderColor = ct.RowNormalBorder;

                if (m_MouseDownRowIndex == e.RowIndex)
                {
                    bt = ct.RowPressedBackground;
                    borderColor = ct.RowPressedBorder;
                }
                else if (m_SelectedRowIndex == e.RowIndex && enabled)
                {
                    if (m_MouseOverRowIndex == e.RowIndex)
                    {
                        bt = ct.RowSelectedMouseOverBackground;
                        borderColor = ct.RowSelectedMouseOverBorder;
                    }
                    else
                    {
                        bt = ct.RowSelectedBackground;
                        borderColor = ct.RowSelectedBorder;
                    }
                }
                else if (this.Rows[e.RowIndex].Selected && enabled)
                {
                    if (m_MouseOverRowIndex == e.RowIndex)
                    {
                        bt = ct.RowPressedBackground;
                        borderColor = ct.RowPressedBorder;
                    }
                    else
                    {
                        bt = ct.RowPressedBackground;
                        borderColor = ct.RowPressedBorder;
                    }
                }
                else if (m_MouseOverRowIndex == e.RowIndex)
                {
                    bt = ct.RowMouseOverBackground;
                    borderColor = ct.RowMouseOverBorder;
                }

                DisplayHelp.FillRectangle(g, r, bt);
                // Paint border
                using (Pen p = new Pen(borderColor))
                {
                    g.DrawLine(p, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);

                    if (m_SelectedRowIndex == e.RowIndex + 1 && enabled)
                    {
                        Color bc = ct.RowSelectedBorder;
                        if (m_MouseDownRowIndex == e.RowIndex  + 1)
                            bc = ct.RowPressedBorder;
                        else if (m_MouseOverRowIndex == e.RowIndex + 1)
                            bc = ct.RowSelectedMouseOverBorder;
                        using (Pen p2 = new Pen(bc))
                            g.DrawLine(p2, r.X, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
                    }
                    else
                        g.DrawLine(p, r.X, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
                }

                e.PaintContent(r);
            }
            else if (e.RowIndex == -1)
            {
                // Determine Colors
                if (m_MouseDownColumnHeader == e.ColumnIndex)
                {
                    bt = ct.ColumnHeaderPressedBackground;
                    borderColor = ct.ColumnHeaderPressedBorder;
                }
                else if (m_MouseOverColumnHeader == e.ColumnIndex)
                {
                    if (m_HighlightSelectedColumnHeaders && (displayIndex >= 0 && e.ColumnIndex >= 0 && (m_ColumnSelectionState.Length > displayIndex && m_ColumnSelectionState[displayIndex].Selected || this.Columns[e.ColumnIndex].Selected)))
                    {
                        bt = ct.ColumnHeaderSelectedMouseOverBackground;
                        borderColor = ct.ColumnHeaderSelectedMouseOverBorder;
                    }
                    else
                    {
                        bt = ct.ColumnHeaderMouseOverBackground;
                        borderColor = ct.ColumnHeaderMouseOverBorder;
                    }
                }
                else if (!m_HighlightSelectedColumnHeaders)
                {
                    bt = ct.ColumnHeaderNormalBackground;
                    borderColor = ct.ColumnHeaderNormalBorder;
                }
                else if (displayIndex >= 0 && e.ColumnIndex >= 0 && (m_ColumnSelectionState.Length > displayIndex && m_ColumnSelectionState[displayIndex].Selected || this.Columns[e.ColumnIndex].Selected) && enabled)
                {
                    bt = ct.ColumnHeaderSelectedBackground;
                    borderColor = ct.ColumnHeaderSelectedBorder;
                }
                else
                {
                    if (this.Columns[e.ColumnIndex].Selected && enabled)
                    {
                        bt = ct.ColumnHeaderPressedBackground;
                        borderColor = ct.ColumnHeaderPressedBorder;
                    }
                    else
                    {
                        bt = ct.ColumnHeaderNormalBackground;
                        borderColor = ct.ColumnHeaderNormalBorder;
                    }
                }

                // Paint row markers
                DisplayHelp.FillRectangle(g, r, bt);
                // Paint border
                using (Pen p = new Pen(borderColor))
                {
                    g.DrawLine(p, r.X, r.Bottom-1, r.Right - 1, r.Bottom-1);
                    //if (displayIndex == 0)
                    //    g.DrawLine(p, r.X, r.Y, r.X, r.Bottom - 1);

                    if (enabled && (m_HighlightSelectedColumnHeaders && (displayIndex>=0 && m_ColumnSelectionState.Length > displayIndex + 1 && (m_ColumnSelectionState[displayIndex + 1].Selected ||
                        m_ColumnSelectionState[displayIndex + 1].ColumnIndex == m_MouseDownColumnHeader))))
                    {
                        Color bc = ct.ColumnHeaderSelectedBorder;
                        if (m_ColumnSelectionState[displayIndex + 1].ColumnIndex == m_MouseDownColumnHeader)
                            bc = ct.ColumnHeaderPressedBorder;
                        else if (m_MouseOverColumnHeader == displayIndex + 1)
                            bc = ct.ColumnHeaderSelectedMouseOverBorder;
                        using (Pen p2 = new Pen(bc))
                            g.DrawLine(p2, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);
                    }
                    else
                        g.DrawLine(p, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);
                }
                e.PaintContent(r);
            }
            else if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            {
                IDataGridViewColumn ic = this.Columns[e.ColumnIndex] as IDataGridViewColumn;

                if (ic != null && ic.OwnerPaintCell == true)
                {
                    base.OnCellPainting(e);
                    return;
                }

                e.PaintBackground(r, false);

                if (enabled)
                {
                    r.Height--;
                    Office2007ButtonItemPainter.PaintBackground(g, m_ButtonStateColorTable, r, RoundRectangleShapeDescriptor.RectangleShape, false, false);
                    r.Height++;

                    if (CurrentCellAddress.X==e.ColumnIndex && CurrentCellAddress.Y==e.RowIndex && (e.PaintParts & DataGridViewPaintParts.Focus) == DataGridViewPaintParts.Focus &&
                        ShowFocusCues && Focused && r.Width > 0 && r.Height > 0)
                    {
                        ControlPaint.DrawFocusRectangle(g, r, Color.Empty, m_ButtonStateColorTable.TopBackground.End);
                    }
                }

                e.PaintContent(r);

            }
            e.Handled = true;
            base.OnCellPainting(e);
        }

        private GraphicsPath GetSelectorPath(Rectangle inner)
        {
            inner.Inflate(-3, -3);
            if (inner.Width > 2 && inner.Height > 2)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(inner.Right, inner.Y, inner.Right, inner.Bottom);
                path.AddLine(inner.Right, inner.Bottom, inner.Right - inner.Height, inner.Bottom);
                path.CloseAllFigures();
                return path;
            }

            return null;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007DataGridViewColorTable ct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.DataGridView;
                UpdateOffice2007Styles(ct);
            }
            base.OnHandleCreated(e);
        }

        private void UpdateOffice2007Styles(Office2007DataGridViewColorTable ct)
        {
            if (this.GridColor != ct.GridColor)
                this.GridColor = ct.GridColor;
            if(m_PaintEnhancedSelection)
                this.DefaultCellStyle.SelectionForeColor = this.DefaultCellStyle.ForeColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Office2007ColorTable ct = null;
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                ct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                m_ColorTable = ct.DataGridView;
                m_ButtonStateColorTable = ct.ButtonItemColors[0].Checked;
            }

            if (this.CurrentCell != null)
                m_SelectedRowIndex = this.CurrentCell.RowIndex;
            else
                m_SelectedRowIndex = -1;

            base.OnPaint(e);

            if (this.VerticalScrollBar.Visible && this.HorizontalScrollBar.Visible)
            {
                Rectangle r = new Rectangle(this.VerticalScrollBar.Left, this.VerticalScrollBar.Bottom, this.VerticalScrollBar.Width, this.HorizontalScrollBar.Height);
                Color c = ct.AppScrollBar.Default.Background.End;
                if (c.IsEmpty) c = ct.AppScrollBar.Default.Background.Start;
                DisplayHelp.FillRectangle(e.Graphics, r, c);
                //e.Graphics.FillRectangle(Brushes.BlueViolet, r);
            }

            m_ColorTable = null;
            m_ButtonStateColorTable = null;
        }

        protected override void OnCurrentCellChanged(EventArgs e)
        {
            if (this.SelectionMode == DataGridViewSelectionMode.FullRowSelect)
                UpdateSelectionState();
            base.OnCurrentCellChanged(e);
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            UpdateSelectionState();
            base.OnSelectionChanged(e);
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
            UpdateSelectionState();
        }

        private void UpdateSelectionState()
        {
            SelectionInfo[] newSelection = new SelectionInfo[this.Columns.Count];
            for (int i = 0; i < this.Columns.Count; i++)
                newSelection[this.Columns[i].DisplayIndex].ColumnIndex = i;
            int columnCount = this.Columns.Count;

            if (this.SelectionMode == DataGridViewSelectionMode.FullRowSelect)
            {
                if (this.CurrentCell != null)
                {
                    int displayIndex = this.Columns[this.CurrentCell.ColumnIndex].DisplayIndex;
                    newSelection[displayIndex].Selected = true;
                    newSelection[displayIndex].ColumnIndex = this.CurrentCell.ColumnIndex;
                }
            }
            else
            {
                foreach (DataGridViewCell cell in this.SelectedCells)
                {
                    if (cell.ColumnIndex == -1) continue;
                    int displayIndex = this.Columns[cell.ColumnIndex].DisplayIndex;
                    if (!newSelection[displayIndex].Selected)
                    {
                        columnCount--;
                        newSelection[displayIndex].Selected = true;
                        newSelection[displayIndex].ColumnIndex = cell.ColumnIndex;
                        if (columnCount == 0) break;
                    }
                }
            }

            for (int i = 0; i < newSelection.Length; i++)
            {
                if (m_ColumnSelectionState.Length > i && newSelection[i].Selected != m_ColumnSelectionState[i].Selected)
                {
                    this.InvalidateColumn(m_ColumnSelectionState[i].ColumnIndex);
                    if (m_ColumnSelectionState[i].ColumnIndex > 0) this.InvalidateColumn(m_ColumnSelectionState[i].ColumnIndex - 1);
                }
            }
            if (m_SelectedRowIndex > 0 && m_SelectedRowIndex < this.Rows.Count)
                this.InvalidateRow(m_SelectedRowIndex - 1);
            if (this.CurrentCell != null && this.CurrentCell.RowIndex > 0)
            {
                m_SelectedRowIndex = this.CurrentCell.RowIndex;
                this.InvalidateRow(m_SelectedRowIndex - 1);
            }
            else
                m_SelectedRowIndex = -2;
            m_ColumnSelectionState = newSelection;
        }

        protected virtual Office2007DataGridViewColorTable GetColorTable()
        {
            return m_ColorTable;
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            // Disable double buffer so column resize drag markers will work
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                this.DoubleBuffered = false;
            }

            if (e.RowIndex == -1)
            {
                m_MouseDownColumnHeader = e.ColumnIndex;
                if (e.ColumnIndex >= 0)
                    this.InvalidatePreviousColumn(this.Columns[e.ColumnIndex].DisplayIndex);
            }
            else
                m_MouseDownColumnHeader = -2;

            if (e.ColumnIndex == -1)
            {
                m_MouseDownRowIndex = e.RowIndex;
                if (m_MouseDownRowIndex > 0 && m_MouseDownRowIndex < this.Rows.Count)
                    this.InvalidateRow(m_MouseDownRowIndex - 1);
            }
            else
                m_MouseDownRowIndex = -2;

            base.OnCellMouseDown(e);
        }

        private void InvalidatePreviousColumn(int displayIndex)
        {
            displayIndex--;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                DataGridViewColumn c = this.Columns[i];
                if (c.Displayed && c.DisplayIndex == displayIndex)
                {
                    this.InvalidateColumn(i);
                    break;
                }
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            CleanUpOnCellMouseUp();
            base.OnDragDrop(drgevent);
        }

        private void CleanUpOnCellMouseUp()
        {
            // Enable double buffering that was disabled in CellMouseDown
            if (!this.DoubleBuffered)
                this.DoubleBuffered = true;

            if (m_MouseDownRowIndex > 0 && m_MouseDownRowIndex < this.Rows.Count)
                this.InvalidateRow(m_MouseDownRowIndex - 1);
            if (m_MouseDownRowIndex >= 0)
                this.InvalidateRow(m_MouseDownRowIndex);

            m_MouseDownRowIndex = -2;
            m_MouseDownColumnHeader = -2;
        }
        protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
        {
            CleanUpOnCellMouseUp();
            base.OnCellMouseUp(e);
        }

        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                m_MouseOverColumnHeader = e.ColumnIndex;
            else
                m_MouseOverColumnHeader = -2;
            if (e.ColumnIndex == -1)
            {
                if (m_MouseOverRowIndex != e.RowIndex)
                {
                    m_MouseOverRowIndex = e.RowIndex;
                    if (m_MouseOverRowIndex > 0)
                        this.InvalidateRow(m_MouseOverRowIndex - 1);
                }
            }
            else
                m_MouseOverRowIndex = -2;
            base.OnCellMouseEnter(e);
        }

        protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
        {
            m_MouseOverColumnHeader = -2;
            m_MouseOverRowIndex = -2;
            base.OnCellMouseLeave(e);
        }

        private struct SelectionInfo
        {
            public bool Selected;
            public int ColumnIndex;
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            DGVScrollBar vsb = GetVScrollBar();
            if (vsb != null && vsb.Visible) vsb.UpdateScrollValues();

            DGHScrollBar hsb = GetHScrollBar();
            if (hsb != null && hsb.Visible) hsb.UpdateScrollValues();
        }

        private DGVScrollBar GetVScrollBar()
        {
            return this.VerticalScrollBar as DGVScrollBar;
            //Type t = typeof(System.Windows.Forms.DataGridView);
            //FieldInfo fi = t.GetField("vertScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            //if (fi == null) return null;
            //DGVScrollBar sb = fi.GetValue(this) as DGVScrollBar;
            //return sb;
        }

        private DGHScrollBar GetHScrollBar()
        {
            return this.HorizontalScrollBar as DGHScrollBar;
            //Type t = typeof(System.Windows.Forms.DataGridView);
            //FieldInfo fi = t.GetField("horizScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            //if (fi == null) return null;
            //DGHScrollBar sb = fi.GetValue(this) as DGHScrollBar;
            //return sb;
        }

        private void InvokeDelayed(MethodInvoker method)
        {
            Timer delayedInvokeTimer = new Timer();
            delayedInvokeTimer = new Timer();
            delayedInvokeTimer.Tag = method;
            delayedInvokeTimer.Interval = 10;
            delayedInvokeTimer.Tick += new EventHandler(DelayedInvokeTimerTick);
            delayedInvokeTimer.Start();
        }
        void DelayedInvokeTimerTick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;
            MethodInvoker method = (MethodInvoker)timer.Tag;
            timer.Stop();
            timer.Dispose();
            method.Invoke();
        }

        protected override void OnResize(EventArgs e)
        {
            Form form = this.FindForm();
            if (form != null && form.IsMdiChild)
                InvokeDelayed(new MethodInvoker(delegate { CallBaseOnResize(e); }));
            else
                base.OnResize(e);
        }

        private void CallBaseOnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        private void SetupScrollBars()
        {
            // Vertical Scroll Bar Replacement
            Type t = typeof(System.Windows.Forms.DataGridView);
            FieldInfo fi = t.GetField("vertScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            if(fi==null) return;
            System.Windows.Forms.ScrollBar sb = fi.GetValue(this) as System.Windows.Forms.ScrollBar;
            if (sb == null) return;
            //sb.Scroll += new ScrollEventHandler(sb_Scroll); return;
            MethodInfo mi = t.GetMethod("DataGridViewVScrolled", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi == null) return;

            DGVScrollBar newVSb = new DGVScrollBar();
            newVSb.AppStyleScrollBar = true;
            newVSb.Minimum = sb.Minimum;
            newVSb.Maximum = sb.Maximum;
            newVSb.SmallChange = sb.SmallChange;
            newVSb.LargeChange = sb.LargeChange;
            newVSb.Top = sb.Top;
            newVSb.AccessibleName = sb.AccessibleName;
            newVSb.Left = sb.Left;
            newVSb.Visible = sb.Visible;
            newVSb.Scroll += (ScrollEventHandler)ScrollEventHandler.CreateDelegate(typeof(ScrollEventHandler), this, mi);
            fi.SetValue(this, newVSb);
            sb.Dispose();
            this.Controls.Remove(sb);
            this.Controls.Add(newVSb);

            // Horizontal Scroll Bar Replacement
            fi = t.GetField("horizScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null) return;
            sb = fi.GetValue(this) as System.Windows.Forms.ScrollBar;
            if (sb == null) return;
            mi = t.GetMethod("DataGridViewHScrolled", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi == null) return;

            DGHScrollBar newHSb = new DGHScrollBar();
            newHSb.AppStyleScrollBar = true;
            newHSb.Minimum = sb.Minimum;
            newHSb.Maximum = sb.Maximum;
            newHSb.SmallChange = sb.SmallChange;
            newHSb.LargeChange = sb.LargeChange;
            newHSb.Top = sb.Top;
            newHSb.AccessibleName = sb.AccessibleName;
            newHSb.Left = sb.Left;
            newHSb.Visible = sb.Visible;
            newHSb.RightToLeft = sb.RightToLeft;
            newHSb.Scroll += (ScrollEventHandler)ScrollEventHandler.CreateDelegate(typeof(ScrollEventHandler), this, mi);
            fi.SetValue(this, newHSb);
            sb.Dispose();
            this.Controls.Remove(sb);
            this.Controls.Add(newHSb);

            base.PerformLayout();
        }

        //void sb_Scroll(object sender, ScrollEventArgs e)
        //{
        //    Console.WriteLine(e.NewValue);
        //}

        /// <summary>
        /// Gets or sets whether selected column header is highlighted. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether selected column header is highlighted.")]
        public bool HighlightSelectedColumnHeaders
        {
            get { return m_HighlightSelectedColumnHeaders; }
            set
            {
                m_HighlightSelectedColumnHeaders = value;
                if(BarFunctions.IsHandleValid(this))
                    this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether select all sign displayed in top-left corner of the grid is visible. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether select all sign displayed in top-left corner of the grid is visible.")]
        public bool SelectAllSignVisible
        {
            get { return m_SelectAllSignVisible; }
            set
            {
                m_SelectAllSignVisible = value;
                if (BarFunctions.IsHandleValid(this))
                    this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether enhanced selection for the cells is painted in Office 2007 style. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether enhanced selection for the cells is painted in Office 2007 style")]
        public bool PaintEnhancedSelection
        {
            get { return m_PaintEnhancedSelection; }
            set
            {
                m_PaintEnhancedSelection = value;
                if (BarFunctions.IsHandleValid(this))
                    this.Invalidate();
            }
        }
        #endregion
    }
}
#endif