using System;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents node connector. Node connector is the line that is drawn to indicate connection between child and parent node.
	/// </summary>
	[ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false),TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class NodeConnector:Component
	{
		#region Private Variables
		private int m_LineWidth=1;
		private Color m_LineColor=SystemColors.Highlight;
		private eNodeConnectorType m_ConnectorType=eNodeConnectorType.Line;
		//private bool m_UnderlineNoBorderNode=true;
		//private eConnectorCap m_EndCap=eConnectorCap.Ellipse;
		//private eConnectorCap m_StartCap=eConnectorCap.None;
		//private Size m_EndCapSize=new Size(5,5);
		//private Size m_StartCapSize=new Size(5,5);
		#endregion

		#region Events
		/// <summary>
		/// Occurs when appearance of the connector has changed as result of changed settings on the connector.
		/// </summary>
		public event EventHandler AppearanceChanged;
		#endregion

		#region Public Interface
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public NodeConnector()
		{
		}

		/// <summary>
		/// Creates new instance of the object with specified parameters.
		/// </summary>
		/// <param name="lineWidth">Connector line width.</param>
		/// <param name="type">Connector type.</param>
		public NodeConnector(int lineWidth, eNodeConnectorType type)
		{
			this.LineWidth=lineWidth;
			this.ConnectorType=type;
		}

		/// <summary>
		/// Gets or sets the connector line width.
		/// </summary>
		[Browsable(true),DefaultValue(1),Category("Appearance"),Description("Indicates connector line width.")]
		public int LineWidth
		{
			get {return m_LineWidth;}
			set 
			{
				m_LineWidth=value;
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the color of the connector line.
		/// </summary>
		[Browsable(true),Category("Appearance"),Description("Indicates color of the connector line.")]
		public Color LineColor
		{
			get {return m_LineColor;}
			set
			{
				m_LineColor=value;
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Returns true if editor should serialize LineColor property.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeLineColor()
		{
			return m_LineColor!=SystemColors.Highlight;
		}

		/// <summary>
		/// Gets or sets the type of the connector.
		/// </summary>
		/// <remarks>
		/// See <see cref="eNodeConnectorType">eNodeConnectorType</see> enum for list of
		/// available connectors.
		/// </remarks>
		/// <seealso cref="eNodeConnectorType">eNodeConnectorType Enumeration</seealso>
		[Browsable(false),DefaultValue(eNodeConnectorType.Line),Category("Appearance"),Description("Indicates visual type of the connector.")]
		public eNodeConnectorType ConnectorType
		{
			get {return m_ConnectorType;}
			set 
			{
				m_ConnectorType=value;
				OnAppearanceChanged();
			}
		}

        private DashStyle _DashStyle = DashStyle.Dot;
        /// <summary>
        /// Gets or sets the DashStyle for the connector line. Default value is DashStyle.Dot.
        /// </summary>
        [DefaultValue(DashStyle.Dot), Category("Appearance"), Description("Indicates DashStyle for the connector line")]
        public DashStyle DashStyle
        {
            get { return _DashStyle; }
            set { _DashStyle = value; }
        }

        ///// <summary>
        ///// Gets or sets whether the child node without borders is underlined as a
        ///// continuation of the connector from node's parent. Default value is true.
        ///// </summary>
        ///// <remarks>
        ///// To enhance visual appearance of the connectors that are connecting to the node
        ///// with no borders assigned the connector is continued as a single line under the node
        ///// when this property is set to true (default) value.
        ///// </remarks>
        //[Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether connector is drawn under the nodes with no borders assigned.")]
        //public bool UnderlineNoBorderNode
        //{
        //    get { return m_UnderlineNoBorderNode; }
        //    set
        //    {
        //        m_UnderlineNoBorderNode = value;
        //        OnAppearanceChanged();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the type of the cap that connector is ended with. Note that connector starts with parent node and ends with the child node. Default value is Ellipse.
        ///// </summary>
        //[Browsable(true),DefaultValue(eConnectorCap.Ellipse),Category("Appearance"),Description("Indicates type of the cap that connector is ended with.")]
        //public eConnectorCap EndCap
        //{
        //    get {return m_EndCap;}
        //    set
        //    {
        //        m_EndCap=value;
        //        OnAppearanceChanged();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the size of the end cap.
        ///// </summary>
        //[Browsable(true),Category("Appearance"),Description("Indicates the size of the end cap.")]
        //public System.Drawing.Size EndCapSize
        //{
        //    get {return m_EndCapSize;}
        //    set
        //    {
        //        m_EndCapSize=value;
        //        OnAppearanceChanged();
        //    }
        //}

        ///// <summary>
        ///// Returns true if EndCapSize property should be serialized by editor.
        ///// </summary>
        ///// <returns></returns>
        //[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeEndCapSize()
        //{
        //    return (m_EndCapSize.Width!=5 || m_EndCapSize.Height!=5);
        //}
		#endregion

		#region Private Implementation
		private void OnAppearanceChanged()
		{
			if(AppearanceChanged!=null)
				AppearanceChanged(this,new EventArgs());
		}
		#endregion

//		/// <summary>
//		/// Gets or sets the type of the cap that connector is started with. Note that connector starts with parent node and ends with the child node.  Default value is None.
//		/// </summary>
//		[Browsable(true),DefaultValue(eConnectorCap.None),Category("Appearance"),Description("Indicates type of the cap that connector is starts with.")]
//		public eConnectorCap StartCap
//		{
//			get {return m_StartCap;}
//			set {m_StartCap=value;}
//		}
//
//		/// <summary>
//		/// Gets or sets the size of the start cap.
//		/// </summary>
//		[Browsable(true),Category("Appearance"),Description("Indicates the size of the start cap.")]
//		public System.Drawing.Size StartCapSize
//		{
//			get {return m_StartCapSize;}
//			set {m_StartCapSize=value;}
//		}
//
//		/// <summary>
//		/// Returns true if StartCapSize property should be serialized by editor.
//		/// </summary>
//		/// <returns></returns>
//		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
//		public bool ShouldSerializeStartCapSize()
//		{
//			return (m_StartCapSize.Width!=5 || m_StartCapSize.Height!=5);
//		}
	}
}
