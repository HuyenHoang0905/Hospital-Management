using System;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents the converted for RibbonTabItemGroup. Used for Windows Forms designer support.
	/// </summary>
	public class RibbonTabItemGroupConverter:ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context,System.Type destinationType) 
		{
			if (destinationType == typeof(RibbonTabItemGroup))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context,CultureInfo culture, object value, System.Type destinationType) 
		{
			if (destinationType == typeof(System.String) && value is RibbonTabItemGroup)
			{

				RibbonTabItemGroup g = (RibbonTabItemGroup)value;

				return g.GroupTitle;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}


	}
}
