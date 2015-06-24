using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.Editors;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines a attribute which applies an integer type numeric editor to a property when used with AdvPropertyGrid control. Applies to DateTime property types only.
    /// </summary>
    public class PropertyIntegerEditor : PropertyValueEditor
    {
        #region Implementation
        /// <summary>
        /// Gets or sets whether up/down button is shown.
        /// </summary>
        public bool ShowUpDownButton = true;
        /// <summary>
        /// Gets or sets the display format for the control when control does not have input focus.
        /// </summary>
        public string DisplayFormat = "";
        /// <summary>
        /// Gets or sets the minimum value that can be entered.
        /// </summary>
        public int MinValue = int.MinValue;
        /// <summary>
        /// Gets or sets the maximum value that can be entered.
        /// </summary>
        public int MaxValue = int.MaxValue;

        /// <summary>
        /// Initializes a new instance of the PropertyIntegerEditor class.
        /// </summary>
        public PropertyIntegerEditor()
        {

        }

        /// <summary>
        /// Initializes a new instance of the PropertyIntegerEditor class.
        /// </summary>
        /// <param name="showUpDownButton"></param>
        public PropertyIntegerEditor(bool showUpDownButton)
        {
            ShowUpDownButton = showUpDownButton;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyIntegerEditor class.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public PropertyIntegerEditor(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyIntegerEditor class.
        /// </summary>
        /// <param name="showUpDownButton"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public PropertyIntegerEditor(bool showUpDownButton, int minValue, int maxValue)
        {
            ShowUpDownButton = showUpDownButton;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyIntegerEditor class.
        /// </summary>
        /// <param name="showUpDownButton"></param>
        /// <param name="displayFormat"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public PropertyIntegerEditor(bool showUpDownButton, string displayFormat, int minValue, int maxValue)
        {
            ShowUpDownButton = showUpDownButton;
            DisplayFormat = displayFormat;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyIntegerEditor class.
        /// </summary>
        /// <param name="showUpDownButton"></param>
        /// <param name="displayFormat"></param>
        public PropertyIntegerEditor(bool showUpDownButton, string displayFormat)
        {
            ShowUpDownButton = showUpDownButton;
            DisplayFormat = displayFormat;
        }

        public override IPropertyValueEditor CreateEditor(System.ComponentModel.PropertyDescriptor propertyDescriptor, object targetObject)
        {
            if (propertyDescriptor.PropertyType != typeof(int) && propertyDescriptor.PropertyType != typeof(int?) &&
                propertyDescriptor.PropertyType != typeof(byte) && propertyDescriptor.PropertyType != typeof(byte?))
                throw new InvalidOperationException("PropertyIntegerEditor works only with int or byte type properties");

            IntegerValueEditor editor = new IntegerValueEditor();
            editor.AutoBorderSize = 1;

            if (!string.IsNullOrEmpty(DisplayFormat))
            {
                editor.DisplayFormat = DisplayFormat;
            }
            editor.ShowUpDown = ShowUpDownButton;
            editor.Height = 14;
            editor.BackgroundStyle.Class = "";
            editor.BackgroundStyle.BackColor = Color.White;
            if (propertyDescriptor.PropertyType == typeof(byte) || propertyDescriptor.PropertyType == typeof(byte?))
            {
                editor.MinValue = Math.Max(byte.MinValue, MinValue);
                editor.MaxValue = Math.Min(byte.MaxValue, MaxValue);
            }
            else
            {
                editor.MinValue = MinValue;
                editor.MaxValue = MaxValue;
            }

            return editor;
        }
        #endregion

        private class IntegerValueEditor : IntegerInput, IPropertyValueEditor
        {
            #region IPropertyValueEditor Members

            public System.Drawing.Font EditorFont
            {
                get
                {
                    return this.Font;
                }
                set
                {
                    this.Font = value;
                }
            }

            public bool IsEditorFocused
            {
                get { return this.Focused; }
            }

            public void FocusEditor()
            {
                this.Focus();
            }

            public object EditValue
            {
                get
                {
                    return this.Value;
                }
                set
                {
                    if (value == null)
                        this.ValueObject = null;
                    else
                        this.Value = (int)value;
                }
            }

            protected override void OnValueChanged(EventArgs e)
            {
                OnEditValueChanged(e);
                base.OnValueChanged(e);
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
