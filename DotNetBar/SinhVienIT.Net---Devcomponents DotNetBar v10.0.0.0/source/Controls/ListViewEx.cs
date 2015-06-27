#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the extended ListView control with the Office 2007 Style.
    /// </summary>
    [ToolboxBitmap(typeof(ListViewEx), "Controls.ListViewEx.ico"), ToolboxItem(true)]
    public class ListViewEx : ListView, INonClientControl
    {
        #region Private Variables
        private NonClientPaintHandler m_NCPainter = null;
        private ElementStyle m_BorderStyle = null;
        #endregion

        #region Constructor, Dispose
        public ListViewEx()
            : base()
        {
            m_BorderStyle = new ElementStyle();
            m_BorderStyle.Class = ElementStyleClassKeys.ListViewBorderKey;
            m_BorderStyle.StyleChanged += new EventHandler(BorderStyle_StyleChanged);
            m_NCPainter = new NonClientPaintHandler(this, eScrollBarSkin.Optimized);
            this.OwnerDraw = true;
            this.DoubleBuffered = true;
            this.BorderStyle = BorderStyle.None;
            StyleManager.Register(this);
        }

        protected override void Dispose(bool disposing)
        {
            StyleManager.Unregister(this);
            if (m_NCPainter != null)
            {
                m_NCPainter.Dispose();
                //m_NCPainter = null;
            }
            if (m_BorderStyle != null) m_BorderStyle.StyleChanged -= new EventHandler(BorderStyle_StyleChanged);
            base.Dispose(disposing);
        }
        #endregion

        #region Inernal Implementation
        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            this.Invalidate(true);
            ResetCachedColorTableReference();
        }

        private void PaintHeaderBackground(Graphics g, Rectangle r)
        {
            Office2007ListViewColorTable ct = GetColorTable();
            DisplayHelp.FillRectangle(g, r, ct.ColumnBackground);
            using (Pen pen = new Pen(ct.Border))
            {
                g.DrawLine(pen, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
            }
        }
        private HeaderHandler m_HeaderHandler = null;
        protected override void OnHandleCreated(EventArgs e)
        {
            m_HeaderHandler = new HeaderHandler(this);
            base.OnHandleCreated(e);
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (m_HeaderHandler != null)
            {
                m_HeaderHandler.ReleaseHandle();
                m_HeaderHandler = null;
            }
            base.OnHandleDestroyed(e);
        }
        /// <summary>
        /// Resets internal Column Header handler. This method should be called after you change the View property
        /// at run-time. If your control is always in single view there is no need to call this method. This method
        /// will work only when controls handle has already been created.
        /// </summary>
        public void ResetHeaderHandler()
        {
            if (!this.IsHandleCreated) return;

            if (m_HeaderHandler != null)
            {
                m_HeaderHandler.ReleaseHandle();
                m_HeaderHandler = null;
            }
            m_HeaderHandler = new HeaderHandler(this);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (System.Environment.OSVersion.Version.Major < 6 && this.Visible)
                WinApi.PostMessage(this.Handle.ToInt32(), 0x120, 0, 0);
            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// Resets the internal color table reference. This method is usually called automatically by 
        /// </summary>
        public void ResetCachedColorTableReference()
        {
            m_ListViewColorTable = null;
            m_CheckBoxColorTable = null;
        }

        private Office2007ListViewColorTable m_ListViewColorTable = null;
        private Office2007ListViewColorTable GetColorTable()
        {
            if (m_ListViewColorTable == null)
            {
                Office2007Renderer r = this.GetRenderer() as Office2007Renderer;
                if (r != null)
                {
                    m_ListViewColorTable = r.ColorTable.ListViewEx;
                }
            }

            return m_ListViewColorTable;
        }

        private Office2007CheckBoxColorTable m_CheckBoxColorTable = null;
        private Office2007CheckBoxColorTable GetCheckBoxColorTable()
        {
            if (m_CheckBoxColorTable == null)
            {
                Office2007Renderer r = this.GetRenderer() as Office2007Renderer;
                if (r != null)
                {
                    m_CheckBoxColorTable = r.ColorTable.CheckBoxItem;
                }
            }

            return m_CheckBoxColorTable;
        }


        private void DrawColumnHeaderInternal(DrawListViewColumnHeaderEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            Office2007ListViewColorTable ct = GetColorTable();

            Color c1 = ct.ColumnBackground.Start;
            Color c2 = ct.ColumnBackground.End;
            if ((e.State & ListViewItemStates.Selected) == ListViewItemStates.Selected && !c2.IsEmpty)
            {
                Color csw = c1;
                c1 = c2;
                c2 = csw;
            }

            DisplayHelp.FillRectangle(g, r, c1, c2, ct.ColumnBackground.GradientAngle);

            using (Pen pen = new Pen(ct.Border))
            {
                g.DrawLine(pen, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
            }
            using (Pen pen = new Pen(ct.ColumnSeparator))
                g.DrawLine(pen, r.Right - 1, r.Y + 3, r.Right - 1, r.Bottom - 4);

            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis | TextFormatFlags.NoPrefix;
            switch (e.Header.TextAlign)
            {
                case HorizontalAlignment.Center:
                    flags |= TextFormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Right:
                    flags |= TextFormatFlags.Right;
                    break;
            }

            if (e.Header.ImageList != null && (e.Header.ImageIndex >= 0 || e.Header.ImageKey != null && e.Header.ImageKey.Length > 0))
            {
                Image img = null;
                if (e.Header.ImageIndex >= 0)
                    img = e.Header.ImageList.Images[e.Header.ImageIndex];
                else
                    img = e.Header.ImageList.Images[e.Header.ImageKey];
                if (img != null)
                {
                    Rectangle imageRect = new Rectangle(r.X + 2, r.Y + (r.Height - img.Height) / 2, img.Width, img.Height);
                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        imageRect.X = r.Right - imageRect.Width - 2;
                        r.Width -= imageRect.Width + 2;
                    }
                    else
                    {
                        r.Width -= imageRect.Width;
                        r.X += imageRect.Width;
                    }
                    g.DrawImage(img, imageRect);
                }
            }

            DrawHeaderText(g, e.Font, e.ForeColor, flags, e.Header.Text, r);
            //e.DrawText(flags);
        }


        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            //DrawColumnHeaderInternal(e);   
            base.OnDrawColumnHeader(e);
        }

        private void DrawHeaderText(Graphics g, Font font, Color c, TextFormatFlags flags, string text, Rectangle r)
        {
            Size sz = TextRenderer.MeasureText(" ", font);
            int w = sz.Width;
            r.Inflate(-(w / 2), 0);
            TextRenderer.DrawText(g, text, font, r, c, flags);
        }

        private void DrawItemBackground(Graphics g, Rectangle r, ListViewItem item, ListViewItemStates state)
        {
            if (!this.Enabled) return;
            // Bug fix in ListView for this specific state
            if (state == 0 && item.Selected && this.View == View.Details && this.FullRowSelect && this.Focused)
                r.X++;

            if (!item.BackColor.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(item.BackColor))
                {
                    g.FillRectangle(brush, r);
                }
            }
            bool selected = IsInState(state, ListViewItemStates.Selected) || state == 0 && item.Selected;
            if (selected)
            {
                Office2007ListViewColorTable ct = GetColorTable();
                // Draw the background for selected item.
                r.Height--;
                r.Width--;
                using (Pen pen = new Pen(ct.SelectionBorder))
                    g.DrawRectangle(pen, r);
                r.Height++;
                r.Width++;
                Rectangle ir = new Rectangle(r.X, r.Y + 1, r.Width, r.Height - 2);
                DisplayHelp.FillRectangle(g, ir, ct.SelectionBackground);
            }
            else if (IsInState(state, ListViewItemStates.Hot) && this.HotTracking)
            {
                Office2007Renderer rnd = this.GetRenderer() as Office2007Renderer;
                Office2007ButtonItemPainter.PaintBackground(g, rnd.ColorTable.ButtonItemColors[0].MouseOver, r, RoundRectangleShapeDescriptor.RectangleShape);
            }

            if (IsInState(state, ListViewItemStates.Focused) && (!IsInState(state, ListViewItemStates.Hot) && this.View != View.LargeIcon || selected))
            {
                Rectangle rFocus = item.Bounds;
                if (this.View == View.Details && !this.FullRowSelect || this.View == View.List)
                    rFocus = item.GetBounds(ItemBoundsPortion.Label);
                else if (this.View == View.Details && this.FullRowSelect)
                    rFocus = r;
                else if (this.View == View.SmallIcon)
                    rFocus = r;

                if (selected)
                {
                    rFocus.Y++;
                    rFocus.Height -= 2;
                }

                DrawFocusRectangle(g, rFocus, item);
            }
            else if (selected && this.View == View.Details && this.FullRowSelect && this.Focused) // Bug fix in ListView for this specific state
            {
                Rectangle rFocus = r;
                rFocus.Y++;
                rFocus.Height -= 2;
                Region oldClip = g.Clip;
                Rectangle rClip = rFocus;
                rClip.Width--;
                g.SetClip(rClip);
                DrawFocusRectangle(g, rFocus, item);
                g.Clip = oldClip;
            }
        }

        private bool IsInState(ListViewItemStates currentState, ListViewItemStates testState)
        {
            return ((currentState & testState) == testState);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            if (e.State == 0 && this.View != View.List && this.View != View.Tile && this.View != View.SmallIcon && this.View != View.LargeIcon || this.View == View.Details)
            {
                base.OnDrawItem(e);
                return;
            }

            FixStateForHideSelection(e);

            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;

            if (this.View == View.List)
                r = e.Item.GetBounds(ItemBoundsPortion.Label);
            else if (this.View == View.SmallIcon)
            {
                r = e.Item.GetBounds(ItemBoundsPortion.Label);
                //Size sz = TextDrawing.MeasureString(g, e.Item.Text, e.Item.Font);
                //sz.Width += 2;
                //if (sz.Width > r.Width)
                //    r.Width = sz.Width;
            }
            DrawItemBackground(g, r, e.Item, e.State);
            DrawListViewItem(g, e.Item, null, e.Item.Font, e.Item.ForeColor, e.State);

            base.OnDrawItem(e);
        }

        private bool HasImage(ListViewItem item)
        {
            if (item.ImageList != null && (item.ImageIndex >= 0 || item.ImageKey.Length > 0))
                return true;
            return false;
        }

        private Size m_CheckSignSize = new Size(13, 13);
        private void DrawListViewItem(Graphics g, ListViewItem item, ColumnHeader header, Font font, Color color, ListViewItemStates state)
        {
            bool hasImage = HasImage(item);
            // Draw Image if any
            if (hasImage && (header == null || header.Width > 4))
            {
                Rectangle rImage = item.GetBounds(ItemBoundsPortion.Icon);
                int index = item.ImageIndex;
                if (index < 0)
                    index = item.ImageList.Images.IndexOfKey(item.ImageKey);
                if (this.View != View.Details && this.View != View.List && this.StateImageList != null)
                    rImage.X += this.StateImageList.ImageSize.Width + 3;
                else if (this.View == View.SmallIcon && this.CheckBoxes && this.Groups.Count == 0)
                    rImage.X += this.SmallImageList.ImageSize.Width;
                else if (this.View == View.LargeIcon && (item.ImageList.ImageSize.Width < rImage.Width || item.ImageList.ImageSize.Height < rImage.Height))
                {
                    if (item.ImageList.ImageSize.Width < rImage.Width)
                        rImage.X += (rImage.Width - item.ImageList.ImageSize.Width) / 2;
                    if (item.ImageList.ImageSize.Height < rImage.Height)
                        rImage.Y += (rImage.Height - item.ImageList.ImageSize.Height) / 2;
                }

                Region oldClip = null;
                if (header != null && header.Width < rImage.Width)
                {
                    Rectangle rClip = rImage;
                    rClip.Width = header.Width;
                    oldClip = g.Clip;
                    g.SetClip(rClip);
                }
                if (rImage.Width > 2)
                {
                    g.DrawImage(item.ImageList.Images[index], rImage.Location); // item.ImageList.Draw(g, rImage.Location, index);
                }

                if (oldClip != null) g.Clip = oldClip;
            }

            // Draw text
            Rectangle rText = item.GetBounds(ItemBoundsPortion.Label);
            if (rText.Width > 2)
            {
                // Draw the item text for views other than the Details view.
                eTextFormat flags = eTextFormat.Left | eTextFormat.SingleLine | eTextFormat.NoPrefix;
                if (this.View == View.Tile && item.SubItems.Count > 1)
                    flags |= eTextFormat.Top;
                else
                    flags |= eTextFormat.VerticalCenter;
                if (this.View == View.LargeIcon)
                {
                    flags = eTextFormat.HorizontalCenter | eTextFormat.WordBreak | eTextFormat.EndEllipsis | eTextFormat.NoClipping;
                }
                else if (this.View == View.Details && header != null)
                {
                    flags |= eTextFormat.EndEllipsis;
                    if (header.TextAlign == HorizontalAlignment.Center)
                        flags |= eTextFormat.HorizontalCenter;
                    else if (header.TextAlign == HorizontalAlignment.Right)
                        flags |= eTextFormat.Right;

                    rText.X += 2;
                }
                else if (this.View == View.List || this.View == View.SmallIcon)
                    rText.X += 2;
                if (!BarFunctions.IsVista) rText.Inflate(0, 1);
                TextDrawing.DrawString(g, item.Text, font, color, rText, flags);

                if (this.View == View.Tile && item.SubItems.Count > 1)
                {
                    Size sz = TextDrawing.MeasureString(g, item.Text, font);
                    rText.Y += sz.Height;
                    rText.Height -= sz.Height;
                    Color c1 = item.SubItems[1].ForeColor;
                    if (!c1.IsEmpty && c1 != color)
                        color = c1;
                    else
                        color = SystemColors.ControlDarkDark;
                    TextDrawing.DrawString(g, item.SubItems[1].Text, font, color, rText, flags);
                }
            }

            if (this.View == View.Details || this.StateImageList != null)
            {
                if (this.StateImageList != null)
                {
                    if (item.StateImageIndex >= 0 && item.StateImageIndex < this.StateImageList.Images.Count)
                    {
                        Rectangle r = item.GetBounds(ItemBoundsPortion.Icon);
                        if (this.View == View.Details || this.View == View.List)
                            r.X -= 19;
                        else if (this.View == View.LargeIcon && r.Width > this.StateImageList.ImageSize.Width)
                        {
                            r.X += (r.Width - this.StateImageList.ImageSize.Width) / 2;
                            r.Y++;
                        }
                        else if (this.View == View.Tile && r.Height > this.StateImageList.ImageSize.Height)
                        {
                            r.Y += (r.Height - this.StateImageList.ImageSize.Height) / 2;
                            r.X++;
                        }
                        this.StateImageList.Draw(g, r.Location, item.StateImageIndex);
                    }
                }
            }

            if (this.CheckBoxes && (this.View == View.Details || this.View == View.List || this.View == View.SmallIcon || this.View == View.LargeIcon) && this.StateImageList == null)
            {
                Rectangle r = item.GetBounds(ItemBoundsPortion.Icon);
                if (this.View == View.LargeIcon)
                    r.X += (r.Width - m_CheckSignSize.Width) / 2 - 4;
                else if (this.View == View.List)
                    r.X -= 18;
                else if (this.View == View.SmallIcon && hasImage && this.Groups.Count > 0)
                    r.X -= 20;
                else if (this.View == View.SmallIcon)
                    r.X -= 3;
                else
                    r.X -= 21;
                Office2007CheckBoxItemPainter p = PainterFactory.CreateCheckBoxItemPainter(null);
                Office2007CheckBoxColorTable ctt = GetCheckBoxColorTable();
                Office2007CheckBoxStateColorTable ct = ctt.Default;
                if ((state & ListViewItemStates.Grayed) != 0 || !this.Enabled)
                    ct = ctt.Disabled;
                //else if ((state & ListViewItemStates.Hot) != 0)
                //    ct = ctt.MouseOver;

                p.PaintCheckBox(g, new Rectangle(r.X + 4, r.Y + (r.Height - m_CheckSignSize.Height) / 2, m_CheckSignSize.Width, m_CheckSignSize.Height),
                    ct, item.Checked ? CheckState.Checked : CheckState.Unchecked);
            }
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            FixStateForHideSelection(e);

            Graphics g = e.Graphics;
            //if (e.ItemState == 0 && this.View == View.Details && this.FullRowSelect) return; Can't do this can cause items not be displayed at all
            if (this.FullRowSelect && IsInState(e.ItemState, ListViewItemStates.Selected) && e.Header != null &&
                (e.Header.DisplayIndex == 0 || e.Bounds.X <= 0 && e.Header.DisplayIndex > 0 && this.Scrollable))
            {
                Rectangle r = e.Item.Bounds;
                if ((HasImage(e.Item) || e.Header.DisplayIndex != e.Header.Index) && e.Header.DisplayIndex == 0)
                {
                    Rectangle rLabel = e.Item.GetBounds(ItemBoundsPortion.Icon);
                    int w = Math.Min(e.Header.Width, (rLabel.Right - r.X) + 1);
                    r.Width -= w;
                    r.X += w;
                }
                DrawItemBackground(g, r, e.Item, e.ItemState);
            }

            if (e.ColumnIndex == 0)
            {
                if (!(this.FullRowSelect && IsInState(e.ItemState, ListViewItemStates.Selected)))
                {
                    Rectangle r = e.Item.GetBounds(ItemBoundsPortion.Label);

                    //if (System.Environment.OSVersion.Version.Major < 6)
                    //    r.Height += 2;
                    //else
                    //    r.Height++;
                    DrawItemBackground(g, r, e.Item, e.ItemState);
                }
                Color foreColor = e.Item.ForeColor;
                if (!this.Enabled) foreColor = _DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor;
                DrawListViewItem(g, e.Item, e.Header, e.Item.Font, foreColor, e.ItemState);

                //int start = m_FirstVisibleColumnIndex;
                //if (start == 0) start++;
                // Paint SubItems
                //for (int i = start; i < e.Item.SubItems.Count; i++)
                //{
                //    System.Windows.Forms.ListViewItem.ListViewSubItem subItem = e.Item.SubItems[i];
                //    Rectangle r = subItem.Bounds;
                //    if (r.IntersectsWith(this.DisplayRectangle))
                //        PaintSubItem(g, subItem, r, e.ItemState, (i < this.Columns.Count ? this.Columns[i] : null), e.Item.BackColor);
                //}
            }
            else
            {
                if (this.FullRowSelect && IsInState(e.ItemState, ListViewItemStates.Selected) && e.Header.DisplayIndex != e.Header.Index)
                {
                    Rectangle r = e.Bounds;
                    DrawItemBackground(g, r, e.Item, e.ItemState);
                }
                if (e.Header.Width > 0)
                    PaintSubItem(g, e.SubItem, e.Bounds, e.ItemState, this.Columns[e.ColumnIndex], e.Item.BackColor);
            }
            base.OnDrawSubItem(e);
        }

        private Color _DisabledForeColor = Color.Empty;
        /// <summary>
        /// Gets or sets the item foreground color when control or item is disabled. By default SystemColors.Dark is used.
        /// </summary>
        [Category("Appearance"), Description("Indicates item foreground color when control or item is disabled.")]
        public Color DisabledForeColor
        {
            get { return _DisabledForeColor; }
            set { _DisabledForeColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized by VS.NET designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDisabledForeColor()
        {
            return !_DisabledForeColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDisabledForeColor()
        {
            DisabledForeColor = Color.Empty;
        }

        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public new bool HideSelection
        //{
        //    get { return base.HideSelection; }
        //    set { base.HideSelection = false; }
        //}

        //private int m_FirstVisibleColumnIndex = 0;
        //private void UpdateFirstVisibleColumnIndex()
        //{
        //    m_FirstVisibleColumnIndex = 0;
        //    if (this.View == View.Details && this.Columns.Count > 0 && this.Items.Count>0)
        //    {
        //        ListViewItem item = this.Items[0];
        //        if (item.Bounds.X < 0)
        //        {
        //            int x = item.Bounds.X;
        //            IList list = GetOrderedColumnList();
        //            for (int i = 0; i < list.Count; i++)
        //            {
        //                ColumnHeader h = list[i] as ColumnHeader;
        //                if (x + h.Width > 0)
        //                {
        //                    m_FirstVisibleColumnIndex = i;
        //                    break;
        //                }
        //                else
        //                    x += h.Width;
        //            }
        //        }
        //    }
        //}

        private IList GetOrderedColumnList()
        {
            IList columns = this.Columns;
            if (this.AllowColumnReorder)
            {
                ArrayList ca = new ArrayList(this.Columns);
                foreach (ColumnHeader h in columns)
                {
                    if (h.DisplayIndex < 0) continue;
                    ca[h.DisplayIndex] = h;
                }
                columns = ca;
            }

            return columns;
        }

        private void PaintSubItem(Graphics g, ListViewItem.ListViewSubItem subitem, Rectangle r, ListViewItemStates state, ColumnHeader header, Color itemBackColor)
        {
            if (!IsInState(state, ListViewItemStates.Selected) && this.Enabled)
            {
                Rectangle rFill = r;
                //rFill.Height++;
                if (!subitem.BackColor.IsEmpty /*&& !(subitem.BackColor == this.BackColor && !itemBackColor.IsEmpty)*/)
                {
                    using (SolidBrush brush = new SolidBrush(subitem.BackColor))
                        g.FillRectangle(brush, rFill);
                }
                else if (!itemBackColor.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(itemBackColor))
                        g.FillRectangle(brush, rFill);
                }
            }

            eTextFormat flags = eTextFormat.Left | eTextFormat.SingleLine | eTextFormat.VerticalCenter | eTextFormat.EndEllipsis | eTextFormat.NoClipping | eTextFormat.NoPrefix;

            if (header != null)
            {
                if (header.TextAlign == HorizontalAlignment.Center)
                    flags |= eTextFormat.HorizontalCenter;
                else if (header.TextAlign == HorizontalAlignment.Right)
                    flags |= eTextFormat.Right;
            }

            Color foreColor = subitem.ForeColor;
            if (!this.Enabled) foreColor = _DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor;
            Rectangle rText = Rectangle.Inflate(r, -2, BarFunctions.IsVista ? 0 : 1); // Rectangle.Inflate(r, -sz.Width, 0);
            TextDrawing.DrawString(g, subitem.Text, subitem.Font, foreColor, rText, flags);
        }

        private void DrawFocusRectangle(Graphics g, Rectangle r, ListViewItem item)
        {
            ControlPaint.DrawFocusRectangle(g, r, item.ForeColor, item.BackColor);
        }

        private Rectangle UpdateBounds(Rectangle originalBounds, bool drawText, ListViewItem item)
        {
            Rectangle r = originalBounds;
            if (this.View == View.Details)
            {
                if (!this.FullRowSelect && (item.SubItems.Count > 0))
                {
                    ListViewItem.ListViewSubItem subItem = item.SubItems[0];
                    Size textSize = TextRenderer.MeasureText(subItem.Text, subItem.Font);
                    r = new Rectangle(originalBounds.X, originalBounds.Y, textSize.Width, textSize.Height);
                    r.X += 4;
                    r.Width++;
                }
                else
                {
                    r.X += 4;
                    r.Width -= 4;
                }
                if (drawText)
                {
                    r.X--;
                }
            }
            return r;
        }

        private Point m_MouseDown = Point.Empty;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_MouseDown = new Point(e.X, e.Y);
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.View == View.Details && this.FullRowSelect && !this.IsDisposed && !this.CheckBoxes)
            {
                ListViewItem clickedItem = GetItemAt(5, e.Y);
                ListViewItem mouseDownItem = GetItemAt(5, m_MouseDown.Y);
                if (clickedItem != null && clickedItem == mouseDownItem)
                {
                    clickedItem.Selected = true;
                    clickedItem.Focused = true;
                }
            }
        }

        private bool m_FirstMouseEnter = true;
        protected override void OnMouseEnter(EventArgs e)
        {
            if (m_FirstMouseEnter)
            {
                this.Refresh();
                m_FirstMouseEnter = false;
            }
            base.OnMouseEnter(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (this.HideSelection)
            {
                if (!this.VirtualMode && this.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in this.SelectedItems)
                    {
                        Rectangle r = item.Bounds;
                        if (this.View == View.Details)
                        {
                            r.X = 0;
                            r.Width = this.Width;
                        }
                        else if (this.View == View.LargeIcon)
                        {
                            r.Inflate(32, 32);
                        }
                        this.Invalidate(r);
                    }
                }
            }

            base.OnLostFocus(e);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool OwnerDraw
        {
            get { return base.OwnerDraw; }
            set { base.OwnerDraw = value; }
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct NMCUSTOMDRAW
        {
            public NMHDR nmcd;
            public int dwDrawStage;
            public IntPtr hdc;
            public WinApi.RECT rc;
            public IntPtr dwItemSpec;
            public int uItemState;
            public IntPtr lItemlParam;
        }

        protected override void WndProc(ref Message m)
        {
            if (m_NCPainter != null)
            {
                bool callBase = m_NCPainter.WndProc(ref m);

                if (callBase)
                    base.WndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }

            //if (m.Msg == (int)WinApi.WindowsMessages.WM_HSCROLL)
            //{
            //    UpdateFirstVisibleColumnIndex();
            //}
        }

        private Rendering.BaseRenderer m_DefaultRenderer = null;
        private Rendering.BaseRenderer m_Renderer = null;
        private eRenderMode m_RenderMode = eRenderMode.Global;
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public virtual Rendering.BaseRenderer GetRenderer()
        {
            if (m_RenderMode == eRenderMode.Global && Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            else if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
                return m_Renderer;

            if (m_DefaultRenderer == null)
                m_DefaultRenderer = new Rendering.Office2007Renderer();

            return m_DefaultRenderer;
        }

        /// <summary>
        /// Gets or sets the redering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
        /// also be set to the custom renderer that will be used.
        /// </summary>
        [Browsable(false), DefaultValue(eRenderMode.Global)]
        public eRenderMode RenderMode
        {
            get { return m_RenderMode; }
            set
            {
                if (m_RenderMode != value)
                {
                    m_RenderMode = value;
                    m_ListViewColorTable = null;
                    this.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom renderer used by the items on this control. RenderMode property must also be set to eRenderMode.Custom in order renderer
        /// specified here to be used.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DevComponents.DotNetBar.Rendering.BaseRenderer Renderer
        {
            get
            {
                return m_Renderer;
            }
            set
            {
                m_Renderer = value;
                m_ListViewColorTable = null;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { }
        }

        private ElementStyle GetBorderStyle()
        {
            m_BorderStyle.SetColorScheme(this.GetColorScheme());
            bool disposeStyle = false;
            return ElementStyleDisplay.GetElementStyle(m_BorderStyle,out disposeStyle);
        }

        /// <summary>
        /// Specifies the control border style. Default value has Class property set so the system style for the control is used.
        /// </summary>
        [Browsable(true), Category("Style"), Description("Specifies the control border style. Default value has Class property set so the system style for the control is used."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle Border
        {
            get { return m_BorderStyle; }
        }
        private void BorderStyle_StyleChanged(object sender, EventArgs e)
        {
            if (!BarFunctions.IsHandleValid(this)) return;
            const int RDW_INVALIDATE = 0x0001;
            const int RDW_FRAME = 0x0400;
            NativeFunctions.RECT r = new NativeFunctions.RECT(0, 0, this.Width, this.Height);
            NativeFunctions.RedrawWindow(this.Handle, ref r, IntPtr.Zero, RDW_INVALIDATE | RDW_FRAME);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                p.ExStyle = p.ExStyle & ~(p.ExStyle & 0x200);
                return p;
            }
        }

        #endregion

        #region INonClientControl Members
        void INonClientControl.BaseWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        ItemPaintArgs INonClientControl.GetItemPaintArgs(System.Drawing.Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(this as IOwner, this, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = false; // m_DesignerSelection;
            pa.GlassEnabled = !this.DesignMode && WinApi.IsGlassEnabled;
            return pa;
        }

        ElementStyle INonClientControl.BorderStyle
        {
            get { return GetBorderStyle(); }
        }

        void INonClientControl.PaintBackground(PaintEventArgs e)
        {
            //if (this.Parent == null) return;
            //Type t = typeof(Control);
            //MethodInfo mi = t.GetMethod("PaintTransparentBackground", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(PaintEventArgs), typeof(Rectangle) }, null);
            //if (mi != null)
            //{
            //    mi.Invoke(this, new object[] { e, new Rectangle(0, 0, this.Width, this.Height) });
            //}
        }

        private ColorScheme m_ColorScheme = null;
        /// <summary>
        /// Returns the color scheme used by control. Color scheme for Office2007 style will be retrived from the current renderer instead of
        /// local color scheme referenced by ColorScheme property.
        /// </summary>
        /// <returns>An instance of ColorScheme object.</returns>
        protected virtual ColorScheme GetColorScheme()
        {
            BaseRenderer r = GetRenderer();
            if (r is Office2007Renderer)
                return ((Office2007Renderer)r).ColorTable.LegacyColors;
            if (m_ColorScheme == null)
                m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
            return m_ColorScheme;
        }

        IntPtr INonClientControl.Handle
        {
            get { return this.Handle; }
        }

        int INonClientControl.Width
        {
            get { return this.Width; }
        }

        int INonClientControl.Height
        {
            get { return this.Height; }
        }

        bool INonClientControl.IsHandleCreated
        {
            get { return this.IsHandleCreated; }
        }

        Point INonClientControl.PointToScreen(Point client)
        {
            return this.PointToScreen(client);
        }

        Color INonClientControl.BackColor
        {
            get { return this.BackColor; }
        }

        void INonClientControl.AdjustClientRectangle(ref Rectangle r) { }

        void INonClientControl.AdjustBorderRectangle(ref Rectangle r) { }

        void INonClientControl.RenderNonClient(Graphics g) { }
        #endregion

        #region HeaderNativeWindowHandler
        private class HeaderHandler : NativeWindow
        {
            private ListViewEx m_Parent = null;
            private bool m_MouseDown = false;
            private Point m_MouseDownPoint = Point.Empty;
            public HeaderHandler(ListViewEx parent)
            {
                m_Parent = parent;
                IntPtr h = new IntPtr(WinApi.SendMessage(parent.Handle, (0x1000 + 31), IntPtr.Zero, IntPtr.Zero));
                if (h != IntPtr.Zero)
                    AssignHandle(h);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == (int)WinApi.WindowsMessages.WM_PAINT)
                {
                    WinApi.PAINTSTRUCT ps = new WinApi.PAINTSTRUCT();
                    IntPtr hdc = WinApi.BeginPaint(m.HWnd, ref ps);
                    try
                    {
                        Graphics g = Graphics.FromHdc(hdc);
                        try
                        {
                            WinApi.RECT r = new WinApi.RECT();
                            WinApi.GetWindowRect(m.HWnd, ref r);
                            Rectangle rc = new Rectangle(0, 0, r.Width, r.Height);
                            using (BufferedBitmap bb = new BufferedBitmap(hdc, rc))
                            {
                                m_Parent.PaintHeaderBackground(bb.Graphics, rc);

                                IList columns = m_Parent.GetOrderedColumnList();

                                int x = 0;
                                foreach (ColumnHeader h in columns)
                                {

                                    Rectangle hr = new Rectangle(x, 0, h.Width, r.Height);
                                    ListViewItemStates state = ListViewItemStates.ShowKeyboardCues;
                                    if (m_MouseDown && hr.Contains(m_MouseDownPoint))
                                    {
                                        Rectangle rt = hr;
                                        rt.Inflate(-6, 0);
                                        if (rt.Contains(m_MouseDownPoint))
                                            state |= ListViewItemStates.Selected;
                                    }

                                    m_Parent.DrawColumnHeaderInternal(new DrawListViewColumnHeaderEventArgs(bb.Graphics, hr, h.DisplayIndex, h,
                                        state, SystemColors.ControlText, Color.Empty, m_Parent.Font));
                                    x += h.Width;
                                }
                                bb.Render(g);
                            }
                        }
                        finally
                        {
                            g.Dispose();
                        }
                    }
                    finally
                    {
                        WinApi.EndPaint(m.HWnd, ref ps);
                    }
                    return;
                }
                else if (m.Msg == (int)WinApi.WindowsMessages.WM_LBUTTONDOWN)
                {
                    if (m_Parent.HeaderStyle == ColumnHeaderStyle.Clickable)
                    {
                        m_MouseDown = true;
                        m_MouseDownPoint = new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam));
                        WinApi.RedrawWindow(m.HWnd, IntPtr.Zero, IntPtr.Zero, WinApi.RedrawWindowFlags.RDW_INVALIDATE);
                    }
                }
                else if (m.Msg == (int)WinApi.WindowsMessages.WM_LBUTTONUP)
                {
                    m_MouseDown = false;
                    m_MouseDownPoint = Point.Empty;
                }
                else if (m.Msg == (int)WinApi.WindowsMessages.WM_MOUSEMOVE && m_MouseDown)
                {
                    m_MouseDownPoint = new Point(WinApi.LOWORD(m.LParam), WinApi.HIWORD(m.LParam));
                }

                base.WndProc(ref m);
            }
        }
        #endregion

        #region Hide-Selection ListView Bug Hacking
        bool _IsItemInitialized = false;
        bool _IsSubItemInitialized = false;
        FieldInfo _ItemStateField = null;
        FieldInfo _SubItemStateField = null;

        private void FixStateForHideSelection(DrawListViewItemEventArgs e)
        {
            if (HideSelection) return;
            if (!this._IsItemInitialized)
            {
                _ItemStateField = e.GetType().GetField("state",BindingFlags.NonPublic | BindingFlags.Instance);
                _IsItemInitialized = true;
            }

            if (_ItemStateField != null)
            {
                UpdateStateBit(e, _ItemStateField, e.Item.Selected,e.State);
            }
        }

        private void FixStateForHideSelection(DrawListViewSubItemEventArgs e)
        {
            if (HideSelection) return;
            if (!_IsSubItemInitialized)
            {

                _SubItemStateField =e.GetType().GetField("itemState", BindingFlags.NonPublic |BindingFlags.Instance);
                _IsSubItemInitialized = true;
            }

            if (_SubItemStateField != null)
            {
                UpdateStateBit(e, _SubItemStateField,e.Item.Selected, e.ItemState);
            }
        }

        private void UpdateStateBit(object o, FieldInfo f, bool itemSelected, ListViewItemStates currentState)
        {
            bool stateSelected = (currentState & ListViewItemStates.Selected) != 0;

            if (itemSelected != stateSelected)
            {
                ListViewItemStates newState = currentState;
                if (itemSelected)
                {
                    newState |= ListViewItemStates.Selected;
                }
                else
                {
                    newState &= ~(ListViewItemStates.Selected);
                }
                try
                {
                    f.SetValue(o, newState);
                }
                catch { }
            }
        }


        #endregion
    }
}
#endif