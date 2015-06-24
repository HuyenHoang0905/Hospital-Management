using System;
using System.Drawing;
using System.Collections;

namespace DevComponents.Tree.Layout
{
	/// <summary>
	/// Internal class that orders one level of the nodes in list like layout. Nodes are ordered from top to bottom.
	/// </summary>
	internal class NodeListLayout:Layout.NodeLayout
	{
		public NodeListLayout(TreeGX treeControl, Rectangle clientArea):base(treeControl,clientArea)
		{
		}

		public override void PerformLayout()
		{
			if(m_Tree.Nodes.Count==0)
				return;

			this.PrepareStyles();
			System.Drawing.Graphics graphics=this.GetGraphics();
			m_Width=0;
			m_Height=0;

			try
			{
				NodeCollection nodes=this.GetLayoutNodes();
				NodeLayoutContextInfo layoutInfo=this.GetDefaultNodeLayoutContextInfo(graphics);
				foreach(Node childNode in nodes)
				{
					if(!childNode.Visible)
						continue;
					layoutInfo.ContextNode=childNode;
					ProcessNode(layoutInfo);
					if(childNode.BoundsRelative.Width>this.Width)
						m_Width=childNode.BoundsRelative.Width;
				}
				m_Height=layoutInfo.Top-this.NodeVerticalSpacing;
			}
			finally
			{
				if(this.DisposeGraphics)
					graphics.Dispose();
			}
		}

		private void ProcessNode(NodeLayoutContextInfo layoutInfo)
		{
			LayoutNode(layoutInfo);
			layoutInfo.ContextNode.SetBounds(new Rectangle(layoutInfo.Left,layoutInfo.Top,layoutInfo.ContextNode.BoundsRelative.Width,layoutInfo.ContextNode.BoundsRelative.Height));
			//OffsetNodeLocation(layoutInfo.ContextNode,layoutInfo.Left,layoutInfo.Top);
			layoutInfo.Top+=(layoutInfo.ContextNode.BoundsRelative.Height+this.NodeVerticalSpacing);
		}

		private NodeCollection GetLayoutNodes()
		{
			return m_Tree.Nodes;
		}
	}
}
