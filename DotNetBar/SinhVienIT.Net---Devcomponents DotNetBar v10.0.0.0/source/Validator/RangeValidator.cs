#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Validator
{
    public class RangeValidator : ValidatorBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RangeValidator class.
        /// </summary>
        public RangeValidator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RangeValidator class.
        /// </summary>
        public RangeValidator(string errorMessage, object minimumValue, object maximumValue)
        {
            this.ErrorMessage = errorMessage;
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
        }

        /// <summary>
        /// Initializes a new instance of the RangeValidator class.
        /// </summary>
        public RangeValidator(string errorMessage, string optionalValidationGroup, object minimumValue, object maximumValue)
        {
            this.ErrorMessage = errorMessage;
            this.OptionalValidationGroup = optionalValidationGroup;
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
        }
        #endregion

        #region Implementation
        public override bool Validate(System.Windows.Forms.Control input)
        {
            if (_MaximumValue == null && _MinimumValue == null) return true;
            object value = GetControlValue(input);
            if (value == null)
            {
                if (_MinimumValue == null) return true;
                return false;
            }

            bool isMinimumValid = true;
            if (_MinimumValue != null)
            {
                isMinimumValid = CompareValidator.Compare(value, _MinimumValue, eValidationCompareOperator.GreaterThanEqual);
                if (!isMinimumValid) return false;
            }
            bool isMaximumValid = true;
            if (_MaximumValue != null)
            {
                isMaximumValid = CompareValidator.Compare(value, _MaximumValue, eValidationCompareOperator.LessThanEqual);
            }

            this.LastValidationResult = isMinimumValid & isMaximumValid;
            return this.LastValidationResult;
        }

        private object _MaximumValue = null;
        /// <summary>
        /// Gets or sets the maximum value control may have.
        /// </summary>
        [DefaultValue((string)null), Category("Behavior"), Description("Indicates maximum value control may have."), TypeConverter(typeof(StringConverter))]
        public object MaximumValue
        {
            get { return _MaximumValue; }
            set { _MaximumValue = value; }
        }

        private object _MinimumValue = null;
        /// <summary>
        /// Gets or sets the minimum value control may have.
        /// </summary>
        [DefaultValue((string)null), Category("Behavior"), Description("Indicates minimum value control may have."), TypeConverter(typeof(StringConverter))]
        public object MinimumValue
        {
            get { return _MinimumValue; }
            set { _MinimumValue = value; }
        }
        #endregion
    }
}
#endif