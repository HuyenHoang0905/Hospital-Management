using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents DocumentDockContainer object converter.
	/// </summary>
	public class DocumentDockContainerConverter:TypeConverter
	{
		public DocumentDockContainerConverter(){}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType==typeof(InstanceDescriptor))
				return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if(destinationType==null)
				throw new ArgumentNullException("destinationType");

			if((destinationType==typeof(InstanceDescriptor)) && (value is DocumentDockContainer))
			{
				DocumentDockContainer doc=(DocumentDockContainer)value;
				Type[] constructorParams=null;
				MemberInfo constructorMemberInfo = null;
				object[] constructorValues = null;

				if(doc.Documents.Count==0)
				{
					constructorParams = new Type[0];
					constructorMemberInfo = typeof(DocumentDockContainer).GetConstructor(constructorParams);
					constructorValues = new object[0];
				}
				else
				{
					constructorParams = new Type[2] { typeof(DocumentBaseContainer[]), typeof(eOrientation) } ;
					constructorMemberInfo = typeof(DocumentDockContainer).GetConstructor(constructorParams);
					DocumentBaseContainer[] documentsArray = new DocumentBaseContainer[doc.Documents.Count];
					doc.Documents.CopyTo(documentsArray);
					constructorValues=new object[2] {documentsArray,doc.Orientation};
				}

				if(constructorMemberInfo!=null)
				{
					return new InstanceDescriptor(constructorMemberInfo, constructorValues);
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

	}
}
