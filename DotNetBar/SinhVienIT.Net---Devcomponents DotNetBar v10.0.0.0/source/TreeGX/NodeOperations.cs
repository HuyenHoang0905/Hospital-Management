using System;
using System.Drawing;
using System.Text;
using DevComponents.Tree.Display;

namespace DevComponents.Tree
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
				sb.Insert(0,pathSeparator+node.Text);
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
		/// Gets the next sibling tree node. The NextNode is the next sibling Node in the NodeCollection stored in the Nodes property of the tree node's parent Node. If there is no next tree node, the NextNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Node object or null if node cannot be found.</returns>
		public static Node GetNextNode(Node node)
		{
            if (node == null)
                return null;
            NodeCollection parentNodes = null;
            if (node.Parent != null)
                parentNodes = node.Parent.Nodes;
            else if (node.internalTreeControl != null)
                parentNodes = node.internalTreeControl.Nodes;

            if (parentNodes != null)
            {
                int index = parentNodes.IndexOf(node);
                if (index < parentNodes.Count - 1)
                    return parentNodes[index + 1];
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
		/// Gets the next visible tree node. The NextVisibleNode can be a child, sibling, or a tree node from another branch. If there is no next tree node, the NextVisibleNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Node object or null if node cannot be found.</returns>
		public static Node GetNextVisibleNode(Node node)
		{
            Node nextVisibleNode = null;

            // First see whether all parent nodes are expanded and if not find the first one that is
            Node parent = node.Parent;
            Node lastNotExpanded = null;
            while (parent != null)
            {
                if (!parent.Expanded)
                    lastNotExpanded = parent;
                parent = parent.Parent;
            }
            if (lastNotExpanded != null)
            {
                // Last parent node expanded
                if (lastNotExpanded.Parent != null)
                    lastNotExpanded = lastNotExpanded.Parent;
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
			TreeGX tree=node.TreeControl;
			if(tree!=null)
			{
                if (tree.Zoom == 1)
                {
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, tree.NodeDisplay.Offset);
                    return tree.ClientRectangle.IntersectsWith(r);
                }
                else
                {
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, tree.NodeDisplay.Offset);
                    Rectangle clientRect = tree.GetLayoutRectangle(tree.ClientRectangle);
                    return clientRect.IntersectsWith(r);
                }
			}
			return false;
		}

        /// <summary>
        /// Returns last rendered node on screen.
        /// </summary>
        /// <param name="tree">Tree control.</param>
        /// <returns>Last rendered node or null</returns>
        public static Node GetLastDisplayedNode(TreeGX tree)
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
		/// Gets the zero based index of position of the tree node in the tree node collection. -1 is returned if node is not added to the nodes collection.
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Zero based index or -1 if node is not in collection.</returns>
		public static int GetNodeIndex(Node node)
		{
			if(node.Parent==null)
				return -1;
			return node.Parent.Nodes.IndexOf(node);
		}

        /// <summary>
        /// Gets the previous sibling tree node. The PrevNode is the previous sibling Node in the NodeCollection stored in the Nodes property of the tree node's parent Node. If there is no previous tree node, the PrevNode property returns a null reference (Nothing in Visual Basic).
        /// </summary>
        /// <param name="node">Reference node.</param>
        /// <returns>Node object or null if node cannot be found.</returns>
        public static Node GetPreviousNode(Node node)
        {
            if (node == null)
                return null;
            NodeCollection parentNodes = null;
            if (node.Parent != null)
                parentNodes = node.Parent.Nodes;
            else if (node.internalTreeControl != null)
                parentNodes = node.internalTreeControl.Nodes;

            if (parentNodes != null)
            {
                int index = parentNodes.IndexOf(node);
                if (index > 0)
                    return parentNodes[index - 1];
            }

            return null;
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
		/// Gets the previous visible tree node. The PrevVisibleNode can be a child, sibling, or a tree node from another branch. If there is no previous tree node, the PrevVisibleNode property returns a null reference (Nothing in Visual Basic).
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>Node object or null if node cannot be found.</returns>
		public static Node GetPreviousVisibleNode(Node node)
		{
            Node prevVisibleNode = null;

            // First see whether all parent nodes are expanded and if not find the first one that is
            Node referenceNode = node.Parent;
            Node lastNotExpanded = null;
            while (referenceNode != null)
            {
                if (!referenceNode.Expanded)
                    lastNotExpanded = referenceNode;
                referenceNode = referenceNode.Parent;
            }
            if (lastNotExpanded != null)
            {
                if (lastNotExpanded.IsVisible) return lastNotExpanded;
                referenceNode = lastNotExpanded;
            }
            else
                referenceNode = node;

            // Walk into the previous node parent
            if (referenceNode.Parent != null)
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

            if (prevVisibleNode == null)
                return null;

            // Walk into the node
            while (prevVisibleNode.Expanded && prevVisibleNode.Nodes.Count > 0)
            {
                prevVisibleNode = prevVisibleNode.Nodes[prevVisibleNode.Nodes.Count - 1];
            }

            return prevVisibleNode;
		}

		/// <summary>
		/// Returns true if node passed is considered root node for display purposes.
		/// </summary>
		/// <param name="tree">Reference to the tree control.</param>
		/// <param name="node">Node to test.</param>
		/// <returns>true if node is root node for display purposes otherwise false.</returns>
		public static bool IsRootNode(TreeGX tree, Node node)
		{
			if(node.Parent==null || node==tree.DisplayRootNode)
				return true;
			return false;
		}

		/// <summary>
		/// Ensures that the node is visible, expanding nodes and scrolling the control as necessary.
		/// </summary>
		/// <param name="reference">Node to be made visible.</param>
		public static void EnsureVisible(Node reference)
		{
			// First expand all parents
			TreeGX tree=reference.TreeControl;
			if(tree!=null)
				tree.SuspendPaint=true;
			try
			{
				Node node=reference;
				bool layout=false;
				while(node.Parent!=null)
				{
					if(!node.Parent.Expanded)
					{
						node.Parent.Expand();
						layout=true;
					}
					node=node.Parent;
				}

				if(layout)
					tree.RecalcLayoutInternal();

				Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,reference,tree.NodeDisplay.Offset);
				if(tree!=null && tree.AutoScroll)
				{	
					if(tree.Zoom!=1)
					{
						r = tree.GetScreenRectangle(r);
					}
					r.Offset(-tree.AutoScrollPosition.X,-tree.AutoScrollPosition.Y);

                    Rectangle treeClientRectangle = tree.GetInnerRectangle();
					if(!treeClientRectangle.Contains(r))
					{
						Point p=Point.Empty;
						if(r.Right>treeClientRectangle.Right)
							p.X=r.Right-treeClientRectangle.Right;
						else if(r.X<treeClientRectangle.X)
							p.X=r.X;
						if(r.Bottom>treeClientRectangle.Bottom)
                            p.Y = r.Y - r.Height;
						else if(r.Y<treeClientRectangle.Y)
							p.Y=r.Y;
						// Scroll it to show the node
						tree.AutoScrollPosition=p;
					}
				}
			}
			finally
			{
				if(tree!=null)
					tree.SuspendPaint=false;
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
        /// Gets first visible node.
        /// </summary>
        /// <param name="tree">Reference to tree.</param>
        /// <returns>Last visible node found or null</returns>
        public static Node GetFirstVisibleNode(TreeGX tree)
        {
            if (tree.Nodes.Count == 0) return null;

            Node node = tree.DisplayRootNode == null ? tree.Nodes[0] : tree.DisplayRootNode;
            if (node.Visible)
                return node;
            return GetNextVisibleNode(node);
        }

		/// <summary>
		/// Returns true if node has at least single visible child node.
		/// </summary>
		/// <param name="node">Reference node.</param>
		/// <returns>True if at least single child node is visible otherwise false.</returns>
		public static bool GetAnyVisibleNodes(Node node)
		{
			if(node.Nodes.Count==0)
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
		public static Node GetNodeAt(TreeGX tree, Point p)
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
		public static Node GetNodeAt(TreeGX tree, int x, int y)
		{
            if (tree.Nodes.Count == 0 || !tree.DisplayRectangle.Contains(x, y) && tree.Zoom == 1)
                return null;

			Node node=tree.Nodes[0];
			Node retNode=null;
			if(tree.DisplayRootNode!=null)
				node=tree.DisplayRootNode;
			while(node!=null)
			{
				Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,node,tree.NodeDisplay.Offset);
				if(r.Contains(x,y))
				{
					retNode=node;
					break;
				}
				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds,node,tree.NodeDisplay.Offset);
				if(!r.Contains(x,y))
					node = GetNextVisibleSibling(node);
				else
					node=node.NextVisibleNode;
			}

			return retNode;
		}
		
		public static TreeAreaInfo GetTreeAreaInfo(TreeGX tree, int x, int y)
		{
			TreeAreaInfo areaInfo=new TreeAreaInfo();

            if (tree.Nodes.Count == 0 || !tree.DisplayRectangle.Contains(x, y) && tree.Zoom == 1)
				return null;

			Node node=tree.Nodes[0];
			if(tree.DisplayRootNode!=null)
				node = tree.DisplayRootNode;
			
			Node dragNode = tree.GetDragNode();
			Rectangle dragNodeBounds=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,dragNode,tree.NodeDisplay.Offset);
			if(dragNodeBounds.Contains(x,y))
			{
				areaInfo.NodeAt=dragNode;
				return areaInfo;
			}

			int dragNodeWidth = dragNode.BoundsRelative.Width;
			Node previousNode = null;
			Rectangle previousRect=Rectangle.Empty;
			
			if(tree.DisplayRootNode!=null)
				node=tree.DisplayRootNode;
			while(node!=null)
			{
				if(!node.IsDisplayed)
				{
					node = node.NextVisibleNode;
					continue;
				}
				Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,node,tree.NodeDisplay.Offset);
				if(r.Contains(x,y))
				{
					areaInfo.NodeAt = node;
					break;
				}
				
				if(previousNode!=null && node.Parent==previousNode.Parent)
				{
					Rectangle totalArea;
					if(r.Width<dragNodeWidth || previousRect.Width<dragNodeWidth)
					{
						totalArea =
							Rectangle.Union(new Rectangle(r.Location, new Size(dragNodeWidth, r.Height)),
							                new Rectangle(previousRect.Location, new Size(dragNodeWidth, previousRect.Height)));
					}
					else
						totalArea=Rectangle.Union(r,previousRect);
					if(totalArea.Contains(x,y))
					{
						Rectangle r1 = r;
						Rectangle r2 = previousRect;
						if(totalArea.Width>r1.Width) r1.Width = totalArea.Width;
						if(totalArea.Width>r2.Width) r2.Width = totalArea.Width;
						if(!r1.Contains(x,y) && !r2.Contains(x,y))
						{
							areaInfo.PreviousNode = previousNode;
							areaInfo.NextNode = node;
							areaInfo.NodeAt = null;
							areaInfo.ParentAreaNode = node.Parent;
							break;
						}
					}
				}
				
				previousNode = node;
				previousRect = r;
				
                //r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds,node,tree.NodeDisplay.Offset);

                //if (!r.Contains(x, y))
                //    node = GetNextVisibleSibling(node);
                //else
				node = node.NextVisibleNode;
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
			// {EB364C34-3CE3-4cd6-BB1B-13513ABE0D62}, 114
			string[] parts = key.Split('-');
			int test = 0;
			for(int i=parts.Length-1;i>=0;i--)
			{
				if(parts[i]=="3CE3")
					test += 12;
				else if(parts[i]=="13513ABE0D62")
					test += 2;
				else if(parts[i]=="4cd6")
					test += 3;
				else if(parts[i]=="BB1B")
					test += 7;
				else if(parts[i]=="EB364C34")
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
				key=key.CreateSubKey("CLSID\\{EB84498D-DBDA-4806-A10C-B668BD4F1BB1}\\InprocServer32");
			
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
	}
}
