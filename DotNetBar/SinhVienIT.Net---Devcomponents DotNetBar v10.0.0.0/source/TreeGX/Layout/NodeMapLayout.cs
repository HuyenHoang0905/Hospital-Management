using System;
using System.Drawing;
using System.Collections;

namespace DevComponents.Tree.Layout
{
	/// <summary>
	/// Represents the class that performs node map layout.
	/// </summary>
	internal class NodeMapLayout:NodeLayout
	{
		#region Private Variables
		private eMapFlow m_MapFlow=eMapFlow.Spread;
		#endregion

		public NodeMapLayout(TreeGX treeControl, Rectangle clientArea):base(treeControl,clientArea)
		{
		}

		public override void PerformLayout()
		{
			this.PrepareStyles();
			// Get default Columns
			//ArrayList defaultColInfoList=this.GetDefaultColumnInfo();

			if(m_Tree.Nodes.Count==0)
				return;

			Graphics graphics=this.GetGraphics();
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
			Node node=layoutInfo.ContextNode;

			// Center root node on the screen
			//if(node.SizeChanged)
			//{
				// Calculate size of the node itself...
				LayoutNode(layoutInfo);

				// Calculate size and location of node column header if any
				//				if(node.NodesColumnHeaderVisible)
				//				{
				//					LayoutColumnHeader(layoutInfo);
				//				}
			//}
			
			
			//			if(node.Expanded && node.NodesColumnHeaderVisible && node.NodesColumns.Count>0)
			//				layoutInfo.Top+=node.ColumnHeaderHeight;
			NodesMapPosition positions=NodesMapPosition.Empty;
			bool bNear=false;
			if(node.Expanded)
			{
				if(node.NodesColumns.Count>0)
					layoutInfo.ChildColumns=GetNodeColumnInfo(node);
				foreach(Node childNode in node.Nodes)
				{
					if(!childNode.Visible)
						continue;

					layoutInfo.ContextNode=childNode;
					
					if(bNear || childNode.MapSubRootPosition==eMapPosition.Near)
					{
						layoutInfo.MapPositionNear=true;
						ProcessSubNode(layoutInfo);

						positions.Near.Add(childNode);
						positions.NearHeight+=GetTotalChildNodeHeight(childNode);
						bNear=true;
					}
					else if(childNode.MapSubRootPosition==eMapPosition.Far)
					{
						layoutInfo.MapPositionNear=false;
						ProcessSubNode(layoutInfo);

						positions.Far.Add(childNode);
						positions.FarHeight+=GetTotalChildNodeHeight(childNode);
					}
					else
						positions.Default.Add(childNode);
				}

				// Assign nodes to appropriate sides....
				if(positions.Default.Count>0)
				{
					int toFar=0;
					if(positions.Near.Count==0 && positions.Far.Count==0 && positions.Default.Count<=4)
						toFar=2;
					else
						toFar=(int)Math.Ceiling((double)((positions.Near.Count-positions.Far.Count+positions.Default.Count))/2);
					for(int i=0;i<toFar;i++)
					{
						layoutInfo.ContextNode=positions.Default[0] as Node;
						if(m_MapFlow==eMapFlow.RightToLeft)
							layoutInfo.MapPositionNear=true;
						else
							layoutInfo.MapPositionNear=false;
						ProcessSubNode(layoutInfo);

						positions.Far.Add(positions.Default[0]);
						positions.FarHeight+=GetTotalChildNodeHeight(positions.Default[0] as Node);
						positions.Default.RemoveAt(0);
						if(positions.Default.Count==0)
							break;
					}
					for(int i=0;i<positions.Default.Count;i++)
					{
						layoutInfo.ContextNode=positions.Default[i] as Node;
						if(m_MapFlow==eMapFlow.LeftToRight)
							layoutInfo.MapPositionNear=false;
						else
							layoutInfo.MapPositionNear=true;
						ProcessSubNode(layoutInfo);

						positions.Near.Add(positions.Default[i]);
						positions.NearHeight+=GetTotalChildNodeHeight(positions.Default[i] as Node); //+this.NodeVerticalSpacing;
					}
					positions.Default.Clear();
				}

				layoutInfo.ChildColumns=null;
				layoutInfo.ContextNode=node;
			}

			// Set the position of the root node
            Point pRoot = new Point(0, 0);
			pRoot.Offset(node.Offset,0);

			// Adjust top position
			node.SetBounds(new Rectangle(pRoot,node.BoundsRelative.Size));
			node.ExpandPartRectangleRelative.Offset(pRoot);
			foreach(Cell c in node.Cells)
				c.BoundsRelative.Offset(pRoot);

			// Set the position of the child nodes
			if(node.Expanded)
			{
				PositionCenterRootSubNodes(node, positions);
			}
			else
			{
				node.ChildNodesBounds = Rectangle.Empty;
				m_Width = node.Bounds.Width;
				m_Height = node.Bounds.Height;
			}
		}

