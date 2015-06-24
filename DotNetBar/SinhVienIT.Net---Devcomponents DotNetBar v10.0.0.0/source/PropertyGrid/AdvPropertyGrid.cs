#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DevComponents.AdvTree;
using System.Drawing;
using System.Collections.Generic;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents advanced property grid control.
    /// </summary>
    [ToolboxItem(true), ToolboxBitmap(typeof(AdvPropertyGrid), "AdvPropertyGrid.ico")]
    public class AdvPropertyGrid : ContainerControl, INotifyPropertyChanged, IPropertyGridLocalizer, IPropertyElementStyleProvider, ISupportInitialize
    {
        private const string STR_DefaultNodeStyle = "DefaultNodeStyle";

        #region Constructors
        private Bar _Toolbar = null;
        private AdvTree.AdvTree _PropertyTree = null;
        private ExpandableSplitter _ExpandableSplitter = null;
        private PanelControl _HelpPanel = null;
        private SuperTooltip _SuperTooltip = null;
        private ElementStyle _CategoryStyle = null;
        private ElementStyle _ReadOnlyStyle = null;
        private ElementStyle _ValueChangedStyle = null;
        private AdvPropertyGridLocalization _SystemText = null;
        private TextBoxItem _SearchTextBox = null;

        /// <summary>
        /// Initializes a new instance of the AdvPropertyGrid class.
        /// </summary>
        public AdvPropertyGrid()
        {
            _SystemText = new AdvPropertyGridLocalization();
            _SystemText.PropertyChanged += new PropertyChangedEventHandler(SystemTextPropertyChanged);

            _Appearance = new AdvPropertyGridAppearance();
            _Appearance.PropertyChanged += new PropertyChangedEventHandler(AppearancePropertyChanged);

            _PropertySettings = new PropertySettingsCollection(this);

            _Toolbar = CreateToolbar();
            _PropertyTree = CreatePropertyTree();
            _SuperTooltip = CreateSuperTooltip();
            _ExpandableSplitter = CreateExpandableSplitter();
            _HelpPanel = CreateHelpPanel();
            _ExpandableSplitter.ExpandableControl = _HelpPanel;

            this.Controls.Add(_PropertyTree);
            this.Controls.Add(_Toolbar);
            this.Controls.Add(_ExpandableSplitter);
            this.Controls.Add(_HelpPanel);
            _ExpandableSplitter.Visible = false;
            _HelpPanel.Visible = false;
        }

        internal bool HasValidatePropertyValueHandlers
        {
            get
            {
                return (ValidatePropertyValue != null);
            }
        }
        internal void InvokeValidatePropertyValue(ValidatePropertyValueEventArgs args)
        {
            ValidatePropertyValueEventHandler h = ValidatePropertyValue;
            if (h != null) h(this, args);
        }

        internal bool InvokePropertyValueChanging(string propertyName, object newValue)
        {
            PropertyValueChangingEventArgs e = new PropertyValueChangingEventArgs(propertyName, newValue);
            OnPropertyValueChanging(e);
            return e.Handled;
        }
        /// <summary>
        /// Invokes PropertyValueChanging event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnPropertyValueChanging(PropertyValueChangingEventArgs e)
        {
            PropertyValueChangingEventHandler handler = PropertyValueChanging;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// Invokes PropertyValueChanged event handler.
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed.</param>
        /// <param name="newValue">New property value.</param>
        /// <param name="oldValue">Old property value.</param>
        public void InvokePropertyValueChanged(string propertyName, object newValue, object oldValue)
        {
            OnPropertyValueChanged(propertyName);
        }
        /// <summary>
        /// Invokes PropertyValueChanged event.
        /// </summary>
        /// <param name="propertyName">Name of property that has changed</param>
        protected virtual void OnPropertyValueChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyValueChanged;
            if(handler!=null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SystemTextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CategorizeToolbarTooltip")
                _CategoryToolbarButton.Tooltip = _SystemText.CategorizeToolbarTooltip;
            else if (e.PropertyName == "AlphabeticalToolbarTooltip")
                _SortToolbarButton.Tooltip = _SystemText.AlphabeticalToolbarTooltip;
            else if (e.PropertyName=="SearchBoxWatermarkText")
            	_SearchTextBox.WatermarkText = _SystemText.SearchBoxWatermarkText;
        }

        private ExpandableSplitter CreateExpandableSplitter()
        {
            ExpandableSplitter splitter = new ExpandableSplitter();
            splitter.Style = eSplitterStyle.Office2007;
            splitter.Dock = DockStyle.Bottom;
            splitter.Height = 6;
            return splitter;
        }
        private PanelControl CreateHelpPanel()
        {
            PanelControl panel = new PanelControl();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 42;
            panel.ApplyPanelStyle(eDotNetBarStyle.Office2007);
            panel.Style.BackColor = Color.White;
            panel.Style.BackColor2 = Color.Empty;
            panel.Style.TextColor = Color.Black;
            panel.Style.WordWrap = true;
            panel.Style.TextAlignment = eStyleTextAlignment.Near;
            return panel;
        }

        private SuperTooltip CreateSuperTooltip()
        {
            SuperTooltip tooltip = new SuperTooltip();
#if (!TRIAL)
            tooltip.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
            tooltip.PositionBelowControl = false;
            tooltip.ShowTooltipImmediately = false;
            return tooltip;
        }

        private ButtonItem _CategoryToolbarButton = null;
        private ButtonItem _SortToolbarButton = null;
        private Bar CreateToolbar()
        {
            Bar bar = new Bar();
            bar.Dock = DockStyle.Top;
            bar.RoundCorners = false;

            ButtonItem item = new ButtonItem("sys_Category");
            item.Image = BarFunctions.LoadBitmap("SystemImages.Category.png");
            item.OptionGroup = "sorting";
            item.Checked = true;
            item.OptionGroupChanging += new OptionGroupChangingEventHandler(SortToolbarButtonOptionGroupChanging);
            item.Tooltip = _SystemText.CategorizeToolbarTooltip;
            bar.Items.Add(item);
            _CategoryToolbarButton = item;

            item = new ButtonItem("sys_Sort");
            item.Image = BarFunctions.LoadBitmap("SystemImages.Sort.png");
            item.OptionGroup = "sorting";
            item.OptionGroupChanging += new OptionGroupChangingEventHandler(SortToolbarButtonOptionGroupChanging);
            item.Tooltip = _SystemText.AlphabeticalToolbarTooltip;
            bar.Items.Add(item);
            _SortToolbarButton = item;

            TextBoxItem textBox = new TextBoxItem();
            textBox.WatermarkEnabled = true;
            textBox.WatermarkText = _SystemText.SearchBoxWatermarkText;
            textBox.WatermarkBehavior = eWatermarkBehavior.HideNonEmpty;
            textBox.TextBoxWidth = 90;
            textBox.BeginGroup = true;
            textBox.InputTextChanged += new DevComponents.DotNetBar.TextBoxItem.TextChangedEventHandler(SearchTextChanged);
            textBox.TextBox.GotFocus += new EventHandler(SearchTextBoxGotFocus);
            textBox.Enabled = false;
            _SearchTextBox = textBox;
            bar.Items.Add(textBox);

            bar.Style = eDotNetBarStyle.Office2007;

            return bar;
        }

        private void SearchTextBoxGotFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_SearchTextBox.ControlText))
                _SearchTextBox.TextBox.SelectAll();
        }

        private bool _IsFiltered = false;
        private List<Node> _HiddenNodes = new List<Node>();
        private void SearchTextChanged(object sender)
        {
            string searchText = _SearchTextBox.ControlText.ToLower();
            if (string.IsNullOrEmpty(searchText) && !_IsFiltered) return;

            if (string.IsNullOrEmpty(searchText))
            {
                _PropertyTree.BeginUpdate();
                foreach (Node item in _HiddenNodes)
                {
                    item.Visible = true;
                }
                _PropertyTree.EndUpdate();
                _IsFiltered = false;
                _HiddenNodes.Clear();
                return;
            }

            _PropertyTree.BeginUpdate();
            foreach (Node item in _HiddenNodes)
            {
                item.Visible = true;
            }
            FilterNodes(_PropertyTree.Nodes, _HiddenNodes, searchText);
            _IsFiltered = _HiddenNodes.Count > 0;
            _PropertyTree.EndUpdate();
        }

        private void FilterNodes(NodeCollection nodeCollection, List<Node> hiddenNodes, string searchText)
        {
            searchText = searchText.ToUpper();
            foreach (Node item in nodeCollection)
            {
                PropertyNode propertyNode = item as PropertyNode;
                if (propertyNode != null && !propertyNode.Text.ToUpper().Contains(searchText))
                {
                    propertyNode.Visible = false;
                    hiddenNodes.Add(propertyNode);
                }
                if (item.HasChildNodes)
                    FilterNodes(item.Nodes, hiddenNodes, searchText);
            }
        }

        private void SortToolbarButtonOptionGroupChanging(object sender, OptionGroupChangingEventArgs e)
        {
            if (e.NewChecked == _CategoryToolbarButton)
            {
                if (PropertySort != ePropertySort.CategorizedAlphabetical)
                    PropertySort = ePropertySort.CategorizedAlphabetical;
            }
            else if (e.NewChecked == _SortToolbarButton)
            {
                if (PropertySort != ePropertySort.Alphabetical)
                    PropertySort = ePropertySort.Alphabetical;
            }
        }

        private AdvTree.AdvTree CreatePropertyTree()
        {

            AdvTree.AdvTree tree = new DevComponents.AdvTree.AdvTree();
#if (!TRIAL)
            tree.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
            // Default node style
            ElementStyle elementStyle1 = new ElementStyle();
            elementStyle1.Name = STR_DefaultNodeStyle;
            elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            elementStyle1.TextTrimming = eStyleTextTrimming.None;
            elementStyle1.TextLineAlignment = eStyleTextAlignment.Center;
            tree.Styles.Add(elementStyle1);
            tree.NodeStyle = elementStyle1;

            _ReadOnlyStyle = new ElementStyle();
            _ReadOnlyStyle.Name = "ReadOnlyStyle";
            _ReadOnlyStyle.TextColor = SystemColors.ControlDark;
            _ReadOnlyStyle.TextTrimming = eStyleTextTrimming.None;
            tree.Styles.Add(_ReadOnlyStyle);

            // Category style
            _CategoryStyle = new ElementStyle();
            _CategoryStyle.Name = "CategoryStyle";
            _CategoryStyle.TextColor = System.Drawing.SystemColors.ControlText;
            _CategoryStyle.BackColorSchemePart = eColorSchemePart.PanelBackground;
            _CategoryStyle.Font = new Font(this.Font, FontStyle.Bold);
            tree.Styles.Add(_CategoryStyle);

            // Value Changed Style
            _ValueChangedStyle = new ElementStyle();
            _ValueChangedStyle.Name = "ValueChangedStyle";
            _ValueChangedStyle.Font = new Font(this.Font, FontStyle.Bold);
            tree.Styles.Add(_ValueChangedStyle);

            tree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            tree.AllowDrop = false;
            tree.DragDropEnabled = false;
            tree.BackColor = System.Drawing.SystemColors.Window;
            tree.BackgroundStyle.Class = "TreeBorderKey";
            tree.Name = "propertyTree";
            tree.PathSeparator = ";";
            tree.Size = new System.Drawing.Size(150, 100);
            tree.TabIndex = 0;
            tree.ExpandWidth = 12;
            tree.ExpandButtonType = eExpandButtonType.Triangle;
            tree.Indent = 0;
            tree.GridRowLines = true;
            tree.GridColumnLines = true;
            tree.GridColumnLineResizeEnabled = true;
            tree.GridLinesColor = Color.WhiteSmoke;
            tree.HScrollBarVisible = false;
            tree.SelectionPerCell = true;
            tree.SelectionBoxStyle = eSelectionStyle.FullRowSelect;
            tree.SelectionFocusAware = false;
            tree.CellHorizontalSpacing = 2;
            tree.BeforeNodeSelect += new AdvTreeNodeCancelEventHandler(PropertyTreeBeforeNodeSelect);
            tree.KeyboardSearchEnabled = false;
            tree.KeyPress += new KeyPressEventHandler(PropertyTreeKeyPress);
            tree.ColumnResizing += new EventHandler(PropertyTreeColumnResized);
            tree.DoubleClick += new EventHandler(PropertyTreeDoubleClick);
            tree.Leave += new EventHandler(PropertyTreeLeave);

            AdvTree.ColumnHeader header = new DevComponents.AdvTree.ColumnHeader();
            header.Name = "propertyName";
            header.Width.Relative = 50;
            tree.Columns.Add(header);

            header = new DevComponents.AdvTree.ColumnHeader();
            header.Name = "propertyValue";
            header.Width.Relative = 50;
            tree.Columns.Add(header);

            tree.ColumnsVisible = false;

            tree.Dock = DockStyle.Fill;
            return tree;
        }

        private void PropertyTreeLeave(object sender, EventArgs e)
        {
            if (_EditedNode != null)
                _EditedNode.ApplyEdit();
        }

        private PropertyNode _EditedNode = null;
        internal void OnPropertyIsEditingChanged(PropertyNode node)
        {
            if (node.IsEditing)
                _EditedNode = node;
            else
                _EditedNode = null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ClearPropertyTree();
            DisposeStyleFont(_CategoryStyle);
            DisposeStyleFont(_ValueChangedStyle);
        }
        private void DisposeStyleFont(ElementStyle style)
        {
            if (style == null) return;
            if (style.Font != null)
            {
                style.Font.Dispose();
                style.Font = null;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when property grid needs property names translated for UI.
        /// </summary>
        [Description("Occurs when property grid needs property names translated for UI.")]
        public event LocalizeEventHandler Localize;
        /// <summary>
        /// Occurs when Super Tooltip that is displayed for the error that occurred when property is being set is being assigned to a property node.
        /// This event allows you to customize the super tooltip displayed.
        /// </summary>
        [Description("Occurs when Super Tooltip that is displayed for the error that occurred when property is being set is being assigned to a property node.")]
        public event PropertyErrorTooltipEventHandler PrepareErrorSuperTooltip;
        /// <summary>
        /// Occurs when property value is being converted to text representation for display. You can handle this event
        /// to provide custom conversion for property values. You must set IsConverted=true on event arguments to indicate that you have performed value conversion.
        /// </summary>
        [Description("Occurs when property value is being converted to text representation for display.")]
        public event ConvertValueEventHandler ConvertPropertyValueToString;
        /// <summary>
        /// Occurs when text entered by user is being converted to typed property value to be assigned to the property. You can handle this event
        /// to provide custom conversion for property values. You must set IsConverted=true on event arguments to indicate that you have performed value conversion.
        /// </summary>
        [Description("Occurs when text entered by user is being converted to typed property value to be assigned to the property.")]
        public event ConvertValueEventHandler ConvertFromStringToPropertyValue;
        /// <summary>
        /// Occurs when property looks for the list of valid values for the property to show on either drop-down or use
        /// in auto-complete list. For example when property type is enum the internal implementation will convert available enum
        /// values into string list and use on drop-down and in auto-complete list. You can use this event to provide
        /// custom value list in combination with ConvertPropertyValueToString, ConvertFromStringToPropertyValue events.
        /// You must set IsListValid=true on event arguments to indicate that list your provided should be used.
        /// </summary>
        [Description("Occurs when property looks for the list of valid values for the property to show on either drop-down or use in auto-complete list.")]
        public event PropertyValueListEventHandler ProvidePropertyValueList;
        /// <summary>
        /// Occurs when property node needs the UITypeEditor for the property. You can handle this event and provide your own UITypeEditor to be used instead of the one specified on the property.
        /// </summary>
        [Description("Occurs when property node needs the UITypeEditor for the property. You can handle this event and provide your own UITypeEditor to be used instead of the one specified on the property.")]
        public event ProvideUITypeEditorEventHandler ProvideUITypeEditor;
        /// <summary>
        /// Occurs when user changes the value of the property in property grid.
        /// </summary>
        [Description("Occurs when user changes the value of the property in property grid.")]
        public event PropertyChangedEventHandler PropertyValueChanged;

        /// <summary>
        /// Occurs before property value entered by the user is set on the property. You can cancel internal assignment by property grid by setting Handled=true on event arguments.
        /// </summary>
        [Description("Occurs before property value entered by the user is set on the property. You can cancel internal assignment by property grid by setting Handled=true on event arguments.")]
        public event PropertyValueChangingEventHandler PropertyValueChanging;

        /// <summary>
        /// Occurs when users changes the property value and attempts to commit the changes. This even allows you to validate the value and show error message if value is invalid and cancel its application.
        /// </summary>
        [Description("Occurs when users changes the property value and attempts to commit the changes. This even allows you to validate the value and show error message if value is invalid and cancel its application.")]
        public event ValidatePropertyValueEventHandler ValidatePropertyValue;
        /// <summary>
        /// Raises ValidatePropertyValue event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValidatePropertyValue(ValidatePropertyValueEventArgs e)
        {
            ValidatePropertyValueEventHandler handler = ValidatePropertyValue;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Commits any property edits that are currently in progress by applying the current entered value to the property.
        /// Returns true if edit was applied.
        /// </summary>
        public bool CommitEdit()
        {
            PropertyNode node = _PropertyTree.SelectedNode as PropertyNode;
            if (node != null && node.IsEditing)
                return node.ApplyEdit();
            return false;
        }


        private object[] _SelectedObjects;
        /// <summary>
        /// Gets or sets the currently selected objects.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public object[] SelectedObjects
        {
            get { return _SelectedObjects; }
            set
            {
                if (value != _SelectedObjects)
                {
                    object[] oldValue = _SelectedObjects;
                    _SelectedObjects = value;
                    OnSelectedObjectsChanged(oldValue, value);
                }
            }
        }

        private void OnSelectedObjectsChanged(object[] oldValue, object[] newValue)
        {
            if (oldValue != null)
            {
                if (oldValue.Length == 1 && oldValue[0] is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)oldValue[0]).PropertyChanged -= SelectedObjectPropertyChanged;
                }
            }

            OnPropertyChanged(new PropertyChangedEventArgs("SelectedObjects"));

            Reload(true);

            if (newValue != null && newValue.Length > 0)
            {
                if (newValue.Length == 1 && newValue[0] is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)_SelectedObjects[0]).PropertyChanged += SelectedObjectPropertyChanged;
                }
            }
        }

        private void SelectedObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePropertyValue(e.PropertyName);
        }

        private void UpdatePropertyValue(Node item, string propertyName)
        {
            PropertyNode node = item as PropertyNode;
            if (node != null && node.PropertyDescriptor != null && node.PropertyDescriptor.Name == propertyName)
            {
                node.UpdateDisplayedValue();
            }

            foreach (Node child in item.Nodes)
            {
                UpdatePropertyValue(child, propertyName);
            }
        }

        /// <summary>
        /// Updates specified property value in property grid.
        /// </summary>
        /// <param name="propertyName">Property Name to update value for.</param>
        public void UpdatePropertyValue(string propertyName)
        {
            foreach (Node item in _PropertyTree.Nodes)
            {
                UpdatePropertyValue(item, propertyName);
            }
        }

        private void ClearPropertyTree()
        {
            _PropertyTree.BeginUpdate();
            foreach (Node node in _PropertyTree.Nodes)
            {
                ClearPropertyNode(node, this);
            }
            _PropertyTree.Nodes.Clear();
            _PropertyTree.EndUpdate(false);
            //object[] keys = new object[_SuperTooltip.SuperTooltipInfoTable.Count];
            //_SuperTooltip.SuperTooltipInfoTable.Keys.CopyTo(keys, 0);
            //foreach (object key in keys)
            //{
            //    if(key is IComponent)
            //        _SuperTooltip.SetSuperTooltip((IComponent)key, null);
            //}
        }

        internal static void ClearPropertyNode(Node node, AdvPropertyGrid propertyGrid)
        {
            node.Style = null;
            PropertyNode propNode = node as PropertyNode;
            if (propNode != null)
            {
                propNode.IsDisposing = true;
                if (propNode.IsEditing)
                {
                    propNode.CancelEdit();
                    propNode.ExitEditorMode(eTreeAction.Code);
                }
                propNode.PropertySettings = null;
                propNode.TargetComponent = null;
                propNode.PropertyDescriptor = null;
                propertyGrid.SuperTooltip.SetSuperTooltip(node, null);
            }
            if (node.HasChildNodes)
            {
                foreach (Node item in node.Nodes)
                {
                    ClearPropertyNode(item, propertyGrid);
                }
            }
            node.Dispose();
            if (propNode != null) propNode.IsDisposing = false;
        }

        private PropertyParser GetPropertyParser(object selectedObject)
        {
            if (selectedObject == null) throw new ArgumentNullException("selectedObject argument cannot be null");

            Attribute[] attributes = new Attribute[this.BrowsableAttributes.Count];
            this.BrowsableAttributes.CopyTo(attributes, 0);
            PropertyParser parser = new PropertyParser(selectedObject,
                                        attributes,
                                        GetPropertyNodeFactory(),
                                        new List<string>(_IgnoredProperties),
                                        new List<string>(_IgnoredCategories),
                                        this,
                                        _PropertySettings);
            parser.HelpType = _HelpType;

            return parser;
        }
        private PropertyParser GetPropertyParser(object[] selectedObjects)
        {
            if (selectedObjects == null) throw new ArgumentNullException("selectedObject argument cannot be null");

            Attribute[] attributes = new Attribute[this.BrowsableAttributes.Count];
            this.BrowsableAttributes.CopyTo(attributes, 0);
            PropertyParser parser = new PropertyParser(selectedObjects,
                                        attributes,
                                        GetPropertyNodeFactory(),
                                        new List<string>(_IgnoredProperties),
                                        new List<string>(_IgnoredCategories),
                                        this,
                                        _PropertySettings);
            parser.HelpType = _HelpType;
            return parser;
        }

        internal PropertyNodeFactory GetPropertyNodeFactory()
        {
            return new PropertyNodeFactory((IPropertyElementStyleProvider)this);
        }
        /// <summary>
        /// Gets or sets the currently selected object.
        /// </summary>
        [DefaultValue(null), Browsable(false)]
        public object SelectedObject
        {
            get
            {
                if (_SelectedObjects == null) return null;
                return _SelectedObjects[0];
            }
            set
            {
                if (value != null)
                {
                    SelectedObjects = new object[] { value };
                }
                else
                    SelectedObjects = null;
            }
        }

        private AttributeCollection _BrowsableAttributes = null;
        /// <summary>
        /// Gets or sets the browsable attributes associated with the object that the property grid is attached to.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Advanced)]
        public AttributeCollection BrowsableAttributes
        {
            get
            {
                if (_BrowsableAttributes == null)
                {
                    _BrowsableAttributes = new AttributeCollection(new Attribute[] { new BrowsableAttribute(true) });
                }
                return _BrowsableAttributes;
            }
            set
            {
                if ((value == null) || (value == AttributeCollection.Empty))
                {
                    _BrowsableAttributes = new AttributeCollection(new Attribute[] { BrowsableAttribute.Yes });
                }
                else
                {
                    Attribute[] array = new Attribute[value.Count];
                    value.CopyTo(array, 0);
                    _BrowsableAttributes = new AttributeCollection(array);
                }
                if (_SelectedObjects != null && _SelectedObjects.Length > 0)
                    this.Reload(false);

            }
        }

        /// <summary>
        /// Refreshes the display of all property values in the grid.
        /// </summary>
        public void RefreshPropertyValues()
        {
            foreach (Node item in _PropertyTree.Nodes)
            {
                RefreshPropertyValues(item);
            }
        }

        internal void RefreshPropertyValues(Node parentNode)
        {
            if (parentNode is PropertyNode)
                ((PropertyNode)parentNode).UpdateDisplayedValue();
            foreach (Node item in parentNode.Nodes)
            {
                RefreshPropertyValues(item);
            }
        }

        /// <summary>
        /// Reloads all properties from selected object.
        /// </summary>
        public void RefreshProperties()
        {
            Reload(false);
        }
        /// <summary>
        /// Collapses all the categories in the AdvPropertyGrid.
        /// </summary>
        public void CollapseAllGridItems()
        {
            if (_PropertyTree == null || _PropertyTree.Nodes.Count == 0) return;
            _PropertyTree.CollapseAll();
        }
        /// <summary>
        /// Expands all the categories in the AdvPropertyGrid.
        /// </summary>
        public void ExpandAllGridItems()
        {
            if (_PropertyTree == null || _PropertyTree.Nodes.Count == 0) return;
            _PropertyTree.ExpandAll();
        }

        private bool _ReloadPending = false;
        private void Reload(bool selectedObjectChanged)
        {
            if (_IsInitializing)
            {
                _ReloadPending = true;
                return;
            }
            _ReloadPending = false;

            string lastSelectedProperty = null;
            if (!selectedObjectChanged && _PropertyTree.SelectedNode is PropertyNode)
            {
                PropertyNode propNode = (PropertyNode)_PropertyTree.SelectedNode;
                lastSelectedProperty = propNode.Text;
            }

            ClearPropertyTree();
            _HelpPanel.Text = "";

            if (_SelectedObjects != null && _SelectedObjects.Length > 0)
            {
                PropertyParser parser = null;
                if (_SelectedObjects.Length == 1)
                    parser = GetPropertyParser(_SelectedObjects[0]);
                else
                    parser = GetPropertyParser(_SelectedObjects);

                _PropertyTree.BeginUpdate();
                try
                {
                    parser.Parse(_PropertyTree.Nodes, _PropertySort, _SuperTooltip);
                    if (!string.IsNullOrEmpty(lastSelectedProperty))
                    {
                        Node node = _PropertyTree.FindNodeByText(lastSelectedProperty);
                        if (node != null)
                            _PropertyTree.SelectedNode = node;
                    }
                }
                finally
                {
                    _PropertyTree.EndUpdate();
                }
                _SearchTextBox.Enabled = true;
            }
            else
                _SearchTextBox.Enabled = false;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            if (_CategoryStyle != null)
            {
                DisposeStyleFont(_CategoryStyle);
                _CategoryStyle.Font = new Font(this.Font, FontStyle.Bold);

                DisposeStyleFont(_ValueChangedStyle);
                _ValueChangedStyle.Font = new Font(this.Font, FontStyle.Bold);
            }
            base.OnFontChanged(e);
        }

        void PropertyTreeBeforeNodeSelect(object sender, AdvTreeNodeCancelEventArgs e)
        {
            PropertyNode propertyNode = _PropertyTree.SelectedNode as PropertyNode;
            if (propertyNode == null) return;

            if (propertyNode.IsEditing)
            {
                if (!propertyNode.ApplyEdit())
                    e.Cancel = true;
            }
        }

        private static bool _HighlightPropertyOnUpdate = true;
        /// <summary>
        /// Gets or sets whether property is highlighted to confirm the value update by user. Default value is true.
        /// </summary>
        public static bool HighlightPropertyOnUpdate
        {
            get { return _HighlightPropertyOnUpdate; }
            set
            {
                _HighlightPropertyOnUpdate = value;
            }
        }

        private string[] _IgnoredProperties = new string[0];
        /// <summary>
        /// Gets or sets the list of property names that are not loaded into property grid regardless of their Browsable attribute setting.
        /// </summary>
        [Description("List of property names that are not loaded into property grid regardless of their Browsable attribute setting."), Category("Behavior")]
        public string[] IgnoredProperties
        {
            get { return _IgnoredProperties; }
            set
            {
                if (value == null) value = new string[0];
                _IgnoredProperties = value;
                Reload(false);
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIgnoredProperties()
        {
            return _IgnoredProperties != null && _IgnoredProperties.Length > 0;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetIgnoredProperties()
        {
            IgnoredProperties = new string[0];
            List<string> strings = new List<string>();
        }

        private string[] _IgnoredCategories = new string[0];
        /// <summary>
        /// Gets or sets the list of category names properties below to that are not loaded into property grid regardless of the property Browsable attribute setting.
        /// </summary>
        [Description("List of category names properties below to that are not loaded into property grid regardless of the property Browsable attribute setting."), Category("Behavior")]
        public string[] IgnoredCategories
        {
            get { return _IgnoredCategories; }
            set
            {
                if (value == null) value = new string[0];
                _IgnoredCategories = value;
                Reload(false);
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIgnoredCategories()
        {
            return _IgnoredCategories != null && _IgnoredCategories.Length > 0;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetIgnoredCategories()
        {
            IgnoredCategories = new string[0];
            List<string> strings = new List<string>();
        }

        internal SuperTooltip SuperTooltip
        {
            get { return _SuperTooltip; }
            set { _SuperTooltip = value; }
        }

        internal void InvokePrepareErrorSuperTooltip(SuperTooltipInfo info, string propertyName, Exception exception, object value)
        {
            OnPrepareErrorSuperTooltip(new PropertyErrorTooltipEventArgs(info, propertyName, exception, value));
        }
        /// <summary>
        /// Raises the PrepareErrorSuperTooltip event.
        /// </summary>
        /// <param name="e">Provides information about event.</param>
        protected virtual void OnPrepareErrorSuperTooltip(PropertyErrorTooltipEventArgs e)
        {
            PropertyErrorTooltipEventHandler handler = PrepareErrorSuperTooltip;
            if (handler != null) handler(this, e);
        }

        private void PropertyTreeKeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) && _PropertyTree.SelectedNode is PropertyNode)
            {
                PropertyNode node = (PropertyNode)_PropertyTree.SelectedNode;
                TextBoxDropDown editor = node.GetEditor();
                if (editor != null && !editor.TextBox.Focused && !editor.TextBox.ReadOnly)
                {
                    if (editor.TextBox.Focus())
                    {
                        editor.Text = e.KeyChar.ToString();
                        editor.TextBox.SelectionStart = 1;
                    }
                }
            }
            else if (char.IsWhiteSpace(e.KeyChar) && _PropertyTree.SelectedNode is PropertyCheckBoxNode)
            {
                PropertyCheckBoxNode node = (PropertyCheckBoxNode)_PropertyTree.SelectedNode;
                if (!node.IsReadOnly)
                    node.SelectNextValue();
            }
        }

        private void PropertyTreeColumnResized(object sender, EventArgs e)
        {
            if (_PropertyTree.Columns[0].Width.Absolute != 0 || _PropertyTree.Columns[1].Width.Absolute != 0)
            {
                int column1 = 0;
                if (_PropertyTree.Columns[0].Width.Absolute > 0)
                    column1 = Math.Min(95, Math.Max(5, (int)Math.Floor(100 * (_PropertyTree.Columns[0].Width.Absolute / (float)(_PropertyTree.NodeLayout.ClientArea.Width-0)))));
                else
                    column1 = _PropertyTree.Columns[0].Width.Relative;
                int column2 = 100 - column1;
                _PropertyTree.Columns[0].Width.Absolute = 0;
                _PropertyTree.Columns[1].Width.Absolute = 0;
                _PropertyTree.Columns[0].Width.Relative = column1;
                _PropertyTree.Columns[1].Width.Relative = column2;
            }
        }

        private void PropertyTreeDoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs mouseArgs = e as MouseEventArgs;
            if (mouseArgs == null) return;
            if (_PropertyTree.SelectedNode != null && _PropertyTree.SelectedNode.Bounds.Contains(mouseArgs.X, mouseArgs.Y)) return;

            Node node = _PropertyTree.GetNodeAt(mouseArgs.Y);
            if (node is PropertyCategoryNode)
                node.Toggle();
        }

        private AdvPropertyGridAppearance _Appearance = null;
        /// <summary>
        /// Defines the appearance of the control.
        /// </summary>
        [Category("Appearance"), Description("Defines appearance of the control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AdvPropertyGridAppearance Appearance
        {
            get { return _Appearance; }
        }
        private void AppearancePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DefaultPropertyStyle")
            {
                if (_Appearance.DefaultPropertyStyle != null)
                    _PropertyTree.NodeStyle = _Appearance.DefaultPropertyStyle;
                else
                    _PropertyTree.NodeStyle = _PropertyTree.Styles[STR_DefaultNodeStyle];
            }
        }
        /// <summary>
        /// Raises the ConvertPropertyValueToString event.
        /// </summary>
        /// <param name="e">Provides event data</param>
        protected virtual void OnConvertPropertyValueToString(ConvertValueEventArgs e)
        {
            ConvertValueEventHandler handler = ConvertPropertyValueToString;
            if (handler != null)
                handler(this, e);
        }
        internal void InvokeConvertPropertyValueToString(ConvertValueEventArgs e)
        {
            OnConvertPropertyValueToString(e);
        }
        internal bool HasConvertPropertyValueToStringHandler
        {
            get
            {
                return ConvertPropertyValueToString != null;
            }
        }

        /// <summary>
        /// Raises the ConvertFromStringToPropertyValue event.
        /// </summary>
        /// <param name="e">Provides event data</param>
        protected virtual void OnConvertFromStringToPropertyValue(ConvertValueEventArgs e)
        {
            ConvertValueEventHandler handler = ConvertFromStringToPropertyValue;
            if (handler != null)
                handler(this, e);
        }
        internal void InvokeConvertFromStringToPropertyValue(ConvertValueEventArgs e)
        {
            OnConvertFromStringToPropertyValue(e);
        }
        internal bool HasConvertFromStringToPropertyValueHandler
        {
            get
            {
                return ConvertFromStringToPropertyValue != null;
            }
        }

        /// <summary>
        /// Raises the ProvidePropertyValueList event.
        /// </summary>
        /// <param name="e">Provides event data</param>
        protected virtual void OnProvidePropertyValueList(PropertyValueListEventArgs e)
        {
            PropertyValueListEventHandler handler = ProvidePropertyValueList;
            if (handler != null)
                handler(this, e);
        }
        internal void InvokeProvidePropertyValueList(PropertyValueListEventArgs e)
        {
            OnProvidePropertyValueList(e);
        }
        internal bool HasProvidePropertyValueListHandler
        {
            get
            {
                return ProvidePropertyValueList != null;
            }
        }

        /// <summary>
        /// Raises the ProvideUITypeEditor event.
        /// </summary>
        /// <param name="e">Provides event data</param>
        protected virtual void OnProvideUITypeEditor(ProvideUITypeEditorEventArgs e)
        {
            ProvideUITypeEditorEventHandler handler = ProvideUITypeEditor;
            if (handler != null)
                handler(this, e);
        }
        internal void InvokeProvideUITypeEditor(ProvideUITypeEditorEventArgs e)
        {
            OnProvideUITypeEditor(e);
        }
        internal bool HasProvideUITypeEditorHandler
        {
            get
            {
                return ProvideUITypeEditor != null;
            }
        }

        internal void PropertySettingsItemAdded(PropertySettings item)
        {
            item.PropertyChanged += new PropertyChangedEventHandler(PropertySettingChanged);
        }

        private void PropertySettingChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        internal void PropertySettingsItemRemoved(PropertySettings item)
        {
            item.PropertyChanged -= new PropertyChangedEventHandler(PropertySettingChanged);
        }

        private PropertySettingsCollection _PropertySettings = null;
        public PropertySettingsCollection PropertySettings
        {
            get
            {
                return _PropertySettings;
            }
        }


        private ePropertySort _PropertySort = ePropertySort.CategorizedAlphabetical;
        /// <summary>
        /// Gets or sets the property sorting inside of the grid.
        /// </summary>
        [DefaultValue(ePropertySort.CategorizedAlphabetical), Category("Appearance"), Description("Indicates property sorting inside of the grid.")]
        public ePropertySort PropertySort
        {
            get { return _PropertySort; }
            set
            {
                if (value != _PropertySort)
                {
                    ePropertySort oldValue = _PropertySort;
                    _PropertySort = value;
                    OnPropertySortChanged(oldValue, value);
                }
            }
        }
        private void OnPropertySortChanged(ePropertySort oldValue, ePropertySort newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("PropertySort"));
            if (newValue == ePropertySort.Alphabetical)
                _SortToolbarButton.Checked = true;
            else if (newValue != ePropertySort.NoSort)
                _CategoryToolbarButton.Checked = true;
            else
            {
                _SortToolbarButton.Checked = false;
                _CategoryToolbarButton.Checked = false;
            }
            Reload(false);
        }

        private ePropertyGridHelpType _HelpType = ePropertyGridHelpType.SuperTooltip;
        /// <summary>
        /// Gets or sets the help type that is provided by the control. Default help type is SuperTooltip which shows tooltip over each property
        /// that provides property description.
        /// </summary>
        [DefaultValue(ePropertyGridHelpType.SuperTooltip), Category("Appearance"), Description("Indicates help type that is provided by the control.")]
        public ePropertyGridHelpType HelpType
        {
            get { return _HelpType; }
            set
            {
                if (value != _HelpType)
                {
                    ePropertyGridHelpType oldValue = _HelpType;
                    _HelpType = value;
                    OnHelpTypeChanged(oldValue, value);
                }
            }
        }

        private void OnHelpTypeChanged(ePropertyGridHelpType oldValue, ePropertyGridHelpType newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("HelpType"));
            if (newValue != ePropertyGridHelpType.Panel)
            {
                _ExpandableSplitter.Visible = false;
                _HelpPanel.Visible = false;
            }
            else
            {
                _ExpandableSplitter.Visible = true;
                _HelpPanel.Visible = true;
            }

            if (newValue == ePropertyGridHelpType.SuperTooltip || oldValue == ePropertyGridHelpType.SuperTooltip)
            {
                Reload(false);
            }
        }

        /// <summary>
        /// Gets reference to the help panel that is displayed below property grid and which provides selected property description.
        /// </summary>
        [Browsable(false)]
        public PanelControl HelpPanel
        {
            get
            {
                return _HelpPanel;
            }
        }
        /// <summary>
        /// Gets reference to the expandable splitter that is displayed above the help panel and allows resizing of the panel.
        /// </summary>
        [Browsable(false)]
        public ExpandableSplitter HelpExpandableSplitter
        {
            get
            {
                return _ExpandableSplitter; ;
            }
        }

        /// <summary>
        /// Gets reference to internal AdvTree control that displays properties.
        /// </summary>
        [Browsable(false)]
        public AdvTree.AdvTree PropertyTree
        {
            get { return _PropertyTree; }
        }

        /// <summary>
        /// Gets or sets the grid lines color.
        /// </summary>
        [Category("Appearance"), Description("Indicates grid lines color.")]
        public Color GridLinesColor
        {
            get { return _PropertyTree.GridLinesColor; }
            set { _PropertyTree.GridLinesColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeGridLinesColor()
        {
            return _PropertyTree.ShouldSerializeGridLinesColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetGridLinesColor()
        {
            _PropertyTree.ResetGridLinesColor();
        }

        /// <summary>
        /// Gets the reference to the internal toolbar control displayed above the property grid.
        /// </summary>
        [Browsable(false)]
        public Bar Toolbar
        {
            get { return _Toolbar; }
        }

        private bool _ToolbarVisible = true;
        /// <summary>
        /// Gets or sets whether toolbar is visible. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether toolbar is visible.")]
        public bool ToolbarVisible
        {
            get { return _ToolbarVisible; }
            set
            {
                if (value != _ToolbarVisible)
                {
                    bool oldValue = _ToolbarVisible;
                    _ToolbarVisible = value;
                    OnToolbarVisibleChanged(oldValue, value);
                }
            }
        }

        private void OnToolbarVisibleChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ToolbarVisible"));
            if (_Toolbar != null) _Toolbar.Visible = newValue;
        }

        private bool _SearchBoxVisible = true;
        /// <summary>
        /// Gets or sets whether search text box that allows property filtering is visible. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether search text box that allows property filtering is visible")]
        public bool SearchBoxVisible
        {
            get { return _SearchBoxVisible; }
            set
            {
                if (value != _SearchBoxVisible)
                {
                    bool oldValue = _SearchBoxVisible;
                    _SearchBoxVisible = value;
                    OnSearchBoxVisibleChanged(oldValue, value);
                }
            }
        }
        private void OnSearchBoxVisibleChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("SearchBoxVisible"));
            _SearchTextBox.Visible = _SearchBoxVisible;
        }

        /// <summary>
        /// Gets reference to internal TextBoxItem that represents search text box.
        /// </summary>
        [Browsable(false)]
        public TextBoxItem SearchTextBoxItem
        {
            get { return _SearchTextBox; }
        }

        /// <summary>
        /// Gets the reference to the property gird localization object which holds all system text used by the component.
        /// </summary>
        [Browsable(true), NotifyParentPropertyAttribute(true), Category("Localization"), Description("Gets system text used by the component."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AdvPropertyGridLocalization SystemText
        {
            get { return _SystemText; }
        }


        private bool _TabKeyNavigation = true;
        /// <summary>
        /// Gets or sets whether Tab key navigates between property nodes. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether Tab key navigates between property nodes.")]
        public bool TabKeyNavigation
        {
            get { return _TabKeyNavigation; }
            set
            {
                _TabKeyNavigation = value;
            }
        }
        

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_TabKeyNavigation)
            {
                Keys keyPressed = (Keys)(keyData & ~Keys.Modifiers); // Must remove modifiers
                if (keyPressed == Keys.Tab || keyPressed == Keys.Tab && (keyData & Keys.Shift) == Keys.Shift)
                {
                    Node node = null;
                    if ((keyData & Keys.Shift) == Keys.Shift)
                        node = GetPreviousPropertyNode();
                    else
                        node = GetNextPropertyNode();
                    if (node != null)
                    {
                        bool processTab=true;
                        if (_PropertyTree.SelectedNode is PropertyNode)
                        {
                            PropertyNode propertyNode = (PropertyNode)_PropertyTree.SelectedNode;
                            if (propertyNode.IsEditing && !propertyNode.ApplyEdit())
                                processTab = false;
                        }

                        if (processTab)
                        {
                            _PropertyTree.SelectNode(node, eTreeAction.Keyboard);
                        }
                        return true;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private Node GetNextPropertyNode()
        {
            if (_PropertyTree.Nodes.Count == 0) return null;

            Node current = _PropertyTree.SelectedNode;
            if (current == null)
            {
                current = _PropertyTree.Nodes[0];
                if (current is PropertyNode && !((PropertyNode)current).IsReadOnly)
                    return current;
            }

            while (current != null)
            {
                current = NodeOperations.GetNextVisibleNode(current);
                if (current is PropertyNode && !((PropertyNode)current).IsReadOnly)
                    return current;
            }

            return null;
        }
        private Node GetPreviousPropertyNode()
        {
            if (_PropertyTree.Nodes.Count == 0) return null;

            Node current = _PropertyTree.SelectedNode;
            if (current == null)
            {
                current = _PropertyTree.Nodes[_PropertyTree.Nodes.Count - 1];
                if (current is PropertyNode && !((PropertyNode)current).IsReadOnly)
                    return current;
            }

            while (current != null)
            {
                current = NodeOperations.GetPreviousVisibleNode(current);
                if (current is PropertyNode && !((PropertyNode)current).IsReadOnly)
                    return current;
            }

            return null;
        }

        private static char _PasswordChar = '*';
        /// <summary>
        /// Gets or sets the password character used by the property values that are marked with PropertyPasswordText attribute. 
        /// </summary>
        public static char PasswordChar
        {
            get { return _PasswordChar; }
            set
            {
                _PasswordChar = value;
            }
        }
        
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when property defined by AdvPropertyGrid control has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
        #endregion

        #region IPropertyGridLocalizer Members

        string IPropertyGridLocalizer.GetPropertyName(string propertyName)
        {
            LocalizeEventHandler handler = Localize;
            if (handler != null)
            {
                AdvPropertyGridLocalizeEventArgs e = new AdvPropertyGridLocalizeEventArgs(propertyName, ePropertyGridLocalizationType.PropertyName);
                handler(this, e);
                return e.LocalizedValue;
            }
            return null;
        }

        string IPropertyGridLocalizer.GetCategoryName(string categoryName)
        {
            LocalizeEventHandler handler = Localize;
            if (handler != null)
            {
                AdvPropertyGridLocalizeEventArgs e = new AdvPropertyGridLocalizeEventArgs(categoryName, ePropertyGridLocalizationType.Category);
                handler(this, e);
                return e.LocalizedValue;
            }
            return null;

        }

        string IPropertyGridLocalizer.GetErrorTooltipMessage(string message)
        {
            LocalizeEventHandler handler = Localize;
            if (handler != null)
            {
                AdvPropertyGridLocalizeEventArgs e = new AdvPropertyGridLocalizeEventArgs("ErrorBody", ePropertyGridLocalizationType.ErrorTooltip);
                e.LocalizedValue = message;
                handler(this, e);
                return e.LocalizedValue;
            }
            return message;
        }
        #endregion

        #region IPropertyElementStyleProvider Members
        /// <summary>
        /// Gets the style that is applied to property node when it is in read-only state.
        /// </summary>
        ElementStyle IPropertyElementStyleProvider.ReadOnlyStyle
        {
            get
            {
                if (_Appearance.ReadOnlyPropertyStyle != null && _Appearance.ReadOnlyPropertyStyle.Custom)
                    return _Appearance.ReadOnlyPropertyStyle;
                return _ReadOnlyStyle;
            }
        }
        /// <summary>
        /// Gets the style that is applied to property node when its value has changed from the default value for the property.
        /// </summary>
        ElementStyle IPropertyElementStyleProvider.ValueChangedStyle
        {
            get
            {
                if (_Appearance.ValueChangedPropertyStyle != null && _Appearance.ValueChangedPropertyStyle.Custom)
                    return _Appearance.ValueChangedPropertyStyle;
                return _ValueChangedStyle;
            }
        }
        /// <summary>
        /// Gets the property category style.
        /// </summary>
        ElementStyle IPropertyElementStyleProvider.CategoryStyle
        {
            get
            {
                if (_Appearance.CategoryStyle != null && _Appearance.CategoryStyle.Custom)
                    return _Appearance.CategoryStyle;
                return _CategoryStyle;
            }
        }

        #endregion

        #region ISupportInitialize Members
        private bool _IsInitializing = false;
        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        public void BeginInit()
        {
            _PropertyTree.BeginUpdate();
            _IsInitializing = true;
        }
        /// <summary>
        /// Signals the object that initialization is ending.
        /// </summary>
        public void EndInit()
        {
            _PropertyTree.EndUpdate();
            _IsInitializing = false;
            if (_ReloadPending)
            {
                Reload(false);
            }
        }

        #endregion
    }

    #region ePropertySort
    /// <summary>
    /// Defines the sorting for the AdvPropertyGrid properties.
    /// </summary>
    public enum ePropertySort
    {
        NoSort,
        Alphabetical,
        Categorized,
        CategorizedAlphabetical
    }
    #endregion

    #region eHelpType
    /// <summary>
    /// Specifies help types property grid uses to display help for selected property.
    /// </summary>
    public enum ePropertyGridHelpType
    {
        /// <summary>
        /// No help is visible.
        /// </summary>
        HelpHidden,
        /// <summary>
        /// SuperTooltip with property description is displayed when mouse is over the property.
        /// </summary>
        SuperTooltip,
        /// <summary>
        /// Panel below property grid is displayed which shows the description for the selected property.
        /// </summary>
        Panel
    }
    #endregion

    #region Localizer
    /// <summary>
    /// Defines an interface that is used by advanced property grid parser to localize property names.
    /// </summary>
    public interface IPropertyGridLocalizer
    {
        /// <summary>
        /// Gets localized property name.
        /// </summary>
        /// <param name="propertyName">Property name to retrieve localized name for.</param>
        /// <returns>Localized Property name or null to use default.</returns>
        string GetPropertyName(string propertyName);
        /// <summary>
        /// Gets localized category name.
        /// </summary>
        /// <param name="categoryName">Category to retrieve localized value for.</param>
        /// <returns>Localized Category name or null to use default.</returns>
        string GetCategoryName(string categoryName);
        /// <summary>
        /// Gets localized message for Tooltip body when error setting the property value has occurred.
        /// </summary>
        /// <param name="message">Default system message.</param>
        /// <returns>Localized message or null to use default.</returns>
        string GetErrorTooltipMessage(string message);
    }
    /// <summary>
    /// Defines delegate for Localize AdvPropertyGrid event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LocalizeEventHandler(object sender, AdvPropertyGridLocalizeEventArgs e);
    /// <summary>
    /// Defines data for Localize AdvPropertyGrid event.
    /// </summary>
    public class AdvPropertyGridLocalizeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the property name or category name localization is performed for. Inspect LocalizationType property to get localization type which
        /// determines value specified in this property.
        /// </summary>
        public readonly string Key;
        /// <summary>
        /// Gets the localization type being performed.
        /// </summary>
        public readonly ePropertyGridLocalizationType LocalizationType;

        /// <summary>
        /// Gets or sets the localized value to be used. Set to null or empty string to use default value.
        /// </summary>
        public string LocalizedValue = null;

        /// <summary>
        /// Initializes a new instance of the AdvPropertyGridLocalizeEventArgs class.
        /// </summary>
        /// <param name="key">Key value event is raised for.</param>
        /// <param name="localizationType">Localization Type being performed.</param>
        public AdvPropertyGridLocalizeEventArgs(string key, ePropertyGridLocalizationType localizationType)
        {
            Key = key;
            LocalizationType = localizationType;
        }
    }
    /// <summary>
    /// Defines the localization types for AdvPropertyGrid control.
    /// </summary>
    public enum ePropertyGridLocalizationType
    {
        /// <summary>
        /// Property name is localized.
        /// </summary>
        PropertyName,
        /// <summary>
        /// Category is localized.
        /// </summary>
        Category,
        /// <summary>
        /// Error super tooltip parts are being localized.
        /// </summary>
        ErrorTooltip
    }
    #endregion

    #region Error SuperTooltip Event
    /// <summary>
    /// Defines delegate for Localize AdvPropertyGrid event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void PropertyErrorTooltipEventHandler(object sender, PropertyErrorTooltipEventArgs e);
    /// <summary>
    /// Defines data for PrepareErrorTooltip AdvPropertyGrid event.
    /// </summary>
    public class PropertyErrorTooltipEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the SuperTooltipInfo that represent tooltip that will be displayed to provide error information.
        /// </summary>
        public SuperTooltipInfo TooltipInfo = null;
        /// <summary>
        /// Gets the name of the property tooltip is displayed for.
        /// </summary>
        public readonly string PropertyName;
        /// <summary>
        /// Gets reference to the exception that was thrown when property was set.
        /// </summary>
        public readonly Exception Exception;
        /// <summary>
        /// Gets the value that was set and caused the error.
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// Initializes a new instance of the PropertyErrorTooltipEventArgs class.
        /// </summary>
        /// <param name="tooltipInfo">SuperTooltipInfo for error tooltip</param>
        /// <param name="propertyName">Property that caused error</param>
        /// <param name="exception">Exception that was raised when property was set</param>
        /// <param name="value">Value that caused the error.</param>
        public PropertyErrorTooltipEventArgs(SuperTooltipInfo tooltipInfo, string propertyName, Exception exception, object value)
        {
            TooltipInfo = tooltipInfo;
            PropertyName = propertyName;
            Exception = exception;
            Value = value;
        }
    }
    #endregion

    #region Value Conversion
    /// <summary>
    /// Defines delegate for Convert Value events.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event data.</param>
    public delegate void ConvertValueEventHandler(object sender, ConvertValueEventArgs e);
    /// <summary>
    /// Defines data for Convert Value events that allows custom value conversion for property grid.
    /// </summary>
    public class ConvertValueEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the string property value.
        /// </summary>
        public string StringValue = null;
        /// <summary>
        /// Gets or sets the typed property value.
        /// </summary>
        public object TypedValue = null;
        /// <summary>
        /// Gets the property name for which conversion is being done.
        /// </summary>
        public readonly string PropertyName;
        /// <summary>
        /// Gets the target component that property is on.
        /// </summary>
        public readonly object TargetComponent;
        /// <summary>
        /// Gets or sets whether converted value is used. You need to set this property to true to indicate that you have performed value conversion.
        /// </summary>
        public bool IsConverted = false;
        /// <summary>
        /// Gets the property descriptor that describes property.
        /// </summary>
        public readonly PropertyDescriptor PropertyDescriptor; 

        /// <summary>
        /// Initializes a new instance of the ConvertValueEventArgs class.
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="typedValue"></param>
        /// <param name="propertyName"></param>
        /// <param name="targetComponent"></param>
        public ConvertValueEventArgs(string stringValue, object typedValue, string propertyName, object targetComponent, PropertyDescriptor descriptor)
        {
            StringValue = stringValue;
            TypedValue = typedValue;
            PropertyName = propertyName;
            TargetComponent = targetComponent;
            PropertyDescriptor = descriptor;
        }
    }
    #endregion

    #region Property Value List
    /// <summary>
    /// Defines delegate for Convert Value events.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event data.</param>
    public delegate void PropertyValueListEventHandler(object sender, PropertyValueListEventArgs e);
    /// <summary>
    /// Defines data for Convert Value events that allows custom value conversion for property grid.
    /// </summary>
    public class PropertyValueListEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the property name value list is needed for.
        /// </summary>
        public readonly string PropertyName;
        /// <summary>
        /// Gets the target component that property is on.
        /// </summary>
        public readonly object TargetComponent;
        /// <summary>
        /// Gets or sets whether property ValueList provided is valid. You must set this property to true in order for the list to be used.
        /// </summary>
        public bool IsListValid = false;
        /// <summary>
        /// Gets or sets the list of valid property values.
        /// </summary>
        public List<string> ValueList = null;
        /// <summary>
        /// Gets the property descriptor that describes property.
        /// </summary>
        public readonly PropertyDescriptor PropertyDescriptor; 

        /// <summary>
        /// Initializes a new instance of the PropertyValueListEventArgs class.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="targetComponent"></param>
        public PropertyValueListEventArgs(string propertyName, object targetComponent, PropertyDescriptor descriptor)
        {
            PropertyName = propertyName;
            TargetComponent = targetComponent;
            PropertyDescriptor = descriptor;
        }
    }
    #endregion

    #region Element Style Provider
    public interface IPropertyElementStyleProvider
    {
        /// <summary>
        /// Gets the style that is applied to property node when it is in read-only state.
        /// </summary>
        ElementStyle ReadOnlyStyle { get;}
        /// <summary>
        /// Gets the style that is applied to property node when its value has changed from the default value for the property.
        /// </summary>
        ElementStyle ValueChangedStyle { get;}
        /// <summary>
        /// Gets the property category style.
        /// </summary>
        ElementStyle CategoryStyle { get;}
    }
    #endregion

    #region Custom UITypeEditor  Event
    /// <summary>
    /// Defines delegate for Localize AdvPropertyGrid event.
    /// </summary>
    /// <param name="sender">Sender of event.</param>
    /// <param name="e">Event data.</param>
    public delegate void ProvideUITypeEditorEventHandler(object sender, ProvideUITypeEditorEventArgs e);
    /// <summary>
    /// Defines data for PrepareErrorTooltip AdvPropertyGrid event.
    /// </summary>
    public class ProvideUITypeEditorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the property name UITypeEditor is looked for.
        /// </summary>
        public readonly string PropertyName;
        /// <summary>
        /// Gets the property descriptor for property UITypeEditor is looked for.
        /// </summary>
        public readonly PropertyDescriptor PropertyDescriptor;
        /// <summary>
        /// Gets or sets the instance of UITypeEditor to be used for the property. You set this property to the UITypeEditor that you want
        /// used for the property while editing. Note that you must set EditorSpecified = true in order for this value to be used.
        /// </summary>
        public System.Drawing.Design.UITypeEditor UITypeEditor = null;
        /// <summary>
        /// Gets or sets whether the value specified in UITypeEditor property is used. You must set this value to true in order for
        /// UITypeEditor property value to be used.
        /// </summary>
        public bool EditorSpecified = false;

        /// <summary>
        /// Initializes a new instance of the ProvideUITypeEditorEventArgs class.
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        /// <param name="propertyDescriptor">Property Descriptor</param>
        public ProvideUITypeEditorEventArgs(string propertyName, PropertyDescriptor propertyDescriptor)
        {
            PropertyName = propertyName;
            PropertyDescriptor = propertyDescriptor;
        }
    }
    #endregion

    #region PropertyValueChanging
    /// <summary>
    /// Defines delegate for PropertyValueChanging event.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event data.</param>
    public delegate void PropertyValueChangingEventHandler(object sender, PropertyValueChangingEventArgs e);
    /// <summary>
    /// Defines arguments for PropertyValueChanging event.
    /// </summary>
    public class PropertyValueChangingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the name of the property which value is changing.
        /// </summary>
        public readonly string PropertyName;
        /// <summary>
        /// Gets the new value that is being set on property.
        /// </summary>
        public readonly object NewValue;
        /// <summary>
        /// Gets or sets whether value assignment was handled by your code. Set to true to cancel internal AdvPropertyGrid property value assignment.
        /// </summary>
        public bool Handled = false;
        /// <summary>
        /// Initializes a new instance of the PropertyValueChangingEventArgs class.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        public PropertyValueChangingEventArgs(string propertyName, object newValue)
        {
            PropertyName = propertyName;
            NewValue = newValue;
        }
    }
    #endregion

    #region ValidatePropertyValue
    /// <summary>
    /// Defines delegate for ValidatePropertyValue event.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">ValidatePropertyValue event arguments</param>
    public delegate void ValidatePropertyValueEventHandler(object sender, ValidatePropertyValueEventArgs e);
    /// <summary>
    /// Defines event arguments for ValidatePropertyValue event.
    /// </summary>
    public class ValidatePropertyValueEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether validation failed. Set to true to cancel property value assignment.
        /// </summary>
        public bool Cancel = false;
        /// <summary>
        /// Gets the name of the property which value is changing.
        /// </summary>
        public readonly string PropertyName;
        /// <summary>
        /// Gets the new value that is being set on property.
        /// </summary>
        public readonly object NewValue;
        /// <summary>
        /// Gets the target object on which property value is set.
        /// </summary>
        public readonly object Target;
        /// <summary>
        /// Gets or sets the message that is displayed to user if validation fails, i.e. Cancel=true.
        /// </summary>
        public string Message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the ValidatePropertyValueEventArgs class.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        /// <param name="target"></param>
        public ValidatePropertyValueEventArgs(string propertyName, object newValue, object target)
        {
            PropertyName = propertyName;
            NewValue = newValue;
            Target = target;
        }
    }
    #endregion
}
#endif