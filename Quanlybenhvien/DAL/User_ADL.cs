using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Data;

namespace DAL
{
  public  class User_ADL
    {
      public static DataTable checkLogin(string acountName, string passWord, string _strConn)
        {
            var conn = new ConnectDB(_strConn);
            try //nvarchar(10),

            {
                SqlParameter[] p = new SqlParameter[2];
               p[0] = new SqlParameter("@Acount",acountName);
               p[1] = new SqlParameter("@PassWord",passWord);
               return conn.ExecuteReaderParmarter("sp_CheckLogin", p);
            }
            catch
            {

                throw;
            }
            finally
            {

            }

        }
    }
}
