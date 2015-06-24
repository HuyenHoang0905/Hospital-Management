using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents item container that arranges items horizontally or vertically.
	/// </summary>
	[ToolboxItem(false),DesignTimeVisible(false),Designer("DevComponents.DotNetBar.Design.ItemContainerDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class ItemContainer:ImageItem , IDesignTimeProvider
	{
		#region Private Variables & Constructor
		private eOrientation m_LayoutOrientation=eOrientation.Horizontal;
		private Size m_EmptyDesignTimeSize=new Size(24,24);
		private bool m_SystemContainer=false;
        private int m_ItemSpacing = 1;
        private Rectangle m_DesignerSelectionRectangle = Rectangle.Empty;
        private bool m_ResizeItemsToFit = true;
        private eHorizontalItemsAlignment m_HorizontalItemAlignment = eHorizontalItemsAlignment.Left;
        private eVerticalItemsAlignment m_VerticalItemAlignment = eVerticalItemsAlignment.Top;
        private Size m_MinimumSize = Size.Empty;
        private ElementStyle m_BackgroundStyle = new ElementStyle();
        private int m_BeginGroupSpacing = 0;
        private bool m_MultiLine = false;
		
		/// <summary>
		/// Creates new instance of the ItemContainer object.
		/// </summary>
		public ItemContainer()
		{
			m_IsContainer=true;
			this.AutoCollapseOnClick=true;
			this.AccessibleRole=System.Windows.Forms.AccessibleRole.Grouping;

            //m_Padding.SpacingChanged += new EventHandler(PaddingChanged);
            //m_Margin.SpacingChanged += new EventHandler(MarginChanged);

            m_BackgroundStyle.StyleChanged += BackgroundStyleChanged;
		}
        protected override void Dispose(bool disposing)
        {
            m_BackgroundStyle.StyleChanged -= BackgroundStyleChanged;
            m_BackgroundStyle.Dispose();
            base.Dispose(disposing);
        }
		#endregion

		#region Internal Implementation
        private void MarginChanged(object sender, EventArgs e)
        {
            NeedRecalcSize = true;
            OnAppearanceChanged();
        }

        private void PaddingChanged(object sender, EventArgs e)
        {
            NeedRecalcSize = true;
            OnAppearanceChanged();
        }

        private void BackgroundStyleChanged(object sender, EventArgs e)
        {
            this.OnAppearanceChanged();
        }

        /// <summary>
        /// Gets or sets the minimum size of the container. Either Width or Height can be set or both. Default value is 0,0 which means
        /// that size is automatically calculated.
        /// </summary>
        public Size MinimumSize
        {
            get { return m_MinimumSize; }
            set
            {
                m_MinimumSize = value;
                this.NeedRecalcSize = true;
                this.OnAppearanceChanged();
            }
        }

        private bool ShouldSerializeMinimumSize()
        {
            return !m_MinimumSize.IsEmpty;
        }

        /// <summary>
        /// Specifies the container background style. Default value is an empty style which means that container does not display any background.
        /// BeginGroup property set to true will override this style on some styles.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets container background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return m_BackgroundStyle; }
        }

        /// <summary>
        /// Gets or sets the item alignment when container is in horizontal layout. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eHorizontalItemsAlignment.Left), Category("Layout"), Description("Indicates item alignment when container is in horizontal layout."), DevCoBrowsable(true)]
        public virtual eHorizontalItemsAlignment HorizontalItemAlignment
        {
            get { return m_HorizontalItemAlignment; }
            set
            {
                m_HorizontalItemAlignment = value;
                this.NeedRecalcSize = true;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the item vertical alignment. Default value is Top.
        /// </summary>
        [Browsable(true), DefaultValue(eVerticalItemsAlignment.Top), Category("Layout"), Description("Indicates item item vertical alignment."), DevCoBrowsable(true)]
        public virtual eVerticalItemsAlignment VerticalItemAlignment
        {
            get { return m_VerticalItemAlignment; }
            set
            {
                m_VerticalItemAlignment = value;
                this.NeedRecalcSize = true;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Layout"), Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
        public virtual bool MultiLine
        {
            get { return m_MultiLine; }
            set
            {
                m_MultiLine = value;
                this.NeedRecalcSize = true;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets spacing in pixels between items. Default value is 0.
        /// </summary>
        [Browsable(true), DefaultValue(1), Category("Layout"), Description("Indicates spacing in pixels between items.")]
        public int ItemSpacing
        {
            get { return m_ItemSpacing; }
            set
            {
                if (m_ItemSpacing != value)
                {
                    m_ItemSpacing = value;
                    this.NeedRecalcSize = true;
                    this.OnAppearanceChanged();
                }
            }
        }

        protected virtual Rectangle GetClipRectangle()
        {
            Rectangle clip = this.DisplayRectangle;
            bool disposeStyle = false;
            ElementStyle style = ElementStyleDisplay.GetElementStyle(m_BackgroundStyle, out disposeStyle);
            clip.X += ElementStyleLayout.LeftWhiteSpace(style);
            clip.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(style);
            clip.Y += ElementStyleLayout.TopWhiteSpace(style);
            clip.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(style);
            if(disposeStyle) style.Dispose();
            return clip;
        }

        internal void InternalPaintBackground(ItemPaintArgs p)
        {
            PaintBackground(p);
        }

        protected virtual void PaintBackground(ItemPaintArgs p)
        {
            m_BackgroundStyle.SetColorScheme(p.Colors);
            Graphics g = p.Graphics;
            Rendering.BaseRenderer renderer = p.Renderer;
            if (renderer != null)
            {
                ItemContainerRendererEventArgs ie = new ItemContainerRendererEventArgs(p.Graphics, this);
                ie.ItemPaintArgs = p;
                renderer.DrawItemContainer(ie);
            }
            else
                ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(m_BackgroundStyle, g, this.DisplayRectangle));
        }

        private bool IsGroupingContainer
        {
            get
            {
                return BarFunctions.IsOffice2007StyleOnly(this.EffectiveStyle) && this.BeginGroup;
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

            if (IsGroupingContainer)
            {
                Rectangle clip = this.DisplayRectangle;
                clip.Inflate(-1, -1);
                oldClip = g.Clip;
                g.SetClip(clip, CombineMode.Replace);
                clipSet = true;
            }
            else
            {
                Rectangle clip = GetClipRectangle();
                oldClip = g.Clip;
                g.SetClip(clip, CombineMode.Intersect);
                clipSet = true;
            }
            
			ItemDisplay display=GetItemDisplay();
			display.Paint(this,p);

            if (clipSet)
            {
                if (oldClip != null)
                    g.Clip = oldClip;
                else
                    g.ResetClip();
            }

            if (oldClip != null)
                oldClip.Dispose();

			if(this.DesignMode && !this.SystemContainer && p.DesignerSelection)
			{
				Rectangle r=this.DisplayRectangle;

                Pen pen = null;
                if (this.Focused)
                    pen = new Pen(Color.FromArgb(190, Color.Navy), 1);
                else
                    pen = new Pen(Color.FromArgb(80, Color.Black), 1);

                pen.DashStyle = DashStyle.Dot;
                DisplayHelp.DrawRoundedRectangle(g, pen, r, 3);
                pen.Dispose();
                pen = null;

                Image image=BarFunctions.LoadBitmap("SystemImages.AddMoreItemsContainer.png");

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

                m_DesignerSelectionRectangle = new Rectangle(r.X + 1, r.Y + 1, image.Width, image.Height);
                g.DrawImageUnscaled(image, m_DesignerSelectionRectangle.Location);
			}

            this.DrawInsertMarker(p.Graphics);
		}

        /// <summary>
        /// Returns empty container default design-time size.
        /// </summary>
        /// <returns>Size of an empty container.</returns>
        protected virtual Size GetEmptyDesignTimeSize()
        {
            Size s = m_EmptyDesignTimeSize;
            if (m_LayoutOrientation == eOrientation.Horizontal)
                s.Width += 12;
            else
                s.Height += 12;

            return s;
        }

        /// <summary>
        /// Recalculates the size of the container. Assumes that DisplayRectangle.Location is set to the upper left location of this container.
        /// </summary>
        public override void RecalcSize()
		{
			if(this.SuspendLayout)
				return;

			if(this.SubItems.Count==0)
			{
				if(this.DesignMode && !this.SystemContainer)
				{
                    m_Rect.Size = GetEmptyDesignTimeSize();
				}
				else
				{
					m_Rect=Rectangle.Empty;
				}
				base.RecalcSize();
				return;
			}

            m_Rect = LayoutItems(m_Rect);

			base.RecalcSize();
		}

        protected virtual Rectangle LayoutItems(Rectangle bounds)
        {
            IContentLayout layout = this.GetContentLayout();
            BlockLayoutManager blockLayout = this.GetBlockLayoutManager();
            Rectangle r = GetLayoutRectangle(bounds);

            BaseItem[] elements = new BaseItem[this.SubItems.Count];
            if (_FitOversizeItemIntoAvailableWidth)
            {
                for (int i = 0; i < this.SubItems.Count; i++)
                {
                    ButtonItem button = this.SubItems[i] as ButtonItem;
                    if (button != null)
                    {
                        button._FitContainer = true;
                        button.WidthInternal = r.Width;
                    }
                    elements[i] = this.SubItems[i];
                }
            }
            else
                this.SubItems.CopyTo(elements, 0);

            bool disposeStyle = false;
            ElementStyle style = ElementStyleDisplay.GetElementStyle(m_BackgroundStyle, out disposeStyle);
            r = layout.Layout(r, elements, blockLayout);
            
            if (!this.SystemContainer)
                r = new Rectangle(bounds.Location, r.Size);

            if (m_MinimumSize.Width > 0 && m_MinimumSize.Width > r.Width)
                r.Width = m_MinimumSize.Width;
            else
                r.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(style);

            if (m_MinimumSize.Height > 0 && m_MinimumSize.Height > r.Height)
                r.Height = m_MinimumSize.Height;
            else
                r.Height += ElementStyleLayout.VerticalStyleWhiteSpace(style);
            if(disposeStyle) style.Dispose();
            return r;
        }

        protected virtual Rectangle GetLayoutRectangle(Rectangle bounds)
        {
            if (bounds.Width == 0)
                bounds.Width = 16;
            if (bounds.Height == 0)
                bounds.Height = 16;

            if (m_MinimumSize.Width > 0)
                bounds.Width = m_MinimumSize.Width;
            if (m_MinimumSize.Height > 0)
                bounds.Height = m_MinimumSize.Height;

            bool disposeStyle = false;
            ElementStyle style = ElementStyleDisplay.GetElementStyle(m_BackgroundStyle, out disposeStyle);
            bounds.X += ElementStyleLayout.LeftWhiteSpace(style); // m_BackgroundStyle.PaddingLeft + m_BackgroundStyle.MarginLeft;
            bounds.Y += ElementStyleLayout.TopWhiteSpace(style); // m_BackgroundStyle.PaddingTop + m_BackgroundStyle.MarginTop;
            bounds.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(style); //m_BackgroundStyle.PaddingHorizontal + m_BackgroundStyle.MarginHorizontal;
            bounds.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(style); //m_BackgroundStyle.PaddingVertical + m_BackgroundStyle.MarginVertical;
            if(disposeStyle) style.Dispose();

            return bounds;
        }

        protected override void OnExternalSizeChange()
        {
            base.OnExternalSizeChange();
            if ((m_VerticalItemAlignment != eVerticalItemsAlignment.Top || IsRightToLeft)&& this.SubItems.Count>0 && !this.SuspendLayout)
            {
                LayoutItems(m_Rect);
            }
        }

        private bool _FitOversizeItemIntoAvailableWidth = false;
        internal bool FitOversizeItemIntoAvailableWidth
        {
            get { return _FitOversizeItemIntoAvailableWidth; }
            set
            {
                _FitOversizeItemIntoAvailableWidth = value;
            }
        }

        protected internal override void OnAfterItemRemoved(BaseItem item)
        {
            if (item is ButtonItem && _FitOversizeItemIntoAvailableWidth)
                ((ButtonItem)item)._FitContainer = false;
            base.OnAfterItemRemoved(item);
        }

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is pressed. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
            if (this.DesignMode && !this.SystemContainer)
            {
                BaseItem item = ItemAtLocation(objArg.X, objArg.Y);
                if (item == this)
                {
                    IOwner owner = this.GetOwner() as IOwner;
                    if (owner != null)
                    {
                        owner.SetFocusItem(this);
                        return;
                    }
                }
            }
            else if (!this.DisplayRectangle.Contains(objArg.X, objArg.Y))
            {
                if (this.DesignMode)
                {
                    IOwner owner = this.GetOwner() as IOwner;
                    if (owner != null)
                        owner.SetFocusItem(null);
                    LeaveHotSubItem(null);
                }
                return;
            }
			base.InternalMouseDown(objArg);
		}

        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            if (!this.DisplayRectangle.Contains(objArg.X, objArg.Y))
            {
                LeaveHotSubItem(null);
                if(!this.SystemContainer)
                    return;
            }
            base.InternalMouseMove(objArg);
        }

        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            if (!this.DisplayRectangle.Contains(objArg.X, objArg.Y))
                return;
            base.InternalMouseUp(objArg);
        }

        /// <summary>
        /// Return Sub Item at specified location
        /// </summary>
        public override BaseItem ItemAtLocation(int x, int y)
        {
            if (this.DesignMode && !this.SystemContainer && !m_DesignerSelectionRectangle.IsEmpty)
            {
                if (m_DesignerSelectionRectangle.Contains(x, y))
                    return this;
            }

            if (!this.Visible || !this.DisplayRectangle.Contains(x, y))
                return null;

            if (m_SubItems != null)
            {
                foreach (BaseItem objSub in m_SubItems)
                {
                    if (objSub.IsContainer)
                    {
                        BaseItem item = objSub.ItemAtLocation(x, y);
                        if (item != null)
                            return item;
                    }
                    else
                    {
                        if ((objSub.Visible || this.IsOnCustomizeMenu) && objSub.Displayed && objSub.DisplayRectangle.Contains(x, y))
                        {
                            if (this.DesignMode && !this.DisplayRectangle.IntersectsWith(objSub.DisplayRectangle))
                                continue;
                            return objSub;
                        }
                    }
                }
            }

            if (this.DisplayRectangle.Contains(x, y) && /*this.DesignMode &&*/ !this.SystemContainer)
                return this;
            return null;
        }

		/// <summary>
		/// Gets or sets orientation inside the container. Do not change the value of this property. It is managed by system only.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override eOrientation Orientation
		{
			get {return eOrientation.Horizontal;}
			set	{}
		}

		/// <summary>
		/// Gets or sets orientation inside the container.
		/// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(eOrientation.Horizontal), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Layout")]
		public virtual eOrientation LayoutOrientation
		{
			get {return m_LayoutOrientation;}
			set
			{
				m_LayoutOrientation=value;
				OnOrientationChanged();
			}
		}

        /// <summary>
        /// Gets or sets whether items contained by container are resized to fit the container bounds. When container is in horizontal
        /// layout mode then all items will have the same height. When container is in vertical layout mode then all items
        /// will have the same width. Default value is true.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(true), Category("Layout"), Description("Indicates whether items contained by container are resized to fit the container bounds. When container is in horizontal layout mode then all items will have the same height. When container is in vertical layout mode then all items will have the same width.")]
        public virtual bool ResizeItemsToFit
        {
            get { return m_ResizeItemsToFit; }
            set
            {
                m_ResizeItemsToFit = value;
                this.OnAppearanceChanged();
            }
        }

        ///// <summary>
        ///// Gets or sets vertical alignment of the items per line. Default value is top.
        ///// </summary>
        //[Browsable(true), DevCoBrowsable(true), DefaultValue(eContainerVerticalAlignment.Top), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public virtual eContainerVerticalAlignment LineAlignment
        //{
        //    get { return m_LineAlignment; }
        //    set
        //    {
        //        m_LineAlignment = value;
        //        OnLineAlignmentChanged();
        //    }
        //}

        private void OnLineAlignmentChanged()
        {
            NeedRecalcSize = true;
            OnAppearanceChanged();
        }

		private void OnOrientationChanged()
		{
			NeedRecalcSize=true;
			if(m_LayoutManager!=null)
				m_LayoutManager.ContentOrientation=(this.LayoutOrientation==eOrientation.Horizontal?eContentOrientation.Horizontal:eContentOrientation.Vertical);
            OnAppearanceChanged();
		}

		/// <summary>
		/// IBlock member implementation
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Rectangle Bounds
		{
			get
			{
				return base.Bounds;
			}
			set
			{
                Point offset=new Point(value.X-m_Rect.X,value.Y-m_Rect.Y);
				base.Bounds=value;

                if (m_HorizontalItemAlignment != eHorizontalItemsAlignment.Left || m_LayoutManager!=null && m_LayoutManager.RightToLeft)
                {
                    LayoutItems(value);
                }
                else
                {
                    if (!offset.IsEmpty)
                    {
                        OffsetSubItems(offset);
                    }
                }
			}
		}

        private bool _Offsetting = false;
		internal void OffsetSubItems(Point offset)
		{
			if(_Offsetting || offset.IsEmpty)
				return;

            try
            {
                _Offsetting = true;
                BaseItem[] items = new BaseItem[this.SubItems.Count];
                this.SubItems.CopyTo(items, 0);
                foreach (IBlock b in items)
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
		/// Called after TopInternal property has changed
		/// </summary>
		protected override void OnTopLocationChanged(int oldValue)
		{
			base.OnTopLocationChanged(oldValue);
			Point offset=new Point(0,this.TopInternal-oldValue);
			OffsetSubItems(offset);
		}

		/// <summary>
		/// Called after LeftInternal property has changed
		/// </summary>
		protected override void OnLeftLocationChanged(int oldValue)
		{
			base.OnLeftLocationChanged(oldValue);
			Point offset=new Point(this.LeftInternal-oldValue,0);
			OffsetSubItems(offset);
		}

		/// <summary>
		/// Returns copy of the item.
		/// </summary>
		public override BaseItem Copy()
		{
			ItemContainer objCopy=new ItemContainer();
			this.CopyToItem(objCopy);
			return objCopy;
		}
		/// <summary>
		/// Copies the ButtonItem specific properties to new instance of the item.
		/// </summary>
		/// <param name="c">New ButtonItem instance.</param>
		protected override void CopyToItem(BaseItem c)
		{
			ItemContainer copy=c as ItemContainer;
            copy.BackgroundStyle.ApplyStyle(m_BackgroundStyle);
            copy.HorizontalItemAlignment = this.HorizontalItemAlignment;
            copy.ItemSpacing = this.ItemSpacing;
            copy.LayoutOrientation = this.LayoutOrientation;
            copy.MinimumSize = this.MinimumSize;
            copy.MultiLine = this.MultiLine;
            copy.ResizeItemsToFit = this.ResizeItemsToFit;
            copy.VerticalItemAlignment = this.VerticalItemAlignment;
			base.CopyToItem(copy);
			
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public override bool Expanded
		{
			get
			{
				return m_Expanded;
			}
			set
			{
				base.Expanded=value;
				if(!value)
					BaseItem.CollapseSubItems(this);
			}
		}

		/// <summary>
		/// Occurs when sub item expanded state has changed.
		/// </summary>
		/// <param name="item">Sub item affected.</param>
		protected internal override void OnSubItemExpandChange(BaseItem item)
		{
			base.OnSubItemExpandChange(item);
			if(item.Expanded)
				this.Expanded=true;
		}

        protected internal override void OnSubItemsClear()
        {
            this.RefreshImageSize();
            base.OnSubItemsClear();
        }

        /// <summary>
        /// Called when item Display state has changed.
        /// </summary>
        protected override void OnDisplayedChanged()
        {
            base.OnDisplayedChanged();

            
            foreach (BaseItem item in this.SubItems)
            {
                if (item.Visible) item.Displayed = this.Displayed;
            }

            if (!this.Displayed)
                this.InternalMouseLeave();
        }

		/// <summary>
		/// Returns whether instance of the item container is used as system container internally by DotNetBar.
		/// </summary>
		[Browsable(false)]
        public bool SystemContainer
		{
			get {return m_SystemContainer;}
		}

		/// <summary>
		/// Sets whether container is used as system container internally by DotNetBar.
		/// </summary>
		/// <param name="b">true or false to indicate whether container is system container or not.</param>
		internal void SetSystemContainer(bool b)
		{
			m_SystemContainer=b;
		}

		/// <summary>
		/// Gets or sets the accessible role of the item.
		/// </summary>
		[DevCoBrowsable(true),Browsable(true),Category("Accessibility"),Description("Gets or sets the accessible role of the item."),DefaultValue(System.Windows.Forms.AccessibleRole.Grouping)]
		public override System.Windows.Forms.AccessibleRole AccessibleRole
		{
			get {return base.AccessibleRole;}
			set {base.AccessibleRole=value;}
		}
		#endregion

		#region Factories
		private SerialContentLayoutManager m_LayoutManager=null;
		protected virtual IContentLayout GetContentLayout()
		{
			if(m_LayoutManager==null)
			{
				m_LayoutManager=new SerialContentLayoutManager();
				m_LayoutManager.BlockSpacing=1;
				m_LayoutManager.FitContainerOversize=false;
                m_LayoutManager.FitContainer = false;
                m_LayoutManager.BeforeNewBlockLayout += new LayoutManagerLayoutEventHandler(LayoutManager_BeforeNewBlockLayout);
			}

            m_LayoutManager.MultiLine = m_MultiLine;
            m_LayoutManager.EvenHeight = m_ResizeItemsToFit;
            
            if (m_MinimumSize.Width > 0 && m_LayoutOrientation == eOrientation.Vertical)
                m_LayoutManager.VerticalFitContainerWidth = true;
            else
                m_LayoutManager.VerticalFitContainerWidth = false;

            if (m_MinimumSize.Height > 0 && m_LayoutOrientation == eOrientation.Horizontal)
                m_LayoutManager.HorizontalFitContainerHeight = true;
            else
                m_LayoutManager.HorizontalFitContainerHeight = false;

            m_LayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
            //if(m_LineAlignment==eContainerVerticalAlignment.Middle)
            //    m_LayoutManager.BlockLineAlignment = eContentVerticalAlignment.Middle;
            //else if (m_LineAlignment == eContainerVerticalAlignment.Bottom)
            //    m_LayoutManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;

            m_LayoutManager.ContentAlignment = GetContentAlignment();
            m_LayoutManager.ContentVerticalAlignment = GetVerticalContentAlignment(); // eContentVerticalAlignment.Top;
            m_LayoutManager.ContentOrientation = (this.LayoutOrientation == eOrientation.Horizontal ? eContentOrientation.Horizontal : eContentOrientation.Vertical);
            m_LayoutManager.RightToLeft = false;

            Control containerControl = this.ContainerControl as Control;
            if (containerControl != null && containerControl.RightToLeft == RightToLeft.Yes)
                m_LayoutManager.RightToLeft = true;
            else
                m_LayoutManager.RightToLeft = false;

            m_LayoutManager.BlockSpacing = m_ItemSpacing - (this.IsGroupingContainer ? 1 : 0);

			return m_LayoutManager;
		}

        private void LayoutManager_BeforeNewBlockLayout(object sender, LayoutManagerLayoutEventArgs e)
        {
            BaseItem item = e.Block as BaseItem;
            if (item != null && IsGroupingContainer && !(item is ItemContainer) && e.BlockVisibleIndex>0)
            {
                if (m_LayoutOrientation == eOrientation.Horizontal)
                    e.CurrentPosition.X += m_BeginGroupSpacing;
                else
                    e.CurrentPosition.Y += m_BeginGroupSpacing;
            }
        }


        private eContentAlignment GetContentAlignment()
        {
            if (m_HorizontalItemAlignment == eHorizontalItemsAlignment.Left /*|| m_LayoutOrientation == eOrientation.Vertical*/)
                return eContentAlignment.Left;
            else if (m_HorizontalItemAlignment == eHorizontalItemsAlignment.Center)
                return eContentAlignment.Center;
            else
                return eContentAlignment.Right;
        }

        private eContentVerticalAlignment GetVerticalContentAlignment()
        {
            if (m_VerticalItemAlignment == eVerticalItemsAlignment.Top)
                return eContentVerticalAlignment.Top;
            else if (m_VerticalItemAlignment == eVerticalItemsAlignment.Middle)
                return eContentVerticalAlignment.Middle;
            else if (m_VerticalItemAlignment == eVerticalItemsAlignment.Bottom)
                return eContentVerticalAlignment.Bottom;

            return eContentVerticalAlignment.Top;
        }

		private BlockLayoutManager GetBlockLayoutManager()
		{
			return new ItemBlockLayoutManager();
		}
		
		private ItemDisplay m_ItemDisplay=null;
		internal ItemDisplay GetItemDisplay()
		{
			if(m_ItemDisplay==null)
				m_ItemDisplay=new ItemDisplay();
			return m_ItemDisplay;
		}
		#endregion

		#region IDesignTimeProvider Implementation
        protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
        {
            return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
        }
		InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
		{
            return GetContainerInsertPosition(pScreen, dragItem);
		}
		void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
		{
			DesignTimeProviderContainer.DrawReversibleMarker(this,iPos,Before);
		}
		void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
		{
			DesignTimeProviderContainer.InsertItemAt(this,objItem,iPos,Before);
		}

		#endregion

		#region Property Hiding From Design-time
        /// <summary>
        /// Gets or sets the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar. Use KeyTips property
        /// when you want to assign the one or more letters to be used to access an item. For example assigning the FN to KeyTips property
        /// will require the user to press F then N keys to select an item. Pressing the F letter will show only keytips for the items that start with letter F.
        /// </summary>
        [Browsable(false), Category("Appearance"), DefaultValue(""), Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
        public override string KeyTips
        {
            get { return base.KeyTips; }
            set { base.KeyTips = value; }
        }

		/// <summary>
		/// Indicates whether the item will auto-collapse (fold) when clicked. 
		/// When item is on popup menu and this property is set to false, menu will not
		/// close when item is clicked.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.DefaultValue(true),System.ComponentModel.Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
		public override bool AutoCollapseOnClick
		{
			get {return base.AutoCollapseOnClick;}
			set {base.AutoCollapseOnClick=value;}
		}

		/// <summary>
		/// Gets or sets whether item can be customized by end user.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether item can be customized by user.")]
		public override bool CanCustomize
		{
			get {return base.CanCustomize;}
			set {base.CanCustomize=value;}
		}

		/// <summary>
		/// Returns category for this item. If item cannot be customzied using the
		/// customize dialog category is empty string.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(""),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Indicates item category used to group similar items at design-time.")]
		public override string Category
		{
			get { return base.Category; }
			set { base.Category=value; }
		}

		/// <summary>
		/// Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
		public override bool ClickAutoRepeat
		{
			get { return base.ClickAutoRepeat; }
			set {base.ClickAutoRepeat=value;}
		}

		/// <summary>
		/// Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(600),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
		public override int ClickRepeatInterval
		{
			get {return base.ClickRepeatInterval;}
			set {base.ClickRepeatInterval=value;}
		}

		/// <summary>
		/// Specifes the mouse cursor displayed when mouse is over the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(null),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies the mouse cursor displayed when mouse is over the item.")]
		public override System.Windows.Forms.Cursor Cursor
		{
			get { return base.Cursor; }
			set { base.Cursor=value; }
		}

		/// <summary>
		/// Gets or sets item description. This description is displayed in
		/// Customize dialog to describe the item function in an application.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(""),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Indicates description of the item that is displayed during design.")]
		public override string Description
		{
			get { return base.Description; }
			set { base.Description=value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is enabled.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether is item enabled.")]
		public override bool Enabled
		{
			get { return base.Enabled; }
			set { base.Enabled=value; }
		}

		/// <summary>
		/// Gets or sets whether item is global or not.
		/// This flag is used to propagate property changes to all items with the same name.
		/// Setting for example Visible property on the item that has GlobalItem set to true will
		/// set visible property to the same value on all items with the same name.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
		public override bool GlobalItem
		{
			get { return base.GlobalItem; }
			set { base.GlobalItem=value; }
		}

		/// <summary>
		/// Gets or sets item alignment inside the container.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(eItemAlignment.Near),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Determines alignment of the item inside the container.")]
		public override eItemAlignment ItemAlignment
		{
			get { return base.ItemAlignment;}
			set { base.ItemAlignment=value;}
		}

		/// <summary>
		/// Gets or sets the collection of shortcut keys associated with the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Indicates list of shortcut keys for this item."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public override ShortcutsCollection Shortcuts
		{
			get { return base.Shortcuts; }
			set { base.Shortcuts=value; }
		}

		/// <summary>
		/// Gets or sets whether item will display sub items.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Determines whether sub-items are displayed.")]
		public override bool ShowSubItems
		{
			get { return base.ShowSubItems; }
			set { base.ShowSubItems=value; }
		}

		/// <summary>
		/// Gets or sets whether the item expands automatically to fill out the remaining space inside the container. Applies to Items on stretchable, no-wrap Bars only.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
		public override bool Stretch
		{
			get { return base.Stretch; }
			set { base.Stretch=value; }
		}

		/// <summary>
		/// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public override bool ThemeAware
		{
			get { return base.ThemeAware; }
			set { base.ThemeAware=value; }
		}

		/// <summary>
		/// Gets/Sets informational text (tooltip) for the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(""),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the text that is displayed when mouse hovers over the item."),System.ComponentModel.Localizable(true)]
		public override string Tooltip
		{
			get { return base.Tooltip; }
			set { base.Tooltip=value; }
		}

        ///// <summary>
        ///// Gets the container padding. Padding is the spacing inside of the container.
        ///// </summary>
        //[Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets the container padding"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public Spacing Padding
        //{
        //    get
        //    {
        //        return m_Padding;
        //    }
        //}

        ///// <summary>
        ///// Gets the container margin. Margin is the spacing outside of the container.
        ///// </summary>
        //[Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets the container margin"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public Spacing Margin
        //{
        //    get
        //    {
        //        return m_Margin;
        //    }
        //}

        /// <summary>
        /// Selects first visible item contained by the container by sending mouse over message.
        /// </summary>
        internal bool SelectFirstItem()
        {
            foreach (BaseItem item in this.SubItems)
            {
                if (item.Visible && item.Displayed)
                {
                    if (item is ItemContainer)
                    {
                        if (((ItemContainer)item).SelectFirstItem())
                        {
                            SetHotSubItem(null);
                            HotSubItem = item;
                            return true;
                        }
                    }
                    else if (CanGetMouseFocus(item))
                    {
                        SetHotSubItem(item);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanGetMouseFocus(BaseItem item)
        {
            if (item is LabelItem)
                return false;
            return true;
        }

        /// <summary>
        /// Sets the new hot-sub item for the container. This method is designed for internal use by the DotNetBar and should not be used.
        /// </summary>
        /// <param name="item">Reference to an instance of BaseItem or null.</param>
        public void SetHotSubItem(BaseItem item)
        {
            if (item == m_HotSubItem)
                return;

            // Select next object
            if (m_HotSubItem != null)
            {
                m_HotSubItem.InternalMouseLeave();
                if (m_AutoExpand && m_HotSubItem.Expanded)
                {
                    m_HotSubItem.Expanded = false;
                }
                HotSubItem = null;
            }

            if (item != null)
            {
                HotSubItem = item;
                m_HotSubItem.InternalMouseEnter();
                m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 1, m_HotSubItem.TopInternal + 1, 0));
                IScrollableItemControl isc = this.ContainerControl as IScrollableItemControl;
                if (isc != null)
                    isc.KeyboardItemSelected(m_HotSubItem);
            }
        }

        ///// <summary>
        ///// Gets the current expanded sub-item.
        ///// </summary>
        ///// <returns></returns>
        //protected internal override BaseItem ExpandedItem()
        //{
        //    BaseItem exp = base.ExpandedItem();
        //    if (exp != null) return exp;

        //    foreach (BaseItem item in this.SubItems)
        //    {
        //        if (item is ItemContainer)
        //        {
        //            exp = item.ExpandedItem();
        //            if (exp != null) return exp;
        //        }
        //    }

        //    return null;
        //}

        public override void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
        {
            base.InternalKeyDown(objArg);

            ContainerKeyboardNavigation.ProcessKeyDown(this, objArg);
        }
		#endregion
	}
}
