using System;
using System.ComponentModel;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents the table header.
	/// </summary>
	public class HeaderDefinition
	{
		private string m_Name="";
		private ColumnHeaderCollection m_Columns=new ColumnHeaderCollection();

		/// <summary>
		/// Default constructor.
		/// </summary>
		public HeaderDefinition()
		{
		}

		/// <summary>
		/// Gets the reference to the collection that contains the columns associated with header.
		/// </summary>
		[Browsable(true),Category("Columns"),Description("Gets the reference to the collection that contains the columns associated with header."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColumnHeaderCollection Columns
		{
			get {return m_Columns;}
		}

		/// <summary>
		/// Gets or sets the name associated with this header definition.
		/// </summary>
		[Browsable(true),Category("Design"),Description("Indicates name associated with this header definition."),DefaultValue("")]
		public string Name
		{
			get {return m_Name;}
			set {m_Name=value;}
		}
	}
}
