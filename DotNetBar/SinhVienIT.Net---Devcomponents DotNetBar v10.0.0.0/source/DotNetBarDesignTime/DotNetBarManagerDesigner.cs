using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for DotNetBarDesigner.
	/// </summary>
	public class DotNetBarManagerDesigner:System.ComponentModel.Design.ComponentDesigner,IDesignerServices
    {
        #region Private Variables
        //private bool m_PopupProvider=false;
		private bool m_Selected=false;
        #endregion

        #region Licensing Stuff
#if !TRIAL
        internal static string GetLicenseKey()
        {
            string key = "";
            Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine;
            regkey = regkey.OpenSubKey("Software\\DevComponents\\Licenses", false);
            if (regkey != null)
            {
                object keyValue = regkey.GetValue("DevComponents.DotNetBar.DotNetBarManager2");
                if (keyValue != null)
                    key = keyValue.ToString();
            }
            return key;
        }
#endif
        #endregion

        #region Internal Implementation

        public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[] {
                    new DesignerVerb("Create Dock Bar Left", new EventHandler(CreateDockBarLeft)),
                    new DesignerVerb("Create Dock Bar Right", new EventHandler(CreateDockBarRight)),
                    new DesignerVerb("Create Dock Bar Bottom", new EventHandler(CreateDockBarBottom)),
                    new DesignerVerb("Create Dock Bar Top", new EventHandler(CreateDockBarTop)),
                    new DesignerVerb("Create Menu Bar", new EventHandler(CreateMenuBar)),
                    new DesignerVerb("Create Toolbar", new EventHandler(CreateToolbar)),
					new DesignerVerb("Enable Document Docking", new EventHandler(OnEnableDocumentDocking))};
				return new DesignerVerbCollection(verbs);
			}
		}

        //public override void DoDefaultAction()
        //{
        //    DotNetBarManager dbm=this.Component as DotNetBarManager;
        //    if(dbm!=null)
        //    {
        //        dbm.Customize(this);
        //    }
        //}

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

        //private void ComponentRenamed(object sender, ComponentRenameEventArgs e)
        //{
        //    IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
        //    if(dh==null)
        //        return;

        //    if(e.Component==dh.RootComponent || e.Component==this.Component)
        //    {
        //        DotNetBarManager m=this.Component as DotNetBarManager;
        //        if(m!=null && m.DefinitionName!="")
        //        {
        //            if(DesignTimeDte.DeleteFromProject(m.DefinitionName, this.Component.Site))
        //            {
        //                m.DefinitionName="";
        //                m.SaveDesignDefinition();
        //            }
        //        }
        //    }
        //}
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
				//cc.ComponentRename+=new ComponentRenameEventHandler(this.ComponentRenamed);
            }
            #if DEBUG
            if(cc!=null)
                cc.ComponentChanging += new ComponentChangingEventHandler(DebugDesignerComponentChanging);
            #endif

            ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged+=new EventHandler(OnSelectionChanged);

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;

            if (dh.Loading)
			{
				dh.LoadComplete+=new EventHandler(this.LoadComplete);
				return;
			}
			
		}
        #if DEBUG
        void DebugDesignerComponentChanging(object sender, ComponentChangingEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("ComponentChanging");
        }
        #endif

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
            SetupToolbarDockSites();

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
            m.EnableFullSizeDock = false;


#if !TRIAL
            string key = GetLicenseKey();
            m.LicenseKey = key;
#endif
        }

		private void LoadComplete(object sender, EventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh!=null && e!=null)
				dh.LoadComplete-=new EventHandler(this.LoadComplete);
			DotNetBarManager bar = this.Component as DotNetBarManager;
#if !TRIAL
            string key = GetLicenseKey();
            if (key != "" && bar != null && bar.LicenseKey == "" && bar.LicenseKey != key)
                TypeDescriptor.GetProperties(bar)["LicenseKey"].SetValue(bar, key);

            //if (NativeFunctions.keyValidated2 != 266)
            //{
            //    RemindForm frm = new RemindForm();
            //    frm.ShowDialog();
            //}
