using System;
using System.Text;
using System.Collections;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class MarkupElementCollection : CollectionBase
    {
        #region Private Variables
        private MarkupElement m_Parent = null;
        #endregion

        #region Internal Implementation
        /// <summary>Creates new instance of the class.</summary>
        public MarkupElementCollection(MarkupElement parent)
		{
            m_Parent = parent;
		}

        /// <summary>
        /// Gets or sets the collection parent element.
        /// </summary>
        public MarkupElement Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="MarkupElement">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(MarkupElement MarkupElement)
		{
			return List.Add(MarkupElement);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public MarkupElement this[int index]
		{
			get {return (MarkupElement)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, MarkupElement value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(MarkupElement value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(MarkupElement value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(MarkupElement value) 
		{
			List.Remove(value);
		}

		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
			MarkupElement me=value as MarkupElement;
            if (m_Parent != null)
            {
                me.SetParent(null);
                m_Parent.IsSizeValid = false;
            }
		}
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);
			MarkupElement me=value as MarkupElement;
            if (m_Parent != null)
            {
                me.SetParent(m_Parent);
                m_Parent.IsSizeValid = false;
            }
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(MarkupElement[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the MarkupElement array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(MarkupElement[] array)
		{
			List.CopyTo(array,0);
		}

		protected override void OnClear()
		{
			base.OnClear();
		}

		#endregion
    }
}
