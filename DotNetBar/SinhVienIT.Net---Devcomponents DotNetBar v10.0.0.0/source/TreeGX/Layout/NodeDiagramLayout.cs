using System;
using System.Collections;
using System.Drawing;

namespace DevComponents.Tree.Layout
{
	/// <summary>
	/// Represents the class that performs node diagram layout.
	/// </summary>
	internal class NodeDiagramLayout:NodeLayout
	{
		private eDiagramFlow m_DiagramFlow=eDiagramFlow.LeftToRight;

		public NodeDiagramLayout(TreeGX treeControl, Rectangle clientArea):base(treeControl,clientArea)
		{
		}

		public override void PerformLayout()
		{
			this.PrepareStyles();

			// Get default Columns
			ArrayList defaultColInfoList=this.GetDefaultColumnInfo();

			if(m_Tree.Nodes.Count==0)
				return;

			System.Drawing.Graphics graphics=this.GetGraphics();
			try
			{
				// Loop through each top-level node
				Node[] topLevelNodes=this.GetTopLevelNodes();
				NodeLayoutContextInfo layoutInfo=this.GetDefaultNodeLayoutContextInfo(graphics);

				foreach(Node childNode in topLevelNodes)
				{
					if(!childNode.Visible)
						continue;
					layoutInfo.ContextNode=childNode;
					ProcessRootNode(layoutInfo);
					break;
				}
				EmptyBoundsUnusedNodes(topLevelNodes);
			}
			finally
			{
				if(this.DisposeGraphics)
					graphics.Dispose();
			}
		}

		private void ProcessRootNode(NodeLayoutContextInfo layoutInfo)
		{
			if(m_DiagramFlow==eDiagramFlow.RightToLeft || m_DiagramFlow==eDiagramFlow.BottomToTop)
				layoutInfo.MapPositionNear=true;
			ProcessSubNode(layoutInfo);
			PositionRootNode(layoutInfo);
		}

		private void PositionRootNode(NodeLayoutContextInfo layoutInfo)
		{
			Point location=Point.Empty;
			Node root=layoutInfo.ContextNode;
			switch(m_DiagramFlow)
			{
				case eDiagramFlow.LeftToRight:
				{
					location.Y=(root.ChildNodesBounds.Height-root.BoundsRelative.Height)/2;
					break;
				}
				case eDiagramFlow.RightToLeft:
				{
					location.X=root.ChildNodesBounds.Width+this.NodeHorizontalSpacing;
					location.Y=(root.ChildNodesBounds.Height-root.BoundsRelative.Height)/2;
					break;
				}
				case eDiagramFlow.TopToBottom:
				{
					location.X=0; //(root.ChildNodesBounds.Width-root.Bounds.Width)/2;
					break;
				}
				case eDiagramFlow.BottomToTop:
				{
					location.X=root.ChildNodesBounds.Width; //-root.Bounds.Width; //-root.Bounds.Width)/2;
					location.Y=root.ChildNodesBounds.Height+this.NodeVerticalSpacing; //-root.Bounds.Height; //+this.NodeVerticalSpacing;
					break;
				}
			}
			OffsetNodeLocation(root,location.X,location.Y);

			Rectangle area=Rectangle.Empty;
			area=Rectangle.Union(area,root.BoundsRelative);

			PositionSubNodes(root);
			
			area=Rectangle.Union(area,root.ChildNodesBounds);
			root.ChildNodesBounds=area;
			m_Width=area.Width;
			m_Height=area.Height;
		}
//		private int GetMaxChildNodeWidth(Node parent)
//		{
//			int width=0;
//			foreach(Node node in parent.Nodes)
//			{
//				if(node.Bounds.Width>width)
//					width=node.Bounds.Width;
//			}
//			return width;
//		}

