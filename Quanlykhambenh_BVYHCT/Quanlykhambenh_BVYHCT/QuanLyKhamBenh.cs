using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using Entities;

namespace Quanlykhambenh_BVYHCT
{
    public partial class QuanLyKhamBenh : Form
    {
        int i;
        public HoSoBenhAn_BUS HoSoBenhAnBus;
        public ChoKhamCLS_Bus ChoKhamCLSBus;
        public HoSoBenhAn_Entities HoSoBenhAn_En;
        public ChoKhamCLS_Entities ChoKhamCLS_En;
        private DataTable dt;
        private BindingSource bs;

        public QuanLyKhamBenh()
        {
            InitializeComponent();
            HoSoBenhAnBus = new HoSoBenhAn_BUS();
            HoSoBenhAn_En = new HoSoBenhAn_Entities();
            ChoKhamCLSBus = new ChoKhamCLS_Bus();
            ChoKhamCLS_En = new ChoKhamCLS_Entities();
            bs = new BindingSource();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
        }

        private void QuanLyKhamBenh_Load(object sender, EventArgs e)
        {
            dt = HoSoBenhAnBus.ThongTinHSBA();
            bs.DataSource = dt;

            
            //Load lên dataGridView
            dgvHSBA.DataSource = bs;

            txtMaHSBA.Enabled = false; txtTrang.Enabled = false;  txtTieuHoa.Enabled = false;
            txtThanTietNieu.Enabled = false; txtCoXuongKhop.Enabled = false; txtTamThan.Enabled = false;
            txtTuanHoan.Enabled = false; txtThanKinh.Enabled = false; txtNgoaiKhoa.Enabled = false;
            txtHamTren.Enabled = false; txtXetNghiemNuocTieu.Enabled = false; txtXetNghiemSinhHoa.Enabled = false;
            txtXetNghiemTeBao.Enabled = false; txtNhipTim.Enabled = false; txtHuyetAp.Enabled = false;
            txtChieuCao.Enabled = false; txtCanNang.Enabled = false; dtpThoiGianKham.Enabled = false;
            txtTrangThai.Enabled = false; txtMaBenhNhan.Enabled = false;

            txtmabn.Enabled = false; txtmalk.Enabled = false; txtmabsyc.Enabled = false; txt_trangthai.Enabled = false;
            

            btnHuy.Enabled = false; btnXoa.Enabled = true; btnLuu.Enabled = false; btnThem.Enabled = true;
            btnSua.Enabled = true; btnthemdechuyen.Enabled=true; btnchuyen.Enabled = false;
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void dgvHSBA_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            //HoSoBenhAn_Entities a = new HoSoBenhAn_Entities();
            //Boolean b = HoSoBenhAnBus.them(a);
            //if (b == true)
            //{
            //    MessageBox.Show("Thêm thành công ", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    MessageBox.Show("Vui lòng kiểm tra lại thông tin", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            txtMaHSBA.Enabled = true; txtTrang.Enabled = true; txtTieuHoa.Enabled = true;
            txtThanTietNieu.Enabled = true; txtCoXuongKhop.Enabled = true; txtTamThan.Enabled = true;
            txtTuanHoan.Enabled = true; txtThanKinh.Enabled = true; txtNgoaiKhoa.Enabled = true;
            txtHamTren.Enabled = true; txtXetNghiemNuocTieu.Enabled = true; txtXetNghiemSinhHoa.Enabled = true;
            txtXetNghiemTeBao.Enabled = true; txtNhipTim.Enabled = true; txtHuyetAp.Enabled = true;
            txtChieuCao.Enabled = true; txtCanNang.Enabled = true; dtpThoiGianKham.Enabled = true;
            txtTrangThai.Enabled = true; txtMaBenhNhan.Enabled = true;
            //---------------
            txtMaHSBA.Text = ""; txtTrang.Text = ""; txtTieuHoa.Text = "";
            txtThanTietNieu.Text = ""; txtCoXuongKhop.Text = ""; txtTamThan.Text = "";
            txtTuanHoan.Text = ""; txtThanKinh.Text = ""; txtNgoaiKhoa.Text = "";
            txtHamTren.Text = ""; txtXetNghiemNuocTieu.Text = ""; txtXetNghiemSinhHoa.Text = "";
            txtXetNghiemTeBao.Text = ""; txtNhipTim.Text = ""; txtHuyetAp.Text = "";
            txtChieuCao.Text = ""; txtCanNang.Text = ""; dtpThoiGianKham.Text = "";
            txtTrangThai.Text = ""; txtMaBenhNhan.Text = "";
            //---------------
            btnThem.Enabled = false; btnSua.Enabled = false; btnXoa.Enabled = false; btnHuy.Enabled = true; btnLuu.Enabled = true;
           
        }
         

