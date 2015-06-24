using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.Tree 
{
	/// <summary>
	/// Represents collection for ColumnHeader objects.
	/// </summary>
	public class ColumnHeaderCollection:CollectionBase 
	{
		#region Private Variables
		private Node m_ParentNode=null;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ColumnHeaderCollection()
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
		/// <param name="parent">ColumnHeader that is parent of this collection.</param>
		internal void SetParentNode(Node parent)
		{
			m_ParentNode=parent;
		}

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="ch">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(ColumnHeader ch)
		{
			return List.Add(ch);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public ColumnHeader this[int index]
		{
			get {return (ColumnHeader)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, ColumnHeader value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(ColumnHeader value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(ColumnHeader value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(ColumnHeader value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(ColumnHeader[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the ColumnHeader array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(ColumnHeader[] array)
		{
			List.CopyTo(array,0);
		}

		protected override void OnClear()
		{
			base.OnClear();
		}
		#endregion
	}

	#region ColumnHeaderCollectionEditor
	/// <summary>
	/// Support for ColumnHeader tabs design-time editor.
	/// </summary>
	public class ColumnHeaderCollectionEditor:System.ComponentModel.Design.CollectionEditor
	{
		/// <summary>Creates new instance of the class</summary>
		/// <param name="type">Type to initialize editor with.</param>
		public ColumnHeaderCollectionEditor(Type type):base(type)
		{
		}
		protected override Type CreateCollectionItemType()
		{
			return typeof(ColumnHeader);
		}
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[] {typeof(ColumnHeader)};
		}
		protected override object CreateInstance(Type itemType)
		{
			object item=base.CreateInstance(itemType);
			if(item is ColumnHeader)
			{
				ColumnHeader ch=item as ColumnHeader;
				ch.Text=ch.Name;
			}
			return item;
		}
	}
	#endregion
}
