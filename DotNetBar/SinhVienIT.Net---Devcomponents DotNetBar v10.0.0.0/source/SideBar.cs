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
	/// Represents the Outlook like Side-bar Control.
	/// </summary>
    [ToolboxItem(true), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.SideBarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class SideBar : System.Windows.Forms.Control,
		IOwnerItemEvents, IOwner, IThemeCache, IMessageHandlerClient, IOwnerMenuSupport, IBarDesignerServices,
        ICustomSerialization
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
		private SideBarContainerItem m_ItemContainer=null;

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
		private eBorderType m_BorderStyle=eBorderType.Sunken;

		private SideBarPanelItem m_DelayedExpandedPanel=null;

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

		private bool m_DispatchShortcuts=false;
		private bool m_FilterInstalled=false;

		private ColorScheme m_ColorScheme=null;

		private const string INFO_EMPTYSIDEBAR="Right-click and choose Add New Panel or use Panels collection to create new panels. Right-click to Choose Color Scheme.";

		private eSideBarAppearance m_Appearance=eSideBarAppearance.Traditional;
		private eSideBarColorScheme m_PredefinedColorScheme=eSideBarColorScheme.Blue;
		private bool m_UsingSystemColors=false;

		private bool m_MenuEventSupport=false;
		private IBarItemDesigner m_BarDesigner=null;
		#endregion

		/// <summary>
		/// Creates new instance of side bar control.
		/// </summary>
		public SideBar()
		{
			m_ItemContainer=new SideBarContainerItem();
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

			//this.TabStop=false;
			//this.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;

			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}

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
			this.AccessibleRole=AccessibleRole.ToolBar;

			m_ColorScheme=new ColorScheme();
            StyleManager.Register(this);
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
                StyleManager.Unregister(this);
			}
			base.Dispose( disposing );
		}

        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            
        }

		#region IBarDesignerServices
		IBarItemDesigner IBarDesignerServices.Designer
		{
			get {return m_BarDesigner;}
			set {m_BarDesigner=value;}
		}
		#endregion

#if FRAMEWORK20
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
            if (m_ItemContainer != null)
                m_ItemContainer.UpdateBindings();
        }
