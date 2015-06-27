using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents SuperTooltipInfoConverter converter.
    /// </summary>
    public class SuperTooltipInfoConverter : TypeConverter
    {
        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public SuperTooltipInfoConverter() { }

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

            if ((destinationType == typeof(InstanceDescriptor)) && (value is SuperTooltipInfo))
            {
                SuperTooltipInfo info = (SuperTooltipInfo)value;
                Type[] constructorParams = null;
                MemberInfo constructorMemberInfo = null;
                object[] constructorValues = null;

                if (info.HeaderVisible && info.FooterVisible && info.CustomSize.IsEmpty)
                {
                    constructorParams = new Type[] { typeof(string), typeof(string), typeof(string), typeof(Image), typeof(Image), typeof(eTooltipColor) };
                    constructorMemberInfo = typeof(SuperTooltipInfo).GetConstructor(constructorParams);
                    constructorValues = new object[] { info.HeaderText, info.FooterText, info.BodyText, info.BodyImage, info.FooterImage, info.Color};
                }
                else
                {
                    //(string headerText, string footerText, string bodyText, Image bodyImage, bool headerVisible, bool footerVisible, Size customSize)
                    constructorParams = new Type[] { typeof(string), typeof(string), typeof(string), typeof(Image), typeof(Image), typeof(eTooltipColor), typeof(bool), typeof(bool), typeof(Size) };
                    constructorMemberInfo = typeof(SuperTooltipInfo).GetConstructor(constructorParams);
                    constructorValues = new object[] { info.HeaderText, info.FooterText, info.BodyText, info.BodyImage, info.FooterImage, info.Color, info.HeaderVisible, info.FooterVisible, info.CustomSize };
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
