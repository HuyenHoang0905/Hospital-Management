using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.Instrumentation.Primitives
{
    public class PointFConverter : ExpandableObjectConverter
    {
        #region CanConvertTo

        public override bool CanConvertTo(
            ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return (true);

            return (base.CanConvertTo(context, destinationType));
        }

        #endregion

        #region ConvertTo

        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                PointF pf = (PointF)value;

                return (String.Format("{0:f}, {1:f}", pf.X, pf.Y));
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }

        #endregion

        #region CanConvertFrom

        public override bool CanConvertFrom(
            ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return (true);

            return (base.CanConvertFrom(context, sourceType));
        }

        #endregion

        #region ConvertFrom

        public override object ConvertFrom(
            ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] values = ((string)value).Split(',');

                if (values.Length != 2)
                    throw new ArgumentException("Invalid value to convert.");

                try
                {
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);

                    PointF pf = new PointF(x, y);

                    return (pf);
                }
                catch (Exception exp)
                {
                    throw new ArgumentException("Invalid value to convert.");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        #endregion

        #region GetCreateInstanceSupported

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return (true);
        }

        #endregion

        #region CreateInstance

        public override object CreateInstance(
            ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues != null)
                return (new PointF((float)propertyValues["X"], (float)propertyValues["Y"]));

            return (null);
        }

        #endregion
    }
}
