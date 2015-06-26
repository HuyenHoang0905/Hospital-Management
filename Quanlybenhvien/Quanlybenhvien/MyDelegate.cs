using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlybenhvien
{
    public delegate void Fiereform(object sender, LoginSuccess e);
    //class MyDelegate
    //{
   
    //}
    public class LoginSuccess : EventArgs
    {
        public string usrname { get; set; }
        public string Passw{ get; set; }
    }
    public class RoleLogin : EventArgs
    {
        public int Role;
    }
}
