using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Tree.Layout
{
	/// <summary>
	/// Summary description for NodeTreeLayout.
	/// </summary>
	internal class NodeTreeLayout:NodeLayout
	{
		//private bool m_LayoutPerformed=false;

		public NodeTreeLayout(TreeGX treeControl, Rectangle clientArea):base(treeControl,clientArea)
		{
			ExpandPartSize=new Size(9,9);
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
				NodeLayoutContextInfo layoutInfo=GetDefaultNodeLayoutContextInfo(graphics);

				foreach(Node childNode in topLevelNodes)
				{
					layoutInfo.ContextNode=childNode;
					ProcessNode(layoutInfo);
				}
			}
			finally
			{
				if(this.DisposeGraphics)
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
			else if(node.BoundsRelative.Top!=layoutInfo.Top)
			{
				// Adjust top position
				node.SetBounds(new Rectangle(node.BoundsRelative.X,layoutInfo.Top,node.BoundsRelative.Width,node.BoundsRelative.Height));
				foreach(Cell c in node.Cells)
					c.SetBounds(new Rectangle(c.BoundsRelative.X,layoutInfo.Top,c.BoundsRelative.Width,c.BoundsRelative.Height));
			}

			// Need to set the Top position properly
			layoutInfo.Top+=(node.BoundsRelative.Height+this.NodeVerticalSpacing);
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
