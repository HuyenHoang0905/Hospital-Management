using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for NewToolbar.
	/// </summary>
	[ToolboxItem(false)]
	internal class ToolbarName : System.Windows.Forms.Form
	{
		public bool RenameDialog=false;
		public System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ToolbarName()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdOK.Location = new System.Drawing.Point(113, 56);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(80, 24);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "barname_ok";
			// 
			// cmdCancel
			// 
			this.cmdCancel.CausesValidation = false;
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdCancel.Location = new System.Drawing.Point(197, 56);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(80, 24);
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Text = "barname_cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(272, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "barname_name";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(8, 28);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(268, 20);
			this.txtName.TabIndex = 1;
			this.txtName.Text = "";
			this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.OnOK);
			// 
			// ToolbarName
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(286, 87);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cmdCancel,
																		  this.cmdOK,
																		  this.label1,
																		  this.txtName});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ToolbarName";
			this.ShowInTaskbar = false;
			this.Text = "barname_caption";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		#endregion

		private void OnLoad(object sender, System.EventArgs e)
		{
			// Load localized strings...
			DotNetBarManager manager=null;
			if(this.Owner is frmCustomize)
				manager=((frmCustomize)this.Owner).GetDotNetBarManager();
			using(LocalizationManager lm=new LocalizationManager(manager))
			{
				foreach(Control ctrl in this.Controls)
				{
					if(ctrl is TextBox)
						continue;
					ctrl.Text=lm.GetLocalizedString(ctrl.Text);
				}
				
				if(this.RenameDialog)
					this.Text=lm.GetLocalizedString(LocalizationKeys.RenameBarDialogCaption);
				else
					this.Text=lm.GetLocalizedString(this.Text);
			}
			manager=null;
		}

		private void OnOK(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(txtName.Text.Trim()=="")
			{
				DotNetBarManager manager=null;
				if(this.Owner is frmCustomize)
					manager=((frmCustomize)this.Owner).GetDotNetBarManager();
				using(LocalizationManager lm=new LocalizationManager(manager))
				{
					MessageBox.Show(lm.GetLocalizedString(LocalizationKeys.BarEditDialogInvalidNameMessage));
				}
				manager=null;
				e.Cancel=true;
			}
		}
	}
}
