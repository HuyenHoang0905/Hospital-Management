using System;
using DevComponents.Tree.Display;

namespace DevComponents.Tree
{
	/// <summary>
	/// Defines the delegate for TreeGX cell based action events.
	/// </summary>
	public delegate void TreeGXCellEventHandler(object sender, TreeGXCellEventArgs e);
	
	/// <summary>
	/// Defines the delegate for TreeGX cell based action events.
	/// </summary>
	public delegate void TreeGXCellCancelEventHandler(object sender, TreeGXCellCancelEventArgs e);
	
	/// <summary>
	/// Defines the delegate for TreeGX node based action events that can be cancelled.
	/// </summary>
	public delegate void TreeGXNodeCancelEventHandler(object sender, TreeGXNodeCancelEventArgs e);
	
	/// <summary>
	/// Defines the delegate for TreeGX node based action events.
	/// </summary>
	public delegate void TreeGXNodeEventHandler(object sender, TreeGXNodeEventArgs e);

	/// <summary>
	/// Defines delegate for Command button events.
	/// </summary>
	public delegate void CommandButtonEventHandler(object sender, CommandButtonEventArgs e);

	/// <summary>
	/// Defines delegate for label editing events.
	/// </summary>
	public delegate void CellEditEventHandler(object sender, CellEditEventArgs e);
	
	/// <summary>
	/// Defines the delegate for TreeGX node based action events.
	/// </summary>
	public delegate void TreeGXNodeCollectionEventHandler(object sender, TreeGXNodeCollectionEventArgs e);
	
	/// <summary>
	/// Defines the delegate for BeforeNodeDrop and AfterNodeDrop events
	/// </summary>
	public delegate void TreeGXDragDropEventHandler(object sender, TreeGXDragDropEventArgs e);
	
	/// <summary>
	/// Defines the delegate for mouse based node events
	/// </summary>
	public delegate void TreeGXNodeMouseEventHandler(object sender, TreeGXNodeMouseEventArgs e);
	
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
	
	/// <summary>
	/// Defines delegate for RenderExpandPart event.
	/// </summary>
	public delegate void NodeCommandPartRendererEventHandler(object sender, NodeCommandPartRendererEventArgs e);
	
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
}
