using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Drawing;

namespace DevComponents.WinForms.Drawing
{
    /// <summary>
    /// Represents BackgroundColorBlend object converter.
    /// </summary>
    public class ColorStopConverter : TypeConverter
    {
        public ColorStopConverter() { }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if ((destinationType == typeof(InstanceDescriptor)) && (value is ColorStop))
            {
                ColorStop doc = (ColorStop)value;
                Type[] constructorParams = null;
                MemberInfo constructorMemberInfo = null;
                object[] constructorValues = null;

                constructorParams = new Type[2] { typeof(Color), typeof(float) };
                constructorMemberInfo = typeof(ColorStop).GetConstructor(constructorParams);
                constructorValues = new object[2] { doc.Color, doc.Position };

                if (constructorMemberInfo != null)
                {
                    return new InstanceDescriptor(constructorMemberInfo, constructorValues);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
