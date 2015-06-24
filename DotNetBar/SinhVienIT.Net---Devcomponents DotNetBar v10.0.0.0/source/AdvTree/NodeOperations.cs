using System;
using System.Drawing;
using System.Text;
using DevComponents.AdvTree.Display;
using System.Collections;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents node operations.
	/// </summary>
	internal class NodeOperations
	{
		/// <summary>
		/// Returns full path to the given node.
		/// </summary>
		/// <param name="node">Node to return path to.</param>
		/// <returns>Full path to the node.</returns>
		public static string GetFullPath(Node node,string pathSeparator)
		{
			if(node==null)
				throw new ArgumentNullException("node");
			
			StringBuilder sb=new StringBuilder(node.Text);
			node=node.Parent;
			while(node!=null)
			{
                sb.Insert(0, node.Text + pathSeparator);
				node=node.Parent;
			}

			return sb.ToString();
		}

		/// <summary>
		/// Gets the last child tree node. The LastNode is the last child Node in the NodeCollection stored in the Nodes property of the current tree node. If the Node has no child tree node, the LastNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Last node if found or null if there is no last node.</returns>
		public static Node GetLastNode(Node node)
		{
			if(node.Nodes.Count>0)
			{
				return node.Nodes[node.Nodes.Count-1];
			}
			return null;
		}

        /// <summary>
        /// Returns last rendered node on screen.
        /// </summary>
        /// <param name="tree">Tree control.</param>
        /// <returns>Last rendered node or null</returns>
        public static Node GetLastDisplayedNode(AdvTree tree)
        {
            Rectangle r = tree.ClientRectangle;
            Node node = tree.SelectedNode;
            if (node == null) node = GetFirstVisibleNode(tree);
            Point scrollPos = Point.Empty;
            if (tree.AutoScroll)
                scrollPos = tree.GetAutoScrollPositionOffset();

            // Find last fully rendered node
            Node lastNode = null;
            if (r.Contains(node.Bounds))
                lastNode = node;
            while (node != null)
            {
                node = NodeOperations.GetNextVisibleNode(node);
                if (node != null && node.Selectable)
                {
                    Rectangle nodeRect = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, scrollPos);
                    if (r.Contains(nodeRect))
                        lastNode = node;
                    else if (nodeRect.Y > r.Bottom)
                        break;
                }
            }

            return lastNode;
        }

        /// <summary>
        /// Returns first rendered node on screen.
        /// </summary>
        /// <param name="tree">Tree control.</param>
        /// <returns>Last rendered node or null</returns>
        public static Node GetFirstDisplayedNode(AdvTree tree)
        {
            Rectangle r = tree.GetInnerRectangle();
            r.Y += tree.ColumnHeaderHeight;
            r.Height -= tree.ColumnHeaderHeight;

            Node node = tree.SelectedNode;
            if (node == null) node = GetFirstVisibleNode(tree);
            Point scrollPos = Point.Empty;
            if (tree.AutoScroll)
                scrollPos = tree.GetAutoScrollPositionOffset();
            // Find last fully rendered node
            Node lastNode = null;
            if (r.Contains(node.Bounds))
                lastNode = node;
            while (node != null)
            {
                node = NodeOperations.GetPreviousVisibleNode(node);
                if (node != null && node.Selectable)
                {
                    Rectangle nodeRect = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, scrollPos);
                    if (r.Contains(nodeRect))
                        lastNode = node;
                    else if (nodeRect.Y < r.Y)
                        break;
                }
            }

            return lastNode;
        }

        /// <summary>
        /// Gets first visible node.
        /// </summary>
        /// <param name="tree">Reference to tree.</param>
        /// <returns>Last visible node found or null</returns>
        public static Node GetFirstVisibleNode(AdvTree tree)
        {
            if (tree.Nodes.Count == 0) return null;

            Node node = tree.DisplayRootNode == null ? tree.Nodes[0] : tree.DisplayRootNode;
            if (node.Visible)
                return node;
            return GetNextVisibleNode(node);
        }

        /// <summary>
        /// Gets last visible node in tree control.
        /// </summary>
        /// <param name="tree">Reference to tree.</param>
        /// <returns>Last visible node found or null</returns>
        public static Node GetLastVisibleNode(AdvTree tree)
        {
            if (tree.Nodes.Count == 0) return null;

            Node node = tree.DisplayRootNode == null ? tree.Nodes[tree.Nodes.Count - 1] : tree.DisplayRootNode;
            if (node.Visible)
            {
                if (node.Nodes.Count > 0 && node.Expanded)
                {
                    Node child = GetLastVisibleChildNode(node);
                    if (child != null)
                    {
                        while (child.Nodes.Count > 0 && child.Expanded)
                        {
                            Node child1 = GetLastVisibleChildNode(child);
                            if (child1 == null) return child;
                            child = child1;
                        }
                        return child;
                    }
                }
                return node;
            }
            return GetPreviousVisibleNode(node);
        }

        /// <summary>
        /// Gets last visible top-level node in tree control.
        /// </summary>
        /// <param name="tree">Reference to tree.</param>
        /// <returns>Last visible node found or null</returns>
        public static Node GetLastVisibleTopLevelNode(AdvTree tree)
        {
            if (tree.Nodes.Count == 0) return null;

            NodeCollection col = tree.Nodes;
            if (tree.DisplayRootNode != null) col = tree.DisplayRootNode.Nodes;

            for (int i = col.Count - 1; i >= 0; i--)
            {
                if (col[i].Visible)
                    return col[i];
            }

            return null;
        }

		/// <summary>
		/// Gets the next sibling tree node. The NextNode is the next sibling Node in the NodeCollection stored in the Nodes property of the tree node's parent Node. If there is no next tree node, the NextNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Node object or null if node cannot be found.</returns>
		public static Node GetNextNode(Node node)
		{
			if(node==null)
				return null;
			NodeCollection parentNodes=null;
			if(node.Parent!=null)
				parentNodes=node.Parent.Nodes;
			else if(node.internalTreeControl!=null)
				parentNodes=node.internalTreeControl.Nodes;
			
			if(parentNodes!=null)
			{
				int index=parentNodes.IndexOf(node);
				if(index<parentNodes.Count-1)
					return parentNodes[index+1];
			}

			return null;
		}
		
		/// <summary>
		/// Returns next visible sibling tree node.
		/// </summary>
		/// <param name="node">Reference node</param>
		/// <returns>Node object or null if next visible node cannot be found</returns>
		public static Node GetNextVisibleSibling(Node node)
		{
			if(node==null)
				return null;
			Node ret = GetNextNode(node);
			while(ret!=null && !ret.Visible)
				ret = GetNextNode(ret);
			return ret;
		}

		/// <summary>
		/// Gets the next visible tree node. The NextVisibleNode can be a child, sibling, or a tree node from another branch. If there is no next tree node, the NextVisibleNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Node object or null if node cannot be found.</returns>
		public static Node GetNextVisibleNode(Node node)
		{
			Node nextVisibleNode=null;
			
			// First see whether all parent nodes are expanded and if not find the first one that is
			Node parent=node.Parent;
			Node lastNotExpanded=null;
			while(parent!=null)
			{
				if(!parent.Expanded)
					lastNotExpanded=parent;
				parent=parent.Parent;
			}
			if(lastNotExpanded!=null)
			{
				// Last parent node expanded
				if(lastNotExpanded.Parent!=null)
					lastNotExpanded=lastNotExpanded.Parent;
                while (lastNotExpanded != null)
                {
                    lastNotExpanded = GetNextNode(lastNotExpanded);
                    if (lastNotExpanded != null && lastNotExpanded.Visible) return lastNotExpanded;
                }
			}

			// Walk into the child nodes
            if (node.Expanded && node.Nodes.Count > 0)
            {
                foreach (Node n in node.Nodes)
                {
                    if (n.Visible) return n;
                }
                //return null;
            }

			// Get next sibling
            nextVisibleNode = GetAnyNextNode(node);
            while (nextVisibleNode != null)
            {
                if (nextVisibleNode.Visible && nextVisibleNode.IsVisible)
                    return nextVisibleNode;
                nextVisibleNode = GetAnyNextNode(nextVisibleNode);
            }

			// Walk-up...
            //while (nextVisibleNode == null && node != null)
            //{
            //    node = node.Parent;
            //    nextVisibleNode = GetAnyNextNode(node);
            //    if (nextVisibleNode != null && !nextVisibleNode.Visible)
            //        nextVisibleNode = null;
            //}
			return nextVisibleNode;
		}

        /// <summary>
        /// Gets the next visible tree node. The NextVisibleNode can be a child, sibling, or a tree node from another branch. If there is no next tree node, the NextVisibleNode property returns a null reference (Nothing in Visual Basic).
        /// </summary>
        /// <param name="node">Reference node.</param>
        /// <returns>Node object or null if node cannot be found.</returns>
        public static Node GetAnyNextNode(Node node)
        {
            Node nextNode = null;

            // Walk into the child nodes
            if (node.Nodes.Count > 0)
            {
                return node.Nodes[0];
            }

            // Get next sibling
            nextNode = GetNextNode(node);

            // Walk-up...
            if (nextNode == null)
            {
                while (nextNode == null && node != null)
                {
                    node = node.Parent;
                    nextNode = GetNextNode(node);
                }
            }

            return nextNode;
        }

		/// <summary>
		/// Gets a value indicating whether the tree node is visible. Node is considered to be visible when it's Visible property is set to true and path to the node is available i.e. all parent nodes are expanded.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static bool GetIsNodeVisible(Node node)
		{
			if(!node.Visible)
				return false;

			Node n=node;
			while(n.Parent!=null)
			{
				n=n.Parent;
				if(!n.Expanded || !n.Visible)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Returns whether node is displayed on the screen and visible to the user. When node is outside of the viewable area this property will return false. It will also return false if node is not visible.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static bool GetIsNodeDisplayed(Node node)
		{
			if(!node.Visible || !GetIsNodeVisible(node))
				return false;
			AdvTree tree=node.TreeControl;
			if(tree!=null)
			{
				Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,node,tree.NodeDisplay.Offset);
				return tree.ClientRectangle.IntersectsWith(r);
			}
			return false;
		}

		/// <summary>
		/// Gets the zero based index of position of the tree node in the tree node collection. -1 is returned if node is not added to the nodes collection.
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Zero based index or -1 if node is not in collection.</returns>
		public static int GetNodeIndex(Node node)
		{
            if (node.Parent == null)
            {
                AdvTree tree = node.TreeControl;
                if (tree != null)
                    return tree.Nodes.IndexOf(node);
                return -1;
            }
			return node.Parent.Nodes.IndexOf(node);
		}

		/// <summary>
		/// Gets the previous sibling tree node. The PrevNode is the previous sibling Node in the NodeCollection stored in the Nodes property of the tree node's parent Node. If there is no previous tree node, the PrevNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Node object or null if node cannot be found.</returns>
		public static Node GetPreviousNode(Node node)
		{
			if(node==null)
				return null;
			NodeCollection parentNodes=null;
			if(node.Parent!=null)
				parentNodes=node.Parent.Nodes;
			else if(node.internalTreeControl!=null)
				parentNodes=node.internalTreeControl.Nodes;
			
			if(parentNodes!=null)
			{
				int index=parentNodes.IndexOf(node);
				if(index>0)
					return parentNodes[index-1];
			}

			return null;
		}

        ///// <summary>
        ///// Gets the previous visible tree node. The PrevVisibleNode can be a child, sibling, or a tree node from another branch. If there is no previous tree node, the PrevVisibleNode property returns a null reference (Nothing in Visual Basic).
        ///// </summary>
        ///// <param name="node">Reference node.</param>
        ///// <returns>Node object or null if node cannot be found.</returns>
        //public static Node GetPreviousDisplayedNode(Node node)
        //{
        //    Node referenceNode = node;
        //    do
        //    {
        //        referenceNode = GetPreviousVisibleNode(referenceNode);
        //        if (referenceNode != null && referenceNode.IsVisible) break;
        //    } while (referenceNode != null);
        //    return referenceNode;
        //}

		/// <summary>
		/// Gets the previous visible tree node. The PrevVisibleNode can be a child, sibling, or a tree node from another branch. If there is no previous tree node, the PrevVisibleNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Node object or null if node cannot be found.</returns>
		public static Node GetPreviousVisibleNode(Node node)
		{
			Node prevVisibleNode=null;
			
			// First see whether all parent nodes are expanded and if not find the first one that is
			Node referenceNode=node.Parent;
			Node lastNotExpanded=null;
			while(referenceNode!=null)
			{
				if(!referenceNode.Expanded)
					lastNotExpanded=referenceNode;
				referenceNode=referenceNode.Parent;
			}
            if (lastNotExpanded != null)
            {
                if (lastNotExpanded.IsVisible) return lastNotExpanded;
                referenceNode = lastNotExpanded;
            }
            else
                referenceNode = node;

			// Walk into the previous node parent
			if(referenceNode.Parent!=null)
			{
                if (referenceNode.Parent.Nodes.IndexOf(referenceNode) > 0)
                {
                    prevVisibleNode = GetAnyPreviousNode(referenceNode);
                    while (prevVisibleNode != null)
                    {
                        if (prevVisibleNode.IsVisible) break;
                        prevVisibleNode = GetAnyPreviousNode(prevVisibleNode);
                    }
                    if (prevVisibleNode != null) return prevVisibleNode;
                }
                else
                {
                    while (referenceNode.Parent != null)
                    {
                        if (referenceNode.Parent.IsVisible)
                            return referenceNode.Parent;
                        referenceNode = referenceNode.Parent;
                    }
                    if (referenceNode.TreeControl != null)
                        prevVisibleNode = GetAnyPreviousNode(referenceNode);
                }
			}
            else if (referenceNode.TreeControl != null)
            {
                prevVisibleNode = GetPreviousNode(referenceNode);
                if (prevVisibleNode != null && (!prevVisibleNode.Visible || !prevVisibleNode.Selectable))
                {
                    while (prevVisibleNode != null && !(prevVisibleNode.Visible || prevVisibleNode.Selectable))
                        prevVisibleNode = GetPreviousNode(prevVisibleNode);
                }
            }
            else
                return null;

			if(prevVisibleNode==null)
				return null;
			
			// Walk into the node
			while(prevVisibleNode.Expanded && prevVisibleNode.Nodes.Count>0)
			{
                prevVisibleNode = prevVisibleNode.Nodes[prevVisibleNode.Nodes.Count - 1];
			}

			return prevVisibleNode;
		}

        /// <summary>
        /// Gets the previous tree node. The Previous Node can be a child, sibling, or a tree node from another branch. If there is no previous tree node, the PrevNode property returns a null reference (Nothing in Visual Basic).
        /// </summary>
        /// <param name="node">Reference node.</param>
        /// <returns>Node object or null if node cannot be found.</returns>
        public static Node GetAnyPreviousNode(Node node)
        {
            Node prevNode = null;

            Node referenceNode = node;

            // Walk into the previous node parent
            if (referenceNode.Parent != null)
            {
                if (referenceNode.Parent.Nodes.IndexOf(referenceNode) > 0)
                {
                    prevNode = GetPreviousNode(referenceNode);
                }
                else
                {
                    return referenceNode.Parent;
                }
            }
            else if (referenceNode.TreeControl != null)
                prevNode = GetPreviousNode(referenceNode);
            else
                return null;

            if (prevNode == null)
                return null;

            // Walk into the node
            while (prevNode.Nodes.Count > 0)
            {
                prevNode = prevNode.Nodes[prevNode.Nodes.Count - 1];
            }

            return prevNode;
        }

		/// <summary>
		/// Returns true if node passed is considered root node for display purposes.
		/// </summary>
		/// <param name="tree">Reference to the tree control.</param>
		/// <param name="node">Node to test.</param>
		/// <returns>true if node is root node for display purposes otherwise false.</returns>
		public static bool IsRootNode(AdvTree tree, Node node)
		{
			if(node.Parent==null || node==tree.DisplayRootNode)
				return true;
			return false;
		}

        /// <summary>
        /// Ensures that the cell is visible, expanding nodes and scrolling the control as necessary.
        /// </summary>
        /// <param name="reference">Cell to be made visible.</param>
        public static void EnsureVisible(Cell cell)
        {
            Node parentNode = cell.Parent;
            if (parentNode == null) return;
            AdvTree tree = parentNode.TreeControl;
            if (tree == null) return;

            EnsureVisible(parentNode, false);

            Rectangle r = NodeDisplay.GetCellRectangle(eCellRectanglePart.CellBounds, cell, tree.NodeDisplay.Offset);

            if (tree != null && tree.AutoScroll)
            {
                if (tree.Zoom != 1)
                {
                    r = tree.GetScreenRectangle(r);
                }
                Rectangle treeClientRectangle = tree.GetInnerRectangle();
                if (tree.VScrollBar != null) treeClientRectangle.Width -= tree.VScrollBar.Width;
                if (tree.HScrollBar != null) treeClientRectangle.Height -= tree.HScrollBar.Height;

                if (!treeClientRectangle.Contains(r))
                {
                    Point p = tree.NodeDisplay.Offset;
                    p.Y = tree.AutoScrollPosition.Y;
                    if (r.X < treeClientRectangle.X)
                        p.X -= r.X;
                    else if (r.X > treeClientRectangle.Right)
                        p.X += (r.Right - treeClientRectangle.Right);
                    // Scroll it to show the cell
                    if (!tree.IsLayoutPending)
                        tree.AutoScrollPosition = p;
                }
            }
        }

		/// <summary>
		/// Ensures that the node is visible, expanding nodes and scrolling the control as necessary.
		/// </summary>
		/// <param name="reference">Node to be made visible.</param>
        public static void EnsureVisible(Node reference, bool scrollHorizontally)
        {
            // First expand all parents
            AdvTree tree = reference.TreeControl;
            if (tree == null) return;

            tree.SuspendPaint = true;
            try
            {
                Node node = reference;
                bool layout = false;
                while (node.Parent != null)
                {
                    if (!node.Parent.Expanded)
                    {
                        node.Parent.Expand();
                        layout = true;
                    }
                    node = node.Parent;
                }

                if (layout || tree.IsLayoutPending)
                    tree.RecalcLayoutInternal();

                // Adjust for tree never being displayed but it has been scrolled
                Point offset = tree.NodeDisplay.Offset;
                if (offset.IsEmpty && tree.AutoScroll && !tree.AutoScrollPosition.IsEmpty)
                    offset = tree.GetAutoScrollPositionOffset();

                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, reference, offset);
                if (tree != null && tree.AutoScroll)
                {
                    if (tree.Zoom != 1)
                    {
                        r = tree.GetScreenRectangle(r);
                    }
                    Rectangle treeClientRectangle = tree.GetInnerRectangle();
                    if (tree.VScrollBar != null) treeClientRectangle.Width -= tree.VScrollBar.Width;
                    if (tree.HScrollBar != null) treeClientRectangle.Height -= tree.HScrollBar.Height;
                    int treeColumnHeaderHeight = tree.ColumnHeaderHeight;
                    treeClientRectangle.Y += treeColumnHeaderHeight;
                    treeClientRectangle.Height -= treeColumnHeaderHeight;

                    if (!treeClientRectangle.Contains(r))
                    {
                        Point p = offset; // tree.NodeDisplay.Offset;
                        if (scrollHorizontally)
                        {
                            /*if (r.Right > treeClientRectangle.Right)
                                p.X -= r.Right - treeClientRectangle.Right;
                            else*/
                            if (r.X < treeClientRectangle.X)
                                p.X -= r.X;
                        }
                        if (r.Height > treeClientRectangle.Height * .8)
                            p.Y = -Math.Max(0, (r.Y - 4));
                        else if (r.Bottom > treeClientRectangle.Bottom)
                            p.Y -= r.Bottom - treeClientRectangle.Bottom + r.Height;
                        else if (r.Y < treeClientRectangle.Y)
                            p.Y -= (r.Y - r.Height - treeColumnHeaderHeight);
                        if (p.Y > -10 && p.Y < 0 || p.Y < 11 && p.Y > 0) p.Y = 0; // Snap to top
                        // Scroll it to show the node
                        if (!tree.IsLayoutPending)
                            tree.AutoScrollPosition = p;
                    }
                }
            }
            finally
            {
                if (tree != null)
                    tree.SuspendPaint = false;
            }
        }

		/// <summary>
		/// Returns number of visible child nodes for given node.
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Number of visible child nodes.</returns>
		public static int GetVisibleNodesCount(Node node)
		{
			if(node.Nodes.Count==0)
				return 0;
			int count=0;
			foreach(Node n in node.Nodes)
			{
				if(n.Visible)
					count++;
			}
			return count;
		}

		/// <summary>
		/// Returns true if node has at least single visible child node.
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>True if at least single child node is visible otherwise false.</returns>
		public static bool GetAnyVisibleNodes(Node node)
		{
            if (!node.HasChildNodes)
                return false;
			foreach(Node n in node.Nodes)
			{
				if(n.Visible)
					return true;
			}
			return false;
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
		/// <param name="tree">Tree control to find node at.</param>
        public static NodeHitTestInfo GetNodeAt(AdvTree tree, Point p)
		{
			return GetNodeAt(tree,p.X,p.Y);
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
		/// <param name="tree">Tree control to find node at.</param>
        public static NodeHitTestInfo GetNodeAt(AdvTree tree, int x, int y)
        {
            return GetNodeAt(tree, x, y, false);
        }

        /// <summary>
		/// Retrieves the tree node that is at the specified location.
		/// </summary>
		/// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
		/// <remarks>
		/// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
		///     MouseDown event as the x and y parameters.</para>
		/// </remarks>
        /// <param name="tree">Tree control to find node at.</param>
		/// <param name="x">The X position to evaluate and retrieve the node from.</param>
		/// <param name="y">The Y position to evaluate and retrieve the node from.</param>
        /// <param name="paintedOnly">Enumerates rendered nodes only.</param>
        public static NodeHitTestInfo GetNodeAt(AdvTree tree, int x, int y, bool paintedOnly)
        {
            return GetNodeAt(tree, x, y, paintedOnly, false);
        }

		/// <summary>
		/// Retrieves the tree node that is at the specified location.
		/// </summary>
		/// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
		/// <remarks>
		/// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
		///     MouseDown event as the x and y parameters.</para>
		/// </remarks>
        /// <param name="tree">Tree control to find node at.</param>
		/// <param name="x">The X position to evaluate and retrieve the node from.</param>
		/// <param name="y">The Y position to evaluate and retrieve the node from.</param>
        /// <param name="paintedOnly">Enumerates rendered nodes only.</param>
        public static NodeHitTestInfo GetNodeAt(AdvTree tree, int x, int y, bool paintedOnly, bool isUserInterfaceHitTest)
		{
            NodeHitTestInfo info = new NodeHitTestInfo();
            if (tree.Nodes.Count == 0 || !tree.DisplayRectangle.Contains(x, y) && tree.Zoom == 1)
                return info;

			Node node=tree.Nodes[0];
			Node retNode=null;
			if(tree.DisplayRootNode!=null)
				node=tree.DisplayRootNode;

            Point displayOffset = tree.NodeDisplay.Offset;

            // Enumerate painted nodes first
            if(tree.DisplayRectangle.Contains(x, y))
            {
                ArrayList paintedNodes = tree.NodeDisplay.PaintedNodes;
                foreach (Node paintedNode in paintedNodes)
                {
                    if (!paintedNode.IsDisplayed) continue;
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, paintedNode, displayOffset);
                    if (r.Contains(x, y))
                    {
                        info.NodeAt = paintedNode;
                        return info;
                    }
                    else if (NodeDisplay.HasColumnsVisible(paintedNode))
                    {
                        Rectangle headerBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, paintedNode, displayOffset);
                        if (headerBounds.Contains(x, y))
                        {
                            info.ColumnsAt = paintedNode.NodesColumns;
                            return info;
                        }
                    }
                }
            }
            if (paintedOnly) return info;

            while (node != null)
            {
                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, displayOffset);
                if (r.Contains(x, y))
                {
                    retNode = node;
                    break;
                }
                else if (NodeDisplay.HasColumnsVisible(node))
                {
                    Rectangle headerBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, node, displayOffset);
                    if (headerBounds.Contains(x, y))
                    {
                        info.ColumnsAt = node.NodesColumns;
                        return info;
                    }
                }
                if (r.Y > y) break;

                r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds, node, displayOffset);
                if (!r.Contains(x, y))
                    node = GetNextVisibleSibling(node);
                else
                    node = node.NextVisibleNode;
            }

            info.NodeAt = retNode;
			return info;
		}

        /// <summary>
        /// Retrieves the tree node that is at the specified vertical location.
        /// </summary>
        /// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
        /// <remarks>
        /// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
        ///     MouseDown event as the x and y parameters.</para>
        /// </remarks>
        /// <param name="y">The Y position to evaluate and retrieve the node from.</param>
        /// <param name="tree">Tree control to find node at.</param>
        public static NodeHitTestInfo GetNodeAt(AdvTree tree, int y)
        {
            return GetNodeAt(tree, y, false);
        }

        /// <summary>
        /// Retrieves the tree node that is at the specified vertical location.
        /// </summary>
        /// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
        /// <remarks>
        /// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
        ///     MouseDown event as the x and y parameters.</para>
        /// </remarks>
        /// <param name="y">The Y position to evaluate and retrieve the node from.</param>
        /// <param name="tree">Tree control to find node at.</param>
        /// <param name="paintedOnly">Enumerates rendered nodes only.</param>
        public static NodeHitTestInfo GetNodeAt(AdvTree tree, int y, bool paintedOnly)
        {
            return GetNodeAt(tree, y, paintedOnly, false);
        }

        /// <summary>
        /// Retrieves the tree node that is at the specified vertical location.
        /// </summary>
        /// <returns>The TreeNode at the specified location, in tree view coordinates.</returns>
        /// <remarks>
        /// 	<para>You can pass the MouseEventArgs.X and MouseEventArgs.Y coordinates of the
        ///     MouseDown event as the x and y parameters.</para>
        /// </remarks>
        /// <param name="y">The Y position to evaluate and retrieve the node from.</param>
        /// <param name="tree">Tree control to find node at.</param>
        /// <param name="paintedOnly">Enumerates rendered nodes only.</param>
        public static NodeHitTestInfo GetNodeAt(AdvTree tree, int y, bool paintedOnly, bool isUserInterfaceHitTest)
        {
            NodeHitTestInfo info = new NodeHitTestInfo();
            if (tree.Nodes.Count == 0 || !tree.DisplayRectangle.Contains(1, y) && tree.Zoom == 1)
                return info;
            Point displayOffset = tree.NodeDisplay.Offset;

            // Enumerate painted node first
            if (tree.DisplayRectangle.Contains(1, y))
            {
                ArrayList paintedNodes = tree.NodeDisplay.PaintedNodes;
                foreach (Node paintedNode in paintedNodes)
                {
                    if (!paintedNode.IsDisplayed && !(paintedNode.Expanded && paintedNode.HasColumns)) continue;
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, paintedNode, displayOffset);
                    if (y >= r.Y && y <= r.Bottom)
                    {
                        info.NodeAt = paintedNode;
                        return info;
                    }
                    else if(NodeDisplay.HasColumnsVisible(paintedNode))
                    {
                        Rectangle headerBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, paintedNode, displayOffset);
                        if (y >= headerBounds.Y && y <= headerBounds.Bottom)
                        {
                            info.ColumnsAt = paintedNode.NodesColumns;
                            return info;
                        }
                    }
                }
            }
            if (paintedOnly) return info;

            Node node = tree.Nodes[0];
            Node retNode = null;
            if (tree.DisplayRootNode != null)
                node = tree.DisplayRootNode;
            while (node != null)
            {
                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, displayOffset);
                if (node.Visible && y >= r.Y && y <= r.Bottom)
                {
                    retNode = node;
                    break;
                }
                else if (NodeDisplay.HasColumnsVisible(node))
                {
                    Rectangle headerBounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, node, displayOffset);
                    if (y >= headerBounds.Y && y <= headerBounds.Bottom)
                    {
                        info.ColumnsAt = node.NodesColumns;
                        return info;
                    }
                }
                if (isUserInterfaceHitTest && (r.Bottom + 16 < y || r.Y + 16 > y))
                    break;
                r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds, node, displayOffset);
                if (!(y >= r.Y && y <= r.Bottom))
                    node = GetNextVisibleSibling(node);
                else
                    node = node.NextVisibleNode;
            }

            info.NodeAt = retNode;
            return info;
        }
		
		public static TreeAreaInfo GetTreeAreaInfo(AdvTree tree, int x, int y)
		{
			TreeAreaInfo areaInfo=new TreeAreaInfo();

            if (tree.Nodes.Count == 0 || !tree.DisplayRectangle.Contains(x, y) && tree.Zoom == 1)
				return null;

			Node node=tree.Nodes[0];
			if(tree.DisplayRootNode!=null)
				node = tree.DisplayRootNode;
			
            if (tree.DisplayRootNode != null)
                node = tree.DisplayRootNode;

            Node previousNode = null;
            Rectangle previousRectangle = Rectangle.Empty;

            if (tree.View == eView.Tile)
            {
                while (node != null)
                {
                    if (!node.IsDisplayed)
                    {
                        node = node.NextVisibleNode;
                        continue;
                    }
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, tree.NodeDisplay.Offset);
                    if (r.Contains(x, y))
                    {
                        areaInfo.NodeAt = node;
                        break;
                    }
                    if (y >= r.Y && y <= r.Bottom && node.HasChildNodes)
                    {
                        areaInfo.NodeAt = node;
                        break;
                    }
                    else if (previousNode != null && y > previousRectangle.Bottom && y < r.Y)
                    {
                        // Covers the case if point is between two nodes in the vertical space between nodes due to node vertical spacing
                        areaInfo.NodeAt = previousNode;
                        break;
                    }

                    previousNode = node;
                    previousRectangle = r;

                    node = node.NextVisibleNode;
                }

            }
            else
            {
                while (node != null)
                {
                    if (!node.IsDisplayed)
                    {
                        node = node.NextVisibleNode;
                        continue;
                    }
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, tree.NodeDisplay.Offset);
                    if (y >= r.Y && y <= r.Bottom)
                    {
                        areaInfo.NodeAt = node;
                        break;
                    }
                    else if (previousNode != null && y > previousRectangle.Bottom && y < r.Y)
                    {
                        // Covers the case if point is between two nodes in the vertical space between nodes due to node vertical spacing
                        areaInfo.NodeAt = previousNode;
                        break;
                    }

                    previousNode = node;
                    previousRectangle = r;

                    node = node.NextVisibleNode;
                }
            }
			return areaInfo;
		}
		
		/// <summary>
		/// Gets the count of visible child nodes (Visible=true) for given node.
		/// </summary>
		/// <param name="parent">Reference to Node object.</param>
		/// <returns>Number of visible nodes.</returns>
		public static int GetVisibleChildNodesCount(Node parent)
		{
			int count = 0;
			foreach(Node node in parent.Nodes)
			{
				if(node.Visible)
					count++;
			}
			return count;
		}
		
		/// <summary>
		/// Gets the first visible child node or returns null if node cannot be found.
		/// </summary>
		/// <param name="parent">Reference to Node object.</param>
		/// <returns>First visible node or null if node cannot be found.</returns>
		public static Node GetFirstVisibleChildNode(Node parent)
		{
			foreach(Node node in parent.Nodes)
			{
				if(node.Visible)
					return node;
			}
			return null;
		}

        /// <summary>
        /// Gets the last visible child node or returns null if node cannot be found.
        /// </summary>
        /// <param name="parent">Reference to Node object.</param>
        /// <returns>Last visible node or null if node cannot be found.</returns>
        public static Node GetLastVisibleChildNode(Node parent)
        {
            NodeCollection col = parent.Nodes;
            int c = col.Count - 1;
            for (int i = c; i >=0; i--)
            {
                Node node = col[i];
                if (node.Visible)
                    return node;
            }
            
            return null;
        }

        /// <summary>
        /// Gets whether any node from array is child node of parent on any level.
        /// </summary>
        /// <param name="parent">Reference to parent node.</param>
        /// <param name="child">Reference to child nodes.</param>
        /// <returns></returns>
        public static bool IsChildNode(Node parent, Node[] childNodes)
        {
            foreach (Node childNode in childNodes)
            {
                if (IsChildNode(parent, childNode))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets whether node is child node of parent on any level.
        /// </summary>
        /// <param name="parent">Reference to parent node.</param>
        /// <param name="child">Reference to child node.</param>
        /// <returns></returns>
        public static bool IsChildNode(Node parent, Node child)
        {
            if (parent == child) return false;
            while (child.Parent != null)
            {
                if (child.Parent == parent) return true;
                child = child.Parent;
            }
            return false;
        }

        /// <summary>
        /// Returns true if child node is child of any parent node at any level.
        /// </summary>
        /// <param name="parents">Parent nodes array</param>
        /// <param name="child">Child node</param>
        /// <returns>true if child otherwise false</returns>
        public static bool IsChildOfAnyParent(Node[] parents, Node child)
        {
            foreach (Node parent in parents)
            {
                if (IsChildNode(parent, child)) return true;
            }
            return false;
        }
		
		#region Licensing
#if !TRIAL
		internal static bool keyValidated=false;
		internal static int keyValidated2=0;
		internal static bool ValidateLicenseKey(string key)
		{
			bool ret = false;
			string[] parts = key.Split('-');
			int i = 10;
			foreach(string s in parts)
			{
				if(s=="88405280")
					i++;
				else if(s=="D06E")
					i += 10;
				else if(s=="4617")
					i += 8;
				else if(s=="8810")
					i += 12;
				else if(s=="64462F60FA93")
					i += 3;
			}
			if(i==29)
				return true;
			keyValidated = true;
			return ret;
		}
		
		internal static bool CheckLicenseKey(string key)
		{
			// {F962CEC7-CD8F-4911-A9E9-CAB39962FC1F}, 114
			string[] parts = key.Split('-');
			int test = 0;
			for(int i=parts.Length-1;i>=0;i--)
			{
				if(parts[i]=="CD8F")
					test += 12;
				else if(parts[i]=="CAB39962FC1F")
					test += 2;
				else if(parts[i]=="A9E9")
					test += 3;
				else if(parts[i]=="4911")
					test += 7;
				else if(parts[i]=="F962CEC7")
					test += 13;
			}

			keyValidated2 = test + 77;

			if(test==23)
				return false;
			
			return true;
		}
#endif
		
#if TRIAL
		private static Color m_ColorExpFlag=Color.Empty;
		internal static int ColorCountExp=0;
		internal static bool ColorExpAlt()
		{
			Color clr=SystemColors.Control;
			Color clr2;
			Color clr3;
			clr2=clr;
			if(clr2.ToArgb()==clr.ToArgb())
			{
				clr3=clr2;
			}
			else
			{
				clr3=clr;
			}

			ColorCountExp=clr.A;

			if(!m_ColorExpFlag.IsEmpty)
			{
				return (m_ColorExpFlag==Color.Black?false:true);
			}
			try
			{
				Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.ClassesRoot;
				key=key.CreateSubKey("CLSID\\{542FD3B2-2F65-4290-AB4F-EBFF0444C54C}\\InprocServer32");
			
				try
				{
					if(key.GetValue("")==null || key.GetValue("").ToString()=="")
					{
						key.SetValue("",DateTime.Today.ToOADate().ToString());
					}
					else
					{
						if(key.GetValue("").ToString()=="windows3.dll")
						{
							m_ColorExpFlag=Color.White;
							key.Close();
							key=null;
							return true;
						}
						DateTime date=DateTime.FromOADate(double.Parse(key.GetValue("").ToString()));
						if(((TimeSpan)DateTime.Today.Subtract(date)).TotalDays>30)
						{
							m_ColorExpFlag=Color.White;
							key.SetValue("","windows3.dll");
							key.Close();
							key=null;
							return true;
						}
						if(((TimeSpan)DateTime.Today.Subtract(date)).TotalDays<0)
						{
							m_ColorExpFlag=Color.White;
							key.SetValue("","windows2.dll");
							key.Close();
							key=null;
							return true;
						}
					}
				}
				finally
				{
					if(key!=null)
						key.Close();
				}
			}
			catch{}
			m_ColorExpFlag=Color.Black;
			return false;
		}
#endif
        #endregion

        /// <summary>
        /// Finds the node based on the Node.Name property.
        /// </summary>
        /// <param name="advTree">Reference to a tree control.</param>
        /// <param name="name">Reference to a node with given name or null if node cannot be found.</param>
        public static Node FindNodeByName(AdvTree advTree, string name)
        {
            return FindNodeByName(advTree.Nodes, name);
        }

        public static Node FindNodeByName(NodeCollection col, string name)
        {
            foreach (Node node in col)
            {
                if (node.Name == name)
                    return node;
                if (node.HasChildNodes)
                {
                    Node n2 = FindNodeByName(node.Nodes, name);
                    if (n2 != null) return n2;
                }
            }
            return null;
        }

        public static void FindNodesByName(NodeCollection col, string name, bool searchAllChildren, ArrayList listToPopulate)
        {
            foreach (Node node in col)
            {
                if (node.Name == name)
                    listToPopulate.Add(node);
                if (searchAllChildren && node.HasChildNodes)
                {
                    FindNodesByName(node.Nodes, name, true, listToPopulate);
                }
            }
        }

        /// <summary>
        /// Finds the node based on the Node.DataKey property.
        /// </summary>
        /// <param name="advTree">Reference to a tree control.</param>
        /// <param name="name">Reference to a node with given key or null if node cannot be found.</param>
        public static Node FindNodeByDataKey(AdvTree advTree, object key)
        {
            return FindNodeByDataKey(advTree.Nodes, key);
        }
        public static Node FindNodeByDataKey(NodeCollection col, object key)
        {
            foreach (Node node in col)
            {
                if (object.Equals(node.DataKey, key))
                    return node;
                if (node.HasChildNodes)
                {
                    Node n2 = FindNodeByDataKey(node.Nodes, key);
                    if (n2 != null) return n2;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the node based on the Node.BindingIndex property.
        /// </summary>
        /// <param name="advTree">Reference to a tree control.</param>
        /// <param name="bindingIndex">Index to look for</param>
        public static Node FindNodeByBindingIndex(AdvTree advTree, int bindingIndex)
        {
            return FindNodeByBindingIndex(advTree.Nodes, bindingIndex);
        }
        public static Node FindNodeByBindingIndex(NodeCollection col, int bindingIndex)
        {
            foreach (Node node in col)
            {
                if (node.BindingIndex == bindingIndex)
                    return node;
                if (node.HasChildNodes)
                {
                    Node n2 = FindNodeByBindingIndex(node.Nodes, bindingIndex);
                    if (n2 != null) return n2;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns next visible cell in node.
        /// </summary>
        /// <param name="node">Reference to a node</param>
        /// <param name="startIndex">The index at which to start search.</param>
        /// <returns>Reference to cell or null if there are no visible cells</returns>
        public static Cell GetNextVisibleCell(Node node, int startIndex)
        {
            return GetVisibleCell(node, startIndex, 1);
        }
        /// <summary>
        /// Returns previous visible cell in node.
        /// </summary>
        /// <param name="node">Reference to a node</param>
        /// <param name="startIndex">The index at which to start search.</param>
        /// <returns>Reference to cell or null if there are no visible cells</returns>
        public static Cell GetPreviousVisibleCell(Node node, int startIndex)
        {
            return GetVisibleCell(node, startIndex, -1);
        }

        public static Cell GetVisibleCell(Node node, int startIndex, int direction)
        {
            startIndex += direction;

            int c = direction > 0 ? node.Cells.Count - 1 : 0;
            int i = startIndex;
            int loopCount = 0;
            while (loopCount<2)
            {
                if (i < 0)
                {
                    i = node.Cells.Count - 1;
                    loopCount++;
                }
                else if (i >= node.Cells.Count)
                {
                    i = 0;
                    loopCount++;
                }

                if (node.Cells[i].IsVisible) return node.Cells[i];
                i += direction;
            }

            return null;
        }

        /// <summary>
        /// Returns the zero based flat index of the node. Flat index is the index of the node as if tree structure
        /// has been flattened into the list.
        /// </summary>
        /// <param name="tree">Reference to parent tree control.</param>
        /// <param name="node">Reference to the node to return index for.</param>
        /// <returns>Zero based node index or -1 if index cannot be determined.</returns>
        internal static int GetNodeFlatIndex(AdvTree tree, Node node)
        {
            if (node == null) throw new NullReferenceException("Parameter node cannot be null");
            int count = 0;
            Node currentNode = GetAnyPreviousNode(node);
            while (currentNode != null)
            {
                count++;
                currentNode = GetAnyPreviousNode(currentNode);
                if (currentNode != null && currentNode.Parent == null)
                {
                    count += tree.Nodes.IndexOf(currentNode) + 1;
                    break;
                }
            }
            
            return count;
        }

        /// <summary>
        /// Returns node based on the flat index. Flat index is the index of the node as if tree structure
        /// has been flattened into the list.
        /// </summary>
        /// <param name="advTree">Parent tree control.</param>
        /// <param name="index">Index to return node for.</param>
        /// <returns>Reference to a node or null if node at specified index cannot be found.</returns>
        internal static Node GetNodeByFlatIndex(AdvTree advTree, int index)
        {
            if (index == -1 || advTree.Nodes.Count == 0) return null;

            Node node = advTree.Nodes[0];
            int count = 0;
            while (node != null)
            {
                if (index == count)
                    return node;
                node = GetAnyNextNode(node);
                count++;
            }

            return null;
        }

        /// <summary>
        /// Finds the first node that starts with the specified text. Node.Text property is searched.
        /// </summary>
        /// <param name="advTree">Parent tree control.</param>
        /// <param name="text">Partial text to look for</param>
        /// <param name="startFromNode">Reference node to start searching from</param>
        /// <param name="ignoreCase">Gets or sets whether search ignores the letter case</param>
        /// <returns>Reference to a node or null if no node is found.</returns>
        internal static Node FindNodeByText(AdvTree advTree, string text, Node startFromNode, bool ignoreCase)
        {
            if (startFromNode == null)
                startFromNode = GetFirstVisibleNode(advTree);
            else
                startFromNode = GetAnyNextNode(startFromNode);
            if (startFromNode == null) return null;

            Node node = startFromNode;
#if FRAMEWORK20
            StringComparison comparison = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            while (node != null)
            {
                if (node.Text.StartsWith(text, comparison))
                    return node;
                node = GetAnyNextNode(node);
            }
#else
			if(ignoreCase)
			{
				text = text.ToLower();
				while (node != null)
				{
					if (node.Text.ToLower().StartsWith(text))
						return node;
					node = GetAnyNextNode(node);
				}
			}
			else
			{
				while (node != null)
				{
					if (node.Text.StartsWith(text))
						return node;
					node = GetAnyNextNode(node);
				}
			}
#endif
            return null;
        }

        internal static ColumnHeader GetCellColumnHeader(AdvTree tree, Cell cell)
        {
            int index = -1;
            if (cell.Parent != null) index = cell.Parent.Cells.IndexOf(cell);

            if (cell.Parent != null && cell.Parent.Parent != null && cell.Parent.Parent.NodesColumns.Count > 0 && index < cell.Parent.Parent.NodesColumns.Count)
            {
                return cell.Parent.Parent.NodesColumns[index];
            }

            if (tree != null && tree.Columns.Count > 0 && index < tree.Columns.Count)
                return tree.Columns[index];

            return null;
        }

        public static void InvalidateNodeLayout(Node node, bool invalidateChildNodes)
        {
            node.SizeChanged = true;
            if (invalidateChildNodes && node.HasChildNodes)
            {
                foreach (Node item in node.Nodes)
                {
                    InvalidateNodeLayout(item, true);
                }
            }
        }
    }

    /// <summary>
    /// Returned as information about the node or its column header at given coordinates.
    /// </summary>
    internal class NodeHitTestInfo
    {
        public Node NodeAt = null;
        public ColumnHeaderCollection ColumnsAt = null;
    }
}

