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
	/// Summary description for TabControlDesigner.
	/// </summary>
	public class TabControlDesigner:TabStripDesigner
	{
		public TabControlDesigner()
		{
            this.TabsVisible = true;
		}

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (!component.Site.DesignMode)
                return;

            TabControl tc = component as TabControl;
            if (this.Inherited)
            {
                if(!tc.TabsVisible)
                    tc.TabsVisible = true;
                if (tc.SelectedPanel != null)
                    tc.SelectedPanel.BringToFront();
            }
        }

		protected override void CreateNewTab(object sender, EventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			TabControl tabControl=this.Control as TabControl;
			if(tabControl==null || dh==null)
				return;

            DesignerTransaction dt = dh.CreateTransaction();
            try
            {
                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                    change.OnComponentChanging(this.Component, null);

                TabItem tab = dh.CreateComponent(typeof(TabItem)) as TabItem;
                tab.Text = tab.Name;

                TabControlPanel panel = dh.CreateComponent(typeof(TabControlPanel)) as TabControlPanel;
                tabControl.ApplyDefaultPanelStyle(panel);
                tab.AttachedControl = panel;
                panel.TabItem = tab;

                tabControl.Tabs.Add(tab);
                tabControl.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                panel.SendToBack();

                tabControl.RecalcLayout();

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
			if(c is TabControlPanel)
				return true;
			return false;
		}

		protected override void InternalComponentRemoving(ComponentEventArgs e)
		{
			if(m_InternalRemove)
				return;
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;

			if(e.Component is TabControlPanel && this.Control!=null && this.Control.Controls.Contains(e.Component as Control))
			{
				try
				{
					TabControlPanel panel=e.Component as TabControlPanel;
					if(panel.TabItem!=null)
					{
						TabControl tabControl=this.Control as TabControl;
						tabControl.Tabs.Remove(panel.TabItem);
						dh.DestroyComponent(panel.TabItem);
					}
				}
				finally
				{
					m_InternalRemove=false;
				}
			}
			else if(e.Component is TabItem && this.Control!=null && ((TabControl)this.Control).Tabs.Contains(e.Component as TabItem))
			{
				try
				{
					TabItem item=e.Component as TabItem;
					if(item.AttachedControl!=null && this.Control.Controls.Contains(item.AttachedControl))
					{
						TabControl tabControl=this.Control as TabControl;
						tabControl.Controls.Remove(item.AttachedControl);
						dh.DestroyComponent(item.AttachedControl);
					}
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
                    TabControl tabControl=this.Control as TabControl;
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

		protected override TabStrip GetTabStrip()
		{
			if(this.Control==null || !(this.Control is TabControl))
				return null;
			return ((TabControl)this.Control).TabStrip;
		}

		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				ArrayList c=new ArrayList(base.AssociatedComponents);
				TabControl tabControl=this.Control as TabControl;
				if(tabControl!=null)
				{
					foreach(TabItem tab in tabControl.Tabs)
						c.Add(tab);
				}

				return c;
			}
		}

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["SelectedTabIndex"] = TypeDescriptor.CreateProperty(typeof(TabControlDesigner), (PropertyDescriptor)properties["SelectedTabIndex"], new Attribute[]
				{
					new BrowsableAttribute(true),
					new CategoryAttribute("Behavior")});
            properties["TabsVisible"] = TypeDescriptor.CreateProperty(typeof(TabControlDesigner), (PropertyDescriptor)properties["TabsVisible"], new Attribute[]
				{
					new BrowsableAttribute(true),
					new CategoryAttribute("Behavior")});
        }

        /// <summary>
        /// Gets or sets whether item is visible.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior"), Description("Indicates the index of selected tab.")]
        public int SelectedTabIndex
        {
            get
            {
                return (int)ShadowProperties["SelectedTabIndex"];
            }
            set
            {
                // this value is not passed to the actual control
                this.ShadowProperties["SelectedTabIndex"] = value;
            }
        }

        /// <summary>
        /// Gets or sets whether tabs are visible. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether tabs are visible")]
        public bool TabsVisible
        {
            get
            {
                return (bool)ShadowProperties["TabsVisible"];
            }
            set
            {
                // this value is not passed to the actual control
                this.ShadowProperties["TabsVisible"] = value;
            }
        }

	}
}
