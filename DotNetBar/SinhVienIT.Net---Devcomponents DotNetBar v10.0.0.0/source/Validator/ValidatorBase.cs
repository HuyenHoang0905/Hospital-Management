#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;
using System.Runtime.Serialization;

namespace DevComponents.DotNetBar.Validator
{
    /// <summary>
    /// Represents base validator used by SuperValidator component.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false)]
    public abstract class ValidatorBase : Component
    {
        #region Events
        /// <summary>
        /// Occurs when validator retrieves the value for the control. It allows you to return value for the controls validator does not recognize.
        /// </summary>
        public event ValidatorGetValueEventHandler GetValue;
        #endregion
        #region Implementation
        private string _ErrorMessage = "";
        /// <summary>
        /// Gets or sets the error message that is displayed by error provider when validation fails.
        /// </summary>
        [DefaultValue(""), Category("Appearance"), Description("Indicates error message that is displayed when validation fails."), Localizable(true)]
        public virtual string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
            }
        }

        private bool _LastValidationResult = true;
        /// <summary>
        /// Gets the last validation result returned from Validate call. True if validation was successful or false if validation failed.
        /// </summary>
        [Browsable(false)]
        public bool LastValidationResult
        {
            get { return _LastValidationResult; }
            internal set
            {
            	_LastValidationResult = value;
            }
        }
        

        /// <summary>
        /// Validates the input control.
        /// </summary>
        /// <param name="input">Input control to validate.</param>
        /// <returns>true if validation is successful otherwise false</returns>
        public abstract bool Validate(Control input);

        /// <summary>
        /// Gets the input control value.
        /// </summary>
        /// <param name="input">Control to return value for.</param>
        /// <returns>Controls value</returns>
        protected virtual object GetControlValue(Control input)
        {
            return GetControlValue(input, _ValuePropertyName);
        }

        /// <summary>
        /// Gets the input control value.
        /// </summary>
        /// <param name="input">Control to return value for.</param>
        /// <returns>Controls value</returns>
        protected virtual object GetControlValue(Control input, string valuePropertyName)
        {
            ValidatorGetValueEventArgs args = new ValidatorGetValueEventArgs(input, this);
            OnGetValue(args);
            if (args.ValueSet) return args.Value;

            if (_SuperValidator != null)
            {
                _SuperValidator.InvokeGetValue(args);
                if (args.ValueSet) return args.Value;
            }

            if (!string.IsNullOrEmpty(valuePropertyName))
            {
                return TypeDescriptor.GetProperties(input)[valuePropertyName].GetValue(input);
            }

            if (input is ComboBox)
            {
                ComboBox box = (ComboBox)input;
                return box.SelectedIndex;
            }

            if (input is DoubleInput)
                return ((DoubleInput)input).ValueObject;
            else if(input is IntegerInput)
                return ((IntegerInput)input).ValueObject;
            else if (input is DateTimeInput)
                return ((DateTimeInput)input).ValueObject;

            object value = input.Text;
            string text = input.Text;
            long l = 0;
            int i = 0;
            double d = 0;
            if (long.TryParse(text, out l))
                value = l;
            else if (int.TryParse(text, out i))
                value = i;
            else if (double.TryParse(text, out d))
                value = d;

            return value;
        }

        private SuperValidator _SuperValidator = null;
        /// <summary>
        /// Returns SuperValidator control validator is assigned to.
        /// </summary>
        [Browsable(false)]
        public SuperValidator SuperValidator
        {
            get { return _SuperValidator; }
            internal set { _SuperValidator = value; }
        }

        /// <summary>
        /// Raises the GetValue event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnGetValue(ValidatorGetValueEventArgs e)
        {
            ValidatorGetValueEventHandler handler = GetValue;
            if (handler != null) handler(this, e);
        }

        private bool _DisplayError = true;
        /// <summary>
        /// Gets or sets whether error is displayed using the error provider on SuperValidator when validation fails. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether error is displayed using the error provider on SuperValidator when validation fails.")]
        public bool DisplayError
        {
            get { return _DisplayError; }
            set
            {
                _DisplayError = value;
            }
        }

        private bool _Enabled = true;
        /// <summary>
        /// Gets or sets whether validator is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether validator is enabled.")]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
            }
        }

        private string _OptionalValidationGroup = "";
        /// <summary>
        /// Gets or sets the group name validation belongs to. When control belongs to optional validation group the validation is considered successful when any of the controls in the group validates.
        /// </summary>
        [DefaultValue(""), Category("Behavior"), Description("Specifies group name validation belongs to. When control belongs to optional validation group the validation is considered successful when any of the controls in the group validates.")]
        public string OptionalValidationGroup
        {
            get { return _OptionalValidationGroup; }
            set
            {
                if (value == null) value = "";
                _OptionalValidationGroup = value;
            }
        }

        private eHighlightColor _HighlightColor = eHighlightColor.None;
        /// <summary>
        /// Gets or sets the highlight color for control when validation fails if Highlighter component is used on SuperValidator. Default Value is None.
        /// </summary>
        [DefaultValue(eHighlightColor.None), Category("Appearance"), Description("Indicates highlight color for control when validation fails if Highlighter component is used on SuperValidator.")]
        public eHighlightColor HighlightColor
        {
            get { return _HighlightColor; }
            set { _HighlightColor = value; }
        }

        private string _ValuePropertyName = "";
        /// <summary>
        /// Gets or sets the value property name for the control to validate.
        /// </summary>
        [DefaultValue(""), Category("Behavior"), Description("Indicates value property name for the control to validate.")]
        public string ValuePropertyName
        {
            get { return _ValuePropertyName; }
            set
            {
                _ValuePropertyName = value;
            }
        }
        
        #endregion
    }

    #region ValidatorGetValue
    public class ValidatorGetValueEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ValidatorGetValueEventArgs class.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="validator"></param>
        public ValidatorGetValueEventArgs(Control control, ValidatorBase validator)
        {
            Control = control;
            Validator = validator;
        }

        /// <summary>
        /// Gets Control to retrieve value for.
        /// </summary>
        public readonly Control Control;

        /// <summary>
        /// Gets validator that is requesting value.
        /// </summary>
        public readonly ValidatorBase Validator;

        private object _Value;
        /// <summary>
        /// Gets or sets the value that will be used by validator.
        /// </summary>
        public object Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                ValueSet = true;
            }
        }
        internal bool ValueSet = false;
        /// <summary>
        /// Resets the Value set and indicates that validator will internally retrieve value for the control.
        /// </summary>
        public void ResetValue()
        {
            _Value = null;
            ValueSet = false;
        }
    }
    public delegate void ValidatorGetValueEventHandler(object sender, ValidatorGetValueEventArgs ea);
    #endregion
}
#endif