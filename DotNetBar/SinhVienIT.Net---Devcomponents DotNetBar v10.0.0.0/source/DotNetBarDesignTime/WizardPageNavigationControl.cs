using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for WizardPageNavigationControl.
	/// </summary>
    [ToolboxItem(false)]
    internal class WizardPageNavigationControl : System.Windows.Forms.UserControl
	{
		public System.Windows.Forms.LinkLabel LinkBack;
		public System.Windows.Forms.LinkLabel LinkNext;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WizardPageNavigationControl()
		{
			// This call is required by the Windows.Forms Form Designer.
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.LinkBack = new System.Windows.Forms.LinkLabel();
			this.LinkNext = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// LinkBack
			// 
			this.LinkBack.Dock = System.Windows.Forms.DockStyle.Left;
			this.LinkBack.Name = "LinkBack";
			this.LinkBack.Size = new System.Drawing.Size(45, 16);
			this.LinkBack.TabIndex = 0;
			this.LinkBack.TabStop = true;
			this.LinkBack.Text = "< Back";
			// 
			// LinkNext
			// 
			this.LinkNext.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LinkNext.Location = new System.Drawing.Point(40, 0);
			this.LinkNext.Name = "LinkNext";
			this.LinkNext.Size = new System.Drawing.Size(45, 16);
			this.LinkNext.TabIndex = 1;
			this.LinkNext.TabStop = true;
			this.LinkNext.Text = "Next >";
			// 
			// WizardPageNavigationControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.LinkNext,
																		  this.LinkBack});
			this.Name = "WizardPageNavigationControl";
			this.Size = new System.Drawing.Size(90, 16);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
