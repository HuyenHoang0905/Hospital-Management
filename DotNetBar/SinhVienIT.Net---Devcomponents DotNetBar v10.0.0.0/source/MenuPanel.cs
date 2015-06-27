using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;
using System.Drawing.Text;
using System.Collections.Generic;

namespace DevComponents.DotNetBar
{
    /// <summary>
    ///    Summary description for PopupWindow.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
    public class MenuPanel : Control, IDesignTimeProvider, IAccessibilitySupport, IKeyTipsControl, IKeyTipsRenderer
    {
		#region Private Variables
		private struct ExpandButtonData
		{
			public ExpandButtonData(bool personalizedallvisible,bool showexpandbutton,Rectangle rExpandButton,bool mouseover)
			{
				this.PersonalizedAllVisible=personalizedallvisible;
				this.ShowExpandButton=showexpandbutton;
				this.Rect=rExpandButton;
				this.MouseOver=mouseover;
			}
			public bool PersonalizedAllVisible;
			public bool ShowExpandButton;
			public Rectangle Rect;
			public bool MouseOver;
		}

		const int WM_MOUSEACTIVATE = 0x21;
		const int MA_NOACTIVATE = 3;
		const int MA_NOACTIVATEANDEAT = 4;
		private const int DEFAULT_SIDE_WIDTH=16;
		private const int GROUP_SPACINGDOTNET=3;
		private const int GROUP_SPACINGOFFICE=9;
		
		protected BaseItem m_ParentItem;
		protected BaseItem m_HotSubItem;
		protected Point m_ParentItemScreenPos;
		private bool m_Hover;
		private bool m_Scroll;
		private bool m_TopScroll;
		private bool m_BottomScroll;
		private int m_ScrollTopPosition;
		private int m_TopScrollHeight, m_BottomScrollHeight;
		private Rectangle m_ClientRect;
		private SideBarImage m_SideBarImage;
		private Rectangle m_SideRect;

		private object m_Owner;

		private bool m_IsCustomizeMenu; // This setting is used internaly for menu that is shown as result of clicking on Add or Remove Button to customize the Bar
		private System.Drawing.Point m_MouseDownPt;

		private ePersonalizedMenus m_PersonalizedMenus=ePersonalizedMenus.Disabled;
		const int DEFAULT_EXPAND_BUTTON_HEIGHT=14;
		const int OFFICE2003_EXPAND_BUTTON_HEIGHT=18;
		private int EXPAND_BUTTON_HEIGHT=DEFAULT_EXPAND_BUTTON_HEIGHT;

		private ExpandButtonData m_ExpandButton=new ExpandButtonData(false,false,Rectangle.Empty,false);

		private System.Windows.Forms.Timer m_ScrollTimer=null;
		private ePopupAnimation m_PopupAnimation=ePopupAnimation.ManagerControlled;

		private PopupShadow m_DropShadow=null;
		private ColorScheme m_ColorScheme=null;

		// Used to eat the mouse move messages when user is using the keyboard to browse through the
		// top level menus. The problem was that on each repaint the mouse move was fired even though the
		// mouse did not move at all. So if mouse was over an menu item it was not possible to switch to the
		// new menu item becouse mouse was "holding" the focus.
		private bool m_IgnoreDuplicateMouseMove=false;
		private System.Windows.Forms.MouseEventArgs m_LastMouseMoveEvent=null;

		internal bool m_AccessibleObjectCreated=false;

		private bool m_PopupMenu=true;
		private bool m_ShowToolTips=true;

		private BaseItem m_DelayedCollapseItem=null;
		private bool m_LastExpandedOnHover=true; // was false
		private bool m_AntiAlias=false;
        private int m_CornerSize = 3;
        private BaseItem m_DoDefaultActionItem = null;
        internal bool UseWholeScreenForSizeChecking = false;
		#endregion

        public MenuPanel():base()
        {
			m_ParentItem=null;
			//m_OldContainer=null;
			m_HotSubItem=null;
			m_Hover=false;
			m_Scroll=false;
			m_TopScroll=false;
			m_BottomScroll=false;
			m_ScrollTopPosition=0;
			m_TopScrollHeight=0;
			m_BottomScrollHeight=0;
			m_ClientRect=new Rectangle();
			m_SideRect=new Rectangle();
			m_SideBarImage=new SideBarImage();
			m_IsCustomizeMenu=false;
			m_Owner=null;
			this.Font=System.Windows.Forms.SystemInformation.MenuFont;//.Clone() as Font;
			this.SetStyle(ControlStyles.Selectable,false);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.IsAccessible=true;
			this.AccessibleRole=AccessibleRole.MenuPopup;
        }
		protected override void Dispose(bool disposing)
        {
			if(this.Parent!=null && this.Parent is PopupContainer)
			{
				Control parent=this.Parent;
				this.Parent.Controls.Remove(this);
                parent.Dispose();
				parent=null;
			}
			if(m_AccessibleObjectCreated)
			{
				PopupMenuAccessibleObject acc=this.AccessibilityObject as PopupMenuAccessibleObject;
				if(acc!=null)
					acc.GenerateEvent(AccessibleEvents.Destroy);
			}

			if(m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
		    this.Font = null;
			RestoreContainer();
			m_ParentItem=null;
			base.Dispose(disposing);
        }
		
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_MOUSEACTIVATE)
			{
				m.Result=new System.IntPtr(MA_NOACTIVATE);
				return;
			}
            else if (m.Msg == NativeFunctions.WM_USER + 107)
            {
                if (m_DoDefaultActionItem != null)
                {
                    m_DoDefaultActionItem.DoAccesibleDefaultAction();
                    m_DoDefaultActionItem = null;
                }

            }
			base.WndProc(ref m);
        }

        private Rendering.BaseRenderer GetRenderer()
        {
            Rendering.BaseRenderer r = Rendering.GlobalManager.Renderer;

            if (m_ParentItem != null && m_ParentItem.GetContainerControl(false) is IRenderingSupport)
            {
                Rendering.BaseRenderer renderer = ((IRenderingSupport)m_ParentItem.GetContainerControl(false)).GetRenderer();
                if (renderer != null)
                    r = renderer;
            }

            return r;
        }

        #region KeyTips Support
        private bool m_ShowKeyTips = false;
        private string m_KeyTipsKeysStack = "";
        private Font m_KeyTipsFont = null;
        private KeyTipsCanvasControl m_KeyTipsCanvas = null;
        /// <summary>
        /// Gets or sets whether Key Tips (accelerator keys) for items are displayed on top of them.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool ShowKeyTips
        {
            get { return m_ShowKeyTips; }
            set
            {
                if (m_ShowKeyTips != value)
                {
                    m_ShowKeyTips = value;
                    this.OnShowKeyTipsChanged();
                }
            }
        }

        string IKeyTipsControl.KeyTipsKeysStack
        {
            get { return m_KeyTipsKeysStack; }
            set
            {
                m_KeyTipsKeysStack = value;
                if (m_KeyTipsCanvas != null)
                {
                    this.Refresh();
                    //m_KeyTipsCanvas.Update();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font that is used to display Key Tips (accelerator keys) when they are displayed. Default value is null which means
        /// that control Font is used for Key Tips display.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Appearance"), Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
        public virtual Font KeyTipsFont
        {
            get { return m_KeyTipsFont; }
            set { m_KeyTipsFont = value; }
        }

        protected virtual void OnShowKeyTipsChanged()
        {
            m_KeyTipsKeysStack = "";
            if (this.ShowKeyTips)
                CreateKeyTipCanvas();
            else
                DestroyKeyTipCanvas();
        }

        protected virtual void CreateKeyTipCanvas()
        {
            if (m_KeyTipsCanvas != null)
            {
                m_KeyTipsCanvas.BringToFront();
                return;
            }

            m_KeyTipsCanvas = new KeyTipsCanvasControl(this);
            m_KeyTipsCanvas.Bounds = new Rectangle(0, 0, this.Width, this.Height);
            m_KeyTipsCanvas.Visible = true;
            this.Controls.Add(m_KeyTipsCanvas);
            m_KeyTipsCanvas.BringToFront();
        }

        protected virtual void DestroyKeyTipCanvas()
        {
            if (m_KeyTipsCanvas == null)
                return;
            m_KeyTipsCanvas.Visible = false;
            this.Controls.Remove(m_KeyTipsCanvas);
            m_KeyTipsCanvas.Dispose();
            m_KeyTipsCanvas = null;
        }

        void IKeyTipsRenderer.PaintKeyTips(Graphics g)
        {
            this.PaintKeyTips(g);
        }

        protected virtual void PaintKeyTips(Graphics g)
        {
            if (!m_ShowKeyTips || m_ParentItem == null)
                return;

            KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, "", GetKeyTipFont(), null);

            Rendering.BaseRenderer renderer = GetRenderer();
            PaintContainerKeyTips(m_ParentItem, renderer, e);
        }

        protected virtual Font GetKeyTipFont()
        {
            Font font = this.Font;
            if (m_KeyTipsFont != null)
                font = m_KeyTipsFont;
            return font;
        }

        internal virtual void PaintContainerKeyTips(BaseItem container, Rendering.BaseRenderer renderer, KeyTipsRendererEventArgs e)
        {
            foreach (BaseItem item in container.SubItems)
            {
                if (!item.Visible || !item.Displayed)
                    continue;

                if (item.IsContainer)
                    PaintContainerKeyTips(item, renderer, e);

                if (item.AccessKey == Char.MinValue && item.KeyTips == "" || m_KeyTipsKeysStack != "" && !item.KeyTips.StartsWith(m_KeyTipsKeysStack)
                    || item.KeyTips == "" && m_KeyTipsKeysStack != "")
                    continue;

                if (item.KeyTips != "")
                    e.KeyTip = item.KeyTips;
                else
                    e.KeyTip = item.AccessKey.ToString().ToUpper();

                e.Bounds = GetKeyTipRectangle(e.Graphics, item, e.Font, e.KeyTip);
                e.ReferenceObject = item;

                renderer.DrawKeyTips(e);
            }
        }

        protected virtual Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
        {
            Size padding = KeyTipsPainter.KeyTipsPadding;
            Size size = TextDrawing.MeasureString(g, keyTip, font);
            size.Width += padding.Width;
            size.Height += padding.Height;

            Rectangle ib = item.DisplayRectangle;
            Rectangle r = new Rectangle(ib.X + 16, ib.Bottom - size.Height, size.Width, size.Height);

            return r;
        }

        public virtual bool ProcessMnemonicEx(char charCode)
        {
            if (ProcessContainerAccessKey(m_ParentItem, charCode))
                return true;
            return false;
        }
        #endregion

        /// <summary>
        /// Gets or sets the item default accessibility action will be performed on.
        /// </summary>
        BaseItem IAccessibilitySupport.DoDefaultActionItem
        {
            get { return m_DoDefaultActionItem; }
            set { m_DoDefaultActionItem = value; }
        }

		[DefaultValue(true), Browsable(true), DevCoBrowsable(true)]
		public bool PopupMenu
		{
			get {return m_PopupMenu;}
			set {m_PopupMenu=value;}
		}

        // Closes the popup menu.
        public void Close()
        {
            if (m_AccessibleObjectCreated)
            {
                MenuPanel.PopupMenuAccessibleObject acc = this.AccessibilityObject as PopupMenuAccessibleObject;
                if (acc != null)
                {
                    if (m_ParentItem != null && m_ParentItem.IsOnMenuBar)
                        acc.GenerateEvent(AccessibleEvents.SystemMenuEnd);
                    acc.GenerateEvent(AccessibleEvents.SystemMenuPopupEnd);
                }
            }
            SetFocusItem(null);
            Hide();
        }

        protected override AccessibleObject CreateAccessibilityInstance()
		{
			m_AccessibleObjectCreated=true;
			return new PopupMenuAccessibleObject(this);
		}

		private void SetupAccessibility()
		{
			if(m_ParentItem!=null && m_ParentItem.Text!="")
				this.AccessibleName=m_ParentItem.Text;
			else
				this.AccessibleName="DotNetBar Popup Menu";
			this.AccessibleDescription=""; //this.AccessibleName+" ("+this.Name+")";
			this.AccessibleRole=AccessibleRole.MenuPopup;
		}

        internal ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa;
            if (m_ColorScheme == null)
                pa = new ItemPaintArgs(m_Owner as IOwner, this, g, new ColorScheme(g));
            else
                pa = new ItemPaintArgs(m_Owner as IOwner, this, g, m_ColorScheme);

            pa.Renderer = GetRenderer();

            if (m_ParentItem.DesignMode)
            {
                ISite site = this.GetSite();
                if (site != null && site.DesignMode)
                    pa.DesignerSelection = true;
            }

            return pa;
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			if(m_ParentItem!=null)
			{
                ItemPaintArgs pa = GetItemPaintArgs(e.Graphics);
                pa.ClipRectangle = e.ClipRectangle;
                SmoothingMode sm = e.Graphics.SmoothingMode;
                TextRenderingHint th = e.Graphics.TextRenderingHint;

				if(m_AntiAlias)
				{
					e.Graphics.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				}

                if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
					PaintOffice(pa);
				else
					PaintDotNet(pa);

                if (m_ParentItem is Office2007StartButton)
                {
                    ((Office2007StartButton)m_ParentItem).OnMenuPaint(pa);
                }

                e.Graphics.SmoothingMode = sm;
                e.Graphics.TextRenderingHint = th;
			}
        }

        #region InternalMenuColors
        private class InternalMenuColors
        {
            public Rendering.LinearGradientColorTable Background = new Rendering.LinearGradientColorTable();
            public Rendering.LinearGradientColorTable Side = new Rendering.LinearGradientColorTable();
            public Rendering.LinearGradientColorTable SideUnused = new Rendering.LinearGradientColorTable();
            public Rendering.LinearGradientColorTable Border = new Rendering.LinearGradientColorTable();
            public Rendering.LinearGradientColorTable SideBorder = new Rendering.LinearGradientColorTable();
            public Rendering.LinearGradientColorTable SideBorderLight = new Rendering.LinearGradientColorTable();
        }
        
        private InternalMenuColors GetMenuColors(ItemPaintArgs pa)
        {
            InternalMenuColors colors = new InternalMenuColors();
            if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && pa.Renderer is Rendering.Office2007Renderer)
            {
                Rendering.Office2007MenuColorTable ct = ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.Menu;
                colors.Background = ct.Background;
                colors.Border = ct.Border;
                colors.Side = ct.Side;
                colors.SideBorder = ct.SideBorder;
                colors.SideBorderLight = ct.SideBorderLight;
                colors.SideUnused = ct.SideUnused;
            }
            else
            {
                colors.Background.Start = pa.Colors.MenuBackground;
                colors.Background.End = pa.Colors.MenuBackground2;
                colors.Background.GradientAngle = pa.Colors.MenuBackgroundGradientAngle;
                colors.Side.Start = pa.Colors.MenuSide;
                colors.Side.End = pa.Colors.MenuSide2;
                colors.Side.GradientAngle = pa.Colors.MenuSideGradientAngle;
                colors.SideUnused.Start = pa.Colors.MenuUnusedSide;
                colors.SideUnused.End = pa.Colors.MenuUnusedSide2;
                colors.SideUnused.GradientAngle = pa.Colors.MenuUnusedSideGradientAngle;
                colors.Border.Start = pa.Colors.MenuBorder;
                colors.SideBorder.Start = pa.Colors.ItemSeparator;
                colors.SideBorderLight.Start = pa.Colors.ItemSeparatorShade;
            }

            return colors;
        }
        #endregion

