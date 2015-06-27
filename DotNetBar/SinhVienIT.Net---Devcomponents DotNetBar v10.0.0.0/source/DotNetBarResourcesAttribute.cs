using System;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for DotNetBarResourcesAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class DotNetBarResourcesAttribute:System.Attribute
	{
		private string m_NamespacePrefix="";
		public DotNetBarResourcesAttribute(string namespacePrefix)
		{
			m_NamespacePrefix=namespacePrefix;
		}

		public virtual string NamespacePrefix
		{
			get {return m_NamespacePrefix;}
		}
	}
}
