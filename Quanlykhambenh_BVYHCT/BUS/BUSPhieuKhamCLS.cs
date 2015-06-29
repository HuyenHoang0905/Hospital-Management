using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entities;
using DAL;

namespace BUS
{
    public class BUSPhieuKhamCLS
    {
        public DALPhieuKhamCLS dalPhieuKhamCLS;
        public BUSPhieuKhamCLS()
        {
            dalPhieuKhamCLS = new DALPhieuKhamCLS();
        }
        public Boolean InsertPhieuKhamCLS(PhieuKhamCLS pk)
        {
            return dalPhieuKhamCLS.InsertPhieuKhamCLS(pk);
        }
        public Boolean UpdateKQPhieuKhamCLS(String KetQua, String MaPhieuKham)
        {
            return dalPhieuKhamCLS.UpdateKQPhieuKhamCLS(KetQua, MaPhieuKham);
        }
        public DataTable SelectBNUpdateKQ()
        {
            return dalPhieuKhamCLS.SelectBNCapNhatKQ();
        }
        public DataTable SelectKQKhamBN(String maPhieuKham)
        {
            return dalPhieuKhamCLS.SelectKQKhamBN(maPhieuKham);
        }
        public DataTable SelectBNMaBN(String maPhieuKham)
        {
            return dalPhieuKhamCLS.SelectBNMaBN(maPhieuKham);
        }
        public DataTable SelectBNChuaChuyenKQ()
        {
            return dalPhieuKhamCLS.SelectBNChuaChuyenKQ();
        }
    }
}
