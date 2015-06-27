using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents the container which layouts items vertically.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class VerticalContainerItem:ImageItem
	{
		#region Private Variables
		private int m_ItemSpacing=0;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public VerticalContainerItem():base()
		{
			m_IsContainer=true;
			m_SystemItem=true;
			m_SupportedOrientation=eSupportedOrientation.Horizontal;
			m_AllowOnlyOneSubItemExpanded=false;
		}
		#endregion

		#region Overrides Implementation
		/// <summary>
		/// Returns copy of VerticalContainerItem item
		/// </summary>
		public override BaseItem Copy()
		{
			VerticalContainerItem objCopy=new VerticalContainerItem();
			this.CopyToItem(objCopy);
			return objCopy;
		}
		/// <summary>
		/// Copies this object into the different instance.
		/// </summary>
		/// <param name="copy">Instance of the object to copy to. Must be of the same type.</param>
		protected override void CopyToItem(BaseItem copy)
		{
			VerticalContainerItem objCopy=copy as VerticalContainerItem;
			base.CopyToItem(objCopy);
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
						if(item.IsContainer)
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
		/// Recalculates the size of the item
		/// </summary>
		public override void RecalcSize()
		{
			// Vertical Container can have none or one or more container items expanded at a time
			// The Control itself is displaying the scroll-bars and handling the
			// case when this container is oversized. It is only possible to have
			// vertical oversize. The items are always made to fit horizontally
			int iY=m_Rect.Top;
			if(m_SubItems!=null)
			{
				foreach(BaseItem item in m_SubItems)
				{
					if(item.Visible)
					{
						// Give it our maximum size
						item.WidthInternal=this.WidthInternal;
						item.HeightInternal=0;
						// Set item position
						item.LeftInternal=m_Rect.Left;
						item.TopInternal=iY;

						item.RecalcSize();
						if(item.WidthInternal!=this.WidthInternal)
							item.WidthInternal=this.WidthInternal;
						
						iY+=(item.HeightInternal+m_ItemSpacing);

						item.Displayed=true;
					}
				}
			}
			iY-=m_ItemSpacing;
			this.HeightInternal=iY;
			base.RecalcSize();
		}

		/// <summary>
		/// Indicates whether TopInternal property has changed.
		/// </summary>
		/// <param name="oldValue">Specifies old value of TopInternal property.</param>
		protected override void OnTopLocationChanged(int oldValue)
		{
			int iDiff=m_Rect.Top-oldValue;
			if(m_SubItems!=null)
			{
				foreach(BaseItem item in m_SubItems)
				{
					if(item.Visible)
					{
						// Set item position
						item.TopInternal+=iDiff;
					}
				}
			}
		}

		/// <summary>
		/// Paints contained items.
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
					if(item is IContainerWordWrap)
					{
						if(((IContainerWordWrap)item).WordWrapSubItems)
							pa.ButtonStringFormat.FormatFlags=pa.ButtonStringFormat.FormatFlags & ~(pa.ButtonStringFormat.FormatFlags & StringFormatFlags.NoWrap);
						else
							pa.ButtonStringFormat.FormatFlags=pa.ButtonStringFormat.FormatFlags | StringFormatFlags.NoWrap;
					}
					else
						pa.ButtonStringFormat.FormatFlags=pa.ButtonStringFormat.FormatFlags & ~(pa.ButtonStringFormat.FormatFlags & StringFormatFlags.NoWrap);
					item.Paint(pa);
				}
			}
		}

		/// <summary>
		/// Gets the orientation of the container. Cannot be used on VerticalContainerItem.
		/// </summary>
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
		/// Occurs when the mouse pointer is over the item and a mouse button is released. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseUp(objArg);
			}
			else
				base.InternalMouseUp(objArg);
		}

		/// <summary>
		/// Sets input focus to next visible item in container.
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

		/// <summary>
		/// Occurs after an item has been added to the container. This procedure is called on both item being added and the parent of the item. To distinguish between those two states check the item parameter.
		/// </summary>
		/// <param name="item">When occurring on the parent this will hold the reference to the item that has been added. When occurring on the item being added this will be null (Nothing).</param>
		protected internal override void OnItemAdded(BaseItem item)
		{
			base.OnItemAdded(item);
			m_NeedRecalcSize=true;
			if(this.DesignMode)
			{
				BarBaseControl bar=this.ContainerControl as BarBaseControl;
				if(bar!=null)
					this.Refresh();
			}
		}

		/// <summary>
		/// Occurs after an item has been removed.
		/// </summary>
		/// <param name="item">Item being removed.</param>
		protected internal override void OnAfterItemRemoved(BaseItem item)
		{
			base.OnAfterItemRemoved(item);
			m_NeedRecalcSize=true;
			if(this.DesignMode)
				this.Refresh();
		}

		/// <summary>
		/// Occurs after SubItems Collection has been cleared.
		/// </summary>
		protected internal override void OnSubItemsClear()
		{
			base.OnSubItemsClear();
			m_NeedRecalcSize=true;
			if(this.DesignMode)
				this.Refresh();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the vertical spacing between items in pixels.
		/// </summary>
		public int ItemSpacing
		{
			get {return m_ItemSpacing;}
			set
			{
				m_NeedRecalcSize=true;
				m_ItemSpacing=value;
			}
		}
		#endregion
	}
}
