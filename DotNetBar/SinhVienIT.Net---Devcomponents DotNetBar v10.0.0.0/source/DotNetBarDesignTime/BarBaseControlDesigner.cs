using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for BarBaseControlDesigner.
	/// </summary>
    public class BarBaseControlDesigner : System.Windows.Forms.Design.ParentControlDesigner, IDesignerServices, IBarItemDesigner
	{
		#region Private Variables
		const string TEMP_NAME="tempDragDropItem";
		private bool m_EnableItemDragDrop=false;
		private bool m_AcceptExternalControls=true;
		private Point m_MouseDownPoint=Point.Empty;
		private BaseItem m_DragItem=null;
		private bool m_DragInProgress=false;
		private IDesignTimeProvider m_DesignTimeProvider=null;
		private int m_InsertPosition;
		private bool m_InsertBefore;
		private bool m_Capture=false;
		private System.Windows.Forms.Control m_DesignerHost=null;
		private Timer m_TimerAdded=null;
		private Timer m_TimerDragDrop=null;
		private bool m_NewControlAdded=false;
		private bool m_SuspendInternalCursor=false;
		private bool m_DragLeave=false;
		private bool m_ControlRemoved=false;
		private DateTime m_JustAdded=DateTime.MinValue;
		private bool m_MouseDownSelectionPerformed=false;
        private bool m_PassiveContainer = false;
        protected bool m_AddingItem = false;
        protected bool m_CreatingItem = false;
        protected DesignerTransaction m_InsertItemTransaction = null;
        private bool m_InternalRemoveInProgress = false;
		#endregion

		#region Designer Implementation
		public BarBaseControlDesigner()
		{
			
		}

		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;

			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
            if (ss != null)
            {
                ss.SelectionChanged += new EventHandler(OnSelectionChanged);
            }

			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
			{
				cc.ComponentRemoving+=new ComponentEventHandler(this.OnComponentRemoving);
				cc.ComponentRemoved+=new ComponentEventHandler(this.OnComponentRemoved);
                cc.ComponentAdding += new ComponentEventHandler(ComponentChangeComponentAdding);
                cc.ComponentAdded += new ComponentEventHandler(ComponentChangeComponentAdded);
			}

			IDesignerEventService ds=GetService(typeof(IDesignerEventService)) as IDesignerEventService;
			if(ds!=null)
			{
				ds.ActiveDesignerChanged+=new ActiveDesignerEventHandler(this.OnActiveDesignerChanged);
				ds.SelectionChanged+=new EventHandler(DesignerSelectionChanged);
			}

			if(this.Control is IBarDesignerServices)
				((IBarDesignerServices)this.Control).Designer=this;

            if (this.Control is ItemControl)
                ((ItemControl)this.Control).SetDesignMode(true);

			if(component is System.Windows.Forms.Control)
			{
				((Control)component).ControlAdded+=new ControlEventHandler(this.ComponentAdded);
				((Control)component).ControlRemoved+=new ControlEventHandler(this.ComponentRemoved);
			}
			//this.EnableDragDrop(false);
		}

        protected virtual void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
        {
            if (m_AddingItem)
            {
                m_AddingItem = false;
                IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (cc != null)
                    cc.OnComponentChanging(this.Control, null);
                this.GetItemContainer().SubItems.Add(e.Component as BaseItem);
                if (cc != null)
                    cc.OnComponentChanged(this.Control, null, null, null);
                m_InsertItemTransaction.Commit();
                m_InsertItemTransaction = null;
                this.RecalcLayout();
            }
        }

        private void ComponentChangeComponentAdding(object sender, ComponentEventArgs e)
        {
            if (m_InsertItemTransaction == null && !m_AddingItem && !m_CreatingItem && e.Component is BaseItem)
            {
                ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;
                if (ss != null && ss.PrimarySelection == this.Control)
                {
                    m_AddingItem = true;
                    IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                    m_InsertItemTransaction = dh.CreateTransaction("Adding Item Clip");
                }
            }
        }

		protected override void Dispose(bool disposing)
		{
			// Unhook events
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
			{
				cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);
				cc.ComponentRemoved-=new ComponentEventHandler(this.OnComponentRemoved);
			}

			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
            if (ss != null)
            {
                ss.SelectionChanged -= new EventHandler(OnSelectionChanged);
            }
			
			IDesignerEventService ds=GetService(typeof(IDesignerEventService)) as IDesignerEventService;
			if(ds!=null)
			{
				ds.ActiveDesignerChanged-=new ActiveDesignerEventHandler(this.OnActiveDesignerChanged);
				ds.SelectionChanged-=new EventHandler(DesignerSelectionChanged);
			}

			if(this.Component is System.Windows.Forms.Control)
			{
				((Control)this.Component).ControlAdded-=new ControlEventHandler(this.ComponentAdded);
				((Control)this.Component).ControlRemoved-=new ControlEventHandler(this.ComponentRemoved);
			}

			base.Dispose(disposing);
		}

		public override bool CanParent(Control control)
		{
			BaseItem item = GetControlItem(control);
			if (item != null && item != m_DragItem && !m_NewControlAdded)
				return false;
			return base.CanParent(control);
		}

		private void ComponentAdded(object sender, ControlEventArgs e)
		{
			if(!m_NewControlAdded || !m_EnableItemDragDrop || !m_AcceptExternalControls || this.IsDockableWindow)
			{
				if(!m_NewControlAdded)
				{
					if(!OnControlAdded(e))
						return;
				}
				else
					return;
			}

			m_TimerAdded=new Timer();
			m_TimerAdded.Tick+=new EventHandler(this.TimerTick);
			m_TimerAdded.Interval=50;
			m_TimerAdded.Enabled=true;
			m_TimerAdded.Start();
			m_NewControlAdded=false;
		}

		/// <summary>
		/// Called after control has been added to container but not through drag & drop. Control added could also be
		/// internal control by the bar container.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <returns>true if acted upon this new control otherwise false.</returns>
		protected virtual bool OnControlAdded(ControlEventArgs e)
		{
			return false;
		}

		private void ComponentRemoved(object sender, ControlEventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null || dh.Loading || m_InternalRemoveInProgress)
				return;

			if(m_JustAdded!=DateTime.MinValue && DateTime.Now.Subtract(m_JustAdded).Seconds<2)
			{
				m_JustAdded=DateTime.MinValue;
				return;
			}
			m_JustAdded=DateTime.MinValue;
			if(m_DragLeave)
				ControlRemoved(e.Control);
			else if(m_TimerDragDrop!=null)
				m_ControlRemoved=true;
			else
			{
				ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
				if(ss!=null && ss.PrimarySelection==e.Control && this.GetControlItem(e.Control)!=null)
				{
					ControlRemoved(e.Control);
				}
			}
		}

		private void ControlRemoved(Control control)
		{
            if (control != null && !m_InternalRemoveInProgress)
			{
				BaseItem item=this.GetControlItem(control);
				if(item!=null)
				{
					MouseDragDrop(-1,-1);
					if(item.Parent!=null)
						item.Parent.SubItems.Remove(item);
                    if (item is ControlContainerItem)
                        ((ControlContainerItem)item).Control = null;
					this.DestroyComponent(item);
					this.RecalcLayout();
				}
			}
		}

		private void TimerTick(object sender, EventArgs e)
		{
			m_TimerAdded.Stop();
			m_TimerAdded.Enabled=false;
			m_TimerAdded=null;
			this.RecalcLayout();
			ISelectionService sel=(ISelectionService)this.GetService(typeof(ISelectionService));
			if(sel!=null && sel.PrimarySelection is Control && this.Control.Controls.Contains((Control)sel.PrimarySelection))
			{
				IComponentChangeService cc=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				cc.OnComponentChanged(sel.PrimarySelection,null,null,null);
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			DesignTimeSelectionChanged(sender as ISelectionService);
		}

		private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
		{
			this.ActiveDesignerChanged(e);
		}

		/// <summary>
		/// Support for popup menu closing.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void ActiveDesignerChanged(ActiveDesignerEventArgs e)
		{
            ClosePopups();
		}

        protected virtual void ClosePopups()
        {
            IOwner owner = GetIOwner();
            if (owner != null)
            {
                owner.OnApplicationDeactivate(); // Closes all popups
            }
        }

        protected virtual IOwner GetIOwner()
        {
            return this.GetItemContainerControl() as IOwner;
        }

        protected virtual IOwnerMenuSupport GetIOwnerMenuSupport()
        {
            return this.GetItemContainerControl() as IOwnerMenuSupport;
        }
 
		/// <summary>
		/// Support for popup menu closing.
		/// </summary>
		protected virtual void DesignTimeSelectionChanged(ISelectionService ss)
		{
			if(ss==null)
				return;
			if(this.Control==null || this.Control.IsDisposed || this.IsDockableWindow)
				return;

			if(ss.PrimarySelection!=this.Control)
			{
				BaseItem container=this.GetItemContainer();
				if(container==null)
					return;
				if(ss.PrimarySelection is BaseItem)
				{
					BaseItem item=ss.PrimarySelection as BaseItem;
                    if (item.ContainerControl == this.GetItemContainerControl())
                    {
                        if ((!item.IsOnMenu || item.IsOnBar && ((Bar)item.ContainerControl).BarState == eBarState.Popup) && !(item.Expanded && item is PopupItem || item is GalleryContainer && ((GalleryContainer)item).PopupGalleryItem.Expanded))
                            this.GetIOwnerMenuSupport().ClosePopups();
                        return;
                    }

                    if (this.GetAllAssociatedComponents().Contains(item))
                    {
                        if(!item.IsOnMenu && !(item.IsOnBar && ((Bar)item.ContainerControl).BarState == eBarState.Popup))
                            this.GetIOwnerMenuSupport().ClosePopups();
                        return;
                    }

                    IOwner owner = this.GetIOwner();
                    if (owner != null && owner!=item.GetOwner())
                    {
                        owner.SetFocusItem(null);
                        owner.OnApplicationDeactivate();  // Closes all popups
                    }
				}
				else
				{
                    IOwner owner = this.GetIOwner();
					if(owner!=null)
					{
						owner.SetFocusItem(null);
						owner.OnApplicationDeactivate(); // Closes all popups
					}
				}
			}
			else
			{
                IOwner owner = this.GetIOwner();
				if(owner!=null)
					owner.OnApplicationDeactivate(); // Closes all popups
			}
		}

		protected virtual BaseItem GetItemContainer()
		{
			BarBaseControl bar=this.Control as BarBaseControl;
			if(bar!=null)
				return bar.GetBaseItemContainer();
			if(this.Control is ItemControl)
				return ((ItemControl)this.Control).GetBaseItemContainer();
			return null;
		}

		protected virtual System.Windows.Forms.Control GetItemContainerControl()
		{
			return this.Control;
		}

		protected bool m_InternalRemoving=false;
		private void OnComponentRemoving(object sender,ComponentEventArgs e)
		{
			if(e.Component==this.Component)
				ThisComponentRemoving(sender,e);
			else
				OtherComponentRemoving(sender,e);
		}

		private void OnComponentRemoved(object sender,ComponentEventArgs e)
		{
			ComponentRemoved(sender,e);
		}

		protected virtual void ComponentRemoved(object sender, ComponentEventArgs e){}

		/// <summary>
		/// Removes all subitems from container.
		/// </summary>
		protected virtual void ThisComponentRemoving(object sender, ComponentEventArgs e)
		{
			if(!m_InternalRemoving)
			{
				m_InternalRemoving=true;
				try
				{
					// Unhook events
					IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
					if(cc!=null)
						cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);

					BaseItem container=this.GetItemContainer();
					IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));

					if(dh==null || container==null)
						return;

                    //foreach(BaseItem item in container.SubItems)
                    //{

                    for (int i = container.SubItems.Count - 1; i >= 0; i--)
                    {
                        BaseItem item = container.SubItems[i];

                        // Covers the undo case in designer
                        if (item.Parent == container)
                        {
                            DestroySubItems(item, dh);
                            dh.DestroyComponent(item);
                        }
                    }
				}
				finally
				{
					m_InternalRemoving=false;
				}
			}
		}

		/// <summary>
		/// Triggered when some other component on the form is removed.
		/// </summary>
		protected virtual void OtherComponentRemoving(object sender, ComponentEventArgs e)
		{
			if(e.Component is BaseItem)
			{
				BaseItem item=e.Component as BaseItem;
				if(item.ContainerControl==GetItemContainerControl())
				{
					if(item.Parent!=null && item.Parent.SubItems.Contains(item))
						item.Parent.SubItems.Remove(item);
					if(item!=null)
						DestroySubItems(item);
					this.RecalcLayout();
				}
			}
		}

		protected virtual void DestroySubItems(BaseItem parent, IDesignerHost dh)
		{
			if(parent is ControlContainerItem)
			{
				if(((ControlContainerItem)parent).Control!=null)
				{
					Control c=((ControlContainerItem)parent).Control;
					((ControlContainerItem)parent).Control=null;
					dh.DestroyComponent(c);
				}
			}
			else if(parent is DockContainerItem)
			{
				if(((DockContainerItem)parent).Control!=null)
				{
					Control c=((DockContainerItem)parent).Control;
					((DockContainerItem)parent).Control=null;
					dh.DestroyComponent(c);
				}
			}

			BaseItem[] subitems=new BaseItem[parent.SubItems.Count];
			parent.SubItems.CopyTo(subitems,0);
			foreach(BaseItem item in subitems)
			{
				DestroySubItems(item,dh);
				dh.DestroyComponent(item);
			}
		}

		protected virtual void DestroySubItems(BaseItem parent)
		{
			IDesignerHost dh=GetService(typeof(IDesignerHost)) as IDesignerHost;
			if(dh!=null)
				DestroySubItems(parent,dh);
		}

		/// <summary>
		/// Selection support for items on container.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
            if(m_DesignerHost==null)
				m_DesignerHost=System.Windows.Forms.Control.FromHandle(m.HWnd);
			BaseItem container=this.GetItemContainer();
			System.Windows.Forms.Control ctrl=this.GetItemContainerControl();
			if(container==null || ctrl==null || ctrl.IsDisposed)
			{
				base.WndProc(ref m);
				return;
			}

			switch(m.Msg)
			{
				case WinApi.WM_LBUTTONDOWN:
                case WinApi.WM_RBUTTONDOWN:
				{
					if(OnMouseDown(ref m, m.Msg==WinApi.WM_LBUTTONDOWN?MouseButtons.Left:MouseButtons.Right))
						return;
					break;
				}
				case WinApi.WM_RBUTTONUP:
				case WinApi.WM_LBUTTONUP:
				{
					if(OnMouseUp(ref m))
						return;

					break;
				}
				case WinApi.WM_MOUSEMOVE:
				{
					if(OnMouseMove(ref m))
					{
						m.Result=IntPtr.Zero;
						return;
					}
					break;
				}
				case WinApi.WM_MOUSELEAVE:
				{
					if(OnMouseLeave(ref m))
						return;
					break;
				}
				case WinApi.WM_LBUTTONDBLCLK:
				{
					if(OnMouseDoubleClick(m))
						return;
					break;
				}
                case WinApi.WM_HSCROLL:
                case WinApi.WM_VSCROLL:
                {
                    base.WndProc(ref m);
                    this.RecalcLayout();
                    return;
                }
			}

			base.WndProc(ref m);
		}

		protected virtual bool OnMouseDown(ref Message m, MouseButtons button)
		{
			if(this.IsDockableWindow)
				return false;

			System.Windows.Forms.Control ctrl=this.GetItemContainerControl();
			BaseItem container=this.GetItemContainer();
            IOwner owner = this.GetIOwner();

			if(ctrl==null || owner==null || container==null)
				return false;

			if(m.Msg==WinApi.WM_RBUTTONDOWN)
			{
				Point pos=ctrl.PointToClient(System.Windows.Forms.Control.MousePosition);
				MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);
				container.InternalMouseDown(e);
                BaseItem focusItem = owner.GetFocusItem();
				if(focusItem!=null)
				{
					ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
					if(selection!=null)
					{
						ArrayList arr=new ArrayList(1);
                        arr.Add(focusItem);
#if FRAMEWORK20
						selection.SetSelectedComponents(arr,SelectionTypes.Primary);
#else
                        selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif
                        OnItemSelected(focusItem);
                        if (arr[0] is PopupItem && ((PopupItem)arr[0]).SubItems.Count > 0 && !((PopupItem)arr[0]).Expanded)
                            ((PopupItem)arr[0]).Expanded = true;
                        focusItem.DesignTimeMouseDownPoint = pos;
						this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);
						return true;
					}
				}
			}
			return false;
		}

		protected virtual bool OnMouseUp(ref Message m)
		{
			bool bProcessed=false;

			if(m_Capture)
			{
				m_Capture=false;
				System.Windows.Forms.Control c=System.Windows.Forms.Control.FromHandle(m.HWnd);
				if(c!=null)
					c.Capture=false;
			}

			System.Windows.Forms.Control ctrl=this.GetItemContainerControl();
			BaseItem container=this.GetItemContainer();
            IOwner owner = this.GetIOwner();

			if(ctrl==null || owner==null || container==null)
				return false;

			Point pos=ctrl.PointToClient(System.Windows.Forms.Control.MousePosition);
			MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);
			container.InternalMouseUp(e);
					
            if (m_MouseDownSelectionPerformed)
                bProcessed = true;
			m_MouseDownSelectionPerformed=false;

			if(m_DragItem!=null && m_DragItem is ControlContainerItem && ((ControlContainerItem)m_DragItem).Control!=null)
				MouseDragDrop(pos.X,pos.Y);

			return bProcessed;
		}

		protected virtual bool OnMouseMove(ref Message m)
		{
			return false;
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
            if (m_PassiveContainer)
            {
                base.OnMouseDragBegin(x, y);
                return;
            }

			Control bar=this.GetItemContainerControl();
			System.Windows.Forms.Control ctrl=this.GetItemContainerControl();
            IOwner owner = this.GetIOwner();
			BaseItem container=this.GetItemContainer();
			Point pos=ctrl.PointToClient(new Point(x,y));
			MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);
			
			BaseItem dragItem=null;
            BaseItem previousFocusItem = owner.GetFocusItem();
			container.InternalMouseDown(e);
			dragItem=owner.GetFocusItem();

