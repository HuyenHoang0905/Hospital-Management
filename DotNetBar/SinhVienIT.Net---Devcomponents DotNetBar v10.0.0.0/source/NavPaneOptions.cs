using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for NavPaneOptions.
	/// </summary>
	public class NavPaneOptions : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Label labelListCaption;
		private NavigationBarContainer m_NavBar=null;
		private System.Windows.Forms.CheckedListBox lbButtons;
		internal System.Windows.Forms.Button cmdMoveUp;
        internal System.Windows.Forms.Button cmdMoveDown;
        internal System.Windows.Forms.Button cmdReset;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdCancel;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NavPaneOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            cmdReset.Visible = false;
		}

		public NavigationBarContainer NavBarContainer
		{
			get {return m_NavBar;}
			set 
			{
				m_NavBar=value;
				ContainerChanged();
			}
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

		private void ContainerChanged()
		{
			lbButtons.Items.Clear();
			if(m_NavBar==null)
				return;
			foreach(BaseItem item in m_NavBar.SubItems)
			{
				lbButtons.Items.Add(item,item.Visible);
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelListCaption = new System.Windows.Forms.Label();
			this.lbButtons = new System.Windows.Forms.CheckedListBox();
			this.cmdMoveUp = new System.Windows.Forms.Button();
			this.cmdMoveDown = new System.Windows.Forms.Button();
			this.cmdReset = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.labelListCaption.AutoSize = true;
			this.labelListCaption.Location = new System.Drawing.Point(8, 8);
			this.labelListCaption.Name = "labelListCaption";
			this.labelListCaption.Size = new System.Drawing.Size(144, 13);
			this.labelListCaption.TabIndex = 0;
			this.labelListCaption.Text = "Display &buttons in this order";
			// 
			// lbButtons
			// 
			this.lbButtons.Location = new System.Drawing.Point(8, 32);
			this.lbButtons.Name = "lbButtons";
			this.lbButtons.Size = new System.Drawing.Size(210, 109);
			this.lbButtons.TabIndex = 1;
			this.lbButtons.SelectedIndexChanged += new System.EventHandler(this.lbButtons_SelectedIndexChanged);
			// 
			// cmdMoveUp
			// 
			this.cmdMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdMoveUp.Location = new System.Drawing.Point(224, 32);
			this.cmdMoveUp.Name = "cmdMoveUp";
			this.cmdMoveUp.TabIndex = 2;
			this.cmdMoveUp.Text = "Move &Up";
			this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
			// 
			// cmdMoveDown
			// 
			this.cmdMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdMoveDown.Location = new System.Drawing.Point(224, 64);
			this.cmdMoveDown.Name = "cmdMoveDown";
			this.cmdMoveDown.TabIndex = 3;
			this.cmdMoveDown.Text = "Move &Down";
			this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
			// 
			// cmdReset
			// 
			this.cmdReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdReset.Location = new System.Drawing.Point(224, 104);
			this.cmdReset.Name = "cmdReset";
			this.cmdReset.TabIndex = 4;
			this.cmdReset.Text = "&Reset";
			this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdOK.Location = new System.Drawing.Point(144, 152);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.TabIndex = 5;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdCancel.Location = new System.Drawing.Point(224, 152);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.TabIndex = 6;
			this.cmdCancel.Text = "Cancel";
			// 
			// NavPaneOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 182);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cmdCancel,
																		  this.cmdOK,
																		  this.cmdReset,
																		  this.cmdMoveDown,
																		  this.cmdMoveUp,
																		  this.lbButtons,
																		  this.labelListCaption});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NavPaneOptions";
			this.ShowInTaskbar = false;
			this.Text = "Navigation Pane Options";
			this.ResumeLayout(false);

		}
		#endregion

		private void lbButtons_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lbButtons.SelectedIndex==0)
				cmdMoveUp.Enabled=false;
			else
				cmdMoveUp.Enabled=true;

			if(lbButtons.SelectedIndex==lbButtons.Items.Count-1)
				cmdMoveDown.Enabled=false;
			else
                cmdMoveDown.Enabled=true;
		}

		private void cmdMoveUp_Click(object sender, System.EventArgs e)
		{
			if(lbButtons.SelectedIndex<0)
				return;
			BaseItem item=lbButtons.SelectedItem as BaseItem;
			int i=lbButtons.SelectedIndex;
            bool isChecked = lbButtons.GetItemChecked(i);
			lbButtons.Items.RemoveAt(i);
			i--;
			lbButtons.Items.Insert(i,item);
            lbButtons.SetItemChecked(i, isChecked);
			lbButtons.SelectedIndex=i;
		}

		private void cmdMoveDown_Click(object sender, System.EventArgs e)
		{
			if(lbButtons.SelectedIndex<0)
				return;
			BaseItem item=lbButtons.SelectedItem as BaseItem;
			int i=lbButtons.SelectedIndex;
            bool isChecked = lbButtons.GetItemChecked(i);
			lbButtons.Items.RemoveAt(i);
			i++;
			lbButtons.Items.Insert(i,item);
			lbButtons.SetItemChecked(i,isChecked);
			lbButtons.SelectedIndex=i;
		}

		private void cmdReset_Click(object sender, System.EventArgs e)
		{
		    
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			m_NavBar.SuspendLayout=true;
			m_NavBar.SubItems._Clear();
			for(int i=0;i<lbButtons.Items.Count;i++)
			{
				BaseItem item=lbButtons.Items[i] as BaseItem;
				item.Visible=lbButtons.GetItemChecked(i);
				m_NavBar.SubItems._Add(item);
			}
			m_NavBar.SuspendLayout=false;
            if (m_NavBar.ContainerControl is BarBaseControl)
                ((BarBaseControl)m_NavBar.ContainerControl).RecalcLayout();
            m_NavBar.OptionsDialogClosed();
		}

	}
}
