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
    public class ChoKhamCLS_Bus
    {

        public ChoKhamCLS_Entities a;
        private DataConnect connect;
        private SqlCommand cmd;
        private SqlDataAdapter dataAP;
        private DataTable dataTable;

        public ChoKhamCLS_Bus()
        {
            a = new ChoKhamCLS_Entities();
            connect = new DataConnect();
        }

        public DataTable ThongTin()
        {
            string sql = "select *from TblChoKhamCLS order by MaBenhNhan";
            return connect.getData(sql);
        }
        public Boolean them_CLS( ChoKhamCLS_Entities a)
        {
            connect.openConnect();
            cmd = new SqlCommand("Them_ChoKhamCLS", connect.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaBenhNhan ", a.MaBenhNhan);
            cmd.Parameters.AddWithValue("@MaLoaiKhamCLS", a.MaLoaiKhamCLS);
            cmd.Parameters.AddWithValue("@MaBSYeuCau", a.MaBSYeuCau);
            cmd.Parameters.AddWithValue("@TinhTrangThanhToan", a.TinhTrangThanhToan);
           try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
            finally
            {
                connect.closeConnect();
            }
        }

    }
}
