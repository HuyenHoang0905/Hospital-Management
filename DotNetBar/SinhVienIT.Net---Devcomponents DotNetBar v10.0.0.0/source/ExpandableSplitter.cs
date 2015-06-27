using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents multi-functional splitter control.
	/// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.ExpandableSplitterDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false), DefaultEvent("ExpandedChanged")]
	public class ExpandableSplitter:Splitter, IMessageHandlerClient
	{
		#region Private Variables
		private int m_AnimationTime=100;
		private Control m_ExpandableControl=null;
		private bool m_Expanded=true;
		private eSplitterStyle m_Style=eSplitterStyle.Office2003;
		private bool m_Expandable=true;
		private eShortcut m_Shortcut=eShortcut.None;
		private bool m_FilterInstalled=false;
		private bool m_MouseOver=false;
		private bool m_MouseDown=false;
		private bool m_Resized=false;
		private Point m_MouseDownPoint=Point.Empty;

		private ColorScheme m_ColorScheme=null;
		private eColorSchemePart m_BackColorSchemePart=eColorSchemePart.None;
		private Color m_BackColor2=Color.Empty;
		private eColorSchemePart m_BackColor2SchemePart=eColorSchemePart.None;
		private int m_BackColorGradientAngle=0;
		private Color m_ExpandFillColor=Color.Empty;
		private eColorSchemePart m_ExpandFillColorSchemePart=eColorSchemePart.None;
		private Color m_ExpandLineColor=Color.Empty;
		private eColorSchemePart m_ExpandLineColorSchemePart=eColorSchemePart.None;
		private Color m_GripDarkColor=Color.Empty;
		private eColorSchemePart m_GripDarkColorSchemePart=eColorSchemePart.None;
		private Color m_GripLightColor=Color.Empty;
		private eColorSchemePart m_GripLightColorSchemePart=eColorSchemePart.None;
		// Hot colors
		private Color m_HotBackColor=Color.Empty;
		private eColorSchemePart m_HotBackColorSchemePart=eColorSchemePart.None;
		private Color m_HotBackColor2=Color.Empty;
		private eColorSchemePart m_HotBackColor2SchemePart=eColorSchemePart.None;
		private int m_HotBackColorGradientAngle=0;
		private Color m_HotExpandFillColor=Color.Empty;
		private eColorSchemePart m_HotExpandFillColorSchemePart=eColorSchemePart.None;
		private Color m_HotExpandLineColor=Color.Empty;
		private eColorSchemePart m_HotExpandLineColorSchemePart=eColorSchemePart.None;
		private Color m_HotGripDarkColor=Color.Empty;
		private eColorSchemePart m_HotGripDarkColorSchemePart=eColorSchemePart.None;
		private Color m_HotGripLightColor=Color.Empty;
		private eColorSchemePart m_HotGripLightColorSchemePart=eColorSchemePart.None;

		private SplitterPainter m_Painter=null;
		private SplitterPaintInfo m_SplitterPaintInfo=new SplitterPaintInfo();
		private bool m_ExpandActionClick=true;
		private bool m_ExpandActionDoubleClick=false;
		#endregion

		#region Events
		public event ExpandChangeEventHandler ExpandedChanging;
		public event ExpandChangeEventHandler ExpandedChanged;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public ExpandableSplitter():base()
		{
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);

			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}
			m_ColorScheme=new ColorScheme(eDotNetBarStyle.Office2003);
			this.ApplyStyle(eSplitterStyle.Office2003);
            StyleManager.Register(this);
		}
        protected override void Dispose(bool disposing)
        {
            if (disposing) StyleManager.Unregister(this);
            base.Dispose(disposing);
        }
		#endregion

		#region Display Support
		private SplitterPainter GetPainter()
		{
			if(m_Painter==null)
			{
				switch(m_Style)
				{
					case eSplitterStyle.Office2003:
                    case eSplitterStyle.Office2007:
					{
						m_Painter=new SplitterOffice2003Painter();
						break;
					}
					case eSplitterStyle.Mozilla:
					{
						m_Painter=new SplitterMozillaPainter();
						break;
					}
				}
			}
			return m_Painter;
		}

		private SplitterPaintInfo GetSplitterPaintInfo(Graphics g)
		{
			m_SplitterPaintInfo.Graphics=g;
			m_SplitterPaintInfo.Colors=this.GetColors();
			m_SplitterPaintInfo.Expandable=m_Expandable;
			m_SplitterPaintInfo.Expanded=m_Expanded;
			m_SplitterPaintInfo.Dock=this.Dock;
			m_SplitterPaintInfo.DisplayRectangle=this.DisplayRectangle;
			return m_SplitterPaintInfo;
		}

		/// <summary>
		/// This member overrides Control.OnPaint.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
