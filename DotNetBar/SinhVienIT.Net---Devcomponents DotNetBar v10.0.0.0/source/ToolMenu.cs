namespace DevComponents.DotNetBar
{
    using System;
	using System.Drawing;

	public struct SideBarImage
	{
		public Image Picture;
		public Color BackColor;
		public System.Drawing.Drawing2D.LinearGradientBrush GradientBrush;
		public eAlignment Alignment;
		public bool StretchPicture;
	}

    /// <summary>
    ///    Summary description for ToolMenu.
    /// </summary>
    [Serializable] public class ToolMenu:PopupItem
    {
		protected int m_PaddingTop, m_PaddingBottom, m_PaddingLeft, m_PaddingRight;
		protected bool m_MouseOver;

		public ToolMenu():this("") {}
        public ToolMenu(string sName):base(sName)
        {
			// This Bar pops-up its children, so they are not contained
			m_PaddingTop=2;
			m_PaddingBottom=3;
			m_PaddingLeft=7;
			m_PaddingRight=9;
			m_IsContainer=false;
			m_MouseOver=false;
			//m_SideBar=new SideBarImage();
			this.PopupType=ePopupType.Menu;
        }

		/// <summary>
		/// Returns copy of ToolMenu item
		/// </summary>
		public override BaseItem Copy()
		{
			ToolMenu objCopy=new ToolMenu();
			this.CopyToItem(objCopy);
			
			return objCopy;
		}

		/// <summary>
		/// Paints this base container
		/// </summary>
		public override void Paint(System.Drawing.Graphics g)
		{
			Font objFont=this.GetFont();
			if(m_Expanded && !this.DesignMode || m_Expanded && this.DesignMode && !m_MouseOver)
			{
				// Office 2000 Style
				if(m_Style==eDotNetBarStyle.Office)
				{
                    g.FillRectangle(SystemBrushes.Control,m_Rect);
					System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_Rect,System.Windows.Forms.Border3DStyle.SunkenOuter,System.Windows.Forms.Border3DSide.All);
				}
				else
				{
					// DotNet Style
					g.FillRectangle(SystemBrushes.Control,m_Rect);
					g.FillRectangle(new SolidBrush(ColorFunctions.ToolMenuFocusBackColor(g)),m_Rect.Left,m_Rect.Top,m_Rect.Width-2,m_Rect.Height);
					Point[] p=new Point[4];
					p[0].X=m_Rect.Left;
					p[0].Y=m_Rect.Top+m_Rect.Height-1;
					p[1].X=m_Rect.Left;
					p[1].Y=m_Rect.Top;
					p[2].X=m_Rect.Left+m_Rect.Width-3;
					p[2].Y=m_Rect.Top;
					p[3].X=m_Rect.Left+m_Rect.Width-3;
					p[3].Y=m_Rect.Top+m_Rect.Height-1;
					g.DrawLines(new Pen(ColorFunctions.MenuFocusBorderColor(g),1),p);
					// Draw the shadow
					g.FillRectangle(SystemBrushes.ControlDark,m_Rect.Left+m_Rect.Width-2,m_Rect.Top+2,2,m_Rect.Height-2);
				}
				
			}
			else if(m_MouseOver && (m_Enabled || this.DesignMode))
			{
				// Office 2000 Style
				if(!this.DesignMode && m_MouseOver)
				{
					if(m_Style==eDotNetBarStyle.Office)
					{
						g.FillRectangle(SystemBrushes.Control,m_Rect);
						System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_Rect,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All);
					}
					else
					{
						// DotNet Style
						g.FillRectangle(SystemBrushes.Control,m_Rect);
						g.FillRectangle(new SolidBrush(ColorFunctions.HoverBackColor(g)),m_Rect.Left,m_Rect.Top,m_Rect.Width-2,m_Rect.Height);
						// TODO: Beta 2 fix --> g.DrawRectangle(SystemPens.Highlight,m_Rect.Left,m_Rect.Top,m_Rect.Width-2,m_Rect.Height);
						NativeFunctions.DrawRectangle(g,SystemPens.Highlight,m_Rect.Left,m_Rect.Top,m_Rect.Width-2,m_Rect.Height);
					}
				}
			}
			else
				g.FillRectangle(SystemBrushes.Control,m_Rect);

			if(this.DesignMode && this.Focused)
			{
				Rectangle r=m_Rect;
				r.Inflate(-1,-1);
				g.DrawRectangle(new Pen(SystemColors.ControlText,2),r);
			}
			
			StringFormat sf=StringFormat.GenericDefault;
			sf.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
			sf.Alignment=System.Drawing.StringAlignment.Center;
			sf.LineAlignment=System.Drawing.StringAlignment.Center;
			if(m_Enabled || this.DesignMode)
			{
				g.DrawString(m_Text,objFont,SystemBrushes.ControlText,m_Rect,sf);
			}
			else
			{
				if(m_Style==eDotNetBarStyle.DotNet)
					g.DrawString(m_Text,objFont,SystemBrushes.ControlDark,m_Rect,sf);
				else
					System.Windows.Forms.ControlPaint.DrawStringDisabled(g,m_Text,objFont,SystemColors.Control,m_Rect,sf);
			}
		}

		/// <summary>
		/// Recalculate Size of this item
		/// </summary>
		public override void RecalcSize()
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(IsHandleValid(objCtrl))
			{
				Graphics g=objCtrl.CreateGraphics();
				Font f=this.GetFont();
				SizeF sf;
				StringFormat sfmt=StringFormat.GenericDefault;			
				sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
				sf=g.MeasureString(m_Text,f,0,sfmt);
				
				if(m_Style==eDotNetBarStyle.DotNet)
				{
					m_Rect.Width=(int)sf.Width+m_PaddingLeft+m_PaddingRight;
					m_Rect.Height=(int)sf.Height+m_PaddingTop+m_PaddingBottom;
				}
				else
				{
					m_Rect.Width=(int)sf.Width+6;
					m_Rect.Height=(int)sf.Height+6;
				}
				g.Dispose();
				objCtrl=null;
			}
			base.RecalcSize();
		}

		public override void MouseEnter()
		{
			m_MouseOver=true;
			this.Refresh();
		}

		public override void MouseLeave()
		{
			m_MouseOver=false;
			this.Refresh();
		}

		public override void MouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(objArg.Button != System.Windows.Forms.MouseButtons.Left || !m_Enabled && !this.DesignMode)
			{
				base.MouseDown(objArg);
				return;
			}

			// This behaviour is specific to menus and maybe the popup Bar items.
			// Needs to be incorporated when ToolMenu is moved into the ButtonItem
			if(m_ParentItem!=null && !this.DesignMode)
			{
				if(this.Expanded)
				{
					m_ParentItem.AutoExpand=false;
					Bar bar=this.ContainerControl as Bar;
					this.Expanded=!this.Expanded;
					if(bar!=null && bar.Focused)
						bar.ReleaseFocus();
				}
				else
				{
					m_ParentItem.AutoExpand=true;
					Bar bar=this.ContainerControl as Bar;
					if(bar!=null && !bar.Focused)
						bar.SetSystemFocus();
					this.Expanded=!this.Expanded;
				}
			}
			else if(this.DesignMode)
				this.Expanded=!this.Expanded;

			base.MouseDown(objArg);
		}

		/*public override void OnExpandChange()
		{
			base.OnExpandChange();
			if(m_ParentItem!=null && !this.DesignMode)
			{
				if(this.Expanded)
					m_ParentItem.AutoExpand=true;
				else
					m_ParentItem.AutoExpand=false;
			}
		}*/

		public override void KeyDown(System.Windows.Forms.KeyEventArgs objArg)
		{
			base.KeyDown(objArg);
			if(this.Expanded)
				return;

			if(objArg.KeyCode==System.Windows.Forms.Keys.Enter  || objArg.KeyCode==System.Windows.Forms.Keys.Return || objArg.KeyCode==System.Windows.Forms.Keys.Down)
			{
				if(SubItemsCount>0)
				{
					if(m_ParentItem!=null)
						m_ParentItem.AutoExpand=true;
					this.Expanded=true;
					objArg.Handled=true;
					return;
				}
			}
			else if(objArg.KeyCode==System.Windows.Forms.Keys.Escape)
			{
				if(SubItemsCount>0 && this.Expanded)
				{
					if(m_ParentItem!=null)
						m_ParentItem.AutoExpand=false;
					this.Expanded=false;
					objArg.Handled=true;
					return;
				}
			}

			//base.KeyDown(objArg);
		}

		protected virtual Font GetFont()
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(objCtrl!=null)
				return (Font)objCtrl.Font;
			return (Font)System.Windows.Forms.SystemInformation.MenuFont;
		}

		/*public override void KeyDown(System.Windows.Forms.KeyEventArgs objArg)
		{
			base.KeyDown(objArg);
			if(!m_Expanded && (objArg.KeyCode==System.Windows.Forms.Keys.Enter  || objArg.KeyCode==System.Windows.Forms.Keys.Return))
			{
				if(m_ParentItem!=null)
					m_ParentItem.AutoExpand=true;
				this.Expanded=true;
			}
			if(this.Expanded)
			{
				m_Popup.ExKeyDown(objArg);
			}
		}*/

		/*public override void OnExpandedChange()
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;

			if(!m_Expanded && m_Popup!=null)
			{
				ClosePopupMenu();
			}

			this.Refresh();

			if(m_Expanded && m_Popup==null)
			{
				if(IsHandleValid(objCtrl))
				{
					Point p=new Point(m_Rect.Left,m_Rect.Top+m_Rect.Height-1), ps;
					ps=objCtrl.PointToScreen(p);
					PopupMenu(ps);
				}	
			}
			objCtrl=null;
		}*/

		/*public void PopupMenu(Point p)
		{
			PopupMenu(p.X,p.Y);
		}*/

		/*public virtual void PopupMenu(int x, int y)
		{
			if(m_Popup!=null)
				ClosePopupMenu();

			if(m_Popup==null)
			{
				m_Popup=new PopupMenu();
				m_Popup.ParentItem=this;
				m_Popup.SideBar=m_SideBar;
				m_Popup.CreateControl();
				m_Popup.RecalcSize();
			}
			
			// Make sure that menu is on-screen
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			System.Windows.Forms.Screen objScreen=null;
			if(IsHandleValid(objCtrl))
			{
				objScreen=System.Windows.Forms.Screen.FromControl(objCtrl);
			}
			else
				objScreen=System.Windows.Forms.Screen.FromPoint(new Point(x,y));

			if(x+m_Popup.Width>objScreen.WorkingArea.Right)
			{
				x=x-((x+m_Popup.Width)-objScreen.WorkingArea.Right);
			}
			// Try to fit whole popup menu "nicely"
			if(y+m_Popup.Height>objScreen.WorkingArea.Bottom)
			{
				// If this container is displayed then try to put it above the menu item
				if(this.Displayed &&IsHandleValid(objCtrl))
				{
					Point p=new Point(m_Rect.Left,m_Rect.Top), ps;
					ps=objCtrl.PointToScreen(p);
					ps.Y+=2;
					if(ps.Y-m_Popup.Height>=objScreen.WorkingArea.Top)
						y=ps.Y-m_Popup.Height;
				}
				else
				{
					y=objScreen.WorkingArea.Bottom-m_Popup.Height;
					if(y<0)
						y=0;
				}
			}
			// If it still does not fit at this point, container will scale itself properly
			// And allow item scrolling
			
			m_Popup.Location=new Point(x,y);
			m_Popup.Show();
			m_Expanded=true;

			Bar bar=objCtrl as Bar;
			if(bar==null || bar.BarState!=eBarState.Popup)
			{
				System.Windows.Forms.Application.AddMessageFilter(this);
				m_FilterInstalled=true;
			}
			bar=null;
		}*/

		/*public virtual void ClosePopupMenu()
		{
			if(m_Popup!=null)
			{
				if(m_FilterInstalled)
					System.Windows.Forms.Application.RemoveMessageFilter(this);
				m_Popup.Hide();
				m_Popup.Dispose();
				m_Popup=null;
			}
			m_Expanded=false;
		}*/

