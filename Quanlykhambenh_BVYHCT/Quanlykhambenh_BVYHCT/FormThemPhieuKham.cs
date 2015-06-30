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
        public FormThemPhieuKham()
        {
            InitializeComponent();
            busDanhMucKhamCLS = new BUSDanhMucKhamCLS();
            busNguoiDung = new BUSNguoiDung();
            busPhieuKhamCLS = new BUSPhieuKhamCLS();
            TXTMaBN.Text = ClassVariableStatic.bnChoKhamCLS.MaBenhNhan;
            TXTBSYeuCau.Text = ClassVariableStatic.bnChoKhamCLS.MaBSYeuCau;
            DataTable dt = busNguoiDung.GetSelect(TXTBSYeuCau.Text.Trim());
            TXTTenBSYeuCau.Text = dt.Rows[0][0].ToString();
            TXTNgayGioThucHien.Text = DateTime.Now.ToString();
        }

        private void FormThemPhieuKham_Load(object sender, EventArgs e)
        {
            this.tblLoaiKhamCLSTableAdapter.Fill(this.dBBenhVienYHocCoTruyenDataSet.TblLoaiKhamCLS);
        }

        private void CBLoaiDanhMucCLS_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String loaiCLS = CBLoaiDanhMucCLS.SelectedValue + "";
            dt = busDanhMucKhamCLS.SelectDanhMucKhamCLS(loaiCLS);
            Dictionary<string, string> dicLoaiXetNghiem = new Dictionary<string, string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dicLoaiXetNghiem.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
            }
            CBLoaiXetNghiem.DataSource = new BindingSource(dicLoaiXetNghiem, null);
            CBLoaiXetNghiem.DisplayMember = "Value";
            CBLoaiXetNghiem.ValueMember = "Key";
            CBLoaiXetNghiem.Text = dt.Rows[0][1].ToString();
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
                MessageBox.Show("Đã thêm phiếu khám CLS thành công!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra dữ liệu nhập vào!!");
            }
        }
    }
}
