using System;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Container control for Navigation Bar.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false)]
	public class NavigationBarContainer:ImageItem
	{
		private ButtonItem m_ConfigureItem=null;
		private int m_ItemPaddingTop=2, m_ItemPaddingBottom=2;
		private int m_BottomLineItemStartIndex=-1;

		private bool m_ConfigureItemVisible=true;
		private bool m_ConfigureShowHideVisible=true;
		private bool m_ConfigureNavOptionsVisible=true;
		private bool m_ConfigureAddRemoveVisible=true;
		
		private bool m_AutoSizeButtonImage=true;
		private Size m_ImageSizeSummaryLine=new Size(16,16);

		private const int DEFAULT_HEIGHT=32;
		private bool m_SummaryLineVisible=true;

		/// <summary>
		/// Creates new instance of NavigationBarContainer.
		/// </summary>
		public NavigationBarContainer():this("","") {}
		/// <summary>
		/// Creates new instance of SideBarPanelItem and assigns the name to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public NavigationBarContainer(string sItemName):this(sItemName,"") {}
		/// <summary>
		/// Creates new instance of SideBarPanelItem and assigns the name and text to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		/// <param name="ItemText">item text.</param>
		public NavigationBarContainer(string sItemName, string ItemText):base(sItemName,ItemText)
		{
			m_IsContainer=true;
			this.SubItemsImageSize=Size.Empty;
		}

		/// <summary>
		/// Returns copy of SideBarPanelItem item.
		/// </summary>
		public override BaseItem Copy()
		{
			NavigationBarContainer objCopy=new NavigationBarContainer();
			this.CopyToItem(objCopy);
			return objCopy;
		}

		/// <summary>
		/// Recalculates size of this container.
		/// </summary>
		public override void RecalcSize()
		{
			m_RecalculatingSize=true;

			// Maximum height of item will be used as a height of bottom line of items
			int iMaxHeight=0;

			int itemSpacing=1;
			int containerBorder=0;
			BaseItem lastVisible=null;
			
			CreateConfigureButton(m_Rect.Bottom,DEFAULT_HEIGHT);

			foreach(BaseItem item in this.SubItems)
			{
				if(item is ButtonItem)
				{
					((ButtonItem)item).ButtonStyle=eButtonStyle.ImageAndText;
					((ButtonItem)item).ImageFixedSize=Size.Empty;
				}

				if(m_AutoSizeButtonImage && !m_ImageSizeSummaryLine.IsEmpty)
					((ButtonItem)item).ImageFixedSize=m_ImageSizeSummaryLine;

				item.RecalcSize();
				item.HeightInternal+=(m_ItemPaddingTop+m_ItemPaddingBottom);

				if(item.Visible)
				{
					lastVisible=item;
					if(item.HeightInternal>iMaxHeight)
						iMaxHeight=item.HeightInternal;
				}

				if(m_AutoSizeButtonImage && !m_ImageSizeSummaryLine.IsEmpty)
				{
					((ButtonItem)item).ImageFixedSize=Size.Empty;
					item.RecalcSize();
					item.HeightInternal+=(m_ItemPaddingTop+m_ItemPaddingBottom);
				}
			}

			// Now arrange them
			int y=m_Rect.Top, x=m_Rect.Left+containerBorder;
			int heightActual=0;
			int itemWidth=this.WidthInternal-(containerBorder*2);
			bool bFirstInLine=false;
			bool bOverFlow=false;
			int iFirstInLine=-1;
			foreach(BaseItem item in this.SubItems)
			{
				if(item.Visible)
				{
					if(y+item.HeightInternal+itemSpacing<=m_Rect.Bottom-(m_SummaryLineVisible?iMaxHeight:0)) //((!bFirstInLine && lastVisible==item)?0:iMaxHeight))
					{
						// Items on top of each other
						item.TopInternal=y;
						item.LeftInternal=x;
						item.WidthInternal=itemWidth;
						item.Displayed=true;
						y+=(item.HeightInternal+itemSpacing);
						heightActual+=(item.HeightInternal+itemSpacing);
					}
					else
					{
						if(m_SummaryLineVisible)
						{
							// Items next to each other
							if(!bFirstInLine)
							{
								heightActual+=(iMaxHeight);
								bFirstInLine=true;
								iFirstInLine=this.SubItems.IndexOf(item);
							}
							if(item is ButtonItem)
							{
								((ButtonItem)item).ButtonStyle=eButtonStyle.Default;
								if(m_AutoSizeButtonImage && !m_ImageSizeSummaryLine.IsEmpty)
									((ButtonItem)item).ImageFixedSize=m_ImageSizeSummaryLine;

								item.RecalcSize();
								item.HeightInternal+=(m_ItemPaddingTop+m_ItemPaddingBottom);
								
								if(bOverFlow || x+item.WidthInternal>m_Rect.Right-GetConfigureItemWidth())
								{
									bOverFlow=true;
									item.Displayed=false;
								}
								else
								{
									item.TopInternal=y;
									item.LeftInternal=x;
									item.HeightInternal=iMaxHeight;
									x+=item.WidthInternal;
									item.Displayed=true;
								}
							}
						}
						else
							item.Displayed=false;
					}
				}
				else
					item.Displayed=false;
			}

			if(iMaxHeight==0)
				iMaxHeight=DEFAULT_HEIGHT;

			CreateConfigureButton(y,iMaxHeight);

			if(!bFirstInLine && m_SummaryLineVisible)
				heightActual+=(iMaxHeight);

			// Right align the line items
			if(iFirstInLine>=0)
			{
				x=m_Rect.Right-containerBorder;
				if(m_ConfigureItem!=null)
					x-=m_ConfigureItem.WidthInternal;
				for(int i=this.SubItems.Count-1;i>=iFirstInLine;i--)
				{
					if(this.SubItems[i].Displayed)
					{
						x-=this.SubItems[i].WidthInternal;
						this.SubItems[i].LeftInternal=x;
					}
				}
			}
			m_BottomLineItemStartIndex=iFirstInLine;

			if(heightActual==0)
				heightActual=DEFAULT_HEIGHT;

			m_Rect.Height=heightActual;

			m_RecalculatingSize=false;
		}

		private void PaintLineBackground(ItemPaintArgs pa,Rectangle r, eDotNetBarStyle style)
		{
            if (BarFunctions.IsOffice2007Style(style) && pa.Renderer != null)
            {
                pa.Renderer.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(pa.Graphics, r));
            }
            else
            {
                // Paint background of bottom line
                if (!pa.Colors.ItemBackground.IsEmpty || !pa.Colors.ItemBackground2.IsEmpty)
                    DisplayHelp.FillRectangle(pa.Graphics, r, pa.Colors.ItemBackground, pa.Colors.ItemBackground2, pa.Colors.ItemBackgroundGradientAngle);
                else if (!pa.Colors.BarBackground.IsEmpty || !pa.Colors.BarBackground2.IsEmpty)
                    DisplayHelp.FillRectangle(pa.Graphics, r, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);
            }
			using(System.Drawing.Pen pen=new System.Drawing.Pen(pa.Colors.PanelBorder,1))
			{
				pa.Graphics.DrawLine(pen,r.Left,r.Top-1,r.Right,r.Top-1);
			}
		}

		public override void Paint(ItemPaintArgs pa)
		{
			if(m_Rect.Width==0 || m_Rect.Height==0)
				return;

			if(m_ConfigureItem!=null)
			{
				Rectangle rBottomLine=new Rectangle(m_Rect.X,m_ConfigureItem.TopInternal,m_Rect.Width,m_ConfigureItem.HeightInternal);
                PaintLineBackground(pa, rBottomLine, this.EffectiveStyle);
			}
			else if(m_BottomLineItemStartIndex>=0)
			{
				Rectangle rBottomLine=new Rectangle(m_Rect.X,this.SubItems[m_BottomLineItemStartIndex].TopInternal,m_Rect.Width,this.SubItems[m_BottomLineItemStartIndex].HeightInternal);
                PaintLineBackground(pa, rBottomLine, this.EffectiveStyle);
			}
            else if (m_SummaryLineVisible)
            {
                int top = -1;
                for (int i = this.SubItems.Count - 1; i >= 0; i--)
                {
                    BaseItem item = this.SubItems[i];
                    if (item.Visible && item.Displayed)
                    {
                        top = item.DisplayRectangle.Bottom;
                        break;
                    }
                }

                if (top >= 0)
                {
                    Rectangle rBottomLine = new Rectangle(m_Rect.X, top, m_Rect.Width, m_Rect.Bottom - top);
                    PaintLineBackground(pa, rBottomLine, this.EffectiveStyle);
                    DisplayHelp.DrawLine(pa.Graphics, m_Rect.X, top, m_Rect.Right, top, pa.Colors.PanelBorder, 1);
                }
            }

            bool paintItemBackground = (pa.Colors.ItemBackground.IsEmpty && pa.Colors.ItemBackground2.IsEmpty);
			for(int i=0;i<this.SubItems.Count;i++)
			{
				BaseItem item=this.SubItems[i];

				if(item.Visible && item.Displayed)
				{
                    if (paintItemBackground)
                    {
                        if (BarFunctions.IsOffice2007Style(this.EffectiveStyle) && pa.Renderer != null)
                            pa.Renderer.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(pa.Graphics, item.DisplayRectangle));
                        else
                            DisplayHelp.FillRectangle(pa.Graphics, item.DisplayRectangle, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);
                    }
					item.Paint(pa);
					if(i>0)
					{
						using(System.Drawing.Pen pen=new System.Drawing.Pen(pa.Colors.PanelBorder,1))
						{
							pa.Graphics.DrawLine(pen,m_Rect.Left,item.TopInternal-1,m_Rect.Right,item.TopInternal-1);
						}
					}
				}
			}
			
			if(m_ConfigureItem!=null)
			{
				m_ConfigureItem.Paint(pa);
			}
		}

		#region Configure Button Implementation
        internal void OptionsDialogClosed()
        {
            NavigationBar bar = this.ContainerControl as NavigationBar;
            if (bar != null) bar.InvokeOptionsDialogClosed();
        }
        private void CreateConfigureButton(int y, int height)
		{
			if(!m_ConfigureItemVisible)
				return;

			if(m_ConfigureItem==null)
			{
				LocalizationManager lm=new LocalizationManager(this.GetOwner() as IOwnerLocalize);

				m_ConfigureItem=new ButtonItem();
				m_ConfigureItem.Image=new System.Drawing.Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"SystemImages.NavBarConfigure.png");
                m_ConfigureItem.ImageFixedSize = m_ConfigureItem.Image.Size;
				m_ConfigureItem.Style=m_Style;
				m_ConfigureItem.ShowSubItems=false;
				m_ConfigureItem.SetSystemItem(true);
				// Add Sub-Items
				ButtonItem item;
				if(m_ConfigureShowHideVisible)
				{
					item=new ButtonItem("sysShowMoreButtons");
					item.Image=new System.Drawing.Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"SystemImages.NavBarShowMore.png");
					item.Text=lm.GetDefaultLocalizedString(LocalizationKeys.NavBarShowMoreButtons);
					item.SetSystemItem(true);
					item.Click+=new EventHandler(this.OnShowMoreButtonsClick);
					m_ConfigureItem.SubItems.Add(item);

					item=new ButtonItem("sysShowFewerButtons");
					item.Image=new System.Drawing.Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"SystemImages.NavBarShowLess.png");
					item.Text=lm.GetDefaultLocalizedString(LocalizationKeys.NavBarShowFewerButtons);
					item.SetSystemItem(true);
					item.Click+=new EventHandler(this.OnShowFewerButtonsClick);
					m_ConfigureItem.SubItems.Add(item);
				}

				if(m_ConfigureNavOptionsVisible)
				{
					item=new ButtonItem("sysNavPaneOptions");
					item.Text=lm.GetDefaultLocalizedString(LocalizationKeys.NavBarOptions);
					item.Click+=new EventHandler(this.OnNavPaneOptionsClick);
					item.SetSystemItem(true);
					m_ConfigureItem.SubItems.Add(item);
				}

				if(m_ConfigureAddRemoveVisible)
				{
					CustomizeItem customize=new CustomizeItem();
					customize.Name="sysNavPaneAddRemove";
					customize.CustomizeItemVisible=false;
					m_ConfigureItem.SubItems.Add(customize);
				}

				m_ConfigureItem.SetParent(this);
				m_ConfigureItem.Click+=new EventHandler(this.ConfigureItemClick);
				m_ConfigureItem.ExpandChange+=new EventHandler(this.ConfigureExpandedChanged);
				m_ConfigureItem.PopupShowing+=new EventHandler(this.ConfigurePopupShowing);
				m_ConfigureItem.PopupSide=ePopupSide.Right;

				lm.Dispose();

				BarBaseControl ctrl=this.ContainerControl as BarBaseControl;
				if(ctrl!=null && ctrl.Font!=null)
					m_ConfigureItem.PopupFont=new Font(ctrl.Font,FontStyle.Regular);
				else
					m_ConfigureItem.PopupFont=System.Windows.Forms.SystemInformation.MenuFont;
			}
			m_ConfigureItem.PopupType=ePopupType.Menu;
			m_ConfigureItem.Displayed=true;
            m_ConfigureItem.HeightInternal = height;
			m_ConfigureItem.RecalcSize();
			m_ConfigureItem.LeftInternal=m_Rect.Right-m_ConfigureItem.WidthInternal;
			m_ConfigureItem.TopInternal=y;
			m_ConfigureItem.HeightInternal=height;
		}

		private int GetConfigureItemWidth()
		{
			if(m_ConfigureItem!=null && m_ConfigureItemVisible)
				return m_ConfigureItem.WidthInternal;
			return 0;
		}

		private void ConfigureItemClick(object sender, EventArgs e)
		{
			m_ConfigureItem.Expanded=!m_ConfigureItem.Expanded;
		}

		private void ConfigureExpandedChanged(object sender, EventArgs e)
		{
			if(m_ConfigureItem.Expanded)
			{
				// Enable Disable buttons
				if(m_ConfigureShowHideVisible)
				{
					m_ConfigureItem.SubItems["sysShowMoreButtons"].Enabled=(m_BottomLineItemStartIndex>=0);
					m_ConfigureItem.SubItems["sysShowFewerButtons"].Enabled=(m_BottomLineItemStartIndex!=0);
				}
				
				// Add overflow items
				bool bFirst=true;
				foreach(BaseItem item in this.SubItems)
				{
					if(item.Visible && !item.Displayed)
					{
						BaseItem copy=item.Clone() as BaseItem;
						if(bFirst)
						{
							copy.GlobalItem=false;
							copy.BeginGroup=true;
							//copy.GlobalItem=true;
                            copy.Click += new EventHandler(ButtonCopyClick);
							bFirst=false;
						}
						else
						{
							copy.GlobalItem=false;
							copy.BeginGroup=false;
							//copy.GlobalItem=true;
                            copy.Click += new EventHandler(ButtonCopyClick);
						}
                        copy.Tag = item;
                        if (copy is ButtonItem)
                        {
                            ((ButtonItem)copy).ImageFixedSize = new Size(16, 16);
                        }
						m_ConfigureItem.SubItems.Insert(m_ConfigureItem.SubItems.Count-1,copy);
					}
				}

				// Set popup location
				System.Drawing.Size popupSize=m_ConfigureItem.PopupSize;
                Point popupLocation = new System.Drawing.Point(m_ConfigureItem.DisplayRectangle.Right, m_ConfigureItem.DisplayRectangle.Top + (m_ConfigureItem.DisplayRectangle.Height / 2) - popupSize.Height);
                System.Windows.Forms.Control c = this.ContainerControl as System.Windows.Forms.Control;
                if (c != null)
                {
                    Point ps = c.PointToScreen(popupLocation);
                    ScreenInformation screen = BarFunctions.ScreenFromControl(c);
                    if (ps.Y < screen.WorkingArea.Top)
                    {
                        popupLocation.Y = c.PointToClient(screen.WorkingArea.Location).Y;
                    }
                }
                m_ConfigureItem.PopupLocation = popupLocation;
			}
			else
			{
				int height=m_ConfigureItem.HeightInternal;
				// Remove any cloned items added
				System.Collections.ArrayList list=new System.Collections.ArrayList();
				foreach(BaseItem item in m_ConfigureItem.SubItems)
				{
					if(!item.SystemItem)
						list.Add(item);
				}
                m_ConfigureItem.SuspendLayout = true;
				foreach(BaseItem item in list)
					m_ConfigureItem.SubItems.Remove(item);
                m_ConfigureItem.SuspendLayout = false;
				m_ConfigureItem.RecalcSize();
				m_ConfigureItem.HeightInternal=height;
				//m_ConfigureItem.Refresh();
			}
		}

        private void ButtonCopyClick(object sender, EventArgs e)
        {
            ButtonItem b = sender as ButtonItem;
            if (b != null)
            {
                ButtonItem l = b.Tag as ButtonItem;
                if (l != null)
                {
                    l.Checked = true;
                    b.Checked = true;
                }
            }
        }

		private void OnShowMoreButtonsClick(object sender, EventArgs e)
		{
			this.ShowMoreButtons();
			BaseItem.CollapseAll(sender as BaseItem);
		}

		/// <summary>
		/// Increases the size of the navigation bar if possible by showing more buttons on the top.
		/// </summary>
		public void ShowMoreButtons()
		{
			// Need to increase the height of host control if possible
			System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
			if(ctrl!=null)
				ctrl.Height+=GetChangeSizeHeight(false)+1;
		}

		internal int GetChangeSizeHeight(bool reduceSize)
		{
			int height=0;
			BaseItem referenceItem=GetResizeReferenceItem(reduceSize);
			if(referenceItem==null)
				return height;
			height=referenceItem.HeightInternal;

			if(referenceItem is ButtonItem && AutoSizeButtonImage && !ImageSizeSummaryLine.IsEmpty && !((ButtonItem)referenceItem).ImageFixedSize.IsEmpty)
			{
				Size size=((ButtonItem)referenceItem).ImageFixedSize;
				((ButtonItem)referenceItem).ImageFixedSize=Size.Empty;
				referenceItem.RecalcSize();
				height=referenceItem.HeightInternal;
				height+=(ItemPaddingBottom+ItemPaddingTop);
				((ButtonItem)referenceItem).ImageFixedSize=size;
				referenceItem.RecalcSize();
			}
			return height;
		}

		private void OnShowFewerButtonsClick(object sender, EventArgs e)
		{
			this.ShowFewerButtons();
			BaseItem.CollapseAll(sender as BaseItem);
		}

		internal BaseItem GetResizeReferenceItem(bool reduceSize)
		{
			if(reduceSize && this.SummaryLineStartItemIndex>1)
				return this.SubItems[this.SummaryLineStartItemIndex-1];
			if(this.SummaryLineStartItemIndex>=0)
				return this.SubItems[this.SummaryLineStartItemIndex];
			foreach(BaseItem item in this.SubItems)
			{
				if(item.Visible && item.Displayed)
					return item;
			}
			return null;
		}

		internal BaseItem GetFirstVisibleItem()
		{
			foreach(BaseItem item in this.SubItems)
			{
				if(item.Visible && item.Displayed)
					return item;
			}
			return null;
		}

		/// <summary>
		/// Returns index of the first item that is displayed in summary line or -1 if there is no item displayed in summary line.
		/// </summary>
		[Browsable(false)]
		public int SummaryLineStartItemIndex
		{
			get { return m_BottomLineItemStartIndex; }
		}

		/// <summary>
		/// Reduces the size of the navigation bar if possible by showing fewer buttons on the top.
		/// </summary>
		public void ShowFewerButtons()
		{
			// Need to reduce the height of host control if possible
			System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
			if(ctrl!=null)
			{
				int height=this.GetChangeSizeHeight(true);
				if(ctrl.Height-height>0)
					ctrl.Height-=height;
			}
		}

		private void OnNavPaneOptionsClick(object sender, EventArgs e)
		{
			BaseItem.CollapseAll(sender as BaseItem);

			NavPaneOptions options=new NavPaneOptions();
            LocalizationManager lm = new LocalizationManager(this.GetOwner() as IOwnerLocalize);
            if (lm != null)
            {
                string s = lm.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogCancel);
                if (s != "") options.cmdCancel.Text = s;
                s = lm.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogMoveDown);
                if (s != "") options.cmdMoveDown.Text = s;
                s = lm.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogMoveUp);
                if (s != "") options.cmdMoveUp.Text = s;
                s = lm.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogOK);
                if (s != "") options.cmdOK.Text = s;
                s = lm.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogReset);
                if (s != "") options.cmdReset.Text = s;
                s = lm.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogTitle);
                if (s != "") options.Text = s;
                s = lm.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogListLabel);
                if (s != "") options.labelListCaption.Text = s;
                
            }

			options.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			options.NavBarContainer=this;
			System.Windows.Forms.DialogResult result=options.ShowDialog();
			options.Dispose();
			if(result==System.Windows.Forms.DialogResult.OK)
			{
				if(this.ContainerControl is BarBaseControl)
					((BarBaseControl)this.ContainerControl).RecalcLayout();
			}
		}

		private void ConfigurePopupShowing(object sender, EventArgs e)
		{
            if (this.ContainerControl is NavigationBar && BarFunctions.IsOffice2007Style(this.EffectiveStyle))
                ((MenuPanel)m_ConfigureItem.PopupControl).ColorScheme = ((NavigationBar)this.ContainerControl).GetColorScheme();
            else
                ((MenuPanel)m_ConfigureItem.PopupControl).ColorScheme = new ColorScheme(this.EffectiveStyle);
		}
		#endregion

		#region Mouse Support
		public override void InternalMouseHover()
		{
			if(m_ConfigureItem!=null && m_ConfigureItem.Expanded && m_HotSubItem!=m_ConfigureItem && this.DesignMode)
			{
				m_ConfigureItem.Expanded=false;
			}
			base.InternalMouseHover();
		}

		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(m_ConfigureItem!=null && m_ConfigureItem.Expanded && m_HotSubItem!=m_ConfigureItem && !this.DesignMode)
			{
				m_ConfigureItem.Expanded=false;
			}
			base.InternalMouseDown(objArg);
		}

		public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			// Call base implementation if we don't have the more items button
			if(m_ConfigureItem==null || !m_ConfigureItem.DisplayRectangle.Contains(objArg.X,objArg.Y) || this.DesignMode)
			{
				base.InternalMouseMove(objArg);
				return;
			}

			// Mouse is over our more items button
			BaseItem objNew=m_ConfigureItem;
			if(objNew!=m_HotSubItem)
			{
				if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseLeave();
					if(objNew!=null && m_HotSubItem.Expanded)
						m_HotSubItem.Expanded=false;
				}
				
				if(objNew!=null)
				{
					if(m_AutoExpand)
					{
						BaseItem objItem=ExpandedItem();
						if(objItem!=null && objItem!=objNew)
							objItem.Expanded=false;
					}
					objNew.InternalMouseEnter();
					objNew.InternalMouseMove(objArg);
					if(m_AutoExpand && objNew.Enabled && objNew.ShowSubItems)
					{
						if(objNew is PopupItem)
						{
							PopupItem pi=objNew as PopupItem;
							ePopupAnimation oldAnim=pi.PopupAnimation;
							pi.PopupAnimation=ePopupAnimation.None;
							objNew.Expanded=true;
							pi.PopupAnimation=oldAnim;
						}
						else
							objNew.Expanded=true;
					}
					m_HotSubItem=objNew;
				}
				else
					m_HotSubItem=null;
			}
			else if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseMove(objArg);
			}
		}
		#endregion

		protected internal override void OnSubItemExpandChange(BaseItem objItem)
		{
			base.OnSubItemExpandChange(objItem);
			if(objItem.Expanded)
			{
				foreach(BaseItem objExp in this.SubItems)
				{
					if(objExp!=objItem && objExp is PopupItem && objExp.Expanded)
						objExp.Expanded=false;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public override bool Expanded
		{
			get
			{
				return base.Expanded;
			}
			set
			{
				if(!value)
				{
					foreach(BaseItem objExp in this.SubItems)
					{
						if(objExp.Expanded)
							objExp.Expanded=false;
					}
					if(m_ConfigureItem!=null && m_ConfigureItem.Expanded)
						m_ConfigureItem.Expanded=false;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether Configure Buttons button is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates Configure Buttons button is visible."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureItemVisible
		{
			get {return m_ConfigureItemVisible;}
			set
			{
				if(m_ConfigureItemVisible!=value)
				{
					m_ConfigureItemVisible=value;
					NeedRecalcSize=true;
					if(!m_ConfigureItemVisible && m_ConfigureItem!=null)
						m_ConfigureItem=null;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureShowHideVisible
		{
			get {return m_ConfigureShowHideVisible;}
			set
			{
				if(m_ConfigureShowHideVisible!=value)
				{
					m_ConfigureShowHideVisible=value;
                    if (m_ConfigureItem != null)
                    {
                        if (m_ConfigureItem.Expanded) m_ConfigureItem.Expanded = false;
                        m_ConfigureItem = null;
                        NeedRecalcSize = true;
                    }
				}
			}
		}

		/// <summary>
		/// Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureNavOptionsVisible
		{
			get {return m_ConfigureNavOptionsVisible;}
			set
			{
				if(m_ConfigureNavOptionsVisible!=value)
				{
					m_ConfigureNavOptionsVisible=value;
                    if (m_ConfigureItem != null)
                    {
                        if (m_ConfigureItem.Expanded) m_ConfigureItem.Expanded = false;
                        m_ConfigureItem = null;
                    }
					NeedRecalcSize=true;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureAddRemoveVisible
		{
			get {return m_ConfigureAddRemoveVisible;}
			set
			{
				if(m_ConfigureAddRemoveVisible!=value)
				{
					m_ConfigureAddRemoveVisible=value;
                    if (m_ConfigureItem != null)
                    {
                        if (m_ConfigureItem.Expanded) m_ConfigureItem.Expanded = false;
                        m_ConfigureItem = null;
                        NeedRecalcSize = true;
                    }
				}
			}
		}

		/// <summary>
		/// Gets or sets the padding in pixels at the top portion of the item. Height of each item will be increased by padding amount.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates the padding in pixels at the top portion of the item."),DefaultValue(2)]
		public int ItemPaddingTop
		{
			get {return m_ItemPaddingTop;}
			set
			{
				m_ItemPaddingTop=value;
				NeedRecalcSize=true;
				if(this.DesignMode)
					this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the padding in pixels for bottom portion of the item. Height of each item will be increased by padding amount.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates the padding in pixels at the bottom of the item."),DefaultValue(2)]
		public int ItemPaddingBottom
		{
			get {return m_ItemPaddingBottom;}
			set
			{
				m_ItemPaddingBottom=value;
				NeedRecalcSize=true;
				if(this.DesignMode)
					this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar."),DefaultValue(true)]
		public bool AutoSizeButtonImage
		{
			get {return m_AutoSizeButtonImage;}
			set
			{
				if(m_AutoSizeButtonImage!=value)
				{
					m_AutoSizeButtonImage=value;
					NeedRecalcSize=true;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
		public Size ImageSizeSummaryLine
		{
			get {return m_ImageSizeSummaryLine;}
			set
			{
				m_ImageSizeSummaryLine=value;
				if(m_AutoSizeButtonImage)
				{
					NeedRecalcSize=true;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeImageSizeSummaryLine()
		{
			return (m_ImageSizeSummaryLine.Width!=16 || m_ImageSizeSummaryLine.Height!=16);
		}

		/// <summary>
		/// Gets or sets whether summary line is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(true),Description("Indicates whether summary line is visible.")]
		public bool SummaryLineVisible
		{
			get {return m_SummaryLineVisible;}
			set
			{
				if(m_SummaryLineVisible!=value)
				{
					m_SummaryLineVisible=value;
					NeedRecalcSize=true;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void RefreshImageSize()
		{}

		/// <summary>
		/// Must be called by any sub item that implements the image when image has changed
		/// </summary>
		public override void OnSubItemImageSizeChanged(BaseItem objItem){}

		/// <summary>
		/// Occurs after an item has been added to the container. This procedure is called on both item being added and the parent of the item. To distinguish between those two states check the item parameter.
		/// </summary>
		/// <param name="item">When occurring on the parent this will hold the reference to the item that has been added. When occurring on the item being added this will be null (Nothing).</param>
		protected internal override void OnItemAdded(BaseItem item)
		{
			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null)
				owner.InvokeItemAdded(item,new EventArgs());
		}

        public override void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
        {
            base.InternalKeyDown(objArg);

            ContainerKeyboardNavigation.ProcessKeyDown(this, objArg);
        }
	}
}
