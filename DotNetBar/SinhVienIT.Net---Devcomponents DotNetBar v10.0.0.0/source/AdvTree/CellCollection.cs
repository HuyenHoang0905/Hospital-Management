using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.AdvTree
{
	///<summary>
	/// A strongly-typed collection of <see cref="Cell"/> objects.
	///</summary>
	public class CellCollection:CollectionBase 
	{
		#region Private Variables
		private Node m_ParentNode=null;
		#endregion

		#region Internal Implementation
		/// <summary>Creates new instance of the class.</summary>
		public CellCollection()
		{
		}
		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="cell">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(Cell cell)
		{
			return List.Add(cell);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public Cell this[int index]
		{
			get {return (Cell)(List[index]);}
			set {List[index] = value;}
		}
        /// <summary>
        /// Returns reference to the object in collection based on it's name. Returns null/nothing if cell with given name is not found.
        /// </summary>
        public Cell this[string name]
        {
            get 
            {
                foreach (Cell item in List)
                {
                    if (item.Name == name) return item;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the cell based on the column name. Node must be able to reach AdvTree control for this method to work.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Cell object or null.</returns>
        public Cell GetByColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException("columnName argument must be non-empty non-null string with column name");
            Node parentNode = this.ParentNode;
            AdvTree tree = parentNode.TreeControl;
            if (tree == null)
                throw new NullReferenceException("AdvTree control cannot be reached. Node is not added to a tree.");
            Cell cell = null;
            if (parentNode.Parent != null && parentNode.Parent.NodesColumns.Count > 0)
            {
                int index= parentNode.Parent.NodesColumns.IndexOf(columnName);
                if (index >= 0) cell = this[index];
            }
            else
            {
                int index = tree.Columns.IndexOf(columnName);
                if (index >= 0) cell = this[index];
            }
            return cell;
        }

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, Cell value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(Cell value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(Cell value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(Cell value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			Cell cell=value as Cell;
			cell.SetParent(null);
			if(m_ParentNode!=null)
				m_ParentNode.OnCellRemoved(cell);
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			Cell cell=value as Cell;
			if(cell.Parent!=null && cell.Parent!=m_ParentNode)
				cell.Parent.Cells.Remove(cell);
			cell.SetParent(m_ParentNode);
			if(m_ParentNode!=null)
				m_ParentNode.OnCellInserted(cell);
		}
        protected override void OnInsert(int index, object value)
        {
            if (m_ParentNode != null && m_ParentNode.Site != null && m_ParentNode.Site.DesignMode && this.List.Count > 0)
            {
                Cell cell = value as Cell;
                if (cell.Site == null && this.List.Contains(cell)) this.List.Remove(cell);
            }

            base.OnInsert(index, value);
        }

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(Cell[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the Cell array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(Cell[] array)
		{
			List.CopyTo(array,0);
		}

        private Cell _RootCell = null;
		protected override void OnClear()
		{
            if (m_ParentNode != null && m_ParentNode.Site != null && m_ParentNode.Site.DesignMode && this.List.Count>0)
            {
                if (this[0].Site == null)
                    _RootCell = this[0];
            }
			base.OnClear();
		}

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            if (_RootCell != null)
            {
                this.Add(_RootCell);
                _RootCell = null;
            }
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            base.OnSet(index, oldValue, newValue);
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
		/// <param name="parent">Cell that is parent of this collection.</param>
		internal void SetParentNode(Node parent)
		{
			m_ParentNode=parent;
		}
		#endregion
	}
}
