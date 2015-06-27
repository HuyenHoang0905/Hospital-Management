using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for DesignTimeProviderContainer.
	/// </summary>
	public class DesignTimeProviderContainer
	{
		/// <summary>
		/// Returns information about insertion position for an item given screen coordinates. Used internally for drag&drop support.
		/// </summary>
		/// <param name="containerItem">Container item</param>
		/// <param name="pScreen">Screen coordinates</param>
		/// <param name="DragItem">Item that is being dragged</param>
		/// <returns>Information about insertion position or null if item cannot be inserted to the container.</returns>
		public static InsertPosition GetInsertPosition(BaseItem containerItem, Point pScreen, BaseItem DragItem)
		{
			InsertPosition objInsertPos=null;
            Control objContainer = null;

            if (containerItem is PopupItem && containerItem.Expanded)
                objContainer = ((PopupItem)containerItem).PopupControl;
            else
                objContainer = containerItem.ContainerControl as Control;
			
            if(objContainer==null)
				return null;

			Point pClient=objContainer.PointToClient(pScreen);
			Rectangle thisRect=containerItem.DisplayRectangle;
            if (containerItem is PopupItem && containerItem.Expanded)
                thisRect = objContainer.DisplayRectangle;
			if(thisRect.Contains(pClient) || containerItem.SubItems.Count==0 && objContainer.ClientRectangle.Contains(pClient) || containerItem is ItemContainer && ((ItemContainer)containerItem).SystemContainer && objContainer.ClientRectangle.Contains(pClient))
			{
				Rectangle r;
				BaseItem objItem;
				// Check first inside any expanded items
				objItem=containerItem.ExpandedItem();
				if(objItem!=null)
				{
					IDesignTimeProvider provider=objItem as IDesignTimeProvider;
					if(provider!=null)
					{
						objInsertPos=provider.GetInsertPosition(pScreen, DragItem);
						if(objInsertPos!=null)
							return objInsertPos;
					}
				}

				for(int i=0;i<containerItem.SubItems.Count;i++)
				{
					objItem=containerItem.SubItems[i];
					r=objItem.DisplayRectangle;
					r.Inflate(2,2);
					if(objItem.Visible && r.Contains(pClient))
					{
						if(objItem.SystemItem && containerItem.SubItems.Count!=1)
						{
							return null;
						}
						if(objItem==DragItem)
							return new InsertPosition();
						
						if(objItem.IsContainer && objItem is IDesignTimeProvider)
						{
							Rectangle inner=r;
							inner.Inflate(-8,-8);
                            if (inner.Contains(pClient))
                            {
                                return ((IDesignTimeProvider)objItem).GetInsertPosition(pScreen, DragItem);
                            }
						}

						objInsertPos=new InsertPosition();
						objInsertPos.TargetProvider=(IDesignTimeProvider)containerItem;
						objInsertPos.Position=i;
						if(objItem.Orientation==eOrientation.Horizontal && !objItem.IsOnMenu)
						{
							if(pClient.X<=objItem.LeftInternal+objItem.WidthInternal/2 || objItem.SystemItem)
								objInsertPos.Before=true;
						}
						else
						{
							if(pClient.Y<=objItem.TopInternal+objItem.HeightInternal/2 || objItem.SystemItem)
								objInsertPos.Before=true;
						}

						// We need to collapse any expanded items that are not on this bar
						IOwner owner=containerItem.GetOwner() as IOwner;
						if(owner!=null)
						{
							BaseItem objExp=owner.GetExpandedItem();
							if(objExp!=null)
							{
								while(objExp.Parent!=null)
									objExp=objExp.Parent;
								BaseItem objParent=objItem;
								while(objParent.Parent!=null)
									objParent=objParent.Parent;
								if(objExp!=objParent)
									owner.SetExpandedItem(null);
							}
						}

						if(objItem is PopupItem && (objItem.SubItems.Count>0 || objItem.IsOnMenuBar))
						{
							if(!objItem.Expanded && objItem.CanCustomize)
								objItem.Expanded=true;
						}
						else
						{
							BaseItem.CollapseSubItems(containerItem);
						}
						break;
					}
				}
				if(objInsertPos==null)
				{
					// Container is empty but it can contain the items
					if(containerItem.SubItems.Count>1 && containerItem.SubItems[containerItem.SubItems.Count-1].SystemItem)
						objInsertPos=new InsertPosition(containerItem.SubItems.Count-2,true,(IDesignTimeProvider)containerItem);
					else
						objInsertPos=new InsertPosition(containerItem.SubItems.Count-1,false,(IDesignTimeProvider)containerItem);
				}
			}
			else
			{
				foreach(BaseItem objItem in containerItem.SubItems)
				{
					if(objItem==DragItem)
						continue;
					IDesignTimeProvider provider=objItem as IDesignTimeProvider;
					if(provider!=null)
					{
						objInsertPos=provider.GetInsertPosition(pScreen, DragItem);
						if(objInsertPos!=null)
							break;
					}
				}				
			}
			return objInsertPos;
		}

		/// <summary>
		/// Draws reversible marker to indicates item drag&drop position
		/// </summary>
		/// <param name="containerItem">Container item</param>
		/// <param name="iPos">Position to draw marker at</param>
		/// <param name="Before">Indicates whether the marker is drawn before the reference position</param>
		public static void DrawReversibleMarker(BaseItem containerItem, int iPos, bool Before)
		{
			Control objCtrl=containerItem.ContainerControl as Control;
			if(objCtrl==null)
				return;

			BaseItem objItem=null;
			if(iPos>=0)
				objItem=containerItem.SubItems[iPos];
			Rectangle r, rl,rr;
			if(objItem!=null)
			{
				if(objItem.DesignInsertMarker!=eDesignInsertPosition.None)
					objItem.DesignInsertMarker=eDesignInsertPosition.None;
				else if(Before)
					objItem.DesignInsertMarker=eDesignInsertPosition.Before;
				else
					objItem.DesignInsertMarker=eDesignInsertPosition.After;
				return;
			}
			else
			{
				Rectangle rTmp=containerItem.DisplayRectangle;
				rTmp.Inflate(-1,-1);
				r=new Rectangle(rTmp.Left+2,rTmp.Top+2,1,rTmp.Height-4);
				rl=new Rectangle(rTmp.Left,rTmp.Top+1,5,1);
				rr=new Rectangle(rTmp.Left,rTmp.Bottom-2,5,1);
			}
            //r.Location=objCtrl.PointToScreen(r.Location);
            //rl.Location=objCtrl.PointToScreen(rl.Location);
            //rr.Location=objCtrl.PointToScreen(rr.Location);
            //ControlPaint.DrawReversibleFrame(r,SystemColors.Control,FrameStyle.Thick);
            //ControlPaint.DrawReversibleFrame(rl,SystemColors.Control,FrameStyle.Thick);
            //ControlPaint.DrawReversibleFrame(rr,SystemColors.Control,FrameStyle.Thick);
		}

		/// <summary>
		/// Inserts drag&drop item at specified position.
		/// </summary>
		/// <param name="containerItem">Container item.</param>
		/// <param name="objItem">Item being inserted</param>
		/// <param name="iPos">Insertion position</param>
		/// <param name="Before">Indicates whether item is inserted before the specified insertion position</param>
		public static void InsertItemAt(BaseItem containerItem, BaseItem objItem, int iPos, bool Before)
		{
			if(containerItem.ExpandedItem()!=null)
			{
				containerItem.ExpandedItem().Expanded=false;
			}
			if(!Before)
			{
				if(iPos+1>=containerItem.SubItems.Count)
				{
					containerItem.SubItems.Add(objItem,GetAppendPosition(containerItem));
				}
				else
				{
					containerItem.SubItems.Add(objItem,iPos+1);
				}
			}
			else
			{
				if(iPos>=containerItem.SubItems.Count)
				{
					containerItem.SubItems.Add(objItem, GetAppendPosition(containerItem));
				}
				else
				{
					containerItem.SubItems.Add(objItem,iPos);
				}
			}
			if(containerItem.ContainerControl is Bar)
				((Bar)containerItem.ContainerControl).RecalcLayout();
			else if(containerItem.ContainerControl is MenuPanel)
				((MenuPanel)containerItem.ContainerControl).RecalcSize();
			else if(containerItem.ContainerControl is BarBaseControl)
				((BarBaseControl)containerItem.ContainerControl).RecalcLayout();
			else if(containerItem.ContainerControl is ItemControl)
				((ItemControl)containerItem.ContainerControl).RecalcLayout();
			else
			{
				containerItem.RecalcSize();
				containerItem.Refresh();
			}
		}

		/// <summary>
		/// Returns insertion index for an item taking in account any system items that are at the end of the collection.
		/// </summary>
		/// <param name="objParent">Parent item</param>
		/// <returns>Returns the index at which an item should be inserted</returns>
		public static int GetAppendPosition(BaseItem objParent)
		{
			int iPos=-1;
			for(int i=objParent.SubItems.Count-1;i>=0;i--)
			{
				if(objParent.SubItems[i].SystemItem)
					iPos=i;
				else
					break;
			}
			return iPos;
		}
	}
}
