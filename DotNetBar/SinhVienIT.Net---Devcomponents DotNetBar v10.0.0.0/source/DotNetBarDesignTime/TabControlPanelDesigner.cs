using System;
using System.Windows.Forms.Design;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
	#region TabControlPanelDesigner
	/// <summary>
	/// Designer for Tab Control Panel.
	/// </summary>
	public class TabControlPanelDesigner:PanelExDesigner
	{
		public override SelectionRules SelectionRules
		{
			get{return (SelectionRules.Locked | SelectionRules.Visible);}
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
			PanelEx p = this.Control as PanelEx;
			if (p == null)
				return;
			p.ApplyLabelStyle();
			p.Text = "";
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
            TabControlPanel cp = this.Control as TabControlPanel;
            if (cp == null || !(cp.Parent is TabControl))
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            TabControl tabControl = cp.Parent as TabControl;
            if (tabControl == null || dh == null)
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
                tabControl.SelectedTab = tab;

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

        private void SelectNextTab(object sender, EventArgs e)
        {
            TabControlPanel panel = this.Control as TabControlPanel;
            if (panel == null || !(panel.Parent is TabControl))
                return;
            TabControl tc = panel.Parent as TabControl;
            tc.SelectNextTab();
        }

        private void SelectPreviousTab(object sender, EventArgs e)
        {
            TabControlPanel panel = this.Control as TabControlPanel;
            if (panel == null || !(panel.Parent is TabControl))
                return;
            TabControl tc = panel.Parent as TabControl;
            tc.SelectPreviousTab();
        }
	}
	#endregion
}
