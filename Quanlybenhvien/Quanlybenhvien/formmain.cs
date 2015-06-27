using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlybenhvien
{
    public partial class formmain : Form
    {
       // public static string name = string.Empty;
        public formmain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dangnhap dn = new dangnhap();
            dn.fireloginfull += new Fiereform(login_fire);
            dn.ShowDialog();
            //if (!string.IsNullOrEmpty(name))
            //{
            //    this.lbUser.Text = name;
            //}

        }
        void login_fire(object sender, LoginSuccess e)
        {
            lbUser.Text = e.usrname;
        }
    }
}
