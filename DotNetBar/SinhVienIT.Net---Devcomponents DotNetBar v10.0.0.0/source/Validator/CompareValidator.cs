#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Validator
{
    /// <summary>
    /// Represents compare validator for SuperValidator control used to compare two input fields or input fields to specified value.
    /// </summary>
    public class CompareValidator : ValidatorBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator(string errorMessage, string optionalValidationGroup)
        {
            this.ErrorMessage = errorMessage;
            this.OptionalValidationGroup = optionalValidationGroup;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator(string errorMessage, string optionalValidationGroup, Control controlToCompare, eValidationCompareOperator compareOperator)
        {
            this.ErrorMessage = errorMessage;
            this.OptionalValidationGroup = optionalValidationGroup;
            this.ControlToCompare = controlToCompare;
            this.Operator = compareOperator;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator(string errorMessage, Control controlToCompare, eValidationCompareOperator compareOperator)
        {
            this.ErrorMessage = errorMessage;
            this.ControlToCompare = controlToCompare;
            this.Operator = compareOperator;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator(string errorMessage, string optionalValidationGroup, object valueToCompare, eValidationCompareOperator compareOperator)
        {
            this.ErrorMessage = errorMessage;
            this.OptionalValidationGroup = optionalValidationGroup;
            this.ValueToCompare = valueToCompare;
            this.Operator = compareOperator;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator(string errorMessage, object valueToCompare, eValidationCompareOperator compareOperator)
        {
            this.ErrorMessage = errorMessage;
            this.ValueToCompare = valueToCompare;
            this.Operator = compareOperator;
        }

        /// <summary>
        /// Initializes a new instance of the RequiredFieldValidator class.
        /// </summary>
        public CompareValidator(string errorMessage, string optionalValidationGroup, Control controlToCompare, object valueToCompare, eValidationCompareOperator compareOperator)
        {
            this.ErrorMessage = errorMessage;
            this.OptionalValidationGroup = optionalValidationGroup;
            this.ControlToCompare = controlToCompare;
            this.Operator = compareOperator;
            this.ValueToCompare = valueToCompare;
        }
        #endregion

        #region Implementation
        public override bool Validate(System.Windows.Forms.Control input)
        {
            object leftValue = GetControlValue(input);
            object rightValue = null;
            if (_ControlToCompare != null)
                rightValue = GetControlValue(_ControlToCompare);
            else
                rightValue = _ValueToCompare;

            this.LastValidationResult = Compare(leftValue, rightValue, _Operator);
            return LastValidationResult;
        }

        private Control _ControlToCompare = null;
        /// <summary>
        /// Gets or sets the control to compare validated control to.
        /// </summary>
        [DefaultValue(null), Category("Behavior"), Description("Indicates control to compare validated control to.")]
        public Control ControlToCompare
        {
            get { return _ControlToCompare; }
            set { _ControlToCompare = value; }
        }

        private string _ControlToCompareValuePropertyName="";
        /// <summary>
        /// Gets or sets the Value property name for the ControlToCompare control.
        /// </summary>
        [DefaultValue(""), Category("Behavior"), Description("Indicates Value property name for the ControlToCompare control.")]
        public string ControlToCompareValuePropertyName
        {
            get { return _ControlToCompareValuePropertyName; }
            set
            {
                _ControlToCompareValuePropertyName = value;
            }
        }
        

        private object _ValueToCompare;
        /// <summary>
        /// Gets or sets the value to compare to the validation control.
        /// </summary>
        [DefaultValue((string)null), Category("Behavior"), Description("Indicates value to compare to the validation control."), TypeConverter(typeof(StringConverter))]
        public object ValueToCompare
        {
            get { return _ValueToCompare; }
            set { _ValueToCompare = value; }
        }

        private eValidationCompareOperator _Operator = eValidationCompareOperator.Equal;
        /// <summary>
        /// Gets or sets the operator used for comparison.
        /// </summary>
        [DefaultValue(eValidationCompareOperator.Equal), Category("Behavior"), Description("Indicates operator used for comparison")]
        public eValidationCompareOperator Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }

        internal static bool Compare(object leftValue, object rightValue, eValidationCompareOperator compareOperator)
        {
            if (compareOperator == eValidationCompareOperator.DataTypeCheck)
            {
                if (leftValue != null && rightValue != null)
                    return leftValue.GetType().Equals(rightValue.GetType());
                else
                    return leftValue == rightValue;
            }
            if (leftValue == null && rightValue == null) return true;
            if (leftValue == null || rightValue == null) return false;

            if (!leftValue.GetType().Equals(rightValue.GetType()) && rightValue is string)
            {
                object temp = null;
                if (ConvertRightToLeftType(leftValue, rightValue, out temp))
                    rightValue = temp;
            }

            if (!leftValue.GetType().Equals(rightValue.GetType()))
                return false;

            int compareResult = 0;

            if (leftValue is string)
                compareResult = ((string)leftValue).CompareTo(rightValue);
            else if (leftValue is int)
                compareResult = ((int)leftValue).CompareTo(rightValue);
            else if (leftValue is double)
                compareResult = ((double)leftValue).CompareTo(rightValue);
            else if (leftValue is long)
                compareResult = ((long)leftValue).CompareTo(rightValue);
            else if (leftValue is DateTime)
                compareResult = ((DateTime)leftValue).CompareTo(rightValue);
            else if (leftValue is decimal)
                compareResult = ((decimal)leftValue).CompareTo(rightValue);
            else if (leftValue is Single)
                compareResult = ((Single)leftValue).CompareTo(rightValue);
            else if (leftValue is bool)
                compareResult = ((bool)leftValue).CompareTo(rightValue);
            else if (leftValue is TimeSpan)
                compareResult = ((TimeSpan)leftValue).CompareTo(rightValue);
            else if (leftValue is byte)
                compareResult = ((byte)leftValue).CompareTo(rightValue);
            else if (leftValue is char)
                compareResult = ((char)leftValue).CompareTo(rightValue);
            else if (leftValue is Guid)
                compareResult = ((Guid)leftValue).CompareTo(rightValue);
            else
                return true;

            switch (compareOperator)
            {
                case eValidationCompareOperator.Equal:
                    return (compareResult == 0);

                case eValidationCompareOperator.NotEqual:
                    return (compareResult != 0);

                case eValidationCompareOperator.GreaterThan:
                    return (compareResult > 0);

                case eValidationCompareOperator.GreaterThanEqual:
                    return (compareResult >= 0);

                case eValidationCompareOperator.LessThan:
                    return (compareResult < 0);

                case eValidationCompareOperator.LessThanEqual:
                    return (compareResult <= 0);
            }
            return true;
        }

        internal static bool ConvertRightToLeftType(object leftValue, object rightValue, out object temp)
        {
            temp = null;
            if (rightValue == null || leftValue == null || !(rightValue is string)) return false;
            if (rightValue.GetType().Equals(leftValue.GetType()))
            {
                temp = rightValue;
                return true;
            }

            Type type = leftValue.GetType();
            string right = (string)rightValue;
            
            if (type == typeof(int))
            {
                int result=0;
                if (int.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(long))
            {
                long result = 0;
                if (long.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(double))
            {
                double result = 0;
                if (double.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(Single))
            {
                Single result = 0;
                if (Single.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(DateTime))
            {
                DateTime result;
                if (DateTime.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(TimeSpan))
            {
                TimeSpan result;
                if (TimeSpan.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(bool))
            {
                bool result;
                if (bool.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(byte))
            {
                byte result;
                if (byte.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            else if (type == typeof(char))
            {
                char result;
                if (char.TryParse(right, out result))
                {
                    temp = result;
                    return true;
                }
            }
            

            return false;
        }
        #endregion
    }
    /// <summary>
    /// Specifies the validation comparison operators used by the CompareValidator.
    /// </summary>
    public enum eValidationCompareOperator
    {
        /// <summary>
        /// A comparison for equality.
        /// </summary>
        Equal,
        /// <summary>
        /// A comparison for inequality.
        /// </summary>
        NotEqual,
        /// <summary>
        /// A comparison for greater than.
        /// </summary>
        GreaterThan,
        /// <summary>
        /// A comparison for greater than or equal to.
        /// </summary>
        GreaterThanEqual,
        /// <summary>
        /// A comparison for less than.
        /// </summary>
        LessThan,
        /// <summary>
        /// A comparison for less than or equal to.
        /// </summary>
        LessThanEqual,
        /// <summary>
        /// A comparison for data type only.
        /// </summary>
        DataTypeCheck
    }
}
#endif