		private int GetTotalChildNodeHeight(Node childNode)
		{
			if(childNode.Expanded)
				return Math.Max(childNode.BoundsRelative.Height,childNode.ChildNodesBounds.Height);
			return childNode.BoundsRelative.Height;
		}

		private void ProcessSubNode(NodeLayoutContextInfo layoutInfo)
		{
			Node node=layoutInfo.ContextNode;

			//if(node.SizeChanged)
			//{
				// Calculate size of the node itself...
				LayoutNode(layoutInfo);
			//}
			//else
			//	node.SetBounds(new Rectangle(Point.Empty,node.Bounds.Size));
			
			if(node.Expanded && node.AnyVisibleNodes)
			{
				ArrayList parentColumns=layoutInfo.ChildColumns;
				ArrayList childColumns=GetNodeColumnInfo(node);

				if(node.NodesColumns.Count>0)
					layoutInfo.ChildColumns=GetNodeColumnInfo(node);
				Rectangle childNodesBounds=Rectangle.Empty;
				foreach(Node childNode in node.Nodes)
				{
					if(!childNode.Visible)
						continue;

					layoutInfo.ContextNode=childNode;
					layoutInfo.ChildColumns=childColumns;
					ProcessSubNode(layoutInfo);

					childNodesBounds.Height+=(Math.Max(childNode.BoundsRelative.Height,childNode.ChildNodesBounds.Height)+this.NodeVerticalSpacing);
					childNodesBounds.Width=Math.Max(childNodesBounds.Width,childNode.BoundsRelative.Width+(childNode.ChildNodesBounds.Width>0?this.NodeHorizontalSpacing+childNode.ChildNodesBounds.Width:0));
				}
				
				layoutInfo.ChildColumns=parentColumns;

				if(childNodesBounds.Height>0)
					childNodesBounds.Height-=this.NodeVerticalSpacing;
				node.ChildNodesBounds=childNodesBounds;
				layoutInfo.ChildColumns=null;
				layoutInfo.ContextNode=node;
			}
			else node.ChildNodesBounds=Rectangle.Empty;
		}

