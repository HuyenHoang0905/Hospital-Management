namespace DevComponents.DotNetBarKnobControl.Design
{
    partial class GaugePropertyGrid
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this._BtnOk = new System.Windows.Forms.Button();
            this._BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(386, 408);
            this.propertyGrid1.TabIndex = 0;
            // 
            // _BtnOk
            // 
            this._BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._BtnOk.Location = new System.Drawing.Point(214, 427);
            this._BtnOk.Name = "_BtnOk";
            this._BtnOk.Size = new System.Drawing.Size(75, 23);
            this._BtnOk.TabIndex = 1;
            this._BtnOk.Text = "Ok";
            this._BtnOk.UseVisualStyleBackColor = true;
            // 
            // _BtnCancel
            // 
            this._BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._BtnCancel.Location = new System.Drawing.Point(295, 427);
            this._BtnCancel.Name = "_BtnCancel";
            this._BtnCancel.Size = new System.Drawing.Size(75, 23);
            this._BtnCancel.TabIndex = 2;
            this._BtnCancel.Text = "Cancel";
            this._BtnCancel.UseVisualStyleBackColor = true;
            // 
            // GaugePropertyGrid
            // 
            this.AcceptButton = this._BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._BtnCancel;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this._BtnCancel);
            this.Controls.Add(this._BtnOk);
            this.Controls.Add(this.propertyGrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GaugePropertyGrid";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GaugePropertyGrid";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button _BtnOk;
        private System.Windows.Forms.Button _BtnCancel;
    }
}