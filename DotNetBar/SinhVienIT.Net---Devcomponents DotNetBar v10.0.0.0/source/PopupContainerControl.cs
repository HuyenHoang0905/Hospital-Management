using System;
using System.Windows.Forms;
using System.Drawing;
namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for PopupContainer.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class PopupContainerControl:UserControl
	{
		private BaseItem m_Parent=null;
		private bool m_DesignMode=false;
		private object m_Owner;
		private Point m_ParentScreenPos=Point.Empty;
		private PopupShadow m_DropShadow=null;

		public PopupContainerControl()
		{
			this.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
			this.SetStyle(ControlStyles.Selectable,false);
			//this.ControlBox=false;
			//this.MinimizeBox=false;
			//this.MaximizeBox=false;
			//this.Text="";
		}

		protected override void Dispose(bool disposing)
		{
			if(m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
			base.Dispose(disposing);
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams p=base.CreateParams;
				p.Style=unchecked((int)(NativeFunctions.WS_POPUP | NativeFunctions.WS_CLIPSIBLINGS | NativeFunctions.WS_CLIPCHILDREN));
				p.ExStyle=(int)(NativeFunctions.WS_EX_TOPMOST | NativeFunctions.WS_EX_TOOLWINDOW);
				p.Caption="";
				return p;
			}
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_MOUSEACTIVATE)
			{
				m.Result=new System.IntPtr(NativeFunctions.MA_NOACTIVATE);
				return;
			}
			base.WndProc(ref m);
		}

		public BaseItem ParentItem
		{
			get
			{
				return m_Parent;
			}
			set
			{
				m_Parent=value;
				// Get the parent's screen position
				if(m_Parent.Displayed)
				{
					System.Windows.Forms.Control objCtrl=m_Parent.ContainerControl as System.Windows.Forms.Control;
					if(BaseItem.IsHandleValid(objCtrl))
					{
						m_ParentScreenPos=objCtrl.PointToScreen(new Point(m_Parent.LeftInternal,m_Parent.TopInternal));
						objCtrl=null;
					}
				}
			}
		}

		internal void SetDesignMode(bool b)
		{
			m_DesignMode=b;
		}

		public void RecalcSize()
		{
			
		}

		public object Owner
		{
			get
			{
				return m_Owner;
			}
			set
			{
				m_Owner=value;
			}
		}

		public new Rectangle ClientRectangle
		{
			get
			{
				Rectangle r=base.ClientRectangle;
				if(IsGradientStyle)
				{
					if(this.DisplayShadow && this.AlphaShadow || !this.DisplayShadow)
					{
						r.Inflate(-1,-1);
					}
					else
					{
						r.Inflate(-2,-2);
						r.Width-=1;
						r.Height-=1;
					}
				}
				else
					r.Inflate(-2,-2);

				return r;
			}
		}

		public new Size ClientSize
		{
			get
			{
				return this.ClientRectangle.Size;
			}
			set
			{
				System.Drawing.Size v=value;
				if(IsGradientStyle)
				{
					if(this.DisplayShadow && this.AlphaShadow || !this.DisplayShadow)
					{
						v.Width+=2;
						v.Height+=2;
					}
					else
					{
						v.Width+=5;
						v.Height+=5;
					}
				}
				else
				{
					v.Width+=4;
					v.Height+=4;
				}
				base.ClientSize=v;
			}
		}

        private bool IsGradientStyle
        {
            get
            {
                if (m_Parent != null)
                {
                    return m_Parent.EffectiveStyle == eDotNetBarStyle.OfficeXP ||
                        m_Parent.EffectiveStyle == eDotNetBarStyle.Office2003 ||
                        m_Parent.EffectiveStyle == eDotNetBarStyle.VS2005 ||
                        BarFunctions.IsOffice2007Style(m_Parent.EffectiveStyle);
                }
                return true;
            }
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g=e.Graphics;
			if(IsGradientStyle)
			{
				if(this.DisplayShadow && !this.AlphaShadow)
					SetupRegion();
				Pen p=null;
				if(m_Parent!=null && m_Parent.ContainerControl is Bar)
					p=new Pen(((Bar)m_Parent.ContainerControl).ColorScheme.ItemHotBorder,1);
				else if(m_Parent!=null && m_Parent.ContainerControl is MenuPanel && ((MenuPanel)m_Parent.ContainerControl).ColorScheme!=null)
					p=new Pen(((MenuPanel)m_Parent.ContainerControl).ColorScheme.ItemHotBorder,1);
				else
					p=new Pen(ColorFunctions.MenuFocusBorderColor(g),1);
				// TODO Beta 2 Fix --> g.DrawRectangle(p,0,0,this.ClientSize.Width-2,this.ClientSize.Height-2);
				if(this.DisplayShadow && !this.AlphaShadow)
					NativeFunctions.DrawRectangle(g,p,0,0,base.ClientSize.Width-2,base.ClientSize.Height-2);
				else
					NativeFunctions.DrawRectangle(g,p,0,0,base.ClientSize.Width,base.ClientSize.Height);


				// Shadow
				if(this.DisplayShadow && !this.AlphaShadow)
				{
					p.Dispose();
					p=new Pen(SystemColors.ControlDark,2);
					Point[] pt=new Point[3];
					pt[0].X=2;
					pt[0].Y=base.ClientSize.Height-1;
					pt[1].X=base.ClientSize.Width-1;
					pt[1].Y=base.ClientSize.Height-1;
					pt[2].X=base.ClientSize.Width-1;
					pt[2].Y=2;
					g.DrawLines(p,pt);
				}

                if (m_Parent is ButtonItem && m_Parent.Displayed && !BarFunctions.IsOffice2007Style(m_Parent.EffectiveStyle))
				{
					// Determine where to draw the line based on parent position
					if(m_ParentScreenPos.Y<this.Location.Y)
					{
						Point p1=new Point((m_ParentScreenPos.X-this.Location.X)+1,0);
						Point p2=new Point(p1.X+m_Parent.WidthInternal-5,0);
						g.DrawLine(new Pen(ColorFunctions.ToolMenuFocusBackColor(g),1),p1,p2);
					}
				}
			}
			else
			{
				ControlPaint.DrawBorder3D(g,base.ClientRectangle,Border3DStyle.Raised,(Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom));
			}
		}

		private void SetupRegion()
		{
			if(this.DisplayShadow && this.AlphaShadow || !this.DisplayShadow)
				return;
			if(m_Parent!=null)
			{
				Rectangle r=new Rectangle(0,0,this.Width,this.Height);
				System.Drawing.Region rgn=new System.Drawing.Region(r);
				r.X=this.Width-2;
				r.Y=0;
				r.Width=2;
				r.Height=2;
				rgn.Xor(r);
				r.X=0;
				r.Y=this.Height-2;
				r.Width=2;
				r.Height=2;
				rgn.Xor(r);
				this.Region=rgn;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if(!this.Visible && m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
		}
		public new void Show()
		{
			if(this.DisplayShadow && this.AlphaShadow)
			{
				if(m_DropShadow!=null)
				{
					m_DropShadow.Hide();
					m_DropShadow.Dispose();
				}
				m_DropShadow=new PopupShadow(true);
				m_DropShadow.CreateControl();
				NativeFunctions.SetWindowPos(m_DropShadow.Handle,NativeFunctions.HWND_NOTOPMOST,this.Left+5,this.Top+5,this.Width-2,this.Height-2,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
			}
			base.Show();
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			//if(m_DropShadow!=null)
			//	NativeFunctions.SetWindowPos(m_DropShadow.Handle.ToInt32(),NativeFunctions.HWND_NOTOPMOST,this.Left+5,this.Top+5,this.Width-2,this.Height-2,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
		}

		private bool DisplayShadow
		{
			get
			{
				IOwnerMenuSupport ownersupport=m_Parent.GetOwner() as IOwnerMenuSupport;
				if(ownersupport!=null)
				{
                    if (m_Parent != null && m_Parent.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						if(ownersupport.MenuDropShadow==eMenuDropShadow.Show)
							return true;
						else
							return false;
					}
					return ownersupport.ShowPopupShadow;
				}
				else
				{
                    if (m_Parent != null && m_Parent.EffectiveStyle == eDotNetBarStyle.Office2000)
						return false;
				}

				return true;
			}
		}

		private bool AlphaShadow
		{
			get
			{
				if(Environment.OSVersion.Version.Major<5)
					return false;
				IOwnerMenuSupport ownersupport=m_Owner as IOwnerMenuSupport;
				if(ownersupport!=null && !ownersupport.AlphaBlendShadow)
					return false;
				return NativeFunctions.CursorShadow;
			}
		}
	}
}
