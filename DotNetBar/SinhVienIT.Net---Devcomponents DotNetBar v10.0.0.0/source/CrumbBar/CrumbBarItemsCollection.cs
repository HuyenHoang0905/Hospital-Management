using System;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents collection of CrumbBarItem buttons.
    /// </summary>
    public class CrumbBarItemsCollection : CollectionBase
    {
        #region Private Variables
        private CrumbBar _Parent = null;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the CrumbBarItemsCollection class.
        /// </summary>
        /// <param name="parent"></param>
        public CrumbBarItemsCollection(CrumbBar parent)
        {
            _Parent = parent;
        }

        /// <summary>
        /// Gets or sets the node this collection is associated with.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CrumbBar Parent
        {
            get { return _Parent; }
        }
        /// <summary>
        /// Sets the node collection belongs to.
        /// </summary>
        /// <param name="parent">CrumbBarItem that is parent of this collection.</param>
        internal void SetParent(CrumbBar parent)
        {
            _Parent = parent;
        }

        /// <summary>
        /// Adds new object to the collection.
        /// </summary>
        /// <param name="ch">Object to add.</param>
        /// <returns>Index of newly added object.</returns>
        public int Add(CrumbBarItem ch)
        {
            return List.Add(ch);
        }
        /// <summary>
        /// Returns reference to the object in collection based on it's index.
        /// </summary>
        public CrumbBarItem this[int index]
        {
            get { return (CrumbBarItem)(List[index]); }
            set { List[index] = value; }
        }
        /// <summary>
        /// Returns reference to the object in collection based on it's name.
        /// </summary>
        public CrumbBarItem this[string name]
        {
            get { return GetByName(name); }
            set 
            {
                int index = GetIndexByName(name);
                if (index == -1)
                    throw new ArgumentException("name cannot be found in this collection");

                List[index] = value; 
            }
        }

        /// <summary>
        /// Inserts new object into the collection.
        /// </summary>
        /// <param name="index">Position of the object.</param>
        /// <param name="value">Object to insert.</param>
        public void Insert(int index, CrumbBarItem value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Returns index of the object inside of the collection.
        /// </summary>
        /// <param name="value">Reference to the object.</param>
        /// <returns>Index of the object.</returns>
        public int IndexOf(CrumbBarItem value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Returns whether collection contains specified object.
        /// </summary>
        /// <param name="value">Object to look for.</param>
        /// <returns>true if object is part of the collection, otherwise false.</returns>
        public bool Contains(CrumbBarItem value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Removes specified object from the collection.
        /// </summary>
        /// <param name="value"></param>
        public void Remove(CrumbBarItem value)
        {
            List.Remove(value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            CrumbBarItem item = (CrumbBarItem)oldValue;
            item.SetOwner(null);
            item = (CrumbBarItem)newValue;
            item.SetOwner(_Parent);

            base.OnSet(index, oldValue, newValue);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            CrumbBarItem item = (CrumbBarItem)value;
            item.SetOwner(null);
            if (item.IsSelected)
                _Parent.SetSelectedItem(null, eEventSource.Code);

            base.OnRemoveComplete(index, value);
        }
        protected override void OnInsertComplete(int index, object value)
        {
            CrumbBarItem item = (CrumbBarItem)value;
            item.SetOwner(_Parent);
            item.ContainerControl = _Parent;
            item.Style = eDotNetBarStyle.Office2007;
            base.OnInsertComplete(index, value);
        }

        /// <summary>
        /// Copies collection into the specified array.
        /// </summary>
        /// <param name="array">Array to copy collection to.</param>
        /// <param name="index">Starting index.</param>
        public void CopyTo(CrumbBarItem[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Copies contained items to the CrumbBarItem array.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        internal void CopyTo(CrumbBarItem[] array)
        {
            List.CopyTo(array, 0);
        }

        protected override void OnClear()
        {
            foreach (CrumbBarItem item in List)
            {
                item.SetOwner(null);
            }
            
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            if (_Parent != null)
                _Parent.OnItemsCleared();
            base.OnClearComplete();
        }

        private CrumbBarItem GetByName(string name)
        {
            foreach (CrumbBarItem d in this.List)
            {
                if (d.Name == name)
                    return d;
            }
            return null;
        }

        private int GetIndexByName(string name)
        {
            for (int i = 0; i < this.List.Count; i++)
            {
                CrumbBarItem item = this[i];
                if (item.Name == name)
                    return i;
            }
            
            return -1;
        }
        #endregion
    }
}
