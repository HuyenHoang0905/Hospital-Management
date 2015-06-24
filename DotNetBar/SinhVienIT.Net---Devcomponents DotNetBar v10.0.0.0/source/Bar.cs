namespace DevComponents.DotNetBar
{
    using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;
	using System.Resources;
	using System.Drawing.Drawing2D;
    using System.ComponentModel.Design;
    using DevComponents.DotNetBar.Rendering;
    using System.Runtime.InteropServices;
    using System.Drawing.Text;

    /// <summary>
    ///  Represents bar control.
    /// </summary>
    [ToolboxBitmap(typeof(Bar), "Bar.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.BarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false), DefaultEvent("ItemClick")]
	public class Bar:Control, IDockInfo, IBarImageSize, IOwner, IOwnerMenuSupport,
		IMessageHandlerClient, ISupportInitialize, IBarDesignerServices,
        ICustomSerialization, IRenderingSupport, IAccessibilitySupport, IOwnerLocalize
	{
		#region Events Definitions
        /// <summary>
        /// Occurs when Item is clicked.
        /// </summary>
        [System.ComponentModel.Description("Occurs when Item is clicked."), Category("Item")]
        public event EventHandler ItemClick;
		/// <summary>
		/// Occurs after Bar is docked.
		/// </summary>
		[System.ComponentModel.Description("Occurs after Bar is docked.")]
		public event EventHandler BarDock;

		/// <summary>
		/// Occurs after Bar is undocked.
		/// </summary>
		[System.ComponentModel.Description("Occurs after Bar is undocked.")]
		public event EventHandler BarUndock;

		/// <summary>
		/// Occurs after Bar definition is loaded.
		/// </summary>
		[System.ComponentModel.Description("Occurs after Bar definition is loaded.")]
		public event EventHandler DefinitionLoaded;

		/// <summary>
		/// Occurs when current Dock tab has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when current Dock tab has changed.")]
		public event DotNetBarManager.DockTabChangeEventHandler DockTabChange;

		/// <summary>
		/// Occurs when bar visibility has changed as a result of user action.
		/// </summary>
		[System.ComponentModel.Description("Occurs when bar visibility has changed as a result of user action.")]
		public event EventHandler UserVisibleChanged;

		/// <summary>
		/// Occurs when bar auto hide state has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when bar auto hide state has changed.")]
		public event EventHandler AutoHideChanged;

		/// <summary>
		/// Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar.")]
		public event DotNetBarManager.BarClosingEventHandler Closing;

		/// <summary>
		/// Occurs when Bar in auto-hide state is about to be displayed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Bar in auto-hide state is about to be displayed.")]
		public event DotNetBarManager.AutoHideDisplayEventHandler AutoHideDisplay;

		/// <summary>
		/// Occurs when popup item is closing. Event is fired only when Bar is used independently of DotNetBarManager.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is closing."),Category("Item")]
		public event EventHandler PopupClose;

		/// <summary>
		/// Occurs when popup of type container is loading. Event is fired only when Bar is used independently of DotNetBarManager.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup of type container is loading."),Category("Item")]
		public event EventHandler PopupContainerLoad;

		/// <summary>
		/// Occurs when popup of type container is unloading. Event is fired only when Bar is used independently of DotNetBarManager.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup of type container is unloading."),Category("Item")]
		public event EventHandler PopupContainerUnload;

		/// <summary>
		/// Occurs when popup item is about to open.  Event is fired only when Bar is used independently of DotNetBarManager.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is about to open."),Category("Item")]
		public event DotNetBarManager.PopupOpenEventHandler PopupOpen;

		/// <summary>
		/// Occurs just before popup window is shown. Event is fired only when Bar is used independently of DotNetBarManager.
		/// </summary>
		[System.ComponentModel.Description("Occurs just before popup window is shown."),Category("Item")]
		public event EventHandler PopupShowing;
		
		/// <summary>
		/// Occurs before dock tab is displayed.
		/// </summary>
		[System.ComponentModel.Description("Occurs before dock tab is displayed.")]
		public event EventHandler BeforeDockTabDisplayed;

		/// <summary>
		/// Occurs when caption button is clicked. Caption button is button displayed on bars with grab handle style task pane.
		/// </summary>
		[System.ComponentModel.Description("Occurs when caption button is clicked on bars with grab handle style task pane.")]
		public event EventHandler CaptionButtonClick;

		/// <summary>
		/// Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.
		/// Event can be canceled by setting the Cancel property of event arguments to true. This even will occur only after user presses the
		/// X button on tab that is displaying the dockable windows/documents.
		/// </summary>
		[System.ComponentModel.Description("Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.")]
		public event DockTabClosingEventHandler DockTabClosing;

        /// <summary>
        /// Occurs on dockable bars after DockContainerItem is closed by end-user. This action cannot be cancelled.
        /// </summary>
        [System.ComponentModel.Description("Occurs on dockable bars after DockContainerItem is closed by end-user. This action cannot be cancelled.")]
        public event DockTabClosingEventHandler DockTabClosed;

        /// <summary>
        /// Occurs after an item has been serialized to XmlElement and provides you with opportunity to add any custom data
        /// to serialized XML. This allows you to serialize any data with the item and load it back up in DeserializeItem event.
        /// </summary>
        /// <remarks>
        /// 	<para>To serialize custom data to XML definition control creates handle this event and use CustomXmlElement
        /// property on SerializeItemEventArgs to add new nodes or set attributes with custom data you want saved.</para>
        /// </remarks>
        public event SerializeItemEventHandler SerializeItem;

        /// <summary>
        /// Occurs after an item has been de-serialized (load) from XmlElement and provides you with opportunity to load any custom data
        /// you have serialized during SerializeItem event.
        /// </summary>
        /// <remarks>
        /// 	<para>To de-serialize custom data from XML definition handle this event and use CustomXmlElement
        /// property on SerializeItemEventArgs to retrive any data you saved in SerializeItem event.</para>
        /// </remarks>
        public event SerializeItemEventHandler DeserializeItem;

		/// <summary>
		/// Occurs after the TabStrip style which used on dockable windows has changed. This event gives you opportunity to
		/// change the style of the tab strip by accessing Bar.DockTabControl.Style property.
		/// </summary>
		public event EventHandler TabStripStyleChanged;

        /// <summary>
        /// Occurs before the bar control is rendered. This event is fired once for each part of the bar control being rendered. Check the Part property of the event arguments to identify the part being rendered.
        /// You can cancel internal rendering by setting Cancel property.
        /// </summary>
        public event RenderBarEventHandler PreRender;

        /// <summary>
        /// Occurs after the bar control is rendered and allows you to render on top of the default rendering provided by the control.
        /// </summary>
        public event RenderBarEventHandler PostRender;

        /// <summary>
        /// Occurs when DotNetBar is looking for translated text for one of the internal text that are
        /// displayed on menus, toolbars and customize forms. You need to set Handled=true if you want
        /// your custom text to be used instead of the built-in system value.
        /// </summary>
        public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;
		#endregion

		#region System Buttons class
		private class System_Buttons
		{
			public System_Buttons(Rectangle closerect,Rectangle custrect,bool mouseoverclose,bool mouseovercustomize, bool mousednclose, bool mousedncust)
			{
				this.CloseButtonRect=closerect;
				this.CustomizeButtonRect=custrect;
				this.MouseOverClose=mouseoverclose;
				this.MouseOverCustomize=mouseovercustomize;
				this.MouseDownClose=mousednclose;
				this.MouseDownCustomize=mousedncust;
				this.MouseOverAutoHide=false;
				this.MouseDownAutoHide=false;
				this.AutoHideButtonRect=Rectangle.Empty;
			}
			public event EventHandler MouseOverCloseChanged;
			public event EventHandler MouseOverCustomizeChanged;
			public event EventHandler MouseOverAutoHideChanged;
			public event EventHandler MouseDownCloseChanged;
			public event EventHandler MouseDownCustomizeChanged;
			public event EventHandler MouseDownAutoHideChanged;
			public event EventHandler MouseOverCaptionChanged;
			public event EventHandler MouseDownCaptionChanged;
			public Rectangle CloseButtonRect;
			public Rectangle CustomizeButtonRect;
			public Rectangle AutoHideButtonRect;
			public Rectangle CaptionButtonRect;
			public System.Drawing.Size ButtonSize=new Size(14,14);
			private bool m_MouseOverClose=false;
			private bool m_MouseOverCustomize=false;
			private bool m_MouseDownClose=false;
			private bool m_MouseDownCustomize=false;
			private bool m_MouseOverAutoHide=false;
			private bool m_MouseDownAutoHide=false;
			private bool m_MouseOverCaption=false;
			private bool m_MouseDownCaption=false;

			public bool MouseOverClose
			{
				get{return m_MouseOverClose;}
				set
				{
					if(m_MouseOverClose!=value)
					{
						m_MouseOverClose=value;
						if(MouseOverCloseChanged!=null)
							MouseOverCloseChanged(this,new EventArgs());
					}
				}
			}
			public bool MouseOverCaption
			{
				get{return m_MouseOverCaption;}
				set
				{
					if(m_MouseOverCaption!=value)
					{
						m_MouseOverCaption=value;
						if(MouseOverCaptionChanged!=null)
							MouseOverCaptionChanged(this,new EventArgs());
					}
				}
			}
			public bool MouseOverCustomize
			{
				get{return m_MouseOverCustomize;}
				set
				{
					if(m_MouseOverCustomize!=value)
					{
						m_MouseOverCustomize=value;
						if(MouseOverCustomizeChanged!=null)
							MouseOverCustomizeChanged(this,new EventArgs());
					}
				}
			}
			public bool MouseDownClose
			{
				get{return m_MouseDownClose;}
				set
				{
					if(m_MouseDownClose!=value)
					{
						m_MouseDownClose=value;
						if(MouseDownCloseChanged!=null)
							MouseDownCloseChanged(this,new EventArgs());
					}
				}
			}
			public bool MouseDownCaption
			{
				get{return m_MouseDownCaption;}
				set
				{
					if(m_MouseDownCaption!=value)
					{
						m_MouseDownCaption=value;
						if(MouseDownCaptionChanged!=null)
							MouseDownCaptionChanged(this,new EventArgs());
					}
				}
			}
			public bool MouseDownCustomize
			{
				get{return m_MouseDownCustomize;}
				set
				{
					if(m_MouseDownCustomize!=value)
					{
						m_MouseDownCustomize=value;
						if(MouseDownCustomizeChanged!=null)
							MouseDownCustomizeChanged(this,new EventArgs());
					}
				}
			}
			public bool MouseOverAutoHide
			{
				get{return m_MouseOverAutoHide;}
				set
				{
					if(m_MouseOverAutoHide!=value)
					{
						m_MouseOverAutoHide=value;
						if(MouseOverAutoHideChanged!=null)
							MouseOverAutoHideChanged(this,new EventArgs());
					}
				}
			}
			public bool MouseDownAutoHide
			{
				get{return m_MouseDownAutoHide;}
				set
				{
					if(m_MouseDownAutoHide!=value)
					{
						m_MouseDownAutoHide=value;
						if(MouseDownAutoHideChanged!=null)
							MouseDownAutoHideChanged(this,new EventArgs());
					}
				}
			}
		}
		#endregion

		#region Constants
		private struct ThemeMargin
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
			public bool IsEmpty
			{
				get
				{ return (Left==0 && Top==0 && Right==0 && Bottom==0);}
			}
		}

		const int WM_MOUSEACTIVATE = 0x21;
		const int MA_NOACTIVATE = 3;
		const int MA_NOACTIVATEANDEAT = 4;
		const long WS_POPUP=0x80000000L;
		const long WS_CLIPSIBLINGS=0x04000000L;
		const long WS_CLIPCHILDREN=0x02000000L;
		const long WS_EX_TOPMOST=0x00000008L;
		const long WS_EX_TOOLWINDOW=0x00000080L;
		const long WS_VISIBLE=0x10000000L;
		const int WM_WINDOWPOSCHANGING=0x0046;
		const int WM_WINDOWPOSCHANGED=0x0047;

		const int SIZE_W=1;
		const int SIZE_E=2;
		const int SIZE_N=3;
		const int SIZE_S=4;
		const int SIZE_NWN=5;
		const int SIZE_NWS=6;
		const int SIZE_NEN=7;
		const int SIZE_NES=8;
		const int SIZE_HSPLITRIGHT=9;
		const int SIZE_HSPLITLEFT=10;
		const int SIZE_VSPLITTOP=11;
		const int SIZE_VSPLITBOTTOM=12;
		const int SIZE_HSPLIT=13;
		const int SIZE_VSPLIT=14;
		const int SIZE_PARENTRESIZE=15;

		const int DRAGRECTANGLE_WIDTH=3;

		const int GrabHandleDotNetWidth=7;
		const int GrabHandleOfficeWidth=7;
		const int GrabHandleResizeWidth=17;
		const int GrabHandleTaskPaneHeight=23;
        const int GrabHandleCaptionHeight = 20;

		const int DOCKTABSTRIP_HEIGHT=25;
		#endregion
		
		#region Private Variables
		private BaseItem m_ParentItem;
		private Point m_ParentItemScreenPos;
		private object m_OldContainer;
		private Rectangle m_ClientRect;
		private SideBarImage m_SideBarImage;
		private Rectangle m_SideBarRect;
		private GenericItemContainer m_ItemContainer;
		//private BaseItem m_OldParent;
		private int m_InitialContainerWidth;
		private eBarState m_BarState;
		private eGrabHandleStyle m_GrabHandleStyle;
		private Rectangle m_GrabHandleRect;

		// IDockInfo members
		private int m_DockOffset;
		private int m_DockLine;

		private object m_Owner;
		private Point m_MouseDownPt;
		private Size m_MouseDownSize;
		private bool m_MoveWindow;
		private int m_DockTabTearOffIndex=-1; // Used if dock tab is being torn-off but bar cannot float so processing needs to be done after the drop is complete

		private Rectangle m_FloatingRect;
		private Size m_DockedSizeH=Size.Empty,m_DockedSizeV=Size.Empty;
		private int m_SizeWindow;
		// Used when show window content when dragging is not set
		private DockSiteInfo m_DragDockInfo = new DockSiteInfo();
		private Rectangle m_LastDragRect;
		internal DockSiteInfo m_LastDockSiteInfo=new DockSiteInfo();
		private bool m_WrapItemsDock;
		private bool m_WrapItemsFloat;
		private bool m_MenuBar;
		private bool m_DockStretch;
		private FloatingContainer m_Float;
		private bool m_DockingInProgress;
		private int m_LastFocusWindow;
		private bool m_CanDockLeft=true, m_CanDockRight=true, m_CanDockTop=true, m_CanDockBottom=true, m_CanUndock, m_CanTearOffTabs=true, m_CanReorderTabs=true;
		private bool m_CanDockTab=true;
		private bool m_CanDockDocument=false;
		private bool m_CanHide=false;
        private bool m_CloseSingleTab = false;
		private bool m_AcceptDropItems=true;
		private eBorderType m_DockedBorder=eBorderType.None;
		private Color m_SingleLineColor=SystemColors.ControlDark;
		private bool m_CustomBar=false;
		private System_Buttons m_SystemButtons=new System_Buttons(Rectangle.Empty,Rectangle.Empty,false,false,false,false);
		private ePopupAnimation m_PopupAnimation=ePopupAnimation.ManagerControlled;
		private PopupShadow m_DropShadow=null;
		private eBarImageSize m_ImageSize=eBarImageSize.Default;
		private Color m_CaptionBackColor=Color.Empty;
		private Color m_CaptionForeColor=Color.Empty;
		private PopupItem m_CustomizeMenu=null;
		private TabStrip m_TabDockItems=null;
		private bool m_AutoHideState=false, m_CanAutoHide=true;
		private bool m_HasFocus=false;
		private bool m_SystemPrefChanged=false, m_CustomFont=false;
		private bool m_LockDockPosition=false;
		private ColorScheme m_ColorScheme=null;
		internal bool PassiveBar=false;
		private bool m_ThemeAware=false;
		private ThemeMargin m_ThemeWindowMargins=new ThemeMargin();
		//private int m_MinClientSize=64;  // Minimum Parent Dockable size

		// Theme Caching Support
		private ThemeWindow m_ThemeWindow=null;
		private ThemeRebar m_ThemeRebar=null;
		private ThemeToolbar m_ThemeToolbar=null;
		private ThemeHeader m_ThemeHeader=null;
		private ThemeScrollBar m_ThemeScrollBar=null;
		private ThemeProgress m_ThemeProgress=null;

		// Support for same line docked windows
//		internal int _SplitDockWidth=0;
//		internal int _SplitDockHeight=0;
		private int m_SplitDockWidthPercent=0;
		private int m_SplitDockHeightPercent=0;

		// While docking is in progress this will hold the tabbed bar that this bar was added to if any
		private Bar m_TempTabBar=null;

		// Tabbed Dockable Windows Tab Control position
		private eTabStripAlignment m_DockTabAlignment=eTabStripAlignment.Bottom;

		// TODO: Menu Merge Implementation  - Item Merge Support
		//private bool m_MergeEnabled=false;
		private bool m_HideFloatingInactive=true;

		private Point m_ResizeOffset=Point.Empty;
		private bool m_TabNavigation=false;

		internal bool m_AccessibleObjectCreated=false;

		private bool m_BarDefinitionLoading=false;
		private bool m_ParentMsgHandlerRegistered=false;

		private BaseItem m_DoDefaultActionItem=null;

		private int m_AutoHideAnimationTime=100;

		private eBackgroundImagePosition m_BackgroundImagePosition=eBackgroundImagePosition.Stretch;
		private byte m_BackgroundImageAlpha=255;

		private bool m_ShowToolTips=true;
		private bool m_IgnoreAnimation=true;

		protected ToolTip m_ToolTipWnd=null;

		private int m_DockSideDelayed=-1;

		private bool m_AnimationInProgress=false;

		private bool m_EnableRedraw=true;

		private bool m_AlwaysDisplayDockTab=false;
		private bool m_MenuEventSupport=false;
		private bool m_MenuFocus=false;

		private int m_BarShowIndex=-1; // Used to control bar Z-Order if WinForms change it...
		private bool m_AlwaysDisplayKeyAccelerators=false;

		private bool m_AutoCreateCaptionMenu=true;
		private PopupItem m_CaptionMenu=null;
		private bool m_AutoSyncBarCaption=false;

		private bool m_SaveLayoutChanges=true;
		private bool m_TabsRearranged=false;

		private IBarItemDesigner m_BarDesigner=null;
		private bool m_LoadingHideFloating=false;
        private bool m_DesignerSelection = false;
		internal Hashtable PropertyBag=new Hashtable();

        private int m_CornerSize = 3;
        private bool m_FadeEffect = false;
        private bool m_AntiAlias = false;
        private eBarType m_BarType = eBarType.Toolbar;
        private bool m_RoundCorners = true;
        private bool m_AutoHideTextAlwaysVisible = false;
        private bool m_DockTabCloseButtonVisible = false;
		#endregion

		/// <summary>
		/// Initializes a new instance of the Bar class.
		/// </summary>
		public Bar()
		{
			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}

			m_ColorScheme=new ColorScheme();
			m_ParentItem=null;
			m_OldContainer=null;
			m_ClientRect=Rectangle.Empty;
			m_SideBarRect=Rectangle.Empty;
			m_SideBarImage=new SideBarImage();
			m_ItemContainer=new GenericItemContainer();
			m_ItemContainer.GlobalItem=false;
			m_ItemContainer.ContainerControl=this;
			m_ItemContainer.WrapItems=true;
			m_ItemContainer.Stretch=false;
			m_ItemContainer.Displayed=true;
			m_ItemContainer.SystemContainer=true;
			//m_ItemContainer.SetSystemItem(true);
			//m_OldParent=null;
			m_InitialContainerWidth=164;
			//m_BarState=eBarState.Popup;
			m_BarState=eBarState.Docked;

			m_DockOffset=0;
			m_DockLine=0;
			m_GrabHandleStyle=eGrabHandleStyle.None;
			m_GrabHandleRect=Rectangle.Empty;
			m_Owner=null;
			m_MouseDownPt=Point.Empty;
			m_MouseDownSize=new Size(0,0);
			m_MoveWindow=false;
			m_FloatingRect=Rectangle.Empty;
			m_SizeWindow=0;

			m_LastDragRect=Rectangle.Empty;

			m_WrapItemsDock=false;
			m_WrapItemsFloat=true;
			m_DockStretch=false;
			m_MenuBar=false;
			this.Text="";
			
			m_Float=null;
			m_DockingInProgress=false;

			this.SetStyle(ControlStyles.Selectable,false);
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			
			this.TabStop=false;
			base.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;

			m_CanDockLeft=true;
			m_CanDockRight=true;
			m_CanDockTop=true;
			m_CanDockBottom=true;
			m_CanUndock=true;

			Microsoft.Win32.SystemEvents.UserPreferenceChanged+=new Microsoft.Win32.UserPreferenceChangedEventHandler(PreferenceChanged);

			this.IsAccessible=true;

			m_SystemButtons.MouseDownAutoHideChanged+=new EventHandler(this.SysButtonHideTooltip);
			m_SystemButtons.MouseDownCloseChanged+=new EventHandler(this.SysButtonHideTooltip);
			m_SystemButtons.MouseDownCustomizeChanged+=new EventHandler(this.SysButtonHideTooltip);
			m_SystemButtons.MouseOverAutoHideChanged+=new EventHandler(this.SysButtonMouseOverAutoHide);
			m_SystemButtons.MouseOverCustomizeChanged+=new EventHandler(this.SysButtonMouseOverCustomize);
			m_SystemButtons.MouseOverCloseChanged+=new EventHandler(this.SysButtonMouseOverClose);
		}

		private void PreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
		{
			if(!m_SystemPrefChanged && !m_CustomFont)
			{
				m_SystemPrefChanged=true;
				NativeFunctions.PostMessage(this.Handle.ToInt32(),NativeFunctions.WM_USER+102,0,0);
			}
		}

		protected override AccessibleObject CreateAccessibilityInstance()
		{
            return new BarAccessibleObject(this);
		}

		private void SetupAccessibility()
		{
			if(this.Text!="")
				this.AccessibleName=this.Text;
			else
				this.AccessibleName="DotNetBar Bar";
			this.AccessibleDescription=this.AccessibleName+" ("+this.Name+")";
			if(this.MenuBar)
				this.AccessibleRole=AccessibleRole.MenuBar;
			else
			{
				if(this.GrabHandleStyle==eGrabHandleStyle.ResizeHandle)
					this.AccessibleRole=AccessibleRole.StatusBar;
				else
					this.AccessibleRole=AccessibleRole.ToolBar;
			}
		}

		/// <summary>
		/// Initializes a new instance of the Control class.
		/// </summary>
		/// <param name="BarCaption">Bar Caption</param>
		public Bar(string BarCaption):this()
		{
			this.Text=BarCaption;
		}

		/// <summary>
		/// Gets/Sets the owner of the Bar object.
		/// </summary>
		[System.ComponentModel.Browsable(false),DefaultValue(null),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Owner
		{
			get
			{
                if (m_ParentItem is CustomizeItem)
                    return m_ParentItem.GetOwner();
				if(this.Parent!=null && !(this.Parent is FloatingContainer) && !(this.Parent is DockSite) && !this.AutoHide && !m_DesignerParent || this.Parent == null && m_Owner == null)
					return this;
				return m_Owner;
			}
			set
			{
				m_Owner=value;
				if(this.DesignMode && this.Site!=null && m_TabDockItems!=null)
					this.RefreshDockTab(false);
			}
		}
		internal bool IsDisposing=false;
		protected override void Dispose(bool disposing)
		{
			IsDisposing=true;
			if(m_ParentMsgHandlerRegistered)
			{
				DotNetBarManager.UnRegisterOwnerParentMsgHandler(this,null);
				m_ParentMsgHandlerRegistered=false;
			}
            if (m_Float != null && disposing)
            {
                try
				{
                    if (m_Float.Controls.Contains(this))
                        m_Float.Controls.Remove(this);
                    m_Float.Close();
                    m_Float.Dispose();
                    m_Float = null;
                }
                catch (Exception)
                { }
            }
			
			if(m_Owner is DotNetBarManager && ((DotNetBarManager)m_Owner).IgnoreLoadedControlDispose)
				this.Controls.Clear();

			if(this.Parent!=null)
			{
				try
				{
					this.Parent.Controls.Remove(this);
				}
				catch(Exception)
				{}
			}
			Microsoft.Win32.SystemEvents.UserPreferenceChanged-=new Microsoft.Win32.UserPreferenceChangedEventHandler(PreferenceChanged);
			if(m_TabDockItems!=null)
			{
				m_TabDockItems.Dispose();
				m_TabDockItems=null;
			}
			if(m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
			if(m_FilterInstalled)
			{
				MessageHandler.UnregisterMessageClient(this);
				m_FilterInstalled=false;
			}

			RestoreContainer();
			m_Owner=null;
			m_ParentItem=null;
			m_OldContainer=null;
			//m_OldParent=null;
			if(m_ItemContainer!=null)
				m_ItemContainer.Dispose();
			m_ItemContainer=null;

			base.Dispose(disposing);
			IsDisposing=false;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams p=base.CreateParams;
				if(PassiveBar)
					return p;
				if(m_BarState==eBarState.Popup && (m_ParentItem==null || (m_ParentItem.Site == null || !m_ParentItem.Site.DesignMode)))
				{
					p.Style=unchecked((int)(WS_POPUP | WS_CLIPSIBLINGS | WS_CLIPCHILDREN));
					p.ExStyle=(int)(WS_EX_TOPMOST | WS_EX_TOOLWINDOW);
				}
				//else if(m_BarState==eBarState.Floating)
				//{
				//	p.style=(int)(WS_POPUP | WS_CLIPSIBLINGS | WS_CLIPCHILDREN);
				//	p.exStyle=(int)(WS_EX_TOOLWINDOW);
				//}
				p.Caption="";
				return p;
			}
		}

        internal bool HasFocus
        {
            get { return m_HasFocus; }
        }

		internal void SetHasFocus(bool b)
		{
			m_HasFocus=b;
            if (this.DockSide == eDockSide.None && this.LayoutType == eLayoutType.DockContainer && this.Owner is DotNetBarManager)
            {
                DotNetBarManager manager = this.Owner as DotNetBarManager;
                if (b)
                    manager.InternalDockContainerActivated(this.SelectedDockContainerItem);
                else
                    manager.InternalDockContainerDeactivated(this.SelectedDockContainerItem);
            }
			this.Refresh();
		}

        protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			if(m_HasFocus)
			{
				m_HasFocus=false;
                if (m_GrabHandleStyle == eGrabHandleStyle.Caption && (m_BarState == eBarState.Docked || m_BarState == eBarState.AutoHide))
                    this.Invalidate();

				if(m_BarState==eBarState.AutoHide)
				{
					GetAutoHidePanel(m_LastDockSiteInfo.DockSide).StartTimer();
				}
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if(!m_HasFocus)
			{
				m_HasFocus=true;
                if (m_GrabHandleStyle == eGrabHandleStyle.Caption && (m_BarState == eBarState.Docked || m_BarState == eBarState.AutoHide))
                    this.Invalidate();
				if(m_BarState==eBarState.AutoHide)
				{
					Point p=this.PointToClient(Control.MousePosition);
					if(this.ClientRectangle.Contains(p))
						GetAutoHidePanel(m_LastDockSiteInfo.DockSide).StopTimer();
				}
			}
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if(keyData==System.Windows.Forms.Keys.Left || keyData==System.Windows.Forms.Keys.Right || keyData==System.Windows.Forms.Keys.Up || keyData==System.Windows.Forms.Keys.Down || keyData==System.Windows.Forms.Keys.Enter || keyData==System.Windows.Forms.Keys.Return || keyData==System.Windows.Forms.Keys.Tab || keyData==System.Windows.Forms.Keys.Escape)
				return true;
			return base.IsInputKey(keyData);
		}

		internal bool EnableRedraw
		{
			get
			{
				return m_EnableRedraw;
			}
			set
			{
				if(m_EnableRedraw!=value && this.Visible)
				{
					m_EnableRedraw=value;
					if(m_EnableRedraw)
						NativeFunctions.SendMessage(this.Handle,NativeFunctions.WM_SETREDRAW,1,0);
					else
						NativeFunctions.SendMessage(this.Handle,NativeFunctions.WM_SETREDRAW,0,0);
				}
			}
		}

#if FRAMEWORK20
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
            if (m_ItemContainer != null)
                m_ItemContainer.UpdateBindings();
        }
#endif

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_MOUSEACTIVATE && (m_BarState==eBarState.Popup || m_BarState==eBarState.Floating && m_ItemContainer.LayoutType==eLayoutType.TaskList))// && m_BarState!=eBarState.Docked)
			{
				m.Result=new System.IntPtr(MA_NOACTIVATE);
				return;
			}
			else if(m.Msg==NativeFunctions.WM_USER+101)
			{
				if(!m_DockingInProgress && m_BarState==eBarState.Floating)
					NativeFunctions.SetWindowPos(this.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE | NativeFunctions.SWP_NOACTIVATE);
			}
			else if(m.Msg==NativeFunctions.WM_USER+102)
			{
				if(this.Name=="StatusBar")
					this.Name=this.Name;
                if (this.LayoutType != eLayoutType.DockContainer)
                    this.Font = System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
				this.RecalcLayout();
				m_SystemPrefChanged=false;
			}
			else if(m.Msg==NativeFunctions.WM_USER+107)
			{
				if(m_DoDefaultActionItem!=null)
				{
					m_DoDefaultActionItem.DoAccesibleDefaultAction();
					m_DoDefaultActionItem=null;
				}

			}
			else if(m.Msg==NativeFunctions.WM_SETFOCUS)
			{
				int wnd=m.WParam.ToInt32();
				Control objCtrl=Control.FromChildHandle(new IntPtr(wnd));
				if(objCtrl!=null && !(objCtrl is DevComponents.DotNetBar.Bar) && !(objCtrl is MenuPanel) && !(objCtrl is Controls.TextBoxX))
				{
					Form form=objCtrl.FindForm();
					if(form==this.FindForm())
						m_LastFocusWindow=m.WParam.ToInt32();
					else if(form!=null)
						m_LastFocusWindow=m.WParam.ToInt32();
				}
			}
			else if(m.Msg==NativeFunctions.WM_KILLFOCUS)
			{
				bool bLostFocus=false;
				Control objCtrl=Control.FromChildHandle(m.WParam);
				if(objCtrl!=null)
				{
					while(objCtrl.Parent!=null)
						objCtrl=objCtrl.Parent;
					if(objCtrl!=this && (m_ItemContainer!=null && !m_ItemContainer.IsAnyOnHandle(objCtrl.Handle.ToInt32())))
						bLostFocus=true;
				}
				else
					bLostFocus=true;

				if(bLostFocus && m_ItemContainer!=null)
					m_ItemContainer.ContainerLostFocus(false);
			}
			else if(m.Msg==NativeFunctions.WM_THEMECHANGED)
			{
				this.RefreshThemes();
				this.RefreshThemeMargins();
				Themes.RefreshIsThemeActive();
			}
            //else if (m.Msg == (int)WinApi.WindowsMessages.WM_NCHITTEST)
            //{
            //    Form form = this.FindForm();
            //    Rectangle resizeRect = GetResizeHandleRectangle();
            //    if (form != null && form.WindowState == FormWindowState.Normal && !resizeRect.IsEmpty)
            //    {
            //        int mouseX = WinApi.LOWORD(m.LParam);
            //        int mouseY = WinApi.HIWORD(m.LParam);
            //        if (resizeRect.Contains(this.PointToClient(new Point(mouseX, mouseY))))
            //        {
            //            IntPtr formHandle = form.Handle;
            //            if (formHandle != IntPtr.Zero)
            //            {
            //                WinApi.POINT point1;
            //                WinApi.RECT rect1 = new WinApi.RECT(0, 0, form.ClientRectangle.Right, form.ClientRectangle.Bottom);
            //                if (this.RightToLeft == RightToLeft.Yes)
            //                {
            //                    point1 = new WinApi.POINT(resizeRect.Left, resizeRect.Bottom);
            //                }
            //                else
            //                {
            //                    point1 = new WinApi.POINT(resizeRect.Right, resizeRect.Bottom);
            //                }
            //                WinApi.MapWindowPoints(this.Handle, formHandle, ref point1, 1);
            //                int num3 = Math.Abs((int)(rect1.Bottom - point1.y));
            //                int num4 = Math.Abs((int)(rect1.Right - point1.x));
            //                if (this.RightToLeft != RightToLeft.Yes)
            //                {
            //                    m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.BottomRightSizeableCorner);
            //                    return;
            //                }
            //            }
            //        }

            //    }   
            //}
			base.WndProc(ref m);
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			Application.DoEvents();
			//m_ColorScheme.Refresh(null,true);
			this.GetColorScheme().Refresh(null,true);
			this.Refresh();
		}

        /// <summary>
        /// Gets or sets the item default accesibility action will be performed on.
        /// </summary>
        BaseItem IAccessibilitySupport.DoDefaultActionItem
		{
			get {return m_DoDefaultActionItem;}
			set {m_DoDefaultActionItem=value;}
		}

		/// <summary>
		/// Releases the focus from the bar and selects the control that had focus before bar was selected. If control that had focus could not be determined focus will stay on the bar.
        /// This method is used by internal DotNetBar implementation and you should not use it.
		/// </summary>
		public void ReleaseFocus()
		{
			if(this.Focused && m_LastFocusWindow!=0)
			{
				Control ctrl=Control.FromChildHandle(new System.IntPtr(m_LastFocusWindow));
				if(ctrl!=null)
				{
					ctrl.Select();
					if(!ctrl.Focused)
						NativeFunctions.SetFocus(m_LastFocusWindow);
				}
				else
				{
					NativeFunctions.SetFocus(m_LastFocusWindow);
				}
				m_LastFocusWindow=0;
				m_ItemContainer.AutoExpand=false;
			}
		}

		/// <summary>
		/// Returns the reference to the control that last had input focus. This property should be used to
		/// determine which control had input focus before bar gained the focus. Use it to apply
		/// the menu command to active control.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control LastFocusControl
		{
			get
			{
				if(this.Focused && m_LastFocusWindow!=0)
				{
					Control ctrl=Control.FromChildHandle(new System.IntPtr(m_LastFocusWindow));
					return ctrl;
				}
				return null;
			}
		}

		internal void SetSystemFocus()
		{
			if(!this.Focused)
			{
				m_ItemContainer.SetSystemFocus();
				this.Focus();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if(m_ItemContainer==null || this.IsDisposed)
				return;

            if (this.BackColor == Color.Transparent)
            {
                base.OnPaintBackground(e);
            }

			Graphics g=e.Graphics;
			g.PageUnit=GraphicsUnit.Pixel;
			// Support for the design-time when insert position is drawn
			#if TRIAL
				if(NativeFunctions.ColorExpAlt())
				{
					g.Clear(Color.White);
					g.DrawString("Your DotNetBar Trial has expired.",this.Font,SystemBrushes.ControlText,0,0);
				}
				else
				{
					if(m_ItemContainer.EffectiveStyle==eDotNetBarStyle.Office2000)
						PaintOffice(g);
					else
						PaintDotNet(e);
				}
            #else
            if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
					PaintOffice(g);
				else
					PaintDotNet(e);
			#endif
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if(m_AnimationInProgress || this.Width==0 || this.Height==0)
				return;

			if(!(this.Parent is DockSite || this.Parent is FloatingContainer) && m_BarState==eBarState.Docked && !m_LayoutSuspended)
				this.RecalcLayout();
			ResizeDockTab();
		}

		private void PaintBackgroundImage(Graphics g)
		{
			if(this.BackgroundImage==null)
				return;
			Rectangle r=this.ClientRectangle;
			BarFunctions.PaintBackgroundImage(g,r,this.BackgroundImage,m_BackgroundImagePosition,m_BackgroundImageAlpha);
		}

        private bool IsGradientStyle
        {
            get
            {
                return (this.Style == eDotNetBarStyle.Office2003 || this.Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.Style));
            }
        }

        /// <summary>
        /// Creates the Graphics object for the control.
        /// </summary>
        /// <returns>The Graphics object for the control.</returns>
        public new Graphics CreateGraphics()
        {
            Graphics g = base.CreateGraphics();
            if (m_AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#if FRAMEWORK20
                if (!SystemInformation.IsFontSmoothingEnabled)
#endif
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
            return g;
        }

        #region Rendering Support
        private Rendering.BaseRenderer m_DefaultRenderer = null;
        private Rendering.BaseRenderer m_Renderer = null;
        private eRenderMode m_RenderMode = eRenderMode.Global;
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public virtual Rendering.BaseRenderer GetRenderer()
        {
            if (m_RenderMode == eRenderMode.Global && Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            else if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
                return m_Renderer;

            if (m_DefaultRenderer == null)
            {
                if (BarFunctions.IsOffice2007Style(this.Style))
                    m_DefaultRenderer = new Rendering.Office2007Renderer();
            }

            return m_Renderer;
        }

        /// <summary>
        /// Gets or sets the redering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
        /// also be set to the custom renderer that will be used.
        /// </summary>
        [Browsable(false), DefaultValue(eRenderMode.Global)]
        public eRenderMode RenderMode
        {
            get { return m_RenderMode; }
            set
            {
                if (m_RenderMode != value)
                {
                    m_RenderMode = value;
                    this.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom renderer used by the items on this control. RenderMode property must also be set to eRenderMode.Custom in order renderer
        /// specified here to be used.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DevComponents.DotNetBar.Rendering.BaseRenderer Renderer
        {
            get
            {
                return m_Renderer;
            }
            set { m_Renderer = value; }
        }
        #endregion

        internal ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(m_Owner as IOwner, this, g, this.GetColorScheme());
            pa.DesignerSelection = m_DesignerSelection;
            pa.Renderer = GetRenderer();

            if (m_BarState==eBarState.Popup && m_ParentItem!=null && m_ParentItem.DesignMode)
            {
                ISite site = this.GetSite();
                if (site!=null && site.DesignMode)
                    pa.DesignerSelection = true;
            }

            return pa;
        }

		protected void PaintDotNet(PaintEventArgs e)
		{
            Graphics g = e.Graphics;
            SmoothingMode sm = g.SmoothingMode;
            TextRenderingHint th = g.TextRenderingHint;
            RenderBarEventArgs re = null;

            if (m_AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
            
			m_SystemButtons.CustomizeButtonRect=Rectangle.Empty;
			m_SystemButtons.CloseButtonRect=Rectangle.Empty;
			m_SystemButtons.AutoHideButtonRect=Rectangle.Empty;
			m_SystemButtons.CaptionButtonRect=Rectangle.Empty;
			ColorScheme cs=this.GetColorScheme();

            ItemPaintArgs pa = GetItemPaintArgs(g);
            pa.ClipRectangle = e.ClipRectangle;
			Pen p;
			if(m_BarState==eBarState.Popup)
				p=new Pen(pa.Colors.BarPopupBorder,1);
			else
				p=new Pen(pa.Colors.BarFloatingBorder);

            if (this.DisplayShadow && !this.AlphaShadow && m_ParentItem != null && !BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
				SetupRegion();
			
			if(m_BarState==eBarState.Popup)
			{
                re = new RenderBarEventArgs(this, g, eBarRenderPart.Background, this.ClientRectangle);
                OnPreRender(re);
                if(!re.Cancel)
                {
                    if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && GetRenderer() != null)
                    {
                        ToolbarRendererEventArgs tre = new ToolbarRendererEventArgs(this, g, this.DisplayRectangle);
                        tre.ItemPaintArgs = pa;
                        GetRenderer().DrawPopupToolbarBackground(tre);
                    }
                    else
                    {
                        using (SolidBrush brush = new SolidBrush(pa.Colors.BarPopupBackground))
                            g.FillRectangle(brush, this.DisplayRectangle);
                        PaintBackgroundImage(pa.Graphics);

                        PaintSideBar(g);

                        Rectangle borderRectangle = this.ClientRectangle;
                        if (this.DisplayShadow && !this.AlphaShadow)
                            borderRectangle = new Rectangle(0, 0, this.ClientSize.Width - 2, this.ClientSize.Height - 2);

                        if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
                            DisplayHelp.DrawRoundedRectangle(g, p, borderRectangle, m_CornerSize);
                        else
                            NativeFunctions.DrawRectangle(g, p, borderRectangle);

                        if (this.DisplayShadow && !this.AlphaShadow)
                        {
                            // Shadow
                            p.Dispose();
                            p = new Pen(SystemColors.ControlDark, 2);
                            Point[] pt = new Point[3];
                            pt[0].X = 2;
                            pt[0].Y = this.ClientSize.Height - 1;
                            pt[1].X = this.ClientSize.Width - 1;
                            pt[1].Y = this.ClientSize.Height - 1;
                            pt[2].X = this.ClientSize.Width - 1;
                            pt[2].Y = 2;
                            g.DrawLines(p, pt);
                        }
                        if (m_ParentItem != null && !BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && m_ParentItem is ButtonItem && m_ParentItem.Displayed)
                        {
                            // Determine where to draw the line based on parent position
                            if (m_ParentItemScreenPos.Y < this.Location.Y)
                            {
                                Point p1 = new Point((m_ParentItemScreenPos.X - this.Location.X) + 1, 0);
                                Point p2 = new Point(p1.X + m_ParentItem.WidthInternal - 5, 0);
                                DisplayHelp.DrawLine(g, p1, p2, pa.Colors.ItemExpandedBackground, 1);
                                //g.DrawLine(new Pen(pa.Colors.ItemExpandedBackground, 1), p1, p2);
                            }
                        }
                    }
                }
			}
            else if (m_BarState == eBarState.Floating)
            {
                bool drawCaptionText = true;

                re = new RenderBarEventArgs(this, g, eBarRenderPart.Background, this.ClientRectangle);
                OnPreRender(re);
                if (!re.Cancel)
                {
                    if (BarFunctions.IsOffice2007Style(this.Style) && this.GetRenderer() != null)
                    {
                        ToolbarRendererEventArgs tre = new ToolbarRendererEventArgs(this, g, this.DisplayRectangle);
                        tre.ItemPaintArgs = pa;
                        this.GetRenderer().DrawToolbarBackground(tre);
                    }
                    else
                    {
                        sm = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.Default;
                        if (this.MenuBar)
                            DisplayHelp.FillRectangle(g, this.ClientRectangle, pa.Colors.MenuBarBackground, pa.Colors.MenuBarBackground2, pa.Colors.MenuBarBackgroundGradientAngle);
                        else
                            DisplayHelp.FillRectangle(g, this.ClientRectangle, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);

                        PaintBackgroundImage(pa.Graphics);
                        g.SmoothingMode = sm;
                    }
                }

                Rectangle r = new Rectangle(0, 0, this.Width, this.Height);

                ThemeWindow theme = null;
                ThemeWindowParts part = ThemeWindowParts.SmallFrameLeft;
                ThemeWindowStates state = ThemeWindowStates.FrameActive;
                
                re = new RenderBarEventArgs(this, g, eBarRenderPart.Caption, Rectangle.Empty);
                
                if (this.DrawThemedCaption)
                {
                    re.Bounds = new Rectangle(0, 0, this.Width, m_ThemeWindowMargins.Top + 1);
                    OnPreRender(re);
                    if (!re.Cancel)
                    {
                        theme = this.ThemeWindow;
                        if (this.LayoutType == eLayoutType.DockContainer && !m_HasFocus)
                            state = ThemeWindowStates.FrameInactive;

                        theme.DrawBackground(g, part, state, new Rectangle(0, 0, m_ThemeWindowMargins.Left, this.Height));
                        part = ThemeWindowParts.SmallFrameRight;
                        theme.DrawBackground(g, part, state, new Rectangle(this.Width - m_ThemeWindowMargins.Right, 0, m_ThemeWindowMargins.Left, this.Height));
                        part = ThemeWindowParts.SmallFrameBottom;
                        theme.DrawBackground(g, part, state, new Rectangle(0, this.Height - m_ThemeWindowMargins.Bottom, this.Width, m_ThemeWindowMargins.Bottom));

                        if (this.LayoutType == eLayoutType.DockContainer && !m_HasFocus)
                            state = ThemeWindowStates.CaptionInactive;
                        part = ThemeWindowParts.SmallCaption;
                    }
                    r = new Rectangle(0, 0, this.Width, m_ThemeWindowMargins.Top + 1);
                    theme.DrawBackground(g, part, state, r);
                    r.Offset(0, 1);
                }
                else if (m_GrabHandleStyle == eGrabHandleStyle.Caption && this.LayoutType == eLayoutType.DockContainer && this.Style == eDotNetBarStyle.Office2000)
                {
                    Rectangle rback = new Rectangle(3, 3, this.Width - 6, GetGrabHandleCaptionHeight());
                    re.Bounds = rback;
                    OnPreRender(re);
                    if (!re.Cancel)
                    {
                        ControlPaint.DrawBorder3D(g, r, Border3DStyle.Raised, Border3DSide.All);
                        eDrawCaption flags = eDrawCaption.DC_SMALLCAP | eDrawCaption.DC_GRADIENT | eDrawCaption.DC_TEXT;
                        if (m_HasFocus) flags |= eDrawCaption.DC_ACTIVE;
                        IntPtr hdc = g.GetHdc();
                        NativeFunctions.RECT rect = new NativeFunctions.RECT(rback.X, rback.Y, rback.Right, rback.Bottom);
                        try
                        {
                            NativeFunctions.DrawCaption(this.Handle, hdc, ref rect, flags);
                        }
                        finally
                        {
                            g.ReleaseHdc(hdc);
                        }
                    }
                    drawCaptionText = false;
                    r = rback;
                }
                else
                {
                    Rectangle rback = new Rectangle(3, 3, this.Width - 6, GetGrabHandleCaptionHeight());
                    re.Bounds = rback;
                    OnPreRender(re);

                    if (!re.Cancel)
                    {
                        Pen p1 = new Pen(pa.Colors.BarFloatingBorder, 1);
                        NativeFunctions.DrawRectangle(g, p, r);
                        r.Inflate(-1, -1);
                        NativeFunctions.DrawRectangle(g, p, r);

                        p1.Dispose();
                        p1 = new Pen(pa.Colors.BarFloatingBorder, 1);
                        g.DrawLine(p1, 1, 2, 2, 2);
                        g.DrawLine(p1, this.Width - 3, 2, this.Width - 2, 2);
                        g.DrawLine(p1, 1, this.Height - 3, 2, this.Height - 3);
                        g.DrawLine(p1, this.Width - 3, this.Height - 3, this.Width - 2, this.Height - 3);
                        p1.Dispose();
                        p1 = null;

                        if (this.GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane)
                        {
                            sm = g.SmoothingMode;
                            g.SmoothingMode = SmoothingMode.Default;

                            if (m_CaptionBackColor.IsEmpty)
                            {
                                if (this.LayoutType == eLayoutType.Toolbar || m_HasFocus)
                                    DisplayHelp.FillRectangle(g, rback, pa.Colors.BarCaptionBackground, pa.Colors.BarCaptionBackground2, pa.Colors.BarCaptionBackgroundGradientAngle);
                                else
                                    DisplayHelp.FillRectangle(g, rback, pa.Colors.BarCaptionInactiveBackground, pa.Colors.BarCaptionInactiveBackground2, pa.Colors.BarCaptionInactiveBackgroundGAngle);
                            }
                            else
                                DisplayHelp.FillRectangle(g, new Rectangle(3, 3, this.Width - 6, GetGrabHandleCaptionHeight()), m_CaptionBackColor, Color.Empty);

                            g.SmoothingMode = sm;
                        }
                    }
                    r = rback;
                }

                r.Inflate(-1, -1);

                if (this.GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane)
                {
                    if (this.CanHideResolved)
                    {
                        m_SystemButtons.CloseButtonRect = new Rectangle(r.Right - (m_SystemButtons.ButtonSize.Width + 3), r.Y + (r.Height-m_SystemButtons.ButtonSize.Height)/2 , m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.CloseButton, m_SystemButtons.CloseButtonRect);
                        OnPreRender(re);
                        m_SystemButtons.CloseButtonRect = re.Bounds;
                        r.Width -= (m_SystemButtons.CloseButtonRect.Width + 3);
                        if(!re.Cancel)
                            PaintCloseButton(pa);
                    }
                    if (this.ShowCustomizeMenuButton)
                    {
                        m_SystemButtons.CustomizeButtonRect = new Rectangle(r.Right - (m_SystemButtons.ButtonSize.Width + 1), r.Y + (r.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.CustomizeButton, m_SystemButtons.CustomizeButtonRect);
                        OnPreRender(re);
                        m_SystemButtons.CustomizeButtonRect = re.Bounds;
                        r.Width -= (m_SystemButtons.CustomizeButtonRect.Width + 2);
                        if(!re.Cancel)
                            PaintCustomizeButton(pa);
                    }
                    r.X += 2;
                    r.Width -= 2;
                    if (r.Width > 0 && drawCaptionText)
                    {
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.CaptionText, r);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            System.Drawing.Font objFont = null;
                            try
                            {
                                objFont = new Font(this.Font, FontStyle.Bold);
                            }
                            catch
                            {
                                objFont = System.Windows.Forms.SystemInformation.MenuFont;
                            }
                            eTextFormat sf = eTextFormat.Default | eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
                            if (m_CaptionForeColor.IsEmpty)
                            {
                                if (this.DrawThemedCaption)
                                {
                                    if (m_HasFocus)
                                        state = ThemeWindowStates.CaptionActive;
                                    else
                                        state = ThemeWindowStates.CaptionInactive;
                                    r.Y += 1;
                                    //theme.DrawText(g, this.Text, objFont, r, part, state, ThemeTextFormat.Left | ThemeTextFormat.VCenter);
                                }
                                //else
                                TextDrawing.DrawString(g, this.Text, objFont, ((m_HasFocus || this.LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText), r, sf);
                            }
                            else
                                TextDrawing.DrawString(g, this.Text, objFont, m_CaptionForeColor, r, sf);
                        }
                    }
                }
                else
                {
                    this.PaintGrabHandle(pa);
                }
            }
            else if (m_BarState == eBarState.Docked && BarFunctions.IsOffice2007Style(this.Style) && this.GetRenderer() != null && (!this.IsThemed || this.MenuBar))
            {
                re = new RenderBarEventArgs(this, g, eBarRenderPart.Background, this.DisplayRectangle);
                OnPreRender(re);
                if (!re.Cancel && this.BackColor != Color.Transparent)
                {
                    ToolbarRendererEventArgs tre = new ToolbarRendererEventArgs(this, g, this.DisplayRectangle);
                    tre.ItemPaintArgs = pa;
                    this.GetRenderer().DrawToolbarBackground(tre);
                }
                else
                    PaintGrabHandle(pa);
            }
            else
            {
                p.Dispose();
                p = null;
                bool drawBorder = true;
                // Docked state
                if (m_ItemContainer.m_BackgroundColor.IsEmpty && this.BackColor != Color.Transparent)
                {
                    if (this.IsThemed)
                    {
                        Rectangle r = new Rectangle(-this.Location.X, -this.Location.Y, this.Parent.Width, this.Parent.Height);
                        ThemeRebar theme = this.ThemeRebar;
                        theme.DrawBackground(g, ThemeRebarParts.Background, ThemeRebarStates.Normal, r);
                    }
                    else
                    {
                        sm = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.Default;
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.Background, this.ClientRectangle);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            if (this.MenuBar)
                            {
                                if (!pa.Colors.MenuBarBackground2.IsEmpty && pa.Colors.MenuBarBackground2.A>0)
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(this.ClientRectangle, pa.Colors.MenuBarBackground, pa.Colors.MenuBarBackground2, pa.Colors.MenuBarBackgroundGradientAngle);
                                    g.FillRectangle(gradient, this.ClientRectangle);
                                    gradient.Dispose();
                                }
                                else if (IsGradientStyle && this.Parent != null && !pa.Colors.DockSiteBackColor2.IsEmpty && !pa.Colors.DockSiteBackColor.IsEmpty)
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(new Rectangle(-this.Left, -this.Top, this.Parent.Width, this.Parent.Height), pa.Colors.DockSiteBackColor, pa.Colors.DockSiteBackColor2, 0f);
                                    g.FillRectangle(gradient, -this.Left, -this.Top, this.Parent.Width, this.Parent.Height);
                                    gradient.Dispose();
                                }
                                else
                                {
                                    using (SolidBrush brush = new SolidBrush(pa.Colors.MenuBarBackground))
                                        g.FillRectangle(brush, this.DisplayRectangle);
                                }
                            }
                            else
                            {
                                if (this.Style == eDotNetBarStyle.VS2005 && this.LayoutType == eLayoutType.DockContainer && !this.BackColor.IsEmpty)
                                {
                                    DisplayHelp.FillRectangle(g, this.ClientRectangle, this.BackColor, Color.Empty);
                                }
                                else if (this.GradientBackground)
                                    DisplayHelp.FillRectangle(g, this.ClientRectangle, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);
                                else
                                {
                                    using (SolidBrush brush = new SolidBrush(pa.Colors.BarBackground))
                                        g.FillRectangle(brush, this.ClientRectangle);
                                }
                            }
                        }
                        else
                            drawBorder = false;
                        g.SmoothingMode = sm;
                    }
                }
                else if(!m_ItemContainer.BackColor.IsEmpty)
                {
                    sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.Default;
                    re = new RenderBarEventArgs(this, g, eBarRenderPart.Background, this.DisplayRectangle);
                    OnPreRender(re);
                    if (!re.Cancel)
                    {
                        using (SolidBrush brush = new SolidBrush(m_ItemContainer.BackColor))
                            g.FillRectangle(brush, this.DisplayRectangle);
                    }
                    else
                        drawBorder = false;
                    g.SmoothingMode = sm;
                }

                if (this.Parent != null && this.Parent.BackgroundImage != null && this.Parent is DockSite)
                {
                    Rectangle r = new Rectangle(-this.Location.X, -this.Location.Y, this.Parent.Width, this.Parent.Height);
                    DockSite site = this.Parent as DockSite;
                    BarFunctions.PaintBackgroundImage(g, r, site.BackgroundImage, site.BackgroundImagePosition, site.BackgroundImageAlpha);
                }
                else
                    PaintBackgroundImage(pa.Graphics);

                if (drawBorder)
                {
                    if (IsGradientStyle && !this.IsThemed && this.LayoutType == eLayoutType.Toolbar && !this.MenuBar && this.BarType == eBarType.Toolbar && this.BackColor != Color.Transparent)
                    {
                        if (p != null) p.Dispose();
                        p = new Pen(pa.Colors.BarDockedBorder, 1);
                        g.DrawLine(p, 0, this.Height - 1, this.Width, this.Height - 1);
                        p.Dispose();
                        p = null;
                    }
                    else
                    {
                        Rectangle border = this.ClientRectangle;
                        border.Inflate(-2, -2);
                        BarFunctions.DrawBorder(g, m_DockedBorder, border, m_SingleLineColor);
                    }
                }
                PaintGrabHandle(pa);
            }
			m_ItemContainer.Paint(pa);
            if (m_BarType == eBarType.StatusBar && this.GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
            {
                PaintGrabHandle(pa);
            }
            if (p != null) p.Dispose();
            g.SmoothingMode = sm;
            g.TextRenderingHint = th;

            if (HasPostRender)
            {
                re = new RenderBarEventArgs(this, g, eBarRenderPart.All, this.ClientRectangle);
                OnPostRender(re);
            }
		}

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">Provides the event arguments</param>
        protected virtual void OnPreRender(RenderBarEventArgs e)
        {
            if (PreRender != null)
                PreRender(this, e);
        }

        /// <summary>
        /// Raises the PostRender event.
        /// </summary>
        /// <param name="e">Provides the event arguments</param>
        protected virtual void OnPostRender(RenderBarEventArgs e)
        {
            if (PostRender != null)
                PostRender(this, e);
        }

        private bool HasPostRender
        {
            get
            {
                return PostRender != null;
            }
        }

		private bool GradientBackground
		{
			get
			{
                if (this.Style == eDotNetBarStyle.VS2005 && this.LayoutType == eLayoutType.DockContainer)
					return false;
				return true;
			}
		}

		internal bool ThemedBackground
		{
			get
			{
				if(this.IsThemed && m_ItemContainer.m_BackgroundColor.IsEmpty)
					return true;
				return false;
			}
		}

        /// <summary>
        /// Drawns bar grab handle if one specified.
        /// </summary>
        /// <param name="pa">Context information.</param>
		internal void PaintGrabHandle(ItemPaintArgs pa)
		{
            RenderBarEventArgs re = null;
			Graphics g=pa.Graphics;
			// Draw grab handles
			if(m_GrabHandleStyle!=eGrabHandleStyle.None)
			{
				Rectangle r=Rectangle.Empty;

                if (m_GrabHandleStyle != eGrabHandleStyle.Caption && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted && this.IsThemed && m_GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
				{
					ThemeRebar theme=this.ThemeRebar;
					ThemeRebarParts part=ThemeRebarParts.Gripper;

					if(m_ItemContainer.Orientation==eOrientation.Vertical)
					{
						part=ThemeRebarParts.GripperVert;
						r=new Rectangle(m_GrabHandleRect.X+2,m_GrabHandleRect.Top+2,m_GrabHandleRect.Width-4,5);
					}
					else
						r=new Rectangle(m_GrabHandleRect.X+2,m_GrabHandleRect.Top+2,5,m_GrabHandleRect.Height-4);
                    re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, r);
                    OnPreRender(re);
                    r = re.Bounds;
                    if(!re.Cancel)
					    theme.DrawBackground(g,part,ThemeRebarStates.Normal,r);
					return;
				}

				Color clr=pa.Colors.BarBackground;
				if(!m_ItemContainer.m_BackgroundColor.IsEmpty)
					clr=m_ItemContainer.m_BackgroundColor;
				
				switch(m_GrabHandleStyle)
				{
					case eGrabHandleStyle.Single:
					{
						if(m_ItemContainer.Orientation==eOrientation.Horizontal && m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
						{
							r=new Rectangle(m_GrabHandleRect.X+2,m_GrabHandleRect.Top+2,3,m_GrabHandleRect.Height-4);
                            re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, r);
                            OnPreRender(re);
                            if(!re.Cancel)
							    BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All,clr);
						}
						else
						{
							r=new Rectangle(m_GrabHandleRect.X+2,m_GrabHandleRect.Top+2,m_GrabHandleRect.Width-4,3);
                            re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, r);
                            OnPreRender(re);
                            if (!re.Cancel)
							    BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All,clr);
						}
						break;
					}
					case eGrabHandleStyle.Double:
					{
						if(m_ItemContainer.Orientation==eOrientation.Horizontal && m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
						{
							r=new Rectangle(m_GrabHandleRect.X+1,m_GrabHandleRect.Top+1,3,m_GrabHandleRect.Height-2);
							BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All,clr);
							r.Offset(3,0);
                            re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, r);
                            OnPreRender(re);
                            if (!re.Cancel)
							    BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All,clr);
						}
						else
						{
							r=new Rectangle(m_GrabHandleRect.X+1,m_GrabHandleRect.Top+1,m_GrabHandleRect.Width-2,3);
							BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All,clr);
							r.Offset(0,3);
                            re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, r);
                            OnPreRender(re);
                            if (!re.Cancel)
							    BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All,clr);
						}
						break;
					}
					case eGrabHandleStyle.DoubleThin:
					{
						if(m_ItemContainer.Orientation==eOrientation.Horizontal && m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
						{
							r=new Rectangle(m_GrabHandleRect.X+1,m_GrabHandleRect.Top+2,3,m_GrabHandleRect.Height-4);							
							BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Right,clr);
							r.Offset(3,0);
                            re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, r);
                            OnPreRender(re);
                            if (!re.Cancel)
							    BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Right,clr);
						}
						else
						{
							r=new Rectangle(m_GrabHandleRect.X+2,m_GrabHandleRect.Top+1,m_GrabHandleRect.Width-4,2);
							BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.Top | System.Windows.Forms.Border3DSide.Bottom,clr);
							r.Offset(0,3);
                            re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, r);
                            OnPreRender(re);
                            if (!re.Cancel)
                                BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.Top | System.Windows.Forms.Border3DSide.Bottom,clr);
						}
						break;
					}
					case eGrabHandleStyle.DoubleFlat:
					{
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            Pen pen = new Pen(ControlPaint.Dark(clr), 1);
                            if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
                            {
                                g.DrawLine(/*SystemPens.ControlDark*/pen, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, m_GrabHandleRect.X + 2, m_GrabHandleRect.Height - 4);
                                g.DrawLine(/*SystemPens.ControlDark*/pen, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + 2, m_GrabHandleRect.X + 5, m_GrabHandleRect.Height - 4);
                            }
                            else
                            {
                                g.DrawLine(pen, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, m_GrabHandleRect.Width - 4, m_GrabHandleRect.Y + 2);
                                g.DrawLine(pen, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 5, m_GrabHandleRect.Width - 4, m_GrabHandleRect.Y + 5);
                            }
                            pen.Dispose();
                            pen = null;
                        }
						break;
					}
					case eGrabHandleStyle.Dotted:
					{
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            Brush brushDark = new SolidBrush(ControlPaint.Dark(clr));
                            Brush brushLight = new SolidBrush(ControlPaint.Light(clr));
                            if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
                            {
                                for (int i = 0; i < (m_GrabHandleRect.Height - 4); i += 4)
                                {
                                    g.FillRectangle(/*SystemBrushes.ControlDark*/brushDark, m_GrabHandleRect.X + 1, m_GrabHandleRect.Top + 2 + i, 2, 2);
                                    g.FillRectangle(/*SystemBrushes.ControlLight*/brushLight, m_GrabHandleRect.X + 1, m_GrabHandleRect.Top + 2 + i, 1, 1);

                                    g.FillRectangle(brushDark, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + 2 + i, 2, 2);
                                    g.FillRectangle(brushLight, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + 2 + i, 1, 1);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < (m_GrabHandleRect.Width - 4); i += 4)
                                {
                                    g.FillRectangle(SystemBrushes.ControlDark, m_GrabHandleRect.Left + 2 + i, m_GrabHandleRect.Y + 1, 2, 2);
                                    g.FillRectangle(SystemBrushes.ControlLight, m_GrabHandleRect.Left + 2 + i, m_GrabHandleRect.Y + 1, 1, 1);

                                    g.FillRectangle(SystemBrushes.ControlDark, m_GrabHandleRect.Left + 2 + i, m_GrabHandleRect.Y + 5, 2, 2);
                                    g.FillRectangle(SystemBrushes.ControlLight, m_GrabHandleRect.Left + 2 + i, m_GrabHandleRect.Y + 5, 1, 1);
                                }
                            }
                            brushDark.Dispose();
                            brushLight.Dispose();
                        }
						break;
					}
					case eGrabHandleStyle.Stripe:
					{
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
                            {
                                for (int i = 0; i < (m_GrabHandleRect.Height - 4); i += 3)
                                    BarFunctions.DrawBorder3D(g, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + i + 2, 5, 2, System.Windows.Forms.Border3DStyle.RaisedInner, System.Windows.Forms.Border3DSide.Top | System.Windows.Forms.Border3DSide.Bottom, clr);
                                //System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_GrabHandleRect.X+2,m_GrabHandleRect.Top+i+2,5,2,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.Top | System.Windows.Forms.Border3DSide.Bottom);
                            }
                            else
                            {
                                for (int i = 0; i < (m_GrabHandleRect.Width - 4); i += 3)
                                    BarFunctions.DrawBorder3D(g, m_GrabHandleRect.Left + i + 2, m_GrabHandleRect.Y + 2, 2, 5, System.Windows.Forms.Border3DStyle.RaisedInner, System.Windows.Forms.Border3DSide.Right | System.Windows.Forms.Border3DSide.Left, clr);
                                //System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_GrabHandleRect.Left+i+2,m_GrabHandleRect.Y+2,2,5,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.Right | System.Windows.Forms.Border3DSide.Left);
                            }
                        }
						break;
					}
					case eGrabHandleStyle.StripeFlat:
					{
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            Pen pen = new Pen(pa.Colors.BarStripeColor/*ControlPaint.Dark(clr)*/, 1);
                            if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
                            {
                                for (int i = 2; i < (m_GrabHandleRect.Height - 6); i += 2)
                                    g.DrawLine(/*SystemPens.ControlDark*/pen, m_GrabHandleRect.X + 3, m_GrabHandleRect.Top + i + 3, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + i + 3);
                            }
                            else
                            {
                                for (int i = 1; i < (m_GrabHandleRect.Width - 6); i += 2)
                                    g.DrawLine(/*SystemPens.ControlDark*/pen, m_GrabHandleRect.Left + i + 3, m_GrabHandleRect.Y + 2, m_GrabHandleRect.Left + i + 3, m_GrabHandleRect.Y + 5);
                            }
                            pen.Dispose();
                        }
						break;
					}
					case eGrabHandleStyle.CaptionTaskPane:
                    case eGrabHandleStyle.CaptionDotted:
					{
						Rectangle targetRect=m_GrabHandleRect;
						Brush brush=null;
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.Caption, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            if (m_BarState == eBarState.Floating)
                            {
                                DisplayHelp.FillRectangle(g, targetRect, pa.Colors.BarCaptionBackground, pa.Colors.BarCaptionBackground2, pa.Colors.BarCaptionBackgroundGradientAngle);
                            }
                            else
                            {
                                if (!this.CaptionBackColor.IsEmpty)
                                    brush = new SolidBrush(this.CaptionBackColor);
                                if (pa.Colors.BarBackground2.IsEmpty)
                                {
                                    if (m_HasFocus && m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
                                        brush = new SolidBrush(pa.Colors.BarCaptionBackground);
                                    else
                                        brush = new SolidBrush(pa.Colors.BarBackground);
                                }
                                else
                                {
                                    brush = BarFunctions.CreateLinearGradientBrush(targetRect, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);
                                }

                                g.FillRectangle(brush, targetRect);

                                brush.Dispose();
                                brush = null;
                            }

                            Brush brushDark = null;
                            Brush brushLight = new SolidBrush(ControlPaint.Light(clr));

                            if (!pa.Colors.BarStripeColor.IsEmpty)
                            {
                                clr = pa.Colors.BarStripeColor;
                                brushDark = new SolidBrush(clr);
                            }
                            else
                                brushDark = new SolidBrush(ControlPaint.Dark(clr));
                            int y = m_GrabHandleRect.Top + 4;
                            int x = m_GrabHandleRect.Left + 4;
                            for (int i = y; i <= m_GrabHandleRect.Bottom - 4; i += 5)
                            {
                                g.FillRectangle(brushLight, x + 1, i + 1, 2, 2);
                                g.FillRectangle(brushDark, x, i, 2, 2);
                            }
                            if (brushLight != null) brushLight.Dispose();
                            if (brushDark != null) brushDark.Dispose();
                        }

						targetRect.X+=8;
						targetRect.Width-=8;

						targetRect.Inflate(-1,-1);
						PaintGrabHandleButtons(pa,ref targetRect);
                        if(m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
                            PaintCaptionText(pa, targetRect);
						break;
					}
					case eGrabHandleStyle.Caption:
					{
						Rectangle targetRect=m_GrabHandleRect;
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.Caption, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            ThemeWindow theme = null;
                            ThemeWindowParts part = ThemeWindowParts.SmallCaption;
                            ThemeWindowStates state = ThemeWindowStates.CaptionInactive;
                            if (this.DrawThemedCaption)
                                theme = this.ThemeWindow;

                            if (m_DockedBorder == eBorderType.None)
                                targetRect.Inflate(-1, 0);
                            if (m_CaptionBackColor.IsEmpty)
                            {
                                if (this.Style == eDotNetBarStyle.Office2000)
                                {
                                    if (m_HasFocus)
                                        g.FillRectangle(SystemBrushes.ActiveCaption, targetRect);
                                    else
                                        g.FillRectangle(SystemBrushes.InactiveCaption, targetRect);
                                }
                                else
                                {
                                    if (this.DrawThemedCaption)
                                    {
                                        if (m_HasFocus)
                                            state = ThemeWindowStates.CaptionActive;
                                        theme.DrawBackground(g, part, state, targetRect);
                                    }
                                    else
                                    {
                                        if (m_HasFocus)
                                        {
                                            if (pa.Colors.BarCaptionBackground2.IsEmpty)
                                                DisplayHelp.FillRectangle(g, targetRect, pa.Colors.BarCaptionBackground);
                                            else
                                            {
                                                using (System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(targetRect, pa.Colors.BarCaptionBackground, pa.Colors.BarCaptionBackground2, pa.Colors.BarCaptionBackgroundGradientAngle))
                                                    g.FillRectangle(gradient, targetRect);
                                            }
                                        }
                                        else
                                        {
                                            if (pa.Colors.BarCaptionInactiveBackground2.IsEmpty)
                                                DisplayHelp.FillRectangle(g, targetRect, pa.Colors.BarCaptionInactiveBackground);
                                            else
                                            {
                                                using (System.Drawing.Drawing2D.LinearGradientBrush brush = BarFunctions.CreateLinearGradientBrush(targetRect, pa.Colors.BarCaptionInactiveBackground, pa.Colors.BarCaptionInactiveBackground2, pa.Colors.BarCaptionInactiveBackgroundGAngle))
                                                {
                                                    g.FillRectangle(brush, targetRect);
                                                }
                                            }

                                            eDotNetBarStyle effectiveStyle = m_ItemContainer.EffectiveStyle;
                                            if (effectiveStyle != eDotNetBarStyle.VS2005 && effectiveStyle != eDotNetBarStyle.Office2010)
                                            {
                                                Pen penCap = new Pen(pa.Colors.BarCaptionBackground, 1);
                                                g.DrawLine(penCap, targetRect.X + 1, targetRect.Y, targetRect.Right - 2, targetRect.Y);
                                                g.DrawLine(penCap, targetRect.X + 1, targetRect.Bottom - 1, targetRect.Right - 2, targetRect.Bottom - 1);
                                                g.DrawLine(penCap, targetRect.X, targetRect.Y + 1, targetRect.X, targetRect.Bottom - 2);
                                                g.DrawLine(penCap, targetRect.Right - 1, targetRect.Y + 1, targetRect.Right - 1, targetRect.Bottom - 2);
                                                penCap.Dispose();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                DisplayHelp.FillRectangle(g, targetRect, m_CaptionBackColor);
                        }

						targetRect.Inflate(-1,-1);
						
						PaintGrabHandleButtons(pa,ref targetRect);

                        PaintCaptionText(pa, targetRect);
						break;
					}
					case eGrabHandleStyle.ResizeHandle:
					{
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.ResizeHandle, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            // Paint Internet Explorer-style dotted sizer in lower right
                            // draw light 'shadows'
                            Form form = this.FindForm();
                            if (form != null && form.WindowState == FormWindowState.Maximized)
                                break;
                            int direction = 1;
                            Point startLoc = new Point(this.ClientRectangle.Right, this.ClientRectangle.Bottom);
                            if (this.RightToLeft == RightToLeft.Yes)
                            {
                                direction = -1;
                                startLoc = new Point(0, this.ClientRectangle.Bottom - 2);
                            }

                            Pen pen = new Pen(ControlPaint.LightLight(clr), 1);
                            g.DrawLine(pen, startLoc.X - 2 * direction, startLoc.Y - 2,
                                startLoc.X - 3 * direction, startLoc.Y - 2);
                            g.DrawLine(pen, startLoc.X - 6 * direction, startLoc.Y - 2,
                                startLoc.X - 7 * direction, startLoc.Y - 2);
                            g.DrawLine(pen, startLoc.X - 10 * direction, startLoc.Y - 2,
                                startLoc.X - 11 * direction, startLoc.Y - 2);
                            g.DrawLine(pen, startLoc.X - 2 * direction, startLoc.Y - 2,
                                startLoc.X - 2 * direction, startLoc.Y - 3);
                            g.DrawLine(pen, startLoc.X - 6 * direction, startLoc.Y - 2,
                                startLoc.X - 6 * direction, startLoc.Y - 3);
                            g.DrawLine(pen, startLoc.X - 10 * direction, startLoc.Y - 2,
                                startLoc.X - 10 * direction, startLoc.Y - 3);
                            g.DrawLine(pen, startLoc.X - 2 * direction, startLoc.Y - 6,
                                startLoc.X - 3 * direction, startLoc.Y - 6);
                            g.DrawLine(pen, startLoc.X - 6 * direction, startLoc.Y - 6,
                                startLoc.X - 7 * direction, startLoc.Y - 6);
                            g.DrawLine(pen, startLoc.X - 2 * direction, startLoc.Y - 6,
                                startLoc.X - 2 * direction, startLoc.Y - 7);
                            g.DrawLine(pen, startLoc.X - 6 * direction, startLoc.Y - 6,
                                startLoc.X - 6 * direction, startLoc.Y - 7);
                            g.DrawLine(pen, startLoc.X - 2 * direction, startLoc.Y - 10,
                                startLoc.X - 3 * direction, startLoc.Y - 10);
                            g.DrawLine(pen, startLoc.X - 2 * direction, startLoc.Y - 10,
                                startLoc.X - 2 * direction, startLoc.Y - 11);
                            pen.Dispose();

                            // draw dark squares
                            pen = new Pen(pa.Colors.BarStripeColor/*ControlPaint.Dark(clr)*/, 1);
                            g.DrawRectangle(pen, startLoc.X - 4 * direction, startLoc.Y - 4, 1, 1);
                            g.DrawRectangle(pen, startLoc.X - 8 * direction, startLoc.Y - 4, 1, 1);
                            g.DrawRectangle(pen, startLoc.X - 12 * direction, startLoc.Y - 4, 1, 1);
                            g.DrawRectangle(pen, startLoc.X - 4 * direction, startLoc.Y - 8, 1, 1);
                            g.DrawRectangle(pen, startLoc.X - 8 * direction, startLoc.Y - 8, 1, 1);
                            g.DrawRectangle(pen, startLoc.X - 4 * direction, startLoc.Y - 12, 1, 1);
                            pen.Dispose();
                            pen = null;
                        }
						break;
					}
					case eGrabHandleStyle.Office2003:
					case eGrabHandleStyle.Office2003SingleDot:
					{
                        re = new RenderBarEventArgs(this, g, eBarRenderPart.GrabHandle, m_GrabHandleRect);
                        OnPreRender(re);
                        if (!re.Cancel)
                        {
                            int x = m_GrabHandleRect.Left + (m_GrabHandleRect.Width - 3) / 2;
                            int y = m_GrabHandleRect.Top + 4;

                            if (m_GrabHandleStyle == eGrabHandleStyle.Office2003SingleDot)
                            {
                                x = m_GrabHandleRect.Left + (m_GrabHandleRect.Width - 3) / 2;
                                y = m_GrabHandleRect.Top + (m_GrabHandleRect.Height - 3) / 2;
                            }
                            else
                            {
                                if (m_ItemContainer.Orientation == eOrientation.Vertical)
                                {
                                    x = m_GrabHandleRect.Left + 4;
                                    y = m_GrabHandleRect.Top + (m_GrabHandleRect.Height - 3) / 2;
                                }
                            }

                            Brush brushDark = null;
                            Brush brushLight = new SolidBrush(ControlPaint.Light(clr));

                            if (!pa.Colors.BarStripeColor.IsEmpty)
                            {
                                clr = pa.Colors.BarStripeColor;
                                brushDark = new SolidBrush(clr);
                            }
                            else
                                brushDark = new SolidBrush(ControlPaint.Dark(clr));

                            if (m_GrabHandleStyle == eGrabHandleStyle.Office2003SingleDot)
                            {
                                g.FillRectangle(brushLight, x + 1, y + 1, 2, 2);
                                g.FillRectangle(brushDark, x, y, 2, 2);
                            }
                            else
                            {
                                if (m_ItemContainer.Orientation == eOrientation.Vertical)
                                {
                                    for (int i = x; i <= m_GrabHandleRect.Right - 4; i += 5)
                                    {
                                        g.FillRectangle(brushLight, i + 1, y + 1, 2, 2);
                                        g.FillRectangle(brushDark, i, y, 2, 2);
                                    }
                                }
                                else
                                {
                                    for (int i = y; i <= m_GrabHandleRect.Bottom - 4; i += 5)
                                    {
                                        g.FillRectangle(brushLight, x + 1, i + 1, 2, 2);
                                        g.FillRectangle(brushDark, x, i, 2, 2);
                                    }
                                }
                            }
                            brushDark.Dispose();
                            brushLight.Dispose();
                        }
						break;
					}
				}
			}
		}

        private void PaintCaptionText(ItemPaintArgs pa, Rectangle targetRect)
        {
            Graphics g = pa.Graphics;
            RenderBarEventArgs re = new RenderBarEventArgs(this, g, eBarRenderPart.CaptionText, targetRect);
            OnPreRender(re);
            if (re.Cancel) return;
            System.Drawing.Font objFont = null;
            bool bDisposeFont = false;
            try
            {
                objFont = this.Font.Clone() as Font; //new Font(this.Font.Name, 8);
                bDisposeFont = true;
            }
            catch
            {
                objFont = System.Windows.Forms.SystemInformation.MenuFont;
            }
            eTextFormat sf = eTextFormat.Default | eTextFormat.SingleLine | eTextFormat.EndEllipsis | eTextFormat.VerticalCenter;
            if (m_CaptionForeColor.IsEmpty)
            {
                if (this.IsThemed)
                {
                    if (targetRect.Height < objFont.Height)
                        targetRect.Height = objFont.Height;
                    if (targetRect.Width > 0)
                    {
                        if (!m_HasFocus)
                            TextDrawing.DrawString(g, this.Text, objFont, SystemColors.WindowText, targetRect, sf);
                        else
                            TextDrawing.DrawString(g, this.Text, objFont, SystemColors.ActiveCaptionText, targetRect, sf);
                    }
                }
                else
                {
                    if (targetRect.Height < objFont.Height)
                        targetRect.Height = objFont.Height;
                    if (targetRect.Width > 0)
                    {
                        Color textColor = pa.Colors.BarCaptionText;
                        if (m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
                            textColor = pa.Colors.ItemText;
                        else if ((this.Style == eDotNetBarStyle.OfficeXP || this.Style == eDotNetBarStyle.Office2003 || this.Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.Style)) && !m_HasFocus)
                            textColor = pa.Colors.BarCaptionInactiveText;
                        TextDrawing.DrawString(g, this.Text, objFont, textColor, targetRect, sf);
                    }
                }
            }
            else if (targetRect.Width > 0)
                TextDrawing.DrawString(g, this.Text, objFont, m_CaptionForeColor, targetRect, sf);
            if (bDisposeFont)
                objFont.Dispose();
        }

        /// <summary>
        /// Gets whether caption of floating bar will be drawn using themes.
        /// </summary>
		internal bool DrawThemedCaption
		{
			get
			{
                if (BarFunctions.IsOffice2007Style(this.Style) || !this.ThemeAware)
                    return false;
				if(this.IsThemed || m_BarState==eBarState.Floating && BarFunctions.ThemedOS && Themes.ThemesActive && this.LayoutType==eLayoutType.DockContainer)
					return true;
				return false;
			}
		}

		private bool GetPaintAutoHidePin()
		{
			bool ret=false;
			if(m_CanAutoHide && m_ItemContainer.LayoutType==eLayoutType.DockContainer && m_BarState!=eBarState.Floating)
			{
				if(this.Parent is DockSite && this.Parent.Dock==DockStyle.Fill)
					ret=false;
				else
					ret=true;
			}
			return ret;
		}

		private void PaintGrabHandleButtons(ItemPaintArgs pa, ref Rectangle targetRect)
		{
            RenderBarEventArgs re = null;
            if (this.CanHideResolved)
			{
				if(this.Style==eDotNetBarStyle.Office2000)
					m_SystemButtons.CloseButtonRect=new Rectangle(targetRect.Right-11,targetRect.Y+(targetRect.Height-m_SystemButtons.ButtonSize.Width)/2,m_SystemButtons.ButtonSize.Width,m_SystemButtons.ButtonSize.Height);
				else
					m_SystemButtons.CloseButtonRect=new Rectangle(targetRect.Right-(m_SystemButtons.ButtonSize.Width+1),targetRect.Y+(targetRect.Height-m_SystemButtons.ButtonSize.Height)/2,m_SystemButtons.ButtonSize.Width,m_SystemButtons.ButtonSize.Height);
                re = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.CloseButton, m_SystemButtons.CloseButtonRect);
                OnPreRender(re);
                m_SystemButtons.CloseButtonRect = re.Bounds;
                targetRect.Width -= (m_SystemButtons.CloseButtonRect.Width + 3);
                if(!re.Cancel)
				    PaintCloseButton(pa);
			}
			if(GetPaintAutoHidePin())
			{
				if(this.Style==eDotNetBarStyle.Office2000)
					m_SystemButtons.AutoHideButtonRect=new Rectangle(targetRect.Right-(m_SystemButtons.ButtonSize.Width-1),targetRect.Y+(targetRect.Height-m_SystemButtons.ButtonSize.Height)/2,m_SystemButtons.ButtonSize.Width,m_SystemButtons.ButtonSize.Height);
				else
					m_SystemButtons.AutoHideButtonRect=new Rectangle(targetRect.Right-(m_SystemButtons.ButtonSize.Width+1),targetRect.Y+(targetRect.Height-m_SystemButtons.ButtonSize.Height)/2,m_SystemButtons.ButtonSize.Width,m_SystemButtons.ButtonSize.Height);
                re = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.AutoHideButton, m_SystemButtons.AutoHideButtonRect);
                OnPreRender(re);
                m_SystemButtons.AutoHideButtonRect = re.Bounds;
                targetRect.Width -= (m_SystemButtons.AutoHideButtonRect.Width + 3);
                if(!re.Cancel)
				    PaintAutoHideButton(pa);
			}
			if(this.ShowCustomizeMenuButton)
			{
                re = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.CustomizeButton, Rectangle.Empty);
                
				if(this.Style==eDotNetBarStyle.Office2000)
				{
					m_SystemButtons.CustomizeButtonRect=new Rectangle(targetRect.X,targetRect.Y+(targetRect.Height-m_SystemButtons.ButtonSize.Height)/2,m_SystemButtons.ButtonSize.Width,m_SystemButtons.ButtonSize.Height);
                    re.Bounds = m_SystemButtons.CustomizeButtonRect;
                    OnPreRender(re);
                    m_SystemButtons.CustomizeButtonRect = re.Bounds;
                    targetRect.Width -= (m_SystemButtons.CustomizeButtonRect.Width + 2);
                    targetRect.X += (m_SystemButtons.CustomizeButtonRect.Width + 2);
				}
				else
				{
					m_SystemButtons.CustomizeButtonRect=new Rectangle(targetRect.Right-(m_SystemButtons.ButtonSize.Width+1),targetRect.Y+(targetRect.Height-m_SystemButtons.ButtonSize.Height)/2,m_SystemButtons.ButtonSize.Width,m_SystemButtons.ButtonSize.Height);
                    re.Bounds = m_SystemButtons.CustomizeButtonRect;
                    OnPreRender(re);
                    m_SystemButtons.CustomizeButtonRect = re.Bounds;
                    targetRect.Width -= (m_SystemButtons.CustomizeButtonRect.Width + 2);
				}

                if(!re.Cancel)
				    PaintCustomizeButton(pa);
			}

			if(this.GrabHandleStyle==eGrabHandleStyle.CaptionTaskPane)
			{
				targetRect.X+=2;
				targetRect.Width-=2;
				targetRect.Inflate(0,-2);
				m_SystemButtons.CaptionButtonRect=targetRect;
                re = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.CaptionTaskPane, m_SystemButtons.CaptionButtonRect);
                OnPreRender(re);
                m_SystemButtons.CaptionButtonRect = re.Bounds;
                if(!re.Cancel)
				    PaintCaptionButton(pa);
			}

			targetRect.X+=2;
			targetRect.Width-=2;
		}

		protected void PaintOffice(Graphics g)
		{
			m_SystemButtons.CustomizeButtonRect=Rectangle.Empty;
			m_SystemButtons.CloseButtonRect=Rectangle.Empty;

			if(m_ItemContainer.BackColor.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(SystemColors.Control))
					g.FillRectangle(brush,this.DisplayRectangle);
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(m_ItemContainer.BackColor))
					g.FillRectangle(brush,this.DisplayRectangle);
			}
			ColorScheme cs=this.GetColorScheme();
			PaintSideBar(g);
            ItemPaintArgs pa = GetItemPaintArgs(g); // new ItemPaintArgs(m_Owner as IOwner, this, g, cs); // TODO: ADD SUPPORT FOR SCHEMES
            //pa.DesignerSelection = m_DesignerSelection;
			if(m_ItemContainer.SubItems.Count>0)
			{
				if(m_BarState==eBarState.Popup)
				{
					System.Windows.Forms.ControlPaint.DrawBorder3D(g,0,0,this.ClientSize.Width,this.ClientSize.Height,System.Windows.Forms.Border3DStyle.Raised);
					PaintSideBar(g);
				}
				else if(m_BarState==eBarState.Floating)
				{
					System.Windows.Forms.ControlPaint.DrawBorder3D(g,0,0,this.ClientSize.Width,this.ClientSize.Height,System.Windows.Forms.Border3DStyle.Raised | System.Windows.Forms.Border3DStyle.RaisedOuter);
                    if (m_CaptionBackColor.IsEmpty)
                        g.FillRectangle(SystemBrushes.ActiveCaption, 4, 4, this.ClientSize.Width - 8, 15);
                    else
                    {
                        using (SolidBrush b1 = new SolidBrush(m_CaptionBackColor))
                            g.FillRectangle(b1, 4, 4, this.ClientSize.Width - 8, 15);
                    }

					System.Drawing.Font objFont=null;
					bool bDisposeFont=false;
					try
					{
						objFont=new Font(this.Font,FontStyle.Bold);
						bDisposeFont=true;
					}
					catch
					{
						objFont=System.Windows.Forms.SystemInformation.MenuFont;
					}

					Rectangle r=Rectangle.Empty;
					if(this.ShowCustomizeMenuButton)
					{
						r=new Rectangle(18,4,this.ClientSize.Width-22,15);
						m_SystemButtons.CustomizeButtonRect=new Rectangle(5,6,15,14);
						PaintCustomizeButton(pa);
					}
					else
					{
						r=new Rectangle(4,4,this.ClientSize.Width-8,15);
					}
                    if (this.CanHideResolved)
					{
						m_SystemButtons.CloseButtonRect=new Rectangle(r.Right-m_SystemButtons.ButtonSize.Width,6,m_SystemButtons.ButtonSize.Width,m_SystemButtons.ButtonSize.Height);
						PaintCloseButton(pa);
						r.Width-=(m_SystemButtons.ButtonSize.Width+1);
					}
					
					if(m_CaptionForeColor.IsEmpty)
                        TextDrawing.DrawString(g, this.Text, objFont, SystemColors.ActiveCaptionText, r,eTextFormat.Default);
					else
                        TextDrawing.DrawString(g, this.Text, objFont, m_CaptionForeColor, r, eTextFormat.Default);

					if(bDisposeFont)
						objFont.Dispose();
				}
				else
				{
					// Docked state
					BarFunctions.DrawBorder(g,m_DockedBorder,this.ClientRectangle,m_SingleLineColor);
					PaintGrabHandle(pa);
				}

				m_ItemContainer.Paint(pa);
			}
		}

		private void PaintCloseButton()
		{
			this.Invalidate(m_SystemButtons.CloseButtonRect);
			this.Update();
		}
		private void PaintCloseButton(ItemPaintArgs pa)
		{
			if(m_SystemButtons.CloseButtonRect.IsEmpty)
				return;

			Graphics g=pa.Graphics;

			if(this.DrawThemedCaption)
			{
				ThemeWindow theme=this.ThemeWindow;
				ThemeWindowParts part=ThemeWindowParts.SmallCloseButton;
				ThemeWindowStates state=ThemeWindowStates.ButtonNormal;
				if(m_SystemButtons.MouseDownClose)
					state=ThemeWindowStates.ButtonPushed;
				else if(m_SystemButtons.MouseOverClose)
					state=ThemeWindowStates.ButtonHot;
				theme.DrawBackground(g,part,state,m_SystemButtons.CloseButtonRect);
				return;
			}

            if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				ButtonState state=ButtonState.Normal;
				if(m_SystemButtons.MouseDownClose)
					state=ButtonState.Pushed;
				ControlPaint.DrawCaptionButton(g,m_SystemButtons.CloseButtonRect,CaptionButton.Close,state);
			}
			else
			{
				Pen pen=null;

				if(m_SystemButtons.MouseDownClose)
				{
					if(pa.Colors.ItemPressedBackground2.IsEmpty)
                        DisplayHelp.FillRectangle(g, m_SystemButtons.CloseButtonRect, pa.Colors.ItemPressedBackground);
					else
					{
						System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CloseButtonRect,pa.Colors.ItemPressedBackground,pa.Colors.ItemPressedBackground2,pa.Colors.ItemPressedBackgroundGradientAngle);
						g.FillRectangle(gradient,m_SystemButtons.CloseButtonRect);
						gradient.Dispose();
					}
                    using (Pen p1 = new Pen(pa.Colors.ItemPressedBorder))
                        NativeFunctions.DrawRectangle(g, p1, m_SystemButtons.CloseButtonRect);
				}
				else if(m_SystemButtons.MouseOverClose)
				{
					pen=new Pen(pa.Colors.ItemHotText); //SystemPens.ControlText;
					if(!pa.Colors.ItemHotBackground2.IsEmpty)
					{
						System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CloseButtonRect,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle);
						g.FillRectangle(gradient,m_SystemButtons.CloseButtonRect);
						gradient.Dispose();
					}
					else
                        DisplayHelp.FillRectangle(g, m_SystemButtons.CloseButtonRect, pa.Colors.ItemHotBackground);
                    pen.Dispose();
                    pen = null;
                    using (Pen p1 = new Pen(pa.Colors.ItemHotBorder))
                        NativeFunctions.DrawRectangle(g, p1, m_SystemButtons.CloseButtonRect);
				}

                if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
					pen=new Pen(pa.Colors.ItemText);
				else
                    pen = new Pen(((m_HasFocus || this.LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText));
				if(m_SystemButtons.MouseOverClose)
					pen=new Pen(pa.Colors.ItemHotText,1);
				else if(m_SystemButtons.MouseDownClose)
					pen=new Pen(pa.Colors.ItemPressedText,1);
				else if(!m_CaptionForeColor.IsEmpty)
					pen=new Pen(m_CaptionForeColor,1);
                else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
					pen=new Pen(pa.Colors.BarCaptionInactiveText);

				Point[] p=new Point[2];
				Rectangle rect=m_SystemButtons.CloseButtonRect;
				rect.Inflate(-1,-1);
				p[0].X=rect.Left+2;
				p[0].Y=rect.Top+3;
				p[1].X=rect.Right-4;
				p[1].Y=rect.Bottom-4;
				g.DrawLine(pen,p[0],p[1]);
				p[0].X++;
				p[1].X++;
				g.DrawLine(pen,p[0],p[1]);

				p[0].X=rect.Right-3;
				p[0].Y=rect.Top+3;
				p[1].X=rect.Left+3;
				p[1].Y=rect.Bottom-4;
				g.DrawLine(pen,p[0],p[1]);
				p[0].X--;
				p[1].X--;
				g.DrawLine(pen,p[0],p[1]);
                pen.Dispose();
			}
		}
		private void PaintCaptionButton()
		{
			this.Invalidate(m_SystemButtons.CaptionButtonRect);
			this.Update();
		}
		private void PaintCaptionButton(ItemPaintArgs pa)
		{
			if(m_SystemButtons.CaptionButtonRect.IsEmpty)
				return;

			Rectangle rect=m_SystemButtons.CaptionButtonRect;
			Graphics g=pa.Graphics;

            if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				ButtonState state=ButtonState.Normal;
				if(m_SystemButtons.MouseDownCaption)
					state=ButtonState.Pushed;
				ControlPaint.DrawButton(g,rect,state);
			}
			else
			{
				if(m_CaptionMenu!=null && m_CaptionMenu.Expanded)
				{
                    if (pa.Colors.ItemExpandedBackground2.IsEmpty)
                        DisplayHelp.FillRectangle(g, rect, pa.Colors.ItemExpandedBackground);
                    else
                    {
                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(rect, pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
                        g.FillRectangle(gradient, rect);
                        gradient.Dispose();
                    }
                    using (Pen p1 = new Pen(pa.Colors.ItemExpandedBorder))
                        NativeFunctions.DrawRectangle(g, p1, rect);
				}
				else if(m_SystemButtons.MouseDownCaption)
				{
					if(pa.Colors.ItemPressedBackground2.IsEmpty)
						DisplayHelp.FillRectangle(g, rect, pa.Colors.ItemPressedBackground);
					else
					{
						System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(rect,pa.Colors.ItemPressedBackground,pa.Colors.ItemPressedBackground2,pa.Colors.ItemPressedBackgroundGradientAngle);
						g.FillRectangle(gradient,rect);
						gradient.Dispose();
					}
                    using (Pen p1 = new Pen(pa.Colors.ItemPressedBorder))
                        NativeFunctions.DrawRectangle(g, p1, rect);
				}
				else if(m_SystemButtons.MouseOverCaption)
				{
					Pen pen=new Pen(pa.Colors.ItemHotText);
					if(!pa.Colors.ItemHotBackground2.IsEmpty)
					{
						System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(rect,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle);
						g.FillRectangle(gradient,rect);
						gradient.Dispose();
					}
					else
						DisplayHelp.FillRectangle(g, rect, pa.Colors.ItemHotBackground);
                    using (Pen p1 = new Pen(pa.Colors.ItemHotBorder))
                        NativeFunctions.DrawRectangle(g, p1, rect);
				}
			}
			SolidBrush brush=null;
            Color textColor = pa.Colors.ItemText;

			if(m_SystemButtons.MouseOverCaption)
				textColor=pa.Colors.ItemHotText;
			else if(m_SystemButtons.MouseDownCaption)
				textColor=pa.Colors.ItemPressedText;
			else if(!m_CaptionForeColor.IsEmpty)
                textColor = m_CaptionForeColor;
			else
                textColor = pa.Colors.ItemText;
            brush = new SolidBrush(textColor);
            eTextFormat format = eTextFormat.Default | eTextFormat.VerticalCenter | eTextFormat.EndEllipsis | eTextFormat.SingleLine;
			Rectangle rText=rect;
			rText.Width-=12;
            rText.X += 3;
			if(rText.Width>8)
                TextDrawing.DrawString(g, this.Text, this.Font, textColor, rText, format);

			Point[] p=new Point[3];
			p[0].X=rect.Right-9;
			p[0].Y=rect.Top+(rect.Height+6)/2;
			p[1].X=p[0].X-4;
			p[1].Y=p[0].Y-5;
			p[2].X=p[1].X+9;
			p[2].Y=p[1].Y;
			g.FillPolygon(brush,p);

			brush.Dispose();
		}
		private void PaintCustomizeButton()
		{
			this.Invalidate(m_SystemButtons.CustomizeButtonRect);
			this.Update();
		}
		private void PaintCustomizeButton(ItemPaintArgs pa)
		{
			if(m_SystemButtons.CustomizeButtonRect.IsEmpty)
				return;
			Graphics g=pa.Graphics;
            Brush brush = new SolidBrush(((m_HasFocus || this.LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText));

			if(!this.DrawThemedCaption)
			{
                if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					Point[] p1=new Point[3];
					p1[0].X=m_SystemButtons.CustomizeButtonRect.X+4;
					p1[0].Y=m_SystemButtons.CustomizeButtonRect.Y+8;
					p1[1].X=p1[0].X-3;
					p1[1].Y=p1[0].Y-4;
					p1[2].X=p1[1].X+7;
					p1[2].Y=p1[1].Y;
					g.FillPolygon(SystemBrushes.ActiveCaptionText,p1);
                    brush.Dispose();
					return;
				}
				else
				{
                    if (!m_CaptionForeColor.IsEmpty)
                    {
                        brush.Dispose();
                        brush = new SolidBrush(m_CaptionForeColor);
                    }
                    if (!m_HasFocus && m_BarState != eBarState.Floating)
                    {
                        brush.Dispose();
                        brush = new SolidBrush(pa.Colors.BarCaptionInactiveText);
                    }

					if(m_SystemButtons.MouseDownCustomize)
					{
                        if (pa.Colors.ItemPressedBackground2.IsEmpty)
                        {
                            using (SolidBrush b = new SolidBrush(pa.Colors.ItemPressedBackground))
                                g.FillRectangle(b, m_SystemButtons.CustomizeButtonRect);
                        }
                        else
                        {
                            System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CustomizeButtonRect, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
                            g.FillRectangle(gradient, m_SystemButtons.CustomizeButtonRect);
                            gradient.Dispose();
                        }
                        using (Pen p1 = new Pen(pa.Colors.ItemPressedBorder))
                            NativeFunctions.DrawRectangle(g, p1, m_SystemButtons.CustomizeButtonRect);
					}
					else if(m_SystemButtons.MouseOverCustomize)
					{
                        brush.Dispose();
						brush=new SolidBrush(pa.Colors.BarCaptionText); // SystemBrushes.ControlText;
                        if (!pa.Colors.ItemHotBackground2.IsEmpty)
                        {
                            System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CustomizeButtonRect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                            g.FillRectangle(gradient, m_SystemButtons.CustomizeButtonRect);
                            gradient.Dispose();
                        }
                        else
                        {
                            using (SolidBrush b = new SolidBrush(pa.Colors.ItemHotBackground))
                                g.FillRectangle(b, m_SystemButtons.CustomizeButtonRect);
                        }
                        using (Pen p1 = new Pen(pa.Colors.ItemHotBorder))
                            NativeFunctions.DrawRectangle(g, p1, m_SystemButtons.CustomizeButtonRect);
					}
				}
			}

            if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
            {
                brush.Dispose();
                brush = new SolidBrush(pa.Colors.ItemText);
            }
            else
            {
                brush.Dispose();
                brush = new SolidBrush(((m_HasFocus || this.LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText));
            }
            if (m_SystemButtons.MouseOverCustomize)
            {
                brush.Dispose();
                brush = new SolidBrush(pa.Colors.ItemHotText);
            }
            else if (m_SystemButtons.MouseDownCustomize)
            {
                brush.Dispose();
                brush = new SolidBrush(pa.Colors.ItemPressedText);
            }
            else if (!m_CaptionForeColor.IsEmpty)
            {
                brush.Dispose();
                brush = new SolidBrush(m_CaptionForeColor);
            }
            else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
            {
                brush.Dispose();
                brush = new SolidBrush(pa.Colors.BarCaptionInactiveText);
            }

			Point[] p=new Point[3];
			p[0].X=m_SystemButtons.CustomizeButtonRect.Right-7;
            p[0].Y = m_SystemButtons.CustomizeButtonRect.Top + 8;
			p[1].X=p[0].X-3;
			p[1].Y=p[0].Y-4;
			p[2].X=p[1].X+7;
			p[2].Y=p[1].Y;
			g.FillPolygon(brush,p);
            brush.Dispose();
		}
		private void PaintAutoHideButton()
		{
			if(!m_SystemButtons.AutoHideButtonRect.IsEmpty)
			{
                Rectangle r = m_SystemButtons.AutoHideButtonRect;
                r.Inflate(1, 1);
				this.Invalidate(r);
				this.Update();
			}
		}
		private void PaintAutoHideButton(ItemPaintArgs pa)
		{
			if(m_SystemButtons.AutoHideButtonRect.IsEmpty)
				return;

			Graphics g=pa.Graphics;

			if(this.DrawThemedCaption)
			{
				Image img=null;
				if(m_SystemButtons.MouseDownAutoHide && !this.AutoHide || this.AutoHide && !m_SystemButtons.MouseDownAutoHide)
				{
					if(m_SystemButtons.MouseOverAutoHide)
						img=BarFunctions.LoadBitmap("SystemImages.Pin.png");
					else
						img=BarFunctions.LoadBitmap("SystemImages.PinInactive.png");
					g.DrawImage(img,m_SystemButtons.AutoHideButtonRect.X+(m_SystemButtons.AutoHideButtonRect.Width-img.Width)/2,m_SystemButtons.AutoHideButtonRect.Y+(m_SystemButtons.AutoHideButtonRect.Height-img.Height)/2,img.Width,img.Height);
				}
				else
				{
					if(m_SystemButtons.MouseOverAutoHide)
						img=BarFunctions.LoadBitmap("SystemImages.PinPushed.png");
					else
						img=BarFunctions.LoadBitmap("SystemImages.PinPushedInactive.png");
					g.DrawImage(img,m_SystemButtons.AutoHideButtonRect.X+(m_SystemButtons.AutoHideButtonRect.Width-img.Width)/2,m_SystemButtons.AutoHideButtonRect.Y+(m_SystemButtons.AutoHideButtonRect.Height-img.Height)/2,img.Width,img.Height);
				}
				return;
			}


            if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				Rectangle r=m_SystemButtons.AutoHideButtonRect;
				r.Inflate(1,0);
				if(m_SystemButtons.MouseDownAutoHide)
				{
					ControlPaint.DrawBorder3D(g,r,Border3DStyle.SunkenInner,Border3DSide.All);
				}
				else
				{
					ControlPaint.DrawBorder3D(g,r,Border3DStyle.Raised,Border3DSide.All);
				}
			}
			else
			{
				if(m_SystemButtons.MouseDownAutoHide)
				{
					if(pa.Colors.ItemPressedBackground2.IsEmpty)
						DisplayHelp.FillRectangle(g, m_SystemButtons.AutoHideButtonRect, pa.Colors.ItemPressedBackground);
					else
					{
						System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(m_SystemButtons.AutoHideButtonRect,pa.Colors.ItemPressedBackground,pa.Colors.ItemPressedBackground2,pa.Colors.ItemPressedBackgroundGradientAngle);
						g.FillRectangle(gradient,m_SystemButtons.AutoHideButtonRect);
						gradient.Dispose();
					}
                    using (Pen p1 = new Pen(pa.Colors.ItemPressedBorder))
                        NativeFunctions.DrawRectangle(g, p1, m_SystemButtons.AutoHideButtonRect);
				}
				else if(m_SystemButtons.MouseOverAutoHide)
				{
					if(!pa.Colors.ItemHotBackground2.IsEmpty)
					{
						System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(m_SystemButtons.AutoHideButtonRect,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle);
						g.FillRectangle(gradient,m_SystemButtons.AutoHideButtonRect);
						gradient.Dispose();
					}
					else
						DisplayHelp.FillRectangle(g, m_SystemButtons.AutoHideButtonRect, pa.Colors.ItemHotBackground);
                    using (Pen p1 = new Pen(pa.Colors.ItemHotBorder))
                        NativeFunctions.DrawRectangle(g, p1, m_SystemButtons.AutoHideButtonRect);
				}
			}
			if(m_SystemButtons.MouseDownAutoHide && !this.AutoHide || this.AutoHide && !m_SystemButtons.MouseDownAutoHide)
			{
				Rectangle r=new Rectangle(m_SystemButtons.AutoHideButtonRect.X+(m_SystemButtons.AutoHideButtonRect.Width-10)/2,m_SystemButtons.AutoHideButtonRect.Y+(m_SystemButtons.AutoHideButtonRect.Height-7)/2,10,7);
				
				Pen pen=null;
                if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
					pen=new Pen(pa.Colors.ItemText);
				else
                    pen = new Pen(((m_HasFocus || this.LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText));
				if(m_SystemButtons.MouseOverAutoHide)
					pen=new Pen(pa.Colors.ItemHotText,1);
				else if(m_SystemButtons.MouseDownAutoHide)
					pen=new Pen(pa.Colors.ItemPressedText,1);
				else if(!m_CaptionForeColor.IsEmpty)
					pen=new Pen(m_CaptionForeColor,1);
                else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
					pen=new Pen(pa.Colors.BarCaptionInactiveText);

				g.DrawRectangle(pen,r.X+4,r.Y+1,5,4);
				g.DrawLine(pen,r.X+4,r.Y+4,r.X+9,r.Y+4);
				g.DrawLine(pen,r.X+4,r.Y,r.X+4,r.Y+6);
				g.DrawLine(pen,r.X,r.Y+3,r.X+3,r.Y+3);
			}
			else
			{
				Rectangle r=new Rectangle(m_SystemButtons.AutoHideButtonRect.X+(m_SystemButtons.AutoHideButtonRect.Width-7)/2,m_SystemButtons.AutoHideButtonRect.Y+(m_SystemButtons.AutoHideButtonRect.Height-9)/2,7,9);
				
				Pen pen=null;
                if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
					pen=new Pen(pa.Colors.ItemText);
				else
                    pen = new Pen(((m_HasFocus || this.LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText));
				if(m_SystemButtons.MouseOverAutoHide)
					pen=new Pen(pa.Colors.ItemHotText,1);
				else if(m_SystemButtons.MouseDownAutoHide)
					pen=new Pen(pa.Colors.ItemPressedText,1);
				else if(!m_CaptionForeColor.IsEmpty)
					pen=new Pen(m_CaptionForeColor,1);
                else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
					pen=new Pen(pa.Colors.BarCaptionInactiveText);

				g.DrawRectangle(pen,r.X+1,r.Y,4,5);
				g.DrawLine(pen,r.X+4,r.Y,r.X+4,r.Y+5);
				g.DrawLine(pen,r.X,r.Y+5,r.X+6,r.Y+5);
				g.DrawLine(pen,r.X+3,r.Y+5,r.X+3,r.Y+8);
			}
		}

        /// <summary>
        /// Paints bar side bar.
        /// </summary>
        /// <param name="g">Reference to graphics object.</param>
		internal void PaintSideBar(Graphics g)
		{
			if(m_SideBarImage.Picture==null || m_BarState!=eBarState.Popup)
				return;
			
			if(!m_SideBarImage.GradientColor1.IsEmpty && !m_SideBarImage.GradientColor2.IsEmpty)
			{
				System.Drawing.Drawing2D.LinearGradientBrush lgb=BarFunctions.CreateLinearGradientBrush(m_SideBarRect,m_SideBarImage.GradientColor1,m_SideBarImage.GradientColor2,m_SideBarImage.GradientAngle);
				g.FillRectangle(lgb,m_SideBarRect);
			}
			else if(!m_SideBarImage.BackColor.Equals(Color.Empty))
				DisplayHelp.FillRectangle(g, m_SideBarRect, m_SideBarImage.BackColor);
			
			if(m_SideBarImage.StretchPicture)
			{
				g.DrawImage(m_SideBarImage.Picture,m_SideBarRect);
			}
			else
			{
				if(m_SideBarImage.Alignment==eAlignment.Top)
					g.DrawImage(m_SideBarImage.Picture,m_SideBarRect.X,m_SideBarRect.Top,m_SideBarRect.Width,m_SideBarImage.Picture.Height);
				else if(m_SideBarImage.Alignment==eAlignment.Bottom)
					g.DrawImage(m_SideBarImage.Picture,m_SideBarRect.Left,m_SideBarRect.Bottom-m_SideBarImage.Picture.Height,m_SideBarImage.Picture.Width,m_SideBarImage.Picture.Height);
				else
					g.DrawImage(m_SideBarImage.Picture,m_SideBarRect.Left,m_SideBarRect.Top+(m_SideBarRect.Height-m_SideBarImage.Picture.Height)/2,m_SideBarImage.Picture.Width,m_SideBarImage.Picture.Height);
			}
		}

		internal BaseItem ParentInternal
		{
			get
			{
				return m_ParentItem;
			}
		}

		/// <summary>
		/// Gets/Sets the parent item of the Bar. The parents item sub-items are displayed on the bar.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public BaseItem ParentItem
		{
			get
			{
				if(m_ParentItem!=null)
					return m_ParentItem;
				return m_ItemContainer;
			}
			set
			{
				RestoreContainer();

				if(m_ItemContainer.SubItems.Count>0)
					m_ItemContainer.SubItems.Clear();

				m_ParentItem=value;

				if(m_ParentItem==null || m_ParentItem.SubItems.Count==0)
					return;
				
				m_ItemContainer.Style=m_ParentItem.Style;

				BaseItem objTmp=m_ParentItem.SubItems[0];
				if(objTmp!=null)
					m_OldContainer=objTmp.ContainerControl;
				else
					m_OldContainer=null;

				m_ItemContainer.SubItems.AllowParentRemove=false;
				foreach(BaseItem objItem in m_ParentItem.SubItems)
				{
					objItem.ContainerControl=this;
					m_ItemContainer.SubItems.Add(objItem);
				}
				m_ItemContainer.SubItems.AllowParentRemove=true;

				// Get the parent's screen position
				if(m_ParentItem.Displayed)
				{
					System.Windows.Forms.Control objCtrl=m_ParentItem.ContainerControl as System.Windows.Forms.Control;
					if(BaseItem.IsHandleValid(objCtrl))
					{
						m_ParentItemScreenPos=objCtrl.PointToScreen(new Point(m_ParentItem.LeftInternal,m_ParentItem.TopInternal));
						objCtrl=null;
					}
				}
			}
		}

		private void SetupRegion()
		{
			if(m_BarState!=eBarState.Popup)
				return;

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

		/// <summary>
		/// Recalculates the layout of the Bar, resizes the Bar if necessary and repaints it.
		/// </summary>
		public void RecalcLayout()
		{
            if (m_LayoutSuspended) return;

			if(m_BarState==eBarState.Docked)
			{
				if(this.Parent!=null && !PassiveBar)
				{
					if(m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
						this.Invalidate(this.Region,true);
						
					if(this.Parent is DockSite)
						((DockSite)this.Parent).RecalcLayout();
					else
					{
						RecalcSize();
						this.Refresh();
					}
					ResizeDockTab();
				}
			}
			else
			{
				RecalcSize();
				ResizeDockTab();
				this.Refresh();
			}
		}

		internal void OnDockContainerWidthChanged(DockContainerItem item, int width)
		{
            if (m_LayoutSuspended)
                return;

            if (this.DockSide == eDockSide.Document || this.Parent is DockSite && ((DockSite)this.Parent).DocumentDockContainer != null)
            {
                ((DockSite)this.DockedSite).GetDocumentUIManager().SetBarWidth(this, width + 2);
            }
            else if (m_BarState == eBarState.AutoHide)
            {
                m_ItemContainer.MinWidth = width;
                m_LastDockSiteInfo.DockedWidth = width + 2;
            }
            //else
            //{
            //    m_ItemContainer.MinWidth=width+2;
            //    if(this.AutoHide)
            //        m_LastDockSiteInfo.DockedWidth=width+2;
				
            //    ChangeSplitSize(new Size(width+2,0));

            //    this.RecalcLayout();
            //}
		}

		internal void OnDockContainerHeightChanged(DockContainerItem item, int height)
		{
            if (m_LayoutSuspended)
                return;

			if(this.DockSide==eDockSide.Document || this.Parent is DockSite && ((DockSite)this.Parent).DocumentDockContainer!=null)
			{
				((DockSite)this.DockedSite).GetDocumentUIManager().SetBarHeight(this,height+this.GetNonClientHeight());
			}
            else if (m_BarState == eBarState.AutoHide)
            {
                m_ItemContainer.MinHeight = height;
                m_LastDockSiteInfo.DockedHeight = height + 2;
            }
            //else
            //{
            //    m_ItemContainer.MinHeight=height+2;
            //    if(this.AutoHide)
            //        m_LastDockSiteInfo.DockedHeight=height+2;

            //    ChangeSplitSize(new Size(0,height+this.GetNonClientHeight())); // Accounts for caption size and border
				
            //    this.RecalcLayout();
            //}
		}

		internal void OnDockContainerMinimumSizeChanged(DockContainerItem item)
		{
			if(this.AutoHide)
			{
				m_LastDockSiteInfo.DockedHeight=0;
				m_LastDockSiteInfo.DockedWidth=0;
			}
		}

		private void ChangeSplitSize(System.Drawing.Size size)
		{
			// Support for resizing of the side-by-side docked bars i.e. on the same line
			if(this.Parent!=null && this.Parent.Controls.Count>1)
			{
				Bar barLeft=null, barRight=null;
				
				Bar previous=this.GetPreviousVisibleBar(this);
				Bar next=this.GetNextVisibleBar(this);
				if(next!=null && next.LayoutType==eLayoutType.DockContainer && next.DockLine==this.DockLine)
				{
					barLeft=this;
					barRight=next;
				}
				else if(previous!=null && previous.LayoutType==eLayoutType.DockContainer && previous.DockLine==this.DockLine)
				{
					barLeft=previous;
					barRight=this;
					if(size.Width>0)
						size.Width=barLeft.Width+(barRight.Width-size.Width);
					if(size.Height>0)
						size.Height=barLeft.Height+(barRight.Height-size.Height);
				}

				if(this.DockSide==eDockSide.Left || this.DockSide==eDockSide.Right)
				{
					if(barLeft!=null && barRight!=null && barLeft.DockLine==barRight.DockLine)
					{
						System.Drawing.Size minLeftSize=barLeft.MinimumDockSize(eOrientation.Horizontal);
						System.Drawing.Size minThisSize=barRight.MinimumDockSize(eOrientation.Horizontal);
						int heightOffset=size.Height-barLeft.Height;
						if(size.Height>=minLeftSize.Height && barRight.Height-heightOffset>=minThisSize.Height)
						{
							Size oldLeftBarSize=barLeft.Size;
							Size oldSize=barRight.Size;
							Size newBarLeftSize=barLeft.RecalcSizeOnly(new Size(barLeft.Width,size.Height));
							Size newSize=barRight.RecalcSizeOnly(new Size(barRight.Width,barRight.Height-heightOffset));
							if(!oldLeftBarSize.Equals(newBarLeftSize) && !oldSize.Equals(newSize))
							{
								barRight.SplitDockHeight=0;
								barLeft.SplitDockHeight=size.Height;
							}
						}
					}
					else
					{
						this.SplitDockHeight=0;
						this.SplitDockWidth=0;
					}
				}
				else if(this.DockSide==eDockSide.Top || this.DockSide==eDockSide.Bottom)
				{
					if(barLeft!=null && barRight!=null && barLeft.DockLine==barRight.DockLine)
					{
						System.Drawing.Size minLeftSize=barLeft.MinimumDockSize(eOrientation.Horizontal);
						System.Drawing.Size minThisSize=barRight.MinimumDockSize(eOrientation.Horizontal);
						int widthOffset=size.Width-barLeft.Width;
						if(size.Width>=minLeftSize.Width && barRight.Width-widthOffset>=minThisSize.Width)
						{
							Size oldLeftBarSize=barLeft.Size;
							Size oldSize=barRight.Size;
							Size newBarLeftSize=barLeft.RecalcSizeOnly(new Size(size.Width,barLeft.Height));
							Size newSize=barRight.RecalcSizeOnly(new Size(barRight.Width-widthOffset,barRight.Height));
							if(!oldLeftBarSize.Equals(newBarLeftSize) && !oldSize.Equals(newSize))
							{
								barRight.SplitDockWidth=0;
								barLeft.SplitDockWidth=size.Width;
							}
						}
					}
					else
					{
						this.SplitDockHeight=0;
						this.SplitDockWidth=0;
					}
				}
			}
		}

		internal int SplitDockHeight
		{
			get
			{
				if(this.Parent!=null && m_SplitDockHeightPercent>0)
				{
					int height=((this.Parent.Height*m_SplitDockHeightPercent)/1000);
					Size sz=GetAdjustedFullSize(this.MinimumDockSize(eOrientation.Horizontal));
					if(height<sz.Height)
						height=sz.Height;
					return height;
				}
				return 0;
			}
			set
			{
				if(this.Parent!=null && value>0)
				{
					if(value>this.Parent.Height)
						value=this.Parent.Height;
					m_SplitDockHeightPercent=(int)(((float)value/(float)this.Parent.Height)*1000);
				}
				else
					m_SplitDockHeightPercent=0;
			}
		}
		internal int SplitDockWidth
		{
			get
			{
				if(this.Parent!=null && m_SplitDockWidthPercent>0)
				{
					return ((this.Parent.Width*m_SplitDockWidthPercent)/1000);
				}
				return 0;
			}
			set
			{
				if(this.Parent!=null && value>0)
				{
					if(value>this.Parent.Width)
						value=this.Parent.Width;
					m_SplitDockWidthPercent=(int)(((float)value/(float)this.Parent.Width)*1000);
				}
				else
					m_SplitDockWidthPercent=0;
			}
		}

		internal int MinHeight
		{
			get
			{
				if(m_ItemContainer.MinHeight>0)
					return m_ItemContainer.MinHeight+GetNonClientHeight();
				return 0;
			}
			set
			{
				if(m_ItemContainer == null) return;
				if(value>0)
					m_ItemContainer.MinHeight=value-GetNonClientHeight();
				else
					m_ItemContainer.MinHeight=0;
			}
		}
		internal int GetNonClientHeight()
		{
			return GetGrabHandleCaptionHeight()+2; // Caption + border
		}

		private bool m_RecalculatingSize=false; // Used to preven re-entrancy
		/// <summary>
		/// Recalculates the layout of the Bar and repaints it. This will not change the size of the Bar it will only force the recalculation of the size for each contained item and it will repaint the bar. To ensure that Bar is resized if necessary as well call RecalcLayout method.
		/// </summary>
		public void RecalcSize()
		{
			if(!BarFunctions.IsHandleValid(this))
				return;

			m_GrabHandleRect=Rectangle.Empty;
			if(m_RecalculatingSize || m_ItemContainer==null)
				return;
            Form form = this.FindForm();
            if (form != null && form.WindowState == FormWindowState.Minimized)
                return;

			m_RecalculatingSize=true;

			if(this.LayoutType==eLayoutType.DockContainer && m_TabDockItems==null)
				RefreshDockTab(false);
			
			try
			{
                //m_ItemContainer.IsRightToLeft = (this.RightToLeft == RightToLeft.Yes);
				if(m_BarState==eBarState.Docked)
				{
					m_ItemContainer.WrapItems=m_WrapItemsDock;
					m_ItemContainer.Stretch=m_DockStretch;
				}
				else if(m_BarState==eBarState.Floating || m_BarState==eBarState.AutoHide)
				{
					m_ItemContainer.WrapItems=m_WrapItemsFloat;
					if(m_ItemContainer.LayoutType==eLayoutType.DockContainer)
						m_ItemContainer.Stretch=m_DockStretch;
					else
						m_ItemContainer.Stretch=false;
				}
				else
				{
					m_ItemContainer.WrapItems=true;
					m_ItemContainer.Stretch=false;
				}

                if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					if(m_BarState==eBarState.Docked && !(this.Parent is DockSite))
					{
						System.Drawing.Size size=RecalcSizeOffice();
						if(m_ItemContainer.Orientation==eOrientation.Horizontal)
							this.Height=size.Height;
						else
							this.Width=size.Width;
					}
					else
						this.ClientSize=RecalcSizeOffice();
				}
				else
				{
					if(m_BarState==eBarState.Docked && !(this.Parent is DockSite))
					{
						System.Drawing.Size size=RecalcSizeDotNet();
						if(m_ItemContainer.Orientation==eOrientation.Horizontal)
							this.Height=size.Height;
						else
							this.Width=size.Width;
					}
					else
						this.ClientSize=RecalcSizeDotNet();
				}
				
				if(m_DropShadow!=null)
					NativeFunctions.SetWindowPos(m_DropShadow.Handle,NativeFunctions.HWND_NOTOPMOST,this.Left+5,this.Top+5,this.Width-2,this.Height-2,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);

				if(m_BarState==eBarState.Docked)
				{
					if(m_ItemContainer.Orientation==eOrientation.Horizontal)
						m_DockedSizeH=this.Size;
					else
						m_DockedSizeV=this.Size;
					RefreshOffice2003Region();
				}
				else if(this.Region!=null)
					this.Region=null;
				if(m_BarState==eBarState.Floating && !PassiveBar)
				{
					m_Float.RefreshRegion(this.ClientSize);
					m_Float.ClientSize=this.ClientSize;
				}
			}
			finally
			{
				m_RecalculatingSize=false;
			}

			if(this.LayoutType==eLayoutType.DockContainer && m_TabDockItems!=null && this.DesignMode && this.Site!=null)
				RefreshDockTab(false);

            OnItemLayoutUpdated(EventArgs.Empty);
		}

        /// <summary>
        /// Occurs after internal item layout has been updated and items have valid bounds assigned.
        /// </summary>
        public event EventHandler ItemLayoutUpdated;
        /// <summary>
        /// Raises ItemLayoutUpdated event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnItemLayoutUpdated(EventArgs e)
        {
            EventHandler handler = ItemLayoutUpdated;
            if (handler != null)
                handler(this, e);
        }

		private void RefreshOffice2003Region()
		{
            if (m_RoundCorners && IsGradientStyle && !this.MenuBar && this.LayoutType == eLayoutType.Toolbar && this.BarType == eBarType.Toolbar && !this.IsThemed && this.GrabHandleStyle != eGrabHandleStyle.ResizeHandle && this.BackColor != Color.Transparent)
			{
				System.Drawing.Drawing2D.GraphicsPath path=new System.Drawing.Drawing2D.GraphicsPath();
				path.AddLine(0,2,2,0);
				path.AddLine(2,0,this.Width-2,0);
				path.AddLine(this.Width,2,this.Width,this.Height-3);
				path.AddLine(this.Width-3,this.Height,3,this.Height);
				path.AddLine(2,this.Height,0,this.Height-3);
				this.Region=new Region(path);
			}
			else if(this.Region!=null)
				this.Region=null;
		}

		internal void RecalcSize(System.Drawing.Size frameSize)
		{
			m_GrabHandleRect=Rectangle.Empty;
			if(m_RecalculatingSize || m_ItemContainer == null)
				return;
			m_RecalculatingSize=true;

			try
				//if(m_ItemContainer.SubItemsCount>0)
			{
				if(m_BarState==eBarState.Docked)
				{
					m_ItemContainer.WrapItems=m_WrapItemsDock;
					m_ItemContainer.Stretch=m_DockStretch;
				}
				else if(m_BarState==eBarState.Floating || m_BarState==eBarState.AutoHide)
				{
					m_ItemContainer.WrapItems=m_WrapItemsFloat;
					if(m_ItemContainer.LayoutType==eLayoutType.DockContainer)
						m_ItemContainer.Stretch=m_DockStretch;
					else
						m_ItemContainer.Stretch=false;
				}
				else
				{
					m_ItemContainer.WrapItems=true;
					m_ItemContainer.Stretch=false;
				}

                if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
					this.ClientSize=RecalcSizeOffice(false,frameSize,m_ItemContainer.Orientation,m_BarState,m_ItemContainer.WrapItems);
				else
				{
					this.ClientSize=RecalcSizeDotNet(false,frameSize,m_ItemContainer.Orientation,m_BarState,m_ItemContainer.WrapItems);
				}

				if(m_DropShadow!=null)
					NativeFunctions.SetWindowPos(m_DropShadow.Handle,NativeFunctions.HWND_NOTOPMOST,this.Left+5,this.Top+5,this.Width-2,this.Height-2,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);

				if(m_BarState==eBarState.Docked)
				{
					if(m_ItemContainer.Orientation==eOrientation.Horizontal)
						m_DockedSizeH=this.Size;
					else
						m_DockedSizeV=this.Size;
					RefreshOffice2003Region();
				}
				if(m_BarState==eBarState.Floating && !PassiveBar)
				{
					m_Float.RefreshRegion(this.ClientSize);
					m_Float.ClientSize=this.ClientSize;
				}
			}
			finally
			{
				m_RecalculatingSize=false;
			}
		}

		private System.Drawing.Size RecalcSizeOnly(System.Drawing.Size objFrameSize)
		{
			return RecalcSizeOnly(objFrameSize,m_ItemContainer.Orientation,m_BarState,m_ItemContainer.WrapItems);
		}
		private System.Drawing.Size RecalcSizeOnly(System.Drawing.Size objFrameSize, eOrientation barOrientation, eBarState barState, bool bWrapItems)
		{
			if(m_ItemContainer.SubItems.Count>0)
			{
                if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
					return RecalcSizeOffice(true,objFrameSize,barOrientation, barState, bWrapItems);
				else
					return RecalcSizeDotNet(true,objFrameSize,barOrientation, barState, bWrapItems);
			}
			return System.Drawing.Size.Empty;
		}

		private System.Drawing.Size RecalcSizeDotNet()
		{
			return RecalcSizeDotNet(false,this.Size,m_ItemContainer.Orientation,this.m_BarState,m_ItemContainer.WrapItems);
		}
		private System.Drawing.Size RecalcSizeDotNet(bool bCalculateOnly, System.Drawing.Size objFrameSize, eOrientation barOrientation, eBarState barState, bool bWrapItems)
		{
			System.Drawing.Size thisSize=System.Drawing.Size.Empty;			
			int iTopMargin=0, iBottomMargin=0, iLeftMargin=0, iRightMargin=0, iRightTabMargin=0;
			int iOrgWidth=0, iOrgHeight=0;
			eOrientation oldOrientation=m_ItemContainer.Orientation;
			eBarState thisBarState=m_BarState;
			bool bOldStretch=m_ItemContainer.Stretch;
			bool bOldWrapItems=m_ItemContainer.WrapItems;
            m_ItemContainer.IsRightToLeft = (this.RightToLeft == RightToLeft.Yes);
			if(bCalculateOnly)
			{
				m_ItemContainer.Orientation=barOrientation;
				thisBarState=barState;
				m_ItemContainer.WrapItems=bWrapItems;
				if(barState==eBarState.Docked)
					m_ItemContainer.Stretch=m_DockStretch;
				else
				{
					if(m_ItemContainer.LayoutType==eLayoutType.DockContainer)
						m_ItemContainer.Stretch=m_DockStretch;
					else
						m_ItemContainer.Stretch=false;
				}
			}

			if(thisBarState==eBarState.Docked)
			{
				if(m_DockedBorder!=eBorderType.None)
				{
					iTopMargin=3;
					iBottomMargin=3;
					iLeftMargin=3;
					iRightMargin=3;
				}
			}
			else if(thisBarState==eBarState.Floating)
			{
				if(this.IsThemed || this.DrawThemedCaption)
				{
					if(m_ThemeWindowMargins.IsEmpty)
						this.RefreshThemeMargins();
					iTopMargin=m_ThemeWindowMargins.Top;
					iLeftMargin=m_ThemeWindowMargins.Left;
					iRightMargin=m_ThemeWindowMargins.Right;
					iBottomMargin=m_ThemeWindowMargins.Bottom;
                    if (this.GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane)
                        iTopMargin = GetGrabHandleTaskPaneHeight() + m_ThemeWindowMargins.Bottom;
				}
				else
				{
					if(this.GrabHandleStyle==eGrabHandleStyle.CaptionTaskPane)
						iTopMargin=GetGrabHandleTaskPaneHeight()+4;
					else
						iTopMargin=GetNonClientHeight();
					iLeftMargin=2;
					iRightMargin=2;
					iBottomMargin=3;
				}
			}
			else
			{
				iTopMargin=this.ClientMarginTop;
				iLeftMargin=this.ClientMarginLeft;
				iRightMargin=this.ClientMarginRight;
				iBottomMargin=this.ClientMarginBottom;
			}

			int iTop=iTopMargin, iLeft=iLeftMargin;

			if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && !m_AutoHideState && GetDockTabVisible())
			{
				if(m_ItemContainer.VisibleSubItems>1 || m_AlwaysDisplayDockTab && m_ItemContainer.VisibleSubItems==1 && m_BarState!=eBarState.Floating)
				{
					switch(m_DockTabAlignment)
					{
						case eTabStripAlignment.Top:
						{
							iTop+=DOCKTABSTRIP_HEIGHT;
							break;
						}
						case eTabStripAlignment.Left:
						{
							iLeft+=DOCKTABSTRIP_HEIGHT;
							break;
						}
						case eTabStripAlignment.Right:
						{
							iRightTabMargin=DOCKTABSTRIP_HEIGHT;
							break;
						}
						default:
						{
							iBottomMargin+=DOCKTABSTRIP_HEIGHT;
							break;
						}
					}
				}
			}
			
			// Take in account the side bar picture
			if(thisBarState==eBarState.Popup && m_SideBarImage.Picture!=null)
				iLeft+=m_SideBarImage.Picture.Width;
			else if(thisBarState==eBarState.Docked || this.BarState==eBarState.AutoHide)
			{
				// Show grab handles if any selected
				if(m_GrabHandleStyle!=eGrabHandleStyle.None)
				{
					if(m_GrabHandleStyle==eGrabHandleStyle.ResizeHandle)
					{
						// Leave space for the window sizer at the right
                        if (this.RightToLeft == RightToLeft.Yes)
                            iLeft += GrabHandleResizeWidth;
                        else
                            iRightMargin += GrabHandleResizeWidth;
					}
					else
					{
						if(m_ItemContainer.Orientation==eOrientation.Horizontal && m_ItemContainer.LayoutType==eLayoutType.Toolbar)
						{

                            if (m_GrabHandleStyle == eGrabHandleStyle.Caption)
                                iLeft += GetGrabHandleCaptionHeight();
                            else
                            {
                                if (this.RightToLeft == RightToLeft.Yes)
                                    iRightMargin += GrabHandleDotNetWidth;
                                else
                                    iLeft += GrabHandleDotNetWidth;
                            }
						}
						else
						{
							if(m_GrabHandleStyle==eGrabHandleStyle.Caption)
                                iTop += GetGrabHandleCaptionHeight();
                            else if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
								iTop+=GetGrabHandleTaskPaneHeight();
							else
								iTop+=GrabHandleDotNetWidth;
						}
					}
				}
			}
            
			if(!bCalculateOnly)
			{
				m_ItemContainer.TopInternal=iTop;
				m_ItemContainer.LeftInternal=iLeft;
			}
			else
			{
				iOrgHeight=m_ItemContainer.HeightInternal;
				iOrgWidth=m_ItemContainer.WidthInternal;
			}
			
			// Suspend Layout while setting suggested size
			m_ItemContainer.SuspendLayout=true;
			if(!bCalculateOnly)
			{
				if(thisBarState==eBarState.Popup)
					m_ItemContainer.WidthInternal=m_InitialContainerWidth;
				else
				{
					m_ItemContainer.WidthInternal=objFrameSize.Width-iLeft-iRightMargin-iRightTabMargin; //m_ItemContainer.WidthInternal=this.Width-iLeft-iRightMargin;
					m_ItemContainer.HeightInternal=objFrameSize.Height-iTop-iBottomMargin; //m_ItemContainer.HeightInternal=this.Height-iTop-iBottomMargin;
				}
			}
			else
			{
				m_ItemContainer.WidthInternal=objFrameSize.Width-iLeft-iRightMargin-iRightTabMargin;
				m_ItemContainer.HeightInternal=objFrameSize.Height-iTop-iBottomMargin;
			}
			m_ItemContainer.SuspendLayout=false;
			
			if(m_ItemContainer.VisibleSubItems==0 || (m_ItemContainer.SubItems.Count==1 && m_ItemContainer.SubItems[0] is CustomizeItem && !m_ItemContainer.SubItems[0].Visible))
			{
				m_ItemContainer.RecalcSize();
				if(this.LayoutType!=eLayoutType.DockContainer && this.DockSide!=eDockSide.Document)
				{
					m_ItemContainer.WidthInternal=36;
					m_ItemContainer.HeightInternal=24;
				}
			}
			else
				m_ItemContainer.RecalcSize();

			iTop+=m_ItemContainer.HeightInternal;
			if(IsGradientStyle && this.LayoutType==eLayoutType.Toolbar && !this.MenuBar && !this.IsThemed)
				iTop++;

            if (!bCalculateOnly)
            {
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    if (this.LayoutType == eLayoutType.Toolbar && m_ItemContainer.WidthInternal < objFrameSize.Width - iLeft - iRightMargin - iRightTabMargin && this.Stretch)
                    {
                        m_ItemContainer.LeftInternal = objFrameSize.Width - iRightMargin - m_ItemContainer.WidthInternal;
                        m_ItemContainer.RecalcSize();
                    }
                    m_ClientRect = new Rectangle(m_ItemContainer.LeftInternal, iTopMargin, m_ItemContainer.WidthInternal, iTop);
                }
                else
                    m_ClientRect = new Rectangle(iLeft, iTopMargin, m_ItemContainer.WidthInternal, iTop);
            }

			if(!bCalculateOnly)
			{
				if((thisBarState==eBarState.Docked || thisBarState==eBarState.AutoHide) && m_GrabHandleStyle!=eGrabHandleStyle.None && m_GrabHandleStyle!=eGrabHandleStyle.ResizeHandle)
				{
					if(m_ItemContainer.Orientation==eOrientation.Horizontal && m_ItemContainer.LayoutType==eLayoutType.Toolbar)
					{
                        if (m_GrabHandleStyle == eGrabHandleStyle.Caption)
                            m_GrabHandleRect = new Rectangle(iLeftMargin, iTopMargin, GetGrabHandleCaptionHeight(), iTop);
                        else
                        {
                            if (this.RightToLeft == RightToLeft.Yes)
                                m_GrabHandleRect = new Rectangle(m_ItemContainer.WidthInternal + iLeft + iLeftMargin, iTopMargin, 10, iTop);
                            else
                                m_GrabHandleRect = new Rectangle(iLeftMargin, iTopMargin, 10, iTop);
                        }
					}
					else
					{
						if(m_GrabHandleStyle==eGrabHandleStyle.Caption)
                            m_GrabHandleRect = new Rectangle(iLeftMargin, iTopMargin, m_ItemContainer.WidthInternal + iLeft - iLeftMargin + iRightTabMargin, GetGrabHandleCaptionHeight());
                        else if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
                            m_GrabHandleRect = new Rectangle(iLeftMargin, iTopMargin, m_ItemContainer.WidthInternal + iLeft - iLeftMargin + iRightTabMargin, GetGrabHandleTaskPaneHeight());
                        else
                        {
                            //if (this.RightToLeft == RightToLeft.Yes)
                            //    m_GrabHandleRect = new Rectangle(objFrameSize.Width - iLeft - iRightMargin - iRightTabMargin, iTopMargin, m_ItemContainer.WidthInternal + iLeft - iLeftMargin + iRightTabMargin, 10);
                            //else
                                m_GrabHandleRect = new Rectangle(iLeftMargin, iTopMargin, m_ItemContainer.WidthInternal + iLeft - iLeftMargin + iRightTabMargin, 10);
                        }
					}
				}
				else if(thisBarState==eBarState.Floating)
				{
					if(m_GrabHandleStyle==eGrabHandleStyle.CaptionTaskPane)
					{
						m_GrabHandleRect=new Rectangle(iLeftMargin,((this.IsThemed || this.DrawThemedCaption)?m_ThemeWindowMargins.Bottom:2),m_ItemContainer.WidthInternal+iLeft-iLeftMargin+iRightTabMargin,GetGrabHandleTaskPaneHeight());
					}
					else
					{
                        m_GrabHandleRect = new Rectangle(2, 2, m_ItemContainer.WidthInternal + iRightTabMargin, GetGrabHandleCaptionHeight());
                        if (this.CanHideResolved)
							m_GrabHandleRect.Width-=14;
					}
				}

				if(thisBarState==eBarState.Popup && m_SideBarImage.Picture!=null)
					m_SideBarRect=new Rectangle(iLeft-m_SideBarImage.Picture.Width,iTopMargin,m_SideBarImage.Picture.Width,iTop-iTopMargin);
				
				//if(iLeft+m_ItemContainer.Width+iRightMargin!=this.ClientSize.Width || this.ClientSize.Height!=iTop+iBottomMargin)
				//	this.ClientSize=new Size(iLeft+m_ItemContainer.Width+iRightMargin,iTop+iBottomMargin);
				thisSize=new Size(iLeft+m_ItemContainer.WidthInternal+iRightMargin+iRightTabMargin,iTop+iBottomMargin);
			}
			else
			{
				//if(m_BarState==eBarState.Floating)
					thisSize=new Size(iLeft+m_ItemContainer.WidthInternal+iRightMargin+iRightTabMargin,iTop+iBottomMargin);
				//else
				//	thisSize=new Size(iLeft+m_ItemContainer.WidthInternal+iRightMargin+iRightTabMargin+14,iTop+iBottomMargin);
				m_ItemContainer.WidthInternal=iOrgWidth;
				m_ItemContainer.HeightInternal=iOrgHeight;
				m_ItemContainer.Orientation=oldOrientation;
				m_ItemContainer.WrapItems=bOldWrapItems;
				m_ItemContainer.Stretch=bOldStretch;
				m_ItemContainer.RecalcSize();
			}
			return thisSize;
		}

		private System.Drawing.Size RecalcSizeOffice()
		{
			return RecalcSizeOffice(false,this.Size,m_ItemContainer.Orientation,this.m_BarState,m_ItemContainer.WrapItems);
		}
		private System.Drawing.Size RecalcSizeOffice(bool bCalculateOnly, System.Drawing.Size objFrameSize, eOrientation barOrientation, eBarState barState, bool bWrapItems)
		{
			System.Drawing.Size thisSize=System.Drawing.Size.Empty;

			int iTopMargin=0, iBottomMargin=0, iLeftMargin=0, iRightMargin=0, iRightTabMargin=0;
			int iOrgWidth=0, iOrgHeight=0;
			eOrientation oldOrientation=m_ItemContainer.Orientation;
			eBarState thisBarState=m_BarState;
			bool bOldStretch=m_ItemContainer.Stretch;
			bool bOldWrapItems=m_ItemContainer.WrapItems;
            m_ItemContainer.IsRightToLeft = (this.RightToLeft == RightToLeft.Yes);
			if(bCalculateOnly)
			{
				m_ItemContainer.Orientation=barOrientation;
				thisBarState=barState;
				m_ItemContainer.WrapItems=bWrapItems;
				if(barState==eBarState.Docked)
					m_ItemContainer.Stretch=m_DockStretch;
				else
				{
					if(m_ItemContainer.LayoutType==eLayoutType.DockContainer)
						m_ItemContainer.Stretch=m_DockStretch;
					else
						m_ItemContainer.Stretch=false;
				}
			}

			if(thisBarState==eBarState.Docked)
			{
				if(m_DockedBorder!=eBorderType.None)
				{
					iTopMargin=2;
					iBottomMargin=2;
					iLeftMargin=2;
					iRightMargin=2;
				}
			}
			else if(thisBarState==eBarState.Floating)
			{
				iTopMargin=22;
				iLeftMargin=4;
				iRightMargin=4;
				iBottomMargin=4;
			}
			else
			{
				iTopMargin=this.ClientMarginTop;
				iLeftMargin=this.ClientMarginLeft;
				iRightMargin=this.ClientMarginRight;
				iBottomMargin=this.ClientMarginBottom;
			}

			int iTop=iTopMargin, iLeft=iLeftMargin;

			if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && !m_AutoHideState && GetDockTabVisible())
			{
				if(m_ItemContainer.VisibleSubItems>1 || m_AlwaysDisplayDockTab && m_ItemContainer.VisibleSubItems==1 && m_BarState!=eBarState.Floating)
				{
					switch(m_DockTabAlignment)
					{
						case eTabStripAlignment.Top:
						{
							iTop+=DOCKTABSTRIP_HEIGHT;
							break;
						}
						case eTabStripAlignment.Left:
						{
							iLeft+=DOCKTABSTRIP_HEIGHT;
							break;
						}
						case eTabStripAlignment.Right:
						{
							iRightTabMargin=DOCKTABSTRIP_HEIGHT;
							break;
						}
						default:
						{
							iBottomMargin+=DOCKTABSTRIP_HEIGHT;
							break;
						}
					}
				}
			}
			
			// Take in account the side bar picture
			if(thisBarState==eBarState.Popup && m_SideBarImage.Picture!=null)
				iLeft+=m_SideBarImage.Picture.Width;
			else if(thisBarState==eBarState.Docked || this.BarState==eBarState.AutoHide)
			{
				// Show grab handles if any selected
				if(m_GrabHandleStyle!=eGrabHandleStyle.None)
				{
					if(m_ItemContainer.Orientation==eOrientation.Horizontal && m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
						iLeft+=GrabHandleOfficeWidth;
					else
					{
						if(m_GrabHandleStyle==eGrabHandleStyle.Caption)
                            iTop += GetGrabHandleCaptionHeight();
                        else if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
							iTop+=GetGrabHandleTaskPaneHeight();
						else
							iTop+=GrabHandleOfficeWidth;
					}
				}
			}
			
			if(!bCalculateOnly)
			{
				m_ItemContainer.TopInternal=iTop;
				m_ItemContainer.LeftInternal=iLeft;
			}
			else
			{
				iOrgHeight=m_ItemContainer.HeightInternal;
				iOrgWidth=m_ItemContainer.WidthInternal;
			}
			
			if(!bCalculateOnly)
			{
				if(thisBarState==eBarState.Popup)
					m_ItemContainer.WidthInternal=m_InitialContainerWidth;
				else
				{
					m_ItemContainer.WidthInternal=objFrameSize.Width-iLeft-iRightMargin-iRightTabMargin; //m_ItemContainer.WidthInternal=this.Width-iLeft-iRightMargin;
					m_ItemContainer.HeightInternal=objFrameSize.Height-iTop-iBottomMargin; //m_ItemContainer.HeightInternal=this.Height-iTop-iBottomMargin;
				}
			}
			else
			{
				m_ItemContainer.WidthInternal=objFrameSize.Width-iLeft-iRightMargin-iRightTabMargin;
				m_ItemContainer.HeightInternal=objFrameSize.Height-iTop-iBottomMargin;
			}

			if(m_ItemContainer.VisibleSubItems==0)
			{
				m_ItemContainer.WidthInternal=36;
				m_ItemContainer.HeightInternal=24;
			}
			else
				m_ItemContainer.RecalcSize();

			iTop+=m_ItemContainer.HeightInternal;
			
			if(!bCalculateOnly)
				m_ClientRect=new Rectangle(iLeft,iTopMargin,m_ItemContainer.WidthInternal,iTop);

			if(!bCalculateOnly)
			{
				if((thisBarState==eBarState.Docked || this.BarState==eBarState.AutoHide) && m_GrabHandleStyle!=eGrabHandleStyle.None)
				{
					if(m_ItemContainer.Orientation==eOrientation.Horizontal && m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
					{
						m_GrabHandleRect=new Rectangle(iLeftMargin,iTopMargin,12,iTop);
					}
					else
					{
						if(m_GrabHandleStyle==eGrabHandleStyle.Caption)
                            m_GrabHandleRect = new Rectangle(iLeftMargin, iTopMargin, m_ItemContainer.WidthInternal + iLeft - iLeftMargin + iRightTabMargin, GetGrabHandleCaptionHeight());
                        else if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
							m_GrabHandleRect=new Rectangle(iLeftMargin,iTopMargin,m_ItemContainer.WidthInternal+iLeft-iLeftMargin+iRightTabMargin,GetGrabHandleTaskPaneHeight());
						else
							m_GrabHandleRect=new Rectangle(iLeftMargin,iTopMargin,m_ItemContainer.WidthInternal+iLeft-iLeftMargin+iRightTabMargin,12);
					}
				}
				else if(thisBarState==eBarState.Floating)
				{
					if(m_GrabHandleStyle==eGrabHandleStyle.CaptionTaskPane)
						m_GrabHandleRect=new Rectangle(4,4,m_ItemContainer.WidthInternal+iRightTabMargin,GetGrabHandleTaskPaneHeight());
					else
						m_GrabHandleRect=new Rectangle(4,4,m_ItemContainer.WidthInternal+iRightTabMargin,15);
                    if (this.CanHideResolved)
						m_GrabHandleRect.Width-=14;
				}

				if(thisBarState==eBarState.Popup && m_SideBarImage.Picture!=null)
					m_SideBarRect=new Rectangle(iLeft-m_SideBarImage.Picture.Width,iTopMargin,m_SideBarImage.Picture.Width,iTop-iTopMargin);
				//this.ClientSize=new Size(iLeft+m_ItemContainer.Width+iRightMargin,iTop+iBottomMargin);
				thisSize=new Size(iLeft+m_ItemContainer.WidthInternal+iRightMargin+iRightTabMargin,iTop+iBottomMargin);
			}
			else
			{
				thisSize=new Size(iLeft+m_ItemContainer.WidthInternal+iRightMargin+iRightTabMargin,iTop+iBottomMargin);
				m_ItemContainer.WidthInternal=iOrgWidth;
				m_ItemContainer.HeightInternal=iOrgHeight;
				m_ItemContainer.Orientation=oldOrientation;
				m_ItemContainer.WrapItems=bOldWrapItems;
				m_ItemContainer.Stretch=bOldStretch;
				m_ItemContainer.RecalcSize();
			}
			return thisSize;
		}

		private void RestoreContainer()
		{
			if(m_ParentItem!=null)// && m_ParentItem.SubItems.Count!=0)
			{
				foreach(BaseItem objItem in m_ParentItem.SubItems)
				{
					objItem.Expanded=false;
					objItem.SetParent(m_ParentItem);
					objItem.ContainerControl=m_OldContainer;
					objItem.Displayed=false;
					objItem.Orientation=m_ParentItem.Orientation;
				}
				m_ItemContainer.SubItems._Clear();
			}
		}

		internal void OnAddedToBars()
		{
			if(m_Owner!=null && m_Owner is IOwner)
			{
				if(((IOwner)m_Owner).Images!=null)
					m_ItemContainer.RefreshImageSize();
			}
		}

		internal void ProcessDelayedCommands()
		{
			if(m_DockSideDelayed>=0)
			{
				if(!((eDockSide)m_DockSideDelayed==eDockSide.Document && this.DockSide==eDockSide.Document))
					this.DockSide=(eDockSide)m_DockSideDelayed;
				m_DockSideDelayed=-1;
				foreach(BaseItem item in m_ItemContainer.SubItems)
					item.OnProcessDelayedCommands();
			}
		}

		private bool IsAnyControl(Control ctrlParent, Control ctrlReference)
		{
			if(ctrlParent==ctrlReference)
				return true;
			foreach(Control ctrl in ctrlParent.Controls)
			{
				if(ctrl==ctrlReference)
					return true;
				bool bRet=IsAnyControl(ctrl,ctrlReference);
				if(bRet)
					return true;
			}
			return false;
		}

		private int GetGrabHandleTaskPaneHeight()
		{
			if(this.Font!=null)
				return Math.Max(GrabHandleTaskPaneHeight, this.Font.Height+2);
			return GrabHandleTaskPaneHeight;
		}

        private int _CaptionHeight = 0;
        /// <summary>
        /// Gets or sets docked bar caption height. Default value is 0 which means system predefined height is used.
        /// </summary>
        [DefaultValue(0), Category("Appearance"), Description("Indicates docked bar caption height. Default value is 0 which means system predefined height is used.")]
        public int CaptionHeight
        {
            get { return _CaptionHeight; }
            set { _CaptionHeight = value; this.RecalcLayout(); }
        }

		private int GetGrabHandleCaptionHeight()
		{
            if (_CaptionHeight > 0) return _CaptionHeight;
			if(this.Font!=null)
				return Math.Max(GrabHandleCaptionHeight, this.Font.Height+2);
			return GrabHandleCaptionHeight;
		}

		/// <summary>
		/// Gets or sets whether bar when changed over to floating state is hidden instead of shown. This property is used
		/// internally to optimize loading of hidden floating bars. You should not use this property in your code. It is for internal DotNetBar
		/// infrastructure use only.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool LoadingHideFloating
		{
			get {return m_LoadingHideFloating;}
			set {m_LoadingHideFloating=value;}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void DockDocumentManager(DockSiteInfo pDockInfo)
        {
            DocumentDockUIManager dm = pDockInfo.objDockSite.GetDocumentUIManager();
            if (pDockInfo.objDockSite.Dock == DockStyle.Fill)
                dm.Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
            else if ((pDockInfo.DockLine == -1 || pDockInfo.DockLine == 999) && (pDockInfo.objDockSite.Dock == DockStyle.Left || pDockInfo.objDockSite.Dock == DockStyle.Right)) // Ajdust for edge case docking
            {
                dm.Dock(null, this, pDockInfo.DockLine == -1 ? eDockSide.Left : eDockSide.None);
            }
            else if ((pDockInfo.DockLine == -1 || pDockInfo.DockLine == 999) && (pDockInfo.objDockSite.Dock == DockStyle.Top || pDockInfo.objDockSite.Dock == DockStyle.Bottom)) // Ajdust for edge case docking
            {
                dm.Dock(null, this, pDockInfo.DockLine == -1 ? eDockSide.Top : eDockSide.None);
            }
            else
                dm.Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
        }

        internal void RemoveFromFloatingContainer()
        {
            if (m_Float == null) return;
            // Remember undocked size
            m_FloatingRect = new Rectangle(m_Float.Location, this.Size);
            m_Float.Controls.Remove(this);
            m_Float.Hide();
            m_Float.Dispose();
            m_Float = null;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void DockingHandler(DockSiteInfo pDockInfo,Point p)
		{
			IOwner owner=m_Owner as IOwner;
			
			if(m_TempTabBar!=null && pDockInfo.TabDockContainer==m_TempTabBar)
				return;

			if(pDockInfo.objDockSite==null && (m_BarState!=eBarState.Floating || m_Float==null))
			{
				m_DockingInProgress=true;
				
				if(m_TempTabBar!=null)
				{
					RemoveTempTabBarItems();
					m_TempTabBar.RecalcLayout();
					m_TempTabBar=null;
				}

				// Remember last docking info
                DockSiteInfo tempInfo = m_LastDockSiteInfo;
                m_LastDockSiteInfo = new DockSiteInfo();
                // Preserve the relative last docked to bar in case the return to same docking position is needed
                m_LastDockSiteInfo.LastRelativeDocumentId = tempInfo.LastRelativeDocumentId;
                m_LastDockSiteInfo.LastRelativeDockToBar = tempInfo.LastRelativeDockToBar;

				m_LastDockSiteInfo.DockedHeight=this.Height;
				m_LastDockSiteInfo.DockedWidth=this.Width;
				m_LastDockSiteInfo.DockLine=this.DockLine;
				m_LastDockSiteInfo.DockOffset=this.DockOffset;
				if(this.Parent!=null)
					m_LastDockSiteInfo.DockSide=this.Parent.Dock;
				else
					m_LastDockSiteInfo.DockSide=DockStyle.Left;

				if(this.Parent!=null && this.Parent is DockSite)
				{
					m_LastDockSiteInfo.InsertPosition=((DockSite)this.Parent).Controls.GetChildIndex(this);
					m_LastDockSiteInfo.objDockSite=(DockSite)this.DockedSite;
				}
				if(m_LastDockSiteInfo.objDockSite==null)
				{
					IOwnerBarSupport barSupp=m_Owner as IOwnerBarSupport;
					if(barSupp!=null)
					{
						switch(m_LastDockSiteInfo.DockSide)
						{
							case DockStyle.Left:
								m_LastDockSiteInfo.objDockSite=barSupp.LeftDockSite;
								break;
							case DockStyle.Right:
								m_LastDockSiteInfo.objDockSite=barSupp.RightDockSite;
								break;
							case DockStyle.Top:
								m_LastDockSiteInfo.objDockSite=barSupp.TopDockSite;
								break;
							case DockStyle.Bottom:
								m_LastDockSiteInfo.objDockSite=barSupp.BottomDockSite;
								break;
                            case DockStyle.Fill:
                                m_LastDockSiteInfo.objDockSite = barSupp.FillDockSite;
                                break;
						}
					}
				}

				// Undock the window
				m_BarState=eBarState.Floating;
				if(m_Float==null)
				{
					m_Float=new FloatingContainer(this);
					m_Float.CreateControl();
				}

				// Must reset the ActiveControl to null because on MDI Forms if this was not done
				// MDI form could not be closed if bar that had ActiveControl is floating.
				if(owner.ParentForm!=null && owner.ParentForm.ActiveControl==this)
				{
					owner.ParentForm.ActiveControl=null;
					this.Focus(); // Fixes the problem on SDI forms
				}
				else if(owner.ParentForm!=null && IsAnyControl(this,owner.ParentForm.ActiveControl))
				{
					owner.ParentForm.ActiveControl=null;
					this.Focus();
				}
				
				// Check for parent since if bar is deserialized there is no parent and state is Docked by default

				if(this.Parent!=null && this.Parent is DockSite)
				{
					if(((DockSite)this.Parent).IsDocumentDock || ((DockSite)this.Parent).DocumentDockContainer!=null)
					{
						((DockSite)this.Parent).GetDocumentUIManager().UnDock(this);
					}
					else
						((DockSite)this.Parent).RemoveBar(this);
				}

				this.Parent=null;
				m_Float.Controls.Add(this);
				if(!this.Visible/* && !m_BarDefinitionLoading*/)
					base.Visible=true;
				// IMPORTANT SINCE WE OVERRIDE BASE LOCATION WE HAVE TO USE BASE HERE TO ACTUALLY MOVE IT
				base.Location=new Point(0,0);

				m_FloatingRect=new Rectangle(m_FloatingRect.Location,GetFloatingSize());
				this.Size=m_FloatingRect.Size;
				
				this.DockOrientation=eOrientation.Horizontal;
				if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && m_AlwaysDisplayDockTab)
					RefreshDockTab(true);
				this.RecalcSize();
				m_FloatingRect.Size=this.Size;

				m_Float.Location=p;
				if(m_ItemContainer.LayoutType!=eLayoutType.Toolbar)
					m_MouseDownPt=new Point(this.Width/2,8);
				else
					m_MouseDownPt=new Point(8,8);
				
				if(m_LoadingHideFloating)
					m_Float.Visible=false;
				else
					m_Float.Show();

				m_FloatingRect.Location=m_Float.Location;
				
				// TODO: Bug Width was sometimes not reflected properly
				if(m_Float.Width!=m_FloatingRect.Width)
					m_Float.Width=m_FloatingRect.Width;

				if(owner.ParentForm!=null)
					owner.ParentForm.Activate();
				m_DockingInProgress=false;

				// Raise events
				if(BarUndock!=null)
					BarUndock(this,new EventArgs());
				IOwnerBarSupport ownerDockEvents=m_Owner as IOwnerBarSupport;
				if(ownerDockEvents!=null)
					ownerDockEvents.InvokeBarUndock(this,new EventArgs());

				// Resize Docking Tab if it exists
				ResizeDockTab();
			}
			else
			{
				// Change the Z-Order of the dock-site if needed
				if((pDockInfo.FullSizeDock || pDockInfo.PartialSizeDock) && pDockInfo.DockSiteZOrderIndex>=0 && pDockInfo.objDockSite!=null && pDockInfo.objDockSite.Parent!=null)
					pDockInfo.objDockSite.Parent.Controls.SetChildIndex(pDockInfo.objDockSite,pDockInfo.DockSiteZOrderIndex);

				if(m_Owner!=null)
				{
					if(pDockInfo.TabDockContainer!=null)
					{
						if(m_TempTabBar!=null && pDockInfo.TabDockContainer!=m_TempTabBar)
						{
							RemoveTempTabBarItems();
							m_TempTabBar.RecalcLayout();
							m_TempTabBar=null;
						}
						if(m_TempTabBar!=pDockInfo.TabDockContainer)
						{
							m_DockingInProgress=true;
														
							if(m_DockTabTearOffIndex==-1)
							{
								if(m_Float!=null && m_Float.Visible)
								{
									if(owner.ParentForm!=null)
										owner.ParentForm.Activate();
									// Remember undocked size
									m_FloatingRect=new Rectangle(m_Float.Location,this.Size);
									m_DockOffset=pDockInfo.DockOffset;
									m_DockLine=pDockInfo.DockLine;
									m_Float.Controls.Remove(this);
									m_Float.Hide();
									m_Float.Dispose();
									m_Float=null;
								}
								else if(this.Parent!=null && this.Parent is DockSite)
									((DockSite)this.Parent).RemoveBar(this);
								m_BarState=eBarState.Docked;
								if(!m_BarDefinitionLoading)
									base.Visible=false;

								foreach(BaseItem item in m_ItemContainer.SubItems)
								{
									DockContainerItem dockitem=item as DockContainerItem;
									if(dockitem!=null)
									{
										DockContainerItem temp=new DockContainerItem();
										temp.Displayed=false;
										temp.Text=item.Text;
										temp.Image=dockitem.Image;
										temp.ImageIndex=dockitem.ImageIndex;
										temp.Icon=dockitem.Icon;
										temp.Tag="systempdockitem";
										pDockInfo.TabDockContainer.Items.Add(temp);
									}
									m_TempTabBar=pDockInfo.TabDockContainer;
									m_TempTabBar.RecalcLayout();
									m_TempTabBar.Refresh();
								}
							}
							else
							{
								DockContainerItem dockitem=m_ItemContainer.SubItems[m_DockTabTearOffIndex] as DockContainerItem;
								if(dockitem!=null)
								{
									DockContainerItem temp=new DockContainerItem();
									temp.Displayed=false;
									temp.Text=dockitem.Text;
									temp.Image=dockitem.Image;
									temp.ImageIndex=dockitem.ImageIndex;
									temp.Icon=dockitem.Icon;
									temp.Tag="systempdockitem";
									pDockInfo.TabDockContainer.Items.Add(temp);
								}
								m_TempTabBar=pDockInfo.TabDockContainer;
								m_TempTabBar.RecalcLayout();
								m_TempTabBar.Refresh();
							}
							m_DockingInProgress=false;
						}

					}
					else
					{
						if(m_TempTabBar!=null)
						{
							// Allow tabbed bar to change position only if it is going to different bar
							if(pDockInfo.objDockSite==m_TempTabBar.Parent)
								return;
							RemoveTempTabBarItems();
							m_TempTabBar.RecalcLayout();
							m_TempTabBar=null;
						}
						// If coming from the tabs
						if(!this.Visible && !m_BarDefinitionLoading)
							this.Visible=true;
                        if (pDockInfo.LastRelativeDockToBar != null && pDockInfo.LastRelativeDockToBar.DockedSite != null && owner is DotNetBarManager)
                        {
                            DotNetBarManager manager = (DotNetBarManager)owner;
                            if (pDockInfo.LastRelativeDockToBar.AutoHide)
                            {
                                if (pDockInfo.objDockSite != null)
                                    pDockInfo.objDockSite.GetDocumentUIManager().Dock(this);
                                else
                                    manager.Dock(this, eDockSide.Right);
                            }
                            else if (!pDockInfo.LastRelativeDockToBar.Visible || 
                                pDockInfo.LastRelativeDockToBar.DockSide == eDockSide.None) // Closed or floating
                            {
                                manager.Dock(this, m_LastDockSiteInfo.LastDockSiteSide);
                            }
                            else if (pDockInfo.LastRelativeDockToBar.DockSide != eDockSide.None &&
                           m_LastDockSiteInfo.LastDockSiteSide != pDockInfo.LastRelativeDockToBar.DockSide) // Reference bar docked somewhere else
                            {
                                manager.Dock(this, m_LastDockSiteInfo.LastDockSiteSide);
                            }
                            else
                                manager.Dock(this, pDockInfo.LastRelativeDockToBar, LastDockSide);
                            if (this.IsDisposed) return;
                            m_BarState = eBarState.Docked;
                            pDockInfo.LastRelativeDockToBar = null;
                        }
                        else if (pDockInfo.objDockSite != null && m_BarState != eBarState.Docked)
                        {
                            m_DockingInProgress = true;
                            if (m_Float != null && m_BarState == eBarState.Floating)
                            {
                                if (owner.ParentForm != null)
                                    owner.ParentForm.Activate();
                                // Remember undocked size
                                m_FloatingRect = new Rectangle(m_Float.Location, this.Size);
                                m_DockOffset = pDockInfo.DockOffset;
                                m_DockLine = pDockInfo.DockLine;
                                m_Float.Controls.Remove(this);
                                m_Float.Hide();
                                m_Float.Dispose();
                                m_Float = null;
                            }
                            m_BarState = eBarState.Docked;

                            if (pDockInfo.objDockSite.IsDocumentDock || pDockInfo.objDockSite.DocumentDockContainer != null)
                            {
                                DockDocumentManager(pDockInfo);
                            }
                            else
                            {
                                if (pDockInfo.InsertPosition == -10)
                                    pDockInfo.objDockSite.AddBar(this);
                                else
                                    pDockInfo.objDockSite.AddBar(this, pDockInfo.InsertPosition);
                            }
                            m_DockingInProgress = false;

                            //							// Raise events
                            //							if(BarDock!=null)
                            //								BarDock(this,new EventArgs());
                            //							IOwnerBarSupport ownerDockEvents=m_Owner as IOwnerBarSupport;
                            //							if(ownerDockEvents!=null)
                            //								ownerDockEvents.InvokeBarDock(this,new EventArgs());
                        }
                        else if (pDockInfo.objDockSite != null && pDockInfo.objDockSite != this.Parent)
                        {
                            m_DockingInProgress = true;
                            // Must reset the ActiveControl to null becouse on MDI Forms if this was not done
                            // MDI form could not be closed if bar that had ActiveControl is floating.
                            if (owner.ParentForm != null && owner.ParentForm.ActiveControl == this)
                            {
                                owner.ParentForm.ActiveControl = null;
                                this.Focus(); // Fixes the problem on SDI forms
                            }
                            else if (owner.ParentForm != null && IsAnyControl(this, owner.ParentForm.ActiveControl))
                            {
                                owner.ParentForm.ActiveControl = null;
                                this.Focus();
                            }

                            // It is docked somewhere else, we need to undockit and dockit on another site
                            // If Bar is deserialized there is no parent
                            if (this.Parent != null && this.Parent is DockSite)
                            {
                                if (((DockSite)this.Parent).IsDocumentDock || ((DockSite)this.Parent).DocumentDockContainer != null)
                                    ((DockSite)this.Parent).GetDocumentUIManager().UnDock(this);
                                else
                                    ((DockSite)this.Parent).RemoveBar(this);
                            }

                            // If coming from the tabs
                            if (!this.Visible && !m_BarDefinitionLoading)
                                this.Visible = true;

                            m_DockOffset = pDockInfo.DockOffset;
                            m_DockLine = pDockInfo.DockLine;

                            if (pDockInfo.objDockSite.IsDocumentDock || pDockInfo.objDockSite.DocumentDockContainer != null)
                            {
                                pDockInfo.objDockSite.GetDocumentUIManager().Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
                            }
                            else
                            {
                                if (pDockInfo.InsertPosition == -10)
                                    pDockInfo.objDockSite.AddBar(this);
                                else
                                    pDockInfo.objDockSite.AddBar(this, pDockInfo.InsertPosition);
                            }
                            // Raise events
                            //							if(BarDock!=null)
                            //								BarDock(this,new EventArgs());
                            //							IOwnerBarSupport ownerDockEvents=m_Owner as IOwnerBarSupport;
                            //							if(ownerDockEvents!=null)
                            //								ownerDockEvents.InvokeBarDock(this,new EventArgs());
                            m_DockingInProgress = false;
                        }
                        else if (this.Parent != null && pDockInfo.objDockSite == this.Parent)
                        {
                            if (pDockInfo.objDockSite.IsDocumentDock || pDockInfo.objDockSite.DocumentDockContainer != null)
                            {
                                pDockInfo.objDockSite.GetDocumentUIManager().Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
                            }
                            else
                            {
                                if (m_DockLine != pDockInfo.DockLine || m_DockOffset != pDockInfo.DockOffset && !this.Stretch || pDockInfo.objDockSite.Controls.GetChildIndex(this) != pDockInfo.InsertPosition || pDockInfo.NewLine)
                                {
                                    m_DockLine = pDockInfo.DockLine;
                                    m_DockOffset = pDockInfo.DockOffset;
                                    // If coming from the tabs
                                    if (!this.Visible && !m_BarDefinitionLoading)
                                        this.Visible = true;
                                    if (pDockInfo.NewLine)
                                        pDockInfo.objDockSite.SetBarPosition(this, pDockInfo.InsertPosition, true);
                                    else if (pDockInfo.InsertPosition == -10)
                                    {
                                        pDockInfo.objDockSite.AdjustBarPosition(this);
                                        pDockInfo.objDockSite.RecalcLayout();
                                    }
                                    else
                                        pDockInfo.objDockSite.SetBarPosition(this, pDockInfo.InsertPosition);
                                }
                            }
                        }
                        else
                        {
                            Point newLocation = new Point(p.X - m_MouseDownPt.X, p.Y - m_MouseDownPt.Y);
                            ScreenInformation screen = BarFunctions.ScreenFromControl(m_Float);
                            if (screen != null)
                            {
                                if (newLocation.Y + 8 >= screen.WorkingArea.Bottom)
                                    newLocation.Y = screen.WorkingArea.Bottom - 8;
                            }
                            m_Float.Location = newLocation;
                            if (((IOwner)m_Owner).ParentForm != null)
                                ((IOwner)m_Owner).ParentForm.Update();
                        }
					}
				}
				if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && (m_TabDockItems==null || !m_TabDockItems.Visible))
				{
					int iVisible=m_ItemContainer.VisibleSubItems;
					if(!(m_BarState==eBarState.Floating && iVisible<=1) && (m_AlwaysDisplayDockTab || iVisible>0) )
					{
						eBarState oldBarState=m_BarState;
						m_BarState=eBarState.Docked;
						RefreshDockTab(false);
						m_BarState=oldBarState;
					}
				}
                if(m_BarState!=eBarState.Floating)
				    InvokeBarDockEvents();
			}
		}

		internal void InvokeBarDockEvents()
		{
			// Raise events
			if(BarDock!=null)
				BarDock(this,new EventArgs());
			IOwnerBarSupport ownerDockEvents=m_Owner as IOwnerBarSupport;
			if(ownerDockEvents!=null)
				ownerDockEvents.InvokeBarDock(this,new EventArgs());
		}

		private Size GetFloatingSize()
		{
			Size size=m_FloatingRect.Size;
			if(size.IsEmpty)
			{
				if(m_ItemContainer.LayoutType==eLayoutType.DockContainer)
				{
					int tab=this.SelectedDockTab;
					if(tab>=0 && m_ItemContainer.SubItems[tab] is DockContainerItem)
					{
						DockContainerItem item=m_ItemContainer.SubItems[tab] as DockContainerItem;
						size=item.DefaultFloatingSize;
					}
					else if(m_ItemContainer.SubItems.Count>0 && m_ItemContainer.SubItems[0] is DockContainerItem)
					{
						DockContainerItem item=m_ItemContainer.SubItems[0] as DockContainerItem;
						size=item.DefaultFloatingSize;
					}
					else
					{
						if(m_ItemContainer.Orientation==eOrientation.Vertical)
							size=new Size(m_ItemContainer.WidthInternal+16,m_ItemContainer.WidthInternal+16);
						else
							size=new Size(m_ItemContainer.HeightInternal+24,m_ItemContainer.HeightInternal+24);
					}
				}
				else
					size=System.Windows.Forms.Screen.FromControl(this).WorkingArea.Size;
			}

			return size;
		}

		private void RemoveTempTabBarItems()
		{
			if(m_TempTabBar!=null)
			{
				System.Collections.ArrayList list=new System.Collections.ArrayList(m_TempTabBar.Items.Count);
				m_TempTabBar.Items.CopyTo(list);
				foreach(BaseItem item in list)
				{
					if(item.Tag!=null && item.Tag.ToString()=="systempdockitem")
						m_TempTabBar.Items.Remove(item);
				}
			}
		}

		internal Bar TempTabBar
		{
			get {return m_TempTabBar;}
		}

		internal void DragMouseMove()
		{
			Point p=this.PointToClient(Control.MousePosition);
			this.OnMouseMove(new MouseEventArgs(MouseButtons.Left,0,p.X,p.Y,0));
		}
		internal void DragMouseUp()
		{
			Point p=this.PointToClient(Control.MousePosition);
			this.OnMouseUp(new MouseEventArgs(MouseButtons.Left,0,p.X,p.Y,0));
		}

		internal void InternalMouseMove(MouseEventArgs e)
		{
			this.OnMouseMove(e);
		}

		/// <summary>
		/// Returns true if bar is being moved/dragged by user.
		/// </summary>
		internal bool IsBarMoving
		{
			get {return m_MoveWindow;}
		}

		/// <summary>
		/// Method is called by DotNetBarManager when bar is being moved and Escape key is pressed.
		/// </summary>
		internal void OnEscapeKey()
		{
			EndDocking(true,Point.Empty);
		}

        protected override void OnFontChanged(EventArgs e)
        {
            this.InvalidateFontChange();
            base.OnFontChanged(e);
        }

        private void InvalidateFontChange()
        {
            if(m_ItemContainer!=null)
                BarUtilities.InvalidateFontChange(m_ItemContainer.SubItems);
        }

		private bool m_InMouseMove=false;
		private Rectangle m_OldOutlineRectangle=Rectangle.Empty;
		private Form m_OutlineForm=null;
		//private bool m_OutlineDrag=true;
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(m_InMouseMove)
				return;
			m_InMouseMove=true;
			// Start window dragging
			if(m_MoveWindow && m_BarState!=eBarState.Popup && e.Button==System.Windows.Forms.MouseButtons.Left)
			{
				if(this.Cursor!=System.Windows.Forms.Cursors.SizeAll)
					this.Cursor=System.Windows.Forms.Cursors.SizeAll;

				Point p=Control.MousePosition;
				Point p2=this.PointToClient(p);
				IOwnerBarSupport ownerDock=m_Owner as IOwnerBarSupport;

				// Graceful exit
				if(ownerDock==null || Math.Abs(p2.X-m_MouseDownPt.X)<=4 && Math.Abs(p2.Y-m_MouseDownPt.Y)<=4)
				{
					base.OnMouseMove(e);
					m_InMouseMove=false;
					return;
				}
				DockSiteInfo pDockInfo=ownerDock.GetDockInfo(this,p.X,p.Y);
				if(pDockInfo.objDockSite==null && !m_CanUndock)
				{
					base.OnMouseMove(e);
					m_InMouseMove=false;
					return;
				}
				bool bPreview=true;
				
				if(!m_OldOutlineRectangle.IsEmpty)
				{
					NativeFunctions.DrawReversibleDesktopRect(m_OldOutlineRectangle,3);
					m_OldOutlineRectangle=Rectangle.Empty;
				}

                if (pDockInfo.UseOutline && pDockInfo.objDockSite != null)
                {
                    Rectangle r = pDockInfo.objDockSite.GetBarDockRectangle(this, ref pDockInfo);
                    if (!r.IsEmpty)
                    {
                        bPreview = false;
                        if (m_OutlineForm == null)
                            m_OutlineForm = CreateOutlineForm();
                        NativeFunctions.SetWindowPos(m_OutlineForm.Handle, NativeFunctions.HWND_TOP, r.X, r.Y, r.Width, r.Height, NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
                    }
                }
				if(bPreview && m_OutlineForm!=null)
				{
					m_OutlineForm.Visible=false;
				}

				m_DragDockInfo=pDockInfo;

				if(bPreview)
				{
					//if(m_BarState==eBarState.Floating || this.LayoutType!=eLayoutType.DockContainer || pDockInfo.TabDockContainer!=null)
						DockingHandler(pDockInfo,p);
				}
			}
			else if(e.Button==System.Windows.Forms.MouseButtons.None)
			{
				if(m_BarState==eBarState.Floating)
				{
					if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && ((e.X<=2 && e.Y<=2) || (e.X>=this.Width-4 && e.Y>=this.Height-4)))
						this.Cursor=System.Windows.Forms.Cursors.SizeNWSE;
					else if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && ((e.X>=this.Width-2 && e.Y<=2) || (e.X<=4 && e.Y>=this.Height-4)))
						this.Cursor=System.Windows.Forms.Cursors.SizeNESW;
					else if(e.X>=0 && e.X<=2 || e.X>=this.Width-3 && e.X<=this.Width)
						this.Cursor=System.Windows.Forms.Cursors.SizeWE;
					else if(e.Y>=0 && e.Y<=2 || e.Y>=this.Height-3 && e.Y<=this.Height)
						this.Cursor=System.Windows.Forms.Cursors.SizeNS;
					else if(this.Cursor==System.Windows.Forms.Cursors.SizeWE || this.Cursor==System.Windows.Forms.Cursors.SizeNS || this.Cursor==System.Windows.Forms.Cursors.SizeNWSE || this.Cursor==System.Windows.Forms.Cursors.SizeNESW)
						this.Cursor=System.Windows.Forms.Cursors.Default;
				}
                //else if(m_BarState==eBarState.Docked && m_ItemContainer.LayoutType==eLayoutType.DockContainer && m_ItemContainer.Stretch)
                //{
                //    if((e.X<=2 && this.DockedSite.Dock==DockStyle.Right) || (e.X>=this.Width-2 && this.DockedSite.Dock==DockStyle.Left))
                //        this.Cursor=Cursors.VSplit;	
                //    else if((e.Y<=2 && this.DockedSite.Dock==DockStyle.Bottom) || (e.Y>=this.Height-2 && this.DockedSite.Dock==DockStyle.Top))
                //        this.Cursor=Cursors.HSplit;
                //    else if(e.X<=2 && (this.DockedSite.Dock==DockStyle.Bottom || this.DockedSite.Dock==DockStyle.Top) && this.Left>0)
                //        this.Cursor=Cursors.VSplit;
                //    else if(e.Y<=2 && (this.DockedSite.Dock==DockStyle.Left || this.DockedSite.Dock==DockStyle.Right) && this.Top>0)
                //        this.Cursor=Cursors.HSplit;
                //    else if(this.Cursor==Cursors.HSplit || this.Cursor==Cursors.VSplit)
                //        this.Cursor=Cursors.Default;
                //}
				else if(m_BarState==eBarState.AutoHide && m_ItemContainer.LayoutType==eLayoutType.DockContainer && m_ItemContainer.Stretch)
				{
					if((e.X<=2 && m_LastDockSiteInfo.DockSide==DockStyle.Right) || (e.X>=this.Width-2 && m_LastDockSiteInfo.DockSide==DockStyle.Left))
						this.Cursor=Cursors.VSplit;	
					else if((e.Y<=2 && m_LastDockSiteInfo.DockSide==DockStyle.Bottom) || (e.Y>=this.Height-2 && m_LastDockSiteInfo.DockSide==DockStyle.Top))
						this.Cursor=Cursors.HSplit;
					else if(e.X<=2 && (m_LastDockSiteInfo.DockSide==DockStyle.Bottom || m_LastDockSiteInfo.DockSide==DockStyle.Top) && this.Left>0)
						this.Cursor=Cursors.VSplit;
					else if(e.Y<=2 && (m_LastDockSiteInfo.DockSide==DockStyle.Left || m_LastDockSiteInfo.DockSide==DockStyle.Right) && this.Top>0)
						this.Cursor=Cursors.HSplit;
					else if(this.Cursor==Cursors.HSplit || this.Cursor==Cursors.VSplit)
						this.Cursor=Cursors.Default;
				}
                else if (m_BarState == eBarState.Docked && m_GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
                {
                    if ((e.X > this.Location.X + this.Width - GrabHandleResizeWidth && this.RightToLeft == RightToLeft.No) ||
                        (e.X < this.Location.X + +GrabHandleResizeWidth && this.RightToLeft == RightToLeft.Yes))
                    {
                        Form form = this.FindForm();
                        if (form != null && form.WindowState == FormWindowState.Maximized)
                        {
                            if (this.Cursor == Cursors.SizeNESW || this.Cursor == Cursors.SizeNWSE)
                                this.Cursor = Cursors.Default;
                        }
                        else
                        {
                            if (this.RightToLeft == RightToLeft.Yes)
                                this.Cursor = Cursors.SizeNESW;
                            else
                                this.Cursor = Cursors.SizeNWSE;
                        }
                    }
                    else if(this.Cursor == Cursors.SizeNESW || this.Cursor == Cursors.SizeNWSE)
                        this.Cursor = Cursors.Default;
                }

                if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.OfficeXP || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle))
				{
					if(!m_SystemButtons.CloseButtonRect.IsEmpty && m_SystemButtons.CloseButtonRect.Contains(e.X,e.Y))
					{
						if(!m_SystemButtons.MouseOverClose)
						{
							m_SystemButtons.MouseOverClose=true;
							PaintCloseButton();
						}
					}
					else if(m_SystemButtons.MouseOverClose)
					{
						m_SystemButtons.MouseOverClose=false;
						PaintCloseButton();
					}
					if(!m_SystemButtons.CaptionButtonRect.IsEmpty && m_SystemButtons.CaptionButtonRect.Contains(e.X,e.Y))
					{
						if(!m_SystemButtons.MouseOverCaption)
						{
							m_SystemButtons.MouseOverCaption=true;
							PaintCaptionButton();
						}
					}
					else if(m_SystemButtons.MouseOverCaption)
					{
						m_SystemButtons.MouseOverCaption=false;
						PaintCaptionButton();
					}
					if(!m_SystemButtons.CustomizeButtonRect.IsEmpty && m_SystemButtons.CustomizeButtonRect.Contains(e.X,e.Y))
					{
						if(!m_SystemButtons.MouseOverCustomize)
						{
							m_SystemButtons.MouseOverCustomize=true;
							PaintCustomizeButton();
						}
					}
					else if(m_SystemButtons.MouseOverCustomize)
					{
						m_SystemButtons.MouseOverCustomize=false;
						PaintCustomizeButton();
					}
					if(!m_SystemButtons.AutoHideButtonRect.IsEmpty && m_SystemButtons.AutoHideButtonRect.Contains(e.X,e.Y))
					{
						if(!m_SystemButtons.MouseOverAutoHide)
						{
							m_SystemButtons.MouseOverAutoHide=true;
							PaintAutoHideButton();
						}
					}
					else if(m_SystemButtons.MouseOverAutoHide)
					{
						m_SystemButtons.MouseOverAutoHide=false;
						PaintAutoHideButton();
					}
				}
			}
			else if(e.Button==System.Windows.Forms.MouseButtons.Left && m_SizeWindow!=0 && m_BarState==eBarState.Floating)
			{
				System.Drawing.Size oldSize=this.ClientSize;
				System.Drawing.Size newSize=System.Drawing.Size.Empty;
				System.Drawing.Size minSize=MinimumDockSize(m_ItemContainer.Orientation);
				if(m_SizeWindow==SIZE_NWN || m_SizeWindow==SIZE_NWS)
				{
					if(m_SizeWindow==SIZE_NWS)
						newSize=RecalcSizeOnly(new System.Drawing.Size(e.X,e.Y));
					else
						newSize=RecalcSizeOnly(new System.Drawing.Size(this.Width-e.X,this.Height-e.Y));
					if(!oldSize.Equals(newSize) && newSize.Width>=minSize.Width && newSize.Height>=minSize.Height)
					{
						if(m_SizeWindow==SIZE_NWS)
							this.ClientSize=newSize;
						else
						{
							m_Float.Location=new Point(m_Float.Left+(this.Width-newSize.Width),m_Float.Top+(this.Height-newSize.Height));
							this.ClientSize=newSize;
						}
						RecalcSize();
						this.Update();
						// Updates the parent form, there was a very bad lagging painting effect when dockable window is resized and the DotNetBar controls where behind it.
						IOwner owner=m_Owner as IOwner;
						if(owner!=null && owner.ParentForm!=null)
							owner.ParentForm.Update();
					}
				}
				else if(m_SizeWindow==SIZE_NEN || m_SizeWindow==SIZE_NES)
				{
					if(m_SizeWindow==SIZE_NES)
						newSize=RecalcSizeOnly(new System.Drawing.Size(this.Width-e.X,e.Y));
					else
						newSize=RecalcSizeOnly(new System.Drawing.Size(e.X,this.Height-e.Y));
					if(!oldSize.Equals(newSize) && newSize.Width>=minSize.Width && newSize.Height>=minSize.Height)
					{
						if(m_SizeWindow==SIZE_NES)
						{
							m_Float.Location=new Point(m_Float.Left+(this.Width-newSize.Width),m_Float.Top);
							this.ClientSize=newSize;
						}
						else
						{
							m_Float.Location=new Point(m_Float.Left,m_Float.Top+(this.Height-newSize.Height));
							this.ClientSize=newSize;
						}
						RecalcSize();
						this.Update();
						// Updates the parent form, there was a very bad lagging painting effect when dockable window is resized and the DotNetBar controls where behind it.
						IOwner owner=m_Owner as IOwner;
						if(owner!=null && owner.ParentForm!=null)
							owner.ParentForm.Update();
					}
				}
				else if(m_SizeWindow==SIZE_E)
				{
					newSize=RecalcSizeOnly(new System.Drawing.Size(e.X,this.Height));
					if(!oldSize.Equals(newSize) && newSize.Width>=minSize.Width && newSize.Height>=minSize.Height)
					{
						this.ClientSize=newSize;
						RecalcSize();
						this.Update();
						// Updates the parent form, there was a very bad lagging painting effect when dockable window is resized and the DotNetBar controls where behind it.
						IOwner owner=m_Owner as IOwner;
						if(owner!=null && owner.ParentForm!=null)
							owner.ParentForm.Update();
					}
				}
				else if(m_SizeWindow==SIZE_W)
				{
					newSize=RecalcSizeOnly(new System.Drawing.Size(this.Width-e.X,this.Height));
					if(!oldSize.Equals(newSize) && newSize.Width>=minSize.Width && newSize.Height>=minSize.Height)
					{
						int iRight=m_Float.Right;
						this.ClientSize=newSize;
						RecalcSize();
						m_Float.Left=m_Float.Left+iRight-m_Float.Right;
						this.Update();
						// Updates the parent form, there was a very bad lagging painting effect when dockable window is resized and the DotNetBar controls where behind it.
						IOwner owner=m_Owner as IOwner;
						if(owner!=null && owner.ParentForm!=null)
							owner.ParentForm.Update();
					}
				}
				else if(m_SizeWindow==SIZE_S)
				{
					if(e.Y>0)
					{
						if(m_ItemContainer.LayoutType==eLayoutType.TaskList || m_ItemContainer.LayoutType==eLayoutType.DockContainer)
							newSize=RecalcSizeOnly(new System.Drawing.Size(this.Width,e.Y));
						else
							newSize=RecalcSizeOnly(new System.Drawing.Size((int)(m_MouseDownSize.Width*((float)m_MouseDownSize.Height/(float)e.Y)),this.Height));
						if(!oldSize.Equals(newSize) && newSize.Width>=minSize.Width && newSize.Height>=minSize.Height)
						{
							this.ClientSize=newSize;
							RecalcSize();
							this.Update();
							// Updates the parent form, there was a very bad lagging painting effect when dockable window is resized and the DotNetBar controls where behind it.
							IOwner owner=m_Owner as IOwner;
							if(owner!=null && owner.ParentForm!=null)
								owner.ParentForm.Update();
						}
					}
				}
				else if(m_SizeWindow==SIZE_N)
				{
					if(e.Y!=0)
					{
						if(m_ItemContainer.LayoutType==eLayoutType.TaskList || m_ItemContainer.LayoutType==eLayoutType.DockContainer)
							newSize=RecalcSizeOnly(new Size(this.Width,this.Height-e.Y));
						else
							newSize=RecalcSizeOnly(new System.Drawing.Size((int)(m_MouseDownSize.Width*((float)m_MouseDownSize.Height/(float)(this.Height-e.Y))),this.Height));
						if(!oldSize.Equals(newSize) && newSize.Width>=minSize.Width && newSize.Height>=minSize.Height)
						{
							int iBottom=m_Float.Bottom;
							this.ClientSize=newSize;
							RecalcSize();
							m_Float.Top=m_Float.Top+iBottom-m_Float.Bottom;
							this.Update();
							// Updates the parent form, there was a very bad lagging painting effect when dockable window is resized and the DotNetBar controls where behind it.
							IOwner owner=m_Owner as IOwner;
							if(owner!=null && owner.ParentForm!=null)
								owner.ParentForm.Update();
						}
					}
				}
				m_FloatingRect=new Rectangle(this.Location,this.Size);
			}
            else if (e.Button == System.Windows.Forms.MouseButtons.Left && m_SizeWindow != 0 && m_BarState == eBarState.AutoHide || m_SizeWindow == SIZE_PARENTRESIZE && m_BarState == eBarState.Docked) //(m_BarState == eBarState.Docked || m_BarState == eBarState.AutoHide))
			{
				System.Drawing.Size oldSize=this.ClientSize;
				System.Drawing.Size newSize=System.Drawing.Size.Empty;
				if(m_SizeWindow==SIZE_PARENTRESIZE)
				{
                    if (this.RightToLeft == RightToLeft.No)
                    {
                        if (this.Parent != null && this.Parent.Parent != null)
                        {
                            Point pScreen = this.PointToScreen(new Point(e.X, e.Y));
                            if (this.Parent.Parent.Parent != null)
                            {
                                Point pl = this.Parent.Parent.Parent.PointToScreen(this.Parent.Parent.Location);
                                this.Parent.Parent.Size = new Size(pScreen.X - pl.X + m_ResizeOffset.X, pScreen.Y - pl.Y + m_ResizeOffset.Y);
                            }
                            else
                                this.Parent.Parent.Size = new Size(pScreen.X - this.Parent.Parent.Location.X + m_ResizeOffset.X, pScreen.Y - this.Parent.Parent.Location.Y + m_ResizeOffset.Y);
                            IOwner owner = m_Owner as IOwner;
                            if (owner != null && owner.ParentForm != null)
                                owner.ParentForm.Update();
                        }
                        else if (m_Owner == null && !(this.Parent is DockSite) && !(this.Parent is FloatingContainer))
                        {
                            Form form = this.FindForm();
                            if (form != null)
                            {
                                Point pScreen = this.PointToScreen(new Point(e.X, e.Y));
                                form.Size = new Size(pScreen.X - form.Location.X + m_ResizeOffset.X, pScreen.Y - form.Location.Y + m_ResizeOffset.Y);
                                form.Update();
                            }
                        }
                    }
                    else
                    {
                        if (this.Parent != null && this.Parent.Parent != null)
                        {
                            Point pScreen = this.PointToScreen(new Point(e.X, e.Y));
                            if (this.Parent.Parent.Parent != null)
                            {
                                Point pl = this.Parent.Parent.Parent.PointToScreen(this.Parent.Parent.Location);
                                Rectangle b = this.Parent.Parent.Bounds;
                                b.X = pScreen.X - m_ResizeOffset.X;
                                b.Width += this.Parent.Parent.Left - b.X;
                                b.Height = pScreen.Y - pl.Y + m_ResizeOffset.Y;
                                this.Parent.Parent.Bounds = b;
                            }
                            else
                            {
                                Rectangle b = this.Parent.Parent.Bounds;
                                b.X=pScreen.X - m_ResizeOffset.X;
                                b.Width += this.Parent.Parent.Left - b.X;
                                b.Height = pScreen.Y - this.Parent.Parent.Location.Y + m_ResizeOffset.Y;
                                this.Parent.Parent.Bounds = b;
                            }
                            IOwner owner = m_Owner as IOwner;
                            if (owner != null && owner.ParentForm != null)
                                owner.ParentForm.Update();
                        }
                        else if (m_Owner == null && !(this.Parent is DockSite) && !(this.Parent is FloatingContainer))
                        {
                            Form form = this.FindForm();
                            if (form != null)
                            {
                                Point pScreen = this.PointToScreen(new Point(e.X, e.Y));
                                Rectangle b = form.Bounds;
                                
                                b.X = pScreen.X - m_ResizeOffset.X;
                                b.Width += form.Left - b.X;
                                b.Height = pScreen.Y - form.Location.Y + m_ResizeOffset.Y;
                                form.Bounds = b;
                                //Size = new Size(pScreen.X - form.Location.X + m_ResizeOffset.X, pScreen.Y - form.Location.Y + m_ResizeOffset.Y);
                                form.Update();
                            }
                        }
                    }
				}
				else if(m_SizeWindow==SIZE_HSPLITRIGHT)
				{
					int formClientWidth=GetFormClientWidth();
					int mouseX=e.X;
                    int minClientSize = 32;
                    if (m_Owner is DotNetBarManager) minClientSize = ((DotNetBarManager)m_Owner).MinimumClientSize.Width;

                    if (minClientSize > 0 && formClientWidth + mouseX < minClientSize)
                        mouseX = minClientSize - formClientWidth;

                    if (formClientWidth + mouseX >= minClientSize || minClientSize == 0)
					{
						int oldMinWidth=m_ItemContainer.MinWidth;
						m_ItemContainer.MinWidth=0;
						newSize=RecalcSizeOnly(new Size(this.Width-mouseX,this.Height));
						m_ItemContainer.MinWidth=oldMinWidth;
						if(!oldSize.Equals(newSize))
						{
							if(m_BarState==eBarState.AutoHide)
							{
								Rectangle oldRect=this.Bounds;
								this.EnableRedraw=false;
								try
								{
									m_ItemContainer.MinWidth=m_ItemContainer.WidthInternal-mouseX;
									this.Width=this.Width-mouseX;
									RecalcSize();
									this.Left=this.Left+(oldSize.Width-this.Size.Width);
								}
								finally
								{
									this.EnableRedraw=true;
								}
								if(this.Parent!=null)
								{
									this.Parent.Invalidate(oldRect,true);
									this.Parent.Invalidate(this.Bounds,true);
									this.Parent.Update();
								}
								else
									this.Refresh();
							}
							else
							{
								int iOldMinWidth=m_ItemContainer.MinWidth;
								m_ItemContainer.MinWidth=m_ItemContainer.WidthInternal-mouseX;

								if(m_ItemContainer.MinWidth<iOldMinWidth)
									SyncLineMinWidth();
								RecalcLayout();
							}
							if(m_BarState==eBarState.AutoHide)
								m_LastDockSiteInfo.DockedWidth=this.Width;
						}
					}
				}
				else if(m_SizeWindow==SIZE_HSPLITLEFT)
				{
					int formClientWidth=GetFormClientWidth();
					int mouseX=e.X;
                    int minClientSize = 32;
                    if (m_Owner is DotNetBarManager) minClientSize = ((DotNetBarManager)m_Owner).MinimumClientSize.Width;

					if(minClientSize>0 && formClientWidth-(mouseX-this.Width)<minClientSize)
					{
						mouseX=formClientWidth+this.Width-minClientSize;
					}

					if(formClientWidth-(mouseX-this.Width)>=minClientSize || minClientSize==0)
					{	
						int oldMinWidth=m_ItemContainer.MinWidth;
						m_ItemContainer.MinWidth=0;
						newSize=RecalcSizeOnly(new Size(mouseX,this.Height));
						m_ItemContainer.MinWidth=oldMinWidth;
						if(!oldSize.Equals(newSize))
						{
							if(m_BarState==eBarState.AutoHide)
							{
								Rectangle oldRect=this.Bounds;
								this.EnableRedraw=false;
								try
								{
									m_ItemContainer.MinWidth=m_ItemContainer.WidthInternal+(mouseX-this.Width);
									this.Width=this.Width+(mouseX-this.Width);
									RecalcSize();
								}
								finally
								{
									this.EnableRedraw=true;
								}
								if(this.Parent!=null)
								{
									this.Parent.Invalidate(oldRect,true);
									this.Parent.Invalidate(this.Bounds,true);
									this.Parent.Update();
								}
								else
									this.Refresh();								
							}
							else
							{
								int iOldMinWidth=m_ItemContainer.MinWidth;
								m_ItemContainer.MinWidth=m_ItemContainer.WidthInternal+(mouseX-this.Width);
								if(m_ItemContainer.MinWidth<iOldMinWidth)
									SyncLineMinWidth();
								RecalcLayout();
							}
							if(m_BarState==eBarState.AutoHide)
								m_LastDockSiteInfo.DockedWidth=this.Width;
						}
					}
				}
				else if(m_SizeWindow==SIZE_VSPLITBOTTOM)
				{
					int formClientHeight=GetFormClientHeight();
					int mouseY=e.Y;
                    int minClientSize = 32;
                    if (m_Owner is DotNetBarManager) minClientSize = ((DotNetBarManager)m_Owner).MinimumClientSize.Height;

					if(minClientSize>0 && formClientHeight+mouseY<minClientSize)
						mouseY=minClientSize-formClientHeight;
						
					if(formClientHeight+mouseY>=minClientSize || minClientSize==0)
					{
						int oldMinHeight=m_ItemContainer.MinHeight;
						m_ItemContainer.MinHeight=0;
						newSize=RecalcSizeOnly(new Size(this.Width,this.Height-mouseY));
						m_ItemContainer.MinHeight=oldMinHeight;
						if(!oldSize.Equals(newSize))
						{							
							if(m_BarState==eBarState.AutoHide)
							{
								Rectangle oldRect=this.Bounds;
								this.EnableRedraw=false;
								try
								{
									m_ItemContainer.MinHeight=m_ItemContainer.HeightInternal-mouseY;
									this.Height=this.Height-mouseY;
									RecalcSize();
									this.Top=this.Top+(oldSize.Height-this.Height);
								}
								finally
								{
									this.EnableRedraw=true;
								}
								if(this.Parent!=null)
								{
									this.Parent.Invalidate(oldRect,true);
									this.Parent.Invalidate(this.Bounds,true);
									this.Parent.Update();
								}
								else
									this.Refresh();
							}
							else
							{
								int iOldMinHeight=m_ItemContainer.MinHeight;
								Size minSize=this.MinimumDockSize(m_ItemContainer.Orientation);
								if(m_ItemContainer.HeightInternal-mouseY>=minSize.Height)
									m_ItemContainer.MinHeight=m_ItemContainer.HeightInternal-mouseY;
								if(m_ItemContainer.MinHeight<iOldMinHeight)
									SyncLineMinHeight();
								RecalcLayout();
							}
						
							if(m_BarState==eBarState.AutoHide)
								m_LastDockSiteInfo.DockedHeight=this.Height;
						}
					}
				}
				else if(m_SizeWindow==SIZE_VSPLITTOP)
				{
					int formClientHeight=GetFormClientHeight();
					int mouseY=e.Y;
                    int minClientSize = 32;
                    if (m_Owner is DotNetBarManager) minClientSize = ((DotNetBarManager)m_Owner).MinimumClientSize.Height;

					if(minClientSize>0 && formClientHeight-(mouseY-this.Height)<minClientSize)
						mouseY=formClientHeight+this.Height-minClientSize;

					if(formClientHeight-(mouseY-this.Height)>=minClientSize || minClientSize==0)
					{
						int oldMinHeight=m_ItemContainer.MinHeight;
						m_ItemContainer.MinHeight=0;
						newSize=RecalcSizeOnly(new Size(this.Width,mouseY));
						m_ItemContainer.MinHeight=oldMinHeight;
						if(!oldSize.Equals(newSize))
						{
							if(m_BarState==eBarState.AutoHide)
							{
								Rectangle oldRect=this.Bounds;
								this.EnableRedraw=false;
								try
								{
									m_ItemContainer.MinHeight=m_ItemContainer.HeightInternal+(mouseY-this.Height);
									this.Height=mouseY;
									this.RecalcSize();
								}
								finally
								{
									this.EnableRedraw=true;
								}
								if(this.Parent!=null)
								{
									this.Parent.Invalidate(oldRect,true);
									this.Parent.Invalidate(this.Bounds,true);
									this.Parent.Update();
								}
								else
									this.Refresh();
							}
							else
							{
								int iOldMinHeight=m_ItemContainer.MinHeight;
								m_ItemContainer.MinHeight=m_ItemContainer.HeightInternal+(mouseY-this.Height);
								if(m_ItemContainer.MinHeight<iOldMinHeight)
									SyncLineMinHeight();
								RecalcLayout();
							}
							
							if(m_BarState==eBarState.AutoHide)
								m_LastDockSiteInfo.DockedHeight=this.Height;
						}
					}
				}
				else if(m_SizeWindow==SIZE_HSPLIT && this.Parent.Controls.IndexOf(this)>0)
				{
					Bar barLeft=GetPreviousVisibleBar(this); //this.Parent.Controls[iIndex] as Bar;
					if(barLeft!=null && barLeft.DockLine==this.DockLine)
					{
						System.Drawing.Size minLeftSize=GetAdjustedFullSize(barLeft.MinimumDockSize(eOrientation.Horizontal));
						System.Drawing.Size minThisSize=GetAdjustedFullSize(this.MinimumDockSize(eOrientation.Horizontal));
                        int x = e.X;
                        if (barLeft.Width + x < minLeftSize.Width)
                            x += (minLeftSize.Width - (barLeft.Width + x))-1;
                        if (this.Width - x < minThisSize.Width)
                            x -= (minThisSize.Width - (this.Width - x))+1;

						if(barLeft.Width+x>=minLeftSize.Width && this.Width-x>=minThisSize.Width)
						{
							Size oldLeftBarSize=barLeft.Size;
							Size newBarLeftSize=barLeft.RecalcSizeOnly(new Size(barLeft.Width+x,this.Height));
							newSize=this.RecalcSizeOnly(new Size(this.Width-x,this.Height));
							if(!oldLeftBarSize.Equals(newBarLeftSize) && !oldSize.Equals(newSize))
							{
								this.SplitDockWidth=0;
								barLeft.SplitDockWidth=barLeft.Width+x;
                                foreach (Control c in this.Parent.Controls)
                                {
                                    if (c != this && c != barLeft && c.Visible && c is Bar)
                                    {
                                        Bar b = c as Bar;
                                        if (b.DockLine == this.DockLine && b.SplitDockHeight == 0)
                                            b.SplitDockWidth = b.Width;
                                    }
                                }
								RecalcLayout();
							}
						}
					}
				}
				else if(m_SizeWindow==SIZE_VSPLIT && this.Parent.Controls.IndexOf(this)>0)
				{
					// Resize two bars that are docked on the same line, this bar is always on the right side
					//int iIndex=this.Parent.Controls.IndexOf(this)-1;  // Index of the control to the left
					Bar barLeft=GetPreviousVisibleBar(this); //this.Parent.Controls[iIndex] as Bar;
					if(barLeft!=null && barLeft.DockLine==this.DockLine)
					{
                        System.Drawing.Size minLeftSize = GetAdjustedFullSize(barLeft.MinimumDockSize(eOrientation.Horizontal));
                        System.Drawing.Size minThisSize = GetAdjustedFullSize(this.MinimumDockSize(eOrientation.Horizontal));
                        int y = e.Y;
                        if (barLeft.Height + y < minLeftSize.Height)
                            y += (minLeftSize.Height - (barLeft.Height + y))-1;
                        if (this.Height - y < minThisSize.Height)
                            y -= minThisSize.Height - (this.Height - y) + 1;
                        if (barLeft.Height + y >= minLeftSize.Height && this.Height - y >= minThisSize.Height)
                        {
                            Size oldLeftBarSize = barLeft.Size;
                            Size newBarLeftSize = barLeft.RecalcSizeOnly(new Size(barLeft.Width, this.Height + y));
                            newSize = this.RecalcSizeOnly(new Size(this.Width, this.Height - y));
                            if (!oldLeftBarSize.Equals(newBarLeftSize) && !oldSize.Equals(newSize))
                            {
                                this.SplitDockHeight = 0;
                                barLeft.SplitDockHeight = barLeft.Height + y;
                                foreach (Control c in this.Parent.Controls)
                                {
                                    if (c != this && c != barLeft && c.Visible && c is Bar)
                                    {
                                        Bar b = c as Bar;
                                        if(b.DockLine==this.DockLine && b.SplitDockHeight==0)
                                            b.SplitDockHeight = b.Height;
                                    }
                                }
                                RecalcLayout();
                            }
                        }
					}
				}
			}

            if (m_BarState==eBarState.Popup && m_ParentItem != null && m_ParentItem.DesignMode && e.Button == System.Windows.Forms.MouseButtons.Left && (Math.Abs(e.X - m_MouseDownPt.X) >= 2 || Math.Abs(e.Y - m_MouseDownPt.Y) >= 2 || m_DragDropInProgress))
            {
                BaseItem focus = m_FocusItem;
                if (m_Owner is IOwner)
                    focus = ((IOwner)m_Owner).GetFocusItem();
                ISite site = this.GetSite();
                if (site != null && focus != null)
                {
                    DesignTimeMouseMove(e);
                }
            }

			base.OnMouseMove(e);
			if(m_ItemContainer.SubItems.Count==0)
			{
				m_InMouseMove=false;
				return;
			}
			if(!m_MoveWindow && m_SizeWindow==0)
				m_ItemContainer.InternalMouseMove(e);
			m_InMouseMove=false;
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Form CreateOutlineForm()
        {
            return BarFunctions.CreateOutlineForm();
        }
        private Bar GetPreviousVisibleBar(Bar startBar)
		{
			if(this.Parent==null)
				return null;
			int iIndex=this.Parent.Controls.IndexOf(this)-1;
			if(iIndex>=0)
			{
				for(int i=iIndex;i>=0;i--)
				{
					if(this.Parent.Controls[i].Visible)
						return this.Parent.Controls[i] as Bar;
				}
			}
			return null;
		}

		private Bar GetNextVisibleBar(Bar startBar)
		{
			if(this.Parent==null)
				return null;
			int iIndex=this.Parent.Controls.IndexOf(this)+1;
			if(iIndex>=0)
			{
				for(int i=iIndex;i<this.Parent.Controls.Count;i++)
				{
					if(this.Parent.Controls[i].Visible)
						return this.Parent.Controls[i] as Bar;
				}
			}
			return null;
		}

		private int GetFormClientWidth()
		{
			IOwner owner=m_Owner as IOwner;
			if(owner==null || this.Parent==null || (this.Parent.Parent==null && m_BarState!=eBarState.AutoHide))
				return 0;

			Control parentControl=this.Parent.Parent;
			if(m_BarState==eBarState.AutoHide)
				parentControl=owner.ParentForm;
            if (parentControl == null && owner.ParentForm == null && m_Owner is DotNetBarManager)
                parentControl = ((DotNetBarManager)m_Owner).ParentUserControl;

			if(parentControl==null)
				return 0;
			int width=parentControl.ClientSize.Width;

			foreach(Control ctrl in parentControl.Controls)
			{
				if(ctrl.Visible && (ctrl.Dock==DockStyle.Left || ctrl.Dock==DockStyle.Right))
					width-=ctrl.Width;
			}
			if(m_BarState==eBarState.AutoHide && this.Visible)
				width-=this.Width;
			return width;
		}

		private int GetFormClientHeight()
		{
			IOwner owner=m_Owner as IOwner;
			if(owner==null || this.Parent==null || (this.Parent.Parent==null && m_BarState!=eBarState.AutoHide))
				return 0;
			Control parentControl=null;
			if(m_BarState==eBarState.AutoHide)
				parentControl=owner.ParentForm;
			else
				parentControl=this.Parent.Parent;

            if (parentControl == null && owner.ParentForm == null && m_Owner is DotNetBarManager)
                parentControl = ((DotNetBarManager)m_Owner).ParentUserControl;

			if(parentControl==null)
				return 0;
			int height=parentControl.ClientSize.Height;

			foreach(Control ctrl in parentControl.Controls)
			{
				if(ctrl.Visible && (ctrl.Dock==DockStyle.Top || ctrl.Dock==DockStyle.Bottom))
					height-=ctrl.Height;
			}
			if(m_BarState==eBarState.AutoHide && this.Visible)
				height-=this.Height;
			return height;
		}

		internal void SyncLineMinWidth()
		{
			if(this.Parent==null)
				return;
			foreach(Control ctrl in this.Parent.Controls)
			{
				Bar bar=ctrl as Bar;
				if(bar==null || bar==this || bar.DockLine!=m_DockLine)
					continue;
				bar.ItemsContainer.MinWidth=m_ItemContainer.MinWidth;
			}
		}
		internal void SyncLineMinHeight()
		{
			if(this.Parent==null)
				return;
			foreach(Control ctrl in this.Parent.Controls)
			{
				Bar bar=ctrl as Bar;
				if(bar==null || bar==this || bar.DockLine!=m_DockLine)
					continue;
				bar.ItemsContainer.MinHeight=m_ItemContainer.MinHeight;
			}
		}
		
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
            InternalMouseHover();
		}

		internal void InternalMouseHover()
		{
			if(!m_MoveWindow)
			{
				if(Control.MouseButtons==MouseButtons.Left)
				{
					Point pClient=this.PointToClient(Control.MousePosition);
					if(!this.ClientRectangle.Contains(pClient))
					{
						IOwnerMenuSupport menu=this.Owner as  IOwnerMenuSupport;
						if(menu!=null)
						{
							if(menu.RelayMouseHover())
								return;
						}
					}
				}
				m_ItemContainer.InternalMouseHover();

				if(m_SystemButtons.MouseOverAutoHide)
				{
					using(LocalizationManager lm=new LocalizationManager(m_Owner as IOwnerLocalize))
					{
						string tip=lm.GetLocalizedString(LocalizationKeys.BarAutoHideButtonTooltip);
						if(tip!="")
							ShowToolTip(tip);
					}
				}
				else if(m_SystemButtons.MouseOverCustomize)
				{
					using(LocalizationManager lm=new LocalizationManager(m_Owner as IOwnerLocalize))
					{
						string tip=lm.GetLocalizedString(LocalizationKeys.BarCustomizeButtonTooltip);
						if(tip!="")
							ShowToolTip(tip);
					}
				}
				else if(m_SystemButtons.MouseOverClose)
				{
					using(LocalizationManager lm=new LocalizationManager(m_Owner as IOwnerLocalize))
					{
						string tip=lm.GetLocalizedString(LocalizationKeys.BarCloseButtonTooltip);
						if(tip!="")
							ShowToolTip(tip);
					}
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			// If we had hot sub item pass the mouse leave message to it...
			if(this.Cursor!=System.Windows.Forms.Cursors.Arrow)
				this.Cursor=System.Windows.Forms.Cursors.Arrow;

			if(m_SystemButtons.MouseOverClose)
			{
				m_SystemButtons.MouseOverClose=false;
				PaintCloseButton();
			}
			if(m_SystemButtons.MouseOverCaption)
			{
				m_SystemButtons.MouseOverCaption=false;
				PaintCaptionButton();
			}
			if(m_SystemButtons.MouseOverCustomize)
			{
				m_SystemButtons.MouseOverCustomize=false;
				PaintCustomizeButton();
			}
			if(m_SystemButtons.MouseOverAutoHide)
			{
				m_SystemButtons.MouseOverAutoHide=false;
				PaintAutoHideButton();
			}

			if(!m_MoveWindow)
				m_ItemContainer.InternalMouseLeave();

			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			m_MouseDownPt=new Point(e.X,e.Y);
			m_MouseDownSize=this.Size;
			HideToolTip();
			if(e.Button==MouseButtons.Left && !m_SystemButtons.CustomizeButtonRect.IsEmpty && m_SystemButtons.CustomizeButtonRect.Contains(m_MouseDownPt) && !this.DesignMode && !m_ItemContainer.DesignMode)
			{
				if(m_CustomizeMenu!=null)
				{
					if(m_CustomizeMenu.GetOwner()==null)
						m_CustomizeMenu.SetOwner(m_Owner);
					Point popupLocation;
                    if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
						popupLocation=new Point(0,16);
					else
						popupLocation=new Point(m_SystemButtons.CustomizeButtonRect.Left,m_SystemButtons.CustomizeButtonRect.Bottom);
					if(popupLocation.X<0)
						popupLocation.X=0;
					popupLocation=this.PointToScreen(popupLocation);
					m_CustomizeMenu.SetSourceControl(this);
					m_CustomizeMenu.Popup(popupLocation);
					base.OnMouseDown(e);
					return;
				}
				else
				{
					foreach(BaseItem objItem in m_ItemContainer.SubItems)
					{
						if(objItem is CustomizeItem && !objItem.Expanded)
						{
                            if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
								((CustomizeItem)objItem).PopupLocation=new Point(this.Left-128,this.Top+16);
							else
								((CustomizeItem)objItem).PopupLocation=new Point(m_SystemButtons.CustomizeButtonRect.Left,m_SystemButtons.CustomizeButtonRect.Bottom);

							objItem.Expanded=true;
							base.OnMouseDown(e);
							return;
						}
					}
				}
			}
			else if(e.Button==MouseButtons.Left && !m_SystemButtons.CloseButtonRect.IsEmpty && m_SystemButtons.CloseButtonRect.Contains(e.X,e.Y))
			{
				m_SystemButtons.MouseDownClose=true;
				PaintCloseButton();
				base.OnMouseDown(e);
				return;
			}
			else if(e.Button==MouseButtons.Left && !m_SystemButtons.CaptionButtonRect.IsEmpty && m_SystemButtons.CaptionButtonRect.Contains(e.X,e.Y))
			{
				m_SystemButtons.MouseDownCaption=true;
				PaintCaptionButton();
				base.OnMouseDown(e);
				return;
			}
			else if(e.Button==MouseButtons.Left && !m_SystemButtons.AutoHideButtonRect.IsEmpty && m_SystemButtons.AutoHideButtonRect.Contains(e.X,e.Y))
			{
				m_SystemButtons.MouseDownAutoHide=true;
				PaintAutoHideButton();
				base.OnMouseDown(e);
				return;
			}
			else if(e.Button==System.Windows.Forms.MouseButtons.Left && m_BarState==eBarState.Floating)
			{
				if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && e.X<=4 && e.Y<=4)
					m_SizeWindow=SIZE_NWN;
				else if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && e.X>=this.Width-4 && e.Y>=this.Height-4)
					m_SizeWindow=SIZE_NWS;
				else if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && e.X>=this.Width-4 && e.Y<=4)
					m_SizeWindow=SIZE_NEN;
				else if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && e.X<=4 && e.Y>=this.Height-4)
					m_SizeWindow=SIZE_NES;
				else if(e.X>=0 && e.X<=2)
					m_SizeWindow=SIZE_W;
				else if(e.X>=this.Width-3 && e.X<=this.Width)
					m_SizeWindow=SIZE_E;
				else if(e.Y>=0 && e.Y<=2)
					m_SizeWindow=SIZE_N;
				else if(e.Y>=this.Height-3 && e.Y<=this.Height)
					m_SizeWindow=SIZE_S;
			}
            //else if(e.Button==System.Windows.Forms.MouseButtons.Left && m_BarState==eBarState.Docked && m_ItemContainer.LayoutType==eLayoutType.DockContainer && m_ItemContainer.Stretch)
            //{
            //    if(this.DockedSite.Dock==DockStyle.Right && e.X<=2)
            //        m_SizeWindow=SIZE_HSPLITRIGHT;
            //    else if(this.DockedSite.Dock==DockStyle.Left && e.X>=this.Width-2)
            //        m_SizeWindow=SIZE_HSPLITLEFT;
            //    else if(this.DockedSite.Dock==DockStyle.Bottom && e.Y<=2)
            //        m_SizeWindow=SIZE_VSPLITBOTTOM;
            //    else if(this.DockedSite.Dock==DockStyle.Top && e.Y>=this.Height-2)
            //        m_SizeWindow=SIZE_VSPLITTOP;
            //    else if((this.DockedSite.Dock==DockStyle.Bottom || this.DockedSite.Dock==DockStyle.Top) && e.X<=2 && this.Left>0)
            //        m_SizeWindow=SIZE_HSPLIT;
            //    else if((this.DockedSite.Dock==DockStyle.Left || this.DockedSite.Dock==DockStyle.Right) && e.Y<=2 && this.Top>0)
            //        m_SizeWindow=SIZE_VSPLIT;
            //}
			else if(e.Button==System.Windows.Forms.MouseButtons.Left && m_BarState==eBarState.AutoHide && m_ItemContainer.LayoutType==eLayoutType.DockContainer && m_ItemContainer.Stretch)
			{
				if(m_LastDockSiteInfo.DockSide==DockStyle.Right && e.X<=2)
					m_SizeWindow=SIZE_HSPLITRIGHT;
				else if(m_LastDockSiteInfo.DockSide==DockStyle.Left && e.X>=this.Width-2)
					m_SizeWindow=SIZE_HSPLITLEFT;
				else if(m_LastDockSiteInfo.DockSide==DockStyle.Bottom && e.Y<=2)
					m_SizeWindow=SIZE_VSPLITBOTTOM;
				else if(m_LastDockSiteInfo.DockSide==DockStyle.Top && e.Y>=this.Height-2)
					m_SizeWindow=SIZE_VSPLITTOP;
				else if((m_LastDockSiteInfo.DockSide==DockStyle.Bottom || m_LastDockSiteInfo.DockSide==DockStyle.Top) && e.X<=2 && this.Left>0)
					m_SizeWindow=SIZE_HSPLIT;
				else if((m_LastDockSiteInfo.DockSide==DockStyle.Left || m_LastDockSiteInfo.DockSide==DockStyle.Right) && e.Y<=2 && this.Top>0)
					m_SizeWindow=SIZE_VSPLIT;
			}
			else if(e.Button==MouseButtons.Right && this.CanCustomize && !m_ItemContainer.DesignMode && m_BarState!=eBarState.Popup && !m_MoveWindow)
			{
				DotNetBarManager owner=m_Owner as DotNetBarManager;
				if(owner!=null)
				{
					IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
					if(ownersupport!=null)
						ownersupport.BarContextMenu(this,e);
				}
			}

            if (e.Button == MouseButtons.Left && m_BarState == eBarState.Docked && this.m_GrabHandleStyle == eGrabHandleStyle.ResizeHandle
                && (m_MouseDownPt.X > this.Location.X + this.Width - GrabHandleResizeWidth) && this.RightToLeft == RightToLeft.No)
            {
                Form formParent = this.FindForm();
                if (formParent != null && formParent.WindowState != FormWindowState.Maximized || formParent == null)
                {
                    // Start resizing parent window...
                    this.Capture = true;
                    this.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    m_SizeWindow = SIZE_PARENTRESIZE;
                    Point p = this.PointToScreen(m_MouseDownPt);
                    if (this.Parent != null && this.Parent.Parent != null)
                    {
                        p = this.Parent.Parent.PointToClient(p);
                        m_ResizeOffset = new Point(this.Parent.Parent.Width - p.X, this.Parent.Parent.ClientRectangle.Height - p.Y);
                    }
                    else if (m_Owner == null && !(this.Parent is DockSite) && !(this.Parent is FloatingContainer))
                    {
                        if (formParent != null)
                        {
                            p = formParent.PointToClient(p);
                            m_ResizeOffset = new Point(formParent.Width - p.X, formParent.ClientRectangle.Height - p.Y);
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Left && m_BarState == eBarState.Docked && this.m_GrabHandleStyle == eGrabHandleStyle.ResizeHandle
           && (m_MouseDownPt.X < this.Location.X + GrabHandleResizeWidth) && this.RightToLeft == RightToLeft.Yes)
            {
                Form formParent = this.FindForm();
                if (formParent != null && formParent.WindowState != FormWindowState.Maximized || formParent == null)
                {
                    // Start resizing parent window...
                    this.Capture = true;
                    this.Cursor = System.Windows.Forms.Cursors.SizeNESW;
                    m_SizeWindow = SIZE_PARENTRESIZE;
                    Point pointMouseDown = m_MouseDownPt;
                    if (formParent.GetType().GetProperty("RightToLeftLayout") != null)
                    {
                        if ((bool)TypeDescriptor.GetProperties(formParent)["RightToLeftLayout"].GetValue(formParent))
                        {
                            // Reverse it
                            pointMouseDown.X = this.Width - pointMouseDown.X;
                        }
                    }

                    Point p = this.PointToScreen(pointMouseDown);
                    if (this.Parent != null && this.Parent.Parent != null)
                    {
                        p = this.Parent.Parent.PointToClient(p);
                        m_ResizeOffset = new Point(p.X, this.Parent.Parent.ClientRectangle.Height - p.Y);
                    }
                    else if (m_Owner == null && !(this.Parent is DockSite) && !(this.Parent is FloatingContainer))
                    {
                        if (formParent != null)
                        {
                            p = formParent.PointToClient(p);
                            m_ResizeOffset = new Point(p.X, formParent.ClientRectangle.Height - p.Y);
                        }
                    }
                }
            }

			if(e.Button==MouseButtons.Left && !m_GrabHandleRect.IsEmpty && m_GrabHandleRect.Contains(m_MouseDownPt) && m_SizeWindow==0 && !m_AutoHideState && !(m_CustomizeMenu!=null && m_CustomizeMenu.Expanded))
			{
				this.Cursor=System.Windows.Forms.Cursors.SizeAll;
				this.Capture=true;
				m_MoveWindow=true;
			}

            if (m_BarState==eBarState.Popup && m_ParentItem!=null && m_ParentItem.DesignMode)
            {
                DesignTimeMouseDown(e);
            }
            else
			    m_ItemContainer.InternalMouseDown(e);

			base.OnMouseDown(e);
		}

		internal bool IsSizingWindow
		{
			get {return m_SizeWindow!=0;}
		}
		internal void InternalMouseUp(MouseEventArgs e)
		{
			this.OnMouseUp(e);
		}

		internal void CloseBar()
		{
			BarClosingEventArgs closingArgs=new BarClosingEventArgs();
			InvokeBarClosing(closingArgs);
			if(!closingArgs.Cancel)
			{
				if(this.AutoHide)
					this.AutoHide=false;
				this.HideBar();
				IOwner owner=this.Owner as IOwner;
				if(owner.ParentForm!=null && !owner.ParentForm.ContainsFocus)
					owner.ParentForm.Focus();
				InvokeUserVisibleChanged();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if(m_MoveWindow || m_SizeWindow!=0)
				this.Cursor=System.Windows.Forms.Cursors.Default;

            if (m_DragDropInProgress)
            {
                DesignTimeMouseUp(e);
            }

			if(m_MoveWindow)
			{
				EndDocking(false,new Point(e.X,e.Y));
			}
			else if(e.Button==MouseButtons.Left && !m_SystemButtons.CloseButtonRect.IsEmpty && m_SystemButtons.CloseButtonRect.Contains(m_MouseDownPt))
			{
				m_SystemButtons.MouseDownClose=false;
				PaintCloseButton();
				if(m_SystemButtons.CloseButtonRect.Contains(e.X,e.Y))
				{
                    if (m_CloseSingleTab && this.SelectedDockContainerItem != null)
                        CloseDockTab(this.SelectedDockContainerItem);
                    else
					    CloseBar();
				}
			}
			else if(e.Button==MouseButtons.Left && !m_SystemButtons.CaptionButtonRect.IsEmpty && m_SystemButtons.CaptionButtonRect.Contains(m_MouseDownPt))
			{
				m_SystemButtons.MouseDownCaption=false;
				PaintCaptionButton();
				if(m_SystemButtons.CaptionButtonRect.Contains(e.X,e.Y))
				{
					InvokeCaptionButtonClick();
					if(m_AutoCreateCaptionMenu)
					{
						ToggleCaptionMenu();
						PaintCaptionButton();
					}
				}
			}
			else if(e.Button==MouseButtons.Left && !m_SystemButtons.AutoHideButtonRect.IsEmpty && m_SystemButtons.AutoHideButtonRect.Contains(m_MouseDownPt))
			{
				m_SystemButtons.MouseDownAutoHide=false;
				PaintAutoHideButton();
				if(m_SystemButtons.AutoHideButtonRect.Contains(e.X,e.Y))
				{
					m_IgnoreAnimation=false;
					this.AutoHide=!this.AutoHide;
				}
			}
			m_MoveWindow=false;
			m_SizeWindow=0;
			
			if(m_ItemContainer!=null)
				m_ItemContainer.InternalMouseUp(e);

			base.OnMouseUp(e);
		}

		private void EndDocking(bool revertLast, Point mousePos)
		{
			if(this.Capture)
				this.Capture=false;

			this.Cursor=System.Windows.Forms.Cursors.Default;

			IOwnerBarSupport barsupport=this.Owner as IOwnerBarSupport;
			if(barsupport!=null)
				barsupport.DockComplete();

			if(m_DragDockInfo.IsEmpty())
			{
				m_MoveWindow=false;
				return;
			}
			
			if(m_TempTabBar!=null || m_DragDockInfo.TabDockContainer!=null)
			{
				DisposeDockingPreview();

				Bar targetBar=m_TempTabBar;
				if(targetBar==null)
					targetBar=m_DragDockInfo.TabDockContainer;

				RemoveTempTabBarItems();
				if(m_DragDockInfo.TabDockContainer!=this)
				{
					if(m_DockTabTearOffIndex==-1)
					{
						System.Collections.ArrayList list=new System.Collections.ArrayList(m_ItemContainer.SubItems.Count);
						m_ItemContainer.SubItems.CopyTo(list);
                        DockContainerItem firstItem = null;
                        Form f = targetBar.FindForm();
                        if (f != null) f.ActiveControl = targetBar;
						foreach(BaseItem item in list)
						{
							DockContainerItem dockitem=item as DockContainerItem;
							if(dockitem!=null)
							{
                                if (firstItem == null) firstItem = dockitem;
								dockitem.Displayed=false;
								if(dockitem.OriginalBarName=="")
								{
									dockitem.OriginalBarName=this.Name;
									dockitem.OriginalPosition=m_ItemContainer.SubItems.IndexOf(dockitem);
								}
								m_ItemContainer.SubItems.Remove(dockitem);
								targetBar.Items.Add(dockitem);
							}
						}
						targetBar.RecalcLayout();

                        if (firstItem != null)
                        {
                            targetBar.SelectedDockContainerItem = firstItem;
                            //if (f != null && firstItem.Control!=null) f.ActiveControl = firstItem.Control;
                        }
                        targetBar.InvokeBarDockEvents();

                        DotNetBarManager manager = m_Owner as DotNetBarManager;
                        if(manager!=null)
						{
                            m_MoveWindow = false;
                            targetBar = null;

                            if (this.CustomBar)
                            {
                                manager.Bars.Remove(this);
                                this.Dispose();
                            }
                            else
                            {
                                this.Visible = false;
                            }

                            if (manager.ParentForm != null)
                                manager.ParentForm.Activate();
							return;
						}
					}
					else
					{
                        Form f = targetBar.FindForm();
                        if (f != null) f.ActiveControl = targetBar;
						DockContainerItem dockitem=m_ItemContainer.SubItems[m_DockTabTearOffIndex] as DockContainerItem;
						dockitem.Displayed=false;
						m_ItemContainer.SubItems.Remove(dockitem);
						targetBar.Items.Add(dockitem);
						
                        targetBar.SelectedDockContainerItem = dockitem;
                        targetBar.RecalcLayout();
                        //if (f != null && dockitem.Control != null) f.ActiveControl = dockitem.Control;
                        targetBar.InvokeBarDockEvents();
					}
				}
				m_TempTabBar=null;
				m_DockTabTearOffIndex=-1;
				m_MoveWindow=false;
				m_DragDockInfo=new DockSiteInfo();
				return;
			}
			
			if(m_DockTabTearOffIndex!=-1)
			{
				DisposeDockingPreview();
				if(revertLast)
					DockingHandler(m_LastDockSiteInfo,this.PointToScreen(new Point(mousePos.X,mousePos.Y)));
				else
				{
                    DockContainerItem dc = (DockContainerItem)this.Items[m_DockTabTearOffIndex];
					Bar bar=TearOffDockContainerItem(dc,false);
					bar.DockingHandler(m_DragDockInfo,this.PointToScreen(new Point(mousePos.X,mousePos.Y)));
                    Form f = this.FindForm();
                    if (f != null && dc!=null && dc.Control!=null)
                    {
                        f.ActiveControl = dc.Control;
                    }
				}
				m_DragDockInfo=new DockSiteInfo();
				m_DockTabTearOffIndex=-1;
			}
			else
			{
				if(!m_OldOutlineRectangle.IsEmpty || m_OutlineForm!=null)
				{
					DisposeDockingPreview();
                    if (revertLast)
                    {
                        if (this.LayoutType == eLayoutType.Toolbar)
                            DockingHandler(m_LastDockSiteInfo, this.PointToScreen(new Point(mousePos.X, mousePos.Y)));
                    }
                    else
                        DockingHandler(m_DragDockInfo, this.PointToScreen(new Point(mousePos.X, mousePos.Y)));
					m_DragDockInfo=new DockSiteInfo();
				}
				else if(revertLast)
				{
					DockingHandler(m_LastDockSiteInfo,this.PointToScreen(new Point(mousePos.X,mousePos.Y)));
					m_DragDockInfo=new DockSiteInfo();
				}
			}

			m_MoveWindow=false;
		}

		private void DisposeDockingPreview()
		{
			if(!m_OldOutlineRectangle.IsEmpty || m_OutlineForm!=null)
			{
				if(!m_OldOutlineRectangle.IsEmpty)
				{
					NativeFunctions.DrawReversibleDesktopRect(m_OldOutlineRectangle,DRAGRECTANGLE_WIDTH);
					m_OldOutlineRectangle=Rectangle.Empty;
				}
				if(m_OutlineForm!=null)
				{
					m_OutlineForm.Visible=false;
					m_OutlineForm.Dispose();
					m_OutlineForm=null;
				}
			}
		}

		internal Bar StartTabDrag()
		{
			if(m_TabDockItems.SelectedTab==null)
				return null;
			DockContainerItem item=m_TabDockItems.SelectedTab.AttachedItem as DockContainerItem;
			if(item==null)
				return null;
			DotNetBarManager manager=m_Owner as DotNetBarManager;
			if(manager==null)
				return null;
			
			if(this.VisibleItemCount==1 && this.GrabHandleStyle==eGrabHandleStyle.None)
			{
				this.StartBarMove();
				return this;
			}
			else if(!this.CanUndock)
			{
				m_DockTabTearOffIndex=this.Items.IndexOf(item);
				this.StartBarMove();
				return this;
			}

			Bar bar=TearOffDockContainerItem(item,true);
			bar.InitalFloatLocation=Point.Empty;
			bar.StartBarMove();
			return bar;
		}

        internal Bar TearOffDockContainerItem(DockContainerItem item, bool floatBar)
        {
            return TearOffDockContainerItem(item, floatBar, new Point(Control.MousePosition.X-32,Control.MousePosition.Y-8));
        }
		internal Bar TearOffDockContainerItem(DockContainerItem item, bool floatBar, Point initialFloatLocation)
		{
            Form f = this.FindForm();
            if (f != null)
            {
                f.ActiveControl = this;
            }
			DotNetBarManager manager=m_Owner as DotNetBarManager;
			m_ItemContainer.SubItems.Remove(item);
			this.RecalcLayout();
			Bar bar=BarFunctions.CreateDuplicateDockBar(this);
			bar.Text=item.Text;
			
			bar.InitalFloatLocation=initialFloatLocation;
            
			if(manager.Bars.Contains(item.Name))
			{
				string name=item.Name;
				int i=0;
				while(manager.Bars.Contains(name+i.ToString()))
					i++;
				bar.Name=name+i.ToString();
			}
			else
				bar.Name=item.Name;
			
			bar.Items.Add(item);
			manager.Bars.Add(bar);
			
			if(floatBar)
			{
				bar.DockSide=eDockSide.None;
				bar.Location=initialFloatLocation;
			}

            bar.LastDockSide = this.DockSide;
            bar.m_LastDockSiteInfo.objDockSite = this.Parent as DockSite;
            bar.m_LastDockSiteInfo.LastRelativeDockToBar = this;
            bar.m_LastDockSiteInfo.LastDockSiteSide = this.DockSide;

			bar.CustomBar=true;

			IOwnerBarSupport ownerBar=m_Owner as IOwnerBarSupport;
			if(ownerBar!=null)
			{
				if(ownerBar.ApplyDocumentBarStyle && this.DockSide==eDockSide.Document)
					BarFunctions.RestoreAutoDocumentBarStyle(bar); //BarFunctions.ApplyAutoDocumentBarStyle(bar);
				ownerBar.InvokeBarTearOff(bar,new EventArgs());
			}

			return bar;
		}

		internal void StartBarMove()
		{
			m_MouseDownPt=this.PointToClient(Control.MousePosition);
			m_MouseDownSize=this.Size;
			this.Cursor=System.Windows.Forms.Cursors.SizeAll;
			m_MoveWindow=true;
		}

		internal void InvokeUserVisibleChanged()
		{
			if(UserVisibleChanged!=null)
				UserVisibleChanged(this,new EventArgs());
		}

		private void InvokeAutoHideChanged()
		{
			EventArgs e=new EventArgs();
			if(AutoHideChanged!=null)
				AutoHideChanged(this,e);
			IOwnerBarSupport barSupp=m_Owner as IOwnerBarSupport;
			if(barSupp!=null)
				barSupp.InvokeAutoHideChanged(this,e);
		}

        /// <summary>
        /// Closes the DockContainerItem with event source set to Code.
        /// </summary>
        /// <param name="dockTab">DockContainerItem to close.</param>
		public void CloseDockTab(DockContainerItem dockTab)
		{
			CloseDockTab(dockTab,eEventSource.Code);
		}

        /// <summary>
        /// Closes the DockContainerItem.
        /// </summary>
        /// <param name="dockTab">DockContainerItem to close.</param>
        /// <param name="source">Source of the event.</param>
		public void CloseDockTab(DockContainerItem dockTab, eEventSource source)
		{
			DockTabClosingEventArgs e=new DockTabClosingEventArgs(dockTab, source);
			InvokeDockTabClosing(e);
			if(e.Cancel)
				return;

			if(this.VisibleItemCount>1)
			{
				if(e.RemoveDockTab)
					this.Items.Remove(dockTab);
				else
					BarUtilities.SetDockContainerVisible(dockTab,false);
			}
			else
			{
				if(e.RemoveDockTab)
					this.Items.Remove(dockTab);
				CloseBar();
				if(!this.Visible)
					dockTab.Visible=false;
			}

            InvokeDockTabClosed(e);
		}

        private void InvokeDockTabClosed(DockTabClosingEventArgs e)
        {
            if (DockTabClosed != null)
                DockTabClosed(this, e);
            if (this.Owner is DotNetBarManager)
            {
                ((DotNetBarManager)this.Owner).InvokeDockTabClosed(this, e);
            }
        }

		internal void InvokeDockTabClosing(DockTabClosingEventArgs e)
		{
			if(DockTabClosing!=null)
				DockTabClosing(this,e);
			if(this.Owner is DotNetBarManager)
			{
				((DotNetBarManager)this.Owner).InvokeDockTabClosing(this,e);
			}
		}

		private void InvokeBarClosing(BarClosingEventArgs e)
		{
			if(Closing!=null)
				Closing(this,e);
			IOwnerBarSupport barSupp=m_Owner as IOwnerBarSupport;
			if(barSupp!=null)
				barSupp.InvokeBarClosing(this,e);
		}

        /// <summary>
        /// Raises the ItemClick event.
        /// </summary>
        /// <param name="item">Item that was clicked.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnItemClick(BaseItem item, EventArgs e)
        {
            if (ItemClick != null)
                ItemClick(item, e);
        }

        internal void InvokeItemClick(BaseItem item, EventArgs e)
        {
            OnItemClick(item, e);
        }

		protected override void OnClick(EventArgs e)
		{
			m_ItemContainer.InternalClick(Control.MouseButtons,this.PointToClient(Control.MousePosition));
			base.OnClick(e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
            if (m_BarState == eBarState.Popup)
            {
                ISite site = this.GetSite();
                if (site != null && site.DesignMode)
                {
                    ISelectionService selection = (ISelectionService)site.GetService(typeof(ISelectionService));
                    if (selection != null)
                    {
                        IDesignerHost host = (IDesignerHost)site.GetService(typeof(IDesignerHost));
                        if (host != null)
                        {
                            IDesigner designer = host.GetDesigner(selection.PrimarySelection as IComponent);
                            if (designer != null)
                            {
                                designer.DoDefaultAction();
                            }
                        }
                    }
                }
            }

			if(/*m_ItemContainer.LayoutType==eLayoutType.DockContainer && */m_GrabHandleRect.Contains(this.PointToClient(Control.MousePosition)))
			{
				if(m_BarState==eBarState.Floating)
				{
                    ReDock();
				}
				else if(m_BarState==eBarState.Docked && this.CanUndock && this.Parent is DockSite)
				{
					Point p=Point.Empty;
					if(m_FloatingRect.IsEmpty)
						p=Control.MousePosition;
					else
						p=m_FloatingRect.Location;
					DockingHandler(new DockSiteInfo(),p);
				}
			}
            if(!this.IsDisposed)
    			m_ItemContainer.InternalDoubleClick(Control.MouseButtons,Control.MousePosition);
			base.OnDoubleClick(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			ExKeyDown(e);
			base.OnKeyDown(e);
		}

		internal void ExKeyDown(KeyEventArgs e)
		{
			m_ItemContainer.InternalKeyDown(e);
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

		public override string ToString()
		{
			return this.Text;
		}

        private bool m_IsVisible = true;
        internal bool IsVisible
        {
            get
            {
                return m_IsVisible;
            }
        }

		protected override void OnVisibleChanged(EventArgs e)
		{
            if(this.IsHandleCreated)
                m_IsVisible = base.Visible;
			// Restore bar order when docked to the side. WinForms can change it's order which
			// creates problem for docking...
			if(m_BarShowIndex>=0)
			{
				if(this.Parent is DockSite && this.Parent.Controls.IndexOf(this)!=m_BarShowIndex)
				{
					this.Parent.Controls.SetChildIndex(this,m_BarShowIndex);
				}
				m_BarShowIndex=-1;
			}

			// Must reset the ActiveControl to null becouse on MDI Forms if this was not done
			// MDI form could not be closed if bar that had ActiveControl is floating.
			IOwner owner=m_Owner as IOwner;
			if(owner!=null)
			{
				if(owner.ParentForm!=null && owner.ParentForm.ActiveControl==this)
				{
					owner.ParentForm.ActiveControl=null;
					this.Focus(); // Fixes the problem on SDI forms
				}
				else if(owner.ParentForm!=null && IsAnyControl(this,owner.ParentForm.ActiveControl))
				{
					owner.ParentForm.ActiveControl=null;
					this.Focus();
				}
			}

			base.OnVisibleChanged(e);

			// m_DockingInProgress check is needed becouse we don't want RecalcLayout to be called from
			// DockSite.AddBar. The visible change event will fire when from that procedure Bar is added
			// to the Dock Site.
            if (m_BarState == eBarState.Docked && !m_DockingInProgress && this.Visible)
            {
                if (this.LayoutType == eLayoutType.DockContainer && m_AlwaysDisplayDockTab) RefreshDockTab(false);
                this.RecalcLayout();
            }

			if(!this.Visible && m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
			if(!this.Visible && m_CustomizeMenu!=null && m_CustomizeMenu.Expanded)
				m_CustomizeMenu.Expanded=false;

		}

        private bool m_LayoutSuspended = false;
        /// <summary>
        /// Suspends normal layout logic.
        /// </summary>
        public new void SuspendLayout()
        {
            m_LayoutSuspended = true;
            base.SuspendLayout();
        }

        /// <summary>
        /// Resumes normal layout logic.
        /// </summary>
        public new void ResumeLayout()
        {
            this.ResumeLayout(true);
        }

        /// <summary>
        /// Resumes normal layout logic. Optionally forces an immediate layout of pending layout requests.
        /// </summary>
        public new void ResumeLayout(bool performLayout)
        {
            m_LayoutSuspended = false;
            base.ResumeLayout(true);
        }

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if(this.Parent!=null && !(this.Parent is FloatingContainer) && !(this.Parent is DockSite))
			{
				m_ItemContainer.SetOwner(this);
				if(!m_ParentMsgHandlerRegistered && this.DesignMode && this.FindForm()!=null)
				{
					DotNetBarManager.RegisterOwnerParentMsgHandler(this,this.FindForm());
					m_ParentMsgHandlerRegistered=true;
				}

				// Cycle shortcuts
				if(m_ShortcutTable.Count==0)
				{
					foreach(BaseItem item in m_ItemContainer.SubItems)
						((IOwner)this).AddShortcutsFromItem(item);
				}

				if(this.Parent!=null && !(this.Parent is FloatingContainer) && !(this.Parent is DockSite) && !this.AutoHide)
				{
					if(!m_FilterInstalled)
					{
						MessageHandler.RegisterMessageClient(this);
						m_FilterInstalled=true;
					}
				}

                if (!m_DockStretch) Stretch = true;
			}
			else if(m_FilterInstalled)
			{
				MessageHandler.UnregisterMessageClient(this);
				m_FilterInstalled=false;
			}

			if(this.DesignMode)
				m_ItemContainer.SetDesignMode(this.DesignMode);
		}
		
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if(m_ThemeWindowMargins.IsEmpty)
			{
				RefreshThemeMargins();	
			}

			if(this.Parent!=null && !(this.Parent is FloatingContainer) && !(this.Parent is DockSite) && !this.AutoHide)
			{
				if(!m_FilterInstalled)
				{
					MessageHandler.RegisterMessageClient(this);
					m_FilterInstalled=true;
				}
			}

			this.RecalcSize();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeThemes();
			MenuEventSupportUnhook();
			if(m_FilterInstalled)
			{
				MessageHandler.UnregisterMessageClient(this);
				m_FilterInstalled=false;
			}
			base.OnHandleDestroyed(e);
		}

		private void RefreshThemeMargins()
		{
			if(!BarFunctions.ThemedOS)
				return;

			ThemeWindow theme=this.ThemeWindow;
			Graphics g=this.CreateGraphics();
			try
			{
				System.Drawing.Size sz=theme.ThemeMinSize(g,ThemeWindowParts.SmallFrameLeft,ThemeWindowStates.FrameActive);
				m_ThemeWindowMargins.Left=sz.Width;
				sz=theme.ThemeMinSize(g,ThemeWindowParts.SmallFrameRight,ThemeWindowStates.FrameActive);
				m_ThemeWindowMargins.Right=sz.Width;
				sz=theme.ThemeMinSize(g,ThemeWindowParts.SmallFrameBottom,ThemeWindowStates.FrameActive);
				m_ThemeWindowMargins.Bottom=sz.Height;
				sz=theme.ThemeTrueSize(g,ThemeWindowParts.SmallCaption,ThemeWindowStates.FrameActive);
				m_ThemeWindowMargins.Top=sz.Height;
			}
			finally
			{
				g.Dispose();
			}
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
			if(m_ThemeProgress!=null)
			{
				m_ThemeProgress.Dispose();
				m_ThemeProgress=null;
			}
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
		}
		internal DevComponents.DotNetBar.ThemeWindow ThemeWindow
		{
			get
			{
				if(m_ThemeWindow==null)
					m_ThemeWindow=new ThemeWindow(this);
				return m_ThemeWindow;
			}
		}
		internal DevComponents.DotNetBar.ThemeRebar ThemeRebar
		{
			get
			{
				if(m_ThemeRebar==null)
					m_ThemeRebar=new ThemeRebar(this);
				return m_ThemeRebar;
			}
		}
		internal DevComponents.DotNetBar.ThemeToolbar ThemeToolbar
		{
			get
			{
				if(m_ThemeToolbar==null)
					m_ThemeToolbar=new ThemeToolbar(this);
				return m_ThemeToolbar;
			}
		}
		internal DevComponents.DotNetBar.ThemeHeader ThemeHeader
		{
			get
			{
				if(m_ThemeHeader==null)
					m_ThemeHeader=new ThemeHeader(this);
				return m_ThemeHeader;
			}
		}
		internal DevComponents.DotNetBar.ThemeScrollBar ThemeScrollBar
		{
			get
			{
				if(m_ThemeScrollBar==null)
					m_ThemeScrollBar=new ThemeScrollBar(this);
				return m_ThemeScrollBar;
			}
		}
		internal DevComponents.DotNetBar.ThemeProgress ThemeProgress
		{
			get
			{
				if(m_ThemeProgress==null)
					m_ThemeProgress=new ThemeProgress(this);
				return m_ThemeProgress;
			}
		}

		private int ClientMarginLeft
		{
			get
			{
				int iMargin=0;

                if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
					iMargin=1;
				else
					iMargin=3;

				return iMargin;
			}
		}

		private int ClientMarginTop
		{
			get
			{
				int iMargin=0;
                if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
					iMargin=2;
				else
					iMargin=3;

				return iMargin;
			}
		}

		private int ClientMarginRight
		{
			get
			{
				bool bShowShadow=true;
				int iMargin=0;
				IOwnerMenuSupport ownersupport=m_Owner as IOwnerMenuSupport;
				if(ownersupport!=null && !ownersupport.ShowPopupShadow)
					bShowShadow=false;
                if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
				{
					if(this.AlphaShadow || !bShowShadow)
						iMargin=1;
					else
						iMargin=3;
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
				bool bShowShadow=true;
				int iMargin=0;
				IOwnerMenuSupport ownersupport=m_Owner as IOwnerMenuSupport;
				if(ownersupport!=null && !ownersupport.ShowPopupShadow)
					bShowShadow=false;
                if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
				{
					if(this.AlphaShadow || !bShowShadow)
						iMargin=2;
					else
						iMargin=4;
				}
				else
					iMargin=3;

				return iMargin;
			}
		}

        /// <summary>
        /// Returns whether popup bar should display shadow.
        /// </summary>
		internal bool DisplayShadow
		{
			get
			{
				if(PassiveBar)
					return false;
				IOwnerMenuSupport ownersupport=m_Owner as IOwnerMenuSupport;
				if(ownersupport!=null)
				{
                    if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
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
                    if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
						return false;
				}

				return true;
			}
		}

        /// <summary>
        /// Returns whether popup bar shadow should be alpha-blended.
        /// </summary>
		internal bool AlphaShadow
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

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if(this.MenuBar)
				this.Refresh();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if(this.MenuBar)
				this.Refresh();
		}

        internal bool MenuFocus
		{
			get
			{
				return m_MenuFocus;
			}
			set
			{
				if(m_MenuFocus!=value)
				{
					m_MenuFocus=value;
					if(m_MenuFocus)
					{
						m_ItemContainer.SetSystemFocus();
						DotNetBarManager manager=m_Owner as DotNetBarManager;
						if(manager!=null)
							manager.FocusedBar=this;
					}
					else
					{
						if(m_ItemContainer!=null)
						{
							m_ItemContainer.AutoExpand=false;
							m_ItemContainer.ReleaseSystemFocus();
							m_ItemContainer.ContainerLostFocus(false);
						}

						DotNetBarManager manager=m_Owner as DotNetBarManager;
						if(manager!=null)
							manager.FocusedBar=null;
					}
					if(this.MenuBar)
					{
						this.Refresh();
					}
				}
			}
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void SetDesignMode(bool b)
		{
			m_ItemContainer.SetDesignMode(b);
		}

        internal bool GetDesignMode()
        {
            return this.DesignMode;
        }

		internal int PopupWidth
		{
			get
			{
				return m_InitialContainerWidth;
			}
			set
			{
				m_InitialContainerWidth=value;
			}
		}

		/// <summary>
		/// Gets/Sets the popup animation that will be applied when popup is shown.
		/// </summary>
		[System.ComponentModel.Browsable(false),DefaultValue(ePopupAnimation.ManagerControlled)]
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

        #region ICustomSerialization Implementation
        /// <summary>
        /// Invokes SerializeItem event.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        void ICustomSerialization.InvokeSerializeItem(SerializeItemEventArgs e)
        {
            if (SerializeItem != null)
                SerializeItem(this, e);
            if (this.Owner!=this && this.Owner is ICustomSerialization)
                ((ICustomSerialization)this.Owner).InvokeSerializeItem(e);
        }

        /// <summary>
        /// Invokes DeserializeItem event.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        void ICustomSerialization.InvokeDeserializeItem(SerializeItemEventArgs e)
        {
            if (DeserializeItem != null)
                DeserializeItem(this, e);
            if (this.Owner!=this && this.Owner is ICustomSerialization)
                ((ICustomSerialization)this.Owner).InvokeDeserializeItem(e);
        }

        /// <summary>
        /// Gets whether any handlers have been defined for SerializeItem event. If no handles have been defined to optimize performance SerializeItem event will not be attempted to fire.
        /// </summary>
        bool ICustomSerialization.HasSerializeItemHandlers
        {
            get
            {
                bool b = SerializeItem != null;
                if (this.Owner!=this && this.Owner is ICustomSerialization)
                    b |= ((ICustomSerialization)this.Owner).HasSerializeItemHandlers;
                return b;
            }
        }

        /// <summary>
        /// Gets whether any handlers have been defined for DeserializeItem event. If no handles have been defined to optimize performance DeserializeItem event will not be attempted to fire.
        /// </summary>
        bool ICustomSerialization.HasDeserializeItemHandlers
        {
            get
            {
                bool b = DeserializeItem != null;
                if (this.Owner!=this && this.Owner is ICustomSerialization)
                    b |= ((ICustomSerialization)this.Owner).HasDeserializeItemHandlers;
                return b;
            }
        }
		#endregion

		/// <summary>
		/// Saves the Bar definition to file.
		/// </summary>
		/// <param name="FileName">Definition file name.</param>
		public void SaveDefinition(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			
			this.SaveDefinition(xmlDoc);

			xmlDoc.Save(FileName);
		}

		internal void SaveDefinition(System.Xml.XmlDocument xmlDoc)
		{
			System.Xml.XmlElement xmlBar=xmlDoc.CreateElement("bar");
			xmlDoc.AppendChild(xmlBar);
			this.Serialize(xmlBar);
		}

		/// <summary>
		/// Loads the Bar definition from file.
		/// </summary>
		/// <param name="FileName">Definition file name.</param>
		public void LoadDefinition(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			xmlDoc.Load(FileName);
			this.LoadDefinition(xmlDoc);			
		}

		internal void LoadDefinition(System.Xml.XmlDocument xmlDoc)
		{
			if(xmlDoc.FirstChild.Name!="bar")
				throw(new System.InvalidOperationException("XML Format not recognized"));

			m_ItemContainer.SubItems.Clear();
			this.Deserialize(xmlDoc.FirstChild as System.Xml.XmlElement);

			IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
			if(ownersupport!=null)
				ownersupport.AddShortcutsFromBar(this);

			((IOwner)this).InvokeDefinitionLoaded(this,new EventArgs());
		}

		/// <summary>
		/// Gets/Sets Bar definition as XML string.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string Definition
		{
			get
			{
				System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
				SaveDefinition(xmlDoc);
				return xmlDoc.OuterXml;
			}
			set
			{
				System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
				xmlDoc.LoadXml(value);
				LoadDefinition(xmlDoc);
			}
		}

		void IOwner.InvokeDefinitionLoaded(object sender,EventArgs e)
		{
			if(DefinitionLoaded!=null)
				DefinitionLoaded(sender,e);
		}

		internal void Serialize(System.Xml.XmlElement xmlThisBar)
		{
			Serialize(xmlThisBar,false);
		}

		internal void Serialize(System.Xml.XmlElement xmlThisBar, bool bPropertiesOnly)
		{
            // Creates serialization context
            ItemSerializationContext context = new ItemSerializationContext();
            context.Serializer = this;
            context.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
            context.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;

			xmlThisBar.SetAttribute("name",this.Name);
			xmlThisBar.SetAttribute("candockleft",System.Xml.XmlConvert.ToString(m_CanDockLeft));
			xmlThisBar.SetAttribute("candockright",System.Xml.XmlConvert.ToString(m_CanDockRight));
			xmlThisBar.SetAttribute("candocktop",System.Xml.XmlConvert.ToString(m_CanDockTop));
			xmlThisBar.SetAttribute("candockbottom",System.Xml.XmlConvert.ToString(m_CanDockBottom));
			xmlThisBar.SetAttribute("candockdoc",System.Xml.XmlConvert.ToString(m_CanDockDocument));
			xmlThisBar.SetAttribute("candocktab",System.Xml.XmlConvert.ToString(m_CanDockTab));
			xmlThisBar.SetAttribute("text",this.Text);
			xmlThisBar.SetAttribute("dockline",System.Xml.XmlConvert.ToString(m_DockLine));
			if(/*m_DockOffset==0 && this.Left>0 && */ (this.DockSide==eDockSide.Top || this.DockSide==eDockSide.Bottom))
				xmlThisBar.SetAttribute("dockoffset",System.Xml.XmlConvert.ToString(this.Left));
			else if(/*m_DockOffset==0 && this.Top>0 &&*/ (this.DockSide==eDockSide.Left || this.DockSide==eDockSide.Right))
				xmlThisBar.SetAttribute("dockoffset",System.Xml.XmlConvert.ToString(this.Top));
            //else
            //    xmlThisBar.SetAttribute("dockoffset",System.Xml.XmlConvert.ToString(m_DockOffset));
			xmlThisBar.SetAttribute("grabhandle",System.Xml.XmlConvert.ToString((int)m_GrabHandleStyle));
			xmlThisBar.SetAttribute("menubar",System.Xml.XmlConvert.ToString(m_MenuBar));
			xmlThisBar.SetAttribute("stretch",System.Xml.XmlConvert.ToString(m_DockStretch));
			xmlThisBar.SetAttribute("style",System.Xml.XmlConvert.ToString((int)m_ItemContainer.Style));
			xmlThisBar.SetAttribute("wrapdock",System.Xml.XmlConvert.ToString(m_WrapItemsDock));
			xmlThisBar.SetAttribute("wrapfloat",System.Xml.XmlConvert.ToString(m_WrapItemsFloat));
			if(m_LockDockPosition)
				xmlThisBar.SetAttribute("lockdockpos",System.Xml.XmlConvert.ToString(m_LockDockPosition));
			if(!m_CanUndock)
				xmlThisBar.SetAttribute("canundock",System.Xml.XmlConvert.ToString(m_CanUndock));
			if(m_TabNavigation)
				xmlThisBar.SetAttribute("tabnav",System.Xml.XmlConvert.ToString(m_TabNavigation));

			if(!m_ShowToolTips)
				xmlThisBar.SetAttribute("tooltips",System.Xml.XmlConvert.ToString(m_ShowToolTips));

			if(m_ItemContainer.MoreItemsOnMenu)
				xmlThisBar.SetAttribute("overflowmenu",System.Xml.XmlConvert.ToString(m_ItemContainer.MoreItemsOnMenu));

			if(m_AutoHideState)
			{
				xmlThisBar.SetAttribute("state",System.Xml.XmlConvert.ToString((int)eBarState.Docked));
				eDockSide dockside=eDockSide.None;
				switch(m_LastDockSiteInfo.DockSide)
				{
					case DockStyle.Left:
						dockside=eDockSide.Left;
						break;
					case DockStyle.Right:
						dockside=eDockSide.Right;
						break;
					case DockStyle.Top:
						dockside=eDockSide.Top;
						break;
					case DockStyle.Bottom:
						dockside=eDockSide.Bottom;
						break;
				}
				xmlThisBar.SetAttribute("dockside",System.Xml.XmlConvert.ToString((int)dockside));
			}
			else
			{
				xmlThisBar.SetAttribute("state",System.Xml.XmlConvert.ToString((int)m_BarState));
				xmlThisBar.SetAttribute("dockside",System.Xml.XmlConvert.ToString((int)this.DockSide));
				if(this.DockSide==eDockSide.None && m_Float!=null)
					xmlThisBar.SetAttribute("fpos",m_Float.Location.X+","+m_Float.Location.Y+","+this.DisplayRectangle.Width+","+this.DisplayRectangle.Height);
			}

			IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
			if(m_BarState==eBarState.Floating && !this.Visible && ownersupport!=null && ownersupport.WereVisible.Count>0 && ownersupport.WereVisible.Contains(this))
				xmlThisBar.SetAttribute("visible",System.Xml.XmlConvert.ToString(true));
			else
			{
				if(m_AutoHideState)
				{
					xmlThisBar.SetAttribute("visible",System.Xml.XmlConvert.ToString(true));
					xmlThisBar.SetAttribute("autohide",System.Xml.XmlConvert.ToString(true));
				}
				else
					xmlThisBar.SetAttribute("visible",System.Xml.XmlConvert.ToString(this.Visible));
			}
			xmlThisBar.SetAttribute("custom",System.Xml.XmlConvert.ToString(m_CustomBar));
			xmlThisBar.SetAttribute("canhide",System.Xml.XmlConvert.ToString(m_CanHide));
			xmlThisBar.SetAttribute("imagesize",System.Xml.XmlConvert.ToString((int)m_ImageSize));
			xmlThisBar.SetAttribute("itemsp",System.Xml.XmlConvert.ToString(m_ItemContainer.ItemSpacing));

			xmlThisBar.SetAttribute("themes",System.Xml.XmlConvert.ToString(m_ThemeAware));

			if(!m_ItemContainer.CanCustomize)
				xmlThisBar.SetAttribute("cancust",System.Xml.XmlConvert.ToString(m_ItemContainer.CanCustomize));

			// Save Font information if needed
			if(this.Font!=null)
			{
				if(m_CustomFont)
				{
					xmlThisBar.SetAttribute("fontname",this.Font.Name);
					xmlThisBar.SetAttribute("fontemsize",System.Xml.XmlConvert.ToString(this.Font.Size));
					xmlThisBar.SetAttribute("fontstyle",System.Xml.XmlConvert.ToString((int)this.Font.Style));
				}
			}

			if(!m_ItemContainer.m_BackgroundColor.IsEmpty)
				xmlThisBar.SetAttribute("backcolor",BarFunctions.ColorToString(m_ItemContainer.BackColor));
			xmlThisBar.SetAttribute("layout",System.Xml.XmlConvert.ToString((int)m_ItemContainer.LayoutType));
			xmlThisBar.SetAttribute("eqbutton",System.Xml.XmlConvert.ToString(m_ItemContainer.EqualButtonSize));
			if(m_DockedBorder!=eBorderType.None)
				xmlThisBar.SetAttribute("dborder",System.Xml.XmlConvert.ToString((int)m_DockedBorder));
			
			if(!m_AcceptDropItems)
				xmlThisBar.SetAttribute("acceptdrop",System.Xml.XmlConvert.ToString(m_AcceptDropItems));

			if(m_SingleLineColor!=SystemColors.ControlDark)
				xmlThisBar.SetAttribute("slcolor",BarFunctions.ColorToString(m_SingleLineColor));

			if(!m_CaptionBackColor.IsEmpty)
				xmlThisBar.SetAttribute("captionbc",BarFunctions.ColorToString(m_CaptionBackColor));
			if(!m_CaptionForeColor.IsEmpty)
				xmlThisBar.SetAttribute("captionfc",BarFunctions.ColorToString(m_CaptionForeColor));

			if(m_AlwaysDisplayDockTab)
				xmlThisBar.SetAttribute("showtab",System.Xml.XmlConvert.ToString(m_AlwaysDisplayDockTab));
			
			if(this.AutoHide)
			{
				if(m_ItemContainer.MinWidth!=0)
					xmlThisBar.SetAttribute("dockwidth",System.Xml.XmlConvert.ToString(m_ItemContainer.MinWidth));
				else if(m_LastDockSiteInfo.DockedWidth!=0)
                    xmlThisBar.SetAttribute("dockwidth",System.Xml.XmlConvert.ToString(m_LastDockSiteInfo.DockedWidth));
				if(m_ItemContainer.MinHeight!=0)
					xmlThisBar.SetAttribute("dockheight",System.Xml.XmlConvert.ToString(m_ItemContainer.MinHeight));
				else if(m_LastDockSiteInfo.DockedHeight!=0)
					xmlThisBar.SetAttribute("dockheight",System.Xml.XmlConvert.ToString(m_LastDockSiteInfo.DockedHeight));				
			}
			else
			{
				if(m_ItemContainer.MinWidth!=0 && this.CanSaveMinWidth)
					xmlThisBar.SetAttribute("dockwidth",System.Xml.XmlConvert.ToString(m_ItemContainer.MinWidth));
				if(m_ItemContainer.MinHeight!=0 && this.CanSaveMinHeight)
					xmlThisBar.SetAttribute("dockheight",System.Xml.XmlConvert.ToString(m_ItemContainer.MinHeight));
			}

			if(m_SplitDockWidthPercent>0)
				xmlThisBar.SetAttribute("splitwidthpercent",System.Xml.XmlConvert.ToString(m_SplitDockWidthPercent));
			if(m_SplitDockHeightPercent>0)
				xmlThisBar.SetAttribute("splitheightpercent",System.Xml.XmlConvert.ToString(m_SplitDockHeightPercent));


			if(m_ItemContainer.PaddingBottom!=1)
				xmlThisBar.SetAttribute("padbottom",System.Xml.XmlConvert.ToString(m_ItemContainer.PaddingBottom));
			if(m_ItemContainer.PaddingLeft!=1)
				xmlThisBar.SetAttribute("padleft",System.Xml.XmlConvert.ToString(m_ItemContainer.PaddingLeft));
			if(m_ItemContainer.PaddingRight!=1)
				xmlThisBar.SetAttribute("padright",System.Xml.XmlConvert.ToString(m_ItemContainer.PaddingRight));
			if(m_ItemContainer.PaddingTop!=1)
				xmlThisBar.SetAttribute("padtop",System.Xml.XmlConvert.ToString(m_ItemContainer.PaddingTop));

			if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && this.SelectedDockTab>=0)
				xmlThisBar.SetAttribute("seldocktab",System.Xml.XmlConvert.ToString(this.SelectedDockTab));

			if(!m_CanAutoHide)
				xmlThisBar.SetAttribute("canautohide",System.Xml.XmlConvert.ToString(m_CanAutoHide));
			if(!m_CanReorderTabs)
				xmlThisBar.SetAttribute("canreordertabs",System.Xml.XmlConvert.ToString(m_CanReorderTabs));
			if(!m_CanTearOffTabs)
				xmlThisBar.SetAttribute("cantearoff",System.Xml.XmlConvert.ToString(m_CanTearOffTabs));
			
			// TODO: Menu Merge Implementation
			//if(m_MergeEnabled)
			//	xmlThisBar.SetAttribute("merge",System.Xml.XmlConvert.ToString(m_MergeEnabled));

			if(!m_HideFloatingInactive)
				xmlThisBar.SetAttribute("hidein",System.Xml.XmlConvert.ToString(m_HideFloatingInactive));

			if(m_DockTabAlignment!=eTabStripAlignment.Bottom)
				xmlThisBar.SetAttribute("tabalign",System.Xml.XmlConvert.ToString((int)m_DockTabAlignment));

			if(m_AutoHideAnimationTime!=100)
				xmlThisBar.SetAttribute("ahanim",System.Xml.XmlConvert.ToString(m_AutoHideAnimationTime));
			
			if(!m_AutoCreateCaptionMenu)
				xmlThisBar.SetAttribute("autocaptionmenu",System.Xml.XmlConvert.ToString(m_AutoCreateCaptionMenu));

			if(m_AutoSyncBarCaption)
				xmlThisBar.SetAttribute("autocaptionsync",System.Xml.XmlConvert.ToString(m_AutoSyncBarCaption));

			if(!m_SaveLayoutChanges)
                xmlThisBar.SetAttribute("savelayout",System.Xml.XmlConvert.ToString(m_SaveLayoutChanges));

			if(m_ColorScheme.SchemeChanged)
			{
				System.Xml.XmlElement xmlScheme=xmlThisBar.OwnerDocument.CreateElement("colorscheme");
				m_ColorScheme.Serialize(xmlScheme);
				xmlThisBar.AppendChild(xmlScheme);
			}

			if(this.BackgroundImage!=null)
			{
				System.Xml.XmlElement elementImage=xmlThisBar.OwnerDocument.CreateElement("backimage");
				xmlThisBar.AppendChild(elementImage);
				BarFunctions.SerializeImage(this.BackgroundImage,elementImage);
				elementImage.SetAttribute("pos",((int)m_BackgroundImagePosition).ToString());
				elementImage.SetAttribute("alpha",m_BackgroundImageAlpha.ToString());
			}

			if(!bPropertiesOnly)
			{
				System.Xml.XmlElement xmlItems=xmlThisBar.OwnerDocument.CreateElement("items");
				xmlThisBar.AppendChild(xmlItems);
				foreach(BaseItem objItem in m_ItemContainer.SubItems)
				{
					if(objItem.ShouldSerialize)
					{
						System.Xml.XmlElement xmlItem=xmlThisBar.OwnerDocument.CreateElement("item");
						xmlItems.AppendChild(xmlItem);
                        context.ItemXmlElement = xmlItem;
						objItem.Serialize(context);
					}
				}
			}
		}

		private bool CanSaveMinWidth
		{
			get 
			{
				eDockSide dockSide=this.DockSide;
				if(dockSide==eDockSide.None || dockSide==eDockSide.Left || dockSide==eDockSide.Right)
					return true;
				return false;
			}
		}

		private bool CanSaveMinHeight
		{
			get 
			{
				eDockSide dockSide=this.DockSide;
				if(dockSide==eDockSide.None || dockSide==eDockSide.Top || dockSide==eDockSide.Bottom)
					return true;
				return false;
			}
		}

        internal void Deserialize(System.Xml.XmlElement xmlThisBar)
        {
            // Creates serialization context
            ItemSerializationContext context = new ItemSerializationContext();
            context.Serializer = this;
            context.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
            context.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;

            Deserialize(xmlThisBar, context);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
		public void Deserialize(System.Xml.XmlElement xmlThisBar, ItemSerializationContext context)
		{
			m_BarDefinitionLoading=true;
			m_IgnoreAnimation=true;
			try
			{
				LoadCommonProperties(xmlThisBar);

				this.BackgroundImage=null;

				foreach(System.Xml.XmlElement xmlElem in xmlThisBar.ChildNodes)
				{
					switch(xmlElem.Name)
					{
						case "items":
						{
							foreach(System.Xml.XmlElement xmlItem in xmlElem.ChildNodes)
							{
								BaseItem objItem=context.CreateItemFromXml(xmlItem);
								m_ItemContainer.SubItems.Add(objItem);
                                context.ItemXmlElement = xmlItem;
								objItem.Deserialize(context);
							}
							break;
						}
						case "colorscheme":
						{
							m_ColorScheme.Deserialize(xmlElem);
							break;
						}
						case "backimage":
						{
							this.BackgroundImage=BarFunctions.DeserializeImage(xmlElem);
							m_BackgroundImagePosition=(eBackgroundImagePosition)System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("pos"));
							m_BackgroundImageAlpha=System.Xml.XmlConvert.ToByte(xmlElem.GetAttribute("alpha"));
							break;
						}
					}
				}
				
				if((eDockSide)System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockside"))==eDockSide.None)
				{
					// Try to load position
					if(xmlThisBar.HasAttribute("fpos"))
					{
						string s=xmlThisBar.GetAttribute("fpos");
						string[] arr=s.Split(',');
						if(arr.Length==4)
						{
							Rectangle r=new Rectangle(System.Xml.XmlConvert.ToInt32(arr[0]),System.Xml.XmlConvert.ToInt32(arr[1]),System.Xml.XmlConvert.ToInt32(arr[2]),System.Xml.XmlConvert.ToInt32(arr[3]));
							m_FloatingRect=r;
						}
					}
					if(!System.Xml.XmlConvert.ToBoolean(xmlThisBar.GetAttribute("visible")) || xmlThisBar.HasAttribute("autohide") && System.Xml.XmlConvert.ToBoolean(xmlThisBar.GetAttribute("autohide")))
						m_LoadingHideFloating=true;
				}

				if(xmlThisBar.HasAttribute("seldocktab"))
				{
					int iTab=System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("seldocktab"));
					if(iTab>=0 && iTab<m_ItemContainer.SubItems.Count)
					{
						foreach(BaseItem dockItem in m_ItemContainer.SubItems)
							dockItem.Displayed=false;
						m_ItemContainer.SubItems[iTab].Displayed=true;
					}
				}

				// Last Thing to do so it is docked properly
				m_DockLine=System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockline"));
				if(m_ItemContainer.LayoutType==eLayoutType.DockContainer)
					RefreshDockTab(true);
				this.DockSide=(eDockSide)System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockside"));
				if(m_LoadingHideFloating) m_LoadingHideFloating=false;
                if (xmlThisBar.HasAttribute("autohide") && context._DesignerHost == null)
                {
                    base.Visible = false;
                    if (m_ItemContainer.MinWidth > 0)
                        this.Width = m_ItemContainer.MinWidth;
                    if (m_ItemContainer.MinHeight > 0)
                        this.Height = m_ItemContainer.MinHeight;
                    this.AutoHide = true;
                    m_LastDockSiteInfo.DockedWidth = m_ItemContainer.MinWidth;
                    m_LastDockSiteInfo.DockedHeight = m_ItemContainer.MinHeight;
                }
                else
                {
                    if (context._DesignerHost == null)
                        this.Visible = System.Xml.XmlConvert.ToBoolean(xmlThisBar.GetAttribute("visible"));
                    else
                        this.Visible = true;
                }
			}
			finally
			{
				m_BarDefinitionLoading=false;
			}
			m_TabsRearranged=false;
			SetupAccessibility();
		}

		private void LoadCommonProperties(System.Xml.XmlElement xmlBar)
		{
			m_BarState=eBarState.Docked;
			this.Name=xmlBar.GetAttribute("name");
			if(xmlBar.HasAttribute("candockleft"))
				m_CanDockLeft=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("candockleft"));
			if(xmlBar.HasAttribute("candockright"))
				m_CanDockRight=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("candockright"));
			if(xmlBar.HasAttribute("candocktop"))
				m_CanDockTop=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("candocktop"));
			if(xmlBar.HasAttribute("candockbottom"))
				m_CanDockBottom=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("candockbottom"));
			if(xmlBar.HasAttribute("candockdoc"))
				m_CanDockDocument=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("candockdoc"));
			if(xmlBar.HasAttribute("candocktab"))
				m_CanDockTab=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("candocktab"));
			if(xmlBar.HasAttribute("text"))
				this.Text=xmlBar.GetAttribute("text");
			if(xmlBar.HasAttribute("dockline"))
				m_DockLine=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dockline"));
			if(xmlBar.HasAttribute("dockoffset"))
				m_DockOffset=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dockoffset"));
			if(xmlBar.HasAttribute("grabhandle"))
				m_GrabHandleStyle=(eGrabHandleStyle)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("grabhandle"));
			if(xmlBar.HasAttribute("menubar"))
				m_MenuBar=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("menubar"));
			if(xmlBar.HasAttribute("stretch"))
				m_DockStretch=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("stretch"));
			if(xmlBar.HasAttribute("wrapdock"))
				m_WrapItemsDock=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("wrapdock"));
			if(xmlBar.HasAttribute("wrapfloat"))
				m_WrapItemsFloat=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("wrapfloat"));
			if(xmlBar.HasAttribute("custom"))
				m_CustomBar=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("custom"));
			if(xmlBar.HasAttribute("canhide"))
				m_CanHide=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("canhide"));
			if(xmlBar.HasAttribute("imagesize"))
				m_ImageSize=(eBarImageSize)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("imagesize"));

			if(xmlBar.HasAttribute("itemsp"))
				m_ItemContainer.ItemSpacing=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("itemsp"));
//			else
//				m_ItemContainer.ItemSpacing=0;

			if(xmlBar.HasAttribute("backcolor"))
				m_ItemContainer.BackColor=BarFunctions.ColorFromString(xmlBar.GetAttribute("backcolor"));

			if(xmlBar.HasAttribute("layout"))
				m_ItemContainer.LayoutType=(eLayoutType)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("layout"));

			if(xmlBar.HasAttribute("eqbutton"))
				m_ItemContainer.EqualButtonSize=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("eqbutton"));

			if(xmlBar.HasAttribute("dborder"))
				m_DockedBorder=(eBorderType)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dborder"));
            //else
            //    m_DockedBorder=eBorderType.None;

			if(xmlBar.HasAttribute("acceptdrop"))
				m_AcceptDropItems=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("acceptdrop"));

			if(xmlBar.HasAttribute("slcolor"))
				m_SingleLineColor=BarFunctions.ColorFromString(xmlBar.GetAttribute("slcolor"));

			if(xmlBar.HasAttribute("captionbc"))
				m_CaptionBackColor=BarFunctions.ColorFromString(xmlBar.GetAttribute("captionbc"));
			if(xmlBar.HasAttribute("captionfc"))
				m_CaptionForeColor=BarFunctions.ColorFromString(xmlBar.GetAttribute("captionfc"));

			if(xmlBar.HasAttribute("dockwidth"))
				m_ItemContainer.MinWidth=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dockwidth"));
			if(xmlBar.HasAttribute("dockheight"))
				m_ItemContainer.MinHeight=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dockheight"));

			if(xmlBar.HasAttribute("splitwidthpercent"))
				m_SplitDockWidthPercent=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("splitwidthpercent"));
			else
				m_SplitDockWidthPercent=0;
			if(xmlBar.HasAttribute("splitheightpercent"))
				m_SplitDockHeightPercent=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("splitheightpercent"));
//			else
//				m_SplitDockHeightPercent=0;

			if(xmlBar.HasAttribute("padbottom"))
				m_ItemContainer.PaddingBottom=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("padbottom"));
			if(xmlBar.HasAttribute("padleft"))
				m_ItemContainer.PaddingLeft=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("padleft"));
			if(xmlBar.HasAttribute("padright"))
				m_ItemContainer.PaddingRight=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("padright"));
			if(xmlBar.HasAttribute("padtop"))
				m_ItemContainer.PaddingTop=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("padtop"));

			if(xmlBar.HasAttribute("lockdockpos"))
				m_LockDockPosition=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("lockdockpos"));
			if(xmlBar.HasAttribute("canundock"))
				m_CanUndock=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("canundock"));
			if(xmlBar.HasAttribute("canreordertabs"))
				m_CanReorderTabs=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("canreordertabs"));
			if(xmlBar.HasAttribute("cantearoff"))
				m_CanTearOffTabs=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("cantearoff"));

			if(xmlBar.HasAttribute("canautohide"))
				m_CanAutoHide=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("canautohide"));

			if(xmlBar.HasAttribute("cancust"))
				m_ItemContainer.CanCustomize=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("cancust"));

			if(xmlBar.HasAttribute("tabalign"))
				m_DockTabAlignment=(eTabStripAlignment)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("tabalign"));

			if(xmlBar.HasAttribute("showtab"))
				m_AlwaysDisplayDockTab=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("showtab"));

			// TODO: Menu Merge Implementation
			//if(xmlBar.HasAttribute("merge"))
			//	m_MergeEnabled=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("merge"));

			if(xmlBar.HasAttribute("hidein"))
				m_HideFloatingInactive=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("hidein"));

			if(xmlBar.HasAttribute("themes"))
			{
				m_ThemeAware=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("themes"));
				m_ItemContainer.ThemeAware=m_ThemeAware;
			}

			if(xmlBar.HasAttribute("tabnav"))
				m_TabNavigation=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("tabnav"));

			if(xmlBar.HasAttribute("tooltips"))
				m_ShowToolTips=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("tooltips"));
//			else
//				m_ShowToolTips=true;

			if(xmlBar.HasAttribute("overflowmenu"))
				m_ItemContainer.MoreItemsOnMenu=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("overflowmenu"));
//			else
//				m_ItemContainer.MoreItemsOnMenu=false;

			if(xmlBar.HasAttribute("autocaptionmenu"))
				m_AutoCreateCaptionMenu=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("autocaptionmenu"));
//			else
//				m_AutoCreateCaptionMenu=true;

			if(xmlBar.HasAttribute("autocaptionsync"))
				this.AutoSyncBarCaption=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("autocaptionsync"));
//			else
//				this.AutoSyncBarCaption=false;

            if (xmlBar.HasAttribute("style"))
            {
                string sty = xmlBar.GetAttribute("style");
                //if (sty == "5")
                //    this.Style = eDotNetBarStyle.Office2007;
                //else
                    this.Style = (eDotNetBarStyle)System.Xml.XmlConvert.ToInt32(sty);
            }

			// Load font information if it exists
			if(xmlBar.HasAttribute("fontname"))
			{
				string FontName=xmlBar.GetAttribute("fontname");
				float FontSize=System.Xml.XmlConvert.ToSingle(xmlBar.GetAttribute("fontemsize"));
				System.Drawing.FontStyle FontStyle=(System.Drawing.FontStyle)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("fontstyle"));
				try
				{
					this.Font=new Font(FontName,FontSize,FontStyle);
				}
				catch(Exception)
				{
					this.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
				}
				m_CustomFont=true;
			}

			if(xmlBar.HasAttribute("ahanim"))
				m_AutoHideAnimationTime=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("ahanim"));
//			else
//				m_AutoHideAnimationTime=100;

			if(xmlBar.HasAttribute("savelayout"))
				m_SaveLayoutChanges=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("savelayout"));
//			else
//				m_SaveLayoutChanges=true;
		}

		/// <summary>
		/// Gets or sets whether layout changes are saved for this bar when DotNetBarManager.SaveLayout method is used to save layout for all bars. Default value is true which means that layout changes are saved.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether layout changes are saved for this bar")]
		public bool SaveLayoutChanges
		{
			get {return m_SaveLayoutChanges;}
			set {m_SaveLayoutChanges=value;}
		}

		/// <summary>
		/// Saves the Bar layout to file.
		/// </summary>
		/// <param name="FileName">Definition file name.</param>
		public void SaveLayout(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			
			this.SaveLayout(xmlDoc);

			xmlDoc.Save(FileName);
		}

		internal void SaveLayout(System.Xml.XmlDocument xmlDoc)
		{
			System.Xml.XmlElement xmlBar=xmlDoc.CreateElement("bar");
			xmlDoc.AppendChild(xmlBar);
			this.SerializeLayout(xmlBar);
		}

		/// <summary>
		/// Loads the Bar definition from file.
		/// </summary>
		/// <param name="FileName">Definition file name.</param>
		public void LoadLayout(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			xmlDoc.Load(FileName);
			this.LoadLayout(xmlDoc);			
		}

		internal void LoadLayout(System.Xml.XmlDocument xmlDoc)
		{
			if(xmlDoc.FirstChild.Name!="bar")
				throw(new System.InvalidOperationException("XML Format not recognized"));

			this.DeserializeLayout(xmlDoc.FirstChild as System.Xml.XmlElement);
		}

		/// <summary>
		/// Gets/Sets Bar layout as XML string.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string LayoutDefinition
		{
			get
			{
				System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
				SaveLayout(xmlDoc);
				return xmlDoc.OuterXml;
			}
			set
			{
				System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
				xmlDoc.LoadXml(value);
				LoadLayout(xmlDoc);
			}
		}

		internal void SerializeLayout(System.Xml.XmlElement xmlThisBar)
		{
			if(this.CustomBar)
			{
				this.Serialize(xmlThisBar,true);
			}
			else
			{
				xmlThisBar.SetAttribute("name",this.Name);
				xmlThisBar.SetAttribute("dockline",System.Xml.XmlConvert.ToString(m_DockLine));
				xmlThisBar.SetAttribute("layout",System.Xml.XmlConvert.ToString((int)this.LayoutType));

				if(m_DockOffset==0 && this.Left>0 && (this.DockSide==eDockSide.Top || this.DockSide==eDockSide.Bottom))
					xmlThisBar.SetAttribute("dockoffset",System.Xml.XmlConvert.ToString(this.Left));
				else if(m_DockOffset==0 && this.Top>0 && (this.DockSide==eDockSide.Left || this.DockSide==eDockSide.Right))
					xmlThisBar.SetAttribute("dockoffset",System.Xml.XmlConvert.ToString(this.Top));
				else
					xmlThisBar.SetAttribute("dockoffset",System.Xml.XmlConvert.ToString(m_DockOffset));

				if(m_AutoHideState)
				{
					xmlThisBar.SetAttribute("state",System.Xml.XmlConvert.ToString((int)eBarState.Docked));
					eDockSide dockside=eDockSide.None;
					switch(m_LastDockSiteInfo.DockSide)
					{
						case DockStyle.Left:
							dockside=eDockSide.Left;
							break;
						case DockStyle.Right:
							dockside=eDockSide.Right;
							break;
						case DockStyle.Top:
							dockside=eDockSide.Top;
							break;
						case DockStyle.Bottom:
							dockside=eDockSide.Bottom;
							break;
					}
					xmlThisBar.SetAttribute("dockside",System.Xml.XmlConvert.ToString((int)dockside));
				}
				else
				{
					xmlThisBar.SetAttribute("state",System.Xml.XmlConvert.ToString((int)m_BarState));
					xmlThisBar.SetAttribute("dockside",System.Xml.XmlConvert.ToString((int)this.DockSide));
					if(this.DockSide==eDockSide.None && m_Float!=null)
						xmlThisBar.SetAttribute("fpos",m_Float.Location.X+","+m_Float.Location.Y+","+this.DisplayRectangle.Width+","+this.DisplayRectangle.Height);
                    else if(!m_FloatingRect.IsEmpty)
                        xmlThisBar.SetAttribute("fpos", m_FloatingRect.X + "," + m_FloatingRect.Y + "," + m_FloatingRect.Width + "," + m_FloatingRect.Height);
				}

				IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
				if(m_BarState==eBarState.Floating && !this.Visible && ownersupport!=null && ownersupport.WereVisible.Count>0 && ownersupport.WereVisible.Contains(this))
					xmlThisBar.SetAttribute("visible",System.Xml.XmlConvert.ToString(true));
				else
				{
					if(m_AutoHideState)
					{
						xmlThisBar.SetAttribute("visible",System.Xml.XmlConvert.ToString(true));
						xmlThisBar.SetAttribute("autohide",System.Xml.XmlConvert.ToString(true));
					}
					else
						xmlThisBar.SetAttribute("visible",System.Xml.XmlConvert.ToString(this.Visible));
				}
			
				if(m_ItemContainer.MinWidth!=0)
					xmlThisBar.SetAttribute("dockwidth",System.Xml.XmlConvert.ToString(m_ItemContainer.MinWidth));
				if(m_ItemContainer.MinHeight!=0)
					xmlThisBar.SetAttribute("dockheight",System.Xml.XmlConvert.ToString(m_ItemContainer.MinHeight));
				if(m_SplitDockWidthPercent>0)
					xmlThisBar.SetAttribute("splitwidthpercent",System.Xml.XmlConvert.ToString(m_SplitDockWidthPercent));
				if(m_SplitDockHeightPercent>0)
					xmlThisBar.SetAttribute("splitheightpercent",System.Xml.XmlConvert.ToString(m_SplitDockHeightPercent));

				if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && this.SelectedDockTab>=0)
					xmlThisBar.SetAttribute("seldocktab",System.Xml.XmlConvert.ToString(this.SelectedDockTab));
			}

			System.Xml.XmlElement xmlItems=xmlThisBar.OwnerDocument.CreateElement("items");
			
			foreach(BaseItem item in m_ItemContainer.SubItems)
			{
				if(item.OriginalBarName!="" || item.OriginalPosition>=0 || item.UserCustomized || this.CustomBar || m_TabsRearranged || this.LayoutType == eLayoutType.DockContainer)
				{
					System.Xml.XmlElement xmlItem=xmlThisBar.OwnerDocument.CreateElement("item");
					xmlItem.SetAttribute("name",item.Name);
					xmlItem.SetAttribute("origBar",item.OriginalBarName);
					xmlItem.SetAttribute("origPos",System.Xml.XmlConvert.ToString(item.OriginalPosition));
					xmlItem.SetAttribute("pos",System.Xml.XmlConvert.ToString(m_ItemContainer.SubItems.IndexOf(item)));
                    if (!item.Visible)
                        xmlItem.SetAttribute("visible", System.Xml.XmlConvert.ToString(item.Visible));
					xmlItems.AppendChild(xmlItem);
				}
			}

			if(xmlItems.ChildNodes.Count>0)
				xmlThisBar.AppendChild(xmlItems);
		}

		internal void DeserializeLayout(System.Xml.XmlElement xmlBar)
		{
			m_BarDefinitionLoading=true;
			try
			{
				m_IgnoreAnimation=true;

				LoadCommonProperties(xmlBar);

				if(xmlBar.ChildNodes.Count>0)
				{
                    foreach (System.Xml.XmlElement xmlChild in xmlBar.ChildNodes)
                    {
                        if (xmlChild.Name == "items")
                        {
                            // Load items stored on bar first...
                            LoadLayoutItems(xmlBar);
                            break;
                        }
                    }
				}

                // Try to load floating position
                if (xmlBar.HasAttribute("fpos"))
                {
                    string s = xmlBar.GetAttribute("fpos");
                    string[] arr = s.Split(',');
                    if (arr.Length == 4)
                    {
                        Rectangle r = new Rectangle(System.Xml.XmlConvert.ToInt32(arr[0]), System.Xml.XmlConvert.ToInt32(arr[1]), System.Xml.XmlConvert.ToInt32(arr[2]), System.Xml.XmlConvert.ToInt32(arr[3]));
                        m_FloatingRect = r;
                    }
                }

				if((eDockSide)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dockside"))==eDockSide.None)
				{
					if(!System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("visible")))
						m_LoadingHideFloating=true;
				}

				if(xmlBar.HasAttribute("seldocktab"))
				{
					int iTab=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("seldocktab"));
					if(iTab>=0 && iTab<m_ItemContainer.SubItems.Count)
					{
						foreach(BaseItem dockItem in m_ItemContainer.SubItems)
							dockItem.Displayed=false;
						m_ItemContainer.SubItems[iTab].Displayed=true;
						if(m_AutoSyncBarCaption)
							this.Text=m_ItemContainer.SubItems[iTab].Text;
					}
				}

				// Last Thing to do so it is docked properly
				//m_DockLine=System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dockline"));
				if(m_ItemContainer.LayoutType==eLayoutType.DockContainer)
					RefreshDockTab(true);
				if(this.AutoHide)
				{
					if(xmlBar.HasAttribute("autohide"))
					{
						// If it is going back to auto-hide mode make sure it does not affect any bars on same line when we
						// bring it out of auto-hide state
						int minWidth=m_ItemContainer.MinWidth;
						int minHeight=m_ItemContainer.MinHeight;
						m_ItemContainer.MinWidth=0;
						m_ItemContainer.MinHeight=0;
						this.AutoHide=false;
						m_ItemContainer.MinWidth=minWidth;
						m_ItemContainer.MinHeight=minHeight;
					}
					else
						this.AutoHide=false;
				}

				this.DockSide=(eDockSide)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("dockside"));
				if(m_LoadingHideFloating) m_LoadingHideFloating=false;
				if(xmlBar.HasAttribute("autohide"))
				{
					if(!this.AutoHide)
					{
						base.Visible=false;
                        if (m_ItemContainer.MinWidth > 0)
                            this.Width = m_ItemContainer.MinWidth;
                        if (m_ItemContainer.MinHeight > 0)
                            this.Height = m_ItemContainer.MinHeight;
						this.AutoHide=true;
                        m_LastDockSiteInfo.DockedWidth = m_ItemContainer.MinWidth;
                        m_LastDockSiteInfo.DockedHeight = m_ItemContainer.MinHeight;
					}
				}
				else
					this.Visible=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("visible"));
			}
			finally
			{
				m_BarDefinitionLoading=false;
			}
		}

		private void LoadLayoutItems(System.Xml.XmlElement xmlBar)
		{
			bool customBar=false;
			eLayoutType layoutType=eLayoutType.Toolbar;
			DotNetBarManager manager=null;
			if(m_Owner!=null && m_Owner is DotNetBarManager)
				manager=m_Owner as DotNetBarManager;

			if(xmlBar.HasAttribute("layout"))
				layoutType=(eLayoutType)System.Xml.XmlConvert.ToInt32(xmlBar.GetAttribute("layout"));
			
			if(layoutType==eLayoutType.DockContainer)
				m_TabsRearranged=true;

			if(xmlBar.HasAttribute("custom"))
				customBar=System.Xml.XmlConvert.ToBoolean(xmlBar.GetAttribute("custom"));

			foreach(System.Xml.XmlElement xmlElem in xmlBar.ChildNodes)
			{
				switch(xmlElem.Name)
				{
					case "items":
					{
						for(int i=0;i<xmlElem.ChildNodes.Count;i++)
						{
							System.Xml.XmlElement xmlItem=xmlElem.ChildNodes[i] as System.Xml.XmlElement;
							string itemName=xmlItem.GetAttribute("name");
							string originalBar=xmlItem.GetAttribute("origBar");
							int originalPos=System.Xml.XmlConvert.ToInt32(xmlItem.GetAttribute("origPos"));
							int actualPos=System.Xml.XmlConvert.ToInt32(xmlItem.GetAttribute("pos"));
                            bool visible = true;
                            if(xmlItem.HasAttribute("visible"))
                                visible = System.Xml.XmlConvert.ToBoolean(xmlItem.GetAttribute("visible"));

							if(layoutType==eLayoutType.DockContainer)
							{
								// Just adjust item position
								if(this.Items.Contains(itemName))
								{
                                    BaseItem item = this.Items[itemName];

                                    if (item.Visible != visible)
                                        item.Visible = visible;

									if(this.Items.IndexOf(this.Items[itemName])!=actualPos)
									{
										this.Items.Remove(item);
                                        if (this.Items.Count > actualPos)
                                            this.Items.Insert(actualPos, item);
                                        else
                                            this.Items.Add(item);
									}
								}
								else if(manager!=null)
								{
									// Move item from other bar onto this bar...
									BaseItem item=manager.GetItem(itemName);
									if(item!=null && item is DockContainerItem)
									{
										Bar parentBar=item.ContainerControl as Bar;
										item.Parent.SubItems.Remove(item);
                                        if (item.Visible != visible)
                                            item.Visible = visible;
                                        if (this.Items.Count > actualPos)
                                            this.Items.Insert(actualPos, item);
                                        else
                                            this.Items.Add(item);
										if(parentBar!=null && parentBar.Items.Count==0)
										{
											if(parentBar.AutoHide) parentBar.AutoHide=false;
											parentBar.Visible=false;
											//manager.Bars.Remove(parentBar);
										}
										else if(parentBar!=null && parentBar.VisibleItemCount==0)
										{
											parentBar.Visible=false;
										}
									}
								}
							}
							else
							{
								BaseItem item=null;
								if(this.Items.Contains(itemName))
								{
									item=this.Items[itemName];
									if(this.Items.IndexOf(itemName)!=actualPos)
									{
										this.Items.Remove(item);
                                        if (this.Items.Count > actualPos)
                                            this.Items.Insert(actualPos, item);
                                        else
                                            this.Items.Add(item);
									}
								}
								else
								{
									if(originalBar!="" && manager!=null && manager.Bars.Contains(originalBar))
									{
										if(manager.Bars[originalBar].Items.Contains(itemName))
											item=manager.Bars[originalBar].Items[itemName];
									}
									if(item==null)
										item=manager.GetItem(itemName,true);
									if(item!=null)
									{
                                        if (item.ContainerControl == null || originalBar == "" && !(item.ContainerControl is Bar))
                                            item = item.Copy();
                                        else
                                            item.Parent.SubItems.Remove(item);
                                        if (this.Items.Count > actualPos)
                                            this.Items.Insert(actualPos, item);
                                        else
                                            this.Items.Add(item);
									}
								}
								if(item!=null)
								{
                                    if (item.Visible != visible)
                                        item.Visible = visible;
									item.OriginalBarName=originalBar;
									item.OriginalPosition=originalPos;
                                    item.UserCustomized = true;
								}
							}
						}
						break;
					}
				}
			}
		}

		// TODO: Menu Merge Implementation
		//		private bool IsParentMdiChild
		//		{
		//			get
		//			{
		//				IOwner owner=m_Owner as IOwner;
		//				if(owner!=null && owner.ParentForm!=null)
		//				{
		//					return ((IOwner)m_Owner).ParentForm.IsMdiChild;
		//				}
		//				return false;
		//			}
		//		}

		/// <summary>
		/// Sets the client size of the bar excluding caption. This method is useful when setting the size of the bars with layout type DockContainer.
		/// </summary>
		/// <param name="width">Width of bar in pixels.</param>
		/// <param name="height">Height of bar in pixels.</param>
        [Obsolete("Method is obsolete. Use DocumentDockUIManager.SetBarWidth or DocumentDockUIManager.SetBarHeight instead. You can obtain DocumentDockUIManager using DockSite.GetDocumentUIManager method.")]
        public void SetSize(int width, int height)
		{
			if(m_BarState==eBarState.Docked)
			{
				m_ItemContainer.MinHeight=height;
				m_ItemContainer.MinWidth=width;
				SyncLineMinWidth();
				SyncLineMinHeight();
			}
			else
			{
				this.Size=new Size(width,height);
			}
			if(this.IsHandleCreated)
				this.RecalcLayout();
		}

		/// <summary>
		/// Sets the client size of the bar excluding caption. This method is useful when setting the size of the bars with layout type DockContainer.
		/// </summary>
		/// <param name="size">New bar size</param>
        [Obsolete("Method is obsolete. Use DocumentDockUIManager.SetBarWidth or DocumentDockUIManager.SetBarHeight instead. You can obtain DocumentDockUIManager using DockSite.GetDocumentUIManager method.")]
        public void SetSize(System.Drawing.Size size)
		{
			SetSize(size.Width, size.Height);
		}

		/// <summary>
		/// Specifies background image position when container is larger than image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(eBackgroundImagePosition.Stretch),Description("Specifies background image position when container is larger than image.")]
		public eBackgroundImagePosition BackgroundImagePosition
		{
			get {return m_BackgroundImagePosition;}
			set
			{
				if(m_BackgroundImagePosition!=value)
				{
					m_BackgroundImagePosition=value;
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// Specifies the transparency of background image.
		/// </summary>
		[Browsable(true),DefaultValue((byte)255),DevCoBrowsable(true),Category("Appearance"),Description("Specifies the transparency of background image.")]
		public byte BackgroundImageAlpha
		{
			get {return m_BackgroundImageAlpha;}
			set
			{
				if(m_BackgroundImageAlpha!=value)
				{
					m_BackgroundImageAlpha=value;
					this.Refresh();
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackgroundImageAlpha()
		{
			return m_BackgroundImageAlpha!=255;
		}

		/// <summary>
		/// Sets/Gets the side bar image structure.
		/// </summary>
		[System.ComponentModel.Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>
		/// Gets/Sets the caption of the Bar. This text is displayed in title of the Bar when Bar is floating.
		/// </summary>
		[DevCoBrowsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if(base.Text!=value)
				{
					base.Text=value;
                    if (this.Visible && (m_BarState == eBarState.Floating ||
                        (m_BarState == eBarState.Docked || m_BarState == eBarState.AutoHide) && 
                        (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || 
                        m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)))
					{
						this.Refresh();
					}
				}
			}
		}

		[DevCoBrowsable(true)]
		public override System.Drawing.Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font=value;
				if(this.Font!=null)
					m_CustomFont=ShouldSerializeFont();
			}
		}

		/// <summary>
		/// Returns true if Font property should be serialized by Windows Forms designer.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeFont()
		{
			if(this.Font!=null)
			{
				if(this.Font.Name!=System.Windows.Forms.SystemInformation.MenuFont.Name || this.Font.Size!=System.Windows.Forms.SystemInformation.MenuFont.Size || this.Font.Style!=System.Windows.Forms.SystemInformation.MenuFont.Style)
					return true;
				else
					return false;
			}
			return false;
		}
        /// <summary>
        /// Designer method to reset the property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void ResetFont()
        {
            if (this.LayoutType == eLayoutType.DockContainer)
                base.ResetFont();
            else
                this.Font = System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
        }

		protected internal bool IsThemed
		{
			get
			{
				if(m_ThemeAware && this.Style!=eDotNetBarStyle.Office2000  && BarFunctions.ThemedOS && Themes.ThemesActive)
					return true;
				return false;
			}
		}

		/// <summary>
		/// Specifies whether Bar is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public virtual bool ThemeAware
		{
			get
			{
				return m_ThemeAware;
			}
			set
			{
				m_ThemeAware=value;
				m_ItemContainer.ThemeAware=value;
			}
		}

		/// <summary>
		/// Returns current Bar state.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public eBarState BarState
		{
			get
			{
				return m_BarState;
			}
		}

		internal void SetBarState(eBarState state)
		{
			m_BarState=state;
			if(m_BarState!=eBarState.Popup && m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
		}

		/// <summary>
		/// Returns the collection of sub-items hosted on the Bar.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SubItemsCollection Items
		{
			get
			{
				if(m_ItemContainer==null)
					return null;
				return m_ItemContainer.SubItems;
			}
		}

		/// <summary>
		/// Returns the reference to the container that containing the sub-items.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public GenericItemContainer ItemsContainer
		{
			get
			{
				return m_ItemContainer;
			}
		}

		/// <summary>
		/// Gets/Sets whether the items that could not be displayed on the non-wrap Bar are displayed on popup menu or popup Bar.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether the items that could not be displayed on the non-wrap Bar are displayed on popup menu or popup Bar."),DefaultValue(false)]
		public bool DisplayMoreItemsOnMenu
		{
			get
			{
				return m_ItemContainer.MoreItemsOnMenu;
			}
			set
			{
				m_ItemContainer.MoreItemsOnMenu=value;
			}
		}

		/// <summary>
		/// Gets/Sets the spacing in pixels between the sub-items.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Indicates the spacing in pixels between the sub-items."),DefaultValue(0)]
		public int ItemSpacing
		{
			get
			{
				return m_ItemContainer.ItemSpacing;
			}
			set
			{
				if(m_ItemContainer.ItemSpacing==value)
					return;
				m_ItemContainer.ItemSpacing=value;
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets/Sets the padding in pixels. This represents the spacing between the top edge of the bar and the top of the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Gets/Sets the padding in pixels. This represents the spacing between the top of the bar and the top of the item."),DefaultValue(1)]
		public int PaddingTop
		{
			get
			{
				return m_ItemContainer.PaddingTop;
			}
			set
			{
				if(m_ItemContainer.PaddingTop==value)
					return;
				m_ItemContainer.PaddingTop=value;
                if (this.DesignMode)
                    this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets/Sets the padding in pixels. This represents the spacing between the bottom edge of the bar and the bottom of the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Gets/Sets the padding in pixels. This represents the spacing between the bottom edge of the bar and the bottom of the item."),DefaultValue(1)]
		public int PaddingBottom
		{
			get
			{
				return m_ItemContainer.PaddingBottom;
			}
			set
			{
				if(m_ItemContainer.PaddingBottom==value)
					return;
				m_ItemContainer.PaddingBottom=value;
                if (this.DesignMode)
                    this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets/Sets the padding in pixels. This represents the spacing between the left edge of the bar and the left side of the first item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Gets/Sets the padding in pixels. This represents the spacing between the left edge of the bar and the left position of the first item."),DefaultValue(1)]
		public int PaddingLeft
		{
			get
			{
				return m_ItemContainer.PaddingLeft;
			}
			set
			{
				if(m_ItemContainer.PaddingLeft==value)
					return;
				m_ItemContainer.PaddingLeft=value;
                if (this.DesignMode)
                    this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets/Sets the padding in pixels. This represents the spacing between the right edge of the bar and the right side of the last item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Gets/Sets the padding in pixels. This represents the spacing between the right edge of the bar and the right side of the last item."),DefaultValue(1)]
		public int PaddingRight
		{
			get
			{
				return m_ItemContainer.PaddingRight;
			}
			set
			{
				if(m_ItemContainer.PaddingRight==value)
					return;
				m_ItemContainer.PaddingRight=value;
                if (this.DesignMode)
                    this.RecalcLayout();
			}
		}

		/// <summary>
		/// Sets/Gets whether bar is menu bar. Menu bar will show system icons
		/// for Maximized forms in MDI Applications. Only one bar can be a Menu bar in an application.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates Bar layout type."),DefaultValue(false)]
		public bool MenuBar
		{
			get
			{
				return m_MenuBar;
			}
			set
			{
				m_MenuBar=value;
				SetupAccessibility();
			}
		}

        /// <summary>
        /// Gets or sets the visual type of the bar. The type specified here is used to determine the appearance of the bar.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(eBarType.Toolbar), Category("Appearance"), Description("Indicates visual type of the bar. The type specified here is used to determine the appearance of the bar.")]
        public eBarType BarType
        {
            get { return m_BarType; }
            set
            {
                m_BarType = value;
                if (m_BarType == eBarType.StatusBar)
                    m_ItemContainer.ToolbarItemsAlign = eContainerVerticalAlignment.Middle;
                else
                    m_ItemContainer.ToolbarItemsAlign = eContainerVerticalAlignment.Top;
                if (BarType == eBarType.DockWindow)
                    this.ResetFont();
                this.Invalidate();
            }
        }

		/// <summary>
		/// Gets or sets Bar Color Scheme.
		/// </summary>
		[System.ComponentModel.Editor(typeof(ColorSchemeVSEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Gets or sets Bar Color Scheme."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DevComponents.DotNetBar.ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				if(value==null)
					throw new ArgumentException("NULL is not a valid value for this property.");
				m_ColorScheme=value;
				if(this.Visible)
					this.Refresh();
			}
		}

		internal ColorScheme GetColorScheme()
		{
            if (BarFunctions.IsOffice2007Style(this.Style))
            {
                Office2007Renderer r = this.GetRenderer() as Office2007Renderer;
                if (r != null && r.ColorTable.LegacyColors != null)
                    return r.ColorTable.LegacyColors;
            }

			if(m_Owner is DotNetBarManager && ((DotNetBarManager)m_Owner).UseGlobalColorScheme)
				return ((DotNetBarManager)m_Owner).ColorScheme;
			return m_ColorScheme;
		}

		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColorScheme()
		{
			return m_ColorScheme.SchemeChanged;
		}

        /// <summary>
        /// Resets the ColorScheme property to its default value.
        /// </summary>
        public void ResetColorScheme()
        {
            m_ColorScheme.Refresh();
            this.Invalidate();
        }

		/// <summary>
		/// Gets or sets Caption (Title bar) background color.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the background color of the caption (Title bar)."),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color CaptionBackColor
		{
			get
			{
				return m_CaptionBackColor;
			}
			set
			{
				if(m_CaptionBackColor!=value)
				{
					m_CaptionBackColor=value;
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets Caption (Title bar) text color.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the caption (Title bar) text color."),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color CaptionForeColor
		{
			get
			{
				return m_CaptionForeColor;
			}
			set
			{
				if(m_CaptionForeColor!=value)
				{
					m_CaptionForeColor=value;
					this.Refresh();
				}
			}
		}

		internal void ClearMDIChildSystemItems(bool bRecalcLayout)
		{
			if(m_ItemContainer==null)
				return;
			try
			{
				if(m_ItemContainer.SubItems.Contains("dotnetbarsysiconitem"))
					m_ItemContainer.SubItems.Remove("dotnetbarsysiconitem");
				if(m_ItemContainer.SubItems.Contains("dotnetbarsysmenuitem"))
					m_ItemContainer.SubItems.Remove("dotnetbarsysmenuitem");
				if(bRecalcLayout)
					this.RecalcLayout();
			}
			catch(Exception)
			{
			}
		}

		internal void ShowMDIChildSystemItems(System.Windows.Forms.Form objMdiChild, bool bRecalcLayout)
		{
			ClearMDIChildSystemItems(bRecalcLayout);
			
			if(objMdiChild==null)
				return;

			MDISystemItem mdi=new MDISystemItem("dotnetbarsysiconitem");
			mdi.Icon=objMdiChild.Icon;
			if(!objMdiChild.ControlBox)
				mdi.CloseEnabled=false;
			if(!objMdiChild.MinimizeBox)
				mdi.MinimizeEnabled=false;
			mdi.Click+=new System.EventHandler(this.MDISysItemClick);
			mdi.IsSystemIcon=true;
			m_ItemContainer.SubItems.Add(mdi,0);

			mdi=new MDISystemItem("dotnetbarsysmenuitem");
			if(!objMdiChild.ControlBox)
				mdi.CloseEnabled=false;
			if(!objMdiChild.MinimizeBox)
				mdi.MinimizeEnabled=false;
			mdi.ItemAlignment=eItemAlignment.Far;
			mdi.Click+=new System.EventHandler(this.MDISysItemClick);

			m_ItemContainer.SubItems.Add(mdi);
			
			if(bRecalcLayout)
				this.RecalcLayout();
		}

		private void MDISysItemClick(object sender, System.EventArgs e)
		{
			MDISystemItem mdi=sender as MDISystemItem;
			IOwner owner=m_Owner as IOwner;
			Form frm=null;
			if(owner!=null)
				frm=owner.ActiveMdiChild;
			if(frm==null)
			{
				ClearMDIChildSystemItems(true);
				return;
			}
			if(mdi.LastButtonClick==SystemButton.Minimize)
			{
				NativeFunctions.PostMessage(frm.Handle.ToInt32(),NativeFunctions.WM_SYSCOMMAND,NativeFunctions.SC_MINIMIZE,0);
				//frm.WindowState=FormWindowState.Minimized;
			}
			else if(mdi.LastButtonClick==SystemButton.Restore)
			{
				NativeFunctions.PostMessage(frm.Handle.ToInt32(),NativeFunctions.WM_SYSCOMMAND,NativeFunctions.SC_RESTORE,0);
				//frm.WindowState=FormWindowState.Normal;
			}
			else if(mdi.LastButtonClick==SystemButton.Close)
			{
				NativeFunctions.PostMessage(frm.Handle.ToInt32(),NativeFunctions.WM_SYSCOMMAND,NativeFunctions.SC_CLOSE,0);
			}
			else if(mdi.LastButtonClick==SystemButton.NextWindow)
			{
				NativeFunctions.PostMessage(frm.Handle.ToInt32(),NativeFunctions.WM_SYSCOMMAND,NativeFunctions.SC_NEXTWINDOW,0);
			}
		}

        /// <summary>
        /// Gets or sets whether toolbars with appropriate style appear with rounded corners. Default value is true.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), Category("Appearance"), Description("Indicates whether toolbars with appropriate style appear with rounded corners."), DefaultValue(true)]
        public bool RoundCorners
        {
            get { return m_RoundCorners; }
            set
            {
                m_RoundCorners = value;
                this.RecalcLayout();
            }
        }

		/// <summary>
		/// Gets/Sets the visual style of the Bar.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies the visual style of the Bar."),DefaultValue(eDotNetBarStyle.OfficeXP)]
		public eDotNetBarStyle Style
		{
			get
			{
				return m_ItemContainer.Style;
			}
			set
			{
				//if(m_ItemContainer.Style==value)
				//	return;
                m_ColorScheme.SwitchStyle(value);
				m_ItemContainer.Style=value;
                if ((m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle)) && this.LayoutType == eLayoutType.Toolbar && this.GrabHandleStyle != eGrabHandleStyle.None && this.GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
					m_GrabHandleStyle=eGrabHandleStyle.Office2003;
				SetDockTabStyle(value);
				if(this.AutoHide)
				{
					AutoHidePanel panel=this.GetAutoHidePanel();
					if(panel!=null)
						panel.Style=value;
				}
				else
				{
					this.Invalidate();
					this.RecalcLayout();
				}
			}
		}

        //internal int MinClientSize
        //{
        //    get
        //    {
        //        if(this.SelectedDockTab>=0)
        //        {
        //            return ((DockContainerItem)m_TabDockItems.SelectedTab.AttachedItem).MinFormClientSize;
        //        }
        //        else if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && m_ItemContainer.SubItems.Count==1 && m_ItemContainer.SubItems[0] is DockContainerItem)
        //            return ((DockContainerItem)m_ItemContainer.SubItems[0]).MinFormClientSize;

        //        return m_MinClientSize;
        //    }
        //    set {m_MinClientSize=value;}
        //}

		internal bool IsPositionOnDockTab(int x, int y)
		{
			// Receives the screen coordinates
			Point p=this.PointToClient(new Point(x,y));
			if(m_GrabHandleRect.Contains(p) || m_TabDockItems!=null && m_TabDockItems.Bounds.Contains(p))
				return true;
			return false;
		}

		internal void AppLostFocus()
		{
			if(m_ItemContainer!=null)
				m_ItemContainer.ContainerLostFocus(true);
			if(!this.IsDisposed)
				AccessibilityNotifyClients(AccessibleEvents.StateChange,0);
		}
		internal void AppActivate()
		{
			if(!this.IsDisposed)
				AccessibilityNotifyClients(AccessibleEvents.StateChange,0);
		}

		internal void HideBar()
		{
            if (m_DesignerParent && this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
                m_DesignerParent = false;
            }

			if(m_BarState==eBarState.Floating)
			{
				if(m_Float!=null)
				{
					m_Float.Hide();
					base.OnVisibleChanged(new EventArgs());
				}
			}
			else
			{
				base.Hide();

                if (!m_DockingInProgress && !m_LayoutSuspended && this.Parent is DockSite && ((DockSite)this.Parent).DocumentDockContainer != null)
                    ((DockSite)this.Parent).GetDocumentUIManager().AdjustContainerSize(this, true);

				if(m_Owner!=null && m_Owner is DotNetBarManager && ((DotNetBarManager)m_Owner).IsDisposed)
					return;
				this.RecalcLayout();
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
                if (parent != null && parent.GetType().Name.IndexOf("DesignerFrame") >= 0)
                    break;
            }
            if (parent == null || parent.Parent == null) return false;
            //parent = parent.Parent;
            Point p = parent.PointToClient(this.Location);
            parent.Controls.Add(this);
            this.Location = p;
            base.Show();
            this.Update();
            this.BringToFront();
            m_DesignerParent = true;
            return true;
        }

		internal void ShowBar()
		{
			if(PassiveBar)
			{
				base.Show();
				this.Update();
				return;
			}

			if(m_BarState==eBarState.Floating)
			{
				if(m_Float==null)
				{
					m_Float=new FloatingContainer(this);
					m_Float.CreateControl();
					if(this.Parent!=null)
					{
						if(this.Parent is DockSite)
							((DockSite)this.Parent).RemoveBar(this);
						else
							this.Parent.Controls.Remove(this);
					}
					this.Parent=null;
					m_Float.Controls.Add(this);
					// WE OVERRIDE BASE LOCATION WE MUST USE BASE.
					base.Location=new Point(0,0);
				}
				if(m_Float!=null)
				{
					// TODO: Show method did not want to show the form, check has it been fixed in newer versions
					//m_Float.Show();
					//NativeFunctions.SetWindowPos(m_Float.Handle.ToInt32(),NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOSIZE | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE);
					//m_Float.TopMost=true;
					m_Float.TopLevel=true;
					m_Float.Visible=true;
					if(!base.Visible)
						base.Visible=true;
					m_Float.Refresh();
				}
			}
			else if(m_BarState==eBarState.Popup)
			{
                // Design mode add
                if (m_ParentItem != null && (m_ParentItem.Site != null && m_ParentItem.Site.DesignMode ||
                    m_ParentItem.Parent != null && m_ParentItem.Parent.Site != null && m_ParentItem.Parent.Site.DesignMode))
                {
                    if (AddtoDesignTimeContainer())
                        return;
                }

				//NativeFunctions.sndPlaySound("MenuPopup",NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
				ePopupAnimation animation=m_PopupAnimation;
				if(!BarFunctions.SupportsAnimation)
					animation=ePopupAnimation.None;
				else
				{
					IOwnerMenuSupport ownersupport=m_Owner as IOwnerMenuSupport;
					if(animation==ePopupAnimation.ManagerControlled)
					{
						if(ownersupport!=null)
							animation=ownersupport.PopupAnimation;
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

                if (BarFunctions.IsOffice2007Style(this.Style) && this.BarType == eBarType.Toolbar)
                    SetRoundRegion(this);
			
				if(animation==ePopupAnimation.Fade && Environment.OSVersion.Version.Major>=5)
				{
					// TODO: Blending was leaving the white dots in the region that was excluded, make sure that it is not happening for final release, test other AnimateWindows
					NativeFunctions.AnimateWindow(this.Handle.ToInt32(),BarFunctions.ANIMATION_INTERVAL,NativeFunctions.AW_BLEND);
				}
				else if(animation==ePopupAnimation.Slide)
					NativeFunctions.AnimateWindow(this.Handle.ToInt32(),BarFunctions.ANIMATION_INTERVAL,(NativeFunctions.AW_SLIDE | NativeFunctions.AW_HOR_POSITIVE | NativeFunctions.AW_VER_POSITIVE));
				else if(animation==ePopupAnimation.Unfold)
					NativeFunctions.AnimateWindow(this.Handle.ToInt32(),BarFunctions.ANIMATION_INTERVAL,(NativeFunctions.AW_HOR_POSITIVE | NativeFunctions.AW_VER_POSITIVE));
				else
					base.Show();

				if(animation!=ePopupAnimation.None && this.Controls.Count>0)
					this.Refresh();

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
                    m_DropShadow.UpdateShadow();
				}
			}
			else
			{
				// Retain bar's position in controls collection it can change when bar is shown
				// OnVisibleChanged will be triggered and index of the bar will be checked against
				// this value. If it has changed it will be restored.
				if(this.Parent is DockSite)
					m_BarShowIndex=this.Parent.Controls.IndexOf(this);
				else
					m_BarShowIndex=-1;

                if (!m_DockingInProgress && !m_LayoutSuspended && this.Parent is DockSite && this.Parent.Dock !=DockStyle.Fill && ((DockSite)this.Parent).DocumentDockContainer != null)
                {
                    DockSite ds = this.Parent as DockSite;
                    m_LayoutSuspended = true;
                    base.Show();
                    m_LayoutSuspended = false;
                    ds.GetDocumentUIManager().AdjustContainerSize(this, true);
                    // Reset the side by side docking for the bars so they are uniform
                    if (!m_BarDefinitionLoading)
                    {
                        DocumentBaseContainer dc = ds.GetDocumentUIManager().GetDocumentFromBar(this);
                        if (dc != null && dc.Parent is DocumentDockContainer)
                        {
                            DocumentDockContainer p = dc.Parent as DocumentDockContainer;
                            int visibleCount = p.Documents.VisibleCount;
                            if (visibleCount > 1)
                            {
                                if (p.Orientation == eOrientation.Horizontal && (ds.Dock == DockStyle.Top || ds.Dock == DockStyle.Bottom))
                                {
                                    //float f = 1 + dc.LayoutBounds.Width / (float)(p.DisplayBounds.Width - dc.LayoutBounds.Width);
                                    //dc.SetLayoutBounds(new Rectangle(dc.LayoutBounds.X, dc.LayoutBounds.Y, (int)(dc.LayoutBounds.Width * f), dc.LayoutBounds.Height));
                                    dc.SetLayoutBounds(new Rectangle(dc.LayoutBounds.X, dc.LayoutBounds.Y, (int)(p.LayoutBounds.Width / visibleCount), dc.LayoutBounds.Height));
                                }
                                else if (p.Orientation == eOrientation.Vertical && (ds.Dock == DockStyle.Left || ds.Dock == DockStyle.Right))
                                {
                                    //float f = 1 + dc.LayoutBounds.Height / (float)(p.DisplayBounds.Height - dc.LayoutBounds.Height);
                                    //dc.SetLayoutBounds(new Rectangle(dc.LayoutBounds.X, dc.LayoutBounds.Y, dc.LayoutBounds.Width, (int)(dc.LayoutBounds.Height * f)));
                                    dc.SetLayoutBounds(new Rectangle(dc.LayoutBounds.X, dc.LayoutBounds.Y, dc.LayoutBounds.Width, (int)(p.LayoutBounds.Height /visibleCount)));
                                }
                            }
                        }
                    }
                    DotNetBarManager man = this.Owner as DotNetBarManager;
                    if (man != null && man.SuspendLayout)
                        ds.NeedsLayout = true;
                }
                else
                    base.Show();

				m_BarShowIndex=-1;
			}
			// This makes the bar paint BEFORE it returns out of this function
			this.Update();
		}

		internal bool DockingInProgress
		{
			get
			{
				return m_DockingInProgress;
			}
		}

		/// <summary>
		/// Gets/Sets whether the items will be wrapped into next line when Bar is full. Applies to both docked and floating Bar states.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool WrapItems
		{
			get
			{
				return m_ItemContainer.WrapItems;
			}
		}

		/// <summary>
		/// Gets/Sets whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are docked.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Indicates whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are docked."),DefaultValue(false)]
		public bool WrapItemsDock
		{
			get
			{
				return m_WrapItemsDock;
			}
			set
			{
				m_WrapItemsDock=value;
			}
		}

		/// <summary>
		/// Gets/Sets whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are floating.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Indicates whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are floating."),DefaultValue(true)]
		public bool WrapItemsFloat
		{
			get
			{
				return m_WrapItemsFloat;
			}
			set
			{
				m_WrapItemsFloat=value;
			}
		}

		/// <summary>
		/// Gets/Sets the grab handle style of the docked Bars.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the grab handle style of the docked Bars."),DefaultValue(eGrabHandleStyle.None)]
		public eGrabHandleStyle GrabHandleStyle
		{
			get
			{
				return m_GrabHandleStyle;
			}
			set
			{
				if(m_GrabHandleStyle==value)
					return;
				m_GrabHandleStyle=value;
				SetupAccessibility();
				if(this.Visible && !m_LayoutSuspended)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets the grab handle client rectangle.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public Rectangle GrabHandleRect
		{
			get
			{
				return m_GrabHandleRect;
			}
		}

        internal void DockContainerItemCanCloseChanged(DockContainerItem item, eDockContainerClose oldValue, eDockContainerClose newValue)
        {
            if (SelectedDockContainerItem != item) return;
            UpdateCloseButtonVisibility();
        }

        private void UpdateCloseButtonVisibility()
        {
            bool canHideResolved = this.CanHideResolved;
            
            if (m_BarState == eBarState.Floating || m_BarState == eBarState.Docked && (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted))
                this.Invalidate(m_GrabHandleRect);

            if (m_TabDockItems != null && m_TabDockItems._TabSystemBox != null)
            {    
                m_TabDockItems._TabSystemBox.CloseVisible = canHideResolved;
            }
        }

        /// <summary>
        /// Returns CanClose based on the selected dock-container item.
        /// </summary>
        private bool CanHideResolved
        {
            get
            {
                DockContainerItem item = this.SelectedDockContainerItem;
                if (item == null || item.CanClose == eDockContainerClose.Inherit)
                    return CanHide;
                return item.CanClose == eDockContainerClose.Yes;
            }
        }

		/// <summary>
		/// Gets/Sets whether the Bar can be hidden by end-user. Applies to Document docked bars only.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Determines whether bar can be hidden."),DefaultValue(false)]
		public bool CanHide
		{
			get
			{
				return m_CanHide;
			}
			set
			{
				if(m_CanHide==value)
					return;
				m_CanHide=value;
                if (m_BarState == eBarState.Floating || m_BarState == eBarState.Docked && (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted))
					this.Invalidate(m_GrabHandleRect);
				UpdateDockTabSettings();
				if(m_TabDockItems!=null && BarFunctions.IsHandleValid(this))
				{
					m_TabDockItems.RecalcSize();
					if(this.DesignMode)
                        m_TabDockItems.Invalidate();						
				}
			}
		}

		/// <summary>
		/// Gets/Sets border style when Bar is docked.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates border style when Bar is docked."),DefaultValue(eBorderType.None)]
		public eBorderType DockedBorderStyle
		{
			get
			{
				return m_DockedBorder;
			}
			set
			{
				if(m_DockedBorder==value)
					return;
				m_DockedBorder=value;
				if(m_BarState==eBarState.Docked && this.Visible)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets/Sets whether floating bar is hidden when application loses focus.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Indicates whether floating bar is hidden when application loses focus.")]
		public bool HideFloatingInactive
		{
			get
			{
				return m_HideFloatingInactive;
			}
			set
			{
				m_HideFloatingInactive=value;
			}
		}

		/// <summary>
		/// Gets/Sets whether tab navigation buttons are shown for tabbed dockable bars.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Docking"),Description("Indicates whether tab navigation buttons are shown for tabbed dockable bars.")]
		public bool TabNavigation
		{
			get
			{
				return m_TabNavigation;
			}
			set
			{
				if(m_TabNavigation!=value)
				{
					m_TabNavigation=value;
					UpdateDockTabSettings();
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the border line color when docked border is a single line.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the border line color when docked border is a single line.")]
		public Color SingleLineColor
		{
			get
			{
				return m_SingleLineColor;
			}
			set
			{
				if(m_SingleLineColor!=value)
				{
					m_SingleLineColor=value;
					if(m_BarState==eBarState.Docked && this.Visible && m_DockedBorder==eBorderType.SingleLine)
						this.Refresh();
				}
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSingleLineColor()
		{
			return (m_SingleLineColor!=SystemColors.ControlDark);
		}

		/// <summary>
		/// Gets/Sets whether Bar is visible or not.
		/// </summary>
		[DevCoBrowsable(true), DefaultValue(true)]
		public new bool Visible
		{
			get
			{
				if(m_BarState==eBarState.Floating && m_Float!=null)
					return m_Float.Visible;
				return base.Visible;
			}
			set
			{
                m_IsVisible = value;
				if(base.Visible==value)
				{
					// This allows the Bar to be hidden when Visible property is set from forms constructor...
					if(/*!this.IsHandleCreated &&*/ value==false)
						base.Visible=false;
					return;
				}
				if(value)
					this.ShowBar();
				else
					this.HideBar();
			}
		}

		/// <summary>
		/// Returns number of items that have Visible property set to true.
		/// </summary>
		[Browsable(false)]
		public int VisibleItemCount
		{
			get
			{
				return m_ItemContainer.VisibleSubItems;
			}
		}

        private BaseItem GetFirstVisibleItem()
		{
			foreach(BaseItem item in this.Items)
			{
				if(item.Visible)
					return item;
			}
			return null;
		}

		/// <summary>
		/// Gets or sets whether bar is valid drop target for end-user bar customization. Default value is true.
        /// When bar is used as dock container then you can use this property to prevent docking of other bars as dock tabs.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Determines whether end-user can drag and drop items on the Bar."),DefaultValue(true)]
		public bool AcceptDropItems
		{
			get
			{
				return m_AcceptDropItems;
			}
			set
			{
				m_AcceptDropItems=value;
			}
		}

		/// <summary>
		/// Gets or sets whether items on the Bar can be customized.
		/// </summary>
		[Browsable(true),Category("Behavior"),Description("Gets or sets whether items on the Bar can be customized."),DefaultValue(true)]
		public bool CanCustomize
		{
			get
			{
				if(m_ItemContainer!=null)
					return m_ItemContainer.CanCustomize;
				return false;
			}
			set
			{
                if (this.DesignMode) return;
				if(m_ItemContainer!=null)
					m_ItemContainer.CanCustomize=value;
			}
		}


		/// <summary>
		/// Makes the Bar display by setting the visible property to true.
		/// </summary>
		public new void Show()
		{
			this.Visible=true;
		}

		/// <summary>
		/// Hides the Bar.
		/// </summary>
		public new void Hide()
		{
			if(this.AutoHide)
				this.AutoHide=false;
			this.Visible=false;
			this.RecalcLayout();
		}

		/// <summary>
		/// Specifies whether Bar was created by user using Customize dialog.
		/// </summary>
		[System.ComponentModel.Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CustomBar
		{
			get
			{
				return m_CustomBar;
			}
			set
			{
				m_CustomBar=value;
			}
		}

		/// <summary>
		/// Gets/Sets the Bar name used to identify Bar from code.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Bar name that can be used to identify Bar from code.")]
		public new string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name=value;
				if(this.Site!=null)
					this.Site.Name=value;
			}
		}

		/// <summary>
		/// Gets/Sets the Image size for all sub-items on the Bar.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifies the Image size that will be used by items on this bar."),DefaultValue(eBarImageSize.Default)]
		public eBarImageSize ImageSize
		{
			get
			{
				return m_ImageSize;
			}
			set
			{
				if(m_ImageSize==value)
					return;
				m_ImageSize=value;
				m_ItemContainer.RefreshImageSize();
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets the layout type.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Indicates Bar layout type."),DefaultValue(eLayoutType.Toolbar)]
		public eLayoutType LayoutType
		{
			get
			{
                if (m_ItemContainer == null)
                    return eLayoutType.Toolbar;
				return m_ItemContainer.LayoutType;
			}
			set
			{
				if(m_ItemContainer.LayoutType==value)
					return;
				m_ItemContainer.LayoutType=value;
                if (value == eLayoutType.DockContainer)
                    this.ResetFont();
				SetupAccessibility();
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether all buttons are automatically resized to the largest button in collection.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates that buttons will be automatically resized to match the size of the largest button on the bar."),DefaultValue(false)]
		public bool EqualButtonSize
		{
			get
			{
				return m_ItemContainer.EqualButtonSize;
			}
			set
			{
				if(m_ItemContainer.EqualButtonSize==value)
					return;
				m_ItemContainer.EqualButtonSize=value;
				this.RecalcLayout();
			}
		}

        /// <summary>
        /// Gets or sets rounded corner size for styles that use rounded corners.
        /// </summary>
        internal int CornerSize
        {
            get { return m_CornerSize; }
            set { m_CornerSize = value; }
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


        /// <summary>
        /// Gets or sets whether mouse over fade effect is enabled for buttons. Default value is false. Note that Fade effect
        /// will work only when Office2007 style is used. For other styles this property has no effect and fade animation is not used regardless
        /// this property setting.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance"), Description("Indicates whether mouse over fade effect for buttons is enabled.")]
        public bool FadeEffect
        {
            get { return m_FadeEffect; }
            set
            {
                m_FadeEffect = value;
            }
        }

        /// <summary>
        /// Gets whether fade effect should be in use.
        /// </summary>
        internal bool IsFadeEnabled
        {
            get
            {
                if (this.DesignMode || (!BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle)) || m_FadeEffect && NativeFunctions.IsTerminalSession() || this.IsThemed || TextDrawing.UseTextRenderer)
                    return false;
                return m_FadeEffect;
            }
        }

		/// <summary>
		/// Gets or sets the Bar back color.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies background color of the Bar."),DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override Color BackColor
		{
			get
			{
				if(m_ItemContainer!=null)
					return m_ItemContainer.BackColor;
				else
				{
					return base.BackColor;
				}
			}
			set
			{
				if(m_ItemContainer!=null)
					m_ItemContainer.BackColor=value;
				else
					base.BackColor=value;
                this.Invalidate();
			}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void ResetBackColor()
		{
			if(m_ItemContainer!=null)
				m_ItemContainer.BackColor=Color.Empty;
			else
				base.BackColor=SystemColors.Control;
		}
		public bool ShouldSerializeBackColor()
		{
			if(m_ItemContainer!=null)
			{
				if(!m_ItemContainer.m_BackgroundColor.IsEmpty)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns whether Size property should be serialized.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSize()
		{
			if(this.LayoutType==eLayoutType.DockContainer && this.Parent is DockSite && this.Parent.Dock==DockStyle.Fill)
				return false;
			return true;
		}

		/// <summary>
		/// Gets or sets the Bar customize menu (Applies to the bars with LayoutType set to DockWindow only).
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates the Customize popup menu."),DefaultValue(null)]
		public PopupItem CustomizeMenu
		{
			get
			{
				return m_CustomizeMenu;
			}
			set
			{
				m_CustomizeMenu=value;
			}
		}

		/// <summary>
		/// Indicates the auto-hide side of the parent form where bar is positioned.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates the auto-hide side of the parent form where bar is positioned.")]
		public eDockSide AutoHideSide
		{
			get
			{
				eDockSide side=eDockSide.None;
				if(m_AutoHideState)
				{
					switch(m_LastDockSiteInfo.DockSide)
					{
						case DockStyle.Left:
						{
							side=eDockSide.Left;
							break;
						}
						case DockStyle.Right:
						{
							side=eDockSide.Right;
							break;
						}
						case DockStyle.Top:
						{
							side=eDockSide.Top;
							break;
						}
						default:
						{
							side=eDockSide.Bottom;
							break;
						}
					}
				}
				return side;
			}
		}

        /// <summary>
        /// Gets or sets whether tab text is always visible while bar is in auto-hide state. Default value is false which indicates that only text for the active dock tab is visible.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Auto-Hide"), Description("")]
        public bool AutoHideTabTextAlwaysVisible
        {
            get { return m_AutoHideTextAlwaysVisible; }
            set
            {
                m_AutoHideTextAlwaysVisible = value;
                if (this.AutoHide && this.GetAutoHidePanel() != null)
                    this.GetAutoHidePanel().Invalidate();
            }
        }

        private bool m_AutoHideStateDelayed = false;
		/// <summary>
		/// Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only.
		/// </summary>
        [Browsable(true), DefaultValue(false), DevCoBrowsable(true), System.ComponentModel.Category("Auto-Hide"), System.ComponentModel.Description("Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool AutoHide
		{
			get
			{
				return m_AutoHideState;
			}
			set
			{
				if(this.DesignMode)
					return;
                if (m_LayoutSuspended)
                {
                    m_AutoHideStateDelayed = value;
                    return;
                }
				if(!m_CanAutoHide || this.LayoutType!=eLayoutType.DockContainer || this.Parent!=null && this.Parent.Dock == DockStyle.Fill && value)
					return;
				if(m_AutoHideState==value)
					return;
                
				m_AutoHideState=value;
				AutoHideStateChanged();
                InvokeAutoHideChanged();
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the bar when bar is in auto-hide state.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.Category("Auto-Hide"),System.ComponentModel.Description("Gets or sets the visibility of the bar when bar is in auto-hide state."), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public bool AutoHideVisible
		{
			get
			{
				return this.Visible;
			}
			set
			{
				if(!this.AutoHide || this.Visible == value)
                    return;

				AutoHidePanel panel=GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
				if(value && panel!=null && m_AutoHideState) 
					panel.ShowBar(this);
				else if(!value && panel!=null && m_AutoHideState) 
					panel.HideBar(this);
			}
		}

		/// <summary>
		/// Indicates whether Bar can be auto hidden.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Auto-Hide"),Description("Indicates whether Bar can be auto hidden.")]
		public bool CanAutoHide
		{
			get
			{
				return m_CanAutoHide;
			}
			set
			{
				m_CanAutoHide=value;
				if(!m_CanAutoHide)
				{
					m_SystemButtons.AutoHideButtonRect=Rectangle.Empty;
					m_SystemButtons.MouseOverAutoHide=false;
					m_SystemButtons.MouseDownAutoHide=false;
				}
				if(this.IsHandleCreated)
					this.Invalidate();
			}
		}

        private Component m_GlobalParentComponent = null;
        /// <summary>
        /// Gets or sets the global parent control used as part of Global Items feature when bar is used as context menu bar. This property is used internally by
        /// DotNetBar and should not be set directly.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Component GlobalParentComponent
        {
            get { return m_GlobalParentComponent; }
            set { m_GlobalParentComponent = value; }
        }

		/// <summary>
		/// Returns the first child item with specified name regardless of it's hierarchy.
		/// </summary>
		/// <param name="name">Item name.</param>
		/// <returns></returns>
		public BaseItem GetItem(string name)
		{
			BaseItem item=BarFunctions.GetSubItemByName(m_ItemContainer,name);
			if(item!=null)
				return item;

            if (m_GlobalParentComponent != null)
            {
                if (m_GlobalParentComponent is RibbonControl)
                    return ((RibbonControl)m_GlobalParentComponent).RibbonStrip.GetItem(name);
                else if (m_GlobalParentComponent is DotNetBarManager)
                    return ((DotNetBarManager)m_GlobalParentComponent).GetItem(name);
            }

			return null;
		}

		/// <summary>
		/// Returns the collection of items with the specified name.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <returns></returns>
		public ArrayList GetItems(string ItemName)
		{
            ArrayList list = new ArrayList(15);
			if (m_GlobalParentComponent != null)
            {
                if (m_GlobalParentComponent is RibbonControl)
                    list.AddRange(((RibbonControl)m_GlobalParentComponent).RibbonStrip.GetItems(ItemName));
                else if (m_GlobalParentComponent is DotNetBarManager)
                    list.AddRange(((DotNetBarManager)m_GlobalParentComponent).GetItems(ItemName));
            }
            else
            {
                BarFunctions.GetSubItemsByName(m_ItemContainer, ItemName, list);
            }

			return list;
		}

		/// <summary>
		/// Returns the collection of items with the specified name and type.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <param name="itemType">Item type to look for.</param>
		/// <returns></returns>
		public ArrayList GetItems(string ItemName, Type itemType)
		{
			ArrayList list=new ArrayList(15);
			
            if (m_GlobalParentComponent != null)
            {
                if (m_GlobalParentComponent is RibbonControl)
                    list.AddRange(((RibbonControl)m_GlobalParentComponent).RibbonStrip.GetItems(ItemName, itemType));
                else if (m_GlobalParentComponent is DotNetBarManager)
                    list.AddRange(((DotNetBarManager)m_GlobalParentComponent).GetItems(ItemName, itemType));
            }
            else
            {
                BarFunctions.GetSubItemsByNameAndType(m_ItemContainer, ItemName, list, itemType);
            }

			return list;
		}

        /// <summary>
        /// Returns the collection of items with the specified name and type. This member is not implemented and should not be used.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <param name="useGlobalName">Indicates whether GlobalName property is used for searching.</param>
        /// <returns></returns>
        public ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
        {
            ArrayList list = new ArrayList(15);
            
            if (m_GlobalParentComponent != null)
            {
                if (m_GlobalParentComponent is RibbonControl)
                    list.AddRange(((RibbonControl)m_GlobalParentComponent).RibbonStrip.GetItems(ItemName, itemType, useGlobalName));
                else if (m_GlobalParentComponent is DotNetBarManager)
                    list.AddRange(((DotNetBarManager)m_GlobalParentComponent).GetItems(ItemName, itemType, false, useGlobalName));
            }
            else
            {
                BarFunctions.GetSubItemsByNameAndType(m_ItemContainer, ItemName, list, itemType, useGlobalName);
            }

            return list;
        }

		/// <summary>
		/// Returns AutoHidePanel that bar is on if in auto-hide state otherwise returns null.
		/// </summary>
		/// <returns>AutoHidePanel object or null if bar is not in auto-hide state.</returns>
		public AutoHidePanel GetAutoHidePanel()
		{
			if(!this.AutoHide)
				return null;

			return GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
		}

		private AutoHidePanel GetAutoHidePanel(DockStyle dock)
		{
			IOwnerAutoHideSupport ownerAutoHide=m_Owner as IOwnerAutoHideSupport;
			if(ownerAutoHide==null)
				return null;

			AutoHidePanel panel=null;
			switch(m_LastDockSiteInfo.DockSide)
			{
				case DockStyle.Left:
				{
					panel=ownerAutoHide.LeftAutoHidePanel;
					break;
				}
				case DockStyle.Right:
				{
					panel=ownerAutoHide.RightAutoHidePanel;
					break;
				}
				case DockStyle.Top:
				{
					panel=ownerAutoHide.TopAutoHidePanel;
					break;
				}
				default:
				{
					panel=ownerAutoHide.BottomAutoHidePanel;
					break;
				}
			}
            
            if (this.AntiAlias) panel.AntiAlias = true;
            panel.Style = this.Style;

			return panel;
		}
		private Size GetLargestMinSize()
		{
			Size ret=Size.Empty;
			foreach(BaseItem item in m_ItemContainer.SubItems)
			{
				if(item is DockContainerItem)
				{
					DockContainerItem dock=item as DockContainerItem;
					if(dock.MinimumSize.Width>ret.Width)
						ret.Width=dock.MinimumSize.Width;
					if(dock.MinimumSize.Height>ret.Height)
						ret.Height=dock.MinimumSize.Height;
				}
			}
			return ret;
		}
		private Control GetOwnerControl()
		{
			Control parentControl=null;
			IOwner owner=m_Owner as IOwner;
			if(owner is DotNetBarManager && ((DotNetBarManager)owner).TopDockSite!=null)
				parentControl=((DotNetBarManager)owner).TopDockSite.Parent;
			else if(owner!=null)
				parentControl=owner.ParentForm;
			return parentControl;
		}

		private void AutoHideStateChanged()
		{
			IOwner owner=m_Owner as IOwner;
			IOwnerAutoHideSupport ownerAutoHide=m_Owner as IOwnerAutoHideSupport;
			if(owner==null)
				return;

			Control parentControl=GetOwnerControl();
			            
			if(parentControl==null)
				return;

			if(m_AutoHideState)
			{
				if(m_BarState==eBarState.AutoHide)
					return;
				if(m_TabDockItems!=null)
					m_TabDockItems.Visible=false;
				// Remember last docking info
                DockSiteInfo tempInfo = m_LastDockSiteInfo;
				m_LastDockSiteInfo=new DockSiteInfo();
                // Preserve the relative last docked to bar in case the return to same docking position is needed
                m_LastDockSiteInfo.LastRelativeDocumentId = tempInfo.LastRelativeDocumentId;
                m_LastDockSiteInfo.LastRelativeDockToBar = tempInfo.LastRelativeDockToBar;

				if(!m_BarDefinitionLoading)
				{
					if(this.Height>0)
						m_LastDockSiteInfo.DockedHeight=this.Height;
					else if(m_ItemContainer!=null && m_ItemContainer.MinHeight>0)
						m_LastDockSiteInfo.DockedHeight=m_ItemContainer.MinHeight+22;
					if(this.Width>0)
						m_LastDockSiteInfo.DockedWidth=this.Width;
					else if(m_ItemContainer!=null && m_ItemContainer.MinWidth>0)
						m_LastDockSiteInfo.DockedWidth=m_ItemContainer.MinWidth+22;
					
					// This will force proper recalculation in SetAutoHideSize procedure
					System.Drawing.Size minSize=GetLargestMinSize();
					if(minSize.Width>m_LastDockSiteInfo.DockedWidth)
						m_LastDockSiteInfo.DockedWidth=0;
					if(minSize.Height>m_LastDockSiteInfo.DockedHeight)
						m_LastDockSiteInfo.DockedHeight=0;
				}

				m_LastDockSiteInfo.DockLine=this.DockLine;
				m_LastDockSiteInfo.DockOffset=this.DockOffset;
				if(this.Parent!=null && m_BarState==eBarState.Docked)
					m_LastDockSiteInfo.DockSide=this.Parent.Dock;
				else
					m_LastDockSiteInfo.DockSide=DockStyle.Left;
				if(this.Parent!=null && this.Parent is DockSite)
				{
					m_LastDockSiteInfo.InsertPosition=((DockSite)this.Parent).Controls.GetChildIndex(this);
					m_LastDockSiteInfo.objDockSite=(DockSite)this.DockedSite;
				}

				// Undock the window
				m_BarState=eBarState.AutoHide;
				ResetActiveControl();
                parentControl.SuspendLayout();
                try
                {
                    // Check for parent since if bar is deserialized there is no parent and state is Docked by default
                    base.Visible = false;
                    if (this.Parent != null && this.Parent is DockSite)
                    {
                        DockSite ds = this.Parent as DockSite;
                        if (ds.DocumentDockContainer != null)
                        {
                            // Remember the split view docking if it was in effect
                            if (ds.Dock != DockStyle.Fill)
                            {
                                DocumentBarContainer dbr = ds.GetDocumentUIManager().GetDocumentFromBar(this) as DocumentBarContainer;
                                if (dbr != null && dbr.Parent is DocumentDockContainer)
                                {
                                    DocumentDockContainer ddc = dbr.Parent as DocumentDockContainer;
                                    if (ddc.Orientation == eOrientation.Horizontal && (ds.Dock == DockStyle.Top || ds.Dock == DockStyle.Bottom) ||
                                        ddc.Orientation == eOrientation.Vertical && (ds.Dock == DockStyle.Left || ds.Dock == DockStyle.Right) && ddc.Documents.Count>1)
                                    {
                                        for (int i = 0; i < ddc.Documents.Count; i++)
                                        {
                                            if (ddc.Documents[i] is DocumentBarContainer && ddc.Documents[i].Visible && ddc.Documents[i] != dbr)
                                            {
                                                m_LastDockSiteInfo.MouseOverBar = ((DocumentBarContainer)ddc.Documents[i]).Bar;
                                                if (i < ddc.Documents.IndexOf(dbr))
                                                    m_LastDockSiteInfo.MouseOverDockSide = (ddc.Orientation == eOrientation.Horizontal?eDockSide.Left:eDockSide.Bottom);
                                                else
                                                    m_LastDockSiteInfo.MouseOverDockSide = (ddc.Orientation == eOrientation.Horizontal ? eDockSide.Right : eDockSide.Top);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            ds.GetDocumentUIManager().UnDock(this);
                        }
                        else
                            ((DockSite)this.Parent).RemoveBar(this);
                    }
                    this.Parent = null;

                    parentControl.Controls.Add(this);
                    parentControl.Controls.SetChildIndex(this, 0);
                    AutoHidePanel panel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
                    panel.AddBar(this);

                    if (!m_BarDefinitionLoading)
                        SetAutoHideSize();
                    if (!m_IgnoreAnimation && m_AutoHideAnimationTime != 0)
                        this.Visible = true;
                }
                finally
                {
                    parentControl.ResumeLayout();
                }
				//this.RecalcSize();
				if(!m_BarDefinitionLoading)
				{
					this.Update();
					AnimateHide();
				}
			}
			else
			{
                AutoHidePanel panel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
                panel.RemoveBar(this);
                if (m_LastDockSiteInfo.objDockSite != null)
                {
                    if (m_LastDockSiteInfo.MouseOverBar != null)
                    {
                        if (m_LastDockSiteInfo.objDockSite.GetDocumentUIManager() == null ||
                            m_LastDockSiteInfo.objDockSite.GetDocumentUIManager().GetDocumentFromBar(m_LastDockSiteInfo.MouseOverBar) == null)
                        {
                            m_LastDockSiteInfo.MouseOverBar = null;
                            m_LastDockSiteInfo.MouseOverDockSide = eDockSide.None;
                            m_LastDockSiteInfo.NewLine = true;
                        }
                    }
                }
                else
                {
                    m_LastDockSiteInfo.MouseOverBar = null;
                    m_LastDockSiteInfo.MouseOverDockSide = eDockSide.None;
                    m_LastDockSiteInfo.NewLine = true;
                }

                //m_LastDockSiteInfo.NewLine = true;
                DockingHandler(m_LastDockSiteInfo, Point.Empty);
                if (!this.IsVisible)
                    this.Visible = true;
				this.RefreshDockTab(true);
				this.ResizeDockTab();
                m_ItemContainer.MinHeight = 0;
                m_ItemContainer.MinWidth = 0;
			}
		}

		internal void SetAutoHideSize()
		{
			IOwner owner=m_Owner as IOwner;
			IOwnerBarSupport ownerBar=m_Owner as IOwnerBarSupport;
			IOwnerAutoHideSupport ownerAutoHide=m_Owner as IOwnerAutoHideSupport;
			Control parentControl=this.GetOwnerControl();

			if(owner==null || parentControl==null || ownerBar==null || ownerAutoHide==null)
				return;

			int width=parentControl.ClientSize.Width;
			int height=parentControl.ClientSize.Height;
			int x=0,y=0;

			//if(ownerBar.TopDockSite.Parent is UserControl)
			{
				switch(m_LastDockSiteInfo.DockSide)
				{
					case DockStyle.Left:
					{
						width=ownerBar.LeftDockSite.Parent.Width;
						height=ownerBar.LeftDockSite.Parent.Height;
						Point pLoc=ownerAutoHide.LeftAutoHidePanel.Location;
						x=pLoc.X+ownerAutoHide.LeftAutoHidePanel.Width;
						y=pLoc.Y;
						break;
					}
					case DockStyle.Right:
					{
						width=ownerBar.RightDockSite.Parent.Width;
						height=ownerBar.RightDockSite.Parent.Height;
						Point pLoc=ownerAutoHide.RightAutoHidePanel.Location;
						x=pLoc.X;
						y=pLoc.Y;
						break;
					}
					case DockStyle.Top:
					{
						width=ownerBar.TopDockSite.Parent.Width;
						height=ownerBar.TopDockSite.Parent.Height;
						Point pLoc=ownerAutoHide.TopAutoHidePanel.Location;
						x=pLoc.X;
						y=pLoc.Y+ownerAutoHide.TopAutoHidePanel.Height;
						break;
					}
					default:
					{
						width=ownerBar.BottomDockSite.Parent.Width;
						height=ownerBar.BottomDockSite.Parent.Height;
						Point pLoc=ownerAutoHide.BottomAutoHidePanel.Location;
						x=pLoc.X;
						y=pLoc.Y;
						break;
					}
				}
			}

			switch(m_LastDockSiteInfo.DockSide)
			{
				case DockStyle.Left:
				{
//					if(ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.TopDockSite)>ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.LeftDockSite))
//					{
//						height-=ownerBar.TopDockSite.Height;
//						y=ownerBar.TopDockSite.Bottom;
//					}
//					if(ownerAutoHide.HasBottomAutoHidePanel)
//						height-=ownerAutoHide.BottomAutoHidePanel.Height;
//					if(ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.BottomDockSite)>ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.LeftDockSite))
//						height-=ownerBar.BottomDockSite.Height;
					height=ownerAutoHide.LeftAutoHidePanel.Height;
					width=m_LastDockSiteInfo.DockedWidth;
					if(width==0)
					{
						int index=-1;
						if(this.SelectedDockTab>=0 && this.Items[this.SelectedDockTab] is DockContainerItem)
							index=this.SelectedDockTab;
						else if(this.Items.Count>0 && this.Items[0] is DockContainerItem)
							index=0;
						if(index>=0)
							width=Math.Max(((DockContainerItem)this.Items[index]).MinimumSize.Width,((DockContainerItem)this.Items[index]).Width);
						this.EnableRedraw=false;
						try
						{
							this.Size=new Size(width,height);
							this.RecalcSize();
						}
						finally
						{
							this.EnableRedraw=true;
						}
						width=this.Width;
						m_LastDockSiteInfo.DockedWidth=width;
					}
//					if(!(ownerBar.TopDockSite.Parent is UserControl))
//					{
//						x=ownerAutoHide.LeftAutoHidePanel.Right;
//					}
//					else
//						y+=ownerBar.TopDockSite.Top;
					break;
				}
				case DockStyle.Right:
				{
//					if(ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.TopDockSite)>ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.RightDockSite))
//					{
//						height-=ownerBar.TopDockSite.Height;
//						y=ownerBar.TopDockSite.Bottom;
//					}
//					if(ownerAutoHide.HasBottomAutoHidePanel)
//						height-=ownerAutoHide.BottomAutoHidePanel.Height;
//					if(ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.BottomDockSite)>ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.RightDockSite))
//						height-=ownerBar.BottomDockSite.Height;
					height=ownerAutoHide.RightAutoHidePanel.Height;
					width=m_LastDockSiteInfo.DockedWidth;
					if(width==0)
					{
						int index=-1;
						if(this.SelectedDockTab>=0 && this.Items[this.SelectedDockTab] is DockContainerItem)
							index=this.SelectedDockTab;
						else if(this.Items.Count>0 && this.Items[0] is DockContainerItem)
							index=0;
						if(index>=0)
							width=Math.Max(((DockContainerItem)this.Items[index]).MinimumSize.Width,((DockContainerItem)this.Items[index]).Width);
						this.EnableRedraw=false;
						try
						{
							this.Size=new Size(width,height);
							this.RecalcSize();
						}
						finally
						{
							this.EnableRedraw=true;
						}
						width=this.Width;
						m_LastDockSiteInfo.DockedWidth=width;
					}
//					if(!(ownerBar.TopDockSite.Parent is UserControl))
//						x=ownerAutoHide.RightAutoHidePanel.Left-width;
//					else
//					{
						x-=width;
                        //y+=ownerBar.TopDockSite.Top;
					//}
					break;
				}
				case DockStyle.Top:
				{
//					if(ownerAutoHide.HasLeftAutoHidePanel)
//					{
//						width-=ownerAutoHide.LeftAutoHidePanel.Width;
//						x+=ownerAutoHide.LeftAutoHidePanel.Width;
//					}
//					if(ownerAutoHide.HasRightAutoHidePanel)
//					{
//						width-=ownerAutoHide.RightAutoHidePanel.Width;
//					}
//					if(ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.TopDockSite)<ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.RightDockSite))
//					{
//						width-=ownerBar.RightDockSite.Width;
//					}
//					if(ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.TopDockSite)<ownerBar.TopDockSite.Parent.Controls.IndexOf(ownerBar.LeftDockSite))
//					{
//						width-=ownerBar.LeftDockSite.Width;
//						x+=ownerBar.LeftDockSite.Width;
//					}
					width=ownerAutoHide.TopAutoHidePanel.Width;
					height=m_LastDockSiteInfo.DockedHeight;
					if(height==0)
					{
						int index=-1;
						if(this.SelectedDockTab>=0 && this.Items[this.SelectedDockTab] is DockContainerItem)
							index=this.SelectedDockTab;
						else if(this.Items.Count>0 && this.Items[0] is DockContainerItem)
							index=0;
						if(index>=0)
							height=Math.Max(((DockContainerItem)this.Items[index]).MinimumSize.Height,((DockContainerItem)this.Items[index]).Height);

						this.EnableRedraw=false;
						try
						{
							this.Size=new Size(width,height);
							this.RecalcSize();
						}
						finally
						{
							this.EnableRedraw=true;
						}
						height=this.Height;
						m_LastDockSiteInfo.DockedHeight=height;
					}

//					if(!(ownerBar.TopDockSite.Parent is UserControl))
//						y=ownerAutoHide.TopAutoHidePanel.Bottom;
						
					break;
				}
				default:
				{
//					if(ownerAutoHide.HasLeftAutoHidePanel)
//					{
//						width-=ownerAutoHide.LeftAutoHidePanel.Width;
//						x+=ownerAutoHide.LeftAutoHidePanel.Width;
//					}
//					if(ownerAutoHide.HasRightAutoHidePanel)
//						width-=ownerAutoHide.RightAutoHidePanel.Width;
//					if(ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.BottomDockSite)<ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.RightDockSite))
//					{
//						width-=ownerBar.RightDockSite.Width;
//					}
//					if(ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.BottomDockSite)<ownerBar.BottomDockSite.Parent.Controls.IndexOf(ownerBar.LeftDockSite))
//					{
//						width-=ownerBar.LeftDockSite.Width;
//						x+=ownerBar.LeftDockSite.Width;
//					}
					width=ownerAutoHide.BottomAutoHidePanel.Width;
					height=m_LastDockSiteInfo.DockedHeight;
					if(height==0)
					{
						int index=-1;
						if(this.SelectedDockTab>=0 && this.Items[this.SelectedDockTab] is DockContainerItem)
							index=this.SelectedDockTab;
						else if(this.Items.Count>0 && this.Items[0] is DockContainerItem)
							index=0;
						if(index>=0)
							height=Math.Max(((DockContainerItem)this.Items[index]).MinimumSize.Height,((DockContainerItem)this.Items[index]).Height);

						this.EnableRedraw=false;
						try
						{
							this.Size=new Size(width,height);
							this.RecalcSize();
						}
						finally
						{
							this.EnableRedraw=true;
						}
						height=this.Height;
                        
						m_LastDockSiteInfo.DockedHeight=height;
                        if (m_TabDockItems != null) m_LastDockSiteInfo.DockedHeight += m_TabDockItems.Height;
					}
					y-=height;
//					if(!(ownerBar.TopDockSite.Parent is UserControl))
//						y=ownerAutoHide.BottomAutoHidePanel.Top-height;
//					else if(this.Parent!=null)
//					{
//						Point p=ownerAutoHide.BottomAutoHidePanel.PointToScreen(Point.Empty);
//						p=this.Parent.PointToClient(p);
//						y=p.Y-height;
//					}
					break;
				}
			}

			Rectangle r=new Rectangle(x,y,width,height);
			AutoHideDisplayEventArgs eventArgs=new AutoHideDisplayEventArgs(r);
			if(AutoHideDisplay!=null)
			{
				AutoHideDisplay(this,eventArgs);
				r=eventArgs.DisplayRectangle;
			}
			if(ownerBar!=null)
			{
				ownerBar.InvokeAutoHideDisplay(this,eventArgs);
				r=eventArgs.DisplayRectangle;
			}
			this.Bounds=r;
			//this.Size=r.Size; //new Size(width,height);
			//this.Location=r.Location; //new Point(x,y);
			this.RecalcSize();
			
			if(m_LastDockSiteInfo.DockSide==DockStyle.Bottom)
			{
				if(this.Height!=height)
					this.Top+=(height-this.Height);
			}
			else if(m_LastDockSiteInfo.DockSide==DockStyle.Right)
			{
				if(this.Width!=width)
					this.Left+=(width-this.Width);
			}
			
		}

		/// <summary>
		/// Gets or sets how long it takes to play the auto-hide animation, in milliseconds. Maximum value is 2000, 0 disables animation.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(100),Category("Auto-Hide"),Description("Specifies how long it takes to play the auto-hide animation, in milliseconds. Maximum value is 2000, 0 disables animation.")]
		public virtual int AutoHideAnimationTime
		{
			get {return m_AutoHideAnimationTime;}
			set
			{
				if(value<0)
					value=0;
				if(value>2000)
					value=2000;
				m_AutoHideAnimationTime=value;
			}
		}

		internal bool AnimateShow()
		{
			if(this.Visible)
                return false;

		    CancelEventArgs e = new CancelEventArgs();
		    OnBeforeAutoHideDisplayed(e);
            if (e.Cancel) return false;

			SetAutoHideSize();
			Rectangle rectEnd=new Rectangle(this.Location,this.Size);
			Rectangle rectStart=rectEnd;
			this.Parent.Controls.SetChildIndex(this,0);
			if(!m_IgnoreAnimation && m_AutoHideAnimationTime>0  /*&& BarFunctions.SupportsAnimation*/)
			{
				switch(m_LastDockSiteInfo.DockSide)
				{
					case DockStyle.Left:
						rectStart.Width=1;
						break;
					case DockStyle.Right:
						rectStart.X=rectStart.Right-1;
						rectStart.Width=1;
						break;
					case DockStyle.Top:
						rectStart.Height=1;
						break;
					default:
						rectStart.Y=rectStart.Bottom-1;
						rectStart.Height=1;
						break;
				}
			}
			try
			{
				m_AnimationInProgress=true;
				BarFunctions.AnimateControl(this,true,m_AutoHideAnimationTime,rectStart,rectEnd);
			}
			finally
			{
				m_AnimationInProgress=false;
			}

			m_IgnoreAnimation=false;

            return true;
		}
		internal bool AnimateHide()
		{
			if(!this.Visible)
				return true;

		    CancelEventArgs e = new CancelEventArgs();
		    OnBeforeAutoHideHidden(e);
            if (e.Cancel) return false;

			this.Parent.Controls.SetChildIndex(this,0);
			Rectangle rectStart=new Rectangle(this.Location,this.Size);
			Rectangle rectEnd=rectStart;

			if(!m_IgnoreAnimation && m_AutoHideAnimationTime>0 /*&& BarFunctions.SupportsAnimation*/)
			{
				switch(m_LastDockSiteInfo.DockSide)
				{
					case DockStyle.Left:
						rectEnd.Width=1;
						break;
					case DockStyle.Right:
						rectEnd.X=rectEnd.Right-1;
						rectEnd.Width=1;
						break;
					case DockStyle.Top:
						rectEnd.Height=1;
						break;
					default:
						rectEnd.Y=rectEnd.Bottom-1;
						rectEnd.Height=1;
						break;
				}

				try
				{
					m_AnimationInProgress=true;
					BarFunctions.AnimateControl(this,false,m_AutoHideAnimationTime,rectStart,rectEnd);
				}
				finally
				{
					m_AnimationInProgress=false;
				}
			}
			else
				this.Visible=false;
            return true;
		}
        /// <summary>
        /// Occurs before the bar in auto-hide state is displayed on popup and allows you to cancel display by setting Cancel=true on event arguments.
        /// </summary>
        public event CancelEventHandler BeforeAutoHideDisplayed;
        /// <summary>
        /// Raises BeforeAutoHideDisplayed event.
        /// </summary>
        /// <param name="e"></param>
		protected virtual void OnBeforeAutoHideDisplayed(CancelEventArgs e)
		{
		    CancelEventHandler eh = BeforeAutoHideDisplayed;
            if (eh != null)
                eh(this, e);
		}

        /// <summary>
        /// Occurs before the bar in auto-hide state is hidden and allows you to cancel display by setting Cancel=true on event arguments.
        /// </summary>
        public event CancelEventHandler BeforeAutoHideHidden;
        /// <summary>
        /// Raises BeforeAutoHideHidden event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBeforeAutoHideHidden(CancelEventArgs e)
        {
            CancelEventHandler eh = BeforeAutoHideHidden;
            if (eh != null)
                eh(this, e);
        }

        private void ResetActiveControl()
		{
			IOwner owner=m_Owner as IOwner;
			// Must reset the ActiveControl to null becouse on MDI Forms if this was not done
			// MDI form could not be closed if bar that had ActiveControl is floating.
			if(owner.ParentForm!=null && owner.ParentForm.ActiveControl==this)
			{
				owner.ParentForm.ActiveControl=null;
				this.Focus(); // Fixes the problem on SDI forms
			}
			else if(owner.ParentForm!=null && IsAnyControl(this,owner.ParentForm.ActiveControl))
			{
				owner.ParentForm.ActiveControl=null;
				this.Focus();
			}
		}

		private bool ShowCustomizeMenuButton
		{
			get
			{
				// We Show customize menu button if there is CustomizeItem in container or when
				// LayoutType is DockingContainer and grab handle style is caption users have the docking container menu
                if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted) && m_CustomizeMenu != null)
					return true;
				if(m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
				{
					if(m_ItemContainer.HaveCustomizeItem)
						return true;
					return (m_CustomizeMenu!=null);
				}
				return false;
			}
		}

		private void SysButtonMouseOverAutoHide(object sender, EventArgs e)
		{
			if(!m_SystemButtons.MouseOverAutoHide)
				this.HideToolTip();
			else
				ResetHover();
		}
		private void SysButtonMouseOverCustomize(object sender, EventArgs e)
		{
			if(!m_SystemButtons.MouseOverCustomize)
				this.HideToolTip();
			else
				ResetHover();
		}
		private void SysButtonMouseOverClose(object sender, EventArgs e)
		{
			if(!m_SystemButtons.MouseOverClose)
				this.HideToolTip();
			else
				ResetHover();
		}
		private void SysButtonHideTooltip(object sender,EventArgs e)
		{
			HideToolTip();
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
				ResetHover();
			}
		}

		internal void HideAllToolTips()
		{
			HideToolTip();
			if(m_ItemContainer!=null)
			{
				foreach(BaseItem item in m_ItemContainer.SubItems)
					this.HideItemToolTips(item);
			}
		}
		private void HideItemToolTips(BaseItem item)
		{
			item.HideToolTip();
			foreach(BaseItem i in item.SubItems)
				this.HideItemToolTips(i);
		}



		/// <summary>
		/// Shows tooltip for this item.
		/// </summary>
		private void ShowToolTip(string tipText)
		{
			if(this.DesignMode || !m_ShowToolTips || tipText==null || tipText=="")
				return;

			if(this.Visible)
			{
				if(m_ToolTipWnd==null)
					m_ToolTipWnd=new ToolTip();
				m_ToolTipWnd.Text=tipText;
				IOwnerItemEvents ownerEvents=this.Owner as IOwnerItemEvents;
				if(ownerEvents!=null)
					ownerEvents.InvokeToolTipShowing(m_ToolTipWnd,new EventArgs());
				m_ToolTipWnd.ShowToolTip();
			}
		}

		/// <summary>
		/// Gets whether tooltip is visible or not.
		/// </summary>
		private bool ToolTipVisible
		{
			get
			{
				return (m_ToolTipWnd!=null);
			}
		}

		private void UpdateDockTabSettings()
		{
			if(m_TabDockItems==null || !BarFunctions.IsHandleValid(this))
				return;

			if(m_TabDockItems.CanReorderTabs!=m_CanReorderTabs)
				m_TabDockItems.CanReorderTabs=m_CanReorderTabs;
			if(m_TabDockItems.TabAlignment!=m_DockTabAlignment)
				m_TabDockItems.TabAlignment=m_DockTabAlignment;

			if(m_TabNavigation)
				m_TabDockItems.TabLayoutType=eTabLayoutType.FixedWithNavigationBox;
			else
				m_TabDockItems.TabLayoutType=eTabLayoutType.FitContainer;

			m_TabDockItems.ShowFocusRectangle=false;

			if(m_TabDockItems._TabSystemBox!=null)
				m_TabDockItems._TabSystemBox.CloseVisible=this.CanHideResolved;

            if (m_AutoHideState)
                m_TabDockItems.Visible = false;
		}

		internal void CreateDockTab()
		{
			if(m_TabDockItems!=null || !BarFunctions.IsHandleValid(this))
				return;
			m_TabDockItems=new TabStrip();
			UpdateDockTabSettings();

			this.SuspendLayout();
			this.ResizeDockTab();
			m_TabDockItems.SelectedTabChanging+=new TabStrip.SelectedTabChangingEventHandler(this.TabStripTabChanging);
            m_TabDockItems.SelectedTabChanged += new TabStrip.SelectedTabChangedEventHandler(TabDockItemsSelectedTabChanged);
			m_TabDockItems.BeforeTabDisplay+=new EventHandler(this.BeforeTabDisplay);
			m_TabDockItems.TabMoved+=new TabStrip.TabMovedEventHandler(this.TabStripTabMoved);
            m_TabDockItems.TabItemClose += new TabStrip.UserActionEventHandler(TabStripTabItemClose);
            m_TabDockItems.CloseButtonOnTabsVisible = m_DockTabCloseButtonVisible;
			this.Controls.Add(m_TabDockItems);

			SetDockTabStyle(this.Style);

			m_TabDockItems.Visible=GetDockTabVisible();
			RefreshDockTabItems();
			this.Invalidate();
			this.ResumeLayout();
		}

        private void TabDockItemsSelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            UpdateCloseButtonVisibility();
        }

        private void TabStripTabItemClose(object sender, TabStripActionEventArgs e)
        {
            TabItem ti = sender as TabItem;
            if (ti != null)
            {
                DockContainerItem di = ti.AttachedItem as DockContainerItem;
                if (di != null)
                    CloseDockTab(di, eEventSource.Mouse);
            }
        }

		internal void SetDockTabStyle(eDotNetBarStyle style)
		{
			if(m_TabDockItems==null)
				return;
			if(this.Style==eDotNetBarStyle.Office2003)
			{
				IOwnerBarSupport ob=m_Owner as IOwnerBarSupport;
				if(this.DockSide==eDockSide.Document && ob!=null && ob.ApplyDocumentBarStyle)
					m_TabDockItems.Style=eTabStripStyle.OneNote;
				else
					m_TabDockItems.Style=eTabStripStyle.Office2003;
			}
            else if (this.Style == eDotNetBarStyle.VS2005)
			{
				IOwnerBarSupport ob=m_Owner as IOwnerBarSupport;
				if(this.DockSide==eDockSide.Document && ob!=null && ob.ApplyDocumentBarStyle)
					m_TabDockItems.Style=eTabStripStyle.VS2005Document;
				else
					m_TabDockItems.Style=eTabStripStyle.VS2005;
			}
            else if (BarFunctions.IsOffice2007Style(this.Style))
            {
                IOwnerBarSupport ob = m_Owner as IOwnerBarSupport;
                if (this.DockSide == eDockSide.Document && ob != null && ob.ApplyDocumentBarStyle)
                    m_TabDockItems.Style = eTabStripStyle.Office2007Document;
                else
                    m_TabDockItems.Style = eTabStripStyle.Office2007Dock;
            }
            else
                m_TabDockItems.Style = eTabStripStyle.Flat;
			m_TabDockItems.ColorScheme.TabBorder=Color.Empty; // No border

			if(TabStripStyleChanged!=null)
				TabStripStyleChanged(this, new EventArgs());
		}

		private void BeforeTabDisplay(object sender, EventArgs e)
		{
			if(sender is TabItem)
			{
				InvokeBeforeDockTabDisplayed(((TabItem)sender).AttachedItem);
			}
		}

		private void DestroyDockTab()
		{
			if(m_TabDockItems==null)
				return;
			if(m_TabDockItems.IsDraggingBar)
			{
				//if(m_TabDockItems.Visible)
					m_TabDockItems.Visible=false;
                    m_TabDockItems.Tabs.Clear();
				return;
			}
			this.Controls.Remove(m_TabDockItems);
			m_TabDockItems.Dispose();
			m_TabDockItems=null;
		}

		internal void RefreshTabStrip()
		{
			if(m_TabDockItems!=null)
				m_TabDockItems.Refresh();
		}

        internal void RefreshDockTab(bool bFireEvents)
        {
            if (m_ItemContainer == null || !BarFunctions.IsHandleValid(this))
                return;

            SyncBarCaption();

            int iVisibleItems = m_ItemContainer.VisibleSubItems;

            if (iVisibleItems > 1 || m_AlwaysDisplayDockTab && m_BarState != eBarState.Floating)
            {
                if (m_TabDockItems == null)
                {
                    CreateDockTab();
                    return;
                }
                else if (!m_TabDockItems.Visible && m_BarState != eBarState.AutoHide)
                    m_TabDockItems.Visible = GetDockTabVisible();
            }
            else
            {
                if (iVisibleItems == 1 && m_TabDockItems != null)
                {
                    // Fire last DockTabChange Event
                    DockContainerItem item = null;
                    foreach (BaseItem baseItem in m_ItemContainer.SubItems)
                    {
                        if (baseItem.Visible && baseItem is DockContainerItem)
                        {
                            item = baseItem as DockContainerItem;
                            break;
                        }
                    }
                    if (item != null)
                    {
                        if (bFireEvents)
                            InvokeDockTabChange(null, item);
                    }
                }
                if (this.DesignMode)
                {
                    if (m_TabDockItems != null)
                        m_TabDockItems.Visible = false;
                }
                else
                    DestroyDockTab();
                return;
            }

            BaseItem oldItem = null;
            if (bFireEvents && m_TabDockItems != null && m_TabDockItems.SelectedTab != null)
                oldItem = m_TabDockItems.SelectedTab.AttachedItem;

            RefreshDockTabItems();

            if (bFireEvents && m_TabDockItems != null && m_TabDockItems.SelectedTab != null && oldItem != m_TabDockItems.SelectedTab.AttachedItem)
                InvokeDockTabChange(oldItem, m_TabDockItems.SelectedTab.AttachedItem);
        }

		internal void RefreshAutoHidePanel()
		{
			if(this.AutoHide)
			{
				AutoHidePanel panel=GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
				if(panel!=null)
				{
					panel.RefreshBar(this);
					if(panel.SelectedDockContainerItem!=null)
					{
						InvokeDockTabChange(null,panel.SelectedDockContainerItem);
					}
				}
			}
		}

		internal void OnDockContainerVisibleChanged(DockContainerItem dockItem)
		{
			if(!this.AutoHide || dockItem.Visible)
				return;
			bool bDisplayed=true;
			foreach(BaseItem item in this.Items)
			{
				if(item.Visible && bDisplayed)
				{
					item.Displayed=true;
					bDisplayed=false;
				}
				else
					item.Displayed=false;
			}
		}

        /// <summary>
        /// Re-docks the floating bar to its previous docking position.
        /// </summary>
        public void ReDock()
        {

            if (m_BarState == eBarState.Floating && (this.CanDockBottom || this.CanDockTop || this.CanDockLeft || this.CanDockRight || this.CanDockDocument))
            {
                DockingHandler(m_LastDockSiteInfo, Point.Empty);
            }
        }

		internal void OnSubItemRemoved(BaseItem objItem)
		{
			if(objItem is DockContainerItem)
			{
				if(this.LayoutType==eLayoutType.DockContainer)
				{
					if(objItem.Displayed)
					{
						bool bOneDisp=false;
						foreach(BaseItem item in this.Items)
						{
							if(item.Displayed)
								bOneDisp=true;
						}
						if(!bOneDisp && this.Items.Count>0)
							this.Items[0].Displayed=true;
					}
					this.RefreshDockTab(true);
					if(this.AutoHide)
					{
						AutoHidePanel panel=this.GetAutoHidePanel();
						if(panel!=null)
							panel.RefreshBar(this);
					}
				}
			}
			SyncBarCaption();
		}

		/// <summary>
		/// Invokes CaptionButtonClick event.
		/// </summary>
		protected void InvokeCaptionButtonClick()
		{
			if(CaptionButtonClick!=null)
				CaptionButtonClick(this,new EventArgs());
		}

		/// <summary>
		/// Displays or hides the automatic caption button popup menu.
		/// </summary>
		private void ToggleCaptionMenu()
		{
			if(m_CaptionMenu!=null)
			{
				bool bClose=m_CaptionMenu.Expanded;
				if(bClose)
					m_CaptionMenu.ClosePopup();
				m_CaptionMenu.Dispose();
				m_CaptionMenu=null;
				if(bClose)
					return;
			}

			if(this.Items.Count<=0)
				return;

			m_CaptionMenu=new ButtonItem("sysCaptionButtonMenuParent");
			m_CaptionMenu.Style=this.Style;
			m_CaptionMenu.PopupShowing+=new EventHandler(this.CaptionMenuShowing);
			m_CaptionMenu.PopupFinalized+=new EventHandler(this.CaptionMenuClose);
			foreach(BaseItem item in this.Items)
			{
				ButtonItem menuItem=new ButtonItem("sysCaption_"+item.Name);
				menuItem.Tag=item;
				menuItem.OptionGroup="sysCaptionMenu";
				menuItem.Text=item.Text;
				menuItem.BeginGroup=item.BeginGroup;
				if(item is DockContainerItem)
				{
					DockContainerItem dock=item as DockContainerItem;
					menuItem.Checked=item.Displayed;
					if(dock.Image!=null)
						menuItem.Image=dock.Image.Clone() as System.Drawing.Image;
					else if(dock.Icon!=null)
						menuItem.Icon=dock.Icon.Clone() as System.Drawing.Icon;
					else if(dock.ImageIndex>=0 && dock.ImageList!=null)
						menuItem.Image=dock.ImageList.Images[dock.ImageIndex].Clone() as System.Drawing.Image;
				}
				else
					menuItem.Checked=item.Visible;
				menuItem.Click+=new EventHandler(this.CaptionMenuItemClick);
				m_CaptionMenu.SubItems.Add(menuItem);
			}

			if(m_CaptionMenu.GetOwner()==null)
				m_CaptionMenu.SetOwner(m_Owner);
			System.Drawing.Size size=m_CaptionMenu.PopupSize;
			Point popupLocation=new Point(m_SystemButtons.CaptionButtonRect.Right-size.Width,m_SystemButtons.CaptionButtonRect.Bottom);
			if(popupLocation.X<0)
				popupLocation.X=0;
			popupLocation=this.PointToScreen(popupLocation);
			m_CaptionMenu.Popup(popupLocation);
		}
		private void CaptionMenuItemClick(object sender, EventArgs e)
		{
			ButtonItem menuItem=sender as ButtonItem;
			if(menuItem==null)
				return;
			if(this.LayoutType==eLayoutType.DockContainer)
				this.SelectedDockTab=this.Items.IndexOf(menuItem.Tag as BaseItem);
			else
			{
				foreach(BaseItem item in this.Items)
				{
					if(item==menuItem.Tag)
						item.Visible=true;
					else
						item.Visible=false;
				}
				this.RecalcLayout();
				m_CaptionMenu.Dispose();
				m_CaptionMenu=null;
			}
		}
		private void CaptionMenuShowing(object sender, EventArgs e)
		{
			if(m_CaptionMenu!=null)
				((MenuPanel)m_CaptionMenu.PopupControl).ColorScheme=this.GetColorScheme();
		}
		private void CaptionMenuClose(object sender, EventArgs e)
		{
			PaintCaptionButton();
		}

		internal void RefreshDockContainerItem(DockContainerItem item)
		{
            if (m_ItemContainer == null || m_TabDockItems == null)
            {
                SyncBarCaption();
                return;
            }

			foreach(TabItem tab in m_TabDockItems.Tabs)
			{
				if(tab.AttachedItem==item)
				{
					tab.Text=item.Text;
					tab.AttachedItem=item;
                    tab.Tooltip = item.Tooltip;
                    tab.Name = item.Name;
					if(item.Icon!=null)
						tab.Icon=item.Icon.Clone() as Icon;
					else
					{
						tab.Image=item.TabImage;
						tab.Icon=null;
					}
					break;
				}
			}
			m_TabDockItems.Refresh();
			if(this.AutoHide)
			{
				AutoHidePanel panel=GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
				if(panel!=null)
					panel.Refresh();
			}
			SyncBarCaption();
		}

		private bool GetDockTabVisible()
		{
			if(m_GrabHandleStyle==eGrabHandleStyle.CaptionTaskPane && m_AutoCreateCaptionMenu)
				return false;
			return true;
		}

		private bool m_RefreshingDockTab=false; // Used to preven re-entrancy
		private void RefreshDockTabItems()
		{
			if(m_RefreshingDockTab)
				return;
			if(!m_TabDockItems.Visible && m_BarState!=eBarState.AutoHide)
			{
				m_TabDockItems.Visible=GetDockTabVisible();
				if(this.IsThemed && m_BarState==eBarState.Floating)
				{
					m_TabDockItems.Size=new Size(this.Width-(m_ThemeWindowMargins.Left+m_ThemeWindowMargins.Right),DOCKTABSTRIP_HEIGHT);
					m_TabDockItems.Location=new Point(m_ThemeWindowMargins.Left,this.Height-DOCKTABSTRIP_HEIGHT-m_ThemeWindowMargins.Bottom);
				}
				else
				{
					m_TabDockItems.Size=new Size(this.Width-4,DOCKTABSTRIP_HEIGHT);
					m_TabDockItems.Location=new Point(2,this.Height-DOCKTABSTRIP_HEIGHT-2);
				}
			}
			m_RefreshingDockTab=true;
			try
			{
				m_TabDockItems.Tabs.Clear();
				int iCount=0;
				foreach(BaseItem item in m_ItemContainer.SubItems)
				{
					if(item.Visible)
					{
						iCount++;
						TabItem tab=new TabItem();
						tab.Text=item.Text;
                        tab.Tooltip = item.Tooltip;
                        tab.Name = item.Name;
						if(item is DockContainerItem)
						{
                            DockContainerItem di = item as DockContainerItem;
                            if (di.Icon != null)
                                tab.Icon = di.Icon.Clone() as Icon;
							else
                                tab.Image = di.TabImage;
                            tab.PredefinedColor = di.PredefinedTabColor;
						}
						m_TabDockItems.Tabs.Add(tab);
						tab.AttachedItem=item;
						if(item.Displayed)
						{
							m_TabDockItems._IgnoreBeforeTabDisplayEvent=true;
							m_TabDockItems.SelectedTab=tab;
							m_TabDockItems._IgnoreBeforeTabDisplayEvent=false;
						}
					}
				}
				if(iCount>1 || iCount==1 && m_AlwaysDisplayDockTab && m_BarState!=eBarState.Floating)
				{
					if(!m_TabDockItems.Visible && m_BarState!=eBarState.AutoHide)
						m_TabDockItems.Visible=GetDockTabVisible();
                    if (m_TabDockItems.SelectedTab == null)
                        m_TabDockItems.SelectedTab = m_TabDockItems.Tabs[0];
                    else if (m_TabDockItems.SelectedTab.AttachedItem != null && !m_TabDockItems.SelectedTab.AttachedItem.Displayed)
                        m_TabDockItems.SelectedTab.AttachedItem.Displayed = true;
					m_TabDockItems.Refresh();
				}
				else if(m_TabDockItems.Visible)
					m_TabDockItems.Visible=false;
			}
			finally
			{
				m_RefreshingDockTab=false;
			}

			if(m_BarState==eBarState.AutoHide)
			{
				AutoHidePanel panel=GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
				if(panel!=null)
					panel.RefreshBar(this);
			}
		}

		private void ResizeDockTab()
		{
			if(m_TabDockItems!=null && m_TabDockItems.Visible)
			{
				Point p=Point.Empty;
				Size sz=Size.Empty;

				switch(m_DockTabAlignment)
				{
					case eTabStripAlignment.Top:
					{
						if(m_GrabHandleStyle!=eGrabHandleStyle.None && !m_GrabHandleRect.IsEmpty)
						{
							p=new Point(m_GrabHandleRect.X,m_GrabHandleRect.Bottom);
							sz=new Size(this.Width-m_GrabHandleRect.Left*2,DOCKTABSTRIP_HEIGHT);
						}
						else
						{
							if(m_BarState==eBarState.Docked)
							{
								p=new Point(1,1);
								sz=new Size(this.Width-2,DOCKTABSTRIP_HEIGHT);
							}
							else if(m_BarState==eBarState.Floating)
							{
								if(this.IsThemed)
								{
									p=new Point(m_ThemeWindowMargins.Left,m_ThemeWindowMargins.Top);
									sz=new Size(this.Width-(m_ThemeWindowMargins.Left+m_ThemeWindowMargins.Right),DOCKTABSTRIP_HEIGHT);
								}
								else
								{
									p=new Point(3,3);
									sz=new Size(this.Width-6,DOCKTABSTRIP_HEIGHT);
								}
							}
							else
								m_TabDockItems.Visible=false;
						}
						break;
					}
					case eTabStripAlignment.Left:
					{
						if(m_GrabHandleStyle!=eGrabHandleStyle.None && !m_GrabHandleRect.IsEmpty)
						{
							p=new Point(m_GrabHandleRect.X+1,m_GrabHandleRect.Bottom+1);
							sz=new Size(DOCKTABSTRIP_HEIGHT,m_ItemContainer.HeightInternal-1);
						}
						else
						{
							if(m_BarState==eBarState.Docked)
							{
								p=new Point(2,1);
								sz=new Size(DOCKTABSTRIP_HEIGHT,this.Height-3);
							}
							else if(m_BarState==eBarState.Floating)
							{
								if(this.IsThemed)
								{
									p=new Point(m_ThemeWindowMargins.Left,m_ThemeWindowMargins.Top);
									sz=new Size(this.Width-(m_ThemeWindowMargins.Left+m_ThemeWindowMargins.Right),DOCKTABSTRIP_HEIGHT);
								}
								else
								{
									p=new Point(3,3);
									sz=new Size(this.Width-6,DOCKTABSTRIP_HEIGHT);
								}
							}
							else
								m_TabDockItems.Visible=false;
						}
						break;
					}
					case eTabStripAlignment.Right:
					{
						if(m_GrabHandleStyle!=eGrabHandleStyle.None && !m_GrabHandleRect.IsEmpty && m_BarState==eBarState.Docked)
						{
							p=new Point(this.ClientRectangle.Right-DOCKTABSTRIP_HEIGHT,m_GrabHandleRect.Bottom+1);
							sz=new Size(DOCKTABSTRIP_HEIGHT-2,m_ItemContainer.HeightInternal);
						}
						else
						{
							if(m_BarState==eBarState.Docked)
							{
								p=new Point(this.Width-DOCKTABSTRIP_HEIGHT,1);
								sz=new Size(DOCKTABSTRIP_HEIGHT,this.Height-2);
							}
							else if(m_BarState==eBarState.Floating)
							{
								if(this.IsThemed)
								{
									p=new Point(this.Width-m_ThemeWindowMargins.Right-DOCKTABSTRIP_HEIGHT,m_ThemeWindowMargins.Top);
									sz=new Size(DOCKTABSTRIP_HEIGHT,this.Height-(m_ThemeWindowMargins.Top+m_ThemeWindowMargins.Bottom));
								}
								else
								{
									p=new Point(this.Width-3-DOCKTABSTRIP_HEIGHT,m_ItemContainer.TopInternal);
									sz=new Size(DOCKTABSTRIP_HEIGHT,m_ItemContainer.HeightInternal);
								}
							}
							else
								m_TabDockItems.Visible=false;
						}
						break;
					}
					default:
					{
						p=new Point(0,this.Height-DOCKTABSTRIP_HEIGHT);
						sz=new Size(this.Width,DOCKTABSTRIP_HEIGHT);
						if(this.DockSide==eDockSide.Top && m_BarState==eBarState.Docked)
						{
							p=new Point(0,this.Height-DOCKTABSTRIP_HEIGHT-1);
							sz=new Size(this.Width,DOCKTABSTRIP_HEIGHT);
						}
						if(m_BarState==eBarState.Docked)
						{
							if(m_DockedBorder!=eBorderType.None)
							{	
								p=new Point(2,this.Height-DOCKTABSTRIP_HEIGHT-3);
								sz=new Size(this.Width-4,DOCKTABSTRIP_HEIGHT);
							}
						}
						else if(m_BarState==eBarState.Floating)
						{
							if(this.IsThemed)
							{
								p=new Point(m_ThemeWindowMargins.Left,this.Height-DOCKTABSTRIP_HEIGHT-m_ThemeWindowMargins.Right);
								sz=new Size(this.Width-(m_ThemeWindowMargins.Left+m_ThemeWindowMargins.Right),DOCKTABSTRIP_HEIGHT);
							}
							else
							{
								p=new Point(3,this.Height-DOCKTABSTRIP_HEIGHT-3);
								sz=new Size(this.Width-6,DOCKTABSTRIP_HEIGHT);
							}
						}
						else
							m_TabDockItems.Visible=false;
						break;
					}
				}
				m_TabDockItems.SetBounds(p.X,p.Y,sz.Width,sz.Height);
			}
		}

		/// <summary>
		/// Returns the reference to internal TabStrip control used to display contained DockContainerItems.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false)]
		public TabStrip DockTabControl
		{
			get
			{
				return m_TabDockItems;
			}
		}

		private void TabStripTabChanging(object sender, TabStripTabChangingEventArgs e)
		{
			if(m_RefreshingDockTab)
				return;
			e.Cancel=InvokeDockTabChange((e.OldTab==null?null:e.OldTab.AttachedItem),(e.NewTab==null?null:e.NewTab.AttachedItem));
		}

		private void SyncBarCaption(BaseItem selected)
		{
			if(m_AutoSyncBarCaption && this.LayoutType==eLayoutType.DockContainer)
			{
                DockContainerItem sel = this.SelectedDockContainerItem;
                if (sel != null && sel.Visible || selected != null)
				{
					if(selected==null)
                        selected = sel; // this.Items[this.SelectedDockTab];

					this.Text=selected.Text;
				}
				else
				{
					foreach(BaseItem item in this.Items)
					{
						if(item.Visible && item.Displayed)
						{
							this.Text=item.Text;
							break;
						}
					}
				}
			}
		}
		internal void SyncBarCaption()
		{
			SyncBarCaption(null);
		}

		private bool InvokeDockTabChange(BaseItem oldItem, BaseItem newItem)
		{
			DockTabChangeEventArgs dockarg=null;
			IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
			bool bCancel=false;
			if(DockTabChange!=null)
			{
				dockarg=new DockTabChangeEventArgs(oldItem,newItem);
				DockTabChange(this,dockarg);
				bCancel=dockarg.Cancel;
				if(bCancel)
					return bCancel;
			}
			if(m_Owner!=null)
			{
				if(dockarg==null)
					dockarg=new DockTabChangeEventArgs(oldItem,newItem);
				if(ownersupport!=null)
					ownersupport.InvokeDockTabChange(this,dockarg);
				bCancel=dockarg.Cancel;
			}
            if (!bCancel)
            {
                SyncBarCaption(newItem);
                UpdateCloseButtonVisibility();
            }
			return bCancel;
		}

		internal void InvokeBeforeDockTabDisplayed(BaseItem item)
		{
			if(BeforeDockTabDisplayed!=null)
				BeforeDockTabDisplayed(item,new EventArgs());

			IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
			if(ownersupport!=null)
				ownersupport.InvokeBeforeDockTabDisplay(item,new EventArgs());
		}

		private void TabStripTabMoved(object sender, TabStripTabMovedEventArgs e)
		{
			m_ItemContainer.SubItems._Remove((DockContainerItem)e.Tab.AttachedItem);
			m_ItemContainer.SubItems._Add((DockContainerItem)e.Tab.AttachedItem,e.NewIndex);
			m_TabsRearranged=true;
		}

		/// <summary>
		/// Gets or sets the selected DockContainerItem if bar represents dockable window.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockContainerItem SelectedDockContainerItem
		{
			get
			{
				if(m_ItemContainer.LayoutType!=eLayoutType.DockContainer) return null;
				int si = this.SelectedDockTab;
				if(si>=0)
					return this.Items[si] as DockContainerItem;
				if(this.VisibleItemCount>0)
				{
					foreach(BaseItem item in this.Items)
					{
						if(item.Visible && item.Displayed) return item as DockContainerItem;
					}
				}
				return null;
			}
			set
			{
				if(m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
					throw new InvalidOperationException("Bar type is not dockable window. LayoutType must be set to eLayoutType.DockContainer");
				if(!this.Items.Contains(value))
					throw new InvalidOperationException("Bar.Items collection does not contain the item");
				this.SelectedDockTab = this.Items.IndexOf(value);
			}
		}

		/// <summary>
		/// Gets or sets the tab (DockContainerItem) index for Bars with LayoutType set to eLayoutType.DockContainer. Index corresponds to the index of the DockContainerItem in Bar.Items collection.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Gets or sets the tab (DockContainerItem) index for Bars with LayoutType set to eLayoutType.DockContainer."),DefaultValue(-1)]
		public int SelectedDockTab
		{
			get
			{
				if(m_ItemContainer.LayoutType!=eLayoutType.DockContainer || m_TabDockItems==null)
					return -1;
				if(m_TabDockItems.SelectedTab!=null)
					return m_ItemContainer.SubItems.IndexOf(m_TabDockItems.SelectedTab.AttachedItem);

				return -1;
			}
			set
			{
				if(m_TabDockItems==null || m_TabDockItems!=null && this.AutoHide)
				{
					if(value<this.Items.Count)
					{
						for(int i=0;i<this.Items.Count;i++)
						{
							if(i==value && this.Items[i].Visible)
								this.Items[i].Displayed=true;
							else
								this.Items[i].Displayed=false;
						}
						if(m_TabDockItems==null)
						{
							if(this.Items[value].Displayed)
								InvokeDockTabChange(null,this.Items[value]);
							RefreshAutoHidePanel();
						}
					}
					if(m_TabDockItems==null) return;
				}
				if(m_ItemContainer.LayoutType!=eLayoutType.DockContainer)
					throw new InvalidOperationException("SelectedDockTab property can be set only for LayoutType=eLayoutType.DockContainer.");
                if (value < 0) value = 0;
				if(value>=m_ItemContainer.SubItems.Count)
					throw new InvalidOperationException("Invalid tab index.");
                //if(this.AutoHide)
                //{
                //    m_LastDockSiteInfo.DockedHeight=0;
                //    m_LastDockSiteInfo.DockedWidth=0;
                //}
				BaseItem item=m_ItemContainer.SubItems[value];
				foreach(TabItem tab in m_TabDockItems.Tabs)
				{
					if(tab.AttachedItem==item)
					{
						if(m_TabDockItems.SelectedTab==tab && this.AutoHide)
							InvokeBeforeDockTabDisplayed(item);
						m_TabDockItems.SelectedTab=tab;
						if(this.AutoHide)
						{
							AutoHidePanel panel=GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
							panel.SelectedDockContainerItem=item as DockContainerItem;
						}
						break;
					}
				}
			}
		}

		#region Inherited properties hiding
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override Cursor Cursor
		{
			get
			{return base.Cursor;}
			set
			{base.Cursor=value;}
		}
		/// <summary>
		/// Indicates Bar background image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates Bar background image.")]
		public override Image BackgroundImage
		{
			get{return base.BackgroundImage;}
			set{base.BackgroundImage=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get{return base.ForeColor;}
			set{base.ForeColor=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get{return base.RightToLeft;}
			set{base.RightToLeft=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override ContextMenu ContextMenu
		{
			get{return base.ContextMenu;}
			set{base.ContextMenu=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get{return base.ImeMode;}
			set{base.ImeMode=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get{return base.TabIndex;}
			set{base.TabIndex=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get{return base.TabStop;}
			set{base.TabStop=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new bool CausesValidation
		{
			get{return base.CausesValidation;}
			set{base.CausesValidation=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override AnchorStyles Anchor
		{
			get{return base.Anchor;}
			set{base.Anchor=value;}
		}
		[Browsable(true),DevCoBrowsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override DockStyle Dock
		{
			get{return base.Dock;}
			set
            {
                if (this.DockSide == eDockSide.Document && value != DockStyle.None && this.Parent is DockSite)
                    return;
                base.Dock=value;
            }
		}

		[Browsable(false)]
		public new Point Location
		{
			get{return base.Location;}
			set
			{
				if(m_BarState==eBarState.Floating)
				{
					if(m_Float!=null)
						m_Float.Location=value;
				}
				else
					base.Location=value;
			}
		}
		[Browsable(false)]
		public new Size Size
		{
			get{return base.Size;}
			set{base.Size=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool AllowDrop
		{
			get{return base.AllowDrop;}
			set{base.AllowDrop=value;}
		}
#if FRAMEWORK20
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public new System.Windows.Forms.Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }
#endif
		#endregion

		/// <summary>
		/// Gets or sets whether caption button menu for bars with grab handle task pane is automatically created.
		/// Caption menu when automatically created will display the list of all items from Items collection
		/// and it will maintain only one item from the list as visible item.
		/// To create custom caption menu that is displayed when user clicks the caption button handle CaptionButtonClick event.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether caption button drop-down menu is automatically created")]
		public virtual bool AutoCreateCaptionMenu
		{
			get
			{
				return m_AutoCreateCaptionMenu;
			}
			set
			{
				m_AutoCreateCaptionMenu=value;
				if(this.Visible)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether caption (text) of the bars with dock container layout is automatically set to the
		/// selected dock container item text.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Behavior"),Description("Indicates whether caption is automatically set to the active dock container item text.")]
		public virtual bool AutoSyncBarCaption
		{
			get {return m_AutoSyncBarCaption;}
			set
			{
				m_AutoSyncBarCaption=value;
				SyncBarCaption();
			}
		}

		// IDockInfo Interface implementation
		/// <summary>
		/// Returns the Minimum Size for specified orientation.
		/// </summary>
		/// <param name="dockOrientation">Orientation to return minimum size for.</param>
		/// <returns></returns>
		public System.Drawing.Size MinimumDockSize(eOrientation dockOrientation)
		{
			if(m_ItemContainer==null) return new Size(32,32);
			if(m_ItemContainer.LayoutType==eLayoutType.DockContainer && (m_ItemContainer.Stretch || m_DockStretch))
			{
                if (m_ItemContainer.SubItems.Count>0 && m_ItemContainer.SubItems[0] is DockContainerItem)
					return ((DockContainerItem)m_ItemContainer.SubItems[0]).MinimumSize;
                return new Size(32, 32);
			}

			if(m_ItemContainer.Stretch || m_DockStretch)
				return PreferredDockSize(dockOrientation);
			
			if(m_ItemContainer.SubItems.Count>0)
			{
				BaseItem objItem=m_ItemContainer.SubItems[0];
				if(objItem!=null)
				{
					return new Size(objItem.WidthInternal+m_ClientRect.Left*2,objItem.HeightInternal);
				}
			}
			
			return new Size(0,0);
		}
		internal Size GetAdjustedFullSize(Size size)
		{
			const int TOTAL_BORDER_SIZE=6;
			Size newSize=size;
			if(m_TabDockItems!=null && m_TabDockItems.Visible && (this.DockTabAlignment == eTabStripAlignment.Top || this.DockTabAlignment == eTabStripAlignment.Bottom))
				newSize.Height+=(m_TabDockItems.Height);
			newSize.Height+=(TOTAL_BORDER_SIZE+SystemInformation.ToolWindowCaptionHeight);
			newSize.Width+=TOTAL_BORDER_SIZE;
			return newSize;
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public int GetBarDockedSize(eOrientation o)
		{
			const int TOTAL_BORDER_SIZE=6;
			if(m_ItemContainer==null)
				return 0;
			if(o==eOrientation.Horizontal && m_LastDockSiteInfo.DockedHeight>0 && (m_LastDockSiteInfo.DockSide==DockStyle.Top || m_LastDockSiteInfo.DockSide==DockStyle.Bottom))
				return m_LastDockSiteInfo.DockedHeight;
			else if(o==eOrientation.Vertical && m_LastDockSiteInfo.DockedWidth>0 && (m_LastDockSiteInfo.DockSide==DockStyle.Left || m_LastDockSiteInfo.DockSide==DockStyle.Right))
				return m_LastDockSiteInfo.DockedWidth;

			if(o==eOrientation.Horizontal && m_ItemContainer.MinHeight>0 && m_ItemContainer.MinHeight+SystemInformation.ToolWindowCaptionHeight<this.GetFormClientHeight())
				return m_ItemContainer.MinHeight+SystemInformation.ToolWindowCaptionHeight;
			else if(o==eOrientation.Vertical && m_ItemContainer.MinWidth>0 && m_ItemContainer.MinWidth+SystemInformation.ToolWindowCaptionHeight<this.GetFormClientWidth())
				return m_ItemContainer.MinWidth+SystemInformation.ToolWindowCaptionHeight;
			
			if(this.SelectedDockTab>=0 || this.VisibleItemCount>0 && this.LayoutType == eLayoutType.DockContainer)
			{
                DockContainerItem item = null;
                if (this.SelectedDockTab >= 0)
                    item = this.Items[this.SelectedDockTab] as DockContainerItem;
                else
                    item = GetFirstVisibleItem() as DockContainerItem;
				if(item!=null)
				{
					if(o==eOrientation.Horizontal)
					{
                        if (item.MinimumSize.Height > 0)
                        {
                            int height = this.Height;
                            if (this.Parent != null && (this.DockSide == eDockSide.Left || this.DockSide == eDockSide.Right) ||
                                this.Parent == null && (LastDockSide == eDockSide.Left || LastDockSide == eDockSide.Right))
                                height = this.Width;

                            if (height > item.MinimumSize.Height)
                                return height;
                            return item.MinimumSize.Height * 2 + DOCKTABSTRIP_HEIGHT + TOTAL_BORDER_SIZE;
                        }
					}
					else
					{
                        if (item.MinimumSize.Width > 0)
                        {
                            int width = this.Width;
                            if (this.Parent != null && (this.DockSide == eDockSide.Top || this.DockSide == eDockSide.Bottom) ||
                                this.Parent == null && (LastDockSide == eDockSide.Top || LastDockSide == eDockSide.Bottom))
                                width = this.Height;
                            if (width > item.MinimumSize.Width)
                                return width;
                            return item.MinimumSize.Width * 2 + TOTAL_BORDER_SIZE;
                        }
					}
				}
			}

			if(o==eOrientation.Horizontal)
				return this.Height;
			else
				return this.Width;
		}

		/// <summary>
		/// Returns the preferred size of the Bar when docked.
		/// </summary>
		/// <param name="dockOrientation">Orientation to return preferred size for.</param>
		/// <returns></returns>
		public System.Drawing.Size PreferredDockSize(eOrientation dockOrientation)
		{
			// Return preffered size for this container
			IOwner owner=m_Owner as IOwner;
			if(dockOrientation==eOrientation.Horizontal)
			{
				if(m_DockedSizeH.IsEmpty)
				{
					if(owner!=null && owner.ParentForm!=null)
						m_DockedSizeH=RecalcSizeOnly(owner.ParentForm.Size,eOrientation.Horizontal,eBarState.Docked,m_WrapItemsDock);
					else
						m_DockedSizeH=RecalcSizeOnly(System.Windows.Forms.Screen.FromControl(this).WorkingArea.Size,eOrientation.Horizontal,eBarState.Docked,m_WrapItemsDock);
				}

				return m_DockedSizeH;
			}
			else
			{
				// We will need to calculate that size
				if(m_DockedSizeV.IsEmpty)
				{
					Size size=Size.Empty;
					if(owner!=null && owner.ParentForm!=null)
						size=RecalcSizeOnly(owner.ParentForm.Size,eOrientation.Horizontal,eBarState.Docked,m_WrapItemsDock);
					else
						size=RecalcSizeOnly(System.Windows.Forms.Screen.FromControl(this).WorkingArea.Size,eOrientation.Horizontal,eBarState.Docked,m_WrapItemsDock);
					m_DockedSizeV=new Size(size.Height,size.Width);
				}
				
				return m_DockedSizeV;
			}
		}

		/// <summary>
		/// Specifies whether Bar can be undocked. Does not apply to stand alone bars.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Specifies whether Bar can be undocked."),DefaultValue(true)]
		public bool CanUndock
		{
			get
			{
				return m_CanUndock;
			}
			set
			{
				if(m_CanUndock!=value)
					m_CanUndock=value;
			}
		}

		/// <summary>
		/// Specifes whether end-user can tear-off (deattach) the tabs on dockable window.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Specifes whether end-user can tear-off (deattach) the tabs on dockable window."),DefaultValue(true)]
		public bool CanTearOffTabs
		{
			get
			{
				return m_CanTearOffTabs;
			}
			set
			{
				if(m_CanTearOffTabs!=value)
					m_CanTearOffTabs=value;
			}
		}

		/// <summary>
		/// Specifes whether end-user can reorder the tabs on dockable window.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Specifes whether end-user can reorder the tabs on dockable window."),DefaultValue(true)]
		public bool CanReorderTabs
		{
			get
			{
				return m_CanReorderTabs;
			}
			set
			{
				if(m_CanReorderTabs!=value)
				{
					m_CanReorderTabs=value;
					if(m_TabDockItems!=null)
						m_TabDockItems.CanReorderTabs=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether bar or DockContainerItem that is torn-off this bar can be docked
		/// as tab to another bar. Default value is true which indicates that bar can be docked as tab to another bar.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),Category("Docking"),Description("Indicates whether bar or DockContainerItem that is torn-off this bar can be docked as tab to another bar.")]
		public bool CanDockTab
		{
			get {return m_CanDockTab;}
			set {m_CanDockTab=value;}
		}

		/// <summary>
		/// Specifes whether Bar can be docked on Top dock site or not. Does not apply to stand alone bars.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Determines whether bar can be docked to the top edge of container.")]
		public bool CanDockTop
		{
			get
			{
				return m_CanDockTop;
			}
			set
			{
				if(m_CanDockTop!=value)
					m_CanDockTop=value;
			}
		}

		/// <summary>
		/// Specifes whether Bar can be docked on Bottom dock site or not. Does not apply to stand alone bars.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),Category("Docking"),Description("Determines whether bar can be docked to the bottom edge of container.")]
		public bool CanDockBottom
		{
			get
			{
				return m_CanDockBottom;
			}
			set
			{
				if(m_CanDockBottom!=value)
					m_CanDockBottom=value;
			}
		}

		/// <summary>
		/// Specifes whether Bar can be docked on Left dock site or not. Does not apply to stand alone bars.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Determines whether bar can be docked to the left edge of container.")]
		public bool CanDockLeft
		{
			get
			{
				return m_CanDockLeft;
			}
			set
			{
				if(m_CanDockLeft!=value)
					m_CanDockLeft=value;
			}
		}

		/// <summary>
		/// Specifes whether Bar can be docked on Right dock site or not. Does not apply to stand alone bars.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Determines whether bar can be docked to the right edge of container.")]
		public bool CanDockRight
		{
			get
			{
				return m_CanDockRight;
			}
			set
			{
				if(m_CanDockRight!=value)
					m_CanDockRight=value;
			}
		}

		/// <summary>
		/// Specifies whether Bar can be docked as document. Default value is false. See DotNetBarManager.EnableDocumentDocking for more details.
		/// </summary>
        [Browsable(true), DefaultValue(false) , Category("Docking"), Description("Specifies whether Bar can be docked as document.")]
		public bool CanDockDocument
		{
			get {return m_CanDockDocument;}
			set {m_CanDockDocument=value;}
		}

		/// <summary>
		/// Specifies whether Bar will stretch to always fill the space in dock site. Applies to the dockable bars only.
		/// </summary>
		[Browsable(false),DefaultValue(false),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Specifies whether Bar will stretch to always fill the space in dock site.")/*,DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)*/]
		public bool Stretch
		{
			get
			{
				return m_DockStretch;
			}
			set
			{
				if(m_DockStretch!=value)
				{
					m_DockStretch=value;
					this.RecalcLayout();
				}
			}
		}

		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
			if(this.Dock!=DockStyle.None)
				m_DockStretch=true;
			else
                m_DockStretch=false;
			this.RecalcLayout();
		}

		private bool m_DisabledImagesGrayScale=true;
		/// <summary>
		/// Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images. Default is true.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(true),System.ComponentModel.Category("Appearance"),Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
		public bool DisabledImagesGrayScale
		{
			get
			{
				return m_DisabledImagesGrayScale;
			}
			set
			{
				m_DisabledImagesGrayScale=value;
			}
		}

		// TODO: Merge Implementation
		//		/// <summary>
		//		/// Specifies whether Bar is merged with MDI Parent Bars when on MDI Child form.
		//		/// </summary>
		//		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Specifies whether Bar is merged with MDI Parent Bars when on MDI Child form.")]
		//		public bool MergeEnabled
		//		{
		//			get
		//			{
		//				return m_MergeEnabled;
		//			}
		//			set
		//			{
		//				m_MergeEnabled=value;
		//				if(!this.DesignMode && this.Visible && IsParentMdiChild)
		//				{
		//					this.Visible=false;
		//				}
		//			}
		//		}

		/// <summary>
		/// Gets/Sets the distance from the far left/top side of the docking site or suggests the order of the docked bar. Upon serialization this property
        /// will contain actual left/top position of the bar. You can use it to re-order the bars docked on the same line. Property value is relative to the other
        /// bars docked on the same line when it is used to change the order. For example setting DockOffset value to 10 will place the bar just after the last bar on the
        /// same line that has DockOffset value less than 10. If there is no bar with DockOffset value less than 10 the bar will be placed in first position.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Indicates the distance from the far left/top side of the docking site.."),DefaultValue(0)]
		public int DockOffset
		{
			get
			{
				return m_DockOffset;
			}
			set
			{
				m_DockOffset=value;
			}
		}
		/// <summary>
		/// Gets/Sets the dock line.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Indicates the docking line."),DefaultValue(0)]
		public int DockLine
		{
			get
			{
				return m_DockLine;
			}
			set
			{
				m_DockLine=value;
				if(m_BarState==eBarState.Docked)
				{
					if(this.Parent is DockSite && !m_LayoutSuspended)
					{
						((DockSite)this.Parent).AdjustBarPosition(this);
					}
				}
			}
		}

		/// <summary>
		/// Sets the dock line but it does not forces the Bar to change position. The position will be changed on next layout request or when dock site needs to recalculate the layout of the bat. Used internally only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void SetDockLine(int iLine)
		{
			m_DockLine=iLine;
		}

		/// <summary>
		/// Gets or sets the dock tab alignment.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Gets or sets the dock tab alignment."),System.ComponentModel.DefaultValue(eTabStripAlignment.Bottom)]
		public eTabStripAlignment DockTabAlignment
		{
			get {return m_DockTabAlignment;}
			set
			{
				m_DockTabAlignment=value;
				if(m_TabDockItems!=null)
				{
					m_TabDockItems.TabAlignment=m_DockTabAlignment;
					RecalcLayout();
					ResizeDockTab();
				}
			}
		}

        /// <summary>
        /// Gets or sets whether selected dock tab is closed when Bar caption close button is pressed. Default value is false which indicates that whole bar will be hidden when bars close button is pressed.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Docking"), Description("Indicates whether selected dock tab is closed when Bar caption close button is pressed. Default value is false which indicates that whole bar will be hidden when bars close button is pressed.")]
        public bool CloseSingleTab
        {
            get { return m_CloseSingleTab; }
            set { m_CloseSingleTab = value; }
        }

        /// <summary>
        /// Gets or sets whether close button is displayed on each dock tab that allows closing of the tab. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Description("Indicates whether close button is displayed on each dock tab that allows closing of the tab."), Category("Docking")]
        public bool DockTabCloseButtonVisible
        {
            get { return m_DockTabCloseButtonVisible; }
            set
            {
                if (m_DockTabCloseButtonVisible != value)
                {
                    m_DockTabCloseButtonVisible = value;
                    if (m_TabDockItems != null)
                    {
                        m_TabDockItems.CloseButtonOnTabsVisible = m_DockTabCloseButtonVisible;
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.LayoutType == eLayoutType.DockContainer && this.VisibleItemCount > 1)
            {
                if ((keyData & Keys.Control) == Keys.Control && (keyData & Keys.Tab) == Keys.Tab && WinApi.HIWORD(WinApi.GetKeyState(9)) != 0)
                {

                    if ((keyData & Keys.Shift) == Keys.Shift)
                    {
                        SelectPreviousTab(true);
                    }
                    else
                    {
                        SelectNextTab(true);
                    }
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        internal bool SelectPreviousTab(bool cycle)
        {
            int newIndex = -1;
            if (this.SelectedDockTab <= 0)
            {
                if (cycle)
                {
                    newIndex = GetTabIndex(this.Items.Count, -1);
                }
                else
                    return false;
            }
            else
            {
                newIndex = GetTabIndex(this.SelectedDockTab, -1);
                if (newIndex < 0 && cycle)
                {
                    newIndex = GetTabIndex(this.Items.Count, -1);
                }
            }
            if (newIndex < 0) return false;
            this.SelectedDockTab = newIndex;
            return true;
        }

        internal bool SelectNextTab(bool cycle)
        {
            int newIndex = -1;
            if (this.SelectedDockTab >= this.Items.Count - 1)
            {
                if (cycle)
                {
                    newIndex = GetTabIndex(-1, 1);
                }
                else
                    return false;
            }
            else
            {
                newIndex = GetTabIndex(this.SelectedDockTab, 1);
                if (newIndex < 0 && cycle)
                {
                    newIndex = GetTabIndex(-1, 1);
                }
            }
            if (newIndex < 0) return false;
            this.SelectedDockTab = newIndex;
            return true;
        }

        private int GetTabIndex(int start, int direction)
        {
            int i = start;
            int end = this.Items.Count - 1;
            int increment = 1;

            if (direction < 0)
            {
                end = 0;
                increment = -1;
                if (start <= 0) return -1;
            }
            else if (start >= end) return -1;

            while (end != i)
            {
                i += increment;
                DockContainerItem tab = this.Items[i] as DockContainerItem;
                if (tab != null && tab.Visible)
                    return i;
            }

            return -1;
        }

		/// <summary>
		/// Gets or sets whether tab that shows all dock containers on the bar is visible all the time. By default
		/// tab is hidden when there is only one item displayed.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Indicates whether tab that shows all dock containers on the bar is visible all the time."),System.ComponentModel.DefaultValue(false)]
		public virtual bool AlwaysDisplayDockTab
		{
			get {return m_AlwaysDisplayDockTab;}
			set
			{
				if(m_AlwaysDisplayDockTab!=value)
				{
					m_AlwaysDisplayDockTab=value;
					RefreshDockTab(false);
				}
			}
		}

		/// <summary>
		/// Gets or sets whether bar is locked to prevent docking below it. Applies to undockable bars only.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Indicates whether bar is locked to prevent docking below it. Applies to undockable bars only."),DefaultValue(false)]
		public bool LockDockPosition
		{
			get {return m_LockDockPosition;}
			set
			{
				if(m_LockDockPosition==value)
					return;
				m_LockDockPosition=value;
			}
		}

		/// <summary>
		/// Gets/Sets the orientation of the Bar.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(eOrientation.Horizontal),DevCoBrowsable(false)]
		public eOrientation DockOrientation
		{
			get
			{
				return m_ItemContainer.Orientation;
			}
			set
			{
				if(m_ItemContainer.Orientation!=value)
				{
					m_ItemContainer.Orientation=value;
				}
				if(this.DesignMode && !(this.Parent is DockSite))
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Returns whether Bar is docked or not.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool Docked
		{
			get
			{
				return (m_BarState==eBarState.Docked);
			}
		}

		/// <summary>
		/// Returns the Bars dock site.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public System.Windows.Forms.Control DockedSite
		{
			get
			{
				return this.Parent;
			}
		}

        public bool ShouldSerializeDockSide()
        {
            if (this.Parent is DockSite && ((DockSite)this.Parent).DocumentDockContainer != null)
                return false;
            return DockSide != eDockSide.None;
        }

        private eDockSide _LastDockSidePrivate = eDockSide.None;
        internal eDockSide LastDockSide
        {
            get
            {
                return _LastDockSidePrivate;
            }
            set
            {
            	_LastDockSidePrivate = value;
            }
        }

		/// <summary>
		/// Gets/Sets the dock side for the Bar.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(true),System.ComponentModel.Category("Docking"),System.ComponentModel.Description("Indicates the dock side for the Bar.")]
		public eDockSide DockSide
		{
			get
			{
				if(m_BarState!=eBarState.Docked)
					return eDockSide.None;
				if(this.Parent==null)
					return eDockSide.Top;
				else if(this.Parent.Dock==DockStyle.Left)
					return eDockSide.Left;
				else if(this.Parent.Dock==DockStyle.Right)
					return eDockSide.Right;
				else if(this.Parent.Dock==DockStyle.Top)
					return eDockSide.Top;
				else if(this.Parent.Dock==DockStyle.Bottom)
					return eDockSide.Bottom;
				else if(this.Parent.Dock==DockStyle.Fill)
					return eDockSide.Document;
				return eDockSide.None;
			}
			set
			{
				if(this.Owner==null || m_LayoutSuspended)
				{
					m_DockSideDelayed=(int)value;
					return;
				}

				if(this.AutoHide)
				{
					ChangeAutoHidePanel(value);
					return;
				}

				DockSiteInfo pDockInfo=new DockSiteInfo();
				IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
				if(ownersupport==null)
				{
					m_DockSideDelayed=(int)value;
					return;
					//throw(new System.InvalidOperationException("Could not find owner of the Bar or owner does not implement IOwnerBarSupport."));
				}

				// Use Dock Line and Dock Offset to determine bar insert position
				pDockInfo.InsertPosition=-10;
				pDockInfo.DockLine=m_DockLine;
				pDockInfo.DockOffset=m_DockOffset;

				// Reset the split width and height
				if(!m_BarDefinitionLoading)
				{
					this.SplitDockHeight=0;
					this.SplitDockWidth=0;
				}

				if(value==eDockSide.Left)
				{
                    DockSite ds = null;
                    if (this.LayoutType == eLayoutType.Toolbar)
                    {
                        ds = ownersupport.ToolbarLeftDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.ToolbarLeftDockSite dock-site is not set."));
                    }
                    else
                    {
                        ds = ownersupport.LeftDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.LeftDockSite dock-site is not set."));
                    }

					pDockInfo.objDockSite=ds;
				}
				else if(value==eDockSide.Right)
				{
                    DockSite ds = null;
                    if (this.LayoutType == eLayoutType.Toolbar)
                    {
                        ds = ownersupport.ToolbarRightDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.ToolbarRightDockSite dock-site is not set."));
                    }
                    else
                    {
                        ds = ownersupport.RightDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.RightDockSite dock-site is not set."));
                    }

					pDockInfo.objDockSite=ds;
				}
				else if(value==eDockSide.Top)
				{
                    DockSite ds = null;
                    if (this.LayoutType == eLayoutType.Toolbar)
                    {
                        ds = ownersupport.ToolbarTopDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.ToolbarTopDockSite dock-site is not set."));
                    }
                    else
                    {
                        ds = ownersupport.TopDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.TopDockSite dock-site is not set."));
                    }

					pDockInfo.objDockSite=ds;
				}
				else if(value==eDockSide.Bottom)
				{
                    DockSite ds = null;
                    if (this.LayoutType == eLayoutType.Toolbar)
                    {
                        ds = ownersupport.ToolbarBottomDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.ToolbarBottomDockSite dock-site is not set."));
                    }
                    else
                    {
                        ds = ownersupport.BottomDockSite;
                        if (ds == null)
                            throw (new System.InvalidOperationException("DotNetBarManager.BottomDockSite dock-site is not set."));
                    }

                    if (ds == null)
                        throw (new System.InvalidOperationException("Bottom dock-site is not set."));
                    pDockInfo.objDockSite = ds;
				}
				else if(value==eDockSide.Document)
				{
					pDockInfo.objDockSite=ownersupport.FillDockSite;
				}
				else
					pDockInfo.objDockSite=null;

				this.DockingHandler(pDockInfo,m_FloatingRect.Location);
			}
		}

		private void ChangeAutoHidePanel(eDockSide side)
		{
			if(!this.AutoHide)
				return;
			DockStyle dockStyle=DockStyle.Left;
			if(side==eDockSide.Bottom)
				dockStyle=DockStyle.Bottom;
			else if(side==eDockSide.Right)
				dockStyle=DockStyle.Right;
			else if(side==eDockSide.Top)
				dockStyle=DockStyle.Top;

			if(m_LastDockSiteInfo.DockSide==dockStyle || side==eDockSide.None)
				return;

			AutoHidePanel panel=GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
			if(panel==null)
				return;

			IOwnerBarSupport barSupp=m_Owner as IOwnerBarSupport;
			if(barSupp==null)
				return;

			AnimateHide();
            
			panel.RemoveBar(this);
			m_LastDockSiteInfo.DockSide=dockStyle;
			switch(dockStyle)
			{
				case DockStyle.Left:
					m_LastDockSiteInfo.objDockSite=barSupp.LeftDockSite;
					break;
				case DockStyle.Right:
					m_LastDockSiteInfo.objDockSite=barSupp.RightDockSite;
					break;
				case DockStyle.Top:
					m_LastDockSiteInfo.objDockSite=barSupp.TopDockSite;
					break;
				case DockStyle.Bottom:
					m_LastDockSiteInfo.objDockSite=barSupp.BottomDockSite;
					break;
			}
			
            panel=GetAutoHidePanel(dockStyle);
			panel.AddBar(this);
		}

		/// <summary>
		/// Gets or sets the inital floating location. This location will be used when DockSide is set to None.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Description("Indicates the inital floating location."),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point InitalFloatLocation
		{
			get {return m_FloatingRect.Location;}
			set {m_FloatingRect.Location=value;}
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

        /// <summary>
        /// Gets or sets whether control is selected in designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DesignerSelection
        {
            get { return m_DesignerSelection; }
            set
            {
                if (m_DesignerSelection != value)
                {
                    m_DesignerSelection = value;
                    this.Refresh();
                }
            }
        }

		#region IOwner
		// IOwner Implementation
		BaseItem m_ExpandedItem=null;
		BaseItem m_FocusItem=null;
		private Hashtable m_ShortcutTable=new Hashtable();
		// Only one Popup Item can be expanded at a time. This is used
		// to track the currently expanded popup item and to close the popup item
		// if another item is expanding.
		void IOwner.SetExpandedItem(BaseItem objItem)
		{
			if(objItem!=null && objItem.Parent is PopupItem)
				return;
			if(m_ExpandedItem!=null)
			{
				if(m_ExpandedItem.Expanded)
					m_ExpandedItem.Expanded=false;
				m_ExpandedItem=null;
			}
			m_ExpandedItem=objItem;
		}

		BaseItem IOwner.GetExpandedItem()
		{
			return m_ExpandedItem;
		}

		// Currently we are using this to communicate "focus" when control is in
		// design mode. This can be used later if we decide to add focus
		// handling to our BaseItem class.
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler FocusItemChange;
		void IOwner.SetFocusItem(BaseItem objFocusItem)
		{
			if(DockSide==eDockSide.Document && m_Owner!=null)
			{
				((IOwner)m_Owner).SetFocusItem(objFocusItem);
				return;
			}

			if(m_FocusItem!=null && m_FocusItem!=objFocusItem)
			{
				m_FocusItem.OnLostFocus();
			}
			m_FocusItem=objFocusItem;
			if(m_FocusItem!=null)
				m_FocusItem.OnGotFocus();

			if(FocusItemChange!=null)
				FocusItemChange(this,new EventArgs());
		}

		BaseItem IOwner.GetFocusItem()
		{
            if (DockSide == eDockSide.Document && m_Owner != null && m_Owner is IOwner)
            {
                return ((IOwner)m_Owner).GetFocusItem();
            }
			return m_FocusItem;
		}
		bool IOwner.DesignMode
		{
			get {return this.DesignMode;}
		}
		void IOwner.DesignTimeContextMenu(BaseItem objItem)
		{
		}

		void IOwner.RemoveShortcutsFromItem(BaseItem objItem)
		{
			ShortcutTableEntry objEntry=null;
			if(objItem.ShortcutString!="")
			{
				foreach(eShortcut key in objItem.Shortcuts)
				{
					if(m_ShortcutTable.ContainsKey(key))
					{
						objEntry=(ShortcutTableEntry)m_ShortcutTable[key];
						try
						{
							objEntry.Items.Remove(objItem.Id);
							if(objEntry.Items.Count==0)
								m_ShortcutTable.Remove(objEntry.Shortcut);
						}
						catch(System.ArgumentException)	{}
					}
				}
			}
			IOwner owner=this as IOwner;
			foreach(BaseItem objTmp in objItem.SubItems)
				owner.RemoveShortcutsFromItem(objTmp);
		}

		void IOwner.AddShortcutsFromItem(BaseItem objItem)
		{
			ShortcutTableEntry objEntry=null;
			if(objItem.ShortcutString!="")
			{
				foreach(eShortcut key in objItem.Shortcuts)
				{
					if(m_ShortcutTable.ContainsKey(key))
						objEntry=(ShortcutTableEntry)m_ShortcutTable[objItem.Shortcuts[0]];
					else
					{
						objEntry=new ShortcutTableEntry(key);
						m_ShortcutTable.Add(objEntry.Shortcut,objEntry);
					}
					try
					{
						objEntry.Items.Add(objItem.Id,objItem);
					}
					catch(System.ArgumentException)	{}
				}
			}
			IOwner owner=this as IOwner;
			foreach(BaseItem objTmp in objItem.SubItems)
				owner.AddShortcutsFromItem(objTmp);
		}

		/// <summary>
		/// Gets or sets whether accelerator letters for menu or toolbar commands are underlined regardless of
		/// current Windows settings. Accelerator keys allow easy access to menu commands by using
		/// Alt + choosen key (letter). Default value is false which indicates that system setting is used
		/// to determine whether accelerator letters are underlined. Setting this property to true
		/// will always display accelerator letter underlined.
		/// </summary>
		[Browsable(true),DevCoBrowsable(false),DefaultValue(false),Category("Run-time Behavior"),Description("Indicates whether accelerator letters for menu or toolbar commands are underlined regardless of current Windows settings.")]
		public bool AlwaysDisplayKeyAccelerators
		{
			get {return m_AlwaysDisplayKeyAccelerators;}
			set
			{
				if(m_AlwaysDisplayKeyAccelerators!=value)
				{
					m_AlwaysDisplayKeyAccelerators=value;
					this.Refresh();
				}
			}
		}

		Form IOwner.ParentForm
		{
			get{return this.FindForm();}
			set {}
		}

		Form IOwner.ActiveMdiChild
		{
			get
			{
				Form form=this.FindForm();
				if(form==null)
					return null;
				if(form.IsMdiContainer)
					return form.ActiveMdiChild;
				return null;
			}
		}
		System.Windows.Forms.MdiClient IOwner.GetMdiClient(System.Windows.Forms.Form MdiForm)
		{
			return BarFunctions.GetMdiClient(MdiForm);
		}

		void IOwner.Customize() {}

		private System.Windows.Forms.ImageList m_ImageList;
		private System.Windows.Forms.ImageList m_ImageListMedium=null;
		private System.Windows.Forms.ImageList m_ImageListLarge=null;
		/// <summary>
		/// ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.
		/// </summary>
		[Browsable(true),DevCoBrowsable(false),DefaultValue(null),Category("Data"),Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
		public System.Windows.Forms.ImageList Images
		{
			get
			{
				return m_ImageList;
			}
			set
			{
				if(m_ImageList!=null)
					m_ImageList.Disposed-=new EventHandler(this.ImageListDisposed);
				m_ImageList=value;
				if(m_ImageList!=null)
					m_ImageList.Disposed+=new EventHandler(this.ImageListDisposed);
			}
		}
		/// <summary>
		/// ImageList for medium-sized images used on Items.
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Data"),Description("ImageList for medium-sized images used on Items.")]
		public System.Windows.Forms.ImageList ImagesMedium
		{
			get
			{
				return m_ImageListMedium;
			}
			set
			{
				if(m_ImageListMedium!=null)
					m_ImageListMedium.Disposed-=new EventHandler(this.ImageListDisposed);
				m_ImageListMedium=value;
				if(m_ImageListMedium!=null)
					m_ImageListMedium.Disposed+=new EventHandler(this.ImageListDisposed);
			}
		}

		/// <summary>
		/// ImageList for large-sized images used on Items.
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Data"),Description("ImageList for large-sized images used on Items.")]
		public System.Windows.Forms.ImageList ImagesLarge
		{
			get
			{
				return m_ImageListLarge;
			}
			set
			{
				if(m_ImageListLarge!=null)
					m_ImageListLarge.Disposed-=new EventHandler(this.ImageListDisposed);
				m_ImageListLarge=value;
				if(m_ImageListLarge!=null)
					m_ImageListLarge.Disposed+=new EventHandler(this.ImageListDisposed);
			}
		}
		private void ImageListDisposed(object sender, EventArgs e)
		{
			if(sender==m_ImageList)
			{
				this.Images=null;
			}
			else if(sender==m_ImageListLarge)
			{
				this.ImagesLarge=null;
			}
			else if(sender==m_ImageListMedium)
			{
				this.ImagesMedium=null;
			}
		}
		#endregion

		#region ISupportInitialize
		/// <summary>
		/// ISupportInitialize.BeginInit implementation.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void BeginInit()
		{
            m_LayoutSuspended = true;
		}

		/// <summary>
		/// ISupportInitialize.EndInit implementation.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void EndInit()
		{
            m_LayoutSuspended = false;
            if (this.DockSide == eDockSide.Document && this.Parent is DockSite)
            {
                ((DockSite)this.Parent).RecalcLayout();
            }
            else
                this.RecalcSize();
            if (this.AutoHide != m_AutoHideStateDelayed)
                this.AutoHide = m_AutoHideStateDelayed;
		}
		#endregion

		#region IBarDesignerServices
		IBarItemDesigner IBarDesignerServices.Designer
		{
			get {return m_BarDesigner;}
			set {m_BarDesigner=value;}
		}
		#endregion

		#region IOwner
		void IOwner.InvokeResetDefinition(BaseItem item,EventArgs e){}
		bool IOwner.ShowResetButton{get{return false;}set{}}
		void IOwner.InvokeUserCustomize(object sender,EventArgs e){}
		void IOwner.InvokeEndUserCustomize(object sender,EndUserCustomizeEventArgs e){}
		bool IOwner.ShowShortcutKeysInToolTips{get{return true;}set{}}
		void IOwner.StartItemDrag(BaseItem item)
		{
			if(this.DesignMode && m_BarDesigner!=null)
				m_BarDesigner.StartExternalDrag(item);
		}
		BaseItem IOwner.DragItem{get{return null;}}
		bool IOwner.DragInProgress{get{return false;}}
		void IOwner.OnApplicationActivate(){}
		void IOwner.OnApplicationDeactivate()
		{
            ClosePopups();
		}
		void IOwner.OnParentPositionChanging(){}


		private void MenuEventSupportHook()
		{
			if(m_MenuEventSupport)
				return;
			m_MenuEventSupport=true;
			
			Form parentForm=this.FindForm();
			if(parentForm==null)
			{
				m_MenuEventSupport=false;
				return;
			}

			parentForm.Resize+=new System.EventHandler(this.ParentResize);
			parentForm.Deactivate+=new System.EventHandler(this.ParentDeactivate);

			DotNetBarManager.RegisterParentMsgHandler(this,parentForm);
		}
		private void MenuEventSupportUnhook()
		{
			if(!m_MenuEventSupport)
				return;
			m_MenuEventSupport=false;

			Form parentForm=this.FindForm();
			if(parentForm==null)
				return;
            DotNetBarManager.UnRegisterParentMsgHandler(this, parentForm);
			parentForm.Resize-=new System.EventHandler(this.ParentResize);
			parentForm.Deactivate-=new System.EventHandler(this.ParentDeactivate);
		}
		private void ParentResize(object sender, System.EventArgs e)
		{
			Form parentForm=this.FindForm();
			if(parentForm!=null && parentForm.WindowState==FormWindowState.Minimized)
				((IOwner)this).OnApplicationDeactivate();
		}
		private void ParentDeactivate(object sender, System.EventArgs e)
		{
			Form parentForm=this.FindForm();
			if(parentForm!=null && parentForm.WindowState==FormWindowState.Minimized)
				((IOwner)this).OnApplicationDeactivate();
		}
		#endregion

		#region IOwnerMenuSupport
        private bool _UseHook = false;
        /// <summary>
        /// Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.
        /// </summary>
        [System.ComponentModel.Browsable(false), DefaultValue(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
        public bool UseHook
        {
            get
            {
                return _UseHook;
            }
            set
            {
                if (_UseHook == value)
                    return;
                _UseHook = value;
            }
        }

		// IOwnerMenuSupport
		private ArrayList m_RegisteredPopups=new ArrayList();
		private bool m_FilterInstalled=false;
        private Hook m_Hook = null;
		bool IOwnerMenuSupport.PersonalizedAllVisible {get{return false;}set{}}
		bool IOwnerMenuSupport.ShowFullMenusOnHover {get{return true;}set{}}
		bool IOwnerMenuSupport.AlwaysShowFullMenus {get{return false;}set{}}

		void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
		{
			if(m_RegisteredPopups.Contains(objPopup))
				return;

            if (!this.DesignMode && !_UseHook)
            {
                if (!m_FilterInstalled)
                {
                    //System.Windows.Forms.Application.AddMessageFilter(this);
                    MessageHandler.RegisterMessageClient(this);
                    m_FilterInstalled = true;
                }
                if (!m_MenuEventSupport)
                    MenuEventSupportHook();
            }
            else
            {
                if (m_Hook == null)
                {
                    m_Hook = new Hook(this);
                }
            }

			m_RegisteredPopups.Add(objPopup);
			if(objPopup.GetOwner()!=this)
				objPopup.SetOwner(this);
		}
		void IOwnerMenuSupport.UnregisterPopup(PopupItem objPopup)
		{
			if(m_RegisteredPopups.Contains(objPopup))
				m_RegisteredPopups.Remove(objPopup);
            if (m_RegisteredPopups.Count == 0)
            {
                MenuEventSupportUnhook();
                if (m_Hook != null)
                {
                    m_Hook.Dispose();
                    m_Hook = null;
                }
            }
		}
		bool IOwnerMenuSupport.RelayMouseHover()
		{
			foreach(PopupItem popup in m_RegisteredPopups)
			{
				Control ctrl=popup.PopupControl;
				if(ctrl!=null && ctrl.DisplayRectangle.Contains(Control.MousePosition))
				{
					if(ctrl is MenuPanel)
						((MenuPanel)ctrl).InternalMouseHover();
					else if(ctrl is Bar)
						((Bar)ctrl).InternalMouseHover();
					return true;
				}
			}
			return false;
		}

        void IOwnerMenuSupport.ClosePopups()
        {
            ClosePopups();
        }

        internal void ClosePopups()
        {
            ArrayList popupList = new ArrayList(m_RegisteredPopups);
            foreach (PopupItem objPopup in popupList)
                objPopup.ClosePopup();
        }

		// Events
		void IOwnerMenuSupport.InvokePopupClose(PopupItem item,EventArgs e)
		{
			if(PopupClose!=null)
				PopupClose(item,e);
		}
		void IOwnerMenuSupport.InvokePopupContainerLoad(PopupItem item,EventArgs e)
		{
			if(PopupContainerLoad!=null)
				PopupContainerLoad(item,e);
		}
		void IOwnerMenuSupport.InvokePopupContainerUnload(PopupItem item,EventArgs e)
		{
			if(PopupContainerUnload!=null)
				PopupContainerUnload(item,e);
		}
		void IOwnerMenuSupport.InvokePopupOpen(PopupItem item,PopupOpenEventArgs e)
		{
			if(PopupOpen!=null)
				PopupOpen(item,e);
		}
		void IOwnerMenuSupport.InvokePopupShowing(PopupItem item,EventArgs e)
		{
			if(PopupShowing!=null)
				PopupShowing(item,e);
		}
		bool IOwnerMenuSupport.ShowPopupShadow {get{return true;}}
		eMenuDropShadow IOwnerMenuSupport.MenuDropShadow{get{return eMenuDropShadow.SystemDefault;}set{}}
		ePopupAnimation IOwnerMenuSupport.PopupAnimation{get {return ePopupAnimation.SystemDefault;}set{}}
		bool IOwnerMenuSupport.AlphaBlendShadow{get {return true;}set{}}
		#endregion

		#region IMessageHandlerClient
        internal bool IgnoreSysKeyUp
        {
            get { return m_IgnoreSysKeyUp; }
            set { m_IgnoreSysKeyUp = value; }
        }
        internal bool EatSysKeyUp
        {
            get { return m_EatSysKeyUp; }
            set { m_EatSysKeyUp = value; }
        }
		// IMessageHandlerClient Implementation
		private bool m_DispatchShortcuts=false;
		private bool m_IgnoreSysKeyUp=false, m_EatSysKeyUp=false, m_IgnoreF10Key=false;
		bool IMessageHandlerClient.IsModal
		{
			get
			{
				Form form=this.FindForm();
				if(form!=null)
					return form.Modal;
				return false;
			}
		}

        bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }

		bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            bool designMode = this.DesignMode;

			if(m_RegisteredPopups.Count>0)
			{
                BaseItem popup = ((BaseItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1]);
                Keys key = (Keys)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                if (key == Keys.None)
                    key = (Keys)wParam.ToInt32();
                if (popup.Parent == null || IsContextPopup(popup) || (key == Keys.Escape || key == Keys.Enter || key == Keys.Left || key == Keys.Right || key == Keys.Down || key == Keys.Up))
				{
					PopupItem objItem=(PopupItem)m_RegisteredPopups[m_RegisteredPopups.Count-1];

					Control ctrl=objItem.PopupControl as Control;
					Control ctrl2=Control.FromChildHandle(hWnd);
					
					if(ctrl2!=null)
					{
						while(ctrl2.Parent!=null)
							ctrl2=ctrl2.Parent;
					}

					bool bIsOnHandle=false;
					if(ctrl2!=null && objItem!=null)
						bIsOnHandle=objItem.IsAnyOnHandle(ctrl2.Handle.ToInt32());
					
					bool bNoEat=ctrl!=null && ctrl2!=null && ctrl.Handle==ctrl2.Handle || bIsOnHandle;

					if(!bIsOnHandle)
					{
						objItem.InternalKeyDown(new KeyEventArgs(key));
					}

					// Don't eat the message if the pop-up window has focus
					if(bNoEat)
						return false;
                    return true && !designMode;
				}
			}

			Form form=this.FindForm();
			if(form==null || form!=Form.ActiveForm && form.MdiParent==null ||
				form.MdiParent!=null && form.MdiParent.ActiveMdiChild!=form)
				return false;

			if(wParam.ToInt32()>=0x70 || System.Windows.Forms.Control.ModifierKeys!=Keys.None || (lParam.ToInt32() & 0x1000000000)!=0 || wParam.ToInt32()==0x2E || wParam.ToInt32()==0x2D) // 2E=VK_DELETE, 2D=VK_INSERT
			{
				int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
                return ProcessShortcut((eShortcut)i) && !designMode;
			}
			return false;
		}

        protected virtual bool IsContextPopup(BaseItem popup)
        {
            return false;
        }
		private bool ProcessShortcut(eShortcut key)
		{
			if(!IsHandlingShortcuts)
				return false;
			bool eat=BarFunctions.ProcessItemsShortcuts(key,m_ShortcutTable);
			return !m_DispatchShortcuts && eat;
//			if(m_ShortcutTable.Contains(key))
//			{
//				ShortcutTableEntry objEntry=(ShortcutTableEntry)m_ShortcutTable[key];
//				// Must convert to independable array, since if this is for example
//				// close command first Click will destroy the collection we are
//				// iterating through and exception will be raised.
//				BaseItem[] arr=new BaseItem[objEntry.Items.Values.Count];
//				objEntry.Items.Values.CopyTo(arr,0);
//				Hashtable hnames=new Hashtable(arr.Length);
//
//				bool eat=false;
//
//				foreach(BaseItem objItem in arr)
//				{
//					if(objItem.CanRaiseClick && (objItem.Name=="" || !hnames.Contains(objItem.Name)))
//					{
//						eat=true;
//						objItem.RaiseClick();
//						if(objItem.Name!="")
//							hnames.Add(objItem.Name,"");
//					}
//				}
//				return !m_DispatchShortcuts && eat; // True will eat the key, false will pass it through
//			}
//			return false;
		}
		private bool IsHandlingShortcuts
		{
			get
			{
				Form form=this.FindForm();
				Form activeForm=Form.ActiveForm;
				if(form==null)
					return false;

				if(form.IsMdiChild)
				{
					if(form.MdiParent!=null)
					{
						if(form.MdiParent.ActiveMdiChild==form && (form==activeForm || form.MdiParent==activeForm))
							return true;
						return false;
					}
				}

				return (form==Form.ActiveForm);
			}
		}
		bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			if(m_RegisteredPopups.Count==0 || this.DesignMode)
				return false;

			//foreach(PopupItem objPopup in m_RegisteredPopups)
			for(int i=m_RegisteredPopups.Count-1;i>=0;i--)
			{
				PopupItem objPopup=m_RegisteredPopups[i] as PopupItem;
				System.Windows.Forms.Control objCtrl=objPopup.ContainerControl as System.Windows.Forms.Control;
				bool bChildHandle=objPopup.IsAnyOnHandle(hWnd.ToInt32());

				if(!bChildHandle)
				{
					System.Windows.Forms.Control cTmp=System.Windows.Forms.Control.FromChildHandle(hWnd);
					if(cTmp!=null)
					{
                        if (cTmp is MenuPanel)
                        {
                            bChildHandle = true;
                        }
                        else
                        {
                            while (cTmp.Parent != null)
                            {
                                cTmp = cTmp.Parent;
                                if (cTmp.GetType().FullName.IndexOf("DropDownHolder") >= 0 || cTmp is MenuPanel || cTmp is PopupContainerControl)
                                {
                                    bChildHandle = true;
                                    break;
                                }
                            }
                        }
                        if(!bChildHandle)
						    bChildHandle=objPopup.IsAnyOnHandle(cTmp.Handle.ToInt32());
					}
                    else
                    {
                        string s = NativeFunctions.GetClassName(hWnd);
                        s = s.ToLower();
                        if (s.IndexOf("combolbox") >= 0)
                            bChildHandle = true;
                    }
				}

                if (!bChildHandle)
                {
                    Control popupContainer = objPopup.PopupControl;
                    if (popupContainer != null)
                        while (popupContainer.Parent != null) popupContainer = popupContainer.Parent;
                    if (popupContainer != null && popupContainer.Bounds.Contains(Control.MousePosition))
                        bChildHandle = true;
                }

				if(bChildHandle)
					break;

				if(objCtrl!=null && hWnd!=objCtrl.Handle && !bChildHandle)
				{
					objPopup.Expanded=!objPopup.Expanded;
					if(!objPopup.Expanded && objPopup.Parent!=null)
					{
						objPopup.Parent.AutoExpand=false;
					}
				}
				else if(objCtrl==null && !bChildHandle)
				{
					objPopup.ClosePopup();
				}

				if(m_RegisteredPopups.Count==0)
					break;
			}
			return false;
		}

		bool IMessageHandlerClient.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			if(m_RegisteredPopups.Count>0)
			{
				foreach(BaseItem item in m_RegisteredPopups)
				{
					if(item.Parent==null)
					{
						Control ctrl=((PopupItem)item).PopupControl;
						if(ctrl!=null && ctrl.Handle!=hWnd && !item.IsAnyOnHandle(hWnd.ToInt32()) && !(ctrl.Parent!=null && ctrl.Parent.Handle!=hWnd))
							return true;
					}
				}
			}
			return false;
		}

        protected virtual bool InternalSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (wParam.ToInt32() == 18 && System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Alt)
                this.ClosePopups();

            Form form = this.FindForm();
            if (form == null || !BarFunctions.IsFormActive(form))
                return false;

            Bar bar = null;
            if (this.MenuBar)
                bar = this;
            if (bar != null && bar.ItemsContainer != null && !bar.ItemsContainer.DesignMode)
            {
                GenericItemContainer cont = bar.ItemsContainer;
                if (cont == null)
                    return false;
                if (wParam.ToInt32() == 18 || (wParam.ToInt32() == 121 && !m_IgnoreF10Key && (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.None || System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Alt)))
                {
                    if (cont.ExpandedItem() != null && bar.Focused)
                    {
                        bar.ReleaseFocus();
                        m_IgnoreSysKeyUp = true;
                        return true;
                    }
                }
                else
                {
                    // Check Shortcuts
                    if (System.Windows.Forms.Control.ModifierKeys != Keys.None || wParam.ToInt32() >= (int)eShortcut.F1 && wParam.ToInt32() <= (int)eShortcut.F12)
                    {
                        int i = (int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
                        if (ProcessShortcut((eShortcut)i))
                            return true;
                    }
                    m_IgnoreSysKeyUp = true;
                    if (wParam.ToInt32() >= 27 && wParam.ToInt32() <= 111) // VK_ESC - VK_DIVIDE range
                    {
                        int key = (int)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                        if (key == 0)
                            key = wParam.ToInt32();
                        if (key > 0 && cont.SysKeyDown(key))
                            return true;
                    }
                }
            }
            else if (bar == null && !this.DesignMode)
            {
                // Check Shortcuts
                if (System.Windows.Forms.Control.ModifierKeys != Keys.None || wParam.ToInt32() >= (int)eShortcut.F1 && wParam.ToInt32() <= (int)eShortcut.F12)
                {
                    int i = (int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
                    if (ProcessShortcut((eShortcut)i))
                        return true;
                }
            }

            if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Alt && wParam.ToInt32() > 0x1B)
            {
                m_IgnoreSysKeyUp = true;
                int key = (int)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                if (key != 0)
                {
                    if (!this.DesignMode)
                    {
                        if (m_ItemContainer.SysKeyDown(key))
                            return true;
                    }
                }
            }
            return false;
        }

		bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            return InternalSysKeyDown(hWnd, wParam, lParam);
		}
		bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            Form form = this.FindForm();
            if (form == null || this.DesignMode || !BarFunctions.IsFormActive(form))
                return false;

			if(wParam.ToInt32()==18 || wParam.ToInt32()==121)
			{
				if(m_IgnoreSysKeyUp)
				{
					m_IgnoreSysKeyUp=false;
					return false;
				}
				if(m_EatSysKeyUp)
				{
					m_EatSysKeyUp=false;
					return true;
				}
				if(wParam.ToInt32()==18 || wParam.ToInt32()==121 && !m_IgnoreF10Key)
				{
					DevComponents.DotNetBar.Bar bar=null;
					if(this.MenuBar)
						bar=this;
					if(bar!=null && !bar.ItemsContainer.DesignMode)
					{
						if(bar.Focused)
							bar.ReleaseFocus();
						else
							bar.SetSystemFocus();
						return true;
					}
				}
			}
			return false;
		}
		#endregion

		#region BarAccessibleObject
		public class BarAccessibleObject : System.Windows.Forms.Control.ControlAccessibleObject
		{
			Bar m_Owner = null;
			public BarAccessibleObject(Bar owner):base(owner)
			{
				m_Owner = owner;
			}

			internal void GenerateEvent(BaseItem sender, System.Windows.Forms.AccessibleEvents e)
			{
				int	iChild = m_Owner.Items.IndexOf(sender);
				if(iChild>=0)
				{
					if(m_Owner!=null && !m_Owner.IsDisposed)
						m_Owner.AccessibilityNotifyClients(e,iChild);
				}
			}

            //public override string Name 
            //{
            //    get
            //    {
            //        if(m_Owner!=null && !m_Owner.IsDisposed)
            //            return m_Owner.AccessibleName;
            //        return "";
            //    }
            //    set
            //    {
            //        if(m_Owner!=null && !m_Owner.IsDisposed)
            //            m_Owner.AccessibleName = value;
            //    }
            //}

            //public override string Description
            //{
            //    get
            //    {
            //        if(m_Owner!=null && !m_Owner.IsDisposed)
            //            return m_Owner.AccessibleDescription;
            //        return "";
            //    }
            //}

			public override AccessibleRole Role
			{
				get
				{
					if(m_Owner!=null && !m_Owner.IsDisposed)
						return m_Owner.AccessibleRole;
					return System.Windows.Forms.AccessibleRole.None;
				}
			}

			/*			public override AccessibleStates State
						{
							get { return m_owner; }
							set { m_owner.AccessibleRole = value; }
						}*/

			/*			public override string Value
						{
							get { return m_owner.AccessibleRole; }
							set { m_owner.AccessibleRole = value; }
						}*/

            public override AccessibleObject Parent
            {
                get
                {
                    if (m_Owner != null && m_Owner.Parent is DockSite)
                        return m_Owner.Parent.AccessibilityObject;
                    return base.Parent;
                }
            }

			public override Rectangle Bounds 
			{
				get
				{
					if(m_Owner!=null && !m_Owner.IsDisposed && m_Owner.Parent!=null)
						return this.m_Owner.Parent.RectangleToScreen(m_Owner.Bounds);
					return Rectangle.Empty;
				}
			}

			public override int GetChildCount()
			{
				if(m_Owner!=null && !m_Owner.IsDisposed && m_Owner.Items!=null)
					return m_Owner.Items.Count;
				return 0;
			}

			public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
			{
				if(m_Owner!=null && !m_Owner.IsDisposed && m_Owner.Items!=null)
					return m_Owner.Items[iIndex].AccessibleObject;
				return null;
			}

			public override AccessibleStates State
			{
				get
				{
					AccessibleStates state;
					if(m_Owner==null || m_Owner.IsDisposed)
                        return AccessibleStates.None;

					if(m_Owner.GrabHandleStyle!=eGrabHandleStyle.None && m_Owner.GrabHandleStyle!=eGrabHandleStyle.ResizeHandle)
					{
						state=AccessibleStates.Moveable;
						if(m_Owner.DockSide==eDockSide.None)
							state=state | AccessibleStates.Floating;
					}
					else
						state=AccessibleStates.None;
					return state;
				}
			}

            public override AccessibleObject Navigate(AccessibleNavigation navdir)
            {
                if (navdir == AccessibleNavigation.FirstChild)
                {
                    if (m_Owner.Items.Count > 0) return m_Owner.Items[0].AccessibleObject;
                }
                else if (navdir == AccessibleNavigation.LastChild)
                {
                    if (m_Owner.Items.Count > 0) return m_Owner.Items[m_Owner.Items.Count - 1].AccessibleObject;
                }
                else if (navdir == AccessibleNavigation.Next)
                {
                    return null;
                    Control parent = m_Owner.Parent;
                    if (parent != null && parent.Controls.IndexOf(m_Owner) < parent.Controls.Count - 1)
                    {
                        return parent.Controls[parent.Controls.IndexOf(m_Owner) + 1].AccessibilityObject;
                    }
                }
                else if (navdir == AccessibleNavigation.Previous)
                {
                    return null;
                    Control parent = m_Owner.Parent;
                    if (parent != null && parent.Controls.IndexOf(m_Owner) > 0)
                    {
                        return parent.Controls[parent.Controls.IndexOf(m_Owner) - 1].AccessibilityObject;
                    }
                }
                else if (navdir == AccessibleNavigation.Down)
                {
                    return null;
                    if (m_Owner.Items.Count > 0) return m_Owner.Items[0].AccessibleObject;
                }
                else if (navdir == AccessibleNavigation.Up)
                {
                    return null;
                    if (m_Owner.Parent != null) return m_Owner.Parent.AccessibilityObject;
                }
                return base.Navigate(navdir);
            }
		}
		#endregion

		#region IDockInfo
		/// <summary>
		/// Specifes whether Bar can be docked on Top dock site or not. Does not apply to stand alone bars.
		/// </summary>
		bool IDockInfo.CanDockTop
		{
			get	{return this.CanDockTop;}
		}

		/// <summary>
		/// Specifes whether Bar can be docked on Bottom dock site or not. Does not apply to stand alone bars.
		/// </summary>
		bool IDockInfo.CanDockBottom
		{
			get	{return this.CanDockBottom;}
		}

		/// <summary>
		/// Specifes whether Bar can be docked on Left dock site or not. Does not apply to stand alone bars.
		/// </summary>
		bool IDockInfo.CanDockLeft
		{
			get {return this.CanDockLeft;}
		}

		/// <summary>
		/// Specifes whether Bar can be docked on Right dock site or not. Does not apply to stand alone bars.
		/// </summary>
		bool IDockInfo.CanDockRight
		{
			get {return this.CanDockRight;}
		}

		/// <summary>
		/// Specifes whether Bar can be docked as document. Default value is false. See DotNetBarManager.EnableDocumentDocking for more details.
		/// </summary>
		bool IDockInfo.CanDockDocument
		{
			get {return this.CanDockDocument;}
		}

		/// <summary>
		/// Returns Minimum docked size of the control.
		/// </summary>
		System.Drawing.Size IDockInfo.MinimumDockSize(eOrientation dockOrientation)
		{
			return this.MinimumDockSize(dockOrientation);
	}
		/// <summary>
		/// Returns Preferrred size of the docked control.
		/// </summary>
		System.Drawing.Size IDockInfo.PreferredDockSize(eOrientation dockOrientation)
		{
			return this.PreferredDockSize(dockOrientation);
		}

		/// <summary>
		///  Indicated whether control can be stretched to fill dock site.
		/// </summary>
		bool IDockInfo.Stretch
		{
			get{return this.Stretch;}
			set{this.Stretch=value;}
		}
		/// <summary>
		/// Holds the left position (dock offset) of the control.
		/// </summary>
		int IDockInfo.DockOffset
		{
			get{return this.DockOffset;}
			set{this.DockOffset=value;}
		}
		/// <summary>
		/// Specifies the dock line for the control.
		/// </summary>
		int IDockInfo.DockLine
		{
			get{return this.DockLine;}
			set{this.DockLine=value;}
		}
		/// <summary>
		/// Specifies current dock orientation.
		/// </summary>
		eOrientation IDockInfo.DockOrientation
		{
			get{return this.DockOrientation;}
			set{this.DockOrientation=value;}
		}
		/// <summary>
		/// Gets whether control is docked.
		/// </summary>
		bool IDockInfo.Docked
		{
			get{return this.Docked;}
		}
		/// <summary>
		/// Returns the dock site of the control.
		/// </summary>
		System.Windows.Forms.Control IDockInfo.DockedSite
		{
			get{return this.DockedSite;}
		}
		/// <summary>
		/// Gets or sets the control dock side.
		/// </summary>
		eDockSide IDockInfo.DockSide
		{
			get{return this.DockSide;}
			set{this.DockSide=value;}
		}
		/// <summary>
		/// Sets the dock line for the control. Used internaly by dock manager.
		/// </summary>
		/// <param name="iLine">New Dock line.</param>
		void IDockInfo.SetDockLine(int iLine)
		{
			this.SetDockLine(iLine);
		}
		/// <summary>
		/// Gets or sets whether bar is locked to prevent docking below it.
		/// </summary>
		bool IDockInfo.LockDockPosition
		{
			get{return this.LockDockPosition;}
			set{this.LockDockPosition=value;}
		}
		#endregion

        #region Design Time Drag & Drop
        private bool m_DragDropInProgress = false;
        private int m_InsertPosition = -1;
        private bool m_InsertBefore = false;
        private IDesignTimeProvider m_DesignTimeProvider = null;
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
            
            if (m_ParentItem != null && m_ParentItem.ContainerControl is Control && (c==null || c!=null && c.Site == null))
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
                if (m_Owner is IOwner)
                    m_DragItem = ((IOwner)m_Owner).GetFocusItem();
                if (m_DragItem != null)
                {
                    // Get top level design-time provider
                    BaseItem item = m_ParentItem;
                    while (item.Parent is IDesignTimeProvider)
                        item = item.Parent;
                    m_DragDropDesignTimeProvider = (IDesignTimeProvider)item;

                    m_DragDropInProgress = true;
                    this.Capture = true;
                }
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
            BaseItem objNew = m_ItemContainer.ItemAtLocation(e.X, e.Y);
            // Ignore system items
            if (objNew != null && objNew.SystemItem)
                objNew = null;

            if (objNew == null)
                return;

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

        #region IOwnerLocalize Implementation
        void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
        {
            if (LocalizeString != null)
                LocalizeString(this, e);
        }
        #endregion
    }

	#region FloatingContainer
	[ToolboxItem(false)]
	internal class FloatingContainer:Form
	{
		const int WM_MOUSEACTIVATE = 0x21;
		const int MA_NOACTIVATE = 3;
		//const int MA_NOACTIVATEANDEAT = 4;
		Bar m_Bar;
		private Size m_OldSize=Size.Empty;
		public FloatingContainer(Bar objBar) {
			m_Bar = objBar;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            IOwner owner = objBar.Owner as IOwner;
            if (owner != null)
                this.Owner = owner.ParentForm;
            this.StartPosition = FormStartPosition.Manual;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
		}
		private bool m_CodeClose=false;
		public new void Close()
		{
			m_CodeClose=true;
			base.Close();
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			if(this.Visible)
			{
				if(!m_CodeClose)
				{
					e.Cancel=true;
                    if (m_Bar != null)
                        m_Bar.CloseBar();
				}
			}
			base.OnClosing(e);
		}
		public void RefreshRegion(System.Drawing.Size sz)
		{
            if (m_Bar == null || m_OldSize == sz)
                return;

            if (!m_Bar.IsThemed)
                return;

            m_OldSize = sz;

            ThemeWindow theme = m_Bar.ThemeWindow;
            Graphics g = this.CreateGraphics();
            try
            {
                IntPtr r = theme.GetThemeBackgroundRegion(g, ThemeWindowParts.SmallCaption, ThemeWindowStates.CaptionActive, new Rectangle(0, 0, sz.Width, sz.Height));
                if (r != IntPtr.Zero)
                {
                    NativeFunctions.SetWindowRgn(this.Handle, r, true);
                }
            }
            finally
            {
                g.Dispose();
            }
		}
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_MOUSEACTIVATE && m_Bar.LayoutType!=eLayoutType.DockContainer)
			{
				m.Result=new System.IntPtr(MA_NOACTIVATE);
				return;
			}
			base.WndProc(ref m);
		}
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if(m_Bar!=null && m_Bar.LayoutType==eLayoutType.DockContainer)
			{
				m_Bar.SetHasFocus(true);
			}
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
			if(m_Bar!=null && m_Bar.LayoutType==eLayoutType.DockContainer)
			{
				m_Bar.SetHasFocus(false);
			}
		}
	}
	#endregion

    #region Custom Rendering Support
    /// <summary>
    /// Defines delegate for the PreRender and PostRender Bar control events.
    /// </summary>
    public delegate void RenderBarEventHandler(object sender, RenderBarEventArgs e);

    /// <summary>
    /// Represents event arguments for PreRender and PostRender Bar control event.
    /// </summary>
    public class RenderBarEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the reference to the Bar being rendered.
        /// </summary>
        public readonly Bar Bar;

        /// <summary>
        /// Gets or sets the rectangle of the part being rendered. Certain parts of bar like the title buttons allow you to set this property to the custom size of your button.
        /// Default value is the system size of the part being rendered.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;

        /// <summary>
        /// Gets the Bar part being rendered.
        /// </summary>
        public readonly eBarRenderPart Part = eBarRenderPart.Background;

        /// <summary>
        /// When used in PreRender event allows you to cancel the default rendering by setting this property to true.
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        /// Gets the reference to the Graphics object to render the tab on.
        /// </summary>
        public readonly Graphics Graphics;

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        public RenderBarEventArgs(Bar bar, Graphics g, eBarRenderPart part, Rectangle bounds)
        {
            this.Bar = bar;
            this.Part = part;
            this.Bounds = bounds;
            this.Graphics = g;
        }
    }

    /// <summary>
    /// Defines the part of the Bar control for custom rendering.
    /// </summary>
    public enum eBarRenderPart
    {
        /// <summary>
        /// Indicates the Bar background and border.
        /// </summary>
        Background,
        /// <summary>
        /// Indicates the Bar caption.
        /// </summary>
        Caption,
        /// <summary>
        /// Indicates the Bar close button displayed inside of caption.
        /// </summary>
        CloseButton,
        /// <summary>
        /// Indicates the Bar customize button displayed inside of caption.
        /// </summary>
        CustomizeButton,
        /// <summary>
        /// Indicates the Bar caption text.
        /// </summary>
        CaptionText,
        /// <summary>
        /// Indicates the Bar grab handle.
        /// </summary>
        GrabHandle,
        /// <summary>
        /// Indicates the Bar resize handle.
        /// </summary>
        ResizeHandle,
        /// <summary>
        /// Indicates the Bar auto-hide button displayed inside of caption.
        /// </summary>
        AutoHideButton,
        /// <summary>
        /// Indicates the Bar caption task pane.
        /// </summary>
        CaptionTaskPane,
        /// <summary>
        /// Indicates the complete bar area. This part is used for the PostRender event.
        /// </summary>
        All

    }
    #endregion
}
