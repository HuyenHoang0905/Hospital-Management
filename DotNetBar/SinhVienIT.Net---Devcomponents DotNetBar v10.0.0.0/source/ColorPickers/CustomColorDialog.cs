using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using DevComponents.DotNetBar.ColorPickers;

namespace DevComponents.DotNetBar.ColorPickerItem
{
    [ToolboxItem(false)]
	internal class CustomColorDialog : DevComponents.DotNetBar.Office2007Form
	{
		#region Private Variables
		internal ButtonX buttonOK;
		internal ButtonX buttonCancel;
		internal System.Windows.Forms.Label labelNewColor;
		internal System.Windows.Forms.Label labelCurrentColor;
		
		private Color m_CurrentColor = Color.Black;
		private Color m_NewColor = Color.Empty;
		private ColorCombControl colorCombControl1;
		internal System.Windows.Forms.Label labelStandardColors;
		internal System.Windows.Forms.Label labelCustomColors;
		internal System.Windows.Forms.Label labelGreen;
		internal System.Windows.Forms.Label labelBlue;
		private DevComponents.DotNetBar.TabControl tabControl2;
		internal DevComponents.DotNetBar.TabItem tabItemStandard;
		private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        internal DevComponents.DotNetBar.TabItem tabItemCustom;
		private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
		private CustomColorBlender customColorBlender3;
		internal System.Windows.Forms.Label labelRed;
		internal System.Windows.Forms.Label labelColorModel;
        internal ComboBoxEx comboColorModel;
#if FRAMEWORK20
        private IntegerInput numericBlue;
        private IntegerInput numericGreen;
        private IntegerInput numericRed;
#else
        private System.Windows.Forms.NumericUpDown numericBlue;
		private System.Windows.Forms.NumericUpDown numericGreen;
		private System.Windows.Forms.NumericUpDown numericRed;
#endif
		private ColorContrastControl colorContrastControl1;
		private ColorSelectionPreviewControl colorSelectionPreview;
		private System.ComponentModel.IContainer components;
		#endregion
		
