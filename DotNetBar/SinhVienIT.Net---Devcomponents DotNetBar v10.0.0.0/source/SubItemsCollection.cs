using System;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for SubItemsCollection.
	/// </summary>
	public class SubItemsCollection:System.Collections.CollectionBase
	{

		private BaseItem m_ParentItem;
		protected bool m_IgnoreEvents=false;
		private bool m_AllowParentRemove=true;

		internal SubItemsCollection(BaseItem parent)
		{
			m_ParentItem=parent;
		}
		internal int _Add(BaseItem item)
		{
			m_IgnoreEvents=true;
			m_AllowParentRemove=false;
			int i=0;
			try
			{
				i=List.Add(item);
			}
			finally
			{
				m_IgnoreEvents=false;
				m_AllowParentRemove=true;
			}
			return i;
		}
		internal void _Add(BaseItem item, int Position)
		{
			m_IgnoreEvents=true;
			m_AllowParentRemove=false;
			try
			{
                if (Position >= 0)
                    List.Insert(Position, item);
                else
                    List.Add(item);
			}
			finally
			{
				m_IgnoreEvents=false;
				m_AllowParentRemove=true;
			}
		}
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
		internal bool AllowParentRemove
		{
			get {return m_AllowParentRemove;}
			set {m_AllowParentRemove=value;}
		}

		public virtual int Add(BaseItem item)
		{
			return Add(item,-1);
		}
		public virtual  int Add(BaseItem item, int Position)
		{
			int iRet=Position;
            if (this.Contains(item))
            {
                return this.IndexOf(item);
            }

			if(Position>=0)
				List.Insert(Position,item);
			else
				iRet=List.Add(item);

			return iRet;
		}

		protected override void OnInsert(int index,object value)
		{
			if(value is BaseItem)
			{
				BaseItem item=value as BaseItem;

				if(m_AllowParentRemove && item.Parent!=null && item.Parent!=m_ParentItem && item.Parent.SubItems.Contains(item))
					item.Parent.SubItems.Remove(item);
			}
			base.OnInsert(index,value);
		}

		protected override void OnInsertComplete(int index,object value)
		{
			if(!m_IgnoreEvents)
			{
				BaseItem item=value as BaseItem;

				object objItemContainer=item.ContainerControl;
				item.Style=m_ParentItem.Style;
				item.Orientation=m_ParentItem.Orientation;
				item.ThemeAware=m_ParentItem.ThemeAware;
				item.SetParent(m_ParentItem);

				IOwner owner=m_ParentItem.GetOwner() as IOwner;
				if(owner!=null)
					owner.AddShortcutsFromItem(item);
                if (m_ParentItem != null && m_ParentItem is PopupItem && m_ParentItem.Expanded && 
                    ((PopupItem)m_ParentItem).PopupType != ePopupType.Container)
                {
                    item.ContainerControl = ((PopupItem)m_ParentItem).PopupControl;
                    item.OnContainerChanged(objItemContainer);
                }
                else
                {
                    item.ContainerControl = null;
                    item.OnContainerChanged(objItemContainer);
                }
				objItemContainer=null;

				item.SetDesignMode(m_ParentItem.DesignMode);

				m_ParentItem.NeedRecalcSize=true;
				m_ParentItem.OnItemAdded(item);
			}

			base.OnInsertComplete(index,value);
		}

		public virtual BaseItem this[int index]
		{
			get {return (BaseItem)(List[index]);}
			set {List[index] = value;}
		}
		public  virtual BaseItem this[string name]
		{
			get {return (BaseItem)(List[this.IndexOf(name)]);}
			set {List[this.IndexOf(name)] = value;}
		}

		public virtual void Insert(int index, BaseItem item) 
		{
			this.Add(item,index);
		}

        internal void _Insert(int index, BaseItem item)
        {
            m_IgnoreEvents = true;
            try { List.Insert(index, item); }
            finally { m_IgnoreEvents = false; }
        }

		public virtual  int IndexOf(BaseItem value) 
		{
			return List.IndexOf(value);
		}

		public virtual  int IndexOf(string name)
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

		public virtual bool Contains(BaseItem value) 
		{
			return List.Contains(value);
		}

		public virtual bool Contains(string name)
		{
			foreach(BaseItem item in List)
			{
				if(item.Name==name)
					return true;
			}
			return false;
		}

		public virtual void Remove(BaseItem item) 
		{
			List.Remove(item);
		}

		public virtual void RemoveRange(BaseItem[] items) 
		{
			foreach(BaseItem item in items)
				this.Remove(item);
		}

		protected override void OnRemove(int index,object value)
		{
			RemoveInternal(index,value);
			base.OnRemove(index,value);
		}

		protected virtual void RemoveInternal(int index,object value)
		{
			// Raise event before item is actually removed so the item is able to clean its state
			// See override in PopupItem
			if(!m_IgnoreEvents)
			{
				BaseItem item=value as BaseItem;
				
				item.OnBeforeItemRemoved(null);
				m_ParentItem.OnBeforeItemRemoved(item);

				IOwner owner=m_ParentItem.GetOwner() as IOwner;
				if(owner!=null)
					owner.RemoveShortcutsFromItem(item);
			}

		}

		protected override void OnRemoveComplete(int index,object value)
		{
			RemoveCompleteInternal(index,value);
			base.OnRemoveComplete(index,value);
		}

		protected virtual void RemoveCompleteInternal(int index, object value)
		{
			if(!m_IgnoreEvents)
			{
				BaseItem item=value as BaseItem;

				item.SetParent(null);
				item.OnAfterItemRemoved(null);
				item.ContainerControl=null;
				m_ParentItem.OnAfterItemRemoved(item);
				m_ParentItem.NeedRecalcSize=true;
				m_ParentItem.Refresh();
			}
		}

        internal void _RemoveAt(int index)
        {
            m_IgnoreEvents = true;
            try { List.RemoveAt(index); }
            finally { m_IgnoreEvents = false; }
        }

		internal void _Remove(BaseItem item)
		{
			m_IgnoreEvents=true;
			try{List.Remove(item);}
			finally{m_IgnoreEvents=false;}
		}

		public void Remove(int index)
		{
			this.Remove((BaseItem)List[index]);
		}

		public virtual void Remove(string name)
		{
			this.Remove(this[name]);
		}

		protected override void OnClear()
		{
			if(!m_IgnoreEvents)
			{
				IOwner owner=m_ParentItem.GetOwner() as IOwner;
				if(m_ParentItem!=null)
				    m_ParentItem.SuspendLayout = true;
                try
                {
                    if (List.Count > 0)
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            BaseItem item = this[i];
                            RemoveInternal(i, item);
                            RemoveCompleteInternal(i, item);
                        }
                    }
                }
                finally
                {
                    if (m_ParentItem != null)
                        m_ParentItem.SuspendLayout = false;
                }
			    owner=null;

				if(m_ParentItem!=null)
				{
					m_ParentItem.OnSubItemsClear();
				}
			}
			//base.OnClear();
		}

		public virtual void AddRange(BaseItem[] items)
		{
            foreach (BaseItem item in items)
            {
                this.Add(item);
            }
		}

        public virtual void InsertRange(int startPosition, BaseItem[] items)
        {
            foreach (BaseItem item in items)
            {
                List.Insert(startPosition, item);
                startPosition++;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
		public void CopyTo(System.Collections.ArrayList list)
		{
			if(list==null)
				return;
			foreach(BaseItem item in this)
				list.Add(item);
		}

		public virtual void CopyTo(BaseItem[] array, int index) 
		{
			List.CopyTo(array, index);
		}

        internal void CopyToFromIndex(BaseItem[] array, int fromCollectionIndex)
        {
            for (int i = fromCollectionIndex; i < List.Count; i++)
            {
                array[i - fromCollectionIndex] = this[i];
            }
        }

        internal bool IgnoreEvents
        {
            get { return m_IgnoreEvents; }
            set { m_IgnoreEvents = value; }
        }
	}
}
