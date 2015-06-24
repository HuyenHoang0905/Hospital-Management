using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.Tree.Display;

namespace DevComponents.Tree
{
	/// <summary>Represents a cell assigned to the Node.</summary>
	/// <remarks>
	/// 	<para>The Cell with Index 0 (zero) always exists for a Node and it is a cell that
	///     you can interact with through the properties on a node, which are forwarding to the
	///     Cell(0), or you can go directly to the Cell(0).</para>
	/// 	<para>When Node has multiple columns defined each column corresponds to Cell in
	///     Node's Cells collection. The first Column has Index 0, second Column Index 1 and so
	///     forth.</para>
	/// 	<para>Note that there is always at least one Cell in a Node even if multiple
	///     columns are not used.</para>
	/// </remarks>
	[DesignTimeVisible(false),ToolboxItem(false)]
	public class Cell:Component
	{
		#region Private Variables
		private ElementStyle m_StyleNormal=null;
		private ElementStyle m_StyleDisabled=null;
		private ElementStyle m_StyleMouseDown=null;
		private ElementStyle m_StyleMouseOver=null;
		private ElementStyle m_StyleSelected=null;
		private bool m_Enabled=true;
		private CellImages m_Images=null;
		private Rectangle m_Bounds=Rectangle.Empty;
		private Rectangle m_TextBounds=Rectangle.Empty;
		private Rectangle m_TextContentBounds=Rectangle.Empty;
		private Rectangle m_ImageBounds=Rectangle.Empty;
		private Rectangle m_CheckBoxBounds=Rectangle.Empty;
		private object m_Tag=null;
		private string m_Text="";
		private eCellPartAlignment m_ImageAlignment=eCellPartAlignment.NearCenter;
		private eCellPartAlignment m_CheckBoxAlignment=eCellPartAlignment.NearCenter;
		private bool m_CheckBoxVisible=false;
		private bool m_Checked=false;
		private bool m_MouseDown=false;
		private bool m_MouseOver=false;
		private bool m_Selected=false;
		private Node m_Parent=null;
		private bool m_Visible=true;
		private eTreeAction m_ActionSource=eTreeAction.Code;
		private eCellPartLayout m_Layout=eCellPartLayout.Default;
		private System.Windows.Forms.Cursor m_Cursor=null;
		private bool m_WordWrap=false;
		private string m_Name="";
		private Control m_HostedControl=null;
		private Size m_HostedControlSize=Size.Empty;
		private bool m_IgnoreHostedControlSizeChange=false;
		#endregion

		#region Constructor/Dispose
		/// <summary>
		/// Initializes new instance of Cell class.
		/// </summary>
		public Cell():this("")
		{
		}

		/// <summary>
		/// Initializes new instance of Cell class.
		/// </summary>
		/// <param name="text">Cell text.</param>
		public Cell(string text)
		{
			m_Text=text;
			m_Images=new CellImages(this);
		}

		/// <summary>
		/// Releases the resources used by the Component.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if(m_HostedControl!=null)
				m_HostedControl.SizeChanged-=new EventHandler(this.HostedControlSizedChanged);
			m_HostedControl=null;
			base.Dispose(disposing);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the control hosted inside of the cell.
		/// </summary>
		/// <remarks>
		/// 	<para>When control is hosted inside of the cell, cell size is determined by the
		///     size of the control hosted inside of it. The cell will not display its text but it will display any image assigned
		///     or check box when control is hosted inside of it. The Style settings like Margin
		///     and Padding will still apply.</para>
		/// </remarks>
		[Browsable(true),Category("Behavior"),Description("Indicates control hosted inside of the cell."),DefaultValue(null)]
		public Control HostedControl
		{
			get {return m_HostedControl;}
			set
			{
				SetHostedControl(value);
			}
		}
		
		/// <summary>
		/// Gets or sets whether hosted control size change event is ignored.
		/// </summary>
		internal bool IgnoreHostedControlSizeChange
		{
			get { return m_IgnoreHostedControlSizeChange;}
			set { m_IgnoreHostedControlSizeChange=value; }
		}
		
		/// <summary>
		/// Gets or sets the hosted control size. Property is used to correctly scale control when TreeGX.Zoom is used to zoom view.
		/// </summary>
		internal Size HostedControlSize
		{
			get { return m_HostedControlSize;}
			set { m_HostedControlSize = value;}	
		}

