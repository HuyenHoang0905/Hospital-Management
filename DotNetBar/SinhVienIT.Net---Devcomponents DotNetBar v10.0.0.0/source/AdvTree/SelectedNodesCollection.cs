using System;
using System.Text;

namespace DevComponents.AdvTree
{
    /// <summary>
    /// Represents the selected nodes collection.
    /// </summary>
    public class SelectedNodesCollection : NodeCollection
    {
        #region Internal Implementation
        internal AdvTree TreeSelectionControl = null;
        internal bool SuspendEvents = false;
        /// <summary>
        /// Initializes a new instance of the SelectedNodesCollection class.
        /// </summary>
        public SelectedNodesCollection()
        {
            PassiveCollection = true;
        }
        /// <summary>
        /// Adds new object to the collection and provides information about the source of the command
        /// </summary>
        /// <param name="node">Node to add</param>
        /// <param name="action">Source action</param>
        /// <returns></returns>
        public override int Add(Node node, eTreeAction action)
        {
            if (this.List.Contains(node)) return -1;

            if (TreeSelectionControl.MultiSelect)
            {
                if (TreeSelectionControl.MultiSelectRule == eMultiSelectRule.SameParent && this.Count>0)
                {
                    if (this[0].Parent != node.Parent)
                        throw new InvalidOperationException("Node being added does not belong to the same parent as currently selected nodes. See AdvTree.MultiSelectRule property");
                }
                if (!SuspendEvents)
                {
                    AdvTreeNodeCancelEventArgs cancelArgs = new AdvTreeNodeCancelEventArgs(action, node);
                    TreeSelectionControl.InvokeOnBeforeNodeSelect(cancelArgs);
                    if (cancelArgs.Cancel)
                        return -1;
                }
            }
            return base.Add(node, action);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (TreeSelectionControl.MultiSelect)
            {
                Node node = value as Node;
                node.IsSelected = true;
                TreeSelectionControl.InvalidateNode(node);
                if (node.SelectedCell == null)
                {
                    node.SelectFirstCell(SourceAction);
                }
                AdvTreeNodeEventArgs args = new AdvTreeNodeEventArgs(SourceAction, node);
                TreeSelectionControl.InvokeOnAfterNodeSelect(args);
                node.InternalSelected(this.SourceAction);
                if(!_MultiNodeOperation)
                    TreeSelectionControl.InvokeSelectionChanged(EventArgs.Empty);
            }
            base.OnInsertComplete(index, value);

        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (TreeSelectionControl.MultiSelect)
            {
                Node node = value as Node;
                node.IsSelected = false;
                TreeSelectionControl.InvalidateNode(node);
                if (node.SelectedCell != null)
                    node.SelectedCell.SetSelected(false, SourceAction);

                TreeSelectionControl.InvokeOnAfterNodeDeselect(new AdvTreeNodeEventArgs(this.SourceAction, node));
                node.InternalDeselected(this.SourceAction);
                if (!_MultiNodeOperation)
                    TreeSelectionControl.InvokeSelectionChanged(EventArgs.Empty);
            }
            base.OnRemoveComplete(index, value);
        }

        protected override void OnClear()
        {
            if (TreeSelectionControl.MultiSelect)
            {
                Node[] list = new Node[this.List.Count];
                this.List.CopyTo(list, 0);
                foreach (Node node in list)
                {
                    node.IsSelected = false;
                    TreeSelectionControl.InvalidateNode(node);
                    if (node.SelectedCell == null)
                        node.Cells[0].SetSelected(false, SourceAction);
                    TreeSelectionControl.InvokeOnAfterNodeDeselect(new AdvTreeNodeEventArgs(this.SourceAction, node));
                    node.InternalDeselected(this.SourceAction);
                }
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            if (TreeSelectionControl.MultiSelect)
            {
                TreeSelectionControl.InvokeSelectionChanged(EventArgs.Empty);
            }
            base.OnClearComplete();
        }

        private bool _MultiNodeOperation;
        internal bool MultiNodeOperation
        {
            get
            {
                return _MultiNodeOperation;
            }
            set
            {
                _MultiNodeOperation = value;
            }
        }
        #endregion
        
    }
}
