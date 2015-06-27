using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DAL;

namespace BUS
{
 public   class Users_BUS
    {
        public static int CheckLogin_BUS(string acountName, string passWord, string strConnect)
        {
            int role;
            var daUser = User_ADL.checkLogin(acountName, passWord, strConnect);
            if (daUser.Rows.Count > 0)
            {
                role = int.Parse(daUser.Rows[0]["Role"].ToString());
                return role;
            }
            else return -1;
        }
    }
}
