namespace Quanlykhambenh_BVYHCT
{
    partial class FormCapNhatKetQua
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCapNhatKetQua));
            this.TXTMaBN = new System.Windows.Forms.TextBox();
            this.TXTMaPhieuKham = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TXTLoaiXetNghiem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BTChonFile = new System.Windows.Forms.Button();
            this.PictureBoxKQ = new System.Windows.Forms.PictureBox();
            this.openFileKQ = new System.Windows.Forms.OpenFileDialog();
            this.TXTCapNhatKQ = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TXTLoaiDanhMuc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BTCapNhatKQ = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxKQ)).BeginInit();
            this.SuspendLayout();
            // 
            // TXTMaBN
            // 
            this.TXTMaBN.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TXTMaBN.Enabled = false;
            this.TXTMaBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMaBN.Location = new System.Drawing.Point(362, 26);
            this.TXTMaBN.Name = "TXTMaBN";
            this.TXTMaBN.Size = new System.Drawing.Size(234, 20);
            this.TXTMaBN.TabIndex = 29;
            // 
            // TXTMaPhieuKham
            // 
            this.TXTMaPhieuKham.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TXTMaPhieuKham.Enabled = false;
            this.TXTMaPhieuKham.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMaPhieuKham.Location = new System.Drawing.Point(362, 3);
            this.TXTMaPhieuKham.Name = "TXTMaPhieuKham";
            this.TXTMaPhieuKham.Size = new System.Drawing.Size(234, 20);
            this.TXTMaPhieuKham.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(188, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Mã bệnh nhân: ";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(188, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Mã phiếu khám: ";
            // 
            // TXTLoaiXetNghiem
            // 
            this.TXTLoaiXetNghiem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TXTLoaiXetNghiem.Enabled = false;
            this.TXTLoaiXetNghiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTLoaiXetNghiem.Location = new System.Drawing.Point(362, 72);
            this.TXTLoaiXetNghiem.Name = "TXTLoaiXetNghiem";
            this.TXTLoaiXetNghiem.Size = new System.Drawing.Size(234, 20);
            this.TXTLoaiXetNghiem.TabIndex = 32;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(188, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Loại xét nghiệm:";
            // 
            // BTChonFile
            // 
            this.BTChonFile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTChonFile.Image = ((System.Drawing.Image)(resources.GetObject("BTChonFile.Image")));
            this.BTChonFile.Location = new System.Drawing.Point(617, 84);
            this.BTChonFile.Name = "BTChonFile";
            this.BTChonFile.Size = new System.Drawing.Size(101, 41);
            this.BTChonFile.TabIndex = 34;
            this.BTChonFile.Text = "Chọn file";
            this.BTChonFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTChonFile.UseVisualStyleBackColor = true;
            this.BTChonFile.Click += new System.EventHandler(this.BTChonFile_Click);
            // 
            // PictureBoxKQ
            // 
            this.PictureBoxKQ.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBoxKQ.Location = new System.Drawing.Point(12, 164);
            this.PictureBoxKQ.Name = "PictureBoxKQ";
            this.PictureBoxKQ.Size = new System.Drawing.Size(793, 492);
            this.PictureBoxKQ.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxKQ.TabIndex = 35;
            this.PictureBoxKQ.TabStop = false;
            // 
            // openFileKQ
            // 
            this.openFileKQ.FileName = "openFileKQ";
            // 
            // TXTCapNhatKQ
            // 
            this.TXTCapNhatKQ.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TXTCapNhatKQ.Enabled = false;
            this.TXTCapNhatKQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTCapNhatKQ.Location = new System.Drawing.Point(362, 95);
            this.TXTCapNhatKQ.Name = "TXTCapNhatKQ";
            this.TXTCapNhatKQ.Size = new System.Drawing.Size(234, 20);
            this.TXTCapNhatKQ.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(188, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "Cập nhật kết quả:";
            // 
            // TXTLoaiDanhMuc
            // 
            this.TXTLoaiDanhMuc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TXTLoaiDanhMuc.Enabled = false;
            this.TXTLoaiDanhMuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTLoaiDanhMuc.Location = new System.Drawing.Point(362, 49);
            this.TXTLoaiDanhMuc.Name = "TXTLoaiDanhMuc";
            this.TXTLoaiDanhMuc.Size = new System.Drawing.Size(234, 20);
            this.TXTLoaiDanhMuc.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(188, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Loại danh mục:";
            // 
            // BTCapNhatKQ
            // 
            this.BTCapNhatKQ.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BTCapNhatKQ.Location = new System.Drawing.Point(377, 121);
            this.BTCapNhatKQ.Name = "BTCapNhatKQ";
            this.BTCapNhatKQ.Size = new System.Drawing.Size(101, 41);
            this.BTCapNhatKQ.TabIndex = 40;
            this.BTCapNhatKQ.Text = "Cập nhật kết quả";
            this.BTCapNhatKQ.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTCapNhatKQ.UseVisualStyleBackColor = true;
            this.BTCapNhatKQ.Click += new System.EventHandler(this.BTCapNhatKQ_Click);
            // 
            // FormCapNhatKetQua
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 668);
            this.Controls.Add(this.BTCapNhatKQ);
            this.Controls.Add(this.TXTLoaiDanhMuc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TXTCapNhatKQ);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PictureBoxKQ);
            this.Controls.Add(this.BTChonFile);
            this.Controls.Add(this.TXTLoaiXetNghiem);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TXTMaBN);
            this.Controls.Add(this.TXTMaPhieuKham);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCapNhatKetQua";
            this.Text = "Cập nhật kết quả bệnh nhân";
            this.Load += new System.EventHandler(this.FormCapNhatKetQua_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxKQ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TXTMaBN;
        private System.Windows.Forms.TextBox TXTMaPhieuKham;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TXTLoaiXetNghiem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BTChonFile;
        private System.Windows.Forms.PictureBox PictureBoxKQ;
        private System.Windows.Forms.OpenFileDialog openFileKQ;
        private System.Windows.Forms.TextBox TXTCapNhatKQ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TXTLoaiDanhMuc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BTCapNhatKQ;
    }
}