#endif
            LoadDefinition();

            if (bar.ToolbarTopDockSite == null && bar.ToolbarBottomDockSite == null && bar.ToolbarLeftDockSite == null && bar.ToolbarRightDockSite == null)
                SetupToolbarDockSites();
		}

		private void LoadDefinition()
		{
			// Make sure at-least this property is set
			DotNetBarManager m=this.Component as DotNetBarManager;

            if (m.DefinitionName.Length > 0)
            {
                DialogResult dr = MessageBox.Show("DotNetBarManager needs to be upgraded to code based serialization. Please make sure that you have BACKUP of this form prior to proceeding.\r\n\r\nDo you want to proceed with the upgrade?", "DotNetBarManager Upgrade", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    ImportDefinition(m);
                    m.DefinitionName = "";
                    MessageBox.Show("DotNetBarManager upgraded. DO NOT save this form unless you have backup.", "DotNetBarManager Upgrade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

			if(m!=null && !m.IsDisposed && (m.LeftDockSite==null && m.RightDockSite==null && m.TopDockSite==null && m.BottomDockSite==null))
			{
				//m_PopupProvider=true;
				m.BarStreamLoad(true);
			}
			else
			{
				//m_PopupProvider=false;
				//m.BarStreamLoad(true);
			}
		}

		private void OnEditDotNetBar(object sender, EventArgs e) 
		{
			DotNetBarManager dbm=this.Component as DotNetBarManager;
			if(dbm!=null)
				dbm.Customize();
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
				TypeDescriptor.GetProperties(bar)["Style"].SetValue(bar,m.Style);

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

        public object CreateComponent(System.Type componentClass, string name)
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return null;
            return dh.CreateComponent(componentClass, name);
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

            // Destroy bars
            Bar[] bars = new Bar[m.Bars.Count];
            m.Bars.CopyTo(bars);
            foreach (Bar bar in bars)
                dh.DestroyComponent(bar);

            if (m.TopDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.TopDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.TopDockSite);
            }
            if (m.BottomDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.BottomDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.BottomDockSite);
            }
            if (m.LeftDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.LeftDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.LeftDockSite);
            }
            if (m.RightDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.RightDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.RightDockSite);
            }

            if (m.FillDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.FillDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.FillDockSite);
            }

            if (m.ToolbarTopDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.ToolbarTopDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.ToolbarTopDockSite);
            }
            if (m.ToolbarBottomDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.ToolbarBottomDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.ToolbarBottomDockSite);
            }
            if (m.ToolbarLeftDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.ToolbarLeftDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.ToolbarLeftDockSite);
            }
            if (m.ToolbarRightDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.ToolbarRightDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.ToolbarRightDockSite);
            }

			if(m.DefinitionName!=null)
			{
				//DesignTimeDte.DeleteFromProject(m.DefinitionName, this.Component.Site);
				m.DefinitionName="";
			}

            //// Remove Command links if any
            //if (m.CommandLinks.Count > 0)
            //{
            //    CommandLink[] links = new CommandLink[m.CommandLinks.Count];
            //    m.CommandLinks.CopyTo(links);
            //    foreach (CommandLink link in links)
            //    {
            //        dh.DestroyComponent(link);
            //    }
            //}
		}

