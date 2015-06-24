using System;
using System.Drawing;
using System.Collections;

namespace DevComponents.Tree.Layout
{
	/// <summary>
	/// Summary description for NodeLayout.
	/// </summary>
	internal abstract class NodeLayout
	{
		#region Private Variables
		protected int m_Height=0;
		protected int m_Width=0;
		protected TreeGX m_Tree=null;
		protected Rectangle m_ClientArea;
		//protected int m_ExpandAreaWidth=8;
        private Size m_ExpandPartSize = new Size(9, 9);
		private int m_CommandAreaWidth=10;
		private int m_TreeLayoutChildNodeIndent = 16;

		private System.Windows.Forms.LeftRightAlignment m_LeftRight=System.Windows.Forms.LeftRightAlignment.Left;
		private int m_NodeVerticalSpacing=0;
		private int m_NodeHorizontalSpacing=0;
		private CellLayout m_CellLayout=null;
		private Graphics m_Graphics=null;
		#endregion

		public NodeLayout(TreeGX treeControl, Rectangle clientArea)
		{
			m_Tree=treeControl;
			m_ClientArea=clientArea;
		}

		/// <summary>
		/// Performs layout of the nodes inside of the tree control.
		/// </summary>
		public virtual void PerformLayout()
		{
		}

		/// <summary>
		/// Performs layout for single unassigned node. Node does not have to be part of the tree control.
		/// </summary>
		/// <param name="node">Node to perform layout on.</param>
		public virtual void PerformSingleNodeLayout(Node node)
		{
			if(node==null)
				return;

			this.PrepareStyles();
			// Get default Columns

			System.Drawing.Graphics graphics=this.GetGraphics();
			try
			{
				NodeLayoutContextInfo layoutInfo=this.GetDefaultNodeLayoutContextInfo(graphics);
				layoutInfo.ContextNode=node;
				LayoutNode(layoutInfo);
			}
			finally
			{
				graphics.Dispose();
			}
		}

		public int Width
		{
			get {return m_Width;}
		}

		public int Height
		{
			get {return m_Height;}
		}

		public Rectangle ClientArea
		{
			get {return m_ClientArea;}
			set {m_ClientArea=value;}
		}
		
		public Graphics Graphics
		{
			get { return m_Graphics;}
			set { m_Graphics = value;}
		}
		
		internal bool DisposeGraphics
		{
			get
			{
				return (m_Graphics == null);
			}
		}

		protected virtual System.Drawing.Graphics GetGraphics()
		{
			if(m_Graphics!=null)
				return m_Graphics;
			
			Graphics g=m_Tree.CreateGraphics();
			if(m_Tree.AntiAlias)
			{
				g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			}
			return g;
		}

		/// <summary>
		/// Resizes all styles and prepares them for layout.
		/// </summary>
		protected virtual void PrepareStyles()
		{
			// Resize styles if needed
			foreach(ElementStyle es in m_Tree.Styles)
			{
				if(es.SizeChanged)
					ElementStyleLayout.CalculateStyleSize(es,m_Tree.Font);
			}
		}

		/// <summary>
		/// Returns default top-level columns for tree control.
		/// </summary>
		/// <returns>Returns array list of ColumnInfo objects.</returns>
		protected virtual ArrayList GetDefaultColumnInfo()
		{
			ArrayList ci=new ArrayList();
			
			ColumnHeaderCollection columns=m_Tree.Columns;
			if(columns!=null)
			{
				foreach(ColumnHeader h in columns)
				{
					ci.Add(new ColumnInfo(h.Bounds.Width, h.Visible));
				}
			}

			return ci;
		}

		/// <summary>
		/// Returns column information for a given node.
		/// </summary>
		/// <param name="node">Node to return column information for</param>
		/// <returns>Returns array list of ColumnInfo objects or null if there are no columns defined.</returns>
		protected virtual ArrayList GetNodeColumnInfo(Node node)
		{
			if(node.NodesColumns.Count==0)
				return null;

			ArrayList ci=new ArrayList();
			foreach(ColumnHeader h in node.NodesColumns)
			{
				ci.Add(new ColumnInfo(h.Width.Absolute, h.Visible));
			}
			return ci;
		}

