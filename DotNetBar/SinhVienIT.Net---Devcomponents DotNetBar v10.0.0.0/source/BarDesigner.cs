using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;
using System.Drawing;
using System.Collections;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Windows Forms designer for Bar control.
	/// </summary>
	public class BarDesigner:BarBaseControlDesigner
	{
		#region Internal Implementation
		private int m_MouseDownSelectedTabIndex=-1;
		private bool m_IsDocking=false;
		private Form m_OutlineForm=null;
		private DockSiteInfo m_DockInfo;
		private bool m_DragDrop=false;

		public BarDesigner():base()
		{
			m_DockInfo=new DockSiteInfo();
			this.EnableItemDragDrop=true;
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				Bar bar=this.Control as Bar;
				DesignerVerb[] verbs=null;
				if(this.IsDockableWindow)
				{
					verbs = new DesignerVerb[]
						{
							new DesignerVerb("Create Document", new EventHandler(CreateDocument))};
				}
				else
				{
					verbs = new DesignerVerb[]
						{
							new DesignerVerb("Create Button", new EventHandler(CreateButton)),
							new DesignerVerb("Create Text Box", new EventHandler(CreateTextBox)),
							new DesignerVerb("Create Combo Box", new EventHandler(CreateComboBox)),
							new DesignerVerb("Create Label", new EventHandler(CreateLabel)),
							new DesignerVerb("Open Bar Designer", new EventHandler(EditBar))};
				}
				
				return new DesignerVerbCollection(verbs);
			}
		}

		private void EditBar(object sender, EventArgs e) 
		{
			OpenDesigner();
		}

		public override void DoDefaultAction()
		{
			ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
			if(!this.IsDockableWindow && selection!=null && selection.PrimarySelection==this.Control)
				OpenDesigner();
			else
				base.DoDefaultAction();
		}

		private void OpenDesigner()
		{
			Bar bar=this.Control as Bar;
			if(bar!=null)	
			{
				DotNetBarManagerDesigner.OpenDesignerEditor(null,bar,this);
			}
		}