//		protected override void PreFilterProperties(System.Collections.IDictionary properties) 
//		{
//			// Always call base first in PreFilter* methods, and last in PostFilter*
//			// methods.
//			base.PreFilterProperties(properties);
//
//			properties["ProvidePopupsOnly"] = TypeDescriptor.CreateProperty(
//				this.GetType(),		// the type this property is defined on
//				"ProvidePopupsOnly",	// the name of the property
//				typeof(bool),		// the type of the property
//				new Attribute[] {CategoryAttribute.Design,System.ComponentModel.DesignerSerializationVisibilityAttribute.Hidden,new DescriptionAttribute("When set to True DotNetBar provides Popups only without Toolbars or Menu Bars. Use on child forms to provide Context Menus.")});	// attributes
//
//            //properties["Style"] = TypeDescriptor.CreateProperty(
//            //    this.GetType(),		// the type this property is defined on
//            //    "Style",	// the name of the property
//            //    typeof(eDotNetBarStyle),		// the type of the property
//            //    new Attribute[0]);
//
//            //properties["ThemeAware"] = TypeDescriptor.CreateProperty(
//            //    this.GetType(),		// the type this property is defined on
//            //    "ThemeAware",	// the name of the property
//            //    typeof(bool),		// the type of the property
//            //    new Attribute[0]);
//		}
//
//		// Popup Provider is design-time only property that determines whether current DotNetBar provides
//		// Popup Context Menu only or is full blown Toolbar/Menu bar component
//		[DefaultValue(false)]
//		protected bool ProvidePopupsOnly
//		{
//			get 
//			{
//				return m_PopupProvider;
//			}
//			set 
//			{
//				if(m_PopupProvider==value)
//					return;
//				
//				m_PopupProvider=value;
//
//				DotNetBarManager m=this.Component as DotNetBarManager;
//				IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
//
//				if(m_PopupProvider)
//				{
//					RemoveDockSites();
//					if(m!=null && dh!=null && dh.RootComponent is System.Windows.Forms.UserControl)
//					{
//						m.ParentUserControl=dh.RootComponent as System.Windows.Forms.UserControl;
//						IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
//						cc.OnComponentChanged(m,(TypeDescriptor.GetProperties(m)["ParentUserControl"]),null,null);
//					}
//				}
//				else
//				{
//					SetupDockSites();
//					if(m!=null)
//					{
//						m.ParentUserControl=null;
//						IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
//						cc.OnComponentChanged(m,(TypeDescriptor.GetProperties(m)["ParentUserControl"]),null,null);
//					}
//				}
//			}
//		}
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
            if (parent == null) return;
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

            if (m.TopDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.TopDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.TopDockSite);
            }
            if (m.BottomDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.BottomDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.BottomDockSite);
            }
            if (m.LeftDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.LeftDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.LeftDockSite);
            }
            if (m.RightDockSite != null)
            {
                DockSiteDesigner designer = dh.GetDesigner(m.RightDockSite) as DockSiteDesigner;
                if (designer != null) designer.DotNetBarManagerRemoving = true;
                dh.DestroyComponent(m.RightDockSite);
            }

			m.LeftDockSite=null;
			m.TopDockSite=null;
			m.BottomDockSite=null;
			m.RightDockSite=null;
		}

        private void SetupToolbarDockSites()
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null) return;

            DotNetBarManager m = this.Component as DotNetBarManager;
            System.Windows.Forms.Control parent = dh.RootComponent as System.Windows.Forms.Control;
            if (m == null || parent == null) return;

            DockSite ds = null;

            // Toolbar Dock Sites
            // Left
            if (m.ToolbarLeftDockSite == null)
            {
                ds = (DockSite)dh.CreateComponent(typeof(DockSite));
                parent.Controls.Add(ds);
                TypeDescriptor.GetProperties(m)["ToolbarLeftDockSite"].SetValue(m, ds);
                ds.Dock = System.Windows.Forms.DockStyle.Left;
            }

            // Toolbar Right Dock Site
            if (m.ToolbarRightDockSite == null)
            {
                ds = (DockSite)dh.CreateComponent(typeof(DockSite));
                parent.Controls.Add(ds);
                TypeDescriptor.GetProperties(m)["ToolbarRightDockSite"].SetValue(m, ds);
                ds.Dock = System.Windows.Forms.DockStyle.Right;
            }

            // Toolbar Top Dock Site
            if (m.ToolbarTopDockSite == null)
            {
                ds = (DockSite)dh.CreateComponent(typeof(DockSite));
                parent.Controls.Add(ds);
                TypeDescriptor.GetProperties(m)["ToolbarTopDockSite"].SetValue(m, ds);
                ds.Dock = System.Windows.Forms.DockStyle.Top;
            }

            // Toolbar Bottom Dock Site
            if (m.ToolbarBottomDockSite == null)
            {
                ds = (DockSite)dh.CreateComponent(typeof(DockSite));
                parent.Controls.Add(ds);
                TypeDescriptor.GetProperties(m)["ToolbarBottomDockSite"].SetValue(m, ds);
                ds.Dock = System.Windows.Forms.DockStyle.Bottom;
            }
        }

        private void SetupDockSites()
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null) return;

            if (dh.Loading)
            {
                dh.LoadComplete += new EventHandler(this.LoadComplete);
                return;
            }

            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // Set default properties for this control and add default dependent controls
            DotNetBarManager m = this.Component as DotNetBarManager;
            System.Windows.Forms.Control parent = dh.RootComponent as System.Windows.Forms.Control;
            if (m == null || parent == null)
                return;

            if (parent is System.Windows.Forms.Form)
                m.ParentForm = (System.Windows.Forms.Form)parent;
            else if (parent is System.Windows.Forms.UserControl)
                m.ParentUserControl = parent;

            // Dockable Windows Dock Sites
            // Left Dock Site
            DockSite ds = (DockSite)dh.CreateComponent(typeof(DockSite));
            parent.Controls.Add(ds);
            m.LeftDockSite = ds;
            ds.Dock = System.Windows.Forms.DockStyle.Left;
            ds.DocumentDockContainer = new DocumentDockContainer();
            ds.BringToFront();

            // Right Dock Site
            ds = (DockSite)dh.CreateComponent(typeof(DockSite));
            parent.Controls.Add(ds);
            m.RightDockSite = ds;
            ds.Dock = System.Windows.Forms.DockStyle.Right;
            ds.DocumentDockContainer = new DocumentDockContainer();
            ds.BringToFront();

            // Top Dock Site
            ds = (DockSite)dh.CreateComponent(typeof(DockSite));
            parent.Controls.Add(ds);
            m.TopDockSite = ds;
            ds.Dock = System.Windows.Forms.DockStyle.Top;
            ds.DocumentDockContainer = new DocumentDockContainer();

            // Bottom Dock Site
            ds = (DockSite)dh.CreateComponent(typeof(DockSite));
            parent.Controls.Add(ds);
            m.BottomDockSite = ds;
            ds.Dock = System.Windows.Forms.DockStyle.Bottom;
            ds.DocumentDockContainer = new DocumentDockContainer();

            if (cc != null)
            {
                cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent).Find("Controls", true));
                cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent).Find("Controls", true), null, null);
            }
        }

        //private void SetupDockSites()
        //{
        //    IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
        //    if(dh==null)
        //    {
        //        return;
        //    }

        //    if(dh.Loading)
        //    {
        //        dh.LoadComplete+=new EventHandler(this.LoadComplete);
        //        return;
        //    }

        //    IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));

        //    // Set default properties for this control and add default dependent controls
        //    DotNetBarManager m=this.Component as DotNetBarManager;
        //    System.Windows.Forms.Form form=dh.RootComponent as System.Windows.Forms.Form;
        //    System.Windows.Forms.UserControl parentControl=dh.RootComponent as System.Windows.Forms.UserControl;
        //    if(m==null || (form==null && parentControl==null))
        //    {
        //        return;
        //    }

        //    if(form!=null)
        //    {
        //        try
        //        {
        //            m.ParentForm=form;

        //            // Left Dock Site
        //            DockSite ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barLeftDockSite");
        //            form.Controls.Add(ds);
        //            m.LeftDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Left;

        //            // Right Dock Site
        //            ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barRightDockSite");
        //            form.Controls.Add(ds);
        //            m.RightDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Right;

        //            // Top Dock Site
        //            ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barTopDockSite");
        //            form.Controls.Add(ds);
        //            m.TopDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Top;
        //            // Bottom Dock Site
        //            ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barBottomDockSite");
        //            form.Controls.Add(ds);
        //            m.BottomDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Bottom;

        //            if(cc!=null)
        //            {
        //                cc.OnComponentChanging(form,TypeDescriptor.GetProperties(form).Find("Controls",true));
        //                cc.OnComponentChanged(form,TypeDescriptor.GetProperties(form).Find("Controls",true),null,null);
        //            }
        //        }
        //        catch(Exception e)
        //        {
        //            // DEBUG
        //            System.Windows.Forms.MessageBox.Show("Error setting up dock sites: " + e.ToString());
        //        }
        //    }
        //    else if(parentControl!=null)
        //    {
        //        try
        //        {
        //            // Left Dock Site
        //            DockSite ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barLeftDockSite");
        //            parentControl.Controls.Add(ds);
        //            m.LeftDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Left;

        //            // Right Dock Site
        //            ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barRightDockSite");
        //            parentControl.Controls.Add(ds);
        //            m.RightDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Right;

        //            // Top Dock Site
        //            ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barTopDockSite");
        //            parentControl.Controls.Add(ds);
        //            m.TopDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Top;
        //            // Bottom Dock Site
        //            ds=(DockSite)dh.CreateComponent(typeof(DockSite),"barBottomDockSite");
        //            parentControl.Controls.Add(ds);
        //            m.BottomDockSite=ds;
        //            ds.Dock=System.Windows.Forms.DockStyle.Bottom;

        //            if(cc!=null)
        //            {
        //                cc.OnComponentChanging(parentControl,TypeDescriptor.GetProperties(parentControl).Find("Controls",true));
        //                cc.OnComponentChanged(parentControl,TypeDescriptor.GetProperties(parentControl).Find("Controls",true),null,null);
        //            }
        //        }
        //        catch(Exception e)
        //        {
        //            // DEBUG
        //            System.Windows.Forms.MessageBox.Show("Error setting up dock sites on UserControl: " + e.ToString());
        //        }
        //    }
        //}

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
		
        //public static bool OpenDesignerEditor(DotNetBarManager manager, IDesignerServices designerServices)
        //{
        //    DevComponents.DotNetBar.Design.DotNetBarDesigner designer=new DevComponents.DotNetBar.Design.DotNetBarDesigner();

        //    if (designerServices != null)
        //        designer.DesignerServices = designerServices;

        //    if(manager!=null)
        //        designer.ExternalManager=manager;

        //    System.Windows.Forms.DialogResult dr=designer.ShowDialog();

        //    if(dr==System.Windows.Forms.DialogResult.Yes)
        //        return true;

        //    return false;
        //}

		/// <summary>
		/// Sets the style of all items in DotNetBar Manager.
		/// </summary>
        //public eDotNetBarStyle Style
        //{
        //    set
        //    {
        //        DotNetBarManager manager=this.Component as DotNetBarManager;
        //        if(manager.Style!=value)
        //        {
        //            manager.Style=value;
        //            IDesignerHost dh=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
        //            if(dh!=null && !dh.Loading)
        //                manager.SaveDesignDefinition();
        //        }
        //    }
        //    get
        //    {
        //        DotNetBarManager manager=this.Component as DotNetBarManager;
        //        return manager.Style;
        //    }
        //}

        ///// <summary>
        ///// Specifies whether new bars created are drawn using Themes when running on OS that supports themes like Windows XP. Note this setting
        ///// applies to new bars created.
        ///// </summary>
        //public bool ThemeAware
        //{
        //    set
        //    {
        //        DotNetBarManager manager=this.Component as DotNetBarManager;
        //        if(manager.ThemeAware!=value)
        //        {
        //            manager.ThemeAware=value;
        //            IDesignerHost dh=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
        //            if(dh!=null && !dh.Loading)
        //                manager.SaveDesignDefinition();
        //        }
        //    }
        //    get
        //    {
        //        DotNetBarManager manager=this.Component as DotNetBarManager;
        //        return manager.ThemeAware;
        //    }
        //}
        #endregion

        #region Dock Bars Creation
        private void CreateDockBarLeft(object sender, EventArgs e)
        {
            DotNetBarManager d = this.Component as DotNetBarManager;
            CreateDockBar(d.LeftDockSite);
        }

        private void CreateDockBarRight(object sender, EventArgs e)
        {
            DotNetBarManager d = this.Component as DotNetBarManager;
            CreateDockBar(d.RightDockSite);
        }

        private void CreateDockBarTop(object sender, EventArgs e)
        {
            DotNetBarManager d = this.Component as DotNetBarManager;
            CreateDockBar(d.TopDockSite);
        }

        private void CreateDockBarBottom(object sender, EventArgs e)
        {
            DotNetBarManager d = this.Component as DotNetBarManager;
            CreateDockBar(d.BottomDockSite);
        }

        private void CreateDockBar(DockSite parentSite)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh == null) return;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            DotNetBarManager dm = this.Component as DotNetBarManager;

            DesignerTransaction dt = dh.CreateTransaction("Creating Dock Bar");
            try
            {
                // Make sure that DockSite has document manager
                if (parentSite.DocumentDockContainer == null)
                    TypeDescriptor.GetProperties(parentSite)["DocumentDockContainer"].SetValue(parentSite, new DocumentDockContainer());

                Bar bar = dh.CreateComponent(typeof(Bar)) as Bar;
                bar.AutoSyncBarCaption = true;
                bar.CloseSingleTab = true;
                bar.CanDockDocument = false;
                bar.Style = dm.Style;
                bar.LayoutType = eLayoutType.DockContainer;
                bar.GrabHandleStyle = eGrabHandleStyle.Caption;
                bar.Stretch = true;

                // Add Dock Container to it...
                DockContainerItem dockItem = dh.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
                dockItem.Text = dockItem.Name;
                bar.Items.Add(dockItem);

                // Panel for DockContainerItem
                PanelDockContainer panel = dh.CreateComponent(typeof(PanelDockContainer)) as PanelDockContainer;
                bar.Controls.Add(panel);
                panel.ColorSchemeStyle = bar.Style;
                panel.ApplyLabelStyle();
                dockItem.Control = panel;
                bar.SelectedDockTab = 0;

                // Add them to the dock manager
                cc.OnComponentChanging(parentSite, null);

                parentSite.GetDocumentUIManager().Dock(bar);

                parentSite.RecalcLayout();

                cc.OnComponentChanged(parentSite, null, null, null);
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if (!dt.Canceled)
                    dt.Commit();
            }
        }
        #endregion

        #region Menu bar, Toolbar Creation
        private void CreateMenuBar(object sender, EventArgs e)
        {
            CreateToolbar(true);
        }

        private void CreateToolbar(object sender, EventArgs e)
        {
            CreateToolbar(false);
        }

        private void CreateToolbar(bool menuBar)
        {
            DotNetBarManager m = this.Component as DotNetBarManager;
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            if (dh == null || m == null) return;

            DesignerTransaction dt = dh.CreateTransaction("Creating Bar Control");
            try
            {
                // Make sure dock sites exists...
                if (m.ToolbarTopDockSite == null)
                    SetupToolbarDockSites();

                Bar bar = dh.CreateComponent(typeof(Bar)) as Bar;
                bar.LayoutType = eLayoutType.Toolbar;
                bar.Text = bar.Name;

                ButtonItem item = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
                item.Text = item.Name;
                bar.Items.Add(item);

                if (menuBar)
                {
                    bar.Stretch = true;
                    bar.MenuBar = true;
                }
                else
                    bar.GrabHandleStyle = eGrabHandleStyle.Office2003;

                cc.OnComponentChanging(m.ToolbarTopDockSite, null);
                m.Bars.Add(bar);
                if (menuBar)
                    bar.DockLine = -1;
                bar.DockSide = eDockSide.Top;
                bar.RecalcLayout();
                cc.OnComponentChanged(m.ToolbarTopDockSite, null, null, null);
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if(!dt.Canceled)
                    dt.Commit();
            }

        }
        #endregion

        #region Old Definition Import
        private void ImportDefinition(DotNetBarManager manager)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (manager.DefinitionName == "" || dh==null) return;

            XmlDocument def = LoadDefinition(manager);
            if (def == null) return;

            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            if (manager.ToolbarTopDockSite == null)
                SetupToolbarDockSites();

            manager.SuspendLayout = true;

            try
            {
                if(manager.TopDockSite!=null && manager.TopDockSite.DocumentDockContainer == null)
                    manager.TopDockSite.DocumentDockContainer = new DocumentDockContainer();
                if (manager.BottomDockSite != null && manager.BottomDockSite.DocumentDockContainer == null)
                    manager.BottomDockSite.DocumentDockContainer = new DocumentDockContainer();
                if (manager.LeftDockSite != null && manager.LeftDockSite.DocumentDockContainer == null)
                    manager.LeftDockSite.DocumentDockContainer = new DocumentDockContainer();
                if (manager.RightDockSite != null && manager.RightDockSite.DocumentDockContainer == null)
                    manager.RightDockSite.DocumentDockContainer = new DocumentDockContainer();

                System.Xml.XmlElement xmlDotNetBar = def.FirstChild as System.Xml.XmlElement;

                // Creates serialization context
                ItemSerializationContext context = new ItemSerializationContext();
                context.Serializer = manager;
                context.HasDeserializeItemHandlers = false;
                context.HasSerializeItemHandlers = false;
                context._DesignerHost = dh;

                // Load bars first 
                foreach (System.Xml.XmlElement xmlElem in xmlDotNetBar.ChildNodes)
                {
                    if (xmlElem.Name == "bars")
                    {
                        foreach (System.Xml.XmlElement xmlBar in xmlElem.ChildNodes)
                        {
                            Bar bar = null;
                            string name = xmlBar.GetAttribute("name");
                            try
                            {
                                bar = dh.CreateComponent(typeof(Bar), name) as Bar;
                            }
                            catch { }
                            if(bar==null)
                                bar = dh.CreateComponent(typeof(Bar)) as Bar;

                            manager.Bars.Add(bar);

                            bar.Deserialize(xmlBar, context);

                            if (bar.LayoutType == eLayoutType.DockContainer)
                            {
                                // Create dock panels for the bar
                                foreach (BaseItem item in bar.Items)
                                {
                                    if (!(item is DockContainerItem)) continue;
                                    DockContainerItem dc = item as DockContainerItem;
                                    PanelDockContainer panel = dh.CreateComponent(typeof(PanelDockContainer)) as PanelDockContainer;
                                    bar.Controls.Add(panel);
                                    panel.ColorSchemeStyle = bar.Style;
                                    panel.ApplyLabelStyle();
                                    dc.Control = panel;
                                }
                            }
                        }
                    }
                }

                // Context menus loading
                foreach (System.Xml.XmlElement xmlElem in xmlDotNetBar.ChildNodes)
                {
                    if (xmlElem.Name == "popups")
                    {
                        ContextMenuBar contextBar = null;
                        Control parent = manager.ParentForm;
                        if (parent == null) parent = manager.ParentUserControl;
                        if (parent == null)
                            parent = dh.RootComponent as System.Windows.Forms.Control;

                        if (parent != null)
                        {
                            if (xmlElem.ChildNodes.Count > 0)
                            {
                                contextBar = dh.CreateComponent(typeof(ContextMenuBar)) as ContextMenuBar;
                                parent.Controls.Add(contextBar);
                                contextBar.BringToFront();
                                contextBar.Style = manager.Style;
                                contextBar.Size = new Size(150, 26);
                            }

                            foreach (System.Xml.XmlElement xmlItem in xmlElem.ChildNodes)
                            {
                                BaseItem objItem = context.CreateItemFromXml(xmlItem);
                                if (objItem == null)
                                    MessageBox.Show("Invalid Item in file found (" + Helpers.GetItemErrorInfo(xmlItem) + ").");
                                // Do not change this order. Item should be Deserialized first and then added to the items
                                // collection. See GlobalItem property for performace reasons...
                                context.ItemXmlElement = xmlItem;
                                objItem.Deserialize(context);
                                if (!objItem.Visible) objItem.Visible = true;
                                if (objItem.Text == "" || objItem.Text == null) objItem.Text = objItem.Name;
                                if (objItem is ButtonItem) ((ButtonItem)objItem).AutoExpandOnClick = true;
                                contextBar.Items.Add(objItem);
                            }
                            if (contextBar != null)
                                contextBar.RecalcLayout();
                            if (cc != null && contextBar != null)
                            {
                                cc.OnComponentChanged(contextBar, TypeDescriptor.GetProperties(contextBar)["Items"], null, null);
                                cc.OnComponentChanged(contextBar, null, null, null);
                            }
                        }
                    }
                }

                if (cc != null)
                {
                    cc.OnComponentChanged(manager, null, null, null);
                    if (manager.TopDockSite != null)
                        cc.OnComponentChanged(manager.TopDockSite, null, null, null);
                    if (manager.BottomDockSite != null)
                        cc.OnComponentChanged(manager.BottomDockSite, null, null, null);
                    if (manager.LeftDockSite != null)
                        cc.OnComponentChanged(manager.LeftDockSite, null, null, null);
                    if (manager.RightDockSite != null)
                        cc.OnComponentChanged(manager.RightDockSite, null, null, null);

                    if (manager.ToolbarTopDockSite != null)
                        cc.OnComponentChanged(manager.ToolbarTopDockSite, null, null, null);
                    if (manager.BottomDockSite != null)
                        cc.OnComponentChanged(manager.ToolbarBottomDockSite, null, null, null);
                    if (manager.ToolbarLeftDockSite != null)
                        cc.OnComponentChanged(manager.ToolbarLeftDockSite, null, null, null);
                    if (manager.ToolbarRightDockSite != null)
                        cc.OnComponentChanged(manager.ToolbarRightDockSite, null, null, null);
                }
            }
            finally
            {
                manager.SuspendLayout = false;
            }

            foreach (Bar bar in manager.Bars)
            {
                if (bar.LayoutType != eLayoutType.DockContainer)
                    continue;

                DockContainerItem dc = bar.SelectedDockContainerItem;
                if (dc != null && dc.Control != null)
                {
                    if (bar.Controls.IndexOf(dc.Control) > 0)
                    {
                        if (cc != null)
                            cc.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["Controls"]);
                        bar.Controls.SetChildIndex(dc.Control, 0);
                        if (cc != null)
                            cc.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["Controls"], null, null);
                    }
                }
            }

            //// Load items for customization i.e. categories etc.
            //foreach (System.Xml.XmlElement xmlElem in xmlDotNetBar.ChildNodes)
            //{
            //    if (xmlElem.Name == "items")
            //    {
            //        foreach (System.Xml.XmlElement xmlItem in xmlElem.ChildNodes)
            //        {
            //            BaseItem objItem = BarFunctions.CreateItemFromXml(xmlItem, dh);
            //            if (objItem == null)
            //                MessageBox.Show("Invalid Item in file found (" + BarFunctions.GetItemErrorInfo(xmlItem) + ").");
            //            // Do not change this order. Item should be Deserialized first and then added to the items
            //            // collection. See GlobalItem property for performace reasons...
            //            context.ItemXmlElement = xmlItem;
            //            objItem.Deserialize(context);
            //            manager.Items.Add(objItem);
            //        }
            //    }
            //}
        }

        private XmlDocument LoadDefinition(DotNetBarManager manager)
        {
            string path = DesignTimeDte.GetDefinitionPath(manager.DefinitionName, manager.Site);
            if (path != null && path != "")
            {
                if (!path.EndsWith("\\"))
                    path += "\\";
                if (System.IO.File.Exists(path + manager.DefinitionName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + manager.DefinitionName);
                    return doc;
                }
                else
                {
                    MessageBox.Show("Could not import. DotNetBar definition file not found: " + path + manager.DefinitionName, "Import Routine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("Could not find DotNetBarManager path. Import aborted.", "Import Routine", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return null;
        }

        #endregion
    }
}
