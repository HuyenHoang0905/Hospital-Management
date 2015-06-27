namespace DevComponents.DotNetBar
{
	using System;
	using System.Drawing;
    /// <summary>
    /// Interface for designer-item integration.
    /// </summary>
    public interface IBarItemDesigner
    {
        void StartExternalDrag(BaseItem item);
    }

    /// <summary>
    /// Interface implemented by target Bar interested in access to designer.
    /// </summary>
    public interface IBarDesignerServices
    {
        /// <summary>
        /// Gets or sets the BarBaseControlDesigner instance.
        /// </summary>
        IBarItemDesigner Designer { get;set;}
    }

	/// <summary>
	/// Provides design-time support for DotNetBar items.
	/// </summary>
	public interface IDesignTimeProvider
	{
		InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem);
		void DrawReversibleMarker(int iPos, bool Before);
		void InsertItemAt(BaseItem objItem, int iPos, bool Before);
	}

	public class InsertPosition
	{
		public int Position;
		public bool Before;
		public IDesignTimeProvider TargetProvider;
		public InsertPosition()
		{
			Position=-1;
			Before=false;
			TargetProvider=null;
		}
		public InsertPosition(int iPosition, bool bBefore, IDesignTimeProvider target)
		{
			this.Position=iPosition;
			this.Before=bBefore;
			this.TargetProvider=target;
		}
	}

	/// <summary>
	/// Provides support for personalized menu items.
	/// </summary>
	public interface IPersonalizedMenuItem
	{
		eMenuVisibility MenuVisibility {get;set;}
		bool RecentlyUsed {get;set;}
	}

	public interface IOwnerItemEvents
	{
		void InvokeItemAdded(BaseItem item,EventArgs e);
		void InvokeItemRemoved(BaseItem item, BaseItem parent);
		void InvokeMouseEnter(BaseItem item,EventArgs e);
		void InvokeMouseHover(BaseItem item,EventArgs e);
        void InvokeMouseLeave(BaseItem item,EventArgs e);
		void InvokeMouseDown(BaseItem item, System.Windows.Forms.MouseEventArgs e);
		void InvokeMouseUp(BaseItem item, System.Windows.Forms.MouseEventArgs e);
		void InvokeMouseMove(BaseItem item, System.Windows.Forms.MouseEventArgs e);
		void InvokeItemClick(BaseItem objItem);
        void InvokeItemDoubleClick(BaseItem objItem, System.Windows.Forms.MouseEventArgs e);
		void InvokeGotFocus(BaseItem item,EventArgs e);
		void InvokeLostFocus(BaseItem item,EventArgs e);
		void InvokeExpandedChange(BaseItem item,EventArgs e);
		void InvokeItemTextChanged(BaseItem item, EventArgs e);
//		void InvokeItemDisplayedChanged(BaseItem item, EventArgs e);

		void InvokeContainerControlDeserialize(BaseItem item,ControlContainerSerializationEventArgs e);
        void InvokeContainerControlSerialize(BaseItem item,ControlContainerSerializationEventArgs e);
		void InvokeContainerLoadControl(BaseItem item,EventArgs e);

		void InvokeOptionGroupChanging(BaseItem item, OptionGroupChangingEventArgs e);

		void InvokeToolTipShowing(object item, EventArgs e);

        void InvokeCheckedChanged(ButtonItem item, EventArgs e);
	}

	public interface IOwnerMenuSupport
	{
		bool PersonalizedAllVisible {get;set;}
		bool ShowFullMenusOnHover {get;set;}
		bool AlwaysShowFullMenus {get;set;}

		void RegisterPopup(PopupItem objPopup);
		void UnregisterPopup(PopupItem objPopup);
		bool RelayMouseHover();

		// Events
		void InvokePopupClose(PopupItem item,EventArgs e);
		void InvokePopupContainerLoad(PopupItem item,EventArgs e);
		void InvokePopupContainerUnload(PopupItem item,EventArgs e);
		void InvokePopupOpen(PopupItem item,PopupOpenEventArgs e);
		void InvokePopupShowing(PopupItem item,EventArgs e);

		bool ShowPopupShadow {get;}
		eMenuDropShadow MenuDropShadow{get;set;}
		ePopupAnimation PopupAnimation{get;set;}
		bool AlphaBlendShadow{get;set;}
        /// <summary>
        /// Closes all popups managed by the owner control.
        /// </summary>
        void ClosePopups();
	}

	public interface IOwnerBarSupport
	{
		void InvokeDockTabChange(Bar bar,DockTabChangeEventArgs e);
		void InvokeBarDock(Bar bar,EventArgs e);
		void InvokeBarUndock(Bar bar,EventArgs e);
		void InvokeAutoHideChanged(Bar bar,EventArgs e);
		void InvokeBarClosing(Bar bar,BarClosingEventArgs e);
		void InvokeAutoHideDisplay(Bar bar,AutoHideDisplayEventArgs e);
		void InvokeBarTearOff(Bar bar,EventArgs e);
		void InvokeBeforeDockTabDisplay(BaseItem item, EventArgs e);

		DockSiteInfo GetDockInfo(IDockInfo pDock, int x, int y);
		void DockComplete();

		void BarContextMenu(System.Windows.Forms.Control bar, System.Windows.Forms.MouseEventArgs e);

		System.Collections.ArrayList WereVisible{get;}
		void AddShortcutsFromBar(Bar bar);
		void RemoveShortcutsFromBar(Bar bar);

		// Dock Site Support
		DockSite LeftDockSite{get;set;}
		DockSite RightDockSite{get;set;}
		DockSite TopDockSite{get;set;}
		DockSite BottomDockSite{get;set;}
		DockSite FillDockSite{get;set;}

        DockSite ToolbarLeftDockSite { get;set;}
        DockSite ToolbarRightDockSite { get;set;}
        DockSite ToolbarTopDockSite { get;set;}
        DockSite ToolbarBottomDockSite { get;set;}

		bool ApplyDocumentBarStyle{get;set;}
	}

	public interface IOwnerAutoHideSupport
	{
		// Auto-hide support
		AutoHidePanel LeftAutoHidePanel {get;set;}
		AutoHidePanel RightAutoHidePanel {get;set;}
		AutoHidePanel TopAutoHidePanel {get;set;}
		AutoHidePanel BottomAutoHidePanel {get;set;}
		bool HasLeftAutoHidePanel {get;}
		bool HasRightAutoHidePanel {get;}
		bool HasTopAutoHidePanel {get;}
		bool HasBottomAutoHidePanel {get;}

	}

	public interface IOwner
	{
		System.Windows.Forms.Form ParentForm {get;set;}
		System.Collections.ArrayList GetItems(string ItemName);
		System.Collections.ArrayList GetItems(string ItemName, Type itemType);
        System.Collections.ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName);
		BaseItem GetItem(string ItemName);
		void SetExpandedItem(BaseItem objItem);
		BaseItem GetExpandedItem();

		void SetFocusItem(BaseItem objFocusItem);
		BaseItem GetFocusItem();
		void DesignTimeContextMenu(BaseItem objItem);

		void RemoveShortcutsFromItem(BaseItem objItem);
		void AddShortcutsFromItem(BaseItem objItem);

		System.Windows.Forms.ImageList Images {get;set;}
		System.Windows.Forms.ImageList ImagesMedium {get;set;}
		System.Windows.Forms.ImageList ImagesLarge {get;set;}

		// Drag support
		void StartItemDrag(BaseItem objItem);
		BaseItem DragItem{get;}
		bool DragInProgress{get;}

		bool ShowToolTips{get;set;}
		bool ShowShortcutKeysInToolTips{get;set;}
		bool AlwaysDisplayKeyAccelerators{get;set;}

		System.Windows.Forms.Form ActiveMdiChild{get;}
		System.Windows.Forms.MdiClient GetMdiClient(System.Windows.Forms.Form MdiForm);
		
		void Customize();
		void InvokeResetDefinition(BaseItem item,EventArgs e);
        bool ShowResetButton{get;set;}

		void InvokeDefinitionLoaded(object sender,EventArgs e);
		void InvokeUserCustomize(object sender,EventArgs e);
		void InvokeEndUserCustomize(object sender, EndUserCustomizeEventArgs e);

		void OnApplicationActivate();
		void OnApplicationDeactivate();
		void OnParentPositionChanging();

		bool DesignMode {get;}

		bool DisabledImagesGrayScale{get;set;}
	}

	/// <summary>
	/// Provides support for custom localization.
	/// </summary>
	public interface IOwnerLocalize
	{
		void InvokeLocalizeString(LocalizeEventArgs e);
	}

	internal interface IThemeCache
	{
		DevComponents.DotNetBar.ThemeWindow ThemeWindow{get;}
		DevComponents.DotNetBar.ThemeRebar ThemeRebar{get;}
		DevComponents.DotNetBar.ThemeToolbar ThemeToolbar{get;}
		DevComponents.DotNetBar.ThemeHeader ThemeHeader{get;}
		DevComponents.DotNetBar.ThemeScrollBar ThemeScrollBar{get;}
		DevComponents.DotNetBar.ThemeExplorerBar ThemeExplorerBar{get;}
		DevComponents.DotNetBar.ThemeProgress ThemeProgress{get;}
        DevComponents.DotNetBar.ThemeButton ThemeButton { get;}
	}

	public interface IBarImageSize
	{
		eBarImageSize ImageSize {get;set;}
	}

	/// <summary>
	/// Describes container support for setting the word-wrap behavior of it's sub-items.
	/// </summary>
	public interface IContainerWordWrap
	{
		/// <summary>
		/// Gets or sets whether sub items text will be word wrapped if it cannot fit the space allocated.
		/// </summary>
		bool WordWrapSubItems{get;set;}
	}

    /// <summary>
    /// Desribes interface that provides custom serialization support for items.
    /// </summary>
    public interface ICustomSerialization
    {
        /// <summary>
        /// Occurs after an item has been serialized to XmlElement and provides you with opportunity to add any custom data
        /// to serialized XML. This allows you to serialize any data with the item and load it back up in DeserializeItem event.
        /// </summary>
        /// <remarks>
        /// 	<para>To serialize custom data to XML definition control creates handle this event and use CustomXmlElement
        /// property on SerializeItemEventArgs to add new nodes or set attributes with custom data you want saved.</para>
        /// </remarks>
        event SerializeItemEventHandler SerializeItem;

        /// <summary>
        /// Occurs after an item has been de-serialized (load) from XmlElement and provides you with opportunity to load any custom data
        /// you have serialized during SerializeItem event.
        /// </summary>
        /// <remarks>
        /// 	<para>To de-serialize custom data from XML definition handle this event and use CustomXmlElement
        /// property on SerializeItemEventArgs to retrive any data you saved in SerializeItem event.</para>
        /// </remarks>
        event SerializeItemEventHandler DeserializeItem;

        /// <summary>
        /// Invokes SerializeItem event.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        void InvokeSerializeItem(SerializeItemEventArgs e);

        /// <summary>
        /// Invokes DeserializeItem event.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        void InvokeDeserializeItem(SerializeItemEventArgs e);

        /// <summary>
        /// Gets whether any handlers have been defined for SerializeItem event. If no handles have been defined to optimize performance SerializeItem event will not be attempted to fire.
        /// </summary>
        bool HasSerializeItemHandlers { get;}

        /// <summary>
        /// Gets whether any handlers have been defined for DeserializeItem event. If no handles have been defined to optimize performance DeserializeItem event will not be attempted to fire.
        /// </summary>
        bool HasDeserializeItemHandlers { get;}
    }

    /// <summary>
    /// Defines interface that should be implemented by controls that support per control renderers.
    /// </summary>
    public interface IRenderingSupport
    {
        /// <summary>
        /// Gets the renderer control will be rendered with. This can be either custom renderer set on the control or the
        /// Rendering.GlobalManager specified renderer.
        /// </summary>
        /// <returns></returns>
        Rendering.BaseRenderer GetRenderer();

        /// <summary>
        /// Gets or sets the redering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
        /// also be set to the custom renderer that will be used.
        /// </summary>
        eRenderMode RenderMode { get; set; }

        /// <summary>
        /// Gets or sets the custom renderer used by the items on this control. RenderMode property must also be set to eRenderMode.Custom in order renderer
        /// specified here to be used.
        /// </summary>
        DevComponents.DotNetBar.Rendering.BaseRenderer Renderer { get; set;}
    }

    /// <summary>
    /// Defines interface for internal accessibility support for DotNetBar control.
    /// </summary>
    public interface IAccessibilitySupport
    {
        /// <summary>
        /// Gets or sets the item default accesibility action will be performed on.
        /// </summary>
        BaseItem DoDefaultActionItem { get; set;}
    }

    /// <summary>
    /// Provides interface for controls that support ribbon customization.
    /// </summary>
    public interface IRibbonCustomize
    {
        /// <summary>
        /// Called when item contained by the container is right-clicked.
        /// </summary>
        /// <param name="item">Instance of the item that was right-clicked.</param>
        void ItemRightClick(BaseItem item);
    }

    /// <summary>
    /// Defines notification interface for scrollable item controls.
    /// </summary>
    public interface IScrollableItemControl
    {
        /// <summary>
        /// Indicates that item has been selected via keyboard.
        /// </summary>
        /// <param name="item">Reference to item being selected</param>
        void KeyboardItemSelected(BaseItem item);
    }
}