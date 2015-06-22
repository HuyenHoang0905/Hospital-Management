using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlykhambenh_BVYHCT
{
    public partial class FormCapNhatKetQua : Form
    {
        public FormCapNhatKetQua()
        {
            InitializeComponent();
        }

        private void FormCapNhatKetQua_Load(object sender, EventArgs e)
        {

        }

        private void BTChonFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileKQ.ShowDialog();
            if (dr == DialogResult.OK)
            {
                String stringKQ = openFileKQ.FileName.ToString();
                String OnlyNameFile = openFileKQ.SafeFileName;
                String[] temp = new String[2];
                temp = stringKQ.Split('.');
                if (temp[1] == "jpg" || temp[1] == "JPG" || temp[1] == "png" || temp[1] == "PNG")
                {
                    txtCapNhatKQ.Text = stringKQ;
                    String PathFile = String.Format(@"..\..\Images\{0}", OnlyNameFile);
                    Image image = Image.FromFile(PathFile);
                    PictureBoxKQ.Image = image;
                }
            }
        }
    }
}
