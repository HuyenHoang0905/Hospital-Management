using System;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Describes the Elliptical Shape.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(EllipticalShapeDescriptorConverter))]
    public class EllipticalShapeDescriptor : ShapeDescriptor
    {
        public override System.Drawing.Drawing2D.GraphicsPath GetShape(Rectangle bounds)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(bounds);
            return path;
        }

        public override System.Drawing.Drawing2D.GraphicsPath GetInnerShape(Rectangle bounds, int borderSize)
        {
            return GetShape(bounds);
        }

        public override bool CanDrawShape(Rectangle bounds)
        {
            return bounds.Width > 2 && bounds.Height > 2;
        }
    }

    /// <summary>
    /// Represents EllipticalShapeDescriptor object converter.
    /// </summary>
    public class EllipticalShapeDescriptorConverter : TypeConverter
    {
        public EllipticalShapeDescriptorConverter() { }

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

            if ((destinationType == typeof(InstanceDescriptor)) && (value is EllipticalShapeDescriptor))
            {
                EllipticalShapeDescriptor doc = (EllipticalShapeDescriptor)value;
                Type[] constructorParams = null;
                MemberInfo constructorMemberInfo = null;
                object[] constructorValues = null;

                constructorParams = new Type[0];
                constructorMemberInfo = typeof(EllipticalShapeDescriptor).GetConstructor(constructorParams);
                constructorValues = new object[0];

                if (constructorMemberInfo != null)
                {
                    return new InstanceDescriptor(constructorMemberInfo, constructorValues);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
