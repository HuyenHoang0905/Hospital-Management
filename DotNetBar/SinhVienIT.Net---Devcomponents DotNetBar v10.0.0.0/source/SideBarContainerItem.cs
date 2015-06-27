using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Provides layout for Side-Bar control.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class SideBarContainerItem:ImageItem, IDesignTimeProvider
	{
		#region Private Variables
		private eSideBarAppearance m_Appearance=eSideBarAppearance.Traditional;
		private bool m_ProcessingExpanded=false;
		private int m_ItemSpacing=1;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Creates new instance of SideBarContainerItem class.
		/// </summary>
		public SideBarContainerItem()
		{
			m_IsContainer=true;
			m_SystemItem=true;
			m_SupportedOrientation=eSupportedOrientation.Horizontal;
            m_AllowOnlyOneSubItemExpanded = false;
		}
		/// <summary>
		/// Returns copy of SideBarContainerItem item
		/// </summary>
		public override BaseItem Copy()
		{
			SideBarContainerItem objCopy=new SideBarContainerItem();
			this.CopyToItem(objCopy);
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			SideBarContainerItem objCopy=copy as SideBarContainerItem;
			base.CopyToItem(objCopy);
		}

		/// <summary>
		/// Recalculates the size of the item
		/// </summary>
		public override void RecalcSize()
		{
			// This container will always have one and only one item expanded at the time.
			// Recalculation of the size for this item is different then rest of the DotNetBar.
			// This container always takes the size assigned by the Control and never changes
			// the Size by itself.
			int iUnexpandItemsHeight=0;
			int iTotalHeight=0;
			bool bOverflow=false;		// Too many items so there is no need to stretch the expanded since it can't fit

			if(m_SubItems!=null)
			{
				SideBarPanelItem expanded=null;
				foreach(BaseItem item in m_SubItems)
				{
					if(item.Visible)
					{
						if(iTotalHeight>this.HeightInternal)
						{
							item.Displayed=false;
							bOverflow=true;
							continue;
						}

						// Give it our maximum size
						item.WidthInternal=this.WidthInternal;
						item.HeightInternal=0;
						item.RecalcSize();
						if(item.WidthInternal!=this.WidthInternal)
							item.WidthInternal=this.WidthInternal;
						if(item.Expanded && item is SideBarPanelItem && expanded==null)
							expanded=item as SideBarPanelItem;

						// Set item position
                        item.LeftInternal=m_Rect.Left;
						item.TopInternal=m_Rect.Top+iTotalHeight;
						iTotalHeight+=m_ItemSpacing;

						// Needed to set the expanded item height and to reposition other items...
						if(!item.Expanded)
							iUnexpandItemsHeight+=(item.HeightInternal+m_ItemSpacing);

						// Remember Total Height
						iTotalHeight+=item.HeightInternal;

						item.Displayed=true;
					}
				}

				if(expanded!=null && !bOverflow)
				{
					// Expanded item is always "stretched" to fill the space...
					expanded.HeightInternal=m_Rect.Height-iUnexpandItemsHeight;
					expanded.RecalcSize();
                    int iOffsetTop = expanded.DisplayRectangle.Bottom + m_ItemSpacing;
					if(m_Appearance==eSideBarAppearance.Flat)
						iOffsetTop--;
					// Move the items after the expanded item to bottom
					for(int i=m_SubItems.IndexOf(expanded)+1;i<m_SubItems.Count;i++)
					{
						BaseItem item=m_SubItems[i];
						if(item.Visible)
						{
							item.TopInternal=iOffsetTop;
							iOffsetTop+=(item.HeightInternal+m_ItemSpacing);
						}
					}
				}
			}

			base.RecalcSize();
		}

		/// <summary>
		/// Paints this base container
		/// </summary>
		public override void Paint(ItemPaintArgs pa)
		{
			if(this.SuspendLayout)
				return;
			System.Drawing.Graphics g=pa.Graphics;
			if(m_NeedRecalcSize)
				RecalcSize();

			if(m_SubItems==null)
				return;

			foreach(BaseItem item in m_SubItems)
			{
				if(item.Visible && item.Displayed)
				{
					item.Paint(pa);
				}
			}
		}

		[System.ComponentModel.Browsable(false)]
		public override eOrientation Orientation
		{
			get
			{
				return eOrientation.Horizontal;
			}
			set
			{
				return;
			}
		}

		/// <summary>
		/// Occurs when sub item expanded state has changed.
		/// </summary>
		/// <param name="item">Sub item affected.</param>
		protected internal override void OnSubItemExpandChange(BaseItem item)
		{
			// Prevent re-entrancy
			if(m_ProcessingExpanded)
				return;

			m_ProcessingExpanded=true;
			try
			{
				// We have to make sure that only one item is always expanded at a time
				if(item.Expanded)
				{
					foreach(BaseItem subitem in m_SubItems)
					{
						if(subitem.Expanded && subitem!=item)
							subitem.Expanded=false;
					}
				}
				else
				{
					bool bOneExpanded=false;
					foreach(BaseItem subitem in m_SubItems)
					{
						if(subitem.Expanded)
						{
							bOneExpanded=true;
							break;
						}
					}
					if(!bOneExpanded)
						item.Expanded=true;
				}
				this.RecalcSize();
				System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
				if(container!=null)
					container.Refresh();
			}
			finally
			{
				m_ProcessingExpanded=false;
			}
		}

		internal eSideBarAppearance Appearance
		{
			get {return m_Appearance;}
			set
			{
				if(m_Appearance!=value)
				{
					m_Appearance=value;
					if(m_Appearance==eSideBarAppearance.Flat)
						m_ItemSpacing=-1;
					else
						m_ItemSpacing=1;
					foreach(SideBarPanelItem item in this.SubItems)
					{
						item.Appearance=value;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public override bool Expanded
		{
			get
			{
				return base.Expanded;
			}
			set
			{
				if(!value)
				{
					foreach(BaseItem item in m_SubItems)
					{
						if(item is SideBarPanelItem)
						{
							foreach(BaseItem popup in item.SubItems)
							{
								if(popup is PopupItem && item.Expanded)
									popup.Expanded=false;
							}
						}
					}
				}
				base.Expanded=value;
			}
		}

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is pressed. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseDown(objArg);
			if(this.DesignMode)
			{
				if(this.ItemAtLocation(objArg.X,objArg.Y)==null)
				{
					IOwner owner=this.GetOwner() as IOwner;
					if(owner!=null)
						owner.SetFocusItem(null);
				}
			}
		}

		/// <summary>
		/// Sets input focus to next visible item in Explorer Bar.
		/// </summary>
		/// <returns>True if focus was set to next visible item otherwise false.</returns>
		public bool FocusNextItem()
		{
			bool bBaseCall=true;
			
			BaseItem focusItem=((IOwner)this.GetOwner()).GetFocusItem();
			bool bFocusNext=false;
			if(focusItem==null)
				bFocusNext=true;
			int iLoopCount=0;
			while(iLoopCount<2)
			{
				foreach(BaseItem item in this.SubItems)
				{
					if(item==focusItem)
						bFocusNext=true;
					else if(item.Visible && bFocusNext) 
					{
						((IOwner)this.GetOwner()).SetFocusItem(item);
						iLoopCount=2;
						bBaseCall=false;
						break;
					}
					if(item.Expanded && item.Visible)
					{
						foreach(BaseItem child in item.SubItems)
						{
							if(child==focusItem)
								bFocusNext=true;
							else if(item.Visible && bFocusNext) 
							{
								((IOwner)this.GetOwner()).SetFocusItem(child);
								iLoopCount=2;
								bBaseCall=false;
								break;
							}
						}
						if(iLoopCount==2)
							break;
					}
				}
				iLoopCount++;
			}				

			return bBaseCall;
		}

		/// <summary>
		/// Sets input focus to previous visible item in Explorer Bar.
		/// </summary>
		/// <returns>True if focus was set to previous visible item otherwise false.</returns>
		public bool FocusPreviousItem()
		{
			bool bBaseCall=true;
			
			BaseItem focusItem=((IOwner)this.GetOwner()).GetFocusItem();
			bool bFocusNext=false;
			if(focusItem==null)
				bFocusNext=true;
			int iLoopCount=0;
			while(iLoopCount<2)
			{
				for(int groupIndex=this.SubItems.Count-1;groupIndex>=0;groupIndex--)
				{
					BaseItem item=this.SubItems[groupIndex];
					
					if(item.Expanded && item.Visible)
					{
						for(int index=item.SubItems.Count-1;index>=0;index--)
						{
							BaseItem child=item.SubItems[index];
							if(child==focusItem)
								bFocusNext=true;
							else if(item.Visible && bFocusNext) 
							{
								((IOwner)this.GetOwner()).SetFocusItem(child);
								iLoopCount=2;
								bBaseCall=false;
								break;
							}
						}
						if(iLoopCount==2)
							break;
					}

					if(item==focusItem)
						bFocusNext=true;
					else if(item.Visible && bFocusNext) 
					{
						((IOwner)this.GetOwner()).SetFocusItem(item);
						iLoopCount=2;
						bBaseCall=false;
						break;
					}
				}
				iLoopCount++;
			}				

			return bBaseCall;
		}

		protected internal override void OnItemAdded(BaseItem item)
		{
			base.OnItemAdded(item);
			NeedRecalcSize=true;
			if(this.ContainerControl is SideBar && item is SideBarPanelItem)
			{
				SideBarPanelItem panel=item as SideBarPanelItem;
				panel.Appearance=((SideBar)this.ContainerControl).Appearance;
				panel.RefreshItemStyleSystemColors();
                //if(panel.Appearance==eSideBarAppearance.Flat && this.DesignMode)
                //{
                //    if(!panel.HasFlatStyle)
                //        SideBar.ApplyColorScheme((SideBarPanelItem)item,((SideBar)this.ContainerControl).PredefinedColorScheme);
                //}
			}
			if(m_SubItems.Count==1)
				item.Expanded=true;
			if(this.DesignMode)
				this.Refresh();
		}

		protected internal override void OnAfterItemRemoved(BaseItem item)
		{
			base.OnAfterItemRemoved(item);
			NeedRecalcSize=true;
			if(this.DesignMode)
				this.Refresh();
		}

		protected internal override void OnSubItemsClear()
		{
			base.OnSubItemsClear();
			NeedRecalcSize=true;
			if(this.DesignMode)
				this.Refresh();
		}
		#endregion

		#region IDesignTimeProvider Implementation
		
		InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
		{
			foreach(BaseItem panel in this.SubItems)
			{
				if(!panel.Visible)
					continue;
				if(panel is IDesignTimeProvider)
				{
					InsertPosition pos=((IDesignTimeProvider)panel).GetInsertPosition(pScreen, dragItem);
					if(pos!=null)
					{
						return pos;
					}
				}
			}
			return null;
		}
		void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before){}
		void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before){}

		#endregion
	}
}