		private void PositionCenterRootSubNodes(Node root, NodesMapPosition positions)
		{
			Rectangle area=Rectangle.Empty;
			area=Rectangle.Union(area,root.BoundsRelative);
			
			// Far Nodes Layout
			//int y=0, x=0;
			int areaHeight=positions.FarHeight;
			int minAreaHeight=this.GetMinimumAreaHeight(positions.Far,root.BoundsRelative.Height);
			if(areaHeight<minAreaHeight)
				areaHeight=minAreaHeight;

			int areaLeft=root.BoundsRelative.Right-root.BoundsRelative.Width/3;
			int areaTop=0;
			if(m_MapFlow==eMapFlow.TopToBottom)
				areaTop=root.BoundsRelative.Bottom+this.NodeVerticalSpacing; // Top Bottom Orientation
			else if(m_MapFlow==eMapFlow.BottomToTop)
				areaTop=root.BoundsRelative.Y-(areaHeight+this.NodeVerticalSpacing*((positions.Far.Count==1?1:positions.Far.Count-1))); //Bottom Top Orientation
			else if(m_MapFlow==eMapFlow.LeftToRight || m_MapFlow==eMapFlow.RightToLeft)
				areaTop=root.BoundsRelative.Y-(areaHeight+this.NodeVerticalSpacing*((positions.Far.Count==1?1:positions.Far.Count-1)));
			else // Spread
				areaTop=root.BoundsRelative.Y+root.BoundsRelative.Height/2-(areaHeight+this.NodeVerticalSpacing*((positions.Far.Count==1?1:positions.Far.Count-1)))/2; // Around spread orientation

			if(m_MapFlow==eMapFlow.RightToLeft)
			{
				int areaRightMost=root.BoundsRelative.X+root.BoundsRelative.Width/3;
				PositionNearSubRootNodes(root,positions.Far,areaRightMost,areaTop,areaHeight,ref area);
			}
			else
			{
				PositionFarSubRootNodes(root,positions.Far,areaLeft,areaTop,ref area);
			}
			
			if(m_MapFlow==eMapFlow.LeftToRight)
			{
				areaTop=root.BoundsRelative.Bottom+this.NodeVerticalSpacing;
				PositionFarSubRootNodes(root,positions.Near,areaLeft,areaTop,ref area);
			}
			else
			{
				// Near nodes layout
				areaHeight=positions.NearHeight;
				minAreaHeight=this.GetMinimumAreaHeight(positions.Near,root.BoundsRelative.Height);
				if(areaHeight<minAreaHeight)
					areaHeight=minAreaHeight;
				int areaRightMost=root.BoundsRelative.X+root.BoundsRelative.Width/3;
				
				if(m_MapFlow==eMapFlow.TopToBottom)
					areaTop=root.BoundsRelative.Bottom+this.NodeVerticalSpacing; // Top Bottom Orientation
				else if(m_MapFlow==eMapFlow.BottomToTop)
					areaTop=root.BoundsRelative.Y-(areaHeight+this.NodeVerticalSpacing*((positions.Near.Count==1?1:positions.Near.Count-1))); //Bottom Top Orientation
				else if(m_MapFlow==eMapFlow.LeftToRight || m_MapFlow==eMapFlow.RightToLeft)
					areaTop=root.BoundsRelative.Bottom+this.NodeVerticalSpacing; // Top Bottom Orientation
				else // Spread
					areaTop=root.BoundsRelative.Y+root.BoundsRelative.Height/2-(areaHeight+this.NodeVerticalSpacing*((positions.Near.Count==1?1:positions.Near.Count-1)))/2; // Around spread orientation

				PositionNearSubRootNodes(root,positions.Near,areaRightMost,areaTop,areaHeight,ref area);
			}

			m_Width=area.Width;
			m_Height=area.Height;
			root.ChildNodesBounds=area;
		}

		private void PositionFarSubRootNodes(Node root, ArrayList farNodes, int areaLeft, int areaTop, ref Rectangle usedArea)
		{
			int top=areaTop;
			bool twoNodesLayout = (farNodes.Count == 2);
			for(int index=0;index<farNodes.Count;index++)
			{
				Node node=farNodes[index] as Node;
				int x=areaLeft+this.NodeHorizontalSpacing;
				int y=top;
				if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && node.AnyVisibleNodes)
					y+=(node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2;

				if(root.BoundsRelative.IntersectsWith(new Rectangle(root.BoundsRelative.X,y,node.BoundsRelative.Width,node.BoundsRelative.Height)))
				{
					if(twoNodesLayout && index==1)
					{
						y = root.BoundsRelative.Bottom + this.NodeVerticalSpacing/2;
						if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && node.AnyVisibleNodes)
							y+=(node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2;
					}
					else
					{
						// Change X position since Y is already accounted for...
						x+=(root.BoundsRelative.Right-x+this.NodeHorizontalSpacing);
					}
				}
				x+=node.Offset;

				OffsetNodeLocation(node,x,y);
				if(node.Expanded && node.AnyVisibleNodes)
				{
					PositionSubNodes(node,true);
					usedArea=Rectangle.Union(usedArea,node.ChildNodesBounds);
				}
				else
					usedArea=Rectangle.Union(usedArea,node.BoundsRelative);

				if(node.Expanded && node.AnyVisibleNodes)
				{
					top+=(Math.Max(node.ChildNodesBounds.Height,node.BoundsRelative.Height)+this.NodeVerticalSpacing);
				}
				else
				{
					top+=(node.BoundsRelative.Height+this.NodeVerticalSpacing);
				}
			}
		}

