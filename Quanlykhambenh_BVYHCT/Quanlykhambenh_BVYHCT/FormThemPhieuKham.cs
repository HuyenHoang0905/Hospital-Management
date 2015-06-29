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
using System.Collections.Generic;
using BUS;
using Entities;

namespace Quanlykhambenh_BVYHCT
{
    public partial class FormThemPhieuKham : Form
    {
        public BUSDanhMucKhamCLS busDanhMucKhamCLS;
        public BUSNguoiDung busNguoiDung;
        public BUSPhieuKhamCLS busPhieuKhamCLS;
        public BUSBenhNhanChoKhamCLS busChoKhamCLS;
        public FormThemPhieuKham()
        {
            InitializeComponent();
            busDanhMucKhamCLS = new BUSDanhMucKhamCLS();
            busNguoiDung = new BUSNguoiDung();
            busPhieuKhamCLS = new BUSPhieuKhamCLS();
            busChoKhamCLS = new BUSBenhNhanChoKhamCLS();
        }

        private void FormThemPhieuKham_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dBBenhVienYHocCoTruyenDataSet1.TblChiTietKham' table. You can move, or remove it, as needed.
            this.tblChiTietKhamTableAdapter.Fill(this.dBBenhVienYHocCoTruyenDataSet1.TblChiTietKham);
            this.tblLoaiKhamCLSTableAdapter.Fill(this.dBBenhVienYHocCoTruyenDataSet.TblLoaiKhamCLS);
            TXTMaBN.Text = ClassVariableStatic.bnChoKhamCLS.MaBenhNhan;
            TXTBSYeuCau.Text = ClassVariableStatic.bnChoKhamCLS.MaBSYeuCau;
            try
            {
                DataTable dt = busNguoiDung.GetSelect(TXTBSYeuCau.Text.Trim());
                TXTTenBSYeuCau.Text = dt.Rows[0][0].ToString();
            }
            catch (Exception) { }
            TXTNgayGioThucHien.Text = DateTime.Now.ToString();
            DataTable dt1 = busChoKhamCLS.SelectBNMaYeuCau(ClassVariableStatic.bnChoKhamCLS.MaYeuCau);
            CBLoaiXetNghiem.Text = dt1.Rows[0][1].ToString();
            CBLoaiDanhMucCLS.Text = dt1.Rows[0][0].ToString();
        }

        private void CBLoaiDanhMucCLS_TextChanged(object sender, EventArgs e)
        {
        }

        private void TBThemPhieuKhamCLS_Click(object sender, EventArgs e)
        {
            try
            {
                PhieuKhamCLS pk = new PhieuKhamCLS();
                pk.MaBenhNhan = TXTMaBN.Text;
                pk.TenXetNghiem = CBLoaiXetNghiem.SelectedValue + "";
                pk.BacSiYeuCau = TXTTenBSYeuCau.Text;
                pk.DanhMucKham = CBLoaiDanhMucCLS.SelectedValue + "";
                pk.NguoiNhapLieu = TXTNguoiThucHien.Text;
                pk.NgayGioThucHien = Convert.ToDateTime(TXTNgayGioThucHien.Text);
                pk.NgayGioiTraKQ = Convert.ToDateTime(TXTNgayGioTraKQ.Text);
                pk.KetQua = "";
                busPhieuKhamCLS.InsertPhieuKhamCLS(pk);
                busChoKhamCLS.DeleteBNMaYeuCau(ClassVariableStatic.bnChoKhamCLS.MaYeuCau);
                MessageBox.Show("Đã thêm phiếu khám CLS thành công!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra dữ liệu nhập vào!!");
            }
        }
    }
}
