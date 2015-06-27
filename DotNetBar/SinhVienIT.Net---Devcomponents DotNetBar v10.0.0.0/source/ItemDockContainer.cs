using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines an container where you can arrange child elements added to SubItems collection either horizontally or vertically, relative to each other using SetDock and GetDock methods.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), ProvideProperty("Dock", typeof(BaseItem))]
    public class ItemDockContainer : ImageItem
    {
        #region Private Variables
        private ElementStyle _BackgroundStyle = new ElementStyle();
        private Rectangle _DesignerSelectionRectangle = Rectangle.Empty;
        private Size _EmptyDesignTimeSize = new Size(24, 24);
        #endregion
        #region Constructor


        /// <summary>
        /// Initializes a new instance of the ItemDockContainer class.
        /// </summary>
        public ItemDockContainer()
        {
            m_IsContainer = true;
            _BackgroundStyle.StyleChanged += new EventHandler(BackgroundStyleChanged);
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            ItemDockContainer objCopy = new ItemDockContainer();
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the ButtonItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ButtonItem instance.</param>
        internal void InternalCopyToItem(ButtonItem copy)
        {
            CopyToItem((BaseItem)copy);
        }

        /// <summary>
        /// Copies the ButtonItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            base.CopyToItem(copy);
            ItemDockContainer idc = (ItemDockContainer)copy;
            idc.LastChildFill = _LastChildFill;
        }

        /// <summary>
        /// Recalculates the size of the container. Assumes that DisplayRectangle.Location is set to the upper left location of this container.
        /// </summary>
        public override void RecalcSize()
        {
            if (this.SuspendLayout)
                return;

            if (this.SubItems.Count == 0 || this.VisibleSubItems == 0)
            {
                if (this.DesignMode && !this.SystemContainer)
                {
                    m_Rect.Size = GetEmptyDesignTimeSize();
                }
                else
                {
                    m_Rect = Rectangle.Empty;
                }
                base.RecalcSize();
                return;
            }

            m_Rect = LayoutItems(m_Rect);

            base.RecalcSize();
        }
        private BaseItem LastVisibleItem
        {
            get
            {
                int count = this.SubItems.Count - 1;
                for (int i = count; i >= 0; i--)
                {
                    BaseItem item = SubItems[i];
                    if (item.Visible)
                    {
                        return item;
                    }
                }
                return null;
            }
        }
        private System.Drawing.Rectangle LayoutItems(Rectangle bounds)
        {
            Rectangle itemsBounds = new Rectangle();
            BaseItem lastVisibleItem = LastVisibleItem;
            foreach (BaseItem item in m_SubItems)
            {
                if (!item.Visible)
                {
                    item.Displayed = false;
                    continue;
                }
                item.Displayed = true;
                item.RecalcSize();
                eItemDock dock = GetDock(item);
                if (_LastChildFill && item == lastVisibleItem)
                {
                    itemsBounds.Width += item.WidthInternal;
                    if (item.HeightInternal > itemsBounds.Height)
                        itemsBounds.Height = item.HeightInternal;
                }
                else if (dock == eItemDock.Left || dock == eItemDock.Right)
                {
                    itemsBounds.Width += item.WidthInternal;
                    if (item.HeightInternal > itemsBounds.Height)
                        itemsBounds.Height = item.HeightInternal;
                }
                else if (dock == eItemDock.Top || dock == eItemDock.Bottom)
                {
                    itemsBounds.Height += item.HeightInternal;
                    if (item.WidthInternal > itemsBounds.Width)
                        itemsBounds.Width = item.WidthInternal;
                }
            }

            if (bounds.Width < itemsBounds.Width)
                bounds.Width = itemsBounds.Width;
            if (bounds.Height < itemsBounds.Height)
                bounds.Height = itemsBounds.Height;

            if (!this.Stretch)
            {
                if (itemsBounds.Height < bounds.Height)
                    bounds.Height = itemsBounds.Height;
            }

            ArrangeItems(bounds);
            return bounds;
        }

        private void ArrangeItems(Rectangle bounds)
        {
            BaseItem lastVisibleItem = LastVisibleItem;
            Rectangle arrangeBounds = bounds;

            foreach (BaseItem item in m_SubItems)
            {
                if (!item.Visible)
                    continue;

                eItemDock dock = GetDock(item);
                if (_LastChildFill && item == lastVisibleItem)
                {
                    item.LeftInternal = arrangeBounds.Left;
                    item.TopInternal = arrangeBounds.Top;
                    item.WidthInternal = arrangeBounds.Width;
                    item.HeightInternal = arrangeBounds.Height;
                }
                else if (dock == eItemDock.Left)
                {
                    item.LeftInternal = arrangeBounds.Left;
                    item.TopInternal = arrangeBounds.Top;
                    item.HeightInternal = arrangeBounds.Height;
                    if (item.WidthInternal <= arrangeBounds.Width)
                    {
                        arrangeBounds.Width -= item.WidthInternal;
                        arrangeBounds.X += item.WidthInternal;
                    }
                    else if (arrangeBounds.Width > 0)
                    {
                        arrangeBounds.X += arrangeBounds.Width;
                        arrangeBounds.Width = 0;
                    }
                }
                else if (dock == eItemDock.Right)
                {
                    item.HeightInternal = arrangeBounds.Height;
                    if (item.WidthInternal <= arrangeBounds.Width)
                    {
                        item.LeftInternal = arrangeBounds.Right - item.WidthInternal;
                        item.TopInternal = arrangeBounds.Top;
                        arrangeBounds.Width -= item.WidthInternal;
                    }
                    else
                    {
                        item.LeftInternal = arrangeBounds.Left;
                        item.TopInternal = arrangeBounds.Top;
                        if (arrangeBounds.Width > 0)
                        {
                            arrangeBounds.X += arrangeBounds.Width;
                            arrangeBounds.Width = 0;
                        }
                    }
                }
                else if (dock == eItemDock.Top)
                {
                    item.LeftInternal = arrangeBounds.Left;
                    item.TopInternal = arrangeBounds.Top;
                    item.WidthInternal = arrangeBounds.Width;
                    if (item.HeightInternal <= arrangeBounds.Height)
                    {
                        arrangeBounds.Height -= item.HeightInternal;
                        arrangeBounds.Y += item.HeightInternal;
                    }
                    else if (arrangeBounds.Height > 0)
                    {
                        arrangeBounds.Y += arrangeBounds.Height;
                        arrangeBounds.Height = 0;
                    }
                }
                else if (dock == eItemDock.Bottom)
                {
                    item.WidthInternal = arrangeBounds.Width;
                    if (item.HeightInternal <= arrangeBounds.Height)
                    {
                        item.LeftInternal = arrangeBounds.Left;
                        item.TopInternal = arrangeBounds.Bottom - item.HeightInternal;
                        arrangeBounds.Height -= item.HeightInternal;
                    }
                    else
                    {
                        item.LeftInternal = arrangeBounds.Left;
                        item.TopInternal = arrangeBounds.Top;
                        if (arrangeBounds.Height > 0)
                        {
                            arrangeBounds.Y += arrangeBounds.Height;
                            arrangeBounds.Height = 0;
                        }
                    }
                }
            }
        }

        protected override void OnExternalSizeChange()
        {
            if (!this.SuspendLayout)
                ArrangeItems(m_Rect);
            base.OnExternalSizeChange();
        }

        /// <summary>
        /// Called after TopInternal property has changed
        /// </summary>
        protected override void OnTopLocationChanged(int oldValue)
        {
            base.OnTopLocationChanged(oldValue);
            Point offset = new Point(0, this.TopInternal - oldValue);
            OffsetSubItems(offset);
        }

        /// <summary>
        /// Called after LeftInternal property has changed
        /// </summary>
        protected override void OnLeftLocationChanged(int oldValue)
        {
            base.OnLeftLocationChanged(oldValue);
            Point offset = new Point(this.LeftInternal - oldValue, 0);
            OffsetSubItems(offset);
        }

        private bool _Offsetting = false;
        internal void OffsetSubItems(Point offset)
        {
            if (_Offsetting || offset.IsEmpty)
                return;

            try
            {
                _Offsetting = true;
                BaseItem[] items = new BaseItem[this.SubItems.Count];
                this.SubItems.CopyTo(items, 0);
                foreach (BaseItem b in items)
                {
                    Rectangle r = b.Bounds;
                    r.Offset(offset);
                    b.Bounds = r;
                }
            }
            finally
            {
                _Offsetting = false;
            }
        }

        /// <summary>
        /// Must be overridden by class that is inheriting to provide the painting for the item.
        /// </summary>
        public override void Paint(ItemPaintArgs p)
        {
            Graphics g = p.Graphics;
            Region oldClip = null;
            bool clipSet = false;

            PaintBackground(p);

            Rectangle clip = GetClipRectangle();
            oldClip = g.Clip;
            g.SetClip(clip, CombineMode.Intersect);
            clipSet = true;

            ItemDisplay display = GetItemDisplay();
            display.Paint(this, p);

            if (clipSet)
            {
                if (oldClip != null)
                    g.Clip = oldClip;
                else
                    g.ResetClip();
            }

            if (this.DesignMode && !this.SystemContainer && p.DesignerSelection)
            {
                Rectangle r = this.DisplayRectangle;

                Pen pen = null;
                if (this.Focused)
                    pen = new Pen(Color.FromArgb(190, Color.Navy), 1);
                else
                    pen = new Pen(Color.FromArgb(80, Color.Black), 1);

                pen.DashStyle = DashStyle.Dot;
                DisplayHelp.DrawRoundedRectangle(g, pen, r, 3);
                pen.Dispose();
                pen = null;

                Image image = BarFunctions.LoadBitmap("SystemImages.AddMoreItemsContainer.png");

                if (this.Parent is ItemContainer && !((ItemContainer)this.Parent).SystemContainer)
                {
                    ItemContainer pc = this.Parent as ItemContainer;
                    while (pc != null && !pc.SystemContainer)
                    {
                        if (new Rectangle(pc.DisplayRectangle.Location, image.Size).Contains(this.DisplayRectangle.Location))
                        {
                            if (r.X + image.Width + 1 > this.DisplayRectangle.Right)
                            {
                                r.X = this.DisplayRectangle.X;
                                if (r.Y + image.Height < this.DisplayRectangle.Bottom)
                                    r.Y += image.Height + 1;
                                else
                                    break;
                            }
                            r.X += (image.Width + 1);
                        }
                        pc = pc.Parent as ItemContainer;
                    }
                }

                _DesignerSelectionRectangle = new Rectangle(r.X + 1, r.Y + 1, image.Width, image.Height);
                g.DrawImageUnscaled(image, _DesignerSelectionRectangle.Location);
            }

            this.DrawInsertMarker(p.Graphics);
        }
        private ItemDisplay _ItemDisplay = null;
        internal ItemDisplay GetItemDisplay()
        {
            if (_ItemDisplay == null)
                _ItemDisplay = new ItemDisplay();
            return _ItemDisplay;
        }
        protected virtual void PaintBackground(ItemPaintArgs p)
        {
            _BackgroundStyle.SetColorScheme(p.Colors);
            Graphics g = p.Graphics;
            ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(_BackgroundStyle, g, this.DisplayRectangle));
        }

        private void BackgroundStyleChanged(object sender, EventArgs e)
        {
            this.OnAppearanceChanged();
        }

        protected virtual Rectangle GetClipRectangle()
        {
            Rectangle clip = this.DisplayRectangle;
            bool disposeStyle = false;
            ElementStyle style = ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
            clip.X += ElementStyleLayout.LeftWhiteSpace(style);
            clip.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(style);
            clip.Y += ElementStyleLayout.TopWhiteSpace(style);
            clip.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(style);
            if (disposeStyle)
                style.Dispose();
            return clip;
        }

        private Dictionary<BaseItem, eItemDock> _ItemDock = new Dictionary<BaseItem, eItemDock>();
        /// <summary>
        /// Retrieves the Balloon Caption text associated with the specified control.
        /// </summary>
        [DefaultValue(eItemDock.Left)]
        public eItemDock GetDock(BaseItem item)
        {
            if (item == null) throw new ArgumentNullException("item cannot be null");
            eItemDock dock = eItemDock.Left;
            if (_ItemDock.TryGetValue(item, out dock))
                return dock;
            return eItemDock.Left;
        }

        /// <summary>
        /// Associates Balloon Caption text with the specified control.
        /// </summary>
        /// <param name="control">The Control to associate the Balloon Caption text with.</param>
        /// <param name="value">The Balloon Caption text to display on the Balloon.</param>
        [Localizable(true)]
        public void SetDock(BaseItem item, eItemDock dock)
        {
            if (item == null) throw new ArgumentNullException("item cannot be null");
            if (dock == eItemDock.Left)
            {
                if (_ItemDock.ContainsKey(item))
                    _ItemDock.Remove(item);
            }
            else
            {
                if (_ItemDock.ContainsKey(item))
                    _ItemDock[item] = dock;
                else
                    _ItemDock.Add(item, dock);
            }
        }

        /// <summary>
        /// Returns empty container default design-time size.
        /// </summary>
        /// <returns>Size of an empty container.</returns>
        protected virtual Size GetEmptyDesignTimeSize()
        {
            Size s = _EmptyDesignTimeSize;
            return s;
        }


        private bool _LastChildFill = true;
        /// <summary>
        /// Gets or sets a value that indicates whether the last child element within a ItemDockContainer stretches to fill the remaining available space.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether the last child element within a ItemDockContainer stretches to fill the remaining available space.")]
        public bool LastChildFill
        {
            get { return _LastChildFill; }
            set
            {
                _LastChildFill = value;
                NeedRecalcSize = true;
                OnAppearanceChanged();
            }
        }

        private bool _SystemContainer = false;
        [Browsable(false)]
        public bool SystemContainer
        {
            get { return _SystemContainer; }
            internal set
            {
                _SystemContainer = value;
            }
        }

        /// <summary>
        /// IBlock member implementation
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override System.Drawing.Rectangle Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                Point offset = new Point(value.X - m_Rect.X, value.Y - m_Rect.Y);
                base.Bounds = value;

                if (!offset.IsEmpty)
                {
                    OffsetSubItems(offset);
                }
            }
        }
        #endregion
    }

    public enum eItemDock
    {
        Left,
        Right,
        Top,
        Bottom
    }
}