//		public override void LostFocus()
//		{
//			base.LostFocus();
//			if(m_Expanded)
//			{
//				this.Expanded=false;
//				if(m_ParentItem!=null)
//					m_ParentItem.AutoExpand=false;
//			}
//		}

//		public override void SubItemSizeChanged(BaseItem objChildItem)
//		{
//			if(m_Popup!=null)
//			{
//				m_Popup.RecalcSize();
//				m_Popup.Refresh();
//			}
//		}


//		public bool PreFilterMessage(ref System.Windows.Forms.Message m)
//		{
//			// Block all the messages relating to the left mouse button.
//			if ((m.Msg >= NativeFunctions.WM_LBUTTONDOWN && m.Msg <= NativeFunctions.WM_LBUTTONDBLCLK) ||
//				(m.Msg>=NativeFunctions.WM_NCLBUTTONDOWN && m.Msg<=NativeFunctions.WM_NCMBUTTONDBLCLK))
//			{
//				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
//				
//				bool bChildHandle=this.IsAnyOnHandle(m.HWnd.ToInt32());
//
//				if(objCtrl!=null && m.HWnd!=objCtrl.Handle && !bChildHandle)
//				{
//					if(m_ParentItem!=null)
//					{
//						if(m_Expanded)
//						{
//							m_ParentItem.AutoExpand=false;
//						}
//						else
//						{
//							m_ParentItem.AutoExpand=true;
//						}
//					}
//					this.Expanded=!m_Expanded;
//				}
//				else if(objCtrl==null && !bChildHandle)
//				{
//					ClosePopupMenu();
//				}
//			}
//			else if(m.Msg==NativeFunctions.WM_ACTIVATEAPP && m.WParam.ToInt32()==0)
//			{
//				ClosePopupMenu();
//			}
//			
//			return false;
//		}
//
//		public void PostFilterMessage(ref System.Windows.Forms.Message m)
//		{
//        }
    }
}