		private void PositionNearSubRootNodes(Node root, ArrayList nearNodes, int areaRightMost, int areaTop, int areaHeight, ref Rectangle usedArea)
		{
			int bottom=areaTop+(areaHeight+this.NodeVerticalSpacing*(nearNodes.Count-1));
			bool twoNodesLayout = (nearNodes.Count == 2);
			
			for(int index=0;index<nearNodes.Count;index++)
			{
				Node node=nearNodes[index] as Node;
				int x=areaRightMost-node.BoundsRelative.Width-this.NodeHorizontalSpacing;
				int y=0;
				
				if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && node.AnyVisibleNodes)
					y=bottom-((node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2+node.BoundsRelative.Height);
				else
					y=bottom-node.BoundsRelative.Height;
					
//				Rectangle rNodeArea=new Rectangle(x,y,node.Bounds.Width,node.Bounds.Height);
//				rNodeArea.Inflate(this.NodeHorizontalSpacing,this.NodeVerticalSpacing);
				//if(root.Bounds.IntersectsWith(rNodeArea))
				if(root.BoundsRelative.IntersectsWith(new Rectangle(root.BoundsRelative.X,y,node.BoundsRelative.Width,node.BoundsRelative.Height)))
				{
					if(twoNodesLayout && index==1)
					{
						y = root.BoundsRelative.Top - this.NodeVerticalSpacing/2;
						if(node.ChildNodesBounds.Height>node.BoundsRelative.Height && node.Expanded && node.AnyVisibleNodes)
							y-=((node.ChildNodesBounds.Height-node.BoundsRelative.Height)/2+node.BoundsRelative.Height);
						else
							y-=node.BoundsRelative.Height;
					}
					else
					{
						// Change X position since Y is already accounted for...
						x-=(x+node.BoundsRelative.Width-root.BoundsRelative.X+this.NodeHorizontalSpacing);
					}
				}
				x-=node.Offset;
				OffsetNodeLocation(node,x,y);
				if(node.Expanded && node.AnyVisibleNodes)
				{
					PositionSubNodes(node,false);
                    usedArea = Rectangle.Union(usedArea, node.BoundsRelative);
					usedArea=Rectangle.Union(usedArea,node.ChildNodesBounds);
				}
				else
					usedArea=Rectangle.Union(usedArea,node.BoundsRelative);
				
				if(node.Expanded && node.AnyVisibleNodes)
				{
					bottom-=(Math.Max(node.ChildNodesBounds.Height,node.BoundsRelative.Height)+this.NodeVerticalSpacing);
				}
				else
				{
					bottom-=(node.BoundsRelative.Height+this.NodeVerticalSpacing);
				}
			}
		}

		private void PositionSubNodes(Node parentNode,bool bFar)
		{
			if(parentNode.Nodes.Count==0)
				return;

			int y=parentNode.BoundsRelative.Y-(parentNode.ChildNodesBounds.Height-parentNode.BoundsRelative.Height)/2;
			int x=parentNode.BoundsRelative.Right+this.SubNodeHorizontalSpacing;
			if(!bFar)
			{
				x=parentNode.BoundsRelative.X-this.SubNodeHorizontalSpacing;
			}

			Rectangle rChildBounds=Rectangle.Empty;
			bool bFirst=true;

			if(bFar)
			{
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
						PositionSubNodes(node,bFar);
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
			else
			{
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
						PositionSubNodes(node,bFar);
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
			parentNode.ChildNodesBounds=rChildBounds;
		}

		private int GetMinimumAreaHeight(ArrayList nodes, int rootHeight)
		{
			int height=rootHeight;
			if(nodes.Count==1)
				height+=(((Node)nodes[0]).BoundsRelative.Height*2);
			else if(nodes.Count>=2)
				height+=(((Node)nodes[0]).BoundsRelative.Height+((Node)nodes[1]).BoundsRelative.Height);
			return height;
		}

		private int SubNodeHorizontalSpacing
		{
			get {return 24;}
		}

		/// <summary>
		/// Returns true if root node should have expanded part
		/// </summary>
		protected override bool RootHasExpandedPart
		{
			get {return false;}
		}


		private struct NodesMapPosition
		{
			public ArrayList Near;
			public ArrayList Far;
			public ArrayList Default;
			public int NearHeight;
			public int FarHeight;
			public static NodesMapPosition Empty
			{
				get
				{
					NodesMapPosition m;
					m.Default=new ArrayList();
					m.Near=new ArrayList();
					m.Far=new ArrayList();
					m.NearHeight=0;
					m.FarHeight=0;
					return m;
				}
			}			
		}

		/// <summary>
		/// Gets or sets the flow of the sub-root nodes for Map layout.
		/// </summary>
		public eMapFlow MapFlow
		{
			get{return m_MapFlow;}
			set{m_MapFlow=value;}
		}
	}
}
