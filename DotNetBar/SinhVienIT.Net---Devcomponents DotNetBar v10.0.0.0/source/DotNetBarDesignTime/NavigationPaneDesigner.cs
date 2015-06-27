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
	/// Summary description for NavigationPaneDesigner.
	/// </summary>
	public class NavigationPaneDesigner:NavigationBarDesigner
	{
		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;
			NavigationPane pane=this.Control as NavigationPane;
			if(pane!=null)
				pane.SetDesignMode();
		}
		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs;
				verbs = new DesignerVerb[]
				{
					new DesignerVerb("Create New Pane", new EventHandler(CreateNewPane)),
				};
				return new DesignerVerbCollection(verbs);
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
            CreateNewPane();
            NavigationPane pane = this.Control as NavigationPane;
            pane.Style = eDotNetBarStyle.StyleManagerControlled;
        }

		private void CreateNewPane(object sender, EventArgs e)
		{
			CreateNewPane();
		}

		private void CreateNewPane()
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			NavigationPane navPane=this.Control as NavigationPane;
			if(navPane==null || dh==null)
				return;

            DesignerTransaction dt = dh.CreateTransaction();
            try
            {
                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                    change.OnComponentChanging(this.Component, null);

                ButtonItem item = null;
                try
                {
                    m_CreatingItem = true;
                    item = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
                    item.Text = item.Name;
                    item.OptionGroup = "navBar";
                    item.Image = Helpers.LoadBitmap("SystemImages.DefaultNavBarImage.png");
                }
                finally
                {
                    m_CreatingItem = false;
                }

                NavigationPanePanel panel = dh.CreateComponent(typeof(NavigationPanePanel)) as NavigationPanePanel;
                panel.ParentItem = item;
                navPane.Items.Add(item);
                navPane.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                panel.SendToBack();
                panel.ApplyLabelStyle();
                panel.ColorSchemeStyle = navPane.Style;
                if (Helpers.IsOffice2007Style(navPane.Style))
                    panel.ColorScheme = navPane.NavigationBar.GetColorScheme();
                panel.Style.Border = eBorderType.None;
                panel.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;

                if (navPane.Items.Count == 1)
                    item.Checked = true;
                navPane.RecalcLayout();
                if (change != null)
                    change.OnComponentChanged(this.Component, null, null, null);

                if (change != null)
                {
                    change.OnComponentChanging(panel, null);
                    change.OnComponentChanged(panel, null, null, null);
                }
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

		public override bool CanParent(Control c)
		{
			if(c is NavigationPanePanel)
				return true;
			return false;
		}

		protected override BaseItem GetItemContainer()
		{
			NavigationPane bar=this.Control as NavigationPane;
			if(bar!=null)
				return bar.NavigationBar.GetBaseItemContainer();
			return null;
		}

		protected override System.Windows.Forms.Control GetItemContainerControl()
		{
			NavigationPane bar=this.Control as NavigationPane;
			if(bar!=null)
				return bar.NavigationBar;
			return null;
		}
		/// <summary>
		/// Triggered when some other component on the form is removed.
		/// </summary>
		protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
		{
			base.OtherComponentRemoving(sender,e);

			NavigationPane bar=this.Component as NavigationPane;
			if(bar==null || bar.IsDisposed)
				return;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;
            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));

			// Check is any of the buttons or panel that we host
			if(!m_InternalRemoving && e.Component is BaseItem && dh.TransactionDescription.StartsWith("Delete "))
			{
                ButtonItem item = e.Component as ButtonItem;
                if (item != null)
                {
                    NavigationPanePanel panel = bar.GetPanel(item);
                    if (panel != null && dh != null)
                    {
                        DesignerTransaction dt = dh.CreateTransaction("Removing associated NavigationPanePanel");
                        panel.ParentItem = null;
                        if (cc != null) cc.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["Controls"]);
                        bar.Controls.Remove(panel);
                        if (cc != null) cc.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["Controls"], null, null);
                        dh.DestroyComponent(panel);
                        dt.Commit();
                        bar.RecalcLayout();
                    }
                }
			}
			else if(!m_InternalRemoving && e.Component is NavigationPanePanel)
			{
				if(bar.Controls.Contains(e.Component as NavigationPanePanel))
				{
					NavigationPanePanel navpane=e.Component as NavigationPanePanel;
                    if (navpane.ParentItem != null)
                    {
                        BaseItem item = navpane.ParentItem;
                        navpane.ParentItem = null;
                        if (cc != null) cc.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["Items"]);
                        bar.Items.Remove(item);
                        if (cc != null) cc.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["Items"], null, null);
                        dh.DestroyComponent(item);
                    }
					bar.RecalcLayout();
				}
			}

            if (bar.CheckedButton == null && bar.Items.Count > 0 && bar.Items[0] is ButtonItem)
            {
                ButtonItem buttonItem = (ButtonItem)bar.Items[0];
                buttonItem.Checked = true;
            }
		}

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["Expanded"] = TypeDescriptor.CreateProperty(typeof(NavigationPaneDesigner), (PropertyDescriptor)properties["Expanded"], new Attribute[]
				{
					new DefaultValueAttribute(true),
					new BrowsableAttribute(true),
					new CategoryAttribute("Title"),
                    new DescriptionAttribute("Indicates whether navigation pane can be collapsed.")});
        }

        /// <summary>
        /// Gets or sets whether navigation pane is expanded. Default value is true. 
        /// When control is collapsed it is reduced in size so it consumes less space.
        /// </summary>
        [Browsable(true), Category("Title"), DefaultValue(true), Description("Indicates whether navigation pane can be collapsed.")]
        public bool Expanded
        {
            get { return (bool)ShadowProperties["Expanded"]; }
            set
            {
                // this value is not passed to the actual control at design-time
                this.ShadowProperties["Expanded"] = value;
            }
        }
		#region Commented Out
