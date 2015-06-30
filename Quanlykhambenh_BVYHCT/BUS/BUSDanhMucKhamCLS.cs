using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data;
using System.Data.SqlClient;

namespace BUS
{
    public class BUSDanhMucKhamCLS
    {
        public DALDanhMucKhamCLS dalDanhMucKhamCLS;
        public BUSDanhMucKhamCLS()
        {
            dalDanhMucKhamCLS = new DALDanhMucKhamCLS();
        }
        public DataTable SelectDanhMucKhamCLS(String LoaiCLS)
        {
            DataTable dt = new DataTable();
            dt = dalDanhMucKhamCLS.GetDanhMucKhamCLS(LoaiCLS);
            return dt;
        }
    }
}
