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
        public DataTable SearchBenhNhanMaYeuCau(String MaYeuCau)
        {
            DataTable dt = new DataTable();
            dt = DalBNChoKhamCLS.SearchBenhNhanMaYeuCau(MaYeuCau);
            return dt;
        }
        public DataTable SelectBNMaYeuCau(String MaYeuCau)
        {
            DataTable dt = new DataTable();
            dt = DalBNChoKhamCLS.SelectBNMaYeuCau(MaYeuCau);
            return dt;
        }
        public void DeleteBNMaYeuCau(String MaYeuCau)
        {
            DalBNChoKhamCLS.DeleteBNMaYeuCau(MaYeuCau);
        }
    }
}
