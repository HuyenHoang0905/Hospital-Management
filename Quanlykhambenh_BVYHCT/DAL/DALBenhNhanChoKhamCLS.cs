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
                + @" TrangThaiThanhToan From [TblChoKhamCLS] "
                + @"inner join TblBenhNhan on [TblChoKhamCLS].MaBenhNhan=[TblBenhNhan].MaBenhNhan";
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
        public DataTable SearchBenhNhanID(String MaBN)
        {
            String query = @"SELECT [TblChoKhamCLS].MaBenhNhan, TenBN,"
                + @" TrangThaiThanhToan, MaBSYeuCau From [TblChoKhamCLS] "
                + @"inner join TblBenhNhan on [TblChoKhamCLS].MaBenhNhan=[TblBenhNhan].MaBenhNhan"
                + @" Where [TblChoKhamCLS].MaBenhNhan='" + MaBN + "'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
    }
}
//"SELECT [TblChoKhamCLS].MaBenhNhan, TenBN, TrangThaiThanhToan 
//From [TblChoKhamCLS] inner join TblBenhNhan on [TblChoKhamCLS].MaBenhNhan=[TblBenhNhan].MaBenhNhanWhere [TblChoKhamCLS].MaBenhNhan='1'"