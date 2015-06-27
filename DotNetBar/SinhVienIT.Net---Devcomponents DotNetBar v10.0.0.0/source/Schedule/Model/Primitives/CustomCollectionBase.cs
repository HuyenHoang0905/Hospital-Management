#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace DevComponents.Schedule.Model.Primitives
{
    /// <summary>
    /// Represents custom collection with INotifyPropertyChanged and INotifyCollectionChanged interface support.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public class CustomCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region Events
        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Internal Implementation
        private SimpleMonitor<T> _monitor;

        [NonSerialized]
        private object _syncRoot;
        private IList<T> items;

        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        public CustomCollection()
        {
            this._monitor = new SimpleMonitor<T>();
            this.items = new List<T>();
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="list">List to initialize collection with.</param>
        public CustomCollection(IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            this._monitor = new SimpleMonitor<T>();
            this.items = list;
        }
        /// <summary>
        /// Add item to collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            if (this.items.IsReadOnly)
            {
                throw new NotSupportedException("collection is read-only");
            }
            int count = this.items.Count;
            this.InsertItem(count, item);
        }
        /// <summary>
        /// Remove all items from collection.
        /// </summary>
        public void Clear()
        {
            if (this.items.IsReadOnly)
            {
                throw new NotSupportedException("collection is read-only");
            }
            this.ClearItems();
        }
        /// <summary>
        /// Remove all items from collection.
        /// </summary>
        protected virtual void ClearItems()
        {
            this.CheckReentrancy();
            this.items.Clear();
            OnPropertyChanged(new PropertyChangedEventArgs(this.CountString));
            OnPropertyChanged(new PropertyChangedEventArgs(this.ItemString));
            this.OnCollectionReset();
        }
        /// <summary>
        /// Checks whether collection contains item.
        /// </summary>
        /// <param name="item">Item to look for.</param>
        /// <returns>true if item is in collection.</returns>
        public bool Contains(T item)
        {
            OnCollectionReadAccess();
            return this.items.Contains(item);
        }
        /// <summary>
        /// Copy collection to array.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="index">Index to copy from.</param>
        public void CopyTo(T[] array, int index)
        {
            OnCollectionReadAccess();
            this.items.CopyTo(array, index);
        }
        /// <summary>
        /// Gets enumerator for collection.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            OnCollectionReadAccess();
            return this.items.GetEnumerator();
        }
        /// <summary>
        /// Returns index of an item.
        /// </summary>
        /// <param name="item">Reference to item.</param>
        /// <returns>Index of item.</returns>
        public int IndexOf(T item)
        {
            OnCollectionReadAccess();
            return this.items.IndexOf(item);
        }
        /// <summary>
        /// Insert item at specified location.
        /// </summary>
        /// <param name="index">Index to insert item in.</param>
        /// <param name="item">Item to insert.</param>
        public void Insert(int index, T item)
        {
            if ((index < 0) || (index > this.items.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.InsertItem(index, item);
        }
        /// <summary>
        /// Inserts item.
        /// </summary>
        /// <param name="index">Index to insert item at.</param>
        /// <param name="item">Reference to item.</param>
        protected virtual void InsertItem(int index, T item)
        {
            this.CheckReentrancy();
            this.items.Insert(index, item);
            OnPropertyChanged(new PropertyChangedEventArgs(this.CountString));
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        private static bool IsCompatibleObject(object value)
        {
            if (!(value is T) && ((value != null) || typeof(T).IsValueType))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Removes item from collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>true if item was removed.</returns>
        public bool Remove(T item)
        {
            if (this.items.IsReadOnly)
            {
                throw new NotSupportedException("collection is read-only");
            }
            int index = this.items.IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            this.RemoveItem(index);
            return true;
        }
        /// <summary>
        /// Remove item at specified location.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public void RemoveAt(int index)
        {
            if (this.items.IsReadOnly)
            {
                throw new NotSupportedException("collection is read-only");
            }
            if ((index < 0) || (index >= this.items.Count))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.RemoveItem(index);
        }
        /// <summary>
        /// Remove item at specified location.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        protected virtual void RemoveItem(int index)
        {
            this.CheckReentrancy();

            object item = items[index];
            this.items.RemoveAt(index);
            OnPropertyChanged(new PropertyChangedEventArgs(this.CountString));
            this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }
        /// <summary>
        /// Set item on location.
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="item">Item to assign.</param>
        protected virtual void SetItem(int index, T item)
        {
            this.CheckReentrancy();
            T oldItem = items[index];
            this.items[index] = item;
            OnPropertyChanged(new PropertyChangedEventArgs(this.CountString));
            OnPropertyChanged(new PropertyChangedEventArgs(this.ItemString));
            this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CheckReentrancy();
            OnCollectionReadAccess();
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Rank != 1)
            {
                throw new ArgumentException("Argument array.Rank multi-dimensional not supported");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Argument array non zero lower bound not supported");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index must be non-negative number");
            }
            if ((array.Length - index) < this.Count)
            {
                throw new ArgumentException("array too small");
            }
            T[] localArray = array as T[];
            if (localArray != null)
            {
                this.items.CopyTo(localArray, index);
            }
            else
            {
                Type elementType = array.GetType().GetElementType();
                Type c = typeof(T);
                if (!elementType.IsAssignableFrom(c) && !c.IsAssignableFrom(elementType))
                {
                    throw new ArgumentException("Argument array of invalid type");
                }
                object[] objArray = array as object[];
                if (objArray == null)
                {
                    throw new ArgumentException("Argument array invalid type");
                }
                int count = this.items.Count;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        objArray[index++] = this.items[i];
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("Argument array invalid type");
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            OnCollectionReadAccess();
            return this.items.GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (this.items.IsReadOnly)
            {
                throw new NotSupportedException("collection is read-only");
            }
            VerifyValueType(value);
            this.Add((T)value);
            return (this.Count - 1);
        }

        private static void VerifyValueType(object value)
        {
            if (!IsCompatibleObject(value))
            {
                throw new ArgumentException("value is of wrong type");
            }
        }


        bool IList.Contains(object value)
        {
            return (CustomCollection<T>.IsCompatibleObject(value) && this.Contains((T)value));
        }

        int IList.IndexOf(object value)
        {
            OnCollectionReadAccess();
            if (CustomCollection<T>.IsCompatibleObject(value))
            {
                return this.IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (this.items.IsReadOnly)
            {
                throw new NotSupportedException("collection is read-only");
            }
            VerifyValueType(value);
            this.Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            if (this.items.IsReadOnly)
            {
                throw new NotSupportedException("collection is read-only");
            }
            if (CustomCollection<T>.IsCompatibleObject(value))
            {
                this.Remove((T)value);
            }
        }

        /// <summary>
        /// Returns number of items in collection.
        /// </summary>
        public int Count
        {
            get
            {
                OnCollectionReadAccess();
                return this.items.Count;
            }
        }

        /// <summary>
        /// Returns item at index.
        /// </summary>
        /// <param name="index">Index of item.</param>
        /// <returns>Item at index.</returns>
        public T this[int index]
        {
            get
            {
                OnCollectionReadAccess();
                return this.items[index];
            }
            set
            {
                if (this.items.IsReadOnly)
                {
                    throw new NotSupportedException("collection is read-only");
                }
                if ((index < 0) || (index >= this.items.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.SetItem(index, value);
            }
        }

        /// <summary>
        /// Returns the IList interface for items in collection.
        /// </summary>
        protected IList<T> Items
        {
            get
            {
                OnCollectionReadAccess();
                return this.items;
            }
        }
        /// <summary>
        /// Returns items directly without checks.
        /// </summary>
        /// <returns>List of items.</returns>
        protected IList<T> GetItemsDirect()
        {
            return this.items;
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return this.items.IsReadOnly;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    ICollection items = this.items as ICollection;
                    if (items != null)
                    {
                        this._syncRoot = items.SyncRoot;
                    }
                    else
                    {
                        System.Threading.Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                    }
                }
                return this._syncRoot;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                IList items = this.items as IList;
                return ((items != null) && items.IsFixedSize);
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return this.items.IsReadOnly;
            }
        }

        object IList.this[int index]
        {
            get
            {
                OnCollectionReadAccess();
                return this.items[index];
            }
            set
            {
                VerifyValueType(value);
                this[index] = (T)value;
            }
        }
        /// <summary>
        /// Occurs when collection is read.
        /// </summary>
        protected virtual void OnCollectionReadAccess()
        {
        }
        /// <summary>
        /// Occurs when collection property has changed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        private string CountString
        {
            get { return "Count"; }
        }
        private string ItemString
        {
            get { return "Item[]"; }
        }
        /// <summary>
        /// Blocks the collection re-entrancy.
        /// </summary>
        /// <returns>IDisposable to end re-entrancy</returns>
        protected IDisposable BlockReentrancy()
        {
            this._monitor.Enter();
            return this._monitor;
        }
        /// <summary>
        /// Checks whether call creates re-entrancy.
        /// </summary>
        protected void CheckReentrancy()
        {
            if ((this._monitor.Busy && (this.CollectionChanged != null)) && (this.CollectionChanged.GetInvocationList().Length > 1))
            {
                throw new InvalidOperationException("CustomCollectionReentrancyNotAllowed");
            }
        }


        #region SimpleMonitor
        [Serializable]
        private class SimpleMonitor<TY> : IDisposable
        {
            // Fields
            private int _busyCount;

            // Methods
            public void Dispose()
            {
                this._busyCount--;
            }

            public void Enter()
            {
                this._busyCount++;
            }

            // Properties
            public bool Busy
            {
                get
                {
                    return (this._busyCount > 0);
                }
            }
        }
        #endregion

        #endregion

        #region INotifyCollectionChanged Members
        /// <summary>
        /// Occurs when collection has changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        private void OnCollectionReset()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Called when collection has changed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                using (this.BlockReentrancy())
                {
                    this.CollectionChanged(this, e);
                }
            }
        }
        #endregion
    }

    #region INotifyCollectionChanged
    /// <summary>
    /// Represents collection changed notification interface.
    /// </summary>
    public interface INotifyCollectionChanged
    {
        /// <summary>
        /// Occurs when collection changed.
        /// </summary>
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
    /// <summary>
    /// Defines change actions.
    /// </summary>
    public enum NotifyCollectionChangedAction
    {
        /// <summary>
        /// Items were added.
        /// </summary>
        Add,
        /// <summary>
        /// Items were removed.
        /// </summary>
        Remove,
        /// <summary>
        /// Items were replaced.
        /// </summary>
        Replace,
        /// <summary>
        /// Items were moved.
        /// </summary>
        Move,
        /// <summary>
        /// Collection was reset.
        /// </summary>
        Reset
    }
    /// <summary>
    /// Defines delegate for collection notification events.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);
    /// <summary>
    /// Defines collection change notification event arguments.
    /// </summary>
    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        #region Private Vars
        private NotifyCollectionChangedAction _action;
        private IList _newItems;
        private int _newStartingIndex;
        private IList _oldItems;
        private int _oldStartingIndex;
        #endregion

        /// <summary>
        /// Create new instance of object.
        /// </summary>
        /// <param name="action">Action</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("WrongActionForCtor");
            }
            this.InitializeAdd(action, null, -1);
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Specifies action.</param>
        /// <param name="changedItems">List of changed items.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItems != null)
                {
                    throw new ArgumentException("ResetActionRequiresNullItem");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                {
                    throw new ArgumentNullException("changedItems");
                }
                this.InitializeAddOrRemove(action, changedItems, -1);
            }
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Specifies action.</param>
        /// <param name="changedItem">Item that was changed.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                {
                    throw new ArgumentException("ResetActionRequiresNullItem");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                this.InitializeAddOrRemove(action, new object[] { changedItem }, -1);
            }
        }

        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="newItems">New items in collection.</param>
        /// <param name="oldItems">Old items in collection.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("WrongActionForCtor");
            }
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }
            if (oldItems == null)
            {
                throw new ArgumentNullException("oldItems");
            }
            this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="changedItems">List of changed items.</param>
        /// <param name="startingIndex">Starting index of change.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItems != null)
                {
                    throw new ArgumentException("ResetActionRequiresNullItem");
                }
                if (startingIndex != -1)
                {
                    throw new ArgumentException("ResetActionRequiresIndexMinus1");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                {
                    throw new ArgumentNullException("changedItems");
                }
                if (startingIndex < -1)
                {
                    throw new ArgumentException("IndexCannotBeNegative");
                }
                this.InitializeAddOrRemove(action, changedItems, startingIndex);
            }
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="changedItem">Changed item</param>
        /// <param name="index">Index of change</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                {
                    throw new ArgumentException("ResetActionRequiresNullItem");
                }
                if (index != -1)
                {
                    throw new ArgumentException("ResetActionRequiresIndexMinus1");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                this.InitializeAddOrRemove(action, new object[] { changedItem }, index);
            }
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="newItem">New item</param>
        /// <param name="oldItem">Old item</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("WrongActionForCtor");
            }
            this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, -1, -1);
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="newItems">New items.</param>
        /// <param name="oldItems">Removed items.</param>
        /// <param name="startingIndex">Starting index of change.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("WrongActionForCtor");
            }
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }
            if (oldItems == null)
            {
                throw new ArgumentNullException("oldItems");
            }
            this.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="changedItems">Changed items</param>
        /// <param name="index">New index</param>
        /// <param name="oldIndex">Old index</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Move)
            {
                throw new ArgumentException("WrongActionForCtor");
            }
            if (index < 0)
            {
                throw new ArgumentException("IndexCannotBeNegative");
            }
            this.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="changedItem">Changed item</param>
        /// <param name="index">New index</param>
        /// <param name="oldIndex">Old index</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Move)
            {
                throw new ArgumentException("WrongActionForCtor");
            }
            if (index < 0)
            {
                throw new ArgumentException("IndexCannotBeNegative");
            }
            object[] newItems = new object[] { changedItem };
            this.InitializeMoveOrReplace(action, newItems, newItems, index, oldIndex);
        }
        /// <summary>
        /// Creates new instance of object.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="newItem">New item</param>
        /// <param name="oldItem">Old item</param>
        /// <param name="index">New index</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("WrongActionForCtor");
            }
            this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, index, index);
        }

        private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
        {
            this._action = action;
            this._newItems = (newItems == null) ? null : ArrayList.ReadOnly(newItems);
            this._newStartingIndex = newStartingIndex;
        }

        private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            if (action == NotifyCollectionChangedAction.Add)
            {
                this.InitializeAdd(action, changedItems, startingIndex);
            }
            else if (action == NotifyCollectionChangedAction.Remove)
            {
                this.InitializeRemove(action, changedItems, startingIndex);
            }
            else
            {
                //Invariant.Assert(false, "Unsupported action: {0}", action.ToString());
            }
        }

        private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
        {
            this.InitializeAdd(action, newItems, startingIndex);
            this.InitializeRemove(action, oldItems, oldStartingIndex);
        }

        private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
        {
            this._action = action;
            this._oldItems = (oldItems == null) ? null : ArrayList.ReadOnly(oldItems);
            this._oldStartingIndex = oldStartingIndex;
        }

        /// <summary>
        /// Gets the type of the collection change action.
        /// </summary>
        public NotifyCollectionChangedAction Action
        {
            get
            {
                return this._action;
            }
        }
        /// <summary>
        /// Gets list of newly added items.
        /// </summary>
        public IList NewItems
        {
            get
            {
                return this._newItems;
            }
        }
        /// <summary>
        /// Gets new starting index.
        /// </summary>
        public int NewStartingIndex
        {
            get
            {
                return this._newStartingIndex;
            }
        }
        /// <summary>
        /// Gets list of removed items.
        /// </summary>
        public IList OldItems
        {
            get
            {
                return this._oldItems;
            }
        }
        /// <summary>
        /// Old starting index.
        /// </summary>
        public int OldStartingIndex
        {
            get
            {
                return this._oldStartingIndex;
            }
        }
    }
    #endregion

}
#endif

