using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entities;
using BUS;

namespace Quanlykhambenh_BVYHCT
{
    public partial class FormCapNhatKetQua : Form
    {
        public BUSPhieuKhamCLS BusPhieuKhamCLS;
        public FormCapNhatKetQua()
        {
            InitializeComponent();
            BusPhieuKhamCLS = new BUSPhieuKhamCLS();
        }

        private void FormCapNhatKetQua_Load(object sender, EventArgs e)
        {
            TXTMaPhieuKham.Text = ClassVariableStatic.bnCapNhatKQ.MaPhieuKham;
            DataTable dt = new DataTable();
            dt = BusPhieuKhamCLS.SelectBNMaBN(ClassVariableStatic.bnCapNhatKQ.MaPhieuKham);
            TXTMaBN.Text = dt.Rows[0][0].ToString();
            TXTMaPhieuKham.Text = dt.Rows[0][1].ToString();
            TXTLoaiDanhMuc.Text = dt.Rows[0][2].ToString();
            TXTLoaiXetNghiem.Text = dt.Rows[0][3].ToString();
        }

        private void BTChonFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileKQ.ShowDialog();
            if (dr == DialogResult.OK)
            {
                String stringKQ = openFileKQ.FileName.ToString();
                String OnlyNameFile = openFileKQ.SafeFileName;
                String[] temp = new String[2];
                temp = stringKQ.Split('.');
                if (temp[1] == "jpg" || temp[1] == "JPG" || temp[1] == "png" || temp[1] == "PNG")
                {
                    TXTCapNhatKQ.Text = OnlyNameFile;
                    String PathFile = String.Format(@"..\..\Images\{0}", OnlyNameFile);
                    Image image = Image.FromFile(PathFile);
                    PictureBoxKQ.Image = image;
                }
            }
        }

        private void BTCapNhatKQ_Click(object sender, EventArgs e)
        {
            try
            {
                BusPhieuKhamCLS.UpdateKQPhieuKhamCLS(TXTCapNhatKQ.Text.Trim(), TXTMaPhieuKham.Text.Trim());
                MessageBox.Show("Cập nhật thành công kết quả khám của bệnh nhân!!");
            }
            catch (Exception) {
                MessageBox.Show("Vui lòng kiểm tra dữ liệu nhập vào!!");
            }
        }
    }
}
