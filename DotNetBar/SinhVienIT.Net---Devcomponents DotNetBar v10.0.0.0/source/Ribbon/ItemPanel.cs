using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents generic item panel container control.
    /// </summary>
    [ToolboxBitmap(typeof(ItemPanel), "Ribbon.ItemPanel.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.ItemPanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false)]
    public class ItemPanel : ItemControl, IScrollableItemControl
    {
        #region Private Variables
        private ItemContainer m_ItemContainer = null;
        private bool m_EnableDragDrop = false;
        private Point m_MouseDownPoint = Point.Empty;
        //private bool m_SuspendPaint = false;
        //private Controls.NonClientPaintHandler m_NCPainter = null;
        //private int m_SuspendPaintCount = 0;
        #endregion

        #region Constructor
        public ItemPanel()
        {
            m_ItemContainer = new ItemContainer();
            m_ItemContainer.GlobalItem = false;
            m_ItemContainer.ContainerControl = this;
            m_ItemContainer.Stretch = false;
            m_ItemContainer.Displayed = true;
            m_ItemContainer.Style = eDotNetBarStyle.Office2007;
            this.ColorScheme.Style = eDotNetBarStyle.Office2007;
            m_ItemContainer.SetOwner(this);
            m_ItemContainer.SetSystemContainer(true);

            this.SetBaseItemContainer(m_ItemContainer);
            m_ItemContainer.Style = eDotNetBarStyle.Office2007;

            this.DragDropSupport = true;
            _ItemTemplate = GetDefaultItemTemplate();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (m_NCPainter != null)
        //    {
        //        m_NCPainter.Dispose();
        //        m_NCPainter = null;
        //    }
        //    base.Dispose(disposing);
        //}
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Invalidates non-client area of the control. This method should be used
        /// when you need to invalidate non-client area of the control.
        /// </summary>
        public void InvalidateNonClient()
        {
            if(BarFunctions.IsHandleValid(this))
                WinApi.RedrawWindow(this.Handle, IntPtr.Zero, IntPtr.Zero, WinApi.RedrawWindowFlags.RDW_FRAME | WinApi.RedrawWindowFlags.RDW_INVALIDATE);
        }
        /// <summary>
        /// Returns first checked top-level button item.
        /// </summary>
        /// <returns>An ButtonItem object or null if no button could be found.</returns>
        public ButtonItem GetChecked()
        {
            foreach (BaseItem item in this.Items)
            {
                if (item.Visible && item is ButtonItem && ((ButtonItem)item).Checked)
                    return item as ButtonItem;
            }

            return null;
        }

        /// <summary>
        /// Gets/Sets the visual style for items and color scheme.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Specifies the visual style of the control."), DefaultValue(eDotNetBarStyle.Office2007)]
        public eDotNetBarStyle Style
        {
            get
            {
                return m_ItemContainer.Style;
            }
            set
            {
                //if(value == eDotNetBarStyle.Office2007)
                //    m_NCPainter.SkinScrollbars = eScrollBarSkin.Optimized;
                //else
                //    m_NCPainter.SkinScrollbars = eScrollBarSkin.None;
                this.ColorScheme.SwitchStyle(value);
                m_ItemContainer.Style = value;
                this.Invalidate();
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets spacing in pixels between items. Default value is 1.
        /// </summary>
        [Browsable(true), DefaultValue(1), Category("Layout"), Description("Indicates spacing in pixels between items.")]
        public int ItemSpacing
        {
            get { return m_ItemContainer.ItemSpacing; }
            set
            {
                m_ItemContainer.ItemSpacing = value;
            }
        }

        /// <summary>
        /// Gets or sets default layout orientation inside the control. You can have multiple layouts inside of the control by adding
        /// one or more instances of the ItemContainer object and chaning it's LayoutOrientation property.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Layout"), DefaultValue(eOrientation.Horizontal), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual eOrientation LayoutOrientation
        {
            get { return m_ItemContainer.LayoutOrientation; }
            set
            {
                m_ItemContainer.LayoutOrientation = value;
                if (this.DesignMode)
                    this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether items contained by container are resized to fit the container bounds. When container is in horizontal
        /// layout mode then all items will have the same height. When container is in vertical layout mode then all items
        /// will have the same width. Default value is true.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(true), Category("Layout")]
        public bool ResizeItemsToFit
        {
            get { return m_ItemContainer.ResizeItemsToFit; }
            set
            {
                m_ItemContainer.ResizeItemsToFit = value;
            }
        }

        /// <summary>
        /// Gets or sets whether ButtonItem buttons when in vertical layout are fit into the available width so any text inside of them
        /// is wrapped if needed. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Layout"), Description("Indicates whether ButtonItem buttons when in vertical layout are fit into the available width so any text inside of them is wrapped if needed.")]
        public bool FitButtonsToContainerWidth
        {
            get { return m_ItemContainer.FitOversizeItemIntoAvailableWidth; }
            set
            {
                m_ItemContainer.FitOversizeItemIntoAvailableWidth = value;
                if (this.DesignMode)
                    RecalcLayout();
            }
        }
        

        /// <summary>
        /// Gets or sets the item alignment when container is in horizontal layout. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eHorizontalItemsAlignment.Left), Category("Layout"), Description("Indicates item alignment when container is in horizontal layout."), DevCoBrowsable(true)]
        public eHorizontalItemsAlignment HorizontalItemAlignment
        {
            get { return m_ItemContainer.HorizontalItemAlignment; }
            set
            {
                m_ItemContainer.HorizontalItemAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Layout"), Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
        public bool MultiLine
        {
            get { return m_ItemContainer.MultiLine; }
            set
            {
                m_ItemContainer.MultiLine = value;
            }
        }

        /// <summary>
        /// Returns collection of items on a bar.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public SubItemsCollection Items
        {
            get
            {
                return m_ItemContainer.SubItems;
            }
        }

        /// <summary>
        /// Scrolls the control so that item is displayed within the visible bounds of the control.
        /// </summary>
        /// <param name="item">Item to ensure visibility for. Item must belong to this control.</param>
        public void EnsureVisible(BaseItem item)
        {
            if (item.ContainerControl != this)
                return;

            Rectangle r = item.DisplayRectangle;
            Rectangle bounds = this.ClientRectangle;
            if (bounds.Width < m_ItemContainer.WidthInternal)
                bounds.Width = m_ItemContainer.WidthInternal;
            if (r.Y < bounds.Y || r.Bottom > bounds.Height)
            {
                if (this.AutoScroll)
                {
                    Point p = Point.Empty;
                    if (r.Y < 0)
                        p = new Point(this.AutoScrollPosition.X, Math.Abs(this.AutoScrollPosition.Y - r.Y) - 2);
                    else
                        p = new Point(this.AutoScrollPosition.X,
                            (r.Bottom + SystemInformation.HorizontalScrollBarHeight + 2) - (this.AutoScrollPosition.Y + this.ClientRectangle.Height));
                    this.InvalidateLayout();
                    this.AutoScrollPosition = p;
                    this.RecalcLayout();
                }
            }
            //m_NCPainter.PaintNonClientAreaBuffered();
        }

        protected override Rectangle GetPaintClipRectangle()
        {
            Rectangle r = this.ClientRectangle;

            if (this.BackgroundStyle == null)
                return r;

            r.X += ElementStyleLayout.LeftWhiteSpace(this.BackgroundStyle);
            r.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(this.BackgroundStyle);
            r.Y += ElementStyleLayout.TopWhiteSpace(this.BackgroundStyle);
            r.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(this.BackgroundStyle);
            if (_VScrollBar != null) r.Width -= _VScrollBar.Width;
            if (_HScrollBar != null) r.Height -= _HScrollBar.Height;

            return r;
        }

        protected override Rectangle GetItemContainerRectangle()
        {
            Rectangle r = base.GetItemContainerRectangle();
            if (this.AutoScroll && this.AutoScrollPosition.Y!=0)
                r.Y += this.AutoScrollPosition.Y;
            if (this.AutoScroll && this.AutoScrollPosition.X != 0)
                r.X += this.AutoScrollPosition.X;
            
            if (_VScrollBar != null)
                r.Width -= _VScrollBar.Width;
            if (_HScrollBar != null)
                r.Height -= _HScrollBar.Height;

            return r;
        }

        private void ApplyScrollChange()
        {
            if (!this.AutoScroll) return;

            if (m_ItemContainer.NeedRecalcSize)
            {
                this.RecalcSize();
                return;
            }

            Rectangle r = base.GetItemContainerRectangle();
            if (this.AutoScrollPosition.Y != 0)
                r.Y += this.AutoScrollPosition.Y;
            if (this.AutoScrollPosition.X != 0)
                r.X += this.AutoScrollPosition.X;
            r.Height = m_ItemContainer.HeightInternal;
            r.Width = m_ItemContainer.WidthInternal;
            m_ItemContainer.Bounds = r;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Focused && this.IsSelectable)
                this.Select();

            m_MouseDownPoint.X = e.X;
            m_MouseDownPoint.Y = e.Y;

            base.OnMouseDown(e);
        }

        private bool IsSelectable
        {
            get
            {
                return this.GetStyle(ControlStyles.Selectable);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (m_EnableDragDrop && !this.DragInProgress && e.Button == MouseButtons.Left)
            {
                if (Math.Abs(e.X - m_MouseDownPoint.X) > SystemInformation.DragSize.Width ||
                    Math.Abs(e.Y - m_MouseDownPoint.Y) > SystemInformation.DragSize.Height)
                {
                    BaseItem item = HitTest(e.X, e.Y);
                    if (item != null)
                        ((IOwner)this).StartItemDrag(item);
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.Focused)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                m_ItemContainer.InternalKeyDown(e);
                if (e.Handled)
                    return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (_VScrollBar != null && _VScrollBar.Visible)
            {
                int newValue = _VScrollBar.Value + _VScrollBar.SmallChange * (e.Delta < 0 ? 1 : -1);
                if (newValue < _VScrollBar.Minimum) newValue = _VScrollBar.Minimum;
                if (newValue > _VScrollBar.Maximum - _VScrollBar.LargeChange + 1)
                    newValue = _VScrollBar.Maximum - _VScrollBar.LargeChange + 1;
                _VScrollBar.DoScroll(newValue, e.Delta < 0 ? ScrollEventType.SmallIncrement : ScrollEventType.SmallDecrement);
                
            }
            base.OnMouseWheel(e);
        }

        private int _EntryCount = 0;
        protected override void RecalcSize()
        {
            _EntryCount++;
            try
            {
                m_ItemContainer.MinimumSize = new Size(this.GetItemContainerRectangle().Width, 0);
                base.RecalcSize();

                if (!this.AutoSize && this.AutoScroll)
                {
                    if (m_ItemContainer.HeightInternal > this.ClientRectangle.Height || m_ItemContainer.WidthInternal > this.ClientRectangle.Width)
                    {
                        Size areaSize = Size.Empty;
                        int size = this.ClientRectangle.Height;
                        if (m_ItemContainer.WidthInternal > this.ClientRectangle.Width)
                            size -= SystemInformation.HorizontalScrollBarHeight;
                        if (m_ItemContainer.HeightInternal > size)
                            areaSize.Height = m_ItemContainer.HeightInternal;

                        size = this.ClientRectangle.Width;
                        if (m_ItemContainer.HeightInternal > this.ClientRectangle.Height)
                            size -= SystemInformation.VerticalScrollBarWidth;
                        if (m_ItemContainer.WidthInternal > size)
                            areaSize.Width = m_ItemContainer.WidthInternal + SystemInformation.VerticalScrollBarWidth;

                        if (this.BackgroundStyle != null)
                        {
                            areaSize.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(this.BackgroundStyle);
                            areaSize.Height += ElementStyleLayout.VerticalStyleWhiteSpace(this.BackgroundStyle);
                            if (areaSize.Width > this.ClientRectangle.Width)
                                areaSize.Height += SystemInformation.HorizontalScrollBarHeight + 1;
                        }

                        bool verticalScrollBarChange = _VScrollBar == null;
                        bool horizontalScrollBarChange = _HScrollBar == null;
                        if (this.AutoScrollMinSize != areaSize)
                        {
                            this.Invalidate();
                            this.AutoScrollMinSize = areaSize;
                        }
                        verticalScrollBarChange ^= (_VScrollBar == null);
                        horizontalScrollBarChange ^= (_HScrollBar == null);
                        if (_EntryCount < 4 && (verticalScrollBarChange || horizontalScrollBarChange))
                        {
                            RecalcSize();
                        }
                    }
                    else if (!this.AutoScrollMinSize.IsEmpty)
                    {
                        this.AutoScrollMinSize = Size.Empty;
                    }
                }
            }
            finally
            {
                _EntryCount--;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateScrollBars();
            RepositionScrollBars();
            //m_NCPainter.PaintNonClientAreaBuffered();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            RepositionScrollBars();
        }
        //protected override void OnVisualPropertyChanged()
        //{
        //    if (this.GetDesignMode() ||
        //        this.Parent != null && this.Parent.Site != null && this.Parent.Site.DesignMode)
        //    {
        //        if (BarFunctions.IsHandleValid(this))
        //        {
        //            NativeFunctions.SetWindowPos(this.Handle, 0, 0, 0, 0, 0,
        //                NativeFunctions.SWP_FRAMECHANGED | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE |
        //                NativeFunctions.SWP_NOOWNERZORDER | NativeFunctions.SWP_NOSENDCHANGING | NativeFunctions.SWP_NOSIZE |
        //                NativeFunctions.SWP_NOZORDER);
        //            this.InvalidateNonClient();
        //        }
        //    }
        //    else
        //        this.InvalidateNonClient();
        //    base.OnVisualPropertyChanged();
        //}
        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    if (m_NCPainter != null) m_NCPainter.PaintNonClientAreaBuffered();
        //}
        ///// <summary>
        ///// This member overrides Control.WndProc.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //protected override void WndProc(ref Message m)
        //{
        //    if (m_NCPainter == null)
        //    {
        //        base.WndProc(ref m);
        //        return;
        //    }

        //    bool callBase = true;

        //    if (this.AutoScroll)
        //    {
        //        if (m.Msg == (int)WinApi.WindowsMessages.WM_HSCROLL || m.Msg == (int)WinApi.WindowsMessages.WM_VSCROLL || m.Msg == (int)WinApi.WindowsMessages.WM_MOUSEWHEEL)
        //        {
        //            m_SuspendPaint = true;
        //            m_NCPainter.SuspendPaint = true;
        //            try
        //            {
        //                callBase = m_NCPainter.WndProc(ref m);
        //                if (callBase)
        //                    base.WndProc(ref m);
        //            }
        //            finally
        //            {
        //                m_SuspendPaint = false;
        //                m_NCPainter.SuspendPaint = false;
        //            }
        //            //m_ItemContainer.NeedRecalcSize = true;
        //            ApplyScrollChange();
        //            //this.RecalcSize();
                    
        //            m_NCPainter.PaintNonClientAreaBuffered();
        //            return;
        //        }
        //        else if (m.Msg == (int)WinApi.WindowsMessages.WM_COMMAND && this.Controls.Count > 0 && WinApi.HIWORD(m.WParam)==0x100 && m.LParam!=IntPtr.Zero)
        //        {
        //            Control c = Control.FromChildHandle(m.LParam);
        //            Rectangle r = new Rectangle(Point.Empty, this.ClientSize);
        //            r.Inflate(-1, -1);
        //            Rectangle rc = Rectangle.Empty;
        //            if (c != null)
        //            {
        //                rc = c.Bounds;
        //                rc.Offset(0, -this.AutoScrollPosition.Y);
        //            }
        //            if (c != null && c.Parent == this && (!r.Contains(c.Bounds) || !r.Contains(rc)))
        //            {
        //                m_SuspendPaintCount = 2;
        //            }
        //        }
        //    }

        //    callBase = m_NCPainter.WndProc(ref m);

        //    if (callBase)
        //        base.WndProc(ref m);
        //}
        
        //protected override void PaintControlBackground(ItemPaintArgs pa)
        //{
        //    ElementStyle style = GetBackgroundStyle();
        //    if (style != null)
        //    {
        //        Rectangle r = new Rectangle(0, 0, this.Width, this.Height);
        //        if (style.PaintBorder && (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal))
        //        {
        //            style = style.Copy();
        //            style.CornerType = eCornerType.Square;
        //            style.Border = eStyleBorderType.None;
        //            r.Inflate(style.BorderWidth, style.BorderWidth);
        //        }

        //        ElementStyleDisplayInfo displayInfo = new ElementStyleDisplayInfo(style, pa.Graphics, r);
        //        ElementStyleDisplay.PaintBackground(displayInfo);
        //        ElementStyleDisplay.PaintBackgroundImage(displayInfo);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the scrollbar skinning type when control is using Office 2007 style.
        ///// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public eScrollBarSkin ScrollbarSkin
        //{
        //    get { return m_NCPainter.SkinScrollbars; }
        //    set { m_NCPainter.SkinScrollbars = value; }
        //}
        #endregion

        #region Scrolling Support
        private DevComponents.DotNetBar.VScrollBarAdv _VScrollBar = null;
        private DevComponents.DotNetBar.ScrollBar.HScrollBarAdv _HScrollBar = null;
        private Control _Thumb = null;
        /// <summary>
        /// Gets the reference to internal vertical scroll-bar control if one is created or null if no scrollbar is visible.
        /// </summary>
        [Browsable(false)]
        public DevComponents.DotNetBar.VScrollBarAdv VScrollBar
        {
            get
            {
                return _VScrollBar;
            }
        }

        /// <summary>
        /// Gets the reference to internal horizontal scroll-bar control if one is created or null if no scrollbar is visible.
        /// </summary>
        [Browsable(false)]
        public DevComponents.DotNetBar.ScrollBar.HScrollBarAdv HScrollBar
        {
            get
            {
                return _HScrollBar;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
            set { base.AutoScrollMargin = value; }
        }

        private bool _AutoScroll = false;
        /// <summary>
        /// Gets or sets a value indicating whether the control enables the user to scroll to items placed outside of its visible boundaries.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        public new bool AutoScroll
        {
            get { return _AutoScroll; }
            set
            {
                if (_AutoScroll != value)
                {
                    _AutoScroll = value;
                    RecalcLayout();
                    UpdateScrollBars();
                }
            }
        }

        private Size _AutoScrollMinSize = Size.Empty;
        /// <summary>
        /// Gets or sets the minimum size of the auto-scroll. Returns a Size that represents the minimum height and width of the scrolling area in pixels.
        /// This property is managed internally by control and should not be modified.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size AutoScrollMinSize
        {
            get { return _AutoScrollMinSize; }
            set 
            { 
                _AutoScrollMinSize = value;
                UpdateScrollBars();
            }
        }

        private Point _AutoScrollPosition = Point.Empty;
        /// <summary>
        /// Gets or sets the location of the auto-scroll position.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Indicates location of the auto-scroll position.")]
        public new Point AutoScrollPosition
        {
            get
            {
                return _AutoScrollPosition;
            }
            set
            {
                if (value.X > 0) value.X = -value.X;
                if (value.Y > 0) value.Y = -value.Y;
                if (_AutoScrollPosition != value)
                {
                    _AutoScrollPosition = value;
                    if (_AutoScroll)
                    {
                        if (_VScrollBar != null && _VScrollBar.Value != -_AutoScrollPosition.Y)
                            _VScrollBar.Value = -_AutoScrollPosition.Y;
                        if (_HScrollBar != null && _HScrollBar.Value != -_AutoScrollPosition.X)
                            _HScrollBar.Value = -_AutoScrollPosition.X;
                        //RepositionHostedControls(false);
                        Invalidate();
                    }
                }
            }
        }

        private void UpdateScrollBars()
        {
            if (!_AutoScroll)
            {
                RemoveHScrollBar();
                RemoveVScrollBar();
                if (_Thumb != null)
                {
                    this.Controls.Remove(_Thumb);
                    _Thumb.Dispose();
                    _Thumb = null;
                }
                return;
            }

            Rectangle innerBounds = ElementStyleLayout.GetInnerRect(this.BackgroundStyle, this.ClientRectangle);
            // Check do we need vertical scrollbar
            Size scrollSize = _AutoScrollMinSize;
            if (scrollSize.Height > innerBounds.Height)
            {
                if (_VScrollBar == null)
                {
                    _VScrollBar = new DevComponents.DotNetBar.VScrollBarAdv();
                    _VScrollBar.Width = SystemInformation.VerticalScrollBarWidth;
                    this.Controls.Add(_VScrollBar);
                    _VScrollBar.BringToFront();
                    _VScrollBar.Scroll += new ScrollEventHandler(VScrollBarScroll);
                }
                if (_VScrollBar.Minimum != 0)
                    _VScrollBar.Minimum = 0;
                if (_VScrollBar.LargeChange != innerBounds.Height && innerBounds.Height > 0)
                    _VScrollBar.LargeChange = innerBounds.Height;
                _VScrollBar.SmallChange = 22;
                if (_VScrollBar.Maximum != _AutoScrollMinSize.Height)
                    _VScrollBar.Maximum = _AutoScrollMinSize.Height;
                if (_VScrollBar.Value != -_AutoScrollPosition.Y)
                    _VScrollBar.Value = (Math.Min(_VScrollBar.Maximum, Math.Abs(_AutoScrollPosition.Y)));
            }
            else
                RemoveVScrollBar();

            // Check horizontal scrollbar
            if (scrollSize.Width > innerBounds.Width)
            {
                if (_HScrollBar == null)
                {
                    _HScrollBar = new DevComponents.DotNetBar.ScrollBar.HScrollBarAdv();
                    _HScrollBar.Height = SystemInformation.HorizontalScrollBarHeight;
                    this.Controls.Add(_HScrollBar);
                    _HScrollBar.BringToFront();
                    _HScrollBar.Scroll += new ScrollEventHandler(HScrollBarScroll);
                }
                if (_HScrollBar.Minimum != 0)
                    _HScrollBar.Minimum = 0;
                if (_HScrollBar.LargeChange != innerBounds.Width && innerBounds.Width > 0)
                    _HScrollBar.LargeChange = innerBounds.Width;
                if (_HScrollBar.Maximum != _AutoScrollMinSize.Width)
                    _HScrollBar.Maximum = _AutoScrollMinSize.Width;
                if (_HScrollBar.Value != -_AutoScrollPosition.X)
                    _HScrollBar.Value = (Math.Min(_HScrollBar.Maximum, Math.Abs(_AutoScrollPosition.X)));
                _HScrollBar.SmallChange = 22;
            }
            else
                RemoveHScrollBar();
            RepositionScrollBars();
        }

        private void VScrollBarScroll(object sender, ScrollEventArgs e)
        {
            _AutoScrollPosition.Y = -e.NewValue;
            ApplyScrollChange();
            this.Invalidate();
#if FRAMEWORK20
            OnScroll(e);
#endif
        }
        private void HScrollBarScroll(object sender, ScrollEventArgs e)
        {
            _AutoScrollPosition.X = -e.NewValue;
            ApplyScrollChange();
            this.Invalidate();
        }

        private void RepositionScrollBars()
        {
            Rectangle innerBounds = ElementStyleLayout.GetInnerRect(this.BackgroundStyle, this.ClientRectangle);
            if (_HScrollBar != null)
            {
                int width = innerBounds.Width;
                if (_VScrollBar != null)
                    width -= _VScrollBar.Width;
                _HScrollBar.Bounds = new Rectangle(innerBounds.X, innerBounds.Bottom - _HScrollBar.Height + 1, width, _HScrollBar.Height);
            }

            if (_VScrollBar != null)
            {
                int height = innerBounds.Height;
                if (_HScrollBar != null)
                    height -= _HScrollBar.Height;
                _VScrollBar.Bounds = new Rectangle(innerBounds.Right - _VScrollBar.Width + 1, innerBounds.Y, _VScrollBar.Width, height);
            }

            if (_VScrollBar != null && _HScrollBar != null)
            {
                if (_Thumb == null)
                {
                    _Thumb = new Control();
                    if (this.BackColor.A == 255)
                        _Thumb.BackColor = this.BackColor;
                    else
                        _Thumb.BackColor = Color.White;
                    if (!this.BackgroundStyle.BackColor.IsEmpty && this.BackgroundStyle.BackColor.A == 255)
                        _Thumb.BackColor = this.BackgroundStyle.BackColor;
                    this.Controls.Add(_Thumb);
                }
                _Thumb.Bounds = new Rectangle(_HScrollBar.Bounds.Right, _VScrollBar.Bounds.Bottom, _VScrollBar.Width, _HScrollBar.Height);
                _Thumb.BringToFront();
            }
            else if (_Thumb != null)
            {
                this.Controls.Remove(_Thumb);
                _Thumb.Dispose();
                _Thumb = null;
            }
        }

        private void RemoveHScrollBar()
        {
            if (_HScrollBar != null)
            {
                Rectangle r = _HScrollBar.Bounds;
                this.Controls.Remove(_HScrollBar);
                _HScrollBar.Dispose();
                _HScrollBar = null;
                this.Invalidate(r);
                _AutoScrollPosition.X = 0;
                if (m_ItemContainer != null)
                    m_ItemContainer.NeedRecalcSize = true;
            }
        }

        private void RemoveVScrollBar()
        {
            if (_VScrollBar != null)
            {
                Rectangle r = _VScrollBar.Bounds;
                this.Controls.Remove(_VScrollBar);
                _VScrollBar.Dispose();
                _VScrollBar = null;
                this.Invalidate(r);
                _AutoScrollPosition.Y = 0;
                if (m_ItemContainer != null)
                    m_ItemContainer.NeedRecalcSize = true;
            }
        }

        private bool _SuspendPaint = false;
        /// <summary>
        /// Gets or sets whether all painting in control is suspended.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SuspendPaint
        {
            get { return _SuspendPaint; }
            set
            {
                _SuspendPaint = value;
            }
        }
        
        #endregion

        #region Licensing
#if !TRIAL
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_SuspendPaint) return;
            base.OnPaint(e);
            if (NativeFunctions.keyValidated2 != 266)
                TextDrawing.DrawString(e.Graphics, "Invalid License", this.Font, Color.FromArgb(180, Color.Red), this.ClientRectangle, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
        }

        private string m_LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return m_LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                m_LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#else
        protected override void OnPaint(PaintEventArgs e)
        {
            if (NativeFunctions.ColorExpAlt() || !NativeFunctions.CheckedThrough)
		    {
			    e.Graphics.Clear(SystemColors.Control);
                return;
            }
            if (_SuspendPaint) return;
            base.OnPaint(e);
        }
#endif
        #endregion

        #region Binding and Templating Support
        private BaseItem _ItemTemplate = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseItem ItemTemplate
        {
            get { return _ItemTemplate; }
            set { _ItemTemplate = value; OnItemTemplateChanged(); }
        }

        private void OnItemTemplateChanged()
        {
            
        }
        private BaseItem GetDefaultItemTemplate()
        {
            ButtonItem button = new ButtonItem();
            button.Shape = new RoundRectangleShapeDescriptor();
            button.OptionGroup = "items";
            button.GlobalItem = false;
            button.ColorTable = eButtonColor.Flat;
            return button;
        }

        /// <summary>
        /// Adds new item to the ItemPanel based on specified ItemTemplate and sets its Text property.
        /// </summary>
        /// <param name="text">Text to assign to the item.</param>
        /// <returns>reference to newly created item</returns>
        public BaseItem AddItem(string text)
        {
            if (_ItemTemplate == null)
                throw new NullReferenceException("ItemTemplate property not set.");
            BaseItem item = _ItemTemplate.Copy();
            item.Text = text;
            this.Items.Add(item);

            return item;
        }

#if (FRAMEWORK20)
        /// <summary>
        /// Gets the list of ButtonItem or CheckBoxItem controls that have their Checked property set to true.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.List<BaseItem> SelectedItems
        {
            get
            {
                System.Collections.Generic.List<BaseItem> items=new System.Collections.Generic.List<BaseItem>();
                foreach(BaseItem item in this.Items)
                {
                	ButtonItem button=item as ButtonItem;
                    if (button != null && button.Checked)
                        items.Add(button);
                    else
                    {
                        CheckBoxItem cb = item as CheckBoxItem;
                        if (cb != null && cb.Checked) items.Add(cb);
                    }
                }
                return items;
            }
        }
#endif
        #endregion

        #region IScrollableItemControl Members
        /// <summary>
        /// Indicates that item has been selected via keyboard.
        /// </summary>
        /// <param name="item">Reference to item being selected</param>
        void IScrollableItemControl.KeyboardItemSelected(BaseItem item)
        {
            if (item != null)
                EnsureVisible(item);
        }

        #endregion

        #region Misc Properties
        /// <summary>
        /// Gets or sets whether external ButtonItem object is accepted in drag and drop operation. UseNativeDragDrop must be set to true in order for this property to be effective.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
        public override bool AllowExternalDrop
        {
            get { return base.AllowExternalDrop; }
            set { base.AllowExternalDrop = value; }
        }

        /// <summary>
        /// Gets or sets whether native .NET Drag and Drop is used by control to perform drag and drop operations. AllowDrop must be set to true to allow drop of the items on control.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
        public override bool UseNativeDragDrop
        {
            get { return base.UseNativeDragDrop; }
            set { base.UseNativeDragDrop = value; }
        }

        /// <summary>
        /// Gets or sets whether automatic drag &amp; drop support is enabled. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Specifies whether automatic drag & drop support is enabled.")]
        public virtual bool EnableDragDrop
        {
            get { return m_EnableDragDrop; }
            set { m_EnableDragDrop = value; }
        }
        #endregion

        //#region INonClientControl Members

        //void DevComponents.DotNetBar.Controls.INonClientControl.BaseWndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //}

        //ItemPaintArgs DevComponents.DotNetBar.Controls.INonClientControl.GetItemPaintArgs(Graphics g)
        //{
        //    return GetItemPaintArgs(g);
        //}

        //ElementStyle DevComponents.DotNetBar.Controls.INonClientControl.BorderStyle
        //{
        //    get { return this.GetBackgroundStyle(); }
        //}

        //void DevComponents.DotNetBar.Controls.INonClientControl.PaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //}

        //IntPtr DevComponents.DotNetBar.Controls.INonClientControl.Handle
        //{
        //    get { return this.Handle; }
        //}

        //int DevComponents.DotNetBar.Controls.INonClientControl.Width
        //{
        //    get { return this.Width; }
        //}

        //int DevComponents.DotNetBar.Controls.INonClientControl.Height
        //{
        //    get { return this.Height; }
        //}

        //bool DevComponents.DotNetBar.Controls.INonClientControl.IsHandleCreated
        //{
        //    get { return this.IsHandleCreated; }
        //}

        //Point DevComponents.DotNetBar.Controls.INonClientControl.PointToScreen(Point client)
        //{
        //    return this.PointToScreen(client);
        //}

        //Color DevComponents.DotNetBar.Controls.INonClientControl.BackColor
        //{
        //    get { return this.BackColor; }
        //}
        //void DevComponents.DotNetBar.Controls.INonClientControl.AdjustClientRectangle(ref Rectangle r) { }
        //void DevComponents.DotNetBar.Controls.INonClientControl.AdjustBorderRectangle(ref Rectangle r) { }
        //void DevComponents.DotNetBar.Controls.INonClientControl.RenderNonClient(Graphics g) { }
        //#endregion
    }
}
