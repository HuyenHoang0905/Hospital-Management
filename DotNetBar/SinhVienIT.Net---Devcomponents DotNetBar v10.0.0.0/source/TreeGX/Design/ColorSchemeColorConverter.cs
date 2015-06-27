using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;

#if TREEGX
namespace DevComponents.Tree.Design
#elif DOTNETBAR
namespace DevComponents.DotNetBar.Design
#endif
{
	/// <summary>
	/// Summary description for ColorSchemeColorConverter.
	/// </summary>
	public class ColorSchemeColorConverter:ColorConverter   
	{
		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
		/// <param name="culture">The CultureInfo to use as the current culture. </param>
		/// <param name="value">The Object to convert. </param>
		/// <returns>An Object that represents the converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Color color=Color.Empty;
			if(value is string && value.ToString().StartsWith("CS."))
			{
				string propertyName=context.PropertyDescriptor.Name;
				PropertyInfo property=context.Instance.GetType().GetProperty(propertyName+"SchemePart",BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if(property!=null)
				{
					property.SetValue(context.Instance,Enum.Parse(typeof(eColorSchemePart),value.ToString().Substring(4)),new object[]{});
					color=(Color)context.PropertyDescriptor.GetValue(context.Instance);
				}
			}
			else
				return base.ConvertFrom(context, culture, value);
			
			return color;
		}
		
		/// <summary>
		/// Converts the given value object to the specified type.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
		/// <param name="culture">A CultureInfo object. If a null reference (Nothing in Visual Basic) is passed, the current culture is assumed. </param>
		/// <param name="value">The Object to convert.</param>
		/// <param name="destinationType">The Type to convert the value parameter to. </param>
		/// <returns>An Object that represents the converted value.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if(context!=null && (destinationType == typeof(string)) && (value is Color))
			{
				string propertyName=context.PropertyDescriptor.Name;
				PropertyInfo property=context.Instance.GetType().GetProperty(propertyName+"SchemePart",BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if(property!=null)
				{
					eColorSchemePart part = (eColorSchemePart)property.GetValue(context.Instance, new object[] {});
					if(part!=eColorSchemePart.None)
						return "CS." + part.ToString();
				}
			}
			return base.ConvertTo(context, culture, value, destinationType); 
		}
	}
}
