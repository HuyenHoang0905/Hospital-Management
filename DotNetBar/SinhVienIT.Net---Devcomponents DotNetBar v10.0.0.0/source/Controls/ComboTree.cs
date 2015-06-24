#if FRAMEWORK20
using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.Editors;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Drawing.Design;
using DevComponents.AdvTree;
using System.Security;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the combo box like control which shows the AdvTree control on popup. Tree control
    /// can be configured to display multiple columns as well.
    /// </summary>
    [ToolboxBitmap(typeof(ComboTree), "Controls.ComboTree.ico"), ToolboxItem(true), DefaultProperty("Text"), DefaultBindingProperty("Text"), DefaultEvent("TextChanged"), Designer("DevComponents.DotNetBar.Design.ComboTreeDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class ComboTree : PopupItemControl, IInputButtonControl, ICommandSource
    {
        #region Private Variables
        //private TextBoxX _TextBox = null;
        private AdvTree.AdvTree _AdvTree = null;
        private ButtonItem _PopupItem = null;
        private static string _DropDownItemContainerName = "sysPopupItemContainer";
        private static string _DropDownControlContainerName = "sysPopupControlContainer";
        private Color _FocusHighlightColor = ColorScheme.GetColor(0xFFFF88);
        private bool _FocusHighlightEnabled = false;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.
        /// </summary>
        public event CancelEventHandler ButtonClearClick;
        /// <summary>
        /// Occurs when Drop-Down button that shows popup is clicked and allows you to cancel showing of the popup.
        /// </summary>
        public event CancelEventHandler ButtonDropDownClick;
        /// <summary>
        /// Occurs when ButtonCustom control is clicked.
        /// </summary>
        public event EventHandler ButtonCustomClick;
        /// <summary>
        /// Occurs when ButtonCustom2 control is clicked.
        /// </summary>
        public event EventHandler ButtonCustom2Click;
        /// <summary>
        /// Occurs when the text alignment in text box has changed.
        /// </summary>
        [Description("Occurs when the text alignment in text box has changed.")]
        public event EventHandler TextAlignChanged;
        ///// <summary>
        ///// Occurs when the value of the Modified property has changed.
        ///// </summary>
        //public event EventHandler ModifiedChanged;
        /// <summary>
        /// Occurs before Node has been selected by user or through the SelectedNode property. Event can be canceled.
        /// </summary>
        public event AdvTreeNodeCancelEventHandler SelectionChanging;
        /// <summary>
        /// Occurs after node has been selected by user or through the SelectedNode property.
        /// </summary>
        public event AdvTreeNodeEventHandler SelectionChanged;
        /// <summary>
        /// Occurs when the DataSource changes.
        /// </summary>
        [Description("Occurs when the DataSource changes.")]
        public event EventHandler DataSourceChanged;
        /// <summary>
        /// Occurs when the DisplayMembers property changes.
        /// </summary>
        [Description("Occurs when the DisplayMembers property changes")]
        public event EventHandler DisplayMembersChanged;
        /// <summary>
        /// Occurs when the control is bound to a data value that need to be converted.
        /// </summary>
        [Description("Occurs when the control is bound to a data value that need to be converted.")]
        public event TreeConvertEventHandler Format;
        /// <summary>
        /// Occurs when FormattingEnabled property changes.
        /// </summary>
        [Description("Occurs when FormattingEnabled property changes.")]
        public event EventHandler FormattingEnabledChanged;
        /// <summary>
        /// Occurs when FormatString property changes.
        /// </summary>
        [Description("Occurs when FormatString property changes.")]
        public event EventHandler FormatStringChanged;
        /// <summary>
        /// Occurs when FormatInfo property has changed.
        /// </summary>
        [Description("Occurs when FormatInfo property has changed.")]
        public event EventHandler FormatInfoChanged;
        /// <summary>
        /// Occurs when a Node for an data-bound object item has been created and provides you with opportunity to modify the node.
        /// </summary>
        [Description("Occurs when a Node for an data-bound object item has been created and provides you with opportunity to modify the node")]
        public event DataNodeEventHandler DataNodeCreated;
        /// <summary>
        /// Occurs when a group Node is created as result of GroupingMembers property setting and provides you with opportunity to modify the node.
        /// </summary>
        [Description("Occurs when a group Node is created as result of GroupingMembers property setting and provides you with opportunity to modify the node")]
        public event DataNodeEventHandler GroupNodeCreated;
        /// <summary>
        /// Occurs when value of ValueMember property has changed.
        /// </summary>
        [Description("Occurs when value of ValueMember property has changed.")]
        public event EventHandler ValueMemberChanged;
        /// <summary>
        /// Occurs when value of SelectedValue property has changed.
        /// </summary>
        [Description("Occurs when value of SelectedValue property has changed.")]
        public event EventHandler SelectedValueChanged;
        /// <summary>
        /// Occurs when value of SelectedIndex property has changed.
        /// </summary>
        [Description("Occurs when value of SelectedValue property has changed.")]
        public event EventHandler SelectedIndexChanged;
        /// <summary>
        /// Occurs when ColumnHeader is automatically created by control as result of data binding and provides you with opportunity to modify it.
        /// </summary>
        [Description("Occurs when ColumnHeader is automatically created by control as result of data binding and provides you with opportunity to modify it.")]
        public event DataColumnEventHandler DataColumnCreated;
        #endregion

        #region Constructor
         /// <summary>
        /// Initializes a new instance of the TextBoxDropDown class.
        /// </summary>
        public ComboTree()
        {
            this.SetStyle(ControlStyles.Selectable, true);
            InitControl();
            this.BackColor = SystemColors.Window;
        }
        private void InitControl()
        {
            _BackgroundStyle.SetColorScheme(this.ColorScheme);
            _BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);

            _ButtonCustom = new InputButtonSettings(this);
            _ButtonCustom2 = new InputButtonSettings(this);
            _ButtonClear = new InputButtonSettings(this);
            _ButtonDropDown = new InputButtonSettings(this);
            CreateButtonGroup();

            //_TextBox = new TextBoxX();
            //_TextBox.BorderStyle = BorderStyle.None;
            //_TextBox.TextChanged += new EventHandler(TextBoxTextChanged);
            //_TextBox.TextAlignChanged += new EventHandler(TextBoxTextAlignChanged);
            //_TextBox.SizeChanged += new EventHandler(TextBoxSizeChanged);
            //_TextBox.Visible=false;
            ////_TextBox.ModifiedChanged += new EventHandler(TextBoxModifiedChanged);
            //this.Controls.Add(_TextBox);

            // Init popup tree
            _AdvTree = new DevComponents.AdvTree.AdvTree();
#if (!TRIAL)
            _AdvTree.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
            _AdvTree.BackgroundStyle.Reset();
            _AdvTree.BackgroundStyle.BackColor = SystemColors.Window;
            _AdvTree.BackgroundStyle.Border = eStyleBorderType.None;
            _AdvTree.ExpandButtonType = eExpandButtonType.Triangle;
            _AdvTree.DragDropEnabled = false;
            _AdvTree.HotTracking = true;
            _AdvTree.Indent = 6;
            _AdvTree.SelectionFocusAware = false;
            ElementStyle nodeStyle = new ElementStyle();
            nodeStyle.Name = "nodeElementStyle";
            nodeStyle.TextColor = System.Drawing.SystemColors.ControlText;
            _AdvTree.NodeStyle = nodeStyle;
            _AdvTree.PathSeparator = ";";
            _AdvTree.Styles.Add(nodeStyle);
            _AdvTree.AfterNodeSelect += new AdvTreeNodeEventHandler(TreeAfterNodeSelect);
            _AdvTree.BeforeNodeSelect += new AdvTreeNodeCancelEventHandler(TreeBeforeNodeSelect);
            _AdvTree.NodeMouseDown += new TreeNodeMouseEventHandler(TreeNodeMouseDown);

            this.DropDownControl = _AdvTree;
        }
        #endregion

        #region Internal Implementation
        protected override void InvalidateAutoSize()
        {
            if (_AdvTree != null)
                _AdvTree.AntiAlias = this.AntiAlias;
        }
        /// <summary>
        /// Gets or sets the tree node that is currently selected in the tree control.
        /// </summary>
        /// <remarks>
        /// 	<para>If no <see cref="Node">Node</see> is currently selected, the
        ///     <b>SelectedNode</b> property is a null reference (<b>Nothing</b> in Visual
        ///     Basic).</para>
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Node SelectedNode
        {
            get
            {
                return _AdvTree.SelectedNode;
            }
            set
            {
                _AdvTree.SelectedNode = value;
            }
        }

        void TreeNodeMouseDown(object sender, TreeNodeMouseEventArgs e)
        {
            if (this.SelectionClosesPopup && e.Button == MouseButtons.Left && e.Node != null && 
                e.Node.IsSelected && this.IsPopupOpen && !e.Node.ExpandPartRectangle.Contains(e.X, e.Y))
            {
                this.IsPopupOpen = false;
            }
        }

        private void TreeBeforeNodeSelect(object sender, AdvTreeNodeCancelEventArgs e)
        {
            if (SelectionChanging != null)
                SelectionChanging(this, e);
        }
        private void TreeAfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);

            if (_DataManager != null)
            {
                Node selectedNode = _AdvTree.SelectedNode;
                if (selectedNode != null && selectedNode.BindingIndex > -1 && _DataManager.Position != selectedNode.BindingIndex)
                {
                    _DataManager.Position = selectedNode.BindingIndex;
                }
                else if (selectedNode == null)
                    _DataManager.Position = -1;
            }

            if (this.SelectionClosesPopup && this.IsPopupOpen && e.Action == eTreeAction.Mouse)
            {
                this.IsPopupOpen = false;
            }

            if (e.Node != null) this.Text = e.Node.ToString();
            this.Invalidate();

            OnSelectedIndexChanged(EventArgs.Empty);
            if(!string.IsNullOrEmpty(this.ValueMember))
                this.OnSelectedValueChanged(EventArgs.Empty);
        }

        private bool _SelectionClosesPopup = true;
        /// <summary>
        /// Gets or sets whether selection change on popup tree closes the popup. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether selection change on popup tree closes the popup.")]
        public bool SelectionClosesPopup
        {
            get { return _SelectionClosesPopup; }
            set
            {
                _SelectionClosesPopup = value;
            }
        }

        private string _SelectedDisplayMemeber = "";
        /// <summary>
        /// Gets or sets the field name that holds the text that will be displayed in the control for selected item. When not set all items set in DisplayMembers will be displayed in control.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates field name that holds the text that will be displayed in the control for selected item. When not set all items set in DisplayMembers will be displayed in control.")]
        public string SelectedDisplayMemeber
        {
            get { return _SelectedDisplayMemeber; }
            set
            {
                _SelectedDisplayMemeber = value;
                this.Invalidate();
            }
        }
        
        
        private List<BindingMemberInfo> _DisplayMembers = null;
        /// <summary>
        /// Gets or sets the comma separated list of property or column names to display on popup tree control.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates comma separated list of property or column names to display on popup tree control")]
        public string DisplayMembers
        {
            get
            {
                if (_DisplayMembers == null || _DisplayMembers.Count == 0)
                    return "";
                StringBuilder members = new StringBuilder();
                for (int i = 0; i < _DisplayMembers.Count; i++)
                {
                    BindingMemberInfo item = _DisplayMembers[i];
                    members.Append(item.BindingMember);
                    if (i + 1 < _DisplayMembers.Count)
                        members.Append(',');
                }
                return members.ToString();
            }
            set
            {
                List<BindingMemberInfo> displayMembers = _DisplayMembers;

                List<BindingMemberInfo> newMembers = null;

                if (!string.IsNullOrEmpty(value))
                {
                    newMembers = new List<BindingMemberInfo>();
                    // Parse the members comma separated list expected...
                    string[] members = value.Split(',');
                    for (int i = 0; i < members.Length; i++)
                    {
                        newMembers.Add(new BindingMemberInfo(members[i].Trim()));
                    }
                }

                try
                {
                    this.SetDataConnection(_DataSource, newMembers, false);
                }
                catch
                {
                    _DisplayMembers = displayMembers;
                }
            }
        }

        private object _DataSource = null;
        /// <summary>
        /// Gets or sets the data source for the ComboTree. Expected is an object that implements the IList or IListSource interfaces, 
        /// such as a DataSet or an Array. The default is null.
        /// </summary>
        [AttributeProvider(typeof(IListSource)), Description("Indicates data source for the ComboTree."), Category("Data"), DefaultValue(null), RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource
        {
            get
            {
                return _DataSource;
            }
            set
            {
                if (((value != null) && !(value is IList)) && !(value is IListSource))
                {
                    throw new ArgumentException("Data type is not supported for complex data binding");
                }
                if (_DataSource != value)
                {
                    try
                    {
                        this.SetDataConnection(value, _DisplayMembers, true);
                    }
                    catch
                    {
                        this.DisplayMembers = "";
                    }
                    if (value == null)
                    {
                        this.DisplayMembers = "";
                    }
                }
            }
        }

        private bool _FormattingEnabled = false;
        /// <summary>
        /// Gets or sets a value indicating whether formatting is applied to the DisplayMembers property of the control.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether formatting is applied to the DisplayMembers property of the control.")]
        public bool FormattingEnabled
        {
            get
            {
                return _FormattingEnabled;
            }
            set
            {
                if (value != _FormattingEnabled)
                {
                    _FormattingEnabled = value;
                    RefreshItems();
                    OnFormattingEnabledChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raises FormattingEnabledChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnFormattingEnabledChanged(EventArgs e)
        {
            EventHandler handler = FormattingEnabledChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private string _FormatString = "";
        /// <summary>
        /// Gets or sets the format-specifier characters that indicate how a value is to be displayed.
        /// </summary>
        [MergableProperty(false), Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), DefaultValue(""), Description("Indicates format-specifier characters that indicate how a value is to be displayed.")]
        public string FormatString
        {
            get
            {
                return _FormatString;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }
                if (!value.Equals(_FormatString))
                {
                    _FormatString = value;
                    RefreshItems();
                    OnFormatStringChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raises FormatStringChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnFormatStringChanged(EventArgs e)
        {
            EventHandler handler = FormatStringChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private IFormatProvider _FormatInfo = null;
        /// <summary>
        /// Gets or sets the IFormatProvider that provides custom formatting behavior. 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false), DefaultValue((string)null)]
        public IFormatProvider FormatInfo
        {
            get
            {
                return _FormatInfo;
            }
            set
            {
                if (value != _FormatInfo)
                {
                    _FormatInfo = value;
                    RefreshItems();
                    OnFormatInfoChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raises FormatInfoChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnFormatInfoChanged(EventArgs e)
        {
            EventHandler handler = FormatInfoChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        private bool _InSetDataConnection = false;
        private void SetDataConnection(object newDataSource, List<BindingMemberInfo> newDisplayMembers, bool force)
        {
            bool dataSourceChanged = _DataSource != newDataSource;
            bool displayMemberChanged = _DisplayMembers != newDisplayMembers;

            if (!_InSetDataConnection)
            {
                try
                {
                    if ((force || dataSourceChanged) || displayMemberChanged)
                    {
                        _InSetDataConnection = true;
                        IList list = (this.DataManager != null) ? this.DataManager.List : null;
                        bool isDataManagerNull = this.DataManager == null;
                        this.UnwireDataSource();
                        _DataSource = newDataSource;
                        _DisplayMembers = newDisplayMembers;
                        this.WireDataSource();
                        if (_IsDataSourceInitialized)
                        {
                            CurrencyManager manager = null;
                            if (((newDataSource != null) && (this.BindingContext != null)) && (newDataSource != Convert.DBNull))
                            {
                                string bindingPath = "";
                                if (_DisplayMembers != null && _DisplayMembers.Count > 0)
                                    bindingPath = newDisplayMembers[0].BindingPath;
                                manager = (CurrencyManager)this.BindingContext[newDataSource, bindingPath];
                            }
                            if (_DataManager != manager)
                            {
                                if (_DataManager != null)
                                {
                                    _DataManager.ItemChanged -= new ItemChangedEventHandler(this.DataManager_ItemChanged);
                                    _DataManager.PositionChanged -= new EventHandler(this.DataManager_PositionChanged);
                                }
                                _DataManager = manager;
                                if (_DataManager != null)
                                {
                                    _DataManager.ItemChanged += new ItemChangedEventHandler(this.DataManager_ItemChanged);
                                    _DataManager.PositionChanged += new EventHandler(this.DataManager_PositionChanged);
                                }
                            }
                            if (((_DataManager != null) && (displayMemberChanged || dataSourceChanged)) && !ValidateDisplayMembers(_DisplayMembers))
                            {
                                throw new ArgumentException("Wrong DisplayMembers parameter", "newDisplayMember");
                            }
                            if (((_DataManager != null) && ((dataSourceChanged || displayMemberChanged) || force)) && (displayMemberChanged || (force && ((list != _DataManager.List) || isDataManagerNull || dataSourceChanged))))
                            {
                                DataManager_ItemChanged(_DataManager, null);
                            }
                        }
                        _Converters.Clear();
                    }
                    if (dataSourceChanged)
                    {
                        this.OnDataSourceChanged(EventArgs.Empty);
                    }
                    if (displayMemberChanged)
                    {
                        this.OnDisplayMembersChanged(EventArgs.Empty);
                    }
                }
                finally
                {
                    _InSetDataConnection = false;
                }
            }
        }

        private bool ValidateDisplayMembers(List<BindingMemberInfo> members)
        {
            if (members == null || members.Count == 0) return true;

            foreach (BindingMemberInfo item in members)
            {
                if (item.BindingMember != null && !BindingMemberInfoInDataManager(item))
                    return false;
            }
            return true;
        }
        private bool BindingMemberInfoInDataManager(BindingMemberInfo bindingMemberInfo)
        {
            if (_DataManager != null)
            {
                PropertyDescriptorCollection itemProperties = _DataManager.GetItemProperties();
                int count = itemProperties.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(bindingMemberInfo.BindingField))
                    {
                        return true;
                    }
                }
                for (int j = 0; j < count; j++)
                {
                    if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && (string.Compare(itemProperties[j].Name, bindingMemberInfo.BindingField, true, CultureInfo.CurrentCulture) == 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Raises the DataSourceChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data. </param>
        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            EventHandler handler = DataSourceChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Raises the DisplayMemberChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data. </param>
        protected virtual void OnDisplayMembersChanged(EventArgs e)
        {
            EventHandler handler = DisplayMembersChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private Hashtable _Converters = new Hashtable();
        private TypeConverter GetFieldConverter(string fieldName)
        {
            if (_Converters.ContainsKey(fieldName))
                return (TypeConverter)_Converters[fieldName];
            if (this.DataManager != null)
            {
                PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
                if (itemProperties != null)
                {
                    PropertyDescriptor descriptor = itemProperties.Find(fieldName, true);
                    if (descriptor != null)
                    {
                        _Converters.Add(fieldName, descriptor.Converter);
                        return descriptor.Converter;
                    }
                }
            }

            return null;
        }

        private void DataManager_ItemChanged(object sender, ItemChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ItemChangedEventHandler(DataManager_ItemChanged), sender, e);
                return;
            }
            if (_DataManager != null)
            {
                if (e==null || e.Index == -1)
                {
                    this.SetItemsCore(_DataManager.List);
                    if (this.AllowSelection)
                    {
                        if (_DataManager.Position == -1)
                            this.SelectedNode = null;
                        else
                            this.SelectedNode = _AdvTree.FindNodeByBindingIndex(_DataManager.Position);
                    }
                }
                else
                {
                    this.SetItemCore(e.Index, _DataManager.List[e.Index]);
                }
            }
        }
        private void DataManager_PositionChanged(object sender, EventArgs e)
        {
            if ((_DataManager != null) && this.AllowSelection)
            {
                if (_DataManager.Position == -1)
                    this.SelectedNode = null;
                else
                    this.SelectedNode = _AdvTree.FindNodeByBindingIndex(_DataManager.Position);
                //this.SelectedIndex = _DataManager.Position;
            }
        }

        /// <summary>
        /// When overridden in a derived class, resynchronizes the item data with the contents of the data source.
        /// </summary>
        public virtual void RefreshItems()
        {
            if (_DataManager != null)
            {
                SetItemsCore(_DataManager.List);
                if (this.AllowSelection)
                {
                    if (_DataManager.Position == -1)
                        this.SelectedNode = null;
                    else
                        this.SelectedNode = _AdvTree.FindNodeByBindingIndex(_DataManager.Position);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the specified array of objects in a collection in the derived class.
        /// </summary>
        /// <param name="items">An array of items.</param>
        protected virtual void SetItemsCore(IList items)
        {
            if(this.DesignMode) return;
            _AdvTree.BeginUpdate();
            _AdvTree.Nodes.Clear();

            bool isGrouping = !string.IsNullOrEmpty(_GroupingMembers);

            List<string> fieldNames = new List<string>();
            // Create Columns
            if (string.IsNullOrEmpty(this.DisplayMembers))
            {
                if (_AdvTree.Columns.Count > 0)
                {
                    foreach (DevComponents.AdvTree.ColumnHeader columnHeader in _AdvTree.Columns)
                    {
                        if(!string.IsNullOrEmpty(columnHeader.DataFieldName))
                            fieldNames.Add(columnHeader.DataFieldName);
                    }
                }
                if (fieldNames.Count == 0)
                {
                    _AdvTree.Columns.Clear();
                    if (_DataManager.List is Array)
                    {
                        DevComponents.AdvTree.ColumnHeader ch = new DevComponents.AdvTree.ColumnHeader("Items");
                        ch.Width.Relative = 100;
                        _AdvTree.Columns.Add(ch);
                        OnDataColumnCreated(new DataColumnEventArgs(ch));
                    }
                    else if (_DataManager != null)
                    {
                        PropertyDescriptorCollection properties = _DataManager.GetItemProperties();
                        foreach (PropertyDescriptor prop in properties)
                        {
                            fieldNames.Add(prop.Name);
                            DevComponents.AdvTree.ColumnHeader ch = new DevComponents.AdvTree.ColumnHeader(StringHelper.GetFriendlyName(prop.Name));
                            ch.DataFieldName = prop.Name;
                            ch.Width.Relative = Math.Max(10, 100 / properties.Count);
                            _AdvTree.Columns.Add(ch);
                            OnDataColumnCreated(new DataColumnEventArgs(ch));
                        }
                    }
                }
            }
            else
            {
                _AdvTree.Columns.Clear();
                if (_DisplayMembers != null && _DisplayMembers.Count > 0)
                {
                    foreach (BindingMemberInfo item in _DisplayMembers)
                    {
                        DevComponents.AdvTree.ColumnHeader ch = new DevComponents.AdvTree.ColumnHeader(StringHelper.GetFriendlyName(item.BindingMember));
                        ch.Tag = item;
                        ch.Width.Relative = Math.Max(10, 100 / _DisplayMembers.Count);
                        _AdvTree.Columns.Add(ch);
                        fieldNames.Add(item.BindingMember);
                        OnDataColumnCreated(new DataColumnEventArgs(ch));
                    }
                }
            }

            if (string.IsNullOrEmpty(_GroupingMembers) && string.IsNullOrEmpty(_ParentFieldNames))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    object item = items[i];
                    Node node = CreateNode(_AdvTree.Nodes, item, i, fieldNames);
                }
            }
            else if (!string.IsNullOrEmpty(_ParentFieldNames))
            {
                isGrouping = true;

                Dictionary<string, Node> nodeParentCollection = new Dictionary<string, Node>();
                Dictionary<string, List<Node>> nodeNeedsParentCollection = new Dictionary<string, List<Node>>();
                string[] parentFields = _ParentFieldNames.Split(',');

                for (int i = 0; i < items.Count; i++)
                {
                    object item = items[i];
                    string nodeKey = GetItemText(item, parentFields[0]);
                    string parentKey = GetItemText(item, parentFields[1]);
                    Node parentNode = null;
                    if (nodeParentCollection.TryGetValue(parentKey, out parentNode))
                    {
                        Node node = CreateNode(parentNode.Nodes, item, i, fieldNames);
                        nodeParentCollection.Add(nodeKey, node);
                    }
                    else
                    {
                        Node node = CreateNode(this.Nodes, item, i, fieldNames);
                        List<Node> list = null;
                        if (!nodeNeedsParentCollection.TryGetValue(parentKey, out list))
                        {
                            list = new List<Node>();
                            nodeNeedsParentCollection.Add(parentKey, list);
                        }
                        list.Add(node);
                        nodeParentCollection.Add(nodeKey, node);
                    }
                }
                // If there are nodes that needed a parent process them now
                foreach (KeyValuePair<string, List<Node>> keyValue in nodeNeedsParentCollection)
                {
                    Node parentNode = null;
                    if (nodeParentCollection.TryGetValue(keyValue.Key, out parentNode))
                    {
                        foreach (Node node in keyValue.Value)
                        {
                            node.Remove();
                            parentNode.Nodes.Add(node);
                        }
                    }
                }
            }
            else
            {
                string[] groupFields = _GroupingMembers.Split(',');
                for (int i = 0; i < groupFields.Length; i++)
                {
                    groupFields[i] = groupFields[i].Trim();
                }
                Dictionary<string, Node> _GroupTable = new Dictionary<string, Node>();
                for (int i = 0; i < items.Count; i++)
                {
                    object item = items[i];
                    NodeCollection parentCollection = _AdvTree.Nodes;

                    // Find the parent collection to add item to
                    string key = "";
                    for (int gi = 0; gi < groupFields.Length; gi++)
                    {
                        string text = GetItemText(item, groupFields[gi]);
                        key += text.ToLower() + "/";
                        Node groupNode = null;
                        if (!_GroupTable.TryGetValue(key, out groupNode))
                        {
                            groupNode = CreateGroupNode(parentCollection, text);
                            _GroupTable.Add(key, groupNode);
                        }
                        parentCollection = groupNode.Nodes;
                    }

                    Node node = CreateNode(parentCollection, item, i, fieldNames);
                }
            }

            // If not grouping then remove the expand part space on left-hand side
            if (isGrouping)
            {
                _AdvTree.ExpandWidth = 14;
            }
            else
            {
                _AdvTree.ExpandWidth = 0;
            }

            _AdvTree.EndUpdate();
        }
        /// <summary>
        /// Raises the DataColumnCreated event.
        /// </summary>
        /// <param name="args">Provides event arguments.</param>
        protected virtual void OnDataColumnCreated(DataColumnEventArgs args)
        {
            if (DataColumnCreated != null)
                DataColumnCreated(this, args);
        }

        private Node CreateGroupNode(NodeCollection parentCollection, string text)
        {
            Node node = new Node();
            node.Text = text;
            node.Style = _GroupNodeStyle;
            node.Expanded = true;
            node.Selectable = false;
            parentCollection.Add(node);
            DataNodeEventArgs eventArgs = new DataNodeEventArgs(node, null);
            OnGroupNodeCreated(eventArgs);
            return node;
        }
        /// <summary>
        /// Raises the DataNodeCreated event.
        /// </summary>
        /// <param name="dataNodeEventArgs">Provides event arguments.</param>
        protected virtual void OnGroupNodeCreated(DataNodeEventArgs dataNodeEventArgs)
        {
            if (GroupNodeCreated != null) GroupNodeCreated(this, dataNodeEventArgs);
        }

        /// <summary>
        /// Creates a new node for the data item.
        /// </summary>
        /// <param name="item">Item to create node for.</param>
        /// <returns>New instance of the node.</returns>
        private Node CreateNode(NodeCollection parentCollection, object item, int itemIndex, List<string> fieldNames)
        {
            Node node = new Node();
            parentCollection.Add(node);

            SetNodeData(node, item, fieldNames, itemIndex);

            DataNodeEventArgs eventArgs = new DataNodeEventArgs(node, item);

            OnDataNodeCreated(eventArgs);

            return eventArgs.Node;
        }

        private void SetNodeData(Node node, object item, List<string> fieldNames, int bindingIndex)
        {
            node.DataKey = item;
            node.BindingIndex = bindingIndex;

            node.CreateCells();

            if (fieldNames.Count > 0)
            {
                for (int i = 0; i < fieldNames.Count; i++)
                {
                    object propertyValue = GetPropertyValue(item, fieldNames[i]);
                    if (propertyValue is Image)
                        node.Cells[i].Images.Image = (Image)propertyValue;
                    else
                        node.Cells[i].Text = GetItemText(item, fieldNames[i]);
                }
            }
            else if (item != null)
                node.Text = item.ToString();
        }


        /// <summary>
        /// Raises the DataNodeCreated event.
        /// </summary>
        /// <param name="dataNodeEventArgs">Provides event arguments.</param>
        protected virtual void OnDataNodeCreated(DataNodeEventArgs dataNodeEventArgs)
        {
            if (DataNodeCreated != null) DataNodeCreated(this, dataNodeEventArgs);
        }
        
        /// <summary>
        /// When overridden in a derived class, sets the object with the specified index in the derived class.
        /// </summary>
        /// <param name="index">The array index of the object.</param>
        /// <param name="value">The object.</param>
        protected virtual void SetItemCore(int index, object value)
        {
            Node node = _AdvTree.FindNodeByBindingIndex(index);
            if (node == null) return;

            List<string> fieldNames = new List<string>();

            foreach (DevComponents.AdvTree.ColumnHeader column in _AdvTree.Columns)
            {
                if (!string.IsNullOrEmpty(column.DataFieldName))
                    fieldNames.Add(column.DataFieldName);
            }

            SetNodeData(node, value, fieldNames, index);
        }

        private string _ParentFieldNames = "";
        /// <summary>
        /// Gets or sets comma separated field or property names that holds the value that is used to identify node and parent node. Format expected is: FieldNodeId,ParentNodeFieldId. For example if your table represents departments, you have DepartmentId field which uniquely identifies a department and ParentDepartmentId field which identifies parent of the department if any you would set this property to DepartmentId,ParentDepartmentId.
        /// Note that you can only use ParentFieldNames or GroupingMembers property but not both. If both are set ParentFieldName take precedence.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates comma separated field or property names that holds the value that is used to identify node and parent node. Format expected is: FieldNodeId,ParentNodeFieldId. For example if your table represents departments, you have DepartmentId field which uniquely identifies a department and ParentDepartmentId field which identifies parent of the department if any you would set this property to DepartmentId,ParentDepartmentId.")]
        public string ParentFieldNames
        {
            get { return _ParentFieldNames; }
            set
            {
                if (value == null) value = "";
                if (!string.IsNullOrEmpty(value))
                {
                    string[] fields = value.Split(',');
                    if (fields.Length != 2)
                        throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character.");
                    if (string.IsNullOrEmpty(fields[0]))
                        throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. First field name is empty.");
                    if (string.IsNullOrEmpty(fields[1]))
                        throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. Second field name is empty.");
                }
                _ParentFieldNames = value;
                OnParentFieldNamesChanged();
            }
        }

        /// <summary>
        /// Called when ParentFieldName property has changed.
        /// </summary>
        protected virtual void OnParentFieldNamesChanged()
        {
            RefreshItems();
        }

        private string _GroupingMembers = "";
        /// <summary>
        /// Gets or sets comma separated list of field or property names that are used for grouping when data-binding is used.
        /// </summary>
        [DefaultValue(""), Category("Data"), Description("Indicates comma separated list of field or property names that are used for grouping when data-binding is used")]
        public string GroupingMembers
        {
            get { return _GroupingMembers; }
            set
            {
                if (value == null) value = "";
                _GroupingMembers = value;
                OnGroupingMembersChanged();
            }
        }

        /// <summary>
        /// Called when GroupingMembers property has changed.
        /// </summary>
        protected virtual void OnGroupingMembersChanged()
        {
            RefreshItems();
        }

        private ElementStyle _GroupNodeStyle = null;
        /// <summary>
        /// Gets or sets style for automatically created group nodes when data-binding is used and GroupingMembers property is set.
        /// </summary>
        /// <value>
        /// Name of the style assigned or null value indicating that no style is used.
        /// Default value is null.
        /// </value>
        [Browsable(true), Category("Node Style"), DefaultValue(null), Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Description("Gets or sets default style for the node.")]
        public ElementStyle GroupNodeStyle
        {
            get { return _GroupNodeStyle; }
            set
            {
                if (_GroupNodeStyle != value)
                {
                    _GroupNodeStyle = value;
                    if (_DataManager != null && _AdvTree.Nodes != null)
                        RefreshItems();
                }
            }
        }

        private bool AllowSelection
        {
            get
            {
                return true;
            }
        }

 
        private CurrencyManager _DataManager = null;
        protected CurrencyManager DataManager
        {
            get
            {
                return _DataManager;
            }
        }
        private bool _IsDataSourceInitEventHooked = false;
        private void UnwireDataSource()
        {
            if (_DataSource is IComponent)
            {
                ((IComponent)_DataSource).Disposed -= new EventHandler(DataSourceDisposed);
            }
            ISupportInitializeNotification dataSource = _DataSource as ISupportInitializeNotification;
            if ((dataSource != null) && _IsDataSourceInitEventHooked)
            {
                dataSource.Initialized -= new EventHandler(DataSourceInitialized);
                _IsDataSourceInitEventHooked = false;
            }
        }
        private void DataSourceDisposed(object sender, EventArgs e)
        {
            this.SetDataConnection(null, null, true);
        }
        private bool _IsDataSourceInitialized = false;
        private void WireDataSource()
        {
            if (_DataSource is IComponent)
            {
                ((IComponent)_DataSource).Disposed += new EventHandler(DataSourceDisposed);
            }
            ISupportInitializeNotification dataSource = _DataSource as ISupportInitializeNotification;
            if ((dataSource != null) && !dataSource.IsInitialized)
            {
                dataSource.Initialized += new EventHandler(DataSourceInitialized);
                _IsDataSourceInitEventHooked = true;
                _IsDataSourceInitialized = false;
            }
            else
            {
                _IsDataSourceInitialized = true;
            }
        }
        private void DataSourceInitialized(object sender, EventArgs e)
        {
            this.SetDataConnection(_DataSource, _DisplayMembers, true);
        }
        /// <summary>
        /// Raises the Format event.
        /// </summary>
        /// <param name="e">Event parameters</param>
        protected virtual void OnFormat(TreeConvertEventArgs e)
        {
            TreeConvertEventHandler handler = Format;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private static TypeConverter stringTypeConverter;
        public string GetItemText(object item, string fieldName)
        {
            object propertyValue = GetPropertyValue(item, fieldName);
            if (!_FormattingEnabled)
            {
                if (item == null)
                {
                    return string.Empty;
                }
                if (propertyValue == null)
                {
                    return "";
                }
                return Convert.ToString(propertyValue, CultureInfo.CurrentCulture);
            }
            
            TreeConvertEventArgs e = new TreeConvertEventArgs(propertyValue, typeof(string), item, fieldName);
            this.OnFormat(e);
            if ((e.Value != item) && (e.Value is string))
            {
                return (string)e.Value;
            }
            if (stringTypeConverter == null)
            {
                stringTypeConverter = TypeDescriptor.GetConverter(typeof(string));
            }
            try
            {
                return (string)FormatHelper.FormatObject(propertyValue, typeof(string), GetFieldConverter(fieldName), stringTypeConverter, _FormatString, _FormatInfo, null, DBNull.Value);
            }
            catch (Exception ex)
            {
                if (ex is SecurityException || IsCriticalException(ex))
                {
                    throw;
                }
                return ((propertyValue != null) ? Convert.ToString(item, CultureInfo.CurrentCulture) : "");
            }
        }
        private static bool IsCriticalException(Exception ex)
        {
            return (((((ex is NullReferenceException) || (ex is StackOverflowException)) || ((ex is OutOfMemoryException) || (ex is ThreadAbortException))) || ((ex is ExecutionEngineException) || (ex is IndexOutOfRangeException))) || (ex is AccessViolationException));
        }
        protected object GetPropertyValue(object item, string fieldName)
        {
            if ((item != null) && (fieldName.Length > 0))
            {
                try
                {
                    PropertyDescriptor descriptor;
                    if (_DataManager != null)
                    {
                        descriptor = _DataManager.GetItemProperties().Find(fieldName, true);
                    }
                    else
                    {
                        descriptor = TypeDescriptor.GetProperties(item).Find(fieldName, true);
                    }
                    if (descriptor != null)
                    {
                        item = descriptor.GetValue(item);
                    }
                }
                catch
                {
                }
            }
            return item;
        }
        /// <summary>
        /// Gets or sets the index specifying the currently selected item.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets or sets the index specifying the currently selected item.")]
        public int SelectedIndex
        {
            get
            {
                if (_DataManager != null)
                {
                    if (_AdvTree.SelectedNode == null || _AdvTree.SelectedNode.BindingIndex < 0)
                        return -1;
                    return _AdvTree.SelectedNode.BindingIndex;
                }
                if (_AdvTree.SelectedNode == null) return -1;
                return _AdvTree.GetNodeFlatIndex(_AdvTree.SelectedNode);
            }
            set
            {
                SetSelectedIndex(value);
            }
        }
        private void SetSelectedIndex(int value)
        {
            if (value == -1)
            {
                _AdvTree.SelectedNode = null;
                return;
            }
            Node node = null;
            if (_DataManager != null)
            {
                node = _AdvTree.FindNodeByBindingIndex(value);
            }
            else
            {
                node = _AdvTree.GetNodeByFlatIndex(value);
            }
            _AdvTree.SelectedNode = node;
        }
        /// <summary>
        /// Raises the SelectedIndexChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = SelectedIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (this.DataSource != null && _AdvTree.Nodes.Count == 0 && _DisplayMembers!=null && _DisplayMembers.Count>0)
            {
                this.SetDataConnection(this.DataSource, _DisplayMembers, true);
                _AdvTree.SetPendingLayout();
            }

            base.OnParentChanged(e);
        }

        private BindingMemberInfo _ValueMember = new BindingMemberInfo();
        /// <summary>
        /// Gets or sets the property to use as the actual value for the items in the control. Applies to data-binding scenarios. SelectedValue property will return the value of selected node as indicated by this property.
        /// </summary>
        [Category("Data"), DefaultValue(""), Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Description("property to use as the actual value for the items in the control. Applies to data-binding scenarios. SelectedValue property will return the value of selected node as indicated by this property.")]
        public string ValueMember
        {
            get
            {
                return _ValueMember.BindingMember;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                BindingMemberInfo newValueMember = new BindingMemberInfo(value);
                if (!newValueMember.Equals(_ValueMember))
                {
                    if (this.DisplayMembers.Length == 0 && this.Columns.Count == 0)
                    {
                        List<BindingMemberInfo> list = new List<BindingMemberInfo>();
                        list.Add(newValueMember);
                        this.SetDataConnection(this.DataSource, list, false);
                    }
                    if (((_DataManager != null) && (value != null)) && ((value.Length != 0) && !this.BindingMemberInfoInDataManager(newValueMember)))
                    {
                        throw new ArgumentException("Invalid value for ValueMember", "value");
                    }
                    _ValueMember = newValueMember;
                    this.OnValueMemberChanged(EventArgs.Empty);
                    this.OnSelectedValueChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Raises the ValueMemberChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnValueMemberChanged(EventArgs e)
        {
            EventHandler handler = ValueMemberChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Raises the SelectedValueChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedValueChanged(EventArgs e)
        {
            EventHandler handler = SelectedValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Gets or sets the value of the member property specified by the ValueMember property.
        /// </summary>
        [Browsable(false), DefaultValue(null), Bindable(true), Category("Data"), Description("Indicates value of the member property specified by the ValueMember property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue
        {
            get
            {
                if (_DataManager != null && this.SelectedIndex != -1)
                {
                    object item = _DataManager.List[this.SelectedIndex];
                    return this.GetPropertyValue(item, _ValueMember.BindingField);
                }
                return null;
            }
            set
            {
                if (_DataManager != null)
                {
                    string bindingField = _ValueMember.BindingField;
                    if (string.IsNullOrEmpty(bindingField))
                    {
                        throw new InvalidOperationException("ValueMember property must be set to be able to set SelectedValue");
                    }
                    PropertyDescriptor property = _DataManager.GetItemProperties().Find(bindingField, true);
                    System.Reflection.MethodInfo mi = _DataManager.GetType().GetMethod("Find", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    int num = -1;
                    if (mi != null)
                    {
                        num = (int)mi.Invoke(_DataManager, new object[] { property, value, true });
                    }
                    else
                    {
                        //int num = _DataManager.Find(property, value, true);
                        // Provide an alternate implementation...
                    }
                    this.SelectedIndex = num;
                }
            }
        }
 

        //private void TextBoxSizeChanged(object sender, EventArgs e)
        //{
        //    if (!_InternalSizeUpdate)
        //        UpdateLayout();
        //}

        //private void TextBoxTextChanged(object sender, EventArgs e)
        //{
        //    base.Text = _TextBox.Text;
        //}

        //private void TextBoxTextAlignChanged(object sender, EventArgs e)
        //{
        //    OnTextAlignChanged(e);
        //}
        ///// <summary>
        ///// Raises the TextAlignChanged event.
        ///// </summary>
        //protected virtual void OnTextAlignChanged(EventArgs e)
        //{
        //    EventHandler eh = TextAlignChanged;
        //    if (eh != null)
        //        eh(this, e);
        //}

        private Font _WatermarkFont = null;
        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public virtual Font WatermarkFont
        {
            get { return _WatermarkFont; }
            set { _WatermarkFont = value; this.Invalidate(); }
        }

        private Color _WatermarkColor = SystemColors.GrayText;
        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public virtual Color WatermarkColor
        {
            get { return _WatermarkColor; }
            set { _WatermarkColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return _WatermarkColor != SystemColors.GrayText;
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            this.WatermarkColor = SystemColors.GrayText;
        }

        private bool _WatermarkEnabled = true;
        /// <summary>
        /// Gets or sets whether watermark text is displayed if set for the input items. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed if set for the input items.")]
        public virtual bool WatermarkEnabled
        {
            get { return _WatermarkEnabled; }
            set { _WatermarkEnabled = value; this.Invalidate(); }
        }

        private string _WatermarkText = "";
        /// <summary>
        /// Gets or sets the watermark text displayed on the input control when control is empty.
        /// </summary>
        [DefaultValue(""), Localizable(true), Description("Indicates watermark text displayed on the input control when control is empty."), Category("Watermark")]
        public string WatermarkText
        {
            get { return _WatermarkText; }
            set
            {
                if (value != null)
                {
                    _WatermarkText = value;
                    this.Invalidate();
                }
            }
        }

        private eTextAlignment _WatermarkAlignment = eTextAlignment.Left;
        /// <summary>
        /// Gets or sets the watermark text alignment. Default value is left.
        /// </summary>
        [Browsable(true), DefaultValue(eTextAlignment.Left), Description("Indicates watermark text alignment."), Category("Watermark")]
        public eTextAlignment WatermarkAlignment
        {
            get { return _WatermarkAlignment; }
            set
            {
                if (_WatermarkAlignment != value)
                {
                    _WatermarkAlignment = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether FocusHighlightColor is used as background color to highlight text box when it has input focus. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
        public bool FocusHighlightEnabled
        {
            get { return _FocusHighlightEnabled; }
            set
            {
                if (_FocusHighlightEnabled != value)
                {
                    _FocusHighlightEnabled = value;
                    //_TextBox.FocusHighlightEnabled = value;
                    if (this.Focused)
                        this.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color used as background color to highlight text box when it has input focus and focus highlight is enabled.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
        public Color FocusHighlightColor
        {
            get { return _FocusHighlightColor; }
            set
            {
                if (_FocusHighlightColor != value)
                {
                    _FocusHighlightColor = value;
                    //_TextBox.FocusHighlightColor = value;
                    if (this.Focused)
                        this.Invalidate(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return !_FocusHighlightColor.Equals(ColorScheme.GetColor(0xFFFF88));
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            FocusHighlightColor = ColorScheme.GetColor(0xFFFF88);
        }

        private ElementStyle _BackgroundStyle = new ElementStyle();
        /// <summary>
        /// Specifies the background style of the control.
        /// </summary>
        [Category("Style"), Description("Gets or sets control background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return _BackgroundStyle; }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            _BackgroundStyle.StyleChanged -= new EventHandler(this.VisualPropertyChanged);
            _BackgroundStyle = new ElementStyle();
            _BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);
            this.Invalidate();
        }

        private void VisualPropertyChanged(object sender, EventArgs e)
        {
            OnVisualPropertyChanged();
        }

        protected virtual void OnVisualPropertyChanged()
        {
            _ButtonGroup.InvalidateArrange();
            this.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (_BackgroundStyle != null) _BackgroundStyle.StyleChanged -= new EventHandler(VisualPropertyChanged);
            System.Windows.Forms.Timer timer = _SearchBufferExpireTimer;
            _SearchBufferExpireTimer = null;
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            if(_DropDownControl!=null)
            {
                _DropDownControl.Dispose();
                _DropDownControl = null;
            }
            base.Dispose(disposing);
        }

        private InputButtonSettings _ButtonDropDown = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that shows drop-down when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that shows drop-down when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonDropDown
        {
            get
            {
                return _ButtonDropDown;
            }
        }

        private InputButtonSettings _ButtonClear = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that clears the content of the control when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that clears the content of the control when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonClear
        {
            get
            {
                return _ButtonClear;
            }
        }


        private InputButtonSettings _ButtonCustom = null;
        /// <summary>
        /// Gets the object that describes the settings for the custom button that can execute an custom action of your choosing when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonCustom
        {
            get
            {
                return _ButtonCustom;
            }
        }

        private InputButtonSettings _ButtonCustom2 = null;
        /// <summary>
        /// Gets the object that describes the settings for the custom button that can execute an custom action of your choosing when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonCustom2
        {
            get
            {
                return _ButtonCustom2;
            }
        }


        void IInputButtonControl.InputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
        {
            OnInputButtonSettingsChanged(inputButtonSettings);
        }

        protected virtual void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
        {
            UpdateButtons();
        }

        private VisualGroup _ButtonGroup = null;
        private void UpdateButtons()
        {
            RecreateButtons();
            _ButtonGroup.InvalidateArrange();
            this.Invalidate();
        }

        protected virtual void RecreateButtons()
        {
            VisualItem[] buttons = CreateOrderedButtonList();
            // Remove all system buttons that are already in the list
            VisualGroup group = _ButtonGroup;
            VisualItem[] items = new VisualItem[group.Items.Count];
            group.Items.CopyTo(items);
            foreach (VisualItem item in items)
            {
                if (item.ItemType == eSystemItemType.SystemButton)
                {
                    group.Items.Remove(item);
                    if (item == _ButtonCustom.ItemReference)
                        item.MouseUp -= new MouseEventHandler(CustomButtonClick);
                    else if (item == _ButtonCustom2.ItemReference)
                        item.MouseUp -= new MouseEventHandler(CustomButton2Click);
                }
            }

            // Add new buttons to the list
            group.Items.AddRange(buttons);
        }

        private void CustomButtonClick(object sender, MouseEventArgs e)
        {
            if (_ButtonCustom.ItemReference.RenderBounds.Contains(e.X, e.Y))
                OnButtonCustomClick(e);
        }

        protected virtual void OnButtonCustomClick(EventArgs e)
        {
            if (ButtonCustomClick != null)
                ButtonCustomClick(this, e);
        }

        private void CustomButton2Click(object sender, MouseEventArgs e)
        {
            if (_ButtonCustom2.ItemReference.RenderBounds.Contains(e.X, e.Y))
                OnButtonCustom2Click(e);
        }

        protected virtual void OnButtonCustom2Click(EventArgs e)
        {
            if (ButtonCustom2Click != null)
                ButtonCustom2Click(this, e);
        }

        private VisualItem[] CreateOrderedButtonList()
        {
            SortedList list = CreateSortedButtonList();

            VisualItem[] items = new VisualItem[list.Count];
            list.Values.CopyTo(items, 0);

            return items;
        }

        protected virtual SortedList CreateSortedButtonList()
        {
            SortedList list = new SortedList(4);
            if (_ButtonCustom.Visible)
            {
                VisualItem button = CreateButton(_ButtonCustom);
                if (_ButtonCustom.ItemReference != null)
                    _ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButtonClick);
                _ButtonCustom.ItemReference = button;
                //button.Click += new EventHandler(CustomButtonClick);
                button.MouseUp += new MouseEventHandler(CustomButtonClick);
                button.Enabled = _ButtonCustom.Enabled;
                list.Add(_ButtonCustom, button);
            }

            if (_ButtonCustom2.Visible)
            {
                VisualItem button = CreateButton(_ButtonCustom2);
                if (_ButtonCustom.ItemReference != null)
                    _ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButton2Click);
                _ButtonCustom2.ItemReference = button;
                //button.Click += new EventHandler(CustomButton2Click);
                button.MouseUp += new MouseEventHandler(CustomButton2Click);
                button.Enabled = _ButtonCustom2.Enabled;
                list.Add(_ButtonCustom2, button);
            }

            if (_ButtonClear.Visible)
            {
                VisualItem button = CreateButton(_ButtonClear);
                if (_ButtonClear.ItemReference != null)
                    _ButtonClear.ItemReference.Click -= new EventHandler(ClearButtonClick);
                _ButtonClear.ItemReference = button;
                button.MouseUp += new MouseEventHandler(ClearButtonClick);
                list.Add(_ButtonClear, button);
            }

            if (_ButtonDropDown.Visible)
            {
                VisualItem button = CreateButton(_ButtonDropDown);
                if (_ButtonDropDown.ItemReference != null)
                {
                    _ButtonDropDown.ItemReference.MouseDown -= new MouseEventHandler(DropDownButtonMouseDown);
                }
                _ButtonDropDown.ItemReference = button;
                button.MouseDown += new MouseEventHandler(DropDownButtonMouseDown);
                list.Add(_ButtonDropDown, button);
            }

            return list;
        }

        protected virtual VisualItem CreateButton(InputButtonSettings buttonSettings)
        {
            VisualItem item = null;

            if (buttonSettings == _ButtonDropDown)
            {
                item = new VisualDropDownButton();
                ApplyButtonSettings(buttonSettings, item as VisualButton);
            }
            else
            {
                item = new VisualCustomButton();
                ApplyButtonSettings(buttonSettings, item as VisualButton);
            }

            VisualButton button = item as VisualButton;
            button.ClickAutoRepeat = false;

            if (buttonSettings == _ButtonClear)
            {
                if (buttonSettings.Image == null)
                    button.Image = DevComponents.DotNetBar.BarFunctions.LoadBitmap("SystemImages.DateReset.png");
            }

            return item;
        }

        protected virtual void ApplyButtonSettings(InputButtonSettings buttonSettings, VisualButton button)
        {
            button.Text = buttonSettings.Text;
            button.Image = buttonSettings.Image;
            button.ItemType = eSystemItemType.SystemButton;
            button.Enabled = buttonSettings.Enabled;
        }

        private void CreateButtonGroup()
        {
            VisualGroup group = new VisualGroup();
            group.HorizontalItemSpacing = 1;
            group.ArrangeInvalid += new EventHandler(ButtonGroupArrangeInvalid);
            group.RenderInvalid += new EventHandler(ButtonGroupRenderInvalid);
            _ButtonGroup = group;
        }

        private void ButtonGroupRenderInvalid(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ButtonGroupArrangeInvalid(object sender, EventArgs e)
        {
            Invalidate();
        }

        private bool _MouseOver = false;
        private PaintInfo CreatePaintInfo(Graphics g)
        {
            PaintInfo p = new PaintInfo();
            p.Graphics = g;
            p.DefaultFont = this.Font;
            p.ForeColor = this.ForeColor;
            p.RenderOffset = new System.Drawing.Point();
            Size s = this.Size;
            bool disposeStyle = false;
            ElementStyle style = GetBackgroundStyle(out disposeStyle);
            s.Height -= (ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(style, eSpacePart.Border)) + 2;
            s.Width -= (ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border)) + 2;
            p.AvailableSize = s;
            p.ParentEnabled = this.Enabled;
            p.MouseOver = _MouseOver || this.Focused;
            if(disposeStyle) style.Dispose();
            return p;
        }

        private ElementStyle GetBackgroundStyle(out bool disposeStyle)
        {
            disposeStyle = false;
            _BackgroundStyle.SetColorScheme(this.GetColorScheme());
            return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Rectangle clientRect = this.ClientRectangle;
            bool enabled = this.Enabled;

#if !TRIAL
            if (NativeFunctions.keyValidated2 != 266)
                TextDrawing.DrawString(e.Graphics, "Invalid License", this.Font, Color.FromArgb(180, Color.Red), this.ClientRectangle, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
#else
            if (NativeFunctions.ColorExpAlt() || !NativeFunctions.CheckedThrough)
		    {
			    e.Graphics.Clear(SystemColors.Control);
                return;
            }
#endif

            if (!this.Enabled)
                e.Graphics.FillRectangle(SystemBrushes.Control, clientRect);
            else
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(brush, clientRect);
            }

            bool disposeStyle = false;
            ElementStyle style = GetBackgroundStyle(out disposeStyle);

            if (style.Custom)
            {
                SmoothingMode sm = g.SmoothingMode;
                if (this.AntiAlias)
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                ElementStyleDisplayInfo displayInfo = new ElementStyleDisplayInfo(style, g, clientRect);
                if (!enabled)
                {
                    ElementStyleDisplay.PaintBorder(displayInfo);
                }
                else
                    ElementStyleDisplay.Paint(displayInfo);
                if (this.AntiAlias)
                    g.SmoothingMode = sm;
            }

            Rectangle buttonBounds = PaintButtons(g);
            // Paint selected content
            Rectangle selectionRect = GetSelectionRectangle(style, clientRect);
            if (!buttonBounds.IsEmpty)
            {
                Rectangle[] split = DisplayHelp.ExcludeRectangle(selectionRect, buttonBounds);
                if (split.Length >= 1)
                    selectionRect = split[0];
                selectionRect.Width--;
            }

            if (this.WatermarkEnabled && this.WatermarkText.Length > 0 && this.IsWatermarkRendered)
            {
                Rectangle watermarkBounds = selectionRect;
                watermarkBounds.Inflate(-1, -1);
                DrawWatermark(g, watermarkBounds);
            }

            PaintSelection(g, selectionRect);

            if(disposeStyle) style.Dispose();
        }

        private void DrawWatermark(Graphics g, Rectangle r)
        {
            if (this.WatermarkText.Length == 0) return;
            Font font = this.Font;
            if (this.WatermarkFont != null) font = this.WatermarkFont;

            eTextFormat format = eTextFormat.Default;
            if (_WatermarkAlignment == eTextAlignment.Center)
                format |= eTextFormat.HorizontalCenter;
            else if (_WatermarkAlignment == eTextAlignment.Right)
                format |= eTextFormat.Right;

            TextDrawing.DrawString(g, this.WatermarkText, font, this.WatermarkColor, r, format);
        }

        protected virtual bool IsWatermarkRendered
        {
            get
            {
                return !this.Focused && this.SelectedNode == null;
            }
        }

        private Rectangle GetSelectionRectangle(ElementStyle style, Rectangle clientRect)
        {
            return new Rectangle(ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + 1,
                ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1,
                clientRect.Width-(ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + 2),
                clientRect.Height-(ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(style, eSpacePart.Border) + 2));

        }

        /// <summary>
        /// Renders the selected node inside the combo box.
        /// </summary>
        /// <param name="g">Graphics reference.</param>
        /// <param name="bounds">Render bounds.</param>
        protected virtual void PaintSelection(Graphics g, Rectangle bounds)
        {
            if (bounds.Width < 2 || bounds.Height < 2) return;

            if (this.Focused && !this.IsPopupOpen)
            {
                SelectionRendererEventArgs selectionArgs = new SelectionRendererEventArgs();
                selectionArgs.Bounds = bounds;
                selectionArgs.Graphics = g;
                selectionArgs.SelectionBoxStyle = eSelectionStyle.HighlightCells;
                selectionArgs.TreeActive = true;
                ((DevComponents.AdvTree.Display.NodeTreeDisplay)_AdvTree.NodeDisplay).GetTreeRenderer().DrawSelection(selectionArgs);
            }

            Node node = _AdvTree.SelectedNode;
            if (node != null)
            {
                if (node.BoundsRelative.IsEmpty || _AdvTree.IsLayoutPending || node.BoundsRelative.Width <= 0)
                {
                    UpdateTreeSize();
                    if (_AdvTree.IsLayoutPending) _AdvTree.RecalcLayout();
                }

                Region oldClip = g.Clip;
                g.SetClip(bounds);

                Rectangle nodeRenderBounds = bounds;

                if (!string.IsNullOrEmpty(_SelectedDisplayMemeber))
                {
                    int index = _AdvTree.Columns.IndexOfField(_SelectedDisplayMemeber);
                    nodeRenderBounds.Y--;
                    nodeRenderBounds.X++;
                    nodeRenderBounds.Width--;
                    if (index >= 0)
                    {
                        TextDrawing.DrawString(g, node.Cells[index].Text, this.Font, this.ForeColor, nodeRenderBounds, eTextFormat.Left | eTextFormat.NoClipping | eTextFormat.NoPadding | eTextFormat.VerticalCenter);
                    }
                }
                else
                {

                    if (DevComponents.AdvTree.Display.NodeDisplay.DrawExpandPart(node))
                    {
                        nodeRenderBounds.X -= _AdvTree.ExpandWidth;
                    }
                    else
                        nodeRenderBounds.X -= (node.Bounds.X - node.BoundsRelative.X - 2);
                    if (nodeRenderBounds.Height > node.BoundsRelative.Height)
                        nodeRenderBounds.Y += (nodeRenderBounds.Height - node.BoundsRelative.Height) / 2;
                    else
                        nodeRenderBounds.Y += 1;
                
                    ((DevComponents.AdvTree.Display.NodeTreeDisplay)_AdvTree.NodeDisplay).ExternalPaintNode(node, g, nodeRenderBounds);
                }

                g.Clip = oldClip;
                if(oldClip!=null) oldClip.Dispose();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor()
        {
            return this.BackColor != SystemColors.Window;
        }

        //protected override void OnBackColorChanged(EventArgs e)
        //{
        //    base.OnBackColorChanged(e);
        //    _TextBox.BackColor = this.BackColor;
        //}

        private Rectangle PaintButtons(Graphics g)
        {
            PaintInfo p = CreatePaintInfo(g);

            if (!_ButtonGroup.IsLayoutValid)
            {
                UpdateLayout(p);
            }

            bool disposeStyle = false;
            ElementStyle style = GetBackgroundStyle(out disposeStyle);
            _ButtonGroup.RenderBounds = GetButtonsRenderBounds(style);
            _ButtonGroup.ProcessPaint(p);
            if(disposeStyle) style.Dispose();
            return _ButtonGroup.RenderBounds;
        }

        private Rectangle GetButtonsRenderBounds(ElementStyle style)
        {
            if (this.RightToLeft == RightToLeft.Yes)
            {
                return new Rectangle((ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + 1), ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1,
                _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
            }

            return new Rectangle(this.Width - (ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) + 1) - _ButtonGroup.Size.Width, ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1,
                _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
        }

        protected override void OnResize(EventArgs e)
        {
            _ButtonGroup.InvalidateArrange();
            UpdateLayout();
            this.Invalidate();
            base.OnResize(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (this.IsPopupOpen) this.IsPopupOpen = false;
            this.Invalidate();
            base.OnLeave(e);
        }

        //protected override void OnGotFocus(EventArgs e)
        //{
        //    if(_TextBox.Visible)
        //        _TextBox.Focus();
        //    base.OnGotFocus(e);
        //}

        protected override bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (this.Focused)
            {
                Keys key = (Keys)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                if (key == Keys.None)
                    key = (Keys)wParam.ToInt32();
                if (key == Keys.F4)
                {
                    this.IsPopupOpen = !this.IsPopupOpen;
                    return true;
                }
                else if (key == Keys.Down && (Control.ModifierKeys & Keys.Alt) != 0 && !this.IsPopupOpen)
                {
                    this.IsPopupOpen = !this.IsPopupOpen;
                    return true;
                }
                else if (this.IsPopupOpen && key == Keys.Tab)
                    this.IsPopupOpen = false;
                else if (this.IsPopupOpen)
                {
                    if (key == Keys.Up)
                    {
                        this.SelectPreviousNode(eTreeAction.Keyboard);
                        return true;
                    }
                    else if (key == Keys.Down)
                    {
                        this.SelectNextNode(eTreeAction.Keyboard);
                        return true;
                    }
                    else
                    {
                        try
                        {
                            byte[] keyState = new byte[256];
                            if (NativeFunctions.GetKeyboardState(keyState))
                            {
                                byte[] chars = new byte[2];
                                if (NativeFunctions.ToAscii((uint)key, 0, keyState, chars, 0) != 0)
                                {
                                    char[] characters = new char[2];
                                    System.Text.Encoding.Default.GetDecoder().GetChars(chars, 0, 2, characters, 0);
                                    if (ProcessKeyboardCharacter(characters[0]))
                                        return true;
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            return base.OnKeyDown(hWnd, wParam, lParam);
        }

        protected override bool IsInputChar(char charCode)
        {
            if(_KeyboardSearchEnabled && char.IsLetterOrDigit(charCode))
                return true;
            return base.IsInputChar(charCode);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if(_KeyboardSearchEnabled)
                e.Handled = ProcessKeyboardCharacter(e.KeyChar);
            base.OnKeyPress(e);
        }

        private bool _KeyboardSearchEnabled = true;
        /// <summary>
        /// Gets or sets whether keyboard incremental search is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether keyboard incremental search is enabled.")]
        public bool KeyboardSearchEnabled
        {
            get { return _KeyboardSearchEnabled; }
            set
            {
                _KeyboardSearchEnabled = value;
            }
        }

        private bool _KeyboardSearchNoSelectionAllowed = true;
        /// <summary>
        /// Gets or sets whether during keyboard search selected node can be set to nothing/null if there is no match found. 
        /// Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether during keyboard search selected node can be set to nothing/null if there is no match found.")]
        public bool KeyboardSearchNoSelectionAllowed
        {
            get { return _KeyboardSearchNoSelectionAllowed; }
            set
            {
                _KeyboardSearchNoSelectionAllowed = value;
            }
        }
        
        

        private System.Windows.Forms.Timer _SearchBufferExpireTimer = null;
        private bool ProcessKeyboardCharacter(char p)
        {
            if (!char.IsLetterOrDigit(p)) return false;
            string searchString = UpdateSearchBuffer(p.ToString());
            Node node = _AdvTree.FindNodeByText(searchString, _AdvTree.SelectedNode, true);
            if (node == null && _AdvTree.SelectedNode != null)
            {
                // Try from top searching
                node = _AdvTree.FindNodeByText(searchString, true);
            }
            // If binding to data do not select the groupping virtual nodes
            if (_DataSource != null && node != null && node.BindingIndex == -1)
            {
                while (node != null && node.BindingIndex == -1)
                    node = _AdvTree.FindNodeByText(searchString, node, true);
            }
            if (!_KeyboardSearchNoSelectionAllowed && node == null) return false;

            SelectNode(node, eTreeAction.Keyboard);
            return false;
        }

        private int _SearchBufferExpireTimeout = 1000;
        /// <summary>
        /// Gets or sets the keyboard search buffer expiration timeout. Default value is 1000 which indicates that
        /// key pressed within 1 second will add to the search buffer and control will be searched for node text
        /// that begins with resulting string. Setting this value to 0 will disable the search buffer.
        /// </summary>
        [DefaultValue(1000), Category("Behavior"), Description("Indicates keyboard search buffer expiration timeout.")]
        public int SearchBufferExpireTimeout
        {
            get { return _SearchBufferExpireTimeout; }
            set
            {
                if (value < 0) value = 0;
                _SearchBufferExpireTimeout = value;
            }
        }

        private string _SearchBuffer = "";
        private string UpdateSearchBuffer(string s)
        {
            if (_SearchBufferExpireTimeout <= 0)
                return s;

            if (_SearchBufferExpireTimer == null)
            {
                _SearchBufferExpireTimer = new System.Windows.Forms.Timer();
                _SearchBufferExpireTimer.Interval = _SearchBufferExpireTimeout;
                _SearchBufferExpireTimer.Tick += new EventHandler(SearchBufferExpireTimerTick);
                _SearchBufferExpireTimer.Start();
            }
            else
                _SearchBufferExpireTimer.Start();
            _SearchBuffer += s;
            return _SearchBuffer;
        }

        private void SearchBufferExpireTimerTick(object sender, EventArgs e)
        {
            _SearchBufferExpireTimer.Stop();
            _SearchBuffer = "";
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (this.Focused && Control.ModifierKeys != Keys.Alt)
                return true;
            return base.ProcessMnemonic(charCode);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Down) == Keys.Down && (Control.ModifierKeys & Keys.Alt) != 0 && !this.IsPopupOpen)
            {
                this.IsPopupOpen = !this.IsPopupOpen;
                return true;
            }
            else if (keyData == Keys.Up)
            {
                this.SelectPreviousNode(eTreeAction.Keyboard);
                return true;
            }
            else if (keyData == Keys.Down)
            {
                this.SelectNextNode(eTreeAction.Keyboard);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SelectNextNode(eTreeAction action)
        {
            Node node = null;
            if (this.SelectedNode == null)
                node = NodeOperations.GetFirstVisibleNode(_AdvTree);
            else
                node = NodeOperations.GetNextVisibleNode(this.SelectedNode);

            if (node != null && !node.CanSelect)
            {
                int counter = 0;
                while (node != null && counter < 100)
                {
                    node = NodeOperations.GetNextVisibleNode(node);
                    if (node != null && node.CanSelect) break;
                }
            }
            
            if (node != null)
            {
                if (_DataManager != null)
                {
                    while (node != null && node.DataKey == null)
                        node = NodeOperations.GetNextVisibleNode(node);
                    if (node != null)
                        SelectNode(node, action);
                }
                else
                    SelectNode(node, action);
            }
        }

        private void SelectPreviousNode(eTreeAction action)
        {
            Node node = null;
            if (this.SelectedNode == null)
                node = NodeOperations.GetLastVisibleNode(_AdvTree);
            else
                node = NodeOperations.GetPreviousVisibleNode(this.SelectedNode);

            if (node != null && !node.CanSelect)
            {
                int counter = 0;
                while (node != null && counter < 100)
                {
                    node = NodeOperations.GetPreviousVisibleNode(node);
                    if (node != null && node.CanSelect) break;
                }
            }

            if (node != null)
            {
                if (_DataManager != null)
                {
                while (node != null && node.DataKey == null)
                    node = NodeOperations.GetNextVisibleNode(node);
                if (node != null)
                    SelectNode(node, action);
            }
            else
                SelectNode(node, action);
            }
        }

        private void SelectNode(Node node, eTreeAction action)
        {
            _AdvTree.SelectNode(node, action);
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            _ButtonGroup.InvalidateArrange();
            UpdateLayout();
            this.Invalidate();
            base.OnRightToLeftChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            _ButtonGroup.InvalidateArrange();
            _AdvTree.Font = this.Font;
            UpdateLayout();
            this.Invalidate();
            base.OnFontChanged(e);
        }

        private void UpdateLayout()
        {
            using (Graphics g = BarFunctions.CreateGraphics(this))
                UpdateLayout(CreatePaintInfo(g));
        }

        private bool _InternalSizeUpdate = false;
        private void UpdateLayout(PaintInfo p)
        {
            if (_InternalSizeUpdate) return;

            _InternalSizeUpdate = true;

            try
            {
                if (!_ButtonGroup.IsLayoutValid)
                {
                    _ButtonGroup.PerformLayout(p);
                }

                bool disposeStyle = false;
                ElementStyle style = GetBackgroundStyle(out disposeStyle);

                Rectangle textBoxControlRect = ElementStyleLayout.GetInnerRect(style, this.ClientRectangle);
                if (RenderButtons)
                {
                    Rectangle buttonsRect = GetButtonsRenderBounds(style);
                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        textBoxControlRect.X += buttonsRect.Right;
                        textBoxControlRect.Width -= buttonsRect.Right;
                    }
                    else
                    {
                        textBoxControlRect.Width -= (textBoxControlRect.Right - buttonsRect.X);
                    }
                }

                if (disposeStyle) style.Dispose();
                //if (_TextBox.PreferredHeight < textBoxControlRect.Height)
                //{
                //    textBoxControlRect.Y += (textBoxControlRect.Height - _TextBox.PreferredHeight) / 2;
                //}
                //_TextBox.Bounds = textBoxControlRect;
            }
            finally
            {
                _InternalSizeUpdate = false;
            }
        }

        private Control _PreviousDropDownControlParent = null;
        private void DropDownButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 250)
            {
                _CloseTime = DateTime.MinValue;
                return;
            }

            ShowDropDown();
        }

        private void UpdateTreeSize()
        {
            _AdvTree.BeginUpdate();
            if (_DropDownWidth == 0) _AdvTree.Width = this.Width - 4;
            
            if (_DropDownHeight == 0)
                _AdvTree.Height = 600;
            else
                _AdvTree.Height = _DropDownHeight;
            _AdvTree.EndUpdate(false);

            if (_DropDownHeight == 0)
            {
                // Auto calculate height based on content
                _AdvTree.RecalcLayout();
                ScreenInformation screenInfo = BarFunctions.ScreenFromControl(this);
                if (screenInfo != null && _AdvTree.NodeLayout.Height > screenInfo.WorkingArea.Height / 2)
                {
                    _AdvTree.Height = screenInfo.WorkingArea.Height / 2;
                }
                else if (_AdvTree.Nodes.Count > 0)
                    _AdvTree.Height = _AdvTree.NodeLayout.Height + 4;//(_AdvTree.ColumnHeaderHeight > 0 ? (_AdvTree.ColumnHeaderHeight + 4) : 4);
                else
                    _AdvTree.Height = this.Height;
            }
            else
            {
                _AdvTree.Height = _DropDownHeight;
            }
        }

        private bool _ShowingPopup = false;
        /// <summary>
        /// Shows drop-down popup. Note that popup will be shown only if there is a DropDownControl assigned or DropDownItems collection has at least one item.
        /// </summary>
        public void ShowDropDown()
        {

            if (_DropDownControl == null && _PopupItem.SubItems.Count == 0 || _ShowingPopup || _PopupItem.Expanded)
                return;

            _ShowingPopup = true;
            try
            {
                UpdateTreeSize();
                ControlContainerItem cc = null;
                ItemContainer ic = null;
                if (_DropDownControl != null)
                {
                    ic = new ItemContainer();
                    ic.Name = _DropDownItemContainerName;
                    cc = new ControlContainerItem(_DropDownControlContainerName);
                    ic.SubItems.Add(cc);
                    _PopupItem.SubItems.Insert(0, ic);
                }

                CancelEventArgs cancelArgs = new CancelEventArgs();
                OnButtonDropDownClick(cancelArgs);
                if (cancelArgs.Cancel || _PopupItem.SubItems.Count == 0)
                {
                    if (ic != null)
                        _PopupItem.SubItems.Remove(ic);
                    ic.Dispose();
                    return;
                }

                _PreviousDropDownControlParent = _DropDownControl.Parent;
                cc.Control = _DropDownControl;

                _PopupItem.SetDisplayRectangle(this.ClientRectangle);
                if (this.RightToLeft == RightToLeft.No)
                {
                    Point pl = new Point(this.Width - _PopupItem.PopupSize.Width, this.Height);
                    ScreenInformation screen = BarFunctions.ScreenFromControl(this);
                    Point ps = PointToScreen(pl);
                    if (screen != null && screen.WorkingArea.X > ps.X)
                    {
                        pl.X = 0;
                    }
                    _PopupItem.PopupLocation = pl;
                }
                if (!IsPopupOpen && this.SelectedNode != null)
                    this.SelectedNode.EnsureVisible();
                    
                _PopupItem.Expanded = !_PopupItem.Expanded;
            }
            finally
            {
                _ShowingPopup = false;
            }
            this.Invalidate();
        }

        /// <summary>
        /// Closes the drop-down popup if it is open.
        /// </summary>
        public void CloseDropDown()
        {
            if (_PopupItem.Expanded) _PopupItem.Expanded = false;
        }

        private DateTime _CloseTime = DateTime.MinValue;
        private void DropDownPopupClose(object sender, EventArgs e)
        {
            _CloseTime = DateTime.Now;
            ItemContainer ic = _PopupItem.SubItems[_DropDownItemContainerName] as ItemContainer;
            if (ic != null)
            {
                ControlContainerItem cc = ic.SubItems[_DropDownControlContainerName] as ControlContainerItem;
                if (cc != null)
                {
                    cc.Control = null;
                    ic.SubItems.Remove(cc);
                    cc.Dispose();
                    if (_DropDownControl != null)
                    {
                        _DropDownControl.Parent = _PreviousDropDownControlParent;
                        _PreviousDropDownControlParent = null;
                    }
                }
                _PopupItem.SubItems.Remove(ic);
                ic.Dispose();
            }
            this.Invalidate();
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonClearClick(cancelArgs);
            if (cancelArgs.Cancel) return;

            this.SelectedNode = null;
        }

        /// <summary>
        /// Raises the ButtonClearClick event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonClearClick(CancelEventArgs e)
        {
            if (ButtonClearClick != null)
                ButtonClearClick(this, e);
        }

        /// <summary>
        /// Raises the ButtonDropDownClick event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonDropDownClick(CancelEventArgs e)
        {
            if (ButtonDropDownClick != null)
                ButtonDropDownClick(this, e);
        }

        private Control _DropDownControl = null;
        /// <summary>
        /// Gets or sets the reference of the control that will be displayed on popup that is shown when drop-down button is clicked.
        /// </summary>
        [DefaultValue(null), Description("Indicates reference of the control that will be displayed on popup that is shown when drop-down button is clicked.")]
        internal Control DropDownControl
        {
            get { return _DropDownControl; }
            set
            {
                _DropDownControl = value;
            }
        }

        protected override PopupItem CreatePopupItem()
        {
            ButtonItem button = new ButtonItem("sysPopupProvider");
            button.PopupFinalized += new EventHandler(DropDownPopupClose);
            _PopupItem = button;
            return button;
        }

        /// <summary>
        /// Gets the collection of BaseItem derived items displayed on popup menu.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SubItemsCollection DropDownItems
        {
            get { return _PopupItem.SubItems; }
        }

        protected override void RecalcSize()
        {
        }

        public override void PerformClick()
        {
        }

        private bool RenderButtons
        {
            get
            {
                return _ButtonCustom != null && _ButtonCustom.Visible || _ButtonCustom2 != null && _ButtonCustom2.Visible ||
                    _ButtonDropDown != null && _ButtonDropDown.Visible || _ButtonClear != null && _ButtonClear.Visible;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RenderButtons)
            {
                _ButtonGroup.ProcessMouseMove(e);
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (RenderButtons)
            {
                _ButtonGroup.ProcessMouseLeave();
            }
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            if (RenderButtons)
            {
                _ButtonGroup.ProcessMouseDown(e);
            }

            Rectangle r = this.ClientRectangle;
            r.Inflate(1, 1);
            if (this.RenderButtons && !_ButtonGroup.RenderBounds.Contains(e.X, e.Y) && r.Contains(e.X, e.Y) && !IsPopupOpen)
            {
                if (_CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 150)
                {
                    _CloseTime = DateTime.MinValue;
                }
                else
                    IsPopupOpen = true;
            }

            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (RenderButtons)
                _ButtonGroup.ProcessMouseUp(e);
            base.OnMouseUp(e);
        }

        ///// <summary>
        ///// Gets the reference to internal TextBox control. Use it to get access to the text box events and properties.
        ///// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public TextBox TextBox
        //{
        //    get
        //    {
        //        return _TextBox;
        //    }
        //}
        /// <summary>
        /// Gets the reference to internal AdvTree control that is displayed on popup Use it to get access to the AdvTree events and properties.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AdvTree.AdvTree AdvTree
        {
            get
            {
                return _AdvTree;
            }
        }

        ///// <summary>
        ///// Gets or sets whether watermark text is displayed when control is empty. Default value is true.
        ///// </summary>
        //[DefaultValue(true), Description("Indicates whether watermark text is displayed when control is empty.")]
        //public virtual bool WatermarkEnabled
        //{
        //    get { return _TextBox.WatermarkEnabled; }
        //    set { _TextBox.WatermarkEnabled = value; this.Invalidate(true); }
        //}

        ///// <summary>
        ///// Gets or sets the watermark (tip) text displayed inside of the control when Text is not set and control does not have input focus. This property supports text-markup.
        ///// </summary>
        //[Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus."), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        //public string WatermarkText
        //{
        //    get { return _TextBox.WatermarkText; }
        //    set
        //    {
        //        if (value == null) value = "";
        //        _TextBox.WatermarkText = value;
        //        this.Invalidate(true);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the watermark font.
        ///// </summary>
        //[Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        //public Font WatermarkFont
        //{
        //    get { return _TextBox.WatermarkFont; }
        //    set { _TextBox.WatermarkFont = value; this.Invalidate(true); }
        //}

        ///// <summary>
        ///// Gets or sets the watermark text color.
        ///// </summary>
        //[Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        //public Color WatermarkColor
        //{
        //    get { return _TextBox.WatermarkColor; }
        //    set { _TextBox.WatermarkColor = value; this.Invalidate(); }
        //}
        ///// <summary>
        ///// Indicates whether property should be serialized by Windows Forms designer.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeWatermarkColor()
        //{
        //    return _TextBox.WatermarkColor != SystemColors.GrayText;
        //}
        ///// <summary>
        ///// Resets the property to default value.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetWatermarkColor()
        //{
        //    this.WatermarkColor = SystemColors.GrayText;
        //}

        ///// <summary>
        ///// Gets or sets the watermark hiding behaviour. Default value indicates that watermark is hidden when control receives input focus.
        ///// </summary>
        //[DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior"), Description("Indicates watermark hiding behaviour.")]
        //public eWatermarkBehavior WatermarkBehavior
        //{
        //    get { return _TextBox.WatermarkBehavior; }
        //    set { _TextBox.WatermarkBehavior = value; this.Invalidate(true); }
        //}

        private int _DropDownHeight = 0;
        /// <summary>
        /// Gets or sets the height in pixels of the drop-down portion of the ComboTreeBox control.
        /// </summary>
        [DefaultValue(0), Category("Behavior"), Description("Indicates height in pixels of the drop-down portion of the ComboTreeBox control.")]
        public int DropDownHeight
        {
            get { return _DropDownHeight; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("DropDownHeight Value must be equal or greater than zero.");
                _DropDownHeight = value;
                OnDropDownSizeChanged();
            }
        }
        private int _DropDownWidth = 0;
        /// <summary>
        /// Gets or sets the width in pixels of the drop-down portion of the ComboTreeBox control.
        /// </summary>
        [DefaultValue(0), Category("Behavior"), Description("Indicates width in pixels of the drop-down portion of the ComboTreeBox control.")]
        public int DropDownWidth
        {
            get { return _DropDownWidth; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("DropDownWidth Value must be equal or greater than zero.");
                _DropDownWidth = value;
                OnDropDownSizeChanged();
            }
        }
        private void OnDropDownSizeChanged()
        {
            _AdvTree.BeginUpdate();
            if (_DropDownWidth > 0)
                _AdvTree.Width = _DropDownWidth;
            if (_DropDownHeight > 0)
                _AdvTree.Height = _DropDownHeight;
            _AdvTree.EndUpdate(false);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the combo box is displaying its drop-down portion.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPopupOpen
        {
            get 
            {
                return _PopupItem.Expanded; 
            }
            set
            {
                if (value != _PopupItem.Expanded)
                {
                    if (value)
                        ShowDropDown();
                    else
                        CloseDropDown();
                }
            }
        }

        /// <summary>
        /// Gets the collection of tree nodes that are assigned to the popup tree view control.
        /// </summary>
        /// <value>
        /// A <see cref="NodeCollection">NodeCollection</see> that represents the tree nodes
        /// assigned to the tree control.
        /// </value>
        /// <remarks>
        /// 	<para>The Nodes property holds a collection of Node objects, each of which has a
        ///     Nodes property that can contain its own NodeCollection.</para>
        /// </remarks>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets the collection of tree nodes that are assigned to the tree control.")]
        public AdvTree.NodeCollection Nodes
        {
            get { return _AdvTree.Nodes; }
        }

        /// <summary>
        /// Gets the collection of column headers that appear in the popup tree.
        /// </summary>
        /// <remarks>
        /// 	<para>By default there are no column headers defined. In that case tree control
        ///     functions as regular tree control where text has unrestricted width.</para>
        /// 	<para>If you want to restrict the horizontal width of the text but not display
        ///     column header you can create one column and set its width to the width desired and
        ///     set its Visible property to false.</para>
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Columns"), Description("Gets collection of column headers that appear in the popup tree.")]
        public ColumnHeaderCollection Columns
        {
            get
            {
                return _AdvTree.Columns;
            }
        }

        /// <summary>
        /// Gets or sets whether column headers are visible if they are defined through Columns collection. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Columns"), Description("Indicates whether column headers are visible if they are defined through Columns collection.")]
        public bool ColumnsVisible
        {
            get { return _AdvTree.ColumnsVisible; }
            set
            {
                _AdvTree.ColumnsVisible = value;
            }
        }
        /// <summary>
        /// Gets or sets whether grid lines are displayed when columns are defined. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Columns"), Description("Indicates whether grid lines are displayed when columns are defined.")]
        public bool GridColumnLines
        {
            get { return _AdvTree.GridColumnLines; }
            set
            {
                _AdvTree.GridColumnLines = value;
            }
        }
        /// <summary>
        /// Gets or sets the grid lines color.
        /// </summary>
        [Category("Columns"), Description("Indicates grid lines color.")]
        public Color GridLinesColor
        {
            get { return _AdvTree.GridLinesColor; }
            set { _AdvTree.GridLinesColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeGridLinesColor()
        {
            return _AdvTree.ShouldSerializeGridLinesColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetGridLinesColor()
        {
            _AdvTree.ResetGridLinesColor();
        }
        /// <summary>
        /// Gets or sets whether horizontal grid lines between each row are displayed. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Columns"), Description("")]
        public bool GridRowLines
        {
            get { return _AdvTree.GridRowLines; }
            set
            {
               _AdvTree.GridRowLines = value;
            }
        }
        /// <summary>
        /// Gets or sets whether node is highlighted when mouse enters the node. Default value is false.
        /// </summary>
        /// <remarks>
        /// There are two ways to enable the node hot-tracking. You can set the HotTracking property to true in which case the
        /// mouse tracking is enabled using system colors specified in TreeColorTable. You can also define the NodeStyleMouseOver 
        /// style which gets applied to the node when mouse is over the node.
        /// </remarks>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether node is highlighted when mouse enters the node.")]
        public bool HotTracking
        {
            get { return _AdvTree.HotTracking; }
            set
            {
                _AdvTree.HotTracking = value;
            }
        }

        /// <summary>
        /// Gets or sets the node selection box style for popup tree control.
        /// </summary>
        /// <seealso cref="SelectionBox">SelectionBox Property</seealso>
        /// <seealso cref="SelectionBoxSize">SelectionBoxSize Property</seealso>
        /// <seealso cref="SelectionBoxFillColor">SelectionBoxFillColor Property</seealso>
        /// <seealso cref="SelectionBoxBorderColor">SelectionBoxBorderColor Property</seealso>
        [DefaultValue(eSelectionStyle.HighlightCells), Category("Selection"), Description("Indicates node selection box style of popup tree control.")]
        public eSelectionStyle SelectionBoxStyle
        {
            get { return _AdvTree.SelectionBoxStyle; }
            set { _AdvTree.SelectionBoxStyle = value; }
        }

        /// <summary>
		/// Gets or sets the ImageList that contains the Image objects used by the tree nodes.
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Images"),Description("Indicates the ImageList that contains the Image objects used by the tree nodes.")]
		public ImageList ImageList
		{
			get
			{
				return _AdvTree.ImageList;
			}
			set
			{
				_AdvTree.ImageList=value;
			}
		}

		/// <summary>
		/// Gets or sets the image-list index value of the default image that is displayed by the tree nodes.
		/// </summary>
        [Browsable(true), Category("Images"), Description("Indicates the image-list index value of the default image that is displayed by the tree nodes."), Editor("DevComponents.AdvTree.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), TypeConverter(typeof(ImageIndexConverter)), DefaultValue(-1)]
        public int ImageIndex
        {
            get { return _AdvTree.ImageIndex; }
            set
            {
                _AdvTree.ImageIndex = value;
            }
        }
        #endregion

        #region Property Forwarding
        ///// <summary>
        ///// Gets or sets a custom StringCollection to use when the AutoCompleteSource property is set to CustomSource.
        ///// <value>A StringCollection to use with AutoCompleteSource.</value>
        ///// </summary>
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Localizable(true), Description("Indicates custom StringCollection to use when the AutoCompleteSource property is set to CustomSource."), Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        //private AutoCompleteStringCollection AutoCompleteCustomSource
        //{
        //    get
        //    {
        //        return _TextBox.AutoCompleteCustomSource;
        //    }
        //    set
        //    {
        //        _TextBox.AutoCompleteCustomSource = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets an option that controls how automatic completion works for the TextBox.
        ///// <value>One of the values of AutoCompleteMode. The values are Append, None, Suggest, and SuggestAppend. The default is None.</value>
        ///// </summary>
        //[Description("Gets or sets an option that controls how automatic completion works for the TextBox."), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0)]
        //private AutoCompleteMode AutoCompleteMode
        //{
        //    get
        //    {
        //        return _TextBox.AutoCompleteMode;
        //    }
        //    set
        //    {
        //        _TextBox.AutoCompleteMode = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets a value specifying the source of complete strings used for automatic completion.
        ///// <value>One of the values of AutoCompleteSource. The options are AllSystemSources, AllUrl, FileSystem, HistoryList, RecentlyUsedList, CustomSource, and None. The default is None.</value>
        ///// </summary>
        //[DefaultValue(0x80), TypeConverter(typeof(TextBoxAutoCompleteSourceConverter)), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
        //private AutoCompleteSource AutoCompleteSource
        //{
        //    get
        //    {
        //        return _TextBox.AutoCompleteSource;
        //    }
        //    set
        //    {
        //        _TextBox.AutoCompleteSource = value;
        //    }
        //}
        #endregion

        #region ICommandSource Members
        protected virtual void ExecuteCommand()
        {
            if (_Command == null) return;
            CommandManager.ExecuteCommand(this);
        }

        /// <summary>
        /// Gets or sets the command assigned to the item. Default value is null.
        /// <remarks>Note that if this property is set to null Enabled property will be set to false automatically to disable the item.</remarks>
        /// </summary>
        [DefaultValue(null), Category("Commands"), Description("Indicates the command assigned to the item.")]
        public Command Command
        {
            get { return (Command)((ICommandSource)this).Command; }
            set
            {
                ((ICommandSource)this).Command = value;
            }
        }

        private ICommand _Command = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ICommand ICommandSource.Command
        {
            get
            {
                return _Command;
            }
            set
            {
                bool changed = false;
                if (_Command != value)
                    changed = true;

                if (_Command != null)
                    CommandManager.UnRegisterCommandSource(this, _Command);
                _Command = value;
                if (value != null)
                    CommandManager.RegisterCommand(this, value);
                if (changed)
                    OnCommandChanged();
            }
        }

        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected virtual void OnCommandChanged()
        {
        }

        private object _CommandParameter = null;
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Commands"), Description("Indicates user defined data value that can be passed to the command when it is executed."), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)), System.ComponentModel.Localizable(true)]
        public object CommandParameter
        {
            get
            {
                return _CommandParameter;
            }
            set
            {
                _CommandParameter = value;
            }
        }

        #endregion

        #region Licensing
#if !TRIAL
        private string _LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return _LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                _LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        #endregion
    }
    /// <summary>
    /// Represents the method that will handle converting for ComboTree control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TreeConvertEventHandler(object sender, TreeConvertEventArgs e);

    public class TreeConvertEventArgs : ConvertEventArgs
    {
        // Fields
        private object _ListItem;

        // Methods
        public TreeConvertEventArgs(object value, Type desiredType, object listItem, string fieldName)
            : base(value, desiredType)
        {
            _ListItem = listItem;
            _FieldName = fieldName;
        }

        /// <summary>
        /// Gets the reference to the item being converted.
        /// </summary>
        public object ListItem
        {
            get
            {
                return _ListItem;
            }
        }

        private string _FieldName = "";
        /// <summary>
        /// Get the reference to the name of the field or property on the item that needs conversion.
        /// </summary>
        public string FieldName
        {
            get { return _FieldName; }
        }
    }
    /// <summary>
    /// Defines delegate for data node based events.
    /// </summary>
    public delegate void DataNodeEventHandler(object sender, DataNodeEventArgs e);
    /// <summary>
    /// Defines event arguments for data node based events.
    /// </summary>
    public class DataNodeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the node that is created for data item.
        /// </summary>
        public Node Node = null;
        /// <summary>
        /// Gets the data-item node is being created for.
        /// </summary>
        public readonly object DataItem = null;

        /// <summary>
        /// Initializes a new instance of the DataNodeEventArgs class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dataItem"></param>
        public DataNodeEventArgs(Node node, object dataItem)
        {
            Node = node;
            DataItem = dataItem;
        }
    }
    /// <summary>
    /// Defines delegate for data column based events.
    /// </summary>
    public delegate void DataColumnEventHandler(object sender, DataColumnEventArgs e);
    /// <summary>
    /// Defines event arguments for data column based events.
    /// </summary>
    public class DataColumnEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the column header that is created for data.
        /// </summary>
        public DevComponents.AdvTree.ColumnHeader ColumnHeader = null;

        /// <summary>
        /// Initializes a new instance of the DataColumnEventArgs class.
        /// </summary>
        /// <param name="header"></param>
        public DataColumnEventArgs(DevComponents.AdvTree.ColumnHeader header)
        {
            ColumnHeader = header;
        }
    }
}
#endif