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
    }
}
