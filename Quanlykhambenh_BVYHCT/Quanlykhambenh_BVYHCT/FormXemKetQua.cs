using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using BUS;
using Entities;

namespace Quanlykhambenh_BVYHCT
{
    public partial class FormXemKetQua : Form
    {
        public BUSPhieuKhamCLS BusPhieuKhamCLS;
        public FormXemKetQua()
        {
            InitializeComponent();
            BusPhieuKhamCLS = new BUSPhieuKhamCLS();
        }

        private void FormXemKetQua_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = BusPhieuKhamCLS.SelectKQKhamBN(ClassVariableStatic.bnCapNhatKQ.MaPhieuKham);
            String ketQuaKham = dt.Rows[0][0].ToString().Trim();
            String PathFile = String.Format(@"..\..\Images\{0}", ketQuaKham);
            Image image = Image.FromFile(PathFile);
            PTViewKQ.Image = image;
        }
    }
}
