using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for MDISystemItem.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class MDISystemItem:PopupItem
	{
		private bool m_IsSystemIcon;
		private Icon m_Icon;

		private bool m_MinimizeEnabled, m_RestoreEnabled, m_CloseEnabled;
		private SystemButton m_MouseOver;
		private SystemButton m_MouseDown;
		private SystemButton m_LastButtonClick;

		public MDISystemItem():this("") {}
		public MDISystemItem(string sName):base(sName)
		{
			this.GlobalItem=false;
			this.SetSystemItem(true);
			m_IsSystemIcon=false;
			m_Icon=null;
			m_MinimizeEnabled=true;
			m_RestoreEnabled=true;
			m_CloseEnabled=true;
			m_MouseOver=SystemButton.None;
			m_MouseDown=SystemButton.None;
			m_LastButtonClick=SystemButton.None;
			this.CanCustomize=false;
			this.Tooltip="";
			this.ShowSubItems=false;
			m_ShouldSerialize=false;
			this.IsAccessible=false;
		}

        protected override void Dispose(bool disposing)
        {
            if (m_Icon != null)
            {
                m_Icon.Dispose();
                m_Icon = null;
            }
            foreach (BaseItem item in this.SubItems)
            {
                ButtonItem button = item as ButtonItem;
                if (button != null && button.Image != null)
                {
                    button.Image.Dispose();
                    button.Image = null;
                }
            }
            base.Dispose(disposing);
        }

		/// <summary>
		/// Returns copy of CustomizeItem item
		/// </summary>
		public override BaseItem Copy()
		{
			MDISystemItem objCopy=new MDISystemItem();
			this.CopyToItem(objCopy);
			objCopy.MinimizeEnabled=m_MinimizeEnabled;
			objCopy.RestoreEnabled=m_RestoreEnabled;
			objCopy.CloseEnabled=m_CloseEnabled;
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			MDISystemItem objCopy=copy as MDISystemItem;
			base.CopyToItem(objCopy);
            objCopy.IsSystemIcon=m_IsSystemIcon;
		}
		
		/// <summary>
		/// MDI System Item can render itself as either Simple icon with system drop down menu or set of
		/// system buttons Minimize, Restore and Close. This item is rendered on the Bar that is designated as
		/// Menu bar and when bar is used on MDI form and MDI Child form is maximized.
		/// </summary>
		public bool IsSystemIcon
		{
			get
			{
				return m_IsSystemIcon;
			}
			set
			{
				if(m_IsSystemIcon!=value)
				{
					m_IsSystemIcon=value;
					NeedRecalcSize=true;
					if(m_IsSystemIcon)
					{
						CreateSystemMenu();
						this.ShowSubItems=true;
					}
					else
					{
						this.SubItems.Clear();
						this.ShowSubItems=false;
					}
					this.ShowSubItems=m_IsSystemIcon;
					this.Refresh();
				}
			}
		}

		public override void Paint(ItemPaintArgs pa)
		{
			if(this.SuspendLayout)
				return;

            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                if (pa.Renderer != null)
                {
                    pa.Renderer.DrawMdiSystemItem(new MdiSystemItemRendererEventArgs(pa.Graphics, this));
                    return;
                }
            }

			System.Drawing.Graphics g=pa.Graphics;
			Rectangle r;
			
			if(m_IsSystemIcon)
			{
				r=this.DisplayRectangle;
				r.Offset((r.Width-16)/2,(r.Height-16)/2);
				if(m_Icon!=null)
					g.DrawIconUnstretched(m_Icon,r);
				return;
			}

            if (this.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
                r = new Rectangle(this.DisplayRectangle.Location, GetButtonSize());
				r.Inflate(-1,-2);
				r.Location=this.DisplayRectangle.Location;

				if(this.Orientation==eOrientation.Horizontal)
					r.Offset(0,(this.DisplayRectangle.Height-r.Height)/2);
				else
					r.Offset((this.WidthInternal-r.Width)/2,0);

				if(!m_MinimizeEnabled)
					System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Minimize,System.Windows.Forms.ButtonState.Inactive);
				else if(m_MouseDown==SystemButton.Minimize)
					System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Minimize,System.Windows.Forms.ButtonState.Pushed);
				else
					System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Minimize,System.Windows.Forms.ButtonState.Normal);
				
				if(this.Orientation==eOrientation.Horizontal)
					r.Offset(r.Width+1,0);
				else
					r.Offset(0,r.Height+1);

				if(!m_RestoreEnabled)
                    System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Restore,System.Windows.Forms.ButtonState.Inactive);
				else if(m_MouseDown==SystemButton.Restore)
					System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Restore,System.Windows.Forms.ButtonState.Pushed);
				else
					System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Restore,System.Windows.Forms.ButtonState.Normal);

				if(this.Orientation==eOrientation.Horizontal)
					r.Offset(r.Width+3,0);
				else
					r.Offset(0,r.Height+3);

				if(!m_CloseEnabled)
                    System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Close,System.Windows.Forms.ButtonState.Inactive);
				else if(m_MouseDown==SystemButton.Close)
					System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Close,System.Windows.Forms.ButtonState.Pushed);
				else
					System.Windows.Forms.ControlPaint.DrawCaptionButton(g,r,System.Windows.Forms.CaptionButton.Close,System.Windows.Forms.ButtonState.Normal);
			}
			else
			{
                r = new Rectangle(this.DisplayRectangle.Location, GetButtonSize());
				r.Inflate(-1,-1);
				if(this.Orientation==eOrientation.Horizontal)
					r.Offset(0,(this.DisplayRectangle.Height-r.Height)/2);
				else
					r.Offset((this.WidthInternal-r.Width)/2,0);

				this.PaintButton(pa,SystemButton.Minimize,r);

				if(this.Orientation==eOrientation.Horizontal)
					r.Offset(r.Width,0);
				else
					r.Offset(0,r.Height);
				this.PaintButton(pa,SystemButton.Restore,r);

				if(this.Orientation==eOrientation.Horizontal)
					r.Offset(r.Width+2,0);
				else
					r.Offset(0,r.Height+2);
				this.PaintButton(pa,SystemButton.Close,r);

			}
		}

		private void PaintButton(ItemPaintArgs pa, SystemButton btn, Rectangle r)
		{
			System.Drawing.Graphics g=pa.Graphics;

			if(this.IsThemed)
			{
				System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
				if(container!=null)
				{
					ThemeWindow theme=pa.ThemeWindow;
					ThemeWindowParts part=ThemeWindowParts.MdiMinButton;
					ThemeWindowStates state=ThemeWindowStates.ButtonNormal;
					switch(btn)
					{
						case SystemButton.Close:
						{
							part=ThemeWindowParts.MdiCloseButton;
							break;
						}
						case SystemButton.Help:
						{
							part=ThemeWindowParts.MdiHelpButton;
							break;
						}
						case SystemButton.Restore:
						{
							part=ThemeWindowParts.MdiRestoreButton;
							break;
						}
					}
					if(btn==m_MouseDown)
						state=ThemeWindowStates.ButtonPushed;
					else if(btn==m_MouseOver)
						state=ThemeWindowStates.ButtonHot;

                    theme.DrawBackground(g,part,state,r);
					return;
				}
			}

			// Draw state if any
			if(btn==m_MouseDown)
			{
				if(pa.Colors.ItemPressedBackground2.IsEmpty)
					g.FillRectangle(new SolidBrush(pa.Colors.ItemPressedBackground),r);
				else
				{
					System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemPressedBackground,pa.Colors.ItemPressedBackground2,pa.Colors.ItemPressedBackgroundGradientAngle);
					g.FillRectangle(gradient,r);
					gradient.Dispose();
				}
				NativeFunctions.DrawRectangle(g,SystemPens.Highlight,r);
			}
			else if(btn==m_MouseOver)
			{
				if(!pa.Colors.ItemHotBackground2.IsEmpty)
				{
					System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle);
					g.FillRectangle(gradient,r);
					gradient.Dispose();
				}
				else
					g.FillRectangle(new SolidBrush(pa.Colors.ItemHotBackground),r);
				NativeFunctions.DrawRectangle(g,new Pen(pa.Colors.ItemHotBorder),r);
			}

            using (Bitmap bmp = GetButtonBitmap(g, btn, r, pa.Colors))
            {
                if (btn == SystemButton.Minimize && !m_MinimizeEnabled ||
                    btn == SystemButton.Restore && !m_RestoreEnabled ||
                    btn == SystemButton.Close && !m_CloseEnabled)
                {
                    float[][] array = new float[5][];
                    array[0] = new float[5] { 0, 0, 0, 0, 0 };
                    array[1] = new float[5] { 0, 0, 0, 0, 0 };
                    array[2] = new float[5] { 0, 0, 0, 0, 0 };
                    array[3] = new float[5] { .5f, .5f, .5f, .5f, 0 };
                    array[4] = new float[5] { 0, 0, 0, 0, 0 };
                    System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                    System.Drawing.Imaging.ImageAttributes disabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
                    disabledImageAttr.ClearColorKey();
                    disabledImageAttr.SetColorMatrix(grayMatrix);
                    g.DrawImage(bmp, r, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, disabledImageAttr);
                }
                else
                {
                    if (btn == m_MouseDown)
                        r.Offset(1, 1);
                    g.DrawImageUnscaled(bmp, r);
                }
            }
		}

        internal Bitmap GetButtonBitmap(Graphics g, SystemButton btn, Rectangle r, ColorScheme colorScheme)
        {
            Bitmap bmp = new Bitmap(r.Width, r.Height, g);
            Graphics gBmp = Graphics.FromImage(bmp);
            Rectangle rBtn = new Rectangle(0, 0, r.Width, r.Height);
            rBtn.Inflate(0, -1);
            Rectangle rClip = rBtn;
            rClip.Inflate(-1, -1);
            using (SolidBrush brush = new SolidBrush(SystemColors.Control))
                gBmp.FillRectangle(brush, 0, 0, r.Width, r.Height);
            gBmp.SetClip(rClip);
            System.Windows.Forms.ControlPaint.DrawCaptionButton(gBmp, rBtn, (System.Windows.Forms.CaptionButton)btn, System.Windows.Forms.ButtonState.Flat);
            gBmp.ResetClip();
            gBmp.Dispose();

            if (!colorScheme.MdiSystemItemForeground.IsEmpty)
            {
                Bitmap bitmap = new Bitmap(bmp, bmp.Width, bmp.Height);
                using (Graphics graphics2 = Graphics.FromImage(bitmap))
                {
                    graphics2.Clear(Color.Transparent);
                    ImageAttributes imageAttrs = new ImageAttributes();
                    ColorMap map = new ColorMap();
                    map.OldColor = Color.Black;
                    map.NewColor = colorScheme.MdiSystemItemForeground;
                    imageAttrs.SetRemapTable(new ColorMap[] { map }, ColorAdjustType.Bitmap);
                    graphics2.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttrs, null, IntPtr.Zero);
                }
                bmp.Dispose();
                bmp=bitmap;
            }

            bmp.MakeTransparent(SystemColors.Control);

            return bmp;
        }

		protected override void OnTooltip(bool bShow)
		{
			base.OnTooltip(bShow);
			if(!bShow || this.IsSystemIcon)
				return;

			Point p=System.Windows.Forms.Control.MousePosition;
			System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
			if(ctrl!=null)
				p=ctrl.PointToClient(p);

			SystemButton btn=this.GetButton(p.X,p.Y);

			using(LocalizationManager lm=new LocalizationManager(this.GetOwner() as IOwnerLocalize))
			{
				if(btn==SystemButton.Minimize)
				{
					this.Tooltip=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMinimizeTooltip); //"Minimize";
				}
				else if(btn==SystemButton.Restore)
				{
					this.Tooltip=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemRestoreTooltip); //"Restore Down";
				}
                else if (btn == SystemButton.Maximize)
                {
                    this.Tooltip = lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMaximize);
                }
				else if(btn==SystemButton.Close)
				{
					this.Tooltip=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemCloseTooltip); //"Close";
				}
				else
					this.Tooltip="";
			}
		}

		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseDown(objArg);
            if (objArg.Button != System.Windows.Forms.MouseButtons.Left || this.DesignMode || !this.GetEnabled())
				return;

			if(this.IsSystemIcon)
			{
				if(m_Parent!=null)
				{
					if(this.Expanded)
						m_Parent.AutoExpand=false;
					else
						m_Parent.AutoExpand=true;
				}

				this.Expanded=!this.Expanded;
				return;
			}

			SystemButton btn=this.GetButton(objArg.X,objArg.Y);

            if (btn == SystemButton.Help)
            {
                m_MouseDown = SystemButton.Help;
                this.Refresh();
            }
			else if(m_MinimizeEnabled && btn==SystemButton.Minimize)
			{
				m_MouseDown=SystemButton.Minimize;
				this.Refresh();
			}
			else if(m_RestoreEnabled && btn==SystemButton.Restore)
			{
				m_MouseDown=SystemButton.Restore;
				this.Refresh();
			}
            else if (!m_RestoreEnabled && btn == SystemButton.Maximize)
            {
                m_MouseDown = SystemButton.Maximize;
                this.Refresh();
            }
			else if(m_CloseEnabled && btn==SystemButton.Close)
			{
				m_MouseDown=SystemButton.Close;
				this.Refresh();
			}
			else// if(m_MouseDown!=btn)
			{
				m_MouseDown=SystemButton.None;
				this.Refresh();
			}
		}

		public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseUp(objArg);
			if(m_MouseDown!=SystemButton.None)
			{
				m_MouseDown=SystemButton.None;
				this.Refresh();
			}

		}

		public override void InternalClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
		{
			if(this.IsSystemIcon)
			{
				m_LastButtonClick=SystemButton.None;
				base.InternalClick(mb,mpos);
				return;
			}

			Point p=mpos;
            //System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
            //if(ctrl!=null)
            //    p=ctrl.PointToClient(p);

			m_LastButtonClick=this.GetButton(p.X,p.Y);
			// Make sure that button is enabled
			if(m_LastButtonClick==SystemButton.Close && !m_CloseEnabled ||
				m_LastButtonClick==SystemButton.Minimize && !m_MinimizeEnabled ||
				m_LastButtonClick==SystemButton.Restore && !m_RestoreEnabled ||
                m_LastButtonClick == SystemButton.Maximize && m_RestoreEnabled)
				m_LastButtonClick=SystemButton.None;

            if (m_LastButtonClick != SystemButton.None)
                base.InternalClick(mb, mpos);
		}

		public override void InternalMouseLeave()
		{
			base.InternalMouseLeave();
			//if(m_MouseOver==SystemButton.None && (System.Windows.Forms.Control.MouseButtons!=System.Windows.Forms.MouseButtons.Left || m_MouseDown==SystemButton.None))
				//return;
			
			m_MouseDown=SystemButton.None;
			m_MouseOver=SystemButton.None;

			this.Refresh();
		}

		public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseMove(objArg);

			SystemButton btn=this.GetButton(objArg.X,objArg.Y);

            if (btn == SystemButton.Help)
            {
                // Help
                if (m_MouseOver != SystemButton.Help)
                {
                    m_MouseOver = SystemButton.Help;
                    if (this.ToolTipVisible)
                    {
                        this.HideToolTip();
                        this.ResetHover();
                    }
                    this.Refresh();
                }
                return;
            }

			if(btn==SystemButton.Minimize)
			{
				// Minimize
				if(m_MouseOver!=SystemButton.Minimize)
				{
					m_MouseOver=SystemButton.Minimize;
					if(this.ToolTipVisible)
					{
						this.HideToolTip();
						this.ResetHover();
					}
					this.Refresh();
				}
				return;
			}

			if(btn==SystemButton.Restore)
			{
				// Restore
				if(m_MouseOver!=SystemButton.Restore)
				{
					m_MouseOver=SystemButton.Restore;
					if(this.ToolTipVisible)
					{
						this.HideToolTip();
						this.ResetHover();
					}
					this.Refresh();
				}
				return;
			}

            if (btn == SystemButton.Maximize)
            {
                // Restore
                if (m_MouseOver != SystemButton.Maximize)
                {
                    m_MouseOver = SystemButton.Maximize;
                    if (this.ToolTipVisible)
                    {
                        this.HideToolTip();
                        this.ResetHover();
                    }
                    this.Refresh();
                }
                return;
            }


			if(btn==SystemButton.Close)
			{
				// Close
				if(m_MouseOver!=SystemButton.Close)
				{
					m_MouseOver=SystemButton.Close;
					if(this.ToolTipVisible)
					{
						this.HideToolTip();
						this.ResetHover();
					}
					this.Refresh();
				}
				return;
			}
            
			if(m_MouseOver!=SystemButton.None)
			{
				m_MouseOver=SystemButton.None;
				if(this.ToolTipVisible)
				{
					this.HideToolTip();
					this.ResetHover();
				}
				this.Refresh();
			}
		}

		public override void RecalcSize()
		{
			if(this.SuspendLayout)
				return;

            if (m_IsSystemIcon)
            {
                m_Rect.Size = new Size(16, 16);
            }
            else
            {
                if (this.Orientation == eOrientation.Horizontal)
                    m_Rect.Size = new Size(GetButtonSize().Width * 3 + 2, GetButtonSize().Height);
                else
                    m_Rect.Size = new Size(GetButtonSize().Width, GetButtonSize().Height * 3 + 2);
            }
			base.RecalcSize();
		}

        internal void SetIcon(Icon icon)
        {
            if (m_Icon != null) m_Icon.Dispose();
            if (icon != null)
                m_Icon = new Icon(icon, 16, 16);
            else
                m_Icon = null;
        }

		public System.Drawing.Icon Icon
		{
			get
			{
				return m_Icon;
			}
			set
			{
				if(value!=null)
					m_Icon=new Icon(value,16,16);
				else
                    m_Icon=null;
				this.Refresh();
			}
		}

        /// <summary>
        /// Returns the single button size.
        /// </summary>
        /// <returns>Size of the button.</returns>
        internal virtual Size GetButtonSize()
        {
            return System.Windows.Forms.SystemInformation.MenuButtonSize;
        }

		internal virtual SystemButton GetButton(int x, int y)
		{
            Rectangle r = new Rectangle(this.DisplayRectangle.Location, GetButtonSize());
			r.Inflate(-1,-2);
			r.Location=this.DisplayRectangle.Location;
			
			if(this.Orientation==eOrientation.Horizontal)
				r.Offset(0,(this.DisplayRectangle.Height-r.Height)/2);
			else
				r.Offset((this.WidthInternal-r.Width)/2,0);

			if(r.Contains(x,y))
				return SystemButton.Minimize;

			if(this.Orientation==eOrientation.Horizontal)
				r.Offset(r.Width+1,0);
			else
				r.Offset(0,r.Height+1);

            if (r.Contains(x, y))
            {
                if(m_RestoreEnabled)
                    return SystemButton.Restore;
                return SystemButton.Maximize;
            }

			if(this.Orientation==eOrientation.Horizontal)
				r.Offset(r.Width+3,0);
			else
				r.Offset(0,r.Height+3);

			if(r.Contains(x,y))
				return SystemButton.Close;

			return SystemButton.None;
		}

		internal SystemButton LastButtonClick
		{
			get
			{
				return m_LastButtonClick;
			}
		}

		public bool MinimizeEnabled
		{
			get
			{
				return m_MinimizeEnabled;
			}
			set
			{
				if(value!=m_MinimizeEnabled)
				{
					m_MinimizeEnabled=value;
					this.Refresh();
				}
			}
		}

		public bool RestoreEnabled
		{
			get
			{
				return m_RestoreEnabled;
			}
			set
			{
				if(value!=m_RestoreEnabled)
				{
					m_RestoreEnabled=value;
					this.Refresh();
				}
			}
		}

		public bool CloseEnabled
		{
			get
			{
				return m_CloseEnabled;
			}
			set
			{
				if(value!=m_CloseEnabled)
				{
					m_CloseEnabled=value;
					this.Refresh();
				}
			}
		}

		private Bitmap GetSysImage(SystemButton btn)
		{
			if(btn==SystemButton.None)
				return null;
            Rectangle rBtn = new Rectangle(0, 0, GetButtonSize().Width - 2, GetButtonSize().Height - 2);
			Bitmap bmp=new Bitmap(rBtn.Width,rBtn.Height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			//bmp.GetHbitmap();

			Graphics gBmp=Graphics.FromImage(bmp);
			rBtn.Inflate(0,-1);
			Rectangle rClip=rBtn;
			rClip.Inflate(-1,-1);
			using(SolidBrush brush=new SolidBrush(SystemColors.Control))
				gBmp.FillRectangle(brush,0,0,bmp.Width,bmp.Height);
			//gBmp.Clear(SystemColors.Control);
			gBmp.SetClip(rClip);
			System.Windows.Forms.ControlPaint.DrawCaptionButton(gBmp,rBtn,(System.Windows.Forms.CaptionButton)btn,System.Windows.Forms.ButtonState.Flat);
			gBmp.ResetClip();
			gBmp.Dispose();

			bmp.MakeTransparent(SystemColors.Control);

			return bmp;
		}

		private void CreateSystemMenu()
		{
			using(LocalizationManager lm=new LocalizationManager(this.GetOwner() as IOwnerLocalize))
			{
				this.SubItems.Clear();
				this.PopupType=ePopupType.Menu;
				ButtonItem btn=new ButtonItem("dotnetbarsysmenurestore");
                btn.GlobalItem = false;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuRestore); //"Restore";
				btn.Image=GetSysImage(SystemButton.Restore);
				btn.Click+=new System.EventHandler(this.MenuClick);
				btn.Enabled=m_RestoreEnabled;
				this.SubItems.Add(btn);

				btn=new ButtonItem();
                btn.GlobalItem = false;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMove); //"Move";
				btn.Enabled=false;
				this.SubItems.Add(btn);

				btn=new ButtonItem();
                btn.GlobalItem = false;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuSize); //"Size";
				btn.Enabled=false;
				this.SubItems.Add(btn);

				btn=new ButtonItem("dotnetbarsysmenuminimize");
                btn.GlobalItem = false;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMinimize); //"Minimize";
				btn.Image=GetSysImage(SystemButton.Minimize);
				btn.Click+=new System.EventHandler(this.MenuClick);
				btn.Enabled=m_MinimizeEnabled;
				this.SubItems.Add(btn);

				btn=new ButtonItem();
                btn.GlobalItem = false;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMaximize); //"Maximize";
				btn.Image=GetSysImage(SystemButton.Maximize);
				btn.Enabled=false;
				this.SubItems.Add(btn);

				btn=new ButtonItem("dotnetbarsysmenuclose");
                btn.GlobalItem = false;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuClose); //"Close";
				btn.Image=GetSysImage(SystemButton.Close);
				btn.BeginGroup=true;
				btn.Click+=new System.EventHandler(this.MenuClick);
				btn.Shortcuts.Add(eShortcut.CtrlF4);
				btn.Enabled=m_CloseEnabled;
				this.SubItems.Add(btn);

				btn=new ButtonItem("dotnetbarsysmenunext");
                btn.GlobalItem = false;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuNext); //"Next";
				btn.BeginGroup=true;
				btn.Shortcuts.Add(eShortcut.CtrlF6);
				btn.Click+=new System.EventHandler(this.MenuClick);
				this.SubItems.Add(btn);
			}
		}

		private void MenuClick(object sender, System.EventArgs e)
		{
			// System items need to be collapsed by us on Click see RaiseClick method on BaseItem
			BaseItem.CollapseAll(this);
			BaseItem objItem=sender as BaseItem;
			if(objItem==null)
				return;
			m_LastButtonClick=SystemButton.None;
			if(objItem.Name=="dotnetbarsysmenurestore")
				m_LastButtonClick=SystemButton.Restore;
			else if(objItem.Name=="dotnetbarsysmenuminimize")
				m_LastButtonClick=SystemButton.Minimize;
			else if(objItem.Name=="dotnetbarsysmenuclose")
				m_LastButtonClick=SystemButton.Close;
			else if(objItem.Name=="dotnetbarsysmenunext")
				m_LastButtonClick=SystemButton.NextWindow;

			if(m_LastButtonClick!=SystemButton.None)
				this.RaiseClick();
		}

        internal SystemButton MouseOverButton
        {
            get { return m_MouseOver; }
        }

        internal SystemButton MouseDownButton
        {
            get { return m_MouseDown; }
        }
	}
}
