using System;
using System.Text;
using System.Windows.Forms.Design;
using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents VS.NET designer for the CrumbBar control.
    /// </summary>
    public class CrumbBarDesigner : ControlDesigner
    {
        #region Internal Implementation
        /// <summary>Initializes designer with given component.</summary>
        /// <param name="component">Component to initialize designer with.</param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (!component.Site.DesignMode)
                return;

            // If our component is removed we need to clean-up
            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (cc != null)
            {
                cc.ComponentRemoving += new ComponentEventHandler(this.OnComponentRemoving);
            }

#if !TRIAL
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete += new EventHandler(dh_LoadComplete);
#endif
        }

        protected override void Dispose(bool disposing)
        {
            // If our component is removed we need to clean-up
            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (cc != null)
                cc.ComponentRemoving -= new ComponentEventHandler(this.OnComponentRemoving);
            base.Dispose(disposing);
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
            SetVistaBackgroundStyle();
            CrumbBar bar = this.Control as CrumbBar;
#if (FRAMEWORK20)
            bar.AutoSize = true;
#endif
#if !TRIAL
            string key = RibbonBarDesigner.GetLicenseKey();
            bar.LicenseKey = key;
#endif
        }
#if !TRIAL
        private void dh_LoadComplete(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete -= new EventHandler(dh_LoadComplete);

            string key = RibbonBarDesigner.GetLicenseKey();
            CrumbBar bar = this.Control as CrumbBar;
            if (key != "" && bar != null && bar.LicenseKey == "" && bar.LicenseKey != key)
                TypeDescriptor.GetProperties(bar)["LicenseKey"].SetValue(bar, key);
        }
#endif

        private void SetVistaBackgroundStyle()
        {
            CrumbBar cb = this.Control as CrumbBar;

            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (cc != null)
                cc.OnComponentChanging(this, TypeDescriptor.GetProperties(cb)["BackgroundStyle"]);

            cb.BackgroundStyle.Reset();
            cb.BackgroundStyle.Border = eStyleBorderType.Solid;
            cb.BackgroundStyle.BorderWidth = 1;
            cb.BackgroundStyle.BorderColor = ColorScheme.GetColor("53595E");
            cb.BackgroundStyle.BorderColor2 = ColorScheme.GetColor("A9B4BF");
            cb.BackgroundStyle.BackColor = ColorScheme.GetColor("F8FAFD");
            if (cc != null)
                cc.OnComponentChanged(this, TypeDescriptor.GetProperties(cb)["BackgroundStyle"], null, null);

        }

        private void SetOffice2007BackgroundStyle()
        {
            CrumbBar cb = this.Control as CrumbBar;

            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (cc != null)
                cc.OnComponentChanging(this, TypeDescriptor.GetProperties(cb)["BackgroundStyle"]);

            cb.BackgroundStyle.Reset();
            cb.BackgroundStyle.Class = ElementStyleClassKeys.CrumbBarBackgroundKey;

            if (cc != null)
                cc.OnComponentChanged(this, TypeDescriptor.GetProperties(cb)["BackgroundStyle"], null, null);
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["Style"] = TypeDescriptor.CreateProperty(typeof(CrumbBarDesigner), (PropertyDescriptor)properties["Style"], new Attribute[]
				{
					new DefaultValueAttribute(eCrumbBarStyle.Vista),
					new BrowsableAttribute(true),
					new CategoryAttribute("Appearance")});

        }

        /// <summary>
        /// Gets or sets the visual style of the control. Default value is Windows Vista style.
        /// </summary>
        [DefaultValue(eCrumbBarStyle.Vista), Category("Appearance"), Description("Indicates visual style of the control.")]
        public eCrumbBarStyle Style
        {
            get { return ((CrumbBar)this.Control).Style; }
            set
            {
                CrumbBar b = this.Control as CrumbBar;
                bool isChanged = (b.Style != value);
                b.Style = value;
                if (isChanged)
                {
                    IDesignerHost ds = GetService(typeof(IDesignerHost)) as IDesignerHost;
                    if (ds != null && !ds.Loading)
                    {
                        if (value == eCrumbBarStyle.Vista)
                            SetVistaBackgroundStyle();
                        else if (value == eCrumbBarStyle.Office2007)
                            SetOffice2007BackgroundStyle();
                    }
                }
            }
        }

        /// <summary>Returns design-time commands applicable to this designer.</summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = null;
#if (FRAMEWORK20)
                verbs = new DesignerVerb[]
					{
                        new DesignerVerb("Edit Items...", new EventHandler(EditItems))
				};
#else
                verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Item", new EventHandler(CreateItem))
				};
