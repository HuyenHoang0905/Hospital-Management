using System;
using System.Collections;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents collection for Node objects.
	/// </summary>
	public class ElementStyleCollection:CollectionBase 
	{
		#region Private Variables
		private AdvTree m_TreeControl=null;
		//private Hashtable m_InnerHashtable=new Hashtable();
		#endregion

		#region Internal Implementation
		/// <summary>Creates new instance of the object.</summary>
		public ElementStyleCollection()
		{
		}
		internal AdvTree TreeControl
		{
			get {return m_TreeControl;}
			set {m_TreeControl=value;}
		}

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="tab">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(ElementStyle style)
		{
			return List.Add(style);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public ElementStyle this[int index]
		{
			get {return (ElementStyle)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Returns reference to the object in collection based on it's name.
		/// </summary>
		public ElementStyle this[string name]
		{
			get
			{
				foreach(ElementStyle style in this.List)
				{
					if(style.Name==name)
						return style;
				}
				return null;
			}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, ElementStyle value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(ElementStyle value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(ElementStyle value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(ElementStyle value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			ElementStyle style=value as ElementStyle;
			style.Parent=null;
			//m_InnerHashtable.Remove(style.Name);
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			ElementStyle style=value as ElementStyle;
			if(style.Parent!=null && style.Parent!=this)
				style.Parent.Remove(style);
			style.Parent=this;
			//m_InnerHashtable.Add(style.Name,style);
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(ElementStyle[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the Node array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(ElementStyle[] array)
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