		/// <summary>
		/// Gets or sets the vertical spacing between nodes in pixels.
		/// </summary>
		public virtual int NodeVerticalSpacing
		{
			get {return m_NodeVerticalSpacing;}
			set {m_NodeVerticalSpacing=value;}
		}

		/// <summary>
		/// Gets or sets the horizontal spacing between nodes in pixels.
		/// </summary>
		public virtual int NodeHorizontalSpacing
		{
			get {return m_NodeHorizontalSpacing;}
			set {m_NodeHorizontalSpacing=value;}
		}

		/// <summary>
		/// Gets or sets the child node indent in pixels.
		/// </summary>
		public virtual int TreeLayoutChildNodeIndent
		{
			get {return m_TreeLayoutChildNodeIndent; }
			set { m_TreeLayoutChildNodeIndent = value; }
		}

		/// <summary>
		/// Returns column header collection for the given column template name.
		/// </summary>
		/// <param name="name">Name of the column template.</param>
		/// <returns>Column header collection or null if template name cannot be found.</returns>
		public virtual ColumnHeaderCollection GetColumnHeader(string name)
		{
			if(name=="" || name==null)
				return null;
			return m_Tree.Headers.GetByName(name).Columns;
		}

		/// <summary>
		/// Returns width of the expand button area. Default is 8 pixels.
		/// </summary>
		protected virtual int ExpandAreaWidth
		{
			get {return m_ExpandPartSize.Width;}
		}

		/// <summary>
		/// Gets or sets width of command button area. Default is 8 pixels.
		/// </summary>
		public virtual int CommandAreaWidth
		{
			get {return m_CommandAreaWidth;}
			set {m_CommandAreaWidth=value;}
		}

		/// <summary>
		/// Sets the position and size of the node command button.
		/// </summary>
		/// <param name="layoutInfo">Node layout context information</param>
		protected virtual void LayoutCommandPart(NodeLayoutContextInfo layoutInfo, ElementStyle nodeStyle)
		{
			// Command part is right-aligned just before the node border
			Rectangle bounds=new Rectangle(layoutInfo.ContextNode.ContentBounds.Right-this.CommandAreaWidth-
				ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Border,eStyleSide.Right),layoutInfo.ContextNode.ContentBounds.Y+
				ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Border,eStyleSide.Top), 
				this.CommandAreaWidth, layoutInfo.ContextNode.ContentBounds.Height-
				ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Border,eStyleSide.Top)-
				ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Border,eStyleSide.Bottom));
