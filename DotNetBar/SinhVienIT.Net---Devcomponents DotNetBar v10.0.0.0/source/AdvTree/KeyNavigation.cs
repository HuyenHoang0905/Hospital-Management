using System;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides AdvTree Keyboard handling.
	/// </summary>
	internal class KeyNavigation
	{
		public static void KeyDown(AdvTree tree, KeyEventArgs e)
		{
            Node node = tree.SelectedNode;
            if (node != null)
            {
                node.InternalKeyDown(e);
                if (e.Handled) return;
            }
            
			switch(e.KeyCode)
			{
				case Keys.Space:
					{
						SpaceKeyDown(tree, e);
						e.Handled = true;
						break;
					}
                case Keys.Add:
                    {
                        e.Handled = PlusKeyDown(tree, e);
                        break;
                    }
                case Keys.Subtract:
                    {
                        e.Handled = MinusKeyDown(tree, e);
                        break;
                    }
                case Keys.F2:
                    {
                        F2KeyDown(tree, e);
                        break;
                    }
                default:
			        {
                        if (e.Control && e.KeyCode == Keys.C)
                            CopyKeyDown(tree, e);
                        else if (e.Control && e.KeyCode == Keys.V)
                            PasteKeyDown(tree, e);
			            break;
			        }
			}
		}

        private static bool MinusKeyDown(AdvTree tree, KeyEventArgs e)
        {
            Node node = tree.SelectedNode;
            if (node != null && tree.SelectedNodes.Count == 1 && node.Expanded)
            {
                node.Collapse();
                return true;
            }
            return false;
        }
        private static bool PlusKeyDown(AdvTree tree, KeyEventArgs e)
        {
            Node node = tree.SelectedNode;
            if (node != null && tree.SelectedNodes.Count == 1 && !node.Expanded)
            {
                node.Expand();
                return true;
            }
            return false;
        }

        private static void PasteKeyDown(AdvTree tree, KeyEventArgs args)
	    {
            if (tree.SelectedNode != null)
                tree.SelectedNode.InvokeKeyboardPaste(args);
	    }

	    private static void CopyKeyDown(AdvTree tree, KeyEventArgs args)
	    {
            if (tree.SelectedNode != null)
                tree.SelectedNode.InvokeKeyboardCopy(args);
	    }

	    private static void F2KeyDown(AdvTree tree, KeyEventArgs e)
        {
            if (tree.EditSelectedCell(eTreeAction.Keyboard))
                e.Handled = true;
        }

        public static void SpaceKeyDown(AdvTree tree, KeyEventArgs e)
        {
            Node node = tree.SelectedNode;
            if (node != null && node.CheckBoxVisible && node.Enabled)
            {
                if (node.CheckBoxThreeState)
                {
                    if (node.CheckState == CheckState.Checked)
                        node.SetChecked(CheckState.Indeterminate, eTreeAction.Keyboard);
                    else if (node.CheckState == CheckState.Unchecked)
                        node.SetChecked(CheckState.Checked, eTreeAction.Keyboard);
                    else if (node.CheckState == CheckState.Indeterminate)
                        node.SetChecked(CheckState.Unchecked, eTreeAction.Keyboard);
                }
                else
                    node.SetChecked(!node.Checked, eTreeAction.Keyboard);
                e.Handled = true;
            }
        }
		
		public static void EnterKeyDown(AdvTree tree, KeyEventArgs e)
		{
            if (tree.SelectedNode != null && tree.SelectedNode.Nodes.Count > 0)
            {
                tree.SelectedNode.Toggle(eTreeAction.Keyboard);
            }
		}
		
		public static bool NavigateKeyDown(AdvTree tree, KeyEventArgs e)
		{
			if(tree.SelectedNode==null)
			{
				if(tree.DisplayRootNode!=null)
					tree.SelectNode(tree.DisplayRootNode, eTreeAction.Keyboard);
				else if(tree.Nodes.Count>0)
					tree.SelectNode(tree.Nodes[0], eTreeAction.Keyboard);
				return true;
			}

            Node node = tree.SelectedNode;
            if (node != null && !node.IsKeyboardNavigationEnabled(e))
            {
                return false;
            }

            if(e.KeyCode == Keys.Right)
			{
                if (node != null && node.Cells.Count > 1 && tree.SelectionPerCell && node.CellNavigationEnabled)
                {
                    if (node.SelectedCell == null)
                        node.SetSelectedCell(NodeOperations.GetNextVisibleCell(node, -1), eTreeAction.Keyboard);
                    else
                        node.SetSelectedCell(NodeOperations.GetNextVisibleCell(node, node.Cells.IndexOf(node.SelectedCell)), eTreeAction.Keyboard);
                    return true; 
                }

                if (tree.View == eView.Tile)
                {
                    Node nextNode = NodeOperations.GetNextVisibleNode(node);
                    if (nextNode != null)
                    {
                        if (node.Expanded)
                            tree.SelectNode(nextNode, eTreeAction.Keyboard);
                        else
                            node.Expand(eTreeAction.Keyboard);
                    }
                    else if (node != null && node.ExpandVisibility == eNodeExpandVisibility.Visible && !node.Expanded)
                        node.Expand(eTreeAction.Keyboard);
                }
                else
                {
                    Node childNode = NodeOperations.GetFirstVisibleChildNode(node);
                    if (childNode != null)
                    {
                        if (node.Expanded)
                            tree.SelectNode(childNode, eTreeAction.Keyboard);
                        else
                            node.Expand(eTreeAction.Keyboard);
                    }
                    else if (node != null && node.ExpandVisibility == eNodeExpandVisibility.Visible && !node.Expanded)
                        node.Expand(eTreeAction.Keyboard);
                }
			}
			else if(e.KeyCode == Keys.Left)
			{
                if (node != null && node.Cells.Count > 1 && tree.SelectionPerCell && node.CellNavigationEnabled)
                {
                    if (node.SelectedCell == null)
                        node.SetSelectedCell(NodeOperations.GetPreviousVisibleCell(node, node.Cells.Count - 1), eTreeAction.Keyboard);
                    else 
                        node.SetSelectedCell(NodeOperations.GetPreviousVisibleCell(node, node.Cells.IndexOf(node.SelectedCell)), eTreeAction.Keyboard);
                    return true;
                }

                if (tree.View == eView.Tile)
                {
                    Node previousNode = NodeOperations.GetPreviousVisibleNode(node);
                    if (previousNode != null)
                        tree.SelectNode(previousNode, eTreeAction.Keyboard);
                }
                else
                {
                    if (node.Expanded && node.IsSelected && NodeOperations.GetFirstVisibleChildNode(node) != null)
                        node.Collapse(eTreeAction.Keyboard);
                    else if (node.Parent != null)
                        tree.SelectNode(node.Parent, eTreeAction.Keyboard);
                }
			}
            else if (e.KeyCode == Keys.End)
            {
                Node last = NodeOperations.GetLastVisibleNode(tree);
                if (last != null)
                {
                    if (!last.Selectable)
                    {
                        while (last != null)
                        {
                            last = NodeOperations.GetPreviousVisibleNode(last);
                            if (last!=null && last.Selectable) break;
                        }
                    }
                    if (last != null)
                        tree.SelectNode(last, eTreeAction.Keyboard);
                }
            }
            else if (e.KeyCode == Keys.Home || e.KeyCode == Keys.PageDown && node == null)
            {
                Node first = NodeOperations.GetFirstVisibleNode(tree);
                if (first != null)
                {
                    if (!first.Selectable)
                    {
                        while (first != null)
                        {
                            first = NodeOperations.GetNextVisibleNode(first);
                            if (first != null && first.Selectable) break;
                        }
                    }
                    if (first != null)
                        tree.SelectNode(first, eTreeAction.Keyboard);
                }
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                // Find last fully rendered node
                Node lastNode = NodeOperations.GetLastDisplayedNode(tree);
                if (lastNode != null)
                {
                    if (tree.SelectedNode == lastNode)
                    {
                        if (tree.VScrollBar != null && tree.AutoScroll)
                        {
                            tree.AutoScrollPosition = new Point(tree.AutoScrollPosition.X, Math.Max(tree.AutoScrollPosition.Y - tree.VScrollBar.LargeChange, -(tree.VScrollBar.Maximum - tree.VScrollBar.LargeChange)));
                            lastNode = NodeOperations.GetLastDisplayedNode(tree);
                        }
                    }
                }
                if (lastNode != null)
                    tree.SelectNode(lastNode, eTreeAction.Keyboard);
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                // Find last fully rendered node
                Node firstNode = NodeOperations.GetFirstDisplayedNode(tree);
                if (firstNode != null)
                {
                    if (tree.SelectedNode == firstNode)
                    {
                        if (tree.VScrollBar != null && tree.AutoScroll && tree.AutoScrollPosition.Y < 0)
                        {
                            tree.AutoScrollPosition = new Point(tree.AutoScrollPosition.X, Math.Min(0, tree.AutoScrollPosition.Y + tree.VScrollBar.LargeChange));
                            firstNode = NodeOperations.GetFirstDisplayedNode(tree);
                        }
                    }
                }
                if (firstNode != null)
                    tree.SelectNode(firstNode, eTreeAction.Keyboard);
            }
            else if ((e.KeyCode & Keys.Down) == Keys.Down)
            {
                int currentCell = 0;

                if (node != null && node.SelectedCell != null) currentCell = node.Cells.IndexOf(node.SelectedCell);

                Node nextNode = NodeOperations.GetNextVisibleNode(node);

                // Adjust nextNode so the multi-selection is proper
                if ((e.KeyData & Keys.Shift) == Keys.Shift && tree.MultiSelect && tree.SelectedNodes.Count > 1)
                {
                    if (tree.SelectedNodes[0].Bounds.Y > tree.SelectedNodes[tree.SelectedNodes.Count - 1].Bounds.Y)
                        nextNode = tree.SelectedNodes[tree.SelectedNodes.Count - 1];
                }

                if (nextNode != null)
                {
                    if (!nextNode.CanSelect)
                    {
                        int counter = 0;
                        while (nextNode != null && counter < 100)
                        {
                            nextNode = NodeOperations.GetNextVisibleNode(nextNode);
                            if (nextNode != null && nextNode.CanSelect) break;
                        }
                    }
                    if ((e.KeyData & Keys.Shift) == Keys.Shift && tree.MultiSelect && tree.SelectedNodes.Count > 0)
                    {
                        if (tree.MultiSelectRule == eMultiSelectRule.SameParent && tree.SelectedNodes[0].Parent != nextNode.Parent) return true;
                    
                        if (nextNode.IsSelected)
                            tree.SelectedNodes.Remove(nextNode, eTreeAction.Keyboard);
                        else
                            tree.SelectedNodes.Add(nextNode, eTreeAction.Keyboard);
                        nextNode.EnsureVisible();
                    }
                    else
                    {
                        tree.SelectNode(nextNode, eTreeAction.Keyboard);
                        if (tree.SelectionPerCell && currentCell < nextNode.Cells.Count && currentCell > 0)
                            nextNode.SetSelectedCell(nextNode.Cells[currentCell], eTreeAction.Keyboard);
                    }
                }
            }
            else if ((e.KeyCode & Keys.Up) == Keys.Up)
            {
                int currentCell = 0;
                if (node != null && node.SelectedCell != null) currentCell = node.Cells.IndexOf(node.SelectedCell);

                Node prevNode = NodeOperations.GetPreviousVisibleNode(node);
                if (prevNode != null)
                {
                    if (!prevNode.CanSelect)
                    {
                        int counter = 0;
                        while (prevNode != null && counter < 100)
                        {
                            prevNode = NodeOperations.GetPreviousVisibleNode(prevNode);
                            if (prevNode != null && prevNode.CanSelect) break;
                        }
                    }
                    if ((e.KeyData & Keys.Shift) == Keys.Shift && tree.MultiSelect && tree.SelectedNodes.Count > 0)
                    {
                        if (tree.MultiSelectRule == eMultiSelectRule.SameParent && tree.SelectedNodes[0].Parent != prevNode.Parent) return true;
                        if (prevNode.IsSelected)
                        {
                            tree.SelectedNodes.Remove(tree.SelectedNodes[tree.SelectedNodes.Count - 1], eTreeAction.Keyboard);
                        }
                        else
                            tree.SelectedNodes.Add(prevNode, eTreeAction.Keyboard);
                        prevNode.EnsureVisible();
                    }
                    else if (prevNode != null)
                    {
                        tree.SelectNode(prevNode, eTreeAction.Keyboard);
                        if (tree.SelectionPerCell && currentCell < prevNode.Cells.Count && currentCell > 0)
                            prevNode.SetSelectedCell(prevNode.Cells[currentCell], eTreeAction.Keyboard);
                    }
                }
            }
            return true;
		}
	}
}
