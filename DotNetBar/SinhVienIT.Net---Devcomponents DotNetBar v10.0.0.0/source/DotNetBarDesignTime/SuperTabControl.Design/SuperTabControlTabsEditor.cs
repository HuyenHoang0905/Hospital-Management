using System;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Support for SuperTabStrip tabs design-time editor
    /// </summary>
    public class SuperTabControlTabsEditor : SuperTabStripTabsEditor
    {
        public SuperTabControlTabsEditor(Type type)
            : base(type)
        {
        }

        #region CreateInstance

        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);

            if (item is SuperTabItem)
            {
                SuperTabItem tab = item as SuperTabItem;

                tab.Text = String.IsNullOrEmpty(tab.Name) ? "My Tab" : tab.Name;

                CreateNewTab(tab);
            }
            else if (item is ButtonItem)
            {
                ButtonItem bi = item as ButtonItem;

                bi.Text = String.IsNullOrEmpty(bi.Name) ? "My Button" : bi.Name;
            }

            return (item);
        }

        #endregion

        #region CreateNewTab

        private void CreateNewTab(SuperTabItem tab)
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            SuperTabControl tabControl = Context.Instance as SuperTabControl;

            if (dh == null || tabControl == null)
                return;

            DesignerTransaction dt = dh.CreateTransaction();

            try
            {
                IComponentChangeService change =
                    GetService(typeof(IComponentChangeService)) as IComponentChangeService;

                if (change != null)
                    change.OnComponentChanging(Context.Container, null);

                SuperTabControlPanel panel = dh.CreateComponent(typeof(SuperTabControlPanel)) as SuperTabControlPanel;

                if (tab != null && panel != null)
                {
                    tab.AttachedControl = panel;
                    panel.TabItem = tab;

                    tabControl.Controls.Add(panel);
                    tabControl.Tabs.Add(tab);

                    panel.Dock = DockStyle.Fill;
                    panel.BringToFront();

                    tabControl.ApplyPanelStyle(panel);

                    tabControl.SelectedTab = tab;
                }

                if (change != null)
                    change.OnComponentChanged(Context.Container, null, null, null);

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
                if (!dt.Canceled)
                    dt.Commit();
            }
        }

        #endregion
    }
}
