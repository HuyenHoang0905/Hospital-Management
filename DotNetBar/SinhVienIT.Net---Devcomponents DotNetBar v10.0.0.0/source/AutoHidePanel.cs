using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false),System.Runtime.InteropServices.ComVisible(false)]
	public class AutoHidePanel : System.Windows.Forms.Control
	{
		private ArrayList m_Panels=new ArrayList(10);
		private System.Windows.Forms.Timer m_Timer=null;

		private eDotNetBarStyle m_Style=eDotNetBarStyle.OfficeXP;

		private bool m_EnableHoverExpand=true;
		private bool m_EnableFocusCollapse=true;

		private int m_AutoHideShowTimeout=800;
		private ColorScheme m_ColorScheme=null;
		private DotNetBarManager m_Owner=null;

		public AutoHidePanel()
		{
			this.SetStyle(ControlStyles.Selectable,false);
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
            this.Font = SystemInformation.MenuFont.Clone() as Font;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing)
			{
				DestroyTimer();
				foreach(PanelBar panel in m_Panels)
				{
					panel.Dispose();
				}
				m_Panels.Clear();
			}
			base.Dispose( disposing );
		}

		internal void SetOwner(DotNetBarManager owner)
		{
			m_Owner=owner;
		}

        private bool m_AntiAlias = false;
        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set
            {
                if (m_AntiAlias != value)
                {
                    m_AntiAlias = value;
                    this.Invalidate();
                }
            }
        }

		protected override void OnPaint(PaintEventArgs p)
		{
            eTextFormat format = eTextFormat.Default | eTextFormat.VerticalCenter | eTextFormat.SingleLine;
			Graphics g=p.Graphics;
			ColorScheme scheme=null;

            if (this.BackColor == Color.Transparent || this.BackgroundImage != null)
            {
                base.OnPaintBackground(p);
            }

            TextRenderingHint textHint = g.TextRenderingHint;
            SmoothingMode sm = g.SmoothingMode;
            if (m_AntiAlias)
            {
                g.TextRenderingHint = BarUtilities.AntiAliasTextRenderingHint;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            bool office2007Style = BarFunctions.IsOffice2007Style(m_Style);

            if (office2007Style && m_ColorScheme == null && Rendering.GlobalManager.Renderer is Rendering.Office2007Renderer)
                scheme = ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable.LegacyColors;
			else if(m_Owner!=null && m_Owner.UseGlobalColorScheme)
				scheme=m_Owner.ColorScheme;
			else if(m_ColorScheme!=null)
				scheme=m_ColorScheme;
			else
				scheme=new ColorScheme(m_Style);

            Rectangle clientRect = this.ClientRectangle;
            clientRect.Inflate(1, 1);
            if (scheme.AutoHidePanelBackgroundImage != null)
                BarFunctions.PaintBackgroundImage(g, clientRect, scheme.AutoHidePanelBackgroundImage, eBackgroundImagePosition.Tile, 255);

            if (!scheme.AutoHidePanelBackground.IsEmpty)
            {
                DisplayHelp.FillRectangle(g, clientRect, scheme.AutoHidePanelBackground, scheme.AutoHidePanelBackground2, 90);
            }
            else if (IsGradientStyle)
            {
                DisplayHelp.FillRectangle(g, clientRect, scheme.BarBackground, scheme.BarBackground2, scheme.BarBackgroundGradientAngle);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(ControlPaint.Light(this.BackColor)))
                    g.FillRectangle(brush, this.DisplayRectangle);
            }
            
            if (m_Style == eDotNetBarStyle.VS2005 || office2007Style)
            {
                VS2005TabDisplay td = new VS2005TabDisplay(this, scheme);
                td.DisplayTextForActiveTabOnly = office2007Style;
                td.Paint(g, m_Panels);


                if (m_AntiAlias)
                {
                    g.TextRenderingHint = textHint;
                    g.SmoothingMode = sm;
                }

                return;
            }

			const int margin=2;
			int width=this.Width-2;
			int height=this.Height-2;

			Color textColor=Color.Empty;
			SolidBrush backBrush=null;
			Pen linePen=null;
			if(m_Style==eDotNetBarStyle.Office2003)
			{
                textColor = scheme.ItemText;
				linePen=new Pen(scheme.ItemHotBorder,1);
			}
            else if (m_Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_Style))
			{
                textColor = scheme.ItemText;
				linePen=new Pen(scheme.MenuBorder,1);
			}
			else
			{
                textColor = g.GetNearestColor(ControlPaint.DarkDark(this.BackColor));
				backBrush=new SolidBrush(this.BackColor);
				linePen=new Pen(g.GetNearestColor(ControlPaint.Dark(this.BackColor)),1);
			}

			if(this.Dock==DockStyle.Top || this.Dock==DockStyle.Bottom || this.Dock==DockStyle.None)
			{
				int x=2;
				foreach(PanelBar panel in m_Panels)
				{
					for(int i=0;i<panel.Tabs.Count;i++)
					{
						DockItemTab tab=panel.Tabs[i] as DockItemTab;
						CompositeImage icon=tab.Icon;
						tab.DisplayRectangle.X=x;
						tab.DisplayRectangle.Y=0;
						tab.DisplayRectangle.Height=height;
						x+=PanelBar.TabPadding;
						if(panel.ActiveTab==i || panel.BoundBar.AutoHideTabTextAlwaysVisible)
						{
							if(icon!=null)
							{
								
								if(this.Dock==DockStyle.Top)
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tab.DisplayRectangle.X,0,icon.Width+panel.MaxTextWidth+PanelBar.TabPadding*3,height),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,tab.DisplayRectangle.X,0,icon.Width+panel.MaxTextWidth+PanelBar.TabPadding*3,height);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,0,icon.Width+panel.MaxTextWidth+PanelBar.TabPadding*3,height);
									icon.DrawImage(g,new Rectangle(x,(height-icon.Height)/2,icon.Width,icon.Height));
									//g.DrawImage(icon,x,(height-icon.Height)/2,icon.Width,icon.Height);
								}
								else
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tab.DisplayRectangle.X,margin,icon.Width+panel.MaxTextWidth+PanelBar.TabPadding*3,height),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,tab.DisplayRectangle.X,margin,icon.Width+panel.MaxTextWidth+PanelBar.TabPadding*3,height);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,margin,icon.Width+panel.MaxTextWidth+PanelBar.TabPadding*3,height);
									icon.DrawImage(g,new Rectangle(x,(height-icon.Height)/2+margin,icon.Width,icon.Height));
									//g.DrawImage(icon,x,(height-icon.Height)/2+margin,icon.Width,icon.Height);
								}
								x+=(icon.Width+PanelBar.TabPadding);
							}
							else
							{
								if(this.Dock==DockStyle.Top)
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tab.DisplayRectangle.X,0,tab.TextSize.Width+PanelBar.TabPadding*2,height),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,tab.DisplayRectangle.X,0,tab.TextSize.Width+PanelBar.TabPadding*2,height);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,0,tab.TextSize.Width+PanelBar.TabPadding*2,height);
								}
								else
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tab.DisplayRectangle.X,margin,tab.TextSize.Width+PanelBar.TabPadding*2,height),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,tab.DisplayRectangle.X,margin,tab.TextSize.Width+PanelBar.TabPadding*2,height);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,margin,tab.TextSize.Width+PanelBar.TabPadding*2,height);
								}
							}

							TextDrawing.DrawStringLegacy(g,tab.Text,this.Font,textColor,new Rectangle(x,this.Dock==DockStyle.Top?0:margin,(icon==null?tab.TextSize.Width:panel.MaxTextWidth),height),format);
							x+=((icon==null?tab.TextSize.Width:panel.MaxTextWidth)+PanelBar.TabPadding);
							tab.DisplayRectangle.Width=x-tab.DisplayRectangle.X;
							if(this.Dock==DockStyle.Top)
								tab.DisplayRectangle.Offset(0,-1);
							else
							{
								tab.DisplayRectangle.Offset(0,1);
								tab.DisplayRectangle.Height+=2;
							}
							g.DrawRectangle(linePen,tab.DisplayRectangle);
						}
						else
						{
							if(icon!=null)
							{
								if(this.Dock==DockStyle.Top)
								{
									if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,0,icon.Width+PanelBar.TabPadding*2,height);
									icon.DrawImage(g,new Rectangle(x,(height-icon.Height)/2,icon.Width,icon.Height));
									//g.DrawImage(icon,x,(height-icon.Height)/2,icon.Width,icon.Height);
								}
								else
								{
									if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,margin,icon.Width+PanelBar.TabPadding*2,height);
									icon.DrawImage(g,new Rectangle(x,(height-icon.Height)/2+margin,icon.Width,icon.Height));
									//g.DrawImage(icon,x,(height-icon.Height)/2+margin,icon.Width,icon.Height);
								}
								x+=(icon.Width+PanelBar.TabPadding);
							}
							else
							{
								TextDrawing.DrawStringLegacy(g,tab.Text,this.Font,textColor,new Rectangle(x,this.Dock==DockStyle.Top?0:margin,tab.TextSize.Width,height),format);
								x+=(tab.TextSize.Width+PanelBar.TabPadding);
							}
							tab.DisplayRectangle.Width=x-tab.DisplayRectangle.X;
							if(this.Dock==DockStyle.Top)
								tab.DisplayRectangle.Y--;
							else
							{
								tab.DisplayRectangle.Y++;
								tab.DisplayRectangle.Height+=2;
							}
							g.DrawRectangle(linePen,tab.DisplayRectangle);
						}
						if(x>this.Width)
							break;
					}
					x+=4;
					if(x>this.Width)
						break;
				}
			}
			else if(this.Dock==DockStyle.Left || this.Dock==DockStyle.Right)
			{
				int y=2;
				foreach(PanelBar panel in m_Panels)
				{
					for(int i=0;i<panel.Tabs.Count;i++)
					{
						DockItemTab tab=panel.Tabs[i] as DockItemTab;
						CompositeImage icon=tab.Icon;
						tab.DisplayRectangle.X=0;
						tab.DisplayRectangle.Y=y;
						tab.DisplayRectangle.Width=width;
						y+=PanelBar.TabPadding;
                        if (panel.ActiveTab == i || panel.BoundBar.AutoHideTabTextAlwaysVisible)
						{
							if(icon!=null)
							{
								if(this.Dock==DockStyle.Left)
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tab.DisplayRectangle.X,tab.DisplayRectangle.Y,width,icon.Height+panel.MaxTextWidth+PanelBar.TabPadding*3),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,tab.DisplayRectangle.X,tab.DisplayRectangle.Y,width,icon.Height+panel.MaxTextWidth+PanelBar.TabPadding*3);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,tab.DisplayRectangle.Y,width,icon.Height+panel.MaxTextWidth+PanelBar.TabPadding*3);
									icon.DrawImage(g,new Rectangle((width-icon.Width-margin)/2,y,icon.Width,icon.Height));
									//g.DrawImage(icon,(width-icon.Width-margin)/2,y,icon.Width,icon.Height);
								}
								else
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(margin,tab.DisplayRectangle.Y,width,icon.Height+panel.MaxTextWidth+PanelBar.TabPadding*3),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,margin,tab.DisplayRectangle.Y,width,icon.Height+panel.MaxTextWidth+PanelBar.TabPadding*3);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,margin,tab.DisplayRectangle.Y,width,icon.Height+panel.MaxTextWidth+PanelBar.TabPadding*3);
									icon.DrawImage(g,new Rectangle((width-icon.Width+margin)/2,y,icon.Width,icon.Height));
									//g.DrawImage(icon,(width-icon.Width+margin)/2,y,icon.Width,icon.Height);
								}
								y+=(icon.Height+PanelBar.TabPadding);
							}
							else
							{
								if(this.Dock==DockStyle.Left)
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tab.DisplayRectangle.X,tab.DisplayRectangle.Y,width,tab.TextSize.Width+PanelBar.TabPadding*2),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,tab.DisplayRectangle.X,tab.DisplayRectangle.Y,width,tab.TextSize.Width+PanelBar.TabPadding*2);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,tab.DisplayRectangle.Y,width,tab.TextSize.Width+PanelBar.TabPadding*2);
								}
								else
								{
									if((m_Style==eDotNetBarStyle.Office2003 || m_Style==eDotNetBarStyle.VS2005) && !scheme.ItemPressedBackground2.IsEmpty)
									{
										using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(margin,tab.DisplayRectangle.Y,width,tab.TextSize.Width+PanelBar.TabPadding*2),scheme.ItemPressedBackground,scheme.ItemPressedBackground2,scheme.ItemPressedBackgroundGradientAngle))
											g.FillRectangle(gradient,margin,tab.DisplayRectangle.Y,width,tab.TextSize.Width+PanelBar.TabPadding*2);
											
									}
									else if(backBrush!=null)
										g.FillRectangle(backBrush,margin,tab.DisplayRectangle.Y,width,tab.TextSize.Width+PanelBar.TabPadding*2);
								}
							}
							g.RotateTransform(90);
                            TextDrawing.DrawStringLegacy(g, tab.Text, this.Font, textColor, new Rectangle(y, -width, (icon == null ? tab.TextSize.Width : panel.MaxTextWidth), width), format);
							g.ResetTransform();
							y+=((icon==null?tab.TextSize.Width:panel.MaxTextWidth)+PanelBar.TabPadding);
							if(this.Dock==DockStyle.Left)
								tab.DisplayRectangle.Offset(-1,0);
							else
								tab.DisplayRectangle.Offset(1,0);
							tab.DisplayRectangle.Height=y-tab.DisplayRectangle.Y;
							g.DrawRectangle(linePen,tab.DisplayRectangle);
						}
						else
						{
							if(icon!=null)
							{
								if(this.Dock==DockStyle.Left)
								{
									if(backBrush!=null)
										g.FillRectangle(backBrush,tab.DisplayRectangle.X,tab.DisplayRectangle.Y,width,icon.Height+PanelBar.TabPadding*2);
									icon.DrawImage(g,new Rectangle((width-icon.Width-margin)/2,y,icon.Width,icon.Height));
									//g.DrawImage(icon,(width-icon.Width-margin)/2,y,icon.Width,icon.Height);
								}
								else
								{
									if(backBrush!=null)
										g.FillRectangle(backBrush,margin,tab.DisplayRectangle.Y,width,icon.Height+PanelBar.TabPadding*2);
									icon.DrawImage(g,new Rectangle((width-icon.Width+margin)/2,y,icon.Width,icon.Height));
									//g.DrawImage(icon,(width-icon.Width+margin)/2,y,icon.Width,icon.Height);
								}
								y+=(icon.Height+PanelBar.TabPadding);
							}
							else
							{
								g.RotateTransform(90);
                                TextDrawing.DrawStringLegacy(g, tab.Text, this.Font, textColor, new Rectangle(y, -width, tab.TextSize.Width, width), format);
								g.ResetTransform();
								y+=(tab.TextSize.Width+PanelBar.TabPadding);
							}
							tab.DisplayRectangle.Height=y-tab.DisplayRectangle.Y;
							if(this.Dock==DockStyle.Left)
								tab.DisplayRectangle.X--;
							else
								tab.DisplayRectangle.X++;
							g.DrawRectangle(linePen,tab.DisplayRectangle);
						}
						if(y>this.Height)
							break;
					}
					y+=4;
					if(y>this.Height)
						break;
				}
			}

			linePen.Dispose();


            if (m_AntiAlias)
            {
                g.TextRenderingHint = textHint;
                g.SmoothingMode = sm;
            }
		}

        private bool IsGradientStyle
        {
            get
            {
                if (m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_Style))
                    return true;
                return false;
            }
        }

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			CreateTimer();

		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if(m_Timer!=null)
				m_Timer.Interval=m_AutoHideShowTimeout;
		}
		private void CreateTimer()
		{
			if(m_Timer!=null)
			{
				m_Timer.Start();
				return;
			}
			m_Timer=new Timer();
			m_Timer.Interval=m_AutoHideShowTimeout;
			m_Timer.Tick+=new EventHandler(this.TimerTick);
			m_Timer.Start();
		}
		internal void StopTimer()
		{
			if(m_Timer!=null)
				m_Timer.Stop();
		}
		internal void StartTimer()
		{
			if(m_Timer!=null)
				m_Timer.Start();
		}

		private void TimerTick(object sender, EventArgs e)
		{
			Point p=this.PointToClient(Control.MousePosition);
			
			if(this.ClientRectangle.Contains(p))
			{
				IntPtr activeWnd=NativeFunctions.GetActiveWindow();
				Form parentForm=this.FindForm();
				if(parentForm!=null && parentForm.Handle!=activeWnd)
				{
					Control c=parentForm;
					bool bExit=true;
					while(c.Parent!=null)
					{
						c=c.Parent;
						if(c.Handle==activeWnd)
						{
							bExit=false;
							break;
						}
					}
					if(bExit)
						return;
				}
				if(!m_EnableHoverExpand)
					return;
				SelectPanel(p.X,p.Y);
			}
			else
			{
				PanelBar activePanel=ActivePanelBar;
				if(activePanel!=null && !activePanel.BoundBar.IsSizingWindow)
				{
					if(activePanel.BoundBar.Visible && !(!m_EnableFocusCollapse && IsBarActive(activePanel.BoundBar)))
					{
						p=activePanel.BoundBar.PointToClient(Control.MousePosition);
						if(!activePanel.BoundBar.ClientRectangle.Contains(p))
						{
                            if (AnimateHide(activePanel.BoundBar))
                                DestroyTimer();
						}
					}
				}
			}
            
		}
		private void DestroyTimer()
		{
			if(m_Timer==null)
				return;
			m_Timer.Stop();
			m_Timer.Dispose();
			m_Timer=null;
		}
		private bool IsBarActive(Bar bar)
		{
			if(bar==null)
				return false;
			Form form=this.FindForm();
			if(form==null)
				return false;
			Control activeControl=form.ActiveControl;
			if(activeControl==null)
				return false;
			while(activeControl!=null)
			{
				if(bar==activeControl)
					return true;
				activeControl=activeControl.Parent;
			}
			return false;
		}

		/// <summary>
		/// Gets or sets the timeout in milliseconds for auto hide/show action.
		/// When timeout has elapsed and mouse has left the bar the bar will be automatically hidden.
		/// If mouse is hovering over the collapsed bar and timeout has elapsed the bar will be displayed.
		/// </summary>
		[DefaultValue(800), Category("Behavior"),Description("Indicates timeout in milliseconds for auto hide/show action.")]
		public int AutoHideShowTimeout
		{
			get {return m_AutoHideShowTimeout;}
			set {m_AutoHideShowTimeout=value;}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(e.Button==MouseButtons.Left)
			{
				SelectPanel(e.X,e.Y);
			}
		}

		public void SelectPanel(int x, int y)
		{
			foreach(PanelBar panel in m_Panels)
			{
				foreach(DockItemTab tab in panel.Tabs)
				{
					if(tab.DisplayRectangle.Contains(x,y))
					{
						PanelBar activePanel=ActivePanelBar;
						if(activePanel!=null)
						{
							if(activePanel!=panel)
							{
								if(activePanel.BoundBar.IsSizingWindow)
									return;
								if(!AnimateHide(activePanel.BoundBar))
                                    return;
							}
							else
							{
                                if (panel.ActiveTab < panel.Tabs.Count)
                                {
                                    if (panel.Tabs[panel.ActiveTab] == tab && panel.BoundBar.Visible)
                                        return;
                                    ((DockItemTab)activePanel.Tabs[activePanel.ActiveTab]).Item.Displayed = false;
                                }
							}
						}
                        if (panel.BoundBar.VisibleItemCount == 0)
                            throw new InvalidOperationException("Bar.Items collection must contain at least one visible DockContainerItem object so auto-hide functionality can function properly.");
						panel.ActiveTab=panel.Tabs.IndexOf(tab);
                        if (panel.BoundBar.IsDisposed) return;
						panel.BoundBar.SelectedDockTab=panel.BoundBar.Items.IndexOf(tab.Item);
						AnimateShow(panel.BoundBar);
						this.Refresh();
						return;
					}
				}
			}
		}

		/// <summary>
		/// Returns the reference to DockContainerItem tab if any under specified coordinates.
		/// </summary>
		/// <param name="x">X - client mouse coordinate</param>
		/// <param name="y">Y - client mouse coordinate</param>
		/// <returns>Reference to DockContainerItem whose tab is at specified coordinates or null if there is no tab at given coordinates</returns>
		public DockContainerItem HitTest(int x, int y)
		{
			foreach(PanelBar panel in m_Panels)
			{
				foreach(DockItemTab tab in panel.Tabs)
				{
					if(tab.DisplayRectangle.Contains(x,y))
					{
						return tab.Item;
					}
				}
			}
			return null;
		}

		internal DockContainerItem SelectedDockContainerItem
		{
			get
			{
				PanelBar panel=this.ActivePanelBar;
				if(panel==null)
				{
					foreach(PanelBar p in m_Panels)
					{
						for(int i=0;i<p.Tabs.Count;i++)
						{
							DockItemTab tab=p.Tabs[i] as DockItemTab;
							if(p.ActiveTab==i)
								return tab.Item;
						}
					}
					return null;
				}
				try
				{
					DockItemTab tab=panel.Tabs[panel.ActiveTab] as DockItemTab;
					return tab.Item;
				}
				catch
				{
					return null;
				}
			}
			set
			{
				foreach(PanelBar panel in m_Panels)
				{
					foreach(DockItemTab tab in panel.Tabs)
					{
						if(tab.Item==value)
						{
							PanelBar activePanel=ActivePanelBar;
							if(activePanel!=null)
							{
								if(activePanel!=panel)
								{
									if(activePanel.BoundBar.IsSizingWindow)
										return;
									if(!AnimateHide(activePanel.BoundBar))
                                        return;
								}
								else
								{
									if(panel.Tabs[panel.ActiveTab]==tab && panel.BoundBar.Visible)
										return;
									((DockItemTab)activePanel.Tabs[activePanel.ActiveTab]).Item.Displayed=false;
								}
							}
							panel.ActiveTab=panel.Tabs.IndexOf(tab);
							this.Refresh();
							return;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets whether bars on auto-hide panel are displayed when mouse hovers over the tab.
		/// </summary>
		[DefaultValue(true),Browsable(true),Description("Indicates whether bars on auto-hide panel are displayed when mouse hovers over the tab.")]
		public bool EnableHoverExpand
		{
			get {return m_EnableHoverExpand;}
			set {m_EnableHoverExpand=value;}
		}

		/// <summary>
		/// Gets or sets whether bars that have focus are collapsed automatically or not.
		/// </summary>
		[DefaultValue(true),Browsable(true),Description("Indicates whether bars that have focus are collapsed automatically or not.")]
		public bool EnableFocusCollapse
		{
			get {return m_EnableFocusCollapse;}
			set {m_EnableFocusCollapse=value;}
		}

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            RefreshPanels();
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            Application.DoEvents();
            this.Font = SystemInformation.MenuFont.Clone() as Font;
        }

        private void RefreshPanels()
        {
            foreach (PanelBar panelbar in m_Panels)
                panelbar.ReloadDockItems();
            this.Invalidate();
        }

		internal void ShowBar(Bar bar)
		{
			PanelBar panel=null;
			foreach(PanelBar panelbar in m_Panels)
			{
				if(panelbar.BoundBar==bar)
				{
					panel=panelbar;
					break;
				}
			}
			PanelBar activePanel=ActivePanelBar;
			if(activePanel==panel)
				return;

			if(activePanel!=null)
			{
				if(activePanel.BoundBar.IsSizingWindow)
					return;
                if (!AnimateHide(activePanel.BoundBar))
                {
                    CreateTimer();
                    return;
                }
			}
			panel.BoundBar.SelectedDockTab=panel.ActiveTab;
			AnimateShow(panel.BoundBar);
			this.Refresh();
			CreateTimer();
		}
		internal void HideBar(Bar bar)
		{
			PanelBar activePanel=ActivePanelBar;
			if(activePanel.BoundBar!=bar)
				return;
			if(activePanel.BoundBar.Visible)
			{
                if (AnimateHide(activePanel.BoundBar))
                    DestroyTimer();
			}
		}

		private bool AnimateHide(Bar bar)
		{
			return bar.AnimateHide();
		}

		private bool AnimateShow(Bar bar)
		{
			if(bar.Enabled)
				return bar.AnimateShow();
            return false;
		}

		private PanelBar ActivePanelBar
		{
			get
			{
				foreach(PanelBar panel in m_Panels)
				{
					if(panel.ActiveTab>=0 && panel.BoundBar.Visible)
						return panel;
				}
				return null;
			}
		}

		/// <summary>
		/// Sets bars position on the auto-hide panel.
		/// </summary>
		/// <param name="bar">Bar for which position should be changed.</param>
		/// <param name="iIndex">New indexed position of the bar.</param>
		public void SetBarPosition(Bar bar, int iIndex)
		{
			if(iIndex<m_Panels.Count)
			{
				for(int i=0;i<m_Panels.Count;i++)
				{
					if(((PanelBar)m_Panels[i]).BoundBar==bar)
					{
						if(i!=iIndex)
						{
							PanelBar panel=(PanelBar)m_Panels[i];
							m_Panels.RemoveAt(i);
							m_Panels.Insert(iIndex,panel);
							this.Refresh();
						}
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the style of auto-hide panel.
		/// </summary>
		public eDotNetBarStyle Style
		{
			get {return m_Style;}
			set
			{
				m_Style=value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the ColorScheme object used by this panel. Default value is null which means that ColorScheme is
		/// automatically created as specified by Style property. Note that if your DotNetBarManager has UseGlobalColorScheme set to true
		/// ColorScheme from DotNetBarManager will be used.
		/// </summary>
		[Browsable(false),DefaultValue(null)]
		public ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set {m_ColorScheme=value;}
		}

		public void AddBar(Bar bar)
		{
			if(bar.Style==eDotNetBarStyle.Office2003)
				m_Style=eDotNetBarStyle.Office2003;
			else if(bar.Style==eDotNetBarStyle.VS2005)
				m_Style=eDotNetBarStyle.VS2005;
            else if (bar.Style == eDotNetBarStyle.Office2007)
                m_Style = eDotNetBarStyle.Office2007;
            else if (bar.Style == eDotNetBarStyle.Office2010)
                m_Style = eDotNetBarStyle.Office2010;
            else if (bar.Style == eDotNetBarStyle.Windows7)
                m_Style = eDotNetBarStyle.Windows7;
            else if (bar.Style == eDotNetBarStyle.StyleManagerControlled)
                m_Style = bar.ItemsContainer.EffectiveStyle;
			else
				m_Style=eDotNetBarStyle.OfficeXP;

			PanelBar panel=new PanelBar(this,bar);
			m_Panels.Add(panel);
			if(this.Dock==DockStyle.Right || this.Dock==DockStyle.Left)
			{
				if(panel.PanelSize.Height>this.Width)
					this.Width=panel.PanelSize.Height;
			}
			else
			{
				if(panel.PanelSize.Height>this.Height)
					this.Height=panel.PanelSize.Height;
			}

			if(!this.Visible)
			{
				BarFunctions.SetControlVisible(this,true);
			}
			else
				this.Refresh();
			this.Parent.PerformLayout();
			this.Parent.Update();
		}

		public void RemoveBar(Bar bar)
		{
			foreach(PanelBar panel in m_Panels)
			{
				if(panel.BoundBar==bar)
				{
					m_Panels.Remove(panel);
					panel.Dispose();
					if(m_Panels.Count==0)
					{
						this.Visible=false;
						if(this.Dock==DockStyle.Right || this.Dock==DockStyle.Left)
						{
							this.Width=0;
						}
						else
						{
							this.Height=0;
						}
					}
					else
						this.Refresh();
					break;
				}
			}
			if(m_Panels.Count==0)
				DestroyTimer();
		}

		public void RefreshBar(Bar bar)
		{
			foreach(PanelBar panel in m_Panels)
			{
				if(panel.BoundBar==bar)
				{
					panel.ReloadDockItems();
					break;
				}
			}
			this.Refresh();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			DestroyTimer();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			PanelBar panel=this.ActivePanelBar;
			if(panel!=null)
			{
				if(panel.BoundBar.Visible)
					panel.BoundBar.SetAutoHideSize();
			}
		}

		#region DockItemTab
		private class DockItemTab:IDisposable
		{
			public Rectangle DisplayRectangle=new Rectangle(0,0,0,0);
			public DockContainerItem Item=null;
			public Size TextSize=Size.Empty;
			public Size IconSize=Size.Empty;
			private string m_Text="";
			private PanelBar m_Parent=null;
			public DockItemTab(PanelBar parent, DockContainerItem item)
			{
				this.Item=item;
				m_Parent=parent;
				item.TextChanged+=new EventHandler(this.ItemTextChanged);
				CompositeImage image=this.Icon;
                if(image!=null)
					IconSize=image.Size;
			}
			public DockItemTab(PanelBar parent, string text)
			{
				m_Text=text;
				m_Parent=parent;
			}
			public string Text
			{
				get
				{
					if(this.Item==null)
						return m_Text;
					else
						return this.Item.Text;
				}
			}
			public CompositeImage Icon
			{
				get
				{
					if(Item==null)
						return null;
					if(Item.Image!=null)
					{
						return new CompositeImage(Item.Image,false);
					}
					if(Item.ImageIndex>=0 && Item.ImageList!=null)
                        return new CompositeImage(Item.ImageList.Images[Item.ImageIndex],false);
					if(Item.Icon!=null)
						return new CompositeImage(Item.Icon,false);
					return null;
				}
			}
			private void ItemTextChanged(object sender, EventArgs e)
			{
				Control container=((BaseItem)sender).ContainerControl as Control;
				if(container!=null)
				{
                    Graphics g = BarFunctions.CreateGraphics(container);
					try
					{
						TextSize=TextDrawing.MeasureStringLegacy(g,((BaseItem)sender).Text,m_Parent.Parent.Font,Size.Empty,eTextFormat.Default);
                        TextSize.Width += 4;
					}
					finally
					{
						g.Dispose();
					}
					CompositeImage image=this.Icon;
					if(image!=null)
						IconSize=image.Size;
					else
						IconSize=Size.Empty;
				}
				m_Parent.Refresh();
			}
			public void Dispose()
			{
				if(this.Item!=null)
					this.Item.TextChanged-=new EventHandler(this.ItemTextChanged);
			}
		}
		#endregion

		#region PanelBar
		private class PanelBar:IDisposable
		{
			public ArrayList Tabs=new ArrayList(10);
			public int ActiveTab=-1;
			public Bar BoundBar=null;
			private AutoHidePanel m_Parent=null;
			public static int TabPadding=4;
			public static int TabTextVPadding=4;
			public static int TabIconVPadding=3;
			private Size m_PanelSize=Size.Empty;
			private int m_MaxTextWidth=0;

			public PanelBar(AutoHidePanel parent, Bar bar)
			{
				m_Parent=parent;
				BoundBar=bar;
				ReloadDockItems();
			}
			public void ReloadDockItems()
			{
				foreach(DockItemTab tab in Tabs)
					tab.Dispose();
				Tabs.Clear();

                Graphics g = BarFunctions.CreateGraphics(BoundBar);
				try
				{
					if(BoundBar.LayoutType==eLayoutType.DockContainer)
					{
						foreach(BaseItem item in BoundBar.Items)
						{
							DockContainerItem dockItem=item as DockContainerItem;
							if(dockItem!=null && dockItem.Visible)
							{
								DockItemTab tab=new DockItemTab(this,dockItem);
								this.Tabs.Add(tab);
                                tab.TextSize = TextDrawing.MeasureStringLegacy(g, dockItem.Text, m_Parent.Font, Size.Empty, eTextFormat.Default);
                                tab.TextSize.Width += 4;
							}
						}
					}
					if(this.Tabs.Count==0)
					{
						DockItemTab tab=new DockItemTab(this,BoundBar.Text);
						this.Tabs.Add(tab);
                        tab.TextSize = TextDrawing.MeasureStringLegacy(g, BoundBar.Text, m_Parent.Font, Size.Empty, eTextFormat.Default);
                        tab.TextSize.Width += 4;
					}
				}
				finally
				{
					g.Dispose();
				}
				m_PanelSize=CalcPanelSize();
				if(BoundBar.SelectedDockTab>=0)
					this.ActiveTab=BoundBar.SelectedDockTab;
				else
					this.ActiveTab=0;
			}
			public void Refresh()
			{
				m_PanelSize=CalcPanelSize();
				m_Parent.Refresh();
			}
			public void Dispose()
			{
				foreach(DockItemTab tab in Tabs)
					tab.Dispose();
				Tabs.Clear();
				BoundBar=null;
				m_Parent=null;
			}

			public AutoHidePanel Parent
			{
				get {return m_Parent;}
			}

			public Size PanelSize
			{
				get
				{
					return m_PanelSize;
				}
			}

			public int MaxTextWidth
			{
				get {return m_MaxTextWidth;}
			}

			private Size CalcPanelSize()
			{
				Size size=Size.Empty;
				Size textSize=Size.Empty;

				bool bHorizontal=true;

				foreach(DockItemTab tab in Tabs)
				{
					if(!tab.IconSize.IsEmpty)
					{
						if(bHorizontal)
						{
							size.Width+=(tab.IconSize.Width+PanelBar.TabPadding*2);
							size.Height=tab.IconSize.Height+TabIconVPadding*2;
						}
						else
						{
							size.Height+=(tab.IconSize.Height+PanelBar.TabPadding*2);
							size.Width=tab.IconSize.Width+TabIconVPadding*2;
						}
					}
					if(bHorizontal)
					{
						if(tab.TextSize.Width>textSize.Width)
							textSize.Width=tab.TextSize.Width;
						if(tab.TextSize.Height>textSize.Height)
							textSize.Height=tab.TextSize.Height;
					}
					else
					{
						if(tab.TextSize.Width>textSize.Height)
							textSize.Height=tab.TextSize.Width;
						if(tab.TextSize.Height>textSize.Width)
							textSize.Width=tab.TextSize.Height;
					}
				}
				if(bHorizontal)
				{
					m_MaxTextWidth=textSize.Width;
					textSize.Width+=PanelBar.TabPadding;
					textSize.Height+=(PanelBar.TabTextVPadding*2);
					size=new Size(size.Width+textSize.Width,(size.Height>textSize.Height?size.Height:textSize.Height));
				}
				else
				{
					m_MaxTextWidth=textSize.Height;
					textSize.Height+=PanelBar.TabPadding;
					textSize.Width+=(PanelBar.TabTextVPadding*2);
					size=new Size((size.Width>textSize.Height?size.Width:textSize.Height),size.Height+textSize.Width);
				}

				return size;
			}
		}
		#endregion

		#region VS2005TabDisplay
		private class VS2005TabDisplay
		{
			private AutoHidePanel m_AutoHidePanel=null;
			private ColorScheme m_ColorScheme=null;
			private int m_xTabOffset=1;
            private bool m_DisplayTextForActiveTabOnly = false;
			/// <summary>
			/// Creates new instance of the class.
			/// </summary>
			public VS2005TabDisplay(AutoHidePanel autoHidePanel, ColorScheme colorScheme)
			{
				m_AutoHidePanel=autoHidePanel;
				m_ColorScheme=colorScheme;
			}

            public bool DisplayTextForActiveTabOnly
            {
                get { return m_DisplayTextForActiveTabOnly; }
                set { m_DisplayTextForActiveTabOnly = value; }
            }

			public void Paint(Graphics g, ArrayList m_Panels)
			{
				ArrayList activeTabs=new ArrayList();
				Point p=new Point(0,0);

                if (m_AutoHidePanel.Dock == DockStyle.Left || m_AutoHidePanel.Dock == DockStyle.Right)
                {
                    p.Y = 2;
                    p.X = 1;
                }
                else
                    p.X = 2;

				foreach(PanelBar panel in m_Panels)
				{
					for(int i=0;i<panel.Tabs.Count;i++)
					{
                        bool displayText = true;
                        if (m_DisplayTextForActiveTabOnly && panel.ActiveTab != i && !panel.BoundBar.AutoHideTabTextAlwaysVisible)
                            displayText=false;
						DockItemTab tab=panel.Tabs[i] as DockItemTab;
						if(m_AutoHidePanel.Dock==DockStyle.Left || m_AutoHidePanel.Dock==DockStyle.Right)
						{
                            tab.DisplayRectangle = new Rectangle(p.X, p.Y, m_AutoHidePanel.Width - 2, tab.IconSize.Width + 2 +
                                (displayText || tab.IconSize.Width==0 ? tab.TextSize.Width : 4));
							p.Y+=(tab.DisplayRectangle.Height+m_xTabOffset);
						}
						else
						{
                            tab.DisplayRectangle = new Rectangle(p.X, p.Y, tab.IconSize.Width + 2 +
                                (displayText || tab.IconSize.Width == 0 ? tab.TextSize.Width : 4), m_AutoHidePanel.Height);
							p.X+=(tab.DisplayRectangle.Width+m_xTabOffset);
						}
						
						if(panel.ActiveTab==i)
							activeTabs.Add(tab);
						else
							PaintTab(g,tab,false);
					}
					if(m_AutoHidePanel.Dock==DockStyle.Left || m_AutoHidePanel.Dock==DockStyle.Right)
						p.Y+=2;
					else
						p.X+=2;
				}

                foreach (DockItemTab tab in activeTabs)
                    PaintTab(g, tab, true);
			}

			protected virtual void PaintTab(Graphics g, DockItemTab tab, bool selected)
			{
				GraphicsPath path=GetTabItemPath(tab);
				TabColors colors=GetTabColors(tab,selected);

				DrawTabItemBackground(tab,path,colors,g);
				
				DrawTabText(tab,colors,g,selected);
			}

			private TabColors GetTabColors(DockItemTab tab, bool selected)
			{
				TabColors c=new TabColors();
                if (tab.Item != null && BarFunctions.IsOffice2007Style(tab.Item.EffectiveStyle))
                {
                    if (tab.Item.PredefinedTabColor == eTabItemColor.Default)
                    {
                        if (selected && !m_ColorScheme.AutoHideSelectedTabBackground.IsEmpty)
                        {
                            c.BackColor = m_ColorScheme.AutoHideSelectedTabBackground;
                            c.BackColor2 = m_ColorScheme.AutoHideSelectedTabBackground2;
                            c.BackColorGradientAngle = m_ColorScheme.AutoHideTabBackgroundGradientAngle;
                            c.TextColor = m_ColorScheme.AutoHideSelectedTabText;
                            c.BorderColor = m_ColorScheme.AutoHideSelectedTabBorder;
                        }
                        else if (!m_ColorScheme.AutoHideTabBackground.IsEmpty)
                        {
                            c.BackColor = m_ColorScheme.AutoHideTabBackground;
                            c.BackColor2 = m_ColorScheme.AutoHideTabBackground2;
                            c.BackColorGradientAngle = m_ColorScheme.AutoHideTabBackgroundGradientAngle;
                            c.TextColor = m_ColorScheme.AutoHideTabText;
                            c.BorderColor = m_ColorScheme.AutoHideTabBorder;
                        }
                        else
                        {
                            c.BackColor = m_ColorScheme.BarBackground;
                            c.BackColor2 = m_ColorScheme.BarBackground2;
                            c.BackColorGradientAngle = m_ColorScheme.BarBackgroundGradientAngle;
                            c.TextColor = m_ColorScheme.ItemText;
                            c.BorderColor = m_ColorScheme.BarDockedBorder;
                        }
                    }
                    else
                    {
                        Color c1, c2;
                        TabColorScheme.GetPredefinedColors(tab.Item.PredefinedTabColor, out c1, out c2);
                        c.BackColor = c1;
                        c.BackColor2 = c2;
                        c.BackColorGradientAngle = 90;
                        c.TextColor = m_ColorScheme.ItemText;
                    }

                    if (m_AutoHidePanel != null && (m_AutoHidePanel.Dock == DockStyle.Left || m_AutoHidePanel.Dock == DockStyle.Right))
                    {
                        c.BackColorGradientAngle -= 90;
                    }
                }
                else
                {
                    c.BackColor = m_ColorScheme.DockSiteBackColor;
                    c.BackColor2 = m_ColorScheme.DockSiteBackColor;
                    c.TextColor = SystemColors.ControlText;
                    c.BorderColor = SystemColors.ControlDarkDark;
                }

				return c;
			}

			protected virtual void DrawTabItemBackground(DockItemTab tab, GraphicsPath path, TabColors colors, Graphics g)
			{
				RectangleF rf=path.GetBounds();
				Rectangle tabRect=new Rectangle((int)rf.X, (int)rf.Y, (int)rf.Width, (int)rf.Height);

				if(colors.BackColor2.IsEmpty)
				{
					if(!colors.BackColor.IsEmpty)
					{
						using(SolidBrush brush=new SolidBrush(colors.BackColor))
							g.FillPath(brush,path);
					}
				}
				else
				{
					using(SolidBrush brush=new SolidBrush(Color.White))
						g.FillPath(brush,path);
					using(LinearGradientBrush brush=CreateTabGradientBrush(tabRect,colors.BackColor,colors.BackColor2,colors.BackColorGradientAngle))
						g.FillPath(brush,path);
				}

				if(!colors.BorderColor.IsEmpty)
				{
					using(Pen pen=new Pen(colors.BorderColor,1))
						g.DrawPath(pen,path);
				}
			}

			protected virtual void DrawTabText(DockItemTab tab, TabColors colors, Graphics g, bool selected)
			{
				int MIN_TEXT_WIDTH=12;
                eTextFormat strFormat = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter
                | eTextFormat.EndEllipsis | eTextFormat.SingleLine;

				Rectangle rText=tab.DisplayRectangle;
				// Draw image
				CompositeImage image=tab.Icon;
				if(image!=null && image.Width+4<=rText.Width)
				{
					if(m_AutoHidePanel.Dock==DockStyle.Top || m_AutoHidePanel.Dock==DockStyle.Bottom)
					{
						image.DrawImage(g,new Rectangle(rText.X+3,rText.Y+(rText.Height-image.Height)/2,image.Width,image.Height));
						int offset=image.Width+2;
						rText.X+=offset;
						rText.Width-=offset;
					}
					else
					{
						image.DrawImage(g,new Rectangle(rText.X+(rText.Width-image.Width)/2,rText.Y+3,image.Width,image.Height));
						int offset=image.Height+2;
						rText.Y+=offset;
						rText.Height-=offset;
					}
				}

                bool drawTextAlways = false;
                if (tab.Item != null && tab.Item.ContainerControl is Bar)
                    drawTextAlways = ((Bar)tab.Item.ContainerControl).AutoHideTabTextAlwaysVisible;
                if (m_DisplayTextForActiveTabOnly && !drawTextAlways && !selected && image != null)
                    return;

				// Draw text
				//if(selected)
				{
					Font font=m_AutoHidePanel.Font;
                    rText.Inflate(-1, -1);
                    
					if((m_AutoHidePanel.Dock==DockStyle.Left || m_AutoHidePanel.Dock==DockStyle.Right))
					{
                        rText.Y += 2;
						g.RotateTransform(90);
						rText=new Rectangle(rText.Top,-rText.Right,rText.Height,rText.Width);
					}

					if(rText.Width>MIN_TEXT_WIDTH)
					{
                        TextDrawing.DrawStringLegacy(g, tab.Text, font, colors.TextColor, rText, strFormat);
					}

					if((m_AutoHidePanel.Dock==DockStyle.Left || m_AutoHidePanel.Dock==DockStyle.Right))
						g.ResetTransform();
				}
			}

			protected virtual LinearGradientBrush CreateTabGradientBrush(Rectangle r,Color color1,Color color2,int gradientAngle)
			{
				if(r.Width<=0)
					r.Width=1;
				if(r.Height<=0)
					r.Height=1;
				LinearGradientBrush brush=new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle);
				return brush;
			}

			protected GraphicsPath GetTabItemPath(DockItemTab tab)
			{
				Rectangle r=tab.DisplayRectangle;
				if(m_AutoHidePanel.Dock==DockStyle.Left)
					r=new Rectangle(r.X-(r.Height-r.Width),r.Y,r.Height,r.Width);
				else if(m_AutoHidePanel.Dock==DockStyle.Right)
					r=new Rectangle(r.X,r.Y,r.Height,r.Width);

				r.Offset(0,1);

				GraphicsPath path=new GraphicsPath();
                int cornerSize = 2;
				// Left line
				//path.AddPath(GetLeftLine(r),true);
                path.AddLine(r.X, r.Bottom, r.X, r.Y + cornerSize);

				// Top line
				path.AddLine(r.X+cornerSize,r.Y,r.Right-cornerSize,r.Y);

				// Right line
				//path.AddPath(GetRightLine(r),true);
                path.AddLine(r.Right, r.Y+cornerSize, r.Right, r.Bottom);

				// Bottom line
				//path.AddLine(r.Right+m_xTabOffset,r.Bottom,r.X-m_xTabOffset,r.Bottom);
                path.AddLine(r.Right , r.Bottom, r.X, r.Bottom);

				path.CloseAllFigures();

				if(m_AutoHidePanel.Dock==DockStyle.Top)
				{
					// Bottom
					Matrix m=new Matrix();
					m.RotateAt(180,new PointF(r.X+r.Width/2,r.Y+r.Height/2));
					path.Transform(m);
				}
				else if(m_AutoHidePanel.Dock==DockStyle.Right)
				{
					// Left
					Matrix m=new Matrix();
					m.RotateAt(-90,new PointF(r.X,r.Bottom));
					m.Translate(r.Height,r.Width-r.Height,MatrixOrder.Append);
					path.Transform(m);
				}
				else if(m_AutoHidePanel.Dock==DockStyle.Left)
				{
					// Right
					Matrix m=new Matrix();
					m.RotateAt(90,new PointF(r.Right,r.Bottom));
					m.Translate(-r.Height,r.Width-(r.Height-1),MatrixOrder.Append);
					path.Transform(m);
				}

				return path;
			}

			private GraphicsPath GetLeftLine(Rectangle r)
			{
				GraphicsPath path=new GraphicsPath();
				// Left line
				path.AddLine(r.X-m_xTabOffset,r.Bottom,r.X,r.Y+5);
				Point[] pc=new Point[3];
				pc[0]=new Point(r.X,r.Y+5);
				pc[1]=new Point(r.X+2,r.Y+2);
				pc[2]=new Point(r.X+5,r.Y);
				path.AddCurve(pc,.9f);
				return path;
			}

			private GraphicsPath GetRightLine(Rectangle r)
			{
				GraphicsPath path=new GraphicsPath();
				// Right line
				Point[] pc=new Point[3];
				pc[0]=new Point(r.Right-5,r.Y);
				pc[1]=new Point(r.Right-2,r.Y+2);
				pc[2]=new Point(r.Right,r.Y+5);
				path.AddCurve(pc,.9f);
				path.AddLine(r.Right,r.Y+5,r.Right+m_xTabOffset,r.Bottom);
				return path;
			}
		}
		#endregion
	}
}

