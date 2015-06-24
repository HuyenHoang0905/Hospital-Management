namespace DevComponents.Instrumentation.Design
{
    partial class FormatStringDropDown
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._CbxFmtType = new System.Windows.Forms.ComboBox();
            this._LbFmtType = new System.Windows.Forms.Label();
            this._LbPrecision = new System.Windows.Forms.Label();
            this._LbSample = new System.Windows.Forms.Label();
            this._TbSample = new System.Windows.Forms.TextBox();
            this._LbDescription = new System.Windows.Forms.Label();
            this._TbCustom = new System.Windows.Forms.TextBox();
            this._TbPrecision = new System.Windows.Forms.TextBox();
            this._BtnOk = new System.Windows.Forms.Button();
            this._BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _CbxFmtType
            // 
            this._CbxFmtType.FormattingEnabled = true;
            this._CbxFmtType.Location = new System.Drawing.Point(95, 17);
            this._CbxFmtType.Name = "_CbxFmtType";
            this._CbxFmtType.Size = new System.Drawing.Size(223, 21);
            this._CbxFmtType.TabIndex = 0;
            this._CbxFmtType.SelectedIndexChanged += new System.EventHandler(this.CbxFmtType_SelectedIndexChanged);
            this._CbxFmtType.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MyPreviewKeyDown);
            // 
            // _LbFmtType
            // 
            this._LbFmtType.AutoSize = true;
            this._LbFmtType.Location = new System.Drawing.Point(20, 20);
            this._LbFmtType.Name = "_LbFmtType";
            this._LbFmtType.Size = new System.Drawing.Size(69, 13);
            this._LbFmtType.TabIndex = 1;
            this._LbFmtType.Text = "Format Type:";
            // 
            // _LbPrecision
            // 
            this._LbPrecision.AutoSize = true;
            this._LbPrecision.Location = new System.Drawing.Point(20, 44);
            this._LbPrecision.Name = "_LbPrecision";
            this._LbPrecision.Size = new System.Drawing.Size(53, 13);
            this._LbPrecision.TabIndex = 2;
            this._LbPrecision.Text = "Precision:";
            // 
            // _LbSample
            // 
            this._LbSample.AutoSize = true;
            this._LbSample.Location = new System.Drawing.Point(20, 70);
            this._LbSample.Name = "_LbSample";
            this._LbSample.Size = new System.Drawing.Size(45, 13);
            this._LbSample.TabIndex = 4;
            this._LbSample.Text = "Sample:";
            // 
            // _TbSample
            // 
            this._TbSample.BackColor = System.Drawing.SystemColors.Info;
            this._TbSample.ForeColor = System.Drawing.Color.Black;
            this._TbSample.Location = new System.Drawing.Point(95, 70);
            this._TbSample.Name = "_TbSample";
            this._TbSample.Size = new System.Drawing.Size(223, 20);
            this._TbSample.TabIndex = 5;
            this._TbSample.Text = "1234.56";
            this._TbSample.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MyPreviewKeyDown);
            this._TbSample.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxKeyPress);
            // 
            // _LbDescription
            // 
            this._LbDescription.Location = new System.Drawing.Point(15, 97);
            this._LbDescription.Name = "_LbDescription";
            this._LbDescription.Size = new System.Drawing.Size(391, 125);
            this._LbDescription.TabIndex = 6;
            this._LbDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _TbCustom
            // 
            this._TbCustom.Location = new System.Drawing.Point(95, 44);
            this._TbCustom.Name = "_TbCustom";
            this._TbCustom.Size = new System.Drawing.Size(223, 20);
            this._TbCustom.TabIndex = 7;
            this._TbCustom.Visible = false;
            this._TbCustom.TextChanged += new System.EventHandler(this.MyTextChanged);
            this._TbCustom.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MyPreviewKeyDown);
            // 
            // _TbPrecision
            // 
            this._TbPrecision.Location = new System.Drawing.Point(95, 44);
            this._TbPrecision.Name = "_TbPrecision";
            this._TbPrecision.Size = new System.Drawing.Size(223, 20);
            this._TbPrecision.TabIndex = 8;
            this._TbPrecision.Visible = false;
            this._TbPrecision.TextChanged += new System.EventHandler(this.MyTextChanged);
            this._TbPrecision.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MyPreviewKeyDown);
            // 
            // _BtnOk
            // 
            this._BtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._BtnOk.Location = new System.Drawing.Point(336, 26);
            this._BtnOk.Name = "_BtnOk";
            this._BtnOk.Size = new System.Drawing.Size(61, 23);
            this._BtnOk.TabIndex = 9;
            this._BtnOk.Text = "Ok";
            this._BtnOk.UseVisualStyleBackColor = true;
            this._BtnOk.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MyPreviewKeyDown);
            this._BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // _BtnCancel
            // 
            this._BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._BtnCancel.Location = new System.Drawing.Point(336, 55);
            this._BtnCancel.Name = "_BtnCancel";
            this._BtnCancel.Size = new System.Drawing.Size(61, 23);
            this._BtnCancel.TabIndex = 10;
            this._BtnCancel.Text = "Cancel";
            this._BtnCancel.UseVisualStyleBackColor = true;
            this._BtnCancel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MyPreviewKeyDown);
            this._BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FormatStringDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._BtnCancel);
            this.Controls.Add(this._BtnOk);
            this.Controls.Add(this._TbPrecision);
            this.Controls.Add(this._LbDescription);
            this.Controls.Add(this._TbSample);
            this.Controls.Add(this._LbSample);
            this.Controls.Add(this._LbPrecision);
            this.Controls.Add(this._LbFmtType);
            this.Controls.Add(this._CbxFmtType);
            this.Controls.Add(this._TbCustom);
            this.Name = "FormatStringDropDown";
            this.Size = new System.Drawing.Size(418, 222);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _CbxFmtType;
        private System.Windows.Forms.Label _LbFmtType;
        private System.Windows.Forms.Label _LbPrecision;
        private System.Windows.Forms.Label _LbSample;
        private System.Windows.Forms.TextBox _TbSample;
        private System.Windows.Forms.Label _LbDescription;
        private System.Windows.Forms.TextBox _TbCustom;
        private System.Windows.Forms.TextBox _TbPrecision;
        private System.Windows.Forms.Button _BtnOk;
        private System.Windows.Forms.Button _BtnCancel;
    }
}
