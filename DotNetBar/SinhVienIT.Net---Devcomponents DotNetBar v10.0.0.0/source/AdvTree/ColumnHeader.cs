using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Collections;

namespace DevComponents.AdvTree
{
    /// <summary>Represents the node or tree ColumnHeader.</summary>
    [ToolboxItem(false), Designer("DevComponents.AdvTree.Design.ColumnHeaderDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class ColumnHeader : Component
    {
        #region Private Variables
        private string m_Text = "";
        private ColumnWidth m_Width = null;
        private bool m_Sortable = true;
        private eStyleTextAlignment m_TextAlign = eStyleTextAlignment.Near;
        private string m_StyleNormal = "";
        private string m_StyleMouseDown = "";
        private string m_StyleMouseOver = "";
        private string m_ColumnName = "";
        private bool m_Visible = true;
        private Rectangle m_Bounds = Rectangle.Empty;
        private bool m_SizeChanged = true;
        private string m_Name = "";
        /// <summary>
        /// Occurs when header size has changed due to the user resizing the column.
        /// </summary>
        public event EventHandler HeaderSizeChanged;
        internal event SortCellsEventHandler SortCells;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when mouse button is pressed over the column header.
        /// </summary>
        public event MouseEventHandler MouseDown;
        /// <summary>
        /// Occurs when mouse button is released over the column header.
        /// </summary>
        public event MouseEventHandler MouseUp;
        /// <summary>
        /// Occurs when header is double clicked.
        /// </summary>
        public event EventHandler DoubleClick;
        /// <summary>
        /// Occurs when header is clicked.
        /// </summary>
        public event EventHandler Click;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public ColumnHeader()
            : this("")
        {

        }

        /// <summary>
        /// Creates new instance of the object and initializes it with text.
        /// </summary>
        /// <param name="text">Text to initialize object with.</param>
        public ColumnHeader(string text)
        {
            m_Text = text;
            m_Width = new ColumnWidth();
            m_Width.WidthChanged += new EventHandler(this.WidthChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages)
            {
                BarUtilities.DisposeImage(ref _Image);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Makes a copy of ColumnHeader object.
        /// </summary>
        /// <returns>Returns new instance of column header object.</returns>
        public virtual ColumnHeader Copy()
        {
            ColumnHeader c = new ColumnHeader();
            c.ColumnName = this.ColumnName;
            c.StyleMouseDown = this.StyleMouseDown;
            c.StyleMouseOver = this.StyleMouseOver;
            c.StyleNormal = this.StyleNormal;
            c.Text = this.Text;
            c.Visible = this.Visible;
            c.Width.Absolute = this.Width.Absolute;
            c.Width.Relative = this.Width.Relative;

            return c;
        }
        #endregion

        #region Properties
        private ColumnHeaderCollection _Parent;
        internal ColumnHeaderCollection Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private bool _Editable = true;
        /// <summary>
        /// Gets or sets whether cells content in this column is editable when cell editing is enabled on tree control. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether cells content in this column is editable when cell editing is enabled on tree control.")]
        public bool Editable
        {
            get { return _Editable; }
            set
            {
                _Editable = value;
            }
        }

        private int _MaxInputLength = 0;
        /// <summary>
        /// Gets or sets the maximum number of characters the user can type or paste when editing cells in this column.
        /// </summary>
        [DefaultValue(0), Category("Behavior"), Description("Indicates maximum number of characters the user can type or paste when editing cells in this column.")]
        public int MaxInputLength
        {
            get { return _MaxInputLength; }
            set
            {
                _MaxInputLength = value;
            }
        }

        /// <summary>
        /// Returns name of the column header that can be used to identify it from the code.
        /// </summary>
        [Browsable(false), Category("Design"), Description("Indicates the name used to identify column header.")]
        public string Name
        {
            get
            {
                if (this.Site != null)
                    m_Name = this.Site.Name;
                return m_Name;
            }
            set
            {
                if (this.Site != null)
                    this.Site.Name = value;
                if (value == null)
                    m_Name = "";
                else
                    m_Name = value;
            }
        }
        /// <summary>
        /// Returns rectangle that this column occupies. If the layout has not been performed on the column the return value will be Rectangle.Empty.
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds
        {
            get { return m_Bounds; }
        }
        /// <summary>
        /// Sets the column bounds.
        /// </summary>
        internal void SetBounds(Rectangle bounds)
        {
            m_Bounds = bounds;
        }

        /// <summary>
        /// Gets the reference to the object that represents width of the column as either
        /// absolute or relative value.
        /// </summary>
        /// <remarks>
        /// Set Width using Absolute or Relative properties of ColumnWidth object.
        /// </remarks>
        /// <seealso cref="ColumnWidth.Absolute">Absolute Property (DevComponents.AdvTree.ColumnWidth)</seealso>
        /// <seealso cref="ColumnWidth.Relative">Relative Property (DevComponents.AdvTree.ColumnWidth)</seealso>
        [Browsable(true), Category("Layout"), Description("Gets or sets the width of the column."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColumnWidth Width
        {
            // TODO: Add Proper TypeConverter for ColumnWidth object and test design-time support
            get { return m_Width; }
        }

        private int _MinimumWidth = 0;
        /// <summary>
        /// Gets or sets the minimum column width in pixels that is enforced when user is resizing the columns using mouse. 
        /// Default value is 0 which indicates that there is no minimum size constraint.
        /// </summary>
        [DefaultValue(0), Category("Layout"), Description("Indicates minimum column width in pixels that is enforced when user is resizing the columns using mouse")]
        public int MinimumWidth
        {
            get { return _MinimumWidth; }
            set
            {
                if (value < 0) value = 0;
                _MinimumWidth = value;
            }
        }

        private bool _StretchToFill = false;
        /// <summary>
        /// Gets or sets whether column is stretched to fill any empty space horizontally in tree when all columns consume less width than available.
        /// Only one column in tree may have this property set to true and only last column with this property set will be stretched.
        /// You should always set the Width for the column since Width will be used when columns consume more space in tree horizontally than available.
        /// Applies to top-level columns only.
        /// </summary>
        [DefaultValue(false), Category("Layout"), Description("Indicates whether column is stretched to fill any empty space horizontally in tree when all columns consume less width than available. Only one column in tree may have this property set to true and only last column with this property set will be stretched. You should always set the Width for the column since Width will be used when columns consume more space in tree horizontally than available. Applies to top-level columns only.")]
        public bool StretchToFill
        {
            get { return _StretchToFill; }
            set 
            {
                if (_StretchToFill != value)
                {
                    _StretchToFill = value;
                    OnSizeChanged();
                }
            }
        }


        /// <summary>
        /// Gets or sets the style class assigned to the column. Empty value indicates that
        /// default style is used as specified on cell's parent's control.
        /// </summary>
        /// <value>
        /// Name of the style assigned to the cell or an empty string indicating that default
        /// style setting from tree control is applied. Default is empty string.
        /// </value>
        /// <remarks>
        /// When property is set to an empty string the style setting from parent tree
        /// controls is used. ColumnStyleNormal on AdvTree control is a root style for a cell.
        /// </remarks>
        /// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
        /// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
        [Browsable(true), DefaultValue(""), Category("Style"), Description("Indicates the style class assigned to the column.")]
        public string StyleNormal
        {
            get { return m_StyleNormal; }
            set
            {
                if (value == null) value = "";
                m_StyleNormal = value;
                this.OnSizeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the style class assigned to the column which is applied when mouse
        /// button is pressed over the header. Empty value indicates that default
        /// style is used as specified on column's parent.
        /// </summary>
        /// <value>
        /// Name of the style assigned to the column or an empty string indicating that default
        /// style setting from tree control is applied. Default is empty string.
        /// </value>
        /// <remarks>
        /// When property is set to an empty string the style setting from parent tree
        /// controls is used. ColumnStyleMouseDown on AdvTree control is a root style for a
        /// cell.
        /// </remarks>
        /// <seealso cref="StyleNormal">StyleNormal Property</seealso>
        /// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
        [Browsable(true), DefaultValue(""), Category("Style"), Description("Indicates the style class assigned to the column when mouse is down.")]
        public string StyleMouseDown
        {
            get { return m_StyleMouseDown; }
            set
            {
                if (value == null) value = "";
                m_StyleMouseDown = value;
                this.OnSizeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the style class assigned to the column which is applied when mouse is
        /// over the column. Empty value indicates that default style is used as specified on column's
        /// parent control.
        /// </summary>
        /// <value>
        /// Name of the style assigned to the column or an empty string indicating that default
        /// style setting from tree control is applied. Default is empty string.
        /// </value>
        /// <remarks>
        /// When property is set to an empty string the style setting from parent tree
        /// controls is used. ColumnStyleMouseOver on AdvTree control is a root style for a
        /// cell.
        /// </remarks>
        /// <seealso cref="StyleNormal">StyleNormal Property</seealso>
        /// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
        [Browsable(true), DefaultValue(""), Category("Style"), Description("Indicates the style class assigned to the cell when mouse is over the column.")]
        public string StyleMouseOver
        {
            get { return m_StyleMouseOver; }
            set
            {
                if (value == null) value = "";
                m_StyleMouseOver = value;
                this.OnSizeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name of the column in the ColumnHeaderCollection.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Data"), Description("Indicates the name of the column in the ColumnHeaderCollection.")]
        public string ColumnName
        {
            get { return m_ColumnName; }
            set
            {
                m_ColumnName = value;
            }
        }

        /// <summary>
        /// Gets or sets the column caption.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Appearance"), Description("Indicates column caption."), Localizable(true)]
        public string Text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
            }
        }

        /// <summary>
        /// Gets or sets whether column is visible. Hiding the header column will also hide corresponding data column.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether column is visible.")]
        public bool Visible
        {
            get { return m_Visible; }
            set
            {
                if (m_Visible != value)
                {
                    m_Visible = value;
                    OnSizeChanged();
                }
            }
        }

        private Image _Image = null;
        /// <summary>
        /// Gets or sets column image.
        /// </summary>
        /// <remarks>
        /// 	<para>The image set through this property will be serialized with the column.
        /// 	<para>
        /// 		<para>If you plan to use alpha-blended images we recommend using PNG-24 format
        ///         which supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1
        ///         do not support alpha-blending when used through Image class.</para>
        /// 	</para>
        /// </remarks>
        /// <value>Image object or <strong>null (Nothing)</strong> if no image is assigned.</value>
        [Browsable(true), DefaultValue(null), Category("Images"), Description("Indicates column image"), DevCoSerialize()]
        public System.Drawing.Image Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                this.OnImageChanged();
            }
        }

        private void OnImageChanged()
        {
            OnSizeChanged();
        }
        /// <summary>
        /// Resets Image property to it's default value (null, VB nothing).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetImage()
        {
            TypeDescriptor.GetProperties(this)["Image"].SetValue(this, null);
        }

        private eColumnImageAlignment _ImageAlignment = eColumnImageAlignment.Left;
        /// <summary>
        /// Gets or sets Image alignment inside of column. Default value is Left.
        /// </summary>
        [DefaultValue(eColumnImageAlignment.Left), Category("Images"), Description("Indicates image alignment."), DevCoSerialize()]
        public eColumnImageAlignment ImageAlignment
        {
            get { return _ImageAlignment; }
            set { _ImageAlignment = value; OnSizeChanged(); }
        }

        private string _DataFieldName = "";
        /// <summary>
        /// Gets or sets the data-field or property name that is used as source of data for this column when data-binding is used.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates data-field or property name that is used as source of data for this column when data-binding is used.")]
        public string DataFieldName
        {
            get { return _DataFieldName; }
            set
            {
                if (value == null) value = "";
                if (_DataFieldName != value)
                {
                    _DataFieldName = value;
                    OnDataFieldNameChanged();
                }
            }
        }

        /// <summary>
        /// Called when DataFieldName property has changed.
        /// </summary>
        protected virtual void OnDataFieldNameChanged()
        {

        }

        private object _Tag = null;
        /// <summary>
        /// Gets or sets additional custom data associated with the column.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        private Color _CellsBackColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the cells background for this column.
        /// </summary>
        [Category("Columns"), Description("Indicates color of cells background for this column.")]
        public Color CellsBackColor
        {
            get { return _CellsBackColor; }
            set
            {
                if (_CellsBackColor != value)
                {
                    _CellsBackColor = value;
                    if (_Parent != null && _Parent.Parent != null)
                        _Parent.Parent.Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCellsBackColor()
        {
            return !_CellsBackColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCellsBackColor()
        {
            this.CellsBackColor = Color.Empty;
        }

        private int _DisplayIndex = -1;
        /// <summary>
        /// Gets or sets display index of the column. -1 indicates default value and is modified to actual display index when the column is added to a ColumnHeaderCollection.
        /// </summary>
        /// <remarks>
        ///     A lower display index means a column will appear first (to the left) of columns with a higher display index. 
        ///     Allowable values are from 0 to num columns - 1. (-1 is legal only as the default value and is modified to something else
        ///     when the column is added to a AdvTree's column collection). AdvTree enforces that no two columns have the same display index;
        ///     changing the display index of a column will cause the index of other columns to adjust as well.
        /// </remarks>
        [DefaultValue(-1), Category("Appearance"), Description("Indicates display index of the column. -1 indicates default value and is modified to actual display index when the column is added to a ColumnHeaderCollection.")]
        public int DisplayIndex
        {
            get { return _DisplayIndex; }
            set
            {
                if (value != _DisplayIndex)
                {
                    int oldValue = _DisplayIndex;
                    _DisplayIndex = value;
                    OnDisplayIndexChanged(oldValue, value);
                }
            }
        }

        private void OnDisplayIndexChanged(int oldValue, int newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("DisplayIndex"));
            if (_Parent != null) _Parent.DisplayIndexChanged(this, newValue, oldValue);
        }

        private bool _SortingEnabled = true;
        /// <summary>
        /// Gets or sets whether user can sort by this column by clicking it.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether user can sort by this column by clicking it.")]
        public bool SortingEnabled
        {
            get { return _SortingEnabled; }
            set
            {
                if (value != _SortingEnabled)
                {
                    bool oldValue = _SortingEnabled;
                    _SortingEnabled = value;
                    OnSortingEnabledChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when SortingEnabled property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnSortingEnabledChanged(bool oldValue, bool newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("SortingEnabled"));
        }

        private eSortDirection _SortDirection = eSortDirection.None;
        /// <summary>
        /// Gets or sets the sort direction. Sort direction can be changed by clicking the column header if SortingEnabled=true.
        /// </summary>
        [DefaultValue(eSortDirection.None), Category("Behavior"), Description("Indicates sort direction. Sort direction can be changed by clicking the column header if SortingEnabled=true.")]
        public eSortDirection SortDirection
        {
            get { return _SortDirection; }
            set
            {
                if (value != _SortDirection)
                {
                    eSortDirection oldValue = _SortDirection;
                    _SortDirection = value;
                    OnSortDirectionChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when SortDirection property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnSortDirectionChanged(eSortDirection oldValue, eSortDirection newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("SortDirection"));
            this.Invalidate();
            if (_Parent != null) _Parent.SortDirectionUpdated(this);
            if (newValue == eSortDirection.Ascending)
                this.Sort(false);
            else if (newValue == eSortDirection.Descending)
                this.Sort(true);
        }
        /// <summary>
        /// Invalidates the appearance of column header.
        /// </summary>
        private void Invalidate()
        {
            if (_Parent == null) return;
            if (_Parent.Parent != null)
            {
                if (_Parent.Parent.ColumnHeaderControl != null)
                    _Parent.Parent.ColumnHeaderControl.Invalidate(this.Bounds);
            }
            else if (_Parent.ParentNode != null)
                _Parent.ParentNode.Invalidate();
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Automatically sets the column width (Width.Absolute) property based on the content of the column.
        /// This will perform the one-time auto sizing of the column. To make column auto-size all the time
        /// set Width.AutoSize=true.
        /// </summary>
        public void AutoSize()
        {
            if (_Parent == null)
                throw new InvalidOperationException("ColumnHeader is not parented.");
            AdvTree tree = AdvTree;
            if (tree == null)
                throw new InvalidOperationException("Cannot obtain reference to parent tree control, column might not be parented.");
            this.Width.AutoSize = true;
            tree.RecalcLayout();
            tree.Invalidate();
            this.Width.SetAutoSize(false);
            this.Width.SetAbsolute(this.Width.AutoSizeWidth);
        }
        /// <summary>
        /// Returns reference to AdvTree control this column belongs to.
        /// </summary>
        [Browsable(false)]
        public AdvTree AdvTree
        {
            get
            {
                if (_Parent == null) return null;
                return _Parent.ParentNode == null ? _Parent.Parent : _Parent.ParentNode.TreeControl;
            }
        }

        private bool _DoubleClickAutoSize = true;
        /// <summary>
        /// Gets or sets whether column is automatically sized to the content when user double-clicks the column 
        /// on the column resize line. Column resizing must be enabled in order for this property to function.
        /// Default value is true which indicates that column will be auto-sized to content when user double-clicks the
        /// column resize marker.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether column is automatically sized to content when user double-clicks the column resize marker.")]
        public bool DoubleClickAutoSize
        {
            get { return _DoubleClickAutoSize; }
            set
            {
                _DoubleClickAutoSize = value;
            }
        }


        /// <summary>
        /// Gets or sets whether column size has changed and it's layout needs to be recalculated.
        /// </summary>
        internal bool SizeChanged
        {
            get { return m_SizeChanged; }
            set { m_SizeChanged = value; }
        }

        private void OnSizeChanged()
        {
            m_SizeChanged = true;
            if (HeaderSizeChanged != null)
                HeaderSizeChanged(this, new EventArgs());
        }

        private void WidthChanged(object sender, EventArgs e)
        {
            this.OnSizeChanged();
        }

        private bool _IsMouseDown = false;
        /// <summary>
        /// Gets whether mouse left button is pressed on the column.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseDown
        {
            get { return _IsMouseDown; }
#if FRAMEWORK20
            internal set
#else
			set
#endif

            {
                if (_IsMouseDown != value)
                {
                    _IsMouseDown = value;
                    OnIsMouseDownChanged();
                }
            }
        }

        private void OnIsMouseDownChanged()
        {
            if (_Parent != null && _Parent.ParentNode != null)
            {
                AdvTree tree = _Parent.ParentNode.TreeControl;
                if (tree != null) tree.Invalidate(this.Bounds);
            }
        }

        private bool _IsMouseOver = false;
        /// <summary>
        /// Gets whether mouse is over the column.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseOver
        {
            get { return _IsMouseOver; }
#if FRAMEWORK20
            internal set
#else
			set
#endif
            {
                _IsMouseOver = value;
            }
        }

        internal bool IsLastVisible = false;
        internal bool IsFirstVisible = false;

        internal virtual void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) IsMouseDown = true;

            MouseEventHandler eh = this.MouseDown;
            if (eh != null)
                eh(this, e);
        }

        internal virtual void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) IsMouseDown = false;

            if (_SortingEnabled)
            {
                if (e.Button == MouseButtons.Middle && _SortDirection != eSortDirection.None)
                    SortDirection = eSortDirection.None;
                else if (e.Button == MouseButtons.Left)
                {
                    if (_SortDirection == eSortDirection.None || _SortDirection == eSortDirection.Descending)
                        SortDirection = eSortDirection.Ascending;
                    else
                        SortDirection = eSortDirection.Descending;
                }
            }

            MouseEventHandler eh = this.MouseUp;
            if (eh != null)
                eh(this, e);
        }

        internal virtual void OnDoubleClick(EventArgs e)
        {
            EventHandler handler = DoubleClick;
            if (handler != null) handler(this, e);
        }

        internal virtual void OnClick(EventArgs e)
        {
            EventHandler handler = Click;
            if (handler != null) handler(this, e);
        }

        private eCellEditorType _EditorType = eCellEditorType.Default;
        /// <summary>
        /// Gets or sets the editor type used to edit the cell. Setting this property to value other than Default
        /// overrides the cell editor type specified on column cell belongs to.
        /// </summary>
        [DefaultValue(eCellEditorType.Default), Category("Behavior"), Description("Indicates editor type used to edit the cell.")]
        public eCellEditorType EditorType
        {
            get { return _EditorType; }
            set { _EditorType = value; }
        }

        private bool _LastSortReverse = false;
        /// <summary>
        /// Sort first level nodes that belong directly to this column. Calling this method repeatedly will
        /// alternate between A-Z and Z-A sorting.
        /// </summary>
        public void Sort()
        {
            Sort(_LastSortReverse);
            _LastSortReverse = !_LastSortReverse;
        }

        /// <summary>
        /// Sort first level nodes that belong directly to this column.
        /// </summary>
        /// <param name="reverse">true to use reverse Z-A sorting, false to sort from A-Z</param>
        public void Sort(bool reverse)
        {
            if (SortCells != null)
                SortCells(this, new SortEventArgs(reverse));
        }

        private IComparer _SortComparer = null;
        /// <summary>
        /// Gets or sets ascending (A-Z) column comparer used to sort nodes when this column is clicked. Your comparer will be passed to NodeCollection.Sort method and should know how to sort by appropriate column.
        /// </summary>
        [Browsable(false), DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IComparer SortComparer
        {
            get { return _SortComparer; }
            set
            {
                if (value != _SortComparer)
                {
                    IComparer oldValue = _SortComparer;
                    _SortComparer = value;
                    OnSortComparerChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when SortComparer property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnSortComparerChanged(IComparer oldValue, IComparer newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("SortComparer"));
        }

        private IComparer _SortComparerReverse = null;
        /// <summary>
        /// Gets or sets descending (Z-A) column comparer used to sort nodes when this column is clicked. Your comparer will be passed to NodeCollection.Sort method and should know how to sort by appropriate column.
        /// </summary>
        [Browsable(false), DefaultValue(null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IComparer SortComparerReverse
        {
            get { return _SortComparerReverse; }
            set
            {
                if (value != _SortComparerReverse)
                {
                    IComparer oldValue = _SortComparerReverse;
                    _SortComparerReverse = value;
                    OnSortComparerReverseChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when SortComparerReverse property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnSortComparerReverseChanged(IComparer oldValue, IComparer newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("SortComparerReverse"));
        }
        #endregion

    }

    internal delegate void SortCellsEventHandler(object sender, SortEventArgs e);
    internal class SortEventArgs : EventArgs
    {
        public bool ReverseSort = false;

        public SortEventArgs(bool reverse)
        {
            ReverseSort = reverse;
        }
    }
    /// <summary>
    /// Defines column related event arguments.
    /// </summary>
    public class ColumnEventArgs : EventArgs
    {
        /// <summary>
        /// Gets reference to the column.
        /// </summary>
        public readonly ColumnHeader Column;
        /// <summary>
        /// Initializes a new instance of the ColumnEventArgs class.
        /// </summary>
        /// <param name="column"></param>
        public ColumnEventArgs(ColumnHeader column)
        {
            Column = column;
        }
    }

    /// <summary>
    /// Defines delegate for ColumnMoved event.
    /// </summary>
    public delegate void ColumnMovedHandler(object sender, ColumnMovedEventArgs ea);
    /// <summary>
    /// Defines column moved event arguments.
    /// </summary>
    public class ColumnMovedEventArgs : ColumnEventArgs
    {
        /// <summary>
        /// Gets the column display index before the column was moved.
        /// </summary>
        public readonly int OldColumnDisplayIndex;
        /// <summary>
        /// Gets the column display index before the column was moved.
        /// </summary>
        public readonly int NewColumnDisplayIndex;
        /// <summary>
        /// Initializes a new instance of the ColumnMovedEventArgs class.
        /// </summary>
        /// <param name="column">Column affected</param>
        /// <param name="oldColumnDisplayIndex">Old display index</param>
        /// <param name="newColumnDisplayIndex">New display index</param>
        public ColumnMovedEventArgs(ColumnHeader column, int oldColumnDisplayIndex, int newColumnDisplayIndex)
            : base(column)
        {
            OldColumnDisplayIndex = oldColumnDisplayIndex;
            NewColumnDisplayIndex = newColumnDisplayIndex;
        }
    }
}
