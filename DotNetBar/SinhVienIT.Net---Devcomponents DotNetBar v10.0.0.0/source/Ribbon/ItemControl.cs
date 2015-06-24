using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents base control for bars.
	/// </summary>
    [ToolboxItem(false), ComVisible(false), DefaultEvent("ItemClick")]
	public abstract class ItemControl: ContainerControl,
		IOwner, IOwnerMenuSupport, IMessageHandlerClient, IOwnerItemEvents,
        IThemeCache, IOwnerLocalize, IBarDesignerServices,
        IKeyTipsRenderer, IRenderingSupport, IAccessibilitySupport, IRibbonCustomize, IBarImageSize, IKeyTipsControl
	{
		// Events
		#region Event Definition
        /// <summary>
        /// Occurs when Checked property of an button has changed.
        /// </summary>
        public event EventHandler ButtonCheckedChanged;

		/// <summary>
		/// Represents the method that will handle the ItemRemoved event.
		/// </summary>
		public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

		/// <summary>
		/// Occurs when Item is clicked.
		/// </summary>
		[Description("Occurs when Item is clicked.")]
		public event EventHandler ItemClick;

        /// <summary>
        /// Occurs when Item is clicked.
        /// </summary>
        [Description("Occurs when Item is double-clicked.")]
        public event MouseEventHandler ItemDoubleClick;

		/// <summary>
		/// Occurs when popup of type container is loading.
		/// </summary>
		[Description("Occurs when popup of type container is loading.")]
		public event EventHandler PopupContainerLoad;

		/// <summary>
		/// Occurs when popup of type container is unloading.
		/// </summary>
		[Description("Occurs when popup of type container is unloading.")]
		public event EventHandler PopupContainerUnload;

		/// <summary>
		/// Occurs when popup item is about to open.
		/// </summary>
		[Description("Occurs when popup item is about to open.")]
		public event EventHandler PopupOpen;

		/// <summary>
		/// Occurs when popup item is closing.
		/// </summary>
		[Description("Occurs when popup item is closing.")]
		public event EventHandler PopupClose;

		/// <summary>
		/// Occurs just before popup window is shown.
		/// </summary>
		[Description("Occurs just before popup window is shown.")]
		public event EventHandler PopupShowing;

		/// <summary>
		/// Occurs when Item Expanded property has changed.
		/// </summary>
		[Description("Occurs when Item Expanded property has changed.")]
		public event EventHandler ExpandedChange;

		private MouseEventHandler EventMouseDown;
		/// <summary>
		/// Occurs when mouse button is pressed.
		/// </summary>
		[Description("Occurs when mouse button is pressed.")]
		new public event MouseEventHandler MouseDown 
		{
			add { EventMouseDown += value; }
			remove { EventMouseDown -= value; }
		}

		private MouseEventHandler EventMouseUp;
		/// <summary>
		/// Occurs when mouse button is released.
		/// </summary>
		[Description("Occurs when mouse button is released.")]
		new public event MouseEventHandler MouseUp
		{
			add { EventMouseUp += value; }
			remove { EventMouseUp -= value; }
		}

		private EventHandler EventMouseEnter;
		/// <summary>
		/// Occurs when mouse enters the item.
		/// </summary>
		[Description("Occurs when mouse enters the item.")]
		new public event EventHandler MouseEnter
		{
			add { EventMouseEnter += value; }
			remove { EventMouseEnter -= value; }
		}
		
		private EventHandler EventMouseLeave;
		/// <summary>
		/// Occurs when mouse leaves the item.
		/// </summary>
		[Description("Occurs when mouse leaves the item.")]
		new public event EventHandler MouseLeave
		{
			add { EventMouseLeave += value; }
			remove { EventMouseLeave -= value; }
		}

		private MouseEventHandler EventMouseMove;
		/// <summary>
		/// Occurs when mouse moves over the item.
		/// </summary>
		[Description("Occurs when mouse moves over the item.")]
		new public event MouseEventHandler MouseMove
		{
			add { EventMouseMove += value; }
			remove { EventMouseMove -= value; }
		}
		
		private EventHandler EventMouseHover;
		/// <summary>
		/// Occurs when mouse remains still inside an item for an amount of time.
		/// </summary>
		[Description("Occurs when mouse remains still inside an item for an amount of time.")]
		new public event EventHandler MouseHover
		{
			add { EventMouseHover += value; }
			remove { EventMouseHover -= value; }
		}
		
		private EventHandler EventLostFocus;
		/// <summary>
		/// Occurs when item loses input focus.
		/// </summary>
		[Description("Occurs when item loses input focus.")]
		new public event EventHandler LostFocus
		{
			add { EventLostFocus += value; }
			remove { EventLostFocus -= value; }
		}
		
		private EventHandler EventGotFocus;
		/// <summary>
		/// Occurs when item receives input focus.
		/// </summary>
		[Description("Occurs when item receives input focus.")]
		new public event EventHandler GotFocus
		{
			add { EventGotFocus += value; }
			remove { EventGotFocus -= value; }
		}
		
		/// <summary>
		/// Occurs when user changes the item position, removes the item, adds new item or creates new bar.
		/// </summary>
		[Description("Occurs when user changes the item position.")]
		public event EventHandler UserCustomize;

		/// <summary>
		/// Occurs after an Item is removed from SubItemsCollection.
		/// </summary>
		[Description("Occurs after an Item is removed from SubItemsCollection.")]
		public event ItemRemovedEventHandler ItemRemoved;

		/// <summary>
		/// Occurs after an Item has been added to the SubItemsCollection.
		/// </summary>
		[Description("Occurs after an Item has been added to the SubItemsCollection.")]
		public event EventHandler ItemAdded;

		/// <summary>
		/// Occurs when ControlContainerControl is created and contained control is needed.
		/// </summary>
		[Description("Occurs when ControlContainerControl is created and contained control is needed.")]
		public event EventHandler ContainerLoadControl;

		/// <summary>
		/// Occurs when Text property of an Item has changed.
		/// </summary>
		[Description("Occurs when Text property of an Item has changed.")]
		public event EventHandler ItemTextChanged;

		/// <summary>
		/// Use this event if you want to serialize the hosted control state directly into the DotNetBar definition file.
		/// </summary>
		public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize;
		/// <summary>
		/// Use this event if you want to deserialize the hosted control state directly from the DotNetBar definition file.
		/// </summary>
		public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize;

		/// <summary>
		/// Occurs after DotNetBar definition is loaded.
		/// </summary>
		[Description("Occurs after DotNetBar definition is loaded.")]
		public event EventHandler DefinitionLoaded;

		/// <summary>
		/// Occurs when DotNetBar is looking for translated text for one of the internal text that are
		/// displayed on menus, toolbars and customize forms. You need to set Handled=true if you want
		/// your custom text to be used instead of the built-in system value.
		/// </summary>
		public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

		/// <summary>
		/// Occurs before an item in option group is checked and provides opportunity to cancel that.
		/// </summary>
		public event OptionGroupChangingEventHandler OptionGroupChanging;

		/// <summary>
		/// Occurs before tooltip for an item is shown. Sender could be the BaseItem or derived class for which tooltip is being displayed or it could be a ToolTip object itself it tooltip is not displayed for any item in particular.
		/// </summary>
		public event EventHandler ToolTipShowing;
		#endregion

		#region Private Variables
		private BaseItem m_BaseItemContainer=null;
		private BaseItem m_ExpandedItem=null;
		private BaseItem m_FocusItem=null;
		private Hashtable m_ShortcutTable=new Hashtable();
		private ImageList m_ImageList;
		private ImageList m_ImageListMedium=null;
		private ImageList m_ImageListLarge=null;
		private BaseItem m_DragItem=null;
		private bool m_DragDropSupport=false;
		private bool m_DragLeft=false;
		private bool m_AllowExternalDrop=false;
		private bool m_UseNativeDragDrop=false;
		private bool m_DragInProgress=false;
		private bool m_ExternalDragInProgress=false;
		private Cursor m_MoveCursor /*, m_CopyCursor*/, m_NACursor;
		private bool m_ShowToolTips=true;
		private bool m_ShowShortcutKeysInToolTips=false;
		private bool m_FilterInstalled=false;
		private bool m_DispatchShortcuts=false;
		private bool m_MenuEventSupport=false;
		private IDesignTimeProvider m_DesignTimeProvider=null;
		private int m_InsertPosition;
		private bool m_InsertBefore=false;
		private Timer m_ClickTimer=null;
		private BaseItem m_ClickRepeatItem=null;
		// Theme Caching Support
		private ThemeWindow m_ThemeWindow=null;
		private ThemeRebar m_ThemeRebar=null;
		private ThemeToolbar m_ThemeToolbar=null;
		private ThemeHeader m_ThemeHeader=null;
		private ThemeScrollBar m_ThemeScrollBar=null;
		private ThemeExplorerBar m_ThemeExplorerBar=null;
		private ThemeProgress m_ThemeProgress=null;
        private ThemeButton m_ThemeButton = null;

		private ColorScheme m_ColorScheme=null;
		private bool m_ThemeAware=false;
		private string m_EmptyContainerDesignTimeHint="Right-click to add more items...";
		private ElementStyle m_BackgroundStyle=new ElementStyle();
		private bool m_DesignModeInternal=false;
		private bool m_AntiAlias=true;
		private bool m_AutoSize=false;
		private bool m_DesignerSelection=false;
		private IBarItemDesigner m_BarDesigner=null;
        private bool m_ShowKeyTips = false;
        private Font m_KeyTipsFont = null;
        private string m_KeyTipsKeysStack = "";
        private KeyTipsCanvasControl m_KeyTipsCanvas = null;
        private bool m_MenuFocus = false;
        private int m_LastFocusWindow = 0;

        private Timer m_ActiveWindowTimer = null;
        private IntPtr m_ForegroundWindow = IntPtr.Zero;
        private IntPtr m_ActiveWindow = IntPtr.Zero;
        private bool m_FadeEffect = true;
        private BaseItem m_DoDefaultActionItem = null;
        private eBarImageSize m_ImageSize = eBarImageSize.Default;
        private bool m_ShortcutsEnabled = true;
        private bool m_UseHook = false;
		#endregion

		#region Constructor
		public ItemControl()
		{
            // This forces the initialization out of paint loop which speeds up how fast components show up
            BaseRenderer renderer = Rendering.GlobalManager.Renderer;

			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}

			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			m_MoveCursor=null;
			m_NACursor=null;

			m_ColorScheme=new ColorScheme(eDotNetBarStyle.Office2003);

			m_BackgroundStyle.SetColorScheme(m_ColorScheme);
			m_BackgroundStyle.StyleChanged+=new EventHandler(this.VisualPropertyChanged);

			this.IsAccessible=true;
		}

		protected bool GetDesignMode()
		{
			if(!m_DesignModeInternal)
				return this.DesignMode;
			return m_DesignModeInternal;
		}
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public void SetDesignMode(bool mode)
		{
			m_DesignModeInternal=mode;
			m_BaseItemContainer.SetDesignMode(mode);
		}

		/// <summary>
		/// Creates new accessibility instance.
		/// </summary>
		/// <returns>Reference to AccessibleObject.</returns>
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ItemControlAccessibleObject(this);
		}

		/// <summary>
		/// Notifies the accessibility client applications of the specified AccessibleEvents for the specified child control.
		/// </summary>
		/// <param name="accEvent">The AccessibleEvents object to notify the accessibility client applications of. </param>
		/// <param name="childID">The child Control to notify of the accessible event.</param>
		internal void InternalAccessibilityNotifyClients(AccessibleEvents accEvent,int childID)
		{
			this.AccessibilityNotifyClients(accEvent,childID);
		}
		#endregion

		#region IOwner Implementation
		/// <summary>
		/// Gets or sets the form SideBar is attached to.
		/// </summary>
		Form IOwner.ParentForm
		{
			get
			{
				return base.FindForm();
			}
			set {}
		}

		/// <summary>
		/// Returns the collection of items with the specified name.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <returns></returns>
		public virtual ArrayList GetItems(string ItemName)
		{
			ArrayList list=new ArrayList(15);
			BarFunctions.GetSubItemsByName(m_BaseItemContainer,ItemName,list);
			return list;
		}

		/// <summary>
		/// Returns the collection of items with the specified name and type.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <param name="itemType">Item type to look for.</param>
		/// <returns></returns>
		public virtual ArrayList GetItems(string ItemName, Type itemType)
		{
			ArrayList list=new ArrayList(15);
			BarFunctions.GetSubItemsByNameAndType(m_BaseItemContainer,ItemName,list,itemType);
			return list;
		}

        /// <summary>
        /// Returns the collection of items with the specified name and type.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <param name="useGlobalName">Indicates whether GlobalName property is used for searching.</param>
        /// <returns></returns>
        public virtual ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByNameAndType(m_BaseItemContainer, ItemName, list, itemType, useGlobalName);
            return list;
        }

		/// <summary>
		/// Returns the first item that matches specified name.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <returns></returns>
		public virtual BaseItem GetItem(string ItemName)
		{
			BaseItem item=BarFunctions.GetSubItemByName(m_BaseItemContainer,ItemName);
			if(item!=null)
				return item;
			return null;
		}

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
		void IOwner.SetFocusItem(BaseItem objFocusItem)
		{
			if(m_FocusItem!=null && m_FocusItem!=objFocusItem)
			{
				m_FocusItem.OnLostFocus();
			}
			OnSetFocusItem(objFocusItem);
			m_FocusItem=objFocusItem;
			if(m_FocusItem!=null)
				m_FocusItem.OnGotFocus();
		}

		protected virtual void OnSetFocusItem(BaseItem objFocusItem)
		{
		}

		BaseItem IOwner.GetFocusItem()
		{
			return m_FocusItem;
		}

		void IOwner.DesignTimeContextMenu(BaseItem objItem)
		{
		}

		bool IOwner.DesignMode
		{
			get {return this.GetDesignMode();}
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
						catch(ArgumentException)	{}
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
					catch(ArgumentException)	{}
				}
			}
			IOwner owner=this as IOwner;
			foreach(BaseItem objTmp in objItem.SubItems)
				owner.AddShortcutsFromItem(objTmp);
		}

		Form IOwner.ActiveMdiChild
		{
			get
			{
				Form form=base.FindForm();
				if(form==null)
					return null;
				if(form.IsMdiContainer)
				{
					return form.ActiveMdiChild;
				}
				return null;
			}
		}

        private bool m_AlwaysDisplayKeyAccelerators = false;
        /// <summary>
        /// Gets or sets whether accelerator letters on buttons are underlined. Default value is false which indicates that system setting is used
        /// to determine whether accelerator letters are underlined. Setting this property to true
        /// will always display accelerator letter underlined.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Run-time Behavior"), Description("Indicates whether accelerator letters for buttons are underlined regardless of current Windows settings.")]
        public bool AlwaysDisplayKeyAccelerators
        {
            get { return m_AlwaysDisplayKeyAccelerators; }
            set
            {
                if (m_AlwaysDisplayKeyAccelerators != value)
                {
                    m_AlwaysDisplayKeyAccelerators = value;
                    this.Invalidate();
                }
            }
        }

		/// <summary>
		/// Invokes the DotNetBar Customize dialog.
		/// </summary>
		void IOwner.Customize()
		{
		}

		void IOwner.InvokeResetDefinition(BaseItem item,EventArgs e)
		{
		}

		/// <summary>
		/// Indicates whether Reset buttons is shown that allows end-user to reset the toolbar state.
		/// </summary>
		bool IOwner.ShowResetButton
		{
			get{return false;}
			set {}
		}

		void IOwner.OnApplicationActivate(){}
		void IOwner.OnApplicationDeactivate()
		{
            ClosePopups();

            if (m_BaseItemContainer is GenericItemContainer)
            {
                ((GenericItemContainer)m_BaseItemContainer).ContainerLostFocus(true);
            }
            else if (m_BaseItemContainer is RibbonStripContainerItem)
            {
                ((RibbonStripContainerItem)m_BaseItemContainer).ContainerLostFocus(true);
            }
		}
		void IOwner.OnParentPositionChanging(){}

        /// <summary>
        /// Starts the drag & drop if enabled and supported by the control.
        /// </summary>
        /// <param name="item">Reference to the item that belongs to the control.</param>
        public void StartItemDrag(BaseItem item)
		{
			if(!m_DragDropSupport)
				return;

			if(m_DragItem==null)
			{
				m_DragItem=item;
				if(!m_UseNativeDragDrop)
				{
					this.Capture=true;
					if(m_MoveCursor!=null)
						Cursor.Current=m_MoveCursor;
					else
						Cursor.Current=Cursors.Hand;
					m_DragInProgress=true;
				}
				else
				{
					m_DragInProgress=true;
                    this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move | DragDropEffects.Scroll);
                    //if(m_DragInProgress)
                    //    MouseDragDrop(-1,-1,null);
				}
					
			}
		}

        /// <summary>
        /// Gets whether drag & drop operation is current being performed by the control.
        /// </summary>
        [Browsable(false)]
		public bool DragInProgress
		{
			get{ return m_DragInProgress;}
		}

		BaseItem IOwner.DragItem
		{
			get {return m_DragItem;}
		}

		void IOwner.InvokeUserCustomize(object sender,EventArgs e)
		{
			if(UserCustomize!=null)
				UserCustomize(sender,e);
		}

		void IOwner.InvokeEndUserCustomize(object sender,EndUserCustomizeEventArgs e){}

		MdiClient IOwner.GetMdiClient(Form MdiForm)
		{
			return BarFunctions.GetMdiClient(MdiForm);
		}

		/// <summary>
		/// ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.
		/// </summary>
		[Browsable(true),Category("Data"),DefaultValue(null),Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
		public virtual ImageList Images
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
		[Browsable(true),Category("Data"),DefaultValue(null),Description("ImageList for medium-sized images used on Items.")]
		public virtual ImageList ImagesMedium
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
		[Browsable(true),Category("Data"),DefaultValue(null),Description("ImageList for large-sized images used on Items.")]
		public virtual ImageList ImagesLarge
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
				m_ImageList=null;
			}
			else if(sender==m_ImageListLarge)
			{
				m_ImageListLarge=null;
			}
			else if(sender==m_ImageListMedium)
			{
				m_ImageListMedium=null;
			}
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if(this.Parent!=null && (this.Images!=null || this.ImagesLarge!=null || this.ImagesMedium!=null))
			{
				foreach(BaseItem panel in m_BaseItemContainer.SubItems)
				{
					foreach(BaseItem item in panel.SubItems)
					{
						if(item is ImageItem)
							((ImageItem)item).OnImageChanged();
					}
				}
			}

			if(this.DesignMode)
				m_BaseItemContainer.SetDesignMode(this.DesignMode);
		}

		void IOwner.InvokeDefinitionLoaded(object sender,EventArgs e)
		{
			if(DefinitionLoaded!=null)
				DefinitionLoaded(sender,e);
		}

		/// <summary>
		/// Indicates whether Tooltips are shown on Bars and menus.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Run-time Behavior"),Description("Indicates whether Tooltips are shown on Bars and menus.")]
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
		/// Indicates whether item shortcut is displayed in Tooltips.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Run-time Behavior"),Description("Indicates whether item shortcut is displayed in Tooltips.")]
		public virtual bool ShowShortcutKeysInToolTips
		{
			get
			{
				return m_ShowShortcutKeysInToolTips;
			}
			set
			{
				m_ShowShortcutKeysInToolTips=value;
			}
		}


		#endregion

		#region IOwnerMenuSupport Implementation
        private Hook m_Hook = null;
		// IOwnerMenuSupport
		private ArrayList m_RegisteredPopups=new ArrayList();
		bool IOwnerMenuSupport.PersonalizedAllVisible {get{return false;}set{}}
		bool IOwnerMenuSupport.ShowFullMenusOnHover {get{return true;}set{}}
		bool IOwnerMenuSupport.AlwaysShowFullMenus {get{return false;}set{}}
        internal void InstallMessageFilter()
        {
            if (!m_FilterInstalled)
            {
                MessageHandler.RegisterMessageClient(this);
                m_FilterInstalled = true;
            }
        }

        /// <summary>
		/// Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.
		/// </summary>
        [System.ComponentModel.Browsable(false), DefaultValue(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
        public bool UseHook
        {
            get
            {
                return m_UseHook;
            }
            set
            {
                if (m_UseHook == value)
                    return;
                m_UseHook = value;
            }
        }

		void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
		{
			if(m_RegisteredPopups.Contains(objPopup))
				return;

            if (!this.GetDesignMode() && !m_UseHook)
            {
                InstallMessageFilter();

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
				if(ctrl!=null && ctrl.DisplayRectangle.Contains(MousePosition))
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
		eMenuDropShadow IOwnerMenuSupport.MenuDropShadow{get{return eMenuDropShadow.Hide;}set{}}
		ePopupAnimation IOwnerMenuSupport.PopupAnimation{get {return ePopupAnimation.SystemDefault;}set{}}
		bool IOwnerMenuSupport.AlphaBlendShadow{get {return true;}set{}}
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

        internal void ClosePopup(string popupName)
        {
            ArrayList popupList = new ArrayList(m_RegisteredPopups);
            foreach (PopupItem objPopup in popupList)
            {
                if (objPopup.Name == popupName)
                    objPopup.ClosePopup();
            }
        }

        /// <summary>
        /// Gets whether control is in menu mode.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasMenuFocus
        {
            get
            {
                return MenuFocus;
            }
        }

        internal virtual bool MenuFocus
        {
            get
            {
                return m_MenuFocus;
            }
            set
            {
                if (m_MenuFocus != value)
                {
                    SetMenuFocus(value);
                    if (m_MenuFocus)
                    {
                        if(m_BaseItemContainer is GenericItemContainer)
                            ((GenericItemContainer)m_BaseItemContainer).SetSystemFocus();
                        else if (m_BaseItemContainer is RibbonStripContainerItem)
                            ((RibbonStripContainerItem)m_BaseItemContainer).SetSystemFocus();
                        SetupActiveWindowTimer();
                    }
                    else
                    {
                        ReleaseActiveWindowTimer();
                        if (m_BaseItemContainer is GenericItemContainer)
                        {
                            ((GenericItemContainer)m_BaseItemContainer).AutoExpand = false;
                            ((GenericItemContainer)m_BaseItemContainer).ReleaseSystemFocus();
                            ((GenericItemContainer)m_BaseItemContainer).ContainerLostFocus(false);
                        }
                        else if (m_BaseItemContainer is RibbonStripContainerItem)
                        {
                            ((RibbonStripContainerItem)m_BaseItemContainer).AutoExpand = false;
                            ((RibbonStripContainerItem)m_BaseItemContainer).ReleaseSystemFocus();
                            ((RibbonStripContainerItem)m_BaseItemContainer).ContainerLostFocus(false);
                        }
                    }
                    this.Refresh();
                }
            }
        }

        internal void SetMenuFocus(bool focus)
        {
            m_MenuFocus = focus;
        }

        /// <summary>
        /// Releases the focus from the bar and selects the control that had focus before bar was selected. If control that had focus could not be determined focus will stay on the bar.
        /// This method is used by internal DotNetBar implementation and you should not use it.
        /// </summary>
        public void ReleaseFocus()
        {
            if (this.Focused && m_LastFocusWindow != 0)
            {
                Control ctrl = Control.FromChildHandle(new System.IntPtr(m_LastFocusWindow));
                if (ctrl != null)
                {
                    ctrl.Select();
                    if (!ctrl.Focused)
                        NativeFunctions.SetFocus(m_LastFocusWindow);
                }
                else
                {
                    NativeFunctions.SetFocus(m_LastFocusWindow);
                }
                m_LastFocusWindow = 0;
                if(m_BaseItemContainer is GenericItemContainer)
                    ((GenericItemContainer)m_BaseItemContainer).AutoExpand = false;
            }
        }

        /// <summary>
        /// Returns the reference to the control that last had input focus. This property should be used to
        /// determine which control had input focus before bar gained the focus. Use it to apply
        /// the menu command to active control.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control LastFocusControl
        {
            get
            {
                if (this.Focused && m_LastFocusWindow != 0)
                {
                    Control ctrl = Control.FromChildHandle(new System.IntPtr(m_LastFocusWindow));
                    return ctrl;
                }
                return null;
            }
        }

        //internal void SetSystemFocus()
        //{
        //    if (!this.Focused)
        //    {
        //        if (m_BaseItemContainer is GenericItemContainer)
        //            ((GenericItemContainer)m_BaseItemContainer).SetSystemFocus();
        //        this.Focus();
        //    }
        //}
		#endregion

		#region IOwnerItemEvents Implementation
        protected void CopyIOwnerEvents(ItemControl target)
        {
            target.ItemClick = this.ItemClick;
        }

		void IOwnerItemEvents.InvokeItemAdded(BaseItem item,EventArgs e)
		{
			if(ItemAdded!=null)
				ItemAdded(item,e);
		}
		void IOwnerItemEvents.InvokeItemRemoved(BaseItem item, BaseItem parent)
		{
			if(ItemRemoved!=null)
			{
				ItemRemoved(item,new ItemRemovedEventArgs(parent));
			}
		}
		void IOwnerItemEvents.InvokeMouseEnter(BaseItem item,EventArgs e)
		{
			if(EventMouseEnter!=null)
				EventMouseEnter(item,e);
		}
		void IOwnerItemEvents.InvokeMouseHover(BaseItem item,EventArgs e)
		{
			if(EventMouseHover!=null)
				EventMouseHover(item,e);
		}
		void IOwnerItemEvents.InvokeMouseLeave(BaseItem item,EventArgs e)
		{
			if(EventMouseLeave!=null)
				EventMouseLeave(item,e);
		}
		void IOwnerItemEvents.InvokeMouseDown(BaseItem item, MouseEventArgs e)
		{
			if (EventMouseDown!= null)
				EventMouseDown(item, e);
			//if(this.MouseDown!=null)
			//	this.MouseDown(item,e);
			if(item.ClickAutoRepeat && e.Button==MouseButtons.Left)
			{
				m_ClickRepeatItem=item;
				if(m_ClickTimer==null)
					m_ClickTimer=new Timer();
				m_ClickTimer.Interval=item.ClickRepeatInterval;
				m_ClickTimer.Tick+=new EventHandler(this.ClickTimerTick);
				m_ClickTimer.Start();
			}
		}
		void IOwnerItemEvents.InvokeMouseUp(BaseItem item, MouseEventArgs e)
		{
			if(EventMouseUp!=null)
				EventMouseUp(item, e);

			if(m_ClickTimer!=null && m_ClickTimer.Enabled)
			{
				m_ClickTimer.Stop();
				m_ClickTimer.Enabled=false;
			}
		}
		private void ClickTimerTick(object sender, EventArgs e)
		{
			if(m_ClickRepeatItem!=null)
				m_ClickRepeatItem.RaiseClick();
			else
				m_ClickTimer.Stop();
		}
		void IOwnerItemEvents.InvokeMouseMove(BaseItem item, MouseEventArgs e)
		{
			if(EventMouseMove!=null)
				EventMouseMove(item,e);
		}
        void IOwnerItemEvents.InvokeItemDoubleClick(BaseItem objItem, MouseEventArgs e)
        {
            OnItemDoubleClick(objItem, e);
        }
        /// <summary>
        /// Invokes ItemDoubleClick event.
        /// </summary>
        /// <param name="objItem">Reference to item double-clicked</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnItemDoubleClick(BaseItem objItem, MouseEventArgs e)
        {
            MouseEventHandler handler = ItemDoubleClick;
            if (handler != null)
                handler(objItem, e);
        }
        void IOwnerItemEvents.InvokeItemClick(BaseItem objItem)
		{
            OnItemClick(objItem);
		}
        /// <summary>
        /// Invokes the ItemClick event.
        /// </summary>
        /// <param name="item">Reference to the item that was clicked.</param>
        protected virtual void OnItemClick(BaseItem item)
        {
            RibbonControl rc = this.GetRibbonControl();
            if (rc!=null)
            {
                rc.OnChildItemClick(item);
            }

            if (this.ShowKeyTips)
            {
                this.ShowKeyTips = false;
            }

            if (ItemClick != null)
                ItemClick(item, new EventArgs());
        }
		void IOwnerItemEvents.InvokeGotFocus(BaseItem item,EventArgs e)
		{
			if(EventGotFocus!=null)
				EventGotFocus(item,e);
		}
		void IOwnerItemEvents.InvokeLostFocus(BaseItem item,EventArgs e)
		{
			if(EventLostFocus!=null)
				EventLostFocus(item,e);
		}
		void IOwnerItemEvents.InvokeExpandedChange(BaseItem item,EventArgs e)
		{
			if(ExpandedChange!=null)
				ExpandedChange(item,e);
		}
		void IOwnerItemEvents.InvokeItemTextChanged(BaseItem item, EventArgs e)
		{
			if(ItemTextChanged!=null)
				ItemTextChanged(item,e);
		}

		void IOwnerItemEvents.InvokeContainerControlDeserialize(BaseItem item,ControlContainerSerializationEventArgs e)
		{
			if(ContainerControlDeserialize!=null)
				ContainerControlDeserialize(item,e);
		}
		void IOwnerItemEvents.InvokeContainerControlSerialize(BaseItem item,ControlContainerSerializationEventArgs e)
		{
			if(ContainerControlSerialize!=null)
				ContainerControlSerialize(item,e);
		}
		void IOwnerItemEvents.InvokeContainerLoadControl(BaseItem item,EventArgs e)
		{
			if(ContainerLoadControl!=null)
				ContainerLoadControl(item,e);
		}

		void IOwnerItemEvents.InvokeOptionGroupChanging(BaseItem item, OptionGroupChangingEventArgs e)
		{
			if(OptionGroupChanging!=null)
				OptionGroupChanging(item,e);
		}

		void IOwnerItemEvents.InvokeToolTipShowing(object item, EventArgs e)
		{
			if(ToolTipShowing!=null)
				ToolTipShowing(item,e);
		}

        void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
        {
            if (ButtonCheckedChanged != null)
                ButtonCheckedChanged(item, e);
        }
		#endregion

		#region IMessageHandlerClient Implementation
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
            return OnMouseWheel(hWnd, wParam, lParam);
        }

        protected virtual bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }

		bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            return OnKeyDown(hWnd, wParam, lParam);
		}

        /// <summary>
        /// Returns whether control has any popups registered.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public bool HasRegisteredPopups
        {
            get
            {
                return m_RegisteredPopups.Count > 0;
            }
        }

        protected virtual bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            bool designMode = this.GetDesignMode();

            if (m_RegisteredPopups.Count > 0)
            {
                if (((BaseItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1]).Parent == null)
                {
                    PopupItem objItem = (PopupItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1];

                    Control ctrl = objItem.PopupControl as Control;
                    Control ctrl2 = FromChildHandle(hWnd);

                    if (ctrl2 != null)
                    {
                        while (ctrl2.Parent != null)
                            ctrl2 = ctrl2.Parent;
                    }

                    bool bIsOnHandle = false;
                    if (ctrl2 != null && objItem != null)
                        bIsOnHandle = objItem.IsAnyOnHandle(ctrl2.Handle.ToInt32());

                    bool bNoEat = ctrl != null && ctrl2 != null && ctrl.Handle == ctrl2.Handle || bIsOnHandle;

                    if (!bIsOnHandle)
                    {
                        Keys key = (Keys)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                        if (key == Keys.None)
                            key = (Keys)wParam.ToInt32();
                        objItem.InternalKeyDown(new KeyEventArgs(key));
                    }

                    // Don't eat the message if the pop-up window has focus
                    if (bNoEat)
                        return false;
                    return true && !designMode;
                }
            }

            if (this.MenuFocus && !designMode)
            {
                bool bPassToMenu = true;
                Control ctrl2 = Control.FromChildHandle(hWnd);
                if (ctrl2 != null)
                {
                    while (ctrl2.Parent != null)
                        ctrl2 = ctrl2.Parent;
                    if ((ctrl2 is MenuPanel || ctrl2 is ItemControl || ctrl2 is PopupContainer || ctrl2 is PopupContainerControl) && ctrl2.Handle != hWnd)
                        bPassToMenu = false;
                }

                if (bPassToMenu)
                {
                    Keys key = (Keys)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                    if (key == Keys.None)
                        key = (Keys)wParam.ToInt32();
                    this.ExKeyDown(new KeyEventArgs(key));
                    return true && !designMode;
                }
            }

            if (!this.IsParentFormActive)
                return false;

            if (wParam.ToInt32() >= 0x70 || ModifierKeys != Keys.None || (lParam.ToInt32() & 0x1000000000) != 0 || wParam.ToInt32() == 0x2E || wParam.ToInt32() == 0x2D) // 2E=VK_DELETE, 2D=VK_INSERT
            {
                int i = (int)ModifierKeys | wParam.ToInt32();
                return ProcessShortcut((eShortcut)i) && !designMode;
            }
            return false;
        }

		private bool ProcessShortcut(eShortcut key)
		{
            if (!m_ShortcutsEnabled) return false;

            Form form = this.FindForm();
            if (form == null || (form != Form.ActiveForm && form.MdiParent == null ||
                form.MdiParent != null && form.MdiParent.ActiveMdiChild != form) && !form.IsMdiContainer || Form.ActiveForm != null && Form.ActiveForm.Modal && Form.ActiveForm != form)
                return false;

			bool eat=BarFunctions.ProcessItemsShortcuts(key,m_ShortcutTable);
			return !m_DispatchShortcuts && eat;
		}
		protected bool IsParentFormActive
		{
			get
			{
				// Process only if parent form is active
				Form form=this.FindForm();
				if(form==null)
					return false;
				if(form.IsMdiChild)
				{
					if(form.MdiParent==null)
						return false;
					if(form.MdiParent.ActiveMdiChild!=form)
						return false;
				}
				else if(form!=Form.ActiveForm)
					return false;
				return true;
			}
		}

        private PopupDelayedClose m_DelayClose = null;
        private PopupDelayedClose GetDelayClose()
        {
            if (m_DelayClose == null)
                m_DelayClose = new PopupDelayedClose();
            return m_DelayClose;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void DesignerNewItemAdded()
        {
            this.GetDelayClose().EraseDelayClose();
        }
        protected virtual bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (m_RegisteredPopups.Count == 0 || this.GetDesignMode())
                return false;

            BaseItem[] popups = new BaseItem[m_RegisteredPopups.Count];
            m_RegisteredPopups.CopyTo(popups);
            for (int i = popups.Length - 1; i >= 0; i--)
            {
                PopupItem objPopup = popups[i] as PopupItem;
                bool bChildHandle = objPopup.IsAnyOnHandle(hWnd.ToInt32());

                if (!bChildHandle)
                {
                    Control cTmp = FromChildHandle(hWnd);
                    if (cTmp != null)
                    {
                        if (cTmp is MenuPanel)
                            bChildHandle = true;
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
                            if (!bChildHandle)
                                bChildHandle = objPopup.IsAnyOnHandle(cTmp.Handle.ToInt32());
                        }
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
                    if (popupContainer != null && popupContainer.Parent != null)
                        popupContainer = popupContainer.Parent;
                    if (popupContainer != null && popupContainer.Bounds.Contains(Control.MousePosition))
                        bChildHandle = true;
                }

                if (bChildHandle)
                    break;

                if (objPopup.Displayed && !this.IsDisposed)
                {
                    // Do not close if mouse is inside the popup parent button
                    Point p = this.PointToClient(MousePosition);
                    if (objPopup.DisplayRectangle.Contains(p))
                        break;
                }

                if (objPopup.PopupControl is IKeyTipsControl)
                    ((IKeyTipsControl)objPopup.PopupControl).ShowKeyTips = false;

                if (this.GetDesignMode())
                {
                    this.GetDelayClose().DelayClose(objPopup);
                }
                else
                {
                    objPopup.ClosePopup();
                }

                if (m_RegisteredPopups.Count == 0)
                    break;
            }
            if (m_RegisteredPopups.Count == 0)
                this.MenuFocus = false;
            return false;
        }
		bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            return OnSysMouseDown(hWnd, wParam, lParam);
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
		bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            return OnSysKeyDown(hWnd, wParam, lParam);
		}

        protected virtual bool OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (!this.GetDesignMode())
            {
                if (wParam.ToInt32() == 18 && System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Alt)
                    this.ClosePopups();

                // Check Shortcuts
                if (ModifierKeys != Keys.None || wParam.ToInt32() >= (int)eShortcut.F1 && wParam.ToInt32() <= (int)eShortcut.F12)
                {
                    int i = (int)ModifierKeys | wParam.ToInt32();
                    if (ProcessShortcut((eShortcut)i))
                        return true;
                }
            }
            return false;
        }

		bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			return OnSysKeyUp(hWnd, wParam, lParam);
		}

        protected virtual bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }

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

            parentForm.Resize += new EventHandler(this.ParentResize);
            parentForm.Deactivate += new EventHandler(this.ParentDeactivate);

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
            parentForm.Resize -= new EventHandler(this.ParentResize);
            parentForm.Deactivate -= new EventHandler(this.ParentDeactivate);
		}
		private void ParentResize(object sender, EventArgs e)
		{
			Form parentForm=this.FindForm();
			if(parentForm!=null && parentForm.WindowState==FormWindowState.Minimized)
				((IOwner)this).OnApplicationDeactivate();
		}
		private void ParentDeactivate(object sender, EventArgs e)
		{
			Form parentForm=this.FindForm();
			if(parentForm!=null && parentForm.WindowState==FormWindowState.Minimized)
				((IOwner)this).OnApplicationDeactivate();
		}
		#endregion

		#region IThemeCache Implementation
		protected override void WndProc(ref Message m)
		{
            if (m.Msg == NativeFunctions.WM_SETFOCUS)
            {
                int wnd = m.WParam.ToInt32();
                Control objCtrl = Control.FromChildHandle(new IntPtr(wnd));
                if (objCtrl != null)
                {
                    if (!(objCtrl is DevComponents.DotNetBar.Bar) && !(objCtrl is MenuPanel) && !(objCtrl is Controls.TextBoxX) && !(objCtrl is Controls.ComboBoxEx))
                    {
                        Form form = objCtrl.FindForm();
                        if (form == this.FindForm())
                            m_LastFocusWindow = m.WParam.ToInt32();
                        else if (form != null)
                            m_LastFocusWindow = m.WParam.ToInt32();
                    }
                }
            }
            else if (m.Msg == NativeFunctions.WM_USER + 107)
            {
                if (m_DoDefaultActionItem != null)
                {
                    m_DoDefaultActionItem.DoAccesibleDefaultAction();
                    m_DoDefaultActionItem = null;
                }
            }
            else if (m.Msg == NativeFunctions.WM_THEMECHANGED)
            {
                this.RefreshThemes();
            }
			base.WndProc(ref m);
		}
        
		protected void RefreshThemes()
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
            if (m_ThemeButton != null)
            {
                m_ThemeButton.Dispose();
                m_ThemeButton = new ThemeButton(this);
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
            if (m_ThemeButton != null)
            {
                m_ThemeButton.Dispose();
                m_ThemeButton = null;
            }
		}
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if(!m_FilterInstalled && !this.DesignMode)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled=true;
			}
            
			this.RecalcLayout();
		}
		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeThemes();
			MenuEventSupportUnhook();
			base.OnHandleDestroyed(e);

			if(m_FilterInstalled)
			{
				MessageHandler.UnregisterMessageClient(this);
				m_FilterInstalled=false;
			}

            if (m_Hook != null)
            {
                m_Hook.Dispose();
                m_Hook = null;
            }
		}
		ThemeWindow IThemeCache.ThemeWindow
		{
			get
			{
				if(m_ThemeWindow==null)
					m_ThemeWindow=new ThemeWindow(this);
				return m_ThemeWindow;
			}
		}
		ThemeRebar IThemeCache.ThemeRebar
		{
			get
			{
				if(m_ThemeRebar==null)
					m_ThemeRebar=new ThemeRebar(this);
				return m_ThemeRebar;
			}
		}
		ThemeToolbar IThemeCache.ThemeToolbar
		{
			get
			{
				if(m_ThemeToolbar==null)
					m_ThemeToolbar=new ThemeToolbar(this);
				return m_ThemeToolbar;
			}
		}
		ThemeHeader IThemeCache.ThemeHeader
		{
			get
			{
				if(m_ThemeHeader==null)
					m_ThemeHeader=new ThemeHeader(this);
				return m_ThemeHeader;
			}
		}
		ThemeScrollBar IThemeCache.ThemeScrollBar
		{
			get
			{
				if(m_ThemeScrollBar==null)
					m_ThemeScrollBar=new ThemeScrollBar(this);
				return m_ThemeScrollBar;
			}
		}
		ThemeExplorerBar IThemeCache.ThemeExplorerBar
		{
			get
			{
				if(m_ThemeExplorerBar==null)
					m_ThemeExplorerBar=new ThemeExplorerBar(this);
				return m_ThemeExplorerBar;
			}
		}
		ThemeProgress IThemeCache.ThemeProgress
		{
			get
			{
				if(m_ThemeProgress==null)
					m_ThemeProgress=new ThemeProgress(this);
				return m_ThemeProgress;
			}
		}
        ThemeButton IThemeCache.ThemeButton
        {
            get
            {
                if (m_ThemeButton == null)
                    m_ThemeButton = new ThemeButton(this);
                return m_ThemeButton;
            }
        }
		#endregion

		#region Mouse & Keyboard Support
		protected override void OnClick(EventArgs e)
		{
            Point p = this.PointToClient(MousePosition);
			InternalOnClick(MouseButtons,p);
			base.OnClick(e);
		}

		protected virtual void InternalOnClick(MouseButtons mb, Point mousePos)
		{
			m_BaseItemContainer.InternalClick(mb, mousePos);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			m_BaseItemContainer.InternalDoubleClick(MouseButtons,MousePosition);
			base.OnDoubleClick(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			ExKeyDown(e);
			base.OnKeyDown(e);
		}

		internal virtual void ExKeyDown(KeyEventArgs e)
		{
			m_BaseItemContainer.InternalKeyDown(e);
		}

        //private Cursor _ControlCursor = null;
        protected override void OnMouseEnter(EventArgs e)
        {
            EventHandler h = EventMouseEnter;
            if (h != null)
                h(this, e);
            //_ControlCursor = this.Cursor;
            base.OnMouseEnter(e);
        }
        
		protected override void OnMouseDown(MouseEventArgs e)
		{
			m_BaseItemContainer.InternalMouseDown(e);
			base.OnMouseDown(e);
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			m_BaseItemContainer.InternalMouseHover();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			// If we had hot sub item pass the mouse leave message to it...
            //if (this.Cursor != _ControlCursor)
            //    this.Cursor = _ControlCursor;
			m_BaseItemContainer.InternalMouseLeave();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(m_DragDropSupport && !m_UseNativeDragDrop)
				MouseDragDrop(e.X,e.Y,null);

			m_BaseItemContainer.InternalMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
            InternalMouseMove(e);
		}

        protected virtual void InternalMouseMove(MouseEventArgs e)
        {
            if(m_DragDropSupport && m_DragInProgress)
			{
				if(!m_UseNativeDragDrop)
					MouseDragOver(e.X,e.Y,null);
			}
			else
				m_BaseItemContainer.InternalMouseMove(e);
        }

		protected override void OnDragOver(DragEventArgs e)
		{
			if(m_DragDropSupport && (m_DragInProgress || m_ExternalDragInProgress))
			{
                e.Effect = DragDropEffects.Move;
                base.OnDragOver(e);
                if (e.Effect != DragDropEffects.None)
                {
                    Point p = this.PointToClient(new Point(e.X, e.Y));
                    MouseDragOver(p.X, p.Y, e);
                }
				m_DragLeft=false;
			}
            else
			    base.OnDragOver(e);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if(m_DragDropSupport)
			{
                if (m_DragInProgress || m_ExternalDragInProgress)
                {
                    if (Control.MouseButtons == MouseButtons.None)
                        MouseDragDrop(-1, -1, null);
                    else
                        MouseDragOver(-1, -1, null);
                }
				m_DragLeft=true;
				m_ExternalDragInProgress=false;
			}
			base.OnDragLeave(e);
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if(m_DragDropSupport)
			{
				if(m_DragInProgress || !m_AllowExternalDrop)
					return;
				if(e.Data.GetData(typeof(ButtonItem))==null)
				{
					if(e.Effect!=DragDropEffects.None)
						m_ExternalDragInProgress=true;
					return;
				}
				if((e.AllowedEffect & DragDropEffects.Move)==DragDropEffects.Move)
					e.Effect=DragDropEffects.Move;
				else if((e.AllowedEffect & DragDropEffects.Copy)==DragDropEffects.Copy)
					e.Effect=DragDropEffects.Move;
				else if((e.AllowedEffect & DragDropEffects.Link)==DragDropEffects.Link)
					e.Effect=DragDropEffects.Move;
				else
					return;
				m_ExternalDragInProgress=true;
			}
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			if(m_DragDropSupport)
			{
				if(m_DragInProgress)
				{
					Point p=this.PointToClient(new Point(e.X,e.Y));
					MouseDragDrop(p.X,p.Y,null);
				}
				else if(m_ExternalDragInProgress)
				{
					Point p=this.PointToClient(new Point(e.X,e.Y));
					MouseDragDrop(p.X,p.Y,e);
				}
			}
			base.OnDragDrop(e);
		}

		protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
            base.OnQueryContinueDrag(e);
			if(m_DragDropSupport)
			{
				if(m_DragInProgress)
				{
					if(m_DragLeft && e.Action==DragAction.Drop || e.Action==DragAction.Cancel)
						MouseDragDrop(-1,-1,null);
				}
			}
		}

		private void MouseDragOver(int x, int y, DragEventArgs dragArgs)
		{
			if(!m_DragInProgress && !m_ExternalDragInProgress)
				return;
			BaseItem dragItem=m_DragItem;
			
			if(m_ExternalDragInProgress && dragArgs!=null)
				dragItem=dragArgs.Data.GetData(typeof(ButtonItem)) as BaseItem;

			if(m_DesignTimeProvider!=null)
			{
				m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition,m_InsertBefore);
				m_DesignTimeProvider=null;
			}

			if(m_ExternalDragInProgress && dragItem==null)
				return;

			Point pScreen=this.PointToScreen(new Point(x,y));
            InsertPosition pos = ((IDesignTimeProvider)m_BaseItemContainer).GetInsertPosition(pScreen, dragItem);

			if(pos!=null)
			{
                if (pos.TargetProvider == null)
                {
                    // Cursor is over drag item
                    if (!m_UseNativeDragDrop)
                    {
                        if (m_NACursor != null)
                            Cursor.Current = m_NACursor;
                        else
                            Cursor.Current = Cursors.No;
                    }
                }
                else
                {
                    pos.TargetProvider.DrawReversibleMarker(pos.Position, pos.Before);
                    m_InsertPosition = pos.Position;
                    m_InsertBefore = pos.Before;
                    m_DesignTimeProvider = pos.TargetProvider;
                    if (!m_UseNativeDragDrop)
                    {
                        if (m_MoveCursor != null)
                            Cursor.Current = m_MoveCursor;
                        else
                            Cursor.Current = Cursors.Hand;
                    }
                    else if (dragArgs != null)
                        dragArgs.Effect = DragDropEffects.Move;
                }
			}
			else
			{
				if(!m_UseNativeDragDrop)
				{
					if(m_NACursor!=null)
						Cursor.Current=m_NACursor;
					else
						Cursor.Current=Cursors.No;
				}
				else if(dragArgs!=null)
					dragArgs.Effect=DragDropEffects.None;
			}
		}

        protected override bool ProcessMnemonic(char charCode)
        {
            if (this.Visible && ProcessMnemonicEx(charCode))
                return true;
            return base.ProcessMnemonic(charCode);
        }

        public virtual bool ProcessMnemonicEx(char charCode)
        {
            if (ProcessMnemonic(m_BaseItemContainer, charCode))
                return true;
            return false;
        }

        protected virtual bool ProcessMnemonic(BaseItem container, char charCode)
        {
            BaseItem item = GetItemForMnemonic(container, charCode, true, true);

            return ProcessItemMnemonicKey(item);
        }

        protected virtual bool ProcessItemMnemonicKey(BaseItem item)
        {
            if (item != null && item.Visible && item.GetEnabled())
            {
                if (item is QatOverflowItem)
                    ((QatOverflowItem)item).Expanded = true;
                else if (item is ButtonItem && (item.ShowSubItems || ((ButtonItem)item).AutoExpandOnClick) && (item.SubItems.Count > 0 && item.VisibleSubItems > 0 || ((ButtonItem)item).PopupType == ePopupType.Container) && !item.Expanded)
                {
                    item.Expanded = true;
                    // If it is a menu select first menu item inside...
                    PopupItem popup = item as PopupItem;
                    if (popup.PopupType == ePopupType.Menu && popup.PopupControl is MenuPanel)
                    {
                        ((MenuPanel)popup.PopupControl).SelectFirstItem();
                    }
                    if (m_BaseItemContainer is GenericItemContainer)
                        m_BaseItemContainer.AutoExpand = true;
                }
                else if (item is ComboBoxItem)
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
                else if (item is GalleryContainer)
                    ((GalleryContainer)item).PopupGallery();
                else
                    item.RaiseClick();
                return true;
            }

            return false;
        }

        protected virtual BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys)
        {
            bool partialMatch = false;
            return GetItemForMnemonic(container, charCode, deepScan, stackKeys, ref partialMatch);
        }

        private BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys, ref bool partialMatch)
        {
            string keyTipsString = m_KeyTipsKeysStack + charCode.ToString();
            keyTipsString = keyTipsString.ToUpper();

            CaptionItemContainer gc=container as CaptionItemContainer;
            int count=container.SubItems.Count;
            if(gc!=null && gc.MoreItems!=null && gc.MoreItems.Visible)
                count++;
            BaseItem[] items = new BaseItem[count];
            container.SubItems.CopyTo(items,0);
            if (gc != null && gc.MoreItems != null && gc.MoreItems.Visible) items[count - 1] = gc.MoreItems;

            foreach (BaseItem item in items)
            {
                if (item.KeyTips != "" || m_KeyTipsKeysStack!="")
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
                    BaseItem mItem = GetItemForMnemonic(item, charCode, deepScan, false, ref partialMatch);
                    if (mItem!=null)
                        return mItem;
                }
            }

            if (partialMatch && stackKeys)
            {
                ((IKeyTipsControl)this).KeyTipsKeysStack += charCode.ToString().ToUpper();
            }
            
            return null;
        }

        
		#endregion

		#region IOwnerLocalize Implementation
		void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
		{
			if(LocalizeString!=null)
				LocalizeString(this,e);
		}
		#endregion

		#region Internal Implementation
        private bool _ContainerControlProcessDialogKey = true;
        /// <summary>
        /// Gets or sets whether ProcessDialogKey method calls base ContainerControl implementation. 
        /// By default base implementation is called but under certain conditions you might want to set
        /// this property to true to receive KeyDown events for Arrow keys.
        /// </summary>
        [DefaultValue(false), Browsable(false)]
        public bool ContainerControlProcessDialogKey
        {
            get { return _ContainerControlProcessDialogKey; }
            set
            {
                _ContainerControlProcessDialogKey = value;
            }
        }
        
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (_ContainerControlProcessDialogKey) return base.ProcessDialogKey(keyData);
            return false;
        }
