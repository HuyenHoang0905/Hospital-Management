using System;
using System.Windows.Forms;

namespace DevComponents.Tree
{
	/// <summary>
	/// Provides TreeGX Keyboard handling.
	/// </summary>
	internal class KeyNavigation
	{
		public static void KeyDown(TreeGX tree, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Enter:
					{
						EnterKeyDown(tree, e);
						e.Handled = true;
						break;
					}
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
					{
						NavigateKeyDown(tree, e);
						e.Handled = true;
						break;
					}
			}
		}
		
		public static void EnterKeyDown(TreeGX tree, KeyEventArgs e)
		{
			if(tree.SelectedNode!=null && tree.SelectedNode.Nodes.Count>0)
			{
				tree.SelectedNode.Toggle(eTreeAction.Keyboard);
			}
		}
		
		public static void NavigateKeyDown(TreeGX tree, KeyEventArgs e)
		{
			if(tree.SelectedNode==null)
			{
				if(tree.DisplayRootNode!=null)
					tree.SelectNode(tree.DisplayRootNode, eTreeAction.Keyboard);
				else if(tree.Nodes.Count>0)
					tree.SelectNode(tree.Nodes[0], eTreeAction.Keyboard);
				return;
			}
			
			Node node=tree.SelectedNode;
			
			if(e.KeyCode == Keys.Down || e.KeyCode == Keys.Right && NodeOperations.GetVisibleChildNodesCount(node) == 0)
			{
                int currentCell = 0;

                if (node != null && node.SelectedCell != null) currentCell = node.Cells.IndexOf(node.SelectedCell);

                Node nextNode = NodeOperations.GetNextVisibleNode(node);

                //// Adjust nextNode so the multi-selection is proper
                //if ((e.KeyData & Keys.Shift) == Keys.Shift && tree.MultiSelect && tree.SelectedNodes.Count > 1)
                //{
                //    if (tree.SelectedNodes[0].Bounds.Y > tree.SelectedNodes[tree.SelectedNodes.Count - 1].Bounds.Y)
                //        nextNode = tree.SelectedNodes[tree.SelectedNodes.Count - 1];
                //}

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
                    //if ((e.KeyData & Keys.Shift) == Keys.Shift && tree.MultiSelect && tree.SelectedNodes.Count > 0)
                    //{
                    //    if (tree.MultiSelectRule == eMultiSelectRule.SameParent && tree.SelectedNodes[0].Parent != nextNode.Parent) return true;

                    //    if (nextNode.IsSelected)
                    //        tree.SelectedNodes.Remove(nextNode, eTreeAction.Keyboard);
                    //    else
                    //        tree.SelectedNodes.Add(nextNode, eTreeAction.Keyboard);
                    //    nextNode.EnsureVisible();
                    //}
                    //else
                    {
                        tree.SelectNode(nextNode, eTreeAction.Keyboard);
                        //if (tree.SelectionPerCell && currentCell < nextNode.Cells.Count && currentCell > 0)
                        //    nextNode.SetSelectedCell(nextNode.Cells[currentCell], eTreeAction.Keyboard);
                    }
                }
			}
			else if(e.KeyCode == Keys.Up || e.KeyCode == Keys.Left && node.Parent == null)
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
                    //if ((e.KeyData & Keys.Shift) == Keys.Shift && tree.MultiSelect && tree.SelectedNodes.Count > 0)
                    //{
                    //    if (tree.MultiSelectRule == eMultiSelectRule.SameParent && tree.SelectedNodes[0].Parent != prevNode.Parent) return true;
                    //    if (prevNode.IsSelected)
                    //    {
                    //        tree.SelectedNodes.Remove(tree.SelectedNodes[tree.SelectedNodes.Count - 1], eTreeAction.Keyboard);
                    //    }
                    //    else
                    //        tree.SelectedNodes.Add(prevNode, eTreeAction.Keyboard);
                    //    prevNode.EnsureVisible();
                    //}
                    //else 
                    if (prevNode != null)
                    {
                        tree.SelectNode(prevNode, eTreeAction.Keyboard);
                        //if (tree.SelectionPerCell && currentCell < prevNode.Cells.Count && currentCell > 0)
                        //    prevNode.SetSelectedCell(prevNode.Cells[currentCell], eTreeAction.Keyboard);
                    }
                }
			}
			else if(e.KeyCode == Keys.Right)
			{
				Node childNode = NodeOperations.GetFirstVisibleChildNode(node);
				if(childNode!=null)
					tree.SelectNode(childNode, eTreeAction.Keyboard);
			}
			else if(e.KeyCode == Keys.Left)
			{
				if(node.Parent!=null)
					tree.SelectNode(node.Parent, eTreeAction.Keyboard);
			}
		}
	}
}
