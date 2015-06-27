#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.AdvTree;
using System.ComponentModel;
using System.Reflection;
using DevComponents.DotNetBar.Controls;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Drawing.Imaging;
using System.Collections;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents a property node in AdvPropertyGrid.
    /// </summary>
    public class PropertyNode : Node, ITypeDescriptorContext, IWindowsFormsEditorService
    {
        #region Private Variables
        private const string ID_UITypeEditorImage = "UITypeEditorImage";
        #endregion

        #region Constructors
        private PropertyDescriptor _Property = null;
        /// <summary>
        /// Initializes a new instance of the PropertyNode class.
        /// </summary>
        /// <param name="property"></param>
        public PropertyNode(PropertyDescriptor property)
        {
            _Property = property;
            this.CellNavigationEnabled = false;

            PasswordPropertyTextAttribute passwordAttribute = _Property.Attributes[typeof(PasswordPropertyTextAttribute)] as PasswordPropertyTextAttribute;
            if (passwordAttribute != null && passwordAttribute.Password)
                IsPassword = true;
        }
        #endregion

        #region Internal Implementation
        public virtual void UpdateDisplayedValue()
        {
            if (_IsDisposed || _IsDisposing) return;
            object propertyValue = PropertyValue;
            UpdateDisplayedValue(propertyValue);
        }

        protected virtual void UpdateDisplayedValue(object propertyValue)
        {
            if (_IsDisposed || _IsDisposing) return;

            // Value cell
            Cell cell = this.EditCell;
            cell.Text = GetPropertyTextValue(propertyValue);
            if (GetIsDefaultValue(propertyValue))
            {
                cell.StyleNormal = null;
            }
            else
            {
                cell.StyleNormal = ValueChangedStyle;
            }

            RevertEditorValue();

            if (IsTypeEditorPaintValueSupported)
            {
                UITypeEditor typeEditor = GetTypeEditor();
                if (typeEditor != null && typeEditor.GetPaintValueSupported(this))
                {
                    Rectangle r = new Rectangle(0, 0, 20, 13);
                    Bitmap image = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
                    image.Tag = ID_UITypeEditorImage;
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        bool error = false;
                        try
                        {
                            typeEditor.PaintValue(new PaintValueEventArgs(this, propertyValue, g, r));
                            //typeEditor.PaintValue(propertyValue, g, r);
                        }
                        catch
                        {
                            error = true;
                        }
                        if (error)
                            DisplayHelp.FillRectangle(g, r, Color.White);

                        DisplayHelp.DrawRectangle(g, SwatchBorderColor, r);
                    }
                    cell.Images.Image = image;
                }
                else
                {
                    if (cell.Images.Image != null && cell.Images.Image.Tag is string && (string)cell.Images.Image.Tag == ID_UITypeEditorImage)
                    {
                        Image image = cell.Images.Image;
                        cell.Images.Image = null;
                        image.Dispose();
                    }
                }
            }
            UpdateChildProperties();
            if (this.HasChildNodes && this.Expanded)
                LoadChildProperties();
        }
        private Color _SwatchBorderColor = Color.Black;
        private Color SwatchBorderColor
        {
            get
            {
                return _SwatchBorderColor;
            }
        }
        protected virtual bool IsTypeEditorPaintValueSupported
        {
            get
            {
                return true;
            }
        }

        private string GetPropertyTextValue()
        {
            return GetPropertyTextValue(this.PropertyValue);
        }

        protected string GetPropertyTextValue(object propertyValue)
        {
            if (_PropertySettings != null && _PropertySettings.HasConvertPropertyValueToStringHandler)
            {
                ConvertValueEventArgs e = new ConvertValueEventArgs(null, propertyValue, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
                _PropertySettings.InvokeConvertPropertyValueToString(e);
                if (e.IsConverted && e.StringValue != null)
                    return e.StringValue;
            }

            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null && grid.HasConvertPropertyValueToStringHandler)
            {
                ConvertValueEventArgs e = new ConvertValueEventArgs(null, propertyValue, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
                grid.InvokeConvertPropertyValueToString(e);
                if (e.IsConverted && e.StringValue != null)
                    return e.StringValue;
            }

            string textValue = null;
            TypeConverter typeConverter = this.TypeConverter;
            try
            {
                textValue = typeConverter.ConvertToString(this, propertyValue);
            }
            catch (Exception)
            {
            }
            if (textValue == null)
            {
                textValue = string.Empty;
            }
            if (IsPassword)
            {
                textValue = StringHelper.Repeat(AdvPropertyGrid.PasswordChar, textValue.Length);
            }
            return textValue;
        }

        private TypeConverter _TypeConverter = null;
        internal virtual TypeConverter TypeConverter
        {
            get
            {
                if (_TypeConverter == null)
                {
                    _TypeConverter = GetPropertyDescriptor().Converter;
                }
                return _TypeConverter;
            }
        }

        public virtual Type PropertyType
        {
            get
            {
                object propertyValue = this.PropertyValue;
                if (propertyValue != null)
                {
                    return propertyValue.GetType();
                }
                else
                    return _Property.PropertyType;
                return null;
            }
        }

        public virtual object PropertyValue
        {
            get
            {
                try
                {
                    object propertyValue = this.GetPropertyValue(this.GetTargetComponent());
                    return propertyValue;
                }
                catch (Exception exception)
                {
                    return exception;
                }
            }
            set
            {
                ApplyValue(value, null);
            }
        }

        protected virtual bool GetIsDefaultValue()
        {
            return GetIsDefaultValue(PropertyValue);
        }

        private bool GetIsDefaultValue(object propertyValue)
        {
            DefaultValueAttribute defaultValueAttribute = GetPropertyDescriptor().Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
            if (defaultValueAttribute != null)
            {
                if (defaultValueAttribute.Value != null)
                    return (defaultValueAttribute.Value.Equals(propertyValue));
                return defaultValueAttribute.Value == propertyValue;
            }

            if (propertyValue != null)
            {
                string strfunct = "ShouldSerialize" + GetPropertyName();
                PropertyDescriptor propDesc = GetPropertyDescriptor();
                if (propDesc!=null && propDesc.ComponentType!=null && propDesc.ComponentType.GetMethod(strfunct, BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) != null)
                {
                    MethodInfo mi = propDesc.ComponentType.GetMethod(strfunct, BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    object result = mi.Invoke(GetTargetComponent(), null);
                    if (result is bool && ((bool)result))
                        return false;
                }
            }

            return true;
        }

        protected virtual object GetTargetComponent()
        {
            if (_TargetComponent != null) return _TargetComponent;

            Node node = this.Parent as Node;
            while (node != null)
            {
                if (node is PropertyNode)
                    return ((PropertyNode)node).GetTargetComponent();
                node = node.Parent as Node;
            }

            return _TargetComponent;
        }

        private object _TargetComponent;
        internal object TargetComponent
        {
            get { return _TargetComponent; }
            set
            {
                object oldValue = _TargetComponent;
                _TargetComponent = value;
                OnTargetComponentChanged(oldValue, value);
            }
        }
        private bool _PropertyChangedEventHandled = false;
        protected virtual void OnTargetComponentChanged(object oldValue, object newValue)
        {
            string eventName = GetPropertyName() + "Changed";
            if (oldValue != null && _PropertyChangedEventHandled)
            {
                try
                {
                    if (oldValue is ICustomTypeDescriptor)
                    {
                        EventDescriptor item = GetEventDescriptor(oldValue, eventName);
                        if (item != null)
                            item.RemoveEventHandler(oldValue, this.PropertyValueChangedEventHandler);
                    }
                    else
                    {
                        EventInfo info = oldValue.GetType().GetEvent(eventName);
                        if (info != null)
                            info.RemoveEventHandler(oldValue, this.PropertyValueChangedEventHandler);
                    }
                }
                finally
                {
                    _PropertyChangedEventHandled = false;
                }
            }

            if (newValue != null && GetPropertyDescriptor() != null)
            {
                // Check whether object has PropertyNameChanged event handler...
                if (newValue is ICustomTypeDescriptor)
                {
                    EventDescriptor item = GetEventDescriptor(newValue, eventName);
                    if (item != null)
                    {
                        try
                        {
                            item.AddEventHandler(newValue, PropertyValueChangedEventHandler);
                            _PropertyChangedEventHandled = true;
                        }
                        catch
                        {
                            _PropertyChangedEventHandled = false;
                        }
                    }
                }
                else
                {
                    EventInfo info = newValue.GetType().GetEvent(eventName);
                    if (info != null && info.EventHandlerType == typeof(EventHandler))
                    {
                        try
                        {
                            info.AddEventHandler(newValue, PropertyValueChangedEventHandler);
                            _PropertyChangedEventHandled = true;
                        }
                        catch
                        {
                            _PropertyChangedEventHandled = false;
                        }
                    }
                }

            }
        }

        private EventDescriptor GetEventDescriptor(object value, string eventName)
        {
            EventDescriptorCollection col = ((ICustomTypeDescriptor)value).GetEvents();
            foreach (EventDescriptor item in col)
            {
                if (item.Name == eventName)
                {
                    if (item.EventType == typeof(EventHandler))
                    {
                        return item;
                    }
                    break;
                }
            }
            return null;
        }

        private void PropertyValueChanged(object sender, EventArgs e)
        {
            UpdateDisplayedValue();
        }

        private EventHandler _PropertyValueChangedEventHandler = null;
        private EventHandler PropertyValueChangedEventHandler
        {
            get
            {
                if (_PropertyValueChangedEventHandler == null)
                    _PropertyValueChangedEventHandler = new EventHandler(PropertyValueChanged);
                return _PropertyValueChangedEventHandler;
            }
        }

        protected virtual object GetPropertyValue(object target)
        {
            PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
            if (propertyDescriptor == null)
            {
                return null;
            }

            if (_IsMultiObject)
            {
                object[] targets = (object[])target;
                object returnValue = GetPropertyValueCore(targets[0], propertyDescriptor);
                TypeConverter converter = this.TypeConverter;
                for (int i = 1; i < targets.Length; i++)
                {
                    object value=null;
                    if (propertyDescriptor.ComponentType.IsAssignableFrom(targets[i].GetType()))
                        value = GetPropertyValueCore(targets[i], propertyDescriptor);
                    else
                    {
                        //PropertyDescriptor desc2 = PropertyParser.GetProperty(propertyDescriptor.Name, targets[i], converter.GetPropertiesSupported(this) ? converter : null);
                        PropertyDescriptor desc2 = PropertyParser.GetProperty(propertyDescriptor.Name, targets[i], null);
                        value = GetPropertyValueCore(targets[i], desc2);
                    }
                    if (returnValue != null && !returnValue.Equals(value) || returnValue == null && value != null)
                    {
                        returnValue = null;
                        break;
                    }
                }
                return returnValue;
            }
            else
            {
                return GetPropertyValueCore(target, propertyDescriptor);
            }
        }

        protected virtual object GetPropertyValueCore(object target, PropertyDescriptor propertyDescriptor)
        {
            object propertyValue;
            
            if (target is ICustomTypeDescriptor)
            {
                target = ((ICustomTypeDescriptor)target).GetPropertyOwner(propertyDescriptor);
            }
            try
            {
                propertyValue = propertyDescriptor.GetValue(target);
            }
            catch
            {
                throw;
            }
            return propertyValue;
        }

        public PropertyDescriptor PropertyDescriptor
        {
            get
            {
                return _Property;
            }
            internal set
            {
                _Property = value;
                UpdateValueInlineEditor();
            }
        }

        protected override void OnParentChanged()
        {
            UpdateValueInlineEditor();
            base.OnParentChanged();
        }

        private bool IsEditorReadOnly
        {
            get
            {
                PropertyDescriptor propDesc = PropertyDescriptor;
                if (propDesc != null) return propDesc.IsReadOnly;
                return false;
            }
        }
        /// <summary>
        /// Releases the inline editor used by the node.
        /// </summary>
        public virtual void ReleaseInlineEditor()
        {
            UpdateValueInlineEditor();
        }

        private bool _IsUsingInlineEditor = false;
        private void UpdateValueInlineEditor()
        {
            if (_IsUsingInlineEditor)
            {
                Cell editCell = this.EditCell;
                Control editor = editCell.HostedControl;
                if (editor != null)
                {
                    editCell.HostedControl = null;
                    if (editor is IPropertyValueEditor)
                        ((IPropertyValueEditor)editor).EditValueChanged -= ValueEditorEditValueChanged;
                    editor.Dispose();
                }
                else if (editCell.HostedItem != null)
                {
                    BaseItem item = editCell.HostedItem;
                    editCell.HostedItem = null;
                    if (item is IPropertyValueEditor)
                        ((IPropertyValueEditor)item).EditValueChanged -= ValueEditorEditValueChanged;
                    item.Dispose();
                }
            }

            _IsUsingInlineEditor = false;

            if (_Property == null) return;

            PropertyValueEditor valueEditor;
            if (_PropertySettings != null && _PropertySettings.ValueEditor != null)
                valueEditor = _PropertySettings.ValueEditor;
            else
                valueEditor = _Property.Attributes[typeof(PropertyValueEditor)] as PropertyValueEditor;

            if (valueEditor != null)
            {
                IPropertyValueEditor editorCustom = valueEditor.CreateEditor(_Property, _TargetComponent);
                editorCustom.EditValueChanged += ValueEditorEditValueChanged;
                _Editor = editorCustom;
                if (editorCustom is Control)
                {
                    this.EditCell.HostedControl = (Control)_Editor;
                    if (this.IsReadOnly || this.IsEditorReadOnly) this.EditCell.HostedControl.Enabled = false;
                }
                else if (editorCustom is BaseItem)
                {
                    this.EditCell.HostedItem = (BaseItem)_Editor;
                    if (this.IsReadOnly || this.IsEditorReadOnly) this.EditCell.Enabled = false;
                }
                else
                    throw new ArgumentException("PropertyValueEditor must inherit from Control or BaseItem.");
                _IsUsingInlineEditor = true;
            }
        }

        private ElementStyle _ValueChangedStyle = null;
        public ElementStyle ValueChangedStyle
        {
            get
            {
                if (_ValueChangedStyle == null)
                {
                    IPropertyElementStyleProvider styleProvider = StyleProvider;
                    if (styleProvider != null) return styleProvider.ValueChangedStyle;
                }
                return _ValueChangedStyle;
            }
            set
            {
                _ValueChangedStyle = value;
            }
        }

        object _Editor = null;
        protected override void OnSelected(eTreeAction action)
        {
            if (_IsDisposed || _IsDisposing) return;

            UpdateDisplayedValue();

            AdvTree.AdvTree tree = this.TreeControl;
            bool focusEditor = false;
            Point pos = Point.Empty;
            if (tree != null)
            {
                pos = tree.PointToClient(Control.MousePosition);
                if (this.EditCell.Bounds.Contains(pos))
                    focusEditor = true;
            }

            if (action == eTreeAction.Keyboard && WinApi.HIWORD(WinApi.GetKeyState(9)) != 0)
                focusEditor = true;

            if (this.Enabled && (!this.IsReadOnly && CanEditPropertyContent()))
            {
                EnterEditorMode(action, focusEditor);
                if (focusEditor && !pos.IsEmpty && _Editor is TextBoxDropDown)
                {
                    ((TextBoxDropDown)_Editor).TextBox.SelectAll();
                }
            }

            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null)
            {
                string description = GetDescriptionAttributeValue();
                
                if (!string.IsNullOrEmpty(description))
                {
                    bool replaceNewLineCharacters = false;
                    if (!TextMarkup.MarkupParser.IsMarkup(ref description))
                        replaceNewLineCharacters = true;
                    description = description.Replace("<", "&lt;"); description = description.Replace(">", "&gt;");
                    description = "<b>" + this.Text + "</b><br/>" + description;
                    if (replaceNewLineCharacters)
                        description = description.Replace("\n", "<br/>");
                }
                grid.HelpPanel.Text = description;
            }

            base.OnSelected(action);
        }

        private string _Description = null;
        private string GetDescriptionAttributeValue()
        {
            if (_PropertySettings != null && _PropertySettings.Description != null)
                return _PropertySettings.Description;

            if (_Description == null)
            {
                _Description = _Property.Description;
                //DescriptionAttribute att = _Property.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
                //if (att != null)
                //    _Description = att.Description;
                //else
                //    _Description = "";
            }
            return _Description;
        }

        internal TextBoxDropDown GetEditor()
        {
            return _Editor as TextBoxDropDown;
        }

        private bool IsUsingPropertyEditor()
        {
            if (_Property != null && !_Property.PropertyType.IsPrimitive && !_Property.PropertyType.IsEnum && !(_Property.PropertyType == typeof(string)))
            {
                UITypeEditor editor = GetTypeEditor();
                if (editor != null) //  && (editor.GetEditStyle(this) == UITypeEditorEditStyle.Modal || editor.GetEditStyle(this) == UITypeEditorEditStyle.None)
                    return true;
            }
            return false;
        }

        private bool CanEditPropertyContent()
        {
            if (_Property != null && !_Property.PropertyType.IsPrimitive && !_Property.PropertyType.IsEnum && !(_Property.PropertyType ==typeof(string)))
            {
                UITypeEditor editor = GetTypeEditor();
                if (editor != null) //  && (editor.GetEditStyle(this) == UITypeEditorEditStyle.Modal || editor.GetEditStyle(this) == UITypeEditorEditStyle.None)
                    return true;
                else
                {
                    if (this.TypeConverter != null && this.TypeConverter.CanConvertFrom(typeof(string)))
                        return true;
                    
                    return false;
                }
            }
            return true;
        }
        private bool _IsReadOnly = false;
        /// <summary>
        /// Gets or sets whether property text box is read only. Default value is false.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set
            {
                if (_IsReadOnly == value) return;

                _IsReadOnly = value;
                if (_IsReadOnly)
                    this.Style = GetReadOnlyStyle();
                else
                    this.Style = GetDefaultStyle();
                OnIsReadOnlyChanged();
            }
        }

        protected virtual void OnIsReadOnlyChanged()
        {
            Cell editCell = this.EditCell;
            if (editCell == null) return;

            if (editCell.HostedControl is IPropertyValueEditor)
                editCell.HostedControl.Enabled = !_IsReadOnly;
            if (editCell.HostedItem is IPropertyValueEditor)
                editCell.HostedItem.Enabled = !_IsReadOnly;
        }

        private ElementStyle GetReadOnlyStyle()
        {
            if (_PropertySettings != null && _PropertySettings.ReadOnlyStyle != null)
                return _PropertySettings.ReadOnlyStyle;

            IPropertyElementStyleProvider styleProvider = this.StyleProvider;
            if (styleProvider != null)
            {
                return styleProvider.ReadOnlyStyle;
            }
            return null;
        }

        public IPropertyElementStyleProvider StyleProvider
        {
            get
            {
                return AdvPropertyGrid as IPropertyElementStyleProvider;
            }
        }
        private ElementStyle GetDefaultStyle()
        {
            if (_PropertySettings != null && _PropertySettings.Style != null)
                return _PropertySettings.Style;
            return null;
        }

        protected virtual Cell EditCell
        {
            get
            {
                return this.Cells[1];
            }
        }

        protected override bool CanKeyboardNavigate(KeyEventArgs e)
        {
            return !IsEditing || IsEditing && (_Editor is TextBoxDropDown && !((TextBoxDropDown)_Editor).TextBox.Focused ||
                _Editor is IPropertyValueEditor && ((IPropertyValueEditor)_Editor).IsEditorFocused);
        }

        /// <summary>
        /// Gets whether node is in editing mode.
        /// </summary>
        public bool IsEditing
        {
            get
            {
                return _IsEditing;
            }
            internal set
            {
                if (_IsEditing != value)
                {
                    _IsEditing = value;
                    AdvPropertyGrid grid = AdvPropertyGrid;
                    if (grid != null)
                        grid.OnPropertyIsEditingChanged(this);
                }
            }
        }

        private bool _IsEditing = false;
        /// <summary>
        /// Places the node into the editing mode if possible, raises the exception if node is read-only.
        /// </summary>
        /// <param name="action">Action that caused the edit mode.</param>
        /// <param name="focusEditor">Indicates whether to focus the editor.</param>
        public virtual void EnterEditorMode(eTreeAction action, bool focusEditor)
        {
            if (IsEditing)
                throw new InvalidOperationException("Node is already in edit mode.");
            if (!Enabled)
                throw new InvalidOperationException("Node is disabled. Cannot enter edit mode.");

            if (_IsUsingInlineEditor)
            {
                if (_Editor is IPropertyValueEditor)
                    ((IPropertyValueEditor)_Editor).EditValue = PropertyValue;
                IsEditing = true;
                if (focusEditor)
                {
                    if (_Editor is IPropertyValueEditor)
                        ((IPropertyValueEditor)_Editor).FocusEditor();
                }
                EditValueChanged = false;
                return;
            }

            if (_Editor == null)
            {
                _Editor = CreateNodeEditor();
                TextBoxDropDown editor = _Editor as TextBoxDropDown;

                if (editor != null)
                {
                    if (IsPassword)
                    {
                        editor.PasswordChar = AdvPropertyGrid.PasswordChar;
                        editor.Text = GetPropertyTextValue();
                    }
                    else
                    {
                        editor.PasswordChar = '\0';
                        editor.Text = this.EditCell.Text;
                    }
                }
                else if (_Editor is IPropertyValueEditor)
                {
                    ((IPropertyValueEditor)_Editor).EditValue = PropertyValue;
                    if (!IsReadOnly)
                        ((Control)_Editor).Enabled = false;
                }

                ElementStyle valueChangedStyle = this.ValueChangedStyle;
                if (this.EditCell.StyleNormal == valueChangedStyle && valueChangedStyle.Font != null)
                {
                    if (editor != null)
                        editor.TextBox.Font = valueChangedStyle.Font;
                    else if (_Editor is IPropertyValueEditor)
                        ((IPropertyValueEditor)_Editor).EditorFont = valueChangedStyle.Font;
                }
                this.EditCell.HostedControl = (Control)_Editor;
                IsEditing = true;
                if (focusEditor)
                {
                    if (editor != null)
                        editor.Focus();
                    else if (_Editor is IPropertyValueEditor)
                        ((IPropertyValueEditor)_Editor).FocusEditor();
                }
                EditValueChanged = false;
            }
        }

        protected virtual Control CreateNodeEditor()
        {
            bool isReadOnly = GetEffectiveReadOnly();

            //PropertyValueEditor valueEditor = _Property.Attributes[typeof(PropertyValueEditor)] as PropertyValueEditor;
            //if (valueEditor != null)
            //{
            //    IPropertyValueEditor editorCustom = valueEditor.CreateEditor(_Property, _TargetComponent);
            //    editorCustom.EditValueChanged += ValueEditorEditValueChanged;
            //    return (Control)editorCustom;
            //}

            TextBoxDropDown editor = new TextBoxDropDown();
            if (BarUtilities.UseTextRenderer)
                editor.BackgroundStyle.MarginLeft = 3;
            editor.Visible = true;
            editor.TextBox.KeyDown += new KeyEventHandler(EditorKeyDown);
            editor.TextBox.TextChanged += new EventHandler(EditorTextChanged);
            editor.TextBox.ReadOnly = isReadOnly;
            editor.TextBox.PreventEnterBeep = true;
            UITypeEditor typeEditor = GetTypeEditor();
            UITypeEditorEditStyle editStyle = UITypeEditorEditStyle.None;
            if (typeEditor != null)
            {
                editStyle = typeEditor.GetEditStyle(this);
                if (editStyle == UITypeEditorEditStyle.Modal)
                {
                    editor.ButtonCustom.Visible = true;
                    editor.ButtonCustomClick += new EventHandler(EditorButtonCustomClick);
                }
                else if (editStyle == UITypeEditorEditStyle.DropDown)
                {
                    editor.ButtonDropDown.Visible = true;
                    editor.ButtonDropDownClick += new CancelEventHandler(EditorButtonDropDownClick);
                }
                if (!IsReadOnly)
                {
                    PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
                    if (propertyDescriptor != null && !propertyDescriptor.PropertyType.IsAssignableFrom(typeof(string)) &&
                        propertyDescriptor.Converter != null && !propertyDescriptor.Converter.CanConvertFrom(typeof(string)))
                        editor.TextBox.ReadOnly = true;
                }
            }

            if (typeEditor == null || editStyle == UITypeEditorEditStyle.None)
            {
                if (!isReadOnly)
                {
                    SetupEditorAutoCompleteList(editor);
                    if (!SetupEditorPopup(editor))
                        editor.ButtonDropDown.Visible = false;
                    else
                        editor.ButtonDropDown.Visible = true;
                }
            }
            return editor;
        }

        void ValueEditorEditValueChanged(object sender, EventArgs e)
        {
            EditValueChanged = true;
            if(IsEditing)
                ApplyEdit();
        }

        private void EditorButtonDropDownClick(object sender, CancelEventArgs e)
        {
            UITypeEditor typeEditor = GetTypeEditor();
            if (typeEditor != null && typeEditor.GetEditStyle(this) == UITypeEditorEditStyle.DropDown)
            {
                object value = typeEditor.EditValue(this, this, this.PropertyValue);
                ApplyValue(value, null);
            }
            e.Cancel = true;
        }

        private bool GetEffectiveReadOnly()
        {
            return this.IsReadOnly;
        }
        private UITypeEditor GetTypeEditor()
        {
            PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();

            if (_PropertySettings != null && _PropertySettings.UITypeEditor != null)
            {
                return _PropertySettings.UITypeEditor;
            }

            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null && grid.HasProvideUITypeEditorHandler)
            {
                ProvideUITypeEditorEventArgs args = new ProvideUITypeEditorEventArgs(GetPropertyName(), propertyDescriptor);
                grid.InvokeProvideUITypeEditor(args);
                if (args.EditorSpecified) return args.UITypeEditor;
            }

            object value = PropertyValue;
            if (value is ICustomTypeDescriptor)
            {
                ICustomTypeDescriptor descriptor = (ICustomTypeDescriptor)value;
                UITypeEditor editor=(UITypeEditor)descriptor.GetEditor(typeof(UITypeEditor));
                if (editor != null)
                    return editor;
            }
            return (UITypeEditor)propertyDescriptor.GetEditor(typeof(UITypeEditor));
        }

        protected virtual string GetPropertyName()
        {
            return GetPropertyDescriptor().Name;
        }

        protected virtual PropertyDescriptor GetPropertyDescriptor()
        {
            return _Property;
        }

        private void EditorButtonCustomClick(object sender, EventArgs e)
        {
            UITypeEditor typeEditor = GetTypeEditor();
            if (typeEditor == null) return;
            object value = null;
            try
            {
                value = typeEditor.EditValue(this, this, this.PropertyValue);
            }
            catch(Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                return;
            }
            ApplyValue(value, null);
        }

        protected virtual bool SetupEditorPopup(TextBoxDropDown editor)
        {
            if (this.PropertyType == null) return false;
            string[] items = GetPopupListContent();
            if (items == null || items.Length == 0) return false;

            TextBoxDropDown textBox = editor;
            ListBox list = new ListBox();
            list.SelectedIndexChanged += new EventHandler(PopupListSelectedIndexChanged);
            list.BorderStyle = BorderStyle.None;
            list.Items.AddRange(items);
            if (!string.IsNullOrEmpty(editor.Text))
                list.SelectedItem = editor.Text;
            textBox.DropDownControl = list;
            list.Size = GetPreferredListBoxSize(ref items);

            return true;
        }

        private void PopupListSelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox list = (ListBox)sender;
            if (!list.Visible || list.SelectedItem == null) return;
            TextBoxDropDown editor = _Editor as TextBoxDropDown;
            editor.Text = (string)list.SelectedItem;
            editor.CloseDropDown();
            EditValueChanged = true;
            if (ApplyEdit())
            {
                AdvTree.AdvTree tree = this.TreeControl;
                if (tree != null) tree.Focus();
                this.SelectedCell = this.Cells[0];
            }
        }

        private Size _CachedPreferredListBoxSize = Size.Empty;
        protected virtual Size GetPreferredListBoxSize(ref string[] items)
        {
            if (items == null || items.Length == 0) return new Size(96, 32);
            AdvTree.AdvTree tree = this.TreeControl;
            if (tree == null) return new Size(96, 32);
            if (!_CachedPreferredListBoxSize.IsEmpty) return _CachedPreferredListBoxSize;

            Size totalSize = Size.Empty;
            using (Graphics g = tree.CreateGraphics())
            {
                Font font = tree.Font;
                for (int i = 0; i < items.Length; i++)
                {
                    Size itemSize = TextDrawing.MeasureString(g, items[i], font);
                    totalSize.Width = Math.Max(itemSize.Width, totalSize.Width);
                    if (totalSize.Height < 320)
                        totalSize.Height += itemSize.Height;
                    else
                    {
                        if (totalSize.Width < 120) totalSize.Width = 128;
                        break;
                    }
                }
            }
            if (totalSize.Width < this.EditCell.Bounds.Width - 8)
                totalSize.Width = this.EditCell.Bounds.Width - 8;
            _CachedPreferredListBoxSize = totalSize;
            return totalSize;
        }

        protected virtual string[] GetPopupListContent()
        {
            if (_PropertySettings != null && _PropertySettings.HasProvidePropertyValueListHandler)
            {
                PropertyValueListEventArgs args = new PropertyValueListEventArgs(GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
                _PropertySettings.InvokeProvidePropertyValueList(args);
                if (args.IsListValid)
                {
                    if (args.ValueList == null) return null;
                    return args.ValueList.ToArray();
                }
            }

            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null && grid.HasProvidePropertyValueListHandler)
            {
                PropertyValueListEventArgs args = new PropertyValueListEventArgs(GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
                grid.InvokeProvidePropertyValueList(args);
                if (args.IsListValid)
                {
                    if (args.ValueList == null) return null;
                    return args.ValueList.ToArray();
                }
            }

            TypeConverter converter = this.TypeConverter;
            if(converter!=null && converter.GetStandardValuesSupported(this))
            {
                System.ComponentModel.TypeConverter.StandardValuesCollection stdValues = converter.GetStandardValues(this);
                if (stdValues != null && stdValues.Count > 0)
                {
                    string[] values = new string[stdValues.Count];
                    for (int i = 0; i < stdValues.Count; i++)
                    {
                        object item = stdValues[i];
                        values[i] = converter.ConvertToString(item);
                    }
                    return values;
                }
            }

            if (this.PropertyType.IsEnum)
            {
                return Enum.GetNames(this.PropertyType);
            }
            else if (this.PropertyType == typeof(bool))
            {
                string[] items = new string[2];
                
                if (converter != null && converter.CanConvertTo(typeof(string)))
                {
                    items[0] = (string)converter.ConvertTo(false, typeof(string));
                    items[1] = (string)converter.ConvertTo(true, typeof(string));
                }
                else
                {
                    items[0] = bool.FalseString;
                    items[1] = bool.TrueString;
                }
                return items;
            }
            return null;
        }

        protected virtual void SetupEditorAutoCompleteList(TextBoxDropDown editor)
        {
            if (this.PropertyType == null) return;

            TextBox textBox = editor.TextBox;
            TypeConverter converter = this.TypeConverter;
            if (converter != null && converter.GetStandardValuesExclusive(this))
                editor.InternalReadOnly = true;
            else
                editor.InternalReadOnly = false;


            
            string[] items = GetPopupListContent();
            if (items != null && items.Length > 0)
            {
                textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox.AutoCompleteCustomSource.Clear();
                textBox.AutoCompleteCustomSource.AddRange(items);
            }

            //textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //textBox.AutoCompleteCustomSource.Clear();
            //textBox.AutoCompleteCustomSource.AddRange(items);
        }

        protected override void OnNodeDoubleClick(EventArgs e)
        {
            base.OnNodeDoubleClick(e);
            if (GetEffectiveReadOnly()) return;
            if (!SelectNextValue())
            {
                if (_Editor is TextBoxDropDown)
                {
                    ((TextBoxDropDown)_Editor).TextBox.SelectAll();
                    ((TextBoxDropDown)_Editor).Focus();
                }
                else if (_Editor is IPropertyValueEditor)
                    ((IPropertyValueEditor)_Editor).FocusEditor();
            }
        }

        public virtual bool SelectNextValue()
        {
            string[] listArray = GetPopupListContent();
            if (listArray == null || listArray.Length == 0) return false;
            List<string> list = new List<string>(listArray);
            string currentValue = this.EditCell.Text;
            if (string.IsNullOrEmpty(currentValue) || !list.Contains(currentValue))
            {
                currentValue = list[0];
            }
            else
            {
                int index = list.IndexOf(currentValue) + 1;
                if (index >= list.Count)
                    index = 0;
                currentValue = list[index];
            }
            Exception valueException = null;
            object value = GetValueFromString(currentValue, out valueException);
            if (valueException != null) return false;
            SetPropertyValue(this.GetTargetComponent(), value);
            RefreshPropertyValuesCheck();
            UpdateDisplayedValue();
            return true;
        }

        void EditorTextChanged(object sender, EventArgs e)
        {
            this.EditValueChanged = true;
        }


        protected virtual void EditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CancelEdit();
                AdvTree.AdvTree tree = this.TreeControl;
                if (tree != null) tree.Focus();
                this.SetSelectedCell(this.Cells[0], eTreeAction.Keyboard);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (ApplyEdit() && !(_IsDisposed || _IsDisposing))
                {
                    this.SetSelectedCell(this.Cells[0], eTreeAction.Keyboard);
                    AdvTree.AdvTree tree = this.TreeControl;
                    if (tree != null) tree.Focus();
                }
            }
            else if (e.KeyCode == Keys.Delete && !this.IsReadOnly && this.IsPropertyValueNullable && this.PropertyValue != null)
            {
                bool reset = true;
                TextBox textBox = sender as TextBox;
                if (textBox != null && textBox.SelectionLength != textBox.TextLength)
                    reset = false;

                if (reset)
                    ApplyValue(null, null);
            }
        }

        protected virtual bool IsPropertyValueNullable
        {
            get
            {
                return !_Property.PropertyType.IsValueType;
            }
        }

        private bool _EditValueChanged = false;
        public bool EditValueChanged
        {
            get { return _EditValueChanged; }
            set
            {
                _EditValueChanged = value;
            }
        }


        protected override void OnDeselected(eTreeAction action)
        {
            if (this.IsEditing)
            {
                if (this.EditValueChanged && !IsReadOnly)
                    ApplyEdit();
                ExitEditorMode(action);
            }
            base.OnDeselected(action);
        }

        /// <summary>
        /// Exits the editor mode.
        /// </summary>
        /// <param name="action">Action that caused the editor mode exit.</param>
        public virtual void ExitEditorMode(eTreeAction action)
        {
            if (!IsEditing) return;

            Control editor = _Editor as Control;

            if (!_IsUsingInlineEditor)
            {
                _Editor = null;
                this.EditCell.HostedControl = null;
            }

            if (editor is TextBoxDropDown)
            {
                TextBoxDropDown editorDropDown = (TextBoxDropDown)editor;
                if (editorDropDown.IsPopupOpen)
                    editorDropDown.CloseDropDown();

                if (editorDropDown.DropDownControl != null && !_UITypeEditorDropDownControl)
                {
                    Control c = editorDropDown.DropDownControl;
                    editorDropDown.DropDownControl = null;
                    c.Dispose();
                }

                editorDropDown.ButtonCustomClick -= EditorButtonCustomClick;
                editorDropDown.ButtonDropDownClick -= EditorButtonDropDownClick;
                editorDropDown.TextBox.KeyDown -= new KeyEventHandler(EditorKeyDown);
                editorDropDown.TextBox.TextChanged -= new EventHandler(EditorTextChanged);
                editorDropDown.Dispose();
            }
            else if (editor != null && !_IsUsingInlineEditor)
            {
                if(editor is IPropertyValueEditor)
                    ((IPropertyValueEditor)editor).EditValueChanged -= ValueEditorEditValueChanged;
                editor.Dispose();
            }

            if (_HighlightWorker != null && _HighlightWorker.IsBusy) _HighlightWorker.CancelAsync();
            IsEditing = false;
        }

        /// <summary>
        /// Called when visual part of the node has changed due to the changes of its properties or properties of the cells contained by node.
        /// </summary>
        internal override void OnDisplayChanged()
        {
            if (!IsDisposing && !IsDisposed)
                base.OnDisplayChanged();
        }

        /// <summary>
        /// Cancel the edit changes and applies the old property value.
        /// </summary>
        public virtual void CancelEdit()
        {
            if (!IsEditing) return;
            UpdateDisplayedValue();
            ClearErrorMarkings();
            _EditValueChanged = false;
            if (_HighlightWorker != null && _HighlightWorker.IsBusy) _HighlightWorker.CancelAsync();
        }

        protected virtual void RevertEditorValue()
        {
            Cell cell = this.EditCell;

            if (_Editor is TextBoxDropDown)
            {
                TextBoxDropDown editor = (TextBoxDropDown)_Editor;    
                editor.Text = cell.Text;
                EditValueChanged = false;
                ElementStyle valueChangedStyle = this.ValueChangedStyle;
                if (valueChangedStyle != null && cell.StyleNormal == valueChangedStyle && valueChangedStyle.Font != null)
                    editor.TextBox.Font = valueChangedStyle.Font;
                else
                    editor.TextBox.Font = null;
            }
            else if (_Editor is IPropertyValueEditor)
            {
                IPropertyValueEditor editor = (IPropertyValueEditor)_Editor;
                if (editor.EditValue != PropertyValue)
                    editor.EditValue = PropertyValue;
                EditValueChanged = false;
                ElementStyle valueChangedStyle = this.ValueChangedStyle;
                if (valueChangedStyle != null && cell.StyleNormal == valueChangedStyle && valueChangedStyle.Font != null)
                    editor.EditorFont = valueChangedStyle.Font;
                else
                    editor.EditorFont = null;
            }
        }

        /// <summary>
        /// Attempts to apply current edit value to the property.
        /// </summary>
        public virtual bool ApplyEdit()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Node is not in edit mode.");
            if (!EditValueChanged) return true;

            if (_Editor is TextBoxDropDown)
                return ApplyEdit(((TextBoxDropDown)_Editor).Text);
            else
                return ApplyEdit(((IPropertyValueEditor)_Editor).EditValue);
        }

        protected virtual bool ApplyEdit(object text)
        {
            Exception valueException = null;

            object value = null;
            if (text is string)
                value = GetValueFromString((string)text, out valueException);
            else
                value = text;

            return ApplyValue(value, valueException);
        }

        protected virtual bool ApplyValue(object value, Exception valueException)
        {
            bool valueApplied = false;

            if (GetPropertyValue(GetTargetComponent()) == value) return true;

            if (valueException == null)
            {
                AdvPropertyGrid grid = this.AdvPropertyGrid;
                if (grid != null)
                {
                    valueApplied = grid.InvokePropertyValueChanging(GetPropertyName(), value);
                }

                if (!valueApplied)
                {
                    try
                    {
                        SetPropertyValue(this.GetTargetComponent(), value);
                        valueApplied = true;
                        if (_IsDisposed || _IsDisposing) return true;
                    }
                    catch (Exception e)
                    {
                        valueException = e;
                    }
                }
            }

            if (!valueApplied)
            {
                // Set error state...
                if (_PropertySettings == null || _PropertySettings.Image == null)
                    this.Image = GetErrorImage();
                this.IsSelectionVisible = false;
                // Get SuperTooltip to display error tip
                SuperTooltip tooltip = GetSuperTooltip();
                if (tooltip != null)
                    AttachErrorTooltip(tooltip, valueException, value);

                StartHighlight(ErrorHighlightColor);
            }
            else
            {
                // if for some reason property value was not set we'll return false
                if (value != null && !value.Equals(PropertyValue) && PropertyType.IsPrimitive) valueApplied = false;

                bool reportSuccess = AdvPropertyGrid.HighlightPropertyOnUpdate && !_IsUsingInlineEditor;
                if (_ErrorTooltipAttached)
                    reportSuccess = true;
                ClearErrorMarkings();
                UpdateDisplayedValue();
                if (reportSuccess)
                    this.IsSelectionVisible = false;
                if (reportSuccess && valueApplied)
                    StartHighlight(SuccessHighlightColor);
                _EditValueChanged = false;

                if (_Property != null && valueApplied)
                {
                    RefreshPropertyValuesCheck();
                }
                
            }

            return valueApplied;
        }

        private void RefreshPropertyValuesCheck()
        {
            if (_Property == null) return;

            RefreshPropertiesAttribute refreshAttribute = GetRefreshPropertiesAttribute();
            RefreshPropertiesAttribute parentRefresh = null;
            if (this.Parent is PropertyNode)
                parentRefresh = ((PropertyNode)this.Parent).GetRefreshPropertiesAttribute();

            if (parentRefresh != null && parentRefresh.RefreshProperties == RefreshProperties.All)
            {
                AdvPropertyGrid.RefreshPropertyValues(this.Parent);
            }
            else if (refreshAttribute != null)
            {
                /*if (refreshAttribute.RefreshProperties == RefreshProperties.Repaint)
                {
                    UpdateDisplayedValue();
                }
                else*/
                if (refreshAttribute.RefreshProperties == RefreshProperties.All)
                {
                    AdvPropertyGrid.RefreshPropertyValues();
                }
            }

            if (this.Parent is PropertyNode)
                ((PropertyNode)this.Parent).UpdateDisplayedValue();
        }
        internal RefreshPropertiesAttribute GetRefreshPropertiesAttribute()
        {
            RefreshPropertiesAttribute refreshAttribute = _Property.Attributes[typeof(RefreshPropertiesAttribute)] as RefreshPropertiesAttribute;
            return refreshAttribute;
        }

        private Color ErrorHighlightColor
        {
            get
            {
                AdvPropertyGrid grid = AdvPropertyGrid;
                if (grid != null)
                {
                    return grid.Appearance.ErrorHighlightColor;
                }
                return ColorScheme.GetColor(0xD99694);
            }
        }
        private Color SuccessHighlightColor
        {
            get
            {
                AdvPropertyGrid grid = AdvPropertyGrid;
                if (grid != null)
                {
                    return grid.Appearance.SuccessHighlightColor;
                }
                return ColorScheme.GetColor(0x9BBB59);
            }
        }

        private void ClearErrorMarkings()
        {
            SuperTooltip tooltip = GetSuperTooltip();
            if (tooltip != null)
                EraseErrorTooltip(tooltip);
            if (_PropertySettings == null || _PropertySettings.Image == null)
                this.Image = null;
            if (_PropertySettings != null) UpdatePropertySettings();
        }
        private bool _ErrorTooltipAttached = false;
        private void AttachErrorTooltip(SuperTooltip tooltip, Exception valueException, object value)
        {
            SuperTooltipInfo info = new SuperTooltipInfo();
            info.HeaderText = this.Text;
            string s = GetErrorString();
            bool showTooltip = false;
            if (valueException is InvalidPropertyValueException && !string.IsNullOrEmpty(valueException.Message))
            {
                s = valueException.Message;
                showTooltip = true;
            }
            else if (valueException != null)
            {
                if (!s.EndsWith(" ")) s += " ";
                s += valueException.Message;
            }
            info.BodyText = s;
            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null)
            {
                grid.InvokePrepareErrorSuperTooltip(info, GetPropertyName(), valueException, value);
            }
            tooltip.SetSuperTooltip(this, info);
            if (showTooltip)
            {
                // Show tooltip below the node
                Rectangle r = this.Cells[0].Bounds;
                AdvTree.AdvTree tree = this.TreeControl;
                if (tree != null)
                    r.Location = tree.PointToScreen(r.Location);
                tooltip.ShowTooltip(this, new Point(r.Right - 18, r.Bottom));
            }
            _ErrorTooltipAttached = true;
        }
        private string GetErrorString()
        {
            string s = "Error setting the value. ";

            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null) s = grid.SystemText.ErrorSettingPropertyValueTooltip;

            IPropertyGridLocalizer localizer = this.PropertyGridLocalizer;
            if (localizer != null)
            {
                return localizer.GetErrorTooltipMessage(s) ?? s;
            }
            return s;
        }
        private void EraseErrorTooltip(SuperTooltip tooltip)
        {
            tooltip.HideTooltip();
            tooltip.SetSuperTooltip(this, null);
            _ErrorTooltipAttached = false;
        }

        private Image _ErrorImage = null;
        private Image GetErrorImage()
        {
            AdvPropertyGrid propertyGrid = this.AdvPropertyGrid;
            if (propertyGrid != null && propertyGrid.Appearance.PropertyValueErrorImage != null)
                return propertyGrid.Appearance.PropertyValueErrorImage;
            if (_ErrorImage == null)
                _ErrorImage = BarFunctions.LoadBitmap("SystemImages.ErrorIcon.png");
            return _ErrorImage;
        }

        private BackgroundWorker _HighlightWorker = null;
        private ElementStyle _HighlightStyle = null;
        private void StartHighlight(Color color)
        {
            if (_HighlightWorker == null)
            {
                _HighlightWorker = new BackgroundWorker();
                _HighlightWorker.WorkerSupportsCancellation = true;
                _HighlightWorker.DoWork += new DoWorkEventHandler(HighlightWorkerDoWork);
                _HighlightWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HighlightWorkerCompleted);
                if (this.Style != null)
                {
                    _HighlightStyle = this.Style.Copy();
                }
                else
                {
                    AdvTree.AdvTree tree = this.TreeControl;
                    if (tree != null && tree.NodeStyle != null)
                        _HighlightStyle = tree.NodeStyle.Copy();
                    else
                        _HighlightStyle = new ElementStyle();
                }
            }
            else if (_HighlightWorker.IsBusy)
            {
                _HighlightWorker.CancelAsync();
                return;
            }

            _HighlightStyle.BackColor = color;
            this.Style = _HighlightStyle;
            _HighlightWorker.RunWorkerAsync(this.TreeControl);
        }

        private SuperTooltip GetSuperTooltip()
        {
            AdvPropertyGrid grid = this.AdvPropertyGrid;
            if (grid != null) return grid.SuperTooltip;
            return null;
        }

        internal void SetHelpTooltip(SuperTooltip superTooltip)
        {
            string description = GetDescriptionAttributeValue();
            if (string.IsNullOrEmpty(description))
                superTooltip.SetSuperTooltip(this, null);
            else
            {
                SuperTooltipInfo info = new SuperTooltipInfo(this.Text, "", description, null, null, eTooltipColor.System, true, false, Size.Empty);
                superTooltip.SetSuperTooltip(this, info);
            }
        }

        private void HighlightWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Style = GetDefaultStyle();
            this.IsSelectionVisible = true;
        }

        private void HighlightWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 255; i > 0; i -= 25)
            {
                if (e.Cancel) break;
                Control control = e.Argument as Control;
                control.BeginInvoke(new ChangeHighlightStyleDelegate(ChangeHighlightStyle), new object[] { i });
                System.Threading.Thread.Sleep(80);
            }
        }

        private void ChangeHighlightStyle(int alpha)
        {
            _HighlightStyle.BackColor = Color.FromArgb(alpha, _HighlightStyle.BackColor.R, _HighlightStyle.BackColor.G, _HighlightStyle.BackColor.B);
        }
        private delegate void ChangeHighlightStyleDelegate(int alpha);

        protected virtual object GetValueFromString(string text, out Exception exception)
        {
            object value = null;
            exception = null;

            if (_PropertySettings != null && _PropertySettings.HasConvertFromStringToPropertyValueHandler)
            {
                ConvertValueEventArgs args = new ConvertValueEventArgs(text, null, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
                try
                {
                    _PropertySettings.InvokeConvertFromStringToPropertyValue(args);
                }
                catch (Exception e)
                {
                    exception = e;
                }
                if (args.IsConverted)
                    return args.TypedValue;
            }

            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null && grid.HasConvertFromStringToPropertyValueHandler)
            {
                ConvertValueEventArgs args = new ConvertValueEventArgs(text, null, GetPropertyName(), GetTargetComponent(), PropertyDescriptor);
                try
                {
                    grid.InvokeConvertFromStringToPropertyValue(args);
                }
                catch (Exception e)
                {
                    exception = e;
                }
                if (args.IsConverted)
                    return args.TypedValue;
            }

            TypeConverter conv = this.TypeConverter;
            try
            {
                value = conv.ConvertFromString(this, text);
            }
            catch (Exception e)
            {
                exception = e;
            }

            return value;
        }

        protected virtual void SetPropertyValue(object target, object value)
        {
            PropertyDescriptor propertyDescriptor = GetPropertyDescriptor();
            if (propertyDescriptor == null)
            {
                return;
            }

            if (_IsMultiObject)
            {
                TypeConverter converter = this.TypeConverter;
                object[] targets = (object[])target;
                foreach (object item in targets)
                {
                    if (propertyDescriptor.PropertyType.IsAssignableFrom(item.GetType()))
                        SetPropertyValueCore(item, value, propertyDescriptor);
                    else
                    {
                        PropertyDescriptor desc2 = PropertyParser.GetProperty(propertyDescriptor.Name, item, converter.GetPropertiesSupported(this) ? converter : null);
                        if (desc2 == null)
                            desc2 = PropertyParser.GetProperty(propertyDescriptor.Name, item, null);
                        SetPropertyValueCore(item, value, desc2);
                    }
                }
            }
            else
            {
                SetPropertyValueCore(target, value, propertyDescriptor);
            }
        }

        private void ValidatePropertyValue(object target, object value, PropertyDescriptor propertyDescriptor)
        {
            AdvPropertyGrid pg = this.AdvPropertyGrid;
            if (pg == null) return;
            if (!pg.HasValidatePropertyValueHandlers) return;
            ValidatePropertyValueEventArgs args = new ValidatePropertyValueEventArgs(propertyDescriptor.Name, value, target);
            pg.InvokeValidatePropertyValue(args);
            if (args.Cancel)
                throw new InvalidPropertyValueException(args.Message);            
        }

        protected virtual void SetPropertyValueCore(object target, object value, PropertyDescriptor propertyDescriptor)
        {
            ValidatePropertyValue(target, value, propertyDescriptor);

            bool useCreateInstance = false;
            PropertyNode parent = this.Parent as PropertyNode;
            if (parent != null)
            {
                TypeConverter parentConverter = parent.TypeConverter;
                if (parentConverter != null && parentConverter.GetCreateInstanceSupported(this))
                    useCreateInstance = true;
            }

            if (useCreateInstance)
            {
                if (target is ICustomTypeDescriptor)
                {
                    target = ((ICustomTypeDescriptor)target).GetPropertyOwner(propertyDescriptor);
                }

                object valueOwner = parent.GetTargetComponent();
                TypeConverter parentTypeConverter = parent.TypeConverter;
                PropertyDescriptorCollection properties = parentTypeConverter.GetProperties(parent, valueOwner);
                Dictionary<string, object> propertyValues = new Dictionary<string, object>(properties.Count);
                object newValueInstance = null;
                for (int i = 0; i < properties.Count; i++)
                {
                    string propertyName = GetPropertyName();
                    if (propertyName != null && propertyName.Equals(properties[i].Name))
                    {
                        propertyValues[properties[i].Name] = value;
                    }
                    else
                    {
                        propertyValues[properties[i].Name] = properties[i].GetValue(target);
                    }
                }
                try
                {
                    newValueInstance = parentTypeConverter.CreateInstance(parent, propertyValues);
                }
                catch (Exception exception)
                {
                    if (string.IsNullOrEmpty(exception.Message))
                    {
                        throw new TargetInvocationException("Exception Creating New Instance Of Object For Property " + this.Text + " " + parent.PropertyType.FullName + " " + exception.ToString(), exception);
                    }
                    throw;
                }
                if (newValueInstance != null)
                {
                    parent.PropertyValue = newValueInstance;
                }
            }
            else
            {
                if (target is ICustomTypeDescriptor)
                {
                    target = ((ICustomTypeDescriptor)target).GetPropertyOwner(propertyDescriptor);
                }
                try
                {
                    propertyDescriptor.SetValue(target, value);
                }
                catch
                {
                    throw;
                }
            }

            AdvPropertyGrid grid = this.AdvPropertyGrid;
            if (grid != null)
                grid.InvokePropertyValueChanged(GetPropertyName(), value, null);
        }

        private bool _IsDisposing;
        internal bool IsDisposing
        {
            get { return _IsDisposing; }
            set
            {
                _IsDisposing = value;
            }
        }

        internal bool IsDisposed
        {
            get
            {
                return _IsDisposed;
            }
        }

        private bool _IsDisposed = false;
        protected override void Dispose(bool disposing)
        {
            _IsDisposed = true;

            if (this.IsEditing)
                CancelEdit();

            BackgroundWorker worker = _HighlightWorker;
            _HighlightWorker = null;
            if (worker != null) worker.Dispose();

            if (_ErrorImage != null) _ErrorImage.Dispose();

            base.Dispose(disposing);
        }

        protected virtual AdvPropertyGrid AdvPropertyGrid
        {
            get
            {
                AdvTree.AdvTree tree = this.TreeControl;
                if (tree == null) return null;
                Control parent = tree.Parent;
                while (parent != null)
                {
                    if (parent is AdvPropertyGrid) return (AdvPropertyGrid)parent;
                    parent = parent.Parent;
                }
                return null;
            }
        }

        internal virtual void OnLoaded()
        {
            UpdateChildProperties();
            UpdatePropertySettings();
        }

        private void UpdateChildProperties()
        {
            if (HasChildProperties)
            {
                this.ExpandVisibility = eNodeExpandVisibility.Visible;
                if (this.Expanded)
                    LoadChildProperties();
            }
            else
            {
                if (this.HasChildNodes)
                {
                    AdvPropertyGrid propertyGrid = AdvPropertyGrid;
                    foreach (Node child in this.Nodes)
                        AdvPropertyGrid.ClearPropertyNode(child, propertyGrid);
                    this.Nodes.Clear();
                }
                this.Expanded = false;
                this.ExpandVisibility = eNodeExpandVisibility.Auto;
            }
        }
        public bool HasChildProperties
        {
            get
            {
                TypeConverter converter = this.TypeConverter;
                if (converter != null && converter.GetPropertiesSupported())
                {
                    PropertyDescriptorCollection childProps = PropertyParser.GetProperties(this, AttributeFilter, this.PropertyValue, converter.GetPropertiesSupported(this) ? converter : null);
                    if (childProps != null && childProps.Count > 0)
                        return true;
                }
                return false;
            }
        }
        protected virtual Attribute[] AttributeFilter
        {
            get
            {
                AdvPropertyGrid grid = AdvPropertyGrid;
                if (grid != null)
                {
                    Attribute[] attributes = new Attribute[grid.BrowsableAttributes.Count];
                    grid.BrowsableAttributes.CopyTo(attributes, 0);
                    return attributes;
                }
                return null;
            }
        }

        protected virtual void LoadChildProperties()
        {
            string lastSelectedProperty = null;
            AdvTree.AdvTree tree = this.TreeControl;
            if (tree != null)
                tree.BeginUpdate();

            try
            {
                if (this.HasChildNodes)
                {
                    if (tree != null && tree.SelectedNode != null && this.Nodes.Contains(tree.SelectedNode))
                        lastSelectedProperty = tree.SelectedNode.Text;
                }

                ClearChildNodes();

                TypeConverter converter = this.TypeConverter;
                if (converter != null && converter.GetPropertiesSupported())
                {
                    Attribute[] attributes = AttributeFilter;
                    List<string> ignoredProperties = this.IgnoredProperties;
                    List<string> ignoredCategories = this.IgnoredCategories;
                    IPropertyGridLocalizer localizer = this.PropertyGridLocalizer;
                    PropertyDescriptorCollection childProps = PropertyParser.GetProperties(this, attributes, this.PropertyValue, converter.GetPropertiesSupported(this) ? converter : null);
                    PropertyNodeFactory factory = this.NodeFactory;
                    if (childProps != null && childProps.Count > 0)
                    {
                        PropertyParser parser = new PropertyParser(
                                                    this,
                                                    this.PropertyValue,
                                                    attributes,
                                                    factory,
                                                    ignoredProperties,
                                                    ignoredCategories,
                                                    localizer,
                                                    this.PropertySettingsCollection);
                        parser.HelpType = AdvPropertyGrid.HelpType;
                        parser.TypeConverter = converter.GetPropertiesSupported(this) ? converter : null;
                        parser.Parse(this.Nodes, ePropertySort.Alphabetical, GetSuperTooltip());
                    }
                }

                if (!string.IsNullOrEmpty(lastSelectedProperty) && this.HasChildNodes && tree != null)
                {
                    foreach (Node item in this.Nodes)
                    {
                        if (item.Text == lastSelectedProperty)
                        {
                            tree.SelectedNode = item;
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (tree != null) tree.EndUpdate();
            }
        }
        private void ClearChildNodes()
        {
            if (!this.HasChildNodes) return;
            AdvPropertyGrid propertyGrid = AdvPropertyGrid;
            foreach (Node item in this.Nodes)
            {
                AdvPropertyGrid.ClearPropertyNode(item, propertyGrid);
                if (item.IsSelected && propertyGrid.PropertyTree != null)
                    propertyGrid.PropertyTree.SelectedNode = null;
            }
            this.Nodes.Clear();
        }
        internal PropertyNodeFactory NodeFactory
        {
            get
            {
                AdvPropertyGrid grid = this.AdvPropertyGrid;
                if (grid != null)
                    return grid.GetPropertyNodeFactory();
                else
                    return new PropertyNodeFactory(null);
            }
        }
        public IPropertyGridLocalizer PropertyGridLocalizer
        {
            get
            {
                return AdvPropertyGrid;
            }
        }
        public PropertySettingsCollection PropertySettingsCollection
        {
            get
            {
                AdvPropertyGrid grid = AdvPropertyGrid;
                if (grid != null) return grid.PropertySettings;
                return null;
            }
        }

        private PropertySettings _PropertySettings = null;
        /// <summary>
        /// Gets or sets the property settings that are applied to this property node.
        /// </summary>
        public PropertySettings PropertySettings
        {
            get { return _PropertySettings; }
            set
            {
                if (_PropertySettings == value) return;
                if (_PropertySettings != null)
                    _PropertySettings.PropertyChanged -= new PropertyChangedEventHandler(PropertySettingsChanged);
                _PropertySettings = value;
                if (_PropertySettings != null)
                    _PropertySettings.PropertyChanged += new PropertyChangedEventHandler(PropertySettingsChanged);
                UpdateValueInlineEditor();
            }
        }

        private void PropertySettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != PropertySettings.NamePropertyName && e.PropertyName != PropertySettings.NamePropertyDescriptor)
                UpdatePropertySettings();
            if (e.PropertyName == "ValueEditor")
                UpdateValueInlineEditor();
        }

        private bool IsCollection
        {
            get
            {
                if (_Property == null) return false;
                if (_Property.PropertyType.GetInterface("IList") != null && !_Property.PropertyType.IsArray && _Property.IsReadOnly)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Applies PropertySettings to this node.
        /// </summary>
        public virtual void UpdatePropertySettings()
        {
            bool parentCreateInstanceSupported = false;
            TypeConverter parentConverter = null;
            if (this.Parent is PropertyNode)
            {
                PropertyNode parent = (PropertyNode)this.Parent;
                parentConverter = parent.TypeConverter;
                if (parentConverter != null && parentConverter.GetCreateInstanceSupported(this))
                    parentCreateInstanceSupported = true;
            }

            if (_PropertySettings == null)
            {
                if (!this.Visible) this.Visible = true;

                bool readOnlyAtt = false;
                if (parentConverter == null | !parentCreateInstanceSupported)
                    readOnlyAtt = PropertyParser.GetReadOnlyAttribute(_Property, this.GetTargetComponent());
                if (readOnlyAtt && !IsCollection)
                    this.IsReadOnly = readOnlyAtt;
                else if (parentCreateInstanceSupported || IsUsingPropertyEditor())
                    this.IsReadOnly = false;
                
                this.Text = GetDefaultPropertyName();
                this.ImageAlignment = eCellPartAlignment.FarCenter;
                if (!_ErrorTooltipAttached)
                {
                    SuperTooltip tooltip = GetSuperTooltip();
                    if (tooltip != null)
                    {
                        tooltip.SetSuperTooltip(this, null);
                    }
                }
                return;
            }

            if (this.Visible != _PropertySettings.Visible)
                this.Visible = _PropertySettings.Visible;

            if (this.IsReadOnly != _PropertySettings.ReadOnly)
                this.IsReadOnly = _PropertySettings.ReadOnly;
            else if (!_ErrorTooltipAttached && !this.IsReadOnly)
                this.Style = GetDefaultStyle();

            if (!_ErrorTooltipAttached && this.IsReadOnly && _PropertySettings.ReadOnlyTooltip != null)
            {
                SuperTooltip tooltip = GetSuperTooltip();
                if (tooltip != null)
                {
                    tooltip.SetSuperTooltip(this, _PropertySettings.ReadOnlyTooltip);
                }
            }

            if (_PropertySettings.Image != null)
            {
                this.Image = _PropertySettings.Image;
                this.ImageAlignment = _PropertySettings.ImageAlignment;
            }
            else
                this.ImageAlignment = eCellPartAlignment.FarCenter;

            if (!string.IsNullOrEmpty(_PropertySettings.DisplayName))
                this.Text = _PropertySettings.DisplayName;
            else
                this.Text = GetDefaultPropertyName();

            if (!_ErrorTooltipAttached && _PropertySettings.Tooltip != null && !(_PropertySettings.ReadOnly && _PropertySettings.ReadOnlyTooltip != null))
            {
                SuperTooltip tooltip = GetSuperTooltip();
                if (tooltip != null)
                {
                    tooltip.SetSuperTooltip(this, _PropertySettings.Tooltip);
                }
            }
        }

        private string GetDefaultPropertyName()
        {
            string text = _Property.DisplayName;
            IPropertyGridLocalizer localizer = this.PropertyGridLocalizer;
            if (localizer != null)
            {
                string name = localizer.GetPropertyName(_Property.Name);
                if (!string.IsNullOrEmpty(name)) text = name;
            }
            return text;
        }
        public List<string> IgnoredCategories
        {
            get
            {
                List<string> categories = new List<string>();
                AdvPropertyGrid grid = AdvPropertyGrid;
                if (grid != null)
                {
                    categories.AddRange(grid.IgnoredCategories);
                }
                return categories;
            }
        }
        public List<string> IgnoredProperties
        {
            get
            {
                List<string> props = new List<string>();
                AdvPropertyGrid grid = AdvPropertyGrid;
                if (grid != null)
                {
                    props.AddRange(grid.IgnoredProperties);
                }
                return props;
            }
        }

        protected override void OnExpandChanging(bool expanded, eTreeAction action)
        {
            if (expanded)
            {
                LoadChildProperties();
            }

            base.OnExpandChanging(expanded, action);
        }

        private bool _IsMultiObject = false;
        /// <summary>
        /// Gets or sets whether property node represents property for multiple objects.
        /// </summary>
        public bool IsMultiObject
        {
            get { return _IsMultiObject; }
            internal set
            {
                _IsMultiObject = value;
            }
        }

        protected override void OnKeyboardCopy(KeyEventArgs args)
        {
            Clipboard.SetText(this.EditCell.Text);
            base.OnKeyboardCopy(args);
        }

        protected override void OnKeyboardPaste(KeyEventArgs args)
        {
            if(Clipboard.ContainsText())
            {
                ApplyEdit(Clipboard.GetText());
            }
            base.OnKeyboardPaste(args);
        }

        private bool _IsPassword = false;
        /// <summary>
        /// Gets or sets whether property is password property which value is not displayed as plain text.
        /// </summary>
        public bool IsPassword
        {
            get
            {
                return _IsPassword;
            }
            set
            {
                _IsPassword = value;
            }
        }
        #endregion

        #region ITypeDescriptorContext Members
        public virtual IComponent Component
        {
            get
            {
                object valueOwner = GetTargetComponent();
                if (valueOwner is IComponent)
                {
                    return (IComponent)valueOwner;
                }

                return null;
            }
        }

        IContainer ITypeDescriptorContext.Container
        {
            get
            {
                IComponent component = this.Component;
                if (component != null)
                {
                    ISite site = component.Site;
                    if (site != null)
                    {
                        return site.Container;
                    }
                }
                return null;
            }
        }

        object ITypeDescriptorContext.Instance
        {
            get { return GetTargetComponent(); }
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {

        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            return true;
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get { return GetPropertyDescriptor(); }
        }

        #endregion

        #region IServiceProvider Members

        object IServiceProvider.GetService(Type serviceType)
        {
            //Console.WriteLine(serviceType);
            if (serviceType == typeof(PropertyNode) || serviceType == typeof(IWindowsFormsEditorService))
            {
                return this;
            }
            AdvPropertyGrid grid = AdvPropertyGrid;
            if (grid != null)
            {
                if (grid.Site != null) return grid.Site.GetService(serviceType);
                if (grid.Parent != null && grid.Parent.Site != null) return grid.Parent.Site.GetService(serviceType);
            }
            return null;
        }

        #endregion

        #region IWindowsFormsEditorService Members

        void IWindowsFormsEditorService.CloseDropDown()
        {
            TextBoxDropDown editor = _Editor as TextBoxDropDown;
            if (editor != null && editor.IsPopupOpen)
                editor.CloseDropDown();
        }
        private bool _UITypeEditorDropDownControl = false;
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        private static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);
        void IWindowsFormsEditorService.DropDownControl(Control control)
        {
            Type type = typeof(Application);
            TextBoxDropDown editor = _Editor as TextBoxDropDown;

            MethodInfo mi = type.GetMethod("DoEventsModal", BindingFlags.Static | BindingFlags.NonPublic);
            if (mi == null || editor == null) return;

            editor.DropDownControl = control;
            _UITypeEditorDropDownControl = true;

            editor.ShowDropDown();

            while (editor != null && editor.IsPopupOpen)
            {
                mi.Invoke(null, null);
                MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 0xff, 4);
            }

            if (editor != null)
            {
                editor.ProcessMouseUpOnGroup();
                editor.DropDownControl = null;
            }
            _UITypeEditorDropDownControl = false;
        }

        DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
        {
            return dialog.ShowDialog();
        }

        #endregion
    }

    /// <summary>
    /// Defines an interface that is implemented by the control that will be used by AdvPropertyGrid control to edit property value.
    /// </summary>
    public interface IPropertyValueEditor
    {
        /// <summary>
        /// Gets or sets the font used by the edit part of the control. Font might be used to visually indicate that property value has changed. Implementing this property is optional.
        /// </summary>
        Font EditorFont { get;set;}
        /// <summary>
        /// Gets whether the edit part of the control is focused.
        /// </summary>
        bool IsEditorFocused { get;}
        /// <summary>
        /// Focus the edit part of the control.
        /// </summary>
        void FocusEditor();
        /// <summary>
        /// Gets or sets the value being edited.
        /// </summary>
        object EditValue { get;set;}
        /// <summary>
        /// Occurs when EditValue changes. Raising this even will cause the property value to be updated with the EditValue.
        /// </summary>
        event EventHandler EditValueChanged;
    }

    /// <summary>
    /// Defines exception which is thrown when property value fails the validation in AdvPropertyGrid.ValidatePropertyValue event.
    /// </summary>
    public class InvalidPropertyValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidPropertyValueException class.
        /// </summary>
        public InvalidPropertyValueException(string message): base(message)
        {
            
        }
    }
}
#endif