#endif

		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new SideBarAccessibleObject(this);
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
						if(item is SideBarPanelItem)
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
			foreach(SideBarPanelItem panel in m_ItemContainer.SubItems)
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
			m_ItemContainer.InternalMouseUp(new MouseEventArgs(MouseButtons.Left,0,x,y,0));
			if(dragItem!=null)
				dragItem._IgnoreClick=false;

			m_DragItem=null;
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

		protected override void OnPaint(PaintEventArgs e)
		{
			if(m_ItemContainer==null || this.IsDisposed)
				return;
			
			Graphics g=e.Graphics;
			g.PageUnit=GraphicsUnit.Pixel;

			Rectangle r=this.ClientRectangle;
            if (!BarFunctions.IsOffice2007Style(this.Style))
			    BarFunctions.DrawBorder(g,m_BorderStyle,r,SystemColors.ControlText);
            ColorScheme cs = m_ColorScheme;
            if (!cs.SchemeChanged && BarFunctions.IsOffice2007Style(this.Style) && GlobalManager.Renderer is Office2007Renderer)
            {
                cs = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
            }
            ItemPaintArgs pa = new ItemPaintArgs(this as IOwner, this, e.Graphics, cs);
            pa.Renderer = GetRenderer();

            if (BarFunctions.IsOffice2007Style(this.Style))
            {
                BaseRenderer renderer = pa.Renderer;
                SideBarRendererEventArgs renderArgs = new SideBarRendererEventArgs(this, g);
                renderArgs.ItemPaintArgs = pa;
                renderer.DrawSideBar(renderArgs);
            }
			else if(this.BackColor==SystemColors.Control && this.IsThemed)
			{
				if(m_BorderStyle==eBorderType.SingleLine)
					r.Inflate(-1,-1);
				else if(m_BorderStyle!=eBorderType.None)
					r.Inflate(-2,-2);

				ThemeRebar theme=((IThemeCache)this).ThemeRebar;
				theme.DrawBackground(e.Graphics,ThemeRebarParts.Background,ThemeRebarStates.Normal,r);
			}
			else
			{
				if(m_BorderStyle==eBorderType.None)
				{
					using(SolidBrush brush=new SolidBrush(this.BackColor))
						g.FillRectangle(brush,this.DisplayRectangle);
					//g.Clear(this.BackColor);
				}
				else if(m_BorderStyle==eBorderType.SingleLine)
				{
					r.Inflate(-1,-1);
				}
				else
				{
					r.Inflate(-2,-2);
				}
				g.FillRectangle(new SolidBrush(this.BackColor),r);
			}

			
			m_ItemContainer.Paint(pa);
			if(m_ItemContainer.SubItems.Count==0 && this.DesignMode)
			{
				string info=INFO_EMPTYSIDEBAR;
				Rectangle rText=this.ClientRectangle;
				r.Inflate(-2,-2);
                eTextFormat format = eTextFormat.Default | eTextFormat.HorizontalCenter | 
                    eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				TextDrawing.DrawString(g,info,this.Font,SystemColors.ControlDark,rText,format);
			}
		}

		/// <summary>
		/// Gets or sets visual appearance for the control.
		/// </summary>
		[Browsable(true),DefaultValue(eSideBarAppearance.Traditional),Category("Appearance"),Description("Indicates visual appearance for the control.")]
		public eSideBarAppearance Appearance
		{
			get {return m_Appearance;}
			set
			{
				if(m_Appearance!=value)
				{
					m_Appearance=value;
					m_ItemContainer.Appearance=value;
					if(m_Appearance==eSideBarAppearance.Flat && this.Panels.Count>0 && this.Panels[0] is SideBarPanelItem)
					{
						if(!((SideBarPanelItem)this.Panels[0]).BackgroundStyle.Custom)
						{
							this.PredefinedColorScheme=eSideBarColorScheme.SystemColors;
							if(this.DesignMode)
                                ApplyPredefinedColorScheme(eSideBarColorScheme.SystemColors);	
						}
					}
					this.Refresh();
				}
			}
		}

		/// <summary>
		///     Gets or sets whether flat side bar is using system colors.
		/// </summary>
		/// <remarks>
		///     This property is used internally by side bar to determine whether to reset color scheme based on system colors.
		///     If you want side bar to use system colors you need to set PredefinedColorScheme property.
		/// </remarks>
		[Browsable(false),DefaultValue(false)]
		public bool UsingSystemColors
		{
			get {return m_UsingSystemColors;}
			set
			{
				m_UsingSystemColors=value;
			}
		}

		[DesignOnly(true),Browsable(false)]
		public eSideBarColorScheme PredefinedColorScheme
		{
			get {return m_PredefinedColorScheme;}
			set
			{
				m_PredefinedColorScheme=value;
				if(m_PredefinedColorScheme==eSideBarColorScheme.SystemColors)
					m_UsingSystemColors=true;
				else
					m_UsingSystemColors=false;
				if(!this.DesignMode)
					ApplyPredefinedColorScheme(m_PredefinedColorScheme);
			}
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
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void ApplyPredefinedColorScheme(eSideBarColorScheme scheme)
		{
			if(m_ItemContainer==null)
				return;
			SideBar.ApplyColorScheme(this.ColorScheme,scheme);
			foreach(BaseItem item in m_ItemContainer.SubItems)
			{
				if(item is SideBarPanelItem)
					SideBar.ApplyColorScheme((SideBarPanelItem)item,scheme);
			}
			this.Refresh();
		}

		/// <summary>
		/// Gets/Sets the visual style of the SideBar.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies the visual style of the side bar."),DefaultValue(eDotNetBarStyle.OfficeXP)]
		public eDotNetBarStyle Style
		{
			get
			{
				return m_ItemContainer.Style;
			}
			set
			{
				if(m_ItemContainer.Style==value)
					return;
				m_ColorScheme.Style=value;
				m_ItemContainer.Style=value;
				this.Invalidate();
				this.RecalcLayout();
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetTraditional()
		{
			m_Appearance=eSideBarAppearance.Traditional;
			m_ColorScheme.Style=m_ItemContainer.Style;
			m_ItemContainer.Style=m_ItemContainer.Style;
			
			foreach(SideBarPanelItem item in this.Panels)
			{
				item.ResetHeaderHotStyle();
				item.ResetHeaderMouseDownStyle();
				item.ResetHeaderSideHotStyle();
				item.ResetHeaderSideMouseDownStyle();
				item.ResetHeaderSideStyle();
				item.ResetHeaderStyle();
				item.ResetBackgroundStyle();
			}

			this.Invalidate();
			this.RecalcLayout();
		}

		protected bool IsThemed
		{
			get
			{
                if (m_ThemeAware && (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.OfficeXP || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle)) && BarFunctions.ThemedOS && Themes.ThemesActive)
					return true;
				return false;
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
				m_ItemContainer.ThemeAware=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);

			Application.DoEvents();

			m_ColorScheme.Refresh(null,true);

			if(m_ItemContainer!=null)
			{
				foreach(SideBarPanelItem item in m_ItemContainer.SubItems)
					item.RefreshItemStyleSystemColors();
			}

			if(m_UsingSystemColors && this.Appearance!=eSideBarAppearance.Traditional)
			{
				this.PredefinedColorScheme=eSideBarColorScheme.SystemColors;
				this.Invalidate(true);
			}
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_THEMECHANGED)
			{
				this.RefreshThemes();
			}
			// This code was added to support windows animation that we used for auto-hide windows.
//			else if(m.Msg==NativeFunctions.WM_PRINTCLIENT && m.WParam!=IntPtr.Zero)
//			{
//				Graphics g=null;
//				try
//				{
//					g=Graphics.FromHdc(m.WParam);
//					this.OnPaint(new PaintEventArgs(g,this.ClientRectangle));
//				}
//				finally
//				{
//					if(g!=null)
//						g.Dispose();
//				}
//			}
			base.WndProc(ref m);
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

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
            if (this.Width == 0 || this.Height == 0)
                return;
			this.RecalcSize();
		}

		protected virtual void RecalcSize()
		{
			if(!BarFunctions.IsHandleValid(this))
				return;
			if(m_BorderStyle==eBorderType.None)
			{
                if (BarFunctions.IsOffice2007Style(this.Style))
                {
                    m_ItemContainer.WidthInternal = this.Width - 2;
                    m_ItemContainer.HeightInternal = this.Height - 2;
                    m_ItemContainer.LeftInternal = 1;
                    m_ItemContainer.TopInternal = 1;
                }
				else if(m_Appearance==eSideBarAppearance.Traditional)
				{
					m_ItemContainer.WidthInternal =this.Width-2;
					m_ItemContainer.HeightInternal=this.Height-2;
				}
				else
				{
					m_ItemContainer.WidthInternal =this.Width;
					m_ItemContainer.HeightInternal=this.Height-2;
				}
			}
			else
			{
				m_ItemContainer.LeftInternal=2;
				m_ItemContainer.TopInternal=2;
				m_ItemContainer.WidthInternal =this.Width-4;
				m_ItemContainer.HeightInternal=this.Height-4;
			}
			m_ItemContainer.RecalcSize();
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

		public void RecalcLayout()
		{
			this.RecalcSize();
			this.Refresh();
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void SetDesignMode()
		{
			m_ItemContainer.SetDesignMode(true);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			if(this.DesignMode)
				SetDesignMode();
			base.OnHandleCreated(e);

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
			if(this.Parent!=null && (this.Images!=null || this.ImagesLarge!=null || this.ImagesMedium!=null))
			{
				foreach(SideBarPanelItem panel in m_ItemContainer.SubItems)
				{
					foreach(BaseItem item in panel.SubItems)
					{
						if(item is ImageItem)
							((ImageItem)item).OnImageChanged();
					}
				}
			}
		}

		/// <summary>
		/// Returns the collection of side-bar Panels.
		/// </summary>
        [System.ComponentModel.Editor("DevComponents.DotNetBar.Design.SideBarPanelsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Returns the collection of side-bar Panels.")]
		public SubItemsCollection Panels
		{
			get
			{
				return m_ItemContainer.SubItems;
			}
		}

		/// <summary>
		/// Gets or sets the expanded panel. Only one panel can be expanded at a time.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Data"),System.ComponentModel.Description("Gets or sets the expanded panel. Only one panel can be expanded at a time.")]
		public SideBarPanelItem ExpandedPanel
		{
			get
			{
				return m_ItemContainer.ExpandedItem() as SideBarPanelItem;
			}
			set
			{
				if(!m_ItemContainer.SubItems.Contains(value))
				{
					//if(this.IsHandleCreated && this.Parent!=null)
						//throw(new ArgumentException("Item is not member of Panels collection"));
					//else
					m_DelayedExpandedPanel=value;
				}
				else
					value.Expanded=true;
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
		/// Gets or sets the form SideBar is attached to.
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
			if(objFocusItem==null)
				objFocusItem=null;
			if(this.DesignMode && this.Panels.Contains(objFocusItem))
			{
				foreach(BaseItem panel in this.Panels)
					BaseItem.CollapseSubItems(panel);
			}
			m_FocusItem=objFocusItem;
			if(m_FocusItem!=null)
				m_FocusItem.OnGotFocus();
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
			get {return this.DesignMode;}
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
		/// Gets or sets whether end-user can rearrange the items inside the panels.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifies whether end-user can rearrange the items inside the panels.")]
		public bool AllowUserCustomize
		{
			get{return m_AllowUserCustomize;}
			set{m_AllowUserCustomize=value;}
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

		bool IOwner.AlwaysDisplayKeyAccelerators
		{
			get {return false;}
			set {}
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

		void IOwner.OnApplicationActivate(){}
		void IOwner.OnApplicationDeactivate()
		{
			ClosePopups();
		}
		void IOwner.OnParentPositionChanging(){}

		/// <summary>
		/// Returns the reference to the container that containing the sub-items.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public SideBarContainerItem ItemsContainer
		{
			get
			{
				return m_ItemContainer;
			}
		}

		/// <summary>
		/// Gets/Sets control border style.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(eBorderType.Sunken),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates border style when Bar is docked.")]
		public eBorderType BorderStyle
		{
			get
			{
				return m_BorderStyle;
			}
			set
			{
				if(m_BorderStyle==value)
					return;
				if(m_BorderStyle==eBorderType.None)
				{
					m_BorderStyle=value;
					this.RecalcSize();
				}
				else
					m_BorderStyle=value;
				this.Refresh();
			}
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get{return base.BackgroundImage;}
			set{base.BackgroundImage=value;}
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
			}
		}

		internal void SaveDefinition(System.Xml.XmlDocument xmlDoc)
		{
			System.Xml.XmlElement xmlBar=xmlDoc.CreateElement("sidebar");
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
			xmlThisBar.SetAttribute("style",System.Xml.XmlConvert.ToString((int)m_ItemContainer.Style));
			xmlThisBar.SetAttribute("app",System.Xml.XmlConvert.ToString((int)m_Appearance));
			xmlThisBar.SetAttribute("psc",System.Xml.XmlConvert.ToString((int)m_PredefinedColorScheme));
			
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

			if(this.BackColor!=SystemColors.Control)
				xmlThisBar.SetAttribute("backcolor",BarFunctions.ColorToString(this.BackColor));
			if(this.ForeColor!=SystemColors.ControlText)
				xmlThisBar.SetAttribute("forecolor",BarFunctions.ColorToString(this.ForeColor));

			//xmlThisBar.SetAttribute("x",System.Xml.XmlConvert.ToString(this.Location.X));
			//xmlThisBar.SetAttribute("y",System.Xml.XmlConvert.ToString(this.Location.Y));
			xmlThisBar.SetAttribute("width",System.Xml.XmlConvert.ToString(this.Width));
			xmlThisBar.SetAttribute("height",System.Xml.XmlConvert.ToString(this.Height));
			
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

			if(m_ColorScheme.SchemeChanged)
			{
				System.Xml.XmlElement xmlScheme=xmlThisBar.OwnerDocument.CreateElement("colorscheme");
				m_ColorScheme.Serialize(xmlScheme);
				xmlThisBar.AppendChild(xmlScheme);
			}
		}

		internal void Deserialize(System.Xml.XmlElement xmlThisBar)
		{
            // Creates serialization context
            ItemSerializationContext context = new ItemSerializationContext();
            context.Serializer = this;
            context.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
            context.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;

			this.Name=xmlThisBar.GetAttribute("name");
			m_ItemContainer.Style=(eDotNetBarStyle)System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("style"));
			
			if(xmlThisBar.HasAttribute("backcolor"))
				this.BackColor=BarFunctions.ColorFromString(xmlThisBar.GetAttribute("backcolor"));
			if(xmlThisBar.HasAttribute("forecolor"))
				this.ForeColor=BarFunctions.ColorFromString(xmlThisBar.GetAttribute("forecolor"));

			//this.Location=new Point(System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("x")),System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("y")));
			this.Size=new Size(System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("width")),System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("height")));

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
				if(xmlElem.Name=="items")
				{
					foreach(System.Xml.XmlElement xmlItem in xmlElem.ChildNodes)
					{
						BaseItem objItem=BarFunctions.CreateItemFromXml(xmlItem);
						m_ItemContainer.SubItems.Add(objItem);
                        context.ItemXmlElement = xmlItem;
						objItem.Deserialize(context);
					}
				}
				else if(xmlElem.Name=="colorscheme")
				{
					m_ColorScheme.Deserialize(xmlElem);
				}
			}


			if(xmlThisBar.HasAttribute("app"))
				this.Appearance=(eSideBarAppearance)System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("app"));
			xmlThisBar.SetAttribute("app",System.Xml.XmlConvert.ToString((int)m_Appearance));
			if(xmlThisBar.HasAttribute("psc"))
			{
				if(this.Appearance!=eSideBarAppearance.Traditional)
					this.PredefinedColorScheme=(eSideBarColorScheme)System.Xml.XmlConvert.ToInt32(xmlThisBar.GetAttribute("psc"));
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
		}

		internal void LoadDefinition(System.Xml.XmlDocument xmlDoc)
		{
			if(xmlDoc.FirstChild.Name!="sidebar")
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
        // IMessageHandlerClient Implementation
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
		//private bool m_IgnoreSysKeyUp=false, m_EatSysKeyUp=false, m_IgnoreF10Key=false;
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
			if(m_RegisteredPopups.Count==0 || this.DesignMode)
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
//			if(!this.IsParentFormActive)
//				return false;
//
//			SideBar bar=null;
//			if(this.MenuBar)
//				bar=this;
//			if(bar!=null && bar.ItemsContainer!=null && !bar.ItemsContainer.DesignMode)
//			{
//				GenericItemContainer cont=bar.ItemsContainer;
//				if(cont==null)
//					return false;
//				if(wParam.ToInt32()==18 || (wParam.ToInt32()==121 && !m_IgnoreF10Key && (System.Windows.Forms.Control.ModifierKeys==System.Windows.Forms.Keys.None || System.Windows.Forms.Control.ModifierKeys==System.Windows.Forms.Keys.Alt)))
//				{
//					if(cont.ExpandedItem()!=null && bar.Focused)
//					{
//						bar.ReleaseFocus();
//						m_IgnoreSysKeyUp=true;
//						return true;
//					}
//				}
//				else
//				{
//					// Check Shortcuts
//					if(System.Windows.Forms.Control.ModifierKeys!=Keys.None || wParam.ToInt32()>=(int)eShortcut.F1 && wParam.ToInt32()<=(int)eShortcut.F12)
//					{
//						int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
//						if(ProcessShortcut((eShortcut)i))
//							return true;
//					}
//					m_IgnoreSysKeyUp=true;
//					if(wParam.ToInt32()>=27 && wParam.ToInt32()<=111) // VK_ESC - VK_DIVIDE range
//					{
//						int key=(int)NativeFunctions.MapVirtualKey((uint)wParam,2);
//						if(key==0)
//							key=wParam.ToInt32();
//						if(key>0 && cont.SysKeyDown(key))
//							return true;
//					}
//				}
//			}
//			
//			if(System.Windows.Forms.Control.ModifierKeys==System.Windows.Forms.Keys.Alt && wParam.ToInt32()>0x1B)
//			{
//				m_IgnoreSysKeyUp=true;
//				int key=(int)NativeFunctions.MapVirtualKey((uint)wParam,2);
//				if(key!=0)
//				{
//					if(!this.DesignMode)
//					{
//						if(m_ItemContainer.SysKeyDown(key))
//							return true;
//					}
//				}
//			}
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
//			if(this.FindForm()==null || !this.FindForm().Focused)
//				return false;
//
//			if(wParam.ToInt32()==18 || wParam.ToInt32()==121)
//			{
//				if(m_IgnoreSysKeyUp)
//				{
//					m_IgnoreSysKeyUp=false;
//					return false;
//				}
//				if(m_EatSysKeyUp)
//				{
//					m_EatSysKeyUp=false;
//					return true;
//				}
//				if(wParam.ToInt32()==18 || wParam.ToInt32()==121 && !m_IgnoreF10Key)
//				{
//					DevComponents.DotNetBar.Bar bar=null;
//					if(this.MenuBar)
//						bar=this;
//					if(bar!=null && !bar.ItemsContainer.DesignMode)
//					{
//						if(bar.Focused)
//							bar.ReleaseFocus();
//						else
//							bar.SetSystemFocus();
//						return true;
//					}
//				}
//			}
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

		// IOwnerMenuSupport
		private ArrayList m_RegisteredPopups=new ArrayList();
		bool IOwnerMenuSupport.PersonalizedAllVisible {get{return false;}set{}}
		bool IOwnerMenuSupport.ShowFullMenusOnHover {get{return true;}set{}}
		bool IOwnerMenuSupport.AlwaysShowFullMenus {get{return false;}set{}}
        private Hook m_Hook = null;

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

		public class SideBarAccessibleObject : System.Windows.Forms.Control.ControlAccessibleObject
		{
			SideBar m_Owner = null;
			public SideBarAccessibleObject(SideBar owner):base(owner)
			{
				m_Owner = owner;
			}

			internal void GenerateEvent(BaseItem sender, System.Windows.Forms.AccessibleEvents e)
			{
				int	iChild = m_Owner.Panels.IndexOf(sender);
				if(iChild>=0)
					m_Owner.AccessibilityNotifyClients(e,iChild);
			}

            //public override string Name 
            //{
            //    get { return m_Owner.AccessibleName; }
            //    set { m_Owner.AccessibleName = value; }
            //}

            //public override string Description
            //{
            //    get { return m_Owner.AccessibleDescription; }
            //}

			public override AccessibleRole Role
			{
				get { return m_Owner.AccessibleRole; }
			}

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
				return m_Owner.Panels.Count;
			}

			public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
			{
				return m_Owner.Panels[iIndex].AccessibleObject;
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

		public static void ApplyColorScheme(SideBarPanelItem item, eSideBarColorScheme scheme)
		{
			SideBarColors colors=SideBar.GetColorScheme(scheme);
			colors.Apply(item);
		}

		public static void ApplyColorScheme(ColorScheme cs, eSideBarColorScheme scheme)
		{
			SideBarColors colors=SideBar.GetColorScheme(scheme);
			colors.Apply(cs);
		}

		private static SideBarColors GetColorScheme(eSideBarColorScheme scheme)
		{
			SideBarColors colors=null;
			switch(scheme)
			{
				case eSideBarColorScheme.Brick:
				case eSideBarColorScheme.Marine:
				case eSideBarColorScheme.Plum:
				case eSideBarColorScheme.Pumpkin:
				case eSideBarColorScheme.Fire:
				case eSideBarColorScheme.Rose:
				case eSideBarColorScheme.Slate:
				case eSideBarColorScheme.Spruce:
				case eSideBarColorScheme.Storm:
				case eSideBarColorScheme.Sunset:
				case eSideBarColorScheme.Wheat:
				{
					colors=GetFlatColorScheme(scheme);
					break;
				}
				case eSideBarColorScheme.Blue:
				case eSideBarColorScheme.Silver:
				case eSideBarColorScheme.Green:
				case eSideBarColorScheme.Orange:
				case eSideBarColorScheme.Red:
				case eSideBarColorScheme.LightBlue:
				case eSideBarColorScheme.Money:
				{
					colors=GetGradientColorScheme(scheme);
					break;
				}
				case eSideBarColorScheme.SystemColors:
				{
					colors=GetSystemColorScheme();
					break;
				}
			}

			return colors;
		}
		private static SideBarColors GetGradientColorScheme(eSideBarColorScheme scheme)
		{
			Color border=Color.Empty;
			Color back=Color.Empty;
			Color back2=Color.Empty;
			Color headerBack=Color.Empty;
			Color headerBack2=Color.Empty;
			Color headerHotBack=Color.Empty;
			Color headerText=Color.Empty;
			Color headerHotText=Color.Empty;
			Color sideBack=Color.Empty;
			Color sideBack2=Color.Empty;
			Color itemText=Color.Empty;
			Color itemHotBack=Color.Empty;
			Color itemHotBack2=Color.Empty;
			Color itemHotBorder=Color.Empty;
			Color itemHotText=Color.Empty;
			Color itemPressedBack=Color.Empty;
			Color itemPressedBack2=Color.Empty;
			Color itemCheckedBack=Color.Empty;
			Color itemCheckedBack2=Color.Empty;
			Color backText=Color.Empty;

			switch(scheme)
			{
				case eSideBarColorScheme.Blue:
				{
					border=Color.FromArgb(59,97,156);
					back=Color.FromArgb(232,232,232);
					back2=Color.White;
					sideBack=Color.FromArgb(200,220,248);
					sideBack2=Color.FromArgb(94,137,207);
					headerBack=Color.FromArgb(221,236,254);
					headerBack2=Color.FromArgb(133,171,228);
					headerText=Color.FromArgb(0,51,102);
					itemText=Color.Black;
					itemHotBack=Color.FromArgb(255,244,204);
					itemHotBack2=Color.FromArgb(255,209,147);
					itemHotBorder=Color.FromArgb(0,0,128);
					itemPressedBack=Color.FromArgb(254,142,75);
					itemPressedBack2=Color.FromArgb(255,207,139);
					itemCheckedBack=Color.FromArgb(255,213,140);
					itemCheckedBack2=Color.FromArgb(255,173,85);
					break;
				}
				case eSideBarColorScheme.Silver:
				{
					border=Color.FromArgb(87,86,113);
					back=Color.FromArgb(232,232,232);
					back2=Color.White;
					sideBack=Color.FromArgb(225,226,236);
					sideBack2=Color.FromArgb(126,125,157);
					headerBack=Color.FromArgb(243,244,250);
					headerBack2=Color.FromArgb(155,153,183);
					headerText=Color.FromArgb(87,86,113);
					itemText=Color.Black;
					itemHotBack=Color.FromArgb(255,244,204);
					itemHotBack2=Color.FromArgb(255,209,147);
					itemHotBorder=Color.FromArgb(87,86,113);
					itemPressedBack=Color.FromArgb(254,142,75);
					itemPressedBack2=Color.FromArgb(255,207,139);
					itemCheckedBack=Color.FromArgb(255,213,140);
					itemCheckedBack2=Color.FromArgb(255,173,85);
					break;
				}
				case eSideBarColorScheme.Green:
				{
					border=Color.FromArgb(96,128,88);
					back=Color.FromArgb(232,232,232);
					back2=Color.White;
					sideBack=Color.FromArgb(217,225,188);
					sideBack2=Color.FromArgb(151,170,111);
					headerBack=Color.FromArgb(244,247,222);
					headerBack2=Color.FromArgb(183,198,145);
					headerText=Color.FromArgb(85,114,78);
					itemText=Color.Black;
					itemHotBack=Color.FromArgb(255,244,204);
					itemHotBack2=Color.FromArgb(255,209,147);
					itemHotBorder=Color.FromArgb(96,128,88);
					itemPressedBack=Color.FromArgb(254,142,75);
					itemPressedBack2=Color.FromArgb(255,207,139);
					itemCheckedBack=Color.FromArgb(255,213,140);
					itemCheckedBack2=Color.FromArgb(255,173,85);
					break;
				}
				case eSideBarColorScheme.Orange:
				{
					border=Color.FromArgb(137,105,28);
					back=Color.FromArgb(232,232,232);
					back2=Color.White;
					sideBack=Color.FromArgb(249,225,164);
					sideBack2=Color.FromArgb(227,185,82);
					headerBack=Color.FromArgb(255,239,201);
					headerBack2=Color.FromArgb(242,210,132);
					headerText=Color.FromArgb(117,83,2);
					itemText=Color.Black;
					itemHotBack=Color.FromArgb(255,244,204);
					itemHotBack2=Color.FromArgb(255,209,147);
					itemHotBorder=Color.FromArgb(137,105,28);
					itemPressedBack=Color.FromArgb(254,142,75);
					itemPressedBack2=Color.FromArgb(255,207,139);
					itemCheckedBack=Color.FromArgb(255,213,140);
					itemCheckedBack2=Color.FromArgb(255,173,85);
					break;
				}
				case eSideBarColorScheme.Red:
				{
					border=Color.FromArgb(144,0,34);
					back=Color.FromArgb(232,232,232);
					back2=Color.White;
					sideBack=Color.FromArgb(255,174,193);
					sideBack2=Color.FromArgb(226,78,113);
					headerBack=Color.FromArgb(252,219,227);
					headerBack2=Color.FromArgb(254,150,174);
					headerText=Color.FromArgb(144,0,34);
					itemText=Color.Black;
					itemHotBack=Color.FromArgb(255,244,204);
					itemHotBack2=Color.FromArgb(255,209,147);
					itemHotBorder=Color.FromArgb(144,0,34);
					itemPressedBack=Color.FromArgb(254,142,75);
					itemPressedBack2=Color.FromArgb(255,207,139);
					itemCheckedBack=Color.FromArgb(255,213,140);
					itemCheckedBack2=Color.FromArgb(255,173,85);
					break;
				}
				case eSideBarColorScheme.LightBlue:
				{
					border=Color.FromArgb(81,100,136);
					back=Color.FromArgb(232,232,232);
					back2=Color.White;
					sideBack=Color.FromArgb(226,235,253);
					sideBack2=Color.FromArgb(175,190,218);
					headerBack=Color.FromArgb(255,255,255);
					headerBack2=Color.FromArgb(210,224,252);
					headerText=Color.FromArgb(69,84,115);
					itemText=Color.Black;
					itemHotBack=Color.FromArgb(255,244,204);
					itemHotBack2=Color.FromArgb(255,209,147);
					itemHotBorder=Color.FromArgb(81,100,136);
					itemPressedBack=Color.FromArgb(254,142,75);
					itemPressedBack2=Color.FromArgb(255,207,139);
					itemCheckedBack=Color.FromArgb(255,213,140);
					itemCheckedBack2=Color.FromArgb(255,173,85);
					break;
				}
				case eSideBarColorScheme.Money:
				{
					border=Color.FromArgb(44,72,112);
					back=Color.FromArgb(91,91,91);
					back2=Color.FromArgb(127,127,127);
					backText=Color.White;
					sideBack=Color.FromArgb(163,187,224);
					sideBack2=Color.FromArgb(99,131,177);
					headerBack=Color.FromArgb(77,108,152);
					headerBack2=Color.Empty;
					headerHotBack=Color.FromArgb(55,85,128); //Color.FromArgb(99,131,177);
					headerText=Color.White;
					itemText=Color.White;
					itemHotText=Color.FromArgb(255,223,127);
					headerHotText=Color.FromArgb(255,223,127);
					itemHotBack=Color.FromArgb(80,80,80);
					itemHotBack2=Color.FromArgb(60,60,60);
					itemHotBorder=Color.Black;
					itemPressedBack=Color.FromArgb(110,110,110);
					itemPressedBack2=Color.FromArgb(80,80,80);
					itemCheckedBack=Color.FromArgb(60,60,60);
					itemCheckedBack2=Color.FromArgb(80,80,80);
					break;
				}
			}

			SideBarColors c=new SideBarColors();
			c.Background=back;
			c.Background2=back2;
			c.Border=border;
			c.HeaderBackground=headerBack;
			c.HeaderBackground2=headerBack2;
			c.HeaderSideBackground=sideBack;
			c.HeaderSideBackground2=sideBack2;
			c.HeaderText=headerText;
			c.HeaderHotText=headerHotText;
			if(headerHotBack.IsEmpty)
			{
				c.HeaderHotBackground=headerBack2;
				c.HeaderHotBackground2=headerBack;
			}
			else
			{
				c.HeaderHotBackground=headerHotBack;
			}
			c.HeaderSideHotBackground=headerBack2;
			c.HeaderSideHotBackground2=headerBack;
			c.ItemCheckedBorder=itemHotBorder;
			c.ItemCheckedText=itemText;
			if(itemHotText.IsEmpty)
				c.ItemHotText=itemText;
			else
				c.ItemHotText=itemHotText;
			c.ItemHotBackground=itemHotBack;
			c.ItemHotBackground2=itemHotBack2;
			c.ItemHotBorder=itemHotBorder;
			c.ItemPressedText=itemText;
			c.ItemPressedBackground=itemPressedBack;
			c.ItemPressedBackground2=itemPressedBack2;
			c.ItemPressedBorder=itemHotBorder;
			c.ItemText=itemText;
			c.MenuBackground=back;
			c.MenuBackground2=back2;
			c.MenuBorder=itemHotBorder;
			c.MenuSide=sideBack;
			c.MenuSide=sideBack2;
			c.BackgroundText=backText;
			return c;
		}
		private static SideBarColors GetFlatColorScheme(eSideBarColorScheme scheme)
		{
			Color border=Color.Empty;
			Color background=Color.Empty;
			Color sideBack=Color.Empty;
			Color headerBack=Color.Empty;
			Color headerText=Color.Empty;
			Color headerHot=Color.Empty;
			Color itemHotBack=Color.Empty;
			Color itemHotBorder=Color.Empty;
			Color itemPressedBack=Color.Empty;

			switch(scheme)
			{
				case eSideBarColorScheme.Brick:
				{
					border=Color.FromArgb(66,0,0);
					background=Color.White;
					sideBack=Color.FromArgb(204,102,102);
					headerBack=Color.FromArgb(227,220,198);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(255,153,153);
					itemHotBorder=Color.FromArgb(132,0,0);
					headerHot=Color.FromArgb(245,238,217);
					break;
				}
				case eSideBarColorScheme.Wheat:
				{
					border=Color.FromArgb(132,130,0);
					background=Color.White;
					sideBack=Color.FromArgb(177,174,0);
					headerBack=Color.FromArgb(239,240,120);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(244,245,169);
					itemHotBorder=Color.FromArgb(132,130,0);
					headerHot=Color.FromArgb(254,255,178);
					break;
				}
				case eSideBarColorScheme.Storm:
				{
					border=Color.FromArgb(132,0,132);
					background=Color.White;
					sideBack=Color.FromArgb(162,78,162);
					headerBack=Color.FromArgb(226,189,226);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(226,189,226);
					itemHotBorder=Color.FromArgb(132,0,132);
					headerHot=Color.FromArgb(244,223,244);
					break;
				}
				case eSideBarColorScheme.Spruce:
				{
					border=Color.FromArgb(51,102,51);
					background=Color.White;
					sideBack=Color.FromArgb(90,150,99);
					headerBack=Color.FromArgb(165,228,165);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(204,255,204);
					itemHotBorder=Color.FromArgb(51,102,51);
					headerHot=Color.FromArgb(204,231,204);
					break;
				}
				case eSideBarColorScheme.Slate:
				{
					border=Color.FromArgb(34,82,127);
					background=Color.White;
					sideBack=Color.FromArgb(123,167,184);
					headerBack=Color.FromArgb(186,204,216);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(129,191,232);
					itemHotBorder=Color.FromArgb(34,82,127);
					headerHot=Color.FromArgb(232,232,232);
					break;
				}
				case eSideBarColorScheme.Rose:
				{
					border=Color.FromArgb(102,45,63);
					background=Color.White;
					sideBack=Color.FromArgb(182,100,125);
					headerBack=Color.FromArgb(206,174,181);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(241,163,180);
					itemHotBorder=Color.FromArgb(102,45,63);
					headerHot=Color.FromArgb(242,200,209);
					break;
				}
				case eSideBarColorScheme.Fire:
				{
					border=Color.FromArgb(92,0,0);
					background=Color.White;
					sideBack=Color.FromArgb(175,32,32);
					headerBack=Color.FromArgb(198,198,198);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(242,84,84);
					itemHotBorder=Color.FromArgb(92,0,0);
					headerHot=Color.FromArgb(255,142,142);
					break;
				}
				case eSideBarColorScheme.Pumpkin:
				{
					border=Color.FromArgb(123,96,27);
					background=Color.White;
					sideBack=Color.FromArgb(214,166,41);
					headerBack=Color.FromArgb(239,215,156);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(239,191,97);
					itemHotBorder=Color.FromArgb(123,96,27);
					headerHot=Color.FromArgb(255,239,198);
					break;
				}
				case eSideBarColorScheme.Plum:
				{
					border=Color.FromArgb(74,65,99);
					background=Color.White;
					sideBack=Color.FromArgb(119,106,154);
					headerBack=Color.FromArgb(173,154,148);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(205,188,182);
					itemHotBorder=Color.FromArgb(74,65,99);
					headerHot=Color.FromArgb(219,195,188);
					break;
				}
				case eSideBarColorScheme.Marine:
				{
					border=Color.FromArgb(0,0,132);
					background=Color.White;
					sideBack=Color.FromArgb(83,168,159);
					headerBack=Color.FromArgb(154,214,207);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(79,198,185);
					itemHotBorder=Color.FromArgb(0,0,132);
					headerHot=Color.FromArgb(188,231,226);
					break;
				}
				case eSideBarColorScheme.Sunset:
				{
					border=Color.FromArgb(176,87,0);
					background=Color.White;
					sideBack=Color.FromArgb(219,155,0);
					headerBack=Color.FromArgb(255,212,110);
					headerText=Color.Black;
					itemHotBack=Color.FromArgb(247,193,77);
					itemHotBorder=Color.FromArgb(176,87,0);
					headerHot=Color.FromArgb(253,226,162);
					break;
				}
			}
			
			itemPressedBack=ControlPaint.Light(itemHotBack);

			SideBarColors c=new SideBarColors();
			c.Background=background;
			c.Border=border;
			c.HeaderBackground=headerBack;
			c.HeaderSideBackground=sideBack;
			c.HeaderHotBackground=headerHot;
			c.HeaderSideHotBackground=headerHot;
			c.HeaderText=headerText;
			c.ItemCheckedBorder=border;
			c.ItemCheckedText=headerText;
			c.ItemHotText=headerText;
			c.ItemHotBackground=itemHotBack;
			c.ItemHotBorder=itemHotBorder;
			c.ItemPressedText=headerText;
			c.ItemPressedBackground=itemPressedBack;
			c.ItemPressedBorder=border;
			c.ItemText=headerText;
			c.MenuBackground=background;
			c.MenuBorder=border;
			c.MenuSide=sideBack;

			return c;
		}
		private static SideBarColors GetSystemColorScheme()
		{
			ColorScheme cs=new ColorScheme(eDotNetBarStyle.Office2003);
			SideBarColors c=new SideBarColors();
			c.Background=cs.MenuBackground;
			c.Background2=ControlPaint.LightLight(cs.MenuBackground);
			c.Border=cs.BarPopupBorder;
			c.HeaderBackground=cs.MenuSide;
			c.HeaderBackground2=cs.MenuSide2;
			c.HeaderSideBackground=cs.BarBackground;
			c.HeaderSideBackground2=cs.BarBackground2;
			c.HeaderText=cs.ItemText;
			c.HeaderHotText=cs.ItemHotText;
			c.HeaderHotBackground=c.HeaderBackground2;
			c.HeaderHotBackground2=c.HeaderBackground;
			c.HeaderSideHotBackground=c.HeaderHotBackground;
			c.HeaderSideHotBackground2=c.HeaderHotBackground2;
			c.ItemCheckedBorder=cs.ItemCheckedBorder;
			c.ItemCheckedText=cs.ItemCheckedText;
			c.ItemHotText=cs.ItemHotText;
			c.ItemHotBackground=cs.ItemHotBackground;
			c.ItemHotBackground2=cs.ItemHotBackground2;
			c.ItemHotBorder=cs.ItemHotBorder;
			c.ItemPressedText=cs.ItemPressedText;
			c.ItemPressedBackground=cs.ItemPressedBackground;
			c.ItemPressedBackground2=cs.ItemPressedBackground2;
			c.ItemPressedBorder=cs.ItemPressedBorder;
			c.ItemText=cs.ItemText;
			c.MenuBackground=cs.MenuBackground;
			c.MenuBackground2=cs.MenuBackground2;
			c.MenuBorder=cs.MenuBorder;
			c.MenuSide=cs.MenuSide;
			c.MenuSide2=cs.MenuSide2;
			c.BackgroundText=cs.ItemText;
			return c;
		}
	}

	internal class SideBarColors
	{
		public Color Border=Color.Empty;
		public Color Background=Color.Empty;
		public Color Background2=Color.Empty;
		public Color BackgroundText=Color.Empty;
		public Color HeaderBackground=Color.Empty;
		public Color HeaderBackground2=Color.Empty;
		public Color HeaderSideBackground=Color.Empty;
		public Color HeaderSideBackground2=Color.Empty;
		public Color HeaderHotBackground=Color.Empty;
		public Color HeaderHotBackground2=Color.Empty;
		public Color HeaderHotText=Color.Empty;
		public Color HeaderSideHotBackground=Color.Empty;
		public Color HeaderSideHotBackground2=Color.Empty;
		public Color HeaderText=Color.Empty;
		public Color ItemHotBackground=Color.Empty;
		public Color ItemHotBackground2=Color.Empty;
		public int ItemHotBackgroundGradientAngle=90;
		public Color ItemHotBorder=Color.Empty;
		public Color ItemHotText=Color.Empty;
		public Color ItemBackground=Color.Empty;
		public Color ItemText=Color.Empty;
		public Color ItemPressedBackground=Color.Empty;
		public Color ItemPressedBackground2=Color.Empty;
		public int ItemPressedBackgroundGradientAngle=90;
		public Color ItemPressedBorder=Color.Empty;
		public Color ItemPressedText=Color.Empty;
		public Color MenuBackground=Color.Empty;
		public Color MenuBackground2=Color.Empty;
		public int MenuBackgroundGradientAngle=0;
		public Color MenuBorder=Color.Empty;
		public Color MenuSide=Color.Empty;
		public Color MenuSide2=Color.Empty;
		public int MenuSideGradientAngle=0;
		public Color ItemCheckedBackground=Color.Empty;
		public Color ItemCheckedBackground2=Color.Empty;
		public int ItemCheckedBackgroundGradientAngle=90;
		public Color ItemCheckedBorder=Color.Empty;
		public Color ItemCheckedText=Color.Empty;

		public void Apply(SideBarPanelItem item)
		{
			item.SetBackgroundStyle(new ItemStyle());
			
			item.BackgroundStyle.BackColor1.Color=Background;
			item.BackgroundStyle.BackColor2.Color=Background2;
			item.BackgroundStyle.BorderColor.Color=Border;
			item.BackgroundStyle.Border=eBorderType.SingleLine;
			item.BackgroundStyle.BorderSide=eBorderSide.All;
			item.BackgroundStyle.ForeColor.Color=BackgroundText;
			
			item.SetHeaderStyle(new ItemStyle());
			item.HeaderStyle.BackColor1.Color=HeaderBackground;
			item.HeaderStyle.BackColor2.Color=HeaderBackground2;
			item.HeaderStyle.GradientAngle=90;
			item.HeaderStyle.BorderSide=eBorderSide.Right | eBorderSide.Top | eBorderSide.Bottom;
			item.HeaderStyle.Border=eBorderType.SingleLine;
			item.HeaderStyle.BorderColor.Color=Border;
			item.HeaderStyle.ForeColor.Color=HeaderText;
			item.HeaderStyle.Font=new Font("Arial",9,FontStyle.Bold);
			if(!HeaderHotBackground.IsEmpty)
			{
				item.SetHeaderHotStyle(new ItemStyle());
				item.HeaderHotStyle.BackColor1.Color=HeaderHotBackground;
				item.HeaderHotStyle.BackColor2.Color=HeaderHotBackground2;
				item.HeaderHotStyle.GradientAngle=90;
				if(!HeaderHotText.IsEmpty)
					item.HeaderHotStyle.ForeColor.Color=HeaderHotText;
			}
			else
				item.SetHeaderHotStyle(null);
			item.SetHeaderMouseDownStyle(null);
			
			item.SetHeaderSideStyle(new ItemStyle());
			item.HeaderSideStyle.Border=eBorderType.SingleLine;
			item.HeaderSideStyle.BorderColor.Color=Border;
			item.HeaderSideStyle.BorderSide=eBorderSide.Left | eBorderSide.Top | eBorderSide.Bottom;
			item.HeaderSideStyle.BackColor1.Color=HeaderSideBackground;
			item.HeaderSideStyle.BackColor2.Color=HeaderSideBackground2;
			item.HeaderSideStyle.GradientAngle=90;
			if(!HeaderSideHotBackground.IsEmpty)
			{
				item.SetHeaderSideHotStyle(new ItemStyle());
				item.HeaderSideHotStyle.BackColor1.Color=HeaderSideHotBackground;
				item.HeaderSideHotStyle.BackColor2.Color=HeaderSideHotBackground2;
				item.HeaderSideHotStyle.GradientAngle=90;
			}
			else
				item.SetHeaderSideHotStyle(null);
			item.SetHeaderSideMouseDownStyle(null);
		}

		public void Apply(ColorScheme scheme)
		{
			scheme.ItemBackground=this.ItemBackground;
			scheme.ItemCheckedBackground=this.ItemCheckedBackground;
			scheme.ItemCheckedBackground2=this.ItemCheckedBackground2;
			scheme.ItemCheckedBackgroundGradientAngle=this.ItemCheckedBackgroundGradientAngle;
			scheme.ItemCheckedBorder=this.ItemCheckedBorder;
			scheme.ItemCheckedText=this.ItemCheckedText;
			scheme.ItemHotBackground=this.ItemHotBackground;
			scheme.ItemHotBackground2=this.ItemHotBackground2;
			scheme.ItemHotBackgroundGradientAngle=this.ItemHotBackgroundGradientAngle;
			scheme.ItemHotBorder=this.ItemHotBorder;
			scheme.ItemHotText=this.ItemHotText;
			scheme.ItemPressedBackground=this.ItemPressedBackground;
			scheme.ItemPressedBackground2=this.ItemPressedBackground2;
			scheme.ItemPressedBackgroundGradientAngle=this.ItemPressedBackgroundGradientAngle;
			scheme.ItemPressedBorder=this.ItemPressedBorder;
			scheme.ItemPressedText=this.ItemPressedText;
			scheme.ItemText=this.ItemText;

			scheme.ItemExpandedBackground=this.ItemPressedBackground;
			scheme.ItemExpandedBackground2=this.ItemPressedBackground2;
			scheme.ItemExpandedBackgroundGradientAngle=this.ItemPressedBackgroundGradientAngle;
			scheme.ItemExpandedShadow=Color.Empty;
			scheme.ItemExpandedText=this.ItemPressedText;

			scheme.MenuBackground=this.MenuBackground;
			scheme.MenuBackground2=this.MenuBackground2;
			scheme.MenuBackgroundGradientAngle=this.MenuBackgroundGradientAngle;
			scheme.MenuBorder=this.MenuBorder;
			scheme.MenuSide=this.MenuSide;
			scheme.MenuSide2=this.MenuSide2;
			scheme.MenuSideGradientAngle=this.MenuSideGradientAngle;
		}
	}
}
