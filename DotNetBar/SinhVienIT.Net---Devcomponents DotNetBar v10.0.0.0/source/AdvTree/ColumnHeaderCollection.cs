using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DevComponents.AdvTree
{
    /// <summary>
    /// Represents collection for ColumnHeader objects.
    /// </summary>
    public class ColumnHeaderCollection : CollectionBase
    {
        #region Private Variables
        private Node _ParentNode = null;
        private AdvTree _Parent = null;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ColumnHeaderCollection()
        {
        }
        /// <summary>
        /// Gets or sets the node this collection is associated with.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Node ParentNode
        {
            get { return _ParentNode; }
        }
        /// <summary>
        /// Sets the node collection belongs to.
        /// </summary>
        /// <param name="parent">ColumnHeader that is parent of this collection.</param>
        internal void SetParentNode(Node parent)
        {
            _ParentNode = parent;
        }

        internal AdvTree Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        /// <summary>
        /// Adds new object to the collection.
        /// </summary>
        /// <param name="ch">Object to add.</param>
        /// <returns>Index of newly added object.</returns>
        public int Add(ColumnHeader ch)
        {
            return List.Add(ch);
        }
        /// <summary>
        /// Returns reference to the object in collection based on it's index.
        /// </summary>
        public ColumnHeader this[int index]
        {
            get { return (ColumnHeader)(List[index]); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Returns reference to the object in collection based on it's name.
        /// </summary>
        public ColumnHeader this[string name]
        {
            get 
            {
                int index = IndexOf(name);
                if (index == -1) return null;
                return this[index];
            }
            set { this[IndexOf(name)] = value; }
        }

        /// <summary>
        /// Inserts new object into the collection.
        /// </summary>
        /// <param name="index">Position of the object.</param>
        /// <param name="value">Object to insert.</param>
        public void Insert(int index, ColumnHeader value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Returns index of the object inside of the collection.
        /// </summary>
        /// <param name="value">Reference to the object.</param>
        /// <returns>Index of the object.</returns>
        public int IndexOf(ColumnHeader value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Returns index of the object inside of the collection.
        /// </summary>
        /// <param name="name">Name of column to return index for.</param>
        /// <returns>Index of the column or -1 if column not found.</returns>
        public int IndexOf(string name)
        {
            for (int i = 0; i < this.List.Count; i++)
            {
                if (this[i].Name == name) return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns index of the object inside of the collection based on column DataFieldName.
        /// </summary>
        /// <param name="dataFieldName">DataFieldName of column to return index for.</param>
        /// <returns>Index of the column or -1 if column not found.</returns>
        public int IndexOfDataField(string dataFieldName)
        {
            dataFieldName = dataFieldName.ToLower();
            for (int i = 0; i < this.List.Count; i++)
            {
                if (this[i].DataFieldName.ToLower() == dataFieldName) return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns index of the object inside of the collection based on column DataFieldName.
        /// </summary>
        /// <param name="fieldName">DataFieldName of column to return index for.</param>
        /// <returns>Index of the column or -1 if column not found.</returns>
        internal int IndexOfField(string fieldName)
        {
            fieldName = fieldName.ToLower();
            for (int i = 0; i < this.List.Count; i++)
            {
                ColumnHeader header = this[i];
                if (header.Tag is BindingMemberInfo && ((BindingMemberInfo)header.Tag).BindingField.ToLower() == fieldName)
                    return i;
                if (header.DataFieldName.ToLower() == fieldName) return i;
                if (header.Name == fieldName) return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns whether collection contains specified object.
        /// </summary>
        /// <param name="value">Object to look for.</param>
        /// <returns>true if object is part of the collection, otherwise false.</returns>
        public bool Contains(ColumnHeader value)
        {
            return List.Contains(value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (oldValue is ColumnHeader)
            {
                ColumnHeader header = (ColumnHeader)oldValue;
                if (header.SortDirection != eSortDirection.None) IsSorted = false;
            }
            if (newValue is ColumnHeader)
            {
                ColumnHeader header = (ColumnHeader)newValue;
                if (header.SortDirection != eSortDirection.None) IsSorted = true;
            }
            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Removes specified object from the collection.
        /// </summary>
        /// <param name="value"></param>
        public void Remove(ColumnHeader value)
        {
            List.Remove(value);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (value is ColumnHeader)
            {
                ColumnHeader header = (ColumnHeader)value;
                if (header.SortDirection != eSortDirection.None) IsSorted = false;
                header.HeaderSizeChanged -= new EventHandler(this.HeaderSizeChanged);
                header.SortCells -= new SortCellsEventHandler(SortCells);
                header.MouseDown -= new System.Windows.Forms.MouseEventHandler(ColumnMouseDown);
                header.MouseUp -= new System.Windows.Forms.MouseEventHandler(ColumnMouseUp);
                header.Parent = null;
            }
            InvalidateDisplayIndexes();
            UpdateTreeLayout();
            base.OnRemoveComplete(index, value);
        }
        protected override void OnInsertComplete(int index, object value)
        {
            if (value is ColumnHeader)
            {
                ((ColumnHeader)value).HeaderSizeChanged += new EventHandler(this.HeaderSizeChanged);
                ((ColumnHeader)value).SortCells += new SortCellsEventHandler(SortCells);
                ((ColumnHeader)value).MouseDown += new System.Windows.Forms.MouseEventHandler(ColumnMouseDown);
                ((ColumnHeader)value).MouseUp += new System.Windows.Forms.MouseEventHandler(ColumnMouseUp);
                ((ColumnHeader)value).Parent = this;
            }
            InvalidateDisplayIndexes();
            UpdateTreeLayout();
            base.OnInsertComplete(index, value);
        }

        [System.Reflection.Obfuscation(Exclude = true)]
        private void ColumnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            AdvTree tree = GetTree();
            if (tree != null)
                tree.InvokeColumnHeaderMouseUp(sender, e);
        }
        [System.Reflection.Obfuscation(Exclude = true)]
        private void ColumnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            AdvTree tree = GetTree();
            if (tree != null)
                tree.InvokeColumnHeaderMouseDown(sender, e);
        }

        private AdvTree GetTree()
        {
            AdvTree tree = _Parent;
            if (tree == null && _ParentNode != null)
                tree = _ParentNode.TreeControl;
            return tree;
        }

        private void SortCells(object sender, SortEventArgs e)
        {
            ColumnHeader ch = (ColumnHeader)sender;
            int i = this.IndexOf(ch);

            IComparer comparer = null;
            if (e.ReverseSort)
            {
                if (ch.SortComparerReverse != null)
                    comparer = ch.SortComparerReverse;
                else
                    comparer = new NodeComparerReverse(i);
            }
            else
            {
                if (ch.SortComparer != null)
                    comparer = ch.SortComparer;
                else
                    comparer = new NodeComparer(i);
            }

            if (_Parent != null)
            {
                _Parent.Nodes.Sort(comparer);
            }
            else if (_ParentNode != null)
                _ParentNode.Nodes.Sort(comparer);
        }
        private bool _UpdatingSortDirection = false;
        /// <summary>
        /// Called when SortDirection property on column header is set to value other than None.
        /// </summary>
        /// <param name="header">Ref to column header</param>
        internal void SortDirectionUpdated(ColumnHeader header)
        {
            if (_UpdatingSortDirection) return;

            _UpdatingSortDirection = true;
            try
            {
                if (header.SortDirection == eSortDirection.None)
                {
                    IsSorted = false;
                    return;
                }
                
                IsSorted = true;

                foreach (ColumnHeader col in this.List)
                {
                    if (col != header && col.SortDirection != eSortDirection.None)
                        col.SortDirection = eSortDirection.None;
                }
            }
            finally
            {
                _UpdatingSortDirection = false;
            }
        }

        private bool _IsSorted;
        /// <summary>
        /// Gets whether a column that is part of this collection has SortDirection set.
        /// </summary>
        public bool IsSorted
        {
            get { return _IsSorted; }
            internal set { _IsSorted = value; }
        }

        internal void UpdateSort()
        {
            if (!_IsSorted) return;
            foreach (ColumnHeader header in this.List)
            {
                if (header.SortDirection != eSortDirection.None)
                {
                    header.Sort(header.SortDirection == eSortDirection.Descending);
                    break;
                }
            }
        }

        private void UpdateTreeLayout()
        {
            if (_Parent != null)
            {
                _Parent.InvalidateNodesSize();
                _Parent.Invalidate();
                if (_Parent.Nodes.Count == 0)
                    _Parent.RecalcLayout();
                else
                    _Parent.SetPendingLayout();
            }
            else if (_ParentNode != null)
            {
                AdvTree tree = _ParentNode.TreeControl;
                if (tree != null)
                {
                    tree.InvalidateNodeSize(_ParentNode);
                    tree.Invalidate();
                }
            }
        }

        private void HeaderSizeChanged(object sender, EventArgs e)
        {
            UpdateTreeLayout();
        }

        /// <summary>
        /// Copies collection into the specified array.
        /// </summary>
        /// <param name="array">Array to copy collection to.</param>
        /// <param name="index">Starting index.</param>
        public void CopyTo(ColumnHeader[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Copies contained items to the ColumnHeader array.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        internal void CopyTo(ColumnHeader[] array)
        {
            List.CopyTo(array, 0);
        }

        protected override void OnClear()
        {
            foreach (ColumnHeader item in this)
            {
                item.HeaderSizeChanged -= new EventHandler(this.HeaderSizeChanged);
                item.SortCells -= new SortCellsEventHandler(SortCells);
                item.MouseDown -= new System.Windows.Forms.MouseEventHandler(ColumnMouseDown);
                item.MouseUp -= new System.Windows.Forms.MouseEventHandler(ColumnMouseUp);
                item.Parent = null;
            }
            IsSorted = false;
            base.OnClear();
        }

        /// <summary>
        ///     A map of display index (key) to index in the column collection (value).  Used to quickly find a column from its display index.
        /// </summary>
        internal List<int> DisplayIndexMap
        {
            get
            {
                if (!_IsDisplayIndexValid)
                {
                    UpdateDisplayIndexMap();
                }


                return _DisplayIndexMap;
            }
        }

        /// <summary>
        /// Gets the display index for specified column.
        /// </summary>
        /// <param name="column">Column that is part f ColumnHeaderCollection</param>
        /// <returns>Display index or -1 column is not part of this collection.</returns>
        public int GetDisplayIndex(ColumnHeader column)
        {
            int index = this.IndexOf(column);
            UpdateDisplayIndexMap();
            for (int i = 0; i < _DisplayIndexMap.Count; i++)
            {
                if (_DisplayIndexMap[i] == index) return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns the column that is displayed at specified display index..
        /// </summary>
        /// <param name="displayIndex">0 based display index.</param>
        /// <returns>ColumnHeader</returns>
        public ColumnHeader ColumnAtDisplayIndex(int displayIndex)
        {
            UpdateDisplayIndexMap();
            return this[_DisplayIndexMap[displayIndex]];
        }

        private List<int> _DisplayIndexMap = new List<int>();
        private bool _IsDisplayIndexValid = false;
        private void UpdateDisplayIndexMap()
        {
            if (_IsDisplayIndexValid) return;

            _IsDisplayIndexValid = true;
            _DisplayIndexMap.Clear();
            List<IndexToDisplayIndex> workingMap = new List<IndexToDisplayIndex>();
            bool isAllDefault = true;
            for (int i = 0; i < Count; i++)
            {
                int displayIndex = this[i].DisplayIndex;
                if (displayIndex != -1) isAllDefault = false;
                workingMap.Add(new IndexToDisplayIndex(i, displayIndex));
            }
            if (!isAllDefault)
                workingMap.Sort(new DisplayIndexComparer());
            foreach (IndexToDisplayIndex item in workingMap)
            {
                _DisplayIndexMap.Add(item.Index);
            }
        }
        /// <summary>
        /// Gets reference to last visible column or null if there is no last visible column.
        /// </summary>
        public DevComponents.AdvTree.ColumnHeader LastVisibleColumn
        {
            get
            {
                List<int> displayMap = DisplayIndexMap;
                for (int i = displayMap.Count - 1; i >=0; i--)
                {
                    if (this[displayMap[i]].Visible) return this[displayMap[i]];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets reference to first visible column or null if there is no first visible column.
        /// </summary>
        public DevComponents.AdvTree.ColumnHeader FirstVisibleColumn
        {
            get
            {
                List<int> displayMap = DisplayIndexMap;
                for (int i = 0; i < displayMap.Count; i++)
                {
                    if (this[displayMap[i]].Visible) return this[displayMap[i]];
                }
                return null;
            }
        }

        #region IndexToDisplayIndex Class
        private class IndexToDisplayIndex
        {
            public int Index = -1;
            public int DisplayIndex = -1;
            /// <summary>
            /// Initializes a new instance of the IndexToDisplayIndex class.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="displayIndex"></param>
            public IndexToDisplayIndex(int index, int displayIndex)
            {
                Index = index;
                DisplayIndex = displayIndex;
            }
        }
        #endregion

        #region DisplayIndexComparer

        private class DisplayIndexComparer : IComparer<IndexToDisplayIndex>
        {
            #region IComparer<IndexToDisplayIndex> Members

            public int Compare(IndexToDisplayIndex x, IndexToDisplayIndex y)
            {
                if (x.DisplayIndex == y.DisplayIndex)
                {
                    return x.Index - y.Index;
                }
                else
                {
                    return x.DisplayIndex - y.DisplayIndex;
                }
            }
            #endregion
        }

        #endregion

        internal void DisplayIndexChanged(ColumnHeader column, int newDisplayIndex, int oldDisplayIndex)
        {
            InvalidateDisplayIndexes();

            if (_Parent != null && !_Parent.IsUpdateSuspended)
                UpdateTreeLayout();
            else if (_ParentNode != null && _ParentNode.TreeControl != null && !_ParentNode.TreeControl.IsUpdateSuspended)
                UpdateTreeLayout();
        }
        /// <summary>
        /// Invalidates the display indexes and causes them to be re-evaluated on next layout.
        /// </summary>
        public void InvalidateDisplayIndexes()
        {
            _IsDisplayIndexValid = false;
        }
        #endregion

        private Rectangle _Bounds = Rectangle.Empty;
        internal void SetBounds(System.Drawing.Rectangle totalBounds)
        {
            _Bounds = totalBounds;
        }

        /// <summary>
        /// Gets the column header rendering bounds.
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds
        {
            get { return _Bounds; }
        }

        internal bool UsesRelativeSize = false;
    }
}
