using System;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for SuperTabControlDesigner
    /// </summary>
    public class SuperTabControlDesigner : SuperTabStripDesigner
    {
        #region Private variables

        private SuperTabControl _TabControl;

        #endregion

        public SuperTabControlDesigner()
        {
            TabsVisible = true;
        }

        #region Initialize

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (component.Site.DesignMode == true)
            {
                _TabControl = component as SuperTabControl;

                if (_TabControl != null)
                {
                    TabStrip = _TabControl.TabStrip;

                    if (Inherited == true)
                    {
                        if (_TabControl.TabsVisible == false)
                            _TabControl.TabsVisible = true;

                        if (_TabControl.SelectedPanel != null)
                            _TabControl.SelectedPanel.BringToFront();
                    }
                    _TabControl.TabStrip.GetBaseItemContainer().SetDesignMode(true);
                }
            }
        }

        protected override BaseItem GetItemContainer()
        {
            return _TabControl.TabStrip.GetBaseItemContainer();
        }

        protected override Control GetItemContainerControl()
        {
            return _TabControl.TabStrip;
        }
        #endregion

        #region PreFilterProperties

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties["SelectedTabIndex"] = TypeDescriptor.CreateProperty(typeof(SuperTabControlDesigner),
                (PropertyDescriptor)properties["SelectedTabIndex"], new Attribute[]
			{
				new BrowsableAttribute(true),
				new CategoryAttribute("Behavior")
            });

            properties["TabsVisible"] = TypeDescriptor.CreateProperty(typeof(SuperTabControlDesigner),
                (PropertyDescriptor)properties["TabsVisible"], new Attribute[]
			{
				new BrowsableAttribute(true),
				new CategoryAttribute("Behavior")
            });
        }

        /// <summary>
        /// Gets or sets whether item is visible.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior")]
        [Description("Indicates the index of selected tab.")]
        public int SelectedTabIndex
        {
            get { return (int)ShadowProperties["SelectedTabIndex"]; }
            set { this.ShadowProperties["SelectedTabIndex"] = value; }
        }

        /// <summary>
        /// Gets or sets whether tabs are visible. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether tabs are visible")]
        public bool TabsVisible
        {
            get { return ((bool)ShadowProperties["TabsVisible"]); }
            set { ShadowProperties["TabsVisible"] = value; }
        }

        #endregion
        
        #region PostFilterEvents

        protected override void  PostFilterEvents(IDictionary events)
        {
            events.Remove("MouseUp");
            events.Remove("MouseDown");
            events.Remove("MouseMove");
            events.Remove("MouseClick");
            events.Remove("MouseDoubleClick");

            events.Remove("MouseEnter");
            events.Remove("MouseHover");
            events.Remove("MouseLeave");

 	        base.PostFilterEvents(events);
        }

        #endregion

        #region CreateNewTab

        public override SuperTabItem CreateNewTab()
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));

            if (dh == null)
                return null;
            SuperTabItem tab = null;
            DesignerTransaction dt = dh.CreateTransaction();

            try
            {
                IComponentChangeService change =
                    GetService(typeof(IComponentChangeService)) as IComponentChangeService;
               
                if (change != null)
                    change.OnComponentChanging(this.Component, null);

                tab = dh.CreateComponent(typeof(SuperTabItem)) as SuperTabItem;
                SuperTabControlPanel panel = dh.CreateComponent(typeof(SuperTabControlPanel)) as SuperTabControlPanel;

                if (tab != null && panel != null)
                {
                    tab.Text = tab.Name;
                    _TabControl.CreateTab(tab, panel, -1);
                    _TabControl.SelectedTab = tab;
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
                if (!dt.Canceled)
                    dt.Commit();
            }
            return tab;
        }

        #endregion

        #region SelectNextTab

        protected override void SelectNextTab(object sender, EventArgs e)
        {
            if (_TabControl.SelectedTabIndex < _TabControl.Tabs.Count - 1)
            {
                TypeDescriptor.GetProperties(_TabControl)
                    ["SelectedTabIndex"].SetValue(_TabControl, _TabControl.SelectedTabIndex + 1);
            }
        }

        #endregion

        #region SelectPreviousTab

        protected override void SelectPreviousTab(object sender, EventArgs e)
        {
            if (_TabControl.SelectedTabIndex > 0)
            {
                TypeDescriptor.GetProperties(_TabControl)
                    ["SelectedTabIndex"].SetValue(_TabControl, _TabControl.SelectedTabIndex - 1);
            }
        }

        #endregion

        #region GetTabStrip

        protected SuperTabStrip GetTabStrip()
        {
            return (_TabControl.TabStrip);
        }

        #endregion

        #region GetIOwner

        protected override IOwner GetIOwner()
        {
            return (GetTabStrip());
        }

        #endregion

        #region CanParent

        public override bool CanParent(Control c)
        {
            return (c is SuperTabControlPanel);
        }

        #endregion

        #region AssociatedComponents

        public override ICollection AssociatedComponents
        {
            get
            {
                ArrayList list = new ArrayList(base.AssociatedComponents);

                foreach (BaseItem item in _TabControl.Tabs)
                    list.Add(item);

                return (list);
            }
        }

        #endregion

        #region ComponentChangeComponentAdded

        protected override void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
        {
            ISelectionService ss = this.GetService(typeof (ISelectionService)) as ISelectionService;

            if (ss != null && ss.PrimarySelection == _TabControl)
            {
                if (m_InsertItemTransaction == null)
                {
                    IDesignerHost dh = sender as IDesignerHost;

                    if (dh != null)
                        m_InsertItemTransaction = dh.CreateTransaction("Adding Item Clip");
                }

                IComponentChangeService cc =
                    GetService(typeof (IComponentChangeService)) as IComponentChangeService;

                if (cc != null)
                {
                    if (e.Component is BaseItem)
                    {
                        cc.OnComponentChanging(_TabControl, null);
                        _TabControl.Tabs.Add(e.Component as BaseItem);
                        cc.OnComponentChanged(_TabControl, null, null, null);

                        if (e.Component is SuperTabItem)
                            _TabControl.SelectedTab = e.Component as SuperTabItem;
                    }
                }

                if (m_InsertItemTransaction != null)
                {
                    m_InsertItemTransaction.Commit();
                    m_InsertItemTransaction = null;
                }

                RecalcLayout();
            }
        }

        #endregion

        #region OtherComponentRemoving

        protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
        {
            if (InternalRemove == true)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));

            if (dh == null)
                return;

            if (e.Component is SuperTabControlPanel &&
                _TabControl.Controls.Contains(e.Component as Control))
            {
                SuperTabControlPanel panel = e.Component as SuperTabControlPanel;

                _TabControl.Controls.Remove(panel);

                if (panel.TabItem != null)
                {
                    if (_TabControl.Tabs.Contains(panel.TabItem))
                        _TabControl.Tabs.Remove(panel.TabItem);
                    if (!Inherited)
                        dh.DestroyComponent(panel.TabItem);
                }
            }
            else if (e.Component is BaseItem &&
                _TabControl.Tabs.Contains(e.Component as BaseItem))
            {
                if (dh.TransactionDescription.StartsWith("Deleting") == false)
                {
                    BaseItem item = e.Component as BaseItem;

                    _TabControl.Tabs.Remove(item);

                    SuperTabItem tab = item as SuperTabItem;

                    if (tab != null)
                    {
                        if (tab.AttachedControl != null &&
                            _TabControl.Controls.Contains(tab.AttachedControl))
                        {
                            _TabControl.Controls.Remove(tab.AttachedControl);

                            dh.DestroyComponent(tab.AttachedControl);
                        }
                    }
                }
            }
            else if (e.Component == this.Control)
            {
                InternalRemove = true;

                try
                {
                    for (int i = _TabControl.Tabs.Count - 1; i >= 0; i--)
                        dh.DestroyComponent(_TabControl.Tabs[i]);
                }
                finally
                {
                    InternalRemove = false;
                }
            }
        }

        #endregion

        #region GetClientPoint

        protected override Point GetClientPoint(int x, int y)
        {
            Point p = _TabControl.TabStrip.PointToClient(new Point(x, y));
            return p;
        }
        #endregion

        #region Backstage Support
        protected override void OnitemCreated(BaseItem item)
        {
            if (item is ButtonItem && IsBackstageTab)
            {
                ButtonItem button = (ButtonItem)item;
                button.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
                button.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
                button.ImagePaddingVertical = 10;
                button.ImagePaddingHorizontal = 18;
                button.Stretch = true;
            }
            base.OnitemCreated(item);
        }
        internal bool IsBackstageTab
        {
            get
            {
                SuperTabControl tab = (SuperTabControl)this.Control;
                if (tab != null && tab.TabStrip != null)
                    return (tab.TabStrip.ApplicationButton != null);
                return false;
            }
        }
        #endregion
    }
}
