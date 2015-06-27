namespace DevComponents.DotNetBar
{
    using System;
	using System.Collections;

    /// <summary>
    ///		Holds reference to all unique items in the DotNetBar.
    /// </summary>
    public class Items:IEnumerable,IDisposable
    {
		private System.Collections.SortedList m_Items;
		private DotNetBarManager m_Owner;

        internal Items(DotNetBarManager objOwner)
        {
			m_Items=new System.Collections.SortedList();
			m_Owner=objOwner;
        }

		public void Dispose()
		{
			this.Clear();
			m_Items=null;
			m_Owner=null;
		}

		public void Clear()
		{
			if(m_Owner!=null)
			{
				foreach(BaseItem item in m_Items.Values)
					((IOwner)m_Owner).RemoveShortcutsFromItem(item);
			}
			m_Items.Clear();
		}

		public void Add(BaseItem objItem)
		{
			if(objItem==null)
				throw new System.ArgumentNullException("Item must be valid value");
			if(objItem.Name==null || objItem.Name=="")
			{
				// Auto assign item name
				objItem.Name="item_"+objItem.Id.ToString();
			}
			if(m_Items.ContainsKey(objItem.Name))
			{
				throw new System.InvalidOperationException("Item with this name already exists");
			}
			
			if(objItem.Parent!=null)
				throw new System.InvalidOperationException("Item already has a Parent. Remove item from Parent first.");
				
			objItem.SetOwner(m_Owner);
			objItem.GlobalItem=true;

			m_Items.Add(objItem.Name,objItem);
		}

		public void AddCopy(BaseItem objItem)
		{
			if(objItem==null)
				throw new System.ArgumentNullException("Item must be valid value");
			if(objItem.Name==null || objItem.Name=="")
			{
				// Auto assign item name
				objItem.Name="item_"+objItem.Id.ToString();
			}
			if(m_Items.ContainsKey(objItem.Name))
				throw new System.InvalidOperationException("Item with this name already exists");
			BaseItem objCopy=objItem.Copy();
			objCopy.SetOwner(m_Owner);
			objCopy.GlobalItem=true;
			m_Items.Add(objCopy.Name,objCopy);
		}

		public void Remove(BaseItem objItemToRemove)
		{
			if(m_Items.ContainsKey(objItemToRemove.Name))
			{
				m_Items.Remove(objItemToRemove.Name);
				objItemToRemove.SetOwner(null);
				((IOwner)m_Owner).RemoveShortcutsFromItem(objItemToRemove);
				//objItemToRemove.GlobalItem=false;
				return;
			}
			
			string sItemName=objItemToRemove.Name;
			foreach(DictionaryEntry o in m_Items)
			{
				BaseItem objItem=o.Value as BaseItem;
				// Name out of sync case
				if(objItem==objItemToRemove)
				{
					m_Items.RemoveAt(m_Items.IndexOfValue(objItem));
					objItemToRemove.SetOwner(null);
					((IOwner)m_Owner).RemoveShortcutsFromItem(objItemToRemove);
                    return;
				}
				if(objItem.SubItems.Count>0)
				{
					if(RemoveItem(sItemName,objItem))
					{
						//objItemToRemove.GlobalItem=false;
						return;
					}
				}
			}

			// This will throw exception...
			m_Items.Remove(objItemToRemove.Name);
			objItemToRemove.SetOwner(null);
		}

		public void Remove(string sItemName)
		{
			Remove(m_Items[sItemName] as BaseItem);
		}
        
		public BaseItem this[string sItemName]
		{
			get
			{
				if(m_Items.ContainsKey(sItemName))
					return m_Items[sItemName] as BaseItem;
				foreach(DictionaryEntry o in m_Items)
				{
					BaseItem objItem=o.Value as BaseItem;
					if(objItem.Name==sItemName)
						return objItem;
					if(objItem.SubItems.Count>0)
					{
						objItem=GetItem(sItemName,objItem);
						if(objItem!=null)
							return objItem;
					}
				}
				throw new InvalidOperationException("Item not found in collection");
				//return null;
			}
		}

		private BaseItem GetItem(string sName, BaseItem objParent)
		{
			BaseItem objItem2=null;
			foreach(BaseItem objItem in objParent.SubItems)
			{
				if(objItem.Name==sName)
					return objItem;
				if(objItem.SubItems.Count>0)
				{
					objItem2=GetItem(sName,objItem);
					if(objItem2!=null)
						return objItem2;
				}
			}
			return null;
		}

		private bool RemoveItem(string sName, BaseItem objParent)
		{
			foreach(BaseItem objItem in objParent.SubItems)
			{
				if(objItem.Name==sName)
				{
					objParent.SubItems.Remove(objItem);
					objItem.SetOwner(null);
					((IOwner)m_Owner).RemoveShortcutsFromItem(objItem);
					return true;
				}
				if(objItem.SubItems.Count>0)
				{
					if(RemoveItem(sName,objItem))
						return true;
				}
			}
			return false;
		}

		//public BaseItem Item(string sItemName)
		//{
		//	return m_Items[sItemName] as BaseItem;
		//}

		public BaseItem this[int iIndex]
		{
			get
			{
				return m_Items.GetByIndex(iIndex) as BaseItem;
			}
		}

		//public BaseItem Item(int iIndex)
		//{
		//	return m_Items.GetByIndex(iIndex) as BaseItem;
		//}

		public int Count
		{
			get
			{
				return m_Items.Count;
			}
		}
		public IEnumerator GetEnumerator()
		{
			return m_Items.GetEnumerator();
		}
		
		public bool Contains(BaseItem objItem)
		{
			return m_Items.ContainsValue(objItem);
		}
		public bool Contains(string sName)
		{
			return m_Items.ContainsKey(sName);
		}
    }
}
