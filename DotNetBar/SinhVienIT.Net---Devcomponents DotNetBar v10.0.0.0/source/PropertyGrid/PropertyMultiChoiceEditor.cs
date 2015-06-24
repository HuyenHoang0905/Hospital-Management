using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines multiple choice, option or check-boxes, in-line AdvPropertyGrid property value editor.
    /// </summary>
    public class PropertyMultiChoiceEditor : PropertyValueEditor
    {
        private KeyValuePair<object, string>[] _Items = null;
        private Color _TextColor = Color.Black;
        private bool _MultiSelect = false;

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        public PropertyMultiChoiceEditor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="items"></param>
        public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items)
        {
            _Items = items;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="textColor"></param>
        public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items, Color textColor)
        {
            _Items = items;
            _TextColor = textColor;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="textColor"></param>
        public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items, Color textColor, bool multiSelect)
        {
            _Items = items;
            _TextColor = textColor;
            _MultiSelect = multiSelect;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="textColor"></param>
        public PropertyMultiChoiceEditor(KeyValuePair<object, string>[] items, bool multiSelect)
        {
            _Items = items;
            _MultiSelect = multiSelect;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="commaSeparatedItemList"></param>
        public PropertyMultiChoiceEditor(string commaSeparetedItemList)
            : this(commaSeparetedItemList, Color.Black)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="commaSeparatedItemList"></param>
        public PropertyMultiChoiceEditor(string commaSeparetedItemList, bool multiSelect)
            : this(commaSeparetedItemList, Color.Black, multiSelect)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="textColor"></param>
        public PropertyMultiChoiceEditor(string commaSeparatedItemList, Color textColor) : 
            this(commaSeparatedItemList, textColor, false)
        {
        
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="textColor"></param>
        public PropertyMultiChoiceEditor(string commaSeparatedItemList, Color textColor, bool multiSelect)
        {
            string[] items = commaSeparatedItemList.Split(',');
            _Items = new KeyValuePair<object, string>[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                _Items[i] = new KeyValuePair<object, string>(items[i], items[i]);
            }

            _TextColor = textColor;
            _MultiSelect = multiSelect;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="keys"></param>
        public PropertyMultiChoiceEditor(string[] values, object[] keys)
            : this(values, keys, Color.Black)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PropertyMultiChoiceEditor class.
        /// </summary>
        /// <param name="displayValues"></param>
        /// <param name="keys"></param>
        /// <param name="textColor"></param>
        public PropertyMultiChoiceEditor(string[] displayValues, object[] keys, Color textColor)
        {
            if (displayValues.Length != keys.Length)
                throw new ArgumentException("values.Length must be same as keys.Length");
            if (displayValues.Length == 0)
                throw new ArgumentException("values.Length must be greater than 0");

            _Items = new KeyValuePair<object, string>[displayValues.Length];
            for (int i = 0; i < displayValues.Length; i++)
            {
                _Items[i] = new KeyValuePair<object, string>(keys[i], displayValues[i]);
            }

            _TextColor = textColor;
        }


        public override IPropertyValueEditor CreateEditor(System.ComponentModel.PropertyDescriptor propertyDescriptor, object targetObject)
        {
            KeyValuePair<object, string>[] items = null;
            bool isMultiSelect = _MultiSelect;
            string selectedValue = string.Empty;
            if (_Items == null)
            {
                if (propertyDescriptor.PropertyType.IsEnum)
                {
                    string[] enumNames = Enum.GetNames(propertyDescriptor.PropertyType);
                    Array enumValue = Enum.GetValues(propertyDescriptor.PropertyType);
                    items = new KeyValuePair<object, string>[enumNames.Length];
                    for (int i = 0; i < enumValue.Length; i++)
                    {
                        items[i] = new KeyValuePair<object, string>(enumValue.GetValue(i), enumNames[i]);
                    }
                    FlagsAttribute flags = propertyDescriptor.Attributes[typeof(FlagsAttribute)] as FlagsAttribute;
                    if (flags != null) isMultiSelect = true;                        
                }
                else
                {
                    items = new KeyValuePair<object, string>[1];
                    items[0] = new KeyValuePair<object, string>(null, "Values not provided");
                }
            }
            else
                items = _Items;

            if (targetObject != null)
            {
                if (propertyDescriptor.PropertyType.IsEnum)
                    selectedValue = (string)Enum.GetName(propertyDescriptor.PropertyType, propertyDescriptor.GetValue(targetObject));
                else
                    selectedValue = (string)propertyDescriptor.GetValue(targetObject);
            }

            
            PropertyMultiChoiceItemEditor editor = new PropertyMultiChoiceItemEditor(items, isMultiSelect, _TextColor, selectedValue);
            return editor;
        }

        private class PropertyMultiChoiceItemEditor : ItemContainer, IPropertyValueEditor
        {
            private KeyValuePair<object, string>[] _ItemsDefinition = null;
            private bool _IsMultiChoice = false;
            private Color _TextColor = Color.Black;
            #region Internal Implementation
            /// <summary>
            /// Initializes a new instance of the PropertyMultiChoiceItemEditor class.
            /// </summary>
            /// <param name="itemsDefinition"></param>
            /// <param name="isMultiChoice"></param>
            public PropertyMultiChoiceItemEditor(KeyValuePair<object, string>[] itemsDefinition, bool isMultiChoice, Color textColor, string initialSelection)
            {
                _ItemsDefinition = itemsDefinition;
                _IsMultiChoice = isMultiChoice;
                _TextColor = textColor;
                CreateItems(initialSelection);
            }

            private void CreateItems(string initialSelection)
            {
                this.LayoutOrientation = eOrientation.Vertical;
                this.SubItems.Clear();
                this.ItemSpacing = 0;

                foreach (KeyValuePair<object, string> definition in _ItemsDefinition)
                {
                    CheckBoxItem item = new CheckBoxItem();
                    item.Text = definition.Value;
                    if (item.Text == initialSelection) item.Checked = true;
                    item.Tag = definition.Key;
                    item.TextColor = _TextColor;
                    if (_IsMultiChoice)
                        item.CheckBoxStyle = eCheckBoxStyle.CheckBox;
                    else
                        item.CheckBoxStyle = eCheckBoxStyle.RadioButton;
                    item.CheckedChanged += new CheckBoxChangeEventHandler(ItemCheckedChanged);
                    this.SubItems.Add(item);
                }
                this.Style = eDotNetBarStyle.StyleManagerControlled;
            }

            private void ItemCheckedChanged(object sender, CheckBoxChangeEventArgs e)
            {
                if (_SettingEditValue) return;
                OnEditValueChanged(EventArgs.Empty);
            }
            #endregion

            #region IPropertyValueEditor Members

            public System.Drawing.Font EditorFont
            {
                get
                {
                    return null;
                }
                set
                {

                }
            }

            public bool IsEditorFocused
            {
                get { return false; }
            }

            public void FocusEditor()
            {

            }

            private bool _SettingEditValue = false;
            private char StringValueSeparator = ',';
            public object EditValue
            {
                get
                {
                    object value = null;
                    foreach (CheckBoxItem item in this.SubItems)
                    {
                        if (item.Checked)
                        {
                            if (_IsMultiChoice)
                            {
                                if (value == null)
                                    value = item.Tag;
                                else if (value is string)
                                    value = (string)value + StringValueSeparator + (string)item.Tag;
                                else if (value is uint)
                                    value = (uint)value | (uint)item.Tag;
                                else if (value is int)
                                    value = (int)value | (int)item.Tag;
                                else if (value is long)
                                    value = (long)value | (long)item.Tag;
                            }
                            else
                            {
                                value = item.Tag;
                                break;
                            }
                        }
                    }
                    return value;
                }
                set
                {
                    _SettingEditValue = true;
                    List<string> valueList = null;
                    if (_IsMultiChoice)
                    {
                        if (value is string)
                            valueList = new List<string>(value.ToString().Split(StringValueSeparator));
                        else if (value is string[])
                            valueList = new List<string>((string[])value);
                    }
                    foreach (CheckBoxItem item in this.SubItems)
                    {
                        if (_IsMultiChoice)
                        {
                            if (valueList != null && valueList.Contains(item.Tag.ToString()))
                                item.Checked = true;
                        }
                        else
                        {
                            if (value == item.Tag)
                            {
                                item.Checked = true;
                                break;
                            }
                        }
                    }
                    _SettingEditValue = false;
                }
            }

            private void OnEditValueChanged(EventArgs e)
            {
                EventHandler ev = EditValueChanged;
                if (ev != null) ev(this, e);
            }
            public event EventHandler EditValueChanged;

            #endregion
        }
    }
}
