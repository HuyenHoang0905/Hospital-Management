namespace DevComponents.DotNetBar
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Adds neccessary functions to base item so it supports images properly.
    /// If your item implements images you should derive from this class instead of BaseItem
    /// </summary>
    [System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
    public abstract class ImageItem:BaseItem
    {
		private System.Drawing.Size m_SubItemsImageSize;  // Biggest Image size for the sub items, they would access this property so they know how to draw themself
		private System.Drawing.Size m_ImageSize;
		internal static System.Drawing.Size _InitalImageSize=new System.Drawing.Size(16,16);
		/// <summary>
		/// Create new instance of ImageItem.
		/// </summary>
		public ImageItem():this("","") {}
		/// <summary>
		/// Create new instance of ImageItem and assigns the item name.
		/// </summary>
		/// <param name="sName">Item name.</param>
		public ImageItem(string sName):this(sName,"") {}
		/// <summary>
		/// Create new instance of ImageItem and assigns the item name and text.
		/// </summary>
		/// <param name="sName">Item name.</param>
		/// <param name="ItemText">Item text.</param>
        public ImageItem(string sName, string ItemText):base(sName, ItemText)
        {
			m_SubItemsImageSize=_InitalImageSize;
			m_ImageSize=_InitalImageSize;
        }

		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual System.Drawing.Size ImageSize
		{
			get
			{
				return m_ImageSize;
			}
			set
			{
				m_ImageSize=value;
			}
		}

		/// <summary>
		/// When parent items does recalc size for its sub-items it should query
		/// image size and store biggest image size into this property.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual System.Drawing.Size SubItemsImageSize
		{
			get
			{
				return m_SubItemsImageSize;
			}
			set
			{
				m_SubItemsImageSize=value;
			}
		}

		protected internal override void OnItemAdded(BaseItem objItem)
		{
			base.OnItemAdded(objItem);
			ImageItem objImageItem=objItem as ImageItem;
			if(objImageItem!=null)
			{
                Size newSize = m_SubItemsImageSize;
				if(objImageItem.ImageSize.Width>m_SubItemsImageSize.Width)
                    newSize.Width = objImageItem.ImageSize.Width;
				if(objImageItem.ImageSize.Height>m_SubItemsImageSize.Height)
                    newSize.Height = objImageItem.ImageSize.Height;
                this.SubItemsImageSize = newSize;
			}
		}

		/*public override void AddSubItem(BaseItem objItem, int Position)
		{
			base.AddSubItem(objItem, Position);
			ImageItem objImageItem=objItem as ImageItem;
			if(objImageItem!=null)
			{
				if(objImageItem.ImageSize.Width>m_SubItemsImageSize.Width)
					m_SubItemsImageSize.Width=objImageItem.ImageSize.Width;
				if(objImageItem.ImageSize.Height>m_SubItemsImageSize.Height)
					m_SubItemsImageSize.Height=objImageItem.ImageSize.Height;
			}
		}*/

		/// <summary>
		/// Must be called by any sub item that implements the image when image has changed
		/// </summary>
		public virtual void OnSubItemImageSizeChanged(BaseItem objItem)
		{
			ImageItem objImageItem=objItem as ImageItem;
			if(objImageItem==null)
				return;
			if(this.SubItems.Count==1)
			{
                this.SubItemsImageSize = new Size(objImageItem.ImageSize.Width, objImageItem.ImageSize.Height);
			}
			else
			{
                Size newSize = m_SubItemsImageSize;
				if(objImageItem.ImageSize.Width>m_SubItemsImageSize.Width)
					newSize.Width=objImageItem.ImageSize.Width;
				if(objImageItem.ImageSize.Height>m_SubItemsImageSize.Height)
                    newSize.Height = objImageItem.ImageSize.Height;
                this.SubItemsImageSize = newSize;
			}
		}

		protected override void OnIsOnCustomizeDialogChanged()
		{
			if(this.IsOnCustomizeDialog && m_ImageSize.Width==_InitalImageSize.Width && m_ImageSize.Height==_InitalImageSize.Height)
                m_ImageSize=new System.Drawing.Size(16,16);
			base.OnIsOnCustomizeDialogChanged();
		}

		/// <summary>
		/// Called after image on an item has changed.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void OnImageChanged(){}

		/// <summary>
		/// Refreshes internal image size structure.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void RefreshImageSize()
		{
			this.SubItemsImageSize=_InitalImageSize;
			m_ImageSize=_InitalImageSize;
			this.OnImageChanged();
			if(m_SubItems!=null)
			{
				foreach(BaseItem item in this.SubItems)
				{
					if(item is ImageItem)
						((ImageItem)item).RefreshImageSize();
				}
			}
		}

        /// <summary>
        /// Occurs after an item has been removed.
        /// </summary>
        /// <param name="item">Item being removed.</param>
        protected internal override void OnAfterItemRemoved(BaseItem item)
        {
            base.OnAfterItemRemoved(item);
            if (item != null)
                this.RefreshSubItemImageSize();
        }

		/// <summary>
		/// Refreshes internal image size structure.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void RefreshSubItemImageSize()
		{
			this.SubItemsImageSize=System.Drawing.Size.Empty;
			if(m_SubItems!=null)
			{
				foreach(BaseItem item in this.SubItems)
				{
					ImageItem objImageItem=item as ImageItem;
					if(objImageItem!=null)
					{
                        Size newSize = m_SubItemsImageSize;
						if(objImageItem.ImageSize.Width>m_SubItemsImageSize.Width)
							newSize.Width=objImageItem.ImageSize.Width;
						if(objImageItem.ImageSize.Height>m_SubItemsImageSize.Height)
							newSize.Height=objImageItem.ImageSize.Height;
                        this.SubItemsImageSize = newSize;
					}
				}
			}

            if (this.SubItemsImageSize.IsEmpty)
                this.SubItemsImageSize = _InitalImageSize;
		}
    }
}
