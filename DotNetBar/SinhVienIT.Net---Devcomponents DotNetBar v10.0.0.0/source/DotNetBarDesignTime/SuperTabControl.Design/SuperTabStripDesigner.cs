using System;
using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Summary description for SuperTabStripDesigner
    /// </summary>
    public class SuperTabStripDesigner : BarBaseControlDesigner
    {
        #region Protected variables

        protected bool InternalRemove;

        #endregion

        #region Private variables

        private SuperTabStrip _TabStrip;
        private SuperTabItem _SelectItem;

        #endregion

        public SuperTabStripDesigner()
        {
            EnableItemDragDrop = true;
        }

        #region Public properties

        public SuperTabStrip TabStrip
        {
            get { return (_TabStrip); }
            set { _TabStrip = value; }
        }

        #endregion

        #region Initialize

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (component.Site.DesignMode == true)
            {
                _TabStrip = component as SuperTabStrip;

                ISelectionService ss =
                    (ISelectionService) GetService(typeof (ISelectionService));

                if (ss != null)
                    ss.SelectionChanged += OnSelectionChanged;
            }
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

        private void SetDesignTimeDefaults()
        {
            if (Component != null && Component.Site != null && Component.Site.DesignMode == true)
            {
                _TabStrip.TabStyle = eSuperTabStyle.Office2007;
                _TabStrip.TabFont = _TabStrip.Font;
                _TabStrip.SelectedTabFont = new System.Drawing.Font(_TabStrip.TabFont, FontStyle.Bold);
                CreateNewTab(null, null);
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
                    new DesignerVerb("Create New Tab", CreateNewTab),

					new DesignerVerb("Add Button", CreateButton),
					new DesignerVerb("Add Text Box", CreateTextBox),
					new DesignerVerb("Add Combo Box", CreateComboBox),
					new DesignerVerb("Add Label", CreateLabel),
					new DesignerVerb("Add Color Picker", CreateColorPicker),
                    new DesignerVerb("Add Progress bar", CreateProgressBar),
                    new DesignerVerb("Add Check box", CreateCheckBox),
                    new DesignerVerb("Add Switch Button", CreateSwitch),
                    new DesignerVerb("Add Micro-Chart", CreateMicroChart)
                };

                return (new DesignerVerbCollection(verbs));
            }
        }

        #endregion

        #region CreateNewTab
        public virtual SuperTabItem CreateNewTab()
        {
            IDesignerHost dh =
                (IDesignerHost)GetService(typeof(IDesignerHost));
            SuperTabItem tab = null;
            if (dh != null)
            {
                DesignerTransaction dt = dh.CreateTransaction();

                try
                {
                    m_CreatingItem = true;
                    IComponentChangeService change =
                        GetService(typeof(IComponentChangeService)) as IComponentChangeService;

                    if (change != null)
                        change.OnComponentChanging(Component, null);

                    tab =
                        dh.CreateComponent(typeof(SuperTabItem)) as SuperTabItem;

                    if (tab != null)
                    {
                        tab.Text = tab.Name;

                        _TabStrip.Tabs.Add(tab);
                    }

                    if (change != null)
                        change.OnComponentChanged(Component, null, null, null);
                    OnitemCreated(tab);
                }
                catch
                {
                    dt.Cancel();
                }
                finally
                {
                    if (dt.Canceled == false)
                        dt.Commit();
                    m_CreatingItem = false;
                }
            }
            return tab;
        }
        private void CreateNewTab(object sender, EventArgs e)
        {
            CreateNewTab();
        }
        #endregion

        #region SelectNextTab

        protected virtual void SelectNextTab(object sender, EventArgs e)
        {
            if (_TabStrip.SelectedTabIndex < _TabStrip.Tabs.Count - 1)
            {
                TypeDescriptor.GetProperties(_TabStrip)
                    ["SelectedTabIndex"].SetValue(_TabStrip, _TabStrip.SelectedTabIndex + 1);
            }
        }

        #endregion

        #region SelectPreviousTab

        protected virtual void SelectPreviousTab(object sender, EventArgs e)
        {
            if (_TabStrip.SelectedTabIndex > 0)
            {
                TypeDescriptor.GetProperties(_TabStrip)
                    ["SelectedTabIndex"].SetValue(_TabStrip, _TabStrip.SelectedTabIndex - 1);
            }
        }

        #endregion

        #region OnSelectionChanged

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (_TabStrip != null && _TabStrip.IsDisposed == false)
            {
                ISelectionService ss = (ISelectionService) sender;
                BaseItem item = null;

                if (ss != null && ss.PrimarySelection != Control)
                {
                    item = ss.PrimarySelection as BaseItem;

                    if (item != null && _TabStrip.Tabs.Contains(item) == false)
                        item = null;
                }

                if (_TabStrip.DesignTimeSelection != item)
                {
                    _TabStrip.DesignTimeSelection = item;

                    if (item != null)
                        ClosePopups();
                }
            }
        }

        #endregion

        #region ComponentChangeComponentAdded

        protected override void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
        {
            ISelectionService ss = this.GetService(typeof (ISelectionService)) as ISelectionService;

            if (ss != null && ss.PrimarySelection == _TabStrip)
            {
                if (e.Component is BaseItem)
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
                        cc.OnComponentChanging(_TabStrip, TypeDescriptor.GetProperties(_TabStrip)["SubItems"]);

                    _TabStrip.Tabs.Add(e.Component as BaseItem);

                    if (cc != null)
                        cc.OnComponentChanged(_TabStrip, TypeDescriptor.GetProperties(_TabStrip)["SubItems"], null, null);

                    if (m_InsertItemTransaction != null)
                    {
                        m_InsertItemTransaction.Commit();
                        m_InsertItemTransaction = null;
                    }

                    RecalcLayout();

                    if (e.Component is SuperTabItem)
                        _TabStrip.SelectedTab = e.Component as SuperTabItem;
                }
                else if (e.Component is SuperTabControlPanel)
                {
                    throw new Exception("Invalid component addition");
                }
            }
        }

        #endregion

        #region OtherComponentRemoving

        protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
        {
            BaseItem item = e.Component as BaseItem;

            if (item != null)
            {
                if (item.Parent != null && item.Parent.SubItems.Contains(item))
                    item.Parent.SubItems.Remove(item);

                DestroySubItems(item);

                RecalcLayout();
            }
        }

        #endregion

        #region AssociatedComponents

        public override ICollection AssociatedComponents
        {
            get
            {
                ArrayList list = new ArrayList(base.AssociatedComponents);

                foreach (BaseItem tab in _TabStrip.Tabs)
                    list.Add(tab);

                return (list);
            }
        }

        #endregion

        #region DragInProgress

        protected override bool DragInProgress
        {
            get { return (base.DragInProgress); }

            set
            {
                if (value == true)
                {
                    if (_TabStrip != null)
                        _SelectItem = _TabStrip.SelectedTab;
                }
                else
                {
                    if (_TabStrip != null)
                        _TabStrip.SelectedTab = _SelectItem;
                }

                base.DragInProgress = value;
            }
        }

        #endregion

        #region OnMouseDragBegin

        protected override void OnMouseDragBegin(int x, int y)
        {
            base.OnMouseDragBegin(x, y);

            if (DragInProgress == true)
                _SelectItem = _TabStrip.SelectedTab;
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            ISelectionService ss =
                (ISelectionService)GetService(typeof(ISelectionService));

            if (ss != null)
                ss.SelectionChanged -= OnSelectionChanged;

            base.Dispose(disposing);
        }

        #endregion
    }
}
