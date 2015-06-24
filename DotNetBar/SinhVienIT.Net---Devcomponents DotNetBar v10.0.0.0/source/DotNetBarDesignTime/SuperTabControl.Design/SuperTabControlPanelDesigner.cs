using System;
using System.Windows.Forms.Design;
using System.Collections;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Designer for Tab Control Panel.
    /// </summary>
    public class SuperTabControlPanelDesigner : PanelControlDesigner
    {
        #region SelectionRules

        public override SelectionRules SelectionRules
        {
            get { return (SelectionRules.Locked | SelectionRules.Visible); }
        }

        #endregion

        #region InitializeNewComponent

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

        #endregion

        #region SetDesignTimeDefaults

        protected override void SetDesignTimeDefaults()
        {
            PanelControl p = this.Control as PanelControl;

            if (p != null)
            {
                p.ApplyLabelStyle();
                p.Text = "";
            }
        }

        #endregion

        #region Verbs

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = new DesignerVerb[]
                {
                    new DesignerVerb("Next Tab", SelectNextTab),
                    new DesignerVerb("Previous Tab", SelectPreviousTab),
                    new DesignerVerb("Create New Tab", CreateNewTab)
                };

                return (new DesignerVerbCollection(verbs));
            }
        }

        #endregion

        #region CreateNewTab

        protected virtual void CreateNewTab(object sender, EventArgs e)
        {
            SuperTabControlPanel cp = this.Control as SuperTabControlPanel;

            if (cp == null || !(cp.Parent is SuperTabControl))
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            SuperTabControl tabControl = cp.Parent as SuperTabControl;

            if (tabControl == null || dh == null)
                return;

            DesignerTransaction dt = dh.CreateTransaction();

            try
            {
                IComponentChangeService change = 
                    GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                
                if (change != null)
                    change.OnComponentChanging(this.Component, null);

                SuperTabItem tab = dh.CreateComponent(typeof(SuperTabItem)) as SuperTabItem;
                SuperTabControlPanel panel = dh.CreateComponent(typeof(SuperTabControlPanel)) as SuperTabControlPanel;

                if (tab != null && panel != null)
                {
                    tab.Text = tab.Name;
                    tabControl.CreateTab(tab, panel, -1);
                    tabControl.SelectedTab = tab;
                }

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

        #endregion

        #region SelectNextTab

        private void SelectNextTab(object sender, EventArgs e)
        {
            SuperTabControlPanel panel = Control as SuperTabControlPanel;

            if (panel == null || !(panel.Parent is SuperTabControl))
                return;

            SuperTabControl tc = panel.Parent as SuperTabControl;

            tc.SelectNextTab();
        }

        #endregion

        #region SelectPreviousTab

        private void SelectPreviousTab(object sender, EventArgs e)
        {
            SuperTabControlPanel panel = Control as SuperTabControlPanel;

            if (panel == null || !(panel.Parent is SuperTabControl))
                return;

            SuperTabControl tc = panel.Parent as SuperTabControl;

            tc.SelectPreviousTab();
        }

        #endregion
    }
}
