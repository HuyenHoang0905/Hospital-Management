using System;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for Popups.
	/// </summary>
	public class ContextMenusCollection:System.Collections.CollectionBase
	{
		private DotNetBarManager m_Owner;
		public ContextMenusCollection(DotNetBarManager owner)
		{
			m_Owner=owner;
		}
		public int Add(BaseItem item)
		{
			item.SetOwner(m_Owner);
			item.Visible=false;
			item.Displayed=false;
			return List.Add(item);
		}
		public BaseItem this[int index]
		{
			get {return (BaseItem)(List[index]);}
			set {List[index] = value;}
		}
		public BaseItem this[string name]
		{
			get {return (BaseItem)(List[this.IndexOf(name)]);}
			set {List[this.IndexOf(name)] = value;}
		}

		public void Insert(int index, BaseItem value) 
		{
			value.Visible=false;
			value.Displayed=false;
			List.Insert(index, value);
		}

		public int IndexOf(BaseItem value) 
		{
			return List.IndexOf(value);
		}

		public int IndexOf(string name)
		{
			int i=-1;
			foreach(BaseItem item in List)
			{
				i++;
				if(item.Name==name)
					return i;
			}
			return -1;
		}

		public bool Contains(BaseItem value) 
		{
			return List.Contains(value);
		}

		public bool Contains(string name)
		{
			foreach(BaseItem item in List)
			{
				if(item.Name==name)
					return true;
			}
			return false;
		}

		public void Remove(BaseItem value) 
		{
			List.Remove(value);
		}

		public void CopyTo(BaseItem[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		protected override void OnClear()
		{
			IOwner owner=m_Owner as IOwner;
			if(List.Count>0 && owner!=null)
			{
				foreach(BaseItem objSub in this)
				{
					if(owner!=null)
						owner.RemoveShortcutsFromItem(objSub);
				}
			}
			base.OnClear();
		}

		internal void SetOwner(DotNetBarManager owner)
		{
			m_Owner=owner;
		}
	}
}
