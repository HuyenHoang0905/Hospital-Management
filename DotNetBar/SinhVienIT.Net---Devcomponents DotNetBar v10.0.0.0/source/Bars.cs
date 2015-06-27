using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;

namespace DevComponents.DotNetBar
{

	/// <summary>
	/// Collection of Bar objects.
	/// </summary>
	public class Bars:System.Collections.CollectionBase,IDisposable
	{
		private DotNetBarManager m_Owner;
		internal Bars(DotNetBarManager Owner)
		{
			m_Owner=Owner;
		}

		internal DotNetBarManager Owner
		{
			get
			{
				return m_Owner;
			}
		}

		/// <summary>
		/// Releases the resources used by the Component.
		/// </summary>
		public void Dispose()
		{
			m_Owner=null;
			List.Clear();
		}

		/// <summary>
		/// Adds an Bar to the end of Bars collection.
		/// </summary>
		/// <param name="bar">The Bar to be added to the end of the Bars collection.</param>
		public void Add(Bar bar)
		{
			if(List.IndexOf(bar)<0)
				List.Add(bar);
			else
				throw new InvalidOperationException("Bar is already in collection.");
		}

		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			Bar bar=(Bar)value;
			bar.Owner=m_Owner;
			bar.ThemeAware=m_Owner.ThemeAware;
			bar.Style=m_Owner.Style;
			IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
			if(ownersupport!=null)
				ownersupport.AddShortcutsFromBar(bar);
			bar.OnAddedToBars();
		}

		/// <summary>
		/// Gets the Bar at the specified index.
		/// </summary>
		public Bar this[int Index]
		{
			get
			{
				return List[Index] as Bar;
			}
		}

		/// <summary>
		/// Gets the Bar with the specified name.
		/// </summary>
		public Bar this[string Name]
		{
			get
			{
				foreach(Bar bar in List)
					if(bar.Name==Name)
						return bar;
				return null;
			}
		}

		/// <summary>
		/// Removes specified bar from collection.
		/// </summary>
		/// <param name="bar">Bar to remove</param>
		public virtual void Remove(Bar bar) 
		{
			List.Remove(bar);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			Bar bar=(Bar)value;
			IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
			if(ownersupport!=null)
				ownersupport.RemoveShortcutsFromBar(bar);

			if(bar.Visible)
				bar.HideBar();

			if(bar.Parent!=null)
			{
				if(bar.Parent is DockSite && ((DockSite)bar.Parent).IsDocumentDock)
					((DockSite)bar.Parent).GetDocumentUIManager().UnDock(bar);
				if(bar.Parent!=null)
					bar.Parent.Controls.Remove(bar);
			}

			base.OnRemoveComplete(index,value);
		}

		protected override void OnClear()
		{
            if (m_Owner != null && m_Owner.GetDesignMode())
                return;

			foreach(Bar bar in List)
			{
                if (bar.GetDesignMode())
                    continue;
				if(bar.AutoHide)
					bar.AutoHide=false;
				if(bar.Visible)
					bar.HideBar();
                if (bar.DockSide == eDockSide.Document && bar.Parent is DockSite && ((DockSite)bar.Parent).GetDocumentUIManager()!=null)
                    ((DockSite)bar.Parent).GetDocumentUIManager().UnDock(bar);
				if(bar!=null && !bar.IsDisposed)
					((IOwnerBarSupport)m_Owner).RemoveShortcutsFromBar(bar);
				if(bar.Parent!=null)
					bar.Parent.Controls.Remove(bar);
                if (!bar.GetDesignMode())
				    bar.Dispose();
			}
		}

		/// <summary>
		/// Determines whether an Bar is in the collection.
		/// </summary>
		/// <param name="bar">The Bar to locate in the collection.</param>
		/// <returns><b>true</b> if item is found in the collection; otherwise, <b>false</b>.</returns>
		public bool Contains(Bar bar)
		{
			return List.Contains(bar);
		}

		/// <summary>
		/// Determines whether bar with given name is in collection.
		/// </summary>
		/// <param name="name">Name of the bar</param>
		/// <returns>True if bar is part of this collection, otherwise false.</returns>
		public bool Contains(string name)
		{
			foreach(Bar bar in this.List)
				if(bar.Name==name)
					return true;
			return false;
		}

		/// <summary>
		/// Returns the zero-based index of the Bar in the collection.
		/// </summary>
		/// <param name="bar">Bar to locate.</param>
		/// <returns></returns>
		public int IndexOf(Bar bar)
		{
			return List.IndexOf(bar);
		}

		public void CopyTo(System.Array array)
		{
			List.CopyTo(array,0);
		}

		internal void ClearNonDocumentBars()
		{
			ArrayList removeList=new ArrayList(this.Count);
			foreach(Bar bar in this.List)
			{
				if(bar.DockSide!=eDockSide.Document)
                    removeList.Add(bar);
			}

			foreach(Bar bar in removeList)
				this.Remove(bar);
		}
	}
}
