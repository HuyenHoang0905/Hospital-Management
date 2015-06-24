using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Tree
{
	/// <summary>
	/// Summary description for NodeTreeLayout.
	/// </summary>
	internal class NodeTreeLayout:NodeLayout
	{
		//private bool m_LayoutPerformed=false;

		public NodeTreeLayout(TreeGX treeControl, Rectangle clientArea):base(treeControl,clientArea)
		{
			//m_ExpandAreaWidth=16;
			m_ExpandPartSize=new Size(9,9);
		}

		public override void PerformLayout()
		{
			this.PrepareStyles();

			// Get default Columns
			ArrayList defaultColInfoList=this.GetDefaultColumnInfo();

			Graphics graphics=this.GetGraphics();
			try
			{
				// Loop through each top-level node
				Node[] topLevelNodes=this.GetTopLevelNodes();
				NodeLayoutContextInfo layoutInfo=GetDefaultNodeLayoutContextInfo(GetGraphics());

				foreach(Node childNode in topLevelNodes)
				{
					layoutInfo.ContextNode=childNode;
					ProcessNode(layoutInfo);
				}
			}
			finally
			{
				graphics.Dispose();
			}
		}

		#region Node routines
		private void ProcessNode(NodeLayoutContextInfo layoutInfo)
		{
			Node node=layoutInfo.ContextNode;
			if(node.SizeChanged)
			{
				// Calculate size of the node itself...
				LayoutNode(layoutInfo);

				// Calculate size and location of node column header if any
//				if(node.NodesColumnHeaderVisible)
//				{
//					layoutInfo.Left+=this.NodeLevelOffset;
//					LayoutColumnHeader(layoutInfo);
//					layoutInfo.Left-=this.NodeLevelOffset;
//				}
			}
			else if(node.Bounds.Top!=layoutInfo.Top)
			{
				// Adjust top position
				node.SetBounds(new Rectangle(node.Bounds.X,layoutInfo.Top,node.Bounds.Width,node.Bounds.Height));
				foreach(Cell c in node.Cells)
					c.SetBounds(new Rectangle(c.Bounds.X,layoutInfo.Top,c.Bounds.Width,c.Bounds.Height));
			}

			// Need to set the Top position properly
			layoutInfo.Top+=(node.Bounds.Height+this.NodeVerticalSpacing);
//			if(node.Expanded && node.NodesColumnHeaderVisible && node.NodesColumns.Count>0)
//				layoutInfo.Top+=node.ColumnHeaderHeight;
			
			if(node.Expanded)
			{
				int originalLevelOffset=layoutInfo.Left;
				layoutInfo.Left+=this.NodeLevelOffset;
				
				ArrayList parentColumns=layoutInfo.ChildColumns;
				ArrayList childColumns=GetNodeColumnInfo(node);

				foreach(Node childNode in node.Nodes)
				{
					layoutInfo.ContextNode=childNode;
					layoutInfo.ChildColumns=childColumns;
					ProcessNode(layoutInfo);
				}
				layoutInfo.ChildColumns=parentColumns;

				layoutInfo.ContextNode=node;
				layoutInfo.Left=originalLevelOffset;
			}
		}

//		private void LayoutNode(NodeLayoutContextInfo layoutInfo)
//		{
//			if(!layoutInfo.ContextNode.SizeChanged)
//				return;
//
//			Node node=layoutInfo.ContextNode;
//			Rectangle nodeContentRect=Rectangle.Empty; // Node content rect excludes expand rect
//			
//			Rectangle nodeRect=Rectangle.Empty;
//			nodeRect.X=layoutInfo.Left;
//			nodeRect.Y=layoutInfo.Top;
//
//			int height=0, width=0;
//
//            bool bExpandPartAlignedNear=this.ExpandPartAlignedNear(node);
//			if(bExpandPartAlignedNear)
//			{
//				width+=(this.ExpandAreaWidth+this.GetCellLayout().CellPartSpacing);
//			}
//
//			int x=width;		// relative to 0,0 of the node
//			int y=0;			// Relative to 0,0 of the node
//			
//			// Apply node style
//			ElementStyle nodeStyle=null;
//			if(node.Expanded && node.StyleExpanded!=null)
//				nodeStyle=node.StyleExpanded;
//			else if(node.Style!=null)
//				nodeStyle=node.Style;
//			else
//				nodeStyle=layoutInfo.DefaultNodeStyle;
//
//			nodeContentRect.X=x;
//
//			if(nodeStyle!=null)
//			{
//				x+=ElementStyleLayout.LeftWhiteSpace(nodeStyle); // nodeStyle.MarginLeft+nodeStyle.PaddingLeft;
//				y+=ElementStyleLayout.TopWhiteSpace(nodeStyle); // nodeStyle.MarginTop+nodeStyle.PaddingTop;
//				nodeContentRect.X+=nodeStyle.MarginLeft;
//				nodeContentRect.Y+=nodeStyle.MarginTop;
//			}
//
//			Size size=this.GetCellLayout().LayoutCells(layoutInfo,x,y);
//			height=size.Height;
//			width+=size.Width;
//
//			nodeContentRect.Width=size.Width;
//			nodeContentRect.Height=size.Height;
//
//			node.SetCellsBounds(new Rectangle(x,y,size.Width,size.Height));
//
//			if(nodeStyle!=null)
//			{
//				nodeContentRect.Width+=(ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Left)+
//					ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Right));
//				nodeContentRect.Height+=(ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Top)+
//					ElementStyleLayout.StyleSpacing(nodeStyle,eSpacePart.Padding | eSpacePart.Border,eStyleSide.Bottom));
//
//				width+=(ElementStyleLayout.HorizontalStyleWhiteSpace(nodeStyle));
//				height+=(ElementStyleLayout.VerticalStyleWhiteSpace(nodeStyle));
//			}
//
//			if(!bExpandPartAlignedNear)
//				width+=this.ExpandAreaWidth;
//			
//			nodeRect.Height=height;
//			nodeRect.Width=width;
//
//			node.SetBounds(nodeRect);
//			node.SetContentBounds(nodeContentRect);
//
//			LayoutExpandPart(layoutInfo,true);
//
//			node.SizeChanged=false;
//		}

		/// <summary>
		/// Returns true if expand part space should be accounted for even if they expand part is not visible or need to be displayed. Default value is false.
		/// </summary>
		protected override bool ReserveExpandPartSpace
		{
			get
			{
				return true;
			}
		}
//
//		private CellLayout m_CellLayout=null;
//		private CellLayout GetCellLayout()
//		{
//			if(m_CellLayout==null)
//				m_CellLayout=new CellLayout();
//			return m_CellLayout;
//		}

//		/// <summary>
//		/// Determines the rectangle of the +/- part of the tree node that is used to expand node.
//		/// </summary>
//		/// <param name="layoutInfo">Node layout context information</param>
//		private void LayoutExpandPart(NodeLayoutContextInfo layoutInfo)
//		{
//			Node node=layoutInfo.ContextNode;
//
//			Size partSize=new Size(9,9);
//
//			// For different styles of the nodes the expand part +/- might be aligned to the right of the node etc.
//			// this routine does default left position
//			Rectangle bounds=new Rectangle(0,0,partSize.Width,partSize.Height);
//			bounds.Y=(node.Bounds.Height-bounds.Height)/2;
//			bounds.X=(this.ExpandAreaWidth-bounds.Width)/2;
//
//			node.SetExpandPartRectangle(bounds);
//		}

		/// <summary>
		/// Gets whether the expand part of the node +/- is aligned to the left of the node in left-to-right layout.
		/// </summary>
		/// <param name="node">Node to get expand part alignment for</param>
		/// <returns>true if node expand part is aligned to the left in left-to-right layout.</returns>
		private bool ExpandPartAlignedNear(Node node)
		{
			return true; // If changed LayoutExpandPart needs to be updated as well
		}

//		private NodeCollection GetTopLevelNodes()
//		{
//			return m_Tree.Nodes;
//		}

		private int NodeLevelOffset
		{
			get {return 10;}
		}

		#endregion

		
	}
}
