#if FRAMEWORK20
using System;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    internal class FormatHelper
    {
        private static Type stringType;
        private static Type checkStateType;
        private static Type booleanType;
 
        /// <summary>
        /// Initializes FormatHelper class.
        /// </summary>
        static FormatHelper()
        {
            stringType = typeof(string);
            booleanType = typeof(bool);
            checkStateType = typeof(CheckState);
            //parseMethodNotFound = new object();
            //defaultDataSourceNullValue = DBNull.Value;
        }
        internal static object FormatObject(object value, Type targetType, TypeConverter sourceConverter, TypeConverter targetConverter, string formatString, IFormatProvider formatInfo, object formattedNullValue, object dataSourceNullValue)
        {
            if (IsNullData(value, dataSourceNullValue))
            {
                value = DBNull.Value;
            }
            Type type = targetType;
            targetType = NullableUnwrap(targetType);
            sourceConverter = NullableUnwrap(sourceConverter);
            targetConverter = NullableUnwrap(targetConverter);
            bool flag = targetType != type;
            object obj2 = FormatObjectInternal(value, targetType, sourceConverter, targetConverter, formatString, formatInfo, formattedNullValue);
            if ((type.IsValueType && (obj2 == null)) && !flag)
            {
                throw new FormatException(GetCantConvertMessage(value, targetType));
            }
            return obj2;
        }

        private static string GetCantConvertMessage(object value, Type targetType)
        {
            string name = (value == null) ? "Can't convert null value" : "Specified format cannot be converted";
            return string.Format(CultureInfo.CurrentCulture, name, new object[] { value, targetType.Name });
        }

 
        private static object FormatObjectInternal(object value, Type targetType, TypeConverter sourceConverter, TypeConverter targetConverter, string formatString, IFormatProvider formatInfo, object formattedNullValue)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                if (formattedNullValue != null)
                {
                    return formattedNullValue;
                }
                if (targetType == stringType)
                {
                    return string.Empty;
                }
                if (targetType == checkStateType)
                {
                    return CheckState.Indeterminate;
                }
                return null;
            }
            if (((targetType == stringType) && (value is IFormattable)) && !string.IsNullOrEmpty(formatString))
            {
                return (value as IFormattable).ToString(formatString, formatInfo);
            }
            Type type = value.GetType();
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (((sourceConverter != null) && (sourceConverter != converter)) && sourceConverter.CanConvertTo(targetType))
            {
                return sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, targetType);
            }
            TypeConverter converter2 = TypeDescriptor.GetConverter(targetType);
            if (((targetConverter != null) && (targetConverter != converter2)) && targetConverter.CanConvertFrom(type))
            {
                return targetConverter.ConvertFrom(null, GetFormatterCulture(formatInfo), value);
            }
            if (targetType == checkStateType)
            {
                if (type == booleanType)
                {
                    return (((bool)value) ? CheckState.Checked : CheckState.Unchecked);
                }
                if (sourceConverter == null)
                {
                    sourceConverter = converter;
                }
                if ((sourceConverter != null) && sourceConverter.CanConvertTo(booleanType))
                {
                    return (((bool)sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, booleanType)) ? CheckState.Checked : CheckState.Unchecked);
                }
            }
            if (targetType.IsAssignableFrom(type))
            {
                return value;
            }
            if (sourceConverter == null)
            {
                sourceConverter = converter;
            }
            if (targetConverter == null)
            {
                targetConverter = converter2;
            }
            if ((sourceConverter != null) && sourceConverter.CanConvertTo(targetType))
            {
                return sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, targetType);
            }
            if ((targetConverter != null) && targetConverter.CanConvertFrom(type))
            {
                return targetConverter.ConvertFrom(null, GetFormatterCulture(formatInfo), value);
            }
            if (!(value is IConvertible))
            {
                throw new FormatException(GetCantConvertMessage(value, targetType));
            }
            return ChangeType(value, targetType, formatInfo);
        }
        private static CultureInfo GetFormatterCulture(IFormatProvider formatInfo)
        {
            if (formatInfo is CultureInfo)
            {
                return (formatInfo as CultureInfo);
            }
            return CultureInfo.CurrentCulture;
        }
        private static object ChangeType(object value, Type type, IFormatProvider formatInfo)
        {
            object obj2;
            try
            {
                if (formatInfo == null)
                {
                    formatInfo = CultureInfo.CurrentCulture;
                }
                obj2 = Convert.ChangeType(value, type, formatInfo);
            }
            catch (InvalidCastException exception)
            {
                throw new FormatException(exception.Message, exception);
            }
            return obj2;
        }

        private static Type NullableUnwrap(Type type)
        {
            if (type == stringType)
            {
                return stringType;
            }
            return (Nullable.GetUnderlyingType(type) ?? type);
        }

        private static TypeConverter NullableUnwrap(TypeConverter typeConverter)
        {
            NullableConverter converter = typeConverter as NullableConverter;
            if (converter == null)
            {
                return typeConverter;
            }
            return converter.UnderlyingTypeConverter;
        }

        internal static bool IsNullData(object value, object dataSourceNullValue)
        {
            if ((value != null) && (value != DBNull.Value))
            {
                return object.Equals(value, NullData(value.GetType(), dataSourceNullValue));
            }
            return true;
        }

        internal static object NullData(Type type, object dataSourceNullValue)
        {
            if (!type.IsGenericType || (type.GetGenericTypeDefinition() != typeof(Nullable<>)))
            {
                return dataSourceNullValue;
            }
            if ((dataSourceNullValue != null) && (dataSourceNullValue != DBNull.Value))
            {
                return dataSourceNullValue;
            }
            return null;
        }
    }
}
#endif