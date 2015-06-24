using System;
using System.Windows.Forms;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for SideBarPanelControl.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false),System.Runtime.InteropServices.ComVisible(false)]
	public class SideBarPanelControlHost:Control,IThemeCache
	{
		private SideBarPanelItem m_ParentPanel=null;
		private BaseItem m_HotSubItem=null;
		private Point m_MouseDownPt=Point.Empty;

		// Theme Caching Support
		private ThemeWindow m_ThemeWindow=null;
		private ThemeRebar m_ThemeRebar=null;
		private ThemeToolbar m_ThemeToolbar=null;
		private ThemeHeader m_ThemeHeader=null;
		private ThemeScrollBar m_ThemeScrollBar=null;
		private ThemeExplorerBar m_ThemeExplorerBar=null;
		private ThemeProgress m_ThemeProgress=null;
        private ThemeButton m_ThemeButton = null;
		private ScrollButton m_TopScrollButton=null;
		private ScrollButton m_BottomScrollButton=null;

		public SideBarPanelControlHost(SideBarPanelItem parentPanel)
		{
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.TabStop=false;
			
			m_TopScrollButton=new ScrollButton();
			m_TopScrollButton.UseTimer=false;
			m_TopScrollButton.StandardButton=true;
			m_TopScrollButton.Visible=false;
			m_TopScrollButton.Orientation=eOrientation.Vertical;
			m_TopScrollButton.ButtonAlignment=eItemAlignment.Near;
			ColorScheme scheme=null;
			if(parentPanel.Appearance==eSideBarAppearance.Flat)
			{
				m_TopScrollButton.Size=new Size(14,14);
				if(parentPanel.HeaderStyle!=null)
				{
					scheme=new ColorScheme();
					scheme.ItemBackground=Color.Empty;
                    scheme.ItemBackground=Color.Empty;
					scheme.ItemBackground=parentPanel.HeaderStyle.BackColor1.Color;
					scheme.ItemHotBackground=parentPanel.HeaderStyle.BackColor1.Color;
					scheme.ItemHotBackground2=parentPanel.HeaderStyle.BackColor2.Color;
					scheme.ItemHotBorder=parentPanel.HeaderStyle.BorderColor.Color;
					scheme.ItemPressedBorder=parentPanel.HeaderStyle.BorderColor.Color;
					scheme.ItemText=parentPanel.HeaderStyle.ForeColor.Color;
                    scheme.ItemHotText=parentPanel.HeaderStyle.ForeColor.Color;
					scheme.ItemPressedText=parentPanel.HeaderStyle.ForeColor.Color;
					scheme.ItemPressedBackground=ControlPaint.Light(parentPanel.HeaderStyle.BackColor1.Color);
					scheme.ItemPressedBackground2=ControlPaint.Light(parentPanel.HeaderStyle.BackColor2.Color);
					m_TopScrollButton._Scheme=scheme;
					m_TopScrollButton.StandardButton=false;
				}
			}
			else
				m_TopScrollButton.Size=new Size(16,16);
			this.Controls.Add(m_TopScrollButton);
			m_BottomScrollButton=new ScrollButton();
			m_BottomScrollButton.UseTimer=false;
			m_BottomScrollButton.StandardButton=true;
			m_BottomScrollButton.Visible=false;
			m_BottomScrollButton.Orientation=eOrientation.Vertical;
			m_BottomScrollButton.ButtonAlignment=eItemAlignment.Far;
			if(parentPanel.Appearance==eSideBarAppearance.Flat)
			{
				m_BottomScrollButton.Size=new Size(14,14);
				if(scheme!=null)
				{
					m_BottomScrollButton._Scheme=scheme;
					m_BottomScrollButton.StandardButton=false;
				}
			}
			else
				m_BottomScrollButton.Size=new Size(16,16);
			this.Controls.Add(m_BottomScrollButton);

			m_BottomScrollButton.MouseDown+=new MouseEventHandler(ScrollButtonClick);
			m_TopScrollButton.MouseDown+=new MouseEventHandler(ScrollButtonClick);

			m_ParentPanel=parentPanel;
			foreach(BaseItem item in  m_ParentPanel.SubItems)
				item.ContainerControl=this;
		}

		public void SetupScrollButtons()
		{
			// Setup scroll buttons
			if(m_ParentPanel.TopItemIndex>0 && m_ParentPanel.EnableScrollButtons)
			{
				m_TopScrollButton.Location=new Point(this.Width-(m_TopScrollButton.Width+2),4);
				m_TopScrollButton.UseThemes=m_ParentPanel.UseThemes;
				m_TopScrollButton.BringToFront();
				if(!m_TopScrollButton.Visible)
				{
					m_TopScrollButton.Visible=true;
				}
			}
			else
				m_TopScrollButton.Visible=false;

			// Draw scroll down button
			if(m_ParentPanel.ScrollDownButton && m_ParentPanel.EnableScrollButtons)
			{
				m_BottomScrollButton.Location=new Point(this.Width-(m_BottomScrollButton.Width+2),this.Height-(m_BottomScrollButton.Height+2));
				m_BottomScrollButton.UseThemes=m_ParentPanel.UseThemes;
				m_BottomScrollButton.BringToFront();
				if(!m_BottomScrollButton.Visible)
				{
					m_BottomScrollButton.Visible=true;					
				}
			}
			else
				m_BottomScrollButton.Visible=false;
		}

		private void ScrollButtonClick(object sender, MouseEventArgs e)
		{
			if(e.Button!=MouseButtons.Left)
				return;
			if(sender==m_TopScrollButton)
			{
				// Scroll up
				m_ParentPanel.ScrollButtonClick(true);
			}
			else
			{
				m_ParentPanel.ScrollButtonClick(false);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if(m_ParentPanel==null || this.IsDisposed)
				return;

			ItemPaintArgs pa=null;
			SideBar bar=m_ParentPanel.ContainerControl as SideBar;
			if(bar!=null)
				pa=new ItemPaintArgs(m_ParentPanel.GetOwner() as IOwner,this,e.Graphics,bar.ColorScheme);
			else
				pa=new ItemPaintArgs(m_ParentPanel.GetOwner() as IOwner,this,e.Graphics,new ColorScheme(e.Graphics));
            pa.Renderer = bar.GetRenderer();
			if(m_ParentPanel.UseThemes)
			{
				((IThemeCache)this).ThemeRebar.DrawBackground(e.Graphics,ThemeRebarParts.Background,ThemeRebarStates.Normal,this.DisplayRectangle);
			}
			else
			{
                if (!m_ParentPanel.BackgroundStyle.BackColor1.IsEmpty)
                {
                    if (m_ParentPanel.Appearance == eSideBarAppearance.Flat)
                    {
                        eBorderSide oldBorder = m_ParentPanel.BackgroundStyle.BorderSide;
                        m_ParentPanel.BackgroundStyle.BorderSide = ~(oldBorder & eBorderSide.Top) & oldBorder;
                        m_ParentPanel.BackgroundStyle.Paint(e.Graphics, this.ClientRectangle);
                        m_ParentPanel.BackgroundStyle.BorderSide = oldBorder;
                    }
                    else
                        m_ParentPanel.BackgroundStyle.Paint(e.Graphics, this.ClientRectangle);
                }
                else
                {
                    Color backColor = SystemColors.Control;
                    if (bar != null && BarFunctions.IsOffice2007Style(bar.Style) && bar.BackColor == SystemColors.Control)
                    {
                        if(pa.Renderer is Office2007Renderer)
                        {
                            DisplayHelp.FillRectangle(e.Graphics, this.ClientRectangle,((Office2007Renderer)pa.Renderer).ColorTable.SideBar.Background);
                            backColor = Color.Empty;
                        }
                    }
                    else if (this.Parent != null && this.Parent.BackColor != Color.Transparent)
                    {
                        backColor = this.Parent.BackColor;
                    }
                    DisplayHelp.FillRectangle(e.Graphics, this.ClientRectangle, backColor, Color.Empty);
                }
			}
			for(int i=m_ParentPanel.TopItemIndex;i<m_ParentPanel.SubItems.Count;i++)
			{
				BaseItem item=m_ParentPanel.SubItems[i];
				if(!item.Displayed || !item.Visible)
					continue;
				item.Paint(pa);
			}
		}

        ///// <summary>
        ///// Returns the renderer control will be rendered with.
        ///// </summary>
        ///// <returns>The current renderer.</returns>
        //private Rendering.BaseRenderer GetRenderer()
        //{
        //    return Rendering.GlobalManager.Renderer;
        //}

		private void InternalClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
		{
			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalClick(mb,mpos);
				return;
			}
		}

		private void InternalDoubleClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
		{
			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalDoubleClick(mb,mpos);
			}
		}

		private void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
		{
			if(m_ParentPanel.DesignMode)
			{
				objArg.Handled=true;
				return;
			}
			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalKeyDown(objArg);
			}
		}

		private void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			IOwner owner=null;

			// Colapse any item if expanded
			m_MouseDownPt=new System.Drawing.Point(objArg.X,objArg.Y);
			BaseItem objItem=m_ParentPanel.ExpandedItem();
			if(objItem!=null && objItem!=m_HotSubItem && !m_ParentPanel.DesignMode)
			{
				objItem.Expanded=false;
				m_ParentPanel.AutoExpand=false;
			}

			if(m_ParentPanel.DesignMode && m_ParentPanel.CanCustomize)
			{
				if(m_ParentPanel.IsContainer && m_ParentPanel.SubItems.Count>0)
				{
					BaseItem objNew=ItemAtLocation(objArg.X,objArg.Y);
					if(objNew!=null && objNew.CanCustomize)
					{
						owner=m_ParentPanel.GetOwner() as IOwner;
						if(owner!=null)
							owner.SetFocusItem(objNew);
					}
					if(objNew!=null)
						objNew.InternalMouseDown(objArg);
				}
				
				if(objArg.Button==System.Windows.Forms.MouseButtons.Right && !m_ParentPanel.IsContainer)
				{
					owner=m_ParentPanel.GetOwner() as IOwner;
					if(owner!=null)
						owner.DesignTimeContextMenu(m_ParentPanel);
				}
			}

			if(!m_ParentPanel.DesignMode)
			{
				owner=m_ParentPanel.GetOwner() as IOwner;
				if(owner!=null && owner.GetFocusItem()!=null)
				{
					BaseItem objNew=ItemAtLocation(objArg.X,objArg.Y);
					if(objNew!=owner.GetFocusItem())
						owner.GetFocusItem().ReleaseFocus();
				}
			}

			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseDown(objArg);
			}
		}

		private void InternalMouseHover()
		{
			if(m_ParentPanel.DesignMode)
				return;

			if(m_HotSubItem!=null)
			{
				if(!m_ParentPanel.AutoExpand)
				{
					BaseItem objItem=m_ParentPanel.ExpandedItem();
					if(objItem!=null && m_HotSubItem!=objItem && (m_ParentPanel.IsOnMenu || m_ParentPanel.ContainerControl is Bar))
						objItem.Expanded=false;
				}
				// Changing the Expanded of the item can cause hot sub item to be set to null
				if(m_HotSubItem!=null)
					m_HotSubItem.InternalMouseHover();
			}
		}

		public virtual void InternalMouseLeave()
		{
			if(m_ParentPanel.DesignMode)
				return;

			// If we had hot sub item pass the mouse leave message to it...
			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseLeave();
				m_HotSubItem=null;
			}
		}

		private void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(m_ParentPanel.DesignMode && objArg.Button==System.Windows.Forms.MouseButtons.Left && (Math.Abs(objArg.X-m_MouseDownPt.X)>=2 || Math.Abs(objArg.Y-m_MouseDownPt.Y)>=2))
			{
				BaseItem objFocus=m_ParentPanel.FocusedItem();
				if(objFocus!=null && objFocus.CanCustomize)
				{
					if(!objFocus.SystemItem)
					{
						IOwner owner=m_ParentPanel.GetOwner() as IOwner;
						if(owner!=null)
							owner.StartItemDrag(objFocus);
					}
				}
				else if(!m_ParentPanel.IsContainer)
				{
					if(!m_ParentPanel.SystemItem && m_ParentPanel.CanCustomize)
					{
						IOwner owner=m_ParentPanel.GetOwner() as IOwner;
						if(owner!=null)
							owner.StartItemDrag(m_ParentPanel);
					}
				}
				return;
			}

			// If item is container and not in design time...
			if(m_ParentPanel.IsContainer && !m_ParentPanel.DesignMode)
			{
				BaseItem objNew=ItemAtLocation(objArg.X,objArg.Y);
				if(objNew!=m_HotSubItem)
				{
					if(m_HotSubItem!=null)
					{
						m_HotSubItem.InternalMouseLeave();
						if(objNew!=null && m_HotSubItem.Expanded && (m_ParentPanel.IsOnMenu || m_ParentPanel.ContainerControl is Bar))
							m_HotSubItem.Expanded=false;
					}
					
					if(objNew!=null)
					{
						if(m_ParentPanel.AutoExpand)
						{
							BaseItem objItem=m_ParentPanel.ExpandedItem();
							if(objItem!=null && objItem!=objNew)
								objItem.Expanded=false;
						}
						objNew.InternalMouseEnter();
						objNew.InternalMouseMove(objArg);
						if(m_ParentPanel.AutoExpand && objNew.Enabled && objNew.ShowSubItems)
						{
							if(objNew is PopupItem)
							{
								PopupItem pi=objNew as PopupItem;
								ePopupAnimation oldAnim=pi.PopupAnimation;
								pi.PopupAnimation=ePopupAnimation.None;
								if(objNew.SubItems.Count>0)
									objNew.Expanded=true;
								pi.PopupAnimation=oldAnim;
							}
							else
								objNew.Expanded=true;
						}
						m_HotSubItem=objNew;
						ResetHover();
					}
					else
						m_HotSubItem=null;
				}
				else if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseMove(objArg);
				}
			}
		}

		private BaseItem ItemAtLocation(int x, int y)
		{
			foreach(BaseItem objSub in m_ParentPanel.SubItems)
			{
				if((objSub.Visible || m_ParentPanel.IsOnCustomizeMenu) && objSub.Displayed && objSub.DisplayRectangle.Contains(x,y))
				{
					return objSub;
				}
			}
			return null;
		}

		private void ResetHover()
		{
			// We need to reset hover thing since it is fired only first time mouse hovers inside the window and we need it for each of our items
			NativeFunctions.TRACKMOUSEEVENT tme=new NativeFunctions.TRACKMOUSEEVENT();
			tme.dwFlags=NativeFunctions.TME_QUERY;
			tme.hwndTrack=this.Handle;
			tme.cbSize=System.Runtime.InteropServices.Marshal.SizeOf(tme);
			NativeFunctions.TrackMouseEvent(ref tme);
			tme.dwFlags=tme.dwFlags | NativeFunctions.TME_HOVER;
			NativeFunctions.TrackMouseEvent(ref tme);
		}

		protected override void OnClick(EventArgs e)
		{
			InternalClick(Control.MouseButtons,Control.MousePosition);
			base.OnClick(e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			InternalDoubleClick(Control.MouseButtons,Control.MousePosition);
			base.OnDoubleClick(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			InternalKeyDown(e);
			base.OnKeyDown(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			InternalMouseDown(e);
			base.OnMouseDown(e);
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			InternalMouseHover();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if(this.Cursor!=System.Windows.Forms.Cursors.Arrow)
				this.Cursor=System.Windows.Forms.Cursors.Arrow;
			InternalMouseLeave();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			InternalMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(m_HotSubItem!=null)
				m_HotSubItem.InternalMouseUp(e);
		}

		DevComponents.DotNetBar.ThemeWindow IThemeCache.ThemeWindow
		{
			get
			{
				if(m_ThemeWindow==null)
					m_ThemeWindow=new ThemeWindow(this);
				return m_ThemeWindow;
			}
		}
		DevComponents.DotNetBar.ThemeRebar IThemeCache.ThemeRebar
		{
			get
			{
				if(m_ThemeRebar==null)
					m_ThemeRebar=new ThemeRebar(this);
				return m_ThemeRebar;
			}
		}
		DevComponents.DotNetBar.ThemeToolbar IThemeCache.ThemeToolbar
		{
			get
			{
				if(m_ThemeToolbar==null)
					m_ThemeToolbar=new ThemeToolbar(this);
				return m_ThemeToolbar;
			}
		}
		DevComponents.DotNetBar.ThemeHeader IThemeCache.ThemeHeader
		{
			get
			{
				if(m_ThemeHeader==null)
					m_ThemeHeader=new ThemeHeader(this);
				return m_ThemeHeader;
			}
		}
		DevComponents.DotNetBar.ThemeScrollBar IThemeCache.ThemeScrollBar
		{
			get
			{
				if(m_ThemeScrollBar==null)
					m_ThemeScrollBar=new ThemeScrollBar(this);
				return m_ThemeScrollBar;
			}
		}
		DevComponents.DotNetBar.ThemeExplorerBar IThemeCache.ThemeExplorerBar
		{
			get
			{
				if(m_ThemeExplorerBar==null)
					m_ThemeExplorerBar=new ThemeExplorerBar(this);
				return m_ThemeExplorerBar;
			}
		}
		DevComponents.DotNetBar.ThemeProgress IThemeCache.ThemeProgress
		{
			get
			{
				if(m_ThemeProgress==null)
					m_ThemeProgress=new ThemeProgress(this);
				return m_ThemeProgress;
			}
		}
        DevComponents.DotNetBar.ThemeButton IThemeCache.ThemeButton
        {
            get
            {
                if (m_ThemeButton == null)
                    m_ThemeButton = new ThemeButton(this);
                return m_ThemeButton;
            }
        }

		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeThemes();
			base.OnHandleDestroyed(e);
		}

		private void DisposeThemes()
		{
			if(m_ThemeWindow!=null)
			{
				m_ThemeWindow.Dispose();
				m_ThemeWindow=null;
			}
			if(m_ThemeRebar!=null)
			{
				m_ThemeRebar.Dispose();
				m_ThemeRebar=null;
			}
			if(m_ThemeToolbar!=null)
			{
				m_ThemeToolbar.Dispose();
				m_ThemeToolbar=null;
			}
			if(m_ThemeHeader!=null)
			{
				m_ThemeHeader.Dispose();
				m_ThemeHeader=null;
			}
			if(m_ThemeScrollBar!=null)
			{
				m_ThemeScrollBar.Dispose();
				m_ThemeScrollBar=null;
			}
            if (m_ThemeProgress != null)
            {
                m_ThemeProgress.Dispose();
                m_ThemeProgress = null;
            }
            if (m_ThemeExplorerBar != null)
            {
                m_ThemeExplorerBar.Dispose();
                m_ThemeExplorerBar = null;
            }
            if (m_ThemeButton != null)
            {
                m_ThemeButton.Dispose();
                m_ThemeButton = null;
            }
		}
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_THEMECHANGED)
			{
				this.RefreshThemes();
			}
			base.WndProc(ref m);
		}

		private void RefreshThemes()
		{
			if(m_ThemeWindow!=null)
			{
				m_ThemeWindow.Dispose();
				m_ThemeWindow=new ThemeWindow(this);
			}
			if(m_ThemeRebar!=null)
			{
				m_ThemeRebar.Dispose();
				m_ThemeRebar=new ThemeRebar(this);
			}
			if(m_ThemeToolbar!=null)
			{
				m_ThemeToolbar.Dispose();
				m_ThemeToolbar=new ThemeToolbar(this);
			}
			if(m_ThemeHeader!=null)
			{
				m_ThemeHeader.Dispose();
				m_ThemeHeader=new ThemeHeader(this);
			}
			if(m_ThemeScrollBar!=null)
			{
				m_ThemeScrollBar.Dispose();
				m_ThemeScrollBar=new ThemeScrollBar(this);
			}
			if(m_ThemeProgress!=null)
			{
				m_ThemeProgress.Dispose();
				m_ThemeProgress=new ThemeProgress(this);
			}
			if(m_ThemeExplorerBar!=null)
			{
				m_ThemeExplorerBar.Dispose();
				m_ThemeExplorerBar=new ThemeExplorerBar(this);
			}
		}
	}
}
