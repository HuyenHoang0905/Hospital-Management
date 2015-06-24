using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for SideBarColorSchemePicker.
	/// </summary>
	[ToolboxItem(false)]
	internal class SideBarColorSchemePicker : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		public eSideBarColorScheme SelectedColorScheme=eSideBarColorScheme.Blue;
		private DevComponents.DotNetBar.SideBar sideBar1;
		private DevComponents.DotNetBar.SideBarPanelItem sideBarPanelItem1;
		private DevComponents.DotNetBar.ButtonItem buttonItem1;
		private DevComponents.DotNetBar.ButtonItem buttonItem2;
		private DevComponents.DotNetBar.ButtonItem buttonItem3;
		private DevComponents.DotNetBar.ButtonItem buttonItem4;
		private DevComponents.DotNetBar.SideBarPanelItem sideBarPanelItem2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SideBarColorSchemePicker()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SideBarColorSchemePicker));
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.sideBar1 = new DevComponents.DotNetBar.SideBar();
			this.sideBarPanelItem1 = new DevComponents.DotNetBar.SideBarPanelItem();
			this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
			this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
			this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
			this.buttonItem4 = new DevComponents.DotNetBar.ButtonItem();
			this.sideBarPanelItem2 = new DevComponents.DotNetBar.SideBarPanelItem();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 24);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(152, 238);
			this.listBox1.TabIndex = 1;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Available Color Schemes:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(176, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(208, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Example of Color Scheme Applied:";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(232, 272);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 4;
			this.button1.Text = "OK";
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(312, 272);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 5;
			this.button2.Text = "Cancel";
			// 
			// sideBar1
			// 
			this.sideBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
			this.sideBar1.Appearance = DevComponents.DotNetBar.eSideBarAppearance.Flat;
			this.sideBar1.BorderStyle = DevComponents.DotNetBar.eBorderType.None;
			this.sideBar1.ColorScheme.BarBackground = System.Drawing.Color.FromArgb(((System.Byte)(239)), ((System.Byte)(237)), ((System.Byte)(222)));
			this.sideBar1.ColorScheme.BarBackground2 = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.BarCaptionBackground = System.Drawing.Color.FromArgb(((System.Byte)(172)), ((System.Byte)(168)), ((System.Byte)(153)));
			this.sideBar1.ColorScheme.BarCaptionInactiveBackground = System.Drawing.SystemColors.Control;
			this.sideBar1.ColorScheme.BarCaptionInactiveText = System.Drawing.SystemColors.ControlText;
			this.sideBar1.ColorScheme.BarCaptionText = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.sideBar1.ColorScheme.BarDockedBorder = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.BarFloatingBorder = System.Drawing.Color.FromArgb(((System.Byte)(172)), ((System.Byte)(168)), ((System.Byte)(153)));
			this.sideBar1.ColorScheme.BarPopupBackground = System.Drawing.Color.FromArgb(((System.Byte)(252)), ((System.Byte)(252)), ((System.Byte)(249)));
			this.sideBar1.ColorScheme.BarPopupBorder = System.Drawing.Color.FromArgb(((System.Byte)(138)), ((System.Byte)(134)), ((System.Byte)(122)));
			this.sideBar1.ColorScheme.BarStripeColor = System.Drawing.Color.FromArgb(((System.Byte)(191)), ((System.Byte)(188)), ((System.Byte)(177)));
			this.sideBar1.ColorScheme.CustomizeBackground = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.CustomizeBackground2 = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.CustomizeText = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.ItemBackground = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.ItemCheckedBackground = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.ItemCheckedBackground2 = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.ItemCheckedBorder = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(128)));
			this.sideBar1.ColorScheme.ItemCheckedText = System.Drawing.Color.Black;
			this.sideBar1.ColorScheme.ItemDesignTimeBorder = System.Drawing.SystemColors.Highlight;
			this.sideBar1.ColorScheme.ItemDisabledBackground = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.ItemDisabledText = System.Drawing.SystemColors.ControlDark;
			this.sideBar1.ColorScheme.ItemExpandedBackground = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(142)), ((System.Byte)(75)));
			this.sideBar1.ColorScheme.ItemExpandedBackground2 = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(207)), ((System.Byte)(139)));
			this.sideBar1.ColorScheme.ItemExpandedShadow = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.ItemExpandedText = System.Drawing.Color.Black;
			this.sideBar1.ColorScheme.ItemHotBackground = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(244)), ((System.Byte)(204)));
			this.sideBar1.ColorScheme.ItemHotBackground2 = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(209)), ((System.Byte)(147)));
			this.sideBar1.ColorScheme.ItemHotBorder = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(128)));
			this.sideBar1.ColorScheme.ItemHotText = System.Drawing.Color.Black;
			this.sideBar1.ColorScheme.ItemPressedBackground = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(142)), ((System.Byte)(75)));
			this.sideBar1.ColorScheme.ItemPressedBackground2 = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(207)), ((System.Byte)(139)));
			this.sideBar1.ColorScheme.ItemPressedBorder = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(128)));
			this.sideBar1.ColorScheme.ItemPressedText = System.Drawing.Color.Black;
			this.sideBar1.ColorScheme.ItemSeparator = System.Drawing.Color.FromArgb(((System.Byte)(197)), ((System.Byte)(194)), ((System.Byte)(184)));
			this.sideBar1.ColorScheme.ItemSeparatorShade = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.ItemText = System.Drawing.Color.Black;
			this.sideBar1.ColorScheme.MenuBackground = System.Drawing.Color.FromArgb(((System.Byte)(232)), ((System.Byte)(232)), ((System.Byte)(232)));
			this.sideBar1.ColorScheme.MenuBackground2 = System.Drawing.Color.White;
			this.sideBar1.ColorScheme.MenuBarBackground = System.Drawing.SystemColors.Control;
			this.sideBar1.ColorScheme.MenuBarBackground2 = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.MenuBorder = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(128)));
			this.sideBar1.ColorScheme.MenuSide = System.Drawing.Color.FromArgb(((System.Byte)(94)), ((System.Byte)(137)), ((System.Byte)(207)));
			this.sideBar1.ColorScheme.MenuSide2 = System.Drawing.Color.Empty;
			this.sideBar1.ColorScheme.MenuUnusedBackground = System.Drawing.Color.FromArgb(((System.Byte)(252)), ((System.Byte)(252)), ((System.Byte)(249)));
			this.sideBar1.ColorScheme.MenuUnusedSide = System.Drawing.Color.FromArgb(((System.Byte)(230)), ((System.Byte)(227)), ((System.Byte)(210)));
			this.sideBar1.ColorScheme.MenuUnusedSide2 = System.Drawing.Color.Empty;
			this.sideBar1.ExpandedPanel = this.sideBarPanelItem1;
			this.sideBar1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.sideBar1.Location = new System.Drawing.Point(176, 24);
			this.sideBar1.Name = "sideBar1";
			this.sideBar1.Panels.AddRange(new DevComponents.DotNetBar.BaseItem[] {
																					 this.sideBarPanelItem1,
																					 this.sideBarPanelItem2});
			this.sideBar1.Size = new System.Drawing.Size(208, 240);
			this.sideBar1.TabIndex = 6;
			this.sideBar1.TabStop = false;
			// 
			// sideBarPanelItem1
			// 
			this.sideBarPanelItem1.BackgroundStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(232)), ((System.Byte)(232)), ((System.Byte)(232)));
			this.sideBarPanelItem1.BackgroundStyle.BackColor2.Color = System.Drawing.Color.White;
			this.sideBarPanelItem1.BackgroundStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.sideBarPanelItem1.BackgroundStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(59)), ((System.Byte)(97)), ((System.Byte)(156)));
			this.sideBarPanelItem1.HeaderHotStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(133)), ((System.Byte)(171)), ((System.Byte)(228)));
			this.sideBarPanelItem1.HeaderHotStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(236)), ((System.Byte)(254)));
			this.sideBarPanelItem1.HeaderHotStyle.GradientAngle = 90;
			this.sideBarPanelItem1.HeaderSideHotStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(133)), ((System.Byte)(171)), ((System.Byte)(228)));
			this.sideBarPanelItem1.HeaderSideHotStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(236)), ((System.Byte)(254)));
			this.sideBarPanelItem1.HeaderSideHotStyle.GradientAngle = 90;
			this.sideBarPanelItem1.HeaderSideStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(220)), ((System.Byte)(248)));
			this.sideBarPanelItem1.HeaderSideStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(94)), ((System.Byte)(137)), ((System.Byte)(207)));
			this.sideBarPanelItem1.HeaderSideStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.sideBarPanelItem1.HeaderSideStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(59)), ((System.Byte)(97)), ((System.Byte)(156)));
			this.sideBarPanelItem1.HeaderSideStyle.BorderSide = ((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Top) 
				| DevComponents.DotNetBar.eBorderSide.Bottom);
			this.sideBarPanelItem1.HeaderSideStyle.GradientAngle = 90;
			this.sideBarPanelItem1.HeaderStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(236)), ((System.Byte)(254)));
			this.sideBarPanelItem1.HeaderStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(133)), ((System.Byte)(171)), ((System.Byte)(228)));
			this.sideBarPanelItem1.HeaderStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.sideBarPanelItem1.HeaderStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(59)), ((System.Byte)(97)), ((System.Byte)(156)));
			this.sideBarPanelItem1.HeaderStyle.BorderSide = ((DevComponents.DotNetBar.eBorderSide.Right | DevComponents.DotNetBar.eBorderSide.Top) 
				| DevComponents.DotNetBar.eBorderSide.Bottom);
			this.sideBarPanelItem1.HeaderStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.sideBarPanelItem1.HeaderStyle.ForeColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(51)), ((System.Byte)(102)));
			this.sideBarPanelItem1.HeaderStyle.GradientAngle = 90;
			this.sideBarPanelItem1.Icon = ((System.Drawing.Icon)(resources.GetObject("sideBarPanelItem1.Icon")));
			this.sideBarPanelItem1.Name = "sideBarPanelItem1";
			this.sideBarPanelItem1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
																								this.buttonItem1,
																								this.buttonItem2,
																								this.buttonItem3,
																								this.buttonItem4});
			this.sideBarPanelItem1.Text = "My Mail Folders";
			// 
			// buttonItem1
			// 
			this.buttonItem1.Icon = ((System.Drawing.Icon)(resources.GetObject("buttonItem1.Icon")));
			this.buttonItem1.Name = "buttonItem1";
			this.buttonItem1.Text = "Inbox";
			// 
			// buttonItem2
			// 
			this.buttonItem2.Icon = ((System.Drawing.Icon)(resources.GetObject("buttonItem2.Icon")));
			this.buttonItem2.Name = "buttonItem2";
			this.buttonItem2.Text = "Sent Items";
			// 
			// buttonItem3
			// 
			this.buttonItem3.Icon = ((System.Drawing.Icon)(resources.GetObject("buttonItem3.Icon")));
			this.buttonItem3.Name = "buttonItem3";
			this.buttonItem3.Text = "Blocked Junk E-Mail";
			// 
			// buttonItem4
			// 
			this.buttonItem4.BeginGroup = true;
			this.buttonItem4.Icon = ((System.Drawing.Icon)(resources.GetObject("buttonItem4.Icon")));
			this.buttonItem4.Name = "buttonItem4";
			this.buttonItem4.Text = "Actions";
			// 
			// sideBarPanelItem2
			// 
			this.sideBarPanelItem2.BackgroundStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(232)), ((System.Byte)(232)), ((System.Byte)(232)));
			this.sideBarPanelItem2.BackgroundStyle.BackColor2.Color = System.Drawing.Color.White;
			this.sideBarPanelItem2.BackgroundStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.sideBarPanelItem2.BackgroundStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(59)), ((System.Byte)(97)), ((System.Byte)(156)));
			this.sideBarPanelItem2.HeaderHotStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(133)), ((System.Byte)(171)), ((System.Byte)(228)));
			this.sideBarPanelItem2.HeaderHotStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(236)), ((System.Byte)(254)));
			this.sideBarPanelItem2.HeaderHotStyle.GradientAngle = 90;
			this.sideBarPanelItem2.HeaderSideHotStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(133)), ((System.Byte)(171)), ((System.Byte)(228)));
			this.sideBarPanelItem2.HeaderSideHotStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(236)), ((System.Byte)(254)));
			this.sideBarPanelItem2.HeaderSideHotStyle.GradientAngle = 90;
			this.sideBarPanelItem2.HeaderSideStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(220)), ((System.Byte)(248)));
			this.sideBarPanelItem2.HeaderSideStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(94)), ((System.Byte)(137)), ((System.Byte)(207)));
			this.sideBarPanelItem2.HeaderSideStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.sideBarPanelItem2.HeaderSideStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(59)), ((System.Byte)(97)), ((System.Byte)(156)));
			this.sideBarPanelItem2.HeaderSideStyle.BorderSide = ((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Top) 
				| DevComponents.DotNetBar.eBorderSide.Bottom);
			this.sideBarPanelItem2.HeaderSideStyle.GradientAngle = 90;
			this.sideBarPanelItem2.HeaderStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(236)), ((System.Byte)(254)));
			this.sideBarPanelItem2.HeaderStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((System.Byte)(133)), ((System.Byte)(171)), ((System.Byte)(228)));
			this.sideBarPanelItem2.HeaderStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.sideBarPanelItem2.HeaderStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(59)), ((System.Byte)(97)), ((System.Byte)(156)));
			this.sideBarPanelItem2.HeaderStyle.BorderSide = ((DevComponents.DotNetBar.eBorderSide.Right | DevComponents.DotNetBar.eBorderSide.Top) 
				| DevComponents.DotNetBar.eBorderSide.Bottom);
			this.sideBarPanelItem2.HeaderStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.sideBarPanelItem2.HeaderStyle.ForeColor.Color = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(51)), ((System.Byte)(102)));
			this.sideBarPanelItem2.HeaderStyle.GradientAngle = 90;
			this.sideBarPanelItem2.Icon = ((System.Drawing.Icon)(resources.GetObject("sideBarPanelItem2.Icon")));
			this.sideBarPanelItem2.Name = "sideBarPanelItem2";
			this.sideBarPanelItem2.Text = "My Calendar";
			// 
			// SideBarColorSchemePicker
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 304);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.sideBar1,
																		  this.button2,
																		  this.button1,
																		  this.label2,
																		  this.label1,
																		  this.listBox1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SideBarColorSchemePicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SideBar Color Scheme Picker";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SideBarColorSchemePicker_Closing);
			this.Load += new System.EventHandler(this.SideBarColorSchemePicker_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void SideBarColorSchemePicker_Load(object sender, System.EventArgs e)
		{
			// Load all color schemes in ListBox
			foreach(string s in Enum.GetNames(typeof(eSideBarColorScheme)))
			{
				listBox1.Items.Add(s);
			}
			listBox1.SelectedIndex=0;
		}

		private void SideBarColorSchemePicker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(listBox1.SelectedIndex<0 && this.DialogResult==DialogResult.OK)
			{
				e.Cancel=true;
				MessageBox.Show("Please select color scheme to apply.","Color Scheme Picker",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			eSideBarColorScheme scheme;
			if(listBox1.SelectedIndex<0)
				return;
			string sSel=listBox1.SelectedItem.ToString();
			scheme=(eSideBarColorScheme)Enum.Parse(typeof(eSideBarColorScheme),sSel,false);
			SelectedColorScheme=scheme;
			sideBar1.PredefinedColorScheme=scheme;
		}
	}
}