//            // Allow control selection
//            if (dragItem != null && dragItem == previousFocusItem && (dragItem.SubItems.Count == 0 || !(dragItem is PopupItem)))
//            {
//                ArrayList arr = new ArrayList(1);
//                arr.Add(this.Control);
//                ISelectionService selection = (ISelectionService)this.GetService(typeof(ISelectionService));
//#if FRAMEWORK20
//                selection.SetSelectedComponents(arr, SelectionTypes.Primary);
//#else
//                    selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
//#endif
//                owner.SetFocusItem(null);
//                dragItem = null;
//            }

			if(dragItem!=null)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null)
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(dragItem);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif
                    OnItemSelected(owner.GetFocusItem());
				}
			}

			if(bar==null || dragItem==null || this.IsDockableWindow || !m_EnableItemDragDrop || !CanDragItem(dragItem))
			{
				if(dragItem==null)
					base.OnMouseDragBegin(x,y);
				else
					this.Control.Capture = true; // Does same as base implementation
				return;
			}

			bar.Capture = true;
            if (this.IsDockableWindow)
            {
                WinApi.RECT rect = new WinApi.RECT(0, 0, 0, 0);
                WinApi.GetWindowRect(bar.Handle, ref rect);
                Rectangle r = Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
                Cursor.Clip = r;
            }
			StartItemDrag(dragItem);

			// Does same as base implementation
			this.Control.Capture = true;
		}
		protected virtual bool CanDragItem(BaseItem item)
		{
            if (item is ControlContainerItem && ((ControlContainerItem)item).Control != null)
                return false;
			return true;
		}
        protected virtual Point GetClientPoint(int x, int y)
        {
            Point p = this.Control.PointToClient(new Point(x, y));
            return p;
        }
        protected override void OnMouseDragMove(int x, int y)
		{
			if(m_DragInProgress)
			{
                Point p = GetClientPoint(x, y);
				MouseDragOver(p.X,p.Y);
			}
		}
		protected override void OnMouseDragEnd(bool cancel)
		{
			if(!this.IsDockableWindow && !m_PassiveContainer)
			{
				this.Control.Capture = false;
				Cursor.Clip = Rectangle.Empty;

				if(m_DragInProgress)
				{
					if(cancel)
						MouseDragDrop(-1,-1);
					else
					{
						Point p=this.Control.PointToClient(Control.MousePosition);
						MouseDragDrop(p.X,p.Y);
					}
					cancel=true;
				}
				else
				{
					System.Windows.Forms.Control ctrl=this.GetItemContainerControl();
                    IOwner owner = this.GetIOwner();
					if(owner!=null && owner.GetFocusItem()!=null)
						cancel=true;
				}
			}

			base.OnMouseDragEnd(cancel);
		}

		protected virtual bool OnMouseLeave(ref Message m)
		{
			return false;
		}

		protected virtual bool OnMouseDoubleClick(Message m)
		{
			bool processed=false;

			ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
            if (selection != null && selection.PrimarySelection == this.Control)
            {
                Point p = this.Control.PointToClient(Control.MousePosition);
                IOwner owner = this.GetIOwner();
                BaseItem container = this.GetItemContainer();
                container.InternalMouseDown(new MouseEventArgs(MouseButtons.Left, 0, p.X, p.Y, 0));
                BaseItem focusItem = owner.GetFocusItem();

                // Allow control selection
                if (focusItem!=null)
                {
                    ArrayList arr = new ArrayList(1);
                    arr.Add(focusItem);
                    selection = (ISelectionService)this.GetService(typeof(ISelectionService));
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif
                }
            }

			if(selection!=null && selection.PrimarySelection is ButtonItem && ((ButtonItem)selection.PrimarySelection).ContainerControl==this.GetItemContainerControl())
			{
				IDesignerHost host=(IDesignerHost) this.GetService(typeof(IDesignerHost));
				if(host!=null)
				{
					IDesigner designer=host.GetDesigner(selection.PrimarySelection as IComponent);
					if(designer!=null)
					{
						designer.DoDefaultAction();
						processed=true;
					}
				}
			}

			return processed;
		}

		protected virtual void OnItemSelected(BaseItem item)
		{

		}

		protected virtual void RecalcLayout()
		{
			Control c=this.GetItemContainerControl();
            if (c is BarBaseControl)
                ((BarBaseControl)c).RecalcLayout();
            else if (c is ItemControl)
                ((ItemControl)c).RecalcLayout();
            else if (c is ButtonX)
                ((ButtonX)c).RecalcLayout();
		}

		/// <summary>
		/// Indicates to the designed control that it has been selected or one of the elements managed by the control is selected in designer.
		/// </summary>
		/// <param name="selected">true if selected otherwise false</param>
		protected virtual void SetComponentSelected(bool selected)
		{
			if(this.GetItemContainerControl() is ItemControl)
			{
				((ItemControl)this.GetItemContainerControl()).DesignerSelection=selected;
			}
            else if (this.GetItemContainerControl() is Bar)
            {
                ((Bar)this.GetItemContainerControl()).DesignerSelection = selected;
            }
            else if (this.GetItemContainerControl() is ExplorerBar)
            {
                ((ExplorerBar)this.GetItemContainerControl()).DesignerSelection = selected;
            }
		}

		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				ArrayList c=new ArrayList(base.AssociatedComponents);
				BaseItem container=this.GetItemContainer();
				if(container!=null)
				{
					foreach(BaseItem item in container.SubItems)
					{
						if(item.DesignMode)
							c.Add(item);
					}
				}
				return c;
			}
		}

        protected System.Collections.ICollection BaseAssociatedComponents
        {
            get { return base.AssociatedComponents; }
        }

		protected virtual ArrayList GetAllAssociatedComponents()
		{
			ArrayList c=new ArrayList(base.AssociatedComponents);
			BaseItem container=this.GetItemContainer();
			if(container!=null)
			{
				AddSubItems(container,c);
			}
			return c;
		}

        protected override bool GetHitTest(Point pt)
        {
            if (base.GetHitTest(pt))
            {
                return true;
            }
            ScrollableControl c = this.Control as ScrollableControl;
            if (c!=null && c.IsHandleCreated && c.AutoScroll)
            {
                int res = WinApi.SendMessage(c.Handle, 0x84, 0, WinApi.MAKELPARAM(pt.X, pt.Y));
                if ((res == 7) || (res == 6))
                {
                    return true;
                }
            }
            return false;
        }

		protected virtual void AddSubItems(BaseItem parent, ArrayList list)
		{
			foreach(BaseItem item in parent.SubItems)
			{
				if(item.DesignMode)
					list.Add(item);
				AddSubItems(item,list);
			}
		}
		#endregion

		#region Design-Time Item Creation
		protected virtual void CreateButton(object sender, EventArgs e)
		{
            CreateButton();
		}
        public virtual ButtonItem CreateButton()
        {
            OnSubItemsChanging();
            ButtonItem button = CreateButton(this.GetItemContainer());
            OnSubItemsChanged();
            return button;
        }
		protected virtual ButtonItem CreateButton(BaseItem parent)
		{
			if(parent==null)
				return null;
            ButtonItem item = null;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh!=null)
			{
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    item = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
                    if (item == null)
                        return null;

                    TypeDescriptor.GetProperties(item)["Text"].SetValue(item, item.Name);

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                }
                finally
                {
                    if (!trans.Canceled)
                        trans.Commit();
                    m_CreatingItem = false;
                }
			}
            return item;
		}

        protected virtual void CreateRatingItem(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateRatingItem(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateRatingItem(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    RatingItem item = dh.CreateComponent(typeof(RatingItem)) as RatingItem;
                    if (item == null)
                        return;

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                }
                finally
                {
                    if (!trans.Canceled)
                        trans.Commit();
                    m_CreatingItem = false;
                }
            }
        }

        protected virtual void CreateCircularProgressItem(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateCircularProgressItem(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateCircularProgressItem(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    CircularProgressItem item = dh.CreateComponent(typeof(CircularProgressItem)) as CircularProgressItem;
                    if (item == null)
                        return;

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                }
                finally
                {
                    if (!trans.Canceled)
                        trans.Commit();
                    m_CreatingItem = false;
                }
            }
        }

        protected virtual void CreateCheckBox(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateCheckBox(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateCheckBox(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    CheckBoxItem item = dh.CreateComponent(typeof(CheckBoxItem)) as CheckBoxItem;
                    if (item == null)
                        return;

                    TypeDescriptor.GetProperties(item)["Text"].SetValue(item, item.Name);

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                }
                finally
                {
                    if (!trans.Canceled)
                        trans.Commit();
                    m_CreatingItem = false;
                }
            }
        }

		protected virtual void CreateTextBox(object sender, EventArgs e)
		{
			OnSubItemsChanging();
			CreateTextBox(this.GetItemContainer());
			OnSubItemsChanged();
		}

		protected virtual void CreateTextBox(BaseItem parent)
		{
			if(parent==null)
				return;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh!=null)
			{
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    TextBoxItem item = dh.CreateComponent(typeof(TextBoxItem)) as TextBoxItem;
                    if (item == null)
                        return;

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                }
                finally
                {
                    if (!trans.Canceled)
                        trans.Commit();
                    m_CreatingItem = false;
                }
			}
		}


		protected virtual void CreateComboBox(object sender, EventArgs e)
		{
			OnSubItemsChanging();
			CreateComboBox(this.GetItemContainer());
			OnSubItemsChanged();
		}

		protected virtual void CreateComboBox(BaseItem parent)
		{
			if(parent==null)
				return;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh!=null)
			{
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
				    ComboBoxItem item=dh.CreateComponent(typeof(ComboBoxItem)) as ComboBoxItem;
				    if(item==null)
					    return;

				    IComponentChangeService cc=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				    if(parent!=this.GetItemContainer() && cc!=null)
					    cc.OnComponentChanging(parent,TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

				    parent.SubItems.Add(item);
				    this.RecalcLayout();

				    if(parent!=this.GetItemContainer() && cc!=null)
					    cc.OnComponentChanged(parent,TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"],null,null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
			}
		}

		protected virtual void CreateLabel(object sender, EventArgs e)
		{
			OnSubItemsChanging();
			CreateLabel(this.GetItemContainer());
			OnSubItemsChanged();
		}

		protected virtual void CreateLabel(BaseItem parent)
		{
			if(parent==null)
				return;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh!=null)
			{
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    LabelItem item = dh.CreateComponent(typeof(LabelItem)) as LabelItem;
                    if (item == null)
                        return;

                    TypeDescriptor.GetProperties(item)["Text"].SetValue(item, item.Name);

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
			}
		}

        protected virtual void CreateColorPicker(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateColorPicker(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateColorPicker(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    ColorPickerDropDown item = dh.CreateComponent(typeof(ColorPickerDropDown)) as ColorPickerDropDown;
                    if (item == null)
                        return;

                    TypeDescriptor.GetProperties(item)["Text"].SetValue(item, item.Name);

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

        protected virtual void CreateContainer(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateContainer(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateContainer(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    ItemContainer item = dh.CreateComponent(typeof(ItemContainer)) as ItemContainer;
                    if (item == null)
                        return;

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

		protected virtual void CreateProgressBar(object sender, EventArgs e)
		{
			OnSubItemsChanging();
            CreateProgressBar(this.GetItemContainer());
			OnSubItemsChanged();
		}

		protected virtual void CreateProgressBar(BaseItem parent)
		{
			if (parent == null)
				return;

			IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
			if (dh != null)
			{
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    ProgressBarItem item = dh.CreateComponent(typeof(ProgressBarItem)) as ProgressBarItem;
                    if (item == null)
                        return;

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
			}
		}

        protected virtual void CreateSwitch(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateSwitch(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateSwitch(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    SwitchButtonItem item = dh.CreateComponent(typeof(SwitchButtonItem)) as SwitchButtonItem;
                    if (item == null)
                        return;

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

        protected virtual void CreateMicroChart(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateMicroChart(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateMicroChart(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    MicroChartItem item = dh.CreateComponent(typeof(MicroChartItem)) as MicroChartItem;
                    if (item == null)
                        return;

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

		protected virtual void CreateDocument(object sender, EventArgs e)
		{
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return;

            DesignerTransaction trans = dh.CreateTransaction("Adding new item");
            try
            {
                m_CreatingItem = true;    
                DockContainerItem item = dh.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
                item.Text = item.Name;
                OnSubItemsChanging();
                this.GetItemContainer().SubItems.Add(item);
                PanelDockContainer panel = dh.CreateComponent(typeof(PanelDockContainer)) as PanelDockContainer;
                this.Control.Controls.Add(panel);
                panel.ColorSchemeStyle = this.InternalStyle;
                panel.ApplyLabelStyle();
                item.Control = panel;
                OnSubItemsChanged();
                this.RecalcLayout();
                if (this.Control is Bar)
                {
                    Bar bar = this.Control as Bar;
                    bar.SelectedDockTab = bar.Items.IndexOf(item);
                }
                panel.BringToFront();
            }
            catch
            {
                trans.Cancel();
                throw;
            }
            finally
            {
                m_CreatingItem = false;
                if (!trans.Canceled)
                    trans.Commit();
            }
		}

        protected virtual void CreateControlContainer(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateControlContainer(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateControlContainer(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    ControlContainerItem item = dh.CreateComponent(typeof(ControlContainerItem)) as ControlContainerItem;
                    if (item == null)
                        return;
                    item.AllowItemResize = false;
                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

        protected virtual void CreateGalleryContainer(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateGalleryContainer(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateGalleryContainer(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    GalleryContainer item = dh.CreateComponent(typeof(GalleryContainer)) as GalleryContainer;
                    if (item == null)
                        return;
                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

        protected virtual void CreateCustomizeItem(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateCustomizeItem(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateCustomizeItem(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    CustomizeItem item = dh.CreateComponent(typeof(CustomizeItem)) as CustomizeItem;
                    if (item == null)
                        return;
                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

        protected virtual void CreateMdiWindowList(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateMdiWindowList(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateMdiWindowList(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    MdiWindowListItem item = dh.CreateComponent(typeof(MdiWindowListItem)) as MdiWindowListItem;
                    if (item == null)
                        return;
                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

        protected virtual void CreateSliderItem(object sender, EventArgs e)
        {
            OnSubItemsChanging();
            CreateSliderItem(this.GetItemContainer());
            OnSubItemsChanged();
        }

        protected virtual void CreateSliderItem(BaseItem parent)
        {
            if (parent == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Adding new item");
                try
                {
                    m_CreatingItem = true;
                    SliderItem item = dh.CreateComponent(typeof(SliderItem)) as SliderItem;
                    if (item == null)
                        return;
                    
                    TypeDescriptor.GetProperties(item)["Text"].SetValue(item, item.Name);

                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);

                    parent.SubItems.Add(item);
                    this.RecalcLayout();

                    if (parent != this.GetItemContainer() && cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);

                    OnitemCreated(item);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    m_CreatingItem = false;
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }
        protected virtual void OnitemCreated(BaseItem item) {}
		protected virtual void OnSubItemsChanging() {}
		protected virtual void OnSubItemsChanged() {}

		protected virtual eDotNetBarStyle InternalStyle
		{
			get {return eDotNetBarStyle.Office2003;}
		}
		#endregion

		#region Drag & Drop support
		private void TimerTickDragDrop(object sender, EventArgs e)
		{
			Point p=this.Control.PointToClient(System.Windows.Forms.Control.MousePosition);
			if(this.Control.Bounds.Contains(p))
				m_DragLeave=false;
			else
				m_DragLeave=true;

			if(System.Windows.Forms.Control.MouseButtons!=MouseButtons.Left)
			{
				m_TimerDragDrop.Enabled=false;
				m_TimerDragDrop.Stop();
				m_TimerDragDrop.Tick-=new EventHandler(TimerTickDragDrop);
				m_TimerDragDrop.Dispose();
				m_TimerDragDrop=null;
				if(m_ControlRemoved)
				{
					m_ControlRemoved=false;
					ISelectionService sel=this.GetService(typeof(ISelectionService)) as ISelectionService;
					if(sel!=null && sel.PrimarySelection is System.Windows.Forms.Control)
						ControlRemoved((System.Windows.Forms.Control)sel.PrimarySelection);
				}
                else if (m_DragInProgress)
                {
                    if (m_DragLeave)
                    {
                        ControlContainerItem cc = m_DragItem as ControlContainerItem;
                        MouseDragDrop(-1, -1);
                        if (cc != null)
                        {
                            cc.Control = null;
                            DestroyComponent(cc);
                        }
                    }
                }
			}
		}

        protected override void OnDragLeave(EventArgs e)
        {
            if (m_DragInProgress && m_AcceptExternalControls && !this.IsDockableWindow)
            {
                MouseDragDrop(-1, -1);
            }
            base.OnDragLeave(e);
        }

		protected override void OnDragOver(DragEventArgs de)
		{
			if(m_DragInProgress)
			{
				Point p=this.Control.PointToClient(new Point(de.X,de.Y));
				MouseDragOver(p.X,p.Y);
				de.Effect=DragDropEffects.Move;
				return;
			}

			if(m_EnableItemDragDrop && m_AcceptExternalControls && !this.IsDockableWindow)
			{
				ISelectionService sel=(ISelectionService)this.GetService(typeof(ISelectionService));
				if(sel!=null && sel.PrimarySelection!=this.Component)
				{
					if(sel.PrimarySelection is Control && this.Control.Controls.Contains((Control)sel.PrimarySelection))
					{			
						BaseItem item=GetControlItem(sel.PrimarySelection as Control);
						if(item!=null)
						{
							m_MouseDownPoint=this.Control.PointToClient(new Point(de.X,de.Y));
							m_SuspendInternalCursor=true;
							StartItemDrag(item);
							if(m_TimerDragDrop==null)
							{
								m_TimerDragDrop=new Timer();
								m_TimerDragDrop.Tick+=new EventHandler(this.TimerTickDragDrop);
								m_TimerDragDrop.Interval=100;
								m_TimerDragDrop.Enabled=true;
								m_TimerDragDrop.Start();
							}
						}
						return;
					}
					else if(sel.SelectionCount>1)
					{
						de.Effect=DragDropEffects.None;
						return;
					}
					else if(sel.PrimarySelection is Control && ((Control)sel.PrimarySelection).Parent!=null)
					{
						// New control being added to the container
						BaseItem dragItem=null;
						if(this.IsDockableWindow)
						{
							DockContainerItem dc=new DockContainerItem();
							dc.Name=TEMP_NAME;
							//dc.Control=sel.PrimarySelection as System.Windows.Forms.Control;
							dragItem=dc;
						}
						else
						{
							ControlContainerItem cc=new ControlContainerItem();
                            cc.AllowItemResize = false;
							cc.Name=TEMP_NAME;
							//cc.Control=sel.PrimarySelection as System.Windows.Forms.Control;
							dragItem=cc;
						}
						m_MouseDownPoint=this.Control.PointToClient(new Point(de.X,de.Y));
						m_SuspendInternalCursor=true;
						StartItemDrag(dragItem);
                        if (m_TimerDragDrop == null)
                        {
                            m_TimerDragDrop = new Timer();
                            m_TimerDragDrop.Tick += new EventHandler(this.TimerTickDragDrop);
                            m_TimerDragDrop.Interval = 100;
                            m_TimerDragDrop.Enabled = true;
                            m_TimerDragDrop.Start();
                        }
					}
				}
			}
			base.OnDragOver (de);
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			if(m_EnableItemDragDrop && m_AcceptExternalControls && !this.IsDockableWindow)
			{
				ISelectionService sel=(ISelectionService)this.GetService(typeof(ISelectionService));
				if(sel!=null && sel.PrimarySelection is Control && this.Control.Controls.Contains((Control)sel.PrimarySelection))
				{
					de.Effect=DragDropEffects.Move;
					Point p=this.Control.PointToClient(new Point(de.X,de.Y));
					MouseDragDrop(p.X,p.Y);
				}
				else
				{
					if(sel.SelectionCount>1)
					{
						de.Effect=DragDropEffects.None;
						return;
					}
					else
					{
						if(m_DragItem!=null && m_DragItem.Name==TEMP_NAME && (m_DragItem is ControlContainerItem || m_DragItem is DockContainerItem))
						{
							m_JustAdded=DateTime.Now;
							BaseItem dragItem=null;
							if(m_DragItem is ControlContainerItem)
							{
								ControlContainerItem cc=m_DragItem as ControlContainerItem;
								cc.Control=null;
                                cc = this.CreateComponent(typeof(ControlContainerItem)) as ControlContainerItem;
                                TypeDescriptor.GetProperties(cc)["Control"].SetValue(cc, sel.PrimarySelection as System.Windows.Forms.Control);
                                TypeDescriptor.GetProperties(cc)["AllowItemResize"].SetValue(cc, false);
                                dragItem = cc;
							}
							else if(m_DragItem is DockContainerItem)
							{
								DockContainerItem dc=m_DragItem as DockContainerItem;
								dc.Control=null;
                                dc = this.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
                                TypeDescriptor.GetProperties(dc)["Control"].SetValue(dc, sel.PrimarySelection as System.Windows.Forms.Control);
                                dragItem = dc;
							}
							m_DragItem=dragItem;
							Point p=this.Control.PointToClient(new Point(de.X,de.Y));
							MouseDragDrop(p.X,p.Y);
						}
						m_NewControlAdded=true;
					}
				}
			}
		
			base.OnDragDrop(de);
		}

		protected virtual BaseItem GetControlItem(System.Windows.Forms.Control control)
		{
			BaseItem parent=this.GetItemContainer();
			if(parent==null)
				return null;
			return GetControlItem(control,parent);
		}

		private BaseItem GetControlItem(System.Windows.Forms.Control control, BaseItem parent)
		{
			if(parent is ControlContainerItem && ((ControlContainerItem)parent).Control==control)
				return parent;
			else if(parent is DockContainerItem && ((DockContainerItem)parent).Control==control)
				return parent;

			foreach(BaseItem item in parent.SubItems)
			{
				BaseItem i2=GetControlItem(control,item);
				if(i2!=null)
					return i2;
			}
			return null;
		}

		protected virtual bool DragInProgress
		{
			get {return m_DragInProgress;}
			set {m_DragInProgress=value;}
		}

		protected void StartItemDrag(BaseItem item)
		{
			if(item==null)
				return;

			if(m_DragItem==null)
			{
				m_DragItem=item;
				if(!m_SuspendInternalCursor)
					System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Hand;
				DragInProgress=true;
			}
		}

		protected void MouseDragOver(int x, int y)
		{
			if(!m_DragInProgress)
				return;
			BaseItem dragItem=m_DragItem;
			BaseItem container=this.GetItemContainer();
			
			if(m_DesignTimeProvider!=null)
			{
				m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition,m_InsertBefore);
				m_DesignTimeProvider=null;
			}

			Control control=this.GetItemContainerControl();
			Point pScreen=control.PointToScreen(new Point(x,y));

			InsertPosition pos=((IDesignTimeProvider)container).GetInsertPosition(pScreen, dragItem);
				
			if(pos!=null)
			{
				if(pos.TargetProvider==null)
				{
					// Cursor is over drag item
					if(!m_SuspendInternalCursor)
						System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.No;
				}
				else
				{
					pos.TargetProvider.DrawReversibleMarker(pos.Position,pos.Before);
					m_InsertPosition=pos.Position;
					m_InsertBefore=pos.Before;
					m_DesignTimeProvider=pos.TargetProvider;
					if(!m_SuspendInternalCursor)
						System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Hand;
				}
			}
			else
			{
				if(!m_SuspendInternalCursor)
					System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.No;
			}
		}

        internal BaseItem DragItem
        {
            get { return m_DragItem; }
        }

		internal void MouseDragDrop(int x, int y)
		{
			if(!m_DragInProgress)
				return;
            if (m_DesignerHost != null && m_DesignerHost.Capture)
                m_DesignerHost.Capture = false;
			BaseItem dragItem=m_DragItem;
			BaseItem container=this.GetItemContainer();
			bool changed=false;

			if(m_DesignTimeProvider!=null)
			{
				if(x==-1 && y==-1)
				{
					// Cancel state
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
				}
				else
				{
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
					if(dragItem!=null)
					{
                        m_InternalRemoveInProgress = true;
                        try
                        {
                            BaseItem objParent = dragItem.Parent;
                            if (objParent != null)
                            {
                                if (objParent == (BaseItem)m_DesignTimeProvider && m_InsertPosition > 0)
                                {
                                    if (objParent.SubItems.IndexOf(dragItem) < m_InsertPosition)
                                        m_InsertPosition--;
                                }
                                objParent.SubItems.Remove(dragItem);
                            }
                            m_DesignTimeProvider.InsertItemAt(dragItem, m_InsertPosition, m_InsertBefore);
                            m_DesignTimeProvider = null;
                            changed = true;
                            if (dragItem.Parent != null && dragItem.Parent != this.GetItemContainer())
                            {
                                IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                                if (cc != null)
                                    cc.OnComponentChanged(dragItem.Parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
                            }
                        }
                        finally
                        {
                            m_InternalRemoveInProgress = false;
                        }
					}
				}
			}
			else if(x!=-1 && y!=-1 && dragItem is ControlContainerItem && ((ControlContainerItem)dragItem).Control!=null)
			{
                BaseItem parent = container;
                if (dragItem.Parent != null)
                    dragItem.Parent.SubItems.Remove(dragItem);
                parent.SubItems.Add(dragItem);
                IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                if (cc != null)
                    cc.OnComponentChanged(dragItem.Parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
                changed = true;
			}

			m_DesignTimeProvider=null;
			DragInProgress=false;
			if(!m_SuspendInternalCursor)
				System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Default;
			if(dragItem!=null)
				dragItem._IgnoreClick=true;
			container.InternalMouseUp(new MouseEventArgs(MouseButtons.Left,0,x,y,0));
			if(dragItem!=null)
				dragItem._IgnoreClick=false;

			m_DragItem=null;
			this.RecalcLayout();

			if(changed)
			{
				IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
				if(cc!=null)
				{
					cc.OnComponentChanged(this.Control,null,null,null);
				}
			}
		}

		/// <summary>
		/// Gets or sets whether drag and drop of BaseItems is enabled. Default is false.
		/// </summary>
		protected virtual bool EnableItemDragDrop
		{
			get {return m_EnableItemDragDrop;}
			set {m_EnableItemDragDrop=value;}
		}

		/// <summary>
		/// Gets or sets whether dropping of external control into Bar is enabled. Default is false. 
		/// </summary>
		protected virtual bool AcceptExternalControls
		{
			get {return m_AcceptExternalControls;}
			set {m_AcceptExternalControls=value;}
		}

		public void StartExternalDrag(BaseItem item)
		{
			if(!m_DragInProgress && m_DesignerHost!=null)
			{
				m_SuspendInternalCursor=false;
				m_MouseDownPoint=this.Control.PointToClient(System.Windows.Forms.Control.MousePosition);
				this.StartItemDrag(item);
				m_DesignerHost.Capture=true;
			}
		}

		protected virtual bool IsDockableWindow
		{
			get {return false;}
		}

		protected virtual void SelectComponent(IComponent comp, SelectionTypes selectionType)
		{
			ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
			if(selection!=null && comp!=null)
			{
				ArrayList arr=new ArrayList(1);
				arr.Add(comp);
				selection.SetSelectedComponents(arr,selectionType);
			}
		}

		protected virtual bool MouseDownSelectionPerformed
		{
			get {return m_MouseDownSelectionPerformed;}
			set {m_MouseDownSelectionPerformed=value;}
		}

        protected virtual bool PassiveContainer
        {
            get { return m_PassiveContainer; }
            set { m_PassiveContainer = value; }
        }
		#endregion

		#region IDesignerServices Implementation
		public object CreateComponent(System.Type componentClass)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;
            object comp = null;
            try
            {
                m_CreatingItem = true;
                comp = dh.CreateComponent(componentClass);
            }
            finally
            {
                m_CreatingItem = false;
            }

            return comp;
		}

        public object CreateComponent(System.Type componentClass, string name)
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return null;
            object comp = null;
            try
            {
                m_CreatingItem = true;
                comp = dh.CreateComponent(componentClass, name);
            }
            finally
            {
                m_CreatingItem = false;
            }

            return comp;
        }

		public void DestroyComponent(IComponent c)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;
			dh.DestroyComponent(c);
		}

		object IDesignerServices.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}
		#endregion

		private void DesignerSelectionChanged(object sender, EventArgs e)
		{
			ISelectionService sel=this.GetService(typeof(ISelectionService)) as ISelectionService;
			if(sel==null)
			{
				SetComponentSelected(false);
				return;
			}
			bool selected=false;
			if(sel.PrimarySelection==this.Control || sel.PrimarySelection==this.GetItemContainerControl())
				selected=true;
			else if(sel.PrimarySelection is BaseItem && ((BaseItem)sel.PrimarySelection).ContainerControl==this.GetItemContainerControl())
				selected=true;

			SetComponentSelected(selected);
		}
	}

	#region IDesignerServices
	/// <summary>
	/// Classes used for internal support of design-time features.
	/// </summary>
	public interface IDesignerServices
	{
		/// <summary>
		/// Creates new component.
		/// </summary>
		/// <param name="componentClass">Component type to create</param>
		/// <returns>New instance of component</returns>
		object CreateComponent(System.Type componentClass);

        /// <summary>
        /// Creates new component.
        /// </summary>
        /// <param name="componentClass">Component type to create</param>
        /// <param name="name">Name to assign to component</param>
        /// <returns>New instance of component</returns>
        object CreateComponent(System.Type componentClass, string name);

		/// <summary>
		/// Destroys component
		/// </summary>
		/// <param name="c">Component to destroy</param>
		void DestroyComponent(IComponent c);

		/// <summary>
		/// Gets specified designer service.
		/// </summary>
		/// <param name="serviceType">Type of the service to get</param>
		/// <returns>Returns reference to the service.</returns>
		object GetService(Type serviceType);
	}
	#endregion
}
