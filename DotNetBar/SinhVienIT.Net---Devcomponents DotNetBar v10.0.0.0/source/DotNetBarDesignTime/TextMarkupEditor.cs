using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for TextMarkupEditor.
	/// </summary>
	[ToolboxItem(false)]
    public class TextMarkupEditor : PanelEx
    {
        public System.Windows.Forms.Button buttonCancel;
		public System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Timer timer1;
		private DevComponents.DotNetBar.PanelEx previewPanel;
		public System.Windows.Forms.TextBox inputText;
        private Label label2;
		private System.ComponentModel.IContainer components;
        private Bar bar1;
        private ButtonItem buttonBold;
        private ButtonItem buttonItalic;
        private ButtonItem buttonUnderline;
        private ColorPickerDropDown buttonColor;
        private SuperTooltipControl m_Tooltip = null;

		public TextMarkupEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.ApplyLabelStyle();
			this.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
			this.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.TabStop = false;
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
            this.components = new System.ComponentModel.Container();
            this.inputText = new System.Windows.Forms.TextBox();
            this.previewPanel = new DevComponents.DotNetBar.PanelEx();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.buttonBold = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItalic = new DevComponents.DotNetBar.ButtonItem();
            this.buttonUnderline = new DevComponents.DotNetBar.ButtonItem();
            this.buttonColor = new DevComponents.DotNetBar.ColorPickerDropDown();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // inputText
            // 
            this.inputText.AcceptsReturn = true;
            this.inputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inputText.Location = new System.Drawing.Point(8, 8);
            this.inputText.Multiline = true;
            this.inputText.Name = "inputText";
            this.inputText.Size = new System.Drawing.Size(332, 110);
            this.inputText.TabIndex = 0;
            this.inputText.TextChanged += new System.EventHandler(this.inputText_TextChanged);
            // 
            // previewPanel
            // 
            this.previewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewPanel.AutoScroll = true;
            this.previewPanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.previewPanel.Location = new System.Drawing.Point(8, 152);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(332, 75);
            this.previewPanel.Style.BackColor1.Color = System.Drawing.Color.White;
            this.previewPanel.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.previewPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.previewPanel.Style.BorderColor.Color = System.Drawing.Color.DarkGray;
            this.previewPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.previewPanel.Style.GradientAngle = 90;
            this.previewPanel.Style.LineAlignment = System.Drawing.StringAlignment.Near;
            this.previewPanel.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(269, 233);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(71, 24);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(194, 233);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(71, 24);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(316, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.MouseLeave += new System.EventHandler(this.label2_MouseLeave);
            this.label2.MouseEnter += new System.EventHandler(this.label2_MouseEnter);
            // 
            // bar1
            // 
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonBold,
            this.buttonItalic,
            this.buttonUnderline,
            this.buttonColor});
            this.bar1.Location = new System.Drawing.Point(8, 124);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(174, 25);
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.bar1.TabIndex = 6;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // buttonBold
            // 
            this.buttonBold.Name = "buttonBold";
            this.buttonBold.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlB);
            this.buttonBold.Text = "Bold";
            this.buttonBold.Click += new System.EventHandler(this.buttonBold_Click);
            // 
            // buttonItalic
            // 
            this.buttonItalic.Name = "buttonItalic";
            this.buttonItalic.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlI);
            this.buttonItalic.Text = "Italic";
            this.buttonItalic.Click += new System.EventHandler(this.buttonItalic_Click);
            // 
            // buttonUnderline
            // 
            this.buttonUnderline.Name = "buttonUnderline";
            this.buttonUnderline.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlU);
            this.buttonUnderline.Text = "Underline";
            this.buttonUnderline.Click += new System.EventHandler(this.buttonUnderline_Click);
            // 
            // buttonColor
            // 
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Tooltip = "Sets the text color";
            this.buttonColor.Image = Helpers.LoadBitmap("SystemImages.ColorPickerButtonImage.png");
            this.buttonColor.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_SelectedColorChanged);
            // 
            // TextMarkupEditor
            // 
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.previewPanel);
            this.Controls.Add(this.inputText);
            this.Name = "TextMarkupEditor";
            this.Size = new System.Drawing.Size(348, 263);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void buttonCancel_Click(object sender, EventArgs e)
        {
            DestroyTooltip();
            this.DialogResult = DialogResult.Cancel;
        }

        void buttonOK_Click(object sender, EventArgs e)
        {
            DestroyTooltip();
            this.DialogResult = DialogResult.OK;
        }
		#endregion

		private void inputText_TextChanged(object sender, System.EventArgs e)
		{
			timer1.Stop();
			timer1.Start();
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Stop();
            UpdatePreview();
		}

        private void UpdatePreview()
        {
            previewPanel.Text = inputText.Text;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            inputText.Focus();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            DestroyTooltip();

            m_Tooltip = new SuperTooltipControl();
            m_Tooltip.AntiAlias = false;
            SuperTooltipInfo info = new SuperTooltipInfo();
            info.HeaderText = "Markup tags available";
            info.BodyText = "<b>b</b> - Bold <br/>" +
                "<b>i</b> - Italic <br/>" +
                "<b>u</b> - Underline <br/>" +
                "<b>p</b> - Paragraph container, <b>width</b> attribute indicates width of block element <br/>" +
                "<b>div</b> - Block-level container, <b>width</b> attribute indicates width of block element<br/>" +
                "<b>span</b> - Inline container, <b>width</b> attribute indicates width of block element. Can be used to create tables<br/>" +
                "<b>br</b> - Line break<br/>" +
                "<b>font</b> - Changes font, color, size. <b>size</b> attribute indicates absolute or relative font size. <b>color</b> attribute to change the color" +
                "You can use relative sizing for example +1 to increase font size by one point or -1 to decrease by one point. <b>face</b> attribute can be employed to set the specific font name to use.<br/>" +
                "<b>h1</b> - Header markup. You can use h1, h2, h3, h4, h5, h6 to represent header sizes<br/>" +
                "<b>a</b> - Denotes hypertext link. <b>href</b> and <b>name</b> attributes are supported<br/>" +
                "<b>expand</b> - Displays the expand part of the button" +
                "<br/>All tags must be lower case and they must be well formed i.e. each tag must be closed with end tag or be an empty tag.";
            info.Color = eTooltipColor.Default;
            Point p = label2.PointToScreen(new Point(0, label2.Height + 1));
            m_Tooltip.ShowTooltip(info, p.X, p.Y, true);
        }

        private void DestroyTooltip()
        {
            if (m_Tooltip == null)
                return;
            m_Tooltip.Hide();
            m_Tooltip.Dispose();
            m_Tooltip = null;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            DestroyTooltip();
        }

        private void buttonBold_Click(object sender, EventArgs e)
        {
            ApplyTag("<b>", "</b>");
        }

        private void buttonItalic_Click(object sender, EventArgs e)
        {
            ApplyTag("<i>", "</i>");
        }

        private void buttonUnderline_Click(object sender, EventArgs e)
        {
            ApplyTag("<u>", "</u>");
        }

        private void buttonColor_SelectedColorChanged(object sender, EventArgs e)
        {
            ApplyTag("<font color=\"" + GetHexColor(buttonColor.SelectedColor) +"\">", "</font>");
        }

        private void ApplyTag(string startTag, string endTag)
        {
            if (inputText.SelectionLength == 0)
                inputText.SelectedText = startTag + endTag;
            else
                inputText.SelectedText = startTag + inputText.SelectedText + endTag;

            UpdatePreview();
        }

        private string GetHexColor(Color color)
        {
            return "#" + color.R.ToString("X02") + color.G.ToString("X02") + color.B.ToString("X02");
        }
	}
}
