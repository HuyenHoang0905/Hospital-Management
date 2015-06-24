using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents helper class for node connector display.
	/// </summary>
	public class ConnectorRendererEventArgs:EventArgs
	{
		/// <summary>
		/// From node reference.
		/// </summary>
		public Node FromNode=null;
		/// <summary>
		/// From node style reference.
		/// </summary>
		public ElementStyle StyleFromNode=null;
		/// <summary>
		/// To node reference.
		/// </summary>
		public Node ToNode=null;
		/// <summary>
		/// To node style reference.
		/// </summary>
		public ElementStyle StyleToNode=null;
		/// <summary>
		/// Graphics object used for drawing.
		/// </summary>
		public System.Drawing.Graphics Graphics=null;
		/// <summary>
		/// Node offset since some node coordinates are relative.
		/// </summary>
		public Point Offset=Point.Empty;
		/// <summary>
		/// Indicates whether from node is a root node.
		/// </summary>
		public bool IsRootNode=false;
		/// <summary>
		/// Reference to node connector object that describes connector type.
		/// </summary>
		public NodeConnector NodeConnector=null;
		/// <summary>
		/// Gets or sets whether connector is link connector.
		/// </summary>
		public bool LinkConnector=false;
		/// <summary>
		/// Reference to the collection of the connector path points. Default value is null indicating there are no path points.
		/// </summary>
		public ConnectorPointsCollection ConnectorPoints=null;
	}
}
