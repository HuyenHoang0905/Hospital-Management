using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Tab-Strip control.
	/// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.TabStripDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ComVisible(false)]
	public class TabStrip : Control
	{
		#region Events
		/// <summary>
		/// Event delegate for SelectedTabChanged event.
		/// </summary>
		public delegate void SelectedTabChangedEventHandler(object sender, TabStripTabChangedEventArgs e);
		/// <summary>
		/// Occurs after selected tab has changed.
		/// </summary>
		public event SelectedTabChangedEventHandler SelectedTabChanged;
		/// <summary>
		/// Event delegate for SelectedTabChanging event.
		/// </summary>
		public delegate void SelectedTabChangingEventHandler(object sender, TabStripTabChangingEventArgs e);
		/// <summary>
		/// Occurs before selected tab changes and gives you opportunity to cancel the change.
		/// </summary>
		public event SelectedTabChangingEventHandler SelectedTabChanging;
		/// <summary>
		/// Event delegate for TabMoved event
		/// </summary>
		public delegate void TabMovedEventHandler(object sender, TabStripTabMovedEventArgs e);
		/// <summary>
		/// Occurs when tab is dragged by user.
		/// </summary>
		public event TabMovedEventHandler TabMoved;
		/// <summary>
		/// Event delegate for NavigateBack, NavigateForward and TabItemClose events.
		/// </summary>
		public delegate void UserActionEventHandler(object sender, TabStripActionEventArgs e);
		/// <summary>
		/// Occurs when the user navigates back using the back arrow.
		/// </summary>
		public event UserActionEventHandler NavigateBack;
		/// <summary>
		/// Occurs when the user navigates forward using the forward arrow.
		/// </summary>
		public event UserActionEventHandler NavigateForward;
		/// <summary>
		/// Occurs when tab item is closing.
		/// </summary>
		public event UserActionEventHandler TabItemClose;
		/// <summary>
		/// Occurs when tab item is added to the tabs collection.
		/// </summary>
		public event EventHandler TabItemOpen;
		/// <summary>
		/// Occurs before control or item attached to the tab is displayed.
		/// </summary>
		public event EventHandler BeforeTabDisplay;
		/// <summary>
		/// Occurs after tab item has been removed from tabs collection.
		/// </summary>
		public event EventHandler TabRemoved;

        /// <summary>
        /// Occurs after Tabs collection has been cleared.
        /// </summary>
        public event EventHandler TabsCleared;

		internal event EventHandler SizeRecalculated;

        /// <summary>
        /// Occurs after the tab item size has been determined and allows you to apply your custom size to the TabItem.
        /// </summary>
        public event MeasureTabItemEventHandler MeasureTabItem;

        /// <summary>
        /// Occurs before tab is rendered and allows you to cancel default tab rendering performed by the control.
        /// </summary>
        public event RenderTabItemEventHandler PreRenderTabItem;

        /// <summary>
        /// Occurs after tab is rendered and allows you to render on top of the default rendering performed by the control.
        /// </summary>
        public event RenderTabItemEventHandler PostRenderTabItem;
		#endregion

		#region Private variables
		private eTabStripAlignment m_Alignment=eTabStripAlignment.Bottom;
		private TabsCollection m_Tabs=null;
		private ImageList m_ImageList=null;
		private TabItem m_SelectedTab=null;
		private bool m_NeedRecalcSize=true;
		private bool m_TabDrag=false;
		private Bar m_DragBar=null;
		private bool m_IsDragging=false;
		private bool m_CanReorderTabs=true;

		private bool m_VariableTabWidth=true;
		private int m_ScrollOffset=0;
		private TabSystemBox m_TabSystemBox=null;

		private bool m_Animate=true;

		private bool m_MdiTabbedDocuments=false;
		private Form m_MdiForm=null;
		private bool m_MdiInitialized=false;
		private int m_MaxMdiCaptionLength=32;
		private bool m_ShowMdiChildIcon=true;
		private bool m_MdiAutoHide=true;
		private bool m_MdiNoFormActivateFlicker=true;

		private Font m_SelectedTabFont=null;
		private bool m_SelectedTabFontCustom=false;

		private bool m_DisplaySelectedTextOnly=false;

		private ThemeTab m_ThemeTab=null;

		private TabItem m_HotTab=null;

		private eTabStripStyle m_Style=eTabStripStyle.Flat;

//		// New Color Styling
//		private Color m_BackColor2=Color.Empty;
//
//		// Selected Tab Colors...
//		private Color m_SelectedTabBackColor=Color.Empty;
//		private Color m_SelectedTabBackColor2=Color.Empty;
//		private Color m_SelectedTabBorderLightColor=Color.Empty;
//		private Color m_SelectedTabBorderDarkColor=Color.Empty;
		private Color m_SelectedTabTextColor=Color.Empty;

//		// Hot Tab Colors
//		private Color m_HotBackColor=Color.Empty;
//		private Color m_HotBackColor2=Color.Empty;
		private Color m_HotTextColor=Color.Empty;
//		private Color m_HotBorderLightColor=Color.Empty;
//		private Color m_HotBorderDarkColor=Color.Empty;

		// Separator Colors
		private Color m_SeparatorColor=Color.Empty;
		private Color m_SeparatorShadeColor=Color.Empty;

		// Auto-click repeat on system items
		private bool m_TabScrollAutoRepeat=false;
		private int m_TabScrollRepeatInterval=300;

		private TabColorScheme m_ColorScheme=null;
		private TabItem m_DesignTimeSelection=null;

		private bool m_ShowFocusRectangle=true;
		private bool m_AutoHideSystemBox=false;
		private int MIN_TEXT_WIDTH=4;

		//private bool m_MultiLine=false;
		private int m_MultiLineSpacing=0;

		private eTabLayoutType m_TabLayoutType=eTabLayoutType.FitContainer;
		private ToolTip m_ToolTip=null;
		private TabItem m_TooltipTab=null;

		private bool m_ThemeAware=false;
		private Rectangle m_TabItemsBounds=Rectangle.Empty;

        private Size m_FixedTabSize = Size.Empty;
        private bool m_CloseButtonOnTabs = false;
        private eTabCloseButtonPosition m_CloseButtonPosition = eTabCloseButtonPosition.Left;
        private static Size default_close_button_size = new Size(11, 11);
        private Size m_CloseButtonSize = default_close_button_size;

        private Image m_TabCloseButtonNormal = null;
        private Image m_TabCloseButtonHot = null;
        private bool m_CloseButtonOnTabsAlwaysDisplayed = true;
        private bool m_AntiAlias = false;
        private bool m_AutoSelectAttachedControl = true;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TabStrip()
		{
			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}
			m_Tabs=new TabsCollection(this);
			this.SetStyle(ControlStyles.Selectable,true);
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);

			m_TabSystemBox=new TabSystemBox(this);
			m_TabSystemBox.Back+=new EventHandler(this.OnTabBack);
			m_TabSystemBox.Close+=new EventHandler(this.OnTabClose);
			m_TabSystemBox.Forward+=new EventHandler(this.OnTabForward);

			m_ColorScheme=new TabColorScheme(m_Style);
			m_ColorScheme.ColorChanged+=new EventHandler(this.ColorSchemeChanged);

            StyleManager.Register(this);
		}

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

		protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Width == 0 || this.Height == 0)
                return;

			m_ScrollOffset=0;
			this.RecalcSize();
			this.EnsureVisible(this.SelectedTab);
		}

		private Rectangle GetFocusRectangle(Rectangle rText)
		{
			rText.Inflate(-1,-1);
			return rText;
		}

		internal void PaintTabSystemBox(Graphics g)
		{
			if(this.HasNavigationBox && m_TabSystemBox.Visible)
			{
				m_TabSystemBox.Paint(g);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
            SmoothingMode sm = e.Graphics.SmoothingMode;
            TextRenderingHint th = e.Graphics.TextRenderingHint;
            
            if (m_AntiAlias)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#if FRAMEWORK20
                if (!SystemInformation.IsFontSmoothingEnabled)
#endif
                    e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            if (this.BackColor == Color.Transparent || this.ColorScheme.TabBackground == Color.Transparent || this.BackgroundImage != null)
            {
                base.OnPaintBackground(e);
            }

			if(m_Style==eTabStripStyle.SimulatedTheme)
			{
				if(m_NeedRecalcSize)
                    this.RecalcSize(e.Graphics, GetTabClientArea(this.DisplayRectangle, (this.TabLayoutType == eTabLayoutType.MultilineWithNavigationBox), false));
				TabStripSimulatedThemeDisplay display=new TabStripSimulatedThemeDisplay();
                display.CloseButtonOnTabs = m_CloseButtonOnTabs;
				display.Paint(e.Graphics,this);
			}
			else if(this.IsThemed)
				PaintThemed(e.Graphics);
            else if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
				PaintOneNote(e.Graphics);
			else if(m_Style==eTabStripStyle.VS2005)
				PaintVS2005(e.Graphics);
			else if(m_Style==eTabStripStyle.RoundHeader)
			{
				if(m_NeedRecalcSize)
                    this.RecalcSize(e.Graphics, GetTabClientArea(this.DisplayRectangle, (this.TabLayoutType == eTabLayoutType.MultilineWithNavigationBox), false));
				TabStripRoundHeaderDisplay display=new TabStripRoundHeaderDisplay();
                display.CloseButtonOnTabs = m_CloseButtonOnTabs;
				display.Paint(e.Graphics,this);
			}
            else if (m_Style == eTabStripStyle.Office2007Dock)
            {
                if (m_NeedRecalcSize)
                    this.RecalcSize(e.Graphics, GetTabClientArea(this.DisplayRectangle,(this.TabLayoutType == eTabLayoutType.MultilineWithNavigationBox), false));
                TabStripOffice2007DockDisplay display = new TabStripOffice2007DockDisplay();
                display.CloseButtonOnTabs = m_CloseButtonOnTabs;
                display.Paint(e.Graphics, this);
            }
            else if (m_Style == eTabStripStyle.VS2005Dock)
            {
                if (m_NeedRecalcSize)
                    this.RecalcSize(e.Graphics, GetTabClientArea(this.DisplayRectangle, (this.TabLayoutType == eTabLayoutType.MultilineWithNavigationBox), false));
                TabStripVS2005DockDisplay display = new TabStripVS2005DockDisplay();
                display.CloseButtonOnTabs = m_CloseButtonOnTabs;
                display.Paint(e.Graphics, this);
            }
            else if (m_Style == eTabStripStyle.Office2007Document)
            {
                if (m_NeedRecalcSize)
                    this.RecalcSize(e.Graphics, GetTabClientArea(this.DisplayRectangle, (this.TabLayoutType == eTabLayoutType.MultilineWithNavigationBox), false));
                TabStripOffice2007DocumentDisplay display = new TabStripOffice2007DocumentDisplay();
                display.CloseButtonOnTabs = m_CloseButtonOnTabs;
                display.Paint(e.Graphics, this);
            }
            else
                PaintFlat(e.Graphics);

            e.Graphics.SmoothingMode = sm;
            e.Graphics.TextRenderingHint = th;
		}

		private void PaintFlat(Graphics g)
		{
			TabColorScheme colorScheme=m_ColorScheme;
//			if(m_Style==eTabStripStyle.Themed)
//				colorScheme=new TabColorScheme(eTabStripStyle.Flat);

			if(!colorScheme.TabBackground2.IsEmpty && this.Height>0 && this.Width>0)
			{
				using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(this.ClientRectangle,colorScheme.TabBackground,colorScheme.TabBackground2,colorScheme.TabBackgroundGradientAngle))
					g.FillRectangle(gradient,this.ClientRectangle);
				
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(colorScheme.TabBackground))
					g.FillRectangle(brush,this.DisplayRectangle);
				//g.Clear(colorScheme.TabBackground);
			}

			if(!colorScheme.TabBorder.IsEmpty)
			{
				using(Pen pen=new Pen(colorScheme.TabBorder,1))
					g.DrawRectangle(pen,new Rectangle(ClientRectangle.X,ClientRectangle.Y,ClientRectangle.Width-1,ClientRectangle.Height-1));
			}

			// Fill tab background color
			Rectangle r=this.DisplayRectangle;

			switch(m_Alignment)
			{
				case eTabStripAlignment.Bottom:
				{
					r.Height-=3;
					break;
				}
				case eTabStripAlignment.Top:
				{
					r.Y+=1;
					r.Height-=3;
					break;
				}
			}

			// See how we will draw items
			TabItem selected=this.SelectedTab;
            eTextFormat strFormat = eTextFormat.Default | eTextFormat.SingleLine | eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter |
                eTextFormat.VerticalCenter;

			int outerBorderAdjustment=(colorScheme.TabBorder.IsEmpty?0:1);
			// Draw the upper back line
			switch(m_Alignment)
			{
				case eTabStripAlignment.Top:
				{
					if(!(this.Parent is TabControl))
						g.FillRectangle(new SolidBrush((colorScheme.TabItemSelectedBackground2.IsEmpty?colorScheme.TabItemSelectedBackground:colorScheme.TabItemSelectedBackground2)),r.X+outerBorderAdjustment,r.Bottom,r.Width-outerBorderAdjustment*2,this.Height-r.Bottom);
					else
					{
						using(Pen pen=new Pen(colorScheme.TabItemSelectedBorder,1))
						{
							g.DrawLine(pen,r.X,r.Bottom,r.X,ClientRectangle.Bottom);
							g.DrawLine(pen,r.Right-1,r.Bottom,r.Right-1,ClientRectangle.Bottom);
						}
					}
					g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight,1),r.X+outerBorderAdjustment,r.Bottom,r.Right-1-outerBorderAdjustment,r.Bottom);
					r.Inflate(-4,0);
					break;
				}
				case eTabStripAlignment.Left:
				{
					g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight,1),r.Right-1,r.Y+outerBorderAdjustment,r.Right-1,r.Bottom-outerBorderAdjustment);
					r.Inflate(0,-4);
					break;
				}
				case eTabStripAlignment.Right:
				{
					g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight,1),r.X,r.Y+outerBorderAdjustment,r.X,r.Bottom-outerBorderAdjustment);
					r.Inflate(0,-4);
					break;
				}
				default:
				{
					if(!(this.Parent is TabControl))
						g.FillRectangle(new SolidBrush(colorScheme.TabItemSelectedBackground),r.X,0,r.Width,r.Y);
					else
					{
						using(Pen pen=new Pen(colorScheme.TabItemSelectedBorder,1))
						{
							g.DrawLine(pen,r.X,r.Y,r.X,0);
							g.DrawLine(pen,r.Right-1,r.Y,r.Right-1,0);
						}
					}
					g.DrawLine(new Pen(colorScheme.TabItemSelectedBorderLight,1),r.X,r.Y,r.Right,r.Y);
					r.Inflate(-4,0);
					break;
				}
			}

            if (this.HasNavigationBox && m_TabSystemBox.Visible)
			{
				if(m_Alignment==eTabStripAlignment.Right || m_Alignment==eTabStripAlignment.Left)
				{
					if(m_TabSystemBox.DisplayRectangle.Height>0)
						r.Height-=m_TabSystemBox.DisplayRectangle.Height;
					else
                        r.Height-=m_TabSystemBox.DefaultWidth;
				}
				else
				{
					if(m_TabSystemBox.DisplayRectangle.Width>0)
						r.Width-=m_TabSystemBox.DisplayRectangle.Width;
					else
                        r.Width-=m_TabSystemBox.DefaultWidth;
                    if(this.IsRightToLeft)
                        r.X += m_TabSystemBox.DefaultWidth;
				}
			}
			if(m_NeedRecalcSize)
				RecalcSize(g,this.GetTabClientArea(this.DisplayRectangle,false,false));
			if(m_Alignment==eTabStripAlignment.Top)
				r.Height++;
			Rectangle rClip=r;
			g.SetClip(r);

			foreach(TabItem tab in m_Tabs)
			{
				if(!tab.Visible || !r.IntersectsWith(tab.DisplayRectangle))
					continue;
                if (HasPreRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePreRenderTabItem(re);
                    if (re.Cancel) continue;
                }

				Rectangle tabRect=tab.DisplayRectangle;
				TabColors tabColors=GetTabColors(tab);
				if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
				{
					if(tab==selected)
					{
						if(m_Alignment==eTabStripAlignment.Left)
						{
							if(tabColors.BackColor2.IsEmpty)
							{
								using(SolidBrush brush=new SolidBrush(g.GetNearestColor(tabColors.BackColor)))
									g.FillRectangle(brush,tabRect);
							}
							else
							{
								using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(tabRect,tabColors.BackColor,tabColors.BackColor2,tabColors.BackColorGradientAngle))
									g.FillRectangle(gradient,tabRect);
							}
							Pen p=new Pen(tabColors.LightBorderColor,1);
							g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
							g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.X,tabRect.Bottom);
							p.Dispose();
							p=new Pen(tabColors.BorderColor,1);
							g.DrawLine(p,tabRect.X+1,tabRect.Bottom,tabRect.Right-2,tabRect.Bottom);
							p.Dispose();
						}
						else
						{
							if(tabColors.BackColor2.IsEmpty)
							{
								using(SolidBrush brush=new SolidBrush(g.GetNearestColor(tabColors.BackColor)))
									g.FillRectangle(brush,tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1);
							}
							else
							{
								using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1),tabColors.BackColor,tabColors.BackColor2,tabColors.BackColorGradientAngle))
									g.FillRectangle(gradient,tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1);
							}
							Pen p=new Pen(tabColors.LightBorderColor,1);
							g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
							g.DrawLine(p,tabRect.Right-1,tabRect.Y,tabRect.Right-1,tabRect.Bottom);
							p.Dispose();
							p=new Pen(tabColors.BorderColor,1);
							g.DrawLine(p,tabRect.X,tabRect.Bottom,tabRect.Right-3,tabRect.Bottom);
							p.Dispose();
						}
					}
					else
					{
						if(!tabColors.BackColor.IsEmpty)
						{
							if(!tabColors.BackColor2.IsEmpty)
							{
								using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(tabRect,tabColors.BackColor,tabColors.BackColor2,tabColors.BackColorGradientAngle))
									g.FillRectangle(gradient,tabRect);
							}
							else
							{
								using(SolidBrush brush=new SolidBrush(tabColors.BackColor))
									g.FillRectangle(brush,tabRect);
							}
						}

						if(!tabColors.LightBorderColor.IsEmpty)
						{
							if(m_Alignment==eTabStripAlignment.Left)
							{
								using(Pen p=new Pen(tabColors.LightBorderColor,1))
								{
									g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
									g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.X,tabRect.Bottom);
								}
							}
							else
							{
								using(Pen p=new Pen(tabColors.LightBorderColor,1))
								{
									g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
									g.DrawLine(p,tabRect.Right-1,tabRect.Y,tabRect.Right-1,tabRect.Bottom);
								}
							}
						}

						if(!tabColors.BorderColor.IsEmpty)
						{
							if(m_Alignment==eTabStripAlignment.Left)
							{
								using(Pen p=new Pen(tabColors.BorderColor,1))
								{
									g.DrawLine(p,tabRect.X+1,tabRect.Bottom,tabRect.Right-2,tabRect.Bottom);
									g.DrawLine(p,tabRect.Right-1,tabRect.Bottom,tabRect.Right-1,tabRect.Y+1);
								}
							}
							else
							{
								using(Pen p=new Pen(tabColors.BorderColor,1))
								{
									g.DrawLine(p,tabRect.X,tabRect.Y+1,tabRect.X,tabRect.Bottom);
									g.DrawLine(p,tabRect.X+1,tabRect.Bottom,tabRect.Right-2,tabRect.Bottom);
								}
							}
						}						
					}

                    if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
                        tab.CloseButtonBounds = PaintTabItemCloseButton(g, true, tab.CloseButtonMouseOver, tab == m_HotTab || tab == this.SelectedTab, ref tabRect);
                    else
                        tab.CloseButtonBounds = Rectangle.Empty;

					Image tabImage=tab.GetImage();
					Icon icon=tab.Icon;
					if(tabImage!=null && tabImage.Width+4<=tabRect.Width || icon!=null && tab.IconSize.Width+4<=tabRect.Width)
					{
						if(icon!=null)
						{
							Rectangle rIcon=new Rectangle(tabRect.X+(tabRect.Width-tab.IconSize.Width)/2,tabRect.Y+4,tab.IconSize.Width,tab.IconSize.Height);
							if(rClip.Contains(rIcon))
								g.DrawIcon(icon,rIcon);
							tabRect.Y+=(tab.IconSize.Height+2);
							tabRect.Height-=(tab.IconSize.Height+2);
						}
						else if(tabImage!=null)
						{
							g.DrawImage(tabImage,tabRect.X+(tabRect.Width-tabImage.Width)/2,tabRect.Y+4,tabImage.Width,tabImage.Height);
							tabRect.Y+=(tabImage.Height+2);
							tabRect.Height-=(tabImage.Height+2);
						}
						tabRect.Inflate(0,-1);
						tabRect.Height-=4;
						tabRect.Y+=3;
					}
					else
					{
						tabRect.Y+=2;
						tabRect.Height-=2;
					}

					g.RotateTransform(90);
					if(!m_DisplaySelectedTextOnly || tab==m_SelectedTab)
					{
						Font font=this.Font;
						if(tab==selected && m_SelectedTabFont!=null)
							font=m_SelectedTabFont;
						Rectangle rText=new Rectangle(tabRect.Top,-tabRect.Right,tabRect.Height,tabRect.Width);
						if(rText.Height>MIN_TEXT_WIDTH)
						{
							TextDrawing.DrawStringLegacy(g,tab.Text,font,tabColors.TextColor,rText,strFormat);
						}
						if(m_ShowFocusRectangle && this.Focused && tab==m_SelectedTab)
							ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(rText));
								g.ResetTransform();
					}
					// Draw separator
					if(tab!=selected)
					{
						g.DrawLine(new Pen(colorScheme.TabItemSeparator,1),tab.DisplayRectangle.X+1,tab.DisplayRectangle.Bottom,tab.DisplayRectangle.Right-4,tab.DisplayRectangle.Bottom);
						if(!colorScheme.TabItemSeparatorShade.IsEmpty)
                            g.DrawLine(new Pen(colorScheme.TabItemSeparatorShade,1),tab.DisplayRectangle.X+1+1,tab.DisplayRectangle.Bottom+1,tab.DisplayRectangle.Right-4+1,tab.DisplayRectangle.Bottom+1);
					}
				}
				else
				{
					if(tab==selected)
					{
						if(m_Alignment==eTabStripAlignment.Bottom)
						{
							if(tabColors.BackColor2.IsEmpty)
							{
								using(SolidBrush brush=new SolidBrush(g.GetNearestColor(tabColors.BackColor)))
									g.FillRectangle(brush,tabRect);
							}
							else
							{
								using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(tabRect,tabColors.BackColor,tabColors.BackColor2,tabColors.BackColorGradientAngle))
									g.FillRectangle(gradient,tabRect);
							}
							using(Pen p=new Pen(tabColors.LightBorderColor,1))
								g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.X,tabRect.Bottom);
							using(Pen p=new Pen(tabColors.BorderColor,1))
							{
								g.DrawLine(p,tabRect.X+1,tabRect.Bottom,tabRect.Right,tabRect.Bottom);
								g.DrawLine(p,tabRect.Right,tabRect.Y,tabRect.Right,tabRect.Bottom);
							}
						}
						else
						{
							if(tabColors.BackColor2.IsEmpty)
							{
								using(SolidBrush brush=new SolidBrush(g.GetNearestColor(tabColors.BackColor)))
									g.FillRectangle(brush,tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1);
							}
							else
							{
								using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1),tabColors.BackColor,tabColors.BackColor2,tabColors.BackColorGradientAngle))
									g.FillRectangle(gradient,tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1);
							}
							using(Pen p=new Pen(tabColors.LightBorderColor,1))
							{
								g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.X,tabRect.Bottom);
								g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
							}
							using(Pen p=new Pen(tabColors.BorderColor,1))
								g.DrawLine(p,tabRect.Right,tabRect.Y,tabRect.Right,tabRect.Bottom);
						}
					}
					else
					{
						if(!tabColors.BackColor.IsEmpty)
						{
							Rectangle rBack=tabRect;
							rBack.Width--;
							rBack.X++;
							rBack.Height--;
							rBack.Y++;
							if(!tabColors.BackColor2.IsEmpty)
							{
								using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(tabRect,tabColors.BackColor,tabColors.BackColor2,tabColors.BackColorGradientAngle))
									g.FillRectangle(gradient,rBack);
							}
							else
							{
								using(SolidBrush brush=new SolidBrush(tabColors.BackColor))
									g.FillRectangle(brush,rBack);
							}
						}
						if(!tabColors.LightBorderColor.IsEmpty)
						{
							if(m_Alignment==eTabStripAlignment.Bottom)
							{
								using(Pen p=new Pen(tabColors.LightBorderColor,1))
								{
									g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
									g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.X,tabRect.Bottom);
								}
							}
							else
							{
								using(Pen p=new Pen(tabColors.LightBorderColor,1))
								{
									g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.X,tabRect.Bottom);
									g.DrawLine(p,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
								}
							}
						}

						if(!tabColors.BorderColor.IsEmpty)
						{
							if(m_Alignment==eTabStripAlignment.Bottom)
							{
								using(Pen p=new Pen(tabColors.BorderColor,1))
								{
									g.DrawLine(p,tabRect.X+1,tabRect.Bottom,tabRect.Right,tabRect.Bottom);
									g.DrawLine(p,tabRect.Right,tabRect.Y,tabRect.Right,tabRect.Bottom);
								}
							}
							else
							{
								using(Pen p=new Pen(tabColors.BorderColor,1))
								{
									g.DrawLine(p,tabRect.Right-1,tabRect.Y,tabRect.Right-1,tabRect.Bottom);
									g.DrawLine(p,tabRect.X,tabRect.Bottom,tabRect.Right,tabRect.Bottom);
								}
							}
						}
					}

					if(m_Alignment==eTabStripAlignment.Top)
						tabRect.Offset(0,1);

                    if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
                        tab.CloseButtonBounds = PaintTabItemCloseButton(g, false, tab.CloseButtonMouseOver, tab == m_HotTab || tab == this.SelectedTab, ref tabRect);
                    else
                        tab.CloseButtonBounds = Rectangle.Empty;

					Image tabImage=tab.GetImage();
					Icon icon=tab.Icon;
					if(tabImage!=null && tabImage.Width+4<tabRect.Width || icon!=null && tab.IconSize.Width+4<tabRect.Width)
					{
						if(icon!=null)
						{
							Rectangle rIcon=new Rectangle(tabRect.X+4,tabRect.Y+(tabRect.Height-tab.IconSize.Height)/2,tab.IconSize.Width,tab.IconSize.Height);
							if(rClip.Contains(rIcon))
								g.DrawIcon(icon,rIcon);
							tabRect.X+=(tab.IconSize.Width+2);
							tabRect.Width-=(tab.IconSize.Width+2);
						}
						else if(tabImage!=null)
						{
							g.DrawImage(tabImage,tabRect.X+4,tabRect.Y+(tabRect.Height-tabImage.Height)/2,tabImage.Width,tabImage.Height);
							tabRect.X+=(tabImage.Width+2);
							tabRect.Width-=(tabImage.Width+2);
						}
						tabRect.Inflate(0,-1);
						tabRect.Width-=4;
						tabRect.X+=3;
					}
					else
					{
						tabRect.X+=2;
						tabRect.Width-=2;
					}

					if(!m_DisplaySelectedTextOnly || tab==m_SelectedTab)
					{
						Font font=this.Font;
						if(tab==selected && m_SelectedTabFont!=null)
							font=m_SelectedTabFont;
						if(tabRect.Width>MIN_TEXT_WIDTH)
						{
							TextDrawing.DrawString(g,tab.Text,font,tabColors.TextColor,tabRect,strFormat);
						}
						if(m_ShowFocusRectangle && this.Focused && tab==m_SelectedTab)
							ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(tabRect));
					}
	                    
					if(tab!=selected)
					{
                        Rectangle rect = tab.DisplayRectangle;
                        if (this.IsRightToLeft)
                            rect.Width -= 2;
						using(Pen p=new Pen(colorScheme.TabItemSeparator,1))
							g.DrawLine(p,rect.Right,rect.Y+2,rect.Right,rect.Bottom-4);
						if(!colorScheme.TabItemSeparatorShade.IsEmpty)
						{
							using(Pen p=new Pen(colorScheme.TabItemSeparatorShade,1))
								g.DrawLine(p,rect.Right+1,rect.Y+2+1,rect.Right+1,rect.Bottom-4+1);
						}
					}
				}

                if (HasPostRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePostRenderTabItem(re);
                }
			}
			g.ResetClip();
			if(this.HasNavigationBox && m_TabSystemBox.Visible)
			{
				g.SetClip(m_TabSystemBox.DisplayRectangle);
				m_TabSystemBox.Paint(g);
				g.ResetClip();
			}
		}

		private Rectangle GetBackgroundArea(Rectangle r)
		{
			switch(m_Alignment)
			{
				case eTabStripAlignment.Bottom:
				{
					r.Y+=3;
					r.Height-=5;
					break;
				}
				case eTabStripAlignment.Top:
				{
					r.Y+=1;
					r.Height-=5;
					break;
				}
				case eTabStripAlignment.Left:
				{
					r.Width-=5;
					r.X++;
					break;
				}
				case eTabStripAlignment.Right:
				{
					r.Width-=5;
					r.X+=3;
					break;
				}
			}
			return r;
		}

		internal Rectangle GetTabClientArea(Rectangle r, bool bExcludeSystemBox, bool bPaintArea)
		{			
			int tabItemHeight=GetSingleTabHeight();
            if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document) && !this.IsThemed)
			{
				r=GetBackgroundArea(r);
				switch(m_Alignment)
				{
					case eTabStripAlignment.Top:
					{
						r.Inflate(-4,0);
						break;
					}
					case eTabStripAlignment.Left:
					{
						r.Inflate(0,-4);
						break;
					}
					case eTabStripAlignment.Right:
					{
						r.Inflate(0,-4);
						break;
					}
					default:
					{
						r.Inflate(-4,0);
						break;
					}
				}

				if(bExcludeSystemBox && this.HasNavigationBox)
				{
					// Reduce size by System Box size
					if(m_Alignment==eTabStripAlignment.Right || m_Alignment==eTabStripAlignment.Left)
					{
						r.Height-=(m_TabSystemBox.DefaultWidth+3);
					}
					else
					{
                        if (this.IsRightToLeft)
                        {
                            r.X += (m_TabSystemBox.DefaultWidth + 3);
                            r.Width -= (m_TabSystemBox.DefaultWidth + 3);
                        }
                        else
                            r.Width -= (m_TabSystemBox.DefaultWidth + 3);
					}
				}

                if (!bPaintArea && (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document) && !this.IsThemed)
				{
					if(m_Alignment==eTabStripAlignment.Right || m_Alignment==eTabStripAlignment.Left)
					{
						if(m_Alignment==eTabStripAlignment.Right)
						{
							r.Height-=tabItemHeight;
							r.Y+=tabItemHeight;
						}
						else
						{
							r.Height-=tabItemHeight;
						}
					}
					else
					{
						//if(m_Alignment==eTabStripAlignment.Top)
						//{
							r.Width-=tabItemHeight;
							r.X+=tabItemHeight;
						//}
                        //else
                        //{
                        //    r.Width-=tabItemHeight;
                        //}
					}
				}
			}
			else
			{
				switch(m_Alignment)
				{
					case eTabStripAlignment.Bottom:
					{
						if(this.IsThemed && this.Parent is TabControl)
							r.Y+=2;
						r.Height-=3;
						break;
					}
					case eTabStripAlignment.Top:
					{
						r.Y+=1;
						r.Height-=3;
//						if(m_Style==eTabStripStyle.Themed && this.Parent is TabControl)
//							r.Offset(0,2);
						break;
					}
					case eTabStripAlignment.Left:
					{
						if(this.IsThemed && this.Parent is TabControl)
						{
							r.Width-=2;
						}
						break;
					}
					case eTabStripAlignment.Right:
					{
						if(this.IsThemed && this.Parent is TabControl)
						{
							r.X+=2;
							r.Width-=2;
						}
						break;
					}
				}

				if(m_Style!=eTabStripStyle.VS2005 && m_Style!=eTabStripStyle.Office2007Document || this.IsThemed)
				{
					switch(m_Alignment)
					{
						case eTabStripAlignment.Top:
						{
							r.Inflate(-4,0);
							break;
						}
						case eTabStripAlignment.Left:
						{
							r.Inflate(0,-4);
							break;
						}
						case eTabStripAlignment.Right:
						{
							r.Inflate(0,-4);
							break;
						}
						default:
						{
							r.Inflate(-4,0);
							break;
						}
					}
				}
//				else
//				{
//					if(m_Alignment==eTabStripAlignment.Right)
//						r.Width-=2;
//				}

				if(this.HasNavigationBox && bExcludeSystemBox)
				{
					if(m_Alignment==eTabStripAlignment.Right || m_Alignment==eTabStripAlignment.Left)
					{
						r.Height-=m_TabSystemBox.DefaultWidth;
					}
					else
					{
						r.Width-=m_TabSystemBox.DefaultWidth;
					}
				}
			}
			return r;
		}

		internal void ClipExcludeSystemBox(Graphics g)
		{
            Rectangle r = GetSystemBoxRectangle();
            if(!r.IsEmpty)
			    g.SetClip(r,CombineMode.Exclude);
		}

        /// <summary>
        /// Returns the bounds of the tab system box if one is available in current tab style.
        /// </summary>
        /// <returns>Rectangle describing the system box bounds.</returns>
        public Rectangle GetSystemBoxRectangle()
        {
            if (!m_TabSystemBox.Visible || !this.HasNavigationBox)
                return Rectangle.Empty;
            Rectangle r = Rectangle.Empty;
            if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
                r = new Rectangle(0, m_TabSystemBox.DisplayRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Bottom - m_TabSystemBox.DisplayRectangle.Y);
            else
            {
                if (this.IsRightToLeft)
                    r = m_TabSystemBox.DisplayRectangle;
                else
                    r = new Rectangle(m_TabSystemBox.DisplayRectangle.X, 0, this.ClientRectangle.Right - m_TabSystemBox.DisplayRectangle.X, this.ClientRectangle.Height);
            }

            if (m_Alignment == eTabStripAlignment.Bottom)
                r.Y++;
            else if (m_Alignment == eTabStripAlignment.Top)
                r.Height -= 2;

            return r;
        }

		private void PaintOneNote(Graphics g)
		{
			// Fill tab background color
			Rectangle r=GetTabClientArea(this.DisplayRectangle,false,false);
			//Rectangle rSysBox=Rectangle.Empty;

			if(m_NeedRecalcSize)
			{
				RecalcSize(g,GetTabClientArea(this.DisplayRectangle,false,false));
			}

			if(!m_ColorScheme.TabBackground2.IsEmpty && this.Height>0 && this.Width>0)
			{
				int gradientAngle=m_ColorScheme.TabBackgroundGradientAngle;
				if(m_Alignment==eTabStripAlignment.Bottom)
					gradientAngle-=180;
				else if(m_Alignment==eTabStripAlignment.Left)
					gradientAngle-=90;
				else if(m_Alignment==eTabStripAlignment.Right)
					gradientAngle+=90;

				using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(this.DisplayRectangle,m_ColorScheme.TabBackground,m_ColorScheme.TabBackground2,gradientAngle))
					g.FillRectangle(gradient,this.DisplayRectangle);
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(m_ColorScheme.TabBackground))
					g.FillRectangle(brush,this.DisplayRectangle);
				//g.Clear(m_ColorScheme.TabBackground);
			}

			if(!m_ColorScheme.TabBorder.IsEmpty)
			{
				using(Pen pen=new Pen(m_ColorScheme.TabBorder,1))
					g.DrawRectangle(pen,this.DisplayRectangle);
			}

			// Set text format
			TabItem selected=this.SelectedTab;
            eTextFormat strFormat = eTextFormat.Default | eTextFormat.EndEllipsis | eTextFormat.SingleLine |
                eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;

			// Draw the upper back line
			Rectangle backArea=this.GetBackgroundArea(this.DisplayRectangle);
			Color backAreaColor=(m_ColorScheme.TabItemSelectedBackground2.IsEmpty?m_ColorScheme.TabItemSelectedBackground:m_ColorScheme.TabItemSelectedBackground2);
			if(selected!=null && selected.PredefinedColor!=eTabItemColor.Default && !selected.BackColor2.IsEmpty)
				backAreaColor=selected.BackColor2;
			switch(m_Alignment)
			{
				case eTabStripAlignment.Top:
				{
					using(SolidBrush brush=new SolidBrush(backAreaColor))
						g.FillRectangle(brush,backArea.X,backArea.Bottom,backArea.Width,this.Height-r.Bottom);
					if(selected!=null)
					{
						using(Pen pen=new Pen(m_ColorScheme.TabItemSelectedBorder,1))
						{
							//RectangleF rs=this.GetTabPath(selected.DisplayRectangle,2,m_Alignment,false).GetBounds();
							Rectangle rs=selected.DisplayRectangle;
							rs.Width+=rs.Height;
							rs.X-=(rs.Height-1);
							if(this.HasNavigationBox && rs.Right>m_TabSystemBox.DisplayRectangle.X)
								rs.Width-=(rs.Right-m_TabSystemBox.DisplayRectangle.X+2);

							g.DrawLine(pen,backArea.X,backArea.Bottom,rs.X-1,backArea.Bottom);
							g.DrawLine(pen,rs.Right+1,backArea.Bottom,backArea.Right,backArea.Bottom);
							if(this.Parent is TabControl)
							{
								g.DrawLine(pen,backArea.X,backArea.Bottom,backArea.X,this.ClientRectangle.Bottom);
								g.DrawLine(pen,backArea.Right-1,backArea.Bottom,backArea.Right-1,this.ClientRectangle.Bottom);
							}
						}
					}
					break;
				}
				case eTabStripAlignment.Left:
				{
					using(SolidBrush brush=new SolidBrush(backAreaColor))
						g.FillRectangle(brush,backArea.Right,backArea.Y,this.Width-backArea.Width,this.Height);
					using(Pen pen=new Pen(m_ColorScheme.TabItemSelectedBorder,1))
					{
						g.DrawLine(pen,backArea.Right-1,backArea.Y,backArea.Right-1,backArea.Bottom);
						if(this.Parent is TabControl)
						{
							g.DrawLine(pen,backArea.Right-1,backArea.Y,this.ClientRectangle.Right,backArea.Y);
                            g.DrawLine(pen,backArea.Right-1,backArea.Bottom-1,this.ClientRectangle.Right,backArea.Bottom-1);
						}
					}
					break;
				}
				case eTabStripAlignment.Right:
				{
					using(SolidBrush brush=new SolidBrush(backAreaColor))
						g.FillRectangle(brush,0,0,this.Width-backArea.Width,this.Height);

					using(Pen pen=new Pen(m_ColorScheme.TabItemSelectedBorder,1))
					{
						g.DrawLine(pen,backArea.X,backArea.Y,backArea.X,backArea.Bottom);
						if(this.Parent is TabControl)
						{
							g.DrawLine(pen,backArea.X,backArea.Y,0,backArea.Y);
							g.DrawLine(pen,backArea.X,backArea.Bottom-1,0,backArea.Bottom-1);
						}
					}
					break;
				}
				default:
				{
					using(SolidBrush brush=new SolidBrush(backAreaColor))
						g.FillRectangle(brush,backArea.X,0,backArea.Width,backArea.Y);
					using(Pen pen=new Pen(m_ColorScheme.TabItemSelectedBorder,1))
					{
						g.DrawLine(pen,backArea.X,backArea.Y,backArea.Right,backArea.Y);
						if(this.Parent is TabControl)
						{
							g.DrawLine(pen,backArea.X,backArea.Y,backArea.X,0);
							g.DrawLine(pen,backArea.Right-1,backArea.Y,backArea.Right-1,0);
						}
					}
					break;
				}
			}

			r=GetTabClientArea(this.DisplayRectangle,m_TabSystemBox.Visible,true);
			Rectangle rClip=r;
			g.SetClip(r);

			bool bVirstVisible=true;
			bool bContinue=(m_Tabs.Count>0);

			// Bi-directional loop
			int iIndex=0;
            bool reverseLoop = false;
            if (m_Alignment == eTabStripAlignment.Left ||
                (this.IsRightToLeft && (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)))
            {
                iIndex = m_Tabs.Count - 1;
                reverseLoop = true;
            }
			int iMultiLineSwitch=-1;
			while(bContinue)
			{
				TabItem tab=m_Tabs[iIndex];
                if (reverseLoop)
				{
					iIndex--;
					if(iIndex<0) bContinue=false;
				}
				else
				{
					iIndex++;
					if(iIndex>=m_Tabs.Count) bContinue=false;
				}

				if(!tab.Visible || !r.IntersectsWith(tab.DisplayRectangle))
					continue;

                if (HasPreRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePreRenderTabItem(re);
                    if (re.Cancel) continue;
                }

				TabColors tabColors=this.GetTabColors(tab);
				Rectangle tabRect=tab.DisplayRectangle;
				if(m_Alignment==eTabStripAlignment.Right)
					tabRect.Height--;
				if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
				{
					if(iMultiLineSwitch==-1)
						iMultiLineSwitch=tab.DisplayRectangle.X;
					if(iMultiLineSwitch!=tab.DisplayRectangle.X)
					{
						iMultiLineSwitch=tab.DisplayRectangle.X;
						bVirstVisible=true;
					}
					GraphicsPath path=this.GetTabPath(tabRect,2,m_Alignment,true);
					Region clip=g.Clip;
					
					if(bVirstVisible || tab==m_SelectedTab)
					{
						RectangleF pb=path.GetBounds();
						pb.Width++;
						if(m_Alignment==eTabStripAlignment.Left)
						{
							//pb.Width--;
							if(bVirstVisible && tab!=m_SelectedTab)
								pb.Width--;
						}
						else
						{
							pb.Height++;
							if(bVirstVisible && tab!=m_SelectedTab)	pb.X++;
						}
						g.SetClip(pb);
					}
					else
					{
						if(m_Alignment==eTabStripAlignment.Right)
							g.SetClip(new Rectangle(tabRect.X+1,tabRect.Y+1,tabRect.Width+1,tabRect.Height+1));
						else
							g.SetClip(new Rectangle(tabRect.X,tabRect.Y,tabRect.Width-1,tabRect.Height));
					}
					ClipExcludeSystemBox(g);

                    if (tabColors.BackColor2.IsEmpty)
                    {
                        using (SolidBrush brush = new SolidBrush(tabColors.BackColor))
                            g.FillPath(brush, path);
                    }
                    else
                    {
                        using (LinearGradientBrush brush = BarFunctions.CreateLinearGradientBrush(path.GetBounds(), tabColors.BackColor, tabColors.BackColor2, (m_Alignment == eTabStripAlignment.Left ? tabColors.BackColorGradientAngle - 90 : tabColors.BackColorGradientAngle + 90)))
                            g.FillPath(brush, path);
                    }

					path=this.GetTabPath(tabRect,2,m_Alignment,false);
					using(Pen pen=new Pen(tabColors.BorderColor,1))
						g.DrawPath(pen,path);

					if(m_TabSystemBox.Visible && this.HasNavigationBox && path.GetBounds().IntersectsWith(m_TabSystemBox.DisplayRectangle))
					{
						Region reg=new Region(path);
						reg.Intersect(m_TabSystemBox.DisplayRectangle);
						RectangleF rg=reg.GetBounds(g);
						reg.Dispose();
						using(Pen pen=new Pen(tabColors.BorderColor,1))
						{
							pen.DashPattern=new float[] {2,2}; 
							g.DrawLine(pen,rg.X,rg.Y-1,rg.Right,rg.Y-1);
							g.DrawLine(pen,rg.X+2,rg.Y-2,rg.Right,rg.Y-2);
						}
					}
					else if(path.GetBounds().Y<-1 && path.GetBounds().Bottom>0)
					{
						RectangleF rg=path.GetBounds();
						using(Pen pen=new Pen(tabColors.BorderColor,1))
						{
							pen.DashPattern=new float[] {2,2}; 
							g.DrawLine(pen,rg.X,0,rg.Right,0);
							g.DrawLine(pen,rg.X+2,1,rg.Right,1);
						}
					}

					Rectangle rh=tabRect;
					if(m_Alignment==eTabStripAlignment.Right)
					{
						rh.Offset(-1,0);
						rh.Height-=1;
					}
					else
					{
						rh.Offset(1,0);
						rh.Height-=1;
						rh.Y++;
					}
					path=GetTabPath(rh,1,m_Alignment,false);
					
					using(Pen pen=new Pen(tabColors.LightBorderColor,1))
						g.DrawPath(pen,path);
					if(!tabColors.DarkBorderColor.IsEmpty)
					{
						RectangleF darkBounds=path.GetBounds();
						if(m_Alignment==eTabStripAlignment.Right)
						{
							darkBounds.Y=darkBounds.Bottom-1;
							darkBounds.Height=2;
							//darkBounds.Width--;
						}
						else
						{
							darkBounds.Height=2;
							darkBounds.Width-=2;
						}
						g.SetClip(darkBounds);
						ClipExcludeSystemBox(g);
						using(Pen pen=new Pen(tabColors.DarkBorderColor,1))
							g.DrawPath(pen,path);
					}

					g.SetClip(clip,CombineMode.Replace);

                    if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
                    {
                        tabRect.Y += 3;
                        tabRect.Height -= 3;
                        tab.CloseButtonBounds = PaintTabItemCloseButton(g, true, tab.CloseButtonMouseOver, tab == m_HotTab || tab == this.SelectedTab, ref tabRect);
                    }
                    else
                        tab.CloseButtonBounds = Rectangle.Empty;

					Image tabImage=tab.GetImage();
					Icon icon=tab.Icon;
					if(icon!=null)
					{
						Rectangle rIcon=new Rectangle(tabRect.X+(tabRect.Width-tab.IconSize.Width)/2,tabRect.Y+6,tab.IconSize.Width,tab.IconSize.Height);
						if(rClip.Contains(rIcon))
							g.DrawIcon(icon,rIcon);
						tabRect.Y+=(tab.IconSize.Height+2);
						tabRect.Height-=(tab.IconSize.Height+2);
					}
					else if(tabImage!=null)
					{
						g.DrawImage(tabImage,tabRect.X+(tabRect.Width-tabImage.Width)/2,tabRect.Y+6,tabImage.Width,tabImage.Height);
						tabRect.Y+=(tabImage.Height+2);
						tabRect.Height-=(tabImage.Height+2);
					}
					tabRect.Inflate(0,-1);
					tabRect.Height-=4;
					if(m_Style==eTabStripStyle.OneNote)
						tabRect.Y+=3;

					g.RotateTransform(90);
					if(!m_DisplaySelectedTextOnly || tab==m_SelectedTab)
					{
						Font font=this.Font;
						if(tab==selected)
						{
							if(m_SelectedTabFont!=null)
								font=m_SelectedTabFont;
						}
						if(tab!=selected)
						{
							Rectangle rText=new Rectangle(tabRect.Top,-tabRect.Right,tabRect.Height,tabRect.Width);
							if(rText.Height>MIN_TEXT_WIDTH)
                                TextDrawing.DrawStringLegacy(g, tab.Text, font, tabColors.TextColor, rText, strFormat);
							if(m_ShowFocusRectangle && this.Focused && tab==m_SelectedTab)
								ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(rText));
						}
						else
						{
							Rectangle rText=new Rectangle(tabRect.Top,-tabRect.Right,tabRect.Height,tabRect.Width);
							if(rText.Height>MIN_TEXT_WIDTH)
                                TextDrawing.DrawStringLegacy(g, tab.Text, font, tabColors.TextColor, rText, strFormat);
							if(m_ShowFocusRectangle && this.Focused && tab==m_SelectedTab)
								ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(rText));
						}
						g.ResetTransform();
					}
					// Draw separator
					if(tab!=selected)
					{
						g.DrawLine(new Pen(m_SeparatorColor,1),tab.DisplayRectangle.X+1,tab.DisplayRectangle.Bottom,tab.DisplayRectangle.Right-4,tab.DisplayRectangle.Bottom);
						if(!m_SeparatorShadeColor.IsEmpty)
							g.DrawLine(new Pen(m_SeparatorShadeColor,1),tab.DisplayRectangle.X+1+1,tab.DisplayRectangle.Bottom+1,tab.DisplayRectangle.Right-4+1,tab.DisplayRectangle.Bottom+1);
					}
				}
				else
				{
					if(iMultiLineSwitch==-1)
						iMultiLineSwitch=tab.DisplayRectangle.Y;
					if(iMultiLineSwitch!=tab.DisplayRectangle.Y)
					{
						iMultiLineSwitch=tab.DisplayRectangle.Y;
						bVirstVisible=true;
					}

					GraphicsPath path=this.GetTabPath(tabRect,2,m_Alignment,true);
					Region clip=g.Clip;
					
					if(bVirstVisible || tab==m_SelectedTab)
					{
						//RectangleF pb=path.GetBounds();
						Rectangle pb=tabRect;
						pb.Width+=pb.Height;
						//if(m_Alignment!=eTabStripAlignment.Bottom)
							pb.X-=pb.Height;
						
						pb.Width++;
                        pb.Height++;
                        if (m_Alignment == eTabStripAlignment.Bottom && bVirstVisible && tab != m_SelectedTab)
                            pb.Y++;
                        
						g.SetClip(pb,CombineMode.Replace);
					}
					else
					{
						if(m_Alignment==eTabStripAlignment.Top)
							g.SetClip(new Rectangle(tabRect.X+1,tabRect.Y,tabRect.Width+1,tabRect.Height),CombineMode.Replace);
						else
                            g.SetClip(new Rectangle(tabRect.X + 1, tabRect.Y+1, tabRect.Width + 1, tabRect.Height+1), CombineMode.Replace);
					}
					ClipExcludeSystemBox(g);

                    if (tabColors.BackColor2.IsEmpty)
                    {
                        using (SolidBrush brush = new SolidBrush(tabColors.BackColor))
                            g.FillPath(brush, path);
                    }
                    else
                    {
                        using (LinearGradientBrush brush = BarFunctions.CreateLinearGradientBrush(path.GetBounds(), tabColors.BackColor, tabColors.BackColor2, (m_Alignment == eTabStripAlignment.Top ? tabColors.BackColorGradientAngle : -tabColors.BackColorGradientAngle)))
                            g.FillPath(brush, path);
                    }

					path=this.GetTabPath(tabRect,2,m_Alignment,false);
					using(Pen pen=new Pen(tabColors.BorderColor,1))
						g.DrawPath(pen,path);

					if(m_TabSystemBox.Visible && this.HasNavigationBox && path.GetBounds().IntersectsWith(m_TabSystemBox.DisplayRectangle))
					{
						Region reg=new Region(path);
						reg.Intersect(m_TabSystemBox.DisplayRectangle);
						RectangleF rg=reg.GetBounds(g);
						reg.Dispose();
						using(Pen pen=new Pen(tabColors.BorderColor,1))
						{
							pen.DashPattern=new float[] {2,2}; 
							g.DrawLine(pen,rg.X-1,rg.Y,rg.X-1,rg.Bottom);
							g.DrawLine(pen,rg.X-2,rg.Y+2,rg.X-2,rg.Bottom);
						}
					}
					else if(path.GetBounds().X<-1 && path.GetBounds().Right>0)
					{
						RectangleF rg=path.GetBounds();
						using(Pen pen=new Pen(tabColors.BorderColor,1))
						{
							pen.DashPattern=new float[] {2,2}; 
							g.DrawLine(pen,0,rg.Y,0,rg.Bottom);
							g.DrawLine(pen,1,rg.Y+2,1,rg.Bottom);
						}
					}
                    
					Rectangle rh=tabRect;
					if(m_Alignment==eTabStripAlignment.Top)
					{
						rh.Offset(1,1);
						rh.X-=1;
						rh.Width-=1;
					}
                    else
                    {
                        rh.Offset(1, -1);
                        rh.X -= 1;
                        rh.Width -= 1;
                    }
					path=GetTabPath(rh,1,m_Alignment,false);

                    if (!tabColors.LightBorderColor.IsEmpty)
                    {
                        RectangleF lightBounds = path.GetBounds();
                        lightBounds.Height--;
                        Region oldClip = g.Clip.Clone() as Region;
                        g.SetClip(lightBounds, CombineMode.Intersect);

                        using (Pen pen = new Pen(tabColors.LightBorderColor, 1))
                            g.DrawPath(pen, path);

                        g.SetClip(oldClip, CombineMode.Replace);
                    }
					
					if(!tabColors.DarkBorderColor.IsEmpty)
					{
						RectangleF darkBounds=path.GetBounds();
						darkBounds.X=darkBounds.Right-1;
						darkBounds.Width=2;
						darkBounds.Height--;

                        g.SetClip(clip,CombineMode.Replace);
						g.SetClip(darkBounds,CombineMode.Intersect);
						using(Pen pen=new Pen(tabColors.DarkBorderColor,1))
							g.DrawPath(pen,path);
					}

					g.SetClip(clip,CombineMode.Replace);

					//if(m_Alignment==eTabStripAlignment.Top)
						tabRect.Offset(0,1);

                    if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
                        tab.CloseButtonBounds = this.PaintTabItemCloseButton(g, false, tab.CloseButtonMouseOver, tab == m_HotTab || tab == this.SelectedTab, ref tabRect);
                    else
                        tab.CloseButtonBounds = Rectangle.Empty;

					Image tabImage=tab.GetImage();
					Icon icon=tab.Icon;
					if(icon!=null)
					{
						Rectangle rIcon=new Rectangle(tabRect.X+tabRect.Height/3+(m_Alignment==eTabStripAlignment.Top?-2:0),tabRect.Y+(tabRect.Height-tab.IconSize.Height)/2,tab.IconSize.Width,tab.IconSize.Height);
						if(rClip.Contains(rIcon))
							g.DrawIcon(icon,rIcon);
						tabRect.X+=(tab.IconSize.Width+2);
						tabRect.Width-=(tab.IconSize.Width+2);
					}
					else if(tabImage!=null)
					{
						g.DrawImage(tabImage,tabRect.X+tabRect.Height/3+(m_Alignment==eTabStripAlignment.Top?-2:0),tabRect.Y+(tabRect.Height-tabImage.Height)/2,tabImage.Width,tabImage.Height);
						tabRect.X+=(tabImage.Width+2);
						tabRect.Width-=(tabImage.Width+2);
					}
					tabRect.Inflate(0,-1);

					tabRect.Width-=4;
					if(m_Style==eTabStripStyle.OneNote)
						tabRect.X+=3;

					if(!m_DisplaySelectedTextOnly || tab==m_SelectedTab)
					{
						Font font=this.Font;
						if(tab==selected && m_SelectedTabFont!=null)
							font=m_SelectedTabFont;

						if(tab!=selected)
						{
							if(tabRect.Width>MIN_TEXT_WIDTH)
								TextDrawing.DrawString(g,tab.Text,font,tabColors.TextColor,tabRect,strFormat);
							if(m_ShowFocusRectangle && this.Focused && tab==m_SelectedTab)
								ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(tabRect));
						}
						else
						{
							if(tabRect.Width>MIN_TEXT_WIDTH)
								TextDrawing.DrawString(g,tab.Text,font,tabColors.TextColor,tabRect,strFormat);
							if(m_ShowFocusRectangle && this.Focused && tab==m_SelectedTab)
								ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(tabRect));
						}
					}	                    
				}
                if (HasPostRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePostRenderTabItem(re);
                }
				bVirstVisible=false;
			}

			g.ResetClip();
			if(this.HasNavigationBox && m_TabSystemBox.Visible)
			{
				m_TabSystemBox.Paint(g);
			}
		}

        private Rectangle PaintTabItemCloseButton(Graphics g, bool vertical, bool mouseOver, bool isTabHotOrSelected, ref Rectangle tabRect)
        {
            Size closeSize = m_CloseButtonSize;
            bool closeOnLeftSide = (m_CloseButtonPosition== eTabCloseButtonPosition.Left);

            int offset = (closeOnLeftSide ? 2 : 4);

            Rectangle close = Rectangle.Empty;

            if (closeOnLeftSide)
            {
                if (vertical)
                    close = new Rectangle(tabRect.X + (tabRect.Width - closeSize.Width) / 2, tabRect.Y + offset, closeSize.Width, closeSize.Height);
                else
                    close = new Rectangle(tabRect.X + offset, tabRect.Y + (tabRect.Height - closeSize.Height) / 2, closeSize.Width, closeSize.Height);
            }
            else
            {
                if (vertical)
                    close = new Rectangle(tabRect.X + (tabRect.Width - closeSize.Width) / 2, tabRect.Bottom - offset - closeSize.Height, closeSize.Width, closeSize.Height);
                else
                    close = new Rectangle(tabRect.Right - offset - closeSize.Width, tabRect.Y + (tabRect.Height - closeSize.Height) / 2, closeSize.Width, closeSize.Height);
            }

            if (isTabHotOrSelected || m_CloseButtonOnTabsAlwaysDisplayed)
                TabStripBaseDisplay.PaintTabItemCloseButton(g, close, mouseOver, this);

            if (vertical)
            {
                if (closeOnLeftSide)
                    tabRect.Y += close.Height + offset;
                tabRect.Height -= close.Height + offset;
            }
            else
            {
                if (closeOnLeftSide)
                    tabRect.X += close.Width + offset;
                tabRect.Width -= close.Width + offset;
            }

            return close;
        }

        private GraphicsPath GetTabPath(Rectangle r, int rightCornerSize, eTabStripAlignment align, bool bCloseFigure)
		{
			Rectangle rbox=r;

			if(align==eTabStripAlignment.Left)
			{
				// Left
				rbox=new Rectangle(r.X,r.Y,r.Height,r.Width);
			}
			else if(align==eTabStripAlignment.Right)
			{
				// Right
				rbox=new Rectangle(r.Right-r.Height,r.Y,r.Height,r.Width);
			}

			GraphicsPath path=new GraphicsPath();
			Point[] p=new Point[4];
			p[0].X=rbox.X+2-(rbox.Height+1);
			p[0].Y=rbox.Bottom-1;
			p[1].X=p[0].X+3;
			p[1].Y=p[0].Y-2;
			p[2].X=p[1].X+rbox.Height-6;
			p[2].Y=rbox.Y+3;
			p[3].X=p[2].X+4;
			p[3].Y=rbox.Y+1;
			path.AddCurve(p,0,3,.5f);
			path.AddLine(p[3].X+1,rbox.Y,rbox.Right-rightCornerSize,rbox.Y);
			path.AddLine(rbox.Right-rightCornerSize,rbox.Y,rbox.Right,rbox.Y+rightCornerSize);
			path.AddLine(rbox.Right,rbox.Y+rightCornerSize,rbox.Right,rbox.Bottom-1);
			if(bCloseFigure)
			{
				path.AddLine(p[0].X,rbox.Bottom,rbox.Right,rbox.Bottom);
				path.CloseAllFigures();
			}
			
			if(align==eTabStripAlignment.Bottom)
			{
                path.Dispose();
                path = new GraphicsPath();
                p = new Point[4];
                p[0].X = rbox.X + 2 - (rbox.Height + 1);
                p[0].Y = rbox.Top+1;
                p[1].X = p[0].X + 3;
                p[1].Y = p[0].Y + 2;
                p[2].X = p[1].X + rbox.Height - 6;
                p[2].Y = rbox.Bottom - 3;
                p[3].X = p[2].X + 4;
                p[3].Y = rbox.Bottom - 1;
                path.AddCurve(p, 0, 3, .5f);
                path.AddLine(p[3].X + 1, rbox.Bottom, rbox.Right - rightCornerSize, rbox.Bottom);
                path.AddLine(rbox.Right - rightCornerSize, rbox.Bottom, rbox.Right, rbox.Bottom - rightCornerSize);
                path.AddLine(rbox.Right, rbox.Bottom - rightCornerSize, rbox.Right, rbox.Y + 1);
                if (bCloseFigure)
                {
                    path.AddLine(p[0].X, rbox.Y, rbox.Right, rbox.Y);
                    path.CloseAllFigures();
                }
				// Bottom
                //Matrix m=new Matrix();
                ////RectangleF rf=path.GetBounds();
                //m.RotateAt(180,new PointF(rbox.X+rbox.Width/2,rbox.Y+rbox.Height/2));
                //path.Transform(m);
			}
			else if(align==eTabStripAlignment.Left)
			{
				// Left
				Matrix m=new Matrix();
				//RectangleF rf=path.GetBounds();
				m.RotateAt(-90,new PointF(rbox.X,rbox.Bottom));
				m.Translate(rbox.Height,rbox.Width-rbox.Height,MatrixOrder.Append);
				path.Transform(m);
			}
			else if(align==eTabStripAlignment.Right)
			{
				// Right
				Matrix m=new Matrix();
				//RectangleF rf=path.GetBounds();
				m.RotateAt(90,new PointF(rbox.Right,rbox.Bottom));
				m.Translate(-rbox.Height,rbox.Width-(rbox.Height-1),MatrixOrder.Append);
				path.Transform(m);
			}

			return path;
		}

		private void PaintThemed(Graphics g)
		{
			if(m_ThemeTab==null)
				this.RefreshThemes();
			
			if(m_ColorScheme.TabBackground.IsEmpty)
				m_ThemeTab.DrawBackground(g,ThemeTabParts.Body,ThemeTabStates.BodyNormal,this.ClientRectangle);
			else if(!m_ColorScheme.TabBackground2.IsEmpty && this.Width>0 && this.Height>0)
			{
				using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(this.ClientRectangle,m_ColorScheme.TabBackground,m_ColorScheme.TabBackground2,m_ColorScheme.TabBackgroundGradientAngle))
					g.FillRectangle(gradient,this.ClientRectangle);
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(m_ColorScheme.TabBackground))
					g.FillRectangle(brush,this.DisplayRectangle);
				//g.Clear(m_ColorScheme.TabBackground);
			}

			Rectangle r=this.GetTabClientArea(this.DisplayRectangle,false,false); //this.DisplayRectangle;

			// Text format
			TabItem selected=this.SelectedTab;
            eTextFormat strFormat = eTextFormat.Default | eTextFormat.SingleLine |
                eTextFormat.EndEllipsis | eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter;

			if(m_NeedRecalcSize)
				RecalcSize(g,r);

			// Draw tab base line
			if(this.Parent is TabControl)
			{
				if(m_Alignment==eTabStripAlignment.Top)
					m_ThemeTab.DrawBackground(g,ThemeTabParts.Pane,ThemeTabStates.PaneNormal,new Rectangle(0,r.Bottom,ClientRectangle.Width,ClientRectangle.Height));
				else
				{
					Size sz=new Size(this.Width,5);
					if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
						sz=new Size(this.Height,5);
					Bitmap bmp=new Bitmap(sz.Width,sz.Height,g);
					try
					{
						Graphics gtmp=Graphics.FromImage(bmp);
						try
						{
							m_ThemeTab.DrawBackground(gtmp,ThemeTabParts.Pane,ThemeTabStates.PaneNormal,new Rectangle(0,0,sz.Width,sz.Height));
						}
						finally
						{
							gtmp.Dispose();
						}
						if(m_Alignment==eTabStripAlignment.Left)
						{
							bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
							g.DrawImageUnscaled(bmp,r.Right,0);
						}
						else if(m_Alignment==eTabStripAlignment.Right)
						{
							bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
							g.DrawImageUnscaled(bmp,r.X-bmp.Width,0);
						}
						else if(m_Alignment==eTabStripAlignment.Bottom)
						{
							bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
							g.DrawImageUnscaled(bmp,0,r.Y-bmp.Height);
						}
					}
					finally
					{
						bmp.Dispose();
					}
				}
			}

			if(m_Alignment==eTabStripAlignment.Top)
				r.Height++;
			Rectangle rClip=r;
			if(m_Alignment==eTabStripAlignment.Bottom)
			{
				rClip.Y--;
				rClip.Height++;
			}
			else if(m_Alignment==eTabStripAlignment.Left)
			{
				rClip.Width++;
			}
			else if(m_Alignment==eTabStripAlignment.Right)
			{
				rClip.X--;
				rClip.Width++;
			}
			if(this.HasNavigationBox)
			{
				if(m_Alignment==eTabStripAlignment.Right || m_Alignment==eTabStripAlignment.Left)
					rClip.Height-=m_TabSystemBox.DisplayRectangle.Height;
				else
					rClip.Width-=m_TabSystemBox.DisplayRectangle.Width;
			}
			g.SetClip(rClip);

			foreach(TabItem tab in m_Tabs)
			{
				if(!tab.Visible || !r.IntersectsWith(tab.DisplayRectangle))
					continue;

                if (HasPreRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePreRenderTabItem(re);
                    if (re.Cancel) continue;
                }
				
				Rectangle tabRect=tab.DisplayRectangle;
				Rectangle rTemp=new Rectangle(0,0,tabRect.Width,tabRect.Height);
				if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
					rTemp=new Rectangle(0,0,tabRect.Height,tabRect.Width);
				if(tab==selected)
				{
					if(m_Alignment==eTabStripAlignment.Top)
						rTemp.Height+=2;
					else if(m_Alignment==eTabStripAlignment.Bottom)
					{
						rTemp.Height+=2;
						tabRect.Y--;
					}
					else if(m_Alignment==eTabStripAlignment.Left)
					{
						rTemp.Height+=2;
						tabRect.X--;
					}
					else if(m_Alignment==eTabStripAlignment.Right)
					{
						rTemp.Height+=2;
						tabRect.X--;
					}
				}

				Bitmap temp=new Bitmap(rTemp.Width,rTemp.Height,g);
				try
				{
					Graphics gtemp=Graphics.FromImage(temp);
					try
					{
						using(SolidBrush brush=new SolidBrush(Color.Transparent))
							gtemp.FillRectangle(brush,0,0,temp.Width,temp.Height);
						//gtemp.Clear(Color.Transparent);
						if(m_Alignment==eTabStripAlignment.Bottom)
						{
							DrawThemedTab(gtemp,rTemp,tab,strFormat,rTemp,true,false);
							temp.RotateFlip(RotateFlipType.Rotate180FlipNone);
							DrawThemedTab(gtemp,rTemp,tab,strFormat,rTemp,false,true);
						}
						else
							DrawThemedTab(gtemp,rTemp,tab,strFormat,rTemp,true,true);
					}
					finally
					{
						gtemp.Dispose();
					}
				}
				finally
				{
					if(m_Alignment==eTabStripAlignment.Left)
						temp.RotateFlip(RotateFlipType.Rotate270FlipNone);
					else if(m_Alignment==eTabStripAlignment.Right)
                        temp.RotateFlip(RotateFlipType.Rotate90FlipNone);

					g.DrawImageUnscaled(temp,tabRect);
					temp.Dispose();
				}

                if (HasPostRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePostRenderTabItem(re);
                }
			}
			g.ResetClip();
			if(this.HasNavigationBox && m_TabSystemBox.Visible)
			{
				g.SetClip(m_TabSystemBox.DisplayRectangle);
				using(SolidBrush brushSysBox=new SolidBrush(m_ColorScheme.TabBackground))
					g.FillRectangle(brushSysBox,m_TabSystemBox.DisplayRectangle);
				m_TabSystemBox.Paint(g);
				g.ResetClip();
			}

		}
		private void DrawThemedTab(Graphics g, Rectangle tabRect, TabItem tab, eTextFormat strFormat, Rectangle rClip, bool drawBack, bool drawInner)
		{
			TabColors tabColor=this.GetTabColors(tab);
			ThemeTabStates tabState=ThemeTabStates.Normal;

			if(m_HotTab==tab)
				tabState=ThemeTabStates.Hot;
			else if(tab==m_SelectedTab)
				tabState=ThemeTabStates.Selected;

			if(drawBack)
			{
				if(tab==m_SelectedTab)
					m_ThemeTab.DrawBackground(g,ThemeTabParts.TabItem,tabState,new Rectangle(tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1));
				else
					m_ThemeTab.DrawBackground(g,ThemeTabParts.TabItem,tabState,new Rectangle(tabRect.X,tabRect.Y,tabRect.Width,tabRect.Height+1));
			}

			tabRect.Offset(0,1);

			if(drawInner)
			{
                if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
                {
                    Rectangle rc = PaintTabItemCloseButton(g, (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right), tab.CloseButtonMouseOver, tab == m_HotTab || tab == this.SelectedTab, ref tabRect);
                    rc.Offset(tab.DisplayRectangle.Location);
                    tab.CloseButtonBounds = rc;
                }
                else
                    tab.CloseButtonBounds = Rectangle.Empty;

				Image tabImage=tab.GetImage();
				Icon icon=tab.Icon;
				if(tabImage!=null && tabImage.Width+4<=tabRect.Width || icon!=null && tab.IconSize.Width+4<=tabRect.Width)
				{
					if(icon!=null)
					{
						Rectangle rIcon=new Rectangle(tabRect.X+4,tabRect.Y+(tabRect.Height-tab.IconSize.Height)/2,tab.IconSize.Width,tab.IconSize.Height);
						if(rClip.Contains(rIcon))
							g.DrawIcon(icon,rIcon);
						tabRect.X+=(tab.IconSize.Width+2);
						tabRect.Width-=(tab.IconSize.Width+2);
					}
					else if(tabImage!=null)
					{
						g.DrawImage(tabImage,tabRect.X+4,tabRect.Y+(tabRect.Height-tabImage.Height)/2,tabImage.Width,tabImage.Height);
						tabRect.X+=(tabImage.Width+2);
						tabRect.Width-=(tabImage.Width+2);
					}
					tabRect.Inflate(0,-1);
					tabRect.Width-=4;
					tabRect.X+=3;
				}

				if(!m_DisplaySelectedTextOnly || tab==m_SelectedTab)
				{
					Font font=this.Font;
					if(tab==m_SelectedTab && m_SelectedTabFont!=null)
						font=m_SelectedTabFont;

					if(tabRect.Width>MIN_TEXT_WIDTH)
						TextDrawing.DrawString(g,tab.Text,font,tabColor.TextColor,tabRect,strFormat);
					if(m_ShowFocusRectangle && this.Focused && tab==m_SelectedTab)
						ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(tabRect));
				}
			}
		}

		private void PaintVS2005(Graphics g)
		{
			if(!m_ColorScheme.TabBackground2.IsEmpty && this.Width>0 && this.Height>0)
			{
				using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(this.ClientRectangle,m_ColorScheme.TabBackground,m_ColorScheme.TabBackground2,m_ColorScheme.TabBackgroundGradientAngle))
					g.FillRectangle(gradient,this.ClientRectangle);
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(m_ColorScheme.TabBackground))
					g.FillRectangle(brush,this.DisplayRectangle);
			}

			Rectangle r=this.GetTabClientArea(this.DisplayRectangle,false,false);
			Rectangle client=this.DisplayRectangle;

			if(m_NeedRecalcSize)
				RecalcSize(g,r);

            // Text format
            TabItem selected = this.SelectedTab;
            Rectangle selectedRect = Rectangle.Empty;
            if (selected != null)
                selectedRect = selected.DisplayRectangle;
            eTextFormat strFormat = eTextFormat.Default | eTextFormat.SingleLine | eTextFormat.EndEllipsis |
                eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter;

			switch(m_Alignment)
			{
				case eTabStripAlignment.Top:
				{
                    using (Pen pen = new Pen(m_ColorScheme.TabItemSelectedBorder, 1))
                    {
                        if (!selectedRect.IsEmpty)
                        {
                            g.DrawLine(pen, client.X, r.Bottom, selectedRect.X, r.Bottom);
                            g.DrawLine(pen, selectedRect.Right - 1, r.Bottom, client.Right, r.Bottom);
                        }
                        else
                            g.DrawLine(pen, client.X, client.Bottom - 1, client.Right, client.Bottom - 1);
                    }
                    //r.Height--;
					break;
				}
				case eTabStripAlignment.Bottom:
				{
                    using (Pen pen = new Pen(m_ColorScheme.TabItemSelectedBorder, 1))
                    {
                        if (!selectedRect.IsEmpty)
                        {
                            g.DrawLine(pen, client.X, client.Y, selectedRect.X, client.Y);
                            g.DrawLine(pen, selectedRect.Right - 1, client.Y, client.Right, client.Y);
                        }
                        else
                            g.DrawLine(pen, client.X, client.Y, client.Right, client.Y);
                    }
                    r.Y++;
                    r.Height--;
					break;
				}
				case eTabStripAlignment.Left:
				{
                    using (Pen pen = new Pen(m_ColorScheme.TabItemSelectedBorder, 1))
                    {
                        if (!selectedRect.IsEmpty)
                        {
                            g.DrawLine(pen, client.Right - 1, client.Y, client.Right - 1, selectedRect.Y);
                            g.DrawLine(pen, client.Right - 1, selectedRect.Bottom - 1, client.Right - 1, client.Bottom);
                        }
                        else
                            g.DrawLine(pen, client.Right - 1, client.Y, client.Right - 1, client.Bottom);
                    }
                    r.Width--;
					break;
				}
				case eTabStripAlignment.Right:
				{
                    using (Pen pen = new Pen(m_ColorScheme.TabItemSelectedBorder, 1))
                    {
                        if (!selectedRect.IsEmpty)
                        {
                            g.DrawLine(pen, client.X, client.Y, client.X, selectedRect.Y);
                            g.DrawLine(pen, client.X, selectedRect.Bottom - 1, client.X, client.Bottom);
                        }
                        else
                            g.DrawLine(pen, client.X, client.Y, client.X, client.Bottom);
                    }
                    r.Width--;
                    r.X++;
					break;
				}
			}

            if (this.HasNavigationBox && m_TabSystemBox.Visible)
			{
                if (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left)
                    r.Height -= m_TabSystemBox.DisplayRectangle.Height;
                else
                {
                    r.Width -= m_TabSystemBox.DisplayRectangle.Width;
                    if (this.IsRightToLeft)
                        r.X += m_TabSystemBox.DisplayRectangle.Width;
                }
			}

			Rectangle rClip=r;
			if(m_Alignment==eTabStripAlignment.Right)
				rClip.Width++;
			
			g.SetClip(rClip);

			foreach(TabItem tab in m_Tabs)
			{
				if(!tab.Visible || !r.IntersectsWith(tab.DisplayRectangle))
					continue;

                if (HasPreRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePreRenderTabItem(re);
                    if (re.Cancel) continue;
                }

				Rectangle tabRect=tab.DisplayRectangle;
				TabColors colors=GetTabColors(tab);
                
				GraphicsPath path=new GraphicsPath();
				switch(m_Alignment)
				{
					case eTabStripAlignment.Top:
					{
                        tabRect.Width--;
						path.AddLine(tabRect.X+2,tabRect.Y,tabRect.Right-2,tabRect.Y);
                        path.AddLine(tabRect.Right,tabRect.Y+2,tabRect.Right,tabRect.Bottom);
                        path.AddLine(tabRect.Right,tabRect.Bottom,tabRect.X,tabRect.Bottom);
						path.AddLine(tabRect.X,tabRect.Y+2,tabRect.X+2,tabRect.Y);
						path.CloseAllFigures();
						break;
					}
					case eTabStripAlignment.Bottom:
					{
                        tabRect.Width--;
						path.AddLine(tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
						path.AddLine(tabRect.Right,tabRect.Y,tabRect.Right,tabRect.Bottom-2);
                        path.AddLine(tabRect.Right-2,tabRect.Bottom,tabRect.X+2,tabRect.Bottom);
						path.AddLine(tabRect.X,tabRect.Bottom-2,tabRect.X,tabRect.Y);
						path.CloseAllFigures();
						break;
					}
					case eTabStripAlignment.Left:
					{
                        tabRect.Height--;
						path.AddLine(tabRect.X+2,tabRect.Y,tabRect.Right,tabRect.Y);
						path.AddLine(tabRect.Right,tabRect.Bottom,tabRect.X+2,tabRect.Bottom);
						path.AddLine(tabRect.X,tabRect.Bottom-2,tabRect.X,tabRect.Y+2);
						path.CloseAllFigures();
						break;
					}
					case eTabStripAlignment.Right:
					{
                        tabRect.Height--;
						tabRect.Width-=2;
						path.AddLine(tabRect.X,tabRect.Y,tabRect.Right-2,tabRect.Y);
						path.AddLine(tabRect.Right,tabRect.Y+2,tabRect.Right,tabRect.Bottom-2);
						path.AddLine(tabRect.Right-2,tabRect.Bottom,tabRect.X,tabRect.Bottom);
						path.CloseAllFigures();
						break;
					}
				}

				// Draw Background
				//g.SetClip(rClip);
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
					using(LinearGradientBrush brush=BarFunctions.CreateLinearGradientBrush(tabRect,colors.BackColor,colors.BackColor2,colors.BackColorGradientAngle))
						g.FillPath(brush,path);
				}
				//g.SetClip(rClip);

				// Draw border
				if(!colors.BorderColor.IsEmpty)
				{
					using(Pen pen=new Pen(colors.BorderColor,1))
						g.DrawPath(pen,path);
				}

                if (m_CloseButtonOnTabs && tab.CloseButtonVisible)
                    tab.CloseButtonBounds = PaintTabItemCloseButton(g, (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right), tab.CloseButtonMouseOver, tab == m_HotTab || tab == this.SelectedTab, ref tabRect);
                else
                    tab.CloseButtonBounds = Rectangle.Empty;
				// Draw image
				CompositeImage image=this.GetTabImage(tab);
				if(image!=null && image.Width+4<=tabRect.Width)
				{
					if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
					{
						image.DrawImage(g,new Rectangle(tabRect.X+3,tabRect.Y+(tabRect.Height-image.Height)/2,image.Width,image.Height));
						int offset=image.Width+2;
						tabRect.X+=offset;
						tabRect.Width-=offset;
					}
					else
					{
						image.DrawImage(g,new Rectangle(tabRect.X+(tabRect.Width-image.Width)/2,tabRect.Y+3,image.Width,image.Height));
						int offset=image.Height+2;
						tabRect.Y+=offset;
						tabRect.Height-=offset;
					}
				}

				// Draw text
				if(!m_DisplaySelectedTextOnly || tab==m_SelectedTab)
				{
					Font font=this.Font;
					if(tab==selected && m_SelectedTabFont!=null)
						font=m_SelectedTabFont;

					Rectangle rText=tabRect;
					if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
					{
						g.RotateTransform(90);
						rText=new Rectangle(rText.Top,-rText.Right,rText.Height,rText.Width);
					}

					if(rText.Width>MIN_TEXT_WIDTH)
					{
                        if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
                            TextDrawing.DrawStringLegacy(g, tab.Text, font, colors.TextColor, rText, strFormat);
                        else
                            TextDrawing.DrawString(g,tab.Text,font,colors.TextColor,rText,strFormat);
					}

					if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
						g.ResetTransform();

					if(m_ShowFocusRectangle && this.Focused && tab==selected)
						ControlPaint.DrawFocusRectangle(g,GetFocusRectangle(tabRect));
				}

                // Draw tab separator line
                if (!tab.IsSelected && m_Tabs.IndexOf(tab) < m_Tabs.Count - 1 && m_Tabs.IndexOf(tab) + 1!=m_Tabs.IndexOf(m_SelectedTab))
                {
                    if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
                    {
                        DisplayHelp.DrawLine(g, tab.DisplayRectangle.X + 3, tab.DisplayRectangle.Bottom - 1, tab.DisplayRectangle.Right - 3, tab.DisplayRectangle.Bottom - 1, m_ColorScheme.TabItemSeparator, 1);
                        DisplayHelp.DrawLine(g, tab.DisplayRectangle.X + 3, tab.DisplayRectangle.Bottom, tab.DisplayRectangle.Right - 3, tab.DisplayRectangle.Bottom, m_ColorScheme.TabItemSeparatorShade, 1);
                    }
                    else
                    {
                        DisplayHelp.DrawLine(g, tab.DisplayRectangle.Right - 1, tab.DisplayRectangle.Y + 3, tab.DisplayRectangle.Right - 1, tab.DisplayRectangle.Bottom - 3, m_ColorScheme.TabItemSeparator, 1);
                        DisplayHelp.DrawLine(g, tab.DisplayRectangle.Right, tab.DisplayRectangle.Y + 3, tab.DisplayRectangle.Right, tab.DisplayRectangle.Bottom - 3, m_ColorScheme.TabItemSeparatorShade, 1);
                    }
                    
                }

                if (HasPostRenderTabItem)
                {
                    RenderTabItemEventArgs re = new RenderTabItemEventArgs(tab, g);
                    InvokePostRenderTabItem(re);
                }
			}

			g.ResetClip();
			if(this.HasNavigationBox && m_TabSystemBox.Visible)
			{
				g.SetClip(m_TabSystemBox.DisplayRectangle);
				using(SolidBrush brushSysBox=new SolidBrush(m_ColorScheme.TabBackground))
					g.FillRectangle(brushSysBox,m_TabSystemBox.DisplayRectangle);
				m_TabSystemBox.Paint(g);
				g.ResetClip();
			}
		}

		private CompositeImage GetTabImage(TabItem tab)
		{
			Image image=tab.GetImage();
			if(image!=null)
				return new CompositeImage(image,false);
			Icon icon=tab.Icon;
			if(icon!=null)
				return new CompositeImage(icon,false,tab.IconSize);

			return null;
		}

		/// <summary>
		/// Returns minimum tab strip height given the style and the tabs it contains.
		/// </summary>
        [Browsable(false)]
		public int MinTabStripHeight
		{
			get
			{
                if (m_Style == eTabStripStyle.SimulatedTheme || m_Style == eTabStripStyle.Office2007Document || m_Style == eTabStripStyle.Office2007Dock)
                {
                    if (m_TabItemsBounds.IsEmpty || m_NeedRecalcSize)
                    {
                        if (this.Tabs.Count == 0 || !m_NeedRecalcSize)
                            return this.TabHeight + 6;
                        this.RecalcSize();
                    }
                    if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
                        return m_TabItemsBounds.Height + 3;
                    else
                        return (m_TabItemsBounds.Width + 3);
                }
                //else if (m_Style == eTabStripStyle.Office2007Document && (m_TabItemsBounds.IsEmpty || m_NeedRecalcSize))
                //    this.RecalcSize();
                if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
					return this.TabHeight+6;
				else
					return this.TabHeight+5;
			}
		}

		private int m_TabHeight=-1;
		private int TabHeight
		{
			get
			{
				if(m_TabHeight<=0)
				{
					Graphics g=null;
					if(BarFunctions.IsHandleValid(this))
						g=this.CreateGraphics();
					else
					{
						try
						{
							g=Graphics.FromHwnd(IntPtr.Zero);
						}
						catch{}
					}
					if(g!=null)
					{
						try
						{
							m_TabHeight=GetTabHeight(g);
						}
						finally
						{
							g.Dispose();
						}
					}
				}

				return m_TabHeight;
			}
		}
		internal void ResetTabHeight()
		{
			m_TabHeight=0;
		}
		private int GetSingleTabHeight()
		{
			if(this.IsMultiLine)
			{
				int height=0;
				if(BarFunctions.IsHandleValid(this))
				{
					Graphics g=this.CreateGraphics();
					try
					{
						height=GetTabHeight(g);
					}
					finally
					{
						g.Dispose();
					}
				}
				return height;
			}
			return this.TabHeight;
		}

        private bool IsVertical
        {
            get
            {
                if ((m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right) && this.Style != eTabStripStyle.SimulatedTheme)
                    return true;
                return false;
            }
        }

		private int GetTabHeight(Graphics g)
		{
            if (m_FixedTabSize.Height > 0)
                return m_FixedTabSize.Height;
			int iHeight=16;
            if ((m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right) && m_Style != eTabStripStyle.OneNote && m_Style != eTabStripStyle.VS2005Document && m_Style!=eTabStripStyle.Office2007Document)
				iHeight=20;
			if(m_ImageList!=null)
				iHeight=m_ImageList.ImageSize.Height;
			else
			{
                Font font = this.Font;
                if (m_SelectedTabFont != null)
                    font = m_SelectedTabFont;
				foreach(TabItem tab in m_Tabs)
				{
					if(tab.Icon!=null && tab.Icon.Height>iHeight)
						iHeight=tab.IconSize.Height;
					else if(tab.Image!=null && tab.Image.Height>iHeight)
						iHeight=tab.Image.Height;
					string s=tab.Text;
					if(s!="")
					{
                        Size sz = Size.Empty;
                        if(IsVertical)
                            sz = TextDrawing.MeasureStringLegacy(g, tab.Text, font,Size.Empty,eTextFormat.Default);
                        else
						    sz=TextDrawing.MeasureString(g,tab.Text,font);
						if(sz.Height>iHeight)
							iHeight=sz.Height;
					}
				}
			}
			if(this.Font.Height>iHeight)
				iHeight=this.Font.Height;
			iHeight+=4;

            if (this.IsThemed)
                iHeight += 1;

			return iHeight;
		}

		/// <summary>
		/// Returns the rectangle that contains all the tabs.
		/// </summary>
		internal Rectangle TabItemsBounds
		{
			get {return m_TabItemsBounds;}
		}

		/// <summary>
		/// Recalculates the size of the tabs.
		/// </summary>
		public void RecalcSize()
		{
			if(!BarFunctions.IsHandleValid(this))
			{
				m_NeedRecalcSize=true;
				return;
			}
			Graphics g=this.CreateGraphics();
			try
			{
                RecalcSize(g, GetTabClientArea(this.DisplayRectangle, (this.TabLayoutType == eTabLayoutType.MultilineWithNavigationBox), false));
			}
			finally
			{
				g.Dispose();
			}
		}

        private bool IsRightToLeft
        {
            get
            {
                return (this.RightToLeft == RightToLeft.Yes);
            }
        }

		private void RecalcSizeContentManager(Graphics g, Rectangle dispRect)
		{
            if (this.Tabs.Count == 0)
            {
                if (this.IsRightToLeft)
                    RecalcSizeTabSystemBox(dispRect, Rectangle.Empty);
                else
                    RecalcSizeTabSystemBox(dispRect, Rectangle.Empty);
                return;
            }
			SerialContentLayoutManager m=new SerialContentLayoutManager();
			TabItemLayoutManager blockManager=new TabItemLayoutManager(this);
            blockManager.FixedTabSize = m_FixedTabSize;
            blockManager.CloseButtonOnTabs = m_CloseButtonOnTabs;
            blockManager.CloseButtonSize = m_CloseButtonSize;
            m.RightToLeft = this.IsRightToLeft;
			if(m_Style==eTabStripStyle.RoundHeader)
			{
				m.ContentAlignment=eContentAlignment.Center;
				m.EvenHeight=true;
				m.FitContainerOversize=true;
			}
            else if (m_Style == eTabStripStyle.Office2007Document)
            {
                m.EvenHeight = true;
                if (m_Alignment == eTabStripAlignment.Bottom)
                {
                    m.ContentAlignment = eContentAlignment.Left;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    m.BlockLineAlignment = eContentVerticalAlignment.Top;
                    m.ContentOrientation = eContentOrientation.Horizontal;
                }
                else if (m_Alignment == eTabStripAlignment.Top)
                {
                    m.ContentAlignment = eContentAlignment.Left;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
                    m.BlockLineAlignment = eContentVerticalAlignment.Bottom;
                    m.ContentOrientation = eContentOrientation.Horizontal;
                }
                else if (m_Alignment == eTabStripAlignment.Left)
                {
                    m.ContentAlignment = eContentAlignment.Right;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    m.BlockLineAlignment = eContentVerticalAlignment.Top;
                    m.ContentOrientation = eContentOrientation.Vertical;
                }
                else if (m_Alignment == eTabStripAlignment.Right)
                {
                    m.ContentAlignment = eContentAlignment.Left;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    m.BlockLineAlignment = eContentVerticalAlignment.Top;
                    m.ContentOrientation = eContentOrientation.Vertical;
                }
                m.BlockSpacing = 0;
                if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
                {
                    //dispRect.X += 4;
                    dispRect.Width -= 2;
                }
                else
                {
                    //dispRect.Y += 4;
                    dispRect.Height -= 2;
                }
                blockManager.PaddingHeight = 6;
            }
            else if (m_Style == eTabStripStyle.Office2007Dock)
            {
                m.EvenHeight = true;
                if (m_Alignment == eTabStripAlignment.Bottom)
                {
                    m.ContentAlignment = eContentAlignment.Left;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    m.BlockLineAlignment = eContentVerticalAlignment.Top;
                    m.ContentOrientation = eContentOrientation.Horizontal;
                }
                else if (m_Alignment == eTabStripAlignment.Top)
                {
                    m.ContentAlignment = eContentAlignment.Left;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
                    m.BlockLineAlignment = eContentVerticalAlignment.Bottom;
                    m.ContentOrientation = eContentOrientation.Horizontal;
                }
                else if (m_Alignment == eTabStripAlignment.Left)
                {
                    m.ContentAlignment = eContentAlignment.Right;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    m.BlockLineAlignment = eContentVerticalAlignment.Top;
                    m.ContentOrientation = eContentOrientation.Vertical;
                }
                else if (m_Alignment == eTabStripAlignment.Right)
                {
                    m.ContentAlignment = eContentAlignment.Left;
                    m.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    m.BlockLineAlignment = eContentVerticalAlignment.Top;
                    m.ContentOrientation = eContentOrientation.Vertical;
                }
                m.BlockSpacing = 2;
                if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
                {
                    dispRect.X += 2;
                    dispRect.Width -= 2;
                }
                else
                {
                    dispRect.Y += 2;
                    dispRect.Height -= 2;
                }
                blockManager.PaddingHeight = 6;
            }
			else if(m_Style==eTabStripStyle.VS2005Dock)
			{
                m.EvenHeight = true;
				if(m_Alignment==eTabStripAlignment.Bottom)
				{
					m.ContentAlignment=eContentAlignment.Left;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Top;
					m.BlockLineAlignment=eContentVerticalAlignment.Top;
					m.ContentOrientation=eContentOrientation.Horizontal;
				}
				else if(m_Alignment==eTabStripAlignment.Top)
				{
					m.ContentAlignment=eContentAlignment.Left;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Bottom;
					m.BlockLineAlignment=eContentVerticalAlignment.Bottom;
					m.ContentOrientation=eContentOrientation.Horizontal;
				}
				else if(m_Alignment==eTabStripAlignment.Left)
				{
					m.ContentAlignment=eContentAlignment.Right;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Top;
					m.BlockLineAlignment=eContentVerticalAlignment.Top;
					m.ContentOrientation=eContentOrientation.Vertical;
				}
				else if(m_Alignment==eTabStripAlignment.Right)
				{
					m.ContentAlignment=eContentAlignment.Left;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Top;
					m.BlockLineAlignment=eContentVerticalAlignment.Top;
					m.ContentOrientation=eContentOrientation.Vertical;
				}
				m.BlockSpacing=8;
				if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
				{
					dispRect.X+=4;
					dispRect.Width-=8;
				}
				else
				{
					dispRect.Y+=4;
					dispRect.Height-=8;
				}
			}
			else if(m_Style==eTabStripStyle.SimulatedTheme)
			{
				if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
				{
					//dispRect.X+=3;
					dispRect.Width-=3;
				}
				else
				{
					//dispRect.Y+=3;
					dispRect.Height-=3;
				}

				if(m_Alignment==eTabStripAlignment.Bottom)
				{
					m.ContentAlignment=eContentAlignment.Left;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Top;
					m.BlockLineAlignment=eContentVerticalAlignment.Top;
					m.ContentOrientation=eContentOrientation.Horizontal;
                    m.EvenHeight = true;
				}
				else if(m_Alignment==eTabStripAlignment.Top)
				{
					m.ContentAlignment=eContentAlignment.Left;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Bottom;
					m.BlockLineAlignment=eContentVerticalAlignment.Bottom;
					m.ContentOrientation=eContentOrientation.Horizontal;
                    m.EvenHeight = true;
				}
				else if(m_Alignment==eTabStripAlignment.Left)
				{
					m.ContentAlignment=eContentAlignment.Right;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Top;
					m.BlockLineAlignment=eContentVerticalAlignment.Top;
					m.ContentOrientation=eContentOrientation.Vertical;
					m.EvenHeight=true;
				}
				else if(m_Alignment==eTabStripAlignment.Right)
				{
					m.ContentAlignment=eContentAlignment.Left;
					m.ContentVerticalAlignment=eContentVerticalAlignment.Top;
					m.BlockLineAlignment=eContentVerticalAlignment.Top;
					m.ContentOrientation=eContentOrientation.Vertical;
					m.EvenHeight=true;
				}
				blockManager.HorizontalText=true;
				//blockManager.SelectedPaddingWidth=1;
				blockManager.PaddingHeight=10;
				blockManager.PaddingWidth=2;
			}

			if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
				m.ContentOrientation=eContentOrientation.Vertical;
			else
				m.ContentOrientation=eContentOrientation.Horizontal;

			if(m_Style!=eTabStripStyle.RoundHeader)
			{
				if(m_TabLayoutType==eTabLayoutType.FitContainer)
				{
					m.FitContainerOversize=true;
                    m.OversizeDistribute = true;
					m.MultiLine=false;
				}
				else if(m_TabLayoutType==eTabLayoutType.FixedWithNavigationBox)
				{
					m.FitContainerOversize=false;
					m.MultiLine=false;
				}
				else if(m_TabLayoutType==eTabLayoutType.MultilineNoNavigationBox || m_TabLayoutType==eTabLayoutType.MultilineWithNavigationBox)
				{
					m.FitContainerOversize=false;
					m.MultiLine=true;
				}

			}

			blockManager.Graphics=g;
			TabItem[] blocks=
				new TabItem[this.Tabs.Count];
				this.Tabs.CopyTo(blocks,0);
			Rectangle tabArea=dispRect;
			if(m_ScrollOffset!=0)
			{
				if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
					tabArea.Y-=m_ScrollOffset;
				else
					tabArea.X-=m_ScrollOffset;
			}
            if (this.IsRightToLeft)
            {
                if (this.HasNavigationBox && m_TabSystemBox.Visible)
                {
                    tabArea.X += m_TabSystemBox.DefaultWidth;
                }
                
                m_TabItemsBounds = m.Layout(tabArea, blocks, blockManager);
                RecalcSizeTabSystemBox(dispRect, tabArea);
            }
            else
            {
                m_TabItemsBounds = m.Layout(tabArea, blocks, blockManager);
                RecalcSizeTabSystemBox(dispRect, m_TabItemsBounds);
            }
		}

        private bool IsContentManagedStyle
        {
            get
            {
                if ((this.Style == eTabStripStyle.RoundHeader || this.Style == eTabStripStyle.VS2005Dock) && !this.IsThemed || this.Style == eTabStripStyle.SimulatedTheme ||
                    this.Style == eTabStripStyle.Office2007Document || this.Style == eTabStripStyle.Office2007Dock)
                    return true;
                else
                    return false;
            }
        }

		private void RecalcSize(Graphics g, Rectangle dispRect)
		{
            if (IsContentManagedStyle)
                RecalcSizeContentManager(g, dispRect);
            else
                RecalcSizeLegacy(g, dispRect);

            m_NeedRecalcSize = false;
            InvokeSizeRecalculated();
		}

		private void RecalcSizeLegacy(Graphics g, Rectangle dispRect)
		{
			int iMaxWidth=dispRect.Width;
			int iWidth=0;
			int iDisplayed=0;
			int firstVisible = -1;
			eTextFormat strFormat=eTextFormat.Default;

			if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
				iMaxWidth=dispRect.Height;

			int iHeight=this.GetTabHeight(g);
			m_TabHeight=iHeight;

			int x=dispRect.X;
			int y=dispRect.Y;
			if(m_Alignment==eTabStripAlignment.Top)
			{
				y=dispRect.Bottom-iHeight;
			}
			else if(m_Alignment==eTabStripAlignment.Left)
			{
				x=dispRect.Right-iHeight;
			}

			if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
				y-=m_ScrollOffset;
			else
				x-=m_ScrollOffset;

			int initalX=x, initalY=y;
			bool bMultiLine=false;

			if(this.HasNavigationBox)
			{
				iMaxWidth-=m_TabSystemBox.DefaultWidth;
				iMaxWidth-=4;
			}
			ArrayList linePositions=new ArrayList();
			foreach(TabItem tab in m_Tabs)
			{
				int iItemWidth=4;
				if(!tab.Visible)
					continue;

				if(firstVisible<0)
					firstVisible = m_Tabs.IndexOf(tab);

				iDisplayed++;

                if (m_FixedTabSize.Width == 0)
                {
                    Image tabImage = tab.GetImage();
                    if (tab.Icon != null)
                    {
                        iItemWidth += tab.IconSize.Width;
                        if (m_Style != eTabStripStyle.OneNote || this.IsThemed)
                            iItemWidth += 4;
                    }
                    else if (tabImage != null)
                    {
                        iItemWidth += tabImage.Width;
                        if (m_Style != eTabStripStyle.OneNote || this.IsThemed)
                            iItemWidth += 4;
                    }

                    if (m_Style == eTabStripStyle.OneNote && !this.IsThemed)
                        iItemWidth += ((iHeight - 6) * 1);
                    else if (m_Style == eTabStripStyle.VS2005Document && !this.IsThemed)
                        iItemWidth += 4;
                    if (tab.Text != "" && (!m_DisplaySelectedTextOnly || tab == m_SelectedTab))
                    {
                        Font font = this.Font;
                        if (m_SelectedTabFont != null && tab == m_SelectedTab)
                            font = m_SelectedTabFont;
                        Size textSize = Size.Empty;
                        if (this.IsVertical)
                            textSize = TextDrawing.MeasureStringLegacy(g, tab.Text, font, Size.Empty, strFormat);
                        else
                            textSize = TextDrawing.MeasureString(g, tab.Text, font, 0, strFormat);
                        iItemWidth += textSize.Width;
                        if ((m_Style != eTabStripStyle.OneNote && m_Style != eTabStripStyle.VS2005Document || this.IsThemed) || m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
                            iItemWidth += 4; // 2 pixels of padding...
						if(this.IsThemed)
							iItemWidth+=10;
                    }

                    if (m_CloseButtonOnTabs && tab.CloseButtonVisible && m_Style != eTabStripStyle.OneNote)
                        iItemWidth += m_CloseButtonSize.Width;
                }
                else
                    iItemWidth = m_FixedTabSize.Width;

				// Multi-line tab support
				if(this.IsMultiLine && iWidth+iItemWidth>iMaxWidth && iDisplayed>1)
				{
					bMultiLine=true;
					switch(m_Alignment)
					{
						case eTabStripAlignment.Left:
						case eTabStripAlignment.Right:
						{
							if(linePositions.Count==0)
								linePositions.Add(x);
							x+=(iHeight+m_MultiLineSpacing);
							y=initalY;
							linePositions.Add(x);
							break;
						}
						case eTabStripAlignment.Top:
						case eTabStripAlignment.Bottom:
						{
							if(linePositions.Count==0)
								linePositions.Add(y);
							y+=(iHeight+m_MultiLineSpacing);
							x=initalX;
							linePositions.Add(y);
							break;
						}
					}
					m_TabHeight+=(iHeight+m_MultiLineSpacing);
					iWidth=0;
				}

				if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
				{
					tab._DisplayRectangle=new Rectangle(x,y,iHeight,iItemWidth);
				}
				else
				{
					tab._DisplayRectangle=new Rectangle(x,y,iItemWidth,iHeight);
				}

                if (MeasureTabItem != null)
                {
                    MeasureTabItemEventArgs mea = new MeasureTabItemEventArgs(tab, tab.DisplayRectangle.Size);
                    MeasureTabItem(this, mea);
                    tab._DisplayRectangle = new Rectangle(tab._DisplayRectangle.Location, mea.Size);
                    if (m_Alignment == eTabStripAlignment.Left || m_Alignment == eTabStripAlignment.Right)
                        iItemWidth = mea.Size.Height;
                    else
                        iItemWidth = mea.Size.Width;
                }

                if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
                    y += iItemWidth;
                else
                    x += iItemWidth;

				iWidth+=iItemWidth;
			}

			if(bMultiLine)
			{
				if(m_Alignment==eTabStripAlignment.Top)
				{
					int offset=initalY-y;
					foreach(TabItem tab in m_Tabs)
						tab._DisplayRectangle=new Rectangle(tab.DisplayRectangle.X,tab.DisplayRectangle.Y+offset,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
					for(int i=0;i<linePositions.Count;i++)
						linePositions[i]=(int)linePositions[i]+offset;
				}
				else if(m_Alignment==eTabStripAlignment.Left)
				{
					int offset=initalX-x;
					foreach(TabItem tab in m_Tabs)
						tab._DisplayRectangle=new Rectangle(tab.DisplayRectangle.X+offset,tab.DisplayRectangle.Y,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
					for(int i=0;i<linePositions.Count;i++)
						linePositions[i]=(int)linePositions[i]+offset;
				}
				// Rearrange tabs so line with the selected tab is always at the edge
				switch(m_Alignment)
				{
					case eTabStripAlignment.Left:
					{
						// Selected tab must be in last line
						if(m_SelectedTab.DisplayRectangle.X!=(int)linePositions[linePositions.Count-1])
						{
							int switchPos=m_SelectedTab.DisplayRectangle.X;
							int selectedPos=(int)linePositions[linePositions.Count-1];
							foreach(TabItem tab in m_Tabs)
							{
								if(tab.DisplayRectangle.X==selectedPos)
									tab._DisplayRectangle=new Rectangle(switchPos,tab.DisplayRectangle.Y,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
								else if(tab.DisplayRectangle.X==switchPos)
									tab._DisplayRectangle=new Rectangle(selectedPos,tab.DisplayRectangle.Y,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
							}
						}
						break;
					}
					case eTabStripAlignment.Right:
					{
						// Selected tab must be in last line
						if(m_SelectedTab.DisplayRectangle.X!=(int)linePositions[0])
						{
							int switchPos=m_SelectedTab.DisplayRectangle.X;
							int selectedPos=(int)linePositions[0];
							foreach(TabItem tab in m_Tabs)
							{
								if(tab.DisplayRectangle.X==selectedPos)
									tab._DisplayRectangle=new Rectangle(switchPos,tab.DisplayRectangle.Y,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
								else if(tab.DisplayRectangle.X==switchPos)
									tab._DisplayRectangle=new Rectangle(selectedPos,tab.DisplayRectangle.Y,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
							}
						}
						break;
					}
					case eTabStripAlignment.Top:
					{
						// Selected tab must be in last line
						if(m_SelectedTab.DisplayRectangle.Y!=(int)linePositions[linePositions.Count-1])
						{
							int switchPos=m_SelectedTab.DisplayRectangle.Y;
							int selectedPos=(int)linePositions[linePositions.Count-1];
							foreach(TabItem tab in m_Tabs)
							{
								if(tab.DisplayRectangle.Y==selectedPos)
									tab._DisplayRectangle=new Rectangle(tab.DisplayRectangle.X,switchPos,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
								else if(tab.DisplayRectangle.Y==switchPos)
									tab._DisplayRectangle=new Rectangle(tab.DisplayRectangle.X,selectedPos,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
							}
						}
						break;
					}
					case eTabStripAlignment.Bottom:
					{
						// Selected tab must be in first line
						if(m_SelectedTab.DisplayRectangle.Y!=(int)linePositions[0])
						{
							int switchPos=m_SelectedTab.DisplayRectangle.Y;
							int selectedPos=(int)linePositions[0];
							foreach(TabItem tab in m_Tabs)
							{
								if(tab.DisplayRectangle.Y==selectedPos)
									tab._DisplayRectangle=new Rectangle(tab.DisplayRectangle.X,switchPos,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
								else if(tab.DisplayRectangle.Y==switchPos)
									tab._DisplayRectangle=new Rectangle(tab.DisplayRectangle.X,selectedPos,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
							}
						}
						break;
					}
				}
			}
			
			if(this.HasNavigationBox)
			{
				if(m_ScrollOffset!=0)
					m_TabSystemBox.BackEnabled=true;
				else
					m_TabSystemBox.BackEnabled=false;

				if(iWidth-m_ScrollOffset>iMaxWidth)
					m_TabSystemBox.ForwardEnabled=true;
				else
					m_TabSystemBox.ForwardEnabled=false;

				if(m_AutoHideSystemBox && !this.IsMultiLine)
				{
					if(iWidth>iMaxWidth)
						m_TabSystemBox.Visible=true;
					else
						m_TabSystemBox.Visible=false;
				}
				else
					m_TabSystemBox.Visible=true;

				Rectangle rSysBox=Rectangle.Empty;
				switch(m_Alignment)
				{
					case eTabStripAlignment.Left:
					{
						rSysBox=new Rectangle((firstVisible<0?dispRect.X:m_Tabs[firstVisible].DisplayRectangle.X),this.ClientRectangle.Bottom-m_TabSystemBox.DefaultWidth-3,(firstVisible<0?dispRect.Width:m_Tabs[firstVisible].DisplayRectangle.Width),m_TabSystemBox.DefaultWidth);
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.X--;
						break;
					}
					case eTabStripAlignment.Right:
					{
						rSysBox=new Rectangle(dispRect.X,this.ClientRectangle.Bottom-m_TabSystemBox.DefaultWidth-3,(firstVisible<0?dispRect.Width:m_Tabs[firstVisible].DisplayRectangle.Width),m_TabSystemBox.DefaultWidth);
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.X++;
						break;
					}
					case eTabStripAlignment.Top:
					{
                        if(this.IsRightToLeft)
                            rSysBox = new Rectangle(this.ClientRectangle.Left, (firstVisible<0 ? dispRect.Y : m_Tabs[firstVisible].DisplayRectangle.Y), m_TabSystemBox.DefaultWidth, (firstVisible<0 ? dispRect.Height : m_Tabs[firstVisible].DisplayRectangle.Height));
                        else
						    rSysBox=new Rectangle(this.ClientRectangle.Right-m_TabSystemBox.DefaultWidth-3,(firstVisible<0?dispRect.Y:m_Tabs[firstVisible].DisplayRectangle.Y),m_TabSystemBox.DefaultWidth,(firstVisible<0?dispRect.Height:m_Tabs[firstVisible].DisplayRectangle.Height));
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.Y--;
						break;
					}
					default:
					{
                        if (this.IsRightToLeft)
                            rSysBox = new Rectangle(this.ClientRectangle.Left, dispRect.Y, m_TabSystemBox.DefaultWidth + 2, (firstVisible<0 ? dispRect.Height : m_Tabs[firstVisible].DisplayRectangle.Height));
                        else
						    rSysBox=new Rectangle(this.ClientRectangle.Right-m_TabSystemBox.DefaultWidth-2,dispRect.Y,m_TabSystemBox.DefaultWidth+2,(firstVisible<0?dispRect.Height:m_Tabs[firstVisible].DisplayRectangle.Height));
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.Y++;
						break;
					}
				}
				
				m_TabSystemBox.DisplayRectangle=rSysBox;
			}

			if(iWidth>iMaxWidth && m_TabLayoutType==eTabLayoutType.FitContainer && !this.IsMultiLine)
			{
				if(iDisplayed>0)
				{
					if(m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right)
					{
						x=dispRect.X;
						y=dispRect.Y;
						if(m_Alignment==eTabStripAlignment.Left)
							x=dispRect.Right-iHeight;
						int iItemWidth=iMaxWidth/iDisplayed;
						int iOverageCount=0;
						foreach(TabItem tab in m_Tabs)
						{
							if(!tab.Visible)
								continue;
							if(tab.DisplayRectangle.Height>iItemWidth)
								iOverageCount++;
							else
							{
								iMaxWidth-=tab.DisplayRectangle.Height;
								iWidth-=tab.DisplayRectangle.Height;
							}
						}
						//int iDiff=(iWidth-iMaxWidth)/iOverageCount;
						float fDiff=(float)iMaxWidth/(float)iWidth;
						foreach(TabItem tab in m_Tabs)
						{
							if(!tab.Visible)
								continue;
							if(tab.DisplayRectangle.Height>iItemWidth)
							{
								if(iOverageCount==iDisplayed)
									tab._DisplayRectangle=new Rectangle(x,y,tab.DisplayRectangle.Width,iItemWidth);
								else
									tab._DisplayRectangle=new Rectangle(x,y,tab.DisplayRectangle.Width,(int)(tab.DisplayRectangle.Height*fDiff));
							}
							else
								tab._DisplayRectangle=new Rectangle(x,y,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
							y+=tab.DisplayRectangle.Height;
						}
					}
					else
					{
						x=dispRect.X;
						int iItemWidth=iMaxWidth/iDisplayed;
						int iOverageCount=0;
						foreach(TabItem tab in m_Tabs)
						{
							if(!tab.Visible)
								continue;
							if(tab.DisplayRectangle.Width>iItemWidth)
								iOverageCount++;
							else
							{
								iMaxWidth-=tab.DisplayRectangle.Width;
								iWidth-=tab.DisplayRectangle.Width;
							}
						}
						//int iDiff=(iWidth-iMaxWidth)/iOverageCount;
						float fDiff=(float)iMaxWidth/(float)iWidth;
						foreach(TabItem tab in m_Tabs)
						{
							if(!tab.Visible)
								continue;
							if(tab.DisplayRectangle.Width>iItemWidth)
							{
								if(iOverageCount==iDisplayed)
									tab._DisplayRectangle=new Rectangle(x,y,iItemWidth,tab.DisplayRectangle.Height);
								else
									tab._DisplayRectangle=new Rectangle(x,y,(int)(tab.DisplayRectangle.Width*fDiff),tab.DisplayRectangle.Height);
							}
							else
								tab._DisplayRectangle=new Rectangle(x,y,tab.DisplayRectangle.Width,tab.DisplayRectangle.Height);
							x+=tab.DisplayRectangle.Width;
						}
					}
				}
			}
            else if (this.IsRightToLeft && m_TabLayoutType != eTabLayoutType.FitContainer &&
                (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom))
            {
                if (iWidth > iMaxWidth)
                {
                    for (int i = m_Tabs.Count - 1; i >= 0; i--)
                    {
                        if (m_Tabs[i].Visible)
                        {
                            x = m_Tabs[i].DisplayRectangle.Right;
                            break;
                        }
                    }
                    if (m_TabSystemBox.Visible)
                        x += m_TabSystemBox.DisplayRectangle.Right;
                }
                else
                    x = dispRect.Right;
                foreach (TabItem tab in m_Tabs)
                {
                    if (tab.Visible)
                    {
                        x -= tab.DisplayRectangle.Width;
                        tab._DisplayRectangle = new Rectangle(x, tab.DisplayRectangle.Y, tab.DisplayRectangle.Width, tab.DisplayRectangle.Height);
                    }
                }
            }
		}

        internal bool HasMeasureTabItem
        {
            get
            {
                return MeasureTabItem != null;
            }
        }

        internal void InvokeMeasureTabItem(MeasureTabItemEventArgs e)
        {
            if (MeasureTabItem != null)
                MeasureTabItem(this, e);
        }

        internal bool HasPreRenderTabItem
        {
            get { return PreRenderTabItem != null; }
        }

        internal bool HasPostRenderTabItem
        {
            get { return PostRenderTabItem != null; }
        }

        internal void InvokePreRenderTabItem(RenderTabItemEventArgs e)
        {
            if (PreRenderTabItem != null)
                PreRenderTabItem(this, e);
        }

        internal void InvokePostRenderTabItem(RenderTabItemEventArgs e)
        {
            if (PostRenderTabItem != null)
                PostRenderTabItem(this, e);
        }

        private TabItem GetFirstVisibleTabItem()
        {
            foreach (TabItem tab in m_Tabs)
            {
                if (tab.Visible) return tab;
            }
            return null;
        }

        internal void RecalcSizeTabSystemBox(Rectangle displayRect, Rectangle tabsBounds)
		{
			if(this.HasNavigationBox)
			{
                Rectangle absTabBounds = tabsBounds;
                
                if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
                    absTabBounds.Offset(m_ScrollOffset, 0);
                else
                    absTabBounds.Offset(0, m_ScrollOffset);

				bool bTabSystemBoxVisible=false;
                if (absTabBounds.Right > (displayRect.Right - (m_AutoHideSystemBox ? 0 : m_TabSystemBox.DefaultWidth)) && (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom) ||
                    absTabBounds.Bottom > (displayRect.Bottom - (m_AutoHideSystemBox ? 0 : m_TabSystemBox.DefaultWidth)) && (m_Alignment == eTabStripAlignment.Right || m_Alignment == eTabStripAlignment.Left))
					bTabSystemBoxVisible=true;

				if(m_ScrollOffset!=0)
					m_TabSystemBox.BackEnabled=true;
				else
					m_TabSystemBox.BackEnabled=false;

				if(m_AutoHideSystemBox && !this.IsMultiLine)
					m_TabSystemBox.Visible=bTabSystemBoxVisible;
				else
					m_TabSystemBox.Visible=true;

				if(tabsBounds.Right+m_ScrollOffset>displayRect.Right-(m_TabSystemBox.Visible?m_TabSystemBox.DefaultWidth:0) && (m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom) ||
					tabsBounds.Bottom+m_ScrollOffset>displayRect.Bottom-(m_TabSystemBox.Visible?m_TabSystemBox.DefaultWidth:0) && (m_Alignment==eTabStripAlignment.Right || m_Alignment==eTabStripAlignment.Left))
					m_TabSystemBox.ForwardEnabled=true;
				else
					m_TabSystemBox.ForwardEnabled=false;

                TabItem firstVisible = GetFirstVisibleTabItem();

				Rectangle rSysBox=Rectangle.Empty;
				switch(m_Alignment)
				{
					case eTabStripAlignment.Left:
					{
                        rSysBox = new Rectangle((firstVisible == null ? displayRect.X : firstVisible.DisplayRectangle.X), this.ClientRectangle.Bottom - m_TabSystemBox.DefaultWidth - 3, (firstVisible == null ? displayRect.Width : firstVisible.DisplayRectangle.Width), m_TabSystemBox.DefaultWidth);
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.X--;
						break;
					}
					case eTabStripAlignment.Right:
					{
                        rSysBox = new Rectangle(displayRect.X, this.ClientRectangle.Bottom - m_TabSystemBox.DefaultWidth - 3, (firstVisible == null ? displayRect.Width : firstVisible.DisplayRectangle.Width), m_TabSystemBox.DefaultWidth);
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.X++;
						break;
					}
					case eTabStripAlignment.Top:
					{
                        if(this.IsRightToLeft)
                            rSysBox = new Rectangle(displayRect.X, (firstVisible == null ? displayRect.Y : firstVisible.DisplayRectangle.Y), m_TabSystemBox.DefaultWidth, (firstVisible == null ? displayRect.Height : firstVisible.DisplayRectangle.Height));
                        else
                            rSysBox = new Rectangle(this.ClientRectangle.Right - m_TabSystemBox.DefaultWidth - 3, (firstVisible == null ? displayRect.Y : firstVisible.DisplayRectangle.Y), m_TabSystemBox.DefaultWidth, (firstVisible == null ? displayRect.Height : firstVisible.DisplayRectangle.Height));
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.Y--;
						break;
					}
					default:
					{
                        if (this.IsRightToLeft)
                            rSysBox = new Rectangle(displayRect.X, displayRect.Y, m_TabSystemBox.DefaultWidth + 2, (firstVisible == null ? displayRect.Height : firstVisible.DisplayRectangle.Height));
                        else
                            rSysBox = new Rectangle(this.ClientRectangle.Right - m_TabSystemBox.DefaultWidth - 2, displayRect.Y, m_TabSystemBox.DefaultWidth + 2, (firstVisible == null ? displayRect.Height : firstVisible.DisplayRectangle.Height));
						if(m_Style==eTabStripStyle.VS2005 && !this.IsThemed) rSysBox.Y++;
						break;
					}
				}
				
				m_TabSystemBox.DisplayRectangle=rSysBox;
			}
		}

		private void InvokeSizeRecalculated()
		{
			if(SizeRecalculated!=null)
				SizeRecalculated(this,new EventArgs());

			if(this.IsMultiLine && !(this.Parent is TabControl))
			{
				switch(this.TabAlignment)
				{
					case eTabStripAlignment.Left:
					case eTabStripAlignment.Right:
						this.Width=this.MinTabStripHeight;
						break;
					default:
						this.Height=this.MinTabStripHeight;
						break;
				}
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public TabItem DesignTimeSelection
		{
			get {return m_DesignTimeSelection;}
			set
			{
				m_DesignTimeSelection=value;
				this.Invalidate();
			}
		}
		
		private void RefreshThemes()
		{
			if(m_ThemeTab!=null)
				m_ThemeTab.Dispose();

			m_ThemeTab=new ThemeTab(this);
		}
		private void DisposeThemes()
		{
			if(m_ThemeTab!=null)
			{
				m_ThemeTab.Dispose();
				m_ThemeTab=null;
			}
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_THEMECHANGED)
			{
				Themes.RefreshIsThemeActive();
				this.RefreshThemes();
			}
			base.WndProc(ref m);
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeThemes();
			base.OnHandleDestroyed(e);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if(m_NeedRecalcSize)
			{
				this.RecalcSize();
				if(this.Parent is TabControl)
					((TabControl)this.Parent).SyncTabStripSize();
			}
			RefreshMdiTabItems();
		}

		internal void OnSelectedTabRemoved(int index, TabItem item)
		{
			if(index>=this.Tabs.Count)
				index=this.Tabs.Count-1;
            int selectedIndex = -1;
            for (int i = index; i < this.Tabs.Count; i++)
            {
                if (this.Tabs[i].Visible)
                {
                    selectedIndex = i;
                    break;
                }
            }
            if (selectedIndex == -1)
            {
                for (int i = 0; i < this.Tabs.Count; i++)
                {
                    if (this.Tabs[i].Visible)
                    {
                        selectedIndex = i;
                        break;
                    }
                }
            }
            if (selectedIndex >= 0)
                this.SelectedTab = this.Tabs[selectedIndex];
			else
				m_SelectedTab=null;
		}

		internal void InvokeTabRemoved(TabItem tab)
		{
			if(m_MovingTab)
				return;
			HideToolTip();
			if(TabRemoved!=null)
				TabRemoved(tab,new EventArgs());
			if(this.VisibleTabCount==0)
				this.SelectedTab=null;
		}

		/// <summary>
		/// Gets or sets the index of currently selected tab.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedTabIndex
		{
			get
			{
				if(m_SelectedTab==null)
					return -1;
				return m_Tabs.IndexOf(m_SelectedTab);
			}
			set
			{
				if(value<0 || value>=m_Tabs.Count || m_Tabs.Count==0)
					return;
				this.SelectedTab=m_Tabs[value];
			}
		}

		/// <summary>
		/// Gets or sets the selected tab.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates selected tab.")]
		public TabItem SelectedTab
		{
			get
			{
				if(m_SelectedTab==null && m_Tabs.Count>0)
					m_SelectedTab=m_Tabs[0] as TabItem;
				return m_SelectedTab;
			}
			set
			{
				SelectTab(value,eEventSource.Code);
			}
		}

		private void SelectTab(TabItem value, eEventSource eventSource)
		{
			if(m_SelectedTab==value)
				return;
			TabItem oldTab=m_SelectedTab;

			if(SelectedTabChanging!=null)
			{
                TabStripTabChangingEventArgs eventData = new TabStripTabChangingEventArgs(m_SelectedTab, value, eventSource);
				SelectedTabChanging(this,eventData);
				if(eventData.Cancel)
					return;
			}

			InvokeBeforeTabDisplay(value);

			Control hideControl=null;

			if(m_SelectedTab!=null && m_SelectedTab.AttachedItem!=null)
				m_SelectedTab.AttachedItem.Displayed=false;
			else if(m_SelectedTab!=null && m_SelectedTab.AttachedControl!=null)
			{
				if(!m_MdiTabbedDocuments)
					hideControl=m_SelectedTab.AttachedControl;
			}
					
			m_SelectedTab=value;
			m_NeedRecalcSize=true;

			EnsureVisible(m_SelectedTab);

			if(m_SelectedTab!=null && m_SelectedTab.AttachedItem!=null)
			{
				m_SelectedTab.AttachedItem.Displayed=true;
				if(m_SelectedTab.AttachedItem is DockContainerItem && ((DockContainerItem)m_SelectedTab.AttachedItem).Control!=null)
				{
                    if(m_AutoSelectAttachedControl)
					    ((DockContainerItem)m_SelectedTab.AttachedItem).Control.Select();
				}
			}
			else if(m_SelectedTab!=null && m_SelectedTab.AttachedControl!=null)
			{
				if(!m_MdiTabbedDocuments)
				{
					//if(!(this.IsDesignMode && m_SelectedTab.AttachedControl is TabControlPanel))
						m_SelectedTab.AttachedControl.Visible=true;
                        if (m_AutoSelectAttachedControl && !(this.TabStop && this.Parent is TabControl && this.Focused && eventSource == eEventSource.Keyboard))
                            m_SelectedTab.AttachedControl.Select();
					if(hideControl==m_SelectedTab.AttachedControl)
						hideControl=null;
				}
				else if(m_SelectedTab.AttachedControl is Form)
				{
					MdiClient client=this.GetMdiClient(m_MdiForm);
					bool bSetRedraw=false;
					Form oldActive=m_MdiForm.ActiveMdiChild;

					if(client!=null && m_MdiNoFormActivateFlicker && m_SelectedTab.AttachedControl is Form && (((Form)m_SelectedTab.AttachedControl).WindowState==FormWindowState.Maximized || oldActive!=null && oldActive.WindowState==FormWindowState.Maximized))
					{
						NativeFunctions.SendMessage(client.Handle,NativeFunctions.WM_SETREDRAW,0,0);
						bSetRedraw=true;
					}

					((Form)m_SelectedTab.AttachedControl).Activate();
					//if(m_TabSystemBox!=null)
					//	m_TabSystemBox.CloseVisible=((Form)m_SelectedTab.AttachedControl).ControlBox;

					if(bSetRedraw)
					{
						NativeFunctions.SendMessage(client.Handle,NativeFunctions.WM_SETREDRAW,1,0);
						client.Refresh();
					}

					if(oldActive!=null && oldActive.WindowState==FormWindowState.Normal)
					{
						if(BarFunctions.ThemedOS)
						{
							// This will force repaint of Title bar since there were some repainting issues with it...
                            string s = oldActive.Text; // GetFormText(oldActive);
							oldActive.Text=s+" ";
							oldActive.Text=s;
						}
					}
				}
			}
			if(hideControl!=null)
			{
				//if(!(this.IsDesignMode && hideControl is TabControlPanel))
					hideControl.Visible=false;
			}
			this.Invalidate();
			
			if(SelectedTabChanged!=null)
			{
                TabStripTabChangedEventArgs eventData = new TabStripTabChangedEventArgs(oldTab, m_SelectedTab, eventSource);
				SelectedTabChanged(this,eventData);
			}
		}

		/// <summary>
		/// Gets or sets whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Behavior"),Description("Indicates whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.")]
		public virtual bool TabScrollAutoRepeat
		{
			get
			{
				return m_TabScrollAutoRepeat;
			}
			set
			{
				m_TabScrollAutoRepeat=value;                
			}
		}

		/// <summary>
		/// Gets or sets the auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(300),Category("Behavior"),Description("Indicates auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.")]
		public virtual int TabScrollRepeatInterval
		{
			get
			{
				return m_TabScrollRepeatInterval;
			}
			set
			{
				m_TabScrollRepeatInterval=value;                
			}
		}

		/// <summary>
		/// Gets or sets whether the Close button that closes the active tab is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Close Button"),Description("Indicates whether the Close button that closes the active tab is visible.")]
		public bool CloseButtonVisible
		{
			get
			{
				if(m_TabSystemBox!=null)
					return m_TabSystemBox.CloseVisible;
				return true;
			}
			set
			{
				if(m_TabSystemBox!=null)
					m_TabSystemBox.CloseVisible=value;
				this.Invalidate();
			}
		}

        /// <summary>
        /// Gets or sets whether close button is visible on each tab instead of in system box.
        /// </summary>
        [Browsable(true), DefaultValue(false), DevCoBrowsable(true), Category("Close Button"), Description("Indicates whether close button is visible on each tab instead of in system box.")]
        public bool CloseButtonOnTabsVisible
        {
            get
            {
                return m_CloseButtonOnTabs;
            }
            set
            {
                m_CloseButtonOnTabs = value;
                m_NeedRecalcSize = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether close button on tabs when visible is displayed for every tab state. Default value is true. When set to false
        /// the close button will be displayed only for selected and tab that mouse is currently over.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoBrowsable(true), Category("Close Button"), Description("Indicates whether close button on tabs when visible is displayed for every tab state.")]
        public bool CloseButtonOnTabsAlwaysDisplayed
        {
            get
            {
                return m_CloseButtonOnTabsAlwaysDisplayed;
            }
            set
            {
                m_CloseButtonOnTabsAlwaysDisplayed = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the position of the close button displayed on each tab. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eTabCloseButtonPosition.Left), DevCoBrowsable(true), Category("Close Button"), Description("Indicates position of the close button displayed on each tab.")]
        public eTabCloseButtonPosition CloseButtonPosition
        {
            get { return m_CloseButtonPosition; }
            set
            {
                m_CloseButtonPosition = value;
                m_NeedRecalcSize = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets custom image that is used on tabs as Close button that allows user to close the tab.
        /// Use TabCloseButtonHot property to specify image that is used when mouse is over the close button. Note that image size must
        /// be same for both images.
        /// Default value is null
        /// which means that internal representation of close button is used.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Close Button"), Description("Indicates custom image that is used on tabs as Close button that allows user to close the tab.")]
        public Image TabCloseButtonNormal
        {
            get { return m_TabCloseButtonNormal; }
            set
            {
                m_TabCloseButtonNormal = value;
                if (m_TabCloseButtonNormal != null)
                    m_CloseButtonSize = m_TabCloseButtonNormal.Size;
                else
                    m_CloseButtonSize = default_close_button_size;
                m_NeedRecalcSize = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets custom image that is used on tabs as Close button whem mouse is over the close button.
        /// To use this property you must set TabCloseButtonNormal as well. Note that image size for both images must be same.
        /// Default value is null which means that internal representation of close button is used.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Close Button"), Description("Indicates custom image that is used on tabs as Close button whem mouse is over the close button.")]
        public Image TabCloseButtonHot
        {
            get { return m_TabCloseButtonHot; }
            set
            {
                m_TabCloseButtonHot = value;
            }
        }

		/// <summary>
		/// Gets the collection of all tabs.
		/// </summary>
        [Editor("DevComponents.DotNetBar.Design.TabStripTabsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Data"), Description("Returns the collection of Tabs.")]
		public TabsCollection Tabs
		{
			get{return m_Tabs;}
		}

        /// <summary>
        /// Gets or sets whether control attached to the TabItem.AttachedControl property is automatically selected when TabItem becomes selected tab. Default value is true.
        /// </summary>
        [Browsable(false)]
        public bool AutoSelectAttachedControl
        {
            get { return m_AutoSelectAttachedControl; }
            set { m_AutoSelectAttachedControl = value; }
        }

		/// <summary>
		/// Gets or sets the image list used by tab items.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return m_ImageList;
			}
			set
			{
				m_ImageList=value;
			}
		}

		/// <summary>
		/// Gets or sets the type of the tab layout.
		/// </summary>
		[Browsable(true),DefaultValue(eTabLayoutType.FitContainer),Category("Appearance"),Description("Indicates the type of the tab layout.")]
		public eTabLayoutType TabLayoutType
		{
			get {return m_TabLayoutType;}
			set
			{
				m_TabLayoutType=value;
				m_NeedRecalcSize=true;
				m_TabSystemBox.DisplayRectangle=Rectangle.Empty;
				OnTabStyleChanged();
				//this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets whether tab size is adjusted to fit the available control size.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Obsolete("Please use TabLayoutType property instead.",true)]
		public bool VariableTabWidth
		{
			get {return m_VariableTabWidth;}
			set
			{
				if(value)
					m_TabLayoutType=eTabLayoutType.FitContainer;
				else
                    m_TabLayoutType=eTabLayoutType.FixedWithNavigationBox;
			}
		}

		/// <summary>
		/// Gets or sets scrolling offset of the first tab. You can use this property to programmatically scroll the tab strip.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(0),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollOffset
		{
			get {return m_ScrollOffset;}
			set
			{
				if(m_ScrollOffset!=value)
				{
					m_ScrollOffset=value;
					m_NeedRecalcSize=true;
					this.Invalidate();
				}
			}
		}

		internal bool IsDesignMode
		{
			get
			{
				if(this.Parent!=null && this.Parent is TabControl && this.Parent.Site!=null)
					return this.Parent.Site.DesignMode;
				return this.DesignMode;
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabSystemBox _TabSystemBox
		{
			get {return m_TabSystemBox;}
		}

        private bool _KeyboardNavigationEnabled = true;
        /// <summary>
        /// Gets or sets whether keyboard navigation using Left and Right arrow keys to select tabs is enabled. Default value is true.
        /// </summary>
        [Category("Behavior"), DefaultValue(true), Description("Indicates whether keyboard navigation using Left and Right arrow keys to select tabs is enabled.")]
        public bool KeyboardNavigationEnabled
        {
            get { return _KeyboardNavigationEnabled; }
            set
            {
                _KeyboardNavigationEnabled = value;
            }
        }

		protected override bool ProcessCmdKey(ref Message msg,Keys keyData)
		{
            if (_KeyboardNavigationEnabled)
            {
                if (keyData == Keys.Left)
                {
                    if (SelectPreviousTab(eEventSource.Keyboard, false))
                        return true;
                }
                else if (keyData == Keys.Right)
                {
                    if (SelectNextTab(eEventSource.Keyboard, false))
                        return true;
                }
            }
			return base.ProcessCmdKey(ref msg,keyData);
		}
		/// <summary>
		///     Selectes previous visible tab. Returns true if previous tab was found for selection.
		/// </summary>
		public bool SelectPreviousTab()
		{
			return SelectPreviousTab(eEventSource.Code, false);
		}
		internal bool SelectPreviousTab(eEventSource eventSource, bool cycle)
		{
			if(this.SelectedTab!=null && this.Tabs.IndexOf(this.SelectedTab)>0)
			{
				for(int i=this.Tabs.IndexOf(this.SelectedTab)-1;i>=0;i--)
				{
					if(this.Tabs[i].Visible)
					{
						this.SelectTab(this.Tabs[i],eventSource);
						return true;
					}
				}
			}
            if (cycle)
            {
                for (int i = this.Tabs.Count-1; i >= 0; i--)
                {
                    if (this.Tabs[i].Visible)
                    {
                        this.SelectTab(this.Tabs[i], eventSource);
                        return true;
                    }
                }
            }
			return false;
		}
		/// <summary>
		///     Selectes next visible tab. Returns true if next tab was found for selection.
		/// </summary>
		public bool SelectNextTab()
		{
			return SelectNextTab(eEventSource.Code, false);
		}
		internal bool SelectNextTab(eEventSource eventSource, bool cycle)
		{
			if(this.SelectedTab!=null && this.Tabs.IndexOf(this.SelectedTab)<this.Tabs.Count)
			{
				for(int i=this.Tabs.IndexOf(this.SelectedTab)+1;i<this.Tabs.Count;i++)
				{
					if(this.Tabs[i].Visible)
					{
						this.SelectTab(this.Tabs[i],eventSource);
						return true;
					}
				}
			}
            if (cycle)
            {
                // Find first visible tab and select it
                for (int i = 0; i < this.Tabs.Count; i++)
                {
                    if (this.Tabs[i].Visible)
                    {
                        this.SelectTab(this.Tabs[i], eventSource);
                        return true;
                    }
                }
            }
			return false;
		}

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            // Invoke TabItem events.
            Point p = this.PointToClient(Control.MousePosition);

			if(this.HasNavigationBox)
			{
				if(m_TabSystemBox.Visible && m_TabSystemBox.DisplayRectangle.Contains(p.X,p.Y))
					return;
			}

            TabItem tabAt = HitTest(p.X, p.Y);
            if (tabAt != null)
            {
                tabAt.InvokeClick(e);
            }
        }

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			InternalOnMouseDown(e);
		}

		internal bool HasNavigationBox
		{
			get {return (m_TabLayoutType==eTabLayoutType.FixedWithNavigationBox || m_TabLayoutType==eTabLayoutType.MultilineWithNavigationBox);}
		}

		internal bool IsMultiLine
		{
			get {return (m_TabLayoutType==eTabLayoutType.MultilineNoNavigationBox || m_TabLayoutType==eTabLayoutType.MultilineWithNavigationBox);}
		}

        private Point _MouseDownPoint = Point.Empty;
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void InternalOnMouseDown(MouseEventArgs e)
		{
			HideToolTip();
			if(this.HasNavigationBox && m_TabSystemBox.Visible && m_TabSystemBox.DisplayRectangle.Contains(e.X,e.Y))
			{
				m_TabSystemBox.MouseDown(e);
				return;
			}
            _MouseDownPoint = new Point(e.X, e.Y);
			if(e.Button==MouseButtons.Left)
			{
				Rectangle rSelected=this.GetSelectedTabRectangle();
				foreach(TabItem tab in m_Tabs)
				{
					if((tab.DisplayRectangle.Contains(new Point(e.X,e.Y)) || rSelected.Contains(e.X,e.Y)) && tab.Visible)
					{
						TabItem newSelected=tab;
						if(rSelected.Contains(e.X,e.Y))
							newSelected=m_SelectedTab;
                        
						if(m_SelectedTab!=newSelected)
						{
							TabItem oldTab=m_SelectedTab;
							if(SelectedTabChanging!=null)
							{
								TabStripTabChangingEventArgs eventData=new TabStripTabChangingEventArgs(m_SelectedTab,newSelected,eEventSource.Mouse);
								SelectedTabChanging(this,eventData);
								if(eventData.Cancel)
									break;
							}
							
							this.InvokeBeforeTabDisplay(newSelected);
							
							if(m_SelectedTab.AttachedItem is DockContainerItem && ((DockContainerItem)m_SelectedTab.AttachedItem).Control!=null && ((DockContainerItem)m_SelectedTab.AttachedItem).Control.Focused)
							{
								this.Focus();
							}
							if(m_SelectedTab.AttachedItem!=null)
								m_SelectedTab.AttachedItem.Displayed=false;
							else if(m_SelectedTab.AttachedControl!=null && !m_MdiTabbedDocuments)
							{
								//if(!(this.IsDesignMode && m_SelectedTab.AttachedControl is TabControlPanel))
									m_SelectedTab.AttachedControl.Visible=false;
							}
							m_SelectedTab=newSelected;
							m_NeedRecalcSize=true;
							EnsureVisible(m_SelectedTab);
							if(m_SelectedTab.AttachedItem!=null)
							{
								m_SelectedTab.AttachedItem.Displayed=true;
								if(m_SelectedTab.AttachedItem is DockContainerItem && ((DockContainerItem)m_SelectedTab.AttachedItem).Control!=null)
								{
                                    if(m_AutoSelectAttachedControl)
									    ((DockContainerItem)m_SelectedTab.AttachedItem).Control.Select();
								}
							}
							else if(m_SelectedTab.AttachedControl!=null)
							{
								if(!m_MdiTabbedDocuments)
								{
									//if(!(this.IsDesignMode && m_SelectedTab.AttachedControl is TabControlPanel))
										m_SelectedTab.AttachedControl.Visible=true;
                                    if (m_AutoSelectAttachedControl)
										m_SelectedTab.AttachedControl.Select();

								}
								else if(m_SelectedTab.AttachedControl is Form)
								{
									MdiClient client=this.GetMdiClient(m_MdiForm);
									bool bSetRedraw=false;
									Form oldActive=m_MdiForm.ActiveMdiChild;

									if(client!=null && m_MdiNoFormActivateFlicker && m_SelectedTab.AttachedControl is Form && (((Form)m_SelectedTab.AttachedControl).WindowState==FormWindowState.Maximized || oldActive!=null && oldActive.WindowState==FormWindowState.Maximized))
									{
										NativeFunctions.SendMessage(client.Handle,NativeFunctions.WM_SETREDRAW,0,0);
										bSetRedraw=true;
									}

									((Form)m_SelectedTab.AttachedControl).Activate();
									//if(m_TabSystemBox!=null)
									//	m_TabSystemBox.CloseVisible=((Form)m_SelectedTab.AttachedControl).ControlBox;

									if(bSetRedraw)
									{
										NativeFunctions.SendMessage(client.Handle,NativeFunctions.WM_SETREDRAW,1,0);
										client.Refresh();
									}

									if(oldActive!=null && oldActive.WindowState==FormWindowState.Normal)
									{
										if(BarFunctions.ThemedOS)
										{
											// This will force repaint of Title bar since there were some repainting issues with it...
                                            string s = oldActive.Text; // GetFormText(oldActive);
											oldActive.Text=s+" ";
											oldActive.Text=s;
										}
									}
								}
							}

							this.Invalidate();

							if(SelectedTabChanged!=null)
							{
								TabStripTabChangedEventArgs eventData=new TabStripTabChangedEventArgs(oldTab,m_SelectedTab,eEventSource.Mouse);
								SelectedTabChanged(this,eventData);
							}
						}
                        else if (m_SelectedTab != null && m_SelectedTab.AttachedItem is DockContainerItem && m_SelectedTab.AttachedItem.ContainerControl is Bar && ((Bar)m_SelectedTab.AttachedItem.ContainerControl).DockSide == eDockSide.Document &&
                            ((DockContainerItem)m_SelectedTab.AttachedItem).Control != null)
                        {
                            if (m_AutoSelectAttachedControl)
                                ((DockContainerItem)m_SelectedTab.AttachedItem).Control.Select();
                        }
                        
						break;
					}
				}
			}

            // Invoke TabItem events.
            TabItem tabAt = HitTest(e.X, e.Y);
            if (tabAt != null)
            {
                tabAt.InvokeMouseDown(e);
            }
		}

		private void InvalidateHotTab()
		{
			if(m_HotTab==null)
				return;
            if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
			{
				Rectangle r=m_HotTab.DisplayRectangle;
				if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
				{
					r.X-=r.Height;
					r.Width+=r.Height*2;
					r.Height+=2;
				}
				else
				{
					r.Y-=r.Width;
					r.Height+=r.Width*2;
					r.Width+=2;
				}

				this.Invalidate(r);
			}
            else if (m_Style == eTabStripStyle.Office2007Document)
            {
                Rectangle r = m_HotTab.DisplayRectangle;
                if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
                {
                    r.Width += r.Height * 2;
                    r.Height += 2;
                }
                else
                {
                    r.Height += r.Width * 2;
                    r.Width += 2;
                    r.X--;
                }

                this.Invalidate(r);
            }
            else if (m_Style == eTabStripStyle.VS2005Dock)
            {
                Rectangle r = m_HotTab.DisplayRectangle;
                r.Inflate(6, 0);
                this.Invalidate(r);
            }
            else
            {
                Rectangle r = m_HotTab.DisplayRectangle;
                r.Inflate(2, 2);
                this.Invalidate(r);
            }
		}

		private Rectangle GetSelectedTabRectangle()
		{
			if(this.SelectedTab==null)
				return Rectangle.Empty;
			Rectangle r=this.SelectedTab.DisplayRectangle;
            if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
			{
				switch(m_Alignment)
				{
					case eTabStripAlignment.Top:
					{
                        r.X -= (r.Height - 6);
                        r.Width += (r.Height - 6);
						break;
					}
					case eTabStripAlignment.Bottom:
					{
                        r.X -= (r.Height - 6);
                        r.Width += (r.Height - 6);
						break;
					}
					case eTabStripAlignment.Left:
					{
                        r.Height += (r.Width - 6);
						break;
					}
					case eTabStripAlignment.Right:
					{
                        r.Y -= (r.Width - 6);
                        r.Height += (r.Width - 6);
						break;
					}
				}
			}

			return r;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.InternalOnMouseMove(e);
		}

		private bool m_MovingTab=false;
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void InternalOnMouseMove(MouseEventArgs e)
		{
			if(m_TooltipTab!=null)
			{
				if(!m_TooltipTab.DisplayRectangle.Contains(e.X,e.Y))
				{
					HideToolTip();
					ResetHover();
				}
			}

            if (m_HotTab != null)
            {
                if (m_HotTab.DisplayRectangle.Contains(e.X, e.Y))
                    m_HotTab.InvokeMouseMove(e);
                else
                {
                    m_HotTab.InvokeMouseLeave(e);
                    TabItem mouseOverTab = HitTest(e.X, e.Y);
                    if (mouseOverTab != null)
                    {
                        mouseOverTab.InvokeMouseEnter(e);
                        ResetHover();
                    }
                }
            }
            else
            {
                TabItem mouseOverTab = HitTest(e.X, e.Y);
                if (mouseOverTab != null)
                {
                    mouseOverTab.InvokeMouseEnter(e);
                    ResetHover();
                }
            }

			if(this.HasNavigationBox && !TabDrag)
			{
				m_TabSystemBox.MouseMove(e);
				if(m_TabSystemBox.Visible && m_TabSystemBox.DisplayRectangle.Contains(e.X,e.Y))
				{
					if(m_HotTab!=null)
					{
						if(m_Tabs.Contains(m_HotTab))
						{
							InvalidateHotTab();
							m_HotTab=null;
							this.Update();
						}
						else
							m_HotTab=null;
					}
					return;
				}
			}

			if(m_DragBar!=null)
			{
                if(!m_DragBar.IsDisposed)
				    m_DragBar.DragMouseMove();
				return;
			}
			if(m_IsDragging)
				return;

			if(e.Button==MouseButtons.Left)
			{
                if (m_HotTab != null && m_HotTab.CloseButtonMouseOver)
                {
                    m_HotTab.CloseButtonMouseOver = false;
                    InvalidateHotTab();
                }

				if(TabDrag)
				{
					if(this.DisplayRectangle.Contains(e.X,e.Y))
					{
						if(m_CanReorderTabs || GetDesignMode())
						{
							foreach(TabItem item in m_Tabs)
							{
								Rectangle r=item.DisplayRectangle;
								if(e.X<r.Right && (m_Alignment==eTabStripAlignment.Bottom || m_Alignment==eTabStripAlignment.Top))
									r.Width=m_SelectedTab.DisplayRectangle.Width;
								else if(e.Y<r.Bottom && (m_Alignment==eTabStripAlignment.Left || m_Alignment==eTabStripAlignment.Right))
									r.Height=m_SelectedTab.DisplayRectangle.Height;
								if(r.Contains(e.X,e.Y))
								{
									if(m_SelectedTab==item)
										break;
									int index=m_Tabs.IndexOf(item);
									if(TabMoved!=null)
									{
										TabStripTabMovedEventArgs arg=new TabStripTabMovedEventArgs(m_SelectedTab,m_Tabs.IndexOf(m_SelectedTab),index);
										TabMoved(this,arg);
										if(arg.Cancel)
											break;
									}
									TabItem tabItem=m_SelectedTab;
									m_MovingTab=true;
									m_Tabs._Remove(tabItem);
									m_Tabs._Insert(index,tabItem);
									this.SelectedTab=tabItem;
									m_MovingTab=false;
                                    this.Refresh();
									break;
								}
							}
						}
					}
					else
					{
						Rectangle r=this.DisplayRectangle;
						r.Inflate(16,16);
						if(!r.Contains(e.X,e.Y))
						{
							int visibleTabCount=this.VisibleTabCount;
							if(this.Parent is Bar && ((Bar)this.Parent).CanTearOffTabs && (visibleTabCount>1 || visibleTabCount==1 && ((Bar)this.Parent).GrabHandleStyle==eGrabHandleStyle.None))
							{
								TabDrag=false;
								m_IsDragging=true; // Very important to set this flag before calling StartTabDrag

								m_DragBar=((Bar)this.Parent).StartTabDrag();
								if(m_DragBar!=null)
								{	
									this.Capture=true;
                                    this.Refresh();
								}
								else
									m_IsDragging=false;
							}
						}
					}

				}
                else if ((Math.Abs(e.X - _MouseDownPoint.X) >= 4 || Math.Abs(e.Y - _MouseDownPoint.Y) >= 4) && m_Tabs.Count > 0 && !this.IsMultiLine && this.CanReorderTabs)
                {
                    TabDrag = true;
                }
			}
			else if(e.Button==MouseButtons.None)
			{
				Rectangle rSelected=this.GetSelectedTabRectangle();
				if(rSelected.Contains(e.X,e.Y))
				{
                    SetHotTab(this.SelectedTab, e.X, e.Y);
					return;
				}
				else
				{
					foreach(TabItem tab in m_Tabs)
					{
						if(tab.DisplayRectangle.Contains(e.X,e.Y))
						{
                            SetHotTab(tab, e.X, e.Y);
							return;
						}
					}
				}
				SetHotTab(null,e.X,e.Y);
			}
		}

        private Cursor _OldCursor = null;
        private bool TabDrag
        {
            get
            {
                return m_TabDrag;
            }
            set
            {
                if (m_TabDrag != value)
                {
                    m_TabDrag = value;
                    if (m_TabDrag)
                    {
                        if (!IsDockTab)
                        {
                            _OldCursor = this.Cursor;
                            this.Cursor = GetTabDragCursor();
                        }
                    }
                    else if(!IsDockTab)
                    {
                        this.Cursor = _OldCursor;
                    }
                }
            }
        }

        private Cursor _TabDragCursor = null;
        /// <summary>
        /// Gets or sets the mouse cursor that is displayed when tab is dragged.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Appearance"), Description("Specifies the mouse cursor that is displayed when tab is dragged.")]
        public Cursor TabDragCursor
        {
            get { return _TabDragCursor; }
            set { _TabDragCursor = value; }
        }

        private Cursor GetTabDragCursor()
        {
            if (_TabDragCursor != null) return _TabDragCursor;
            return Cursors.SizeAll;
        }
        private bool IsDockTab
        {
            get { return this.Parent is Bar; }
        }

        private bool GetDesignMode()
        {
            if (this.Parent is TabControl)
            {
                TabControl tb = this.Parent as TabControl;
                if (tb.Site != null) return tb.Site.DesignMode;
            }
            return this.DesignMode;
        }

		private void SetHotTab(TabItem tab, int x, int y)
		{
			if(m_HotTab!=tab)
			{
				if(m_Tabs.Contains(m_HotTab))
					InvalidateHotTab();
                if (m_HotTab != null)
                    m_HotTab.CloseButtonMouseOver = false;
				m_HotTab=tab;
                if (m_HotTab != null)
                {
                    if (!m_HotTab.CloseButtonBounds.IsEmpty && m_HotTab.CloseButtonBounds.Contains(x, y))
                        m_HotTab.CloseButtonMouseOver = true;
                    InvalidateHotTab();
                }
				this.Update();
				ResetHover();
			}
            else if (m_HotTab != null && !m_HotTab.CloseButtonBounds.IsEmpty)
            {
                if (m_HotTab.CloseButtonBounds.Contains(x, y))
                {
                    if (!m_HotTab.CloseButtonMouseOver)
                    {
                        m_HotTab.CloseButtonMouseOver = true;
                        InvalidateHotTab();
                        this.Update();
                    }
                }
                else if (m_HotTab.CloseButtonMouseOver)
                {
                    m_HotTab.CloseButtonMouseOver = false;
                    InvalidateHotTab();
                    this.Update();
                }
            }
		}

		/// <summary>
		/// Returns tab mouse is over or null if mouse is not over the tab.
		/// </summary>
		internal TabItem MouseOverTab
		{
			get {return m_HotTab;}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			m_TabSystemBox.MouseWheel(e);
		}

		internal int VisibleTabCount
		{
			get
			{
				int count=0;
				foreach(TabItem tab in m_Tabs)
				{
					if(tab.Visible)
						count++;
				}
				return count;
			}
		}

		internal bool IsDraggingBar
		{
			get
			{
				return m_IsDragging;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			InternalOnMouseUp(e);
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void InternalOnMouseUp(MouseEventArgs e)
		{
			if(this.HasNavigationBox)
			{
				m_TabSystemBox.MouseUp(e);
				if(m_TabSystemBox.Visible && m_TabSystemBox.DisplayRectangle.Contains(e.X,e.Y))
					return;
			}

            Point mouseDownPoint = _MouseDownPoint;
            _MouseDownPoint = Point.Empty;

			if(m_IsDragging)
				this.Capture=false;
			if(m_DragBar!=null)
			{
                if(!m_DragBar.IsDisposed)
				    m_DragBar.DragMouseUp();
				m_DragBar=null;
			}
			m_IsDragging=false;
			TabDrag=false;

            // Invoke TabItem events.
            TabItem tabAt = HitTest(e.X, e.Y);
            if (tabAt != null)
            {
                tabAt.InvokeMouseUp(e);
            }

            if (this.SelectedTab != null && this.SelectedTab == tabAt && !this.SelectedTab.CloseButtonBounds.IsEmpty && e.Button == MouseButtons.Left)
            {
                Rectangle closeButtonBounds = this.SelectedTab.CloseButtonBounds;
                // Increase the width for 2 pixels since when tab is selected it moves 2 pixels to the left
                closeButtonBounds.Width += 2;
                if (closeButtonBounds.Contains(e.X, e.Y) && closeButtonBounds.Contains(mouseDownPoint))
                    this.CloseSelectedTab();
            }
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			InternalOnMouseLeave(e);
			base.OnMouseLeave(e);
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void InternalOnMouseLeave(EventArgs e)
		{
			HideToolTip();
			if(this.HasNavigationBox)
				m_TabSystemBox.MouseLeave();
			if(m_HotTab!=null)
			{
                m_HotTab.InvokeMouseLeave(e);
				if(m_Tabs.Contains(m_HotTab))
				{
                    m_HotTab.CloseButtonMouseOver = false;
					InvalidateHotTab();
					m_HotTab=null;
					this.Update();
				}
				else
					m_HotTab=null;
			}
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			if(this.HasNavigationBox)
				m_TabSystemBox.MouseHover();
			this.InternalMouseHover();
		}

		internal void InternalTabsCleared()
		{
			HideToolTip();
            OnTabsCleared(new EventArgs());
			if(m_SelectedTab!=null)
			{
				if(SelectedTabChanged!=null)
				{
					TabStripTabChangedEventArgs eventData=new TabStripTabChangedEventArgs(m_SelectedTab,null,eEventSource.Code);
					SelectedTabChanged(this,eventData);
				}
				m_SelectedTab=null;
			}
		}

        /// <summary>
        /// Invokes the TabCleared event.
        /// </summary>
        /// <param name="e">Provides events arguments</param>
        protected virtual void OnTabsCleared(EventArgs e)
        {
            if (TabsCleared != null)
                TabsCleared(this, e);
        }

		/// <summary>
		/// Ensures that the tab is visible, scrolling the tab-strip view as necessary.
		/// </summary>
		/// <param name="tab">Tab to make visible.</param>
		public void EnsureVisible(TabItem tab)
		{
			if(tab==null || !m_Tabs.Contains(tab) || m_TabLayoutType==eTabLayoutType.FitContainer || this.IsMultiLine || !BarFunctions.IsHandleValid(this))
				return;

			Rectangle r=tab.DisplayRectangle;

            if (r.IsEmpty)
            {
                this.Invalidate();
                this.RecalcSize();
            }

			r=tab.DisplayRectangle;

            if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document)
			{
                if (m_Alignment == eTabStripAlignment.Top || m_Alignment == eTabStripAlignment.Bottom)
				{
                    if(m_Style != eTabStripStyle.Office2007Document)
					    r.X-=(this.TabHeight-6);
					r.Width+=(this.TabHeight-6);
				}
				else if(m_Alignment==eTabStripAlignment.Right)
				{
                    if (m_Style != eTabStripStyle.Office2007Document)
					    r.Y-=(this.TabHeight-6);
					r.Height+=(this.TabHeight-6);
				}
				else if(m_Alignment==eTabStripAlignment.Left)
					r.Height+=(this.TabHeight-6);

			}

			int offset=this.ScrollOffset;

			if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
			{
                if (r.X < 0 || this.IsRightToLeft && r.X < m_TabSystemBox.DisplayRectangle.Right && m_TabSystemBox.Visible)
                {
                    offset = m_TabItemsBounds.Width - this.ClientRectangle.Width;
                    if (this.IsRightToLeft && (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document))
                        offset -= r.Height;
                    if (offset < 0 || this.IsRightToLeft && offset < m_TabSystemBox.DisplayRectangle.Right)
                    {
                        offset = 0;
                    }
                    this.ScrollOffset = offset;
                }
                else if (!this.IsRightToLeft && r.Right > m_TabSystemBox.DisplayRectangle.Left && m_TabSystemBox.Visible && !m_TabSystemBox.DisplayRectangle.IsEmpty)
                {
                    offset = offset + r.Right - m_TabSystemBox.DisplayRectangle.Left + 2/*+m_TabSystemBox.DefaultWidth*/;
                    this.ScrollOffset = offset;
                }
                else if (this.IsRightToLeft && r.Right > this.DisplayRectangle.Right)
                {
                    offset = offset + r.Right - this.DisplayRectangle.Right + 2;
                    this.ScrollOffset = offset;
                }
                else if (tab.IsSelected && IsLastVisibleTab(tab) && offset > 0) // Ensures while tab is scrolled that there is little space to the right and most content visible
                {
                    offset = m_TabItemsBounds.Width - (this.ClientRectangle.Width - m_TabSystemBox.DisplayRectangle.Width - TabHeight);
                    if (offset < 0 || this.IsRightToLeft && offset < m_TabSystemBox.DisplayRectangle.Right)
                    {
                        offset = 0;
                    }
                    this.ScrollOffset = offset;
                }
			}
			else
			{
				if(r.Y<0)
				{
					offset=offset+(r.Y-4);
					if(offset<0)
						offset=0;
					this.ScrollOffset=offset;
				}
				else if(r.Bottom>m_TabSystemBox.DisplayRectangle.Top && m_TabSystemBox.Visible && !m_TabSystemBox.DisplayRectangle.IsEmpty)
				{
					offset=offset+r.Bottom-m_TabSystemBox.DisplayRectangle.Top+4;
					this.ScrollOffset=offset;
				}
			}
		}

        private bool IsLastVisibleTab(TabItem tab)
        {
            int start = this.Tabs.Count - 1;
            for (int i = start; i >= 0; i--)
            {
                if (!this.Tabs[i].Visible) continue;
                if (this.Tabs[i] == tab) return true;
                break;
            }
            return false;
        }

		internal bool _IgnoreBeforeTabDisplayEvent=false;
		internal void InvokeBeforeTabDisplay(TabItem item)
		{
			if(!_IgnoreBeforeTabDisplayEvent && BeforeTabDisplay!=null)
				BeforeTabDisplay(item,new EventArgs());
		}

		
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                StyleManager.Unregister(this);
			}

            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_TabCloseButtonNormal);
                BarUtilities.DisposeImage(ref m_TabCloseButtonHot);
            }

			base.Dispose( disposing );
		}

        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            OnTabStyleChanged();
        }

		/// <summary>
		/// Gets or sets the selected tab Font
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),DefaultValue(null),Description("Gets or sets the selected tab Font")]
		public Font SelectedTabFont
		{
			get
			{
				return m_SelectedTabFont;
			}
			set
			{
				m_SelectedTabFont=value;
				if(m_SelectedTabFont==null)
					m_SelectedTabFontCustom=false;
				else
					m_SelectedTabFontCustom=true;
				m_NeedRecalcSize=true;
				this.Invalidate();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetSelectedTabFont()
		{
			m_SelectedTabFont=null;
		}

		/// <summary>
		/// Gets or sets the tab alignment within the Tab-Strip control.
		/// </summary>
		[DefaultValue(eTabStripAlignment.Bottom),Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates the tab alignment within the Tab-Strip control.")]
		public eTabStripAlignment TabAlignment
		{
			get {return m_Alignment;}
			set
			{
				if(m_Alignment==value)
					return;
				m_Alignment=value;
				m_NeedRecalcSize=true;
				this.Invalidate();
			}
		}

		internal bool NeedRecalcSize
		{
			get {return m_NeedRecalcSize;}
			set {m_NeedRecalcSize=value;}
		}

		/// <summary>
		/// Specifes whether end-user can reorder the tabs.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Specifes whether end-user can reorder the tabs.")]
		public bool CanReorderTabs
		{
			get
			{
				return m_CanReorderTabs;
			}
			set
			{
				if(m_CanReorderTabs!=value)
					m_CanReorderTabs=value;
			}
		}

		/// <summary>
		/// Gets or sets whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Behavior"),Description("Indicates whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.")]
		public bool AutoHideSystemBox
		{
			get {return m_AutoHideSystemBox;}
			set
			{
				if(m_AutoHideSystemBox==value)
					return;
				m_AutoHideSystemBox=value;
				m_NeedRecalcSize=true;
				if(this.IsDesignMode)
					this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[Browsable(false),Description("Indicates the background color."),Category("Style"),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Obsolete("This property is obsolete. Please use ColorScheme property to change all tab colors."),EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set{base.BackColor=value;}
		}

		/// <summary>
		/// Gets or sets whether only selected tab is displaying it's text.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Specifes whether only selected tab is displaying it's text."),DefaultValue(false)]
		public bool DisplaySelectedTextOnly
		{
			get {return m_DisplaySelectedTextOnly;}
			set
			{
				m_DisplaySelectedTextOnly=value;
				m_NeedRecalcSize=true;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets TabStrip style. Theme style is supported only on themed OS and only for bottom or top aligned tabs.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Specifes the TabStrip style."),DefaultValue(eTabStripStyle.Flat)]
		public eTabStripStyle Style
		{
			get
			{
				return m_Style;
			}
			set
			{
				m_Style=value;
				OnTabStyleChanged();
			}
		}

        private void RefreshColorScheme()
        {
            m_ColorScheme.Style = m_Style;
            m_ColorScheme.Themed = this.IsThemed;
            m_ColorScheme.Refresh();
        }

        private void OnTabStyleChanged()
		{
			m_NeedRecalcSize=true;
            RefreshColorScheme();
			m_ScrollOffset=0;
            if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document) && !this.IsThemed)
			{
				if(m_TabLayoutType==eTabLayoutType.FitContainer)
					m_TabLayoutType=eTabLayoutType.FixedWithNavigationBox;
			}
			this.Invalidate();
			this.EnsureVisible(this.SelectedTab);
		}

		/// <summary>
		/// Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Appearance"),Description("Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public virtual bool ThemeAware
		{
			get
			{
				return m_ThemeAware;
			}
			set
			{
				m_ThemeAware=value;
				m_NeedRecalcSize=true;
				m_ColorScheme.Themed=this.IsThemed;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets whether control should be represented in themed style.
		/// </summary>
		internal bool IsThemed
		{
			get
			{
				if(m_ThemeAware && BarFunctions.ThemedOS && Themes.ThemesActive && m_Style!=eTabStripStyle.SimulatedTheme &&
                    m_Style != eTabStripStyle.Office2007Dock && m_Style != eTabStripStyle.Office2007Document)
					return true;
				return false;
			}
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			Application.DoEvents();
			OnTabStyleChanged();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			m_TabHeight=0;
			m_NeedRecalcSize=true;
			if(this.IsDesignMode)
			{
                if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document) && !m_SelectedTabFontCustom)
				{
					m_SelectedTabFont=new Font(this.Font,FontStyle.Bold);
				}
				this.Invalidate();
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if(this.ShowFocusRectangle)
				this.Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if(this.ShowFocusRectangle)
				this.Invalidate();
		}

		internal void OnTabVisibleChanged(TabItem tab)
		{
			m_NeedRecalcSize=true;
			if(tab==m_TooltipTab)
				HideToolTip();
			if(!tab.Visible && m_SelectedTab==tab)
			{
				int i=this.Tabs.IndexOf(tab)-1;
				TabItem newSelected=null;
				while(i>=0)
				{
					if(this.Tabs[i].Visible)
					{
						newSelected=this.Tabs[i];
						break;
					}
					i--;
				}
				if(newSelected==null)
				{
					if(i<0) i=0;
					for(int it=i;it<this.Tabs.Count;it++)
					{
						if(this.Tabs[it].Visible)
						{
							newSelected=this.Tabs[it];
							break;
						}
					}
				}
				this.SelectedTab=newSelected;
			}
			else if(this.VisibleTabCount==1 && tab.Visible)
				this.SelectedTab=tab;
		}

		/// <summary>
		/// Gets or sets Tab Color Scheme.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Gets or sets Tab Color Scheme."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TabColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				if(value==null)
					throw new ArgumentException("NULL is not a valid value for this property.");
				if(m_ColorScheme!=null)
					m_ColorScheme.ColorChanged-=new EventHandler(this.ColorSchemeChanged);
				m_ColorScheme=value;
				m_ColorScheme.ColorChanged+=new EventHandler(this.ColorSchemeChanged);
				if(this.Visible)
					this.Invalidate();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColorScheme()
		{
			return m_ColorScheme.SchemeChanged;
		}
        /// <summary>
		/// Resets color scheme to default value.
		/// </summary>
		public void ResetColorScheme()
		{
			m_ColorScheme.ResetChangedFlag();
			m_ColorScheme.Refresh();
			if(this.IsDesignMode)
				this.Invalidate();
		}

		private void ColorSchemeChanged(object sender, EventArgs e)
		{
			if(this.IsDesignMode || this.Parent is TabControl)
				this.Invalidate();
		}

		protected override bool ProcessMnemonic(char charCode)
		{
			string s="&"+charCode.ToString();
			s=s.ToLower();
			foreach(TabItem tab in this.Tabs)
			{
				string text=tab.Text.ToLower();
				if(text.IndexOf(s)>=0)
				{
					this.SelectTab(tab,eEventSource.Keyboard);
					return true;
				}
			}
			return base.ProcessMnemonic(charCode);
		}

		internal TabColors GetTabColors(TabItem tab)
		{
			TabColors c=new TabColors();
			if(tab==m_HotTab && !tab.IsSelected)
			{
				c.BackColor=m_ColorScheme.TabItemHotBackground;
				c.BackColor2=m_ColorScheme.TabItemHotBackground2;
				c.BackColorGradientAngle=m_ColorScheme.TabItemHotBackgroundGradientAngle;
				c.TextColor=m_ColorScheme.TabItemHotText;
				c.LightBorderColor=m_ColorScheme.TabItemHotBorderLight;
				c.DarkBorderColor=m_ColorScheme.TabItemHotBorderDark;
				c.BorderColor=m_ColorScheme.TabItemHotBorder;
                c.BackgroundColorBlend.CopyFrom(m_ColorScheme.TabItemHotBackgroundColorBlend);
			}
			
			if(this.SelectedTab==tab)
			{
                if (tab.PredefinedColor != eTabItemColor.Default)
				{
					c.BackColor=tab.BackColor;
					c.BackColor2=tab.BackColor2;
					c.BackColorGradientAngle=tab.BackColorGradientAngle;
                    c.TextColor = tab.TextColor;
				}
				if(c.BackColor.IsEmpty)
				{
					c.BackColor=m_ColorScheme.TabItemSelectedBackground;
					c.BackColor2=m_ColorScheme.TabItemSelectedBackground2;
					c.BackColorGradientAngle=m_ColorScheme.TabItemSelectedBackgroundGradientAngle;
                    c.BackgroundColorBlend.CopyFrom(m_ColorScheme.TabItemSelectedBackgroundColorBlend);
				}
				if(c.TextColor.IsEmpty)
				{
					if(m_ColorScheme.TabItemSelectedText.IsEmpty)
						c.TextColor=m_ColorScheme.TabItemText;
					else
						c.TextColor=m_ColorScheme.TabItemSelectedText;
				}
				if(c.BorderColor.IsEmpty)
				{
					c.LightBorderColor=m_ColorScheme.TabItemSelectedBorderLight;
					c.DarkBorderColor=m_ColorScheme.TabItemSelectedBorderDark;
					c.BorderColor=m_ColorScheme.TabItemSelectedBorder;
				}
			}

			if(c.BackColor.IsEmpty)
			{
				if(tab.BackColor.IsEmpty && tab.BackColor2.IsEmpty)
				{
					c.BackColor=m_ColorScheme.TabItemBackground;
					c.BackColor2=m_ColorScheme.TabItemBackground2;
					c.BackColorGradientAngle=m_ColorScheme.TabItemBackgroundGradientAngle;
                    c.BackgroundColorBlend.CopyFrom(m_ColorScheme.TabItemBackgroundColorBlend);
				}
				else
				{
					c.BackColor=tab.BackColor;
					c.BackColor2=tab.BackColor2;
					c.BackColorGradientAngle=tab.BackColorGradientAngle;
                    c.TextColor = tab.TextColor;
				}
			}

			if(c.TextColor.IsEmpty)
			{
				if(tab.TextColor.IsEmpty)
					c.TextColor=m_ColorScheme.TabItemText;
				else
					c.TextColor=tab.TextColor;
			}

			if(c.BorderColor.IsEmpty)
			{
				if(tab.BorderColor.IsEmpty && tab.LightBorderColor.IsEmpty && tab.DarkBorderColor.IsEmpty)
				{
					c.DarkBorderColor=m_ColorScheme.TabItemBorderDark;
					c.LightBorderColor=m_ColorScheme.TabItemBorderLight;
					c.BorderColor=m_ColorScheme.TabItemBorder;
				}
				else
				{
					c.LightBorderColor=tab.LightBorderColor;
					c.DarkBorderColor=tab.DarkBorderColor;
					c.BorderColor=tab.BorderColor;
				}
			}
			
			return c;
		}

		private void OnTabBack(object sender, EventArgs e)
		{
			ScrollBackwards();
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void ScrollBackwards()
		{
			if(NavigateBack!=null)
			{
				TabStripActionEventArgs action=new TabStripActionEventArgs();
				NavigateBack(this,action);
				if(action.Cancel)
					return;
			}

			int iOffset=0;
			for(int i=m_Tabs.Count-1;i>=0;i--)
			{
				TabItem item=m_Tabs[i];
				if(!item.Visible)
					continue;
				if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
				{
					if(item.DisplayRectangle.X<4)
					{
						iOffset=-item.DisplayRectangle.Width;
                        if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
                            iOffset -= item.DisplayRectangle.Height;
						break;
					}
				}
				else
				{
					if(item.DisplayRectangle.Y<4)
					{
						iOffset=-item.DisplayRectangle.Height;
                        if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document)
                            iOffset -= item.DisplayRectangle.Height;
						break;
					}
				}
			}
            if (iOffset == 0 && m_ScrollOffset > 0)
                iOffset = -m_ScrollOffset;

			int iStart=m_ScrollOffset;
			int iEnd=m_ScrollOffset+iOffset;
			if(iEnd<0)
				iEnd=0;
			if(m_Animate)
			{
				DateTime start=DateTime.Now;
				for(int i=iStart;i>iEnd;i--)
				{
					this.ScrollOffset=i;
					if(((TimeSpan)DateTime.Now.Subtract(start)).TotalMilliseconds >200)
						break;
				}
			}
			this.ScrollOffset=iEnd;
		}
		private void OnTabForward(object sender, EventArgs e)
		{
			ScrollForward();
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void ScrollForward()
		{
			if(NavigateForward!=null)
			{
				TabStripActionEventArgs action=new TabStripActionEventArgs();
				NavigateForward(this,action);
				if(action.Cancel)
					return;
			}

			int iOffset=0;
			TabItem lastItem=null;
            int start = 0, end = m_Tabs.Count, direction = 1;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                start = m_Tabs.Count - 1;
                end = -1;
                direction = -1;
            }
            for (int i = start; i != end; i += direction)
            {
                TabItem item = m_Tabs[i];
				if(!item.Visible)
					continue;
				if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
				{
                    if (item.DisplayRectangle.X < 4 && this.RightToLeft == RightToLeft.No ||
                        item.DisplayRectangle.Right <= this.Width && this.RightToLeft == RightToLeft.Yes)
                        continue;
					lastItem=item;
					iOffset=item.DisplayRectangle.Width;
					if(item.DisplayRectangle.Right>this.Width-m_TabSystemBox.DefaultWidth)
						break;
				}
				else
				{
					if(item.DisplayRectangle.Y<4)
						continue;
					lastItem=item;
					iOffset=item.DisplayRectangle.Height;
					if(item.DisplayRectangle.Bottom>this.Height-m_TabSystemBox.DefaultWidth)
						break;
				}
			}
			
			if(lastItem!=null)
			{
				if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
				{
					if(lastItem.DisplayRectangle.Right+2<this.Width-m_TabSystemBox.DefaultWidth)
						return;
				}
				else
				{
					if(lastItem.DisplayRectangle.Bottom+2<this.Height-m_TabSystemBox.DefaultWidth)
						return;
				}
			}

			int iStart=m_ScrollOffset;
			int iEnd=m_ScrollOffset+iOffset;
			if(m_Alignment==eTabStripAlignment.Top || m_Alignment==eTabStripAlignment.Bottom)
			{
                if (iEnd > m_Tabs[m_Tabs.Count - 1].DisplayRectangle.Right + m_ScrollOffset && this.RightToLeft == RightToLeft.No)
                    iEnd = m_Tabs[m_Tabs.Count - 1].DisplayRectangle.Right + m_ScrollOffset + m_TabSystemBox.DefaultWidth;
                else if (iEnd < 0 && this.RightToLeft == RightToLeft.Yes)
                    iEnd = 0;
			}
			else
			{
				if(iEnd>m_Tabs[m_Tabs.Count-1].DisplayRectangle.Bottom+m_ScrollOffset)
					iEnd=m_Tabs[m_Tabs.Count-1].DisplayRectangle.Bottom+m_ScrollOffset+m_TabSystemBox.DefaultWidth;
			}

			if(m_Animate)
			{
				DateTime animationStart=DateTime.Now;
				for(int i=iStart;i<iEnd;i++)
				{
					this.ScrollOffset=i;
                    if (((TimeSpan)DateTime.Now.Subtract(animationStart)).TotalMilliseconds > 200)
						break;
				}
			}
			this.ScrollOffset=iEnd;
		}
		private void OnTabClose(object sender, EventArgs e)
		{
            CloseSelectedTab();
		}

        private void CloseSelectedTab()
        {
            if (TabItemClose != null)
            {
                TabStripActionEventArgs action = new TabStripActionEventArgs();
                TabItemClose(this, action);
                if (action.Cancel)
                    return;
            }

            if (this.SelectedTab == null)
                return;

            if (m_MdiTabbedDocuments && this.SelectedTab != null && this.SelectedTab.AttachedControl is Form)
                ((Form)this.SelectedTab.AttachedControl).Close();
            else if (this.SelectedTab != null && this.SelectedTab.AttachedItem != null)
            {
                if (this.SelectedTab.AttachedItem is DockContainerItem)
                {
                    Bar bar = this.SelectedTab.AttachedItem.ContainerControl as Bar;
                    if (bar != null)
                        bar.CloseDockTab((DockContainerItem)this.SelectedTab.AttachedItem, eEventSource.Mouse);
                    else
                        this.SelectedTab.AttachedItem.Visible = false;
                }
            }
        }

		/// <summary>
		/// Gets or sets whether the tab scrolling is animanted.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Description("Indicates whether the tab scrolling is animanted."),Category("Behavior"),DefaultValue(true)]
		public bool Animate
		{
			get{return m_Animate;}
			set {m_Animate=value;}
		}

        /// <summary>
        /// Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.
        /// Value of 0 indicates that size is automatically calculated which is
        /// default behavior.
        /// </summary>
        [Browsable(true),Category("Appearance"),Description("Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.")]
        public Size FixedTabSize
        {
            get { return m_FixedTabSize; }
            set 
            {
                m_FixedTabSize = value;
                m_NeedRecalcSize=true;
                if(this.DesignMode)
                    this.Invalidate();
            }
        }
        /// <summary>
        /// Memeber used by Windows Forms designer.
        /// </summary>
        /// <returns>true if property should be serialized.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFixedTabSize()
        {
            return !m_FixedTabSize.IsEmpty;
        }
        /// <summary>
        /// Memeber used by Windows Forms designer to reset property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFixedTabSize()
        {
            TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
        }

		/// <summary>
		/// Returns tab item that contains specified coordinates.
		/// </summary>
		/// <param name="x">X - coordinate to hit test</param>
		/// <param name="y">Y - coordinate to hit test</param>
		/// <returns></returns>
		public TabItem HitTest(int x, int y)
		{
            if (m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document)
			{
				if(this.GetSelectedTabRectangle().Contains(x,y))
					return this.SelectedTab;
			}
            Rectangle systemBox = GetSystemBoxRectangle();
			foreach(TabItem tab in this.Tabs)
			{
				if(tab.Visible && tab.DisplayRectangle.Contains(x,y))
					return tab;
			}

			return null;
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
            RefreshColorScheme();
			if(m_MdiTabbedDocuments && m_MdiForm==null)
			{
				Form form=this.FindForm();
				if(form!=null && form.IsMdiContainer)
					this.MdiForm=form;
			}
            if ((m_Style == eTabStripStyle.OneNote || m_Style == eTabStripStyle.VS2005Document || m_Style == eTabStripStyle.Office2007Document) && m_SelectedTabFont == null)
			{
				m_SelectedTabFont=new Font(this.Font,FontStyle.Bold);
			}
		}

		/// <summary>
		/// Gets or sets whether focus rectangle is displayed when tab has input focus.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether focus rectangle is displayed when tab has input focus.")]
		public bool ShowFocusRectangle
		{
			get {return m_ShowFocusRectangle;}
			set {m_ShowFocusRectangle=value;}
		}

		/// <summary>
		/// Gets or sets whether Tab-Strip control provides Tabbed MDI Child form support. Default value is false.
		/// </summary>
		[Browsable(true),Description("Indicates whether Tab-Strip control provides Tabbed MDI Child form support."),Category("Mdi Support"),DefaultValue(false)]
		public bool MdiTabbedDocuments
		{
			get{return m_MdiTabbedDocuments;}
			set
			{
				if(m_MdiTabbedDocuments!=value)
				{
					m_MdiTabbedDocuments=value;
					if(m_MdiForm==null && m_MdiTabbedDocuments)
					{
						// Try to get to the MDI form
						Form form=this.FindForm();
						if(form!=null && form.IsMdiContainer)
							this.MdiForm=form;
					}
					else if(!m_MdiTabbedDocuments)
					{
						this.MdiForm=null;
						m_Tabs.Clear();
						this.Invalidate();
					}
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the maximum number of characters that will be used as Tab text from Mdi Child caption.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Description("Indicates the maximum number of characters that will be used as Tab text from Mdi Child caption."),Category("Mdi Support"),DefaultValue(32)]
		public int MaxMdiCaptionLength
		{
			get{return m_MaxMdiCaptionLength;}
			set
			{
				if(m_MaxMdiCaptionLength!=value)
				{
					m_MaxMdiCaptionLength=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the Mdi Child Icon is displayed on Tab.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Description("Indicates whether the Mdi Child Icon is displayed on Tab."),Category("Mdi Support"),DefaultValue(true)]
		public bool ShowMdiChildIcon
		{
			get{return m_ShowMdiChildIcon;}
			set
			{
				if(m_ShowMdiChildIcon!=value)
				{
					m_ShowMdiChildIcon=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the Tab-strip is automatically hidden when there are not Mdi Child forms open.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Description("Indicates whether the Tab-strip is automatically hidden when there are not Mdi Child forms open."),Category("Mdi Support"),DefaultValue(true)]
		public bool MdiAutoHide
		{
			get{return m_MdiAutoHide;}
			set
			{
				if(m_MdiAutoHide!=value)
				{
					m_MdiAutoHide=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether flicker associated with switching maximized Mdi child forms is attempted to eliminate. You should set this property to false if you encounter any painting problems with your Mdi child forms.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Description("Indicates whether flicker associated with switching maximized Mdi child forms is attempted to eliminate."),Category("Mdi Support"),DefaultValue(true)]
		public bool MdiNoFormActivateFlicker
		{
			get{return m_MdiNoFormActivateFlicker;}
			set
			{
				if(m_MdiNoFormActivateFlicker!=value)
				{
					m_MdiNoFormActivateFlicker=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets Mdi Container form for which Tab-Strip is providing Tabbed MDI Child support.
		/// </summary>
        [Browsable(true), DefaultValue(null), Description("Indicates Mdi Container form for which Tab-Strip is providing Tabbed MDI Child support.")]
		public Form MdiForm
		{
			get {return m_MdiForm;}
			set
			{
                //if(value!=null && !value.IsMdiContainer)
                //    throw new ArgumentException("Form is not a Mdi Container.");

				if(m_MdiForm!=null)
					ReleaseMdiForm();

				m_MdiForm=value;

				if(m_MdiForm!=null && !this.DesignMode)
				{
					InitializeMdiForm();
					RefreshMdiTabItems();
				}
			}
		}

		private void InitializeMdiForm()
		{
			MdiClient client=GetMdiClient(m_MdiForm);
			if(client==null)
			{
				m_MdiForm.Load+=new EventHandler(this.ParentFormLoaded);
				return;
			}

			client.ControlAdded+=new ControlEventHandler(this.MdiFormAdded);
			client.ControlRemoved+=new ControlEventHandler(this.MdiFormRemoved);
			m_MdiForm.MdiChildActivate+=new EventHandler(this.MdiFormActivated);
			m_MdiInitialized=true;
		}

		private void ReleaseMdiForm()
		{
			if(!m_MdiInitialized)
				return;

			MdiClient client=GetMdiClient(m_MdiForm);
			if(client==null)
				return;

			client.ControlAdded-=new ControlEventHandler(this.MdiFormAdded);
			client.ControlRemoved-=new ControlEventHandler(this.MdiFormRemoved);
			m_MdiForm.MdiChildActivate-=new EventHandler(this.MdiFormActivated);
			m_MdiInitialized=false;
		}

		private void ParentFormLoaded(object sender, EventArgs e)
		{
            if (m_MdiForm != null)
            {
                m_MdiForm.Load -= new EventHandler(this.ParentFormLoaded);
                if (!m_MdiInitialized)
                    InitializeMdiForm();
            }
            else if (sender is Form)
            {
                ((Form)sender).Load -= new EventHandler(this.ParentFormLoaded);
            }
		}

		internal void OnTabAdded(TabItem item)
		{
			HideToolTip();

			if(m_MovingTab)
				return;
			if(TabItemOpen!=null)
				TabItemOpen(item,new EventArgs());
			if(this.VisibleTabCount==1 && item.Visible && m_SelectedTab==null)
				this.SelectedTab=item;
		}

		private string GetFormText(Form form)
		{
			if(form==null)
				return "";
            string text = form.Text.Replace("&", "&&");
			if(text.Length>m_MaxMdiCaptionLength)
			{
				int i=m_MaxMdiCaptionLength/2;
				return text.Substring(0,i)+"..."+text.Substring(text.Length-(m_MaxMdiCaptionLength-i));
			}
			return text;
		}

		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void MdiFormAdded(object sender, ControlEventArgs e)
		{
			if(m_MdiForm==null || !m_MdiInitialized)
				return;

			Form form=e.Control as Form;
			if(form==null)
			{
				RefreshMdiTabItems();
				return;
			}
            
			string text=GetFormText(form);

			TabItem item=new TabItem();
			item.Text=text;
			item.AttachedControl=form;
			
			//Image img=null;
			if(m_ShowMdiChildIcon && form.Icon!=null)
			{
				item.Icon=new Icon(form.Icon,item.IconSize);
//				img=new Bitmap(16,16,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
//				Graphics g=Graphics.FromImage(img);
//				g.DrawIcon(form.Icon,new Rectangle(0,0,16,16));
//				g.Dispose();
//				item.Image=img;
			}

			m_Tabs.Add(item);

			form.TextChanged+=new EventHandler(this.FormTextChanged);
			form.VisibleChanged+=new EventHandler(this.FormVisibleChanged);

			if(m_MdiForm.ActiveMdiChild==form)
				this.SelectedTab=item;

			if(m_MdiAutoHide && m_MdiForm!=null && !this.Visible && !this.IsDesignMode)
			{
				BarFunctions.SetControlVisible(this,true);
			}
			this.Invalidate();
		}

		private void RefreshMdiTabItems()
		{
			if(m_MdiForm==null || !m_MdiTabbedDocuments || !BarFunctions.IsHandleValid(this))
				return;

            if (m_Tabs.Count > 0)
            {
                foreach (TabItem tab in m_Tabs)
                {
                    if (tab.AttachedControl is Form)
                    {
                        Form form = (Form)tab.AttachedControl;
                        form.TextChanged -= new EventHandler(this.FormTextChanged);
                        form.VisibleChanged -= new EventHandler(this.FormVisibleChanged);
                    }
                    tab.Dispose();
                }
            }

			m_Tabs.Clear();

			foreach(Form form in m_MdiForm.MdiChildren)
			{
				string text=GetFormText(form);

				TabItem item=new TabItem();
				item.Text=text;
				item.AttachedControl=form;
			
				if(m_ShowMdiChildIcon && form.Icon!=null)
				{
					item.Icon=new Icon(form.Icon,item.IconSize);
				}

				m_Tabs.Add(item);
				item.Visible=form.Visible;

				form.TextChanged+=new EventHandler(this.FormTextChanged);
				form.VisibleChanged+=new EventHandler(this.FormVisibleChanged);

				if(m_MdiForm.ActiveMdiChild==form)
					this.SelectedTab=item;
			}

			if(m_MdiAutoHide && m_MdiForm!=null)
			{
				if(m_MdiForm.MdiChildren.Length==0 && this.Visible && !this.IsDesignMode)
					this.Visible=false;
				else if(m_MdiForm.MdiChildren.Length>0 && !this.Visible && !this.IsDesignMode)
				{
					BarFunctions.SetControlVisible(this,true);
				}
			}
		}

		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void MdiFormRemoved(object sender, ControlEventArgs e)
		{
			Form form=e.Control as Form;
			if(form==null)
			{
				if(TabItemClose!=null)
				{
					TabStripActionEventArgs action=new TabStripActionEventArgs();
					TabItemClose(this,action);
				}
				RefreshMdiTabItems();
				return;
			}
			foreach(TabItem item in m_Tabs)
			{
				if(item.AttachedControl==form)
				{
					if(TabItemClose!=null)
					{
						TabStripActionEventArgs action=new TabStripActionEventArgs();
						TabItemClose(item,action);
					}
					m_Tabs.Remove(item);
                    item.AttachedControl = null;
                    item.Dispose();
					form.TextChanged-=new EventHandler(this.FormTextChanged);
					form.VisibleChanged-=new EventHandler(this.FormVisibleChanged);
					this.Invalidate();
					break;
				}
			}
			if(m_MdiAutoHide && m_MdiForm!=null)
			{
				if(m_MdiForm.MdiChildren.Length==0 && !this.IsDesignMode)
					this.Visible=false;
			}
		}

		private void MdiFormActivated(object sender, EventArgs e)
		{
			Form form=m_MdiForm.ActiveMdiChild;
			foreach(TabItem item in m_Tabs)
			{
				if(item.AttachedControl==form)
				{
					if(this.SelectedTab!=item)
						this.SelectedTab=item;
					break;
				}
			}
		}

		private void FormTextChanged(object sender, EventArgs e)
		{
			Form form=sender as Form;
			if(form==null)
				return;
			foreach(TabItem item in m_Tabs)
			{
				if(item.AttachedControl==form)
				{
					item.Text=GetFormText(form);
					break;
				}
			}
		}

		private void FormVisibleChanged(object sender, EventArgs e)
		{
			Form form=sender as Form;
			if(form==null)
				return;
			foreach(TabItem item in m_Tabs)
			{
				if(item.AttachedControl==form)
				{
					item.Visible=form.Visible;
					break;
				}
			}
		}

		private MdiClient GetMdiClient(Form MdiForm)
		{
			if(!MdiForm.IsMdiContainer)
				return null;
			foreach(Control ctrl in MdiForm.Controls)
			{
				if(ctrl is MdiClient)
					return (ctrl as MdiClient);
			}
			return null;
		}

		internal Color GetBackColorFlat()
		{
			Color color=ControlPaint.Light(SystemColors.Control);
			if(BarFunctions.ThemedOS && NativeFunctions.ColorDepth>=16)
			{
				if(SystemColors.Control.ToArgb()==Color.FromArgb(236,233,216).ToArgb() && SystemColors.Highlight.ToArgb()==Color.FromArgb(49,106,197).ToArgb())
					color=Color.FromArgb(255,251,233);
				else if(SystemColors.Control.ToArgb()==Color.FromArgb(224,223,227).ToArgb() && SystemColors.Highlight.ToArgb()==Color.FromArgb(178,180,191).ToArgb())
					color=Color.FromArgb(251,250,255);
				else if(SystemColors.Control.ToArgb()==Color.FromArgb(236,233,216).ToArgb() && SystemColors.Highlight.ToArgb()==Color.FromArgb(147,160,112).ToArgb())
					color=Color.FromArgb(255,251,233);
			}
			return color;
		}

		#region Tooltip Support
		/// <summary>
		/// Hides tooltip for a tab is one is displayed.
		/// </summary>
		public void HideToolTip()
		{
			if(m_ToolTip!=null)
			{
				m_ToolTip.Hide();
				m_ToolTip.Dispose();
				m_ToolTip=null;
			}
			m_TooltipTab=null;
		}

		/// <summary>
		/// Shows tooltip for given tab.
		/// </summary>
		private void ShowToolTip(TabItem tab)
		{
			if(m_ToolTip!=null)
				HideToolTip();

			if(tab.Tooltip==null || tab.Tooltip=="")
				return;

			if(this.Visible)
			{
				if(m_ToolTip==null)
					m_ToolTip=new ToolTip();
				m_ToolTip.Text=tab.Tooltip;
				m_ToolTip.ShowToolTip();
				m_TooltipTab=tab;
			}
		}

		/// <summary>
		/// Resets Hoover timer.
		/// </summary>
		private void ResetHover()
		{
			NativeFunctions.TRACKMOUSEEVENT tme=new NativeFunctions.TRACKMOUSEEVENT();
			tme.dwFlags=NativeFunctions.TME_QUERY;
			tme.hwndTrack=this.Handle;
			tme.cbSize=Marshal.SizeOf(tme);
			NativeFunctions.TrackMouseEvent(ref tme);
			tme.dwFlags=tme.dwFlags | NativeFunctions.TME_HOVER;
			NativeFunctions.TrackMouseEvent(ref tme);
		}

		private void InternalMouseHover()
		{
			Point p=Control.MousePosition;
			p=this.PointToClient(p);
			TabItem tab=HitTest(p.X,p.Y);

            if (tab != null)
            {
                tab.InvokeMouseHover(new EventArgs());
            }

			if(tab==null)
			{
				HideToolTip();
			}
			else if(m_TooltipTab!=tab)
			{
				HideToolTip();

                if (this.Style == eTabStripStyle.Office2007Document)
                {
                    Rectangle r = GetSystemBoxRectangle();
                    if (!r.IsEmpty && r.IntersectsWith(tab.DisplayRectangle))
                        return;
                }

				ShowToolTip(tab);
			}
		}
		#endregion
	}

	/// <summary>
	/// Represents the event arguments for tab selection events.
	/// </summary>
	public class TabStripTabChangedEventArgs : EventArgs 
	{
		/// <summary>
		/// Currently selected tab.
		/// </summary>
		public readonly TabItem OldTab;
		/// <summary>
		/// Tab being selected.
		/// </summary>
		public readonly TabItem NewTab;
		/// <summary>
		/// Specifies the action that caused the event.
		/// </summary>
		public readonly eEventSource EventSource=eEventSource.Code;
 
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="oldtab">Currently selected tab.</param>
		/// <param name="newtab">New selection.</param>
		public TabStripTabChangedEventArgs(TabItem oldtab,TabItem newtab,eEventSource source) 
		{
			this.OldTab=oldtab;
			this.NewTab=newtab;
			this.EventSource=source;
		}
	}

	/// <summary>
	/// Represents the event arguments for tab selection events.
	/// </summary>
	public class TabStripTabChangingEventArgs : EventArgs 
	{
		/// <summary>
		/// Currently selected tab.
		/// </summary>
		public readonly TabItem OldTab;
		/// <summary>
		/// Tab being selected.
		/// </summary>
		public readonly TabItem NewTab;
		/// <summary>
		/// Cancels the selection operation.
		/// </summary>
		public bool Cancel=false;
		/// <summary>
		/// Specifies the action that caused the event.
		/// </summary>
		public readonly eEventSource EventSource=eEventSource.Code;
 
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="oldtab">Currently selected tab.</param>
		/// <param name="newtab">New selection.</param>
		public TabStripTabChangingEventArgs(TabItem oldtab,TabItem newtab,eEventSource source) 
		{
			this.OldTab=oldtab;
			this.NewTab=newtab;
			this.EventSource=source;
		}
	}

	/// <summary>
	/// Represents the event arguments for tab moving events.
	/// </summary>
	public class TabStripTabMovedEventArgs : EventArgs 
	{
		/// <summary>
		/// Tab being moved.
		/// </summary>
		public readonly TabItem Tab;
		/// <summary>
		/// Moved from index.
		/// </summary>
		public readonly int OldIndex;
		/// <summary>
		/// Moving to index.
		/// </summary>
		public readonly int NewIndex;
		/// <summary>
		/// Cancels the operation.
		/// </summary>
		public bool Cancel=false;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="tab">Tab</param>
		/// <param name="oldindex">Old Index</param>
		/// <param name="newindex">New Index</param>
		public TabStripTabMovedEventArgs(TabItem tab,int oldindex, int newindex) 
		{
			this.Tab=tab;
			this.OldIndex=oldindex;
			this.NewIndex=newindex;
		}
	}

	/// <summary>
	/// Represents the event arguments for action events.
	/// </summary>
	public class TabStripActionEventArgs:EventArgs
	{
		/// <summary>
		/// Cancels the operation.
		/// </summary>
		public bool Cancel=false;
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public TabStripActionEventArgs(){}
	}
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class TabSystemBox
	{
		public Rectangle DisplayRectangle=Rectangle.Empty;
		private Rectangle m_CloseRect=Rectangle.Empty;
		private Rectangle m_ForwardRect=Rectangle.Empty;
		private Rectangle m_BackRect=Rectangle.Empty;
		public int DefaultWidth=43;
		private bool m_BackEnabled=true;
		private bool m_ForwardEnabled=true;
		private bool m_CloseVisible=true;
		public event EventHandler Back;
		public event EventHandler Forward;
		public event EventHandler Close;
		private eMouseState m_CloseMouseState=eMouseState.None;
		private eMouseState m_BackMouseState=eMouseState.None;
		private eMouseState m_ForwardMouseState=eMouseState.None;
		private TabStrip m_Parent=null;
		protected ToolTip m_ToolTipWnd=null;
		private Timer m_ClickTimer=null;
		private bool m_ScrollBack=false;
		private bool m_ScrollForward=false;
		public bool Visible=true;

		public TabSystemBox(TabStrip parent)
		{
			m_Parent=parent;
		}

		public void Dispose()
		{
			this.DestroyTimer();
		}

		public bool CloseVisible
		{
			get {return m_CloseVisible;}
			set
			{
				m_CloseVisible=value;
				if(m_CloseVisible)
					DefaultWidth=43;
				else
					DefaultWidth=32;
			}
		}

		public Rectangle ForwardRect
		{
			get {return m_ForwardRect;}
		}

		public Rectangle BackRect
		{
			get {return m_BackRect;}
		}

		private void DestroyTimer()
		{
			m_ScrollBack=false;
			m_ScrollForward=false;
			if(m_ClickTimer!=null)
			{
				m_ClickTimer.Stop();
				m_ClickTimer.Enabled=false;
				m_ClickTimer.Dispose();
				m_ClickTimer=null;
			}
		}

		public bool ForwardEnabled
		{
			get {return m_ForwardEnabled;}
			set
			{
				if(m_ForwardEnabled!=value)
				{
					m_ForwardEnabled=value;
					if(!m_ForwardEnabled)
						DestroyTimer();
				}
			}
		}

		public bool BackEnabled
		{
			get {return m_BackEnabled;}
			set
			{
				if(m_BackEnabled!=value)
				{
					m_BackEnabled=value;
					if(!m_BackEnabled)
						DestroyTimer();
				}
			}
		}

		public void MouseLeave()
		{
			HideToolTip();

			if(DisplayRectangle.IsEmpty)
				return;
			if(m_CloseMouseState!=eMouseState.None || m_BackMouseState!=eMouseState.None || m_ForwardMouseState!=eMouseState.None)
			{
				ResetButtonMouseState();
				m_Parent.Invalidate(DisplayRectangle,false);
			}
		}
		public void MouseHover()
		{
			HideToolTip();
			if(DisplayRectangle.IsEmpty)
				return;

		}
		public void MouseMove(MouseEventArgs e)
		{
			if(DisplayRectangle.IsEmpty)
				return;
			bool bInvalidate=false;
			if(m_CloseMouseState!=eMouseState.None || m_BackMouseState!=eMouseState.None || m_ForwardMouseState!=eMouseState.None)
			{
				ResetButtonMouseState();
				bInvalidate=true;
			}

			if(!DisplayRectangle.Contains(e.X,e.Y))
			{
				if(bInvalidate)
					m_Parent.Invalidate(DisplayRectangle,false);
				return;
			}

			if(m_CloseRect.Contains(e.X,e.Y))
			{
				if(CloseVisible && m_Parent.SelectedTab!=null)
				{
					if(e.Button==MouseButtons.Left)
						m_CloseMouseState=eMouseState.Down;
					else
						m_CloseMouseState=eMouseState.Hot;
					bInvalidate=true;
				}
			}
			else if(m_BackRect.Contains(e.X,e.Y))
			{
				if(BackEnabled)
				{
					if(e.Button==MouseButtons.Left)
						m_BackMouseState=eMouseState.Down;
					else
						m_BackMouseState=eMouseState.Hot;
					bInvalidate=true;
				}
			}
			else if(m_ForwardRect.Contains(e.X,e.Y))
			{
				if(ForwardEnabled)
				{
					if(e.Button==MouseButtons.Left)
						m_ForwardMouseState=eMouseState.Down;
					else
						m_ForwardMouseState=eMouseState.Hot;
					bInvalidate=true;
				}
			}

			if(bInvalidate)
			{
				Rectangle r=DisplayRectangle;
				r.Inflate(2,2);
				m_Parent.Invalidate(r,false);
			}
            
		}
        
		public void MouseDown(MouseEventArgs e)
		{
			if(e.Button!=MouseButtons.Left)
				return;

			if(DisplayRectangle.IsEmpty)
				return;
			if(CloseVisible && m_CloseRect.Contains(e.X,e.Y) && m_Parent.SelectedTab!=null)
				m_CloseMouseState=eMouseState.Down;
			else if(BackEnabled && m_BackRect.Contains(e.X,e.Y))
			{
				m_BackMouseState=eMouseState.Down;
				if(m_Parent.TabScrollAutoRepeat)
				{
					m_ScrollBack=true;
					m_ScrollForward=false;
					if(m_ClickTimer==null)
						m_ClickTimer=new Timer();
					m_ClickTimer.Interval=m_Parent.TabScrollRepeatInterval;
					m_ClickTimer.Tick+=new EventHandler(this.ScrollClickTimer);
					m_ClickTimer.Start();
				}
			}
			else if(ForwardEnabled && m_ForwardRect.Contains(e.X,e.Y))
			{
				m_ForwardMouseState=eMouseState.Down;
				if(m_Parent.TabScrollAutoRepeat)
				{
					m_ScrollForward=true;
					m_ScrollBack=false;
					if(m_ClickTimer==null)
						m_ClickTimer=new Timer();
					m_ClickTimer.Interval=m_Parent.TabScrollRepeatInterval;
					m_ClickTimer.Tick+=new EventHandler(this.ScrollClickTimer);
					m_ClickTimer.Start();
				}
			}

			if(m_CloseMouseState!=eMouseState.None || m_BackMouseState!=eMouseState.None || m_ForwardMouseState!=eMouseState.None)
				m_Parent.Invalidate(DisplayRectangle,false);
		}
		private void ScrollClickTimer(object sender, EventArgs e)
		{
			if(m_ScrollBack)
			{
				if(Back!=null)
					Back(this,new EventArgs());
			}
			else if(m_ScrollForward)
			{
				if(Forward!=null)
					Forward(this,new EventArgs());
			}
			else if(m_ClickTimer!=null)
				m_ClickTimer.Stop();
		}
		public void MouseUp(MouseEventArgs e)
		{
			DestroyTimer();

			if(e.Button!=MouseButtons.Left)
				return;

			if(DisplayRectangle.IsEmpty)
				return;
			bool bInvalidate=false;
			if(m_CloseMouseState!=eMouseState.None || m_BackMouseState!=eMouseState.None || m_ForwardMouseState!=eMouseState.None)
				bInvalidate=true;

			if(CloseVisible && m_CloseRect.Contains(e.X,e.Y) && m_Parent.SelectedTab!=null)
			{
				if(m_CloseMouseState==eMouseState.Down && Close!=null)
					Close(this,new EventArgs());
				ResetButtonMouseState();
				//m_CloseMouseState=eMouseState.Hot;
			}
			else if(BackEnabled && m_BackRect.Contains(e.X,e.Y) )
			{
				if(m_BackMouseState==eMouseState.Down && Back!=null)
					Back(this,new EventArgs());
				ResetButtonMouseState();
				m_BackMouseState=eMouseState.Hot;
			}
			else if(ForwardEnabled && m_ForwardRect.Contains(e.X,e.Y))
			{
				if(m_ForwardMouseState==eMouseState.Down && Forward!=null )
					Forward(this,new EventArgs());
				ResetButtonMouseState();
				m_ForwardMouseState=eMouseState.Hot;
			}

            if(bInvalidate || m_CloseMouseState!=eMouseState.None || m_BackMouseState!=eMouseState.None || m_ForwardMouseState!=eMouseState.None)
				m_Parent.Invalidate(DisplayRectangle,false);
		}
		public void MouseWheel(MouseEventArgs e)
		{
            if (m_Parent.VisibleTabCount <= 0) return;
			if(e.Delta>0)
			{
				if(Back!=null && BackEnabled)
					Back(this,new EventArgs());
			}
			else
			{
				if(Forward!=null && ForwardEnabled)
					Forward(this,new EventArgs());
			}
		}
		private void ResetButtonMouseState()
		{
			m_CloseMouseState=eMouseState.None;
			m_BackMouseState=eMouseState.None;
			m_ForwardMouseState=eMouseState.None;
		}
		public void Paint(Graphics g)
		{
            if (DisplayRectangle.IsEmpty)
                return;
			
			int x=DisplayRectangle.X;
			int y=DisplayRectangle.Y;
			Color colorDark=g.GetNearestColor(ControlPaint.Light(m_Parent.ColorScheme.TabItemText));
			//Color colorBorder=g.GetNearestColor(ControlPaint.LightLight(m_Parent.ColorScheme.TabBackground));
			if(Math.Abs(colorDark.GetBrightness()-m_Parent.ColorScheme.TabBackground.GetBrightness())<=.2)
				colorDark=(m_Parent.ColorScheme.TabBackground.GetBrightness()<.5?ControlPaint.Light(m_Parent.ColorScheme.TabBackground):ControlPaint.Dark(m_Parent.ColorScheme.TabBackground));
			
			SolidBrush brush=null;
			Pen pen=new Pen(colorDark,1);
			if(BackEnabled || ForwardEnabled)
				brush=new SolidBrush(colorDark);

			if(m_Parent.TabAlignment==eTabStripAlignment.Left || m_Parent.TabAlignment==eTabStripAlignment.Right)
			{
				// Offset position to center the system items...
				y+=(DisplayRectangle.Height-(m_CloseVisible?32:18))/2;
				x+=(DisplayRectangle.Width-9)/2;
				Point[] p=new Point[3];
				p[0].X=x+4;
				p[0].Y=y;
				p[1].X=x;
				p[1].Y=y+4;
				p[2].X=x+8;
				p[2].Y=y+4;

				m_BackRect=new Rectangle(x-3,y-5,14,14);
				if(BackEnabled)
				{
					if(m_BackMouseState==eMouseState.Hot)
						DrawBackgroundHot(g,m_BackRect); // BarFunctions.DrawBorder3D(g,m_BackRect,Border3DStyle.RaisedInner,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					else if(m_BackMouseState==eMouseState.Down)
						DrawBackgroundPressed(g,m_BackRect); // BarFunctions.DrawBorder3D(g,m_BackRect,Border3DStyle.SunkenOuter,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					g.FillPolygon(brush,p);
				}
				g.DrawPolygon(pen,p);

				y+=14;
				p[0].X=x;
				p[0].Y=y;
				p[1].X=x+8;
				p[1].Y=y;
				p[2].X=x+4;
				p[2].Y=y+4;
				m_ForwardRect=new Rectangle(x-3,y-5,14,14);
				if(ForwardEnabled)
				{
					if(m_ForwardMouseState==eMouseState.Hot)
						DrawBackgroundHot(g,m_ForwardRect); // BarFunctions.DrawBorder3D(g,m_ForwardRect,Border3DStyle.RaisedInner,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					else if(m_ForwardMouseState==eMouseState.Down)
						DrawBackgroundPressed(g,m_ForwardRect); // BarFunctions.DrawBorder3D(g,m_ForwardRect,Border3DStyle.SunkenOuter,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					g.FillPolygon(brush,p);
				}
				g.DrawPolygon(pen,p);

				y+=13;
				if(CloseVisible)
				{
					m_CloseRect=new Rectangle(x-3,y-3,14,14);
					if(CloseVisible)
					{
						if(m_CloseMouseState==eMouseState.Hot)
							DrawBackgroundHot(g,m_CloseRect); // BarFunctions.DrawBorder3D(g,m_CloseRect,Border3DStyle.RaisedInner,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
						else if(m_CloseMouseState==eMouseState.Down)
							DrawBackgroundPressed(g,m_CloseRect); //BarFunctions.DrawBorder3D(g,m_CloseRect,Border3DStyle.SunkenOuter,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					}
					g.DrawLine(pen,x+1,y,x+7,y+6);
					g.DrawLine(pen,x+1,y+1,x+7,y+7);
					g.DrawLine(pen,x+1,y+6,x+7,y);
					g.DrawLine(pen,x+1,y+7,x+7,y+1);
				}
			}
			else
			{
				// Offset position to center the system items...
				x+=(DisplayRectangle.Width-(m_CloseVisible?32:18))/2;
				y+=(DisplayRectangle.Height-9)/2;
				Point[] p=new Point[3];
				p[0].X=x+4;
				p[0].Y=y;
				p[1].X=x+4;
				p[1].Y=y+8;
				p[2].X=x;
				p[2].Y=y+4;
				
				m_BackRect=new Rectangle(x-5,y-3,14,14);
				if(BackEnabled)
				{
					if(m_BackMouseState==eMouseState.Hot)
						DrawBackgroundHot(g,m_BackRect); // BarFunctions.DrawBorder3D(g,m_BackRect,Border3DStyle.RaisedInner,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					else if(m_BackMouseState==eMouseState.Down)
						DrawBackgroundPressed(g,m_BackRect); // BarFunctions.DrawBorder3D(g,m_BackRect,Border3DStyle.SunkenOuter,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					g.FillPolygon(brush,p);
				}
				g.DrawPolygon(pen,p);
				
				x+=14;
                p[0].X=x;
				p[0].Y=y;
				p[1].X=x;
				p[1].Y=y+8;
				p[2].X=x+4;
				p[2].Y=y+4;

				m_ForwardRect=new Rectangle(x-5,y-3,14,14);
				if(ForwardEnabled)
				{
					if(m_ForwardMouseState==eMouseState.Hot)
						DrawBackgroundHot(g,m_ForwardRect); // BarFunctions.DrawBorder3D(g,m_ForwardRect,Border3DStyle.RaisedInner,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					else if(m_ForwardMouseState==eMouseState.Down)
						DrawBackgroundPressed(g,m_ForwardRect); // BarFunctions.DrawBorder3D(g,m_ForwardRect,Border3DStyle.SunkenOuter,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					g.FillPolygon(brush,p);
				}
				g.DrawPolygon(pen,p);

				x+=13;
				if(CloseVisible)
				{
					m_CloseRect=new Rectangle(x-4,y-3,14,14);
					if(CloseVisible)
					{
						if(m_CloseMouseState==eMouseState.Hot)
							DrawBackgroundHot(g,m_CloseRect); // BarFunctions.DrawBorder3D(g,m_CloseRect,Border3DStyle.RaisedInner,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
						else if(m_CloseMouseState==eMouseState.Down)
							DrawBackgroundPressed(g,m_CloseRect); // BarFunctions.DrawBorder3D(g,m_CloseRect,Border3DStyle.SunkenOuter,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
					}
                    g.DrawLine(pen,x,y+1,x+6,y+7);
					g.DrawLine(pen,x+1,y+1,x+7,y+7);
					g.DrawLine(pen,x+6,y+1,x,y+7);
					g.DrawLine(pen,x+7,y+1,x+1,y+7);
				}
			}
			pen.Dispose();
			if(brush!=null)
				brush.Dispose();
		}

		private void DrawBackgroundHot(Graphics g, Rectangle r)
		{
			Color border=m_Parent.ColorScheme.TabItemHotBorder;
			Color back=m_Parent.ColorScheme.TabItemHotBackground;
			Color back2=m_Parent.ColorScheme.TabItemHotBackground2;

			if((m_Parent.Style==eTabStripStyle.Office2003 || m_Parent.Style==eTabStripStyle.VS2005 || m_Parent.Style==eTabStripStyle.VS2005Document || m_Parent.Style==eTabStripStyle.VS2005Dock) && back.IsEmpty && back2.IsEmpty)
			{
				ColorScheme cs;
				if(m_Parent.Style==eTabStripStyle.Office2003)
					cs=new ColorScheme(eDotNetBarStyle.Office2003);
				else
                    cs=new ColorScheme(eDotNetBarStyle.VS2005);
				back=cs.ItemHotBackground;
				back2=cs.ItemHotBackground2;
				border=cs.ItemHotBorder;
			}

            if (m_Parent.Style == eTabStripStyle.OneNote || m_Parent.Style == eTabStripStyle.Office2007Document || m_Parent.Style == eTabStripStyle.Office2007Dock || m_Parent.Style == eTabStripStyle.Office2003 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005Document || m_Parent.Style == eTabStripStyle.VS2005Dock)
			{
				if(back2.IsEmpty)
				{
					if(!back.IsEmpty)
					{
						using(SolidBrush brush=new SolidBrush(back))
							g.FillRectangle(brush,r);
					}
				}
				else
				{
					using(LinearGradientBrush brush=new LinearGradientBrush(r,back,back2,m_Parent.ColorScheme.TabItemHotBackgroundGradientAngle))
						g.FillRectangle(brush,r);
				}
				using(Pen pen=new Pen(border,1))
					g.DrawRectangle(pen,r);
			}
			else
			{
				Color colorBorder=g.GetNearestColor(ControlPaint.LightLight(m_Parent.ColorScheme.TabBackground));
				BarFunctions.DrawBorder3D(g,r,Border3DStyle.RaisedInner,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
			}
		}

		private void DrawBackgroundPressed(Graphics g, Rectangle r)
		{
			Color border=m_Parent.ColorScheme.TabItemHotBorder;
			Color back=m_Parent.ColorScheme.TabItemHotBackground;
			Color back2=m_Parent.ColorScheme.TabItemHotBackground2;

			if((m_Parent.Style==eTabStripStyle.Office2003 || m_Parent.Style==eTabStripStyle.VS2005 || m_Parent.Style==eTabStripStyle.VS2005Document || m_Parent.Style==eTabStripStyle.VS2005Dock)&& back.IsEmpty && back2.IsEmpty)
			{
				ColorScheme cs;
				if(m_Parent.Style==eTabStripStyle.Office2003)
					cs=new ColorScheme(eDotNetBarStyle.Office2003);
				else
					cs=new ColorScheme(eDotNetBarStyle.VS2005);
				back=cs.ItemPressedBackground;
				back2=cs.ItemPressedBackground2;
				border=cs.ItemPressedBorder;
			}

            if (m_Parent.Style == eTabStripStyle.OneNote || m_Parent.Style == eTabStripStyle.Office2007Document || m_Parent.Style == eTabStripStyle.Office2007Dock || m_Parent.Style == eTabStripStyle.Office2003 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005Document)
			{
				if(back2.IsEmpty)
				{
					if(!back.IsEmpty)
					{
						using(SolidBrush brush=new SolidBrush(back))
							g.FillRectangle(brush,r);
					}
				}
				else
				{
					using(LinearGradientBrush brush=new LinearGradientBrush(r,back2,back,m_Parent.ColorScheme.TabItemHotBackgroundGradientAngle))
						g.FillRectangle(brush,r);
				}
				using(Pen pen=new Pen(border,1))
					g.DrawRectangle(pen,r);
			}
			else
			{
				Color colorBorder=g.GetNearestColor(ControlPaint.LightLight(m_Parent.ColorScheme.TabBackground));
				BarFunctions.DrawBorder3D(g,r,Border3DStyle.SunkenOuter,Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom,colorBorder,false);
			}
		}

		/// <summary>
		/// Destroys tooltip window.
		/// </summary>
		private void HideToolTip()
		{
			if(m_ToolTipWnd!=null)
			{
				m_ToolTipWnd.Hide();
				m_ToolTipWnd.Dispose();
				m_ToolTipWnd=null;
			}
		}
	}

    #region TabStrip Enums
    /// <summary>
    /// Specifies the tab alignment inside the Tab-Strip control.
    /// </summary>
    public enum eTabStripAlignment : int
    {
        /// <summary>
        /// Tabs are left aligned.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Tabs are right aligned.
        /// </summary>
        Right = 1,
        /// <summary>
        /// Tabs are top aligned.
        /// </summary>
        Top = 2,
        /// <summary>
        /// Tabs are bottom aligned.
        /// </summary>
        Bottom = 3
    }

    /// <summary>
    /// Indicates tab strip style.
    /// </summary>
    public enum eTabStripStyle : int
    {
        /// <summary>
        /// Default VS.NET like flat style.
        /// </summary>
        Flat = 0,
        /// <summary>
        /// Office 2003 like style.
        /// </summary>
        Office2003 = 2,
        /// <summary>
        /// OneNote like style.
        /// </summary>
        OneNote = 3,
        /// <summary>
        /// VS.NET 2005 style tabs.
        /// </summary>
        VS2005,
        /// <summary>
        /// Tab style where tabs are centered and first and last tab have the corners rounded. This style does not support multi-line tabs or tab scrolling.
        /// </summary>
        RoundHeader,
        /// <summary>
        /// VS.NET 2005 dock style tabs.
        /// </summary>
        VS2005Dock,
        /// <summary>
        /// VS.NET 2005 document style tabs.
        /// </summary>
        VS2005Document,
        /// <summary>
        /// Simulated theme style with the horizontal text alignment at all times.
        /// </summary>
        SimulatedTheme,
        /// <summary>
        /// Office 2007 document style.
        /// </summary>
        Office2007Document,
        /// <summary>
        /// Office 2007 dock style.
        /// </summary>
        Office2007Dock
    }
    #endregion

    #region Measure, Render Events
    /// <summary>
    /// Defines delegate for the MeasureTabItem event.
    /// </summary>
    public delegate void MeasureTabItemEventHandler(object sender, MeasureTabItemEventArgs e);

    /// <summary>
    /// Represents event arguments for MeasureTabItem event.
	/// </summary>
    public class MeasureTabItemEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the TabItem that is being measured.
        /// </summary>
        public readonly TabItem TabItem;
        /// <summary>
        /// Gets or sets the size of the TabItem. The default size calculated by the control will be set by default. You can inspect it and change it to the
        /// custom size by setting this property.
        /// </summary>
        public Size Size = Size.Empty;

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        /// <param name="tab">TabItem being measured.</param>
        /// <param name="size">Default size.</param>
        public MeasureTabItemEventArgs(TabItem tab, Size size)
        {
            this.TabItem = tab;
            this.Size = size;
        }
    }

    /// <summary>
    /// Defines delegate for the PreRenderTabItem and PostRenderTabItem events.
    /// </summary>
    public delegate void RenderTabItemEventHandler(object sender, RenderTabItemEventArgs e);

    /// <summary>
    /// Represents event arguments for PreRenderTabItem and PostRenderTabItem event.
	/// </summary>
    public class RenderTabItemEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the reference to the TabItem being rendered. You can use properties like DisplayRectangle to determine the rendering bounds for the tab.
        /// </summary>
        public readonly TabItem TabItem;

        /// <summary>
        /// When used in PreRenderTabItem event allows you to cancel the default rendering by setting this property to true.
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        /// Gets the reference to the Graphics object to render the tab on.
        /// </summary>
        public readonly Graphics Graphics;

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        /// <param name="tab">Default value for TabItem property.</param>
        /// <param name="g">Default value  for Graphics property.</param>
        public RenderTabItemEventArgs(TabItem tab, Graphics g)
        {
            this.TabItem = tab;
            this.Graphics = g;
        }
    }
    #endregion
}
