#if FRAMEWORK20
namespace DevComponents.DotNetBar.Design
{
    partial class ShapeEditorForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapeEditorForm));
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.buttonDefault = new DevComponents.DotNetBar.ButtonItem();
            this.buttonRect = new DevComponents.DotNetBar.ButtonItem();
            this.buttonRound = new DevComponents.DotNetBar.ButtonItem();
            this.buttonEllipse = new DevComponents.DotNetBar.ButtonItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabDefault = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel4 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabRect = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel3 = new DevComponents.DotNetBar.TabControlPanel();
            this.roundBottomRight = new DevComponents.Editors.IntegerInput();
            this.roundTopRight = new DevComponents.Editors.IntegerInput();
            this.roundBottomLeft = new DevComponents.Editors.IntegerInput();
            this.roundTopLeft = new DevComponents.Editors.IntegerInput();
            this.buttonRoundPreview = new DevComponents.DotNetBar.ButtonX();
            this.tabRound = new DevComponents.DotNetBar.TabItem(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roundBottomRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundBottomLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundTopLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // itemPanel1
            // 
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanel1.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanel1.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanel1.BackgroundStyle.PaddingBottom = 1;
            this.itemPanel1.BackgroundStyle.PaddingLeft = 1;
            this.itemPanel1.BackgroundStyle.PaddingRight = 1;
            this.itemPanel1.BackgroundStyle.PaddingTop = 1;
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.itemPanel1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.itemContainer1});
            this.itemPanel1.ItemSpacing = 2;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.Location = new System.Drawing.Point(0, 0);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(387, 69);
            this.itemPanel1.TabIndex = 0;
            this.itemPanel1.Text = "itemPanel1";
            this.itemPanel1.OptionGroupChanging += new DevComponents.DotNetBar.OptionGroupChangingEventHandler(this.itemPanel1_OptionGroupChanging);
            // 
            // labelItem1
            // 
            this.labelItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelItem1.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.labelItem1.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.PaddingBottom = 2;
            this.labelItem1.PaddingLeft = 2;
            this.labelItem1.PaddingTop = 2;
            this.labelItem1.SingleLineColor = System.Drawing.Color.DarkGray;
            this.labelItem1.Text = "<b>Choose the shape you want to use:</b>";
            // 
            // itemContainer1
            // 
            this.itemContainer1.Name = "itemContainer1";
            this.itemContainer1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonDefault,
            this.buttonRect,
            this.buttonRound,
            this.buttonEllipse});
            // 
            // buttonDefault
            // 
            this.buttonDefault.AutoCheckOnClick = true;
            this.buttonDefault.Image = ((System.Drawing.Image)(resources.GetObject("buttonDefault.Image")));
            this.buttonDefault.ImagePaddingHorizontal = 8;
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.OptionGroup = "shape";
            this.buttonDefault.Text = "Default";
            this.buttonDefault.Tooltip = "Use Default Shape";
            // 
            // buttonRect
            // 
            this.buttonRect.AutoCheckOnClick = true;
            this.buttonRect.Image = ((System.Drawing.Image)(resources.GetObject("buttonRect.Image")));
            this.buttonRect.ImagePaddingHorizontal = 8;
            this.buttonRect.Name = "buttonRect";
            this.buttonRect.OptionGroup = "shape";
            this.buttonRect.Text = "Rect";
            this.buttonRect.Tooltip = "Rectangle Shape";
            // 
            // buttonRound
            // 
            this.buttonRound.AutoCheckOnClick = true;
            this.buttonRound.Image = ((System.Drawing.Image)(resources.GetObject("buttonRound.Image")));
            this.buttonRound.ImagePaddingHorizontal = 8;
            this.buttonRound.Name = "buttonRound";
            this.buttonRound.OptionGroup = "shape";
            this.buttonRound.Text = "Round";
            this.buttonRound.Tooltip = "Rounded Rectangle Shape";
            // 
            // buttonEllipse
            // 
            this.buttonEllipse.AutoCheckOnClick = true;
            this.buttonEllipse.Image = ((System.Drawing.Image)(resources.GetObject("buttonEllipse.Image")));
            this.buttonEllipse.ImagePaddingHorizontal = 8;
            this.buttonEllipse.Name = "buttonEllipse";
            this.buttonEllipse.OptionGroup = "shape";
            this.buttonEllipse.Text = "Elliptical Shape";
            this.buttonEllipse.Tooltip = "Elliptical Shape";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(228, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(309, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Controls.Add(this.tabControlPanel4);
            this.tabControl1.Controls.Add(this.tabControlPanel2);
            this.tabControl1.Controls.Add(this.tabControlPanel3);
            this.tabControl1.Location = new System.Drawing.Point(0, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(387, 89);
            this.tabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Dock;
            this.tabControl1.TabIndex = 3;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabDefault);
            this.tabControl1.Tabs.Add(this.tabRect);
            this.tabControl1.Tabs.Add(this.tabRound);
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.TabsVisible = false;
            this.tabControl1.Text = "tabControl1";
            this.tabControl1.SelectedTabChanged += new DevComponents.DotNetBar.TabStrip.SelectedTabChangedEventHandler(this.tabControl1_SelectedTabChanged);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 22);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(387, 67);
            this.tabControlPanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.White;
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.White;
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.ForeColor.Color = System.Drawing.Color.DimGray;
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabDefault;
            this.tabControlPanel1.Text = "No properties available for this shape.";
            this.tabControlPanel1.UseCustomStyle = true;
            // 
            // tabDefault
            // 
            this.tabDefault.AttachedControl = this.tabControlPanel1;
            this.tabDefault.Name = "tabDefault";
            this.tabDefault.Text = "Default";
            // 
            // tabControlPanel4
            // 
            this.tabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel4.Location = new System.Drawing.Point(0, 22);
            this.tabControlPanel4.Name = "tabControlPanel4";
            this.tabControlPanel4.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel4.Size = new System.Drawing.Size(387, 67);
            this.tabControlPanel4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.tabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.White;
            this.tabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.White;
            this.tabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel4.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel4.Style.ForeColor.Color = System.Drawing.Color.DimGray;
            this.tabControlPanel4.Style.GradientAngle = 90;
            this.tabControlPanel4.TabIndex = 4;
            this.tabControlPanel4.TabItem = this.tabItem1;
            this.tabControlPanel4.Text = "No properties available for this shape.";
            this.tabControlPanel4.UseCustomStyle = true;
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel4;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "Ellipse";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 22);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(387, 67);
            this.tabControlPanel2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.White;
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.White;
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.ForeColor.Color = System.Drawing.Color.DimGray;
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabRect;
            this.tabControlPanel2.Text = "No properties available for this shape.";
            this.tabControlPanel2.UseCustomStyle = true;
            // 
            // tabRect
            // 
            this.tabRect.AttachedControl = this.tabControlPanel2;
            this.tabRect.Name = "tabRect";
            this.tabRect.Text = "Rect";
            // 
            // tabControlPanel3
            // 
            this.tabControlPanel3.Controls.Add(this.roundBottomRight);
            this.tabControlPanel3.Controls.Add(this.roundTopRight);
            this.tabControlPanel3.Controls.Add(this.roundBottomLeft);
            this.tabControlPanel3.Controls.Add(this.roundTopLeft);
            this.tabControlPanel3.Controls.Add(this.buttonRoundPreview);
            this.tabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel3.Location = new System.Drawing.Point(0, 22);
            this.tabControlPanel3.Name = "tabControlPanel3";
            this.tabControlPanel3.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel3.Size = new System.Drawing.Size(387, 67);
            this.tabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.White;
            this.tabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.White;
            this.tabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.tabControlPanel3.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel3.Style.GradientAngle = 90;
            this.tabControlPanel3.TabIndex = 3;
            this.tabControlPanel3.TabItem = this.tabRound;
            this.tabControlPanel3.UseCustomStyle = true;
            // 
            // roundBottomRight
            // 
            this.roundBottomRight.AllowEmptyState = false;
            // 
            // 
            // 
            this.roundBottomRight.BackgroundStyle.Class = "DateTimeInputBackground";
            this.roundBottomRight.Location = new System.Drawing.Point(231, 29);
            this.roundBottomRight.MaxValue = 999;
            this.roundBottomRight.MinValue = 0;
            this.roundBottomRight.Name = "roundBottomRight";
            this.roundBottomRight.ShowUpDown = true;
            this.roundBottomRight.Size = new System.Drawing.Size(46, 20);
            this.roundBottomRight.TabIndex = 4;
            this.roundBottomRight.ValueChanged += new System.EventHandler(this.RoundCorner_ValueChanged);
            // 
            // roundTopRight
            // 
            this.roundTopRight.AllowEmptyState = false;
            // 
            // 
            // 
            this.roundTopRight.BackgroundStyle.Class = "DateTimeInputBackground";
            this.roundTopRight.Location = new System.Drawing.Point(231, 4);
            this.roundTopRight.MaxValue = 999;
            this.roundTopRight.MinValue = 0;
            this.roundTopRight.Name = "roundTopRight";
            this.roundTopRight.ShowUpDown = true;
            this.roundTopRight.Size = new System.Drawing.Size(46, 20);
            this.roundTopRight.TabIndex = 3;
            this.roundTopRight.ValueChanged += new System.EventHandler(this.RoundCorner_ValueChanged);
            // 
            // roundBottomLeft
            // 
            this.roundBottomLeft.AllowEmptyState = false;
            // 
            // 
            // 
            this.roundBottomLeft.BackgroundStyle.Class = "DateTimeInputBackground";
            this.roundBottomLeft.Location = new System.Drawing.Point(96, 29);
            this.roundBottomLeft.MaxValue = 999;
            this.roundBottomLeft.MinValue = 0;
            this.roundBottomLeft.Name = "roundBottomLeft";
            this.roundBottomLeft.ShowUpDown = true;
            this.roundBottomLeft.Size = new System.Drawing.Size(46, 20);
            this.roundBottomLeft.TabIndex = 2;
            this.roundBottomLeft.ValueChanged += new System.EventHandler(this.RoundCorner_ValueChanged);
            // 
            // roundTopLeft
            // 
            this.roundTopLeft.AllowEmptyState = false;
            // 
            // 
            // 
            this.roundTopLeft.BackgroundStyle.Class = "DateTimeInputBackground";
            this.roundTopLeft.Location = new System.Drawing.Point(96, 4);
            this.roundTopLeft.MaxValue = 999;
            this.roundTopLeft.MinValue = 0;
            this.roundTopLeft.Name = "roundTopLeft";
            this.roundTopLeft.ShowUpDown = true;
            this.roundTopLeft.Size = new System.Drawing.Size(46, 20);
            this.roundTopLeft.TabIndex = 1;
            this.roundTopLeft.ValueChanged += new System.EventHandler(this.RoundCorner_ValueChanged);
            // 
            // buttonRoundPreview
            // 
            this.buttonRoundPreview.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonRoundPreview.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonRoundPreview.Location = new System.Drawing.Point(148, 4);
            this.buttonRoundPreview.Name = "buttonRoundPreview";
            this.buttonRoundPreview.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(2);
            this.buttonRoundPreview.Size = new System.Drawing.Size(77, 45);
            this.buttonRoundPreview.TabIndex = 0;
            this.buttonRoundPreview.Text = "Preview";
            // 
            // tabRound
            // 
            this.tabRound.AttachedControl = this.tabControlPanel3;
            this.tabRound.Name = "tabRound";
            this.tabRound.Text = "Round";
            // 
            // ShapeEditorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(387, 194);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.itemPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShapeEditorForm";
            this.Text = "Shape Editor";
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.roundBottomRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundBottomLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roundTopLeft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ItemPanel itemPanel1;
        private ButtonItem buttonRect;
        private ButtonItem buttonRound;
        private LabelItem labelItem1;
        private ItemContainer itemContainer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private TabControl tabControl1;
        private TabControlPanel tabControlPanel1;
        private TabItem tabDefault;
        private ButtonItem buttonDefault;
        private TabControlPanel tabControlPanel3;
        private TabItem tabRound;
        private TabControlPanel tabControlPanel2;
        private TabItem tabRect;
        private DevComponents.Editors.IntegerInput roundBottomRight;
        private DevComponents.Editors.IntegerInput roundTopRight;
        private DevComponents.Editors.IntegerInput roundBottomLeft;
        private DevComponents.Editors.IntegerInput roundTopLeft;
        private ButtonX buttonRoundPreview;
        private ButtonItem buttonEllipse;
        private TabControlPanel tabControlPanel4;
        private TabItem tabItem1;

    }
}
#endif