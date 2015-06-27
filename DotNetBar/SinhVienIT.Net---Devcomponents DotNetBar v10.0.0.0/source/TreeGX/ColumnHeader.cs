using System;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.Tree
{
	/// <summary>Represents the node or tree ColumnHeader.</summary>
	[ToolboxItem(false)]
	public class ColumnHeader:Component
	{
		#region Private Variables
		private string m_Text="";
		private ColumnWidth m_Width=null;
		private string m_StyleNormal="";
		private string m_StyleMouseDown="";
		private string m_StyleMouseOver="";
		private string m_ColumnName="";
		private bool m_Visible=true;
		private Rectangle m_Bounds=Rectangle.Empty;
		private bool m_SizeChanged=true;
		private string m_Name="";

		internal event EventHandler HeaderSizeChanged;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public ColumnHeader():this("")
		{
			
		}

		/// <summary>
		/// Creates new instance of the object and initalizes it with text.
		/// </summary>
		/// <param name="text">Text to initalize object with.</param>
		public ColumnHeader(string text)
		{
			m_Text=text;
			m_Width=new ColumnWidth();
			m_Width.WidthChanged+=new EventHandler(this.WidthChanged);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Makes a copy of ColumnHeader object.
		/// </summary>
		/// <returns>Returns new instance of column header object.</returns>
		public virtual ColumnHeader Copy()
		{
			ColumnHeader c=new ColumnHeader();
			c.ColumnName=this.ColumnName;
			c.StyleMouseDown=this.StyleMouseDown;
			c.StyleMouseOver=this.StyleMouseOver;
			c.StyleNormal=this.StyleNormal;
			c.Text=this.Text;
			c.Visible=this.Visible;
			c.Width.Absolute=this.Width.Absolute;
			c.Width.Relative=this.Width.Relative;

			return c;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Returns name of the column header that can be used to identify it from the code.
		/// </summary>
		[Browsable(false),Category("Design"),Description("Indicates the name used to identify column header.")]
		public string Name
		{
			get
			{
				if(this.Site!=null)
					m_Name=this.Site.Name;
				return m_Name;
			}
			set
			{
				if(this.Site!=null)
					this.Site.Name=value;
				if(value==null)
					m_Name="";
				else
					m_Name=value;
			}
		}
		/// <summary>
		/// Returns rectangle that this column occupies. If the layout has not been performed on the column the return value will be Rectangle.Empty.
		/// </summary>
		[Browsable(false)]
		public Rectangle Bounds
		{
			get {return m_Bounds;}
		}
		/// <summary>
		/// Sets the column bounds.
		/// </summary>
		internal void SetBounds(Rectangle bounds)
		{
			m_Bounds=bounds;
		}
		
		/// <summary>
		/// Gets the reference to the object that represents width of the column as either
		/// absolute or relative value.
		/// </summary>
		/// <remarks>
		/// Set Width using Absolute or Relative properties of ColumnWidth object.
		/// </remarks>
		/// <seealso cref="ColumnWidth.Absolute">Absolute Property (DevComponents.Tree.ColumnWidth)</seealso>
		/// <seealso cref="ColumnWidth.Relative">Relative Property (DevComponents.Tree.ColumnWidth)</seealso>
		[Browsable(true),Category("Layout"),Description("Gets or sets the width of the column."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColumnWidth Width
		{
			// TODO: Add Proper TypeConverter for ColumnWidth object and test design-time support
			get {return m_Width;}
		}

		/// <summary>
		/// Gets or sets the style class assigned to the column. Empty value indicates that
		/// default style is used as specified on cell's parent's control.
		/// </summary>
		/// <value>
		/// Name of the style assigned to the cell or an empty string indicating that default
		/// style setting from tree control is applied. Default is empty string.
		/// </value>
		/// <remarks>
		/// When property is set to an empty string the style setting from parent tree
		/// controls is used. ColumnStyleNormal on TreeGX control is a root style for a cell.
		/// </remarks>
		/// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
		/// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
		[Browsable(true),DefaultValue(""),Category("Style"),Description("Indicates the style class assigned to the column.")]
		public string StyleNormal
		{
			get {return m_StyleNormal;}
			set
			{
				m_StyleNormal=value;
				this.OnSizeChanged();
			}
		}

		/// <summary>
		/// Gets or sets the style class assigned to the column which is applied when mouse
		/// button is pressed over the header. Empty value indicates that default
		/// style is used as specified on column's parent.
		/// </summary>
		/// <value>
		/// Name of the style assigned to the column or an empty string indicating that default
		/// style setting from tree control is applied. Default is empty string.
		/// </value>
		/// <remarks>
		/// When property is set to an empty string the style setting from parent tree
		/// controls is used. ColumnStyleMouseDown on TreeGX control is a root style for a
		/// cell.
		/// </remarks>
		/// <seealso cref="StyleNormal">StyleNormal Property</seealso>
		/// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
		[Browsable(true),DefaultValue(""),Category("Style"),Description("Indicates the style class assigned to the column when mouse is down.")]
		public string StyleMouseDown
		{
			get {return m_StyleMouseDown;}
			set
			{
				m_StyleMouseDown=value;
				this.OnSizeChanged();
			}
		}

		/// <summary>
		/// Gets or sets the style class assigned to the column which is applied when mouse is
		/// over the column. Empty value indicates that default style is used as specified on column's
		/// parent control.
		/// </summary>
		/// <value>
		/// Name of the style assigned to the column or an empty string indicating that default
		/// style setting from tree control is applied. Default is empty string.
		/// </value>
		/// <remarks>
		/// When property is set to an empty string the style setting from parent tree
		/// controls is used. ColumnStyleMouseOver on TreeGX control is a root style for a
		/// cell.
		/// </remarks>
		/// <seealso cref="StyleNormal">StyleNormal Property</seealso>
		/// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
		[Browsable(true),DefaultValue(""),Category("Style"),Description("Indicates the style class assigned to the cell when mouse is over the column.")]
		public string StyleMouseOver
		{
			get {return m_StyleMouseOver;}
			set
			{
				m_StyleMouseOver=value;
				this.OnSizeChanged();
			}
		}

		/// <summary>
		/// Gets or sets the name of the column in the ColumnHeaderCollection.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Data"),Description("Indicates the name of the column in the ColumnHeaderCollection.")]
		public string ColumnName
		{
			get {return m_ColumnName;}
			set
			{
				m_ColumnName=value;
			}
		}

		/// <summary>
		/// Gets or sets the column caption.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Appearance"),Description("Indicates column caption.")]
		public string Text
		{
			get {return m_Text;}
			set
			{
				m_Text=value;
			}
		}

		/// <summary>
		/// Gets or sets whether column is visible. Hidding the header column will also hide coresponding data column.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether column is visible.")]
		public bool Visible
		{
			get {return m_Visible;}
			set
			{
				if(m_Visible!=value)
				{
					m_Visible=value;
				}
			}
		}
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Gets or sets whether column size has changed and it's layout needs to be recalculated.
		/// </summary>
		internal bool SizeChanged
		{
			get {return m_SizeChanged;}
			set {m_SizeChanged=value;}
		}

		private void OnSizeChanged()
		{
			m_SizeChanged=true;
			if(HeaderSizeChanged!=null)
				HeaderSizeChanged(this,new EventArgs());
		}

		private void WidthChanged(object sender, EventArgs e)
		{
			this.OnSizeChanged();
		}
		#endregion
	}
}
