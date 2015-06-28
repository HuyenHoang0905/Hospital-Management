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
    public class BUSBenhNhanChoKhamCLS
    {
        public DALBenhNhanChoKhamCLS DalBNChoKhamCLS;
        public BUSBenhNhanChoKhamCLS()
        {
            DalBNChoKhamCLS = new DALBenhNhanChoKhamCLS();
        }
        public DataTable GetBenhNhan()
        {
            DataTable dt = new DataTable();
            dt = DalBNChoKhamCLS.GetBenhNhan();
            return dt;
        }
        public DataTable GetBenhNhanID(String mabn)
        {
            DataTable dt = new DataTable();
            dt = DalBNChoKhamCLS.GetBenhNhanID(mabn);
            return dt;
        }
        public DataTable SearchBenhNhanID(String mabn)
        {
            DataTable dt = new DataTable();
            dt = DalBNChoKhamCLS.SearchBenhNhanID(mabn);
            return dt;
        }
    }
}
