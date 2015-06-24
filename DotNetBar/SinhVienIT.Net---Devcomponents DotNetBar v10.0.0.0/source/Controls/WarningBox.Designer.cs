#if FRAMEWORK20
namespace DevComponents.DotNetBar.Controls
{
    partial class WarningBox
    {
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
            if (disposing) StyleManager.Unregister(this);
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
            this.OptionsButton = new DevComponents.DotNetBar.PanelEx();
            this.PanelWarning = new DevComponents.DotNetBar.PanelEx();
            this.WarningLabel = new DevComponents.DotNetBar.LabelX();
            this.PanelClose = new System.Windows.Forms.Panel();
            this.CloseButton = new DevComponents.DotNetBar.ButtonX();
            this.PanelWarning.SuspendLayout();
            this.PanelClose.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionsButton
            // 
            this.OptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsButton.CanvasColor = System.Drawing.SystemColors.Control;
            this.OptionsButton.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.OptionsButton.Location = new System.Drawing.Point(311, 5);
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(88, 23);
            this.OptionsButton.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.OptionsButton.Style.BackColor1.Color = System.Drawing.Color.White;
            this.OptionsButton.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(244)))), ((int)(((byte)(254)))));
            this.OptionsButton.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.OptionsButton.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
            this.OptionsButton.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(188)))), ((int)(((byte)(213)))));
            this.OptionsButton.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.OptionsButton.Style.GradientAngle = 90;
            this.OptionsButton.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
            this.OptionsButton.StyleMouseDown.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.OptionsButton.StyleMouseDown.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.OptionsButton.StyleMouseDown.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBorder;
            this.OptionsButton.StyleMouseDown.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedText;
            this.OptionsButton.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;
            this.OptionsButton.StyleMouseOver.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground;
            this.OptionsButton.StyleMouseOver.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground2;
            this.OptionsButton.StyleMouseOver.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBorder;
            this.OptionsButton.StyleMouseOver.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotText;
            this.OptionsButton.TabIndex = 1;
            this.OptionsButton.Text = "Options...";
            this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
            // 
            // PanelWarning
            // 
            this.PanelWarning.CanvasColor = System.Drawing.SystemColors.Control;
            this.PanelWarning.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.PanelWarning.Controls.Add(this.WarningLabel);
            this.PanelWarning.Controls.Add(this.OptionsButton);
            this.PanelWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelWarning.Location = new System.Drawing.Point(0, 0);
            this.PanelWarning.Name = "PanelWarning";
            this.PanelWarning.Padding = new System.Windows.Forms.Padding(4);
            this.PanelWarning.Size = new System.Drawing.Size(405, 33);
            this.PanelWarning.Style.BackColor1.Color = System.Drawing.Color.White;
            this.PanelWarning.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(244)))), ((int)(((byte)(254)))));
            this.PanelWarning.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.PanelWarning.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(188)))), ((int)(((byte)(213)))));
            this.PanelWarning.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.PanelWarning.Style.GradientAngle = 90;
            this.PanelWarning.Style.MarginLeft = 5;
            this.PanelWarning.TabIndex = 11;
            // 
            // WarningLabel
            // 
            this.WarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.WarningLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.WarningLabel.Location = new System.Drawing.Point(4, 5);
            this.WarningLabel.Margin = new System.Windows.Forms.Padding(5);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.PaddingLeft = 2;
            this.WarningLabel.Size = new System.Drawing.Size(304, 22);
            this.WarningLabel.TabIndex = 0;
            this.WarningLabel.MarkupLinkClick += new DevComponents.DotNetBar.MarkupLinkClickEventHandler(this.WarningLabel_MarkupLinkClick);
            // 
            // PanelClose
            // 
            this.PanelClose.BackColor = System.Drawing.Color.Transparent;
            this.PanelClose.Controls.Add(this.CloseButton);
            this.PanelClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.PanelClose.Location = new System.Drawing.Point(405, 0);
            this.PanelClose.Name = "PanelClose";
            this.PanelClose.Size = new System.Drawing.Size(22, 33);
            this.PanelClose.TabIndex = 12;
            // 
            // CloseButton
            // 
            this.CloseButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CloseButton.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.CloseButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CloseButton.FocusCuesEnabled = false;
            this.CloseButton.Location = new System.Drawing.Point(0, 0);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(22, 17);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Tooltip = "Close";
            this.CloseButton.AntiAlias = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // WarningBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(219)))), ((int)(((byte)(249)))));
            this.Controls.Add(this.PanelWarning);
            this.Controls.Add(this.PanelClose);
            this.Name = "WarningBox";
            this.Size = new System.Drawing.Size(427, 33);
            this.PanelWarning.ResumeLayout(false);
            this.PanelClose.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal PanelEx PanelWarning;
        internal System.Windows.Forms.Panel PanelClose;
        internal ButtonX CloseButton;
        private PanelEx OptionsButton;
        private LabelX WarningLabel;
    }
}
#endif