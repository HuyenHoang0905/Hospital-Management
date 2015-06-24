#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Validator
{
    public class CustomValidator : ValidatorBase
    {
        #region Events
        public event ValidateValueEventHandler ValidateValue;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the CustomValidator class.
        /// </summary>
        public CustomValidator()
        {
        }
        #endregion

        #region Implementation
        public override bool Validate(System.Windows.Forms.Control input)
        {
            ValidateValueEventArgs e = new ValidateValueEventArgs(input);
            OnValidateValue(e);

            if (this.SuperValidator != null)
                this.SuperValidator.InvokeCustomValidatorValidateValue(this, e);

            this.LastValidationResult = e.IsValid;
            return LastValidationResult;
        }
        protected virtual void OnValidateValue(ValidateValueEventArgs e)
        {
            ValidateValueEventHandler h = ValidateValue;
            if (h != null) h(this, e);
        }

        private object _Tag;
        /// <summary>
        /// Gets or sets custom data associated with the validator
        /// </summary>
        [DefaultValue((string)null), Localizable(false), TypeConverter(typeof(StringConverter)), Category("Data"), Description("Custom data associated with the validator")]
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        #endregion
    }
    /// <summary>
    /// Defines delegate for CustomValidator ValidateValue event.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event arguments</param>
    public delegate void ValidateValueEventHandler(object sender, ValidateValueEventArgs e);

    public class ValidateValueEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the reference to the control to validate.
        /// </summary>
        public readonly Control ControlToValidate;
        /// <summary>
        /// Gets or sets whether control's value is valid.
        /// </summary>
        public bool IsValid = false;

        /// <summary>
        /// Initializes a new instance of the ValidateValueEventArgs class.
        /// </summary>
        /// <param name="controlToValidate">Control to validate.</param>
        public ValidateValueEventArgs(Control controlToValidate)
        {
            ControlToValidate = controlToValidate;
        }
    }
}
#endif