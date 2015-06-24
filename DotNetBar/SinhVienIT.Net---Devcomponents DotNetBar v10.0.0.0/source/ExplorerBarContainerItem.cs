using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Provides layout for Explorer-Bar control.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class ExplorerBarContainerItem:ImageItem,IDesignTimeProvider
	{
		#region Internal Implementation
		internal int m_ItemSpacing=15;
		/// <summary>
		/// Creates new instance of ExplorerBarContainerItem class.
		/// </summary>
		public ExplorerBarContainerItem()
		{
			m_IsContainer=true;
			m_SystemItem=true;
			m_SupportedOrientation=eSupportedOrientation.Horizontal;
			m_AllowOnlyOneSubItemExpanded=false;
		}
		/// <summary>
		/// Returns copy of ExplorerBarContainerItem item
		/// </summary>
		public override BaseItem Copy()
		{
			ExplorerBarContainerItem objCopy=new ExplorerBarContainerItem();
			this.CopyToItem(objCopy);
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			ExplorerBarContainerItem objCopy=copy as ExplorerBarContainerItem;
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
						if(item is ExplorerBarGroupItem)
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
			// Explorer Container can have none or one or more panels expanded at a time
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
                    if (item is ExplorerBarGroupItem)
                    {
                        if (((ExplorerBarGroupItem)item).WordWrapSubItems)
                        {
                            pa.ButtonStringFormat = pa.ButtonStringFormat & ~(pa.ButtonStringFormat & eTextFormat.SingleLine);
                            pa.ButtonStringFormat |= eTextFormat.WordBreak;
                        }
                        else
                        {
                            pa.ButtonStringFormat |= eTextFormat.SingleLine;
                            pa.ButtonStringFormat = pa.ButtonStringFormat & ~(pa.ButtonStringFormat & eTextFormat.WordBreak);
                        }
                    }
                    else
                    {
                        pa.ButtonStringFormat = pa.ButtonStringFormat & ~(pa.ButtonStringFormat & eTextFormat.SingleLine);
                        pa.ButtonStringFormat |= eTextFormat.WordBreak;
                    }
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
			if(item is ExplorerBarGroupItem)
				((ExplorerBarGroupItem)item).VisualPropertyChanged();
			if(this.DesignMode)
			{
				//ExplorerBar bar=this.ContainerControl as ExplorerBar; 
				//this.Refresh();
				ExplorerBar bar=this.ContainerControl as ExplorerBar; 
				if(bar!=null)
					bar.RecalcLayout();
			}
		}

		protected internal override void OnAfterItemRemoved(BaseItem item)
		{
			base.OnAfterItemRemoved(item);
			NeedRecalcSize=true;
			if(this.DesignMode)
			{
				ExplorerBar bar=this.ContainerControl as ExplorerBar; 
				if(bar!=null)
					bar.RecalcLayout();
				//this.Refresh();
			}
		}

		protected internal override void OnSubItemsClear()
		{
			base.OnSubItemsClear();
			NeedRecalcSize=true;
			if(this.DesignMode)
			{
				//this.Refresh();
				ExplorerBar bar=this.ContainerControl as ExplorerBar; 
				if(bar!=null)
					bar.RecalcLayout();
			}
		}
		internal bool _Animating=false;
		protected internal override void OnSubItemExpandChange(BaseItem objChildItem)
		{
			base.OnSubItemExpandChange(objChildItem);
			ExplorerBar exbar=this.ContainerControl as ExplorerBar;
			try
			{
				if(exbar!=null && exbar.AnimationEnabled)
				{
					TimeSpan totalAnimationTime = new TimeSpan(0, 0, 0, 0, exbar.AnimationTime);
					_Animating=true;
					int iStep=1;
					DateTime startTime=DateTime.Now;
					if(objChildItem.Expanded)
					{
						int initalHeight=objChildItem.HeightInternal;
						objChildItem.RecalcSize();
						int targetHeight=objChildItem.HeightInternal;
						for(int i=initalHeight;i<targetHeight;i+=iStep)
						{
							DateTime startPerMove = DateTime.Now;
							objChildItem.HeightInternal=i;
							foreach(BaseItem item in objChildItem.SubItems)
							{
								if(!objChildItem.DisplayRectangle.Contains(item.DisplayRectangle))
									item.Displayed=false;
								else
									item.Displayed=true;
							}
							for(int pos=this.SubItems.IndexOf(objChildItem)+1;pos<this.SubItems.Count;pos++)
								this.SubItems[pos].TopInternal+=iStep;
							
//							float perc=(float)i/targetHeight+1;
//							iStep=(int)Math.Exp(perc);
							exbar.Refresh();

							TimeSpan elapsedPerMove = DateTime.Now - startPerMove;
							TimeSpan elapsedTime = DateTime.Now - startTime;
                            int totalMs = (int)(totalAnimationTime - elapsedTime).TotalMilliseconds;
							if (totalMs <= 0)
							{
								iStep=targetHeight-i;
							}
                            else if (totalMs == 0)
								iStep=1;
							else
							{
								iStep=(targetHeight-i)*(int)elapsedPerMove.TotalMilliseconds / totalMs;
								if(iStep<=0) iStep=1;

							}
							if(iStep<=0)
								iStep=targetHeight-i;
						}
					}
					else
					{
						int initalHeight=objChildItem.HeightInternal;
						objChildItem.RecalcSize();
						int targetHeight=objChildItem.HeightInternal;
						for(int i=initalHeight;i>targetHeight;i-=iStep)
						{
							DateTime startPerMove = DateTime.Now;
							objChildItem.HeightInternal=i;
							foreach(BaseItem item in objChildItem.SubItems)
							{
								if(!objChildItem.DisplayRectangle.Contains(item.DisplayRectangle))
									item.Displayed=false;
								else
									item.Displayed=true;
							}
							for(int pos=this.SubItems.IndexOf(objChildItem)+1;pos<this.SubItems.Count;pos++)
								this.SubItems[pos].TopInternal-=iStep;
//							float perc=(float)targetHeight/i+1;
//							iStep=(int)Math.Exp(perc);
							exbar.Refresh();

							TimeSpan elapsedPerMove = DateTime.Now - startPerMove;
							TimeSpan elapsedTime = DateTime.Now - startTime;
							if ((totalAnimationTime - elapsedTime).TotalMilliseconds <= 0)
							{
								iStep=i-targetHeight;
							}
							else if((totalAnimationTime - elapsedTime).TotalMilliseconds==0)
								iStep=1;
							else
							{
								iStep=(i-targetHeight)*(int)elapsedPerMove.TotalMilliseconds / Math.Max(1,(int)((totalAnimationTime - elapsedTime).TotalMilliseconds));
								if(iStep<=0) iStep=1;
							}
							if(iStep<=0)
								iStep=i-targetHeight;
						}
					}
				}
			}
			finally
			{
				_Animating=false;
			}
			if(exbar!=null)
				exbar.RecalcLayout();
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
