using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Reflection;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents designer for SuperTooltip.
	/// </summary>
    [ToolboxItem(false)]
	public class SuperTooltipVisualEditor : UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private PanelEx panelEx1;
        internal ButtonX buttonOK;
        internal ButtonX buttonCancel;
        private System.Windows.Forms.CheckBox footerVisible;
        private System.Windows.Forms.CheckBox headerVisible;
        private System.Windows.Forms.TextBox textFooter;
        private System.Windows.Forms.TextBox textBody;
        private System.Windows.Forms.TextBox textHeader;
        private PanelEx panelBodyImage;
        private PanelEx panelEx5;
        private System.Windows.Forms.ComboBox comboColors;
        private System.Windows.Forms.Label label1;
        private PanelEx panelFooterImage;
        private System.Windows.Forms.CheckBox checkCustomSize;
        private System.Windows.Forms.NumericUpDown customHeight;
        private System.Windows.Forms.NumericUpDown customWidth;
        private System.Windows.Forms.Label label2;

        private SuperTooltipInfo m_Info = null;
        private bool m_Canceled = false;
        private IWindowsFormsEditorService m_EditorService = null;
        private bool m_BodyImageSelected = false;
        private bool m_FooterImageSelected = false;
        private SuperTooltipControl m_Tooltip = null;
        private PanelEx resetBodyImage;
        private PanelEx resetFooterImage;
        private SuperTooltip superTooltip1;
        private CustomTypeEditorProvider m_EditorProvider = null;
        private Font m_DefaultFont = null;
        internal int MaximumWidth = 0;
        private SuperTooltip m_ParentSuperTooltip = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.resetFooterImage = new DevComponents.DotNetBar.PanelEx();
            this.resetBodyImage = new DevComponents.DotNetBar.PanelEx();
            this.customWidth = new System.Windows.Forms.NumericUpDown();
            this.customHeight = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkCustomSize = new System.Windows.Forms.CheckBox();
            this.panelFooterImage = new DevComponents.DotNetBar.PanelEx();
            this.comboColors = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelEx5 = new DevComponents.DotNetBar.PanelEx();
            this.panelBodyImage = new DevComponents.DotNetBar.PanelEx();
            this.textFooter = new System.Windows.Forms.TextBox();
            this.textBody = new System.Windows.Forms.TextBox();
            this.textHeader = new System.Windows.Forms.TextBox();
            this.footerVisible = new System.Windows.Forms.CheckBox();
            this.headerVisible = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.superTooltip1 = new DevComponents.DotNetBar.SuperTooltip();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.AntiAlias = true;
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.Controls.Add(this.resetFooterImage);
            this.panelEx1.Controls.Add(this.resetBodyImage);
            this.panelEx1.Controls.Add(this.customWidth);
            this.panelEx1.Controls.Add(this.customHeight);
            this.panelEx1.Controls.Add(this.label2);
            this.panelEx1.Controls.Add(this.checkCustomSize);
            this.panelEx1.Controls.Add(this.panelFooterImage);
            this.panelEx1.Controls.Add(this.comboColors);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.panelEx5);
            this.panelEx1.Controls.Add(this.panelBodyImage);
            this.panelEx1.Controls.Add(this.textFooter);
            this.panelEx1.Controls.Add(this.textBody);
            this.panelEx1.Controls.Add(this.textHeader);
            this.panelEx1.Controls.Add(this.footerVisible);
            this.panelEx1.Controls.Add(this.headerVisible);
            this.panelEx1.Controls.Add(this.buttonCancel);
            this.panelEx1.Controls.Add(this.buttonOK);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(432, 320);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.WhiteSmoke;
            this.panelEx1.Style.BackColor2.Color = System.Drawing.Color.White;
            this.panelEx1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.Color = System.Drawing.Color.DimGray;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // resetFooterImage
            // 
            this.resetFooterImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetFooterImage.AntiAlias = true;
            this.resetFooterImage.CanvasColor = System.Drawing.SystemColors.Control;
            this.resetFooterImage.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.resetFooterImage.Location = new System.Drawing.Point(29, 230);
            this.resetFooterImage.Name = "resetFooterImage";
            this.resetFooterImage.Size = new System.Drawing.Size(73, 20);
            this.resetFooterImage.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.resetFooterImage.Style.BackColor1.Color = System.Drawing.Color.Gainsboro;
            this.resetFooterImage.Style.BackColor2.Color = System.Drawing.Color.WhiteSmoke;
            this.resetFooterImage.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.resetFooterImage.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.resetFooterImage.Style.BorderColor.Color = System.Drawing.Color.Gray;
            this.resetFooterImage.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.resetFooterImage.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.resetFooterImage.Style.GradientAngle = 90;
            this.resetFooterImage.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
            this.resetFooterImage.StyleMouseDown.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.resetFooterImage.StyleMouseDown.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.resetFooterImage.StyleMouseDown.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBorder;
            this.resetFooterImage.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.resetFooterImage.StyleMouseDown.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedText;
            this.resetFooterImage.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;
            this.resetFooterImage.StyleMouseOver.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground;
            this.resetFooterImage.StyleMouseOver.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground2;
            this.resetFooterImage.StyleMouseOver.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBorder;
            this.resetFooterImage.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.resetFooterImage.StyleMouseOver.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotText;
            this.superTooltip1.SetSuperTooltip(this.resetFooterImage, new DevComponents.DotNetBar.SuperTooltipInfo("Reset footer image", "", "Click to reset footer image.", null, null, DevComponents.DotNetBar.eTooltipColor.Lemon, true, false, new System.Drawing.Size(0, 0)));
            this.resetFooterImage.TabIndex = 17;
            this.resetFooterImage.Text = "Reset Image";
            this.resetFooterImage.Click += new System.EventHandler(this.resetFooterImage_Click);
            // 
            // resetBodyImage
            // 
            this.resetBodyImage.AntiAlias = true;
            this.resetBodyImage.CanvasColor = System.Drawing.SystemColors.Control;
            this.resetBodyImage.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.resetBodyImage.Location = new System.Drawing.Point(7, 60);
            this.resetBodyImage.Name = "resetBodyImage";
            this.resetBodyImage.Size = new System.Drawing.Size(95, 20);
            this.resetBodyImage.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.resetBodyImage.Style.BackColor1.Color = System.Drawing.Color.Gainsboro;
            this.resetBodyImage.Style.BackColor2.Color = System.Drawing.Color.WhiteSmoke;
            this.resetBodyImage.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.resetBodyImage.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.resetBodyImage.Style.BorderColor.Color = System.Drawing.Color.Gray;
            this.resetBodyImage.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.resetBodyImage.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.resetBodyImage.Style.GradientAngle = 90;
            this.resetBodyImage.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
            this.resetBodyImage.StyleMouseDown.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.resetBodyImage.StyleMouseDown.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.resetBodyImage.StyleMouseDown.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBorder;
            this.resetBodyImage.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.resetBodyImage.StyleMouseDown.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedText;
            this.resetBodyImage.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;
            this.resetBodyImage.StyleMouseOver.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground;
            this.resetBodyImage.StyleMouseOver.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground2;
            this.resetBodyImage.StyleMouseOver.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBorder;
            this.resetBodyImage.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.resetBodyImage.StyleMouseOver.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotText;
            this.superTooltip1.SetSuperTooltip(this.resetBodyImage, new DevComponents.DotNetBar.SuperTooltipInfo("Reset Body Image", "", "Click to reset body image.", null, null, DevComponents.DotNetBar.eTooltipColor.Lemon, true, false, new System.Drawing.Size(0, 0)));
            this.resetBodyImage.TabIndex = 16;
            this.resetBodyImage.Text = "Reset Image";
            this.resetBodyImage.Click += new System.EventHandler(this.resetBodyImage_Click);
            // 
            // customWidth
            // 
            this.customWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customWidth.Enabled = false;
            this.customWidth.Location = new System.Drawing.Point(107, 256);
            this.customWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.customWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.customWidth.Name = "customWidth";
            this.customWidth.Size = new System.Drawing.Size(52, 21);
            this.customWidth.TabIndex = 13;
            this.customWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // customHeight
            // 
            this.customHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customHeight.Enabled = false;
            this.customHeight.Location = new System.Drawing.Point(174, 256);
            this.customHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.customHeight.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.customHeight.Name = "customHeight";
            this.customHeight.Size = new System.Drawing.Size(52, 21);
            this.customHeight.TabIndex = 14;
            this.customHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "x";
            // 
            // checkCustomSize
            // 
            this.checkCustomSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkCustomSize.Location = new System.Drawing.Point(7, 260);
            this.checkCustomSize.Name = "checkCustomSize";
            this.checkCustomSize.Size = new System.Drawing.Size(83, 17);
            this.checkCustomSize.TabIndex = 12;
            this.checkCustomSize.Text = "Custom size";
            this.checkCustomSize.CheckedChanged += new System.EventHandler(this.checkCustomSize_CheckedChanged);
            // 
            // panelFooterImage
            // 
            this.panelFooterImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelFooterImage.AntiAlias = true;
            this.panelFooterImage.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelFooterImage.Location = new System.Drawing.Point(76, 203);
            this.panelFooterImage.Name = "panelFooterImage";
            this.panelFooterImage.Size = new System.Drawing.Size(24, 24);
            this.panelFooterImage.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelFooterImage.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelFooterImage.Style.BackColor2.Color = System.Drawing.Color.White;
            this.panelFooterImage.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Center;
            this.panelFooterImage.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelFooterImage.Style.BorderColor.Color = System.Drawing.Color.DimGray;
            this.panelFooterImage.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelFooterImage.Style.GradientAngle = 90;
            this.superTooltip1.SetSuperTooltip(this.panelFooterImage, new DevComponents.DotNetBar.SuperTooltipInfo("Click to set footer image", "", "Allows you to choose footer image. Note that image displayed here is preview image. You can see tooltip preview below.", null, null, DevComponents.DotNetBar.eTooltipColor.Lemon, true, false, new System.Drawing.Size(280, 170)));
            this.panelFooterImage.TabIndex = 11;
            this.panelFooterImage.Click += new System.EventHandler(this.panelFooterImage_Click);
            // 
            // comboColors
            // 
            this.comboColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboColors.Location = new System.Drawing.Point(107, 6);
            this.comboColors.Name = "comboColors";
            this.comboColors.Size = new System.Drawing.Size(316, 21);
            this.comboColors.Sorted = true;
            this.comboColors.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Predefined Colors:";
            // 
            // panelEx5
            // 
            this.panelEx5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx5.AntiAlias = true;
            this.panelEx5.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx5.Location = new System.Drawing.Point(7, 287);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(283, 24);
            this.panelEx5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx5.Style.BackColor1.Color = System.Drawing.Color.LightGray;
            this.panelEx5.Style.BackColor2.Color = System.Drawing.Color.DimGray;
            this.panelEx5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx5.Style.BorderColor.Color = System.Drawing.Color.DimGray;
            this.panelEx5.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.panelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx5.Style.GradientAngle = 90;
            this.panelEx5.TabIndex = 8;
            this.panelEx5.Text = "Place mouse here to preview";
            this.panelEx5.MouseLeave += new System.EventHandler(this.panelEx5_MouseLeave);
            this.panelEx5.MouseEnter += new System.EventHandler(this.panelEx5_MouseEnter);
            // 
            // panelBodyImage
            // 
            this.panelBodyImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panelBodyImage.AntiAlias = true;
            this.panelBodyImage.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelBodyImage.Location = new System.Drawing.Point(7, 83);
            this.panelBodyImage.Name = "panelBodyImage";
            this.panelBodyImage.Size = new System.Drawing.Size(94, 117);
            this.panelBodyImage.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelBodyImage.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelBodyImage.Style.BackColor2.Color = System.Drawing.Color.White;
            this.panelBodyImage.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Center;
            this.panelBodyImage.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelBodyImage.Style.BorderColor.Color = System.Drawing.Color.DimGray;
            this.panelBodyImage.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelBodyImage.Style.GradientAngle = 90;
            this.superTooltip1.SetSuperTooltip(this.panelBodyImage, new DevComponents.DotNetBar.SuperTooltipInfo("Click to set body image", "", "Allows you to choose body image. Note that image displayed here is preview image. You can see tooltip preview below", null, null, DevComponents.DotNetBar.eTooltipColor.Lemon, true, false, Size.Empty));
            this.panelBodyImage.TabIndex = 6;
            this.panelBodyImage.Click += new System.EventHandler(this.panelBodyImage_Click);
            // 
            // textFooter
            // 
            this.textFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFooter.Location = new System.Drawing.Point(107, 206);
            this.textFooter.Name = "textFooter";
            this.textFooter.Size = new System.Drawing.Size(316, 21);
            this.textFooter.TabIndex = 2;
            // 
            // textBody
            // 
            this.textBody.AcceptsReturn = true;
            this.textBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBody.Location = new System.Drawing.Point(107, 60);
            this.textBody.Multiline = true;
            this.textBody.Name = "textBody";
            this.textBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBody.Size = new System.Drawing.Size(316, 140);
            this.textBody.TabIndex = 1;
            // 
            // textHeader
            // 
            this.textHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textHeader.Location = new System.Drawing.Point(107, 33);
            this.textHeader.Name = "textHeader";
            this.textHeader.Size = new System.Drawing.Size(316, 21);
            this.textHeader.TabIndex = 0;
            // 
            // footerVisible
            // 
            this.footerVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.footerVisible.Location = new System.Drawing.Point(7, 208);
            this.footerVisible.Name = "footerVisible";
            this.footerVisible.Size = new System.Drawing.Size(58, 17);
            this.footerVisible.TabIndex = 7;
            this.footerVisible.Text = "Footer";
            // 
            // headerVisible
            // 
            this.headerVisible.Location = new System.Drawing.Point(7, 39);
            this.headerVisible.Name = "headerVisible";
            this.headerVisible.Size = new System.Drawing.Size(61, 17);
            this.headerVisible.TabIndex = 5;
            this.headerVisible.Text = "Header";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.AntiAlias = true;
            this.buttonCancel.Style = eDotNetBarStyle.Office2007;
            this.buttonCancel.ColorTable = eButtonColor.Office2007WithBackground;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(364, 287);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(60, 25);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.AntiAlias = true;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(296, 287);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(62, 25);
            this.buttonOK.Style = eDotNetBarStyle.Office2007;
            this.buttonOK.ColorTable = eButtonColor.Office2007WithBackground;
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // SuperTooltipVisualEditor
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SuperTooltipVisualEditor";
            this.Size = new System.Drawing.Size(432, 320);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public SuperTooltipVisualEditor()
        {
            InitializeComponent();
            panelBodyImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");
            panelFooterImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");
            comboColors.Items.AddRange(Enum.GetNames(typeof(eTooltipColor)));
#if (!TRIAL)
            this.superTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
        }

        private void panelBodyImage_Click(object sender, EventArgs e)
        {
            Image image = null;

            if (m_EditorProvider != null)
            {
                UITypeEditor edit = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
                m_EditorProvider.SetInstance(m_Info, TypeDescriptor.GetProperties(m_Info)["BodyImage"]);

                image = edit.EditValue(m_EditorProvider, m_EditorProvider, m_Info.BodyImage) as Image;
            }
            
            if (m_Info.BodyImage != image)
            {
                m_Info.BodyImage = image;
                if (m_Info.BodyImage != null)
                    panelBodyImage.Style.BackgroundImage = m_Info.BodyImage;
                else
                    panelBodyImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");
                m_BodyImageSelected = true;
            }
        }

        public CustomTypeEditorProvider EditorProvider
        {
            get { return m_EditorProvider; }
            set { m_EditorProvider = value; }
        }

        public SuperTooltipInfo SuperTooltipInfo
        {
            get
            {
                SuperTooltipInfo info = new SuperTooltipInfo();
                UpdateSuperTooltipInfo(info);
                return info; 
            }
            set
            {
                m_Info = value;
                OnSuperTooltipInfoChanged();
            }
        }

        //public Font DefaultSuperTooltipFont
        //{
        //    get { return m_DefaultFont; }
        //    set { m_DefaultFont = value; }
        //}

        public IWindowsFormsEditorService EditorService
        {
            get { return m_EditorService; }
            set { m_EditorService = value; }
        }

        public bool Canceled
        {
            get { return m_Canceled; }
        }

        private void OnSuperTooltipInfoChanged()
        {
            if (m_Info == null)
                return;
            textHeader.Text = m_Info.HeaderText;
            headerVisible.Checked = m_Info.HeaderVisible;
            textBody.Text = m_Info.BodyText;
            if (m_Info.BodyImage != null)
            {
                panelBodyImage.Style.BackgroundImage = m_Info.BodyImage;
                m_BodyImageSelected = true;
            }
            else
                panelBodyImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");

            if (m_Info.FooterImage != null)
            {
                panelFooterImage.Style.BackgroundImage = m_Info.FooterImage;
                m_FooterImageSelected = true;
            }
            else
                panelFooterImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");

            textFooter.Text = m_Info.FooterText;
            footerVisible.Checked = m_Info.FooterVisible;
            comboColors.SelectedItem = Enum.GetName(typeof(eTooltipColor), m_Info.Color);

            if (m_Info.CustomSize.IsEmpty)
            {
                checkCustomSize.Checked = false;
            }
            else
            {
                checkCustomSize.Checked = true;
                customWidth.Value = m_Info.CustomSize.Width;
                customHeight.Value = m_Info.CustomSize.Height;
            }
            customHeight.Enabled = checkCustomSize.Checked;
            customWidth.Enabled = checkCustomSize.Checked;
        }

        private void UpdateSuperTooltipInfo(SuperTooltipInfo info)
        {
            if (info == null)
                return;
            info.HeaderText = textHeader.Text;
            info.HeaderVisible = headerVisible.Checked;
            info.BodyText = textBody.Text;
            if(m_BodyImageSelected)
                info.BodyImage = panelBodyImage.Style.BackgroundImage;
            else
                info.BodyImage = null;

            if (m_FooterImageSelected)
                info.FooterImage = panelFooterImage.Style.BackgroundImage;
            else
                info.FooterImage = null;

            info.FooterText = textFooter.Text;
            info.FooterVisible = footerVisible.Checked;
            info.Color = (eTooltipColor)Enum.Parse(typeof(eTooltipColor), comboColors.SelectedItem.ToString());

            if (checkCustomSize.Checked)
                info.CustomSize = new Size((int)customWidth.Value, (int)customHeight.Value);
            else
                info.CustomSize = Size.Empty;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DestroyTooltip();
            m_Canceled = true;
            ((Form)this.Parent).Close();
            //if (m_EditorService != null)
            //    m_EditorService.CloseDropDown();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DestroyTooltip();
            m_Canceled = false;
            ((Form)this.Parent).Close();
            //if (m_EditorService != null)
            //    m_EditorService.CloseDropDown();
        }

        private void DestroyTooltip()
        {
            if (m_Tooltip == null)
                return;
            m_Tooltip.Hide();
            m_Tooltip.Dispose();
            m_Tooltip = null;
        }

        private void panelEx5_MouseEnter(object sender, EventArgs e)
        {
            DestroyTooltip();

            m_Tooltip = new SuperTooltipControl();
            if (m_DefaultFont != null)
                m_Tooltip.Font = m_DefaultFont;
            if (m_ParentSuperTooltip != null)
            {
                m_Tooltip.MaximumWidth = m_ParentSuperTooltip.MaximumWidth;
                m_Tooltip.MinimumTooltipSize = m_ParentSuperTooltip.MinimumTooltipSize;
                m_Tooltip.AntiAlias = m_ParentSuperTooltip.AntiAlias;
                if (m_ParentSuperTooltip.DefaultFont != null)
                    m_Tooltip.Font = m_ParentSuperTooltip.DefaultFont;
            }
            else
                m_Tooltip.MaximumWidth = MaximumWidth;
            SuperTooltipInfo info=new SuperTooltipInfo();
            UpdateSuperTooltipInfo(info);
            Point p = panelEx5.PointToScreen(new Point(0, panelEx5.Height + 1));
            m_Tooltip.ShowTooltip(info, p.X, p.Y, true);
        }

        private void panelEx5_MouseLeave(object sender, EventArgs e)
        {
            DestroyTooltip();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if(!this.Visible)
                DestroyTooltip();
        }

        private void panelFooterImage_Click(object sender, EventArgs e)
        {
            Image image = null;

            if (m_EditorProvider != null)
            {
                UITypeEditor edit = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
                m_EditorProvider.SetInstance(m_Info, TypeDescriptor.GetProperties(m_Info)["FooterImage"]);

                image = edit.EditValue(m_EditorProvider, m_EditorProvider, m_Info.FooterImage) as Image;
            }

            if (m_Info.FooterImage != image)
            {
                m_Info.FooterImage = image;
                if (m_Info.FooterImage != null)
                    panelFooterImage.Style.BackgroundImage = m_Info.FooterImage;
                else
                    panelFooterImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");
                m_FooterImageSelected = true;
            }
        }

        private void checkCustomSize_CheckedChanged(object sender, EventArgs e)
        {
            customWidth.Enabled = checkCustomSize.Checked;
            customHeight.Enabled = checkCustomSize.Checked;
            if (checkCustomSize.Checked)
            {
                SuperTooltipInfo info = new SuperTooltipInfo();
                UpdateSuperTooltipInfo(info);
                SuperTooltipControl c = new SuperTooltipControl();
                c.UpdateWithSuperTooltipInfo(info);
                c.RecalcSize();
                customWidth.Value = c.Width;
                customHeight.Value = c.Height;
                c.Dispose();
            }
        }

        private void resetBodyImage_Click(object sender, EventArgs e)
        {
            panelBodyImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder.png");
            m_Info.BodyImage = null;
            m_BodyImageSelected = false;
        }

        private void resetFooterImage_Click(object sender, EventArgs e)
        {
            panelFooterImage.Style.BackgroundImage = Helpers.LoadBitmap("SystemImages.ImagePlaceHolder16x16.png");
            m_Info.FooterImage = null;
            m_FooterImageSelected = false;
        }

        public SuperTooltip ParentSuperTooltip
        {
            get { return m_ParentSuperTooltip; }
            set { m_ParentSuperTooltip = value; }
        }
    }
}
