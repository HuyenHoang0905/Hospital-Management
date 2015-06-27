#if FRAMEWORK20
using System;
using System.Text;
using System.Collections;

namespace DevComponents.Editors
{
    public class VisualItemCollection : CollectionBase
    {
        #region Internal Implementation
        /// <summary>Creates new instance of the class.</summary>
        public VisualItemCollection() { }

        /// <summary>Creates new instance of the class.</summary>
        public VisualItemCollection(VisualGroup parent)
        {
            _Parent = parent;
        }

        private VisualGroup _Parent = null;
        internal VisualGroup Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        /// <summary>
        /// Adds new object to the collection.
        /// </summary>
        /// <param name="item">Object to add.</param>
        /// <returns>Index of newly added object.</returns>
        public int Add(VisualItem item)
        {
            return List.Add(item);
        }

        /// <summary>
        /// Adds array of new objects to the collection.
        /// </summary>
        /// <param name="items">Array of object to add.</param>
        public void AddRange(VisualItem[] items)
        {
            foreach (VisualItem item in items)
                this.Add(item);
        }

        /// <summary>
        /// Adds array of new objects to the collection.
        /// </summary>
        /// <param name="items">Array of object to add.</param>
        public void AddRange(IList items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                this.Add((VisualItem)items[i]);
            }
        }

        /// <summary>
        /// Returns reference to the object in collection based on it's index.
        /// </summary>
        public VisualItem this[int index]
        {
            get { return (VisualItem)(List[index]); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Inserts new object into the collection.
        /// </summary>
        /// <param name="index">Position of the object.</param>
        /// <param name="value">Object to insert.</param>
        public void Insert(int index, VisualItem value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Returns index of the object inside of the collection.
        /// </summary>
        /// <param name="value">Reference to the object.</param>
        /// <returns>Index of the object.</returns>
        public int IndexOf(VisualItem value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Returns whether collection contains specified object.
        /// </summary>
        /// <param name="value">Object to look for.</param>
        /// <returns>true if object is part of the collection, otherwise false.</returns>
        public bool Contains(VisualItem value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Removes specified object from the collection.
        /// </summary>
        /// <param name="value"></param>
        public void Remove(VisualItem value)
        {
            List.Remove(value);
        }

        protected override void OnRemove(int index, object value)
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, new VisualItem[] { (VisualItem)value }, eCollectionChangeType.Removing));
            }
            base.OnRemove(index, value);
        }
        protected override void OnRemoveComplete(int index, object value)
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, new VisualItem[] { (VisualItem)value }, eCollectionChangeType.Removed));
            }
            base.OnRemoveComplete(index, value);
        }

        protected override void OnInsert(int index, object value)
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[] { (VisualItem)value }, null, eCollectionChangeType.Adding));
            }
            base.OnInsert(index, value);
        }
        protected override void OnInsertComplete(int index, object value)
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[] { (VisualItem)value }, null, eCollectionChangeType.Added));
            }
            base.OnInsertComplete(index, value);
        }

        protected override void OnClear()
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, null, eCollectionChangeType.Clearing));
            }
            base.OnClear();
        }

        protected override void OnClearComplete()
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, null, eCollectionChangeType.Cleared));
            }
            base.OnClearComplete();
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[] { (VisualItem)oldValue },
                    new VisualItem[] { (VisualItem)newValue }, eCollectionChangeType.Removing));
            }
            base.OnSet(index, oldValue, newValue);
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (_Parent != null)
            {
                _Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[] { (VisualItem)oldValue },
                    new VisualItem[] { (VisualItem)newValue }, eCollectionChangeType.Removed));
            }
            base.OnSetComplete(index, oldValue, newValue);
        }

        /// <summary>
        /// Copies collection into the specified array.
        /// </summary>
        /// <param name="array">Array to copy collection to.</param>
        /// <param name="index">Starting index.</param>
        public void CopyTo(VisualItem[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Copies contained items to the VisualItem array.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        internal void CopyTo(VisualItem[] array)
        {
            List.CopyTo(array, 0);
        }

        #endregion
    }

    public class CollectionChangedInfo
    {
        public CollectionChangedInfo(VisualItem[] added, VisualItem[] removed, eCollectionChangeType changeType)
        {
            _Added = added;
            _Removed = removed;
            _ChangeType = changeType;
        }

        private VisualItem[] _Added;
        public VisualItem[] Added
        {
            get { return _Added; }
        }

        private VisualItem[] _Removed;
        public VisualItem[] Removed
        {
            get { return _Removed; }
        }

        private eCollectionChangeType _ChangeType;
        public eCollectionChangeType ChangeType
        {
            get { return _ChangeType; }
        }
    }

    internal class VisualCollectionEnumerator : IEnumerable, IEnumerator
    {
        private IList _ParentCollection = null;
        private int _CurrentIndex = -1;
        private int _Direction = 1, _Start = -1, _End = -1;

        public VisualCollectionEnumerator(VisualItemCollection parentCollection, bool rightToLeft)
        {
            _ParentCollection = parentCollection;
            if (rightToLeft)
            {
                _Direction = -1;
                _Start = parentCollection.Count - 1;
                _End = -1;
            }
            else
            {
                _Start = 0;
                _End = parentCollection.Count;
            }
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        #endregion

        #region IEnumerator Members

        public object Current
        {
            get
            {
                if (_CurrentIndex < 0)
                    throw new InvalidOperationException("IEnumerator Pointer is invalid. Use MoveNext to advance enumerator.");
                return _ParentCollection[_CurrentIndex];
            }
        }

        internal int CurrentIndex
        {
            get
            {
                return _CurrentIndex;
            }
            set
            {
                _CurrentIndex = value;
            }
        }

        public bool MoveNext()
        {
            if (_CurrentIndex == _End && _CurrentIndex >= 0)
                throw new InvalidOperationException("Enumerator cannot advance beyond the boundaries of array. Use Reset to reset enumerator");

            if (_CurrentIndex == -1) _CurrentIndex = _Start - _Direction;

            _CurrentIndex += _Direction;
            if (_CurrentIndex == _End) return false;

            return true;
        }

        public void Reset()
        {
            _CurrentIndex = -1;
        }
        #endregion
    }
}
#endif

