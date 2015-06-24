namespace DevComponents.Tree
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
}
