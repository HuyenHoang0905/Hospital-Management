using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represent an style for the item.
	/// </summary>
	[ToolboxItem(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class ItemStyle: ICloneable
	{
		const eBorderSide DEFAULT_BORDERSIDE=eBorderSide.Top | eBorderSide.Bottom | eBorderSide.Left | eBorderSide.Right;
		const int DEFAULT_CORNER_DIAMETER=8;
		private ColorEx m_BackColor1=ColorEx.Empty;
		private ColorEx m_BackColor2=ColorEx.Empty;
		private int m_GradientAngle=0;
		private ColorEx m_ForeColor=ColorEx.Empty;
        private Font m_Font=null;
		// Background Image support
		private Image m_BackgroundImage=null;
		private eBackgroundImagePosition m_BackgroundImagePosition=eBackgroundImagePosition.Stretch;
		private byte m_BackgroundImageAlpha=255;

		// Text Formating
		private bool m_WordWrap=false;
		private StringAlignment m_Alignment=StringAlignment.Near;
		private StringAlignment m_LineAlignment=StringAlignment.Center;
		private StringTrimming m_TextTrimming=StringTrimming.EllipsisCharacter;

		private eBorderType m_Border=eBorderType.None;
		private eBorderSide m_BorderSide=DEFAULT_BORDERSIDE;
		private int m_BorderWidth=1;
		private ColorEx m_BorderColor=ColorEx.Empty;
		private bool m_Custom=false;
		private DashStyle m_BorderDashStyle=DashStyle.Solid;

		private int m_MarginLeft=0, m_MarginRight=0,m_MarginTop=0,m_MarginBottom=0;

        private bool m_VerticalText = false;

		internal event EventHandler VisualPropertyChanged;

		// Corner type support
		private eCornerType m_CornerType=eCornerType.Square;
		private int m_CornerDiameter=DEFAULT_CORNER_DIAMETER;

		/// <summary>
		/// Default Constructor
		/// </summary>
		public ItemStyle()
		{
			m_BackColor1.ColorChanged+=new EventHandler(this.ColorChanged);
			m_BackColor2.ColorChanged+=new EventHandler(this.ColorChanged);
			m_ForeColor.ColorChanged+=new EventHandler(this.ColorChanged);
			m_BorderColor.ColorChanged+=new EventHandler(this.ColorChanged);
		}

		/// <summary>
		/// Makes a copy of the ItemStyle object.
		/// </summary>
		/// <returns>New Instance of a ItemStyle object</returns>
		public object Clone()
		{
			ItemStyle copy=new ItemStyle();
			copy.Alignment=this.Alignment;
			copy.BackColor1.SetColor(this.BackColor1.Color);
			copy.BackColor1.Alpha=this.BackColor1.Alpha;
			copy.BackColor1.SetColorSchemePart(this.BackColor1.ColorSchemePart);
			copy.BackColor2.SetColor(this.BackColor2.Color);
			copy.BackColor2.Alpha=this.BackColor2.Alpha;
			copy.BackColor2.SetColorSchemePart(this.BackColor2.ColorSchemePart);
			if(this.BackgroundImage!=null)
				copy.BackgroundImage=this.BackgroundImage.Clone() as System.Drawing.Image;
			copy.BackgroundImageAlpha=this.BackgroundImageAlpha;
			copy.BackgroundImagePosition=this.BackgroundImagePosition;
			copy.Border=this.Border;
			copy.BorderDashStyle=this.BorderDashStyle;
			copy.BorderColor.SetColor(this.BorderColor.Color);
			copy.BorderColor.Alpha=this.BorderColor.Alpha;
			copy.BorderColor.SetColorSchemePart(this.BorderColor.ColorSchemePart);
			copy.BorderWidth=this.BorderWidth;
			if(this.Font!=null)
				copy.Font=this.Font.Clone() as Font;
			copy.ForeColor.SetColor(this.ForeColor.Color);
			copy.ForeColor.Alpha=this.ForeColor.Alpha;
			copy.ForeColor.SetColorSchemePart(this.ForeColor.ColorSchemePart);
			copy.GradientAngle=this.GradientAngle;
			copy.LineAlignment=this.LineAlignment;
			copy.BorderSide=this.BorderSide;
			copy.TextTrimming=this.TextTrimming;
			copy.WordWrap=this.WordWrap;
			copy.Custom=this.Custom;
			copy.MarginBottom=this.MarginBottom;
			copy.MarginLeft=this.MarginLeft;
			copy.MarginRight=this.MarginRight;
			copy.MarginTop=this.MarginTop;
			copy.CornerType=this.CornerType;
			copy.CornerDiameter=this.CornerDiameter;
            copy.VerticalText = this.VerticalText;
            copy.UseMnemonic = _UseMnemonic;
			return copy;
		}

		internal void ApplyStyle(ItemStyle style)
		{
			if(style==null)
				return;

			m_Alignment=style.Alignment;
			if(!style.BackColor1.IsEmpty)
				m_BackColor1=style.BackColor1;
			if(!style.BackColor2.IsEmpty)
				m_BackColor2=style.BackColor2;
			if(style.BackgroundImage!=null)
                m_BackgroundImage=style.BackgroundImage;
			if(style.BackgroundImageAlpha!=255)
				m_BackgroundImageAlpha=style.BackgroundImageAlpha;
			if(style.BackgroundImagePosition!=eBackgroundImagePosition.Stretch)
				m_BackgroundImagePosition=style.BackgroundImagePosition;
			if(style.Border!=eBorderType.None)
				m_Border=style.Border;
			if(style.BorderDashStyle!=DashStyle.Solid)
				m_BorderDashStyle=style.BorderDashStyle;
			if(!style.BorderColor.IsEmpty)
				m_BorderColor=style.BorderColor;
			if(style.BorderSide!=DEFAULT_BORDERSIDE)
				m_BorderSide=style.BorderSide;
			if(style.BorderWidth!=1)
				m_BorderWidth=style.BorderWidth;
			if(style.Font!=null)
				m_Font=style.Font;
			if(!style.ForeColor.IsEmpty)
				m_ForeColor=style.ForeColor;
			if(style.LineAlignment!=StringAlignment.Center)
				m_LineAlignment=style.LineAlignment;
			if(style.TextTrimming!=StringTrimming.EllipsisCharacter)
				m_TextTrimming=style.TextTrimming;
			if(style.WordWrap!=false)
				m_WordWrap=style.WordWrap;
			if(style.MarginBottom!=0)
				m_MarginBottom=style.MarginBottom;
			if(style.MarginLeft!=0)
				m_MarginLeft=style.MarginLeft;
			if(style.MarginRight!=0)
				m_MarginRight=style.MarginRight;
			if(style.MarginTop!=0)
				m_MarginTop=style.MarginTop;
            if(style.CornerType!=eCornerType.Square)
				m_CornerType=style.CornerType;
			if(style.CornerDiameter!=DEFAULT_CORNER_DIAMETER)
				m_CornerDiameter=style.CornerDiameter;
            if (style.VerticalText)
                m_VerticalText = style.VerticalText;
            if (!style.UseMnemonic)
                _UseMnemonic = style.UseMnemonic;
		}

		internal void ApplyColorScheme(ColorScheme cs)
		{
            if (this.BackColor1.ColorSchemePart != eColorSchemePart.Custom && this.BackColor1.ColorSchemePart != eColorSchemePart.None)
				this.BackColor1.SetColor((Color)cs.GetType().GetProperty(this.BackColor1.ColorSchemePart.ToString()).GetValue(cs,null));
            if (this.BackColor2.ColorSchemePart != eColorSchemePart.Custom && this.BackColor2.ColorSchemePart != eColorSchemePart.None)
				this.BackColor2.SetColor((Color)cs.GetType().GetProperty(this.BackColor2.ColorSchemePart.ToString()).GetValue(cs,null));
            if (this.BorderColor.ColorSchemePart != eColorSchemePart.Custom && this.BorderColor.ColorSchemePart != eColorSchemePart.None)
                this.BorderColor.SetColor((Color)cs.GetType().GetProperty(this.BorderColor.ColorSchemePart.ToString()).GetValue(cs, null));
            if (this.ForeColor.ColorSchemePart != eColorSchemePart.Custom && this.ForeColor.ColorSchemePart != eColorSchemePart.None)
				this.ForeColor.SetColor((Color)cs.GetType().GetProperty(this.ForeColor.ColorSchemePart.ToString()).GetValue(cs,null));
		}

		private void ColorChanged(object sender, EventArgs e)
		{
			InvokeVisualPropertyChanged();
		}

		private void InvokeVisualPropertyChanged()
		{
			if(VisualPropertyChanged!=null)
				VisualPropertyChanged(this,new EventArgs());
		}

		internal bool Custom
		{
			get
			{
				return m_Custom | m_BackColor1.Custom | m_BackColor2.Custom | m_ForeColor.Custom;
			}
			set
			{
				m_Custom=value;
				m_BackColor1.Custom=value;
				m_BackColor2.Custom=value;
				m_ForeColor.Custom=value;
			}
		}

        /// <summary>
        /// Gets or sets whether text is drawn vertically by this style.
        /// </summary>
        [Browsable(false),DefaultValue(false)]
        public bool VerticalText
        {
            get { return m_VerticalText; }
            set
            {
                if (m_VerticalText != value)
                {
                    m_VerticalText = value;
                    InvokeVisualPropertyChanged();
                }
            }
        }
		
		/// <summary>
		/// Gets or sets a background color or starting color for gradient background.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets a background color or starting color for gradient background."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColorEx BackColor1
		{
			get
			{
				return m_BackColor1;
			}
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor1()
		{return !m_BackColor1.IsEmpty;}
		public void ResetBackColor1()
		{
			m_BackColor1.ColorChanged-=new EventHandler(this.ColorChanged);
			m_BackColor1=ColorEx.Empty;
			m_BackColor1.ColorChanged+=new EventHandler(this.ColorChanged);
		}
		internal void SetBackColor1(ColorEx c)
		{
			m_BackColor1=c;
		}

		/// <summary>
		/// Gets or sets a background color or ending color for gradient background.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets a background color or ending color for gradient background."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColorEx BackColor2
		{
			get
			{
				return m_BackColor2;
			}
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor2()
		{return !m_BackColor2.IsEmpty;}
		public void ResetBackColor2()
		{
			m_BackColor2.ColorChanged-=new EventHandler(this.ColorChanged);
			m_BackColor2=ColorEx.Empty;
			m_BackColor2.ColorChanged+=new EventHandler(this.ColorChanged);
		}
		internal void SetBackColor2(ColorEx c)
		{
			m_BackColor2=c;
		}

		/// <summary>
		/// Gets or sets a text color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets a text color."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColorEx ForeColor
		{
			get
			{
				return m_ForeColor;
			}
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool ShouldSerializeForeColor()
		{return !m_ForeColor.IsEmpty;}

		/// <summary>
		/// Gets or sets the gradient angle.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(0),Description("Gets or sets the gradient angle.")]
		public int GradientAngle
		{
			get
			{
				return m_GradientAngle;
			}
			set
			{
				if(m_GradientAngle!=value)
				{
					m_GradientAngle=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		internal System.Drawing.Region GetRegion(Rectangle clientRectangle)
		{
            // GetPath will reduce the size so account for that here
            clientRectangle.Width++;
            clientRectangle.Height++;

			GraphicsPath path=this.GetPath(clientRectangle);
			Region r=new Region();
			r.MakeEmpty();
			r.Union(path);
			// Widen path for the border...
			if(m_Border!=eBorderType.None && m_BorderWidth>0 && m_CornerType!=eCornerType.Square && !m_BorderColor.IsEmpty)
			{
				using(Pen pen=new Pen(Color.Black,m_BorderWidth))
				{
					path.Widen(pen);
				}
				Region r2=new Region(path);
				r.Union(path);
			}

			return r;
		}

		private GraphicsPath GetPath(Rectangle clientRectangle)
		{
			GraphicsPath path=new GraphicsPath();

			if(m_CornerType!=eCornerType.Square)
			{
				clientRectangle.Width--;
				clientRectangle.Height--;
			}

			switch(m_CornerType)
			{
				case eCornerType.Square:
				{
					path.AddRectangle(clientRectangle);
					break;
				}
				case eCornerType.Rounded:
				{
					path.AddLine(clientRectangle.X,clientRectangle.Bottom-m_CornerDiameter,clientRectangle.X,clientRectangle.Y+m_CornerDiameter);
					path.AddArc(clientRectangle.X,clientRectangle.Y,m_CornerDiameter*2,m_CornerDiameter*2,180,90);
					path.AddLine(clientRectangle.X+m_CornerDiameter,clientRectangle.Y,clientRectangle.Right-m_CornerDiameter,clientRectangle.Y);
					path.AddArc(clientRectangle.Right-m_CornerDiameter*2,clientRectangle.Y,m_CornerDiameter*2,m_CornerDiameter*2,270,90);
					path.AddLine(clientRectangle.Right,clientRectangle.Y+m_CornerDiameter,clientRectangle.Right,clientRectangle.Bottom-m_CornerDiameter);
					path.AddArc(clientRectangle.Right-m_CornerDiameter*2,clientRectangle.Bottom-m_CornerDiameter*2,m_CornerDiameter*2,m_CornerDiameter*2,0,90);
					path.AddLine(clientRectangle.Right-m_CornerDiameter,clientRectangle.Bottom,clientRectangle.X+m_CornerDiameter,clientRectangle.Bottom);
					path.AddArc(clientRectangle.X,clientRectangle.Bottom-m_CornerDiameter*2,m_CornerDiameter*2,m_CornerDiameter*2,90,90);
					path.CloseAllFigures();
					break;
				}
				case eCornerType.Diagonal:
				{
					path.AddLine(clientRectangle.X,clientRectangle.Bottom-m_CornerDiameter,clientRectangle.X,clientRectangle.Y+m_CornerDiameter);
					path.AddLine(clientRectangle.X+m_CornerDiameter,clientRectangle.Y,clientRectangle.Right-m_CornerDiameter,clientRectangle.Y);
					path.AddLine(clientRectangle.Right,clientRectangle.Y+m_CornerDiameter,clientRectangle.Right,clientRectangle.Bottom-m_CornerDiameter);
					path.AddLine(clientRectangle.Right-m_CornerDiameter,clientRectangle.Bottom,clientRectangle.X+m_CornerDiameter,clientRectangle.Bottom);
					path.CloseAllFigures();
					break;
				}
			}

			return path;
		}

		/// <summary>
		/// Gets or sets the style Font
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(null),Description("Gets or sets the style Font")]
		public System.Drawing.Font Font
		{
			get
			{
				return m_Font;
			}
			set
			{
				m_Font=value;
				m_Custom=true;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets a value that determines whether text is displayed in multiple lines or one long line.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(false),Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
		public bool WordWrap
		{
			get {return m_WordWrap;}
			set
			{
				if(m_WordWrap!=value)
					m_WordWrap=value;
				m_Custom=true;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Specifies alignment of the text.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),DefaultValue(StringAlignment.Near),Description("Specifies alignment of the text.")]
		public StringAlignment Alignment
		{
			get {return m_Alignment;}
			set
			{
				if(m_Alignment!=value)
				{
					m_Alignment=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Specifies alignment of the text.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(StringAlignment.Center),Description("Specifies alignment of the text.")]
		public StringAlignment LineAlignment
		{
			get {return m_LineAlignment;}
			set
			{
				if(m_LineAlignment!=value)
				{
					m_LineAlignment=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Specifies how to trim characters when text does not fit.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(StringTrimming.EllipsisCharacter),Description("Specifies how to trim characters when text does not fit.")]
		public StringTrimming TextTrimming
		{
			get {return m_TextTrimming;}
			set
			{
				if(m_TextTrimming!=value)
				{
					m_TextTrimming=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Specifies background image.
        /// </summary>
        [Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(null),Description("Specifies background image."),DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Image BackgroundImage
		{
			get {return m_BackgroundImage;}
			set
			{
				m_BackgroundImage=value;
				m_Custom=true;
				InvokeVisualPropertyChanged();
			}
		}
		public void ResetBackgroundImage()
		{
			m_BackgroundImage=null;
		}

		/// <summary>
		/// Specifies background image position when container is larger than image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(eBackgroundImagePosition.Stretch),Description("Specifies background image position when container is larger than image.")]
		public eBackgroundImagePosition BackgroundImagePosition
		{
			get {return m_BackgroundImagePosition;}
			set
			{
				if(m_BackgroundImagePosition!=value)
				{
					m_BackgroundImagePosition=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Specifies the transparency of background image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue((byte)255),Description("Specifies the transparency of background image.")]
		public byte BackgroundImageAlpha
		{
			get {return m_BackgroundImageAlpha;}
			set
			{
				if(m_BackgroundImageAlpha!=value)
				{
					m_BackgroundImageAlpha=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the corner type.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(eCornerType.Square),Description("Indicates corner type.")]		
		public eCornerType CornerType
		{
			get {return m_CornerType;}
			set
			{
				if(m_CornerType!=value)
				{
					m_CornerType=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the diameter in pixels of the corner type rounded or diagonal.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(DEFAULT_CORNER_DIAMETER),Description("Indicates diameter in pixels of the corner type rounded or diagonal.")]
		public int CornerDiameter
		{
			get {return m_CornerDiameter;}
			set
			{
				if(m_CornerDiameter!=value)
				{
					m_CornerDiameter=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border type.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(eBorderType.None),Description("Specifies the border type.")]
		public eBorderType Border
		{
			get {return m_Border;}
			set
			{
				if(m_Border!=value)
				{
					m_Border=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets dash style for single line border type.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(DashStyle.Solid),NotifyParentPropertyAttribute(true),Category("Style"),Description("Indicates dash style for single line border type.")]
		public DashStyle BorderDashStyle
		{
			get {return m_BorderDashStyle;}
			set
			{
				m_BorderDashStyle=value;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the border sides that are displayed.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(DEFAULT_BORDERSIDE),Description("Specifies border sides that are displayed.")]
		public eBorderSide BorderSide
		{
			get {return m_BorderSide;}
			set
			{
				if(m_BorderSide!=value)
				{
					m_BorderSide=value;
					m_Custom=true;
					InvokeVisualPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Specifies the border color."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColorEx BorderColor
		{
			get {return m_BorderColor;}
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderColor()
		{return !m_BorderColor.IsEmpty;}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void ResetBorderColor()
		{
			m_BorderColor.ColorChanged-=new EventHandler(this.ColorChanged);
			m_BorderColor=ColorEx.Empty;
			m_BorderColor.ColorChanged+=new EventHandler(this.ColorChanged);
		}

		/// <summary>
		/// Gets or sets the line tickness of single line border.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(1),Description("Specifies the line tickness of single line border.")]
		public int BorderWidth
		{
			get {return m_BorderWidth;}
			set 
			{
				m_BorderWidth=value;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the left text margin.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(0),Description("Specifies left text margin.")]
		public int MarginLeft
		{
			get {return m_MarginLeft;}
			set 
			{
				m_MarginLeft=value;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the right text margin.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(0),Description("Specifies right text margin.")]
		public int MarginRight
		{
			get {return m_MarginRight;}
			set 
			{
				m_MarginRight=value;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the top text margin.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(0),Description("Specifies top text margin.")]
		public int MarginTop
		{
			get {return m_MarginTop;}
			set 
			{
				m_MarginTop=value;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the bottom text margin.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),DefaultValue(0),Description("Specifies bottom text margin.")]
		public int MarginBottom
		{
			get {return m_MarginBottom;}
			set 
			{
				m_MarginBottom=value;
				InvokeVisualPropertyChanged();
			}
		}

		/// <summary>
		/// Paints the style.
		/// </summary>
		/// <param name="g">Graphics object</param>
		/// <param name="r">Target area</param>
		/// <param name="text">Text</param>
		/// <param name="textRect">Text area</param>
		/// <param name="font">Text Font</param>
		public void Paint(Graphics g, Rectangle r, string text, Rectangle textRect, System.Drawing.Font font)
		{
			this.Paint(g,r,text,textRect,font,null);
		}
		/// <summary>
		/// Paints the style
		/// </summary>
		/// <param name="g">Graphics object</param>
		/// <param name="r">Target Area</param>
		/// <param name="text">Text</param>
		/// <param name="textRect">Text area</param>
		/// <param name="font">Text Font</param>
		/// <param name="borderShape">Border Type</param>
		public void Paint(Graphics g, Rectangle r, string text, Rectangle textRect, System.Drawing.Font font, Point[] borderShape)
		{
			if(r.Width==0 || r.Height==0)
				return;

			Rectangle rPath=r;
			if(g.SmoothingMode==System.Drawing.Drawing2D.SmoothingMode.AntiAlias)
				rPath.Inflate(1,1);
            if (m_Border != eBorderType.None && !m_BorderColor.IsEmpty)
            {
                int borderReduction = 1;
                if (m_BorderWidth > 1)
                    borderReduction=m_BorderWidth / 2;
                if ((m_BorderSide & eBorderSide.Top) == eBorderSide.Top)
                {
                    rPath.Y += borderReduction;
                    rPath.Height -= borderReduction;
                }

                if ((m_BorderSide & eBorderSide.Bottom) == eBorderSide.Bottom)
                {
                    rPath.Height -= borderReduction;
                }

                if ((m_BorderSide & eBorderSide.Left) == eBorderSide.Left)
                {
                    rPath.X += borderReduction;
                    rPath.Width -= borderReduction;
                }

                if ((m_BorderSide & eBorderSide.Right) == eBorderSide.Right)
                {
                    rPath.Width -= borderReduction;
                }
            }
			GraphicsPath path=this.GetPath(rPath);

			if(!m_BackColor1.IsEmpty)
			{
				if(!m_BackColor2.IsEmpty)
				{
					Color clr1=Color.FromArgb(m_BackColor1.Alpha, m_BackColor1.Color);
					Color clr2=Color.FromArgb(m_BackColor2.Alpha, m_BackColor2.Color);;
					// Gradient Background fill
					Rectangle rGradient=r;
					rGradient.Inflate(1,1);
					LinearGradientBrush brush=BarFunctions.CreateLinearGradientBrush(rGradient,clr1,clr2,m_GradientAngle,false);
					//g.FillRectangle(brush,r);
					g.FillPath(brush,path);
					brush.Dispose();
				}
				else if(m_BackColor1.Color!=Color.Transparent)
				{
					Color clr=Color.FromArgb(m_BackColor1.Alpha, m_BackColor1.Color);
					SolidBrush brush=new SolidBrush(clr);
					//g.FillRectangle(brush,r);
					g.FillPath(brush,path);
					brush.Dispose();
				}
			}
            path.Dispose();
			path=this.GetPath(r);

			if(m_BackgroundImage!=null)
			{
				Region clip=g.Clip;
				g.SetClip(path);

				BarFunctions.PaintBackgroundImage(g,r,m_BackgroundImage,m_BackgroundImagePosition,m_BackgroundImageAlpha);
				g.Clip=clip;
                if (clip != null) clip.Dispose();
			}

			// Draw Border
			if(m_Border!=eBorderType.None && !m_BorderColor.IsEmpty)
			{
				if(m_CornerType!=eCornerType.Square)
				{
					if(m_BorderDashStyle==DashStyle.Solid)
						g.SmoothingMode=SmoothingMode.AntiAlias;
					using(Pen pen=new Pen(m_BorderColor.GetCompositeColor(),m_BorderWidth))
					{
						pen.Alignment=PenAlignment.Inset;
						pen.DashStyle=m_BorderDashStyle;						
						g.DrawPath(pen,path);
					}
					if(m_BorderDashStyle==DashStyle.Solid)
						g.SmoothingMode=SmoothingMode.Default;
				}
				else if(m_Border==eBorderType.SingleLine && borderShape!=null && borderShape.Length>0)
				{
					using(Pen pen=new Pen(m_BorderColor.GetCompositeColor(),m_BorderWidth))
					{
						pen.DashStyle=m_BorderDashStyle;
						g.DrawLines(pen,borderShape);
					}
				}
				else
				{
					switch(m_Border)
					{
						case eBorderType.Bump:
						case eBorderType.Etched:
						case eBorderType.DoubleLine:
						case eBorderType.SingleLine:
							BarFunctions.DrawBorder(g,m_Border,r,(m_BorderColor.IsEmpty?SystemColors.ControlText:Color.FromArgb(m_BorderColor.Alpha,m_BorderColor.Color)),m_BorderSide,m_BorderDashStyle,m_BorderWidth);
							break;
						case eBorderType.Raised:
						{
							System.Windows.Forms.Border3DSide border3dside;
							if(m_BorderSide==eBorderSide.All)
								border3dside=System.Windows.Forms.Border3DSide.All;
							else
								border3dside=(((m_BorderSide&eBorderSide.Left)!=0)?System.Windows.Forms.Border3DSide.Left:0) |
								(((m_BorderSide&eBorderSide.Right)!=0)?System.Windows.Forms.Border3DSide.Right:0) |
								(((m_BorderSide&eBorderSide.Top)!=0)?System.Windows.Forms.Border3DSide.Top:0) | 
								(((m_BorderSide&eBorderSide.Bottom)!=0)?System.Windows.Forms.Border3DSide.Bottom:0);
                            BarFunctions.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.RaisedInner, border3dside, (m_BorderColor.IsEmpty ? m_BackColor1.Color : Color.FromArgb(m_BorderColor.Alpha, m_BorderColor.Color)), false);
							break;
						}
                        case eBorderType.RaisedInner:
                        {
                            using (Pen pen = new Pen(Color.White))
                            {
                                if ((m_BorderSide & eBorderSide.Top) == eBorderSide.Top)
                                    g.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
                                if ((m_BorderSide & eBorderSide.Left) == eBorderSide.Left)
                                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                            }

                            using (Pen pen = new Pen(m_BorderColor.GetCompositeColor()))
                            {
                                if ((m_BorderSide & eBorderSide.Right) == eBorderSide.Right)
                                    g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
                                if ((m_BorderSide & eBorderSide.Bottom) == eBorderSide.Bottom)
                                    g.DrawLine(pen, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
                            }
                            break;
                        }
						case eBorderType.Sunken:
						{
							System.Windows.Forms.Border3DSide border3dside;
							if(m_BorderSide==eBorderSide.All)
								border3dside=System.Windows.Forms.Border3DSide.All;
							else
								border3dside=(((m_BorderSide&eBorderSide.Left)!=0)?System.Windows.Forms.Border3DSide.Left:0) |
								(((m_BorderSide&eBorderSide.Right)!=0)?System.Windows.Forms.Border3DSide.Right:0) |
								(((m_BorderSide&eBorderSide.Top)!=0)?System.Windows.Forms.Border3DSide.Top:0) | 
								(((m_BorderSide&eBorderSide.Bottom)!=0)?System.Windows.Forms.Border3DSide.Bottom:0);
							BarFunctions.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.SunkenOuter,border3dside,(m_BorderColor.IsEmpty?m_BackColor1.Color:Color.FromArgb(m_BorderColor.Alpha,m_BorderColor.Color)),false);
							break;
						}
					}
				}
			}

			if(text!="" && !m_ForeColor.IsEmpty)
			{
				if(m_Font!=null)
					font=m_Font;
				Color clr=Color.FromArgb(m_ForeColor.Alpha, m_ForeColor.Color);
				
				if(m_MarginLeft!=0)
				{
					textRect.X+=m_MarginLeft;
					textRect.Width-=m_MarginLeft;
				}
				if(m_MarginRight!=0)
				{
					textRect.Width-=m_MarginRight;
				}
				if(m_MarginTop!=0)
				{
					textRect.Y+=m_MarginTop;
					textRect.Height-=m_MarginTop;
				}
				if(m_MarginBottom!=0)
					textRect.Height-=m_MarginBottom;

                if (textRect.Width > 0 && textRect.Height > 0)
                {
                    if (m_VerticalText)
                    {
                        g.RotateTransform(-90);
                        TextDrawing.DrawStringLegacy(g, text, font, clr, new Rectangle(-textRect.Bottom, textRect.X, textRect.Height, textRect.Width), this.GetTextFormat());
                        g.ResetTransform();
                    }
                    else
                        TextDrawing.DrawString(g, text, font, clr, textRect, this.GetTextFormat());
                }
			}
            if (path != null) path.Dispose();
			if(m_CornerType!=eCornerType.Square)
				g.ResetClip();
		}
		/// <summary>
		/// Paints the style
		/// </summary>
		/// <param name="g">Graphics object</param>
		/// <param name="r">Target Area</param>
		public void Paint(Graphics g, Rectangle r)
		{
			this.Paint(g,r,"",Rectangle.Empty,System.Windows.Forms.Control.DefaultFont);
		}
		/// <summary>
		/// Paints the style text only.
		/// </summary>
		/// <param name="g">Graphics object</param>
		/// <param name="text">Text</param>
		/// <param name="textRect">Text area</param>
		/// <param name="font">Font</param>
		public void PaintText(Graphics g, string text, Rectangle textRect, System.Drawing.Font font)
		{
			if(text!="")
			{
				if(m_Font!=null)
					font=m_Font;
				Color clr=Color.FromArgb(m_ForeColor.Alpha, m_ForeColor.Color);

				if(m_MarginLeft!=0)
				{
					textRect.X+=m_MarginLeft;
					textRect.Width-=m_MarginLeft;
				}
				if(m_MarginRight!=0)
				{
					textRect.Width-=m_MarginRight;
				}
				if(m_MarginTop!=0)
				{
					textRect.Y+=m_MarginTop;
					textRect.Height-=m_MarginTop;
				}
				if(m_MarginBottom!=0)
					textRect.Height-=m_MarginBottom;

                if (textRect.Width > 0 && textRect.Height > 0)
                {
                    if (m_VerticalText)
                    {
                        g.RotateTransform(-90);
                        TextDrawing.DrawStringLegacy(g, text, font, clr, new Rectangle(-textRect.Bottom, textRect.X, textRect.Height, textRect.Width), this.GetTextFormat());
                        g.ResetTransform();
                    }
                    else
                        TextDrawing.DrawString(g, text, font, clr, textRect, this.GetTextFormat());
                }
			}
		}

		internal System.Drawing.StringFormat StringFormat
		{
			get
			{
				System.Drawing.StringFormat format=BarFunctions.CreateStringFormat(); // new System.Drawing.StringFormat(StringFormat.GenericTypographic);
				format.Alignment=m_Alignment;
				format.LineAlignment=m_LineAlignment;
				if(m_WordWrap)
					format.FormatFlags=format.FormatFlags & ~(format.FormatFlags & StringFormatFlags.NoWrap);
				else
					format.FormatFlags=format.FormatFlags | StringFormatFlags.NoWrap;
				format.Trimming=m_TextTrimming;
				format.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
				return format;
			}
		}

        internal eTextFormat GetTextFormat()
        {
            eTextFormat format = eTextFormat.Default;
            if (m_Alignment == StringAlignment.Center)
                format |= eTextFormat.HorizontalCenter;
            else if (m_Alignment == StringAlignment.Far)
                format |= eTextFormat.Right;
            if (m_LineAlignment == StringAlignment.Center)
                format |= eTextFormat.VerticalCenter;
            else if (m_LineAlignment == StringAlignment.Far)
                format |= eTextFormat.Bottom;
            if (!m_WordWrap)
                format |= eTextFormat.SingleLine;
            else
                format |= eTextFormat.WordBreak;
            if (m_TextTrimming != StringTrimming.None)
                format |= eTextFormat.EndEllipsis;
            if(!_UseMnemonic)
                format |= eTextFormat.NoPrefix;
            return format;
        }

        private bool _UseMnemonic = true;
        /// <summary>
        /// Gets or sets a value indicating whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
        public bool UseMnemonic
        {
            get { return _UseMnemonic; }
            set
            {
                _UseMnemonic = value;
            }
        }

		internal ThemeTextFormat ThemeStringFormat
		{
			get
			{
				ThemeTextFormat format=ThemeTextFormat.HidePrefix;
				switch(m_Alignment)
				{
					case StringAlignment.Center:
						format=format | ThemeTextFormat.Center;
						break;
					case StringAlignment.Near:
						format=format | (System.Windows.Forms.SystemInformation.RightAlignedMenus?ThemeTextFormat.Right:ThemeTextFormat.Left);
						break;
					case StringAlignment.Far:
						format=format | (System.Windows.Forms.SystemInformation.RightAlignedMenus?ThemeTextFormat.Left:ThemeTextFormat.Right);
						break;
				}
				switch(m_LineAlignment)
				{
					case StringAlignment.Center:
						format=format | (m_WordWrap?ThemeTextFormat.Bottom:ThemeTextFormat.VCenter);
						break;
					case StringAlignment.Near:
						format=format | ThemeTextFormat.Top;
						break;
					case StringAlignment.Far:
						format=format | ThemeTextFormat.Bottom;
						break;
				}
				if(m_WordWrap)
					format=format | ThemeTextFormat.WordBreak;
				else
					format=format | ThemeTextFormat.SingleLine;
				switch(m_TextTrimming)
				{
					case StringTrimming.Character:
						format=format | ThemeTextFormat.NoFullWidthCharBreak;
						break;
					case StringTrimming.EllipsisCharacter:
						format=format | ThemeTextFormat.EndEllipsis;
						break;
					case StringTrimming.EllipsisPath:
						format=format | ThemeTextFormat.PathElliosis;
						break;
					case StringTrimming.EllipsisWord:
						format=format | ThemeTextFormat.WordEllipsis;
						break;
					case StringTrimming.Word:
						format=format | ThemeTextFormat.WordEllipsis;
						break;
				}
				return format;
			}
		}
		internal void Serialize(System.Xml.XmlElement styleElement)
		{
			if(!m_BackColor1.IsEmpty)
			{
				if(m_BackColor1.ColorSchemePart==eColorSchemePart.Custom)
				{
					styleElement.SetAttribute("bc1",BarFunctions.ColorToString(m_BackColor1.Color));
					styleElement.SetAttribute("bc1a",m_BackColor1.Alpha.ToString());
				}
				else
					styleElement.SetAttribute("bc1csp",m_BackColor1.ColorSchemePart.ToString());
			}
			if(!m_BackColor2.IsEmpty)
			{
				if(m_BackColor2.ColorSchemePart==eColorSchemePart.Custom)
				{
					styleElement.SetAttribute("bc2",BarFunctions.ColorToString(m_BackColor2.Color));
					styleElement.SetAttribute("bc2a",m_BackColor2.Alpha.ToString());
				}
				else
					styleElement.SetAttribute("bc2csp",m_BackColor2.ColorSchemePart.ToString());
			}
			if(!m_ForeColor.IsEmpty)
			{
				if(m_ForeColor.ColorSchemePart==eColorSchemePart.Custom)
				{
					styleElement.SetAttribute("fc",BarFunctions.ColorToString(m_ForeColor.Color));
					styleElement.SetAttribute("fca",m_ForeColor.Alpha.ToString());
				}
				else
					styleElement.SetAttribute("fccsp",m_ForeColor.ColorSchemePart.ToString());
			}
			if(!m_BorderColor.IsEmpty)
			{
				if(m_BorderColor.ColorSchemePart==eColorSchemePart.Custom)
				{
					styleElement.SetAttribute("borderc",BarFunctions.ColorToString(m_BorderColor.Color));
					styleElement.SetAttribute("borderca",m_BorderColor.Alpha.ToString());
				}
				else
					styleElement.SetAttribute("bordercsp",m_BorderColor.ColorSchemePart.ToString());
			}
			styleElement.SetAttribute("ga",m_GradientAngle.ToString());
			if(m_Font!=null)
			{
				styleElement.SetAttribute("fontname",m_Font.Name);
				styleElement.SetAttribute("fontemsize",System.Xml.XmlConvert.ToString(m_Font.Size));
				styleElement.SetAttribute("fontstyle",System.Xml.XmlConvert.ToString((int)m_Font.Style));
			}
			if(m_BackgroundImage!=null)
			{
				System.Xml.XmlElement elementImage=styleElement.OwnerDocument.CreateElement("backimage");
				styleElement.AppendChild(elementImage);
				BarFunctions.SerializeImage(m_BackgroundImage,elementImage);
				elementImage.SetAttribute("pos",((int)m_BackgroundImagePosition).ToString());
				elementImage.SetAttribute("alpha",m_BackgroundImageAlpha.ToString());
			}
			if(m_WordWrap)
				styleElement.SetAttribute("wordwrap",System.Xml.XmlConvert.ToString(m_WordWrap));
			if(m_Alignment!=StringAlignment.Near)
				styleElement.SetAttribute("align",System.Xml.XmlConvert.ToString((int)m_Alignment));
			if(m_LineAlignment!=StringAlignment.Center)
				styleElement.SetAttribute("valign",System.Xml.XmlConvert.ToString((int)m_LineAlignment));
			if(m_TextTrimming!=StringTrimming.EllipsisCharacter)
				styleElement.SetAttribute("trim",System.Xml.XmlConvert.ToString((int)m_TextTrimming));
            if(m_Border!=eBorderType.None)
				styleElement.SetAttribute("border",System.Xml.XmlConvert.ToString((int)m_Border));
			if(m_BorderWidth!=1)
				styleElement.SetAttribute("borderw",m_BorderWidth.ToString());

			if(m_CornerDiameter!=DEFAULT_CORNER_DIAMETER)
				styleElement.SetAttribute("cornerdiameter",m_CornerDiameter.ToString());
			if(m_CornerType!=eCornerType.Square)
				styleElement.SetAttribute("cornertype",System.Xml.XmlConvert.ToString((int)m_CornerType));
		}
		internal void Deserialize(System.Xml.XmlElement styleElement)
		{
			m_BackColor1=ColorEx.Empty;
			if(styleElement.HasAttribute("bc1"))
			{
				m_BackColor1.Color=BarFunctions.ColorFromString(styleElement.GetAttribute("bc1"));
				m_BackColor1.Alpha=System.Xml.XmlConvert.ToByte(styleElement.GetAttribute("bc1a"));
			}
			else if(styleElement.HasAttribute("bc1csp"))
				m_BackColor1.ColorSchemePart=(eColorSchemePart)Enum.Parse(typeof(eColorSchemePart),styleElement.GetAttribute("bc1csp"));
			m_BackColor2=ColorEx.Empty;
			if(styleElement.HasAttribute("bc2"))
			{
				m_BackColor2.Color=BarFunctions.ColorFromString(styleElement.GetAttribute("bc2"));
				m_BackColor2.Alpha=System.Xml.XmlConvert.ToByte(styleElement.GetAttribute("bc2a"));
			}
			else if(styleElement.HasAttribute("bc2csp"))
				m_BackColor2.ColorSchemePart=(eColorSchemePart)Enum.Parse(typeof(eColorSchemePart),styleElement.GetAttribute("bc2csp"));
			m_ForeColor=ColorEx.Empty;
			if(styleElement.HasAttribute("fc"))
			{
				m_ForeColor.Color=BarFunctions.ColorFromString(styleElement.GetAttribute("fc"));
				m_ForeColor.Alpha=System.Xml.XmlConvert.ToByte(styleElement.GetAttribute("fca"));
			}
			else if(styleElement.HasAttribute("fccsp"))
				m_ForeColor.ColorSchemePart=(eColorSchemePart)Enum.Parse(typeof(eColorSchemePart),styleElement.GetAttribute("fccsp"));

			m_BorderColor=ColorEx.Empty;
			if(styleElement.HasAttribute("borderc"))
			{
				m_BorderColor.Color=BarFunctions.ColorFromString(styleElement.GetAttribute("borderc"));
				m_BorderColor.Alpha=System.Xml.XmlConvert.ToByte(styleElement.GetAttribute("borderca"));
			}
			else if(styleElement.HasAttribute("bordercsp"))
				m_BorderColor.ColorSchemePart=(eColorSchemePart)Enum.Parse(typeof(eColorSchemePart),styleElement.GetAttribute("bordercsp"));

			m_GradientAngle=System.Xml.XmlConvert.ToInt16(styleElement.GetAttribute("ga"));

			// Load font information if it exists
			m_Font=null;
			if(styleElement.HasAttribute("fontname"))
			{
				string FontName=styleElement.GetAttribute("fontname");
				float FontSize=System.Xml.XmlConvert.ToSingle(styleElement.GetAttribute("fontemsize"));
				System.Drawing.FontStyle FontStyle=(System.Drawing.FontStyle)System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("fontstyle"));
				try
				{
					m_Font=new Font(FontName,FontSize,FontStyle);
				}
				catch(Exception)
				{
					m_Font=null;
				}
			}
			// Load Image
			m_BackgroundImage=null;
			foreach(System.Xml.XmlElement xmlElem in styleElement.ChildNodes)
			{
				if(xmlElem.Name=="backimage")
				{
					m_BackgroundImage=BarFunctions.DeserializeImage(xmlElem);
					m_BackgroundImagePosition=(eBackgroundImagePosition)System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("pos"));
					m_BackgroundImageAlpha=System.Xml.XmlConvert.ToByte(xmlElem.GetAttribute("alpha"));
				}
			}

			m_WordWrap=false;
			if(styleElement.HasAttribute("wordwrap"))
				m_WordWrap=System.Xml.XmlConvert.ToBoolean(styleElement.GetAttribute("wordwrap"));
            
			m_Alignment=StringAlignment.Near;
			if(styleElement.HasAttribute("align"))
				m_Alignment=(StringAlignment)System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("align"));
			m_LineAlignment=StringAlignment.Center;
			if(styleElement.HasAttribute("valign"))
				m_LineAlignment=(StringAlignment)System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("valign"));
			m_TextTrimming=StringTrimming.EllipsisCharacter;
			if(styleElement.HasAttribute("trim"))
				m_TextTrimming=(StringTrimming)System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("trim"));

			m_Border=eBorderType.None;
			if(styleElement.HasAttribute("border"))
				m_Border=(eBorderType)System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("border"));
	
			m_BorderWidth=1;
            if(styleElement.HasAttribute("borderw"))
				m_BorderWidth=System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("borderw"));

			m_CornerType=eCornerType.Square;
			if(styleElement.HasAttribute("cornertype"))
				m_CornerType=(eCornerType)System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("cornertype"));
			m_CornerDiameter=DEFAULT_CORNER_DIAMETER;
			if(styleElement.HasAttribute("cornerdiameter"))
				m_CornerDiameter=System.Xml.XmlConvert.ToInt32(styleElement.GetAttribute("cornerdiameter"));
		}

	}

	/// <summary>
	/// ColorEx object that provides the transparency setting ability.
	/// </summary>
	[ToolboxItem(false),TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter)),EditorBrowsable(EditorBrowsableState.Never)]
	public class ColorEx
	{
		private System.Drawing.Color _Color=System.Drawing.Color.Empty;
		private byte _Alpha=255;
		private eColorSchemePart m_ColorSchemePart=eColorSchemePart.Custom;
		internal bool Custom=false;
		internal event EventHandler ColorChanged;
		public ColorEx()
		{
		}

		/// <summary>
		/// Constructor with Color Initialization.
		/// </summary>
		/// <param name="color">Color object</param>
		ColorEx(System.Drawing.Color color)
		{
			this.Color=color;
			this.Alpha=255;
		}

		/// <summary>
		/// Constructor with Color and Transparency Initialization.
		/// </summary>
		/// <param name="color">Color object</param>
		/// <param name="opacity">Transparency</param>
		ColorEx(System.Drawing.Color color, byte opacity)
		{
			this.Color=color;
			this.Alpha=opacity;
		}

		/// <summary>
		/// Gets or sets the Color object which does not include transparency.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true)]
		public System.Drawing.Color Color
		{
			get {return _Color;}
			set {
				_Color=value;
				if(_Color!=Color.Empty)
				{
					m_ColorSchemePart=eColorSchemePart.Custom;
					Custom=true;
				}
				if(ColorChanged!=null)
					ColorChanged(this,new EventArgs());
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never),Browsable(false)]
		public bool ShouldSerializeColor()
		{
			return (_Color!=Color.Empty && m_ColorSchemePart==eColorSchemePart.Custom);
		}
		internal void SetColor(Color c)
		{
			_Color=c;
		}

		/// <summary>
		/// Indicates the transparency for the color.
		/// </summary>
		[DefaultValue((byte)255),NotifyParentPropertyAttribute(true),Browsable(true),DevCoBrowsable(true)]
		public byte Alpha
		{
			get {return _Alpha;}
			set {
				if(_Alpha!=value)
				{
					_Alpha=value;
					Custom=true;
				}
				if(ColorChanged!=null)
					ColorChanged(this,new EventArgs());
			}
		}

		/// <summary>
		///     Specifies that color derived from system colors which is part of DotNetBar Color Scheme object is used.
		///     Colors derived from system colors are automatically refreshed when
		///     system colors are changed.
		/// </summary>
		/// <remarks>
		///     We recommend using this property to specify color rather than setting color directly.
		///     Using colors that are derived from system colors improves uniform look of your application
		///     and visual integration into user environment.
		/// </remarks>
		[DefaultValue(eColorSchemePart.Custom),NotifyParentPropertyAttribute(true),Browsable(true),DevCoBrowsable(true)]
		public eColorSchemePart ColorSchemePart
		{
			get {return m_ColorSchemePart;}
			set
			{
				m_ColorSchemePart=value;
				if(m_ColorSchemePart!=eColorSchemePart.Custom)
				{
					_Color=Color.Empty;
					Custom=true;
				}
				if(ColorChanged!=null)
					ColorChanged(this,new EventArgs());
			}
		}

		internal void SetColorSchemePart(eColorSchemePart p)
		{
			m_ColorSchemePart=p;
		}

		/// <summary>
		/// Returns empty ColorEx object.
		/// </summary>
		public static ColorEx Empty
		{
			get{return new ColorEx(Color.Empty);}
		}

		/// <summary>
		/// Indicates whether object contain any color.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public bool IsEmpty
		{
			get
			{
				return (_Color==Color.Empty);
			}
		}
		/// <summary>
		/// Returns the color object with the transparency set.
		/// </summary>
		/// <returns>Color object</returns>
		public Color GetCompositeColor()
		{
			if(_Color.IsEmpty)
				return Color.Empty;
			else
				return Color.FromArgb(_Alpha,_Color);
		}
	}

//	public class ItemStyleTypeConverter:TypeConverter 
//	{
//		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) 
//		{
//			if (destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor)) 
//				return true;
//			return base.CanConvertTo(context, destinationType);
//		}
//
//		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) 
//		{
//			// other TypeConverter operations go here...
//			//
//			if (destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) && value is ItemStyle) 
//			{
//				System.Reflection.ConstructorInfo ctor = typeof(ItemStyle).GetConstructor(Type.EmptyTypes);
//				if(ctor != null) 
//				{
//					return new System.ComponentModel.Design.Serialization.InstanceDescriptor(ctor,null);
//				}
//			}
//			return base.ConvertTo(context, culture, value, destinationType);      
//		}
//
////		public override object CreateInstance(ITypeDescriptorContext ctx, System.Collections.IDictionary d)
////		{
////			return new ItemStyle();
////		}
//
//		
////		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) 
////		{
////			return true;
////		}
//
//		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, System.Attribute[] attributes) 
//		{
//			PropertyDescriptorCollection propDesc;
//			string[] prop;
//
//			propDesc = TypeDescriptor.GetProperties(typeof(ItemStyle), attributes);
//
//			prop = new String[16];
//			prop[0] = "Alignment";
//			prop[1] = "BackColor1";
//			prop[2] = "BackColor2";
//			prop[3] = "BackgroundImage";
//			prop[4] = "BackgroundImageAlpha";
//			prop[5] = "BackgroundImagePosition";
//			prop[6] = "Border";
//			prop[7] = "BorderColor";
//			prop[8] = "BorderSide";
//			prop[9] = "BorderWidth";
//			prop[10] = "Font";
//			prop[11] = "ForeColor";
//			prop[12] = "GradientAngle";
//			prop[13] = "LineAlignment";
//			prop[14] = "TextTrimming";
//			prop[15] = "WordWrap";
//			return propDesc.Sort(prop);
//		}
//
//		public override bool GetPropertiesSupported(ITypeDescriptorContext context) 
//		{
//			return true;
//		}
//	}
//
//	public class ColorExTypeConverter:TypeConverter 
//	{
//		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) 
//		{
//			if (destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor)) 
//				return true;
//			return base.CanConvertTo(context, destinationType);
//		}
//
//		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) 
//		{
//			// other TypeConverter operations go here...
//			//
//			if (destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) && value is ColorEx) 
//			{
//				System.Reflection.ConstructorInfo ctor = typeof(ColorEx).GetConstructor(Type.EmptyTypes);
//				if(ctor != null) 
//				{
//					return new System.ComponentModel.Design.Serialization.InstanceDescriptor(ctor,null);
//				}
//			}
//			return base.ConvertTo(context, culture, value, destinationType);      
//		}
//
////		public override object CreateInstance(ITypeDescriptorContext ctx, System.Collections.IDictionary d)
////		{
////			return new ColorEx();
////		}
////
////		
////		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) 
////		{
////			return true;
////		}
//
//		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, System.Attribute[] attributes) 
//		{
//			PropertyDescriptorCollection propDesc;
//			string[] prop;
//
//			propDesc = TypeDescriptor.GetProperties(typeof(ColorEx), attributes);
//
//			prop = new String[2];
//			prop[0] = "Color";
//			prop[1] = "Alpha";
//			return propDesc.Sort(prop);
//		}
//
//		public override bool GetPropertiesSupported(ITypeDescriptorContext context) 
//		{
//			return true;
//		}
//	}
}
