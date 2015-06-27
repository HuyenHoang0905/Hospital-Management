using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ColorSchemeEditor.
	/// </summary>
	public class ColorSchemeEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ColorScheme m_ColorScheme=null;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private MenuPanel m_Menu=null;
		private Bar m_SimpleBar=null;
		private Bar m_FloatBar=null;
		private System.Windows.Forms.Button button3;
		private Bar m_PopupBar=null;
		private System.Windows.Forms.Button btnSaveScheme;
		private System.Windows.Forms.Button btnLoadScheme;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		public bool ColorSchemeChanged=false;

		public ColorSchemeEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.StartPosition=FormStartPosition.CenterScreen;
		}

		private void AddDotNetBarControls()
		{
			if(m_Menu!=null)
			{
				m_Menu.RecalcSize();
				m_SimpleBar.Size=new Size(groupBox1.ClientRectangle.Width-12,16);
				m_SimpleBar.RecalcSize();
				m_FloatBar.Size=new Size(groupBox1.ClientRectangle.Width,16);
				m_FloatBar.RecalcSize();
				m_PopupBar.Size=new Size(groupBox1.ClientRectangle.Width,24);
				m_PopupBar.RecalcSize();
				return;
			}

			m_Menu=new MenuPanel();
			m_Menu.PopupMenu=false;
			m_Menu.Location=new Point(8,135);

			// Create Simple Toolbar
			m_SimpleBar=new Bar();
			m_SimpleBar.PassiveBar=true;
			m_SimpleBar.Location=new Point(4,16);
			m_SimpleBar.ThemeAware=false;

			m_FloatBar=new Bar();
			m_FloatBar.PassiveBar=true;
			m_FloatBar.Location=new Point(4,46);
			m_FloatBar.ThemeAware=false;

			m_PopupBar=new Bar();
			m_PopupBar.PassiveBar=true;
			m_PopupBar.Location=new Point(4,96);
			m_PopupBar.ThemeAware=false;

			ButtonItem menuItem=new ButtonItem();
			ButtonItem item, item2;
			Bitmap bmp=null;

			item=new ButtonItem("new","&New...");
			bmp=BarFunctions.LoadBitmap("BarEditorImages.FileNew.bmp");
			bmp.MakeTransparent(Color.Magenta);
			item.Image=bmp;
			m_SimpleBar.Items.Add(item.Copy());
			m_FloatBar.Items.Add(item.Copy());
			m_PopupBar.Items.Add(item.Copy());
			menuItem.SubItems.Add(item);
			item=new ButtonItem("open","&Open");
			bmp=BarFunctions.LoadBitmap("BarEditorImages.FileOpen.bmp");
			bmp.MakeTransparent(Color.Magenta);
			item.Image=bmp;
			item2=(ButtonItem)item.Copy();
			item2.ButtonStyle=eButtonStyle.ImageAndText;
			m_SimpleBar.Items.Add(item2);
			m_FloatBar.Items.Add(item2.Copy());
			m_PopupBar.Items.Add(item2.Copy());
			menuItem.SubItems.Add(item);
			item=new ButtonItem("close","&Close");
			bmp=BarFunctions.LoadBitmap("BarEditorImages.FileClose.bmp");
			bmp.MakeTransparent(Color.Magenta);
			item.Image=bmp;
			item2=(ButtonItem)item.Copy();
			item2.Checked=true;
			item2.ButtonStyle=eButtonStyle.ImageAndText;
			m_SimpleBar.Items.Add(item2);
			m_PopupBar.Items.Add(item2.Copy());
			item2=(ButtonItem)item2.Copy();
			item2.Enabled=false;
			m_FloatBar.Items.Add(item2.Copy());
			menuItem.SubItems.Add(item);
			item=new ButtonItem("open","Add Ne&w Item...");
			menuItem.SubItems.Add(item);
			item=new ButtonItem("open","Add Existin&g Item...");
			menuItem.SubItems.Add(item);
			item=new ButtonItem("opensol","Open Solution...");
			item.BeginGroup=true;
			bmp=BarFunctions.LoadBitmap("BarEditorImages.FileOpenSol.bmp");
			bmp.MakeTransparent(Color.Magenta);
			item.Image=bmp;
			item2=(ButtonItem)item.Copy();
			item2.Enabled=false;
			m_SimpleBar.Items.Add(item2);
			m_FloatBar.Items.Add(item2.Copy());
			m_PopupBar.Items.Add(item2.Copy());
			menuItem.SubItems.Add(item);
			item=new ButtonItem("open","Close Solution");
			bmp=BarFunctions.LoadBitmap("BarEditorImages.FileCloseSol.bmp");
			bmp.MakeTransparent(Color.Magenta);
			item.Image=bmp;
			item.Enabled=false;
			menuItem.SubItems.Add(item);

			m_Menu.ParentItem=menuItem;

			groupBox1.Controls.Add(m_Menu);
			m_Menu.RecalcSize();
			m_Menu.Show();
            
			//m_SimpleBar.SetBarState(eBarState.Docked);
			m_SimpleBar.Size=new Size(groupBox1.ClientRectangle.Width,16);
			m_SimpleBar.GrabHandleStyle=eGrabHandleStyle.StripeFlat;
			groupBox1.Controls.Add(m_SimpleBar);
			m_SimpleBar.RecalcSize();
			m_SimpleBar.Show();

			m_FloatBar.SetBarState(eBarState.Floating);
			m_FloatBar.Size=new Size(groupBox1.ClientRectangle.Width,16);
			groupBox1.Controls.Add(m_FloatBar);
			m_FloatBar.Text="Bar Caption";
			m_FloatBar.RecalcSize();
			m_FloatBar.Show();

			m_PopupBar.SetBarState(eBarState.Popup);
			m_PopupBar.Size=new Size(groupBox1.ClientRectangle.Width,24);
			m_PopupBar.PopupWidth=groupBox1.ClientRectangle.Width;
			groupBox1.Controls.Add(m_PopupBar);
			m_PopupBar.RecalcSize();
			m_PopupBar.Show();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.btnSaveScheme = new System.Windows.Forms.Button();
			this.btnLoadScheme = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(208, 4);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(352, 299);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(10, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(190, 303);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Color Scheme Preview:";
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(404, 307);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(76, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(316, 307);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(76, 24);
			this.button2.TabIndex = 3;
			this.button2.Text = "&Reset";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button3.Location = new System.Drawing.Point(484, 307);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(76, 24);
			this.button3.TabIndex = 4;
			this.button3.Text = "Cancel";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// btnSaveScheme
			// 
			this.btnSaveScheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSaveScheme.Location = new System.Drawing.Point(10, 308);
			this.btnSaveScheme.Name = "btnSaveScheme";
			this.btnSaveScheme.Size = new System.Drawing.Size(92, 24);
			this.btnSaveScheme.TabIndex = 5;
			this.btnSaveScheme.Text = "Save Scheme...";
			this.btnSaveScheme.Click += new System.EventHandler(this.btnSaveScheme_Click);
			// 
			// btnLoadScheme
			// 
			this.btnLoadScheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnLoadScheme.Location = new System.Drawing.Point(107, 308);
			this.btnLoadScheme.Name = "btnLoadScheme";
			this.btnLoadScheme.Size = new System.Drawing.Size(92, 24);
			this.btnLoadScheme.TabIndex = 6;
			this.btnLoadScheme.Text = "Load Scheme...";
			this.btnLoadScheme.Click += new System.EventHandler(this.btnLoadScheme_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "*.dcs";
			this.openFileDialog1.Filter = "DotNetBar Color Scheme files|*.dcs|All files|*.*";
			this.openFileDialog1.Title = "Open DotNetBar Color Scheme File";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "dcs";
			this.saveFileDialog1.FileName = "colorscheme1";
			this.saveFileDialog1.Filter = "DotNetBar Color Scheme files|*.dcs|All files|*.*";
			this.saveFileDialog1.Title = "Save DotNetBar Color Scheme File";
			// 
			// ColorSchemeEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(567, 338);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnLoadScheme,
																		  this.btnSaveScheme,
																		  this.button3,
																		  this.button2,
																		  this.button1,
																		  this.groupBox1,
																		  this.propertyGrid1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorSchemeEditor";
			this.Text = "ColorScheme Editor";
			this.Load += new System.EventHandler(this.ColorSchemeEditorLoad);
			this.ResumeLayout(false);

		}
		#endregion

		private void ColorSchemeEditorLoad(object sender, EventArgs e)
		{
			AddDotNetBarControls();
		}

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			m_Menu.Refresh();
			m_SimpleBar.Refresh();
			m_FloatBar.Refresh();
			m_PopupBar.Refresh();
			ColorSchemeChanged=true;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show("Reseting the Color Scheme will destroy any changes you made. Are you sure you want to reset the Color Scheme?","Color Scheme Editor",MessageBoxButtons.YesNo)==DialogResult.Yes)
			{
				this.ColorScheme=new ColorScheme(this.ColorScheme.Style);
				ColorSchemeChanged=true;
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			ColorSchemeChanged=false;
			this.Hide();
		}

		private void btnSaveScheme_Click(object sender, System.EventArgs e)
		{
			if(saveFileDialog1.ShowDialog()==DialogResult.OK)
			{
				XmlDocument doc=new XmlDocument();
				XmlElement elem1=doc.CreateElement("colorschemes");
				doc.AppendChild(elem1);
				XmlElement elem2=doc.CreateElement("colorscheme");
				elem1.AppendChild(elem2);
				m_ColorScheme.Serialize(elem2);
				doc.Save(saveFileDialog1.FileName);
				MessageBox.Show("Color Scheme has been saved.","DotNetBar Color Scheme Editor",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}

		private void btnLoadScheme_Click(object sender, System.EventArgs e)
		{
			if(openFileDialog1.ShowDialog()==DialogResult.OK && System.IO.File.Exists(openFileDialog1.FileName))
			{
				DevComponents.DotNetBar.ColorScheme scheme=new ColorScheme();
				XmlDocument doc=new XmlDocument();
				doc.Load(openFileDialog1.FileName);
				scheme.Deserialize(doc.FirstChild.FirstChild as XmlElement);
				this.ColorScheme=scheme;
				ColorSchemeChanged=true;
			}
		}

		public DevComponents.DotNetBar.ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				if(m_Menu==null)
					AddDotNetBarControls();
				m_ColorScheme=value;
				propertyGrid1.SelectedObject=m_ColorScheme;
				m_Menu.ColorScheme=m_ColorScheme;
				m_SimpleBar.ColorScheme=m_ColorScheme;
				m_FloatBar.ColorScheme=m_ColorScheme;
				m_PopupBar.ColorScheme=m_ColorScheme;
				ColorSchemeChanged=false;
			}
		}
	}
}
