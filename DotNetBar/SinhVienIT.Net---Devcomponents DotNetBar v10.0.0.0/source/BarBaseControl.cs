using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents base control for bars.
	/// </summary>
	[ToolboxItem(false),System.Runtime.InteropServices.ComVisible(false)]
	public abstract class BarBaseControl: System.Windows.Forms.Control,
        IOwner, IOwnerMenuSupport, IMessageHandlerClient, IOwnerItemEvents, IThemeCache, IOwnerLocalize, IBarDesignerServices, IRenderingSupport,
        IAccessibilitySupport
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
		[System.ComponentModel.Description("Occurs when Item is clicked.")]
		public event EventHandler ItemClick;

        /// <summary>
        /// Occurs when Item is clicked.
        /// </summary>
        [Description("Occurs when Item is double-clicked.")]
        public event MouseEventHandler ItemDoubleClick;

		/// <summary>
		/// Occurs when popup of type container is loading.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup of type container is loading.")]
		public event EventHandler PopupContainerLoad;

		/// <summary>
		/// Occurs when popup of type container is unloading.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup of type container is unloading.")]
		public event EventHandler PopupContainerUnload;

		/// <summary>
		/// Occurs when popup item is about to open.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is about to open.")]
		public event EventHandler PopupOpen;

		/// <summary>
		/// Occurs when popup item is closing.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is closing.")]
		public event EventHandler PopupClose;

		/// <summary>
		/// Occurs just before popup window is shown.
		/// </summary>
		[System.ComponentModel.Description("Occurs just before popup window is shown.")]
		public event EventHandler PopupShowing;

		/// <summary>
		/// Occurs when Item Expanded property has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Item Expanded property has changed.")]
		public event EventHandler ExpandedChange;

		private MouseEventHandler EventMouseDown;
		/// <summary>
		/// Occurs when mouse button is pressed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse button is pressed.")]
		new public event MouseEventHandler MouseDown 
		{
			add { EventMouseDown += value; }
			remove { EventMouseDown -= value; }
		}

		private MouseEventHandler EventMouseUp;
		/// <summary>
		/// Occurs when mouse button is released.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse button is released.")]
		new public event MouseEventHandler MouseUp
		{
			add { EventMouseUp += value; }
			remove { EventMouseUp -= value; }
		}

		private EventHandler EventMouseEnter;
		/// <summary>
		/// Occurs when mouse enters the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse enters the item.")]
		new public event EventHandler MouseEnter
		{
			add { EventMouseEnter += value; }
			remove { EventMouseEnter -= value; }
		}
		
		private EventHandler EventMouseLeave;
		/// <summary>
		/// Occurs when mouse leaves the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse leaves the item.")]
		new public event EventHandler MouseLeave
		{
			add { EventMouseLeave += value; }
			remove { EventMouseLeave -= value; }
		}

		private MouseEventHandler EventMouseMove;
		/// <summary>
		/// Occurs when mouse moves over the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse moves over the item.")]
		new public event MouseEventHandler MouseMove
		{
			add { EventMouseMove += value; }
			remove { EventMouseMove -= value; }
		}
		
		private EventHandler EventMouseHover;
		/// <summary>
		/// Occurs when mouse remains still inside an item for an amount of time.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse remains still inside an item for an amount of time.")]
		new public event EventHandler MouseHover
		{
			add { EventMouseHover += value; }
			remove { EventMouseHover -= value; }
		}
		
		private EventHandler EventLostFocus;
		/// <summary>
		/// Occurs when item loses input focus.
		/// </summary>
		[System.ComponentModel.Description("Occurs when item loses input focus.")]
		new public event EventHandler LostFocus
		{
			add { EventLostFocus += value; }
			remove { EventLostFocus -= value; }
		}
		
		private EventHandler EventGotFocus;
		/// <summary>
		/// Occurs when item receives input focus.
		/// </summary>
		[System.ComponentModel.Description("Occurs when item receives input focus.")]
		new public event EventHandler GotFocus
		{
			add { EventGotFocus += value; }
			remove { EventGotFocus -= value; }
		}
		
		/// <summary>
		/// Occurs when user changes the item position, removes the item, adds new item or creates new bar.
		/// </summary>
		[System.ComponentModel.Description("Occurs when user changes the item position.")]
		public event EventHandler UserCustomize;

		/// <summary>
		/// Occurs after an Item is removed from SubItemsCollection.
		/// </summary>
		[System.ComponentModel.Description("Occurs after an Item is removed from SubItemsCollection.")]
		public event ItemRemovedEventHandler ItemRemoved;

		/// <summary>
		/// Occurs after an Item has been added to the SubItemsCollection.
		/// </summary>
		[System.ComponentModel.Description("Occurs after an Item has been added to the SubItemsCollection.")]
		public event EventHandler ItemAdded;

		/// <summary>
		/// Occurs when ControlContainerControl is created and contained control is needed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when ControlContainerControl is created and contained control is needed.")]
		public event EventHandler ContainerLoadControl;

		/// <summary>
		/// Occurs when Text property of an Item has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Text property of an Item has changed.")]
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
		[System.ComponentModel.Description("Occurs after DotNetBar definition is loaded.")]
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

        /// <summary>
        /// Occurs after main application form is activated.
        /// </summary>
        internal event EventHandler ApplicationActivate;
        /// <summary>
        /// Occurs after main application form is deacticated.
        /// </summary>
        internal event EventHandler ApplicationDeactivate;
        /// <summary>
        /// Occurs on application wide mouse down event.
        /// </summary>
        internal event HandlerMessageEventHandler ApplicationMouseDown;
		#endregion

		#region Private Variables
		private BaseItem m_BaseItemContainer=null;
		private BaseItem m_ExpandedItem=null;
		private BaseItem m_FocusItem=null;
		private Hashtable m_ShortcutTable=new Hashtable();
		private System.Windows.Forms.ImageList m_ImageList;
		private System.Windows.Forms.ImageList m_ImageListMedium=null;
		private System.Windows.Forms.ImageList m_ImageListLarge=null;
		private BaseItem m_DragItem=null;
		private bool m_DragDropSupport=false;
		private bool m_DragLeft=false;
		private bool m_AllowExternalDrop=false;
		private bool m_UseNativeDragDrop=false;
		private bool m_DragInProgress=false;
		private bool m_ExternalDragInProgress=false;
		private Cursor m_MoveCursor, m_CopyCursor, m_NACursor;
		private bool m_ShowToolTips=true;
		private bool m_ShowShortcutKeysInToolTips=false;
		private bool m_FilterInstalled=false;
		private bool m_DispatchShortcuts=false;
		private bool m_MenuEventSupport=false;
		private IDesignTimeProvider m_DesignTimeProvider=null;
		private int m_InsertPosition;
		private bool m_InsertBefore=false;
		private System.Windows.Forms.Timer m_ClickTimer=null;
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
		private ItemStyle m_BackgroundStyle=new ItemStyle();
		private bool m_DesignModeInternal=false;
		private IBarItemDesigner m_BarDesigner=null;
        private BaseItem m_DoDefaultActionItem = null;
        private bool m_AntiAlias = false;
		#endregion

		#region Constructor
		public BarBaseControl()
		{
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

			try
			{
				m_MoveCursor=new Cursor(typeof(DevComponents.DotNetBar.DotNetBarManager),"DRAGMOVE.CUR");
				m_CopyCursor=new Cursor(typeof(DevComponents.DotNetBar.DotNetBarManager),"DRAGCOPY.CUR");
				m_NACursor=new Cursor(typeof(DevComponents.DotNetBar.DotNetBarManager),"DRAGNONE.CUR");
			}
			catch(Exception)
			{
				m_MoveCursor=null;
				m_CopyCursor=null;
				m_NACursor=null;
			}

			m_ColorScheme=new ColorScheme(eDotNetBarStyle.Office2003);

			m_BackgroundStyle.BackColor1.Color=SystemColors.Control;
			m_BackgroundStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);

			this.IsAccessible=true;
		}

		protected bool GetDesignMode()
		{
			if(!m_DesignModeInternal)
				return this.DesignMode;
			return m_DesignModeInternal;
		}

		internal void SetDesignMode(bool mode)
		{
			m_DesignModeInternal=mode;
			m_BaseItemContainer.SetDesignMode(mode);
		}

		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new BarBaseControlAccessibleObject(this);
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

		bool IOwner.AlwaysDisplayKeyAccelerators
		{
			get {return false;}
			set {}
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

		void IOwner.OnApplicationActivate()
        {
            if (ApplicationActivate != null)
                ApplicationActivate(this, new EventArgs());
        }
		void IOwner.OnApplicationDeactivate()
		{
            ClosePopups();
            if (ApplicationDeactivate != null)
                ApplicationDeactivate(this, new EventArgs());
		}
		void IOwner.OnParentPositionChanging(){}

		void IOwner.StartItemDrag(BaseItem item)
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
						System.Windows.Forms.Cursor.Current=m_MoveCursor;
					else
						System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Hand;
					m_DragInProgress=true;
				}
				else
				{
					m_DragInProgress=true;
					this.DoDragDrop(item,DragDropEffects.All);
					if(m_DragInProgress)
						MouseDragDrop(-1,-1,null);
				}
					
			}
		}

		bool IOwner.DragInProgress
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

		System.Windows.Forms.MdiClient IOwner.GetMdiClient(System.Windows.Forms.Form MdiForm)
		{
			return BarFunctions.GetMdiClient(MdiForm);
		}

		/// <summary>
		/// ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),DefaultValue(null),System.ComponentModel.Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
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
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),DefaultValue(null),System.ComponentModel.Description("ImageList for medium-sized images used on Items.")]
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
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),DefaultValue(null),System.ComponentModel.Description("ImageList for large-sized images used on Items.")]
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
		[System.ComponentModel.Browsable(true),DefaultValue(true),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Indicates whether Tooltips are shown on Bars and menus.")]
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
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Indicates whether item shortcut is displayed in Tooltips.")]
		public bool ShowShortcutKeysInToolTips
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
		// IOwnerMenuSupport
        private Hook m_Hook = null;
		private ArrayList m_RegisteredPopups=new ArrayList();
		bool IOwnerMenuSupport.PersonalizedAllVisible {get{return false;}set{}}
		bool IOwnerMenuSupport.ShowFullMenusOnHover {get{return true;}set{}}
		bool IOwnerMenuSupport.AlwaysShowFullMenus {get{return false;}set{}}

		void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
		{
			if(m_RegisteredPopups.Contains(objPopup))
				return;
			
			if(!this.GetDesignMode())
			{
				if(!m_FilterInstalled)
				{
					//System.Windows.Forms.Application.AddMessageFilter(this);
					MessageHandler.RegisterMessageClient(this);
					m_FilterInstalled=true;
				}	
			}
            else
            {
                if (m_Hook == null)
                {
                    m_Hook = new Hook(this);
                }
            }

			if(!m_MenuEventSupport)
				MenuEventSupportHook();

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

        private void ClosePopups()
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
		bool IOwnerMenuSupport.ShowPopupShadow {get{return false;}}
		eMenuDropShadow IOwnerMenuSupport.MenuDropShadow{get{return eMenuDropShadow.Hide;}set{}}
		ePopupAnimation IOwnerMenuSupport.PopupAnimation{get {return ePopupAnimation.SystemDefault;}set{}}
		bool IOwnerMenuSupport.AlphaBlendShadow{get {return true;}set{}}
		#endregion

		#region IOwnerItemEvents Implementation
        void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
        {
            if (ButtonCheckedChanged != null)
                ButtonCheckedChanged(item, e);
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
		void IOwnerItemEvents.InvokeMouseDown(BaseItem item, System.Windows.Forms.MouseEventArgs e)
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
		void IOwnerItemEvents.InvokeMouseUp(BaseItem item, System.Windows.Forms.MouseEventArgs e)
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
		void IOwnerItemEvents.InvokeMouseMove(BaseItem item, System.Windows.Forms.MouseEventArgs e)
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
			if(ItemClick!=null)
				ItemClick(objItem,new EventArgs());
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
            return false;
        }
		bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			if(m_RegisteredPopups.Count>0)
			{
				if(((BaseItem)m_RegisteredPopups[m_RegisteredPopups.Count-1]).Parent==null)
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
						Keys key=(Keys)NativeFunctions.MapVirtualKey((uint)wParam,2);
						if(key==Keys.None)
							key=(Keys)wParam.ToInt32();
						objItem.InternalKeyDown(new KeyEventArgs(key));
					}

					// Don't eat the message if the pop-up window has focus
					if(bNoEat)
						return false;
					return true;
				}
			}

			if(!this.IsParentFormActive)
				return false;

			if(wParam.ToInt32()>=0x70 || System.Windows.Forms.Control.ModifierKeys!=Keys.None || (lParam.ToInt32() & 0x1000000000)!=0 || wParam.ToInt32()==0x2E || wParam.ToInt32()==0x2D) // 2E=VK_DELETE, 2D=VK_INSERT
			{
				int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
				return ProcessShortcut((eShortcut)i);
			}
			return false;
		}
		private bool ProcessShortcut(eShortcut key)
		{
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
		bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            if(ApplicationMouseDown!=null)
                ApplicationMouseDown(this,new HandlerMessageEventArgs(hWnd,wParam,lParam));

			if(m_RegisteredPopups.Count==0)
				return false;

			for(int i=m_RegisteredPopups.Count-1;i>=0;i--)
			{
				PopupItem objPopup=m_RegisteredPopups[i] as PopupItem;
				bool bChildHandle=objPopup.IsAnyOnHandle(hWnd.ToInt32());

				if(!bChildHandle)
				{
                    System.Windows.Forms.Control cTmp = System.Windows.Forms.Control.FromChildHandle(hWnd);
                    if (cTmp != null)
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
                        if (!bChildHandle)
                            bChildHandle = objPopup.IsAnyOnHandle(cTmp.Handle.ToInt32());
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

				if(objPopup.Displayed)
				{
					// Do not close if mouse is inside the popup parent button
					Point p=this.PointToClient(Control.MousePosition);
					if(objPopup.DisplayRectangle.Contains(p))
						break;
				}
				
				objPopup.ClosePopup();

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
		bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			if(!this.GetDesignMode())
			{
				// Check Shortcuts
				if(System.Windows.Forms.Control.ModifierKeys!=Keys.None || wParam.ToInt32()>=(int)eShortcut.F1 && wParam.ToInt32()<=(int)eShortcut.F12)
				{
					int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
					if(ProcessShortcut((eShortcut)i))
						return true;
				}
			}
			return false;
		}
		bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
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

		#region IThemeCache Implementation
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_THEMECHANGED)
			{
				this.RefreshThemes();
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
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if(!m_FilterInstalled && !this.DesignMode)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled=true;
			}
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
		#endregion

		#region Mouse & Keyboard Support
		protected override void OnClick(EventArgs e)
		{
			m_BaseItemContainer.InternalClick(Control.MouseButtons,Control.MousePosition);
			base.OnClick(e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			m_BaseItemContainer.InternalDoubleClick(Control.MouseButtons,Control.MousePosition);
			base.OnDoubleClick(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			ExKeyDown(e);
			base.OnKeyDown(e);
		}

		internal void ExKeyDown(KeyEventArgs e)
		{
			m_BaseItemContainer.InternalKeyDown(e);
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
			if(this.Cursor!=System.Windows.Forms.Cursors.Arrow)
				this.Cursor=System.Windows.Forms.Cursors.Arrow;
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
				Point p=this.PointToClient(new Point(e.X,e.Y));
				MouseDragOver(p.X,p.Y,e);
				m_DragLeft=false;
			}
			base.OnDragOver(e);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if(m_DragDropSupport)
			{
				if(m_DragInProgress || m_ExternalDragInProgress)
					MouseDragOver(-1,-1,null);
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
			if(m_DragDropSupport)
			{
				if(m_DragInProgress)
				{
					if(m_DragLeft && e.Action==DragAction.Drop || e.Action==DragAction.Cancel)
						MouseDragDrop(-1,-1,null);
				}
			}
			base.OnQueryContinueDrag(e);
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
			foreach(SideBarPanelItem panel in m_BaseItemContainer.SubItems)
			{
				if(!panel.Visible)
					continue;
				InsertPosition pos=((IDesignTimeProvider)panel).GetInsertPosition(pScreen, dragItem);
				
				if(pos!=null)
				{
					if(pos.TargetProvider==null)
					{
						// Cursor is over drag item
						if(!m_UseNativeDragDrop)
						{
							if(m_NACursor!=null)
								System.Windows.Forms.Cursor.Current=m_NACursor;
							else
								System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.No;
						}
						break;
					}
					pos.TargetProvider.DrawReversibleMarker(pos.Position,pos.Before);
					m_InsertPosition=pos.Position;
					m_InsertBefore=pos.Before;
					m_DesignTimeProvider=pos.TargetProvider;
					if(!m_UseNativeDragDrop)
					{
						if(m_MoveCursor!=null)
							System.Windows.Forms.Cursor.Current=m_MoveCursor;
						else
							System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Hand;
					}
					else if(dragArgs!=null)
						dragArgs.Effect=DragDropEffects.Move;
					break;
				}
				else
				{
					if(!m_UseNativeDragDrop)
					{
						if(m_NACursor!=null)
							System.Windows.Forms.Cursor.Current=m_NACursor;
						else
							System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.No;
					}
					else if(dragArgs!=null)
						dragArgs.Effect=DragDropEffects.None;
				}
			}
		}
		#endregion

		#region IOwnerLocalize Implementation
		void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
		{
			if(LocalizeString!=null)
				LocalizeString(this,e);
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

		protected void SetBaseItemContainer(BaseItem item)
		{
			m_BaseItemContainer=item;
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public BaseItem GetBaseItemContainer()
		{
			return m_BaseItemContainer;
		}

#if FRAMEWORK20
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
            if (m_BaseItemContainer != null)
                m_BaseItemContainer.UpdateBindings();
        }
#endif

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
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Specifies the background style of the Explorer Bar.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Gets or sets bar background style."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle BackgroundStyle
		{
			get {return m_BackgroundStyle;}
		}

		public void ResetBackgroundStyle()
		{
			m_BackgroundStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_BackgroundStyle=new ItemStyle();
			m_BackgroundStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

		private void VisualPropertyChanged(object sender, EventArgs e)
		{
			if(m_BackgroundStyle!=null)
				m_BackgroundStyle.ApplyColorScheme(this.ColorScheme);
			if(this.GetDesignMode())
				this.Refresh();
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
							Control ctrl=objParent.ContainerControl as Control;
							objParent.Refresh();
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
				System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Default;
			this.Capture=false;
			if(dragItem!=null)
				dragItem._IgnoreClick=true;
			m_BaseItemContainer.InternalMouseUp(new MouseEventArgs(MouseButtons.Left,0,x,y,0));
			if(dragItem!=null)
				dragItem._IgnoreClick=false;

			m_DragItem=null;
		}

		/// <summary>
		/// Indicates whether shortucts handled by items are dispatched to the next handler or control.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
		public bool DispatchShortcuts
		{
			get {return m_DispatchShortcuts;}
			set {m_DispatchShortcuts=value;}
		}

		/// <summary>
		/// Gets or sets Bar Color Scheme.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Category("Appearance"),Description("Gets or sets Bar Color Scheme."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColorScheme()
		{
			return m_ColorScheme.SchemeChanged;
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

		/// <summary>
		/// Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.
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

		#region Painting Support
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ColorScheme GetColorScheme()
        {
            if (BarFunctions.IsOffice2007Style(m_BaseItemContainer.EffectiveStyle))
            {
                Office2007Renderer r = this.GetRenderer() as Office2007Renderer;
                if (r != null && r.ColorTable.LegacyColors != null)
                    return r.ColorTable.LegacyColors;
            }
            return m_ColorScheme;
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			if(m_BaseItemContainer==null || this.IsDisposed)
				return;

			ItemPaintArgs pa=new ItemPaintArgs(this as IOwner,this,e.Graphics,GetColorScheme());
            pa.Renderer = this.GetRenderer();
            if (m_AntiAlias)
            {
                pa.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                pa.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

			PaintControlBackground(pa);

			pa.Graphics.SetClip(this.GetItemContainerRectangle());
			PaintItemContainer(pa);
			pa.Graphics.ResetClip();

			if(m_BaseItemContainer.SubItems.Count==0 && this.GetDesignMode())
				PaintDesignTimeEmptyHint(pa);
		}

		protected virtual void PaintControlBackground(ItemPaintArgs pa)
		{
            if (m_BackgroundStyle != null)
            {
                m_BackgroundStyle.ApplyColorScheme(pa.Colors);
                m_BackgroundStyle.Paint(pa.Graphics, this.ClientRectangle);
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
            eTextFormat format = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			TextDrawing.DrawString(pa.Graphics,info,this.Font,SystemColors.ControlDark,rText,format);
		}
		#endregion

		#region Layout Support
		protected virtual Rectangle GetItemContainerRectangle()
		{
			if(m_BackgroundStyle==null)
				return this.ClientRectangle;

			Rectangle r=this.ClientRectangle;
			
			if(m_BackgroundStyle.Border==eBorderType.SingleLine)
			{
				if(m_BackgroundStyle.BorderSide==eBorderSide.All)
					r.Inflate(-1,-1);
				else
				{
					if((m_BackgroundStyle.BorderSide & eBorderSide.Left)!=0)
						r.X++;
					if((m_BackgroundStyle.BorderSide & eBorderSide.Right)!=0)
						r.Width--;
					if((m_BackgroundStyle.BorderSide & eBorderSide.Top)!=0)
						r.Y++;
					if((m_BackgroundStyle.BorderSide & eBorderSide.Bottom)!=0)
						r.Height--;
				}
			}
			else if(m_BackgroundStyle.Border!=eBorderType.None)
			{
				if(m_BackgroundStyle.BorderSide==eBorderSide.All)
					r.Inflate(-2,-2);
				else
				{
					if((m_BackgroundStyle.BorderSide & eBorderSide.Left)!=0)
						r.X+=2;
					if((m_BackgroundStyle.BorderSide & eBorderSide.Right)!=0)
						r.Width-=2;
					if((m_BackgroundStyle.BorderSide & eBorderSide.Top)!=0)
						r.Y+=2;
					if((m_BackgroundStyle.BorderSide & eBorderSide.Bottom)!=0)
						r.Height-=2;
				}
			}

			return r;
		}

		protected virtual void RecalcSize()
		{
			if(m_BaseItemContainer.IsRecalculatingSize)
				return;
			Rectangle r=this.GetItemContainerRectangle();
            m_BaseItemContainer.IsRightToLeft = (this.RightToLeft == RightToLeft.Yes);
			m_BaseItemContainer.LeftInternal=r.X;
			m_BaseItemContainer.TopInternal=r.Y;
			m_BaseItemContainer.WidthInternal =r.Width;
			m_BaseItemContainer.HeightInternal=r.Height;

			m_BaseItemContainer.RecalcSize();

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

		/// <summary>
		/// Applies any layout changes and repaint the control.
		/// </summary>
		public virtual void RecalcLayout()
		{
			if(m_BaseItemContainer.IsRecalculatingSize)
				return;
			this.RecalcSize();
			this.Invalidate();
		}
		#endregion

		#region IBarDesignerServices
		IBarItemDesigner IBarDesignerServices.Designer
		{
			get {return m_BarDesigner;}
			set {m_BarDesigner=value;}
		}
		#endregion
	}

	#region HandlerMessageEventArgs
    internal class HandlerMessageEventArgs : EventArgs
    {
        public IntPtr hWnd = IntPtr.Zero;
        public IntPtr wParam = IntPtr.Zero;
        public IntPtr lParam = IntPtr.Zero;

        public HandlerMessageEventArgs(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            this.hWnd = hwnd;
            this.wParam = wparam;
            this.lParam = lparam;
        }
    }

    internal delegate void HandlerMessageEventHandler(object sender, HandlerMessageEventArgs e);
	#endregion


	#region BarAccessibleObject
	/// <summary>
	/// Represents class for Accessibility support.
	/// </summary>
	public class BarBaseControlAccessibleObject : System.Windows.Forms.Control.ControlAccessibleObject
	{
		BarBaseControl m_Owner = null;
		/// <summary>
		/// Creates new instance of the object and initializes it with owner control.
		/// </summary>
		/// <param name="owner">Reference to owner control.</param>
		public BarBaseControlAccessibleObject(BarBaseControl owner):base(owner)
		{
			m_Owner = owner;
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
