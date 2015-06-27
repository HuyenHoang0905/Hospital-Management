namespace DevComponents.DotNetBar
{
    partial class TaskDialogForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.headerLabel = new DevComponents.DotNetBar.LabelX();
            this.contentLabel = new DevComponents.DotNetBar.LabelX();
            this.buttonsPanel = new DevComponents.DotNetBar.ItemPanel();
            this.bottomPanel = new DevComponents.DotNetBar.PanelEx();
            this.footerLabel = new DevComponents.DotNetBar.LabelX();
            this.footerImage = new System.Windows.Forms.PictureBox();
            this.taskCheckBox = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.footerPanel = new DevComponents.DotNetBar.PanelEx();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonClose = new DevComponents.DotNetBar.ButtonX();
            this.buttonRetry = new DevComponents.DotNetBar.ButtonX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonOk = new DevComponents.DotNetBar.ButtonX();
            this.buttonNo = new DevComponents.DotNetBar.ButtonX();
            this.buttonYes = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).BeginInit();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.footerImage)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerImage
            // 
            this.headerImage.Location = new System.Drawing.Point(12, 12);
            this.headerImage.Name = "headerImage";
            this.headerImage.Size = new System.Drawing.Size(32, 32);
            this.headerImage.TabIndex = 0;
            this.headerImage.TabStop = false;
            // 
            // headerLabel
            // 
            this.headerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.headerLabel.BackgroundStyle.Class = "";
            this.headerLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.headerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerLabel.Location = new System.Drawing.Point(50, 12);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(444, 32);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Task Dialog Header Instructions";
            this.headerLabel.TextLineAlignment = System.Drawing.StringAlignment.Near;
            this.headerLabel.WordWrap = true;
            // 
            // contentLabel
            // 
            this.contentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.contentLabel.BackgroundStyle.Class = "";
            this.contentLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.contentLabel.Location = new System.Drawing.Point(50, 60);
            this.contentLabel.Name = "contentLabel";
            this.contentLabel.Size = new System.Drawing.Size(444, 35);
            this.contentLabel.TabIndex = 1;
            this.contentLabel.Text = "Task Dialog actual content of the dialog with some instructions and more text wit" +
                "h text-markup support and word wrapping.";
            this.contentLabel.TextLineAlignment = System.Drawing.StringAlignment.Near;
            this.contentLabel.WordWrap = true;
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.buttonsPanel.BackgroundStyle.Class = "";
            this.buttonsPanel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.buttonsPanel.ContainerControlProcessDialogKey = true;
            this.buttonsPanel.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
