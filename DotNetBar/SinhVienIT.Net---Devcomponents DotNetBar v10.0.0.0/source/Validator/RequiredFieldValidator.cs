#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Validator
{
    /// <summary>
    /// Describes required field validator used with SuperValidator control.
    /// </summary>
    [TypeConverter("DevComponents.DotNetBar.Design.RequiredFieldValidatorConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), DesignTimeVisible(false), ToolboxItem(false), Localizable(true)]
    public class RequiredFieldValidator : ValidatorBase
    {
        #region Events
        /// <summary>
        /// Occurs when controls value needs to be evaluated to check whether it is empty. You can use this event to perform custom evaluation.
        /// </summary>
        public event EvaluateIsEmptyEventHandler EvaluateIsEmpty;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public RequiredFieldValidator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public RequiredFieldValidator(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public RequiredFieldValidator(string errorMessage, string optionalValidationGroup)
        {
            this.ErrorMessage = errorMessage;
            this.OptionalValidationGroup = optionalValidationGroup;
        }
        #endregion

        #region Implementation
        public override bool Validate(System.Windows.Forms.Control input)
        {
            object value = GetControlValue(input);
            this.LastValidationResult = !IsEmpty(input, value);
            return LastValidationResult;
        }

        protected virtual bool IsEmpty(System.Windows.Forms.Control input, object value)
        {
            EvaluateIsEmptyEventArgs args = new EvaluateIsEmptyEventArgs(input, value, this);
            OnEvaluateIsEmpty(args);
            if (args.IsEmptySet) return args.IsEmpty;

            if (value == null) return true;
            if (value is string)
            {
                if (_IsEmptyStringValid)
                    return value == null;
                else
                    return string.IsNullOrEmpty((string)value);
            }
            else if (input is ComboBox && value is int)
                return ((int)value) < 0;

            return false;
        }
        /// <summary>
        /// Raises EvaluateIsEmpty event.
        /// </summary>
        /// <param name="args">Event Arguments</param>
        protected virtual void OnEvaluateIsEmpty(EvaluateIsEmptyEventArgs args)
        {
            EvaluateIsEmptyEventHandler h = EvaluateIsEmpty;
            if (h != null) h(this, args);
        }

        private bool _IsEmptyStringValid = false;
        /// <summary>
        /// Indicates whether empty string of zero length is considered valid input.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether empty string of zero length is considered valid input.")]
        public bool IsEmptyStringValid
        {
            get { return _IsEmptyStringValid; }
            set
            {
                _IsEmptyStringValid = value;
            }
        }
        
        #endregion
    }

    #region EvaluateIsEmptyEventArgs
    public class EvaluateIsEmptyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ValidatorGetValueEventArgs class.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="validator"></param>
        public EvaluateIsEmptyEventArgs(Control control, object value, ValidatorBase validator)
        {
            Control = control;
            Validator = validator;
            Value = value;
        }

        /// <summary>
        /// Gets Control to retrieve value for.
        /// </summary>
        public readonly object Control;
        /// <summary>
        /// Gets the Value to evaluate.
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// Gets validator that is requesting value.
        /// </summary>
        public readonly ValidatorBase Validator;

        private bool _IsEmpty;
        /// <summary>
        /// Gets or sets the value that will be used by validator.
        /// </summary>
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set
            {
                _IsEmpty = value;
                IsEmptySet = true;
            }
        }
        internal bool IsEmptySet = false;
        /// <summary>
        /// Resets the Value set and indicates that validator will internally retrieve value for the control.
        /// </summary>
        public void ResetIsEmpty()
        {
            _IsEmpty = false;
            IsEmptySet = false;
        }
    }
    public delegate void EvaluateIsEmptyEventHandler(object sender, EvaluateIsEmptyEventArgs ea);
    #endregion
}
#endif