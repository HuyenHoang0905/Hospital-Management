using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using BUS;
using Entities;

namespace Quanlykhambenh_BVYHCT
{
    public partial class FormThemPhieuKham : Form
    {
        public BUSDanhMucKhamCLS busDanhMucKhamCLS;
        public BUSNguoiDung busNguoiDung;
        public FormThemPhieuKham()
        {
            InitializeComponent();
            busDanhMucKhamCLS = new BUSDanhMucKhamCLS();
            busNguoiDung = new BUSNguoiDung();
            TXTMaBN.Text = ClassVariableStatic.bnChoKhamCLS.MaBenhNhan;
            TXTBSYeuCau.Text = ClassVariableStatic.bnChoKhamCLS.MaBSYeuCau;
            DataTable dt = busNguoiDung.GetSelect(TXTBSYeuCau.Text.Trim());
            TXTTenBSYeuCau.Text = dt.Rows[0][0].ToString();
            TXTNgayGioThucHien.Text = DateTime.Now.ToString();
        }

        private void FormThemPhieuKham_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dBBenhVienYHocCoTruyenDataSet.TblLoaiKhamCLS' table. You can move, or remove it, as needed.
            this.tblLoaiKhamCLSTableAdapter.Fill(this.dBBenhVienYHocCoTruyenDataSet.TblLoaiKhamCLS);
        }

        private void CBLoaiDanhMucCLS_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String loaiCLS = CBLoaiDanhMucCLS.SelectedValue + "";
            dt = busDanhMucKhamCLS.SelectDanhMucKhamCLS(loaiCLS);
            CBLoaiXetNghiem.Items.Clear();
            CBLoaiXetNghiem.DisplayMember = "Text";
            CBLoaiXetNghiem.ValueMember = "Value";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CBLoaiXetNghiem.Items.Add(new { Text = dt.Rows[i][1].ToString(), Value = dt.Rows[i][0].ToString() });
            }
            CBLoaiXetNghiem.Text = dt.Rows[0][1].ToString();
        }

        private void TBThemPhieuKhamCLS_Click(object sender, EventArgs e)
        {

        }
    }
}
