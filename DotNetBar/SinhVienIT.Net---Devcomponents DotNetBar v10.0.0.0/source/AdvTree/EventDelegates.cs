using System;
using DevComponents.AdvTree.Display;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Defines the delegate for AdvTree cell based action events.
	/// </summary>
	public delegate void AdvTreeCellEventHandler(object sender, AdvTreeCellEventArgs e);
	
	/// <summary>
	/// Defines the delegate for AdvTree cell based action events.
	/// </summary>
	public delegate void AdvTreeCellCancelEventHandler(object sender, TreeCellCancelEventArgs e);
	
	/// <summary>
	/// Defines the delegate for AdvTree node based action events that can be cancelled.
	/// </summary>
	public delegate void AdvTreeNodeCancelEventHandler(object sender, AdvTreeNodeCancelEventArgs e);
	
	/// <summary>
	/// Defines the delegate for AdvTree node based action events.
	/// </summary>
	public delegate void AdvTreeNodeEventHandler(object sender, AdvTreeNodeEventArgs e);

	/// <summary>
	/// Defines delegate for Command button events.
	/// </summary>
	public delegate void CommandButtonEventHandler(object sender, CommandButtonEventArgs e);

	/// <summary>
	/// Defines delegate for label editing events.
	/// </summary>
	public delegate void CellEditEventHandler(object sender, CellEditEventArgs e);
	
	/// <summary>
	/// Defines the delegate for AdvTree node based action events.
	/// </summary>
	public delegate void TreeNodeCollectionEventHandler(object sender, TreeNodeCollectionEventArgs e);
	
	/// <summary>
	/// Defines the delegate for BeforeNodeDrop and AfterNodeDrop events
	/// </summary>
	public delegate void TreeDragDropEventHandler(object sender, TreeDragDropEventArgs e);

    /// <summary>
    /// Defines the delegate for NodeDragFeedback event.
    /// </summary>
    public delegate void TreeDragFeedbackEventHander(object sender, TreeDragFeedbackEventArgs e);
	
	/// <summary>
	/// Defines the delegate for mouse based node events
	/// </summary>
	public delegate void TreeNodeMouseEventHandler(object sender, TreeNodeMouseEventArgs e);
	
	/// <summary>
	/// Defines delegate for node rendering events.
	/// </summary>
	public delegate void NodeRendererEventHandler(object sender, NodeRendererEventArgs e);
	
	/// <summary>
	/// Defines delegate for cell rendering events.
	/// </summary>
	public delegate void NodeCellRendererEventHandler(object sender, NodeCellRendererEventArgs e);
	
	/// <summary>
	/// Defines delegate for RenderExpandPart event.
	/// </summary>
	public delegate void NodeExpandPartRendererEventHandler(object sender, NodeExpandPartRendererEventArgs e);
	
    ///// <summary>
    ///// Defines delegate for RenderExpandPart event.
    ///// </summary>
    //public delegate void NodeCommandPartRendererEventHandler(object sender, NodeCommandPartRendererEventArgs e);
	
	/// <summary>
	/// Defines delegate for RenderExpandPart event.
	/// </summary>
	public delegate void SelectionRendererEventHandler(object sender, SelectionRendererEventArgs e);
	/// <summary>
	/// Defines delegate for RenderConnector event.
	/// </summary>
	public delegate void ConnectorRendererEventHandler(object sender, ConnectorRendererEventArgs e);
	/// <summary>
	/// Defines delegate for TreeBackgroundRenderer events.
	/// </summary>
	public delegate void TreeBackgroundRendererEventHandler(object sender, TreeBackgroundRendererEventArgs e);
    /// <summary>
    /// Defines delegate for RenderDragDropMarker event.
    /// </summary>
    public delegate void DragDropMarkerRendererEventHandler(object sender, DragDropMarkerRendererEventArgs e);
    /// <summary>
    /// Defines delegate for RenderColumnHeader event.
    /// </summary>
    public delegate void ColumnHeaderRendererEventHandler(object sender, ColumnHeaderRendererEventArgs e);

    public delegate void AdvTreeCellBeforeCheckEventHandler(object sender, AdvTreeCellBeforeCheckEventArgs e);
}
