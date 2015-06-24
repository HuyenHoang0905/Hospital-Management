using System;
using System.Text;
using System.Collections;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines collection for Office2007RibbonTabItemColorTable items.
    /// </summary>
    public class Office2007RibbonTabItemColorTableCollection : CollectionBase
    {
        #region Internal Implementation
        /// <summary>Creates new instance of the class.</summary>
        public Office2007RibbonTabItemColorTableCollection() { }

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="Office2007ButtonItemColorTable">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(Office2007RibbonTabItemColorTable colorTable)
		{
            return List.Add(colorTable);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public Office2007RibbonTabItemColorTable this[int index]
		{
			get {return (Office2007RibbonTabItemColorTable)(List[index]);}
			set {List[index] = value;}
		}

        /// <summary>
        /// Returns reference to the object in collection based on it's index.
        /// </summary>
        public Office2007RibbonTabItemColorTable this[string name]
        {
            get
            {
                foreach (Office2007RibbonTabItemColorTable ct in this.List)
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
		public void Insert(int index, Office2007RibbonTabItemColorTable value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(Office2007RibbonTabItemColorTable value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(Office2007RibbonTabItemColorTable value) 
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
            foreach (Office2007RibbonTabItemColorTable ct in this.List)
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
		public void Remove(Office2007RibbonTabItemColorTable value) 
		{
			List.Remove(value);
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(Office2007RibbonTabItemColorTable[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the Office2007ButtonItemColorTable array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(Office2007RibbonTabItemColorTable[] array)
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
