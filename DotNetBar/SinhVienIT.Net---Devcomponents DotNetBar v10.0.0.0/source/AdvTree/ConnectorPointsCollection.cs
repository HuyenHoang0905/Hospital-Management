using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents collection of connector points for a node.
	/// </summary>
	public class ConnectorPointsCollection:CollectionBase 
	{
		#region Private Variables
		private Node m_ParentNode=null;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ConnectorPointsCollection()
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
		public int Add(Point p)
		{
			return List.Add(p);
		}

		/// <summary>
		/// Adds range of objects to the array.
		/// </summary>
		/// <param name="ap">Array to add.</param>
		public void AddRange(Point[] ap)
		{
			foreach(Point p in ap)
				this.Add(p);
		}

		/// <summary>
		/// Copies objects of the collection to the array.
		/// </summary>
		/// <returns></returns>
		public Point[] ToArray()
		{
			Point[] ap=new Point[this.Count];
			this.CopyTo(ap);
			return ap;
		}

		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public Point this[int index]
		{
			get {return (Point)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, Point value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(Point value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(Point value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(Point value) 
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
		public void CopyTo(Point[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the ColumnHeader array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(Point[] array)
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