#if !TRIAL
            this.buttonsPanel.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
            this.buttonsPanel.Location = new System.Drawing.Point(50, 101);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(444, 120);
            this.buttonsPanel.TabIndex = 2;
            this.buttonsPanel.Text = "itemPanel1";
            // 
            // bottomPanel
            // 
            this.bottomPanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.bottomPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bottomPanel.Controls.Add(this.footerLabel);
            this.bottomPanel.Controls.Add(this.footerImage);
            this.bottomPanel.Controls.Add(this.taskCheckBox);
            this.bottomPanel.Controls.Add(this.footerPanel);
            this.bottomPanel.Controls.Add(this.flowLayoutPanel1);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 240);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.bottomPanel.Size = new System.Drawing.Size(506, 70);
            this.bottomPanel.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.bottomPanel.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.bottomPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.bottomPanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.bottomPanel.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Top;
            this.bottomPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.bottomPanel.Style.GradientAngle = 90;
            this.bottomPanel.TabIndex = 0;
            // 
            // footerLabel
            // 
            this.footerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.footerLabel.BackgroundStyle.Class = "";
            this.footerLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.footerLabel.Location = new System.Drawing.Point(33, 49);
            this.footerLabel.Name = "footerLabel";
            this.footerLabel.Size = new System.Drawing.Size(467, 15);
            this.footerLabel.TabIndex = 2;
            this.footerLabel.Text = "Footer Label with text-markup support";
            // 
            // footerImage
            // 
            this.footerImage.Location = new System.Drawing.Point(8, 48);
            this.footerImage.Name = "footerImage";
            this.footerImage.Size = new System.Drawing.Size(16, 16);
            this.footerImage.TabIndex = 8;
            this.footerImage.TabStop = false;
            // 
            // taskCheckBox
            // 
            this.taskCheckBox.AutoSize = true;
            // 
            // 
            // 
            this.taskCheckBox.BackgroundStyle.Class = "";
            this.taskCheckBox.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.taskCheckBox.Location = new System.Drawing.Point(3, 10);
            this.taskCheckBox.Name = "taskCheckBox";
            this.taskCheckBox.Size = new System.Drawing.Size(75, 15);
            this.taskCheckBox.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.taskCheckBox.TabIndex = 10;
            this.taskCheckBox.Text = "Check-box";
            // 
            // footerPanel
            // 
            this.footerPanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.footerPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(3, 40);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Size = new System.Drawing.Size(500, 30);
            this.footerPanel.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.footerPanel.Style.Border = DevComponents.DotNetBar.eBorderType.Raised;
            this.footerPanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.footerPanel.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Top;
            this.footerPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.footerPanel.Style.GradientAngle = 90;
            this.footerPanel.TabIndex = 10;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonClose);
            this.flowLayoutPanel1.Controls.Add(this.buttonCancel);
            this.flowLayoutPanel1.Controls.Add(this.buttonOk);
            this.flowLayoutPanel1.Controls.Add(this.buttonRetry);
            this.flowLayoutPanel1.Controls.Add(this.buttonNo);
            this.flowLayoutPanel1.Controls.Add(this.buttonYes);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(500, 35);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // buttonClose
            // 
            this.buttonClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonClose.Location = new System.Drawing.Point(436, 5);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(61, 24);
            this.buttonClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "&Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRetry
            // 
            this.buttonRetry.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonRetry.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonRetry.Location = new System.Drawing.Point(369, 5);
            this.buttonRetry.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(61, 24);
            this.buttonRetry.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonRetry.TabIndex = 4;
            this.buttonRetry.Text = "&Retry";
            this.buttonRetry.Click += new System.EventHandler(this.buttonRetry_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(302, 5);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(61, 24);
            this.buttonCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOk.Location = new System.Drawing.Point(235, 5);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(61, 24);
            this.buttonOk.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "&OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonNo
            // 
            this.buttonNo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonNo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonNo.Location = new System.Drawing.Point(168, 5);
            this.buttonNo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(61, 24);
            this.buttonNo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonNo.TabIndex = 1;
            this.buttonNo.Text = "&No";
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // buttonYes
            // 
            this.buttonYes.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonYes.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonYes.Location = new System.Drawing.Point(101, 5);
            this.buttonYes.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(61, 24);
            this.buttonYes.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonYes.TabIndex = 0;
            this.buttonYes.Text = "&Yes";
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // TaskDialogForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(506, 310);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.contentLabel);
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.headerImage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskDialogForm";
            this.Text = "DotNetBar Task Dialog";
            this.Load += new System.EventHandler(this.TaskDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.footerImage)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox headerImage;
        private LabelX headerLabel;
        private LabelX contentLabel;
        private ItemPanel buttonsPanel;
        private PanelEx bottomPanel;
        private ButtonX buttonOk;
        private ButtonX buttonClose;
        private ButtonX buttonRetry;
        private ButtonX buttonCancel;
        private ButtonX buttonNo;
        private ButtonX buttonYes;
        private DevComponents.DotNetBar.Controls.CheckBoxX taskCheckBox;
        private LabelX footerLabel;
        private System.Windows.Forms.PictureBox footerImage;
        private PanelEx footerPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}