		#region Constructor, Dispose
		public CustomColorDialog()
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
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.labelStandardColors = new System.Windows.Forms.Label();
			this.colorCombControl1 = new ColorCombControl();
			this.labelBlue = new System.Windows.Forms.Label();
			this.labelGreen = new System.Windows.Forms.Label();
#if (FRAMEWORK20)
            this.numericBlue = new IntegerInput();
            this.numericGreen = new IntegerInput();
            this.numericRed = new IntegerInput();
#else
            this.numericBlue = new System.Windows.Forms.NumericUpDown();
			this.numericGreen = new System.Windows.Forms.NumericUpDown();
			this.numericRed = new System.Windows.Forms.NumericUpDown();
#endif
            this.labelCustomColors = new System.Windows.Forms.Label();
			this.buttonOK = new ButtonX();
			this.buttonCancel = new ButtonX();
			this.labelNewColor = new System.Windows.Forms.Label();
			this.labelCurrentColor = new System.Windows.Forms.Label();
			this.tabControl2 = new DevComponents.DotNetBar.TabControl();
			this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
			this.tabItemStandard = new DevComponents.DotNetBar.TabItem(this.components);
			this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
			this.colorContrastControl1 = new ColorContrastControl();
			this.customColorBlender3 = new CustomColorBlender();
			this.comboColorModel = new ComboBoxEx();
			this.labelRed = new System.Windows.Forms.Label();
			this.labelColorModel = new System.Windows.Forms.Label();
			this.tabItemCustom = new DevComponents.DotNetBar.TabItem(this.components);
			this.colorSelectionPreview = new ColorSelectionPreviewControl();
			((System.ComponentModel.ISupportInitialize)(this.numericBlue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericGreen)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericRed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tabControl2)).BeginInit();
			this.tabControl2.SuspendLayout();
			this.tabControlPanel1.SuspendLayout();
			this.tabControlPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.labelStandardColors.BackColor = System.Drawing.Color.Transparent;
			this.labelStandardColors.Dock = System.Windows.Forms.DockStyle.Top;
			this.labelStandardColors.Location = new System.Drawing.Point(1, 1);
            this.labelStandardColors.Name = "labelStandardColors";
			this.labelStandardColors.Size = new System.Drawing.Size(224, 19);
			this.labelStandardColors.TabIndex = 1;
			this.labelStandardColors.Text = " Colors:";
			this.labelStandardColors.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// colorCombControl1
			// 
			this.colorCombControl1.BackColor = System.Drawing.Color.Transparent;
			this.colorCombControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorCombControl1.Location = new System.Drawing.Point(1, 20);
			this.colorCombControl1.Name = "colorCombControl1";
			this.colorCombControl1.Size = new System.Drawing.Size(224, 267);
			this.colorCombControl1.TabIndex = 0;
			this.colorCombControl1.SelectedColorChanged += new System.EventHandler(this.colorCombControl1_SelectedColorChanged);
			// 
			// label8
			// 
			this.labelBlue.BackColor = System.Drawing.Color.Transparent;
			this.labelBlue.Location = new System.Drawing.Point(4, 256);
            this.labelBlue.Name = "labelBlue";
			this.labelBlue.Size = new System.Drawing.Size(90, 16);
			this.labelBlue.TabIndex = 10;
			this.labelBlue.Text = "&Blue:";
			// 
			// label7
			// 
			this.labelGreen.BackColor = System.Drawing.Color.Transparent;
			this.labelGreen.Location = new System.Drawing.Point(4, 232);
            this.labelGreen.Name = "labelGreen";
			this.labelGreen.Size = new System.Drawing.Size(90, 16);
			this.labelGreen.TabIndex = 8;
			this.labelGreen.Text = "&Green:";
			// 
			// numericBlue
			// 
			this.numericBlue.Location = new System.Drawing.Point(98, 260);
#if (FRAMEWORK20)
            this.numericBlue.MaxValue = 255;
            this.numericBlue.MinValue = 0;
            this.numericBlue.ShowUpDown = true;
            this.numericBlue.AllowEmptyState = false;
#else
            this.numericBlue.Maximum = new System.Decimal(new int[] {
																		255,
																		0,
																		0,
																		0});
#endif
            this.numericBlue.Name = "numericBlue";
			this.numericBlue.Size = new System.Drawing.Size(47, 20);
			this.numericBlue.TabIndex = 11;
			this.numericBlue.ValueChanged += new System.EventHandler(this.numericRGBValueChanged);
			// 
			// numericGreen
			// 
			this.numericGreen.Location = new System.Drawing.Point(98, 236);
#if (FRAMEWORK20)
            this.numericGreen.MaxValue = 255;
            this.numericGreen.MinValue = 0;
            this.numericGreen.ShowUpDown = true;
            this.numericGreen.AllowEmptyState = false;
#else
            this.numericGreen.Maximum = new System.Decimal(new int[] {
																		255,
																		0,
																		0,
																		0});
#endif
            this.numericGreen.Name = "numericGreen";
			this.numericGreen.Size = new System.Drawing.Size(47, 20);
			this.numericGreen.TabIndex = 9;
			this.numericGreen.ValueChanged += new System.EventHandler(this.numericRGBValueChanged);
			// 
			// numericRed
			// 
			this.numericRed.Location = new System.Drawing.Point(98, 212);
#if (FRAMEWORK20)
            this.numericRed.MaxValue = 255;
            this.numericRed.MinValue = 0;
            this.numericRed.ShowUpDown = true;
            this.numericRed.AllowEmptyState = false;
#else
            this.numericRed.Maximum = new System.Decimal(new int[] {
																		255,
																		0,
																		0,
																		0});
#endif
            this.numericRed.Name = "numericRed";
			this.numericRed.Size = new System.Drawing.Size(47, 20);
			this.numericRed.TabIndex = 7;
			this.numericRed.ValueChanged += new System.EventHandler(this.numericRGBValueChanged);
			// 
			// label4
			// 
			this.labelCustomColors.BackColor = System.Drawing.Color.Transparent;
			this.labelCustomColors.Dock = System.Windows.Forms.DockStyle.Top;
			this.labelCustomColors.Location = new System.Drawing.Point(1, 1);
            this.labelCustomColors.Name = "labelCustomColors";
			this.labelCustomColors.Size = new System.Drawing.Size(224, 20);
			this.labelCustomColors.TabIndex = 2;
			this.labelCustomColors.Text = " Colors:";
			this.labelCustomColors.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// button1
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(242, 6);
            this.buttonOK.ColorTable = eButtonColor.OrangeWithBackground;
            this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(74, 24);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			// 
			// button2
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(242, 36);
            this.buttonCancel.ColorTable = eButtonColor.OrangeWithBackground;
            this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(74, 24);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			// 
			// label1
			// 
			this.labelNewColor.Location = new System.Drawing.Point(256, 232);
            this.labelNewColor.Name = "labelNewColor";
			this.labelNewColor.Size = new System.Drawing.Size(44, 12);
			this.labelNewColor.TabIndex = 4;
			this.labelNewColor.Text = "New";
			this.labelNewColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.labelCurrentColor.Location = new System.Drawing.Point(256, 306);
            this.labelCurrentColor.Name = "labelCurrentColor";
			this.labelCurrentColor.Size = new System.Drawing.Size(44, 16);
			this.labelCurrentColor.TabIndex = 5;
			this.labelCurrentColor.Text = "Current";
			this.labelCurrentColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabControl2
			// 
            this.tabControl2.BackColor = Color.Transparent;
            this.tabControl2.CanReorderTabs = true;
			this.tabControl2.ColorScheme.TabBackground = Color.Transparent;
			this.tabControl2.Controls.Add(this.tabControlPanel2);
			this.tabControl2.Controls.Add(this.tabControlPanel1);
			this.tabControl2.FixedTabSize = new System.Drawing.Size(60, 0);
			this.tabControl2.Location = new System.Drawing.Point(8, 8);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.tabControl2.SelectedTabIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(226, 314);
			this.tabControl2.Style = DevComponents.DotNetBar.eTabStripStyle.VS2005;
			this.tabControl2.TabIndex = 6;
			this.tabControl2.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
			this.tabControl2.Tabs.Add(this.tabItemStandard);
			this.tabControl2.Tabs.Add(this.tabItemCustom);
			this.tabControl2.ThemeAware = true;
			// 
			// tabControlPanel1
			// 
			this.tabControlPanel1.Controls.Add(this.colorCombControl1);
			this.tabControlPanel1.Controls.Add(this.labelStandardColors);
			this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlPanel1.DockPadding.All = 1;
			this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
			this.tabControlPanel1.Name = "tabControlPanel1";
			this.tabControlPanel1.Size = new System.Drawing.Size(226, 288);
			this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.SystemColors.Control;
			this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
				| DevComponents.DotNetBar.eBorderSide.Bottom)));
			this.tabControlPanel1.Style.GradientAngle = 90;
			this.tabControlPanel1.TabIndex = 1;
			this.tabControlPanel1.TabItem = this.tabItemStandard;
			this.tabControlPanel1.ThemeAware = true;
			// 
			// tabItem1
			// 
			this.tabItemStandard.AttachedControl = this.tabControlPanel1;
			this.tabItemStandard.CloseButtonBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.tabItemStandard.Name = "tabItemStandard";
			this.tabItemStandard.Text = "Standard";
			// 
			// tabControlPanel2
			// 
			this.tabControlPanel2.Controls.Add(this.colorContrastControl1);
			this.tabControlPanel2.Controls.Add(this.numericGreen);
			this.tabControlPanel2.Controls.Add(this.labelGreen);
			this.tabControlPanel2.Controls.Add(this.numericRed);
			this.tabControlPanel2.Controls.Add(this.customColorBlender3);
			this.tabControlPanel2.Controls.Add(this.comboColorModel);
			this.tabControlPanel2.Controls.Add(this.labelRed);
			this.tabControlPanel2.Controls.Add(this.labelColorModel);
			this.tabControlPanel2.Controls.Add(this.numericBlue);
			this.tabControlPanel2.Controls.Add(this.labelBlue);
			this.tabControlPanel2.Controls.Add(this.labelCustomColors);
			this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlPanel2.DockPadding.All = 1;
			this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
			this.tabControlPanel2.Name = "tabControlPanel2";
			this.tabControlPanel2.Size = new System.Drawing.Size(226, 288);
			this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.SystemColors.Control;
			this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
				| DevComponents.DotNetBar.eBorderSide.Bottom)));
			this.tabControlPanel2.Style.GradientAngle = 90;
			this.tabControlPanel2.TabIndex = 2;
			this.tabControlPanel2.TabItem = this.tabItemCustom;
			this.tabControlPanel2.ThemeAware = true;
			// 
			// colorContrastControl1
			// 
			this.colorContrastControl1.BackColor = System.Drawing.Color.Transparent;
			this.colorContrastControl1.Location = new System.Drawing.Point(188, 28);
			this.colorContrastControl1.Name = "colorContrastControl1";
			this.colorContrastControl1.SelectedColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
			this.colorContrastControl1.Size = new System.Drawing.Size(32, 152);
			this.colorContrastControl1.TabIndex = 12;
			this.colorContrastControl1.SelectedColorChanged += new System.EventHandler(this.colorContrastControl1_SelectedColorChanged);
			// 
			// customColorBlender3
			// 
			this.customColorBlender3.Location = new System.Drawing.Point(8, 28);
			this.customColorBlender3.Name = "customColorBlender3";
			this.customColorBlender3.Size = new System.Drawing.Size(174, 152);
			this.customColorBlender3.TabIndex = 3;
			this.customColorBlender3.SelectedColorChanged += new System.EventHandler(this.customColorBlender3_SelectedColorChanged);
			// 
			// comboColorModel
			// 
            this.comboColorModel.DrawMode = DrawMode.OwnerDrawFixed;
			this.comboColorModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboColorModel.Items.AddRange(new object[] {
																 "RGB"});
			this.comboColorModel.Location = new System.Drawing.Point(98, 188);
			this.comboColorModel.Name = "comboColorModel";
			this.comboColorModel.Size = new System.Drawing.Size(89, 21);
			this.comboColorModel.TabIndex = 5;
			// 
			// label9
			// 
			this.labelRed.BackColor = System.Drawing.Color.Transparent;
			this.labelRed.Location = new System.Drawing.Point(4, 212);
            this.labelRed.Name = "labelRed";
			this.labelRed.Size = new System.Drawing.Size(90, 16);
			this.labelRed.TabIndex = 6;
			this.labelRed.Text = "&Red:";
			// 
			// label10
			// 
			this.labelColorModel.BackColor = System.Drawing.Color.Transparent;
			this.labelColorModel.Location = new System.Drawing.Point(4, 192);
            this.labelColorModel.Name = "labelColorModel";
			this.labelColorModel.Size = new System.Drawing.Size(90, 16);
			this.labelColorModel.TabIndex = 4;
			this.labelColorModel.Text = "Color Mo&del:";
			// 
			// tabItem2
			// 
			this.tabItemCustom.AttachedControl = this.tabControlPanel2;
			this.tabItemCustom.CloseButtonBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.tabItemCustom.Name = "tabItemCustom";
			this.tabItemCustom.Text = "Custom";
			// 
			// colorSelectionPreview
			// 
			this.colorSelectionPreview.CurrentColor = System.Drawing.Color.Black;
			this.colorSelectionPreview.Location = new System.Drawing.Point(252, 252);
			this.colorSelectionPreview.Name = "colorSelectionPreview";
			this.colorSelectionPreview.NewColor = System.Drawing.Color.Empty;
			this.colorSelectionPreview.Size = new System.Drawing.Size(56, 48);
			this.colorSelectionPreview.TabIndex = 7;
			// 
			// CustomColorDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(324, 326);
			this.Controls.Add(this.colorSelectionPreview);
			this.Controls.Add(this.tabControl2);
			this.Controls.Add(this.labelCurrentColor);
			this.Controls.Add(this.labelNewColor);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomColorDialog";
			this.ShowInTaskbar = false;
			this.Text = "Colors";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericBlue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericGreen)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericRed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tabControl2)).EndInit();
			this.tabControl2.ResumeLayout(false);
			this.tabControlPanel1.ResumeLayout(false);
			this.tabControlPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Internal Implementation
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Size fixedSize = tabControl2.FixedTabSize;
            using (Graphics g = this.CreateGraphics())
            {
                System.Drawing.Size s = TextDrawing.MeasureString(g, tabItemCustom.Text, this.Font);
                System.Drawing.Size s1 = TextDrawing.MeasureString(g, tabItemStandard.Text, this.Font);
                fixedSize.Width = Math.Max(fixedSize.Width, s.Width + 5);
                fixedSize.Width = Math.Max(fixedSize.Width, s1.Width + 5);
            }
            tabControl2.FixedTabSize = fixedSize;
            if (fixedSize.Width > tabControl2.Width / 2)
            {
                tabControl2.FixedTabSize = Size.Empty;
            }
        }
        internal void SetStyle(eDotNetBarStyle style)
        {
            if (BarFunctions.IsOffice2007Style(style))
            {
                this.EnableCustomStyle = true;
                tabControl2.Style = eTabStripStyle.Office2007Dock;
            }
            else
            {
                this.EnableCustomStyle = false;
                this.tabControl2.Style = DevComponents.DotNetBar.eTabStripStyle.VS2005;
            }

            buttonCancel.Style = style;
            buttonOK.Style = style;

            tabControl2.BackColor = Color.Transparent;
            tabControl2.ColorScheme.TabBackground = Color.Transparent;
            tabControl2.ColorScheme.TabBackground2 = Color.Empty;
        }
		private void colorCombControl1_SelectedColorChanged(object sender, System.EventArgs e)
		{
			SetNewColor(colorCombControl1.SelectedColor);
			colorContrastControl1.SelectedColor = colorCombControl1.SelectedColor;
		}

		private void customColorBlender3_SelectedColorChanged(object sender, System.EventArgs e)
		{
            colorContrastControl1.SelectedColor = customColorBlender3.SelectedColor;
            SetNewColor(colorContrastControl1.SelectedColor);
		}
		
		private bool m_SettingColor = false;
		private void SetNewColor(Color color)
		{
			if(m_SettingColor) return;
			m_SettingColor = true;
			try
			{
				m_NewColor = color;
				colorSelectionPreview.NewColor = color;
			
				numericRed.Value = m_NewColor.R;
				numericGreen.Value = m_NewColor.G;
				numericBlue.Value = m_NewColor.B;
			}
			finally
			{
				m_SettingColor = false;
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			comboColorModel.SelectedIndex = 0;
		}

		private void numericRGBValueChanged(object sender, System.EventArgs e)
		{
			SetNewColor(Color.FromArgb((int)numericRed.Value, (int)numericGreen.Value, (int)numericBlue.Value));
		}

		private void colorContrastControl1_SelectedColorChanged(object sender, System.EventArgs e)
		{
			SetNewColor(colorContrastControl1.SelectedColor);
		}
		
		public Color CurrentColor
		{
			get { return m_CurrentColor;}
			set
			{
				m_CurrentColor = value;
				colorSelectionPreview.CurrentColor = value;
			}
		}
		
		public Color NewColor
		{
			get { return m_NewColor;}
		}
		#endregion
	}
}
