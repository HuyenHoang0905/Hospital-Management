namespace DevComponents.DotNetBar
{
	using System;
	using System.Drawing;
    using System.ComponentModel;
    using DevComponents.DotNetBar.Rendering;

	/// <summary>
	///		PopupItem Class can pop-up it's subitems on either the popup Bar
	///		or menu.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public abstract class PopupItem:ImageItem,IDesignTimeProvider
	{
		#region Events
		/// <summary>
		/// Occurs when popup container is loading. Use this event to assign control to the popup container. If you want to use same control
        /// that you added to the popup container after popup is closed you must handle the PopupContainerUnload event and remove the control
        /// from the popup container so it is not disposed with the container.
		/// </summary>
		public event EventHandler PopupContainerLoad;
		/// <summary>
		/// Occurs when popup container is unloading. Use this event to save any state associated with the control that was contained by the container or
        /// to remove the control from the container so it is not disposed with container.
		/// </summary>
		public event EventHandler PopupContainerUnload;
		/// <summary>
		/// Occurs when popup item is about to open.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is about to open.")]
		public event DotNetBarManager.PopupOpenEventHandler PopupOpen;

		/// <summary>
		/// Occurs while popup item is closing.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is closing.")]
		public event EventHandler PopupClose;

		/// <summary>
		/// Occurs after popup item has been closed.
		/// </summary>
		[System.ComponentModel.Description("Occurs after popup item has been closed.")]
		public event EventHandler PopupFinalized;

		/// <summary>
		/// Occurs just before popup window is shown.
		/// </summary>
		[System.ComponentModel.Description("Occurs just before popup window is shown.")]
		public event EventHandler PopupShowing;
		#endregion

		private MenuPanel m_PopupMenu;
		private Bar m_PopupBar;
		private PopupContainerControl m_PopupContainer=null;
		private bool m_FilterInstalled;
		protected SideBarImage m_SideBar;
		private ePopupType m_PopupType;
		private bool m_WasParentFocused;
		private Size m_OldSubItemsImageSize;  // For menus we need minimum sub items image size of 16x16 default is 12x12
		private int m_PopupWidth=200;
		private ePersonalizedMenus m_PersonalizedMenus=ePersonalizedMenus.Disabled;
		private Point m_PopupLocation=Point.Empty;
		private ePopupAnimation m_PopupAnimation=ePopupAnimation.ManagerControlled;
		private System.Drawing.Font m_PopupFont=null;
		private System.Windows.Forms.Control m_SourceControl=null;
		private ePopupSide m_PopupSide=ePopupSide.Default;
        private bool m_PopupPositionAdjusted = false;

		/// <summary>
		/// Creates new instance of PopupItem.
		/// </summary>
		public PopupItem():this("","") {}
		/// <summary>
		/// Creates new instance of PopupItem and assigns the name to it.
		/// </summary>
		/// <param name="sName">Item name</param>
		public PopupItem(string sName):this(sName,""){}
		/// <summary>
		/// Creates new instance of PopupItem and assigns the name and text to it.
		/// </summary>
		/// <param name="sName">Item name.</param>
		/// <param name="ItemText">Item text.</param>
		public PopupItem(string sName, string ItemText):base(sName, ItemText)
		{
			m_PopupMenu=null;
			m_PopupBar=null;
			m_FilterInstalled=false;
			m_PopupType=ePopupType.Menu;
			m_SideBar=new SideBarImage();
			m_WasParentFocused=false;
			m_OldSubItemsImageSize=Size.Empty;
		}

        protected override void Dispose(bool disposing)
		{
			if(this.Expanded) ClosePopup();
			base.Dispose(disposing);
		}

		/// <summary>
		/// Copies the PopupItem specific properties to new instance of the item.
		/// </summary>
		/// <param name="copy">New ButtonItem instance.</param>
		protected override void CopyToItem(BaseItem copy)
		{
			base.CopyToItem(copy);

			PopupItem popupItem=copy as PopupItem;
			popupItem.PopupContainerLoad=this.PopupContainerLoad;
			popupItem.PopupContainerUnload=this.PopupContainerUnload;
			popupItem.PersonalizedMenus=this.PersonalizedMenus;
			popupItem.PopUpSideBar=this.PopUpSideBar;
			popupItem.PopupAnimation=this.PopupAnimation;
			popupItem.PopupSide=this.PopupSide;
            popupItem.PopupWidth=this.PopupWidth;
            popupItem.PopupClose = this.PopupClose;
            popupItem.PopupContainerLoad = this.PopupContainerLoad;
            popupItem.PopupContainerUnload = this.PopupContainerUnload;
            popupItem.PopupFinalized = this.PopupFinalized;
            popupItem.PopupOpen = this.PopupOpen;
            popupItem.PopupShowing = this.PopupShowing;
            popupItem.PopupType = this.PopupType;
            popupItem.PopupWidth = this.PopupWidth;
		}

		protected internal override void OnItemAdded(BaseItem objItem)
		{
			base.OnItemAdded(objItem);
			// Popup items don't need to recalculate size
			if(this.SubItems.Count>1)
				m_NeedRecalcSize=false;
		}

		/*public override void AddSubItem(BaseItem objItem, int Position)
		{
			base.AddSubItem(objItem, Position);
			// Popup items don't need to recalculate size
			if(this.SubItemsCount>1)
				m_NeedRecalcSize=false;
		}*/

		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			ThisItem.SetAttribute("PopupType",System.Xml.XmlConvert.ToString(((int)m_PopupType)));
			ThisItem.SetAttribute("PopupWidth",System.Xml.XmlConvert.ToString(m_PopupWidth));
			ThisItem.SetAttribute("PersonalizedMenus",System.Xml.XmlConvert.ToString((int)m_PersonalizedMenus));
			ThisItem.SetAttribute("panim",System.Xml.XmlConvert.ToString((int)m_PopupAnimation));

			// Save Font information if needed
			if(m_PopupFont!=null)
			{
				ThisItem.SetAttribute("fontname",m_PopupFont.Name);
				ThisItem.SetAttribute("fontemsize",System.Xml.XmlConvert.ToString(m_PopupFont.Size));
				ThisItem.SetAttribute("fontstyle",System.Xml.XmlConvert.ToString((int)m_PopupFont.Style));
			}
		}

		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);
            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;
			m_PopupType=(ePopupType)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("PopupType"));
			m_PopupWidth=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("PopupWidth"));
			m_PersonalizedMenus=(ePersonalizedMenus)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("PersonalizedMenus"));

			if(ItemXmlSource.HasAttribute("panim"))
				m_PopupAnimation=(ePopupAnimation)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("panim"));
			else
				m_PopupAnimation=ePopupAnimation.SystemDefault;

			m_PopupFont=null;
			// Load font information if it exists
			if(ItemXmlSource.HasAttribute("fontname"))
			{
				string FontName=ItemXmlSource.GetAttribute("fontname");
				float FontSize=System.Xml.XmlConvert.ToSingle(ItemXmlSource.GetAttribute("fontemsize"));
				System.Drawing.FontStyle FontStyle=(System.Drawing.FontStyle)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("fontstyle"));
				try
				{
					m_PopupFont=new Font(FontName,FontSize,FontStyle);
				}
				catch(Exception)
				{
					m_PopupFont=null;
				}
			}
		}

		public override void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
		{
			if(this.Expanded)
			{
				if(m_PopupMenu!=null)
				{
					if(m_PopupMenu.FocusedItem()!=null)
						m_PopupMenu.FocusedItem().InternalKeyDown(objArg);
					if(m_PopupMenu!=null)
						m_PopupMenu.ExKeyDown(objArg);
				}
				else if(m_PopupBar!=null)
				{
					m_PopupBar.ExKeyDown(objArg);
				}
				else if(m_PopupContainer!=null)
				{
					
				}
				return;
			}
			base.InternalKeyDown(objArg);
		}

		/// <summary>
		/// Occurs when the mouse pointer is moved over the item. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseMove(objArg);
			if(this.Expanded && m_PopupMenu!=null && !this.DesignMode)
			{
				System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
                MenuPanel menuPanel = m_PopupMenu;
                if (container != null)
                {
                    Point p = container.PointToScreen(new Point(objArg.X, objArg.Y));
                    if (menuPanel != null && !menuPanel.IsDisposed)
                    {
                        p = menuPanel.PointToClient(p);
                        menuPanel.InternalMouseMove(new System.Windows.Forms.MouseEventArgs(objArg.Button, objArg.Clicks, p.X, p.Y, objArg.Delta));
                    }
                }
			}
			else if(this.Expanded && m_PopupBar!=null && !this.DesignMode)
			{
				System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
                Bar popupBar = m_PopupBar;
				if(container!=null)
				{
					Point p=container.PointToScreen(new Point(objArg.X,objArg.Y));
                    if (popupBar != null && !popupBar.IsDisposed)
                    {
                        p = popupBar.PointToClient(p);
                        popupBar.InternalMouseMove(new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.MouseButtons.None, objArg.Clicks, p.X, p.Y, objArg.Delta));
                    }
				}
			}
		}

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is released. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseUp(objArg);
			if(this.Expanded && m_PopupMenu!=null && !this.DesignMode)
			{
				System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
				if(container!=null)
				{
					Point p=container.PointToScreen(new Point(objArg.X,objArg.Y));
					p=m_PopupMenu.PointToClient(p);
					m_PopupMenu.InternalMouseUp(new System.Windows.Forms.MouseEventArgs(objArg.Button,objArg.Clicks,p.X,p.Y,objArg.Delta));
				}
			}
			else if(this.Expanded && m_PopupBar!=null && !this.DesignMode)
			{
				System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
				if(container!=null)
				{
					Point p=container.PointToScreen(new Point(objArg.X,objArg.Y));
					p=m_PopupBar.PointToClient(p);
					m_PopupBar.InternalMouseUp(new System.Windows.Forms.MouseEventArgs(objArg.Button,objArg.Clicks,p.X,p.Y,objArg.Delta));
				}
			}
		}

		protected override void OnSubItemGotFocus(BaseItem objItem)
		{
			// Make sure to collapse any other item that might be expanded
			foreach(BaseItem item in this.SubItems)
			{
				PopupItem popup=item as PopupItem;
				if(popup!=null && popup.Expanded && objItem!=item)
                    popup.Expanded=false;
			}
			base.OnSubItemGotFocus(objItem);
		}

		/*public override void KeyDown(System.Windows.Forms.KeyEventArgs objArg)
		{
			if(this.Expanded)
			{
				if(m_PopupMenu!=null)
				{
					m_PopupMenu.ExKeyDown(objArg);
				}
				else if(m_PopupBar!=null)
				{
					m_PopupBar.ExKeyDown(objArg);
				}
				return;
			}
			else if(objArg.KeyCode==System.Windows.Forms.Keys.Enter  || objArg.KeyCode==System.Windows.Forms.Keys.Return)
			{
				if(SubItemsCount>0)
				{
					if(m_Parent!=null)
						m_Parent.AutoExpand=true;
					this.Expanded=true;
					objArg.Handled=true;
					return;
				}
			}
			else if(objArg.KeyCode==System.Windows.Forms.Keys.Escape)
			{
				if(SubItemsCount>0 && this.Expanded)
				{
					if(m_Parent!=null)
						m_Parent.AutoExpand=false;
					this.Expanded=false;
					objArg.Handled=true;
					return;
				}
			}

			base.KeyDown(objArg);
		}*/

		public override void ContainerLostFocus(bool appLostFocus)
		{
            base.ContainerLostFocus(appLostFocus);
			if(this.Expanded)
			{
				this.Expanded=false;
				if(m_Parent!=null)
					m_Parent.AutoExpand=false;
			}
		}

		public override void SubItemSizeChanged(BaseItem objChildItem)
		{
			if(m_PopupMenu!=null)
			{
				m_PopupMenu.RecalcSize();
				//m_PopupMenu.Refresh();
			}
			else if(m_PopupBar!=null)
			{
				m_PopupBar.RecalcLayout();
				//m_PopupBar.Refresh();
			}
		}

		protected internal override void OnExpandChange()
		{
			base.OnExpandChange();

			if(!this.Expanded)
				 m_PopupLocation=Point.Empty;

			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;

			if(!BaseItem.IsOnPopup(this) || this.Parent==null)
			{
				IOwner owner=this.GetOwner() as IOwner;
				if(owner!=null && this.Name!="syscustomizepopupmenu")
				{
					if(this.Expanded)
						owner.SetExpandedItem(this);
					else if(owner.GetExpandedItem()==this)
						owner.SetExpandedItem(null);
					owner=null;
				}
				// Set/release focus to container so we can receive keyboard events...
				if(objCtrl is Bar && !this.DesignMode)
				{
					if(this.Expanded)
					{
						m_WasParentFocused=false;
						if(objCtrl.Focused)
							m_WasParentFocused=true;
						else
						{
							// TODO: To fix the focus menu expand comment this out
							if(objCtrl is Bar)
							{
								Bar bar=objCtrl as Bar;
								if(bar.BarState!=eBarState.Floating || bar.MenuBar)
								{
									bar.MenuFocus=true;
									//objCtrl.Focus();
								}
							}
							else
								objCtrl.Focus();
						}
					}
					else
					{
						if(!m_WasParentFocused)
							((Bar)objCtrl).ReleaseFocus();
						Bar bar=objCtrl as Bar;
						if(bar.BarState!=eBarState.Floating && !bar.MenuBar)
							bar.MenuFocus=false;
						m_WasParentFocused=false;
					}
				}
                else if (objCtrl is ItemControl && !this.DesignMode)
                {
                    if (this.Expanded)
                    {
                        m_WasParentFocused = false;
                        if (objCtrl.Focused)
                            m_WasParentFocused = true;
                        else
                        {
                            ItemControl bar = objCtrl as ItemControl;
                            bar.MenuFocus = true;
                        }
                    }
                    else
                    {
                        if (!m_WasParentFocused)
                            ((ItemControl)objCtrl).ReleaseFocus();
						((ItemControl)objCtrl).MenuFocus=false;
                        m_WasParentFocused = false;
                    }
                }
			}

			if(!this.Expanded && (m_PopupMenu!=null || m_PopupBar!=null || m_PopupContainer!=null))
			{
				ClosePopup();
			}

			if(objCtrl==null)
				return;

//			if(this.Expanded && !this.ShowSubItems)
//				return;

			if(IsHandleValid(objCtrl))
			{
				this.Refresh();
			}

			if(this.Expanded && m_PopupMenu==null && m_PopupBar==null && m_PopupContainer==null)
			{
				if(IsHandleValid(objCtrl))
				{
					if(m_PopupType==ePopupType.Menu)
					{
						Point p,ps;
						if(!m_PopupLocation.IsEmpty)
							p=m_PopupLocation;
						else if(m_PopupSide!=ePopupSide.Default)
							p=GetDisplayLocation();
						else if(this.Parent is CustomizeItem || this is CustomizeItem)
						{
							if(this.IsRightHanded)
								p=new Point(m_Rect.Left+3,m_Rect.Top);
							else
								p=new Point(m_Rect.Right-2,m_Rect.Top-2);
						}
						else if(this.IsOnMenu)
						{
							if(this.IsRightHanded)
								p=new Point(m_Rect.Left+2,m_Rect.Top-2);
							else
								p=new Point(m_Rect.Right,m_Rect.Top-2);
						}
						else
						{
							if(m_Orientation==eOrientation.Horizontal)
							{
								if(this.IsRightHanded)
									p=new Point(m_Rect.Right,m_Rect.Bottom);
								else
									p=new Point(m_Rect.Left,m_Rect.Bottom);
							}
							else
							{
								p=new Point(m_Rect.Right,m_Rect.Top);
							}
						}
						ps=objCtrl.PointToScreen(p);
						PopupMenu(ps);
					}
					else if(m_PopupType==ePopupType.ToolBar)
					{
						// Popup on Bar
						Point p,ps;
						if(!m_PopupLocation.IsEmpty)
							p=m_PopupLocation;
						else if(m_PopupSide!=ePopupSide.Default)
							p=GetDisplayLocation();
						else if(this.IsOnMenu)
						{
							if(this.IsRightHanded)
								p=new Point(m_Rect.Left+2,m_Rect.Top-2);
							else
								p=new Point(m_Rect.Right,m_Rect.Top-2);
						}
						else
						{
							if(m_Orientation==eOrientation.Horizontal)
							{
								if(this.IsRightHanded)
									p=new Point(m_Rect.Right,m_Rect.Bottom);
								else
									p=new Point(m_Rect.Left,m_Rect.Bottom);
							}
							else
							{
								p=new Point(m_Rect.Right,m_Rect.Top);
							}
						}
						ps=objCtrl.PointToScreen(p);
						PopupBar(ps);
					}
					else if(m_PopupType==ePopupType.Container)
					{
						Point p,ps;
						if(!m_PopupLocation.IsEmpty)
							p=m_PopupLocation;
						else if(m_PopupSide!=ePopupSide.Default)
							p=GetDisplayLocation();
						else if(this.IsOnMenu)
						{
							if(this.IsRightHanded)
								p=new Point(m_Rect.Left+2,m_Rect.Top-2);
							else
								p=new Point(m_Rect.Right,m_Rect.Top-2);

						}
						else
						{
							if(m_Orientation==eOrientation.Horizontal)
							{
								if(this.IsRightHanded)
									p=new Point(m_Rect.Right,m_Rect.Bottom);
								else
									p=new Point(m_Rect.Left,m_Rect.Bottom);
							}
							else
							{
								p=new Point(m_Rect.Right,m_Rect.Top);
							}
						}
						ps=objCtrl.PointToScreen(p);
						PopupContainer(ps);
					}
				}
			}
			objCtrl=null;
		}

		protected internal override void OnSubItemExpandChange(BaseItem objItem)
		{
			base.OnSubItemExpandChange(objItem);
			if(objItem.Expanded)
			{
				foreach(BaseItem objExp in this.SubItems)
				{
					if(objExp.Expanded && objExp!=objItem)
						objExp.Expanded=false;
				}
			}
		}

        protected virtual bool IsRightHanded
        {
            get
            {
                if (NativeFunctions.RightHandedMenus)
                    return true;
                System.Windows.Forms.Control c = this.ContainerControl as System.Windows.Forms.Control;
                if (c != null)
                    return (c.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
                return false;
            }
        }

		/// <summary>
		/// Displays the sub-items on popup menu.
		/// </summary>
		/// <param name="p">Popup location.</param>
		public void PopupMenu(Point p)
		{
			PopupMenu(p.X,p.Y);
		}

		protected internal IOwnerMenuSupport GetIOwnerMenuSupport()
		{
			return (this.GetOwner() as IOwnerMenuSupport);
		}

		/// <summary>
		/// Gets the size of the popup before it is displayed.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public System.Drawing.Size PopupSize
		{
			get
			{
				if(this.SubItemsImageSize.Width==ImageItem._InitalImageSize.Width && this.SubItemsImageSize.Height==ImageItem._InitalImageSize.Height)
				{
					m_OldSubItemsImageSize=this.SubItemsImageSize;
					this.SubItemsImageSize=new Size(16,16);
				}

				foreach(BaseItem objItem in this.SubItems)
					objItem.Orientation=eOrientation.Horizontal;

				MenuPanel popupMenu=new MenuPanel();
				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
				IOwnerMenuSupport ownerMenu=this.GetIOwnerMenuSupport();

				popupMenu.Owner=this.GetOwner();
				if(m_PopupFont!=null)
					popupMenu.Font=m_PopupFont;
				else if(objCtrl!=null && objCtrl.Font!=null)
				    popupMenu.Font = objCtrl.Font; // (System.Drawing.Font)objCtrl.Font.Clone();
				
				if(ownerMenu!=null && ownerMenu.AlwaysShowFullMenus)
					popupMenu.PersonalizedMenus=ePersonalizedMenus.Disabled;
				else
					popupMenu.PersonalizedMenus=m_PersonalizedMenus;
				if(ownerMenu!=null && !ownerMenu.ShowFullMenusOnHover && (popupMenu.PersonalizedMenus==ePersonalizedMenus.DisplayOnHover || popupMenu.PersonalizedMenus==ePersonalizedMenus.Both))
					popupMenu.PersonalizedMenus=ePersonalizedMenus.DisplayOnClick;

				popupMenu.PopupAnimation=m_PopupAnimation;

				if(ownerMenu!=null && (this.IsOnMenuBar || this.IsOnMenu))
				{
					popupMenu.PersonalizedAllVisible=ownerMenu.PersonalizedAllVisible;
				}

				if(this.Parent is CustomizeItem || this is CustomizeItem && !(this is QatCustomizeItem))
					popupMenu.IsCustomizeMenu=true;

				if(this.GetOwner() is DotNetBarManager && ((DotNetBarManager)this.GetOwner()).UseGlobalColorScheme)
					popupMenu.ColorScheme=((DotNetBarManager)this.GetOwner()).ColorScheme;
                if (objCtrl is Bar)
                    popupMenu.ColorScheme = ((Bar)objCtrl).ColorScheme;
                else if (objCtrl is MenuPanel)
                    popupMenu.ColorScheme = ((MenuPanel)objCtrl).ColorScheme;
                else if(objCtrl is PopupItemControl)
                    popupMenu.ColorScheme = ((PopupItemControl)objCtrl).GetColorScheme();
                else if (BarFunctions.IsOffice2007Style(this.EffectiveStyle) && Rendering.GlobalManager.Renderer is Office2007Renderer)
                    popupMenu.ColorScheme = ((Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable.LegacyColors;
                else
                    popupMenu.ColorScheme = new ColorScheme(this.EffectiveStyle);

				popupMenu.SideBar=m_SideBar;
				popupMenu.ParentItem=this;
				popupMenu.CreateControl();
				popupMenu.RecalcSize();
				System.Drawing.Size size=popupMenu.Size;
				popupMenu.Dispose();
				popupMenu=null;
				return size;
			}
		}

        /// <summary>
        /// Invokes PopupOpen event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPopupOpen(PopupOpenEventArgs e)
        {
            if (PopupOpen != null)
                PopupOpen(this, e);
            if (!e.Cancel)
            {
                IOwnerMenuSupport ownerMenu = this.GetIOwnerMenuSupport();
                if(ownerMenu!=null)
                    ownerMenu.InvokePopupOpen(this, e);
            }
        }

        /// <summary>
		/// Displays the sub-items on popup menu.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
        public virtual void PopupMenu(int x, int y)
        {
            PopupMenu(x, y, true);
        }

		/// <summary>
		/// Displays the sub-items on popup menu.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
        /// <param name="verifyScreenPosition">Indicates whether screen position of the menu is verified so the menu always appears on the screen.</param>
		public virtual void PopupMenu(int x, int y, bool verifyScreenPosition)
		{
            PopupPositionAdjusted = false;
			IOwnerMenuSupport ownerMenu=this.GetIOwnerMenuSupport();

			//if(ownerMenu==null)
			//	throw(new InvalidOperationException("Current owner is not assigned or it does not have popup menu support."));

			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;

			if(ownerMenu!=null)
			{
                PopupOpenEventArgs args=new PopupOpenEventArgs();
                OnPopupOpen(args);
				if(args.Cancel)
				{
					this.Expanded=false;
					return;
				}
			}

			if(this.SubItemsImageSize.Width==ImageItem._InitalImageSize.Width && this.SubItemsImageSize.Height==ImageItem._InitalImageSize.Height)
			{
				m_OldSubItemsImageSize=this.SubItemsImageSize;
				this.SubItemsImageSize=new Size(16,16);
			}

			foreach(BaseItem objItem in this.SubItems)
				objItem.Orientation=eOrientation.Horizontal;

			if(m_PopupMenu==null)
			{
				m_PopupMenu=new MenuPanel();
				m_PopupMenu.Owner=this.GetOwner();
                m_PopupMenu.UseWholeScreenForSizeChecking = !verifyScreenPosition;

				if(m_PopupFont!=null)
					m_PopupMenu.Font=m_PopupFont;
				else if(objCtrl!=null && objCtrl.Font!=null)
				    m_PopupMenu.Font = objCtrl.Font; // (System.Drawing.Font)objCtrl.Font.Clone();
				
				if(ownerMenu!=null && ownerMenu.AlwaysShowFullMenus)
					m_PopupMenu.PersonalizedMenus=ePersonalizedMenus.Disabled;
				else
                    m_PopupMenu.PersonalizedMenus=m_PersonalizedMenus;
				if(ownerMenu!=null && !ownerMenu.ShowFullMenusOnHover && (m_PopupMenu.PersonalizedMenus==ePersonalizedMenus.DisplayOnHover || m_PopupMenu.PersonalizedMenus==ePersonalizedMenus.Both))
					m_PopupMenu.PersonalizedMenus=ePersonalizedMenus.DisplayOnClick;

				m_PopupMenu.PopupAnimation=m_PopupAnimation;

				if(ownerMenu!=null && (this.IsOnMenuBar || this.IsOnMenu))
				{
					m_PopupMenu.PersonalizedAllVisible=ownerMenu.PersonalizedAllVisible;
				}

				if(this.Parent is CustomizeItem || this is CustomizeItem && !(this is QatCustomizeItem))
					m_PopupMenu.IsCustomizeMenu=true;

				if(objCtrl is Bar)
				{
                    Bar bar=(Bar)objCtrl;
                    m_PopupMenu.ColorScheme = bar.GetColorScheme();
                    m_PopupMenu.ShowToolTips = bar.ShowToolTips;
                    m_PopupMenu.AntiAlias = bar.AntiAlias;
                    if (objCtrl is ContextMenuBar)
                        m_PopupMenu.UseWholeScreenForSizeChecking = true;
				}
				else if(objCtrl is MenuPanel)
				{
                    MenuPanel menuPanel = (MenuPanel)objCtrl;
                    m_PopupMenu.ColorScheme = menuPanel.ColorScheme;
                    m_PopupMenu.ShowToolTips = menuPanel.ShowToolTips;
                    m_PopupMenu.AntiAlias = menuPanel.AntiAlias;
				}
				else if(objCtrl is SideBar)
					m_PopupMenu.ColorScheme=((SideBar)objCtrl).ColorScheme;
                else if (objCtrl is ExplorerBar)
                {
                    ExplorerBar eb = objCtrl as ExplorerBar;
                    m_PopupMenu.ColorScheme = eb.ColorScheme;
                    m_PopupMenu.AntiAlias = eb.AntiAlias;
                }
                else if (objCtrl is BarBaseControl)
                    m_PopupMenu.ColorScheme = ((BarBaseControl)objCtrl).ColorScheme;
                else if (objCtrl is ItemControl)
                {
                    ItemControl ic = (ItemControl)objCtrl;
                    m_PopupMenu.ColorScheme = ic.GetColorSchemeInternal();
                    m_PopupMenu.AntiAlias = ic.AntiAlias;
                }
                else if (objCtrl is PopupItemControl)
                {
                    PopupItemControl bx = (PopupItemControl)objCtrl;
                    m_PopupMenu.ColorScheme = bx.GetColorScheme();
                    m_PopupMenu.AntiAlias = bx.AntiAlias;
                }
                else
                {
                    if (this.Style == eDotNetBarStyle.StyleManagerControlled && GlobalManager.Renderer is Office2007Renderer)
                    {
                        m_PopupMenu.ColorScheme = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
                    }
                    else
                    {
                        m_PopupMenu.ColorScheme = new ColorScheme(this.EffectiveStyle);
                    }
                    object own = this.GetOwner();
                    if (own is ItemControl)
                    {
                        m_PopupMenu.AntiAlias = ((ItemControl)own).AntiAlias;
                    }
                    else if (own is Bar)
                    {
                        m_PopupMenu.AntiAlias = ((Bar)own).AntiAlias;
                    }
                }

                if (objCtrl != null)
                    m_PopupMenu.RightToLeft = objCtrl.RightToLeft;

				m_PopupMenu.SideBar=m_SideBar;
				m_PopupMenu.ParentItem=this;
				m_PopupMenu.CreateControl();
				m_PopupMenu.RecalcSize();
			}

			// Make sure that menu is on-screen
			ScreenInformation objScreen=BarFunctions.ScreenFromPoint(new Point(x,y));
            if(objScreen == null && IsHandleValid(objCtrl))
				objScreen=BarFunctions.ScreenFromControl(objCtrl);

			Rectangle workingArea=objScreen.WorkingArea;
            if (this.Parent == null)
            {
                workingArea = objScreen.Bounds;
                m_PopupMenu.UseWholeScreenForSizeChecking = true;
            }

			if(this.IsRightHanded)
			{
				// When working with right-handed menus for TabletPC the coordinates passed here are
				// the upper right x,y coordinates since menus are right aligned...
				x-=m_PopupMenu.Width;
                if (objScreen != null && x < workingArea.Left && verifyScreenPosition)
				{
                    PopupPositionAdjusted = true;
					// Push it to the right side
					if(this.Displayed && objCtrl!=null && (m_PopupMenu.IsCustomizeMenu || this.IsOnMenu))
					{
						Point p,ps;
						
						if(m_PopupMenu.IsCustomizeMenu)
						{
							p=new Point(m_Rect.Right,m_Rect.Bottom);
						}
						else
						{
							p=new Point(m_Rect.Right,m_Rect.Top);
						}

						ps=objCtrl.PointToScreen(p);
						x=ps.X;
                        if (x + m_PopupMenu.Width > workingArea.Right)
                        {
                            x = workingArea.Right - m_PopupMenu.Width;
                        }
					}
					else
						x=workingArea.Left;
				}
			}
			else
			{
                if (objScreen != null && (x + m_PopupMenu.Width > workingArea.Right || x < workingArea.Left) && verifyScreenPosition)
                {
                    PopupPositionAdjusted = true;
                    if (x + m_PopupMenu.Width > workingArea.Right)
                    {
                        // Push it to the right side
                        if (this.Displayed && objCtrl != null && !(objCtrl is ContextMenuBar))
                        {
                            Point p, ps;

                            if (m_PopupMenu.IsCustomizeMenu)
                            {
                                p = new Point(m_Rect.Left, m_Rect.Bottom);
                            }
                            else
                            {
                                if (this.IsOnMenu)
                                    p = new Point(m_Rect.Left, m_Rect.Top);
                                else
                                    p = new Point(m_Rect.Right, m_Rect.Top);
                            }

                            ps = objCtrl.PointToScreen(p);
                            x = ps.X - m_PopupMenu.Width;
                            if (x + m_PopupMenu.Width > workingArea.Right)
                                x = workingArea.Right - m_PopupMenu.Width;
                        }
                        else
                            x = workingArea.Right - m_PopupMenu.Width;
                    }
                    else if (x < workingArea.Left)
                        x = workingArea.Left;
                }
			}

			// Try to fit whole popup menu "nicely"
            if (objScreen != null && verifyScreenPosition)
			{
                if (y + m_PopupMenu.Height > workingArea.Bottom)
                {
                    // If this container is displayed then try to put it above the menu item
                    if (this.Displayed && objCtrl != null && objCtrl.Visible)
                    {
                        Point p = new Point(m_Rect.Left, m_Rect.Bottom - 1), ps;
                        ps = objCtrl.PointToScreen(p);
                        ps.Y += 2;
                        if (ps.Y - m_PopupMenu.Height >= workingArea.Top)
                            y = Math.Min(workingArea.Bottom, ps.Y) - m_PopupMenu.Height;
                        else if (this is Office2007StartButton)
                            y = workingArea.Bottom - m_PopupMenu.Height - 4;
                    }
                    else
                        y = workingArea.Bottom - m_PopupMenu.Height;
                    if (y < workingArea.Top)
                        y = workingArea.Top;
                    PopupPositionAdjusted = true;
                }
			}
			// If it still does not fit at this point, container will scale itself properly
			// And allow item scrolling
			m_PopupMenu.Location=new Point(x,y);

            OnPopupShowing(EventArgs.Empty);

			m_PopupMenu.Show();

            if (this.ContainerControl is IKeyTipsControl)
            {
                IKeyTipsControl kc = this.ContainerControl as IKeyTipsControl;
                m_PopupMenu.ShowKeyTips = kc.ShowKeyTips;
                kc.ShowKeyTips = false;
            }

			this.Expanded=true;
			
			if(ownerMenu!=null)
			{
				if(!(objCtrl is Bar) && !(objCtrl is MenuPanel))
				{
					ownerMenu.RegisterPopup(this);
					m_FilterInstalled=true;
				}
				else if(objCtrl is Bar)
				{
					Bar bar=objCtrl as Bar;
					if(bar.BarState!=eBarState.Popup)
					{
						ownerMenu.RegisterPopup(this);
						m_FilterInstalled=true;
					}		
					bar=null;
				}
			}
		}

        /// <summary>
        /// Raises PopupShowing event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPopupShowing(EventArgs e)
        {
            if (PopupShowing != null)
                PopupShowing(this, e);

            IOwnerMenuSupport ownerMenu = this.GetIOwnerMenuSupport();
            if (ownerMenu != null)
                ownerMenu.InvokePopupShowing(this, e);
        }
        /// <summary>
		/// Displays the sub-items on popup toolbar.
		/// </summary>
		/// <param name="p">Popup location.</param>
		public void PopupBar(Point p)
		{
			PopupBar(p.X,p.Y);
		}
		
		/// <summary>
		/// Displays the sub-items on popup toolbar.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
		public virtual void PopupBar(int x, int y)
		{
			//DotNetBarManager owner=this.GetOwner();
			IOwnerMenuSupport ownerMenu=this.GetIOwnerMenuSupport();
			//if(ownerMenu==null)
			//	throw(new InvalidOperationException("Current owner is not assigned or it does not have popup bar support."));

			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;

			if(ownerMenu!=null)
			{
				PopupOpenEventArgs args=new PopupOpenEventArgs();
				if(PopupOpen!=null)
					PopupOpen(this,args);
				if(!args.Cancel)
					ownerMenu.InvokePopupOpen(this,args);
				if(args.Cancel)
				{
					this.Expanded=false;
					return;
				}
			}

			foreach(BaseItem objItem in this.SubItems)
				objItem.Orientation=eOrientation.Horizontal;

			if(m_PopupBar==null)
			{
				m_PopupBar=new Bar();

				if(m_PopupFont!=null)
					m_PopupBar.Font=m_PopupFont;
				else if(objCtrl!=null && objCtrl.Font!=null)
					m_PopupBar.Font=(System.Drawing.Font)objCtrl.Font.Clone();
				if(objCtrl is Bar && ((Bar)objCtrl).ColorScheme!=null)
					m_PopupBar.ColorScheme=((Bar)objCtrl).ColorScheme;
				else if(objCtrl is MenuPanel && ((MenuPanel)objCtrl).ColorScheme!=null)
					m_PopupBar.ColorScheme=((MenuPanel)objCtrl).ColorScheme;
                else if (objCtrl is ItemControl && ((ItemControl)objCtrl).ColorScheme != null)
                    m_PopupBar.ColorScheme = ((ItemControl)objCtrl).ColorScheme;
                else if (objCtrl is ButtonX && ((ButtonX)objCtrl).ColorScheme != null)
                    m_PopupBar.ColorScheme = ((ButtonX)objCtrl).ColorScheme;
				else
                    m_PopupBar.ColorScheme = new ColorScheme(this.EffectiveStyle);
				m_PopupBar.SetBarState(eBarState.Popup);
				m_PopupBar.PopupAnimation=m_PopupAnimation;
				m_PopupBar.Owner=this.GetOwner();
				m_PopupBar.ParentItem=this;
                if(objCtrl!=null)
                    m_PopupBar.RightToLeft = objCtrl.RightToLeft;
				m_PopupBar.CreateControl();
				m_PopupBar.SetDesignMode(this.DesignMode);
				m_PopupBar.ThemeAware=this.ThemeAware;
				
				if(objCtrl is IBarImageSize)
					m_PopupBar.ImageSize=((IBarImageSize)objCtrl).ImageSize;					

				m_PopupBar.PopupWidth=m_PopupWidth;

				m_PopupBar.RecalcSize();
			}

			// Make sure that menu is on-screen
			ScreenInformation objScreen=null;
			if(IsHandleValid(objCtrl))
			{
				objScreen=BarFunctions.ScreenFromControl(objCtrl);
			}
			else
			{
				objScreen=BarFunctions.ScreenFromPoint(new Point(x,y));
			}

			if(this.IsRightHanded)
				x-=m_PopupBar.Width;

			if(objScreen!=null && x+m_PopupBar.Width>objScreen.WorkingArea.Right)
			{
				// Push it to the right side
				x=objScreen.WorkingArea.Right-m_PopupBar.Width;
			}
			else if(objScreen!=null && x<objScreen.WorkingArea.Left)
				x=objScreen.WorkingArea.Left;

			// Try to fit whole popup menu "nicely"
			if(objScreen!=null && y+m_PopupBar.Height>objScreen.WorkingArea.Bottom)
			{
				// If this container is displayed then try to put it above the menu item
				if(this.Displayed && objCtrl!=null)
				{
					Point p=new Point(m_Rect.Left,m_Rect.Bottom), ps;
					ps=objCtrl.PointToScreen(p);
					ps.Y+=2;
					if(ps.Y-m_PopupBar.Height>=objScreen.WorkingArea.Top)
						y=ps.Y-m_PopupBar.Height;
				}
			}
			// If it still does not fit at this point, container will scale itself properly
			// And allow item scrolling
			m_PopupBar.Location=new Point(x,y);
			if(ownerMenu!=null)
				ownerMenu.InvokePopupShowing(this,new EventArgs());
			m_PopupBar.ShowBar();
			this.Expanded=true;
			
			if(ownerMenu!=null)
			{
				if(!(objCtrl is Bar) && !(objCtrl is MenuPanel))
				{
					ownerMenu.RegisterPopup(this);
					m_FilterInstalled=true;
				}
				else if(objCtrl is Bar)
				{
					Bar bar=objCtrl as Bar;
					if(bar.BarState!=eBarState.Popup)
					{
						ownerMenu.RegisterPopup(this);
						m_FilterInstalled=true;
					}		
					bar=null;
				}
			}
		}

		/// <summary>
		/// Displays popup container.
		/// </summary>
		/// <param name="p">Popup location.</param>
		public void PopupContainer(Point p)
		{
			PopupContainer(p.X,p.Y);
		}

        /// <summary>
        /// Creates the popup container control which is a parent/holder control for the controls you want to display on the popup.
        /// This method can be used if you do not wish to handle the PopupContainerLoad to add controls to the popup container.
        /// After calling this method you can access PopupContainerControl property to add your controls to be displayed on the popup.
        /// </summary>
        /// <param name="fireLoadEvents">Indicates whether PopupContainerLoad events are fired.</param>
        public virtual void CreatePopupContainer(bool fireLoadEvents)
        {
            if (this.PopupType != ePopupType.Container)
                throw new InvalidOperationException("Method can only be called for PopupType set to ePopupType.Container");

            if (m_PopupContainer == null)
            {
                m_PopupContainer = new PopupContainerControl();
                m_PopupContainer.Owner = this.GetOwner();
                m_PopupContainer.ParentItem = this;
                m_PopupContainer.CreateControl();
                m_PopupContainer.SetDesignMode(this.DesignMode);
                m_PopupContainer.Width = m_PopupWidth;

                if (fireLoadEvents)
                {
                    IOwnerMenuSupport ownerMenu = this.GetIOwnerMenuSupport();
                    // Fire off events
                    if (PopupContainerLoad != null)
                        PopupContainerLoad(this, new EventArgs());
                    if (ownerMenu != null)
                        ownerMenu.InvokePopupContainerLoad(this, new EventArgs());

                    // Recalc Size would go here...
                    m_PopupContainer.RecalcSize();
                }
            }
        }

		/// <summary>
		/// Displays popup container.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
		public virtual void PopupContainer(int x, int y)
		{
			//DotNetBarManager owner=this.GetOwner();
			IOwnerMenuSupport ownerMenu=this.GetIOwnerMenuSupport();
			if(ownerMenu==null)
				throw(new InvalidOperationException("Current owner is not assigned or it does not have popup bar support."));

            CreatePopupContainer(true);
			
			// Make sure that container is on-screen
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			ScreenInformation objScreen=null;
			if(IsHandleValid(objCtrl))
			{
				objScreen=BarFunctions.ScreenFromControl(objCtrl);
			}
			else
			{
				objScreen=BarFunctions.ScreenFromPoint(new Point(x,y));
			}

			if(this.IsRightHanded)
				x-=m_PopupContainer.Width;

			if(objScreen!=null && x+m_PopupContainer.Width>objScreen.WorkingArea.Right)
			{
				// Push it to the right side
				x=objScreen.WorkingArea.Right-m_PopupContainer.Width;
			}
			else if(objScreen!=null && x<objScreen.WorkingArea.Left)
				x=objScreen.WorkingArea.Left;

			// Try to fit whole popup menu "nicely"
			if(objScreen!=null && y+m_PopupContainer.Height>objScreen.WorkingArea.Bottom)
			{
				// If this container is displayed then try to put it above the menu item
				if(this.Displayed && objCtrl!=null)
				{
					Point p=new Point(m_Rect.Left,m_Rect.Top), ps;
					ps=objCtrl.PointToScreen(p);
					ps.Y+=2;
					if(ps.Y-m_PopupContainer.Height>=objScreen.WorkingArea.Top)
						y=ps.Y-m_PopupContainer.Height;
				}
			}
			// If it still does not fit at this point, container will scale itself properly
			// And allow item scrolling
			m_PopupContainer.Location=new Point(x,y);
			if(ownerMenu!=null)
				ownerMenu.InvokePopupShowing(this,new EventArgs());
			m_PopupContainer.Show();
			this.Expanded=true;
			
			if(ownerMenu!=null)
			{
				if(!(objCtrl is Bar) && !(objCtrl is MenuPanel))
				{
					ownerMenu.RegisterPopup(this);
					m_FilterInstalled=true;
				}
				else if(objCtrl is Bar)
				{
					Bar bar=objCtrl as Bar;
					if(bar.BarState!=eBarState.Popup)
					{
						ownerMenu.RegisterPopup(this);
						m_FilterInstalled=true;
					}		
					bar=null;
				}
			}
		}

		/// <summary>
		/// Displays the sub-items on popup specified by PopupType.
		/// </summary>
		/// <param name="p">Popup location.</param>
		public virtual void Popup(Point p)
		{
			this.Popup(p.X,p.Y);
		}

		/// <summary>
		/// Displays the sub-items on popup specified by PopupType.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
		public virtual void Popup(int x, int y)
		{
			if(m_PopupBar!=null || m_PopupMenu!=null || m_PopupContainer!=null)
				ClosePopup();
			switch(m_PopupType)
			{
				case ePopupType.Container:
					this.PopupContainer(x,y);
					break;
				case ePopupType.Menu:
					this.PopupMenu(x,y);
					break;
				case ePopupType.ToolBar:
					this.PopupBar(x,y);
					break;
			}
		}

        /// <summary>
        /// Raises PopupClose event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPopupClose(EventArgs e)
        {
            IOwnerMenuSupport ownerMenu = this.GetIOwnerMenuSupport();
            if (PopupClose != null)
                PopupClose(this, e);

            if (ownerMenu != null)
                ownerMenu.InvokePopupClose(this, e);
        }


		/// <summary>
		/// Closes the currently open popup.
		/// </summary>
		public virtual void ClosePopup()
		{
			if(m_OldSubItemsImageSize!=Size.Empty)
			{
				this.SubItemsImageSize=m_OldSubItemsImageSize;
				m_OldSubItemsImageSize=Size.Empty;
			}

			//DotNetBarManager owner=this.GetOwner();
			IOwnerMenuSupport ownerMenu=this.GetIOwnerMenuSupport();

			if(ownerMenu!=null)
				ownerMenu.UnregisterPopup(this);
			m_FilterInstalled=false;

			// Fire off events
			if(m_PopupContainer!=null)
			{
				if(PopupContainerUnload!=null)
					PopupContainerUnload(this,new EventArgs());
				
				if(ownerMenu!=null)
					ownerMenu.InvokePopupContainerUnload(this,new EventArgs());
			}

			if((m_PopupBar!=null || m_PopupMenu!=null || m_PopupContainer!=null) && ownerMenu!=null)
			{
                OnPopupClose(EventArgs.Empty);
			}

			if(m_PopupMenu!=null)
			{
                if (this.ContainerControl is IKeyTipsControl)
                {
                    IKeyTipsControl kc = this.ContainerControl as IKeyTipsControl;
                    kc.ShowKeyTips = m_PopupMenu.ShowKeyTips;
                    m_PopupMenu.ShowKeyTips = false;
                }
                m_PopupMenu.Close();
				m_PopupMenu.Dispose();
				m_PopupMenu=null;
			}
			if(m_PopupBar!=null)
			{
				m_PopupBar.Hide();
				m_PopupBar.Dispose();
				m_PopupBar=null;
			}
			if(m_PopupContainer!=null)
			{
				m_PopupContainer.Hide();
				m_PopupContainer.Dispose();
				m_PopupContainer=null;
			}
			this.Expanded=false;

			IOwner owner=this.GetOwner() as IOwner;
			if(owner!=null && owner.ParentForm!=null)
			{
				owner.ParentForm.Update();
			}

            OnPopupFinalized(EventArgs.Empty);
		}

        /// <summary>
        /// Raises PopupFinalized event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPopupFinalized(EventArgs e)
        {
            if (PopupFinalized != null)
                PopupFinalized(this, e);
        }
        /// <summary>
		/// Indicates whether sub-items are shown on popup Bar or popup menu.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates whether sub-items are shown on popup Bar or popup menu."),System.ComponentModel.DefaultValue(ePopupType.Menu)]
		public virtual ePopupType PopupType
		{
			get
			{
				return m_PopupType;
			}
			set
			{
				if(m_PopupType==value)
					return;
				m_PopupType=value;
                if(ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "PopupType");
				NeedRecalcSize=true;
				this.Refresh();
			}
		}

		/// <summary>
		/// Indicates the font that will be used on the popup window.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the font that will be used on the popup window."),System.ComponentModel.DefaultValue(null)]
		public virtual System.Drawing.Font PopupFont
		{
			get
			{
				return m_PopupFont;
			}
			set
			{
				if(m_PopupFont==value)
					return;
				m_PopupFont=value;
			}
		}

		/// <summary>
		/// Indicates side bar for pop-up window.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates side bar for pop-up window.")]
		public virtual SideBarImage PopUpSideBar
		{
			get
			{
				return m_SideBar;
			}
			set
			{
				m_SideBar=value;
			}
		}

		/// <summary>
		/// Gets or sets the location of popup in relation to it's parent.
		/// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(ePopupSide.Default), Description("Indicates location of popup in relation to it's parent.")]
		public virtual ePopupSide PopupSide
		{
			get {return m_PopupSide;}
			set
            {
                m_PopupSide=value;
                OnAppearanceChanged();
            }
		}

		private System.Drawing.Point GetDisplayLocation()
		{
			System.Drawing.Point p=System.Drawing.Point.Empty;
			if(this.Parent==null && !(this.ContainerControl is ButtonX))
				return p;

			if(m_PopupSide==ePopupSide.Right)
			{
				p.X=m_Rect.Right;
				p.Y=m_Rect.Top;
			}
			else if(m_PopupSide==ePopupSide.Left)
			{
				System.Drawing.Size ps=this.PopupSize;
				p.X=m_Rect.Left-ps.Width;
				p.Y=m_Rect.Top;
			}
			else if(m_PopupSide==ePopupSide.Top)
			{
				System.Drawing.Size ps=this.PopupSize;
				p.Y=m_Rect.Top-ps.Height;
				//if(p.Y<0)
				//	p.Y=m_Rect.Bottom;
				if(ps.Width>m_Rect.Width)
					p.X=m_Rect.Left;
				else
					p.X=m_Rect.Right-ps.Width;
			}
			else if(m_PopupSide==ePopupSide.Bottom)
			{
				System.Drawing.Size ps=this.PopupSize;
				p.Y=m_Rect.Bottom;
				if(ps.Width>m_Rect.Width)
					p.X=m_Rect.Left;
				else
					p.X=m_Rect.Right-ps.Width;
			}
			return p;
		}

		/// <summary>
		/// Gets or sets the popup location.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Point PopupLocation
		{
			get
			{
				if(m_PopupMenu!=null)
					return m_PopupMenu.Location;
				else if(m_PopupBar!=null)
					return m_PopupBar.Location;
				else if(m_PopupContainer!=null)
					return m_PopupContainer.Location;
				return m_PopupLocation;
			}
			set
			{
				m_PopupLocation=value;
				if(m_PopupMenu!=null)
					m_PopupMenu.Location=value;
                else if(m_PopupBar!=null)
					m_PopupBar.Location=value;
				else if(m_PopupContainer!=null)
					m_PopupContainer.Location=value;
			}
		}

		protected internal override void OnBeforeItemRemoved(BaseItem objItem)
		{
			base.OnBeforeItemRemoved(objItem);
			if(m_PopupBar!=null && objItem!=null)
				m_PopupBar.Items.Remove(objItem);
		}

		protected internal override void OnAfterItemRemoved(BaseItem objItem)
		{
			base.OnAfterItemRemoved(objItem);
			if(this.Expanded && objItem==null)
				this.Expanded=false;
			if(m_PopupMenu!=null && !this.SuspendLayout)
				m_PopupMenu.RecalcSize();
            else if (m_PopupBar != null && !this.SuspendLayout)
				m_PopupBar.RecalcLayout();
		}

		/// <summary>
		/// Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true."),System.ComponentModel.DefaultValue(ePersonalizedMenus.Disabled)]
		public virtual ePersonalizedMenus PersonalizedMenus
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

		/// <summary>
		/// Indicates Animation type for Popups.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates Animation type for Popups."),System.ComponentModel.DefaultValue(ePopupAnimation.ManagerControlled)]
		public virtual ePopupAnimation PopupAnimation
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

		internal bool FilterInstalled
		{
			get
			{
				return m_FilterInstalled;
			}
			set
			{
				m_FilterInstalled=true;
			}
		}

		protected internal override bool IsAnyOnHandle(int iHandle)
		{
			bool bRet=base.IsAnyOnHandle(iHandle);
			if(!bRet && m_PopupContainer!=null && m_PopupContainer.Handle.ToInt32()==iHandle)
				bRet=true;
			return bRet;
		}

		//***********************************************
		// IDesignTimeProvider Implementation
		//***********************************************
		public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
		{
			if(!this.Expanded)
				return null;
			if(m_PopupMenu!=null)
				return m_PopupMenu.GetInsertPosition(pScreen,DragItem);
			else if(m_PopupBar!=null)
				return ((IDesignTimeProvider)m_PopupBar.ItemsContainer).GetInsertPosition(pScreen, DragItem);
			return null;
		}
		public void DrawReversibleMarker(int iPos, bool Before)
		{
			if(!this.Expanded)
				return;
			if(m_PopupMenu!=null)
				m_PopupMenu.DrawReversibleMarker(iPos, Before);
			else if(m_PopupBar!=null)
				((IDesignTimeProvider)m_PopupBar.Items).DrawReversibleMarker(iPos, Before);
		}
		public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
		{
			if(!this.Expanded)
				return;
			if(m_PopupMenu!=null)
				m_PopupMenu.InsertItemAt(objItem, iPos, Before);
			else
				((IDesignTimeProvider)m_PopupBar.Items).InsertItemAt(objItem, iPos, Before);
		}

		/// <summary>
		/// Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only."),System.ComponentModel.DefaultValue(200)]
		public virtual int PopupWidth
		{
			get
			{
				return m_PopupWidth;
			}
			set
			{
				if(m_PopupWidth!=value && value>0)
				{
					m_PopupWidth=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "PopupWidth");
				}
			}
		}

		/// <summary>
		/// Gets the control that is displaying the context menu.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.Description("Gets the control that is displaying the context menu.")]
		public System.Windows.Forms.Control SourceControl
		{
			get
			{
				return m_SourceControl;
			}
		}

        /// <summary>
        /// Sets the SourceControl for popup menu or toolbar.
        /// </summary>
        /// <param name="ctrl">Control that popup menu or toolbar was invoked for.</param>
		public void SetSourceControl(System.Windows.Forms.Control ctrl)
		{
			m_SourceControl=ctrl;
		}

		/// <summary>
		/// Gets the reference to the internal host PopupContainerControl control.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public PopupContainerControl PopupContainerControl
		{
			get
			{
				return m_PopupContainer;
			}
		}

		[System.ComponentModel.Browsable(false)/*,System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)*/]
		public System.Windows.Forms.Control PopupControl
		{
			get
			{
				if(m_PopupMenu!=null)
					return m_PopupMenu;
				else if(m_PopupBar!=null)
					return m_PopupBar;
				else if(m_PopupContainer!=null)
					return m_PopupContainer;
				return null;
			}
		}

		protected override void OnDisplayedChanged()
		{
			if(!this.Displayed && this.Expanded)
                this.Expanded=false;				
		}

        internal virtual bool PopupPositionAdjusted
        {
            get { return m_PopupPositionAdjusted; }
            set
            {
                m_PopupPositionAdjusted = value;
            }
        }

		//****************************************
		// IMessageFilter Implementation
		//****************************************
		/*public bool PreFilterMessage(ref System.Windows.Forms.Message m)
		{
			// Block all the messages relating to the left mouse button.
			//if ((m.Msg >= NativeFunctions.WM_LBUTTONDOWN && m.Msg <= NativeFunctions.WM_LBUTTONDBLCLK) ||
			//	(m.Msg>=NativeFunctions.WM_NCLBUTTONDOWN && m.Msg<=NativeFunctions.WM_NCMBUTTONDBLCLK))
			if (m.Msg == NativeFunctions.WM_LBUTTONDOWN || m.Msg==NativeFunctions.WM_NCLBUTTONDOWN ||
				m.Msg==NativeFunctions.WM_RBUTTONDOWN || m.Msg==NativeFunctions.WM_MBUTTONDOWN || 
				m.Msg==NativeFunctions.WM_NCMBUTTONDOWN || m.Msg==NativeFunctions.WM_NCRBUTTONDOWN)
			{
				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
				// TODO: Add checking for popup child windows here !
				
				bool bChildHandle=this.IsAnyOnHandle(m.HWnd.ToInt32());

				if(!bChildHandle)
				{
					System.Windows.Forms.Control cTmp=System.Windows.Forms.Control.FromChildHandle(m.HWnd);
					if(cTmp!=null)
					{
						while(cTmp.Parent!=null)
							cTmp=cTmp.Parent;
                        bChildHandle=this.IsAnyOnHandle(cTmp.Handle.ToInt32());
					}
				}

				if(objCtrl!=null && m.HWnd!=objCtrl.Handle && !bChildHandle)
				{
					if(m_Parent!=null && !this.DesignMode)
					{
						if(this.Expanded)
						{
							m_Parent.AutoExpand=false;
						}
						else
						{
							m_Parent.AutoExpand=true;
						}
					}
					this.Expanded=!this.Expanded;
				}
				else if(objCtrl==null && !bChildHandle)
				{
					ClosePopup();
				}
			}
			else if(m.Msg==NativeFunctions.WM_ACTIVATEAPP && m.WParam.ToInt32()==0)
			{
				ClosePopup();
			}
			
			return false;
		}

		public void PostFilterMessage(ref System.Windows.Forms.Message m)
		{
		}*/
	}
}
