using System.ComponentModel;
namespace DevComponents.DotNetBar
{
	using System;
	using System.Collections;
	/// <summary>
	///		Summary description for ShortcutList.
	/// </summary>
	public class ShortcutsCollection:System.Collections.CollectionBase
	{
		private BaseItem m_Parent=null;
		public ShortcutsCollection(BaseItem parent)
		{
			m_Parent=parent;
		}
		public int Add(eShortcut key)
		{
			int iRet=0;
			iRet=List.Add(key);
			return iRet;
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			RefreshOwnerShortcuts();
		}
		public eShortcut this[int index]
		{
			get {return (eShortcut)(List[index]);}
			set {List[index] = value;}
		}

		public void Insert(int index, eShortcut value) 
		{
			List.Insert(index, value);
			RefreshOwnerShortcuts();
		}

		public int IndexOf(eShortcut value) 
		{
			return List.IndexOf(value);
		}

		public string ToString(string Delimiter)
		{
			if(List.Count==0)
				return "";

			System.Text.StringBuilder sb=new System.Text.StringBuilder(List.Count*(2+Delimiter.Length));int c=List.Count-1;
			for(int i=0;i<c;i++)
				sb.Append(((int)List[i]).ToString()+Delimiter);

			sb.Append(((int)List[c]).ToString());

			return sb.ToString();
		}

		public void FromString(string Data, string Delimiter)
		{
			List.Clear();
			string[] str=Data.Split(Delimiter.ToCharArray());
			foreach(string s in str)
				List.Add((eShortcut)System.Xml.XmlConvert.ToInt32(s));
		}

		public bool Contains(eShortcut value) 
		{
			return List.Contains(value);
		}

		public void Remove(eShortcut value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			RefreshOwnerShortcuts();
		}

		protected override void OnClear()
		{
			base.OnClear();
			RemoveOwnerShortcuts();
		}

		public void CopyTo(eShortcut[] array, int index) 
		{
			List.CopyTo(array, index);
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public BaseItem Parent
		{
			get
			{
				return m_Parent;
			}
			set
			{
				m_Parent=value;
                if (m_Parent != null)
                    m_Parent.RefreshShortcutString();
			}
		}
		private void RefreshOwnerShortcuts()
		{
            if (m_Parent != null)
                m_Parent.RefreshShortcutString();

			IOwner owner=null;
			if(m_Parent!=null)
				owner=m_Parent.GetOwner() as IOwner;
			if(m_Parent!=null && owner!=null)
			{
				owner.RemoveShortcutsFromItem(m_Parent);
				owner.AddShortcutsFromItem(m_Parent);
			}
		}
		private void RemoveOwnerShortcuts()
		{
			IOwner owner=null;
			if(m_Parent!=null)
				owner=m_Parent.GetOwner() as IOwner;
			if(m_Parent!=null && owner!=null)
			{
				owner.RemoveShortcutsFromItem(m_Parent);
			}
		}
	}
}
