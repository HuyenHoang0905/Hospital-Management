using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALNguoiDung
    {
        public ConnectDB ConnectionDB;
        public DALNguoiDung()
        {
            ConnectionDB = new ConnectDB();
        }
        public DataTable GetNguoiDung(String MaND)
        {
            DataTable dt = new DataTable();
            String query = "Select [TenNguoiDung] From [NguoiDung]" +
                " Where [TenDangNhap]='" + MaND + "'";
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        }
    }
}
