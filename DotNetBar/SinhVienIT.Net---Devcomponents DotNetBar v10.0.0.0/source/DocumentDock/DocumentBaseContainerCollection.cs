using System;
using System.Collections;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Collection of DocumentBaseContainer objects.
	/// </summary>
	public class DocumentBaseContainerCollection:CollectionBase
	{
		#region Private Variables and Events
		public event EventHandler DocumentRemoved;
		public event EventHandler DocumentAdded;
		private DocumentBaseContainer m_Owner=null;
		#endregion

		#region Internal Implementation
		public DocumentBaseContainerCollection()
		{
			//m_Owner=owner;
		}
		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="tab">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(DocumentBaseContainer tab)
		{
			return List.Add(tab);
		}

		/// <summary>
		/// Adds new objects to the collection.
		/// </summary>
		/// <param name="documents">Array of documents to add.</param>
		public void AddRange(DocumentBaseContainer[] documents)
		{
			foreach(DocumentBaseContainer doc in documents)
				List.Add(doc);
		}

		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public DocumentBaseContainer this[int index]
		{
			get {return (DocumentBaseContainer)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, DocumentBaseContainer value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(DocumentBaseContainer value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(DocumentBaseContainer value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(DocumentBaseContainer value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			DocumentBaseContainer item=value as DocumentBaseContainer;
			item.SetParent(null);
			if(DocumentRemoved!=null)
				DocumentRemoved(item,new EventArgs());
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			DocumentBaseContainer item=value as DocumentBaseContainer;
			if(m_Owner!=null)
				item.SetParent(m_Owner);
			if(DocumentAdded!=null)
				DocumentAdded(item,new EventArgs());
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(DocumentBaseContainer[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the DocumentBaseContainer array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(DocumentBaseContainer[] array)
		{
			List.CopyTo(array,0);
		}

		internal DocumentBaseContainer Owner
		{
			get {return m_Owner;}
			set {m_Owner=value;}
		}

		protected override void OnClear()
		{
			//m_Owner.OnTabsCleared();
			base.OnClear();
		}

        public int VisibleCount
        {
            get
            {
                int count = 0;
                foreach (DocumentBaseContainer item in this.List)
                {
                    if (item.Visible) count++;
                }
                return count;
            }
        }
		#endregion
	}
}