#if FRAMEWORK20
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
            if (m_BaseItemContainer != null)
                m_BaseItemContainer.UpdateBindings();
        }
#endif

        /// <summary>
        /// Gets or sets whether shortuct processing for the items hosted by this control is enabled. Default value is true.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShortcutsEnabled
        {
            get { return m_ShortcutsEnabled; }
            set { m_ShortcutsEnabled = value; }
        }

        protected virtual RibbonControl GetRibbonControl()
        {
            Control parent = this.Parent;
            while (parent != null && !(parent is RibbonControl))
            {
                if (parent is RibbonPanel && ((RibbonPanel)parent).GetRibbonControl() != null)
                    return ((RibbonPanel)parent).GetRibbonControl();
                parent = parent.Parent;
            }

            return parent as RibbonControl;
        }

        /// <summary>
        /// Returns the item at specified coordinates or null if no item can be found.
        /// </summary>
        /// <param name="x">X - coordinate to test.</param>
        /// <param name="y">Y - coordinate to test.</param>
        /// <returns></returns>
        public virtual BaseItem HitTest(int x, int y)
        {
            return m_BaseItemContainer.ItemAtLocation(x, y);
        }

        /// <summary>
        /// Gets or sets the item default accessibility action will be performed on.
        /// </summary>
        BaseItem IAccessibilitySupport.DoDefaultActionItem
        {
            get { return m_DoDefaultActionItem; }
            set { m_DoDefaultActionItem = value; }
        }

		/// <summary>
		/// Gets or sets whether control height is set automatically based on the content. Default value is false.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates whether control height is set automatically."),DefaultValue(false)]
