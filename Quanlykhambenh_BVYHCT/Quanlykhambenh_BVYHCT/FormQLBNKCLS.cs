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
    public partial class FormQLBNKCLS : Form
    {
        public FormQLBNKCLS()
        {
            InitializeComponent();
        }

        private void lậpBáoCáoThốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void BTThemPhieuCLS_Click(object sender, EventArgs e)
        {
            FormThemPhieuKham formThemPhieuKham = new FormThemPhieuKham();
            formThemPhieuKham.Show();
        }

        private void BTCapNhatKQ_Click(object sender, EventArgs e)
        {
            FormCapNhatKetQua formCapNhatKQ = new FormCapNhatKetQua();
            formCapNhatKQ.Show();
        }

        private void BTXemKQ_Click(object sender, EventArgs e)
        {
            FormXemKetQua formXemKQ = new FormXemKetQua();
            formXemKQ.Show();
        }

        private void quảnLýVậtTưYTếToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQLVatTuYTePKCLS formQLVatTuYTe = new FormQLVatTuYTePKCLS();
            formQLVatTuYTe.Show();
        }
    }
}
