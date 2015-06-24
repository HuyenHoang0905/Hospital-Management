using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.ColorPickerItem
{
    /// <summary>
    /// Represents the color selection control.
    /// </summary>
    [ToolboxItem(false)]
	internal class ColorContrastControl : System.Windows.Forms.Control
	{
		#region Events
        /// <summary>
        /// Occurs when SelectedColor has changed.
        /// </summary>
        public event EventHandler SelectedColorChanged;
        /// <summary>
        /// Raises SelectedColorChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnSelectedColorChanged(EventArgs e)
        {
            EventHandler handler = SelectedColorChanged;
            if (handler != null)
                handler(this, e);
        }
		#endregion
		
		#region Private Variables
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Color m_SelectedColor = Color.White;
		private Bitmap m_BlendBitmap=null;
		private double m_SelectedLuminance = -1;
		#endregion
		
		#region Constructor, Dispose
		public ColorContrastControl()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
		
		#region Internal Implementation
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			
			if(this.BackColor == Color.Transparent)
			{
				base.OnPaintBackground(e);
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(this.BackColor))
					g.FillRectangle(brush, this.ClientRectangle);
			}
			
			if(m_BlendBitmap!=null)
				g.DrawImageUnscaled(m_BlendBitmap, 0, 0);
			
			if(m_SelectedLuminance>=0)
			{
				int y = (int)(this.ClientRectangle.Height * (1 - m_SelectedLuminance));
				int x = m_BlendBitmap.Width + 4;
				GraphicsPath path = new GraphicsPath();
				path.AddLine(x, y, x + 7, y - 4);
				path.AddLine(x+ 7 , y - 4, x + 7, y + 4);
				path.CloseAllFigures();
				using(SolidBrush brush=new SolidBrush(Color.Black))
					g.FillPath(brush, path);
				path.Dispose();
			}
		}
		
		protected override void OnResize(EventArgs e)
		{
			CreateBlendBitmap();
			base.OnResize (e);
		}

		
		private void CreateBlendBitmap()
		{
			Rectangle clientRect = this.ClientRectangle;

			Bitmap bmp = new Bitmap(12, clientRect.Height, PixelFormat.Format24bppRgb);
			using (Graphics graph = Graphics.FromImage(bmp))
			{
				graph.FillRectangle(SystemBrushes.Control, clientRect);
				
				Color color = m_SelectedColor;
				int ry = color.R, gy = color.G, by = color.B;
				
				int pointHeight = 4;
				int pointWidth = 12;
				
				int x = clientRect.X;
				int y = clientRect.Y;
				double h = 0, s = 0, l = 0;
				GetHSLFromRGB(m_SelectedColor, ref h, ref s, ref l);

				for (int j = clientRect.Y; j < clientRect.Height; j += pointHeight)
				{
					double lumCurrent = 1 - (double) j / (double) clientRect.Height;
					Color c = GetColorFromHSL(h, s, lumCurrent);
					using (SolidBrush brush = new SolidBrush(c))
						graph.FillRectangle(brush, new Rectangle(x, y, pointWidth, pointHeight));
					y += pointHeight;					
				}
			}
			
			if(m_BlendBitmap!=null)
				m_BlendBitmap.Dispose();
			m_BlendBitmap = bmp;
		}
		
        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectedColor
		{
			get
			{
				if(m_SelectedLuminance>=0)
				{
					double h = 0, s = 0, l = 0;
					GetHSLFromRGB(m_SelectedColor, ref h, ref s, ref l);
					return GetColorFromHSL(h, s, m_SelectedLuminance);
				}
				return m_SelectedColor;
			}
			set
			{
				m_SelectedColor = value;
				double h = 0, s = 0, l = 0;
				GetHSLFromRGB(m_SelectedColor, ref h, ref s, ref l);
                if(m_SelectedLuminance<0)
				    m_SelectedLuminance = l;
				CreateBlendBitmap();
				this.Invalidate();
			}
		}
		
		private int GetRGBFromHue(float rm1, float rm2, float rh)
		{
			if (rh > 360.0f)
				rh -= 360.0f;
			else if (rh < 0.0f)
				rh += 360.0f;

			if (rh <  60.0f)
				rm1 = rm1 + (rm2 - rm1) * rh / 60.0f;   
			else if (rh < 180.0f)
				rm1 = rm2;
			else if (rh < 240.0f)
				rm1 = rm1 + (rm2 - rm1) * (240.0f - rh) / 60.0f;      

			return (int)(rm1 * 255);
		}
		
		private Color GetColorFromHSL( double H, double S, double L)
		{
			double r=0,g=0,b=0;
			double temp1,temp2; 
			if(L==0) 
			{ 
				r=g=b=0; 
			} 
			else
			{
				if(S==0) 
				{ 
					r=g=b=L; 
				} 
				else 
				{ 
					temp2 = ((L<=0.5) ? L*(1.0+S) : L+S-(L*S)); 
					temp1 = 2.0*L-temp2; 
					double[] t3=new double[]{H+1.0/3.0,H,H-1.0/3.0}; 
					double[] clr=new double[]{0,0,0}; 
					for(int i=0;i<3;i++) 
					{ 
						if(t3[i]<0) 
							t3[i]+=1.0; 

						if(t3[i]>1) 
							t3[i]-=1.0; 

						if(6.0*t3[i] < 1.0) 
							clr[i]=temp1+(temp2-temp1)*t3[i]*6.0; 
						else if(2.0*t3[i] < 1.0) 
							clr[i]=temp2; 
						else if(3.0*t3[i] < 2.0) 
							clr[i]=(temp1+(temp2-temp1)*((2.0/3.0)-t3[i])*6.0); 
						else 
							clr[i]=temp1; 
					} 

					r=clr[0]; 
					g=clr[1]; 
					b=clr[2]; 
				}
			}
			
			return Color.FromArgb((int)(255*r),(int)(255*g),(int)(255*b));
		}
		
		private void GetHSLFromRGB(Color color, ref double H, ref double S, ref double L )
		{   
			H=color.GetHue()/360.0; // we store hue as 0-1 as opposed to 0-360 
			L=color.GetBrightness(); 
			S=color.GetSaturation(); 
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			double d = 1 - (double) e.Y / (double) ClientRectangle.Height;
			SetLuminance(d);
			base.OnMouseDown (e);
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left && e.Y>=0  && e.Y<ClientRectangle.Bottom)
			{
				double d = 1 - (double) e.Y / (double) ClientRectangle.Height;
				SetLuminance(d);
			}
			base.OnMouseMove (e);
		}
		
		private void SetLuminance(double d)
		{
			m_SelectedLuminance = d;
			this.Invalidate();
			if(SelectedColorChanged!=null)
				SelectedColorChanged(this, new EventArgs());
		}
		#endregion
		
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ColorContrastControl
			// 
			this.Name = "ColorContrastControl";
			this.Size = new System.Drawing.Size(24, 248);

		}
		#endregion
	}
}
