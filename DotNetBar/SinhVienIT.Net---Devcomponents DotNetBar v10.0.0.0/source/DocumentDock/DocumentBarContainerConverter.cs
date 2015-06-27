using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents DocumentBarContainer converter.
	/// </summary>
	public class DocumentBarContainerConverter:TypeConverter
	{
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public DocumentBarContainerConverter(){}

		/// <summary>
		/// Checks whether conversion can be made to specified type.
		/// </summary>
		/// <param name="context">Context Information.</param>
		/// <param name="destinationType">Destination type.</param>
		/// <returns></returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType==typeof(InstanceDescriptor))
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
			if(destinationType==null)
				throw new ArgumentNullException("destinationType");

			if((destinationType==typeof(InstanceDescriptor)) && (value is DocumentBarContainer))
			{
				DocumentBarContainer doc=(DocumentBarContainer)value;
				Type[] constructorParams=null;
				MemberInfo constructorMemberInfo = null;
				object[] constructorValues = null;

				constructorParams = new Type[3] { typeof(Bar),typeof(int),typeof(int) } ;
				constructorMemberInfo = typeof(DocumentBarContainer).GetConstructor(constructorParams);
				constructorValues=new object[3] {doc.Bar,doc.LayoutBounds.Width,doc.LayoutBounds.Height};

				if(constructorMemberInfo!=null)
				{
					return new InstanceDescriptor(constructorMemberInfo, constructorValues);
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
