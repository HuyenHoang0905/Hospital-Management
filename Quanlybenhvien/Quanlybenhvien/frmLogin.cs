using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;

namespace Quanlybenhvien
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            SQLHelper sqlconnec = new SQLHelper();
            string strConnec = sqlconnec.strConn;
            string acount = txtacount.Text;
            string pass = txtpass.Text;
            int role = Users_BUS.CheckLogin_BUS(acount, pass, strConnec);
            if (role != -1)
            {
                frmQuanly.role = role;

                frmQuanly fm = new frmQuanly();
              
                fm.Show();
            }
            else 
            {
                lblThongbao.Visible = true;
                MessageBox.Show("Bạn nhập sai thông tin");
            }

        }

        private void txtacount_MouseClick(object sender, MouseEventArgs e)
        {
            lblThongbao.Visible = false;
        }

        private void txtpass_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }
    }
}
