using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for RemindForm.
	/// </summary>
	internal class RemindForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.LinkLabel linkOrder;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.LinkLabel linkEmail;
		private System.Windows.Forms.LinkLabel linkHome;
		private System.ComponentModel.IContainer components;

		public RemindForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			label2.Text="(c) "+System.DateTime.Now.Year.ToString()+" by DevComponents, All Rights Reserved.";
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

        static DateTime _LastShown = DateTime.MinValue;
        public new void ShowDialog()
        {
            if (_LastShown != DateTime.MinValue)
            {
                if (_LastShown.Subtract(DateTime.Now).TotalMinutes < 15)
                    return;
            }
            _LastShown = DateTime.Now;
            base.ShowDialog();
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.linkEmail = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.linkHome = new System.Windows.Forms.LinkLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.linkOrder = new System.Windows.Forms.LinkLabel();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(252, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "DevComponents DotNetBar Component";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(307, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "(c) 2001-2004 by DevComponents.com, All Rights Reserved";
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point(10, 242);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(89, 242);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 3;
			this.button2.Text = "Buy Now";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 800;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// linkEmail
			// 
			this.linkEmail.Location = new System.Drawing.Point(52, 200);
			this.linkEmail.Name = "linkEmail";
			this.linkEmail.Size = new System.Drawing.Size(145, 16);
			this.linkEmail.TabIndex = 4;
			this.linkEmail.TabStop = true;
			this.linkEmail.Text = "info@devcomponents.com";
			this.linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEmail_LinkClicked);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 201);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "E-Mail:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 217);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Web:";
			// 
			// linkHome
			// 
			this.linkHome.Location = new System.Drawing.Point(53, 216);
			this.linkHome.Name = "linkHome";
			this.linkHome.Size = new System.Drawing.Size(145, 16);
			this.linkHome.TabIndex = 6;
			this.linkHome.TabStop = true;
			this.linkHome.Text = "www.devcomponents.com";
			this.linkHome.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHome_LinkClicked);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.label7,
																					this.linkOrder,
																					this.label6,
																					this.label5});
			this.groupBox1.Location = new System.Drawing.Point(14, 55);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(294, 135);
			this.groupBox1.TabIndex = 8;
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
			this.linkOrder.Text = "http://www.devcomponents.com/dotnetbar/order.html";
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
			this.label5.Size = new System.Drawing.Size(276, 41);
			this.label5.TabIndex = 0;
			this.label5.Text = "This component is not registered and it is provided for evaluation purposes only." +
				" This message will not appear after you register the component.";
			// 
			// RemindForm
			// 
			this.BackColor=Color.WhiteSmoke;
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(321, 273);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.linkHome,
																		  this.linkEmail,
																		  this.button2,
																		  this.button1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RemindForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Component not registered";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Enabled=false;
			button1.Enabled=true;
			button1.Focus();
		}

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

		private void linkEmail_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("mailto:info@devcomponents.com");
		}

		private void linkOrder_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(linkOrder.Text);
		}
	}
}