#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			SetDesignTimeDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            Bar bar = this.Component as Bar;
            if (bar == null)
                return;
            bar.Style = eDotNetBarStyle.Office2003;
        }

		protected override BaseItem GetItemContainer()
		{
			Bar bar=this.Control as Bar;
			if(bar!=null)
				return bar.ItemsContainer;
			return null;
		}

		protected override void RecalcLayout()
		{
			Bar bar=this.GetItemContainerControl() as Bar;
			if(bar!=null)
				bar.RecalcLayout();
		}

		protected override void OnSubItemsChanging()
		{
			base.OnSubItemsChanging();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				Bar bar=this.GetItemContainerControl() as Bar;
				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("Items",true));
			}
		}

		protected override void OnSubItemsChanged()
		{
			base.OnSubItemsChanged();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				Bar bar=this.GetItemContainerControl() as Bar;
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("Items",true),null,null);
			}
		}

		
		#endregion

		#region Support For Dockable Windows

		public override SelectionRules SelectionRules
		{
			get
			{
				if(this.IsDockableWindow)
					return (SelectionRules.Locked);
				return base.SelectionRules;
			}
		}

		public override bool CanParent(Control control)
		{
			if(control.Contains(this.Control))
				return false;
			if(this.IsDockableWindow && !(control is PanelDockContainer))
				return false;
			return base.CanParent(control);
		}

		protected override bool IsDockableWindow
		{
			get
			{
				Bar bar=this.Control as Bar;
				if(bar!=null && bar.LayoutType==eLayoutType.DockContainer)
					return true;
				return false;
			}
		}

		private void SelectDockTab(int index)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return;

			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"]);
			bar.SelectedDockTab=index;
			if(cc!=null)
				cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"],null,null);
			if(bar.SelectedDockTab>=0)
			{
#if FRAMEWORK20
                SelectComponent(bar.Items[bar.SelectedDockTab], SelectionTypes.Primary);
#else
                SelectComponent(bar.Items[bar.SelectedDockTab],SelectionTypes.MouseDown);
#endif
                DockContainerItem dock=bar.Items[bar.SelectedDockTab] as DockContainerItem;
				if(dock!=null && dock.Control!=null)
				{
					dock.Control.BringToFront();
					if(cc!=null)
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
				}
			}
		}

		protected override eDotNetBarStyle Style
		{
			get
			{
				Bar bar=this.Control as Bar;
				if(bar!=null)
					return bar.Style;
				return base.Style;;
			}
		}

		protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
		{
			Bar bar=this.Control as Bar;
			if(e.Component is Control)
			{
				BaseItem item=GetControlItem(e.Component as Control);
				if(item!=null && item.Parent!=null && item.Parent.SubItems.Contains(item))
				{
					if(item is DockContainerItem)
						((DockContainerItem)item).Control=null;
                    else if(item is ControlContainerItem)
                        ((ControlContainerItem)item).Control = null;
					item.Parent.SubItems.Remove(item);
					DestroySubItems(item);
					this.RecalcLayout();
					if(bar!=null && bar.Items.Count>0)
						SelectDockTab(0);
				}
			}
			else if(!m_InternalRemoving && bar!=null && e.Component is DockContainerItem && bar.Items.Contains((BaseItem)e.Component))
			{
				// Throw exception to stop removing the last dock container item.
				if(bar.VisibleItemCount==1)
					throw new InvalidOperationException("Cannot delete last DockContainerItem object. Select and Delete Bar object instead");
			}
			base.OtherComponentRemoving(sender,e);
		}

		protected override bool OnMouseDown(ref Message m)
		{
			Bar bar=this.Control as Bar;
			if(!this.IsDockableWindow || bar==null)
				return base.OnMouseDown(ref m);
			Point p=Control.MousePosition;
			int i=GetTabAt(p.X,p.Y);
			if(i>=0 && m.Msg==NativeFunctions.WM_RBUTTONDOWN)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null && selection.PrimarySelection!=bar.Items[i])
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(bar.Items[i]);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif

                    SelectDockTab(i);
					this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);
					return true;
				}
			}


			return base.OnMouseDown(ref m);
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
			Bar bar=this.Control as Bar;
			m_DragDrop=false;

			if(bar!=null && this.IsDockableWindow)
			{
				m_MouseDownSelectedTabIndex=GetTabAt(x,y);
				if(m_MouseDownSelectedTabIndex!=-1)
				{
					if(bar.SelectedDockTab!=m_MouseDownSelectedTabIndex)
						SelectDockTab(m_MouseDownSelectedTabIndex);
					m_DragDrop=true;
				}
				else if(IsInTabSystemBox(x,y))
				{
					MouseDownTabSystemBox(x,y);
				}
				else if(IsCaptionGrabHandle(bar))
				{
					Point clientPos=bar.PointToClient(new Point(x,y));
					if(bar.GrabHandleRect.Contains(clientPos))
						m_DragDrop=true;
				}
			}
			base.OnMouseDragBegin(x,y);
			
			if(m_MouseDownSelectedTabIndex!=-1)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null && selection.PrimarySelection!=bar.Items[m_MouseDownSelectedTabIndex])
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(bar.Items[m_MouseDownSelectedTabIndex]);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.Click);
#endif
                }
			}

			if(m_DragDrop)
			{
				bar.Capture = true;
				NativeFunctions.RECT rect = new NativeFunctions.RECT(0,0,0,0);
				NativeFunctions.GetWindowRect(bar.Parent.Handle, ref rect);
				Rectangle r=Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
				Cursor.Clip=r;
			}
		}

		private bool IsCaptionGrabHandle(Bar bar)
		{
			return (bar.GrabHandleStyle==eGrabHandleStyle.Caption || bar.GrabHandleStyle==eGrabHandleStyle.CaptionTaskPane);
		}

		protected override void OnMouseDragMove(int x, int y)
		{
			if(!this.IsDockableWindow || !m_DragDrop)
			{
				base.OnMouseDragMove(x,y);
				return;
			}

			Point screenPos=new Point(x,y);
			
			Bar bar=this.Control as Bar;
			if(bar==null) return;

			Point tabPos=bar.DockTabControl.PointToClient(screenPos);
			if(bar.DockTabControl!=null && bar.DockTabControl.ClientRectangle.Contains(tabPos))
			{
				if(m_IsDocking)
				{
					EndBarOwnerDocking(bar);
					m_IsDocking=false;
					m_DockInfo=new DockSiteInfo();
				}
				MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,tabPos.X,tabPos.Y,0);
				bar.DockTabControl.InternalOnMouseMove(e);
			}
			else
			{
				m_IsDocking=true;
				IOwnerBarSupport ownerDock=bar.Owner as IOwnerBarSupport;
				m_DockInfo=ownerDock.GetDockInfo(bar,screenPos.X,screenPos.Y);
				if(m_DockInfo.objDockSite==null)
				{
					if(m_OutlineForm!=null)
						m_OutlineForm.Hide();
				}
				else
				{
					Rectangle r=m_DockInfo.objDockSite.GetBarDockRectangle(bar,ref m_DockInfo);
					if(!r.IsEmpty)
					{
						if(m_OutlineForm==null)
							m_OutlineForm=BarFunctions.CreateOutlineForm();
						NativeFunctions.SetWindowPos(m_OutlineForm.Handle.ToInt32(),NativeFunctions.HWND_TOP,r.X,r.Y,r.Width,r.Height,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
					}
					else if(m_OutlineForm!=null)
						m_OutlineForm.Hide();
				}
			}
		}

		protected override void OnMouseDragEnd(bool cancel)
		{
			if(!this.IsDockableWindow)
			{
				base.OnMouseDragEnd(cancel);
				return;
			}

			this.Control.Capture = false;
			Cursor.Clip = Rectangle.Empty;
			
			Bar bar=this.Control as Bar;

			if(cancel || bar==null || !m_DragDrop)
			{
				base.OnMouseDragEnd(cancel);
				return;
			}

			m_DragDrop=false;

			IDesignerHost designerHost=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if(designerHost==null)
			{
				EndBarOwnerDocking(bar);
				m_IsDocking=false;
				m_MouseDownSelectedTabIndex=-1;
				base.OnMouseDragEnd(cancel);
				return;
			}

			if(m_IsDocking)
			{
				EndBarOwnerDocking(bar);
				// Moves and docks the selected DockContainerItem or bar
				Bar referenceBar=m_DockInfo.MouseOverBar;
				if(m_DockInfo.MouseOverDockSide!=eDockSide.None && m_DockInfo.MouseOverDockSide!=eDockSide.Document && (referenceBar!=bar || m_MouseDownSelectedTabIndex!=-1 && bar.VisibleItemCount>1))
				{
					DesignerTransaction trans=designerHost.CreateTransaction("DotNetBar Docking");
					IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
					try
					{
						Bar newBar=null;
						if(m_MouseDownSelectedTabIndex!=-1 && bar.VisibleItemCount>1)
							newBar=BarFunctions.CreateDuplicateDockBar(bar,this);
						else
							newBar=bar;

						cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"]);
						
						if(m_MouseDownSelectedTabIndex!=-1 && bar.VisibleItemCount>1)
						{
							DockContainerItem item=bar.Items[bar.SelectedDockTab] as DockContainerItem;
							cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
							cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
							bar.Items.Remove(item);
							cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
							cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);

							cc.OnComponentChanging(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
							cc.OnComponentChanging(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
							newBar.Items.Add(item);
							cc.OnComponentChanged(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
							cc.OnComponentChanged(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);							
						}
						
						cc.OnComponentChanging(m_DockInfo.objDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["Controls"]);
						cc.OnComponentChanging(newBar,null);
						cc.OnComponentChanging(referenceBar,null);
						cc.OnComponentChanging(m_DockInfo.objDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
						m_DockInfo.objDockSite.GetDocumentUIManager().Dock(referenceBar,newBar,m_DockInfo.MouseOverDockSide);
						cc.OnComponentChanged(m_DockInfo.objDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"],null,null);
						cc.OnComponentChanged(referenceBar,null,null,null);
						cc.OnComponentChanged(newBar,null,null,null);
						cc.OnComponentChanged(m_DockInfo.objDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["Controls"],null,null);

					}
					catch
					{
						trans.Cancel();
						throw;
					}
					finally
					{
						if(!trans.Canceled)
							trans.Commit();
					}
				}
				else if(m_DockInfo.MouseOverDockSide==eDockSide.Document && bar!=referenceBar)
				{
					BarDesigner referenceDesigner=designerHost.GetDesigner(referenceBar) as BarDesigner;
					if(referenceDesigner!=null)
						referenceDesigner.DelayedDockTabs(bar,m_MouseDownSelectedTabIndex);
				}

				m_DockInfo=new DockSiteInfo();
			}
			else if(m_MouseDownSelectedTabIndex!=-1)
			{
				if(m_MouseDownSelectedTabIndex!=bar.SelectedDockTab)
				{
					IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
					if(cc!=null)
					{
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"],null,null);
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
					}
				}
			}
			m_IsDocking=false;
			m_MouseDownSelectedTabIndex=-1;
			base.OnMouseDragEnd(cancel);
		}

		private Timer m_TimerDelayRemove=null;
		private Bar m_DelayDockBar=null;
		private int m_DelayDockTabIndex=-1;
		internal void DelayedDockTabs(Bar bar, int tabIndex)
		{
			if(m_TimerDelayRemove==null)
			{
				m_DelayDockBar=bar;
				m_DelayDockTabIndex=tabIndex;
				m_TimerDelayRemove=new Timer();
				m_TimerDelayRemove.Tick+=new EventHandler(this.TimerTickDelayRemove);
				m_TimerDelayRemove.Interval=200;
				m_TimerDelayRemove.Enabled=true;
				m_TimerDelayRemove.Start();
			}
		}
		private void TimerTickDelayRemove(object sender, EventArgs e)
		{
			m_TimerDelayRemove.Stop();
			m_TimerDelayRemove.Enabled=false;
			m_TimerDelayRemove.Dispose();
			m_TimerDelayRemove=null;

			DockTabs(m_DelayDockBar,m_DelayDockTabIndex,this.Control as Bar);

			m_DelayDockBar=null;
			m_DelayDockTabIndex=-1;
		}

		private void DockTabs(Bar sourceBar, int selectedTabIndex, Bar targetBar)
		{
			// Move Dock-container item to different bar
			IDesignerHost designerHost=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			DesignerTransaction trans=designerHost.CreateTransaction("DotNetBar Docking");
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			try
			{
				DockContainerItem[] items=null;
				if(selectedTabIndex!=-1)
					items=new DockContainerItem[] {sourceBar.Items[sourceBar.SelectedDockTab] as DockContainerItem};
				else
				{
					items=new DockContainerItem[sourceBar.Items.Count];
					sourceBar.Items.CopyTo(items,0);
				}

				cc.OnComponentChanging(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
				cc.OnComponentChanging(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
				sourceBar.Items.RemoveRange(items);
				cc.OnComponentChanged(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
				cc.OnComponentChanged(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);

				cc.OnComponentChanging(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
				cc.OnComponentChanging(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
				targetBar.Items.AddRange(items);
				cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
				cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);

				if(sourceBar.Items.Count==0)
				{
					DockSite sourceDockSite=sourceBar.Parent as DockSite;
					cc.OnComponentChanging(sourceDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
					cc.OnComponentChanging(sourceBar,null);
					sourceDockSite.GetDocumentUIManager().UnDock(sourceBar,false);
					cc.OnComponentChanged(sourceBar,null,null,null);
					cc.OnComponentChanged(sourceDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"],null,null);

					designerHost.DestroyComponent(sourceBar);
				}

				if(targetBar!=null && targetBar.SelectedDockTab>=0)
				{
					DockContainerItem dock=targetBar.Items[targetBar.SelectedDockTab] as DockContainerItem;
					if(dock!=null && dock.Control!=null)
					{
						cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
						dock.Control.BringToFront();
						cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
					}
				}
			}
			catch
			{
				trans.Cancel();
				throw;
			}
			finally
			{
				if(!trans.Canceled)
					trans.Commit();
			}
		}

		private void EndBarOwnerDocking(Bar bar)
		{
			IOwnerBarSupport barSupport=bar.Owner as IOwnerBarSupport;
			if(barSupport!=null)
				barSupport.DockComplete();
			if(m_OutlineForm!=null)
			{
				m_OutlineForm.Hide();
				m_OutlineForm.Dispose();
				m_OutlineForm=null;
			}
		}

		/// <summary>
		/// Returns tab index under specified coordinates.
		/// </summary>
		/// <param name="x">Screen X coordinate</param>
		/// <param name="y">Screen Y coordinate</param>
		/// <returns>Tab index or -1 if tab was not found</returns>
		private int GetTabAt(int x, int y)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return -1;
			
			// Select dockable tab if mouse is clicked over the tab
			if(this.IsDockableWindow && bar.DockTabControl!=null)
			{
				Point posTab=bar.DockTabControl.PointToClient(new Point(x,y));
				if(bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(posTab))
				{
					return -1;
				}
				TabItem tab=bar.DockTabControl.HitTest(posTab.X,posTab.Y);
				if(tab!=null)
					return bar.Items.IndexOf(tab.AttachedItem);
			}

			return -1;
		}

		private bool IsInTabSystemBox(int x, int y)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return false;

			// Select dockable tab if mouse is clicked over the tab
			if(this.IsDockableWindow && bar.DockTabControl!=null)
			{
				Point posTab=bar.DockTabControl.PointToClient(new Point(x,y));
				if(bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(posTab))
					return true;
			}

			return false;
		}

		private void MouseDownTabSystemBox(int x, int y)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return;

			// Select dockable tab if mouse is clicked over the tab
			if(this.IsDockableWindow && bar.DockTabControl!=null)
			{
				Point posTab=bar.DockTabControl.PointToClient(new Point(x,y));
				if(bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(posTab))
				{
					if(bar.DockTabControl._TabSystemBox.ForwardEnabled && bar.DockTabControl._TabSystemBox.ForwardRect.Contains(posTab))
					{
						bar.DockTabControl.ScrollForward();
					}
					else if(bar.DockTabControl._TabSystemBox.BackEnabled && bar.DockTabControl._TabSystemBox.BackRect.Contains(posTab))
					{
						bar.DockTabControl.ScrollBackwards();
					}
				}
			}

		}

		/// <summary>
		/// Removes all subitems from container.
		/// </summary>
		protected override void ThisComponentRemoving(object sender, ComponentEventArgs e)
		{
			if(!m_InternalRemoving)
			{
				m_InternalRemoving=true;
				try
				{
					if(this.IsDockableWindow)
					{
						Bar bar=this.Control as Bar;
						if(bar!=null && bar.DockSide==eDockSide.Document && bar.Parent is DockSite)
						{
							IComponentChangeService cc=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
							if(cc!=null)
								cc.OnComponentChanging(((DockSite)bar.Parent),TypeDescriptor.GetProperties(typeof(DockSite)).Find("DocumentDockContainer",true));
							((DockSite)bar.Parent).GetDocumentUIManager().UnDock(bar,false);
							if(cc!=null)
								cc.OnComponentChanged(((DockSite)bar.Parent),TypeDescriptor.GetProperties(typeof(DockSite)).Find("DocumentDockContainer",true),null,null);
						}
					}
				}
				finally
				{
					m_InternalRemoving=false;
				}
			}

			base.ThisComponentRemoving(sender,e);
		}
		#endregion
	}
}
