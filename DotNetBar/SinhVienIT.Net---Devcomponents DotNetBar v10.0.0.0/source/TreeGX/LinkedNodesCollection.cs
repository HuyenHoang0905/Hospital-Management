using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents collection of LinkedNode objects that describe linked node properties.
	/// </summary>
	public class LinkedNodesCollection:CollectionBase 
	{
		#region Private Variables
		private Node m_ParentNode=null;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LinkedNodesCollection()
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

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="p">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(LinkedNode p)
		{
			return List.Add(p);
		}

		/// <summary>
		/// Adds range of objects to the array.
		/// </summary>
		/// <param name="ap">Array to add.</param>
		public void AddRange(LinkedNode[] ap)
		{
			foreach(LinkedNode p in ap)
				this.Add(p);
		}

		/// <summary>
		/// Copies objects of the collection to the array.
		/// </summary>
		/// <returns></returns>
		public LinkedNode[] ToArray()
		{
			LinkedNode[] ap=new LinkedNode[this.Count];
			this.CopyTo(ap);
			return ap;
		}

		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public LinkedNode this[int index]
		{
			get {return (LinkedNode)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, LinkedNode value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(LinkedNode value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(LinkedNode value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(LinkedNode value) 
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
		public void CopyTo(LinkedNode[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the ColumnHeader array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(LinkedNode[] array)
		{
			List.CopyTo(array,0);
		}

		protected override void OnClear()
		{
			base.OnClear();
		}
		#endregion
	}
}
