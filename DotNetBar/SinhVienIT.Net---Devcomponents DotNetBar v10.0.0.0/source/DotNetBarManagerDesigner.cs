using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Collections;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for DotNetBarDesigner.
	/// </summary>
	public class DotNetBarManagerDesigner:System.ComponentModel.Design.ComponentDesigner,IDesignerServices
	{
		private bool m_PopupProvider=false;
		private bool m_Selected=false;
		
		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[] {new DesignerVerb("Edit DotNetBar", new EventHandler(OnEditDotNetBar))															  ,
					new DesignerVerb("Enable Document Docking", new EventHandler(OnEnableDocumentDocking))};
				return new DesignerVerbCollection(verbs);
			}
		}

		public override void DoDefaultAction()
		{
			DotNetBarManager dbm=this.Component as DotNetBarManager;
			if(dbm!=null)
			{
				dbm.Customize(this);
			}
		}

		protected override void Dispose(bool disposing) 
		{
			// Unhook events
			if(disposing)
			{
				IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
				if(cc!=null)
					cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);
				ISelectionService ss = (ISelectionService)GetService(typeof(ISelectionService));
				if (ss!=null)
					ss.SelectionChanged-=new EventHandler(OnSelectionChanged);
			}

			base.Dispose(disposing);
		}

		private void ComponentRenamed(object sender, ComponentRenameEventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;

			if(e.Component==dh.RootComponent || e.Component==this.Component)
			{
				DotNetBarManager m=this.Component as DotNetBarManager;
				if(m!=null && m.DefinitionName!="")
				{
					if(DesignTimeDte.DeleteFromProject(m.DefinitionName, this.Component.Site))
					{
						m.DefinitionName="";
						m.SaveDesignDefinition();
					}
				}
			}
		}
		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;

			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
			{
				cc.ComponentRemoving+=new ComponentEventHandler(this.OnComponentRemoving);
				cc.ComponentRename+=new ComponentRenameEventHandler(this.ComponentRenamed);
			}

			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged+=new EventHandler(OnSelectionChanged);

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;

			if(dh.Loading)
			{
				dh.LoadComplete+=new EventHandler(this.LoadComplete);
				return;
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
            SetupDockSites();

            // Set auto shortcuts that are automatically dispatched...
            DotNetBarManager m = this.Component as DotNetBarManager;
            m.AutoDispatchShortcuts.Add(eShortcut.F1);
            m.AutoDispatchShortcuts.Add(eShortcut.CtrlC);
            m.AutoDispatchShortcuts.Add(eShortcut.CtrlA);
            m.AutoDispatchShortcuts.Add(eShortcut.CtrlV);
            m.AutoDispatchShortcuts.Add(eShortcut.CtrlX);
            m.AutoDispatchShortcuts.Add(eShortcut.CtrlZ);
            m.AutoDispatchShortcuts.Add(eShortcut.CtrlY);
            m.AutoDispatchShortcuts.Add(eShortcut.Del);
            m.AutoDispatchShortcuts.Add(eShortcut.Ins);
            m.Style = eDotNetBarStyle.Office2003;
        }

		private void LoadComplete(object sender, EventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh!=null && e!=null)
				dh.LoadComplete-=new EventHandler(this.LoadComplete);
			LoadDefinition();
		}

		private void LoadDefinition()
		{
			// Make sure at-least this property is set
			DotNetBarManager m=this.Component as DotNetBarManager;

			if(m!=null && !m.IsDisposed && (m.LeftDockSite==null && m.RightDockSite==null && m.TopDockSite==null && m.BottomDockSite==null))
			{
				m_PopupProvider=true;
				m.BarStreamLoad(true);
			}
			else
			{
				m_PopupProvider=false;
				//m.BarStreamLoad(true);
			}
		}

		private void OnEditDotNetBar(object sender, EventArgs e) 
		{
			DotNetBarManager dbm=this.Component as DotNetBarManager;
			if(dbm!=null)
				dbm.Customize(this);
		}

		private void OnEnableDocumentDocking(object sender, EventArgs e)
		{
			DotNetBarManager m=this.Component as DotNetBarManager;
			if(m.FillDockSite==null)
			{
				IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
				if(dh==null)
					return;

				System.Windows.Forms.Control parent=dh.RootComponent as System.Windows.Forms.Control;
				if(parent==null)
					return;

				IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
				if(cc==null)
					return;

				DesignerTransaction trans=dh.CreateTransaction("Document Docking Enabled");
				
                DockSite fillDock=dh.CreateComponent(typeof(DockSite)) as DockSite;
                fillDock.Dock=System.Windows.Forms.DockStyle.Fill;

				cc.OnComponentChanging(parent,TypeDescriptor.GetProperties(typeof(System.Windows.Forms.Control))["Controls"]);
				parent.Controls.Add(fillDock);
				fillDock.BringToFront();
				cc.OnComponentChanged(parent,TypeDescriptor.GetProperties(typeof(System.Windows.Forms.Control))["Controls"],null,null);

				cc.OnComponentChanging(m,TypeDescriptor.GetProperties(typeof(DotNetBarManager))["FillDockSite"]);
				m.FillDockSite=fillDock;
				cc.OnComponentChanged(m,TypeDescriptor.GetProperties(typeof(DotNetBarManager))["FillDockSite"],null,fillDock);

				DocumentDockContainer dockContainer=new DocumentDockContainer();
				cc.OnComponentChanging(fillDock,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
				fillDock.DocumentDockContainer=dockContainer;
				cc.OnComponentChanged(fillDock,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"],null,dockContainer);
				
				Bar bar=dh.CreateComponent(typeof(Bar)) as Bar;
				BarUtilities.InitializeDocumentBar(bar);
//				bar.LayoutType=eLayoutType.DockContainer;
//				bar.DockTabAlignment=eTabStripAlignment.Top;
//				bar.AlwaysDisplayDockTab=true;
//				bar.Stretch=true;
//				bar.GrabHandleStyle=eGrabHandleStyle.None;
				TypeDescriptor.GetProperties(bar)["Style"].SetValue(bar,m.Style);
//				bar.Style=m.Style;
//				bar.CanDockBottom=false;
//				bar.CanDockTop=false;
//				bar.CanDockLeft=false;
//				bar.CanDockRight=false;
//				bar.CanDockDocument=true;
//				bar.CanUndock=false;
//				bar.CanHide=true;
//				bar.CanCustomize=false;

				DockContainerItem item=dh.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
				item.Text=item.Name;
				cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
				bar.Items.Add(item);
				PanelDockContainer panel=dh.CreateComponent(typeof(PanelDockContainer)) as PanelDockContainer;
				cc.OnComponentChanging(bar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
				item.Control=panel;                
				cc.OnComponentChanged(bar, TypeDescriptor.GetProperties(typeof(Bar))["Controls"], null, null);
                //bar.Controls.Add(panel);
				panel.ColorSchemeStyle=m.Style;
				panel.ApplyLabelStyle();
				cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);

				DocumentBarContainer doc=new DocumentBarContainer(bar);
				dockContainer.Documents.Add(doc);
                
				cc.OnComponentChanging(fillDock,TypeDescriptor.GetProperties(typeof(DockSite))["Controls"]);
				fillDock.Controls.Add(bar);
				cc.OnComponentChanged(fillDock,TypeDescriptor.GetProperties(typeof(DockSite))["Controls"],null,null);

				fillDock.RecalcLayout();
				
				trans.Commit();
			}
		}

		#region IDesignerServices Implementation
		public object CreateComponent(System.Type componentClass)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;
			return dh.CreateComponent(componentClass);
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

		private void OnComponentRemoving(object sender,ComponentEventArgs e)
		{
			
			if(e.Component!=this.Component)
			{
				return;
			}
			// Unhook events
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);
            
			DotNetBarManager m=this.Component as DotNetBarManager;
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null || m==null)
				return;

			if(m.TopDockSite!=null)
				dh.DestroyComponent(m.TopDockSite);
			if(m.BottomDockSite!=null)
				dh.DestroyComponent(m.BottomDockSite);
			if(m.LeftDockSite!=null)
				dh.DestroyComponent(m.LeftDockSite);
			if(m.RightDockSite!=null)
				dh.DestroyComponent(m.RightDockSite);

			if(m.DefinitionName!=null)
			{
				DesignTimeDte.DeleteFromProject(m.DefinitionName, this.Component.Site);
				m.DefinitionName="";
			}
		}

		protected override void PreFilterProperties(System.Collections.IDictionary properties) 
		{
			// Always call base first in PreFilter* methods, and last in PostFilter*
			// methods.
			base.PreFilterProperties(properties);

			properties["ProvidePopupsOnly"] = TypeDescriptor.CreateProperty(
				this.GetType(),		// the type this property is defined on
				"ProvidePopupsOnly",	// the name of the property
				typeof(bool),		// the type of the property
				new Attribute[] {CategoryAttribute.Design,System.ComponentModel.DesignerSerializationVisibilityAttribute.Hidden,new DescriptionAttribute("When set to True DotNetBar provides Popups only without Toolbars or Menu Bars. Use on child forms to provide Context Menus.")});	// attributes

			properties["Style"] = TypeDescriptor.CreateProperty(
				this.GetType(),		// the type this property is defined on
				"Style",	// the name of the property
				typeof(eDotNetBarStyle),		// the type of the property
				new Attribute[0]);

			properties["ThemeAware"] = TypeDescriptor.CreateProperty(
				this.GetType(),		// the type this property is defined on
				"ThemeAware",	// the name of the property
				typeof(bool),		// the type of the property
				new Attribute[0]);
		}

		// Popup Provider is design-time only property that determines whether current DotNetBar provides
		// Popup Context Menu only or is full blown Toolbar/Menu bar component
		[DefaultValue(false)]
		protected bool ProvidePopupsOnly
		{
			get 
			{
				return m_PopupProvider;
			}
			set 
			{
				if(m_PopupProvider==value)
					return;
				
				m_PopupProvider=value;

				DotNetBarManager m=this.Component as DotNetBarManager;
				IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));

				if(m_PopupProvider)
				{
					RemoveDockSites();
					if(m!=null && dh!=null && dh.RootComponent is System.Windows.Forms.UserControl)
					{
						m.ParentUserControl=dh.RootComponent as System.Windows.Forms.UserControl;
						IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
						cc.OnComponentChanged(m,(TypeDescriptor.GetProperties(m)["ParentUserControl"]),null,null);
					}
				}
				else
				{
					SetupDockSites();
					if(m!=null)
					{
						m.ParentUserControl=null;
						IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
						cc.OnComponentChanged(m,(TypeDescriptor.GetProperties(m)["ParentUserControl"]),null,null);
					}
				}
			}
		}
		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				DotNetBarManager m=this.Component as DotNetBarManager;
				if(m==null)
					return base.AssociatedComponents;
				System.Collections.ArrayList list=new System.Collections.ArrayList(4);
				if(m.TopDockSite!=null)
					list.Add(m.TopDockSite);
				if(m.BottomDockSite!=null)
					list.Add(m.BottomDockSite);
				if(m.LeftDockSite!=null)
					list.Add(m.LeftDockSite);
				if(m.RightDockSite!=null)
					list.Add(m.RightDockSite);
				if(m.DefinitionName=="")
				{
					list.AddRange(m.Bars);
					foreach(Bar bar in m.Bars)
						AddItems(bar.ItemsContainer,list);
					foreach(BaseItem item in m.Items)
					{
						list.Add(item);
						AddItems(item,list);
					}
					foreach(BaseItem item in m.ContextMenus)
					{
						list.Add(item);
						AddItems(item,list);
					}
				}
				return list;
			}
		}
		private void AddItems(BaseItem parent,System.Collections.ArrayList list)
		{
			foreach(BaseItem item in parent.SubItems)
			{
				if(!item.SystemItem)
				{
					list.Add(item);
					if(item.SubItems.Count>0)
						AddItems(item,list);
				}
			}
		}
		private void RemoveDockSites()
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;		

			DotNetBarManager m=this.Component as DotNetBarManager;

			if(m.LeftDockSite!=null)
				dh.DestroyComponent(m.LeftDockSite);
			if(m.RightDockSite!=null)
				dh.DestroyComponent(m.RightDockSite);
			if(m.TopDockSite!=null)
				dh.DestroyComponent(m.TopDockSite);
			if(m.BottomDockSite!=null)
				dh.DestroyComponent(m.BottomDockSite);

			m.LeftDockSite=null;
			m.TopDockSite=null;
			m.BottomDockSite=null;
			m.RightDockSite=null;
		}

		private void SetupDockSites()
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
			{
				return;
			}

			if(dh.Loading)
			{
				dh.LoadComplete+=new EventHandler(this.LoadComplete);
				return;
			}

			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));

			// Set default properties for this control and add default dependent controls
			DotNetBarManager m=this.Component as DotNetBarManager;
			System.Windows.Forms.Form form=dh.RootComponent as System.Windows.Forms.Form;
			System.Windows.Forms.UserControl parentControl=dh.RootComponent as System.Windows.Forms.UserControl;
			if(m==null || (form==null && parentControl==null))
			{
				return;
			}

			if(form!=null)
			{
				try
				{
					m.ParentForm=form;

					// Left Dock Site
					DockSite ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barLeftDockSite");
					form.Controls.Add(ds);
					m.LeftDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Left;

					// Right Dock Site
					ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barRightDockSite");
					form.Controls.Add(ds);
					m.RightDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Right;

					// Top Dock Site
					ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barTopDockSite");
					form.Controls.Add(ds);
					m.TopDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Top;
					// Bottom Dock Site
					ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barBottomDockSite");
					form.Controls.Add(ds);
					m.BottomDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Bottom;

					if(cc!=null)
					{
						cc.OnComponentChanging(form,TypeDescriptor.GetProperties(form).Find("Controls",true));
						cc.OnComponentChanged(form,TypeDescriptor.GetProperties(form).Find("Controls",true),null,null);
					}
				}
				catch(Exception e)
				{
					// DEBUG
					System.Windows.Forms.MessageBox.Show("Error setting up dock sites: " + e.ToString());
				}
			}
			else if(parentControl!=null)
			{
				try
				{
					// Left Dock Site
					DockSite ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barLeftDockSite");
					parentControl.Controls.Add(ds);
					m.LeftDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Left;

					// Right Dock Site
					ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barRightDockSite");
					parentControl.Controls.Add(ds);
					m.RightDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Right;

					// Top Dock Site
					ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barTopDockSite");
					parentControl.Controls.Add(ds);
					m.TopDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Top;
					// Bottom Dock Site
					ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barBottomDockSite");
					parentControl.Controls.Add(ds);
					m.BottomDockSite=ds;
					ds.Dock=System.Windows.Forms.DockStyle.Bottom;

					if(cc!=null)
					{
						cc.OnComponentChanging(parentControl,TypeDescriptor.GetProperties(parentControl).Find("Controls",true));
						cc.OnComponentChanged(parentControl,TypeDescriptor.GetProperties(parentControl).Find("Controls",true),null,null);
					}
				}
				catch(Exception e)
				{
					// DEBUG
					System.Windows.Forms.MessageBox.Show("Error setting up dock sites on UserControl: " + e.ToString());
				}
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			ISelectionService ss = (ISelectionService)sender;
			if(ss!=null && ss.PrimarySelection==this.Component)
			{
				IOwner manager=this.Component as IOwner;
				manager.OnApplicationActivate();
				m_Selected=true;
			}
			else if(m_Selected)
			{
				IOwner manager=this.Component as IOwner;
				manager.OnApplicationDeactivate();
				m_Selected=false;
			}
		}
		
		public static bool OpenDesignerEditor(DotNetBarManager manager, Bar bar, IDesignerServices designerServices)
		{
			DevComponents.DotNetBar.Design.DotNetBarDesigner designer=new DevComponents.DotNetBar.Design.DotNetBarDesigner();
			if(manager!=null)
				designer.ExternalManager=manager;
			else if(bar!=null)
				designer.ExternalBar=bar;

            if(designerServices!=null)
				designer.DesignerServices=designerServices;

			System.Windows.Forms.DialogResult dr=designer.ShowDialog();

			if(dr==System.Windows.Forms.DialogResult.Yes)
				return true;

			return false;
		}

		/// <summary>
		/// Sets the style of all items in DotNetBar Manager.
		/// </summary>
		public eDotNetBarStyle Style
		{
			set
			{
				DotNetBarManager manager=this.Component as DotNetBarManager;
				if(manager.Style!=value)
				{
					manager.Style=value;
					IDesignerHost dh=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if(dh!=null && !dh.Loading)
						manager.SaveDesignDefinition();
				}
			}
			get
			{
				DotNetBarManager manager=this.Component as DotNetBarManager;
				return manager.Style;
			}
		}

		/// <summary>
		/// Specifies whether new bars created are drawn using Themes when running on OS that supports themes like Windows XP. Note this setting
		/// applies to new bars created.
		/// </summary>
		public bool ThemeAware
		{
			set
			{
				DotNetBarManager manager=this.Component as DotNetBarManager;
				if(manager.ThemeAware!=value)
				{
					manager.ThemeAware=value;
					IDesignerHost dh=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if(dh!=null && !dh.Loading)
						manager.SaveDesignDefinition();
				}
			}
			get
			{
				DotNetBarManager manager=this.Component as DotNetBarManager;
				return manager.ThemeAware;
			}
		}
   
	}
}
