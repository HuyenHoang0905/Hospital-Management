using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents collection for Node objects.
	/// </summary>
	public class NodeCollection:CollectionBase 
	{
		#region Private Variables
		private Node m_ParentNode=null;
		private AdvTree m_TreeControl=null;
		private eTreeAction m_SourceAction=eTreeAction.Code;
        private bool _PassiveCollection = false;
		#endregion

		#region Internal Implementation
		public NodeCollection()
		{
		}
        internal eTreeAction SourceAction
        {
            get
            {
                return m_SourceAction;
            }
            set
            {
                m_SourceAction = value;
            }
        }
        internal bool PassiveCollection
        {
            get { return _PassiveCollection; }
            set
            {
                _PassiveCollection = value;
            }
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

		internal AdvTree TreeControl
		{
			get {return m_TreeControl;}
			set {m_TreeControl=value;}
		}

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="node">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public virtual int Add(Node node)
		{
			return Add(node, eTreeAction.Code);
		}
		
		/// <summary>
		/// Adds new object to the collection and provides information about the source of the command
		/// </summary>
		/// <param name="node">Node to add</param>
		/// <param name="action">Source action</param>
		/// <returns></returns>
		public virtual int Add(Node node, eTreeAction action)
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
				this.Add(node);
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
		public virtual void Insert(int index, Node value) 
		{
			List.Insert(index, value);
		}
		
		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		/// <param name="action">Action that is causing the event</param>
        public virtual void Insert(int index, Node value, eTreeAction action) 
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
		public virtual void Remove(Node value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Removes specified object from the collection and provides information about source of the command
		/// </summary>
		/// <param name="node">Node to remove</param>
		/// <param name="action">Source action</param>
		public virtual void Remove(Node node, eTreeAction action)
		{
			m_SourceAction = action;
			List.Remove(node);
		}
		
		protected override void OnRemove(int index, object value)
		{
            if (!_PassiveCollection)
            {
                AdvTree tree = GetTreeControl();
                if (tree != null)
                    tree.InvokeBeforeNodeRemove(m_SourceAction, value as Node, m_ParentNode);
            }
			base.OnRemove (index, value);
		}


		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
            NodeRemoveComplete(index, value);
		}

        private void NodeRemoveComplete(int index, object value)
        {
            if (!_PassiveCollection)
            {
                Node node = value as Node;
                node.SetParent(null);
                node.internalTreeControl = null;
                if (m_ParentNode != null) m_ParentNode.OnChildNodeRemoved(node);

                AdvTree tree = GetTreeControl();
                if (tree != null)
                {
                    //tree.InvokeAfterNodeRemove(m_SourceAction, value as Node, m_ParentNode);
                    tree.NodeRemoved(m_SourceAction, value as Node, m_ParentNode, index);
                }
            }
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            base.OnSetComplete(index, oldValue, newValue);

            NodeRemoveComplete(index, oldValue);
            NodeInsertComplete(newValue);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (!_PassiveCollection)
            {
                AdvTree tree = GetTreeControl();

                if (tree != null)
                    tree.InvokeBeforeNodeRemove(m_SourceAction, oldValue as Node, m_ParentNode);

                if (tree != null)
                    tree.InvokeBeforeNodeInsert(m_SourceAction, newValue as Node, m_ParentNode);
            }
            base.OnSet(index, oldValue, newValue);
        }
		
		protected override void OnInsert(int index, object value)
		{
            if (!_PassiveCollection)
            {
                AdvTree tree = GetTreeControl();
                if (tree != null)
                    tree.InvokeBeforeNodeInsert(m_SourceAction, value as Node, m_ParentNode);
            }
			base.OnInsert (index, value);
		}

		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
            NodeInsertComplete(value);
		}

        private void NodeInsertComplete(object value)
        {
            if (!_PassiveCollection)
            {
                Node node = value as Node;
                if (m_ParentNode != null)
                {
                    if (node.Parent != null && node.Parent != m_ParentNode)
                        node.Remove();
                    node.SetParent(m_ParentNode);
                    if (m_ParentNode.NodesColumns.IsSorted)
                    {
                        AdvTree parentTree = m_TreeControl;
                        if (parentTree == null) parentTree = m_ParentNode.TreeControl;
                        if (parentTree != null)
                            parentTree.PushSortRequest(m_ParentNode);
                    }
                }
                else
                {
                    if (node.Parent != null)
                        node.Remove();
                    else
                        node.InvokeOnParentChanged();
                    if (m_TreeControl != null && m_TreeControl.Columns.IsSorted)
                    {
                        m_TreeControl.PushSortRequest();
                    }
                }
                node.internalTreeControl = m_TreeControl;

                if (m_ParentNode != null)
                    m_ParentNode.OnChildNodeInserted(node);
                else
                    node.SizeChanged = true;

                AdvTree tree = GetTreeControl();
                if (tree != null)
                    tree.InvokeAfterNodeInsert(m_SourceAction, value as Node, m_ParentNode);
            }
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
		[EditorBrowsable(EditorBrowsableState.Never)]
        public void CopyTo(Node[] array)
		{
			List.CopyTo(array,0);
		}

		protected override void OnClear()
		{
            if (!_PassiveCollection)
            {
                foreach (Node node in this.List)
                {
                    node.SetParent(null);
                    node.internalTreeControl = null;
                }
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

            AdvTree tree = GetTreeControl();
            if (tree != null)
            {
                tree.ValidateSelectedNode();
            }

            base.OnClearComplete();
        }
		
		private AdvTree GetTreeControl()
		{
			if(m_TreeControl!=null)
				return m_TreeControl;
			if(m_ParentNode!=null)
				return m_ParentNode.TreeControl;
			return null;
		}

        /// <summary>
        /// Sorts the elements in the entire collection using the IComparable implementation of each element.
        /// </summary>
        public virtual void Sort()
        {
            this.Sort(0, this.Count, Comparer.Default);
        }
        /// <summary>
        /// Sorts the elements in the entire collection using the specified comparer.
        /// </summary>
        /// <param name="comparer">The IComparer implementation to use when comparing elements.-or- null to use the IComparable implementation of each element.</param>
        public virtual void Sort(IComparer comparer)
        {
            this.Sort(0, this.Count, comparer);
        }
        /// <summary>
        /// Sorts the elements in a range of elements in collection using the specified comparer.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="comparer"></param>
        public virtual void Sort(int index, int count, IComparer comparer)
        {
            AdvTree tree = GetTreeControl();
            if (!_PassiveCollection && tree != null)
                tree.BeginUpdate();

            this.InnerList.Sort(index, count, comparer);

            if (!_PassiveCollection && tree != null)
                tree.EndUpdate(); 
        }
        
        /// <summary>
        /// Finds the tree nodes with specified key, optionally searching sub-nodes.
        /// </summary>
        /// <param name="name">The name of the tree node to search for.</param>
        /// <param name="searchAllChildren">true to search child nodes of tree nodes; otherwise, false. </param>
        /// <returns>An array of Node objects whose Name property matches the specified key.</returns>
        public Node[] Find(string name,	bool searchAllChildren)
        {
            ArrayList list = new ArrayList();
            NodeOperations.FindNodesByName(this, name, searchAllChildren, list);
            Node[] nodes = new Node[list.Count];
            if (list.Count > 0) list.CopyTo(nodes);
            return nodes;
        }

		#endregion
	}

    #region Node Comparer
    public class NodeComparer : IComparer
    {
        private int _ColumnIndex = 0;
        /// <summary>
        /// Creates new instance of NodeComparer class. You can use NodeComparer to sort the nodes by specific column/cell by calling
        /// NodeCollection.Sort method and pass new instance of NodeComparer class. 
        /// </summary>
        public NodeComparer()
        {
        }
        /// <summary>
        /// Creates new instance of NodeComparer class. You can use NodeComparer to sort the nodes by specific column/cell by calling
        /// NodeCollection.Sort method and pass new instance of NodeComparer class. 
        /// </summary>
        /// <param name="columnIndex">Column/Cell index to use for sorting.</param>
        public NodeComparer(int columnIndex)
        {
            _ColumnIndex = columnIndex;
        }

        /// <summary>
        /// Gets or sets the Column/Cell index that is used for sorting.
        /// </summary>
        public int ColumnIndex
        {
            get { return _ColumnIndex; }
            set { _ColumnIndex = value; }
        }
        #region IComparer Members

        public virtual int Compare(object x, object y)
        {
            Node nx = (Node)x;
            Node ny = (Node)y;
            if (_ColumnIndex < nx.Cells.Count && _ColumnIndex < ny.Cells.Count)
                return Utilities.CompareAlphaNumeric(nx.Cells[_ColumnIndex].Text, ny.Cells[_ColumnIndex].Text);
            return 0;
        }

        #endregion
    }
    /// <summary>
    /// Reverse sort nodes.
    /// </summary>
    public class NodeComparerReverse : NodeComparer
    {
        /// <summary>
        /// Creates new instance of NodeComparer class. You can use NodeComparer to sort the nodes by specific column/cell by calling
        /// NodeCollection.Sort method and pass new instance of NodeComparer class. 
        /// </summary>
        /// <param name="columnIndex">Column/Cell index to use for sorting.</param>
        public NodeComparerReverse(int columnIndex) : base(columnIndex)
        {
        }

        public override int Compare(object x, object y)
        {
            return base.Compare(y, x);
        }
    }
    /// <summary>
    /// Sort by flat node index.
    /// </summary>
    public class NodeFlatIndexComparer : IComparer
    {
        private AdvTree _TreeControl = null;
        /// <summary>
        /// Creates new instance of NodeComparer class. You can use NodeComparer to sort the nodes by specific column/cell by calling
        /// NodeCollection.Sort method and pass new instance of NodeComparer class. 
        /// </summary>
        public NodeFlatIndexComparer(AdvTree treeControl)
        {
            _TreeControl = treeControl;
        }
        #region IComparer Members

        public virtual int Compare(object x, object y)
        {
            Node nx = (Node)x;
            Node ny = (Node)y;

            if (_TreeControl.GetNodeFlatIndex(nx) < _TreeControl.GetNodeFlatIndex(ny))
                return -1;
            else
                return 1;
        }

        #endregion
    }
    #endregion
}
