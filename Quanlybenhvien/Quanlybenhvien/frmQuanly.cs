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
    public partial class frmQuanly : Form
    {
        public static int role = -1;
        public frmQuanly()
        {
            InitializeComponent();
            if (role!=1)
            {
                btnThanhtoan.Visible = false;
                ribItemThanhtoan.Visible = false;

               //ribItemThanhtoan.
            }
        }
        public void addtab(string strTabName, UserControl userControlName)
        { 
            
            //-----------If exist tabpage then exit---------------
            foreach (TabItem tabPage in tabConten.Tabs)
              if (tabPage.Text ==strTabName)
               {
                  tabConten.SelectedTab = tabPage;
                  return;
               }
            //-------------------------Clear Tab Before---------------
           

            TabControlPanel newTabPanel = new DevComponents.DotNetBar.TabControlPanel();
            TabItem newTabPage = new TabItem(this.components);
            newTabPage.ImageIndex = 0;
         //   newTabPage.MouseDown += new System.Windows.Forms.MouseEventHandler(tabItem_MouseDown);
            newTabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            newTabPanel.Location = new System.Drawing.Point(0, 26);
            newTabPanel.Name ="panel"+ strTabName;
            newTabPanel.Padding = new System.Windows.Forms.Padding(1);
            newTabPanel.Size = new System.Drawing.Size(1230, 384);
            newTabPanel.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            newTabPanel.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            newTabPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            newTabPanel.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            newTabPanel.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            newTabPanel.Style.GradientAngle = 90;
            newTabPanel.TabIndex = 2;
            newTabPanel.TabItem =newTabPage;
            //-------------New  tab page---------------------
  
            newTabPage.AttachedControl = newTabPanel;
            newTabPage.Text = strTabName;
           userControlName.Dock = DockStyle.Fill;
            newTabPanel.Controls.Add(userControlName);
            //------------add Tab page to Tab control-------------
            tabConten.Controls.Add(newTabPanel);
            tabConten.Tabs.Add(newTabPage);
            tabConten.SelectedTab = newTabPage;
        }

        private void labelX1_Click(object sender, EventArgs e)
        {
            Uc_lapphieutamthu uc = new Uc_lapphieutamthu();
            addtab("dung", uc);
        }
        private void AddTab(string name, UserControl uc)
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
            tabgiothieu2.Name = "pal" + name;
            tabgiothieu2.Text = name;
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

        private void tabConten_TabItemClose(object sender, DevComponents.DotNetBar.TabStripActionEventArgs e)
        {
            tabConten.SelectedTab.Visible=false;
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hello");
        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {
            foreach (TabItem tabPage in tabConten.Tabs)
            {
                if (tabPage.Name == "Itemgiothieu")
                {
                    tabPage.Visible = false;

                }
                if (tabPage.Name == "tabitemDoanhmuctt")
                {
                    tabPage.Visible = true;

                }

            }

           
        }

        private void lbxemttbn_Click(object sender, EventArgs e)
        {
            UcDanhsachbenhnha ucDSBenhnhan = new UcDanhsachbenhnha();
           addtab("Danh sách bệnh nhân", ucDSBenhnhan);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            UcDanhsachbenhnha ucDSBenhnhan = new UcDanhsachbenhnha();
           addtab("Danh sách bệnh nhân", ucDSBenhnhan);
        
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            foreach (TabItem tabPage in tabConten.Tabs)
            {
                if (tabPage.Name == "Itemgiothieu")
                {
                    tabPage.Visible = true;
                    tabConten.SelectedTab = tabPage;
                }
                else
                {
                    tabPage.Visible = false;
                }
            }
        }

        private void labelX2_Click(object sender, EventArgs e)
        {
            Uc_lapphieutamthu ucLptthu = new Uc_lapphieutamthu();
            addtab("Danh sách bệnh nhân", ucLptthu);
        }

        private void labelX3_Click(object sender, EventArgs e)
        {
            Uc_Laphoadonthanhtoan ucLptthanhtoan = new Uc_Laphoadonthanhtoan();
            addtab("Phiếu thanh toán", ucLptthanhtoan);
        }

        private void labelX4_Click(object sender, EventArgs e)
        {
            Uc_Laphoadonthanhtoan ucLptthanhtoan = new Uc_Laphoadonthanhtoan();
            addtab("Phiếu thanh toán", ucLptthanhtoan);
        }
        
    }
}
