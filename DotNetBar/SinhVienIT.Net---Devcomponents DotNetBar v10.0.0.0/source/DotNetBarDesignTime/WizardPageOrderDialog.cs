using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    [ToolboxItem(false)]
    internal class WizardPageOrderDialog : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private Bar bar1;
        private ButtonItem buttonPageUp;
        private ButtonItem buttonPageDown;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderTitle;
        private System.Windows.Forms.ColumnHeader columnHeaderDesc;
        private Wizard m_Wizard = null;
        private ColumnHeader columnInterior;
        internal bool OrderChanged = false;


        public WizardPageOrderDialog()
        {
            InitializeComponent();
        }

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTitle = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDesc = new System.Windows.Forms.ColumnHeader();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.buttonPageUp = new DevComponents.DotNetBar.ButtonItem();
            this.buttonPageDown = new DevComponents.DotNetBar.ButtonItem();
            this.columnInterior = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnInterior,
            this.columnHeaderTitle,
            this.columnHeaderDesc});
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 31);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(488, 233);
            this.listView1.TabIndex = 0;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.Resize += new System.EventHandler(this.listView1_Resize);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 100;
            // 
            // columnHeaderTitle
            // 
            this.columnHeaderTitle.Text = "Page Title";
            this.columnHeaderTitle.Width = 150;
            // 
            // columnHeaderDesc
            // 
            this.columnHeaderDesc.Text = "Page Description";
            this.columnHeaderDesc.Width = 187;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(352, 270);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(73, 25);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(427, 270);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(73, 25);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            // 
            // bar1
            // 
            this.bar1.BackgroundImageAlpha = ((byte)(255));
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonPageUp,
            this.buttonPageDown});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(512, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.bar1.TabIndex = 3;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // buttonPageUp
            // 
            this.buttonPageUp.Name = "buttonPageUp";
            this.buttonPageUp.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlUp);
            this.buttonPageUp.Text = "Move Page Up";
            this.buttonPageUp.Tooltip = "Change selected page order by moving it up";
            this.buttonPageUp.Click += new System.EventHandler(this.buttonPageUp_Click);
            // 
            // buttonPageDown
            // 
            this.buttonPageDown.Name = "buttonPageDown";
            this.buttonPageDown.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlDown);
            this.buttonPageDown.Text = "Move Page Down";
            this.buttonPageDown.Tooltip = "Change selected page order by moving it down";
            this.buttonPageDown.Click += new System.EventHandler(this.buttonPageDown_Click);
            // 
            // columnInterior
            // 
            this.columnInterior.Text = "Interior";
            this.columnInterior.Width = 47;
            // 
            // WizardPageOrderDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(512, 303);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.listView1);
            this.MinimizeBox = false;
            this.Name = "WizardPageOrderDialog";
            this.Text = "Wizard Pages";
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public void SetWizard(Wizard w)
        {
            m_Wizard = w;
            foreach(WizardPage page in m_Wizard.WizardPages)
            {
                ListViewItem item = new ListViewItem(new string[] { page.Name, (page.InteriorPage?"Yes":"No") ,page.PageTitle, page.PageDescription });
                listView1.Items.Add(item);
            }
        }

        private void buttonPageUp_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            int i=listView1.SelectedItems[0].Index;
            if (i == 0)
                return;
            ListViewItem item = listView1.SelectedItems[0];
            listView1.Items.Remove(item);
            listView1.Items.Insert(i - 1, item);
            OrderChanged = true;
        }

        private void buttonPageDown_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;

            if (listView1.SelectedItems.Count != 1)
                return;
            int i = listView1.SelectedItems[0].Index;
            if (i == listView1.Items.Count - 1)
                return;
            ListViewItem item = listView1.SelectedItems[0];
            listView1.Items.Remove(item);
            listView1.Items.Insert(i + 1, item);

            OrderChanged = false;
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            int i = listView1.Width - columnHeaderName.Width - columnInterior.Width - columnHeaderTitle.Width - 6;
            if(i>64)
                columnHeaderDesc.Width=i;
        }

        internal string SelectedPageName
        {
            get
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    return listView1.SelectedItems[0].Text;
                }
                return "";
            }
        }

        internal string[] OrderedPageNames
        {
            get
            {
                string[] names = new string[listView1.Items.Count];
                for (int i = 0; i < names.Length; i++)
                    names[i] = listView1.Items[i].Text;
                return names;
            }
        }
        }
}