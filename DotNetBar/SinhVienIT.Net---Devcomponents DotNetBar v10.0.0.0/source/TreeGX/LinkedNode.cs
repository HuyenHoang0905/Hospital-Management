using System;
using System.ComponentModel;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents a description of linked node and any connector points that link nodes.
	/// </summary>
	public class LinkedNode
	{
		#region Private Variables
		private ConnectorPointsCollection m_ConnectorPoints=null;
		private Node m_Node=null;
		private Node m_Parent=null;
		#endregion
		
		#region Internal Implementation
		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public LinkedNode()
		{
			m_ConnectorPoints=new ConnectorPointsCollection();
		}
		
		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		/// <param name="linkedNode">Linkded node to initialize new instace with.</param>
		public LinkedNode(Node linkedNode) : this()
		{
			this.Node = linkedNode;
		}
		
		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		/// <param name="linkedNode">Linkded node to initialize new instace with.</param>
		/// <param name="connectorPoints">Connector points collection to initialize new instance with.</param>
		public LinkedNode(Node linkedNode, ConnectorPointsCollection connectorPoints) : this()
		{
			this.Node = linkedNode;
			if(connectorPoints!=null)
			{
				m_ConnectorPoints = connectorPoints;
				m_ConnectorPoints.SetParentNode(m_Parent);
			}
		}
		
		/// <summary>
		/// Gets or sets reference to linked node.
		/// </summary>
		/// <remarks>
		/// Linked nodes are nodes that are related to given node but do not have strict
		/// parent child relationship with the node. Each linked node must be already added as
		/// child node to some other node or it will not be displayed. Linked nodes are used in Map
		/// and Diagram layout styles to indicates relationships between nodes. Also See TreeGX.LinkConnector property.
		/// </remarks>
		[Browsable(true), DefaultValue(null)]
		public Node Node
		{
			get { return m_Node;}
			set { m_Node = value;}
		}
		
		/// <summary>
		/// Gets the collection of the link connector line relative points. If a node has a link to this node through LinkedNodes collection
		/// this these points outline the path connector will be drawn through from this node to it's parent linked node. By default this collection is empty which indicates that
		/// connector line is drawn using predefined path. Points added here are the points through which the connector line will travel to the
		/// parent node. The point coordinates added to this collection are relative from the top-left corner of this node.
		/// </summary>
		[Browsable(true),Category("Connectors"),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ConnectorPointsCollection ConnectorPoints
		{
			get {return m_ConnectorPoints;}
		}
		
		/// <summary>
		/// Gets the Node this linked node description is attached to.
		/// </summary>
		[Browsable(false)]
		public Node Parent
		{
			get { return m_Parent;}
		}
		
		/// <summary>
		/// Sets the parent node.
		/// </summary>
		/// <param name="node">Reference to parent node.</param>
		internal void SetParent(Node node)
		{
			m_Parent = node;
			m_ConnectorPoints.SetParentNode(node);
		}
		#endregion
	}
}
