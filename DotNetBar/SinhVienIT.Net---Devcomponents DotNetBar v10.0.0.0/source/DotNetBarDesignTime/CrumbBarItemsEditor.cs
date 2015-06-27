#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Design;
using System.ComponentModel.Design;
using System.Drawing.Imaging;
using DevComponents.DotNetBar.Rendering;
using DevComponents.AdvTree;
using System.Windows.Forms.Design;
using System.Reflection;

namespace DevComponents.DotNetBar.Design
{
    internal class CrumbBarItemsEditor : UserControl
    {
        #region Private Variables
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ButtonX buttonAddItem;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.ColumnHeader columnHeader2;
        private DevComponents.DotNetBar.ButtonX buttonRemoveItem;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        internal DevComponents.DotNetBar.ButtonX buttonX1;

        private CrumbBar _CrumbBar = null;
        private Label label1;
        private CrumbBarDesigner _Designer = null;
        #endregion

        #region Constructors
        public CrumbBarItemsEditor()
        {
            InitializeComponent();
#if (!TRIAL)
            this.advTree1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.buttonRemoveItem = new DevComponents.DotNetBar.ButtonX();
            this.buttonAddItem = new DevComponents.DotNetBar.ButtonX();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader2 = new DevComponents.AdvTree.ColumnHeader();
            this.node1 = new DevComponents.AdvTree.Node();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(300, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(284, 331);
            this.propertyGrid1.TabIndex = 4;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(515, 341);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor();
            this.buttonX1.Size = new System.Drawing.Size(69, 24);
            this.buttonX1.TabIndex = 5;
            this.buttonX1.Text = "&Close";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // buttonRemoveItem
            // 
            this.buttonRemoveItem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemoveItem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonRemoveItem.Enabled = false;
            this.buttonRemoveItem.FocusCuesEnabled = false;
            this.buttonRemoveItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveItem.Location = new System.Drawing.Point(32, 341);
            this.buttonRemoveItem.Name = "buttonRemoveItem";
            this.buttonRemoveItem.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor();
            this.buttonRemoveItem.Size = new System.Drawing.Size(24, 24);
            this.buttonRemoveItem.TabIndex = 3;
            this.buttonRemoveItem.Click += new System.EventHandler(this.buttonRemoveItem_Click);
            // 
            // buttonAddItem
            // 
            this.buttonAddItem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddItem.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAddItem.FocusCuesEnabled = false;
            this.buttonAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddItem.Location = new System.Drawing.Point(4, 341);
            this.buttonAddItem.Name = "buttonAddItem";
            this.buttonAddItem.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor();
            this.buttonAddItem.Size = new System.Drawing.Size(24, 24);
            this.buttonAddItem.TabIndex = 1;
            this.buttonAddItem.Click += new System.EventHandler(this.buttonAddItem_Click);
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.AllowDrop = true;
            this.advTree1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.advTree1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.Columns.Add(this.columnHeader1);
            this.advTree1.Columns.Add(this.columnHeader2);
            this.advTree1.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle;
            this.advTree1.ExpandWidth = 18;
            this.advTree1.Location = new System.Drawing.Point(3, 3);
            this.advTree1.Name = "advTree1";
            this.advTree1.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advTree1.NodeStyle = this.elementStyle1;
            this.advTree1.PathSeparator = ";";
            this.advTree1.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect;
            this.advTree1.Size = new System.Drawing.Size(291, 332);
            this.advTree1.Styles.Add(this.elementStyle1);
            this.advTree1.SuspendPaint = false;
            this.advTree1.TabIndex = 0;
            this.advTree1.Text = "advTree1";
            this.advTree1.AfterNodeDrop += new DevComponents.AdvTree.TreeDragDropEventHandler(this.advTree1_AfterNodeDrop);
            this.advTree1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.advTree1_MouseUp);
            this.advTree1.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.advTree1_AfterNodeSelect);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "Text";
            this.columnHeader1.Width.Relative = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Name = "columnHeader2";
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width.Relative = 30;
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // elementStyle1
            // 
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Drag && drop items to re-order";
            this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            // 
            // CrumbBarItemsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonRemoveItem);
            this.Controls.Add(this.buttonAddItem);
            this.Controls.Add(this.advTree1);
            this.Name = "CrumbBarItemsEditor";
            this.Size = new System.Drawing.Size(587, 373);
            this.Load += new System.EventHandler(this.CrumbBarItemsEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Internal Implementation

        private object GetDesignService(Type serviceType)
        {
            if (_Designer != null)
            {
                return _Designer.GetDesignService(serviceType);
            }
            return null;
        }

        private CrumbBarItem CreateItem()
        {
            IDesignerHost dh = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
            if (dh == null)
                return null;

            CrumbBarItem item = (CrumbBarItem)dh.CreateComponent(typeof(CrumbBarItem));
            item.Text = item.Name;
            return item;
        }

        private void AddNewItem()
        {
            AddNewItem(null);
        }

        private void AddNewItem(CrumbBarItem parent)
        {
            IDesignerHost dh = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
            DesignerTransaction dt = null;
            if (dh != null)
            {
                dt = dh.CreateTransaction("New CrumbBarItem");
            }
            bool isEmpty = advTree1.Nodes.Count == 0;
            CrumbBarItem item = CreateItem();
            if (item == null) return;

            IComponentChangeService cc = GetDesignService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (cc != null)
            {
                if (parent == null)
                    cc.OnComponentChanging(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"]);
                else
                    cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
            }

            if (parent == null)
                _CrumbBar.Items.Add(item);
            else
                parent.SubItems.Add(item);

            if (cc != null)
            {
                if (parent == null)
                    cc.OnComponentChanged(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"], null, null);
                else
                    cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
            }

            if (dt != null)
                dt.Commit();

            if (parent == null)
                advTree1.Nodes.Add(CreateNodeForItem(item));
            else
            {
                advTree1.SelectedNode.Nodes.Add(CreateNodeForItem(item));
                advTree1.SelectedNode.Expand();
            }

            if (isEmpty && advTree1.SelectedNode == null && advTree1.Nodes.Count > 0)
                advTree1.SelectedNode = advTree1.Nodes[0];
        }
        #endregion

        private void CrumbBarItemsEditor_Load(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            Color c = Color.DarkGray;
            if (GlobalManager.Renderer is Office2007Renderer)
                c = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.CheckBoxItem.Default.Text;
            using (Graphics g = Graphics.FromImage(img))
            {
                using (Brush brush = new SolidBrush(c))
                {
                    g.FillRectangle(brush, 7, 2, 3, 13);
                    g.FillRectangle(brush, 2, 7, 13, 3);
                }
            }
            buttonAddItem.Image = img;

            img = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(img))
            {
                using (Brush brush = new SolidBrush(c))
                {
                    g.FillRectangle(brush, 2, 7, 13, 3);
                }
            }
            buttonRemoveItem.Image = img;

#if (FRAMEWORK20)
            IUIService service = GetDesignService(typeof(IUIService)) as IUIService;
            if (service != null)
            {
                PropertyInfo pi = propertyGrid1.GetType().GetProperty("ToolStripRenderer", System.Reflection.BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    pi.SetValue(propertyGrid1, (ToolStripProfessionalRenderer)service.Styles["VsToolWindowRenderer"], null);
                }
            }
#endif
        }

        private void advTree1_MouseUp(object sender, MouseEventArgs e)
        {
            Node node = advTree1.GetNodeAt(e.Y);
            if (node == null) advTree1.SelectedNode = null;
        }

        internal void UpdateDisplay()
        {
            advTree1.Nodes.Clear();
            if (_CrumbBar == null) return;

            advTree1.BeginUpdate();

            foreach (CrumbBarItem item in _CrumbBar.Items)
            {
                Node node = CreateNodeForItem(item);
                advTree1.Nodes.Add(node);
                LoadSubItems(node, item);
            }

            advTree1.EndUpdate();
        }

        private void LoadSubItems(Node parent, CrumbBarItem item)
        {
            foreach (BaseItem o in item.SubItems)
            {
                if (o is CrumbBarItem)
                {
                    CrumbBarItem cb = (CrumbBarItem)o;
                    Node node = CreateNodeForItem(cb);
                    parent.Nodes.Add(node);
                    LoadSubItems(node, cb);
                }
            }
        }

        private Node CreateNodeForItem(CrumbBarItem item)
        {
            Node node = new Node();
            node.Expanded = true;
            node.Tag = item;
            node.Text = item.Text;
            node.Image = item.GetItemImage();
            node.Cells.Add(new Cell(item.Name));
            return node;
        }

        public CrumbBarDesigner Designer
        {
            get { return _Designer; }
            set 
            {
                _Designer = value; 
                if(_Designer!=null)
                    this.propertyGrid1.Site = new PropertyGridSite((IServiceProvider)_Designer.Component.Site, this.propertyGrid1);
            }
        }

        public CrumbBar CrumbBar
        {
            get { return _CrumbBar; }
            set
            {
                if (value != _CrumbBar)
                {
                    CrumbBar oldValue = _CrumbBar;
                    _CrumbBar = value;
                    OnCrumbBarChanged(oldValue, value);
                }
            }
        }

        private void OnCrumbBarChanged(CrumbBar oldValue, CrumbBar newValue)
        {
            UpdateDisplay();
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            CrumbBarItem parent = null;
            if (advTree1.SelectedNode != null) parent = advTree1.SelectedNode.Tag as CrumbBarItem;
            AddNewItem(parent);
        }

        private void buttonRemoveItem_Click(object sender, EventArgs e)
        {
            if (advTree1.SelectedNode == null) return;
            CrumbBarItem item = advTree1.SelectedNode.Tag as CrumbBarItem;
            if (item == null) return;
            advTree1.SelectedNode.Remove();
            DeleteItem(item);
        }

        private void DeleteItem(CrumbBarItem item)
        {
            IDesignerHost dh = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
            if (dh == null)
                return;
            dh.DestroyComponent(item);
        }

        private void advTree1_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            buttonRemoveItem.Enabled = e.Node != null;
            if (e.Node != null)
                propertyGrid1.SelectedObject = e.Node.Tag;
            else
                propertyGrid1.SelectedObject = null;
        }

        private void advTree1_AfterNodeDrop(object sender, TreeDragDropEventArgs e)
        {
            IDesignerHost dh = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
            DesignerTransaction dt = null;
            if (dh != null) dt = dh.CreateTransaction("Move items");

            IComponentChangeService cc = GetDesignService(typeof(IComponentChangeService)) as IComponentChangeService;

            try
            {
                CrumbBarItem movedItem = e.Node.Tag as CrumbBarItem;
                CrumbBarItem parent = (CrumbBarItem)movedItem.Parent;
                if (cc != null)
                {
                    if (parent == null)
                        cc.OnComponentChanging(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"]);
                    else
                        cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
                }

                if (parent == null)
                    _CrumbBar.Items.Remove(movedItem);
                else
                    parent.SubItems.Remove(movedItem);

                if (cc != null)
                {
                    if (parent == null)
                        cc.OnComponentChanged(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"], null, null);
                    else
                        cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
                }

                if (e.NewParentNode == null)
                {
                    int index = advTree1.Nodes.IndexOf(e.Node);
                    if (cc != null) cc.OnComponentChanging(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"]);
                    _CrumbBar.Items.Insert(index, movedItem);
                    if (cc != null) cc.OnComponentChanged(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"], null, null);
                }
                else
                {
                    parent = e.NewParentNode.Tag as CrumbBarItem;
                    int index = e.NewParentNode.Nodes.IndexOf(e.Node);
                    if (cc != null) cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
                    parent.SubItems.Insert(index, movedItem);
                    if (cc != null) cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
                }
            }
            catch
            {
                if (dt != null)
                    dt.Cancel();
                throw;
            }
            finally
            {
                if (dt != null && !dt.Canceled)
                    dt.Commit();
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (advTree1.SelectedNode == null) return;
            Node node = advTree1.SelectedNode;
            CrumbBarItem item = (CrumbBarItem)propertyGrid1.SelectedObject;
            if (e.ChangedItem.PropertyDescriptor.Name == "Text")
            {
                node.Text = item.Text;
            }
            else if (e.ChangedItem.PropertyDescriptor.Name == "Image" || e.ChangedItem.PropertyDescriptor.Name=="ImageIndex")
            {
                node.Image = item.GetItemImage();
            }
            else if (e.ChangedItem.PropertyDescriptor.Name == "Name")
            {
                node.Cells[1].Text = item.Name;
            }
        }

        #region PropertyGridSite
        internal class PropertyGridSite : ISite, IServiceProvider
        {
            // Fields
            private IComponent comp;
            private bool inGetService;
            private IServiceProvider sp;

            // Methods
            public PropertyGridSite(IServiceProvider sp, IComponent comp)
            {
                this.sp = sp;
                this.comp = comp;
            }

            public object GetService(Type t)
            {
                if (!this.inGetService && (this.sp != null))
                {
                    try
                    {
                        this.inGetService = true;
                        return this.sp.GetService(t);
                    }
                    finally
                    {
                        this.inGetService = false;
                    }
                }
                return null;
            }

            // Properties
            public IComponent Component
            {
                get
                {
                    return this.comp;
                }
            }

            public IContainer Container
            {
                get
                {
                    return null;
                }
            }

            public bool DesignMode
            {
                get
                {
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return null;
                }
                set
                {
                }
            }
        }
        #endregion

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Form form = this.FindForm();
            if (form != null) form.Close();
        }
    }
}
#endif