//		public override System.Collections.ICollection AssociatedComponents
//		{
//			get
//			{
//				ArrayList c=new ArrayList(base.AssociatedComponents);
//				NavigationPane navpane=this.Control as NavigationPane;
//				if(navpane!=null)
//				{
//					foreach(BaseItem item in navpane.Items)
//						c.Add(item);
//				}
//				return c;
//			}
//		}

//		private bool m_InternalRemoving=false;
//		private void OnComponentRemoving(object sender,ComponentEventArgs e)
//		{
//			NavigationPane bar=this.Component as NavigationPane;
//			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
//
//			if(e.Component!=this.Component)
//			{
//				// Check is any of the buttons or panel that we host
//				if(!m_InternalRemoving && e.Component is BaseItem)
//				{
//					BaseItem item=e.Component as BaseItem;
//					if(bar.NavigationBar.GetItem(item.Name)==item)
//					{
//						if(item is ButtonItem)
//						{
//							NavigationPanePanel panel=bar.GetPanel(item as ButtonItem);
//							if(panel!=null && dh!=null)
//							{
//								bar.Controls.Remove(panel);
//								dh.DestroyComponent(panel);
//							}
//						}
//						item.Parent.SubItems.Remove(item);
//						bar.RecalcLayout();
//					}
//									}
//				else if(!m_InternalRemoving && e.Component is NavigationPanePanel)
//				{
//					if(bar.Controls.Contains(e.Component as NavigationPanePanel))
//					{
//						NavigationPanePanel navpane=e.Component as NavigationPanePanel;
//						if(navpane.ParentItem!=null)
//							bar.Items.Remove(navpane.ParentItem);
//						bar.RecalcLayout();
//					}
//				}
//				return;
//			}
//
//			m_InternalRemoving=true;
//			try
//			{
//				// Unhook events
//				IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
//				if(cc!=null)
//					cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);
//
//				if(dh==null || bar==null)
//					return;
//
//				foreach(BaseItem item in bar.Items)
//				{
//					DestroySubItems(item,dh);
//					dh.DestroyComponent(item);
//				}
//			}
//			finally
//			{
//				m_InternalRemoving=false;
//			}
//		}
//		private void DestroySubItems(BaseItem parent, IDesignerHost dh)
//		{
//			foreach(BaseItem item in parent.SubItems)
//			{
//				DestroySubItems(item,dh);
//				dh.DestroyComponent(item);
//			}
//		}

//		private void OnSelectionChanged(object sender, EventArgs e) 
//		{
//			ISelectionService ss = (ISelectionService)sender;
//			if(ss.PrimarySelection!=this.Control)
//			{
//				NavigationPane bar=this.Control as NavigationPane;
//				if(ss.PrimarySelection is BaseItem)
//				{
//					BaseItem item=ss.PrimarySelection as BaseItem;
//					if(!bar.Items.Contains(item))
//					{
//						foreach(BaseItem panel in bar.Items)
//						{
//							if(panel.SubItems.Contains(item))
//								return;
//						}
//						((IOwner)bar).SetFocusItem(null);
//					}
//				}
//				else
//				{
//					((IOwner)bar).SetFocusItem(null);
//					((IOwner)bar).OnApplicationDeactivate(); // Closes all popups
//				}
//			}
//		}

