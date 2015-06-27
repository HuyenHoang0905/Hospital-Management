using System;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents collection of tab panels.
	/// </summary>
	public class TabPanelsCollection:System.Collections.CollectionBase
	{
		private TabControl m_Owner;
		public TabPanelsCollection(TabControl owner)
		{
			m_Owner=owner;
		}
		public int Add(TabPanel item)
		{
			m_Owner.NeedRecalcSize=true;
			return List.Add(item);
		}
		public TabPanel this[int index]
		{
			get {return (TabPanel)(List[index]);}
			set {List[index] = value;}
		}
		public void Insert(int index, TabPanel value) 
		{
			value.Visible=true;
			m_Owner.NeedRecalcSize=true;
			List.Insert(index, value);
		}

		public int IndexOf(TabPanel value) 
		{
			return List.IndexOf(value);
		}

		public bool Contains(TabPanel value) 
		{
			return List.Contains(value);
		}

		public void Remove(TabPanel value) 
		{
			List.Remove(value);
		}
		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			TabPanel item=value as TabPanel;
			item.Visible=true;
			if(m_Owner.SelectedTab==item)
				m_Owner.SelectedTab=null;

			m_Owner.NeedRecalcSize=true;
			if(m_Owner._DesignMode)
				m_Owner.Refresh();
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			TabPanel item=value as TabPanel;
			item.Visible=true;
			m_Owner.OnTabAdded(item);
			m_Owner.NeedRecalcSize=true;
			if(m_Owner._DesignMode)
				m_Owner.Refresh();
		}

		public void CopyTo(TabPanel[] array, int index) 
		{
			List.CopyTo(array, index);
		}
		protected override void OnClear()
		{
			m_Owner.OnTabsCleared();
			base.OnClear();
		}
	}
}