#endif
                return new DesignerVerbCollection(verbs);
            }
        }

        private void EditItems(object sender, EventArgs e)
        {
            EditItems();
        }

        internal void EditItems()
        {
            CrumbBar crumbBar = this.Component as CrumbBar;
#if (FRAMEWORK20)
            Form form = new Form();
            form.Text = "CrumbBar control editor";
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.MinimizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
            CrumbBarItemsEditor editor = new CrumbBarItemsEditor();
            editor.Dock = DockStyle.Fill;
            form.Size = new System.Drawing.Size(800, 600);
            form.Controls.Add(editor);

            editor.CrumbBar = crumbBar;
            editor.Designer = this;
            editor.UpdateDisplay();
            form.ShowDialog();
            form.Dispose();
#else
            DevComponents.AdvTree.Design.AdvTreeDesigner.EditValue(this, crumbBar, "Items");
#endif
        }

        private void CreateItem(object sender, EventArgs e)
        {
            CreateItem();
        }

        internal void CreateItem()
        {
            CrumbBarItem node = CreateItem(this.Component as CrumbBar);
            if (node != null)
            {
                ISelectionService sel = this.GetService(typeof(ISelectionService)) as ISelectionService;
                ArrayList list = new ArrayList(1);
                list.Add(node);
                if (sel != null)
                {
                    sel.SetSelectedComponents(list, SelectionTypes.MouseDown);
                    CrumbBar crumbBar = this.Component as CrumbBar;
                    if (crumbBar != null && crumbBar.SelectedItem == null)
                        crumbBar.SelectedItem = node;
                }
            }
        }

        public object GetDesignService(Type serviceType)
        {
            return GetService(serviceType);
        }

        private CrumbBarItem CreateItem(CrumbBar crumbBar)
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return null;

            CrumbBarItem node = null;
            try
            {
                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                {
                    change.OnComponentChanging(this.Component, TypeDescriptor.GetProperties(crumbBar).Find("Items", true));
                }

                node = dh.CreateComponent(typeof(CrumbBarItem)) as CrumbBarItem;
                if (node != null)
                {
                    node.Text = node.Name;
                    crumbBar.Items.Add(node);

                    if (change != null)
                        change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(crumbBar).Find("Items", true), null, null);
                }
            }
            finally
            {
                crumbBar.RecalcLayout();
            }

            return node;
        }

        /// <summary>Called when component is about to be removed from designer.</summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        public void OnComponentRemoving(object sender, ComponentEventArgs e)
        {
            if (e.Component == this.Component)
            {
                IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
                if (dh == null)
                    return;

                ArrayList list = new ArrayList(this.AssociatedComponents);
                foreach (IComponent c in list)
                    dh.DestroyComponent(c);
            }
            else if (e.Component is CrumbBarItem && ((CrumbBarItem)e.Component).GetOwner() == this.Control)
            {
                OnNodeRemoving(e.Component as CrumbBarItem);
            }
        }

        private void OnNodeRemoving(CrumbBarItem node)
        {
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            
            // Root node
            CrumbBar tree = this.Control as CrumbBar;
            bool wasSelected = tree.GetIsInSelectedPath(node);

            if (node.Parent != null)
            {
                CrumbBarItem parent = node.Parent as CrumbBarItem;
                if (parent != null)
                {
                    if (cc != null)
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
                    parent.SubItems.Remove(node);
                    if (cc != null)
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
                }
            }
            else
            {
                if (cc != null)
                    cc.OnComponentChanging(tree, TypeDescriptor.GetProperties(tree)["Items"]);
                tree.Items.Remove(node);
                if (cc != null)
                    cc.OnComponentChanged(tree, TypeDescriptor.GetProperties(tree)["Items"], null, null);
            }

            if (node.SubItems.Count > 0)
            {
                BaseItem[] nodes = new BaseItem[node.SubItems.Count];
                node.SubItems.CopyTo(nodes, 0);
                foreach (BaseItem n in nodes)
                {
                    node.SubItems.Remove(n);

                    if (dh != null)
                        dh.DestroyComponent(n);
                }
            }

            if (wasSelected)
            {
                tree.SelectedItem = tree.GetFirstVisibleItem();
            }

            this.RecalcLayout();
        }

        private void RecalcLayout()
        {
            CrumbBar tree = this.Control as CrumbBar;
            if (tree != null) tree.RecalcLayout();
        }

        /// <summary>
        /// Returns all components associated with this control
        /// </summary>
        public override ICollection AssociatedComponents
        {
            get
            {
                ArrayList c = new ArrayList(base.AssociatedComponents);
                CrumbBar tree = this.Control as CrumbBar;
                if (tree != null)
                {
                    foreach (BaseItem node in tree.Items)
                        GetItemsRecursive(node, c);
                }
                return c;
            }
        }

        private void GetItemsRecursive(BaseItem parent, ArrayList c)
        {
            c.Add(parent);
            foreach (BaseItem node in parent.SubItems)
            {
                c.Add(node);
                GetItemsRecursive(node, c);
            }
        }
        #endregion
    }
}
