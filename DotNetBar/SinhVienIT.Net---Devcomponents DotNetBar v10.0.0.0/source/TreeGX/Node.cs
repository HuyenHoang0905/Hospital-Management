using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.Tree.Display;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents the Node in Tree control.
	/// </summary>
	[DesignTimeVisible(false),ToolboxItem(false),Designer(typeof(Design.NodeDesigner))]
	public class Node:Component
	{
		#region Events
		/// <summary>
		/// Occurs when the mouse pointer is over the node and a mouse button is pressed.
		/// </summary>
		public event MouseEventHandler NodeMouseDown;
		
		/// <summary>
		/// Occurs when the mouse pointer is over the node and a mouse button is released.
		/// </summary>
		public event MouseEventHandler NodeMouseUp;
		
		/// <summary>
		/// Occurs when the mouse pointer is moved over the node.
		/// </summary>
		public event MouseEventHandler NodeMouseMove;
		
		/// <summary>
		/// Occurs when the mouse enters the node.
		/// </summary>
		public event EventHandler NodeMouseEnter;
		
		/// <summary>
		/// Occurs when the mouse leaves the node.
		/// </summary>
		public event EventHandler NodeMouseLeave;
		
		/// <summary>
		/// Occurs when the mouse hovers over the node.
		/// </summary>
		public event EventHandler NodeMouseHover;
		
		/// <summary>
		/// Occurs when the node is clicked with left mouse button. If you need to know more information like if another mouse button is clicked etc. use
		/// NodeMouseDown event.
		/// </summary>
		public event EventHandler NodeClick;
		
		/// <summary>
		/// Occurs when the node is double-clicked.
		/// </summary>
		public event EventHandler NodeDoubleClick;
		
		/// <summary>
		/// Occurs when hyperlink in text-markup is clicked.
		/// </summary>
		public event MarkupLinkClickEventHandler MarkupLinkClick;
		#endregion
		
		#region Private Variables
		private Rectangle m_BoundsRelative=Rectangle.Empty;
		private Rectangle m_ChildNodesBounds=Rectangle.Empty;
		private Rectangle m_CellsBounds=Rectangle.Empty;
		private Rectangle m_ContentBounds=Rectangle.Empty;
		private Rectangle m_CommandBoundsRelative=Rectangle.Empty;
		private NodeCollection m_Nodes=null;
		private LinkedNodesCollection m_LinkedNodes=null;
		private CellCollection m_Cells=new CellCollection();
		//private string m_Header="";
		private ColumnHeaderCollection m_NodesColumns;
		//private bool m_NodesColumnHeaderVisible=true;
		private object m_DataKey=null;
		private string m_Text="";
		private bool m_Expanded=false;
		private eNodeExpandVisibility m_ExpandVisibility=eNodeExpandVisibility.Auto;
		private Rectangle m_ExpandPartRectangle=Rectangle.Empty;
		private Node m_Parent=null;
		
		private ElementStyle m_StyleExpanded=null;
		private ElementStyle m_StyleSelected=null;
		private ElementStyle m_StyleMouseOver=null;
		private bool m_SizeChanged=true;
		//private eNodeHeaderVisibility m_HeaderVisibility=eNodeHeaderVisibility.Automatic;
		private int m_ColumnHeaderHeight=0;
		private eMapPosition m_MapSubRootPosition=eMapPosition.Default;
		private int m_Offset=0;
		internal TreeGX internalTreeControl=null;
		private ElementStyle m_Style=null;
		private eMouseOverNodePart m_MouseOverNodePart=eMouseOverNodePart.None;
		private NodeConnector m_ParentConnector=null;
		private eCellLayout m_CellLayout=eCellLayout.Default;
		private bool m_Visible=true;
		private bool m_CommandButton=false;
		private bool m_Editing=false;
		private bool m_DragDropEnabled=true;
		private string m_Name="";
		private ConnectorPointsCollection m_ParentConnectorPoints=null;
		private bool m_SelectedConnectorMarker=false;
		private eNodeRenderMode m_RenderMode=eNodeRenderMode.Default;
		private NodeRenderer m_NodeRenderer=null;
		private object m_ContextMenu=null;
		#endregion

		/// <summary>Default Constructor.</summary>
		public Node()
		{
			m_NodesColumns=new ColumnHeaderCollection();
			m_Cells.SetParentNode(this);
			m_ParentConnectorPoints=new ConnectorPointsCollection();
			m_ParentConnectorPoints.SetParentNode(this);

			Cell defaultCell=new Cell();
			this.Cells.Add(defaultCell);
		}

		#region Properties
		/// <summary>
		/// Gets or sets the context menu assigned to this node. Standard Context Menus, VS.NET 2005 Context Menus and DotNetBar Suite context menus are supported.
		/// Default value is null (Nothing) which indicates that no context menu is assigned.
		/// </summary>
		[Browsable(true), DefaultValue(null), Description("Indicates the context menu assigned to this node."), Editor(typeof(Design.NodeContextMenuTypeEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public object ContextMenu
		{
			get { return m_ContextMenu; }
			set { m_ContextMenu=value; }
		}
		
		/// <summary>
		/// Gets whether any of the cells inside the node has HostedControl property set.
		/// </summary>
		[Browsable(false)]
		public bool HasHostedControls
		{
			get
			{
				foreach(Cell cell in m_Cells)
				{
					if(cell.HostedControl!=null)
						return true;
				}
				return false;
			}
		}
		/// <summary>
		/// Gets or sets custom node renderer. You can set this property to your custom renderer. When set the RenderMode should be set to custom to enable
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
		/// Gets or sets the render mode used to render the node. Default value is eNodeRenderMode.Default which indicates that system default renderer is used.
		/// Note that if you specify custom renderer you need to set either TreeGX.NodeRenderer or Node.NodeRenderer property.
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
		/// Gets or sets whether node is expanded. Expanded node shows it's child nodes.
		/// </summary>
		[Browsable(true),Category("Node State"),DefaultValue(false),Description("Indicates whether node is expanded."),DevCoSerialize()]
		public bool Expanded
		{
			get {return m_Expanded;}
			set
			{
				if(value)
					this.Expand();
				else
					this.Collapse();
			}
		}
		/// <summary>
		/// Returns name of the node that can be used to identify it from the code.
		/// </summary>
		[Browsable(false),Category("Design"),Description("Indicates the name used to identify node."), DevCoSerialize()]
		public string Name
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
		/// Gets or sets whether node can be dragged and dropped. Default value is true.
		/// </summary>
		[Browsable(true),Category("Behavior"),Description("Indicates whether node can be dragged and dropped."),DefaultValue(true),DevCoSerialize()]
		public bool DragDropEnabled
		{
			get {return m_DragDropEnabled;}
			set {m_DragDropEnabled=value;}
		}

		/// <summary>
		/// Gets or sets visibility of the expand button. Default value is <strong>Auto</strong> meaning that
		/// expand button is displayed only if node has at least one child node.
		/// </summary>
		/// <remarks>
		/// You can use this property for example to dynamically load the child nodes when user
		/// tries to expand the node. You could for example handle BeforeExpand event to load child
		/// nodes into the node.
		/// </remarks>
		[Category("Appearance"),Browsable(true),DefaultValue(eNodeExpandVisibility.Auto),Description("Indicates whether the expand button is always visible regardless of whether node contains child nodes or not."),DevCoSerialize()]
		public eNodeExpandVisibility ExpandVisibility
		{
			get {return m_ExpandVisibility;}
			set
			{
				m_ExpandVisibility=value;
				this.SizeChanged = true;
			}
		}

		/// <summary>
		/// Gets or sets whether any operation on the node has been performed that would affect node's size. Size changed flag 
		/// internally indicates that node's size needs to be recalculated becouse it has changed
		/// due to the changes in data.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SizeChanged
		{
			get {return m_SizeChanged;}
			set
			{
				m_SizeChanged=value;
				OnSizeChanged();
			}
		}
		
		/// <summary>
		/// Gets the relative bounds of the tree node including the expand part of the node.
		/// </summary>
		[Browsable(false)]
		internal Rectangle BoundsRelative
		{
			get
			{
				return m_BoundsRelative;
			}
		}
		
		/// <summary>
		/// Gets the absolute bounds of the tree node including the expand part of the node.
		/// </summary>
		[Browsable(false)]
		public Rectangle Bounds
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,this,tree.NodeDisplay.Offset);
				}
				return Rectangle.Empty;
			}
		}

		/// <summary>
		/// Sets the bounds of the node.
		/// </summary>
		/// <param name="r">New location and size of the node.</param>
		internal void SetBounds(Rectangle r)
		{
			m_BoundsRelative=r;
		}

		/// <summary>
		/// Sets the content bounds of the node. Content bound is bound for the the cells inside the node
		/// and it excludes the expand rectangle. Bounds also include the node style padding and
		/// reflect node margin.
		/// </summary>
		/// <param name="r">New location and size of the node.</param>
		internal void SetContentBounds(Rectangle r)
		{
			m_ContentBounds=r;
		}

		/// <summary>
		/// Gets the node content bounds.
		/// </summary>
		internal Rectangle ContentBounds
		{
			get {return m_ContentBounds;}
		}

		/// <summary>
		/// Gets the bounds for all the cells inside the node. The bounds do not include the expand part.
		/// </summary>
		[Browsable(false)]
		internal Rectangle CellsBoundsRelative
		{
			get {return m_CellsBounds;}
		}
		
		/// <summary>
		/// Gets the bounds for all the cells inside the node. The bounds do not include the expand part.
		/// </summary>
		[Browsable(false)]
		public Rectangle CellsBounds
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CellsBounds,this,tree.NodeDisplay.Offset);
				}
				return Rectangle.Empty;
			}
		}

		/// <summary>
		/// Sets cell bounds.
		/// </summary>
		/// <param name="r">New cells bounds.</param>
		internal void SetCellsBounds(Rectangle r)
		{
			m_CellsBounds=r;
		}

		/// <summary>
		/// Sets node parent.
		/// </summary>
		/// <param name="parent">Parent node object.</param>
		internal void SetParent(Node parent)
		{
			m_Parent=parent;
			if(m_Parent==null)
			{
				foreach(Cell cell in m_Cells)
				{
					if(cell.HostedControl!=null && cell.HostedControl.Parent is TreeGX)
						cell.HostedControl.Parent = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the bounds of child nodes.
		/// </summary>
		internal Rectangle ChildNodesBounds
		{
			get {return m_ChildNodesBounds;}
			set {m_ChildNodesBounds=value;}
		}

		/// <summary>
		/// Gets the expand part rectangle. Expand part is used to expand/collapse node.
		/// </summary>
		[Browsable(false)]
		internal Rectangle ExpandPartRectangleRelative
		{
			get {return m_ExpandPartRectangle;}
		}
		
		/// <summary>
		/// Gets the expand part rectangle. Expand part is used to expand/collapse node.
		/// </summary>
		[Browsable(false)]
		public Rectangle ExpandPartRectangle
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds,this,tree.NodeDisplay.Offset);
				}
				return Rectangle.Empty;
			}
		}

		/// <summary>
		/// Sets the bounds of the expand part.
		/// </summary>
		/// <param name="r">New part bounds.</param>
		internal void SetExpandPartRectangle(Rectangle r)
		{
			m_ExpandPartRectangle=r;
		}
		
		/// <summary>
		/// Gets or sets the Command part bounds if command part is visible.
		/// </summary>
		[Browsable(false)]
		public Rectangle CommandBounds
		{
			get
			{
				if(!m_CommandBoundsRelative.IsEmpty)
				{
					TreeGX tree=this.TreeControl;
					if(tree!=null)
					{
						return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CommandBounds,this,tree.NodeDisplay.Offset);
					}
				}
				return Rectangle.Empty;
			}
		}

		internal Rectangle CommandBoundsRelative
		{
			get {return m_CommandBoundsRelative;}
			set { m_CommandBoundsRelative = value;}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the tree node is in a checked state.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Appearance"),Description("Indicates whether the tree node is in a checked state."),DevCoSerialize()]
		public bool Checked
		{
			get
			{
				return m_Cells[0].Checked;
			}
			set
			{
				m_Cells[0].Checked=value;
			}
		}

		/// <summary>
		/// Gets or sets the checkbox alignment in relation to the text displayed by first default cell.
		/// </summary>
		[Browsable(true),Category("Check-box Properties"),DefaultValue(eCellPartAlignment.NearCenter),Description("Indicates checkbox alignment in relation to the text displayed by cell."), DevCoSerialize()]
		public eCellPartAlignment CheckBoxAlignment
		{
			get
			{
				return m_Cells[0].CheckBoxAlignment;
			}
			set
			{
				m_Cells[0].CheckBoxAlignment=value;
			}
		}

		/// <summary>
		/// Gets or sets whether check box is visible inside the cell.
		/// </summary>
		[Browsable(true),Category("Check-box Properties"),DefaultValue(false),Description("Indicates whether check box is visible inside the cell."),DevCoSerialize()]
		public bool CheckBoxVisible
		{
			get
			{
				return m_Cells[0].CheckBoxVisible;
			}
			set
			{
				m_Cells[0].CheckBoxVisible=value;
			}
		}

		/// <summary>
		/// Gets the path from the root tree node to the current tree node. The path consists of the labels of all the tree nodes that must be navigated to get to this tree node, starting at the root tree node. The node labels are separated by the delimiter character specified in the PathSeparator property of the Tree control that contains this node.
		/// </summary>
		[Browsable(false)]
		public string FullPath
		{
			get
			{
				if(this.TreeControl==null)
					return "";
				return NodeOperations.GetFullPath(this,this.TreeControl.PathSeparator);
			}
		}

		/// <summary>
		/// Gets the zero based index of position of the tree node in the tree node collection. -1 is returned if node is not added to the nodes collection.
		/// </summary>
		[Browsable(false)]
		public int Index
		{
			get
			{
				return NodeOperations.GetNodeIndex(this);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the tree node is in an editable state. true if the tree node is in editable state; otherwise, false.
		/// </summary>
		[Browsable(false)]
		public bool IsEditing
		{
			get { return m_Editing;}
		}
		/// <summary>
		/// Sets whether node is in edit mode or not.
		/// </summary>
		/// <param name="b">True indicating that node is in edit mode false otherwise.</param>
		internal void SetEditing(bool b)
		{
			m_Editing=b;	
		}

		/// <summary>
		/// Gets whether left mouse button is pressed on any cell contained by this node.
		/// </summary>
		[Browsable(false)]
		public bool IsMouseDown
		{
			get
			{
				foreach(Cell cell in this.Cells)
				{
					if(cell.IsMouseDown)
						return true;
				}
				return false;
			}
		}
		
		/// <summary>
		/// Gets whether mouse cursor is over on any cell contained by this node.
		/// </summary>
		[Browsable(false)]
		public bool IsMouseOver
		{
			get
			{
				foreach(Cell cell in this.Cells)
				{
					if(cell.IsMouseOver)
						return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the tree node is in the selected state. true if the tree node is in the selected state; otherwise, false.
		/// </summary>
		[Browsable(false)]
		public bool IsSelected
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					if(tree.SelectedNode==this)
						return true;
				}
				return false;
			}
		}

        internal void OnNodesCleared()
        {
            this.SizeChanged = true;
        }

        private bool _Enabled = true;
        /// <summary>
        /// Gets or sets whether node is enabled. Default value is true. Setting this value to false will set Enabled=false on all child cells.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Gets or sets whether node is enabled."), DevCoSerialize()]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                foreach (Cell c in m_Cells)
                {
                    c.Enabled = _Enabled;
                }
            }
        }

        private bool _Selectable = true;
        /// <summary>
        /// Gets or sets whether node can be selected by user by clicking it with the mouse or using keyboard. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether node can be selected by user by clicking it with the mouse or using keyboard.")]
        public bool Selectable
        {
            get { return _Selectable; }
            set
            {
                _Selectable = value;
            }
        }

        /// <summary>
        /// Gets whether node can be selected. Node must be Visible, Enabled and Selectable in order for it to be selected.
        /// </summary>
        [Browsable(false)]
        public bool CanSelect
        {
            get
            {
                return this.Enabled & this.Visible & this.Selectable;
            }
        }

		/// <summary>
		/// Gets or sets a cell that is in selected state otherwise it retuns null.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Cell SelectedCell
		{
			get
			{
				if(this.IsSelected)
				{
					foreach(Cell cell in m_Cells)
						if(cell.IsSelected)
							return cell;
				}
				return null;
			}
			set
			{
				TreeGX tree=this.TreeControl;

				if(!this.IsSelected)
				{
					if(tree!=null)
						tree.SelectedNode=this;
				}
				foreach(Cell cell in m_Cells)
				{
					if(cell==value)
						cell.SetSelected(true);
					else
						cell.SetSelected(false);
				}
				if(tree!=null)
				{
					tree.InvalidateNode(this);
					tree.Update();
				}

			}
		}

		/// <summary>
		/// Gets a value indicating whether the tree node is visible. Node is considered to be visible when it's Visible property is set to true and path to the node is available i.e. all parent nodes are expanded.
		/// </summary>
		[Browsable(false)]
		public bool IsVisible
		{
			get
			{
				return NodeOperations.GetIsNodeVisible(this);
			}
		}

		/// <summary>
		/// Returns whether node is displayed on the screen and visible to the user. When node is outside of the viewable area this property will return false. It will also return false if node is not visible.
		/// </summary>
		[Browsable(false)]
		public bool IsDisplayed
		{
			get {return NodeOperations.GetIsNodeDisplayed(this);}	
		}

		/// <summary>
		/// Gets the last child tree node. The LastNode is the last child Node in the NodeCollection stored in the Nodes property of the current tree node. If the Node has no child tree node, the LastNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		[Browsable(false)]
		public Node LastNode
		{
			get
			{
				return NodeOperations.GetLastNode(this);
			}
		}

		/// <summary>
		/// Gets the next sibling tree node. The NextNode is the next sibling Node in the NodeCollection stored in the Nodes property of the tree node's parent Node. If there is no next tree node, the NextNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		[Browsable(false)]
		public Node NextNode
		{
			get
			{
				return NodeOperations.GetNextNode(this);
			}
		}

		/// <summary>
		/// Gets the next visible tree node. The NextVisibleNode can be a child, sibling, or a tree node from another branch. If there is no next tree node, the NextVisibleNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		[Browsable(false)]
		public Node NextVisibleNode
		{
			get
			{
				return NodeOperations.GetNextVisibleNode(this);
			}
		}

		/// <summary>
		/// Gets the collection of Node objects assigned to the current tree node. The Nodes property can hold a collection of other Node objects. Each of the tree node in the collection has a Nodes property that can contain its own NodeCollection. Nesting of tree nodes can make it difficult to navigate a tree structure. The FullPath property makes it easier to determine your location in a tree.
		/// </summary>
		[Browsable(true),Category("Nodes"),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public NodeCollection Nodes
		{
			get
			{
				if(m_Nodes==null)
				{
					m_Nodes=new NodeCollection();
					m_Nodes.SetParentNode(this);
				}
				return m_Nodes;
			}
		}

		/// <summary>
		/// Gets the collection of LinkedNode objects that describe nodes linked to this node.
		/// </summary>
		/// <remarks>
		/// Linked nodes are nodes that are related to given node but do not have strict
		/// parent child relationship with the node. Each linked node must be already added as
		/// child node to some other node or it will not be displayed. Linked nodes are used in Map
		/// and Diagram layout styles to display relationships between nodes.
		/// </remarks>
		[Browsable(true),Category("Nodes"), Description("Collection of LinkedNode objects that describe nodes linked to this node")]
		public LinkedNodesCollection LinkedNodes
		{
			get
			{
				if(m_LinkedNodes==null)
				{
					m_LinkedNodes=new LinkedNodesCollection();	
				}
				return m_LinkedNodes;
			}
		}

		/// <summary>
		/// Gets whether Node has any linked nodes.
		/// </summary>
		[Browsable(false)]
		public bool HasLinkedNodes
		{
            get {return (m_LinkedNodes!=null && m_LinkedNodes.Count>0);}
		}

		/// <summary>
		/// Gets whether there is at least one child node that has its Visible property set to true.
		/// </summary>
		[Browsable(false)]
		public bool AnyVisibleNodes
		{
			get
			{
				return NodeOperations.GetAnyVisibleNodes(this);
			}
		}

		/// <summary>
		/// Gets the parent tree node of the current tree node. If the tree node is at the root level, the Parent property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		[Browsable(false)]
		public Node Parent
		{
			get
			{
				return m_Parent;
			}
		}

		/// <summary>
		/// Gets the previous sibling tree node. The PrevNode is the previous sibling Node in the NodeCollection stored in the Nodes property of the tree node's parent Node. If there is no previous tree node, the PrevNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		[Browsable(false)]
		public Node PrevNode
		{
			get
			{
				return NodeOperations.GetPreviousNode(this);
			}
		}

		/// <summary>
		/// Gets the previous visible tree node. The PrevVisibleNode can be a child, sibling, or a tree node from another branch. If there is no previous tree node, the PrevVisibleNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		[Browsable(false)]
		public Node PrevVisibleNode
		{
			get
			{
				return NodeOperations.GetPreviousVisibleNode(this);
			}
		}

		/// <summary>
		/// Gets or sets the object that contains data about the tree node. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
		/// </summary>
		[Browsable(false),DefaultValue(null),Category("Data"),Description("Indicates text that contains data about the tree node."),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Tag
		{
			get
			{
				return m_Cells[0].Tag;
			}
			set
			{
				m_Cells[0].Tag = value;
			}
		}

		/// <summary>
		/// Gets or sets the object that contains data about the tree node. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Data"),Description("Indicates text that contains data about the tree node."), DevCoSerialize()]
		public string TagString
		{
			get
			{
				return m_Cells[0].TagString;
			}
			set
			{
				m_Cells[0].TagString=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the object that contains additional data about the tree node. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
		/// This property has same function as Tag property and provides you with additional separate storage of data.
		/// </summary>
		[Browsable(false),DefaultValue(null),Category("Data"),Description("Indicates text that contains data about the tree node."),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object DataKey
		{
			get
			{
				return m_DataKey;
			}
			set
			{
				m_DataKey=value;
			}
		}

		/// <summary>
		/// Gets or sets the object that contains additional data about the tree node. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
		/// This property has same function as Tag property and provides you with additional separate storage of data.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Data"),Description("Indicates text that contains data about the tree node."),DevCoSerialize()]
		public string DataKeyString
		{
			get
			{
				if(m_DataKey==null)
					return "";
				return m_DataKey.ToString();
			}
			set
			{
				m_DataKey=value;
			}
		}

		/// <summary>
		/// Gets or sets the text displayed in the tree node.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Appearance"),Localizable(true),Description("Indicates text displayed in the tree node."),DevCoSerialize()]
		public string Text
		{
			get
			{
				return this.Cells[0].Text;
			}
			set
			{
				this.Cells[0].Text=value;
			}
		}

		/// <summary>
		/// Gets or sets the control hosted inside of the first node cell.
		/// </summary>
		/// <remarks>
		/// 	<para>When control is hosted inside of the cell, cell size is determined by the
		///     size of the control hosted inside of it. The cell will not display its text but it will display any image assigned
		///     or check box when control is hosted inside of it. The Style settings like Margin
		///     and Padding will still apply.</para>
		/// </remarks>
		[Browsable(true),Category("Behavior"),Description("Indicates control hosted inside of the cell."),DefaultValue(null)]
		public Control HostedControl
		{
			get {return this.Cells[0].HostedControl;}
			set
			{
				this.Cells[0].HostedControl=value;
			}
		}

		/// <summary>
		/// Gets the parent tree control that the tree node is assigned to.
		/// </summary>
		[Browsable(false)]
		public TreeGX TreeControl
		{
			get
			{
				return this.GetTreeControl();
			}
		}

		/// <summary>
		/// Gets or sets the layout of the cells inside the node. Default value is Horizontal layout which
		/// means that cell are positioned horizontally next to each other.
		/// </summary>
		[Browsable(true),DefaultValue(eCellLayout.Default),Category("Cells"),Description("Indicates layout of the cells inside the node."), DevCoSerialize()]
		public eCellLayout CellLayout
		{
			get {return m_CellLayout;}
			set
			{
				m_CellLayout=value;
				this.SizeChanged = true;
			}
		}
		
		/// <summary>
		/// Gets or sets the layout of the cell parts like check box, image and text. Layout can be horizontal (default)
		/// where parts of the cell are positioned next to each other horizontally, or vertical where
		/// parts of the cell are positioned on top of each other vertically.
		/// Alignment of the each part is controlled by alignment properties. This property affects only the first cell inside of the node.
		/// Use Cell.Layout property to change the part layout on each cell contained by node.
		/// </summary>
		[Browsable(true),DefaultValue(eCellPartLayout.Default),Category("Cells"),Description("Indicates the layout of the cell parts like check box, image and text."), DevCoSerialize()]
		public eCellPartLayout CellPartLayout
		{
			get {return m_Cells[0].Layout;}
			set
			{
				if(m_Cells[0].Layout!=value)
				{
					m_Cells[0].Layout=value;
				}
			}
		}

		/// <summary>
		/// Gets the collection of all Cells assigned to this node. There should be always at least one cell in a node which is default cell. Default
		/// collection contains a single cell.
		/// </summary>
		[Browsable(true),DesignerSerializationVisibility(DesignerSerializationVisibility.Content),Category("Cells"),Description("Collection of Cells assigned to this node.")]
		public CellCollection Cells
		{
			get {return m_Cells;}
		}

		/// <summary>
		/// Get collection of child node columns.
		/// </summary>
		[Browsable(true),DesignerSerializationVisibility(DesignerSerializationVisibility.Content),Category("Columns"),Description("Child nodes columns definition.")]
		public ColumnHeaderCollection NodesColumns
		{
			get {return m_NodesColumns;}
		}

//		/// <summary>
//		/// Gets or sets whether child node column header is visible or not.
//		/// </summary>
//		[Browsable(true),DefaultValue(true),Category("Columns"),Description("Indicates whether child node column header is visible or not.")]
//		public bool NodesColumnHeaderVisible
//		{
//			get {return m_NodesColumnHeaderVisible;}
//			set
//			{
//				if(m_NodesColumnHeaderVisible!=value)
//				{
//					m_NodesColumnHeaderVisible=value;
//					if(this.Expanded)
//						this.OnChildNodesSizeChanged();
//				}
//			}
//		}

		/// <summary>Gets or sets the style of the cells when node is expanded.</summary>
		/// <value>
		/// Reference to the style assigned to the node/cell or null value indicating that
		/// default style setting from tree control is applied. Default value is null.
		/// </value>
		/// <remarks>
		/// 	<para>When node is expanded the style specified here will be used on all cells
		///     associated with this node instead of the
		///     <see cref="Cell.StyleNormal">Cell.StyleNormal</see>. That way you can give
		///     different appearance to your node's cells when node is expanded.</para>
		/// 	<para>When property is set to null value the style setting from parent tree
		///     controls is used. <see cref="TreeGX.NodeStyleExpanded">NodeStyleExpanded</see> on
		///     TreeGX control is a root style for a cell.</para>
		/// </remarks>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style for the child nodes when node is expanded.")]
		public ElementStyle StyleExpanded
		{
			get {return m_StyleExpanded;}
			set
			{
				if(m_StyleExpanded!=value)
				{
					if(m_StyleExpanded!=null)
						m_StyleExpanded.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_StyleExpanded=value;
					if(this.Expanded)
						this.SizeChanged = true;
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the expanded style name used by node. This member is provided for internal use only. To set or get the style use StyleExpanded property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleExpandedName
		{
			get
			{
				if(m_StyleExpanded!=null)
					return m_StyleExpanded.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleExpanded"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleExpanded"].SetValue(this, tree.Styles[value]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the style used when Node is selected. Default value is NULL (VB
		/// Nothing)
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style for the nodes when node is selected.")]
		public ElementStyle StyleSelected
		{
			get {return m_StyleSelected;}
			set
			{
				if(m_StyleSelected!=value)
				{
					if(m_StyleSelected!=null)
						m_StyleSelected.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_StyleSelected=value;
					if(this.IsSelected)
						this.SizeChanged = true;
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the selected style name used by node. This member is provided for internal use only. To set or get the style use StyleSelected property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleSelectedName
		{
			get
			{
				if(m_StyleSelected!=null)
					return m_StyleSelected.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, tree.Styles[value]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the style used when mouse is over the Node. Default value is NULL
		/// (VB Nothing)
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style for the nodes when node is selected.")]
		public ElementStyle StyleMouseOver
		{
			get {return m_StyleMouseOver;}
			set
			{
				if(m_StyleMouseOver!=value)
				{
					if(m_StyleMouseOver!=null)
						m_StyleMouseOver.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_StyleMouseOver=value;
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the mouse over style name used by node. This member is provided for internal use only. To set or get the style use StyleMouseOver property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleMouseOverName
		{
			get
			{
				if(m_StyleMouseOver!=null)
					return m_StyleMouseOver.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, tree.Styles[value]);
				}
			}
		}

		/// <summary>
		///     Gets or sets the node style.
		/// </summary>
		/// <value>
		/// Reference to the style assigned to the node or null value indicating that default
		/// style setting from tree control is applied. Default value is null.
		/// </value>
		/// <remarks>
		/// 	<para>Style specified by this property will be used as default style for the node.
		///     Each cell within the node can also specify it's own style. Since node contains the
		///     cells using this style property can you for example create a border around all cell
		///     contained by the node.</para>
		/// 	<para>When this property is set to null value (default value) NodeStyle
		///     property on TreeGX control is used.</para>
		/// </remarks>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the node style.")]
		public ElementStyle Style
		{
			get {return m_Style;}
			set
			{
				if(m_Style!=value)
				{
					if(m_Style!=null)
						m_Style.StyleChanged-=new EventHandler(this.ElementStyleChanged);
					if(value!=null)
						value.StyleChanged+=new EventHandler(this.ElementStyleChanged);
					m_Style=value;
					this.SizeChanged = true;
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the style name used by node. This member is provided for internal use only. To set or get the style use Style property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleName
		{
			get
			{
				if(m_Style!=null)
					return m_Style.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["Style"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["Style"].SetValue(this, tree.Styles[value]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the part of the node mouse is over.
		/// </summary>
		internal eMouseOverNodePart MouseOverNodePart
		{
			get {return m_MouseOverNodePart;}
			set {m_MouseOverNodePart=value;}
		}


		/// <summary>
		/// Gets or sets the node horizontal offset from the position determined by the layout manager.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),DefaultValue(0)]
		internal int Offset
		{
			get {return m_Offset;}
			set
			{
				m_Offset=value;
				this.SizeChanged = true;
			}
		}

		/// <summary>
		/// Gets layout position of the node when tree is in Map layout mode and node is the first child note of top-level root node.
		/// To set the layout position for a node use TreeGX.SetNodeMapPosition method.
		/// </summary>
		[Browsable(false),DefaultValue(eMapPosition.Default),Category("Behavior"),Description("Indicates the position of the node when tree is in Map layout mode.")]
		public eMapPosition MapSubRootPosition
		{
			get {return m_MapSubRootPosition;}
		}
		
		/// <summary>
		/// Gets or sets the image alignment in relation to the text displayed by cell. This property affects only first default cell inside the node.
		/// Property with same name is available on each cell and you can use it to affect each cell individually.
		/// </summary>
		[Browsable(true),Category("Image Properties"),DefaultValue(eCellPartAlignment.NearCenter),Description("Gets or sets the image alignment in relation to the text displayed by cell."),DevCoSerialize()]
		public eCellPartAlignment ImageAlignment
		{
			get
			{
				return m_Cells[0].ImageAlignment;
			}
			set
			{
				if(m_Cells[0].ImageAlignment!=value)
				{
					m_Cells[0].ImageAlignment=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the image displayed when the tree node is in the unselected state.
		/// </summary>
		/// <remarks>
		/// Image specified will be used as a default image for any other node state where
		/// different image is not specified.
		/// </remarks>
		[Browsable(true),Category("Images"),Description("Indicates image displayed when the tree node is in the unselected state."),DefaultValue(null),DevCoSerialize()]
		public System.Drawing.Image Image
		{
			get{return m_Cells[0].Images.Image;}
			set
			{
				this.SizeChanged = true;
				m_Cells[0].Images.Image=value;
			}
		}
		
		/// <summary>
		/// Resets image to its default value. Windows Forms designer support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImage()
		{
			TypeDescriptor.GetProperties(this)["Image"].SetValue(this, null);
		}

		/// <summary>
		/// Gets or sets the image-list index value of the default image that is displayed by the tree nodes.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates the image-list index value of the default image that is displayed by the tree nodes."),System.ComponentModel.Editor(typeof(ImageIndexEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),DefaultValue(-1),DevCoSerialize()]
		public int ImageIndex
		{
			get {return this.Cells[0].Images.ImageIndex;}
			set
			{
				this.SizeChanged = true;
				this.Cells[0].Images.ImageIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets the image displayed when mouse is over the tree node.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates the image displayed when mouse is over the tree node."),DefaultValue(null),DevCoSerialize()]
		public System.Drawing.Image ImageMouseOver
		{
			get{return this.Cells[0].Images.ImageMouseOver;}
			set
			{
				this.Cells[0].Images.ImageMouseOver = value;
				this.SizeChanged = true;
			}
		}
		
		/// <summary>
		/// Resets ImageMouseOver to its default value. Windows Forms designer support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImageMouseOver()
		{
			TypeDescriptor.GetProperties(this)["ImageMouseOver"].SetValue(this, null);
		}

		/// <summary>
		/// Gets or sets the image-list index value of the image that is displayed by the tree nodes when mouse is over the node.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates the image-list index value of the image that is displayed by the tree nodes when mouse is over the node."),System.ComponentModel.Editor(typeof(ImageIndexEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),DefaultValue(-1),DevCoSerialize()]
		public int ImageMouseOverIndex
		{
			get {return this.Cells[0].Images.ImageMouseOverIndex;}
			set
			{
				this.SizeChanged = true;
				this.Cells[0].Images.ImageMouseOverIndex = value;
			}
		}

//		/// <summary>
//		/// Gets or sets the image displayed when node is disabled.
//		/// </summary>
//		[Browsable(true),Category("Images"),Description("Indicates the image displayed when node is disabled."),DefaultValue(null),DevCoSerialize()]
//		public System.Drawing.Image ImageDisabled
//		{
//			get{return m_Image Disabled;}
//			set
//			{
//				if(m_ImageDisabled!=value)
//				{
//					this.SizeChanged = true;
//					m_ImageDisabled=value;
//				}
//			}
//		}
		
		/// <summary>
		/// Resets ImageDisabled to its default value. Windows Forms designer support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImageDisabled()
		{
			TypeDescriptor.GetProperties(this)["ImageDisabled"].SetValue(this, null);
		}

//		/// <summary>
//		/// Gets or sets the image-list index value of the image that is displayed by the tree nodes when node is disabled.
//		/// </summary>
//		[Browsable(true),Category("Images"),Description("Indicates the image-list index value of the image that is displayed by the tree nodes when node is disabled."),System.ComponentModel.Editor(typeof(ImageIndexEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),DefaultValue(-1),DevCoSerialize()]
//		public int ImageDisabledIndex
//		{
//			get {return m_ImageDisabledIndex;}
//			set
//			{
//				if(m_ImageDisabledIndex!=value)
//				{
//					this.SizeChanged = true;
//					m_ImageDisabledIndex=value;
//				}
//			}
//		}

		/// <summary>
		/// Gets or sets the image displayed when node is expanded.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates the image displayed when node is expanded."),DefaultValue(null),DevCoSerialize()]
		public System.Drawing.Image ImageExpanded
		{
			get{return this.Cells[0].Images.ImageExpanded;}
			set
			{
				this.Cells[0].Images.ImageExpanded = value;
				this.SizeChanged = true;
			}
		}
		
		/// <summary>
		/// Resets ImageExpanded to its default value. Windows Forms designer support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImageExpanded()
		{
			TypeDescriptor.GetProperties(this)["ImageExpanded"].SetValue(this, null);
		}

		/// <summary>
		/// Gets or sets the image-list index value of the image that is displayed by the tree nodes when node is expanded.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Indicates the image-list index value of the image that is displayed by the tree nodes when node is expanded."),System.ComponentModel.Editor(typeof(ImageIndexEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),DefaultValue(-1),DevCoSerialize()]
		public int ImageExpandedIndex
		{
			get {return this.Cells[0].Images.ImageExpandedIndex;}
			set
			{
				this.SizeChanged = true;
				this.Cells[0].Images.ImageExpandedIndex = value;
			}
		}

		/// <summary>
		/// Property Editor support for ImageIndex selection
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					return tree.ImageList;
				}
				return null;
			}
		}
		
		/// <summary>
		/// Sets the node relative position form the root when map layout is used.
		/// </summary>
		/// <param name="pos">Relative node position.</param>
		internal void SetMapSubRootPosition(eMapPosition pos)
		{
			m_MapSubRootPosition=pos;
		}

		/// <summary>
		/// Gets or sets the NodeConnector object that describes the type of the connector used for
		/// displaying connection between current node and its parent node. 
		/// Default value is null which means that settings from TreeGX control are used.
		/// </summary>
		/// <seealso cref="TreeGX.RootConnector">RootConnector Property (DevComponents.Tree.TreeGX)</seealso>
		/// <seealso cref="TreeGX.NodesConnector">NodesConnector Property (DevComponents.Tree.TreeGX)</seealso>
		[Browsable(true),Category("Connectors"),DefaultValue(null),Editor(typeof(Design.NodeConnectorTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the nested nodes connector.")]
		public NodeConnector ParentConnector
		{
			get {return m_ParentConnector;}
			set
			{
				if(m_ParentConnector!=value)
				{
					if(m_ParentConnector!=null)
						m_ParentConnector.AppearanceChanged-=new EventHandler(this.ConnectorAppearanceChanged);
					if(value!=null)
						value.AppearanceChanged+=new EventHandler(this.ConnectorAppearanceChanged);
					m_ParentConnector=value;
					this.OnDisplayChanged();
				}
			}
		}

		/// <summary>
		/// Gets the collection of the parent connector line relative points. By default this collection is empty which indicates that
		/// connector line is drawn using predefined path. Points added here are the points through which the connector line will travel to the
		/// parent node. The point coordinates added to this collection are relative from the top-left corner of this node.
		/// </summary>
		[Browsable(true),Category("Connectors"),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ConnectorPointsCollection ParentConnectorPoints
		{
			get {return m_ParentConnectorPoints;}
		}

		/// <summary>
		/// Gets or sets whether node is visible.
		/// </summary>
		[Browsable(true),Category("Behavior"),DefaultValue(true),Description("Indicated whether node is visible"),DevCoSerialize()]
		public bool Visible
		{
			get {return m_Visible;}
			set
			{
				m_Visible=value;
				this.OnVisibleChanged();
				this.SizeChanged = true;
			}
		}

		/// <summary>
		/// Gets or sets whether command button is visible. Default value is false.
		/// Command button can be used to display for example popup menu with commands for node,
		/// or to display the list of linked nodes.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Command Button"),Description("Indicates visibility of command button."),DevCoSerialize()]
		public bool CommandButton
		{
			get {return m_CommandButton;}
			set
			{
				m_CommandButton=value;
				this.OnDisplayChanged();
			}
		}
		
		/// <summary>
		/// Gets or sets internal value that indicates that node is on "path" of the selected node.
		/// </summary>
		internal bool SelectedConnectorMarker
		{
			get { return m_SelectedConnectorMarker; }
			set { m_SelectedConnectorMarker = value; }
		}

		#endregion

		#region Methods
		/// <summary>
		/// Invalidates node and causes a paint message to be sent to the tree.
		/// </summary>
		public void Invalidate()
		{
			TreeGX tree=this.TreeControl;
			if(tree!=null)
				tree.InvalidateNode(this);
		}

		/// <summary>Initiates the editing of node text.</summary>
		/// <remarks>
		/// This method by default edits text stored in Node.Text. Call to this method is
		/// same as calling the overload method BeginData(0) with zero as parameter. Use BeginData
		/// overload method to begin editing the specific column for multi-column nodes.
		/// </remarks>
		public void BeginEdit()
		{
			BeginEdit(0);
		}
		
		/// <summary>Initiates the editing of node text.</summary>
		/// <param name="initialText">
		/// The initial text to be entered into the edit TextBox. Specify null to use existing text.
		/// </param>
		public void BeginEdit(string initialText)
		{
			BeginEdit(0, initialText);
		}

		/// <summary>Initiates text editing of certain Node column.</summary>
		/// <param name="iColumnIndex">
		/// Zero based index of a column to begin editing for. Column 0 always corresponds to
		/// Node.Text property.
		/// </param>
		public void BeginEdit(int iColumnIndex)
		{
			BeginEdit(iColumnIndex, null);
		}
		
		/// <summary>Initiates text editing of certain Node column.</summary>
		/// <param name="iColumnIndex">
		/// Zero based index of a column to begin editing for. Column 0 always corresponds to
		/// Node.Text property.
		/// </param>
		/// <param name="initialText">
		/// The initial text to be entered into the edit TextBox. Specify null to edit existing text.
		/// </param>
		public void BeginEdit(int iColumnIndex, string initialText)
		{
			TreeGX tree=this.TreeControl;
			if(tree==null)
				return;
			tree.EditCell(this.Cells[iColumnIndex], eTreeAction.Code, initialText);
		}

		/// <summary>Makes a "shallow" copy of a Node.</summary>
		/// <remarks>
		/// Shallow copy of a Node is a exact copy of Node but <strong>without</strong> copy
		/// of all child nodes in Nodes collection.
		/// </remarks>
		public virtual Node Copy()
		{
			// TODO: Verify that all properties are copied
			Node n=new Node();
			n.CellLayout=this.CellLayout;
			n.Cells.Clear();
			foreach(Cell c in this.Cells)
				n.Cells.Add(c.Copy());
			n.Checked=this.Checked;
			n.CommandButton=this.CommandButton;
			n.ExpandVisibility=this.ExpandVisibility;
			n.Image=this.Image;
//			n.ImageDisabled=this.ImageDisabled;
//			n.ImageDisabledIndex=this.ImageDisabledIndex;
			n.ImageExpanded=this.ImageExpanded;
			n.ImageExpandedIndex=this.ImageExpandedIndex;
			n.ImageIndex=this.ImageIndex;
			n.ImageMouseOver=this.ImageMouseOver;
			n.ImageMouseOverIndex=this.ImageMouseOverIndex;
			//n.NodesColumnHeaderVisible=this.NodesColumnHeaderVisible;
			foreach(ColumnHeader c in this.NodesColumns)
				n.NodesColumns.Add(c.Copy());
			n.ParentConnector=this.ParentConnector;
			n.Style=this.Style;
			n.StyleExpanded=this.StyleExpanded;
			n.StyleMouseOver=this.StyleMouseOver;
			n.StyleSelected=this.StyleSelected;
			n.Tag=this.Tag;
			n.DataKey = this.DataKey;
			n.Text=this.Text;
			n.Visible=this.Visible;
            n.Selectable = this.Selectable;
			n.ParentConnectorPoints.AddRange(this.ParentConnectorPoints.ToArray());
			
			return n;
		}

		/// <summary>Makes a "deep" copy of a node.</summary>
		/// <remarks>
		/// Deep copy of Node is a exact copy of Node including exact copies of all child nodes
		/// in this node's Nodes collection.
		/// </remarks>
		public virtual Node DeepCopy()
		{
			Node n=this.Copy();
			foreach(Node c in this.Nodes)
				n.Nodes.Add(c.DeepCopy());
			return n;
		}

		/// <summary>
		/// Collapses the tree node.
		/// </summary>
		public void Collapse()
		{
			this.SetExpanded(false,eTreeAction.Code);
		}
		
		/// <summary>
		/// Collapses the tree node.
		/// </summary>
		/// <param name="action">Action that caused the event</param>
		public void Collapse(eTreeAction action)
		{
			this.SetExpanded(false,action);
		}

		/// <summary>
		/// Collapses all the child tree nodes.
		/// </summary>
		public void CollapseAll()
		{
			TreeGX tree=this.TreeControl;
			if(tree!=null)
				tree.BeginUpdate();
			this.Collapse();
			foreach(Node node in this.Nodes)
				node.CollapseAll();
			if(tree!=null)
				tree.EndUpdate();
		}

		/// <summary>
		/// Ends the editing of the node text or column.
		/// </summary>
		/// <param name="cancelChanges"><strong>true</strong> if the editing of the tree node label text was canceled without being saved; otherwise, <strong>false</strong>. </param>
		public void EndEdit(bool cancelChanges)
		{
			if(!this.IsEditing)
				return;
			TreeGX tree=this.TreeControl;
			if(tree==null)
				return;
			if(cancelChanges)
				tree.CancelCellEdit(eTreeAction.Code);
			else
				tree.EndCellEditing(eTreeAction.Code);
		}

		/// <summary>
		/// Ensures that the node is visible, expanding nodes and scrolling the control as necessary.
		/// </summary>
		public void EnsureVisible()
		{
			NodeOperations.EnsureVisible(this);
		}

		/// <summary>
		/// Expands the node.
		/// </summary>
		/// <remarks>
		/// The Expand method expands the current Node down to the next level of nodes.
		/// The state of a Node is persisted. For example, if the next level of child nodes was not collapsed previously, when the Expand method is called, the child nodes appear in their previously expanded state.
		/// </remarks>
		public void Expand()
		{
			SetExpanded(true,eTreeAction.Code);
		}
		
		/// <summary>
		/// Expands the node.
		/// </summary>
		/// <remarks>
		/// The Expand method expands the current Node down to the next level of nodes.
		/// The state of a Node is persisted. For example, if the next level of child nodes was not collapsed previously, when the Expand method is called, the child nodes appear in their previously expanded state.
		/// </remarks>
		/// <param name="action">Action that caused the event.</param>
		public void Expand(eTreeAction action)
		{
			SetExpanded(true,action);
		}

		/// <summary>
		/// Expands all the child tree nodes.
		/// </summary>
		public void ExpandAll()
		{
			TreeGX tree=this.TreeControl;
			if(tree!=null)
				tree.BeginUpdate();
			try
			{
				foreach(Node node in this.Nodes)
				{
					node.Expand();
					node.ExpandAll();
				}
			}
			finally
			{
				if(tree!=null)
					tree.EndUpdate();
			}
		}

		/// <summary>
		/// Removes the current node from the control.
		/// </summary>
		/// <remarks>
		/// When the Remove method is called, the node and any child nodes assigned to the Node are removed from the Tree. The removed child nodes are removed from the Tree, but are still attached to this node.
		/// </remarks>
		public void Remove()
		{
			if(this.Parent!=null)
				this.Parent.Nodes.Remove(this);
			else if(this.TreeControl!=null && this.TreeControl.Nodes.Contains(this))
				this.TreeControl.Nodes.Remove(this);
		}
		
		/// <summary>
		/// Removes the current node from the control and provides information about source of action
		/// </summary>
		/// <remarks>
		/// When the Remove method is called, the node and any child nodes assigned to the Node are removed from the Tree. The removed child nodes are removed from the Tree, but are still attached to this node.
		/// </remarks>
		public void Remove(eTreeAction source)
		{
			if(this.Parent!=null)
				this.Parent.Nodes.Remove(this, source);
			else if(this.TreeControl!=null && this.TreeControl.Nodes.Contains(this))
				this.TreeControl.Nodes.Remove(this, source);
		}

		/// <summary>
		/// Toggles the node to either the expanded or collapsed state.
		/// </summary>
		public void Toggle()
		{
			if(this.Expanded)
				this.Collapse();
			else
				this.Expand();
		}
		
		/// <summary>
		/// Toggles the node to either the expanded or collapsed state.
		/// </summary>
		/// <param name="action">Action that caused the event.</param>
		public void Toggle(eTreeAction action)
		{
			if(this.Expanded)
				this.Collapse(action);
			else
				this.Expand(action);
		}

		/// <summary>Returns string representation of the Node.</summary>
		public override string ToString()
		{
			return m_Text;
		}
		#endregion

		#region Private Implementation
		/// <summary>
		/// Called after new cell has been added to Cells collection.
		/// </summary>
		/// <param name="cell">Reference to the new cell added.</param>
		internal void OnCellInserted(Cell cell)
		{
			this.SizeChanged = true;
			OnDisplayChanged();
		}

		/// <summary>
		/// Called after cell has  been removed from Cells collection.
		/// </summary>
		/// <param name="cell">Reference to the removed cell.</param>
		internal void OnCellRemoved(Cell cell)
		{
			this.SizeChanged = true;
			OnDisplayChanged();
		}

		/// <summary>
		/// Gets or sets the child column header height.
		/// </summary>
		internal int ColumnHeaderHeight
		{
			get {return m_ColumnHeaderHeight;}
			set {m_ColumnHeaderHeight=value;}
		}
		
		/// <summary>
		/// Occurs when property on the node has changed that influences the size of the node.
		/// </summary>
		private void OnSizeChanged()
		{
			if(m_SizeChanged)
			{
				TreeGX tree = this.TreeControl;
				if(tree!=null)
					tree.SetPendingLayout();
			}
		}

		/// <summary>
		/// Occurs when any image property for the cell has changed.
		/// </summary>
		internal void OnImageChanged()
		{
			this.SizeChanged = true;
			OnDisplayChanged();
		}

		/// <summary>
		/// Occurs when size of the child nodes has changed.
		/// </summary>
		internal void OnChildNodesSizeChanged()
		{
			foreach(Node node in this.Nodes)
			{
				node.SizeChanged = true;
				node.OnChildNodesSizeChanged();
			}
		}
		private void OnColumnHeaderSizeChanged(object sender, EventArgs e)
		{
			if(this.Expanded)
				this.SizeChanged = true;
		}
		private void SetExpanded(bool e, eTreeAction action)
		{
			if(e==m_Expanded)
				return;

			INodeNotify notification=GetINodeNotify();
			if(notification!=null)
			{
				TreeGXNodeCancelEventArgs cancelArgs=new TreeGXNodeCancelEventArgs(action , this);
				if(e)
					notification.OnBeforeExpand(cancelArgs);
				else
					notification.OnBeforeCollapse(cancelArgs);
				if(cancelArgs.Cancel)
					return;				
			}
#if TRIAL
			if(NodeOperations.ColorCountExp==0)
				return;
#endif
			m_Expanded=e;
			
			if(notification!=null)
				notification.ExpandedChanged(this);
			
			if(notification!=null)
			{
				TreeGXNodeEventArgs args=new TreeGXNodeEventArgs(action,this);
				if(e)
					notification.OnAfterExpand(args);
				else
					notification.OnAfterCollapse(args);
			}
			
			foreach(Node node in this.Nodes)
			{
				node.OnParentExpandedChanged(m_Expanded);	
			}
		}
		
		internal void OnParentExpandedChanged(bool expanded)
		{
			foreach(Cell cell in this.Cells)
				cell.OnParentExpandedChanged(expanded);
			foreach(Node node in this.Nodes)
			{
				node.OnParentExpandedChanged(expanded);
			}
		}
		
		private TreeGX GetTreeControl()
		{
			Node node=this;
			while(node.Parent!=null)
				node=node.Parent;
			return node.internalTreeControl;
		}
		private INodeNotify GetINodeNotify()
		{
			TreeGX tree=this.TreeControl;
			if(tree!=null) return tree as INodeNotify;
			return null;
		}
		private void ElementStyleChanged(object sender, EventArgs e)
		{
			this.SizeChanged=true;
			this.OnDisplayChanged();
		}
		/// <summary>
		/// Called when visual part of the node has changed due to the changes of its properties or properties of the cells contained by node.
		/// </summary>
		internal void OnDisplayChanged()
		{
			TreeGX tree = this.TreeControl;
			if(tree!=null && !tree.SuspendPaint)
			{
				if(this.SizeChanged)
					this.TreeControl.RecalcLayout();
				this.TreeControl.Invalidate(true);
			}
		}

		private void ConnectorAppearanceChanged(object sender, EventArgs e)
		{
			this.OnDisplayChanged();
		}

		/// <summary>
		/// Called after new node has been addded to Nodes collection.
		/// </summary>
		/// <param name="node">Reference to the new node.</param>
		internal void OnChildNodeInserted(Node node)
		{
			SizeChanged=true;
		}

		/// <summary>
		/// Called after node has been removed from Nodes collection.
		/// </summary>
		/// <param name="node">Reference to the node that is removed.</param>
		internal void OnChildNodeRemoved(Node node)
		{
			SizeChanged=true;
			if(node.IsSelected)
			{
				if(this.TreeControl!=null)
					this.TreeControl.SelectedNode=null;
			}
		}
		
		internal void OnVisibleChanged()
		{
			foreach (Cell cell in this.Cells)
			{
				cell.OnVisibleChanged();
			}
			
			foreach(Node node in this.Nodes)
			{
				node.OnVisibleChanged();
			}
		}
		#endregion

		#region Event Invokation
		protected internal virtual void InvokeNodeMouseDown(object sender, MouseEventArgs e)
		{
			if(NodeMouseDown!=null)
				NodeMouseDown(this, e);
			
			foreach(Cell cell in m_Cells)
			{
				cell.InvokeNodeMouseDown(sender, e);
			}
		}
		
		protected internal virtual void InvokeNodeMouseUp(object sender, MouseEventArgs e)
		{
			if(NodeMouseUp!=null)
				NodeMouseUp(this, e);
			
			foreach(Cell cell in m_Cells)
			{
				cell.InvokeNodeMouseUp(sender, e);
			}
		}
		
		protected internal virtual void InvokeNodeMouseMove(object sender, MouseEventArgs e)
		{
			if(NodeMouseMove!=null)
				NodeMouseMove(this, e);
			
			foreach(Cell cell in m_Cells)
			{
				cell.InvokeNodeMouseMove(sender, e);
			}
		}
		
		protected internal virtual void InvokeNodeClick(object sender, EventArgs e)
		{
			if(NodeClick!=null)
				NodeClick(this, e);
			
			foreach(Cell cell in m_Cells)
			{
				cell.InvokeNodeClick(sender, e);
			}
		}
		
		protected internal virtual void InvokeNodeDoubleClick(object sender, EventArgs e)
		{
			if(NodeDoubleClick!=null)
				NodeDoubleClick(this, e);
		}
		
		protected internal virtual void InvokeNodeMouseEnter(object sender, EventArgs e)
		{
			if(NodeMouseEnter!=null)
				NodeMouseEnter(this, e);
		}
		
		protected internal virtual void InvokeNodeMouseLeave(object sender, EventArgs e)
		{
			if(NodeMouseLeave!=null)
				NodeMouseLeave(this, e);
			
			foreach(Cell cell in m_Cells)
			{
				cell.InvokeNodeMouseLeave(sender, e);
			}
		}
		
		protected internal virtual void InvokeNodeMouseHover(object sender, EventArgs e)
		{
			if(NodeMouseHover!=null)
				NodeMouseHover(this, e);
		}
		
		internal bool FireHoverEvent
		{
			get { return NodeMouseHover!=null; }
		}
		
		protected internal virtual void InvokeMarkupLinkClick(Cell cell, MarkupLinkClickEventArgs args)
		{
			if(MarkupLinkClick!=null)
				MarkupLinkClick(cell, args);
			TreeGX tree = this.TreeControl;
			if(tree!=null)
				tree.InvokeMarkupLinkClick(cell, args);
		}
		#endregion
	}
}
