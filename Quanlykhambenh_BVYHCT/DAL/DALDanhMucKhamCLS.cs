using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class DALDanhMucKhamCLS
    {
        public ConnectDB ConnectionDB;
        public DALDanhMucKhamCLS()
        {
            ConnectionDB = new ConnectDB();
        }
        public DataTable GetDanhMucKhamCLS(String LoaiKhamCLS)
        {
            String query = @"SELECT [MaChiTietKham],[TenChiTietKham] From [TblChiTietKham] "
                + @" Where[MaLoaiKhamCLS]='" + LoaiKhamCLS + "'";
            DataTable dt = new DataTable();
            dt = ConnectionDB.ExecuteSelect(query);
            return dt;
        } 
    }
}
