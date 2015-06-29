using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALBenhNhanChoKhamCLS
    {
        public ConnectDB ConnectionDB;
        public DALBenhNhanChoKhamCLS()
        {
            ConnectionDB = new ConnectDB();
        }
        public DataTable GetBenhNhan()
        {
            String query = @"SELECT [TblChoKhamCLS].MaBenhNhan, TenBN,"
                + @" MaYeuCau From [TblChoKhamCLS] "
                + @"inner join TblBenhNhan on [TblChoKhamCLS].MaBenhNhan=[TblBenhNhan].MaBenhNhan"
                + @" Where TrangThaiThanhToan='True'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
        public DataTable GetBenhNhanID(String maBN)
        {
            DataTable dt = new DataTable();
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@mabn", maBN);
            dt = ConnectionDB.ExecuteReaderParmarter("SelectBenhNhan", p);
            return dt;
        }
        public DataTable SearchBenhNhanMaYeuCau(String MaYeuCau)
        {
            String query = @"SELECT [TblChoKhamCLS].MaBenhNhan, TenBN,"
                + @" MaYeuCau, MaBSYeuCau From [TblChoKhamCLS] "
                + @"inner join TblBenhNhan on [TblChoKhamCLS].MaBenhNhan=[TblBenhNhan].MaBenhNhan"
                + @" Where [TblChoKhamCLS].MaYeuCau='" + MaYeuCau + "'"
                + @" and TrangThaiThanhToan='True'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
        public DataTable SelectBNMaYeuCau(String MaYeuCau)
        {
            String query = @"SELECT TenKhamCLS,TenChiTietKham FROM TblChoKhamCLS"
                +" inner join TblChiTietKham on TblChoKhamCLS.MaChiTietKham=TblChiTietKham.MaChiTietKham"
                +" inner join TblLoaiKhamCLS on TblChoKhamCLS.MaLoaiKhamCLS=TblLoaiKhamCLS.MaLoaiKhamCLS"
                +" Where MaYeuCau='" + MaYeuCau + "'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
        public void DeleteBNMaYeuCau(String MaYeuCau)
        {
            String query = @"Delete TblChoKhamCLS"
                + " Where MaYeuCau='" + MaYeuCau + "'";
            ConnectionDB.ExecuteSelect(query);
        }
    }
}
