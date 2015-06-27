using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ScrollButton.
	/// </summary>
	[ToolboxItem(false)]
	internal class ScrollButton : System.Windows.Forms.UserControl
	{
		private bool m_MouseOver=false;
		private bool m_MouseDown=false;
		private eOrientation m_Orientation=eOrientation.Horizontal;
		private eItemAlignment m_ButtonAlignment=eItemAlignment.Near;
		private System.Windows.Forms.Timer m_ScrollTimer=null;
		public ColorScheme _Scheme=null;
		private ThemeScrollBar m_ThemeScrollBar=null;

		public bool UseThemes=false;
		public bool UseTimer=true;
		public bool StandardButton=false;

		public ScrollButton()
		{
			this.SetStyle(ControlStyles.Selectable,false);
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.TabStop=false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g=e.Graphics;
			if(StandardButton)
			{
				PaintStandardButton(e);
				return;
			}

			if(m_MouseOver)
			{
				if(_Scheme==null)
				{
					using(SolidBrush brush=new SolidBrush(ColorFunctions.HoverBackColor(g)))
						g.FillRectangle(brush,this.DisplayRectangle);
					//g.Clear(ColorFunctions.HoverBackColor(g));
					NativeFunctions.DrawRectangle(g,SystemPens.Highlight,this.ClientRectangle);
				}
				else
				{
					if(!_Scheme.ItemHotBackground2.IsEmpty)
					{
						System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(this.ClientRectangle,_Scheme.ItemHotBackground,_Scheme.ItemHotBackground2,_Scheme.ItemHotBackgroundGradientAngle);
						g.FillRectangle(gradient,this.ClientRectangle);
						gradient.Dispose();
					}
					else
					{
						using(SolidBrush brush=new SolidBrush(_Scheme.ItemHotBackground))
							g.FillRectangle(brush,this.DisplayRectangle);
						//g.Clear(_Scheme.ItemHotBackground);
					}
					NativeFunctions.DrawRectangle(g,new Pen(_Scheme.ItemHotBorder),this.ClientRectangle);
				}
			}
			else
			{
				if(_Scheme==null)
				{
					using(SolidBrush brush=new SolidBrush(SystemColors.Control))
						g.FillRectangle(brush,this.DisplayRectangle);
					//g.Clear(SystemColors.Control);
					NativeFunctions.DrawRectangle(g,SystemPens.ControlDark,this.ClientRectangle);
				}
				else
				{
					if(_Scheme.ItemBackground.IsEmpty)
					{
						using(SolidBrush brush=new SolidBrush(_Scheme.BarBackground))
							g.FillRectangle(brush,this.DisplayRectangle);
						//g.Clear(_Scheme.BarBackground);
					}
					else
					{
						if(_Scheme.ItemBackground2.IsEmpty)
						{
							using(SolidBrush brush=new SolidBrush(_Scheme.ItemBackground))
								g.FillRectangle(brush,this.DisplayRectangle);
							//g.Clear(_Scheme.ItemBackground);
						}
						else
						{
							using(System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(this.DisplayRectangle,_Scheme.ItemBackground,_Scheme.ItemBackground2,_Scheme.ItemBackgroundGradientAngle))
								g.FillRectangle(gradient,this.DisplayRectangle);
						}
					}
				}
			}
			if(m_Orientation==eOrientation.Horizontal)
			{
				if(m_ButtonAlignment==eItemAlignment.Far)
				{
					Point[] p=new Point[3];
					p[0].X=(this.ClientRectangle.Width/2)-2;
					p[0].Y=(this.ClientRectangle.Height-8)/2;
					p[1].X=p[0].X;
					p[1].Y=p[0].Y+8;
					p[2].X=p[0].X+4;
					p[2].Y=p[0].Y+4;
					if(_Scheme==null)
						g.FillPolygon(SystemBrushes.ControlText,p);
					else
						g.FillPolygon(new SolidBrush(_Scheme.ItemText),p);
				}
				else
				{
					Point[] p=new Point[3];
					p[0].X=(this.ClientRectangle.Width/2)+2;
					p[0].Y=(this.ClientRectangle.Height-8)/2;
					p[1].X=p[0].X;
					p[1].Y=p[0].Y+8;
					p[2].X=p[0].X-4;
					p[2].Y=p[0].Y+4;
					if(_Scheme==null)
						g.FillPolygon(SystemBrushes.ControlText,p);
					else
						g.FillPolygon(new SolidBrush(_Scheme.ItemText),p);
				}
			}
			else
			{
				if(m_ButtonAlignment==eItemAlignment.Far)
				{
					Point[] p=new Point[3];
					p[0].X=(this.ClientRectangle.Width)/2-3;
					p[0].Y=(this.ClientRectangle.Height-4)/2;
					p[1].X=p[0].X+7;
					p[1].Y=p[0].Y;
					p[2].X=p[0].X+3;
					p[2].Y=p[0].Y+4;
					if(_Scheme==null)
						g.FillPolygon(SystemBrushes.ControlText,p);
					else
						g.FillPolygon(new SolidBrush(_Scheme.ItemText),p);
				}
				else
				{
					Point[] p=new Point[3];
					p[0].X=(this.ClientRectangle.Width)/2-3;
					p[0].Y=(this.ClientRectangle.Height+3)/2;
					p[1].X=p[0].X+7;
					p[1].Y=p[0].Y;
					p[2].X=p[0].X+3;
					p[2].Y=p[0].Y-4;
					if(_Scheme==null)
						g.FillPolygon(SystemBrushes.ControlText,p);
					else
						g.FillPolygon(new SolidBrush(_Scheme.ItemText),p);
				}
			}
		}

		private void PaintStandardButton(PaintEventArgs e)
		{
			Graphics g=e.Graphics;
			if(UseThemes)
			{
				ThemeScrollBar scroll=this.ThemeScrollBar;
				ThemeScrollBarParts part=ThemeScrollBarParts.ArrowBtn;
				ThemeScrollBarStates state=ThemeScrollBarStates.ArrowBtnUpNormal;

				if(m_MouseOver)
				{
					if(m_Orientation==eOrientation.Vertical)
					{
						if(m_ButtonAlignment==eItemAlignment.Near)
							state=ThemeScrollBarStates.ArrowBtnUpHot;
						else
							state=ThemeScrollBarStates.ArrowBtnDownHot;
					}
					else
					{
						if(m_ButtonAlignment==eItemAlignment.Near)
							state=ThemeScrollBarStates.ArrowBtnLeftHot;
						else
							state=ThemeScrollBarStates.ArrowBtnRightHot;
					}
				}
				else
				{
					if(m_Orientation==eOrientation.Vertical)
					{
						if(m_ButtonAlignment==eItemAlignment.Near)
							state=ThemeScrollBarStates.ArrowBtnUpNormal;
						else
							state=ThemeScrollBarStates.ArrowBtnDownNormal;
					}
					else
					{
						if(m_ButtonAlignment==eItemAlignment.Near)
							state=ThemeScrollBarStates.ArrowBtnLeftNormal;
						else
							state=ThemeScrollBarStates.ArrowBtnRightNormal;
					}
				}

				scroll.DrawBackground(g,part,state,this.DisplayRectangle);
			}
			else
			{
				if(m_MouseDown)
					BarFunctions.DrawBorder3D(g,this.DisplayRectangle,System.Windows.Forms.Border3DStyle.SunkenInner);
				else
					BarFunctions.DrawBorder3D(g,this.DisplayRectangle,System.Windows.Forms.Border3DStyle.Raised);

				if(m_Orientation==eOrientation.Vertical)
				{
					PaintArrow(g,this.DisplayRectangle,SystemColors.ControlText,(m_ButtonAlignment==eItemAlignment.Near));
				}
			}
		}

		private void PaintArrow(Graphics g, Rectangle rect, Color c, bool up)
		{
			Point[] p=new Point[3];
			if(up)
			{
				p[0].X=rect.Left+(rect.Width-9)/2;
				p[0].Y=rect.Top+rect.Height/2+1;
				p[1].X=p[0].X+8;
				p[1].Y=p[0].Y;
				p[2].X=p[0].X+4;
				p[2].Y=p[0].Y-5;
			}
			else
			{
				p[0].X=rect.Left+(rect.Width-7)/2;
				p[0].Y=rect.Top+(rect.Height-4)/2;
				p[1].X=p[0].X+7;
				p[1].Y=p[0].Y;
				p[2].X=p[0].X+3;
				p[2].Y=p[0].Y+4;
			}
			g.FillPolygon(new SolidBrush(c),p);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			if(!m_MouseOver)
			{
				m_MouseOver=true;
				this.Refresh();
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			
			if(m_ScrollTimer!=null)
			{
				m_ScrollTimer.Stop();
				m_ScrollTimer.Dispose();
				m_ScrollTimer=null;
			}

			if(m_MouseOver)
			{
				m_MouseOver=false;
				this.Refresh();
			}
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			if(UseTimer)
				SetupScrollTimer();
		}

		public eOrientation Orientation
		{
			get
			{
				return m_Orientation;
			}
			set
			{
				if(m_Orientation!=value)
				{
					m_Orientation=value;
					this.Refresh();
				}
			}
		}

		public eItemAlignment ButtonAlignment
		{
			get
			{
				return m_ButtonAlignment;
			}
			set
			{
				if(m_ButtonAlignment!=value)
				{
					m_ButtonAlignment=value;
					this.Refresh();
				}
			}
		}

		private void SetupScrollTimer()
		{
			if(m_ScrollTimer==null)
			{
				m_ScrollTimer=new Timer();
				m_ScrollTimer.Interval=200;
				m_ScrollTimer.Tick+=new EventHandler(this.ScrollTimerTick);
				m_ScrollTimer.Start();
			}
		}

		private void ScrollTimerTick(object sender, EventArgs e)
		{
			base.InvokeOnClick(this,new EventArgs());
		}
		
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(m_ScrollTimer!=null)
			{
				m_ScrollTimer.Stop();
				m_ScrollTimer.Dispose();
				m_ScrollTimer=null;
			}
			base.Dispose(disposing);
		}

		private DevComponents.DotNetBar.ThemeScrollBar ThemeScrollBar
		{
			get
			{
				if(m_ThemeScrollBar==null)
					m_ThemeScrollBar=new ThemeScrollBar(this);
				return m_ThemeScrollBar;
			}
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeThemes();
			base.OnHandleDestroyed(e);
		}

		private void DisposeThemes()
		{
			if(m_ThemeScrollBar!=null)
			{
				m_ThemeScrollBar.Dispose();
				m_ThemeScrollBar=null;
			}
		}
		protected override void WndProc(ref Message m)
		{
			const int WM_MOUSEACTIVATE = 0x21;
			const int MA_NOACTIVATE = 3;
			if(m.Msg==NativeFunctions.WM_THEMECHANGED)
			{
				this.RefreshThemes();
			}
			else if(m.Msg==WM_MOUSEACTIVATE)
			{
				m.Result=new System.IntPtr(MA_NOACTIVATE);
				return;
			}
			base.WndProc(ref m);
		}

		private void RefreshThemes()
		{
			if(m_ThemeScrollBar!=null)
			{
				m_ThemeScrollBar.Dispose();
				m_ThemeScrollBar=new ThemeScrollBar(this);
			}
		}
	}
}
