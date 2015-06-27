#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DevComponents.DotNetBar.Validator
{
    /// <summary>
    /// Represents the regular expression validator used with SuperValidator control.
    /// </summary>
    public class RegularExpressionValidator : ValidatorBase
    {
        #region Implementation
        /// <summary>
        /// Initializes a new instance of the RegularExpressionValidator class.
        /// </summary>
        public RegularExpressionValidator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RegularExpressionValidator class.
        /// </summary>
        /// <param name="validationExpression"></param>
        public RegularExpressionValidator(string errorMessage, string validationExpression)
        {
            _ValidationExpression = validationExpression;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the RegularExpressionValidator class.
        /// </summary>
        /// <param name="validationExpression"></param>
        public RegularExpressionValidator(string errorMessage, string validationExpression, bool emptyValueIsValid)
        {
            _ValidationExpression = validationExpression;
            this.ErrorMessage = errorMessage;
            this.EmptyValueIsValid = emptyValueIsValid;
        }

         /// <summary>
        /// Initializes a new instance of the RegularExpressionValidator class.
        /// </summary>
        /// <param name="validationExpression"></param>
        public RegularExpressionValidator(string errorMessage, string validationExpression, string optionalValidationGroup, bool emptyValueIsValid)
        {
            _ValidationExpression = validationExpression;
            this.ErrorMessage = errorMessage;
            this.OptionalValidationGroup = optionalValidationGroup;
        }

        private string _ValidationExpression = "";
        /// <summary>
        /// Gets or sets regular expression used to validate controls value.
        /// </summary>
        [DefaultValue(""), Category("Behavior"), Description("Indicates regular expression used to validate controls value."), Editor("DevComponents.DotNetBar.Design.ValidationExpressionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string ValidationExpression
        {
            get { return _ValidationExpression; }
            set
            {
                _ValidationExpression = value;
            }
        }

        private bool _EmptyValueIsValid = false;
        /// <summary>
        /// Gets or sets whether empty value is considered valid value by the validator. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether empty value is considered valid value by the validator")]
        public bool EmptyValueIsValid
        {
            get { return _EmptyValueIsValid; }
            set
            {
                _EmptyValueIsValid = value;
            }
        }
        

        public override bool Validate(System.Windows.Forms.Control input)
        {
            if (string.IsNullOrEmpty(_ValidationExpression))
            {
                this.LastValidationResult = true;
                return true;
            }
            object value = GetControlValue(input);
            string textValue = "";

            if (value != null)
            {
                textValue = value.ToString();
            }

            if (_EmptyValueIsValid && string.IsNullOrEmpty(textValue))
            {
                this.LastValidationResult = true;
                return true;
            }

            Regex regularExpression = new Regex(_ValidationExpression);

            this.LastValidationResult = regularExpression.IsMatch(textValue);
            return LastValidationResult;
        }
        #endregion
    }
}
#endif