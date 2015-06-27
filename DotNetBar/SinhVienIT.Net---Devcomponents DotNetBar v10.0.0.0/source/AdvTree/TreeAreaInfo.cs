namespace DevComponents.AdvTree
{
	/// <summary>
	/// Summary description for TreeAreaInfo.
	/// </summary>
	internal class TreeAreaInfo
	{
		/// <summary>
		/// Reference to parent node in which child bounds the coordinates are. Can be null if no parent node contains given coordinates.
		/// </summary>
		public Node ParentAreaNode=null;
		/// <summary>
		/// Node which contains specified coordinates. Can be null if no node contains coordinates.
		/// </summary>
		public Node NodeAt=null;
		/// <summary>
		/// Previous reference node for given coordinates. If coordinates fall between two nodes this will indicate previous node or null.
		/// </summary>
		public Node PreviousNode=null;
		/// <summary>
		/// Next reference node for given coordinates. If coordinates fall between two nodes this will indicate next node or null.
		/// </summary>
		public Node NextNode=null;
	}

    internal class NodeDragInfo
    {
        /// <summary>
        /// Gets or sets the parent node drag node will be added to. When null the drag node is being added as top-level node.
        /// </summary>
        public Node Parent = null;
        /// <summary>
        /// Gets or sets the insert index of drag node into the parent's node Nodes collection.
        /// </summary>
        public int InsertIndex = -1;

        /// <summary>
        /// Initializes a new instance of the NodeDragInfo class.
        /// </summary>
        public NodeDragInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the NodeDragInfo class.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="insertIndex"></param>
        public NodeDragInfo(Node parent, int insertIndex)
        {
            Parent = parent;
            InsertIndex = insertIndex;
        }

        public override string ToString()
        {
            return string.Format("NodeDragInfo-> Parent={0}, InsertIndex={1}", Parent, InsertIndex);
        }
    }
}
