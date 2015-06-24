using DevComponents.DotNetBar;
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
    public partial class frmquanly : Form
    {
        public frmquanly()
        {
            InitializeComponent();
        }

        private void Demmo_Load(object sender, EventArgs e)
        {

        }

        private void ribbonBar1_ItemClick(object sender, EventArgs e)
        {

        }

        private void ribbonBar2_ItemClick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_TabItemClose(object sender, DevComponents.DotNetBar.TabStripActionEventArgs e)
        {
            tabConten.Tabs.Remove(tabConten.SelectedTab);
         //   tabConten.Tabs.Remove(tabConten.SelectedTab);
           
           
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("xin chào bạn");
        }

        private void labelX1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
           // tabConten.Tabs.Add(tapdanhsach);
                
           tapdanhsach.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (TabItem tabPage in tabConten.Tabs)
                if (tabPage.Text == "THEMSP")
                {
                    tabConten.SelectedTab = tabPage;
                    return;
                }

             TabControlPanel tabControlAdd;
              tabControlAdd = new DevComponents.DotNetBar.TabControlPanel();
              tabControlAdd.SuspendLayout();
               TabItem tabgiothieu2=new TabItem();
               tabgiothieu2.AttachedControl = tabControlAdd;
            tabgiothieu2.CloseButtonVisible = false;
            tabgiothieu2.Name = "tabadd";
            tabgiothieu2.Text = "THEMSP";
            tabControlAdd.Controls.Add(button1);
            tabControlAdd.Controls.Add(label1);
            tabControlAdd.Controls.Add(pictureBox1);
            tabControlAdd.DisabledBackColor = System.Drawing.Color.Empty;
            tabControlAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlAdd.Location = new System.Drawing.Point(0, 26);
            tabControlAdd.Name = "tabControlPanel1";
            tabControlAdd.Padding = new System.Windows.Forms.Padding(1);
            tabControlAdd.Size = new System.Drawing.Size(517, 156);
            tabControlAdd.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            tabControlAdd.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            tabControlAdd.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            tabControlAdd.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            tabControlAdd.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            tabControlAdd.Style.GradientAngle = 90;
            tabControlAdd.TabIndex = 1;
            tabControlAdd.TabItem = tabgiothieu2;
            tabConten.Controls.Add(tabControlAdd);
            tabConten.Tabs.Add(tabgiothieu2);
            tabConten.SelectedTab = tabgiothieu2;
        }
        private void AddTab(string name, UserControl  uc)
        {
            foreach (TabItem tabPage in tabConten.Tabs)
                if (tabPage.Text == name)
                {
                    tabConten.SelectedTab = tabPage;
                    return;
                }
               
            TabControlPanel tabControlAdd;
            tabControlAdd = new DevComponents.DotNetBar.TabControlPanel();
            tabControlAdd.SuspendLayout();
            TabItem tabgiothieu2 = new TabItem();
            tabgiothieu2.AttachedControl = tabControlAdd;
            tabgiothieu2.CloseButtonVisible = false;
            tabgiothieu2.Name = "pal"+name;
            tabgiothieu2.Text = name;
            tabControlAdd.Controls.Add(button1);
            tabControlAdd.Controls.Add(label1);
            tabControlAdd.Controls.Add(pictureBox1);
         
            tabControlAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlAdd.Location = new System.Drawing.Point(0, 26);
            tabControlAdd.Name = "tabControlPanel1";
            tabControlAdd.Padding = new System.Windows.Forms.Padding(1);
            tabControlAdd.Size = new System.Drawing.Size(517, 156);
            tabControlAdd.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            tabControlAdd.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            tabControlAdd.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            tabControlAdd.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            tabControlAdd.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            tabControlAdd.Style.GradientAngle = 90;
            tabControlAdd.TabIndex = 1;
            tabControlAdd.TabItem = tabgiothieu2;
            uc.Dock = DockStyle.Fill;
            tabConten.Controls.Add(uc);
            tabConten.Tabs.Add(tabgiothieu2);
            tabConten.SelectedTab = tabgiothieu2;
        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {
           
            Uc_gioithieu uc = new Uc_gioithieu();
            AddTab("Niềm tin", uc);

        }
    }
}
