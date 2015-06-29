using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace DAL
{
    public class DALPhieuKhamCLS
    {
        public ConnectDB ConnectionDB;
        public DALPhieuKhamCLS()
        {
            ConnectionDB = new ConnectDB();
        }
        public Boolean InsertPhieuKhamCLS(PhieuKhamCLS pk)
        {
            SqlParameter[] p = new SqlParameter[8];
            p[0] = new SqlParameter("@MaBenhNhan", pk.MaBenhNhan);
            p[1] = new SqlParameter("@TenXetNghiem", pk.TenXetNghiem);
            p[2] = new SqlParameter("@BacSiYeuCau", pk.BacSiYeuCau);
            p[3] = new SqlParameter("@DanhMucKham", pk.DanhMucKham);
            p[4] = new SqlParameter("@NgayGioThucHien", pk.NgayGioThucHien);
            p[5] = new SqlParameter("@NgayGioTraKQ", pk.NgayGioiTraKQ);
            p[6] = new SqlParameter("@KetQuaXetNghiem", pk.KetQua);
            p[7] = new SqlParameter("@NguoiNhapLieu", pk.NguoiNhapLieu);
            try
            {
                ConnectionDB.ExecuteScalarParmarter("InsertPhieuKhamCLS", p);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DataTable SelectBNCapNhatKQ()
        {
            String query = @"SELECT TblPhieuKhamCLS.MaBenhNhan,MaPhieuKham,TrangThaiThanhToan From TblPhieuKhamCLS
                            inner join TblBenhNhan on TblPhieuKhamCLS.MaBenhNhan =TblBenhNhan.MaBenhNhan
                            inner join TblChoKhamCLS on TblPhieuKhamCLS.MaBenhNhan =TblChoKhamCLS.MaBenhNhan
                            where KetQuaXetNghiem='' and TblChoKhamCLS.TrangThaiThanhToan='True'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
        public DataTable SelectBNChuaChuyenKQ()
        {
            String query = @"SELECT TblPhieuKhamCLS.MaBenhNhan,MaPhieuKham,TrangThaiThanhToan From TblPhieuKhamCLS
                            inner join TblBenhNhan on TblPhieuKhamCLS.MaBenhNhan =TblBenhNhan.MaBenhNhan
                            inner join TblChoKhamCLS on TblPhieuKhamCLS.MaBenhNhan =TblChoKhamCLS.MaBenhNhan
                            where TrangThaiChuyenKQ='False'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
        public DataTable SelectBNMaBN(String maPhieuKham)
        {
            String query = @"SELECT [MaBenhNhan]
                                  ,MaPhieuKham
                                  ,TenKhamCLS
                                  ,TenChiTietKham
                              FROM [TblPhieuKhamCLS] inner join TblChiTietKham on TenXetNghiem=MaChiTietKham
                              inner join TblLoaiKhamCLS on TblLoaiKhamCLS.MaLoaiKhamCLS=DanhMucKham"
                            + @" Where MaPhieuKham= '" + maPhieuKham + "'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
        public DataTable SelectKQKhamBN(String maPhieuKham)
        {
            String query = @"SELECT [KetQuaXetNghiem] FROM [TblPhieuKhamCLS]"
                            + @" Where MaPhieuKham= '" + maPhieuKham + "'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
        public Boolean UpdateKQPhieuKhamCLS(String KetQua, String MaPhieuKham)
        {
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@MaPhieuKham", MaPhieuKham);
            p[1] = new SqlParameter("@KetQuaXetNghiem", KetQua);
            try
            {
                ConnectionDB.ExecuteScalarParmarter("UpdateKSPhieuKhamCLS", p);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
