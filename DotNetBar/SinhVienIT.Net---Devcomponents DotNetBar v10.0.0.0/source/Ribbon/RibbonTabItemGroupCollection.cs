using System;
using System.Collections;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Collection of RibbonTabItemGroup objects.
	/// </summary>
	public class RibbonTabItemGroupCollection:CollectionBase
	{
		#region Private Variables and Events
		public event EventHandler GroupRemoved;
		public event EventHandler GroupAdded;
		private RibbonStrip m_Owner=null;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="tab">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(RibbonTabItemGroup tab)
		{
			return List.Add(tab);
		}

		/// <summary>
		/// Adds new objects to the collection.
		/// </summary>
		/// <param name="groups">Array of groups to add.</param>
		public void AddRange(RibbonTabItemGroup[] groups)
		{
			foreach(RibbonTabItemGroup doc in groups)
				List.Add(doc);
		}

		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public RibbonTabItemGroup this[int index]
		{
			get {return (RibbonTabItemGroup)(List[index]);}
			set {List[index] = value;}
		}

        /// <summary>
        /// Returns reference to the object in collection based on it's name.
        /// </summary>
        public RibbonTabItemGroup this[string name]
        {
            get
            {
                foreach (RibbonTabItemGroup g in this.List)
                {
                    if (g.Name == name)
                        return g;
                }
                return null;
            }
        }

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, RibbonTabItemGroup value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(RibbonTabItemGroup value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(RibbonTabItemGroup value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(RibbonTabItemGroup value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			RibbonTabItemGroup item=value as RibbonTabItemGroup;
			item.ParentRibbonStrip=null;
			if(GroupRemoved!=null)
				GroupRemoved(item,new EventArgs());
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			RibbonTabItemGroup item=value as RibbonTabItemGroup;
			if(m_Owner!=null)
				item.ParentRibbonStrip=m_Owner;
			if(GroupAdded!=null)
				GroupAdded(item,new EventArgs());
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(RibbonTabItemGroup[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the RibbonTabItemGroup array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(RibbonTabItemGroup[] array)
		{
			List.CopyTo(array,0);
		}

		internal RibbonStrip Owner
		{
			get {return m_Owner;}
			set {m_Owner=value;}
		}

		protected override void OnClear()
		{
			base.OnClear();
		}
		#endregion
	}
}
