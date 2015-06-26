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
    public partial class dangnhap : Form
    {
        public event Fiereform fireloginfull = null;
      
        public dangnhap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cách một dùng biến static
            //if (txtACC.Text == "dung")
            //{
               
            //    MessageBox.Show("Login is OK");
            //  //  formmain.name = txtACC.Text;
            //    this.Close();
            //}
            if (fireloginfull != null)
            {
                fireloginfull(this, new LoginSuccess { usrname = txtACC.Text, Passw = txtPass.Text});
            }
            else
            {
                MessageBox.Show("Login is not ok");

            }

        }
    }
}