//			if(m_AntiAlias)
//			{
//				e.Graphics.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
//				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
//			}

			this.GetPainter().Paint(this.GetSplitterPaintInfo(e.Graphics));
		}
		
		#endregion

		#region Properties
//		/// <summary>
//		/// Gets or sets whether anti-alias smoothing is used while painting.
//		/// </summary>
//		[DefaultValue(false),Browsable(true),Category("Appearance"),Description("Gets or sets whether anti-aliasing is used while painting.")]
//		public bool AntiAlias
//		{
//			get {return m_AntiAlias;}
//			set
//			{
//				if(m_AntiAlias!=value)
//				{
//					m_AntiAlias=value;
//					this.Refresh();
//				}
//			}
//		}

		/// <summary>
		/// Gets or sets whether expandable control ExpandableControl assigned to this splitter is expaned or not. Default value is true.
		/// </summary>
		[Browsable(true),Category("Expand"),DefaultValue(true),Description("Indicates whether expandable control ExpandableControl assigned to this splitter is expaned or not. Default value is true.")]
		public bool Expanded
		{
			get {return m_Expanded;}
			set
			{
				if(m_Expanded!=value)
					SetExpanded(value,eEventSource.Code);
			}
		}

		/// <summary>
		/// Gets or sets whether Click event is triggering expand/collapse of the splitter. Default value is true.
		/// </summary>
		[Browsable(true),Category("Expand"),DefaultValue(true),Description("Indicates whether Click event is triggering expand/collapse of the splitter. Default value is true.")]
		public bool ExpandActionClick
		{
			get {return m_ExpandActionClick;}
			set {m_ExpandActionClick=value;}
		}

		/// <summary>
		/// Gets or sets whether DoubleClick event is triggering expand/collapse of the splitter. Default value is false.
		/// </summary>
		[Browsable(true),Category("Expand"),DefaultValue(false),Description("Indicates whether DoubleClick event is triggering expand/collapse of the splitter. Default value is false.")]
		public bool ExpandActionDoubleClick
		{
			get {return m_ExpandActionDoubleClick;}
			set {m_ExpandActionDoubleClick=value;}
		}

		/// <summary>
		/// Gets or sets whether splitter will act as expandable splitter. Default value is true. When set to true ExpandableControl property should be set to the control that should be expanded/collapsed.
		/// </summary>
		[Browsable(true),Category("Expand"),DefaultValue(true),Description("Indicates whether splitter will act as expandable splitter. Default value is true.")]
		public bool Expandable
		{
			get {return m_Expandable;}
			set
			{
				m_Expandable=value;
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the control that will be expanded/collapsed by the splitter. Default value is null. Expandable property should be also set to true (default) to enable expand/collapse functionality.
		/// </summary>
		[Browsable(true),Category("Expand"),DefaultValue(null),Description("Indicates control that will be expanded/collapsed by the splitter.")]
		public Control ExpandableControl
		{
			get {return m_ExpandableControl;}
			set
			{
				if(value==this)
					return;
				m_ExpandableControl=value;
				this.OnExpandableControlChanged();
			}
		}

		/// <summary>
		/// Gets or sets visual style of the control. Default value is eSplitterStyle.Office2003.
		/// </summary>
		[Browsable(true),Category("Appearance"),DefaultValue(eSplitterStyle.Office2003),Description("Indicates visual style of the control.")]
		public eSplitterStyle Style
		{
			get {return m_Style;}
			set
			{
				m_Style=value;
				OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets animation time in milliseconds. Default value is 100 miliseconds. You can set this to 0 (zero) to disable animation.
		/// </summary>
		[Browsable(true),DefaultValue(100),Category("Expand"),Description("Indicates animation time in milliseconds, default value is 100.")]
		public int AnimationTime
		{
			get {return m_AnimationTime;}
			set
			{
				if(m_AnimationTime>=0)
					m_AnimationTime=value;
			}
		}

		/// <summary>
		/// Gets or sets the shortcut key to expand/collapse splitter.
		/// </summary>
		[Browsable(true),Category("Expand"),DefaultValue(eShortcut.None),Description("Indicates shortcut key to expand/collapse splitter."),]
		public eShortcut Shortcut
		{
			get {return m_Shortcut;}
			set
			{
				m_Shortcut=value;
				if(m_Shortcut!=eShortcut.None && this.IsHandleCreated)
					InstallIMessageHandlerClient();
			}
		}
		#endregion

		#region Private Implementation
		private void OnExpandedChanged()
		{
			if(this.ExpandableControl==null)
				return;
			
			if(this.Expanded)
			{
				if(!this.ExpandableControl.Visible)
				{
					if(this.AnimationTime==0 || this.DesignMode)
					{
						if(this.DesignMode)
							TypeDescriptor.GetProperties(this.ExpandableControl)["Visible"].SetValue(this.ExpandableControl,true);
						this.ExpandableControl.Visible=true;
					}
					else
					{
						Rectangle controlRect=GetAnimationTarget(this.ExpandableControl,false);
						Rectangle targetRect=this.ExpandableControl.Bounds;
						BarFunctions.AnimateControl(this.ExpandableControl,true,m_AnimationTime,controlRect,targetRect);
					}
				}
			}
			else
			{
				if(this.ExpandableControl.Visible || !this.IsHandleCreated)
				{
					if(this.AnimationTime==0 || this.DesignMode || !this.IsHandleCreated)
					{
						if(this.DesignMode)
							TypeDescriptor.GetProperties(this.ExpandableControl)["Visible"].SetValue(this.ExpandableControl,false);
						this.ExpandableControl.Visible=false;
					}
					else
					{
						Rectangle controlRect=this.ExpandableControl.Bounds;
						Rectangle targetRect=GetAnimationTarget(this.ExpandableControl,false);
						BarFunctions.AnimateControl(this.ExpandableControl,false,m_AnimationTime,controlRect,targetRect);
						this.ExpandableControl.Visible=false;
						this.ExpandableControl.Bounds=controlRect;
					}
				}
			}
			this.Refresh();
		}

		private Rectangle GetAnimationTarget(Control c, bool showControl)
		{
			Rectangle r=Rectangle.Empty;
			DockStyle dock=GetControlDock(c);
			if(!showControl)
			{
				if(dock==DockStyle.Left)
				{
					r=new Rectangle(c.Left,c.Top,0,c.Height);
				}
				else if(dock==DockStyle.Right)
				{
					r=new Rectangle(c.Right,c.Top,0,c.Height);
				}
				else if(dock==DockStyle.Top)
				{
					r=new Rectangle(c.Left,c.Top,c.Width,0);
				}
				else if(dock==DockStyle.Bottom)
				{
					r=new Rectangle(c.Left,c.Bottom,c.Width,0);
				}
			}
			else
			{
				if(dock==DockStyle.Left)
				{
					r=new Rectangle(c.Left,c.Top,c.Width,c.Height);
				}
				else if(dock==DockStyle.Right)
				{
					r=new Rectangle(c.Left-c.Width,c.Top,c.Width,c.Height);
				}
				else if(dock==DockStyle.Top)
				{
					r=new Rectangle(c.Left,c.Top,c.Width,c.Height);
				}
				else if(dock==DockStyle.Bottom)
				{
					r=new Rectangle(c.Left,c.Top-c.Height,c.Width,c.Height);
				}
			}

			return r;
		}

		private DockStyle GetControlDock(Control c)
		{
			if(c.Dock==DockStyle.None)
			{
				if(c.Right<this.Left)
					return DockStyle.Left;
				else if(c.Left>this.Right)
					return DockStyle.Right;
				else if(c.Bottom<this.Top)
					return DockStyle.Top;
				else if(c.Top>this.Bottom)
					return DockStyle.Bottom;
				return DockStyle.Left;
			}
			else if(c.Dock==DockStyle.Fill)
				return DockStyle.Left;
			return c.Dock;
		}

		private void OnExpandableControlChanged()
		{
			
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			InstallIMessageHandlerClient();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			UninstallIMessageHandlerClient();			
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if(this.Expandable && m_MouseOver && !m_Resized && m_ExpandActionClick)
			{
				SetExpanded(!this.Expanded,eEventSource.Mouse);
			}
			m_Resized=false;
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);
			if(this.Expandable && m_MouseOver && m_ExpandActionDoubleClick)
			{
				SetExpanded(!this.Expanded,eEventSource.Mouse);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(m_MouseDown)
			{
				base.OnMouseDown(e);
			}
			else
			{
				m_Resized=false;
				m_MouseDownPoint=new Point(e.X,e.Y);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			m_MouseDown=false;
			base.OnMouseUp(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			m_MouseOver=true;
			this.Refresh();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			m_MouseOver=false;
			this.Refresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(!this.DisplayRectangle.Contains(e.X,e.Y) && !m_MouseDown && e.Button==MouseButtons.Left)
			{
				m_MouseDown=true;
				m_Resized=true;
				this.OnMouseDown(new MouseEventArgs(e.Button,1,m_MouseDownPoint.X,m_MouseDownPoint.Y,e.Delta));
			}
		}

		private void InvokeExpandedChanging(ExpandedChangeEventArgs e)
		{
			if(ExpandedChanging!=null)
				ExpandedChanging(this,e);
		}
		private void InvokeExpandedChanged(ExpandedChangeEventArgs e)
		{
			if(ExpandedChanged!=null)
				ExpandedChanged(this,e);
		}

		private void SetExpanded(bool expanded, eEventSource action)
		{
			ExpandedChangeEventArgs e=new ExpandedChangeEventArgs(action,expanded);
			InvokeExpandedChanging(e);
			if(e.Cancel)
				return;
			m_Expanded=expanded;
			OnExpandedChanged();
			InvokeExpandedChanged(e);
		}

		private void OnStyleChanged()
		{
			m_Painter=null;
			this.Invalidate();
		}
		#endregion

		#region Color Support
		/// <summary>
		/// Gets or sets the background color for UI element. If used in combination with
		/// BackgroundColor2 is specifies starting gradient color.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Gets or sets the background color for UI element."),Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public new Color BackColor
		{
			get
			{
				return GetColor(base.BackColor,m_BackColorSchemePart);
			}
			set
			{
				if(m_BackColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.BackColorSchemePart=eColorSchemePart.None;
				base.BackColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void ResetBackColor()
		{
			base.BackColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as background color. Setting
		/// this property overrides the setting of the corresponding BackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through BackColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart BackColorSchemePart
		{
			get {return m_BackColorSchemePart;}
			set
			{
				m_BackColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the target gradient background color for UI element.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Gets or sets the target gradient background color for UI element.")]
		public Color BackColor2
		{
			get {return GetColor(m_BackColor2,m_BackColor2SchemePart);}
			set
			{
				if(m_BackColor2SchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.BackColor2SchemePart=eColorSchemePart.None;
				m_BackColor2=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor2()
		{
			m_BackColor2=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as target gradient background color. Setting
		/// this property overrides the setting of the corresponding BackColor2 property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through BackColor2 property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart BackColor2SchemePart
		{
			get {return m_BackColor2SchemePart;}
			set
			{
				m_BackColor2SchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the background gradient angle.
		/// </summary>
		[Browsable(true),Category("Background"),DefaultValue(0),Description("Gets or sets the background gradient angle.")]
		public int BackColorGradientAngle
		{
			get
			{
				return m_BackColorGradientAngle;
			}
			set
			{
				if(m_BackColorGradientAngle!=value)
				{
					m_BackColorGradientAngle=value;
					this.OnStyleChanged();

				}
			}
		}

		/// <summary>
		/// Gets or sets the expand part fill color.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates expand part fill color.")]
		public Color ExpandFillColor
		{
			get {return GetColor(m_ExpandFillColor,m_ExpandFillColorSchemePart);}
			set
			{
				if(m_ExpandFillColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.ExpandFillColorSchemePart=eColorSchemePart.None;
				m_ExpandFillColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetExpandFillColor()
		{
			m_ExpandFillColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used expand part fill color. Setting
		/// this property overrides the setting of the corresponding ExpandFillColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through ExpandFillColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart ExpandFillColorSchemePart
		{
			get {return m_ExpandFillColorSchemePart;}
			set
			{
				m_ExpandFillColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the expand part line color.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates expand part line color.")]
		public Color ExpandLineColor
		{
			get {return GetColor(m_ExpandLineColor,m_ExpandLineColorSchemePart);}
			set
			{
				if(m_ExpandLineColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.ExpandLineColorSchemePart=eColorSchemePart.None;
				m_ExpandLineColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetExpandLineColor()
		{
			m_ExpandLineColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used expand part line color. Setting
		/// this property overrides the setting of the corresponding ExpandLineColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through ExpandLineColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart ExpandLineColorSchemePart
		{
			get {return m_ExpandLineColorSchemePart;}
			set
			{
				m_ExpandLineColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the grip part dark color.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates expand part line color.")]
		public Color GripDarkColor
		{
			get {return GetColor(m_GripDarkColor,m_GripDarkColorSchemePart);}
			set
			{
				if(m_GripDarkColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.GripDarkColorSchemePart=eColorSchemePart.None;
				m_GripDarkColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetGripDarkColor()
		{
			m_GripDarkColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used grip part dark color. Setting
		/// this property overrides the setting of the corresponding GripDarkColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through GripDarkColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart GripDarkColorSchemePart
		{
			get {return m_GripDarkColorSchemePart;}
			set
			{
				m_GripDarkColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the expand part line color.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates expand part line color.")]
		public Color GripLightColor
		{
			get {return GetColor(m_GripLightColor,m_GripLightColorSchemePart);}
			set
			{
				if(m_GripLightColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.GripLightColorSchemePart=eColorSchemePart.None;
				m_GripLightColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetGripLightColor()
		{
			m_GripLightColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used expand part line color. Setting
		/// this property overrides the setting of the corresponding GripLightColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through GripLightColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart GripLightColorSchemePart
		{
			get {return m_GripLightColorSchemePart;}
			set
			{
				m_GripLightColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the background color for UI element when mouse is over the element. If used in combination with
		/// BackgroundColor2 is specifies starting gradient color.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Gets or sets the background color for UI element when mouse is over the element."),Editor(typeof(ColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public Color HotBackColor
		{
			get
			{
				return GetColor(m_HotBackColor,m_HotBackColorSchemePart);
			}
			set
			{
				if(m_HotBackColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.HotBackColorSchemePart=eColorSchemePart.None;
				m_HotBackColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetHotBackColor()
		{
			m_HotBackColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as background color when mouse is over the element. Setting
		/// this property overrides the setting of the corresponding HotBackColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through HotBackColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart HotBackColorSchemePart
		{
			get {return m_HotBackColorSchemePart;}
			set
			{
				m_HotBackColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the target gradient background color for UI element when mouse is over the element.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Gets or sets the target gradient background color for UI element when mouse is over the element.")]
		public Color HotBackColor2
		{
			get {return GetColor(m_HotBackColor2,m_HotBackColor2SchemePart);}
			set
			{
				if(m_HotBackColor2SchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.HotBackColor2SchemePart=eColorSchemePart.None;
				m_HotBackColor2=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetHotBackColor2()
		{
			m_HotBackColor2=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the color scheme color that is used as target gradient background color when mouse is over the element. Setting
		/// this property overrides the setting of the corresponding HotBackColor2 property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through HotBackColor2 property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart HotBackColor2SchemePart
		{
			get {return m_HotBackColor2SchemePart;}
			set
			{
				m_HotBackColor2SchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the background gradient angle when mouse is over the element.
		/// </summary>
		[Browsable(true),Category("Background"),DefaultValue(0),Description("Gets or sets the background gradient angle when mouse is over the element.")]
		public int HotBackColorGradientAngle
		{
			get
			{
				return m_HotBackColorGradientAngle;
			}
			set
			{
				if(m_HotBackColorGradientAngle!=value)
				{
					m_HotBackColorGradientAngle=value;
					this.OnStyleChanged();

				}
			}
		}

		/// <summary>
		/// Gets or sets the expand part fill color when mouse is over the element.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates expand part fill color when mouse is over the element.")]
		public Color HotExpandFillColor
		{
			get {return GetColor(m_HotExpandFillColor,m_HotExpandFillColorSchemePart);}
			set
			{
				if(m_HotExpandFillColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.HotExpandFillColorSchemePart=eColorSchemePart.None;
				m_HotExpandFillColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetHotExpandFillColor()
		{
			m_HotExpandFillColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used expand part fill color when mouse is over the element. Setting
		/// this property overrides the setting of the corresponding HotExpandFillColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through HotExpandFillColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart HotExpandFillColorSchemePart
		{
			get {return m_HotExpandFillColorSchemePart;}
			set
			{
				m_HotExpandFillColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the expand part line color when mouse is over the element.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates expand part line color when mouse is over the element.")]
		public Color HotExpandLineColor
		{
			get {return GetColor(m_HotExpandLineColor,m_HotExpandLineColorSchemePart);}
			set
			{
				if(m_HotExpandLineColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.HotExpandLineColorSchemePart=eColorSchemePart.None;
				m_HotExpandLineColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetHotExpandLineColor()
		{
			m_HotExpandLineColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used expand part line color when mouse is over the element. Setting
		/// this property overrides the setting of the corresponding HotExpandLineColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through HotExpandLineColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart HotExpandLineColorSchemePart
		{
			get {return m_HotExpandLineColorSchemePart;}
			set
			{
				m_HotExpandLineColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the grip part dark color when mouse is over the element.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates expand part line color when mouse is over the element.")]
		public Color HotGripDarkColor
		{
			get {return GetColor(m_HotGripDarkColor,m_HotGripDarkColorSchemePart);}
			set
			{
				if(m_HotGripDarkColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.HotGripDarkColorSchemePart=eColorSchemePart.None;
				m_HotGripDarkColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetHotGripDarkColor()
		{
			m_HotGripDarkColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used grip part dark color when mouse is over the element. Setting
		/// this property overrides the setting of the corresponding HotGripDarkColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through HotGripDarkColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart HotGripDarkColorSchemePart
		{
			get {return m_HotGripDarkColorSchemePart;}
			set
			{
				m_HotGripDarkColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the grip part light color when mouse is over the element.
		/// </summary>
		[Browsable(true),Category("Colors"),Description("Indicates grip part light color when mouse is over the element.")]
		public Color HotGripLightColor
		{
			get {return GetColor(m_HotGripLightColor,m_HotGripLightColorSchemePart);}
			set
			{
				if(m_HotGripLightColorSchemePart!=eColorSchemePart.None && !value.IsEmpty)
					this.HotGripLightColorSchemePart=eColorSchemePart.None;
				m_HotGripLightColor=value;
				this.OnStyleChanged();
			}
		}
		/// <summary>
		/// Resets BackgroundColor2 to it's default value. Used by windows forms designer design-time support.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetHotGripLightColor()
		{
			m_HotGripLightColor=Color.Empty;
		}
		/// <summary>
		/// Gets or sets the color scheme color that is used grip part light color when mouse is over the element. Setting
		/// this property overrides the setting of the corresponding HotGripLightColor property.
		/// Color scheme colors are automatically managed and are based on current system colors.
		/// That means if colors on the system change the color scheme will ensure that it's colors
		/// are changed as well to fit in the color scheme of target system. Set this property to
		/// eColorSchemePart.None to
		/// specify explicit color to use through HotGripLightColor property.
		/// </summary>
		[Browsable(false)]
		public eColorSchemePart HotGripLightColorSchemePart
		{
			get {return m_HotGripLightColorSchemePart;}
			set
			{
				m_HotGripLightColorSchemePart=value;
				this.OnStyleChanged();
			}
		}

		private Color GetColor(Color color, eColorSchemePart p)
		{
			if(p==eColorSchemePart.None || p==eColorSchemePart.Custom)
				return color;
			ColorScheme cs=this.GetColorScheme();
			if(cs==null)
				return color;
			return (Color)cs.GetType().GetProperty(p.ToString()).GetValue(cs,null);
		}

		private ColorScheme GetColorScheme()
		{
            if (this.Style == eSplitterStyle.Office2007 && GlobalManager.Renderer is Office2007Renderer)
            {
                return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
            }

			return m_ColorScheme;
		}

		private SplitterColors GetColors()
		{
			SplitterColors colors=new SplitterColors();
			if(m_MouseOver && !this.HotBackColor.IsEmpty)
			{
				colors.BackColor=this.HotBackColor;
				colors.BackColor2=this.HotBackColor2;
				colors.BackColorGradientAngle=this.HotBackColorGradientAngle;
				colors.ExpandFillColor=this.HotExpandFillColor;
				colors.ExpandLineColor=this.HotExpandLineColor;
				colors.GripDarkColor=this.HotGripDarkColor;
				colors.GripLightColor=this.HotGripLightColor;
			}
			else
			{
				colors.BackColor=this.BackColor;
				colors.BackColor2=this.BackColor2;
				colors.BackColorGradientAngle=this.BackColorGradientAngle;
				colors.ExpandFillColor=this.ExpandFillColor;
				colors.ExpandLineColor=this.ExpandLineColor;
				colors.GripDarkColor=this.GripDarkColor;
				colors.GripLightColor=this.GripLightColor;
			}

			return colors;
		}

		/// <summary>
		/// Apply default splitter style colors.
		/// </summary>
		/// <param name="style">Style colors to apply.</param>
		public void ApplyStyle(eSplitterStyle style)
		{
			if(style==eSplitterStyle.Office2003)
			{
				m_ColorScheme=new ColorScheme(eDotNetBarStyle.Office2003);
				this.BackColorSchemePart=eColorSchemePart.PanelBackground;
				this.BackColor2SchemePart=eColorSchemePart.PanelBorder;
				this.GripLightColorSchemePart=eColorSchemePart.BarBackground;
				this.GripDarkColorSchemePart=eColorSchemePart.ItemText;
				this.ExpandFillColorSchemePart=eColorSchemePart.PanelBorder;
				this.ExpandLineColorSchemePart=eColorSchemePart.ItemText;
				
				this.HotBackColorSchemePart=eColorSchemePart.ItemPressedBackground;
				this.HotBackColor2SchemePart=eColorSchemePart.ItemPressedBackground2;
				this.HotGripLightColorSchemePart=eColorSchemePart.BarBackground;
				this.HotGripDarkColorSchemePart=eColorSchemePart.PanelBorder;
				this.HotExpandFillColorSchemePart=eColorSchemePart.PanelBorder;
				this.HotExpandLineColorSchemePart=eColorSchemePart.ItemText;
			}
            else if (style == eSplitterStyle.Office2007)
            {
                m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
                this.BackColorSchemePart = eColorSchemePart.PanelBackground;
                this.BackColor2SchemePart = eColorSchemePart.PanelBorder;
                this.GripLightColorSchemePart = eColorSchemePart.BarBackground;
                this.GripDarkColorSchemePart = eColorSchemePart.ItemText;
                this.ExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
                this.ExpandLineColorSchemePart = eColorSchemePart.ItemText;

                this.HotBackColorSchemePart = eColorSchemePart.ItemPressedBackground;
                this.HotBackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
                this.HotGripLightColorSchemePart = eColorSchemePart.BarBackground;
                this.HotGripDarkColorSchemePart = eColorSchemePart.PanelBorder;
                this.HotExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
                this.HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
            }
			else if(style==eSplitterStyle.Mozilla)
			{
				m_ColorScheme=new ColorScheme(eDotNetBarStyle.VS2005);
				this.BackColorSchemePart=eColorSchemePart.None;
				this.BackColor=SystemColors.ControlLight;
				this.BackColor2SchemePart=eColorSchemePart.None;
				this.BackColor2=Color.Empty;
				this.GripLightColorSchemePart=eColorSchemePart.MenuBackground;
				this.GripDarkColorSchemePart=eColorSchemePart.ItemPressedBorder;
				this.ExpandFillColorSchemePart=eColorSchemePart.ItemPressedBackground;
				this.ExpandLineColorSchemePart=eColorSchemePart.ItemPressedBorder;
				
				this.HotBackColorSchemePart=eColorSchemePart.ItemCheckedBackground;
				this.HotBackColor2=Color.Empty;
				this.HotBackColor2SchemePart=eColorSchemePart.None;
				this.HotGripLightColorSchemePart=eColorSchemePart.MenuBackground;
				this.HotGripDarkColorSchemePart=eColorSchemePart.ItemPressedBorder;
				this.HotExpandFillColorSchemePart=eColorSchemePart.ItemPressedBackground;
				this.HotExpandLineColorSchemePart=eColorSchemePart.ItemPressedBorder;
			}
		}
		#endregion

		#region IMessageHandlerClient Implementation
        
		bool IMessageHandlerClient.IsModal
		{
			get
			{
				Form form=this.FindForm();
				if(form!=null)
					return form.Modal;
				return false;
			}
		}
        bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }
		bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			// Check Shortcuts
			if(System.Windows.Forms.Control.ModifierKeys!=Keys.None || wParam.ToInt32()>=(int)eShortcut.F1 && wParam.ToInt32()<=(int)eShortcut.F12)
			{
				int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
				if(ProcessShortcut((eShortcut)i))
					return true;
			}
			return false;
		}
		private bool ProcessShortcut(eShortcut key)
		{
			if(key==m_Shortcut && this.Expandable)
			{
				SetExpanded(!this.Expanded,eEventSource.Keyboard);
			}
			return false;
		}
		bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			return false;
		}
		bool IMessageHandlerClient.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			return false;
		}
		bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			if(!this.DesignMode)
			{
				// Check Shortcuts
				if(System.Windows.Forms.Control.ModifierKeys!=Keys.None || wParam.ToInt32()>=(int)eShortcut.F1 && wParam.ToInt32()<=(int)eShortcut.F12)
				{
					int i=(int)System.Windows.Forms.Control.ModifierKeys | wParam.ToInt32();
					if(ProcessShortcut((eShortcut)i))
						return true;
				}
			}
			return false;
		}
		bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
		{
			return false;
		}
		private void InstallIMessageHandlerClient()
		{
			if(!m_FilterInstalled && !this.DesignMode && m_Shortcut!=eShortcut.None)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled=true;
			}
		}
		private void UninstallIMessageHandlerClient()
		{
			if(m_FilterInstalled)
			{
				MessageHandler.UnregisterMessageClient(this);
				m_FilterInstalled=false;
			}
		}
		#endregion
	}

	#region Enums
	/// <summary>
	/// Indicates the style of mutli-functional splitter control.
	/// </summary>
	public enum eSplitterStyle
	{
		/// <summary>
		/// Specifies Office 2003 like splitter style and color scheme.
		/// </summary>
		Office2003,
		/// <summary>
		/// Specifies Mozilla like splitter style and color scheme.
		/// </summary>
		Mozilla,
        /// <summary>
        /// Specifies Office 2007 like splitter style and color scheme.
        /// </summary>
        Office2007
	}
	#endregion

	#region Event Arguments and delegates
	public delegate void ExpandChangeEventHandler(object sender, ExpandedChangeEventArgs e);

	/// <summary>
	/// Represents event arguments for ExpandedChanging and ExpandedChanged events.
	/// </summary>
	public class ExpandedChangeEventArgs:EventArgs
	{
		/// <summary>
		/// Gets the action that caused the event, event source.
		/// </summary>
		public readonly eEventSource EventSource=eEventSource.Code;
		/// <summary>
		/// Gets or sets whether execution Expand event should be canceled. Applies only to ExpandedChanging event. Default is false.
		/// </summary>
		public bool Cancel=false;
		/// <summary>
		/// Indicates new value for the Expanded property.
		/// </summary>
		public readonly bool NewExpandedValue=false;

		public ExpandedChangeEventArgs(eEventSource action, bool newExpandedValue)
		{
			this.EventSource=action;
			this.NewExpandedValue=newExpandedValue;
		}
	}
	#endregion
}
