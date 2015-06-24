using System;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.AdvTree
{
    /// <summary>
    /// Defines the data for NodeDragFeedback event.
    /// </summary>
    public class TreeDragFeedbackEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether this drop location is accepted. Default value is true. You can set this to false to disable drop at this location.
        /// </summary>
        public bool AllowDrop = true;
        /// <summary>
        /// Gets or sets the parent node for the node that is being dragged. This can be null/nothing value to indicate a root top-level node that
        /// is in AdvTree.Nodes collection.
        /// </summary>
        public Node ParentNode = null;
        /// <summary>
        /// Gets or sets the new insert position inside of ParentNode.Nodes collection for the node being dragged. If InsertPosition is -1 
        /// the ParentNode refers to the current mouse over node and drag &amp; drop node will be added as child node to it.
        /// </summary>
        public int InsertPosition = 0;

        private Node _DragNode;
        /// <summary>
        /// Gets reference to the node being dragged.
        /// </summary>
        public Node DragNode
        {
            get { return _DragNode; }
#if FRAMEWORK20
            set
            {
            	_DragNode = value;
            }
#endif
        }

        /// <summary>
        /// Initializes a new instance of the TreeDragFeedbackEventArgs class.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="insertPosition"></param>
        public TreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node dragNode)
        {
            ParentNode = parentNode;
            InsertPosition = insertPosition;
            _DragNode = dragNode;
        }

        /// <summary>
        /// Initializes a new instance of the TreeDragFeedbackEventArgs class.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="insertPosition"></param>
        public TreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node dragNode, DragDropEffects effect)
        {
            ParentNode = parentNode;
            InsertPosition = insertPosition;
            _DragNode = dragNode;
            _Effect = effect;
        }

        /// <summary>
        /// Initializes a new instance of the TreeDragFeedbackEventArgs class.
        /// </summary>
        public TreeDragFeedbackEventArgs()
        {
        }

        internal bool EffectSet = false;
        private DragDropEffects _Effect = DragDropEffects.None;
        /// <summary>
        /// Gets or sets the drop effect for the drag-drop operation.
        /// </summary>
        public DragDropEffects Effect
        {
            get { return _Effect; }
            set 
            { 
                _Effect = value;
                EffectSet = true;
            }
        }
    }

    public class MultiNodeTreeDragFeedbackEventArgs : TreeDragFeedbackEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the TreeDragFeedbackEventArgs class.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="insertPosition"></param>
        public MultiNodeTreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node[] dragNodes) : 
            base(parentNode, insertPosition, dragNodes[0])
        {
            _DragNodes = dragNodes;
        }

        /// <summary>
        /// Initializes a new instance of the TreeDragFeedbackEventArgs class.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="insertPosition"></param>
        public MultiNodeTreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node[] dragNodes, DragDropEffects effect) : 
            base(parentNode, insertPosition, dragNodes[0], effect)
        {
            _DragNodes = dragNodes;
        }

        private Node[] _DragNodes;
        /// <summary>
        /// Gets reference to the node being dragged.
        /// </summary>
        public Node[] DragNodes
        {
            get { return _DragNodes; }
#if FRAMEWORK20
            set
            {
                _DragNodes = value;
            }
#endif
        }
    }
}
