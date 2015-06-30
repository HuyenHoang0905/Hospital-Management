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
    public partial class FormQLBNKCLS : Form
    {
<<<<<<< HEAD
        //public BUSBenhNhanChoKhamCLS BusBNChoKhamCLS;
        public FormQLBNKCLS()
        {
            InitializeComponent();
            //BusBNChoKhamCLS = new BUSBenhNhanChoKhamCLS();
            //DataTable dt = new System.Data.DataTable();
            //dt = BusBNChoKhamCLS.GetBenhNhan();
            //DGVBenhNhan.DataSource = dt;
=======
        public BUSBenhNhanChoKhamCLS BusBNChoKhamCLS;
        public BUSPhieuKhamCLS BusPhieuKhamCLS;
        public FormQLBNKCLS()
        {
            InitializeComponent();
            DTGVCapNhatKQ.Hide();
            BusBNChoKhamCLS = new BUSBenhNhanChoKhamCLS();
            BusPhieuKhamCLS = new BUSPhieuKhamCLS();
            DataTable dt = new System.Data.DataTable();
            dt = BusBNChoKhamCLS.GetBenhNhan();
            dt = this.ConvertTrangThaiThuTien(dt);
            DGVBenhNhan.DataSource = dt;
>>>>>>> origin/master
        }

        private void lậpBáoCáoThốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void BTThemPhieuCLS_Click(object sender, EventArgs e)
        {
            if (TXTMaBN.Text != "" || RDBNChoKham.Checked == true)
            {
                FormThemPhieuKham formThemPhieuKham = new FormThemPhieuKham();
                formThemPhieuKham.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 bệnh nhân để thêm hồ sơ bệnh án cho bệnh nhân đó!");
            }
        }

        private void BTCapNhatKQ_Click(object sender, EventArgs e)
        {
            if (TXTMaBN.Text != "" || RDListBNUpdateKQ.Checked == true)
            {
                FormCapNhatKetQua formCapNhatKQ = new FormCapNhatKetQua();
                formCapNhatKQ.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 bệnh nhân để cập nhật kết quả khám CLS cho bệnh nhân đó!");
            }

        }

        private void BTXemKQ_Click(object sender, EventArgs e)
        {
            if (TXTMaBN.Text != "" || RDBNChuaChuyenKQ.Checked == true)
            {
                FormXemKetQua formXemKQ = new FormXemKetQua();
                formXemKQ.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 bệnh nhân để xem kết quả của bệnh nhân đó!");
            }

        }

        private void quảnLýVậtTưYTếToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQLVatTuYTePKCLS formQLVatTuYTe = new FormQLVatTuYTePKCLS();
            formQLVatTuYTe.Show();
        }

        private void FormQLBNKCLS_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dBBenhVienYHocCoTruyenDataSet.TblChoKhamCLS' table. You can move, or remove it, as needed.

        }

        private void DGVBenhNhan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = DGVBenhNhan.CurrentCell.RowIndex;
            String maBenhNhan = DGVBenhNhan.Rows[index].Cells[0].Value.ToString();
            DataTable dt = new DataTable();
            dt = BusBNChoKhamCLS.GetBenhNhanID(maBenhNhan);
            TXTMaBN.Text = dt.Rows[0][0].ToString();
            TXTMaHoSoBenhAn.Text = dt.Rows[0][1].ToString();
            TXTTenBN.Text = dt.Rows[0][2].ToString();
            DateTime dtNgaySinh = Convert.ToDateTime(dt.Rows[0][3].ToString());
            TXTNgaySinh.Text = dtNgaySinh.ToShortDateString();
            TXTDiaChi.Text = dt.Rows[0][4].ToString();
            TXTNoiLamViec.Text = dt.Rows[0][5].ToString();
            String gioiTinh = dt.Rows[0][6].ToString();
            if (gioiTinh == "True")
                TXTGioiTinh.Text = "Nam";
            else
                TXTGioiTinh.Text = "Nữ";
            TXTLyDoKham.Text = dt.Rows[0][7].ToString();
            DataTable dt1 = new DataTable();
            dt1 = BusBNChoKhamCLS.SearchBenhNhanID(dt.Rows[0][0].ToString());
            ClassVariableStatic.bnChoKhamCLS.MaBenhNhan = dt1.Rows[0][0].ToString();
            ClassVariableStatic.bnChoKhamCLS.MaBSYeuCau = dt1.Rows[0][3].ToString();
        }

        private void BtTimKiem_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = BusBNChoKhamCLS.SearchBenhNhanID(TXTSearch.Text);
            dt = this.ConvertTrangThaiThuTien(dt);
            DGVBenhNhan.DataSource = dt;
        }
        public DataTable ConvertTrangThaiThuTien(DataTable dt)
        {
            //clone datatable     
            DataTable dtCloned = dt.Clone();
            //change data type of column
            dtCloned.Columns[2].DataType = typeof(String);
            //import row to cloned datatable
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }
            for (int i = 0; i < dtCloned.Rows.Count; i++)
            {
                if (dtCloned.Rows[i][2].ToString() == "True")
                {
                    dtCloned.Rows[i][2] = "Đã thu";
                }
                else
                {
                    dtCloned.Rows[i][2] = "Chưa thu";

                }
            }
            return dtCloned;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void RDBNChoKham_CheckedChanged(object sender, EventArgs e)
        {
            DTGVCapNhatKQ.Hide();
            DGVBenhNhan.Show();
            DataTable dt = new System.Data.DataTable();
            dt = BusBNChoKhamCLS.GetBenhNhan();
            dt = this.ConvertTrangThaiThuTien(dt);
            DGVBenhNhan.DataSource = dt;
        }

        private void RDListBNUpdateKQ_CheckedChanged(object sender, EventArgs e)
        {
            DGVBenhNhan.Hide();
            DTGVCapNhatKQ.Show();
            DataTable dt = new System.Data.DataTable();
            dt = BusPhieuKhamCLS.SelectBNUpdateKQ();
            dt = this.ConvertTrangThaiThuTien(dt);
            DTGVCapNhatKQ.DataSource = dt;
        }

        private void DTGVCapNhatKQ_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = DTGVCapNhatKQ.CurrentCell.RowIndex;
            String maBenhNhan = DTGVCapNhatKQ.Rows[index].Cells[0].Value.ToString();
            DataTable dt = new DataTable();
            dt = BusBNChoKhamCLS.GetBenhNhanID(maBenhNhan);
            TXTMaBN.Text = dt.Rows[0][0].ToString();
            TXTMaHoSoBenhAn.Text = dt.Rows[0][1].ToString();
            TXTTenBN.Text = dt.Rows[0][2].ToString();
            DateTime dtNgaySinh = Convert.ToDateTime(dt.Rows[0][3].ToString());
            TXTNgaySinh.Text = dtNgaySinh.ToShortDateString();
            TXTDiaChi.Text = dt.Rows[0][4].ToString();
            TXTNoiLamViec.Text = dt.Rows[0][5].ToString();
            String gioiTinh = dt.Rows[0][6].ToString();
            if (gioiTinh == "True")
                TXTGioiTinh.Text = "Nam";
            else
                TXTGioiTinh.Text = "Nữ";
            TXTLyDoKham.Text = dt.Rows[0][7].ToString();
            String maPhieuKham = DTGVCapNhatKQ.Rows[index].Cells[1].Value.ToString();
            ClassVariableStatic.bnCapNhatKQ.MaPhieuKham = maPhieuKham;
        }

        private void RDBNChuaChuyenKQ_CheckedChanged(object sender, EventArgs e)
        {
            DGVBenhNhan.Hide();
            DTGVCapNhatKQ.Show();
            DataTable dt = new System.Data.DataTable();
            dt = BusPhieuKhamCLS.SelectBNChuaChuyenKQ();
            dt = this.ConvertTrangThaiThuTien(dt);
            DTGVCapNhatKQ.DataSource = dt;
        }
    }
}
