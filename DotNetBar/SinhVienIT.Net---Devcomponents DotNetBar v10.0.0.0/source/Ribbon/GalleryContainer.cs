using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.UI.ContentManager;
using System.Collections;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the class that provides Office 2007 style Gallery container with drop-down ability.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), ProvideProperty("GalleryGroup", typeof(BaseItem)), Designer("DevComponents.DotNetBar.Design.GalleryContainerDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class GalleryContainer : ItemContainer, IExtenderProvider
    {
        #region Private Variables
        private bool m_EnableGalleryPopup = true;
        private ButtonItem m_ScrollUp = null;
        private ButtonItem m_ScrollDown = null;
        private ButtonItem m_PopupGallery = null;
        private Size m_ScrollButtonSize = new Size(15, 20);
        private int m_ScrollButtonContentSpacing = 1;
        private Size m_DefaultSize = Size.Empty;
        private Point m_ScrollPosition = Point.Empty;
        private Size m_PopupGallerySize = Size.Empty;
        private ScrollBar.ScrollBarCore m_ScrollBar = null;
        private bool m_PopupGalleryStyle = false;
        private Rectangle m_ViewRectangle = Rectangle.Empty;
        private Size m_RecommendedSize = Size.Empty;
        private bool m_StretchGallery = false;
        internal bool SystemGallery = false;
        private bool m_IncrementalSizing = true;
        private GalleryGroupCollection m_Groups = null;
        private bool m_ScrollAnimation = true;
        private ThreadUIOperation m_ThreadUI = null;
        private bool m_PopupUsesStandardScrollbars = true;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Gallery popup item is about to open.
        /// </summary>
        [System.ComponentModel.Description("Occurs when Gallery popup item is about to open.")]
        public event DotNetBarManager.PopupOpenEventHandler GalleryPopupOpen;

        /// <summary>
        /// Occurs just before Gallery popup window is shown.
        /// </summary>
        [System.ComponentModel.Description("Occurs just before Gallery popup window is shown.")]
        public event EventHandler GalleryPopupShowing;

        /// <summary>
        /// Occurs before the Gallery popup item is closed.
        /// </summary>
        [System.ComponentModel.Description("Occurs before the Gallery popup item is closed")]
        public event EventHandler GalleryPopupClose;

        /// <summary>
        /// Occurs after Gallery popup item has been closed.
        /// </summary>
        [System.ComponentModel.Description("Occurs after popup item has been closed.")]
        public event EventHandler GalleryPopupFinalized;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the class
        /// </summary>
        public GalleryContainer()
            : base()
        {
            m_DefaultSize = GetDefaultSize();
            m_PopupGallerySize = GetDefaultPopupSize();
            m_Groups = new GalleryGroupCollection();
            m_Groups.Owner = this;
            this.MultiLine = true;
            //this.BackgroundStyle.Class = Rendering.ElementStyleClassKeys.RibbonGalleryContainerKey;
            CreateButtons();
            this.MinimumSize = new Size(42 + m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing, m_DefaultSize.Height);
            m_ThreadUI = new ThreadUIOperation(new EventHandler(this.RecordStartState), new EventHandler(this.UpdateThreadedUI), new EventHandler(this.CleanupState));
        }

        private Size GetDefaultSize()
        {
            return new Size(140 + m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing, m_ScrollButtonSize.Height * 3 - 2);
        }

        private Size GetDefaultPopupSize()
        {
            return Size.Empty;
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            GalleryContainer objCopy = new GalleryContainer();
            this.CopyToItem(objCopy);
            return objCopy;
        }
        /// <summary>
        /// Copies the ButtonItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="c">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem c)
        {
            GalleryContainer copy = c as GalleryContainer;
            copy.DefaultSize = this.DefaultSize;
            copy.PopupGallerySize = this.PopupGallerySize;
            copy.StretchGallery = this.StretchGallery;
            copy.PopupUsesStandardScrollbars = this.PopupUsesStandardScrollbars;

            foreach (BaseItem item in this.PopupGalleryItems)
                copy.PopupGalleryItems.Add(item.Copy());

            

            base.CopyToItem(copy);

            // Copy Gallery Groups
            foreach (GalleryGroup group in this.GalleryGroups)
            {
                GalleryGroup groupCopy = new GalleryGroup();
                groupCopy.Name = group.Name;
                groupCopy.Text = group.Text;
                groupCopy.DisplayOrder = group.DisplayOrder;
                copy.GalleryGroups.Add(groupCopy);

                foreach (BaseItem item in group.Items)
                {
                    copy.SetGalleryGroup(copy.SubItems[item.Name], groupCopy);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (m_ScrollBar != null)
            {
                m_ScrollBar.Dispose();
                m_ScrollBar = null;
            }
            m_ThreadUI.Stop();
            base.Dispose(disposing);
        }

        private void CreateButtons()
        {
            m_ScrollUp = new ButtonItem("sysgalleryscrollup");
            m_ScrollUp.Text = "<expand direction=\"top\"/>";
            m_ScrollUp.Style = this.Style;
            m_ScrollUp.ColorTable = eButtonColor.OrangeWithBackground;
            m_ScrollUp.SetParent(this);
            m_ScrollUp.Click += new EventHandler(ScrollUp_Click);
            m_ScrollUp.ClickAutoRepeat = true;
            m_ScrollUp.ClickRepeatInterval = 200;
            m_ScrollUp.CanCustomize = false;
            m_ScrollUp.AutoCollapseOnClick = false;

            m_ScrollDown = new ButtonItem("sysgalleryscrolldown");
            m_ScrollDown.Text = "<expand direction=\"bottom\"/>";
            m_ScrollDown.Style = this.Style;
            m_ScrollDown.ColorTable = eButtonColor.OrangeWithBackground;
            m_ScrollDown.SetParent(this);
            m_ScrollDown.Click += new EventHandler(ScrollDown_Click);
            m_ScrollDown.ClickAutoRepeat = true;
            m_ScrollDown.ClickRepeatInterval = 200;
            m_ScrollDown.CanCustomize = false;
            m_ScrollDown.AutoCollapseOnClick = false;

            m_PopupGallery = new ButtonItem("sysgallerypopup");
            m_PopupGallery.Text = "<expand direction=\"popup\"/>";
            m_PopupGallery.ButtonStyle = eButtonStyle.TextOnlyAlways;
            m_PopupGallery.Style = this.Style;
            m_PopupGallery.ColorTable = eButtonColor.OrangeWithBackground;
            m_PopupGallery.SetParent(this);
            m_PopupGallery.Click += new EventHandler(PopupGallery_Click);
            m_PopupGallery.CanCustomize = false;
            m_PopupGallery.PopupOpen += new DotNetBarManager.PopupOpenEventHandler(PopupGallery_PopupOpen);
            m_PopupGallery.PopupShowing += new EventHandler(PopupGallery_PopupShowing);
            m_PopupGallery.PopupClose += new EventHandler(PopupGallery_PopupClose);
            m_PopupGallery.PopupFinalized += new EventHandler(PopupGallery_PopupFinalized);
        }

        private void PopupGallery_PopupShowing(object sender, EventArgs e)
        {
            OnGalleryPopupShowing(sender, e);
        }

        private void PopupGallery_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            OnGalleryPopupOpen(sender, e);
        }
        
        private void PopupGallery_Click(object sender, EventArgs e)
        {
            PopupGallery();
        }

        private Size GetPopupGallerySize(Point screenPos)
        {
            Size size = Size.Empty;
            if (this.DisplayRectangle.Width < this.DefaultSize.Width || m_ViewRectangle.Size.IsEmpty)
            {
                int width = this.DefaultSize.Width;
                int itemBasedWidth = 0;
                foreach (BaseItem item in this.SubItems)
                {
                    if (!item.Visible) continue;
                    itemBasedWidth += item.WidthInternal + this.ItemSpacing;
                    if (itemBasedWidth > width)
                        break;
                }
                if (itemBasedWidth > 0)
                {
                    width = itemBasedWidth;
                    if (m_PopupUsesStandardScrollbars)
                        width += m_ScrollButtonContentSpacing + m_ScrollButtonSize.Width;
                }
                size = new Size(width, Math.Max(this.DefaultSize.Height, m_ViewRectangle.Height));
            }
            else
                size = new Size(this.DisplayRectangle.Width, m_ViewRectangle.Height);

            ScreenInformation screenInfo = null;
            screenInfo = BarFunctions.ScreenFromPoint(screenPos);
            
            // Give the grouped gallery max size since we do not know yet how tall it is
            if (m_Groups.Count > 0 && screenInfo!=null && screenInfo.WorkingArea.Bottom - screenPos.Y>size.Height)
            {
                size.Height = screenInfo.WorkingArea.Bottom - screenPos.Y;
            }

            if (screenInfo != null)
            {
                int red = 16;
                red += this.PopupGalleryItems.Count * 26;
                int yDiff= 0, xDiff = 0;
                if (screenPos.Y + size.Height > screenInfo.WorkingArea.Bottom - red)
                    yDiff = screenPos.Y + size.Height - (screenInfo.WorkingArea.Bottom - red);
                if (screenPos.X + size.Width > screenInfo.WorkingArea.Right - red)
                    xDiff = screenPos.X + size.Width - (screenInfo.WorkingArea.Right - red);
                if (size.Width - xDiff > this.MinimumSize.Width)
                    size.Width -= xDiff;
                else
                    size.Width = this.MinimumSize.Width + m_ScrollButtonContentSpacing + m_ScrollButtonSize.Width;
                if (size.Height - yDiff > this.MinimumSize.Height)
                    size.Height -= yDiff;
                else
                    size.Height = this.MinimumSize.Height;
            }

            return size;
        }

        internal bool PopupGalleryStyle
        {
            get { return m_PopupGalleryStyle; }
            set
            {
                m_PopupGalleryStyle = value;
                
                if (m_ScrollBar != null)
                {
                    m_ScrollBar.Dispose();
                    m_ScrollBar = null;
                }

                if (m_PopupGalleryStyle)
                {
                    if (m_PopupUsesStandardScrollbars)
                    {
                        m_ScrollBar = new ScrollBar.ScrollBarCore();
                        m_ScrollBar.ValueChanged += new EventHandler(PopupScrollBar_ValueChanged);
                    }
                }
            }
        }

        ///// <summary>
        ///// Adds the items that are not visible to the overflow popup.
        ///// </summary>
        //private void AddItems(GalleryContainer c)
        //{
        //    BaseItem[] items = new BaseItem[this.SubItems.Count];
        //    this.SubItems.CopyTo(items, 0);
        //    foreach (BaseItem objItem in items)
        //    {
        //        c.SubItems._Add(objItem);
        //        this.SubItems._Remove(objItem);
        //        objItem.SetParent(c);
        //        objItem.ContainerControl = null;
        //    }

        //    items = new BaseItem[m_PopupGalleryItems.Count];
        //    m_PopupGalleryItems.CopyTo(items, 0);
        //    foreach (BaseItem objItem in items)
        //    {
        //        m_PopupGallery.SubItems._Add(objItem);
        //        m_PopupGalleryItems._Remove(objItem);
        //        objItem.SetParent(c);
        //        objItem.ContainerControl = null;
        //    }

        //    c.NeedRecalcSize = true;
        //}

        private void ClearPopupGalleryItems()
        {
            if (!this.DesignMode)
            {
                // When copy is made this ensures the proper cleanup
                foreach (BaseItem item in m_PopupGallery.SubItems)
                {
                    GalleryContainer gc = item as GalleryContainer;
                    if (gc != null && gc.SystemGallery)
                    {
                        m_PopupGallery.SubItems.Remove(gc);
                        gc.Dispose();
                        break;
                    }
                }
            }
        }

        private void PopupGallery_PopupFinalized(object sender, EventArgs e)
        {
            OnGalleryPopupFinalized(sender, e);
            ClearPopupGalleryItems();
        }

        private void PopupGallery_PopupClose(object sender, EventArgs e)
        {
            OnGalleryPopupClose(sender, e);
            RestoreGalleryItems();
        }

        private void RestoreGalleryItems()
        {
            //GalleryContainer c = m_PopupGallery.SubItems[0] as GalleryContainer;
            //if (c == null) return;
            //BaseItem[] items = new BaseItem[c.SubItems.Count];
            //c.SubItems.CopyTo(items, 0);
            //foreach (BaseItem item in items)
            //{
            //    this.SubItems._Add(item);
            //    c.SubItems._Remove(item);
            //    item.SetParent(this);
            //    item.ContainerControl = null;
            //}

            //for (int i = 1; i < m_PopupGallery.SubItems.Count; i++)
            //{
            //    BaseItem item = m_PopupGallery.SubItems[i];
            //    m_PopupGalleryItems._Add(item);
            //    item.SetParent(this);
            //    item.ContainerControl = null;
            //}

            //m_PopupGallery.SubItems._Clear();
            //this.NeedRecalcSize = true;
        }

        private void ScrollUp_Click(object sender, EventArgs e)
        {
            this.ScrollUp();
        }

        private void ScrollDown_Click(object sender, EventArgs e)
        {
            this.ScrollDown();
        }

        internal bool ScrollHitTest(int x, int y)
        {
            if (m_ScrollUp.Enabled && m_ScrollUp.DisplayRectangle.Contains(x, y))
                return true;
            else if (m_ScrollDown.Enabled && m_ScrollDown.DisplayRectangle.Contains(x,y))
                return true;
            else if (m_ScrollBar != null && m_ScrollBar.Visible)
            {
                if (m_ScrollBar.ThumbIncreaseRectangle.Contains(x,y))
                    return true;
                else if (m_ScrollBar.ThumbDecreaseRectangle.Contains(x,y))
                    return true;
            }
            return false;
        }

        private bool m_MouseDownOnScrollBar = false;
        public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
        {
            if (this.IsDisposed) return;

            m_MouseDownOnScrollBar = false;
            if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars)
            {
                m_ScrollBar.MouseDown(objArg);
                if (m_ScrollBar.DisplayRectangle.Contains(objArg.X, objArg.Y))
                    m_MouseDownOnScrollBar = true;
            }

            base.InternalMouseDown(objArg);

            if (this.DesignMode)
            {
                if (m_ScrollUp.Enabled && m_ScrollUp.DisplayRectangle.Contains(objArg.X, objArg.Y))
                    ScrollUp();
                else if (m_ScrollDown.Enabled && m_ScrollDown.DisplayRectangle.Contains(objArg.X, objArg.Y))
                    ScrollDown();
                else if (m_PopupGallery.Enabled && m_PopupGallery.DisplayRectangle.Contains(objArg.X, objArg.Y))
                    PopupGallery();
                else if (m_ScrollBar != null && m_ScrollBar.Visible)
                {
                    if(m_ScrollBar.ThumbIncreaseRectangle.Contains(objArg.X, objArg.Y))
                        ScrollDown();
                    else if (m_ScrollBar.ThumbDecreaseRectangle.Contains(objArg.X, objArg.Y))
                        ScrollUp();
                }
            }
        }

        public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
        {
            if (m_PopupGalleryStyle && !this.IsDisposed)
            {
                if (m_PopupUsesStandardScrollbars)
                    m_ScrollBar.MouseMove(objArg);
            }

            base.InternalMouseMove(objArg);
        }

        public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
        {
            if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars && !this.IsDisposed)
            {
                m_ScrollBar.MouseUp(objArg);
                if (m_ScrollBar.DisplayRectangle.Contains(objArg.X, objArg.Y) || m_MouseDownOnScrollBar)
                {
                    m_MouseDownOnScrollBar = false;
                    return;
                }
            }
            base.InternalMouseUp(objArg);
        }

        public override void InternalMouseLeave()
        {
            if (m_PopupGalleryStyle && !this.IsDisposed)
            {
                if (m_PopupUsesStandardScrollbars)
                    m_ScrollBar.MouseLeave();
                //else
                //{
                //    if (m_ScrollUp.IsMouseOver)
                //        m_ScrollUp.InternalMouseLeave();
                //    if (m_ScrollDown.IsMouseOver)
                //        m_ScrollDown.InternalMouseLeave();
                //}
            }
            base.InternalMouseLeave();
        }

        public override void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
        {
            if (this.IsGalleryPopupOpen)
            {
                m_PopupGallery.InternalKeyDown(objArg);
                return;
            }

            //BaseItem oldHotSubItem = this.HotSubItem;
            base.InternalKeyDown(objArg);
            // Check whether new hot item is out of bounds and if it is scroll it into the view
            //Rectangle displayRect = GetClientRectangle();
            //if (this.HotSubItem != oldHotSubItem && this.HotSubItem != null && !displayRect.Contains(this.HotSubItem.Bounds))
            //{
            //    EnsureVisible(this.HotSubItem);
            //}
        }

        protected override void OnHotSubItemChanged(BaseItem newValue, BaseItem oldValue)
        {
            base.OnHotSubItemChanged(newValue, oldValue);

            if (newValue != null && newValue != oldValue)
            {
                Rectangle displayRect = GetClientRectangle();
                if (!displayRect.Contains(newValue.Bounds))
                {
                    EnsureVisible(newValue);
                }
            }
        }

        /// <summary>
        /// Returns the client rectangle which is DisplayRectangle excluding scroll-bar bounds
        /// </summary>
        /// <returns></returns>
        private Rectangle GetClientRectangle()
        {
            Rectangle r = this.DisplayRectangle;
            if (m_ScrollBar != null)
                r.Width -= m_ScrollBar.DisplayRectangle.Width;

            if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
            {
                if (m_ScrollUp != null)
                {
                    r.Y += m_ScrollUp.HeightInternal;
                    r.Height -= m_ScrollUp.HeightInternal;
                }
                if (m_ScrollDown != null)
                {
                    r.Height -= m_ScrollDown.HeightInternal;
                }
            }
            else
            {
                if (m_ScrollUp != null)
                    r.Width -= m_ScrollUp.WidthInternal;
            }

            return r;
        }

        internal Size RecommendedSize
        {
            get { return m_RecommendedSize;  }
            set { m_RecommendedSize = value;  }
        }

        /// <summary>
        /// Invokes GalleryPopupOpen event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGalleryPopupOpen(object sender, PopupOpenEventArgs e)
        {
            if (GalleryPopupOpen != null)
                GalleryPopupOpen(sender, e);
        }

        /// <summary>
        /// Invokes GalleryPopupShowing event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGalleryPopupShowing(object sender, EventArgs e)
        {
            if (GalleryPopupShowing != null)
                GalleryPopupShowing(sender, e);
        }

        /// <summary>
        /// Invokes GalleryPopupClose event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGalleryPopupClose(object sender, EventArgs e)
        {
            if (GalleryPopupClose != null)
                GalleryPopupClose(sender, e);
        }

        /// <summary>
        /// Invokes GalleryPopupFinalized event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGalleryPopupFinalized(object sender, EventArgs e)
        {
            if (GalleryPopupFinalized != null)
                GalleryPopupFinalized(sender, e);
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle PopupGalleryButtonBounds
        {
            get { return m_PopupGallery.DisplayRectangle; }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonItem PopupGalleryItem
        {
            get { return m_PopupGallery; }
        }
        #endregion

        #region Property Hidding
        /// <summary>
        /// Gets or sets orientation inside the container. GalleryContainer automatically manages the layout orientation and this property should not be changed from its default value.
        /// </summary>
        [Browsable(false), DefaultValue(eOrientation.Horizontal), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Layout")]
        public override eOrientation LayoutOrientation
        {
            get { return base.LayoutOrientation; }
            set { base.LayoutOrientation = value; }
        }

        /// <summary>
        /// Gets or sets the item alignment when container is in horizontal layout. Default value is Left.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override eHorizontalItemsAlignment HorizontalItemAlignment
        {
            get { return base.HorizontalItemAlignment; }
            set { base.HorizontalItemAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the item vertical alignment. Default value is Top.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override eVerticalItemsAlignment VerticalItemAlignment
        {
            get { return base.VerticalItemAlignment; }
            set { base.VerticalItemAlignment = value; }
        }

        /// <summary>
        /// Gets or sets whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Layout"), Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
        public override bool MultiLine
        {
            get { return base.MultiLine; }
            set { base.MultiLine = value; }
        }

        /// <summary>
        /// Gets or sets whether items contained by container are resized to fit the container bounds. When container is in horizontal
        /// layout mode then all items will have the same height. When container is in vertical layout mode then all items
        /// will have the same width. Default value is true.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool ResizeItemsToFit
        {
            get { return base.ResizeItemsToFit; }
            set { base.ResizeItemsToFit = value; }
        }
        #endregion

        #region Public Interface
        /// <summary>
        /// Gets or sets whether Gallery when on popup is using standard scrollbars to scroll the content.
        /// Standard scrollbars are displayed on right hand side of the Gallery. Default value for this property is true.
        /// When set to false the scroll buttons are displayed only when needed and two buttons on top and bottom
        /// of the Gallery are used to indicate the scrolling possibility and enable scrolling. Buttons are only
        /// visible when needed. This scrolling button style can be used for example on Application Menu
        /// to enable scrolling of list of most recently used files.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true), Description("Indicates whether Gallery when on popup is using standard scrollbars to scroll the content.")]
        public bool PopupUsesStandardScrollbars
        {
            get { return m_PopupUsesStandardScrollbars; }
            set
            {
                m_PopupUsesStandardScrollbars = value;
            }
        }

        /// <summary>
        /// Gets or sets whether gallery is using incremental sizing when stretched. Default
        /// value is true. Incremental sizing will resize the width of the gallery so it fits
        /// completely the items it can contain in available space. That means that gallery will
        /// occupy enough space to display the whole items within available space. When set to
        /// false, it indicates that gallery will resize to fill all available space.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Layout"), Description("Indicates whether gallery is using incremental sizing when stretched.")]
        public bool IncrementalSizing
        {
            get { return m_IncrementalSizing; }
            set
            {
                m_IncrementalSizing = value;
                this.NeedRecalcSize = true;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether Gallery width is determined based on the RibbonBar width. This property is in effect when
        /// Gallery is hosted directly the RibbonBar control. Default value is false.
        /// When set to true the Gallery size is changed as the RibbonBar control is resized. The initial size of the Gallery is
        /// determined by DefaultSize property. The MinimumSize property specifies the minimum size of the Gallery.
        /// Note that only single Gallery can be stretched per RibbonBar control.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Layout"), Description("Indicates whether Gallery width is determined based on the Ribbon container size.")]
        public bool StretchGallery
        {
            get { return m_StretchGallery; }
            set
            {
                if (m_StretchGallery != value)
                {
                    m_StretchGallery = value;
                    
                    RibbonBar bar = this.ContainerControl as RibbonBar;
                    if (bar != null)
                    {
                        if (m_StretchGallery)
                        {
                            if (bar.GalleryStretch == null)
                                bar.GalleryStretch = this;
                        }
                        else
                        {
                            if (bar.GalleryStretch == this)
                                bar.GalleryStretch = null;
                        }
                    }

                    NeedRecalcSize = true;
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets the collection of the items that are added to the popup gallery. The items displayed on the gallery are combined with the
        /// items from this collection and they are displayed on the gallery popup. This collection can for example have items that are
        /// customizing the choices of the commands in gallery etc.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public SubItemsCollection PopupGalleryItems
        {
            get { return m_PopupGallery.SubItems; }
        }
        /// <summary>
        /// Gets or sets the default size of the gallery. The gallery height will be always enforced so all scroll buttons can be displayed.
        /// Gallery width should allow display of both scroll buttons and the gallery content.
        /// </summary>
        [Browsable(true), Category("Layout"), Description("Indicates the default size of the gallery.")]
        public Size DefaultSize
        {
            get { return m_DefaultSize; }
            set
            {
                m_DefaultSize = value;
                this.NeedRecalcSize = true;
                this.OnAppearanceChanged();
            }
        }
        /// <summary>
        /// Gets whether DefaultSize property is serialized by Windows Forms designer based on its current value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDefaultSize()
        {
            return (m_DefaultSize != GetDefaultSize());
        }
        /// <summary>
        /// Resets DefaultSize property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDefaultSize()
        {
            TypeDescriptor.GetProperties(this)["DefaultSize"].SetValue(this, GetDefaultSize());
        }

        /// <summary>
        /// Gets or sets the default size of the gallery when gallery is displayed on the popup menu.
        /// </summary>
        [Browsable(true), Category("Layout"), Description("Indicates the default size of the gallery when gallery is displayed on the popup menu.")]
        public Size PopupGallerySize
        {
            get { return m_PopupGallerySize; }
            set { m_PopupGallerySize = value; }
        }
        /// <summary>
        /// Gets whether PopupGallerySize property is serialized by Windows Forms designer based on its current value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePopupGallerySize()
        {
            return (m_PopupGallerySize != GetDefaultPopupSize());
        }
        /// <summary>
        /// Resets PopupGallerySize property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPopupGallerySize()
        {
            TypeDescriptor.GetProperties(this)["PopupGallerySize"].SetValue(this, GetDefaultPopupSize());
        }

        /// <summary>
        /// Gets or sets whether gallery can be displayed on the popup. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Gallery"), Description("Indicates whether gallery can be displayed on the popup.")]
        public bool EnableGalleryPopup
        {
            get { return m_EnableGalleryPopup; }
            set
            {
                m_EnableGalleryPopup = value;
                this.NeedRecalcSize = true;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Scrolls the gallery if necessary to ensures that item is visible.
        /// </summary>
        /// <param name="item">Reference to the items that is part of the gallery.</param>
        public void EnsureVisible(BaseItem item)
        {
            if (item == null) throw new NullReferenceException("Item must be non null value.");

            int scrollPositionY = GetScrollPosition().Y;
            Rectangle clientRectangle = GetClientRectangle();
            if (clientRectangle.IsEmpty) return;

            if (item.TopInternal + scrollPositionY < clientRectangle.Y || item.Bounds.Bottom > clientRectangle.Bottom)
            {
                int i = clientRectangle.Top - item.TopInternal + scrollPositionY;
                if (item.Bounds.Bottom > clientRectangle.Bottom)
                    i = -((item.Bounds.Bottom - clientRectangle.Y) - clientRectangle.Height) + scrollPositionY;
                if (Math.Abs(i) <= 4) i = 0;
                SetScrollPosition(i);
            }
        }

        /// <summary>
        /// Scrolls gallery down to show next line of items.
        /// </summary>
        public void ScrollDown()
        {
            SetScrollPosition(GetNextScrollLine());
        }

        /// <summary>
        /// Scrolls gallery up to show previous line of items.
        /// </summary>
        public void ScrollUp()
        {
            SetScrollPosition(GetPreviousScrollLine());
        }
        #endregion

        #region Internal Implementation
        public override BaseItem ItemAtLocation(int x, int y)
        {
            if (!this.DesignMode && (!m_PopupGalleryStyle || !m_PopupUsesStandardScrollbars))
            {
                if (m_ScrollUp.DisplayRectangle.Contains(x, y) && m_ScrollUp.Visible)
                    return m_ScrollUp;
                else if (m_ScrollDown.DisplayRectangle.Contains(x, y) && m_ScrollDown.Visible)
                    return m_ScrollDown;
                else if (m_EnableGalleryPopup && m_PopupGallery.DisplayRectangle.Contains(x, y))
                    return m_PopupGallery;
            }
            else if (!this.DesignMode && m_PopupGalleryStyle && m_PopupUsesStandardScrollbars && !SystemGallery && IsOnPopup(this))
            {
                if (m_ScrollBar != null && (m_ScrollBar.DisplayRectangle.Contains(x, y) || m_ScrollBar.IsMouseDown)) return this;
            }

            if (m_MouseDownOnScrollBar)
                return this;
            return base.ItemAtLocation(x, y);
        }

        internal ScrollBar.ScrollBarCore ScrollBarControl
        {
            get
            {
                return m_ScrollBar;
            }
        }

        void PopupScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!_SettingScrollPosition)
                SetScrollPosition(-m_ScrollBar.Value);
        }

        private bool _SettingScrollPosition = false;
        private void SetScrollPosition(int y)
        {
            if (m_ScrollPosition.Y == y) return;

            _SettingScrollPosition = true;
            int diff = y - m_ScrollPosition.Y;
            int start = m_ScrollPosition.Y;

            m_ScrollPosition.Y = y;
            OffsetSubItems(new Point(0, diff));

            if (!m_PopupGalleryStyle || !m_PopupUsesStandardScrollbars) EnableScrollButtons(this.DisplayRectangle);
            if (IsAnimationEnabled)
                StartAnimation(start, y);
            else
                this.Refresh();
            if (m_ScrollBar != null && m_ScrollBar.Value != -y)
                m_ScrollBar.Value = -y;
            _SettingScrollPosition = false;
        }

        public override void Paint(ItemPaintArgs p)
        {
            m_ThreadUI.StartReadOperation();
            try
            {
                if (m_StateBitmap != null)
                {
                    base.PaintBackground(p);
                    Graphics g = p.Graphics;
                    Rectangle clip = GetClipRectangle();
                    Region oldClip = g.Clip;
                    g.SetClip(clip, CombineMode.Intersect);
                    
                    p.Graphics.DrawImage(m_StateBitmap, clip.X, clip.Y + m_AnimationCurrentPos);

                    if (oldClip != null)
                        g.Clip = oldClip;
                    else
                        g.ResetClip();
                    
                }
                else
                    base.Paint(p);
            }
            finally
            {
                m_ThreadUI.EndReadOperation();
            }

            //int cornerSize = 0;
            //Office2007ButtonItemPainter painter = PainterFactory.CreateButtonPainter(m_ScrollUp) as Office2007ButtonItemPainter;
            //if (painter != null)
            //{
            //    cornerSize = painter.CornerSize;
            //    painter.CornerSize = 2;
            //}

            if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars)
            {
                if (m_ScrollBar.ParentControl == null) m_ScrollBar.ParentControl = p.ContainerControl;
                m_ScrollBar.Paint(p);
            }
            else
            {
                if(m_ScrollUp.Visible)
                    m_ScrollUp.Paint(p);
                if (m_ScrollDown.Visible)
                    m_ScrollDown.Paint(p);
                if (m_EnableGalleryPopup)
                    m_PopupGallery.Paint(p);
            }

            //if (painter != null)
            //    painter.CornerSize = cornerSize;
        }

        private Point GetScrollPosition()
        {
            return m_ScrollPosition;
        }

        private Size GetLayoutSize()
        {
            if (!m_RecommendedSize.IsEmpty && m_RecommendedSize.Width >= this.MinimumSize.Width && m_RecommendedSize.Height >= this.MinimumSize.Height)
                return m_RecommendedSize;
            Size s = m_DefaultSize;
            if (this.PopupGalleryStyle && !this.SystemGallery)
            {
                if (this.MinimumSize.Width > s.Width)
                    s.Width = this.MinimumSize.Width;
                if (this.MinimumSize.Height > s.Height)
                    s.Height = this.MinimumSize.Height;
            }
            return s;
        }

        protected override System.Drawing.Rectangle GetLayoutRectangle(System.Drawing.Rectangle bounds)
        {
            Rectangle r = new Rectangle(bounds.Location, GetLayoutSize());

            if (m_PopupUsesStandardScrollbars || !m_PopupGalleryStyle)
            {
                r.Width -= (m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing);
                if (this.IsRightToLeft)
                    r.X += (m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing);
            }
            bool disposeStyle = false;
            ElementStyle style = ElementStyleDisplay.GetElementStyle(BackgroundStyle, out disposeStyle);
            r.X += ElementStyleLayout.LeftWhiteSpace(style);
            r.Y += ElementStyleLayout.TopWhiteSpace(style);
            r.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(style);
            r.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(style);

            r.Offset(GetScrollPosition());

            if(disposeStyle) style.Dispose();

            return r;
        }

        protected override Rectangle LayoutItems(Rectangle bounds)
        {
            m_ViewRectangle = base.LayoutItems(bounds);
            if (m_ViewRectangle.Height < Math.Abs(m_ScrollPosition.Y)) SetScrollPosition(0);
            Size size = GetLayoutSize();
            if (this.SystemGallery && !this.EnableGalleryPopup || this.IsOnMenu)
            {
                if(size.Height > m_ViewRectangle.Height)
                    size.Height = m_ViewRectangle.Height;
            }
            if (m_IncrementalSizing && !PopupGalleryStyle)
                size.Width = m_ViewRectangle.Width + (m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing);
            Rectangle r = new Rectangle(bounds.Location, size);

            int minHeight = m_ScrollButtonSize.Height * 2 - 1;
            if (m_EnableGalleryPopup) minHeight += m_ScrollButtonSize.Height - 1;
            r.Height = Math.Max(minHeight, r.Height);
            RepositionButtons(r);

            if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars)
            {
                if (m_ViewRectangle.Bottom - r.Bottom > 0)
                {
                    m_ScrollBar.Enabled = true;
                    m_ScrollBar.Maximum = m_ViewRectangle.Bottom - r.Bottom;
                    m_ScrollBar.Minimum = 0;
                    m_ScrollBar.SmallChange = GetScrollLineHeight();
                    m_ScrollBar.LargeChange = (int)(m_ScrollBar.Maximum * ((float)r.Height / (float)m_ViewRectangle.Height));
                }
                else
                    m_ScrollBar.Enabled = false;
            }
            else
            {
                EnableScrollButtons(r);
            }
            return r;
        }

        protected override Size GetEmptyDesignTimeSize()
        {
            if (m_PopupGalleryStyle && !this.SystemGallery)
            {
                Size s = this.MinimumSize;
                if(!s.IsEmpty)
                    return s;
            }

            return base.GetEmptyDesignTimeSize();
        }

        internal bool IsAtMinimumSize
        {
            get
            {
                if (m_IncrementalSizing && this.SubItems.Count>0)
                {
                    return this.WidthInternal - this.SubItems[0].WidthInternal <= this.MinimumSize.Width;
                }
                else
                {
                    return this.WidthInternal <= this.MinimumSize.Width;
                }
            }
        }

        private void EnableScrollButtons(Rectangle displayRect)
        {
            if (GetScrollPosition().Y == 0)
            {
                m_ScrollUp.Enabled = false;
            }
            else
                m_ScrollUp.Enabled = true;

            if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
            {
                m_ScrollUp.Visible = m_ScrollUp.Enabled;
                m_ScrollUp.Displayed = m_ScrollUp.Enabled;
            }

            if (m_ViewRectangle.Bottom + GetScrollPosition().Y > displayRect.Bottom)
                m_ScrollDown.Enabled = true;
            else
                m_ScrollDown.Enabled = false;

            if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
            {
                m_ScrollDown.Visible = m_ScrollDown.Enabled;
                m_ScrollDown.Displayed = m_ScrollDown.Enabled;
            }
        }

        protected override void OnExternalSizeChange()
        {
            if (this.PopupGalleryStyle)
                RepositionButtons(this.DisplayRectangle);
            base.OnExternalSizeChange();
        }

        private void RepositionButtons(Rectangle r)
        {
            int x = r.Right - (m_ScrollButtonSize.Width);
            int y = r.Y;
            if (this.IsRightToLeft)
                x = r.X + m_ScrollButtonSize.Width;
            if (!m_ViewRectangle.IsEmpty)
            {
                if (m_ViewRectangle.Y != r.Y)
                    m_ViewRectangle.Y = r.Y;
                if (m_ViewRectangle.X != r.X)
                    m_ViewRectangle.X = r.X;
            }

            if (m_PopupGalleryStyle && m_ScrollBar != null)
            {
                bool disposeStyle = false;
                ElementStyle style = ElementStyleDisplay.GetElementStyle(BackgroundStyle, out disposeStyle);
                x -= ElementStyleLayout.RightWhiteSpace(style);
                y += ElementStyleLayout.TopWhiteSpace(style);
                r.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(style);
                m_ScrollBar.DisplayRectangle = new Rectangle(x, y, m_ScrollButtonSize.Width, r.Height);

                if(disposeStyle) style.Dispose();

                return;
            }
            else if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
            {
                Size buttonSize = new Size(r.Width, 12);
                //m_ScrollUp.Displayed = false;
                //m_ScrollUp.Visible = false;
                m_ScrollUp.RecalcSize();
                m_ScrollUp.Size = buttonSize;
                m_ScrollUp.SetDisplayRectangle(new Rectangle(r.X, r.Y,
                    buttonSize.Width, buttonSize.Height));

                //m_ScrollDown.Displayed = false;
                //m_ScrollDown.Visible = false;
                m_ScrollDown.RecalcSize();
                m_ScrollDown.Size = buttonSize;
                m_ScrollDown.SetDisplayRectangle(new Rectangle(r.X, r.Bottom - buttonSize.Height,
                    buttonSize.Width, buttonSize.Height));
                return;
            }

            Rectangle buttonRect = new Rectangle(x, y, m_ScrollButtonSize.Width, m_ScrollButtonSize.Height);
            m_ScrollUp.Displayed = true;
            m_ScrollUp.RecalcSize();
            m_ScrollUp.Size = buttonRect.Size;
            m_ScrollUp.SetDisplayRectangle(buttonRect);
            if (m_EnableGalleryPopup)
            {
                y = r.Bottom - (m_ScrollButtonSize.Height);
                buttonRect = new Rectangle(x, y, m_ScrollButtonSize.Width, m_ScrollButtonSize.Height);
                m_PopupGallery.Displayed = true;
                m_PopupGallery.RecalcSize();
                m_PopupGallery.Size = buttonRect.Size;
                m_PopupGallery.SetDisplayRectangle(buttonRect);
                buttonRect.Offset(0, -(m_ScrollButtonSize.Height - 1));
            }
            else
            {
                m_PopupGallery.Displayed = false;
                y = r.Bottom - (m_ScrollButtonSize.Height);
                buttonRect = new Rectangle(x, y, m_ScrollButtonSize.Width, m_ScrollButtonSize.Height);
            }

            m_ScrollDown.Displayed = true;
            m_ScrollDown.RecalcSize();
            m_ScrollDown.Size = buttonRect.Size;
            m_ScrollDown.SetDisplayRectangle(buttonRect);
        }

        // <summary>
        /// Called after LeftInternal property has changed
        /// </summary>
        protected override void OnLeftLocationChanged(int oldValue)
        {
            base.OnLeftLocationChanged(oldValue);
            RepositionButtons(this.DisplayRectangle);
        }

        public override Rectangle Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
                RepositionButtons(this.DisplayRectangle);
            }
        }

        protected override void OnTopLocationChanged(int oldValue)
        {
            base.OnTopLocationChanged(oldValue);
            RepositionButtons(this.DisplayRectangle);
        }

        protected override void OnStyleChanged()
        {
            m_ScrollUp.Style = this.Style;
            m_ScrollDown.Style = this.Style;
            m_PopupGallery.Style = this.Style;
            if (BarFunctions.IsOffice2010Style(this.Style))
            {
                m_ScrollUp.Shape = new RoundRectangleShapeDescriptor();
                m_ScrollDown.Shape = new RoundRectangleShapeDescriptor();
                m_PopupGallery.Shape = new RoundRectangleShapeDescriptor();
            }
            else
            {
                m_ScrollUp.Shape = null;
                m_ScrollDown.Shape = null;
                m_PopupGallery.Shape = null;
            }
            base.OnStyleChanged();
        }

        private int GetScrollLineHeight()
        {
            int itemY = this.DisplayRectangle.Bottom + GetScrollPosition().Y;
            foreach (BaseItem item in this.SubItems)
            {
                if (!item.Visible) continue;

                if (item.TopInternal > itemY)
                    return item.HeightInternal;
            }
            return 24;
        }

        private int GetNextScrollLine()
        {
            bool firstVisibleFound = false;
            int itemY = 0;
            foreach (BaseItem item in this.SubItems)
            {
                if (!item.Visible) continue;

                if (firstVisibleFound && item.TopInternal > itemY)
                    return GetScrollForItem(item);
                if (!firstVisibleFound && item.TopInternal >= this.TopInternal)
                {
                    firstVisibleFound = true;
                    itemY = item.TopInternal;
                }
            }

            return GetScrollPosition().Y;
        }

        private int GetPreviousScrollLine()
        {
            if (GetScrollPosition().Y == 0)
                return 0;

            int c = this.SubItems.Count -1;
            for (int i = c; i >= 0; i--)
            {
                BaseItem item = this.SubItems[i];
                if (!item.Visible) continue;
                if (item.TopInternal < this.TopInternal)
                    return GetScrollForItem(item);
            }

            return 0;
        }

        private int GetScrollForItem(BaseItem item)
        {
            int i = this.DisplayRectangle.Top - item.TopInternal + GetScrollPosition().Y;
            if (Math.Abs(i) <= 4) i = 0;
            return i;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public override bool Expanded
        {
            get
            {
                return m_Expanded;
            }
            set
            {
                base.Expanded = value;
                if (!value && m_PopupGallery.Expanded)
                    m_PopupGallery.Expanded = false;
            }
        }

        /// <summary>
        /// Gets whether the Gallery popup is open.
        /// </summary>
        [Browsable(false)]
        public bool IsGalleryPopupOpen
        {
            get
            {
                return m_PopupGallery.Expanded;
            }
        }

        /// <summary>
        /// Opens the Gallery popup menu.
        /// </summary>
        public void PopupGallery()
        {
            if (m_PopupGallery.Expanded)
            {
                m_PopupGallery.Expanded = false;
            }
            else
            {
                if (!this.DesignMode)
                {
                    System.Windows.Forms.Control ctrl = this.ContainerControl as System.Windows.Forms.Control;
                    Point p = ctrl.PointToScreen(this.DisplayRectangle.Location);
                    GalleryContainer c = null;
                    if (m_Groups.Count > 0)
                    {
                        c = new GalleryContainer();
                        c.DefaultSize = this.DefaultSize;
                        c.StretchGallery = this.StretchGallery;
                        c.MultiLine = false;
                        c.LayoutOrientation = eOrientation.Vertical;

                        foreach (BaseItem item in this.PopupGalleryItems)
                            c.PopupGalleryItems.Add(item.Copy());

                        ArrangePopupGroups(c);
                    }
                    else
                        c = this.Copy() as GalleryContainer;
                    c.EnableGalleryPopup = false;
                    c.SystemGallery = true;
                    c.IncrementalSizing = false;
                    c.PopupUsesStandardScrollbars = m_PopupUsesStandardScrollbars;
                    if (this.PopupGallerySize.IsEmpty)
                        c.DefaultSize = GetPopupGallerySize(p);
                    else
                        c.DefaultSize = this.PopupGallerySize;
                    c.BackgroundStyle.Reset();
                    c.PopupGalleryStyle = true;
                    m_PopupGallery.SubItems.Insert(0, c);
                    m_PopupGallery.PopupMenu(p);
                }
                else
                    m_PopupGallery.Expanded = true;
            }
        }

        protected internal override void OnContainerChanged(object objOldContainer)
        {
            if (!this.SystemGallery)
            {
                if (BaseItem.IsOnPopup(this))
                {
                    SetScrollPosition(0);
                    this.PopupGalleryStyle = true;
                }
            }
            base.OnContainerChanged(objOldContainer);
        }

        private void ArrangePopupGroups(GalleryContainer gc)
        {
            ArrayList groupList = GetSortedGroupList();
            foreach(GalleryGroup g in groupList)
            {
                if (g.Items.Count == 0)
                    continue;

                LabelItem label = CreateLabelForGroup(g);
                gc.SubItems.Add(label);
                ItemContainer cont = CreateContainerFormGroup(g);
                gc.SubItems.Add(cont);
            }
        }

        private ItemContainer CreateContainerFormGroup(GalleryGroup g)
        {
            ItemContainer c = new ItemContainer();
            c.LayoutOrientation = eOrientation.Horizontal;
            c.MultiLine = true;
            c.Name = "container_" + g.Name;

            foreach (BaseItem item in g.Items)
            {
                c.SubItems.Add(item.Copy());
            }

            return c;
        }

        private LabelItem CreateLabelForGroup(GalleryGroup g)
        {
            LabelItem l = new LabelItem("label_" + g.Name, g.Text);
            if (Rendering.GlobalManager.Renderer is Rendering.Office2007Renderer)
            {
                Rendering.Office2007ColorTable ct = ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable;
                l.BackColor = ct.Gallery.GroupLabelBackground;
                l.ForeColor = ct.Gallery.GroupLabelText;
                l.SingleLineColor= ct.Gallery.GroupLabelBorder;
                l.BorderType = eBorderType.SingleLine;
                l.BorderSide = eBorderSide.Bottom;
                l.PaddingTop = 1;
                l.PaddingLeft = 10;
                l.PaddingBottom = 1;
            }
            return l;
        }

        private ArrayList GetSortedGroupList()
        {
            ArrayList groups = new ArrayList(m_Groups.Count);
            foreach(GalleryGroup g in m_Groups)
                groups.Add(g);
            groups.Sort(new GroupDisplayOrderComparer());

            return groups;
        }

        private class GroupDisplayOrderComparer : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(object x, object y)
            {
                if (x is GalleryGroup && y is GalleryGroup)
                {
                    return ((GalleryGroup)x).DisplayOrder - ((GalleryGroup)y).DisplayOrder;
                }
                else
                    return ((new CaseInsensitiveComparer()).Compare(x, y));
            }
        }

        protected override InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
        {
            InsertPosition pos = DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
            if (pos == null && m_PopupGallery.Expanded)
                pos = m_PopupGallery.GetInsertPosition(pScreen, dragItem);
            return pos;
        }

        protected override IContentLayout GetContentLayout()
        {
            SerialContentLayoutManager lm = base.GetContentLayout() as SerialContentLayoutManager;
            if (lm != null)
                lm.HorizontalFitContainerHeight = false;

            return lm;
        }

        /// <summary>
        /// Gets or sets the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar. Use KeyTips property
        /// when you want to assign the one or more letters to be used to access an item. For example assigning the FN to KeyTips property
        /// will require the user to press F then N keys to select an item. Pressing the F letter will show only keytips for the items that start with letter F.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(""), Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
        public override string KeyTips
        {
            get { return base.KeyTips; }
            set { base.KeyTips = value; }
        }
        #endregion

        #region Scroll Animation
        private Bitmap m_StateBitmap = null;
        private int m_AnimationEndPos = 0;
        private int m_AnimationCurrentPos = 0;
        private int m_AnimationStep = 0;
        private void StartAnimation(int start, int end)
        {
            m_AnimationEndPos = end;
            m_AnimationCurrentPos = start;
            m_AnimationStep = (int)Math.Max(8, Math.Abs(end - start) / 3);
            //Console.WriteLine("Start ={0}  End = {1}   Step = {2}", start, end, m_AnimationStep);
            if (end < start) m_AnimationStep *= -1;

            m_ThreadUI.Start();

        }

        private void RecordStartState(object sender, EventArgs e)
        {
            m_StateBitmap = new Bitmap(m_ViewRectangle.Width, m_ViewRectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics g1 = Graphics.FromImage(m_StateBitmap);
            BufferedBitmap bb = new BufferedBitmap(g1, new Rectangle(0, 0, m_ViewRectangle.Width, m_ViewRectangle.Height));
            Graphics g = bb.Graphics;
            Color transparentColor = Color.Empty;
            try
            {
                bool disposeStyle = false;
                ElementStyle style = ElementStyleDisplay.GetElementStyle(this.BackgroundStyle, out disposeStyle);
                if (!style.BackColor.IsEmpty)
                    transparentColor = style.BackColor;
                else if (style.BackColorBlend.Count > 0 && !style.BackColorBlend[0].Color.IsEmpty)
                    transparentColor = style.BackColorBlend[0].Color;
                else
                    transparentColor = Color.WhiteSmoke;

                if (!transparentColor.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(transparentColor))
                        g.FillRectangle(brush, 0, 0, m_StateBitmap.Width, m_StateBitmap.Height);
                }
                System.Windows.Forms.Control cc = this.ContainerControl as System.Windows.Forms.Control;
                bool antiAlias = false;
                ItemPaintArgs pa = null;
                if (cc is ItemControl)
                {
                    antiAlias = ((ItemControl)cc).AntiAlias;
                    pa = ((ItemControl)cc).GetItemPaintArgs(g);
                }
                else if (cc is Bar)
                {
                    antiAlias = ((Bar)cc).AntiAlias;
                    pa = ((Bar)cc).GetItemPaintArgs(g);
                }
                else if (cc is ButtonX)
                {
                    antiAlias = ((ButtonX)cc).AntiAlias;
                    pa = ((ButtonX)cc).GetItemPaintArgs(g);
                }
                else if (cc is MenuPanel)
                {
                    antiAlias = ((MenuPanel)cc).AntiAlias;
                    pa = ((MenuPanel)cc).GetItemPaintArgs(g);
                }

                Rectangle clip = GetClipRectangle();
                System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
                myMatrix.Translate( -(clip.X + +m_ScrollPosition.X),
                                    -(clip.Y + m_ScrollPosition.Y), 
                                    System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;

                if (antiAlias)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                }

                if (pa == null)
                {
                    m_StateBitmap.Dispose();
                    m_StateBitmap = null;
                }

                // Do not paint scrollbars just content
                ItemDisplay display = GetItemDisplay();
                display.Paint(this, pa);
                bb.Render(g1);

                if(disposeStyle) style.Dispose();
            }
            finally
            {
                g = null;
                bb.Dispose();
                g1.Dispose();
            }
            if (!transparentColor.IsEmpty)
                m_StateBitmap.MakeTransparent(transparentColor);
        }

        private void CleanupState(object sender, EventArgs e)
        {
            if (m_StateBitmap != null)
            {
                m_StateBitmap.Dispose();
                m_StateBitmap = null;
            }
        }

        private void UpdateThreadedUI(object sender, EventArgs e)
        {
            m_AnimationCurrentPos += m_AnimationStep;

            if (m_AnimationCurrentPos <= m_AnimationEndPos && m_AnimationEndPos < 0 && m_AnimationStep < 0 ||
                m_AnimationCurrentPos >= m_AnimationEndPos && m_AnimationEndPos >= 0 && m_AnimationStep > 0 || 
                m_AnimationCurrentPos >= m_AnimationEndPos && m_AnimationEndPos < 0 && m_AnimationStep > 0 ||
                m_AnimationCurrentPos <= m_AnimationEndPos && m_AnimationEndPos >= 0 && m_AnimationStep < 0)
                m_ThreadUI.Stop();

            System.Windows.Forms.Control cc = this.ContainerControl as System.Windows.Forms.Control;
            if (cc != null)
                cc.Invalidate(this.DisplayRectangle);
        }

        private bool IsAnimationEnabled
        {
            get
            {
                if (this.DesignMode || !m_ScrollAnimation || !this.Displayed || TextDrawing.UseTextRenderer)
                    return false;
                System.Windows.Forms.Control cc = this.ContainerControl as System.Windows.Forms.Control;
                if (cc != null)
                {
                    if (cc is ItemControl)
                        return ((ItemControl)cc).IsFadeEnabled;
                    else if (cc is Bar)
                        return ((Bar)cc).IsFadeEnabled;
                    else if (cc is ButtonX)
                        return ((ButtonX)cc).IsFadeEnabled;
                }
                return m_ScrollAnimation;
            }
        }

        protected override void OnDisplayedChanged()
        {
            if(!this.Displayed && !this.SystemGallery && this.PopupGalleryStyle)
                SetScrollPosition(0);
            base.OnDisplayedChanged();
        }

        protected internal override void OnVisibleChanged(bool newValue)
        {
            base.OnVisibleChanged(newValue);
            if (!newValue)
            {
                m_ThreadUI.Stop();
            }
        }

        /// <summary>
        /// Gets or sets whether scroll animation is enabled. Default value is true.
        /// Scroll animation will be disabled if gallery is running under Remote Windows Terminal session or fade animation effect is disabled on the
        /// container control.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Animation"), Description("Indicates whether scroll animation is enabled.")]
        public bool ScrollAnimation
        {
            get { return m_ScrollAnimation; }
            set { m_ScrollAnimation = value; }
        }
        #endregion

        #region IExtenderProvider Implementation, GalleryGroup property extender
        /// <summary>
        /// Returns whether Gallery can extend the object.
        /// </summary>
        /// <param name="extendee">Object to test extensibility for.</param>
        /// <returns>Returns true if object can be extended otherwise false.</returns>
        public bool CanExtend(object extendee)
        {
            BaseItem item = extendee as BaseItem;
            if (item == null) return false;
            return this.SubItems.Contains(item);
        }

        /// <summary>
        /// Gets the GalleryGroup item is assigned to.
        /// </summary>
        /// <param name="item">Reference to item.</param>
        /// <returns>An instance of GalleryGroup object or null if item is not assigned to the group</returns>
        [Editor("DevComponents.DotNetBar.Design.GalleryGroupTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DefaultValue(null)]
        public GalleryGroup GetGalleryGroup(BaseItem item)
        {
            if (item == null) return null;
            return GetItemGalleryGroup(item);
        }
        /// <summary>
        /// Assigns the item to the gallery group.
        /// </summary>
        /// <param name="item">Item to assign.</param>
        /// <param name="group">Group to assign item to. Can be null to remove item assignment.</param>
        public void SetGalleryGroup(BaseItem item, GalleryGroup group)
        {
            if (item == null)
                throw new ArgumentNullException("item argument cannot be null.");

            if (group == null)
            {
                GalleryGroup g = GetItemGalleryGroup(item);
                if (g != null)
                    g.Items.Remove(item);
            }
            else
            {
                GalleryGroup g = GetItemGalleryGroup(item);
                if (g != null)
                    g.Items.Remove(item);
                group.Items.Add(item);
            }
        }

        private GalleryGroup GetItemGalleryGroup(BaseItem item)
        {
            foreach (GalleryGroup g in m_Groups)
            {
                if (g.Items.Contains(item))
                    return g;
            }
            return null;
        }

        /// <summary>
        /// Gets the collection of GalleryGroup objects associated with this gallery. Groups are assigned optionally to one or more items
        /// that are part of the GalleryContainer. Groups are used to visually group the items when gallery is displayed on the popup.
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.GalleryGroupCollectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Groups"), Browsable(false), Description("Gets the collection of GalleryGroup objects associated with this gallery.")]
        public GalleryGroupCollection GalleryGroups
        {
            get { return m_Groups; }
        }
        #endregion
    }
}
