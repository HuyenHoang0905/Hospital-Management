using System;
using System.Xml;
using System.IO;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides means for AdvTree serialization.
	/// </summary>
	public class TreeSerializer
	{
		#region Private Variables
		private static string XmlNodeName="Node";
		private static string XmlCellsName="Cells";
		private static string XmlCellName="Cell";
		private static string XmlCellImagesName="Images";
		private static string XmlAdvTreeName="AdvTree";
		private static string XmlCustomName = "Custom";
		#endregion
		
		#region Saving
		/// <summary>
		/// Saves Nodes to specified file.
		/// </summary>
		/// <param name="tree">AdvTree to save</param>
		/// <param name="fileName">Target file name</param>
		public static void Save(AdvTree tree, string fileName)
		{
			XmlDocument document=Save(tree);
			document.Save(fileName);
		}
		
		/// <summary>
		/// Saves Nodes to stream.
		/// </summary>
		/// <param name="tree">AdvTree to save</param>
		/// <param name="outStream">Stream to save nodes to.</param>
		public static void Save(AdvTree tree, Stream outStream)
		{
			XmlDocument document=Save(tree);
			document.Save(outStream);
		}
		
		/// <summary>
		/// Saves Nodes to TextWriter
		/// </summary>
		/// <param name="tree">AdvTree to save</param>
		/// <param name="writer">TextWriter to write nodes to.</param>
		public static void Save(AdvTree tree, TextWriter writer)
		{
			XmlDocument document=Save(tree);
			document.Save(writer);
		}
		
		/// <summary>
		/// Saves nodes to XmlWriter.
		/// </summary>
		/// <param name="tree">AdvTree to save</param>
		/// <param name="writer">XmlWriter to write nodes to</param>
		public static void Save(AdvTree tree, XmlWriter writer)
		{
			XmlDocument document=Save(tree);
			document.Save(writer);
		}
		
		/// <summary>
		/// Creates new XmlDocument and serializes AdvTree into it.
		/// </summary>
		/// <param name="tree">AdvTree to serialize</param>
		/// <returns>New instance of XmlDocument/returns>
		public static XmlDocument Save(AdvTree tree)
		{
			XmlDocument document=new XmlDocument();
			Save(tree, document);
			return document;
		}
		
		/// <summary>
		/// Saves AdvTree to an existing XmlDocument. New node AdvTree is created in document and Nodes are serialized into it.
		/// </summary>
		/// <param name="tree">AdvTree to serialize</param>
		/// <param name="document">XmlDocument instance.</param>
		public static void Save(AdvTree tree, XmlDocument document)
		{
			XmlElement parent = document.CreateElement(XmlAdvTreeName);
			document.AppendChild(parent);
			TreeSerializer.Save(tree, parent);
		}
		
		/// <summary>
		/// Serializes AdvTree object to XmlElement object.
		/// </summary>
		/// <param name="tree">Instance of AdvTree to serialize.</param>
		/// <param name="parent">XmlElement to serialize to.</param>
		public static void Save(AdvTree tree, XmlElement parent)
		{
			NodeSerializationContext context = new NodeSerializationContext();
			context.RefXmlElement = parent;
			context.AdvTree = tree;
			context.HasSerializeNodeHandlers = tree.HasSerializeNodeHandlers;
			context.HasDeserializeNodeHandlers = tree.HasDeserializeNodeHandlers;
			
			foreach(Node node in tree.Nodes)
			{
				Save(node, context);
			}
		}
		
		/// <summary>
		/// Serializes Node and all child nodes to XmlElement object.
		/// </summary>
		/// <param name="node">Node to serialize.</param>
		/// <param name="context">Provides serialization context.</param>
		public static void Save(Node node, NodeSerializationContext context)
		{
			XmlElement parent = context.RefXmlElement;
			
			XmlElement xmlNode=parent.OwnerDocument.CreateElement(XmlNodeName);
			parent.AppendChild(xmlNode);
			
			ElementSerializer.Serialize(node, xmlNode);
			
			if(context.HasSerializeNodeHandlers)
			{
				XmlElement customXml = parent.OwnerDocument.CreateElement(XmlCustomName);
				SerializeNodeEventArgs e = new SerializeNodeEventArgs(node, xmlNode, customXml);
				context.AdvTree.InvokeSerializeNode(e);
				if (customXml.Attributes.Count > 0 || customXml.ChildNodes.Count > 0)
					xmlNode.AppendChild(customXml);
			}
			
			if(node.Cells.Count>1)
			{
				XmlElement xmlCells = parent.OwnerDocument.CreateElement(XmlCellsName);
				xmlNode.AppendChild(xmlCells);
				
				for(int i=1; i<node.Cells.Count;i++)
				{
					Cell cell=node.Cells[i];
					XmlElement xmlCell= parent.OwnerDocument.CreateElement(XmlCellName);
					xmlCells.AppendChild(xmlCell);
					ElementSerializer.Serialize(cell, xmlCell);
					if(cell.ShouldSerializeImages())
					{
						XmlElement xmlCellImages = parent.OwnerDocument.CreateElement(XmlCellImagesName);
						xmlCell.AppendChild(xmlCellImages);
						ElementSerializer.Serialize(cell.Images, xmlCellImages);
					}
				}
			}
			
			context.RefXmlElement = xmlNode;
			foreach(Node childNode in node.Nodes)
			{
				Save(childNode, context);
			}
			context.RefXmlElement = parent;
		}
		#endregion
		
		#region Loading
		/// <summary>
		/// Load AdvTree Nodes from file.
		/// </summary>
		/// <param name="tree">Reference to AdvTree to populate</param>
		/// <param name="fileName">File name.</param>
		public static void Load(AdvTree tree, string fileName)
		{
			XmlDocument document=new XmlDocument();
			document.Load(fileName);
			Load(tree, document);
		}
		
		/// <summary>
		/// Load AdvTree Nodes from stream.
		/// </summary>
		/// <param name="tree">Reference to AdvTree to populate</param>
		/// <param name="inStream">Reference to stream</param>
		public static void Load(AdvTree tree, Stream inStream)
		{
			XmlDocument document=new XmlDocument();
			document.Load(inStream);
			Load(tree, document);
		}
		
		/// <summary>
		/// Load AdvTree Nodes from reader.
		/// </summary>
		/// <param name="tree">Reference to AdvTree to populate</param>
		/// <param name="reader">Reference to reader.</param>
		public static void Load(AdvTree tree, TextReader reader)
		{
			XmlDocument document=new XmlDocument();
			document.Load(reader);
			Load(tree, document);
		}
		
		/// <summary>
		/// Load AdvTree Nodes from reader.
		/// </summary>
		/// <param name="tree">Reference to AdvTree to populate</param>
		/// <param name="reader">Reference to reader.</param>
		public static void Load(AdvTree tree, XmlReader reader)
		{
			XmlDocument document=new XmlDocument();
			document.Load(reader);
			Load(tree, document);
		}
		
		/// <summary>
		/// Load AdvTree from XmlDocument that was created by Save method.
		/// </summary>
		/// <param name="tree">Tree Control to load</param>
		/// <param name="document">XmlDocument to load control from</param>
		public static void Load(AdvTree tree, XmlDocument document)
		{
			foreach(XmlNode xmlNode in document.ChildNodes)
			{
				if(xmlNode.Name==XmlAdvTreeName && xmlNode is XmlElement)
				{
					Load(tree, xmlNode as XmlElement);
					break;
				}
			}
		}
		
		/// <summary>
		/// Load nodes from XmlElement.
		/// </summary>
		/// <param name="tree">Reference to AdvTree to be populated.</param>
		/// <param name="parent">XmlElement that tree was serialized to.</param>
		public static void Load(AdvTree tree, XmlElement parent)
		{
			tree.BeginUpdate();
			tree.DisplayRootNode = null;
			tree.Nodes.Clear();
			
			NodeSerializationContext context = new NodeSerializationContext();
			context.AdvTree = tree;
			context.HasDeserializeNodeHandlers  = tree.HasDeserializeNodeHandlers;
			context.HasSerializeNodeHandlers = tree.HasSerializeNodeHandlers;
			
			try
			{
				foreach(XmlNode xmlNode in parent.ChildNodes)
				{
					if(xmlNode.Name==XmlNodeName && xmlNode is XmlElement)
					{
						Node node=new Node();
						tree.Nodes.Add(node);
						context.RefXmlElement = xmlNode as XmlElement;
						LoadNode(node, context);
					}
				}
			}
			finally
			{
				tree.EndUpdate();
			}
		}
		
		/// <summary>
		/// Load single node and it's child nodes if any.
		/// </summary>
		/// <param name="nodeToLoad">New instance of node that is populated with loaded data.</param>
		/// <param name="context">Provides deserialization context.</param>
		public static void LoadNode(Node nodeToLoad, NodeSerializationContext context)
		{
			XmlElement xmlNode = context.RefXmlElement;
			
			ElementSerializer.Deserialize(nodeToLoad, xmlNode);

			foreach(XmlNode xmlChild in xmlNode.ChildNodes)
			{
				XmlElement xmlElem = xmlChild as XmlElement;
				if(xmlElem == null)
					continue;
				if(xmlElem.Name==XmlNodeName)
				{
					Node node=new Node();
					nodeToLoad.Nodes.Add(node);
					context.RefXmlElement = xmlElem;
					LoadNode(node, context);
				}
				else if(xmlElem.Name == XmlCellsName)
				{
					LoadCells(nodeToLoad, xmlElem);
				}
				else if(xmlElem.Name == XmlCustomName)
				{
					if(context.HasDeserializeNodeHandlers)
					{
						SerializeNodeEventArgs e = new SerializeNodeEventArgs(nodeToLoad, xmlNode, xmlElem);
						context.AdvTree.InvokeDeserializeNode(e);
					}
				}
			}
			context.RefXmlElement = xmlNode;
		}
		
		private static void LoadCells(Node parentNode, XmlElement xmlCells)
		{
			foreach(XmlNode xmlChild in xmlCells.ChildNodes)
			{
				if(xmlChild.Name==XmlCellName && xmlChild is XmlElement)
				{
					Cell cell=new Cell();
					parentNode.Cells.Add(cell);
					ElementSerializer.Deserialize(cell, xmlChild as XmlElement);
					// Load images if any
					foreach(XmlElement xmlImage in xmlChild.ChildNodes)
					{
						if(xmlImage.Name==XmlCellImagesName)
						{
							ElementSerializer.Deserialize(cell.Images, xmlImage);
							break;
						}
					}
				}
			}
		}
		#endregion
	}
	
	/// <summary>
	/// Provides context information for serialization.
	/// </summary>
	public class NodeSerializationContext
	{
		/// <summary>
		/// Gets or sets reference to context parent XmlElement when serializing or actual Node element when deserializing.
		/// </summary>
		public System.Xml.XmlElement RefXmlElement = null;

		/// <summary>
		/// Gets or sets whether SerializeNode event handler has been defined and whether event should be fired.
		/// </summary>
		public bool HasSerializeNodeHandlers = false;

		/// <summary>
		/// Gets or sets whether DeserializeNode event handler has been defined and whether event should be fired.
		/// </summary>
		public bool HasDeserializeNodeHandlers = false;

		/// <summary>
		/// Provides access to serializer.
		/// </summary>
		public AdvTree AdvTree = null;
	}
}
