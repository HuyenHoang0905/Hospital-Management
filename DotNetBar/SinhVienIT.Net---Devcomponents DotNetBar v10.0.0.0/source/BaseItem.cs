using System;
using System.Collections;
using DevComponents.UI.ContentManager;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines the base class for items that are used by DotNetBar.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
    public abstract class BaseItem : Component, ICloneable, IBlock, ICommandSource
#if FRAMEWORK20
        , IBindableComponent
#endif
	{
		#region Event Definition
		// Events
		/// <summary>
		/// Occurs when Item is clicked.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Item is clicked.")]
		public event EventHandler Click;

        /// <summary>
        /// Occurs when Item is double-clicked.
        /// </summary>
        [System.ComponentModel.Description("Occurs when Item is double-clicked.")]
        public event EventHandler DoubleClick;

		/// <summary>
		/// Occurs when Item Expanded property has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Item Expanded property has changed.")]
		public event EventHandler ExpandChange;

		/// <summary>
		/// Occurs when item loses input focus.
		/// </summary>
		[System.ComponentModel.Description("Occurs when item loses input focus.")]
		public event EventHandler LostFocus;

		/// <summary>
		/// Occurs when item receives input focus.
		/// </summary>
		[System.ComponentModel.Description("Occurs when item receives input focus.")]
		public event EventHandler GotFocus;

		/// <summary>
		/// Occurs when mouse button is pressed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse button is pressed.")]
		public event System.Windows.Forms.MouseEventHandler MouseDown;

		/// <summary>
		/// Occurs when mouse button is released.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse button is released.")]
		public event System.Windows.Forms.MouseEventHandler MouseUp;

		/// <summary>
		/// Occurs when mouse enters the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse enters the item.")]
		public event EventHandler MouseEnter;

		/// <summary>
		/// Occurs when mouse leaves the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse leaves the item.")]
		public event EventHandler MouseLeave;

		/// <summary>
		/// Occurs when mouse moves over the item.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse moves over the item.")]
		public event System.Windows.Forms.MouseEventHandler MouseMove;

		/// <summary>
		/// Occurs when mouse remains still inside an item for an amount of time.
		/// </summary>
		[System.ComponentModel.Description("Occurs when mouse remains still inside an item for an amount of time.")]
		public event EventHandler MouseHover;

		/// <summary>
		/// Occurs when copy of the item is made.
		/// </summary>
		[System.ComponentModel.Description("Occurs when copy of the item is made.")]
		public event EventHandler ItemCopied;

		/// <summary>
		/// Occurs when Text property of an Item has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Text property of an Item has changed.")]
		public event EventHandler TextChanged;

		/// <summary>
		/// Occurs when Visible property of an Item has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Visible property of an Item has changed.")]
		public event EventHandler VisibleChanged;

		/// <summary>
		/// Occurs when Enabled property of an Item has changed.
		/// </summary>
		[System.ComponentModel.Description("Occurs when Enabled property of an Item has changed.")]
		public event EventHandler EnabledChanged;

        ///// <summary>
        ///// Occurs when component is Disposed.
        ///// </summary>
        //[System.ComponentModel.Description("Occurs when component is Disposed.")]
        //public event EventHandler Disposed;

        /// <summary>
        /// Occurs when item's tooltip visibility has changed.
        /// </summary>
        [System.ComponentModel.Description("Occurs when item's tooltip visibility has changed.")]
        public event EventHandler ToolTipVisibleChanged;
//		/// <summary>
//		/// Occurs when actual visiblity on screen of an item has changed i.e. Displayed property change.
//		/// </summary>
//		[System.ComponentModel.Description("Occurs when actual visiblity on screen of an item has changed.")]
//		public event EventHandler DisplayedChanged;
        /// <summary>
        /// Occurs when content of SubItems collection has changed. 
        /// </summary>
        [Description("Occurs when content of SubItems collection has changed")]
        public event CollectionChangeEventHandler SubItemsChanged;
		#endregion

		#region Private variables
		protected string m_Text;
		protected System.Drawing.Rectangle m_Rect;
		protected SubItemsCollection m_SubItems;
		protected bool m_Visible;
		protected BaseItem m_HotSubItem;  // Sub Item with Mouse over it
		protected bool m_IsContainer;
		protected BaseItem m_Parent;
		protected bool m_NeedRecalcSize;
		protected bool m_Expanded;
		protected bool m_AutoExpand;  // If this is a container should it expand automaticly its child items
		protected object m_ContainerControl;
		protected bool m_Displayed; // Set to true when item is shown on screen
		protected object m_ItemData;
		protected bool m_Enabled;
		protected bool m_BeginGroup;
		protected eDotNetBarStyle m_Style;
		protected string m_Description;				// Item description
		protected string m_Tooltip;					// Tooltip
		protected string m_Name="";	// Unique Item name that can be used to access item
        private string m_GlobalName = "";
		protected string m_Category;
		protected eOrientation m_Orientation;
		protected bool m_SystemItem;
		protected eSupportedOrientation m_SupportedOrientation=eSupportedOrientation.Both;
		protected bool m_RecalculatingSize=false;
        protected bool m_CheckMouseMovePressed = false;
		private bool m_CanCustomize;
		private int m_Id;
		private bool m_ShowSubItems;
		private bool m_Focused;
		private bool m_Stretch=false;
        internal Rectangle m_ViewRectangle;

		protected ToolTip m_ToolTipWnd;
		private static int s_IdCounter;
		private bool m_IsOnCustomizeMenu;
		private bool m_IsOnCustomizeDialog;
		protected char m_AccessKey;
		private bool m_DesignMode;
		private eItemAlignment m_ItemAlignment; // Item alignment in containers that support alignment

		private ShortcutsCollection m_Shortcuts;
        private string m_ShortcutString = "";

		private System.Drawing.Point m_MouseDownPt;

		private object m_Owner;

		private bool m_ClickAutoRepeat=false;
		private int m_ClickRepeatInterval=600;
		private bool m_AutoCollapseOnClick=true;
		private bool m_SuspendLayout=false;
        private bool m_IsRightToLeft = false;
		//private System.ComponentModel.ISite m_ComponentSite=null;
		protected bool m_ThemeAware=false;

		// When item is parented to the Items collection GlobalItem is set to true.
		// This flag is used to propagate property changes to all items with the same name.
		// Setting for example Visible property on the item that has GlobalItem true will
		// set visible property to the same value on all items with the same name.
		private bool m_GlobalItem=true;

		private eDesignInsertPosition m_DesignInsertMarker=eDesignInsertPosition.None;

		private System.Windows.Forms.Cursor m_Cursor=null;
		private System.Windows.Forms.Cursor m_ContainerCursor=null;
		protected bool m_ShouldSerialize=true;
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool _IgnoreClick=false; 
		protected bool m_AllowOnlyOneSubItemExpanded=true;  // Controls whether only ONE sub item can be expanded at a time, see InternalMouseDown

		//private System.Windows.Forms.AccessibleObject m_Accessible=null;
		private string m_AccessibleDefaultActionDescription="";
		private System.Windows.Forms.AccessibleRole m_AccessibleRole=System.Windows.Forms.AccessibleRole.Default;
		private string m_AccessibleName="";
		private string m_AccessibleDescription="";
		private bool m_IsAccessible=true;

		// Used to save extended information about item customization...
		private string m_OriginalBarName="";
		private int m_OriginalPosition=-1;
        private string m_KeyTips = "";
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point DesignTimeMouseDownPoint = Point.Empty;
        internal bool RenderText = true;
        internal bool MouseUpNotification = false;
        internal bool MouseDownCapture = false; // Indicates whether item will receive mouse messages if it is pressed and mouse leaves the item
        private bool m_UserCustomized = false;
        private bool m_IsDisposed = false;
		#endregion

        #region Constructor
        /// <summary>
		/// Creates new instance of BaseItem.
		/// </summary>
		public BaseItem():this("","") {}
		/// <summary>
		/// Creates new instance of BaseItem and assigns item name.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public BaseItem(string sItemName):this(sItemName,""){}
		/// <summary>
		/// Creates new instance of BaseItem and assigns item name and item text.
		/// </summary>
		/// <param name="sItemName">Item Name</param>
		/// <param name="ItemText">Item Text</param>
		public BaseItem(string sItemName, string ItemText)
		{
            if (ItemText == null)
                ItemText = "";
			m_Text=ItemText;
			if(m_Text!="")
				m_AccessKey=NativeFunctions.GetAccessKey(m_Text);
			m_Rect=System.Drawing.Rectangle.Empty;
			m_ContainerControl=null;
			m_HotSubItem=null;
			m_IsContainer=false;
			m_SubItems=null;
			m_Parent=null;
			m_NeedRecalcSize=true;
			m_Visible=true;
			m_Expanded=false;
			m_AutoExpand=false;
			m_ItemData="";
			m_Enabled=true;
			m_BeginGroup=false;
			m_Style=eDotNetBarStyle.OfficeXP;
			m_Description="";
			m_Tooltip="";
			m_Name=sItemName;
			m_Category="";
			m_Orientation=eOrientation.Horizontal;
			m_ToolTipWnd=null;
			m_Id=++s_IdCounter;
			m_IsOnCustomizeMenu=false;
			m_IsOnCustomizeDialog=false;
			m_SystemItem=false;
			//m_AccessKey=;
			m_Shortcuts=null;
			m_DesignMode=false;
			m_CanCustomize=true;
			m_ShowSubItems=true;
			m_ItemAlignment=eItemAlignment.Near;
			m_Focused=false;
			m_Owner=null;
            MarkupTextChanged();
        }
        #endregion

        #region Serialization
        /// <summary>
		/// Serializes the item and all sub-items into the XmlElement.
		/// </summary>
		/// <param name="ThisItem">XmlElement to serialize the item to.</param>
		protected internal virtual void Serialize(ItemSerializationContext context)
		{
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			Type t=this.GetType();
			System.Reflection.AssemblyName aname=t.Assembly.GetName();
			if(aname.Name!="DevComponents.DotNetBar")
				ThisItem.SetAttribute("assembly",aname.Name+", PublicKeyToken="+System.Text.Encoding.ASCII.GetString(aname.GetPublicKey()));
			else
				ThisItem.SetAttribute("assembly",aname.Name);

			ThisItem.SetAttribute("class",t.FullName);

			ThisItem.SetAttribute("name",m_Name);
            ThisItem.SetAttribute("globalname", m_GlobalName);
			ThisItem.SetAttribute("text",m_Text);
			ThisItem.SetAttribute("iscontainer",System.Xml.XmlConvert.ToString(m_IsContainer));
			ThisItem.SetAttribute("visible",System.Xml.XmlConvert.ToString(m_Visible));
			if(m_ItemData!=null && m_ItemData.ToString()!="")
				ThisItem.SetAttribute("itemdata",m_ItemData.ToString());
			else
				ThisItem.SetAttribute("itemdata","");
			ThisItem.SetAttribute("enabled",System.Xml.XmlConvert.ToString(m_Enabled));
			ThisItem.SetAttribute("begingroup",System.Xml.XmlConvert.ToString(m_BeginGroup));
			ThisItem.SetAttribute("style",System.Xml.XmlConvert.ToString((int)m_Style));
			ThisItem.SetAttribute("desc",m_Description);
			ThisItem.SetAttribute("tooltip",m_Tooltip);
			ThisItem.SetAttribute("category",m_Category);
			ThisItem.SetAttribute("cancustomize",System.Xml.XmlConvert.ToString(m_CanCustomize));
			ThisItem.SetAttribute("showsubitems",System.Xml.XmlConvert.ToString(m_ShowSubItems));
			ThisItem.SetAttribute("itemalignment",System.Xml.XmlConvert.ToString((int)m_ItemAlignment));
			ThisItem.SetAttribute("stretch",System.Xml.XmlConvert.ToString(m_Stretch));
			ThisItem.SetAttribute("global",System.Xml.XmlConvert.ToString(m_GlobalItem));
			ThisItem.SetAttribute("themes",System.Xml.XmlConvert.ToString(m_ThemeAware));
			if(m_AccessibleDefaultActionDescription!="")
                ThisItem.SetAttribute("adefdesc",m_AccessibleDefaultActionDescription);
			if(m_AccessibleDescription!="")
				ThisItem.SetAttribute("adesc",m_AccessibleDescription);
			if(m_AccessibleName!="")
				ThisItem.SetAttribute("aname",m_AccessibleName);
			if(m_AccessibleRole!=System.Windows.Forms.AccessibleRole.Default)
				ThisItem.SetAttribute("arole",System.Xml.XmlConvert.ToString((int)m_AccessibleRole));
			if(!m_AutoCollapseOnClick)
				ThisItem.SetAttribute("autocollapse",System.Xml.XmlConvert.ToString(m_AutoCollapseOnClick));
			if(m_ClickAutoRepeat)
				ThisItem.SetAttribute("autorepeat",System.Xml.XmlConvert.ToString(m_ClickAutoRepeat));
			if(m_ClickRepeatInterval!=600)
				ThisItem.SetAttribute("clickrepinterv",System.Xml.XmlConvert.ToString(m_ClickRepeatInterval));
            //if(m_GenerateCommandLink)
            //    ThisItem.SetAttribute("GenerateCommandLink", System.Xml.XmlConvert.ToString(m_GenerateCommandLink));
                

			if(m_Cursor!=null)
			{
				if(m_Cursor==System.Windows.Forms.Cursors.Hand)
					ThisItem.SetAttribute("cur","4");
				else if(m_Cursor==System.Windows.Forms.Cursors.AppStarting)
					ThisItem.SetAttribute("cur","1");
				else if(m_Cursor==System.Windows.Forms.Cursors.Arrow)
					ThisItem.SetAttribute("cur","2");
				else if(m_Cursor==System.Windows.Forms.Cursors.Cross)
					ThisItem.SetAttribute("cur","3");
				else if(m_Cursor==System.Windows.Forms.Cursors.Help)
					ThisItem.SetAttribute("cur","5");
				else if(m_Cursor==System.Windows.Forms.Cursors.HSplit)
					ThisItem.SetAttribute("cur","6");
				else if(m_Cursor==System.Windows.Forms.Cursors.IBeam)
					ThisItem.SetAttribute("cur","7");
				else if(m_Cursor==System.Windows.Forms.Cursors.No)
					ThisItem.SetAttribute("cur","8");
				else if(m_Cursor==System.Windows.Forms.Cursors.NoMove2D)
					ThisItem.SetAttribute("cur","9");
				else if(m_Cursor==System.Windows.Forms.Cursors.NoMoveHoriz)
					ThisItem.SetAttribute("cur","10");
				else if(m_Cursor==System.Windows.Forms.Cursors.NoMoveVert)
					ThisItem.SetAttribute("cur","11");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanEast)
					ThisItem.SetAttribute("cur","12");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanNE)
					ThisItem.SetAttribute("cur","13");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanNorth)
					ThisItem.SetAttribute("cur","14");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanNW)
					ThisItem.SetAttribute("cur","15");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanSE)
					ThisItem.SetAttribute("cur","16");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanSouth)
					ThisItem.SetAttribute("cur","17");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanSW)
					ThisItem.SetAttribute("cur","18");
				else if(m_Cursor==System.Windows.Forms.Cursors.PanWest)
					ThisItem.SetAttribute("cur","19");
				else if(m_Cursor==System.Windows.Forms.Cursors.SizeAll)
					ThisItem.SetAttribute("cur","20");
				else if(m_Cursor==System.Windows.Forms.Cursors.SizeNESW)
					ThisItem.SetAttribute("cur","21");
				else if(m_Cursor==System.Windows.Forms.Cursors.SizeNS)
					ThisItem.SetAttribute("cur","22");
				else if(m_Cursor==System.Windows.Forms.Cursors.SizeNWSE)
					ThisItem.SetAttribute("cur","23");
				else if(m_Cursor==System.Windows.Forms.Cursors.SizeWE)
					ThisItem.SetAttribute("cur","24");
				else if(m_Cursor==System.Windows.Forms.Cursors.UpArrow)
					ThisItem.SetAttribute("cur","25");
				else if(m_Cursor==System.Windows.Forms.Cursors.VSplit)
					ThisItem.SetAttribute("cur","26");
				else if(m_Cursor==System.Windows.Forms.Cursors.WaitCursor)
					ThisItem.SetAttribute("cur","27");
			}

			if(SubItems.Count>0 && this.ShouldSerializeSubItems())
			{
				System.Xml.XmlElement subitems=ThisItem.OwnerDocument.CreateElement("subitems");
				ThisItem.AppendChild(subitems);
				foreach(BaseItem objItem in this.SubItems)
				{
					if(objItem.ShouldSerialize)
					{
						System.Xml.XmlElement ChildItem=ThisItem.OwnerDocument.CreateElement("item");
						t=objItem.GetType();
						subitems.AppendChild(ChildItem);
                        context.ItemXmlElement = ChildItem;
						objItem.Serialize(context);
                        context.ItemXmlElement = ThisItem;
					}
				}
			}

			// Serialize Shortcuts
			if(m_Shortcuts!=null && m_Shortcuts.Count>0)
				ThisItem.SetAttribute("shortcuts",m_Shortcuts.ToString(","));

            if (context.HasSerializeItemHandlers)
            {
                System.Xml.XmlElement customData = ThisItem.OwnerDocument.CreateElement("customData");
                SerializeItemEventArgs e = new SerializeItemEventArgs(this, ThisItem, customData);
                context.Serializer.InvokeSerializeItem(e);
                if (customData.Attributes.Count > 0 || customData.ChildNodes.Count > 0)
                    ThisItem.AppendChild(customData);
            }
		}

		/// <summary>
		/// Deserialize the Item from the XmlElement.
		/// </summary>
		/// <param name="ItemXmlSource">Source XmlElement.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void Deserialize(ItemSerializationContext context)
		{
            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;

			m_Name=ItemXmlSource.GetAttribute("name");
            if (ItemXmlSource.HasAttribute("globalname"))
                m_GlobalName = ItemXmlSource.GetAttribute("globalname");
			m_Text=ItemXmlSource.GetAttribute("text");
			m_AccessKey=NativeFunctions.GetAccessKey(m_Text);
			m_IsContainer=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("iscontainer"));
            if (context._DesignerHost == null)
                m_Visible = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("visible"));
            else
                TypeDescriptor.GetProperties(this)["Visible"].SetValue(this, System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("visible")));
			if(ItemXmlSource.GetAttribute("itemdata")!="")
				m_ItemData=ItemXmlSource.GetAttribute("itemdata");
			else
				m_ItemData="";
			m_Enabled=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("enabled"));
			m_BeginGroup=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("begingroup"));
			m_Style=(eDotNetBarStyle)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("style"));
			m_Description=ItemXmlSource.GetAttribute("desc");
			m_Tooltip=ItemXmlSource.GetAttribute("tooltip");
			m_Category=ItemXmlSource.GetAttribute("category");
			m_CanCustomize=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("cancustomize"));
			m_ShowSubItems=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("showsubitems"));
			m_ItemAlignment=(eItemAlignment)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("itemalignment"));
			if(ItemXmlSource.HasAttribute("stretch"))
				m_Stretch=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("stretch"));
			else
				m_Stretch=false;
			if(ItemXmlSource.HasAttribute("global"))
				m_GlobalItem=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("global"));

			if(ItemXmlSource.HasAttribute("themes"))
				m_ThemeAware=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("themes"));

			if(ItemXmlSource.HasAttribute("adefdesc"))
				m_AccessibleDefaultActionDescription=ItemXmlSource.GetAttribute("adefdesc");
			if(ItemXmlSource.HasAttribute("adesc"))
				m_AccessibleDescription=ItemXmlSource.GetAttribute("adesc");
			if(ItemXmlSource.HasAttribute("aname"))
				m_AccessibleName=ItemXmlSource.GetAttribute("aname");
			if(ItemXmlSource.HasAttribute("arole"))
				m_AccessibleRole=(System.Windows.Forms.AccessibleRole)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("arole"));
			if(ItemXmlSource.HasAttribute("autocollapse"))
				m_AutoCollapseOnClick=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("autocollapse"));
			else
                m_AutoCollapseOnClick=true;
			
			if(ItemXmlSource.HasAttribute("autorepeat"))
				m_ClickAutoRepeat=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("autorepeat"));
			else
				m_ClickAutoRepeat=false;
			if(ItemXmlSource.HasAttribute("clickrepinterv"))
				m_ClickRepeatInterval=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("clickrepinterv"));
			else
				m_ClickRepeatInterval=600;

            //if (ItemXmlSource.HasAttribute("GenerateCommandLink"))
            //    m_GenerateCommandLink = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("GenerateCommandLink"));
            //else
            //    m_GenerateCommandLink = false;
			
			// Load Cursor
			if(ItemXmlSource.HasAttribute("cur"))
			{
				switch(ItemXmlSource.GetAttribute("cur"))
				{
					case "4":
						m_Cursor=System.Windows.Forms.Cursors.Hand;
						break;
					case "1":
						m_Cursor=System.Windows.Forms.Cursors.AppStarting;
						break;
					case "2":
						m_Cursor=System.Windows.Forms.Cursors.Arrow;
						break;
					case "3":
						m_Cursor=System.Windows.Forms.Cursors.Cross;
						break;
					case "5":
						m_Cursor=System.Windows.Forms.Cursors.Help;
						break;
					case "6":
						m_Cursor=System.Windows.Forms.Cursors.HSplit;
						break;
					case "7":
						m_Cursor=System.Windows.Forms.Cursors.IBeam;
						break;
					case "8":
						m_Cursor=System.Windows.Forms.Cursors.No;
						break;
					case "9":
						m_Cursor=System.Windows.Forms.Cursors.NoMove2D;
						break;
					case "10":
						m_Cursor=System.Windows.Forms.Cursors.NoMoveHoriz;
						break;
					case "11":
						m_Cursor=System.Windows.Forms.Cursors.NoMoveVert;
						break;
					case "12":
						m_Cursor=System.Windows.Forms.Cursors.PanEast;
						break;
					case "13":
						m_Cursor=System.Windows.Forms.Cursors.PanNE;
						break;
					case "14":
						m_Cursor=System.Windows.Forms.Cursors.PanNorth;
						break;
					case "15":
						m_Cursor=System.Windows.Forms.Cursors.PanNW;
						break;
					case "16":
						m_Cursor=System.Windows.Forms.Cursors.PanSE;
						break;
					case "17":
						m_Cursor=System.Windows.Forms.Cursors.PanSouth;
						break;
					case "18":
						m_Cursor=System.Windows.Forms.Cursors.PanSW;
						break;
					case "19":
						m_Cursor=System.Windows.Forms.Cursors.PanWest;
						break;
					case "20":
						m_Cursor=System.Windows.Forms.Cursors.SizeAll;
						break;
					case "21":
						m_Cursor=System.Windows.Forms.Cursors.SizeNESW;
						break;
					case "22":
						m_Cursor=System.Windows.Forms.Cursors.SizeNS;
						break;
					case "23":
						m_Cursor=System.Windows.Forms.Cursors.SizeNWSE;
						break;
					case "24":
						m_Cursor=System.Windows.Forms.Cursors.SizeWE;
						break;
					case "25":
						m_Cursor=System.Windows.Forms.Cursors.UpArrow;
						break;
					case "26":
						m_Cursor=System.Windows.Forms.Cursors.VSplit;
						break;
					case "27":
						m_Cursor=System.Windows.Forms.Cursors.WaitCursor;
						break;
				}
			}

			System.Xml.XmlNodeList list=ItemXmlSource.GetElementsByTagName("subitems");
			if(list.Count>0)
			{
				foreach(System.Xml.XmlElement xmlChild in list[0].ChildNodes)
				{
//					BaseItem oi=null;
//					System.Reflection.Assembly a=System.Reflection.Assembly.Load(xmlChild.GetAttribute("assembly"));
//					if(a==null) continue;
//					oi=a.CreateInstance(xmlChild.GetAttribute("class")) as BaseItem;
					BaseItem oi=context.CreateItemFromXml(xmlChild);
					if(oi!=null)
					{
						this.SubItems.Add(oi);
                        context.ItemXmlElement = xmlChild;
						oi.Deserialize(context);
                        context.ItemXmlElement = ItemXmlSource;
					}
				}
			}
			if(ItemXmlSource.HasAttribute("shortcuts"))
			{
				if(m_Shortcuts==null)
					m_Shortcuts=new ShortcutsCollection(this);
				m_Shortcuts.FromString(ItemXmlSource.GetAttribute("shortcuts"),",");
			}

            if (context.HasDeserializeItemHandlers)
            {
                System.Xml.XmlNodeList customDataList=ItemXmlSource.GetElementsByTagName("customData");
                if (customDataList.Count > 0)
                {
                    System.Xml.XmlElement customData = customDataList[0] as System.Xml.XmlElement;
                    SerializeItemEventArgs e = new SerializeItemEventArgs(this, ItemXmlSource, customData);
                    context.Serializer.InvokeDeserializeItem(e);
                }
            }

            MarkupTextChanged();
        }

        /// <summary>
        /// Indicates whether SubItems collection is serialized. Default value is true.
        /// </summary>
        protected virtual bool ShouldSerializeSubItems()
        {
            return true;
        }
        #endregion

        #region Internal Implementation

        /// <summary>
		/// Called when item container has changed. If you override this method you must call the base implementation to allow default processing to occur.
		/// </summary>
		/// <param name="objOldContainer">Previous container of the item.</param>
		protected internal virtual void OnContainerChanged(object objOldContainer)
		{
			if(m_SubItems==null || !this.IsContainer)
				return;
			foreach(BaseItem objItem in m_SubItems)
				objItem.OnContainerChanged(objOldContainer);
		}

		/// <summary>
		/// Control Container (System.Windows.Forms.Control or its descendant)
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual object ContainerControl
		{
			get
			{
                return GetContainerControl(true);
                //if(m_Parent==null)
                //    return m_ContainerControl;

                //if(m_ContainerControl==null && m_Parent.IsContainer)
                //    return m_Parent.ContainerControl;

                //return m_ContainerControl;
			}
			set
			{
				object oldContainer=this.ContainerControl;
				m_ContainerControl=value;
				//if(oldContainer!=this.ContainerControl)
				OnContainerChanged(oldContainer);
			}
		}

        internal object GetContainerControl(bool checkIsContainer)
        {
            if (m_Parent == null)
                return m_ContainerControl;

            if (m_ContainerControl == null && (m_Parent.IsContainer || !checkIsContainer))
                return m_Parent.GetContainerControl(checkIsContainer);

            return m_ContainerControl;
        }

		/// <summary>
		/// Returns the Parent of the item.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual BaseItem Parent
		{
			get
			{
				if(this.ContainerControl is Bar)
				{
					Bar bar=this.ContainerControl as Bar;
					if(bar.ParentItem==this)
						return null;
					if(m_Parent==bar.ItemsContainer)
						return bar.ParentItem;
				}
				return m_Parent;
			}
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void SetParent(BaseItem o)
		{
			m_Parent=o;
            // Do not set recalc size flag on parent...
			m_NeedRecalcSize=true;
            OnParentChanged();
		}
        /// <summary>
        /// Called when item parent has changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
            
        }

        /// <summary>
        /// Returns whether item is enabled including the parent control item is on.
        /// </summary>
        /// <returns></returns>
        internal bool GetEnabled(System.Windows.Forms.Control container)
        {
            if (!m_Enabled || container == null) return m_Enabled;
            return container.Enabled && m_Enabled;
        }

        /// <summary>
        /// Returns whether item is enabled including the parent control item is on.
        /// </summary>
        /// <returns></returns>
        internal bool GetEnabled()
        {
            return GetEnabled(GetContainerControl(false) as Control);
        }

		/// <summary>
		/// Gets or sets a value indicating whether the item is enabled.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether is item enabled.")]
		public virtual bool Enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				if(m_Enabled!=value)
				{
					m_Enabled=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "Enabled");

					if(!m_Enabled && m_Expanded)
					{
						this.Expanded=false;
						//CollapseAll(this);
					}
					this.Refresh();
					this.OnEnabledChanged();
					OnAppearanceChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets whether item separator is shown before this item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates whether this item is beginning of the group.")]
		public virtual bool BeginGroup
		{
			get
			{
				return m_BeginGroup;
			}
			set
			{
				if(m_BeginGroup!=value)
				{
					NeedRecalcSize=true;
					m_BeginGroup=value;

                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "BeginGroup");

					this.Refresh();
					OnAppearanceChanged();
				}
			}
		}


		/// <summary>
		/// Gets or sets Left position of this item
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual int LeftInternal
		{
			get
			{
				return m_Rect.Left;
			}
			set
			{
				if(m_Rect.X!=value)
				{
					int old=m_Rect.X;
					m_Rect.X=value;
					this.OnLeftLocationChanged(old);
				}
			}
		}

		/// <summary>
		/// Gets or sets Top position of this item
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual int TopInternal
		{
			get
			{
				return m_Rect.Top;
			}
			set
			{
				if(m_Rect.Y!=value)
				{
					int oldValue=m_Rect.Y;
					m_Rect.Y=value;
					this.OnTopLocationChanged(oldValue);
				}
			}
		}

		/// <summary>
		/// Gets or sets Width of this item
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual int WidthInternal
		{
			get
			{
				return m_Rect.Width;
			}
			set
			{
				if(m_Rect.Width!=value)
				{
					m_Rect.Width=value;
					OnExternalSizeChange();
				}
			}
		}

        /// <summary>
        /// Gets whether item is in right-to-left layout mode.
        /// </summary>
        protected virtual internal bool IsRightToLeft
        {
            get { return m_IsRightToLeft; }
            set
            {
                if (m_IsRightToLeft == value)
                    return;
                m_IsRightToLeft = value;
                foreach (BaseItem item in this.SubItems)
                {
                    item.IsRightToLeft = value;
                }
            }
        }

		/// <summary>
		/// Called when size of the item is changed externaly.
		/// </summary>
		protected virtual void OnExternalSizeChange(){}

		/// <summary>
		/// Gets or sets Height of this item
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual int HeightInternal
		{
			get
			{
				return m_Rect.Height;
			}
			set
			{
				if(m_Rect.Height!=value)
				{
					m_Rect.Height=value;
					OnExternalSizeChange();
				}
			}
		}

		/// <summary>
		/// Called when Visibility of the items has changed.
		/// </summary>
		/// <param name="newValue">New Visible state.</param>
		protected internal virtual void OnVisibleChanged(bool newValue)
		{
			if(this.VisibleChanged!=null)
				this.VisibleChanged(this,new EventArgs());
            if (this.DesignMode)
            {
                if (this.Parent != null)
                {
                    this.Parent.NeedRecalcSize = true;
                    this.Parent.Refresh();
                }
            }
            else if (this.Parent != null && this.Parent is ItemContainer)
                this.Parent.NeedRecalcSize = true;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is visible.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Determines whether the item is visible or hidden.")]
		public virtual bool Visible
		{
			get
			{
				return m_Visible;
			}
			set
			{
				if(m_Visible==value)
					return;

				m_Visible=value;
                this.NeedRecalcSize = true;
                if(ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "Visible");

				if(!m_Visible && m_Displayed && m_Parent!=null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
				if(!m_IsOnCustomizeMenu)
					this.Displayed=false;

				OnVisibleChanged(value);
			}
		}

        internal void SetVisibleDirect(bool b)
        {
            m_Visible = b;
            if (!m_Visible && !m_IsOnCustomizeMenu)
                m_Displayed = false;
        }

		/// <summary>
		/// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual bool Expanded
		{
            get { return m_Expanded; }

		    set
			{
				if(m_Expanded!=value)
				{
					m_Expanded = value;

					OnExpandChange();

					if(m_Parent!=null)
						m_Parent.OnSubItemExpandChange(this);
				}
			}
		}

		/// <summary>
		/// Indicates whether the item will auto-collapse (fold) when clicked. 
		/// When item is on popup menu and this property is set to false, menu will not
		/// close when item is clicked.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.DefaultValue(true),System.ComponentModel.Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
		public virtual bool AutoCollapseOnClick
		{
			get
			{
				return m_AutoCollapseOnClick;
			}
			set
			{
				m_AutoCollapseOnClick=value;
			}
		}

		/// <summary>
		/// Gets or sets whether item will auto expand when mouse is over the item or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual bool AutoExpand
		{
			get
			{
				return m_AutoExpand;
			}
			set
			{
				if(m_AutoExpand==value)
					return;
				m_AutoExpand=value;
                if(ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "AutoExpand");
			}
		}

		/// <summary>
		/// Gets the rectangle that represents the display area of the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual System.Drawing.Rectangle DisplayRectangle
		{
			get
			{
				return m_Rect;
			}
		}

		/// <summary>
		/// Gets or sets the size of the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual System.Drawing.Size Size
		{
			get {return m_Rect.Size;}
			set
			{
				if(m_Rect.Size!=value)
				{
					m_Rect.Size=value;
					OnExternalSizeChange();
				}
			}
		}

        /// <summary>
        /// IBlock member implementation
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public virtual System.Drawing.Rectangle Bounds
        {
            get { return m_Rect; }
            set
            {
                if (m_Rect != value)
                {
                    bool sizeChanged = m_Rect.Size != value.Size;
                    bool topLocationChanged = m_Rect.Top != value.Top;
                    bool leftLocationChanged = m_Rect.Left != value.Left;
                    int oldLeft = m_Rect.Left;
                    m_Rect = value;
                    if(sizeChanged)
                        OnExternalSizeChange();
                    //if (leftLocationChanged)
                    //    OnLeftLocationChanged(oldLeft);
                }
            }
        }

        internal void SetDisplayRectangle(Rectangle r)
        {
            m_Rect = r;
        }

        internal void SetViewRectangle(Rectangle r)
        {
            m_ViewRectangle = r;
        }

		/// <summary>
		/// Specifies whether this item is visual container or not. For example
		/// Tool Menu is not container since it drops-down its items and they are
		/// not "visualy" contained. Also, the pop-up menus, drop-down Bars etc. are not containers.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual bool IsContainer
		{
			get
			{
				return m_IsContainer;
			}
		}

		protected internal void SetIsContainer(bool b)
		{
			m_IsContainer=b;
		}

		/// <summary>
		/// Returns true if this item is currently displayed. This property should not be set directly since it is managed by system and container of the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual bool Displayed
		{
			get
			{
				return m_Displayed;
			}
			set
			{
				if(m_Displayed!=value)
				{
					m_Displayed=value;
					OnDisplayedChanged();
				}
			}
		}

		/// <summary>
		/// Called when item Display state has changed.
		/// </summary>
		protected virtual void OnDisplayedChanged()
		{
//			if(DisplayedChanged!=null)
//				DisplayedChanged(this,new EventArgs());
			if(this.Displayed && this.Visible)
				this.GenerateAccessibilityEvent(System.Windows.Forms.AccessibleEvents.Show);
			else
				this.GenerateAccessibilityEvent(System.Windows.Forms.AccessibleEvents.Hide);
//			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
//			if(owner!=null)
//				owner.InvokeItemDisplayedChanged(this,new EventArgs());
		}

		protected internal virtual void OnProcessDelayedCommands(){}

		/// <summary>
		/// Called when item owner has changed.
		/// </summary>
		protected virtual void OnOwnerChanged() {}

        /// <summary>
        /// Raises SubItemsChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSubItemsChanged(CollectionChangeEventArgs e)
        {
            CollectionChangeEventHandler eh = SubItemsChanged;
            if (eh != null) eh(this, e);
        }

		/// <summary>
		/// Occurs after an item has been added to the container. This procedure is called on both item being added and the parent of the item. To distinguish between those two states check the item parameter.
		/// </summary>
		/// <param name="item">When occurring on the parent this will hold the reference to the item that has been added. When occurring on the item being added this will be null (Nothing).</param>
		protected internal virtual void OnItemAdded(BaseItem item)
		{
            if (SubItemsChanged != null) OnSubItemsChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null)
				owner.InvokeItemAdded(item,new EventArgs());
		}
		/// <summary>
		/// Occurs after an item has been removed.
		/// </summary>
		/// <param name="item">Item being removed.</param>
		protected internal virtual void OnAfterItemRemoved(BaseItem item)
		{
			if(item==null)
				return;
            if (SubItemsChanged != null) OnSubItemsChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));

			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null)
				owner.InvokeItemRemoved(item,this);
		}

		/// <summary>
		/// Occurs just before Click event is fired.
		/// </summary>
		protected virtual void OnClick(){}

		/// <summary>
		/// Occurs after SubItems Collection has been cleared.
		/// </summary>
		protected internal virtual void OnSubItemsClear() {}
		/// <summary>
		/// Occurs before an item is removed.
		/// </summary>
		/// <param name="item">Item being removed.</param>
		protected internal virtual void OnBeforeItemRemoved(BaseItem item) {}
		/// <summary>
		/// Occurs when IsOnCustomizeMenu property has changed.
		/// </summary>
		protected virtual void OnIsOnCustomizeMenuChanged(){}
		/// <summary>
		/// Occurs when IsOnCustomizeDialogChanged property has changed.
		/// </summary>
		protected virtual void OnIsOnCustomizeDialogChanged(){}
		/// <summary>
		/// Occurs when item enter or exists the design mode.
		/// </summary>
		protected virtual void OnDesignModeChanged(){}
		/// <summary>
		/// Occurs when tooltip is about to be shown or hidden.
		/// </summary>
		/// <param name="bShow">Specifies whether tooltip is shown or hidden.</param>
		protected virtual void OnTooltip(bool bShow){}
		/// <summary>
		/// Occurs after item has received the input focus.
		/// </summary>
		/// <param name="item">Item that received the focus.</param>
		protected virtual void OnSubItemGotFocus(BaseItem item){}
		/// <summary>
		/// Occurs after item has lost the input focus.
		/// </summary>
		/// <param name="item">Item that lost the input focus.</param>
		protected virtual void OnSubItemLostFocus(BaseItem item){}
		/// <summary>
		/// Indicates whether the item enabled property has changed.
		/// </summary>
		protected virtual void OnEnabledChanged()
		{
			if(this.EnabledChanged!=null)
				this.EnabledChanged(this,new EventArgs());
		}
		/// <summary>
		/// Called after TopInternal property has changed
		/// </summary>
		protected virtual void OnTopLocationChanged(int oldValue){}

		/// <summary>
		/// Called after LeftInternal property has changed
		/// </summary>
		protected virtual void OnLeftLocationChanged(int oldValue){}

		/// <summary>
		/// Occurs when the mouse pointer enters the item. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalMouseEnter()
		{
			if(!m_DesignMode)
			{
				if(MouseEnter!=null)
					MouseEnter(this,new EventArgs());
				IOwnerItemEvents owner=this.GetIOwnerItemEvents();
				if(owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
					owner.InvokeMouseEnter(this,new EventArgs());
			}
			if(!this.DesignMode)
			{
				ResetHover();

				System.Windows.Forms.Control objContainer=this.ContainerControl as System.Windows.Forms.Control;

				if(m_Cursor!=null && objContainer!=null)
				{
					m_ContainerCursor=objContainer.Cursor;
					objContainer.Cursor=m_Cursor;
				}

				// Accessibility Narrator support for menu items ONLY
                if (BarUtilities.AlwaysGenerateAccessibilityFocusEvent && !(objContainer is MenuPanel) || objContainer is Bar && this.IsOnMenuBar)
				{
					Bar.BarAccessibleObject acc=objContainer.AccessibilityObject as Bar.BarAccessibleObject;
                    if (acc != null)
                    {
                        //acc.GenerateEvent(this,System.Windows.Forms.AccessibleEvents.SystemMenuStart);
                        acc.GenerateEvent(this, System.Windows.Forms.AccessibleEvents.Focus);
                    }
                    else
                    {
                        ItemControlAccessibleObject ic = objContainer.AccessibilityObject as ItemControlAccessibleObject;
                        if (ic != null)
                            ic.GenerateEvent(this, System.Windows.Forms.AccessibleEvents.Focus);
                    }
				}
				else if(objContainer is MenuPanel && !((MenuPanel)objContainer).IsDisposed)
				{
					MenuPanel.PopupMenuAccessibleObject a=objContainer.AccessibilityObject as MenuPanel.PopupMenuAccessibleObject;
					if(a!=null)
					{
						a.GenerateEvent(this,System.Windows.Forms.AccessibleEvents.Focus);
					}
				}
			}			
		}

		/// <summary>
		/// Occurs when the mouse pointer hovers the item. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalMouseHover()
		{
			if(m_DesignMode)
				return;

			if(m_HotSubItem!=null)
			{
				if(!m_AutoExpand)
				{
					BaseItem objItem=ExpandedItem();
					if(objItem!=null && m_HotSubItem!=objItem && objItem.Visible && (this.IsOnMenu || this.ContainerControl is Bar || this.ContainerControl is RibbonBar))
						objItem.Expanded=false;
				}
				// Changing the Expanded of the item can cause hot sub item to be set to null
				if(m_HotSubItem!=null)
					m_HotSubItem.InternalMouseHover();
			}
			else
			{
				if(System.Windows.Forms.Control.MouseButtons==System.Windows.Forms.MouseButtons.None)
					ShowToolTip();
				if(!m_DesignMode)
				{
					if(MouseHover!=null)
						MouseHover(this,new EventArgs());
					IOwnerItemEvents owner=this.GetIOwnerItemEvents();
					if(owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
						owner.InvokeMouseHover(this,new EventArgs());
				}
			}
		}

		/// <summary>
		/// Occurs when the mouse pointer leaves the item. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalMouseLeave()
		{
			if(m_ContainerCursor!=null)
			{
				System.Windows.Forms.Control objContainer=this.ContainerControl as System.Windows.Forms.Control;
				if(objContainer!=null)
				{
					objContainer.Cursor=m_ContainerCursor;
				}
				m_ContainerCursor=null;
			}

			if(m_DesignMode)
				return;

			// If we had hot sub item pass the mouse leave message to it...
			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseLeave();
                HotSubItem = null;
			}
			else
			{
                if (m_TextMarkup != null)
                    m_TextMarkup.MouseLeave(this.ContainerControl as System.Windows.Forms.Control);

				if(MouseLeave!=null)
					MouseLeave(this,new EventArgs());
				IOwnerItemEvents owner=this.GetIOwnerItemEvents();
				if(owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
					owner.InvokeMouseLeave(this,new EventArgs());
			}
			HideToolTip();
		}
        /// <summary>
        /// Gets the mouse down coordinates.
        /// </summary>
        internal Point MouseDownPt
        {
            get
            {
                return m_MouseDownPt;
            }
        }

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is pressed. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			IOwner owner=null;

			// Colapse any item if expanded
			m_MouseDownPt=new System.Drawing.Point(objArg.X,objArg.Y);
			BaseItem objItem=ExpandedItem();
			if(objItem!=null && objItem!=m_HotSubItem && !this.DesignMode && m_AllowOnlyOneSubItemExpanded)
			{
				objItem.Expanded=false;
				m_AutoExpand=false;
			}

			if(m_DesignMode && (m_CanCustomize || this.Site!=null && this.Site.DesignMode))// && !this.SystemItem)
			{
                if(this.IsContainer && this.SubItems.Count>0)
				{
					BaseItem objNew=ItemAtLocation(objArg.X,objArg.Y);

					if(objNew!=null && (objNew.CanCustomize || this.Site!=null && this.Site.DesignMode || objNew.Site!=null && objNew.Site.DesignMode))
					{
						owner=this.GetOwner() as IOwner;
						if(owner!=null)
							owner.SetFocusItem(objNew);
					}
					else if(objNew==null)
					{
						owner=this.GetOwner() as IOwner;
						if(owner!=null)
							owner.SetFocusItem(null);
					}

					if(objNew!=null && objNew!=this)
						objNew.InternalMouseDown(objArg);
				}
				
				if(objArg.Button==System.Windows.Forms.MouseButtons.Right && !m_IsContainer)
				{
					owner=this.GetOwner() as IOwner;
					if(owner!=null)
						owner.DesignTimeContextMenu(this);
				}
			}

			if(!this.DesignMode)
			{
				owner=this.GetOwner() as IOwner;
				if(owner!=null && owner.GetFocusItem()!=null)
				{
					BaseItem objNew=ItemAtLocation(objArg.X,objArg.Y);
					if(objNew!=owner.GetFocusItem())
						owner.GetFocusItem().ReleaseFocus();
				}
			}

			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseDown(objArg);
			}
			else if(!m_DesignMode)
			{
                if (m_TextMarkup != null)
                    m_TextMarkup.MouseDown(this.ContainerControl as System.Windows.Forms.Control, objArg);

				if(MouseDown!=null)
					MouseDown(this,objArg);
				//if(owner==null)
				//	owner=this.GetOwner() as IOwner;
				IOwnerItemEvents ownerEvents=this.GetIOwnerItemEvents();
				if(ownerEvents!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
					ownerEvents.InvokeMouseDown(this,objArg);
			}

			HideToolTip();
		}

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is released. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(m_HotSubItem!=null)
			{
				if(m_HotSubItem.DisplayRectangle.Contains(objArg.X,objArg.Y) || m_HotSubItem.MouseUpNotification)
					m_HotSubItem.InternalMouseUp(objArg);
			}
			else if(!m_DesignMode)
			{
                if (m_TextMarkup != null)
                    m_TextMarkup.MouseUp(this.ContainerControl as System.Windows.Forms.Control, objArg);
				if(MouseUp!=null)
					MouseUp(this,objArg);
				IOwnerItemEvents owner=this.GetIOwnerItemEvents();
				if(owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
					owner.InvokeMouseUp(this,objArg);
                if ((objArg.Button == System.Windows.Forms.MouseButtons.Left || objArg.Button == System.Windows.Forms.MouseButtons.Right && this.IsOnContextMenu) && this.DisplayRectangle.Contains(objArg.X, objArg.Y) && (this.DisplayRectangle.Contains(m_MouseDownPt) && this.GetEnabled() || this.IsOnMenu))
					RaiseClick(eEventSource.Mouse);
			}
			else
			{
				BaseItem objNew=ItemAtLocation(objArg.X,objArg.Y);
				if(objNew!=null && objNew!=this)
					objNew.InternalMouseUp(objArg);
			}

			m_MouseDownPt=System.Drawing.Point.Empty;
		}

        /// <summary>
        /// Gets whether item is on context menu created using ContextMenuBar
        /// </summary>
        [Browsable(false)]
        public bool IsOnContextMenu
        {
            get { return this.GetOwner() is ContextMenuBar; }
        }

		/// <summary>
		/// Occurs when a key is pressed down while the item has focus. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
		{
			if(m_DesignMode)
			{
				objArg.Handled=true;
				return;
			}

			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalKeyDown(objArg);
			}

			if(objArg.KeyCode==System.Windows.Forms.Keys.Enter  || objArg.KeyCode==System.Windows.Forms.Keys.Return)
			{
                if (!(this.IsOnMenuBar || (this.IsOnMenu || this.ShowSubItems) && this.SubItems.Count > 0) && this.GetEnabled())
				{
					RaiseClick(eEventSource.Keyboard);
					objArg.Handled=true;

                    if (this.GetOwner() is RibbonStrip || this.GetOwner() is RibbonBar)
                    {
                        ((ItemControl)this.GetOwner()).ReleaseFocus();
                        ((ItemControl)this.GetOwner()).MenuFocus = false;
                    }
				}
			}
		}

        /// <summary>
        /// Processes the MouseLeave for the current mouse over item.
        /// </summary>
        protected virtual void LeaveHotSubItem(BaseItem newMouseOverItem)
        {
            if (m_HotSubItem != null)
            {
                m_HotSubItem.InternalMouseLeave();
                //if(objNew!=null && m_AutoExpand && m_HotSubItem.Expanded)
                if (newMouseOverItem != null && m_HotSubItem.Expanded && (this.IsOnMenu || this.ContainerControl is Bar && ((Bar)this.ContainerControl).BarType == eBarType.MenuBar))
                    m_HotSubItem.Expanded = false;
                HotSubItem = null;
            }
        }

		/// <summary>
		/// Occurs when the mouse pointer is moved over the item. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(m_DesignMode && objArg.Button==System.Windows.Forms.MouseButtons.Left && (Math.Abs(objArg.X-m_MouseDownPt.X)>=2 || Math.Abs(objArg.Y-m_MouseDownPt.Y)>=2))
			{
				BaseItem objFocus=this.FocusedItem();
				if(objFocus!=null && objFocus.CanCustomize)
				{
					if(!objFocus.SystemItem)
					{
						IOwner owner=this.GetOwner() as IOwner;
						if(owner!=null)
							owner.StartItemDrag(objFocus);
					}
				}
				else if(!m_IsContainer)
				{
					if(!this.SystemItem && m_CanCustomize)
					{
						IOwner owner=this.GetOwner() as IOwner;
						if(owner!=null)
							owner.StartItemDrag(this);
					}
				}
				return;
			}

			// If item is container and not in design time...
			if(m_IsContainer && !m_DesignMode)
			{
				BaseItem objNew=ItemAtLocation(objArg.X,objArg.Y);
				if(objNew!=m_HotSubItem && (!(objArg.Button != MouseButtons.None && m_HotSubItem!=null) || m_CheckMouseMovePressed) && 
                    !(m_HotSubItem!=null && m_HotSubItem.MouseDownCapture && objArg.Button == MouseButtons.Left))
				{
					LeaveHotSubItem(objNew);
					
					if(objNew!=null)
					{
						if(m_AutoExpand)
						{
							BaseItem objItem=ExpandedItem();
							if(objItem!=null && objItem!=objNew)
								objItem.Expanded=false;
						}
                        if (objNew != this)
                        {
                            objNew.InternalMouseEnter();
                            objNew.InternalMouseMove(objArg);
                            if (m_AutoExpand && objNew.ShowSubItems && objNew.GetEnabled())
                            {
                                if (objNew is PopupItem)
                                {
                                    PopupItem pi = objNew as PopupItem;
                                    ePopupAnimation oldAnim = pi.PopupAnimation;
                                    pi.PopupAnimation = ePopupAnimation.None;
                                    if (objNew.SubItems.Count > 0)
                                        objNew.Expanded = true;
                                    pi.PopupAnimation = oldAnim;
                                }
                                else
                                    objNew.Expanded = true;
                            }
                            HotSubItem = objNew;
                        }
					}
					else
						HotSubItem=null;
                    if(MouseMove!=null)
                        MouseMove(this, objArg);
				}
				else if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseMove(objArg);
				}
				else
				{
					if(MouseMove!=null)
						MouseMove(this,objArg);
					IOwnerItemEvents owner=this.GetIOwnerItemEvents();
					if(owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
						owner.InvokeMouseMove(this,objArg);
				}

                if (m_TextMarkup != null)
                {
                    //m_TextMarkup.MouseMove(this.ContainerControl as Control, objArg);

                    Control control = GetParentControl();

                    if (control != null)
                        m_TextMarkup.MouseMove(control, objArg);
                }
			}
			else
			{
				if (!m_DesignMode)
				{
                    if (m_TextMarkup != null)
                    {
                        //m_TextMarkup.MouseMove(this.ContainerControl as Control, objArg);

                        Control control = GetParentControl();

                        if (control != null)
                            m_TextMarkup.MouseMove(control, objArg);
                    }

				    if (MouseMove!=null)
						MouseMove(this,objArg);

					IOwnerItemEvents owner=this.GetIOwnerItemEvents();

					if (owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
						owner.InvokeMouseMove(this,objArg);
				}
			}
        }

        internal Control GetParentControl()
        {
            Control control = ContainerControl as Control;

            if (control == null)
            {
                BaseItem item = ContainerControl as BaseItem;

                while (item != null)
                {
                    control = item.ContainerControl as Control;

                    if (control != null)
                        break;

                    item = item.Parent;
                }
            }

            return (control);
        }

		/// <summary>
		/// Occurs when the item is clicked. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
		{
			if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalClick(mb,mpos);
				return;
			}

            if (m_TextMarkup != null)
                m_TextMarkup.Click(this.ContainerControl as System.Windows.Forms.Control);

			//RaiseClick();
		}

		/// <summary>
		/// Gets whether RaiseClick method will generate a click event give current item state.
		/// </summary>
		internal bool CanRaiseClick
		{
			get
			{
                if (this.DesignMode || !m_Visible && !this.SystemItem && !this.IsOnCustomizeMenu || _IgnoreClick || !GetEnabled())
					return false;
#if TRIAL
				if(NativeFunctions.ColorExpAlt())
					return false;
#endif

				return true;
			}
		}

        /// <summary>
        /// Raises the click event and provide the information about the source of the event.
        /// </summary>
        /// <param name="source"></param>
        public virtual void RaiseClick(eEventSource source)
        {
            //if (BaseItem.IsOnPopup(this))
            //    NativeFunctions.sndPlaySound("MenuCommand", NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
            if (!CanRaiseClick)
                return;

            IOwnerItemEvents owner = this.GetIOwnerItemEvents();
            BaseItem rootParent = this.RootParentItem;

            if (m_AutoCollapseOnClick)
            {
                if ((!this.SystemItem || this.SystemItem && this.Name.StartsWith("mdi-")) && IsOnPopup(this) && !(m_Parent != null && m_Parent.SystemItem && !(m_Parent is DisplayMoreItem)))
                {
                    if (!(this is PopupItem && ((PopupItem)this).ShowSubItems && (SubItems.Count > 0 || ((PopupItem)this).PopupType == ePopupType.Container)))
                        CollapseAll(this);
                }

                if (m_Parent != null && m_Parent.AutoExpand && this.SubItems.Count == 0)
                    m_Parent.AutoExpand = false;
            }

            if (m_InClickEvent) return;
            m_InClickEvent = true;
            try
            {
                this.OnClick();

                if (Click != null)
                    Click(this, new DevComponents.DotNetBar.Events.EventSourceArgs(source));

                if (rootParent != null)
                {
                    Bar bar = rootParent.ContainerControl as Bar;
                    if (bar != null)
                        bar.InvokeItemClick(this, new DevComponents.DotNetBar.Events.EventSourceArgs(source));
                }

                if (owner != null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                    owner.InvokeItemClick(this);
            }
            finally
            {
                m_InClickEvent = false;
            }
        }

		/// <summary>
		/// Raises the Click event with default source as Code.
		/// </summary>
		private bool m_InClickEvent=false;
		public virtual void RaiseClick()
		{
            RaiseClick(eEventSource.Code);
		}

        private BaseItem RootParentItem
        {
            get
            {
                BaseItem parent = m_Parent;
                while (parent != null && parent.Parent != null)
                    parent = parent.Parent;
                return parent;
            }
        }

		internal void ClearClick()
		{
			Click=null;
		}

		/// <summary>
		/// Occurs when the item is double clicked. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void InternalDoubleClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
		{
            if (m_HotSubItem != null)
            {
                m_HotSubItem.InternalDoubleClick(mb, mpos);
            }
            else
            {
                InvokeDoubleClick();

                IOwnerItemEvents owner = this.GetIOwnerItemEvents();
                if (owner != null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                    owner.InvokeItemDoubleClick(this, new MouseEventArgs(mb, 2, mpos.X, mpos.Y, 0));
            }
		}

        /// <summary>
        /// Invokes DoubleClick event.
        /// </summary>
        protected virtual void InvokeDoubleClick()
        {
            if (DoubleClick != null)
                DoubleClick(this, new EventArgs());
        }

		/// <summary>
		/// Occurs when the item receives focus. If overridden base implementation must be called so default processing can occur.
		/// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void OnGotFocus()
		{
			m_Focused=true;

			if(!m_DesignMode)
			{
				IOwnerItemEvents owner=this.GetIOwnerItemEvents();
				if(owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
					owner.InvokeGotFocus(this,new EventArgs());
			}

			this.Refresh();

			if(GotFocus!=null)
				GotFocus(this,new System.EventArgs());
		}

		/// <summary>
		/// Occurs when the item has lost the focus. If overridden base implementation must be called so default processing can occur.
		/// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void OnLostFocus()
		{
            if (this.IsDisposed) return;

            if (!m_DesignMode)
            {
                IOwnerItemEvents owner = this.GetIOwnerItemEvents();
                if (owner != null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                    owner.InvokeLostFocus(this, new EventArgs());
            }
            else
                InternalMouseLeave();
			m_Focused=false;
			this.Refresh();

			if(LostFocus!=null)
				LostFocus(this,new System.EventArgs());
		}

		/// <summary>
		/// Sets the input focus to the item. If overridden base implementation must be called so default processing can occur.
		/// </summary>
		public virtual void Focus()
		{
			if(BaseItem.IsOnPopup(this))
			{
				MenuPanel menu=this.ContainerControl as MenuPanel;
				if(menu!=null && menu.ParentItem.Parent==null)
				{
					menu.SetFocusItem(this);
					return;
				}
			}

			IOwner owner=this.GetOwner() as IOwner;
			if(owner!=null)
				owner.SetFocusItem(this);
			else
				this.OnGotFocus();

			if(this.Parent!=null)
				this.Parent.OnSubItemGotFocus(this);
		}

		/// <summary>
		/// Releases the input focus. If overridden base implementation must be called so default processing can occur.
		/// </summary>
		public virtual void ReleaseFocus()
		{
			if(m_Focused)
			{
				if(BaseItem.IsOnPopup(this))
				{
					MenuPanel menu=this.ContainerControl as MenuPanel;
					if(menu!=null && menu.ParentItem.Parent==null)
					{
						menu.SetFocusItem(null);
						return;
					}
				}

				IOwner owner=this.GetOwner() as IOwner;
				if(owner!=null)
					owner.SetFocusItem(null);
				else
					this.OnLostFocus();

				if(this.Parent!=null)
					this.Parent.OnSubItemLostFocus(this);
			}
		}

		/// <summary>
		/// Gets whether item has input focus.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual bool Focused
		{
			get
			{
				return m_Focused;
			}
		}

		/// <summary>
		/// Occurs when item container has lost the input focus. If overridden base implementation must be called so default processing can occur.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void ContainerLostFocus(bool appLostFocus)
		{
			if(m_SubItems!=null)
			{
				foreach(BaseItem objSub in m_SubItems)
				{
					objSub.ContainerLostFocus(appLostFocus);
				}
			}
			if(m_HotSubItem!=null)
			{
                bool leave = true;
                if (!appLostFocus)
                {
                    System.Windows.Forms.Control c = this.ContainerControl as System.Windows.Forms.Control;
                    if (c != null)
                    {
                        Point p = c.PointToClient(System.Windows.Forms.Control.MousePosition);
                        if (m_HotSubItem.DisplayRectangle.Contains(p))
                            leave = false;
                    }
                }
                if (leave)
                {
                    m_HotSubItem.InternalMouseLeave();
                    HotSubItem = null;
                }
			}
			if(this.Focused)
			{
				IOwner owner=this.GetOwner() as IOwner;
				if(owner!=null)
					owner.SetFocusItem(null);
			}
		}

		/// <summary>
		/// Occurs when item container receives the input focus. If overridden base implementation must be called so default processing can occur.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void ContainerGotFocus()
		{
			if(m_SubItems!=null)
			{
				foreach(BaseItem objSub in m_SubItems)
				{
					objSub.ContainerGotFocus();
				}
			}
		}

		/// <summary>
		/// Indicates that item size has changed. It must be called by child item to let the parent know that its size
		/// has been changed.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual void SubItemSizeChanged(BaseItem objChildItem) {}
		
		/// <summary>
		/// Return Sub Item at specified location
		/// </summary>
		public virtual BaseItem ItemAtLocation(int x, int y)
		{
			if(!m_IsContainer)
			{
				return null;
			}
			if(m_SubItems!=null)
			{
                foreach (BaseItem objSub in m_SubItems)
                {
                    //if(objSub.Visible && objSub.Displayed && x>=objSub.Left && x<=(objSub.Left+objSub.Width) && y>=objSub.Top && y<=(objSub.Top+objSub.Height))
                    if ((objSub.Visible || m_IsOnCustomizeMenu) && objSub.Displayed && objSub.DisplayRectangle.Contains(x, y))
                    {
                        if (m_ViewRectangle.IsEmpty || m_ViewRectangle.Contains(x, y))
                            return objSub;
                    }
                }
			}
			return null;
		}

		/// <summary>
		/// Gets the current expanded subitem.
		/// </summary>
		/// <returns></returns>
		protected internal virtual BaseItem ExpandedItem()
		{
			if(!m_IsContainer)
			{
				return null;
			}
			if(m_SubItems!=null)
			{
				foreach(BaseItem objSub in m_SubItems)
				{
                    if (objSub is GalleryContainer && ((GalleryContainer)objSub).IsGalleryPopupOpen)
                        return objSub;
                    if (objSub is ItemContainer)
                    {
                        BaseItem exp = objSub.ExpandedItem();
                        if (exp != null)
                            return exp;
                    }
					else if(objSub.Expanded)
					{
						return objSub;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the item that has input focus.
		/// </summary>
		/// <returns>Item that has focus or Null (Nothing) if none of the subitems have focus.</returns>
		protected internal BaseItem FocusedItem()
		{
			if(!m_IsContainer)
				return null;
			if(m_SubItems!=null)
			{
				foreach(BaseItem objSub in m_SubItems)
				{
					if(objSub.Focused)
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
		
		/// <summary>
		/// Gets the owner of the item.
		/// </summary>
		/// <returns>DotNetBarManager that owns the item.</returns>
		public object GetOwner()
		{
			if(m_Owner!=null)
				return m_Owner;

			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;

			if (objCtrl is Bar)
                return ((Bar)objCtrl).Owner;
			else if(objCtrl is MenuPanel)
				return ((MenuPanel)objCtrl).Owner;
			else if(m_Parent!=null)
				return m_Parent.GetOwner();
			
			return null;
		}

		internal protected IOwnerItemEvents GetIOwnerItemEvents()
		{
			return this.GetOwner() as IOwnerItemEvents;
		}

		internal void SetOwner(object owner)
		{
			if(m_Owner!=owner)
			{
				m_Owner=owner;
				this.OnOwnerChanged();
			}
		}

		/// <summary>
		/// Recalculate the size of the item. If overridden base implementation must be called so default processing can occur.
		/// </summary>
		public virtual void RecalcSize()
		{
			m_NeedRecalcSize=false;
		}

		/// <summary>
		/// Indicates that item is recalculating its size.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false)]
		public bool IsRecalculatingSize
		{
			get {return m_RecalculatingSize;}
		}
		
		/// <summary>
		/// Must be overridden by class that is inheriting to provide the painting for the item.
		/// </summary>
		public abstract void Paint(ItemPaintArgs p);

		/// <summary>
		/// Must be overridden by class that is inheriting to provide the method to
		/// return copy of an item. 
		/// </summary>
		public abstract BaseItem Copy();

		/// <summary>
		/// Internal Copy implementation.
		/// </summary>
		/// <param name="objCopy">Item to copy to.</param>
		protected virtual void CopyToItem(BaseItem objCopy)
		{
			objCopy.Visible=m_Visible;
			objCopy.SetIsContainer(m_IsContainer);
			objCopy.AutoExpand=m_AutoExpand;
			objCopy.BeginGroup=m_BeginGroup;
			objCopy.Enabled=m_Enabled;
			objCopy.Description=m_Description;
			objCopy.Tooltip=m_Tooltip;
			objCopy.Category=m_Category;
			objCopy.Orientation=m_Orientation;
			objCopy.Text=m_Text;
			objCopy.Name=m_Name;
			objCopy.GlobalName = m_GlobalName;
            objCopy.GlobalItem = m_GlobalItem;
			objCopy.Style=m_Style;
			objCopy.SetDesignMode(m_DesignMode);
			objCopy.ShowSubItems=m_ShowSubItems;
			objCopy.Stretch=m_Stretch;
			if(m_Shortcuts!=null && m_Shortcuts.Count>0)
				objCopy.Shortcuts.FromString(m_Shortcuts.ToString(","),",");
			
			// Copy Events
			objCopy.Click=this.Click;
			objCopy.ExpandChange=this.ExpandChange;
			objCopy.LostFocus=this.LostFocus;
			objCopy.GotFocus=this.GotFocus;
			objCopy.MouseDown=this.MouseDown;
			objCopy.MouseEnter=this.MouseEnter;
			objCopy.MouseHover=this.MouseHover;
			objCopy.MouseLeave=this.MouseLeave;
			objCopy.MouseMove=this.MouseMove;
			objCopy.MouseUp=this.MouseUp;
			objCopy.VisibleChanged=this.VisibleChanged;
			objCopy.EnabledChanged=this.EnabledChanged;
			objCopy.TextChanged=this.TextChanged;
            objCopy.IsRightToLeft = this.IsRightToLeft;
			objCopy.Cursor=m_Cursor;
			objCopy.ThemeAware=m_ThemeAware;
            objCopy.AutoCollapseOnClick = this.AutoCollapseOnClick;
			objCopy.Tag=m_ItemData;
            objCopy.CanCustomize = m_CanCustomize;
            objCopy.Command = this.Command;
            objCopy.CommandParameter = this.CommandParameter;
            objCopy.KeyTips = this.KeyTips;
			if(m_SubItems!=null)
			{
				foreach(BaseItem objItem in m_SubItems)
					objCopy.SubItems.Add(objItem.Copy());
			}

			if(ItemCopied!=null)
				ItemCopied(objCopy,new EventArgs());
		}

		/// <summary>
		/// Returns copy of the item.
		/// </summary>
		/// <returns>Copy of the item.</returns>
		public object Clone()
		{
			return this.Copy();
		}

		/// <summary>
		/// Forces the repaint the item.
		/// </summary>
		public virtual void Refresh()
		{
			if(m_SuspendLayout)
				return;

			if((m_Visible || m_IsOnCustomizeMenu) && m_Displayed)
			{
				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
                if (objCtrl != null && IsHandleValid(objCtrl) && !(objCtrl is ItemsListBox))
                {
                    if (objCtrl.InvokeRequired)
                    {
                        objCtrl.BeginInvoke(new MethodInvoker(delegate { this.Refresh(); }));
                        return;
                    }

                    if (m_NeedRecalcSize)
                    {
                        if (m_Parent is ItemContainer)
                        {
                            m_Parent.RecalcSize();
                        }
                        else
                        {
                            RecalcSize();
                            if (m_Parent != null)
                                m_Parent.SubItemSizeChanged(this);
                        }
                    }
                    Invalidate(objCtrl);
                }
			}
		}

        protected virtual void Invalidate(Control containerControl)
        {
            if (this.DesignMode)
            {
                if (containerControl.InvokeRequired)
                    containerControl.BeginInvoke(new MethodInvoker(delegate { containerControl.Invalidate(); }));
                else
                    containerControl.Invalidate();
            }
            else
            {
                if (containerControl.InvokeRequired)
                    containerControl.BeginInvoke(new MethodInvoker(delegate { containerControl.Invalidate(m_Rect, true); }));
                else
                    containerControl.Invalidate(m_Rect, true);
            }
        }

		private bool m_Refreshing=false;
		protected internal void OnAppearanceChanged()
		{
			if(m_Refreshing || !this.DesignMode)
				return;

			m_Refreshing=true;
			try
			{
				System.Windows.Forms.Control c=this.ContainerControl as System.Windows.Forms.Control;
				if(c!=null)
				{
                    BarFunctions.InvokeRecalcLayout(c, true);
				}
			}
			finally
			{
				m_Refreshing=false;
			}
		}

		/// <summary>
		/// Occurs when Expanded state changes. If overridden base implementation must be called so default processing can occur.
		/// </summary>
		protected internal virtual void OnExpandChange()
		{
			HideToolTip();
			if(ExpandChange!=null)
				ExpandChange(this,new EventArgs());

			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null && !this.SystemItem && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
				owner.InvokeExpandedChange(this,new EventArgs());
		}

		/// <summary>
		/// Occurs when sub item expanded state has changed.
		/// </summary>
		/// <param name="item">Sub item affected.</param>
		protected internal virtual void OnSubItemExpandChange(BaseItem item){}

		/// <summary>
		/// Suspends all layout for the item including painting. Use this property carefully and only to improve performace.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual bool SuspendLayout
		{
			get
			{
				return m_SuspendLayout;
			}
			set
			{
				m_SuspendLayout=value;
			}
		}

		/// <summary>
		/// Releases all resurces used in this control. After calling Dispose()
		/// object is not in valid state and cannot be recovered to the valid state.
		/// Recreation of the object is required.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if(m_Expanded && (this.Site!=null && !this.Site.DesignMode || this.Site==null))
				this.Expanded=false;
			
			HideToolTip();

			if(m_Parent!=null)
			{
				m_Parent=null;
			}
			IOwner owner=this.GetOwner() as IOwner;
			if(owner!=null)
				owner.RemoveShortcutsFromItem(this);

			if(this.Site==null || this.Site!=null && !this.Site.DesignMode)
			{
				if(m_SubItems!=null && m_SubItems.Count>0)
				{
					ArrayList subitems=new ArrayList(m_SubItems.Count);
					m_SubItems.CopyTo(subitems);
					foreach(BaseItem objSub in subitems)
					{
						if(objSub.Parent==this)
							objSub.Dispose();
					}
					m_SubItems._Clear();
				}
				m_SubItems=null;
			}

            //if(Disposed != null)
            //    Disposed(this,EventArgs.Empty);

			if(this.Site==null || this.Site!=null && !this.Site.DesignMode)
			{
				if(m_Shortcuts!=null)
					m_Shortcuts=null;
				//m_ItemData=null;
				m_ContainerControl=null;
			}

            if (_Command != null)
                this.Command = null;

            m_IsDisposed = true;

            base.Dispose(disposing);
		}

        /// <summary>
        /// Gets whether item has been disposed through Dispose method call.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposed
        {
            get
            {
                return m_IsDisposed;
            }
        }

		/// <summary>
		/// Returns the collection of sub items.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public virtual SubItemsCollection SubItems
		{
			get
			{
				if(m_SubItems==null)
					m_SubItems=new SubItemsCollection(this);
				return m_SubItems;
			}
		}

        /// <summary>
        /// Returns count of sub items in SubItems collection that have Visible property set to true.
        /// </summary>
        [Browsable(false)]
        public int VisibleSubItems
        {
            get
            {
                int iCount = 0;
                foreach (BaseItem item in this.SubItems)
                    if (item.Visible)
                        iCount++;
                return iCount;
            }
        }


		/// <summary>
		/// Unique ID that indentifies the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Specifies unique ID of the item.")]
		public long Id
		{
			get
			{
				return m_Id;
			}
		}

//		/// <summary>
//		/// Allows the user to associate custom user data with the item.
//		/// </summary>
//		[System.ComponentModel.Browsable(false), DevCoBrowsable(false),System.ComponentModel.Category("Data"),System.ComponentModel.DefaultValue(""),System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)),Obsolete("ItemData is being replaced by Tag property.")]
//		public virtual object ItemData
//		{
//			get
//			{
//				return m_ItemData;
//			}
//			set
//			{
//				m_ItemData=value;
//			}
//		}

		/// <summary>
		/// Allows the user to associate custom user data with the item.
		/// </summary>
		[System.ComponentModel.Browsable(true), DevCoBrowsable(true),System.ComponentModel.Category("Data"),System.ComponentModel.DefaultValue(""),System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)),System.ComponentModel.Localizable(true)]
		public virtual object Tag
		{
			get
			{
				return m_ItemData;
			}
			set
			{
				m_ItemData=value;
			}
		}
		
		/// <summary>
		/// Applies new visual style to this the item and all of its sub-items.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.DefaultValue(eDotNetBarStyle.OfficeXP),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Determines the display of the item when shown.")]
		public virtual eDotNetBarStyle Style
		{
			get
			{
				return m_Style;
			}
			set
			{
				m_Style=value;
				if(m_SubItems!=null)
				{
					foreach(BaseItem objSub in m_SubItems)
					{
						objSub.Style=m_Style;
					}
				}
				OnStyleChanged();
			}
		}
        /// <summary>
        /// Gets the effective item style.
        /// </summary>
        [Browsable(false)]
        public virtual eDotNetBarStyle EffectiveStyle
        {
            get
            {
                if (Style == eDotNetBarStyle.StyleManagerControlled)
                    return StyleManager.GetEffectiveStyle();
                return Style;
            }
        }

		/// <summary>
		/// Occurs after item visual style has changed.
		/// </summary>
		protected virtual void OnStyleChanged(){}

		/// <summary>
		/// Occurs after text has changed.
		/// </summary>
		protected virtual void OnTextChanged()
		{
            MarkupTextChanged();
			if(TextChanged!=null)
				TextChanged(this,new EventArgs());
			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null)
				owner.InvokeItemTextChanged(this,new EventArgs());
			this.GenerateAccessibilityEvent(System.Windows.Forms.AccessibleEvents.NameChange);

			this.OnAppearanceChanged();
		}

		/// <summary>
		/// Gets or sets whether item will display sub items.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Determines whether sub-items are displayed.")]
		public virtual bool ShowSubItems
		{
			get
			{
				return m_ShowSubItems;
			}
			set
			{
				if(m_ShowSubItems!=value)
				{
					NeedRecalcSize=true;
					m_ShowSubItems=value;
					if(this.Displayed)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets item alignment inside the container.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(eItemAlignment.Near),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Determines alignment of the item inside the container.")]
		public virtual eItemAlignment ItemAlignment
		{
			get
			{
				return m_ItemAlignment;
			}
			set
			{
				if(m_ItemAlignment!=value)
				{
					m_ItemAlignment=value;
					NeedRecalcSize=true;
					if(this.Displayed && this.Parent!=null)
						this.Parent.SubItemSizeChanged(this);
				}
			}
		}

		/// <summary>
		/// Returns if passed control is valid.
		/// </summary>
		/// <param name="objCtrl">Control to test.</param>
		/// <returns></returns>
		protected internal static bool IsHandleValid(System.Windows.Forms.Control objCtrl)
		{
			return (objCtrl!=null && !objCtrl.Disposing && !objCtrl.IsDisposed && objCtrl.IsHandleCreated);
		}

		/// <summary>
		/// Resets Hoover timer.
		/// </summary>
		protected virtual void ResetHover()
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(!IsHandleValid(objCtrl))
				return;
			// We need to reset hover thing since it is fired only first time mouse hovers inside the window and we need it for each of our items
			NativeFunctions.TRACKMOUSEEVENT tme=new NativeFunctions.TRACKMOUSEEVENT();
			tme.dwFlags=NativeFunctions.TME_QUERY;
			tme.hwndTrack=objCtrl.Handle;
			tme.cbSize=System.Runtime.InteropServices.Marshal.SizeOf(tme);
			NativeFunctions.TrackMouseEvent(ref tme);
			tme.dwFlags=tme.dwFlags | NativeFunctions.TME_HOVER;
			NativeFunctions.TrackMouseEvent(ref tme);
			objCtrl=null;
		}

		/// <summary>
		/// Returns true if any subitem is contained on the control with a given handle.
		/// </summary>
		/// <param name="iHandle">Container handle to test.</param>
		/// <returns></returns>
		protected internal virtual bool IsAnyOnHandle(int iHandle)
		{
			bool bRet=false;
			if(m_SubItems==null || m_SubItems.Count==0)
				return bRet;
			BaseItem objTmp=m_SubItems[0] as BaseItem;
			System.Windows.Forms.Control objCtrl=null;
			// Since all items are always contained on same control check the first child only
			if(objTmp!=null)
			{
				objCtrl=objTmp.ContainerControl as System.Windows.Forms.Control;
				if(objCtrl is MenuPanel && ((MenuPanel)objCtrl).PopupMenu)
					objCtrl=objCtrl.Parent;
				if(objCtrl!=null && objCtrl.Handle.ToInt32()==iHandle)
					return true;
				objTmp=null;
			}

			foreach(BaseItem objItem in m_SubItems)
			{
				// Now go through each child and if it is expanded call this function to verify the handle
				if(objItem.Expanded)
				{
					if(objItem.IsAnyOnHandle(iHandle))
					{
						bRet=true;
						break;
					}
				}
			}

			return bRet;
		}

		/// <summary>
		/// Gets or sets item description. This description is displayed in
		/// Customize dialog to describe the item function in an application.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(""),Category("Design"),Description("Indicates description of the item that is displayed during design."), Localizable(true)]
		public virtual string Description
		{
			get
			{
			
				return m_Description;
			}
			set
			{
				if(m_Description==value)
					return;
				m_Description=value;

                if(ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "Description");
			}
		}

		/// <summary>
		/// Gets or sets whether the item expands automatically to fill out the remaining space inside the container. Applies to Items on stretchable, no-wrap Bars only.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
		public virtual bool Stretch
		{
			get
			{
				return m_Stretch;
			}
			set
			{
				if(m_Stretch==value)
					return;
				m_Stretch=value;
                if(ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "Stretch");
				NeedRecalcSize=true;
				if(this.Displayed && m_Parent!=null && !m_SuspendLayout)
				{
					m_Parent.SubItemSizeChanged(this);
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Occurs after Tooltip text has changed.
		/// </summary>
		protected virtual void OnTooltipChanged(){}
		/// <summary>
		/// Gets/Sets informational text (tooltip) for the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(""),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the text that is displayed when mouse hovers over the item."),System.ComponentModel.Localizable(true)]
		public virtual string Tooltip
		{
			get
			{
			
				return m_Tooltip;
			}
            set
            {
                if (m_Tooltip == value)
                    return;
                if (value == null) value = "";
                m_Tooltip = value;

                if (this.ToolTipVisible)
                {
                    if (string.IsNullOrEmpty(m_Tooltip))
                        this.HideToolTip();
                    else
                    {
                        ToolTip tooltipWindow = m_ToolTipWnd;
                        tooltipWindow.Text = m_Tooltip;
                        tooltipWindow.ShowToolTip();
                        tooltipWindow.Invalidate();
                    }
                }

                if (ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "Tooltip");
                OnTooltipChanged();
            }
		}

		/// <summary>
		/// Returns category for this item. If item cannot be customzied using the
		/// customize dialog category is empty string.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(""),Category("Design"),Description("Indicates item category used to group similar items at design-time."), Localizable(true)]
		public virtual string Category
		{
			get
			{
			
				return m_Category;
			}
			set
			{
				m_Category=value;
			}
		}

		/// <summary>
		/// Returns name of the item that can be used to identify item from the code.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Indicates the name used to identify item.")]
		public virtual string Name
		{
			get
			{
				if(this.Site!=null)
					m_Name=this.Site.Name;
				return m_Name;
			}
			set
			{
				if(this.Site!=null)
					this.Site.Name=value;
				if(value==null)
					m_Name="";
				else
					m_Name=value;
			}
		}

        /// <summary>
        /// Gets or sets the global name of the item that is used to synchronize the Global properties for the item across all instances with same
        /// global name. Note that only instances that belong to the same control are synchronized. GlobalItem must be set to true to perform the synchronization.
        /// You can find more information and list of
        /// properties that are synchronized in help file.
        /// </summary>
        [Browsable(true), Category("Design"), Description("Indicates global name of the item that is used to synchronize the Global properties for the item across all instances with same global name."), DefaultValue("")]
        public virtual string GlobalName
        {
            get { return m_GlobalName; }
            set { m_GlobalName = value;  }
        }

        /// <summary>
        /// Gets whether global properties should synchronized.
        /// </summary>
        protected virtual bool ShouldSyncProperties
        {
            get { return this.GlobalItem && (this.GlobalName.Length > 0 || this.Name.Length > 0) && !m_IsOnCustomizeDialog; }
        }

		/// <summary>
		/// Gets orientation within container that is supported by this item. If item does not support certain orientation the container automatically hides it when container switches into that orientation.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public eSupportedOrientation SupportedOrientation
		{
			get
			{
				return m_SupportedOrientation;
			}
		}

		/// <summary>
		/// Gets or sets whether item is global or not.
		/// This flag is used to propagate property changes to all items with the same name.
		/// Setting for example Visible property on the item that has GlobalItem set to true will
		/// set visible property to the same value on all items with the same name.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
		public virtual bool GlobalItem
		{
			get
			{
				return m_GlobalItem;
			}
			set
			{
				m_GlobalItem=value;
			}
		}

		/// <summary>
		/// Gets or sets orientation inside the container. Do not change the value of this property. It is managed by system only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual eOrientation Orientation
		{
			get
			{
				return m_Orientation;
			}
			set
			{
				if(m_Orientation!=value)
				{
                    SetOrientation(value);
					NeedRecalcSize=true;
				}
			}
		}

        /// <summary>
        /// Sets orientation of the item but it does not cause the recalculate layout flag setting on the parent item.
        /// </summary>
        /// <param name="o">New orientation value.</param>
        internal void SetOrientation(eOrientation o)
        {
            if (m_Orientation != o)
            {
                m_Orientation = o;
                if (m_SubItems != null)
                {
                    foreach (BaseItem objItem in m_SubItems)
                    {
                        objItem.Orientation = m_Orientation;
                    }
                }
                m_NeedRecalcSize = true;
            }
        }

		/// <summary>
		/// Destroys tooltip window.
		/// </summary>
		internal protected void HideToolTip()
		{
			if(m_ToolTipWnd!=null)
			{
				System.Drawing.Rectangle tipRect=m_ToolTipWnd.Bounds;
				tipRect.Width+=5;
				tipRect.Height+=6;
				
				OnTooltip(false);
                OnToolTipVisibleChanged(new EventArgs());
				try
				{
					if(m_ToolTipWnd!=null)
					{
						m_ToolTipWnd.Hide();
						m_ToolTipWnd.Dispose();
						m_ToolTipWnd=null;
					}
				}
				catch{}

				if(this.ContainerControl!=null)
				{
					System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
					if(ctrl!=null)
						ctrl.Invalidate(ctrl.RectangleToClient(tipRect),false);
				}
			}
		}

        private void OnToolTipVisibleChanged(EventArgs eventArgs)
        {
            EventHandler h = ToolTipVisibleChanged;
            if (h != null)
                ToolTipVisibleChanged(this, eventArgs);
        }

        internal BaseItem HotSubItem
        {
            get
            {
                return m_HotSubItem;
            }
            set
            {
                BaseItem oldValue = m_HotSubItem;
                m_HotSubItem = value;
                if (oldValue != value)
                    OnHotSubItemChanged(value, oldValue);
            }
        }
        /// <summary>
        /// Called when HotSubItem has changed.
        /// </summary>
        /// <param name="newValue">New value.</param>
        /// <param name="oldValue">Old value.</param>
        protected virtual void OnHotSubItemChanged(BaseItem newValue, BaseItem oldValue)
        {
        }

        /// <summary>
		/// Shows tooltip for this item.
		/// </summary>
		public virtual void ShowToolTip()
		{
			if(m_DesignMode)
				return;

			if(m_Visible && m_Displayed && !m_Expanded)
			{
				IOwner owner=this.GetOwner() as IOwner;
				if(owner!=null && !owner.ShowToolTips || !this.ShowToolTips)
					return;
                Control container = this.ContainerControl as Control;
                if (container is Bar && !((Bar)container).ShowToolTips)
					return;

                if (container is MenuPanel && !((MenuPanel)container).ShowToolTips)
					return;

				OnTooltip(true);
				if(m_Tooltip!="")
				{
					if(m_ToolTipWnd==null)
						m_ToolTipWnd=new ToolTip();
                    m_ToolTipWnd.Style = EffectiveStyle;
					m_ToolTipWnd.Text=m_Tooltip;
					if(owner!=null && owner.ShowShortcutKeysInToolTips && m_Shortcuts!=null && m_Shortcuts.Count>0)
                        m_ToolTipWnd.Text+=(" ("+GetTooltipShortcutString()+")");
					IOwnerItemEvents ownerEvents=this.GetIOwnerItemEvents();
					if(ownerEvents!=null)
						ownerEvents.InvokeToolTipShowing(this,new EventArgs());

                    m_ToolTipWnd.ReferenceRectangle = ScreenRectangle;

                    OnToolTipVisibleChanged(new EventArgs());
					m_ToolTipWnd.ShowToolTip();
				}
			}
		}
        /// <summary>
        /// Returns the shortcut string that is displayed on tooltip.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTooltipShortcutString()
        {
            return this.ShortcutString;
        }

        /// <summary>
        /// Gets whether tooltip for the item is displayed.
        /// </summary>
        protected virtual bool ShowToolTips
        {
            get { return true; }
        }

        internal virtual Rectangle ScreenRectangle
        {
            get
            {
                System.Windows.Forms.Control c = this.ContainerControl as System.Windows.Forms.Control;
                if (c == null) return Rectangle.Empty;
                return new Rectangle(c.PointToScreen(this.DisplayRectangle.Location), this.DisplayRectangle.Size);
            }
        }

		/// <summary>
		/// Gets whether tooltip is visible or not.
		/// </summary>
		internal protected bool ToolTipVisible
		{
			get
			{
				return (m_ToolTipWnd!=null);
			}
		}

		/// <summary>
		/// Gets or sets the name of the bar this item originated on. This is used to remember the
		/// originating bar when user is moving the items from bar to bar.
		/// </summary>
		internal string OriginalBarName
		{
			get {return m_OriginalBarName;}
			set {m_OriginalBarName=value;}
		}

		/// <summary>
		/// Gets or sets item's original position (index) if item position has changed due to the user customization.
		/// </summary>
		internal int OriginalPosition
		{
			get {return m_OriginalPosition;}
			set {m_OriginalPosition=value;}
		}

        /// <summary>
        /// Gets or sets flag that indicates whether item was customize by the end-user.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UserCustomized
        {
            get { return m_UserCustomized; }
            set { m_UserCustomized = value; }
        }

		/// <summary>
		/// Gets reference to the tooltip control if tooltip is displayed for this item.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public ToolTip ToolTipControl
		{
			get {return m_ToolTipWnd;}
		}

        /// <summary>
        /// Gets or sets the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar. Use KeyTips property
        /// when you want to assign the one or more letters to be used to access an item. For example assigning the FN to KeyTips property
        /// will require the user to press F then N keys to select an item. Pressing the F letter will show only keytips for the items that start with letter F.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(""), Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
        public virtual string KeyTips
        {
            get { return m_KeyTips; }
            set
            {
                if (value == null) value = "";
                m_KeyTips = value.ToUpper();
            }
        }

		/// <summary>
		/// Gets or sets the text associated with this item.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("The text contained in the item."),Localizable(true),DefaultValue(""), Bindable(true)]
		public virtual string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if(m_Text==value)
					return;
				if(value==null)
					m_Text="";
				else
					m_Text=value;
				m_AccessKey=NativeFunctions.GetAccessKey(m_Text);

                if(ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "Text");

				NeedRecalcSize=true;
				if(this.Displayed && m_Parent!=null && !m_SuspendLayout)
				{
					RecalcSize();
					m_Parent.SubItemSizeChanged(this);
				}

				this.OnTextChanged();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether item can be customized by end user.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether item can be customized by user.")]
		public virtual bool CanCustomize
		{
			get
			{
				return m_CanCustomize;
			}
			set
			{
				m_CanCustomize=value;
			}
		}

		/// <summary>
		/// Returns whether item is hosted on Customize menu.
		/// </summary>
		protected internal bool IsOnCustomizeMenu
		{
			get
			{
				return m_IsOnCustomizeMenu;
			}
		}

		internal void SetIsOnCustomizeMenu(bool b)
		{
			if(m_IsOnCustomizeMenu!=b)
			{
				m_IsOnCustomizeMenu=b;
				OnIsOnCustomizeMenuChanged();
			}
		}

		public override string ToString()
		{
			return m_Text;
		}

		/// <summary>
		/// Returns whether item is hosted on Customize Dialog.
		/// </summary>
		protected internal bool IsOnCustomizeDialog
		{
			get
			{
				return m_IsOnCustomizeDialog;
			}
		}

		internal void SetIsOnCustomizeDialog(bool b)
		{
			if(m_IsOnCustomizeDialog!=b)
			{
				m_IsOnCustomizeDialog=b;
				OnIsOnCustomizeDialogChanged();
			}
		}

		/// <summary>
		/// Returns whether item is hosted on menu or not.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool IsOnMenu
		{
			get
			{
				if(this.ContainerControl is MenuPanel || this.ContainerControl is ItemsListBox)
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// Returns whether item is hosted on menu bar or not.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual bool IsOnMenuBar
		{
			get
			{
				System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
				if(ctrl!=null && ctrl is Bar)
					return ((Bar)ctrl).MenuBar;
				return false;
			}
		}

		/// <summary>
		/// Returns whether item is hosted on bar or not.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool IsOnBar
		{
			get
			{
				System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
				if(ctrl!=null && ctrl is Bar)
					return true;
				return false;
			}
		}

		/// <summary>
		/// Returns whether item is in design mode or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false)]
		public virtual bool DesignMode
		{
			get
			{
				return m_DesignMode;
			}
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void SetDesignMode(bool b)
		{
			if(m_DesignMode!=b)
			{
				m_DesignMode=b;
				if(m_SubItems!=null)
				{
					foreach(BaseItem objItem in m_SubItems)
						objItem.SetDesignMode(m_DesignMode);
				}
				if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseLeave();
                    HotSubItem = null;
				}
				OnDesignModeChanged();
			}
		}

		/// <summary>
		/// Get or sets whether item has been changed in a way that it needs its size recalculated. This is internal
		/// property and it should not be used by your code.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool NeedRecalcSize
		{
			get
			{
				return m_NeedRecalcSize;
			}
			set
			{
				m_NeedRecalcSize=value;
				if(value && this.Parent!=null && this.ContainerControl == this.Parent.ContainerControl)
				{
					this.Parent.NeedRecalcSize=true;
				}
			}
		}
		

		/// <summary>
		/// Returns whether item is System item.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool SystemItem
		{
			get
			{
				return m_SystemItem;
			}
		}

		internal void SetSystemItem(bool b)
		{
			m_SystemItem=b;
		}

		/// <summary>
		/// Return Access key for the item.
		/// </summary>
		protected internal char AccessKey
		{
			get
			{
				return m_AccessKey;
			}
		}

		/// <summary>
		/// Gets or sets the collection of shortcut keys associated with the item.
		/// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Indicates list of shortcut keys for this item."), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public virtual ShortcutsCollection Shortcuts
		{
			get
			{
				if(m_Shortcuts==null)
					m_Shortcuts=new ShortcutsCollection(this);
				return m_Shortcuts;
			}
			set
			{
				IOwner owner=this.GetOwner() as IOwner;
				if(m_Shortcuts!=null && owner!=null)
                    owner.RemoveShortcutsFromItem(this);
                m_Shortcuts=value;
				m_Shortcuts.Parent=this;
				if(m_Shortcuts!=null && owner!=null)
					owner.AddShortcutsFromItem(this);
			}
		}

		/// <summary>
		/// Returns text representation of shortcut for this item.
		/// </summary>
		protected internal string ShortcutString
		{
			get 
			{
                return m_ShortcutString;
			}
		}

        internal void RefreshShortcutString()
        {
            m_ShortcutString = "";
            if (m_Shortcuts == null || m_Shortcuts.Count == 0)
                return;
            System.Windows.Forms.KeysConverter objConv = new System.Windows.Forms.KeysConverter();
            m_ShortcutString = objConv.ConvertToString((System.Windows.Forms.Keys)m_Shortcuts[0]);
        }

		/// <summary>
		/// Collapses all sub items by setting their Expanded property to false.
		/// </summary>
		/// <param name="item">Item to collapse.</param>
		public static void CollapseSubItems(BaseItem item)
		{
			if(item==null && item.SubItems.Count==0)
				return;
            BaseItem[] subItems = new BaseItem[item.SubItems.Count];
            item.SubItems.CopyTo(subItems, 0);
            foreach (BaseItem o in subItems)
                if (o.Expanded)
                    o.Expanded = false;
		}

		/// <summary>
		/// Collapses whole tree for the item starting with its parent.
		/// </summary>
		/// <param name="objItem">Item to collapse.</param>
		public static void CollapseAll(BaseItem objItem)
		{
			if(objItem==null)
				return;

			do
			{
				System.Windows.Forms.Control objCtrl=objItem.ContainerControl as System.Windows.Forms.Control;
				if(objCtrl is MenuPanel)
				{
					if(objItem.Parent!=null)
						objItem=objItem.Parent;
				}
				else if(objCtrl is Bar)
				{
					if(((Bar)objCtrl).ParentInternal==null)
						break;
					objItem=((Bar)objCtrl).ParentItem;
				}
				else if(objItem.Parent!=null)
					objItem=objItem.Parent;
			} while (objItem!=null && objItem.Parent!=null);

			if(objItem!=null)
			{
				objItem.Expanded=false;
				objItem.AutoExpand=false;
				if(objItem.Parent!=null)
					objItem.Parent.AutoExpand=false;
			}
		}

		/// <summary>
		/// Returns whether item is hosted on popup menu or bar.
		/// </summary>
		/// <param name="item">Item to get information for.</param>
		/// <returns></returns>
		public static bool IsOnPopup(BaseItem item)
		{
			if(item==null)
				return false;
			object objCont=item.ContainerControl;
			if(objCont==null)
				return false;
			if(objCont is MenuPanel)
				return true;
            if(objCont is Bar)
			{
				if(((Bar)objCont).BarState==eBarState.Popup)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
		public virtual bool ClickAutoRepeat
		{
			get
			{
				return m_ClickAutoRepeat;
			}
			set
			{
                m_ClickAutoRepeat=value;                
			}
		}

		/// <summary>
		/// Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(600),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
		public virtual int ClickRepeatInterval
		{
			get
			{
				return m_ClickRepeatInterval;
			}
			set
			{
				m_ClickRepeatInterval=value;                
			}
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>Hash code.</returns>
		public override int GetHashCode()
		{
			return m_Id;
		}

		internal eDesignInsertPosition DesignInsertMarker
		{
			get {return m_DesignInsertMarker;}
			set
			{
				if(m_DesignInsertMarker==value)
					return;
				m_DesignInsertMarker=value;
				//if(this.DesignMode)
				this.Refresh();
			}
		}

        private eDesignMarkerOrientation _DesignMarkerOrientation = eDesignMarkerOrientation.NotSet;
        /// <summary>
        /// Gets or sets the design-marker orientation for the item.
        /// </summary>
        protected internal virtual eDesignMarkerOrientation DesignMarkerOrientation
        {
            get
            {
                return _DesignMarkerOrientation;
            }
            set
            {
                _DesignMarkerOrientation = value;
            }
        }

        /// <summary>
        /// Gets whether design-time item drag & drop marker is horizontal.
        /// </summary>
		private bool IsDesignMarkHorizontal
		{
			get
			{
                if (_DesignMarkerOrientation != eDesignMarkerOrientation.NotSet)
                    return _DesignMarkerOrientation == eDesignMarkerOrientation.Horizontal;

                if (this.Parent is ItemContainer)
                {
                    if (((ItemContainer)this.Parent).LayoutOrientation == eOrientation.Horizontal)
                        return true;
                    else
                        return false;
                }

				if(m_Orientation==eOrientation.Horizontal && !this.IsOnMenu && !(this.Parent is SideBarPanelItem) && !(this.Parent is ExplorerBarGroupItem))
					return true;
                
				return false;
			}
		}

		protected internal void DrawInsertMarker(System.Drawing.Graphics g)
		{
            IOwner owner = GetOwner() as IOwner;
            if (m_DesignInsertMarker == eDesignInsertPosition.None || !this.Visible || !this.Displayed || !(owner != null && owner.DragInProgress || this.DesignMode))
                return;

            Color lineColor = ColorScheme.GetColor("834DD5");
            Color fillColor = ColorScheme.GetColor("CCCFF8");

            int size = 4;
            int lineThickness = 1;
            int padding = 2;

			if(IsDesignMarkHorizontal)
			{
                Point start = new Point(m_Rect.X, m_Rect.Y + padding), end = new Point(m_Rect.X, m_Rect.Bottom - (padding + 1));
                if (m_DesignInsertMarker == eDesignInsertPosition.After)
                {
                    start = new Point(m_Rect.Right - size * 2, m_Rect.Y + padding);
                    end = new Point(m_Rect.Right - size * 2, m_Rect.Bottom - (padding+1));
                }

                using (SolidBrush fillBrush = new SolidBrush(fillColor))
                {
                    using (Pen pen = new Pen(lineColor, 1))
                    {
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            path.AddLine(start.X, start.Y + size, start.X + size, start.Y);
                            path.AddLine(start.X + size * 2, start.Y + size, start.X + (size * 2 - (size - lineThickness)), start.Y + size);
                            path.AddLine(end.X + (size * 2 - (size - lineThickness)), end.Y - size, end.X + size * 2, end.Y - size);
                            path.AddLine(end.X + size, end.Y, end.X, end.Y - size);
                            path.AddLine(end.X + (size - lineThickness), end.Y - size, start.X + (size - lineThickness), start.Y + size);
                            path.CloseAllFigures();

                            g.FillPath(fillBrush, path);
                            g.DrawPath(pen, path);
                        }
                    }
                }
			}
			else
			{
                Point start = new Point(m_Rect.X + padding, m_Rect.Y), end = new Point(m_Rect.Right - (padding+1), m_Rect.Y);
                if (m_DesignInsertMarker == eDesignInsertPosition.After)
                {
                    start = new Point(m_Rect.X + padding, m_Rect.Bottom - (size * 2 + 1));
                    end = new Point(m_Rect.Right - (padding + 1), m_Rect.Bottom - (size * 2 + 1));
                }

                using (SolidBrush fillBrush = new SolidBrush(fillColor))
                {
                    using (Pen pen = new Pen(lineColor, 1))
                    {
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            path.AddLine(start.X, start.Y + size, start.X + size, start.Y);
                            path.AddLine(start.X + size, start.Y + (size - lineThickness), end.X - size, end.Y + (size - lineThickness));
                            path.AddLine(end.X - size, end.Y, end.X, end.Y + size);
                            path.AddLine(end.X - size, end.Y + size * 2, end.X - size, end.Y + (size*2 - (size-padding)));
                            path.AddLine(start.X + size, start.Y + (size * 2 - (size - padding)), start.X + size, start.Y + size * 2);

                            path.CloseAllFigures();

                            g.FillPath(fillBrush, path);
                            g.DrawPath(pen, path);
                        }
                    }
                }
                //if(m_DesignInsertMarker==eDesignInsertPosition.Before)
                //{
                //    p[0].X=m_Rect.Left+1;
                //    p[0].Y=m_Rect.Top;
                //    p[1].X=m_Rect.Left+1;
                //    p[1].Y=m_Rect.Top+4;
                //    g.DrawLines(pen,p);

                //    p[0].X=m_Rect.Left+1;
                //    p[0].Y=m_Rect.Top+2;
                //    p[1].X=m_Rect.Right-1;
                //    p[1].Y=m_Rect.Top+2;
                //    g.DrawLines(pen,p);

                //    p[0].X=m_Rect.Right-1;
                //    p[0].Y=m_Rect.Top;
                //    p[1].X=m_Rect.Right-1;
                //    p[1].Y=m_Rect.Top+4;
                //    g.DrawLines(pen,p);
                //}
                //else
                //{
                //    p[0].X=m_Rect.Left+1;
                //    p[0].Y=m_Rect.Bottom-4;
                //    p[1].X=m_Rect.Left+1;
                //    p[1].Y=m_Rect.Bottom;
                //    g.DrawLines(pen,p);

                //    p[0].X=m_Rect.Left+1;
                //    p[0].Y=m_Rect.Bottom-2;
                //    p[1].X=m_Rect.Right-1;
                //    p[1].Y=m_Rect.Bottom-2;
                //    g.DrawLines(pen,p);

                //    p[0].X=m_Rect.Right-1;
                //    p[0].Y=m_Rect.Bottom-4;
                //    p[1].X=m_Rect.Right-1;
                //    p[1].Y=m_Rect.Bottom;
                //    g.DrawLines(pen,p);
                //}
			}
            //g.SmoothingMode = sm;
		}

		/// <summary>
		/// Specifes the mouse cursor displayed when mouse is over the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(null),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies the mouse cursor displayed when mouse is over the item.")]
		public virtual System.Windows.Forms.Cursor Cursor
		{
			get
			{
				return m_Cursor;
			}
			set
			{
				if(m_Cursor!=value)
				{
					m_Cursor=value;
					if(m_Cursor!=null && m_Visible && m_Displayed)
					{
                        System.Windows.Forms.Control cont=this.ContainerControl as System.Windows.Forms.Control;
						if(cont!=null)
						{
							System.Drawing.Point p=cont.PointToClient(System.Windows.Forms.Control.MousePosition);
							if(m_Rect.Contains(p))
								cont.Cursor=m_Cursor;
						}
					}
				}
			}
		}

		[System.ComponentModel.Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override System.ComponentModel.ISite Site
		{
			get
			{
                return base.Site;
			}
			set
			{
				base.Site = value;
                OnSiteChanged();
			}
		}

        protected virtual void OnSiteChanged()
        {
            
        }

		/// <summary>
		/// Indicates whether item will be Serialized.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public bool ShouldSerialize
		{
			get
			{
				return m_ShouldSerialize;
			}
			set
			{
				m_ShouldSerialize=value;
			}
		}

		internal bool IsThemed
		{
			get
			{
				if(m_ThemeAware && BarFunctions.ThemedOS && Themes.ThemesActive)
					return true;
				return false;
			}
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public virtual bool IsWindowed
		{
			get {return false;}
		}

		/// <summary>
		/// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Appearance"),Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public virtual bool ThemeAware
		{
			get
			{
				return m_ThemeAware;
			}
			set
			{
				m_ThemeAware=value;
				if(m_SubItems!=null)
				{
					foreach(BaseItem item in m_SubItems)
						item.ThemeAware=value;
				}
			}
		}

		/// <summary>
		/// Gets the AccessibleObject assigned to the item.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false)]
		public virtual System.Windows.Forms.AccessibleObject AccessibleObject
		{
			get 
			{
				return CreateAccessibilityInstance(); //m_Accessible;
			}
		}

        private ItemAccessibleObject _ItemAccessibleObject = null;
		protected virtual System.Windows.Forms.AccessibleObject CreateAccessibilityInstance()
		{
            if (_ItemAccessibleObject == null)
                _ItemAccessibleObject = new ItemAccessibleObject(this);
            return _ItemAccessibleObject;
		}

		/// <summary>
		/// Gets or sets the default action description of the control for use by accessibility client applications.
		/// </summary>
		[DevCoBrowsable(true),System.ComponentModel.Browsable(true),System.ComponentModel.Category("Accessibility"),System.ComponentModel.DefaultValue(""),System.ComponentModel.Description("Gets or sets the default action description of the control for use by accessibility client applications.")]
		public virtual string AccessibleDefaultActionDescription
		{
			get {return m_AccessibleDefaultActionDescription;}
			set {m_AccessibleDefaultActionDescription=value;}
		}

		/// <summary>
		/// Gets or sets the description of the control used by accessibility client applications.
		/// </summary>
		[DevCoBrowsable(true),System.ComponentModel.Browsable(true),System.ComponentModel.Category("Accessibility"),System.ComponentModel.DefaultValue(""),System.ComponentModel.Description("Gets or sets the description of the control used by accessibility client applications.")]
		public virtual string AccessibleDescription
		{
			get {return m_AccessibleDescription;}
			set
			{
				if(m_AccessibleDescription!=value)
				{
					m_AccessibleDescription=value;
					GenerateAccessibilityEvent(System.Windows.Forms.AccessibleEvents.DescriptionChange);
				}
			}
		}

		/// <summary>
		/// Gets or sets the name of the control used by accessibility client applications.
		/// </summary>
		[DevCoBrowsable(true),System.ComponentModel.Browsable(true),System.ComponentModel.Category("Accessibility"),System.ComponentModel.Description("Gets or sets the name of the control used by accessibility client applications."),System.ComponentModel.DefaultValue("")]
		public virtual string AccessibleName
		{
			get {return m_AccessibleName;}
			set
			{
				if(m_AccessibleName!=value)
				{
					m_AccessibleName=value;
					GenerateAccessibilityEvent(System.Windows.Forms.AccessibleEvents.NameChange);
				}
			}
		}

		/// <summary>
		/// Gets or sets the accessible role of the item.
		/// </summary>
		[DevCoBrowsable(true),System.ComponentModel.Browsable(true),System.ComponentModel.Category("Accessibility"),System.ComponentModel.Description("Gets or sets the accessible role of the item."),System.ComponentModel.DefaultValue(System.Windows.Forms.AccessibleRole.Default)]
		public virtual System.Windows.Forms.AccessibleRole AccessibleRole
		{
			get {return m_AccessibleRole;}
			set {m_AccessibleRole=value;}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is visible to accessibility applications.
		/// </summary>
		[DevCoBrowsable(false),System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public virtual bool IsAccessible
		{
			get {return m_IsAccessible;}
			set {m_IsAccessible=value;}
		}

		internal void GenerateAccessibilityEvent(System.Windows.Forms.AccessibleEvents e)
		{
			if(this.ContainerControl is Bar && ((Bar)this.ContainerControl).m_AccessibleObjectCreated)
			{
				Bar.BarAccessibleObject a=((Bar)this.ContainerControl).AccessibilityObject as Bar.BarAccessibleObject;
				if(a!=null)
					a.GenerateEvent(this,e);
			}
//			else if(this.ContainerControl is PopupMenu && ((PopupMenu)this.ContainerControl).m_AccessibleObjectCreated)
//			{
//				PopupMenu.PopupMenuAccessibleObject a=((PopupMenu)this.ContainerControl).AccessibilityObject as PopupMenu.PopupMenuAccessibleObject;
//				if(a!=null)
//					a.GenerateEvent(this,e);
//			}
		}
        internal bool _AccessibleExpandAction = false;
		internal virtual void DoAccesibleDefaultAction()
		{
			if(this.VisibleSubItems>0 && (this.IsOnMenu || this.IsOnMenuBar || _AccessibleExpandAction))
			{
				if(this.Expanded)
				{
					this.Expanded=false;
					if(this.Parent!=null && this.IsOnMenuBar)
						this.Parent.AutoExpand=false;
				}
				else
				{
					if(this.IsOnMenuBar && this.Parent!=null)
						this.Parent.AutoExpand=true;
					this.Expanded=true;
				}
				this.Refresh();
                _AccessibleExpandAction = false;
			}
			else
				this.RaiseClick(eEventSource.Keyboard);
        }
        #endregion

        #region ICommandSource Members
        protected virtual void ExecuteCommand()
        {
            if (_Command == null) return;
            CommandManager.ExecuteCommand(this);
        }

        internal void ExecuteCommandInternal()
        {
            ExecuteCommand();
        }

        /// <summary>
        /// Gets or sets the command assigned to the item. Default value is null.
        /// <remarks>Note that for ButtonItem instances if this property is set to null and command was assigned previously, Enabled property will be set to false automatically to disable the item.</remarks>
        /// </summary>
        [DefaultValue(null), Category("Commands"), Description("Indicates the command assigned to the item.")]
        public virtual Command Command
        {
            get { return (Command)((ICommandSource)this).Command; }
            set
            {
                ((ICommandSource)this).Command = value;
                if (ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "Command");
            }
        }

        private ICommand _Command = null;
        //[System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        ICommand ICommandSource.Command
        {
            get
            {
                return _Command;
            }
            set
            {
                bool changed = false;
                if (_Command != value)
                    changed = true;

                if (_Command != null)
                    CommandManager.UnRegisterCommandSource(this, _Command);
                _Command = value;
                if (value != null)
                    CommandManager.RegisterCommand(this, value);
                if (changed)
                    OnCommandChanged();
            }
        }
        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected virtual void OnCommandChanged()
        {
        }

        private object _CommandParameter = null;
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Commands"), Description("Indicates user defined data value that can be passed to the command when it is executed."), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)), System.ComponentModel.Localizable(true)]
        public virtual object CommandParameter
        {
            get
            {
                return _CommandParameter;
            }
            set
            {
                _CommandParameter = value;
                if (ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "CommandParameter");
            }
        }

        #endregion

        #region ItemAccessibleObject
        public class ItemAccessibleObject: System.Windows.Forms.AccessibleObject
		{
			private BaseItem m_Owner=null;
			private bool m_Hot=false;
			public ItemAccessibleObject(BaseItem owner)
			{
				m_Owner = owner;
				m_Owner.MouseEnter+=new EventHandler(OnMouseEnter);
                m_Owner.MouseLeave+=new EventHandler(OnMouseLeave);
			}

			protected virtual void OnMouseEnter(object sender, System.EventArgs e)
			{
				m_Hot=true;
			}

            protected virtual void OnMouseLeave(object sender, System.EventArgs e)
			{
				m_Hot=false;
			}

            internal BaseItem Owner
            {
                get { return m_Owner; }
            }

			public override string Name 
			{
				get 
				{
					if(m_Owner==null)
						return"";

					if(m_Owner.AccessibleName!="")
						return m_Owner.AccessibleName;

					if(m_Owner.Text!=null)
						return m_Owner.Text.Replace("&", "");
					
					return m_Owner.Tooltip;
				}
				set
				{
					m_Owner.AccessibleName=value;
				}
			}

			public override string Description
			{
				get 
				{ 
					if(m_Owner==null)
						return "";
					if(m_Owner.AccessibleDescription!="")
						return m_Owner.AccessibleDescription;
					if (m_Owner.IsOnMenu)
						return Name + " menu item"; 
					else if(m_Owner.IsOnMenuBar)
						return Name + " menu"; 
					else
						return Name + " button"; 
				}
			}

			public override System.Windows.Forms.AccessibleRole Role
			{
				get 
				{
					if(m_Owner==null || !m_Owner.IsAccessible)
						return System.Windows.Forms.AccessibleRole.None;

					if(m_Owner.AccessibleRole!=System.Windows.Forms.AccessibleRole.Default)
						return m_Owner.AccessibleRole;
					BaseItem topParent=m_Owner;
					while(topParent.Parent!=null)
						topParent=topParent.Parent;
					return (m_Owner.IsOnMenu || m_Owner.IsOnMenuBar || topParent.IsOnMenuBar ? 
						System.Windows.Forms.AccessibleRole.MenuItem : 
						System.Windows.Forms.AccessibleRole.PushButton); 
				}
			}

			public override System.Windows.Forms.AccessibleStates State
			{
				get 
				{ 
					if(m_Owner==null)
						return System.Windows.Forms.AccessibleStates.Unavailable;
					//System.Diagnostics.Trace.WriteLine("Getting state for "+m_Owner.Text);
					System.Windows.Forms.AccessibleStates state=0;

					if(!m_Owner.IsAccessible)
						return System.Windows.Forms.AccessibleStates.Unavailable;

					if(!m_Owner.Displayed || !m_Owner.Visible)
						state |= System.Windows.Forms.AccessibleStates.Invisible;
                    else if (!m_Owner.GetEnabled())
					{
						state=System.Windows.Forms.AccessibleStates.Unavailable;
						if(m_Owner.Expanded || m_Hot || m_Owner is ButtonItem && ((ButtonItem)m_Owner).IsMouseOver)
							state|=(System.Windows.Forms.AccessibleStates.HotTracked | System.Windows.Forms.AccessibleStates.Focused);
						return state;
					}
					else
					{
						if(m_Owner.Expanded || m_Hot || m_Owner is ButtonItem && ((ButtonItem)m_Owner).IsMouseOver)
							state |= (System.Windows.Forms.AccessibleStates.Focused | System.Windows.Forms.AccessibleStates.HotTracked);
						else
							state |= System.Windows.Forms.AccessibleStates.Focusable;
						if(m_Owner.ShowSubItems && m_Owner.SubItems.Count>0)
						{
							if(m_Owner.Expanded)
								state|=System.Windows.Forms.AccessibleStates.Expanded;
							else
								state|=System.Windows.Forms.AccessibleStates.Collapsed;
						}
					}
					

//					if(m_Owner.SubItems.Count>0 && m_Owner.ShowSubItems)
//						state|=(System.Windows.Forms.AccessibleStates)0x40000000;

					return state;
				}
			}

			/*public override string Value
			{
				get { return m_owner.AccessibleRole; }
				set { m_owner.AccessibleRole = value; }
			}*/

			public override System.Windows.Forms.AccessibleObject Parent 
			{
				get	
				{
					if(m_Owner==null)
						return null;
					if(m_Owner.Parent!=null)
					{
						if(!(m_Owner.Parent is GenericItemContainer && ((GenericItemContainer)m_Owner.Parent).SystemContainer))
							return m_Owner.Parent.AccessibleObject;
					}

					System.Windows.Forms.Control control=m_Owner.ContainerControl as System.Windows.Forms.Control;
					if(control!=null)
						return control.AccessibilityObject;
					return null;
				}
			}

			public override System.Drawing.Rectangle Bounds 
			{
				get 
				{ 
					if(m_Owner==null)
						return System.Drawing.Rectangle.Empty;

					System.Windows.Forms.Control objCtrl=m_Owner.ContainerControl as System.Windows.Forms.Control;
					if (objCtrl != null)
						return objCtrl.RectangleToScreen(m_Owner.DisplayRectangle);
					else
						return m_Owner.m_Rect; 
				}
			}

			public override int GetChildCount()
			{
				if(m_Owner==null)
					return 0;
				return m_Owner.SubItems.Count;
			}

			public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
			{
                if (m_Owner == null || iIndex < 0 || iIndex >= m_Owner.SubItems.Count)
                    return null;
				return m_Owner.SubItems[iIndex].AccessibleObject;
			}

			public override string DefaultAction 
			{
				get
				{
					if(m_Owner.AccessibleDefaultActionDescription!="")
						return m_Owner.AccessibleDefaultActionDescription;
					return "Press";
				}
			}

			public override void DoDefaultAction()
			{
				if(m_Owner==null)
					return;

				System.Windows.Forms.Control cont=m_Owner.ContainerControl as System.Windows.Forms.Control;
				if(cont is MenuPanel && !(cont is IAccessibilitySupport))
				{
					cont=((MenuPanel)cont).ParentItem.ContainerControl as System.Windows.Forms.Control;
				}

                IAccessibilitySupport ias = cont as IAccessibilitySupport;
				if(ias!=null)
				{
					ias.DoDefaultActionItem=m_Owner;
					NativeFunctions.PostMessage(cont.Handle.ToInt32(),NativeFunctions.WM_USER+107,0,0);
				}
				
				base.DoDefaultAction();
			}

			public override string KeyboardShortcut
			{
				get
				{
					return m_Owner.ShortcutString;
				}
			}

            public override System.Windows.Forms.AccessibleObject GetSelected()
            {
                if (m_Owner == null)
                    return base.GetSelected();
                foreach (BaseItem item in m_Owner.SubItems)
                {
                    if ((item.AccessibleObject.State & System.Windows.Forms.AccessibleStates.HotTracked) == System.Windows.Forms.AccessibleStates.HotTracked)
                        return item.AccessibleObject;
                }

                return base.GetSelected();
            }

            public override System.Windows.Forms.AccessibleObject HitTest(int x, int y)
            {
                if (m_Owner == null)
                    return base.HitTest(x, y);

                Point screen=new Point(x,y);
                foreach (BaseItem item in m_Owner.SubItems)
                {
                    System.Windows.Forms.Control cont = item.ContainerControl as System.Windows.Forms.Control;
                    if (cont != null)
                    {
                        Point p = cont.PointToClient(screen);
                        if (item.DisplayRectangle.Contains(p))
                            return item.AccessibleObject;
                    }
                }

                return base.HitTest(x, y);
            }

            public override System.Windows.Forms.AccessibleObject Navigate(System.Windows.Forms.AccessibleNavigation navdir)
            {
                if (m_Owner == null)
                    return base.Navigate(navdir);
                
                BaseItem item = null;

                if (navdir == System.Windows.Forms.AccessibleNavigation.Down || navdir == System.Windows.Forms.AccessibleNavigation.Right)
                {
                    if (m_Owner.Parent != null)
                    {
                        BaseItem parent = m_Owner.Parent;
                        item = GetFirstVisible(parent.SubItems, parent.SubItems.IndexOf(m_Owner) + 1);
                    }
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.Next)
                {
                    if (m_Owner != null && m_Owner.Parent != null)
                    {
                        int index = m_Owner.Parent.SubItems.IndexOf(m_Owner);
                        if (index < m_Owner.Parent.SubItems.Count - 1)
                            item = m_Owner.Parent.SubItems[index + 1];
                    }
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.Previous)
                {
                    if (m_Owner != null && m_Owner.Parent != null)
                    {
                        int index = m_Owner.Parent.SubItems.IndexOf(m_Owner);
                        if (index > 0)
                            item = m_Owner.Parent.SubItems[index - 1];
                    }
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.FirstChild)
                {
                    item = GetFirstVisible(m_Owner.SubItems, 0);
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.LastChild)
                {
                    item = GetFirstVisibleReverse(m_Owner.SubItems, m_Owner.SubItems.Count - 1);
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.Up)
                {
                    if (m_Owner != null && m_Owner.Parent.IsContainer && m_Owner.Parent.SystemItem)
                    {
                        Control container = m_Owner.Parent.ContainerControl as Control;
                        if (container != null) return container.AccessibilityObject;
                    }
                    if (m_Owner.Parent != null) return m_Owner.Parent.AccessibleObject;
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.Left)
                {
                    BaseItem parent = m_Owner.Parent;
                    item = GetFirstVisibleReverse(m_Owner.SubItems, parent.SubItems.IndexOf(m_Owner) - 1);
                }

                if (item != null)
                    return item.AccessibleObject;

                return base.Navigate(navdir);
            }

            private BaseItem GetFirstVisible(SubItemsCollection col, int startIndex)
            {
                int count = col.Count;
                if (count == 0) return null;
                if (startIndex >= col.Count) startIndex = col.Count - 1;
                for (int i = startIndex; i < count; i++)
                {
                    if (col[i].Visible)
                        return col[i];
                }
                return null;
            }

            private BaseItem GetFirstVisibleReverse(SubItemsCollection col, int startIndex)
            {
                if (col.Count == 0) return null;
                if (startIndex >= col.Count) startIndex = col.Count - 1;
                for (int i = startIndex; i >= 0; i--)
                {
                    if (col[i].Visible)
                        return col[i];
                }
                return null;
            }
            
        }
        #endregion

        #region Markup Implementation
        private TextMarkup.BodyElement m_TextMarkup = null;

        private void MarkupTextChanged()
        {
            m_TextMarkup = null;

            if (!IsMarkupSupported)
                return;

            if (!TextMarkup.MarkupParser.IsMarkup(ref m_Text))
                return;

            m_TextMarkup = TextMarkup.MarkupParser.Parse(m_Text);

            if(m_TextMarkup!=null)
                m_TextMarkup.HyperLinkClick += new EventHandler(TextMarkupLinkClick);
        }

        /// <summary>
        /// Occurs when text markup link is clicked.
        /// </summary>
        protected virtual void TextMarkupLinkClick(object sender, EventArgs e) {}

        /// <summary>
        /// Gets reference to parsed markup body element if text was markup otherwise returns null.
        /// </summary>
        internal TextMarkup.BodyElement TextMarkupBody
        {
            get { return m_TextMarkup; }
        }

        /// <summary>
        /// Gets plain text without text-markup if text-markup is used in Text.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string PlainText
        {
            get
            {
                return m_TextMarkup != null ? m_TextMarkup.PlainText : Text;
            }
        }

        /// <summary>
        /// Gets whether item supports text markup. Default is false.
        /// </summary>
        protected virtual bool IsMarkupSupported
        {
            get { return false; }
        }
        #endregion

        #region IBindableComponent Members
#if FRAMEWORK20
        private BindingContext _BindingContext = null;
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BindingContext BindingContext
        {
            get
            {
                if (_BindingContext != null) return _BindingContext;
                Control container = this.ContainerControl as Control;
                if (container != null)
                {
                    if (container is MenuPanel)
                    {
                        object owner = this.GetOwner();
                        if (owner is DotNetBarManager && ((DotNetBarManager)owner).ParentForm != null)
                            return ((DotNetBarManager)owner).ParentForm.BindingContext;
                        else if (owner is Control)
                            return ((Control)owner).BindingContext;
                    }
                    else
                        return container.BindingContext;
                }
                else
                {
                    object owner = this.GetOwner();
                    if (owner is DotNetBarManager && ((DotNetBarManager)owner).ParentForm != null)
                        return ((DotNetBarManager)owner).ParentForm.BindingContext;
                    else if (owner is Control)
                    {
                        return ((Control)owner).BindingContext;
                    }
                }

                return null;
            }
            set
            {
                _BindingContext = value;
                UpdateBindings();
            }
        }

        /// <summary>
        /// Updates data bindings for item and its sub-items in response to binding context change on parent control.
        /// </summary>
        public void UpdateBindings()
        {
            if(_DataBindings!=null)
            {
                BindingContext context = this.BindingContext;
                for (int i = 0; i < _DataBindings.Count; i++)
                {
                    BindingContext.UpdateBinding(context, _DataBindings[i]);
                }
            }
            if (m_SubItems != null)
            {
                foreach (BaseItem item in m_SubItems)
                {
                    item.UpdateBindings();
                }
            }
        }

        private ControlBindingsCollection _DataBindings = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Data"), ParenthesizePropertyName(true), RefreshProperties(RefreshProperties.All)]
        public ControlBindingsCollection DataBindings
        {
            get 
            {
                if (_DataBindings == null)
                {
                    _DataBindings = new ControlBindingsCollection(this);
                }
                return _DataBindings;
            }
        }
#endif
        #endregion

        #region Invoke Support
        /// <summary>
        /// Gets a value indicating whether the caller must call an invoke method when making method calls to the item because the caller is on a different thread than the one the item was created on.
        /// This property calls directly the ContainerControl.InvokeRequired and is provided as shortcut convinience property only.
        /// </summary>
        [Browsable(false)]
        public bool InvokeRequired 
        {
            get
            {
                Control parent = this.ContainerControl as Control;
                if (parent != null) return parent.InvokeRequired;
                return false;
            }
        }

        /// <summary>
        /// Executes the specified delegate, on the thread that owns the control's underlying window handle, with the specified list of arguments.
        /// This property calls directly the ContainerControl.Invoke and is provided as shortcut convinience property only.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters of the same number and type that are contained in the args parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to the specified method. This parameter can be nullNothingnullptra null reference (Nothing in Visual Basic) if the method takes no arguments.</param>
        /// <returns>An Object that contains the return value from the delegate being invoked, or nullNothingnullptra null reference (Nothing in Visual Basic) if the delegate has no return value.</returns>
        public Object Invoke(Delegate method,Object[] args)
        {
            Control parent = this.ContainerControl as Control;
            return parent.Invoke(method, args);
        }

        /// <summary>
        /// Executes the specified delegate, on the thread that owns the control's underlying window handle, with the specified list of arguments.
        /// This property calls directly the ContainerControl.Invoke and is provided as shortcut convinience property only.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters of the same number and type that are contained in the args parameter.</param>
        /// <returns>An Object that contains the return value from the delegate being invoked, or nullNothingnullptra null reference (Nothing in Visual Basic) if the delegate has no return value.</returns>
        public Object Invoke(Delegate method)
        {
            Control parent = this.ContainerControl as Control;
            return parent.Invoke(method);
        }
        #endregion
    }
}
