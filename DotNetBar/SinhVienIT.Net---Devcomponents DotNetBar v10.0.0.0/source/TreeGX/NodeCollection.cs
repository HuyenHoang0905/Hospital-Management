using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents collection for Node objects.
	/// </summary>
	public class NodeCollection:CollectionBase 
	{
		#region Private Variables
		private Node m_ParentNode=null;
		private TreeGX m_TreeControl=null;
		private eTreeAction m_SourceAction=eTreeAction.Code;
		#endregion

		#region Internal Implementation
		public NodeCollection()
		{
		}
		/// <summary>
		/// Gets or sets the node this collection is associated with.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Node ParentNode
		{
			get {return m_ParentNode;}
		}
		/// <summary>
		/// Sets the node collection belongs to.
		/// </summary>
		/// <param name="parent">Node that is parent of this collection.</param>
		internal void SetParentNode(Node parent)
		{
			m_ParentNode=parent;
		}

		internal TreeGX TreeControl
		{
			get {return m_TreeControl;}
			set {m_TreeControl=value;}
		}

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="node">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(Node node)
		{
			return List.Add(node);
		}
		
		/// <summary>
		/// Adds new object to the collection and provides information about the source of the command
		/// </summary>
		/// <param name="node">Node to add</param>
		/// <param name="action">Source action</param>
		/// <returns></returns>
		public int Add(Node node, eTreeAction action)
		{
			m_SourceAction = action;
			return List.Add(node);
		}
		
		/// <summary>
		/// Adds an array of objects to the collection. 
		/// </summary>
		/// <param name="nodes">Array of Node objects.</param>
		public void AddRange(Node[] nodes)
		{
			foreach(Node node in nodes)
				List.Add(node);
		}
		
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public Node this[int index]
		{
			get {return (Node)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, Node value) 
		{
			List.Insert(index, value);
		}
		
		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		/// <param name="action">Action that is causing the event</param>
		public void Insert(int index, Node value, eTreeAction action) 
		{
			m_SourceAction = action;
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(Node value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(Node value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(Node value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Removes specified object from the collection and provides information about source of the command
		/// </summary>
		/// <param name="node">Node to remove</param>
		/// <param name="action">Source action</param>
		public void Remove(Node node, eTreeAction action)
		{
			m_SourceAction = action;
			List.Remove(node);
		}
		
		protected override void OnRemove(int index, object value)
		{
			TreeGX tree = GetTreeControl();
			if(tree!=null)
				tree.InvokeBeforeNodeRemove(m_SourceAction, value as Node, m_ParentNode);
			base.OnRemove (index, value);
		}


		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			Node node=value as Node;
			node.SetParent(null);
			node.internalTreeControl=null;
			if(m_ParentNode!=null) m_ParentNode.OnChildNodeRemoved(node);
			
			TreeGX tree = GetTreeControl();
			if(tree!=null)
			{
				tree.InvokeAfterNodeRemove(m_SourceAction, value as Node, m_ParentNode);
				tree.NodeRemoved(m_SourceAction, value as Node, m_ParentNode, index);
			}
		}
		
		protected override void OnInsert(int index, object value)
		{
			TreeGX tree = GetTreeControl();
			if(tree!=null)
				tree.InvokeBeforeNodeInsert(m_SourceAction, value as Node, m_ParentNode);
			base.OnInsert (index, value);
		}

		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			Node node=value as Node;
			if(m_ParentNode!=null)
			{
				if(node.Parent!=null && node.Parent!=m_ParentNode)
					node.Remove();
				node.SetParent(m_ParentNode);
			}
			node.internalTreeControl=m_TreeControl;
			
			if(m_ParentNode!=null) m_ParentNode.OnChildNodeInserted(node);
			
			TreeGX tree = GetTreeControl();
			if(tree!=null)
				tree.InvokeAfterNodeInsert(m_SourceAction, value as Node, m_ParentNode);
			
			m_SourceAction = eTreeAction.Code;
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(Node[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the Node array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(Node[] array)
		{
			List.CopyTo(array,0);
		}

		protected override void OnClear()
		{
			foreach(Node node in this.List)
			{
				node.SetParent(null);
				node.internalTreeControl=null;
			}
			
			base.OnClear();
		}

        protected override void OnClearComplete()
        {
            if (m_TreeControl != null && !PassiveCollection)
            {
                m_TreeControl.OnNodesCleared();
            }
            else if (m_ParentNode != null && !PassiveCollection)
                m_ParentNode.OnNodesCleared();

            TreeGX tree = GetTreeControl();
            if (tree != null)
            {
                tree.ValidateSelectedNode();
            }

            base.OnClearComplete();
        }
		
		private TreeGX GetTreeControl()
		{
			if(m_TreeControl!=null)
				return m_TreeControl;
			if(m_ParentNode!=null)
				return m_ParentNode.TreeControl;
			return null;
		}

        private bool _PassiveCollection = false;
        internal bool PassiveCollection
        {
            get { return _PassiveCollection; }
            set
            {
                _PassiveCollection = value;
            }
        }
		#endregion
	}

	#region NodeCollectionEditor
	/// <summary>
	/// Support for Node tabs design-time editor.
	/// </summary>
	public class NodeCollectionEditor:System.ComponentModel.Design.CollectionEditor
	{
		public NodeCollectionEditor(Type type):base(type)
		{
		}
		protected override Type CreateCollectionItemType()
		{
			return typeof(Node);
		}
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] {typeof(Node)};
		}
		protected override object CreateInstance(Type itemType)
		{
			object item=base.CreateInstance(itemType);
			if(item is Node)
			{
				Node node=item as Node;
				node.Text=node.Name;
			}
			return item;
		}
	}
	#endregion
}
