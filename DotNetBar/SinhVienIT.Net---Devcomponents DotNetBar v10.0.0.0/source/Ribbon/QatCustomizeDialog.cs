using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Ribbon
{
    public class QatCustomizeDialog : DevComponents.DotNetBar.Office2007Form
    {
        private QatCustomizePanel qatCustomizePanel1;
        internal ButtonX buttonOK;
        internal ButtonX buttonCancel;
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.qatCustomizePanel1 = new DevComponents.DotNetBar.Ribbon.QatCustomizePanel();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // qatCustomizePanel1
            // 
            this.qatCustomizePanel1.BackColor = System.Drawing.Color.Transparent;
            this.qatCustomizePanel1.Location = new System.Drawing.Point(0, 0);
            this.qatCustomizePanel1.Name = "qatCustomizePanel1";
            this.qatCustomizePanel1.Size = new System.Drawing.Size(444, 298);
            this.qatCustomizePanel1.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(285, 297);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(73, 21);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            // 
            // buttonCancel
            // 
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(364, 297);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(73, 21);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            // 
            // QatCustomizeDialog
            // 
            this.ClientSize = new System.Drawing.Size(445, 324);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.qatCustomizePanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Name = "QatCustomizeDialog";
            this.Text = "Customize";
            this.ResumeLayout(false);

        }

        #endregion

        public QatCustomizeDialog()
        {
            InitializeComponent();
            //if (System.Environment.OSVersion.Version.Major >= 6)
            //{
            //    this.ClientSize = new Size(this.ClientSize.Width + 10, this.ClientSize.Height + 10);
            //}
        }

        /// <summary>
        /// Loads the items for the customization into the ribbon control. All Ribbon Bars on the ribbon are enumerated and items
        /// are added if they have CanCustomize=true.
        /// </summary>
        /// <param name="rc">Ribbon control to enumerate.</param>
        public void LoadItems(RibbonControl rc)
        {
            qatCustomizePanel1.LoadItems(rc);
        }

        /// <summary>
        /// Gets reference to the internal Quick Access Toolbar Customization panel.
        /// </summary>
        public QatCustomizePanel QatCustomizePanel
        {
            get { return qatCustomizePanel1; }
        }
    }
}