using System;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for TabsCollection.
	/// </summary>
	public class TabsCollection:System.Collections.CollectionBase
	{
		#region Private Variables
		private TabStrip m_Owner;
		#endregion

		#region Internal Implementation
        private bool m_IgnoreEvents = false;
		public TabsCollection(TabStrip owner)
		{
			m_Owner=owner;
		}
		public int Add(TabItem item)
		{
			m_Owner.NeedRecalcSize=true;
			return List.Add(item);
		}
		public TabItem this[int index]
		{
			get {return (TabItem)(List[index]);}
			set {List[index] = value;}
		}

        /// <summary>
        /// Get the TabItem with given name. Name comparison is case insensitive.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TabItem this[string name]
        {
            get
            {
                if (name == null) return null;

                int i = GetIndexOf(name);
                if (i >= 0)
                    return this[i];

                return null;
            }
            set
            {
                int i = GetIndexOf(name);
                if (i >= 0)
                    List[i] = value;
            }
        }

        private int GetIndexOf(string name)
        {
            name = name.ToLower();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Name.ToLower() == name) return i;
            }
            return -1;
        }

		public void Insert(int index, TabItem value) 
		{
			value.Visible=true;
			m_Owner.NeedRecalcSize=true;
			List.Insert(index, value);
		}

        internal void _Insert(int index, TabItem value)
        {
            m_IgnoreEvents = true;
            try
            {
                value.Visible = true;
                m_Owner.NeedRecalcSize = true;
                List.Insert(index, value);
            }
            finally
            {
                m_IgnoreEvents = false;
            }
        }

		public int IndexOf(TabItem value) 
		{
			return List.IndexOf(value);
		}

		public bool Contains(TabItem value) 
		{
			return List.Contains(value);
		}

		public void Remove(TabItem value) 
		{
			List.Remove(value);
		}

        internal void _Remove(TabItem value)
        {
            m_IgnoreEvents = true;
            try
            {
                List.Remove(value);
            }
            finally
            {
                m_IgnoreEvents = false;
            }
        }

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
            if (m_IgnoreEvents) return;
			TabItem item=value as TabItem;
			m_Owner.InvokeTabRemoved(item);
			item.SetParent(null);
			item.Visible=true;
			if(m_Owner.SelectedTab==item)
			{
				m_Owner.OnSelectedTabRemoved(index, item);
			}

			m_Owner.NeedRecalcSize=true;
			if(m_Owner.IsDesignMode)
				m_Owner.Refresh();
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
            if (m_IgnoreEvents) return;
			TabItem item=value as TabItem;
			item.SetParent(m_Owner);
			//item.Visible=true;
			m_Owner.OnTabAdded(item);
			m_Owner.NeedRecalcSize=true;
			if(m_Owner.IsDesignMode)
				m_Owner.Refresh();
		}

		public void CopyTo(TabItem[] array, int index) 
		{
			List.CopyTo(array, index);
		}
		protected override void OnClear()
		{
			m_Owner.InternalTabsCleared();
			base.OnClear();
		}
		#endregion
	}
}