		private void PositionSubNodes(Node parentNode)
		{
			if(parentNode.Nodes.Count==0)
				return;

			Rectangle rChildBounds=Rectangle.Empty;
			bool bFirst=true;
			bool rootNode=IsRootNode(parentNode);

			if(m_DiagramFlow==eDiagramFlow.LeftToRight || !rootNode && m_DiagramFlow==eDiagramFlow.TopToBottom)
			{
				int y=parentNode.BoundsRelative.Y-(parentNode.ChildNodesBounds.Height-parentNode.BoundsRelative.Height)/2;
				int x=0;
//				if(parentNode.Parent!=null)
//					x=parentNode.Bounds.X+GetMaxChildNodeWidth(parentNode.Parent)+this.NodeHorizontalSpacing;
//				else
					x=parentNode.BoundsRelative.Right+this.NodeHorizontalSpacing;

				foreach(Node node in parentNode.Nodes)
				{
					if(!node.Visible)
						continue;

					int top=y;
					bool anyVisibleNodes=node.AnyVisibleNodes;
					if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && anyVisibleNodes)
						top+=(node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2;
					OffsetNodeLocation(node,x,top);
					if(node.Expanded && anyVisibleNodes)
					{
						PositionSubNodes(node);
						y+=(Math.Max(node.BoundsRelative.Height,node.ChildNodesBounds.Height)+this.NodeVerticalSpacing);
					}
					else y+=(node.BoundsRelative.Height+this.NodeVerticalSpacing);
					if(bFirst)
					{
						rChildBounds=node.BoundsRelative;
						bFirst=false;
					}
					else
						rChildBounds=Rectangle.Union(rChildBounds,node.BoundsRelative);
					if(!node.ChildNodesBounds.IsEmpty)
						rChildBounds=Rectangle.Union(rChildBounds,node.ChildNodesBounds);
				}
			}
			else if(m_DiagramFlow==eDiagramFlow.RightToLeft || !rootNode && m_DiagramFlow==eDiagramFlow.BottomToTop)
			{
				int y=parentNode.BoundsRelative.Y-(parentNode.ChildNodesBounds.Height-parentNode.BoundsRelative.Height)/2;
				int x=parentNode.BoundsRelative.X-this.NodeHorizontalSpacing;

				foreach(Node node in parentNode.Nodes)
				{
					if(!node.Visible)
						continue;

					int left=x-node.BoundsRelative.Width;
					int top=y;
					bool anyVisibleNodes=node.AnyVisibleNodes;
					if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && anyVisibleNodes)
						top+=(node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2;
					OffsetNodeLocation(node,left,top);
					if(node.Expanded && anyVisibleNodes)
					{
						PositionSubNodes(node);
						y+=(Math.Max(node.BoundsRelative.Height,node.ChildNodesBounds.Height)+this.NodeVerticalSpacing);
					}
					else y+=(node.BoundsRelative.Height+this.NodeVerticalSpacing);
					if(bFirst)
					{
						rChildBounds=node.BoundsRelative;
						bFirst=false;
					}
					else
						rChildBounds=Rectangle.Union(rChildBounds,node.BoundsRelative);
					if(!node.ChildNodesBounds.IsEmpty)
						rChildBounds=Rectangle.Union(rChildBounds,node.ChildNodesBounds);
				}
			}
			else if(m_DiagramFlow==eDiagramFlow.TopToBottom)
			{
				int y=parentNode.BoundsRelative.Bottom+this.NodeVerticalSpacing;
				
				foreach(Node node in parentNode.Nodes)
				{
					if(!node.Visible)
						continue;

					int left=parentNode.BoundsRelative.Right+this.NodeHorizontalSpacing;
					int top=y;
					bool anyVisibleNodes=node.AnyVisibleNodes;
					if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && anyVisibleNodes)
						top+=(node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2;
					OffsetNodeLocation(node,left,top);
					if(node.Expanded && anyVisibleNodes)
					{
						PositionSubNodes(node);
						y+=(Math.Max(node.BoundsRelative.Height,node.ChildNodesBounds.Height)+this.NodeVerticalSpacing);
					}
					else y+=(node.BoundsRelative.Height+this.NodeVerticalSpacing);
					if(bFirst)
					{
						rChildBounds=node.BoundsRelative;
						bFirst=false;
					}
					else
						rChildBounds=Rectangle.Union(rChildBounds,node.BoundsRelative);
					if(!node.ChildNodesBounds.IsEmpty)
						rChildBounds=Rectangle.Union(rChildBounds,node.ChildNodesBounds);
				}
			}
			else if(m_DiagramFlow==eDiagramFlow.BottomToTop)
			{
				int y=parentNode.BoundsRelative.Top-this.NodeVerticalSpacing;
				
				foreach(Node node in parentNode.Nodes)
				{
					if(!node.Visible)
						continue;

					int left=parentNode.BoundsRelative.Left-this.NodeHorizontalSpacing-node.BoundsRelative.Width;
					int top=y-node.BoundsRelative.Height;
					bool anyVisibleNodes=node.AnyVisibleNodes;
					if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && anyVisibleNodes)
						top-=(node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2;
					OffsetNodeLocation(node,left,top);
					if(node.Expanded && anyVisibleNodes)
					{
						PositionSubNodes(node);
						y-=(Math.Max(node.BoundsRelative.Height,node.ChildNodesBounds.Height)+this.NodeVerticalSpacing);
					}
					else y-=(node.BoundsRelative.Height+this.NodeVerticalSpacing);
					if(bFirst)
					{
						rChildBounds=node.BoundsRelative;
						bFirst=false;
					}
					else
						rChildBounds=Rectangle.Union(rChildBounds,node.BoundsRelative);
					if(!node.ChildNodesBounds.IsEmpty)
						rChildBounds=Rectangle.Union(rChildBounds,node.ChildNodesBounds);
				}
			}
			parentNode.ChildNodesBounds=rChildBounds;
		}

