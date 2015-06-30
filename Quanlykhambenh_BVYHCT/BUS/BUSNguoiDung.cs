using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BUS
{
    public class BUSNguoiDung
    {
        public DALNguoiDung dalNguoiDung;
        public BUSNguoiDung()
        {
            dalNguoiDung = new DALNguoiDung();
        }
        public DataTable GetSelect(String MaNguoiDung)
        {
            DataTable dt = new DataTable();
            dt = dalNguoiDung.GetNguoiDung(MaNguoiDung);
            return dt;
        }
    }
}
