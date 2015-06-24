using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.Tree.Design
{
	/// <summary>
	/// Summary description for ComponentNotLicensed.
	/// </summary>
	public class ComponentNotLicensed : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.LinkLabel linkOrder;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkHome;
		private System.Windows.Forms.LinkLabel linkEmail;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		public ComponentNotLicensed()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.linkOrder = new System.Windows.Forms.LinkLabel();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.linkHome = new System.Windows.Forms.LinkLabel();
			this.linkEmail = new System.Windows.Forms.LinkLabel();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.linkOrder);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(11, 54);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(294, 135);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(3, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(285, 12);
			this.label7.TabIndex = 3;
			this.label7.Text = "Order at:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// linkOrder
			// 
			this.linkOrder.Location = new System.Drawing.Point(6, 116);
			this.linkOrder.Name = "linkOrder";
			this.linkOrder.Size = new System.Drawing.Size(280, 15);
			this.linkOrder.TabIndex = 2;
			this.linkOrder.TabStop = true;
			this.linkOrder.Text = "http://www.devcomponents.com/treegx/order.html";
			this.linkOrder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkOrder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOrder_LinkClicked);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(9, 72);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(276, 31);
			this.label6.TabIndex = 1;
			this.label6.Text = "For pricing and licensing information please visit our web site. ";
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.Maroon;
			this.label5.Location = new System.Drawing.Point(8, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(276, 52);
			this.label5.TabIndex = 0;
			this.label5.Text = "This component is not registered and it is provided for evaluation purposes only." +
				" This message will not appear after you register the component.";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(10, 216);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 16);
			this.label4.TabIndex = 16;
			this.label4.Text = "Web:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 200);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 14;
			this.label3.Text = "E-Mail:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(307, 16);
			this.label2.TabIndex = 11;
			this.label2.Text = "(c) 2005-2006 by DevComponents.com, All Rights Reserved";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(9, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(236, 18);
			this.label1.TabIndex = 9;
			this.label1.Text = "DevComponents TreeGX Component";
			// 
			// linkHome
			// 
			this.linkHome.Location = new System.Drawing.Point(50, 215);
			this.linkHome.Name = "linkHome";
			this.linkHome.Size = new System.Drawing.Size(145, 16);
			this.linkHome.TabIndex = 15;
			this.linkHome.TabStop = true;
			this.linkHome.Text = "www.devcomponents.com";
			this.linkHome.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHome_LinkClicked);
			// 
			// linkEmail
			// 
			this.linkEmail.Location = new System.Drawing.Point(49, 199);
			this.linkEmail.Name = "linkEmail";
			this.linkEmail.Size = new System.Drawing.Size(167, 16);
			this.linkEmail.TabIndex = 13;
			this.linkEmail.TabStop = true;
			this.linkEmail.Text = "support@devcomponents.com";
			this.linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEmail_LinkClicked);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(232, 240);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 12;
			this.button2.Text = "Buy Now";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point(152, 240);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 10;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 800;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// ComponentNotLicensed
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.BlanchedAlmond;
			this.ClientSize = new System.Drawing.Size(314, 273);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkHome);
			this.Controls.Add(this.linkEmail);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "ComponentNotLicensed";
			this.Text = "TreeGX License not found";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void button2_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start(linkOrder.Text);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void linkHome_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.devcomponents.com");
		}

		private void linkOrder_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(linkOrder.Text);
		}

		private void linkEmail_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(linkEmail.Text);
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Enabled=false;
			button1.Enabled=true;
			button1.Focus();
		}
	}
}