        private bool IsContainerMenu
        {
            get
            {
                if (m_ParentItem != null && m_ParentItem.SubItems.Count == 1 && m_ParentItem.SubItems[0] is ItemContainer)
                    return true;
                return false;
            }
        }

        protected void PaintDotNet(ItemPaintArgs pa)
		{
			Graphics g=pa.Graphics;
			if(this.DisplayShadow && !this.AlphaShadow)
				SetupRegion();
            InternalMenuColors menuColors = this.GetMenuColors(pa);

            DisplayHelp.FillRectangle(g, this.ClientRectangle, menuColors.Background);
			
			PaintSideBar(pa);

            //Pen p=new Pen(pa.Colors.MenuBorder,1);
            if (m_ParentItem != null)
            {
                Rectangle borderRect = this.ClientRectangle;
                if (this.DisplayShadow && !this.AlphaShadow)
                    borderRect = new Rectangle(0, 0, this.ClientSize.Width - 2, this.ClientSize.Height - 2);
                if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                {
                    if (this.Region != null)
                    {
                        DisplayHelp.DrawRoundedRectangle(g, menuColors.Border.Start, borderRect, m_CornerSize);
                        Rectangle borderInset = borderRect;
                        borderInset.Inflate(-1, -1);
                        Color c = ControlPaint.LightLight(menuColors.Background.Start);
                        if (IsContainerMenu)
                        {
                            c = Color.Empty;
                            GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(borderInset, m_CornerSize);
                            Region reg = new Region(path);
                            path.Widen(SystemPens.Control);
                            reg.Union(path);
                            path.Dispose();
                            path = null;
                            g.Clip = reg;
                        }

                        if (!c.IsEmpty)
                        {
                            using (Pen pen = new Pen(c))
                                DisplayHelp.DrawRoundedRectangle(g, pen, borderInset, m_CornerSize);
                        }
                    }
                    else
                    {
                        DisplayHelp.DrawRectangle(g, menuColors.Border.Start, borderRect);
                    }
                }
                else
                    DisplayHelp.DrawGradientRectangle(g, borderRect, menuColors.Border, 1);

                // Shadow
                if (this.DisplayShadow && !this.AlphaShadow && m_ParentItem != null && !BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                {
                    using (Pen p = new Pen(SystemColors.ControlDark, 2))
                    {
                        Point[] pt = new Point[3];
                        pt[0].X = 2;
                        pt[0].Y = this.ClientSize.Height - 1;
                        pt[1].X = this.ClientSize.Width - 1;
                        pt[1].Y = this.ClientSize.Height - 1;
                        pt[2].X = this.ClientSize.Width - 1;
                        pt[2].Y = 2;
                        g.DrawLines(p, pt);
                    }
                }

                if ((!BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) || m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2010) && m_ParentItem is ButtonItem && m_ParentItem.Displayed && m_ParentItem.Visible && m_ParentItem.Orientation == eOrientation.Horizontal && !(m_ParentItem.ContainerControl is ContextMenuBar))
                {
                    // Determine where to draw the line based on parent position
                    if (m_PopupMenu && this.Parent != null && m_ParentItemScreenPos.Y < this.Parent.Location.Y)
                    {
                        Point p1 = new Point((m_ParentItemScreenPos.X - this.Parent.Location.X) + 1, 0);
                        Point p2 = new Point(p1.X + m_ParentItem.WidthInternal - (pa.Colors.ItemExpandedShadow.IsEmpty ? 3 : 5), 0);
                        using (Pen pen = new Pen(pa.Colors.ItemExpandedBackground, 1))
                            g.DrawLine(pen, p1, p2);
                    }
                }
                // If menu scrolls paint side bars
                if (m_Scroll)
                {
                    BaseItem objItem = m_ParentItem.SubItems[0];
                    if (m_TopScroll)
                    {
                        ImageItem objImageItem = m_ParentItem as ImageItem;
                        if (objImageItem != null)
                        {
                            Rectangle sideRect = new Rectangle(m_ClientRect.Left, m_ClientRect.Top, objImageItem.SubItemsImageSize.Width + 7, m_TopScrollHeight);
                            DisplayHelp.FillRectangle(g, sideRect, menuColors.Side);
                            if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                                PaintMenuItemSide(pa, sideRect, menuColors);
                        }
                        else
                        {
                            Rectangle sideRect = new Rectangle(m_ClientRect.Left, m_ClientRect.Top, DEFAULT_SIDE_WIDTH + 7, m_TopScrollHeight);
                            DisplayHelp.FillRectangle(g, sideRect, menuColors.Side);
                            if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                                PaintMenuItemSide(pa, sideRect, menuColors);
                        }
                    }
                    if (m_BottomScroll)
                    {
                        ImageItem objImageItem = m_ParentItem as ImageItem;
                        if (objImageItem != null)
                        {
                            Rectangle sideRect = new Rectangle(m_ClientRect.Left, m_ClientRect.Bottom - m_BottomScrollHeight - 1, objImageItem.SubItemsImageSize.Width + 7, m_BottomScrollHeight);
                            DisplayHelp.FillRectangle(g, sideRect, menuColors.Side);
                            if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                                PaintMenuItemSide(pa, sideRect, menuColors);
                        }
                        else
                        {
                            Rectangle sideRect = new Rectangle(m_ClientRect.Left, m_ClientRect.Bottom - m_BottomScrollHeight - 1, DEFAULT_SIDE_WIDTH + 7, m_BottomScrollHeight);
                            DisplayHelp.FillRectangle(g, sideRect, menuColors.Side);
                            if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                                PaintMenuItemSide(pa, sideRect, menuColors);
                        }
                    }
                    objItem = null;
                }
                PaintItems(pa, menuColors);
            }
            else
                DisplayHelp.DrawGradientRectangle(g, this.ClientRectangle, menuColors.Border, 1);
		}

		protected void PaintOffice(ItemPaintArgs pa)
		{
			using(SolidBrush brush=new SolidBrush(SystemColors.Control))
				pa.Graphics.FillRectangle(brush,this.DisplayRectangle);
			//pa.Graphics.Clear(SystemColors.Control);
			
			PaintSideBar(pa);

			System.Windows.Forms.ControlPaint.DrawBorder3D(pa.Graphics,0,0,this.ClientSize.Width,this.ClientSize.Height,System.Windows.Forms.Border3DStyle.Raised);
			if(m_ParentItem!=null)
				PaintItems(pa, GetMenuColors(pa));
		}

        private bool DoPaintSide(BaseItem item)
        {
            if (item is LabelItem || item is ItemContainer)
                return false;
            return true;
        }

        private void PaintItems(ItemPaintArgs pa, InternalMenuColors menuColors)
		{
			Graphics g=pa.Graphics;
            bool rightToLeft = (this.RightToLeft == RightToLeft.Yes);

			// Draw all contained items
			if(m_ParentItem!=null && m_ParentItem!=null)
			{
				// Special case handling when menus exceed the size of window
				Point[] p;
				Rectangle clipRect=new Rectangle((int)g.ClipBounds.X,(int)g.ClipBounds.Y,(int)g.ClipBounds.Width,(int)g.ClipBounds.Height);
				if(m_Scroll)
				{
					if(m_TopScroll)
					{
						p=new Point[3];
						p[0].X=m_ClientRect.Left+(m_ClientRect.Width-8)/2;
						p[0].Y=m_ClientRect.Top+8;
						p[1].X=p[0].X+8;
						p[1].Y=p[0].Y;
						p[2].X=p[0].X+4;
						p[2].Y=p[0].Y-5;
						g.FillPolygon(SystemBrushes.ControlText,p);
					}
					BaseItem objTmp;
					ImageItem objParentImageItem=m_ParentItem as ImageItem;
					int iSideWidth=DEFAULT_SIDE_WIDTH;
					if(objParentImageItem!=null)
						iSideWidth=objParentImageItem.SubItemsImageSize.Width;
					objParentImageItem=null;
                    bool previousContainerItem = false;
					for(int i=m_ScrollTopPosition;i<m_ParentItem.SubItems.Count;i++)
					{
						objTmp=m_ParentItem.SubItems[i];
						if(!clipRect.IntersectsWith(objTmp.DisplayRectangle))
							continue;
						if(objTmp.Visible || m_IsCustomizeMenu)
						{
							if(objTmp.Displayed)
							{
                                SmoothingMode sm = g.SmoothingMode;
                                g.SmoothingMode = SmoothingMode.Default;
								if(objTmp.BeginGroup && objTmp.TopInternal>ClientMarginTop)  // Exclude first element
								{
                                    if (objTmp.EffectiveStyle == eDotNetBarStyle.Office2000)
									{
										if(m_IsCustomizeMenu)
											System.Windows.Forms.ControlPaint.DrawBorder3D(g,objTmp.HeightInternal+2+objTmp.LeftInternal+13,objTmp.TopInternal-5,objTmp.WidthInternal-26-objTmp.HeightInternal-2,2,System.Windows.Forms.Border3DStyle.Etched,System.Windows.Forms.Border3DSide.Top);
										else
											System.Windows.Forms.ControlPaint.DrawBorder3D(g,objTmp.LeftInternal+13,objTmp.TopInternal-5,objTmp.WidthInternal-26,2,System.Windows.Forms.Border3DStyle.Etched,System.Windows.Forms.Border3DSide.Top);
									}
									else
									{
										if(m_IsCustomizeMenu)
										{
                                            Rectangle sideRect = new Rectangle(objTmp.LeftInternal, objTmp.TopInternal - 3, objTmp.HeightInternal + 2 + iSideWidth + 7, 3);
                                            sideRect.Inflate(0, 1);
                                            DisplayHelp.FillRectangle(g, sideRect, menuColors.Side.Start, menuColors.Side.End, menuColors.Side.GradientAngle);

                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                                PaintMenuItemSide(pa, sideRect, menuColors);
                                            DisplayHelp.DrawLine(g, objTmp.LeftInternal + iSideWidth + 8 + 7 + objTmp.HeightInternal + 2, objTmp.TopInternal - 2, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 2,
                                                menuColors.SideBorder.Start, 1);
                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
											    DisplayHelp.DrawLine(g, objTmp.LeftInternal + iSideWidth + 8 + 7 + objTmp.HeightInternal + 2, objTmp.TopInternal - 1, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 1, menuColors.SideBorderLight.Start, 1);

										}
										else
										{
                                            bool paintSide = DoPaintSide(objTmp);
                                            bool itemOnlySeparator = paintSide;
                                            if(paintSide)
                                            {
                                                Rectangle sideRect = new Rectangle(objTmp.LeftInternal, objTmp.TopInternal - 3, iSideWidth + 7, 3);
                                                sideRect.Inflate(0, 1);
											    // Paint side bar
                                                if (objTmp is IPersonalizedMenuItem && ((IPersonalizedMenuItem)objTmp).MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !((IPersonalizedMenuItem)objTmp).RecentlyUsed)
                                                    DisplayHelp.FillRectangle(g, sideRect, menuColors.SideUnused);
                                                else
                                                    DisplayHelp.FillRectangle(g, sideRect, menuColors.Side);

                                                if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                                    PaintMenuItemSide(pa, sideRect, menuColors);
                                            }
                                            if (itemOnlySeparator && previousContainerItem)
                                                itemOnlySeparator = false;
                                            DisplayHelp.DrawLine(g, objTmp.LeftInternal + (itemOnlySeparator ? (iSideWidth + 8 + 7) : 0), objTmp.TopInternal - 2, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 2, menuColors.SideBorder.Start, 1);
                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                                DisplayHelp.DrawLine(g, objTmp.LeftInternal + (itemOnlySeparator ? (iSideWidth + 8 + 7) : 0), objTmp.TopInternal - 1, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
										}
									}
								}
                                g.SmoothingMode = sm;
								objTmp.Paint(pa);
                                previousContainerItem = (objTmp.IsContainer);
							}
						}
					}
					if(m_BottomScroll)
					{
						// Draw down button
						p=new Point[3];
						p[0].X=m_ClientRect.Left+(m_ClientRect.Width-8)/2;
						p[0].Y=m_ClientRect.Bottom-8;
						p[1].X=p[0].X+8;
						p[1].Y=p[0].Y;
						p[2].X=p[0].X+4;
						p[2].Y=p[0].Y+4;
						g.FillPolygon(SystemBrushes.ControlText,p);
					}
				}
				else
				{
					// This is very simple...
					ImageItem objParentImageItem=m_ParentItem as ImageItem;
					int iSideBarWidth=DEFAULT_SIDE_WIDTH;
					if(objParentImageItem!=null)
						iSideBarWidth=objParentImageItem.SubItemsImageSize.Width;
					objParentImageItem=null;
					bool bFirst=true;
                    bool previousContainerItem = false;
					foreach(BaseItem objTmp in m_ParentItem.SubItems)
					{
						if(!clipRect.IntersectsWith(objTmp.DisplayRectangle))
							continue;

						if(objTmp.Visible && objTmp.Displayed || m_IsCustomizeMenu)
						{
                            SmoothingMode sm = g.SmoothingMode;
                            g.SmoothingMode = SmoothingMode.Default;
							if(objTmp.BeginGroup && !bFirst)
							{
                                if (objTmp.EffectiveStyle == eDotNetBarStyle.Office2000)
								{
									if(m_IsCustomizeMenu)
										System.Windows.Forms.ControlPaint.DrawBorder3D(g,objTmp.HeightInternal+2+objTmp.LeftInternal+13,objTmp.TopInternal-5,objTmp.WidthInternal-26-objTmp.HeightInternal-2,2,System.Windows.Forms.Border3DStyle.Etched,System.Windows.Forms.Border3DSide.Top);
									else
										System.Windows.Forms.ControlPaint.DrawBorder3D(g,objTmp.LeftInternal+13,objTmp.TopInternal-5,objTmp.WidthInternal-26,2,System.Windows.Forms.Border3DStyle.Etched,System.Windows.Forms.Border3DSide.Top);
								}
								else
								{
									if(m_IsCustomizeMenu)
									{
                                        Rectangle sideBar = new Rectangle(objTmp.LeftInternal, objTmp.TopInternal - 3, objTmp.HeightInternal + 2 + iSideBarWidth + 7, 3);
                                        sideBar.Inflate(0, 1);
                                        if (rightToLeft)
                                        {
                                            sideBar.Width = iSideBarWidth + 7;
                                            sideBar.X = objTmp.DisplayRectangle.Right - sideBar.Width;
                                        }
										// Paint side bar
                                        DisplayHelp.FillRectangle(g, sideBar, menuColors.Side);

                                        if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                            PaintMenuItemSide(pa, sideBar, menuColors);

                                        if (rightToLeft)
                                        {
                                            DisplayHelp.DrawLine(g, objTmp.LeftInternal + 1, objTmp.TopInternal - 2, objTmp.DisplayRectangle.Right - (iSideBarWidth + 8 + 7 + objTmp.HeightInternal + 2 + 1), objTmp.TopInternal - 2, menuColors.SideBorder.Start, 1);
                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                                DisplayHelp.DrawLine(g, objTmp.LeftInternal + 1, objTmp.TopInternal - 1, objTmp.DisplayRectangle.Right - (iSideBarWidth + 8 + 7 + objTmp.HeightInternal + 2 + 1), objTmp.TopInternal - 1, menuColors.SideBorderLight.Start,1);
                                        }
                                        else
                                        {
                                            DisplayHelp.DrawLine(g, objTmp.LeftInternal + iSideBarWidth + 8 + 7 + objTmp.HeightInternal + 2, objTmp.TopInternal - 2, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 2, menuColors.SideBorder.Start, 1);
                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                                DisplayHelp.DrawLine(g, objTmp.LeftInternal + iSideBarWidth + 8 + 7 + objTmp.HeightInternal + 2, objTmp.TopInternal - 1, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
                                        }
									}
									else
									{
                                        Rectangle sideBar = new Rectangle(objTmp.LeftInternal, objTmp.TopInternal - 2, iSideBarWidth + 7, 3);
                                        sideBar.Inflate(0, 1);
                                        if (rightToLeft)
                                            sideBar.X = objTmp.DisplayRectangle.Right - sideBar.Width - 1;
                                        
                                        bool paintSide = DoPaintSide(objTmp);
                                        bool itemOnlySeparator = paintSide;
                                        if (itemOnlySeparator && previousContainerItem)
                                        {
                                            paintSide = false;
                                            itemOnlySeparator = false;
                                        }
                                        if (paintSide)
                                        {
                                            // Paint side bar
                                            if (objTmp is IPersonalizedMenuItem && ((IPersonalizedMenuItem)objTmp).MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !((IPersonalizedMenuItem)objTmp).RecentlyUsed)
                                            {
                                                DisplayHelp.FillRectangle(g, sideBar, menuColors.SideUnused);
                                            }
                                            else
                                            {
                                                DisplayHelp.FillRectangle(g, sideBar, menuColors.Side);
                                            }

                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                                PaintMenuItemSide(pa, sideBar, menuColors);
                                        }
                                        
                                        if (rightToLeft)
                                        {
                                            DisplayHelp.DrawLine(g, objTmp.LeftInternal + 1, objTmp.TopInternal - 2, objTmp.DisplayRectangle.Right - (itemOnlySeparator ? (iSideBarWidth + 8 + 7 + 1) : 0), objTmp.TopInternal - 2, menuColors.SideBorder.Start, 1);
                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle))
                                                DisplayHelp.DrawLine(g, objTmp.LeftInternal + 1, objTmp.TopInternal - 1, objTmp.DisplayRectangle.Right - (itemOnlySeparator ? (iSideBarWidth + 8 + 7 + 1) : 0), objTmp.TopInternal - 1, menuColors.SideBorderLight.Start, 1);
                                        }
                                        else
                                        {
                                            DisplayHelp.DrawLine(g, objTmp.LeftInternal + (itemOnlySeparator ? (iSideBarWidth + 8 + 7) : 0), objTmp.TopInternal - 2, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 2, pa.Colors.ItemSeparator, 1);
                                            if (BarFunctions.IsOffice2007Style(objTmp.EffectiveStyle) && !pa.Colors.ItemSeparatorShade.IsEmpty)
                                                DisplayHelp.DrawLine(g, objTmp.LeftInternal + (itemOnlySeparator ? (iSideBarWidth + 8 + 7) : 0), objTmp.TopInternal - 1, objTmp.DisplayRectangle.Right - 1, objTmp.TopInternal - 1, pa.Colors.ItemSeparatorShade, 1);
                                        }
									}
								}
							}
                            g.SmoothingMode = sm;
							bFirst=false;
							objTmp.Paint(pa);
							objTmp.Displayed=true;
                            previousContainerItem = objTmp.IsContainer;
						}
					}
				}
                PaintExpandButton(pa, menuColors);
			}
		}

