using System;
using System.Collections;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents typed collection of BubbleButton objects.
	/// </summary>
	public class BubbleButtonCollection:CollectionBase
	{
		#region Private Variables
		private BubbleBarTab m_Parent=null;
		private bool m_IgnoreEvents=false;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Copies contained items to the IBlock array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(IBlock[] array)
		{
			List.CopyTo(array,0);
		}

		/// <summary>
		/// Creates new instance of the collection.
		/// </summary>
		/// <param name="parent">Parent of the collection.</param>
		internal BubbleButtonCollection(BubbleBarTab parent)
		{
			m_Parent=parent;
		}

		/// <summary>
		/// Gets the parent of the collection.
		/// </summary>
		internal BubbleBarTab Parent
		{
			get {return m_Parent;}
		}

		/// <summary>
		/// Adds new item to the collection but it does not raise internal events.
		/// </summary>
		/// <param name="item">New item to add.</param>
		/// <returns>Index of newly added item.</returns>
		internal int _Add(BubbleButton item)
		{
			m_IgnoreEvents=true;
			int i=0;
			try
			{
				i=List.Add(item);
			}
			finally
			{
				m_IgnoreEvents=false;
			}
			return i;
		}

		/// <summary>
		/// Adds new item to the collection at specified location but it does not raise internal events.
		/// </summary>
		/// <param name="item">New item to add.</param>
		/// <param name="Position">Position to add item to.</param>
		internal void _Add(BubbleButton item, int Position)
		{
			m_IgnoreEvents=true;
			try
			{
				List.Insert(Position,item);
			}
			finally
			{
				m_IgnoreEvents=false;
			}
		}

		/// <summary>
		/// Clears the collection but it does not raise internal events.
		/// </summary>
		internal void _Clear()
		{
			m_IgnoreEvents=true;
			try
			{
				List.Clear();
			}
			finally
			{
				m_IgnoreEvents=false;
			}
		}

		/// <summary>
		/// Performs additional custom processes before setting a value in the CollectionBase instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSet(int index,object oldValue,object newValue)
		{
			if(newValue==null)
				throw new InvalidOperationException("Setting of null values to BubbleButtonCollection is not allowed.");

			BubbleButton item=newValue as BubbleButton;
			if(item.Parent!=null)
			{
				item.Parent.Buttons.Remove(item);
			}

			base.OnSet(index,oldValue,newValue);
		}

		/// <summary>
		/// Performs additional custom processes after setting a value in the CollectionBase instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index. </param>
		protected override void OnSetComplete(int index,object oldValue,object newValue)
		{
			if(!m_IgnoreEvents)
			{
				BubbleButton item=newValue as BubbleButton;
				item.SetParentCollection(this);
				m_Parent.OnButtonInserted(item);
			}
			base.OnSetComplete(index,oldValue,newValue);
		}

		/// <summary>
		/// Performs additional custom processes before inserting a new element into the CollectionBase instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value. </param>
		/// <param name="value">The new value of the element at index.</param>
		protected override void OnInsert(int index,object value)
		{
			BubbleButton item=value as BubbleButton;
			if(item.Parent!=null && item.Parent!=this.Parent)
			{
				item.Parent.Buttons.Remove(item);
			}
			base.OnInsert(index,value);
		}

		/// <summary>
		/// Performs additional custom processes after inserting a new element into the CollectionBase instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value.</param>
		/// <param name="value">The new value of the element at index.</param>
		protected override void OnInsertComplete(int index,object value)
		{
			if(!m_IgnoreEvents)
			{
				BubbleButton item=value as BubbleButton;
				item.SetParentCollection(this);
				m_Parent.OnButtonInserted(item);
			}

			base.OnInsertComplete(index,value);
		}

		/// <summary>
		/// Performs additional custom processes when removing an element from the CollectionBase instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemove(int index,object value)
		{
//			if(!m_IgnoreEvents)
//			{
//				BubbleButton item=value as BubbleButton;				
//			}
			base.OnRemove(index,value);
		}

		/// <summary>
		/// Performs additional custom processes after removing an element from the CollectionBase instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found. </param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemoveComplete(int index,object value)
		{
			if(!m_IgnoreEvents)
			{
				BubbleButton item=value as BubbleButton;
				item.SetParentCollection(null);
				m_Parent.OnButtonRemoved(item);
			}

			base.OnRemoveComplete(index,value);
		}

		/// <summary>
		/// Removes an item without raising internal events.
		/// </summary>
		/// <param name="item">Item to remove.</param>
		internal void _Remove(BubbleButton item)
		{
			m_IgnoreEvents=true;
			try{List.Remove(item);}
			finally{m_IgnoreEvents=false;}
		}

		/// <summary>
		/// Performs additional custom processes when clearing the contents of the CollectionBase instance.
		/// </summary>
		protected override void OnClear()
		{
			if(!m_IgnoreEvents)
			{
//				if(List.Count>0)
//				{
//					foreach(BubbleButton objSub in this)
//					{
//						if(owner!=null)
//							owner.RemoveShortcutsFromItem(objSub);
//					}
//				}
				if(m_Parent!=null)
				{
					m_Parent.OnButtonsCollectionClear();
				}
			}
			base.OnClear();
		}

		/// <summary>
		/// Copies the collection to the ArrayList object.
		/// </summary>
		/// <param name="list">Target ArrayList.</param>
		public void CopyTo(ArrayList list)
		{
			if(list==null)
				return;
			foreach(BubbleButton item in this)
				list.Add(item);
		}
		#endregion

		#region Public Interface
		/// <summary>
		/// Adds new item to the collection.
		/// </summary>
		/// <param name="item">New item to add.</param>
		/// <returns>Index of newly added item.</returns>
		public virtual int Add(BubbleButton item)
		{
			return Add(item,-1);
		}

		/// <summary>
		/// Adds new item to the collection at specified location.
		/// </summary>
		/// <param name="item">New item to add.</param>
		/// <param name="Position">Position to insert item at. Position of -1 will append the item to the end of the collection.</param>
		/// <returns>Index of the newly added item.</returns>
		public virtual int Add(BubbleButton item, int Position)
		{
			int iRet=Position;
			
			if(Position>=0)
				List.Insert(Position,item);
			else
				iRet=List.Add(item);

			return iRet;
		}

		/// <summary>
		/// Accesses items inside of the collection based on the index.
		/// </summary>
		public virtual BubbleButton this[int index]
		{
			get {return (BubbleButton)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Accesses items inside of the collection based on the name.
		/// </summary>
		public  virtual BubbleButton this[string name]
		{
			get {return (BubbleButton)(List[this.IndexOf(name)]);}
			set {List[this.IndexOf(name)] = value;}
		}

		/// <summary>
		/// Inserts new item at the specified position.
		/// </summary>
		/// <param name="index">Position to insert item at.</param>
		/// <param name="item">Item to insert.</param>
		public virtual void Insert(int index, BubbleButton item) 
		{
			this.Add(item,index);
		}

		/// <summary>
		/// Returns index of an item.
		/// </summary>
		/// <param name="value">Item to return index for.</param>
		/// <returns>Item at the specified position.</returns>
		public virtual int IndexOf(BubbleButton value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns index of an item with given the item's name.
		/// </summary>
		/// <param name="name">Name of the item.</param>
		/// <returns>Index of the Item with the specified name or -1 if item is not found.</returns>
		public virtual int IndexOf(string name)
		{
			int i=-1;
			foreach(BubbleButton item in List)
			{
				i++;
				if(item.Name==name)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Returns true if given item is contained by this collection.
		/// </summary>
		/// <param name="value">Item to test.</param>
		/// <returns>True if item is part of this collection otherwise false.</returns>
		public virtual bool Contains(BubbleButton value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Returns true if item with given name is part of this collection.
		/// </summary>
		/// <param name="name">Item name.</param>
		/// <returns>True if item is part of this collection otherwise false.</returns>
		public virtual bool Contains(string name)
		{
			foreach(BubbleButton item in List)
			{
				if(item.Name==name)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes an item from the collection.
		/// </summary>
		/// <param name="item">Item to remove.</param>
		public virtual void Remove(BubbleButton item) 
		{
			List.Remove(item);
		}

		/// <summary>
		/// Removes an item from collection at specified index.
		/// </summary>
		/// <param name="index">Index of the item to remove.</param>
		public void Remove(int index)
		{
			this.Remove((BubbleButton)List[index]);
		}

		/// <summary>
		/// Removes item from the collection with specified name.
		/// </summary>
		/// <param name="name">Name of the item to remove.</param>
		public virtual void Remove(string name)
		{
			this.Remove(this[name]);
		}

		/// <summary>
		/// Adds array of the items to the collection.
		/// </summary>
		/// <param name="items">Array of items to add.</param>
		public virtual void AddRange(BubbleButton[] items)
		{
			foreach(BubbleButton item in items)
			{
				this.Add(item);
			}
		}

		/// <summary>
		/// Copy the collection to the array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">The zero-based relative index in array at which copying begins.</param>
		public virtual void CopyTo(BubbleButton[] array, int index) 
		{
			List.CopyTo(array, index);
		}
		#endregion
	}
}