#if FRAMEWORK20
		public override bool AutoSize
#else
        public bool AutoSize
#endif
		{
			get {return m_AutoSize;}
			set
			{
				m_AutoSize=value;
				OnAutoSizeChanged();
			}
		}
		private void OnAutoSizeChanged()
		{
			AutoSyncSize();
		}

		private int m_SyncingSize=0;
		/// <summary>
		/// Sets the height of the control to the automatically calcualted height based on content.
		/// </summary>
		public virtual void AutoSyncSize()
		{
			if(!m_AutoSize || m_SyncingSize>2)	
				return;

			if(m_BaseItemContainer.NeedRecalcSize)
				return;

			m_SyncingSize++;
			try
			{
				int newHeight=GetAutoSizeHeight();
				if(this.Height!=newHeight && m_BaseItemContainer.HeightInternal>0)
				{
					this.Height=newHeight;
					this.RecalcSize();
				}
			}
			finally
			{
				m_SyncingSize--;
			}
		}

        protected virtual ElementStyle GetBackgroundStyle()
        {
            return m_BackgroundStyle;
        }

        internal ElementStyle GetPaintBackgroundStyle()
        {
            return GetBackgroundStyle();
        }

		/// <summary>
		/// Returns automatically calculated height of the control given current content.
		/// </summary>
		/// <returns>Height in pixels.</returns>
		public virtual int GetAutoSizeHeight()
		{
            return (m_BaseItemContainer.HeightInternal + ElementStyleLayout.VerticalStyleWhiteSpace(GetBackgroundStyle()));
		}

		protected void SetBaseItemContainer(BaseItem item)
		{
			m_BaseItemContainer=item;
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public BaseItem GetBaseItemContainer()
		{
			return m_BaseItemContainer;
		}

		protected virtual string EmptyContainerDesignTimeHint
		{
			get {return m_EmptyContainerDesignTimeHint;}
			set {m_EmptyContainerDesignTimeHint=value;}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(m_ClickTimer!=null)
				{
					m_ClickTimer.Stop();
					m_ClickTimer.Dispose();
					m_ClickTimer=null;
				}

                if (m_DelayClose != null)
                {
                    m_DelayClose.Dispose();
                    m_DelayClose = null;
                }
                if (m_BaseItemContainer != null)
                    m_BaseItemContainer.Dispose();

                if (m_ImageList != null)
                    m_ImageList.Disposed -= new EventHandler(this.ImageListDisposed);
                if (m_ImageListMedium != null)
                    m_ImageListMedium.Disposed -= new EventHandler(this.ImageListDisposed);
                if (m_ImageListLarge != null)
                    m_ImageListLarge.Disposed -= new EventHandler(this.ImageListDisposed);

			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Specifies the background style of the control.
		/// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets bar background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ElementStyle BackgroundStyle
		{
			get {return m_BackgroundStyle;}
            //set
            //{
            //    if (value == null)
            //        throw new InvalidOperationException("Null is not valid value for this property.");
            //    m_BackgroundStyle = value;
            //}
		}

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackgroundStyle()
		{
			m_BackgroundStyle.StyleChanged-=new EventHandler(this.VisualPropertyChanged);
			m_BackgroundStyle=new ElementStyle();
			m_BackgroundStyle.StyleChanged+=new EventHandler(this.VisualPropertyChanged);
            this.Invalidate();
		}

		private void VisualPropertyChanged(object sender, EventArgs e)
		{
            OnVisualPropertyChanged();
		}

        protected virtual void OnVisualPropertyChanged()
        {
            if(this.GetDesignMode() || 
                this.Parent!=null && this.Parent.Site!=null && this.Parent.Site.DesignMode)
			{
				this.RecalcLayout();
			}
        }

		protected virtual void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
		{
			if(!m_DragInProgress && !m_ExternalDragInProgress)
				return;
			BaseItem dragItem=m_DragItem;
			if(m_ExternalDragInProgress)
				dragItem=dragArgs.Data.GetData(typeof(ButtonItem)) as BaseItem;

			if(dragItem!=null)
				dragItem.InternalMouseLeave();

			if(m_DesignTimeProvider!=null)
			{
				if(x==-1 && y==-1)
				{
					// Cancel state
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
				}
				else
				{
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
					if(dragItem!=null)
					{
						BaseItem objParent=dragItem.Parent;
						if(objParent!=null)
						{
							if(objParent==(BaseItem)m_DesignTimeProvider && m_InsertPosition>0)
							{
								if(objParent.SubItems.IndexOf(dragItem)<m_InsertPosition)
									m_InsertPosition--;
							}
							objParent.SubItems.Remove(dragItem);
							objParent.Refresh();
                            if (objParent.ContainerControl != this)
                                BarUtilities.InvokeRecalcLayout(objParent.ContainerControl as Control);
						}
						m_DesignTimeProvider.InsertItemAt(dragItem,m_InsertPosition,m_InsertBefore);
						m_DesignTimeProvider=null;
						((IOwner)this).InvokeUserCustomize(dragItem,new EventArgs());
					}
				}
			}
			m_DesignTimeProvider=null;
			m_DragInProgress=false;
			m_ExternalDragInProgress=false;
			if(!m_UseNativeDragDrop)
				Cursor.Current=Cursors.Default;
			this.Capture=false;
			if(dragItem!=null)
				dragItem._IgnoreClick=true;
			m_BaseItemContainer.InternalMouseUp(new MouseEventArgs(MouseButtons.Left,0,x,y,0));
			if(dragItem!=null)
				dragItem._IgnoreClick=false;

			m_DragItem=null;

            if (UserCustomize != null)
                UserCustomize(this, new EventArgs());
		}

		/// <summary>
		/// Indicates whether shortucts handled by items are dispatched to the next handler or control.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Behavior"),Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
		public bool DispatchShortcuts
		{
			get {return m_DispatchShortcuts;}
			set {m_DispatchShortcuts=value;}
		}

		/// <summary>
		/// Gets or sets Bar Color Scheme.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Category("Appearance"),Description("Gets or sets Bar Color Scheme."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColorScheme ColorScheme
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
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColorScheme()
		{
			return m_ColorScheme.SchemeChanged;
		}

		/// <summary>
		/// Gets or sets whether anti-alias smoothing is used while painting. Default value is true.
		/// </summary>
		[DefaultValue(true),Browsable(true),Category("Appearance"),Description("Gets or sets whether anti-aliasing is used while painting.")]
		public bool AntiAlias
		{
			get {return m_AntiAlias;}
			set
			{
				if(m_AntiAlias!=value)
				{
					m_AntiAlias=value;
                    this.Invalidate();
				}
			}
		}

		protected bool IsThemed
		{
			get
			{
                if (m_ThemeAware && m_BaseItemContainer.EffectiveStyle != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && Themes.ThemesActive)
					return true;
				return false;
			}
		}

		private bool m_DisabledImagesGrayScale=true;
		/// <summary>
		/// Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images. Default is true.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Appearance"),Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
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

		/// <summary>
		/// Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Appearance"),Description("Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public virtual bool ThemeAware
		{
			get
			{
				return m_ThemeAware;
			}
			set
			{
				m_ThemeAware=value;
				m_BaseItemContainer.ThemeAware=value;
				if(this.GetDesignMode())
					this.Refresh();
			}
		}

		/// <summary>
		/// Applies design-time defaults to control.
		/// </summary>
		public virtual void SetDesignTimeDefaults()
		{
		}
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool DesignerSelection
		{
			get {return m_DesignerSelection;}
			set
			{
				if(m_DesignerSelection!=value)
				{
					m_DesignerSelection=value;
					this.Refresh();
				}
			}
		}

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
                if (m_KeyTipsCanvas != null && m_KeyTipsCanvas.Parent!=null)
                {
                    m_KeyTipsCanvas.Parent.Invalidate(true);
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
		#endregion

		#region Painting Support
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
                if(!SystemInformation.IsFontSmoothingEnabled)
                #endif
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
            return g;
        }

        internal virtual ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(this as IOwner, this, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = m_DesignerSelection;
            pa.GlassEnabled = !this.DesignMode && IsGlassEnabled;
            return pa;
        }

        internal virtual bool IsGlassEnabled
        {
            get
            {
                if (this.GetDesignMode()) return false;
                return WinApi.IsGlassEnabled;
            }
        }


        internal ColorScheme GetColorSchemeInternal()
        {
            return GetColorScheme();
        }

        /// <summary>
        /// Returns the color scheme used by control. Color scheme for Office2007 style will be retrived from the current renderer instead of
        /// local color scheme referenced by ColorScheme property.
        /// </summary>
        /// <returns>An instance of ColorScheme object.</returns>
        protected virtual ColorScheme GetColorScheme()
        {
            if (BarFunctions.IsOffice2007Style(m_BaseItemContainer.EffectiveStyle))
            {
                BaseRenderer r = GetRenderer();
                if (r is Office2007Renderer)
                    return ((Office2007Renderer)r).ColorTable.LegacyColors;
            }
            return m_ColorScheme;
        }

        internal void InternalPaint(PaintEventArgs e)
        {
            OnPaint(e, true);
        }

		protected override void OnPaint(PaintEventArgs e)
        {
            OnPaint(e, false);
            if (m_KeyTipsCanvas != null)
            {
                InvalidateKeyTipsCanvas();
                InvalidateKeyTipsDelayed();
            }
            base.OnPaint(e);
		}

        private Timer m_KeyTipDelayRefreshTimer = null;
        private void InvalidateKeyTipsDelayed()
        {
            if (m_KeyTipDelayRefreshTimer != null) return;
            m_KeyTipDelayRefreshTimer = new Timer();
            m_KeyTipDelayRefreshTimer.Tick+=new EventHandler(KeyTipDelayRefreshTimerTick);
            m_KeyTipDelayRefreshTimer.Interval = 100;
            m_KeyTipDelayRefreshTimer.Start();
        }
        private void KeyTipDelayRefreshTimerTick(object sender, EventArgs e)
        {
            Timer timer = m_KeyTipDelayRefreshTimer;
            m_KeyTipDelayRefreshTimer = null;
            timer.Stop();
            timer.Tick -= new EventHandler(KeyTipDelayRefreshTimerTick);
            timer.Dispose();
            InvalidateKeyTipsCanvas();
        }

        internal void InvalidateKeyTipsCanvas()
        {
            if (m_KeyTipsCanvas != null) m_KeyTipsCanvas.Invalidate();
            //if (this.Parent != null && this.Parent.Parent is RibbonControl) ((RibbonControl)this.Parent.Parent).RibbonStrip.InvalidateKeyTipsCanvas();
        }

        private bool m_FirstScrollPaint = true;
        private void OnPaint(PaintEventArgs e, bool cashedPaint)
        {
            SmoothingMode sm = e.Graphics.SmoothingMode;
            TextRenderingHint th = e.Graphics.TextRenderingHint;

            if (this.BackColor.IsEmpty || this.BackColor == Color.Transparent)
            {
                base.OnPaintBackground(e);
            }

            if (m_BackgroundStyle != null)
                m_BackgroundStyle.SetColorScheme(this.GetColorScheme());

            ItemPaintArgs pa = GetItemPaintArgs(e.Graphics);
            pa.ClipRectangle = e.ClipRectangle;
            if (m_FirstScrollPaint && this.AutoScroll && this.Controls.Count > 0)
            {
                pa.ClipRectangle = Rectangle.Empty;
                m_FirstScrollPaint = false;
            }
            pa.CachedPaint = cashedPaint;
            PaintControl(pa);

            e.Graphics.SmoothingMode = sm;
            e.Graphics.TextRenderingHint = th;
        }

        protected virtual void ClearBackground(ItemPaintArgs pa)
        {
            using (SolidBrush brush = new SolidBrush(this.BackColor))
                pa.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        protected virtual void PaintControl(ItemPaintArgs pa)
        {
            if (m_BaseItemContainer == null || this.IsDisposed)
                return;

            if (m_BaseItemContainer.NeedRecalcSize)
                this.RecalcSize();

            if (!this.BackColor.IsEmpty && this.BackColor!=Color.Transparent)
            {
                ClearBackground(pa);
            }

#if TRIAL
				if(NativeFunctions.ColorExpAlt())
				{
					pa.Graphics.DrawString("Your DotNetBar Trial has expired.",this.Font,SystemBrushes.ControlText,0,0);
					return;
				}
#endif

            if (m_AntiAlias)
            {
                pa.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                pa.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            PaintControlBackground(pa);

            Region oldClip = pa.Graphics.Clip;
            pa.Graphics.SetClip(GetPaintClipRectangle(), CombineMode.Intersect);
            PaintItemContainer(pa);
            pa.Graphics.Clip = oldClip;
            if (oldClip != null) oldClip.Dispose();
            if (m_BaseItemContainer.SubItems.Count == 0 && this.GetDesignMode())
                PaintDesignTimeEmptyHint(pa);
        }

        protected virtual Rectangle GetPaintClipRectangle()
        {
            return this.GetItemContainerRectangle();
        }

        protected virtual Rectangle GetPaintControlBackgroundRectangle()
        {
            return this.ClientRectangle;
        }
        protected virtual void PaintControlBackground(ItemPaintArgs pa)
		{
            ElementStyle style = GetBackgroundStyle();
			if(style!=null)
			{
                Rectangle r = GetPaintControlBackgroundRectangle();
				ElementStyleDisplayInfo displayInfo=new ElementStyleDisplayInfo(style,pa.Graphics,r);
				ElementStyleDisplay.Paint(displayInfo);
			}
		}

		protected virtual void PaintItemContainer(ItemPaintArgs pa)
		{
			m_BaseItemContainer.Paint(pa);
		}

		protected virtual void PaintDesignTimeEmptyHint(ItemPaintArgs pa)
		{
			string info=m_EmptyContainerDesignTimeHint;
			Rectangle rText=this.GetItemContainerRectangle();
			rText.Inflate(-2,-2);
			if(rText.Width<0 || rText.Height<0)
				return;
			StringFormat format=BarFunctions.CreateStringFormat(); //StringFormat.GenericDefault.Clone() as StringFormat;
			format.Alignment=StringAlignment.Center;
			format.LineAlignment=StringAlignment.Center;
			pa.Graphics.DrawString(info,this.Font,SystemBrushes.ControlDark,rText,format);
		}
        
        protected virtual void PaintKeyTips(Graphics g)
        {
            if (!m_ShowKeyTips)
                return;

            KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, "", GetKeyTipFont(), null);
            
            Rendering.BaseRenderer renderer = GetRenderer();
            PaintContainerKeyTips(m_BaseItemContainer, renderer, e);
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
                PaintItemKeyTip(item, renderer, e);
            }

            CaptionItemContainer gc = container as CaptionItemContainer;
            if (gc != null && gc.MoreItems != null && gc.MoreItems.Visible)
            {
                PaintItemKeyTip(gc.MoreItems, renderer, e);
            }
        }

        private void PaintItemKeyTip(BaseItem item, Rendering.BaseRenderer renderer, KeyTipsRendererEventArgs e)
        {
            if (!item.Visible || !item.Displayed)
                return;

            if (item.IsContainer)
                PaintContainerKeyTips(item, renderer, e);

            if (item.AccessKey == Char.MinValue && item.KeyTips == "" || m_KeyTipsKeysStack != "" && !item.KeyTips.StartsWith(m_KeyTipsKeysStack)
                || item.KeyTips == "" && m_KeyTipsKeysStack != "")
                return;

            if (item.KeyTips != "")
                e.KeyTip = item.KeyTips;
            else
                e.KeyTip = item.AccessKey.ToString().ToUpper();

            e.Bounds = GetKeyTipRectangle(e.Graphics, item, e.Font, e.KeyTip);
            e.ReferenceObject = item;

            renderer.DrawKeyTips(e);
        }

        protected virtual Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
        {
            Size padding = KeyTipsPainter.KeyTipsPadding;
            Size size = TextDrawing.MeasureString(g, keyTip, font);
            size.Width += padding.Width;
            size.Height += padding.Height;

            Rectangle ib = item.DisplayRectangle;
            Rectangle r = new Rectangle(ib.X + (ib.Width - size.Width) / 2, ib.Bottom - size.Height, size.Width, size.Height);
            if (item is RibbonTabItem)
                r.Y = ib.Bottom - 7;
            
            return r;
        }

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
                m_DefaultRenderer = new Rendering.Office2007Renderer();
                
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

        protected virtual Rectangle GetKeyTipCanvasBounds()
        {
            if (this.Parent != null)
                return new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height + 16);
            else
                return new Rectangle(0, 0, this.Width, this.Height);
        }

        protected virtual void CreateKeyTipCanvas()
        {
            if (m_KeyTipsCanvas != null)
            {
                m_KeyTipsCanvas.BringToFront();
                if(m_KeyTipsCanvas.Parent!=null)
                    m_KeyTipsCanvas.Parent.Invalidate(m_KeyTipsCanvas.Bounds);
                return;
            }

            m_KeyTipsCanvas = new KeyTipsCanvasControl(this);
            m_KeyTipsCanvas.Bounds = GetKeyTipCanvasBounds();
            m_KeyTipsCanvas.Visible = true;

            RibbonControl rc = this.GetRibbonControl();
            if (rc != null)
            {
                if (rc.Expanded && !NeedsTopLevelKeyTipCanvasParent)
                {
                    rc.Controls.Add(m_KeyTipsCanvas); 
                }
                else
                {
                    Form f = rc.FindForm();
                    if (f != null)
                        f.Controls.Add(m_KeyTipsCanvas);
                    else
                        rc.Controls.Add(m_KeyTipsCanvas); 
                }
            }
            else
                this.Controls.Add(m_KeyTipsCanvas);
            m_KeyTipsCanvas.BringToFront();
        }

        protected virtual bool NeedsTopLevelKeyTipCanvasParent
        {
            get
            {
                return false;
            }
        }

        internal void UpdateKeyTipsCanvas()
        {
            if (m_KeyTipsCanvas != null)
            {
                m_KeyTipsCanvas.Refresh();
            }
        }

        protected virtual void DestroyKeyTipCanvas()
        {
            if (m_KeyTipsCanvas == null)
                return;
            m_KeyTipsCanvas.Visible = false;
            if (m_KeyTipsCanvas.Parent != null) m_KeyTipsCanvas.Parent.Controls.Remove(m_KeyTipsCanvas);
            m_KeyTipsCanvas.Dispose();
            m_KeyTipsCanvas = null;
        }

        void IKeyTipsRenderer.PaintKeyTips(Graphics g)
        {
            this.PaintKeyTips(g);
        }

        /// <summary>
        /// Gets or sets whether mouse over fade effect is enabled. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether mouse over fade effect is enabled")]
        public bool FadeEffect
        {
            get { return m_FadeEffect; }
            set
            {
                m_FadeEffect = value;
            }
        }

        internal bool IsFadeEnabled
        {
            get
            {
                if (this.DesignMode || (!BarFunctions.IsOffice2007Style(m_BaseItemContainer.EffectiveStyle)) ||
                    m_FadeEffect && NativeFunctions.IsTerminalSession() || TextDrawing.UseTextRenderer || m_BaseItemContainer.EffectiveStyle == eDotNetBarStyle.Office2010)
                    return false;
                return m_FadeEffect;
            }
        }
		#endregion

		#region Layout Support
        private int m_UpdateSuspendCount = 0;
        /// <summary>
        /// Indicates to control that all further update operations should not result in layout and refresh of control content.
        /// Use this method to optimize the addition of new items to the control. This method supports nested calls meaning
        /// that multiple calls are allowed but they must be ended with appropriate number of EndUpdate calls.
        /// IsUpdateSuspended property returns whether update is suspended.
        /// </summary>
        public void BeginUpdate()
        {
            m_UpdateSuspendCount++;
        }

        /// <summary>
        /// Indicates that update operation is complete and that control should perform layout and refresh to show changes. See BeginUpdate
        /// for more details.
        /// </summary>
        public void EndUpdate()
        {
            EndUpdate(true);
        }

        /// <summary>
        /// Indicates that update operation is complete and that control should perform layout and refresh to show changes. See BeginUpdate
        /// for more details.
        /// </summary>
        public void EndUpdate(bool callRecalcLayout)
        {
            if (m_UpdateSuspendCount > 0)
            {
                m_UpdateSuspendCount--;
                if (m_UpdateSuspendCount == 0 && callRecalcLayout)
                    this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets whether control layout is suspended becouse of the call to BeginUpdate method.
        /// </summary>
        [Browsable(false)]
        public bool IsUpdateSuspended
        {
            get { return m_UpdateSuspendCount > 0; }
        }

		protected virtual Rectangle GetItemContainerRectangle()
		{
            ElementStyle style = GetBackgroundStyle();
			if(style==null)
				return this.ClientRectangle;

			Rectangle r=this.ClientRectangle;
			r.X+=ElementStyleLayout.LeftWhiteSpace(style);
			r.Width-=ElementStyleLayout.HorizontalStyleWhiteSpace(style);
			r.Y+=ElementStyleLayout.TopWhiteSpace(style);
			r.Height-=ElementStyleLayout.VerticalStyleWhiteSpace(style);
			
			return r;
		}

		protected virtual void RecalcSize()
		{
            if (!BarFunctions.IsHandleValid(this) || IsUpdateSuspended)
				return;
			if(m_BaseItemContainer.IsRecalculatingSize)
				return;
            Form f = this.FindForm();
            if (f != null && f.WindowState == FormWindowState.Minimized)
                return;
            if (m_BaseItemContainer.NeedRecalcSize)
            {
                Rectangle r = this.GetItemContainerRectangle();
                m_BaseItemContainer.SuspendLayout = true;
                m_BaseItemContainer.IsRightToLeft = (this.RightToLeft == RightToLeft.Yes);
                m_BaseItemContainer.LeftInternal = r.X;
                m_BaseItemContainer.TopInternal = r.Y;
                m_BaseItemContainer.WidthInternal = r.Width;
                m_BaseItemContainer.HeightInternal = r.Height;
                m_BaseItemContainer.SuspendLayout = false;
                m_BaseItemContainer.RecalcSize();

                OnItemLayoutUpdated(EventArgs.Empty);
            }
			AutoSyncSize();
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

        protected override void OnFontChanged(EventArgs e)
        {
            this.InvalidateFontChange();
            this.InvalidateLayout();
            this.RecalcLayout();
            base.OnFontChanged(e);
        }

        private void InvalidateFontChange()
        {
            BarUtilities.InvalidateFontChange(m_BaseItemContainer.SubItems);
        }

        /// <summary>
        /// Invalidates the control layout and forces the control to recalculates the layout for the items it contains on next layout operation invoked using RecalcLayout method.
        /// </summary>
        /// <remarks>
        ///     Call to this method is usually followed by the call to
        ///     <see cref="RecalcLayout">RecalcLayout</see> method to perform the control layout.
        /// </remarks>
        public void InvalidateLayout()
        {
            m_BaseItemContainer.NeedRecalcSize = true;
        }

		/// <summary>
		/// Applies any layout changes and repaint the control.
		/// </summary>
		public virtual void RecalcLayout()
		{
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate { this.RecalcLayout(); }));
                return;
            }

			if(m_BaseItemContainer.IsRecalculatingSize || IsUpdateSuspended)
				return;
			this.RecalcSize();
			this.Invalidate();
		}

		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize (e);
            if (this.Width == 0 || this.Height == 0)
                return;
            Form form = this.FindForm();
            if (form != null && form.WindowState == FormWindowState.Minimized)
                return;
            m_BaseItemContainer.NeedRecalcSize = true;
			this.RecalcSize();
		}
		#endregion

		#region IBarDesignerServices
		IBarItemDesigner IBarDesignerServices.Designer
		{
			get {return m_BarDesigner;}
			set {m_BarDesigner=value;}
		}
		#endregion

        #region Active Window Changed Handling
        /// <summary>
        /// Sets up timer that watches when active window changes.
        /// </summary>
        protected virtual void SetupActiveWindowTimer()
        {
            if (m_ActiveWindowTimer != null)
                return;
            m_ActiveWindowTimer = new Timer();
            m_ActiveWindowTimer.Interval = 100;
            m_ActiveWindowTimer.Tick += new EventHandler(ActiveWindowTimer_Tick);

            m_ForegroundWindow = NativeFunctions.GetForegroundWindow();
            m_ActiveWindow = NativeFunctions.GetActiveWindow();

            m_ActiveWindowTimer.Start();
        }

        private void ActiveWindowTimer_Tick(object sender, EventArgs e)
        {
            if (m_ActiveWindowTimer == null)
                return;

            IntPtr f = NativeFunctions.GetForegroundWindow();
            IntPtr a = NativeFunctions.GetActiveWindow();

            if (f != m_ForegroundWindow || a != m_ActiveWindow)
            {
                if (a != IntPtr.Zero)
                {
                    Control c = Control.FromHandle(a);
                    if (c is PopupContainer || c is PopupContainerControl)
                        return;
                }
                m_ActiveWindowTimer.Stop();
                OnActiveWindowChanged();
            }
        }

        /// <summary>
        /// Called after change of active window has been detected. SetupActiveWindowTimer must be called to enable detection.
        /// </summary>
        protected virtual void OnActiveWindowChanged()
        {
            if (this.MenuFocus)
                this.MenuFocus = false;
        }

        /// <summary>
        /// Releases and disposes the active window watcher timer.
        /// </summary>
        protected virtual void ReleaseActiveWindowTimer()
        {
            if (m_ActiveWindowTimer != null)
            {
                Timer timer = m_ActiveWindowTimer;
                m_ActiveWindowTimer = null;
                timer.Stop();
                timer.Tick -= new EventHandler(ActiveWindowTimer_Tick);
                timer.Dispose();
            }
        }
        #endregion

        #region IRibbonCustomize Members
        /// <summary>
        /// Called when item on popup container is right-clicked.
        /// </summary>
        /// <param name="item">Instance of the item that is right-clicked.</param>
        void IRibbonCustomize.ItemRightClick(BaseItem item)
        {
            OnPopupItemRightClick(item);
        }

        /// <summary>
        /// Called when item on popup container is right-clicked.
        /// </summary>
        /// <param name="item">Instance of the item that is right-clicked.</param>
        protected virtual void OnPopupItemRightClick(BaseItem item) { }
        #endregion

        #region IBarImageSize Members

        /// <summary>
        /// Gets/Sets the Image size for all sub-items on the Bar.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Specifies the Image size that will be used by items on this bar."), DefaultValue(eBarImageSize.Default)]
        public virtual eBarImageSize ImageSize
        {
            get
            {
                return m_ImageSize;
            }
            set
            {
                m_ImageSize = value;
                if(m_BaseItemContainer is ImageItem)
                    ((ImageItem)m_BaseItemContainer).RefreshImageSize();
                this.RecalcLayout();
            }
        }

        #endregion

        #region Misc Properties
        /// <summary>
        /// Gets or sets whether external ButtonItem object is accepted in drag and drop operation. UseNativeDragDrop must be set to true in order for this property to be effective.
        /// </summary>
        [Browsable(false), DefaultValue(false), Category("Behavior"), Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
        public virtual bool AllowExternalDrop
        {
            get { return m_AllowExternalDrop; }
            set { m_AllowExternalDrop = value; }
        }

        /// <summary>
        /// Gets or sets whether native .NET Drag and Drop is used by control to perform drag and drop operations. AllowDrop must be set to true to allow drop of the items on control.
        /// </summary>
        [Browsable(false), DefaultValue(false), Category("Behavior"), Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
        public virtual bool UseNativeDragDrop
        {
            get { return m_UseNativeDragDrop; }
            set { m_UseNativeDragDrop = value; }
        }

        /// <summary>
        /// Gets or sets whether control supports drag &amp; drop. Default value is false.
        /// </summary>
        [Browsable(false), DefaultValue(false), Category("Behavior"), Description("Indicates whether internal automatic drag & drop support is enabled")]
        protected virtual bool DragDropSupport
        {
            get { return m_DragDropSupport; }
            set { m_DragDropSupport = value; }
        }
        #endregion
    }

	#region ItemControlAccessibleObject
	/// <summary>
	/// Represents class for Accessibility support.
	/// </summary>
	public class ItemControlAccessibleObject : System.Windows.Forms.Control.ControlAccessibleObject
	{
		private ItemControl m_Owner = null;
		/// <summary>
		/// Creates new instance of the object and initializes it with owner control.
		/// </summary>
		/// <param name="owner">Reference to owner control.</param>
		public ItemControlAccessibleObject(ItemControl owner):base(owner)
		{
			m_Owner = owner;
		}

        protected ItemControl Owner
        {
            get { return m_Owner; }
        }

		internal void GenerateEvent(BaseItem sender, System.Windows.Forms.AccessibleEvents e)
		{
			int	iChild = m_Owner.GetBaseItemContainer().SubItems.IndexOf(sender);
			if(iChild>=0)
			{
				if(m_Owner!=null && !m_Owner.IsDisposed)
					m_Owner.InternalAccessibilityNotifyClients(e,iChild);
			}
		}

        ///// <summary>
        ///// Gets or sets accessible name.
        ///// </summary>
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

        ///// <summary>
        ///// Gets accessible description.
        ///// </summary>
        //public override string Description
        //{
        //    get
        //    {
        //        if(m_Owner!=null && !m_Owner.IsDisposed)
        //            return m_Owner.AccessibleDescription;
        //        return "";
        //    }
        //}

		/// <summary>
		/// Gets accessible role.
		/// </summary>
		public override AccessibleRole Role
		{
			get
			{
				if(m_Owner!=null && !m_Owner.IsDisposed)
					return m_Owner.AccessibleRole;
				return System.Windows.Forms.AccessibleRole.None;
			}
		}

		/// <summary>
		/// Gets parent accessibility object.
		/// </summary>
		public override AccessibleObject Parent 
		{
			get
			{
				if(m_Owner!=null && !m_Owner.IsDisposed)
					return m_Owner.Parent.AccessibilityObject;
				return null;
			}
		}

		/// <summary>
		/// Returns bounds of the control.
		/// </summary>
		public override Rectangle Bounds 
		{
			get
			{
				if(m_Owner!=null && !m_Owner.IsDisposed && m_Owner.Parent!=null)
					return this.m_Owner.Parent.RectangleToScreen(m_Owner.Bounds);
				return Rectangle.Empty;
			}
		}

		/// <summary>
		/// Returns number of child objects.
		/// </summary>
		/// <returns>Total number of child objects.</returns>
		public override int GetChildCount()
		{
			if(m_Owner!=null && !m_Owner.IsDisposed && m_Owner.GetBaseItemContainer()!=null)
				return m_Owner.GetBaseItemContainer().SubItems.Count;
			return 0;
		}

		/// <summary>
		/// Returns reference to child object given the index.
		/// </summary>
		/// <param name="iIndex">0 based index of child object.</param>
		/// <returns>Reference to child object.</returns>
		public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
		{
			if(m_Owner!=null && !m_Owner.IsDisposed && m_Owner.GetBaseItemContainer()!=null)
				return m_Owner.GetBaseItemContainer().SubItems[iIndex].AccessibleObject;
			return null;
		}

		/// <summary>
		/// Returns current accessible state.
		/// </summary>
		public override AccessibleStates State
		{
			get
			{
				AccessibleStates state;
				if(m_Owner==null || m_Owner.IsDisposed)
					return AccessibleStates.None;

				state=AccessibleStates.None;
				return state;
			}
		}
	}
	#endregion
}
