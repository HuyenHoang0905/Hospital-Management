using System;
using System.Collections.ObjectModel;

namespace DevComponents.Instrumentation.Primitives
{
    public class BaseCollection<T> : Collection<T> where T : class, ICloneable, new()
    {
        #region Events

        public event EventHandler<EventArgs> CollectionChanged;

        #endregion

        #region Private variables

        private bool _IsRangeSet;

        #endregion

        #region AddRange

        /// <summary>
        /// Adds a range of items to the collection
        /// </summary>
        /// <param name="items">Array of items to add</param>
        public void AddRange(T[] items)
        {
            try
            {
                _IsRangeSet = true;

                for (int i = 0; i < items.Length; i++)
                    Add(items[i]);
            }
            finally
            {
                _IsRangeSet = false;

                OnCollectionChanged();
            }
        }

        #endregion

        #region RemoveItem

        /// <summary>
        /// Processes list RemoveItem calls
        /// </summary>
        /// <param name="index">Index to remove</param>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            OnCollectionChanged();
        }

        #endregion

        #region InsertItem

        /// <summary>
        /// Processes list InsertItem calls
        /// </summary>
        /// <param name="index">Index to add</param>
        /// <param name="item">Text to add</param>
        protected override void InsertItem(int index, T item)
        {
            if (item != null)
            {
                base.InsertItem(index, item);

                OnCollectionChanged();
            }
        }

        #endregion

        #region SetItem

        /// <summary>
        /// Processes list SetItem calls (e.g. replace)
        /// </summary>
        /// <param name="index">Index to replace</param>
        /// <param name="newItem">Text to replace</param>
        protected override void SetItem(int index, T newItem)
        {
            base.SetItem(index, newItem);

            OnCollectionChanged();
        }

        #endregion

        #region ClearItems

        /// <summary>
        /// Processes list Clear calls (e.g. remove all)
        /// </summary>
        protected override void ClearItems()
        {
            if (Count > 0)
                base.ClearItems();

            OnCollectionChanged();
        }

        #endregion

        #region OnCollectionChanged

        private void OnCollectionChanged()
        {
            if (CollectionChanged != null)
            {
                if (_IsRangeSet == false)
                    CollectionChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            BaseCollection<T> copy = new BaseCollection<T>();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(BaseCollection<T> copy)
        {
            foreach (T item in this)
                copy.Add((T)item.Clone());
        }

        #endregion
    }

    public class GenericCollection<T> : BaseCollection<T> where T : GaugeItem, new()
    {
        #region Name indexer

        public T this[string name]
        {
            get
            {
                foreach (T item in Items)
                {
                    if (item.Name != null && item.Name.Equals(name))
                        return (item);
                }

                return (null);
            }
        }

        #endregion
    }
}
