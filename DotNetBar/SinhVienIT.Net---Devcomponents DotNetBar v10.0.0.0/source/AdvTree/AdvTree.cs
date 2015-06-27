using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.AdvTree.Display;
using DevComponents.AdvTree.Layout;
using System.Xml;
using System.IO;
using DevComponents.DotNetBar;
using System.Runtime.Serialization;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
#if FRAMEWORK20
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Security;
#endif

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents advanced multi-column Tree control.
	/// </summary>
    [ToolboxItem(true), DefaultEvent("Click"), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.AdvTree.Design.AdvTreeDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ToolboxBitmap(typeof(ToolboxIconResFinder), "AdvTree.ico")]
	public class AdvTree : System.Windows.Forms.ScrollableControl, INodeNotify, ISupportInitialize
	{
		#region Private Variables
		private ElementStyleCollection m_Styles=new ElementStyleCollection();
		private ColumnHeaderCollection m_Columns=new ColumnHeaderCollection();
		private NodeCollection m_Nodes=new NodeCollection();
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ElementStyle m_CellStyleDefault=null;
		private ElementStyle m_CellStyleMouseDown=null;
		private ElementStyle m_CellStyleMouseOver=null;
		private ElementStyle m_CellStyleSelected=null;
		private ElementStyle m_CellStyleDisabled=null;
		private ElementStyle m_NodeStyleExpanded=null;
		private ElementStyle m_NodeStyle=null;
		private ElementStyle m_NodeStyleSelected=null;
		private ElementStyle m_NodeStyleMouseOver=null;

		private ElementStyle m_ColumnStyleNormal=null;
		private ElementStyle m_ColumnStyleMouseOver=null;
		private ElementStyle m_ColumnStyleMouseDown=null;

		private Layout.NodeLayout m_NodeLayout=null;
		private NodeDisplay m_NodeDisplay=null;

		private HeadersCollection m_Headers=null;

		private string m_PathSeparator=System.IO.Path.PathSeparator.ToString();

		private int m_UpdateSuspended=0;
		private bool m_PendingLayout=false;
		private bool m_SuspendPaint=false;

		private Node m_SelectedNode=null;
        private SelectedNodesCollection m_SelectedNodes = new SelectedNodesCollection();
		//private Cell m_SelectedCell=null;
		private Node m_MouseOverNode=null;
		private Cell m_MouseOverCell=null;
		private int m_CellMouseDownCounter=0;

		private ImageList m_ImageList=null;
		private int m_ImageIndex=-1;
		private bool m_AntiAlias=true;
		
		//private NodeConnector m_RootConnector=null;
		private NodeConnector m_NodesConnector=null;
		private NodeConnector m_LinkConnector=null;
		private NodeConnector m_SelectedPathConnector=null;
		private eCellLayout m_CellLayout=eCellLayout.Default;
		private eCellPartLayout m_CellPartLayout=eCellPartLayout.Default;
		private Cursor m_OriginalCursor=null;
		private Cursor m_DefaultCellCursor=null;
		private ColorScheme m_ColorScheme=null;
		private bool m_CenterContent=true;

		// Selection box properties
		private bool m_SelectionBox=true;
		private int m_SelectionBoxSize=4;
		//private Color m_SelectionBoxBorderColor=Color.Empty;
		//private Color m_SelectionBoxFillColor=Color.Empty;

		// Expand Part Properties
		private Size m_DefaultExpandPartSize=new Size(8,8);
		private Size m_ExpandButtonSize=Size.Empty;
		private Color m_ExpandBorderColor=Color.Empty;
		private eColorSchemePart m_ExpandBorderColorSchemePart=eColorSchemePart.None;
		private Color m_ExpandBackColor=Color.Empty;
		private eColorSchemePart m_ExpandBackColorSchemePart=eColorSchemePart.None;
		private Color m_ExpandBackColor2=Color.Empty;
		private eColorSchemePart m_ExpandBackColor2SchemePart=eColorSchemePart.None;
		private int m_ExpandBackColorGradientAngle=0;
		private Color m_ExpandLineColor=Color.Empty;
		private eColorSchemePart m_ExpandLineColorSchemePart=eColorSchemePart.None;
		private Image m_ExpandImage=null;
		private Image m_ExpandImageCollapse=null;
		private eExpandButtonType m_ExpandButtonType=eExpandButtonType.Rectangle;

		private Node m_DisplayRootNode=null;

		// Command button properties
		private int m_CommandWidth=10;
		private Color m_CommandBackColor=Color.Empty;
		private eColorSchemePart m_CommandBackColorSchemePart=eColorSchemePart.CustomizeBackground;
		private Color m_CommandBackColor2=Color.Empty;
		private eColorSchemePart m_CommandBackColor2SchemePart=eColorSchemePart.CustomizeBackground2;
		private Color m_CommandForeColor=Color.Empty;
		private eColorSchemePart m_CommandForeColorSchemePart=eColorSchemePart.CustomizeText;
		private int m_CommandBackColorGradientAngle=90;
		private Color m_CommandMouseOverBackColor=Color.Empty;
		private eColorSchemePart m_CommandMouseOverBackColorSchemePart=eColorSchemePart.ItemHotBackground;
		private Color m_CommandMouseOverBackColor2=Color.Empty;
		private eColorSchemePart m_CommandMouseOverBackColor2SchemePart=eColorSchemePart.ItemHotBackground2;
		private Color m_CommandMouseOverForeColor=Color.Empty;
		private eColorSchemePart m_CommandMouseOverForeColorSchemePart=eColorSchemePart.ItemHotText;
		private int m_CommandMouseOverBackColorGradientAngle=90;

		// Cell editing support
		private bool m_CellEdit=false;
		private bool m_CellEditing=false;
		private Cell m_EditedCell=null;
		private TextBoxEx m_EditTextBox=null;

		// Drag & Drop
		private bool m_DragDropEnabled=true;
		private Node m_DragNode=null;
		private Point m_MouseDownLocation=Point.Empty;

		// Layout
		private eNodeLayout m_Layout=eNodeLayout.Map;
		private int m_NodeHorizontalSpacing=32;
		private int m_NodeVerticalSpacing=32;
		private eDiagramFlow m_DiagramLayoutFlow=eDiagramFlow.LeftToRight;
		private eMapFlow m_MapLayoutFlow=eMapFlow.Spread;

		private ElementStyle m_BackgroundStyle=new ElementStyle();
		private eNodeRenderMode m_RenderMode=eNodeRenderMode.Default;
		private TreeRenderer m_NodeRenderer=null;
		
		private ArrayList m_HostedControlCells=new ArrayList();
		private float m_ZoomFactor=1f;
		private object m_DotNetBarManager=null;

        private DevComponents.DotNetBar.VScrollBarAdv _VScrollBar = null;
        private DevComponents.DotNetBar.ScrollBar.HScrollBarAdv _HScrollBar = null;
        private Control _Thumb = null;
        private ColumnHeaderControl _ColumnHeader = null;
        private ArrayList _FullRowBackgroundNodes = null;
        private DevComponents.AdvTree.Layout.LayoutSettings _LayoutSettings = new DevComponents.AdvTree.Layout.LayoutSettings();
		#endregion

		#region Events
        /// <summary>
        /// Occurs just before cell editor is released for editing. It allows you to customize any properties on edit control.
        /// </summary>
        [Description(" Occurs just before cell editor is released for editing. It allows you to customize any properties on edit control.")]
        public event PrepareCellEditorEventHandler PrepareCellEditorControl;
        /// <summary>
        /// Occurs when mouse button is pressed over the column header.
        /// </summary>
        public event MouseEventHandler ColumnHeaderMouseDown;
        /// <summary>
        /// Occurs when mouse button is released over the column header.
        /// </summary>
        public event MouseEventHandler ColumnHeaderMouseUp;
		/// <summary>
		/// Occurs after the cell check box is checked.
		/// </summary>
		public event AdvTreeCellEventHandler AfterCheck;

        /// <summary>
        /// Occurs before the cell check box is checked and provides opportunity to cancel the event.
        /// </summary>
        public event AdvTreeCellBeforeCheckEventHandler BeforeCheck;
		
		/// <summary>
		/// Occurs after the tree node is collapsed.
		/// </summary>
		public event AdvTreeNodeEventHandler AfterCollapse;
		
		/// <summary>
		/// Occurs before the tree node is collapsed.
		/// </summary>
		public event AdvTreeNodeCancelEventHandler BeforeCollapse;
		
		/// <summary>
		/// Occurs after the tree node is expanded.
		/// </summary>
		public event AdvTreeNodeEventHandler AfterExpand;
		
		/// <summary>
		/// Occurs before the tree node is expanded.
		/// </summary>
		public event AdvTreeNodeCancelEventHandler BeforeExpand;

		/// <summary>
		/// Occurs when command button on node is clicked.
		/// </summary>
		public event CommandButtonEventHandler CommandButtonClick;

		/// <summary>
		/// Occurs before cell is edited. The order of the cell editing events is as follows:
        /// BeforeCellEdit, CellEditEnding, AfterCellEdit, AfterCellEditComplete.
		/// </summary>
		public event CellEditEventHandler BeforeCellEdit;
		/// <summary>
		/// Occurs just before the cell editing is ended. The text box for editing is still visible and you can cancel
		/// the exit out of editing mode at this point. The order of the cell editing events is as follows:
        /// BeforeCellEdit, CellEditEnding, AfterCellEdit, AfterCellEditComplete.
		/// </summary>
		public event CellEditEventHandler CellEditEnding;
		/// <summary>
		/// Occurs after cell editing has ended and before the new text entered by the user is assigned to the cell. You can abort the edits in this event.
		/// The order of the cell editing events is as follows:
        /// BeforeCellEdit, CellEditEnding, AfterCellEdit, AfterCellEditComplete.
		/// </summary>
		public event CellEditEventHandler AfterCellEdit;

        /// <summary>
        /// Occurs after cell editing has been completed. This event cannot be canceled.
        /// </summary>
        public event CellEditEventHandler AfterCellEditComplete;
        /// <summary>
        /// Occurs after node selection has changed.
        /// </summary>
        public event EventHandler SelectionChanged;
		/// <summary>
		/// Occurs before Node has been selected by user or through the SelectedNode property. Event can be cancelled.
		/// </summary>
		public event AdvTreeNodeCancelEventHandler BeforeNodeSelect;
		
		/// <summary>
		/// Occurs after node has been selected by user or through the SelectedNode property.
		/// </summary>
		public event AdvTreeNodeEventHandler AfterNodeSelect;

        /// <summary>
        /// Occurs after node has been deselected by user or through the SelectedNode or SelectedNodes properties.
        /// </summary>
        public event AdvTreeNodeEventHandler AfterNodeDeselect;
		
		/// <summary>
		/// Occurs before node has been removed from its parent.
		/// </summary>
		public event TreeNodeCollectionEventHandler BeforeNodeRemove;

		/// <summary>
		/// Occurs after node has been removed from its parent.
		/// </summary>
		public event TreeNodeCollectionEventHandler AfterNodeRemove;

		/// <summary>
		/// Occurs before node is inserted or added as child node to parent node.
		/// </summary>
		public event TreeNodeCollectionEventHandler BeforeNodeInsert;

		/// <summary>
		/// Occurs after node is inserted or added as child node.
		/// </summary>
		public event TreeNodeCollectionEventHandler AfterNodeInsert;

        /// <summary>
        /// Occurs when node drag &amp; drop operation is initiated.
        /// </summary>
        public event EventHandler NodeDragStart;
        /// <summary>
        /// Occurs before internal node drag &amp; drop support is initiated and allows you to cancel the drag &amp; drop.
        /// </summary>
        public event AdvTreeNodeCancelEventHandler BeforeNodeDragStart;
		/// <summary>
		/// Occurs before Drag-Drop of a node is completed and gives you information about new parent of the node that is being dragged
		/// as well as opportunity to cancel the operation.
		/// </summary>
		public event TreeDragDropEventHandler BeforeNodeDrop;

        /// <summary>
        /// Occurs while node is being dragged. You can handle this event to disable the drop at specific nodes or to even change the
        /// drop location for the node by modifying event arguments.
        /// </summary>
        public event TreeDragFeedbackEventHander NodeDragFeedback;
		
		/// <summary>
		/// Occurs after Drag-Drop of a node is completed. This operation cannot be cancelled.
		/// </summary>
		public event TreeDragDropEventHandler AfterNodeDrop;

		/// <summary>
		/// Occurs when the mouse pointer is over the node and a mouse button is pressed.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeMouseDown;
		
		/// <summary>
		/// Occurs when the mouse pointer is over the node and a mouse button is released.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeMouseUp;
		
		/// <summary>
		/// Occurs when the mouse pointer is moved over the node.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeMouseMove;
		
		/// <summary>
		/// Occurs when the mouse enters the node.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeMouseEnter;
		
		/// <summary>
		/// Occurs when the mouse leaves the node.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeMouseLeave;
		
		/// <summary>
		/// Occurs when the mouse hovers over the node.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeMouseHover;
		
		/// <summary>
		/// Occurs when the node is clicked with left mouse button. If you need to know more information like if another mouse button is clicked etc. use
		/// NodeMouseDown event.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeClick;
		
		/// <summary>
		/// Occurs when the node is double-clicked.
		/// </summary>
		public event TreeNodeMouseEventHandler NodeDoubleClick;
		
		/// <summary>
		/// Occurs after an node has been serialized to XmlElement and provides you with opportunity to add any custom data
		/// to serialized XML. This allows you to serialize any data associated with the node and load it back up in DeserializeNode event.
		/// </summary>
		/// <remarks>
		/// 	<para>To serialize custom data to XML definition control creates handle this event and use CustomXmlElement
		/// property on SerializeNodeEventArgs to add new nodes or set attributes with custom data you want saved.</para>
		/// </remarks>
		public event SerializeNodeEventHandler SerializeNode;

		/// <summary>
		/// Occurs after an node has been de-serialized (loaded) from XmlElement and provides you with opportunity to load any custom data
		/// you have serialized during SerializeItem event.
		/// </summary>
		/// <remarks>
		/// 	<para>To de-serialize custom data from XML definition handle this event and use CustomXmlElement
		/// property on SerializeItemEventArgs to retrieve any data you saved in SerializeNode event.</para>
		/// </remarks>
		public event SerializeNodeEventHandler DeserializeNode;
		
		/// <summary>
		/// Occurs when hyperlink in text-markup is clicked.
		/// </summary>
		public event MarkupLinkClickEventHandler MarkupLinkClick;

        /// <summary>
        /// Occurs when cell with custom editor type is about to be edited by user. Handle this event to provide 
        /// custom editors.
        /// </summary>
        public event CustomCellEditorEventHandler ProvideCustomCellEditor;

#if FRAMEWORK20
        /// <summary>
        /// Occurs when the DataSource changes.
        /// </summary>
        [Description("Occurs when the DataSource changes.")]
        public event EventHandler DataSourceChanged;
        /// <summary>
        /// Occurs when the DisplayMembers property changes.
        /// </summary>
        [Description("Occurs when the DisplayMembers property changes")]
        public event EventHandler DisplayMembersChanged;
        /// <summary>
        /// Occurs when the control is bound to a data value that need to be converted.
        /// </summary>
        [Description("Occurs when the control is bound to a data value that need to be converted.")]
        public event DevComponents.DotNetBar.Controls.TreeConvertEventHandler Format;
        /// <summary>
        /// Occurs when FormattingEnabled property changes.
        /// </summary>
        [Description("Occurs when FormattingEnabled property changes.")]
        public event EventHandler FormattingEnabledChanged;
        /// <summary>
        /// Occurs when FormatString property changes.
        /// </summary>
        [Description("Occurs when FormatString property changes.")]
        public event EventHandler FormatStringChanged;
        /// <summary>
        /// Occurs when FormatInfo property has changed.
        /// </summary>
        [Description("Occurs when FormatInfo property has changed.")]
        public event EventHandler FormatInfoChanged;
        /// <summary>
        /// Occurs when a Node for an data-bound object item has been created and provides you with opportunity to modify the node.
        /// </summary>
        [Description("Occurs when a Node for an data-bound object item has been created and provides you with opportunity to modify the node")]
        public event DevComponents.DotNetBar.Controls.DataNodeEventHandler DataNodeCreated;
        /// <summary>
        /// Occurs when a group Node is created as result of GroupingMembers property setting and provides you with opportunity to modify the node.
        /// </summary>
        [Description("Occurs when a group Node is created as result of GroupingMembers property setting and provides you with opportunity to modify the node")]
        public event DevComponents.DotNetBar.Controls.DataNodeEventHandler GroupNodeCreated;
        /// <summary>
        /// Occurs when value of ValueMember property has changed.
        /// </summary>
        [Description("Occurs when value of ValueMember property has changed.")]
        public event EventHandler ValueMemberChanged;
        /// <summary>
        /// Occurs when value of SelectedValue property has changed.
        /// </summary>
        [Description("Occurs when value of SelectedValue property has changed.")]
        public event EventHandler SelectedValueChanged;
        /// <summary>
        /// Occurs when value of SelectedIndex property has changed.
        /// </summary>
        [Description("Occurs when value of SelectedValue property has changed.")]
        public event EventHandler SelectedIndexChanged;
        /// <summary>
        /// Occurs when ColumnHeader is automatically created by control as result of data binding and provides you with opportunity to modify it.
        /// </summary>
        [Description("Occurs when ColumnHeader is automatically created by control as result of data binding and provides you with opportunity to modify it.")]
        public event DevComponents.DotNetBar.Controls.DataColumnEventHandler DataColumnCreated;
#endif
        /// <summary>
        /// Occurs after column has been resized by end-user.
        /// </summary>
        [Description("Occurs after column has been resized by end-user")]
        public event EventHandler ColumnResized;
        /// <summary>
        /// Occurs while column is being resized by end-user.
        /// </summary>
        [Description("Occurs while column is being resized by end-user.")]
        public event EventHandler ColumnResizing;

        /// <summary>
        /// Occurs after cell has been selected.
        /// </summary>
        [Description("Occurs after cell has been selected.")]
        public event AdvTreeCellEventHandler CellSelected;
        /// <summary>
        /// Occurs after cell has been unselected.
        /// </summary>
        [Description("Occurs after cell has been unselected.")]
        public event AdvTreeCellEventHandler CellUnselected;
        /// <summary>
        /// Occurs after users has moved the column.
        /// </summary>
        [Description("Occurs after users has moved the column.")]
        public event ColumnMovedHandler ColumnMoved;
		#endregion

		#region Constructor/Dispose
		/// <summary>Creates new instance of the class.</summary>
		public AdvTree()
		{
            if (!ColorFunctions.ColorsLoaded)
            {
                NativeFunctions.RefreshSettings();
                NativeFunctions.OnDisplayChange();
                ColorFunctions.LoadColors();
            }
            m_SelectedNodes.TreeSelectionControl = this;
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, true);

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			m_BackgroundStyle.StyleChanged+=new EventHandler(ElementStyleChanged);
			m_BackgroundStyle.TreeControl=this;
            
            m_Columns.Parent = this;

			// Setup layout helper
			m_NodeLayout=new NodeTreeLayout(this,this.ClientRectangle, _LayoutSettings);
            _LayoutSettings.NodeVerticalSpacing = _NodeSpacing;

			m_NodeLayout.LeftRight=this.RtlTranslateLeftRight(LeftRightAlignment.Left);

#if TRIAL
			NodeOperations.ColorExpAlt();
#endif
			// Setup display helper
            m_NodeDisplay = new NodeTreeDisplay(this);

			m_Headers=new HeadersCollection();

			m_Nodes.TreeControl=this;
			m_Styles.TreeControl=this;

			m_ColorScheme=new ColorScheme(eColorSchemeStyle.Office2007);

			//m_SelectionBoxBorderColor=GetDefaultSelectionBoxBorderColor();
			//m_SelectionBoxFillColor=GetDefaultSelectionBoxFillColor();

			m_ExpandButtonSize=GetDefaultExpandButtonSize();

			this.AllowDrop = true;
            this.IsAccessible = true;
		}

        private bool _Disposed = false;
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            _Disposed = true;
			if( disposing )
			{
				if( components != null )
					components.Dispose();

                System.Windows.Forms.Timer timer = _SearchBufferExpireTimer;
                _SearchBufferExpireTimer = null;
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
			}

            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _CheckBoxImageChecked);
                BarUtilities.DisposeImage(ref _CheckBoxImageIndeterminate);
                BarUtilities.DisposeImage(ref _CheckBoxImageUnChecked);
                BarUtilities.DisposeImage(ref m_ExpandImage);
                BarUtilities.DisposeImage(ref m_ExpandImageCollapse);
            }

            if (m_Nodes != null && disposing)
            {
                foreach (Node node in m_Nodes)
                    node.Dispose();
                //m_Nodes.Clear();
            }

			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Properties
        /// <summary>
        /// Gets the column header control which renderes the columns.
        /// </summary>
        internal ColumnHeaderControl ColumnHeaderControl
        {
            get { return _ColumnHeader; }
        }
        private eScrollBarAppearance _ScrollBarAppearance = eScrollBarAppearance.Default;
        /// <summary>
        /// Gets or sets the scroll-bar visual style.
        /// </summary>
        [DefaultValue(eScrollBarAppearance.Default), Category("Appearance"), Description("Gets or sets the scroll-bar visual style.")]
        public eScrollBarAppearance ScrollBarAppearance
        {
            get { return _ScrollBarAppearance; }
            set
            { 
                _ScrollBarAppearance = value;
                OnScrollBarAppearance();
            }
        }
        private void OnScrollBarAppearance()
        {
            if (_VScrollBar != null) _VScrollBar.Appearance = _ScrollBarAppearance;
            if (_HScrollBar != null) _HScrollBar.Appearance = _ScrollBarAppearance;
        }

        private Size _TileSize = new Size(184, 30);
        /// <summary>
        /// Gets or sets the proposed size of the tile in Tile view. The size of the tile might be larger than specified if Style assigned to node, cells adds padding, margins etc. or if Node.Image or font is greater than width or height specified here.
        /// </summary>
        [Category("Appearance"), Description("Indicates size of the tile in Tile view.")]
        public Size TileSize
        {
            get { return _TileSize; }
            set
            {
                if (value != _TileSize)
                {
                    Size oldValue = _TileSize;
                    _TileSize = value;
                    OnTileSizeChanged(oldValue, value);
                }
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTileSize()
        {
            return _TileSize.Width != 184 || _TileSize.Height != 30;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTileSize()
        {
            TileSize = new Size(184, 30);
        }
        private void OnTileSizeChanged(Size oldValue, Size newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("TileSize"));
        }

        private bool _HideSelection = false;
        /// <summary>
        /// Gets or sets a value indicating whether the selected tree node remains highlighted even when the tree control has lost the focus.
        /// </summary>
        [DefaultValue(false), Category("Selection"), Description("Indicates whether the selected tree node remains highlighted even when the tree control has lost the focus.")]
        public bool HideSelection
        {
            get { return _HideSelection; }
            set
            {
                _HideSelection = value;
                if (this.SelectedNode != null && !this.IsKeyboardFocusWithin)
                    this.Invalidate();
            }
        }

        private bool _IsKeyboardFocusWithin = false;
        /// <summary>
        /// Gets whether keyboard focus is within the control.
        /// </summary>
        [Browsable(false)]
        public bool IsKeyboardFocusWithin
        {
            get { return _IsKeyboardFocusWithin; }
#if FRAMEWORK20
            internal set
#else
            set
#endif
            {
                _IsKeyboardFocusWithin = value;
                OnIsKeyboardFocusWithinChanged();
            }
        }
        protected virtual void OnIsKeyboardFocusWithinChanged()
        {
            if (_IsKeyboardFocusWithin)
            {
                if (_MultiSelect)
                {
                    foreach (Node node in m_SelectedNodes)
                    {
                        InvalidateNode(node);
                    }
                }
                else if (this.SelectedNode != null)
                    InvalidateNode(this.SelectedNode);
            }
            else
            {
                if (_MultiSelect)
                {
                    foreach (Node node in m_SelectedNodes)
                    {
                        InvalidateNode(node);
                    }
                }
                else if (this.SelectedNode != null)
                    InvalidateNode(this.SelectedNode);

                if (m_CellEditing && _EndCellEditingOnLostFocus)
                    EndCellEditing(eTreeAction.Keyboard);
            }
        }

        private bool _EndCellEditingOnLostFocus = true;
        /// <summary>
        /// Gets or sets whether cell editing is completed when control loses input focus. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Editing"), Description("Indicates whether cell editing is completed when control loses input focus. Default value is true.")]
        public bool EndCellEditingOnLostFocus
        {
            get { return _EndCellEditingOnLostFocus; }
            set
            {
                _EndCellEditingOnLostFocus = value;
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            this.IsKeyboardFocusWithin = true;
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.IsKeyboardFocusWithin = false;
            base.OnLeave(e);
        }

        internal ArrayList FullRowBackgroundNodes
        {
            get
            {
                return _FullRowBackgroundNodes;
            }
            set
            {
                _FullRowBackgroundNodes = value;
            }
        }

        private ContextMenuBar _ContextMenuBar = null;
		/// <summary>
		/// Gets or sets the reference to DotNetBar ContextMenuBar component which is used to provide context menu for nodes. This property
		/// is automatically maintained by AdvTree.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null), Browsable(false)]
		public ContextMenuBar ContextMenuBar
		{
            get { return _ContextMenuBar; }
            set { _ContextMenuBar = value; }
		}
		
		/// <summary>
		/// Gets or sets zoom factor for the control. Default value is 1. To zoom display of the nodes for 20% set zoom factor to 1.2
		/// To zoom view 2 times set zoom factor to 2. Value must be greater than 0. Zoom is supported only when non-column tree setup is used.
		/// </summary>
		[Browsable(false), DefaultValue(1f), Category("Layout"), Description("Indicates zoom factor for the control.")]
		public float Zoom
		{
			get { return m_ZoomFactor;}
			set
			{
				if(value<=0.1)
					return;
				m_ZoomFactor = value;
				this.RecalcLayout();
			}
		}
		
		/// <summary>
		/// Gets the size of the tree.
		/// </summary>
		[Browsable(false)]
		public Size TreeSize
		{
			get
			{
				return new Size(m_NodeLayout.Width, m_NodeLayout.Height);
			}
		}
		
		/// <summary>
		/// Gets or sets custom node renderer. You can set this property to your custom renderer. When set the RenderMode should be set to Custom to enable
		/// your custom renderer. To choose one of the system renderer use RenderMode property. Default value is null.
		/// </summary>
		[Browsable(false), Category("Style"), DefaultValue(null), Description("Indicates render mode used to render the node.")]
		internal TreeRenderer NodeRenderer
		{
			get { return m_NodeRenderer;}
			set
			{
				m_NodeRenderer = value;
				OnDisplayChanged();
			}
		}
		/// <summary>
		/// Gets or sets the render mode used to render all nodes. Default value is eNodeRenderMode.Default which indicates that system default renderer is used.
		/// Note that if you specify custom renderer you need to set AdvTree.NodeRenderer property to your custom renderer.
		/// </summary>
		[Browsable(true), Category("Style"), DefaultValue(eNodeRenderMode.Default), Description("Indicates render mode used to render the node.")]
		internal eNodeRenderMode RenderMode
		{
			get { return m_RenderMode;}
			set
			{
				m_RenderMode = value;
				OnDisplayChanged();
			}
		}
		
		/// <summary>
		/// Gets the style for the background of the control.
		/// </summary>
		[Browsable(true),Category("Appearance"),Description("Gets the style for the background of the control."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ElementStyle BackgroundStyle
		{
			get {return m_BackgroundStyle;}
		}

        /// <summary>
        /// Gets the current renderer used by the control.
        /// </summary>
        /// <returns>Reference to the TreeRenderer used by the control.</returns>
        public TreeRenderer GetRenderer()
        {
            if (m_RenderMode == eNodeRenderMode.Default)
            {
                if (m_NodeDisplay is NodeTreeDisplay)
                    return ((NodeTreeDisplay)m_NodeDisplay).SystemRenderer;
                return null;
            }
            else
                return m_NodeRenderer;
        }

        /// <summary>
        /// Gets or sets internal layout cell horizontal spacing. This property is for advanced internal use and you should not set it.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int CellHorizontalSpacing
        {
            get
            {
                return _LayoutSettings.CellHorizontalSpacing;
            }
            set
            {
                _LayoutSettings.CellHorizontalSpacing = value;
            }
        }

        ///// <summary>
        ///// Gets or sets the flow of nodes when Diagram layout is used. Note that this setting applies only to Diagram layout type.
        ///// </summary>
        //[Browsable(true),Category("Layout"),DefaultValue(eDiagramFlow.LeftToRight),Description("Indicates flow of nodes when Diagram layout is used.")]
        //public eDiagramFlow DiagramLayoutFlow
        //{
        //    get {return m_DiagramLayoutFlow;}
        //    set
        //    {
        //        m_DiagramLayoutFlow=value;
        //        OnLayoutChanged();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the flow of nodes when Map layout is used. Note that this setting applies only to Map layout type.
        ///// </summary>
        //[Browsable(true),Category("Layout"),DefaultValue(eMapFlow.Spread),Description("Indicates flow of nodes when Map layout is used.")]
        //public eMapFlow MapLayoutFlow
        //{
        //    get {return m_MapLayoutFlow;}
        //    set
        //    {
        //        m_MapLayoutFlow=value;
        //        OnLayoutChanged();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the horizontal spacing in pixels between nodes. Default value is 32.
        ///// </summary>
        //[Browsable(true),Category("Layout"),DefaultValue(32),Description("Indicates horizontal spacing in pixels between nodes.")]
        //public int NodeHorizontalSpacing
        //{
        //    get {return m_NodeHorizontalSpacing;}
        //    set
        //    {
        //        m_NodeHorizontalSpacing=value;
        //        OnLayoutChanged();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the vertical spacing in pixels between nodes. Default value is 16.
        ///// </summary>
        //[Browsable(true),Category("Layout"),DefaultValue(32),Description("Indicates vertical spacing in pixels between nodes.")]
        //public int NodeVerticalSpacing
        //{
        //    get {return m_NodeVerticalSpacing;}
        //    set
        //    {
        //        m_NodeVerticalSpacing=value;
        //        OnLayoutChanged();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the layout type for the nodes.
        ///// </summary>
        //[Browsable(true),Category("Layout"),DefaultValue(eNodeLayout.Map),Description("Indicates layout type for the nodes.")]
        //public eNodeLayout LayoutType
        //{
        //    get {return m_Layout;}
        //    set
        //    {
        //        if(m_Layout!=value)
        //        {
        //            SetLayout(value);
        //        }
        //    }
        //}

		/// <summary>
		/// Gets or sets whether automatic drag and drop is enabled. Default value is true.
		/// </summary>
		[Browsable(true),Category("Behavior"),Description("Indicates whether automatic drag and drop is enabled."),DefaultValue(true)]
		public bool DragDropEnabled
		{
			get {return m_DragDropEnabled;}
			set {m_DragDropEnabled=value;}
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

		/// <summary>
		/// Gets or sets whether anti-alias smoothing is used while painting the tree.
		/// </summary>
		[DefaultValue(true),Browsable(true),Category("Appearance"),Description("Gets or sets whether anti-aliasing is used while painting the tree.")]
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

		/// <summary>
		/// Gets or sets the delimiter string that the tree node path uses.
		/// </summary>
		[DefaultValue("\\"),Browsable(false),Description("Indicates the delimiter string that the tree node path uses.")]
		public string PathSeparator
		{
			get{return m_PathSeparator;}
			set{m_PathSeparator=value;}
		}

        private bool _AllowUserToResizeColumns = true;
        /// <summary>
        /// Gets or sets whether user can resize the columns. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Columns"), Description("Indicates whether user can resize the columns.")]
        public bool AllowUserToResizeColumns
        {
            get { return _AllowUserToResizeColumns; }
            set
            {
                _AllowUserToResizeColumns = value;
            }
        }

        private bool _AllowUserToReorderColumns = false;
        /// <summary>
        /// Gets or sets whether user can reorder the columns. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Columns"), Description("Indicates whether user can reorder the columns.")]
        public bool AllowUserToReorderColumns
        {
            get { return _AllowUserToReorderColumns; }
            set
            {
                _AllowUserToReorderColumns = value;
            }
        }
        

		/// <summary>
		/// Gets the collection of column headers that appear in the tree.
		/// </summary>
		/// <remarks>
		/// 	<para>By default there are no column headers defined. In that case tree control
		///     functions as regular tree control where text has unrestricted width.</para>
		/// 	<para>If you want to restrict the horizontal width of the text but not display
		///     column header you can create one column and set its width to the width desired and
		///     set its Visible property to false.</para>
		/// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Columns"), Description("Gets collection of column headers that appear in the tree.")]
        public ColumnHeaderCollection Columns
		{
			get
			{
				return m_Columns;
			}
		}

        private bool _ColumnsVisible = true;
        /// <summary>
        /// Gets or sets whether column headers are visible if they are defined through Columns collection. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Columns"), Description("Indicates whether column headers are visible if they are defined through Columns collection.")]
        public bool ColumnsVisible
        {
            get { return _ColumnsVisible; }
            set
            {
                if (_ColumnsVisible != value)
                {
                    _ColumnsVisible = value;
                    if (m_Columns.Count > 0) RecalcLayout();
                }
            }
        }

        internal void SetColumnHeaderControlVisibility(bool visible)
        {
            if (visible)
            {
                if (_ColumnHeader == null)
                {
                    _ColumnHeader = new ColumnHeaderControl();
                    _ColumnHeader.Columns = this.Columns;
                    this.Controls.Add(_ColumnHeader);
                }
            }
            else if(_ColumnHeader!=null)
            {
                _ColumnHeader.Columns = null;
                this.Controls.Remove(_ColumnHeader);
                _ColumnHeader.Dispose();
                _ColumnHeader = null;
            }
            RepositionColumnHeader();
        }

        internal bool CanResizeColumnAt(int x, int y)
        {
            if (!m_Columns.Bounds.Contains(x, y)) return false;

            ColumnHeader col = GetColumnForResizeAt(x, y, m_Columns, false);
            return col != null;
        }

        internal ColumnHeader GetColumnAt(int x, int y, ColumnHeaderCollection columns)
        {
            Point p = new Point(x, y);
            Point offset = Point.Empty;
            if (AutoScroll)
            {
                offset = GetAutoScrollPositionOffset();
                if (columns.ParentNode == null)
                    offset.Y = 0;
            }

            Rectangle columnsBounds = columns.Bounds;
            columnsBounds.Offset(offset);
            if (!columnsBounds.Contains(p)) return null;

            foreach (ColumnHeader item in columns)
            {
                if (!item.Visible) continue;
                Rectangle r = item.Bounds;
                r.Offset(offset);
                if (r.Contains(p))
                    return item;
            }
            return null;
        }

        private ColumnHeader GetColumnForResizeAt(int x, int y, ColumnHeaderCollection columns, bool ignoreY)
        {
            return GetColumnForResizeAt(x, y, columns, ignoreY, false);
        }
        private ColumnHeader GetColumnForResizeAt(int x, int y, ColumnHeaderCollection columns, bool ignoreY, bool ignoreLastColumn)
        {
            Point p = new Point(x, y);
            Point offset = Point.Empty;
            if (AutoScroll)
            {
                offset = GetAutoScrollPositionOffset();
                if (columns.ParentNode == null)
                   offset.Y = 0;
            }

            Rectangle columnsBounds = columns.Bounds;
            columnsBounds.Offset(offset);
            if (!columnsBounds.Contains(p) && !ignoreY) return null;

            int count = columns.Count - (ignoreLastColumn ? 1 : 0);
            for (int i = 0; i < count; i++)
            {
                ColumnHeader item = columns[i];
                Rectangle r = new Rectangle(item.Bounds.Right - 4, item.Bounds.Y, 6, item.Bounds.Height);
                r.Offset(offset);
                if (r.Contains(p) || ignoreY && p.X >= r.X && p.X <= r.Right)
                    return item;
            }
            return null;
        }

        private ColumnHeader _ColumnReorder = null;
        private Cursor _OldCursor = null;
        private int _ColumnMoveMarkerIndex = -1;
        private int ColumnMoveMarkerIndex
        {
            get
            {
                return _ColumnMoveMarkerIndex;
            }
            set
            {
                if (_ColumnReorder == null) throw new ArgumentException("Column is not being reordered");
                _ColumnMoveMarkerIndex = value;
                if (_ColumnReorder.Parent == m_Columns)
                    _ColumnHeader.ColumnMoveMarkerIndex = value;
                else
                {
                    this.Invalidate(_ColumnReorder.Parent.Bounds);
                }   
            }
        }
        internal void StartColumnReorder(int x, int y)
        {
            StartColumnReorder(x, y, m_Columns);
        }
        private void StartColumnReorder(int x, int y, ColumnHeaderCollection columns)
        {
            ColumnHeader col = GetColumnAt(x, y, columns);
            if (col == null) return;
            _OldCursor = this.Cursor;
            this.Cursor = Cursors.Hand;
            this.Capture = true;
            _ColumnReorder = col;
            this.ColumnMoveMarkerIndex = columns.IndexOf(col);
            this.Invalidate();
        }
        
        private ColumnHeader _ColumnResize = null;
        internal void StartColumnResize(int x, int y)
        {
            StartColumnResize(x, y, m_Columns);
        }

        private void StartColumnResize(int x, int y, ColumnHeaderCollection columns)
        {
            ColumnHeader col = GetColumnForResizeAt(x, y, columns, _GridColumnLineResizeEnabled, _GridColumnLineResizeEnabled);
            if (col == null) return;

            this.Capture = true;
            _ColumnResize = col;
        }

		/// <summary>
		/// Gets the collection of all style elements created for the tree.
		/// </summary>
		[Browsable(true),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ElementStyleCollection Styles
		{
			get {return m_Styles;}
		}

		/// <summary>
		/// Gets or sets default style for the node cell.
		/// </summary>
        [Browsable(true), Category("Node Style"), DefaultValue(null), Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates default style for the node cell.")]
		public ElementStyle CellStyleDefault
		{
			get {return m_CellStyleDefault;}
			set
			{
				if(m_CellStyleDefault!=value)
				{
					if(m_CellStyleDefault!=null)
						m_CellStyleDefault.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_CellStyleDefault=value;
					this.OnCellStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets default style for the node cell when mouse is pressed.
		/// </summary>
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell when mouse is pressed.")]
		public ElementStyle CellStyleMouseDown
		{
			get {return m_CellStyleMouseDown;}
			set
			{
				if(m_CellStyleMouseDown!=value)
				{
					if(m_CellStyleMouseDown!=null)
						m_CellStyleMouseDown.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_CellStyleMouseDown=value;
					this.OnCellStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets default style for the node cell when mouse is over the cell.
		/// </summary>
        [Browsable(true), Category("Node Style"), DefaultValue(null), Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates default style for the node cell when mouse is over the cell.")]
		public ElementStyle CellStyleMouseOver
		{
			get {return m_CellStyleMouseOver;}
			set
			{
				if(m_CellStyleMouseOver!=value)
				{
					if(m_CellStyleMouseOver!=null)
						m_CellStyleMouseOver.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_CellStyleMouseOver=value;
					this.OnCellStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets default style for the node cell when cell is selected.
		/// </summary>
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell when cell is selected.")]
		public ElementStyle CellStyleSelected
		{
			get {return m_CellStyleSelected;}
			set
			{
				if(m_CellStyleSelected!=value)
				{
					if(m_CellStyleSelected!=null)
						m_CellStyleSelected.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_CellStyleSelected=value;
					this.OnCellStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets default style for the node cell when cell is disabled.
		/// </summary>
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell when cell is disabled.")]
		public ElementStyle CellStyleDisabled
		{
			get {return m_CellStyleDisabled;}
			set
			{
				if(m_CellStyleDisabled!=value)
				{
					if(m_CellStyleDisabled!=null)
						m_CellStyleDisabled.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_CellStyleDisabled=value;
					this.OnCellStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets default style for the node when node is expanded.
		/// </summary>
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets default style for the node cell when node that cell belongs to is expanded.")]
		public ElementStyle NodeStyleExpanded
		{
			get {return m_NodeStyleExpanded;}
			set
			{
				if(m_NodeStyleExpanded!=value)
				{
					if(m_NodeStyleExpanded!=null)
						m_NodeStyleExpanded.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_NodeStyleExpanded=value;
					this.OnCellStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets default style for all nodes where style is not specified
		/// explicity.
		/// </summary>
		/// <value>
		/// Name of the style assigned or null value indicating that no style is used.
		/// Default value is null.
		/// </value>
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets default style for the node.")]
		public ElementStyle NodeStyle
		{
			get {return m_NodeStyle;}
			set
			{
				if(m_NodeStyle!=value)
				{
					if(m_NodeStyle!=null)
						m_NodeStyle.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_NodeStyle=value;
					this.OnCellStyleChanged();
				}
			}
		}
		/// <summary>
		/// Gets or sets style for the node when node is selected. Note that this style is applied to the default node style.
		/// </summary>
		/// <value>
		/// Reference to the style assigned or null value indicating that no style is used.
		/// Default value is null.
		/// </value>
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets style of the node when node is selected.")]
		public ElementStyle NodeStyleSelected
		{
			get {return m_NodeStyleSelected;}
			set
			{
				if(m_NodeStyleSelected!=value)
				{
					if(m_NodeStyleSelected!=null)
						m_NodeStyleSelected.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_NodeStyleSelected=value;
					this.OnCellStyleChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets style for the node when mouse is over node. Note that this style is applied to the default node style.
		/// </summary>
		/// <value>
		/// Reference to the style assigned or null value indicating that no style is used.
		/// Default value is null.
		/// </value>
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets style of the node when mouse is over node.")]
		public ElementStyle NodeStyleMouseOver
		{
			get {return m_NodeStyleMouseOver;}
			set
			{
				if(m_NodeStyleMouseOver!=value)
				{
					if(m_NodeStyleMouseOver!=null)
						m_NodeStyleMouseOver.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_NodeStyleMouseOver=value;
					this.OnCellStyleChanged();
				}
			}
		}


		/// <summary>
		/// Gets the collection of tree nodes that are assigned to the tree view control.
		/// </summary>
		/// <value>
		/// A <see cref="NodeCollection">NodeCollection</see> that represents the tree nodes
		/// assigned to the tree control.
		/// </value>
		/// <remarks>
		/// 	<para>The Nodes property holds a collection of Node objects, each of which has a
		///     Nodes property that can contain its own NodeCollection.</para>
		/// </remarks>
		[Browsable(true),DesignerSerializationVisibility(DesignerSerializationVisibility.Content),Description("Gets the collection of tree nodes that are assigned to the tree control.")]
		public NodeCollection Nodes
		{
			get {return m_Nodes;}
		}

        private int _NodeSpacing = 3;
        /// <summary>
        /// Gets or sets the vertical spacing between nodes in pixels. Default value is 3.
        /// </summary>
        [DefaultValue(3), Description("Indicates vertical spacing between nodes in pixels."), Category("Layout")]
        public int NodeSpacing
        {
            get { return _NodeSpacing; }
            set
            {
                _NodeSpacing = value;
                _LayoutSettings.NodeVerticalSpacing = value;
                this.InvalidateNodesSize();
                this.RecalcLayout();
            }
        }
        /// <summary>
        /// Gets or sets the horizontal spacing between nodes in pixels when control is in Tile layout. Default value is 4.
        /// </summary>
        [DefaultValue(4), Description("Indicates horizontal spacing between nodes in pixels when control is in Tile layout."), Category("Layout")]
        public int NodeHorizontalSpacing
        {
            get { return _LayoutSettings.NodeHorizontalSpacing; }
            set
            {
                if (value != _LayoutSettings.NodeHorizontalSpacing)
                {
                    _LayoutSettings.NodeHorizontalSpacing = value;
                    this.InvalidateNodesSize();
                    this.RecalcLayout();
                }
            }
        }

        private bool _GridRowLines = false;
        /// <summary>
        /// Gets or sets whether horizontal grid lines between each row are displayed. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Columns"), Description("")]
        public bool GridRowLines
        {
            get { return _GridRowLines; }
            set
            {
                if (_GridRowLines != value)
                {
                    _GridRowLines = value;
                    this.Invalidate();
                }
            }
        }

        private bool _GridColumnLineResizeEnabled = false;
        /// <summary>
        /// Gets or sets whether column can be resized when mouse is over the column grid line and outside of the column header.
        /// GridColumnLines must be set to true to make column lines visible.
        /// </summary>
        [DefaultValue(false), Category("Columns"), Description("Indicates whether column can be resized when mouse is over the column grid line and outside of the column header.")]
        public bool GridColumnLineResizeEnabled
        {
            get { return _GridColumnLineResizeEnabled; }
            set
            {
                _GridColumnLineResizeEnabled = value;
            }
        }

        private bool _GridLines = true;
        /// <summary>
        /// Gets or sets whether grid lines are displayed when columns are defined. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Columns"), Description("Indicates whether grid lines are displayed when columns are defined.")]
        public bool GridColumnLines
        {
            get { return _GridLines; }
            set
            {
                if (_GridLines != value)
                {
                    _GridLines = value;
                    if (m_Columns.Count > 0 && _ColumnsVisible)
                        this.Invalidate();
                }
            }
        }

        private Color _GridLinesColor = Color.Empty;
        /// <summary>
        /// Gets or sets the grid lines color.
        /// </summary>
        [Category("Columns"), Description("Indicates grid lines color.")]
        public Color GridLinesColor
        {
            get { return _GridLinesColor; }
            set { _GridLinesColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeGridLinesColor()
        {
            return !_GridLinesColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetGridLinesColor()
        {
            this.GridLinesColor = Color.Empty;
        }

        private Color _AlternateRowColor = Color.Empty;
        /// <summary>
        /// Gets or sets the alternate row color applied to every other row. Default value is Color.Empty.
        /// </summary>
        [Category("Columns"), Description("Indicates alternate row color applied to every other row.")]
        public  Color AlternateRowColor
        {
        	get {	return _AlternateRowColor; }
        	set 
            {
                _AlternateRowColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAlternateRowColor()
        {
            return !_AlternateRowColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetAlternateRowColor()
        {
            this.AlternateRowColor = Color.Empty;
        }

        private ElementStyle _NodesColumnsBackgroundStyle = null;
        /// <summary>
        /// Gets or sets the background style for the child nodes columns. Background style defines the appearance of the column header background.
        /// </summary>
        /// <value>
        /// Reference to the style assigned to the column header.
        /// </value>
        [Browsable(true), DefaultValue(null), Category("Column Header Style"), Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates the style class assigned to the child nodes columns.")]
        public ElementStyle NodesColumnsBackgroundStyle
        {
            get { return _NodesColumnsBackgroundStyle; }
            set
            {
                if (_NodesColumnsBackgroundStyle != null)
                    _NodesColumnsBackgroundStyle.StyleChanged -= new EventHandler(this.ElementStyleChanged);
                if (value != null)
                    value.StyleChanged += new EventHandler(this.ElementStyleChanged);
                _NodesColumnsBackgroundStyle = value;
            }
        }

        private ElementStyle _ColumnsBackgroundStyle = null;
        /// <summary>
        /// Gets or sets the background style for the columns. Background style defines the appearance of the column header background.
        /// </summary>
        /// <value>
        /// Reference to the style assigned to the column header.
        /// </value>
        /// <seealso cref="ColumnStyleNormal">ColumnStyleNormal Property</seealso>
        /// <seealso cref="ColumnStyleMouseDown">ColumnStyleMouseDown Property</seealso>
        /// <seealso cref="ColumnStyleMouseOver">ColumnStyleMouseOver Property</seealso>
        [Browsable(true), DefaultValue(null), Category("Column Header Style"), Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates the style class assigned to the column.")]
        public ElementStyle ColumnsBackgroundStyle
        {
            get { return _ColumnsBackgroundStyle; }
            set
            {
                if (_ColumnsBackgroundStyle != null)
                    _ColumnsBackgroundStyle.StyleChanged -= new EventHandler(this.ElementStyleChanged);
                if (value != null)
                    value.StyleChanged += new EventHandler(this.ElementStyleChanged);
                _ColumnsBackgroundStyle = value;
            }
        }

		/// <summary>
		/// Gets or sets the default style class assigned to the column headers.
		/// </summary>
		/// <value>
		/// Reference to the style assigned to the column header.
		/// </value>
		/// <remarks>
		/// When style is not set on ColumnHeader objects then style setting from this property is used instead.
		/// </remarks>
		/// <seealso cref="ColumnStyleMouseDown">ColumnStyleMouseDown Property</seealso>
		/// <seealso cref="ColumnStyleMouseOver">ColumnStyleMouseOver Property</seealso>
		[Browsable(true),DefaultValue(null),Category("Column Header Style"),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the column.")]
		public ElementStyle ColumnStyleNormal
		{
			get {return m_ColumnStyleNormal;}
			set
			{
				if(m_ColumnStyleNormal!=null)
					m_ColumnStyleNormal.StyleChanged-=new EventHandler(this.ElementStyleChanged);
				if(value!=null)
					value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
				m_ColumnStyleNormal=value;
			}
		}

		/// <summary>
		/// Gets or sets default style class assigned to the column which is applied when mouse
		/// button is pressed over the header.
		/// </summary>
		/// <value>
		/// Name of the style assigned to the column.
		/// </value>
		/// <remarks>
		/// When style is not set on ColumnHeader objects then style setting from this property is used instead.
		/// </remarks>
		/// <seealso cref="ColumnStyleNormal">ColumnStyleNormal Property</seealso>
		/// <seealso cref="ColumnStyleMouseOver">ColumnStyleMouseOver Property</seealso>
		[Browsable(true),DefaultValue(null),Category("Column Header Style"),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the column when mouse is down.")]
		public ElementStyle ColumnStyleMouseDown
		{
			get {return m_ColumnStyleMouseDown;}
			set
			{
				if(m_ColumnStyleMouseDown!=null)
					m_ColumnStyleMouseDown.StyleChanged-=new EventHandler(this.ElementStyleChanged);
				if(value!=null)
					value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
				m_ColumnStyleMouseDown=value;
			}
		}

		/// <summary>
		/// Gets or sets default style class assigned to the column which is applied when mouse is
		/// over the column.
		/// </summary>
		/// <value>
		/// Name of the style assigned to the column.
		/// </value>
		/// <remarks>
		/// When style is not set on ColumnHeader objects then style setting from this property is used instead.
		/// </remarks>
		/// <seealso cref="ColumnStyleNormal">ColumnStyleNormal Property</seealso>
		/// <seealso cref="ColumnStyleMouseDown">ColumnStyleMouseDown Property</seealso>
		[Browsable(true),DefaultValue(null),Category("Column Header Style"),Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the cell when mouse is over the column.")]
		public ElementStyle ColumnStyleMouseOver
		{
			get {return m_ColumnStyleMouseOver;}
			set
			{
				if(m_ColumnStyleMouseOver!=null)
					m_ColumnStyleMouseOver.StyleChanged-=new EventHandler(this.ElementStyleChanged);
				if(value!=null)
					value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
				m_ColumnStyleMouseOver=value;
			}
		}

		/// <summary>
		/// Gets collection that holds definition of column headers associated with nodes.
		/// </summary>
		[Browsable(true),DesignerSerializationVisibility(DesignerSerializationVisibility.Content),Description("Collection that holds definition of column headers associated with nodes.")]
		internal HeadersCollection Headers
		{
			get {return m_Headers;}
		}

		/// <summary>
		/// Gets or sets the tree node that is currently selected in the tree control.
		/// </summary>
		/// <remarks>
		/// 	<para>If no <see cref="Node">Node</see> is currently selected, the
		///     <b>SelectedNode</b> property is a null reference (<b>Nothing</b> in Visual
		///     Basic).</para>
		/// </remarks>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Node SelectedNode
		{
			get 
            {
                if (_MultiSelect && m_SelectedNodes.Count > 0)
                    return m_SelectedNodes[m_SelectedNodes.Count - 1];
                return m_SelectedNode;
            }
			set
			{
				if(m_SelectedNode!=value)
				{
					SelectNode(value,eTreeAction.Code);
				}
			}
		}

        /// <summary>
        /// Gets or sets the collection of currently selected nodes in tree control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NodeCollection SelectedNodes
        {
            get
            {
                return m_SelectedNodes;
            }
        }

        private bool _MultiSelect = false;
        /// <summary>
        /// Gets or sets whether multi-node selection is enabled. Default value is false. When
        /// multi-selection is enabled use SelectedNodes property to retrive collection of selected nodes.
        /// Use MultiSelectRule property to change the multi-node selection rule.
        /// </summary>
        [DefaultValue(false), Category("Selection"), Description("Indicates whether multi-node selection is enabled.")]
        public bool MultiSelect
        {
            get { return _MultiSelect; }
            set
            {
                if (_MultiSelect != value)
                {
                    if (_MultiSelect)
                        m_SelectedNodes.Clear();
                    _MultiSelect = value;
                }
            }
        }

        private eMultiSelectRule _MultiSelectRule = eMultiSelectRule.SameParent;
        /// <summary>
        /// Gets or sets the rule that governs the multiple node selection. Default value indicates that only nodes
        /// belonging to same parent can be multi-selected.
        /// </summary>
        [DefaultValue(eMultiSelectRule.SameParent), Category("Selection"), Description("Indicates rule that governs the multiple node selection.")]
        public eMultiSelectRule MultiSelectRule
        {
            get { return _MultiSelectRule; }
            set { _MultiSelectRule = value; }
        }
        
        /// <summary>
		/// Invalidates node bounds on canvas.
		/// </summary>
		/// <param name="node">Reference node.</param>
        internal void InvalidateNode(Node[] nodes)
        {
            foreach (Node node in nodes)
            {
                InvalidateNode(node);
            }
        }

		/// <summary>
		/// Invalidates node bounds on canvas.
		/// </summary>
		/// <param name="node">Reference node.</param>
		internal void InvalidateNode(Node node)
		{
            if (node == null)
                return;

			Rectangle r;
            if (node == m_DragNode && m_DragNode.Parent == null)
                r = m_DragNode.BoundsRelative;
            else
                r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, m_NodeDisplay.Offset);
            if (node.FullRowBackground)
            {
                r.Width = this.Width;
                r.X = 0;
            }

            if (m_SelectionBox)
            {
                r.Inflate(m_SelectionBoxSize, m_SelectionBoxSize);
                if (_SelectionBoxStyle == eSelectionStyle.FullRowSelect)
                {
                    r.X = this.ClientRectangle.X;
                    r.Width = this.ClientRectangle.Width;
                }
            }

            if (this.NodeStyleMouseOver != null || this.CellStyleMouseOver != null || node.StyleMouseOver != null || this.NodeStyleMouseOver != null || _HotTracking)
                r.Inflate(1, 1);
			
			r = GetScreenRectangle(r);
			
			this.Invalidate(r);
		}

        /// <summary>
        /// Finds the node based on the Node.Name property.
        /// </summary>
        /// <param name="name">Name of the node to find.</param>
        /// <returns>Reference to a node with given name or null if node cannot be found.</returns>
        public Node FindNodeByName(string name)
        {
            return NodeOperations.FindNodeByName(this, name);
        }

        /// <summary>
        /// Finds the node based on the Node.DataKey property.
        /// </summary>
        /// <param name="name">Data key to look for.</param>
        /// <returns>Reference to a node with given key or null if node cannot be found.</returns>
        public Node FindNodeByDataKey(object key)
        {
            return NodeOperations.FindNodeByDataKey(this, key);
        }

        /// <summary>
        /// Finds the node based on the Node.BindingIndex property.
        /// </summary>
        /// <param name="bindingIndex">Index to look for.</param>
        /// <returns>Reference to a node with given key or null if node cannot be found.</returns>
        public Node FindNodeByBindingIndex(int bindingIndex)
        {
            return NodeOperations.FindNodeByBindingIndex(this, bindingIndex);
        }


        /// <summary>
        /// Finds the first node that starts with the specified text. Node.Text property is searched.
        /// </summary>
        /// <param name="text">Partial text to look for</param>
        /// <returns>Reference to a node or null if no node is found.</returns>
        public Node FindNodeByText(string text)
        {
            return NodeOperations.FindNodeByText(this, text, null, true);
        }

        /// <summary>
        /// Finds the first node that starts with the specified text. Node.Text property is searched.
        /// </summary>
        /// <param name="text">Partial text to look for</param>
        /// <param name="ignoreCase">Controls whether case insensitive search is performed</param>
        /// <returns>Reference to a node or null if no node is found.</returns>
        public Node FindNodeByText(string text, bool ignoreCase)
        {
            return NodeOperations.FindNodeByText(this, text, null, ignoreCase);
        }

        /// <summary>
        /// Finds the first node that starts with the specified text. Node.Text property is searched.
        /// </summary>
        /// <param name="text">Partial text to look for</param>
        /// <param name="startFromNode">Reference node to start searching from</param>
        /// <param name="ignoreCase">Controls whether case insensitive search is performed</param>
        /// <returns>Reference to a node or null if no node is found.</returns>
        public Node FindNodeByText(string text, Node startFromNode, bool ignoreCase)
        {
            return NodeOperations.FindNodeByText(this, text, startFromNode, ignoreCase);
        }

		/// <summary>
		/// Returns reference to node layout object.
		/// </summary>
		internal Layout.NodeLayout NodeLayout
		{
			get {return m_NodeLayout;}
		}

		/// <summary>
		/// Returns reference to node display object.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public NodeDisplay NodeDisplay
		{
			get {return m_NodeDisplay;}
		}

		/// <summary>
		/// Gets whether layout is suspended for tree control. Layout is suspended after
		/// call to <see cref="BeginUpdate">BeginUpdate</see> method and it is resumed after the
		/// call to <see cref="EndUpdate">EndUpdate</see> method.
		/// </summary>
		[Browsable(false)]
		public bool IsUpdateSuspended
		{
			get {return (m_UpdateSuspended>0);}
		}
		
		internal void SetPendingLayout()
		{
			m_PendingLayout = true;
		}

        /// <summary>
        /// Gets whether control has layout operation pending on next paint or update.
        /// </summary>
        [Browsable(false)]
        public bool IsLayoutPending
        {
            get { return m_PendingLayout; }
        }
        

		/// <summary>
		/// Gets or sets whether paint operations are suspended for the control. You should use this method
		/// if you need the RecalcLayout operations to proceed but you want to stop painting of the control.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SuspendPaint
		{
			get {return m_SuspendPaint;}
			set {m_SuspendPaint=value;}
		}

		/// <summary>
		/// Gets or sets the ImageList that contains the Image objects used by the tree nodes.
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Images"),Description("Indicates the ImageList that contains the Image objects used by the tree nodes.")]
		public ImageList ImageList
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
		/// Gets or sets the image-list index value of the default image that is displayed by the tree nodes.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates the image-list index value of the default image that is displayed by the tree nodes."),Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),TypeConverter(typeof(ImageIndexConverter)),DefaultValue(-1)]
		public int ImageIndex
		{
			get {return m_ImageIndex;}
			set
			{
				if(m_ImageIndex!=value)
				{
					m_ImageIndex=value;
				}
			}
		}

        private Image _CheckBoxImageChecked = null;
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box in cell is checked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box in cell is checked")]
        public Image CheckBoxImageChecked
        {
            get { return _CheckBoxImageChecked; }
            set 
            { 
                _CheckBoxImageChecked = value;
                OnCheckBoxImageChanged();
            }
        }
        private Image _CheckBoxImageUnChecked = null;
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box in cell is unchecked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box in cell is unchecked")]
        public Image CheckBoxImageUnChecked
        {
            get { return _CheckBoxImageUnChecked; }
            set
            {
                _CheckBoxImageUnChecked = value;
                OnCheckBoxImageChanged();
            }
        }
        private Image _CheckBoxImageIndeterminate = null;
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box in cell is in indeterminate state.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box in cell is in indeterminate state")]
        public Image CheckBoxImageIndeterminate
        {
            get { return _CheckBoxImageIndeterminate; }
            set
            {
                _CheckBoxImageIndeterminate = value;
                OnCheckBoxImageChanged();
            }
        }
        private void OnCheckBoxImageChanged()
        {
            InvalidateNodesSize();
            SetPendingLayout();
            Invalidate();
        }

        ///// <summary>
        ///// Gets or sets the NodeConnector object that describes the type of the connector used for
        ///// displaying connection between root node and it's nested nodes. The root node is defined as
        ///// the top level node i.e. which belongs directly to AdvTree.Nodes collection. Default value is null.
        ///// </summary>
        ///// <remarks>
        ///// You can use
        ///// <a href="AdvTree~DevComponents.AdvTree.Node~ParentConnector.html">Node.ParentConnector</a>
        ///// property to specify per node connectors.
        ///// </remarks>
        //[Browsable(true),Category("Connectors"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.NodeConnectorTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the root node connector.")]
        //public NodeConnector RootConnector
        //{
        //    get {return m_RootConnector;}
        //    set
        //    {
        //        if(m_RootConnector!=value)
        //        {
        //            if(m_RootConnector!=null)
        //                m_RootConnector.AppearanceChanged-=new EventHandler(this.ConnectorAppearanceChanged);
        //            if(value!=null)
        //                value.AppearanceChanged+=new EventHandler(this.ConnectorAppearanceChanged);
        //            m_RootConnector=value;
        //            this.OnDisplayChanged();
        //        }
        //    }
        //}

		/// <summary>
		/// Gets or sets the NodeConnector object that describes the type of the connector used for
		/// displaying connection between nested nodes. RootConnector property specifies the connector
		/// between root node and it's imidate nested nodes. This property specifies connector for all other nested levels.
		/// Default value is null.
		/// </summary>
		/// <remarks>
		/// You can use
		/// <a href="AdvTree~DevComponents.AdvTree.Node~ParentConnector.html">Node.ParentConnector</a>
		/// property to specify per node connectors.
		/// </remarks>
        [Browsable(true), Category("Connectors"), DefaultValue(null), Editor("DevComponents.AdvTree.Design.NodeConnectorTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Description("Indicates the nested nodes connector.")]
		public NodeConnector NodesConnector
		{
			get {return m_NodesConnector;}
			set
			{
				if(m_NodesConnector!=value)
				{
					if(m_NodesConnector!=null)
						m_NodesConnector.AppearanceChanged-=new EventHandler(this.ConnectorAppearanceChanged);
					if(value!=null)
						value.AppearanceChanged+=new EventHandler(this.ConnectorAppearanceChanged);
					m_NodesConnector=value;
					this.OnDisplayChanged();
				}
			}
		}

        ///// <summary>
        ///// Gets or sets the NodeConnector object that describes the type of the connector used for
        ///// displaying connection between linked nodes. This property specifies connector for all linked nodes.
        ///// Default value is null.
        ///// </summary>
        //[Browsable(true),Category("Connectors"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.NodeConnectorTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the linked nodes connector.")]
        //public NodeConnector LinkConnector
        //{
        //    get {return m_LinkConnector;}
        //    set
        //    {
        //        if(m_LinkConnector!=value)
        //        {
        //            if(m_LinkConnector!=null)
        //                m_LinkConnector.AppearanceChanged-=new EventHandler(this.ConnectorAppearanceChanged);
        //            if(value!=null)
        //                value.AppearanceChanged+=new EventHandler(this.ConnectorAppearanceChanged);
        //            m_LinkConnector=value;
        //            this.OnDisplayChanged();
        //        }
        //    }
        //}

		/// <summary>
		/// Gets or sets the NodeConnector object that describes the type of the connector used for
		/// displaying connection between linked nodes. Connector specified here is used to display the connection
		/// between nodes that are on the path to the selected node. When set you can use it to visually indicate the path to the currently selected node.
		/// Default value is null.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false),Category("Connectors"),DefaultValue(null),Editor("DevComponents.AdvTree.Design.NodeConnectorTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the linked nodes connector.")]
		public NodeConnector SelectedPathConnector
		{
			get {return m_SelectedPathConnector;}
			set
			{
				if(m_SelectedPathConnector!=value)
				{
					if(m_SelectedPathConnector!=null)
						m_SelectedPathConnector.AppearanceChanged-=new EventHandler(this.ConnectorAppearanceChanged);
					if(value!=null)
						value.AppearanceChanged+=new EventHandler(this.ConnectorAppearanceChanged);
					m_SelectedPathConnector=value;
					this.OnDisplayChanged();
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the layout of the cells inside the node. Default value is Horizontal layout which
		/// means that cell are positioned horizontally next to each other.
		/// </summary>
		/// <remarks>
		/// You can specify cell layout on each node by using
		/// <a href="AdvTree~DevComponents.AdvTree.Node~CellLayout.html">Node.CellLayout</a>
		/// property.
		/// </remarks>
		[Browsable(true),DefaultValue(eCellLayout.Default),Category("Appearance"),Description("Indicates layout of the cells inside the node.")]
		public eCellLayout CellLayout
		{
			get {return m_CellLayout;}
			set
			{
				m_CellLayout=value;
                InvalidateNodesSize();
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the layout of the cells inside the node. Default value is Horizontal layout which
		/// means that cell are positioned horizontally next to each other.
		/// </summary>
		/// <remarks>
		/// You can specify cell layout on each node by using
		/// <a href="AdvTree~DevComponents.AdvTree.Node~CellLayout.html">Node.CellLayout</a>
		/// property.
		/// </remarks>
		[Browsable(true),DefaultValue(eCellPartLayout.Default),Category("Appearance"),Description("Indicates layout of the cells inside the node.")]
		public eCellPartLayout CellPartLayout
		{
			get {return m_CellPartLayout;}
			set
			{
				m_CellPartLayout=value;
                InvalidateNodesSize();
                this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the color scheme style. Color scheme provides predefined colors based on popular visual styles.
		/// We recommend that you use "SchemePart" color settings since they maintain consistant look that is
		/// based on target system color scheme setting.
		/// </summary>
		[Browsable(true),DefaultValue(eColorSchemeStyle.Office2007),Category("Appearance"),Description("Indicates the color scheme style.")]
		public eColorSchemeStyle ColorSchemeStyle
		{
            get { return MapStyle(m_ColorScheme.Style); }
			set
			{
                m_ColorScheme.Style = MapStyle(value);
				if(this.DesignMode)
					this.Refresh();
			}
		}

        internal static eDotNetBarStyle MapStyle(eColorSchemeStyle style)
        {
            if (style == eColorSchemeStyle.Office2003)
                return eDotNetBarStyle.Office2003;
            else if (style == eColorSchemeStyle.VS2005)
                return eDotNetBarStyle.VS2005;
            return eDotNetBarStyle.Office2007;
        }
        internal static eColorSchemeStyle MapStyle(eDotNetBarStyle style)
        {
            if (style == eDotNetBarStyle.Office2003)
                return eColorSchemeStyle.Office2003;
            else if (style == eDotNetBarStyle.VS2005)
                return eColorSchemeStyle.VS2005;
            
            return eColorSchemeStyle.Office2007;
        }

		/// <summary>
		/// Gets the reference to the color scheme object.
		/// </summary>
		internal ColorScheme ColorScheme
		{
			get 
            {
                if (ColorSchemeStyle == eColorSchemeStyle.Office2007 && DevComponents.DotNetBar.Rendering.GlobalManager.Renderer is DevComponents.DotNetBar.Rendering.Office2007Renderer)
                {
                    return ((DevComponents.DotNetBar.Rendering.Office2007Renderer)DevComponents.DotNetBar.Rendering.GlobalManager.Renderer).ColorTable.LegacyColors;
                }
                return m_ColorScheme;
            }
		}

		/// <summary>
		/// Gets or sets whether the content of the control is centered within the bounds of control. Default value is true.
		/// </summary>
		[Browsable(true),DefaultValue(true),Description("Indicates whether the content of the control is centered within the bounds of control."),Category("Layout")]
		internal bool CenterContent
		{
			get {return m_CenterContent;}
			set
			{
				m_CenterContent=value;
				OnLayoutChanged();
			}
		}

        private bool _SelectionPerCell = false;
        /// <summary>
        /// Gets or sets whether per cell selection mode is enabled. In cell selection mode the selection box is drawn over selected cell only
        /// instead of all cells in the node.
        /// Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Selection"), Description("Indicates whether selection is drawn for selected cell only instead of all cells inside of node")]
        public bool SelectionPerCell
        {
            get { return _SelectionPerCell; }
            set
            {
                if (_SelectionPerCell != value)
                {
                    _SelectionPerCell = value;
                    if (this.SelectedNode != null)
                    {
                        InvalidateNode(this.SelectedNode);
                    }
                }
            }
        }

        private bool _SelectionFocusAware = true;
        /// <summary>
        /// Gets or sets whether selection appearance changes depending on whether control has input focus. Default value is true. Setting this value to false causes selection box to be rendered as if control has focus all the time.
        /// </summary>
        [DefaultValue(true), Category("Selection"), Description("Indicates whether selection appearance changes depending on whether control has input focus.")]
        public bool SelectionFocusAware
        {
            get { return _SelectionFocusAware; }
            set
            {
                if (_SelectionFocusAware != value)
                {
                    _SelectionFocusAware = value;
                    this.Invalidate();
                }
            }
        }
        

        private eSelectionStyle _SelectionBoxStyle = eSelectionStyle.HighlightCells;
        /// <summary>
        /// Gets or sets the node selection box style.
        /// </summary>
        /// <seealso cref="SelectionBox">SelectionBox Property</seealso>
        /// <seealso cref="SelectionBoxSize">SelectionBoxSize Property</seealso>
        /// <seealso cref="SelectionBoxFillColor">SelectionBoxFillColor Property</seealso>
        /// <seealso cref="SelectionBoxBorderColor">SelectionBoxBorderColor Property</seealso>
        [DefaultValue(eSelectionStyle.HighlightCells), Category("Selection"), Description("Indicates node selection box style.")]
        public eSelectionStyle SelectionBoxStyle
        {
            get { return _SelectionBoxStyle; }
            set { _SelectionBoxStyle = value; }
        }

		/// <summary>
		/// Gets or sets the value that indicates whether selection box is drawn around the
		/// selected node. Default value is true. Another way to provide the visual indication that
		/// node is selected is by using selected state style properties like
		/// <a href="AdvTree~DevComponents.AdvTree.AdvTree~NodeStyleSelected.html">NodeStyleSelected</a>
		/// and
		/// <a href="AdvTree~DevComponents.AdvTree.AdvTree~CellStyleSelected.html">CellStyleSelected</a>.
		/// </summary>
		/// <seealso cref="CellStyleSelected">CellStyleSelected Property</seealso>
		/// <seealso cref="NodeStyleSelected">NodeStyleSelected Property</seealso>
		[Browsable(true),DefaultValue(true),Category("Selection"),Description("Indicates whether selection box is drawn around selected node.")]
		public bool SelectionBox
		{
			get {return m_SelectionBox;}
			set
			{
				if(m_SelectionBox!=value)
				{
					m_SelectionBox=value;
					this.OnSelectionBoxChanged();
				}
			}
		}

        /// <summary>
        /// Gets or sets the size/thickness in pixel of the selection box drawn around selected
        /// node.
        /// </summary>
        [Browsable(true), DefaultValue(4), Category("Selection"), Description("Indicates the size in pixels of the selection box drawn around selected node.")]
        internal int SelectionBoxSize
        {
            get { return m_SelectionBoxSize; }
            set
            {
                if (m_SelectionBoxSize != value)
                {
                    m_SelectionBoxSize = value;
                    this.OnSelectionBoxChanged();
                }
            }
        }

        private bool _FullRowSelect = true;
        /// <summary>
        /// Gets or sets whether node is selected when mouse is pressed anywhere within node vertical bounds. Default value is true.
        /// </summary>
        /// <remarks>
        /// When set to false the node is selected only when mouse is pressed over the node content.
        /// </remarks>
        [DefaultValue(true), Description("Indicates whether node is selected when mouse is pressed anywhere within node vertical bounds."), Category("Selection")]
        public bool FullRowSelect
        {
            get { return _FullRowSelect; }
            set
            {
                _FullRowSelect = value;
            }
        }
        

        ///// <summary>
        ///// Gets or sets the selection box border color.
        ///// </summary>
        //[Browsable(true),Category("Selection"),Description("Indicates the selection box border color.")]
        //public Color SelectionBoxBorderColor
        //{
        //    get {return m_SelectionBoxBorderColor;}
        //    set
        //    {
        //        if(m_SelectionBoxBorderColor!=value)
        //        {
        //            m_SelectionBoxBorderColor=value;
        //            this.OnSelectionBoxChanged();
        //        }
        //    }

        //}
        ///// <summary>
        ///// Indicates whether SelectionBoxBorderColor should be serialized. Used by windows forms designer design-time support.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeSelectionBoxBorderColor()
        //{return (m_SelectionBoxBorderColor!=GetDefaultSelectionBoxBorderColor());}
        ///// <summary>
        ///// Resets SelectionBoxBorderColor to it's default value. Used by windows forms designer design-time support.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetSelectionBoxBorderColor()
        //{
        //    m_SelectionBoxBorderColor=GetDefaultSelectionBoxBorderColor();
        //}

        ///// <summary>
        ///// Gets or sets the selection box fill color.
        ///// </summary>
        //[Browsable(true),Category("Selection"),Description("Indicates the selection box fill color.")]
        //public Color SelectionBoxFillColor
        //{
        //    get {return m_SelectionBoxFillColor;}
        //    set
        //    {
        //        if(m_SelectionBoxFillColor!=value)
        //        {
        //            m_SelectionBoxFillColor=value;
        //            this.OnSelectionBoxChanged();
        //        }
        //    }

        //}
        ///// <summary>
        ///// Indicates whether SelectionBoxFillColor should be serialized. Used by windows forms designer design-time support.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeSelectionBoxFillColor()
        //{return (m_SelectionBoxFillColor!=GetDefaultSelectionBoxFillColor());}
        ///// <summary>
        ///// Resets SelectionBoxFillColor to it's default value. Used by windows forms designer design-time support.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetSelectionBoxFillColor()
        //{
        //    m_SelectionBoxFillColor=GetDefaultSelectionBoxFillColor();
        //}

        private int _ExpandWidth = 24;
        /// <summary>
        /// Gets or sets the total node expand area width in pixels. The expand button with ExpandButtonSize is fitted into this area. Default value is 24. 
        /// </summary>
        [DefaultValue(24), Category("Expand Button"), Description("Indicates total node expand area width in pixels.")]
        public int ExpandWidth
        {
            get { return _ExpandWidth; }
            set
            {
                if (_ExpandWidth != value)
                {
                    _ExpandWidth = value;
                    _LayoutSettings.ExpandAreaWidth = value;
                    this.InvalidateNodesSize();
                    this.Invalidate();
                }
            }
        }
        

		/// <summary>
		/// Gets or sets the size of the expand button that is used to expand/collapse node. Default value is 8,8.
		/// </summary>
		[Browsable(true),Category("Expand Button"),Description("Indicates size of the expand button that is used to expand/collapse node.")]
		public Size ExpandButtonSize
		{
			get {return m_ExpandButtonSize;}
			set
			{
                if (m_ExpandButtonSize != value)
                {
                    m_ExpandButtonSize = value;
                    this.OnExpandButtonChanged();
                }
			}
		}
		/// <summary>
		/// Indicates whether SelectionBoxFillColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandButtonSize()
		{return (m_ExpandButtonSize!=GetDefaultExpandButtonSize());}
		/// <summary>
		/// Resets SelectionBoxFillColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetExpandButtonSize()
		{
			this.ExpandButtonSize=GetDefaultExpandButtonSize();
		}

		/// <summary>
		/// Gets or sets expand button border color. Note that setting ExpandBorderColorSchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Expand Button"),Description("Indicates expand button border color.")]
		public Color ExpandBorderColor
		{
			get {return m_ExpandBorderColor;}
			set
			{
				m_ExpandBorderColor=value;
				this.OnExpandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether ExpandBorderColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandBorderColor()
		{return (!m_ExpandBorderColor.IsEmpty && m_ExpandBorderColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets ExpandBorderColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetExpandBorderColor()
		{
			this.ExpandBorderColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets expand button color scheme border color. Setting
		/// this property overrides the setting of the corresponding ExpandBorderColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through ExpandBorderColor property.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(eColorSchemePart.None),Description("Indicates expand button border color.")]
		public eColorSchemePart ExpandBorderColorSchemePart
		{
			get {return m_ExpandBorderColorSchemePart;}
			set
			{
				m_ExpandBorderColorSchemePart=value;
				this.OnExpandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets expand button back color. Note that setting ExpandBackColorSchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Expand Button"),Description("Indicates expand button back color.")]
		public Color ExpandBackColor
		{
			get {return m_ExpandBackColor;}
			set
			{
				m_ExpandBackColor=value;
				this.OnExpandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether ExpandBackColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandBackColor()
        { return (!m_ExpandBackColor.IsEmpty && m_ExpandBackColorSchemePart == eColorSchemePart.None); }
		/// <summary>
		/// Resets ExpandBackColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetExpandBackColor()
		{
			this.ExpandBackColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets expand button color scheme back color. Setting
		/// this property overrides the setting of the corresponding ExpandBackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through ExpandBackColor property.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(eColorSchemePart.None),Description("Indicates expand button back color.")]
		public eColorSchemePart ExpandBackColorSchemePart
		{
			get {return m_ExpandBackColorSchemePart;}
			set
			{
				m_ExpandBackColorSchemePart=value;
				this.OnExpandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets expand button target gradientback color. Note that setting ExpandBackColor2SchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Expand Button"),Description("Indicates expand button target gradient back color.")]
		public Color ExpandBackColor2
		{
			get {return m_ExpandBackColor2;}
			set
			{
				m_ExpandBackColor2=value;
				this.OnExpandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether ExpandBackColor2 should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandBackColor2()
		{return (!m_ExpandBackColor2.IsEmpty && m_ExpandBackColor2SchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets ExpandBackColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetExpandBackColor2()
		{
			this.ExpandBackColor2=Color.Empty;
		}
		/// <summary>
		/// Gets or sets expand button color scheme target gradient back color. Setting
		/// this property overrides the setting of the corresponding ExpandBackColor2 property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through ExpandBackColor2 property.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(eColorSchemePart.None),Description("Indicates expand button target gradient back color.")]
		public eColorSchemePart ExpandBackColor2SchemePart
		{
			get {return m_ExpandBackColor2SchemePart;}
			set
			{
				m_ExpandBackColor2SchemePart=value;
				this.OnExpandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets expand button line color. Note that setting ExpandLineColorSchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Expand Button"),Description("Indicates expand button line color.")]
		public Color ExpandLineColor
		{
			get {return m_ExpandLineColor;}
			set
			{
				m_ExpandLineColor=value;
				this.OnExpandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether ExpandLineColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandLineColor()
		{return (!m_ExpandLineColor.IsEmpty && m_ExpandLineColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets ExpandLineColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetExpandLineColor()
		{
			this.ExpandLineColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets expand button color scheme line color. Setting
		/// this property overrides the setting of the corresponding ExpandLineColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through ExpandLineColor property.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(eColorSchemePart.None),Description("Indicates expand button line color.")]
		public eColorSchemePart ExpandLineColorSchemePart
		{
			get {return m_ExpandLineColorSchemePart;}
			set
			{
				m_ExpandLineColorSchemePart=value;
				this.OnExpandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets the expand button background gradient angle.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(0),Description("Indicates expand button background gradient angle.")]
		public int ExpandBackColorGradientAngle
		{
			get
			{
				return m_ExpandBackColorGradientAngle;
			}
			set
			{
				if(m_ExpandBackColorGradientAngle!=value)
				{
					m_ExpandBackColorGradientAngle=value;
					this.OnExpandButtonChanged();

				}
			}
		}

		/// <summary>
		/// Gets or sets the expand button image which is used to indicate that node will be expanded. To use images as expand buttons you also need to set ExpandButtonType=eExpandButtonType.Image.
		/// </summary>
		[Browsable(true),DefaultValue(null),Description("Indicates expand button image which is used to indicate that node will be expanded."),Category("Expand Button")]
		public Image ExpandImage
		{
			get {return m_ExpandImage;}
			set
			{
				if(m_ExpandImage!=value)
				{
					m_ExpandImage=value;
					if(m_ExpandButtonType==eExpandButtonType.Image)
						this.OnExpandButtonChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the expand button image which is used to indicate that node will be collapsed. To use images as expand buttons you also need to set ExpandButtonType=eExpandButtonType.Image.
		/// </summary>
		[Browsable(true),DefaultValue(null),Description("Indicates expand button image which is used to indicate that node will be collapsed."),Category("Expand Button")]
		public Image ExpandImageCollapse
		{
			get {return m_ExpandImageCollapse;}
			set
			{
				if(m_ExpandImageCollapse!=value)
				{
					m_ExpandImageCollapse=value;
					if(m_ExpandButtonType==eExpandButtonType.Image)
						this.OnExpandButtonChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of the expand button used to expand/collapse nodes.
		/// </summary>
		[Browsable(true),DefaultValue(eExpandButtonType.Rectangle),Category("Expand Button"),Description("Indicates type of the expand button used to expand/collapse nodes.")]
		public eExpandButtonType ExpandButtonType
		{
			get {return m_ExpandButtonType;}
			set
			{
				if(m_ExpandButtonType!=value)
				{
					m_ExpandButtonType=value;
					this.OnExpandButtonChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the display root node. Setting this property allows you to use any
		/// Node as root display node. Default value is Null which means that first node from
		/// AdvTree.Nodes collection is used as display root node.
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Appearance"),Description("Indicates display root node.")]
		public Node DisplayRootNode
		{
			get {return m_DisplayRootNode;}
			set
			{
				if(m_DisplayRootNode!=value)
				{
					m_DisplayRootNode=value;
                    if (m_NodeDisplay != null) m_NodeDisplay.PaintedNodes.Clear();
					InvalidateNodesSize();
					this.RecalcLayout();
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the width of the command button. Default value is 10 pixels.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(10),Description("Indicates width of the command button.")]
		internal int CommandWidth
		{
			get{return m_CommandWidth;}
			set
			{
				m_CommandWidth=value;
				OnCommandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets command button back color. Note that setting CommandBackColorSchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Command Button"),Description("Indicates command button back color.")]
		internal Color CommandBackColor
		{
			get {return m_CommandBackColor;}
			set
			{
				m_CommandBackColor=value;
				this.OnCommandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether CommandBackColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeCommandBackColor()
		{return (!m_CommandBackColor.IsEmpty && m_CommandBackColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandBackColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal void ResetCommandBackColor()
		{
			this.CommandBackColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme back color. Setting
		/// this property overrides the setting of the corresponding CommandBackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandBackColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.CustomizeBackground),Description("Indicates command button back color.")]
		internal eColorSchemePart CommandBackColorSchemePart
		{
			get {return m_CommandBackColorSchemePart;}
			set
			{
				m_CommandBackColorSchemePart=value;
				this.OnCommandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets command button target gradient back color. Note that setting CommandBackColor2SchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Command Button"),Description("Indicates command button target gradient back color.")]
		internal Color CommandBackColor2
		{
			get {return m_CommandBackColor2;}
			set
			{
				m_CommandBackColor2=value;
				this.OnCommandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether CommandBackColor2 should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeCommandBackColor2()
		{return (!m_CommandBackColor2.IsEmpty && m_CommandBackColor2SchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandBackColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal void ResetCommandBackColor2()
		{
			this.CommandBackColor2=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme target gradient back color. Setting
		/// this property overrides the setting of the corresponding CommandBackColor2 property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandBackColor2 property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.CustomizeBackground2),Description("Indicates command button target gradient back color.")]
		internal eColorSchemePart CommandBackColor2SchemePart
		{
			get {return m_CommandBackColor2SchemePart;}
			set
			{
				m_CommandBackColor2SchemePart=value;
				this.OnCommandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets command button foreground color. Note that setting CommandForeColorSchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Command Button"),Description("Indicates command button fore color.")]
		internal Color CommandForeColor
		{
			get {return m_CommandForeColor;}
			set
			{
				m_CommandForeColor=value;
				this.OnCommandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether CommandForeColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeCommandForeColor()
		{return (!m_CommandForeColor.IsEmpty && m_CommandForeColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandForeColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal void ResetCommandForeColor()
		{
			this.CommandForeColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme foreground color. Setting
		/// this property overrides the setting of the corresponding CommandForeColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandForeColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.CustomizeText),Description("Indicates command button foreground color.")]
		internal eColorSchemePart CommandForeColorSchemePart
		{
			get {return m_CommandForeColorSchemePart;}
			set
			{
				m_CommandForeColorSchemePart=value;
				this.OnCommandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets the command button background gradient angle.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(0),Description("Indicates command button background gradient angle.")]
		internal int CommandBackColorGradientAngle
		{
			get
			{
				return m_CommandBackColorGradientAngle;
			}
			set
			{
				if(m_CommandBackColorGradientAngle!=value)
				{
					m_CommandBackColorGradientAngle=value;
					this.OnExpandButtonChanged();

				}
			}
		}

		/// <summary>
		/// Gets or sets command button mouse over back color. Note that setting CommandMouseOverBackColorSchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Command Button"),Description("Indicates command button mouse over back color.")]
		internal Color CommandMouseOverBackColor
		{
			get {return m_CommandMouseOverBackColor;}
			set
			{
				m_CommandMouseOverBackColor=value;
				this.OnCommandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether CommandMouseOverBackColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeCommandMouseOverBackColor()
		{return (!m_CommandMouseOverBackColor.IsEmpty && m_CommandMouseOverBackColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandMouseOverBackColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal void ResetCommandMouseOverBackColor()
		{
			this.CommandMouseOverBackColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme mouse over back color. Setting
		/// this property overrides the setting of the corresponding CommandMouseOverBackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandMouseOverBackColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.ItemHotBackground),Description("Indicates command button mouse over back color.")]
		internal eColorSchemePart CommandMouseOverBackColorSchemePart
		{
			get {return m_CommandMouseOverBackColorSchemePart;}
			set
			{
				m_CommandMouseOverBackColorSchemePart=value;
				this.OnCommandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets command button mouse over target gradient back color. Note that setting CommandMouseOverBackColor2SchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Command Button"),Description("Indicates command button mouse over target gradient back color.")]
		internal Color CommandMouseOverBackColor2
		{
			get {return m_CommandMouseOverBackColor2;}
			set
			{
				m_CommandMouseOverBackColor2=value;
				this.OnCommandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether CommandMouseOverBackColor2 should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeCommandMouseOverBackColor2()
		{return (!m_CommandMouseOverBackColor2.IsEmpty && m_CommandMouseOverBackColor2SchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandMouseOverBackColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetCommandMouseOverBackColor2()
		{
			this.CommandMouseOverBackColor2=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button mouse over color scheme target gradient back color. Setting
		/// this property overrides the setting of the corresponding CommandMouseOverBackColor2 property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandMouseOverBackColor2 property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.ItemPressedBackground2),Description("Indicates command button mouse over target gradient back color.")]
        internal eColorSchemePart CommandMouseOverBackColor2SchemePart
		{
			get {return m_CommandMouseOverBackColor2SchemePart;}
			set
			{
				m_CommandMouseOverBackColor2SchemePart=value;
				this.OnCommandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets command button mouse over foreground color. Note that setting CommandMouseOverForeColorSchemePart property will override the value that you set here.
		/// </summary>
		[Browsable(true),Category("Command Button"),Description("Indicates command button mouse over fore color.")]
        internal Color CommandMouseOverForeColor
		{
			get {return m_CommandMouseOverForeColor;}
			set
			{
				m_CommandMouseOverForeColor=value;
				this.OnCommandButtonChanged();
			}
		}
		/// <summary>
		/// Indicates whether CommandMouseOverForeColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializeCommandMouseOverForeColor()
		{return (!m_CommandMouseOverForeColor.IsEmpty && m_CommandMouseOverForeColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandMouseOverForeColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetCommandMouseOverForeColor()
		{
			this.CommandMouseOverForeColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button mouse over color scheme foreground color. Setting
		/// this property overrides the setting of the corresponding CommandMouseOverForeColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="AdvTree~DevComponents.AdvTree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandMouseOverForeColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.ItemHotText),Description("Indicates command button mouse over foreground color.")]
        internal eColorSchemePart CommandMouseOverForeColorSchemePart
		{
			get {return m_CommandMouseOverForeColorSchemePart;}
			set
			{
				m_CommandMouseOverForeColorSchemePart=value;
				this.OnCommandButtonChanged();
			}
		}

		/// <summary>
		/// Gets or sets the command button mouse over background gradient angle.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(0),Description("Indicates command button mouse over background gradient angle.")]
        internal int CommandMouseOverBackColorGradientAngle
		{
			get
			{
				return m_CommandMouseOverBackColorGradientAngle;
			}
			set
			{
				if(m_CommandMouseOverBackColorGradientAngle!=value)
				{
					m_CommandMouseOverBackColorGradientAngle=value;
					this.OnExpandButtonChanged();

				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the label text of the node cells can be edited. Default value is false.
		/// </summary>
		[Browsable(true),DefaultValue(false),Description("Indicates whether the label text of the node cells can be edited."),Category("Editing")]
		public bool CellEdit
		{
			get {return m_CellEdit;}
			set {m_CellEdit=value;}
		}

		/// <summary>
		/// Returns whether cell editing is in progress.
		/// </summary>
		[Browsable(false)]
		public bool IsCellEditing
		{
			get {return m_CellEditing;}
		}

        /// <summary>
        /// Returns the zero based flat index of the node. Flat index is the index of the node as if tree structure
        /// has been flattened into the list.
        /// </summary>
        /// <param name="node">Reference to the node to return index for.</param>
        /// <returns>Zero based node index or -1 if index cannot be determined.</returns>
        public int GetNodeFlatIndex(Node node)
        {
            return NodeOperations.GetNodeFlatIndex(this, node);
        }

        /// <summary>
        /// Returns node based on the flat index. Flat index is the index of the node as if tree structure
        /// has been flattened into the list.
        /// </summary>
        /// <param name="index">Index to return node for.</param>
        /// <returns>Reference to a node or null if node at specified index cannot be found.</returns>
        public Node GetNodeByFlatIndex(int index)
        {
            return NodeOperations.GetNodeByFlatIndex(this, index);
        }
		#endregion

		#region Private implementation
        private bool _AccessibleObjectCreated = false;
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            _AccessibleObjectCreated = true;
            return new AdvTreeAccessibleObject(this);
        }

		/// <summary>
		/// Returns color scheme part color if set otherwise returns color passed in.
		/// </summary>
		/// <param name="color">Color.</param>
		/// <param name="p">Color scheme part.</param>
		/// <returns>Color.</returns>
		internal Color GetColor(Color color, eColorSchemePart p)
		{
            if (p == eColorSchemePart.None || p == eColorSchemePart.Custom)
                return color;
			ColorScheme cs=this.ColorScheme;
			if(cs==null)
				return color;
			return (Color)cs.GetType().GetProperty(p.ToString()).GetValue(cs,null);
		}
		private void OnExpandButtonChanged()
		{
            _LayoutSettings.ExpandPartSize = m_ExpandButtonSize;
			this.InvalidateNodesSize();
            this.RecalcLayout();
			this.Refresh();
		}
		private Size GetDefaultExpandButtonSize()
		{
			return m_DefaultExpandPartSize;
		}
		private void ElementStyleChanged(object sender, EventArgs e)
		{
			this.InvalidateNodesSize();
			if(this.DesignMode)
				this.RecalcLayout();
			this.OnDisplayChanged();
		}
		private void OnDisplayChanged()
		{
			if(this.DesignMode)
				this.Refresh();
		}
        private void OnLayoutChanged()
        {
            _LayoutSettings.NodeHorizontalSpacing = m_NodeHorizontalSpacing;
            _LayoutSettings.NodeVerticalSpacing = m_NodeVerticalSpacing;

            //// Layout specific properties
            //if (m_Layout == eNodeLayout.Diagram)
            //{
            //    Layout.NodeDiagramLayout nd = m_NodeLayout as Layout.NodeDiagramLayout;
            //    nd.DiagramFlow = m_DiagramLayoutFlow;
            //}
            //else if (m_Layout == eNodeLayout.Map)
            //{
            //    NodeMapLayout nd = m_NodeLayout as NodeMapLayout;
            //    nd.MapFlow = m_MapLayoutFlow;
            //}

            this.RecalcLayout();
            OnDisplayChanged();
        }
		private void ConnectorAppearanceChanged(object sender, EventArgs e)
		{
			OnDisplayChanged();
		}
        //private void SetLayout(eNodeLayout layout)
        //{
        //    m_Layout=layout;
        //    if(m_Layout==eNodeLayout.Map)
        //    {
        //        m_NodeLayout=new NodeMapLayout(this,this.ClientRectangle);
        //    }
        //    else if(m_Layout==eNodeLayout.Diagram)
        //    {
        //        m_NodeLayout=new Layout.NodeDiagramLayout(this,this.ClientRectangle);
        //    }
        //    InvalidateNodesSize();
        //    OnLayoutChanged();
        //}

        /// <summary>
        /// Collapses all nodes in a tree.
        /// </summary>
        public void CollapseAll()
        {
            this.BeginUpdate();
            try
            {
                foreach (Node item in this.Nodes)
                {
                    item.CollapseAll();
                }
            }
            finally
            {
                this.EndUpdate(true);
            }
        }

        /// <summary>
        /// Expands all the tree nodes.
        /// </summary>
        public void ExpandAll()
        {
            this.BeginUpdate();
            try
            {
                foreach (Node item in this.Nodes)
                {
                    item.Expand();
                    item.ExpandAll();
                }
            }
            finally
            {
                this.EndUpdate(true);
            }
        }

		private void PaintStyleBackground(Graphics g)
		{
			Display.TreeRenderer renderer = this.NodeRenderer;
			if(renderer!=null)
			{
				this.NodeRenderer.DrawTreeBackground(new TreeBackgroundRendererEventArgs(g, this));
				return;
			}
			
			if(!this.BackColor.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(this.BackColor))
					g.FillRectangle(brush,this.DisplayRectangle);
			}

			ElementStyleDisplayInfo info=new ElementStyleDisplayInfo();
			info.Bounds=this.DisplayRectangle;
			info.Graphics=g;
            bool disposeStyle = false;
            ElementStyle style = ElementStyleDisplay.GetElementStyle(m_BackgroundStyle, out disposeStyle);
			info.Style=style;
			ElementStyleDisplay.Paint(info);
            if (disposeStyle)
                style.Dispose();
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			if(this.SuspendPaint)
				return;

            try
            {
                if (m_PendingLayout)
                {
                    this.RecalcLayout();
                }

                if (this.BackColor.A < 255)
                {
                    base.OnPaintBackground(e);
                }

                Graphics g = e.Graphics;

                PaintStyleBackground(g);
                Point offset = Point.Empty;
                bool setOffset = false;
                if (this.AutoScroll)
                {
                    offset = GetAutoScrollPositionOffset();
                    setOffset = true;
                }

                SmoothingMode sm = g.SmoothingMode;
                TextRenderingHint th = g.TextRenderingHint;
                if (m_AntiAlias)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                }

                Rectangle clipRect = GetInnerRectangle();
                if (_VScrollBar != null) clipRect.Width -= _VScrollBar.Width;
                if (_HScrollBar != null) clipRect.Height -= _HScrollBar.Height;
                g.SetClip(clipRect);

                PaintTree(g, e.ClipRectangle, offset, setOffset, m_ZoomFactor);

                if (_ColumnMoveMarkerIndex >= 0 && _ColumnReorder != null && _ColumnReorder.Parent != m_Columns)
                    DevComponents.AdvTree.Display.ColumnHeaderDisplay.PaintColumnMoveMarker(g, this, _ColumnMoveMarkerIndex, _ColumnReorder.Parent);

                if (m_AntiAlias)
                {
                    g.SmoothingMode = sm;
                    g.TextRenderingHint = th;
                }
            }
            finally
            {
                base.OnPaint(e);
            }
		}

        internal Rectangle GetInnerRectangle()
        {
            Rectangle r = ElementStyleLayout.GetInnerRect(this.BackgroundStyle, this.ClientRectangle); 
            return r;
        }

        internal Point GetAutoScrollPositionOffset()
        {
            Point p = this.AutoScrollPosition;
            if (this.SelectionBoxStyle == eSelectionStyle.NodeMarker)
            {
                p.Y += this.SelectionBoxSize;
                p.X += this.SelectionBoxSize;
            }
            return p;
        }
		
		private void PaintTree(Graphics g, Rectangle clipRectangle, Point offset, bool setOffset, float zoomFactor)
		{
			
#if TRIAL
			if(NodeOperations.ColorExpAlt())
			{
				StringFormat format=new StringFormat(StringFormat.GenericDefault);
				format.Alignment=StringAlignment.Center;
				format.FormatFlags=format.FormatFlags & ~(format.FormatFlags & StringFormatFlags.NoWrap);	
				g.DrawString("Thank you very much for trying AdvTree. Unfortunately your trial period is over. To continue using AdvTree you should purchase license at http://www.devcomponents.com",new Font(this.Font.FontFamily,12),SystemBrushes.Highlight,this.ClientRectangle,format);
				format.Dispose();
				return;
			}
#else
			if(NodeOperations.keyValidated2!=114)
			{
				Font trial=new Font(this.Font.FontFamily,7);
				SolidBrush brushTrial = new SolidBrush(Color.FromArgb(200, SystemColors.Highlight));
				StringFormat format=new StringFormat(StringFormat.GenericDefault);
				format.Alignment=StringAlignment.Center;
				g.DrawString("DotNetBar AdvTree license not found. Please purchase license at http://www.devcomponents.com",
					trial, brushTrial, this.DisplayRectangle.X+this.DisplayRectangle.Width/2, this.DisplayRectangle.Bottom - 14, format);
				brushTrial.Dispose();
				format.Dispose();
				trial.Dispose();
			}
#endif
				
			//Creates the drawing matrix with the right zoom;
            if (zoomFactor != 1)
            {
                System.Drawing.Drawing2D.Matrix mx = GetTranslationMatrix(zoomFactor);
                //use it for drawing
                g.Transform = mx;

                // Translate ClipRectangle
                clipRectangle = GetLayoutRectangle(clipRectangle);
            }

			if(setOffset)
			{
				m_NodeDisplay.Offset = offset;
			}
			m_NodeDisplay.Paint(g,clipRectangle);
			
#if TRIAL
			Font trial=new Font(this.Font.FontFamily,7);
			SolidBrush brushTrial = new SolidBrush(Color.FromArgb(220, SystemColors.Highlight));
			StringFormat formatTrial=new StringFormat(StringFormat.GenericDefault);
			formatTrial.Alignment=StringAlignment.Center;
			g.DrawString("Thank you for trying AdvTree. Please purchase license at http://www.devcomponents.com",
			                      trial, brushTrial, this.DisplayRectangle.X+this.DisplayRectangle.Width/2, this.DisplayRectangle.Bottom - 14, formatTrial);
			brushTrial.Dispose();
			formatTrial.Dispose();
			trial.Dispose();
#endif
		}
		
		/// <summary>
		/// Paints control to canvas. This method might be used for print output.
		/// </summary>
		/// <param name="g">Graphics object to paint control to.</param>
		/// <param name="background">Indicates whether to paint control background.</param>
		public void PaintTo(Graphics g, bool background)
		{
			PaintTo(g, background, Rectangle.Empty);
		}
				
		/// <summary>
		/// Paints control to canvas. This method might be used for print output.
		/// </summary>
		/// <param name="g">Graphics object to paint control to.</param>
		/// <param name="background">Indicates whether to paint control background.</param>
		/// <param name="clipRectangle">Indicates clipping rectangle. Nodes outside of clipping rectangle will not be painted. You can pass Rectangle.Empty and all nodes will be painted.</param>
		public void PaintTo(Graphics g, bool background, Rectangle clipRectangle)
		{
			if(background)
				PaintStyleBackground(g);
			Point lockedOffset=m_NodeDisplay.GetLockedOffset();
			Point offsetDisplay = m_NodeDisplay.Offset;
			Point offset=Point.Empty;
			
			m_NodeLayout.Graphics = g;
			try
			{
				m_NodeLayout.PerformLayout();
			}
			finally
			{
				m_NodeLayout.Graphics = null;
			}
			
			Node displayNode=this.GetDisplayRootNode();
			
            //if(displayNode!=null)
            //{
            //    if(this.NodeLayout is NodeMapLayout)
            //    {
            //        offset = new Point(Math.Abs(displayNode.ChildNodesBounds.Left),Math.Abs(displayNode.ChildNodesBounds.Top));
            //    }
            //}
			
			m_NodeDisplay.SetLockedOffset(offset);
			
			try
			{
				PaintTree(g, clipRectangle, Point.Empty, true, 1f);
			}
			finally
			{
				m_NodeLayout.PerformLayout();
				m_NodeDisplay.SetLockedOffset(lockedOffset);
				if(lockedOffset.IsEmpty)
					m_NodeDisplay.Offset = offsetDisplay;
			}
		}
		
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			this.RecalcLayout();
			base.OnRightToLeftChanged(e);			
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated (e);
			#if TRIAL
			if(!this.DesignMode)
			{
                RemindForm f = new RemindForm();
				//Design.ComponentNotLicensed f = new Design.ComponentNotLicensed();
				f.ShowDialog();
				f.Dispose();
			}
			#endif
		}

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DestroyDragScrollTimer();
            base.OnHandleDestroyed(e);
        }

        private int _IsLayoutOrResize = 0;
		protected override void OnResize(EventArgs e)
        {
            _IsLayoutOrResize++;
            try
            {
                base.OnResize(e);
                if (this.Size.Width == 0 || this.Size.Height == 0)
                    return;
                if (m_Columns.Count > 0 && m_Columns.UsesRelativeSize && !_FirstLayout)
                    this.InvalidateNodesSize();
                this.RecalcLayout();
                UpdateControlBorderPanel();
            }
            finally
            {
                _IsLayoutOrResize--;
            }
            
 		}

        protected override void OnLayout(LayoutEventArgs levent)
        {
            try
            {
                _IsLayoutOrResize++;
                base.OnLayout(levent);
            }
            finally
            {
                _IsLayoutOrResize--;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (_KeyboardSearchEnabled && char.IsLetterOrDigit(e.KeyChar))
            {
                Node node = this.SelectedNode;
                if (!(node != null && node.CheckBoxVisible && node.Enabled && e.KeyChar == ' ') || node == null)
                    e.Handled = ProcessKeyboardCharacter(e.KeyChar);
            }
            base.OnKeyPress(e);
        }

        private bool _KeyboardSearchEnabled = true;
        /// <summary>
        /// Gets or sets whether keyboard incremental search through Node.Text property is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether keyboard incremental search is enabled.")]
        public bool KeyboardSearchEnabled
        {
            get { return _KeyboardSearchEnabled; }
            set
            {
                _KeyboardSearchEnabled = value;
            }
        }

        private System.Windows.Forms.Timer _SearchBufferExpireTimer = null;
        /// <summary>
        /// Processes the keyboard character and executes the search through the nodes.
        /// </summary>
        /// <param name="p">Character to process.</param>
        /// <returns></returns>
        public virtual bool ProcessKeyboardCharacter(char p)
        {
            string searchString = UpdateSearchBuffer(p.ToString());
            Node node = FindNodeByText(searchString, SelectedNode, true);
            if (node == null && SelectedNode != null)
            {
                // Try from top searching
                node = FindNodeByText(searchString, true);
            }
            else
            {
                while (node != null && !node.Selectable)
                {
                    node = FindNodeByText(searchString, node, true);
                }
            }
            if (node != null)
                SelectNode(node, eTreeAction.Keyboard);
            return false;
        }

        private int _SearchBufferExpireTimeout = 1000;
        /// <summary>
        /// Gets or sets the keyboard search buffer expiration timeout. Default value is 1000 which indicates that
        /// key pressed within 1 second will add to the search buffer and control will be searched for node text
        /// that begins with resulting string. Setting this value to 0 will disable the search buffer.
        /// </summary>
        [DefaultValue(1000), Category("Behavior"), Description("Indicates keyboard search buffer expiration timeout.")]
        public int SearchBufferExpireTimeout
        {
            get { return _SearchBufferExpireTimeout; }
            set
            {
                if (value < 0) value = 0;
                _SearchBufferExpireTimeout = value;
            }
        }

        private string _SearchBuffer = "";
        private string UpdateSearchBuffer(string s)
        {
            if (_SearchBufferExpireTimeout <= 0)
                return s;

            if (_SearchBufferExpireTimer == null)
            {
                _SearchBufferExpireTimer = new System.Windows.Forms.Timer();
                _SearchBufferExpireTimer.Interval = _SearchBufferExpireTimeout;
                _SearchBufferExpireTimer.Tick += new EventHandler(SearchBufferExpireTimerTick);
                _SearchBufferExpireTimer.Start();
            }
            else
                _SearchBufferExpireTimer.Start();
            _SearchBuffer += s;
            return _SearchBuffer;
        }

        private void SearchBufferExpireTimerTick(object sender, EventArgs e)
        {
            _SearchBufferExpireTimer.Stop();
            _SearchBuffer = "";
        }
		
		protected override void OnKeyDown(KeyEventArgs e)
		{
			KeyNavigation.KeyDown(this, e);
			base.OnKeyDown (e);
		}

        protected override bool IsInputChar(char charCode)
        {
            if (char.IsLetterOrDigit(charCode))
                return true;
            return base.IsInputChar(charCode);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.IsKeyboardFocusWithin && !this.Focused)
            {
                foreach (Cell item in m_HostedControlCells)
                {
                    if (item.HostedControl != null && (item.HostedControl.Focused || item.HostedControl.ContainsFocus))
                        return base.ProcessCmdKey(ref msg, keyData);
                }
            }

            if (!m_CellEditing)
            {
                if (this.AutoScroll && this.AutoScrollMinSize.Height > this.ClientRectangle.Height && _VScrollBar!=null)
                {
                    if (keyData == (Keys.Down | Keys.Control))
                    {
                        // Scroll UP
                        AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Max(AutoScrollPosition.Y - _VScrollBar.SmallChange, -(_VScrollBar.Maximum - _VScrollBar.LargeChange)));
                        return true;
                    }
                    else if (keyData == (Keys.Up | Keys.Control))
                    {
                        // Scroll Down
                        AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Min(0, AutoScrollPosition.Y + _VScrollBar.SmallChange));
                        return true;
                    }
                }

                if (keyData == Keys.Left || keyData == Keys.Right ||
                    keyData == Keys.Up || keyData == (Keys.Up | Keys.Shift) ||
                    keyData == Keys.Down || keyData == (Keys.Down | Keys.Shift) || keyData == Keys.End || keyData == Keys.Home || 
                    keyData == Keys.PageDown || keyData == Keys.PageUp)
                {
                    return KeyNavigation.NavigateKeyDown(this, new KeyEventArgs(keyData));
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private ColumnHeader _MouseDownColumnHeader = null;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
            
			if(!this.Focused)
				this.Focus();
			
			Point mousePos = GetLayoutPosition(e);
			if(m_CellEditing)
			{
				m_CellMouseDownCounter=0;
				if(!EndCellEditing(eTreeAction.Mouse))
					return;
			}

            NodeHitTestInfo info = e.Button == MouseButtons.Left && _FullRowSelect && _View != eView.Tile ? NodeOperations.GetNodeAt(this, mousePos.Y, false, true) : NodeOperations.GetNodeAt(this, mousePos.X, mousePos.Y, false, true);
                
            if (_MouseOverColumnHeader != null && e.Button == MouseButtons.Left && AllowUserToResizeColumns)
            {
                if (info.ColumnsAt != null)
                {
                    StartColumnResize(mousePos.X, mousePos.Y, info.ColumnsAt);
                }
                else if (_GridColumnLineResizeEnabled)
                {
                    StartColumnResize(mousePos.X, mousePos.Y, m_Columns);
                }
            }
            else if (info.ColumnsAt != null)
            {
                ColumnHeader ch = GetColumnAt(mousePos.X, mousePos.Y, info.ColumnsAt);
                if (ch != null)
                {
                    _MouseDownColumnHeader = ch;
                    ch.OnMouseDown(e);
                }
            }

            Node node = info.NodeAt; // e.Button == MouseButtons.Left && _FullRowSelect ? this.GetNodeAt(mousePos.Y) : this.GetNodeAt(mousePos.X, mousePos.Y);
            if (node != null)
                OnNodeMouseDown(node, e, m_NodeDisplay.Offset);
            else if (_MultiSelect && this.SelectedNodes.Count > 1)
                SelectNode(null, eTreeAction.Mouse);
			m_MouseDownLocation=mousePos;
			
#if !TRIAL
			if(NodeOperations.keyValidated2!=114 && !m_DialogDisplayed)
			{
                RemindForm f = new RemindForm();
				f.ShowDialog();
				f.Dispose();
				m_DialogDisplayed=true;
			}
#endif
		}

        private DateTime _LastMouseUpTime = DateTime.MinValue;
        private DateTime _IgnoreDoubleClickTime = DateTime.MinValue;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_MouseDownColumnHeader != null)
                _MouseDownColumnHeader.OnMouseUp(e);
            _MouseDownColumnHeader = null;

            if (_ColumnResize != null)
            {
                DateTime now = DateTime.Now;
                ColumnHeader header = _ColumnResize;
                _ColumnResize = null;
                this.Capture = false;
                if (_LastMouseUpTime == DateTime.MinValue || now.Subtract(_LastMouseUpTime).TotalMilliseconds > SystemInformation.DoubleClickTime)
                    _LastMouseUpTime = DateTime.Now;
                else if (now.Subtract(_LastMouseUpTime).TotalMilliseconds <= SystemInformation.DoubleClickTime)
                {
                    _LastMouseUpTime = DateTime.MinValue;
                    header.OnDoubleClick(EventArgs.Empty);
                    if (header.DoubleClickAutoSize) header.AutoSize();
                    if (header.Parent != null && header.Parent.ParentNode == null)
                    {
                        if (_ColumnHeader != null)
                            _ColumnHeader.IgnoreDoubleClickTime = now;
                    }
                    else if (header.Parent != null && header.Parent.Parent != null)
                        _IgnoreDoubleClickTime = now;
                    else
                        _IgnoreDoubleClickTime = DateTime.MinValue;

                }
                else
                    _LastMouseUpTime = DateTime.MinValue;
                OnColumnResized(header, EventArgs.Empty);
                //if (ColumnResized != null) ColumnResized(this, EventArgs.Empty);
                return;
            }
            else
            {
                ColumnHeader columnToMove = _ColumnReorder;
                if (columnToMove != null)
                {
                    this.Cursor = _OldCursor;
                    _OldCursor = null;
                    ColumnHeaderCollection columnsCollection = columnToMove.Parent;
                    if (columnToMove.Parent == columnsCollection)
                    {
                        int moveToPosition = this.ColumnMoveMarkerIndex;
                        int newDisplayIndex = -1;
                        this.ColumnMoveMarkerIndex = -1;
                        if (moveToPosition >= -1)
                        {
                            if (moveToPosition == columnsCollection.Count)
                                newDisplayIndex = columnsCollection.GetDisplayIndex(columnsCollection.LastVisibleColumn) + 1;
                            else if (moveToPosition >= 0)
                            {
                                int displayIndex = columnsCollection.GetDisplayIndex(columnsCollection[moveToPosition]);
                                if (displayIndex >= 0)
                                    newDisplayIndex = displayIndex;
                                else
                                    newDisplayIndex = 0;
                            }
                            List<int> displayIndexMap = columnsCollection.DisplayIndexMap;
                            int oldDisplayIndex = columnsCollection.GetDisplayIndex(columnToMove);
                            displayIndexMap.RemoveAt(oldDisplayIndex);
                            if (oldDisplayIndex < newDisplayIndex) newDisplayIndex--;
                            displayIndexMap.Insert(newDisplayIndex, columnsCollection.IndexOf(columnToMove));
                            for (int i = 0; i < displayIndexMap.Count; i++)
                            {
                                columnsCollection[displayIndexMap[i]].DisplayIndex = i;
                            }
                            OnColumnMoved(new ColumnMovedEventArgs(columnToMove, oldDisplayIndex, newDisplayIndex));
                        }
                    }
                    _ColumnReorder = null;
                }
            }

            _LastMouseUpTime = DateTime.MinValue;

            ReleaseMouseOverColumnHeader();

            OnNodeMouseUp(e);
        }
        /// <summary>
        /// Raises ColumnResized event.
        /// </summary>
        /// <param name="header">ColumnHeader that was resized.</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnColumnResized(ColumnHeader header, EventArgs e)
        {
            EventHandler handler = ColumnResized;
            if (handler != null)
                handler(header, e);
        }
        
        /// <summary>
        /// Raises ColumnMoved event.
        /// </summary>
        /// <param name="header">ColumnHeader that was moved.</param>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnColumnMoved(ColumnMovedEventArgs e)
        {
            ColumnMovedHandler handler = ColumnMoved;
            if (handler != null)
                handler(this, e);
        }
		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);

			OnNodeMouseClick(e);
		}
		
		protected override void OnDoubleClick(EventArgs e)
		{
            base.OnDoubleClick (e);
            MouseEventArgs sourceArgs = e as MouseEventArgs;

            Node selectedNode = this.SelectedNode;
			if(selectedNode!=null)
			{
				Point p=this.PointToClient(Control.MousePosition);
                p = GetLayoutPosition(p);
                if (selectedNode.Bounds.Contains(p))
                {
                    Cell cell = GetCellAt(p);
                    bool toggle = true;
                    if (cell != null)
                    {
                        Rectangle r = NodeDisplay.GetCellRectangle(eCellRectanglePart.CheckBoxBounds, cell, m_NodeDisplay.Offset);
                        if (!r.IsEmpty && r.Contains(p))
                            toggle = false;
                    }
                    Node node = selectedNode;
                    if (_DoubleClickTogglesNode && toggle && (node.HasChildNodes || node.ExpandVisibility == eNodeExpandVisibility.Visible))
                        node.Toggle(eTreeAction.Mouse);

                    InvokeNodeDoubleClick(new TreeNodeMouseEventArgs(node, 
                                              (sourceArgs != null ? sourceArgs.Button : MouseButtons.Left), 
                                              2, 
                                              0, 
                                              p.X, 
                                              p.Y));
                }

                if (selectedNode.HasColumns && selectedNode.NodesColumnsHeaderVisible && selectedNode.CommandBounds.Contains(p))
                {
                    ColumnHeader ch = GetColumnAt(p.X, p.Y, selectedNode.NodesColumns);
                    if (ch != null)
                    {
                        DateTime now = DateTime.Now;
                        if (_IgnoreDoubleClickTime != DateTime.MinValue && now.Subtract(_IgnoreDoubleClickTime).TotalMilliseconds <= SystemInformation.DoubleClickTime)
                        {
                            _IgnoreDoubleClickTime = DateTime.MinValue;
                            return;
                        }
                        _IgnoreDoubleClickTime = DateTime.MinValue;
                        ch.OnDoubleClick(e);
                    }
                }
			}
		}

        private bool _DoubleClickTogglesNode = true;
        /// <summary>
        /// Gets or sets whether double-clicking the node will toggle its expanded state. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether double-clicking the node will toggle its expanded state.")]
        public bool DoubleClickTogglesNode
        {
            get { return _DoubleClickTogglesNode; }
            set
            {
                _DoubleClickTogglesNode = value;
            }
        }
        

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			
			if(m_MouseOverNode!=null)
				this.SetMouseOverNode(null);
		}
		
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover (e);
			if(m_MouseOverNode!=null && (FireHoverEvent || m_MouseOverNode.FireHoverEvent))
			{
				Point p=this.PointToClient(Control.MousePosition);
				p=GetLayoutPosition(p);
				InvokeNodeMouseHover(new TreeNodeMouseEventArgs(m_MouseOverNode, Control.MouseButtons, 0, 0, p.X, p.Y));
			}
		}

        private ColumnHeader _MouseOverColumnHeader = null;
        private ColumnHeaderCollection _MouseOverColumsHeaderCollection = null;
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool bUpdate=false;

            Point mousePos = GetLayoutPosition(e);

            if (_MouseDownColumnHeader != null && _ColumnReorder == null && Math.Abs(mousePos.X - m_MouseDownLocation.X) >= 2 && _AllowUserToReorderColumns)
            {
                StartColumnReorder(e.X, e.Y, _MouseDownColumnHeader.Parent);
                return;
            }

            if (_ColumnResize != null)
            {
                Point offset = Point.Empty;
                if (AutoScroll)
                {
                    offset = GetAutoScrollPositionOffset();
                    if (_MouseOverColumsHeaderCollection==null) // Main column header resize
                        offset.Y = 0;
                }
                Rectangle columnBounds = _ColumnResize.Bounds;
                columnBounds.Offset(offset);
                columnBounds = GetScreenRectangle(columnBounds);
                int columnWidth = Math.Max(Math.Max(2, _ColumnResize.MinimumWidth), mousePos.X - columnBounds.X);
                
                if (columnWidth != _ColumnResize.Width.Absolute)
                {
                    _ColumnResize.Width.AutoSize = false;
                    _ColumnResize.Width.Absolute = columnWidth;
                    OnColumnResizing(_ColumnResize, EventArgs.Empty);
                    if (_MouseOverColumsHeaderCollection != null) InvalidateNodeSize(_MouseOverColumsHeaderCollection.ParentNode);
                    this.Update();
                    if (_ColumnHeader != null) _ColumnHeader.Refresh();
                    if (m_CellEditing && _CellEditControl is Control)
                    {
                        Control editControl = (Control)_CellEditControl;
                        Rectangle rCell = NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds, m_EditedCell, m_NodeDisplay.Offset);
                        rCell = GetScreenRectangle(rCell);
                        editControl.Bounds = rCell;
                    }
                }
                return;
            }
            else if (_ColumnReorder != null)
            {
                ColumnHeader col = GetColumnAt(mousePos.X, mousePos.Y, _ColumnReorder.Parent);
                int columnIndex = -1;
                if (col != null)
                    columnIndex = col.Parent.IndexOf(col);
                else if (col == null)
                {
                    ColumnHeader lastVisible= _ColumnReorder.Parent.LastVisibleColumn;
                    if (lastVisible != null && mousePos.X >= lastVisible.Bounds.Right)
                        columnIndex = _ColumnReorder.Parent.Count;
                }
                if (columnIndex >= 0)
                {
                    this.ColumnMoveMarkerIndex = columnIndex;
                }
                return;
            }

            if (e.Button == MouseButtons.Left && !m_CellEditing && m_DragDropEnabled && m_MouseOverNode != null && m_DragNode == null && m_MouseOverNode.DragDropEnabled && m_MouseOverNode.IsSelected &&
                (Math.Abs(m_MouseDownLocation.X - mousePos.X) > SystemInformation.DragSize.Width || Math.Abs(m_MouseDownLocation.Y - mousePos.Y) > SystemInformation.DragSize.Height))
            {
                StartDragDrop();
            }

			Rectangle r=Rectangle.Empty;
			if(m_MouseOverNode!=null)
				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,m_MouseOverNode,m_NodeDisplay.Offset);

            bool checkColumns = true;
            if (_AllowUserToResizeColumns && _GridLines && _GridColumnLineResizeEnabled && m_Columns.Count > 0)
            {
                ColumnHeader mouseOverHeader = GetColumnForResizeAt(mousePos.X, mousePos.Y, m_Columns, true, true);
                if (mouseOverHeader != null)
                {
                    if (_MouseOverColumnHeader == null)
                    {
                        m_OriginalCursor = this.Cursor;
                        this.Cursor = Cursors.VSplit;
                    }
                    _MouseOverColumnHeader = mouseOverHeader;
                    checkColumns = false;
                }
                else
                    ReleaseMouseOverColumnHeader();
            }

			if(!r.IsEmpty && r.Contains(mousePos))
				bUpdate=OnNodeMouseMove(m_MouseOverNode,e,m_NodeDisplay.Offset);
			else
			{
                Node node = null;
                NodeHitTestInfo info = null;
                if(_FullRowSelect && _View == eView.Tree)
                    info = NodeOperations.GetNodeAt(this, mousePos.Y, true);
                else
                    info = NodeOperations.GetNodeAt(this, mousePos.X, mousePos.Y, true);
                node = info.NodeAt;
                if (node != m_MouseOverNode)
                    bUpdate = SetMouseOverNode(node);

                if (checkColumns)
                {
                    // Check for mouse over Columns
                    if (node == null && info.ColumnsAt != null && _AllowUserToResizeColumns)
                    {
                        ColumnHeader mouseOverHeader = GetColumnForResizeAt(mousePos.X, mousePos.Y, info.ColumnsAt, false);
                        if (mouseOverHeader != null)
                        {
                            if (_MouseOverColumnHeader == null)
                            {
                                m_OriginalCursor = this.Cursor;
                                this.Cursor = Cursors.VSplit;
                            }
                            _MouseOverColumnHeader = mouseOverHeader;
                            _MouseOverColumsHeaderCollection = info.ColumnsAt;
                        }
                        else
                            ReleaseMouseOverColumnHeader();
                    }
                    else
                        ReleaseMouseOverColumnHeader();
                }

                if (m_MouseOverNode != null)
                    bUpdate = bUpdate | OnNodeMouseMove(m_MouseOverNode, e, m_NodeDisplay.Offset);
			}

			if(bUpdate)
				this.Update();
		}
        /// <summary>
        /// Raises ColumnResizing event.
        /// </summary>
        /// <param name="column">Column being resized</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnColumnResizing(ColumnHeader column, EventArgs e)
        {
            EventHandler handler = ColumnResizing;
            if (handler != null) handler(column, e);
        }
        private void ReleaseMouseOverColumnHeader()
        {
            if (_MouseOverColumnHeader != null)
            {
                _MouseOverColumnHeader = null;
                if (m_OriginalCursor != null)
                    this.Cursor = m_OriginalCursor;
                else
                    this.Cursor = null;
                m_OriginalCursor = null;
            }
            _MouseOverColumsHeaderCollection = null;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (this.AutoScroll && _VScrollBar != null && !this.IsCellEditing)
            {
                if (e.Delta < 0)
                {
                    AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Max(AutoScrollPosition.Y - _VScrollBar.SmallChange * SystemInformation.MouseWheelScrollLines, -(_VScrollBar.Maximum - _VScrollBar.LargeChange)));
                }
                else
                {
                    AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Min(0, AutoScrollPosition.Y + _VScrollBar.SmallChange * SystemInformation.MouseWheelScrollLines));
                    
                }
            }
            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Deselect specified node. Use this method when multiple node selection is enabled to deselect single node or all nodes.
        /// </summary>
        /// <param name="node">Reference to node to select or null to deselect all selected nodes.</param>
        /// <param name="action">Action that is selecting the node.</param>
        public void DeselectNode(Node node, eTreeAction action)
        {
            if (!_MultiSelect || node == null)
            {
                SelectNode(null, action);
                return;
            }

            if (!node.IsSelected) return;
            m_SelectedNodes.Remove(node, action);
        }

        internal void InvokeSelectionChanged(EventArgs e)
        {
            OnSelectionChanged(e);
        }

        /// <summary>
        /// Raises SelectionChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            EventHandler handler = SelectionChanged;
            if (handler != null) handler(this, e);
        }

        internal void InvokeCellSelected(AdvTreeCellEventArgs e)
        {
            OnCellSelected(e);
        }
        /// <summary>
        /// Raises CellSelected event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCellSelected(AdvTreeCellEventArgs e)
        {
            AdvTreeCellEventHandler handler = CellSelected;
            if (handler != null) handler(this, e);
        }
        internal void InvokeCellUnselected(AdvTreeCellEventArgs e)
        {
            OnCellUnselected(e);
        }
        /// <summary>
        /// Raises CellUnselected event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCellUnselected(AdvTreeCellEventArgs e)
        {
            AdvTreeCellEventHandler handler = CellUnselected;
            if (handler != null) handler(this, e);
        }

        /// <summary>
		/// Selected specified node.
		/// </summary>
		/// <param name="node">Node to select.</param>
		/// <param name="action">Action that is selecting the node.</param>
		public void SelectNode(Node node, eTreeAction action)
		{
            if (node != null && !node.Selectable && !this.DesignMode) return;
            if (!_MultiSelect && node == m_SelectedNode) return;
            
            AdvTreeNodeCancelEventArgs cancelArgs = new AdvTreeNodeCancelEventArgs(action, node);
			OnBeforeNodeSelect(cancelArgs);
			if(cancelArgs.Cancel)
				return;

            if (_MultiSelect && !this.DesignMode)
            {
                m_SelectedNodes.SourceAction = action;
                m_SelectedNodes.Clear();
                m_SelectedNodes.SourceAction = eTreeAction.Code;
                bool fireSelection = false;
                if (node != null)
                {
                    m_SelectedNodes.SuspendEvents = true;
                    try
                    {
                        if (m_SelectedNodes.Add(node, action) != -1)
                        {
                            node.EnsureVisible();
                            fireSelection = true;
                        }
                    }
                    finally
                    {
                        m_SelectedNodes.SuspendEvents = false;
                    }
                }
                this.Invalidate();
                //OnSelectionChanged(EventArgs.Empty);  // Fired from SelectedNodes collection
                return;
            }
			
			//bool bUpdate=false;

			if(m_CellEditing)
			{
				if(!EndCellEditing(eTreeAction.Code))
					return;
			}
			
#if !TRIAL
			if(NodeOperations.keyValidated2<114)
			{
				NodeDisplay.keyInvalid = true;
				NodeOperations.keyValidated2 = NodeOperations.keyValidated2 + 124;
			}
			else if(NodeOperations.keyValidated2>114)
				return;
#endif
            Node oldSelected = m_SelectedNode;
            m_SelectedNode = node;

            if (oldSelected != null)
            {
                oldSelected.IsSelected = false;
                InvalidateNode(oldSelected);
                //bUpdate = true;
                if (oldSelected.GetSelectedCell() != null && oldSelected != node)
                    oldSelected.SetSelectedCell(null, action); // SelectedCell.SetSelected(false);
                if (m_SelectedNodes.Contains(oldSelected))
                    m_SelectedNodes.Remove(oldSelected);
                OnAfterNodeDeselect(new AdvTreeNodeEventArgs(action, oldSelected));
                oldSelected.InternalDeselected(action);
            }

            if (m_SelectedNode != null)
            {
                m_SelectedNode.IsSelected = true;
                m_SelectedNodes.Add(m_SelectedNode);
                InvalidateNode(m_SelectedNode);
                //bUpdate = true;
                if (m_SelectedNode.SelectedCell == null)
                {
                    m_SelectedNode.SelectFirstCell(action);
                }
                m_SelectedNode.EnsureVisible();
            }

			if(this.SelectedPathConnector!=null)
				this.Invalidate();
			
			AdvTreeNodeEventArgs args = new AdvTreeNodeEventArgs(action, node);
			OnAfterNodeSelect(args);

            if (m_SelectedNode != null)
            {
                m_SelectedNode.InternalSelected(action);
            }
#if (FRAMEWORK20)
            if (_DataManager != null)
            {
                if (node != null && node.BindingIndex > -1 && _DataManager.Position != node.BindingIndex)
                {
                    _DataManager.Position = node.BindingIndex;
                }
                else if (node == null)
                    _DataManager.Position = -1;
            }
            OnSelectedIndexChanged(EventArgs.Empty);
            if (!string.IsNullOrEmpty(this.ValueMember))
                this.OnSelectedValueChanged(EventArgs.Empty);
#endif
            OnSelectionChanged(EventArgs.Empty);
		}

		private void OnCellStyleChanged()
		{
			InvalidateNodesSize();
			if(this.DesignMode)
			{
				this.RecalcLayout();
				this.Refresh();
			}
		}

		private void SetSizeChanged(Node node)
		{
			node.SizeChanged=true;
			foreach(Node c in node.Nodes)
				SetSizeChanged(c);
		}

        
        /// <summary>
        /// Invalidates the size for all top-level nodes and their sub-nodes.
        /// </summary>
		public void InvalidateNodesSize()
		{
			foreach(Node node in m_Nodes)
				SetSizeChanged(node);
		}

        /// <summary>
        /// Invalidates the size for a node and its sub-nodes.
        /// </summary>
        /// <param name="node">Node to invalidate size for.</param>
        public void InvalidateNodeSize(Node node)
        {
            SetSizeChanged(node);
        }

		private void ImageListDisposed(object sender, EventArgs e)
		{
			if(sender==m_ImageList)
			{
				this.ImageList=null;
			}
		}

		/// <summary>
		/// Ensures that selected node is visible i.e. that all parents of the selected node are expanded. If not selects the first parent node not expanded.
		/// </summary>
		internal void ValidateSelectedNode()
		{
			Node node=this.SelectedNode;
            Node nodeSelected = node;
            if (node == null)
                return;
            if (node.TreeControl != this)
            {
                nodeSelected = null;
            }
            else
            {
                while (node != null)
                {
                    node = node.Parent;
                    if (node != null && !node.Expanded && node.Selectable)
                        nodeSelected = node;
                }
            }
            if (nodeSelected == null || !nodeSelected.IsVisible)
            {
                if (!SelectFirstNode(eTreeAction.Code))
                    this.SelectedNode = null;
            }
            else
            {
                if (_MultiSelect)
                {
                    if (!this.SelectedNodes.Contains(nodeSelected))
                        this.SelectedNode = nodeSelected;
                }
                else
                {
                    if (this.SelectedNode != nodeSelected)
                        this.SelectedNode = nodeSelected;
                }
            }
		}

		private bool SetMouseOverCell(Cell mouseOverCell)
		{
			bool bUpdate=false;
			if(mouseOverCell==m_MouseOverCell)
				return bUpdate;

			if(m_MouseOverCell!=null && m_MouseOverCell!=mouseOverCell)
			{
				if(this.CellStyleMouseOver!=null || m_MouseOverCell.StyleMouseOver!=null || m_RenderMode!=eNodeRenderMode.Default || m_MouseOverCell.Parent!=null && m_MouseOverCell.Parent.RenderMode!=eNodeRenderMode.Default)
					bUpdate=true;
				m_MouseOverCell.SetMouseOver(false);
			}

			m_MouseOverCell=mouseOverCell;
			if(m_MouseOverCell!=null)
			{
				m_MouseOverCell.SetMouseOver(true);
				if(this.CellStyleMouseOver!=null || m_MouseOverCell.StyleMouseOver!=null || m_RenderMode!=eNodeRenderMode.Default || m_MouseOverCell.Parent!=null && m_MouseOverCell.Parent.RenderMode!=eNodeRenderMode.Default )
					bUpdate=true;
			}

			UpdateTreeCursor();

			return bUpdate;
		}

		private bool SetMouseOverNode(Node mouseOverNode)
		{
			bool bUpdate=false;
			if(m_MouseOverNode!=null)
			{
				bUpdate=bUpdate | SetMouseOverCell(null);
                if (m_MouseOverNode.MouseOverNodePart == eMouseOverNodePart.Expand)
                    bUpdate = true;
				m_MouseOverNode.MouseOverNodePart=eMouseOverNodePart.None;
				if(this.NodeStyleMouseOver!=null || this.CellStyleMouseOver!=null || m_MouseOverNode.StyleMouseOver!=null || this.NodeStyleMouseOver!=null ||
					m_RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.CommandButton || bUpdate || _HotTracking)
				{
					InvalidateNode(m_MouseOverNode);
					bUpdate=true;
				}
				
				if(m_MouseOverNode!=mouseOverNode && m_MouseOverNode!=null)
				{
					Point p=this.PointToClient(Control.MousePosition);
					InvokeNodeMouseLeave(new TreeNodeMouseEventArgs(m_MouseOverNode, Control.MouseButtons, 0, 0, p.X, p.Y));
				}
			}

            bool mouseOverNodeChanged = m_MouseOverNode != mouseOverNode;
			m_MouseOverNode=mouseOverNode;
			
			if(m_MouseOverNode!=null)
			{
				m_MouseOverNode.MouseOverNodePart=eMouseOverNodePart.Node;
				if(m_MouseOverNode.StyleMouseOver!=null || this.NodeStyleMouseOver!=null ||
					m_RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.CommandButton || _HotTracking)
				{
					InvalidateNode(m_MouseOverNode);
					bUpdate=true;
				}
				
				Point p=this.PointToClient(Control.MousePosition);
				InvokeNodeMouseEnter(new TreeNodeMouseEventArgs(m_MouseOverNode, Control.MouseButtons, 0, 0, p.X, p.Y));

                if (mouseOverNodeChanged && (FireHoverEvent || m_MouseOverNode.FireHoverEvent))
                {
                    Interop.WinApi.ResetHover(this);
                }
			}

			return bUpdate;
		}
        private static readonly string DotNetBarPrefix = "DotNetBar.";
		private void OnNodeMouseDown(Node node, MouseEventArgs e, Point offset)
		{
			Point mousePos = GetLayoutPosition(e);
			
			InvokeNodeMouseDown(new TreeNodeMouseEventArgs(node, e.Button, e.Clicks, e.Delta, mousePos.X, mousePos.Y));
			
			if(e.Button==MouseButtons.Left)
			{
                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandHitTestBounds, node, offset);
								
				if(r.Contains(mousePos) && e.Clicks == 1 && node.ExpandVisibility != eNodeExpandVisibility.Hidden)
				{
					m_CellMouseDownCounter=0;
                    node.Toggle(eTreeAction.Mouse);
					return;
				}

				if(node.CommandButton)
				{
					m_CellMouseDownCounter=0;
					r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CommandBounds,node,offset);
					if(r.Contains(mousePos))
					{
						InvokeCommandButtonClick(node,new CommandButtonEventArgs(eTreeAction.Mouse,node));
						return;
					}
				}

                r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, offset);
                if ((r.Contains(mousePos) || _FullRowSelect && mousePos.Y >= r.Y && mousePos.Y <= r.Bottom) && node.TreeControl != null)
                {
                    if (node.TreeControl.SelectedNode != node)
                        m_CellMouseDownCounter = 0;

                    if (node.Selectable)
                    {
                        if (_MultiSelect && m_SelectedNodes.Count > 0 && Control.ModifierKeys == Keys.None) // Deselect all
                            _SelectOnMouseUp = true;
                        else
                            _SelectOnMouseUp = false;

                        if (_MultiSelect && m_SelectedNodes.Count > 0 && (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control))
                        {
                            m_CellMouseDownCounter = 0;
                            if (_MultiSelectRule == eMultiSelectRule.SameParent && m_SelectedNodes[0].Parent != node.Parent) return;
                            if (Control.ModifierKeys == Keys.Shift && m_SelectedNodes.Count > 0)
                            {
                                // Range selection
                                Node startNode = m_SelectedNodes[0];
                                Node currentNode = node;
                                bool selectionChanged = false;
                                m_SelectedNodes.MultiNodeOperation = true;
                                try
                                {
                                    while (m_SelectedNodes.Count > 1)
                                    {
                                        m_SelectedNodes.Remove(m_SelectedNodes[m_SelectedNodes.Count - 1], eTreeAction.Mouse);
                                        selectionChanged = true;
                                    }
                                }
                                finally
                                {
                                    m_SelectedNodes.MultiNodeOperation = false;
                                }
                                if (currentNode != startNode)
                                {
                                    if (currentNode.Bounds.Y > startNode.Bounds.Y)
                                    {
                                        // Selecting down
                                        m_SelectedNodes.MultiNodeOperation = true;
                                        try
                                        {
                                            do
                                            {
                                                if (!currentNode.IsSelected && currentNode.Selectable)
                                                {
                                                    if (_MultiSelectRule == eMultiSelectRule.AnyNode ||
                                                        _MultiSelectRule == eMultiSelectRule.SameParent && m_SelectedNodes.Count > 0 && m_SelectedNodes[0].Parent == currentNode.Parent)
                                                        m_SelectedNodes.Add(currentNode, eTreeAction.Mouse);
                                                }
                                                currentNode = NodeOperations.GetPreviousVisibleNode(currentNode);

                                            } while (startNode != currentNode && currentNode != null);
                                        }
                                        finally
                                        {
                                            m_SelectedNodes.MultiNodeOperation = false;
                                            InvokeSelectionChanged(EventArgs.Empty);
                                        }
                                    }
                                    else
                                    {
                                        // Selecting upwards
                                        m_SelectedNodes.MultiNodeOperation = true;
                                        try
                                        {
                                            do
                                            {
                                                if (!currentNode.IsSelected && currentNode.Selectable)
                                                {
                                                    if (_MultiSelectRule == eMultiSelectRule.AnyNode ||
                                                        _MultiSelectRule == eMultiSelectRule.SameParent && m_SelectedNodes.Count > 0 && m_SelectedNodes[0].Parent == currentNode.Parent)
                                                        m_SelectedNodes.Add(currentNode, eTreeAction.Mouse);
                                                }
                                                currentNode = NodeOperations.GetNextVisibleNode(currentNode);

                                            } while (startNode != currentNode && currentNode != null);
                                        }
                                        finally
                                        {
                                            m_SelectedNodes.MultiNodeOperation = false;
                                            InvokeSelectionChanged(EventArgs.Empty);
                                        }
                                    }
                                }
                                else if(selectionChanged)
                                    InvokeSelectionChanged(EventArgs.Empty);
                            }
                            else
                            {
                                if (node.IsSelected)
                                    m_SelectedNodes.Remove(node, eTreeAction.Mouse);
                                else
                                    m_SelectedNodes.Add(node, eTreeAction.Mouse);
                            }
                            //OnSelectionChanged(EventArgs.Empty);
                            return;
                        }
                        else if(!node.IsSelected)
                        {
                            SelectNode(node, eTreeAction.Mouse);
                            if (node.TreeControl == null || node.TreeControl.SelectedNode != node) // Action cancelled
                                return;
                        }
                    }

                    Cell cell = GetCellAt(node, mousePos.X, mousePos.Y, offset);
                    if (cell != null)
                    {
                        bool checkBoxSelection = false;
                        if (cell.CheckBoxVisible && cell.GetEnabled())
                        {
                            Rectangle rCheckBox = cell.CheckBoxBoundsRelative;
                            r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, offset);
                            rCheckBox.Offset(r.Location);
                            if (rCheckBox.Contains(mousePos))
                            {

                                if (cell.CheckBoxThreeState)
                                {
                                    if (cell.CheckState == CheckState.Checked)
                                        cell.SetChecked(CheckState.Indeterminate, eTreeAction.Mouse);
                                    else if (cell.CheckState == CheckState.Unchecked)
                                        cell.SetChecked(CheckState.Checked, eTreeAction.Mouse);
                                    else if (cell.CheckState == CheckState.Indeterminate)
                                        cell.SetChecked(CheckState.Unchecked, eTreeAction.Mouse);
                                }
                                else
                                    cell.SetChecked(!cell.Checked, eTreeAction.Mouse);
                                checkBoxSelection = true;
                                m_CellMouseDownCounter = 0;
                            }
                        }
                        if (node.SelectedCell != cell)
                        {
                            m_CellMouseDownCounter = 1;
                        }
                        else if (!checkBoxSelection)
                            m_CellMouseDownCounter++;
                        node.SetSelectedCell(cell, eTreeAction.Mouse);
                        cell.SetMouseDown(true);
                    }
                }
                else
                    m_CellMouseDownCounter = 0;
			}
			else if(e.Button==MouseButtons.Right)
			{
                if (node.TreeControl == null) return;
                if(!node.IsSelected)
				    SelectNode(node, eTreeAction.Mouse);
				if(!this.MultiSelect && node.TreeControl.SelectedNode!=node) // Action cancelled
					return;
				if(node.ContextMenu!=null)
				{
					if(node.ContextMenu is ContextMenu)
					{
						ContextMenu cm=node.ContextMenu as ContextMenu;
						cm.Show(this, new Point(e.X, e.Y));
					}
					else if(node.ContextMenu.GetType().FullName=="System.Windows.Forms.ContextMenuStrip")
					{
						node.ContextMenu.GetType().InvokeMember("Show", System.Reflection.BindingFlags.InvokeMethod, null,
						                                        node.ContextMenu, new object[] {this, new Point(e.X, e.Y)});
					}
					else if(node.ContextMenu.GetType().FullName=="DevComponents.DotNetBar.ButtonItem")
					{
						Point p=this.PointToScreen(new Point(e.X, e.Y));
                        ((PopupItem)node.ContextMenu).SetSourceControl(this);
						node.ContextMenu.GetType().InvokeMember("Popup", System.Reflection.BindingFlags.InvokeMethod,
							null, node.ContextMenu, new object[]{p});
					}
					else if(node.ContextMenu.ToString().StartsWith(DotNetBarPrefix) && m_DotNetBarManager!=null)
					{
						string menuName=node.ContextMenu.ToString().Substring(DotNetBarPrefix.Length);
						object contextMenus=m_DotNetBarManager.GetType().InvokeMember("ContextMenus", 
							System.Reflection.BindingFlags.GetProperty, null, m_DotNetBarManager, null);
						int index=(int)contextMenus.GetType().InvokeMember("IndexOf", System.Reflection.BindingFlags.InvokeMethod,
							null, contextMenus, new string[]{menuName});
						if(index>=0)
						{
							IList list=contextMenus as IList;
							object popup=list[index];
							// Older version of DotNetBar do not have this method exposed so ignore the error...
							try
							{
								popup.GetType().InvokeMember("SetSourceControl", System.Reflection.BindingFlags.InvokeMethod,
									null, popup, new object[]{this});
							}
							catch {}
							
							Point p=this.PointToScreen(new Point(e.X, e.Y));
							popup.GetType().InvokeMember("Popup", System.Reflection.BindingFlags.InvokeMethod,
								null, popup, new object[]{p});
						}
					}
				}
			}
		}

        private bool _SelectOnMouseUp = false;
		private void OnNodeMouseUp(MouseEventArgs e)
		{
			bool bUpdate=false;
			Point mousePos = GetLayoutPosition(e);

            if (this.SelectedNode != null)
            {
                if (this.SelectedNode.SelectedCell != null && this.SelectedNode.SelectedCell.IsMouseDown)
                {
                    this.SelectedNode.SelectedCell.SetMouseDown(false);
                    this.InvalidateNode(this.SelectedNode);
                    bUpdate = true;
                }
            }

			if(bUpdate)
				this.Update();

            Node node = null;
            if (this.SelectedNode != null && this.SelectedNode.Bounds.Contains(mousePos))
                node = this.SelectedNode;
            else
                node = NodeOperations.GetNodeAt(this, mousePos.X, mousePos.Y, true).NodeAt; //this.GetNodeAt(mousePos);

            if (_SelectOnMouseUp && node != null && node.IsSelected && _MultiSelect && m_SelectedNodes.Count > 1 && Control.ModifierKeys == Keys.None) // Deselect all
                SelectNode(node, eTreeAction.Mouse);

			if(node!=null)
			{
				InvokeNodeMouseUp(new TreeNodeMouseEventArgs(node, e.Button, e.Clicks, e.Delta, mousePos.X, mousePos.Y));
			}
		}

        private void OnNodeMouseClick(EventArgs e)
		{
			if(this.SelectedNode==null)
				return;
            MouseEventArgs me = e as MouseEventArgs;
            if (me == null)
            {
                Point p = this.PointToClient(Control.MousePosition);
                me = new MouseEventArgs(MouseButtons.Left, 1, p.X, p.Y, 0);
            }

            Point mousePos = GetLayoutPosition(me);
            NodeHitTestInfo info = (me.Button == MouseButtons.Left && _FullRowSelect && _View == eView.Tree) ? NodeOperations.GetNodeAt(this, mousePos.Y, true, true) : NodeOperations.GetNodeAt(this, mousePos.X, mousePos.Y, true, true);
            if (info.NodeAt == this.SelectedNode && info.NodeAt!=null && !info.NodeAt.ExpandPartRectangle.Contains(mousePos))
            {
                InvokeNodeClick(new TreeNodeMouseEventArgs(this.SelectedNode, me.Button, me.Clicks, me.Delta, me.X, me.Y));
                if (m_CellMouseDownCounter > 1)
                    EditSelectedCell(eTreeAction.Mouse);
            }

            if (info.ColumnsAt != null)
            {
                ColumnHeader ch = GetColumnAt(mousePos.X, mousePos.Y, info.ColumnsAt);
                if (ch != null)
                {
                    ch.OnClick(e);
                }
            }
            
		}

        internal bool EditSelectedCell(eTreeAction actionSource)
        {
            // Start editing if allowed
            if (this.CellEdit && this.SelectedNode.SelectedCell != null && this.SelectedNode.SelectedCell.IsEditable)
            {
                EditCell(this.SelectedNode.SelectedCell, actionSource);
                return true;
            }
            return false;
        }

        private bool _HotTracking = false;
        /// <summary>
        /// Gets or sets whether node is highlighted when mouse enters the node. Default value is false.
        /// </summary>
        /// <remarks>
        /// There are two ways to enable the node hot-tracking. You can set the HotTracking property to true in which case the
        /// mouse tracking is enabled using system colors specified in TreeColorTable. You can also define the NodeStyleMouseOver 
        /// style which gets applied to the node when mouse is over the node.
        /// </remarks>
        [DefaultValue(false), Category("Appearance"), Description("Indicates whether node is highlighted when mouse enters the node.")]
        public bool HotTracking
        {
            get { return _HotTracking; }
            set
            {
                _HotTracking = value;
                this.Invalidate();
            }
        }

		/// <summary>
		/// Starts editing specified cell, places the cell into the edit mode.
		/// </summary>
		/// <param name="cell">Cell to start editing.</param>
		/// <param name="action">Action that is a cause for the edit.</param>
		internal void EditCell(Cell cell, eTreeAction action)
		{
			EditCell(cell, action, null);
		}
		
        private ICellEditControl _CellEditControl = null;
		/// <summary>
		/// Starts editing specified cell, places the cell into the edit mode.
		/// </summary>
		/// <param name="cell">Cell to start editing.</param>
		/// <param name="action">Action that is a cause for the edit.</param>
		/// <param name="initialText">Specifies the text to be edited instead of the text of the cell. Passing the NULL value will edit the text of the cell.</param>
		internal void EditCell(Cell cell, eTreeAction action, string initialText)
		{
            if (cell == null || !cell.GetEnabled()) return;
			
			if(m_CellEditing)
			{
				if(!EndCellEditing(action))
					return;
			}
			
			CellEditEventArgs e = new CellEditEventArgs(cell,action,"");
			OnBeforeCellEdit(e);
			if(e.Cancel)
				return;
            // No editing on non-data nodes
            if (_DataManager != null && cell.Parent.DataKey == null) return;

            ICellEditControl editControl = GetCellEditor(cell);
            if (editControl == null) return;

            NodeOperations.EnsureVisible(cell);
            this.Update();

            TextBoxEx textBox = editControl as TextBoxEx;
            Control control = editControl as Control;

			Rectangle rCell=NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds, cell,m_NodeDisplay.Offset);
			rCell = GetScreenRectangle(rCell);
			// It is important that text is assigned first or textbox will be resized to different size otherwise
            editControl.CurrentValue = cell.Text;
			editControl.EditWordWrap=cell.WordWrap;

			Font font =CellDisplay.GetCellFont(this,cell);
			if(m_ZoomFactor!=1 && font!=null)
			{
				font=new Font(font.FontFamily, font.SizeInPoints * m_ZoomFactor);
			}
            
            control.Font = font;
            control.Location = rCell.Location;
            control.Size = rCell.Size;
            control.Visible = true;
            control.Focus();

            if (textBox != null)
            {
                //if initial text is null then follow default behaviour of selecting all text
                //if initial text is not null, set that as the default text and set the caret to the end of the text
                if (initialText == null)
                {
                    textBox.SelectAll();
                }
                else
                {
                    textBox.Text = initialText;
                    textBox.Select(textBox.Text.Length, 0);
                }
            }

            PrepareCellEditor(cell, editControl);
			
			cell.Parent.SetEditing(true);
			m_EditedCell=cell;
			m_CellEditing=true;
            _CellEditControl = editControl;
            editControl.BeginEdit();
		}

        /// <summary>
        /// Called just before cell editor is released for editing.
        /// </summary>
        /// <param name="cell">Reference to the cell being edited.</param>
        /// <param name="editControl">Reference to the editor control.</param>
        protected virtual void PrepareCellEditor(Cell cell, ICellEditControl editControl)
        {
            OnPrepareCellEditorControl(new PrepareCellEditorEventArgs(cell, editControl));
        }

        protected virtual void OnPrepareCellEditorControl(PrepareCellEditorEventArgs e)
        {
            PrepareCellEditorEventHandler h = PrepareCellEditorControl;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Raises the ProvideCustomCellEditor event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnProvideCustomCellEditor(CustomCellEditorEventArgs e)
        {
            CustomCellEditorEventHandler handler = ProvideCustomCellEditor;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
		/// Ends cell editing.
		/// </summary>
		/// <param name="action">Specifies which action is cause for end of the editing.</param>
		/// <returns>Returns true if edits were applied to the cell or false otherwise.</returns>
        internal bool EndCellEditing(eTreeAction action)
        {
            return EndCellEditing(action, false);
        }

        private bool _CellEditEnding = false;
		/// <summary>
		/// Ends cell editing.
		/// </summary>
		/// <param name="action">Specifies which action is cause for end of the editing.</param>
		/// <returns>Returns true if edits were applied to the cell or false otherwise.</returns>
        internal bool EndCellEditing(eTreeAction action, bool cancelEdit)
        {
            Cell editedCell = m_EditedCell;
            if (editedCell == null || _CellEditControl == null)
            {
                m_CellEditing = false;
                return true;
            }
            ICellEditControl editControl = _CellEditControl;
            Control control = editControl as Control;

            string text = editControl.CurrentValue.ToString();

            CellEditEventArgs e = new CellEditEventArgs(editedCell, action, text);
            InvokeCellEditEnding(e);
            if (e.Cancel)
                return false;
            this.Focus();
            control.Visible = false;

            text = e.NewText;
            InvokeAfterCellEdit(e);
            text = e.NewText;

            if (!e.Cancel && editedCell.Text != text && !cancelEdit)
            {
                if (this.DesignMode && editedCell.Parent.Cells[0] == editedCell)
                    TypeDescriptor.GetProperties(editedCell.Parent)["Text"].SetValue(editedCell.Parent, text);
                else
                    TypeDescriptor.GetProperties(editedCell)["Text"].SetValue(editedCell, text);
                this.BeginUpdate();
                this.RecalcLayout();
                this.EndUpdate();
            }

            editedCell.Parent.SetEditing(false);

            // Try to apply to data-binding
            if (_DataSource != null)
            {
                object item = editedCell.Parent.DataKey;
                string fieldName = this.Columns[editedCell.Parent.Cells.IndexOf(editedCell)].DataFieldName;
                if (item != null)
                {
                    IEditableObject editableObject = item as IEditableObject;
                    PropertyDescriptor descriptor;
                    if (_DataManager != null)
                        descriptor = _DataManager.GetItemProperties().Find(fieldName, true);
                    else
                        descriptor = TypeDescriptor.GetProperties(item).Find(fieldName, true);

                    if (descriptor != null)
                    {
                        if (_DataManager != null && _DataManager.Position != editedCell.Parent.BindingIndex)
                            _DataManager.Position = editedCell.Parent.BindingIndex;
                        if (editableObject != null)
                            editableObject.BeginEdit();

                        descriptor.SetValue(item, text);

                        if (editableObject != null)
                            editableObject.EndEdit();
                    }
                }
            }

            m_EditedCell = null;
            m_CellEditing = false;

            OnAfterCellEditComplete(e);

            if (editControl != m_EditTextBox)
            {
                this.Controls.Remove(control);
                editControl.EditComplete -= new EventHandler(EditControlEndEdit);
                editControl.CancelEdit -= new EventHandler(EditControlCancelEdit);
            }
            editControl.EndEdit();

            return true;
        }

        /// <summary>
        /// Raises the AfterCellEditComplete event.
        /// </summary>
        /// <param name="e">Provides information about event.</param>
        protected virtual void OnAfterCellEditComplete(CellEditEventArgs e)
        {
            CellEditEventHandler eh = AfterCellEditComplete;
            if (eh != null) eh(this, e);
        }

		/// <summary>
		/// Cancels the cell editing if it is in progress.
		/// </summary>
		/// <param name="action">Specifies which action is cause for canceling of editing.</param>
		internal void CancelCellEdit(eTreeAction action)
		{
			if(m_EditedCell==null)
				return;

            if (m_EditTextBox != null)
                m_EditTextBox.Text = m_EditedCell.Text;
            else
            {
                ICellEditControl editControl = _CellEditControl;
                if (editControl != null)
                    editControl.CurrentValue = m_EditedCell.Text;
            }
			this.EndCellEditing(action, true);		
		}

        private ICellEditControl GetCellEditor(Cell cell)
        {
            eCellEditorType editorType = cell.GetEffectiveEditorType();
            ICellEditControl editor = null;

            if (editorType == eCellEditorType.Default)
            {
                editor = GetTextBoxEditor();
                if (editor is TextBox)
                {
                    ColumnHeader header = NodeOperations.GetCellColumnHeader(this, cell);
                    if (header != null)
                        ((TextBox)editor).MaxLength = header.MaxInputLength;
                    else
                        ((TextBox)editor).MaxLength = 0;
                }
            }
#if FRAMEWORK20
            else if (editorType == eCellEditorType.NumericInteger)
            {
                IntegerCellEditor edit = new IntegerCellEditor();
                edit.ShowUpDown = true;
                editor = edit;
            }
            else if (editorType == eCellEditorType.NumericDouble)
            {
                DoubleCellEditor edit = new DoubleCellEditor();
                edit.ShowUpDown = true;
                editor = edit;
            }
            else if (editorType == eCellEditorType.NumericCurrency)
            {
                DoubleCellEditor edit = new DoubleCellEditor();
                edit.DisplayFormat = "C";
                edit.ShowUpDown = true;
                editor = edit;
            }
#endif
            else if (editorType == eCellEditorType.Custom)
            {
                CustomCellEditorEventArgs e = new CustomCellEditorEventArgs(cell);
                OnProvideCustomCellEditor(e);
                if (e.EditControl == null)
                    throw new ArgumentNullException("CustomCellEditorEventArgs.EditControl must be set to the custom cell editor.");
                
                editor = e.EditControl;
            }

            if(editor==null)
                throw new NotImplementedException("Editor type " + editorType.ToString() + " not implemented.");

            if (((Control)editor).Parent != this)
            {
                this.Controls.Add((Control)editor);
                editor.EditComplete += new EventHandler(EditControlEndEdit);
                editor.CancelEdit += new EventHandler(EditControlCancelEdit);
            }

            return editor;
        }

		private TextBoxEx GetTextBoxEditor()
		{
            if (m_EditTextBox == null)
            {
                m_EditTextBox = new TextBoxEx();
                m_EditTextBox.Name = "DefaultCellEditor";
                m_EditTextBox.AutoSize = false;
                m_EditTextBox.PreventEnterBeep = true;
                this.Controls.Add(m_EditTextBox);
                m_EditTextBox.EditComplete += new EventHandler(EditControlEndEdit);
                m_EditTextBox.CancelEdit += new EventHandler(EditControlCancelEdit);
            }
			return m_EditTextBox;
		}

		private void EditControlEndEdit(object sender, EventArgs e)
		{
			if(m_EditedCell==null)
				return;

			this.EndCellEditing(eTreeAction.Keyboard);	
		}

		private void EditControlCancelEdit(object sender, EventArgs e)
		{
			CancelCellEdit(eTreeAction.Keyboard);
		}

		private void UpdateTreeCursor()
		{
			if(m_MouseOverCell!=null)
			{
				if(m_MouseOverCell.Cursor!=null && this.Cursor!=m_MouseOverCell.Cursor)
				{
					if(m_OriginalCursor==null)
						m_OriginalCursor=this.Cursor;
					this.Cursor=m_MouseOverCell.Cursor;
				}
				else if(m_DefaultCellCursor!=null && this.Cursor!=m_DefaultCellCursor)
				{
					if(m_OriginalCursor==null)
						m_OriginalCursor=this.Cursor;
					this.Cursor=m_DefaultCellCursor;
				}
			}
			else if(m_OriginalCursor!=null)
			{
				this.Cursor=m_OriginalCursor;
				m_OriginalCursor=null;
			}
		}

		private Cell GetCellAt(Node node, int x, int y, Point offset)
		{
			Cell cellAt=null;
			Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,node,offset);
			foreach(Cell cell in node.Cells)
			{
                if (!cell.IsVisible) continue;
                Rectangle rCell = cell.BoundsRelative;
                //rCell.Offset(offset);
                rCell.Offset(r.Location);
				if(rCell.Contains(x,y))
				{
					cellAt=cell;
					break;
				}
			}
			return cellAt;
		}

		private bool OnNodeMouseMove(Node node, MouseEventArgs e, Point offset)
		{
			Point mousePos = GetLayoutPosition(e);
			
			InvokeNodeMouseMove(new TreeNodeMouseEventArgs(node, e.Button, e.Clicks, e.Delta, mousePos.X, mousePos.Y));

            Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandHitTestBounds, node, offset);
			bool bUpdate=false;
			if(r.Contains(mousePos) && node.ExpandVisibility != eNodeExpandVisibility.Hidden)
			{
				node.MouseOverNodePart=eMouseOverNodePart.Expand;
				if(m_MouseOverCell!=null)
				{
					bUpdate|=SetMouseOverCell(null);
				}
				bUpdate=true;
			}
            else if (node.MouseOverNodePart == eMouseOverNodePart.Expand)
            {
                node.MouseOverNodePart = eMouseOverNodePart.Node;
                bUpdate = true;
            }

			r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CommandBounds,node,offset);
			if(r.Contains(mousePos))
			{
				node.MouseOverNodePart=eMouseOverNodePart.Command;
				if(m_MouseOverCell!=null)
				{
					bUpdate|=SetMouseOverCell(null);
				}
				bUpdate=true;
			}
            else if (node.MouseOverNodePart != eMouseOverNodePart.Expand)
			{
				if(node.MouseOverNodePart!=eMouseOverNodePart.Node)
				{
					node.MouseOverNodePart=eMouseOverNodePart.Node;
					bUpdate=true;
				}
				Cell cell=GetCellAt(node,mousePos.X,mousePos.Y,offset);

				if(cell!=null)
				{
					bUpdate|=SetMouseOverCell(cell);
				}
			}
			
			if(bUpdate)
				InvalidateNode(node);
			return bUpdate;
		}
//
//		private class MouseOverInfo
//		{
//			public DevComponents.AdvTree.Node Node=null;
//			public DevComponents.AdvTree.Cell Cell=null;
//			MouseOverInfo()
//			{
//				this.Node=null;
//				this.Cell=null;
//			}
//		}

		private void OnSelectionBoxChanged()
		{
			if(this.SelectedNode!=null)
			{
				InvalidateNode(this.SelectedNode);
				this.Update();
			}
		}

        //private Color GetDefaultSelectionBoxBorderColor()
        //{
        //    return Color.FromArgb(96,SystemColors.Highlight);
        //}

        //private Color GetDefaultSelectionBoxFillColor()
        //{
        //    return Color.FromArgb(64,SystemColors.Highlight);
        //}

		private void OnCommandButtonChanged()
		{
            _LayoutSettings.CommandAreaWidth = m_CommandWidth;
			this.RecalcLayout();
		}

		private void InvokeCellEditEnding(CellEditEventArgs e)
		{
			if(CellEditEnding!=null)
				CellEditEnding(this,e);
		}

		private void InvokeAfterCellEdit(CellEditEventArgs e)
		{
			if(AfterCellEdit!=null)
				AfterCellEdit(this,e);
		}
		
		/// <summary>
		/// Raises BeforeNodeInsert event
		/// </summary>
		/// <param name="node">Node that is about to be inserted</param>
		/// <param name="action">Source of the event</param>
		internal protected virtual void InvokeBeforeNodeInsert(eTreeAction action, Node node, Node parentNode)
		{
			if(BeforeNodeInsert!=null)
			{
				TreeNodeCollectionEventArgs e=new TreeNodeCollectionEventArgs(action, node, parentNode);
				BeforeNodeInsert(this, e);
			}
		}
		
		/// <summary>
		/// Raises AfterNodeInsert event
		/// </summary>
		/// <param name="node">Node that is inserted</param>
		/// <param name="action">Source of the event</param>
		internal protected virtual void InvokeAfterNodeInsert(eTreeAction action, Node node, Node parentNode)
		{
			if(AfterNodeInsert!=null)
			{
				TreeNodeCollectionEventArgs e=new TreeNodeCollectionEventArgs(action, node, parentNode);
				AfterNodeInsert(this, e);
			}
		}
		
		/// <summary>
		/// Raises BeforeNodeRemove event
		/// </summary>
		/// <param name="node">Node that is about to be removed</param>
		/// <param name="action">Source of the event</param>
		internal protected virtual void InvokeBeforeNodeRemove(eTreeAction action, Node node, Node parentNode)
		{
			if(BeforeNodeRemove!=null)
			{
				TreeNodeCollectionEventArgs e=new TreeNodeCollectionEventArgs(action, node, parentNode);
				BeforeNodeRemove(this, e);
			}
		}
		
		/// <summary>
		/// Raises AfterNodeRemove event
		/// </summary>
		/// <param name="node">Node that is removed</param>
		/// <param name="action">Source of the event</param>
		internal protected virtual void InvokeAfterNodeRemove(eTreeAction action, Node node, Node parentNode)
		{
			if(AfterNodeRemove!=null)
			{
				TreeNodeCollectionEventArgs e=new TreeNodeCollectionEventArgs(action, node, parentNode);
				AfterNodeRemove(this, e);
			}
		}
		
		/// <summary>
		/// Called after node has been removed
		/// </summary>
		/// <param name="node">Node that is removed</param>
		/// <param name="action">Source of the event</param>
		internal protected virtual void NodeRemoved(eTreeAction action, Node node, Node parentNode, int indexOfRemovedNode)
		{
			InvokeAfterNodeRemove(action, node, parentNode);

            if(m_NodeDisplay!=null) m_NodeDisplay.PaintedNodes.Clear();

            if (!this.IsDisposed)
                RecalcLayout();

			if(!this.IsUpdateSuspended)
			{
                bool updateSelection = false;
                if (node.IsSelected)
                    updateSelection = true;
                else
                {
                    Node n = this.SelectedNode;
                    while (n != null)
                    {
                        if (n != null && n == node)
                        {
                            updateSelection = true;
                            break;
                        }
                        n = n.Parent;
                    }
                }

				if(updateSelection)
				{
                    Node refNode = parentNode;
                    bool selectFirst = true;
                    if (parentNode != null)
                    {
                        if (parentNode.Nodes.Count > 0)
                        {
                            if (indexOfRemovedNode >= parentNode.Nodes.Count)
                                indexOfRemovedNode = parentNode.Nodes.Count - 1;
                            refNode = parentNode.Nodes[indexOfRemovedNode];
                            if (refNode.CanSelect)
                            {
                                this.SelectNode(refNode, action);
                                selectFirst = false;
                                refNode = null;
                            }
                        }
                        else
                        {
                            if (refNode.CanSelect)
                            {
                                this.SelectNode(refNode, action);
                                selectFirst = false;
                                refNode = null;
                            }
                        }

                        if (refNode != null)
                        {
                            while (refNode != null)
                            {
                                refNode = NodeOperations.GetPreviousVisibleNode(refNode);
                                if (refNode != null && refNode.CanSelect)
                                {
                                    this.SelectNode(refNode, action);
                                    selectFirst = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        int startIndex = indexOfRemovedNode;
                        if (this.Nodes.Count == indexOfRemovedNode) startIndex--;
                        if (startIndex > 0 && this.Nodes.Count > startIndex)
                        {
                            for (int i = startIndex; i >= 0; i--)
                            {
                                refNode = this.Nodes[i];
                                if (refNode.CanSelect && refNode.Visible && refNode.Enabled)
                                {
                                    this.SelectNode(refNode, action);
                                    selectFirst = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (selectFirst && !SelectFirstNode(action))
					    this.SelectNode(null, action);
				}
				this.Invalidate();
			}
		}

        private bool SelectFirstNode(eTreeAction action)
        {
            bool selected = false;
            foreach (Node node in this.Nodes)
            {
                if (node.Selectable && node.Visible && node.Enabled)
                {
                    this.SelectNode(node, action);
                    if (this.SelectedNode == node)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
		
		/// <summary>
		/// Invokes BeforeNodeDrop event. If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeBeforeNodeDrop(TreeDragDropEventArgs e)
		{
			if(BeforeNodeDrop!=null)
				BeforeNodeDrop(this, e);
		}
		
		/// <summary>
		/// Invokes AfterNodeDrop event. If overridden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeAfterNodeDrop(TreeDragDropEventArgs e)
		{
			if(AfterNodeDrop!=null)
				AfterNodeDrop(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseDown event. If overridden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseDown(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseDown(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
			
			if(NodeMouseDown!=null)
				NodeMouseDown(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseUp event. If overridden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseUp(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseUp(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
			
			if(NodeMouseUp!=null)
				NodeMouseUp(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseMove event. If overridden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseMove(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseMove(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
			
			if(NodeMouseMove!=null)
				NodeMouseMove(this, e);
		}

        internal void InternalInvokeNodeClick(TreeNodeMouseEventArgs e)
        {
            InvokeNodeClick(e);
        }
		/// <summary>
		/// Invokes NodeClick event. If overridden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeClick(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeClick(this, e);
			
			if(NodeClick!=null)
				NodeClick(this, e);
		}
		
		/// <summary>
		/// Invokes NodeDoubleClick event. If overridden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeDoubleClick(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeDoubleClick(this, e);
			
			if(NodeDoubleClick!=null)
				NodeDoubleClick(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseEnter event.  If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseEnter(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseEnter(this, e);
			
			if(NodeMouseEnter!=null)
				NodeMouseEnter(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseLeave event.  If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseLeave(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseLeave(this, e);
			
			if(NodeMouseLeave!=null)
				NodeMouseLeave(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseHover event.  If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseHover(TreeNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseHover(this, e);
			
			if(NodeMouseHover!=null)
				NodeMouseHover(this, e);
		}
		
		private bool FireHoverEvent
		{
			get { return NodeMouseHover!=null; }
		}
		#endregion

		#region INodeNotify
		void INodeNotify.ExpandedChanged(Node node)
		{
            if(!node.Expanded)
			    ValidateSelectedNode();
			if(!this.IsUpdateSuspended)
			{
				this.RecalcLayout();
				this.Refresh();
			}
		}

		#endregion

		#region Public Interface
        private eView _View = eView.Tree;
        /// <summary>
        /// Gets or sets how control positions the items. Default value is standard TreeView layout.
        /// </summary>
        [DefaultValue(eView.Tree), Category("Appearance"), Description("Indicates how control positions the items.")]
        public eView View
        {
            get { return _View; }
            set
            {
                if (value != _View)
                {
                    eView oldValue = _View;
                    _View = value;
                    OnViewChanged(oldValue, value);
                }
            }
        }
        private void OnViewChanged(eView oldValue, eView newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("View"));
            if (newValue == eView.Tree)
            {
                m_NodeLayout = new NodeTreeLayout(this, this.ClientRectangle, _LayoutSettings);
                //m_NodeLayout.NodeVerticalSpacing = _NodeSpacing;
            }
            else if (newValue == eView.Tile)
            {
                m_NodeLayout = new NodeTileLayout(this, this.ClientRectangle, _LayoutSettings);
                //m_NodeLayout.NodeHorizontalSpacing = 8;
                //m_NodeLayout.NodeVerticalSpacing = 8;
                //m_NodeLayout.NodeVerticalSpacing = _NodeSpacing;
            }
            InvalidateNodesSize();
            RecalcLayout();
        }

        private static readonly Color DefaultTileGroupLineColor = ColorScheme.GetColor(0xE2E2E2);
        private Color _TileGroupLineColor = DefaultTileGroupLineColor;
        /// <summary>
        /// Gets or sets the color of the group divider line when in tile view.  
        /// </summary>
        [Category("Columns"), Description("Indicates color of the group divider line when in tile view.")]
        public Color TileGroupLineColor
        {
            get { return _TileGroupLineColor; }
            set 
            {
                _TileGroupLineColor = value;
                if (_View == eView.Tile)
                    this.Invalidate();
            }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTileGroupLineColor()
        {
            return !ColorFunctions.IsEqual(DefaultTileGroupLineColor, _TileGroupLineColor);
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTileGroupLineColor()
        {
            this.TileGroupLineColor = DefaultTileGroupLineColor;
        }

		/// <summary>
		/// Save nodes to XmlDocument. New Node AdvTree is created and nodes are serialized into it.
		/// </summary>
		/// <param name="document">Reference to an instance of XmlDocument object</param>
		public void Save(XmlDocument document)
		{
			TreeSerializer.Save(this, document);
		}
		
		/// <summary>
		/// Saves nodes to a file.
		/// </summary>
		/// <param name="fileName">File name to save nodes to.</param>
		public void Save(string fileName)
		{
			TreeSerializer.Save(this, fileName);
		}
		
		/// <summary>
		/// Saves nodes to specified stream.
		/// </summary>
		/// <param name="outStream">Stream to save nodes to.</param>
		public void Save(Stream outStream)
		{
			TreeSerializer.Save(this, outStream);
		}
		
		/// <summary>
		/// Saves nodes to specified writer.
		/// </summary>
		/// <param name="writer">Writer to save nodes to.</param>
		public void Save(TextWriter writer)
		{
			TreeSerializer.Save(this, writer);
		}
		
		/// <summary>
		/// Saves nodes to specified writer.
		/// </summary>
		/// <param name="writer">Writer to save nodes to.</param>
		public void Save(XmlWriter writer)
		{
			TreeSerializer.Save(this, writer);
		}
		
		/// <summary>
		/// Load nodes from file.
		/// </summary>
		/// <param name="fileName">File to load nodes from</param>
		public void Load(string fileName)
		{
			TreeSerializer.Load(this, fileName);
		}
		
		/// <summary>
		/// Load nodes from stream.
		/// </summary>
		/// <param name="inStream">Stream to load from</param>
		public void Load(Stream inStream)
		{
			TreeSerializer.Load(this, inStream);
		}
		
		/// <summary>
		/// Load nodes from reader.
		/// </summary>
		/// <param name="reader">Reader to load from.</param>
		public void Load(XmlReader reader)
		{
			TreeSerializer.Load(this, reader);
		}
		
		/// <summary>
		/// Load nodes from reader.
		/// </summary>
		/// <param name="reader">Reader to load from.</param>
		public void Load(TextReader reader)
		{
			TreeSerializer.Load(this, reader);
		}
		
		/// <summary>
		/// Load nodes from an XmlDocument object.
		/// </summary>
		/// <param name="document">Document to load Nodes from.</param>
		public void Load(XmlDocument document)
		{
			TreeSerializer.Load(this, document);
		}
		
		/// <summary>
		/// Forces the control to invalidate its client area and immediately redraw itself
		/// and any child controls. Note however that this method will node do anything if refresh
		/// is suspended as result of call to BeginUpdate method without corresponding EndUpdate
		/// call or if SuspendPaint property is set to true.
		/// </summary>
		public override void Refresh()
		{
			if(!this.IsUpdateSuspended && !this.SuspendPaint)
				base.Refresh();
		}
		
        ///// <summary>
        ///// Sets the node map position when tree is in Map layout mode. The node's position
        ///// can be set only for the sub-root nodes, i.e. nodes that are parented directly to
        ///// top-level root node. Setting map position for any other node does not have any effect.
        ///// </summary>
        ///// <remarks>
        ///// 	<para>Note that setting map position explicitly can change the position for other
        /////     nodes that are on the same level as the node that you pass into this method. Since
        /////     Map mode layouts the nodes clock-wise, setting the node position to Near will cause
        /////     all nodes that are in collection <strong>after</strong> the reference node to be
        /////     positioned Near as well.</para>
        ///// 	<para>Similarly, setting the node position to Far will cause all nodes that are in
        /////     collection <strong>before</strong> the reference node to be positioned Far as
        /////     well.</para>
        ///// </remarks>
        ///// <param name="node">Sub-root node to set layout position for.</param>
        ///// <param name="position">The position relative to the root node should take</param>
        //public void SetNodeMapPosition(Node node, eMapPosition position)
        //{
        //    if(node==null || node.Parent==null)
        //        return;
            
        //    if(position==eMapPosition.Default)
        //    {
        //        node.SetMapSubRootPosition(position);
        //    }
        //    else if(position==eMapPosition.Near)
        //    {
        //        int start=node.Parent.Nodes.IndexOf(node);
        //        int end=node.Parent.Nodes.Count;
        //        for(int i=start;i<end;i++)
        //            node.Parent.Nodes[i].SetMapSubRootPosition(position);
        //    }
        //    else if(position==eMapPosition.Far)
        //    {
        //        int start=node.Parent.Nodes.IndexOf(node);
        //        for(int i=start;i>=0;i--)
        //            node.Parent.Nodes[i].SetMapSubRootPosition(position);
        //    }
        //}

        private void ProcessSortRequests()
        {
            if (_TopLevelSortRequest)
            {
                _TopLevelSortRequest = false;
                if (m_Columns.IsSorted)
                    m_Columns.UpdateSort();
            }
            if (_SortRequestNodes.Count > 0)
            {
                foreach (Node sortRequestNode in _SortRequestNodes)
                {
                    if (sortRequestNode.NodesColumns.IsSorted)
                        sortRequestNode.NodesColumns.UpdateSort();
                }
                _SortRequestNodes.Clear();
            }
        }

        private List<Node> _SortRequestNodes = new List<Node>();
        private bool _TopLevelSortRequest = false;
        internal void PushSortRequest()
        {
            PushSortRequest(null);
        }
        internal void PushSortRequest(Node parentNode)
        {
            if (parentNode == null)
                _TopLevelSortRequest = true;
            else if (!_SortRequestNodes.Contains(parentNode))
                _SortRequestNodes.Add(parentNode);
        }

		/// <summary>
		/// Disables any redrawing of the tree control. To maintain performance while items
		/// are added one at a time to the control, call the BeginUpdate method. The BeginUpdate
		/// method prevents the control from painting until the
		/// <see cref="EndUpdate">EndUpdate</see> method is called.
		/// </summary>
		public void BeginUpdate()
		{
			m_UpdateSuspended++;
		}

		/// <summary>
		/// Enables the redrawing of the tree view. To maintain performance while items are
		/// added one at a time to the control, call the <see cref="BeginUpdate">BeginUpdate</see>
		/// method. The BeginUpdate method prevents the control from painting until the EndUpdate
		/// method is called.
		/// </summary>
		/// <remarks>
		/// Call to EndUpdate will enable the layout and painting in tree control. If there
		/// are any pending layouts the EndUpdate will call
		/// <see cref="RecalcLayout">RecalcLayout</see> method to perform the layout and it will
		/// repaint the control.
		/// </remarks>
		public void EndUpdate()
		{
			EndUpdate(true);
		}
		
		/// <summary>
		/// Enables the redrawing of the tree view. To maintain performance while items are
		/// added one at a time to the control, call the <see cref="BeginUpdate">BeginUpdate</see>
		/// method. The BeginUpdate method prevents the control from painting until the EndUpdate
		/// method is called.
		/// </summary>
		/// <param name="performLayoutAndRefresh">Gets or sets whether layout and refresh of control is performed if there are no other update blocks pending.</param>
		public void EndUpdate(bool performLayoutAndRefresh)
		{
			if(m_UpdateSuspended>0) m_UpdateSuspended--;
			if(m_UpdateSuspended==0 && performLayoutAndRefresh)
			{
				this.RecalcLayout();
				this.Invalidate(true);
			}
		}

		/// <summary>
		/// Retrieves the tree node that is at the specified location.
		/// </summary>
		/// <returns>The Node at the specified point, in tree view coordinates.</returns>
		/// <remarks>
		/// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
		///     MouseDown event as the x and y parameters.</para>
		/// </remarks>
		/// <param name="p">The Point to evaluate and retrieve the node from.</param>
		public Node GetNodeAt(Point p)
		{
			return GetNodeAt(p.X,p.Y);
		}

		/// <summary>
		/// Retrieves the tree node that is at the specified location.
		/// </summary>
		/// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
		/// <remarks>
		/// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
		///     MouseDown event as the x and y parameters.</para>
		/// </remarks>
		/// <param name="x">The X position to evaluate and retrieve the node from.</param>
		/// <param name="y">The Y position to evaluate and retrieve the node from.</param>
		public Node GetNodeAt(int x, int y)
		{
			return NodeOperations.GetNodeAt(this, x, y).NodeAt;
		}

        /// <summary>
        /// Retrieves the tree node that is at the specified location.
        /// </summary>
        /// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
        /// <remarks>
        /// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
        ///     MouseDown event as the x and y parameters.</para>
        /// </remarks>
        /// <param name="x">The X position to evaluate and retrieve the node from.</param>
        /// <param name="y">The Y position to evaluate and retrieve the node from.</param>
        /// <param name="displayedOnly">Whether to enumerated displayed nodes only.</param>
        public Node GetNodeAt(int x, int y, bool displayedOnly)
        {
            return NodeOperations.GetNodeAt(this, x, y, displayedOnly).NodeAt;
        }

        /// <summary>
        /// Retrieves the tree node that is at the specified vertical location.
        /// </summary>
        /// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
        /// <remarks>
        /// 	<para>You can pass the MouseEventArgs.Y coordinates of the
        ///     MouseDown event as the y parameter.</para>
        /// </remarks>
        /// <param name="y">The Y position to evaluate and retrieve the node from.</param>
        public Node GetNodeAt(int y)
        {
            return NodeOperations.GetNodeAt(this, y).NodeAt;
        }
		
		/// <summary>
		/// Retrieves the node cell that is at the specified location.
		/// </summary>
		/// <param name="p">The Point to evaluate and retrieve the cell from.</param>
		/// <returns>The Cell at the specified point, in tree view coordinates.</returns>
		public Cell GetCellAt(Point p)
		{
			return GetCellAt(p.X, p.Y);
		}
		
		/// <summary>
		/// Retrieves the node cell that is at the specified location.
		/// </summary>
		/// <param name="x">The X position to evaluate and retrieve the cell from.</param>
		/// <param name="y">The Y position to evaluate and retrieve the cell from.</param>
		/// <returns>The Cell at the specified point, in tree view coordinates.</returns>
		public Cell GetCellAt(int x, int y)
		{
            return GetCellAt(x, y, false);
		}

        /// <summary>
        /// Retrieves the node cell that is at the specified location.
        /// </summary>
        /// <param name="x">The X position to evaluate and retrieve the cell from.</param>
        /// <param name="y">The Y position to evaluate and retrieve the cell from.</param>
        /// <param name="displayedOnly">Whether to enumerated displayed nodes only.</param>
        /// <returns>The Cell at the specified point, in tree view coordinates.</returns>
        public Cell GetCellAt(int x, int y, bool displayedOnly)
        {
            Node node = GetNodeAt(x, y, displayedOnly);
            if (node != null)
            {
                return GetCellAt(node, x, y, m_NodeDisplay.Offset);
            }
            return null;
        }

		/// <summary>
		/// Returns the reference to the node mouse is currently over or null (Nothing) if mouse is not over any node in tree.
		/// </summary>
		[Browsable(false)]
		public Node MouseOverNode
		{
			get
			{
				return m_MouseOverNode;
			}
		}

		/// <summary>
		/// Specifies the mouse cursor displayed when mouse is over the cell. Default value
		/// is null which means that default control cursor is used.
		/// </summary>
		/// <remarks>
		/// To specify cursor for each individual cell use
		/// <a href="AdvTree~DevComponents.AdvTree.Cell~Cursor.html">Cell.Cursor</a> property.
		/// </remarks>
		[Browsable(true),DefaultValue(null),Category("Appearance"),Description("Specifies the default mouse cursor displayed when mouse is over the cell.")]
		public Cursor DefaultCellCursor
		{
			get
			{
				return m_DefaultCellCursor;
			}
			set
			{
				if(m_DefaultCellCursor!=value)
				{
					m_DefaultCellCursor=value;
				}
			}
		}

		/// <summary>Applies any layout changes to the tree control.</summary>
		/// <remarks>
		/// Layout will not be performed if BeginUpdate is called. Any calls to the
		/// RecalcLayout will return without executing requested layout operation.
		/// </remarks>
		public void RecalcLayout()
		{
			if(this.IsUpdateSuspended)
			{
				m_PendingLayout=true;
				return;
			}
			RecalcLayoutInternal();
		}

        protected override void OnFontChanged(EventArgs e)
        {
            InvalidateNodesSize();
            RecalcLayout();
            base.OnFontChanged(e);
        }

		/// <summary>
		/// Gets reference to array of Cell objects that have HostedControl property set.
		/// </summary>
		internal ArrayList HostedControlCells
		{
			get { return m_HostedControlCells;}
		}

        private bool _FirstLayout = true;
        private delegate void NoArgumentsDelegate();
		/// <summary>
		/// Recalculates layout for the tree control. Not affected by BeginUpdate call.
		/// </summary>
		internal void RecalcLayoutInternal()
		{
            if (this.Bounds.IsEmpty || this.IsDisposed) return;
            if(this.InvokeRequired)
            {
                this.Invoke(new NoArgumentsDelegate(RecalcLayoutInternal));
                return;
            }
            
            _FirstLayout = false;

            // Process any pending sorting requests first
            ProcessSortRequests();

		    Rectangle clientArea = GetInnerRectangle();
            if ((m_Columns.Count > 0 && m_Columns.UsesRelativeSize || _View == eView.Tile) && _VScrollBar != null)
                clientArea.Width -= _VScrollBar.Width;
            Rectangle controlRect = clientArea;

            if (this.SelectionBoxStyle == eSelectionStyle.NodeMarker)
                clientArea.Inflate(-this.SelectionBoxSize, -this.SelectionBoxSize);
            else
            {
                if (this.DisplayRootNode != null && !this.DisplayRootNode.Selectable || this.Nodes.Count > 0 && !this.Nodes[0].Selectable)
                    clientArea.Height -= 2;
                else
                    clientArea.Inflate(-2, -2);
            }
            m_NodeLayout.ClientArea = clientArea;
			m_NodeLayout.LeftRight=this.RtlTranslateLeftRight(LeftRightAlignment.Left);
			m_NodeLayout.PerformLayout();
			m_PendingLayout=false;
			
			float zoom = m_ZoomFactor;
			Rectangle screenRect=GetScreenRectangle(new Rectangle(0,0,m_NodeLayout.Width,m_NodeLayout.Height));
			Size nodeLayoutSize=screenRect.Size;
            if (nodeLayoutSize.Width > controlRect.Width || nodeLayoutSize.Height > controlRect.Height)
			{
				Size autoScrollMinSize =nodeLayoutSize;
                if (this.SelectionBoxStyle == eSelectionStyle.NodeMarker)
                {
                    autoScrollMinSize.Width += this.SelectionBoxSize * 2;
                    autoScrollMinSize.Height += this.SelectionBoxSize * 2;
                }
                else
                {
                    //autoScrollMinSize.Width += 2;
                    autoScrollMinSize.Height += 2;
                }
                Rectangle inner = clientArea; //GetInnerRectangle();
                
                if (nodeLayoutSize.Height > inner.Height && _VScrollBarVisible || 
                    nodeLayoutSize.Width > inner.Width && nodeLayoutSize.Height > inner.Height - SystemInformation.HorizontalScrollBarHeight)
                    autoScrollMinSize.Width += SystemInformation.VerticalScrollBarWidth;
                
                if(_HScrollBarVisible)
                    autoScrollMinSize.Height += SystemInformation.HorizontalScrollBarHeight;

				if(!this.AutoScroll)
				{
					this.BeginUpdate();
					this.Invalidate();
					this.AutoScroll=true;
					this.AutoScrollMinSize = autoScrollMinSize;
					this.AutoScrollPosition=m_NodeDisplay.DefaultOffset;
                    if (m_Columns.Count > 0 && m_Columns.UsesRelativeSize || _View == eView.Tile)
                    {
                        InvalidateNodesSize();
                        SetPendingLayout();
                    }
					this.EndUpdate(false);
				}
                else if (this.AutoScrollMinSize != autoScrollMinSize)
				{
					this.BeginUpdate();
					this.AutoScrollMinSize = autoScrollMinSize;
                    if (nodeLayoutSize.Width <= this.Bounds.Width && this.AutoScrollPosition.X != 0)
                    {
                        this.AutoScrollPosition = new Point(0, this.AutoScrollPosition.Y);
                    }
                    Point asp = this.AutoScrollPosition;
                    if (Math.Abs(asp.Y) > autoScrollMinSize.Height)
                    {
                        asp.Y = -(autoScrollMinSize.Height - inner.Height);
                    }
                    else if (nodeLayoutSize.Height <= this.Bounds.Height)
                        asp.Y = 0;

                    if (Math.Abs(asp.X) > autoScrollMinSize.Width)
                    {
                        asp.X = -(autoScrollMinSize.Width - inner.Width);
                    }
                    else if (nodeLayoutSize.Width <= this.Bounds.Width)
                        asp.X = 0;
                    if (this.AutoScrollPosition != asp) this.AutoScrollPosition = asp;
					this.EndUpdate(false);
				}
			}
			else if(this.AutoScroll)
			{
				this.BeginUpdate();
                bool updateColumnsWidth = false;
                if (m_Columns.Count > 0 && _VScrollBar != null)
                {
                    if (m_Columns.UsesRelativeSize)
                        InvalidateNodesSize();
                    else
                        updateColumnsWidth = true;
                }

				this.AutoScroll=false;
				m_NodeDisplay.Offset=m_NodeDisplay.DefaultOffset;
                if (updateColumnsWidth) m_NodeLayout.UpdateTopLevelColumnsWidth();
				this.EndUpdate(false);
			}
			else
				m_NodeDisplay.Offset=m_NodeDisplay.DefaultOffset;

            if (_InvalidControlBorder)
                UpdateControlBorderPanel();
			RepositionHostedControls(true);
            RepositionColumnHeader();
            UpdateScrollBars();
			this.Invalidate(true);
		}

        private bool _ClipHostedControls = true;
        /// <summary>
        /// Gets or sets whether hosted controls are clipped so they don't overlap the control borders. Default value is true.
        /// </summary>
        [Browsable(false), DefaultValue(true)]
        public bool ClipHostedControls
        {
            get { return _ClipHostedControls; }
            set
            {
                _ClipHostedControls = value;
                UpdateControlBorderPanel();
            }
        }

        internal bool IsRepositioningControls
        {
            get
            {
                return _IsRepositioningControls;
            }
        }
        private bool _IsRepositioningControls = false;
        private PanelControl _ControlBorderPanel = null;
        private void RemoveControlBorderPanel()
        {
            if (_ControlBorderPanel == null) return;

            if (_IsLayoutOrResize > 0)
            {
                if (_IsUpdateControlBorderPanelPending) return;

                // Delay this due to http://support.microsoft.com/kb/949458 bug in WinForms and MDI-Child Forms
                _IsUpdateControlBorderPanelPending = true;
                InvokeDelayed(new MethodInvoker(delegate { this.RemoveControlBorderPanel(); }));
                return;
            }
            _IsUpdateControlBorderPanelPending = false;

            PanelControl control = _ControlBorderPanel;
            _ControlBorderPanel = null;
            this.Controls.Remove(control);
            control.Dispose();
        }

        private bool _InvalidControlBorder = true;
        /// <summary>
        /// Gets or sets whether control border needs to be updated by calling UpdateControlBorderPanel()
        /// </summary>
        internal bool InvalidControlBorder
        {
            get { return _InvalidControlBorder; }
            set
            {
                _InvalidControlBorder = value;
            }
        }
        private bool _IsUpdateControlBorderPanelPending = false;
        private void UpdateControlBorderPanel()
        {
            if (!_ClipHostedControls)
            {
                RemoveControlBorderPanel();
                return;
            }
            if (m_HostedControlCells.Count > 0)
            {
                Rectangle innerRect = GetInnerRectangle();
                Rectangle cr = this.ClientRectangle;
                if (cr.Contains(innerRect) && innerRect != cr &&
                    innerRect.X > cr.X && innerRect.Right < cr.Right && innerRect.Y > cr.Y && innerRect.Bottom < cr.Bottom)
                {
                    if (_ControlBorderPanel == null)
                    {
                        if (_IsLayoutOrResize > 0)
                        {
                            if (_IsUpdateControlBorderPanelPending) return;

                            // Delay this due to http://support.microsoft.com/kb/949458 bug in WinForms and MDI-Child Forms
                            _IsUpdateControlBorderPanelPending = true;
                            InvokeDelayed(new MethodInvoker(delegate { this.UpdateControlBorderPanel(); }));
                            return;
                        }
                        _IsUpdateControlBorderPanelPending = false;
                        _ControlBorderPanel = new PanelControl();
                        _ControlBorderPanel.CanSetRegion = false;
                        _ControlBorderPanel.Style.ApplyStyle(this.BackgroundStyle);
                        _ControlBorderPanel.Style.Class = this.BackgroundStyle.Class;
                        this.Controls.Add(_ControlBorderPanel);
                    }
                    _ControlBorderPanel.Bounds = cr;
                    Region region = new Region(cr);
                    region.Exclude(innerRect);
                    _ControlBorderPanel.Region = region;
                    _ControlBorderPanel.BringToFront();
                }
                else
                {
                    RemoveControlBorderPanel();
                }
            }
            else
                RemoveControlBorderPanel();
            _InvalidControlBorder = false;
        }
        private void RepositionHostedControls(bool performLayout)
		{
            if (_IsRepositioningControls) return;
            try
            {
                _IsRepositioningControls = true;
                if (m_HostedControlCells.Count > 0)
                {
                    this.SuspendLayout();
                    if (this.AutoScroll)
                        m_NodeDisplay.Offset = GetAutoScrollPositionOffset();
                    m_NodeDisplay.MoveHostedControls();

                    this.ResumeLayout(performLayout);
                }
                else if (_ControlBorderPanel != null)
                {
                    RemoveControlBorderPanel();
                }

                if (m_CellEditing && _CellEditControl is Control && m_EditedCell != null)
                {
                    Point offset = m_NodeDisplay.Offset;
                    if (this.AutoScroll)
                        offset = GetAutoScrollPositionOffset();
                    Control editControl = (Control)_CellEditControl;
                    Rectangle rCell = NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds, m_EditedCell, offset);
                    rCell = GetScreenRectangle(rCell);
                    editControl.Location = rCell.Location;
                }

                
            }
            finally
            {
                _IsRepositioningControls = false;
            }
		}

        /// <summary>
        /// Gets the reference to internal vertical scroll-bar control if one is created or null if no scrollbar is visible.
        /// </summary>
        [Browsable(false)]
        public DevComponents.DotNetBar.VScrollBarAdv VScrollBar
        {
            get
            {
                return _VScrollBar;
            }
        }

        private bool _VScrollBarVisible = true;
        /// <summary>
        /// Gets or sets whether Vertical Scroll-bar is shown if needed because content of the control exceeds available height. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether Vertical Scroll-bar is shown if needed because content of the control exceeds available height.")]
        public bool VScrollBarVisible
        {
            get { return _VScrollBarVisible; }
            set
            {
                _VScrollBarVisible = value;
                if (!_VScrollBarVisible && _VScrollBar != null) _VScrollBar.Visible = false;
            }
        }

        private bool _HScrollBarVisible = true;
        /// <summary>
        /// Gets or sets whether Horizontal Scroll-bar is shown if needed because content of the control exceeds available width. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether Vertical Scroll-bar is shown if needed because content of the control exceeds available height.")]
        public bool HScrollBarVisible
        {
            get { return _HScrollBarVisible; }
            set
            {
                _HScrollBarVisible = value;
                if (!_HScrollBarVisible && _HScrollBar != null) _HScrollBar.Visible = false;
            }
        }
        

        /// <summary>
        /// Gets the reference to internal horizontal scroll-bar control if one is created or null if no scrollbar is visible.
        /// </summary>
        [Browsable(false)]
        public DevComponents.DotNetBar.ScrollBar.HScrollBarAdv HScrollBar
        {
            get
            {
                return _HScrollBar;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
            set { base.AutoScrollMargin = value; }
        }

        private bool _AutoScroll = false;
        /// <summary>
        /// Gets or sets a value indicating whether the tree control enables the user to scroll to any nodes placed outside of its visible boundaries.
        /// This property is managed internally by AdvTree control and should not be modified.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool AutoScroll
        {
            get { return _AutoScroll; }
            set
            {
                if (_AutoScroll != value)
                {
                    _AutoScroll = value;
                    UpdateScrollBars();
                }
            }
        }

        private Size _AutoScrollMinSize = Size.Empty;
        /// <summary>
        /// Gets or sets the minimum size of the auto-scroll. Returns a Size that represents the minimum height and width of the scrolling area in pixels.
        /// This property is managed internally by AdvTree control and should not be modified.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size AutoScrollMinSize
        {
            get { return _AutoScrollMinSize; }
            set 
            { 
                _AutoScrollMinSize = value;
                UpdateScrollBars();
            }
        }

        private Point _AutoScrollPosition = Point.Empty;
        /// <summary>
        /// Gets or sets the location of the auto-scroll position.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Indicates location of the auto-scroll position.")]
        public new Point AutoScrollPosition
        {
            get
            {
                return _AutoScrollPosition;
            }
            set
            {
                if (value.X > 0) value.X = -value.X;
                if (value.Y > 0) value.Y = -value.Y;
                if (_AutoScrollPosition != value)
                {
                    _AutoScrollPosition = value;
                    if (_AutoScroll)
                    {
                        if (_VScrollBar != null && _VScrollBar.Value != -_AutoScrollPosition.Y)
                            _VScrollBar.Value = Math.Min(_VScrollBar.Maximum, -_AutoScrollPosition.Y);
                        if (_HScrollBar != null && _HScrollBar.Value != -_AutoScrollPosition.X)
                            _HScrollBar.Value = Math.Min(_HScrollBar.Maximum, -_AutoScrollPosition.X);
                        RepositionHostedControls(false);
                        Invalidate();
                        m_NodeDisplay.Offset = Point.Empty; // Reset render offset
                        if (_ColumnHeader != null) _ColumnHeader.Invalidate();
                    }
                }
            }
        }

        private void InvokeDelayed(MethodInvoker method)
        {
            Timer delayedInvokeTimer = new Timer();
            delayedInvokeTimer = new Timer();
            delayedInvokeTimer.Tag = method;
            delayedInvokeTimer.Interval = 10;
            delayedInvokeTimer.Tick += new EventHandler(DelayedInvokeTimerTick);
            delayedInvokeTimer.Start();
        }
        void DelayedInvokeTimerTick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;
            MethodInvoker method = (MethodInvoker)timer.Tag;
            timer.Stop();
            timer.Dispose();
            method.Invoke();
        }

        private bool _IsUpdateScrollBarsPending = false;
        private void UpdateScrollBars()
        {
            if (_IsLayoutOrResize > 0)
            {
                //Form form = this.FindForm();
                //if (form != null && form.IsMdiChild)
                {
                    if (_IsUpdateScrollBarsPending) return;

                    // Delay this due to http://support.microsoft.com/kb/949458 bug in WinForms and MDI-Child Forms
                    _IsUpdateScrollBarsPending = true;
                    InvokeDelayed(new MethodInvoker(delegate { this.UpdateScrollBars(); }));
                    return;
                }
            }

            _IsUpdateScrollBarsPending = false;
            if (!_AutoScroll)
            {
                RemoveHScrollBar();
                RemoveVScrollBar();
                if (_Thumb != null)
                {
                    this.Controls.Remove(_Thumb);
                    _Thumb.Dispose();
                    _Thumb = null;
                }
                return;
            }

            Node root = GetDisplayRootNode();
            Rectangle innerBounds = ElementStyleLayout.GetInnerRect(this.BackgroundStyle, this.ClientRectangle);
            // Check do we need vertical scrollbar
            Size scrollSize = _AutoScrollMinSize;
            if (_VScrollBarVisible && scrollSize.Height > innerBounds.Height)
            {
                if (_VScrollBar == null)
                {
                    _VScrollBar = new DevComponents.DotNetBar.VScrollBarAdv();
                    _VScrollBar.Appearance = _ScrollBarAppearance;
                    _VScrollBar.Width = SystemInformation.VerticalScrollBarWidth;
                    this.Controls.Add(_VScrollBar);
                    _VScrollBar.BringToFront();
                    _VScrollBar.Scroll += new ScrollEventHandler(VScrollBarScroll);
                }
                if (_VScrollBar.Minimum != 0)
                    _VScrollBar.Minimum = 0;
                int innerHeight = innerBounds.Height - 12;
                if (_ColumnHeader != null && _ColumnHeader.Visible)
                    innerHeight -= _ColumnHeader.Height + 2;
                if (_VScrollBar.LargeChange != innerHeight && innerHeight > 0)
                    _VScrollBar.LargeChange = innerHeight;
                if (root != null && root.Bounds.Height > 0)
                    _VScrollBar.SmallChange = root.Bounds.Height;
                else
                    _VScrollBar.SmallChange = 22;
                if (_VScrollBar.Maximum != _AutoScrollMinSize.Height)
                    _VScrollBar.Maximum = _AutoScrollMinSize.Height;
                if (_VScrollBar.Value != -_AutoScrollPosition.Y)
                    _VScrollBar.Value = (Math.Min(_VScrollBar.Maximum, Math.Abs(_AutoScrollPosition.Y)));
            }
            else
                RemoveVScrollBar();

            // Check horizontal scrollbar
            if (_HScrollBarVisible && scrollSize.Width > innerBounds.Width)
            {
                if (_HScrollBar == null)
                {
                    _HScrollBar = new DevComponents.DotNetBar.ScrollBar.HScrollBarAdv();
                    _HScrollBar.Appearance = _ScrollBarAppearance;
                    _HScrollBar.Height = SystemInformation.HorizontalScrollBarHeight;
                    this.Controls.Add(_HScrollBar);
                    _HScrollBar.BringToFront();
                    _HScrollBar.Scroll += new ScrollEventHandler(HScrollBarScroll);
                }
                if (_HScrollBar.Minimum != 0)
                    _HScrollBar.Minimum = 0;
                if (_HScrollBar.LargeChange != innerBounds.Width && innerBounds.Width > 0)
                    _HScrollBar.LargeChange = innerBounds.Width;
                if (_HScrollBar.Maximum != _AutoScrollMinSize.Width)
                    _HScrollBar.Maximum = _AutoScrollMinSize.Width;
                if (_HScrollBar.Value != -_AutoScrollPosition.X)
                    _HScrollBar.Value = (Math.Min(_HScrollBar.Maximum, Math.Abs(_AutoScrollPosition.X)));
                if (root != null && root.Bounds.Height > 0)
                    _HScrollBar.SmallChange = root.Bounds.Height;
                else
                    _HScrollBar.SmallChange = 22;
            }
            else
                RemoveHScrollBar();

            RepositionScrollBars();

            if (_Thumb != null)
                _Thumb.BringToFront();
        }

        private void VScrollBarScroll(object sender, ScrollEventArgs e)
        {
#if (FRAMEWORK20)
            if (e.NewValue == e.OldValue) return;
#endif
            _AutoScrollPosition.Y = -e.NewValue;
            RepositionHostedControls(false);
            this.Invalidate();
            if (_ColumnHeader != null) _ColumnHeader.Invalidate();
            if (e.Type == ScrollEventType.ThumbTrack && m_HostedControlCells.Count > 0)
                this.Update();
            #if (FRAMEWORK20)
            OnScroll(new ScrollEventArgs(e.Type, e.OldValue, e.NewValue, ScrollOrientation.VerticalScroll));
            #endif
        }
        private void HScrollBarScroll(object sender, ScrollEventArgs e)
        {
            _AutoScrollPosition.X = -e.NewValue;
            RepositionHostedControls(false);
            this.Invalidate();
            if (_ColumnHeader != null) _ColumnHeader.Invalidate();
            if (e.Type == ScrollEventType.ThumbTrack && m_HostedControlCells.Count > 0)
                this.Update();
            #if (FRAMEWORK20)
            OnScroll(new ScrollEventArgs(e.Type, e.OldValue, e.NewValue, ScrollOrientation.HorizontalScroll));
            #endif
        }

        private void RepositionScrollBars()
        {
            Rectangle innerBounds = ElementStyleLayout.GetInnerRect(this.BackgroundStyle, this.ClientRectangle);
            if (_HScrollBar != null)
            {
                int width = innerBounds.Width;
                if (_VScrollBar != null)
                    width -= _VScrollBar.Width;
                _HScrollBar.Bounds = new Rectangle(innerBounds.X, innerBounds.Height - _HScrollBar.Height + 1, width, _HScrollBar.Height);
            }

            if (_VScrollBar != null)
            {
                int height = innerBounds.Height;
                if (_HScrollBar != null)
                    height -= _HScrollBar.Height;
                _VScrollBar.Bounds = new Rectangle(innerBounds.Right - _VScrollBar.Width, innerBounds.Y, _VScrollBar.Width, height);
            }

            if (_VScrollBar != null && _HScrollBar != null)
            {
                if (_Thumb == null)
                {
                    _Thumb = new Control();
                    _Thumb.BackColor = this.BackColor;
                    this.Controls.Add(_Thumb);
                }
                _Thumb.Bounds = new Rectangle(_HScrollBar.Bounds.Right, _VScrollBar.Bounds.Bottom, _VScrollBar.Width, _HScrollBar.Height);
            }
            else if (_Thumb != null)
            {
                this.Controls.Remove(_Thumb);
                _Thumb.Dispose();
                _Thumb = null;
            }
            RepositionColumnHeader();
        }

        internal int ColumnHeaderHeight
        {
            get
            {
                return _ColumnHeader != null ? _ColumnHeader.Height : 0;
            }
        }

        private void RepositionColumnHeader()
        {
            Rectangle innerBounds = ElementStyleLayout.GetInnerRect(this.BackgroundStyle, this.ClientRectangle);
            if (_ColumnHeader != null)
            {
                Rectangle r = GetScreenRectangle(new Rectangle(innerBounds.X, innerBounds.Y,
                    innerBounds.Width - (_VScrollBar != null ? _VScrollBar.Width : 0), _ColumnHeader.Columns.Bounds.Height));
                r.X = innerBounds.X;
                r.Y = innerBounds.Y;
                r.Width = Math.Max(0, innerBounds.Width - (_VScrollBar != null ? _VScrollBar.Width : 0));
                r.Height = Math.Max(0, r.Height);
                _ColumnHeader.Bounds = r;
            }
        }

        private void RemoveHScrollBar()
        {
            if (_HScrollBar != null)
            {
                Rectangle r = _HScrollBar.Bounds;
                this.Controls.Remove(_HScrollBar);
                _HScrollBar.Dispose();
                _HScrollBar = null;
                this.Invalidate(r);
            }
        }

        private void RemoveVScrollBar()
        {
            if (_VScrollBar != null)
            {
                Rectangle r = _VScrollBar.Bounds;
                this.Controls.Remove(_VScrollBar);
                _VScrollBar.Dispose();
                _VScrollBar = null;
                this.Invalidate(r);
            }
        }
		
		private System.Drawing.Drawing2D.Matrix GetTranslationMatrix(float zoom)
		{
			System.Drawing.Drawing2D.Matrix mx = new System.Drawing.Drawing2D.Matrix(zoom, 0, 0, zoom, 0, 0);
			if(this.CenterContent)
			{
				float offsetX = 0, offsetY = 0;
				if(this.AutoScroll)
				{
					if(this.Width>this.NodeLayout.Width)
					{
						if(this.Width<this.NodeLayout.Width*zoom)
							offsetX = - (this.Width - this.NodeLayout.Width)/2;
						else
							offsetX = (this.Width*(1.0f/zoom) - this.Width)/2;
					}
					
					if(this.Height>this.NodeLayout.Height)
					{
						if(this.Height<this.NodeLayout.Height*zoom)
							offsetY = - (this.Height - this.NodeLayout.Height)/2;
						else
							offsetY = (this.Height*(1.0f/zoom) - this.Height)/2;
					}
				}
				else
				{
					offsetX = (this.Width*(1.0f/zoom) - this.Width)/2;
					offsetY = (this.Height*(1.0f/zoom) - this.Height)/2;
				}
					
				mx.Translate(offsetX, offsetY);
			}
			return mx;
		}
		
		/// <summary>
		/// Returns translation matrix for current Zoom. Translation matrix is used to translate internal node coordinates to screen
		/// coordinates when Zoom is not set to 1.
		/// </summary>
		/// <returns>Returns new instance of Matrix object.</returns>
		public System.Drawing.Drawing2D.Matrix GetTranslationMatrix()
		{
			return GetTranslationMatrix(m_ZoomFactor);
		}
		
		/// <summary>
		/// Returns layout based rectangle from screen rectangle. Layout based rectangle will be different
		/// from screen rectangle when Zoom is not set to 1. This method will translate the screen rectangle enlarged by Zoom
		/// to layout rectangle which does not have Zoom applied.
		/// </summary>
		/// <param name="r">Screen rectangle</param>
		/// <returns>Layout rectangle</returns>
		public virtual Rectangle GetLayoutRectangle(Rectangle r)
		{
			if(m_ZoomFactor==1)
				return r;
			
			Point[] p=new Point[] {new Point(r.X, r.Y), new Point(r.Right, r.Bottom)};
			using(System.Drawing.Drawing2D.Matrix mx = GetTranslationMatrix())
			{
				mx.Invert();
				mx.TransformPoints(p);
			}
			return new Rectangle(p[0].X, p[0].Y, p[1].X-p[0].X, p[1].Y-p[0].Y);
		}
		
		/// <summary>
		/// Returns mouse position which is translated if control Zoom is not equal 1
		/// </summary>
		/// <param name="e">Mouse event arguments</param>
		/// <returns>Returns translated position</returns>
		protected virtual Point GetLayoutPosition(MouseEventArgs e)
		{
			return GetLayoutPosition(e.X, e.Y);
		}
		
		/// <summary>
		/// Returns mouse position which is translated if control Zoom is not equal 1
		/// </summary>
		/// <param name="mousePosition">Mouse position</param>
		/// <returns>Returns translated position</returns>
		public virtual Point GetLayoutPosition(Point mousePosition)
		{
			return GetLayoutPosition(mousePosition.X, mousePosition.Y);
		}
		
		/// <summary>
		/// Returns mouse position which is translated if control Zoom is not equal 1
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		/// <returns></returns>
		public virtual Point GetLayoutPosition(int x, int y)
		{
			if(m_ZoomFactor==1)
				return new Point(x, y);
			Point[] p=new Point[] {new Point(x, y)};
			using(System.Drawing.Drawing2D.Matrix mx = GetTranslationMatrix())
			{
				mx.Invert();
				mx.TransformPoints(p);
			}
			return p[0];
		}
		
		/// <summary>
		/// Returns rectangle translated to screen rectangle if Zoom is not equal 1.
		/// </summary>
		/// <param name="r">Rectangle to translate</param>
		/// <returns>Screen Rectangle</returns>
		public virtual Rectangle GetScreenRectangle(Rectangle r)
		{
			if(m_ZoomFactor==1)
				return r;
			Point[] p=new Point[] {new Point(r.X, r.Y), new Point(r.Right, r.Bottom)};
			using(System.Drawing.Drawing2D.Matrix mx = GetTranslationMatrix())
			{
				mx.TransformPoints(p);
			}
			return new Rectangle(p[0].X, p[0].Y, p[1].X-p[0].X, p[1].Y-p[0].Y);
		}
		
		/// <summary>
		/// Returns size translated to screen dimension if Zoom is not equal 1.
		/// </summary>
		/// <param name="s">Size to translate</param>
		/// <returns>Screen Size</returns>
		public virtual Size GetScreenSize(Size s)
		{
			return GetScreenRectangle(new Rectangle(Point.Empty, s)).Size;
		}

        internal void InvokeColumnHeaderMouseUp(object sender, MouseEventArgs e)
        {
            OnColumnHeaderMouseUp(sender, e);
        }
        /// <summary>
        /// Raises ColumnHeaderMouseUp event.
        /// </summary>
        /// <param name="sender">Reference to ColumnHeader</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnColumnHeaderMouseUp(object sender, MouseEventArgs e)
        {
            if (ColumnHeaderMouseUp != null)
                ColumnHeaderMouseUp(sender, e);
        }

        internal void InvokeColumnHeaderMouseDown(object sender, MouseEventArgs e)
        {
            OnColumnHeaderMouseDown(sender, e);
        }
        /// <summary>
        /// Raises ColumnHeaderMouseDown event.
        /// </summary>
        /// <param name="sender">Reference to ColumnHeader</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnColumnHeaderMouseDown(object sender, MouseEventArgs e)
        {
            if (ColumnHeaderMouseDown != null)
                ColumnHeaderMouseDown(sender, e);
        }
		
		#endregion

		#region Event Invocation
        /// <summary>
        /// Calls <see cref="OnAfterCheck">OnBeforeCheck</see> method which fired
        /// <see cref="AfterCheck">OnBeforeCheck</see> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void InvokeBeforeCheck(AdvTreeCellBeforeCheckEventArgs e)
        {
            OnBeforeCheck(e);
        }

        /// <summary>Raises the <see cref="BeforeCheck">BeforeCheck</see> event.</summary>
        /// <param name="e">
        /// A <see cref="AdvTreeCellBeforeCheckEventArgs">AdvTreeCellBeforeCheckEventArgs</see> that contains the event
        /// data.
        /// </param>
        protected virtual void OnBeforeCheck(AdvTreeCellBeforeCheckEventArgs e)
        {
            if (BeforeCheck != null)
                BeforeCheck(this, e);
        }

		/// <summary>
		/// Calls <see cref="OnAfterCheck">OnAfterCheck</see> method which fired
		/// <see cref="AfterCheck">AfterCheck</see> event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		internal void InvokeAfterCheck(AdvTreeCellEventArgs e)
		{
			OnAfterCheck(e);
		}

		/// <summary>Raises the <see cref="AfterCheck">AfterCheck</see> event.</summary>
		/// <param name="e">
		/// A <see cref="AdvTreeCellEventArgs">AdvTreeEventArgs</see> that contains the event
		/// data.
		/// </param>
		protected virtual void OnAfterCheck(AdvTreeCellEventArgs e)
		{
			if(AfterCheck!=null)
				AfterCheck(this,e);
		}

		/// <summary>
		/// Invokes CommandButtonClick event.
		/// </summary>
		/// <param name="node">Context node.</param>
		/// <param name="e">Event arguments.</param>
		internal void InvokeCommandButtonClick(Node node, CommandButtonEventArgs e)
		{
			if(CommandButtonClick!=null)
				CommandButtonClick(node,e);
		}
		
		protected virtual void OnBeforeCellEdit(CellEditEventArgs e)
		{
			if(BeforeCellEdit!=null)
				BeforeCellEdit(this, e);
		}
		
		void INodeNotify.OnBeforeCollapse(AdvTreeNodeCancelEventArgs e)
		{
			if(BeforeCollapse!=null)
				BeforeCollapse(this, e);
		}
		
		void INodeNotify.OnAfterCollapse(AdvTreeNodeEventArgs e)
		{
			if(AfterCollapse!=null)
				AfterCollapse(this, e);
		}
		
		void INodeNotify.OnBeforeExpand(AdvTreeNodeCancelEventArgs e)
		{
			if(BeforeExpand!=null)
				BeforeExpand(this, e);
		}
		
		void INodeNotify.OnAfterExpand(AdvTreeNodeEventArgs e)
		{
			if(AfterExpand!=null)
				AfterExpand(this, e);
		}

        internal void InvokeOnAfterNodeDeselect(AdvTreeNodeEventArgs args)
        {
            OnAfterNodeDeselect(args);
        }
        protected virtual void OnAfterNodeDeselect(AdvTreeNodeEventArgs args)
		{
			if(AfterNodeDeselect!=null)
                AfterNodeDeselect(this, args);
		}

        internal void InvokeOnAfterNodeSelect(AdvTreeNodeEventArgs args)
        {
            OnAfterNodeSelect(args);
        }
		protected virtual void OnAfterNodeSelect(AdvTreeNodeEventArgs args)
		{
			if(AfterNodeSelect!=null)
				AfterNodeSelect(this, args);
		}

        internal void InvokeOnBeforeNodeSelect(AdvTreeNodeCancelEventArgs args)
        {
            OnBeforeNodeSelect(args);
        }
		protected virtual void OnBeforeNodeSelect(AdvTreeNodeCancelEventArgs args)
		{
			if(BeforeNodeSelect!=null)
				BeforeNodeSelect(this,args);
		}
		
		/// <summary>
		/// Invokes DeserializeNode event.
		/// </summary>
		/// <param name="e">Provides more information about the event</param>
		protected virtual void OnDeserializeNode(SerializeNodeEventArgs e)
		{
			if(DeserializeNode!=null)
				DeserializeNode(this, e);
		}
		
		/// <summary>
		/// Invokes SerializeNode event.
		/// </summary>
		/// <param name="e">Provides more information about the event</param>
		protected virtual void OnSerializeNode(SerializeNodeEventArgs e)
		{
			if(SerializeNode!=null)
				SerializeNode(this, e);
		}
		
		internal void InvokeSerializeNode(SerializeNodeEventArgs e)
		{
			OnSerializeNode(e);
		}
		
		internal void InvokeDeserializeNode(SerializeNodeEventArgs e)
		{
			OnDeserializeNode(e);
		}
		
		internal bool HasSerializeNodeHandlers
		{
			get { return (SerializeNode != null);}
		}
		
		internal bool HasDeserializeNodeHandlers
		{
			get { return (DeserializeNode != null);}
		}
		
		internal void InvokeMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
		{
			OnMarkupLinkClick(sender, e);
			m_CellMouseDownCounter = 0;
		}
		
		/// <summary>
		/// Invokes the MarkupLinkClick evcent.
		/// </summary>
		/// <param name="sender">Sender of the event, usually instance Cell object.</param>
		/// <param name="e">Event arguments</param>
		protected virtual void OnMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
		{
			if(MarkupLinkClick!=null)
				MarkupLinkClick(sender, e);
		}
		#endregion
		
		#region Drag & Drop Support
		[DllImport("user32.dll", SetLastError=true)] 
		[return:MarshalAs(UnmanagedType.Bool)] 
		private static extern bool SetCursorPos(int X, int Y); 

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop (drgevent);
			InternalDragDrop(drgevent);
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void InternalDragDrop(DragEventArgs drgevent)
		{
			ReleaseDragNode(true);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave (e);
			InternalDragLeave();
		}

        private Timer _DragScrollTimer = null;
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void InternalDragLeave()
		{
			ReleaseDragNode(false);
            if (AutoScroll && _DragScrollTimer == null)
            {
                _DragScrollTimer = new Timer();
                _DragScrollTimer.Interval = 100;
                _DragScrollTimer.Tick += new EventHandler(DragScrollTimerTick);
                _DragScrollTimer.Start();
            }
		}

        private void DestroyDragScrollTimer()
        {
            Timer t = _DragScrollTimer;
            if (t == null) return;
            _DragScrollTimer = null;
            t.Stop();
            t.Tick -= new EventHandler(DragScrollTimerTick);
            t.Dispose();
        }

        private void DragScrollTimerTick(object sender, EventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left && this.IsHandleCreated && this.AutoScroll)
            {
                Point p = this.PointToClient(Control.MousePosition);
                Rectangle temp = this.ClientRectangle;
                temp.Inflate(32, 32);
                if (!temp.Contains(p)) return;

                if (p.Y < 0 && this.AutoScrollPosition.Y != 0 && _VScrollBar!=null)
                    this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, Math.Min(0, this.AutoScrollPosition.Y + _VScrollBar.SmallChange));
                else if (_VScrollBar!=null && p.Y > this.ClientRectangle.Bottom && -(this.AutoScrollPosition.Y - _VScrollBar.SmallChange) < (_VScrollBar.Maximum - _VScrollBar.LargeChange))
                {
                    this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, Math.Min(0, this.AutoScrollPosition.Y - _VScrollBar.SmallChange));
                }
            }
            else
                DestroyDragScrollTimer();
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            DestroyDragScrollTimer();
            base.OnDragEnter(drgevent);
        }

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver (drgevent);
			InternalDragOver(drgevent);
		}

        /// <summary>
        /// Raises the NodeDragFeedback event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnNodeDragFeedback(TreeDragFeedbackEventArgs e)
        {
            TreeDragFeedbackEventHander h=NodeDragFeedback;
            if (h != null)
                h(this, e);
        }

        private int _DropAsChildOffset = 24;
        /// <summary>
        /// Gets or sets the offset in pixels from node's X position that is used during drag &amp; drop operation to indicate that
        /// dragged node is dropped as child node of the parent's node.
        /// </summary>
        [DefaultValue(24), Category("Behavior"), Description("Indicates offset in pixels from node's X position that is used during drag &amp; drop operation to indicate that dragged node is dropped as child node of the parent's node.")]
        public int DropAsChildOffset
        {
            get { return _DropAsChildOffset; }
            set
            {
                _DropAsChildOffset = value;
            }
        }

        private bool _DragDropNodeCopyEnabled = true;
        /// <summary>
        /// Gets or sets whether drag &amp; drop internal implementation allows the copying of the node being dragged when CTRL key is pressed.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether drag &amp; drop internal implementation allows the copying of the node being dragged when CTRL key is pressed.")]
        public bool DragDropNodeCopyEnabled
        {
            get { return _DragDropNodeCopyEnabled; }
            set
            {
                _DragDropNodeCopyEnabled = value;
            }
        }

        private bool GetNodesContain(Node[] nodes, Node nodeToLookFor)
        {
            foreach (Node node in nodes)
            {
                if (node == nodeToLookFor)
                    return true;
            }
            return false;
        }

        private NodeDragInfo _DragInfo = null;
		/// <summary>
		/// Processes drag over event.
		/// </summary>
		/// <param name="drgevent">Drag event arguments.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void InternalDragOver(DragEventArgs drgevent)
		{
			if(!m_DragDropEnabled)
				return;

            if (m_DragNode == null)
                CreateDragNode(drgevent);

			if(m_DragNode==null)
			{
				drgevent.Effect=DragDropEffects.None;
				return;
			}

            drgevent.Effect = (Control.ModifierKeys == Keys.Control && _DragDropNodeCopyEnabled) ? DragDropEffects.Copy : DragDropEffects.Move;

			InvalidateNode(m_DragNode);
			Point p=this.PointToClient(new Point(drgevent.X,drgevent.Y));
			p=GetLayoutPosition(p);
			TreeAreaInfo areaInfo = NodeOperations.GetTreeAreaInfo(this, p.X, p.Y);
			Point insideNodeOffset=Point.Empty;

            NodeDragInfo newDragInfo = null;

            if (areaInfo != null && areaInfo.NodeAt == null && this.ClientRectangle.Contains(p.X, p.Y))
            {
                Node lastVisibleNode = NodeOperations.GetLastVisibleTopLevelNode(this);
                if (lastVisibleNode != null && p.Y >= lastVisibleNode.Bounds.Y)
                    areaInfo.NodeAt = lastVisibleNode;
            }
            
            if (areaInfo != null && areaInfo.NodeAt != null && !GetNodesContain((Node[])m_DragNode.Tag, areaInfo.NodeAt) && !NodeOperations.IsChildOfAnyParent((Node[])m_DragNode.Tag, areaInfo.NodeAt))
            {
                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, areaInfo.NodeAt, m_NodeDisplay.Offset);
                Rectangle rContent = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, areaInfo.NodeAt, m_NodeDisplay.Offset);

                if (_View == eView.Tile)
                {
                    if (areaInfo.NodeAt.HasChildNodes)
                    {
                        // Drop as child
                        MultiNodeTreeDragFeedbackEventArgs feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt, -1, (Node[])m_DragNode.Tag, drgevent.Effect);
                        OnNodeDragFeedback(feedbackArgs);
                        if (feedbackArgs.AllowDrop)
                        {
                            // Adding the drag node as child node
                            newDragInfo = new NodeDragInfo(feedbackArgs.ParentNode, feedbackArgs.InsertPosition);
                            if (feedbackArgs.EffectSet)
                                drgevent.Effect = feedbackArgs.Effect;
                        }
                        else
                            drgevent.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        TreeDragFeedbackEventArgs feedbackArgs = null;
                        // Pick insert position based on X position within the mouse over node. X<node.Width/2 insert before otherwise insert after
                        if (p.X < (r.X + r.Width / 3))
                            feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt.Parent, areaInfo.NodeAt.Index - 1, (Node[])m_DragNode.Tag, drgevent.Effect);
                        else
                        {
                            if (p.Y < rContent.Y)
                                feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt.Parent, areaInfo.NodeAt.Index + 1, (Node[])m_DragNode.Tag, drgevent.Effect);
                            else if (areaInfo.NodeAt.Expanded && areaInfo.NodeAt.Nodes.Count > 0)
                                feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt, -1, (Node[])m_DragNode.Tag);
                            else
                                feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt.Parent, areaInfo.NodeAt.Index, (Node[])m_DragNode.Tag, drgevent.Effect);
                        }
                        OnNodeDragFeedback(feedbackArgs);
                        if (feedbackArgs.AllowDrop)
                        {
                            // Adding the drag node as child node
                            newDragInfo = new NodeDragInfo(feedbackArgs.ParentNode, feedbackArgs.InsertPosition);
                            if (feedbackArgs.EffectSet)
                                drgevent.Effect = feedbackArgs.Effect;
                        }
                        else
                            drgevent.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    // Determine drag node insert position
                    if (p.X > rContent.X + _DropAsChildOffset)
                    {
                        MultiNodeTreeDragFeedbackEventArgs feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt, -1, (Node[])m_DragNode.Tag, drgevent.Effect);
                        OnNodeDragFeedback(feedbackArgs);
                        if (feedbackArgs.AllowDrop)
                        {
                            // Adding the drag node as child node
                            newDragInfo = new NodeDragInfo(feedbackArgs.ParentNode, feedbackArgs.InsertPosition);
                            if (feedbackArgs.EffectSet)
                                drgevent.Effect = feedbackArgs.Effect;
                        }
                        else
                            drgevent.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        TreeDragFeedbackEventArgs feedbackArgs = null;
                        // Pick insert position based on Y position within the mouse over node. y<node.Height/2 insert before otherwise insert after
                        if (p.Y < (r.Y + r.Height / 3))
                            feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt.Parent, areaInfo.NodeAt.Index - 1, (Node[])m_DragNode.Tag, drgevent.Effect);
                        else
                        {
                            if (p.X < rContent.X)
                                feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt.Parent, areaInfo.NodeAt.Index + 1, (Node[])m_DragNode.Tag, drgevent.Effect);
                            else if (areaInfo.NodeAt.Expanded && areaInfo.NodeAt.Nodes.Count > 0)
                                feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt, -1, (Node[])m_DragNode.Tag);
                            else
                                feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(areaInfo.NodeAt.Parent, areaInfo.NodeAt.Index, (Node[])m_DragNode.Tag, drgevent.Effect);
                        }
                        OnNodeDragFeedback(feedbackArgs);
                        if (feedbackArgs.AllowDrop)
                        {
                            // Adding the drag node as child node
                            newDragInfo = new NodeDragInfo(feedbackArgs.ParentNode, feedbackArgs.InsertPosition);
                            if (feedbackArgs.EffectSet)
                                drgevent.Effect = feedbackArgs.Effect;
                        }
                        else
                            drgevent.Effect = DragDropEffects.None;
                    }
                }
            }
            else if (m_Nodes.Count == 0) // Drop into empty tree
            {
                MultiNodeTreeDragFeedbackEventArgs feedbackArgs = new MultiNodeTreeDragFeedbackEventArgs(null, 0, (Node[])m_DragNode.Tag, drgevent.Effect);
                OnNodeDragFeedback(feedbackArgs);
                if (feedbackArgs.AllowDrop)
                {
                    // Adding the drag node as child node
                    newDragInfo = new NodeDragInfo(null, 0);
                    if (feedbackArgs.EffectSet)
                        drgevent.Effect = feedbackArgs.Effect;
                }
                else
                    drgevent.Effect = DragDropEffects.None;
            }
            else
            {
                Rectangle temp = this.ClientRectangle;
                temp.Inflate(32, 32);
                if (AutoScroll && temp.Contains(p))
                {
                    if (p.Y < 0)
                    {
                        if (_VScrollBar != null && _VScrollBar.Value > 0)
                            _VScrollBar.Value -= _VScrollBar.SmallChange;
                    }
                    else if (p.Y > this.ClientRectangle.Bottom)
                    {
                        if (_VScrollBar != null && _VScrollBar.Value + _VScrollBar.SmallChange < _VScrollBar.Maximum)
                            _VScrollBar.Value += _VScrollBar.SmallChange;
                    }

                }
                drgevent.Effect = DragDropEffects.None;
            }

            bool update = false;
            if (newDragInfo != null)
            {
                if (_DragInfo == null)
                {
                    _DragInfo = newDragInfo;
                    InvalidateDragInfo(_DragInfo);
                    update = true;
                }
                else if (_DragInfo.Parent != newDragInfo.Parent || _DragInfo.InsertIndex != newDragInfo.InsertIndex)
                {
                    InvalidateDragInfo(_DragInfo);
                    InvalidateDragInfo(newDragInfo);
                    _DragInfo = newDragInfo;
                    update = true;
                }
            }
            else if (newDragInfo != _DragInfo)
            {
                InvalidateDragInfo(_DragInfo);
                _DragInfo = null;
                update = true;
            }

            m_DragNode.SetBounds(new Rectangle(p.X,p.Y,m_DragNode.BoundsRelative.Width,m_DragNode.BoundsRelative.Height));
			m_DragNode.Visible=true;
            InvalidateNode(m_DragNode);

            if (update)
            {
                if (this.DesignMode)
                    this.Refresh();
                else
                    this.Update();
            }
		}

        private void InvalidateDragInfo(NodeDragInfo dragInfo)
        {
            if (dragInfo == null) return;
            Rectangle r = GetDragInsertionBounds(dragInfo);
            if (!r.IsEmpty)
                Invalidate(r);
        }

        private Node GetDragInfoReferenceNode(NodeDragInfo dragInfo)
        {
            Node referenceNode = null;
            NodeCollection col = null;
            if (dragInfo.Parent != null)
            {
                col = dragInfo.Parent.Nodes;
            }
            else
            {
                col = this.Nodes;
            }

            if (col.Count > 0)
            {
                if (dragInfo.InsertIndex <= 0)
                    referenceNode = col[0];
                else if (dragInfo.InsertIndex >= col.Count)
                    referenceNode = col[col.Count - 1];
                else
                    referenceNode = col[dragInfo.InsertIndex];
            }

            return referenceNode;
        }
        internal static readonly int DragInsertMarkSize = 9;
        internal Rectangle GetDragInsertionBounds(NodeDragInfo dragInfo)
        {
            if (dragInfo == null) return Rectangle.Empty;

            Rectangle r = Rectangle.Empty;

            Node referenceNode = GetDragInfoReferenceNode(dragInfo);

            if (_View == eView.Tile)
            {
                //Console.WriteLine(dragInfo);
                if (referenceNode != null)
                {
                    r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, referenceNode, m_NodeDisplay.Offset);
                    if (dragInfo.InsertIndex == -1)
                        r.X -= 4;
                    else
                        r.X = r.Right - DragInsertMarkSize;
                    r.Height = referenceNode.Bounds.Height;
                    r.Width = DragInsertMarkSize;                    
                }
                else if (dragInfo.Parent != null)
                {
                    r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, dragInfo.Parent, m_NodeDisplay.Offset);
                    r.X += this.Indent;
                    r.Width = dragInfo.Parent.Bounds.Width;
                    r.Height = DragInsertMarkSize;
                }

                
            }
            else
            {
                if (referenceNode != null)
                {
                    r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, referenceNode, m_NodeDisplay.Offset);
                    r.Width = referenceNode.Bounds.Width;
                    if (referenceNode.Expanded && dragInfo.InsertIndex >= 0)
                    {
                        Rectangle childBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds, referenceNode, m_NodeDisplay.Offset);
                        r.Y = childBounds.Bottom - r.Height;
                    }
                }
                else if (dragInfo.Parent != null)
                {
                    r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, dragInfo.Parent, m_NodeDisplay.Offset);
                    r.X += this.Indent;
                    r.Width = dragInfo.Parent.Bounds.Width;
                }

                if (dragInfo.InsertIndex < 0 && referenceNode != null)
                    r.Y -= 4;
                else
                    r.Y = r.Bottom - 2;
                r.Height = DragInsertMarkSize;
            }

            return r;
        }

        private int _Indent = 16;
        /// <summary>
        /// Gets or sets the distance to indent each of the child tree node levels. Default value is 16.
        /// </summary>
        [DefaultValue(16), Description("Indicates distance to indent each of the child tree node levels."), Category("Appearance")]
        public int Indent
        {
            get { return _Indent; }
            set
            {
                if (_Indent != value)
                {
                    _Indent = value;
                    if (m_NodeLayout is NodeTreeLayout)
                        ((NodeTreeLayout)m_NodeLayout).NodeLevelOffset = value;
                    InvalidateNodesSize();
                    RecalcLayout();
                }
            }
        }
        

		/// <summary>
		/// Gets or sets whether drag and drop operation is in progress. This member supports
		/// the AdvTree infrastructure and is not intended to be used directly from your
		/// code.
		/// </summary>
		[Browsable(false)]
		public bool IsDragDropInProgress
		{
			get {return (m_DragNode!=null);}
		}

        private Node[] SortNodesByFlatIndex(Node[] dragNodes)
        {
            ArrayList nodes = new ArrayList(dragNodes);
            nodes.Sort(new NodeFlatIndexComparer(this));
            nodes.CopyTo(dragNodes);
            return dragNodes;
        }
        private void ReleaseDragNode(bool drop)
		{
            if (m_DragNode == null)
                return;
            bool isCopy = (Control.ModifierKeys == Keys.Control && _DragDropNodeCopyEnabled);

			this.BeginUpdate();
			try
			{
				if(m_NodeDisplay.LockOffset)
					m_NodeDisplay.LockOffset=false;

                Node[] dragNodes = m_DragNode.Tag as Node[];
                foreach (Node item in dragNodes)
                {
                    if (!item.Visible) item.Visible = true;
                }
                
                if (drop && _DragInfo != null)
                {
                    Node parent = _DragInfo.Parent;
                    // Fire off events and cancel processing if needed
                    TreeDragDropEventArgs e = new TreeDragDropEventArgs(eTreeAction.Mouse, dragNodes, dragNodes[0].Parent, parent, isCopy, _DragInfo.InsertIndex);
					InvokeBeforeNodeDrop(e);
                    if (!e.Cancel && e.Nodes!=null && e.Nodes.Length>0)
                    {
                        isCopy = e.IsCopy;
                        dragNodes = e.Nodes;
                        int index = e.InsertPosition + 1;

                        if (dragNodes.Length > 1)
                            dragNodes = SortNodesByFlatIndex(dragNodes);
                        
                        for (int i = 0; i < dragNodes.Length; i++)
                        {
                            Node childNode = dragNodes[i];
                            Node node = childNode;
                            if (NodeOperations.IsChildOfAnyParent(dragNodes, node)) continue;

                            if (isCopy)
                            {
                                node = node.DeepCopy();
                            }
                            else
                            {
                                if (parent != null)
                                {
                                    if (node.Parent == parent && parent.Nodes.IndexOf(node) < index)
                                        index--;
                                }
                                else
                                {
                                    if (node.Parent == null && node.TreeControl == this && this.Nodes.IndexOf(node) < index)
                                        index--;
                                }
                                node.Remove(eTreeAction.Mouse);
                            }

                            // Adjust index in case node was removed and flat indexes changed
                            if (index < 0)
                                index = 0;
                            else if (parent != null && index > parent.Nodes.Count)
                                index = parent.Nodes.Count;
                            else if (parent == null && index > this.Nodes.Count)
                                index = this.Nodes.Count;
                            
                            if (parent == null)
                                this.Nodes.Insert(index, node, eTreeAction.Mouse);
                            else
                            {
                                parent.Nodes.Insert(index, node, eTreeAction.Mouse);
                                if (!parent.Expanded)
                                    parent.Expanded = true;
                            }
                            index++;
                        }

                        InvokeAfterNodeDrop(e);
                    }
                }
                _DragInfo = null;
			}
			finally
			{
				m_DragNode=null;
				if(!m_PendingLayout)
				{
					this.EndUpdate();
					this.RecalcLayout();
					this.Refresh();
				}
				else
					this.EndUpdate();
			}
		}

        /// <summary>
        /// Raises the NodeDragStart event.
        /// </summary>
        /// <param name="sender">Reference to node being dragged.</param>
        /// <param name="e">Event parameters</param>
        protected virtual void OnNodeDragStart(object sender, EventArgs e)
        {
            EventHandler eh = NodeDragStart;
            if (eh != null)
                eh(sender, e);
        }

		private void CreateDragNode(DragEventArgs e)
		{
			if(e.Data!=null)
			{
                Node node = null;
                Node[] nodes = null;
                if (e.Data.GetDataPresent(typeof(Node)))
                    node = e.Data.GetData(typeof(Node)) as Node;
                else if (e.Data.GetDataPresent(typeof(Node[])))
                    nodes = e.Data.GetData(typeof(Node[])) as Node[];
                else if (e.Data.GetFormats().Length > 0)
                {
                    node = e.Data.GetData(e.Data.GetFormats()[0]) as Node;
                    nodes = e.Data.GetData(e.Data.GetFormats()[0]) as Node[];
                }

                if (node != null)
                    CreateDragNode(new Node[1] { node });
                else if (nodes != null && nodes.Length > 0)
                {
                    CreateDragNode(nodes);
                }
			}
		}

        private bool _MultiNodeDragCountVisible = true;
        /// <summary>
        /// Gets or sets whether number of nodes being dragged is displayed on drag node preview. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Selection"), Description("Indicates whether number of nodes being dragged is displayed on drag node preview.")]
        public bool MultiNodeDragCountVisible
        {
            get
            {
                return _MultiNodeDragCountVisible;
            }
            set
            {
            	_MultiNodeDragCountVisible = value;
            }
        }
        private void CreateDragNode(Node[] nodes)
		{
            AdvTreeMultiNodeCancelEventArgs eventArgs = new AdvTreeMultiNodeCancelEventArgs(eTreeAction.Mouse, nodes);
            OnBeforeNodeDragStart(eventArgs);
            if (eventArgs.Cancel) return;

            m_DragNode = nodes[0].Copy();
            m_DragNode.Tag = nodes;
            m_DragNode.IsDragNode = true;
			PrepareDragNodeElementStyle(m_DragNode, nodes[0]);

            if (nodes.Length > 1 && this.MultiNodeDragCountVisible)
            {
                Cell cell = new Cell();
                cell.Text = nodes.Length.ToString();
                cell.StyleNormal = new ElementStyle();
                cell.StyleNormal.BackColor = ColorScheme.GetColor(0x40408C);
                cell.StyleNormal.BackColor2 = ColorScheme.GetColor(0x0093F9);
                cell.StyleNormal.BackColorGradientAngle = 90;
                cell.StyleNormal.TextColor = Color.White;
                cell.StyleNormal.Padding = 3;
                cell.Name = "sysNodeCount";
                cell.Tag = m_DragNode.Tag;
                m_DragNode.Tag = null;
                m_DragNode.Cells.Insert(0, cell);
            }

			m_DragNode.Visible=false;
			m_NodeLayout.PerformSingleNodeLayout(m_DragNode);
			this.Refresh();
            OnNodeDragStart(nodes[0], new EventArgs());
		}

        /// <summary>
        /// Raises BeforeNodeDragStart event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnBeforeNodeDragStart(AdvTreeNodeCancelEventArgs e)
        {
            if (BeforeNodeDragStart != null) BeforeNodeDragStart(this, e);
        }

		private void PrepareDragNodeElementStyle(Node dragNode, Node originalNode)
		{
			// Setup half transparent styles
			int alpha=128;
			ElementStyle style=null;
			if(originalNode.Style!=null)
				style=originalNode.Style.Copy();
			else if(this.NodeStyle!=null)
				style=this.NodeStyle.Copy();
			else
			{
				style=new ElementStyle();
				style.TextColorSchemePart=eColorSchemePart.ItemText;	
			}
			ElementStyle.SetColorsAlpha(style,alpha);
			dragNode.Style=style;

			if(this.CellStyleDefault!=null)
				style=this.CellStyleDefault.Copy();
			else
				style=ElementStyle.GetDefaultCellStyle(dragNode.Style);
			ElementStyle.SetColorsAlpha(style,alpha);
			foreach(Cell cell in dragNode.Cells)
			{
    			cell.StyleNormal=style;
			}

			for(int i=0;i<dragNode.Cells.Count;i++)
			{
				if(originalNode.Cells[i].StyleNormal!=null)
				{
					ElementStyle cellStyle=originalNode.Cells[i].StyleNormal.Copy();
					ElementStyle.SetColorsAlpha(style,alpha);
					dragNode.Cells[i].StyleNormal=cellStyle;
				}
				else
					dragNode.Cells[i].StyleNormal=style;
			}
		}

        private bool _MultiNodeDragDropAllowed = true;
        /// <summary>
        /// Gets or sets whether multiple nodes drag &amp; drop is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Selection"), Description("Indicates whether multiple nodes drag &amp; drop is enabled.")]
        public bool MultiNodeDragDropAllowed
        {
            get
            {
                return _MultiNodeDragDropAllowed;
            }
            set
            {
                _MultiNodeDragDropAllowed = value;
            }
        }
        private bool StartDragDrop()
		{
			if(m_MouseOverNode==null)
				return false;

            if (this.MultiSelect && this.SelectedNodes.Count > 1 && this.MultiNodeDragDropAllowed)
            {
                Node[] nodes = new Node[this.SelectedNodes.Count];
                this.SelectedNodes.CopyTo(nodes);
                return StartDragDrop(nodes);
            }

			return StartDragDrop(m_MouseOverNode);
		}

        private bool StartDragDrop(Node[] nodes)
        {
            if (IsDragDropInProgress)
                return false;

            _SelectOnMouseUp = false;
            foreach (Node node in nodes)
            {
                if (!node.DragDropEnabled || node == m_DisplayRootNode)
                    return false;
                if (node.IsMouseDown)
                    OnNodeMouseUp(new MouseEventArgs(Control.MouseButtons, 0, 0, 0, 0));
            }

            this.EndCellEditing(eTreeAction.Mouse);

            Point m_MouseDownPoint = Control.MousePosition;

            if (this.DesignMode)
                CreateDragNode(nodes);
            else
                this.DoDragDrop(nodes, DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move | DragDropEffects.Scroll);

            return true;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool StartDragDrop(Node node)
		{
			if(IsDragDropInProgress || !node.DragDropEnabled)
				return false;
			
			this.EndCellEditing(eTreeAction.Mouse);

			if(node.IsMouseDown)
				OnNodeMouseUp(new MouseEventArgs(Control.MouseButtons,0,0,0,0));

			if(node == m_DisplayRootNode /*|| node.Parent==null*/) // Drag & drop in designer means that parent is null
				return false;

			Point m_MouseDownPoint=Control.MousePosition;

			if(this.DesignMode)
                CreateDragNode(new Node[1] { node });
			else
				this.DoDragDrop(node,DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move | DragDropEffects.Scroll);

			return true;
		}
		
		/// <summary>
		/// Returns the display root node.
		/// </summary>
		/// <returns>Instance of node or null if there is no display root node.</returns>
		internal Node GetDisplayRootNode()
		{
			if(m_DisplayRootNode!=null)
				return m_DisplayRootNode;
			if(this.Nodes.Count>0)
				return this.Nodes[0];
			return null;
		}

		/// <summary>
		/// Returns reference to the node involved in drag-drop operation if any.
		/// </summary>
		/// <returns>Reference to node object or null if there is no drag node.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
        public Node GetDragNode()
		{
			return m_DragNode;
		}

        /// <summary>
        /// Returns the reference to node drag information class that gives information about node drag & drop.
        /// </summary>
        /// <returns>Reference to node drag info object or null if no drag information is available</returns>
        internal NodeDragInfo GetNodeDragInfo()
        {
            return _DragInfo;
        }
		#endregion

		#region ISupportInitialize
		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be
		/// used directly from your code.
		/// </summary>
		public void BeginInit()
		{
			this.BeginUpdate();
		}

		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be
		/// used directly from your code.
		/// </summary>
		public void EndInit()
		{
			this.EndUpdate();
		}
		#endregion

        #region Property Hiding
        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }
#if FRAMEWORK20
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }
#endif
        #endregion

        #region Licensing
#if !TRIAL
        private string m_LicenseKey="";
		private bool m_DialogDisplayed=false;
		[Browsable(false), DefaultValue("")]
		public string LicenseKey
		{
			get {return m_LicenseKey;}
			set
			{
				if(NodeOperations.ValidateLicenseKey(value))
					return;
				m_LicenseKey = (!NodeOperations.CheckLicenseKey(value) ? "9dsjkhds7" : value);
			}
		}
#endif
		#endregion

        #region BarAccessibleObject
        public class AdvTreeAccessibleObject : System.Windows.Forms.Control.ControlAccessibleObject
        {
            AdvTree _Owner = null;
            public AdvTreeAccessibleObject(AdvTree owner)
                : base(owner)
            {
                _Owner = owner;
            }

            internal void GenerateEvent(Node sender, System.Windows.Forms.AccessibleEvents e)
            {
                int iChild = _Owner.Nodes.IndexOf(sender);
                if (iChild >= 0)
                {
                    if (_Owner != null && !_Owner.IsDisposed)
                        _Owner.AccessibilityNotifyClients(e, iChild);
                }
            }

            public override AccessibleRole Role
            {
                get
                {
                    if (_Owner != null && !_Owner.IsDisposed)
                        return _Owner.AccessibleRole;
                    return System.Windows.Forms.AccessibleRole.None;
                }
            }

            public override AccessibleObject Parent
            {
                get
                {
                    if (_Owner != null && !_Owner.IsDisposed)
                        return _Owner.Parent.AccessibilityObject;
                    return null;
                }
            }

            public override Rectangle Bounds
            {
                get
                {
                    if (_Owner != null && !_Owner.IsDisposed && _Owner.Parent != null)
                        return this._Owner.Parent.RectangleToScreen(_Owner.Bounds);
                    return Rectangle.Empty;
                }
            }

            public override int GetChildCount()
            {
                if (_Owner != null && !_Owner.IsDisposed && _Owner.Nodes != null)
                    return _Owner.Nodes.Count;
                return 0;
            }

            public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
            {
                if (_Owner != null && !_Owner.IsDisposed && _Owner.Nodes != null)
                    return _Owner.Nodes[iIndex].AccessibleObject;
                return null;
            }

            public override AccessibleStates State
            {
                get
                {
                    AccessibleStates state = AccessibleStates.Default;
                    if (_Owner == null || _Owner.IsDisposed)
                        return AccessibleStates.None;
                    if (_Owner.Focused)
                        state = AccessibleStates.Focused;

                    return state;
                }
            }
        }

        internal void OnNodesCleared()
        {
            if (m_NodeDisplay != null)
                m_NodeDisplay.PaintedNodes.Clear();
            if (_FullRowBackgroundNodes != null)
                _FullRowBackgroundNodes.Clear();
            if (this.MultiSelect) this.SelectedNodes.Clear();
            this.SelectedNode = null;
            if (this.IsHandleCreated)
                this.RecalcLayout();
            else
                SetPendingLayout();
        }

        internal Rectangle GetPaintRectangle()
        {
            Rectangle rect = this.ClientRectangle;
            if(_ColumnHeader!=null && _ColumnHeader.Visible)
            {
                rect.Y += _ColumnHeader.Height;
                rect.Height -= _ColumnHeader.Height;
            }
            if(_VScrollBar!=null && _VScrollBar.Visible)
            {
                rect.Width -= _VScrollBar.Width;
            }
            if(_HScrollBar!=null && _HScrollBar.Visible)
            {
                rect.Height -= _HScrollBar.Height;
            }
            return rect;
        }

	    #endregion

        #region Data-binding
#if FRAMEWORK20
        private List<BindingMemberInfo> _DisplayMembers = null;
        /// <summary>
        /// Gets or sets the comma separated list of property or column names to display on popup tree control.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates comma separated list of property or column names to display on popup tree control")]
        public string DisplayMembers
        {
            get
            {
                if (_DisplayMembers == null || _DisplayMembers.Count == 0)
                    return "";
                StringBuilder members = new StringBuilder();
                for (int i = 0; i < _DisplayMembers.Count; i++)
                {
                    BindingMemberInfo item = _DisplayMembers[i];
                    members.Append(item.BindingMember);
                    if (i + 1 < _DisplayMembers.Count)
                        members.Append(',');
                }
                return members.ToString();
            }
            set
            {
                List<BindingMemberInfo> displayMembers = _DisplayMembers;

                List<BindingMemberInfo> newMembers = null;

                if (!string.IsNullOrEmpty(value))
                {
                    newMembers = new List<BindingMemberInfo>();
                    // Parse the members comma separated list expected...
                    string[] members = value.Split(',');
                    for (int i = 0; i < members.Length; i++)
                    {
                        newMembers.Add(new BindingMemberInfo(members[i].Trim()));
                    }
                }

                try
                {
                    this.SetDataConnection(_DataSource, newMembers, false);
                }
                catch
                {
                    _DisplayMembers = displayMembers;
                }
            }
        }

        private object _DataSource = null;
        /// <summary>
        /// Gets or sets the data source for the ComboTree. Expected is an object that implements the IList or IListSource interfaces, 
        /// such as a DataSet or an Array. The default is null.
        /// </summary>
        [AttributeProvider(typeof(IListSource)), Description("Indicates data source for the ComboTree."), Category("Data"), DefaultValue(null), RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource
        {
            get
            {
                return _DataSource;
            }
            set
            {
                if (((value != null) && !(value is IList)) && !(value is IListSource))
                {
                    throw new ArgumentException("Data type is not supported for complex data binding");
                }
                if (_DataSource != value)
                {
                    try
                    {
                        this.SetDataConnection(value, _DisplayMembers, true);
                    }
                    catch
                    {
                        this.DisplayMembers = "";
                    }
                    if (value == null)
                    {
                        this.DisplayMembers = "";
                    }
                }
            }
        }

        private bool _FormattingEnabled = false;
        /// <summary>
        /// Gets or sets a value indicating whether formatting is applied to the DisplayMembers property of the control.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether formatting is applied to the DisplayMembers property of the control.")]
        public bool FormattingEnabled
        {
            get
            {
                return _FormattingEnabled;
            }
            set
            {
                if (value != _FormattingEnabled)
                {
                    _FormattingEnabled = value;
                    RefreshItems();
                    OnFormattingEnabledChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raises FormattingEnabledChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnFormattingEnabledChanged(EventArgs e)
        {
            EventHandler handler = FormattingEnabledChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private string _FormatString = "";
        /// <summary>
        /// Gets or sets the format-specifier characters that indicate how a value is to be displayed.
        /// </summary>
        [MergableProperty(false), Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)), DefaultValue(""), Description("Indicates format-specifier characters that indicate how a value is to be displayed.")]
        public string FormatString
        {
            get
            {
                return _FormatString;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }
                if (!value.Equals(_FormatString))
                {
                    _FormatString = value;
                    RefreshItems();
                    OnFormatStringChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raises FormatStringChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnFormatStringChanged(EventArgs e)
        {
            EventHandler handler = FormatStringChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private IFormatProvider _FormatInfo = null;
        /// <summary>
        /// Gets or sets the IFormatProvider that provides custom formatting behavior. 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false), DefaultValue((string)null)]
        public IFormatProvider FormatInfo
        {
            get
            {
                return _FormatInfo;
            }
            set
            {
                if (value != _FormatInfo)
                {
                    _FormatInfo = value;
                    RefreshItems();
                    OnFormatInfoChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raises FormatInfoChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnFormatInfoChanged(EventArgs e)
        {
            EventHandler handler = FormatInfoChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        private bool _InSetDataConnection = false;
        private void SetDataConnection(object newDataSource, List<BindingMemberInfo> newDisplayMembers, bool force)
        {
            bool dataSourceChanged = _DataSource != newDataSource;
            bool displayMemberChanged = _DisplayMembers != newDisplayMembers;

            if (!_InSetDataConnection)
            {
                try
                {
                    if ((force || dataSourceChanged) || displayMemberChanged)
                    {
                        _InSetDataConnection = true;
                        IList list = (this.DataManager != null) ? this.DataManager.List : null;
                        bool isDataManagerNull = this.DataManager == null;
                        this.UnwireDataSource();
                        _DataSource = newDataSource;
                        _DisplayMembers = newDisplayMembers;
                        this.WireDataSource();
                        if (_IsDataSourceInitialized)
                        {
                            CurrencyManager manager = null;
                            if (((newDataSource != null) && (this.BindingContext != null)) && (newDataSource != Convert.DBNull))
                            {
                                string bindingPath = "";
                                if (_DisplayMembers != null && _DisplayMembers.Count > 0)
                                    bindingPath = newDisplayMembers[0].BindingPath;
                                manager = (CurrencyManager)this.BindingContext[newDataSource, bindingPath];
                            }
                            if (_DataManager != manager)
                            {
                                if (_DataManager != null)
                                {
                                    _DataManager.ItemChanged -= new ItemChangedEventHandler(this.DataManager_ItemChanged);
                                    _DataManager.PositionChanged -= new EventHandler(this.DataManager_PositionChanged);
                                }
                                _DataManager = manager;
                                if (_DataManager != null)
                                {
                                    _DataManager.ItemChanged += new ItemChangedEventHandler(this.DataManager_ItemChanged);
                                    _DataManager.PositionChanged += new EventHandler(this.DataManager_PositionChanged);
                                }
                            }
                            if (((_DataManager != null) && (displayMemberChanged || dataSourceChanged)) && !ValidateDisplayMembers(_DisplayMembers))
                            {
                                throw new ArgumentException("Wrong DisplayMembers parameter", "newDisplayMember");
                            }
                            if ((_DataManager != null && (dataSourceChanged || displayMemberChanged || force))
                                && (displayMemberChanged || dataSourceChanged || (force && ((list != _DataManager.List) || isDataManagerNull))))
                            {
                                DataManager_ItemChanged(_DataManager, null);
                            }
                        }
                        _Converters.Clear();
                    }
                    if (dataSourceChanged)
                    {
                        this.OnDataSourceChanged(EventArgs.Empty);
                    }
                    if (displayMemberChanged)
                    {
                        this.OnDisplayMembersChanged(EventArgs.Empty);
                    }
                }
                finally
                {
                    _InSetDataConnection = false;
                }
            }
        }

        private bool ValidateDisplayMembers(List<BindingMemberInfo> members)
        {
            if (members == null || members.Count == 0) return true;

            foreach (BindingMemberInfo item in members)
            {
                if (item.BindingMember != null && !BindingMemberInfoInDataManager(item))
                    return false;
            }
            return true;
        }
        private bool BindingMemberInfoInDataManager(BindingMemberInfo bindingMemberInfo)
        {
            if (_DataManager != null)
            {
                PropertyDescriptorCollection itemProperties = _DataManager.GetItemProperties();
                int count = itemProperties.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(bindingMemberInfo.BindingField))
                    {
                        return true;
                    }
                }
                for (int j = 0; j < count; j++)
                {
                    if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && (string.Compare(itemProperties[j].Name, bindingMemberInfo.BindingField, true, CultureInfo.CurrentCulture) == 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Raises the DataSourceChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data. </param>
        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            EventHandler handler = DataSourceChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Raises the DisplayMemberChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data. </param>
        protected virtual void OnDisplayMembersChanged(EventArgs e)
        {
            EventHandler handler = DisplayMembersChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private Hashtable _Converters = new Hashtable();
        private TypeConverter GetFieldConverter(string fieldName)
        {
            if (_Converters.ContainsKey(fieldName))
                return (TypeConverter)_Converters[fieldName];
            if (this.DataManager != null)
            {
                PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
                if (itemProperties != null)
                {
                    PropertyDescriptor descriptor = itemProperties.Find(fieldName, true);
                    if (descriptor != null)
                    {
                        _Converters.Add(fieldName, descriptor.Converter);
                        return descriptor.Converter;
                    }
                }
            }

            return null;
        }

        private void DataManager_ItemChanged(object sender, ItemChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ItemChangedEventHandler(DataManager_ItemChanged), sender, e);
                return;
            }
            if (_DataManager != null)
            {
                if (e == null || e.Index == -1)
                {
                    this.SetItemsCore(_DataManager.List);
                    if (this.AllowSelection)
                    {
                        if (_DataManager.Position == -1)
                            this.SelectedNode = null;
                        else
                            this.SelectedNode = this.FindNodeByBindingIndex(_DataManager.Position);
                    }
                }
                else
                {
                    this.SetItemCore(e.Index, _DataManager.List[e.Index]);
                }
            }
        }
        private void DataManager_PositionChanged(object sender, EventArgs e)
        {
            if ((_DataManager != null) && this.AllowSelection)
            {
                if (_DataManager.Position == -1)
                    this.SelectedNode = null;
                else
                    this.SelectedNode = this.FindNodeByBindingIndex(_DataManager.Position);
                //this.SelectedIndex = _DataManager.Position;
            }
        }

        /// <summary>
        /// When overridden in a derived class, resynchronizes the item data with the contents of the data source.
        /// </summary>
        public virtual void RefreshItems()
        {
            if (_DataManager != null)
            {
                SetItemsCore(_DataManager.List);
                if (this.AllowSelection)
                {
                    if (_DataManager.Position == -1)
                        this.SelectedNode = null;
                    else
                        this.SelectedNode = this.FindNodeByBindingIndex(_DataManager.Position);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the specified array of objects in a collection in the derived class.
        /// </summary>
        /// <param name="items">An array of items.</param>
        protected virtual void SetItemsCore(IList items)
        {
            this.BeginUpdate();
            this.Nodes.Clear();

            //bool isGrouping = !string.IsNullOrEmpty(_GroupingMembers);

            List<string> fieldNames = new List<string>();
            // Create Columns
            if (string.IsNullOrEmpty(this.DisplayMembers))
            {
                if (this.Columns.Count > 0)
                {
                    foreach (DevComponents.AdvTree.ColumnHeader columnHeader in this.Columns)
                    {
                        if (!string.IsNullOrEmpty(columnHeader.DataFieldName))
                            fieldNames.Add(columnHeader.DataFieldName);
                    }
                }
                if (fieldNames.Count == 0)
                {
                    this.Columns.Clear();
                    if (_DataManager.List is Array)
                    {
                        DevComponents.AdvTree.ColumnHeader ch = new DevComponents.AdvTree.ColumnHeader("Items");
                        ch.Width.Relative = 100;
                        this.Columns.Add(ch);
                        OnDataColumnCreated(new DevComponents.DotNetBar.Controls.DataColumnEventArgs(ch));
                    }
                    else if (_DataManager != null)
                    {
                        PropertyDescriptorCollection properties = _DataManager.GetItemProperties();
                        foreach (PropertyDescriptor prop in properties)
                        {
                            fieldNames.Add(prop.Name);
                            DevComponents.AdvTree.ColumnHeader ch = new DevComponents.AdvTree.ColumnHeader(StringHelper.GetFriendlyName(prop.Name));
                            ch.DataFieldName = prop.Name;
                            ch.Width.Relative = Math.Max(15, 100 / properties.Count);
                            this.Columns.Add(ch);
                            OnDataColumnCreated(new DevComponents.DotNetBar.Controls.DataColumnEventArgs(ch));
                        }
                    }
                }
            }
            else
            {
                this.Columns.Clear();
                if (_DisplayMembers != null && _DisplayMembers.Count > 0)
                {
                    foreach (BindingMemberInfo item in _DisplayMembers)
                    {
                        DevComponents.AdvTree.ColumnHeader ch = new DevComponents.AdvTree.ColumnHeader(StringHelper.GetFriendlyName(item.BindingMember));
                        ch.DataFieldName = item.BindingMember;
                        ch.Tag = item;
                        ch.Width.Relative = Math.Max(15, 100 / _DisplayMembers.Count);
                        this.Columns.Add(ch);
                        fieldNames.Add(item.BindingMember);
                        OnDataColumnCreated(new DevComponents.DotNetBar.Controls.DataColumnEventArgs(ch));
                    }
                }
            }

            if (string.IsNullOrEmpty(_GroupingMembers) && string.IsNullOrEmpty(_ParentFieldNames))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    object item = items[i];
                    Node node = CreateNode(this.Nodes, item, i, fieldNames);
                }
            }
            else if (!string.IsNullOrEmpty(_ParentFieldNames))
            {
                //isGrouping = true;

                Dictionary<string, Node> nodeParentCollection = new Dictionary<string, Node>();
                Dictionary<string, List<Node>> nodeNeedsParentCollection = new Dictionary<string, List<Node>>();
                string[] parentFields = _ParentFieldNames.Split(',');

                for (int i = 0; i < items.Count; i++)
                {
                    object item = items[i];
                    string nodeKey = GetItemText(item, parentFields[0]);
                    string parentKey = GetItemText(item, parentFields[1]);
                    Node parentNode = null;
                    if (nodeParentCollection.TryGetValue(parentKey, out parentNode))
                    {
                        Node node = CreateNode(parentNode.Nodes, item, i, fieldNames);
                        nodeParentCollection.Add(nodeKey, node);
                    }
                    else
                    {
                        Node node = CreateNode(this.Nodes, item, i, fieldNames);
                        List<Node> list = null;
                        if (!nodeNeedsParentCollection.TryGetValue(parentKey, out list))
                        {
                            list = new List<Node>();
                            nodeNeedsParentCollection.Add(parentKey, list);
                        }
                        list.Add(node);
                        nodeParentCollection.Add(nodeKey, node);
                    }
                }
                // If there are nodes that needed a parent process them now
                foreach (KeyValuePair<string, List<Node>> keyValue in nodeNeedsParentCollection)
                {
                    Node parentNode = null;
                    if (nodeParentCollection.TryGetValue(keyValue.Key, out parentNode))
                    {
                        foreach (Node node in keyValue.Value)
                        {
                            node.Remove();
                            parentNode.Nodes.Add(node);
                        }
                    }
                }
            }
            else
            {
                string[] groupFields = _GroupingMembers.Split(',');
                for (int i = 0; i < groupFields.Length; i++)
                {
                    groupFields[i] = groupFields[i].Trim();
                }
                Dictionary<string, Node> groupTable = new Dictionary<string, Node>();
                for (int i = 0; i < items.Count; i++)
                {
                    object item = items[i];
                    NodeCollection parentCollection = this.Nodes;

                    // Find the parent collection to add item to
                    string key = "";
                    for (int gi = 0; gi < groupFields.Length; gi++)
                    {
                        string text = GetItemText(item, groupFields[gi]);
                        key += text.ToLower() + "/";
                        Node groupNode = null;
                        if (!groupTable.TryGetValue(key, out groupNode))
                        {
                            groupNode = CreateGroupNode(parentCollection, text);
                            groupTable.Add(key, groupNode);
                        }
                        parentCollection = groupNode.Nodes;
                    }

                    Node node = CreateNode(parentCollection, item, i, fieldNames);
                }
            }

            //// If not grouping then remove the expand part space on left-hand side
            //if (isGrouping)
            //{
            //    this.ExpandWidth = 24;
            //}
            //else
            //{
            //    this.ExpandWidth = 0;
            //}

            this.EndUpdate();
        }

        /// <summary>
        /// Raises the DataColumnCreated event.
        /// </summary>
        /// <param name="args">Provides event arguments.</param>
        protected virtual void OnDataColumnCreated(DevComponents.DotNetBar.Controls.DataColumnEventArgs args)
        {
            if (DataColumnCreated != null)
                DataColumnCreated(this, args);
        }

        private Node CreateGroupNode(NodeCollection parentCollection, string text)
        {
            Node node = new Node();
            node.Text = text;
            node.Style = _GroupNodeStyle;
            node.Expanded = true;
            node.Selectable = false;
            parentCollection.Add(node);
            DevComponents.DotNetBar.Controls.DataNodeEventArgs eventArgs = new DevComponents.DotNetBar.Controls.DataNodeEventArgs(node, null);
            OnGroupNodeCreated(eventArgs);
            return node;
        }
        /// <summary>
        /// Raises the DataNodeCreated event.
        /// </summary>
        /// <param name="dataNodeEventArgs">Provides event arguments.</param>
        protected virtual void OnGroupNodeCreated(DevComponents.DotNetBar.Controls.DataNodeEventArgs dataNodeEventArgs)
        {
            if (GroupNodeCreated != null) GroupNodeCreated(this, dataNodeEventArgs);
        }

        /// <summary>
        /// Creates a new node for the data item.
        /// </summary>
        /// <param name="item">Item to create node for.</param>
        /// <returns>New instance of the node.</returns>
        private Node CreateNode(NodeCollection parentCollection, object item, int itemIndex, List<string> fieldNames)
        {
            Node node = new Node();
            parentCollection.Add(node);

            SetNodeData(node, item, fieldNames, itemIndex);

            DevComponents.DotNetBar.Controls.DataNodeEventArgs eventArgs = new DevComponents.DotNetBar.Controls.DataNodeEventArgs(node, item);

            OnDataNodeCreated(eventArgs);

            return eventArgs.Node;
        }

        private void SetNodeData(Node node, object item, List<string> fieldNames, int bindingIndex)
        {
            node.DataKey = item;
            node.BindingIndex = bindingIndex;

            node.CreateCells();

            if (fieldNames.Count > 0)
            {
                for (int i = 0; i < fieldNames.Count; i++)
                {
                    object propertyValue = GetPropertyValue(item, fieldNames[i]);
                    if (propertyValue is Image)
                        node.Cells[i].Images.Image = (Image)propertyValue;
                    else
                        node.Cells[i].Text = GetItemText(item, fieldNames[i]);
                }
            }
            else if (item != null)
                node.Text = item.ToString();
        }


        /// <summary>
        /// Raises the DataNodeCreated event.
        /// </summary>
        /// <param name="dataNodeEventArgs">Provides event arguments.</param>
        protected virtual void OnDataNodeCreated(DevComponents.DotNetBar.Controls.DataNodeEventArgs dataNodeEventArgs)
        {
            if (DataNodeCreated != null) DataNodeCreated(this, dataNodeEventArgs);
        }

        /// <summary>
        /// When overridden in a derived class, sets the object with the specified index in the derived class.
        /// </summary>
        /// <param name="index">The array index of the object.</param>
        /// <param name="value">The object.</param>
        protected virtual void SetItemCore(int index, object value)
        {
            Node node = this.FindNodeByBindingIndex(index);
            if (node == null) return;

            List<string> fieldNames = new List<string>();

            foreach (DevComponents.AdvTree.ColumnHeader column in this.Columns)
            {
                if (!string.IsNullOrEmpty(column.DataFieldName))
                    fieldNames.Add(column.DataFieldName);
            }

            SetNodeData(node, value, fieldNames, index);
        }

        private string _ParentFieldNames = "";
        /// <summary>
        /// Gets or sets comma separated field or property names that holds the value that is used to identify node and parent node. Format expected is: FieldNodeId,ParentNodeFieldId. For example if your table represents departments, you have DepartmentId field which uniquely identifies a department and ParentDepartmentId field which identifies parent of the department if any you would set this property to DepartmentId,ParentDepartmentId.
        /// Note that you can only use ParentFieldNames or GroupingMembers property but not both. If both are set ParentFieldName take precedence.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates comma separated field or property names that holds the value that is used to identify node and parent node. Format expected is: FieldNodeId,ParentNodeFieldId. For example if your table represents departments, you have DepartmentId field which uniquely identifies a department and ParentDepartmentId field which identifies parent of the department if any you would set this property to DepartmentId,ParentDepartmentId.")]
        public string ParentFieldNames
        {
            get { return _ParentFieldNames; }
            set
            {
                if (value == null) value = "";
                if (!string.IsNullOrEmpty(value))
                {
                    string[] fields = value.Split(',');
                    if (fields.Length != 2)
                        throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character.");
                    if(string.IsNullOrEmpty(fields[0]))
                        throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. First field name is empty.");
                    if (string.IsNullOrEmpty(fields[1]))
                        throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. Second field name is empty.");
                }
                _ParentFieldNames = value;
                OnParentFieldNamesChanged();
            }
        }

        /// <summary>
        /// Called when ParentFieldName property has changed.
        /// </summary>
        protected virtual void OnParentFieldNamesChanged()
        {
            RefreshItems();
        }

        private string _GroupingMembers = "";
        /// <summary>
        /// Gets or sets comma separated list of field or property names that are used for grouping when data-binding is used. Note that you can only use ParentFieldName or GroupingMembers property but not both. If both are set ParentFieldName take precedence.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates comma separated list of field or property names that are used for grouping when data-binding is used")]
        public string GroupingMembers
        {
            get { return _GroupingMembers; }
            set
            {
                if (value == null) value = "";
                _GroupingMembers = value;
                OnGroupingMembersChanged();
            }
        }

        /// <summary>
        /// Called when GroupingMembers property has changed.
        /// </summary>
        protected virtual void OnGroupingMembersChanged()
        {
            RefreshItems();
        }

        private ElementStyle _GroupNodeStyle = null;
        /// <summary>
        /// Gets or sets style for automatically created group nodes when data-binding is used and GroupingMembers property is set.
        /// </summary>
        /// <value>
        /// Name of the style assigned or null value indicating that no style is used.
        /// Default value is null.
        /// </value>
        [Browsable(true), Category("Node Style"), DefaultValue(null), Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Description("Gets or sets default style for the node.")]
        public ElementStyle GroupNodeStyle
        {
            get { return _GroupNodeStyle; }
            set
            {
                if (_GroupNodeStyle != value)
                {
                    _GroupNodeStyle = value;
                    if (_DataManager != null && this.Nodes != null)
                        RefreshItems();
                }
            }
        }

        private bool AllowSelection
        {
            get
            {
                return true;
            }
        }


        private CurrencyManager _DataManager = null;
        protected CurrencyManager DataManager
        {
            get
            {
                return _DataManager;
            }
        }
        private bool _IsDataSourceInitEventHooked = false;
        private void UnwireDataSource()
        {
            if (_DataSource is IComponent)
            {
                ((IComponent)_DataSource).Disposed -= new EventHandler(DataSourceDisposed);
            }
            ISupportInitializeNotification dataSource = _DataSource as ISupportInitializeNotification;
            if ((dataSource != null) && _IsDataSourceInitEventHooked)
            {
                dataSource.Initialized -= new EventHandler(DataSourceInitialized);
                _IsDataSourceInitEventHooked = false;
            }
        }
        private void DataSourceDisposed(object sender, EventArgs e)
        {
            this.SetDataConnection(null, null, true);
        }
        private bool _IsDataSourceInitialized = false;
        private void WireDataSource()
        {
            if (_DataSource is IComponent)
            {
                ((IComponent)_DataSource).Disposed += new EventHandler(DataSourceDisposed);
            }
            ISupportInitializeNotification dataSource = _DataSource as ISupportInitializeNotification;
            if ((dataSource != null) && !dataSource.IsInitialized)
            {
                dataSource.Initialized += new EventHandler(DataSourceInitialized);
                _IsDataSourceInitEventHooked = true;
                _IsDataSourceInitialized = false;
            }
            else
            {
                _IsDataSourceInitialized = true;
            }
        }
        private void DataSourceInitialized(object sender, EventArgs e)
        {
            this.SetDataConnection(_DataSource, _DisplayMembers, true);
        }
        /// <summary>
        /// Raises the Format event.
        /// </summary>
        /// <param name="e">Event parameters</param>
        protected virtual void OnFormat(DevComponents.DotNetBar.Controls.TreeConvertEventArgs e)
        {
            DevComponents.DotNetBar.Controls.TreeConvertEventHandler handler = Format;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private static TypeConverter stringTypeConverter;
        public string GetItemText(object item, string fieldName)
        {
            object propertyValue = GetPropertyValue(item, fieldName);
            if (!_FormattingEnabled)
            {
                if (item == null)
                {
                    return string.Empty;
                }
                if (propertyValue == null)
                {
                    return "";
                }
                return Convert.ToString(propertyValue, CultureInfo.CurrentCulture);
            }

            DevComponents.DotNetBar.Controls.TreeConvertEventArgs e = new DevComponents.DotNetBar.Controls.TreeConvertEventArgs(propertyValue, typeof(string), item, fieldName);
            this.OnFormat(e);
            if ((e.Value != item) && (e.Value is string))
            {
                return (string)e.Value;
            }
            if (stringTypeConverter == null)
            {
                stringTypeConverter = TypeDescriptor.GetConverter(typeof(string));
            }
            try
            {
                return (string)FormatHelper.FormatObject(propertyValue, typeof(string), GetFieldConverter(fieldName), stringTypeConverter, _FormatString, _FormatInfo, null, DBNull.Value);
            }
            catch (Exception ex)
            {
                if (ex is SecurityException || IsCriticalException(ex))
                {
                    throw;
                }
                return ((propertyValue != null) ? Convert.ToString(item, CultureInfo.CurrentCulture) : "");
            }
        }
        private static bool IsCriticalException(Exception ex)
        {
            return (((((ex is NullReferenceException) || (ex is StackOverflowException)) || ((ex is OutOfMemoryException) || (ex is System.Threading.ThreadAbortException))) || ((ex is ExecutionEngineException) || (ex is IndexOutOfRangeException))) || (ex is AccessViolationException));
        }
        protected object GetPropertyValue(object item, string fieldName)
        {
            if ((item != null) && (fieldName.Length > 0))
            {
                try
                {
                    PropertyDescriptor descriptor;
                    if (_DataManager != null)
                    {
                        descriptor = _DataManager.GetItemProperties().Find(fieldName, true);
                    }
                    else
                    {
                        descriptor = TypeDescriptor.GetProperties(item).Find(fieldName, true);
                    }
                    if (descriptor != null)
                    {
                        item = descriptor.GetValue(item);
                    }
                }
                catch
                {
                }
            }
            return item;
        }
        /// <summary>
        /// Gets or sets the index specifying the currently selected item.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets or sets the index specifying the currently selected item.")]
        public int SelectedIndex
        {
            get
            {
                if (_DataManager != null)
                {
                    if (this.SelectedNode == null || this.SelectedNode.BindingIndex < 0)
                        return -1;
                    return this.SelectedNode.BindingIndex;
                }
                if (this.SelectedNode == null) return -1;
                return this.GetNodeFlatIndex(this.SelectedNode);
            }
            set
            {
                SetSelectedIndex(value);
            }
        }
        private void SetSelectedIndex(int value)
        {
            if (value == -1)
            {
                this.SelectedNode = null;
                return;
            }
            Node node = null;
            if (_DataManager != null)
            {
                node = this.FindNodeByBindingIndex(value);
            }
            else
            {
                node = this.GetNodeByFlatIndex(value);
            }
            this.SelectedNode = node;
        }
        /// <summary>
        /// Raises the SelectedIndexChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = SelectedIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private BindingMemberInfo _ValueMember = new BindingMemberInfo();
        /// <summary>
        /// Gets or sets the property to use as the actual value for the items in the control. Applies to data-binding scenarios. SelectedValue property will return the value of selected node as indicated by this property.
        /// </summary>
        [Category("Data"), DefaultValue(""), Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)), Description("property to use as the actual value for the items in the control. Applies to data-binding scenarios. SelectedValue property will return the value of selected node as indicated by this property.")]
        public string ValueMember
        {
            get
            {
                return _ValueMember.BindingMember;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                BindingMemberInfo newValueMember = new BindingMemberInfo(value);
                if (!newValueMember.Equals(_ValueMember))
                {
                    if (this.DisplayMembers.Length == 0)
                    {
                        List<BindingMemberInfo> list = new List<BindingMemberInfo>();
                        list.Add(newValueMember);
                        this.SetDataConnection(this.DataSource, list, false);
                    }
                    if (((_DataManager != null) && (value != null)) && ((value.Length != 0) && !this.BindingMemberInfoInDataManager(newValueMember)))
                    {
                        throw new ArgumentException("Invalid value for ValueMember", "value");
                    }
                    _ValueMember = newValueMember;
                    this.OnValueMemberChanged(EventArgs.Empty);
                    this.OnSelectedValueChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raises the ValueMemberChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnValueMemberChanged(EventArgs e)
        {
            EventHandler handler = ValueMemberChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Raises the SelectedValueChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedValueChanged(EventArgs e)
        {
            EventHandler handler = SelectedValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Gets or sets the value of the member property specified by the ValueMember property.
        /// </summary>
        [Browsable(false), DefaultValue(null), Bindable(true), Category("Data"), Description("Indicates value of the member property specified by the ValueMember property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue
        {
            get
            {
                if (_DataManager != null && this.SelectedIndex != -1)
                {
                    object item = _DataManager.List[this.SelectedIndex];
                    return this.GetPropertyValue(item, _ValueMember.BindingField);
                }
                return null;
            }
            set
            {
                if (_DataManager != null)
                {
                    string bindingField = _ValueMember.BindingField;
                    if (string.IsNullOrEmpty(bindingField))
                    {
                        throw new InvalidOperationException("ValueMember property must be set to be able to set SelectedValue");
                    }
                    PropertyDescriptor property = _DataManager.GetItemProperties().Find(bindingField, true);
                    System.Reflection.MethodInfo mi = _DataManager.GetType().GetMethod("Find", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    int num = -1;
                    if (mi != null)
                    {
                        num = (int)mi.Invoke(_DataManager, new object[] { property, value, true });
                    }
                    else
                    {
                        //int num = _DataManager.Find(property, value, true);
                        // Provide an alternate implementation...
                    }
                    this.SelectedIndex = num;
                }
            }
        }
#endif
        #endregion
    }

    #region AdvTreeSettings
    /// <summary>
    /// Static class that holds AdvTree settings that are not commonly used.
    /// </summary>
    public class AdvTreeSettings
    {
        /// <summary>
        /// Gets or sets whether tree control is scrolled horizontally so selected node is brought into the view. Default value is true.
        /// You can set this property to false to disable the horizontal scrolling of tree control when selected node has changed.
        /// </summary>
        public static bool SelectedScrollIntoViewHorizontal = true;
    }
    #endregion

    #region ICellEditControl
    /// <summary>
    /// Defines an interface for cell edit control that allows custom controls to be used as cell editors. AdvTree control
    /// expects that editing control inherits from System.Windows.Forms.Control.
    /// </summary>
    public interface ICellEditControl
    {
        /// <summary>
        /// Called when edit operation is started. The AdvTree control will first set CurrentValue, then call BeginEdit and will call EditComplete once
        /// editing is completed.
        /// </summary>
        void BeginEdit();
        /// <summary>
        /// Called when edit operation is completed.
        /// </summary>
        void EndEdit();
        /// <summary>
        /// Gets or sets current edit value.
        /// </summary>
        object CurrentValue { get;set;}
        /// <summary>
        /// AdvTree control subscribes to this event to be notified when edit operation is completed. For example when Enter key is
        /// pressed the edit control might raise this event to indicate the completion of editing operation.
        /// </summary>
        event EventHandler EditComplete;
        /// <summary>
        /// AdvTree control subscribes to this event to be notified that user has cancelled the editing. For example when Escape key is
        /// pressed the edit control might raise this event to indicate that editing has been cancelled.
        /// </summary>
        event EventHandler CancelEdit;
        /// <summary>
        /// Gets or sets whether cell requests the word-wrap based on the current cell style. If your editor does not support
        /// word-wrap functionality this can be ignored.
        /// </summary>
        bool EditWordWrap { get;set;}
    }
    #endregion

    #region PrepareCellEditor
    /// <summary>
    /// Defines delegate for PrepareCellEditor event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    public delegate void PrepareCellEditorEventHandler(object sender, PrepareCellEditorEventArgs e);
    /// <summary>
    /// Event arguments for PrepareCellEditor event.
    /// </summary>
    public class PrepareCellEditorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets reference to the cell being edited.
        /// </summary>
        public readonly Cell EditedCell;
        /// <summary>
        /// Gets reference to the cell editor control.
        /// </summary>
        public readonly ICellEditControl Editor;
        /// <summary>
        /// Initializes a new instance of the PrepareCellEditorEventArgs class.
        /// </summary>
        /// <param name="editedCell"></param>
        /// <param name="editor"></param>
        public PrepareCellEditorEventArgs(Cell editedCell, ICellEditControl editor)
        {
            EditedCell = editedCell;
            Editor = editor;
        }
    }
    #endregion
}