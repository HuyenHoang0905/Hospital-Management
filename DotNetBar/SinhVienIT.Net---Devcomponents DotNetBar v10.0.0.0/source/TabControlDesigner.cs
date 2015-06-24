using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for TabControlDesigner.
	/// </summary>
	public class TabControlDesigner:TabStripDesigner
	{
		public TabControlDesigner()
		{
		}

		protected override void CreateNewTab(object sender, EventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			TabControl tabControl=this.Control as TabControl;
			if(tabControl==null || dh==null)
				return;

			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
				change.OnComponentChanging(this.Component,null);

			TabItem tab=dh.CreateComponent(typeof(TabItem)) as TabItem;
			tab.Text=tab.Name;

			TabControlPanel panel=dh.CreateComponent(typeof(TabControlPanel)) as TabControlPanel;
			tabControl.ApplyDefaultPanelStyle(panel);
			tab.AttachedControl=panel;
			panel.TabItem=tab;
			
			tabControl.Tabs.Add(tab);
			tabControl.Controls.Add(panel);
			panel.Dock=DockStyle.Fill;
			panel.SendToBack();
			
			tabControl.RecalcLayout();

			if(change!=null)
				change.OnComponentChanged(this.Component,null,null,null);

			if(change!=null)
			{
				change.OnComponentChanging(panel,null);
				change.OnComponentChanged(panel,null,null,null);
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
	}
}