		/// <summary>
		/// Returns name of the cell that can be used to identify it from the code.
		/// </summary>
		[Browsable(false),Category("Design"),Description("Indicates the name used to identify cell.")]
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
		/// Gets the relative bounds of the cell.
		/// </summary>
		[Browsable(false)]
		internal Rectangle BoundsRelative
		{
			get
			{
				return m_Bounds;			
			}
		}
		
		/// <summary>
		/// Gets the bounds of the cell.
		/// </summary>
		[Browsable(false)]
		public Rectangle Bounds
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					return NodeDisplay.GetCellRectangle(eCellRectanglePart.CellBounds, this, tree.NodeDisplay.Offset);
				}
				return Rectangle.Empty;			
			}
		}

		/// <summary>
		/// Sets the bounds of the cell.
		/// </summary>
		/// <param name="bounds">New cell bounds.</param>
		internal void SetBounds(Rectangle bounds)
		{
			m_Bounds=bounds;
		}

//		/// <summary>
//		/// Gets the bounds of the text inside of cell.
//		/// </summary>
//		[Browsable(false)]
//		public Rectangle TextBounds
//		{
//			get
//			{
//				return m_TextBounds;			
//			}
//		}

//		/// <summary>
//		/// Sets the bounds of the text inside of the cell.
//		/// </summary>
//		/// <param name="bounds">New cell bounds.</param>
//		internal void SetTextBounds(Rectangle bounds)
//		{
//			m_TextBounds=bounds;
//		}

		/// <summary>
		/// Gets or sets the available content bounds for the text. Text will fitted into these bounds
		/// but it's true location can be obtained only after it is displayed.
		/// </summary>
		internal Rectangle TextContentBounds
		{
			get {return m_TextContentBounds;}
			set {m_TextContentBounds=value;}
		}

		/// <summary>
		/// Gets the relative bounds of the image inside of cell.
		/// </summary>
		[Browsable(false)]
		internal Rectangle ImageBoundsRelative
		{
			get
			{
				return m_ImageBounds;			
			}
		}
		
		/// <summary>
		/// Gets the bounds of the image inside of cell.
		/// </summary>
		[Browsable(false)]
		public Rectangle ImageBounds
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					return NodeDisplay.GetCellRectangle(eCellRectanglePart.ImageBounds, this, tree.NodeDisplay.Offset);
				}
				return Rectangle.Empty;			
			}
		}

		/// <summary>
		/// Sets the bounds of the image inside of the cell.
		/// </summary>
		/// <param name="bounds">New cell bounds.</param>
		internal void SetImageBounds(Rectangle bounds)
		{
			m_ImageBounds=bounds;
		}

		/// <summary>
		/// Gets the bounds of the image inside of cell.
		/// </summary>
		[Browsable(false)]
		internal Rectangle CheckBoxBoundsRelative
		{
			get
			{
				return m_CheckBoxBounds;			
			}
		}
		
		/// <summary>
		/// Gets the bounds of the image inside of cell.
		/// </summary>
		[Browsable(false)]
		internal Rectangle CheckBoxBounds
		{
			get
			{
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					return NodeDisplay.GetCellRectangle(eCellRectanglePart.CheckBoxBounds, this, tree.NodeDisplay.Offset);
				}
				return Rectangle.Empty;			
			}
		}

		/// <summary>
		/// Sets the bounds of the check box inside of the cell.
		/// </summary>
		/// <param name="bounds">New cell bounds.</param>
		internal void SetCheckBoxBounds(Rectangle bounds)
		{
			m_CheckBoxBounds=bounds;
		}

		/// <summary>
		/// Gets a value indicating whether the cell is in an editable state. true if the cell is in editable state; otherwise, false.
		/// </summary>
		[Browsable(false)]
		public bool IsEditing
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the cell is in the selected state. true if the cell is in the selected state; otherwise, false.
		/// </summary>
		[Browsable(false)]
		public bool IsSelected
		{
			get
			{
				return m_Selected;
			}
		}

		internal void SetSelected(bool selected)
		{
			m_Selected=selected;
		}

		/// <summary>
		/// Gets a value indicating whether the cell is visible. Cell is considered to be visible when it's Bounds are within the display rectangle of tree.
		/// </summary>
		[Browsable(false)]
		public bool IsVisible
		{
			get
			{
				return m_Visible;
			}
		}

		/// <summary>
		/// Sets whether cells is visible or not. This is set by node layout manager and it is based on column visibility.
		/// </summary>
		/// <param name="visible">True if visible otherwise false.</param>
		internal void SetVisible(bool visible)
		{
			if(m_Visible!=visible)
			{
				m_Visible=visible;
				OnVisibleChanged();
			}
		}

		/// <summary>
		/// Gets the parent node of the current cell.
		/// </summary>
		[Browsable(false)]
		public Node Parent
		{
			get
			{
				return m_Parent;
			}
		}

		/// <summary>
		/// Sets the parent of the cell.
		/// </summary>
		/// <param name="parent">Parent node.</param>
		internal void SetParent(Node parent)
		{
			m_Parent=parent;
		}

		/// <summary>
		/// Gets or sets the object that contains data about the cell. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
		/// </summary>
		[Browsable(false),DefaultValue(null),Category("Data"),Description("Indicates text that contains data about the cell.")]
		public object Tag
		{
			get
			{
				return m_Tag;
			}
			set
			{
				m_Tag=value;
			}
		}

		/// <summary>
		/// Gets or sets the object that contains data about the cell. Any Object derived type can be assigned to this property. If this property is being set through the Windows Forms designer, only text can be assigned.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Data"),Description("Indicates text that contains data about the cell."),DevCoSerialize()]
		public string TagString
		{
			get
			{
				if(m_Tag==null)
					return "";
				return m_Tag.ToString();
			}
			set
			{
				m_Tag=value;
			}
		}

		/// <summary>
		/// Gets or sets the text displayed in the cell.
		/// </summary>
		[Browsable(true),DefaultValue(""),Category("Appearance"),Localizable(true),Description("Indicates text displayed in the cell."), DevCoSerialize()]
		public string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if(m_Text!=value)
				{
					m_Text=value;
					OnTextChanged();
					this.OnSizeChanged();
				}
			}
		}
		
		/// <summary>
		/// Occurs after text has changed.
		/// </summary>
		protected virtual void OnTextChanged()
		{
			MarkupTextChanged();
		}

		/// <summary>
		/// Gets the parent tree control that the cell belongs to.
		/// </summary>
		[Browsable(false)]
		public TreeGX TreeControl
		{
			get
			{
				if(this.Parent!=null)
					return this.Parent.TreeControl;
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the style class assigned to the cell. Null value indicates that
		/// default style is used as specified on cell's parent.
		/// </summary>
		/// <value>
		/// Reference to the style assigned to the cell or null (VB Nothing) indicating that default
		/// style setting from tree control is applied. Default value is null.
		/// </value>
		/// <remarks>
		/// When property is set to null (VB Nothing) the style setting from parent tree
		/// controls is used. CellStyleNormal on TreeGX control is a root style for a cell.
		/// </remarks>
		/// <seealso cref="StyleDisabled">StyleDisabled Property</seealso>
		/// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
		/// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
		/// <seealso cref="StyleSelected">StyleSelected Property</seealso>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the cell.")]
		public ElementStyle StyleNormal
		{
			get {return m_StyleNormal;}
			set
			{
				m_StyleNormal=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the style name used by cell. This member is provided for internal use only. To set or get the style use StyleNormal property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleNormalName
		{
			get
			{
				if(m_StyleNormal!=null)
					return m_StyleNormal.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleNormal"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleNormal"].SetValue(this, tree.Styles[value]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the style class that is to when cell is selected. Null value indicates that
		/// default style is used as specified on cell's parent.
		/// </summary>
		/// <value>
		/// Reference to the style assigned to the cell or null value indicating that default
		/// style setting from tree control is applied. Default is null value.
		/// </value>
		/// <remarks>
		/// When property is set to null value the style setting from parent tree
		/// controls is used. CellStyleSelected on TreeGX control is a root style for a cell.
		/// </remarks>
		/// <seealso cref="StyleNormal">StyleNormal Property</seealso>
		/// <seealso cref="StyleDisabled">StyleDisabled Property</seealso>
		/// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
		/// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the cell.")]
		public ElementStyle StyleSelected
		{
			get {return m_StyleSelected;}
			set
			{
				m_StyleSelected=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the selected style name used by cell. This member is provided for internal use only. To set or get the style use StyleSelected property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleSelectedName
		{
			get
			{
				if(m_StyleSelected!=null)
					return m_StyleSelected.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, tree.Styles[value]);
				}
			}
		}


		/// <summary>
		/// Gets or sets the disabled style class assigned to the cell. Null value indicates
		/// that default style is used as specified on cell's parent.
		/// </summary>
		/// <value>
		/// Reference to the style assigned to the cell or null value indicating that default
		/// style setting from tree control is applied. Default value is null.
		/// </value>
		/// <remarks>
		/// When property is set to null value the style setting from parent tree
		/// controls is used. CellStyleDisabled on TreeGX control is a root style for a
		/// cell.
		/// </remarks>
		/// <seealso cref="StyleNormal">StyleNormal Property</seealso>
		/// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
		/// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
		/// <seealso cref="StyleSelected">StyleSelected Property</seealso>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the disabled style class assigned to the cell.")]
		public ElementStyle StyleDisabled
		{
			get {return m_StyleDisabled;}
			set
			{
				m_StyleDisabled=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the disabled style name used by cell. This member is provided for internal use only. To set or get the style use StyleDisabled property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleDisabledName
		{
			get
			{
				if(m_StyleDisabled!=null)
					return m_StyleDisabled.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleDisabled"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleDisabled"].SetValue(this, tree.Styles[value]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the style class assigned to the cell which is applied when mouse
		/// button is pressed while mouse is over the cell. Null value indicates that default
		/// style is used as specified on cell's parent.
		/// </summary>
		/// <value>
		/// Reference to the style assigned to the cell or null value indicating that default
		/// style setting from tree control is applied. Default value is null.
		/// </value>
		/// <remarks>
		/// When property is set to null value style setting from parent tree
		/// controls is used. CellStyleMouseDown on TreeGX control is a root style for a
		/// cell.
		/// </remarks>
		/// <seealso cref="StyleNormal">StyleNormal Property</seealso>
		/// <seealso cref="StyleDisabled">StyleDisabled Property</seealso>
		/// <seealso cref="StyleMouseOver">StyleMouseOver Property</seealso>
		/// <seealso cref="StyleSelected">StyleSelected Property</seealso>
		[Browsable(true),DefaultValue(null),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the cell when mouse is down.")]
		public ElementStyle StyleMouseDown
		{
			get {return m_StyleMouseDown;}
			set
			{
				m_StyleMouseDown=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the mouse down style name used by cell. This member is provided for internal use only. To set or get the style use StyleMouseDown property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleMouseDownName
		{
			get
			{
				if(m_StyleMouseDown!=null)
					return m_StyleMouseDown.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleMouseDown"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleMouseDown"].SetValue(this, tree.Styles[value]);
				}
			}
		}

		/// <summary>
		/// Gets or sets the style class assigned to the cell which is applied when mouse is
		/// over the cell. Null value indicates that default style is used as specified on cell's
		/// parent.
		/// </summary>
		/// <value>
		/// Reference to the style assigned to the cell or null value indicating that default
		/// style setting from tree control is applied. Default value is null.
		/// </value>
		/// <remarks>
		/// When property is set to null value the style setting from parent tree
		/// controls is used. CellStyleMouseOver on TreeGX control is a root style for a
		/// cell.
		/// </remarks>
		/// <seealso cref="StyleNormal">StyleNormal Property</seealso>
		/// <seealso cref="StyleDisabled">StyleDisabled Property</seealso>
		/// <seealso cref="StyleMouseDown">StyleMouseDown Property</seealso>
		/// <seealso cref="StyleSelected">StyleSelected Property</seealso>
		[Browsable(true),DefaultValue(""),Category("Style"),Editor(typeof(Design.ElementStyleTypeEditor),typeof(System.Drawing.Design.UITypeEditor)),Description("Indicates the style class assigned to the cell when mouse is over the cell.")]
		public ElementStyle StyleMouseOver
		{
			get {return m_StyleMouseOver;}
			set
			{
				m_StyleMouseOver=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the mouse over style name used by cell. This member is provided for internal use only. To set or get the style use StyleMouseOver property instead.
		/// </summary>
		[Browsable(false), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never) , DevCoSerialize()]
		public string StyleMouseOverName
		{
			get
			{
				if(m_StyleMouseOver!=null)
					return m_StyleMouseOver.Name;
				return "";
			}
			set
			{
				if(value.Length==0)
				{
					TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, null);
					return;
				}
				
				TreeGX tree=this.TreeControl;
				if(tree!=null)
				{
					TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, tree.Styles[value]);
				}
			}
		}


		/// <summary>
		/// Gets or sets whether cell is enabled or not.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Behavior"),Description("Gets or sets whether cell is enabled or not."),DevCoSerialize()]
		public bool Enabled
		{
			get {return m_Enabled;}
			set
			{
				m_Enabled=value;
			}
		}

		/// <summary>
		/// Gets the reference to images associated with this cell.
		/// </summary>
		[Browsable(true),Category("Images"),Description("Gets the reference to images associated with this cell."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public CellImages Images
		{
			get {return m_Images;}
		}
		/// <summary>
		/// Sets the Images to the new CellImages object.
		/// </summary>
		/// <param name="ci">CellImages object.</param>
		internal void SetCellImages(CellImages ci)
		{
			m_Images=ci;
			m_Images.Parent=this;
		}
		/// <summary>
		/// Returns whether Images property should be serialized. Used internally for windows forms designer support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeImages()
		{
			return m_Images.ShouldSerialize;
		}

		/// <summary>
		/// Gets or sets the image alignment in relation to the text displayed by cell.
		/// </summary>
		[Browsable(true),Category("Image Properties"),DefaultValue(eCellPartAlignment.NearCenter),Description("Gets or sets the image alignment in relation to the text displayed by cell."),DevCoSerialize()]
		public eCellPartAlignment ImageAlignment
		{
			get
			{
				return m_ImageAlignment;
			}
			set
			{
				if(m_ImageAlignment!=value)
				{
					m_ImageAlignment=value;
					this.OnAppearanceChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the checkbox alignment in relation to the text displayed by cell.
		/// </summary>
		[Browsable(true),Category("Check-box Properties"),DefaultValue(eCellPartAlignment.NearCenter),Description("Indicates checkbox alignment in relation to the text displayed by cell.")]
		public eCellPartAlignment CheckBoxAlignment
		{
			get
			{
				return m_CheckBoxAlignment;
			}
			set
			{
				if(m_CheckBoxAlignment!=value)
				{
					m_CheckBoxAlignment=value;
					this.OnAppearanceChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets whether check box is visible inside the cell.
		/// </summary>
		[Browsable(true),Category("Check-box Properties"),DefaultValue(false),Description("Indicates whether check box is visible inside the cell."),DevCoSerialize()]
		public bool CheckBoxVisible
		{
			get
			{
				return m_CheckBoxVisible;
			}
			set
			{
				if(m_CheckBoxVisible!=value)
				{
					m_CheckBoxVisible=value;
					this.OnAppearanceChanged();
				}
			}
		}

		/// <summary>
		/// Gets or set a value indicating whether the check box is in the checked state.
		/// </summary>
		[Browsable(true),Category("Check-box Properties"),DefaultValue(false),Description("Indicates whether the check box is in the checked state."),DevCoSerialize()]
		public bool Checked
		{
			get {return m_Checked;}
			set
			{
				if(m_Checked!=value)
				{
					m_Checked=value;
					this.OnAppearanceChanged();
					InvokeAfterCheck();
				}
			}
		}

		/// <summary>
		/// Gets whether mouse is over the cell.
		/// </summary>
		[Browsable(false)]
		public bool IsMouseOver
		{
			get{return m_MouseOver;}
		}
		/// <summary>
		/// Sets the mouse over flag.
		/// </summary>
		/// <param name="over">true if mouse is over the cell otherwise false.</param>
		internal void SetMouseOver(bool over)
		{
			m_MouseOver=over;
		}

		/// <summary>
		/// Gets whether left mouse button is pressed while over the cell.
		/// </summary>
		[Browsable(false)]
		public bool IsMouseDown
		{
			get{return m_MouseDown;}
		}
		/// <summary>
		/// Sets the mouse down flag.
		/// </summary>
		/// <param name="over">true if left mouse button is pressed while over the cell otherwise false.</param>
		internal void SetMouseDown(bool over)
		{
			m_MouseDown=over;
		}

		/// <summary>
		/// Gets or sets the layout of the cell parts like check box, image and text. Layout can be horizontal (default)
		/// where parts of the cell are positioned next to each other horizontally, or vertical where
		/// parts of the cell are positioned on top of each other vertically.
		/// Alignment of the each part is controlled by alignment properties.
		/// </summary>
		/// <seealso cref="ImageAlignment">ImageAlignment Property</seealso>
		/// <seealso cref="CheckBoxAlignment">CheckBoxAlignment Property</seealso>
		[Browsable(true),DefaultValue(eCellPartLayout.Default),Category("Cells"),Description("Indicates the layout of the cell parts like check box, image and text."), DevCoSerialize()]
		public eCellPartLayout Layout
		{
			get {return m_Layout;}
			set
			{
				if(m_Layout!=value)
				{
					m_Layout=value;
					this.OnAppearanceChanged();
				}
			}
		}

		/// <summary>
		/// Specifes the mouse cursor displayed when mouse is over the cell.
		/// </summary>
		[Browsable(true),DefaultValue(null),Category("Appearance"),Description("Specifies the mouse cursor displayed when mouse is over the cell.")]
		public System.Windows.Forms.Cursor Cursor
		{
			get
			{
				return m_Cursor;
			}
			set
			{
				if(m_Cursor!=value)
				{
					m_Cursor=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether cell wrapped the text during the layout.
		/// </summary>
		internal bool WordWrap
		{
			get {return m_WordWrap;}
			set {m_WordWrap=value;}
		}
		#endregion

		#region Methods
		/// <summary>Makes a copy of a Cell.</summary>
		public virtual Cell Copy()
		{
			// TODO: Make sure that new properties are copied
			Cell c=new Cell();
			c.CheckBoxAlignment=this.CheckBoxAlignment;
			c.CheckBoxVisible=this.CheckBoxVisible;
			c.Checked=this.Checked;
			c.Cursor=this.Cursor;
			c.Enabled=this.Enabled;
			c.ImageAlignment=this.ImageAlignment;
			c.SetCellImages(this.Images.Copy());
			c.Layout=this.Layout;
			c.StyleDisabled=this.StyleDisabled;
			c.StyleMouseDown=this.StyleMouseDown;
			c.StyleMouseOver=this.StyleMouseOver;
			c.StyleNormal=this.StyleNormal;
			c.StyleSelected=this.StyleSelected;
			c.Tag=this.Tag;
			c.Text=this.Text;
			return c;
		}

		private void OnAppearanceChanged()
		{
			if(this.Parent!=null)
			{
				this.Parent.SizeChanged=true;
				this.Parent.OnDisplayChanged();
			}
		}

		private void OnSizeChanged()
		{
			if(this.Parent!=null)
			{
				this.Parent.SizeChanged=true;
				this.Parent.OnDisplayChanged();
			}
		}

		/// <summary>
		/// Occurs when any image property for the cell has changed.
		/// </summary>
		internal void OnImageChanged()
		{
			if(m_Parent!=null)
				m_Parent.OnImageChanged();
		}

		/// <summary>
		/// Invokes <see cref="TreeGX.AfterCheck">AfterCheck</see> event on TreeGX
		/// control.
		/// </summary>
		protected virtual void InvokeAfterCheck()
		{
			TreeGX tree=this.TreeControl;
			if(tree!=null)
			{
				tree.InvokeAfterCheck(new TreeGXCellEventArgs(m_ActionSource,this));
			}
			m_ActionSource=eTreeAction.Code;
		}

		/// <summary>
		/// Called just before cell layout is to be performed.
		/// </summary>
		internal void OnLayoutCell()
		{
			if(m_HostedControl!=null && m_HostedControl.Parent==null)
			{
				AddHostedControlToTree();	
			}
		}
		
		private void AddHostedControlToTree()
		{
			TreeGX tree = this.TreeControl;
			if(tree!=null)
			{
				tree.Controls.Add(m_HostedControl);
				tree.HostedControlCells.Add(this);
			}
		}
		
		private void RemoveHostedControlFromTree()
		{
			TreeGX tree = m_HostedControl.Parent as TreeGX;
			if(tree!=null)
			{
				tree.Controls.Remove(m_HostedControl);
				tree.HostedControlCells.Remove(this);
			}
		}

		private void SetHostedControl(Control value)
		{
			// Note that hosted control when set is guided by the size of the text. So the TextContentBounds are actually bounds of the control.
			if(value==this.TreeControl)
				return;
			
			if(m_HostedControl!=null)
			{
				m_HostedControl.SizeChanged-=new EventHandler(this.HostedControlSizedChanged);
				RemoveHostedControlFromTree();
			}
			
			m_HostedControl=value;
			if(m_HostedControl!=null)
			{
				m_HostedControl.SizeChanged+=new EventHandler(this.HostedControlSizedChanged);
				TypeDescriptor.GetProperties(m_HostedControl)["Dock"].SetValue(m_HostedControl,DockStyle.None);
				if(m_HostedControl.Parent!=null)
					m_HostedControl.Parent.Controls.Remove(m_HostedControl);
				
				if(!this.DesignMode)
					m_HostedControl.Visible = false;
				if(this.DesignMode || this.Parent!=null && this.Parent.Site!=null && this.Parent.Site.DesignMode)
				{
					bool visible=GetVisible();
					TypeDescriptor.GetProperties(m_HostedControl)["Visible"].SetValue(m_HostedControl,visible);
				}
				
				AddHostedControlToTree();
			}

			OnSizeChanged();
		}
		private void HostedControlSizedChanged(object sender, EventArgs e)
		{
			if(m_IgnoreHostedControlSizeChange)
				return;
			if(!m_HostedControlSize.IsEmpty && m_HostedControl!=null)
				m_HostedControlSize = m_HostedControl.Size;
			OnSizeChanged();
		}

		internal void OnVisibleChanged()
		{
			if(m_HostedControl!=null)
			{
				TypeDescriptor.GetProperties(m_HostedControl)["Visible"].SetValue(m_HostedControl,GetVisible());
			}
		}
		
		private bool GetVisible()
		{
			if(m_Parent!=null && !NodeOperations.GetIsNodeVisible(m_Parent))
				return false;
			return m_Visible;
		}
		
		internal void OnParentExpandedChanged(bool expanded)
		{
			if(m_HostedControl!=null && m_HostedControl.Visible != expanded)
			{
				bool visible=GetVisible();
				m_HostedControl.Visible = visible;
				if(this.DesignMode || this.Parent!=null && this.Parent.Site!=null && this.Parent.Site.DesignMode)
					TypeDescriptor.GetProperties(m_HostedControl)["Visible"].SetValue(m_HostedControl,visible);
			}
		}
		#endregion
		
		#region Markup Implementation
		private TextMarkup.BodyElement m_TextMarkup = null;

		private void MarkupTextChanged()
		{
			if (!IsMarkupSupported)
				return;

			if(m_TextMarkup!=null)
				m_TextMarkup.HyperLinkClick -= new EventHandler(this.TextMarkupLinkClick);
			
			m_TextMarkup = null;

			if (!TextMarkup.MarkupParser.IsMarkup(ref m_Text))
				return;

			m_TextMarkup = TextMarkup.MarkupParser.Parse(m_Text);

			if(m_TextMarkup!=null)
				m_TextMarkup.HyperLinkClick += new EventHandler(TextMarkupLinkClick);
		}

		/// <summary>
		/// Occurs when text markup link is clicked.
		/// </summary>
		protected virtual void TextMarkupLinkClick(object sender, EventArgs e)
		{
			TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
			
			if(link!=null && this.Parent!=null)
				this.Parent.InvokeMarkupLinkClick(this, new MarkupLinkClickEventArgs(link.Name, link.HRef));
		}

		/// <summary>
		/// Gets reference to parsed markup body element if text was markup otherwise returns null.
		/// </summary>
		internal TextMarkup.BodyElement TextMarkupBody
		{
			get { return m_TextMarkup; }
		}

		/// <summary>
		/// Gets whether item supports text markup. Default is false.
		/// </summary>
		protected virtual bool IsMarkupSupported
		{
			get { return true; }
		}

		protected internal virtual void InvokeNodeMouseMove(object sender, MouseEventArgs e)
		{
			if(m_TextMarkup!=null)
				m_TextMarkup.MouseMove(sender as Control, e);
		}
		
		protected internal virtual void InvokeNodeMouseDown(object sender, MouseEventArgs e)
		{
			if(m_TextMarkup!=null)
				m_TextMarkup.MouseDown(sender as Control, e);
		}
		
		protected internal virtual void InvokeNodeMouseUp(object sender, MouseEventArgs e)
		{
			if(m_TextMarkup!=null)
				m_TextMarkup.MouseUp(sender as Control, e);
		}
		
		protected internal virtual void InvokeNodeMouseLeave(object sender, EventArgs e)
		{
			if(m_TextMarkup!=null)
				m_TextMarkup.MouseLeave(sender as Control);
		}
		
		protected internal virtual void InvokeNodeClick(object sender, EventArgs e)
		{
			if(m_TextMarkup!=null)
				m_TextMarkup.Click(sender as Control);
		}
		#endregion
	}
}