        private void dgvHSBA_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            i = e.RowIndex;
            
            txtMaHSBA.Text = dgvHSBA.Rows[i].Cells[0].Value.ToString();
            txtTrang.Text = dgvHSBA.Rows[i].Cells[1].Value.ToString();
            txtTieuHoa.Text = dgvHSBA.Rows[i].Cells[2].Value.ToString();
            txtThanTietNieu.Text = dgvHSBA.Rows[i].Cells[3].Value.ToString();
            txtCoXuongKhop.Text = dgvHSBA.Rows[i].Cells[4].Value.ToString();
            txtTamThan.Text = dgvHSBA.Rows[i].Cells[5].Value.ToString();
            txtTuanHoan.Text = dgvHSBA.Rows[i].Cells[6].Value.ToString();
            txtThanKinh.Text = dgvHSBA.Rows[i].Cells[7].Value.ToString();
            txtNgoaiKhoa.Text = dgvHSBA.Rows[i].Cells[8].Value.ToString();
            txtHamTren.Text = dgvHSBA.Rows[i].Cells[9].Value.ToString();
            txtXetNghiemNuocTieu.Text = dgvHSBA.Rows[i].Cells[10].Value.ToString();
            txtXetNghiemSinhHoa.Text = dgvHSBA.Rows[i].Cells[11].Value.ToString();
            txtXetNghiemTeBao.Text = dgvHSBA.Rows[i].Cells[12].Value.ToString();
            txtNhipTim.Text = dgvHSBA.Rows[i].Cells[13].Value.ToString();
            txtHuyetAp.Text = dgvHSBA.Rows[i].Cells[14].Value.ToString();
            txtChieuCao.Text = dgvHSBA.Rows[i].Cells[15].Value.ToString();
            txtCanNang.Text = dgvHSBA.Rows[i].Cells[16].Value.ToString();
            dtpThoiGianKham.Text = dgvHSBA.Rows[i].Cells[17].Value.ToString();
            txtTrangThai.Text = dgvHSBA.Rows[i].Cells[18].Value.ToString();
            txtMaBenhNhan.Text = dgvHSBA.Rows[i].Cells[18].Value.ToString();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaHSBA.Text = dgvHSBA.Rows[i].Cells[0].Value.ToString();
            txtTrang.Text = dgvHSBA.Rows[i].Cells[1].Value.ToString();
            txtTieuHoa.Text = dgvHSBA.Rows[i].Cells[2].Value.ToString();
            txtThanTietNieu.Text = dgvHSBA.Rows[i].Cells[3].Value.ToString();
            txtCoXuongKhop.Text = dgvHSBA.Rows[i].Cells[4].Value.ToString();
            txtTamThan.Text = dgvHSBA.Rows[i].Cells[5].Value.ToString();
            txtTuanHoan.Text = dgvHSBA.Rows[i].Cells[6].Value.ToString();
            txtThanKinh.Text = dgvHSBA.Rows[i].Cells[7].Value.ToString();
            txtNgoaiKhoa.Text = dgvHSBA.Rows[i].Cells[8].Value.ToString();
            txtHamTren.Text = dgvHSBA.Rows[i].Cells[9].Value.ToString();
            txtXetNghiemNuocTieu.Text = dgvHSBA.Rows[i].Cells[10].Value.ToString();
            txtXetNghiemSinhHoa.Text = dgvHSBA.Rows[i].Cells[11].Value.ToString();
            txtXetNghiemTeBao.Text = dgvHSBA.Rows[i].Cells[12].Value.ToString();
            txtNhipTim.Text = dgvHSBA.Rows[i].Cells[13].Value.ToString();
            txtHuyetAp.Text = dgvHSBA.Rows[i].Cells[14].Value.ToString();
            txtChieuCao.Text = dgvHSBA.Rows[i].Cells[15].Value.ToString();
            txtCanNang.Text = dgvHSBA.Rows[i].Cells[16].Value.ToString();
            dtpThoiGianKham.Text = dgvHSBA.Rows[i].Cells[17].Value.ToString();
            txtTrangThai.Text = dgvHSBA.Rows[i].Cells[18].Value.ToString();
            txtMaBenhNhan.Text = dgvHSBA.Rows[i].Cells[19].Value.ToString();

            //-------------------
            txtMaHSBA.Enabled = false; txtTrang.Enabled = false; txtTieuHoa.Enabled = false;
            txtThanTietNieu.Enabled = false; txtCoXuongKhop.Enabled = false; txtTamThan.Enabled = false;
            txtTuanHoan.Enabled = false; txtThanKinh.Enabled = false; txtNgoaiKhoa.Enabled = false;
            txtHamTren.Enabled = false; txtXetNghiemNuocTieu.Enabled = false; txtXetNghiemSinhHoa.Enabled = false;
            txtXetNghiemTeBao.Enabled = false; txtNhipTim.Enabled = false; txtHuyetAp.Enabled = false;
            txtChieuCao.Enabled = false; txtCanNang.Enabled = false; dtpThoiGianKham.Enabled = false;
            txtTrangThai.Enabled = false; txtMaBenhNhan.Enabled = false;