//			Rectangle bounds=new Rectangle(layoutInfo.ContextNode.ContentBounds.Right-this.CommandAreaWidth-
//				ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Border,eStyleSide.Right),layoutInfo.ContextNode.ContentBounds.Y, 
//				this.CommandAreaWidth, layoutInfo.ContextNode.ContentBounds.Height);
			layoutInfo.ContextNode.CommandBoundsRelative=bounds;
		}

		/// <summary>
		/// Determines the rectangle of the +/- part of the tree node that is used to expand node.
		/// </summary>
		/// <param name="layoutInfo">Node layout context information</param>
		protected virtual void LayoutExpandPart(NodeLayoutContextInfo layoutInfo, bool bLeftNode)
		{
			Node node=layoutInfo.ContextNode;

			Size partSize=GetExpandPartSize();

			Rectangle bounds=new Rectangle(0,0,partSize.Width,partSize.Height);
			
			bounds.Y=(node.BoundsRelative.Height-bounds.Height)/2;

			if(bLeftNode)
				bounds.X=(this.ExpandAreaWidth-bounds.Width)/2;
			else
				bounds.X=node.BoundsRelative.Right-this.ExpandAreaWidth+(this.ExpandAreaWidth-partSize.Width)/2;

			node.SetExpandPartRectangle(bounds);
		}

		/// <summary>
		/// Returns the size of the node expand part.
		/// </summary>
		/// <returns>Size of the expand part, default 8,8.</returns>
		protected virtual Size GetExpandPartSize()
		{
			return m_ExpandPartSize;
		}

		/// <summary>
		/// Gets or sets the size of the expand part that is expanding/collapsing the node. Default value is 8,8.
		/// </summary>
		public System.Drawing.Size ExpandPartSize
		{
			get {return m_ExpandPartSize;}
			set 
            {
                m_ExpandPartSize=value;
            }
		}

		/// <summary>
		/// Provides the layout for single node.
		/// </summary>
		/// <param name="layoutInfo">Layout information.</param>
		protected virtual void LayoutNode(NodeLayoutContextInfo layoutInfo)
		{
//			if(!layoutInfo.ContextNode.SizeChanged)
//				return;

			bool bHasExpandPart=this.HasExpandPart(layoutInfo);
			bool bHasCommandPart=this.HasCommandPart(layoutInfo);

			Node node=layoutInfo.ContextNode;
			
			Rectangle nodeRect=Rectangle.Empty;
			Rectangle nodeContentRect=Rectangle.Empty; // Node content rect excludes expand rect

			int height=0, width=0;
			
			// Left node relative to the main root node...
			bool bLeftNode=(layoutInfo.MapPositionNear && layoutInfo.LeftToRight);
			if(bLeftNode && bHasExpandPart || this.ReserveExpandPartSpace)
			{
				width+=(this.ExpandAreaWidth+this.GetCellLayout().CellPartSpacing);
			}

			int x=width;		// relative to 0,0 of the node
			int y=0;			// Relative to 0,0 of the node
			
			// Apply node style
			ElementStyle nodeStyle=null;
			if(node.Expanded && node.StyleExpanded!=null)
				nodeStyle=node.StyleExpanded;
			else if(node.Style!=null)
				nodeStyle=node.Style;
			else
				nodeStyle=layoutInfo.DefaultNodeStyle;

			nodeContentRect.X=x;

			if(nodeStyle!=null)
			{
				x+=ElementStyleLayout.LeftWhiteSpace(nodeStyle); // nodeStyle.MarginLeft+nodeStyle.PaddingLeft;
				y+=ElementStyleLayout.TopWhiteSpace(nodeStyle); //nodeStyle.MarginTop+nodeStyle.PaddingTop;
				nodeContentRect.X+=nodeStyle.MarginLeft;
				nodeContentRect.Y+=nodeStyle.MarginTop;
			}

			Size size=this.GetCellLayout().LayoutCells(layoutInfo,x,y);

			node.SetCellsBounds(new Rectangle(x,y,size.Width,size.Height));

			height=size.Height;
			width+=size.Width;

			nodeContentRect.Width=size.Width;
			nodeContentRect.Height=size.Height;

			if(nodeStyle!=null)
			{
				nodeContentRect.Width+=(ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Left)+
					ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Right));
				nodeContentRect.Height+=(ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Top)+
					ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Bottom));

				width+=(ElementStyleLayout.HorizontalStyleWhiteSpace(nodeStyle));
				height+=(ElementStyleLayout.VerticalStyleWhiteSpace(nodeStyle));
			}

			if(!bLeftNode && bHasExpandPart)
				width+=this.ExpandAreaWidth;

			if(bHasCommandPart)
			{
				width+=this.CommandAreaWidth;
				nodeContentRect.Width+=this.CommandAreaWidth;
			}

			nodeRect.Height=height;
			nodeRect.Width=width;
			node.SetBounds(nodeRect);
			node.SetContentBounds(nodeContentRect);

			if(bHasCommandPart)
				LayoutCommandPart(layoutInfo, nodeStyle);
			else
				node.CommandBoundsRelative=Rectangle.Empty;

			if(bHasExpandPart)
				LayoutExpandPart(layoutInfo,bLeftNode);
			else
				node.SetExpandPartRectangle(Rectangle.Empty);

			node.SizeChanged=false;

			// Calculate size and location of node column header if any
			//if(node.NodesColumnHeaderVisible)
			{
				//layoutInfo.Left+=this.NodeLevelOffset;
				LayoutColumnHeader(layoutInfo);
				//layoutInfo.Left-=this.NodeLevelOffset;
			}
		}

		/// <summary>
		/// Returns true if given node has expand part.
		/// </summary>
		/// <param name="layoutInfo">Layout context information.</param>
		/// <returns></returns>
		protected virtual bool HasExpandPart(NodeLayoutContextInfo layoutInfo)
		{
			Node node=layoutInfo.ContextNode;
			if(node.ExpandVisibility==eNodeExpandVisibility.Auto)
			{
				if(IsRootNode(node) && !RootHasExpandedPart || !NodeOperations.GetAnyVisibleNodes(node))
					return false;
				return true;
			}
			else
				return (node.ExpandVisibility==eNodeExpandVisibility.Visible);
		}

		/// <summary>
		/// Returns whether given node has command part.
		/// </summary>
		/// <param name="layoutInfo">Layout context information.</param>
		/// <returns>True if command part should be drawn otherwise false.</returns>
		protected virtual bool HasCommandPart(NodeLayoutContextInfo layoutInfo)
		{
			return layoutInfo.ContextNode.CommandButton;
		}

		/// <summary>
		/// Returns true if root node should have expanded part
		/// </summary>
		protected virtual bool RootHasExpandedPart
		{
			get {return true;}
		}

		/// <summary>
		/// Returns true if expand part space should be accounted for even if they expand part is not visible or need to be displayed. Default value is false.
		/// </summary>
		protected virtual bool ReserveExpandPartSpace
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Returns class responsible for cell layout.
		/// </summary>
		/// <returns>Cell layout class.</returns>
		protected virtual CellLayout GetCellLayout()
		{
			if(m_CellLayout==null)
				m_CellLayout=new CellLayout();
			return m_CellLayout;
		}

		/// <summary>
		/// Offsets node location and location of it's child nodes bounds.
		/// </summary>
		/// <param name="node">Node to offset.</param>
		/// <param name="x">Horizontal offset.</param>
		/// <param name="y">Vertical offset.</param>
		protected virtual void OffsetNodeLocation(Node node, int x, int y)
		{
			node.SetBounds(new Rectangle(node.BoundsRelative.X+x,node.BoundsRelative.Y+y,node.BoundsRelative.Width,node.BoundsRelative.Height));
			if(node.Expanded)
				node.ChildNodesBounds=new Rectangle(node.ChildNodesBounds.X+x,node.ChildNodesBounds.Y+y,node.ChildNodesBounds.Width,node.ChildNodesBounds.Height);
		}

		protected virtual NodeLayoutContextInfo GetDefaultNodeLayoutContextInfo(System.Drawing.Graphics graphics)
		{
			NodeLayoutContextInfo layoutInfo=new NodeLayoutContextInfo();
			layoutInfo.ClientRectangle=m_ClientArea;
			layoutInfo.DefaultColumns=this.GetDefaultColumnInfo();
			layoutInfo.ChildColumns=null;
			layoutInfo.Left=0;
			layoutInfo.Top=0; // TODO: Include Columns if visible into this...
			layoutInfo.DefaultFont=m_Tree.Font;
			layoutInfo.LeftToRight=(this.LeftRight==System.Windows.Forms.LeftRightAlignment.Left);
			layoutInfo.Graphics=graphics;
			layoutInfo.Styles=m_Tree.Styles;
			if(m_Tree.CellLayout!=eCellLayout.Default)
				layoutInfo.CellLayout=m_Tree.CellLayout;
			if(m_Tree.CellPartLayout!=eCellPartLayout.Default)
				layoutInfo.CellPartLayout=m_Tree.CellPartLayout;
				
			if(m_Tree.NodeStyle!=null)
				layoutInfo.DefaultNodeStyle=m_Tree.NodeStyle;

			if(m_Tree.CellStyleDefault!=null)
				layoutInfo.DefaultCellStyle=m_Tree.CellStyleDefault;
			else
				layoutInfo.DefaultCellStyle=ElementStyle.GetDefaultCellStyle(layoutInfo.DefaultNodeStyle);

			// Determine size of the default Column Header
			if(m_Tree.ColumnStyleNormal!=null)
			{
				ElementStyleLayout.CalculateStyleSize(m_Tree.ColumnStyleNormal,layoutInfo.DefaultFont);
				layoutInfo.DefaultHeaderSize=m_Tree.ColumnStyleNormal.Size;
			}

			if(layoutInfo.DefaultHeaderSize.IsEmpty)
				layoutInfo.DefaultHeaderSize.Height=layoutInfo.DefaultFont.Height+4;

			return layoutInfo;
		}

		protected Node[] GetTopLevelNodes()
		{
			if(m_Tree.DisplayRootNode!=null)
				return new Node[] {m_Tree.DisplayRootNode};
			else
			{
				Node[] nodes=new Node[m_Tree.Nodes.Count];
				m_Tree.Nodes.CopyTo(nodes);
				return nodes;
			}
		}

		protected bool IsRootNode(Node node)
		{
			return NodeOperations.IsRootNode(m_Tree,node);
		}

		protected virtual void EmptyBoundsUnusedNodes(Node[] topLevelNodes)
		{
			if(m_Tree.DisplayRootNode!=null)
			{
				Node node=m_Tree.DisplayRootNode.PrevVisibleNode;
				while(node!=null)
				{
					node.SetBounds(Rectangle.Empty);
					node=node.PrevVisibleNode;
				}
				node=m_Tree.DisplayRootNode.NextNode;
				if(node==null)
				{
					node=m_Tree.DisplayRootNode.Parent;
					while(node!=null)
					{
						if(node.NextNode!=null)
						{
							node=node.NextNode;
							break;
						}
						else
							node=node.Parent;
					}
				}
				while(node!=null)
				{
					node.SetBounds(Rectangle.Empty);
					node=node.NextVisibleNode;
				}
			}
			else
			{
				for(int i=1;i<topLevelNodes.Length;i++)
				{
					topLevelNodes[i].SetBounds(Rectangle.Empty);
				}
			}
		}

		#region Column Support
		// Assumes that layoutInfo is up-to-date and that Node that is connected with
		// columns is already processed and it's size and location calculated.
		// layoutInfo.Top member reflects the next position below the node
		// layoutInfo.LevelOffset should reflect the X offset for the child nodes.
		public void LayoutColumnHeader(NodeLayoutContextInfo layoutInfo)
		{
			Node node=layoutInfo.ContextNode;

			if(node.NodesColumns.Count==0)
			{
				node.ColumnHeaderHeight=0;
				return;
			}
			
			int x=layoutInfo.Left;
			int y=layoutInfo.ContextNode.BoundsRelative.Bottom;

			bool bLeftNode=(layoutInfo.MapPositionNear && layoutInfo.LeftToRight);
			int expandPartWidth=this.ExpandAreaWidth;
			int cellPartSpacing=GetCellLayout().CellPartSpacing;

			if(!bLeftNode)
				x+=(expandPartWidth+cellPartSpacing);

			int clientWidth=layoutInfo.ClientRectangle.Width-(layoutInfo.Left+expandPartWidth);
			if(clientWidth<=0)
				clientWidth=layoutInfo.ClientRectangle.Width;
			
			node.ColumnHeaderHeight=Layout.ColumnHeaderLayout.LayoutColumnHeader(layoutInfo,x,y,clientWidth,this.GetCellLayout().CellHorizontalSpacing);
		}
		#endregion
		
		#region RTL Support
		public virtual System.Windows.Forms.LeftRightAlignment LeftRight
		{
			get {return m_LeftRight;}
			set {m_LeftRight=value;}
		}
		#endregion
	}
}