		private void ProcessSubNode(NodeLayoutContextInfo layoutInfo)
		{
			Node node=layoutInfo.ContextNode;
			bool bHorizontalFlow=true;//(m_DiagramFlow==eDiagramFlow.LeftToRight || m_DiagramFlow==eDiagramFlow.RightToLeft);
			if(node.SizeChanged)
			{
				// Calculate size of the node itself...
				LayoutNode(layoutInfo);
			}
			else
				node.SetBounds(new Rectangle(Point.Empty,node.BoundsRelative.Size));
			
			if(node.Expanded)
			{
				ArrayList parentColumns=layoutInfo.ChildColumns;
				ArrayList childColumns=GetNodeColumnInfo(node);

				Rectangle childNodesBounds=Rectangle.Empty;
				foreach(Node childNode in node.Nodes)
				{
					if(!childNode.Visible)
						continue;
					layoutInfo.ContextNode=childNode;
				
					layoutInfo.ChildColumns=childColumns;
					ProcessSubNode(layoutInfo);

					if(bHorizontalFlow)
					{
						childNodesBounds.Height+=(Math.Max(childNode.BoundsRelative.Height,childNode.ChildNodesBounds.Height)+this.NodeVerticalSpacing);
						childNodesBounds.Width=Math.Max(childNodesBounds.Width,childNode.BoundsRelative.Width+(childNode.ChildNodesBounds.Width>0?this.NodeHorizontalSpacing+childNode.ChildNodesBounds.Width:0));
					}
					else
					{
						childNodesBounds.Width+=(Math.Max(childNode.BoundsRelative.Width,childNode.ChildNodesBounds.Width)+this.NodeHorizontalSpacing);
						childNodesBounds.Height=Math.Max(childNodesBounds.Height,childNode.BoundsRelative.Height+(childNode.ChildNodesBounds.Height>0?this.NodeVerticalSpacing+childNode.ChildNodesBounds.Height:0));
					}
				}

				layoutInfo.ChildColumns=parentColumns;

				if(bHorizontalFlow)
				{
					if(childNodesBounds.Height>0)
						childNodesBounds.Height-=this.NodeVerticalSpacing;
				}
				else
				{
					if(childNodesBounds.Width>0)
						childNodesBounds.Width-=this.NodeHorizontalSpacing;
				}
				node.ChildNodesBounds=childNodesBounds;
				layoutInfo.ChildColumns=null;
				layoutInfo.ContextNode=node;
			}
			else node.ChildNodesBounds=Rectangle.Empty;
		}

		/// <summary>
		/// Returns true if root node should have expanded part
		/// </summary>
		protected override bool RootHasExpandedPart
		{
			get {return false;}
		}

		/// <summary>
		/// Indicates the layout flow for the nodes.
		/// </summary>
		public eDiagramFlow DiagramFlow
		{
			get {return m_DiagramFlow;}
			set
			{
				m_DiagramFlow=value;
			}
		}
	}
}
