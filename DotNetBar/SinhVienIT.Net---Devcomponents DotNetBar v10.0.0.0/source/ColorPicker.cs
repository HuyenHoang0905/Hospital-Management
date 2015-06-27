using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.UI
{
	/// <summary>
	/// Provides popup color picker.
	/// </summary>
	[ToolboxItem(false)]
	internal class ColorPicker : UserControl
	{
		#region Private Variables
		private Color[] m_CustomColors=new Color[48];
		private Rectangle[] m_CustomColorsPos=new Rectangle[48];
		private object m_ColorScheme=null;
		private string m_TransText="";

		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private TabPage tabPage3;
		private TabPage tabPage4;
		private Label label1;
		private ListBox listScheme;
		private Button btnOK;
		private Button btnCancel;
		private Panel colorPanel;
		private ListBox listSystem;
		private ListBox listWeb;

		private Color m_SelectedColor=Color.Empty;
		private string m_SelectedColorSchemeName="";
		private Panel colorPreview;
		private TrackBar transparencyTrack;

		private bool m_Canceled=false;

		private IWindowsFormsEditorService m_EditorService=null;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		#endregion

		#region Constructor and Dispose
		public ColorPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			InitCustomColors();
			InitOtherColors();
			m_TransText=label1.Text;
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
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.listScheme = new System.Windows.Forms.ListBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.listSystem = new System.Windows.Forms.ListBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.listWeb = new System.Windows.Forms.ListBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.colorPanel = new System.Windows.Forms.Panel();
			this.transparencyTrack = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.colorPreview = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.transparencyTrack)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Location = new System.Drawing.Point(1, 1);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(208, 192);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.listScheme);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(200, 166);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Scheme";
			// 
			// listScheme
			// 
			this.listScheme.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listScheme.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listScheme.IntegralHeight = false;
			this.listScheme.Location = new System.Drawing.Point(0, 0);
			this.listScheme.Name = "listScheme";
			this.listScheme.Size = new System.Drawing.Size(200, 166);
			this.listScheme.TabIndex = 0;
			this.listScheme.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawColorItem);
			this.listScheme.SelectedIndexChanged += new System.EventHandler(this.ListSelectionChange);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.listSystem);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(200, 166);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "System";
			// 
			// listSystem
			// 
			this.listSystem.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listSystem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listSystem.IntegralHeight = false;
			this.listSystem.Location = new System.Drawing.Point(0, 0);
			this.listSystem.Name = "listSystem";
			this.listSystem.Size = new System.Drawing.Size(200, 166);
			this.listSystem.TabIndex = 1;
			this.listSystem.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawColorItem);
			this.listSystem.SelectedIndexChanged += new System.EventHandler(this.ListSelectionChange);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.listWeb);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(200, 166);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Web";
			// 
			// listWeb
			// 
			this.listWeb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listWeb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listWeb.IntegralHeight = false;
			this.listWeb.Location = new System.Drawing.Point(0, 0);
			this.listWeb.Name = "listWeb";
			this.listWeb.Size = new System.Drawing.Size(200, 166);
			this.listWeb.TabIndex = 1;
			this.listWeb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawColorItem);
			this.listWeb.SelectedIndexChanged += new System.EventHandler(this.ListSelectionChange);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.colorPanel);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(200, 166);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Custom";
			// 
			// colorPanel
			// 
			this.colorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorPanel.Location = new System.Drawing.Point(0, 0);
			this.colorPanel.Name = "colorPanel";
			this.colorPanel.Size = new System.Drawing.Size(200, 166);
			this.colorPanel.TabIndex = 0;
			this.colorPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CustomColorMouseUp);
			this.colorPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintCustomColors);
			// 
			// transparencyTrack
			// 
			this.transparencyTrack.Enabled = false;
			this.transparencyTrack.Location = new System.Drawing.Point(1, 204);
			this.transparencyTrack.Maximum = 255;
			this.transparencyTrack.Name = "transparencyTrack";
			this.transparencyTrack.Size = new System.Drawing.Size(200, 45);
			this.transparencyTrack.TabIndex = 1;
			this.transparencyTrack.TickFrequency = 16;
			this.transparencyTrack.Value = 255;
			this.transparencyTrack.ValueChanged += new System.EventHandler(this.transparencyTrack_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(1, 194);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Transparency";
			// 
			// colorPreview
			// 
			this.colorPreview.BackColor = System.Drawing.Color.White;
			this.colorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.colorPreview.Location = new System.Drawing.Point(8, 240);
			this.colorPreview.Name = "colorPreview";
			this.colorPreview.Size = new System.Drawing.Size(40, 32);
			this.colorPreview.TabIndex = 3;
			this.colorPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.colorPreview_Paint);
			// 
			// btnOK
			// 
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(72, 248);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(142, 248);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// ColorPicker
			// 
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.colorPreview);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.transparencyTrack);
			this.DockPadding.All = 1;
			this.Name = "ColorPicker";
			this.Size = new System.Drawing.Size(211, 280);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorPicker_Paint);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.transparencyTrack)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Color Init
		private void InitCustomColors()
		{
			m_CustomColors[0]=Color.FromArgb(255,255,255);
			m_CustomColors[1]=Color.FromArgb(255,195,198);
			m_CustomColors[2]=Color.FromArgb(255,227,198);
			m_CustomColors[3]=Color.FromArgb(255,255,198);
			m_CustomColors[4]=Color.FromArgb(198,255,198);
			m_CustomColors[5]=Color.FromArgb(198,255,255);
			m_CustomColors[6]=Color.FromArgb(198,195,255);
			m_CustomColors[7]=Color.FromArgb(255,195,255);

			m_CustomColors[8]=Color.FromArgb(231,227,231);
			m_CustomColors[9]=Color.FromArgb(255,130,132);
			m_CustomColors[10]=Color.FromArgb(255,195,132);
			m_CustomColors[11]=Color.FromArgb(255,255,132);
			m_CustomColors[12]=Color.FromArgb(132,255,132);
			m_CustomColors[13]=Color.FromArgb(132,255,255);
			m_CustomColors[14]=Color.FromArgb(132,130,255);
			m_CustomColors[15]=Color.FromArgb(255,130,255);

			m_CustomColors[16]=Color.FromArgb(198,195,198);
			m_CustomColors[17]=Color.FromArgb(255,0,0);
			m_CustomColors[18]=Color.FromArgb(255,130,0);
			m_CustomColors[19]=Color.FromArgb(255,255,0);
			m_CustomColors[20]=Color.FromArgb(0,255,0);
			m_CustomColors[21]=Color.FromArgb(0,255,255);
			m_CustomColors[22]=Color.FromArgb(0,0,255);
			m_CustomColors[23]=Color.FromArgb(255,0,255);

			m_CustomColors[24]=Color.FromArgb(132,130,132);
			m_CustomColors[25]=Color.FromArgb(198,0,0);
			m_CustomColors[26]=Color.FromArgb(198,65,0);
			m_CustomColors[27]=Color.FromArgb(198,195,0);
			m_CustomColors[28]=Color.FromArgb(0,195,0);
			m_CustomColors[29]=Color.FromArgb(0,195,198);
			m_CustomColors[30]=Color.FromArgb(0,0,198);
			m_CustomColors[31]=Color.FromArgb(198,0,198);

			m_CustomColors[32]=Color.FromArgb(66,65,66);
			m_CustomColors[33]=Color.FromArgb(132,0,0);
			m_CustomColors[34]=Color.FromArgb(132,65,0);
			m_CustomColors[35]=Color.FromArgb(132,130,0);
			m_CustomColors[36]=Color.FromArgb(0,130,0);
			m_CustomColors[37]=Color.FromArgb(0,130,132);
			m_CustomColors[38]=Color.FromArgb(0,0,132);
			m_CustomColors[39]=Color.FromArgb(132,0,132);

			m_CustomColors[40]=Color.FromArgb(0,0,0);
			m_CustomColors[41]=Color.FromArgb(66,0,0);
			m_CustomColors[42]=Color.FromArgb(132,65,66);
			m_CustomColors[43]=Color.FromArgb(66,65,0);
			m_CustomColors[44]=Color.FromArgb(0,65,0);
			m_CustomColors[45]=Color.FromArgb(0,65,66);
			m_CustomColors[46]=Color.FromArgb(0,0,66);
			m_CustomColors[47]=Color.FromArgb(66,0,66);
		}

		private void InitOtherColors()
		{
			listWeb.BeginUpdate();
			listWeb.Items.Clear();
			Type type = typeof(Color);
			PropertyInfo[] fields=type.GetProperties(BindingFlags.Public | BindingFlags.Static);
			Color clr=new Color();
			foreach(PropertyInfo pi in fields)
				listWeb.Items.Add(pi.GetValue(clr,null));
			listWeb.EndUpdate();

			listSystem.BeginUpdate();
			listSystem.Items.Clear();
			type=typeof(SystemColors);
			fields=type.GetProperties(BindingFlags.Public | BindingFlags.Static);
			foreach(PropertyInfo pi in fields)
				listSystem.Items.Add(pi.GetValue(clr,null));
			listSystem.EndUpdate();
		}

		private void InitColorSchemeColors()
		{
			if(m_ColorScheme!=null)
			{
				if(!tabControl1.TabPages.Contains(tabPage1))
				{
					tabControl1.TabPages.Add(tabPage1);
				}
				listScheme.BeginUpdate();
				listScheme.Items.Clear();
				Type type=m_ColorScheme.GetType();
				PropertyInfo[] fields=type.GetProperties();
				foreach(PropertyInfo pi in fields)
				{
					if(pi.PropertyType==typeof(Color))
						listScheme.Items.Add(pi.Name);
				}
				listScheme.EndUpdate();
			}
			else
			{
				if(tabControl1.TabPages.Contains(tabPage1))
					tabControl1.TabPages.Remove(tabPage1);
			}
		}
		#endregion

		#region Public Interface

		/// <summary>
		/// Gets or sets the reference to the IWindowsFormsEditorService interface used for Windows Forms design time support.
		/// </summary>
		internal IWindowsFormsEditorService EditorService
		{
			get {return m_EditorService;}
			set {m_EditorService=value;}
		}

		/// <summary>
		/// Gets or sets the ColorScheme object for Scheme colors.
		/// </summary>
		[Browsable(false),DefaultValue(null)]
		public object ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				m_ColorScheme=value;
				InitColorSchemeColors();
			}
		}

		/// <summary>
		/// Gets or sets currently selected color.
		/// </summary>
		public Color SelectedColor
		{
			get {return m_SelectedColor;}
			set
			{
				m_SelectedColor=value;
				OnSelectedColorChanged();
			}
		}

		/// <summary>
		/// Gets the selected color color scheme name if color scheme color is selected otherwise it returns an empty string.
		/// </summary>
		public string SelectedColorSchemeName
		{
			get {return m_SelectedColorSchemeName;}
			set {m_SelectedColorSchemeName=value;}
		}

		/// <summary>
		/// Returns true if color selection was cancelled.
		/// </summary>
		public bool Canceled
		{
			get {return m_Canceled;}
		}
		#endregion

		#region Painting
		private void PaintCustomColors(object sender, PaintEventArgs e)
		{
			Rectangle r=Rectangle.Empty;
			int x=6, y=12;
			Graphics g=e.Graphics;
			Border3DSide side=(Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom);
			int width=colorPanel.ClientRectangle.Width;
			int iIndex=0;
			foreach(Color clr in m_CustomColors)
			{
				r=new Rectangle(x,y,21,21);
				if(r.Right>width)
				{
					y+=25;
					x=6;
					r.X=x;
					r.Y=y;
				}
				ControlPaint.DrawBorder3D(g,x,y,21,21,Border3DStyle.Sunken,side);
				r.Inflate(-2,-2);
				g.FillRectangle(new SolidBrush(clr),r);

				m_CustomColorsPos[iIndex]=r;
				iIndex++;

				x+=24;
			}
		}

		private void DrawColorItem(object sender, DrawItemEventArgs e)
		{
			Rectangle r=e.Bounds;
			Rectangle rClr=new Rectangle(r.X+1,r.Y+2,24,r.Height-4);
			ListBox list=sender as ListBox;
			
			Color textColor=SystemColors.ControlText;
			if((e.State & DrawItemState.Selected)!=0)
			{
				textColor=SystemColors.HighlightText;
				e.Graphics.FillRectangle(SystemBrushes.Highlight,e.Bounds);
			}
			else
				e.Graphics.FillRectangle(SystemBrushes.Window,e.Bounds);

			Color clr=Color.Empty;
			string colorName="";
			if(list.Items[e.Index].GetType()==typeof(Color))
			{
				clr=(Color)list.Items[e.Index];
				colorName=clr.Name;
			}
			else
			{
				colorName=list.Items[e.Index].ToString();
				clr=(Color)m_ColorScheme.GetType().GetProperty(colorName).GetValue(m_ColorScheme,null);
			}

			e.Graphics.FillRectangle(new SolidBrush(clr),rClr);
			e.Graphics.DrawRectangle(SystemPens.ControlText,rClr);
			r.Offset(30,0);
			r.Width-=30;
			DevComponents.DotNetBar.TextDrawing.DrawString(e.Graphics,colorName,list.Font,textColor,r,DevComponents.DotNetBar.eTextFormat.Default);
		}
		#endregion

		#region Internals

		private void CustomColorMouseUp(object sender, MouseEventArgs e)
		{
			for(int i=0;i<48;i++)
			{
				if(m_CustomColorsPos[i].Contains(e.X,e.Y))
				{
					this.SelectedColor=m_CustomColors[i];
					m_SelectedColorSchemeName="";
					break;					
				}
			}
		}

		private void ListSelectionChange(object sender, EventArgs e)
		{
			ListBox list=sender as ListBox;

			if(list.SelectedItem!=null)
			{
				if(list.SelectedItem is Color)
				{
					this.SelectedColor=(Color)list.SelectedItem;
					m_SelectedColorSchemeName="";
				}
				else
				{
                    m_SelectedColorSchemeName = list.SelectedItem.ToString();
					this.SelectedColor=(Color)m_ColorScheme.GetType().GetProperty(this.SelectedColorSchemeName).GetValue(m_ColorScheme,null);
				}
			}
			else
			{
				this.SelectedColor=Color.Empty;
                m_SelectedColorSchemeName = "";
			}
		}

		private void OnSelectedColorChanged()
		{
			colorPreview.BackColor=m_SelectedColor;
			transparencyTrack.Value=m_SelectedColor.A;
			UpdateTransparencyText();
		}

        public void UpdateUIWithSelection()
        {
            listSystem.SelectedIndex = -1;
            listWeb.SelectedIndex = -1;
            listScheme.SelectedIndex = -1;
            if (m_SelectedColor.IsSystemColor)
            {
                tabControl1.SelectedTab = tabPage2;
                SelectListBoxItem(listSystem, m_SelectedColor.Name);
            }
            else if (m_SelectedColor.IsNamedColor)
            {
                tabControl1.SelectedTab = tabPage3;
                SelectListBoxItem(listWeb, m_SelectedColor.Name);
            }
            else if (m_SelectedColorSchemeName != "")
            {
                tabControl1.SelectedTab = tabPage1;
                SelectListBoxItem(listScheme, m_SelectedColorSchemeName);
            }
            else
            {
                tabControl1.SelectedTab = tabPage4;
            }
        }

        private void SelectListBoxItem(ListBox listBox, string item)
        {
            foreach (object o in listBox.Items)
            {
                if (o.ToString() == item)
                {
                    listBox.SelectedItem = o;
                    return;
                }
            }
        }

		private void UpdateTransparencyText()
		{
			if(!this.SelectedColor.IsEmpty)
			{
				label1.Text=m_TransText + " (" + this.SelectedColor.A.ToString()+")";
			}
			else
				label1.Text=m_TransText;
		}

		private void transparencyTrack_ValueChanged(object sender, EventArgs e)
		{
			if(!this.SelectedColor.IsEmpty && this.SelectedColor.A!=transparencyTrack.Value)
			{
				this.SelectedColor=Color.FromArgb(transparencyTrack.Value,this.SelectedColor);
				m_SelectedColorSchemeName="";
			}
			UpdateTransparencyText();
		}

		private void colorPreview_Paint(object sender, PaintEventArgs e)
		{
			if(this.SelectedColor.IsEmpty)
			{
				Rectangle r=this.colorPreview.ClientRectangle;
				r.Inflate(-2,-2);
				e.Graphics.DrawLine(SystemPens.ControlText,r.X,r.Y,r.Right,r.Bottom);
				e.Graphics.DrawLine(SystemPens.ControlText,r.Right,r.Y,r.X,r.Bottom);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.ClosePicker();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_Canceled=true;
			this.ClosePicker();
		}

		private void ClosePicker()
		{
			if(m_EditorService!=null)
			{
				m_EditorService.CloseDropDown();
			}
			else
			{
				if(this.Parent!=null)
					this.Parent.Hide();
				else
					this.Hide();
			}
		}

		private void ColorPicker_Paint(object sender, PaintEventArgs e)
		{
			Rectangle r=this.ClientRectangle;
			r.Width--;
			r.Height--;
			e.Graphics.DrawRectangle(SystemPens.ControlDarkDark,r);
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(tabControl1.SelectedIndex==0 && m_ColorScheme!=null && m_SelectedColorSchemeName!="")
				transparencyTrack.Enabled=false;
			else
				transparencyTrack.Enabled=true;
		}
		#endregion
	}
}
