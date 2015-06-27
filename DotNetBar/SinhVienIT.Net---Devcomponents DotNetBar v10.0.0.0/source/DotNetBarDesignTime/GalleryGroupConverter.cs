using System;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents converter for GalleryGroup object.
    /// </summary>
    public class GalleryGroupConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(GalleryGroup))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is GalleryGroup)
            {

                GalleryGroup g = (GalleryGroup)value;

                return g.Text;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
