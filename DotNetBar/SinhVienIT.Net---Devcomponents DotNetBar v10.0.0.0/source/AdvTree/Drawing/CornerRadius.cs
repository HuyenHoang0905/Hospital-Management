using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;

namespace DevComponents.WinForms.Drawing
{
    [StructLayout(LayoutKind.Sequential), TypeConverter(typeof(CornerRadiusConverter))]
    public struct CornerRadius 
#if FRAMEWORK20
		: IEquatable<CornerRadius>
#endif
    {
        #region Private Variables
        private int _TopLeft;
        private int _topRight;
        private int _bottomLeft;
        private int _bottomRight;
        #endregion

        #region Constructor
        public CornerRadius(int uniformRadius)
        {
            this._TopLeft = this._topRight = this._bottomLeft = this._bottomRight = uniformRadius;
        }

        public CornerRadius(int topLeft, int topRight, int bottomRight, int bottomLeft)
        {
            this._TopLeft = topLeft;
            this._topRight = topRight;
            this._bottomRight = bottomRight;
            this._bottomLeft = bottomLeft;
        }
        #endregion

        #region Internal Implementation
        public override bool Equals(object obj)
        {
            if (obj is CornerRadius)
            {
                CornerRadius radius = (CornerRadius)obj;
                return (this == radius);
            }
            return false;
        }

        public bool Equals(CornerRadius cornerRadius)
        {
            return (this == cornerRadius);
        }

        public override int GetHashCode()
        {
            return (((this._TopLeft.GetHashCode() ^ this._topRight.GetHashCode()) ^ this._bottomLeft.GetHashCode()) ^ this._bottomRight.GetHashCode());
        }

        public override string ToString()
        {
            return CornerRadiusConverter.ToString(this, CultureInfo.InvariantCulture);
        }

        public static bool operator ==(CornerRadius cr1, CornerRadius cr2)
        {
            return ((cr1._TopLeft == cr2._TopLeft) && (cr1._topRight == cr2._topRight) && (cr1._bottomRight == cr2._bottomRight) && (cr1._bottomLeft == cr2._bottomLeft));
        }

        public static bool operator !=(CornerRadius cr1, CornerRadius cr2)
        {
            return !(cr1 == cr2);
        }

        public int TopLeft
        {
            get
            {
                return this._TopLeft;
            }
            set
            {
                this._TopLeft = value;
            }
        }
        public int TopRight
        {
            get
            {
                return this._topRight;
            }
            set
            {
                this._topRight = value;
            }
        }
        public int BottomRight
        {
            get
            {
                return this._bottomRight;
            }
            set
            {
                this._bottomRight = value;
            }
        }
        public int BottomLeft
        {
            get
            {
                return this._bottomLeft;
            }
            set
            {
                this._bottomLeft = value;
            }
        }

        internal bool IsValid(bool allowNegative)
        {
            if (!allowNegative && (((this._TopLeft < 0.0) || (this._topRight < 0.0)) || ((this._bottomLeft < 0.0) || (this._bottomRight < 0.0))))
            {
                return false;
            }
            return true;
        }

        internal bool IsZero
        {
            get
            {
                return (_TopLeft == 0 && _topRight == 0 && _bottomRight == 0 && _bottomLeft == 0);
            }
        }
        #endregion
    }

    #region CornerRadiusConverter
    public class CornerRadiusConverter : TypeConverter
    {
        // Methods
        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            switch (Type.GetTypeCode(sourceType))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.String:
                    return true;
            }
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
        {
            if ((destinationType != typeof(InstanceDescriptor)) && (destinationType != typeof(string)))
            {
                return false;
            }
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
        {
            if (source == null)
            {
                throw base.GetConvertFromException(source);
            }
            if (source is string)
            {
                return FromString((string)source, cultureInfo);
            }
            return new CornerRadius(Convert.ToInt32(source, cultureInfo));
        }

        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (!(value is CornerRadius))
            {
                throw new ArgumentException("Unexpected parameter type", "value");
            }
            CornerRadius cr = (CornerRadius)value;
            if (destinationType == typeof(string))
            {
                return ToString(cr, cultureInfo);
            }
            if (destinationType != typeof(InstanceDescriptor))
            {
                throw new ArgumentException("Cannot convert to type " + destinationType.FullName);
            }
            return new InstanceDescriptor(typeof(CornerRadius).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) }), new object[] { cr.TopLeft, cr.TopRight, cr.BottomRight, cr.BottomLeft });
        }

        internal static CornerRadius FromString(string s, CultureInfo cultureInfo)
        {
            string[] parsed = s.Split(GetNumericListSeparator(cultureInfo));
            int[] numArray = new int[4];
            for (int i = 0; i < parsed.Length; i++)
            {
                numArray[i] = int.Parse(parsed[i], cultureInfo);
            }

            int index = Math.Min(5, parsed.Length);
            
            switch (index)
            {
                case 1:
                    return new CornerRadius(numArray[0]);

                case 4:
                    return new CornerRadius(numArray[0], numArray[1], numArray[2], numArray[3]);
            }
            throw new FormatException("Invalid string corner radius");
        }

        internal static string ToString(CornerRadius cr, CultureInfo cultureInfo)
        {
            char numericListSeparator = GetNumericListSeparator(cultureInfo);
            StringBuilder builder = new StringBuilder(0x40);
            builder.Append(cr.TopLeft.ToString(cultureInfo));
            builder.Append(numericListSeparator);
            builder.Append(cr.TopRight.ToString(cultureInfo));
            builder.Append(numericListSeparator);
            builder.Append(cr.BottomRight.ToString(cultureInfo));
            builder.Append(numericListSeparator);
            builder.Append(cr.BottomLeft.ToString(cultureInfo));
            return builder.ToString();
        }

        internal static char GetNumericListSeparator(IFormatProvider provider)
        {
            char ch = ',';
            NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
            if ((instance.NumberDecimalSeparator.Length > 0) && (ch == instance.NumberDecimalSeparator[0]))
            {
                ch = ';';
            }
            return ch;
        }
    }
    #endregion

}
