#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Collections;
using DevComponents.DotNetBar.Controls;
using DevComponents.AdvTree;
using System.ComponentModel;
using System.ComponentModel.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design
{
    public class ComboTreeDesigner : ControlDesigner
    {
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (component.Site!=null && !component.Site.DesignMode)
                return;

            // If our component is removed we need to clean-up
            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (cc != null)
            {
                cc.ComponentRemoving += new ComponentEventHandler(this.OnComponentRemoved);
            }

#if !TRIAL
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete += new EventHandler(dh_LoadComplete);
#endif
        }
#if !TRIAL
        private void dh_LoadComplete(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete -= new EventHandler(dh_LoadComplete);

            string key = RibbonBarDesigner.GetLicenseKey();
            ComboTree bar = this.Control as ComboTree;
            if (key != "" && bar != null && bar.LicenseKey == "" && bar.LicenseKey != key)
                TypeDescriptor.GetProperties(bar)["LicenseKey"].SetValue(bar, key);
        }
#endif

        protected override void Dispose(bool disposing)
        {
            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (cc != null)
                cc.ComponentRemoved -= new ComponentEventHandler(this.OnComponentRemoved);

            base.Dispose(disposing);
        }

        private void SetDesignTimeDefaults()
        {
            ComboTree c = this.Control as ComboTree;
            PropertyDescriptor d = TypeDescriptor.GetProperties(c)["Text"];
            if (d != null && d.PropertyType == typeof(string) && !d.IsReadOnly && d.IsBrowsable)
            {
                d.SetValue(c, "");
            }
            c.ButtonDropDown.Visible = true;
            c.BackgroundStyle.Class = ElementStyleClassKeys.TextBoxBorderKey;
            c.Style = eDotNetBarStyle.StyleManagerControlled;
#if !TRIAL
            string key = RibbonBarDesigner.GetLicenseKey();
            c.LicenseKey = key;
#endif
        }

		public override ICollection AssociatedComponents
		{
			get
			{
				ArrayList al = new ArrayList(base.AssociatedComponents);
                ComboTree combo = this.Component as ComboTree;
				foreach(Node o in combo.Nodes)
				{
                    al.Add(o);
                    AddChildNodes(o, al);
				}
				return al;
			}
		}

        private void AddChildNodes(Node parent, ArrayList al)
        {
            foreach (Node item in parent.Nodes)
            {
                al.Add(item);
            }
        }

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			if (e.Component == this.Component)
			{
                ICollection nodes = AssociatedComponents;
				IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (dh == null) return;
				foreach (object item in nodes)
				{
					if (item is Node)
					{
						Node ci = item as Node;
						dh.DestroyComponent(ci);
					}
				}
			}
		}

        internal void EditColumns()
        {
            ComboTree tree = this.Component as ComboTree;
            DevComponents.AdvTree.Design.AdvTreeDesigner.EditValue(this, tree, "Columns");
        }

        private DesignerActionListCollection _ActionLists = null;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (this._ActionLists == null)
                {
                    this._ActionLists = new DesignerActionListCollection();
                    this._ActionLists.Add(new ComboTreeActionList(this));
                }
                return this._ActionLists;
            }
        }

        internal void CreateNode()
        {
            Node node = CreateNode(this.Component as ComboTree);
            //if (node != null)
            //{
            //    ISelectionService sel = this.GetService(typeof(ISelectionService)) as ISelectionService;
            //    ArrayList list = new ArrayList(1);
            //    list.Add(node);
            //    if (sel != null)
            //    {
            //        sel.SetSelectedComponents(list, SelectionTypes.MouseDown);
            //        node.TreeControl.SelectedNode = node;
            //    }
            //}
        }

        private Node CreateNode(ComboTree tree)
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return null;

            Node node = null;
            tree.AdvTree.BeginUpdate();
            try
            {
                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                {
                    change.OnComponentChanging(this.Component, TypeDescriptor.GetProperties(tree).Find("Nodes", true));
                }

                node = dh.CreateComponent(typeof(Node)) as Node;
                if (node != null)
                {
                    node.Text = node.Name;
                    node.Expanded = true;
                    tree.Nodes.Add(node);

                    if (change != null)
                        change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(tree).Find("Nodes", true), null, null);
                }
            }
            finally
            {
                tree.AdvTree.EndUpdate();
            }

            return node;
        }
    }
}
#endif