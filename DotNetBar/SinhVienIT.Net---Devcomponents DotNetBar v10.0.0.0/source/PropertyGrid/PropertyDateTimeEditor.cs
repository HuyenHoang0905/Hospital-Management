using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines a attribute which applies an date-time editor to a property when used with AdvPropertyGrid control. Applies to DateTime property types only.
    /// </summary>
    public class PropertyDateTimeEditor : PropertyValueEditor
    {
        #region Implementation
        /// <summary>
        /// Gets or sets pre-defined format for date-time input.
        /// </summary>
        public eDateTimePickerFormat Format = eDateTimePickerFormat.Short;
        /// <summary>
        /// Gets or sets custom format for date-time input.
        /// </summary>
        public string CustomFormat = "";
        /// <summary>
        /// Gets or sets whether empty null/nothing state of the control is allowed. Default value is false.
        /// </summary>
        public bool AllowEmptyState = false;
        /// <summary>
        /// Gets or sets whether drop-down button that shows calendar is visible. Default value is true.
        /// </summary>
        public bool ShowDropDownButton = true;
        /// <summary>
        /// Gets or sets the minimum date that control accepts.
        /// </summary>
        public DateTime MinDate = DateTimeGroup.MinDateTime;
        /// <summary>
        /// Gets or sets the maximum date that control accepts.
        /// </summary>
        public DateTime MaxDate = DateTimeGroup.MaxDateTime;

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        /// <param name="format"></param>
        public PropertyDateTimeEditor(eDateTimePickerFormat format)
        {
            Format = format;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        public PropertyDateTimeEditor(DateTime minDate, DateTime maxDate)
        {
            MinDate = minDate;
            MaxDate = maxDate;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        public PropertyDateTimeEditor(eDateTimePickerFormat format, DateTime minDate, DateTime maxDate)
        {
            Format = format;
            MinDate = minDate;
            MaxDate = maxDate;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="customFormat"></param>
        /// <param name="allowEmptyState"></param>
        /// <param name="showDropDownButton"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        public PropertyDateTimeEditor(eDateTimePickerFormat format, string customFormat, bool allowEmptyState, bool showDropDownButton, DateTime minDate, DateTime maxDate)
        {
            Format = format;
            CustomFormat = customFormat;
            AllowEmptyState = allowEmptyState;
            ShowDropDownButton = showDropDownButton;
            MinDate = minDate;
            MaxDate = maxDate;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="allowEmptyState"></param>
        public PropertyDateTimeEditor(eDateTimePickerFormat format, bool allowEmptyState)
        {
            Format = format;
            AllowEmptyState = allowEmptyState;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        /// <param name="customFormat"></param>
        public PropertyDateTimeEditor(string customFormat)
        {
            CustomFormat = customFormat;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        /// <param name="customFormat"></param>
        /// <param name="allowEmptyState"></param>
        public PropertyDateTimeEditor(string customFormat, bool allowEmptyState)
        {
            CustomFormat = customFormat;
            AllowEmptyState = allowEmptyState;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyDateTimeEditor class.
        /// </summary>
        public PropertyDateTimeEditor()
        {
            
        }

        public override IPropertyValueEditor CreateEditor(System.ComponentModel.PropertyDescriptor propertyDescriptor, object targetObject)
        {
            if (propertyDescriptor.PropertyType != typeof(DateTime) && propertyDescriptor.PropertyType != typeof(DateTime?))
                throw new InvalidOperationException("PropertyDateTimeEditor works only with DateTime type properties");

            DateTimeValueEditor editor = new DateTimeValueEditor();
            editor.AutoBorderSize = 1;

            if (!string.IsNullOrEmpty(CustomFormat))
            {
                editor.Format = eDateTimePickerFormat.Custom;
                editor.CustomFormat = CustomFormat;
            }
            else
                editor.Format = Format;
            editor.AllowEmptyState = AllowEmptyState;
            editor.BackgroundStyle.Class = "";
            editor.BackgroundStyle.BackColor = Color.White;
            editor.MinDate = this.MinDate;
            editor.MaxDate = this.MaxDate;
            
            if (AllowEmptyState)
            {
                editor.ButtonClear.Visible = true;
                editor.ButtonDropDown.DisplayPosition = 1;
            }
            editor.Height = 14;
            editor.ButtonDropDown.Visible = ShowDropDownButton;

            return editor;
        }
        #endregion

        #region DateTimeValueEditor
        private class DateTimeValueEditor : DateTimeInput, IPropertyValueEditor
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
                    return this.ValueObject;
                }
                set
                {
                    if (value == null)
                        this.ValueObject = null;
                    else if (this.Value != (DateTime)value)
                        this.Value = (DateTime)value;
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
        #endregion
    }
}
