namespace Quanlykhambenh_BVYHCT
{
    partial class FormQLBNKCLS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQLBNKCLS));
            this.DGVBenhNhan = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BTCapNhatKQ = new System.Windows.Forms.Button();
            this.BTThemPhieuCLS = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýVậtTưYTếToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lậpBáoCáoThốngKêkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TXTLyDoKham = new System.Windows.Forms.TextBox();
            this.TXTGioiTinh = new System.Windows.Forms.TextBox();
            this.TXTNoiLamViec = new System.Windows.Forms.TextBox();
            this.TXTDiaChi = new System.Windows.Forms.TextBox();
            this.TXTNgaySinh = new System.Windows.Forms.TextBox();
            this.TXTTenBN = new System.Windows.Forms.TextBox();
            this.TXTMaHoSoBenhAn = new System.Windows.Forms.TextBox();
            this.TXTMaBN = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TXTSearch = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.BtTimKiem = new System.Windows.Forms.Button();
            this.BTXemKQ = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.RDBNChoKham = new System.Windows.Forms.RadioButton();
            this.RDListBNUpdateKQ = new System.Windows.Forms.RadioButton();
            this.DTGVCapNhatKQ = new System.Windows.Forms.DataGridView();
            this.RDBNChuaChuyenKQ = new System.Windows.Forms.RadioButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGVBenhNhan)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DTGVCapNhatKQ)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVBenhNhan
            // 
            this.DGVBenhNhan.AllowUserToAddRows = false;
            this.DGVBenhNhan.AllowUserToDeleteRows = false;
            this.DGVBenhNhan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVBenhNhan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.DGVBenhNhan.Location = new System.Drawing.Point(12, 123);
            this.DGVBenhNhan.Name = "DGVBenhNhan";
            this.DGVBenhNhan.ReadOnly = true;
            this.DGVBenhNhan.Size = new System.Drawing.Size(301, 358);
            this.DGVBenhNhan.TabIndex = 0;
            this.DGVBenhNhan.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVBenhNhan_CellClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "MaYeuCau";
            this.Column1.HeaderText = "Mã yêu cầu";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "MaBenhNhan";
            this.Column2.HeaderText = "Mã bệnh nhân";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "TenBN";
            this.Column3.HeaderText = "Tên bệnh nhân";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // BTCapNhatKQ
            // 
            this.BTCapNhatKQ.Image = ((System.Drawing.Image)(resources.GetObject("BTCapNhatKQ.Image")));
            this.BTCapNhatKQ.Location = new System.Drawing.Point(319, 261);
            this.BTCapNhatKQ.Name = "BTCapNhatKQ";
            this.BTCapNhatKQ.Size = new System.Drawing.Size(91, 51);
            this.BTCapNhatKQ.TabIndex = 3;
            this.BTCapNhatKQ.Text = "Cập nhật kết quả KCLS";
            this.BTCapNhatKQ.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTCapNhatKQ.UseVisualStyleBackColor = true;
            this.BTCapNhatKQ.Click += new System.EventHandler(this.BTCapNhatKQ_Click);
            // 
            // BTThemPhieuCLS
            // 
            this.BTThemPhieuCLS.Image = ((System.Drawing.Image)(resources.GetObject("BTThemPhieuCLS.Image")));
            this.BTThemPhieuCLS.Location = new System.Drawing.Point(319, 207);
            this.BTThemPhieuCLS.Name = "BTThemPhieuCLS";
            this.BTThemPhieuCLS.Size = new System.Drawing.Size(91, 44);
            this.BTThemPhieuCLS.TabIndex = 4;
            this.BTThemPhieuCLS.Text = "Thêm phiếu khám CLS";
            this.BTThemPhieuCLS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTThemPhieuCLS.UseVisualStyleBackColor = true;
            this.BTThemPhieuCLS.Click += new System.EventHandler(this.BTThemPhieuCLS_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(929, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quảnLýVậtTưYTếToolStripMenuItem,
            this.lậpBáoCáoThốngKêkToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.menuToolStripMenuItem.Text = "Mở rộng";
            // 
            // quảnLýVậtTưYTếToolStripMenuItem
            // 
            this.quảnLýVậtTưYTếToolStripMenuItem.Name = "quảnLýVậtTưYTếToolStripMenuItem";
            this.quảnLýVậtTưYTếToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.quảnLýVậtTưYTếToolStripMenuItem.Text = "Quản lý vật tư y tế";
            this.quảnLýVậtTưYTếToolStripMenuItem.Click += new System.EventHandler(this.quảnLýVậtTưYTếToolStripMenuItem_Click);
            // 
            // lậpBáoCáoThốngKêkToolStripMenuItem
            // 
            this.lậpBáoCáoThốngKêkToolStripMenuItem.Name = "lậpBáoCáoThốngKêkToolStripMenuItem";
            this.lậpBáoCáoThốngKêkToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.lậpBáoCáoThốngKêkToolStripMenuItem.Text = "Lập báo cáo thống kê";
            this.lậpBáoCáoThốngKêkToolStripMenuItem.Click += new System.EventHandler(this.lậpBáoCáoThốngKêToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TXTLyDoKham);
            this.groupBox1.Controls.Add(this.TXTGioiTinh);
            this.groupBox1.Controls.Add(this.TXTNoiLamViec);
            this.groupBox1.Controls.Add(this.TXTDiaChi);
            this.groupBox1.Controls.Add(this.TXTNgaySinh);
            this.groupBox1.Controls.Add(this.TXTTenBN);
            this.groupBox1.Controls.Add(this.TXTMaHoSoBenhAn);
            this.groupBox1.Controls.Add(this.TXTMaBN);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(416, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(501, 426);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin bệnh nhân";
            // 
            // TXTLyDoKham
            // 
            this.TXTLyDoKham.Enabled = false;
            this.TXTLyDoKham.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTLyDoKham.Location = new System.Drawing.Point(251, 330);
            this.TXTLyDoKham.Name = "TXTLyDoKham";
            this.TXTLyDoKham.Size = new System.Drawing.Size(234, 20);
            this.TXTLyDoKham.TabIndex = 15;
            // 
            // TXTGioiTinh
            // 
            this.TXTGioiTinh.Enabled = false;
            this.TXTGioiTinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTGioiTinh.Location = new System.Drawing.Point(251, 288);
            this.TXTGioiTinh.Name = "TXTGioiTinh";
            this.TXTGioiTinh.Size = new System.Drawing.Size(234, 20);
            this.TXTGioiTinh.TabIndex = 14;
            // 
            // TXTNoiLamViec
            // 
            this.TXTNoiLamViec.Enabled = false;
            this.TXTNoiLamViec.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTNoiLamViec.Location = new System.Drawing.Point(251, 246);
            this.TXTNoiLamViec.Name = "TXTNoiLamViec";
            this.TXTNoiLamViec.Size = new System.Drawing.Size(234, 20);
            this.TXTNoiLamViec.TabIndex = 13;
            // 
            // TXTDiaChi
            // 
            this.TXTDiaChi.Enabled = false;
            this.TXTDiaChi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTDiaChi.Location = new System.Drawing.Point(251, 204);
            this.TXTDiaChi.Name = "TXTDiaChi";
            this.TXTDiaChi.Size = new System.Drawing.Size(234, 20);
            this.TXTDiaChi.TabIndex = 12;
            // 
            // TXTNgaySinh
            // 
            this.TXTNgaySinh.Enabled = false;
            this.TXTNgaySinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTNgaySinh.Location = new System.Drawing.Point(251, 162);
            this.TXTNgaySinh.Name = "TXTNgaySinh";
            this.TXTNgaySinh.Size = new System.Drawing.Size(234, 20);
            this.TXTNgaySinh.TabIndex = 11;
            // 
            // TXTTenBN
            // 
            this.TXTTenBN.Enabled = false;
            this.TXTTenBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTTenBN.Location = new System.Drawing.Point(251, 120);
            this.TXTTenBN.Name = "TXTTenBN";
            this.TXTTenBN.Size = new System.Drawing.Size(234, 20);
            this.TXTTenBN.TabIndex = 10;
            // 
            // TXTMaHoSoBenhAn
            // 
            this.TXTMaHoSoBenhAn.Enabled = false;
            this.TXTMaHoSoBenhAn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMaHoSoBenhAn.Location = new System.Drawing.Point(251, 78);
            this.TXTMaHoSoBenhAn.Name = "TXTMaHoSoBenhAn";
            this.TXTMaHoSoBenhAn.Size = new System.Drawing.Size(234, 20);
            this.TXTMaHoSoBenhAn.TabIndex = 9;
            // 
            // TXTMaBN
            // 
            this.TXTMaBN.Enabled = false;
            this.TXTMaBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMaBN.Location = new System.Drawing.Point(251, 36);
            this.TXTMaBN.Name = "TXTMaBN";
            this.TXTMaBN.Size = new System.Drawing.Size(234, 20);
            this.TXTMaBN.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(42, 335);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Lý do khám chữa bệnh: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(42, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Nơi làm việc: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(42, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Giới tính: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(42, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Địa chỉ: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(42, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tên bệnh nhân: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mã hồ sơ bệnh án: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ngày sinh: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã bệnh nhân: ";
            // 
            // TXTSearch
            // 
            this.TXTSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTSearch.Location = new System.Drawing.Point(108, 25);
            this.TXTSearch.Name = "TXTSearch";
            this.TXTSearch.Size = new System.Drawing.Size(153, 20);
            this.TXTSearch.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Mã bệnh nhân: ";
            // 
            // BtTimKiem
            // 
            this.BtTimKiem.Image = ((System.Drawing.Image)(resources.GetObject("BtTimKiem.Image")));
            this.BtTimKiem.Location = new System.Drawing.Point(267, 19);
            this.BtTimKiem.Name = "BtTimKiem";
            this.BtTimKiem.Size = new System.Drawing.Size(91, 30);
            this.BtTimKiem.TabIndex = 11;
            this.BtTimKiem.Text = "Tìm kiếm";
            this.BtTimKiem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtTimKiem.UseVisualStyleBackColor = true;
            this.BtTimKiem.Click += new System.EventHandler(this.BtTimKiem_Click);
            // 
            // BTXemKQ
            // 
            this.BTXemKQ.Image = ((System.Drawing.Image)(resources.GetObject("BTXemKQ.Image")));
            this.BTXemKQ.Location = new System.Drawing.Point(319, 322);
            this.BTXemKQ.Name = "BTXemKQ";
            this.BTXemKQ.Size = new System.Drawing.Size(91, 44);
            this.BTXemKQ.TabIndex = 12;
            this.BTXemKQ.Text = "Xem kết quả khám";
            this.BTXemKQ.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BTXemKQ.UseVisualStyleBackColor = true;
            this.BTXemKQ.Click += new System.EventHandler(this.BTXemKQ_Click);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(319, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 52);
            this.button1.TabIndex = 13;
            this.button1.Text = "Chuyển kết quả";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RDBNChoKham
            // 
            this.RDBNChoKham.AutoSize = true;
            this.RDBNChoKham.Checked = true;
            this.RDBNChoKham.Location = new System.Drawing.Point(45, 51);
            this.RDBNChoKham.Name = "RDBNChoKham";
            this.RDBNChoKham.Size = new System.Drawing.Size(181, 17);
            this.RDBNChoKham.TabIndex = 14;
            this.RDBNChoKham.TabStop = true;
            this.RDBNChoKham.Text = "Danh sách bệnh nhân chờ khám";
            this.RDBNChoKham.UseVisualStyleBackColor = true;
            this.RDBNChoKham.CheckedChanged += new System.EventHandler(this.RDBNChoKham_CheckedChanged);
            // 
            // RDListBNUpdateKQ
            // 
            this.RDListBNUpdateKQ.AutoSize = true;
            this.RDListBNUpdateKQ.Location = new System.Drawing.Point(45, 74);
            this.RDListBNUpdateKQ.Name = "RDListBNUpdateKQ";
            this.RDListBNUpdateKQ.Size = new System.Drawing.Size(236, 17);
            this.RDListBNUpdateKQ.TabIndex = 15;
            this.RDListBNUpdateKQ.Text = "Danh sách bệnh nhân cần cập nhật kết quả";
            this.RDListBNUpdateKQ.UseVisualStyleBackColor = true;
            this.RDListBNUpdateKQ.CheckedChanged += new System.EventHandler(this.RDListBNUpdateKQ_CheckedChanged);
            // 
            // DTGVCapNhatKQ
            // 
            this.DTGVCapNhatKQ.AllowUserToAddRows = false;
            this.DTGVCapNhatKQ.AllowUserToDeleteRows = false;
            this.DTGVCapNhatKQ.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DTGVCapNhatKQ.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.DTGVCapNhatKQ.Location = new System.Drawing.Point(12, 123);
            this.DTGVCapNhatKQ.Name = "DTGVCapNhatKQ";
            this.DTGVCapNhatKQ.ReadOnly = true;
            this.DTGVCapNhatKQ.Size = new System.Drawing.Size(301, 358);
            this.DTGVCapNhatKQ.TabIndex = 16;
            this.DTGVCapNhatKQ.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DTGVCapNhatKQ_CellClick);
            // 
            // RDBNChuaChuyenKQ
            // 
            this.RDBNChuaChuyenKQ.AutoSize = true;
            this.RDBNChuaChuyenKQ.Location = new System.Drawing.Point(45, 97);
            this.RDBNChuaChuyenKQ.Name = "RDBNChuaChuyenKQ";
            this.RDBNChuaChuyenKQ.Size = new System.Drawing.Size(208, 17);
            this.RDBNChuaChuyenKQ.TabIndex = 17;
            this.RDBNChuaChuyenKQ.Text = "Danh sách bệnh chưa chuyển kết quả";
            this.RDBNChuaChuyenKQ.UseVisualStyleBackColor = true;
            this.RDBNChuaChuyenKQ.CheckedChanged += new System.EventHandler(this.RDBNChuaChuyenKQ_CheckedChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "MaBenhNhan";
            this.dataGridViewTextBoxColumn1.HeaderText = "Mã bệnh nhân";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "MaPhieuKham";
            this.dataGridViewTextBoxColumn2.HeaderText = "Mã phiếu khám";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // FormQLBNKCLS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 492);
            this.Controls.Add(this.RDBNChuaChuyenKQ);
            this.Controls.Add(this.DTGVCapNhatKQ);
            this.Controls.Add(this.RDListBNUpdateKQ);
            this.Controls.Add(this.RDBNChoKham);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BTXemKQ);
            this.Controls.Add(this.BtTimKiem);
            this.Controls.Add(this.TXTSearch);
            this.Controls.Add(this.DGVBenhNhan);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BTThemPhieuCLS);
            this.Controls.Add(this.BTCapNhatKQ);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormQLBNKCLS";
            this.Text = "Quản lý bệnh nhân khoa khám cận lâm sàng";
            this.Load += new System.EventHandler(this.FormQLBNKCLS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVBenhNhan)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DTGVCapNhatKQ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVBenhNhan;
        private System.Windows.Forms.Button BTCapNhatKQ;
        private System.Windows.Forms.Button BTThemPhieuCLS;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quảnLýVậtTưYTếToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lậpBáoCáoThốngKêkToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TXTLyDoKham;
        private System.Windows.Forms.TextBox TXTGioiTinh;
        private System.Windows.Forms.TextBox TXTNoiLamViec;
        private System.Windows.Forms.TextBox TXTDiaChi;
        private System.Windows.Forms.TextBox TXTNgaySinh;
        private System.Windows.Forms.TextBox TXTTenBN;
        private System.Windows.Forms.TextBox TXTMaHoSoBenhAn;
        private System.Windows.Forms.TextBox TXTMaBN;
        private System.Windows.Forms.TextBox TXTSearch;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtTimKiem;
        private System.Windows.Forms.Button BTXemKQ;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton RDBNChoKham;
        private System.Windows.Forms.RadioButton RDListBNUpdateKQ;
        private System.Windows.Forms.DataGridView DTGVCapNhatKQ;
        private System.Windows.Forms.RadioButton RDBNChuaChuyenKQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}

