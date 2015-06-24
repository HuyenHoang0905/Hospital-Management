using System;
using System.Drawing;
namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ItemPaintArgs.
	/// </summary>
	public class ItemPaintArgs
	{
		public System.Drawing.Graphics Graphics;
		public ColorScheme Colors;
		public System.Windows.Forms.Control ContainerControl;
		public eTextFormat ButtonStringFormat;
		public bool IsOnMenu=false;
        public bool IsOnPopupBar = false;
		public bool IsOnMenuBar=false;
        public bool IsOnRibbonBar = false;
        public bool IsOnNavigationBar = false;
		public System.Drawing.Font Font;
		public IOwner Owner;
        public bool RightToLeft = false;
		private DevComponents.DotNetBar.ThemeWindow m_ThemeWindow=null;
		private DevComponents.DotNetBar.ThemeRebar m_ThemeRebar=null;
		private DevComponents.DotNetBar.ThemeToolbar m_ThemeToolbar=null;
		private DevComponents.DotNetBar.ThemeHeader m_ThemeHeader=null;
		private DevComponents.DotNetBar.ThemeScrollBar m_ThemeScrollBar=null;
		private DevComponents.DotNetBar.ThemeExplorerBar m_ThemeExplorerBar=null;
		private DevComponents.DotNetBar.ThemeProgress m_ThemeProgress=null;
        private DevComponents.DotNetBar.ThemeButton m_ThemeButton = null;
        public bool DesignerSelection = false;
        public bool GlassEnabled = false;
        private Rendering.BaseRenderer m_Renderer = null;
        public ButtonItemRendererEventArgs ButtonItemRendererEventArgs = new ButtonItemRendererEventArgs();
        public Rectangle ClipRectangle = Rectangle.Empty;
        public bool ControlExpanded = true;
        internal bool CachedPaint = false;
        internal bool IsDefaultButton = false;
        public bool IsFlatOffice2007Representation = false;

		public ItemPaintArgs(IOwner owner, System.Windows.Forms.Control control, System.Drawing.Graphics g, ColorScheme scheme)
		{
			this.Graphics=g;
			this.Colors=scheme;
			this.ContainerControl=control;
			this.Owner=owner;
            if(control!=null)
                this.RightToLeft = (control.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
            if (control is MenuPanel || this.ContainerControl is ItemsListBox)
                this.IsOnMenu = true;
            else if (control is Bar && ((Bar)control).MenuBar)
                this.IsOnMenuBar = true;
            else if (control is Bar && ((Bar)control).BarState == eBarState.Popup)
                this.IsOnPopupBar = true;
            else if (control is RibbonBar)
                this.IsOnRibbonBar = true;
            else if (control is NavigationBar)
                this.IsOnNavigationBar = true;
            if(control!=null)
			    this.Font=control.Font;
			CreateStringFormat();
		}

        internal Rendering.BaseRenderer Renderer
        {
            get { return m_Renderer; }
            set { m_Renderer = value; }
        }

		internal DevComponents.DotNetBar.ThemeWindow ThemeWindow
		{
			get
			{
				if(m_ThemeWindow==null)
				{
					if(this.ContainerControl is Bar)
						m_ThemeWindow=((Bar)this.ContainerControl).ThemeWindow;
					else if(this.ContainerControl is IThemeCache)
						m_ThemeWindow=((IThemeCache)this.ContainerControl).ThemeWindow;

				}
				return m_ThemeWindow;
			}
		}
		internal DevComponents.DotNetBar.ThemeRebar ThemeRebar
		{
			get
			{
				if(m_ThemeRebar==null)
				{
					if(this.ContainerControl is Bar)
						m_ThemeRebar=((Bar)this.ContainerControl).ThemeRebar;
					else if(this.ContainerControl is IThemeCache)
						m_ThemeRebar=((IThemeCache)this.ContainerControl).ThemeRebar;
				}
				return m_ThemeRebar;
			}
		}
		internal DevComponents.DotNetBar.ThemeToolbar ThemeToolbar
		{
			get
			{
				if(m_ThemeToolbar==null)
				{
					if(this.ContainerControl is Bar)
						m_ThemeToolbar=((Bar)this.ContainerControl).ThemeToolbar;
					else if(this.ContainerControl is IThemeCache)
						m_ThemeToolbar=((IThemeCache)this.ContainerControl).ThemeToolbar;
				}
				return m_ThemeToolbar;
			}
		}
		internal DevComponents.DotNetBar.ThemeHeader ThemeHeader
		{
			get
			{
				if(m_ThemeHeader==null)
				{
					if(this.ContainerControl is Bar)
						m_ThemeHeader=((Bar)this.ContainerControl).ThemeHeader;
					else if(this.ContainerControl is IThemeCache)
						m_ThemeHeader=((IThemeCache)this.ContainerControl).ThemeHeader;						
				}
				return m_ThemeHeader;
			}
		}
		internal DevComponents.DotNetBar.ThemeScrollBar ThemeScrollBar
		{
			get
			{
				if(m_ThemeScrollBar==null)
				{
					if(this.ContainerControl is Bar)
						m_ThemeScrollBar=((Bar)this.ContainerControl).ThemeScrollBar;
					else if(this.ContainerControl is IThemeCache)
						m_ThemeScrollBar=((IThemeCache)this.ContainerControl).ThemeScrollBar;
				}
				return m_ThemeScrollBar;
			}
		}

		internal DevComponents.DotNetBar.ThemeExplorerBar ThemeExplorerBar
		{
			get
			{
				if(m_ThemeExplorerBar==null)
				{
					if(this.ContainerControl is IThemeCache)
						m_ThemeExplorerBar=((IThemeCache)this.ContainerControl).ThemeExplorerBar;
				}
				return m_ThemeExplorerBar;
			}
		}

		internal DevComponents.DotNetBar.ThemeProgress ThemeProgress
		{
			get
			{
				if(m_ThemeProgress==null)
				{
					if(this.ContainerControl is Bar)
						m_ThemeProgress=((Bar)this.ContainerControl).ThemeProgress;
					else if(this.ContainerControl is IThemeCache)
						m_ThemeProgress=((IThemeCache)this.ContainerControl).ThemeProgress;
				}
				return m_ThemeProgress;
			}
		}

        internal DevComponents.DotNetBar.ThemeButton ThemeButton
        {
            get
            {
                if (m_ThemeButton == null)
                {
                    if (this.ContainerControl is IThemeCache)
                        m_ThemeButton = ((IThemeCache)this.ContainerControl).ThemeButton;
                }
                return m_ThemeButton;
            }
        }

		private void CreateStringFormat()
		{
            eTextFormat sfmt = eTextFormat.Default;
            if (this.ContainerControl is ItemControl && !(this.Owner!=null && this.Owner.AlwaysDisplayKeyAccelerators))
                sfmt |= eTextFormat.HidePrefix;
			else if(!((this.Owner!=null && this.Owner.AlwaysDisplayKeyAccelerators) || NativeFunctions.ShowKeyboardCues || this.IsOnMenu)) 
			{
				Bar bar=this.ContainerControl as Bar;
                if (!((this.ContainerControl != null && this.ContainerControl.Focused) || (bar != null && bar.MenuFocus) || (bar != null && bar.AlwaysDisplayKeyAccelerators) || this.ContainerControl is NavigationBar))
                    sfmt |= eTextFormat.HidePrefix;
			}

            sfmt |= eTextFormat.SingleLine | eTextFormat.VerticalCenter;
            if (!TextDrawing.UseTextRenderer)
                sfmt |= eTextFormat.EndEllipsis;
			this.ButtonStringFormat=sfmt;
		}
	}
}