        private void PaintMenuItemSide(ItemPaintArgs pa, Rectangle sideRect, InternalMenuColors menuColors)
        {
            if (pa.Owner is DotNetBarManager) // Do not paint side lines for DotNetBarManager owned items...
                return;
            Graphics g = pa.Graphics;
            if (pa.RightToLeft)
            {
                Point p = new Point(sideRect.X, sideRect.Y);
                DisplayHelp.DrawLine(g, p.X, p.Y, p.X, p.Y + sideRect.Height, menuColors.SideBorderLight.Start, 1);
                
                p.X++;

                DisplayHelp.DrawLine(g, p.X, p.Y, p.X, p.Y + sideRect.Height, menuColors.SideBorder.Start, 1);
            }
            else
            {
                Point p = new Point(sideRect.Right - 2, sideRect.Y);

                DisplayHelp.DrawLine(g, p.X, p.Y, p.X, p.Y + sideRect.Height, menuColors.SideBorder.Start, 1);
                
                p.X++;

                DisplayHelp.DrawLine(g, p.X, p.Y, p.X, p.Y + sideRect.Height, menuColors.SideBorderLight.Start, 1);
            }
        }

		private void RefreshExpandButton()
		{
			Graphics g=this.CreateGraphics();
            try
            {
                ItemPaintArgs pa;
                if (m_ColorScheme == null)
                    pa = new ItemPaintArgs(m_Owner as IOwner, this, g, new ColorScheme(g));
                else
                    pa = new ItemPaintArgs(m_Owner as IOwner, this, g, m_ColorScheme);

                PaintExpandButton(pa, GetMenuColors(pa));
            }
            finally
            {
                g.Dispose();
            }
		}

