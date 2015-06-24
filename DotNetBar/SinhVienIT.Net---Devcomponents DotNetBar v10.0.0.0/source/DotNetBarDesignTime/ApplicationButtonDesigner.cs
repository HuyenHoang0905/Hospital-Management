using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    public class ApplicationButtonDesigner : BaseItemDesigner
    {
        #region Internal Implementation
        protected override DesignerVerb[] GetVerbs()
        {
            DesignerVerb[] baseVerbs = base.GetVerbs();
            int verbsCount = baseVerbs.Length + 1;
            if (IsBackstageSet) verbsCount = 1;

            bool includeClearSubItems = false;
            Office2007StartButton appButton = this.Component as Office2007StartButton;
            if (appButton != null && appButton.BackstageTab != null && appButton.SubItems.Count > 0)
            {
                includeClearSubItems = true;
                verbsCount++;
            }

            int verbsOffset = 1;
            DesignerVerb[] verbs = new DesignerVerb[verbsCount];
            verbs[0] = new DesignerVerb((IsBackstageSet ? "Remove Backstage" : "Set Backstage"), new EventHandler(CreateBackstageTab));

            if (includeClearSubItems)
            {
                verbs[1] = new DesignerVerb("Clear Sub-items", new EventHandler(ClearSubItems));
                verbsOffset++;
            }

            if (!IsBackstageSet)
            {
                for (int i = 0; i < baseVerbs.Length; i++)
                {
                    verbs[i + verbsOffset] = baseVerbs[i];
                }
            }

            return verbs;
        }

        private void ClearSubItems(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all sub-items?", "DotNetBar Application Button", MessageBoxButtons.YesNo) == DialogResult.No) return;

            IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (dh == null) return;
            Office2007StartButton appButton = (Office2007StartButton)this.Component;
            if (appButton.SubItems.Count == 0) return;

            DesignerTransaction trans = dh.CreateTransaction("Clearing Application Button SubItems");
            try
            {
                cc.OnComponentChanging(appButton, TypeDescriptor.GetProperties(appButton)["SubItems"]);
                BaseItem[] items = new BaseItem[appButton.SubItems.Count];
                appButton.SubItems.CopyTo(items, 0);
                foreach (BaseItem item in items)
                {
                    appButton.SubItems.Remove(item);
                    dh.DestroyComponent(item);
                }
                cc.OnComponentChanged(appButton, TypeDescriptor.GetProperties(appButton)["SubItems"], null, null);
            }
            catch
            {
                trans.Cancel();
                throw;
            }
            finally
            {
                if (!trans.Canceled)
                    trans.Commit();
            }
        }

        private bool IsBackstageSet
        {
            get
            {
                Office2007StartButton appButton = (Office2007StartButton)this.Component;
                if (appButton != null && appButton.BackstageTab != null) return true;
                return false;
            }
        }

        private void CreateBackstageTab()
        {
            IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (dh == null) return;

            Office2007StartButton appButton = (Office2007StartButton)this.Component;
            if (appButton.BackstageTab != null)
            {
                DesignerTransaction trans1 = dh.CreateTransaction("Removing Backstage tab");
                try
                {
                    SuperTabControl backstageTab = appButton.BackstageTab;
                    SetProperty(appButton, "BackstageTab", null);
                    backstageTab.Visible = true;
                    SetProperty(backstageTab, "Location", new System.Drawing.Point(0, 150));
                    SetProperty(backstageTab, "Size", new System.Drawing.Size(250, 350));
                    ISelectionService selection = (ISelectionService)GetService(typeof(ISelectionService));
                    if (selection != null) selection.SetSelectedComponents(new Control[] { backstageTab }, SelectionTypes.Primary);
                }
                catch
                {
                    trans1.Cancel();
                    throw;
                }
                finally
                {
                    if (!trans1.Canceled)
                        trans1.Commit();
                }
                return;
            }

            DesignerTransaction trans = dh.CreateTransaction("Create Backstage");
            try
            {

                if (appButton.Expanded) appButton.Expanded = false;
                SuperTabControl backstageTab = (SuperTabControl)dh.CreateComponent(typeof(SuperTabControl));
                SetProperty(appButton, "BackstageTab", backstageTab);

                Control root = dh.RootComponent as Control;
                if (root != null)
                {
                    cc.OnComponentChanging(root, TypeDescriptor.GetProperties(root)["Controls"]);
                    root.Controls.Add(backstageTab);
                    cc.OnComponentChanged(root, TypeDescriptor.GetProperties(root)["Controls"], null, null);
                }
                SetupBackstageTab(backstageTab, dh);
            }
            catch
            {
                trans.Cancel();
                throw;
            }
            finally
            {
                if (!trans.Canceled)
                    trans.Commit();
            }
        }
        private void CreateBackstageTab(object sender, EventArgs e)
        {
            CreateBackstageTab();
        }

        private void SetupBackstageTab(SuperTabControl backstageTab, IDesignerHost dh)
        {
            backstageTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            backstageTab.ControlBox.Visible = false;
            backstageTab.ItemPadding.Left = 6;
            backstageTab.ItemPadding.Right = 4;
            backstageTab.ItemPadding.Top = 4;
            backstageTab.ReorderTabsEnabled = false;
            try
            {
                backstageTab.SelectedTabFont = new System.Drawing.Font("Segoe UI", 9.75F);
            }
            catch { }
            backstageTab.SelectedTabIndex = 0;
            backstageTab.Size = new System.Drawing.Size(614, 315);
            backstageTab.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Left;
            try
            {
                backstageTab.TabFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            catch { }
            backstageTab.TabHorizontalSpacing = 16;
            backstageTab.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.Office2010BackstageBlue;
            backstageTab.TabVerticalSpacing = 8;

            SuperTabControlDesigner tabDesigner = dh.GetDesigner(backstageTab) as SuperTabControlDesigner;
            if (tabDesigner != null)
            {
                ButtonItem button;
                // Save button
                button = tabDesigner.CreateButton();
                SetProperty(button, "Text", "Save");
                SetProperty(button, "KeyTips", "S");
                SetProperty(button, "Image", RibbonControlDesigner.LoadImage("Save16.png"));

                // Open
                button = tabDesigner.CreateButton();
                SetProperty(button, "Text", "Open");
                SetProperty(button, "KeyTips", "O");
                SetProperty(button, "Image", RibbonControlDesigner.LoadImage("Open.png"));

                // Close
                button = tabDesigner.CreateButton();
                SetProperty(button, "Text", "Close");
                SetProperty(button, "KeyTips", "C");
                SetProperty(button, "Image", RibbonControlDesigner.LoadImage("Close16.png"));

                SuperTabItem tab;
                tab = tabDesigner.CreateNewTab();
                SetProperty(tab, "Text", "Recent");
                SetProperty(tab, "KeyTips", "R");
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImage", RibbonControlDesigner.LoadImage("BlueBackstageBgImage.png"));
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImagePosition", DevComponents.DotNetBar.eStyleBackgroundImage.BottomRight);

                tab = tabDesigner.CreateNewTab();
                SetProperty(tab, "Text", "New");
                SetProperty(tab, "KeyTips", "N");
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImage", RibbonControlDesigner.LoadImage("BlueBackstageBgImage.png"));
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImagePosition", DevComponents.DotNetBar.eStyleBackgroundImage.BottomRight);

                tab = tabDesigner.CreateNewTab();
                SetProperty(tab, "Text", "Print");
                SetProperty(tab, "KeyTips", "P");
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImage", RibbonControlDesigner.LoadImage("BlueBackstageBgImage.png"));
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImagePosition", DevComponents.DotNetBar.eStyleBackgroundImage.BottomRight);

                tab = tabDesigner.CreateNewTab();
                SetProperty(tab, "Text", "Help");
                SetProperty(tab, "KeyTips", "H");
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImage", RibbonControlDesigner.LoadImage("BlueBackstageBgImage.png"));
                SetProperty((SuperTabControlPanel)tab.AttachedControl, "BackgroundImagePosition", DevComponents.DotNetBar.eStyleBackgroundImage.BottomRight);

                // Options
                button = tabDesigner.CreateButton();
                SetProperty(button, "Text", "Options");
                SetProperty(button, "KeyTips", "T");
                SetProperty(button, "Image", RibbonControlDesigner.LoadImage("Options2.png"));

                // Exit
                button = tabDesigner.CreateButton();
                SetProperty(button, "Text", "Exit");
                SetProperty(button, "KeyTips", "X");
                SetProperty(button, "Image", RibbonControlDesigner.LoadImage("Exit2.png"));
            }
        }

        private void SetProperty(object targetObject, string propertyName, object propertyValue)
        {
            TypeDescriptor.GetProperties(targetObject)[propertyName].SetValue(targetObject, propertyValue);
        }
        #endregion
    }
}
