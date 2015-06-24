using System;
using System.ComponentModel;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents the width of the Column. Supports absolute width in Pixels and
	/// relative width as percentage of the width of parent control.
	/// </summary>
	public class ColumnWidth
	{
		private int m_Relative=0;
		private int m_Absolute=0;
		internal event EventHandler WidthChanged;

		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public ColumnWidth()
		{
		}

		/// <summary>
		/// Gets or sets relative width expressed as percentage between 1-100. 0 indicates that
		/// absolute width will be used.
		/// </summary>
		/// <remarks>
		/// Relative width is expressed as percentage between 1-100 of the parent controls
		/// width. 0 indicates that absolute width will be used. Absolute width always takes
		/// priority over relative width. For example value of 30 assigned to this property
		/// indicates that width of the column will be 30% of the total client width of the
		/// control.
		/// </remarks>
		[DefaultValue(0),Browsable(true),Description("Gets or sets relative width in percent. Valid values are between 1-100 with 0 indicating that absolute width will be used.")]
		public int Relative
		{
			get {return m_Relative;}
			set
			{
				if(m_Relative!=value)
				{
					m_Relative=value;
					OnSizeChanged();
				}
			}
		}

		/// <summary>Gets or sets the absolute width of the column in pixels.</summary>
		/// <remarks>
		/// Absolute width always takes precedence over the relative width of the
		/// column.
		/// </remarks>
		[DefaultValue(0),Browsable(true),Description("Gets or sets the absolute width of the column in pixels.")]
		public int Absolute
		{
			get {return m_Absolute;}
			set
			{
				if(m_Absolute<0)
					return;
				if(m_Absolute!=value)
				{
					m_Absolute=value;
					if(m_Absolute!=0)
						m_Relative=0;
					OnSizeChanged();
				}
			}
		}

		private void OnSizeChanged()
		{
			if(WidthChanged!=null)
				WidthChanged(this,new EventArgs());
		}
	}
}