        private bool IsGradientStyle
        {
            get
            {
                if (m_ParentItem != null)
                {
                    return m_ParentItem.EffectiveStyle == eDotNetBarStyle.OfficeXP || m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle);
                }
                return true;
            }
        }

		private void PaintExpandButton(ItemPaintArgs pa, InternalMenuColors menuColors)
		{
			if(!m_ExpandButton.ShowExpandButton || m_ParentItem==null)
				return;

			Graphics g=pa.Graphics;

			ImageItem objParentImageItem=m_ParentItem as ImageItem;
			int iSideWidth=DEFAULT_SIDE_WIDTH;
			if(objParentImageItem!=null && objParentImageItem.SubItemsImageSize.Width>iSideWidth)
				iSideWidth=objParentImageItem.SubItemsImageSize.Width;
			iSideWidth+=7;

			objParentImageItem=null;

            if (IsGradientStyle)
			{
				if(m_ExpandButton.MouseOver)
				{
                    Rectangle r = m_ExpandButton.Rect;
                    DisplayHelp.FillRectangle(g, r, pa.Colors.MenuBackground, Color.Empty);
					r.Inflate(-1,0);
					if(Control.MouseButtons==MouseButtons.Left)
					{
                        DisplayHelp.FillRectangle(g, r, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
					}
					else
					{
                        DisplayHelp.FillRectangle(g, r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
					}
					NativeFunctions.DrawRectangle(g,SystemPens.Highlight,r);
				}
				else
				{
                    Rectangle r=new Rectangle(m_ExpandButton.Rect.Left+iSideWidth,m_ExpandButton.Rect.Top,m_ExpandButton.Rect.Width-iSideWidth,m_ExpandButton.Rect.Height);
                    DisplayHelp.FillRectangle(g, r, pa.Colors.MenuBackground, Color.Empty);

                    Rectangle sideRect = new Rectangle(m_ExpandButton.Rect.Left, m_ExpandButton.Rect.Top, iSideWidth, m_ExpandButton.Rect.Height);
                    DisplayHelp.FillRectangle(g, sideRect, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
                    if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                        PaintMenuItemSide(pa, sideRect, menuColors);
				}

                if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				{
					System.Drawing.Drawing2D.GraphicsPath path=new System.Drawing.Drawing2D.GraphicsPath();
					Point pTopLeft=new Point(m_ExpandButton.Rect.X+(m_ExpandButton.Rect.Width-16)/2,m_ExpandButton.Rect.Y+(m_ExpandButton.Rect.Height-16)/2);
					path.AddEllipse(pTopLeft.X,pTopLeft.Y,16,16);
					System.Drawing.Drawing2D.PathGradientBrush gb=new System.Drawing.Drawing2D.PathGradientBrush(path);
					gb.CenterColor=pa.Colors.MenuSide;
					gb.SurroundColors=new Color[]{pa.Colors.MenuSide2};
					gb.CenterPoint=new Point(pTopLeft.X+3,pTopLeft.Y+3);
					System.Drawing.Drawing2D.SmoothingMode sm=g.SmoothingMode;
					g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
					g.FillEllipse(gb,pTopLeft.X,pTopLeft.Y,16,16);
					g.SmoothingMode=sm;
				}

			}
            else if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				if(m_ExpandButton.MouseOver)
				{	
					//g.FillRectangle(new SolidBrush(g.GetNearestColor(ControlPaint.Light(SystemColors.Control))),m_ExpandButton.Rect);
					g.FillRectangle(new SolidBrush(ColorFunctions.RecentlyUsedOfficeBackColor()),m_ExpandButton.Rect);
					if(Control.MouseButtons==MouseButtons.Left)
						ControlPaint.DrawBorder(g,m_ExpandButton.Rect,SystemColors.Control,ButtonBorderStyle.Inset);
					else
						ControlPaint.DrawBorder3D(g,m_ExpandButton.Rect,Border3DStyle.RaisedInner,(Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom));
					//NativeFunctions.DrawRectangle(g,SystemPens.Highlight,m_ExpandButton.Rect);
				}
				else
				{
					g.FillRectangle(SystemBrushes.Control,m_ExpandButton.Rect);
				}
			}

			Point[] p=new Point[3];
			p[0].X=m_ExpandButton.Rect.Left+(m_ExpandButton.Rect.Width-4)/2;
            if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				p[0].Y=m_ExpandButton.Rect.Top+(m_ExpandButton.Rect.Height-7)/2;
			else
				p[0].Y=m_ExpandButton.Rect.Top+3;
			p[1].X=p[0].X+2;
			p[1].Y=p[0].Y+2;
			p[2].X=p[0].X+4;
			p[2].Y=p[0].Y;
			g.DrawLines(SystemPens.ControlText,p);
			p[0].Y+=1;
			p[1].Y+=1;
			p[2].Y+=1;
			g.DrawLines(SystemPens.ControlText,p);

			p[0].Y+=3;
			p[1].Y+=3;
			p[2].Y+=3;
			g.DrawLines(SystemPens.ControlText,p);
			p[0].Y+=1;
			p[1].Y+=1;
			p[2].Y+=1;
			g.DrawLines(SystemPens.ControlText,p);

		}

		private void PaintSideBar(ItemPaintArgs pa)
		{
			Graphics g=pa.Graphics;
			if(m_SideBarImage.Picture==null)
				return;
			
			if(!m_SideBarImage.GradientColor1.IsEmpty && !m_SideBarImage.GradientColor2.IsEmpty)
			{
				PointF[] p=new PointF[2];
				p[0].X=m_SideRect.Left;
				p[0].Y=m_SideRect.Top;
                p[1].X=m_SideRect.Left;
				p[1].Y=m_SideRect.Bottom;

				System.Drawing.Drawing2D.LinearGradientBrush lgb=BarFunctions.CreateLinearGradientBrush(m_SideRect,m_SideBarImage.GradientColor1,m_SideBarImage.GradientColor2,m_SideBarImage.GradientAngle);
                g.FillRectangle(lgb,m_SideRect);
			}
			else if(!m_SideBarImage.BackColor.Equals(Color.Empty))
				g.FillRectangle(new SolidBrush(m_SideBarImage.BackColor),m_SideRect);
			
			if(m_SideBarImage.StretchPicture)
			{
				g.DrawImage(m_SideBarImage.Picture,m_SideRect);
			}
			else
			{
				if(m_SideBarImage.Alignment==eAlignment.Top)
					g.DrawImage(m_SideBarImage.Picture,m_SideRect.X,m_SideRect.Top,m_SideRect.Width,m_SideBarImage.Picture.Height);
				else if(m_SideBarImage.Alignment==eAlignment.Bottom)
					g.DrawImage(m_SideBarImage.Picture,m_SideRect.Left,m_SideRect.Bottom-m_SideBarImage.Picture.Height,m_SideBarImage.Picture.Width,m_SideBarImage.Picture.Height);
				else
					g.DrawImage(m_SideBarImage.Picture,m_SideRect.Left,m_SideRect.Top+(m_SideRect.Height-m_SideBarImage.Picture.Height)/2,m_SideBarImage.Picture.Width,m_SideBarImage.Picture.Height);
			}
		}

		public BaseItem ParentItem
		{
			get
			{
				return m_ParentItem;
			}
			set
			{
				RestoreContainer();
				m_ParentItem=value;
				if(m_ParentItem==null || m_ParentItem.SubItems.Count==0)
					return;

                if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ParentItem.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
					EXPAND_BUTTON_HEIGHT=OFFICE2003_EXPAND_BUTTON_HEIGHT;
				else
					EXPAND_BUTTON_HEIGHT=DEFAULT_EXPAND_BUTTON_HEIGHT;

				// Force ContainerChanged event so the items like MdiWindowListItem do not modify collection when we loop through it
				if(m_ParentItem.SubItems.Count>0)
					m_ParentItem.SubItems[0].ContainerControl=this;
				for(int i=1;i<m_ParentItem.SubItems.Count;i++)
					m_ParentItem.SubItems[i].ContainerControl=this;
				
				// Get the parent's screen position
				System.Windows.Forms.Control objCtrl=m_ParentItem.ContainerControl as System.Windows.Forms.Control;
				if(m_ParentItem.Displayed)
				{
					if(BaseItem.IsHandleValid(objCtrl))
					{
						m_ParentItemScreenPos=objCtrl.PointToScreen(new Point(m_ParentItem.LeftInternal,m_ParentItem.TopInternal));
						objCtrl=null;
					}
				}
				if(objCtrl is ItemControl)
					m_AntiAlias=((ItemControl)objCtrl).AntiAlias;
			}
		}

		private void SetupRegion()
		{
			if(m_ParentItem!=null)
			{
				Rectangle r=new Rectangle(0,0,this.Width,this.Height);
				System.Drawing.Region rgn=new System.Drawing.Region(r);
				r.X=this.Width-2;
				r.Y=0;
				r.Width=2;
				r.Height=2;
				rgn.Xor(r);
				//rgn.Exclude(r);
				r.X=0;
				r.Y=this.Height-2;
				r.Width=2;
				r.Height=2;
				rgn.Xor(r);
				this.Region=rgn;
			}
		}

        public void RecalcLayout()
        {
            this.RecalcSize();
            this.Invalidate(true);
        }

		public void RecalcSize()
		{
			if(m_ParentItem!=null)
			{
                m_ParentItem.IsRightToLeft = (this.RightToLeft == RightToLeft.Yes);
                if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
					RecalcSizeOffice();
				else
					RecalcSizeDotNet();
				if(this.Visible && m_PopupMenu && this.Parent!=null && this.Parent is PopupContainer)
				{
					ScreenInformation objScreen=BarFunctions.ScreenFromControl(this.Parent);
					if(objScreen!=null)
					{
                        Rectangle screenArea = objScreen.WorkingArea;
                        if (UseWholeScreenForSizeChecking)
                            screenArea = objScreen.Bounds;
						if(this.Parent.Location.Y+this.Size.Height>screenArea.Bottom)
						{
							// Reduce the size
							m_ClientRect.Height=m_ClientRect.Height-(this.Parent.Location.Y+this.Size.Height-screenArea.Bottom);
                            this.Height = this.Height - (this.Parent.Location.Y + this.Size.Height - screenArea.Bottom);
                            if (!m_Scroll)
                            {
                                m_Scroll = true;
                                m_ScrollTopPosition = 0;
                            }
							RepositionItems();
						}
						else
							m_Scroll=false;
					}
				}
			}
            if (this.Visible)
            {
                if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && IsRoundRegion)
                {
                    if(this.Parent!=null)
                        SetRoundRegion(this.Parent);
                    SetRoundRegion(this);
                }
                this.Refresh();
            }
		}

		protected override void OnResize(EventArgs e)
		{
			if(this.Parent is PopupContainer)
			{
				this.Parent.Size=this.Size;
			}
			base.OnResize(e);
		}

		private void RecalcSizeDotNet()
		{
			m_ExpandButton.ShowExpandButton=false;

			if(m_ParentItem==null || m_ParentItem.SubItems.Count==0)
			{
				this.Height=24;
				this.Width=m_ParentItem.WidthInternal+16;
				//SetupRegion(); Very bad from AnimateWindow on W2K when control is not shown
				return;
			}

			int iMaxWidth=0, iTop=ClientMarginTop, iLeft=ClientMarginLeft;
			int iMaxHeight=0;
			bool bAdjustHeight=false;
			bool rightToLeft = (this.RightToLeft == RightToLeft.Yes);
			// Take in account the side bar picture
			if(m_SideBarImage.Picture!=null)
				iLeft+=m_SideBarImage.Picture.Width;

			foreach(BaseItem objItem in m_ParentItem.SubItems)
			{
				objItem.RecalcSize();
				if(objItem.Visible || m_IsCustomizeMenu)
				{
					if(!m_ParentItem.DesignMode && m_PersonalizedMenus!=ePersonalizedMenus.Disabled && !m_ExpandButton.PersonalizedAllVisible && !m_IsCustomizeMenu)
					{
						IPersonalizedMenuItem ipm=objItem as IPersonalizedMenuItem;
						if(ipm!=null && ipm.MenuVisibility==eMenuVisibility.VisibleIfRecentlyUsed && !ipm.RecentlyUsed)
						{
                            objItem.Displayed=false;
							m_ExpandButton.ShowExpandButton=true;
							continue;
						}
					}
					if(objItem.HeightInternal!=iMaxHeight && objItem is ImageItem)
					{
						if(iMaxHeight>0)
							bAdjustHeight=true;
						if(objItem.HeightInternal>iMaxHeight)
							iMaxHeight=objItem.HeightInternal;
					}
					if(objItem.BeginGroup && iTop>ClientMarginTop)
						iTop+=GROUP_SPACINGDOTNET;
					objItem.TopInternal=iTop;
					objItem.LeftInternal=iLeft;
					iTop+=objItem.HeightInternal;
					if(objItem.WidthInternal>iMaxWidth)
						iMaxWidth=objItem.WidthInternal;
					objItem.Displayed=true;
				}
				else
					objItem.Displayed=false;
			}

			if(iMaxWidth==0)
				iMaxWidth=120;

			if(bAdjustHeight)
			{
				iTop=ClientMarginTop;
				foreach(BaseItem objItem in m_ParentItem.SubItems)
				{
					objItem.WidthInternal=iMaxWidth;
					
					if(objItem.Displayed)
					{
						if(objItem.BeginGroup && iTop>ClientMarginTop)
							iTop+=GROUP_SPACINGDOTNET;
						objItem.TopInternal=iTop;
						iTop+=objItem.HeightInternal;
					}
                    //if (objItem.IsContainer && rightToLeft)
                    //    objItem.RecalcSize();
				}
			}
			else
			{
				// Set each item width to max width
                foreach (BaseItem objItem in m_ParentItem.SubItems)
                {
                    objItem.WidthInternal = iMaxWidth;
                    //if (objItem.IsContainer && rightToLeft)
                    //    objItem.RecalcSize();
                }
			}
			
			if(m_ExpandButton.ShowExpandButton)
			{
				m_ExpandButton.Rect=new Rectangle(iLeft,iTop,iMaxWidth,EXPAND_BUTTON_HEIGHT);
				iTop+=EXPAND_BUTTON_HEIGHT;
			}

			m_ClientRect=new Rectangle(iLeft,ClientMarginTop,iMaxWidth,iTop-ClientMarginTop);
			if(m_SideBarImage.Picture!=null)
				m_SideRect=new Rectangle(iLeft-m_SideBarImage.Picture.Width,ClientMarginTop,m_SideBarImage.Picture.Width,iTop-ClientMarginTop);
			this.Size=new Size(iLeft+iMaxWidth+ClientMarginRight,iTop+ClientMarginBottom);
			if(this.Height==92)
				this.Height=92;
			//SetupRegion(); Very bad for AnimateWindow on W2K when control is not shown
		}

		private void RecalcSizeOffice()
		{
			m_ExpandButton.ShowExpandButton=false;

			if(m_ParentItem==null || m_ParentItem.SubItems.Count==0)
			{
				this.Height=24;
				this.Width=m_ParentItem.WidthInternal+16;
				return;
			}

			int iMaxWidth=0, iTop=ClientMarginTop, iLeft=ClientMarginLeft;
			int iMaxHeight=0;
			bool bAdjustHeight=false;
			
			// Take in account the side bar picture
			if(m_SideBarImage.Picture!=null)
				iLeft+=m_SideBarImage.Picture.Width;

			foreach(BaseItem objItem in m_ParentItem.SubItems)
			{
				objItem.RecalcSize();
				if(objItem.Visible || m_IsCustomizeMenu)
				{
					if(!m_ParentItem.DesignMode && m_PersonalizedMenus!=ePersonalizedMenus.Disabled && !m_ExpandButton.PersonalizedAllVisible && !m_IsCustomizeMenu)
					{
						IPersonalizedMenuItem ipm=objItem as IPersonalizedMenuItem;
						if(ipm!=null && ipm.MenuVisibility==eMenuVisibility.VisibleIfRecentlyUsed && !ipm.RecentlyUsed)
						{
							objItem.Displayed=false;
							m_ExpandButton.ShowExpandButton=true;
							continue;
						}
					}
					if(objItem.HeightInternal!=iMaxHeight && objItem is ImageItem)
					{
						if(iMaxHeight>0)
							bAdjustHeight=true;
						if(objItem.HeightInternal>iMaxHeight)
							iMaxHeight=objItem.HeightInternal;
					}
					if(objItem.BeginGroup && iTop>ClientMarginTop)
						iTop+=GROUP_SPACINGOFFICE;
					objItem.TopInternal=iTop;
					objItem.LeftInternal=iLeft;
					iTop+=objItem.HeightInternal;
					if(objItem.WidthInternal>iMaxWidth)
						iMaxWidth=objItem.WidthInternal;
					objItem.Displayed=true;
				}
				else
					objItem.Displayed=false;
			}

			if(iMaxWidth==0)
				iMaxWidth=120;

			if(bAdjustHeight)
			{
				iTop=3;
				foreach(BaseItem objItem in m_ParentItem.SubItems)
				{
                    //if (objItem is ImageItem)
                    //    objItem.HeightInternal = iMaxHeight;
					objItem.WidthInternal=iMaxWidth;

					if(objItem.Displayed)
					{
						if(objItem.BeginGroup && iTop>ClientMarginTop)
							iTop+=GROUP_SPACINGOFFICE;
						objItem.TopInternal=iTop;
						//iTop+=iMaxHeight;
						iTop+=objItem.HeightInternal;
					}
				}
			}
			else
			{
				// Set each item width to max width
				foreach(BaseItem objItem in m_ParentItem.SubItems)
					objItem.WidthInternal=iMaxWidth;
			}

			if(m_ExpandButton.ShowExpandButton)
			{
				m_ExpandButton.Rect=new Rectangle(iLeft,iTop,iMaxWidth,EXPAND_BUTTON_HEIGHT);
				iTop+=EXPAND_BUTTON_HEIGHT;
			}
			
			m_ClientRect=new Rectangle(iLeft,ClientMarginTop,iMaxWidth,iTop-ClientMarginTop);

			if(m_SideBarImage.Picture!=null)
				m_SideRect=new Rectangle(iLeft-m_SideBarImage.Picture.Width,ClientMarginTop,m_SideBarImage.Picture.Width,iTop);
			this.Size=new Size(iLeft+iMaxWidth+ClientMarginRight,iTop+ClientMarginBottom);
		}

		private void RestoreContainer()
		{
			if(m_ParentItem!=null && m_ParentItem.SubItems.Count!=0)
			{
				foreach(BaseItem objItem in m_ParentItem.SubItems)
				{
					objItem.ContainerControl=null;
				}
				int iLastIndex=0;
				while(m_ParentItem!=null && iLastIndex<m_ParentItem.SubItems.Count)
				{
					m_ParentItem.SubItems[iLastIndex].Expanded=false;
					iLastIndex++;
				}
				iLastIndex=0;
				while(m_ParentItem!=null && iLastIndex<m_ParentItem.SubItems.Count)
				{
					m_ParentItem.SubItems[iLastIndex].Displayed=false;
					iLastIndex++;
				}
			}
		}

		private BaseItem ExpandedItem()
		{
			if(m_ParentItem!=null && m_ParentItem.SubItems.Count>0)
			{
				foreach(BaseItem objSub in m_ParentItem.SubItems)
				{
                    if (objSub.Expanded && objSub is PopupItem)
						return objSub;
                    if (objSub.IsContainer)
                    {
                        BaseItem exp = objSub.ExpandedItem();
                        if (exp is PopupItem)
                            return exp;
                    }
				}
			}
			return null;
		}

		protected internal BaseItem FocusedItem()
		{
			if(m_ParentItem!=null && m_ParentItem.SubItems.Count>0)
			{
                foreach (BaseItem objSub in m_ParentItem.SubItems)
                {
                    if (objSub.Focused)
                    {
                        return objSub;
                    }
                    else if (objSub.IsContainer)
                    {
                        BaseItem focused = objSub.FocusedItem();
                        if (focused != null)
                            return focused;
                    }
                }
			}
			return null;
		}

		internal void SetFocusItem(BaseItem objFocus)
		{
			if(m_ParentItem==null || m_ParentItem.Parent!=null)
				return;
			BaseItem objItem=FocusedItem();
            if(objItem==objFocus)
				return;
			
			if(objItem!=null)
				objItem.OnLostFocus();

			if(objFocus!=null)
				objFocus.OnGotFocus();
		}

		internal void InternalMouseMove(MouseEventArgs e)
		{
			this.OnMouseMove(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if(m_IgnoreDuplicateMouseMove)
			{
				if(m_LastMouseMoveEvent==null)
				{
					m_LastMouseMoveEvent=new System.Windows.Forms.MouseEventArgs(e.Button,e.Clicks,e.X,e.Y,e.Delta);
					return;
				}
				if(m_LastMouseMoveEvent.X!=e.X || m_LastMouseMoveEvent.Y!=e.Y ||
					m_LastMouseMoveEvent.Button!=e.Button)
				{
					m_IgnoreDuplicateMouseMove=false;
					m_LastMouseMoveEvent=null;
				}
				else
					return;
			}

			// Don't forward event if some other item has focus. This is the case for example for
			// Text box items or any other item that can receive focus...
			//if(!m_ParentItem.DesignMode && m_Owner!=null && m_Owner.GetFocusItem()!=null && m_ParentItem.ContainerControl!=null || this.FocusedItem()!=null && !m_ParentItem.DesignMode)
			//	return;

            if (m_ParentItem != null && m_ParentItem.DesignMode && e.Button == System.Windows.Forms.MouseButtons.Left && (Math.Abs(e.X - m_MouseDownPt.X) >= 2 || Math.Abs(e.Y - m_MouseDownPt.Y) >= 2 || m_DragDropInProgress))
			{
				BaseItem objFocus=this.FocusedItem();
				IOwner owner=m_Owner as IOwner;
                ISite site = this.GetSite();
                if (site != null && objFocus!=null)
                {
                    DesignTimeMouseMove(e);
                }
                else
                {
                    if (objFocus != null && owner != null && objFocus.CanCustomize)
                        owner.StartItemDrag(objFocus);
                }
			}

			if(m_ParentItem==null || m_ParentItem.SubItems.Count==0 || m_ParentItem.DesignMode)
				return;

            //if (e.Button != MouseButtons.None && m_HotSubItem != null)
            //{
            //    m_HotSubItem.InternalMouseMove(e);
            //    return;
            //}

			BaseItem objNew=ItemAtLocation(e.X,e.Y);

			BaseItem objExpanded=ExpandedItem();

			if(objExpanded!=null && objExpanded is PopupItem)
			{
				Control ctrlExpanded=((PopupItem)objExpanded).PopupControl;
				if(ctrlExpanded!=null)
				{
					Point pClient=ctrlExpanded.PointToClient(this.PointToScreen(new Point(e.X,e.Y)));
					if(ctrlExpanded.ClientRectangle.Contains(pClient))
					{
						if(ctrlExpanded is MenuPanel)
						{
							((MenuPanel)ctrlExpanded).InternalMouseMove(new MouseEventArgs(e.Button,e.Clicks,pClient.X,pClient.Y,e.Delta));
							return;
						}
					}
				}
			}

            if (objExpanded != null && objNew != objExpanded)
            {
                if (objExpanded is PopupItem && ((PopupItem)objExpanded).PopupType != ePopupType.Menu ||
                    objExpanded != null && m_ParentItem != null && m_ParentItem.SubItems.Count > 0 && m_ParentItem.SubItems[0] is ItemContainer && !(m_ParentItem is Office2007StartButton))
                {
                    objExpanded.Expanded = false;
                }
                else
                    CollapseItemDelayed(objExpanded);
            }

			if(m_ExpandButton.ShowExpandButton)
			{
				if(objNew==null && m_ExpandButton.Rect.Contains(e.X,e.Y) && !m_ExpandButton.MouseOver)
				{
					m_ExpandButton.MouseOver=true;
					RefreshExpandButton();
				}
				else if(objNew!=null && m_ExpandButton.MouseOver)
				{
					m_ExpandButton.MouseOver=false;
					RefreshExpandButton();
				}
			}

            if (objNew == null && m_HotSubItem is GalleryContainer)
            {
                GalleryContainer gc = m_HotSubItem as GalleryContainer;
                if (!gc.SystemGallery && gc.PopupUsesStandardScrollbars && gc.ScrollBarControl != null && gc.ScrollBarControl.IsMouseDown && e.Button == MouseButtons.Left)
                    objNew = m_HotSubItem;
            }

			if(objNew!=m_HotSubItem)
			{
				if(m_HotSubItem!=null)
				{
					// Don't leave the hot item if it is exanded and mouse is inside the side bar picture, if we have side bar picture
					//if(m_HotSubItem.Expanded && !m_SideRect.IsEmpty && m_SideRect.Contains(e.X,e.Y))
					//	return;
					m_HotSubItem.InternalMouseLeave();
					// We need to reset hover thing since it is fired only first time mouse hovers inside the window and we need it for each of our items
					if(m_Hover)
					{
						ResetHover();
						m_Hover=false;
					}
				}

                if (objNew != null)
                {
                    objNew.InternalMouseEnter();
                    objNew.InternalMouseMove(e);
                    m_HotSubItem = objNew;
                }
                else
                {
                    m_HotSubItem = null;
                }
			}
			else if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseMove(e);
			}

            // Gallery scrollbar support
            if (this.Capture && !this.DisplayRectangle.Contains(e.X, e.Y) && m_ParentItem!=null &&
                m_ParentItem.SubItems.Count>0 && m_ParentItem.SubItems[0] is GalleryContainer)
            {
                if (m_ParentItem.SubItems[0].Visible && m_ParentItem.SubItems[0].Displayed)
                    m_ParentItem.SubItems[0].InternalMouseMove(e);
            }
        }

        #region Design Time Drag & Drop
        private bool m_DragDropInProgress = false;
        private int m_InsertPosition=-1;
        private bool m_InsertBefore=false;
        private IDesignTimeProvider m_DesignTimeProvider=null;
        private BaseItem m_DragItem = null;
        private IDesignTimeProvider m_DragDropDesignTimeProvider = null;

        private ISite GetSite()
        {
            ISite site = null;
            IOwner owner = this.Owner as IOwner;
            Control c = null;
            if (owner is Control)
            {
                c = owner as Control;
            }
            else if (m_ParentItem != null && m_ParentItem.ContainerControl is Control)
            {
                c = m_ParentItem.ContainerControl as Control;
            }

            if (c != null)
            {
                while (site == null && c != null)
                {
                    if (c.Site != null && c.Site.DesignMode)
                        site = c.Site;
                    else
                        c = c.Parent;
                }
            }

            if (site == null && m_ParentItem != null)
            {
                BaseItem item = m_ParentItem;
                while (site == null && item != null)
                {
                    if (item.Site != null && item.Site.DesignMode)
                        site = item.Site;
                    else
                        item = item.Parent;
                }
            }

            return site;
        }

        private void DesignTimeMouseMove(MouseEventArgs e)
        {
            if (m_DragDropInProgress)
            {
                try
                {
                    if (m_DesignTimeProvider != null)
                    {
                        m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
                        m_DesignTimeProvider = null;
                    }
                    InsertPosition pos = m_DragDropDesignTimeProvider.GetInsertPosition(Control.MousePosition, m_DragItem);
                    
                    if (pos != null)
                    {
                        if (pos.TargetProvider == null)
                        {
                            // Cursor is over drag item
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.No;
                        }
                        else
                        {
                            pos.TargetProvider.DrawReversibleMarker(pos.Position, pos.Before);
                            m_InsertPosition = pos.Position;
                            m_InsertBefore = pos.Before;
                            m_DesignTimeProvider = pos.TargetProvider;
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.No;
                    }
                }
                catch
                {
                    m_DesignTimeProvider = null;
                }
            }
            else
            {
                // Get top level design-time provider
                BaseItem item = m_ParentItem;
                while (item.Parent is IDesignTimeProvider)
                    item = item.Parent;

                ISite site = GetSite();
                if (site != null && item.ContainerControl!=null)
                {
                    IDesignerHost dh = site.GetService(typeof(IDesignerHost)) as IDesignerHost;
                    if (dh != null)
                    {
                        Control c = item.ContainerControl as Control;
                        if (c is RibbonStrip && c.Site == null && c.Parent is RibbonControl)
                            c = c.Parent;
                        IBarItemDesigner designer = dh.GetDesigner(c) as IBarItemDesigner;
                        if (designer != null)
                        {
                            designer.StartExternalDrag(this.FocusedItem());
                            return;
                        }
                    }
                }
                
                m_DragDropDesignTimeProvider = (IDesignTimeProvider)item;
                m_DragItem = this.FocusedItem();
                m_DragDropInProgress = true;
                this.Capture = true;
            }
        }

        private void DesignTimeMouseUp(MouseEventArgs e)
        {
            ISite site = GetSite();
            if (site == null)
                return;
            IComponentChangeService cc = site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            if (m_DesignTimeProvider != null)
            {
                m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
                BaseItem objParent = m_DragItem.Parent;
                if (objParent != null)
                {
                    if (objParent == (BaseItem)m_DesignTimeProvider && m_InsertPosition > 0)
                    {
                        if (objParent.SubItems.IndexOf(m_DragItem) < m_InsertPosition)
                            m_InsertPosition--;
                    }
                    if (cc != null)
                        cc.OnComponentChanging(objParent, TypeDescriptor.GetProperties(objParent)["SubItems"]);
                    
                    objParent.SubItems.Remove(m_DragItem);
                    
                    if (cc != null)
                        cc.OnComponentChanged(objParent, TypeDescriptor.GetProperties(objParent)["SubItems"], null, null);

                    Control ctrl = objParent.ContainerControl as Control;
                    if (ctrl is Bar)
                        ((Bar)ctrl).RecalcLayout();
                    else if (ctrl is MenuPanel)
                        ((MenuPanel)ctrl).RecalcSize();
                }
                
                m_DesignTimeProvider.InsertItemAt(m_DragItem, m_InsertPosition, m_InsertBefore);
                
                m_DesignTimeProvider = null;
                
            }
            m_DragDropDesignTimeProvider = null;
            m_DragItem = null;
            m_DragDropInProgress = false;
            this.Capture = false;
        }

        private void DesignTimeMouseDown(MouseEventArgs e)
        {
            IOwner owner = this.Owner as IOwner;
            BaseItem objNew = ItemAtLocation(e.X, e.Y);
            
            // If parent of the item is GalleryContainer ensure that mouse is not over gallery scroll buttons
            if (objNew != null && objNew.Parent is GalleryContainer)
            {
                GalleryContainer gc = objNew.Parent as GalleryContainer;
                if (gc.ScrollHitTest(e.X, e.Y))
                    objNew = gc;
            }

            // Ignore system items
            if (objNew != null && objNew.SystemItem)
                objNew = null;

            if (objNew == null)
            {
                if(e.Button == MouseButtons.Right)
                {
                    ISite site = GetSite();
                    if (site != null)
                    {
                        ISelectionService selection = (ISelectionService)site.GetService(typeof(ISelectionService));
                        if (selection.PrimarySelection == m_ParentItem)
                        {
                            // Show context menu for parent item...
                            IMenuCommandService service1 = (IMenuCommandService)site.GetService(typeof(IMenuCommandService));
                            if (service1 != null)
                            {
                                service1.ShowContextMenu(new CommandID(new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}"), 0x500)/*System.Windows.Forms.Design.MenuCommands.SelectionMenu*/, Control.MousePosition.X, Control.MousePosition.Y);
                            }
                        }
                    }
                }
                return;
            }

            BaseItem objExpanded = ExpandedItem();
            if (objExpanded != null && m_HotSubItem != objExpanded)
                objExpanded.Expanded = false;

            if (owner != null)
            {
                owner.SetFocusItem(objNew);
                ISite site = GetSite();
                if (site != null)
                {
                    ISelectionService selection = (ISelectionService)site.GetService(typeof(ISelectionService));
                    if (selection != null)
                    {
                        ArrayList arr = new ArrayList(1);
                        arr.Add(objNew);
#if FRAMEWORK20
                        selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                            selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        IMenuCommandService service1 = (IMenuCommandService)site.GetService(typeof(IMenuCommandService));
                        if (service1 != null)
                        {
                            service1.ShowContextMenu(new CommandID(new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}"), 0x500)/*System.Windows.Forms.Design.MenuCommands.SelectionMenu*/, Control.MousePosition.X, Control.MousePosition.Y);
                        }
                    }
                }
            }
            owner = null;
            if (objNew != null)
                objNew.InternalMouseDown(e);
        }
        #endregion

        internal void SelectFirstItem()
		{
			if(m_ParentItem!=null)
			{
				BaseItem disp=null;
				foreach(BaseItem item in m_ParentItem.SubItems)
				{
                    if (item.Displayed && item.GetEnabled())
					{
                        if (item is ItemContainer)
                        {
                            if (!((ItemContainer)item).SelectFirstItem())
                                continue;
                        }
						disp=item;
						break;
					}
				}
				if(disp!=null && m_HotSubItem!=disp)
				{
					if(m_HotSubItem!=null)
					{
						// Don't leave the hot item if it is exanded and mouse is inside the side bar picture, if we have side bar picture
						//if(m_HotSubItem.Expanded && !m_SideRect.IsEmpty && m_SideRect.Contains(e.X,e.Y))
						//	return;
						m_HotSubItem.InternalMouseLeave();
						// We need to reset hover thing since it is fired only first time mouse hovers inside the window and we need it for each of our items
						if(m_Hover)
						{
							ResetHover();
							m_Hover=false;
						}
					}
				
					if(disp!=null)
					{
                        if (!(disp is ItemContainer))
                        {
                            disp.InternalMouseEnter();
                            disp.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, disp.LeftInternal + 1, disp.TopInternal + 1, 0));
                        }
						m_HotSubItem=disp;
					}
				}
			}
		}
		
		private void CollapseItemDelayed(BaseItem item)
		{
			ResetHover();
			
			if(m_DelayedCollapseItem!=null && m_DelayedCollapseItem!=item)
			{
				m_DelayedCollapseItem.Expanded=false;
			}

			if(!m_LastExpandedOnHover)
			{
				item.Expanded=false;
				m_DelayedCollapseItem=null;
				m_LastExpandedOnHover=true; // was false
			}
			else
			{
				m_DelayedCollapseItem=item;
				m_DelayedCollapseItem.InternalMouseLeave();
			}
		}

		internal void InternalMouseHover()
		{
			if(m_ParentItem!=null && m_ParentItem.DesignMode || m_ParentItem==null)
				return;
			
			// Hover delayed collapse of last expanded item
			if(m_DelayedCollapseItem!=null)
			{
				m_DelayedCollapseItem.Expanded=false;
				m_DelayedCollapseItem=null;
			}

			if(m_HotSubItem!=null)
			{
				bool bWasExpanded=false;
				bWasExpanded=m_HotSubItem.Expanded;
				m_HotSubItem.InternalMouseHover();
				m_Hover=true;
				if(!bWasExpanded && m_HotSubItem!=null && m_HotSubItem.Expanded)
					m_LastExpandedOnHover=true;
				else
					m_LastExpandedOnHover=true; // was false
			}
			else if(m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
			{
				if(m_PersonalizedMenus!=ePersonalizedMenus.DisplayOnClick)
				{
					m_ExpandButton.PersonalizedAllVisible=true;
					IOwnerMenuSupport ownersupport=m_Owner as IOwnerMenuSupport;
					if(ownersupport!=null)
						ownersupport.PersonalizedAllVisible=true;
					RecalcSize();
					ResetHover();
				}
			}
			else if(m_Scroll)
			{
				if(m_ScrollTimer==null)
				{
					CheckScrolling();
					ResetHover();
				}
			}
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			InternalMouseHover();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if(m_ParentItem!=null && m_ParentItem.DesignMode || m_ParentItem==null)
				return;
			// If we had hot sub item pass the mouse leave message to it...
			if(m_HotSubItem!=null) // && !m_HotSubItem.Expanded)
			{
				// Handle the case when child control of the item gets the mouse
				Point mp=this.PointToClient(Control.MousePosition);
                if (!m_HotSubItem.DisplayRectangle.Contains(mp))
				{
                    if (!(m_HotSubItem is PopupItem && m_HotSubItem.Expanded))
                    {
                        m_HotSubItem.InternalMouseLeave();
                        m_HotSubItem = null;
                    }
				}
			}
			else if(m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
			{
				m_ExpandButton.MouseOver=false;
				RefreshExpandButton();
			}

            if (m_HotSubItem == null)
            {
                BaseItem exp = ExpandedItem();
                if (exp != null && exp is PopupItem)
                {
                    m_HotSubItem = exp;
                    m_HotSubItem.InternalMouseEnter();
                    m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.DisplayRectangle.X+4, m_HotSubItem.DisplayRectangle.Y+4, 0));
                }
            }

			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(m_ParentItem==null)
				return;
            this.ShowKeyTips = false;

			IOwner owner=this.Owner as IOwner;
			m_MouseDownPt=new System.Drawing.Point(e.X,e.Y);
			if(m_ParentItem.DesignMode)
			{
                DesignTimeMouseDown(e);
			}

			if(!this.DesignMode && owner!=null && owner.GetFocusItem()!=null)
			{
				BaseItem objNew=ItemAtLocation(e.X,e.Y);
				if(objNew!=owner.GetFocusItem())
					owner.GetFocusItem().ReleaseFocus();
					
			}

            if (e.Button == MouseButtons.Right && m_HotSubItem != null && !this.IsCustomizeMenu && owner is IRibbonCustomize && m_ParentItem!=null && m_ParentItem.Name!="syscustomizepopupmenu")
                ((IRibbonCustomize)owner).ItemRightClick(m_HotSubItem);

			if(m_HotSubItem!=null)
				m_HotSubItem.InternalMouseDown(e);
			else if(m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
			{
				RefreshExpandButton();
			}

			base.OnMouseDown(e);
		}

		internal void InternalMouseUp(MouseEventArgs e)
		{
			this.OnMouseUp(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
            if (m_DragDropInProgress)
            {
                DesignTimeMouseUp(e);
            }
            else
            {
                if (m_HotSubItem != null)
                {
                    m_HotSubItem.InternalMouseUp(e);
                }
                else if (m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
                {
                    if (m_PersonalizedMenus != ePersonalizedMenus.DisplayOnHover && m_ExpandButton.Rect.Contains(e.X, e.Y))
                    {
                        ExpandRecentlyUsed();
                    }
                    else
                        RefreshExpandButton();
                }
            }

			base.OnMouseUp(e);
		}

		internal void ExpandRecentlyUsed()
		{
			if(m_PersonalizedMenus!=ePersonalizedMenus.DisplayOnHover)
			{
				m_ExpandButton.PersonalizedAllVisible=true;
				IOwnerMenuSupport ownermenu=this.Owner as IOwnerMenuSupport;
				if(ownermenu!=null)
					ownermenu.PersonalizedAllVisible=true;
				RecalcSize();
			}
		}

		protected override void OnClick(EventArgs e)
		{
			if(m_ParentItem!=null && m_ParentItem.DesignMode || m_ParentItem==null)
				return;

			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalClick(Control.MouseButtons,Control.MousePosition);
			}
			else if(m_Scroll)
			{
				CheckScrolling();
			}
			base.OnClick(e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
            ISite site = this.GetSite();
			if(site!=null && site.DesignMode)
			{
				ISelectionService selection = (ISelectionService) site.GetService(typeof(ISelectionService));
				if(selection!=null)
				{
					IDesignerHost host=(IDesignerHost) site.GetService(typeof(IDesignerHost));
					if(host!=null)
					{
						IDesigner designer=host.GetDesigner(selection.PrimarySelection as IComponent);
						if(designer!=null)
						{
							designer.DoDefaultAction();
						}
					}
				}
			}

			if(m_ParentItem!=null && m_ParentItem.DesignMode || m_ParentItem==null)
				return;

			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalDoubleClick(Control.MouseButtons,Control.MousePosition);
			}
			base.OnDoubleClick(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(m_ParentItem==null || m_ParentItem.SubItems.Count==0)
			{
				base.OnKeyDown(e);
				return;
			}
			this.ExKeyDown(e);
			base.OnKeyDown(e);
		}

        private bool CanGetMouseFocus(BaseItem item)
        {
            if (item is LabelItem)
                return false;
            return true;
        }

		internal void ExKeyDown(KeyEventArgs e)
		{
			if(m_ParentItem==null || m_ParentItem.SubItems.Count==0 || m_ParentItem.DesignMode)
				return;

			if(m_HotSubItem==null || m_HotSubItem!=null && !m_HotSubItem.Expanded)
			{
				if(e.KeyCode==Keys.Down)
				{
                    if (m_ParentItem is ItemContainer)
                    {
                        m_ParentItem.InternalKeyDown(e);
                    }
                    else
                    {
                        if (m_HotSubItem is ItemContainer)
                            m_HotSubItem.InternalKeyDown(e);

                        if (!e.Handled)
                        {
                            int i = 0;
                            if (m_HotSubItem != null)
                            {
                                m_HotSubItem.InternalMouseLeave();
                                i = m_ParentItem.SubItems.IndexOf(m_HotSubItem) + 1;
                                if (i == m_ParentItem.SubItems.Count && m_ExpandButton.ShowExpandButton)
                                {
                                    m_ExpandButton.MouseOver = true;
                                    RefreshExpandButton();
                                }
                                else if (i < 0 || i == m_ParentItem.SubItems.Count)
                                {
                                    i = 0;
                                }
                                m_HotSubItem = null;
                            }
                            else if (m_ExpandButton.MouseOver)
                            {
                                m_ExpandButton.MouseOver = false;
                                RefreshExpandButton();
                                i = 0;
                            }
                            BaseItem objTmp;
                            for (int f = i; f < m_ParentItem.SubItems.Count; f++)
                            {
                                objTmp = m_ParentItem.SubItems[f];
                                if (objTmp.Displayed && objTmp.Visible && CanGetMouseFocus(objTmp))
                                {
                                    m_HotSubItem = objTmp;
                                    m_HotSubItem.InternalMouseEnter();
                                    m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
                                    if (m_HotSubItem.IsContainer)
                                        m_HotSubItem.InternalKeyDown(e);
                                    break;
                                }
                            }
                            if (m_HotSubItem == null && m_ExpandButton.ShowExpandButton && !m_ExpandButton.MouseOver)
                            {
                                m_ExpandButton.MouseOver = true;
                                RefreshExpandButton();
                            }
                        }
                    }
					e.Handled=true;
					m_IgnoreDuplicateMouseMove=true;
				}
				else if(e.KeyCode==Keys.Up)
				{
                    if (m_ParentItem is ItemContainer)
                    {
                        m_ParentItem.InternalKeyDown(e);
                    }
                    else
                    {
                        if (m_HotSubItem is ItemContainer)
                            m_HotSubItem.InternalKeyDown(e);
                        if (!e.Handled)
                        {
                            int i = 0;
                            if (m_HotSubItem != null)
                            {
                                m_HotSubItem.InternalMouseLeave();
                                i = m_ParentItem.SubItems.IndexOf(m_HotSubItem) - 1;
                                if (i < 0 && m_ExpandButton.ShowExpandButton)
                                {
                                    m_ExpandButton.MouseOver = true;
                                    RefreshExpandButton();
                                }
                                else if (i < 0)
                                    i = m_ParentItem.SubItems.Count - 1;
                                m_HotSubItem = null;
                            }
                            else if (m_ExpandButton.MouseOver)
                            {
                                m_ExpandButton.MouseOver = false;
                                RefreshExpandButton();
                                i = m_ParentItem.SubItems.Count - 1;
                            }
                            else if (m_ExpandButton.ShowExpandButton)
                            {
                                m_ExpandButton.MouseOver = true;
                                RefreshExpandButton();
                            }
                            else
                                i = m_ParentItem.SubItems.Count - 1;
                            BaseItem objTmp;
                            for (int f = i; f >= 0; f--)
                            {
                                objTmp = m_ParentItem.SubItems[f];
                                if (objTmp.Displayed && objTmp.Visible && CanGetMouseFocus(objTmp))
                                {
                                    m_HotSubItem = objTmp;
                                    m_HotSubItem.InternalMouseEnter();
                                    m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
                                    if (m_HotSubItem.IsContainer)
                                        m_HotSubItem.InternalKeyDown(e);
                                    break;
                                }
                            }
                            if (m_HotSubItem == null && m_ExpandButton.ShowExpandButton && !m_ExpandButton.MouseOver)
                            {
                                m_ExpandButton.MouseOver = true;
                                RefreshExpandButton();
                            }
                        }
                    }
					e.Handled=true;
					m_IgnoreDuplicateMouseMove=true;
				}
				else if(e.KeyCode==Keys.Right)
				{
					m_IgnoreDuplicateMouseMove=true;
                    if (m_HotSubItem != null && m_HotSubItem.GetEnabled())
                    {
                        ButtonItem objBtn = m_HotSubItem as ButtonItem;
                        if (objBtn != null)
                        {
                            if (objBtn.SubItems.Count > 0 && objBtn.ShowSubItems && !objBtn.Expanded)
                            {
                                objBtn.Expanded = true;
                                if (objBtn.PopupControl is MenuPanel)
                                    ((MenuPanel)objBtn.PopupControl).SelectFirstItem();

                                e.Handled = true;
                            }
                        }
                    }
                    if (m_HotSubItem != null && m_HotSubItem is ItemContainer)
                        m_HotSubItem.InternalKeyDown(e);

                    if (!e.Handled && m_ParentItem!=null)
                    {
                        foreach (BaseItem item in m_ParentItem.SubItems)
                        {
                            if (item is ItemContainer)
                            {
                                item.InternalKeyDown(e);
                                if (e.Handled)
                                    break;
                            }
                        }
                    }
				}
				else if(e.KeyCode==Keys.Left)
				{
                    if (m_HotSubItem != null && m_HotSubItem is ItemContainer)
                        m_HotSubItem.InternalKeyDown(e);
                    if (!e.Handled)
                    {
                        m_IgnoreDuplicateMouseMove = true;
                        // Close this popup
                        if (BaseItem.IsOnPopup(m_ParentItem))
                        {
                            m_ParentItem.Expanded = false;
                            e.Handled = true;
                        }
                    }
				}
				else if(e.KeyCode==Keys.Escape)
				{
                    BaseItem parent = m_ParentItem;
                    parent.Expanded = false;
                    if (parent.Parent != null && parent.Parent is GenericItemContainer && parent.Parent.AutoExpand)
                        parent.Parent.AutoExpand = false;
                    e.Handled = true;
				}
                else if (e.KeyCode == Keys.Enter && m_HotSubItem != null && m_HotSubItem.IsContainer)
                {
                    m_HotSubItem.InternalKeyDown(e);
                }
                else if (e.KeyCode == Keys.Enter && m_HotSubItem != null && m_HotSubItem.SubItems.Count > 0 && m_HotSubItem.ShowSubItems && !m_HotSubItem.Expanded && m_HotSubItem.GetEnabled())
                {
                    m_HotSubItem.Expanded = true;
                    if (m_HotSubItem is PopupItem && ((PopupItem)m_HotSubItem).PopupControl is MenuPanel)
                        ((MenuPanel)((PopupItem)m_HotSubItem).PopupControl).SelectFirstItem();

                    e.Handled = true;
                }
                else if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space) && m_ExpandButton.ShowExpandButton && m_ExpandButton.MouseOver)
                {
                    ExpandRecentlyUsed();
                }
                else
                {
                    int key = 0;
                    if (e.Shift)
                    {
                        try
                        {
                            byte[] keyState = new byte[256];
                            if (NativeFunctions.GetKeyboardState(keyState))
                            {
                                byte[] chars = new byte[2];
                                if (NativeFunctions.ToAscii((uint)e.KeyValue, 0, keyState, chars, 0) != 0)
                                {
                                    key = chars[0];
                                }
                            }
                        }
                        catch (Exception)
                        {
                            key = 0;
                        }
                    }

                    if (key == 0 && !BarFunctions.IsSystemKey(e.KeyCode))
                    {
                        key = (int)NativeFunctions.MapVirtualKey((uint)e.KeyValue, 2);
                        if (key == 0)
                            key = e.KeyValue;
                    }
                    if (key > 0)
                    {
                        char[] ch = new char[1];
                        byte[] by = new byte[1];
                        by[0] = System.Convert.ToByte(key);
                        System.Text.Encoding.Default.GetDecoder().GetChars(by, 0, 1, ch, 0);
                        string s = ch[0].ToString();
                        ch[0] = (s.ToLower())[0];
                        if (ProcessContainerAccessKey(m_ParentItem, ch[0]))
                            e.Handled = true;
                    }
                }
			}

			if(!e.Handled && m_HotSubItem!=null)
			{
                bool raiseKeyDown=true;
                if (this.Controls.Count > 0)
                {
                    foreach (Control childControl in this.Controls)
                    {
                        if (childControl.Focused)
                        {
                            raiseKeyDown = false;
                            break;
                        }
                    }
                }
                if(raiseKeyDown)
    				m_HotSubItem.InternalKeyDown(e);
			}
		}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.Enter)
            {
                KeyEventArgs args = new KeyEventArgs(keyData);
                ExKeyDown(args);
                if (args.Handled) return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool ProcessContainerAccessKey(BaseItem container, char key)
        {
            List<BaseItem> itemsForMnemonic = GetItemsForMnemonic(container, key);
            if (itemsForMnemonic.Count > 1)
            {
                // Special processing for case when more than one item uses same mnemonic key
                BaseItem newHotSubitem = null;
                if (m_HotSubItem == null)
                    newHotSubitem = itemsForMnemonic[0];
                else
                {
                    int hotItemIndex = itemsForMnemonic.IndexOf(m_HotSubItem);
                    if (hotItemIndex == -1)
                        newHotSubitem = itemsForMnemonic[0];
                    else
                    {
                        if (hotItemIndex == itemsForMnemonic.Count - 1)
                            hotItemIndex = 0;
                        else
                            hotItemIndex++;
                        newHotSubitem = itemsForMnemonic[hotItemIndex];
                    }
                }
                if (newHotSubitem != null)
                {
                    if (m_HotSubItem != null)
                        m_HotSubItem.InternalMouseLeave();
                    m_HotSubItem = newHotSubitem;
                    m_HotSubItem.InternalMouseEnter();
                    m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
                }
                return true;
            }

            BaseItem item = GetItemForMnemonic(container, key, true);
            if (item!=null && item.GetEnabled() && item.Visible)
            {
                if (item is ItemContainer)
                {
                    this.ShowKeyTips = false;
                    this.HotSubItem = item;
                }
                else if (item.SubItems.Count > 0 && item.ShowSubItems && !item.Expanded)
                {
                    if (m_HotSubItem != item)
                    {
                        if (m_HotSubItem != null)
                            m_HotSubItem.InternalMouseLeave();
                        m_HotSubItem = item;
                        m_HotSubItem.InternalMouseEnter();
                        m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
                    }
                    item.Expanded = true;
                    if (item is PopupItem && ((PopupItem)item).PopupControl is MenuPanel)
                    {
                        ((MenuPanel)((PopupItem)item).PopupControl).SelectFirstItem();
                    }
                    m_IgnoreDuplicateMouseMove = true;
                }
                else
                {
                    this.ShowKeyTips = false;
                    if (item is ComboBoxItem)
                    {
                        ((ComboBoxItem)item).ComboBoxEx.Focus();
                        ((ComboBoxItem)item).ComboBoxEx.DroppedDown = true;
                    }
                    else if (item is TextBoxItem)
                    {
                        ((TextBoxItem)item).TextBox.Focus();
                    }
                    else if (item is ControlContainerItem && ((ControlContainerItem)item).Control != null)
                        ((ControlContainerItem)item).Control.Focus();
                    else
                        item.RaiseClick();
                }
                return true;
            }

            return false;
        }

        private List<BaseItem> GetItemsForMnemonic(BaseItem container, char charCode)
        {
            List<BaseItem> items = new List<BaseItem>();
            foreach (BaseItem item in container.SubItems)
            {
                if (IsMnemonic(charCode, item.Text))
                {
                    if (item.Visible && item.GetEnabled())
                    {
                        items.Add(item);
                    }
                }
            }
            return items;
        }

        protected virtual BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan)
        {
            string keyTipsString = m_KeyTipsKeysStack + charCode.ToString();
            keyTipsString = keyTipsString.ToUpper();
            bool partialMatch = false;

            foreach (BaseItem item in container.SubItems)
            {
                if (item.KeyTips != "" || m_KeyTipsKeysStack != "")
                {
                    if (item.KeyTips != "")
                    {
                        if (item.KeyTips == keyTipsString)
                        {
                            if (item.Visible && item.GetEnabled())
                            {
                                return item;
                            }
                        }
                        else if (item.KeyTips.StartsWith(keyTipsString))
                        {
                            partialMatch = true;
                        }
                    }
                }
                else if (IsMnemonic(charCode, item.Text))
                {
                    if (item.Visible && item.GetEnabled())
                    {
                        return item;
                    }
                }

                if (deepScan && item.IsContainer)
                {
                    BaseItem mItem = GetItemForMnemonic(item, charCode, deepScan);
                    if (mItem != null)
                        return mItem;
                }
                else if (deepScan && item is ControlContainerItem && ((ControlContainerItem)item).Control is RibbonBar)
                {
                    RibbonBar rb = ((ControlContainerItem)item).Control as RibbonBar;
                    BaseItem mItem = GetItemForMnemonic(rb.GetBaseItemContainer(), charCode, deepScan);
                    if (mItem != null)
                        return mItem;
                }
            }

            if (partialMatch)
            {
                m_KeyTipsKeysStack += charCode.ToString().ToUpper();
            }

            return null;
        }

		/// <summary>
		/// Return Sub Item at specified location
		/// </summary>
		protected BaseItem ItemAtLocation(int x, int y)
		{
			if(m_ParentItem!=null && m_ParentItem.SubItems.Count!=0)
			{
				foreach(BaseItem objSub in m_ParentItem.SubItems)
				{
					if((objSub.Visible || m_IsCustomizeMenu) && objSub.Displayed && x>=objSub.LeftInternal && x<=(objSub.LeftInternal+objSub.WidthInternal) && y>=objSub.TopInternal && y<=(objSub.TopInternal+objSub.HeightInternal))
					{
                        if (objSub.IsContainer)
                        {
                            BaseItem item = objSub.ItemAtLocation(x, y);
                            if (item != null)
                                return item;
                        }
						return objSub;
					}
				}
			}
			return null;
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			ClearHotSubItem();
			base.OnHandleDestroyed(e);
		}

		internal void ClearHotSubItem()
		{
			if(m_HotSubItem!=null)
			{	
				//if(m_ParentItem!=null && m_ParentItem.DesignMode && this.Owner!=null)
				//	this.Owner.SetFocusItem(null);
				//else
					m_HotSubItem.InternalMouseLeave();

				m_HotSubItem=null;
			}
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

		private void SetupScrollTimer()
		{
			m_ScrollTimer=new Timer();
			m_ScrollTimer.Interval=100;
			m_ScrollTimer.Tick+=new EventHandler(this.ScrollTimerTick);
			m_ScrollTimer.Start();
		}

		private void ScrollTimerTick(object sender, EventArgs e)
		{
			CheckScrolling();
		}

		private bool CheckScrolling()
		{
			if(!m_Scroll)
				return false;
			Point p=this.PointToClient(Control.MousePosition);
			if(m_TopScroll && p.Y<=m_TopScrollHeight+m_ClientRect.Top)
			{
				if(m_ScrollTopPosition>0)
				{
					m_ScrollTopPosition--;
					//BaseItem o=m_ParentItem.SubItems[m_ScrollTopPosition] as BaseItem;
					//if(o is SeparatorItem && m_ScrollTopPosition>0)
					//	m_ScrollTopPosition--;
						
				}
				RepositionItems();
				this.Refresh();
				if(m_ScrollTimer==null)
					SetupScrollTimer();
				return true;
			}
			else if(m_BottomScroll && p.Y>=this.Height-m_BottomScrollHeight)
			{
				m_ScrollTopPosition++;
				//BaseItem o=m_ParentItem.SubItems[m_ScrollTopPosition] as BaseItem;
				//if(o is SeparatorItem && m_ScrollTopPosition<m_ParentItem.SubItemsCount)
				//	m_ScrollTopPosition++;
				RepositionItems();
				this.Refresh();
				if(m_ScrollTimer==null)
					SetupScrollTimer();
				return true;
			}
			else
			{
				if(m_ScrollTimer!=null)
				{
					m_ScrollTimer.Stop();
					m_ScrollTimer.Dispose();
					m_ScrollTimer=null;
				}
			}
			return false;
		}

		// Reposition items when scrolling changes
		// This function will change only Y coordinate of the items
		// It will also set the Displayed property properly
		private void RepositionItems()
		{
			m_TopScroll=false;
			m_BottomScroll=false;
            if(!m_Scroll || m_ParentItem==null || m_ParentItem.SubItems.Count==0)
				return;
			int iTop=m_ClientRect.Top;
			int iItemHeight=0;
			if(m_ScrollTopPosition>0)
			{
				// There is a top scroll triangle, with inital height of 16
                m_TopScrollHeight=16;
				iTop+=m_TopScrollHeight;
				m_TopScroll=true;
			}
			BaseItem objItem;
			bool bOverrun=false;
			for(int i=0;i<m_ParentItem.SubItems.Count;i++)
			{
                objItem=m_ParentItem.SubItems[i];
				if(bOverrun || i<m_ScrollTopPosition || !objItem.Visible && !m_IsCustomizeMenu)
				{
					objItem.Displayed=false;
					continue;
				}

				iItemHeight=objItem.HeightInternal;
				if(objItem.BeginGroup)
				{
                    if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
						iItemHeight+=GROUP_SPACINGOFFICE;
					else
						iItemHeight+=GROUP_SPACINGDOTNET;
				}

                if(iItemHeight+iTop>m_ClientRect.Bottom || m_ClientRect.Bottom-(iItemHeight+iTop)<15 && i+1<m_ParentItem.SubItems.Count)
				{
					// Overflows, this item is out
					objItem.Displayed=false;
					bOverrun=true;
					
					// We will need bottom scrolling triangle
					m_BottomScroll=true;
					m_BottomScrollHeight=m_ClientRect.Bottom-iTop;
					continue;
				}

				// If this item is last item we need to change positions so last item
				// Is at the bottom of the window
				if(i==m_ParentItem.SubItems.Count-1)
				{
					// This is a last item, now we have to do more work to make this look better
					// Get the previous item so we know what our top position will be
					objItem.Displayed=true;
					iTop=m_ClientRect.Bottom;
                    for(int i1=i;i1>=m_ScrollTopPosition;i1--)
					{
						objItem=m_ParentItem.SubItems[i1];
						if(!objItem.Visible && !m_IsCustomizeMenu)
							continue;
						iTop-=objItem.HeightInternal;
						objItem.TopInternal=iTop;
						if(objItem.BeginGroup)
						{
                            if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
								iTop-=GROUP_SPACINGOFFICE;
							else
								iTop-=GROUP_SPACINGDOTNET;
						}
					}
					// See can we fit more items on top
					if(m_ScrollTopPosition>0)
					{
						objItem=m_ParentItem.SubItems[m_ScrollTopPosition-1];
						if(iTop-objItem.HeightInternal-15>m_ClientRect.Top)
						{
							// Great we can fit some more
							for(int i1=m_ScrollTopPosition-1;i1>=0;i1--)
							{
						        objItem=m_ParentItem.SubItems[i1];
								if(!objItem.Visible && !m_IsCustomizeMenu)
									continue;
								if(iTop-objItem.HeightInternal-15>m_ClientRect.Top)
								{
									iTop-=objItem.HeightInternal;
									objItem.TopInternal=iTop;
									objItem.Displayed=true;
									if(objItem.BeginGroup)
									{
                                        if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
											iTop-=GROUP_SPACINGOFFICE;
										else
											iTop-=GROUP_SPACINGDOTNET;
									}
									m_ScrollTopPosition=i1;
								}
								else
									break;
							}
						}
					}

					// Set the top scroll triangle height
					m_TopScrollHeight=iTop-m_ClientRect.Top;
					m_BottomScroll=false;
					break;
				}

				if(objItem.BeginGroup)
				{
                    if (m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
						iTop+=GROUP_SPACINGOFFICE;
					else
						iTop+=GROUP_SPACINGDOTNET;
				}

				objItem.TopInternal=iTop;
				objItem.Displayed=true;
				iTop+=objItem.HeightInternal;
			}
		}

        private bool IsRoundRegion
        {
            get
            {
                if (m_ParentItem != null)
                {
                    if (m_ParentItem is Office2007StartButton && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Windows7) return false;

                    BaseItem p = m_ParentItem;
                    while(p.Parent!=null)
                        p=p.Parent;
                    Control c = p.ContainerControl as Control;
                    if (c is RibbonStrip || c is RibbonBar || c is ContextMenuBar && BarFunctions.IsOffice2007Style(((ContextMenuBar)c).Style))
                        return true;
                }
                return false;
            }
        }

        private void SetRoundRegion(Control c)
        {
            Rectangle rectPath = c.ClientRectangle;
            rectPath.Width--;
            rectPath.Height--;
            GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(rectPath, m_CornerSize);
            Region r = new Region();
            r.MakeEmpty();
            r.Union(path);
            // Widen path for the border...
            path.Widen(SystemPens.ControlText);
            r.Union(path);
            c.Region = r;
        }

        public new void Hide()
        {
            base.Hide();

            if (m_DesignerParent && this.Parent!=null)
            {
                this.Parent.Controls.Remove(this);
                m_DesignerParent = false;
            }
        }

        private bool m_DesignerParent = false;
        private bool AddtoDesignTimeContainer()
        {
            ISite site = GetSite();
            if (site == null)
                return false;

            IDesignerHost dh = site.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh == null) return false;

            Control parent = dh.RootComponent as Control;
            while (parent != null)
            {
                parent = parent.Parent;
                if (parent!=null && parent.GetType().Name.IndexOf("DesignerFrame")>=0)
                    break;
            }
            if (parent == null || parent.Parent==null) return false;
            //parent = parent.Parent;
            Point p = parent.PointToClient(this.Location);
            parent.Controls.Add(this);
            this.Location = p;
            base.Visible = true;
            this.Update();
            this.BringToFront();
            m_DesignerParent = true;
            return true;
        }

		public new void Show()
		{
			if(!m_PopupMenu)
			{
				base.Visible=true;
				this.Update();
				return;
			}

            // Design mode add
            if (m_ParentItem != null && (m_ParentItem.Site != null && m_ParentItem.Site.DesignMode || m_ParentItem.DesignMode ||
                m_ParentItem.Parent != null && m_ParentItem.Parent.Site != null && m_ParentItem.Parent.Site.DesignMode))
            {
                if (AddtoDesignTimeContainer())
                    return;
            }

			PopupContainer popup=new PopupContainer();
			popup.ShowDropShadow=(this.DisplayShadow && this.AlphaShadow);
			
			//NativeFunctions.sndPlaySound("MenuPopup",NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
			// This window cannot be outside the screen
			// If height of the window is outside the screen, reduce the
			// window size and set the flag so we know we have to scroll the window
			ScreenInformation objScreen=BarFunctions.ScreenFromControl(this);

			if(objScreen!=null)
			{
				Rectangle workingArea=objScreen.WorkingArea;
                if (IsContextMenu || UseWholeScreenForSizeChecking)
					workingArea=objScreen.Bounds;
				if(this.Location.Y+this.Size.Height>workingArea.Bottom)
				{
					// Reduce the size
					m_ClientRect.Height=m_ClientRect.Height-(this.Location.Y+this.Size.Height-workingArea.Bottom);
					this.Height=this.Height-(this.Location.Y+this.Size.Height-workingArea.Bottom);
					m_Scroll=true;
					m_ScrollTopPosition=0;
					RepositionItems();
				}
			}
            
			popup.Location=this.Location;
			popup.Controls.Add(this);
			popup.Size=this.Size;
			this.Location=new Point(0,0);
			popup.CreateControl();

            if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && IsRoundRegion)
            {
                SetRoundRegion(this);
                SetRoundRegion(popup);
            }

			ePopupAnimation animation=m_PopupAnimation;
			if(!BarFunctions.SupportsAnimation)
				animation=ePopupAnimation.None;
			else
			{
				if(animation==ePopupAnimation.ManagerControlled)
				{
					IOwnerMenuSupport ownermenu=m_Owner as IOwnerMenuSupport;
					if(ownermenu!=null)
						animation=ownermenu.PopupAnimation;
					if(animation==ePopupAnimation.ManagerControlled)
						animation=ePopupAnimation.SystemDefault;
				}

				if(animation==ePopupAnimation.SystemDefault)
					animation=NativeFunctions.SystemMenuAnimation;
				else if(animation==ePopupAnimation.Random)
				{
					Random r=new System.Random();
					int i=r.Next(2);
					animation=ePopupAnimation.Fade;
					if(i==1)
						animation=ePopupAnimation.Slide;
					else if(i==2)
						animation=ePopupAnimation.Unfold;
				}
			}

			// No animation if menu is hosting controls... Animation caused strange errors on some machines
			if(this.Controls.Count>0)
				animation=ePopupAnimation.None;

			if(animation==ePopupAnimation.Fade && Environment.OSVersion.Version.Major>=5)
			{
				NativeFunctions.AnimateWindow(popup.Handle.ToInt32(),BarFunctions.ANIMATION_INTERVAL,NativeFunctions.AW_BLEND);
                popup.Visible = true;
			}
			else if(animation==ePopupAnimation.Slide)
			{
				NativeFunctions.AnimateWindow(popup.Handle.ToInt32(),BarFunctions.ANIMATION_INTERVAL,(NativeFunctions.AW_SLIDE | NativeFunctions.AW_HOR_POSITIVE | NativeFunctions.AW_VER_POSITIVE));
                popup.Visible = true;
			}
            else if (animation == ePopupAnimation.Unfold)
            {
                NativeFunctions.AnimateWindow(popup.Handle.ToInt32(), BarFunctions.ANIMATION_INTERVAL, (NativeFunctions.AW_HOR_POSITIVE | NativeFunctions.AW_VER_POSITIVE));
                popup.Visible = true;
            }
            else
            {
                base.Visible = true;
                popup.Visible = true;
            }
			popup.ShowShadow();

			if(animation!=ePopupAnimation.None && this.Controls.Count>0)
				this.Refresh();

			Rectangle rectDisplay=new Rectangle(popup.Location,popup.Size);
			if(rectDisplay.Contains(Control.MousePosition))
			{
				m_IgnoreDuplicateMouseMove=true;
			}

			// This makes the menu paint BEFORE it returns out of this function
			this.Update();

			MenuPanel.PopupMenuAccessibleObject acc=this.AccessibilityObject as PopupMenuAccessibleObject;
			if(acc!=null)
			{
				if(m_ParentItem!=null && m_ParentItem.IsOnMenuBar)
					acc.GenerateEvent(AccessibleEvents.SystemMenuStart);
				acc.GenerateEvent(AccessibleEvents.SystemMenuPopupStart);
			}
		}

		private bool IsContextMenu
		{
			get
			{
				if(m_ParentItem==null)
					return false;
				if(m_ParentItem.Parent==null || m_ParentItem.ContainerControl is ContextMenuBar)
					return true;
				BaseItem topParent=m_ParentItem.Parent;
				while(topParent.Parent!=null)
					topParent=topParent.Parent;
				if(topParent is PopupItem)
					return true;
				return false;
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
			if(!this.Visible && this.Parent!=null && this.Parent is PopupContainer)
			{
				this.Parent.Visible=false;
			}

			if(!this.Visible && m_ScrollTimer!=null)
			{
				m_ScrollTimer.Stop();
				m_ScrollTimer.Dispose();
				m_ScrollTimer=null;
			}

			if(m_AccessibleObjectCreated)
			{
				MenuPanel.PopupMenuAccessibleObject acc=this.AccessibilityObject as PopupMenuAccessibleObject;
				if(acc!=null)
				{
					if(m_ParentItem!=null)
					{
						foreach(BaseItem item in m_ParentItem.SubItems)
							acc.GenerateEvent(item,AccessibleEvents.Destroy);
					}
					if(!this.Visible)
					{
						acc.GenerateEvent(AccessibleEvents.SystemMenuPopupEnd);
						acc.GenerateEvent(AccessibleEvents.StateChange);
					}
				}
			}
		}

		private int ClientMarginLeft
		{
			get
			{
				int iMargin=0;

                if (m_ParentItem != null && IsGradientStyle)
                {
                    iMargin = 1;
                    if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                        iMargin++;
                    if (IsContainerMenu)
                        iMargin--;
                }
                else
                    iMargin = 3;

				return iMargin;
			}
		}

		private int ClientMarginTop
		{
			get
			{
				int iMargin=0;
				if(m_ParentItem!=null && IsGradientStyle)
					iMargin=2;
				else
					iMargin=3;
                
                if (IsContainerMenu)
                    iMargin--;

				return iMargin;
			}
		}

		private int ClientMarginRight
		{
			get
			{
				bool bShowShadow=true;
				int iMargin=0;
				IOwnerMenuSupport ownermenu=m_Owner as IOwnerMenuSupport;
				if(ownermenu!=null && !ownermenu.ShowPopupShadow)
					bShowShadow=false;
				if(m_ParentItem!=null && IsGradientStyle)
				{
					if(this.AlphaShadow || !bShowShadow)
						iMargin=1;
					else
						iMargin=3;
                    if (BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                        iMargin++;

                    if (IsContainerMenu)
                        iMargin--;
				}
				else
					iMargin=3;

				return iMargin;
			}
		}

		private int ClientMarginBottom
		{
			get
			{
				IOwnerMenuSupport ownermenu=m_Owner as IOwnerMenuSupport;
				bool bShowShadow=true;
				int iMargin=0;
				if(ownermenu!=null && !ownermenu.ShowPopupShadow)
					bShowShadow=false;
				if(m_ParentItem!=null && IsGradientStyle)
				{
					if(this.AlphaShadow || !bShowShadow)
						iMargin=2;
					else
						iMargin=4;
                    if (IsContainerMenu)
                        iMargin--;
				}
				else
					iMargin=3;

				return iMargin;
			}
		}

		internal bool DisplayShadow
		{
			get
			{
				if(!m_PopupMenu || m_ParentItem!=null && m_ParentItem.Site!=null && m_ParentItem.Site.DesignMode)
					return false;
				IOwnerMenuSupport ownermenu=m_Owner as IOwnerMenuSupport;
				if(ownermenu!=null)
				{
                    if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						if(ownermenu.MenuDropShadow==eMenuDropShadow.Show)
							return true;
						else
							return false;
					}
					return ownermenu.ShowPopupShadow;
				}
				else
				{
                    if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
						return false;
				}

				return true;
			}
		}

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set {m_AntiAlias = value; }
        }

		internal bool AlphaShadow
		{
			get
			{
				if(Environment.OSVersion.Version.Major<5)
					return false;
				IOwnerMenuSupport ownermenu=m_Owner as IOwnerMenuSupport;
				if(ownermenu!=null && !ownermenu.AlphaBlendShadow)
					return false;
				return true; //NativeFunctions.CursorShadow;
			}
		}

        

		/// <summary>
		/// Indicates whether Tooltips are shown on Bars and menus.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),DefaultValue(true),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Indicates whether Tooltips are shown on Bar and it's sub-items.")]
		public bool ShowToolTips
		{
			get
			{
				return m_ShowToolTips;
			}
			set
			{
				m_ShowToolTips=value;
			}
		}

//		protected override void OnLocationChanged(EventArgs e)
//		{
//			if(m_DropShadow!=null)
//				NativeFunctions.SetWindowPos(m_DropShadow.Handle.ToInt32(),NativeFunctions.HWND_NOTOPMOST,this.Left+5,this.Top+5,this.Width-2,this.Height-2,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
//			base.OnLocationChanged(e);
//		}

		/// <summary>
		/// Sets,Gets the side bar image structure.
		/// </summary>
		public SideBarImage SideBar
		{
			get
			{
				return m_SideBarImage;
			}
			set
			{
				m_SideBarImage=value;
			}
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

		internal bool IsCustomizeMenu
		{
			get
			{
				return m_IsCustomizeMenu;
			}
			set
			{
				m_IsCustomizeMenu=value;
				if(m_ParentItem!=null)
				{
					foreach(BaseItem objItem in m_ParentItem.SubItems)
						objItem.SetIsOnCustomizeMenu(m_IsCustomizeMenu);
				}
			}
		}

		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
		public ePersonalizedMenus PersonalizedMenus
		{
			get
			{
				return m_PersonalizedMenus;
			}
			set
			{
				m_PersonalizedMenus=value;
			}
		}

		public bool PersonalizedAllVisible
		{
			get
			{
				return m_ExpandButton.PersonalizedAllVisible;
			}
			set
			{
				m_ExpandButton.PersonalizedAllVisible=value;
			}
		}

		public ePopupAnimation PopupAnimation
		{
			get
			{
				return m_PopupAnimation;
			}
			set
			{
				m_PopupAnimation=value;
			}
		}

		public DevComponents.DotNetBar.ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				m_ColorScheme=value;
				if(this.Visible)
					this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the HotSubItem on the menu. This method is used internally by DotNetBar and should not be used in your application.
		/// </summary>
		public BaseItem HotSubItem
		{
			get {return m_HotSubItem;}
			set
			{
				if(m_HotSubItem!=null)
					m_HotSubItem.InternalMouseLeave();
				m_HotSubItem=null;
				if(this.ParentItem.SubItems.Contains(value) || IsChildItem(value))
				{
					m_HotSubItem=value;
					if(m_HotSubItem!=null)
					{
						m_HotSubItem.InternalMouseEnter();
						m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None,0,m_HotSubItem.LeftInternal+1,m_HotSubItem.TopInternal+1,0));
					}
				}
			}
		}

        private bool IsChildItem(BaseItem item)
        {
            if (item == null) return false;
            while (item != null)
            {
                if (this.ParentItem == item) return true;
                item = item.Parent;
            }
            return false;
        }

		//***********************************************
		// IDesignTimeProvider Implementation
		//***********************************************
		public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
		{
			if(m_ParentItem==null)
				return null;

            return DesignTimeProviderContainer.GetInsertPosition(m_ParentItem, pScreen, DragItem);

            //InsertPosition objInsertPos=null;
            //Point pClient=this.PointToClient(pScreen);
            //Rectangle r;
            //if(this.ClientRectangle.Contains(pClient))
            //{
            //    for(int i=0;i<m_ParentItem.SubItems.Count;i++)
            //    {
            //        BaseItem objItem=m_ParentItem.SubItems[i];
            //        r=objItem.DisplayRectangle;
            //        r.Inflate(2,2);
            //        if(objItem.Visible && r.Contains(pClient))
            //        {
            //            if(objItem.SystemItem)
            //                return null;
            //            if(objItem==DragItem)
            //                return new InsertPosition();

            //            if (objItem.IsContainer && objItem is IDesignTimeProvider)
            //            {
            //                Rectangle inner = r;
            //                inner.Inflate(-5, -5);
            //                if (inner.Contains(pClient))
            //                    return ((IDesignTimeProvider)objItem).GetInsertPosition(pScreen, DragItem);
            //            }

            //            objInsertPos=new InsertPosition();
            //            objInsertPos.TargetProvider=(IDesignTimeProvider)m_ParentItem;
            //            objInsertPos.Position=i;
            //            //if(objItem.Orientation==eOrientation.Horizontal)
            //            //{
            //            //	if(pClient.X<=objItem.Left+objItem.Width/2)
            //            //		objInsertPos.Before=true;
            //            //}
            //            //else
            //            //{
            //                if(pClient.Y<=objItem.TopInternal+objItem.HeightInternal/2)
            //                    objInsertPos.Before=true;
            //            //}
            //            if(objItem is PopupItem && objItem.SubItems.Count>0)
            //            {
            //                if(!objItem.Expanded)
            //                    objItem.Expanded=true;
            //            }
            //            else
            //                BaseItem.CollapseSubItems(m_ParentItem);

            //            break;
            //        }
            //    }
            //    if(objInsertPos==null)
            //    {
            //        // Container is empty but it can contain the items
            //        //objInsertPos=new InsertPosition(m_ParentItem.SubItemsCount-1,false,this);
            //        if(m_ParentItem.SubItems.Count>1 && m_ParentItem.SubItems[m_ParentItem.SubItems.Count-1].SystemItem)
            //            objInsertPos=new InsertPosition(m_ParentItem.SubItems.Count-2,true,(IDesignTimeProvider)m_ParentItem);
            //        else
            //            objInsertPos=new InsertPosition(m_ParentItem.SubItems.Count-1,false,(IDesignTimeProvider)m_ParentItem);
            //    }
            //}
            //else
            //{
            //    foreach(BaseItem objItem in m_ParentItem.SubItems)
            //    {
            //        if(objItem==DragItem)
            //            continue;
            //        IDesignTimeProvider provider=objItem as IDesignTimeProvider;
            //        if(provider!=null)
            //        {
            //            objInsertPos=provider.GetInsertPosition(pScreen, DragItem);
            //            if(objInsertPos!=null)
            //                break;
            //        }
            //    }
            //}
            //return objInsertPos;
		}

		public void DrawReversibleMarker(int iPos, bool Before)
		{
			Rectangle r, rl,rr;

			if(iPos>=0)
			{
				BaseItem objItem=m_ParentItem.SubItems[iPos];
				if(objItem.DesignInsertMarker!=eDesignInsertPosition.None)
					objItem.DesignInsertMarker=eDesignInsertPosition.None;
				else if(Before)
					objItem.DesignInsertMarker=eDesignInsertPosition.Before;
				else
					objItem.DesignInsertMarker=eDesignInsertPosition.After;
				return;
			}
			else
			{
				r=new Rectangle(this.ClientRectangle.Left+2,this.ClientRectangle.Top+2,this.ClientRectangle.Width-4,1);
				rl=new Rectangle(this.ClientRectangle.Left+1,this.ClientRectangle.Top,1,5);
				rr=new Rectangle(this.ClientRectangle.Right-2,this.ClientRectangle.Top,1,5);
			}

            //r.Location=this.PointToScreen(r.Location);
            //rl.Location=this.PointToScreen(rl.Location);
            //rr.Location=this.PointToScreen(rr.Location);
            //ControlPaint.DrawReversibleFrame(r,SystemColors.Control,FrameStyle.Thick);
            //ControlPaint.DrawReversibleFrame(rl,SystemColors.Control,FrameStyle.Thick);
            //ControlPaint.DrawReversibleFrame(rr,SystemColors.Control,FrameStyle.Thick);
		}
		public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
		{
			if(!Before)
			{
				//objItem.BeginGroup=true; //!objItem.BeginGroup;
				if(iPos+1>=m_ParentItem.SubItems.Count)
					m_ParentItem.SubItems.Add(objItem);
				else
					m_ParentItem.SubItems.Add(objItem,iPos+1);
			}
			else
			{
				//objItem.BeginGroup=false;
				if(iPos>=m_ParentItem.SubItems.Count)
					m_ParentItem.SubItems.Add(objItem);
				else
					m_ParentItem.SubItems.Add(objItem,iPos);
			}
			objItem.ContainerControl=this;
			ClearHotSubItem();
			this.RecalcSize();

            ISite site = GetSite();
            if (site != null && m_ParentItem != null)
            {
                IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (cc != null)
                    cc.OnComponentChanged(m_ParentItem, TypeDescriptor.GetProperties(m_ParentItem)["SubItems"], null, null);
            }
		}

		public class PopupMenuAccessibleObject : Control.ControlAccessibleObject
		{
			MenuPanel m_Owner = null;
			public PopupMenuAccessibleObject(MenuPanel owner):base(owner)
			{
				m_Owner = owner;
			}

			internal void GenerateEvent(BaseItem sender, System.Windows.Forms.AccessibleEvents e)
			{
				if(m_Owner==null)
					return;
				if(m_Owner!=null && m_Owner.m_ParentItem!=null)
				{
					int	iChild = m_Owner.m_ParentItem.SubItems.IndexOf(sender);
					if(iChild>=0)
					{
						m_Owner.AccessibilityNotifyClients(e,iChild);
					}
				}
			}

			internal void GenerateEvent(System.Windows.Forms.AccessibleEvents e)
			{
				if(m_Owner==null)
					return;
				m_Owner.AccessibilityNotifyClients(e,-1);
            }

            //public override string Name 
            //{
            //    get
            //    {
            //        if(m_Owner==null)
            //            return "";
            //        return m_Owner.AccessibleName;
            //    }
            //    set
            //    {
            //        if(m_Owner==null)
            //            return;
            //        m_Owner.AccessibleName = value;
            //    }
            //}

            //public override string Description
            //{
            //    get
            //    {
            //        if(m_Owner==null)
            //            return "";
            //        return m_Owner.AccessibleDescription;
            //    }
            //}

			public override AccessibleRole Role
			{
				get
				{
					if(m_Owner==null)
						return AccessibleRole.None;
					return m_Owner.AccessibleRole;
				}
			}

			public override AccessibleObject Parent 
			{
				get
				{
                    if (m_Owner == null || m_Owner.ParentItem == null)
                        return null;
					return m_Owner.ParentItem.AccessibleObject;
				}
			}

			public override Rectangle Bounds 
			{
				get
				{
					if(m_Owner==null)
						return Rectangle.Empty;
					return this.m_Owner.DisplayRectangle;
				}
			}

			public override int GetChildCount()
			{
				if(m_Owner!=null && m_Owner.m_ParentItem!=null)
					return this.m_Owner.m_ParentItem.SubItems.Count;
				return 0;
			}

			public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
			{
				if(m_Owner!=null && m_Owner.m_ParentItem!=null)
					return this.m_Owner.m_ParentItem.SubItems[iIndex].AccessibleObject;
				return null;
			}

			public override AccessibleStates State
			{
				get
				{
					if(m_Owner==null || m_Owner.IsDisposed)
						return AccessibleStates.Unavailable;

					return AccessibleStates.Floating;
				}
			}

			public override void Select(AccessibleSelection flags){}
			public override void DoDefaultAction() {}

			//			public virtual string DefaultAction {get;}
			//			public virtual AccessibleStates State {get;}
			//			public virtual void DoDefaultAction();
			//			public virtual bool Equals(object obj);
			//			public static bool Equals(object objA,object objB);
			//			public virtual AccessibleObject GetFocused();
			//			public virtual AccessibleObject GetSelected();
			//			public virtual AccessibleObject HitTest(int xScreen,int yScreen);
			//			public virtual AccessibleObject Navigate(AccessibleNavigation navdir);
		}
    }
}
