using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.Tree.Display;
using DevComponents.Tree.Layout;
using System.Xml;
using System.IO;

namespace DevComponents.Tree
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	[ToolboxItem(true),DefaultEvent("Click"),System.Runtime.InteropServices.ComVisible(false),Designer(typeof(Design.TreeGXDesigner)),ToolboxBitmap(typeof(TreeGX),"TreeGX.ico")]
	public class TreeGX : System.Windows.Forms.ContainerControl, INodeNotify, ISupportInitialize
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
		//private Cell m_SelectedCell=null;
		private Node m_MouseOverNode=null;
		private Cell m_MouseOverCell=null;
		private int m_CellMouseDownCounter=0;

		private ImageList m_ImageList=null;
		private int m_ImageIndex=-1;
		private bool m_AntiAlias=true;
		
		private NodeConnector m_RootConnector=null;
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
		private Color m_SelectionBoxBorderColor=Color.Empty;
		private Color m_SelectionBoxFillColor=Color.Empty;

		// Expand Part Properties
		private Size m_DefaultExpandPartSize=new Size(9,9);
		private Size m_ExpandButtonSize=Size.Empty;
		private Color m_ExpandBorderColor=Color.Empty;
		private eColorSchemePart m_ExpandBorderColorSchemePart=eColorSchemePart.BarDockedBorder;
		private Color m_ExpandBackColor=Color.Empty;
		private eColorSchemePart m_ExpandBackColorSchemePart=eColorSchemePart.None;
		private Color m_ExpandBackColor2=Color.Empty;
		private eColorSchemePart m_ExpandBackColor2SchemePart=eColorSchemePart.None;
		private int m_ExpandBackColorGradientAngle=0;
		private Color m_ExpandLineColor=Color.Empty;
		private eColorSchemePart m_ExpandLineColorSchemePart=eColorSchemePart.BarDockedBorder;
		private Image m_ExpandImage=null;
		private Image m_ExpandImageCollapse=null;
		private eExpandButtonType m_ExpandButtonType=eExpandButtonType.Ellipse;

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
		private NodeRenderer m_NodeRenderer=null;
		
		private ArrayList m_HostedControlCells=new ArrayList();
		private float m_ZoomFactor=1f;
		private object m_DotNetBarManager=null;
		#endregion

		#region Events
		/// <summary>
		/// Occurs after the cell check box is checked.
		/// </summary>
		public event TreeGXCellEventHandler AfterCheck;
		
		/// <summary>
		/// Occurs after the tree node is collapsed.
		/// </summary>
		public event TreeGXNodeEventHandler AfterCollapse;
		
		/// <summary>
		/// Occurs before the tree node is collapsed.
		/// </summary>
		public event TreeGXNodeCancelEventHandler BeforeCollapse;
		
		/// <summary>
		/// Occurs after the tree node is expanded.
		/// </summary>
		public event TreeGXNodeEventHandler AfterExpand;
		
		/// <summary>
		/// Occurs before the tree node is expanded.
		/// </summary>
		public event TreeGXNodeCancelEventHandler BeforeExpand;

		/// <summary>
		/// Occurs when command button on node is clicked.
		/// </summary>
		public event CommandButtonEventHandler CommandButtonClick;

		/// <summary>
		/// Occurs before cell is edited. The order of the cell editing events is as follows:
		/// BeforeCellEdit, CellEditEnding, AfterCellEdit.
		/// </summary>
		public event CellEditEventHandler BeforeCellEdit;
		/// <summary>
		/// Occurs just before the cell editing is ended. The text box for editing is still visible and you can cancel
		/// the exit out of editing mode at this point. The order of the cell editing events is as follows:
		/// BeforeCellEdit, CellEditEnding, AfterCellEdit.
		/// </summary>
		public event CellEditEventHandler CellEditEnding;
		/// <summary>
		/// Occurs after cell editing has ended and before the new text entered by the user is assigned to the cell. You can abort the edits in this event.
		/// The order of the cell editing events is as follows:
		/// BeforeCellEdit, CellEditEnding, AfterCellEdit.
		/// </summary>
		public event CellEditEventHandler AfterCellEdit;
		
		/// <summary>
		/// Occurs after Node has been selected by user or through the SelectedNode property. Event can be cancelled.
		/// </summary>
		public event TreeGXNodeCancelEventHandler BeforeNodeSelect;
		
		/// <summary>
		/// Occurs after node has been selected by user or through the SelectedNode property.
		/// </summary>
		public event TreeGXNodeEventHandler AfterNodeSelect;
		
		/// <summary>
		/// Occurs before node has been removed from its parent.
		/// </summary>
		public event TreeGXNodeCollectionEventHandler BeforeNodeRemove;

		/// <summary>
		/// Occurs after node has been removed from its parent.
		/// </summary>
		public event TreeGXNodeCollectionEventHandler AfterNodeRemove;

		/// <summary>
		/// Occurs before node is inserted or added as child node to parent node.
		/// </summary>
		public event TreeGXNodeCollectionEventHandler BeforeNodeInsert;

		/// <summary>
		/// Occurs after node is inserted or added as child node.
		/// </summary>
		public event TreeGXNodeCollectionEventHandler AfterNodeInsert;
		
		/// <summary>
		/// Occurs before Drag-Drop of a node is completed and gives you information about new parent of the node that is being dragged
		/// as well as opportunity to cancel the operation.
		/// </summary>
		public event TreeGXDragDropEventHandler BeforeNodeDrop;
		
		/// <summary>
		/// Occurs after Drag-Drop of a node is completed. This operation cannot be cancelled.
		/// </summary>
		public event TreeGXDragDropEventHandler AfterNodeDrop;

		/// <summary>
		/// Occurs when the mouse pointer is over the node and a mouse button is pressed.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeMouseDown;
		
		/// <summary>
		/// Occurs when the mouse pointer is over the node and a mouse button is released.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeMouseUp;
		
		/// <summary>
		/// Occurs when the mouse pointer is moved over the node.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeMouseMove;
		
		/// <summary>
		/// Occurs when the mouse enters the node.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeMouseEnter;
		
		/// <summary>
		/// Occurs when the mouse leaves the node.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeMouseLeave;
		
		/// <summary>
		/// Occurs when the mouse hovers over the node.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeMouseHover;
		
		/// <summary>
		/// Occurs when the node is clicked with left mouse button. If you need to know more information like if another mouse button is clicked etc. use
		/// NodeMouseDown event.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeClick;
		
		/// <summary>
		/// Occurs when the node is double-clicked.
		/// </summary>
		public event TreeGXNodeMouseEventHandler NodeDoubleClick;
		
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
		/// property on SerializeItemEventArgs to retrive any data you saved in SerializeNode event.</para>
		/// </remarks>
		public event SerializeNodeEventHandler DeserializeNode;
		
		/// <summary>
		/// Occurs when hyperlink in text-markup is clicked.
		/// </summary>
		public event MarkupLinkClickEventHandler MarkupLinkClick;
		#endregion

		#region Constructor/Dispose
		/// <summary>Creates new instance of the class.</summary>
		public TreeGX()
		{
			ColorFunctions.Initialize();

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

			// Setup layout helper
			//m_NodeLayout=new NodeTreeLayout(this,this.ClientRectangle);
			//m_NodeLayout=new NodeListLayout(this,this.ClientRectangle);
			//m_NodeLayout=new NodeDiagramLayout(this,this.ClientRectangle);
			m_NodeLayout=new NodeMapLayout(this,this.ClientRectangle);
			m_NodeLayout.NodeHorizontalSpacing=m_NodeHorizontalSpacing;
			m_NodeLayout.NodeVerticalSpacing=m_NodeVerticalSpacing;

			m_NodeLayout.LeftRight=this.RtlTranslateLeftRight(LeftRightAlignment.Left);

#if TRIAL
			NodeOperations.ColorExpAlt();
#endif
			// Setup display helper
			m_NodeDisplay=new NodeTreeDisplay(this);

			m_Headers=new HeadersCollection();

			m_Nodes.TreeControl=this;
			m_Styles.TreeControl=this;

			m_ColorScheme=new ColorScheme(eColorSchemeStyle.Office2003);

			m_SelectionBoxBorderColor=GetDefaultSelectionBoxBorderColor();
			m_SelectionBoxFillColor=GetDefaultSelectionBoxFillColor();

			m_ExpandButtonSize=GetDefaultExpandButtonSize();

			this.AllowDrop = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
		/// Gets or sets the reference to DotNetBar DotNetBarManager component which is used to provide context menu for nodes. This property
		/// is automatically maintained by TreeGX.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null), Browsable(false)]
		public object DotNetBarManager
		{
			get { return m_DotNetBarManager; }
			set { m_DotNetBarManager=value; }
		}
		
		/// <summary>
		/// Gets or sets zoom factor for the control. Default value is 1. To zoom display of the nodes for 20% set zoom factor to 1.2
		/// To zoom view 2 times set zoom factor to 2. Value must be greater than 0.
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
		public NodeRenderer NodeRenderer
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
		/// Note that if you specify custom renderer you need to set TreeGX.NodeRenderer property to your custom renderer.
		/// </summary>
		[Browsable(true), Category("Style"), DefaultValue(eNodeRenderMode.Default), Description("Indicates render mode used to render the node.")]
		public eNodeRenderMode RenderMode
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
		/// Gets or sets the flow of nodes when Diagram layout is used. Note that this setting applies only to Diagram layout type.
		/// </summary>
		[Browsable(true),Category("Layout"),DefaultValue(eDiagramFlow.LeftToRight),Description("Indicates flow of nodes when Diagram layout is used.")]
		public eDiagramFlow DiagramLayoutFlow
		{
			get {return m_DiagramLayoutFlow;}
			set
			{
				m_DiagramLayoutFlow=value;
				OnLayoutChanged();
			}
		}

		/// <summary>
		/// Gets or sets the flow of nodes when Map layout is used. Note that this setting applies only to Map layout type.
		/// </summary>
		[Browsable(true),Category("Layout"),DefaultValue(eMapFlow.Spread),Description("Indicates flow of nodes when Map layout is used.")]
		public eMapFlow MapLayoutFlow
		{
			get {return m_MapLayoutFlow;}
			set
			{
				m_MapLayoutFlow=value;
				OnLayoutChanged();
			}
		}

		/// <summary>
		/// Gets or sets the horizontal spacing in pixels between nodes. Default value is 32.
		/// </summary>
		[Browsable(true),Category("Layout"),DefaultValue(32),Description("Indicates horizontal spacing in pixels between nodes.")]
		public int NodeHorizontalSpacing
		{
			get {return m_NodeHorizontalSpacing;}
			set
			{
				m_NodeHorizontalSpacing=value;
				OnLayoutChanged();
			}
		}

		/// <summary>
		/// Gets or sets the vertical spacing in pixels between nodes. Default value is 16.
		/// </summary>
		[Browsable(true),Category("Layout"),DefaultValue(32),Description("Indicates vertical spacing in pixels between nodes.")]
		public int NodeVerticalSpacing
		{
			get {return m_NodeVerticalSpacing;}
			set
			{
				m_NodeVerticalSpacing=value;
				OnLayoutChanged();
			}
		}

		/// <summary>
		/// Gets or sets the layout type for the nodes.
		/// </summary>
		[Browsable(true),Category("Layout"),DefaultValue(eNodeLayout.Map),Description("Indicates layout type for the nodes.")]
		public eNodeLayout LayoutType
		{
			get {return m_Layout;}
			set
			{
				if(m_Layout!=value)
				{
					SetLayout(value);
				}
			}
		}

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

		/// <summary>
		/// Gets the collection of all column headers that appear in the tree.
		/// </summary>
		/// <remarks>
		/// 	<para>By default there are no column headers defined. In that case tree control
		///     functions as regular tree control where text has unrestricted width.</para>
		/// 	<para>If you want to restrict the horizontal width of the text but not display
		///     column header you can create one column and set its width to the width desired and
		///     set its Visible property to false.</para>
		/// </remarks>
		public ColumnHeaderCollection Columns
		{
			get
			{
				return m_Columns;
			}
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell when mouse is pressed.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell when mouse is over the cell.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell when cell is selected.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates default style for the node cell when cell is disabled.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets default style for the node cell when node that cell belongs to is expanded.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets default style for the node.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets style of the node when node is selected.")]
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
		[Browsable(true),Category("Node Style"),DefaultValue(null),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Gets or sets style of the node when mouse is over node.")]
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
		[Browsable(true),DefaultValue(null),Category("Column Header Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the column.")]
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
		[Browsable(true),DefaultValue(null),Category("Column Header Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the column when mouse is down.")]
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
		[Browsable(true),DefaultValue(null),Category("Column Header Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the cell when mouse is over the column.")]
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
		public HeadersCollection Headers
		{
			get {return m_Headers;}
		}

		/// <summary>
		/// Gets or sets the tree node that is currently selected in the tree view control.
		/// </summary>
		/// <remarks>
		/// 	<para>If no <see cref="Node">Node</see> is currently selected, the
		///     <b>SelectedNode</b> property is a null reference (<b>Nothing</b> in Visual
		///     Basic).</para>
		/// </remarks>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Node SelectedNode
		{
			get {return m_SelectedNode;}
			set
			{
				if(m_SelectedNode!=value)
				{
					SelectNode(value,eTreeAction.Code);
				}
			}
		}

		/// <summary>
		/// Invalidates node bounds on canvas.
		/// </summary>
		/// <param name="node">Reference node.</param>
		internal void InvalidateNode(Node node)
		{
			if(node==null)
				return;

			Rectangle r;
			//if(node.Parent!=null)
			if(node==m_DragNode && m_DragNode.Parent==null)
				r=m_DragNode.BoundsRelative;
			else
				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,node,m_NodeDisplay.Offset);
			//else
			//	r=node.Bounds;

			if(m_SelectionBox)
				r.Inflate(m_SelectionBoxSize,m_SelectionBoxSize);
			
			r = GetScreenRectangle(r);
			
			this.Invalidate(r);
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
		internal NodeDisplay NodeDisplay
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
		/// Gets or sets whether paint operations are suspended for the control. You should use this method
		/// if you need the RecalcLayout operations to proceed but you want to stop painting of the control.
		/// </summary>
		[Browsable(false)]
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
		[Browsable(true),Category("Images"),Description("Indicates the image-list index value of the default image that is displayed by the tree nodes."),Editor(typeof(ImageIndexEditor), typeof(System.Drawing.Design.UITypeEditor)),TypeConverter(typeof(ImageIndexConverter)),DefaultValue(-1)]
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

		/// <summary>
		/// Gets or sets the NodeConnector object that describes the type of the connector used for
		/// displaying connection between root node and it's nested nodes. The root node is defined as
		/// the top level node i.e. which belongs directly to TreeGX.Nodes collection. Default value is null.
		/// </summary>
		/// <remarks>
		/// You can use
		/// <a href="TreeGX~DevComponents.Tree.Node~ParentConnector.html">Node.ParentConnector</a>
		/// property to specify per node connectors.
		/// </remarks>
		[Browsable(true),Category("Connectors"),DefaultValue(null),Editor(typeof(Design.NodeConnectorTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the root node connector.")]
		public NodeConnector RootConnector
		{
			get {return m_RootConnector;}
			set
			{
				if(m_RootConnector!=value)
				{
					if(m_RootConnector!=null)
						m_RootConnector.AppearanceChanged-=new EventHandler(this.ConnectorAppearanceChanged);
					if(value!=null)
						value.AppearanceChanged+=new EventHandler(this.ConnectorAppearanceChanged);
					m_RootConnector=value;
					this.OnDisplayChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the NodeConnector object that describes the type of the connector used for
		/// displaying connection between nested nodes. RootConnector property specifies the connector
		/// between root node and it's imidate nested nodes. This property specifies connector for all other nested levels.
		/// Default value is null.
		/// </summary>
		/// <remarks>
		/// You can use
		/// <a href="TreeGX~DevComponents.Tree.Node~ParentConnector.html">Node.ParentConnector</a>
		/// property to specify per node connectors.
		/// </remarks>
		[Browsable(true),Category("Connectors"),DefaultValue(null),Editor(typeof(Design.NodeConnectorTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the nested nodes connector.")]
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

		/// <summary>
		/// Gets or sets the NodeConnector object that describes the type of the connector used for
		/// displaying connection between linked nodes. This property specifies connector for all linked nodes.
		/// Default value is null.
		/// </summary>
		[Browsable(true),Category("Connectors"),DefaultValue(null),Editor(typeof(Design.NodeConnectorTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the linked nodes connector.")]
		public NodeConnector LinkConnector
		{
			get {return m_LinkConnector;}
			set
			{
				if(m_LinkConnector!=value)
				{
					if(m_LinkConnector!=null)
						m_LinkConnector.AppearanceChanged-=new EventHandler(this.ConnectorAppearanceChanged);
					if(value!=null)
						value.AppearanceChanged+=new EventHandler(this.ConnectorAppearanceChanged);
					m_LinkConnector=value;
					this.OnDisplayChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the NodeConnector object that describes the type of the connector used for
		/// displaying connection between linked nodes. Connector specified here is used to display the connection
		/// between nodes that are on the path to the selected node. When set you can use it to visually indicate the path to the currently selected node.
		/// Default value is null.
		/// </summary>
		[Browsable(true),Category("Connectors"),DefaultValue(null),Editor(typeof(Design.NodeConnectorTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the linked nodes connector.")]
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
		/// <a href="TreeGX~DevComponents.Tree.Node~CellLayout.html">Node.CellLayout</a>
		/// property.
		/// </remarks>
		[Browsable(true),DefaultValue(eCellLayout.Default),Category("Appearance"),Description("Indicates layout of the cells inside the node.")]
		public eCellLayout CellLayout
		{
			get {return m_CellLayout;}
			set
			{
				m_CellLayout=value;
				this.RecalcLayout();
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the layout of the cells inside the node. Default value is Horizontal layout which
		/// means that cell are positioned horizontally next to each other.
		/// </summary>
		/// <remarks>
		/// You can specify cell layout on each node by using
		/// <a href="TreeGX~DevComponents.Tree.Node~CellLayout.html">Node.CellLayout</a>
		/// property.
		/// </remarks>
		[Browsable(true),DefaultValue(eCellPartLayout.Default),Category("Appearance"),Description("Indicates layout of the cells inside the node.")]
		public eCellPartLayout CellPartLayout
		{
			get {return m_CellPartLayout;}
			set
			{
				m_CellPartLayout=value;
				this.RecalcLayout();
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the color scheme style. Color scheme provides predefined colors based on popular visual styles.
		/// We recommend that you use "SchemePart" color settings since they maintain consistant look that is
		/// based on target system color scheme setting.
		/// </summary>
		[Browsable(true),DefaultValue(eColorSchemeStyle.Office2003),Category("Appearance"),Description("Indicates the color scheme style.")]
		public eColorSchemeStyle ColorSchemeStyle
		{
			get {return m_ColorScheme.Style;}
			set
			{
				m_ColorScheme.Style=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}

		/// <summary>
		/// Gets the reference to the color scheme object.
		/// </summary>
		internal ColorScheme ColorScheme
		{
			get {return m_ColorScheme;}
		}

		/// <summary>
		/// Gets or sets whether the content of the control is centered within the bounds of control. Default value is true.
		/// </summary>
		[Browsable(true),DefaultValue(true),Description("Indicates whether the content of the control is centered within the bounds of control."),Category("Layout")]
		public bool CenterContent
		{
			get {return m_CenterContent;}
			set
			{
				m_CenterContent=value;
				OnLayoutChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that indicates whether selection box is drawn around the
		/// selected node. Default value is true. Another way to provide the visual indication that
		/// node is selected is by using selected state style properties like
		/// <a href="TreeGX~DevComponents.Tree.TreeGX~NodeStyleSelected.html">NodeStyleSelected</a>
		/// and
		/// <a href="TreeGX~DevComponents.Tree.TreeGX~CellStyleSelected.html">CellStyleSelected</a>.
		/// </summary>
		/// <seealso cref="CellStyleSelected">CellStyleSelected Property</seealso>
		/// <seealso cref="NodeStyleSelected">NodeStyleSelected Property</seealso>
		[Browsable(true),DefaultValue(true),Category("Selected Node"),Description("Indicates whether selection box is drawn around selected node.")]
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
		[Browsable(true),DefaultValue(4),Category("Selected Node"),Description("Indicates the size in pixels of the selection box drawn around selected node.")]
		public int SelectionBoxSize
		{
			get {return m_SelectionBoxSize;}
			set
			{
				if(m_SelectionBoxSize!=value)
				{
					m_SelectionBoxSize=value;
					this.OnSelectionBoxChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the selection box border color.
		/// </summary>
		[Browsable(true),Category("Selected Node"),Description("Indicates the selection box border color.")]
		public Color SelectionBoxBorderColor
		{
			get {return m_SelectionBoxBorderColor;}
			set
			{
				if(m_SelectionBoxBorderColor!=value)
				{
					m_SelectionBoxBorderColor=value;
					this.OnSelectionBoxChanged();
				}
			}

		}
		/// <summary>
		/// Indicates whether SelectionBoxBorderColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSelectionBoxBorderColor()
		{return (m_SelectionBoxBorderColor!=GetDefaultSelectionBoxBorderColor());}
		/// <summary>
		/// Resets SelectionBoxBorderColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetSelectionBoxBorderColor()
		{
			m_SelectionBoxBorderColor=GetDefaultSelectionBoxBorderColor();
		}

		/// <summary>
		/// Gets or sets the selection box fill color.
		/// </summary>
		[Browsable(true),Category("Selected Node"),Description("Indicates the selection box fill color.")]
		public Color SelectionBoxFillColor
		{
			get {return m_SelectionBoxFillColor;}
			set
			{
				if(m_SelectionBoxFillColor!=value)
				{
					m_SelectionBoxFillColor=value;
					this.OnSelectionBoxChanged();
				}
			}

		}
		/// <summary>
		/// Indicates whether SelectionBoxFillColor should be serialized. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSelectionBoxFillColor()
		{return (m_SelectionBoxFillColor!=GetDefaultSelectionBoxFillColor());}
		/// <summary>
		/// Resets SelectionBoxFillColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetSelectionBoxFillColor()
		{
			m_SelectionBoxFillColor=GetDefaultSelectionBoxFillColor();
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
				if(m_ExpandButtonSize!=value && !m_ExpandButtonSize.IsEmpty)
				{
					m_ExpandButtonSize=value;
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
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through ExpandBorderColor property.
		/// </summary>
		[Browsable(true),Category("Expand Button"),DefaultValue(eColorSchemePart.BarDockedBorder),Description("Indicates expand button border color.")]
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
		{return (!m_ExpandBackColor.IsEmpty && m_ExpandBackColorSchemePart==eColorSchemePart.None);}
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
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
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
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
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
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
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
		[Browsable(true),DefaultValue(eExpandButtonType.Ellipse),Category("Expand Button"),Description("Indicates type of the expand button used to expand/collapse nodes.")]
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
		/// Gets or sets the display root node. For example in Map or Diagram layout single node
		/// is always used as root node for display. Setting this property allows you to use any
		/// Node as root display node. Default value is Null which means that first node from
		/// TreeGX.Nodes collection is used as display root node.
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
		public int CommandWidth
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
		public Color CommandBackColor
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
		public bool ShouldSerializeCommandBackColor()
		{return (!m_CommandBackColor.IsEmpty && m_CommandBackColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandBackColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetCommandBackColor()
		{
			this.CommandBackColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme back color. Setting
		/// this property overrides the setting of the corresponding CommandBackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandBackColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.CustomizeBackground),Description("Indicates command button back color.")]
		public eColorSchemePart CommandBackColorSchemePart
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
		public Color CommandBackColor2
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
		public bool ShouldSerializeCommandBackColor2()
		{return (!m_CommandBackColor2.IsEmpty && m_CommandBackColor2SchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandBackColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetCommandBackColor2()
		{
			this.CommandBackColor2=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme target gradient back color. Setting
		/// this property overrides the setting of the corresponding CommandBackColor2 property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandBackColor2 property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.CustomizeBackground2),Description("Indicates command button target gradient back color.")]
		public eColorSchemePart CommandBackColor2SchemePart
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
		public Color CommandForeColor
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
		public bool ShouldSerializeCommandForeColor()
		{return (!m_CommandForeColor.IsEmpty && m_CommandForeColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandForeColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetCommandForeColor()
		{
			this.CommandForeColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme foreground color. Setting
		/// this property overrides the setting of the corresponding CommandForeColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandForeColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.CustomizeText),Description("Indicates command button foreground color.")]
		public eColorSchemePart CommandForeColorSchemePart
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
		public int CommandBackColorGradientAngle
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
		public Color CommandMouseOverBackColor
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
		public bool ShouldSerializeCommandMouseOverBackColor()
		{return (!m_CommandMouseOverBackColor.IsEmpty && m_CommandMouseOverBackColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandMouseOverBackColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetCommandMouseOverBackColor()
		{
			this.CommandMouseOverBackColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button color scheme mouse over back color. Setting
		/// this property overrides the setting of the corresponding CommandMouseOverBackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandMouseOverBackColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.ItemHotBackground),Description("Indicates command button mouse over back color.")]
		public eColorSchemePart CommandMouseOverBackColorSchemePart
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
		public Color CommandMouseOverBackColor2
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
		public bool ShouldSerializeCommandMouseOverBackColor2()
		{return (!m_CommandMouseOverBackColor2.IsEmpty && m_CommandMouseOverBackColor2SchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandMouseOverBackColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetCommandMouseOverBackColor2()
		{
			this.CommandMouseOverBackColor2=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button mouse over color scheme target gradient back color. Setting
		/// this property overrides the setting of the corresponding CommandMouseOverBackColor2 property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandMouseOverBackColor2 property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.ItemPressedBackground2),Description("Indicates command button mouse over target gradient back color.")]
		public eColorSchemePart CommandMouseOverBackColor2SchemePart
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
		public Color CommandMouseOverForeColor
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
		public bool ShouldSerializeCommandMouseOverForeColor()
		{return (!m_CommandMouseOverForeColor.IsEmpty && m_CommandMouseOverForeColorSchemePart==eColorSchemePart.None);}
		/// <summary>
		/// Resets CommandMouseOverForeColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetCommandMouseOverForeColor()
		{
			this.CommandMouseOverForeColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets command button mouse over color scheme foreground color. Setting
		/// this property overrides the setting of the corresponding CommandMouseOverForeColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// <a href="TreeGX~DevComponents.Tree.eColorSchemePart.html">eColorSchemePart.None</a> to
		/// specify explicit color to use through CommandMouseOverForeColor property.
		/// </summary>
		[Browsable(true),Category("Command Button"),DefaultValue(eColorSchemePart.ItemHotText),Description("Indicates command button mouse over foreground color.")]
		public eColorSchemePart CommandMouseOverForeColorSchemePart
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
		public int CommandMouseOverBackColorGradientAngle
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
		#endregion

		#region Private implementation
		/// <summary>
		/// Returns color scheme part color if set otherwise returns color passed in.
		/// </summary>
		/// <param name="color">Color.</param>
		/// <param name="p">Color scheme part.</param>
		/// <returns>Color.</returns>
		internal Color GetColor(Color color, eColorSchemePart p)
		{
			if(p==eColorSchemePart.None)
				return color;
			ColorScheme cs=this.ColorScheme;
			if(cs==null)
				return color;
			return (Color)cs.GetType().GetProperty(p.ToString()).GetValue(cs,null);
		}
		private void OnExpandButtonChanged()
		{
			m_NodeLayout.ExpandPartSize=m_ExpandButtonSize;
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
			m_NodeLayout.NodeHorizontalSpacing=m_NodeHorizontalSpacing;
			m_NodeLayout.NodeVerticalSpacing=m_NodeVerticalSpacing;
			
			// Layout specific properties
			if(m_Layout==eNodeLayout.Diagram)
			{
				Layout.NodeDiagramLayout nd=m_NodeLayout as Layout.NodeDiagramLayout;
				nd.DiagramFlow=m_DiagramLayoutFlow;
			}
            else if(m_Layout==eNodeLayout.Map)
			{
				NodeMapLayout nd=m_NodeLayout as NodeMapLayout;
				nd.MapFlow=m_MapLayoutFlow;
			}

            this.RecalcLayout();
			OnDisplayChanged();
		}
		private void ConnectorAppearanceChanged(object sender, EventArgs e)
		{
			OnDisplayChanged();
		}
		private void SetLayout(eNodeLayout layout)
		{
			m_Layout=layout;
			if(m_Layout==eNodeLayout.Map)
			{
				m_NodeLayout=new NodeMapLayout(this,this.ClientRectangle);
			}
			else if(m_Layout==eNodeLayout.Diagram)
			{
				m_NodeLayout=new Layout.NodeDiagramLayout(this,this.ClientRectangle);
			}
			InvalidateNodesSize();
			OnLayoutChanged();
		}
		
		private void PaintStyleBackground(Graphics g)
		{
			Display.NodeRenderer renderer = this.NodeRenderer;
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
			info.Style=m_BackgroundStyle;
			ElementStyleDisplay.Paint(info);
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			if(this.SuspendPaint)
				return;
			
			if(m_PendingLayout)
			{
				this.RecalcLayout();
			}

			if(this.BackColor.A<255)
			{
				base.OnPaintBackground(e);
			}
			
			Graphics g=e.Graphics;

			PaintStyleBackground(g);
			Point offset=Point.Empty;
			bool setOffset=false;
			if(this.AutoScroll)
			{
                offset = GetAutoScrollPositionOffset();
				setOffset=true;
			}
			
			if(m_AntiAlias)
			{
				g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			}
			
			PaintTree(g, e.ClipRectangle, offset, setOffset, m_ZoomFactor);
		}

        internal Rectangle GetInnerRectangle()
        {
            Rectangle r = ElementStyleLayout.GetInnerRect(this.BackgroundStyle, this.ClientRectangle);
            return r;
        }

        internal Point GetAutoScrollPositionOffset()
        {
            Point p = this.AutoScrollPosition;
            p.Y += this.SelectionBoxSize;
            p.X += this.SelectionBoxSize;
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
				g.DrawString("Thank you very much for trying TreeGX. Unfortunately your trial period is over. To continue using TreeGX you should purchase license at http://www.devcomponents.com",new Font(this.Font.FontFamily,12),SystemBrushes.Highlight,this.ClientRectangle,format);
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
				g.DrawString("TreeGX license not found. Please purchase license at http://www.devcomponents.com",
					trial, brushTrial, this.DisplayRectangle.X+this.DisplayRectangle.Width/2, this.DisplayRectangle.Bottom - 14, format);
				brushTrial.Dispose();
				format.Dispose();
				trial.Dispose();
			}
#endif
				
			//Creates the drawing matrix with the right zoom;
			if(zoomFactor!=1)
			{
				System.Drawing.Drawing2D.Matrix mx = GetTranslationMatrix(zoomFactor);
				//use it for drawing
				g.Transform=mx;
				
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
			g.DrawString("Thank you for trying TreeGX. Please purchase license at http://www.devcomponents.com",
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
			
			if(displayNode!=null)
			{
				if(this.NodeLayout is NodeMapLayout)
				{
					offset = new Point(Math.Abs(displayNode.ChildNodesBounds.Left),Math.Abs(displayNode.ChildNodesBounds.Top));
				}
			}
			
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
				Design.ComponentNotLicensed f = new Design.ComponentNotLicensed();
				f.ShowDialog();
				f.Dispose();
			}
			#endif
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if(this.Size.Width==0 || this.Size.Height==0)
				return;

			this.RecalcLayout();
		}
		
		protected override void OnKeyDown(KeyEventArgs e)
		{
			KeyNavigation.KeyDown(this, e);
			base.OnKeyDown (e);
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(!m_CellEditing && (keyData==Keys.Left || keyData==Keys.Right || keyData==Keys.Up || keyData==Keys.Down))
			{
				KeyNavigation.NavigateKeyDown(this, new KeyEventArgs(keyData));
				return true;
			}
			return base.ProcessCmdKey (ref msg, keyData);
		}



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

			Node node=this.GetNodeAt(mousePos.X,mousePos.Y);
			if(node!=null)
				OnNodeMouseDown(node,e,m_NodeDisplay.Offset);
			m_MouseDownLocation=mousePos;
			
#if !TRIAL
			if(NodeOperations.keyValidated2!=114 && !m_DialogDisplayed)
			{
				Design.ComponentNotLicensed f = new Design.ComponentNotLicensed();
				f.ShowDialog();
				f.Dispose();
				m_DialogDisplayed=true;
			}
#endif
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			OnNodeMouseUp(e);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);
            MouseEventArgs mouseArgs = e as MouseEventArgs;
            MouseButtons mb = MouseButtons.Left;
            if (mouseArgs != null)
                mb = mouseArgs.Button;
			OnNodeMouseClick(mb);
		}
		
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);
			if(this.SelectedNode!=null)
			{
				Point p=this.PointToClient(Control.MousePosition);
				InvokeNodeDoubleClick(new TreeGXNodeMouseEventArgs(this.SelectedNode, MouseButtons.Left, 2, 0, p.X, p.Y));
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
				InvokeNodeMouseHover(new TreeGXNodeMouseEventArgs(m_MouseOverNode, Control.MouseButtons, 0, 0, p.X, p.Y));
			}
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool bUpdate=false;
			
			Point mousePos=GetLayoutPosition(e);
			
			if(e.Button==MouseButtons.Left && !m_CellEditing && m_DragDropEnabled && m_MouseOverNode!=null && m_DragNode==null && m_MouseOverNode.DragDropEnabled &&
				(Math.Abs(m_MouseDownLocation.X-mousePos.X)>=SystemInformation.DragSize.Width || Math.Abs(m_MouseDownLocation.Y-mousePos.Y)>=SystemInformation.DragSize.Height))
			{
				StartDragDrop();
			}

			Rectangle r=Rectangle.Empty;
			if(m_MouseOverNode!=null)
				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,m_MouseOverNode,m_NodeDisplay.Offset);

			if(!r.IsEmpty && r.Contains(mousePos))
				bUpdate=OnNodeMouseMove(m_MouseOverNode,e,m_NodeDisplay.Offset);
			else
			{
				Node node=GetNodeAt(mousePos);
				if(node!=m_MouseOverNode)
					bUpdate=SetMouseOverNode(node);
				if(m_MouseOverNode!=null)
					bUpdate=bUpdate | OnNodeMouseMove(m_MouseOverNode,e,m_NodeDisplay.Offset);
			}

			if(bUpdate)
				this.Update();
		}

		/// <summary>
		/// Selected specified node.
		/// </summary>
		/// <param name="node">Node to select.</param>
		/// <param name="action">Action that is selecting the node.</param>
		public void SelectNode(Node node, eTreeAction action)
		{
            if (node != null && !node.Selectable && !this.DesignMode) return;

			TreeGXNodeCancelEventArgs cancelArgs = new TreeGXNodeCancelEventArgs(action, node);
			OnBeforeNodeSelect(cancelArgs);
			if(cancelArgs.Cancel)
				return;
			
			bool bUpdate=false;

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
			if(m_SelectedNode!=null)
			{
				InvalidateNode(m_SelectedNode);
				bUpdate=true;
				if(m_SelectedNode.SelectedCell!=null)
					m_SelectedNode.SelectedCell.SetSelected(false);
			}

			m_SelectedNode=node;

			if(m_SelectedNode!=null)
			{
				InvalidateNode(m_SelectedNode);
				bUpdate=true;
				if(m_SelectedNode.SelectedCell==null)
					m_SelectedNode.Cells[0].SetSelected(true);
			}
			
			if(this.SelectedPathConnector!=null)
				this.Invalidate();

			if(bUpdate)
				this.Update();
			
			TreeGXNodeEventArgs args = new TreeGXNodeEventArgs(action, node);
			OnAfterNodeSelect(args);
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

		private void InvalidateNodesSize()
		{
			foreach(Node node in m_Nodes)
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
            Node node = this.SelectedNode;
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
                //if (_MultiSelect)
                //{
                //    if (!this.SelectedNodes.Contains(nodeSelected))
                //        this.SelectedNode = nodeSelected;
                //}
                //else
                {
                    if (this.SelectedNode != nodeSelected)
                        this.SelectedNode = nodeSelected;
                }
            }
		}

        internal void OnNodesCleared()
        {
            this.SelectedNode = null;

            if (this.IsHandleCreated)
                this.RecalcLayout();
            else
                SetPendingLayout();
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
				m_MouseOverNode.MouseOverNodePart=eMouseOverNodePart.None;
				if(this.NodeStyleMouseOver!=null || this.CellStyleMouseOver!=null || m_MouseOverNode.StyleMouseOver!=null || this.NodeStyleMouseOver!=null ||
					m_RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.CommandButton || bUpdate)
				{
					InvalidateNode(m_MouseOverNode);
					bUpdate=true;
				}
				
				if(m_MouseOverNode!=mouseOverNode && m_MouseOverNode!=null)
				{
					Point p=this.PointToClient(Control.MousePosition);
					InvokeNodeMouseLeave(new TreeGXNodeMouseEventArgs(m_MouseOverNode, Control.MouseButtons, 0, 0, p.X, p.Y));
				}
			}
			
			m_MouseOverNode=mouseOverNode;
			
			if(m_MouseOverNode!=null)
			{
				m_MouseOverNode.MouseOverNodePart=eMouseOverNodePart.Node;
				if(m_MouseOverNode.StyleMouseOver!=null || this.NodeStyleMouseOver!=null ||
					m_RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.RenderMode!=eNodeRenderMode.Default || m_MouseOverNode.CommandButton)
				{
					InvalidateNode(m_MouseOverNode);
					bUpdate=true;
				}
				
				Point p=this.PointToClient(Control.MousePosition);
				InvokeNodeMouseEnter(new TreeGXNodeMouseEventArgs(m_MouseOverNode, Control.MouseButtons, 0, 0, p.X, p.Y));
				
				if(FireHoverEvent || m_MouseOverNode.FireHoverEvent)
					Interop.WinApi.ResetHover(this);
			}

			return bUpdate;
		}

		private void OnNodeMouseDown(Node node, MouseEventArgs e, Point offset)
		{
			Point mousePos = GetLayoutPosition(e);
			
			InvokeNodeMouseDown(new TreeGXNodeMouseEventArgs(node, e.Button, e.Clicks, e.Delta, mousePos.X, mousePos.Y));
			
			if(e.Button==MouseButtons.Left)
			{
				Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds,node,offset);
								
				if(r.Contains(mousePos))
				{
					m_CellMouseDownCounter=0;
					node.Toggle();
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

				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,node,offset);
				if(r.Contains(mousePos) && node.TreeControl!=null)
				{
					if(node.TreeControl.SelectedNode!=node)
						m_CellMouseDownCounter=0;
                    if (node.Selectable)
                    {
                        SelectNode(node, eTreeAction.Mouse);
                        if (node.TreeControl.SelectedNode != node) // Action cancelled
                            return;

                        Cell cell = GetCellAt(node, mousePos.X, mousePos.Y, offset);
                        if (cell != null)
                        {
                            bool checkBoxSelection = false;
                            if (cell.CheckBoxVisible)
                            {
                                Rectangle rCheckBox = cell.CheckBoxBoundsRelative;
                                rCheckBox.Offset(r.Location);
                                if (rCheckBox.Contains(mousePos))
                                {
                                    cell.Checked = !cell.Checked;
                                    checkBoxSelection = true;
                                    m_CellMouseDownCounter = 0;
                                }
                            }
                            if (!checkBoxSelection)
                                m_CellMouseDownCounter++;
                            node.SelectedCell = cell;
                            cell.SetMouseDown(true);
                        }
                    }
				}
				else
					m_CellMouseDownCounter=0;
			}
			else if(e.Button==MouseButtons.Right)
			{
				SelectNode(node, eTreeAction.Mouse);
				if(node.TreeControl.SelectedNode!=node) // Action cancelled
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
						node.ContextMenu.GetType().InvokeMember("Popup", System.Reflection.BindingFlags.InvokeMethod,
							null, node.ContextMenu, new object[]{p});
					}
					else if(node.ContextMenu.ToString().StartsWith(Design.NodeContextMenuTypeEditor.DotNetBarPrefix) && m_DotNetBarManager!=null)
					{
						string menuName=node.ContextMenu.ToString().Substring(Design.NodeContextMenuTypeEditor.DotNetBarPrefix.Length);
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

		private void OnNodeMouseUp(MouseEventArgs e)
		{
			bool bUpdate=false;
			Point mousePos = GetLayoutPosition(e);
			if(this.SelectedNode!=null)
			{
				if(this.SelectedNode.SelectedCell!=null && this.SelectedNode.SelectedCell.IsMouseDown)
				{
					this.SelectedNode.SelectedCell.SetMouseDown(false);
					this.InvalidateNode(this.SelectedNode);
					bUpdate=true;
				}
			}

			if(bUpdate)
				this.Update();
			
			if(NodeMouseUp!=null)
			{
				Node node = this.GetNodeAt(mousePos);
				if(node!=null)
				{
					InvokeNodeMouseUp(new TreeGXNodeMouseEventArgs(node, e.Button, e.Clicks, e.Delta, mousePos.X, mousePos.Y));
				}
			}
		}

		private void OnNodeMouseClick(MouseButtons mouseButton)
		{
			if(this.SelectedNode==null)
				return;
			
			InvokeNodeClick(new TreeGXNodeMouseEventArgs(this.SelectedNode, mouseButton, 1, 0, 0, 0));

			// Start editing if allowed
			if(this.CellEdit && this.SelectedNode.SelectedCell!=null && m_CellMouseDownCounter>1)
			{
				EditCell(this.SelectedNode.SelectedCell,eTreeAction.Mouse);
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
		
		/// <summary>
		/// Starts editing specified cell, places the cell into the edit mode.
		/// </summary>
		/// <param name="cell">Cell to start editing.</param>
		/// <param name="action">Action that is a cause for the edit.</param>
		/// <param name="initialText">Specifies the text to be edited instead of the text of the cell. Passing the NULL value will edit the text of the cell.</param>
		internal void EditCell(Cell cell, eTreeAction action, string initialText)
		{
			if(cell==null) return;
			
			if(m_CellEditing)
			{
				if(!EndCellEditing(action))
					return;
			}
			
			CellEditEventArgs e = new CellEditEventArgs(cell,action,"");
			OnBeforeCellEdit(e);
			if(e.Cancel)
				return;

			TextBoxEx textBox=GetTextBox();
			Rectangle rCell=NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds, cell,m_NodeDisplay.Offset);
			rCell = GetScreenRectangle(rCell);
			// It is important that text is assigned first or textbox will be resized to different size otherwise
			textBox.Text=cell.Text;
			textBox.EditWordWrap=cell.WordWrap;
			Font font =CellDisplay.GetCellFont(this,cell);
			if(m_ZoomFactor!=1 && font!=null)
			{
				font=new Font(font.FontFamily, font.SizeInPoints * m_ZoomFactor);
			}
			textBox.Font = font;
			textBox.Location=rCell.Location;
			textBox.Size=rCell.Size;
			textBox.Visible=true;
			textBox.Focus();
			
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
			
			cell.Parent.SetEditing(true);
			m_EditedCell=cell;
			m_CellEditing=true;
		}

		/// <summary>
		/// Ends cell editing.
		/// </summary>
		/// <param name="action">Specifies which action is cause for end of the editing.</param>
		/// <returns>Returns true if edits were applied to the cell or false otherwise.</returns>
		internal bool EndCellEditing(eTreeAction action)
		{
			if(m_EditedCell==null || m_EditTextBox==null)
			{
				m_CellEditing=false;
				return true;
			}

			TextBox textBox=m_EditTextBox;
			string text=textBox.Text;

			CellEditEventArgs e=new CellEditEventArgs(m_EditedCell,action,text);
			InvokeCellEditEnding(e);
			if(e.Cancel)
				return false;

			textBox.Visible=false;

			text=e.NewText;
			InvokeAfterCellEdit(e);
			text=e.NewText;

			if(!e.Cancel && m_EditedCell.Text!=text)
			{
				if(this.DesignMode && m_EditedCell.Parent.Cells[0]==m_EditedCell)
					TypeDescriptor.GetProperties(m_EditedCell.Parent)["Text"].SetValue(m_EditedCell.Parent, text);
				else
					TypeDescriptor.GetProperties(m_EditedCell)["Text"].SetValue(m_EditedCell, text);
				this.BeginUpdate();
				this.RecalcLayout();
				this.EndUpdate();
			}
			
			m_EditedCell.Parent.SetEditing(false);
			m_EditedCell=null;
			m_CellEditing=false;
			return true;
		}

		/// <summary>
		/// Cancels the cell editing if it is in progress.
		/// </summary>
		/// <param name="action">Specifies which action is cause for canceling of editing.</param>
		internal void CancelCellEdit(eTreeAction action)
		{
			if(m_EditedCell==null)
				return;

			m_EditTextBox.Text=m_EditedCell.Text;
			this.EndCellEditing(action);		
		}

		private TextBoxEx GetTextBox()
		{
			if(m_EditTextBox==null)
			{
				m_EditTextBox=new TextBoxEx();
				m_EditTextBox.AutoSize=false;
				this.Controls.Add(m_EditTextBox);
				m_EditTextBox.EndEdit+= new EventHandler(EditTextBoxEndEdit);
				m_EditTextBox.CancelEdit+=new EventHandler(EditTextBoxCancelEdit);
			}
			return m_EditTextBox;
		}

		private void EditTextBoxEndEdit(object sender, EventArgs e)
		{
			if(m_EditedCell==null)
				return;

			this.EndCellEditing(eTreeAction.Keyboard);	
		}

		private void EditTextBoxCancelEdit(object sender, EventArgs e)
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
				Rectangle rCell=cell.BoundsRelative;
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
			
			InvokeNodeMouseMove(new TreeGXNodeMouseEventArgs(node, e.Button, e.Clicks, e.Delta, mousePos.X, mousePos.Y));
			
			Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds,node,offset);
			bool bUpdate=false;
			if(r.Contains(mousePos))
			{
				node.MouseOverNodePart=eMouseOverNodePart.Expand;
				if(m_MouseOverCell!=null)
				{
					bUpdate|=SetMouseOverCell(null);
				}
				bUpdate=true;
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
			else
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
//			public DevComponents.Tree.Node Node=null;
//			public DevComponents.Tree.Cell Cell=null;
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

		private Color GetDefaultSelectionBoxBorderColor()
		{
			return Color.FromArgb(96,SystemColors.Highlight);
		}

		private Color GetDefaultSelectionBoxFillColor()
		{
			return Color.FromArgb(64,SystemColors.Highlight);
		}

		private void OnCommandButtonChanged()
		{
			m_NodeLayout.CommandAreaWidth=m_CommandWidth;
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
				TreeGXNodeCollectionEventArgs e=new TreeGXNodeCollectionEventArgs(action, node, parentNode);
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
				TreeGXNodeCollectionEventArgs e=new TreeGXNodeCollectionEventArgs(action, node, parentNode);
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
				TreeGXNodeCollectionEventArgs e=new TreeGXNodeCollectionEventArgs(action, node, parentNode);
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
				TreeGXNodeCollectionEventArgs e=new TreeGXNodeCollectionEventArgs(action, node, parentNode);
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

            //if (m_NodeDisplay != null) m_NodeDisplay.PaintedNodes.Clear();

            if (!this.IsDisposed)
                RecalcLayout();

            if (!this.IsUpdateSuspended)
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

                if (updateSelection)
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
		protected virtual void InvokeBeforeNodeDrop(TreeGXDragDropEventArgs e)
		{
			if(BeforeNodeDrop!=null)
				BeforeNodeDrop(this, e);
		}
		
		/// <summary>
		/// Invokes AfterNodeDrop event. If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeAfterNodeDrop(TreeGXDragDropEventArgs e)
		{
			if(AfterNodeDrop!=null)
				AfterNodeDrop(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseDown event. If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseDown(TreeGXNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseDown(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
			
			if(NodeMouseDown!=null)
				NodeMouseDown(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseUp event. If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseUp(TreeGXNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseUp(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
			
			if(NodeMouseUp!=null)
				NodeMouseUp(this, e);
		}
		
		/// <summary>
		/// Invokes NodeMouseMove event. If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeMouseMove(TreeGXNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeMouseMove(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
			
			if(NodeMouseMove!=null)
				NodeMouseMove(this, e);
		}
		
		/// <summary>
		/// Invokes NodeClick event. If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeClick(TreeGXNodeMouseEventArgs e)
		{
			if(e.Node!=null)
				e.Node.InvokeNodeClick(this, e);
			
			if(NodeClick!=null)
				NodeClick(this, e);
		}
		
		/// <summary>
		/// Invokes NodeDoubleClick event. If overriden base implementation must be called in order for event to fire.
		/// </summary>
		/// <param name="e">Provides information about event</param>
		protected virtual void InvokeNodeDoubleClick(TreeGXNodeMouseEventArgs e)
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
		protected virtual void InvokeNodeMouseEnter(TreeGXNodeMouseEventArgs e)
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
		protected virtual void InvokeNodeMouseLeave(TreeGXNodeMouseEventArgs e)
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
		protected virtual void InvokeNodeMouseHover(TreeGXNodeMouseEventArgs e)
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
			ValidateSelectedNode();
			if(!this.IsUpdateSuspended)
			{
				this.RecalcLayout();
				this.Refresh();
			}
		}

		#endregion

		#region Public Interface
		/// <summary>
		/// Save nodes to XmlDocument. New Node TreeGX is created and nodes are serialized into it.
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
		
		/// <summary>
		/// Sets the node map position when tree is in Map layout mode. The node's position
		/// can be set only for the sub-root nodes, i.e. nodes that are parented directly to
		/// top-level root node. Setting map position for any other node does not have any effect.
		/// </summary>
		/// <remarks>
		/// 	<para>Note that setting map position explicitly can change the position for other
		///     nodes that are on the same level as the node that you pass into this method. Since
		///     Map mode layouts the nodes clock-wise, setting the node position to Near will cause
		///     all nodes that are in collection <strong>after</strong> the reference node to be
		///     positioned Near as well.</para>
		/// 	<para>Similarly, setting the node position to Far will cause all nodes that are in
		///     collection <strong>before</strong> the reference node to be positioned Far as
		///     well.</para>
		/// </remarks>
		/// <param name="node">Sub-root node to set layout position for.</param>
		/// <param name="position">The position relative to the root node should take</param>
		public void SetNodeMapPosition(Node node, eMapPosition position)
		{
			if(node==null || node.Parent==null)
				return;
            
			if(position==eMapPosition.Default)
			{
				node.SetMapSubRootPosition(position);
			}
			else if(position==eMapPosition.Near)
			{
				int start=node.Parent.Nodes.IndexOf(node);
				int end=node.Parent.Nodes.Count;
				for(int i=start;i<end;i++)
					node.Parent.Nodes[i].SetMapSubRootPosition(position);
			}
			else if(position==eMapPosition.Far)
			{
				int start=node.Parent.Nodes.IndexOf(node);
				for(int i=start;i>=0;i--)
					node.Parent.Nodes[i].SetMapSubRootPosition(position);
			}
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
			return NodeOperations.GetNodeAt(this, x, y);
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
			Node node = GetNodeAt(x, y);
			if(node!=null)
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
		/// <a href="TreeGX~DevComponents.Tree.Cell~Cursor.html">Cell.Cursor</a> property.
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

		/// <summary>
		/// Gets reference to array of Cell objects that have HostedControl property set.
		/// </summary>
		internal ArrayList HostedControlCells
		{
			get { return m_HostedControlCells;}
		}
		
		/// <summary>
		/// This member overrides Control.WndProc.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void WndProc(ref Message m)
		{
			const int WM_HSCROLL        = 0x0114;
			const int WM_VSCROLL = 0x115;
			const int WM_MOUSEWHEEL = 0x020A;

			base.WndProc(ref m);
			
			if(this.AutoScroll)
			{
				if (m.Msg==WM_HSCROLL || m.Msg==WM_VSCROLL || m.Msg==WM_MOUSEWHEEL)
				{
					this.ActiveControl = null; // Must set to null otherwise scrolling will get reset if active control goes out of view
					RepositionHostedControls(false);
					this.Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Recalculates layout for the tree control. Not afffected by BeginUpdate call.
		/// </summary>
		internal void RecalcLayoutInternal()
		{
            Rectangle clientArea = this.ClientRectangle;
            clientArea.Inflate(-this.SelectionBoxSize, -this.SelectionBoxSize);
            m_NodeLayout.ClientArea = clientArea;
			m_NodeLayout.LeftRight=this.RtlTranslateLeftRight(LeftRightAlignment.Left);
			m_NodeLayout.PerformLayout();
			m_PendingLayout=false;
			
			float zoom = m_ZoomFactor;
			Rectangle screenRect=GetScreenRectangle(new Rectangle(0,0,m_NodeLayout.Width,m_NodeLayout.Height));
			Size nodeLayoutSize=screenRect.Size;
			if(nodeLayoutSize.Width>this.Bounds.Width || nodeLayoutSize.Height>this.Bounds.Height)
			{
				Size autoScrollMinSize =nodeLayoutSize;
                autoScrollMinSize.Width += this.SelectionBoxSize * 2;
                autoScrollMinSize.Height += this.SelectionBoxSize * 2;
				if(!this.AutoScroll)
				{
					this.BeginUpdate();
					this.Invalidate();
					this.AutoScroll=true;
					this.AutoScrollMinSize = autoScrollMinSize;
					this.AutoScrollPosition=m_NodeDisplay.DefaultOffset;
					this.EndUpdate(false);
				}
				else if(this.AutoScrollMinSize!=nodeLayoutSize)
				{
					this.BeginUpdate();
					this.AutoScrollMinSize = autoScrollMinSize;
					this.EndUpdate(false);
				}
			}
			else if(this.AutoScroll)
			{
				this.BeginUpdate();
				this.AutoScroll=false;
				m_NodeDisplay.Offset=m_NodeDisplay.DefaultOffset;
				this.EndUpdate(false);
			}
			else
				m_NodeDisplay.Offset=m_NodeDisplay.DefaultOffset;
			
			RepositionHostedControls(true);
			this.Invalidate(true);
		}
		
		private void RepositionHostedControls(bool performLayout)
		{
			if(m_HostedControlCells.Count>0)
			{
				this.SuspendLayout();
				if(this.AutoScroll)
                    m_NodeDisplay.Offset = GetAutoScrollPositionOffset();
				m_NodeDisplay.MoveHostedControls();
				this.ResumeLayout(performLayout);
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
		
		#endregion

		#region Event Invocation
		/// <summary>
		/// Calls <see cref="OnAfterCheck">OnAfterCheck</see> method which fired
		/// <see cref="AfterCheck">AfterCheck</see> event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		internal void InvokeAfterCheck(TreeGXCellEventArgs e)
		{
			OnAfterCheck(e);
		}

		/// <summary>Raises the <see cref="AfterCheck">AfterCheck</see> event.</summary>
		/// <param name="e">
		/// A <see cref="TreeGXCellEventArgs">TreeGXEventArgs</see> that contains the event
		/// data.
		/// </param>
		protected virtual void OnAfterCheck(TreeGXCellEventArgs e)
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
		
		void INodeNotify.OnBeforeCollapse(TreeGXNodeCancelEventArgs e)
		{
			if(BeforeCollapse!=null)
				BeforeCollapse(this, e);
		}
		
		void INodeNotify.OnAfterCollapse(TreeGXNodeEventArgs e)
		{
			if(AfterCollapse!=null)
				AfterCollapse(this, e);
		}
		
		void INodeNotify.OnBeforeExpand(TreeGXNodeCancelEventArgs e)
		{
			if(BeforeExpand!=null)
				BeforeExpand(this, e);
		}
		
		void INodeNotify.OnAfterExpand(TreeGXNodeEventArgs e)
		{
			if(AfterExpand!=null)
				AfterExpand(this, e);
		}
		
		protected virtual void OnAfterNodeSelect(TreeGXNodeEventArgs args)
		{
			if(AfterNodeSelect!=null)
				AfterNodeSelect(this, args);
		}

		protected virtual void OnBeforeNodeSelect(TreeGXNodeCancelEventArgs args)
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

		internal void InternalDragDrop(DragEventArgs drgevent)
		{
			ReleaseDragNode(true);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave (e);
			InternalDragLeave();
		}

		internal void InternalDragLeave()
		{
			ReleaseDragNode(false);
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver (drgevent);
			InternalDragOver(drgevent);
		}

		/// <summary>
		/// Processes drag over event.
		/// </summary>
		/// <param name="drgevent">Drag event arguments.</param>
		internal void InternalDragOver(DragEventArgs drgevent)
		{
			if(!m_DragDropEnabled)
				return;

			if(m_DragNode==null)
				CreateDragNode(drgevent);

			if(m_DragNode==null)
			{
				drgevent.Effect=DragDropEffects.None;
				return;
			}

			drgevent.Effect=DragDropEffects.Move;
			InvalidateNode(m_DragNode);
			Point p=this.PointToClient(new Point(drgevent.X,drgevent.Y));
			p=GetLayoutPosition(p);
			TreeAreaInfo areaInfo = NodeOperations.GetTreeAreaInfo(this, p.X, p.Y);
			Point insideNodeOffset=Point.Empty;
			Node offsetNode=null;
			bool recalcLayout=false;

            //if (areaInfo == null)
            //    Console.WriteLine("{0} Area info null   Point:{1}", DateTime.Now, p);
            //else if (areaInfo.NodeAt != null)
            //    Console.WriteLine("{0} Area found node at: {1}   Point:{2}", DateTime.Now, areaInfo.NodeAt, p);
            //else
            //    Console.WriteLine("{0} Area found NO NODE AT: {1}    Point:{2}", DateTime.Now, areaInfo.NodeAt, p);

			if(areaInfo!=null && areaInfo.PreviousNode!=m_DragNode && areaInfo.NextNode!=m_DragNode &&
			   (areaInfo.NodeAt!=m_DragNode || m_DragNode.Parent==null))
			{
				if(areaInfo.NodeAt!=null && areaInfo.NodeAt!=m_DragNode.Tag && areaInfo.NodeAt!=m_DragNode)
				{
					if(areaInfo.NodeAt!=m_DragNode.Parent)
					{
						Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,areaInfo.NodeAt,m_NodeDisplay.Offset);
						insideNodeOffset=new Point(p.X-r.X,p.Y-r.Y);

						recalcLayout=true;
						m_DragNode.Remove(eTreeAction.Mouse);
						areaInfo.NodeAt.Nodes.Add(m_DragNode, eTreeAction.Mouse);

						offsetNode = areaInfo.NodeAt;
					}
				}
				else if(areaInfo.PreviousNode!=null && areaInfo.NextNode!=null)
				{
					if(m_DragNode.Parent==null || !(m_DragNode.Parent==areaInfo.ParentAreaNode && areaInfo.ParentAreaNode.Nodes.IndexOf(areaInfo.NextNode)==areaInfo.ParentAreaNode.Nodes.IndexOf(m_DragNode)))
					{						
						Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,m_DragNode,m_NodeDisplay.Offset);
						
						recalcLayout=true;
						m_DragNode.Remove(eTreeAction.Mouse);
						areaInfo.ParentAreaNode.Nodes.Insert(areaInfo.ParentAreaNode.Nodes.IndexOf(areaInfo.NextNode), m_DragNode, eTreeAction.Mouse);

						insideNodeOffset=new Point(m_DragNode.BoundsRelative.Width/2,m_DragNode.BoundsRelative.Height/2);
						offsetNode=m_DragNode;
					}
				}
				else
				{
					if(m_DragNode.Parent!=null)
					{
						//System.Diagnostics.Trace.WriteLine("Floating");
						Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,m_DragNode.Parent,m_NodeDisplay.Offset);
						insideNodeOffset=new Point(p.X-r.X,p.Y-r.Y);
						offsetNode=m_DragNode.Parent;
						recalcLayout=true;
						m_DragNode.Remove(eTreeAction.Mouse);
						m_DragNode.internalTreeControl=this;
					}
					if(m_NodeDisplay.LockOffset)
					{
						m_NodeDisplay.LockOffset=false;
						recalcLayout=true;
					}
					m_DragNode.SetBounds(new Rectangle(p.X,p.Y,m_DragNode.BoundsRelative.Width,m_DragNode.BoundsRelative.Height));
				}
				m_DragNode.Visible=true;
				if(recalcLayout)
				{
					this.RecalcLayout();
					if(!insideNodeOffset.IsEmpty)
					{
						if(offsetNode!=null)
						{
							offsetNode.EnsureVisible();
							this.Refresh();

							Rectangle r=GetScreenRectangle(NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,offsetNode,m_NodeDisplay.Offset));
							Point screenPos=r.Location;
							screenPos.Offset((int)(insideNodeOffset.X*m_ZoomFactor), (int)(insideNodeOffset.Y*m_ZoomFactor));
							screenPos=this.PointToScreen(screenPos);
							SetCursorPos(screenPos.X,screenPos.Y);
						}
					}
					this.Refresh();
				}
				else
				{
					InvalidateNode(m_DragNode);
					if(this.DesignMode)
						this.Refresh();
					else
						this.Update();
				}
			}
		}

		/// <summary>
		/// Gets or sets whether drag and drop operation is in progress. This member supports
		/// the TreeGX infrastructure and is not intended to be used directly from your
		/// code.
		/// </summary>
		[Browsable(false)]
		public bool IsDragDropInProgress
		{
			get {return (m_DragNode!=null);}
		}

		private void ReleaseDragNode(bool drop)
		{
			if(m_DragNode==null)
				return;
			this.BeginUpdate();
			try
			{
				if(m_NodeDisplay.LockOffset)
					m_NodeDisplay.LockOffset=false;
				Node node=m_DragNode.Tag as Node;
				node.Visible = true;
				if(m_DragNode.Parent!=null)
				{
					int index=m_DragNode.Parent.Nodes.IndexOf(m_DragNode);
					if(drop)
					{
						Node parent=m_DragNode.Parent;
						m_DragNode.Remove(eTreeAction.Mouse);
						
						// Fire off events and cancel processing if needed
						TreeGXDragDropEventArgs e=new TreeGXDragDropEventArgs(eTreeAction.Mouse, node, node.Parent,  parent);
						InvokeBeforeNodeDrop(e);
						if(!e.Cancel)
						{
                            bool isCopy = e.IsCopy;
							bool recursive=false;
                            Node test = parent;
                            if (!isCopy && test != null)
                            {
                                do
                                {
                                    if (test.Parent == node)
                                    {
                                        recursive = true;
                                        break;
                                    }
                                    test = test.Parent;
                                } while (test != null && test.Parent != null);
                            }

							if(recursive)
							{
								Node rp=node.Parent;
								int rpi=0;
								if(node.Parent!=null)
									rpi=node.Parent.Nodes.IndexOf(node);
								else
									rpi=this.Nodes.IndexOf(node);

								Node[] na=new Node[node.Nodes.Count];
								node.Nodes.CopyTo(na);
								foreach(Node childNode in na)
								{
									if(rp==null)
									{
										childNode.Remove(eTreeAction.Mouse);
										this.Nodes.Insert(rpi,childNode, eTreeAction.Mouse);
										break;
									}
									childNode.Remove(eTreeAction.Mouse);
									rp.Nodes.Insert(rpi,childNode, eTreeAction.Mouse);
									rpi++;
								}
							}
                            if (isCopy)
                            {
                                node = node.DeepCopy();
                            }
                            else
    							node.Remove(eTreeAction.Mouse);
							node.SizeChanged = true;
							if(parent!=null && index>parent.Nodes.Count)
								index=parent.Nodes.Count;
							if(parent!=null)
								parent.Nodes.Insert(index,node, eTreeAction.Mouse);
							else
								this.Nodes.Insert(index,node, eTreeAction.Mouse);
							node.EnsureVisible();
							
							InvokeAfterNodeDrop(e);
						}
					}
					else
						m_DragNode.Remove(eTreeAction.Mouse);
				}
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

		private void CreateDragNode(DragEventArgs e)
		{
            if (e.Data != null)
            {
                Node node = null;
                if (e.Data.GetDataPresent(typeof(Node)))
                    node = e.Data.GetData(typeof(Node)) as Node;
                else if (e.Data.GetFormats().Length > 0)
                    node = e.Data.GetData(e.Data.GetFormats()[0]) as Node;
                if (node != null)
                    CreateDragNode(node);
            }
		}

		private void CreateDragNode(Node node)
		{
			m_DragNode=node.Copy();
			m_DragNode.Tag=node;
			PrepareDragNodeElementStyle(m_DragNode, node);
			m_DragNode.Visible=false;
			m_NodeLayout.PerformSingleNodeLayout(m_DragNode);
			node.Visible = false;
			this.RecalcLayout();
			this.Refresh();
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

		private bool StartDragDrop()
		{
			if(m_MouseOverNode==null)
				return false;
			
			return StartDragDrop(m_MouseOverNode);
		}

		internal bool StartDragDrop(Node node)
		{
			if(IsDragDropInProgress)
				return false;
			
			this.EndCellEditing(eTreeAction.Mouse);

			if(node.IsMouseDown)
				OnNodeMouseUp(new MouseEventArgs(Control.MouseButtons,0,0,0,0));

			if(node == m_DisplayRootNode /*|| node.Parent==null*/) // Drag & drop in designer means that parent is null
				return false;

			Point m_MouseDownPoint=Control.MousePosition;

			if(this.DesignMode)
				CreateDragNode(node);
			else
				this.DoDragDrop(node,DragDropEffects.All);

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
		internal Node GetDragNode()
		{
			return m_DragNode;
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
	}
}
