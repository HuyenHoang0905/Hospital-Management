using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.Tree
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

		protected override void OnClear()
		{
			base.OnClear();
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

	#region CellCollectionEditor
	/// <summary>
	/// Support for Cell tabs design-time editor.
	/// </summary>
	public class CellCollectionEditor:System.ComponentModel.Design.CollectionEditor
	{
		/// <summary>Creates new instance of cell collection editor.</summary>
		/// <param name="type">Type to initialize editor with.</param>
		public CellCollectionEditor(Type type):base(type)
		{
		}
		protected override Type CreateCollectionItemType()
		{
			return typeof(Cell);
		}
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] {typeof(Cell)};
		}
		protected override object CreateInstance(Type itemType)
		{
			object item=base.CreateInstance(itemType);
			if(item is Cell)
			{
				Cell cell=item as Cell;
				cell.Text=cell.Name;
			}
			return item;
		}
	}
	#endregion
}
