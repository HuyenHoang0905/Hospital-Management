using System;
using System.Collections;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Collection for Office2007RibbonTabGroupColorTable type.
    /// </summary>
    public class Office2007RibbonTabGroupColorTableCollection : CollectionBase
    {
        #region Internal Implementation
        /// <summary>Creates new instance of the class.</summary>
        public Office2007RibbonTabGroupColorTableCollection() { }

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="Office2007RibbonTabItemColorTable">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(Office2007RibbonTabGroupColorTable colorTable)
		{
            return List.Add(colorTable);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public Office2007RibbonTabGroupColorTable this[int index]
		{
			get {return (Office2007RibbonTabGroupColorTable)(List[index]);}
			set {List[index] = value;}
		}

        /// <summary>
        /// Returns reference to the object in collection based on it's index.
        /// </summary>
        public Office2007RibbonTabGroupColorTable this[string name]
        {
            get
            {
                foreach (Office2007RibbonTabGroupColorTable ct in this.List)
                {
                    if (ct.Name == name)
                        return ct;
                }
                return null;
            }
        }

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, Office2007RibbonTabGroupColorTable value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(Office2007RibbonTabGroupColorTable value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(Office2007RibbonTabGroupColorTable value) 
		{
			return List.Contains(value);
		}

        /// <summary>
        /// Returns whether collection contains object with specified name.
        /// </summary>
        /// <param name="name">Name of the object to look for</param>
        /// <returns>true if object with given name is part of the collection otherwise false</returns>
        public bool Contains(string name)
        {
            foreach (Office2007RibbonTabGroupColorTable ct in this.List)
            {
                if (ct.Name == name)
                    return true;
            }
            return false;
        }

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(Office2007RibbonTabGroupColorTable value) 
		{
			List.Remove(value);
		}

        //protected override void OnRemoveComplete(int index,object value)
        //{
        //    base.OnRemoveComplete(index,value);
        //    Office2007RibbonTabItemColorTable me=value as Office2007RibbonTabItemColorTable;
        //    if (m_Parent != null)
        //    {
        //        me.SetParent(null);
        //        m_Parent.IsSizeValid = false;
        //    }
        //}
        //protected override void OnInsertComplete(int index,object value)
        //{
        //    base.OnInsertComplete(index,value);
        //    Office2007RibbonTabItemColorTable me=value as Office2007RibbonTabItemColorTable;
        //    if (m_Parent != null)
        //    {
        //        me.SetParent(m_Parent);
        //        m_Parent.IsSizeValid = false;
        //    }
        //}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(Office2007RibbonTabGroupColorTable[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the Office2007RibbonTabItemColorTable array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(Office2007RibbonTabGroupColorTable[] array)
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
