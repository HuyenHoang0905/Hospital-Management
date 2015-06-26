using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlybenhvien
{
    class SQLHelper
    {
        public string strConn { get; set; }
        //get connection string get from file web.config
        public SQLHelper()
        {
            strConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
    }
}