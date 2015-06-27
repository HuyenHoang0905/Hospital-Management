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
	/// Summary description for TabStripDesigner.
	/// </summary>
	public class TabStripDesigner:ParentControlDesigner
	{
		public TabStripDesigner()
		{
		}

		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;

			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged+=new EventHandler(OnSelectionChanged);

			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.ComponentRemoving+=new ComponentEventHandler(this.OnComponentRemoving);

			TabStrip tabStrip=this.GetTabStrip();
			if(tabStrip!=null)
				tabStrip.TabMoved+=new TabStrip.TabMovedEventHandler(this.TabMoved);
		}

		protected override void Dispose(bool disposing)
		{
			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged-=new EventHandler(OnSelectionChanged);

			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);

			base.Dispose(disposing);
		}
		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs;
				verbs = new DesignerVerb[]
				{
                    new DesignerVerb("Next Tab", new EventHandler(SelectNextTab)),
                    new DesignerVerb("Previous Tab", new EventHandler(SelectPreviousTab)),
                    new DesignerVerb("Create New Tab", new EventHandler(CreateNewTab))
				};
				return new DesignerVerbCollection(verbs);
			}
		}

		protected virtual void CreateNewTab(object sender, EventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			TabStrip tabStrip=this.Control as TabStrip;
			if(tabStrip==null || dh==null)
				return;

            DesignerTransaction dt = dh.CreateTransaction();
            try
            {
                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                    change.OnComponentChanging(this.Component, null);

                TabItem tab = dh.CreateComponent(typeof(TabItem)) as TabItem;
                tab.Text = tab.Name;

                tabStrip.Tabs.Add(tab);

                if (change != null)
                    change.OnComponentChanged(this.Component, null, null, null);
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if (!dt.Canceled) dt.Commit();
            }
		}

        private void SelectNextTab(object sender, EventArgs e)
        {
            TabStrip tabStrip = this.Control as TabStrip;
            if (tabStrip == null)
                return;
            if (tabStrip.SelectedTabIndex < tabStrip.Tabs.Count - 1)
                TypeDescriptor.GetProperties(tabStrip)["SelectedTabIndex"].SetValue(tabStrip, tabStrip.SelectedTabIndex + 1);
        }

        private void SelectPreviousTab(object sender, EventArgs e)
        {
            TabStrip tabStrip = this.Control as TabStrip;
            if (tabStrip == null)
                return;
            if (tabStrip.SelectedTabIndex >0)
                TypeDescriptor.GetProperties(tabStrip)["SelectedTabIndex"].SetValue(tabStrip, tabStrip.SelectedTabIndex - 1);
        }

		private void TabMoved(object sender, TabStripTabMovedEventArgs e)
		{
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				change.OnComponentChanging(this.Control,null);
				change.OnComponentChanged(this.Control,null,null,null);
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			TabStrip tabStrip=this.GetTabStrip();
			if(tabStrip==null || tabStrip.IsDisposed)
				return;
			
			ISelectionService ss = (ISelectionService)sender;
			if(ss!=null && ss.PrimarySelection!=this.Control)
			{
				if(ss.PrimarySelection is TabItem && tabStrip.Tabs.Contains(ss.PrimarySelection as TabItem))
				{
					tabStrip.DesignTimeSelection=ss.PrimarySelection as TabItem;
					return;
				}
			}
			tabStrip.DesignTimeSelection=null;
		}

		protected bool m_InternalRemove=false;
		private void OnComponentRemoving(object sender,ComponentEventArgs e)
		{
			InternalComponentRemoving(e);
		}

		protected virtual void InternalComponentRemoving(ComponentEventArgs e)
		{
			if(m_InternalRemove)
				return;
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;

			TabStrip tabStrip=this.GetTabStrip();
			if(tabStrip==null || tabStrip.IsDisposed)
				return;

			if(e.Component is TabItem && this.Control!=null && tabStrip.Tabs.Contains(e.Component as TabItem))
			{
				try
				{
					TabItem item=e.Component as TabItem;
					if(item.AttachedControl!=null && this.Control.Controls.Contains(item.AttachedControl))
					{
						this.Control.Controls.Remove(item.AttachedControl);
						dh.DestroyComponent(item.AttachedControl);
					}
					tabStrip.Tabs.Remove(item);
					tabStrip.RecalcSize();
					tabStrip.Refresh();
				}
				finally
				{
					m_InternalRemove=false;
				}
			}
            else if (e.Component == this.Control)
            {
                m_InternalRemove = true;
                try
                {
                    TabStrip tabControl = this.Control as TabStrip;
                    TabItem[] tabs = new TabItem[tabControl.Tabs.Count];
                    tabControl.Tabs.CopyTo(tabs, 0);
                    foreach (TabItem tab in tabs)
                        dh.DestroyComponent(tab);
                }
                finally
                {
                    m_InternalRemove = false;
                }
            }
		}

		protected virtual TabStrip GetTabStrip()
		{
			return this.Control as TabStrip;
		}

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            if (this.Control is TabStrip)
            {
                properties["MdiTabbedDocuments"] = TypeDescriptor.CreateProperty(typeof(TabStripDesigner), (PropertyDescriptor)properties["MdiTabbedDocuments"], new Attribute[]
				{
					new DefaultValueAttribute(false)});
            }
        }

        /// <summary>
        /// Gets or sets whether Tab-Strip control provides Tabbed MDI Child form support. Default value is false.
        /// </summary>
        [Browsable(true), Description("Indicates whether Tab-Strip control provides Tabbed MDI Child form support."), Category("Mdi Support"), DefaultValue(false)]
        public bool MdiTabbedDocuments
        {
            get
            {
                return (bool)ShadowProperties["MdiTabbedDocuments"];
            }
            set
            {
                IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
                DesignerTransaction dt = null;
                if (dh != null) dt = dh.CreateTransaction();

                try
                {
                    // this value is not passed to the actual control
                    this.ShadowProperties["MdiTabbedDocuments"] = value;
                    TabStrip ts = this.Control as TabStrip;
                    if (ts != null)
                    {
                        if (dh == null || dh.Loading)
                            return;

                        if (value)
                        {
                            TypeDescriptor.GetProperties(ts)["MdiForm"].SetValue(ts, null);
                            
                            System.Windows.Forms.Form f = dh.RootComponent as System.Windows.Forms.Form;
                            TypeDescriptor.GetProperties(ts)["MdiForm"].SetValue(ts, f);
                        }
                        else
                            TypeDescriptor.GetProperties(ts)["MdiForm"].SetValue(ts, null);
                    }
                }
                catch
                {
                    if (dt != null) dt.Cancel();
                    throw;
                }
                finally
                {
                    if (dt != null && !dt.Canceled) dt.Commit();
                }
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
            if (this.Component == null || this.Component.Site == null || !this.Component.Site.DesignMode)
                return;
            TabStrip ts = this.Control as TabStrip;
            if (ts != null)
                ts.Style = eTabStripStyle.VS2005;
            this.CreateNewTab(null, null);
        }

		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				ArrayList c=new ArrayList(base.AssociatedComponents);
				TabStrip tabStrip=this.GetTabStrip();
				if(tabStrip!=null)
				{
					foreach(TabItem tab in tabStrip.Tabs)
						c.Add(tab);
				}

				return c;
			}
		}
		#region WndProc Support
		/// <summary>
		/// Selection support for items on container.
		/// </summary>
		private TabItem m_SelectItem=null;
		protected override void WndProc(ref Message m)
		{
			System.Windows.Forms.Control ctrl=this.Control;
			if(ctrl==null || ctrl.IsDisposed)
			{
				base.WndProc(ref m);
				return;
			}

			switch(m.Msg)
			{
				case WinApi.WM_LBUTTONDOWN:
				case WinApi.WM_RBUTTONDOWN:
				{
					if(OnMouseDown(ref m))
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
						return;
					break;
				}
				case WinApi.WM_MOUSELEAVE:
				{
					if(OnMouseLeave(ref m))
						return;
					break;
				}
				case WinApi.WM_USER+101:
				{
					if(OnProcessPendingSelection(ref m))
						return;
					break;
				}
			}

			base.WndProc(ref m);
		}

		private bool m_IgnoreMouseUp=false;
		private bool m_SelectTabControl=false;
		protected virtual bool OnMouseDown(ref Message m)
		{
			TabStrip tabStrip=this.GetTabStrip();

			if(tabStrip==null || tabStrip.IsDisposed || this.Control.IsDisposed)
				return false;

			Point pos=tabStrip.PointToClient(System.Windows.Forms.Control.MousePosition);
			MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);
			TabItem oldSelected=tabStrip.SelectedTab;
			tabStrip.InternalOnMouseDown(e);

			ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
			TabItem selected=null;
			if(selection!=null && selection.PrimarySelection is TabItem)
				selected=selection.PrimarySelection as TabItem;

			if(tabStrip.SelectedTab!=null && selected!=tabStrip.SelectedTab && tabStrip.HitTest(e.X,e.Y)!=null)
			{
				ArrayList arr=new ArrayList(1);
				arr.Add(tabStrip.SelectedTab);
#if FRAMEWORK20
                selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif

                //m_SelectItem=tabStrip.SelectedTab;
				//WinApi.PostMessage(m.HWnd.ToInt32(),WinApi.WM_USER+101,0,0);
				m_IgnoreMouseUp=true;

				if(m.Msg==WinApi.WM_RBUTTONDOWN)
					this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);
				return true;
			}
			else if(tabStrip.SelectedTab==oldSelected)
			{
				if(tabStrip.HitTest(e.X,e.Y)!=null)
				{
					if(m.Msg==WinApi.WM_RBUTTONDOWN)
						this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);
					return true;
				}
			}
			
			return false;
		}

		protected virtual bool OnMouseUp(ref Message m)
		{
			TabStrip tabStrip=this.GetTabStrip();
			if(tabStrip==null || tabStrip.IsDisposed || this.Control.IsDisposed)
				return false;

			Point pos=tabStrip.PointToClient(System.Windows.Forms.Control.MousePosition);
			MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);

			tabStrip.InternalOnMouseUp(e);
			
			if(m_IgnoreMouseUp)
			{
				m_IgnoreMouseUp=false;
				return true;
			}

			if(m_SelectTabControl)
			{
				m_SelectTabControl=false;
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null && selection.PrimarySelection!=this.Control && !(selection.PrimarySelection.GetType() == this.Control.GetType()))
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(this.Control);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.Click);
#endif
                }
			}

			return false;
		}

		protected virtual bool OnMouseMove(ref Message m)
		{
			TabStrip tabStrip=this.GetTabStrip();
			if(tabStrip==null || tabStrip.IsDisposed || this.Control.IsDisposed)
				return false;

			Point pos=tabStrip.PointToClient(System.Windows.Forms.Control.MousePosition);
			MouseEventArgs e=new MouseEventArgs((m.WParam.ToInt32()==1?MouseButtons.Left:MouseButtons.None),0,pos.X,pos.Y,0);
			tabStrip.InternalOnMouseMove(e);
			if(tabStrip.DisplayRectangle.Contains(pos))
				return true;

			return false;
		}

		protected virtual bool OnMouseLeave(ref Message m)
		{
			TabStrip tabStrip=this.GetTabStrip();
			if(tabStrip==null || tabStrip.IsDisposed || this.Control.IsDisposed)
				return false;
			tabStrip.InternalOnMouseLeave(new EventArgs());
			return false;
		}

		protected virtual bool OnProcessPendingSelection(ref Message m)
		{
			return ProcessPendingSelection();
		}

		protected virtual bool ProcessPendingSelection()
		{
			TabStrip tabStrip=this.GetTabStrip();
			if(tabStrip==null || tabStrip.IsDisposed || this.Control.IsDisposed)
				return false;

			if(m_SelectItem!=null)
			{
				tabStrip.DesignTimeSelection=m_SelectItem;
				//Point pos=tabStrip.PointToClient(System.Windows.Forms.Control.MousePosition);
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection==null)
					return false;
				ArrayList arr=new ArrayList(1);
				arr.Add(m_SelectItem);
#if FRAMEWORK20
                selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                selection.SetSelectedComponents(arr,SelectionTypes.Click);
#endif

                m_SelectItem =null;

				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if(change!=null)
				{
					change.OnComponentChanging(this.Control,null);
					change.OnComponentChanged(this.Control,null,null,null);
				}
				return true;
			}
			return false;
		}
		#endregion
	}
}