//		private BaseItem m_SelectItem=null;
//		protected override void WndProc(ref Message m)
//		{
//			NavigationPane navpane=this.Control as NavigationPane;
//			if(navpane==null || navpane.IsDisposed)
//			{
//				base.WndProc(ref m);
//				return;
//			}
//			NavigationBar navbar=navpane.NavigationBar;
//			if(navbar==null || navbar.ItemsContainer==null || navbar.IsDisposed)
//			{
//				base.WndProc(ref m);
//				return;
//			}
//
//			switch(m.Msg)
//			{
//				case NativeFunctions.WM_LBUTTONDOWN:
//				case NativeFunctions.WM_RBUTTONDOWN:
//				{
//					Point pos=navbar.PointToClient(System.Windows.Forms.Control.MousePosition);
//					MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);
//					BaseItem focusItem=((IOwner)navbar).GetFocusItem();
//					navbar.ItemsContainer.InternalMouseDown(e);
//					if(focusItem==((IOwner)navbar).GetFocusItem())
//					{
//						if(m.Msg==NativeFunctions.WM_RBUTTONDOWN)
//							return;
//
//						((IOwner)navbar).SetFocusItem(null);
//					}
//					
//					if(m.Msg==NativeFunctions.WM_LBUTTONDOWN)
//					{
//						if(navbar.HitTestSplitter(e.X,e.Y))
//							navbar.SplitterMouseDown(e);
//						if(navbar.IsSplitterMouseDown || navbar.ClientRectangle.Contains(pos))
//							return;
//					}
//					break;
//				}
//				case NativeFunctions.WM_RBUTTONUP:
//				case NativeFunctions.WM_LBUTTONUP:
//				{
//					Point pos=navbar.PointToClient(System.Windows.Forms.Control.MousePosition);
//					MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);
//					navbar.ItemsContainer.InternalMouseUp(e);
//					
//					// Design-time splitter support
//					if(navbar.IsSplitterMouseDown)
//					{
//						navbar.SplitterMouseUp(e);
//						IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
//						if(change!=null)
//						{
//							change.OnComponentChanging(this.Control,TypeDescriptor.GetProperties(this.Control).Find("NavigationBarHeight",true));
//							change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(this.Control).Find("NavigationBarHeight",true),0,navpane.NavigationBarHeight);
//						}
//					}
//					else
//						navbar.SplitterMouseUp(e);
//
//					if(((IOwner)navbar).GetFocusItem()!=null)
//					{
//						m_SelectItem=((IOwner)navbar).GetFocusItem();
//						NativeFunctions.PostMessage(m.HWnd.ToInt32(),NativeFunctions.WM_USER+101,0,0);
//					}
//					else
//					{
//						ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
//						if(selection.PrimarySelection!=navpane && !(selection.PrimarySelection.GetType() == typeof(NavigationPanePanel)))
//						{
//							ArrayList arr=new ArrayList(1);
//							arr.Add(navpane);
//							selection.SetSelectedComponents(arr,SelectionTypes.Click);
//						}
//					}
//
//					break;
//				}
//				case NativeFunctions.WM_MOUSEMOVE:
//				{
//					Point pos=navbar.PointToClient(System.Windows.Forms.Control.MousePosition);
//					MouseEventArgs e=new MouseEventArgs(System.Windows.Forms.Control.MouseButtons,0,pos.X,pos.Y,0);
//					navbar.SplitterMouseMove(e);
//					break;
//				}
//				case NativeFunctions.WM_MOUSELEAVE:
//				{
//					navbar.SplitterMouseLeave();
//					break;
//				}
//				case NativeFunctions.WM_USER+101:
//				{
//					if(ProcessPendingSelection())
//						return;
//					break;
//				}
//			}
//
//			base.WndProc(ref m);
//		}
//
//		private bool ProcessPendingSelection()
//		{
//			NavigationPane navpane=this.Control as NavigationPane;
//			NavigationBar navbar=navpane.NavigationBar;
//			if(navbar==null)
//				return false;
//			if(m_SelectItem!=null)
//			{
//				Point pos=navbar.PointToClient(System.Windows.Forms.Control.MousePosition);
//				if(m_SelectItem is ButtonItem && !((ButtonItem)m_SelectItem).Checked)
//				{
//					ButtonItem button=m_SelectItem as ButtonItem;
//					IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
//					if(change!=null)
//						change.OnComponentChanging(button,TypeDescriptor.GetProperties(button).Find("Checked",true));
//					m_SelectItem.InternalClick(MouseButtons.Left,pos);
//					button.Checked=true;
//					if(change!=null)
//						change.OnComponentChanged(button,TypeDescriptor.GetProperties(button).Find("Checked",true),null,null);
//				}
//				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
//				ArrayList arr=new ArrayList(1);
//				arr.Add(m_SelectItem);
//				selection.SetSelectedComponents(arr,SelectionTypes.Click);
//
//				m_SelectItem=null;
//				return true;
//			}
//			return false;
//		}
//		public override void Initialize(IComponent component) 
//		{
//			base.Initialize(component);
//			if(!component.Site.DesignMode)
//				return;
//
//			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
//			if(ss!=null)
//				ss.SelectionChanged+=new EventHandler(OnSelectionChanged);
//
//			// If our component is removed we need to clean-up
//			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
//			if(cc!=null)
//				cc.ComponentRemoving+=new ComponentEventHandler(this.OnComponentRemoving);
//		}
//
		#endregion
	}
}
