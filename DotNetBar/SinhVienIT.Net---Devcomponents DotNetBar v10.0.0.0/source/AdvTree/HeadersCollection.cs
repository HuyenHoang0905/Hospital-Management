using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.AdvTree 
{
	/// <summary>
	/// Represents collection for HeaderDefinition objects.
	/// </summary>
	public class HeadersCollection:CollectionBase 
	{
		#region Private Variables
		private AdvTree m_Parent=null;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Gets or sets the node this collection is associated with.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AdvTree Parent
		{
			get {return m_Parent;}
		}
		/// <summary>
		/// Sets the node collection belongs to.
		/// </summary>
		/// <param name="parent">HeaderDefinition that is parent of this collection.</param>
		internal void SetParent(AdvTree parent)
		{
			m_Parent=parent;
		}

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="ch">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(HeaderDefinition ch)
		{
			return List.Add(ch);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public HeaderDefinition this[int index]
		{
			get {return (HeaderDefinition)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, HeaderDefinition value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(HeaderDefinition value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(HeaderDefinition value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(HeaderDefinition value) 
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
		public void CopyTo(HeaderDefinition[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the HeaderDefinition array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(HeaderDefinition[] array)
		{
			List.CopyTo(array,0);
		}

		protected override void OnClear()
		{
			base.OnClear();
		}
		
		public HeaderDefinition GetByName(string name)
		{
			foreach(HeaderDefinition d in this.List)
			{
				if(d.Name==name)
					return d;
			}
			return null;
		}
		#endregion
	}
}
