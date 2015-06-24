[assembly:System.CLSCompliant(true)]
namespace DevComponents.DotNetBar
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Windows.Forms;
    using System.Xml;

    /// <summary>
    /// Represent the menu, toolbar and popup menu structure for the form.
    /// </summary>
    [ToolboxBitmap(typeof(DotNetBarManager), "DotNetBarManager.ico"), Designer("DevComponents.DotNetBar.Design.DotNetBarManagerDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ToolboxItem(true), System.Runtime.InteropServices.ComVisible(false), DefaultEvent("ItemClick")]
	public class DotNetBarManager : Component,
		System.ComponentModel.IExtenderProvider,
		IOwnerItemEvents,
		IOwnerMenuSupport,
		IOwner,
		IOwnerBarSupport,
		IOwnerAutoHideSupport,
		IMessageHandlerClient,
		IOwnerLocalize,
        ICustomSerialization
	{
		// Events
		#region Event Definition
        /// <summary>
        /// Occurs when focused (active) DockContainerItem has changed. You can use ActiveDockContainerItem property to get reference to currently focused DockContainerItem.
        /// </summary>
        [Description("Occurs when focused (active) DockContainerItem has changed. You can use ActiveDockContainerItem property to get reference to currently focused DockContainerItem.")]
        public event ActiveDockContainerChangedEventHandler ActiveDockContainerChanged;
		/// <summary>
		/// Represents delegate for ContextMenu events.
		/// </summary>
		public delegate void CustomizeContextMenuEventHandler(object sender, CustomizeContextMenuEventArgs e);

		/// <summary>
		/// Represents the method that will handle the ItemRemoved event.
		/// </summary>
		public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

		/// <summary>
		/// Occurs just before customize popup menu is shown.
		/// </summary>
		[System.ComponentModel.Description("Occurs just before customize popup menu is shown."),Category("Item")]
		public event CustomizeContextMenuEventHandler CustomizeContextMenu;

		/// <summary>
		/// Occurs when Item is clicked.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Item is clicked."),Category("Item")]
		public event EventHandler ItemClick;

        /// <summary>
        /// Occurs when Item is clicked.
        /// </summary>
        [Description("Occurs when Item is double-clicked.")]
        public event MouseEventHandler ItemDoubleClick;

		/// <summary>
		/// Occurs when popup of type container is loading.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup of type container is loading."),Category("Item")]
		public event EventHandler PopupContainerLoad;

		/// <summary>
		/// Occurs when popup of type container is unloading.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup of type container is unloading."),Category("Item")]
		public event EventHandler PopupContainerUnload;

		public delegate void PopupOpenEventHandler(object sender, PopupOpenEventArgs e);
		/// <summary>
		/// Occurs when popup item is about to open.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is about to open."),Category("Item")]
		public event PopupOpenEventHandler PopupOpen;

		/// <summary>
		/// Occurs when popup item is closing.
		/// </summary>
		[System.ComponentModel.Description("Occurs when popup item is closing."),Category("Item")]
		public event EventHandler PopupClose;

		/// <summary>
		/// Occurs just before popup window is shown.
		/// </summary>
		[System.ComponentModel.Description("Occurs just before popup window is shown."),Category("Item")]
		public event EventHandler PopupShowing;

		/// <summary>
		/// Occurs when Item Expanded property has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Item Expanded property has changed."),Category("Item")]
		public event EventHandler ExpandedChange;

		/// <summary>
		/// Occurs when Bar is docked.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Bar is docked."),Category("Bar")]
		public event EventHandler BarDock;

		/// <summary>
		/// Occurs when Bar is Undocked.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Bar is Undocked."),Category("Bar")]
		public event EventHandler BarUndock;

		/// <summary>
		/// Occurs before dock tab is displayed.
		/// </summary>
		[System.ComponentModel.Description("Occurs before dock tab is displayed."),Category("Bar")]
		public event EventHandler BeforeDockTabDisplay;

		/// <summary>
		/// Occurs when Bar auto-hide state has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Bar auto-hide state has changed."),Category("Bar")]
		public event EventHandler AutoHideChanged;

		/// <summary>
		/// Occurs when mouse button is pressed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse button is pressed."),Category("Item")]
		public event MouseEventHandler MouseDown;

		/// <summary>
		/// Occurs when mouse button is released.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse button is released."),Category("Item")]
		public event MouseEventHandler MouseUp;

		/// <summary>
		/// Occurs when mouse enters the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse enters the item."),Category("Item")]
		public event EventHandler MouseEnter;

		/// <summary>
		/// Occurs when mouse leaves the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse leaves the item."),Category("Item")]
		public event EventHandler MouseLeave;

		/// <summary>
		/// Occurs when mouse moves over the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse moves over the item."),Category("Item")]
		public event MouseEventHandler MouseMove;

		/// <summary>
		/// Occurs when mouse remains still inside an item for an amount of time.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse remains still inside an item for an amount of time."),Category("Item")]
		public event EventHandler MouseHover;

		/// <summary>
		/// Occurs when item loses input focus.
		/// </summary>
		[System.ComponentModel.Description("Occurs when item loses input focus."),Category("Item")]
		public event EventHandler LostFocus;

		/// <summary>
		/// Occurs when item receives input focus.
		/// </summary>
		[System.ComponentModel.Description("Occurs when item receives input focus."),Category("Item")]
		public event EventHandler GotFocus;

		/// <summary>
		/// Occurs when user changes the item position, removes the item, adds new item or creates new bar.
		/// </summary>
		[System.ComponentModel.Description("Occurs when user changes the item position, removes the item, adds new item or creates new bar."),Category("Item")]
		public event EventHandler UserCustomize;

		/// <summary>
		/// Occurs after DotNetBar definition is loaded.
		/// </summary>
		[System.ComponentModel.Description("Occurs after DotNetBar definition is loaded."),Category("General")]
		public event EventHandler DefinitionLoaded;

		/// <summary>
		/// Occurs when users wants to reset the DotNetBar to default state.
		/// </summary>
		[System.ComponentModel.Description("Occurs when users wants to reset the DotNetBar to default state."),Category("General")]
		public event EventHandler ResetDefinition;

		/// <summary>
		/// Occurs after an Item is removed from SubItemsCollection.
		/// </summary>
		[System.ComponentModel.Description("Occurs after an Item is removed from SubItemsCollection."),Category("Item")]
		public event ItemRemovedEventHandler ItemRemoved;

		/// <summary>
		/// Occurs after an Item has been added to the SubItemsCollection.
		/// </summary>
		[System.ComponentModel.Description("Occurs after an Item has been added to the SubItemsCollection."),Category("Item")]
		public event EventHandler ItemAdded;

		/// <summary>
		/// Occurs when ControlContainerControl is created and contained control is needed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when ControlContainerControl is created and contained control is needed."),Category("Item")]
		public event EventHandler ContainerLoadControl;

		/// <summary>
		/// Occurs when Text property of an Item has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Text property of an Item has changed."),Category("Item")]
		public event EventHandler ItemTextChanged;

		/// <summary>
		/// Occurs when Customize Dialog is about to be shown.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Customize Dialog is about to be shown."),Category("General")]
		public event EventHandler EnterCustomize;

		/// <summary>
		/// Occurs when Customize Dialog is closed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Customize Dialog is closed."),Category("General")]
		public event EventHandler ExitCustomize;

		/// <summary>
		/// Use this event if you want to serialize the hosted control state directly into the DotNetBar definition file.
		/// </summary>
		public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize;
		/// <summary>
		/// Use this event if you want to deserialize the hosted control state directly from the DotNetBar definition file.
		/// </summary>
		public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize;

		/// <summary>
		/// Defines the delegate for DockTabChange event
		/// </summary>
		public delegate void DockTabChangeEventHandler(object sender, DockTabChangeEventArgs e);
		/// <summary>
		/// Occurs when current Dock tab has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when current Dock tab has changed."),Category("Bar")]
		public event DockTabChangeEventHandler DockTabChange;

		/// <summary>
		/// Defines the delegate for BarClosing event
		/// </summary>
		public delegate void BarClosingEventHandler(object sender, BarClosingEventArgs e);
		/// <summary>
		/// Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar."),Category("Bar")]
		public event BarClosingEventHandler BarClosing;

		/// <summary>
		/// Defines the delegate for BarAutoHideDisplay event
		/// </summary>
		public delegate void AutoHideDisplayEventHandler(object sender, AutoHideDisplayEventArgs e);
		/// <summary>
		/// Occurs when Bar in auto-hide state is about to be displayed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Bar in auto-hide state is about to be displayed."),Category("Bar")]
		public event AutoHideDisplayEventHandler AutoHideDisplay;

		/// <summary>
		/// Occurs when user starts to drag the item when customize dialog is open.
		/// </summary>
		[System.ComponentModel.Description("Occurs when user start to drag item when customize dialog is open.")]
		public event EventHandler CustomizeStartItemDrag;

		/// <summary>
		/// Occurs when users Tears-off the Tab from the Bar and new Bar is created as result of that action.
		/// </summary>
		[System.ComponentModel.Description("Occurs when users Tears-off the Tab from the Bar and new Bar is created as result of that action.")]
		public event EventHandler BarTearOff;

		/// <summary>
		/// Represents the method that will handle the LocalizeString event.
		/// </summary>
		public delegate void LocalizeStringEventHandler(object sender, LocalizeEventArgs e);

		/// <summary>
		/// Occurs when DotNetBar is looking for translated text for one of the internal text that are
		/// displayed on menus, toolbars and customize forms. You need to set Handled=true if you want
		/// your custom text to be used instead of the built-in system value.
		/// </summary>
		public event LocalizeStringEventHandler LocalizeString;

		/// <summary>
		/// Occurs before an item in option group is checked and provides opportunity to cancel the change.
		/// </summary>
		public event OptionGroupChangingEventHandler OptionGroupChanging;

		/// <summary>
		/// Occurs before tooltip for an item is shown. Sender could be the BaseItem or derived class for which tooltip is being displayed or it could be a ToolTip object itself it tooltip is not displayed for any item in particular.
		/// </summary>
		public event EventHandler ToolTipShowing;

		public event EndUserCustomizeEventHandler EndUserCustomize;

		/// <summary>
		/// Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.
		/// Event can be canceled by setting the Cancel property of event arguments to true. This even will occur only after user presses the
		/// X button on tab that is displaying the dockable windows/documents.
		/// </summary>
		[System.ComponentModel.Description("Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.")]
		public event DockTabClosingEventHandler DockTabClosing;

        /// <summary>
        /// Occurs on dockable bars after DockContainerItem is closed. This action cannot be cancelled.
        /// </summary>
        [System.ComponentModel.Description("Occurs on dockable bars after DockContainerItem is closed. This event cannot be cancelled.")]
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
        /// Occurs when TextBoxItem input text has changed.
        /// </summary>
        public event EventHandler TextBoxItemTextChanged;

        /// <summary>
        /// Occurs when color on ColorPickerDropDown is choosen from drop-down color picker or from Custom Colors dialog box. Selected color can be accessed through SelectedColor property.
        /// </summary>
        [Description("Occurs when color is choosen from drop-down color picker or from Custom Colors dialog box.")]
        public event EventHandler ColorPickerSelectedColorChanged;

        /// <summary>
        /// Occurs when Checked property of an button has changed.
        /// </summary>
        public event EventHandler ButtonCheckedChanged;
		#endregion

		#region Private Variables
		private static Hashtable m_MdiChildParentHandlers=new Hashtable(10);
		private License m_License = null;

		private DockSite m_TopDockSite;
		private DockSite m_BottomDockSite;
		private DockSite m_LeftDockSite;
		private DockSite m_RightDockSite;
		private DockSite m_FillDockSite;

        private DockSite m_ToolbarTopDockSite;
        private DockSite m_ToolbarBottomDockSite;
        private DockSite m_ToolbarLeftDockSite;
        private DockSite m_ToolbarRightDockSite;

		private Bars m_Bars;
		private ArrayList m_WereVisible;
		//private ParentMsgHandler m_MsgHandler;
		private Items m_Items;
		private Hashtable m_ShortcutTable;
		private bool m_FilterInstalled;
		private frmCustomize m_frmCustomize;
		private ArrayList m_DisabledControls;
		private BaseItem m_FocusItem;
		private BaseItem m_ExpandedItem;

		private ArrayList m_RegisteredPopups;
		private MDIClientMsgHandler m_MdiHandler;

		private System.Windows.Forms.Form m_ActiveMdiChild;
		private bool m_MdiChildMaximized;

		private bool m_IgnoreSysKeyUp=false, m_EatSysKeyUp=false;

		private System.Windows.Forms.ImageList m_ImageList;
		private System.Windows.Forms.ImageList m_ImageListMedium=null;
		private System.Windows.Forms.ImageList m_ImageListLarge=null;

		private DotNetBarStreamer m_BarStreamer=null;
		private Form m_ParentForm;
		private bool m_Disposed=false;
		private bool m_ParentActivated=false;

		private bool m_PersonalizedAllVisible=false;

		private System.Windows.Forms.Timer m_ClickTimer=null;
		private BaseItem m_ClickRepeatItem=null;

		private bool m_ShowCustomizeContextMenu=true;
		private PopupItem m_ContextMenu=null;

		private ContextMenusCollection m_ContextMenus=null;
		private eMenuDropShadow m_MenuDropShadow=eMenuDropShadow.SystemDefault;
		private bool m_AlphaBlendShadow=true;
		private bool m_UseHook=false;
		private Hook m_Hook=null;
		private bool m_ShowResetButton=false;

		private bool m_AlwaysShowFullMenus=false;
		private bool m_ShowFullMenusOnHover=true;
		private bool m_ShowToolTips=true;
		private bool m_ShowShortcutKeysInToolTips=false;
		private ePopupAnimation m_PopupAnimation=ePopupAnimation.SystemDefault;

		// Used to provide Context-Menu extender property...
		private Hashtable m_ContextExMenus=new Hashtable();
		private Hashtable m_ContextExHandlers=new Hashtable();

		private bool m_AllowUserBarCustomize=true;
		private bool m_EventControlAdded=false;
		private bool m_MdiChildActivateHandled=false;
		private bool m_MdiClientHandleCreatedHandled=false;
		private bool m_UseCustomCustomizeDialog=false;
		private bool m_MdiSystemItemVisible=true;
		private bool m_DispatchShortcuts=false;
		private ShortcutsCollection m_AutoDispatchShortcuts;

		// Auto-hide panels
		private AutoHidePanel m_LeftAutoHidePanel=null;
		private AutoHidePanel m_RightAutoHidePanel=null;
		private AutoHidePanel m_TopAutoHidePanel=null;
		private AutoHidePanel m_BottomAutoHidePanel=null;

		private bool m_SuspendLayout=false;
		private string m_DefinitionName="";
		private bool m_DefinitionLoaded=false;
		private bool m_IgnoreF10Key=false;
		//private bool m_ContextMenuSubclass=true;
		private bool m_ThemeAware=false;
		private eDotNetBarStyle m_Style=eDotNetBarStyle.Office2003;

		private bool m_DefinitionCodeSerialize=false;

		private Bar m_FocusedBar=null;
		
		private Control m_ParentUserControl=null;
		private bool m_DockingHints=true;

		internal bool IgnoreLoadedControlDispose=false;

		private bool m_AlwaysDisplayKeyAccelerators=false;
		private bool m_EnableFullSizeDock=true;
		private bool m_LoadingDefinition=false;
        private bool m_LoadingLayout = false;
		private ColorScheme m_ColorScheme=null;
		private bool m_UseGlobalColorScheme=false;
		private bool m_ApplyDocumentBarStyle=true;
		private bool m_HideMdiSystemMenu=false;
		private bool m_IncludeDockDocumentsInDefinition=false;
		//TODO: Menu Merging Implementation MDI Item Merging support
		//private Hashtable m_MdiChildManagers=null;
        //private ArrayList m_CommandLinks = new ArrayList();
        private Size m_MinimumClientSize = new Size(48, 48);
        private ContextMenuBar m_GlobalContextMenuBar = null;
		#endregion

		#if TRIAL
		private int iCount=0,iMaxCount=96;
		#endif

		/// <summary>
		/// Creates new instance of DotNetBarManager.
		/// </summary>
		public DotNetBarManager():this(null) {}
		/// <summary>
		/// Creates new instance of DotNetBarManager.
		/// </summary>
		/// <param name="cont">Container.</param>
		public DotNetBarManager(IContainer cont)
		{
			#if TRIAL
				RemindForm frm=new RemindForm();
				frm.ShowDialog();
				frm.Dispose();
			#endif
            //#if !DEBUG
            //    m_License = LicenseManager.Validate(typeof(DotNetBarManager), this);
            //#endif
			//#endif

			InitializeComponent(cont);
		}

		private void InitializeComponent(IContainer cont)
		{
			m_TopDockSite=null;
			m_BottomDockSite=null;
			m_LeftDockSite=null;
			m_RightDockSite=null;
			//m_MsgHandler=null;
			m_Bars=new Bars(this);
			m_WereVisible=new ArrayList();
			m_Items=new Items(this);
			m_ShortcutTable=new Hashtable();
			m_FilterInstalled=false;
			m_frmCustomize=null;
			m_DisabledControls=null;
			m_FocusItem=null;
			m_ExpandedItem=null;
			m_RegisteredPopups=new ArrayList();
			m_MdiHandler=null;
			m_ActiveMdiChild=null;
			m_IgnoreSysKeyUp=false;
			m_ImageList=null;
			m_ParentForm=null;
			m_ContextMenus=new ContextMenusCollection(this);

			if(cont!=null)
				cont.Add(this);

			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}

			m_AutoDispatchShortcuts=new ShortcutsCollection(null);
			m_ColorScheme=new ColorScheme(m_Style);
		}

	#if !TRIAL
		internal DotNetBarManager(IContainer cont,bool dnbe)
		{
            InitializeComponent(cont);
		}
	#endif

        #region Licensing
#if !TRIAL
        private string m_LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return m_LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                m_LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        #endregion

        #region Docking Shortcut methods
        /// <summary>
        /// Dock bar to the specified side of the form.
        /// </summary>
        /// <param name="barToDock">Bar to dock.</param>
        /// <param name="side">Side to dock bar to.</param>
        public void Dock(Bar barToDock, eDockSide side)
        {
            if (side == eDockSide.None)
            {
                Float(barToDock);
                return;
            }

            if (barToDock.LayoutType != eLayoutType.DockContainer)
            {
                barToDock.DockSide = side;
                return;
            }

            DocumentDockUIManager dockManager = GetDocumentUIManager(side);
            if (!this.Bars.Contains(barToDock))
                this.Bars.Add(barToDock);
            dockManager.Dock(barToDock);
        }
        /// <summary>
        /// Docks the bar to the specified side of the reference bar.
        /// </summary>
        /// <param name="barToDock">Bar to dock.</param>
        /// <param name="referenceBar">Reference bar.</param>
        /// <param name="dockToReferenceBarSide">Side of the reference bar to dock the bar to.</param>
        public void Dock(Bar barToDock, Bar referenceBar, eDockSide dockToReferenceBarSide)
        {
            if (dockToReferenceBarSide == eDockSide.None)
                throw new ArgumentException("eDockSide.None is not supported value for dockToReferenceBarSide parameter. Use Float method to make floating bar.");
            if (referenceBar.DockSide == eDockSide.None)
                throw new ArgumentException("referenceBar must be docked to be used as reference for docking.");

            if (barToDock.LayoutType != eLayoutType.DockContainer)
            {
                barToDock.DockSide = dockToReferenceBarSide;
                return;
            }

            if (dockToReferenceBarSide == eDockSide.Document)
            {
                System.Collections.ArrayList list = new System.Collections.ArrayList(barToDock.Items.Count);
                barToDock.Items.CopyTo(list);
                DockContainerItem firstItem = null;
                foreach (BaseItem item in list)
                {
                    DockContainerItem dockitem = item as DockContainerItem;
                    if (dockitem != null)
                    {
                        if (firstItem == null) firstItem = dockitem;
                        dockitem.Displayed = false;
                        if (dockitem.OriginalBarName == "")
                        {
                            dockitem.OriginalBarName = barToDock.Name;
                            dockitem.OriginalPosition = barToDock.Items.IndexOf(dockitem);
                        }
                        barToDock.Items.Remove(dockitem);
                        referenceBar.Items.Add(dockitem);
                    }
                }
                referenceBar.RecalcLayout();

                if (firstItem != null)
                    referenceBar.SelectedDockContainerItem = firstItem;
                referenceBar.InvokeBarDockEvents();

                if (barToDock.CustomBar)
                {
                    this.Bars.Remove(barToDock);
                    barToDock.Dispose();
                }
                else
                {
                    barToDock.Visible = false;
                }
            }
            else
            {
                DocumentDockUIManager dockManager = GetDocumentUIManager(referenceBar.DockSide);
                if (!this.Bars.Contains(barToDock))
                    this.Bars.Add(barToDock);
                dockManager.Dock(referenceBar, barToDock, dockToReferenceBarSide);
            }
        }
        /// <summary>
        /// Docks specified DockContainerItem.
        /// </summary>
        /// <param name="itemToDock">DockContainerItem to dock.</param>
        /// <param name="side">Side to dock item to.</param>
        public void Dock(DockContainerItem itemToDock, eDockSide side)
        {
            Dock(itemToDock, null, side);
        }
        /// <summary>
        /// Docks specified DockContainerItem.
        /// </summary>
        /// <param name="itemToDock">DockContainerItem to dock.</param>
        /// <param name="referenceBar">Reference bar.</param>
        /// <param name="dockToReferenceBarSide">Side to dock item to.</param>
        public void Dock(DockContainerItem itemToDock, Bar referenceBar, eDockSide dockToReferenceBarSide)
        {
            Dock(itemToDock, referenceBar, dockToReferenceBarSide, Point.Empty);
        }
        /// <summary>
        /// Docks specified DockContainerItem.
        /// </summary>
        /// <param name="itemToDock">DockContainerItem to dock.</param>
        /// <param name="referenceBar">Reference bar.</param>
        /// <param name="dockToReferenceBarSide">Side to dock item to.</param>
        private void Dock(DockContainerItem itemToDock, Bar referenceBar, eDockSide dockToReferenceBarSide, Point initialFloatLocation)
        {
            Bar parentBar = itemToDock.ContainerControl as Bar;

            if (dockToReferenceBarSide == eDockSide.None)
            {
                if (parentBar != null)
                {
                    parentBar.TearOffDockContainerItem(itemToDock, true, initialFloatLocation);
                    return;
                }
            }

            Bar newBar = null;
            if (parentBar != null)
            {
                newBar = BarFunctions.CreateDuplicateDockBar(parentBar);
            }

            // Un-parent DockContainerItem
            if (itemToDock.Parent != null)
            {
                if (parentBar != null)
                {
                    parentBar.Items.Remove(itemToDock);
                    if (parentBar.Items.Count == 0)
                    {
                        if (parentBar.CustomBar)
                        {
                            this.Bars.Remove(parentBar);
                            parentBar.Dispose();
                        }
                        else
                        {
                            parentBar.Visible = false;
                        }
                    }
                    else if (parentBar.VisibleItemCount == 0)
                        parentBar.Visible = false;
                    else
                        parentBar.RecalcLayout();
                }
                else
                    itemToDock.Parent.SubItems.Remove(itemToDock);
            }

            if (newBar == null)
            {
                newBar = new Bar();
                newBar.Name = "customBar" + itemToDock.Name;
                newBar.LayoutType = eLayoutType.DockContainer;
                newBar.Text = itemToDock.Text;
                newBar.AutoSyncBarCaption = true;
                newBar.CustomBar = true;
                this.Bars.Add(newBar);
            }

            newBar.Items.Add(itemToDock);
            if (dockToReferenceBarSide == eDockSide.None)
            {
                newBar.DockSide = eDockSide.None;
                newBar.Location = initialFloatLocation;
                return;
            }

            if (referenceBar == null)
                Dock(newBar, dockToReferenceBarSide);
            else
                Dock(newBar, referenceBar, dockToReferenceBarSide);
        }

        /// <summary>
        /// Tear-off specified DockContainerItem and float it.
        /// </summary>
        /// <param name="itemToFloat">Item to float.</param>
        public void Float(DockContainerItem itemToFloat)
        {
            Bar parentBar = itemToFloat.ContainerControl as Bar;
            if (parentBar != null && parentBar.Items.Count == 1)
            {
                Float(parentBar);
                return;
            }
            Dock(itemToFloat, eDockSide.None);
        }
        /// <summary>
        /// Tear-off specified DockContainerItem and float it.
        /// </summary>
        /// <param name="itemToFloat">Item to float.</param>
        public void Float(DockContainerItem itemToFloat, Point initialFloatLocation)
        {
            Bar parentBar = itemToFloat.ContainerControl as Bar;
            if (parentBar != null && parentBar.Items.Count == 1)
            {
                Float(parentBar, initialFloatLocation);
                return;
            }
            Dock(itemToFloat, null, eDockSide.None, initialFloatLocation);
        }
        /// <summary>
        /// Make specified bar floating bar, i.e. undock it and place in floating window.
        /// </summary>
        /// <param name="barToFloat">Bar to float.</param>
        public void Float(Bar barToFloat)
        {
            barToFloat.DockSide = eDockSide.None;
        }
        /// <summary>
        /// Make specified bar floating bar, i.e. undock it and place in floating window and specified location on the screen.
        /// </summary>
        /// <param name="barToFloat">Bar to float.</param>
        /// <param name="initialFloatingLocation">Screen coordinates for the floating bar.</param>
        public void Float(Bar barToFloat, Point initialFloatingLocation)
        {
            barToFloat.InitalFloatLocation = initialFloatingLocation;
            barToFloat.DockSide = eDockSide.None;
        }

        private DocumentDockUIManager GetDocumentUIManager(eDockSide side)
        {
            if (side == eDockSide.None)
                throw new ArgumentException("eDockSide.None is not supported parameter value");

            if (side == eDockSide.Bottom)
            {
                return this.BottomDockSite.GetDocumentUIManager();
            }
            else if (side == eDockSide.Left)
            {
                return this.LeftDockSite.GetDocumentUIManager();
            }
            else if (side == eDockSide.Right)
            {
                return this.RightDockSite.GetDocumentUIManager();
            }
            else if (side == eDockSide.Top)
            {
                return this.TopDockSite.GetDocumentUIManager();
            }
            return this.FillDockSite.GetDocumentUIManager();
        }
        #endregion

        /// <summary>
        /// Gets or sets the Context menu bar associated with the this control which is used as part of Global Items feature. The context menu 
        /// bar assigned here will be used to search for the items with the same Name or GlobalName property so global properties can be propagated when changed.
        /// You should assign this property to enable the Global Items feature to reach your ContextMenuBar.
        /// </summary>
        [DefaultValue(null), Description("Indicates Context menu bar associated with the DotNetBarManager which is used as part of Global Items feature."), Category("Data")]
        public ContextMenuBar GlobalContextMenuBar
        {
            get { return m_GlobalContextMenuBar; }
            set
            {
                if (m_GlobalContextMenuBar != null)
                    m_GlobalContextMenuBar.GlobalParentComponent = null;
                m_GlobalContextMenuBar = value;
                if (m_GlobalContextMenuBar != null)
                    m_GlobalContextMenuBar.GlobalParentComponent = this;
            }
        }

        ///// <summary>
        ///// Gets the list of all CommandLinks objects associated with this manager.
        ///// </summary>
        //internal ArrayList CommandLinks
        //{
        //    get { return m_CommandLinks; }
        //}
        internal static bool IsManagerRegistered(IOwner manager, Form parentForm)
        {
            if (m_MdiChildParentHandlers.Contains(parentForm))
            {
                ParentMsgHandler handler = m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
                return handler.IsRegistered(manager);
            }
            return false;
        }

		internal static void RegisterParentMsgHandler(IOwner manager,Form parentForm)
		{
			if(m_MdiChildParentHandlers.Contains(parentForm))
			{
				ParentMsgHandler handler=m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
				if(!handler.IsRegistered(manager))
					handler.Register(manager);
			}
			else
			{
                ParentMsgHandler handler=new ParentMsgHandler(manager.DesignMode);
				handler.AssignHandle(parentForm.Handle);
				handler.Register(manager);
				m_MdiChildParentHandlers[parentForm]=handler;
			}
		}
		internal static void UnRegisterParentMsgHandler(IOwner manager,Form parentForm)
		{
			if(parentForm==null)
				return;
			if(m_MdiChildParentHandlers.Contains(parentForm))
			{
				ParentMsgHandler handler=m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
				if(handler.IsRegistered(manager))
				{
					handler.Unregister(manager);
					if(handler.RegisteredCount==0 && handler.RegisteredOwnersCount==0)
					{
						handler.ReleaseHandle();
						m_MdiChildParentHandlers.Remove(parentForm);
					}
					return;
				}
			}
			else
			{
				foreach(DictionaryEntry item in m_MdiChildParentHandlers)
				{
					ParentMsgHandler handler=item.Value as ParentMsgHandler;
					if(handler.IsRegistered(manager))
					{
                        handler.Unregister(manager);
						if(handler.RegisteredCount==0 && handler.RegisteredOwnersCount==0)
						{
							handler.ReleaseHandle();
							m_MdiChildParentHandlers.Remove(parentForm);
						}
						break;
					}
				}
			}
		}
		internal static void RegisterOwnerParentMsgHandler(IOwner owner, Form parentForm)
		{
			if(m_MdiChildParentHandlers.Contains(parentForm))
			{
				ParentMsgHandler handler=m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
				if(!handler.IsOwnerRegistered(owner))
					handler.RegisterOwner(owner);
			}
			else
			{
				ParentMsgHandler handler=new ParentMsgHandler(true);
				handler.AssignHandle(parentForm.Handle);
				handler.RegisterOwner(owner);
				m_MdiChildParentHandlers[parentForm]=handler;
			}
		}
		internal static void UnRegisterOwnerParentMsgHandler(IOwner owner,Form parentForm)
		{
			if(parentForm==null)
				return;

			if(m_MdiChildParentHandlers.Contains(parentForm))
			{
				ParentMsgHandler handler=m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
				if(handler.IsOwnerRegistered(owner))
				{
					handler.UnregisterOwner(owner);
					if(handler.RegisteredCount==0 && handler.RegisteredOwnersCount==0)
					{
						handler.ReleaseHandle();
						m_MdiChildParentHandlers.Remove(parentForm);
					}
					return;
				}
			}
			else
			{
				foreach(DictionaryEntry item in m_MdiChildParentHandlers)
				{
					ParentMsgHandler handler=item.Value as ParentMsgHandler;
					if(handler.IsOwnerRegistered(owner))
					{
						handler.UnregisterOwner(owner);
						if(handler.RegisteredCount==0 && handler.RegisteredOwnersCount==0)
						{
							handler.ReleaseHandle();
							m_MdiChildParentHandlers.Remove(parentForm);
						}
						break;
					}
				}
			}
		}

		internal static void RelayApplicationActivate(bool bActivate)
		{
			ParentMsgHandler[] handlers=new ParentMsgHandler[m_MdiChildParentHandlers.Count];
			m_MdiChildParentHandlers.Values.CopyTo(handlers,0);
			foreach(ParentMsgHandler handler in handlers)
			{
                if (bActivate)
                    handler.ApplicationActivate();
                else
                    handler.ApplicationDeactivate();
			}
		}

		/// <summary>
		/// Gets or sets the form DotNetBarManager is attached to.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),System.ComponentModel.Description("Gets or sets the form DotNetBarManager is attached to.")]
		public Form ParentForm
		{
			get
			{
				return m_ParentForm;
			}
			set
			{
				if(m_ParentForm!=null)
				{
					ReleaseParentFormHooks();
				}
				m_ParentForm=value;
				OnParentChanged();
			}
		}

		/// <summary>
		/// Gets or sets the user control DotNetBarManager is parented to when on user control and providing popups only.
		/// </summary>
		[Browsable(false),DefaultValue(null)]
		public Control ParentUserControl
		{
			get {return m_ParentUserControl;}
			set
			{
				m_ParentUserControl=value;
				SetupParentUserControl();
			}
		}

        /// <summary>
        /// Gets currently focused (active) DockContainerItem. Note that only if DockContainer Item has input focus it will be consider active
        /// so there can only be one active DockContainerItem at a time.
        /// </summary>
        [Browsable(false)]
        public DockContainerItem ActiveDockContainerItem
        {
            get
            {
                return _ActiveDockContainerItem;
            }
        }

        private DockContainerItem _ActiveDockContainerItem = null;
        internal void InternalDockContainerDeactivated(DockContainerItem item)
        {
            if (_ActiveDockContainerItem != null)
            {
                OnActiveDockContainerChanged(new ActiveDockContainerChangedEventArgs(item, false));
                _ActiveDockContainerItem = null;
            }
        }
        internal void InternalDockContainerActivated(DockContainerItem item)
        {
            if (_ActiveDockContainerItem != item)
            {
                if (_ActiveDockContainerItem != null)
                {
                    InternalDockContainerDeactivated(_ActiveDockContainerItem);
                }
                _ActiveDockContainerItem = item;
                OnActiveDockContainerChanged(new ActiveDockContainerChangedEventArgs(item, true));
            }
        }

        /// <summary>
        /// Raises the ActiveDockContainerChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnActiveDockContainerChanged(ActiveDockContainerChangedEventArgs e)
        {
            ActiveDockContainerChangedEventHandler h = ActiveDockContainerChanged;
            if (h != null) h(this, e);
        }
        
        bool IExtenderProvider.CanExtend(object target) 
		{
			if (target is Control)
				return true;
			return false;
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

		internal void InvokeDockTabClosing(Bar bar, DockTabClosingEventArgs e)
		{
			if(DockTabClosing!=null)
				DockTabClosing(this,e);
		}
        internal void InvokeDockTabClosed(Bar bar, DockTabClosingEventArgs e)
        {
            if (DockTabClosed != null)
                DockTabClosed(this, e);
        }

        internal void InvokeTextBoxItemTextChanged(TextBoxItem ti)
        {
            if (TextBoxItemTextChanged != null)
                TextBoxItemTextChanged(ti, new EventArgs());
        }

        internal void InvokeColorPickerSelectedColorChanged(ColorPickerDropDown d)
        {
            if (ColorPickerSelectedColorChanged != null)
                ColorPickerSelectedColorChanged(d, new EventArgs());
        }

		/// <summary>
		/// Returns the Bar object that contains the item.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public DevComponents.DotNetBar.Bar GetItemBar(BaseItem item)
		{
			while(item.Parent!=null)
				item=item.Parent;
			Bar bar=item.ContainerControl as Bar;
			return bar;
		}

        internal bool GetDesignMode()
        {
            return this.DesignMode;
        }

		private void OnParentChanged()
		{
			// We always create docking sites on top level controls
			if(m_ParentForm==null)
				return;

            if(!this.DesignMode)
			    UnRegisterParentMsgHandler(this,m_ParentForm);

			if(m_ParentForm.IsHandleCreated && !this.DesignMode)
			{
				if(m_ParentForm.IsMdiChild && m_ParentForm.MdiParent!=null)
				{
					RegisterParentMsgHandler(this,m_ParentForm.MdiParent);
				}
				else
				{
                    RegisterParentMsgHandler(this,m_ParentForm);					
				}
			}

			m_ParentForm.HandleDestroyed+=new System.EventHandler(this.ParentHandleDestroyed);
			m_ParentForm.HandleCreated+=new System.EventHandler(this.ParentHandleCreated);
			if(!this.DesignMode)
			{
				m_ParentForm.Activated+=new System.EventHandler(this.ParentActivated);
				m_ParentForm.Deactivate+=new System.EventHandler(this.ParentDeactivate);
				m_ParentForm.Load+=new System.EventHandler(this.ParentLoad);
				m_ParentForm.VisibleChanged+=new System.EventHandler(this.ParentVisibleChanged);
				m_ParentForm.Resize+=new System.EventHandler(this.ParentResize);
			}

			if(!this.DesignMode)
			{
				if(!m_FilterInstalled && !m_UseHook)
				{
					MessageHandler.RegisterMessageClient(this);
					m_FilterInstalled=true;
				}
				else if(m_UseHook && m_Hook==null)
				{
					m_Hook=new Hook(this);
				}
			}

			// MDI Parent Handling
			if(m_MdiHandler!=null)
			{
                m_MdiHandler.MdiSetMenu -= this.OnMdiSetMenu;
				m_MdiHandler.ReleaseHandle();
				m_MdiHandler=null;
			}

			// MDI Handling...
			System.Windows.Forms.MdiClient client=((IOwner)this).GetMdiClient(m_ParentForm);
			if(client!=null)
			{
				SetupMdiHandler(client);
			}
			else
			{
				// Watch for its creation to catch it early
				m_ParentForm.ControlAdded+=new ControlEventHandler(ParentControlAdded);
				m_EventControlAdded=true;
			}

			// Try to load bar stream if possible
			//if(!this.DesignMode)
				BarStreamLoad();
		}

		private void ParentHandleDestroyed(object sender, EventArgs e)
		{
            if(!this.DesignMode)
			    UnRegisterParentMsgHandler(this,m_ParentForm);
//			if(m_MsgHandler!=null)
//			{
//				m_MsgHandler.ReleaseHandle();
//				m_MsgHandler=null;
//			}
		}
		private void ParentHandleCreated(object sender, EventArgs e)
		{
            if (!this.DesignMode)
            {
                if (m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null)
                    RegisterParentMsgHandler(this, m_ParentForm.MdiParent);
                else
                    RegisterParentMsgHandler(this, m_ParentForm);

                if (m_MdiHandler != null)
                {
                    m_MdiHandler.MdiSetMenu -= this.OnMdiSetMenu;
                    m_MdiHandler.ReleaseHandle();
                    m_MdiHandler = null;
                    System.Windows.Forms.MdiClient client = ((IOwner)this).GetMdiClient(m_ParentForm);
                    if (client != null)
                    {
                        SetupMdiHandler(client);
                    }
                }
            }

			//if(!this.DesignMode)
				BarStreamLoad(true);
		}

		private void SetupMdiHandler(System.Windows.Forms.MdiClient client)
		{
			// Do not force creation of window handle wait for the system to create it...
			if(client.IsHandleCreated)
				SetupClientMsgHandler(client);
			else if(!m_MdiClientHandleCreatedHandled)
			{
				client.HandleCreated+=new EventHandler(this.MdiClientHandleCreates);
				m_MdiClientHandleCreatedHandled=true;
			}
			
			if(!m_MdiChildActivateHandled)
			{
				m_ParentForm.MdiChildActivate+=new EventHandler(this.OnMdiChildActivate);
				m_MdiChildActivateHandled=true;
				if(m_ParentForm.ActiveMdiChild!=null)
					this.OnMdiChildActivate(m_ParentForm,new EventArgs());
			}
		}

		private void SetupClientMsgHandler(System.Windows.Forms.MdiClient client)
		{
			if(client==null)
				return;
			if(m_MdiHandler!=null)
			{
                m_MdiHandler.MdiSetMenu -= this.OnMdiSetMenu;
				m_MdiHandler.ReleaseHandle();
				m_MdiHandler=null;
			}
			m_MdiHandler=new MDIClientMsgHandler();
            m_MdiHandler.MdiSetMenu += new EventHandler(this.OnMdiSetMenu);
			m_MdiHandler.AssignHandle(client.Handle);
			if(m_MdiClientHandleCreatedHandled)
			{
				client.HandleCreated-=new EventHandler(this.MdiClientHandleCreates);
				m_MdiClientHandleCreatedHandled=false;
			}
		}

		private void MdiClientHandleCreates(object sender, EventArgs e)
		{
			SetupClientMsgHandler(sender as System.Windows.Forms.MdiClient);
		}

		private void ParentResize(object sender, System.EventArgs e)
		{
			if(m_ParentForm!=null && m_ParentForm.WindowState!=FormWindowState.Minimized && m_ParentActivated && m_WereVisible.Count>0)
				((IOwner)this).OnApplicationActivate();
			else if(m_ParentForm!=null && m_ParentForm.WindowState==FormWindowState.Minimized && m_WereVisible.Count==0)
				((IOwner)this).OnApplicationDeactivate();
		}
		private void ParentActivated(object sender, System.EventArgs e)
		{
			m_ParentActivated=true;
			if(m_WereVisible.Count>0 && m_ParentForm!=null && m_ParentForm.WindowState!=FormWindowState.Minimized)
				((IOwner)this).OnApplicationActivate();
		}
		private void ParentDeactivate(object sender, System.EventArgs e)
		{
			m_ParentActivated=false;
			if(m_ParentForm!=null && m_ParentForm.WindowState==FormWindowState.Minimized)
				((IOwner)this).OnApplicationDeactivate();
		}
		private void ParentVisibleChanged(object sender, System.EventArgs e)
		{
			if(m_ParentForm==null)
				return;
			if(m_ParentForm.Visible)
			{
				if(!m_FilterInstalled && !m_UseHook)
				{
					m_FilterInstalled=true;
					//System.Windows.Forms.Application.AddMessageFilter(this);
					MessageHandler.RegisterMessageClient(this);
				}
				else if(m_UseHook && m_Hook==null)
				{
					m_Hook=new Hook(this);
				}
			}
			else
			{
				if(m_FilterInstalled)
				{
					//System.Windows.Forms.Application.RemoveMessageFilter(this);
					MessageHandler.UnregisterMessageClient(this);
					m_FilterInstalled=false;
				}
				else if(m_Hook!=null)
				{
					m_Hook.Dispose();
					m_Hook=null;
				}
			}
		}
		private void ParentLoad(object sender, System.EventArgs e)
		{
//			if(m_TriggerParentChange)
//			{
//				m_ParentForm.Load-=new System.EventHandler(this.ParentLoad);
//				OnParentChanged();
//			}
			// Last chance to check for MDI Stuff...
			if(m_MdiHandler==null)
			{
				System.Windows.Forms.MdiClient client=((IOwner)this).GetMdiClient(m_ParentForm);
				if(client!=null)
				{
					SetupMdiHandler(client);
				}
			}

            if (m_ParentForm.IsHandleCreated && !this.DesignMode)
            {
                if (m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null)
                {
                    if (!IsManagerRegistered(this, m_ParentForm.MdiParent))
                    {
                        UnRegisterParentMsgHandler(this, m_ParentForm);
                        RegisterParentMsgHandler(this, m_ParentForm.MdiParent);
                    }
                }
                else
                    RegisterParentMsgHandler(this, m_ParentForm);
            }
            
            BarStreamLoad(true);

// TODO: Menu Merging Implementation
//			// Check do we have any Bars participating in item merging
//			if(m_ParentForm.IsMdiChild)
//			{
//                // Try to get to the MDI form and register with that DotNetBarManager
//				if(m_ParentForm.MdiParent!=null)
//				{
//					DotNetBarManager mdiManager=null;
//					foreach(Control ctrl in m_ParentForm.MdiParent.Controls)
//					{
//						if(ctrl is DockSite)
//						{
//							mdiManager=((DockSite)ctrl).Owner;
//							break;
//						}
//					}
//					if(mdiManager!=null)
//						mdiManager.RegisterMdiChildManager(this);
//					m_ParentForm.Closed+=new EventHandler(ParentFormClosed);
//				}
//			}
		}
//		private void ParentFormClosed(object sender, EventArgs e)
//		{
//			if(m_ParentForm.MdiParent!=null)
//			{
//				DotNetBarManager mdiManager=null;
//				foreach(Control ctrl in m_ParentForm.MdiParent.Controls)
//				{
//					if(ctrl is DockSite)
//					{
//						mdiManager=((DockSite)ctrl).Owner;
//						break;
//					}
//				}
//				if(mdiManager!=null)
//					mdiManager.UnregisterMdiChildManager(this);
//			}
//		}

		private void ParentControlAdded(object sender, ControlEventArgs e)
		{
			if(e.Control is System.Windows.Forms.MdiClient)
			{
				SetupMdiHandler((System.Windows.Forms.MdiClient)e.Control);
				//m_ParentForm.ControlAdded-=new ControlEventHandler(this.ParentControlAdded); 11/17/2003 Fix Matt Act, solve on-the-fly MDI form creation
				//m_EventControlAdded=false;
			}
		}

// TODO: Menu Merging Implementation
//		internal void RegisterMdiChildManager(DotNetBarManager mdiChildManager)
//		{
//			if(m_MdiChildManagers==null)
//			{
//				m_MdiChildManagers=new Hashtable();
//				m_ParentForm.MdiChildActivate+=new EventHandler(ParentMdiChildActivate);
//			}
//			m_MdiChildManagers.Add(mdiChildManager.ParentForm,mdiChildManager);
//		}
//
//		internal void UnregisterMdiChildManager(DotNetBarManager mdiChildManager)
//		{
//			if(m_MdiChildManagers==null)
//				return;
//			if(m_MdiChildManagers.Contains(mdiChildManager.ParentForm))
//				m_MdiChildManagers.Remove(mdiChildManager);
//		}

//		private void ParentMdiChildActivate(object sender, EventArgs e)
//		{
//			if(m_MdiChildManagers.Contains(sender))
//			{
//			}
//		}

		private void BarStreamLoad()
		{
			BarStreamLoad(false);
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void BarStreamLoad(bool bForceLoad)
		{
			if(m_DefinitionLoaded)
				return;

			if(m_DefinitionName!="" && (m_TopDockSite!=null && m_TopDockSite.Parent!=null && m_BottomDockSite!=null && m_BottomDockSite.Parent!=null && m_LeftDockSite!=null && m_LeftDockSite.Parent!=null && m_RightDockSite!=null && m_RightDockSite.Parent!=null || bForceLoad || IsPopupProviderOnly && this.ParentForm!=null && m_ParentUserControl!=null))
			{
				if(this.DesignMode)
				{
					// In design mode we are loading our definition file directly
					string path=DesignTimeDte.GetDefinitionPath(m_DefinitionName, this.Site); // GetProjectPath();
					//MessageBox.Show(path);
					if(path!=null && path!="")
					{
						if(!path.EndsWith("\\"))
                            path+="\\";
						if(System.IO.File.Exists(path+m_DefinitionName))
						{
							this.LoadDefinition(path+m_DefinitionName);
							// Removed becouse problem was found that form was loaded before the 
							// definition file was loaded inside the project
//							if(!DesignTimeDte.ExistInProject(m_DefinitionName))
//							{
//								if(MessageBox.Show("Definition file is not included in existing projects. "+m_DefinitionName+" but it is found at: "+path+"\n\nDo you want DotNetBar to add it to the project?","DotNetBar Definition Load",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
//									DesignTimeDte.AddFileToProject(path+m_DefinitionName);
//							}
						}
						else
						{
							MessageBox.Show("DotNetBar definition file not found: "+path+m_DefinitionName,"DotNetBar Designer",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            m_DefinitionName="";							
						}
					}
				}
				else
				{					
					if((this.ParentForm!=null && (this.ParentForm.IsHandleCreated || bForceLoad || this.IsPopupProviderOnly)) || (m_TopDockSite!=null && m_TopDockSite.Parent is UserControl))
					{
						System.IO.Stream stream=null;
						Hashtable assemblies=new Hashtable();
						if(m_TopDockSite!=null && m_TopDockSite.Parent is UserControl)
						{
							assemblies.Add(m_TopDockSite.Parent.GetType().Assembly.FullName,m_TopDockSite.Parent.GetType().Assembly);
							stream=m_TopDockSite.Parent.GetType().Assembly.GetManifestResourceStream(m_TopDockSite.Parent.GetType().Namespace+"."+m_DefinitionName); // stream=m_TopDockSite.Parent.GetType().Assembly.GetManifestResourceStream(m_TopDockSite.Parent.GetType().Assembly.GetName().Name+"."+m_DefinitionName);
						}
						else
						{
							if(this.ParentForm.GetType().BaseType.FullName!="System.Windows.Forms.Form")
							{
								Type type=this.ParentForm.GetType();
								Stack types=new Stack(10);
								int iCount=0;
								while(iCount<64)
								{
									types.Push(type);
									if(!assemblies.ContainsKey(type.Assembly.FullName))
										assemblies.Add(type.Assembly.FullName,type.Assembly);
									if(type.BaseType.FullName=="System.Windows.Forms.Form")
										break;
									type=type.BaseType;
									iCount++;
								}
								stream=type.Assembly.GetManifestResourceStream(type.Namespace+"."+m_DefinitionName);
								if(stream==null)
								{
									// Maybe manager was added on this form...
									while(types.Count>0 && stream==null)
									{
										type=types.Pop() as Type;
										stream=type.Assembly.GetManifestResourceStream(type.Namespace+"."+m_DefinitionName);
									}
								}
								types.Clear();
							}
							else
							{
								stream=this.ParentForm.GetType().Assembly.GetManifestResourceStream(this.ParentForm.GetType().Namespace+"."+m_DefinitionName);
								assemblies.Add(this.ParentForm.GetType().Assembly.FullName,this.ParentForm.GetType().Assembly);
							}
                            if (m_ParentUserControl != null && !assemblies.Contains(m_ParentUserControl.GetType().Assembly.FullName))
                            {
                                assemblies.Add(m_ParentUserControl.GetType().Assembly.FullName, m_ParentUserControl.GetType().Assembly);
                            }
						}
						if(stream==null)
						{
							// It is possible that Form is not in the same default namespace as the definition file so we have to enumerate through the manifest and look for our definition
							System.Reflection.Assembly[] arr=new System.Reflection.Assembly[assemblies.Count];
							assemblies.Values.CopyTo(arr,0);
							for(int i=arr.Length-1;i>=0;i--)
							{
								System.Reflection.Assembly a=arr[i];
								string[] res=a.GetManifestResourceNames();
								foreach(string s in res)
								{
									if(s.EndsWith(m_DefinitionName))
									{
										stream=a.GetManifestResourceStream(s);
										break;
									}
								}
								if(stream!=null)
									break;
							}
						}
						assemblies.Clear();
						if(stream!=null)
						{
							System.Xml.XmlDocument document=new System.Xml.XmlDocument();
							document.Load(stream);
							LoadDefinition(document);
							m_DefinitionLoaded=true;
						}
					}
				}
                m_BarStreamer=null;
			}
			else if(m_BarStreamer!=null && (m_TopDockSite!=null && m_TopDockSite.Parent!=null && m_BottomDockSite!=null && m_BottomDockSite.Parent!=null && m_LeftDockSite!=null && m_LeftDockSite.Parent!=null && m_RightDockSite!=null && m_RightDockSite.Parent!=null || bForceLoad))
			{
				if(m_BarStreamer.Data!=null)
				{
					LoadDefinition(m_BarStreamer.Data);
					m_DefinitionLoaded=true;
				}
				m_BarStreamer=null;
			}
			
		}

        private bool IsPopupProviderOnly
        {
            get { return m_TopDockSite == null && m_BottomDockSite == null && m_LeftDockSite == null && m_RightDockSite == null; }
        }

		void IOwner.OnApplicationDeactivate()
		{
            if (m_ParentForm != null && m_ParentForm.InvokeRequired)
            {
                m_ParentForm.BeginInvoke(new InvokeActivateDelegate(OnApplicationDeactivateInternal));
                return;
            }
            OnApplicationDeactivateInternal();
		}

        private void OnApplicationDeactivateInternal()
        {
            CloseDockingHints();
            if (m_WereVisible.Count == 0)
            {
                if (m_Bars != null)
                {
                    // Break out if we have docking in progress since that will trigger these events...
                    foreach (Bar bar in m_Bars)
                    {
                        if (bar.DockingInProgress)
                            return;
                    }

                    foreach (Bar bar in m_Bars)
                    {
                        bar.AppLostFocus();

                        // TODO: bar.Visible is returning false when bar is floating and visible!!! Possible bug!!!
                        if (bar.BarState == eBarState.Floating && bar.Visible && bar.HideFloatingInactive)
                        {
                            //bar.Visible=false;
                            bar.HideBar();
                            m_WereVisible.Add(bar);
                        }
                    }
                }
                ArrayList popupList = new ArrayList(m_RegisteredPopups);
                foreach (PopupItem objPopup in popupList)
                    objPopup.ClosePopup();
            }

            Bar menuBar = this.GetMenuBar();
            if (menuBar != null && menuBar.MenuFocus)
                menuBar.MenuFocus = false;
        }
        //		internal void App_LostFocusMessage()
//		{
//			this.App_LostFocus();
//
//			if(ms_Managers.Count>1)
//			{
//				foreach(DotNetBarManager m in ms_Managers)
//				{
//					if(m!=this)
//						m.App_LostFocus();
//				}
//			}
//		}

		//protected void Parent_Activate(object sender, System.EventArgs e)
		void IOwner.OnApplicationActivate()
		{
			if(m_ParentForm==null || m_ParentForm.WindowState==FormWindowState.Minimized)
				return;
            if (m_ParentForm != null && m_ParentForm.InvokeRequired)
            {
                m_ParentForm.BeginInvoke(new InvokeActivateDelegate(OnApplicationActivateInternal));
                return;
            }
            OnApplicationActivateInternal();
		}

        private void OnApplicationActivateInternal()
        {
            if (m_ParentForm == null || m_ParentForm.WindowState == FormWindowState.Minimized)
                return;
            Form active = Form.ActiveForm;
            if (active == null || active.Modal && active != m_ParentForm)
                return;
            try
            {
                foreach (Bar bar in m_WereVisible)
                {
                    bar.ShowBar();
                }
            }
            finally
            {
                m_WereVisible.Clear();
            }
        }

//		internal void App_ActivateMessage()
//		{
//			this.App_Activate();
//			if(ms_Managers.Count>1)
//			{
//				foreach(DotNetBarManager m in ms_Managers)
//				{
//					if(m!=this)
//						m.App_Activate();
//				}
//			}
//		}

		ArrayList IOwnerBarSupport.WereVisible
		{
			get
			{
				return m_WereVisible;
			}
		}

		/// <summary>
		///     Gets whether component has been disposed.
		/// </summary>
		/// <value>
		/// </value>
		/// <remarks>
		///     Disposed will return true after Dispose method has been executed.
		/// </remarks>
		[Browsable(false)]
		public bool IsDisposed
		{
			get{return m_Disposed;}
		}
		/// <summary>
		///    Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if(m_Disposed)
			{
				base.Dispose(disposing);
				return;
			}

			m_Disposed=true;

			if(disposing && m_License != null) 
			{
				m_License.Dispose();
				m_License = null;
			}

			if(m_ActiveMdiChild!=null)
			{
				m_ActiveMdiChild.Resize-=new EventHandler(this.OnMdiChildResize);
				m_ActiveMdiChild.VisibleChanged-=new EventHandler(this.OnMdiChildVisibleChanged);
				m_ActiveMdiChild=null;
			}

			this.Images=null;
			this.ImagesMedium=null;
			this.ImagesLarge=null;
			
			//ms_Managers.Remove(this);

			if(m_ClickTimer!=null)
			{
				m_ClickTimer.Stop();
				m_ClickTimer.Dispose();
				m_ClickTimer=null;
			}
			if(m_RegisteredPopups!=null && m_RegisteredPopups.Count>0)
			{
				BaseItem[] popups;
				lock(this)
				{
					popups=(BaseItem[])m_RegisteredPopups.ToArray(typeof(BaseItem));
				}
				foreach(PopupItem popup in popups)
					if(popup.Expanded) popup.ClosePopup();
				//m_RegisteredPopups=null;
			}

			// This was commented out becouse of the issue described below. This might have some of the same
			// problems as the solution below and after testing it did not show any ill effects.
//			if(m_MdiHandler!=null && m_MdiParentMsgHandler)
//			{
//				m_MdiHandler.ReleaseHandle();
//				m_MdiHandler=null;
//			}

			// Closing the form which contains dockable window which in turn contains the UserControl with DotNetBar and 
			// when DotNetBar is used to provide context menu for that UserControl
			// was rasing an NULL exception. fixed after 1.0.0.10
//			if(m_MsgHandler!=null)
//			{
//				m_MsgHandler.ReleaseHandle();
//				m_MsgHandler=null;
//			}
			ReleaseParentFormHooks();

			// Very bad things were happening when we put in the try..catch block, so I just commented it out. It seems it is working ok
			if(m_FilterInstalled)
			{
				// This was causing the Local Data Storage has been freed exception under some conditions. Client: Eric.Plummer@frontrange.com, 02/25/2002
				//try
				//{
					//System.Windows.Forms.Application.RemoveMessageFilter(this);
					MessageHandler.UnregisterMessageClient(this);
				//}
				//catch(Exception)
				//{}
				m_FilterInstalled=false;
			}

			if(m_Hook!=null)
			{
				m_Hook.Dispose();
				m_Hook=null;
			}

			/*if(m_TopDockSite!=null)
			{
				if(m_TopDockSite.Parent!=null)
					m_TopDockSite.Parent.Controls.Remove(m_TopDockSite);
				m_TopDockSite.Dispose();
			}

			if(m_BottomDockSite!=null)
			{
				if(m_BottomDockSite.Parent!=null)
					m_BottomDockSite.Parent.Controls.Remove(m_BottomDockSite);
				m_BottomDockSite.Dispose();
			}

			if(m_LeftDockSite!=null)
			{
				if(m_LeftDockSite.Parent!=null)
					m_LeftDockSite.Parent.Controls.Remove(m_LeftDockSite);
				m_LeftDockSite.Dispose();
			}

			if(m_RightDockSite!=null)
			{
				if(m_RightDockSite.Parent!=null)
					m_RightDockSite.Parent.Controls.Remove(m_RightDockSite);
				m_RightDockSite.Dispose();
			}*/

			if(m_frmCustomize!=null)
			{
				m_frmCustomize.Close();
				if(m_frmCustomize!=null)
					m_frmCustomize.Dispose();
			}

			//foreach(DevComponents.DotNetBar.Bar  bar in m_Bars)
			//	bar.Dispose();
			if(m_Bars!=null)
			{
				//m_Bars.Clear();
				m_Bars=null;
			}
			if(m_Items!=null)
			{
				m_Items.Dispose();
				m_Items=null;
			}
			if(m_ContextMenus!=null)
			{
				m_ContextMenus.SetOwner(null);
				m_ContextMenus=null;
			}

			if(m_DisposeGCCollect) GC.Collect();

			base.Dispose(disposing);
		}

		private bool m_DisposeGCCollect=true;
		/// <summary>
		/// Gets or sets whether GC.Collect() is called when this component is disposed.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool DisposeGCCollect
		{
			get {return m_DisposeGCCollect;}
			set {m_DisposeGCCollect=value;}
		}

		private void ReleaseParentFormHooks()
		{
			if(m_ParentForm!=null)
			{
                if(!this.DesignMode)
				    UnRegisterParentMsgHandler(this,m_ParentForm);				
			
				m_ParentForm.HandleDestroyed-=new System.EventHandler(this.ParentHandleDestroyed);
				m_ParentForm.HandleCreated-=new System.EventHandler(this.ParentHandleCreated);

				if(m_EventControlAdded)
				{
					m_ParentForm.ControlAdded-=new ControlEventHandler(this.ParentControlAdded);
					m_EventControlAdded=false;
				}

				if(!this.DesignMode)
				{
					m_ParentForm.Activated-=new System.EventHandler(this.ParentActivated);
					m_ParentForm.Deactivate-=new System.EventHandler(this.ParentDeactivate);
					m_ParentForm.Load-=new System.EventHandler(this.ParentLoad);
					m_ParentForm.VisibleChanged-=new System.EventHandler(this.ParentVisibleChanged);
					m_ParentForm.Resize-=new System.EventHandler(this.ParentResize);
				}

				if(m_MdiChildActivateHandled)
				{
					m_ParentForm.MdiChildActivate-=new EventHandler(this.OnMdiChildActivate);
					m_MdiChildActivateHandled=false;
				}
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

        /// <summary>
        /// Gets or sets the minimum client size that docking windows will try to maintain for the client area (not occupied by dock windows).
        /// Note that this value is suggested value and cannot be observed when form is resized below the minimum size required for the given layout.
        /// Default value is 48x48 pixels.
        /// </summary>
        [Browsable(true), Category("Docking"), Description("Indicates minimum client size that docking windows will try to maintain for the client area (not occupied by dock windows).")]
        public Size MinimumClientSize
        {
            get { return m_MinimumClientSize; }
            set
            {
                m_MinimumClientSize = value;
            }
        }
        /// <summary>
        /// Returns whether property should be serialized by Windows Forms designer.
        /// </summary>
        public bool ShouldSerializeMinimumClientSize()
        {
            return (m_MinimumClientSize.Width != 48 || m_MinimumClientSize.Height != 48);
        }

		/// <summary>
		/// Indicates whether DotNetBar provides docking hints for easy docking of bars.
		/// </summary>
		[Obsolete(), Browsable(false),DefaultValue(true), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),  Category("Docking"),Description("Indicates whether DotNetBar provides docking hints for easy docking of bars.")]
		public bool DockingHintsEnabled
		{
			get {return m_DockingHints;}
			set
			{m_DockingHints=value;}
		}

		/// <summary>
		/// Gets or sets whether user can control how first bar is docked when using docking hints (default value is True). When enabled (default value) placing the mouse over the middle
		/// docking hint will dock the bar at partial size and using the far docking hint will dock bar at full size. Full size indicates that dock site which hosts the bar
		/// consumes all the space of the parent form while partial size indicates that dock site consumes the full size minus the space of the other dock sites. Default value is true.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Docking"),Description("Indicates whether user can control how first bar is docked when using docking hints.")]
		public bool EnableFullSizeDock
		{
			get {return m_EnableFullSizeDock;}
			set {m_EnableFullSizeDock=value;}
		}

		/// <summary>
		/// Gets or sets whether uniform styling is applied to bars docked as documents. Default value is true which means
		/// that bar that will be docked as document will have it's style changed so it fits in default document styling.
		/// Such bars will have GrabHandleStyle=None, DockTabAlignment=Top and AlwaysDisplayDockTab=true.
		/// Set this property to false to have bars keep these properties once they are docked as documents.
		/// Value of these properties will be returned back to the default values once bar is not docked as document.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Docking"),Description("")]
		public bool ApplyDocumentBarStyle
		{
			get {return m_ApplyDocumentBarStyle;}
			set {m_ApplyDocumentBarStyle=value;}
		}

		/// <summary>
		/// Use to remove bar from DotNetBar control. Bar will be undocked if it is docked and
		/// removed from all internal collections.
		/// </summary>
		/// <param name="bar">Bar to remove.</param>
		public void RemoveBar(Bar bar)
		{
			if(!m_Bars.Contains(bar))
				throw new System.ArgumentException("Bar not part of Bars collections.");
			m_Bars.Remove(bar);

            // It is docked somewhere else, we need to undockit and dockit on another site
            // If Bar is deserialized there is no parent
            if (bar.Parent != null && bar.Parent is DockSite)
            {
                if (((DockSite)bar.Parent).IsDocumentDock || ((DockSite)bar.Parent).DocumentDockContainer != null)
                    ((DockSite)bar.Parent).GetDocumentUIManager().UnDock(bar);
                else
                    ((DockSite)bar.Parent).RemoveBar(bar);
            }
			((IOwnerBarSupport)this).RemoveShortcutsFromBar(bar);
		}

		void IOwnerBarSupport.AddShortcutsFromBar(Bar bar)
		{
			IOwner owner=this as IOwner;
			foreach(BaseItem objItem in bar.Items)
				owner.AddShortcutsFromItem(objItem);
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
						if(!objEntry.Items.ContainsKey(objItem.Id))
							objEntry.Items.Add(objItem.Id,objItem);
					}
					catch(System.ArgumentException)	{}
				}
			}
			IOwner owner=this as IOwner;
			foreach(BaseItem objTmp in objItem.SubItems)
				owner.AddShortcutsFromItem(objTmp);
		}

		void IOwnerBarSupport.RemoveShortcutsFromBar(Bar bar)
		{
			IOwner owner=this as IOwner;
			if(bar.Items==null)
				return;
			foreach(BaseItem objItem in bar.Items)
				owner.RemoveShortcutsFromItem(objItem);
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

		//[Editor(typeof(DotNetBarEditor), typeof(System.Drawing.Design.UITypeEditor)),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		/// <summary>
		/// Gets the collection of the Bar objects associated with DotNetBarManager.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)/*,DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)*/]
		public DevComponents.DotNetBar.Bars Bars
		{
			get
			{
				return m_Bars;
			}
		}

		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBars()
		{
			if(m_DefinitionCodeSerialize)
				return true;
			return false;
		}

		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DefinitionName
		{
			get {return m_DefinitionName;}
			set
			{
				m_DefinitionName=value;
				m_DefinitionLoaded=false;
			}
		}

		/// <summary>
		/// Forces the loading of the definition specified in DefinitionName property.
		/// By default definition is loaded after parent form handle has been created and form is loaded.
		/// However, under certain circumstances you might need DotNetBar to load definition right away so
		/// you can access bars and items. We recommend moving the code to Form Load event and leaving the loading process for DotNetBar definition as is.
		/// </summary>
		public void ForceDefinitionLoad()
		{
			if(!m_DefinitionLoaded)
				BarStreamLoad(true);
		}

		/// <summary>
		/// Gets whether definition is loaded.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public bool IsDefinitionLoaded
		{
			get
			{
				return m_DefinitionLoaded;
			}
		}

		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public DotNetBarStreamer BarStream
		{
			get
			{
				if(this.DefinitionName!="")
					return null;
				if(m_BarStreamer!=null)
					return m_BarStreamer;
				return new DotNetBarStreamer(this);
			}
			set
			{
				m_BarStreamer=null;
				if(value!=null)
				{
					if(value.Data!=null)
					{
						//if(this.ParentForm!=null)
						//	this.LoadDefinition(value.Data);
						//else
							m_BarStreamer=value;
						m_DefinitionLoaded=false;
						BarStreamLoad();
					}
				}
			}
		}

		protected bool ShouldSerializeBarStream()
		{
			//if(m_Bars.Count==0 && m_Items.Count==0 && m_Po)
			//	return false;
			//if(m_Bars.Count==0 && m_Items.Count==0 && m_ContextMenus.Count==0 || this.DefinitionName!="" || )
			return false;
			//return true;
		}

		internal bool IsDesignTime()
		{
			return this.DesignMode;
		}

		/// <summary>
		/// Gets the collection of all items that are used for end-user design-time customization.
		/// </summary>
		[Browsable(false)]
		public Items Items
		{
			get
			{
				return m_Items;
			}
		}

		/// <summary>
		/// Gets the collection of all popup menus managed by DotNetBarManager.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Obsolete("ContextMenus on DotNetBarManager is removed. Use ContextMenuBar control instead.")]
		public ContextMenusCollection ContextMenus
		{
			get
			{
				return m_ContextMenus;
			}
		}

		DockSiteInfo IOwnerBarSupport.GetDockInfo(IDockInfo pDock, int x, int y)
		{
			DockSiteInfo objRet=new DockSiteInfo();

			// Prevent Docking if Ctrl key is pressed
			if((Control.ModifierKeys & Keys.Control)!=0)
				return objRet;

			if(((Bar)pDock).LayoutType==eLayoutType.DockContainer)
			{
                if (pDock.CanDockBottom || pDock.CanDockLeft || pDock.CanDockRight || pDock.CanDockTop || pDock.CanDockDocument)
                {
                    if (DockingHintHandler(ref objRet, pDock, x, y))
                        return objRet;
                }
                return objRet;
			}

			if(pDock.CanDockTop && m_ToolbarTopDockSite!=null)
			{
                objRet = m_ToolbarTopDockSite.GetDockSiteInfo(pDock, x, y);
				if(objRet.objDockSite!=null)
					return objRet;
			}

			if(pDock.CanDockBottom && m_ToolbarBottomDockSite!=null)
			{
                objRet = m_ToolbarBottomDockSite.GetDockSiteInfo(pDock, x, y);
				if(objRet.objDockSite!=null)
					return objRet;
			}

			if(pDock.CanDockLeft && m_ToolbarLeftDockSite!=null)
			{
                objRet = m_ToolbarLeftDockSite.GetDockSiteInfo(pDock, x, y);
				if(objRet.objDockSite!=null)
					return objRet;
			}

			if(pDock.CanDockRight && m_ToolbarRightDockSite!=null)
			{
                objRet = m_ToolbarRightDockSite.GetDockSiteInfo(pDock, x, y);
				if(objRet.objDockSite!=null)
					return objRet;
			}

			return objRet;
		}

		void IOwnerBarSupport.DockComplete()
		{
			CloseDockingHints();
			m_DockingHintSetup=false;
		}

		private bool IsDocumentDockingEnabled
		{
			get {return (m_FillDockSite!=null);}
		}

		#region Docking Hints Support

		private bool CanDockToBar(IDockInfo barDockInfo, Bar referenceBar)
		{
            if (!referenceBar.AcceptDropItems)
                return false;

			if(referenceBar.DockSide==eDockSide.Bottom && !barDockInfo.CanDockBottom)
				return false;
			if(referenceBar.DockSide==eDockSide.Left && !barDockInfo.CanDockLeft)
				return false;
			if(referenceBar.DockSide==eDockSide.Right && !barDockInfo.CanDockRight)
				return false;
			if(referenceBar.DockSide==eDockSide.Document && !barDockInfo.CanDockDocument)
				return false;
			if(referenceBar.DockSide==eDockSide.Top && !barDockInfo.CanDockTop)
				return false;

			return true;
		}

		private DockingHint m_DockingHintLeft=null;
		private DockingHint m_DockingHintRight=null;
		private DockingHint m_DockingHintTop=null;
		private DockingHint m_DockingHintBottom=null;
		private DockingHint m_DockingHintMiddle=null;
		private bool m_DockingHintSetup=false;
		private bool DockingHintHandler(ref DockSiteInfo dockInfo, IDockInfo barDockInfo, int x, int y)
		{
			if(!m_DockingHintSetup)
			{
				m_DockingHintSetup=true;
				return true;
			}
			// Determine whether mouse is inside of any of our bars
			Bar barMouseOver=null;
			foreach(Bar bar in this.Bars)
			{
				if(bar.Visible && (bar!=barDockInfo || this.DesignMode || bar.DockSide==eDockSide.Document))
				{
					Point p=bar.PointToClient(new Point(x,y));
					if(bar.LayoutType==eLayoutType.DockContainer && bar.ClientRectangle.Contains(p) && bar.DockSide!=eDockSide.None && CanDockToBar(barDockInfo, bar))
					{
						barMouseOver=bar;
						break;
					}
				}
			}

			// Setup Docking Hints Windows
			SetupDockingHintWindows(barMouseOver,barDockInfo);

			bool bMiddleDockHint=false;
			dockInfo.DockSiteZOrderIndex=-1;

			eMouseOverHintSide dockHintSide=eMouseOverHintSide.None;
			if(m_DockingHintLeft!=null)
			{
				dockHintSide=m_DockingHintLeft.ExMouseMove(x,y);
			}
			if(m_DockingHintRight!=null)
			{
				eMouseOverHintSide ds=m_DockingHintRight.ExMouseMove(x,y);
				if(dockHintSide==eMouseOverHintSide.None)
					dockHintSide=ds;
			}
			if(m_DockingHintTop!=null)
			{
				eMouseOverHintSide ds=m_DockingHintTop.ExMouseMove(x,y);
				if(dockHintSide==eMouseOverHintSide.None)
					dockHintSide=ds;
			}
			if(m_DockingHintBottom!=null)
			{
				eMouseOverHintSide ds=m_DockingHintBottom.ExMouseMove(x,y);
				if(dockHintSide==eMouseOverHintSide.None)
					dockHintSide=ds;
			}
			if(m_DockingHintMiddle!=null)
			{
				eMouseOverHintSide ds=m_DockingHintMiddle.ExMouseMove(x,y);
				if(dockHintSide==eMouseOverHintSide.None)
				{
					dockHintSide=ds;
					if(ds!=eMouseOverHintSide.None)
						bMiddleDockHint=true;
					if(bMiddleDockHint && this.IsDocumentDockingEnabled && barDockInfo.CanDockDocument && barMouseOver==null && barDockInfo.CanDockDocument)
					{
						if(!barDockInfo.CanDockTop && ds==eMouseOverHintSide.Top)
							dockHintSide=eMouseOverHintSide.DockTab;
						else if(!barDockInfo.CanDockBottom && ds==eMouseOverHintSide.Bottom)
							dockHintSide=eMouseOverHintSide.DockTab;
						else if(!barDockInfo.CanDockLeft && ds==eMouseOverHintSide.Left)
							dockHintSide=eMouseOverHintSide.DockTab;
						else if(!barDockInfo.CanDockRight && ds==eMouseOverHintSide.Right)
							dockHintSide=eMouseOverHintSide.DockTab;
					}
				}
			}
			
			dockInfo.MouseOverBar=barMouseOver;
			if(barMouseOver==null || !bMiddleDockHint)
			{
				if(m_EnableFullSizeDock)
				{
					dockInfo.FullSizeDock=!bMiddleDockHint;
					dockInfo.PartialSizeDock=bMiddleDockHint;
				}
				switch(dockHintSide)
				{
					case eMouseOverHintSide.Left:
					{
                        dockInfo.MouseOverDockSide = eDockSide.Left;
						if(barDockInfo.DockSide!=eDockSide.Left)
						{
							dockInfo.DockSide=DockStyle.Left;
                            if (bMiddleDockHint)
                            {
                                dockInfo.DockLine = 999;
                                dockInfo.InsertPosition = 999;
                            }
                            else
                            {
                                dockInfo.DockLine = -1;
                                dockInfo.InsertPosition = 0;
                            }
                            dockInfo.NewLine = true;
						}
						else
						{
							dockInfo.DockSide=DockStyle.Left;
							dockInfo.DockLine=barDockInfo.DockLine;
							dockInfo.DockOffset=barDockInfo.DockOffset;
							dockInfo.InsertPosition=m_LeftDockSite.Controls.IndexOf(barDockInfo as Control);
						}
						dockInfo.objDockSite=m_LeftDockSite;
						break;
					}
					case eMouseOverHintSide.Right:
					{
                        dockInfo.MouseOverDockSide = eDockSide.Right;
						if(barDockInfo.DockSide!=eDockSide.Right)
						{
							dockInfo.DockSide=DockStyle.Right;
                            if (bMiddleDockHint)
                            {
                                dockInfo.DockLine = -1;
                                dockInfo.InsertPosition = 0;
                            }
                            else
                            {
                                dockInfo.DockLine = 999;
                                dockInfo.InsertPosition = 999;
                            }
                            dockInfo.NewLine = true;
						}
						else
						{
							dockInfo.DockSide=DockStyle.Right;
							dockInfo.DockLine=barDockInfo.DockLine;
							dockInfo.DockOffset=barDockInfo.DockOffset;
							dockInfo.InsertPosition=m_RightDockSite.Controls.IndexOf(barDockInfo as Control);
						}
						dockInfo.objDockSite=m_RightDockSite;
						break;
					}
				
					case eMouseOverHintSide.Top:
					{
                        dockInfo.MouseOverDockSide = eDockSide.Top;
						if(barDockInfo.DockSide!=eDockSide.Top)
						{
							dockInfo.DockSide=DockStyle.Top;
                            if (bMiddleDockHint)
                            {
                                dockInfo.DockLine = 999;
                                dockInfo.InsertPosition = 999;
                            }
                            else
                            {
                                dockInfo.DockLine = -1;
                                dockInfo.InsertPosition = 0;
                            }
							
                            dockInfo.NewLine = true;
						}
						else
						{
							dockInfo.DockSide=DockStyle.Top;
							dockInfo.DockLine=barDockInfo.DockLine;
							dockInfo.DockOffset=barDockInfo.DockOffset;
							dockInfo.InsertPosition=m_TopDockSite.Controls.IndexOf(barDockInfo as Control);
						}
						dockInfo.objDockSite=m_TopDockSite;
						break;
					}
					case eMouseOverHintSide.Bottom:
					{
                        dockInfo.MouseOverDockSide = eDockSide.Bottom;
						if(barDockInfo.DockSide!=eDockSide.Bottom)
						{
							dockInfo.DockSide=DockStyle.Bottom;
                            if (bMiddleDockHint)
                            {
                                dockInfo.DockLine = -1;
                                dockInfo.InsertPosition = 0;
                            }
                            else
                            {
                                dockInfo.DockLine = 999;
                                dockInfo.InsertPosition = 999;
                            }
							dockInfo.NewLine=true;
						}
						else
						{
							dockInfo.DockSide=DockStyle.Bottom;
							dockInfo.DockLine=barDockInfo.DockLine;
							dockInfo.DockOffset=barDockInfo.DockOffset;
							dockInfo.InsertPosition=m_BottomDockSite.Controls.IndexOf(barDockInfo as Control);
						}
						dockInfo.objDockSite=m_BottomDockSite;
						break;
					}
					case eMouseOverHintSide.DockTab:
					{
						if(this.IsDocumentDockingEnabled)
						{
							dockInfo.objDockSite=m_FillDockSite;
							dockInfo.DockSide=DockStyle.Fill;
							dockInfo.MouseOverDockSide=eDockSide.Document;
						}
						break;
					}
				}
			}
			else
			{
				switch(dockHintSide)
				{
					case eMouseOverHintSide.DockTab:
					{
						dockInfo.MouseOverDockSide=eDockSide.Document;
						if(barMouseOver!=null)
						{
							//if(((Bar)barDockInfo).TempTabBar==barMouseOver)
							{
								dockInfo.TabDockContainer=barMouseOver;
								dockInfo.DockSide=barMouseOver.Parent.Dock;
								dockInfo.objDockSite=barMouseOver.Parent as DockSite;
							}
//							else
//							{
//								if(barDockInfo.DockSide!=eDockSide.None)
//								{
//									dockInfo.DockSide=barMouseOver.Parent.Dock;
//									dockInfo.objDockSite=barMouseOver.Parent as DockSite;
//									dockInfo.DockLine=barDockInfo.DockLine;
//									dockInfo.DockOffset=barDockInfo.DockOffset;
//								}
//								else
//								{
//									dockInfo.TabDockContainer=barMouseOver;
//								}
//							}
						}
						break;
					}
					case eMouseOverHintSide.Right:
					{
						dockInfo.MouseOverDockSide=eDockSide.Right;
						dockInfo.DockSide=barMouseOver.Parent.Dock;
						dockInfo.objDockSite=barMouseOver.DockedSite as DockSite;
						switch(barMouseOver.DockSide)
						{
							case eDockSide.Top:
							case eDockSide.Bottom:
							{
                                dockInfo.DockLine = 0; // barMouseOver.DockLine;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver)+1;
								dockInfo.DockOffset=barMouseOver.DockOffset+1;
								break;
							}
							default:
							{
								dockInfo.DockLine=barMouseOver.DockLine+1;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver);
								dockInfo.InsertPosition++;
								dockInfo.NewLine=true;
								break;
							}
						}
						break;
					}
					case eMouseOverHintSide.Left:
					{
						dockInfo.MouseOverDockSide=eDockSide.Left;
						dockInfo.DockSide=barMouseOver.Parent.Dock;
						dockInfo.objDockSite=barMouseOver.DockedSite as DockSite;
						switch(barMouseOver.DockSide)
						{
							case eDockSide.Top:
							case eDockSide.Bottom:
							{
                                dockInfo.DockLine = 0; // barMouseOver.DockLine;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver);
								dockInfo.DockOffset=barMouseOver.DockOffset-1;
								break;
							}
							default:
							{
                                dockInfo.DockLine = 0; // barMouseOver.DockLine - 1;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver);
								dockInfo.NewLine=true;
								break;
							}
						}
						break;
					}
					case eMouseOverHintSide.Top:
					{
						dockInfo.MouseOverDockSide=eDockSide.Top;
						dockInfo.DockSide=barMouseOver.Parent.Dock;
						dockInfo.objDockSite=barMouseOver.DockedSite as DockSite;
						switch(barMouseOver.DockSide)
						{
							case eDockSide.Top:
							case eDockSide.Bottom:
							{
                                dockInfo.DockLine = 0; // barMouseOver.DockLine - 1;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver);
								dockInfo.NewLine=true;
								break;
							}
							default:
							{
                                dockInfo.DockLine = 0;// barMouseOver.DockLine;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver);
								dockInfo.DockOffset=barMouseOver.DockOffset-1;
								break;
							}
						}
						break;
					}
					case eMouseOverHintSide.Bottom:
					{
						dockInfo.MouseOverDockSide=eDockSide.Bottom;
						dockInfo.DockSide=barMouseOver.Parent.Dock;
						dockInfo.objDockSite=barMouseOver.DockedSite as DockSite;
						switch(barMouseOver.DockSide)
						{
							case eDockSide.Top:
							case eDockSide.Bottom:
							{
								dockInfo.DockLine=barMouseOver.DockLine+1;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver);
								if(barMouseOver.DockSide==eDockSide.Bottom)
									dockInfo.InsertPosition++;
								dockInfo.NewLine=true;
								break;
							}
							default:
							{
                                dockInfo.DockLine = 0; // barMouseOver.DockLine;
								dockInfo.InsertPosition=barMouseOver.Parent.Controls.IndexOf(barMouseOver)+1;
								dockInfo.DockOffset=barMouseOver.DockOffset+1;
								break;
							}
						}
						break;
					}
				}
			}

			dockInfo.UseOutline=true;
			return true;
		}

		private bool m_ClosingHints=false;
		private void CloseDockingHints()
		{
			if(m_ClosingHints)
				return;
			try
			{
				m_ClosingHints=true;
				if(m_DockingHintLeft!=null)
				{
					NativeFunctions.SetWindowPos(m_DockingHintLeft.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_HIDEWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE);
					m_DockingHintLeft.Close();
					m_DockingHintLeft.Dispose();
					m_DockingHintLeft=null;
				}

				if(m_DockingHintRight!=null)
				{
					NativeFunctions.SetWindowPos(m_DockingHintRight.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_HIDEWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE);
					m_DockingHintRight.Close();
					m_DockingHintRight.Dispose();
					m_DockingHintRight=null;
				}

				if(m_DockingHintTop!=null)
				{
					NativeFunctions.SetWindowPos(m_DockingHintTop.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_HIDEWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE);
					m_DockingHintTop.Close();
					m_DockingHintTop.Dispose();
					m_DockingHintTop=null;
				}

				if(m_DockingHintBottom!=null)
				{
					NativeFunctions.SetWindowPos(m_DockingHintBottom.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_HIDEWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE);
					m_DockingHintBottom.Close();
					m_DockingHintBottom.Dispose();
					m_DockingHintBottom=null;
				}

				if(m_DockingHintMiddle!=null)
				{
					NativeFunctions.SetWindowPos(m_DockingHintMiddle.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_HIDEWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE);
					m_DockingHintMiddle.Close();
					m_DockingHintMiddle.Dispose();
					m_DockingHintMiddle=null;
				}
			}
			catch{}
			finally
			{
				m_ClosingHints=false;
			}
		}

        private eDotNetBarStyle GetEffectiveStyle()
        {
            if(m_Style== eDotNetBarStyle.StyleManagerControlled)
                return StyleManager.GetEffectiveStyle();
            return m_Style;
        }
        private void SetupDockingHintWindows(Bar barMouseOver, IDockInfo barDockInfo)
		{
			Rectangle parentFormScreenRect=Rectangle.Empty;
			const int HINT_EDGE_OFFSET=32;
			if(m_TopDockSite.Parent.Parent!=null)
			{
				parentFormScreenRect=new Rectangle(m_TopDockSite.Parent.PointToScreen(Point.Empty),m_TopDockSite.Parent.Size);
				parentFormScreenRect.Y+=m_TopDockSite.Height;
				parentFormScreenRect.Height-=m_TopDockSite.Height;
			}
			else
			{
				parentFormScreenRect=m_TopDockSite.Parent.Bounds;
				parentFormScreenRect.Y+=m_TopDockSite.Height;
				parentFormScreenRect.Height-=m_TopDockSite.Height;
			}

			if(!this.DesignMode)
			{
				// Left Docking Hint
				if(barDockInfo.CanDockLeft)
				{
					if(m_DockingHintLeft==null)
					{
						m_DockingHintLeft=new DockingHint(eDockingHintSide.Left, GetEffectiveStyle());
						m_DockingHintLeft.Location=new Point(parentFormScreenRect.X+HINT_EDGE_OFFSET,parentFormScreenRect.Y+(parentFormScreenRect.Height-m_DockingHintLeft.Height)/2);
						m_DockingHintLeft.ShowFocusless();
					}
				}
				else if(m_DockingHintLeft!=null)
				{
					m_DockingHintLeft.Close();
					m_DockingHintLeft.Dispose();
					m_DockingHintLeft=null;
				}

				// Right Docking Hint
				if(barDockInfo.CanDockRight)
				{
					if(m_DockingHintRight==null)
					{
						m_DockingHintRight=new DockingHint(eDockingHintSide.Right, GetEffectiveStyle());
						m_DockingHintRight.Location=new Point(parentFormScreenRect.Right-HINT_EDGE_OFFSET-m_DockingHintRight.Width,parentFormScreenRect.Y+(parentFormScreenRect.Height-m_DockingHintRight.Height)/2);
						m_DockingHintRight.ShowFocusless();
					}
				}
				else if(m_DockingHintRight!=null)
				{
					m_DockingHintRight.Close();
					m_DockingHintRight.Dispose();
					m_DockingHintRight=null;
				}

				// Top Docking Hint
				if(barDockInfo.CanDockTop)
				{
					if(m_DockingHintTop==null)
					{
						m_DockingHintTop=new DockingHint(eDockingHintSide.Top, GetEffectiveStyle());
						m_DockingHintTop.Location=new Point(parentFormScreenRect.X+(parentFormScreenRect.Width-m_DockingHintTop.Width)/2,parentFormScreenRect.Y+HINT_EDGE_OFFSET);
						m_DockingHintTop.ShowFocusless();
					}
				}
				else if(m_DockingHintTop!=null)
				{
					m_DockingHintTop.Close();
					m_DockingHintTop.Dispose();
					m_DockingHintTop=null;
				}

				// Bottom Docking Hint
				if(barDockInfo.CanDockBottom)
				{
					if(m_DockingHintBottom==null)
					{
						m_DockingHintBottom=new DockingHint(eDockingHintSide.Bottom, GetEffectiveStyle());
						m_DockingHintBottom.Location=new Point(parentFormScreenRect.X+(parentFormScreenRect.Width-m_DockingHintBottom.Width)/2,parentFormScreenRect.Bottom-HINT_EDGE_OFFSET-m_DockingHintBottom.Height);
						m_DockingHintBottom.ShowFocusless();
					}
				}
				else if(m_DockingHintBottom!=null)
				{
					m_DockingHintBottom.Close();
					m_DockingHintBottom.Dispose();
					m_DockingHintBottom=null;
				}
			}

			// Middle docking hint
			bool bShowMidHint=false;
			eDockingHintSide hintSides=eDockingHintSide.All;
			if(barMouseOver==null)
			{
				hintSides=eDockingHintSide.Top;
				if(barDockInfo.CanDockBottom)
					hintSides=eDockingHintSide.Bottom;
				if(barDockInfo.CanDockLeft)
					hintSides=hintSides | eDockingHintSide.Left;
				if(barDockInfo.CanDockRight)
					hintSides=hintSides | eDockingHintSide.Right;
				if(barDockInfo.CanDockTop)
					hintSides=hintSides | eDockingHintSide.Top;
				else
					hintSides=hintSides & ~(hintSides & eDockingHintSide.Top);
				if(this.IsDocumentDockingEnabled && barDockInfo.CanDockDocument)
				{
					hintSides=eDockingHintSide.All;
				}
			}
			else
			{
				if(!barDockInfo.CanDockTab)
					hintSides=hintSides & ~(hintSides & eDockingHintSide.DockTab);
			}
			
			if(m_DockingHintMiddle==null)
			{
				if(barMouseOver!=null)
					m_DockingHintMiddle=new DockingHint(hintSides,true, GetEffectiveStyle());
				else
                    m_DockingHintMiddle = new DockingHint(hintSides, true, GetEffectiveStyle());
				bShowMidHint=true;
			}
			else
			{
				if(barMouseOver!=null)
					m_DockingHintMiddle.DockingHintSides=hintSides; // eDockingHintSide.All;
				else
					m_DockingHintMiddle.DockingHintSides=hintSides; //eDockingHintSide.Sides;
			}

			Point midLocation=new Point(parentFormScreenRect.X+(parentFormScreenRect.Width-m_DockingHintMiddle.Width)/2,
				parentFormScreenRect.Y+(parentFormScreenRect.Height-m_DockingHintMiddle.Height)/2);

			if(barMouseOver!=null)
			{
				Point p=barMouseOver.PointToScreen(Point.Empty);
				midLocation=new Point(p.X+(barMouseOver.Width-m_DockingHintMiddle.Width)/2,
					p.Y+(barMouseOver.Height-m_DockingHintMiddle.Height)/2);
			}

			Rectangle r=new Rectangle(midLocation,m_DockingHintMiddle.Size);
			if(m_DockingHintLeft!=null && r.IntersectsWith(m_DockingHintLeft.Bounds))
			{
				if(Control.MousePosition.Y<m_DockingHintLeft.Top)
					midLocation.Y=m_DockingHintLeft.Top-m_DockingHintMiddle.Height;
				else
					midLocation.Y=m_DockingHintLeft.Bottom;
			}
			else if(m_DockingHintRight!=null && r.IntersectsWith(m_DockingHintRight.Bounds))
			{
				if(Control.MousePosition.Y<m_DockingHintRight.Top)
					midLocation.Y=m_DockingHintRight.Top-m_DockingHintMiddle.Height;
				else
					midLocation.Y=m_DockingHintRight.Bottom;
			}
			else if(m_DockingHintTop!=null && r.IntersectsWith(m_DockingHintTop.Bounds))
			{
				if(Control.MousePosition.X>m_DockingHintTop.Left)
					midLocation.X=m_DockingHintTop.Right;
				else
					midLocation.X=m_DockingHintTop.Left-m_DockingHintMiddle.Width;
			}
			else if(m_DockingHintBottom!=null && r.IntersectsWith(m_DockingHintBottom.Bounds))
			{
				if(Control.MousePosition.X>m_DockingHintBottom.Left)
					midLocation.X=m_DockingHintBottom.Right;
				else
					midLocation.X=m_DockingHintBottom.Left-m_DockingHintMiddle.Width;
			}

			if(m_DockingHintMiddle.Location!=midLocation)
			{
				m_DockingHintMiddle.Location=midLocation;
			}

			if(bShowMidHint)
			{
				m_DockingHintMiddle.ShowFocusless();
			}

		}
		#endregion

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
		/// Gets or sets the collection of shortcut keys that are automatically dispatched to the control that has focus even if they are handled and used by one of the items. This gives you fine control over which shortcuts are passed through the system and which ones are marked as handled by DotNetBar.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates list of shortcut keys that are automatically dispatched to the control that has focus even if they are handled and used by one of the items."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public ShortcutsCollection AutoDispatchShortcuts
		{
			get
			{
				return m_AutoDispatchShortcuts;
			}
			set
			{
				m_AutoDispatchShortcuts=value;
				m_AutoDispatchShortcuts.Parent=null;
			}
		}

		/// <summary>
		/// Specifes the auto-hide animation speed value from 0-100. 0 value disables the animation.
		/// </summary>
		//[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never),DefaultValue(100),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifes the auto-hide animation speed value from 0-100. The 0 disables the animation.")]
//		int IOwnerBarSupport.AutoHideAnimationSpeed
//		{
//			get {return m_AutoHideAnimationSpeed;}
//			set
//			{
//				m_AutoHideAnimationSpeed=value;
//				if(m_AutoHideAnimationSpeed<0)
//					m_AutoHideAnimationSpeed=0;
//				else if(m_AutoHideAnimationSpeed>100)
//					m_AutoHideAnimationSpeed=100;
//			}
//		}

		/// <summary>
		/// Indicates whether Reset buttons is shown that allows end-user to reset the toolbar state.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether Reset buttons is shown that allows end-user to reset the toolbar state.")]
		public bool ShowResetButton
		{
			get
			{
				return m_ShowResetButton;
			}
			set
			{
				m_ShowResetButton=value;
			}
		}

		/// <summary>
		/// ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Data"),Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
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

		/// <summary>
		/// Suspends the bar layout for all bars.
		/// </summary>
		[System.ComponentModel.Browsable(false),DefaultValue(false)]
		public bool SuspendLayout
		{
			get
			{
				return m_SuspendLayout;
			}
			set
			{
				if(m_SuspendLayout!=value)
				{
					m_SuspendLayout=value;
					if(!m_SuspendLayout)
					{
						if(m_TopDockSite!=null && m_TopDockSite.NeedsLayout)
							m_TopDockSite.RecalcLayout();
						if(m_BottomDockSite!=null && m_BottomDockSite.NeedsLayout)
							m_BottomDockSite.RecalcLayout();
						if(m_LeftDockSite!=null && m_LeftDockSite.NeedsLayout)
							m_LeftDockSite.RecalcLayout();
						if(m_RightDockSite!=null && m_RightDockSite.NeedsLayout)
							m_RightDockSite.RecalcLayout();
						if(m_FillDockSite!=null && m_FillDockSite.NeedsLayout)
							m_FillDockSite.RecalcLayout();

                        if (m_ToolbarTopDockSite != null && m_ToolbarTopDockSite.NeedsLayout)
                            m_ToolbarTopDockSite.RecalcLayout();
                        if (m_ToolbarBottomDockSite != null && m_ToolbarBottomDockSite.NeedsLayout)
                            m_ToolbarBottomDockSite.RecalcLayout();
                        if (m_ToolbarLeftDockSite != null && m_ToolbarLeftDockSite.NeedsLayout)
                            m_ToolbarLeftDockSite.RecalcLayout();
                        if (m_ToolbarRightDockSite != null && m_ToolbarRightDockSite.NeedsLayout)
                            m_ToolbarRightDockSite.RecalcLayout();
					}
				}
			}
		}

		/// <summary>
		/// Specifes whether drop shadow is displayed for Menus and pop-up Bars. OfficeXP Style only.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(eMenuDropShadow.SystemDefault),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifes whether drop shadow is displayed for Menus and pop-up Bars. OfficeXP Style only.")]
		public eMenuDropShadow MenuDropShadow
		{
			get
			{
				return m_MenuDropShadow;
			}
			set
			{
				m_MenuDropShadow=value;
			}
		}

		/// <summary>
		/// Specifes whether to use Alpha-Blending shadows for pop-up items if supported by target OS. Disabling Alpha-Blended shadows can improve performance.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifes whether to use Alpha-Blending shadows for pop-up items if supported by target OS. Disabling Alpha-Blended shadows can improve performance.")]
		public bool AlphaBlendShadow
		{
			get
			{
				return m_AlphaBlendShadow;
			}
			set
			{
				m_AlphaBlendShadow=value;
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
		/// Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
		public bool UseHook
		{
			get
			{
				return m_UseHook;
			}
			set
			{
				if(m_UseHook==value)
					return;
				m_UseHook=value;
				
				if(m_ParentForm==null)
					return;

				if(m_UseHook)
				{
					if(m_FilterInstalled)
						MessageHandler.UnregisterMessageClient(this);
						//Application.RemoveMessageFilter(this);
					m_FilterInstalled=false;
					if(m_Hook==null)
						m_Hook=new Hook(this);
				}
				else
				{
					if(m_Hook!=null)
					{
						m_Hook.Dispose();
						m_Hook=null;
					}
					if(!this.DesignMode)
					{
						if(!m_FilterInstalled)
							MessageHandler.RegisterMessageClient(this);
							//Application.AddMessageFilter(this);
						m_FilterInstalled=true;
					}
				}
			}
		}

		bool IOwnerMenuSupport.ShowPopupShadow
		{
			get
			{
				if(m_MenuDropShadow==eMenuDropShadow.Show)
					return true;
				else if(m_MenuDropShadow==eMenuDropShadow.Hide)
					return false;
				// W2K and gang
				if(Environment.OSVersion.Version.Major==5 && Environment.OSVersion.Version.Minor<1 || Environment.OSVersion.Version.Major<5)
					return true;
				
				return NativeFunctions.ShowDropShadow;
			}
		}

		/// <summary>
		/// Loads DotNetBar definition from file.
		/// </summary>
		/// <param name="FileName">File that contains DotNetBar defintion.</param>
		public void LoadDefinition(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			xmlDoc.Load(FileName);
			LoadDefinition(xmlDoc);
			
		}

        private Hashtable GetDockControls()
        {
            Hashtable h = new Hashtable();
            foreach (Bar bar in this.Bars)
            {
                if (bar.LayoutType != eLayoutType.DockContainer)
                    continue;
                foreach (BaseItem item in bar.Items)
                {
                    if (item.Name !="" && item is DockContainerItem)
                    {
                        DockContainerItem dock = item as DockContainerItem;
                        if (dock.Control != null)
                        {
                            try
                            {
                                h.Add(dock.Name, dock.Control);
                                dock.Control = null;
                            }
                            catch { }
                        }
                    }
                }
            }
            if (h.Keys.Count == 0)
                return null;
            return h;
        }

		internal void LoadDefinition(System.Xml.XmlDocument xmlDoc)
		{
            System.Xml.XmlElement xmlDotNetBar=xmlDoc.FirstChild as System.Xml.XmlElement;

            // Do not load old uncompatible definitions
            if (xmlDotNetBar != null && (!xmlDotNetBar.HasAttribute(DocumentSerializationXml.Version) ||
               XmlConvert.ToInt32(xmlDotNetBar.GetAttribute(DocumentSerializationXml.Version)) < 6))
                return;

			bool bSuspendLayout=false;

            // Creates serialization context
            ItemSerializationContext context = new ItemSerializationContext();
            context.Serializer = this;
            context.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
            context.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;

            // Cache content of DockContainerItem objects so it can be loaded by new definition if they match
            context.DockControls = GetDockControls();

			try
			{
				m_LoadingDefinition=true;

				if(!this.SuspendLayout)
				{
					bSuspendLayout=true;
					this.SuspendLayout=true;
				}

				try
				{
					IgnoreLoadedControlDispose=true;
					m_Items.Clear();
					if(m_IncludeDockDocumentsInDefinition)
						m_Bars.Clear();
					else
						m_Bars.ClearNonDocumentBars();
					m_ContextMenus.Clear();
					m_ShortcutTable.Clear();
				}
				finally
				{
					IgnoreLoadedControlDispose=false;
				}
				
				// Destroy auto-hide sites
				DestroyAutoHidePanels();			

				if(xmlDotNetBar==null)
				{
					if(bSuspendLayout)
						this.SuspendLayout=false;
					return;
				}

				if(xmlDotNetBar.Name!="dotnetbar")
					throw new System.InvalidOperationException("Invalid file format (dotnetbar).");

//				if(xmlDotNetBar.HasAttribute("zorder"))
//					RestoreDockSiteZOrder(xmlDotNetBar.GetAttribute("zorder"));
			
				// Load user defined settings
				if(xmlDotNetBar.HasAttribute("fullmenus"))
					m_AlwaysShowFullMenus=System.Xml.XmlConvert.ToBoolean(xmlDotNetBar.GetAttribute("fullmenus"));
				else
					m_AlwaysShowFullMenus=false;
				if(xmlDotNetBar.HasAttribute("fullmenushover"))
					m_ShowFullMenusOnHover=System.Xml.XmlConvert.ToBoolean(xmlDotNetBar.GetAttribute("fullmenushover"));
				else
					m_ShowFullMenusOnHover=true;
				if(xmlDotNetBar.HasAttribute("tooltips"))
					m_ShowToolTips=System.Xml.XmlConvert.ToBoolean(xmlDotNetBar.GetAttribute("tooltips"));
				else
					m_ShowToolTips=true;
				if(xmlDotNetBar.HasAttribute("scintooltip"))
					m_ShowShortcutKeysInToolTips=System.Xml.XmlConvert.ToBoolean(xmlDotNetBar.GetAttribute("scintooltip"));
				else
					m_ShowShortcutKeysInToolTips=false;
				if(xmlDotNetBar.HasAttribute("animation"))
				{
					if((ePopupAnimation)System.Xml.XmlConvert.ToInt32(xmlDotNetBar.GetAttribute("animation"))!=ePopupAnimation.SystemDefault)
						m_PopupAnimation=(ePopupAnimation)System.Xml.XmlConvert.ToInt32(xmlDotNetBar.GetAttribute("animation"));
				}
				else
					m_PopupAnimation=ePopupAnimation.SystemDefault;
				
				foreach(System.Xml.XmlElement xmlElem in xmlDotNetBar.ChildNodes)
				{
					if(xmlElem.Name=="items")
					{
						foreach(System.Xml.XmlElement xmlItem in xmlElem.ChildNodes)
						{
							BaseItem objItem=BarFunctions.CreateItemFromXml(xmlItem);
							if(objItem==null)
								throw new System.InvalidOperationException("Invalid Item in file found ("+BarFunctions.GetItemErrorInfo(xmlItem)+").");
							// Do not change this order. Item should be Deserialized first and then added to the items
							// collection. See GlobalItem property for performace reasons...
                            context.ItemXmlElement = xmlItem;
							objItem.Deserialize(context);
							m_Items.Add(objItem);
						}
					}
					else if(xmlElem.Name=="bars")
					{
						foreach(System.Xml.XmlElement xmlBar in xmlElem.ChildNodes)
						{
							DevComponents.DotNetBar.Bar bar=new DevComponents.DotNetBar.Bar();
							bar.Visible=false;
							bar.SetDesignMode(this.DesignMode);
							m_Bars.Add(bar);
							bar.Deserialize(xmlBar);
							IOwnerBarSupport ownersupport=this as IOwnerBarSupport;
							ownersupport.AddShortcutsFromBar(bar);
						}
					}
					else if(xmlElem.Name=="popups")
					{
						foreach(System.Xml.XmlElement xmlItem in xmlElem.ChildNodes)
						{
							BaseItem objItem=BarFunctions.CreateItemFromXml(xmlItem);
							if(objItem==null)
								throw new System.InvalidOperationException("Invalid Item in file found ("+BarFunctions.GetItemErrorInfo(xmlItem)+").");
							// Do not change this order. Item should be Deserialized first and then added to the items
							// collection. See GlobalItem property for performace reasons...
                            context.ItemXmlElement = xmlItem;
							objItem.Deserialize(context);
							m_ContextMenus.Add(objItem);
						}
					}
					else if(xmlElem.Name==DocumentSerializationXml.Documents && m_IncludeDockDocumentsInDefinition)
					{
                        context.ItemXmlElement = xmlElem;
						if(m_FillDockSite!=null)
							m_FillDockSite.GetDocumentUIManager().DeserializeDefinition(context);
					}
                    else if (xmlElem.Name == DocumentSerializationXml.DockSite)
                    {
                        context.ItemXmlElement = xmlElem;

                        DockStyle dockingSide = (DockStyle)Enum.Parse(typeof(DockStyle), xmlElem.GetAttribute(DocumentSerializationXml.DockingSide));
                        if (dockingSide == DockStyle.Left)
                        {
                            if (m_LeftDockSite != null) m_LeftDockSite.GetDocumentUIManager().DeserializeDefinition(context);
                        }
                        else if (dockingSide == DockStyle.Right)
                        {
                            if (m_RightDockSite != null) m_RightDockSite.GetDocumentUIManager().DeserializeDefinition(context);
                        }
                        else if (dockingSide == DockStyle.Top)
                        {
                            if (m_TopDockSite != null) m_TopDockSite.GetDocumentUIManager().DeserializeDefinition(context);
                        }
                        else if (dockingSide == DockStyle.Bottom)
                        {
                            if (m_BottomDockSite != null) m_BottomDockSite.GetDocumentUIManager().DeserializeDefinition(context);
                        }
                    }
				}
			}
			finally
			{
				m_LoadingDefinition=false;
				if(bSuspendLayout) this.SuspendLayout=false;
			}

            if (context.DockControls != null && context.DockControls.Count > 0)
            {
                foreach (Control c in context.DockControls.Values)
                    c.Dispose();
            }

			((IOwner)this).InvokeDefinitionLoaded(this,new EventArgs());

			if(m_ActiveMdiChild!=null && m_MdiChildMaximized)
			{
				m_MdiChildMaximized=false;
                this.OnMdiChildResize(m_ActiveMdiChild,new EventArgs());
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
        }

        /// <summary>
        /// Invokes DeserializeItem event.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        void ICustomSerialization.InvokeDeserializeItem(SerializeItemEventArgs e)
        {
            if (DeserializeItem != null)
                DeserializeItem(this, e);
        }

        /// <summary>
        /// Gets whether any handlers have been defined for SerializeItem event. If no handles have been defined to optimize performance SerializeItem event will not be attempted to fire.
        /// </summary>
        bool ICustomSerialization.HasSerializeItemHandlers
        {
            get
            {
                return SerializeItem != null;
            }
        }

        /// <summary>
        /// Gets whether any handlers have been defined for DeserializeItem event. If no handles have been defined to optimize performance DeserializeItem event will not be attempted to fire.
        /// </summary>
        bool ICustomSerialization.HasDeserializeItemHandlers
        {
            get
            {
                return DeserializeItem != null;
            }
        }
        #endregion

        /// <summary>
		/// Returns whether definition is being currently loaded.
		/// </summary>
		internal bool IsLoadingDefinition
		{
			get {return m_LoadingDefinition;}
		}

		/// <summary>
		/// Gets or sets whether document bars are saved in definition file. Default value is false which means that document
		/// bars are not saved as part of definition file. You can set this value to true to save document bar to definition file and 
		/// be able to load them.
		/// </summary>
		/// <remarks>
		/// Note that by default Document bars that you created during design-time get member
		/// variables assigned to them by Windows Forms designer. If you decide to save definition
		/// of such bars and load definition back member variables will not point to correct bar
		/// instances since loading definition recreates all bars from scratch. You should always
		/// use Bars collection to access bars when saving and loading definitions.
		/// </remarks>
		[Browsable(false),DefaultValue(false)]
		public bool IncludeDockDocumentsInDefinition
		{
			get {return m_IncludeDockDocumentsInDefinition;}
			set {m_IncludeDockDocumentsInDefinition=value;}
		}

		/// <summary>
		/// Gets or sets the DotNetBar definition string.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>
		/// Gets or sets the DotNetBar layout string.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>
		/// Saves current DotNetBar definition and state to the file.
		/// </summary>
		/// <param name="FileName">File name.</param>
		public void SaveDefinition(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			SaveDefinition(xmlDoc);
			xmlDoc.Save(FileName);
		}

		internal void SaveDefinition(System.Xml.XmlDocument xmlDoc)
		{
            // Creates serialization context
            ItemSerializationContext context = new ItemSerializationContext();
            context.Serializer = this;
            context.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
            context.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;

			// Save first items from categories
			System.Xml.XmlElement xmlDotNetBar=xmlDoc.CreateElement("dotnetbar");
            xmlDotNetBar.SetAttribute(DocumentSerializationXml.Version, "6");
			xmlDoc.AppendChild(xmlDotNetBar);

			// Save user settings
			xmlDotNetBar.SetAttribute("fullmenus",System.Xml.XmlConvert.ToString(m_AlwaysShowFullMenus));
			xmlDotNetBar.SetAttribute("fullmenushover",System.Xml.XmlConvert.ToString(m_ShowFullMenusOnHover));
			xmlDotNetBar.SetAttribute("tooltips",System.Xml.XmlConvert.ToString(m_ShowToolTips));
			xmlDotNetBar.SetAttribute("scintooltip",System.Xml.XmlConvert.ToString(m_ShowShortcutKeysInToolTips));
			xmlDotNetBar.SetAttribute("animation",System.Xml.XmlConvert.ToString((int)m_PopupAnimation));

			System.Xml.XmlElement xmlItems=xmlDoc.CreateElement("items");
			xmlDotNetBar.AppendChild(xmlItems);

			foreach(DictionaryEntry o in m_Items)
			{
				BaseItem objItem=o.Value as BaseItem;
				System.Xml.XmlElement xmlItem=xmlDoc.CreateElement("item");
				xmlItems.AppendChild(xmlItem);
                context.ItemXmlElement = xmlItem;
				objItem.Serialize(context);
			}

			// Go through the Bars and serialize each
			System.Xml.XmlElement xmlBars=xmlDoc.CreateElement("bars");
			xmlDotNetBar.AppendChild(xmlBars);
			
			// Serialize docked bars
			if(m_TopDockSite!=null)
                SerializeDockSite(m_TopDockSite, xmlDotNetBar);
			if(m_BottomDockSite!=null)
                SerializeDockSite(m_BottomDockSite, xmlDotNetBar);
			if(m_LeftDockSite!=null)
                SerializeDockSite(m_LeftDockSite, xmlDotNetBar);
			if(m_RightDockSite!=null)
                SerializeDockSite(m_RightDockSite, xmlDotNetBar);
			if(m_IncludeDockDocumentsInDefinition && m_FillDockSite!=null)
                SerializeDockSite(m_FillDockSite,xmlDotNetBar);

            // Serialize toolbars bars
            if (m_ToolbarTopDockSite != null)
                SerializeDockSite(m_ToolbarTopDockSite, xmlBars);
            if (m_ToolbarBottomDockSite != null)
                SerializeDockSite(m_ToolbarBottomDockSite, xmlBars);
            if (m_ToolbarLeftDockSite != null)
                SerializeDockSite(m_ToolbarLeftDockSite, xmlBars);
            if (m_ToolbarRightDockSite != null)
                SerializeDockSite(m_ToolbarRightDockSite, xmlBars);

			// Serialize floating bars
			foreach(DevComponents.DotNetBar.Bar bar in m_Bars)
			{
				if(bar.DockSide==eDockSide.None)
				{
					System.Xml.XmlElement xmlBar=xmlDoc.CreateElement("bar");
					xmlBars.AppendChild(xmlBar);
					bar.Serialize(xmlBar);
				}
			}

			// Serialize any popup items...
			System.Xml.XmlElement xmlPopups=xmlDoc.CreateElement("popups");
			xmlDotNetBar.AppendChild(xmlPopups);
			foreach(BaseItem objItem in m_ContextMenus)
			{
				System.Xml.XmlElement xmlItem=xmlDoc.CreateElement("item");
				xmlPopups.AppendChild(xmlItem);
                context.ItemXmlElement = xmlItem;
				objItem.Serialize(context);
			}
		}

		/// <summary>
		/// Loads DotNetBar layout from file.
		/// </summary>
		/// <param name="FileName">File that contains DotNetBar defintion.</param>
		public void LoadLayout(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			xmlDoc.Load(FileName);
			LoadLayout(xmlDoc);
			
		}

		internal void LoadLayout(System.Xml.XmlDocument xmlDoc)
		{
            System.Xml.XmlElement xmlDotNetBar = xmlDoc.FirstChild as System.Xml.XmlElement;
            if (xmlDotNetBar == null || !xmlDotNetBar.HasAttribute(DocumentSerializationXml.Version) ||
                XmlConvert.ToInt32(xmlDotNetBar.GetAttribute(DocumentSerializationXml.Version))<6)
                return;

			// Destroy auto-hide sites
			DestroyAutoHidePanels();
			
			bool bSuspendLayout=false;
            m_LoadingLayout = true;

			try
			{
				if(xmlDotNetBar.Name!="dotnetbarlayout")
					throw new System.InvalidOperationException("Invalid file format (dotnetbarlayout expected).");

				if(!this.SuspendLayout)
				{
					bSuspendLayout=true;
					this.SuspendLayout=true;
				}

				foreach(System.Xml.XmlElement xmlElem in xmlDotNetBar.ChildNodes)
				{
					if(xmlElem.Name=="bars")
					{
						foreach(System.Xml.XmlElement xmlBar in xmlElem.ChildNodes)
						{
                            if (xmlBar.Name != "bar")
                                continue;

							DevComponents.DotNetBar.Bar bar=null;
							if(m_Bars.Contains(m_Bars[xmlBar.GetAttribute("name")]))
								bar=m_Bars[xmlBar.GetAttribute("name")];
							else
							{
								bar=new Bar();
								this.Bars.Add(bar);
							}
							bar.DeserializeLayout(xmlBar);
						}
					}
					else if(xmlElem.Name==DocumentSerializationXml.Documents)
					{
						if(m_FillDockSite!=null)
							m_FillDockSite.GetDocumentUIManager().DeserializeLayout(xmlElem);
					}
                    else if (xmlElem.Name == DocumentSerializationXml.DockSite)
                    {
                        DockStyle dockingSide = (DockStyle)Enum.Parse(typeof(DockStyle), xmlElem.GetAttribute(DocumentSerializationXml.DockingSide));
                        if (dockingSide == DockStyle.Left)
                        {
                            if (m_LeftDockSite != null) m_LeftDockSite.GetDocumentUIManager().DeserializeLayout(xmlElem);
                        }
                        else if (dockingSide == DockStyle.Right)
                        {
                            if (m_RightDockSite != null) m_RightDockSite.GetDocumentUIManager().DeserializeLayout(xmlElem);
                        }
                        else if (dockingSide == DockStyle.Top)
                        {
                            if (m_TopDockSite != null) m_TopDockSite.GetDocumentUIManager().DeserializeLayout(xmlElem);
                        }
                        else if (dockingSide == DockStyle.Bottom)
                        {
                            if (m_BottomDockSite != null) m_BottomDockSite.GetDocumentUIManager().DeserializeLayout(xmlElem);
                        }
                    }
				}
                if (xmlDotNetBar.HasAttribute("zorder"))
                    RestoreDockSiteZOrder(xmlDotNetBar.GetAttribute("zorder"));

                // Clean up custom created empty bars
                Bar[] bars = new Bar[m_Bars.Count];
                m_Bars.CopyTo(bars);
                foreach (Bar bar in bars)
                {
                    if (bar.CustomBar && bar.Items.Count == 0)
                    {
                        bar.Visible = false;
                        this.Bars.Remove(bar);
                        bar.Dispose();
                    }
                }
			}
			finally
			{
				if(bSuspendLayout) this.SuspendLayout=false;
                m_LoadingLayout = false;
			}

			if(m_ActiveMdiChild!=null && m_MdiChildMaximized)
			{
				m_MdiChildMaximized=false;
				this.OnMdiChildResize(m_ActiveMdiChild,new EventArgs());
			}
		}

        /// <summary>
        /// Gets whether DotNetBarManager is loading layout.
        /// </summary>
        [Browsable(false)]
        public bool IsLoadingLayout
        {
            get { return m_LoadingLayout; }
        }

		/// <summary>
		/// Saves current DotNetBar layout to the file.
		/// </summary>
		/// <param name="FileName">File name.</param>
		public void SaveLayout(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			SaveLayout(xmlDoc);
			xmlDoc.Save(FileName);
		}

		internal void SaveLayout(System.Xml.XmlDocument xmlDoc)
		{
			// Save first items from categories
			System.Xml.XmlElement xmlDotNetBar=xmlDoc.CreateElement("dotnetbarlayout");
            xmlDotNetBar.SetAttribute(DocumentSerializationXml.Version, "6");
			xmlDoc.AppendChild(xmlDotNetBar);
			xmlDotNetBar.SetAttribute("zorder",GetDockSitesZOrder());
			
			// Serialize docked bars
			if(m_TopDockSite!=null)
                SerializeDockSiteLayout(m_TopDockSite, xmlDotNetBar);
			if(m_BottomDockSite!=null)
                SerializeDockSiteLayout(m_BottomDockSite, xmlDotNetBar);
			if(m_LeftDockSite!=null)
                SerializeDockSiteLayout(m_LeftDockSite, xmlDotNetBar);
			if(m_RightDockSite!=null)
                SerializeDockSiteLayout(m_RightDockSite, xmlDotNetBar);
			if(m_FillDockSite!=null)
                SerializeDockSiteLayout(m_FillDockSite, xmlDotNetBar);

            // Go through the Bars and serialize each
            System.Xml.XmlElement xmlBars = xmlDoc.CreateElement("bars");
            xmlDotNetBar.AppendChild(xmlBars);
			
            // Serialize Toolbars
            if (m_ToolbarTopDockSite != null)
                SerializeDockSiteLayout(m_ToolbarTopDockSite, xmlBars);
            if (m_ToolbarBottomDockSite != null)
                SerializeDockSiteLayout(m_ToolbarBottomDockSite, xmlBars);
            if (m_ToolbarLeftDockSite != null)
                SerializeDockSiteLayout(m_ToolbarLeftDockSite, xmlBars);
            if (m_ToolbarRightDockSite != null)
                SerializeDockSiteLayout(m_ToolbarRightDockSite, xmlBars);

			// Serialize floating bars
			foreach(DevComponents.DotNetBar.Bar bar in m_Bars)
			{
				if(bar.DockSide==eDockSide.None && bar.Name!="" && bar.SaveLayoutChanges)
				{
					System.Xml.XmlElement xmlBar=xmlDoc.CreateElement("bar");
					xmlBars.AppendChild(xmlBar);
					bar.SerializeLayout(xmlBar);
				}
			}
		}

		private string GetDockSitesZOrder()
		{
			string s="";
			if(m_TopDockSite!=null && m_TopDockSite.Parent!=null)
				s+=m_TopDockSite.Parent.Controls.IndexOf(m_TopDockSite);
			s+=",";
			if(m_BottomDockSite!=null && m_BottomDockSite.Parent!=null)
				s+=m_BottomDockSite.Parent.Controls.IndexOf(m_BottomDockSite);
			s+=",";
			if(m_LeftDockSite!=null && m_LeftDockSite.Parent!=null)
				s+=m_LeftDockSite.Parent.Controls.IndexOf(m_LeftDockSite);
			s+=",";
			if(m_RightDockSite!=null)
				s+=m_RightDockSite.Parent.Controls.IndexOf(m_RightDockSite);
			return s;
		}

		private void RestoreDockSiteZOrder(string s)
		{
			try
			{
				string[] sa=s.Split(',');
				if(sa[0]!="" && m_TopDockSite!=null && m_TopDockSite.Parent!=null)
					m_TopDockSite.Parent.Controls.SetChildIndex(m_TopDockSite,int.Parse(sa[0]));

				if(sa[1]!="" && m_BottomDockSite!=null && m_BottomDockSite.Parent!=null)
					m_BottomDockSite.Parent.Controls.SetChildIndex(m_BottomDockSite,int.Parse(sa[1]));

				if(sa[2]!="" && m_LeftDockSite!=null && m_LeftDockSite.Parent!=null)
					m_LeftDockSite.Parent.Controls.SetChildIndex(m_LeftDockSite,int.Parse(sa[2]));

				if(sa[3]!="" && m_RightDockSite!=null && m_RightDockSite.Parent!=null)
					m_RightDockSite.Parent.Controls.SetChildIndex(m_RightDockSite,int.Parse(sa[3]));
			}
			catch{}
		}

		private void SerializeDockSite(DockSite site, System.Xml.XmlElement xmlBars)
		{
			if(site.IsDocumentDock || site.DocumentDockContainer != null)
			{
				site.GetDocumentUIManager().SerializeDefinition(xmlBars);
			}
			else
			{
				foreach(Control ctrl in site.Controls)
				{
					DevComponents.DotNetBar.Bar bar=ctrl as DevComponents.DotNetBar.Bar;
					if(bar!=null)
					{
						System.Xml.XmlElement xmlBar=xmlBars.OwnerDocument.CreateElement("bar");
						xmlBars.AppendChild(xmlBar);
						bar.Serialize(xmlBar);
					}
				}
			}
		}

		private void SerializeDockSiteLayout(DockSite site, System.Xml.XmlElement xmlBars)
		{
			if(site.IsDocumentDock || site.DocumentDockContainer!=null)
			{
				site.GetDocumentUIManager().SerializeLayout(xmlBars);
			}
			else
			{
				foreach(Control ctrl in site.Controls)
				{
					DevComponents.DotNetBar.Bar bar=ctrl as DevComponents.DotNetBar.Bar;
					if(bar!=null && bar.Name!="" && bar.SaveLayoutChanges)
					{
						System.Xml.XmlElement xmlBar=xmlBars.OwnerDocument.CreateElement("bar");
						xmlBars.AppendChild(xmlBar);
						bar.SerializeLayout(xmlBar);
					}
				}
			}
		}

		internal void SetFloatingBarVisible(bool bVisible)
		{
			foreach(Bar bar in m_Bars)
			{
				if(bar.DockSide==eDockSide.None && bar.Visible!=bVisible)
					bar.Visible=bVisible;
			}
		}

        private bool m_AutoCreatedCategories = false;
		/// <summary>
		/// Invokes the DotNetBar Customize dialog.
		/// </summary>
		public void Customize()
		{
			// Disable all child controls on parent form
			if(m_DisabledControls==null)
				m_DisabledControls=new ArrayList();

			Form objParent=m_ParentForm.TopLevelControl as Form;
			if(objParent==null)
				objParent=this.ParentForm;
			foreach(Control objCtrl in objParent.Controls)
			{
				if(!(objCtrl is DockSite) && objCtrl.Enabled)
				{
					objCtrl.Enabled=false;
					m_DisabledControls.Add(objCtrl);
				}
			}

			m_Customizing=true;

            if (this.Items.Count == 0)
            {
                m_AutoCreatedCategories = true;
                RescanCategories();
            }

			if(!m_UseCustomCustomizeDialog)
			{
				if(m_frmCustomize==null)
					m_frmCustomize=new frmCustomize(this);
				if(EnterCustomize!=null)
					EnterCustomize(m_frmCustomize,new EventArgs());
				m_frmCustomize.Show();
				m_frmCustomize.Owner=this.ParentForm;
			}
			else
			{
				if(EnterCustomize!=null)
					EnterCustomize(null,new EventArgs());
				else
					MessageBox.Show("You need to add event handler for EnterCustomize event since your UseCustomCustomizeDialog property is set to true.");
			}
		}
		private bool m_Customizing=false;
		internal bool IsCustomizing
		{
			get {return m_Customizing;}
		}

        private void RescanCategories()
        {
            DotNetBarManager manager = this;
            if (manager.Bars.Count == 0)
                return;
            manager.Items.Clear();
            foreach (Bar bar in manager.Bars)
            {
                if (bar.LayoutType == eLayoutType.Toolbar)
                {
                    foreach (BaseItem item in bar.Items)
                        AutoCategorizeItem(item);
                }
            }
        }

        private void AutoCategorizeItem(BaseItem item)
        {
            if (item.Category != "" && item.Name != "" && !this.Items.Contains(item.Name))
                this.Items.Add(item.Copy());
            foreach (BaseItem i in item.SubItems)
                AutoCategorizeItem(i);
        }


		/// <summary>
		/// Specifies that custom customize dialog will be used. Use EnterCustomize event to show your custom dialog box.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Specifies that custom customize dialog will be used. Use EnterCustomize event to show your custom dialog box.")]
		public bool UseCustomCustomizeDialog
		{
			get
			{
				return m_UseCustomCustomizeDialog;
			}
			set
			{
				m_UseCustomCustomizeDialog=value;
			}

		}

		/// <summary>
		/// Returns the collection of items with the specified name.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <returns></returns>
		public ArrayList GetItems(string ItemName)
		{
			ArrayList list=new ArrayList(15);
			foreach(Bar bar in m_Bars)
			{
                if (bar.IsDisposed) continue;
				GetSubItemsByName(bar.ItemsContainer,ItemName,list);
			}
            if(m_GlobalContextMenuBar!=null)
                GetSubItemsByName(m_GlobalContextMenuBar.ItemsContainer, ItemName, list);
			return list;
		}

		/// <summary>
		/// Returns the collection of items with the specified name. This method search for items on all Bars, Items collection and ContextMenus collection.
		/// The order of search is as follows. All Bars from Bars collections are searched, then Items collection and then ContextMenus collection.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <param name="FullSearch">Specifies that search will be performed through all DotNetBar collections.</param>
		/// <returns></returns>
		public ArrayList GetItems(string ItemName, bool FullSearch)
		{
			if(!FullSearch)
				return GetItems(ItemName);

			ArrayList list=new ArrayList(15);
			foreach(Bar bar in m_Bars)
			{
                if (bar.IsDisposed) continue;
				GetSubItemsByName(bar.ItemsContainer,ItemName,list);
			}
			foreach(DictionaryEntry e in m_Items)
			{
				BaseItem item=(BaseItem)e.Value;
				if(item.Name==ItemName)
					list.Add(item);
				GetSubItemsByName(item,ItemName,list);
			}
			foreach(BaseItem item in m_ContextMenus)
			{
				if(item.Name==ItemName)
					list.Add(item);
				GetSubItemsByName(item,ItemName,list);
			}

			return list;
		}

        /// <summary>
        /// Returns the collection of items with the specified name and type.
        /// </summary>
        /// <param name="itemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <returns></returns>
        public ArrayList GetItems(string itemName, Type itemType)
        {
            ArrayList list = new ArrayList(15);
            foreach (Bar bar in m_Bars)
            {
                if (bar.IsDisposed) continue;
                BarFunctions.GetSubItemsByNameAndType(bar.ItemsContainer, itemName, list, itemType, false);
            }
            if (m_GlobalContextMenuBar != null && !m_GlobalContextMenuBar.IsDisposed)
                BarFunctions.GetSubItemsByNameAndType(m_GlobalContextMenuBar.ItemsContainer, itemName, list, itemType, false);
            return list;
        }

		/// <summary>
		/// Returns the collection of items with the specified name and type. This method will searchs for items on all Bars, Items collection and ContextMenus collection.
		/// The order of search is as follows. All Bars from Bars collections are searced, then Items collection and then ContextMenus collection.
		/// </summary>
        /// <param name="itemName">Item name to look for.</param>
		/// <param name="itemType">Item type to look for.</param>
        /// <param name="fullSearch">Specifies that full search (through all collections) will be performed.</param>
		/// <returns></returns>
		public ArrayList GetItems(string itemName, Type itemType, bool fullSearch)
		{
            return GetItems(itemName, itemType, fullSearch, false);
		}

        /// <summary>
        /// Returns the collection of items with the specified name and type. This method will searchs for items on all Bars, Items collection and ContextMenus collection.
        /// The order of search is as follows. All Bars from Bars collections are searced, then Items collection and then ContextMenus collection.
        /// </summary>
        /// <param name="itemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <param name="fullSearch">Specifies that full search (through all collections) will be performed.</param>
        /// <returns></returns>
        public ArrayList GetItems(string itemName, Type itemType, bool fullSearch, bool useGlobalName)
        {
            ArrayList list = new ArrayList(15);
            if (m_Bars == null || this.IsDisposed) return list;
            foreach (Bar bar in m_Bars)
            {
                if(!bar.IsDisposed)
                    BarFunctions.GetSubItemsByNameAndType(bar.ItemsContainer, itemName, list, itemType, useGlobalName);
            }

            if (m_GlobalContextMenuBar != null && !m_GlobalContextMenuBar.IsDisposed)
                BarFunctions.GetSubItemsByNameAndType(m_GlobalContextMenuBar.ItemsContainer, itemName, list, itemType, useGlobalName);

            if (fullSearch)
            {
                foreach (DictionaryEntry e in m_Items)
                {
                    BaseItem item = (BaseItem)e.Value;
                    if ((!useGlobalName && item.Name == itemName || useGlobalName && item.GlobalName == itemName) && item.GetType() == itemType)
                        list.Add(item);
                    BarFunctions.GetSubItemsByNameAndType(item, itemName, list, itemType, useGlobalName);
                }
                foreach (BaseItem item in m_ContextMenus)
                {
                    if ((!useGlobalName && item.Name == itemName || useGlobalName && item.GlobalName == itemName) && item.GetType() == itemType)
                        list.Add(item);
                    BarFunctions.GetSubItemsByNameAndType(item, itemName, list, itemType, useGlobalName);
                }
            }

            return list;
        }


		/// <summary>
		/// Returns the first item that matches specified name.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <returns></returns>
		public BaseItem GetItem(string ItemName)
		{
			foreach(Bar bar in m_Bars)
			{
                if (bar.IsDisposed) continue;
				BaseItem item=GetSubItemByName(bar.ItemsContainer,ItemName);
				if(item!=null)
					return item;
			}
            if (m_GlobalContextMenuBar != null)
                return GetSubItemByName(m_GlobalContextMenuBar.ItemsContainer, ItemName);
			return null;
		}

		/// <summary>
		/// Returns the first item that matches specified name with the option to indicate full search of all collections.
		/// The order of search is as follows. All Bars from Bars collections are searced, then Items collection and then ContextMenus collection.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <param name="FullSearch">Specifies that all collection will be searched.</param>
		/// <returns></returns>
		public BaseItem GetItem(string ItemName, bool FullSearch)
		{
			foreach(Bar bar in m_Bars)
			{
                if (bar.IsDisposed) continue;
				BaseItem item=GetSubItemByName(bar.ItemsContainer,ItemName);
				if(item!=null)
					return item;
			}
            if (m_GlobalContextMenuBar != null && !m_GlobalContextMenuBar.IsDisposed)
            {
                BaseItem item = GetSubItemByName(m_GlobalContextMenuBar.ItemsContainer, ItemName);
                if (item != null)
                    return item;
            }
			foreach(DictionaryEntry e in m_Items)
			{
				BaseItem parent=(BaseItem)e.Value;
				if(parent.Name==ItemName)
					return parent;
				BaseItem item=GetSubItemByName(parent,ItemName);
				if(item!=null)
					return item;
			}

			foreach(BaseItem parent in m_ContextMenus)
			{
				if(parent.Name==ItemName)
					return parent;
				BaseItem item=GetSubItemByName(parent,ItemName);
				if(item!=null)
					return item;
			}

			return null;
		}

		private void GetSubItemsByName(BaseItem objParent, string ItemName,ArrayList list)
		{
			if(objParent is GenericItemContainer)
			{
				GenericItemContainer cont=objParent as GenericItemContainer;
				if(cont.MoreItems!=null)
					GetSubItemsByName(cont.MoreItems,ItemName,list);
			}

			foreach(BaseItem objItem in objParent.SubItems)
			{
				if(objItem.Name==ItemName)
					list.Add(objItem);
				if(objItem.SubItems.Count>0)
					GetSubItemsByName(objItem, ItemName, list);
			}
		}

		private void GetSubItemsByNameAndType(BaseItem objParent, string ItemName,ArrayList list, Type itemType)
		{
			if(objParent is GenericItemContainer)
			{
				GenericItemContainer cont=objParent as GenericItemContainer;
				if(cont.MoreItems!=null)
					GetSubItemsByNameAndType(cont.MoreItems,ItemName,list,itemType);
			}

			if(objParent.SubItems!=null)
			{
				foreach(BaseItem objItem in objParent.SubItems)
				{
					if(objItem.GetType()==itemType && objItem.Name==ItemName)
						list.Add(objItem);
					if(objItem.SubItems.Count>0)
						GetSubItemsByNameAndType(objItem, ItemName, list, itemType);
				}
			}
		}

		private BaseItem GetSubItemByName(BaseItem objParent, string ItemName)
		{
			if(objParent is GenericItemContainer)
			{
				GenericItemContainer cont=objParent as GenericItemContainer;
				if(cont.MoreItems!=null)
				{
					BaseItem item=GetSubItemByName(cont.MoreItems,ItemName);
					if(item!=null)
						return item;
				}
			}

			foreach(BaseItem objItem in objParent.SubItems)
			{
				if(objItem.Name==ItemName)
					return objItem;
				if(objItem.SubItems.Count>0)
				{
					BaseItem item=GetSubItemByName(objItem, ItemName);
					if(item!=null)
						return item;
				}
			}
			return null;
		}

		/// <summary>
		/// Called before modal dialog is displayed using ShowModal() method.
		/// </summary>
		public void BeginModalDisplay()
		{
			if(this.IsDisposed)
				return;
			if(m_Bars==null)
				return;
			foreach(Bar bar in m_Bars)
			{
				if(bar.DockSide==eDockSide.None && bar.Parent is Form && bar.Parent.Visible)
				{
					((Form)bar.Parent).TopMost=false;
				}
			}
		}

		/// <summary>
		/// Called after modal dialog is closed.
		/// </summary>
		public void EndModalDisplay()
		{
			if(this.IsDisposed)
				return;
			if(m_Bars==null)
				return;
			foreach(Bar bar in m_Bars)
			{
				if(bar.Visible && bar.DockSide==eDockSide.None && bar.Parent is Form)
				{
					if(!((Form)bar.Parent).TopMost)
						((Form)bar.Parent).TopMost=true;
				}
			}
		}

		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void SetDesignMode(bool designmode)
		{
			foreach(Bar bar in m_Bars)
			{
				bar.SetDesignMode(designmode);
				bar.RecalcLayout();
			}
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
			m_FocusItem=objFocusItem;
			if(m_FocusItem!=null)
				m_FocusItem.OnGotFocus();
		}

		BaseItem IOwner.GetFocusItem()
		{
			return m_FocusItem;
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


		/// <summary>
		/// You must call this procedure if you are implementing custom customize dialog box after your dialog box is closed.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public void CustomizeClosing()
		{
			m_Customizing=false;

			if(m_DisabledControls==null)
				return;

			foreach(Control objCtrl in m_DisabledControls)
				objCtrl.Enabled=true;
			m_DisabledControls.Clear();
			m_DisabledControls=null;
			m_frmCustomize=null;

			if(ExitCustomize!=null)
				ExitCustomize(this,new EventArgs());

            if (m_AutoCreatedCategories)
            {
                m_AutoCreatedCategories = false;
                this.Items.Clear();
            }
		}

		void IOwner.DesignTimeContextMenu(BaseItem objItem)
		{
			if(m_frmCustomize!=null)
				m_frmCustomize.DesignTimeContextMenu(objItem);
			else if(m_UseCustomCustomizeDialog)
				this.OnCustomizeContextMenu(objItem,null);
		}
		
		void IOwner.OnParentPositionChanging()
		{
			if(m_Bars!=null)
			{
				Bar[] arr=new Bar[m_Bars.Count];
				m_Bars.CopyTo(arr);
				foreach(Bar bar in arr)
				{
					if(!bar.IsDisposed && bar.IsHandleCreated && bar.BarState==eBarState.Floating)
					{
						NativeFunctions.PostMessage(bar.Handle.ToInt32(),NativeFunctions.WM_USER+101,0,0);
						//NativeFunctions.SetWindowPos(bar.Handle,Microsoft.Win32.Interop.win.HWND_TOP,0,0,0,0,Microsoft.Win32.Interop.win.SWP_NOMOVE | Microsoft.Win32.Interop.win.SWP_NOSIZE | Microsoft.Win32.Interop.win.SWP_NOACTIVATE);
					}
				}
			}
		}

		void IOwner.StartItemDrag(BaseItem objItem)
		{
			if(CustomizeStartItemDrag!=null)
				CustomizeStartItemDrag(objItem,EventArgs.Empty);
			if(m_frmCustomize!=null)
			{
				objItem.Expanded=false;
				m_frmCustomize.DragItem=objItem;
				NativeFunctions.PostMessage(m_frmCustomize.Handle.ToInt32(),NativeFunctions.WM_USER+707,0,0);
			}
		}

		BaseItem IOwner.DragItem
		{
			get
			{
				if(m_frmCustomize!=null)
					return m_frmCustomize.DragItem;
				return null;
			}
		}

		bool IOwner.DragInProgress
		{
			get
			{
				if(m_frmCustomize!=null)
					return m_frmCustomize.DragInProgress;
				return false;
			}
		}

		internal System.Windows.Forms.Form CustomizeForm
		{
			get{ return m_frmCustomize;}
		}

		internal void OnCustomizeContextMenu(object Sender, ButtonItem Parent)
		{
			if(CustomizeContextMenu!=null)
				CustomizeContextMenu(Sender,new CustomizeContextMenuEventArgs(Parent));
		}

		/// <summary>
		/// Registers popup item with DotNetBar. Use this function carefully. The registration is required only if Popup item is created completely from code and it is not added to any DotNetBarManager collection.
		/// </summary>
		/// <param name="objPopup"></param>
		public void RegisterPopup(PopupItem objPopup)
		{
			if(m_RegisteredPopups.Contains(objPopup))
				return;
			
			if(!this.DesignMode)
			{
				if(!m_FilterInstalled && !m_UseHook)
				{
					//System.Windows.Forms.Application.AddMessageFilter(this);
					MessageHandler.RegisterMessageClient(this);
					m_FilterInstalled=true;
				}
				else if(m_UseHook && m_Hook==null)
				{
					m_Hook=new Hook(this);
				}
			}
            else if (m_Hook == null)
            {
                m_Hook = new Hook(this);
            }

			m_RegisteredPopups.Add(objPopup);
			if(objPopup.GetOwner()==null)
				objPopup.SetOwner(this);
		}

		bool IOwnerMenuSupport.RelayMouseHover()
		{
			foreach(PopupItem popup in m_RegisteredPopups)
			{
				Control ctrl=popup.PopupControl;
				if(ctrl!=null)
				{
					Point pClient=ctrl.PointToClient(Control.MousePosition);
					if(ctrl.ClientRectangle.Contains(pClient))
					{
						if(ctrl is MenuPanel)
							((MenuPanel)ctrl).InternalMouseHover();
						else if(ctrl is Bar)
							((Bar)ctrl).InternalMouseHover();
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Un-registers specified popup. See RegisterPopup for more information.
		/// </summary>
		/// <param name="objPopup"></param>
		public void UnregisterPopup(PopupItem objPopup)
		{
			if(m_RegisteredPopups.Contains(objPopup))
				m_RegisteredPopups.Remove(objPopup);
			if(m_RegisteredPopups.Count==0 && m_Hook!=null && (!m_UseHook || this.DesignMode))
			{
				m_Hook.Dispose();
				m_Hook=null;
			}
		}

        /// <summary>
		/// Gets or sets the toolbar Top dock site used by DotNetBarManager.
		/// </summary>
        [Browsable(false), DefaultValue(null)]
        public DockSite ToolbarTopDockSite
        {
            get { return m_ToolbarTopDockSite; }
            set
            {
                m_ToolbarTopDockSite = value;
                if (m_ToolbarTopDockSite != null)
                {
                    m_ToolbarTopDockSite.Dock = DockStyle.Top;
                    m_ToolbarTopDockSite.SetOwner(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the toolbar Bottom dock site used by DotNetBarManager.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DockSite ToolbarBottomDockSite
        {
            get { return m_ToolbarBottomDockSite; }
            set
            {
                m_ToolbarBottomDockSite = value;
                if (m_ToolbarBottomDockSite != null)
                {
                    m_ToolbarBottomDockSite.Dock = DockStyle.Bottom;
                    m_ToolbarBottomDockSite.SetOwner(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the toolbar Left dock site used by DotNetBarManager.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DockSite ToolbarLeftDockSite
        {
            get { return m_ToolbarLeftDockSite; }
            set
            {
                m_ToolbarLeftDockSite = value;
                if (m_ToolbarLeftDockSite != null)
                {
                    m_ToolbarLeftDockSite.Dock = DockStyle.Left;
                    m_ToolbarLeftDockSite.SetOwner(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the toolbar Right dock site used by DotNetBarManager.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DockSite ToolbarRightDockSite
        {
            get { return m_ToolbarRightDockSite; }
            set
            {
                m_ToolbarRightDockSite = value;
                if (m_ToolbarRightDockSite != null)
                {
                    m_ToolbarRightDockSite.Dock = DockStyle.Right;
                    m_ToolbarRightDockSite.SetOwner(this);
                }
            }
        }

		/// <summary>
		/// Gets or sets the Top dock site used by DotNetBarManager.
		/// </summary>
		[Browsable(false)]
		public DockSite TopDockSite
		{
			get
			{
				return m_TopDockSite;
			}
			set
			{
				m_TopDockSite=value;
				if(m_TopDockSite!=null)
				{
					m_TopDockSite.Dock=DockStyle.Top;
					m_TopDockSite.SetOwner(this);
					
					if(m_TopDockSite.Parent==null)
					{
						m_TopDockSite.ParentChanged+=new EventHandler(this.DockSiteParentChanged);
					}
					else
					{
						if(m_TopDockSite.Parent is UserControl)
							SetupParentUserControl();
						//if(!this.DesignMode) 
							BarStreamLoad();
						ProcessDelayedCommands();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the Bottom dock site used by DotNetBarManager.
		/// </summary>
		[Browsable(false)]
		public DockSite BottomDockSite
		{
			get
			{
				return m_BottomDockSite;
			}
			set
			{
				m_BottomDockSite=value;
				if(m_BottomDockSite!=null)
				{
					m_BottomDockSite.Dock=DockStyle.Bottom;
					m_BottomDockSite.SetOwner(this);

					if(m_BottomDockSite.Parent==null)
						m_BottomDockSite.ParentChanged+=new EventHandler(this.DockSiteParentChanged);
					else
					{
						//if(!this.DesignMode)
							BarStreamLoad();
						ProcessDelayedCommands();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the Left dock site used by DotNetBarManager.
		/// </summary>
		[Browsable(false)]
		public DockSite LeftDockSite
		{
			get
			{
				return m_LeftDockSite;
			}
			set
			{
				m_LeftDockSite=value;
				if(m_LeftDockSite!=null)
				{
					m_LeftDockSite.Dock=DockStyle.Left;
					m_LeftDockSite.SetOwner(this);

					if(m_LeftDockSite.Parent==null)
						m_LeftDockSite.ParentChanged+=new EventHandler(this.DockSiteParentChanged);
					else
					{
						//if(!this.DesignMode)
							BarStreamLoad();
						ProcessDelayedCommands();
					}
				}
			}
		}

		private void ProcessDelayedCommands()
		{
			if(this.DesignMode && m_TopDockSite!=null && m_BottomDockSite!=null && m_LeftDockSite!=null && m_RightDockSite!=null)
			{
				if(m_TopDockSite.Parent!=null && m_BottomDockSite.Parent!=null && m_LeftDockSite.Parent!=null && m_RightDockSite.Parent!=null)
                foreach(Bar bar in this.Bars)
					bar.ProcessDelayedCommands();
			}
		}

		/// <summary>
		/// Gets or sets the Right dock site used by DotNetBarManager.
		/// </summary>
		[Browsable(false)]
		public DockSite RightDockSite
		{
			get
			{
				return m_RightDockSite;
			}
			set
			{
				m_RightDockSite=value;
				if(m_RightDockSite!=null)
				{
					m_RightDockSite.Dock=DockStyle.Right;
					m_RightDockSite.SetOwner(this);

					if(m_RightDockSite.Parent==null)
						m_RightDockSite.ParentChanged+=new EventHandler(this.DockSiteParentChanged);
					else
					{
						//if(!this.DesignMode)
							BarStreamLoad();
						ProcessDelayedCommands();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the Fill dock site used by DotNetBarManager. Fill dock site is most commonly used as dock site
		/// for document type docking.
		/// </summary>
		[Browsable(false),DefaultValue(null)]
		public DockSite FillDockSite
		{
			get
			{
				return m_FillDockSite;
			}
			set
			{
				m_FillDockSite=value;
				if(m_FillDockSite!=null)
					m_FillDockSite.SetOwner(this);
			}
		}

		AutoHidePanel IOwnerAutoHideSupport.LeftAutoHidePanel
		{
			get
			{
				if(m_LeftAutoHidePanel==null)
				{
					if(m_LeftDockSite.Parent==null)
						return null;
					m_LeftAutoHidePanel=new AutoHidePanel();
					m_LeftAutoHidePanel.Font = SystemInformation.MenuFont.Clone() as Font;
					m_LeftAutoHidePanel.SetOwner(this);
					m_LeftAutoHidePanel.Dock=DockStyle.Left;
                    m_LeftAutoHidePanel.Width=0;
					m_LeftAutoHidePanel.Visible=false;
					m_LeftDockSite.Parent.Controls.Add(m_LeftAutoHidePanel);
					if(m_LeftDockSite.HasFixedBars)
						m_LeftDockSite.Parent.Controls.SetChildIndex(m_LeftAutoHidePanel,m_LeftDockSite.Parent.Controls.GetChildIndex(m_LeftDockSite));
					else
						m_LeftDockSite.Parent.Controls.SetChildIndex(m_LeftAutoHidePanel,m_LeftDockSite.Parent.Controls.GetChildIndex(m_LeftDockSite)+1);
					//m_LeftDockSite.Parent.Controls.SetChildIndex(m_LeftAutoHidePanel,GetMaxDockSiteIndex(false,true,true,false)+1);
				}
				return m_LeftAutoHidePanel;
			}
			set {m_LeftAutoHidePanel=value;}
		}
		bool IOwnerAutoHideSupport.HasLeftAutoHidePanel
		{
			get {return (m_LeftAutoHidePanel!=null);}
		}

		AutoHidePanel IOwnerAutoHideSupport.RightAutoHidePanel
		{
			get
			{
				if(m_RightAutoHidePanel==null)
				{
					if(m_RightDockSite.Parent==null)
						return null;
					m_RightAutoHidePanel=new AutoHidePanel();
                    //m_RightAutoHidePanel.Font = SystemInformation.MenuFont.Clone() as Font;
					m_RightAutoHidePanel.SetOwner(this);
					m_RightAutoHidePanel.Dock=DockStyle.Right;
					m_RightAutoHidePanel.Width=0;
					m_RightAutoHidePanel.Visible=false;
					m_RightDockSite.Parent.Controls.Add(m_RightAutoHidePanel);
					if(m_RightDockSite.HasFixedBars)
						m_RightDockSite.Parent.Controls.SetChildIndex(m_RightAutoHidePanel,m_RightDockSite.Parent.Controls.GetChildIndex(m_RightDockSite));
					else
						m_RightDockSite.Parent.Controls.SetChildIndex(m_RightAutoHidePanel,m_RightDockSite.Parent.Controls.GetChildIndex(m_RightDockSite)+1);
					//m_RightDockSite.Parent.Controls.SetChildIndex(m_RightAutoHidePanel,GetMaxDockSiteIndex(false,true,false,true)+1);
				}
				return m_RightAutoHidePanel;
			}
			set {m_RightAutoHidePanel=value;}
		}
		bool IOwnerAutoHideSupport.HasRightAutoHidePanel
		{
			get {return (m_RightAutoHidePanel!=null);}
		}

		AutoHidePanel IOwnerAutoHideSupport.TopAutoHidePanel
		{
			get
			{
				if(m_TopAutoHidePanel==null)
				{
					if(m_TopDockSite.Parent==null)
						return null;
					m_TopAutoHidePanel=new AutoHidePanel();
                    m_TopAutoHidePanel.Font = SystemInformation.MenuFont.Clone() as Font;
					m_TopAutoHidePanel.SetOwner(this);
					m_TopAutoHidePanel.Dock=DockStyle.Top;
					m_TopAutoHidePanel.Width=0;
					m_TopAutoHidePanel.Visible=false;
					m_TopDockSite.Parent.Controls.Add(m_TopAutoHidePanel);
					
					if(m_TopDockSite.HasFixedBars)
						m_TopDockSite.Parent.Controls.SetChildIndex(m_TopAutoHidePanel,m_TopDockSite.Parent.Controls.GetChildIndex(m_TopDockSite));
					else
						m_TopDockSite.Parent.Controls.SetChildIndex(m_TopAutoHidePanel,m_TopDockSite.Parent.Controls.GetChildIndex(m_TopDockSite)+1);
				}
				return m_TopAutoHidePanel;
			}
			set {m_TopAutoHidePanel=value;}
		}
		bool IOwnerAutoHideSupport.HasTopAutoHidePanel
		{
			get {return (m_TopAutoHidePanel!=null);}
		}

		AutoHidePanel IOwnerAutoHideSupport.BottomAutoHidePanel
		{
			get
			{
				if(m_BottomAutoHidePanel==null)
				{
					if(m_BottomDockSite.Parent==null)
						return null;
					m_BottomAutoHidePanel=new AutoHidePanel();
                    m_BottomAutoHidePanel.Font = SystemInformation.MenuFont.Clone() as Font;
					m_BottomAutoHidePanel.SetOwner(this);
					m_BottomAutoHidePanel.Dock=DockStyle.Bottom;
					m_BottomAutoHidePanel.Width=0;
					m_BottomAutoHidePanel.Visible=false;
					m_BottomDockSite.Parent.Controls.Add(m_BottomAutoHidePanel);
					if(m_BottomDockSite.HasFixedBars)
						m_BottomDockSite.Parent.Controls.SetChildIndex(m_BottomAutoHidePanel,m_BottomDockSite.Parent.Controls.GetChildIndex(m_BottomDockSite));
					else
						m_BottomDockSite.Parent.Controls.SetChildIndex(m_BottomAutoHidePanel,m_BottomDockSite.Parent.Controls.GetChildIndex(m_BottomDockSite)+1);
					//m_BottomDockSite.Parent.Controls.SetChildIndex(m_BottomAutoHidePanel,GetMaxDockSiteIndex(false,true,true,true)+1);
				}
				return m_BottomAutoHidePanel;
			}
			set {m_BottomAutoHidePanel=value;}
		}
		bool IOwnerAutoHideSupport.HasBottomAutoHidePanel
		{
			get {return (m_BottomAutoHidePanel!=null);}
		}

		private void DestroyAutoHidePanels()
		{
			if(m_LeftAutoHidePanel!=null)
			{
				if(m_LeftAutoHidePanel.Parent!=null)
					m_LeftAutoHidePanel.Parent.Controls.Remove(m_LeftAutoHidePanel);
				m_LeftAutoHidePanel.Dispose();
				m_LeftAutoHidePanel=null;
			}
			if(m_RightAutoHidePanel!=null)
			{
				if(m_RightAutoHidePanel.Parent!=null)
					m_RightAutoHidePanel.Parent.Controls.Remove(m_RightAutoHidePanel);
				m_RightAutoHidePanel.Dispose();
				m_RightAutoHidePanel=null;
			}
			if(m_TopAutoHidePanel!=null)
			{
				if(m_TopAutoHidePanel.Parent!=null)
					m_TopAutoHidePanel.Parent.Controls.Remove(m_TopAutoHidePanel);
				m_TopAutoHidePanel.Dispose();
				m_TopAutoHidePanel=null;
			}
			if(m_BottomAutoHidePanel!=null)
			{
				if(m_BottomAutoHidePanel.Parent!=null)
					m_BottomAutoHidePanel.Parent.Controls.Remove(m_BottomAutoHidePanel);
				m_BottomAutoHidePanel.Dispose();
				m_BottomAutoHidePanel=null;
			}

		}

		/// <summary>
		/// Specifies whether bars are drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[Browsable(true),DevCoBrowsable(false),DefaultValue(false),Category("Appearance"),Description("Specifies whether new bars created are drawn using Themes when running on OS that supports themes like Windows XP.")]
		public virtual bool ThemeAware
		{
			get
			{
				return m_ThemeAware;
			}
			set
			{
				foreach(Bar bar in m_Bars)
					bar.ThemeAware=value;
				m_ThemeAware=value;
				if(this.DesignMode)
				{
					if(m_TopDockSite!=null)
						m_TopDockSite.RecalcLayout();
					if(m_BottomDockSite!=null)
						m_BottomDockSite.RecalcLayout();
					if(m_LeftDockSite!=null)
						m_LeftDockSite.RecalcLayout();
					if(m_RightDockSite!=null)
						m_RightDockSite.RecalcLayout();
				}
				else
				{
					if(m_TopDockSite!=null && m_TopDockSite.Controls.Count>0)
						m_TopDockSite.NeedsLayout=true;
					if(m_BottomDockSite!=null && m_BottomDockSite.Controls.Count>0)
						m_BottomDockSite.NeedsLayout=true;
					if(m_LeftDockSite!=null && m_LeftDockSite.Controls.Count>0)
						m_LeftDockSite.NeedsLayout=true;
					if(m_RightDockSite!=null && m_RightDockSite.Controls.Count>0)
						m_RightDockSite.NeedsLayout=true;
				}
			}
		}

		private void DockSiteParentChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.Control c=sender as System.Windows.Forms.Control;
			if(c!=null)
			{
				if(c==m_TopDockSite && c.Parent is UserControl)
					SetupParentUserControl();
				c.ParentChanged-=new EventHandler(DockSiteParentChanged);
				if((c.Parent==null || c.Parent.FindForm()==null) && !this.DesignMode && c.Parent is UserControl)
					return;
				// Try to load bar stream
				//if(!this.DesignMode)
					BarStreamLoad();
				ProcessDelayedCommands();
			}
		}

		private void SetupParentUserControl()
		{
			// VERY IMPORTANT that this is not done when UserControl is in design mode
			// The whole designer was reporting exception when UserControl with DotNetBar is opened in design-mode, switched to code, code changed, switched back to design mode.
			if((m_TopDockSite==null || !(m_TopDockSite.Parent is UserControl)) && m_ParentUserControl==null || this.DesignMode)
			//if(m_TopDockSite==null || !(m_TopDockSite.Parent is UserControl) || this.DesignMode)
				return;
			Control parent=null;
			if(m_TopDockSite!=null)
				parent=m_TopDockSite.Parent as UserControl;
			else
				parent=m_ParentUserControl;
            Form form = parent.FindForm();
			if(form!=null)
			{
				// Easy case...
				this.ParentForm=form;
			}
			else
			{
				parent.ParentChanged+=new EventHandler(ParentUserControlParentChanged);
			}
		}

		private void ParentUserControlParentChanged(object sender, EventArgs e)
		{
			Control parent=null;
			if(m_TopDockSite!=null)
				parent=m_TopDockSite.Parent as UserControl;
			else
				parent=m_ParentUserControl;

			Control ctrl=sender as Control;

			if(ctrl!=null)
				ctrl.ParentChanged-=new EventHandler(ParentUserControlParentChanged);

            Form parentForm = parent.FindForm();
            if (parentForm != null)
			{
				// Easy case...
                this.ParentForm = parentForm;
			}
			else if(parent.Parent!=null)
			{
				while(ctrl.Parent!=null)
					ctrl=ctrl.Parent;
                if (ctrl is PopupContainerControl)
                {
                    PopupContainerControl pc = ctrl as PopupContainerControl;
                    if (pc.ParentItem != null && pc.ParentItem.ContainerControl is Control)
                    {
                        Control c = pc.ParentItem.ContainerControl as Control;
                        Form form = c.FindForm();
                        if (form != null)
                        {
                            this.ParentForm = form;
                            if (!m_DefinitionLoaded)
                                BarStreamLoad(true);
                            return;
                        }
                    }
                }

				// Walk up in the chain till we manage to get to the form
				ctrl.ParentChanged+=new EventHandler(ParentUserControlParentChanged);
			}
		}

		/// <summary>
		/// Sets the style of all items in DotNetBar Manager.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Color Scheme"),System.ComponentModel.Description("Specifies the default visual style of the Bars.")]
		public eDotNetBarStyle Style
		{
			set
			{
				foreach(Bar bar in m_Bars)
				{
					TypeDescriptor.GetProperties(bar)["Style"].SetValue(bar,value);
				}
				foreach(BaseItem item in m_ContextMenus)
					item.Style=value;
				foreach(DictionaryEntry e in m_Items)
					((BaseItem)e.Value).Style=value;
				if(m_Style!=value)
					m_ColorScheme.SwitchStyle(value);
				m_Style=value;
			}
			get
			{
				return m_Style;
			}
		}

		/// <summary>
		/// Gets or sets Color scheme for all bars. Note that you need to set UseGlobalColorScheme to true to indicate
		/// that this ColorScheme object will be used on all bars managed by this instance of DotNetBarManager.
		/// </summary>
		[System.ComponentModel.Editor(typeof(ColorSchemeVSEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Color Scheme"),System.ComponentModel.Description("Indicates Color Scheme for all bars."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DevComponents.DotNetBar.ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				if(value==null)
					throw new ArgumentException("NULL is not a valid value for this property.");
				m_ColorScheme=value;
			}
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
            foreach (Bar bar in m_Bars)
            {
                bar.Invalidate();
            }
        }

		/// <summary>
		/// Gets or sets whether ColorScheme object on DotNetBarManager is used as a default ColorScheme for all bars managed by DotNetBarManager.
		/// Default value is false which indicates that ColorScheme on each Bar is used.
		/// When set to true each bar will use the ColorScheme settings from DotNetBarManager and it will ignore any
		/// setting on the Bar.ColorScheme object.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Color Scheme"),Description("Indicates whether ColorScheme object on DotNetBarManager is used as a default ColorScheme for all bars managed by DotNetBarManager."),DefaultValue(false)]
		public bool UseGlobalColorScheme
		{
			get {return m_UseGlobalColorScheme;}
			set {m_UseGlobalColorScheme=value;}
		}

		// Following Properties reflect the end-user settings in Customize Dialog
		/// <summary>
		/// Indicates whether the Personalized menu setting is ignored and full menus are always shown.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Indicates whether the Personalized menu setting is ignored and full menus are always shown.")]
		public bool AlwaysShowFullMenus
		{
			get
			{
				return m_AlwaysShowFullMenus;
			}
			set
			{
				m_AlwaysShowFullMenus=value;
			}
		}

		/// <summary>
		/// Gets or sets whether accelerator letters for menu or toolbar commands are underlined regardless of
		/// current Windows settings. Accelerator keys allow easy access to menu commands by using
		/// Alt + choosen key (letter). Default value is false which indicates that system setting is used
		/// to determine whether accelerator letters are underlined. Setting this property to true
		/// will always display accelerator letter underlined.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Run-time Behavior"),Description("Indicates whether accelerator letters for menu or toolbar commands are underlined regardless of current Windows settings.")]
		public bool AlwaysDisplayKeyAccelerators
		{
			get {return m_AlwaysDisplayKeyAccelerators;}
			set
			{
				m_AlwaysDisplayKeyAccelerators=value;
				Bar bar=this.GetMenuBar();
				if(bar!=null)
					bar.Refresh();
			}
		}

		/// <summary>
		/// Returns whether theme support is enabled on the OS that supports themes like Windows XP.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool IsThemeActive
		{
			get
			{
				if(!BarFunctions.ThemedOS) // System.Windows.Forms.OSFeature.Feature.IsPresent(System.Windows.Forms.OSFeature.Themes))
					return false;
				return Themes.ThemesActive;
			}
		}
		
		// Following Properties reflect the end-user settings in Customize Dialog
		/// <summary>
		/// Indicates whether the CustomizeItem (allows toolbar customization) is added for new Bars end users are creating.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(true),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Indicates whether the CustomizeItem (allows toolbar customization) is added for new Bars end users are creating.")]
		public bool AllowUserBarCustomize
		{
			get
			{
				return m_AllowUserBarCustomize;
			}
			set
			{
				m_AllowUserBarCustomize=value;
			}
		}

		/// <summary>
		/// Gets or sets whether DotNetBar ignores the F10 key which when pressed sets the focus to menu bar
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Indicates whether DotNetBar ignores the F10 key which when pressed sets the focus to menu bar.")]		
		public bool IgnoreF10Key
		{
			get {return m_IgnoreF10Key;}
			set {m_IgnoreF10Key=value;}
		}

		/// <summary>
		/// Indicates whether the items that are not recenly used are shown after mouse hovers over the expand button.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(true),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Indicates whether the items that are not most recenly used are shown after mouse hovers over the expand button.")]
		public bool ShowFullMenusOnHover
		{
			get
			{
				return m_ShowFullMenusOnHover;
			}
			set
			{
				m_ShowFullMenusOnHover=value;
			}
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

		/// <summary>
		/// Specifies the pop-up animation style.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(ePopupAnimation.SystemDefault),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Specifies the pop-up animation style.")]
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

		/// <summary>
		/// Specifies whether the MDI system buttons are displayed in menu bar when MDI Child window is maximized.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(true),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Specifies whether the MDI system buttons are displayed in menu bar when MDI Child window is maximized.")]
		public bool MdiSystemItemVisible
		{
			get
			{
				return m_MdiSystemItemVisible;
			}
			set
			{
				m_MdiSystemItemVisible=value;
				if(!this.DesignMode)
				{
					Bar bar=GetMenuBar();
					if(bar==null)
						return;
					if(m_MdiSystemItemVisible)
					{
						if(m_ParentForm!=null && m_ParentForm.IsMdiContainer && m_ParentForm.ActiveMdiChild!=null && m_ParentForm.ActiveMdiChild.WindowState==FormWindowState.Maximized)
						{
							bar.ShowMDIChildSystemItems(m_ParentForm.ActiveMdiChild,true);
							m_MdiChildMaximized=true;
						}
					}
					else
						bar.ClearMDIChildSystemItems(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets whether MDI Child form System Menu is hidden. System menu is displayed in MDI form menu area when form is maximized. Default value is false.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Run-time Behavior"),Description("Indicates whether MDI Child form System Menu is hidden. Default value is false.")]
		public bool HideMdiSystemMenu
		{
			get {return m_HideMdiSystemMenu;}
			set {m_HideMdiSystemMenu=value;}
		}


		System.Windows.Forms.MdiClient IOwner.GetMdiClient(System.Windows.Forms.Form MdiForm)
		{
			return BarFunctions.GetMdiClient(MdiForm);
		}

        /// <summary>
        /// Informs the DotNetBarManager that Mdi Child for has been activated. Calling this method is needed only under special
        /// conditions where MDI child system items do not show.
        /// </summary>
        public void MdiChildActivated()
        {
            OnMdiChildActivate(this.ParentForm, new EventArgs());
        }

        private Timer _DelayCheckTimer = null;
        private void DelayCheckMaximizedState()
        {
            if (_DelayCheckTimer == null)
            {
                _DelayCheckTimer = new Timer();
                _DelayCheckTimer.Interval = 200;
                _DelayCheckTimer.Tick += new EventHandler(DelayCheckTimerTick);
            }
            _DelayCheckTimer.Start();
        }

        private void DelayCheckTimerTick(object sender, EventArgs e)
        {
            Timer timer = _DelayCheckTimer;
            _DelayCheckTimer = null;
            timer.Stop();
            timer.Tick -= new EventHandler(DelayCheckTimerTick);
            timer.Dispose();
            if (m_ActiveMdiChild != null && m_ActiveMdiChild.WindowState == FormWindowState.Maximized)
                MdiChildActivated();
        }

        private void OnMdiChildActivate(object sender,System.EventArgs e)
		{
			if(m_ActiveMdiChild!=null)
			{
				m_ActiveMdiChild.Resize-=new EventHandler(this.OnMdiChildResize);
				m_ActiveMdiChild.VisibleChanged-=new EventHandler(this.OnMdiChildVisibleChanged);
			}
			m_ActiveMdiChild=null;

			if(this.ParentForm.ActiveMdiChild!=null)
			{
				if(GetMenuBar()!=null)
				{
					m_ActiveMdiChild=this.ParentForm.ActiveMdiChild;
					m_ActiveMdiChild.Resize+=new EventHandler(this.OnMdiChildResize);
					m_ActiveMdiChild.VisibleChanged+=new EventHandler(this.OnMdiChildVisibleChanged);
                    if (m_ActiveMdiChild.WindowState == System.Windows.Forms.FormWindowState.Maximized || m_MdiChildMaximized)
                    {
                        this.OnMdiChildResize(m_ActiveMdiChild, null);
                    }
                    else
                        DelayCheckMaximizedState();
				}
			}
			else if(m_MdiChildMaximized)
			{
				DevComponents.DotNetBar.Bar bar=GetMenuBar();
				if(bar!=null)
					bar.ClearMDIChildSystemItems(true);
			}
		}

		private void OnMdiChildVisibleChanged(object sender, System.EventArgs e)
		{
			if(m_ActiveMdiChild!=null && !m_ActiveMdiChild.Visible && m_MdiChildMaximized)
			{
				Bar bar=GetMenuBar();
				if(bar!=null)
					bar.ClearMDIChildSystemItems(true);
				m_MdiChildMaximized=false;
			}
		}
		private void OnMdiChildResize(object sender, System.EventArgs e)
		{
			DevComponents.DotNetBar.Bar bar=GetMenuBar();

			bool bRecalcLayout=false;
			
			if(m_ActiveMdiChild!=null && m_ActiveMdiChild.WindowState!=FormWindowState.Maximized)
			{
				if(m_MdiChildMaximized)
				{
					if(m_MdiSystemItemVisible)
					{
						if(e==null)
						{
							// Call from MDI Child Activate
							m_MdiChildMaximized=false;
							return;
						}
						if(bar!=null)
							bar.ClearMDIChildSystemItems(false);
						bRecalcLayout=true;
					}
				}
				m_MdiChildMaximized=false;
				if(bar!=null)
					bar.Refresh();
				return;
			}

			if(!m_MdiChildMaximized)
			{
				if(m_MdiSystemItemVisible)
				{
					if(bar==null)
						return;

					bar.ShowMDIChildSystemItems(m_ActiveMdiChild,false);
					bRecalcLayout=true;
				}
				m_MdiChildMaximized=true;
			}

			if(bar!=null)
			{
				if(bRecalcLayout) bar.RecalcLayout();
			}
		}

		bool IOwner.DesignMode
		{
			get {return this.DesignMode;}
		}

		Form IOwner.ActiveMdiChild
		{
			get
			{
				return m_ActiveMdiChild;
			}
		}

		private DevComponents.DotNetBar.Bar GetMenuBar()
		{
			if(m_Bars==null)
				return null;
			try
			{
				foreach(DevComponents.DotNetBar.Bar bar in m_Bars)
					if(bar.MenuBar && bar.Visible)
						return bar;
			}
			catch(Exception){}

			return null;
		}

		private void OnMdiSetMenu(object sender, System.EventArgs e)
		{
            MDIClientMsgHandler m=sender as MDIClientMsgHandler;
			if(this.GetMenuBar()!=null || m_HideMdiSystemMenu)
				m.EatMessage=true;
		}

		internal Bar FocusedBar
		{
			get {return m_FocusedBar;}
			set {m_FocusedBar=value;}
		}

		bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			if(!m_ParentActivated || this.DesignMode)
				return false;

            if (m_ParentForm != null && m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null && !m_ParentForm.MdiParent.Focused)
                return false;

			DevComponents.DotNetBar.Bar bar=GetMenuBar();
			if(bar!=null && bar.ItemsContainer!=null && !bar.ItemsContainer.DesignMode)
			{
				GenericItemContainer cont=bar.ItemsContainer;
				if(cont==null)
					return false;
				if(wParam.ToInt32()==18 || (wParam.ToInt32()==121 && !m_IgnoreF10Key && (System.Windows.Forms.Control.ModifierKeys==System.Windows.Forms.Keys.None || System.Windows.Forms.Control.ModifierKeys==System.Windows.Forms.Keys.Alt)))
				{
					if(cont.ExpandedItem()!=null && (bar.Focused || bar.MenuFocus))
					{
						bar.ReleaseFocus();
						bar.MenuFocus=false;
						m_IgnoreSysKeyUp=true;
						return true;
					}
				}
				else
				{
					// Check Shortcuts
					if(System.Windows.Forms.Control.ModifierKeys!=Keys.None || wParam.ToInt32()>=(int)eShortcut.F1 && wParam.ToInt32()<=(int)eShortcut.F12)
					{
						int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
						if(ProcessShortcut((eShortcut)i))
							return true;
					}
					m_IgnoreSysKeyUp=true;
					if(wParam.ToInt32()>=27 && wParam.ToInt32()<=111) // VK_ESC - VK_DIVIDE range
					{
						int key=(int)NativeFunctions.MapVirtualKey((uint)wParam,2);
						if(key==0)
							key=wParam.ToInt32();
						if(cont.SysKeyDown(key))
							return true;
					}
					//return true;
				}
			}
			else if(bar==null && !this.DesignMode)
			{
				// Check Shortcuts
				if(System.Windows.Forms.Control.ModifierKeys!=Keys.None || wParam.ToInt32()>=(int)eShortcut.F1 && wParam.ToInt32()<=(int)eShortcut.F12)
				{
					int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
					if(ProcessShortcut((eShortcut)i))
						return true;
				}
			}
			
			if((System.Windows.Forms.Control.ModifierKeys & Keys.Alt)==Keys.Alt && wParam.ToInt32()>0x1B && m_Bars!=null)
			{
				m_IgnoreSysKeyUp=true;
				int key=0;
				if((Control.ModifierKeys & Keys.Shift)==Keys.Shift)
				{
					try
					{
						byte[] keyState=new byte[256];
						if(NativeFunctions.GetKeyboardState(keyState))
						{
							byte[] chars=new byte[2];
							if(NativeFunctions.ToAscii((uint)wParam,0,keyState,chars,0)!=0)
							{
								key=chars[0];
							}
						}
					}
					catch(Exception)
					{
						key=0;
					}
				}
				
				if(key==0)
					key=(int)NativeFunctions.MapVirtualKey((uint)wParam,2);

				if(key!=0)
				{
					foreach(Bar b in m_Bars)
					{
						if(b.ItemsContainer!=null && !b.ItemsContainer.DesignMode && b.Enabled && b.Visible)
						{
							if(b.ItemsContainer.SysKeyDown(key))
								return true;
						}
					}
				}
			}
			return false;
		}

		bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			if(!m_ParentActivated || this.DesignMode)
				return false;
            if (m_ParentForm != null && m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null && !m_ParentForm.MdiParent.Focused)
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
				foreach(Bar b in this.Bars)
				{
					if(b.Visible) b.HideAllToolTips();
				}
				if(wParam.ToInt32()==18 || wParam.ToInt32()==121 && !m_IgnoreF10Key)
				{
					DevComponents.DotNetBar.Bar bar=GetMenuBar();
					if(bar!=null && !bar.ItemsContainer.DesignMode)
					{
//						if(bar.Focused)
//							bar.ReleaseFocus();
//						else
//							bar.SetSystemFocus();
						if(bar.MenuFocus)
							bar.MenuFocus=false;
						else
							bar.MenuFocus=true;
						return true;
					}
				}
			}
			return false;
		}

		bool IMessageHandlerClient.IsModal
		{
			get
			{
                if (m_ParentForm != null && m_ParentActivated)
					return m_ParentForm.Modal;
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
					return true && !designMode;
				}
			}
			
			if(this.FocusedBar!=null && !designMode)
			{
				bool bPassToMenu=true;
				Control ctrl2=Control.FromChildHandle(hWnd);	
				if(ctrl2!=null)
				{
					while(ctrl2.Parent!=null)
						ctrl2=ctrl2.Parent;
					if((ctrl2 is MenuPanel || ctrl2 is Bar || ctrl2 is PopupContainer || ctrl2 is PopupContainerControl) && ctrl2.Handle!=hWnd)
						bPassToMenu=false;
				}

				if(bPassToMenu)
				{
					Keys key=(Keys)NativeFunctions.MapVirtualKey((uint)wParam,2);
					if(key==Keys.None)
						key=(Keys)wParam.ToInt32();
					this.FocusedBar.ExKeyDown(new KeyEventArgs(key));
					return true;
				}
			}
			else if(wParam.ToInt32()==27) // Escape key
			{
				foreach(Bar bar in this.Bars)
				{
					if(bar.IsBarMoving)
					{
						bar.OnEscapeKey();
						break;
					}
				}
			}

			if(!m_ParentActivated && !(Form.ActiveForm is FloatingContainer) || designMode)
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
			if(m_AutoDispatchShortcuts!=null && m_AutoDispatchShortcuts.Contains(key))
				eat=false;
			return !m_DispatchShortcuts && eat; // True will eat the key, false will pass it through

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
//				
//				if(m_AutoDispatchShortcuts!=null && m_AutoDispatchShortcuts.Contains(key))
//					eat=false;
//				return !m_DispatchShortcuts && eat; // True will eat the key, false will pass it through
//			}
//			return false;
		}

		bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
            if (m_RegisteredPopups == null || this.DesignMode)
                return false;

			if(m_RegisteredPopups.Count==0)
			{
				Bar menuBar=this.GetMenuBar();
				if(menuBar!=null && menuBar.MenuFocus)
				{
					if(hWnd!=menuBar.Handle)
						menuBar.MenuFocus=false;
				}
				return false;
			}

			//foreach(PopupItem objPopup in m_RegisteredPopups)
			for(int i=m_RegisteredPopups.Count-1;i>=0;i--)
			{
				PopupItem objPopup=m_RegisteredPopups[i] as PopupItem;
				System.Windows.Forms.Control objCtrl=objPopup.ContainerControl as System.Windows.Forms.Control;
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
                        s=s.ToLower();
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
					if(objPopup.Parent!=null && !this.DesignMode)
					{
						/*if(objPopup.Expanded)
						{
							objPopup.Parent.AutoExpand=false;
						}
						else
						{
							objPopup.Parent.AutoExpand=true;
						}*/
					}
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
				bool eatMessage=true;
				// This passes all messages to the registered popups and no mouse move message to anybody else
				foreach(BaseItem item in m_RegisteredPopups)
				{
					Control ctrl=((PopupItem)item).PopupControl;
					Control ctrlParent=null;
					if(item.Parent!=null)
						ctrlParent=item.Parent.ContainerControl as Control;

					Control c=Control.FromChildHandle(hWnd);
					if(c is MenuPanel && c.Parent!=null)
						c=c.Parent;
					else if(c!=null)
					{
						Control topParent=c;
						while(topParent.Parent!=null)
							topParent=topParent.Parent;
						if(topParent is Bar || topParent is PopupContainerControl || topParent is PopupContainer)
							c=topParent;
					}
					if(ctrl!=null && c!=null && ctrl!=c && !item.IsAnyOnHandle(c.Handle.ToInt32()) && (ctrlParent==null || ctrlParent!=null && ctrlParent!=c))
						eatMessage=eatMessage && true;
					else
					{
						eatMessage=false;
						break;
					}
				}
				return eatMessage;
			}
			return false;
		}

		void IOwnerBarSupport.BarContextMenu(Control bar, MouseEventArgs e)
		{
			if(!ShowCustomizeContextMenu || m_frmCustomize!=null || this.DesignMode)
				return;

			// Create popup menu that lets users hide/show bars...
			m_ContextMenu=new ButtonItem("sys_customizecontextnmenu");
			ButtonItem btn;
			eDotNetBarStyle style=eDotNetBarStyle.OfficeXP;
			if(m_Bars.Count>0)
				style=m_Bars[0].Style;
			m_ContextMenu.Style=style;
			foreach(Bar b in m_Bars)
			{
                if (b.LayoutType == eLayoutType.DockContainer && b.Items.Count == 0) continue;
				// TODO: Menu Merge implementation
				//if(b.CanHide && (!m_ParentForm.IsMdiChild || !b.MergeEnabled ))
				if(b.CanHide && (b.LayoutType==eLayoutType.DockContainer && b.CanCustomize || b.LayoutType!=eLayoutType.DockContainer) && b.Text!="")
				{
					btn=new ButtonItem();
					btn.Text=b.Text;
					btn.Checked=b.Visible || b.AutoHide;
					btn.SetSystemItem(true);
					btn.Tag=b;
					btn.Click+=new System.EventHandler(this.CustomizeItemClick);
					m_ContextMenu.SubItems.Add(btn);
				}
			}
			btn=new ButtonItem("customize");
			if(m_ContextMenu.SubItems.Count>0)
				btn.BeginGroup=true;
			using(LocalizationManager lm=new LocalizationManager(this))
			{
				btn.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeItemCustomize);
			}
			btn.SetSystemItem(true);
			btn.Style=style;
			btn.Click+=new System.EventHandler(this.CustomizeItemClick);
			m_ContextMenu.SubItems.Add(btn);
            OnCustomizeContextMenu(this, (ButtonItem)m_ContextMenu);
			this.RegisterPopup(m_ContextMenu);
			m_ContextMenu.PopupMenu(bar.PointToScreen(new Point(e.X,e.Y)));
		}

		private void CustomizeItemClick(object sender, System.EventArgs e)
		{
			BaseItem objItem=sender as BaseItem;
			m_ContextMenu.Expanded=false;
			if(objItem.Name=="customize")
			{
				this.Customize();
			}
			else
			{
				Bar bar=objItem.Tag as Bar;
				if(bar.AutoHide)
					bar.AutoHide=false;
                if (!bar.Visible && bar.LayoutType == eLayoutType.DockContainer && bar.VisibleItemCount == 0 && bar.Items.Count > 0)
                    bar.Items[0].Visible = true;
				bar.Visible=!bar.Visible;
				//if(!bar.Visible)
				bar.RecalcLayout();
				bar.InvokeUserVisibleChanged();
			}
			m_ContextMenu.Dispose();
			m_ContextMenu=null;
		}

		/// <summary>
		/// Resets all usage data collected by DotNetBar in relation to the Personalized menus.
		/// </summary>
		public void ResetUsageData()
		{
			foreach(Bar bar in m_Bars)
			{
				foreach(BaseItem item in bar.Items)
				{
					IPersonalizedMenuItem ipm=item as IPersonalizedMenuItem;
					if(ipm!=null && ipm.MenuVisibility==eMenuVisibility.VisibleIfRecentlyUsed)
						ipm.RecentlyUsed=false;
					if(item.SubItems.Count>0)
						ResetItemUsageData(item);
				}
			}
		}

		private void ResetItemUsageData(BaseItem item)
		{
			foreach(BaseItem child in item.SubItems)
			{
				IPersonalizedMenuItem ipm=child as IPersonalizedMenuItem;
				if(ipm!=null && ipm.MenuVisibility==eMenuVisibility.VisibleIfRecentlyUsed)
					ipm.RecentlyUsed=false;
				if(child.SubItems.Count>0)
					ResetItemUsageData(child);
			}
				
		}

		/// <summary>
		/// Gets or sets whether customize context menu is shown on all bars or dock sites.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(true),System.ComponentModel.Category("Run-time Behavior"),System.ComponentModel.Description("Gets or sets whether customize context menu is shown on all bars or dock sites.")]
		public bool ShowCustomizeContextMenu
		{
			get
			{
				return m_ShowCustomizeContextMenu;
			}
			set
			{
				m_ShowCustomizeContextMenu=value;
			}
		}

		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),EditorBrowsable(EditorBrowsableState.Never)]
		public bool PersonalizedAllVisible
		{
			get
			{
				return m_PersonalizedAllVisible;
			}
			set
			{
				m_PersonalizedAllVisible=value;
			}
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

		void IOwnerItemEvents.InvokeItemTextChanged(BaseItem item, EventArgs e)
		{
			if(ItemTextChanged!=null)
				ItemTextChanged(item,e);
		}

		void IOwner.InvokeResetDefinition(BaseItem item,EventArgs e)
		{
			if(ResetDefinition!=null)
				ResetDefinition(item,e);
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
			#if TRIAL
			iCount++;
			#endif
		}

		void IOwnerMenuSupport.InvokePopupClose(PopupItem item,EventArgs e)
		{
			if(PopupClose!=null)
				PopupClose(item,e);
			#if TRIAL
			if(iCount>iMaxCount || iMaxCount<=0)
			{
				iCount=0;
				RemindForm f=new RemindForm();
				f.ShowDialog();
			}
			#endif
		}

		void IOwnerMenuSupport.InvokePopupShowing(PopupItem item,EventArgs e)
		{
			if(PopupShowing!=null)
				PopupShowing(item,e);
		}

		void IOwnerItemEvents.InvokeExpandedChange(BaseItem item,EventArgs e)
		{
			if(ExpandedChange!=null)
				ExpandedChange(item,e);
		}

		void IOwnerBarSupport.InvokeBarDock(Bar bar,EventArgs e)
		{
			if(BarDock!=null)
				BarDock(bar,e);
		}

		void IOwnerBarSupport.InvokeBarUndock(Bar bar,EventArgs e)
		{
			if(BarUndock!=null)
				BarUndock(bar,e);
		}

		void IOwnerBarSupport.InvokeBeforeDockTabDisplay(BaseItem item,EventArgs e)
		{
			if(BeforeDockTabDisplay!=null)
				BeforeDockTabDisplay(item,e);
		}

		void IOwnerItemEvents.InvokeMouseDown(BaseItem item, MouseEventArgs e)
		{
			if(MouseDown!=null)
				MouseDown(item,e);
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

		void IOwnerItemEvents.InvokeMouseEnter(BaseItem item,EventArgs e)
		{
			if(MouseEnter!=null)
				MouseEnter(item,e);
		}

		void IOwnerItemEvents.InvokeMouseLeave(BaseItem item,EventArgs e)
		{
			if(MouseLeave!=null)
				MouseLeave(item,e);
		}

		void IOwnerItemEvents.InvokeMouseHover(BaseItem item,EventArgs e)
		{
			if(MouseHover!=null)
				MouseHover(item,e);
		}

//		void IOwnerItemEvents.InvokeItemDisplayedChanged(BaseItem item,EventArgs e)
//		{
//			if(ItemDisplayedChanged!=null)
//				ItemDisplayedChanged(item,e);
//		}

		void IOwnerItemEvents.InvokeMouseMove(BaseItem item, MouseEventArgs e)
		{
			if(MouseMove!=null)
				MouseMove(item,e);
		}

		private void ClickTimerTick(object sender, EventArgs e)
		{
			if(m_ClickRepeatItem!=null)
				m_ClickRepeatItem.RaiseClick();
			else
				m_ClickTimer.Stop();
		}

		void IOwnerItemEvents.InvokeMouseUp(BaseItem item, MouseEventArgs e)
		{
			if(MouseUp!=null)
				MouseUp(item,e);
			if(m_ClickTimer!=null && m_ClickTimer.Enabled)
			{
				m_ClickTimer.Stop();
				m_ClickTimer.Enabled=false;
			}
		}

		void IOwnerItemEvents.InvokeGotFocus(BaseItem item,EventArgs e)
		{
			if(GotFocus!=null)
				GotFocus(item,e);
		}

		void IOwnerItemEvents.InvokeLostFocus(BaseItem item,EventArgs e)
		{
			if(LostFocus!=null)
				LostFocus(item,e);
		}

		void IOwner.InvokeUserCustomize(object sender,EventArgs e)
		{
			if(UserCustomize!=null)
				UserCustomize(sender,e);
		}

		void IOwner.InvokeEndUserCustomize(object sender,EndUserCustomizeEventArgs e)
		{
			if(EndUserCustomize!=null)
				EndUserCustomize(sender,e);
		}

		void IOwner.InvokeDefinitionLoaded(object sender,EventArgs e)
		{
			if(DefinitionLoaded!=null)
				DefinitionLoaded(sender,e);
		}

		void IOwnerItemEvents.InvokeItemRemoved(BaseItem item, BaseItem parent)
		{
			if(ItemRemoved!=null)
			{
				ItemRemoved(item,new ItemRemovedEventArgs(parent));
			}
		}

		void IOwnerItemEvents.InvokeItemAdded(BaseItem item,EventArgs e)
		{
			if(ItemAdded!=null)
				ItemAdded(item,e);
		}

		void IOwnerItemEvents.InvokeContainerLoadControl(BaseItem item,EventArgs e)
		{
			if(ContainerLoadControl!=null)
				ContainerLoadControl(item,e);
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

		void IOwnerItemEvents.InvokeOptionGroupChanging(BaseItem item, OptionGroupChangingEventArgs e)
		{
			if(OptionGroupChanging!=null)
				OptionGroupChanging(item,e);
		}

        void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
        {
            if (ButtonCheckedChanged != null)
                ButtonCheckedChanged(item, e);
        }

		void IOwnerItemEvents.InvokeToolTipShowing(object item, EventArgs e)
		{
			if(ToolTipShowing!=null)
				ToolTipShowing(item,e);
		}

		void IOwnerBarSupport.InvokeDockTabChange(Bar bar,DockTabChangeEventArgs e)
		{
			if(DockTabChange!=null)
				DockTabChange(bar,e);
		}

		void IOwnerBarSupport.InvokeBarClosing(Bar bar,BarClosingEventArgs e)
		{
			if(BarClosing!=null)
				BarClosing(bar,e);
		}

		void IOwnerBarSupport.InvokeBarTearOff(Bar bar, EventArgs e)
		{
			if(BarTearOff!=null)
                BarTearOff(bar,e);
		}

		void IOwnerBarSupport.InvokeAutoHideChanged(Bar bar,EventArgs e)
		{
			if(AutoHideChanged!=null)
				AutoHideChanged(bar,e);
		}

		void IOwnerBarSupport.InvokeAutoHideDisplay(Bar bar,AutoHideDisplayEventArgs e)
		{
			if(AutoHideDisplay!=null)
				AutoHideDisplay(bar,e);
		}

		internal bool IsHandleOwnedByPopup(IntPtr handle)
		{
			if(m_RegisteredPopups.Count>0)
			{
				foreach(BaseItem item in m_RegisteredPopups)
				{
					Control ctrl=((PopupItem)item).PopupControl;
					if(ctrl!=null && ctrl.Handle==handle || item.IsAnyOnHandle(handle.ToInt32()))
						return true;
				}
			}
			return false;
		}

		void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
		{
			if(LocalizeString!=null)
				LocalizeString(this,e);
		}

//		public bool PreFilterMessage(ref System.Windows.Forms.Message m)
//		{
//			switch(m.Msg)
//			{
//				case NativeFunctions.WM_SYSKEYDOWN:
//				{
//					return this.OnSysKeyDown(m.HWnd.ToInt32(),m.WParam.ToInt32(),m.LParam.ToInt32());
//					//break;
//				}
//				case NativeFunctions.WM_SYSKEYUP:
//				{
//					return this.OnSysKeyUp(m.HWnd.ToInt32(),m.WParam.ToInt32(),m.LParam.ToInt32());
//					//break;
//				}
//				case NativeFunctions.WM_KEYDOWN:
//				{
//					return this.OnKeyDown(m.HWnd.ToInt32(),m.WParam.ToInt32(),m.LParam.ToInt32());
//					//break;
//				}
//				case NativeFunctions.WM_LBUTTONDOWN:
//				case NativeFunctions.WM_NCLBUTTONDOWN:
//				case NativeFunctions.WM_RBUTTONDOWN:
//				case NativeFunctions.WM_MBUTTONDOWN:
//				case NativeFunctions.WM_NCMBUTTONDOWN:
//				case NativeFunctions.WM_NCRBUTTONDOWN:
//				{
//					this.OnMouseDown(m.HWnd.ToInt32(),m.WParam.ToInt32(),m.LParam.ToInt32());
//					break;
//				}
//				case NativeFunctions.WM_MOUSEMOVE:
//				{
//					if(m_RegisteredPopups.Count>0)
//					{
//						foreach(BaseItem item in m_RegisteredPopups)
//						{
//							if(item.Parent==null)
//							{
//								Control ctrl=((PopupItem)item).PopupControl;
//								if(ctrl!=null && ctrl.Handle!=m.HWnd && !item.IsAnyOnHandle(m.HWnd.ToInt32()) && !(ctrl.Parent!=null && ctrl.Parent.Handle!=m.HWnd))
//									return true;
//							}
//						}
//					}
//					break;
//				}
//			}
//			
//			return false;
//		}
//
//		public void PostFilterMessage(ref System.Windows.Forms.Message m)
//		{
//		}

		// *********************************************************************
		//
		// Extended Property ContextMenuEx implementation code
		//
		// *********************************************************************
		//private delegate void WmContextEventHandler(object sender, WmContextEventArgs e);
		//[DefaultValue(""),Editor(typeof(ContextExMenuTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public string GetContextMenuEx(Control control) 
		{
			string text = (string)m_ContextExMenus[control];
			if (text == null) 
			{
				text = string.Empty;
			}
			return text;
		}
		public void SetContextMenuEx(Control control, string value)
		{
			if (value == null) 
			{
				value = string.Empty;
			}

			if(value.Length == 0) 
			{
				if(m_ContextExMenus.Contains(control))
				{
					m_ContextExMenus.Remove(control);
                    //control.MouseUp-=new MouseEventHandler(this.ContextExMouseUp);
                    //try
                    //{control.HandleDestroyed-=new EventHandler(this.ContextExHandleDestroy);}
                    //catch{}
                    //try
                    //{control.HandleCreated-=new EventHandler(this.ContextExHandleCreate);}
                    //catch{}
				}
			}
			else 
			{
				if(m_ContextExMenus.Contains(control))
				{
					m_ContextExMenus[control] = value;
				}
				else
				{
					m_ContextExMenus[control] = value;
                    //if(!m_ContextExHandlers.Contains(control) && !this.DesignMode)
                    //{
                    //    if(!(control is System.Windows.Forms.TreeView) && !(control is System.Windows.Forms.Form) && !(control is System.Windows.Forms.Panel) && m_ContextMenuSubclass)
                    //    {
                    //        if(control.IsHandleCreated)
                    //        {
                    //            ContextMessageHandler h=new ContextMessageHandler();
                    //            h.ContextMenu+=new WmContextEventHandler(this.OnContextMenu);
                    //            h.ParentControl=control;
                    //            h.AssignHandle(control.Handle);
                    //            m_ContextExHandlers[control]=h;
                    //        }
                    //        control.HandleDestroyed+=new EventHandler(this.ContextExHandleDestroy);
                    //        control.HandleCreated+=new EventHandler(this.ContextExHandleCreate);
                    //    }
                    //}
                    //try{control.MouseUp+=new MouseEventHandler(this.ContextExMouseUp);}
                    //catch{}
				}
			}
		}

//        internal bool HasContextMenu(Control ctrl)
//        {
//            return m_ContextExMenus.Contains(ctrl);
//        }

//        private void ContextExHandleDestroy(object sender, EventArgs e)
//        {
//            Control control=sender as Control;
//            if(control==null)
//                return;
//            ContextMessageHandler h=m_ContextExHandlers[control] as ContextMessageHandler;
//            if(h!=null)
//            {
//                h.ReleaseHandle();
//                h=null;
//            }
//            m_ContextExHandlers.Remove(control);
//        }

//        private void ContextExHandleCreate(object sender, EventArgs e)
//        {
//            Control control=sender as Control;
//            if(control==null)
//                return;
//            if(m_ContextExHandlers.Contains(control))
//                return;

//            ContextMessageHandler h=new ContextMessageHandler();
//            h.ContextMenu+=new WmContextEventHandler(this.OnContextMenu);
//            h.ParentControl=control;
//            h.AssignHandle(control.Handle);
//            m_ContextExHandlers[control]=h;
//        }

//        private void ContextExMouseUp(object sender, MouseEventArgs e)
//        {
//            if(e.Button!=MouseButtons.Right)
//                return;
//            Control ctrl=sender as Control;
//            if(ctrl==null)
//                return;
//            string itemName=GetContextExItemName(ctrl);
//            if(itemName=="")
//                return;

//            // Find it in pop-ups
//            PopupItem popup=null;
//            if(m_ContextMenus.Contains(itemName))
//                popup=m_ContextMenus[itemName] as PopupItem;
//            if(popup==null)
//            {
//                MessageBox.Show("Could not find popup item '"+itemName+"'.");
//                return;
//            }
//            if(!popup.Expanded)
//            {
//                popup.Style=m_Style;
//                popup.SetSourceControl(ctrl);
//                popup.Popup(ctrl.PointToScreen(new Point(e.X,e.Y)));
//            }
//        }

//        private Control FindControl(Control parent, IntPtr handle)
//        {
//            foreach(Control ctrl in parent.Controls)
//            {
//                if(ctrl.Handle==handle)
//                    return ctrl;
//                if(ctrl.Controls.Count>0)
//                {
//                    Control ret=FindControl(ctrl,handle);
//                    if(ret!=null)
//                        return ret;
//                }
//            }
//            return parent;
//        }

//        private string GetContextExItemName(Control ctrl)
//        {
//            string itemName=(string)m_ContextExMenus[ctrl];
//            if(itemName==null)
//                itemName=String.Empty;

//            return itemName;
//        }
//        private void OnContextMenu(object sender, WmContextEventArgs e)
//        {
//            ContextMessageHandler h=sender as ContextMessageHandler;
//            if(h==null)
//                return;
			
//            string itemName=GetContextExItemName(h.ParentControl);
//            if(itemName=="")
//                return;

//            // Find it in pop-ups
//            PopupItem popup=null;
//            if(m_ContextMenus.Contains(itemName))
//                popup=m_ContextMenus[itemName] as PopupItem;
//            if(popup==null)
//            {
//                MessageBox.Show("Could not find popup item '"+itemName+"'.");
//                return;
//            }
			
//            popup.Style=this.Style;

//            if(e.Button==MouseButtons.None)
//            {
//                // Get the control with focus
//                Control ctrl=Control.FromChildHandle(e.Handle);
//                if(ctrl!=null && ctrl.Handle!=e.Handle)
//                {
//                    ctrl=FindControl(ctrl,e.Handle);
//                }
//                popup.SetSourceControl(h.ParentControl);
//                if(ctrl!=null)
//                    popup.Popup(ctrl.PointToScreen(Point.Empty));
//                else
//                    popup.Popup(Control.MousePosition);

//                // We need to eat the message in OnSysKeyUp for Shift+F10 case
//                if(m_IgnoreSysKeyUp)
//                {
//                    m_IgnoreSysKeyUp=false;
//                    m_EatSysKeyUp=true;
//                }
//            }
//            else
//            {
//                // This is handled by the WM_RBUTTONUP just eat it
//                if(!e.WmContext)
//                {
//                    popup.SetSourceControl(h.ParentControl);
//                    popup.Popup(e.X,e.Y);
//                }
//            }
//            e.Handled=true;
//        }

//        private class ContextMessageHandler:NativeWindow
//        {
//            public event WmContextEventHandler ContextMenu;
//            private const int WM_CONTEXTMENU=0x007B;
//            private const int WM_RBUTTONUP=0x0205;
//            private const int WM_NCRBUTTONUP=0x00A5;
//            private const int WM_RBUTTONDOWN=0x0204;
//            public Control ParentControl=null;
//            protected override void WndProc(ref Message m)
//            {
//                if(m.Msg==WM_CONTEXTMENU)
//                {
//                    if(ContextMenu!=null)
//                    {
//                        int ilParam=m.LParam.ToInt32();
//                        int y=ilParam>>16;
//                        int x=ilParam & 0xFFFF;
//                        IntPtr hWnd=m.WParam;
//                        if(hWnd==IntPtr.Zero)
//                            hWnd=m.HWnd;
//                        bool context=true;
//                        if(m.HWnd!=m.WParam)
//                            context=false;
//                        WmContextEventArgs e=new WmContextEventArgs(hWnd,x,y,((x==65535 && y==-1)?MouseButtons.None:MouseButtons.Right),context);
//                        ContextMenu(this,e);
//                        if(e.Handled)
//                            return;
//                    }
//                }
//                // This case was taken out becouse the message was not generated for the listview control and possibly for
//                // treview control so this code was moved to the MouseUp event of the Control see ContextExMouseUp
////				else if(m.Msg==WM_RBUTTONUP || m.Msg==WM_NCRBUTTONUP)
////				{
////					if(ContextMenu!=null)
////					{
////						int ilParam=m.LParam.ToInt32();
////						int y=ilParam>>16;
////						int x=ilParam & 0xFFFF;
////						Point p=ParentControl.PointToScreen(new Point(x,y));
////						WmContextEventArgs e=new WmContextEventArgs(m.HWnd,p.X,p.Y,MouseButtons.Right,false);
////						ContextMenu(this,e);
////					}
////				}
//                base.WndProc(ref m);
//            }
//        }
//        private class WmContextEventArgs : EventArgs 
//        {
//            private readonly int x;
//            private readonly int y;
//            private readonly MouseButtons button=0;
//            private readonly IntPtr hwnd;
//            private readonly bool wmcontext;
//            public bool Handled=false;
//            public WmContextEventArgs(IntPtr phwnd, int ix, int iy, MouseButtons eButton, bool WmContextMessage) 
//            {
//                this.x=ix;
//                this.y=iy;
//                this.button=eButton;
//                this.hwnd=phwnd;
//                this.wmcontext=WmContextMessage;
//            }
//            public int X
//            {
//                get{return this.x;}
//            }
//            public int Y
//            {
//                get{return this.y;}
//            }
//            public MouseButtons Button
//            {
//                get{return this.button;}
//            }
//            public IntPtr Handle
//            {
//                get{return this.hwnd;}
//            }
//            public bool WmContext
//            {
//                get{return this.wmcontext;}
//            }
//        }
		// *********************************************************************
		//
		// End Of Extended Property ContextMenuEx implementation code
		//
		// *********************************************************************

		/// <summary>
		/// DotNetBar Design-Time editor
		/// </summary>
//		private class DotNetBarEditor:System.Drawing.Design.UITypeEditor
//		{
//            private System.Windows.Forms.Design.IWindowsFormsEditorService m_EditorService=null;
//
//			public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
//			{
//				if (context != null && context.Instance != null) 
//				{
//					return System.Drawing.Design.UITypeEditorEditStyle.Modal;
//				}
//				return base.GetEditStyle(context);
//			}
//
//			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
//			{
//				if (context!=null && context.Instance!=null && provider!=null) 
//				{
//					
//					m_EditorService=(System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
//					
//					if(m_EditorService!=null) 
//					{
//						Bars b=null;
//						DotNetBarManager ctrl=context.Instance as DotNetBarManager;
//						if(ctrl!=null)
//							b=ctrl.Bars;
//						BarEditor editor=new BarEditor(ctrl);
//
//						m_EditorService.ShowDialog(editor);
//						context.OnComponentChanged();
//					}
//				}
//			
//				return value;
//			}
//		}
    }
	
	internal class ShortcutTableEntry
	{
		public eShortcut Shortcut;
		public Hashtable Items;
		public ShortcutTableEntry(eShortcut key)
		{
			Shortcut=key;
			Items=new Hashtable();
		}
		public override int GetHashCode()
		{
			return (int)Shortcut;
		}
	}

	internal class ParentMsgHandler:NativeWindow
	{
		const int WM_WINDOWPOSCHANGING=0x0046;
		//private DotNetBarManager m_BarControl;
		private ArrayList m_Managers=new ArrayList();
		private ArrayList m_Owners=new ArrayList();
		private bool m_DesignMode=false;
		public ParentMsgHandler(bool designmode):base()
		{
			//m_Managers.Add(objBarControl);
			m_DesignMode=designmode;
		}
		public void Register(IOwner manager)
		{
			m_Managers.Add(manager);
		}
		public void Unregister(IOwner manager)
		{
			m_Managers.Remove(manager);
		}
		public bool IsRegistered(IOwner manager)
		{
			return m_Managers.Contains(manager);
		}
		public int RegisteredCount
		{
			get
			{
				return m_Managers.Count;
			}
		}
		public void RegisterOwner(IOwner owner)
		{
			m_Owners.Add(owner);
		}
		public void UnregisterOwner(IOwner owner)
		{
			m_Owners.Remove(owner);
		}
		public bool IsOwnerRegistered(IOwner owner)
		{
			return m_Owners.Contains(owner);
		}
		public int RegisteredOwnersCount
		{
			get
			{
				return m_Owners.Count;
			}
		}
        
		public void ApplicationActivate()
		{
			if(m_Managers.Count>0)
			{
				IOwner[] managers;
				lock(this)
				{
					managers=(IOwner[])m_Managers.ToArray(typeof(IOwner));
				}
                foreach (IOwner manager in managers)
                {
                    if (manager is Control && ((Control)manager).InvokeRequired)
                    {
                        ((Control)manager).BeginInvoke(new InvokeActivateDelegate(manager.OnApplicationActivate));
                    }
                    else
                        manager.OnApplicationActivate();
                }
			}
		}

		public void ApplicationDeactivate()
		{
			if(m_Managers.Count>0)
			{
				IOwner[] managers;
				lock(this)
				{
					managers=(IOwner[])m_Managers.ToArray(typeof(IOwner));
				}
                foreach (IOwner manager in managers)
                {
                    if (manager is Control && ((Control)manager).InvokeRequired)
                    {
                        ((Control)manager).BeginInvoke(new InvokeActivateDelegate(manager.OnApplicationDeactivate));
                    }
                    else
                        manager.OnApplicationDeactivate();
                }
			}	
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_WINDOWPOSCHANGING)
			{
				NativeFunctions.WINDOWPOS pos=new NativeFunctions.WINDOWPOS();
				pos=(NativeFunctions.WINDOWPOS)m.GetLParam(pos.GetType());
				if(pos.hwndInsertAfter!=0)
				{
					if(m_Managers.Count>0)
					{
						IOwner[] managers;
						lock(this)
						{
							managers=(IOwner[])m_Managers.ToArray(typeof(IOwner));
						}
						foreach(IOwner manager in managers)
							manager.OnParentPositionChanging();
					}
				}
			}
			else if(m.Msg==NativeFunctions.WM_ACTIVATEAPP)
			{
				if(m.WParam.ToInt32()!=0)
				{
					DotNetBarManager.RelayApplicationActivate(true);
					//ApplicationActivate();
				}
				else
				{
					DotNetBarManager.RelayApplicationActivate(false);
					//ApplicationDeactivate();				
				}
			}
			else if(m.Msg==NativeFunctions.WM_ACTIVATE)
			{
				if((m.WParam.ToInt32() & 0xFF)==NativeFunctions.WA_INACTIVE)
				{
					// Ignore if it is our Bar window that is being activated...
					Control ctrl=Control.FromChildHandle(m.LParam);
					bool customizing=false;
					foreach(IOwner manager in m_Managers)
					{
						if(manager is DotNetBarManager && ((DotNetBarManager)manager).IsCustomizing)
						{
							customizing=true;
							break;
						}
					}
					if(ctrl!=null && !customizing)
					{
						while(ctrl.Parent!=null)
							ctrl=ctrl.Parent;
						if(!(ctrl is DevComponents.DotNetBar.Bar || ctrl is DevComponents.DotNetBar.MenuPanel || ctrl is DevComponents.DotNetBar.PopupContainerControl || ctrl is DevComponents.DotNetBar.PopupContainer))
						{
							if(m_Managers.Count>0)
							{
								IOwner[] managers;
								lock(this)
								{
									managers=(IOwner[])m_Managers.ToArray(typeof(IOwner));
								}
								foreach(IOwner manager in managers)
									manager.SetExpandedItem(null);
							}	
						}
					}
				}
			}
			else if(m.Msg==NativeFunctions.WM_DISPLAYCHANGE)
			{
				NativeFunctions.OnDisplayChange();
			}
			else if(m.Msg==NativeFunctions.WM_NCACTIVATE && m_DesignMode && m.WParam==IntPtr.Zero)
			{
				// THIS IS POSSIBLE TO USE ONLY WHILE WINDOW IS IN DESIGN MODE!!!
				if(m_Managers.Count>0)
				{
					IOwner[] managers;
					lock(this)
					{
						managers=(IOwner[])m_Managers.ToArray(typeof(IOwner));
					}
					foreach(IOwner manager in managers)
						manager.SetExpandedItem(null);
					IOwner[] owners;
					lock(this)
					{
						owners=(IOwner[])m_Owners.ToArray(typeof(IOwner));
					}
					foreach(IOwner owner in owners)
						owner.SetExpandedItem(null);
				}	
			}

			base.WndProc(ref m);
		}
	}

	internal class MDIClientMsgHandler:NativeWindow
	{
		const int WM_MDISETMENU=0x230;
        const int WM_MDIREFRESHMENU = 0x234;
		public event EventHandler MdiSetMenu;
		public bool EatMessage=false;
		protected override void WndProc(ref Message m)
		{
            if (m.Msg == WM_MDISETMENU || m.Msg == WM_MDIREFRESHMENU)
			{
				if(MdiSetMenu!=null)
					MdiSetMenu(this, new EventArgs());
				if(EatMessage)
				{
					EatMessage=false;
					return;
				}
			}
			base.WndProc(ref m);
		}
	}

	public class ItemRemovedEventArgs : EventArgs 
	{
		private readonly BaseItem parent;
		public ItemRemovedEventArgs(BaseItem parentItem) 
		{
			this.parent=parentItem;
		}
		public BaseItem Parent
		{
			get{return this.parent;}
		}
	}
	public class CustomizeContextMenuEventArgs : EventArgs 
	{
		private readonly BaseItem parent;
		public CustomizeContextMenuEventArgs(BaseItem parentItem) 
		{
			this.parent=parentItem;
		}
		public BaseItem Parent
		{
			get{return this.parent;}
		}
	}

	public class DockTabChangeEventArgs : EventArgs 
	{
		public readonly BaseItem OldTab;
		public readonly BaseItem NewTab;
		public bool Cancel=false;
		public DockTabChangeEventArgs(BaseItem oldtab,BaseItem newtab) 
		{
			this.OldTab=oldtab;
			this.NewTab=newtab;
		}
	}

	public class BarClosingEventArgs : EventArgs 
	{
		public bool Cancel=false;
		public BarClosingEventArgs() 
		{
		}
	}

	public class AutoHideDisplayEventArgs : EventArgs
	{
        /// <summary>
        /// Gets or sets the display rectangle for popup auto-hide bar.
        /// </summary>
		public Rectangle DisplayRectangle=Rectangle.Empty;
		public AutoHideDisplayEventArgs(Rectangle displayRectangle)
		{
			this.DisplayRectangle=displayRectangle;
		}
	}

	public class PopupOpenEventArgs : EventArgs
	{
		public bool Cancel=false;
		public PopupOpenEventArgs() {}
	}

	/// <summary>
	/// Event arguments for LocalizeString event.
	/// </summary>
	public class LocalizeEventArgs : EventArgs
	{
		/// <summary>
		/// Indicates that event has been handled and that LocalizedValue should be used.
		/// </summary>
		public bool Handled=false;
		/// <summary>
		/// Indicates the string key for the text that needs to be localized.
		/// </summary>
		public string Key="";
		/// <summary>
		/// Indicates the localized text value. If you are performing custom string localization
		/// you need to set this value to the translated text for current locale and you need to set
		/// Handled property to true.
		/// </summary>
		public string LocalizedValue="";
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LocalizeEventArgs() {}
	}

	/// <summary>
	/// Event arguments for EndUserCustomize event.
	/// </summary>
	public class EndUserCustomizeEventArgs:EventArgs
	{
		/// <summary>
		/// Indicates the customize action that user executed.
		/// </summary>
		public readonly eEndUserCustomizeAction Action;
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="action">Indicates action user executed.</param>
		public EndUserCustomizeEventArgs(eEndUserCustomizeAction action)
		{
			this.Action=action;
		}
	}
	
	/// <summary>
	/// Delegate for EndUserCustomize event.
	/// </summary>
	public delegate void EndUserCustomizeEventHandler(object sender, EndUserCustomizeEventArgs e);

    public class ToolboxIconResFinder { }

    internal delegate void InvokeActivateDelegate();

    /// <summary>
    /// Defines delegate for ActiveDockContainerChanged event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ActiveDockContainerChangedEventHandler(object sender, ActiveDockContainerChangedEventArgs e);
    /// <summary>
    /// Provides event arguments for ActiveDockContainerChanged event.
    /// </summary>
    public class ActiveDockContainerChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the DockContainerItem that has been activate or deactivated.
        /// </summary>
        public readonly DockContainerItem Item;

        public readonly bool IsActive;

        /// <summary>
        /// Initializes a new instance of the ActiveDockContainerChangedEventArgs class.
        /// </summary>
        /// <param name="item"></param>
        public ActiveDockContainerChangedEventArgs(DockContainerItem item, bool isActive)
        {
            Item = item;
            IsActive = isActive;
        }
    }
}
 
