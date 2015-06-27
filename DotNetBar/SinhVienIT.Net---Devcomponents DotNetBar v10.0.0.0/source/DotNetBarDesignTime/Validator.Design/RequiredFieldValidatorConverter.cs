#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design
{
    public class RequiredFieldValidatorConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public RequiredFieldValidatorConverter() { }

        /// <summary>
        /// Checks whether conversion can be made to specified type.
        /// </summary>
        /// <param name="context">Context Information.</param>
        /// <param name="destinationType">Destination type.</param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts object to specified type.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="culture">Culture information.</param>
        /// <param name="value">Object to convert.</param>
        /// <param name="destinationType">Destination type.</param>
        /// <returns>Object converted to destination type.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if ((destinationType == typeof(InstanceDescriptor)) && (value is RequiredFieldValidator))
            {
                RequiredFieldValidator validator = (RequiredFieldValidator)value;
                Type[] constructorParams = null;
                MemberInfo constructorMemberInfo = null;
                object[] constructorValues = null;

                if (string.IsNullOrEmpty(validator.OptionalValidationGroup))
                {
                    constructorParams = new Type[] { typeof(string) };
                    constructorMemberInfo = typeof(RequiredFieldValidator).GetConstructor(constructorParams);
                    constructorValues = new object[] { validator.ErrorMessage};
                }
                else
                {
                    constructorParams = new Type[] { typeof(string), typeof(string) };
                    constructorMemberInfo = typeof(RequiredFieldValidator).GetConstructor(constructorParams);
                    constructorValues = new object[] { validator.ErrorMessage, validator.OptionalValidationGroup };
                }
                

                if (constructorMemberInfo != null)
                {
                    return new InstanceDescriptor(constructorMemberInfo, constructorValues);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
#endif