            btnHuy.Enabled = false; btnXoa.Enabled = true; btnLuu.Enabled = false; btnThem.Enabled = true;
            btnSua.Enabled = true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không ", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("Bạn có muốn xóa nhân viên : " + txtMaHSBA.Text + "  không ?", "Hỏi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Boolean b = HoSoBenhAnBus.xoa(txtMaHSBA.Text);
                if (b == true)
                {
                    MessageBox.Show("Xóa thành công ", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xóa không thành công", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            dgvHSBA.DataSource = HoSoBenhAnBus.ThongTinHSBA();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTrang.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập trang");
                return;
            }
            if (txtTieuHoa.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập tiêu hóa");
                return;
            }
            if (txtThanTietNieu.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập thần tiết niệu");
                return;
            }
            if (txtCoXuongKhop.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập xương khớp");
                return;
            }
            if (txtTamThan.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập tâm thần ");
                return;
            }

            if(txtMaHSBA.Enabled == true)
            {
                HoSoBenhAn_En.maHSBA = int.Parse(txtMaHSBA.Text);
                HoSoBenhAn_En.trang = int.Parse(txtTrang.Text);
                HoSoBenhAn_En.tieuHoa = txtTieuHoa.Text;
                HoSoBenhAn_En.thanTietNieu = txtThanTietNieu.Text;
                HoSoBenhAn_En.coXuongKhop = txtCoXuongKhop.Text;
                HoSoBenhAn_En.tamThan = txtTamThan.Text;
                HoSoBenhAn_En.tuanHoan = txtTuanHoan.Text;
                HoSoBenhAn_En.thanKinh = txtThanKinh.Text;
                HoSoBenhAn_En.ngoaiKhoa = txtNgoaiKhoa.Text;
                HoSoBenhAn_En.hamTren = txtHamTren.Text;
                HoSoBenhAn_En.xetNghiemNuocTieu = txtXetNghiemNuocTieu.Text;
                HoSoBenhAn_En.xetNghiemSinhHoa = txtXetNghiemSinhHoa.Text;
                HoSoBenhAn_En.xetNghiemTeBao = txtXetNghiemTeBao.Text;
                HoSoBenhAn_En.nhipTim = txtNhipTim.Text;
                HoSoBenhAn_En.huyetAp = txtHuyetAp.Text;
                HoSoBenhAn_En.chieuCao = txtChieuCao.Text;
                HoSoBenhAn_En.canNang = txtCanNang.Text;
                HoSoBenhAn_En.thoiGianKham = Convert.ToDateTime(dtpThoiGianKham.Text);
                HoSoBenhAn_En.trangThai = txtTrangThai.Text;
                HoSoBenhAn_En.maBenhNhan = int.Parse(txtMaBenhNhan.Text);

                //HoSoBenhAnBus.them(HoSoBenhAn_En);
                Boolean b = HoSoBenhAnBus.them(HoSoBenhAn_En);
                if (b == true)
                {
                    MessageBox.Show("Thêm thành công ", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Vui lòng kiểm tra lại thông tin", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                QuanLyKhamBenh_Load(sender, e);
                return;
            }

            if(txtMaHSBA.Enabled==false)
            {
                if (MessageBox.Show("Bạn có muốn sửa hồ sơ bệnh án " + txtMaHSBA.Text + " không ?", "Hỏi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    HoSoBenhAn_En.maHSBA = int.Parse(txtMaHSBA.Text);
                    HoSoBenhAn_En.trang = int.Parse(txtTrang.Text);
                    HoSoBenhAn_En.tieuHoa = txtTieuHoa.Text;
                    HoSoBenhAn_En.thanTietNieu = txtThanTietNieu.Text;
                    HoSoBenhAn_En.coXuongKhop = txtCoXuongKhop.Text;
                    HoSoBenhAn_En.tamThan = txtTamThan.Text;
                    HoSoBenhAn_En.tuanHoan = txtTuanHoan.Text;
                    HoSoBenhAn_En.thanKinh = txtThanKinh.Text;
                    HoSoBenhAn_En.ngoaiKhoa = txtNgoaiKhoa.Text;
                    HoSoBenhAn_En.hamTren = txtHamTren.Text;
                    HoSoBenhAn_En.xetNghiemNuocTieu = txtXetNghiemNuocTieu.Text;
                    HoSoBenhAn_En.xetNghiemSinhHoa = txtXetNghiemSinhHoa.Text;
                    HoSoBenhAn_En.xetNghiemTeBao = txtXetNghiemTeBao.Text;
                    HoSoBenhAn_En.nhipTim = txtXetNghiemTeBao.Text;
                    HoSoBenhAn_En.huyetAp = txtHuyetAp.Text;
                    HoSoBenhAn_En.chieuCao = txtChieuCao.Text;
                    HoSoBenhAn_En.canNang = txtCanNang.Text;
                    HoSoBenhAn_En.thoiGianKham = Convert.ToDateTime(dtpThoiGianKham.Text);
                    HoSoBenhAn_En.TrangThai = txtTrangThai.Text;
                    HoSoBenhAn_En.maBenhNhan = int.Parse(txtMaBenhNhan.Text);


                    //HoSoBenhAnBus.them(HoSoBenhAn_En);
                    Boolean b = HoSoBenhAnBus.sua(HoSoBenhAn_En);
                    if (b == true)
                    {
                        MessageBox.Show("Sua thành công ", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng kiểm tra lại thông tin", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    QuanLyKhamBenh_Load(sender, e);
                    return;
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtMaHSBA.Enabled = false; txtTrang.Enabled = true; txtTieuHoa.Enabled = true;
            txtThanTietNieu.Enabled = true; txtCoXuongKhop.Enabled = true; txtTamThan.Enabled = true;
            txtTuanHoan.Enabled = true; txtThanKinh.Enabled = true; txtNgoaiKhoa.Enabled = true;
            txtHamTren.Enabled = true; txtXetNghiemNuocTieu.Enabled = true; txtXetNghiemSinhHoa.Enabled = true;
            txtXetNghiemTeBao.Enabled = true; txtNhipTim.Enabled = true; txtHuyetAp.Enabled = true;
            txtChieuCao.Enabled = true; txtCanNang.Enabled = true; dtpThoiGianKham.Enabled = true;
            txtTrangThai.Enabled = true; txtMaBenhNhan.Enabled = true;

            btnThem.Enabled = false; btnSua.Enabled = false; btnXoa.Enabled = false; btnHuy.Enabled = true; btnLuu.Enabled = true;
            
        }

        private void txtNoidungtk_TextChanged(object sender, EventArgs e)
        {
            if (cboTimKiem.SelectedItem == cboTimKiem.Items[0])
            {
                dt = HoSoBenhAnBus.timkiem_MaHSBA(txtNoidungtk.Text);
                bs.DataSource = dt;
                dgvHSBA.DataSource = bs;
            }
            else if(cboTimKiem.SelectedItem == cboTimKiem.Items[1])
            {

                dt = HoSoBenhAnBus.timkiem_MaBenhNhan(txtNoidungtk.Text);
                bs.DataSource = dt;
                dgvHSBA.DataSource = bs;
            }
            else
            {
                dt = HoSoBenhAnBus.timkiem_Trang(txtNoidungtk.Text);
                bs.DataSource = dt;
                dgvHSBA.DataSource = bs;
            }
            
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void btnthemdechuyen_Click(object sender, EventArgs e)
        {
            txtmabn.Enabled = true; txtmalk.Enabled = true; txtmabsyc.Enabled = true; txt_trangthai.Enabled = true;
            //---------------
            txtmabn.Text = ""; txtmalk.Text=""; txtmabsyc.Text = ""; txt_trangthai.Text = "";
            btnthemdechuyen.Enabled = false; btnchuyen.Enabled = true;
        }

        private void btnchuyen_Click(object sender, EventArgs e)
        {
            if (txtmabn.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập mã bệnh nhân");
                return;
            }
            if (txtmalk.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập mã loại khám");
                return;
            }
            

            if (txtmabn.Enabled == true)
            {
                ChoKhamCLS_En.maBenhNhan = int.Parse(txtmabn.Text);
                ChoKhamCLS_En.maLoaiKhamCLS = int.Parse(txtmalk.Text);
                ChoKhamCLS_En.maBSYeuCau = int.Parse(txtmabsyc.Text);
                ChoKhamCLS_En.tinhTrangThanhToan = txt_trangthai.Text;



                Boolean b = ChoKhamCLSBus.them_CLS(ChoKhamCLS_En);
                if (b == true)
                {
                    MessageBox.Show("Bệnh nhân đã được chuyển qua CLS ", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kiểm tra lại thông tin", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                QuanLyKhamBenh_Load(sender, e);
                return;
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            //dt = ChoKhamCLSBus.ThongTin();
            //bs.DataSource = dt;


            ////Load lên dataGridView
            //dgvCLS.DataSource = bs;
        }

    }
}
