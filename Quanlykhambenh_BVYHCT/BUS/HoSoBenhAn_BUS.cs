using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace BUS
{
    public class HoSoBenhAn_BUS
    {

        public HoSoBenhAn_Entities HSBA_En;
        private DataConnect connect;
        private SqlCommand cmd;
        private SqlDataAdapter dataAP;
        private DataTable dataTable;

        public HoSoBenhAn_BUS()
        {
            HSBA_En = new HoSoBenhAn_Entities();
            connect = new DataConnect();
        }

        public DataTable ThongTinHSBA()
        {
            string sql = "select *from TblHoSoBenhAn order by MaHSBA";
            return connect.getData(sql);
        }
        public Boolean them(HoSoBenhAn_Entities HSBA_En)
        {
            connect.openConnect();
            cmd = new SqlCommand("Them_HSBA", connect.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaHSBA ", HSBA_En.maHSBA);
            cmd.Parameters.AddWithValue("@Trang", HSBA_En.trang);
            cmd.Parameters.AddWithValue("@TieuHoa", HSBA_En.tieuHoa);
            cmd.Parameters.AddWithValue("@ThanTietNieu", HSBA_En.thanTietNieu);
            cmd.Parameters.AddWithValue("@CoXuongKhop", HSBA_En.coXuongKhop);
            cmd.Parameters.AddWithValue("@TamThan", HSBA_En.tamThan);
            cmd.Parameters.AddWithValue("@TuanHoan", HSBA_En.tuanHoan);
            cmd.Parameters.AddWithValue("@ThanKinh", HSBA_En.thanKinh);
            cmd.Parameters.AddWithValue("@NgoaiKhoa", HSBA_En.ngoaiKhoa);
            cmd.Parameters.AddWithValue("@HamTren", HSBA_En.hamTren);
            cmd.Parameters.AddWithValue("@XetNghiemNuocTieu", HSBA_En.xetNghiemNuocTieu);
            cmd.Parameters.AddWithValue("@XetNghiemSinhHoa", HSBA_En.xetNghiemSinhHoa);
            cmd.Parameters.AddWithValue("@XetNghiemTeBao", HSBA_En.xetNghiemTeBao);
            cmd.Parameters.AddWithValue("@NhipTim", HSBA_En.nhipTim);
            cmd.Parameters.AddWithValue("@HuyetAp", HSBA_En.huyetAp);
            cmd.Parameters.AddWithValue("@ChieuCao", HSBA_En.chieuCao);
            cmd.Parameters.AddWithValue("@CanNang", HSBA_En.canNang);
            cmd.Parameters.AddWithValue("@ThoiGianKham", HSBA_En.thoiGianKham);
            cmd.Parameters.AddWithValue("@TrangThai", HSBA_En.trangThai);
            cmd.Parameters.AddWithValue("@MaBenhNhan ", HSBA_En.maBenhNhan);
            cmd.Parameters.AddWithValue("@KetQua", HSBA_En.KetQua);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
               
                return false;
            }
            finally
            {
                connect.closeConnect();
            }
        }

        public Boolean sua(HoSoBenhAn_Entities HSBA_En)
        {
            connect.openConnect();
            cmd = new SqlCommand("Sua_HSBA", connect.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaHSBA", HSBA_En.maHSBA);
            cmd.Parameters.AddWithValue("@Trang", HSBA_En.trang);
            cmd.Parameters.AddWithValue("@TieuHoa", HSBA_En.tieuHoa);
            cmd.Parameters.AddWithValue("@ThanTietNieu", HSBA_En.thanTietNieu);
            cmd.Parameters.AddWithValue("@CoXuongKhop", HSBA_En.coXuongKhop);
            cmd.Parameters.AddWithValue("@TamThan", HSBA_En.tamThan);
            cmd.Parameters.AddWithValue("@TuanHoan", HSBA_En.tuanHoan);
            cmd.Parameters.AddWithValue("@ThanKinh", HSBA_En.thanKinh);
            cmd.Parameters.AddWithValue("@NgoaiKhoa", HSBA_En.ngoaiKhoa);
            cmd.Parameters.AddWithValue("@HamTren", HSBA_En.hamTren);
            cmd.Parameters.AddWithValue("@XetNghiemNuocTieu", HSBA_En.xetNghiemNuocTieu);
            cmd.Parameters.AddWithValue("@XetNghiemSinhHoa", HSBA_En.xetNghiemSinhHoa);
            cmd.Parameters.AddWithValue("@XetNghiemTeBao", HSBA_En.xetNghiemTeBao);
            cmd.Parameters.AddWithValue("@NhipTim", HSBA_En.nhipTim);
            cmd.Parameters.AddWithValue("@HuyetAp", HSBA_En.huyetAp);
            cmd.Parameters.AddWithValue("@ChieuCao", HSBA_En.chieuCao);
            cmd.Parameters.AddWithValue("@CanNang", HSBA_En.canNang);
            cmd.Parameters.AddWithValue("@ThoiGianKham", HSBA_En.thoiGianKham);
            cmd.Parameters.AddWithValue("@TrangThai", HSBA_En.trangThai);
            cmd.Parameters.AddWithValue("@MaBenhNhan ", HSBA_En.maBenhNhan);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connect.closeConnect();
            }

           
        }

        public Boolean xoa(string ma)
        {
            string sql = "delete from TblHoSoBenhAn where MaHSBA='" + ma + "'";
            if (connect.ExcuteQuery(sql))
            {
                //MessageBox.Show("Xóa thành công", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            return false;
        }

       

        public DataTable timkiem_MaHSBA(string MaHoSoBenhAn)
        {
            string sql = "Select * From	TblHoSoBenhAn WHERE MaHSBA  Like N'%" + MaHoSoBenhAn + "%'";
            return connect.getData(sql);
        }

        public DataTable timkiem_MaBenhNhan(string MaBenhNhan)
        {
            string sql = "Select * From	TblHoSoBenhAn WHERE MaBenhNhan  Like N'%" + MaBenhNhan + "%'";
            return connect.getData(sql);
        }

        public DataTable timkiem_Trang(string Trang)
        {
            string sql = "Select * From	TblHoSoBenhAn WHERE Trang  Like N'%" + Trang + "%'";
            return connect.getData(sql);
        }
        

    }
}
