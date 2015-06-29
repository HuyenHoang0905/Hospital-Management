namespace Quanlykhambenh_BVYHCT
{
    partial class FormXemKetQua
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXemKetQua));
            this.PTViewKQ = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PTViewKQ)).BeginInit();
            this.SuspendLayout();
            // 
            // PTViewKQ
            // 
            this.PTViewKQ.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PTViewKQ.Location = new System.Drawing.Point(95, 12);
            this.PTViewKQ.Name = "PTViewKQ";
            this.PTViewKQ.Size = new System.Drawing.Size(614, 461);
            this.PTViewKQ.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PTViewKQ.TabIndex = 0;
            this.PTViewKQ.TabStop = false;
            // 
            // FormXemKetQua
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 485);
            this.Controls.Add(this.PTViewKQ);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormXemKetQua";
            this.Text = "Xem kết quả";
            this.Load += new System.EventHandler(this.FormXemKetQua_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PTViewKQ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PTViewKQ;
    }
}