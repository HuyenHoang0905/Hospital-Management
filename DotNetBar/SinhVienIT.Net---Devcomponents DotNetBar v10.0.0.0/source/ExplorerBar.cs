using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents the Outlook like Explorer-bar Control.
	/// </summary>
    [ToolboxItem(true), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.ExplorerBarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class ExplorerBar : System.Windows.Forms.Control,
		IOwnerItemEvents, IOwner, IThemeCache, IMessageHandlerClient, IOwnerMenuSupport,
        ISupportInitialize, IBarDesignerServices, ICustomSerialization, IAccessibilitySupport
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
		/// Occurs before an item in option group is checked and provides opportunity to cancel that.
		/// </summary>
		public event OptionGroupChangingEventHandler OptionGroupChanging;

		/// <summary>
		/// Occurs before tooltip for an item is shown. Sender could be the BaseItem or derived class for which tooltip is being displayed or it could be a ToolTip object itself it tooltip is not displayed for any item in particular.
		/// </summary>
		public event EventHandler ToolTipShowing;

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
		#endregion

		#region Private Variables
		private ExplorerBarContainerItem m_ItemContainer=null;

		private System.Windows.Forms.Timer m_ClickTimer=null;
		private BaseItem m_ClickRepeatItem=null;
		private BaseItem m_ExpandedItem=null;
		private BaseItem m_FocusItem=null;

		private System.Windows.Forms.ImageList m_ImageList;
		private System.Windows.Forms.ImageList m_ImageListMedium=null;
		private System.Windows.Forms.ImageList m_ImageListLarge=null;

		private bool m_ShowToolTips=true;
		private bool m_ShowShortcutKeysInToolTips=false;

		private Hashtable m_ShortcutTable=new Hashtable();
		private eBorderType m_BorderStyle=eBorderType.None;

		private ExplorerBarGroupItem m_DelayedExpandedPanel=null;

		private BaseItem m_DragItem=null;
		private bool m_DragInProgress=false;
		private Cursor m_MoveCursor, m_CopyCursor, m_NACursor;
		private IDesignTimeProvider m_DesignTimeProvider=null;
		private int m_InsertPosition;
		private bool m_InsertBefore;
		private bool m_AllowUserCustomize=true;
		private bool m_ThemeAware=false;

		// Theme Caching Support
		private ThemeWindow m_ThemeWindow=null;
		private ThemeRebar m_ThemeRebar=null;
		private ThemeToolbar m_ThemeToolbar=null;
		private ThemeHeader m_ThemeHeader=null;
		private ThemeScrollBar m_ThemeScrollBar=null;
		private ThemeExplorerBar m_ThemeExplorerBar=null;
		private ThemeProgress m_ThemeProgress=null;
        private ThemeButton m_ThemeButton = null;

		private bool m_UseNativeDragDrop=false;
		private bool m_AllowExternalDrop=false;
		private bool m_ExternalDragInProgress=false;
		private bool m_DragLeft=false;
		private VScrollBar m_ScrollBar=null;
		private int m_Margin=4;
		private ItemStyleMapper m_BackgroundStyle=null;
        private ElementStyle m_BackStyle = new ElementStyle();
		private eExplorerBarStockStyle m_StockStyle=eExplorerBarStockStyle.Custom;
		private bool m_AnimationEnabled=true;
		private int m_AnimationTime=100;
		private bool m_DispatchShortcuts=false;
		private bool m_FilterInstalled=false;

		private const string INFO_EMPTYEXPLORERBAR="Right-click and choose Add New Group or use Groups collection to create new groups.";

		private bool m_MenuEventSupport=false;
		private bool m_VisualPropertyChanged=false;

		private ColorScheme m_ColorScheme=null;
		private bool m_AntiAlias=true;

		private IBarItemDesigner m_BarDesigner=null;
		private bool m_SuspendLayout=false;

        private Image m_GroupButtonExpandNormal = null;
        private Image m_GroupButtonExpandHot = null;
        private Image m_GroupButtonExpandPressed = null;
        private Image m_GroupButtonCollapseNormal = null;
        private Image m_GroupButtonCollapseHot = null;
        private Image m_GroupButtonCollapsePressed = null;
        private BaseItem m_DoDefaultActionItem = null;
        private bool m_DesignerSelection = false;
		#endregion

		/// <summary>
		/// Creates new instance of side bar control.
		/// </summary>
		public ExplorerBar()
		{
			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}
			m_ColorScheme=new ColorScheme(eDotNetBarStyle.Office2003);

			m_ItemContainer=new ExplorerBarContainerItem();
			m_ItemContainer.GlobalItem=false;
			m_ItemContainer.ContainerControl=this;
			m_ItemContainer.Stretch=false;
			m_ItemContainer.Displayed=true;
			m_ItemContainer.SetOwner(this);

			//this.SetStyle(ControlStyles.Selectable,false);
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.SetStyle(ControlStyles.ContainerControl,true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor,true);

			this.TabStop=true;

			#if TRIAL
				RemindForm frm=new RemindForm();
				frm.ShowDialog();
			#endif

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

			ApplyDefaultSettings();
			m_BackStyle.StyleChanged+=new EventHandler(this.VisualPropertyChanged);

			this.AccessibleRole=AccessibleRole.ToolBar;

            m_BackgroundStyle = new ItemStyleMapper(m_BackStyle);
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
                if (m_ItemContainer != null)
                    m_ItemContainer.Dispose();
                m_ItemContainer = null;

                DisposeCursor(ref m_MoveCursor);
                DisposeCursor(ref m_CopyCursor);
                DisposeCursor(ref m_NACursor);
			}
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_GroupButtonCollapseHot);
                BarUtilities.DisposeImage(ref m_GroupButtonCollapseNormal);
                BarUtilities.DisposeImage(ref m_GroupButtonCollapsePressed);
                BarUtilities.DisposeImage(ref m_GroupButtonExpandHot);
                BarUtilities.DisposeImage(ref m_GroupButtonExpandNormal);
                BarUtilities.DisposeImage(ref m_GroupButtonExpandPressed);
            }
			base.Dispose( disposing );
		}

        private void DisposeCursor(ref Cursor cursor)
        {
            if (cursor != null)
            {
                cursor.Dispose();
                cursor = null;
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

		/// <summary>
		/// Suspends the layout and painting of the control. When you want to perform multiple operations on the explorer bar and you want to ensure
		/// that layout of the items on it and painting is not performed for performance and appearance reasons set this property to true. Once you
		/// want to enable layout and painting set the property back to false and call RecalcLayout method. Default value is false.
		/// </summary>
		[Browsable(false),DefaultValue(false)]
		public bool SuspendLayoutDisplay
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

		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ExplorerBarAccessibleObject(this);
		}

		private void VisualPropertyChanged(object sender, EventArgs e)
		{
			m_VisualPropertyChanged=true;
			ColorScheme cs=this.ColorScheme;
			m_BackStyle.SetColorScheme(cs);
			if(this.IsTransparentBackground)
				this.BackColor=Color.Transparent;
			else
				this.BackColor=SystemColors.Control;

			if(this.DesignMode)
			{
				this.Refresh();
			}
		}

		protected override void OnClick(EventArgs e)
		{
			m_ItemContainer.InternalClick(Control.MouseButtons,Control.MousePosition);
			base.OnClick(e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
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

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
            ((IOwner)this).SetFocusItem(null);			
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if(m_ItemContainer!=null)
				m_ItemContainer.FocusNextItem();
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if(this.Focused)
			{
				if(keyData==Keys.Down)
				{
					if(m_ItemContainer!=null)
						m_ItemContainer.FocusNextItem();
					if(((IOwner)this).GetFocusItem()!=null)
						return true;
				}
				else if(keyData==Keys.Up)
				{
					if(m_ItemContainer!=null)
						m_ItemContainer.FocusPreviousItem();
					if(((IOwner)this).GetFocusItem()!=null)
						return true;
				}
				else if(keyData==Keys.Enter)
				{
					if(((IOwner)this).GetFocusItem()!=null)
					{
						BaseItem item=((IOwner)this).GetFocusItem();
                        if(item is ExplorerBarGroupItem)
							item.Expanded=!item.Expanded;
						else
							item.RaiseClick();
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			m_ItemContainer.InternalMouseDown(e);
			base.OnMouseDown(e);
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			m_ItemContainer.InternalMouseHover();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			// If we had hot sub item pass the mouse leave message to it...
			if(this.Cursor!=System.Windows.Forms.Cursors.Arrow)
				this.Cursor=System.Windows.Forms.Cursors.Arrow;
			m_ItemContainer.InternalMouseLeave();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(m_DragInProgress)
			{
				if(!m_UseNativeDragDrop)
					MouseDragOver(e.X,e.Y,null);
			}
			else
				m_ItemContainer.InternalMouseMove(e);
//			if(EventMouseMove!=null)
//				EventMouseMove(this,e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if(m_ScrollBar!=null)
			{
				int change=(Math.Abs(e.Delta)/120)*m_ScrollBar.SmallChange;
				if(e.Delta<0)
				{
					int val=m_ScrollBar.Value+change;
					if(val>m_ScrollBar.Maximum)
						val=m_ScrollBar.Maximum;
					m_ScrollBar.Value=val;
				}
				else
				{
					int val=m_ScrollBar.Value-change;
					if(val<0) val=0;
					m_ScrollBar.Value=val;
				}
				UpdateScrollValue(m_ScrollBar.Value);
			}
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			if(m_DragInProgress || m_ExternalDragInProgress)
			{
				Point p=this.PointToClient(new Point(e.X,e.Y));
				MouseDragOver(p.X,p.Y,e);
				m_DragLeft=false;
			}
			base.OnDragOver(e);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if(m_DragInProgress || m_ExternalDragInProgress)
				MouseDragOver(-1,-1,null);
			m_DragLeft=true;
			m_ExternalDragInProgress=false;
			base.OnDragLeave(e);
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
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

		protected override void OnDragDrop(DragEventArgs e)
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
			base.OnDragDrop(e);
		}

		protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			if(m_DragInProgress)
			{
				if(m_DragLeft && e.Action==DragAction.Drop || e.Action==DragAction.Cancel)
					MouseDragDrop(-1,-1,null);
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
			foreach(ExplorerBarGroupItem panel in m_ItemContainer.SubItems)
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

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(!m_UseNativeDragDrop)
				MouseDragDrop(e.X,e.Y,null);

			m_ItemContainer.InternalMouseUp(e);
		}

		private void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
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
							//Control ctrl=objParent.ContainerControl as Control;
							//objParent.Refresh();
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
			m_ItemContainer.InternalMouseUp(new MouseEventArgs(MouseButtons.Left,0,x,y,0));
			if(dragItem!=null)
				dragItem._IgnoreClick=false;

			m_DragItem=null;
			this.RecalcLayout();
		}
		/// <summary>
		/// Gets or sets Bar Color Scheme.
		/// </summary>
		[System.ComponentModel.Editor(typeof(ColorSchemeVSEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Gets or sets Color Scheme."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DevComponents.DotNetBar.ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
			set
			{
				if(value==null)
					throw new ArgumentException("NULL is not a valid value for this property.");
				m_ColorScheme=value;
				m_VisualPropertyChanged=true;
				foreach(ExplorerBarGroupItem item in this.Groups)
					item.VisualPropertyChanged();
				if(this.Visible)
					this.Refresh();
			}
		}
		/// <summary>
		/// Returns true if color scheme has changed.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColorScheme()
		{
			return m_ColorScheme.SchemeChanged;
		}
		/// <summary>
		/// Resets color scheme to it's default value.
		/// </summary>
		public void ResetColorScheme()
		{
			m_ColorScheme=new ColorScheme(eDotNetBarStyle.Office2003);
		}

		private bool IsTransparentBackground
		{
			get
			{
				if((m_BackStyle.BackColor.IsEmpty && m_BackStyle.BackColor2.IsEmpty) || 
					(m_BackStyle.BackColor==Color.Transparent && m_BackStyle.BackColor2==Color.Transparent))
                    return true;
				return false;
			}
		}

        /// <summary>
        /// Creates the Graphics object for the control.
        /// </summary>
        /// <returns>The Graphics object for the control.</returns>
        public new Graphics CreateGraphics()
        {
            Graphics g = base.CreateGraphics();
            if(m_AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				#if FRAMEWORK20
                if (!SystemInformation.IsFontSmoothingEnabled)
                #endif
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
            return g;
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			if(m_ItemContainer==null || this.IsDisposed || m_SuspendLayout)
				return;

			using(SolidBrush brush=new SolidBrush(Color.White))
				e.Graphics.FillRectangle(brush,this.DisplayRectangle);
            
            Graphics g = e.Graphics;

            SmoothingMode sm = g.SmoothingMode;
            TextRenderingHint th = g.TextRenderingHint;

			if(m_AntiAlias)
			{
				e.Graphics.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
            
			g.PageUnit=GraphicsUnit.Pixel;

			Rectangle r=this.ClientRectangle;
			
			ColorScheme cs=this.ColorScheme;
			if(m_VisualPropertyChanged)
			{
				m_BackStyle.SetColorScheme(cs);
				m_VisualPropertyChanged=false;
			}

            Rectangle rBack = r;
            rBack.Inflate(1, 1);
            ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(m_BackStyle, g, rBack);
            ElementStyleDisplay.Paint(info);
			
			if(m_BorderStyle==eBorderType.SingleLine)
			{
				r.Inflate(-1,-1);
			}
			else if(m_BorderStyle!=eBorderType.None)
			{
				r.Inflate(-2,-2);
			}
			
			if(this.IsTransparentBackground)
				base.OnPaintBackground(e);

			//g.FillRectangle(new SolidBrush(this.BackColor),r);
			Region oldClip=e.Graphics.Clip;
			e.Graphics.SetClip(r);
			
			ItemPaintArgs pa=new ItemPaintArgs(this as IOwner,this,e.Graphics,cs); // TODO: ADD SUPPORT FOR GRAPHICS
            pa.Renderer = GetRenderer();
            pa.DesignerSelection = m_DesignerSelection;
            pa.GlassEnabled = !this.DesignMode && WinApi.IsGlassEnabled;
			m_ItemContainer.Paint(pa);
			if(oldClip!=null)
				e.Graphics.SetClip(oldClip,System.Drawing.Drawing2D.CombineMode.Replace);
			else
				e.Graphics.ResetClip();
            oldClip.Dispose();

			if(m_ItemContainer.SubItems.Count==0 && this.DesignMode)
			{
				string infoText=INFO_EMPTYEXPLORERBAR;
				Rectangle rText=this.ClientRectangle;
				r.Inflate(-2,-2);
                eTextFormat format = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				Font font=new Font(this.Font.FontFamily,7);
                TextDrawing.DrawString(g, infoText, font, SystemColors.ControlText, rText, format);
				font.Dispose();
			}

            g.SmoothingMode = sm;
            g.TextRenderingHint = th;
		}

        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        private Rendering.BaseRenderer GetRenderer()
        {
            return Rendering.GlobalManager.Renderer;
        }

		protected bool IsThemed
		{
			get
			{
                if (m_ThemeAware && (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.OfficeXP || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003) && BarFunctions.ThemedOS && Themes.ThemesActive)
					return true;
				return false;
			}
		}

		/// <summary>
		/// Specifies whether ExplorerBar is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies whether ExplorerBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
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
				if(this.DesignMode)
					this.Refresh();
			}
		}

        /// <summary>
        /// Specifies the background style of the Explorer Bar.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets bar background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackStyle
        {
            get { return m_BackStyle; }
        }

        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("This property is obsolete. Use BackStyle property instead"), EditorBrowsable(EditorBrowsableState.Never)]
		public ItemStyleMapper BackgroundStyle
		{
			get {return m_BackgroundStyle;}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void ApplyDefaultSettings()
		{
			ColorScheme cs=this.ColorScheme;
			m_BackStyle.BackColorSchemePart=eColorSchemePart.ExplorerBarBackground;
			m_BackStyle.BackColor2SchemePart=eColorSchemePart.ExplorerBarBackground2;
			m_BackStyle.BackColorGradientAngle=cs.ExplorerBarBackgroundGradientAngle;
			m_BackStyle.SetColorScheme(cs);
		}

		/// <summary>
		/// Gets or sets the vertical spacing between the group items.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Specifies the vertical spacing between the group items."),DefaultValue(15)]
		public int GroupSpacing
		{
			get {return m_ItemContainer.m_ItemSpacing;}
			set
			{
				m_ItemContainer.m_ItemSpacing=value;
				this.RecalcLayout();
			}
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			Application.DoEvents();
			this.RefreshThemes();
			this.ColorScheme.Refresh(null,true);
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
			if(m_ThemeExplorerBar!=null)
			{
				m_ThemeExplorerBar.Dispose();
				m_ThemeExplorerBar=new ThemeExplorerBar(this);
			}
			if(m_ThemeProgress!=null)
			{
				m_ThemeProgress.Dispose();
				m_ThemeProgress=new ThemeProgress(this);
			}
            if (m_ThemeButton != null)
            {
                m_ThemeButton.Dispose();
                m_ThemeButton = new ThemeButton(this);
            }
			
			SyncGroupColors();
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
            if (m_ThemeButton != null)
            {
                m_ThemeButton.Dispose();
                m_ThemeButton = null;
            }
		}
		private void SyncGroupColors()
		{
            //eExplorerBarStockStyle stock=eExplorerBarStockStyle.Blue;
            //eExplorerBarStockStyle special=eExplorerBarStockStyle.BlueSpecial;
            //eExplorerBarStockStyle thisSpecial=GetSpecialStyle(m_StockStyle);

            //if(SystemColors.Control.ToArgb()==Color.FromArgb(224,223,227).ToArgb() && SystemColors.Highlight.ToArgb()==Color.FromArgb(178,180,191).ToArgb())
            //{
            //    stock=eExplorerBarStockStyle.Silver;
            //    special=eExplorerBarStockStyle.SilverSpecial;
            //}
            //else if(SystemColors.Control.ToArgb()==Color.FromArgb(236,233,216).ToArgb() && SystemColors.Highlight.ToArgb()==Color.FromArgb(147,160,112).ToArgb())
            //{
            //    stock=eExplorerBarStockStyle.OliveGreen;
            //    special=eExplorerBarStockStyle.OliveGreenSpecial;
            //}

			foreach(BaseItem item in m_ItemContainer.SubItems)
			{
				ExplorerBarGroupItem group=item as ExplorerBarGroupItem;
				if(group==null)
					continue;
				if(group.StockStyle==eExplorerBarStockStyle.SystemColors)
				{
					group.StockStyle = eExplorerBarStockStyle.SystemColors;
				}
			}
		}
		private eExplorerBarStockStyle GetSpecialStyle(eExplorerBarStockStyle style)
		{
			eExplorerBarStockStyle special=eExplorerBarStockStyle.Custom;
			switch(style)
			{
				case eExplorerBarStockStyle.Blue:
                    special=eExplorerBarStockStyle.BlueSpecial;
					break;
				case eExplorerBarStockStyle.OliveGreen:
					special=eExplorerBarStockStyle.OliveGreenSpecial;
					break;
				case eExplorerBarStockStyle.Silver:
					special=eExplorerBarStockStyle.SilverSpecial;
					break;
				case eExplorerBarStockStyle.BlueSpecial:
				case eExplorerBarStockStyle.OliveGreenSpecial:
				case eExplorerBarStockStyle.SilverSpecial:
					special=style;
					break;
			}
			return special;
		}
		private bool IsStyleSpecial(eExplorerBarStockStyle style)
		{
			if(style==eExplorerBarStockStyle.BlueSpecial || style==eExplorerBarStockStyle.OliveGreenSpecial || style==eExplorerBarStockStyle.SilverSpecial)
				return true;
			return false;
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

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if(this.Visible)
				SyncGroupColors();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
            if (this.Width == 0 || this.Height == 0)
                return;
			this.RecalcSize();
		}

		protected virtual void RecalcSize()
		{
			if(m_SuspendLayout)
				return;
			if(!BarFunctions.IsHandleValid(this))
				return;
            if(this.RightToLeft == RightToLeft.Yes)
                m_ItemContainer.IsRightToLeft = (this.RightToLeft == RightToLeft.Yes);
			if(m_BorderStyle==eBorderType.None)
			{
				m_ItemContainer.LeftInternal=m_Margin;
				m_ItemContainer.TopInternal=m_Margin;
				m_ItemContainer.WidthInternal =this.Width-m_Margin*2;
				m_ItemContainer.HeightInternal=this.Height-m_Margin;
			}
			else
			{
				m_ItemContainer.LeftInternal=m_Margin;
				m_ItemContainer.TopInternal=m_Margin;
				m_ItemContainer.WidthInternal =this.Width-4-m_Margin*2;
				m_ItemContainer.HeightInternal=this.Height-m_Margin-2;
			}
			m_ItemContainer.RecalcSize();
			if(m_ItemContainer.HeightInternal>this.ClientRectangle.Height || m_ItemContainer.HeightInternal<=this.ClientRectangle.Height && m_ScrollBar!=null)
			{
				this.ContainerSizeChanged();
			}

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

		internal void ContainerSizeChanged()
		{
			if(m_ItemContainer.HeightInternal>this.ClientRectangle.Height)
			{
				CreateScrollBar();
				if(m_BorderStyle==eBorderType.None)
				{
					m_ItemContainer.LeftInternal=m_Margin;
					m_ItemContainer.TopInternal=m_Margin;
					m_ItemContainer.WidthInternal =this.Width-m_ScrollBar.Width-m_Margin*2;
					m_ItemContainer.HeightInternal=this.Height-m_Margin;
				}
				else
				{
					m_ItemContainer.LeftInternal=m_Margin;
					m_ItemContainer.TopInternal=m_Margin;
					m_ItemContainer.WidthInternal =this.Width-4-m_ScrollBar.Width-m_Margin*2;
					m_ItemContainer.HeightInternal=this.Height-m_Margin-2;
				}
				m_ItemContainer.RecalcSize();
				CreateScrollBar();
				m_ItemContainer.TopInternal=-m_ScrollBar.Value+m_Margin;
			}
			else if(m_ScrollBar!=null)
			{
				this.Controls.Remove(m_ScrollBar);
				m_ScrollBar.Dispose();
				m_ScrollBar=null;

				if(m_BorderStyle==eBorderType.None)
				{
					m_ItemContainer.LeftInternal=m_Margin;
					m_ItemContainer.TopInternal=m_Margin;
					m_ItemContainer.WidthInternal =this.Width-m_Margin*2;
					m_ItemContainer.HeightInternal=this.Height-m_Margin;
				}
				else
				{
					m_ItemContainer.LeftInternal=m_Margin;
					m_ItemContainer.TopInternal=m_Margin;
					m_ItemContainer.WidthInternal =this.Width-4-m_Margin*2;
					m_ItemContainer.HeightInternal=this.Height-m_Margin-2;
				}
				m_ItemContainer.RecalcSize();
			}
		}

		private void CreateScrollBar()
		{
			if(m_ScrollBar==null)
			{
				m_ScrollBar=new VScrollBar();
				m_ScrollBar.Width=SystemInformation.VerticalScrollBarWidth;
				m_ScrollBar.Dock=DockStyle.Right;
				m_ScrollBar.Visible=true;
				this.Controls.Add(m_ScrollBar);
				m_ScrollBar.Scroll+=new ScrollEventHandler(this.ScrollBarValueChanged);
				m_ScrollBar.Site=null; // Make it not in design mode.
			}
            m_ScrollBar.Maximum = m_ItemContainer.HeightInternal;
			m_ScrollBar.LargeChange=this.Height/2;
			m_ScrollBar.SmallChange=m_ScrollBar.LargeChange/5;
            int maximum=(m_ItemContainer.HeightInternal-this.Height)+m_ScrollBar.LargeChange+m_ScrollBar.SmallChange;
			if(m_ScrollBar.SmallChange>0 && Math.Ceiling((float)maximum/(float)m_ScrollBar.SmallChange)>maximum/m_ScrollBar.SmallChange)
				maximum=(int)Math.Ceiling((float)maximum/(float)m_ScrollBar.SmallChange)*m_ScrollBar.SmallChange;
			if(m_ScrollBar.Maximum!=maximum)
			{
				m_ScrollBar.Maximum=maximum;
				//m_ScrollBar.Value=0;
			}
		}

		private void ScrollBarValueChanged(object sender, ScrollEventArgs e)
		{
			UpdateScrollValue(e.NewValue);
		}

		private void UpdateScrollValue(int iScrollValue)
		{
			m_ItemContainer.TopInternal=-iScrollValue+m_Margin;
			this.Invalidate(this.ClientRectangle,false);
		}

		/// <summary>
		/// Applies any layout changes and repaint the control.
		/// </summary>
		public void RecalcLayout()
		{
			if(m_SuspendLayout)
				return;
			this.RecalcSize();
			this.Refresh();
		}

		/// <summary>
		/// Gets or sets whether anti-alias smoothing is used while painting.
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
					this.Refresh();
				}
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void SetDesignMode()
		{
			m_ItemContainer.SetDesignMode(true);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if(this.DesignMode)
				SetDesignMode();
			if(!m_FilterInstalled && !this.DesignMode)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled=true;
			}
			this.RecalcLayout();
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if(m_DelayedExpandedPanel!=null)
			{
				if(m_ItemContainer.SubItems.Contains(m_DelayedExpandedPanel))
					m_DelayedExpandedPanel.Expanded=true;
				m_DelayedExpandedPanel=null;
			}
			if(this.Images!=null || ((IOwner)this).ImagesLarge!=null || ((IOwner)this).ImagesMedium!=null)
			{
				foreach(ExplorerBarGroupItem panel in m_ItemContainer.SubItems)
				{
					foreach(BaseItem item in panel.SubItems)
					{
						if(item is ImageItem)
							((ImageItem)item).OnImageChanged();
					}
				}
			}
			this.RecalcLayout();
		}

		/// <summary>
		/// Returns the collection of Explorer Bar Groups.
		/// </summary>
		[System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ExplorerBarPanelsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),DesignerSerializationVisibility(DesignerSerializationVisibility.Content),System.ComponentModel.Category("Data"),System.ComponentModel.Description("Returns the collection of Explorer Bar Groups.")]
		public SubItemsCollection Groups
		{
			get
			{
				return m_ItemContainer.SubItems;
			}
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
				EventMouseEnter.Invoke(item,e);
		}
		void IOwnerItemEvents.InvokeMouseHover(BaseItem item,EventArgs e)
		{
			if(EventMouseHover!=null)
				EventMouseHover.Invoke(item,e);
		}
		void IOwnerItemEvents.InvokeMouseLeave(BaseItem item,EventArgs e)
		{
			if(EventMouseLeave!=null)
				EventMouseLeave.Invoke(item,e);
		}
		void IOwnerItemEvents.InvokeMouseDown(BaseItem item, System.Windows.Forms.MouseEventArgs e)
		{
			if (EventMouseDown!= null)
	 			EventMouseDown.Invoke(item, e);
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
				EventMouseUp.Invoke(item, e);

			if(m_ClickTimer!=null && m_ClickTimer.Enabled)
			{
				m_ClickTimer.Stop();
				m_ClickTimer.Enabled=false;
			}
		}
		void IOwnerItemEvents.InvokeMouseMove(BaseItem item, System.Windows.Forms.MouseEventArgs e)
		{
			if(EventMouseMove!=null)
				EventMouseMove.Invoke(item,e);
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
				EventGotFocus.Invoke(item,e);
		}
		void IOwnerItemEvents.InvokeLostFocus(BaseItem item,EventArgs e)
		{
			if(EventLostFocus!=null)
				EventLostFocus.Invoke(item,e);
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
//		void IOwnerItemEvents.InvokeItemDisplayedChanged(BaseItem item,EventArgs e)
//		{
////			if(ItemDisplayedChanged!=null)
////				ItemDisplayedChanged(item,e);
//		}

		private void ClickTimerTick(object sender, EventArgs e)
		{
			if(m_ClickRepeatItem!=null)
				m_ClickRepeatItem.RaiseClick();
			else
				m_ClickTimer.Stop();
		}

		/// <summary>
		/// Gets or sets the form ExplorerBar is attached to.
		/// </summary>
		Form IOwner.ParentForm
		{
			get
			{
				return base.FindForm();
			}
			set
			{
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
			BarFunctions.GetSubItemsByName(m_ItemContainer,ItemName,list);
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
			BarFunctions.GetSubItemsByNameAndType(m_ItemContainer,ItemName,list,itemType);
			return list;
		}

        /// <summary>
        /// Returns the collection of items with the specified name and type.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <returns></returns>
        public ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByNameAndType(m_ItemContainer, ItemName, list, itemType, useGlobalName);
            return list;
        }

		/// <summary>
		/// Returns the first item that matches specified name.
		/// </summary>
		/// <param name="ItemName">Item name to look for.</param>
		/// <returns></returns>
		public BaseItem GetItem(string ItemName)
		{
			BaseItem item=BarFunctions.GetSubItemByName(m_ItemContainer,ItemName);
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
			if(this.DesignMode && this.Groups.Contains(objFocusItem))
			{
				foreach(BaseItem panel in this.Groups)
					BaseItem.CollapseSubItems(panel);
			}
			m_FocusItem=objFocusItem;
			if(m_FocusItem!=null)
				m_FocusItem.OnGotFocus();
			
			if(!this.DesignMode)
				EnsureVisible(m_FocusItem);
		}

		BaseItem IOwner.GetFocusItem()
		{
			return m_FocusItem;
		}

		bool IOwner.AlwaysDisplayKeyAccelerators
		{
			get {return false;}
			set {}
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
			//bool bAdded=false;
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

		void IOwner.OnApplicationActivate(){}
		void IOwner.OnApplicationDeactivate()
		{
            ClosePopups();
		}
		void IOwner.OnParentPositionChanging(){}


		/// <summary>
		/// Ensures that item is displayed on the screen. Item needs to have it's Visible property set to true. This method will expand the group if needed or it will scroll control to display an item.
		/// </summary>
		/// <param name="item">Item to display.</param>
		public void EnsureVisible(BaseItem item)
		{
			if(item==null)
				return;
			if(item.ContainerControl!=this)
				return;
			if(item.Parent is ExplorerBarGroupItem)
			{
				if(item.Parent.Visible)
				{
					if(!item.Parent.Expanded)
                        item.Parent.Expanded=true;
				}
				else
					return;
			}

			if(m_ScrollBar==null || !m_ScrollBar.Visible)
				return;

			Rectangle r=this.ClientRectangle;
			Rectangle rItem=item.DisplayRectangle;
			if(item is ExplorerBarGroupItem)
			{
				rItem=((ExplorerBarGroupItem)item).PanelRect;
				rItem.Y=item.TopInternal;
			}
			
			if(!r.Contains(rItem))
			{
				int scroll=0;
				// Ensure that rItem is visible
				if(rItem.Top<0)
				{
					scroll=m_ScrollBar.Value+rItem.Top-m_Margin;
					if(scroll<0)
						scroll=0;
				}
				else
				{
					scroll=Math.Abs(m_ItemContainer.TopInternal)+rItem.Bottom-r.Height+m_Margin;
					
				}
				m_ScrollBar.Value=scroll;
				UpdateScrollValue(m_ScrollBar.Value);
			}
		}

		/// <summary>
		/// Gets or sets the margin in pixels between the explorer bar groups and the edge of the control.
		/// </summary>
		[Browsable(true),DefaultValue(4),Category("Layout"),Description("Indicates margin in pixels between the explorer bar groups and the edge of the control.")]
		public int ContentMargin
		{
			get {return m_Margin;}
			set 
			{
				m_Margin=value;
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether end-user can rearrange the items inside the panels.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifies whether end-user can rearrange the items inside the panels.")]
		public bool AllowUserCustomize
		{
			get{return m_AllowUserCustomize;}
			set{m_AllowUserCustomize=value;}
		}

		/// <summary>
		/// Gets or sets whether animation is enabled.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifies whether animation is enabled.")]
		public bool AnimationEnabled
		{
			get{return m_AnimationEnabled;}
			set{m_AnimationEnabled=value;}
		}

		/// <summary>
		/// Gets or sets maximum animation time in milliseconds.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(100),Category("Behavior"),Description("Indicates maximum animation time in milliseconds, default value is 100.")]
		public int AnimationTime
		{
			get {return m_AnimationTime;}
			set
			{
				if(m_AnimationTime>0)
					m_AnimationTime=value;
			}
		}

		/// <summary>
		/// Gets or sets whether native .NET Drag and Drop is used by side-bar to perform drag and drop operations. AllowDrop must be set to true to allow drop of the items on control.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifies whether native .NET Drag and Drop is used by side-bar to perform drag and drop operations.")]
		public bool UseNativeDragDrop
		{
			get{return m_UseNativeDragDrop;}
			set{m_UseNativeDragDrop=value;}
		}

		/// <summary>
		/// Gets or sets whether external ButtonItem object is accepted in drag and drop operation. UseNativeDragDrop must be set to true in order for this property to be effective.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
		public bool AllowExternalDrop
		{
			get{return m_AllowExternalDrop;}
			set{m_AllowExternalDrop=value;}
		}

		/// <summary>
		/// Applies the stock style to the object.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),DefaultValue(eExplorerBarStockStyle.Custom),Description("Applies the stock style to the object.")/*,DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)*/]
		public eExplorerBarStockStyle StockStyle
		{
			get {return m_StockStyle;}
			set
			{
				BarFunctions.SetExplorerBarStyle(this,m_StockStyle);
				foreach(BaseItem item in m_ItemContainer.SubItems)
				{
					if(item is ExplorerBarGroupItem && ((ExplorerBarGroupItem)item).StockStyle != eExplorerBarStockStyle.Custom)
					{
						((ExplorerBarGroupItem)item).StockStyle=value;
					}
                }
				m_StockStyle=value;
				this.Refresh();
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
        /// Specifies custom image to be used as button on ExplorerBarGroup item to expand the group. Default value is null
        /// which indicates that default button is used.
        /// </summary>
        [Browsable(true), Category("Group Expand Button"), DefaultValue(null), Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
        public Image GroupButtonExpandNormal
        {
            get { return m_GroupButtonExpandNormal; }
            set
            {
                m_GroupButtonExpandNormal = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Specifies custom image to be used as button on ExplorerBarGroup item to expand the group. This image is used when mouse is over the button. Default value is null
        /// which indicates that default button is used.
        /// </summary>
        [Browsable(true), Category("Group Expand Button"), DefaultValue(null), Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
        public Image GroupButtonExpandHot
        {
            get { return m_GroupButtonExpandHot; }
            set
            {
                m_GroupButtonExpandHot = value;
            }
        }

        /// <summary>
        /// Specifies custom image to be used as button on ExplorerBarGroup item to expand the group. This image is used when user presses left mouse button while cursor is positioned over the button. Default value is null
        /// which indicates that default button is used.
        /// </summary>
        [Browsable(true), Category("Group Expand Button"), DefaultValue(null), Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
        public Image GroupButtonExpandPressed
        {
            get { return m_GroupButtonExpandPressed; }
            set
            {
                m_GroupButtonExpandPressed = value;
            }
        }

        /// <summary>
        /// Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group. Default value is null
        /// which indicates that default button is used.
        /// </summary>
        [Browsable(true), Category("Group Expand Button"), DefaultValue(null), Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
        public Image GroupButtonCollapseNormal
        {
            get { return m_GroupButtonCollapseNormal; }
            set
            {
                m_GroupButtonCollapseNormal = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group. 
        /// This Image is used when mouse is over the button. Default value is null
        /// which indicates that default button is used.
        /// </summary>
        [Browsable(true), Category("Group Expand Button"), DefaultValue(null), Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
        public Image GroupButtonCollapseHot
        {
            get { return m_GroupButtonCollapseHot; }
            set {m_GroupButtonCollapseHot = value;}
        }

        /// <summary>
        /// Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group. 
        /// This Image is used when left mouse button is pressed while cursor is over the button. Default value is null
        /// which indicates that default button is used.
        /// </summary>
        [Browsable(true), Category("Group Expand Button"), DefaultValue(null), Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
        public Image GroupButtonCollapsePressed
        {
            get { return m_GroupButtonCollapsePressed; }
            set { m_GroupButtonCollapsePressed = value; }
        }

		/// <summary>
		/// ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),System.ComponentModel.Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
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

		//private bool m_Initializing=false;
		void ISupportInitialize.BeginInit()
		{
			//m_Initializing=true;
		}
		void ISupportInitialize.EndInit()
		{
			//m_Initializing=false;
			if(this.Images!=null)
			{
				foreach(BaseItem item in m_ItemContainer.SubItems)
					FireOnImageChanged(item as ImageItem);
			}
			this.RecalcLayout();
		}

		private void FireOnImageChanged(ImageItem item)
		{
			if(item==null)
				return;

			item.OnImageChanged();
			foreach(BaseItem child in item.SubItems)
				FireOnImageChanged(child as ImageItem);
		}

		/// <summary>
		/// ImageList for images displayed on the Group Item.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),System.ComponentModel.Description("ImageList for images displayed on the Group Item.")]
		public System.Windows.Forms.ImageList GroupImages
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

		System.Windows.Forms.ImageList IOwner.ImagesMedium
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
		//[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),System.ComponentModel.Description("ImageList for large-sized images used on Items.")]
		System.Windows.Forms.ImageList IOwner.ImagesLarge
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

		void IOwner.StartItemDrag(BaseItem item)
		{
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
		/// Returns the item located at specific client coordinates.
		/// </summary>
		/// <param name="clientX">X Coordinate</param>
		/// <param name="clientY">Y Coordinate</param>
		/// <returns></returns>
		public BaseItem HitTest(int clientX, int clientY)
		{
			BaseItem ret=null;
			foreach(BaseItem item in m_ItemContainer.SubItems)
			{
				if(item.DisplayRectangle.Contains(clientX,clientY))
				{
					ret=item;
					foreach(BaseItem child in item.SubItems)
					{
						if(child.DisplayRectangle.Contains(clientX,clientY))
						{
							ret=child;
							break;
						}
					}
				}
			}
			return ret;
		}
        
        /// <summary>
        /// Gets or sets the item default accesibility action will be performed on.
        /// </summary>
        BaseItem IAccessibilitySupport.DoDefaultActionItem
        {
            get { return m_DoDefaultActionItem; }
            set { m_DoDefaultActionItem = value; }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeFunctions.WM_USER + 107)
            {
                if (m_DoDefaultActionItem != null)
                {
                    m_DoDefaultActionItem.DoAccesibleDefaultAction();
                    m_DoDefaultActionItem = null;
                }

            }
            base.WndProc(ref m);
        }

		// Menu Support Code START
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
			
			if(!this.DesignMode)
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
		// Menu Support Code END

		bool IOwner.DesignMode
		{
			get {return this.DesignMode;}
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

		System.Windows.Forms.MdiClient IOwner.GetMdiClient(System.Windows.Forms.Form MdiForm)
		{
			return BarFunctions.GetMdiClient(MdiForm);
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

		/// <summary>
		/// Returns the reference to the container that containing the sub-items.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public ExplorerBarContainerItem ItemsContainer
		{
			get
			{
				return m_ItemContainer;
			}
		}

//		/// <summary>
//		/// Gets/Sets border style when Bar is docked.
//		/// </summary>
//		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(eBorderType.None),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates border style when Bar is docked.")]
//		public eBorderType BorderStyle
//		{
//			get
//			{
//				return m_BorderStyle;
//			}
//			set
//			{
//				if(m_BorderStyle==value)
//					return;
//				if(m_BorderStyle==eBorderType.None)
//				{
//					m_BorderStyle=value;
//					this.RecalcSize();
//				}
//				else
//					m_BorderStyle=value;
//				this.Refresh();
//			}
//		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get{return base.BackgroundImage;}
			set{base.BackgroundImage=value;}
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get{return base.BackColor;}
			set{base.BackColor=value;}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get{return base.ForeColor;}
			set{base.ForeColor=value;}
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
				this.Refresh();
			}
		}

		internal void SaveDefinition(System.Xml.XmlDocument xmlDoc)
		{
			System.Xml.XmlElement xmlBar=xmlDoc.CreateElement("ExplorerBar");
			xmlDoc.AppendChild(xmlBar);
			this.Serialize(xmlBar);
		}

		internal void Serialize(System.Xml.XmlElement xmlThisBar)
		{
            // Creates serialization context
            ItemSerializationContext context = new ItemSerializationContext();
            context.Serializer = this;
            context.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
            context.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;

			xmlThisBar.SetAttribute("name",this.Name);
			
			// Save Font information if needed
			if(this.Font!=null)
			{
				if(this.Font.Name!=System.Windows.Forms.SystemInformation.MenuFont.Name || this.Font.Size!=System.Windows.Forms.SystemInformation.MenuFont.Size || this.Font.Style!=System.Windows.Forms.SystemInformation.MenuFont.Style)
				{
					xmlThisBar.SetAttribute("fontname",this.Font.Name);
					xmlThisBar.SetAttribute("fontemsize",System.Xml.XmlConvert.ToString(this.Font.Size));
					xmlThisBar.SetAttribute("fontstyle",System.Xml.XmlConvert.ToString((int)this.Font.Style));
				}
			}

//			if(this.BackColor!=SystemColors.Control)
//				xmlThisBar.SetAttribute("backcolor",BarFunctions.ColorToString(this.BackColor));
//			if(this.ForeColor!=SystemColors.ControlText)
//				xmlThisBar.SetAttribute("forecolor",BarFunctions.ColorToString(this.ForeColor));

			//xmlThisBar.SetAttribute("x",System.Xml.XmlConvert.ToString(this.Location.X));
			//xmlThisBar.SetAttribute("y",System.Xml.XmlConvert.ToString(this.Location.Y));
			xmlThisBar.SetAttribute("width",System.Xml.XmlConvert.ToString(this.Width));
			xmlThisBar.SetAttribute("height",System.Xml.XmlConvert.ToString(this.Height));
			if(!m_AnimationEnabled)
				xmlThisBar.SetAttribute("animate",System.Xml.XmlConvert.ToString(m_AnimationEnabled));
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

			System.Xml.XmlElement xmlStyle=xmlThisBar.OwnerDocument.CreateElement("backstyle");
			xmlThisBar.AppendChild(xmlStyle);
            SerializeElementStyle(m_BackStyle,xmlStyle);
		}

        private void SerializeElementStyle(ElementStyle style, System.Xml.XmlElement xmlElement)
        {
            ElementSerializer.Serialize(style, xmlElement);
        }

        private void DeserializeElementStyle(ElementStyle style, System.Xml.XmlElement xmlElement)
        {
            ElementSerializer.Deserialize(style, xmlElement);
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

		/// <summary>
		/// Indicates whether shortucts handled by items are dispatched to the next handler or control.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
		public bool DispatchShortcuts
		{
			get {return m_DispatchShortcuts;}
			set {m_DispatchShortcuts=value;}
		}

		private bool IsParentFormActive
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
		bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
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
			if(!this.DesignMode)
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

		internal void Deserialize(System.Xml.XmlElement xmlThisBar)
		{
            // Creates serialization context
            ItemSerializationContext context = new ItemSerializationContext();
            context.Serializer = this;
            context.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
            context.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;

			m_ShortcutTable.Clear();

			this.Name=xmlThisBar.GetAttribute("name");
			//m_ItemContainer.Style=(eDotNetBarStyle)System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("style"));
			
//			if(xmlThisBar.HasAttribute("backcolor"))
//				this.BackColor=BarFunctions.ColorFromString(xmlThisBar.GetAttribute("backcolor"));
//			if(xmlThisBar.HasAttribute("forecolor"))
//				this.ForeColor=BarFunctions.ColorFromString(xmlThisBar.GetAttribute("forecolor"));

			//this.Location=new Point(System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("x")),System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("y")));
			this.Size=new Size(System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("width")),System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("height")));
			
			if(xmlThisBar.HasAttribute("animate"))
				m_AnimationEnabled=System.Xml.XmlConvert.ToBoolean(xmlThisBar.GetAttribute("animate"));
			else
				m_AnimationEnabled=true;

			// Load font information if it exists
			if(xmlThisBar.HasAttribute("fontname"))
			{
				string FontName=xmlThisBar.GetAttribute("fontname");
				float FontSize=System.Xml.XmlConvert.ToSingle(xmlThisBar.GetAttribute("fontemsize"));
				System.Drawing.FontStyle FontStyle=(System.Drawing.FontStyle)System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("fontstyle"));
				try
				{
					this.Font=new Font(FontName,FontSize,FontStyle);
				}
				catch(Exception)
				{
					this.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
				}
			}

			foreach(System.Xml.XmlElement xmlElem in xmlThisBar.ChildNodes)
			{
				switch(xmlElem.Name)
				{
					case "items":
					{
						foreach(System.Xml.XmlElement xmlItem in xmlElem.ChildNodes)
						{
							BaseItem objItem=BarFunctions.CreateItemFromXml(xmlItem);
							m_ItemContainer.SubItems.Add(objItem);
                            context.ItemXmlElement = xmlItem;
							objItem.Deserialize(context);
						}
						break;
					}
					case "backstyle":
                        DeserializeElementStyle(m_BackStyle, xmlElem);
						m_VisualPropertyChanged=true;
						break;
				}
			}
		}

		/// <summary>
		/// Loads the Side bar definition from file.
		/// </summary>
		/// <param name="FileName">Definition file name.</param>
		public void LoadDefinition(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			xmlDoc.Load(FileName);
			this.LoadDefinition(xmlDoc);
			this.Refresh();
		}

		internal void LoadDefinition(System.Xml.XmlDocument xmlDoc)
		{
			if(xmlDoc.FirstChild.Name!="ExplorerBar")
				throw(new System.InvalidOperationException("XML Format not recognized"));

			m_ItemContainer.SubItems.Clear();
			this.Deserialize(xmlDoc.FirstChild as System.Xml.XmlElement);

			IOwner owner=this as IOwner;
			if(owner!=null)
				owner.AddShortcutsFromItem(m_ItemContainer);

			((IOwner)this).InvokeDefinitionLoaded(this,new EventArgs());
			this.RecalcSize();
		}

		/// <summary>
		/// Saves the Side bar definition to file.
		/// </summary>
		/// <param name="FileName">Definition file name.</param>
		public void SaveDefinition(string FileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			
			this.SaveDefinition(xmlDoc);

			xmlDoc.Save(FileName);
		}

		void IOwner.InvokeDefinitionLoaded(object sender,EventArgs e)
		{
			if(DefinitionLoaded!=null)
				DefinitionLoaded(sender,e);
		}

		#region IBarDesignerServices
		IBarItemDesigner IBarDesignerServices.Designer
		{
			get {return m_BarDesigner;}
			set {m_BarDesigner=value;}
		}
		#endregion

		public class ExplorerBarAccessibleObject : System.Windows.Forms.AccessibleObject
		{
			ExplorerBar m_Owner = null;
			public ExplorerBarAccessibleObject(ExplorerBar owner)
			{
				m_Owner = owner;
			}

			internal void GenerateEvent(BaseItem sender, System.Windows.Forms.AccessibleEvents e)
			{
				int	iChild = m_Owner.Groups.IndexOf(sender);
				if(iChild>=0)
					m_Owner.AccessibilityNotifyClients(e,iChild);
			}

			public override string Name 
			{
				get { return m_Owner.AccessibleName; }
				set { m_Owner.AccessibleName = value; }
			}

			public override string Description
			{
				get { return m_Owner.AccessibleDescription; }
			}

			public override AccessibleRole Role
			{
				get { return m_Owner.AccessibleRole; }
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
					if(m_Owner.Parent!=null)
						return m_Owner.Parent.AccessibilityObject;
					return null;
				}
			}

			public override Rectangle Bounds 
			{
				get
				{
					if(m_Owner.Parent!=null)
						return this.m_Owner.Parent.RectangleToScreen(m_Owner.Bounds);
					return m_Owner.DisplayRectangle;
				}
			}

			public override int GetChildCount()
			{
				return m_Owner.Groups.Count;
			}

			public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
			{
				return m_Owner.Groups[iIndex].AccessibleObject;
			}

			public override AccessibleStates State
			{
				get
				{
					AccessibleStates state=AccessibleStates.Default;
					if(m_Owner.Focused)
					{
						state=AccessibleStates.Focused;
					}
					return state;
				}
			}
		}
	}
}
