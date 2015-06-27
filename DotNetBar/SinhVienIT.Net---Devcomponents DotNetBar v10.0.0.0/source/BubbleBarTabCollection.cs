using System;
using System.Collections;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents collection for BubbleBarTab objects.
	/// </summary>
	public class BubbleBarTabCollection:CollectionBase 
	{
		#region Private Variables
		private BubbleBar m_Owner=null;
		#endregion

		#region Internal Implementation
		public BubbleBarTabCollection(BubbleBar owner)
		{
			m_Owner=owner;
		}
		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="tab">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(BubbleBarTab tab)
		{
			return List.Add(tab);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public BubbleBarTab this[int index]
		{
			get {return (BubbleBarTab)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, BubbleBarTab value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(BubbleBarTab value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(BubbleBarTab value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(BubbleBarTab value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			BubbleBarTab item=value as BubbleBarTab;
			m_Owner.OnTabRemoved(item);
			item.SetParent(null);
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			BubbleBarTab item=value as BubbleBarTab;
			item.SetParent(m_Owner);
			m_Owner.OnTabAdded(item);
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(BubbleBarTab[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the IBlock array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(IBlock[] array)
		{
			List.CopyTo(array,0);
		}

		/// <summary>
		/// Copies contained items to the ISimpleTab array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(ISimpleTab[] array)
		{
			List.CopyTo(array,0);
		}

		protected override void OnClear()
		{
			m_Owner.OnTabsCleared();
			base.OnClear();
		}

		/// <summary>
		/// Returns next visible tab from the reference tab.
		/// </summary>
		/// <param name="tabFrom">Reference tab.</param>
		/// <returns>Next visible tab or null if next visible tab cannot be determined.</returns>
		internal BubbleBarTab GetNextVisibleTab(BubbleBarTab tabFrom)
		{
			int from=0;
			if(tabFrom!=null)from=this.IndexOf(tabFrom)+1;
			for(int i=from;i<this.Count;i++)
			{
				if(this[i].Visible)
					return this[i];
			}
			return null;
		}

		/// <summary>
		/// Returns previous visible tab from the reference tab.
		/// </summary>
		/// <param name="tabFrom">Reference tab.</param>
		/// <returns>Previous visible tab or null if Previous visible tab cannot be determined.</returns>
		internal BubbleBarTab GetPreviousVisibleTab(BubbleBarTab tabFrom)
		{
			int from=0;
			if(tabFrom!=null) from=this.IndexOf(tabFrom)-1;

			for(int i=from;i>=0;i--)
			{
				if(this[i].Visible)
					return this[i];
			}
			return null;
		}
		#endregion
	}
}
